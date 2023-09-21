using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ItmFreqHeader
    {
        public const int Size = 0x20;

        public bint _Length;
        public bint _DataLength;
        public bint _OffCount;
        public bint _DataTable; // Should be "1"
        public int _pad0;
        public int _pad1;
        public int _pad2;
        public int _pad3;


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

        public string Str => (Address + _DataLength + _OffCount * 4 + Size + _DataTable * ItmFreqOffPair.Size).GetUTF8String();
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ItmFreqEntry
    {
        public const int Size = 0x10;

        public bint _ID;
        public bint _subItem;
        public bfloat _frequency;
        public bshort _min;
        public bshort _max;

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
    public unsafe struct ItmFreqTableList
    {
        public const int Size = 0x28;

        public ItmFreqOffEntry _table1;
        public ItmFreqOffEntry _table2;
        public ItmFreqOffEntry _table3;
        public ItmFreqOffEntry _table4;
        public ItmFreqOffEntry _table5;

        public ItmFreqOffEntry* Entries => (ItmFreqOffEntry*) Address;

        public VoidPtr this[int index] => (byte*)Address + index * ItmFreqOffEntry.Size;

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
    public unsafe struct ItmFreqGroup
    {
        public const int Size = 0x14;

        public bint _unknown0;      // 0x00
        public bint _unknown1;      // 0x04
        public bint _unknown2;      // 0x08
        public buint _entryOffset;  // 0x0C
        public buint _entryCount;   // 0x10

        public VoidPtr Address
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
    public unsafe struct ItmFreqOffEntry
    {
        public const int Size = 0x08;

        public buint _offset;
        public buint _count;

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
    public unsafe struct ItmFreqOffPair
    {
        public const int Size = 0x08;

        public buint _offset1;
        public buint _offset2;

        public ItmFreqOffPair(buint off1, buint off2)
        {
            _offset1 = off1;
            _offset2 = off2;
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