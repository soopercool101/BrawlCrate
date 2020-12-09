using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Stage_Tables
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TBGCEntry // TBGC
    {
        public const int Size = 0x8;

        public byte _unk0x00;
        public byte _unk0x01;
        public byte _unk0x02;
        public byte _unk0x03;
        public byte _unk0x04;
        public byte _unk0x05;
        public byte _unk0x06;
        public byte _unk0x07;
    }
}