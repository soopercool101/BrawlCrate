using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Hazards
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GHPEEntry
    {
        public const int Size = 0x30;

        public buint _unknown0x00;
        public buint _unknown0x04;
        public buint _unknown0x08;
        public buint _unknown0x0C;
        public buint _unknown0x10;
        public buint _unknown0x14;
        public bfloat _unknown0x18;
        public bfloat _unknown0x1C;
        public bfloat _unknown0x20;
        public bfloat _unknown0x24;
        public byte _unknown0x28;
        public byte _unknown0x29;
        public byte _unknown0x2A;
        public byte _unknown0x2B;
        public bshort _unknown0x2C;
        public bshort _unknown0x2E;
    }
}