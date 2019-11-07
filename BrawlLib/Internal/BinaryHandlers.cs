using System;
using System.Runtime.InteropServices;

namespace BrawlLib.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Bin32
    {
        public buint _data;

        public Bin32(uint val)
        {
            _data = val;
        }

        public static implicit operator uint(Bin32 val)
        {
            return val._data;
        }

        public static implicit operator Bin32(uint val)
        {
            return new Bin32(val);
        }
        //public static implicit operator int(Bin32 val) { return (int)val._data; }
        //public static implicit operator Bin32(int val) { return new Bin32((uint)val); }

        public override string ToString()
        {
            int i = 0;
            string val = "";
            while (i++ < 32)
            {
                val += (_data >> (32 - i)) & 1;
                if (i % 4 == 0 && i != 32)
                {
                    val += " ";
                }
            }

            return val;
        }

        public bool this[int index]
        {
            get => ((_data >> index) & 1) != 0;
            set
            {
                if (value)
                {
                    _data |= (uint) (1 << index);
                }
                else
                {
                    _data &= ~(uint) (1 << index);
                }
            }
        }

        //public uint this[int shift, int mask]
        //{
        //    get { return (uint)(data >> shift & mask); }
        //    set { data = (uint)((data & ~(mask << shift)) | ((value & mask) << shift)); }
        //}

        public uint this[int shift, int bitCount]
        {
            get
            {
                int mask = 0;
                for (int i = 0; i < bitCount; i++)
                {
                    mask |= 1 << i;
                }

                return (uint) ((_data >> shift) & mask);
            }
            set
            {
                int mask = 0;
                for (int i = 0; i < bitCount; i++)
                {
                    mask |= 1 << i;
                }

                _data = (uint) ((_data & ~(mask << shift)) | ((value & mask) << shift));
            }
        }

        public static Bin32 FromString(string s)
        {
            char[] delims = new char[] {',', '(', ')', ' '};

            uint b = 0;
            string[] arr = s.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            for (int len = 0; len < arr.Length; len++)
            {
                byte bit = 0;
                for (int i = 0; i < arr[len].Length; i++)
                {
                    b <<= 1;
                    byte.TryParse(arr[len][i].ToString(), out bit);
                    bit = bit.Clamp(0, 1);
                    b += bit;
                }
            }

            return new Bin32(b);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Bin24
    {
        public BUInt24 _data;

        public Bin24(BUInt24 val)
        {
            _data = val;
        }

        public static implicit operator int(Bin24 val)
        {
            return val._data;
        }

        public static implicit operator uint(Bin24 val)
        {
            return val._data;
        }

        public static implicit operator Bin24(uint val)
        {
            return new Bin24(val);
        }

        public static implicit operator BUInt24(Bin24 val)
        {
            return val._data;
        }

        public static implicit operator Bin24(BUInt24 val)
        {
            return new Bin24(val);
        }

        public override string ToString()
        {
            int i = 0;
            string val = "";
            uint data = _data;
            while (i++ < 24)
            {
                val += (data >> (24 - i)) & 1;
                if (i % 4 == 0 && i != 24)
                {
                    val += " ";
                }
            }

            return val;
        }

        public bool this[int index]
        {
            get => (((uint) _data >> index) & 1) != 0;
            set
            {
                if (value)
                {
                    _data = _data | (uint) (1 << index);
                }
                else
                {
                    _data = _data & ~(uint) (1 << index);
                }
            }
        }

        //public uint this[int shift, int mask]
        //{
        //    get { return (uint)(data >> shift & mask); }
        //    set { data = (uint)((data & ~(mask << shift)) | ((value & mask) << shift)); }
        //}

        public int this[int shift, int bitCount]
        {
            get
            {
                int mask = 0;
                for (int i = 0; i < bitCount; i++)
                {
                    mask |= 1 << i;
                }

                return (int) (((uint) _data >> shift) & mask);
            }
            set
            {
                int mask = 0;
                for (int i = 0; i < bitCount; i++)
                {
                    mask |= 1 << i;
                }

                _data = (uint) (((uint) _data & ~(mask << shift)) | (((uint) value & mask) << shift));
            }
        }

        public static Bin24 FromString(string s)
        {
            char[] delims = new char[] {',', '(', ')', ' '};

            uint b = 0;
            string[] arr = s.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            for (int len = 0; len < arr.Length; len++)
            {
                byte bit = 0;
                for (int i = 0; i < arr[len].Length; i++)
                {
                    b <<= 1;
                    byte.TryParse(arr[len][i].ToString(), out bit);
                    bit = bit.Clamp(0, 1);
                    b += bit;
                }
            }

            return new Bin24(b);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Bin16
    {
        public bushort _data;

        public Bin16(ushort val)
        {
            _data = val;
        }

        public static implicit operator ushort(Bin16 val)
        {
            return val._data;
        }

        public static implicit operator Bin16(ushort val)
        {
            return new Bin16(val);
        }
        //public static implicit operator short(Bin16 val) { return (short)val._data; }
        //public static implicit operator Bin16(short val) { return new Bin16((ushort)val); }

        public override string ToString()
        {
            int i = 0;
            string val = "";
            while (i++ < 16)
            {
                val += (_data >> (16 - i)) & 1;
                if (i % 4 == 0 && i != 16)
                {
                    val += " ";
                }
            }

            return val;
        }

        public bool this[int index]
        {
            get => ((_data >> index) & 1) != 0;
            set
            {
                if (value)
                {
                    _data = (ushort) (_data | (ushort) (1 << index));
                }
                else
                {
                    _data = (ushort) (_data & ~(ushort) (1 << index));
                }
            }
        }

        //public ushort this[int shift, int mask]
        //{
        //    get { return (ushort)(data >> shift & mask); }
        //    set { data = (ushort)((data & ~(mask << shift)) | ((value & mask) << shift)); }
        //}

        public ushort this[int shift, int bitCount]
        {
            get
            {
                int mask = 0;
                for (int i = 0; i < bitCount; i++)
                {
                    mask |= 1 << i;
                }

                return (ushort) ((_data >> shift) & mask);
            }
            set
            {
                int mask = 0;
                for (int i = 0; i < bitCount; i++)
                {
                    mask |= 1 << i;
                }

                _data = (ushort) ((_data & ~(mask << shift)) | ((value & mask) << shift));
            }
        }

        public static Bin16 FromString(string s)
        {
            char[] delims = new char[] {',', '(', ')', ' '};

            ushort b = 0;
            string[] arr = s.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            for (int len = 0; len < arr.Length; len++)
            {
                byte bit = 0;
                for (int i = 0; i < arr[len].Length; i++)
                {
                    b <<= 1;
                    byte.TryParse(arr[len][i].ToString(), out bit);
                    bit = bit.Clamp(0, 1);
                    b += bit;
                }
            }

            return new Bin16(b);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Bin8
    {
        public byte _data;

        public Bin8(byte val)
        {
            _data = val;
        }

        public static implicit operator byte(Bin8 val)
        {
            return val._data;
        }

        public static implicit operator Bin8(byte val)
        {
            return new Bin8(val);
        }
        //public static implicit operator sbyte(Bin8 val) { return (sbyte)val._data; }
        //public static implicit operator Bin8(sbyte val) { return new Bin8((byte)val); }

        public override string ToString()
        {
            int i = 0;
            string val = "";
            while (i++ < 8)
            {
                val += (_data >> (8 - i)) & 1;
                if (i % 4 == 0 && i != 8)
                {
                    val += " ";
                }
            }

            return val;
        }

        public bool this[int index]
        {
            get => ((_data >> index) & 1) != 0;
            set
            {
                if (value)
                {
                    _data |= (byte) (1 << index);
                }
                else
                {
                    _data &= (byte) ~(1 << index);
                }
            }
        }

        //public byte this[int shift, int mask]
        //{
        //    get { return (byte)(data >> shift & mask); }
        //    set { data = (byte)((data & ~(mask << shift)) | ((value & mask) << shift)); }
        //}

        public byte this[int shift, int bitCount]
        {
            get
            {
                int mask = 0;
                for (int i = 0; i < bitCount; i++)
                {
                    mask |= 1 << i;
                }

                return (byte) ((_data >> shift) & mask);
            }
            set
            {
                int mask = 0;
                for (int i = 0; i < bitCount; i++)
                {
                    mask |= 1 << i;
                }

                _data = (byte) ((_data & ~(mask << shift)) | ((value & mask) << shift));
            }
        }

        public static Bin8 FromString(string s)
        {
            char[] delims = new char[] {',', '(', ')', ' '};

            byte b = 0;
            string[] arr = s.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            for (int len = 0; len < arr.Length; len++)
            {
                byte bit = 0;
                for (int i = 0; i < arr[len].Length; i++)
                {
                    b <<= 1;
                    byte.TryParse(arr[len][i].ToString(), out bit);
                    bit = bit.Clamp(0, 1);
                    b += bit;
                }
            }

            return new Bin8(b);
        }
    }
}