using BrawlLib.Internal;
using BrawlLib.Wii.Textures;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TEX0v1
    {
        public const int Size = 0x40;
        public const uint Tag = 0x30584554;

        public BRESCommonHeader _header;
        public bint _headerLen;
        public bint _stringOffset;
        public bint _hasPalette;
        public bshort _width;
        public bshort _height;
        public bint _pixelFormat;
        public bint _levelOfDetail;
        public bfloat _minLod;
        public bfloat _maxLod;
        public bint _origPathOffset;
        private fixed int padding[3];

        internal VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
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

        public VoidPtr PixelData => Address + _headerLen;
        public int PixelDataLength => _header._size - _headerLen;

        public WiiPixelFormat PixelFormat
        {
            get => (WiiPixelFormat) (int) _pixelFormat;
            set => _pixelFormat = (int) value;
        }

        public bool HasPalette
        {
            get => _hasPalette != 0;
            set => _hasPalette = value ? 1 : 0;
        }

        public TEX0v1(int width, int height, WiiPixelFormat format, int mipLevels) : this(width, height, format,
            mipLevels, Size)
        {
        }

        public TEX0v1(int width, int height, WiiPixelFormat format, int mipLevels, int headerLen)
        {
            _header._tag = Tag;
            _header._size = TextureConverter.Get(format).GetMipOffset(width, height, mipLevels + 1) + headerLen;
            _header._version = 1;
            _header._bresOffset = 0;

            _headerLen = headerLen;
            _stringOffset = 0;
            _hasPalette = format == WiiPixelFormat.CI4 || format == WiiPixelFormat.CI8 ? 1 : 0;
            _width = (short) width;
            _height = (short) height;
            _pixelFormat = (int) format;
            _levelOfDetail = mipLevels;
            _minLod = 0;
            _maxLod = mipLevels - 1.0f;
            _origPathOffset = 0;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TEX0v2
    {
        public const int Size = 0x40;
        public const uint Tag = 0x30584554;

        public BRESCommonHeader _header;
        public bint _hasPalette;
        public bint _headerLen;
        public bint _stringOffset;
        public bint _origPathOffset;
        public bshort _width;
        public bshort _height;
        public bint _pixelFormat;
        public bint _levelOfDetail;
        public bfloat _minLod;
        public bfloat _maxLod;
        private fixed int padding[3];

        internal VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
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

        public VoidPtr PixelData => Address + _headerLen;
        public int PixelDataLength => _header._size - _headerLen;

        public WiiPixelFormat PixelFormat
        {
            get => (WiiPixelFormat) (int) _pixelFormat;
            set => _pixelFormat = (int) value;
        }

        public bool HasPalette
        {
            get => _hasPalette != 0;
            set => _hasPalette = value ? 1 : 0;
        }

        public TEX0v2(int width, int height, WiiPixelFormat format, int mipLevels) : this(width, height, format,
            mipLevels, Size)
        {
        }

        public TEX0v2(int width, int height, WiiPixelFormat format, int mipLevels, int headerLen)
        {
            _header._tag = Tag;
            _header._size = TextureConverter.Get(format).GetMipOffset(width, height, mipLevels + 1) + headerLen;
            _header._version = 1;
            _header._bresOffset = 0;

            _headerLen = headerLen;
            _stringOffset = 0;
            _hasPalette = format == WiiPixelFormat.CI4 || format == WiiPixelFormat.CI8 ? 1 : 0;
            _width = (short) width;
            _height = (short) height;
            _pixelFormat = (int) format;
            _levelOfDetail = mipLevels;
            _minLod = 0;
            _maxLod = mipLevels - 1.0f;
            _origPathOffset = 0;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TEX0v3
    {
        public const int Size = 0x40;
        public const uint Tag = 0x30584554;

        public BRESCommonHeader _header;
        public bint _headerLen;
        public bint _stringOffset;
        public bint _hasPalette;
        public bshort _width;
        public bshort _height;
        public bint _pixelFormat;
        public bint _levelOfDetail;
        public bfloat _minLod;
        public bfloat _maxLod;
        public bint _origPathOffset;
        public bint _userDataOffset;
        private fixed int padding[2];

        //User Data comes before texture data. Align to 0x20

        internal VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public string OrigPath => new string((sbyte*) OrigPathAddress);

        public VoidPtr OrigPathAddress
        {
            get => Address + _origPathOffset;
            set => _origPathOffset = (int) value - (int) Address;
        }

        public UserData* UserData
        {
            get => _userDataOffset == 0 ? null : (UserData*) (Address + _userDataOffset);
            set => _userDataOffset = (int) (VoidPtr) value - (int) Address;
        }

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }

        public VoidPtr PixelData => Address + _headerLen;
        public int PixelDataLength => _header._size - _headerLen;

        public WiiPixelFormat PixelFormat
        {
            get => (WiiPixelFormat) (int) _pixelFormat;
            set => _pixelFormat = (int) value;
        }

        public bool HasPalette
        {
            get => _hasPalette != 0;
            set => _hasPalette = value ? 1 : 0;
        }

        public TEX0v3(int width, int height, WiiPixelFormat format, int mipLevels, int headerLen)
        {
            _header._tag = Tag;
            _header._size = TextureConverter.Get(format).GetMipOffset(width, height, mipLevels + 1) + headerLen;
            _header._version = 3;
            _header._bresOffset = 0;

            _headerLen = headerLen;
            _stringOffset = 0;
            _hasPalette = format == WiiPixelFormat.CI4 || format == WiiPixelFormat.CI8 ? 1 : 0;
            _width = (short) width;
            _height = (short) height;
            _pixelFormat = (int) format;
            _levelOfDetail = mipLevels;
            _minLod = 0;
            _maxLod = mipLevels - 1.0f;
            _origPathOffset = 0;
            _userDataOffset = 0;
        }
    }
}