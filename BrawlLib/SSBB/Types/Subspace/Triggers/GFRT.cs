using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Triggers
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GFRTEntry
    {
        public const int Size = 0x14;

        public bfloat _unknown0x00;
        public bfloat _unknown0x04;
        public bfloat _unknown0x08;
        public TriggerData _unknown0x0C;
        public TriggerData _unknown0x10;
    }
}