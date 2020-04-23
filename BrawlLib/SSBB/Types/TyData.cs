using BrawlLib.Internal;
using BrawlLib.SSBB.Types.ProjectPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TyDataHeader
    {
        public static readonly uint HeaderSize = 0x20;
        public buint _size;
        public buint _dataOffset;
        public buint _dataEntries;
        public buint _entries;
        public buint _pad1;
        public buint _pad2;
        public buint _pad3;
        public buint _pad4;


        public VoidPtr GetDataEntryOffset(int index)
        {
            if (index > _dataEntries)
            {
                return null;
            }

            return Address + HeaderSize + _dataOffset + index * 4;
        }

        public VoidPtr GetEntry(int index)
        {
            return Address + HeaderSize + this[index].UInt;
        }

        public string GetEntryName(int index)
        {
            return (Address + HeaderSize + _dataOffset + _dataEntries * 4 + _entries * TyEntry.Size +
                   ((TyEntry*) this[index])->_strOffset).GetUTF8String();
        }

        public VoidPtr this[int index] => (byte*)Address + HeaderSize + _dataOffset + _dataEntries * 4 + index * TyEntry.Size;
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
    public unsafe struct TyEntry
    {
        public static readonly uint Size = 0x08;
        public buint _unknown;
        public buint _strOffset;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TySeal
    {
        public bint _id;
        public bint _nameOffset;
        public bint _brresOffset;
        public bint _unknown0x0C;
        public bint _unknown0x10;
        public bint _unknown0x14;
        public bshort _order1; // 0x1C
        public bshort _order2; // 0x1E
        public bshort _unknown0x20; // 0x20
        public blong _characterFlags; //0x22
    }
}
