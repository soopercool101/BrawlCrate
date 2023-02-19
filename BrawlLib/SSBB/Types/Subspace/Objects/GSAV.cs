using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GSAVEntry
    {
        public const int Size = 0x34;

        public buint _unknown0x00;
        public buint _unknown0x04;
        public buint _unknown0x08;
        public buint _unknown0x0C;
        public buint _unknown0x10;
        public buint _unknown0x14;
        public buint _unknown0x18;
        public bfloat _unknown0x1C;
        public bfloat _unknown0x20;
        public bfloat _unknown0x24;
        public bfloat _posX;         // 0x28
        public bfloat _posY;         // 0x2C
        public byte _modelDataIndex; // 0x30
        public byte _unknown0x31;
        public byte _unknown0x32;
        public byte _unknown0x33;
    }
}
