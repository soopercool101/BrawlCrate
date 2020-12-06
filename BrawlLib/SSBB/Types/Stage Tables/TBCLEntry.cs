using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Stage_Tables
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TBCLEntry
    {
        public const int Size = 0x3C;

        public byte _count;       // 0x0
        public byte _unk0x1;      // 0x1
        public byte _unk0x2;      // 0x2
        public byte _unk0x3;      // 0x3
        public bfloat _collObj1;  // 0x4
        public bfloat _collObj2;  // 0x8
        public bfloat _collObj3;  // 0xC
        public bfloat _collObj4;  // 0x10
        public bfloat _collObj5;  // 0x10
        public bfloat _collObj6;  // 0x14
        public bfloat _collObj7;  // 0x18
        public bfloat _collObj8;  // 0x1C
        public bfloat _collObj9;  // 0x20
        public bfloat _collObj10; // 0x24
        public bfloat _collObj11; // 0x28
        public bfloat _collObj12; // 0x2C
        public bfloat _collObj13; // 0x30
        public bfloat _collObj14; // 0x34
    }
}