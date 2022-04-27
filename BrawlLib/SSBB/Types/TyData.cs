using BrawlLib.Internal;
using System.Runtime.InteropServices;

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
    public struct TyEntry
    {
        public static readonly uint Size = 0x08;
        public buint _offset;
        public buint _strOffset;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TySeal
    {
        public static readonly int Size = 0x64;

        public bint _id;
        public buint _nameOffset;
        public buint _brresOffset;
        public bint _unknown0x0C;
        public bint _unknown0x10;
        public bshort _unknown0x14;
        public bshort _alphabeticalOrder; // 0x16
        public bshort _unknown0x18; // 0x18
        public bshort _unknown0x1A; // 0x1A
        public blong _characterFlags; //0x1C
        public bint _unknown0x24; // Ranges from 0-3, Enum?
        public buint _unknown0x28; // Always 0x00030000, offset?
        public bint _effectType; // 0x2C
        public bfloat _effectStrength; // 0x30
        public bfloat _unknown0x34; // 0 for item carry, 1 otherwise?
        public bfloat _unknown0x38; // Rarity?
        public bint _pad0x3C; // Always 0?
        public bint _pad0x40; // Always 0?
        public bint _pad0x44; // Always 0?
        public bint _pad0x48; // Always 0?
        public bfloat _unknown0x4C; // Always 1.0?
        public bshort _textureWidthSmall; // Something to do with size?
        public bshort _textureLengthSmall; // Something to do with size?
        public bshort _textureWidth; // Something to do with size?
        public bshort _textureLength; // Something to do with size?
        public bint _sizeOrder; // 0x58
        public bint _pad0x5C; // Always 0?
        public bint _pad0x60; // Always 0?
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TySealVertData
    {
        public static readonly uint HeaderSize = 0x04;
        public buint _entries;

        public VoidPtr this[int index] => (byte*)Address + HeaderSize + index * TySealVertDataEntry.Size;
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
    public struct TySealVertDataEntry
    {
        public static readonly uint Size = 0x08;

        public bint _unknown0x00;
        public bint _unknown0x04;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TySealVertListEntry
    {
        public static readonly uint Size = 0x04;

        public bfloat _value;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TyDataListEntry // Trophy Data
    {
        public static readonly uint Size = 0x60;

        public bint _id;
        public buint _nameOffset;
        public buint _brresOffset;
        public bint _gameIcon1;
        public bint _gameIcon2;
        public bint _nameIndex; // Name
        public bint _gameIndex; // Games 
        public bint _descriptionIndex; // Description
        public bint _series;
        public bint _category;
        public bint _pad0x28;
        public bint _pad0x2C;
        public bint _pad0x30;
        public bfloat _unknown0x34; // Always 1?
        public bfloat _unknown0x38; // Always 1?
        public bint _pad0x3C;
        public bint _unknown0x40; // Unlock criteria?
        public bint _unknown0x44; // Always 1?
        public bint _pad0x48;
        public bint _thumbnail; // Icon
        public bfloat _unknown0x50;
        public bfloat _unknown0x54;
        public bfloat _unknown0x58;
        public bfloat _unknown0x5C;
    }
}
