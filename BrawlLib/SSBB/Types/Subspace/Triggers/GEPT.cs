using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Triggers
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GEPTEntry
    {
        public const int Size = 0x1C;

        public TriggerData _trigger1; // 0x00
        public TriggerData _trigger2; // 0x04
        public TriggerData _trigger3; // 0x08
        public TriggerData _trigger4; // 0x0C
        public TriggerData _trigger5; // 0x10
        public TriggerData _trigger6; // 0x14
        public TriggerData _trigger7; // 0x18
    }
}
