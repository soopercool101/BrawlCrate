using BrawlLib.Internal;
using BrawlLib.Internal.Drawing.Imaging;
using BrawlLib.Internal.IO;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace BrawlLib.Imaging
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct TGAHeader
    {
        public const int Size = 18;

        public byte idLength;
        public byte colorMapType;
        public TGAImageType imageType;
        public TGAColorMapSpecification colorMapSpecification;
        public TGAImageSpecification imageSpecification;

        private VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public byte* ImageId => (byte*) (Address + Size);
        public byte* ColorMapData => ImageId + idLength;
        public byte* ImageData => ColorMapData + colorMapSpecification.DataLength;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TGAColorMapSpecification
    {
        public ushort firstEntryIndex;
        public ushort length;
        public byte entrySize;

        public int DataLength => (((int) entrySize).Align(4) * length).Align(8) >> 3;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TGAImageSpecification
    {
        public ushort xOrigin;
        public ushort yOrigin;
        public ushort width;
        public ushort height;
        public byte pixelDepth;
        public byte imageDescriptor;

        public byte AlphaBits
        {
            get => (byte) (imageDescriptor & 0xF);
            set => imageDescriptor = (byte) ((value & 0xF) | (imageDescriptor & 0x30));
        }

        public TGAOrigin ImageOrigin
        {
            get => (TGAOrigin) ((imageDescriptor >> 4) & 0x3);
            set => imageDescriptor = (byte) (((byte) value << 4) | (imageDescriptor & 0xF));
        }

        public int DataLength => ((((int) pixelDepth).Align(4) * width).Align(8) >> 3) * height;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct TGAFooter
    {
        public const int Size = 26;
        public const string Signature = "TRUEVISION-XFILE.";

        public uint extensionAreaOffset;
        public uint developerDirectoryOffset;
        private fixed byte signature[18]; //"TRUEVISION-XFILE.\0"

        public TGAFooter(uint extOffset, uint devOffset)
        {
            extensionAreaOffset = extOffset;
            developerDirectoryOffset = devOffset;
            fixed (byte* ptr = signature)
            {
                for (int i = 0; i < Signature.Length; i++)
                {
                    ptr[i] = (byte) Signature[i];
                }

                ptr[18] = 0;
            }
        }
    }

    internal enum TGAOrigin : byte
    {
        BottomLeft = 0,
        BottomRight = 1,
        TopLeft = 2,
        TopRight = 3
    }

    internal enum TGAImageType : byte
    {
        None = 0,
        UncompressedColorMapped = 1,
        UncompressedTrueColor = 2,
        UncompressedGreyscale = 3,
        RLEColorMapped = 9,
        RLETrueColor = 10,
        RLEGreyscale = 11
    }

    public static class TGA
    {
        private delegate Color PaletteParser(ref VoidPtr data);

        private delegate void ColorParser(VoidPtr sPtr, int sIndex, VoidPtr dPtr, int dIndex);


        private delegate void PaletteEncoder(ref VoidPtr data, Color c);

        private delegate void ColorEncoder(VoidPtr sPtr, int sIndex, Stream dstStream);

        private static unsafe int DecodeRLE(byte* sPtr, byte* dPtr, int stride, int entryBits)
        {
            int sLen = 0, pControl, pCount;
            int entryLen = entryBits / 8;
            byte* ceil = dPtr + stride;

            while (dPtr < ceil)
            {
                pControl = *sPtr++;
                sLen++;
                pCount = (pControl & 0x7F) + 1;

                //Is run-length packet?
                if ((pControl & 0x80) != 0)
                {
                    for (int x = 0; x < pCount; x++)
                    {
                        for (int y = 0; y < entryLen; y++)
                        {
                            *dPtr++ = sPtr[y];
                        }
                    }

                    sLen += entryLen;
                    sPtr += entryLen;
                }
                else
                {
                    for (int x = 0; x < pCount; x++)
                    {
                        for (int y = 0; y < entryLen; y++, sLen++)
                        {
                            *dPtr++ = *sPtr++;
                        }
                    }
                }
            }

            return sLen;
        }

        public static unsafe Bitmap FromFile(string path)
        {
            using (FileMap view = FileMap.FromFile(path, FileMapProtect.Read)
            ) // FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                TGAHeader* header = (TGAHeader*) view.Address;

                int w = header->imageSpecification.width, h = header->imageSpecification.height;
                int entryBpp = header->imageSpecification.pixelDepth;
                int alphaBits = header->imageSpecification.AlphaBits;

                ColorPalette palette = null;
                PixelFormat format;
                ColorParser cParser;
                switch (header->imageType & (TGAImageType) 0x3)
                {
                    case TGAImageType.UncompressedColorMapped:
                    {
                        int mapBpp = header->colorMapSpecification.entrySize;
                        if (entryBpp == 4)
                        {
                            format = PixelFormat.Format4bppIndexed;
                            cParser = delegate(VoidPtr sPtr, int sIndex, VoidPtr dPtr, int dIndex)
                            {
                                byte val = ((byte*) sPtr)[sIndex >> 1], val2 = ((byte*) dPtr)[dIndex >> 1];
                                val = (sIndex & 1) == 0 ? (byte) (val >> 4) : (byte) (val & 0xF);
                                ((byte*) dPtr)[dIndex >> 1] = (dIndex & 1) == 0
                                    ? (byte) ((val2 & 0xF) | (val << 4))
                                    : (byte) ((val2 & 0xF0) | val);
                            };
                        }
                        else if (entryBpp == 8)
                        {
                            format = PixelFormat.Format8bppIndexed;
                            cParser = delegate(VoidPtr sPtr, int sIndex, VoidPtr dPtr, int dIndex)
                            {
                                ((byte*) dPtr)[dIndex] = ((byte*) sPtr)[sIndex];
                            };
                        }
                        else
                        {
                            throw new InvalidDataException("Invalid TGA color map format.");
                        }

                        int firstIndex = header->colorMapSpecification.firstEntryIndex;
                        int palSize = firstIndex + header->colorMapSpecification.length;
                        palette = ColorPaletteExtension.CreatePalette(ColorPaletteFlags.None, palSize);

                        PaletteParser pParser;
                        if (mapBpp == 32)
                        {
                            pParser = (ref VoidPtr x) =>
                            {
                                Color c = (Color) (*(ARGBPixel*) x);
                                x += 4;
                                return c;
                            };
                        }
                        else if (mapBpp == 24)
                        {
                            pParser = (ref VoidPtr x) =>
                            {
                                Color c = (Color) (*(RGBPixel*) x);
                                x += 3;
                                return c;
                            };
                        }
                        else
                        {
                            throw new InvalidDataException("Invalid TGA color map format.");
                        }

                        VoidPtr palData = header->ColorMapData;
                        for (int i = firstIndex; i < palSize; i++)
                        {
                            palette.Entries[i] = pParser(ref palData);
                        }

                        break;
                    }

                    case TGAImageType.UncompressedTrueColor:
                    {
                        if (entryBpp == 15 || entryBpp == 16 && alphaBits == 0)
                        {
                            format = PixelFormat.Format16bppRgb555;
                            cParser = delegate(VoidPtr sPtr, int sIndex, VoidPtr dPtr, int dIndex)
                            {
                                ((RGB555Pixel*) dPtr)[dIndex] = ((RGB555Pixel*) sPtr)[sIndex];
                            };
                        }
                        else if (entryBpp == 16)
                        {
                            format = PixelFormat.Format16bppArgb1555;
                            cParser = delegate(VoidPtr sPtr, int sIndex, VoidPtr dPtr, int dIndex)
                            {
                                ((RGB555Pixel*) dPtr)[dIndex] = ((RGB555Pixel*) sPtr)[sIndex];
                            };
                        }
                        else if (entryBpp == 24)
                        {
                            format = PixelFormat.Format24bppRgb;
                            cParser = delegate(VoidPtr sPtr, int sIndex, VoidPtr dPtr, int dIndex)
                            {
                                ((RGBPixel*) dPtr)[dIndex] = ((RGBPixel*) sPtr)[sIndex];
                            };
                        }
                        else if (entryBpp == 32)
                        {
                            format = alphaBits == 8 ? PixelFormat.Format32bppArgb : PixelFormat.Format32bppRgb;
                            cParser = delegate(VoidPtr sPtr, int sIndex, VoidPtr dPtr, int dIndex)
                            {
                                ((ARGBPixel*) dPtr)[dIndex] = ((ARGBPixel*) sPtr)[sIndex];
                            };
                        }
                        else
                        {
                            throw new InvalidDataException("Unknown TGA file format.");
                        }

                        break;
                    }

                    case TGAImageType.UncompressedGreyscale:
                    {
                        if (entryBpp == 8)
                        {
                            format = PixelFormat.Format24bppRgb;
                            cParser = delegate(VoidPtr sPtr, int sIndex, VoidPtr dPtr, int dIndex)
                            {
                                ((RGBPixel*) dPtr)[dIndex] = RGBPixel.FromIntensity(((byte*) sPtr)[sIndex]);
                            };
                        }
                        else
                        {
                            throw new InvalidDataException("Unknown TGA file format.");
                        }

                        break;
                    }

                    default: throw new InvalidDataException("Unknown TGA file format.");
                }

                Bitmap bmp = new Bitmap(w, h, format);
                if (palette != null)
                {
                    bmp.Palette = palette;
                }

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, format);

                bool rle = ((int) header->imageType & 0x8) != 0;
                int srcStride = (entryBpp * w).Align(8) / 8;
                int rleBufferLen = rle ? srcStride : 0;

                byte* buffer = stackalloc byte[rleBufferLen];

                int origin = (int) header->imageSpecification.ImageOrigin;
                int xStep = (origin & 1) == 0 ? 1 : -1;
                int yStep = (origin & 2) != 0 ? 1 : -1;
                byte* imgSrc = header->ImageData;
                for (int dY = yStep == 1 ? 0 : h - 1, sY = 0; sY < h; dY += yStep, sY++)
                {
                    VoidPtr imgDst = (VoidPtr) data.Scan0 + data.Stride * dY;

                    if (rle)
                    {
                        imgSrc += DecodeRLE(imgSrc, buffer, srcStride, entryBpp);
                    }

                    for (int dX = xStep == 1 ? 0 : w - 1, sX = 0; sX < w; dX += xStep, sX++)
                    {
                        cParser(rle ? buffer : imgSrc, sX, imgDst, dX);
                    }

                    if (!rle)
                    {
                        imgSrc += srcStride;
                    }
                }

                bmp.UnlockBits(data);
                return bmp;
            }
        }

        public static unsafe void ToStream(Bitmap bmp, FileStream stream)
        {
            int w = bmp.Width, h = bmp.Height;

            TGAHeader header = new TGAHeader();
            TGAFooter footer = new TGAFooter(0, 0);

            PaletteEncoder pEnc = null;
            ColorParser cEnc = null;
            ColorPalette pal = null;

            header.imageSpecification.width = (ushort) w;
            header.imageSpecification.height = (ushort) h;
            header.imageType = TGAImageType.UncompressedTrueColor;
            switch (bmp.PixelFormat)
            {
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                {
                    pal = bmp.Palette;

                    header.colorMapType = 1;
                    header.imageType = TGAImageType.UncompressedColorMapped;
                    header.colorMapSpecification.length = (ushort) pal.Entries.Length;
                    header.colorMapSpecification.entrySize = 24;
                    header.imageSpecification.pixelDepth = 8;

                    pEnc = (ref VoidPtr ptr, Color c) =>
                    {
                        *(RGBPixel*) ptr = (RGBPixel) c;
                        ptr += 3;
                    };

                    if (bmp.PixelFormat == PixelFormat.Format4bppIndexed)
                    {
                        cEnc = delegate(VoidPtr sPtr, int sIndex, VoidPtr dPtr, int dIndex)
                        {
                            ((byte*) dPtr)[dIndex] = (sIndex & 1) == 0
                                ? (byte) (((byte*) sPtr)[sIndex >> 1] >> 4)
                                : (byte) (((byte*) sPtr)[sIndex >> 1] & 0xF);
                        };
                    }
                    else
                    {
                        cEnc = delegate(VoidPtr sPtr, int sIndex, VoidPtr dPtr, int dIndex)
                        {
                            ((byte*) dPtr)[dIndex] = ((byte*) sPtr)[sIndex];
                        };
                    }

                    break;
                }

                case PixelFormat.Format32bppRgb:
                case PixelFormat.Format32bppArgb:
                {
                    header.imageSpecification.pixelDepth = 32;
                    header.imageSpecification.AlphaBits =
                        bmp.PixelFormat == PixelFormat.Format32bppArgb ? (byte) 8 : (byte) 0;

                    cEnc = delegate(VoidPtr sPtr, int sIndex, VoidPtr dPtr, int dIndex)
                    {
                        ((ARGBPixel*) dPtr)[dIndex] = ((ARGBPixel*) sPtr)[sIndex];
                    };
                    break;
                }

                case PixelFormat.Format24bppRgb:
                {
                    header.imageSpecification.pixelDepth = 24;

                    cEnc = delegate(VoidPtr sPtr, int sIndex, VoidPtr dPtr, int dIndex)
                    {
                        ((RGBPixel*) dPtr)[dIndex] = ((RGBPixel*) sPtr)[sIndex];
                    };
                    break;
                }

                case PixelFormat.Format16bppRgb555:
                {
                    header.imageSpecification.pixelDepth = 15;
                    cEnc = delegate(VoidPtr sPtr, int sIndex, VoidPtr dPtr, int dIndex)
                    {
                        ((RGB555Pixel*) dPtr)[dIndex] = ((RGB555Pixel*) sPtr)[sIndex];
                    };
                    break;
                }

                case PixelFormat.Format16bppArgb1555:
                {
                    header.imageSpecification.pixelDepth = 16;
                    header.imageSpecification.AlphaBits = 1;
                    cEnc = delegate(VoidPtr sPtr, int sIndex, VoidPtr dPtr, int dIndex)
                    {
                        ((RGB555Pixel*) dPtr)[dIndex] = ((RGB555Pixel*) sPtr)[sIndex];
                    };
                    break;
                }

                default:
                    throw new FormatException("Input pixel format unsupported.");
            }

            int mapLen = header.colorMapSpecification.DataLength;
            int dataLen = header.imageSpecification.DataLength;

            int totalLen = TGAHeader.Size + mapLen + dataLen + TGAFooter.Size;
            stream.SetLength(totalLen);

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, bmp.PixelFormat);
            //using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 0x1000, FileOptions.RandomAccess))
            using (FileMap view = FileMap.FromStream(stream)
            ) // stream.MapView(0, (uint)totalLen, FileMapProtect.ReadWrite))
            {
                //Create header
                TGAHeader* pHeader = (TGAHeader*) view.Address;
                *pHeader = header;

                //Write id

                //Write color map
                if (pal != null)
                {
                    VoidPtr pMap = pHeader->ColorMapData;
                    for (int i = 0; i < pal.Entries.Length; i++)
                    {
                        pEnc(ref pMap, pal.Entries[i]);
                    }
                }

                //Write color data
                int dstStride = (pHeader->imageSpecification.pixelDepth * w).Align(8) / 8;
                int origin = (int) pHeader->imageSpecification.ImageOrigin;
                int xStep = (origin & 1) == 0 ? 1 : -1;
                int yStep = (origin & 2) != 0 ? 1 : -1;
                byte* imgDst = pHeader->ImageData;
                for (int sY = yStep == 1 ? 0 : h - 1, dY = 0; dY < h; sY += yStep, dY++)
                {
                    VoidPtr imgSrc = (VoidPtr) data.Scan0 + data.Stride * sY;

                    //Do RLE encoding

                    for (int sX = xStep == 1 ? 0 : w - 1, dX = 0; dX < w; sX += xStep, dX++)
                    {
                        cEnc(imgSrc, sX, imgDst, dX);
                    }

                    imgDst += dstStride;
                }

                //Write footer
                TGAFooter* pFooter = (TGAFooter*) (pHeader->ImageData + pHeader->imageSpecification.DataLength);
                *pFooter = footer;
            }

            bmp.UnlockBits(data);
        }

        public static void ToFile(Bitmap bmp, string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 8,
                FileOptions.RandomAccess))
            {
                ToStream(bmp, stream);
            }
        }
    }
}