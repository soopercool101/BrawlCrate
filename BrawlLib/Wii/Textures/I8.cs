using BrawlLib.Imaging;
using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Textures
{
    internal unsafe class I8 : TextureConverter
    {
        public override int BitsPerPixel => 8;
        public override int BlockWidth => 8;

        public override int BlockHeight => 4;

        //public override PixelFormat DecodedFormat { get { return PixelFormat.Format24bppRgb; } }
        public override WiiPixelFormat RawFormat => WiiPixelFormat.I8;

        protected override void DecodeBlock(VoidPtr blockAddr, ARGBPixel* dPtr, int width)
        {
            I8Pixel* sPtr = (I8Pixel*) blockAddr;
            //RGBPixel* dPtr = (RGBPixel*)destAddr;
            for (int y = 0; y < BlockHeight; y++, dPtr += width)
            {
                for (int x = 0; x < BlockWidth;)
                {
                    dPtr[x++] = (ARGBPixel) (*sPtr++);
                }
            }
        }

        protected override void EncodeBlock(ARGBPixel* sPtr, VoidPtr blockAddr, int width)
        {
            I8Pixel* dPtr = (I8Pixel*) blockAddr;
            for (int y = 0; y < BlockHeight; y++, sPtr += width)
            {
                for (int x = 0; x < BlockWidth;)
                {
                    *dPtr++ = (I8Pixel) sPtr[x++];
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct I8Pixel
    {
        public byte _value;

        public static explicit operator ARGBPixel(I8Pixel p)
        {
            return new ARGBPixel(255, p._value, p._value, p._value);
        }

        public static explicit operator RGBPixel(I8Pixel p)
        {
            return new RGBPixel {R = p._value, G = p._value, B = p._value};
        }

        public static explicit operator I8Pixel(ARGBPixel p)
        {
            return new I8Pixel
            {
                _value = (byte) ((p.R + p.G + p.B + 1) / 3)
            }; // Extra 1 added to get effect of rounding to nearest instead of rounding down
        }
    }
}