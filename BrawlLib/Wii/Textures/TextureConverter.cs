using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.Internal.Drawing;
using BrawlLib.Internal.Drawing.Imaging;
using BrawlLib.Internal.IO;
using BrawlLib.SSBB.Types;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace BrawlLib.Wii.Textures
{
    public abstract unsafe class TextureConverter
    {
        public static readonly TextureConverter I4 = new I4();
        public static readonly TextureConverter IA4 = new IA4();
        public static readonly TextureConverter I8 = new I8();
        public static readonly TextureConverter IA8 = new IA8();
        public static readonly TextureConverter RGB565 = new RGB565();
        public static readonly TextureConverter RGB5A3 = new RGB5A3();
        public static readonly TextureConverter CI4 = new CI4();
        public static readonly TextureConverter CI8 = new CI8();
        public static readonly CMPR CMPR = new CMPR();
        public static readonly TextureConverter RGBA8 = new RGBA8();

        public abstract WiiPixelFormat RawFormat { get; }
        public abstract int BitsPerPixel { get; }
        public abstract int BlockWidth { get; }
        public abstract int BlockHeight { get; }
        public bool IsIndexed => RawFormat == WiiPixelFormat.CI4 || RawFormat == WiiPixelFormat.CI8;

        public int GetMipOffset(int width, int height, int mipLevel)
        {
            return GetMipOffset(ref width, ref height, mipLevel);
        }

        public int GetMipOffset(ref int width, ref int height, int mipLevel)
        {
            int offset = 0;
            while (mipLevel-- > 1)
            {
                offset += (width.Align(BlockWidth) * height.Align(BlockHeight) * BitsPerPixel) >> 3;
                width = Math.Max(width >> 1, 1);
                height = Math.Max(height >> 1, 1);
            }

            return offset;
        }

        public int GetFileSize(int width, int height, int mipLevels)
        {
            return GetMipOffset(width, height, mipLevels + 1);
        }

        public virtual FileMap EncodeTPLTextureIndexed(Bitmap src, int numColors, WiiPaletteFormat format,
                                                       QuantizationAlgorithm algorithm, out FileMap paletteFile)
        {
            using (Bitmap indexed = src.Quantize(algorithm, numColors, RawFormat, format, null))
            {
                return EncodeTPLTextureIndexed(indexed, 1, format, out paletteFile);
            }
        }

        public virtual FileMap EncodeTextureIndexed(Bitmap src, int mipLevels, int numColors, WiiPaletteFormat format,
                                                    QuantizationAlgorithm algorithm, out FileMap paletteFile)
        {
            using (Bitmap indexed = src.Quantize(algorithm, numColors, RawFormat, format, null))
            {
                return EncodeTEX0TextureIndexed(indexed, mipLevels, format, out paletteFile);
            }
        }

        public virtual FileMap EncodeREFTTextureIndexed(Bitmap src, int mipLevels, int numColors,
                                                        WiiPaletteFormat format, QuantizationAlgorithm algorithm)
        {
            using (Bitmap indexed = src.Quantize(algorithm, numColors, RawFormat, format, null))
            {
                return EncodeREFTTextureIndexed(indexed, mipLevels, format);
            }
        }

        public virtual FileMap EncodeTPLTextureIndexed(Bitmap src, int mipLevels, WiiPaletteFormat format,
                                                       out FileMap paletteFile)
        {
            if (!src.IsIndexed())
            {
                throw new ArgumentException("Source image must be indexed.");
            }

            FileMap texMap = EncodeTPLTexture(src, mipLevels);
            paletteFile = EncodeTPLPalette(src.Palette, format);
            return texMap;
        }

        public virtual FileMap EncodeREFTTextureIndexed(Bitmap src, int mipLevels, WiiPaletteFormat format)
        {
            if (!src.IsIndexed())
            {
                throw new ArgumentException("Source image must be indexed.");
            }

            return EncodeREFTTexture(src, mipLevels, format);
        }

        public virtual FileMap EncodeTEX0TextureIndexed(Bitmap src, int mipLevels, WiiPaletteFormat format,
                                                        out FileMap paletteFile)
        {
            if (!src.IsIndexed())
            {
                throw new ArgumentException("Source image must be indexed.");
            }

            FileMap texMap = EncodeTEX0Texture(src, mipLevels);
            paletteFile = EncodePLT0Palette(src.Palette, format);
            return texMap;
        }

        public virtual FileMap EncodeREFTTexture(Bitmap src, int mipLevels, WiiPaletteFormat format)
        {
            int w = src.Width, h = src.Height;
            int bw = BlockWidth, bh = BlockHeight;

            ColorPalette pal = src.Palette;

            PixelFormat fmt = src.IsIndexed() ? src.PixelFormat : PixelFormat.Format32bppArgb;

            FileMap fileView =
                FileMap.FromTempFile(GetFileSize(w, h, mipLevels) + 0x20 + (pal != null ? pal.Entries.Length * 2 : 0));
            try
            {
                //Build REFT image header
                REFTImageHeader* header = (REFTImageHeader*) fileView.Address;
                *header = new REFTImageHeader((ushort) w, (ushort) h, (byte) RawFormat, (byte) format,
                    (ushort) (pal != null ? pal.Entries.Length : 0),
                    (uint) fileView.Length - 0x20 - (uint) (pal != null ? pal.Entries.Length * 2 : 0),
                    (byte) (mipLevels - 1));

                int sStep = bw * Image.GetPixelFormatSize(fmt) / 8;
                int dStep = bw * bh * BitsPerPixel / 8;

                using (DIB dib = DIB.FromBitmap(src, bw, bh, fmt))
                {
                    for (int i = 1; i <= mipLevels; i++)
                    {
                        EncodeLevel((VoidPtr) header + 0x20, dib, src, dStep, sStep, i);
                    }
                }

                if (pal != null)
                {
                    int count = pal.Entries.Length;

                    switch (format)
                    {
                        case WiiPaletteFormat.IA8:
                        {
                            IA8Pixel* dPtr = (IA8Pixel*) header->PaletteData;
                            for (int i = 0; i < count; i++)
                            {
                                dPtr[i] = (IA8Pixel) pal.Entries[i];
                            }

                            break;
                        }

                        case WiiPaletteFormat.RGB565:
                        {
                            wRGB565Pixel* dPtr = (wRGB565Pixel*) header->PaletteData;
                            for (int i = 0; i < count; i++)
                            {
                                dPtr[i] = (wRGB565Pixel) pal.Entries[i];
                            }

                            break;
                        }

                        case WiiPaletteFormat.RGB5A3:
                        {
                            wRGB5A3Pixel* dPtr = (wRGB5A3Pixel*) header->PaletteData;
                            for (int i = 0; i < count; i++)
                            {
                                dPtr[i] = (wRGB5A3Pixel) pal.Entries[i];
                            }

                            break;
                        }
                    }
                }

                return fileView;
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
                fileView.Dispose();
                return null;
            }
        }

        public virtual FileMap EncodeTEX0Texture(Bitmap src, int mipLevels)
        {
            int w = src.Width, h = src.Height;
            int bw = BlockWidth, bh = BlockHeight;

            PixelFormat fmt;
            switch (RawFormat)
            {
                case WiiPixelFormat.CI4:
                case WiiPixelFormat.CI8:
                case WiiPixelFormat.CMPR:
                    fmt = src.IsIndexed() ? src.PixelFormat : PixelFormat.Format32bppArgb;
                    break;
                default:
                    fmt = PixelFormat.Format32bppArgb;
                    break;
            }

            FileMap fileView = FileMap.FromTempFile(GetFileSize(w, h, mipLevels) + 0x40);
            try
            {
                //Build TEX header
                TEX0v1* header = (TEX0v1*) fileView.Address;
                *header = new TEX0v1(w, h, RawFormat, mipLevels);

                int sStep = bw * Image.GetPixelFormatSize(fmt) / 8;
                int dStep = bw * bh * BitsPerPixel / 8;

                using (DIB dib = DIB.FromBitmap(src, bw, bh, fmt))
                {
                    for (int i = 1; i <= mipLevels; i++)
                    {
                        EncodeLevel(header->PixelData, dib, src, dStep, sStep, i);
                    }
                }

                return fileView;
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
                fileView.Dispose();
                return null;
            }
        }

        public virtual FileMap EncodeTPLTexture(Bitmap src, int mipLevels)
        {
            int w = src.Width, h = src.Height;
            int bw = BlockWidth, bh = BlockHeight;

            PixelFormat fmt = src.IsIndexed() ? src.PixelFormat : PixelFormat.Format32bppArgb;

            FileMap fileView = FileMap.FromTempFile(GetFileSize(w, h, mipLevels) + TPLTextureHeader.Size);
            try
            {
                //Build TPL header
                TPLTextureHeader* tex = (TPLTextureHeader*) fileView.Address;
                tex->_wrapS = 0;
                tex->_wrapT = 0;
                tex->_minFilter = 1;
                tex->_magFilter = 1;
                tex->_minLOD = 0;
                tex->_maxLOD = (byte) (mipLevels - 1);
                tex->PixelFormat = RawFormat;
                tex->_width = (ushort) w;
                tex->_height = (ushort) h;
                tex->_data = TPLTextureHeader.Size;

                int sStep = bw * Image.GetPixelFormatSize(fmt) / 8;
                int dStep = bw * bh * BitsPerPixel / 8;
                VoidPtr baseAddr = fileView.Address;

                using (DIB dib = DIB.FromBitmap(src, bw, bh, fmt))
                {
                    for (int i = 1; i <= mipLevels; i++)
                    {
                        EncodeLevel(baseAddr + tex->_data, dib, src, dStep, sStep, i);
                    }
                }

                return fileView;
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
                fileView.Dispose();
                return null;
            }
        }

        internal virtual void EncodeLevel(VoidPtr dstAddr, DIB dib, Bitmap src, int dStep, int sStep, int level)
        {
            int mw = dib.Width, mh = dib.Height, aw = mw.Align(BlockWidth);
            if (level != 1)
            {
                dstAddr += GetMipOffset(ref mw, ref mh, level);
                using (Bitmap mip = src.GenerateMip(level))
                {
                    dib.ReadBitmap(mip, mw, mh);
                }
            }

            mw = mw.Align(BlockWidth);
            mh = mh.Align(BlockHeight);

            int bStride = mw * BitsPerPixel / 8;
            for (int y = 0; y < mh; y += BlockHeight)
            {
                VoidPtr sPtr = (int) dib.Scan0 + y * dib.Stride;
                VoidPtr dPtr = dstAddr + y * bStride;
                for (int x = 0; x < mw; x += BlockWidth, dPtr += dStep, sPtr += sStep)
                {
                    EncodeBlock((ARGBPixel*) sPtr, dPtr, aw);
                }
            }
        }

        protected abstract void EncodeBlock(ARGBPixel* sPtr, VoidPtr blockAddr, int width);

        public Bitmap DecodeTexture(TEX0v1* texture, ColorPalette palette = null)
        {
            return DecodeTexture(texture, 1, palette);
        }

        public virtual Bitmap DecodeTexture(TEX0v1* texture, int mipLevel, ColorPalette palette = null)
        {
            int w = (ushort) texture->_width, h = (ushort) texture->_height;
            VoidPtr addr = texture->PixelData + GetMipOffset(ref w, ref h, mipLevel);
            int aw = w.Align(BlockWidth), ah = h.Align(BlockHeight);

            using (DIB dib = new DIB(w, h, BlockWidth, BlockHeight, PixelFormat.Format32bppArgb))
            {
                int sStep = BlockWidth * BlockHeight * BitsPerPixel / 8;
                int bStride = aw * BitsPerPixel / 8;
                for (int y = 0; y < ah; y += BlockHeight)
                {
                    ARGBPixel* dPtr = (ARGBPixel*) dib.Scan0 + y * aw;
                    VoidPtr sPtr = addr + y * bStride;
                    for (int x = 0; x < aw; x += BlockWidth, dPtr += BlockWidth, sPtr += sStep)
                    {
                        DecodeBlock(sPtr, dPtr, aw, palette);
                    }
                }

                return dib.ToBitmap();
            }
        }

        public static Bitmap Decode(VoidPtr addr, int w, int h, int mipLevel, WiiPixelFormat fmt, ColorPalette palette = null)
        {
            return Get(fmt).DecodeTexture(addr, w, h, mipLevel, palette);
        }

        public virtual Bitmap DecodeTexture(VoidPtr addr, int w, int h, int mipLevel, ColorPalette palette = null)
        {
            addr += GetMipOffset(ref w, ref h, mipLevel);

            int aw = w.Align(BlockWidth), ah = h.Align(BlockHeight);

            using (DIB dib = new DIB(w, h, BlockWidth, BlockHeight, PixelFormat.Format32bppArgb))
            {
                int sStep = BlockWidth * BlockHeight * BitsPerPixel / 8;
                int bStride = aw * BitsPerPixel / 8;
                for (int y = 0; y < ah; y += BlockHeight)
                {
                    ARGBPixel* dPtr = (ARGBPixel*) dib.Scan0 + y * aw;
                    VoidPtr sPtr = addr + y * bStride;
                    for (int x = 0; x < aw; x += BlockWidth, dPtr += BlockWidth, sPtr += sStep)
                    {
                        DecodeBlock(sPtr, dPtr, aw, palette);
                    }
                }

                return dib.ToBitmap();
            }
        }

        public virtual Bitmap DecodeTextureIndexed(TEX0v1* texture, PLT0v1* palette, int mipLevel)
        {
            return DecodeTextureIndexed(texture, DecodePalette(palette), mipLevel);
        }

        public virtual Bitmap DecodeTextureIndexed(TEX0v1* texture, ColorPalette palette, int mipLevel)
        {
            return DecodeTexture(texture, mipLevel, palette);
        }

        public virtual Bitmap DecodeTextureIndexed(VoidPtr addr, int w, int h, ColorPalette palette, int mipLevel,
                                                   WiiPixelFormat fmt)
        {
            return Decode(addr, w, h, mipLevel, fmt, palette);
        }

        protected abstract void DecodeBlock(VoidPtr blockAddr, ARGBPixel* destAddr, int width, ColorPalette palette = null);

        public static TextureConverter Get(WiiPixelFormat format)
        {
            switch (format)
            {
                case WiiPixelFormat.I4:     return I4;
                case WiiPixelFormat.IA4:    return IA4;
                case WiiPixelFormat.I8:     return I8;
                case WiiPixelFormat.IA8:    return IA8;
                case WiiPixelFormat.RGB565: return RGB565;
                case WiiPixelFormat.RGB5A3: return RGB5A3;
                case WiiPixelFormat.CI4:    return CI4;
                case WiiPixelFormat.CI8:    return CI8;
                case WiiPixelFormat.CMPR:   return CMPR;
                case WiiPixelFormat.RGBA8:  return RGBA8;
            }

            return null;
        }

        public static Bitmap Decode(TEX0v1* texture, int mipLevel)
        {
            return Get(texture->PixelFormat).DecodeTexture(texture, mipLevel);
        }

        public static Bitmap DecodeIndexed(TEX0v1* texture, PLT0v1* palette, int mipLevel)
        {
            return Get(texture->PixelFormat).DecodeTextureIndexed(texture, palette, mipLevel);
        }

        public static Bitmap DecodeIndexed(TEX0v1* texture, ColorPalette palette, int mipLevel)
        {
            return Get(texture->PixelFormat).DecodeTextureIndexed(texture, palette, mipLevel);
        }

        public static Bitmap DecodeIndexed(VoidPtr addr, int w, int h, ColorPalette palette, int mipLevel,
                                           WiiPixelFormat fmt)
        {
            return Get(fmt).DecodeTextureIndexed(addr, w, h, palette, mipLevel, fmt);
        }

        public static FileMap EncodePLT0Palette(ColorPalette pal, WiiPaletteFormat format)
        {
            FileMap fileView = FileMap.FromTempFile(pal.Entries.Length * 2 + 0x40);
            try
            {
                PLT0v1* header = (PLT0v1*) fileView.Address;
                *header = new PLT0v1(pal.Entries.Length, format);

                EncodePalette(fileView.Address + 0x40, pal, format);
                return fileView;
            }
            catch (Exception)
            {
                fileView.Dispose();
                throw;
                //MessageBox.Show(x.ToString());
                //fileView.Dispose();
                //return null;
            }
        }

        public static FileMap EncodeTPLPalette(ColorPalette pal, WiiPaletteFormat format)
        {
            FileMap fileView = FileMap.FromTempFile(pal.Entries.Length * 2 + 0xC);
            try
            {
                TPLPaletteHeader* header = (TPLPaletteHeader*) fileView.Address;
                header->_format = (uint) format;
                header->_numEntries = (ushort) pal.Entries.Length;
                header->_data = 0xC;

                EncodePalette(fileView.Address + 0xC, pal, format);
                return fileView;
            }
            catch (Exception)
            {
                fileView.Dispose();
                throw;
                //MessageBox.Show(x.ToString());
                //fileView.Dispose();
                //return null;
            }
        }

        public static FileMap EncodePalette(ColorPalette pal, WiiPaletteFormat format)
        {
            FileMap fileView = FileMap.FromTempFile(pal.Entries.Length * 2);
            try
            {
                EncodePalette(fileView.Address, pal, format);
                return fileView;
            }
            catch (Exception)
            {
                fileView.Dispose();
                throw;
                //MessageBox.Show(x.ToString());
                //fileView.Dispose();
                //return null;
            }
        }

        public static void EncodePalette(VoidPtr destAddr, ColorPalette pal, WiiPaletteFormat format)
        {
            int count = pal.Entries.Length;

            switch (format)
            {
                case WiiPaletteFormat.IA8:
                {
                    IA8Pixel* dPtr = (IA8Pixel*) destAddr;
                    for (int i = 0; i < count; i++)
                    {
                        dPtr[i] = (IA8Pixel) pal.Entries[i];
                    }

                    break;
                }

                case WiiPaletteFormat.RGB565:
                {
                    wRGB565Pixel* dPtr = (wRGB565Pixel*) destAddr;
                    for (int i = 0; i < count; i++)
                    {
                        dPtr[i] = (wRGB565Pixel) pal.Entries[i];
                    }

                    break;
                }

                case WiiPaletteFormat.RGB5A3:
                {
                    wRGB5A3Pixel* dPtr = (wRGB5A3Pixel*) destAddr;
                    for (int i = 0; i < count; i++)
                    {
                        dPtr[i] = (wRGB5A3Pixel) pal.Entries[i];
                    }

                    break;
                }
            }
        }

        public static ColorPalette DecodePalette(PLT0v1* palette)
        {
            int count = palette->_numEntries;
            ColorPalette pal = ColorPaletteExtension.CreatePalette(ColorPaletteFlags.HasAlpha, count);
            switch (palette->PaletteFormat)
            {
                case WiiPaletteFormat.IA8:
                {
                    IA8Pixel* sPtr = (IA8Pixel*) palette->PaletteData;
                    for (int i = 0; i < count; i++)
                    {
                        pal.Entries[i] = (Color) sPtr[i];
                    }

                    break;
                }

                case WiiPaletteFormat.RGB565:
                {
                    wRGB565Pixel* sPtr = (wRGB565Pixel*) palette->PaletteData;
                    for (int i = 0; i < count; i++)
                    {
                        pal.Entries[i] = (Color) sPtr[i];
                    }

                    break;
                }

                case WiiPaletteFormat.RGB5A3:
                {
                    wRGB5A3Pixel* sPtr = (wRGB5A3Pixel*) palette->PaletteData;
                    for (int i = 0; i < count; i++)
                    {
                        pal.Entries[i] = (Color) (ARGBPixel) sPtr[i];
                    }

                    break;
                }
            }

            return pal;
        }

        public static ColorPalette DecodePalette(VoidPtr address, int count, WiiPaletteFormat format)
        {
            ColorPalette pal = ColorPaletteExtension.CreatePalette(ColorPaletteFlags.HasAlpha, count);
            switch (format)
            {
                case WiiPaletteFormat.IA8:
                {
                    IA8Pixel* sPtr = (IA8Pixel*) address;
                    for (int i = 0; i < count; i++)
                    {
                        pal.Entries[i] = (Color) sPtr[i];
                    }

                    break;
                }

                case WiiPaletteFormat.RGB565:
                {
                    wRGB565Pixel* sPtr = (wRGB565Pixel*) address;
                    for (int i = 0; i < count; i++)
                    {
                        pal.Entries[i] = (Color) sPtr[i];
                    }

                    break;
                }

                case WiiPaletteFormat.RGB5A3:
                {
                    wRGB5A3Pixel* sPtr = (wRGB5A3Pixel*) address;
                    for (int i = 0; i < count; i++)
                    {
                        pal.Entries[i] = (Color) (ARGBPixel) sPtr[i];
                    }

                    break;
                }
            }

            return pal;
        }
    }
}