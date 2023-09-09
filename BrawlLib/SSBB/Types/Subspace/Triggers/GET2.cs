using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Triggers
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GET2Entry
    {
        public const int SIZE = 0x18;

        public byte _unknown0x00;
        public byte _unknown0x01;
        public byte _unknown0x02;
        public byte _unknown0x03;
        public bfloat _x1;
        public bfloat _y1;
        public bfloat _x2;
        public bfloat _y2;
        public TriggerData _trigger;
    }
}