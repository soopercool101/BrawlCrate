using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Triggers
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GFT2Entry
    {
        public const int Size = 0xC;

        public bfloat _unknown0x00;
        public TriggerData _trigger1; // 0x04
        public TriggerData _trigger2; // 0x08
    }
}
