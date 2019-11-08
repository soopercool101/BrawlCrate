using BrawlLib.Internal;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Animations
{
    [Flags]
    public enum PAT0Flags
    {
        Enabled = 0x1,
        FixedTexture = 0x2,
        HasTexture = 0x4,
        HasPalette = 0x8
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PAT0v3
    {
        public const string Tag = "PAT0";
        public const int Size = 0x38;

        public BRESCommonHeader _header;
        public bint _dataOffset;

        public bint _texTableOffset; //String list 1
        public bint _pltTableOffset; //String list 2
        public bint _texPtrTableOffset;
        public bint _pltPtrTableOffset;

        public bint _stringOffset;
        public bint _origPathOffset;

        public bushort _numFrames;
        public bushort _numEntries; //Same as group entries, directly below
        public bushort _numTexPtr;
        public bushort _numPltPtr;

        public bint _loop;

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

        public ResourceGroup* Group => (ResourceGroup*) (Address + _dataOffset);

        public string OrigPath => new string((sbyte*) OrigPathAddress);

        public VoidPtr OrigPathAddress
        {
            get => Address + _origPathOffset;
            set => _origPathOffset = (int) value - (int) Address;
        }

        public bint* TexFile => (bint*) (Address + _texTableOffset);
        public bint* PltFile => (bint*) (Address + _pltTableOffset);

        public string GetTexStringEntry(int index)
        {
            return new string((sbyte*) ((VoidPtr) TexFile + TexFile[index]));
        }

        public string GetPltStringEntry(int index)
        {
            return new string((sbyte*) ((VoidPtr) PltFile + PltFile[index]));
        }

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PAT0v4
    {
        public const string Tag = "PAT0";
        public const int Size = 0x3C;

        public BRESCommonHeader _header;
        public buint _dataOffset;

        public bint _texTableOffset; //String list 1
        public bint _pltTableOffset; //String list 2
        public bint _texPtrTableOffset;
        public bint _pltPtrTableOffset;

        public bint _userDataOffset;
        public bint _stringOffset;
        public bint _origPathOffset;

        public bushort _numFrames;
        public bushort _numEntries; //Same as group entries, directly below
        public bushort _numTexPtr;
        public bushort _numPltPtr;

        public bint _loop;

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

        public ResourceGroup* Group => (ResourceGroup*) (Address + _dataOffset);

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

        public bint* TexFile => (bint*) (Address + _texTableOffset);
        public bint* PltFile => (bint*) (Address + _pltTableOffset);


        public string GetTexStringEntry(int index)
        {
            return new string((sbyte*) ((VoidPtr) TexFile + TexFile[index]));
        }

        public string GetPltStringEntry(int index)
        {
            return new string((sbyte*) ((VoidPtr) PltFile + PltFile[index]));
        }

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PAT0Pattern
    {
        public const int Size = 0x8;

        public bint _stringOffset;
        public buint _flags;

        //Use this if there are multiple textures
        public PAT0TextureTable* GetTexTable(int index)
        {
            return (PAT0TextureTable*) (Address + GetTexTableOffset(index));
        }

        public int GetTexTableOffset(int index)
        {
            return *((bint*) Address + 2 + index);
        }

        public void SetTexTableOffset(int index, int value)
        {
            *((bint*) Address + 2 + index) = value;
        }

        public void SetTexTableOffset(int index, VoidPtr value)
        {
            *((bint*) Address + 2 + index) = (int) value - (int) Address;
        }

        //Use this only if texture is fixed
        public ushort GetIndex(int index, bool palette)
        {
            return *((bushort*) Address + 4 + index * 2 + (palette ? 1 : 0));
        }

        public void SetIndex(int index, ushort value, bool palette)
        {
            *((bushort*) Address + 4 + index * 2 + (palette ? 1 : 0)) = value;
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

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }

        public bint* Offsets => (bint*) Address + 2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PAT0TextureTable
    {
        public const int Size = 0x8;

        public bshort _textureCount;
        public bshort _pad;
        public bfloat _frameScale; // == 1 / last entry's key

        public VoidPtr Address
        {
            get
            {
                fixed (void* p = &this)
                {
                    return p;
                }
            }
        }

        public PAT0Texture* Textures => (PAT0Texture*) (Address + 0x8);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PAT0Texture
    {
        public const int Size = 0x8;

        public bfloat _key;
        public bushort _texFileIndex;
        public bushort _pltFileIndex;

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