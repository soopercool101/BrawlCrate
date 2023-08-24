using BrawlLib.Internal;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    public unsafe struct GBLKEntry
    {
        public const int Size = 0x40;

        public bfloat _unknown0x00;
        public bfloat _unknown0x04;
        public bfloat _unknown0x08;
        public bfloat _unknown0x0C;
        public bfloat _unknown0x10;
        public bfloat _unknown0x14;
        public bfloat _hurtboxSize;         // 0x18
        public byte _unknown0x1C;
        public byte _unknown0x1D;
        public byte _unknown0x1E;
        public byte _unknown0x1F;
        public bint _unknown0x20;           // Always 10?
        public byte _unknown0x24;
        public byte _unknown0x25;
        public byte _unknown0x26;
        public byte _unknown0x27;
        public byte _unknown0x28;
        public byte _baseModelIndex;        // 0x29
        public byte _unknown0x2A;
        public byte _unknown0x2B;
        public byte _positionModelIndex;    // 0x2C
        public byte _collisionModelIndex;   // 0x2D
        public byte _unknown0x2E;
        public byte _unknown0x2F;
        public bint _unknown0x30;
        public bint _unknown0x34;
        public bshort _unknown0x38;
        public bshort _unknown0x3A;
        public bint _unknown0x3C;

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