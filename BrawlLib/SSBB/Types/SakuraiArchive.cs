using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SakuraiArchiveHeader
    {
        public const int Size = 0x20;

        public bint _fileSize;
        public bint _lookupOffset;
        public bint _lookupEntryCount;
        public bint _sectionCount;            //Has string entry
        public bint _externalSubRoutineCount; //Has string entry
        public int _pad1, _pad2, _pad3;

        //From here begins file data. All offsets are relative to this location (0x20).
        public VoidPtr BaseAddress => Address + 0x20;

        public bint* LookupEntries => (bint*) (BaseAddress + _lookupOffset);

        public sStringEntry* Sections => (sStringEntry*) (BaseAddress + _lookupOffset + _lookupEntryCount * 4);

        public sStringEntry* ExternalSubRoutines =>
            (sStringEntry*) (BaseAddress + _lookupOffset + _lookupEntryCount * 4 + _sectionCount * 8);

        //For Sections and References
        public sStringTable* StringTable => (sStringTable*) (BaseAddress + _lookupOffset + _lookupEntryCount * 4 +
                                                             _sectionCount * 8 + _externalSubRoutineCount * 8);

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
    public unsafe struct sStringTable
    {
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

        public string GetString(int offset)
        {
            return new string((sbyte*) Address + offset);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct sStringEntry
    {
        public bint _dataOffset;
        public bint _stringOffset; //Base is string table
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct sListOffset
    {
        public const int Size = 8;

        public bint _startOffset;
        public bint _listCount;

        public sListOffset(int offset, int count)
        {
            _startOffset = offset;
            _listCount = count;
        }

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
}