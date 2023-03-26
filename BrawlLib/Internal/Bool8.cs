using System.Runtime.InteropServices;

namespace BrawlLib.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct bool8
    {
        public Bin8 _data;

        public static implicit operator bool(bool8 val)
        {
            return val._data[0];
        }

        public static implicit operator bool8(bool val)
        {
            return new bool8 { _data = (byte) (val ? 1 : 0) };
        }

        public static explicit operator bool8(byte val)
        {
            return new bool8 { _data = val };
        }

        public static explicit operator bool8(Bin8 val)
        {
            return new bool8 { _data = val };
        }

        public bool Value => this;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct bool32
    {
        public buint _data;

        public static implicit operator bool(bool32 val)
        {
            return val._data != 0;
        }

        public static implicit operator bool32(bool val)
        {
            return new bool32 { _data = (buint)(val ? 1 : 0) };
        }

        public static explicit operator bool32(buint val)
        {
            return new bool32 { _data = val };
        }

        public static explicit operator bool32(uint val)
        {
            return new bool32 { _data = val };
        }

        public static explicit operator bool32(bint val)
        {
            return new bool32 { _data = (uint)val };
        }

        public static explicit operator bool32(int val)
        {
            return new bool32 { _data = (uint)val };
        }
    }
}
