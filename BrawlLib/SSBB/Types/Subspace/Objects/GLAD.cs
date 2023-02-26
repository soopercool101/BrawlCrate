using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    public unsafe struct GLADEntry
    {
        public const int Size = 0x98;

        public MotionPathData _motionPathData;              // 0x00
        public buint _unknown0x08;
        public buint _unknown0x0C;
        public buint _unknown0x10;
        public buint _unknown0x14;
        public buint _unknown0x18;
        public buint _unknown0x1C;
        public bfloat _areaOffsetPosX;                      // 0x20
        public bfloat _areaOffsetPosY;                      // 0x24
        public bfloat _areaRangeX;                          // 0x28
        public bfloat _areaRangeY;                          // 0x2C
        public byte _modelIndex;                            // 0x30
        public byte _unknown0x31;
        public bool8 _restrictUpExit;                       // 0x32
        public byte _unknown0x33;
        public fixed byte _boneName[0x20];                  // 0x34
        public TriggerData _isValidTrigger;                 // 0x54
        public TriggerData _motionPathTrigger;              // 0x58
        public DifficultyRatios _difficultyMotionRatios;    // 0x5C

        public string BoneName
        {
            get => Address.GetUTF8String(0x34, 0x20);
            set => Address.WriteUTF8String(value, 0x34, 0x20);
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
}
