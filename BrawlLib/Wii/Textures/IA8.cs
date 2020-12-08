using BrawlLib.Imaging;
using BrawlLib.Internal;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Textures
{
    internal unsafe class IA8 : TextureConverter
    {
        public override int BitsPerPixel => 16;
        public override int BlockWidth => 4;

        public override int BlockHeight => 4;

        //public override PixelFormat DecodedFormat { get { return PixelFormat.Format32bppArgb; } }
        public override WiiPixelFormat RawFormat => WiiPixelFormat.IA8;

        protected override void DecodeBlock(VoidPtr blockAddr, ARGBPixel* dPtr, int width)
        {
            IA8Pixel* sPtr = (IA8Pixel*) blockAddr;
            //ARGBPixel* dPtr = (ARGBPixel*)destAddr;
            for (int y = 0; y < BlockHeight; y++, dPtr += width)
            {
                for (int x = 0; x < BlockWidth;)
                {
                    dPtr[x++] = *sPtr++;
                }
            }
        }

        protected override void EncodeBlock(ARGBPixel* sPtr, VoidPtr blockAddr, int width)
        {
            IA8Pixel* dPtr = (IA8Pixel*) blockAddr;
            for (int y = 0; y < BlockHeight; y++, sPtr += width)
            {
                for (int x = 0; x < BlockWidth;)
                {
                    *dPtr++ = sPtr[x++];
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct IA8Pixel
    {
        public byte alpha;
        public byte intensity;

        public static implicit operator ARGBPixel(IA8Pixel p)
        {
            return new ARGBPixel {A = p.alpha, R = p.intensity, G = p.intensity, B = p.intensity};
        }

        public static implicit operator IA8Pixel(ARGBPixel p)
        {
            return new IA8Pixel
            {
                intensity = (byte) ((p.R + p.G + p.B + 1) / 3), alpha = p.A
            }; // Extra 1 added to get effect of rounding to nearest instead of rounding down
        }

        public static explicit operator Color(IA8Pixel p)
        {
            return Color.FromArgb(p.alpha, p.intensity, p.intensity, p.intensity);
        }

        public static explicit operator IA8Pixel(Color p)
        {
            return new IA8Pixel
            {
                intensity = (byte) ((p.R + p.G + p.B + 1) / 3), alpha = p.A
            }; // Extra 1 added to get effect of rounding to nearest instead of rounding down
        }
    }
}