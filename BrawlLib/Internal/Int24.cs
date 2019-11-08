using System.Runtime.InteropServices;

namespace BrawlLib.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BInt24
    {
        public byte _dat0, _dat1, _dat2;

        public byte this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:  return _dat0;
                    case 1:  return _dat1;
                    case 2:  return _dat2;
                    default: return 0;
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        _dat0 = value;
                        break;
                    case 1:
                        _dat1 = value;
                        break;
                    case 2:
                        _dat2 = value;
                        break;
                }
            }
        }

        public int Value
        {
            get => (((_dat0 & 0x7F) << 16) | (_dat1 << 8) | _dat2) * ((_dat0 & 0x80) == 0x80 ? -1 : 1);
            set
            {
                _dat2 = (byte) (value & 0xFF);
                _dat1 = (byte) ((value >> 8) & 0xFF);
                _dat0 = (byte) ((value >> 16) & 0x7F);
                if (value < 0)
                {
                    _dat0 |= 0x80;
                }
            }
        }

        public static explicit operator int(BInt24 val)
        {
            return val.Value;
        }

        public static explicit operator BInt24(int val)
        {
            return new BInt24(val);
        }

        public static explicit operator uint(BInt24 val)
        {
            return (uint) val.Value;
        }

        public static explicit operator BInt24(uint val)
        {
            return new BInt24((int) val);
        }

        public BInt24(int value)
        {
            _dat2 = (byte) (value & 0xFF);
            _dat1 = (byte) ((value >> 8) & 0xFF);
            _dat0 = (byte) ((value >> 16) & 0x7F);
            if (value < 0)
            {
                _dat0 |= 0x80;
            }
        }

        public BInt24(byte v0, byte v1, byte v2)
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