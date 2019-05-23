using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ItmFreqHeader
    {
        public const int Size = 0x20;

        public bint _Length;
        public bint _DataLength;
        public bint _OffCount;
        public bint _DataTable;
        public int _pad0;
        public int _pad1;
        public int _pad2;
        public int _pad3;


        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public string Str { get { return new string((sbyte*)Address + _DataLength + (_OffCount * 4) + 0x20 + (_DataTable*8)); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ItmFreqEntry
    {
        public const int Size = 0x10;

        public bint _ID;
        public bint _subItem;
        public bfloat _frequency;
        public bshort _action;
        public bshort _subaction;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ItmFreqTableList
    {
        public const int Size = 0x28;

        public ItmFreqOffEntry _table1;
        public ItmFreqOffEntry _table2;
        public ItmFreqOffEntry _table3;
        public ItmFreqOffEntry _table4;
        public ItmFreqOffEntry _table5;

        public ItmFreqOffEntry* Entries { get { return (ItmFreqOffEntry*)Address; } }
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ItmFreqGroup
    {
        public const int Size = 0x14;

        public bint _unknown0;
        public bint _unknown1;
        public bint _unknown2;
        public bint _entryOffset;
        public bint _entryCount;
        public bint _unknown3;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ItmFreqOffEntry
    {
        public const int Size = 0x08;

        public bint _offset;
        public bint _count;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ItmFreqOffPair
    {
        public const int Size = 0x08;

        public bint _offset1;
        public bint _offset2;

        public ItmFreqOffPair(bint off1, bint off2)
        {
            _offset1 = off1;
            _offset2 = off2;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }
}