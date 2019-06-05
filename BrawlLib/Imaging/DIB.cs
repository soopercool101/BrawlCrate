using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace BrawlLib.Imaging
{
    internal class DIB : IDisposable
    {
        public DIB(int width, int height, PixelFormat format)
        {
            Width = width;
            Height = height;
            PixelFormat = format;

            Stride = (width * Image.GetPixelFormatSize(format)).Align(32) / 8;
            Scan0 = Marshal.AllocHGlobal(Stride * Height);
        }

        public DIB(int width, int height, int wAlign, int hAlign, PixelFormat format)
        {
            Width = width;
            Height = height;
            PixelFormat = format;

            Stride = (width.Align(wAlign) * Image.GetPixelFormatSize(format)).Align(8) / 8;
            Scan0 = Marshal.AllocHGlobal(Stride * height.Align(hAlign));
        }

        public int Width { get; }

        public int Height { get; }

        public int Stride { get; }

        public PixelFormat PixelFormat { get; }

        public IntPtr Scan0 { get; private set; }

        public BitmapData BitmapData
        {
            get
            {
                var data = new BitmapData
                {
                    Width = Width,
                    Height = Height,
                    Stride = Stride,
                    PixelFormat = PixelFormat,
                    Scan0 = Scan0
                };
                return data;
            }
        }

        public void Dispose()
        {
            if (Scan0 != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(Scan0);
                Scan0 = IntPtr.Zero;
            }
        }


        public static DIB FromBitmap(Bitmap src)
        {
            return FromBitmap(src, src.PixelFormat);
        }

        public static DIB FromBitmap(Bitmap src, PixelFormat format)
        {
            var dib = new DIB(src.Width, src.Height, format);
            dib.ReadBitmap(src);
            return dib;
        }

        public static DIB FromBitmap(Bitmap src, int wAlign, int hAlign, PixelFormat format)
        {
            var dib = new DIB(src.Width, src.Height, wAlign, hAlign, format);
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
            return ToBitmap(Width, Height);
        }

        internal Bitmap ToBitmap(int w, int h)
        {
            var bmp = new Bitmap(w, h, PixelFormat);
            WriteBitmap(bmp, w, h);
            return bmp;
        }

        internal void ReadBitmap(Bitmap src)
        {
            ReadBitmap(src, 0, 0, Width, Height);
        }

        internal void ReadBitmap(Bitmap src, int w, int h)
        {
            ReadBitmap(src, 0, 0, w, h);
        }

        internal void ReadBitmap(Bitmap src, int x, int y, int width, int height)
        {
            var data = src.LockBits(new Rectangle(x, y, width, height), ImageLockMode.ReadOnly, PixelFormat);

            var sStride = (width * Image.GetPixelFormatSize(PixelFormat)).Align(8) / 8;
            var xStart = (x * Image.GetPixelFormatSize(PixelFormat)).Align(8) / 8;
            for (var y2 = 0; y2 < height; y2++)
            {
                VoidPtr dPtr = (int) Scan0 + Stride * (y + y2) + xStart;
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
            var data = dst.LockBits(new Rectangle(x, y, width, height), ImageLockMode.WriteOnly, PixelFormat);

            var sStride = (width * Image.GetPixelFormatSize(PixelFormat)).Align(8) / 8;
            var xStart = (x * Image.GetPixelFormatSize(PixelFormat)).Align(8) / 8;
            for (var y2 = 0; y2 < height; y2++)
            {
                VoidPtr sPtr = (int) Scan0 + Stride * (y + y2) + xStart;
                VoidPtr dPtr = (int) data.Scan0 + data.Stride * (y + y2) + xStart;
                Memory.Move(dPtr, sPtr, (uint) sStride);
            }

            dst.UnlockBits(data);
            //dst.UnlockBits(dst.LockBits(new Rectangle(x, y, width, height), ImageLockMode.UserInputBuffer | ImageLockMode.WriteOnly, _format, BitmapData));
        }

        internal void ReadRaw(VoidPtr dest, int x, int y, int width, int height)
        {
            var bpp = Image.GetPixelFormatSize(PixelFormat);
            var linelen = (width * bpp) >> 3;

            VoidPtr src = ((x * bpp) >> 3) + y * Stride;


            for (var i = 0; i < height; i++, dest += linelen, src += Stride) Memory.Move(dest, src, (uint) linelen);
        }
    }
}