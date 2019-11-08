using BrawlLib.Imaging;
using BrawlLib.Internal;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Textures
{
    public unsafe class IA4 : TextureConverter
    {
        public override int BitsPerPixel => 8;
        public override int BlockWidth => 8;

        public override int BlockHeight => 4;

        //public override PixelFormat DecodedFormat { get { return PixelFormat.Format32bppArgb; } }
        public override WiiPixelFormat RawFormat => WiiPixelFormat.IA4;

        protected override void DecodeBlock(VoidPtr blockAddr, ARGBPixel* dPtr, int width)
        {
            IA4Pixel* sPtr = (IA4Pixel*) blockAddr;
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
            IA4Pixel* dPtr = (IA4Pixel*) blockAddr;
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
    internal struct IA4Pixel
    {
        public byte data;

        public byte Alpha
        {
            get => (byte) ((data >> 4) | (data & 0xF0));
            set => data = (byte) ((data & 0x0F) | (value << 4));
        }

        public byte Intensity
        {
            get => (byte) ((data << 4) | (data & 0x0F));
            set => data = (byte) ((data & 0xF0) | (value & 0x0F));
        }

        public static implicit operator ARGBPixel(IA4Pixel p)
        {
            byte i = p.Intensity;
            return new ARGBPixel {A = p.Alpha, R = i, G = i, B = i};
        }

        public static implicit operator IA4Pixel(ARGBPixel p)
        {
            int intensity =
                (p.R + p.G + p.B + 1) /
                3;                                                   // Extra 1 added to get effect of rounding to nearest instead of rounding down
            intensity = Convert.ToInt32(intensity * (15.0 / 255.0)); // Convert intensity from 8 bits to 4
            int alpha = Convert.ToInt32(p.A * (15.0 / 255.0));       // Convert alpha from 8 bits to 4
            return new IA4Pixel {data = (byte) ((alpha << 4) | intensity)};
        }
    }
}