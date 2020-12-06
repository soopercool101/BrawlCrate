using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Stage_Tables
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TB
    {
        public uint _tag;
        public bint _count;
        public bint _pad0x8;
        public bint _pad0xC;
        public const int Size = 0x10;

        public VoidPtr this[int index] => (byte*)Address + Offsets(index);

        public uint Offsets(int index)
        {
            return *(buint*)((byte*)Address + Size + index * 4);
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
}
