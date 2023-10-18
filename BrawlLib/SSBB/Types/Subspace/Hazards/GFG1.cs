using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Hazards
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GFG1Entry
    {
        public const int Size = 0x54;

        public byte _fighterID; // 0x00
        public byte _stockCount; // 0x01
        public byte _costumeID; // 0x02
        public byte _unknown0x03;
        public byte _unknown0x04;
        public bool8 _isTeamMember; // 0x05
        public byte _unknown0x06;
        public bool8 _isMetal; // 0x07
        public byte _unknown0x08;
        public bool8 _isSpy; // 0x09
        public bool8 _isLowGravity; // 0x0A
        public bool8 _hasNoVoice; // 0x0B
        public byte _unknown0x0C;
        public byte _unknown0x0D;
        public byte _unknown0x0E;
        public byte _unknown0x0F;
        public bool8 _displayStaminaHitPoints; // 0x10
        public byte _unknown0x11;
        public byte _cpuType; // 0x12
        public byte _cpuRank; // 0x13
        public byte _playerNumber; // 0x14
        public byte _costumeType; // 0x15
        public byte _unknown0x16;
        public byte _unknown0x17;
        public bshort _startDamage; // 0x18
        public bshort _hitPointMax; // 0x1A
        public buint _unknown0x1C;
        public bshort _glowAttack; // 0x20
        public bshort _glowDefense; // 0x22
        public bfloat _offensiveKnockbackMultiplier; // 0x24
        public bfloat _defensiveKnockbackMultiplier; // 0x28
        public bfloat _scale; // 0x2C
        public buint _unknown0x30;
        public buint _unknown0x34;
        public buint _unknown0x38;
        public buint _unknown0x3C;
        public buint _unknown0x40;
        public buint _unknown0x44;
        public buint _unknown0x48;
        public TriggerData _unknown0x4C;
        public TriggerData _unknown0x50;
    }
}