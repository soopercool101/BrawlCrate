using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SimpleAreaData
    {
        public const int Size = 0x14;

        public bool8 _useTwoPoints; // 0x00
        public byte _unknown0x01;
        public byte _unknown0x02;
        public byte _unknown0x03;
        public bfloat _pos1X;       // 0x04
        public bfloat _pos1Y;       // 0x08
        public bfloat _pos2X;       // 0x0C
        public bfloat _pos2Y;       // 0x10
    }
}