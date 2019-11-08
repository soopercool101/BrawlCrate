using BrawlLib.Imaging;
using BrawlLib.Internal;

namespace BrawlLib.Wii.Textures
{
    internal unsafe class RGB565 : TextureConverter
    {
        public override int BitsPerPixel => 16;
        public override int BlockWidth => 4;

        public override int BlockHeight => 4;

        //public override PixelFormat DecodedFormat { get { return PixelFormat.Format24bppRgb; } }
        public override WiiPixelFormat RawFormat => WiiPixelFormat.RGB565;

        protected override void DecodeBlock(VoidPtr blockAddr, ARGBPixel* dPtr, int width)
        {
            wRGB565Pixel* sPtr = (wRGB565Pixel*) blockAddr;
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
            wRGB565Pixel* dPtr = (wRGB565Pixel*) blockAddr;
            for (int y = 0; y < BlockHeight; y++, sPtr += width)
            {
                for (int x = 0; x < BlockWidth;)
                {
                    *dPtr++ = (wRGB565Pixel) sPtr[x++];
                }
            }
        }
    }
}