using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
   [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GDOR
    {
        public const uint Tag = 0x524F4447;
        public const int Size = 0x08;
        public uint _tag;
        public bint _count;

        public GDOR(int count)
        {
            _tag = Tag;
            _count = count;
        }

        public VoidPtr this[int index] { get { return (VoidPtr)((byte*)Address + Offsets(index)); } }
        public uint Offsets(int index) { return *(buint*)((byte*)Address + 0x08 + (index * 4)); }
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }
   [StructLayout(LayoutKind.Sequential, Pack = 1)]
   public unsafe struct GDOREntry
   {
       public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
   }
}
