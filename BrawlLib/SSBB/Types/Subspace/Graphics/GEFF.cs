using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Graphics
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GEFFEntry
    {
        public const int Size = 0x28;

        public bfloat _unknown0x00;
        public bfloat _unknown0x04;
        public bfloat _unknown0x08;
        public bfloat _unknown0x0C;
        public bfloat _unknown0x10;
        public bfloat _unknown0x14;
        public bool8 _unknown0x18;
        public byte _unknown0x19;
        public byte _unknown0x1A;
        public byte _unknown0x1B;
        public TriggerData _unknown0x1C;
        public bshort _unknown0x20;
        public bshort _unknown0x22;
        public buint _unknown0x24;
    }
}