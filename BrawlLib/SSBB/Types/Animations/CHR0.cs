using System;
using System.Runtime.InteropServices;
using BrawlLib.Wii.Animations;

namespace BrawlLib.SSBBTypes
{
    [StructLayout( LayoutKind.Sequential, Pack=1)]
    public unsafe struct CHR0v4_3
    {
        public const int Size = 0x28;
        public const string Tag = "CHR0";

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _stringOffset;
        public bint _origPathOffset;
        public bushort _numFrames;
        public bushort _numEntries;
        public bint _loop;
        public bint _scalingRule;
        
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + _dataOffset); } }

        public string OrigPath { get { return new String((sbyte*)OrigPathAddress); } }
        public VoidPtr OrigPathAddress
        {
            get { return Address + _origPathOffset; }
            set { _origPathOffset = (int)value - (int)Address; }
        }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }

        public CHR0v4_3(int version, int size, int frames, int entries, bool loop)
        {
            _header._tag = Tag;
            _header._size = size;
            _header._bresOffset = 0;

            _header._version = version;
            _dataOffset = Size;
            _stringOffset = 0;
            _origPathOffset = _scalingRule = 0;
            _numFrames = (ushort)frames;
            _numEntries = (ushort)entries;
            _loop = loop ? 1 : 0;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CHR0v5
    {
        public const int Size = 0x2C;
        public const string Tag = "CHR0";

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _userDataOffset;
        public bint _stringOffset;
        public bint _origPathOffset;
        public bushort _numFrames;
        public bushort _numEntries;
        public bint _loop;
        public bint _scalingRule;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + _dataOffset); } }

        public VoidPtr UserData
        {
            get { return _userDataOffset == 0 ? null : Address + _userDataOffset; }
            set { _userDataOffset = (int)value - (int)Address; }
        }

        public string OrigPath { get { return new String((sbyte*)OrigPathAddress); } }
        public VoidPtr OrigPathAddress
        {
            get { return Address + _origPathOffset; }
            set { _origPathOffset = (int)value - (int)Address; }
        }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }

        public CHR0v5(int version, int size, int frames, int entries, bool loop)
        {
            _header._tag = Tag;
            _header._size = size;
            _header._bresOffset = 0;

            _header._version = version;
            _dataOffset = Size;
            _stringOffset = 0;
            _userDataOffset = 0;
            _origPathOffset = _scalingRule = 0;
            _numFrames = (ushort)frames;
            _numEntries = (ushort)entries;
            _loop = loop ? 1 : 0;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CHR0Entry
    {
        public bint _stringOffset;
        public buint _code;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public VoidPtr Data { get { return Address + 8; } }

        public AnimationCode Code { get { return (AnimationCode)(uint)_code; } set { _code = (uint)value; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }
}