using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    public enum MotionPathMode : byte
    {
        Return = 0x0,
        Loop = 0x1,
        Once = 0x2
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MotionPathData
    {
        public const int Size = 8;

        public bfloat _motionRatio;         // 0x0
        public byte _index;                 // 0x4
        public MotionPathMode _pathMode;    // 0x5
        public byte _modelIndex;            // 0x6
        public byte _unknown0x7;
    }
}