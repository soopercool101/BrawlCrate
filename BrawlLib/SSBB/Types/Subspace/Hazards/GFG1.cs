using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Hazards
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GFG1Entry
    {
        public byte _fighterID;
        public byte _stockCount;
        public byte _costumeID;
        public byte _unknown0x03;
        public byte _unknown0x04;
        public byte _isTeamMember; // 0x5
        public byte _unknown0x06;
        public byte _isMetal; // 0x7
        public byte _unknown0x08;
        public byte _isSpyCloak; // 0x9
        public byte _isLowGravity; // 0xA
        public byte _isNoVoice; // 0xB
        public byte _unknown0x0C;
        public byte _unknown0x0D;
        public byte _unknown0x0E;
        public byte _unknown0x0F;
        public byte _displayStaminaHP; // 0x10
        public byte _unknown0x11;
        public byte _cpuType; // 0x12
        public byte _cpuRank; // 0x13
        public byte _playerNumber; // 0x14
        public byte _costumeType; // 0x15 - [ 0 = Normal | 1 = Dark | 2 = Fake ]
        public byte _unknown0x16;
        public byte _unknown0x17;
        public bshort _startDamage; // 0x18
        public bshort _hitPointMax; // 0x1A
        public byte _unknown0x1C;
        public byte _unknown0x1D;
        public byte _unknown0x1E;
        public byte _unknown0x1F;
        public bshort _glowAttack;
        public bshort _glowDefense;
        public bfloat _offenseKBMult; // 0x24
        public bfloat _defenseKBMult; // 0x28
        public bfloat _scale;         // 0x2C
        public byte _unknown0x30;
        public byte _unknown0x31;
        public byte _unknown0x32;
        public byte _unknown0x33;
        public byte _unknown0x34;
        public byte _unknown0x35;
        public byte _unknown0x36;
        public byte _unknown0x37;
        public byte _unknown0x38;
        public byte _unknown0x39;
        public byte _unknown0x3A;
        public byte _unknown0x3B;
        public byte _unknown0x3C;
        public byte _unknown0x3D;
        public byte _unknown0x3E;
        public byte _unknown0x3F;
        public byte _unknown0x40;
        public byte _unknown0x41;
        public byte _unknown0x42;
        public byte _unknown0x43;
        public byte _unknown0x44;
        public byte _unknown0x45;
        public byte _unknown0x46;
        public byte _unknown0x47;
        public byte _unknown0x48;
        public byte _unknown0x49;
        public byte _unknown0x4A;
        public byte _unknown0x4B;
        public byte _unknown0x4C;
        public byte _unknown0x4D;
        public byte _unknown0x4E;
        public byte _unknown0x4F;
        public TriggerData _trigger;

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