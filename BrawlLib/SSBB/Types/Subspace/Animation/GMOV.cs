using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GMOV
    {
        public const uint Tag = 0x564F4D47;
        public const int Size = 504;
        public uint _tag;
        public bint _count;
        public bint _DataOffset;

        public VoidPtr this[int index] { get { return (VoidPtr)((byte*)Address + Offsets(index)); } }
        public uint Offsets(int index) { return *(buint*)((byte*)Address + 0x08 + (index * 4)); }
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }
    public unsafe struct GMOVEntry
    {
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }
}