using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GELAEntry
    {
        public const int Size = 0x2C;

        public bfloat _unknown0x00;
        public bfloat _unknown0x04;
        public buint _unknown0x08;
        public bfloat _unknown0x0C;
        public byte _unknown0x10;
        public byte _unknown0x11;
        public byte _unknown0x12;
        public byte _unknown0x13;
        public bfloat _unknown0x14;
        public bint _unknown0x18;
        public byte _unknown0x1C;
        public byte _unknown0x1D;
        public byte _unknown0x1E;
        public byte _unknown0x1F;
        public bint _unknown0x20;
        public bool8 _unknown0x24;
        public byte _unknown0x25;
        public byte _unknown0x26;
        public byte _unknown0x27;
        public TriggerData _unknown0x28;
    }
}