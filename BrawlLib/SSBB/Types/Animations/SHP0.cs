using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Animations
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SHP0v3
    {
        public const string Tag = "SHP0";
        public const int Size = 0x28;

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _stringListOffset; //List of vertex node strings
        public bint _stringOffset;
        public bint _origPathOffset;
        public bushort _numFrames;
        public bushort _numEntries;
        public bint _loop;

        public SHP0v3(bool loop, ushort frames, ushort entries)
        {
            _header._tag = Tag;
            _header._size = Size;
            _header._version = 3;
            _header._bresOffset = 0;

            _dataOffset = 0x28;
            _origPathOffset = 0;
            _numFrames = frames;
            _loop = loop ? 1 : 0;
            _stringOffset = 0;
            _numEntries = entries;

            _stringListOffset = 0;
            _stringOffset = 0;
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

        public ResourceGroup* Group => (ResourceGroup*) (Address + _dataOffset);

        public string OrigPath => new string((sbyte*) OrigPathAddress);

        public VoidPtr OrigPathAddress
        {
            get => Address + _origPathOffset;
            set => _origPathOffset = (int) value - (int) Address;
        }

        public bint* StringEntries => (bint*) (Address + _stringListOffset);

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SHP0v4
    {
        public const string Tag = "SHP0";
        public const int Size = 0x2C;

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _stringListOffset; //List of vertex node strings
        public bint _userDataOffset;
        public bint _stringOffset;
        public bint _origPathOffset;
        public bushort _numFrames;
        public bushort _numEntries;
        public bint _loop;

        public SHP0v4(bool loop, ushort frames, ushort entries)
        {
            _header._tag = Tag;
            _header._size = Size;
            _header._version = 4;
            _header._bresOffset = 0;

            _dataOffset = Size;
            _origPathOffset = 0;
            _userDataOffset = 0;
            _numFrames = frames;
            _loop = loop ? 1 : 0;
            _stringOffset = 0;
            _numEntries = entries;

            _stringListOffset = 0;
            _stringOffset = 0;
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

        public ResourceGroup* Group => (ResourceGroup*) (Address + _dataOffset);

        public bint* StringEntries => (bint*) (Address + _stringListOffset);

        public VoidPtr UserData
        {
            get => _userDataOffset == 0 ? null : Address + _userDataOffset;
            set => _userDataOffset = (int) value - (int) Address;
        }

        public string OrigPath => new string((sbyte*) OrigPathAddress);

        public VoidPtr OrigPathAddress
        {
            get => Address + _origPathOffset;
            set => _origPathOffset = (int) value - (int) Address;
        }

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SHP0Entry
    {
        public const int Size = 0x14;

        public bint _flags;
        public bint _stringOffset;
        public bshort _nameIndex;
        public bshort _numIndices;
        public bint _fixedFlags; //Bit is set if entry is fixed
        public bint _indiciesOffset;

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

        //Aligned to 4 bytes
        public bshort* Indicies => (bshort*) (Address + _indiciesOffset);

        public bint* EntryOffset => (bint*) (Address + (_indiciesOffset - 4 * _numIndices));

        public SHP0KeyframeEntries* GetEntry(int index)
        {
            bint* ptr = &EntryOffset[index];
            return (SHP0KeyframeEntries*) ((VoidPtr) ptr + *ptr);
        }

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SHP0KeyframeEntries
    {
        public bshort _numEntries;
        public bshort _unk1;

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

        public BVec3* Entries => (BVec3*) (Address + 4);
    }
}