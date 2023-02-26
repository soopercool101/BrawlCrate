using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    public unsafe struct GMPSEntry
    {
        public const int Size = 0x120;

        public MotionPathData _motionPathData;              // 0x000
        public MotionPathData _sliderPathData;              // 0x008
        public HitData _hitData;                            // 0x010
        public buint _unknown0x030;                         // 0x030
        public AttackData _attackData;                      // 0x034
        public buint _unknown0x08C;
        public buint _unknown0x090;
        public bfloat _unknown0x094;
        public bfloat _unknown0x098;
        public bfloat _unknown0x09C;
        public bfloat _unknown0x0A0;
        public bfloat _unknown0x0A4;
        public bfloat _unknown0x0A8;
        public DifficultyRatios _difficultyMotionRatios;    // 0x0AC
        public TriggerData _triggerData;                    // 0x0E8
        public uint _unknown0x0EC;
        public uint _unknown0x0F0;
        public uint _unknown0x0F4;
        public byte _modelIndex;                            // 0x0F8
        public byte _unknown0x0F9;
        public byte _unknown0x0FA;
        public byte _unknown0x0FB;
        public fixed byte _boneName[0x20];                  // 0x0FC
        public buint _unknown0x11C;

        public string BoneName
        {
            get => Address.GetUTF8String(0x0FC, 0x20);
            set => Address.WriteUTF8String(value, 0x0FC, 0x20);
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