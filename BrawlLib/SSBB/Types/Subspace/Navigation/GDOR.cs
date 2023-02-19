using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Navigation
{
    public enum DoorGimmickKind : byte
    {
        Ground = 0x0,
        Air = 0x1,
        GroundAuto = 0x2,
        AirAuto = 0x3
    }

    public enum DoorType : byte
    {
        NormalDoor = 0x0,
        YellowDoor = 0x1,
        ArrowDoor = 0x2,
        ShadowDoor = 0x3,
        SaveDoor = 0x4,
        WarpDoor = 0x5,
        FactoryDoor = 0x6,
        FactoryGoldDoor = 0x7
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GDOREntry
    {
        public const int Size = 0x90;

        public MotionPathData _motionPathData;  // 0x00
        public buint _unknown0x08;
        public buint _unknown0x0C;
        public buint _unknown0x10;
        public buint _unknown0x14;
        public buint _unknown0x18;
        public bfloat _areaOffsetPosX;          // 0x1C
        public bfloat _areaOffsetPosY;          // 0x20
        public bfloat _areaRangeX;              // 0x24
        public bfloat _areaRangeY;              // 0x28
        public byte _levelId;                   // 0x2C
        public byte _levelSequenceId;           // 0x2D
        public byte _levelSegmentId;            // 0x2E
        public byte _doorIndex;                 // 0x2F
        public buint _jumpData;                 // 0x30
        public DoorGimmickKind _doorGimmick;    // 0x34
        public byte _unknown0x35;
        public byte _modelIndex;                // 0x36
        public bool8 _playDoorTypeEffect;
        public bfloat _positionX;               // 0x38
        public bfloat _positionY;               // 0x3C
        public TriggerData _openDoorTrigger;    // 0x40
        public DoorType _doorType;              // 0x44
        public byte _unknown0x45;
        public byte _unknown0x46;
        public byte _unknown0x47;
        public bint _soundId;                   // 0x48
        public TriggerData _motionPathTrigger;  // 0x4C
        public TriggerData _isValidTrigger;     // 0x50
        public bfloat _difficultyMotionRatio1;  // 0x54
        public bfloat _difficultyMotionRatio2;  // 0x58
        public bfloat _difficultyMotionRatio3;  // 0x5C
        public bfloat _difficultyMotionRatio4;  // 0x60
        public bfloat _difficultyMotionRatio5;  // 0x64
        public bfloat _difficultyMotionRatio6;  // 0x68
        public bfloat _difficultyMotionRatio7;  // 0x6C
        public bfloat _difficultyMotionRatio8;  // 0x70
        public bfloat _difficultyMotionRatio9;  // 0x74
        public bfloat _difficultyMotionRatio10; // 0x78
        public bfloat _difficultyMotionRatio11; // 0x7C
        public bfloat _difficultyMotionRatio12; // 0x80
        public bfloat _difficultyMotionRatio13; // 0x84
        public bfloat _difficultyMotionRatio14; // 0x88
        public bfloat _difficultyMotionRatio15; // 0x8C
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GDTPEntry
    {
        public const int Size = 0xA0;

        public GDOREntry _doorHeader;
        public TriggerData _trigger1;
        public TriggerData _trigger2;
        public TriggerData _trigger3;
        public TriggerData _trigger4;
    }
}