using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    public enum AttackElementType : uint
    {
        Normal = 0x0,
        None0x01 = 0x1,
        Slash = 0x2,
        Electric = 0x3,
        Freezing = 0x4,
        Flame = 0x5,
        Coin = 0x6,
        Reverse = 0x7,
        Trip = 0x8,
        Sleep = 0x9,
        None0x0A = 0xA,
        Bury = 0xB,
        Stun = 0xC,
        Sparkle = 0xD,
        Flower = 0xE,
        YellowSteam = 0xF,
        None0x10 = 0x10,
        Grass = 0x11,
        Water = 0x12,
        Darkness = 0x13,
        Paralyze = 0x14,
        Aura = 0x15,
        Plunge = 0x16,
        Down = 0x17,
        Flinchless = 0x18,
        None0x19 = 0x19,
        None0x1A = 0x1A,
        None0x1B = 0x1B,
        None0x1C = 0x1C,
        None0x1D = 0x1D,
        None0x1E = 0x1E,
        None0x1F = 0x1F
    }

    public enum HitSoundType : uint
    {
        NoneUnique = 0x0,
        Punch = 0x1,
        Kick = 0x2,
        Slash = 0x3,
        Coin = 0x4,
        Bat = 0x5,
        Paper = 0x6,        // (Harisen)
        Electric = 0x7,
        Fire = 0x8,
        Water = 0x9,
        Blank = 0xA,
        Explosion = 0xB,
        Blank2 = 0xC,
        SnakeThud = 0xD,    // Exclusive to Snake
        IkeSlam = 0xE,      // Exclusive to Ike
        DededeThwomp = 0xF, // Exclusive to Dedede
        Magic = 0x10,
        Shell = 0x11,
        PeachSlap = 0x12,   // Exclusive to Peach
        PeachPan = 0x13,    // Exclusive to Peach
        PeachClub = 0x14,   // Exclusive to Peach
        PeachRacket = 0x15, // Exclusive to Peach
        LucarioAura = 0x16, // Exclusive to Lucario
        MarthTreble = 0x17, // Exclusive to Marth
        MarioCoin = 0x18,   // Exclusive to Mario
        MarioStatic = 0x19, // Exclusive to Mario
        LuigiCoin = 0x1A,   // Exclusive to Luigi
        NessBat = 0x1B,     // Exclusive to Ness
        Frost = 0x1C
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AttackData
    {
        public const int Size = 0x58;

        public bfloat _damage;                  // 0x00
        public bfloat _offsetPosX;              // 0x04
        public bfloat _offsetPosY;              // 0x08
        public bfloat _offsetPosZ;              // 0x0C
        public bfloat _size;                    // 0x10
        public bint _vector;                    // 0x14
        public bint _reactionEffect;            // 0x18
        public bint _reactionFix;               // 0x1C
        public bint _reactionAdd;               // 0x20
        public buint _unknown0x24;
        public buint _elementType;              // 0x28
        public byte _isClankable;               // 0x2C
        public byte _unknown0x2D;
        public byte _unknown0x2E;
        public byte _unknown0x2F;
        public buint _unknown0x30;
        public buint _unknown0x34;
        public buint _unknown0x38;
        public buint _detectionRate;            // 0x3C
        public buint _hitSoundLevel;            // 0x40
        public buint _hitSoundType;             // 0x44
        public byte _unknown0x48;
        public byte _isShapeCapsule;            // 0x49
        public byte _unknown0x4A;
        public byte _unknown0x4B;
        public buint _unknown0x4C;
        public buint _nodeIndex;                // 0x50
        public bint _power;                     // 0x54
    }
}
