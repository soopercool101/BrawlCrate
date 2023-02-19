using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Animation
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GLK2Entry
    {
        public static readonly int SIZE = 0x18;

        public bint _unknown0x00;
        public byte _modelDataID;       // 0x04
        public byte _unkflag2;          // 0x05
        public byte _unkflag3;          // 0x06
        public byte _unkflag4;          // 0x07
        public TriggerData _trigger1;   // 0x08
        public TriggerData _trigger2;   // 0x0C
        public TriggerData _trigger3;   // 0x10
        public TriggerData _trigger4;   // 0x14
    }
}