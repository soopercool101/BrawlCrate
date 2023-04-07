using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Stage_Tables
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TBCLEntry
    {
        public const int Size = 0x3C;

        public byte _count;         // 0x00
        public byte _unknown0x01;   // 0x01
        public byte _unknown0x02;   // 0x02
        public byte _unknown0x03;   // 0x03
        public bfloat _unknown0x04; // 0x04
        public bfloat _unknown0x08; // 0x08
        public bfloat _unknown0x0C; // 0x0C
        public bfloat _unknown0x10; // 0x10
        public bfloat _unknown0x14; // 0x14
        public bfloat _unknown0x18; // 0x18
        public bfloat _unknown0x1C; // 0x1C
        public bfloat _unknown0x20; // 0x20
        public bfloat _unknown0x24; // 0x24
        public bfloat _unknown0x28; // 0x28
        public bfloat _unknown0x2C; // 0x2C
        public bfloat _unknown0x30; // 0x30
        public bfloat _unknown0x34; // 0x34
        public bfloat _unknown0x38; // 0x38
    }
}