using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Audio
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct RSTMHeader
    {
        public const uint Tag = 0x4D545352;

        public NW4RCommonHeader _header;
        public bint _headOffset;
        public bint _headLength;
        public bint _adpcOffset;
        public bint _adpcLength;
        public bint _dataOffset;
        public bint _dataLength;

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

        public void Set(int headLen, int adpcLen, int dataLen)
        {
            int len = 0x40;

            //Set header
            _header._tag = Tag;
            _header.Endian = Endian.Big;
            _header._version = 0x100;
            _header._firstOffset = 0x40;
            _header._numEntries = 2;

            //Set offsets/lengths
            _headOffset = len;
            _headLength = headLen;
            _adpcOffset = len += headLen;
            _adpcLength = adpcLen;
            _dataOffset = len += adpcLen;
            _dataLength = dataLen;

            if (adpcLen == 0)
            {
                _adpcOffset = 0;
            }

            _header._length = len + dataLen;

            //Fill padding
            Memory.Fill(Address + 0x28, 0x18, 0);
        }

        public HEADHeader* HEADData => (HEADHeader*) (Address + _headOffset);
        public ADPCHeader* ADPCData => (ADPCHeader*) (Address + _adpcOffset);
        public RSTMDATAHeader* DATAData => (RSTMDATAHeader*) (Address + _dataOffset);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct HEADHeader
    {
        public const uint Tag = 0x44414548;

        public uint _tag;
        public bint _size;
        public RuintCollection _entries;

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

        public void Set(int size, int channels, WaveEncoding encoding)
        {
            RuintList* list;
            VoidPtr offset = _entries.Address;
            int dataOffset = 0x60 + channels * 8;

            _tag = Tag;
            _size = size;

            //Set entry offsets
            _entries.Entries[0] = 0x18;
            _entries.Entries[1] = 0x4C;
            _entries.Entries[2] = 0x5C;

            //Audio info
            //HEADPart1* part1 = Part1;

            //Set single channel info
            list = Part2;
            list->_numEntries._data = 1; //Number is little-endian
            list->Entries[0] = 0x58;
            // TODO: This is not actually AudioFormatInfo. Set it as a buint instead.
            *(AudioFormatInfo*) list->Get(offset, 0) =
                channels == 1 ? new AudioFormatInfo(1, 0, 0, 0) : new AudioFormatInfo(2, 0, 1, 0);

            //Set adpcm infos
            list = Part3;
            list->_numEntries._data = channels; //little-endian
            for (int i = 0; i < channels; i++)
            {
                //Set initial pointer
                list->Entries[i] = dataOffset;

                if (encoding == WaveEncoding.ADPCM)
                {
                    //Set embedded pointer
                    *(ruint*) (offset + dataOffset) = dataOffset + 8;
                    dataOffset += 8;

                    //Set info
                    //*(ADPCMInfo*)(offset + dataOffset) = info[i];
                    dataOffset += ADPCMInfo.Size;

                    //Set padding
                    //*(short*)(offset + dataOffset) = 0;
                    //dataOffset += 2;
                }
                else
                {
                    //Set embedded pointer
                    *(ruint*) (offset + dataOffset) = 0;
                    dataOffset += 8;
                }
            }

            //Fill remaining
            int* p = (int*) (offset + dataOffset);
            for (dataOffset += 8; dataOffset < size; dataOffset += 4)
            {
                *p++ = 0;
            }
        }

        public StrmDataInfo* Part1 => (StrmDataInfo*) _entries[0]; //Audio info
        public RuintList* Part2 => (RuintList*) _entries[1];       //ADPC block flags?
        public RuintList* Part3 => (RuintList*) _entries[2];       //ADPCMInfo array, one for each channel?

        public ADPCMInfo* GetChannelInfo(int index)
        {
            return (ADPCMInfo*) ((ruint*) Part3->Get(_entries.Address, index))->Offset(_entries.Address);
        }

        public ADPCMInfo[] ChannelInfo
        {
            get
            {
                RuintList* list = Part3;
                VoidPtr offset = _entries.Address;
                int count = list->_numEntries._data;
                ADPCMInfo[] arr = new ADPCMInfo[count];
                for (int i = 0; i < count; i++)
                {
                    arr[i] = *(ADPCMInfo*) ((ruint*) list->Get(offset, i))->Offset(offset);
                }

                return arr;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct StrmDataInfo
    {
        public AudioFormatInfo _format;
        public bushort _sampleRate; //0x7D00
        public bushort _blockHeaderOffset;
        public bint _loopStartSample;
        public bint _numSamples;
        public bint _dataOffset;
        public bint _numBlocks;
        public bint _blockSize;
        public bint _samplesPerBlock; //0x3800
        public bint _lastBlockSize;   //Without padding
        public bint _lastBlockSamples;
        public bint _lastBlockTotal; //Includes padding
        public bint _dataInterval;   //0x3800
        public bint _bitsPerSample;

        public StrmDataInfo(CSTMDataInfo o, int dataOffset)
        {
            _format = o._format;
            _sampleRate = checked((ushort) o._sampleRate);
            _blockHeaderOffset = 0;
            _loopStartSample = o._loopStartSample;
            _numSamples = o._numSamples;
            _dataOffset = dataOffset;
            _numBlocks = o._numBlocks;
            _blockSize = o._blockSize;
            _samplesPerBlock = o._samplesPerBlock;
            _lastBlockSize = o._lastBlockSize;
            _lastBlockSamples = o._lastBlockSamples;
            _lastBlockTotal = o._lastBlockTotal;
            _dataInterval = o._dataInterval;
            _bitsPerSample = o._bitsPerSample;
        }

        public StrmDataInfo(FSTMDataInfo o, int dataOffset)
        {
            _format = o._format;
            _sampleRate = checked((ushort) (int) o._sampleRate);
            _blockHeaderOffset = 0;
            _loopStartSample = o._loopStartSample;
            _numSamples = o._numSamples;
            _dataOffset = dataOffset;
            _numBlocks = o._numBlocks;
            _blockSize = o._blockSize;
            _samplesPerBlock = o._samplesPerBlock;
            _lastBlockSize = o._lastBlockSize;
            _lastBlockSamples = o._lastBlockSamples;
            _lastBlockTotal = o._lastBlockTotal;
            _dataInterval = o._dataInterval;
            _bitsPerSample = o._bitsPerSample;
        }

        //public void Set(int sampleRate, int loopStart, int numSamples, int channels, int dataOffset)
        //{
        //    _format = new AudioFormatInfo(2, (byte)(loopStart >= 0 ? 1 : 0), (byte)channels, 0);
        //    _sampleRate = (ushort)_sampleRate;
        //    _unk1 = 0;
        //    _loopStartSample = loopStart;
        //    _numSamples = numSamples;
        //    _dataOffset = dataOffset;

        //    int tmp, lbSize;

        //    _numBlocks = (numSamples + 0x37FF) / 0x3800;
        //    if ((tmp = numSamples % 0x3800) != 0)
        //    {
        //        _lastBlockSamples = tmp;
        //        lbSize = tmp / 14 * 8;
        //        if ((tmp %= 14) != 0)
        //            lbSize += (tmp + 1) / 2 + 1;
        //        _lastBlockSize = lbSize;
        //        _lastBlockTotal = (lbSize + 0x19) / 0x20;
        //    }
        //    else
        //    {
        //        _lastBlockSamples = 0x3800;
        //        _lastBlockTotal = _lastBlockSize = 0x2000;
        //    }

        //    _blockSize = 0x2000;
        //    _samplesPerBlock = 0x3800;
        //    _unk8 = 0x3800;
        //    _bitsPerSample = 4;
        //}
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct ADPCHeader
    {
        public const uint Tag = 0x43504441;

        public uint _tag;
        public bint _length;

        public void Set(int length)
        {
            _tag = Tag;
            _length = length;
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

        public VoidPtr Data => Address + 8;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct RSTMDATAHeader
    {
        public const int Size = 0x20;
        public const uint Tag = 0x41544144;

        public uint _tag;
        public bint _length;
        public bint _dataOffset;
        public int _pad1;

        public void Set(int length)
        {
            _tag = Tag;
            _length = length;
            _dataOffset = 0x18;
            _pad1 = 0;
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

        public VoidPtr Data => Address + 8 + _dataOffset;
    }
}