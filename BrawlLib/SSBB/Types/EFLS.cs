using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct EFLSHeader
    {
        public const uint Tag = 0x534C4645;
        public const int Size = 0x10;

        public uint _tag;
        public bshort _numEntries;
        public bshort _numBrres;
        public bint _unk1; //0
        public bint _unk2; //0

        public EFLSHeader(int entries, int brresCount, int unk1, int unk2)
        {
            _tag = Tag;
            _numEntries = (short) entries;
            _numBrres = (short) brresCount;
            _unk1 = unk1;
            _unk2 = unk2;
        }

        private VoidPtr Address
        {
            get
            {
                fixed (void* p = &this)
                {
                    return p;
                }
            }
        }

        //First entry is always empty?
        public EFLSEntry* Entries => (EFLSEntry*) (Address + 0x10);

        public string GetString(int index)
        {
            EFLSEntry* entry = &Entries[index];
            if (entry->_stringOffset == 0)
            {
                return "<null>";
            }

            return new string((sbyte*) Address + entry->_stringOffset);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EFLSEntry
    {
        public const int Size = 0x10;

        public bshort _brresID1;   //-1 if no brres, otherwise brres ID
        public bshort _brresID2;   //brres ID
        public bint _stringOffset; //From file, 0 is empty string
        public bint _re3dOffset;   //From file
        public int _unk;

        public EFLSEntry(int id1, int id2, int stringOffset, int unk)
        {
            _brresID1 = (short) id1;
            _brresID2 = (short) id2;
            _stringOffset = stringOffset;
            _unk = unk;
            _re3dOffset = 0;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RE3D //Found inside EFLS files
    {
        public const uint Tag = 0x44334552;
        public const int Size = 0x10;

        public uint _tag;
        public byte _numEntries;
        public fixed byte pad[11];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RE3DEntry
    {
        public bint _stringOffset;
        public bint _unk1;
        public bshort _unk2;
        public bshort _unk3;
        public bint _effectNameOffset;

        internal byte* Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return (byte*) ptr;
                }
            }
        }
    }
}