using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.SSEEX
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SELC
    {
        public const int Size = 0x10;

        public byte _characterCount;
        public sbyte _stockCount;
        public byte _unlockSetting;
        public byte _disableSubfighterSelection;
        public byte _sublevelChanger;
        public sbyte _rosterMode;
        public byte _minimumUnlocks;
        public byte _addSurvivingMembers;
        public byte _team1Count;
        public byte _team2Count;
        public byte _team3Count;
        public byte _team4Count;
        public byte _team5Count;
        public byte _team6Count;
        public byte _team7Count;
        public byte _team8Count;

        public VoidPtr this[int index] => (byte*)Address + Size + index;

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

        public byte TeamXCount(byte team)
        {
            return *((byte*) (Address + 0x8 + team));
        }
    }
}