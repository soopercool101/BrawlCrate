using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Hazards
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GEG1Entry
    {
        public const int Size = 0x84;

        public MotionPathData _motionPathData;  // 0x00
        public bshort _epbmIndex;               // 0x08
        public bshort _epspIndex;               // 0x0A
        public bushort _connectedEnemyID;       // 0x0C
        public bshort _unknown0x0E;             // 0x0E
        public buint _unknown0x10;
        public buint _unknown0x14;
        public buint _unknown0x18;
        public bushort _enemyID;                // 0x1C
        public bshort _unknown0x1E;
        public buint _startingAction;           // 0x20
        public buint _unknown0x24;
        public bfloat _spawnPosX;               // 0x28
        public bfloat _spawnPosY;               // 0x2C
        public bfloat _spawnPosZ;               // 0x30
        public bfloat _unknown0x34;
        public bfloat _posX1;                   // 0x38
        public bfloat _posX2;                   // 0x3C
        public bfloat _posY1;                   // 0x40
        public bfloat _posY2;                   // 0x44
        public bfloat _unknown0x48;
        public bfloat _unknown0x4C;
        public bfloat _unknown0x50;
        public buint _unknown0x54;
        public bfloat _unknown0x58;
        public bfloat _unknown0x5C;
        public bfloat _unknown0x60;
        public byte _unknown0x64;
        public byte _unknown0x65;
        public byte _unknown0x66;
        public bool8 _isFacingRight;
        public byte _unknown0x68;
        public byte _unknown0x69;
        public byte _unknown0x6A;
        public byte _unknown0x6B;
        public bint _itemDropGenParamId;       // 0x6C
        public bint _rareItemDropGenParamId;   // 0x70
        public byte _unknown0x74;
        public byte _unknown0x75;
        public byte _unknown0x76;
        public byte _unknown0x77;
        public buint _unknown0x78;
        public TriggerData _spawnTrigger;       // 0x7C
        public TriggerData _defeatTrigger;      // 0x80

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