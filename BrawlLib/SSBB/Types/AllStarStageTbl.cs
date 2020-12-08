using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct AllstarStageTbl
    {
        public const int Size = 420;

        public bint _stage1;
        public bint _stage2;
        public bint _stage3;
        public bint _stage4;
        public bint _stage5;
        public AllstarFighterData _opponent1;
        public AllstarFighterData _opponent2;
        public AllstarFighterData _opponent3;
        public AllstarFighterData _opponent4;
        public AllstarFighterData _opponent5;

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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AllstarFighterData
    {
        public const int Size = 0x50;

        public byte _fighterID;
        public byte _unknown01;
        public byte _unknown02;
        public byte _unknown03;
        public bfloat _unknown04;
        public AllstarDifficultyData _difficulty1;
        public AllstarDifficultyData _difficulty2;
        public AllstarDifficultyData _difficulty3;
        public AllstarDifficultyData _difficulty4;
        public AllstarDifficultyData _difficulty5;
        public byte _padding4e;
        public byte _padding4f;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AllstarDifficultyData
    {
        public const int Size = 0x0E;

        public byte _unknown00;
        public byte _unknown01;
        public byte _unknown02;
        public byte _unknown03;
        public bshort _offenseRatio;
        public bshort _defenseRatio;
        public byte _unknown08;
        public byte _color;
        public byte _stock;
        public byte _unknown0b;
        public bshort _unknown0c;
    }
}