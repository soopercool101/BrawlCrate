using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Triggers
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GET1
    {
        public const uint TAG = 0x31544547; //GET1
        public const int SIZE = 0x8;

        public uint _tag;
        public bint _entryCount;

        public VoidPtr this[int index] => (byte*) Address + Offsets(index);

        public uint Offsets(int index)
        {
            return *(buint*) ((byte*) Address + 0x08 + index * 4);
        }

        private VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

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