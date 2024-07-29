using BrawlLib.Imaging;
using BrawlLib.Internal;
using System.Drawing.Imaging;

namespace BrawlLib.Wii.Textures
{
    internal unsafe class CI4 : TextureConverter
    {
        public override int BitsPerPixel => 4;
        public override int BlockWidth => 8;

        public override int BlockHeight => 8;

        //public override PixelFormat DecodedFormat { get { return PixelFormat.Format4bppIndexed; } }
        public override WiiPixelFormat RawFormat => WiiPixelFormat.CI4;

        protected override void DecodeBlock(VoidPtr blockAddr, ARGBPixel* dPtr, int width, ColorPalette palette = null)
        {
            byte* sPtr = (byte*) blockAddr;
            byte b;
            if (palette != null)
            {
                for (int y = 0; y < BlockHeight; y++, dPtr += width)
                {
                    for (int x = 0; x < BlockWidth;)
                    {
                        b = *sPtr++;
                        dPtr[x++] = (ARGBPixel)palette.Entries[b >> 4];
                        dPtr[x++] = (ARGBPixel)palette.Entries[b & 0xF];
                    }
                }
            }
            else
            {
                for (int y = 0; y < BlockHeight; y++, dPtr += width)
                {
                    for (int x = 0; x < BlockWidth;)
                    {
                        b = *sPtr++;
                        dPtr[x++] = new ARGBPixel((byte) (b & 0xF0));
                        dPtr[x++] = new ARGBPixel((byte) (b << 4));
                    }
                }
            }
        }

        protected override void EncodeBlock(ARGBPixel* sPtr, VoidPtr blockAddr, int width)
        {
            byte* stPtr = (byte*) sPtr;
            byte* dPtr = (byte*) blockAddr;
            for (int y = 0; y < BlockHeight; y++, stPtr += width / 2)
            {
                for (int x = 0; x < BlockWidth / 2;)
                {
                    *dPtr++ = stPtr[x++];
                }
            }

            //*dPtr++ = (byte)((_workingPalette.FindMatch(sPtr[x++]) << 4) | (_workingPalette.FindMatch(sPtr[x++]) & 0x0F));
        }
    }
}