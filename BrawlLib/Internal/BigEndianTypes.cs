using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace BrawlLib.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct bint
    {
        public int _data;

        public static implicit operator int(bint val)
        {
            return val._data.Reverse();
        }

        public static implicit operator bint(int val)
        {
            return new bint {_data = val.Reverse()};
        }

        public static explicit operator uint(bint val)
        {
            return (uint) val._data.Reverse();
        }

        public static explicit operator bint(uint val)
        {
            return new bint {_data = (int) val.Reverse()};
        }

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

        public VoidPtr OffsetAddress
        {
            get => Address + Value;
            set => _data = (value - Address).Reverse();
        }

        public int Value => this;

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct buint
    {
        public uint _data;

        public static implicit operator uint(buint val)
        {
            return val._data.Reverse();
        }

        public static implicit operator buint(uint val)
        {
            return new buint {_data = val.Reverse()};
        }

        public static explicit operator int(buint val)
        {
            return (int) val._data.Reverse();
        }

        public static explicit operator buint(int val)
        {
            return new buint {_data = (uint) val.Reverse()};
        }

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

        public VoidPtr OffsetAddress
        {
            get => Address + Value;
            set => _data = ((uint) (value - Address)).Reverse();
        }

        public uint Value => this;

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct bfloat
    {
        public float _data;

        public static implicit operator float(bfloat val)
        {
            return val._data.Reverse();
        }

        public static implicit operator bfloat(float val)
        {
            return new bfloat {_data = val.Reverse()};
        }

        public static implicit operator bfloat(byte[] val)
        {
            return new bfloat {_data = BitConverter.ToSingle(val, 0)};
        }

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

        public float Value => this;

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct bshort
    {
        public short _data;

        public static implicit operator short(bshort val)
        {
            return val._data.Reverse();
        }

        public static implicit operator bshort(short val)
        {
            return new bshort {_data = val.Reverse()};
        }

        public static explicit operator ushort(bshort val)
        {
            return (ushort) val._data.Reverse();
        }

        public static explicit operator bshort(ushort val)
        {
            return new bshort {_data = (short) val.Reverse()};
        }

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

        public short Value => this;

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct bushort
    {
        public ushort _data;

        public static implicit operator ushort(bushort val)
        {
            return val._data.Reverse();
        }

        public static implicit operator bushort(ushort val)
        {
            return new bushort {_data = val.Reverse()};
        }

        public static explicit operator short(bushort val)
        {
            return (short) val._data.Reverse();
        }

        public static explicit operator bushort(short val)
        {
            return new bushort {_data = (ushort) val.Reverse()};
        }

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

        public ushort Value => this;

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct blong
    {
        public long _data;

        public static implicit operator long(blong val)
        {
            return val._data.Reverse();
        }

        public static implicit operator blong(long val)
        {
            return new blong {_data = val.Reverse()};
        }

        public static explicit operator ulong(blong val)
        {
            return (ulong) val._data.Reverse();
        }

        public static explicit operator blong(ulong val)
        {
            return new blong {_data = (long) val.Reverse()};
        }

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

        public VoidPtr OffsetAddress
        {
            get => Address + Value;
            set => _data = ((long) (value - Address)).Reverse();
        }

        public long Value => this;

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct bulong
    {
        public ulong _data;

        public static implicit operator ulong(bulong val)
        {
            return val._data.Reverse();
        }

        public static implicit operator bulong(ulong val)
        {
            return new bulong {_data = val.Reverse()};
        }

        public static explicit operator long(bulong val)
        {
            return (long) val._data.Reverse();
        }

        public static explicit operator bulong(long val)
        {
            return new bulong {_data = (ulong) val.Reverse()};
        }

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

        public VoidPtr OffsetAddress
        {
            get => Address + Value;
            set => _data = ((ulong) (value - Address)).Reverse();
        }

        public ulong Value => this;

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BVec2
    {
        public bfloat _x;
        public bfloat _y;

        public BVec2(float x, float y)
        {
            _x = x;
            _y = y;
        }

        public override string ToString()
        {
            return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Contains(",")
                ? $"({(float) _x} {(float) _y})"
                : $"({(float) _x}, {(float) _y})";
        }

        public static implicit operator Vector2(BVec2 v)
        {
            return new Vector2(v._x, v._y);
        }

        public static implicit operator BVec2(Vector2 v)
        {
            return new BVec2(v._x, v._y);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BVec3
    {
        public bfloat _x;
        public bfloat _y;
        public bfloat _z;

        public BVec3(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public override string ToString()
        {
            return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Contains(",")
                ? $"({(float) _x} {(float) _y} {(float) _z})"
                : $"({(float) _x}, {(float) _y}, {(float) _z})";
        }

        public static implicit operator Vector3(BVec3 v)
        {
            return new Vector3(v._x, v._y, v._z);
        }

        public static implicit operator BVec3(Vector3 v)
        {
            return new BVec3(v._x, v._y, v._z);
        }

        public static implicit operator Vector4(BVec3 v)
        {
            return new Vector4(v._x, v._y, v._z, 1);
        }

        public static implicit operator BVec3(Vector4 v)
        {
            return new BVec3(v._x / v._w, v._y / v._w, v._z / v._w);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BVec4
    {
        public bfloat _x;
        public bfloat _y;
        public bfloat _z;
        public bfloat _w;

        public BVec4(float x, float y, float z, float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        public override string ToString()
        {
            return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Contains(",")
                ? $"({(float) _x} {(float) _y} {(float) _z} {(float) _w})"
                : $"({(float) _x}, {(float) _y}, {(float) _z}, {(float) _w})";
        }

        public static implicit operator Vector4(BVec4 v)
        {
            return new Vector4(v._x, v._y, v._z, v._w);
        }

        public static implicit operator BVec4(Vector4 v)
        {
            return new BVec4(v._x, v._y, v._z, v._w);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct bMatrix43
    {
        private fixed float _data[12];

        public bfloat* Data
        {
            get
            {
                fixed (float* ptr = _data)
                {
                    return (bfloat*) ptr;
                }
            }
        }

        public float this[int x, int y]
        {
            get => Data[(y << 2) + x];
            set => Data[(y << 2) + x] = value;
        }

        public float this[int index]
        {
            get => Data[index];
            set => Data[index] = value;
        }

        public override string ToString()
        {
            return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Contains(",")
                ? $"({this[0]} {this[1]} {this[2]} {this[3]})({this[4]} {this[5]} {this[6]} {this[7]})({this[8]} {this[9]} {this[10]} {this[11]})"
                : $"({this[0]},{this[1]},{this[2]},{this[3]})({this[4]},{this[5]},{this[6]},{this[7]})({this[8]},{this[9]},{this[10]},{this[11]})";
        }

        public static implicit operator Matrix(bMatrix43 bm)
        {
            Matrix m;

            bfloat* sPtr = (bfloat*) &bm;
            float* dPtr = (float*) &m;

            dPtr[0] = sPtr[0];
            dPtr[1] = sPtr[4];
            dPtr[2] = sPtr[8];
            dPtr[3] = 0.0f;
            dPtr[4] = sPtr[1];
            dPtr[5] = sPtr[5];
            dPtr[6] = sPtr[9];
            dPtr[7] = 0.0f;
            dPtr[8] = sPtr[2];
            dPtr[9] = sPtr[6];
            dPtr[10] = sPtr[10];
            dPtr[11] = 0.0f;
            dPtr[12] = sPtr[3];
            dPtr[13] = sPtr[7];
            dPtr[14] = sPtr[11];
            dPtr[15] = 1.0f;

            return m;
        }

        public static implicit operator bMatrix43(Matrix m)
        {
            bMatrix43 bm;

            bfloat* dPtr = (bfloat*) &bm;
            float* sPtr = (float*) &m;

            dPtr[0] = sPtr[0];
            dPtr[1] = sPtr[4];
            dPtr[2] = sPtr[8];
            dPtr[3] = sPtr[12];
            dPtr[4] = sPtr[1];
            dPtr[5] = sPtr[5];
            dPtr[6] = sPtr[9];
            dPtr[7] = sPtr[13];
            dPtr[8] = sPtr[2];
            dPtr[9] = sPtr[6];
            dPtr[10] = sPtr[10];
            dPtr[11] = sPtr[14];

            return bm;
        }

        public static implicit operator Matrix34(bMatrix43 bm)
        {
            Matrix34 m = new Matrix34();
            float* dPtr = (float*) &m;
            bfloat* sPtr = (bfloat*) &bm;
            for (int i = 0; i < 12; i++)
            {
                dPtr[i] = sPtr[i];
            }

            return m;
        }

        public static implicit operator bMatrix43(Matrix34 m)
        {
            bMatrix43 bm = new bMatrix43();
            bfloat* dPtr = (bfloat*) &bm;
            float* sPtr = (float*) &m;
            for (int i = 0; i < 12; i++)
            {
                dPtr[i] = sPtr[i];
            }

            return bm;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct bMatrix
    {
        private fixed float _data[16];

        public bfloat* Data
        {
            get
            {
                fixed (float* ptr = _data)
                {
                    return (bfloat*) ptr;
                }
            }
        }

        public float this[int x, int y]
        {
            get => Data[(y << 2) + x];
            set => Data[(y << 2) + x] = value;
        }

        public float this[int index]
        {
            get => Data[index];
            set => Data[index] = value;
        }

        public override string ToString()
        {
            return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Contains(",")
                ? $"({this[0]} {this[1]} {this[2]} {this[3]})({this[4]} {this[5]} {this[6]} {this[7]})({this[8]} {this[9]} {this[10]} {this[11]})({this[12]} {this[13]} {this[14]} {this[15]})"
                : $"({this[0]},{this[1]},{this[2]},{this[3]})({this[4]},{this[5]},{this[6]},{this[7]})({this[8]},{this[9]},{this[10]},{this[11]})({this[12]},{this[13]},{this[14]},{this[15]})";
        }

        public static implicit operator Matrix(bMatrix bm)
        {
            Matrix m = new Matrix();
            float* dPtr = (float*) &m;
            bfloat* sPtr = (bfloat*) &bm;
            for (int i = 0; i < 16; i++)
            {
                dPtr[i] = sPtr[i];
            }

            return m;
        }

        public static implicit operator bMatrix(Matrix m)
        {
            bMatrix bm = new bMatrix();
            bfloat* dPtr = (bfloat*) &bm;
            float* sPtr = (float*) &m;
            for (int i = 0; i < 16; i++)
            {
                dPtr[i] = sPtr[i];
            }

            return bm;
        }
    }
}