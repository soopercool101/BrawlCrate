using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GIB2
    {
        public const uint Tag = 0x32424947;
        public const int Size = 127;
        public uint _tag;
        public bint _count;
        public bint _DataOffset;

        public GIB2(int count)
        {
            _tag = Tag;
            _count = count;
            _DataOffset = count * 4;
        }

        public VoidPtr this[int index] => (byte*) Address + Offsets(index);

        public uint Offsets(int index)
        {
            return *(buint*) ((byte*) Address + 0x08 + index * 4);
        }

        private VoidPtr Address
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
    public unsafe struct GIB2Entry
    {
        public bfloat _header;
        public byte _unknown0x04;
        public byte _unkflag0;
        public byte _unkflag1;
        public byte _unknown0x07;
        public byte _unknown0x08;
        public byte _unknown0x09;
        public byte _unknown0x0A;
        public byte _unknown0x0B;
        public byte _unknown0x0C;
        public byte _unknown0x0D;
        public byte _unknown0x0E;
        public byte _unknown0x0F;
        public byte _unknown0x10;
        public byte _unknown0x11;
        public byte _unknown0x12;
        public byte _unknown0x13;
        public byte _unknown0x14;
        public byte _unknown0x15;
        public byte _unknown0x16;
        public byte _unknown0x17;
        public byte _unknown0x18;
        public byte _unknown0x19;
        public float _unkflag2;
        public byte _unknown0x1E;
        public byte _unknown0x1F;
        public byte _unknown0x20;
        public byte _unknown0x21;
        public byte _unknown0x22;
        public byte _unknown0x23;
        public byte _unknown0x24;
        public byte _unknown0x25;
        public byte _unknown0x26;
        public byte _unknown0x27;
        public byte _unknown0x28;
        public byte _unknown0x29;
        public byte _unknown0x2A;
        public byte _unkflag3;
        public byte _modeldataid;
        public byte _unkflag5;
        public byte _unknown0x2E;
        public byte _unknown0x2F;
        public float _unkflag6;
        public float _unkflag7;
        public byte _unknown0x38;
        public byte _unknown0x39;
        public byte _unknown0x3A;
        public byte _unkflag8;
        public byte _unknown0x3C;
        public byte _unknown0x3D;
        public byte _unknown0x3E;
        public byte _unknown0x3F;
        public byte _unknown0x40;
        public byte _unknown0x41;
        public byte _unknown0x42;
        public byte _unknown0x43;
        public byte _unknown0x44;
        public byte _unkflag9;
        public byte _unkflag10;
        public byte _unkflag11;
        public byte _unkflag12;
        public byte _unknown0x48;
        public byte _unknown0x49;
        public byte _unkflag13;
        public byte _unknown0x4B;
        public byte _unknown0x4C;
        public byte _unknown0x4D;
        public byte _unkflag14;
        public byte _unknown0x4F;
        public byte _unknown0x50;
        public byte _unknown0x51;
        public byte _unkflag15;
        public byte _unknown0x53;

        private VoidPtr Address
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