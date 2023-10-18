using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GIB2Entry
    {
        public const int Size = 0x54;

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
        public bfloat _unknown0x20;
        public bshort _unknown0x24;
        public bshort _unknown0x26;
        public buint _unknown0x28;
        public byte _modelDataIndex; // 0x2C
        public byte _collisionDataIndex; // 0x2D
        public byte _unknown0x2E;
        public byte _unknown0x2F;
        public bfloat _positionX; // 0x30
        public bfloat _positionY; // 0x34
        public buint _itemSpawnGroup; // 0x38
        public byte _unknown0x3C;
        public byte _unknown0x3D;
        public byte _unknown0x3E;
        public byte _unknown0x3F;
        public buint _unknown0x40;
        public TriggerData _unknown0x44;
        public TriggerData _unknown0x48;
        public TriggerData _unknown0x4C;
        public TriggerData _unknown0x50;
    }
}