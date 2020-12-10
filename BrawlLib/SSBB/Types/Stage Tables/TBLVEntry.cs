using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Stage_Tables
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TBLVEntry // Table (Lava)
    {
        public const int Size = 0xC;

        public bfloat _height; // 0x0 - Lava Height
        public bfloat _unk0x4;
        public bfloat _unk0x8;
    }
}