using System.Runtime.InteropServices;

namespace BrawlLib.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BUInt24
    {
        public byte _dat0, _dat1, _dat2;

        public uint Value
        {
            get => ((uint) _dat0 << 16) | ((uint) _dat1 << 8) | _dat2;
            set
            {
                _dat2 = (byte) (value & 0xFF);
                _dat1 = (byte) ((value >> 8) & 0xFF);
                _dat0 = (byte) ((value >> 16) & 0xFF);
            }
        }

        public static implicit operator int(BUInt24 val)
        {
            return (int) val.Value;
        }

        public static implicit operator BUInt24(int val)
        {
            return new BUInt24((uint) val);
        }

        public static implicit operator uint(BUInt24 val)
        {
            return val.Value;
        }

        public static implicit operator BUInt24(uint val)
        {
            return new BUInt24(val);
        }

        public BUInt24(uint value)
        {
            _dat2 = (byte) (value & 0xFF);
            _dat1 = (byte) ((value >> 8) & 0xFF);
            _dat0 = (byte) ((value >> 16) & 0xFF);
        }

        public BUInt24(byte v0, byte v1, byte v2)
        {
            _dat2 = v2;
            _dat1 = v1;
            _dat0 = v0;
        }

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct UInt24
    {
        public byte _dat2, _dat1, _dat0;

        public uint Value
        {
            get => ((uint) _dat0 << 16) | ((uint) _dat1 << 8) | _dat2;
            set
            {
                _dat2 = (byte) (value & 0xFF);
                _dat1 = (byte) ((value >> 8) & 0xFF);
                _dat0 = (byte) ((value >> 16) & 0xFF);
            }
        }

        public static implicit operator int(UInt24 val)
        {
            return (int) val.Value;
        }

        public static implicit operator UInt24(int val)
        {
            return new UInt24((uint) val);
        }

        public static implicit operator uint(UInt24 val)
        {
            return val.Value;
        }

        public static implicit operator UInt24(uint val)
        {
            return new UInt24(val);
        }

        public UInt24(uint value)
        {
            _dat2 = (byte) (value & 0xFF);
            _dat1 = (byte) ((value >> 8) & 0xFF);
            _dat0 = (byte) ((value >> 16) & 0xFF);
        }

        public UInt24(byte v0, byte v1, byte v2)
        {
            _dat2 = v2;
            _dat1 = v1;
            _dat0 = v0;
        }

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }
}