using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Stage_Tables
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TBGDEntry // TBGD
    {
        public const int Size = 0x14;

        public byte _unk0x00;   // ID?
        public byte _unk0x01;   // Padding?
        public byte _unk0x02;   // Padding?
        public byte _unk0x03;   // Padding?
        public bfloat _unk0x04; // Probability?
        public bool8 _unk0x08;  // Everything from here onwards appears to be a byte boolean
        public bool8 _unk0x09;
        public bool8 _unk0x0A;
        public bool8 _unk0x0B;
        public bool8 _unk0x0C;
        public bool8 _unk0x0D;
        public bool8 _unk0x0E;
        public bool8 _unk0x0F;
        public bool8 _unk0x10;
        public bool8 _unk0x11;
        public bool8 _unk0x12;
        public bool8 _unk0x13;
    }
}