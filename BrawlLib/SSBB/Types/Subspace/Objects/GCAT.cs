using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GCATEntry
    {
        public const int Size = 0x54;

        public bfloat _unknown0x00;
        public buint _unknown0x04;
        public buint _unknown0x08;
        public buint _unknown0x0C;
        public buint _unknown0x10;
        public buint _unknown0x14;
        public buint _unknown0x18;
        public buint _unknown0x1C;
        public bfloat _unknown0x20;
        public bfloat _unknown0x24;
        public bfloat _unknown0x28;
        public bfloat _unknown0x2C;
        public bfloat _unknown0x30;
        public bfloat _unknown0x34;
        public bfloat _unknown0x38;
        public bfloat _unknown0x3C;
        public bfloat _unknown0x40;
        public bfloat _unknown0x44;
        public bfloat _unknown0x48;
        public bfloat _unknown0x4C;
        public byte _modelDataIndex; // 0x50
        public byte _unknown0x51;
        public byte _unknown0x52;
        public byte _unknown0x53;
    }
}
