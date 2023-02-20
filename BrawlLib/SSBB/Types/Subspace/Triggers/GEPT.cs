using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Triggers
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GEPTEntry
    {
        public const int Size = 0x1C;

        public TriggerData _activateTrigger;    // 0x00
        public TriggerData _referenceTrigger1;  // 0x04
        public TriggerData _referenceTrigger2;  // 0x08
        public TriggerData _referenceTrigger3;  // 0x0C
        public TriggerData _referenceTrigger4;  // 0x10
        public TriggerData _referenceTrigger5;  // 0x14
        public bool8 _unknown0x18;
        public bool8 _unknown0x19;
        public byte _unknown0x1A;
        public byte _unknown0x1B;
    }
}
