using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GITMEntry
    {
        public const int Size = 0x18;

        public bfloat _positionX; // 0x00
        public bfloat _positionY; // 0x04
        public buint _unknown0x08;
        public TriggerData _unknown0x0C;
        public TriggerData _unknown0x10;
        public bool8 _unknown0x14;
        public byte _unknown0x15;
        public byte _unknown0x16;
        public byte _unknown0x17;
    }
}