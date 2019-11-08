using BrawlLib.Imaging;
using BrawlLib.Internal;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Textures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct wRGBXPixel
    {
        public byte R, G, B, X;

        public static explicit operator wRGBXPixel(ARGBPixel p)
        {
            return new wRGBXPixel {R = p.R, G = p.G, B = p.B, X = 0};
        }

        public static explicit operator ARGBPixel(wRGBXPixel p)
        {
            return new ARGBPixel {A = 0xFF, R = p.R, G = p.G, B = p.B};
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct wRGBAPixel
    {
        public byte R, G, B, A;

        public static explicit operator wRGBAPixel(ARGBPixel p)
        {
            return new wRGBAPixel {A = p.A, R = p.R, G = p.G, B = p.B};
        }

        public static explicit operator ARGBPixel(wRGBAPixel p)
        {
            return new ARGBPixel {A = p.A, R = p.R, G = p.G, B = p.B};
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct wRGBA4Pixel
    {
        private bushort _data;

        public static explicit operator ARGBPixel(wRGBA4Pixel p)
        {
            int val = p._data;
            int r = val & 0xF000;
            r = (r >> 8) | (r >> 12);
            int g = val & 0x0F00;
            g = (g >> 4) | (g >> 8);
            int b = val & 0x00F0;
            b |= b >> 4;
            int a = val & 0x000F;
            a |= a << 4;
            return new ARGBPixel((byte) a, (byte) r, (byte) g, (byte) b);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct wRGBA6Pixel
    {
        private readonly byte _b1, _b2, _b3;

        public static explicit operator ARGBPixel(wRGBA6Pixel p)
        {
            int val = (p._b1 << 16) | (p._b2 << 8) | p._b3;
            int r = val & 0xFC0000;
            r = (r >> 16) | (r >> 22);
            int g = val & 0x3F000;
            g = (g >> 10) | (g >> 16);
            int b = val & 0xFC0;
            b = (b >> 4) | (b >> 10);
            int a = val & 0x3F;
            a = (a << 2) | (a >> 4);
            return new ARGBPixel((byte) a, (byte) r, (byte) g, (byte) b);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct wRGBPixel
    {
        public byte R, G, B;

        public static explicit operator wRGBPixel(ARGBPixel p)
        {
            return new wRGBPixel {R = p.R, G = p.G, B = p.B};
        }

        public static explicit operator ARGBPixel(wRGBPixel p)
        {
            return new ARGBPixel {A = 0xFF, R = p.R, G = p.G, B = p.B};
        }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct wRGB565Pixel
    {
        public bushort _data;

        public wRGB565Pixel(bushort data)
        {
            _data = data;
        }

        public wRGB565Pixel(int r, int g, int b)
        {
            r = Convert.ToInt32(r * (31.0 / 255.0));
            g = Convert.ToInt32(g * (63.0 / 255.0));
            b = Convert.ToInt32(b * (31.0 / 255.0));
            _data = (ushort) ((r << 11) | (g << 5) | b);
        }

        public static bool operator >(wRGB565Pixel p1, wRGB565Pixel p2)
        {
            return p1._data > p2._data;
        }

        public static bool operator <(wRGB565Pixel p1, wRGB565Pixel p2)
        {
            return p1._data < p2._data;
        }

        public static bool operator >=(wRGB565Pixel p1, wRGB565Pixel p2)
        {
            return p1._data >= p2._data;
        }

        public static bool operator <=(wRGB565Pixel p1, wRGB565Pixel p2)
        {
            return p1._data <= p2._data;
        }

        public static bool operator ==(wRGB565Pixel p1, wRGB565Pixel p2)
        {
            return p1._data._data == p2._data._data;
        }

        public static bool operator !=(wRGB565Pixel p1, wRGB565Pixel p2)
        {
            return p1._data._data != p2._data._data;
        }

        public override bool Equals(object o)
        {
            return o is wRGB565Pixel && (wRGB565Pixel) o == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static explicit operator ARGBPixel(wRGB565Pixel p)
        {
            int r, g, b;
            ushort val = p._data;
            r = (val >> 11) & 0x1F;
            r = Convert.ToInt32(r * (255.0 / 31.0));
            g = (val >> 5) & 0x3F;
            g = Convert.ToInt32(g * (255.0 / 63.0));
            b = val & 0x1F;
            b = Convert.ToInt32(b * (255.0 / 31.0));
            return new ARGBPixel(0xFF, (byte) r, (byte) g, (byte) b);
        }

        public static explicit operator RGBPixel(wRGB565Pixel p)
        {
            int r, g, b;
            ushort val = p._data;
            r = (val >> 11) & 0x1F;
            r = Convert.ToInt32(r * (255.0 / 31.0));
            g = (val >> 5) & 0x3F;
            g = Convert.ToInt32(g * (255.0 / 63.0));
            b = val & 0x1F;
            b = Convert.ToInt32(b * (255.0 / 31.0));
            return new RGBPixel {R = (byte) r, G = (byte) g, B = (byte) b};
        }

        public static explicit operator Color(wRGB565Pixel p)
        {
            int r, g, b;
            ushort val = p._data;
            r = (val >> 11) & 0x1F;
            r = Convert.ToInt32(r * (255.0 / 31.0));
            g = (val >> 5) & 0x3F;
            g = Convert.ToInt32(g * (255.0 / 63.0));
            b = val & 0x1F;
            b = Convert.ToInt32(b * (255.0 / 31.0));
            return Color.FromArgb(0xFF, r, g, b);
        }

        public static explicit operator wRGB565Pixel(ARGBPixel p)
        {
            return new wRGB565Pixel(p.R, p.G, p.B);
        }

        public static explicit operator wRGB565Pixel(RGBPixel p)
        {
            return new wRGB565Pixel(p.R, p.G, p.B);
        }

        public static explicit operator wRGB565Pixel(Color p)
        {
            return new wRGB565Pixel(p.R, p.G, p.B);
        }

        public static explicit operator wRGB565Pixel(Vector3 v)
        {
            int r = Math.Max(Math.Min(Convert.ToInt32(v._x * 31.0f), 31), 0);
            int g = Math.Max(Math.Min(Convert.ToInt32(v._y * 63.0f), 63), 0);
            int b = Math.Max(Math.Min(Convert.ToInt32(v._z * 31.0f), 31), 0);
            return new wRGB565Pixel((ushort) ((r << 11) | (g << 5) | b));
        }

        //public uint ColorData()
        //{
        //    int r, g, b;
        //    ushort val = _data;
        //    r = val & 0xF800; r = (r >> 8) | (r >> 13);
        //    g = (val & 0x7E0); g = (g >> 3) | (g >> 9);
        //    b = (val & 0x1F); b = (b << 3) | (b >> 2);
        //    return (uint)((r << 16) | (g << 8) | b);
        //}
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct wRGB5A3Pixel
    {
        public bushort _data;

        public wRGB5A3Pixel(int a, int r, int g, int b)
        {
            a = Convert.ToInt32(a * (7.0 / 255.0));
            if (a == 7)
            {
                r = Convert.ToInt32(r * (31.0 / 255.0));
                g = Convert.ToInt32(g * (31.0 / 255.0));
                b = Convert.ToInt32(b * (31.0 / 255.0));
                _data = (ushort) ((1 << 15) | (r << 10) | (g << 5) | b);
            }
            else
            {
                r = Convert.ToInt32(r * (15.0 / 255.0));
                g = Convert.ToInt32(g * (15.0 / 255.0));
                b = Convert.ToInt32(b * (15.0 / 255.0));
                _data = (ushort) ((a << 12) | (r << 8) | (g << 4) | b);
            }
        }

        public static explicit operator ARGBPixel(wRGB5A3Pixel p)
        {
            int a, r, g, b;
            ushort val = p._data;
            if ((val & 0x8000) != 0)
            {
                a = 0xFF;
                r = (val >> 10) & 0x1F;
                r = Convert.ToInt32(r * (255.0 / 31.0));
                g = (val >> 5) & 0x1F;
                g = Convert.ToInt32(g * (255.0 / 31.0));
                b = val & 0x1F;
                b = Convert.ToInt32(b * (255.0 / 31.0));
            }
            else
            {
                a = (val >> 12) & 0x07;
                a = Convert.ToInt32(a * (255.0 / 7.0));
                r = val & 0xF00;
                r = (r >> 4) | (r >> 8);
                g = val & 0xF0;
                g |= g >> 4;
                b = val & 0x0F;
                b |= b << 4;
            }

            return new ARGBPixel {A = (byte) a, R = (byte) r, G = (byte) g, B = (byte) b};
        }

        public static explicit operator wRGB5A3Pixel(ARGBPixel p)
        {
            return new wRGB5A3Pixel(p.A, p.R, p.G, p.B);
        }

        public static explicit operator Color(wRGB5A3Pixel p)
        {
            int a, r, g, b;
            ushort val = p._data;
            if ((val & 0x8000) != 0)
            {
                a = 0xFF;
                r = (val >> 10) & 0x1F;
                r = Convert.ToInt32(r * (255.0 / 31.0));
                g = (val >> 5) & 0x1F;
                g = Convert.ToInt32(g * (255.0 / 31.0));
                b = val & 0x1F;
                b = Convert.ToInt32(b * (255.0 / 31.0));
            }
            else
            {
                a = (val >> 12) & 0x07;
                a = Convert.ToInt32(a * (255.0 / 7.0));
                r = val & 0xF00;
                r = (r >> 4) | (r >> 8);
                g = val & 0xF0;
                g |= g >> 4;
                b = val & 0x0F;
                b |= b << 4;
            }

            return Color.FromArgb((byte) a, (byte) r, (byte) g, (byte) b);
        }

        public static explicit operator wRGB5A3Pixel(Color p)
        {
            return new wRGB5A3Pixel(p.A, p.R, p.G, p.B);
        }
    }
}