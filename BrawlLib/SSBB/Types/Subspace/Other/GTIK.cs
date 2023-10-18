using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Other
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GTIKEntry
    {
        public const int Size = 0x30;

        public bfloat _unknown0x00;
        public bfloat _unknown0x04;
        public buint _unknown0x08;
        public buint _unknown0x0C;
        public bfloat _unknown0x10;
        public bshort _unknown0x14;
        public bshort _unknown0x16;
        public bfloat _unknown0x18;
        public byte _unknown0x1C;
        public byte _unknown0x1D;
        public byte _unknown0x1E;
        public byte _unknown0x1F;
        public byte _unknown0x20;
        public byte _unknown0x21;
        public byte _unknown0x22;
        public byte _unknown0x23;
        public buint _unknown0x24;
        public buint _unknown0x28;
        public TriggerData _unknown0x2C;
    }
}