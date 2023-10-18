using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Hazards
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GWD2Entry
    {
        public const int Size = 0x4C;

        public buint _unknown0x00;
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
        public buint _unknown0x2C;
        public buint _unknown0x30;
        public bfloat _unknown0x34;
        public bfloat _unknown0x38;
        public bfloat _unknown0x3C;
        public bfloat _unknown0x40;
        public buint _unknown0x44;
        public buint _unknown0x48;
    }
}