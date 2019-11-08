using BrawlLib.Imaging;
using BrawlLib.Internal;

namespace BrawlLib.Wii.Textures
{
    internal unsafe class RGB5A3 : TextureConverter
    {
        public override int BitsPerPixel => 16;
        public override int BlockWidth => 4;

        public override int BlockHeight => 4;

        //public override PixelFormat DecodedFormat { get { return PixelFormat.Format32bppArgb; } }
        public override WiiPixelFormat RawFormat => WiiPixelFormat.RGB5A3;

        protected override void DecodeBlock(VoidPtr blockAddr, ARGBPixel* dPtr, int width)
        {
            wRGB5A3Pixel* sPtr = (wRGB5A3Pixel*) blockAddr;
            //ARGBPixel* dPtr = (ARGBPixel*)destAddr;
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
            wRGB5A3Pixel* dPtr = (wRGB5A3Pixel*) blockAddr;
            for (int y = 0; y < BlockHeight; y++, sPtr += width)
            {
                for (int x = 0; x < BlockWidth;)
                {
                    *dPtr++ = (wRGB5A3Pixel) sPtr[x++];
                }
            }
        }
    }
}