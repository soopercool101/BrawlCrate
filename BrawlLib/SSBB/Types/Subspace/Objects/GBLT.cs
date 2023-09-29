using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    public unsafe struct GBLTEntry
    {
        public const int Size = 0x40;

        public bint _unknown0x00;
        public bint _unknown0x04;
        public bint _unknown0x08;
        public bint _unknown0x0C;
        public bint _unknown0x10;
        public bint _unknown0x14;
        public bint _unknown0x18;
        public bint _unknown0x1C;
        public bfloat _areaWidth;
        public bfloat _areaHeight;
        public bfloat _areaCenterX;
        public bfloat _areaCenterY;
        public bfloat _unknown0x30;
        public bfloat _speed;
        public Directionality _directionality;
        public byte _unknown0x39;
        public byte _unknown0x3A;
        public byte _unknown0x3B;
        public TriggerData _trigger;    // 0x3C
    }

    public enum Directionality : byte
    {
        Left = 0,
        Right = 1,
    }
}