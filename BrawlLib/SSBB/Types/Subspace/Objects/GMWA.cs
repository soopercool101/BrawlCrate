using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    public unsafe struct GMWAEntry
    {
        public const int Size = 0x98;

        public bfloat _locationX;                   // 0x00
        public bfloat _locationY;                   // 0x04
        public bfloat _rotationZ;                   // 0x08
        public bfloat _unknown0x0C;
        public byte _unknown0x10;
        public byte _mdlIndex;                      // 0x11
        public byte _unknown0x12;                   // 0x12 - Collision index
        public byte _unknown0x13;
        public bfloat _unknown0x14;
        public bfloat _unknown0x18;
        public MotionPathData _motionPathData;      // 0x1C
        public buint _unknown0x24;
        public TriggerData _trigger0x28;            // 0x28 - Spawn trigger?
        public TriggerData _trigger0x2C;            // 0x2C
        public TriggerData _trigger0x30;            // 0x30
        public DifficultyRatios _difficultyRatios;  // 0x34
        public bfloat _unknown0x70;
        public bfloat _unknown0x74;
        public bfloat _unknown0x78;
        public bfloat _unknown0x7C;
        public bfloat _unknown0x80;
        public bfloat _unknown0x84;
        public bfloat _hurtboxSize;                 // 0x88
        public byte _unknown0x8C;
        public byte _unknown0x8D;
        public byte _unknown0x8E;
        public byte _unknown0x8F;
        public byte _unknown0x90;
        public byte _unknown0x91;
        public byte _unknown0x92;
        public byte _unknown0x93;
        public byte _unknown0x94;
        public byte _unknown0x95;
        public byte _unknown0x96;
        public byte _unknown0x97;

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