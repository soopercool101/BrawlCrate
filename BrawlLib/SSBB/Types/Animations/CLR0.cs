using BrawlLib.Imaging;
using BrawlLib.Internal;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Animations
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CLR0v3
    {
        public const int Size = 0x24;
        public const string Tag = "CLR0";

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _stringOffset;
        public bint _origPathOffset;
        public bushort _frames;
        public bushort _entries;
        public bint _loop;

        public CLR0v3(int size, int frames, int entries, bool loop)
        {
            _header._tag = Tag;
            _header._size = size;
            _header._bresOffset = 0;
            _header._version = 3;

            _dataOffset = Size;
            _stringOffset = 0;
            _origPathOffset = 0;
            _frames = (ushort) frames;
            _entries = (ushort) entries;
            _loop = loop ? 1 : 0;
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

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CLR0v4
    {
        public const int Size = 0x28;
        public const string Tag = "CLR0";

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _userDataOffset;
        public bint _stringOffset;
        public bint _origPathOffset;
        public bushort _frames;
        public bushort _entries;
        public bint _loop;

        public CLR0v4(int size, int frames, int entries, bool loop)
        {
            _header._tag = Tag;
            _header._size = size;
            _header._bresOffset = 0;
            _header._version = 4;

            _userDataOffset = 0;
            _dataOffset = Size;
            _stringOffset = 0;
            _origPathOffset = 0;
            _frames = (ushort) frames;
            _entries = (ushort) entries;
            _loop = loop ? 1 : 0;
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

    [Flags]
    public enum CLR0EntryFlags : uint
    {
        Material0Exists = 0x1,
        Material0Constant = 0x2,
        Material1Exists = 0x4,
        Material1Constant = 0x8,
        Ambient0Exists = 0x10,
        Ambient0Constant = 0x20,
        Ambient1Exists = 0x40,
        Ambient1Constant = 0x80,
        TevReg0Exists = 0x100,
        TevReg0Constant = 0x200,
        TevReg1Exists = 0x400,
        TevReg1Constant = 0x800,
        TevReg2Exists = 0x1000,
        TevReg2Constant = 0x2000,
        TevKonst0Exists = 0x4000,
        TevKonst0Constant = 0x8000,
        TevKonst1Exists = 0x10000,
        TevKonst1Constant = 0x20000,
        TevKonst2Exists = 0x40000,
        TevKonst2Constant = 0x80000,
        TevKonst3Exists = 0x100000,
        TevKonst3Constant = 0x200000
    }

    public enum EntryTarget
    {
        LightChannel0MaterialColor, // GX_COLOR0A0
        LightChannel1MaterialColor, // GX_COLOR1A1
        LightChannel0AmbientColor,  // GX_COLOR0A0
        LightChannel1AmbientColor,  // GX_COLOR1A1
        ColorRegister0,             // GX_TEVREG0
        ColorRegister1,             // GX_TEVREG1
        ColorRegister2,             // GX_TEVREG2
        ConstantColorRegister0,     // GX_KCOLOR0
        ConstantColorRegister1,     // GX_KCOLOR1
        ConstantColorRegister2,     // GX_KCOLOR2
        ConstantColorRegister3      // GX_KCOLOR3
    }

    [Flags]
    public enum EntryFlag
    {
        Exists = 0x1,
        Constant = 0x2
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CLR0Material
    {
        public const int Size = 8;

        public bint _stringOffset;
        public buint _flags;

        public CLR0Material(CLR0EntryFlags flags, RGBAPixel mask, int offset)
        {
            _stringOffset = 0;
            _flags = (uint) flags;
        }

        public CLR0Material(CLR0EntryFlags flags, RGBAPixel mask, RGBAPixel color)
        {
            _stringOffset = 0;
            _flags = (uint) flags;
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

        public CLR0EntryFlags Flags
        {
            get => (CLR0EntryFlags) (uint) _flags;
            set => _flags = (uint) value;
        }

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CLR0MaterialEntry
    {
        public RGBAPixel _colorMask; //Used as a mask for source color before applying frames
        public bint _data;

        public RGBAPixel SolidColor
        {
            get => *(RGBAPixel*) (Address + 4);
            set => *(RGBAPixel*) (Address + 4) = value;
        }

        public RGBAPixel* Data => (RGBAPixel*) (Address + _data + 4);

        public CLR0MaterialEntry(RGBAPixel mask, RGBAPixel color)
        {
            _colorMask = mask;
            _data._data = *(int*) &color;
        }

        public CLR0MaterialEntry(RGBAPixel mask, int offset)
        {
            _colorMask = mask;
            _data = offset;
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