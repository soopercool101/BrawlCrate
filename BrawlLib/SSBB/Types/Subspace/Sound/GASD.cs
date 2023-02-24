using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Sound
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GASDEntry
    {
        public const int Size = 0x34;

        public buint _unknown0x00;
        public bfloat _unknown0x04;
        public bfloat _unknown0x08;
        public bfloat _unknown0x0C;
        public bfloat _unknown0x10;
        public buint _unknown0x14;
        public bushort _unknown0x18;
        public bushort _unknown0x1A;
        public TriggerData _trigger1;   // 0x1C
        public TriggerData _trigger2;   // 0x20
        public buint _unknown0x24;
        public buint _unknown0x28;
        public buint _unknown0x2C;
        public buint _unknown0x30;
    }
}
