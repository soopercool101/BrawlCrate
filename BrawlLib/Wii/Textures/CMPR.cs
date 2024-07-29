using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Textures
{
    public unsafe class CMPR : TextureConverter
    {
        public override int BitsPerPixel => 4;
        public override int BlockWidth => 8;

        public override int BlockHeight => 8;

        //public override PixelFormat DecodedFormat { get { return PixelFormat.Format32bppArgb; } }
        public override WiiPixelFormat RawFormat => WiiPixelFormat.CMPR;

        private UnsafeBuffer _blockBuffer;
        //private List<CMPBlock> _blockCache = new List<CMPBlock>();
        //private int _blockIndex;

        protected override void DecodeBlock(VoidPtr blockAddr, ARGBPixel* dPtr, int width, ColorPalette palette = null)
        {
            CMPRBlock* sPtr = (CMPRBlock*) blockAddr;
            //ARGBPixel* dPtr = (ARGBPixel*)destAddr;

            //int index = 0;
            for (int y = 0; y < 8; y += 4)
            {
                for (int x = 0; x < 8; x += 4, sPtr++)
                {
                    sPtr->Decode(&dPtr[y * width + x], width);
                }
            }

            //DXT1.DecodeBlock(&sPtr[index++], &dPtr[(y * width) + x], width);
        }

        //public override FileView EncodeTexture(Bitmap src, uint mipLevels, out FileView paletteFile)
        //{
        //    _blockIndex = 0;
        //    return base.EncodeTexture(src, mipLevels, out paletteFile);
        //}
        internal FileMap EncodeTPLTextureCached(Bitmap src, int mipLevels, UnsafeBuffer blockBuffer)
        {
            _blockBuffer = blockBuffer;
            try
            {
                return base.EncodeTPLTexture(src, mipLevels);
            }
            finally
            {
                _blockBuffer = null;
            }
        }

        public FileMap EncodeREFTTextureCached(Bitmap src, int mipLevels, UnsafeBuffer blockBuffer)
        {
            _blockBuffer = blockBuffer;
            try
            {
                return base.EncodeREFTTexture(src, mipLevels, WiiPaletteFormat.IA8);
            }
            finally
            {
                _blockBuffer = null;
            }
        }

        public FileMap EncodeTEX0TextureCached(Bitmap src, int mipLevels, UnsafeBuffer blockBuffer)
        {
            _blockBuffer = blockBuffer;
            try
            {
                return base.EncodeTEX0Texture(src, mipLevels);
            }
            finally
            {
                _blockBuffer = null;
            }
        }

        public UnsafeBuffer GeneratePreview(Bitmap bmp)
        {
            //_blockCache.Clear();
            int w = bmp.Width, h = bmp.Height;
            int aw = w.Align(BlockWidth), ah = h.Align(BlockHeight);

            UnsafeBuffer buffer = new UnsafeBuffer(aw / 4 * (ah / 4) * 8);
            CMPRBlock* bPtr = (CMPRBlock*) buffer.Address;

            //using (Bitmap bmp = src.Clone(new Rectangle(0, 0, aw, ah), PixelFormat.Format32bppArgb))
            //{
            //    BitmapData data = bmp.LockBits(new Rectangle(0,0,aw,ah), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            //    for (int y1 = 0; y1 < ah; y1 += 8)
            //        for (int x1 = 0; x1 < aw; x1 += 8)
            //            for (int y = 0; y < 8; y += 4)
            //                for (int x = 0; x < 8; x += 4)
            //                {
            //                    ARGBPixel* ptr = (ARGBPixel*)dib.Scan0 + (((y1 + y) * aw) + (x1 + x));
            //                    CMPBlock block = CMPBlock.Encode(ptr, aw, false);
            //                    _blockCache.Add(block);
            //                    block.Decode(ptr, aw);
            //                }
            //}

            using (DIB dib = DIB.FromBitmap(bmp, BlockWidth, BlockHeight, PixelFormat.Format32bppArgb))
            {
                ARGBPixel* img = (ARGBPixel*) dib.Scan0;
                for (int y1 = 0; y1 < ah; y1 += 8)
                {
                    for (int x1 = 0; x1 < aw; x1 += 8)
                    {
                        for (int y = 0; y < 8; y += 4)
                        {
                            for (int x = 0; x < 8; x += 4)
                            {
                                *bPtr = NVDXT.compressDXT1a(img, x1 + x, y1 + y, aw, ah);
                                bPtr->Decode(img, x1 + x, y1 + y, aw, ah);
                                bPtr++;
                            }
                        }
                    }
                }

                dib.WriteBitmap(bmp, w, h);
            }

            return buffer;
        }

        //internal override void EncodeLevel(TEX0v1* header, DIB dib, Bitmap src, int dStep, int sStep, int level)
        //{
        //    if ((level == 1) && (_blockBuffer != null))
        //    {
        //        CMPRBlock* sPtr = (CMPRBlock*)_blockBuffer.Address;
        //        CMPRBlock* dPtr = (CMPRBlock*)header->PixelData;

        //        int blocks = _blockBuffer.Length / 8;
        //        for (int i = 0; i < blocks; i++)
        //            dPtr[i] = sPtr[i];
        //    }
        //    else
        //        base.EncodeLevel(header, dib, src, dStep, sStep, level);
        //}

        internal override void EncodeLevel(VoidPtr addr, DIB dib, Bitmap src, int dStep, int sStep, int level)
        {
            if (level == 1 && _blockBuffer != null)
            {
                CMPRBlock* sPtr = (CMPRBlock*) _blockBuffer.Address;
                CMPRBlock* dPtr = (CMPRBlock*) addr;

                int blocks = _blockBuffer.Length / 8;
                for (int i = 0; i < blocks; i++)
                {
                    dPtr[i] = sPtr[i];
                }
            }
            else
            {
                base.EncodeLevel(addr, dib, src, dStep, sStep, level);
            }
        }

        protected override void EncodeBlock(ARGBPixel* sPtr, VoidPtr blockAddr, int width)
        {
            CMPRBlock* dPtr = (CMPRBlock*) blockAddr;
            for (int y = 0; y < 2; y++, sPtr += width * 4)
            {
                for (int x = 0; x < 8; x += 4)
                {
                    *dPtr++ = CMPRBlock.Encode(&sPtr[x], width, false);
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CMPRBlock
    {
        public wRGB565Pixel _root0;
        public wRGB565Pixel _root1;
        public buint _lookup;

        public void Decode(ARGBPixel* image, int imgX, int imgY, int imgW, int imgH)
        {
            Decode(image + (imgX + imgY * imgW), imgW);
        }

        public void Decode(ARGBPixel* block, int width)
        {
            uint* pixelData = stackalloc uint[4];
            ARGBPixel* pixel = (ARGBPixel*) pixelData;

            pixel[0] = (ARGBPixel) _root0;
            pixel[1] = (ARGBPixel) _root1;
            if (_root0._data > _root1._data)
            {
                pixel[2] = new ARGBPixel(255, (byte) (((pixel[0].R << 1) + pixel[1].R) / 3),
                    (byte) (((pixel[0].G << 1) + pixel[1].G) / 3), (byte) (((pixel[0].B << 1) + pixel[1].B) / 3));
                pixel[3] = new ARGBPixel(255, (byte) (((pixel[1].R << 1) + pixel[0].R) / 3),
                    (byte) (((pixel[1].G << 1) + pixel[0].G) / 3), (byte) (((pixel[1].B << 1) + pixel[0].B) / 3));
            }
            else
            {
                pixel[2] = new ARGBPixel(255, (byte) ((pixel[0].R + pixel[1].R) >> 1),
                    (byte) ((pixel[0].G + pixel[1].G) >> 1), (byte) ((pixel[0].B + pixel[1].B) >> 1));
                pixel[3] = new ARGBPixel();
            }

            uint lookup = _lookup;

            for (int y = 0, shift = 30; y < 4; y++, block += width)
            {
                for (int x = 0; x < 4; shift -= 2)
                {
                    block[x++] = pixel[(lookup >> shift) & 0x03];
                }
            }
        }


        public static CMPRBlock Encode(ARGBPixel* block, int width, bool fast)
        {
            CMPRBlock p = new CMPRBlock();

            uint* pData = stackalloc uint[16];
            ARGBPixel* pColor = (ARGBPixel*) pData;

            bool hasAlpha = false;
            for (int y = 0, i = 0; y < 4; y++, block += width)
            {
                for (int x = 0; x < 4; i++)
                {
                    pColor[i] = block[x++];
                    if (pData[i] != pData[0])
                    {
                    }

                    if (pColor[i].A < 0x80)
                    {
                        hasAlpha = true;
                    }
                }
            }

            /*
             *  Foreach block:
             *      copy block to buffer
             *      mirror remaning colors?
             *      
             *      If block is single color:
             *          run optiml compress?
             *      else
             *          Initialize color set
             *          Compress block using color set
             * 
             * 
             */

            //BlockDecoder decoder = new BlockDecoder(width, 4, 4, 4);
            //bool isSingle = true, hasAlpha = false, allAlpha = true;
            //for (int i = 0; i < 16; i++)
            //{
            //    int index = decoder[i];
            //    pColor[i] = block[index];
            //    if (pData[i] != pData[0]) isSingle = false;
            //    if (pColor[i].A < 0x80) hasAlpha = true;
            //    else allAlpha = false;
            //}

            //bool isSingle = true, hasAlpha = false, allAlpha = true;
            //ARGBPixel* ptr = block;
            //int index = 0;
            //for (int y = 0; y < 4; y++, ptr += width - 4)
            //{
            //    for (int x = 0; x < 4; x++, ptr++, index++)
            //    {
            //        pColor[index] = block[(y * width) + x];
            //        if (pData[0] != pData[index]) isSingle = false;
            //        if (pColor[index].A < 0x80) hasAlpha = true;
            //        else allAlpha = false;
            //    }
            //}

            //if (allAlpha)
            //{
            //    p._root0._data = 0;
            //    p._root1._data = 0xFFFF;
            //    p._lookup = 0xFFFFFFFF;
            //}
            //else if (isSingle)
            //{
            //    p._root0 = (RGB565Pixel)(*block);
            //    p._root1._data = 0;
            //    p._lookup = 0x00000000;
            //}
            //else
            //{
            uint* palData = stackalloc uint[4];
            ARGBPixel* palCol = (ARGBPixel*) palData;

            int bestDist = -1;
            for (int i = 0; i < 16; i++)
            {
                ARGBPixel p1 = pColor[i];
                for (int x = i + 1; x < 16; x++)
                {
                    ARGBPixel p2 = pColor[x];
                    int d = p1.DistanceTo(p2);
                    if (d > bestDist)
                    {
                        bestDist = d;
                        palCol[2] = p1;
                        palCol[3] = p2;
                    }
                }
            }

            wRGB565Pixel smax = (wRGB565Pixel) palCol[2];
            wRGB565Pixel smin = (wRGB565Pixel) palCol[3];

            if (smax < smin)
            {
                smax = (wRGB565Pixel) palCol[3];
                smin = (wRGB565Pixel) palCol[2];
            }

            if (hasAlpha)
            {
                p._root0 = smin;
                p._root1 = smax;
                palCol[0] = (ARGBPixel) smin;
                palCol[1] = (ARGBPixel) smax;
                palCol[2] = new ARGBPixel(255, (byte) ((palCol[0].R + palCol[1].R) >> 1),
                    (byte) ((palCol[0].G + palCol[1].G) >> 1), (byte) ((palCol[0].B + palCol[1].B) >> 1));
                palCol[3] = new ARGBPixel();
            }
            else
            {
                p._root0 = smax;
                p._root1 = smin;
                palCol[0] = (ARGBPixel) smax;
                palCol[1] = (ARGBPixel) smin;
                palCol[2] = new ARGBPixel(255, (byte) (((palCol[0].R << 1) + palCol[1].R) / 3),
                    (byte) (((palCol[0].G << 1) + palCol[1].G) / 3), (byte) (((palCol[0].B << 1) + palCol[1].B) / 3));
                palCol[3] = new ARGBPixel(255, (byte) (((palCol[1].R << 1) + palCol[0].R) / 3),
                    (byte) (((palCol[1].G << 1) + palCol[0].G) / 3), (byte) (((palCol[1].B << 1) + palCol[0].B) / 3));
            }

            uint indicies = 0;
            for (int i = 0, shift = 30; i < 16; i++, shift -= 2)
            {
                uint index = 3;
                if (pColor[i].A >= 0x80)
                {
                    int bd = int.MaxValue;
                    for (int x = 0; x < (hasAlpha ? 4 : 3); x++)
                    {
                        int dist = palCol[x].DistanceTo(pColor[i]);
                        if (dist < bd)
                        {
                            bd = dist;
                            index = (uint) x;
                        }
                    }
                }

                indicies |= index << shift;
            }

            p._lookup = indicies;

            //p = DXT1Fast.Compress(pColor);
            //}


            return p;
        }
    }
}