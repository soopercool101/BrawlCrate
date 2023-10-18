using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Other
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GDMDEntry
    {
        public const int Size = 0x64;

        public bfloat _unknown0x00;
        public buint _unknown0x04;
        public buint _unknown0x08;
        public buint _unknown0x0C;
        public buint _unknown0x10;
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
        public buint _unknown0x40;
        public buint _unknown0x44;
        public buint _unknown0x48;
        public buint _unknown0x4C;
        public buint _unknown0x50;
        public buint _unknown0x54;
        public buint _unknown0x58;
        public buint _unknown0x5C;
        public buint _unknown0x60;
    }
}