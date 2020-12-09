using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Stage_Tables
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TBSTEntry // TBST
    {
        public const int Size = 0x14;

        // Every valid value here appears to be 0x00, 0x01 or 0x0A
        public byte _unk0x00;
        public byte _unk0x01;
        public byte _unk0x02;
        public byte _unk0x03;
        public byte _unk0x04;
        public byte _unk0x05;
        public byte _unk0x06;
        public byte _unk0x07;
        public byte _unk0x08;
        public byte _unk0x09;
        public byte _unk0x0A;
        public byte _unk0x0B;
        public byte _unk0x0C;
        public byte _unk0x0D;
        public byte _unk0x0E;
        public byte _unk0x0F;
        public byte _unk0x10;
        public byte _unk0x11;
        public byte _unk0x12;
        public byte _unk0x13;
    }
}