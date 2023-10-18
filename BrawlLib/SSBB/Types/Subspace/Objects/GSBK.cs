using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GSBKEntry
    {
        public const int Size = 0x58;

        public buint _unknown0x00;
        public buint _unknown0x04;
        public buint _unknown0x08;
        public buint _unknown0x0C;
        public buint _unknown0x10;
        public buint _unknown0x14;
        public bfloat _unknown0x18;
        public bshort _unknown0x1C;
        public bshort _unknown0x1E;
        public buint _unknown0x20;
        public byte _unknown0x24;
        public byte _unknown0x25;
        public byte _unknown0x26;
        public byte _unknown0x27;
        public bshort _unknown0x28;
        public bshort _unknown0x2A;
        public byte _unknown0x2C;
        public byte _unknown0x2D;
        public byte _unknown0x2E;
        public byte _unknown0x2F;
        public bint _unknown0x30;
        public bint _unknown0x34;
        public bool8 _unknown0x38;
        public byte _unknown0x39;
        public byte _unknown0x3A;
        public byte _unknown0x3B;
        public buint _unknown0x3C;
        public bshort _unknown0x40;
        public bshort _unknown0x42;
        public bfloat _unknown0x44;
        public TriggerData _unknown0x48;
        public buint _unknown0x4C;
        public buint _unknown0x50;
        public buint _unknown0x54;
    }
}