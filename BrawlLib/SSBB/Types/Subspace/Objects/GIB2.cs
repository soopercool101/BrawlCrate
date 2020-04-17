using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GIB2Entry
    {
        public const int SIZE = 0x54;

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
        public bfloat _unkflag2;
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
        public byte _collisiondataid;
        public byte _unknown0x2E;
        public byte _unknown0x2F;
        public bfloat _posX;
        public bfloat _posY;
        public bint _itemspawngroup;
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
        public buint _trigger1;
        public buint _trigger2;
        public buint _trigger3;

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