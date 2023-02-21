using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Hazards
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GWATEntry
    {
        public const int Size = 0x38;

        public buint _unknown0x00;
        public buint _unknown0x04;
        public buint _unknown0x08;
        public buint _unknown0x0C;
        public buint _unknown0x10;
        public buint _unknown0x14;
        public bfloat _posX;            // 0x18
        public bfloat _depth;           // 0x1C
        public bfloat _width;           // 0x20
        public bfloat _unknown0x24;     // 0x24
        public bfloat _posY;            // 0x28
        public bool8 _canDrown;         // 0x2C
        public byte _unknown0x2D;
        public byte _unknown0x2E;
        public byte _unknown0x2F;
        public bfloat _currentSpeed;    // 0x30
        public buint _unknown0x34;

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