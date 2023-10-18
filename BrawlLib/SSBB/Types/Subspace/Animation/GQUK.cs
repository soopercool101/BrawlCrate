using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Animation
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GQUKEntry
    {
        public const int Size = 0x1C;

        public bfloat _unknown0x00;
        public bfloat _unknown0x04;
        public buint _unknown0x08;
        public bshort _unknown0x0C;
        public bshort _unknown0x0E;
        public TriggerData _unknown0x10;
        public bint _unknown0x14;
        public buint _unknown0x18;
    }
}