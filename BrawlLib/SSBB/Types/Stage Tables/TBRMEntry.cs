using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Stage_Tables
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TBRMEntry // TBRM
    {
        public const int Size = 0x14;

        public bint _unk0x00;
        public bshort _unk0x04;
        public bshort _unk0x06;
        public bshort _unk0x08;
        public bshort _unk0x0A;
        public bshort _unk0x0C;
        public bshort _unk0x0E;
        public bfloat _unk0x10;
    }
}