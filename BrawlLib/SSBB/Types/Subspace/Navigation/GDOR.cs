using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
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
        public byte _levelId;                               // 0x30
        public byte _levelSequenceId;                       // 0x31
        public byte _levelSegmentId;                        // 0x32
        public byte _doorIndex;                             // 0x33
        public DoorGimmickKind _doorGimmick;                // 0x34
        public byte _unknown0x35;
        public byte _modelIndex;                            // 0x36
        public bool8 _playDoorTypeEffect;
        public bfloat _positionX;                           // 0x38
        public bfloat _positionY;                           // 0x3C
        public TriggerData _openDoorTrigger;                // 0x40
        public DoorType _doorType;                          // 0x44
        public byte _unknown0x45;
        public byte _unknown0x46;
        public byte _unknown0x47;
        public bint _soundId;                               // 0x48
        public TriggerData _motionPathTrigger;              // 0x4C
        public TriggerData _isValidTrigger;                 // 0x50
        public DifficultyRatios _difficultyMotionRatios;    // 0x54
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GDTPEntry
    {
        public const int Size = 0xA0;

        public GDOREntry _doorHeader;
        public TriggerData _unlockTrigger;
        public TriggerData _pinTrigger1;
        public TriggerData _pinTrigger2;
        public TriggerData _pinTrigger3;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GDBSEntry
    {
        public const int Size = 0x130;

        public GDOREntry _doorHeader;
        public TriggerData _unlockTrigger1;
        public TriggerData _unlockTrigger2;
        public TriggerData _unlockTrigger3;
        public TriggerData _unlockTrigger4;
        public TriggerData _unlockTrigger5;
        public TriggerData _unlockTrigger6;
        public TriggerData _unlockTrigger7;
        public TriggerData _unlockTrigger8;
        public TriggerData _unlockTrigger9;
        public TriggerData _unlockTrigger10;
        public TriggerData _unlockTrigger11;
        public TriggerData _unlockTrigger12;
        public TriggerData _unlockTrigger13;
        public TriggerData _unlockTrigger14;
        public TriggerData _unlockTrigger15;
        public TriggerData _unlockTrigger16;
        public TriggerData _unlockTrigger17;
        public TriggerData _unlockTrigger18;
        public TriggerData _unlockTrigger19;
        public TriggerData _unlockTrigger20;
        public TriggerData _unlockTrigger21;
        public TriggerData _unlockTrigger22;
        public TriggerData _unlockTrigger23;
        public TriggerData _unlockTrigger24;
        public TriggerData _unlockTrigger25;
        public TriggerData _unlockTrigger26;
        public TriggerData _unlockTrigger27;
        public TriggerData _unlockTrigger28;
        public TriggerData _unlockTrigger29;
        public TriggerData _unlockTrigger30;
        public TriggerData _unlockTrigger31;
        public TriggerData _unlockTrigger32;
        public TriggerData _unlockTrigger33;
        public TriggerData _unlockTrigger34;
        public TriggerData _unlockTrigger35;
        public TriggerData _unlockTrigger36;
        public TriggerData _unlockTrigger37;
        public TriggerData _unlockTrigger38;
        public TriggerData _unlockTrigger39;
        public TriggerData _unlockTrigger40;
    }
}