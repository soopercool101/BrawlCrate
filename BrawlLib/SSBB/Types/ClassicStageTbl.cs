using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ClassicStageBlock
    {
        public const int Size = 260;

        public ClassicStageBlockStageData _stages;
        public AllstarFighterData _opponent1;
        public AllstarFighterData _opponent2;
        public AllstarFighterData _opponent3;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ClassicStageBlockStageData {
        public const int Size = 20;

        public bint _unknown00;
        public bint _stageID1;
        public bint _stageID2;
        public bint _stageID3;
        public bint _stageID4;

        private VoidPtr Address { get { fixed (void* ptr = &this) return ptr; } }
    }
}
