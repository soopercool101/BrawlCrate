using System;
using BrawlLib.Imaging;

namespace BrawlLib.Wii.Textures
{
    internal unsafe class CI8 : TextureConverter
    {
        public override int BitsPerPixel => 8;
        public override int BlockWidth => 8;

        public override int BlockHeight => 4;

        //public override PixelFormat DecodedFormat { get { return PixelFormat.Format8bppIndexed; } }
        public override WiiPixelFormat RawFormat => WiiPixelFormat.CI8;

        protected override void DecodeBlock(VoidPtr blockAddr, ARGBPixel* dPtr, int width)
        {
            var sPtr = (byte*) blockAddr;
            if (_workingPalette != null)
                for (var y = 0; y < BlockHeight; y++, dPtr += width)
                for (var x = 0; x < BlockWidth; x++)
                {
                    var b = *sPtr++;
                    if (b >= _workingPalette.Entries.Length)
                        dPtr[x] = new ARGBPixel();
                    else
                        dPtr[x] = (ARGBPixel) _workingPalette.Entries[b];
                }
            else
                for (var y = 0; y < BlockHeight; y++, dPtr += width)
                for (var x = 0; x < BlockWidth; x++)
                    dPtr[x] = new ARGBPixel(*sPtr++);
        }

        protected override void EncodeBlock(ARGBPixel* sPtr, VoidPtr blockAddr, int width)
        {
            var stPtr = (byte*) sPtr;
            var dPtr = (byte*) blockAddr;
            for (var y = 0; y < BlockHeight; y++, stPtr += width)
            for (var x = 0; x < BlockWidth;)
                *dPtr++ = stPtr[x++];
        }
    }
}