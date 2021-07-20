using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Compression
{
    public enum CompressionType
    {
        None = 0x0,
        LZ77 = 0x1,
        ExtendedLZ77 = 0x101,
        Huffman = 0x2,
        RunLength = 0x3,
        RunLengthYAZ0 = 0x103,
        RunLengthYAY0 = 0x203,
        LZ77Huffman = 0x4,
        LZ77RangeCoder = 0x5,
        Differential = 0x8
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CompressionHeader
    {
        private Bin8 _algorithm;
        private UInt24 _size;
        private uint _extSize;

        public CompressionType Algorithm
        {
            get
            {
                CompressionType c = (CompressionType) _algorithm[4, 4];
                if (c == CompressionType.LZ77)
                {
                    if (IsExtendedLZ77)
                    {
                        c = CompressionType.ExtendedLZ77;
                    }
                }

                return c;
            }
            set
            {
                CompressionType c = value;
                if (c == CompressionType.ExtendedLZ77)
                {
                    IsExtendedLZ77 = true;
                    c = CompressionType.LZ77;
                }

                _algorithm[4, 4] = (byte) c;
            }
        }

        public uint Parameter
        {
            get => _algorithm[0, 4];
            set => _algorithm[0, 4] = (byte) value;
        }

        public bool IsExtendedLZ77
        {
            get => Parameter != 0;
            set => Parameter = (uint) (value ? 1 : 0);
        }

        public bool LargeSize => (uint) _size == 0;

        public uint ExpandedSize
        {
            get => LargeSize ? _extSize : (uint) _size;
            set
            {
                if (value > 0xFFFFFF) //Use extended header for sizes > 24 bits
                {
                    _extSize = value;
                    _size = 0;
                }
                else
                {
                    _size = value;
                }
            }
        }

        public bool HasLegitCompression()
        {
            return Enum.IsDefined(typeof(CompressionType), (int) _algorithm[4, 4]) && Algorithm != CompressionType.None;
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

        public VoidPtr Data => Address + 4 + (LargeSize ? 4 : 0);
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct YAZ0
    {
        public const int Size = 0x10;
        public const string Tag = "Yaz0";

        public BinTag _tag;
        public buint _unCompDataLen;
        public fixed int padding[2];

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

        public VoidPtr Data => Address + Size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct YAY0
    {
        public const int Size = 0x10;
        public const string Tag = "Yay0";

        public BinTag _tag;
        public buint _unCompDataLen;
        public buint _countOffset;
        public buint _dataOffset;

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

        public VoidPtr Data => Address + Size;
    }
}