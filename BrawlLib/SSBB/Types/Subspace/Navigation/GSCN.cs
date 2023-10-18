using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Navigation
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GSCNEntry
    {
        public const int Size = 0x1C;

        public bshort _unknown0x00;
        public bshort _unknown0x02;
        public bfloat _unknown0x04;
        public bint _unknown0x08;
        public bint _unknown0x0C;
        public buint _unknown0x10;
        public buint _unknown0x14;
        public TriggerData _unknown0x18;
    }
}