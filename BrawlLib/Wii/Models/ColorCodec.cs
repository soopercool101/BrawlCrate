using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Textures;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Models
{
    public unsafe delegate void ColorCodecConverter(ref byte* pIn, ref byte* pOut);

    public unsafe class ColorCodec : IDisposable
    {
        #region Encoders

        private static void Color_RGBA_wRGB565(ref byte* pIn, ref byte* pOut)
        {
            int data = *pIn++ >> 3;
            data |= (*pIn++ >> 2) << 5;
            data |= (*pIn++ >> 3) << 11;

            byte* p = (byte*) &data;
            *pOut++ = p[1];
            *pOut++ = p[0];
        }

        private static void Color_RGBA_RGB(ref byte* pIn, ref byte* pOut)
        {
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            pIn++;
        }

        private static void Color_RGBA_RGBX(ref byte* pIn, ref byte* pOut)
        {
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = 0;
            pIn++;
        }

        private static void Color_RGBA_wRGBA4(ref byte* pIn, ref byte* pOut)
        {
            int data = 0;
            byte* p = (byte*) data;

            int i = 16;
            while ((i -= 4) >= 0)
            {
                data |= (*pIn++ >> 4) << i;
            }

            *pOut++ = p[1];
            *pOut++ = p[0];
        }

        private static void Color_RGBA_wRGBA6(ref byte* pIn, ref byte* pOut)
        {
            int data = 0;
            byte* p = (byte*) data;

            int i = 24;
            while ((i -= 6) >= 0)
            {
                data |= (*pIn++ >> 2) << i;
            }

            *pOut++ = p[2];
            *pOut++ = p[1];
            *pOut++ = p[0];
        }

        #endregion

        #region Decoders

        private static void Color_wRGB565_RGBA(ref byte* pIn, ref byte* pOut)
        {
            int val, data = *(bushort*) pIn;
            pIn += 2;

            val = data & 0xF800;
            *pOut++ = (byte) ((val >> 8) | (val >> 13));
            val = data & 0x7E0;
            *pOut++ = (byte) ((val >> 3) | (val >> 9));
            val = data & 0x1F;
            *pOut++ = (byte) ((val << 3) | (val >> 2));
            *pOut++ = 255;
        }

        private static void Color_RGB_RGBA(ref byte* pIn, ref byte* pOut)
        {
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = 255;
        }

        private static void Color_RGBX_RGBA(ref byte* pIn, ref byte* pOut)
        {
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = 255;
            pIn++;
        }

        private static void Color_wRGBA4_RGBA(ref byte* pIn, ref byte* pOut)
        {
            int val, data = *(bushort*) pIn;
            pIn += 2;

            int i = 4;
            while (i-- > 0)
            {
                val = (data >> (i << 2)) & 0xF;
                *pOut++ = (byte) (val | (val << 4));
            }
        }

        private static void Color_wRGBA6_RGBA(ref byte* pIn, ref byte* pOut)
        {
            *(RGBAPixel*) pOut = (ARGBPixel) (*(wRGBA6Pixel*) pIn);

            pIn += 3;
            pOut += 4;
        }

        private static void Color_RGBA_RGBA(ref byte* pIn, ref byte* pOut)
        {
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
        }

        #endregion

        public WiiColorComponentType _outType;
        public bool _hasAlpha;
        public int _dataLen;

        public int _srcCount;
        public int _dstCount, _dstStride;

        private GCHandle _handle;
        private RGBAPixel* _pData;

        public Remapper _remapData;

        public ColorCodec(RGBAPixel[] pixels)
        {
            _srcCount = pixels.Length;
            _handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            _pData = (RGBAPixel*) _handle.AddrOfPinnedObject();
            Evaluate();
        }

        public ColorCodec(RGBAPixel* sPtr, int count)
        {
            _pData = sPtr;
            _srcCount = count;
            Evaluate();
        }

        ~ColorCodec()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_handle.IsAllocated)
            {
                _handle.Free();
            }

            _pData = null;
            GC.SuppressFinalize(this);
        }

        private void Evaluate()
        {
            //Colors will almost always need remapping
            _remapData = new Remapper();
            _remapData.Remap(new MemoryList<RGBAPixel>(_pData, _srcCount), null); //Don't bother sorting

            int[] imp = _remapData._impTable;
            int impLen = imp.Length;

            _dstCount = impLen;

            //Do we have alpha?
            int i = 0;
            while (i < _dstCount && _pData[imp[i]].A == 255)
            {
                i++;
            }

            _hasAlpha = i < _srcCount;

            //Determine format
            if (_hasAlpha)
            {
                _outType = WiiColorComponentType.RGBA8;
            }
            else
            {
                _outType = WiiColorComponentType.RGB8;
            }

            switch (_outType)
            {
                case WiiColorComponentType.RGB565:
                case WiiColorComponentType.RGBA4:
                    _dstStride = 2;
                    break;
                case WiiColorComponentType.RGB8:
                case WiiColorComponentType.RGBA6:
                    _dstStride = 3;
                    break;
                case WiiColorComponentType.RGBA8:
                case WiiColorComponentType.RGBX8:
                    _dstStride = 4;
                    break;
            }

            _dataLen = _dstStride * _dstCount;

            GetEncoder();
        }

        private ColorCodecConverter enc;

        public void GetEncoder()
        {
            switch (_outType)
            {
                case WiiColorComponentType.RGB565:
                    enc = Color_RGBA_wRGB565;
                    break;
                case WiiColorComponentType.RGB8:
                    enc = Color_RGBA_RGB;
                    break;
                case WiiColorComponentType.RGBX8:
                    enc = Color_RGBA_RGBX;
                    break;
                case WiiColorComponentType.RGBA4:
                    enc = Color_RGBA_wRGBA4;
                    break;
                case WiiColorComponentType.RGBA6:
                    enc = Color_RGBA_wRGBA6;
                    break;
                case WiiColorComponentType.RGBA8:
                    enc = Color_RGBA_RGBA;
                    break;
                default: return;
            }
        }

        public void Write(byte* pOut)
        {
            try
            {
                //Write colors
                byte* sPtr;
                foreach (int i in _remapData._impTable)
                {
                    sPtr = (byte*) (_pData + i);
                    enc(ref sPtr, ref pOut);
                }

                //Zero-fill
                for (int x = _dataLen; (x & 0x1F) != 0; x++)
                {
                    *pOut++ = 0;
                }
            }
            finally
            {
                Dispose();
            }
        }

        public void Write(ref byte* pOut, int index)
        {
            byte* sPtr = (byte*) (_pData + _remapData._impTable[index]);
            enc(ref sPtr, ref pOut);
        }

        public static UnsafeBuffer Decode(MDL0ColorData* header)
        {
            int count = header->_numEntries;
            UnsafeBuffer buffer = new UnsafeBuffer(count * 4);
            byte* pIn = (byte*) header + header->_dataOffset;
            byte* pOut = (byte*) buffer.Address;

            ColorCodecConverter dec;
            switch (header->Type)
            {
                case WiiColorComponentType.RGB565:
                    dec = Color_wRGB565_RGBA;
                    break;
                case WiiColorComponentType.RGB8:
                    dec = Color_RGB_RGBA;
                    break;
                case WiiColorComponentType.RGBA4:
                    dec = Color_wRGBA4_RGBA;
                    break;
                case WiiColorComponentType.RGBA6:
                    dec = Color_wRGBA6_RGBA;
                    break;
                case WiiColorComponentType.RGBA8:
                    dec = Color_RGBA_RGBA;
                    break;
                case WiiColorComponentType.RGBX8:
                    dec = Color_RGBX_RGBA;
                    break;
                default: return null;
            }

            while (count-- > 0)
            {
                dec(ref pIn, ref pOut);
            }

            return buffer;
        }

        public static UnsafeBuffer Decode(VoidPtr addr, uint length, WiiColorComponentType componentType)
        {
            int bytesPerVal = 0;
            ColorCodecConverter dec;
            switch (componentType)
            {
                case WiiColorComponentType.RGB565:
                    dec = Color_wRGB565_RGBA;
                    bytesPerVal = 2;
                    break;
                case WiiColorComponentType.RGB8:
                    dec = Color_RGB_RGBA;
                    bytesPerVal = 3;
                    break;
                case WiiColorComponentType.RGBA4:
                    dec = Color_wRGBA4_RGBA;
                    bytesPerVal = 3;
                    break;
                case WiiColorComponentType.RGBA6:
                    dec = Color_wRGBA6_RGBA;
                    bytesPerVal = 2;
                    break;
                case WiiColorComponentType.RGBA8:
                    dec = Color_RGBA_RGBA;
                    bytesPerVal = 4;
                    break;
                case WiiColorComponentType.RGBX8:
                    dec = Color_RGBX_RGBA;
                    bytesPerVal = 4;
                    break;
                default: return null;
            }

            int count = (int) (length / bytesPerVal);

            UnsafeBuffer buffer = new UnsafeBuffer(count * 4);
            byte* pIn = (byte*) addr;
            byte* pOut = (byte*) buffer.Address;

            while (count-- > 0)
            {
                dec(ref pIn, ref pOut);
            }

            return buffer;
        }

        public static ARGBPixel[] ExtractColors(MDL0ColorData* colors)
        {
            int count = colors->_numEntries;
            ARGBPixel[] c = new ARGBPixel[count];

            fixed (ARGBPixel* p = c)
            {
                ARGBPixel* dPtr = p;

                switch (colors->Type)
                {
                    case WiiColorComponentType.RGB565:
                    {
                        wRGB565Pixel* sPtr = (wRGB565Pixel*) colors->Data;
                        for (int i = 0; i < count; i++)
                        {
                            *dPtr++ = (ARGBPixel) (*sPtr++);
                        }

                        break;
                    }

                    case WiiColorComponentType.RGB8:
                    {
                        wRGBPixel* sPtr = (wRGBPixel*) colors->Data;
                        for (int i = 0; i < count; i++)
                        {
                            *dPtr++ = (ARGBPixel) (*sPtr++);
                        }

                        break;
                    }

                    case WiiColorComponentType.RGBX8:
                    {
                        wRGBXPixel* sPtr = (wRGBXPixel*) colors->Data;
                        for (int i = 0; i < count; i++)
                        {
                            *dPtr++ = (ARGBPixel) (*sPtr++);
                        }

                        break;
                    }

                    case WiiColorComponentType.RGBA4:
                    {
                        wRGBA4Pixel* sPtr = (wRGBA4Pixel*) colors->Data;
                        for (int i = 0; i < count; i++)
                        {
                            *dPtr++ = (ARGBPixel) (*sPtr++);
                        }

                        break;
                    }

                    case WiiColorComponentType.RGBA6:
                    {
                        wRGBA6Pixel* sPtr = (wRGBA6Pixel*) colors->Data;
                        for (int i = 0; i < count; i++)
                        {
                            *dPtr++ = (ARGBPixel) (*sPtr++);
                        }

                        break;
                    }

                    case WiiColorComponentType.RGBA8:
                    {
                        wRGBAPixel* sPtr = (wRGBAPixel*) colors->Data;
                        for (int i = 0; i < count; i++)
                        {
                            *dPtr++ = (ARGBPixel) (*sPtr++);
                        }

                        break;
                    }
                }
            }

            return c;
        }

        //public static List<RGBAPixel> ToRGBA(ARGBPixel[] pixels)
        //{
        //    List<RGBAPixel> newPixels = new List<RGBAPixel>(pixels.Length);
        //    int i = 0;
        //    foreach (ARGBPixel p in pixels)
        //    {
        //        newPixels.Add((RGBAPixel)p);
        //        i += 1;
        //    }
        //    return newPixels;
        //}
    }
}