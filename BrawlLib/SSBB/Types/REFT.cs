using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct REFT
    {
        //Header + string is aligned to 4 bytes

        public const uint Tag = 0x54464552;

        public NW4RCommonHeader _header;
        public uint _tag;        //Same as header
        public bint _dataLength; //Size of second REFT block. (file size - 0x18)
        public bint _dataOffset; //Offset from itself. Begins first entry
        public bint _linkPrev;   //0
        public bint _linkNext;   //0
        public bshort _stringLen;
        public bshort _padding; //0

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

        public string IdString
        {
            get => new string((sbyte*) Address + 0x28);
            set
            {
                int len = value.Length + 1;
                _stringLen = (short) len;

                byte* dPtr = (byte*) Address + 0x28;
                fixed (char* sPtr = value)
                {
                    for (int i = 0; i < len; i++)
                    {
                        *dPtr++ = (byte) sPtr[i];
                    }
                }

                //Align to 4 bytes
                while ((len++ & 3) != 0)
                {
                    *dPtr++ = 0;
                }
            }
        }

        public REFTypeObjectTable* Table => (REFTypeObjectTable*) (Address + 0x18 + _dataOffset);
    }

    public unsafe struct REFTypeObjectTable
    {
        //REFF table size is aligned to 4 bytes; REFT aligns to 0x20
        //All entry offsets are relative to this offset

        public bint _length;
        public bushort _entries;
        public bushort _pad;

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

        public REFTypeObjectEntry* First => (REFTypeObjectEntry*) (Address + 8);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct REFTypeObjectEntry
    {
        public bshort _strLen;

        public string Name
        {
            get => new string((sbyte*) Address + 2);
            set
            {
                _strLen = (short) (value.Length + 1);
                value.Write((sbyte*) Address + 2);
            }
        }

        public int DataOffset
        {
            get => (int) *(buint*) ((byte*) Address + 2 + _strLen);
            set => *(buint*) ((byte*) Address + 2 + _strLen) = (uint) value;
        }

        public int DataLength
        {
            get => (int) *(buint*) ((byte*) Address + 2 + _strLen + 4);
            set => *(buint*) ((byte*) Address + 2 + _strLen + 4) = (uint) value;
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

        public REFTypeObjectEntry* Next => (REFTypeObjectEntry*) (Address + 10 + _strLen);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct REFTImageHeader
    {
        public buint _unknown;
        public bushort _width;
        public bushort _height;
        public buint _imagelen;
        public byte _format;
        public byte _pltFormat;
        public bushort _colorCount;

        public buint _pltSize;
        public byte _mipmap;
        public byte _min_filt;
        public byte _mag_filt;
        public byte _reserved;
        public bfloat _lod_bias;

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

        public REFTImageHeader(ushort width, ushort height, byte format, byte pltFormat, ushort colors, uint imgSize,
                               byte lod)
        {
            _unknown = 0;
            _width = width;
            _height = height;
            _imagelen = imgSize;
            _format = format;
            _pltFormat = pltFormat;
            _colorCount = colors;
            _pltSize = (uint) colors * 2;
            _mipmap = lod;
            _min_filt = 0;
            _mag_filt = 0;
            _reserved = 0;
            _lod_bias = 0;
        }

        public void Set(byte min, byte mag, float lodBias)
        {
            _min_filt = min;
            _mag_filt = mag;
            _lod_bias = lodBias;
        }

        //From here starts the image.

        public VoidPtr PaletteData => Address + 0x20 + _imagelen;
    }
}