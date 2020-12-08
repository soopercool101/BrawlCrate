using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Stage_Tables
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TBGMEntry // TBGM
    {
        public const int Size = 0x18;

        public bfloat _unk0x00;
        public bfloat _unk0x04;
        public bfloat _unk0x08;
        public bfloat _unk0x0C;
        public bfloat _unk0x10;
        public bfloat _unk0x14;
    }
}