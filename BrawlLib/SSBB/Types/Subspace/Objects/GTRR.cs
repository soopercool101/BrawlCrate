using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GTRREntry
    {
        public const int Size = 0x18;

        public SimpleAreaData _areaData;    // 0x00
        public TriggerData _triggerData;    // 0x14
    }
}