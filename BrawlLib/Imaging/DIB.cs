using BrawlLib.Internal;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace BrawlLib.Imaging
{
    internal class DIB : IDisposable
    {
        private readonly int _width, _height, _stride;
        private readonly PixelFormat _format;
        private IntPtr _scan0;

        public int Width => _width;
        public int Height => _height;
        public int Stride => _stride;
        public PixelFormat PixelFormat => _format;
        public IntPtr Scan0 => _scan0;

        public BitmapData BitmapData
        {
            get
            {
                BitmapData data = new BitmapData
                {
                    Width = _width,
                    Height = _height,
                    Stride = _stride,
                    PixelFormat = _format,
                    Scan0 = _scan0
                };
                return data;
            }
        }

        public DIB(int width, int height, PixelFormat format)
        {
            _width = width;
            _height = height;
            _format = format;

            _stride = (width * Image.GetPixelFormatSize(format)).Align(32) / 8;
            _scan0 = Marshal.AllocHGlobal(_stride * _height);
        }

        public DIB(int width, int height, int wAlign, int hAlign, PixelFormat format)
        {
            _width = width;
            _height = height;
            _format = format;

            _stride = (width.Align(wAlign) * Image.GetPixelFormatSize(format)).Align(8) / 8;
            _scan0 = Marshal.AllocHGlobal(_stride * height.Align(hAlign));
        }


        public static DIB FromBitmap(Bitmap src)
        {
            return FromBitmap(src, src.PixelFormat);
        }

        public static DIB FromBitmap(Bitmap src, PixelFormat format)
        {
            DIB dib = new DIB(src.Width, src.Height, format);
            dib.ReadBitmap(src);
            return dib;
        }

        public static DIB FromBitmap(Bitmap src, int wAlign, int hAlign, PixelFormat format)
        {
            DIB dib = new DIB(src.Width, src.Height, wAlign, hAlign, format);
            dib.ReadBitmap(src);
            return dib;
        }

        public void BltRead(Bitmap bmp)
        {
        }

        ~DIB()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_scan0 != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_scan0);
                _scan0 = IntPtr.Zero;
            }
        }

        //public DIB GenerateMipMap(uint level, int blockWidth, int blockHeight)
        //{
        //    int w = _width, h = _height;
        //    for(int i = 1 ; i < level ; i++)
        //    {
        //        w = Math.Max(1, w >>1);
        //        h = Math.Max(1, h >>1);
        //    }
        //    int aw = w.Align(blockWidth), ah = h.Align(blockHeight);

        //    DIB dst = new DIB(aw, ah, _format);

        //    float step = (_width - 1.0f) / (w - 1.0f);

        //    float sy = 0.5f;
        //    for (int dy = 0; dy < w; dy++, sy += step, y = (int)step)
        //    {
        //        for (int x = 0; x < h; x++)
        //        {
        //        }
        //    }

        //    return dst;
        //}


        internal Bitmap ToBitmap()
        {
            return ToBitmap(_width, _height);
        }

        internal Bitmap ToBitmap(int w, int h)
        {
            Bitmap bmp = new Bitmap(w, h, _format);
            WriteBitmap(bmp, w, h);
            return bmp;
        }

        internal void ReadBitmap(Bitmap src)
        {
            ReadBitmap(src, 0, 0, _width, _height);
        }

        internal void ReadBitmap(Bitmap src, int w, int h)
        {
            ReadBitmap(src, 0, 0, w, h);
        }

        internal void ReadBitmap(Bitmap src, int x, int y, int width, int height)
        {
            BitmapData data = src.LockBits(new Rectangle(x, y, width, height), ImageLockMode.ReadOnly, _format);

            int sStride = (width * Image.GetPixelFormatSize(_format)).Align(8) / 8;
            int xStart = (x * Image.GetPixelFormatSize(_format)).Align(8) / 8;
            for (int y2 = 0; y2 < height; y2++)
            {
                VoidPtr dPtr = (int) _scan0 + _stride * (y + y2) + xStart;
                VoidPtr sPtr = (int) data.Scan0 + data.Stride * (y + y2) + xStart;
                Memory.Move(dPtr, sPtr, (uint) sStride);
            }

            src.UnlockBits(data);
            //src.UnlockBits(src.LockBits(new Rectangle(x, y, width, height), ImageLockMode.UserInputBuffer | ImageLockMode.ReadOnly, _format, BitmapData));
        }

        internal void WriteBitmap(Bitmap dst, int width, int height)
        {
            WriteBitmap(dst, 0, 0, width, height);
        }

        internal void WriteBitmap(Bitmap dst, int x, int y, int width, int height)
        {
            BitmapData data = dst.LockBits(new Rectangle(x, y, width, height), ImageLockMode.WriteOnly, _format);

            int sStride = (width * Image.GetPixelFormatSize(_format)).Align(8) / 8;
            int xStart = (x * Image.GetPixelFormatSize(_format)).Align(8) / 8;
            for (int y2 = 0; y2 < height; y2++)
            {
                VoidPtr sPtr = (int) _scan0 + _stride * (y + y2) + xStart;
                VoidPtr dPtr = (int) data.Scan0 + data.Stride * (y + y2) + xStart;
                Memory.Move(dPtr, sPtr, (uint) sStride);
            }

            dst.UnlockBits(data);
            //dst.UnlockBits(dst.LockBits(new Rectangle(x, y, width, height), ImageLockMode.UserInputBuffer | ImageLockMode.WriteOnly, _format, BitmapData));
        }

        internal void ReadRaw(VoidPtr dest, int x, int y, int width, int height)
        {
            int bpp = Image.GetPixelFormatSize(_format);
            int linelen = (width * bpp) >> 3;

            VoidPtr src = ((x * bpp) >> 3) + y * _stride;


            for (int i = 0; i < height; i++, dest += linelen, src += _stride)
            {
                Memory.Move(dest, src, (uint) linelen);
            }
        }
    }
}