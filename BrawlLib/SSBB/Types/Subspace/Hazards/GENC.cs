using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Hazards
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GENCEntry
    {
        public const int Size = 0xAC;

        public buint _unknown0x00;
        public bfloat _unknown0x04;
        public buint _unknown0x08;
        public buint _unknown0x0C;
        public buint _unknown0x10;
        public buint _unknown0x14;
        public bfloat _unknown0x18;
        public bshort _unknown0x1C;
        public bshort _unknown0x1E;
        public buint _unknown0x20;
        public bfloat _unknown0x24;
        public byte _unknown0x28;
        public byte _unknown0x29;
        public byte _unknown0x2A;
        public byte _unknown0x2B;
        public bfloat _unknown0x2C;
        public bfloat _unknown0x30;
        public bfloat _unknown0x34;
        public bfloat _unknown0x38;
        public bfloat _unknown0x3C;
        public bfloat _unknown0x40;
        public bfloat _unknown0x44;
        public bfloat _unknown0x48;
        public bfloat _unknown0x4C;
        public bfloat _unknown0x50;
        public bfloat _unknown0x54;
        public bfloat _unknown0x58;
        public bfloat _unknown0x5C;
        public bfloat _unknown0x60;
        public bfloat _unknown0x64;
        public bfloat _unknown0x68;
        public bfloat _unknown0x6C;
        public buint _unknown0x70;
        public buint _unknown0x74;
        public bfloat _unknown0x78;
        public byte _unknown0x7C;
        public byte _unknown0x7D;
        public byte _unknown0x7E;
        public byte _unknown0x7F;
        public bint _unknown0x80;
        public bint _unknown0x84;
        public buint _unknown0x88;
        public byte _unknown0x8C;
        public byte _unknown0x8D;
        public byte _unknown0x8E;
        public byte _unknown0x8F;
        public buint _unknown0x90;
        public buint _unknown0x94;
        public buint _unknown0x98;
        public TriggerData _unknown0x9C;
        public TriggerData _unknown0xA0;
        public TriggerData _unknown0xA4;
        public TriggerData _unknown0xA8;
    }
}