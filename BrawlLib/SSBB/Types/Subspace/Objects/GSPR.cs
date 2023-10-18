using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GSPREntry
    {
        public const int Size = 0x50;

        public bfloat _unknown0x00;
        public byte _unknown0x04;
        public byte _unknown0x05;
        public byte _unknown0x06;
        public byte _unknown0x07;
        public buint _unknown0x08;
        public buint _unknown0x0C;
        public buint _unknown0x10;
        public buint _unknown0x14;
        public buint _unknown0x18;
        public buint _unknown0x1C;
        public buint _unknown0x20;
        public buint _unknown0x24;
        public bfloat _unknown0x28;
        public bfloat _unknown0x2C;
        public bfloat _unknown0x30;
        public bfloat _unknown0x34;
        public bfloat _unknown0x38;
        public bfloat _unknown0x3C;
        public buint _unknown0x40;
        public buint _unknown0x44;
        public byte _unknown0x48;
        public byte _unknown0x49;
        public byte _unknown0x4A;
        public byte _unknown0x4B;
        public TriggerData _unknown0x4C;
    }
}