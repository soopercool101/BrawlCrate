using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Camera
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GCAMEntry
    {
        public const int Size = 0x24;

        public bfloat _unknown0x00;
        public byte _unknown0x04;
        public byte _unknown0x05;
        public byte _unknown0x06;
        public byte _unknown0x07;
        public bfloat _unknown0x08;
        public byte _unknown0x0C;
        public byte _unknown0x0D;
        public byte _unknown0x0E;
        public byte _unknown0x0F;
        public TriggerData _unknown0x10;
        public TriggerData _unknown0x14;
        public buint _unknown0x18;
        public buint _unknown0x1C;
        public buint _unknown0x20;
    }
}