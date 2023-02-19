using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Triggers
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TriggerData
    {
        public const int Size = 4;

        public bushort _triggerId;  // 0x0
        public bool8 _isValid;      // 0x2
        public byte _unknown0x3;
    }
}
