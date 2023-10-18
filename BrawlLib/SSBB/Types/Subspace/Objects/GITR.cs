using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GITREntry
    {
        public const int Size = 0x38;

        public buint _unknown0x00;
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
        public buint _unknown0x30;
        public TriggerData _unknown0x34;
    }
}