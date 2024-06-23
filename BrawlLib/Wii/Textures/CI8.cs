using BrawlLib.Imaging;
using BrawlLib.Internal;
using System.Drawing.Imaging;

namespace BrawlLib.Wii.Textures
{
    internal unsafe class CI8 : TextureConverter
    {
        public override int BitsPerPixel => 8;
        public override int BlockWidth => 8;

        public override int BlockHeight => 4;

        //public override PixelFormat DecodedFormat { get { return PixelFormat.Format8bppIndexed; } }
        public override WiiPixelFormat RawFormat => WiiPixelFormat.CI8;

        protected override void DecodeBlock(VoidPtr blockAddr, ARGBPixel* dPtr, int width, ColorPalette palette = null)
        {
            byte* sPtr = (byte*) blockAddr;
            if (palette != null)
            {
                for (int y = 0; y < BlockHeight; y++, dPtr += width)
                {
                    for (int x = 0; x < BlockWidth; x++)
                    {
                        byte b = *sPtr++;
                        if (b >= palette.Entries.Length)
                        {
                            dPtr[x] = new ARGBPixel();
                        }
                        else
                        {
                            dPtr[x] = (ARGBPixel)palette.Entries[b];
                        }
                    }
                }
            }
            else
            {
                for (int y = 0; y < BlockHeight; y++, dPtr += width)
                {
                    for (int x = 0; x < BlockWidth; x++)
                    {
                        dPtr[x] = new ARGBPixel(*sPtr++);
                    }
                }
            }
        }

        protected override void EncodeBlock(ARGBPixel* sPtr, VoidPtr blockAddr, int width)
        {
            byte* stPtr = (byte*) sPtr;
            byte* dPtr = (byte*) blockAddr;
            for (int y = 0; y < BlockHeight; y++, stPtr += width)
            {
                for (int x = 0; x < BlockWidth;)
                {
                    *dPtr++ = stPtr[x++];
                }
            }
        }
    }
}