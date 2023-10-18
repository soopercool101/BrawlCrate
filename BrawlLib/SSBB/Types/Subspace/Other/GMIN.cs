using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Other
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GMINEntry
    {
        public const int Size = 0x90;

        public bfloat _unknown0x00;
        public buint _unknown0x04;
        public buint _unknown0x08;
        public buint _unknown0x0C;
        public bfloat _unknown0x10;
        public buint _unknown0x14;
        public buint _unknown0x18;
        public buint _unknown0x1C;
        public buint _unknown0x20;
        public buint _unknown0x24;
        public buint _unknown0x28;
        public byte _unknown0x2C;
        public byte _unknown0x2D;
        public byte _unknown0x2E;
        public byte _unknown0x2F;
        public buint _unknown0x30;
        public buint _unknown0x34;
        public buint _unknown0x38;
        public buint _unknown0x3C;
        public byte _unknown0x40;
        public byte _unknown0x41;
        public byte _unknown0x42;
        public byte _unknown0x43;
        public buint _unknown0x44;
        public buint _unknown0x48;
        public buint _unknown0x4C;
        public buint _unknown0x50;
        public byte _unknown0x54;
        public byte _unknown0x55;
        public byte _unknown0x56;
        public byte _unknown0x57;
        public buint _unknown0x58;
        public buint _unknown0x5C;
        public buint _unknown0x60;
        public buint _unknown0x64;
        public buint _unknown0x68;
        public buint _unknown0x6C;
        public buint _unknown0x70;
        public bfloat _unknown0x74;
        public bshort _unknown0x78;
        public bshort _unknown0x7A;
        public buint _unknown0x7C;
        public TriggerData _unknown0x80;
        public byte _unknown0x84;
        public byte _unknown0x85;
        public byte _unknown0x86;
        public byte _unknown0x87;
        public bfloat _unknown0x88;
        public byte _unknown0x8C;
        public byte _unknown0x8D;
        public byte _unknown0x8E;
        public byte _unknown0x8F;
    }
}