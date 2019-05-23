using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    //Alot of this was reused from STPM
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BLOC
    {
        public const uint Tag = 0x434F4C42;
        public const int Size = 0x10;

        public uint _tag;
        public bint _count;
        public bint _unk0;
        public int _pad1;

        public VoidPtr this[int index] { get { return (VoidPtr)((byte*)Address + Offsets(index)); } }
        public uint Offsets(int index) { return *(buint*)((byte*)Address + 0x10 + (index * 4)); }
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public BLOC(int count)
        {
            _tag = Tag;
            _count = count;
            _unk0 = 0x80;
            _pad1 = 0x00;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BLOCEntry
    {
        //public bushort _id;
        //public byte _id2;
        //public byte _echo;

        //public fixed int _values[64];

        //public BLOCEntry(ushort id, byte echo, byte id2)
        //{
        //    _id = id;
        //    _echo = echo;
        //    _id2 = id2;
        //}

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }
}