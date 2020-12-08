using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace BrawlLib.Internal
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Vector4 : ISerializable
    {
        public float _x, _y, _z, _w;

        public Vector4(float x, float y, float z, float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        public Vector4(float s)
        {
            _x = s;
            _y = s;
            _z = s;
            _w = 1;
        }

        public Vector4(SerializationInfo info, StreamingContext context)
        {
            _x = info.GetSingle("_x");
            _z = info.GetSingle("_y");
            _y = info.GetSingle("_z");
            _w = info.GetSingle("_w");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_x", _x);
            info.AddValue("_y", _y);
            info.AddValue("_z", _z);
            info.AddValue("_w", _w);
        }

        //public static explicit operator Vector3(Vector4 v) { return new Vector3(v._x / v._w, v._y / v._w, v._z / v._w); }
        public static explicit operator Vector4(Vector3 v)
        {
            return new Vector4(v._x, v._y, v._z, 1.0f);
        }

        public static Vector4 operator *(Vector4 v, float f)
        {
            return new Vector4(v._x * f, v._y * f, v._z * f, v._w * f);
        }

        public static Vector4 operator /(Vector4 v, float f)
        {
            return new Vector4(v._x / f, v._y / f, v._z / f, v._w / f);
        }

        public static Vector4 operator -(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1._x - v2._x, v1._y - v2._y, v1._z - v2._z, v1._w - v2._w);
        }

        public static Vector4 operator +(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1._x + v2._x, v1._y + v2._y, v1._z + v2._z, v1._w + v2._w);
        }

        public static Vector4 operator *(Vector4 v1, Vector4 v2)
        {
            Vector4 v = new Vector4();

            Vector3 x1 = new Vector3(v1._x, v1._y, v1._z);
            Vector3 x2 = new Vector3(v2._x, v2._y, v2._z);
            float w1 = v1._w;
            float w2 = v2._w;

            //Vector3 s = new Vector3(w1 * w2 - x1.Dot(x2), w1 * x2 + w2 * x1 + x1.Cross(x2));

            return v;
        }

        public static bool operator ==(Vector4 v1, Vector4 v2)
        {
            return v1._x == v2._x && v1._y == v2._y && v1._z == v2._z && v1._w == v2._w;
        }

        public static bool operator !=(Vector4 v1, Vector4 v2)
        {
            return v1._x != v2._x || v1._y != v2._y || v1._z != v2._z || v1._w != v2._w;
        }

        public float Length()
        {
            return (float) Math.Sqrt(Dot());
        }

        public float Dot()
        {
            return _x * _x + _y * _y + _z * _z + _w * _w;
        }

        public float Dot(Vector4 v)
        {
            return _x * v._x + _y * v._y + _z * v._z + _w * v._w;
        }

        public Vector4 Normalize()
        {
            return this * (1.0f / Length());
        }

        public float Dot3()
        {
            return _x * _x + _y * _y + _z * _z;
        }

        public float Dot3(Vector4 v)
        {
            return _x * v._x + _y * v._y + _z * v._z;
        }

        public float Length3()
        {
            return (float) Math.Sqrt(Dot3());
        }

        public Vector4 Normalize3()
        {
            float scale = 1.0f / Length3();
            return new Vector4(_x * scale, _y * scale, _z * scale, _w);
        }

        public override string ToString()
        {
            return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Contains(",")
                ? $"({_x} {_y} {_z} {_w})"
                : $"({_x},{_y},{_z},{_w})";
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector4 vector4)
            {
                return this == vector4;
            }

            return false;
        }

        public override int GetHashCode()
        {
            fixed (Vector4* p = &this)
            {
                int* p2 = (int*) p;
                return p2[0] ^ p2[1] ^ p2[2] ^ p2[3];
            }
        }

        public float this[int index]
        {
            get
            {
                fixed (Vector4* p = &this)
                {
                    return ((float*) p)[index];
                }
            }
            set
            {
                fixed (Vector4* p = &this)
                {
                    ((float*) p)[index] = value;
                }
            }
        }

        public static readonly Vector4 UnitX = new Vector4(1, 0, 0, 0);
        public static readonly Vector4 UnitY = new Vector4(0, 1, 0, 0);
        public static readonly Vector4 UnitZ = new Vector4(0, 0, 1, 0);
        public static readonly Vector4 UnitW = new Vector4(0, 0, 0, 1);
        public static readonly Vector4 Zero = new Vector4(0, 0, 0, 0);
        public static readonly Vector4 One = new Vector4(1, 1, 1, 1);
        public static readonly Vector4 Identity = new Vector4(0, 0, 0, 1);

        public void ToAxisAngle(out Vector3 axis, out float angle)
        {
            Vector4 result = ToAxisAngle();
            axis = new Vector3(result._x, result._y, result._z);
            angle = result._w;
        }

        public Vector4 ToAxisAngle()
        {
            Vector4 q = this;
            if (q._w > 1.0f)
            {
                q.Normalize();
            }

            Vector4 result = new Vector4
            {
                _w = 2.0f * (float) Math.Acos(q._w)
            };
            float den = (float) Math.Sqrt(1.0 - q._w * q._w);
            if (den > 0.0001f)
            {
                result._x = q._x / den;
                result._y = q._y / den;
                result._z = q._z / den;
            }
            else
            {
                result._x = 1;
            }

            return result;
        }

        public static Vector4 FromAxisAngle(Vector3 axis, float angle)
        {
            if (axis.Dot() == 0.0f)
            {
                return Identity;
            }

            Vector4 result = Identity;

            angle *= 0.5f;
            axis.Normalize();
            result._x = axis._x * (float) Math.Sin(angle);
            result._y = axis._y * (float) Math.Sin(angle);
            result._z = axis._z * (float) Math.Sin(angle);
            result._w = (float) Math.Cos(angle);

            return result.Normalize();
        }

        public static Vector4 FromEulerAngles(Vector3 v)
        {
            Vector3
                vx = new Vector3(1, 0, 0),
                vy = new Vector3(0, 1, 0),
                vz = new Vector3(0, 0, 1);
            Vector4 qx, qy, qz;

            qx = FromAxisAngle(vx, v._x);
            qy = FromAxisAngle(vy, v._y);
            qz = FromAxisAngle(vz, v._z);

            return qx * qy * qz;
        }

        public Vector3 ToEuler()
        {
            return new Vector3(
                (float) Math.Atan2(2 * (_x * _y + _z * _w), 1 - 2 * (_y * _y + _z * _z)),
                (float) Math.Asin(2 * (_x * _z - _w * _y)),
                (float) Math.Atan2(2 * (_x * _w + _y * _z), 1 - 2 * (_z * _z + _w * _w)));
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