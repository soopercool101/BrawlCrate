using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace BrawlLib.Internal
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PartialVector3
    {
        public float? _x, _y, _z;

        public PartialVector3(float? x, float? y, float? z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public static explicit operator Vector3(PartialVector3 v)
        {
            return new Vector3(
                v._x ?? throw new Exception("Cannot cast to Vector3 (X value is missing)"),
                v._y ?? throw new Exception("Cannot cast to Vector3 (Y value is missing)"),
                v._z ?? throw new Exception("Cannot cast to Vector3 (Z value is missing)"));
        }

        public static implicit operator PartialVector3(Vector3 v)
        {
            return new PartialVector3(v._x, v._y, v._z);
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Vector3 : IComparable, ISerializable
    {
        public float _x, _y, _z;

        public Vector3(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public Vector3(float s)
        {
            _x = s;
            _y = s;
            _z = s;
        }

        public Vector3(SerializationInfo info, StreamingContext context)
        {
            _x = info.GetSingle("_x");
            _y = info.GetSingle("_y");
            _z = info.GetSingle("_z");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_x", _x);
            info.AddValue("_y", _y);
            info.AddValue("_z", _z);
        }

        public static explicit operator Vector3(Vector4 v)
        {
            if (v._w == 0.0f)
            {
                return new Vector3(v._x, v._y, v._z);
            }

            return new Vector3(v._x / v._w, v._y / v._w, v._z / v._w);
        }

        public static explicit operator Vector3(OpenTK.Vector3 v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static explicit operator OpenTK.Vector3(Vector3 v)
        {
            return new OpenTK.Vector3(v._x, v._y, v._z);
        }

        private const float _colorFactor = 1.0f / 255.0f;

        public static explicit operator Vector3(Color c)
        {
            return new Vector3(c.R * _colorFactor, c.G * _colorFactor, c.B * _colorFactor);
        }

        public static explicit operator Color(Vector3 v)
        {
            return Color.FromArgb((int) (v._x / _colorFactor), (int) (v._y / _colorFactor),
                (int) (v._z / _colorFactor));
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v._x, -v._y, -v._z);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1._x + v2._x, v1._y + v2._y, v1._z + v2._z);
        }

        public static Vector3 operator +(Vector3 v1, float f)
        {
            return new Vector3(v1._x + f, v1._y + f, v1._z + f);
        }

        public static Vector3 operator +(float f, Vector3 v1)
        {
            return new Vector3(v1._x + f, v1._y + f, v1._z + f);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1._x - v2._x, v1._y - v2._y, v1._z - v2._z);
        }

        public static Vector3 operator -(Vector3 v1, float f)
        {
            return new Vector3(v1._x - f, v1._y - f, v1._z - f);
        }

        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1._x * v2._x, v1._y * v2._y, v1._z * v2._z);
        }

        public static Vector3 operator *(Vector3 v1, float s)
        {
            return new Vector3(v1._x * s, v1._y * s, v1._z * s);
        }

        public static Vector3 operator *(float s, Vector3 v1)
        {
            return new Vector3(v1._x * s, v1._y * s, v1._z * s);
        }

        public static Vector3 operator /(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1._x / v2._x, v1._y / v2._y, v1._z / v2._z);
        }

        public static Vector3 operator /(Vector3 v1, float s)
        {
            return new Vector3(v1._x / s, v1._y / s, v1._z / s);
        }

        public static Vector3 operator /(float s, Vector3 v1)
        {
            return new Vector3(s / v1._x, s / v1._y, s / v1._z);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1._x == v2._x && v1._y == v2._y && v1._z == v2._z;
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return v1._x != v2._x || v1._y != v2._y || v1._z != v2._z;
        }

        public static bool operator <(Vector3 v1, Vector3 v2)
        {
            return v1._x < v2._x && v1._y < v2._y && v1._z < v2._z;
        }

        public static bool operator >(Vector3 v1, Vector3 v2)
        {
            return v1._x > v2._x && v1._y > v2._y && v1._z > v2._z;
        }

        public static bool operator <=(Vector3 v1, Vector3 v2)
        {
            return v1._x <= v2._x && v1._y <= v2._y && v1._z <= v2._z;
        }

        public static bool operator >=(Vector3 v1, Vector3 v2)
        {
            return v1._x >= v2._x && v1._y >= v2._y && v1._z >= v2._z;
        }

        public void Add(Vector3* v)
        {
            _x += v->_x;
            _y += v->_y;
            _z += v->_z;
        }

        public void Add(float v)
        {
            _x += v;
            _y += v;
            _z += v;
        }

        public void Sub(Vector3* v)
        {
            _x -= v->_x;
            _y -= v->_y;
            _z -= v->_z;
        }

        public void Sub(float v)
        {
            _x -= v;
            _y -= v;
            _z -= v;
        }

        public void Multiply(Vector3* v)
        {
            _x *= v->_x;
            _y *= v->_y;
            _z *= v->_z;
        }

        public void Multiply(float v)
        {
            _x *= v;
            _y *= v;
            _z *= v;
        }

        public static float* Mult(float* v1, float* v2)
        {
            v1[0] = v1[0] * v2[0];
            v1[1] = v1[1] * v2[1];
            v1[2] = v1[2] * v2[2];
            return v1;
        }

        public static float* Mult(float* v1, float v2)
        {
            v1[0] = v1[0] * v2;
            v1[1] = v1[1] * v2;
            v1[2] = v1[2] * v2;
            return v1;
        }

        public static float* Add(float* v1, float* v2)
        {
            v1[0] = v1[0] + v2[0];
            v1[1] = v1[1] + v2[1];
            v1[2] = v1[2] + v2[2];
            return v1;
        }

        public static float* Add(float* v1, float v2)
        {
            v1[0] = v1[0] + v2;
            v1[1] = v1[1] + v2;
            v1[2] = v1[2] + v2;
            return v1;
        }

        public static float* Sub(float* v1, float* v2)
        {
            v1[0] = v1[0] - v2[0];
            v1[1] = v1[1] - v2[1];
            v1[2] = v1[2] - v2[2];
            return v1;
        }

        public static float* Sub(float* v1, float v2)
        {
            v1[0] = v1[0] - v2;
            v1[1] = v1[1] - v2;
            v1[2] = v1[2] - v2;
            return v1;
        }

        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return v1._x * v2._x + v1._y * v2._y + v1._z * v2._z;
        }

        public float Dot(Vector3 v)
        {
            return _x * v._x + _y * v._y + _z * v._z;
        }

        public float Dot(Vector3* v)
        {
            return _x * v->_x + _y * v->_y + _z * v->_z;
        }

        public float Dot()
        {
            return _x * _x + _y * _y + _z * _z;
        }

        public static Vector3 Clamp(Vector3 v1, float min, float max)
        {
            v1.Clamp(min, max);
            return v1;
        }

        public void Clamp(float min, float max)
        {
            Max(min);
            Min(max);
        }

        public static Vector3 Min(Vector3 v1, Vector3 v2)
        {
            return new Vector3(Math.Min(v1._x, v2._x), Math.Min(v1._y, v2._y), Math.Min(v1._z, v2._z));
        }

        public static Vector3 Min(Vector3 v1, float f)
        {
            return new Vector3(Math.Min(v1._x, f), Math.Min(v1._y, f), Math.Min(v1._z, f));
        }

        public void Min(Vector3 v)
        {
            _x = Math.Min(_x, v._x);
            _y = Math.Min(_y, v._y);
            _z = Math.Min(_z, v._z);
        }

        public void Min(Vector3* v)
        {
            if (v->_x < _x)
            {
                _x = v->_x;
            }

            if (v->_y < _y)
            {
                _y = v->_y;
            }

            if (v->_z < _z)
            {
                _z = v->_z;
            }
        }

        public void Min(float f)
        {
            _x = Math.Min(_x, f);
            _y = Math.Min(_y, f);
            _z = Math.Min(_z, f);
        }

        public static Vector3 Max(Vector3 v1, Vector3 v2)
        {
            return new Vector3(Math.Max(v1._x, v2._x), Math.Max(v1._y, v2._y), Math.Max(v1._z, v2._z));
        }

        public static Vector3 Max(Vector3 v1, Vector3* v2)
        {
            return new Vector3(Math.Max(v1._x, v2->_x), Math.Max(v1._y, v2->_y), Math.Max(v1._z, v2->_z));
        }

        public static Vector3 Max(Vector3 v1, float f)
        {
            return new Vector3(Math.Max(v1._x, f), Math.Max(v1._y, f), Math.Max(v1._z, f));
        }

        public void Max(Vector3 v)
        {
            _x = Math.Max(_x, v._x);
            _y = Math.Max(_y, v._y);
            _z = Math.Max(_z, v._z);
        }

        public void Max(Vector3* v)
        {
            if (v->_x > _x)
            {
                _x = v->_x;
            }

            if (v->_y > _y)
            {
                _y = v->_y;
            }

            if (v->_z > _z)
            {
                _z = v->_z;
            }
        }

        public void Max(float f)
        {
            _x = Math.Max(_x, f);
            _y = Math.Max(_y, f);
            _z = Math.Max(_z, f);
        }

        public float DistanceTo(Vector3 v)
        {
            return (v - this).Dot();
        }

        public static Vector3 Lerp(Vector3 v1, Vector3 v2, float median)
        {
            return v1 * (1.0f - median) + v2 * median;
        }

        public static Vector3 Floor(Vector3 v)
        {
            return new Vector3((int) v._x, (int) v._y, (int) v._z);
        }

        public static readonly Vector3 UnitX = new Vector3(1.0f, 0.0f, 0.0f);
        public static readonly Vector3 UnitY = new Vector3(0.0f, 1.0f, 0.0f);
        public static readonly Vector3 UnitZ = new Vector3(0.0f, 0.0f, 1.0f);
        public static readonly Vector3 Zero = new Vector3(0.0f);
        public static readonly Vector3 One = new Vector3(1.0f);

        public Vector3 Cross(Vector3 v)
        {
            return new Vector3(_y * v._z - v._y * _z, _z * v._x - v._z * _x, _x * v._y - v._x * _y);
        }

        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1._y * v2._z - v2._y * v1._z, v1._z * v2._x - v2._z * v1._x,
                v1._x * v2._y - v2._x * v1._y);
        }

        public Vector3 Round(int places)
        {
            return new Vector3((float) Math.Round(_x, places), (float) Math.Round(_y, places),
                (float) Math.Round(_z, places));
        }

        public static Vector3 Truncate(Vector3 v)
        {
            return new Vector3(
                v._x > 0.0f ? (float) Math.Floor(v._x) : (float) Math.Ceiling(v._x),
                v._y > 0.0f ? (float) Math.Floor(v._y) : (float) Math.Ceiling(v._z),
                v._z > 0.0f ? (float) Math.Floor(v._z) : (float) Math.Ceiling(v._z));
        }

        public override string ToString()
        {
            return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Contains(",")
                ? $"({_x} {_y} {_z})"
                : $"({_x},{_y},{_z})";
        }

        public bool Contained(Vector3 start, Vector3 end, float expansion)
        {
            return Contained(this, start, end, expansion);
        }

        public static bool Contained(Vector3 point, Vector3 start, Vector3 end, float expansion)
        {
            float* sPtr = (float*) &point;
            float* s1 = (float*) &start, s2 = (float*) &end;
            float* temp;
            for (int i = 0; i < 3; i++)
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

        public static Vector3 IntersectZ(Vector3 ray1, Vector3 ray2, float z)
        {
            float a = ray2._z - ray1._z;

            float tanX = (ray1._y - ray2._y) / a;
            float tanY = (ray2._x - ray1._x) / a;

            a = z - ray1._z;
            return new Vector3(tanY * a + ray1._x, -tanX * a + ray1._y, z);
        }

        public float TrueDistance(Vector3 p)
        {
            return (float) Math.Sqrt((p - this).Dot());
        }

        public float TrueDistance()
        {
            return (float) Math.Sqrt(Dot());
        }

        public Vector3 Normalize()
        {
            return this / TrueDistance();
        }

        public Vector3 Normalize(Vector3 origin)
        {
            return (this - origin).Normalize();
        }

        public Vector3 GetAngles()
        {
            return new Vector3(AngleX(), AngleY(), AngleZ());
        }

        public Vector3 GetAngles(Vector3 origin)
        {
            return (this - origin).GetAngles();
        }

        public Vector3 LookatAngles()
        {
            return new Vector3((float) Math.Atan2(_y, Math.Sqrt(_x * _x + _z * _z)), (float) Math.Atan2(-_x, -_z),
                0.0f);
        }

        public Vector3 LookatAngles(Vector3 origin)
        {
            return (this - origin).LookatAngles();
        }

        public float AngleX()
        {
            return (float) Math.Atan2(_y, -_z);
        }

        public float AngleY()
        {
            return (float) Math.Atan2(-_z, _x);
        }

        public float AngleZ()
        {
            return (float) Math.Atan2(_y, _x);
        }

        public override int GetHashCode()
        {
            fixed (Vector3* p = &this)
            {
                int* p2 = (int*) p;
                return p2[0] ^ p2[1] ^ p2[2];
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3 vector3)
            {
                return this == vector3;
            }

            return false;
        }

        public float this[int index]
        {
            get
            {
                fixed (Vector3* p = &this)
                {
                    return ((float*) p)[index];
                }
            }
            set
            {
                fixed (Vector3* p = &this)
                {
                    ((float*) p)[index] = value;
                }
            }
        }

        public void Lerp(Vector3 dest, float percent)
        {
            this += (dest - this) * percent;
        }

        public Vector3 Lerped(Vector3 dest, float percent)
        {
            return this + (dest - this) * percent;
        }

        public void RemapToRange(float min, float max)
        {
            _x = _x.RemapToRange(min, max);
            _y = _y.RemapToRange(min, max);
            _z = _z.RemapToRange(min, max);
        }

        public Vector3 RemappedToRange(float min, float max)
        {
            return new Vector3(
                _x.RemapToRange(min, max),
                _y.RemapToRange(min, max),
                _z.RemapToRange(min, max));
        }

        public int CompareTo(object obj)
        {
            if (obj is Vector3)
            {
                Vector3 o = (Vector3) obj;
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

                if (_z > o._z)
                {
                    return 1;
                }

                if (_z < o._z)
                {
                    return -1;
                }

                return 0;
            }

            return 1;
        }

        public Vector4 ToQuat()
        {
            Vector4 q = new Vector4();

            double
                xRadian = _x / 2.0f,
                yRadian = _y / 2.0f,
                zRadian = _z / 2.0f,
                sinX = Math.Sin(xRadian),
                cosX = Math.Cos(xRadian),
                sinY = Math.Sin(yRadian),
                cosY = Math.Cos(yRadian),
                sinZ = Math.Sin(zRadian),
                cosZ = Math.Cos(zRadian);

            q._x = (float) (cosX * cosY * cosZ + sinX * sinY * sinZ);
            q._y = (float) (sinX * cosY * cosZ - cosX * sinY * sinZ);
            q._z = (float) (cosX * sinY * cosZ + sinX * cosY * sinZ);
            q._w = (float) (cosX * cosY * sinZ - sinX * sinY * cosZ);

            return q;
        }

        public bool IsInTriangle(Vector3 triPt1, Vector3 triPt2, Vector3 triPt3)
        {
            Vector3 v0 = triPt2 - triPt1;
            Vector3 v1 = triPt3 - triPt1;
            Vector3 v2 = this - triPt1;

            float dot00 = v0.Dot(v0);
            float dot01 = v0.Dot(v1);
            float dot02 = v0.Dot(v2);
            float dot11 = v1.Dot(v1);
            float dot12 = v1.Dot(v2);

            //Get barycentric coordinates
            float d = dot00 * dot11 - dot01 * dot01;
            float u = (dot11 * dot02 - dot01 * dot12) / d;
            float v = (dot00 * dot12 - dot01 * dot02) / d;

            return u >= 0 && v >= 0 && u + v < 1;
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
    }
}