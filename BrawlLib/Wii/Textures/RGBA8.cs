using BrawlLib.Imaging;
using BrawlLib.Internal;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Textures
{
    internal unsafe class RGBA8 : TextureConverter
    {
        public override int BitsPerPixel => 32;
        public override int BlockWidth => 4;

        public override int BlockHeight => 4;

        //public override PixelFormat DecodedFormat { get { return PixelFormat.Format32bppArgb; } }
        public override WiiPixelFormat RawFormat => WiiPixelFormat.RGBA8;

        protected override void DecodeBlock(VoidPtr blockAddr, ARGBPixel* dPtr, int width, ColorPalette palette = null)
        {
            byte* s1 = (byte*) blockAddr;
            byte* s2 = s1 + 32;
            byte* d2 = (byte*) dPtr;
            for (int y = 0; y < 4; y++, d2 += (width - 4) << 2)
            {
                for (int x = 0; x < 4; x++, d2 += 4)
                {
                    d2[3] = *s1++;
                    d2[2] = *s1++;
                    d2[1] = *s2++;
                    d2[0] = *s2++;
                }
            }

            //RGBA8Pixel* sPtr = (RGBA8Pixel*)blockAddr;
            ////ARGBPixel* dPtr = (ARGBPixel*)destAddr;
            //for (int y = 0; y < 4; y++, dPtr += width)
            //    for (int x = 0; x < 4; sPtr = (RGBA8Pixel*)((int)sPtr + 2) )
            //        dPtr[x++] = (ARGBPixel)(*sPtr);
        }

        protected override void EncodeBlock(ARGBPixel* sPtr, VoidPtr blockAddr, int width)
        {
            RGBA8Pixel* dPtr = (RGBA8Pixel*) blockAddr;
            for (int y = 0; y < BlockHeight; y++, sPtr += width)
            {
                for (int x = 0; x < BlockWidth; dPtr = dPtr->Increase())
                {
                    dPtr->Set(&sPtr[x++]);
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct RGBA8Pixel
    {
        public byte A;
        public byte R;
        private fixed byte _padding[30];
        public byte G;
        public byte B;

        public RGBA8Pixel* Increase()
        {
            fixed (RGBA8Pixel* ptr = &this)
            {
                return (RGBA8Pixel*) ((uint) ptr + 2);
            }
        }

        public RGBA8Pixel* Jump(int num)
        {
            fixed (RGBA8Pixel* ptr = &this)
            {
                return (RGBA8Pixel*) ((uint) ptr + (num << 1));
            }
        }

        public static explicit operator ARGBPixel(RGBA8Pixel p)
        {
            return new ARGBPixel {A = p.A, R = p.R, G = p.G, B = p.B};
        }

        public void Set(ARGBPixel* p)
        {
            A = p->A;
            R = p->R;
            G = p->G;
            B = p->B;
        }
    }
}