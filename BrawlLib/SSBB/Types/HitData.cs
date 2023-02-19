using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HitData
    {
        public const int Size = 0x20;

        public bfloat _startOffsetX; // 0x00
        public bfloat _startOffsetY; // 0x04
        public bfloat _startOffsetZ; // 0x08
        public bfloat _endOffsetX;   // 0x0C
        public bfloat _endOffsetY;   // 0x10
        public bfloat _endOffsetZ;   // 0x14
        public bfloat _size;         // 0x18
        public byte _nodeIndex;      // 0x1C
        public byte _unknown0x1D;
        public byte _unknown0x1E;
        public byte _unknown0x1F;
    }
}
