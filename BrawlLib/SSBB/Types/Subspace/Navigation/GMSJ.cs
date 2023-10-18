using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Navigation
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GMSJEntry
    {
        public const int Size = 0xC;

        public byte _unknown0x00;
        public byte _unknown0x01;
        public byte _unknown0x02;
        public byte _unknown0x03;
        public bshort _unknown0x04;
        public bshort _unknown0x06;
        public TriggerData _unknown0x08;
    }
}