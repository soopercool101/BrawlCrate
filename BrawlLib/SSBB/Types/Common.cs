using BrawlLib.Internal;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    public enum Endian
    {
        Big = 0xFEFF,
        Little = 0xFFFE
    }

    public unsafe struct BinTag
    {
        private readonly byte _1, _2, _3, _4;

        public BinTag(string tag)
        {
            _1 = _2 = _3 = _4 = 0;
            Set(tag);
        }

        public BinTag(uint tag, bool isLittleEndian)
        {
            _1 = _2 = _3 = _4 = 0;
            if (isLittleEndian)
            {
                *(uint*) Address = tag;
            }
            else
            {
                *(buint*) Address = tag;
            }
        }

        public uint Get(bool returnLittleEndian)
        {
            if (returnLittleEndian)
            {
                return *(uint*) Address;
            }

            return *(buint*) Address;
        }

        public string Get()
        {
            return new string(Address, 0, 4);
        }

        public void Set(string tag)
        {
            if (tag.Length > 4)
            {
                tag = tag.Substring(0, 4);
            }

            tag.Write(Address);
        }

        public static implicit operator BinTag(string r)
        {
            return new BinTag(r);
        }

        public static implicit operator string(BinTag r)
        {
            return r.Get();
        }

        public static implicit operator BinTag(uint r)
        {
            return new BinTag(r, true);
        }

        public static implicit operator uint(BinTag r)
        {
            return r.Get(true);
        }

        public sbyte* Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return (sbyte*) ptr;
                }
            }
        }

        public override string ToString()
        {
            return Get();
        }
    }

    public struct DataBlock
    {
        private VoidPtr _address;
        private uint _length;

        public VoidPtr Address
        {
            get => _address;
            set => _address = value;
        }

        public uint Length
        {
            get => _length;
            set => _length = value;
        }

        public VoidPtr EndAddress => _address + _length;

        public DataBlock(VoidPtr address, uint length)
        {
            _address = address;
            _length = length;
        }
    }

    public unsafe struct DataBlockCollection
    {
        private DataBlock _block;

        public DataBlockCollection(DataBlock block)
        {
            _block = block;
        }

        private buint* Data => (buint*) _block.EndAddress;

        public DataBlock this[int index] => new DataBlock(_block.Address + Data[index << 1], Data[(index << 1) + 1]);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NW4RCommonHeader
    {
        public const uint Size = 0x10;

        public BinTag _tag;
        public bushort _endian;
        public bushort _version;
        public bint _length;
        public bushort _firstOffset;
        public bushort _numEntries;

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

        public DataBlock DataBlock => new DataBlock(Address, Size);

        public byte VersionMajor
        {
            get => ((byte*) _version.Address)[0];
            set => ((byte*) _version.Address)[0] = value;
        }

        public byte VersionMinor
        {
            get => ((byte*) _version.Address)[1];
            set => ((byte*) _version.Address)[1] = value;
        }

        public Endian Endian
        {
            get => (Endian) (short) _endian;
            set => _endian = (ushort) value;
        }

        public DataBlockCollection Entries => new DataBlockCollection(DataBlock);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct NW4FCommonHeader
    {
        public const uint Size = 0x10;

        public BinTag _tag;
        public bushort _endian;
        public bushort _firstOffset;
        public bushort _numEntries;
        public bushort _version;
        public bint _length;

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

        public DataBlock DataBlock => new DataBlock(Address, Size);

        public byte VersionMajor
        {
            get => ((byte*) _version.Address)[0];
            set => ((byte*) _version.Address)[0] = value;
        }

        public byte VersionMinor
        {
            get => ((byte*) _version.Address)[1];
            set => ((byte*) _version.Address)[1] = value;
        }

        public Endian Endian
        {
            get => (Endian) (short) _endian;
            set => _endian = (ushort) value;
        }

        public DataBlockCollection Entries => new DataBlockCollection(DataBlock);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ruint
    {
        public enum RefType
        {
            Address = 0,
            Offset = 1
        }

        public byte _refType;

        public byte _dataType;
        //Specifies which struct to get, ex
        //DataRef<T, T1, T2, T3>
        //if dataType == 2, return T2 struct at address

        public bushort _reserved;
        public bint _dataOffset;

        public ruint(RefType refType, byte dataType, int data)
        {
            _refType = (byte) refType;
            _dataType = dataType;
            _reserved = 0;
            _dataOffset = data;
        }

        public VoidPtr Offset(VoidPtr baseAddr)
        {
            return baseAddr + _dataOffset;
        }

        public static implicit operator ruint(int r)
        {
            return new ruint {_refType = 1, _dataOffset = r};
        }

        public static implicit operator int(ruint r)
        {
            return r._dataOffset;
        }

        public static implicit operator ruint(uint r)
        {
            return new ruint {_refType = 1, _dataOffset = (int) r};
        }

        public static implicit operator uint(ruint r)
        {
            return (uint) r._dataOffset;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RuintList
    {
        //This address is the base of all ruint entry offsets

        public bint _numEntries;

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

        public ruint* Entries => (ruint*) (Address + 4);
        public VoidPtr Data => Address + _numEntries * 8 + 4;

        public VoidPtr this[int index]
        {
            get => (int) Entries[index];
            set => Entries[index] = (int) value;
        }

        public VoidPtr Get(VoidPtr offset, int index)
        {
            return offset + Entries[index];
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct RuintCollection
    {
        private ruint _first;

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

        public ruint* Entries => (ruint*) Address;

        public VoidPtr this[int index]
        {
            get => Address + Entries[index];
            set => Entries[index] = value - Address;
        }

        public VoidPtr Offset(VoidPtr offset)
        {
            return Address + offset;
        }

        public VoidPtr Get(int index)
        {
            return Address + Entries[index];
        }

        public void Set(int index, ruint.RefType refType, byte dataType, VoidPtr address)
        {
            *((ruint*) Address + index) = new ruint(refType, dataType, address - Address);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SSBBEntryHeader
    {
        public const uint Size = 8;

        public BinTag _tag;
        public bint _length;

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

        public DataBlock DataBlock => new DataBlock(Address, Size);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ResourceGroup : IEnumerable<ResourcePair>
    {
        public const int Size = 0x18;

        public bint _totalSize;
        public bint _numEntries;
        public ResourceEntry _first;

        public ResourceGroup(int numEntries)
        {
            _totalSize = numEntries * 0x10 + 0x18;
            _numEntries = numEntries;
            _first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);
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

        public ResourceEntry* First => (ResourceEntry*) (Address + 0x18);
        public VoidPtr EndAddress => Address + _totalSize;

        public IEnumerator<ResourcePair> GetEnumerator()
        {
            return new ResourceEnumerator((ResourceGroup*) Address);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new ResourceEnumerator((ResourceGroup*) Address);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ResourcePair
    {
        public PString Name;
        public VoidPtr Data;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ResourceEnumerator : IEnumerator<ResourcePair>
    {
        private readonly ResourceGroup* pGroup;
        private ResourceEntry* pEntry;
        private readonly int count;
        private int index;

        public ResourceEnumerator(ResourceGroup* group)
        {
            pGroup = group;
            count = pGroup->_numEntries;
            index = 0;
            pEntry = &pGroup->_first;
        }

        public ResourcePair Current => new ResourcePair
            {Name = (sbyte*) pGroup + pEntry->_stringOffset, Data = (byte*) pGroup + pEntry->_dataOffset};

        public void Dispose()
        {
        }

        object System.Collections.IEnumerator.Current => (VoidPtr) pEntry;

        public bool MoveNext()
        {
            if (index == count)
            {
                return false;
            }

            pEntry++;
            index++;
            return true;
        }

        public void Reset()
        {
            index = 0;
            pEntry = &pGroup->_first;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ResourceEntry
    {
        public const int Size = 0x10;

        public bushort _id;
        public bushort _flag;
        public bushort _leftIndex;
        public bushort _rightIndex;
        public bint _stringOffset;
        public bint _dataOffset;

        public int CharIndex
        {
            get => _id >> 3;
            set => _id = (ushort) ((value << 3) | (_id & 0x7));
        }

        public int CharShift
        {
            get => _id & 0x7;
            set => _id = (ushort) ((value & 0x7) | (_id & 0xFFF8));
        }

        public ResourceEntry(int id, int left, int right) : this(id, left, right, 0, 0)
        {
        }

        public ResourceEntry(int id, int left, int right, int dataOffset) : this(id, left, right, dataOffset, 0)
        {
        }

        public ResourceEntry(int id, int left, int right, int dataOffset, int stringOffset)
        {
            _id = (ushort) id;
            _flag = 0;
            _leftIndex = (ushort) left;
            _rightIndex = (ushort) right;
            _stringOffset = stringOffset;
            _dataOffset = dataOffset;
        }

        private ResourceEntry* Address
        {
            get
            {
                fixed (ResourceEntry* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public VoidPtr DataAddress
        {
            get => (VoidPtr) Parent + _dataOffset;
            set => _dataOffset = (int) value - (int) Parent;
        }

        public VoidPtr StringAddress
        {
            get => (VoidPtr) Parent + _stringOffset;
            set => _stringOffset = (int) value - (int) Parent;
        }

        public VoidPtr DataAddressRelative
        {
            get => _dataOffset.OffsetAddress;
            set => _dataOffset.OffsetAddress = value;
        }

        public VoidPtr StringAddressRelative
        {
            get => _stringOffset.OffsetAddress;
            set => _stringOffset.OffsetAddress = value;
        }

        public string GetName()
        {
            return new string((sbyte*) StringAddress);
        }

        public string GetNameRelative()
        {
            return new string((sbyte*) StringAddressRelative);
        }

        public static void Build(ResourceGroup* group, int index, VoidPtr dataAddress, BRESString* pString)
        {
            //Get the first entry in the group, which is empty
            ResourceEntry* list = &group->_first;
            //Get the entry that will be modified
            ResourceEntry* entry = &list[index];
            //Get the first entry again
            ResourceEntry* prev = &list[0];
            //Get the entry that the first entry's left index points to
            ResourceEntry* current = &list[prev->_leftIndex];
            //The index of the current entry
            ushort currentIndex = prev->_leftIndex;

            bool isRight = false;

            //Get the length of the string
            int strLen = pString->_length;
            if (strLen < 0 || strLen > ushort.MaxValue)
            {
                return;
            }

            //Create a byte pointer to the struct's string data
            byte* pChar = (byte*) pString + 4, sChar;

            int eIndex = strLen - 1, eBits = pChar[eIndex].CompareBits(0), val;
            *entry = new ResourceEntry((eIndex << 3) | eBits, index, index, (int) dataAddress - (int) group,
                (int) pChar - (int) group);

            //Continue while the previous id is greater than the current. Loop backs will stop the processing.
            //Continue while the entry id is less than or equal the current id. Being higher than the current id means we've found a place to insert.
            while (entry->_id <= current->_id && prev->_id > current->_id)
            {
                if (entry->_id == current->_id)
                {
                    sChar = (byte*) group + current->_stringOffset;

                    //Rebuild new id relative to current entry
                    for (eIndex = strLen; eIndex-- > 0 && pChar[eIndex] == sChar[eIndex];)
                    {
                        ;
                    }

                    eBits = pChar[eIndex].CompareBits(sChar[eIndex]);

                    entry->_id = (ushort) ((eIndex << 3) | eBits);

                    if (((sChar[eIndex] >> eBits) & 1) != 0)
                    {
                        entry->_leftIndex = (ushort) index;
                        entry->_rightIndex = currentIndex;
                    }
                    else
                    {
                        entry->_leftIndex = currentIndex;
                        entry->_rightIndex = (ushort) index;
                    }
                }

                //Is entry to the right or left of current?
                isRight = (val = current->_id >> 3) < strLen && ((pChar[val] >> (current->_id & 7)) & 1) != 0;

                prev = current;
                current = &list[currentIndex = isRight ? current->_rightIndex : current->_leftIndex];
            }

            sChar = current->_stringOffset == 0 ? null : (byte*) group + current->_stringOffset;
            val = sChar == null ? 0 : (int) *(bint*) (sChar - 4);

            if (val == strLen && ((sChar[eIndex] >> eBits) & 1) != 0)
            {
                entry->_rightIndex = currentIndex;
            }
            else
            {
                entry->_leftIndex = currentIndex;
            }

            if (isRight)
            {
                prev->_rightIndex = (ushort) index;
            }
            else
            {
                prev->_leftIndex = (ushort) index;
            }
        }

        public ResourceGroup* Parent
        {
            get
            {
                ResourceEntry* entry = Address;
                while (entry->_id != 0xFFFF)
                {
                    entry--;
                }

                return (ResourceGroup*) ((VoidPtr) entry - 8);
            }
        }
    }
}