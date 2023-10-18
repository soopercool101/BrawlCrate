using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GMWOEntry
    {
        public const int Size = 0x98;

        public bfloat _unknown0x00;
        public bfloat _unknown0x04;
        public buint _unknown0x08;
        public buint _unknown0x0C;
        public byte _unknown0x10;
        public byte _unknown0x11;
        public byte _unknown0x12;
        public byte _unknown0x13;
        public bfloat _unknown0x14;
        public bool8 _unknown0x18;
        public byte _unknown0x19;
        public byte _unknown0x1A;
        public byte _unknown0x1B;
        public bfloat _unknown0x1C;
        public byte _unknown0x20;
        public byte _unknown0x21;
        public byte _unknown0x22;
        public byte _unknown0x23;
        public buint _unknown0x24;
        public buint _unknown0x28;
        public TriggerData _unknown0x2C;
        public TriggerData _unknown0x30;
        public buint _unknown0x34;
        public buint _unknown0x38;
        public buint _unknown0x3C;
        public buint _unknown0x40;
        public buint _unknown0x44;
        public buint _unknown0x48;
        public buint _unknown0x4C;
        public buint _unknown0x50;
        public buint _unknown0x54;
        public buint _unknown0x58;
        public buint _unknown0x5C;
        public buint _unknown0x60;
        public buint _unknown0x64;
        public buint _unknown0x68;
        public buint _unknown0x6C;
        public buint _unknown0x70;
        public bfloat _unknown0x74;
        public buint _unknown0x78;
        public buint _unknown0x7C;
        public bfloat _unknown0x80;
        public buint _unknown0x84;
        public bfloat _unknown0x88;
        public byte _unknown0x8C;
        public byte _unknown0x8D;
        public byte _unknown0x8E;
        public byte _unknown0x8F;
        public buint _unknown0x90;
        public bshort _unknown0x94;
        public bshort _unknown0x96;
    }
}