using BrawlLib.Internal;
using BrawlLib.Internal.Windows.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace BrawlLib.Imaging
{
    [Serializable]
    [Editor(typeof(PropertyGridColorEditor), typeof(UITypeEditor))]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ARGBPixel : ISerializable
    {
        private const float ColorFactor = 1.0f / 255.0f;

        //This struct is little-endian for PixelFormat.Format32bppArgb
        public byte B, G, R, A;

        public ARGBPixel(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public ARGBPixel(byte intensity)
        {
            A = 255;
            R = intensity;
            G = intensity;
            B = intensity;
        }

        public ARGBPixel(SerializationInfo info, StreamingContext context)
        {
            B = info.GetByte("B");
            G = info.GetByte("G");
            R = info.GetByte("R");
            A = info.GetByte("A");
        }

        public int DistanceTo(Color c)
        {
            int val = A - c.A;
            int dist = val * val;
            val = R - c.R;
            dist += val * val;
            val = G - c.G;
            dist += val * val;
            val = B - c.B;
            dist += val * val;
            return dist;
        }

        public int DistanceTo(ARGBPixel p)
        {
            int val = A - p.A;
            int dist = val * val;
            val = R - p.R;
            dist += val * val;
            val = G - p.G;
            dist += val * val;
            val = B - p.B;
            return dist + val;
        }

        public float Luminance()
        {
            return 0.299f * R + 0.587f * G + 0.114f * B;
        }

        public bool IsGreyscale()
        {
            return R == G && G == B;
        }

        public int Greyscale()
        {
            return (R + G + B) / 3;
        }

        public static explicit operator ARGBPixel(int val)
        {
            return *(ARGBPixel*) &val;
        }

        public static explicit operator int(ARGBPixel p)
        {
            return *(int*) &p;
        }

        public static explicit operator ARGBPixel(uint val)
        {
            return *(ARGBPixel*) &val;
        }

        public static explicit operator uint(ARGBPixel p)
        {
            return *(uint*) &p;
        }

        public static explicit operator ARGBPixel(Color val)
        {
            return (ARGBPixel) val.ToArgb();
        }

        public static explicit operator Color(ARGBPixel p)
        {
            return Color.FromArgb((int) p);
        }

        public static explicit operator Vector3(ARGBPixel p)
        {
            return new Vector3(p.R * ColorFactor, p.G * ColorFactor, p.B * ColorFactor);
        }

        public static implicit operator Vector4(ARGBPixel p)
        {
            return new Vector4(p.R * ColorFactor, p.G * ColorFactor, p.B * ColorFactor, p.A * ColorFactor);
        }

        public ARGBPixel Min(ARGBPixel p)
        {
            return new ARGBPixel(Math.Min(A, p.A), Math.Min(R, p.R), Math.Min(G, p.G), Math.Min(B, p.B));
        }

        public ARGBPixel Max(ARGBPixel p)
        {
            return new ARGBPixel(Math.Max(A, p.A), Math.Max(R, p.R), Math.Max(G, p.G), Math.Max(B, p.B));
        }

        public static bool operator ==(ARGBPixel p1, ARGBPixel p2)
        {
            return *(uint*) &p1 == *(uint*) &p2;
        }

        public static bool operator !=(ARGBPixel p1, ARGBPixel p2)
        {
            return *(uint*) &p1 != *(uint*) &p2;
        }

        public override string ToString()
        {
            return $"A:{A} R:{R} G:{G} B:{B}";
        }

        public string ToHexString()
        {
            return $"A:{A:X2} R:{R:X2} G:{G:X2} B:{B:X2}";
        }

        public string ToPaddedString()
        {
            return $"A:{A,3} R:{R,3} G:{G,3} B:{B,3}";
        }

        public string ToARGBColorCode()
        {
            return $"{A:X2}{R:X2}{G:X2}{B:X2}";
        }

        public string ToRGBAColorCode()
        {
            return $"{R:X2}{G:X2}{B:X2}{A:X2}";
        }

        public override int GetHashCode()
        {
            return (int) this;
        }

        public override bool Equals(object obj)
        {
            if (obj is ARGBPixel)
            {
                return (ARGBPixel) obj == this;
            }

            return false;
        }

        internal ARGBPixel Inverse()
        {
            return new ARGBPixel(A, (byte) (255 - R), (byte) (255 - G), (byte) (255 - B));
        }

        internal ARGBPixel Lighten(int amount)
        {
            return new ARGBPixel(A, (byte) Math.Min(R + amount, 255), (byte) Math.Min(G + amount, 255),
                (byte) Math.Min(B + amount, 255));
        }

        internal ARGBPixel Darken(int amount)
        {
            return new ARGBPixel(A, (byte) Math.Max(R - amount, 0), (byte) Math.Max(G - amount, 0),
                (byte) Math.Max(B - amount, 0));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("B", B);
            info.AddValue("G", G);
            info.AddValue("R", R);
            info.AddValue("A", A);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct HSVPixel
    {
        public ushort H;
        public byte S, V;

        public HSVPixel(ushort h, byte s, byte v)
        {
            H = h;
            S = s;
            V = v;
        }

        public static explicit operator HSVPixel(ARGBPixel p)
        {
            HSVPixel outp;

            int min = Math.Min(Math.Min(p.R, p.G), p.B);
            int max = Math.Max(Math.Max(p.R, p.G), p.B);
            int diff = max - min;

            if (diff == 0)
            {
                outp.H = 0;
                outp.S = 0;
            }
            else
            {
                if (max == p.R)
                {
                    outp.H = (ushort) ((60 * ((float) (p.G - p.B) / diff) + 360) % 360);
                }
                else if (max == p.G)
                {
                    outp.H = (ushort) (60 * ((float) (p.B - p.R) / diff) + 120);
                }
                else
                {
                    outp.H = (ushort) (60 * ((float) (p.R - p.G) / diff) + 240);
                }

                if (max == 0)
                {
                    outp.S = 0;
                }
                else
                {
                    outp.S = (byte) (diff * 100 / max);
                }
            }

            outp.V = (byte) (max * 100 / 255);

            return outp;
        }

        public static explicit operator ARGBPixel(HSVPixel pixel)
        {
            ARGBPixel newPixel;

            byte v = (byte) (pixel.V * 255 / 100);
            if (pixel.S == 0)
            {
                newPixel = new ARGBPixel(255, v, v, v);
            }
            else
            {
                int h = pixel.H / 60 % 6;
                float f = pixel.H / 60.0f - pixel.H / 60;

                byte p = (byte) (pixel.V * (100 - pixel.S) * 255 / 10000);
                byte q = (byte) (pixel.V * (100 - (int) (f * pixel.S)) * 255 / 10000);
                byte t = (byte) (pixel.V * (100 - (int) ((1.0f - f) * pixel.S)) * 255 / 10000);

                switch (h)
                {
                    case 0:
                        newPixel = new ARGBPixel(255, v, t, p);
                        break;
                    case 1:
                        newPixel = new ARGBPixel(255, q, v, p);
                        break;
                    case 2:
                        newPixel = new ARGBPixel(255, p, v, t);
                        break;
                    case 3:
                        newPixel = new ARGBPixel(255, p, q, v);
                        break;
                    case 4:
                        newPixel = new ARGBPixel(255, t, p, v);
                        break;
                    default:
                        newPixel = new ARGBPixel(255, v, p, q);
                        break;
                }
            }

            return newPixel;
        }

        public static explicit operator Color(HSVPixel p)
        {
            ARGBPixel np = (ARGBPixel) p;
            return Color.FromArgb(*(int*) &np);
        }

        public static explicit operator HSVPixel(Color c)
        {
            return (HSVPixel) (ARGBPixel) c;
        }
    }

    [Editor(typeof(PropertyGridColorEditor), typeof(UITypeEditor))]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RGBAPixel : IComparable //, ICustomTypeDescriptor
    {
        public const float ColorFactor = 1.0f / 255.0f;

        public byte R, G, B, A;

        public static implicit operator RGBAPixel(ARGBPixel p)
        {
            return new RGBAPixel {A = p.A, B = p.B, G = p.G, R = p.R};
        }

        public static implicit operator ARGBPixel(RGBAPixel p)
        {
            return new ARGBPixel {A = p.A, B = p.B, G = p.G, R = p.R};
        }

        public static explicit operator Color(RGBAPixel p)
        {
            ARGBPixel a = p;
            return Color.FromArgb((int) a);
        }

        public static implicit operator RGBAPixel(Vector4 p)
        {
            return new RGBAPixel((byte) (p._x * 255.0f), (byte) (p._y * 255.0f), (byte) (p._z * 255.0f),
                (byte) (p._w * 255.0f));
        }

        public static implicit operator Vector4(RGBAPixel p)
        {
            return new Vector4(p.R / 255.0f, p.G / 255.0f, p.B / 255.0f, p.A / 255.0f);
        }

        public static implicit operator Vector3(RGBAPixel p)
        {
            return new Vector3(p.R / 255.0f, p.G / 255.0f, p.B / 255.0f);
        }

        public static implicit operator RGBAPixel(uint u)
        {
            return new RGBAPixel(u);
        }

        public static implicit operator uint(RGBAPixel p)
        {
            return BitConverter.ToUInt32(new byte[] {p.R, p.G, p.B, p.A}, 0);
        }

        public RGBAPixel(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public RGBAPixel(uint u)
        {
            string temp = u.ToString("X8");
            R = Convert.ToByte(temp.Substring(0, 2), 16);
            G = Convert.ToByte(temp.Substring(2, 2), 16);
            B = Convert.ToByte(temp.Substring(4, 2), 16);
            A = Convert.ToByte(temp.Substring(6, 2), 16);
        }

        [Category("RGBA Pixel")]
        public byte Red
        {
            get => R;
            set => R = value;
        }

        [Category("RGBA Pixel")]
        public byte Green
        {
            get => G;
            set => G = value;
        }

        [Category("RGBA Pixel")]
        public byte Blue
        {
            get => B;
            set => B = value;
        }

        [Category("RGBA Pixel")]
        public byte Alpha
        {
            get => A;
            set => A = value;
        }
        //[Category("RGBA Pixel"), TypeConverter(typeof(RGBAStringConverter))]
        //public RGBAPixel Value { get { return this; } set { this = value; } }

        public override string ToString()
        {
            //return String.Format("R:{0:X2} G:{1:X2} B:{2:X2} A:{3:X2}", R, G, B, A);
            return $"R:{R} G:{G} B:{B} A:{A}";
        }

        public override int GetHashCode()
        {
            fixed (RGBAPixel* p = &this)
            {
                return *(int*) p;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is RGBAPixel)
            {
                return this == (RGBAPixel) obj;
            }

            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj is RGBAPixel)
            {
                RGBAPixel o = (RGBAPixel) obj;
                if (A > o.A)
                {
                    return 1;
                }

                if (A < o.A)
                {
                    return -1;
                }

                if (R > o.R)
                {
                    return 1;
                }

                if (R < o.R)
                {
                    return -1;
                }

                if (G > o.G)
                {
                    return 1;
                }

                if (G < o.G)
                {
                    return -1;
                }

                if (B > o.B)
                {
                    return 1;
                }

                if (B < o.B)
                {
                    return -1;
                }

                return 0;
            }

            return 1;
        }

        public static RGBAPixel operator &(RGBAPixel p1, RGBAPixel p2)
        {
            return new RGBAPixel((byte) (p1.R & p2.R), (byte) (p1.G & p2.G), (byte) (p1.B & p2.B),
                (byte) (p1.A & p2.A));
        }

        public static RGBAPixel operator |(RGBAPixel p1, RGBAPixel p2)
        {
            return new RGBAPixel((byte) (p1.R | p2.R), (byte) (p1.G | p2.G), (byte) (p1.B | p2.B),
                (byte) (p1.A | p2.A));
        }

        public static bool operator ==(RGBAPixel p1, RGBAPixel p2)
        {
            return *(int*) &p1 == *(int*) &p2;
        }

        public static bool operator !=(RGBAPixel p1, RGBAPixel p2)
        {
            return *(int*) &p1 != *(int*) &p2;
        }

        public static int Compare(RGBAPixel p1, RGBAPixel p2)
        {
            int v1 = *(int*) &p1;
            int v2 = *(int*) &p2;

            if (v1 > v2)
            {
                return 1;
            }

            if (v1 < v2)
            {
                return -1;
            }

            return 1;
        }

        [Browsable(false)]
        public VoidPtr Address
        {
            get
            {
                fixed (void* p = &this)
                {
                    return p;
                }
            }
        }

        public static RGBAPixel Parse(string s)
        {
            RGBAPixel p;
            byte* ptr = (byte*) &p;
            for (int i = 0; i < 8; i += 2)
            {
                *ptr++ = s.Length >= i + 2
                    ? byte.Parse(s.Substring(i, 2), System.Globalization.NumberStyles.HexNumber)
                    : (byte) (i == 6 ? 0xFF : 0);
            }

            return p;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RGBPixel
    {
        public byte B, G, R;

        public static explicit operator RGBPixel(ARGBPixel p)
        {
            return new RGBPixel {R = p.R, G = p.G, B = p.B};
        }

        public static explicit operator ARGBPixel(RGBPixel p)
        {
            return new ARGBPixel {A = 0xFF, R = p.R, G = p.G, B = p.B};
        }

        public static explicit operator Color(RGBPixel p)
        {
            return Color.FromArgb(p.R, p.G, p.B);
        }

        public static explicit operator RGBPixel(Color p)
        {
            return new RGBPixel {R = p.R, G = p.G, B = p.B};
        }

        public static RGBPixel FromIntensity(byte value)
        {
            return new RGBPixel {R = value, G = value, B = value};
        }

        public override string ToString()
        {
            return $"R:{R:X2} G:{G:X2} B:{B:X2}";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RGB555Pixel
    {
        public ushort _data;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ARGB15Pixel
    {
        public ushort _data;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct ColorF4
    {
        private const float ColorFactor = 1.0f / 255.0f;

        public float A;
        public float R;
        public float G;
        public float B;

        public ColorF4(float a, float r, float g, float b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public float DistanceTo(ColorF4 p)
        {
            float a = A - p.A;
            float r = R - p.R;
            float g = G - p.G;
            float b = B - p.B;
            return a * a + r * r + g * g + b * b;
        }

        public Color ToColor()
        {
            return Color.FromArgb((int) (A / ColorFactor + 0.5f), (int) (R / ColorFactor + 0.5f),
                (int) (G / ColorFactor + 0.5f), (int) (B / ColorFactor + 0.5f));
        }

        public static ColorF4 Factor(ColorF4 p1, ColorF4 p2, float factor)
        {
            float f1 = factor, f2 = 1.0f - factor;
            return new ColorF4(p1.A * f1 + p2.A * f2, p1.R * f1 + p2.R * f2, p1.G * f1 + p2.G * f2,
                p1.B * f1 + p2.B * f2);
        }

        public void Factor(ColorF4 p, float factor)
        {
            float f1 = 1.0f - factor, f2 = factor;
            A = A * f1 + p.A * f2;
            R = R * f1 + p.R * f2;
            G = G * f1 + p.G * f2;
            B = B * f1 + p.B * f2;
        }

        public static explicit operator ColorF4(ARGBPixel p)
        {
            return new ColorF4(p.A * ColorFactor, p.R * ColorFactor, p.G * ColorFactor, p.B * ColorFactor);
        }

        public static explicit operator ColorF4(GXColorS10 p)
        {
            return new ColorF4(p.A * ColorFactor, p.R * ColorFactor, p.G * ColorFactor, p.B * ColorFactor);
        }

        public static bool operator ==(ColorF4 p1, ColorF4 p2)
        {
            return p1.A == p2.A && p1.R == p2.R && p1.G == p2.G && p1.B == p2.B;
        }

        public static bool operator !=(ColorF4 p1, ColorF4 p2)
        {
            return p1.A != p2.A || p1.R != p2.R || p1.G != p2.G || p1.B != p2.B;
        }

        public static ColorF4 operator *(ColorF4 c1, ColorF4 c2)
        {
            return new ColorF4(c1.A * c2.A, c1.R * c2.R, c1.G * c2.G, c1.B * c2.B);
        }

        public static ColorF4 operator *(ColorF4 c1, float f)
        {
            return new ColorF4(c1.A * f, c1.R * f, c1.G * f, c1.B * f);
        }

        public static ColorF4 operator *(float f, ColorF4 c1)
        {
            return new ColorF4(c1.A * f, c1.R * f, c1.G * f, c1.B * f);
        }

        public static ColorF4 operator +(ColorF4 c1, ColorF4 c2)
        {
            return new ColorF4(c1.A + c2.A, c1.R + c2.R, c1.G + c2.G, c1.B + c2.B);
        }

        public static ColorF4 operator +(float f, ColorF4 c2)
        {
            return new ColorF4(f + c2.A, f + c2.R, f + c2.G, f + c2.B);
        }

        public static ColorF4 operator +(ColorF4 c2, float f)
        {
            return new ColorF4(f + c2.A, f + c2.R, f + c2.G, f + c2.B);
        }

        public static ColorF4 operator -(ColorF4 c1, ColorF4 c2)
        {
            return new ColorF4(c1.A - c2.A, c1.R - c2.R, c1.G - c2.G, c1.B - c2.B);
        }

        public static ColorF4 operator -(float f, ColorF4 c2)
        {
            return new ColorF4(f - c2.A, f - c2.R, f - c2.G, f - c2.B);
        }

        public static ColorF4 operator -(ColorF4 c2, float f)
        {
            return new ColorF4(f - c2.A, f - c2.R, f - c2.G, f - c2.B);
        }

        public static ColorF4 operator /(ColorF4 c1, float f)
        {
            return new ColorF4(c1.A / f, c1.R / f, c1.G / f, c1.B / f);
        }

        public static ColorF4 operator /(ColorF4 c1, ColorF4 c2)
        {
            return new ColorF4(c1.A / c2.A, c1.R / c2.R, c1.G / c2.G, c1.B / c2.B);
        }

        public static ColorF4 operator /(float f, ColorF4 c1)
        {
            return new ColorF4(c1.A / f, c1.R / f, c1.G / f, c1.B / f);
        }

        public override bool Equals(object obj)
        {
            if (obj is ColorF4)
            {
                return this == (ColorF4) obj;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [Editor(typeof(PropertyGridColorEditor), typeof(UITypeEditor))]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GXColorS10
    {
        public const float ColorFactor = 1.0f / 255.0f;

        public short R, G, B, A;

        public static explicit operator GXColorS10(ARGBPixel p)
        {
            return new GXColorS10 {A = p.A, B = p.B, G = p.G, R = p.R};
        }

        public static explicit operator ARGBPixel(GXColorS10 p)
        {
            return new ARGBPixel
                {A = (byte) (p.A & 0xFF), B = (byte) (p.B & 0xFF), G = (byte) (p.G & 0xFF), R = (byte) (p.R & 0xFF)};
        }

        public static explicit operator RGBAPixel(GXColorS10 p)
        {
            return new RGBAPixel
                {A = (byte) (p.A & 0xFF), B = (byte) (p.B & 0xFF), G = (byte) (p.G & 0xFF), R = (byte) (p.R & 0xFF)};
        }

        public static implicit operator Vector4(GXColorS10 p)
        {
            return new Vector4(p.R / 255.0f, p.G / 255.0f, p.B / 255.0f, p.A / 255.0f);
        }

        public GXColorS10(short a, short r, short g, short b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        [Category("RGBA Pixel")]
        public short Red
        {
            get => R;
            set => R = value;
        }

        [Category("RGBA Pixel")]
        public short Green
        {
            get => G;
            set => G = value;
        }

        [Category("RGBA Pixel")]
        public short Blue
        {
            get => B;
            set => B = value;
        }

        [Category("RGBA Pixel")]
        public short Alpha
        {
            get => A;
            set => A = value;
        }

        public override string ToString()
        {
            //return String.Format("R:{0:X2} G:{1:X2} B:{2:X2} A:{3:X2}", R, G, B, A);
            return $"R:{R} G:{G} B:{B} A:{A}";
        }

        public static bool operator ==(GXColorS10 p1, GXColorS10 p2)
        {
            return p1.A == p2.A && p1.R == p2.R && p1.G == p2.G && p1.B == p2.B;
        }

        public static bool operator !=(GXColorS10 p1, GXColorS10 p2)
        {
            return p1.A != p2.A || p1.R != p2.R || p1.G != p2.G || p1.B != p2.B;
        }

        public static GXColorS10 operator *(GXColorS10 c1, GXColorS10 c2)
        {
            return new GXColorS10((short) (c1.A * c2.A), (short) (c1.R * c2.R), (short) (c1.G * c2.G),
                (short) (c1.B * c2.B));
        }

        public static GXColorS10 operator *(GXColorS10 c1, float f)
        {
            return new GXColorS10((short) (c1.A * f), (short) (c1.R * f), (short) (c1.G * f), (short) (c1.B * f));
        }

        public static GXColorS10 operator +(GXColorS10 c1, GXColorS10 c2)
        {
            return new GXColorS10((short) (c1.A + c2.A), (short) (c1.R + c2.R), (short) (c1.G + c2.G),
                (short) (c1.B + c2.B));
        }

        public static GXColorS10 operator -(GXColorS10 c1, GXColorS10 c2)
        {
            return new GXColorS10((short) (c1.A - c2.A), (short) (c1.R - c2.R), (short) (c1.G - c2.G),
                (short) (c1.B - c2.B));
        }

        public static GXColorS10 operator -(float f, GXColorS10 c2)
        {
            return new GXColorS10((short) (f - c2.A), (short) (f - c2.R), (short) (f - c2.G), (short) (f - c2.B));
        }

        public static GXColorS10 operator -(GXColorS10 c2, float f)
        {
            return new GXColorS10((short) (f - c2.A), (short) (f - c2.R), (short) (f - c2.G), (short) (f - c2.B));
        }

        public static GXColorS10 operator +(float f, GXColorS10 c2)
        {
            return new GXColorS10((short) (f + c2.A), (short) (f + c2.R), (short) (f + c2.G), (short) (f + c2.B));
        }

        public static GXColorS10 operator +(GXColorS10 c2, float f)
        {
            return new GXColorS10((short) (f + c2.A), (short) (f + c2.R), (short) (f + c2.G), (short) (f + c2.B));
        }

        public static GXColorS10 operator /(GXColorS10 c1, float f)
        {
            return new GXColorS10((short) (c1.A / f), (short) (c1.R / f), (short) (c1.G / f), (short) (c1.B / f));
        }

        public static explicit operator Color(GXColorS10 p)
        {
            ARGBPixel a = (ARGBPixel) p;
            return Color.FromArgb((int) a);
        }

        public void Clamp(short min, short max)
        {
            A = A > max ? max : A < min ? min : A;
            R = R > max ? max : R < min ? min : R;
            G = G > max ? max : G < min ? min : G;
            B = B > max ? max : B < min ? min : B;
        }

        //Is used when "clamped" by tev stage
        public void CutoffTo8bit()
        {
            A = (short) (A & 0xFF);
            R = (short) (R & 0xFF);
            G = (short) (G & 0xFF);
            B = (short) (B & 0xFF);
        }

        [Browsable(false)]
        public VoidPtr Address
        {
            get
            {
                fixed (void* p = &this)
                {
                    return p;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is GXColorS10)
            {
                return this == (GXColorS10) obj;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}