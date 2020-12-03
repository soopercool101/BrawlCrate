using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace BrawlLib.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Vector2 : IComparable
    {
        public float _x;
        public float X
        {
            get => _x;
            set => _x = value;
        }
        public float _y;
        public float Y
        {
            get => _y;
            set => _y = value;
        }

        public Vector2(float x, float y)
        {
            _x = x;
            _y = y;
        }

        public Vector2(float s)
        {
            _x = s;
            _y = s;
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v._x, -v._y);
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1._x + v2._x, v1._y + v2._y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1._x - v2._x, v1._y - v2._y);
        }

        public static Vector2 operator -(Vector2 v1, float f)
        {
            return new Vector2(v1._x - f, v1._y - f);
        }

        public static Vector2 operator *(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1._x * v2._x, v1._y * v2._y);
        }

        public static Vector2 operator *(Vector2 v1, float s)
        {
            return new Vector2(v1._x * s, v1._y * s);
        }

        public static Vector2 operator *(float s, Vector2 v1)
        {
            return new Vector2(v1._x * s, v1._y * s);
        }

        public static Vector2 operator /(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1._x / v2._x, v1._y / v2._y);
        }

        public static Vector2 operator /(Vector2 v1, float s)
        {
            return new Vector2(v1._x / s, v1._y / s);
        }

        public static Vector2 operator +(Vector2 v1, Vector3 v2)
        {
            return new Vector2(v1._x + v2._x, v1._y + v2._y);
        }

        public static Vector2 operator +(Vector3 v1, Vector2 v2)
        {
            return new Vector2(v1._x + v2._x, v1._y + v2._y);
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1._x == v2._x && v1._y == v2._y;
        }

        public static bool operator <=(Vector2 v1, Vector2 v2)
        {
            return v1._x <= v2._x && v1._y <= v2._y;
        }

        public static bool operator >=(Vector2 v1, Vector2 v2)
        {
            return v1._x >= v2._x && v1._y >= v2._y;
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return v1._x != v2._x || v1._y != v2._y;
        }

        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return v1._x * v2._x + v1._y * v2._y;
        }

        public float Dot(Vector2 v)
        {
            return _x * v._x + _y * v._y;
        }

        public static Vector2 Clamp(Vector2 v1, float min, float max)
        {
            v1.Clamp(min, max);
            return v1;
        }

        public void Clamp(float min, float max)
        {
            Max(min);
            Min(max);
        }

        public static Vector2 Min(Vector2 v1, Vector2 v2)
        {
            return new Vector2(Math.Min(v1._x, v2._x), Math.Min(v1._y, v2._y));
        }

        public static Vector2 Min(Vector2 v1, float f)
        {
            return new Vector2(Math.Min(v1._x, f), Math.Min(v1._y, f));
        }

        public void Min(Vector2 v)
        {
            if (_x > v._x)
            {
                _x = v._x;
            }

            if (_y > v._y)
            {
                _y = v._y;
            }
        }

        public void Min(float f)
        {
            _x = Math.Min(_x, f);
            _y = Math.Min(_y, f);
        }

        public static Vector2 Max(Vector2 v1, Vector2 v2)
        {
            return new Vector2(Math.Max(v1._x, v2._x), Math.Max(v1._y, v2._y));
        }

        public static Vector2 Max(Vector2 v1, float f)
        {
            return new Vector2(Math.Max(v1._x, f), Math.Max(v1._y, f));
        }

        public void Max(Vector2 v)
        {
            if (_x < v._x)
            {
                _x = v._x;
            }

            if (_y < v._y)
            {
                _y = v._y;
            }
        }

        public void Max(float f)
        {
            _x = Math.Max(_x, f);
            _y = Math.Max(_y, f);
        }

        public float DistanceTo(Vector2 v)
        {
            Vector2 v1 = this - v;
            return Dot(v1, v1);
        }

        public static Vector2 Lerp(Vector2 v1, Vector2 v2, float median)
        {
            return v1 * median + v2 * (1.0f - median);
        }

        public static explicit operator Vector2(Vector3 v)
        {
            return new Vector2(v._x, v._y);
        }

        public static explicit operator Vector3(Vector2 v)
        {
            return new Vector3(v._x, v._y, 0.0f);
        }

        public static explicit operator PointF(Vector2 v)
        {
            return new PointF(v._x, v._y);
        }

        public static explicit operator Vector2(PointF v)
        {
            return new Vector2(v.X, v.Y);
        }

        public static explicit operator Point(Vector2 v)
        {
            Vector2 t = Truncate(v);
            return new Point((int) t._x, (int) t._y);
        }

        public static explicit operator Vector2(Point v)
        {
            return new Vector2(v.X, v.Y);
        }

        public static Vector2 Truncate(Vector2 v)
        {
            return new Vector2(
                v._x > 0.0f ? (float) Math.Floor(v._x) : (float) Math.Ceiling(v._x),
                v._y > 0.0f ? (float) Math.Floor(v._y) : (float) Math.Ceiling(v._y));
        }

        public override string ToString()
        {
            return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Contains(",")
                ? $"({_x} {_y})"
                : $"({_x},{_y})";
        }

        public bool Contained(Vector2 start, Vector2 end, float expansion)
        {
            return Contained(this, start, end, expansion);
        }

        public static bool Contained(Vector2 point, Vector2 start, Vector2 end, float expansion)
        {
            float* sPtr = (float*) &point;
            float* s1 = (float*) &start, s2 = (float*) &end;
            float* temp;
            for (int i = 0; i < 2; i++)
            {
                if (s1[i] > s2[i])
                {
                    temp = s1;
                    s1 = s2;
                    s2 = temp;
                }

                if (sPtr[i] < s1[i] - expansion || sPtr[i] > s2[i] + expansion)
                {
                    return false;
                }
            }

            return true;
        }

        public float TrueDistance(Vector2 p)
        {
            float lenX = Math.Abs(p._x - _x);
            float lenY = Math.Abs(p._y - _y);

            if (lenX == 0.0f)
            {
                return lenY;
            }

            if (lenY == 0.0f)
            {
                return lenX;
            }

            return (float) (lenX / Math.Cos(Math.Atan(lenY / lenX)));
        }

        public void RemapToRange(float min, float max)
        {
            _x = _x.RemapToRange(min, max);
            _y = _y.RemapToRange(min, max);
        }

        public Vector2 RemappedToRange(float min, float max)
        {
            return new Vector2(_x.RemapToRange(min, max), _y.RemapToRange(min, max));
        }

        public override int GetHashCode()
        {
            fixed (Vector2* p = &this)
            {
                int* p2 = (int*) p;
                return p2[0] ^ p2[1];
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2 vector2)
            {
                return this == vector2;
            }

            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj is Vector2)
            {
                Vector2 o = (Vector2) obj;
                if (_x > o._x)
                {
                    return 1;
                }

                if (_x < o._x)
                {
                    return -1;
                }

                if (_y > o._y)
                {
                    return 1;
                }

                if (_y < o._y)
                {
                    return -1;
                }

                return 0;
            }

            return 1;
        }
    }
}