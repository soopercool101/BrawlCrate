using BrawlLib.Imaging;
using BrawlLib.Internal;
using System;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Textures
{
    internal unsafe class I4 : TextureConverter
    {
        public override int BitsPerPixel => 4;
        public override int BlockWidth => 8;

        public override int BlockHeight => 8;

        //public override PixelFormat DecodedFormat { get { return PixelFormat.Format24bppRgb; } }
        public override WiiPixelFormat RawFormat => WiiPixelFormat.I4;

        protected override void DecodeBlock(VoidPtr blockAddr, ARGBPixel* dPtr, int width, ColorPalette palette = null)
        {
            I4Pixel* sPtr = (I4Pixel*) blockAddr;
            //RGBPixel* dPtr = (RGBPixel*)destAddr;
            for (int y = 0; y < BlockHeight; y++, dPtr += width)
            {
                for (int x = 0; x < BlockWidth;)
                {
                    dPtr[x++] = (*sPtr)[0];
                    dPtr[x++] = (*sPtr++)[1];
                }
            }
        }

        protected override void EncodeBlock(ARGBPixel* sPtr, VoidPtr blockAddr, int width)
        {
            I4Pixel* dPtr = (I4Pixel*) blockAddr;
            for (int y = 0; y < BlockHeight; y++, sPtr += width)
            {
                for (int x = 0; x < BlockWidth;)
                {
                    (*dPtr)[0] = sPtr[x++];
                    (*dPtr++)[1] = sPtr[x++];
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct I4Pixel
    {
        public byte _data;

        public ARGBPixel this[int index]
        {
            get
            {
                byte c = index % 2 == 0
                    ? (byte) ((_data & 0xF0) | (_data >> 4))
                    : (byte) ((_data & 0x0F) | (_data << 4));
                return new ARGBPixel {A = 0xFF, R = c, G = c, B = c};
            }
            set
            {
                int c = (value.R + value.G + value.B + 1) /
                        3;                               // Extra 1 added to get effect of rounding to nearest instead of rounding down
                c = Convert.ToInt32(c * (15.0 / 255.0)); // Convert from 8 bits to 4
                _data = index % 2 == 0 ? (byte) ((c << 4) | (_data & 0x0F)) : (byte) (c | (_data & 0xF0));
            }
        }


        public static explicit operator I4Pixel(ARGBPixel p)
        {
            int value = (p.R + p.G + p.B + 1) /
                        3;                                   // Extra 1 added to get effect of rounding to nearest instead of rounding down
            value = Convert.ToInt32(value * (15.0 / 255.0)); // Convert from 8 bits to 4
            return new I4Pixel {_data = (byte) ((value << 4) | value)};
        }
    }
}