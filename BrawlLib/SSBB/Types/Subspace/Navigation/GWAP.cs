using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Navigation
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GWAPEntry
    {
        public const int Size = 0x80;

        public MotionPathData _motionPathData;  // 0x00
        public buint _unknown0x08;
        public buint _unknown0x0C;
        public buint _unknown0x10;
        public buint _unknown0x14;
        public buint _unknown0x18;
        public buint _unknown0x1C;
        public bfloat _areaOffsetPositionX;     // 0x20
        public bfloat _areaOffsetPositionY;     // 0x24
        public bfloat _areaRangeX;              // 0x28
        public bfloat _areaRangeY;              // 0x2C
        public fixed byte _boneName[0x20];      // 0x30
        public byte _modelDataIndex;            // 0x50
        public byte _unknown0x51;
        public byte _unknown0x52;
        public byte _unknown0x53;
        public bfloat _positionX;               // 0x54
        public bfloat _positionY;               // 0x58
        public buint _soundId1;                 // 0x5C
        public buint _soundId2;                 // 0x60
        public buint _unknown0x64;
        public buint _unknown0x68;
        public TriggerData _warpTriggerData;    // 0x6C
        public buint _unknown0x70;
        public buint _unknown0x74;
        public buint _unknown0x78;
        public TriggerData _isValidTriggerData; // 0x7C

        public string BoneName
        {
            get => Address.GetUTF8String(0x30, 0x20);
            set => Address.WriteUTF8String(value, 0x30, 0x20);
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
