using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GFIMEntry
    {
        public const int Size = 0x18;

        public bfloat _unknown0x00;
        public bfloat _unknown0x04;
        public bfloat _unknown0x08;
        public buint _unknown0x0C;
        public TriggerData _unknown0x10;
        public bshort _unknown0x14;
        public bshort _unknown0x16;
    }
}