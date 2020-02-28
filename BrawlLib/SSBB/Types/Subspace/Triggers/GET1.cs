using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Triggers
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GET1Entry
    {
        public const int SIZE = 0x18;

        public buint _trigger1;
        public bfloat _x1;
        public bfloat _y1;
        public bfloat _x2;
        public bfloat _y2;
        public buint _trigger2;
    }
}