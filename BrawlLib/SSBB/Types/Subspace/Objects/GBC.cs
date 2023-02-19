using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GBCHeader
    {
        public const int Size = 0x140;

        public MotionPathData _motionPathData;  // 0x000
        public buint _unknown0x008;
        public buint _unknown0x00C;
        public buint _unknown0x010;
        public buint _unknown0x014;
        public buint _unknown0x018;
        public buint _unknown0x01C;
        public bfloat _areaOffsetPosX;          // 0x020
        public bfloat _areaOffsetPosY;          // 0x024
        public bfloat _areaRangeX;              // 0x028
        public bfloat _areaRangeY;              // 0x02C
        public bfloat _posX;                    // 0x030
        public bfloat _posY;                    // 0x034
        public bfloat _rot;                     // 0x038
        public bfloat _maxRot;                  // 0x03C
        public bfloat _difficultyRotateSpeed1;  // 0x040
        public bfloat _difficultyRotateSpeed2;  // 0x044
        public bfloat _difficultyRotateSpeed3;  // 0x048
        public bfloat _difficultyRotateSpeed4;  // 0x04C
        public bfloat _difficultyRotateSpeed5;  // 0x050
        public bfloat _difficultyRotateSpeed6;  // 0x054
        public bfloat _difficultyRotateSpeed7;  // 0x058
        public bfloat _difficultyRotateSpeed8;  // 0x05C
        public bfloat _difficultyRotateSpeed9;  // 0x060
        public bfloat _difficultyRotateSpeed10; // 0x064
        public bfloat _difficultyRotateSpeed11; // 0x068
        public bfloat _difficultyRotateSpeed12; // 0x06C
        public bfloat _difficultyRotateSpeed13; // 0x070
        public bfloat _difficultyRotateSpeed14; // 0x074
        public bfloat _difficultyRotateSpeed15; // 0x078
        public bfloat _difficultyMotionRatio1;  // 0x07C
        public bfloat _difficultyMotionRatio2;  // 0x080
        public bfloat _difficultyMotionRatio3;  // 0x084
        public bfloat _difficultyMotionRatio4;  // 0x088
        public bfloat _difficultyMotionRatio5;  // 0x08C
        public bfloat _difficultyMotionRatio6;  // 0x090
        public bfloat _difficultyMotionRatio7;  // 0x094
        public bfloat _difficultyMotionRatio8;  // 0x098
        public bfloat _difficultyMotionRatio9;  // 0x09C
        public bfloat _difficultyMotionRatio10; // 0x0A0
        public bfloat _difficultyMotionRatio11; // 0x0A4
        public bfloat _difficultyMotionRatio12; // 0x0A8
        public bfloat _difficultyMotionRatio13; // 0x0AC
        public bfloat _difficultyMotionRatio14; // 0x0B0
        public bfloat _difficultyMotionRatio15; // 0x0B4
        public buint _maxFrames;                // 0x0B8
        public bfloat _maxFireRot;              // 0x0BC
        public bfloat _cameraOffsetX;           // 0x0C0
        public bfloat _cameraOffsetY;           // 0x0C4
        public byte _isAutoFire;                // 0x0C8
        public byte _unknown0x0C9;
        public byte _fullRotate;                // 0x0CA
        public byte _alwaysRotate;              // 0x0CB
        public byte _mdlIndex;                  // 0x0CC
        public byte _unknown0x0CD;
        public bushort _unknown0x0CE;
        public buint _unknown0x0D0;
        public buint _unknown0x0D4;
        public TriggerData _enterCannonTrigger; // 0x0D8
        public TriggerData _motionPathTrigger;  // 0x0DC
        public TriggerData _isValidTrigger;     // 0x0E0
        public AttackData _attackData;          // 0x0E4
        public buint _unknown0x13C;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GBC1Entry
    {
        public const int Size = GBCHeader.Size + MotionPathData.Size;
        
        public GBCHeader _header;
        public MotionPathData _shootMotionPathData;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GBC2Entry
    {
        public const int Size = GBCHeader.Size + 0x10;

        public GBCHeader _header;
        public bfloat _shootSpeed;          // 0x140
        public bfloat _shootTimerSpeed;     // 0x144
        public bfloat _shootAngleOffset;    // 0x148
        public bfloat _shootStunTimerSpeed; // 0x14C
    }
}
