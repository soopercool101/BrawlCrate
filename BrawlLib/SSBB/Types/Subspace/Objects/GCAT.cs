using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GCATEntry
    {
        public const int Size = 0x54;

        public MotionPathData _motionPathData;
        public buint _unknown0x08;
        public buint _unknown0x0C;
        public buint _unknown0x10;
        public buint _unknown0x14;
        public buint _unknown0x18;
        public buint _unknown0x1C;
        public bfloat _areaOffsetPosX;          // 0x20
        public bfloat _areaOffsetPosY;          // 0x24
        public bfloat _areaRangeX;              // 0x28
        public bfloat _areaRangeY;              // 0x2C
        public bfloat _framesBeforeStartMove;   // 0x30
        public bfloat _unknown0x34;
        public bfloat _unknown0x38;
        public bfloat _vector;                  // 0x3C
        public bfloat _unknown0x40;
        public bfloat _unknown0x44;
        public bfloat _unknown0x48;
        public bfloat _unknown0x4C;
        public byte _modelDataIndex;            // 0x50
        public byte _isFaceLeft;                // 0x51
        public byte _useNoHelperWarp;           // 0x52
        public byte _unknown0x53;
    }
}
