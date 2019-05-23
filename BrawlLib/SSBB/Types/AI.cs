using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct ATKD
    {
        public const uint Tag = 0x444B5441;

        public uint _tag;
        public bint _numEntries;
        public bint _unk1;//0x1CE
        public bint _unk2;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ATKDEntry* entries { get { return (ATKDEntry*)(Address + 0x10); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct ATKDEntry
    {
        public const uint Size = 0x24;

        public bshort _SubActID;//ID of Sub Action
        public bshort _unk1;
        public bshort _StartFrame;
        public bshort _EndFrame;
        public bfloat _xMinRange;
        public bfloat _xMaxRange;
        public bfloat _yMinRange;
        public bfloat _yMaxRange;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public BVec2 MinimumRange { get { return new BVec2(_xMinRange, _yMinRange); } }
        public BVec2 MaximumRange { get { return new BVec2(_xMaxRange, _yMaxRange); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct CEHeader//stands for Condition Evaluation
    {
        public bint _unk1;
        public bint _numEntries;//number of offsets
        public bint _unk2;//0x1
        public bint _unk3;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public bint* entryOffsets { get { return (bint*)(Address + 0x10); } }//contains entry offset. each offset is distance from Address
        public bint* stringOffsets { get { return (bint*)Address + 0x10 * _numEntries; } }//contains string offset but there seems to be other entries on that address
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct CEEntry
    {
        public bint _ID;
        public bint _EventsOffset;
        public bint _part2Offset;
        public bint _unknown;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public VoidPtr Event { get { return (Address + _EventsOffset); } }
        public bfloat* part2 { get { return (bfloat*)(Address + _part2Offset); } }
    }

    public unsafe struct CEEvent
    {
        public sbyte _type;
        public sbyte _numEntries;//sometimes, it is 0
        public bshort _entrySize;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public bint* Entries { get { return (bint*)(Address + 0x4); } }
    }

    public enum CEEventType
    {
        SetReaction = 0x1,
        If = 0x6,
        EndIf = 0x9,
    }

    public unsafe struct CEString
    {
        public bint _numEntries;
        public bint _unk1;
        public bint _unk2;
        public bint _unk3;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public bint* Entries { get { return (bint*)(Address + 0x10); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct AIPD
    {
        public const uint Tag = 0x44504941;
        public const int Size = 0xF;

        public uint _tag;
        public bint DataOffset;//0000000C
        public bint _unk1;
        public bint _unk2;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public VoidPtr DefBlock1 { get { return Address + 0x10; } }
        public VoidPtr DefBlock2 { get { return Address + 0x70; } }
        public VoidPtr SubBlock1 { get { return Address + 0xD0; } }
        public VoidPtr SubBlock2 { get { return Address + 0x100; } }
        public VoidPtr UnkBlock { get { return Address + 0x130; } }
        public VoidPtr Type1Offsets { get { return Address + 0x170; } }//from 0x170 to 0x1CF
        public VoidPtr Type2Offsets { get { return Address + 0x1E0; } }//from0x1E0 to 0x263
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct AIPDDefBlock
    {
        public bfloat _float1;
        public bfloat _float2;
        public bshort _short1;
        public bshort _short2;
        public bshort _short3;
        public bshort _short4;
        public bfloat _float3;
        public bfloat _float4;
        public bshort _short5;
        public bshort _short6;
        public bshort _short7;
        public bshort _short8;
        public bfloat _float5;
        public bshort _short9;
        public bshort _short10;
        public bfloat _float6;
        public bshort _short11;
        public bshort _short12;
        public bfloat _float7;
        public bfloat _float8;
        public bfloat _float9;
        public bfloat _float10;
        public bint _int1;
        public bint _int2;
        public bint _int3;
        public bint _int4;
        public bint _int5;
        public bint _int6;
        public bint _int7;
        public byte _byte1;
        public byte _byte2;
        public byte _byte3;
        public byte _byte4;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct AIPDSubBlock
    {
        public bshort _short1;
        public bshort _short2;
        public bshort _short3;
        public bshort _short4;
        public bfloat _float1;
        public bfloat _float2;
        public bshort _short5;
        public bshort _short6;
        public bshort _short7;
        public bshort _short8;
        public bshort _short9;
        public bshort _short10;
        public byte _byte1;
        public byte _byte2;
        public byte _byte3;
        public byte _byte4;
        public bshort _short11;
        public bshort _short12;
        public bint _int1;
        public bint _int2;
        public bint _int3;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct AIPDUnkBlock
    {
        public const int numEntries = 64;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public byte[] Padding
        {
            get
            {
                byte[] entries = new byte[numEntries];
                for (int i = 0; i < numEntries; i++)
                { entries[i] = ((byte*)Address)[i]; }
                return entries;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct AIPDType1Offsets
    {
        public const int _numEntries = 28;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public bint* Offsets { get { return (bint*)Address; } }
        public List<VoidPtr> Entries
        {
            get
            {
                VoidPtr entry = null;
                List<VoidPtr> ptrs = new List<VoidPtr>();
                for (int i = 0; i < _numEntries - 4; i++)
                { entry = Address - 0x170 + Offsets[i]; if (Offsets[i] > 0)ptrs.Add(entry); else ptrs.Add(0x0); }
                return ptrs;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct AIPDType1
    {
        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public List<VoidPtr> Entries
        {
            get
            {
                List<VoidPtr> ptrs = new List<VoidPtr>();
                AIPDType1Entry* entry = (AIPDType1Entry*)Address;
                while (entry->_command != 0)
                { ptrs.Add(entry); entry++; }
                return ptrs;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct AIPDType1Entry
    {
        public byte _command;
        public byte _control1;
        public byte _control2;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct AIPDType2Offsets
    {
        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public bint* Offsets { get { return (bint*)Address; } }
        public List<VoidPtr> Entries
        {
            get
            {
                AIPDType2* entry = null;
                List<VoidPtr> ptrs = new List<VoidPtr>();
                for (int i = 0; Offsets[i] < 0xFFFF; i++)
                { entry = (AIPDType2*)(Address - 0x1E0 + Offsets[i]); ptrs.Add(entry); }
                return ptrs;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct AIPDType2
    {
        public bshort _id;
        public byte _flag;
        public byte _numEntries;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public List<VoidPtr> Entries
        {
            get
            {
                AIPDType2Entry* entry = (AIPDType2Entry*)(Address + 0x4);
                List<VoidPtr> ptrs = new List<VoidPtr>();
                for (int i = 0; i < _numEntries; i++)
                { ptrs.Add(entry); entry++; }
                return ptrs;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct AIPDType2Entry
    {
        public byte _unk1;
        public byte _unk2;
        public byte _unk3;
        public byte _unk4;
        public bshort _unk5;
        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct AIUnkDef1
    {
        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public AIUnkDef1Entry* Entries { get { return (AIUnkDef1Entry*)Address; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct AIUnkDef1Entry
    {
        public bfloat _unk1;
        public bfloat _unk2;
        public bfloat _unk3;
        public bfloat _unk4;
        public byte _unk5;//seems to be always 0F
        public byte _unk6;//usually 14 but 1E,0A sometimes
        public bint padding;
        public bfloat _unk7;
        public bfloat _unk8;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct AIUnkDef2
    {
        public bfloat _unk1;
        public bfloat _unk2;
        public bfloat _unk3;
        public bint _pad1;
        public bint _pad2;
        public bint _pad3;
        public bint _pad4;
        //I haven't finished this yet.
    }

    public enum SubActionID : short
    {
        Wait1 = 0x000,
        Wait2 = 0x001,
        Wait3 = 0x002,
        Wait4 = 0x003,
        Wait5 = 0x004,
        WaitItem = 0x005,
        ItemHandPickUp = 0x006,
        ItemHandHave = 0x007,
        ItemHandGrip = 0x008,
        ItemHandSmash = 0x009,
        WalkSlow = 0x00A,
        WalkMiddle = 0x00B,
        WalkFast = 0x00C,
        WalkBrake = 0x00D,
        Dash = 0x00E,
        Run = 0x00F,
        RunBrake = 0x010,
        Turn = 0x011,
        TurnRun = 0x012,
        TurnRunBrake = 0x013,
        JumpSquat = 0x014,
        JumpF = 0x015,
        JumpF_ = 0x016,
        JumpB = 0x017,
        JumpB_ = 0x018,
        JumpAerialF = 0x019,
        JumpAerialB = 0x01A,
        JumpAerialF_ = 0x01B,
        JumpAerialF2 = 0x01C,
        JumpAerialF3 = 0x01D,
        JumpAerialF4 = 0x01E,
        JumpAerialF5 = 0x01F,
        Fall = 0x020,
        FallF = 0x021,
        FallB = 0x022,
        FallAerial = 0x023,
        FallAerialF = 0x024,
        FallAerialB = 0x025,
        FallSpecial = 0x026,
        FallSpecialF = 0x027,
        FallSpecialB = 0x028,
        DamageFall = 0x029,
        Squat = 0x02A,
        SquatWait = 0x02B,
        SquatWait2 = 0x02C,
        SquatWaitItem = 0x02D,
        SquatF = 0x02E,
        SquatB = 0x02F,
        SquatRv = 0x030,
        LandingLight = 0x031,
        LandingHeavy = 0x032,
        LandingFallSpecial = 0x033,
        StepJump = 0x034,
        StepPose = 0x035,
        StepBack = 0x036,
        StepAirPose = 0x037,
        StepFall = 0x038,
        GlideStart = 0x039,
        GlideDirection = 0x03A,
        GlideWing = 0x03B,
        GlideAttack = 0x03C,
        GlideEnd = 0x03D,
        GlideLanding = 0x03E,
        GuardOn = 0x03F,
        Guard = 0x040,
        GuardOff = 0x041,
        GuardDamage = 0x042,
        EscapeN = 0x043,
        EscapeF = 0x044,
        EscapeB = 0x045,
        EscapeAir = 0x046,
        Rebound = 0x047,
        Attack11 = 0x048,
        Attack12 = 0x049,
        Attack13 = 0x04A,
        Attack100Start = 0x04B,
        Attack100 = 0x04C,
        AttackEnd = 0x04D,
        AttackDash = 0x04E,
        AttackS3Hi = 0x04F,
        AttackS3S = 0x050,
        AttackS3S2 = 0x051,
        AttackS3S3 = 0x052,
        AttackS3Lw = 0x053,
        AttackHi3 = 0x054,
        AttackLw3 = 0x055,
        AttackS4Start = 0x056,
        AttackS4S = 0x057,
        AttackS4S_ = 0x058,
        AttackS4S2 = 0x059,
        AttackS4S__ = 0x05A,
        AttackS4Hold = 0x05B,
        AttackHi4Start = 0x05C,
        AttackHi4 = 0x05D,
        AttackHi4Hold = 0x05E,
        AttackLw4Start = 0x05F,
        AttackLw4 = 0x060,
        AttackLw4Hold = 0x061,
        AttackAirN = 0x062,
        AttackAirF = 0x063,
        AttackAirB = 0x064,
        AttackAirHi = 0x065,
        AttackAirLw = 0x066,
        LandingAirN = 0x067,
        LandingAirF = 0x068,
        LandingAirB = 0x069,
        LandingAirHi = 0x06A,
        LandingAirLw = 0x06B,
        Catch = 0x06C,
        CatchDash = 0x06D,
        CatchTurn = 0x06E,
        CatchWait = 0x06F,
        CatchAttack = 0x070,
        CatchCut = 0x071,
        ThrowB = 0x072,
        ThrowF = 0x073,
        ThrowHi = 0x074,
        ThrowLw = 0x075,
        ThrownB = 0x076,
        ThrownF = 0x077,
        ThrownHi = 0x078,
        ThrownLw = 0x079,
        ThrownDxB = 0x07A,
        ThrownDxF = 0x07B,
        ThrownDxHi = 0x07C,
        ThrownDxLw = 0x07D,
        CapturePulledHi = 0x07E,
        CaptureWaitHi = 0x07F,
        CaptureDamageHi = 0x080,
        CapturePulledLw = 0x081,
        CaptureWaitLw = 0x082,
        CaptureDamageLw = 0x083,
        CapturePulledSnake = 0x084,
        CaptureWaitSnake = 0x085,
        CaptureDamageSnake = 0x086,
        CapturePulledSnake_ = 0x087,
        CaptureWaitSnake_ = 0x088,
        CaptureDamageSnake_ = 0x089,
        CapturePulledDxSnake = 0x08A,
        CaptureWaitDxSnake = 0x08B,
        CaptureDamageDxSnake = 0x08C,
        CapturePulledDxSnake_ = 0x08D,
        CaptureWaitDxSnake_ = 0x08E,
        CaptureDamageDxSnake_ = 0x08F,
        CapturePulledBigSnake = 0x090,
        CaptureWaitBigSnake = 0x091,
        CaptureDamageBigSnake = 0x092,
        CapturePulledBigSnake_ = 0x093,
        CaptureWaitBigSnake_ = 0x094,
        CaptureDamageBigSnake_ = 0x095,
        CaptureCut = 0x096,
        CaptureJump = 0x097,
        DamageHi1 = 0x098,
        DamageHi2 = 0x099,
        DamageHi3 = 0x09A,
        DamageN1 = 0x09B,
        DamageN2 = 0x09C,
        DamageN3 = 0x09D,
        DamageLw1 = 0x09E,
        DamageLw2 = 0x09F,
        DamageLw3 = 0x0A0,
        DamageAir1 = 0x0A1,
        DamageAir2 = 0x0A2,
        DamageAir3 = 0x0A3,
        DamageFlyHi = 0x0A4,
        DamageFlyN = 0x0A5,
        DamageFlyLw = 0x0A6,
        DamageFlyTop = 0x0A7,
        DamageFlyRoll = 0x0A8,
        DamageElec = 0x0A9,
        DownBoundU = 0x0AA,
        DownWaitU = 0x0AB,
        DownDamageU = 0x0AC,
        DownDamageU3 = 0x0AD,
        DownEatU = 0x0AE,
        DownStandU = 0x0AF,
        DownAttackU = 0x0B0,
        DownForwardU = 0x0B1,
        DownBackU = 0x0B2,
        DownBoundD = 0x0B3,
        DownWaitD = 0x0B4,
        DownDamageD = 0x0B5,
        DownDamageD3 = 0x0B6,
        DownEatD = 0x0B7,
        DownStandD = 0x0B8,
        DownAttackD = 0x0B9,
        DownForwardD = 0x0BA,
        DownBackD = 0x0BB,
        DownSpotU = 0x0BC,
        DownSpotD = 0x0BD,
        Passive = 0x0BE,
        PassiveStandF = 0x0BF,
        PassiveStandB = 0x0C0,
        PassiveWall = 0x0C1,
        PassiveWallJump = 0x0C2,
        PassiveCeil = 0x0C3,
        PassiveWall_ = 0x0C4,
        FuraFura = 0x0C5,
        FuraFuraStartD = 0x0C6,
        FuraFuraStartU = 0x0C7,
        FuraFuraEnd = 0x0C8,
        FuraSleepStart = 0x0C9,
        FuraSleepLoop = 0x0CA,
        FuraSleepEnd = 0x0CB,
        Swallowed = 0x0CC,
        Pass = 0x0CD,
        Ottotto = 0x0CE,
        OttottoWait = 0x0CF,
        WallDamage = 0x0D0,
        StopCeil = 0x0D1,
        StopWall = 0x0D2,
        StopCeil_ = 0x0D3,
        MissFoot = 0x0D4,
        CliffCatch = 0x0D5,
        CliffWait = 0x0D6,
        CliffAttackQuick = 0x0D7,
        CliffClimbQuick = 0x0D8,
        CliffEscapeQuick = 0x0D9,
        CliffJumpQuick1 = 0x0DA,
        CliffJumpQuick2 = 0x0DB,
        CliffAttackSlow = 0x0DC,
        CliffClimbSlow = 0x0DD,
        CliffEscapeSlow = 0x0DE,
        CliffJumpSlow1 = 0x0DF,
        CliffJumpSlow2 = 0x0E0,
        SlipDown = 0x0E1,
        Slip = 0x0E2,
        SlipTurn = 0x0E3,
        SlipDash = 0x0E4,
        SlipWait = 0x0E5,
        SlipStand = 0x0E6,
        SlipAttack = 0x0E7,
        SlipEscapeF = 0x0E8,
        SlipEscapeB = 0x0E9,
        AirCatch = 0x0EA,
        AirCatchPose = 0x0EB,
        AirCatchHit = 0x0EC,
        AirCatch_ = 0x0ED,
        SwimRise = 0x0EE,
        SwimUp = 0x0EF,
        SwimUpDamage = 0x0F0,
        Swim = 0x0F1,
        SwimF = 0x0F2,
        SwimEnd = 0x0F3,
        SwimTurn = 0x0F4,
        SwimDrown = 0x0F5,
        SwimDrownOut = 0x0F6,
        LightGet = 0x0F7,
        LightWalkGet = 0x0F8,
        LightEat = 0x0F9,
        LightWalkEat = 0x0FA,
        HeavyGet = 0x0FB,
        HeavyWalk1 = 0x0FC,
        HeavyWalk2 = 0x0FD,
        LightThrowDrop = 0x0FE,
        LightThrowF = 0x0FF,
        LightThrowB = 0x100,
        LightThrowHi = 0x101,
        LightThrowLw = 0x102,
        LightThrowF_ = 0x103,
        LightThrowB_ = 0x104,
        LightThrowHi_ = 0x105,
        LightThrowLw_ = 0x106,
        LightThrowDash = 0x107,
        LightThrowAirF = 0x108,
        LightThrowAirB = 0x109,
        LightThrowAirHi = 0x10A,
        LightThrowAirLw = 0x10B,
        LightThrowAirF_ = 0x10C,
        LightThrowAirB_ = 0x10D,
        LightThrowAirHi_ = 0x10E,
        LightThrowAirLw_ = 0x10F,
        HeavyThrowF = 0x110,
        HeavyThrowB = 0x111,
        HeavyThrowHi = 0x112,
        HeavyThrowLw = 0x113,
        HeavyThrowF_ = 0x114,
        HeavyThrowB_ = 0x115,
        HeavyThrowHi_ = 0x116,
        HeavyThrowLw_ = 0x117,
        SmashThrowF = 0x118,
        SmashThrowB = 0x119,
        SmashThrowHi = 0x11A,
        SmashThrowLw = 0x11B,
        SmashThrowDash = 0x11C,
        SmashThrowAirF = 0x11D,
        SmashThrowAirB = 0x11E,
        SmashThrowAirHi = 0x11F,
        SmashThrowAirLw = 0x120,
        Swing1 = 0x121,
        Swing3 = 0x122,
        Swing4Start = 0x123,
        Swing4 = 0x124,
        Swing42 = 0x125,
        Swing4Hold = 0x126,
        SwingDash = 0x127,
        Swing1_ = 0x128,
        Swing3_ = 0x129,
        Swing4Bat = 0x12A,
        SwingDash_ = 0x12B,
        Swing1__ = 0x12C,
        Swing3__ = 0x12D,
        Swing4Start_ = 0x12E,
        Swing4_ = 0x12F,
        Swing42_ = 0x130,
        Swing4Hold_ = 0x131,
        SwingDash__ = 0x132,
        Swing1___ = 0x133,
        Swing3___ = 0x134,
        Swing4Start__ = 0x135,
        Swing4__ = 0x136,
        Swing42__ = 0x137,
        Swing4Hold__ = 0x138,
        SwingDash___ = 0x139,
        Swing1____ = 0x13A,
        Swing3____ = 0x13B,
        Swing4Start___ = 0x13C,
        Swing4___ = 0x13D,
        Swing42___ = 0x13E,
        Swing4Hold___ = 0x13F,
        SwingDash____ = 0x140,
        ItemHammerWait = 0x141,
        ItemHammerMove = 0x142,
        ItemHammerAir = 0x143,
        ItemHammerWait_ = 0x144,
        ItemHammerMove_ = 0x145,
        ItemHammerAir_ = 0x146,
        ItemDragoonRide = 0x147,
        ItemScrew = 0x148,
        ItemScrew_ = 0x149,
        ItemScrewFall = 0x14A,
        ItemKikc = 0x14B,
        ItemDragoonGet = 0x14C,
        ItemDragoonRide_ = 0x14D,
        ItemBIg = 0x14E,
        ItemSmall = 0x14F,
        ItemLegsWait = 0x150,
        ItemLegsSlowF = 0x151,
        ItemLegsMiddleF = 0x152,
        ItemLegsFastF = 0x153,
        ItemLegsBrakeF = 0x154,
        ItemLegsDashF = 0x155,
        ItemLegsSlowB = 0x156,
        ItemLegsMiddleB = 0x157,
        ItemLegsFastB = 0x158,
        ItemLegsBrakeB = 0x159,
        ItemLegsDashB = 0x15A,
        ItemLegsJumpSquat = 0x15B,
        ItemLegsLanding = 0x15C,
        ItemShoot = 0x15D,
        ItemShootAir = 0x15E,
        ItemShoot_ = 0x15F,
        ItemShootAir__ = 0x160,
        ItemShoot__ = 0x161,
        ItemShootAir___ = 0x162,
        ItemScopeStart = 0x163,
        ItemScopeRapid = 0x164,
        ItemScopeFire = 0x165,
        ItemScopeEnd = 0x166,
        ItemScopeAirStart = 0x167,
        ItemScopeAirRapid = 0x168,
        ItemScopeAirFire = 0x169,
        ItemScopeAirEnd = 0x16A,
        ItemScopeStart_ = 0x16B,
        ItemScopeRapid_ = 0x16C,
        ItemScopeFire_ = 0x16D,
        ItemScopeEnd_ = 0x16E,
        ItemScopeAirStart_ = 0x16F,
        ItemScopeAirRapid_ = 0x170,
        ItemScopeAirFire_ = 0x171,
        ItemScopeAirEnd_ = 0x172,
        ItemLauncher = 0x173,
        ItemLauncherFire = 0x174,
        ItemLauncherAirFire = 0x175,
        ItemLauncher_ = 0x176,
        ItemLauncherFire_ = 0x177,
        ItemLauncherAirFire_ = 0x178,
        ItemLauncherFall = 0x179,
        ItemLauncherAir = 0x17A,
        ItemAssist = 0x17B,
        GekikaraWait = 0x17C,
        ItemScrew__ = 0x17D,
        Guard_ = 0x17E,
        LadderWait = 0x17F,
        LadderUp = 0x180,
        LadderDown = 0x181,
        LadderCatchR = 0x182,
        LadderCatchL = 0x183,
        LadderCatchAirR = 0x184,
        LadderCatchAirL = 0x185,
        LadderCatchEndR = 0x186,
        LadderCatchEndL = 0x187,
        RopeCatch = 0x188,
        RopeFishing = 0x189,
        SpecialNBittenStart = 0x18A,
        SpecialNBitten = 0x18B,
        SpecialNBittenEnd = 0x18C,
        SpecialAirNBittenStart = 0x18D,
        SpecialAirNBitten = 0x18E,
        SpecialAirNBittenEnd = 0x18F,
        SpecialNDxBittenStart = 0x190,
        SpecialNDxBitten = 0x191,
        SpecialNDxBittenEnd = 0x192,
        SpecialAirNDxBittenStart = 0x193,
        SpecialAirNDxBitten = 0x194,
        SpecialAirNDxBittenEnd = 0x195,
        SpecialNBigBittenStart = 0x196,
        SpecialNBigBitten = 0x197,
        SpecialNBigBittenEnd = 0x198,
        SpecialAirNBigBittenStart = 0x199,
        SpecialAirNBigBitten = 0x19A,
        SpecialAirNBigBittenEnd = 0x19B,
        SpecialHiCapture = 0x19C,
        SpecialHiDxCapture = 0x19D,
        SpecialSStickCapture = 0x19E,
        SpecialSStickAttackCapture = 0x19F,
        SpecialSStickJumpCapture = 0x1A0,
        SpecialSDxStickCapture = 0x1A1,
        SpecialSDxStickAttackCapture = 0x1A2,
        SpecialSDXStickJumpCapture = 0x1A3,
        ThrownZitabata = 0x1A4,
        ThrownDxZitabata = 0x1A5,
        ThrownGirlZitabata = 0x1A6,
        ThrownFF = 0x1A7,
        ThrownFB = 0x1A8,
        ThrownFHi = 0x1A9,
        ThrownFLw = 0x1AA,
        ThrownDxFF = 0x1AB,
        ThrownDxFB = 0x1AC,
        ThrownDxFHi = 0x1AD,
        ThrownDxFLw = 0x1AE,
        GanonSpecialHiCapture = 0x1AF,
        GanonSpecialHiDxCapture = 0x1B0,
        SpecialSCapture = 0x1B1,
        SpecailAirSCatchCapture = 0x1B2,
        SpecialAirSFallCapture = 0x1B3,
        SpecialAirSCapture = 0x1B4,
        SpecialSDxCapture = 0x1B5,
        SpecailAirSDxCatchCapture = 0x1B6,
        SpecialAirSDxFallCapture = 0x1B7,
        SpecialAirSDxCapture = 0x1B8,
        SpecialNEgg = 0x1B9,
        SpecialSZitabata = 0x1BA,
        SpecialSDxZitabata = 0x1BB,
        AppealHiR = 0x1BC,
        AppealHiL = 0x1BD,
        AppealSR = 0x1BE,
        AppealSL = 0x1BF,
        AppealLwR = 0x1C0,
        AppealLwL = 0x1C1,
        EntryR = 0x1C2,
        EntryL = 0x1C3,
        Win1 = 0x1C4,
        Win1Wait = 0x1C5,
        Win2 = 0x1C6,
        Win2Wait = 0x1C7,
        Win3 = 0x1C8,
        Win3Wait = 0x1C9,
        Lose = 0x1CA,
        DamageFace = 0x1CB,
        Dark = 0x1CC,
        Spycloak = 0x1CD,
    }
}
