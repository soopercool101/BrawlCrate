using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ClassicStageBlock
    {
        public const int Size = 260;

        public ClassicStageBlockStageData _stages;
        public ClassicFighterData _opponent1;
        public ClassicFighterData _opponent2;
        public ClassicFighterData _opponent3;

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
    public unsafe struct ClassicStageBlockStageData
    {
        public const int Size = 20;

        public bint _battleType;
        public bint _stageID1;
        public bint _stageID2;
        public bint _stageID3;
        public bint _stageID4;

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
    public struct ClassicFighterData
    {
        public const int Size = 0x50;

        public byte _fighterID;
        public byte _fighterstatus;
        public byte _unknown02;
        public byte _unknown03;
        public bfloat _fighterscale;
        public ClassicDifficultyData _difficulty1;
        public ClassicDifficultyData _difficulty2;
        public ClassicDifficultyData _difficulty3;
        public ClassicDifficultyData _difficulty4;
        public ClassicDifficultyData _difficulty5;
        public byte _padding4e;
        public byte _padding4f;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ClassicDifficultyData
    {
        public const int Size = 0x0E;

        public byte _unknown00;
        public byte _handicap;
        public byte _aiLevel;
        public byte _unknown03;
        public bshort _offenseRatio;
        public bshort _defenseRatio;
        public byte _aiType;
        public byte _color;
        public byte _stock;
        public byte _unknown0b;
        public bshort _unknown0c;
    }
}