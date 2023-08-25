using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Camera
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GCMPEntry
    {
        public const int Size = 0x8;

        public byte _pathDataIndex;
        public byte _unknown0x01;
        public byte _unknown0x02;
        public byte _unknown0x03;
        public TriggerData _activationTrigger;
    }
}
