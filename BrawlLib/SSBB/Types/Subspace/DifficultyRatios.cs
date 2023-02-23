using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DifficultyRatios
    {
        public const int Size = 0x3C;

        public bfloat _difficulty1;     // 0x00
        public bfloat _difficulty2;     // 0x04
        public bfloat _difficulty3;     // 0x08
        public bfloat _difficulty4;     // 0x0C
        public bfloat _difficulty5;     // 0x10
        public bfloat _difficulty6;     // 0x14
        public bfloat _difficulty7;     // 0x18
        public bfloat _difficulty8;     // 0x1C
        public bfloat _difficulty9;     // 0x20
        public bfloat _difficulty10;    // 0x24
        public bfloat _difficulty11;    // 0x28
        public bfloat _difficulty12;    // 0x2C
        public bfloat _difficulty13;    // 0x30
        public bfloat _difficulty14;    // 0x34
        public bfloat _difficulty15;    // 0x38
    }
}
