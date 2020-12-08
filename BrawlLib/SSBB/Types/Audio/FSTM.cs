using BrawlLib.Internal;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Audio
{
    /// <summary>
    /// A type/offset pair similar to ruint.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FSTMReference
    {
        public enum RefType : short
        {
            ByteTable = 0x0100,
            ReferenceTable = 0x0101,
            SampleData = 0x1F00,
            DSPADPCMInfo = 0x0300,
            InfoBlock = 0x4000,
            SeekBlock = 0x4001,
            DataBlock = 0x4002,
            StreamInfo = 0x4100,
            TrackInfo = 0x4101,
            ChannelInfo = 0x4102
        };

        public bshort _type;
        public bshort _padding;
        public bint _dataOffset;
    }

    /// <summary>
    /// A list of FSTMReferences, beginning with a length value. Similar to RuintList.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FSTMReferenceList
    {
        public bint _numEntries;

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

        public FSTMReference* Entries => (FSTMReference*) (Address + 4);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct FSTMHeader
    {
        public const uint Tag = 0x4D545346;

        public BinTag _tag;
        public bushort _endian;
        public bshort _headerSize;
        public bint _version;
        public bint _length;
        public bshort _numBlocks;
        public bshort _reserved;
        public FSTMReference _infoBlockRef;
        public bint _infoBlockSize;
        public FSTMReference _seekBlockRef;
        public bint _seekBlockSize;
        public FSTMReference _dataBlockRef;
        public bint _dataBlockSize;

        public Endian Endian
        {
            get => (Endian) (short) _endian;
            private set => _endian = (ushort) value;
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

        public void Set(int infoLen, int seekLen, int dataLen)
        {
            int len = 0x40;

            //Set header
            _tag = Tag;
            Endian = Endian.Big;
            _headerSize = (short) len;
            _version = 0x00030000;
            _numBlocks = 3;
            _reserved = 0;

            //Set offsets/lengths
            _infoBlockRef._type = (short) FSTMReference.RefType.InfoBlock;
            _infoBlockRef._dataOffset = len;
            _infoBlockSize = infoLen;
            _seekBlockRef._type = (short) FSTMReference.RefType.SeekBlock;
            _seekBlockRef._dataOffset = len += infoLen;
            _seekBlockSize = seekLen;
            _dataBlockRef._type = (short) FSTMReference.RefType.DataBlock;
            _dataBlockRef._dataOffset = len += seekLen;
            _dataBlockSize = dataLen;

            _length = len + dataLen;
        }

        public FSTMINFOHeader* INFOData => (FSTMINFOHeader*) (Address + _infoBlockRef._dataOffset);
        public FSTMSEEKHeader* SEEKData => (FSTMSEEKHeader*) (Address + _seekBlockRef._dataOffset);
        public FSTMDATAHeader* DATAData => (FSTMDATAHeader*) (Address + _dataBlockRef._dataOffset);
    }

    /// <summary>
    /// Represents a single TrackInfo segment (with the assumption that there will be only one in a file.) Some unknown data (the Byte Table) is hardcoded in the constructor.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FSTMTrackInfoStub
    {
        public byte _volume;
        public byte _pan;
        public bshort _padding;
        public FSTMReference _byteTableReference;
        public bint _byteTableCount;
        public bint _byteTable;

        public FSTMTrackInfoStub(byte volume, byte pan)
        {
            _volume = volume;
            _pan = pan;
            _padding = 0;
            _byteTableReference._type = (short) FSTMReference.RefType.ByteTable;
            _byteTableReference._padding = 0;
            _byteTableReference._dataOffset = 12;
            _byteTableCount = 2;
            _byteTable = 0x00010000;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct FSTMINFOHeader
    {
        public const uint Tag = 0x4F464E49;

        public uint _tag;
        public bint _size;
        public FSTMReference _streamInfoRef;
        public FSTMReference _trackInfoRefTableRef;
        public FSTMReference _channelInfoRefTableRef;
        public FSTMDataInfo _dataInfo;

        private VoidPtr DataInfoEnd
        {
            get
            {
                fixed (FSTMDataInfo* dataInfoPtr = &_dataInfo)
                {
                    byte* endptr = (byte*) (dataInfoPtr + 1);
                    return endptr;
                }
            }
        }

        // Below properties will find different parts of the INFO header, assuming that there are zero or one TrackInfo structures.

        public FSTMReferenceList* TrackInfoRefTable => (FSTMReferenceList*) DataInfoEnd;

        public FSTMReferenceList* ChannelInfoRefTable
        {
            get
            {
                if (_trackInfoRefTableRef._dataOffset == -1)
                {
                    fixed (FSTMReference* x = &_streamInfoRef)
                    {
                        // Look to see what the _channelInfoRefTableRef says - but if it's a file we're building, it may not have been filled in yet.
                        FSTMReferenceList* fromRef =
                            (FSTMReferenceList*) ((VoidPtr) x + _channelInfoRefTableRef._dataOffset);
                        // If it's not filled in, give a 0x0C gap to allow for the TrackInfoRefTable.
                        FSTMReferenceList* guess = (FSTMReferenceList*) (DataInfoEnd + 0x0C);
                        if (fromRef > guess)
                        {
                            Console.Error.WriteLine(
                                "There is extra data between the DataInfo and ChannelInfoRefTable that will be discarded.");
                            return fromRef;
                        }

                        return guess;
                    }
                }

                FSTMReferenceList* prevTable = TrackInfoRefTable;
                int* ptr = (int*) prevTable;
                int count = prevTable->_numEntries;
                if (count == 0)
                {
                    throw new Exception(
                        "Track info's ref table must be populated before channel info's ref table can be accessed.");
                }

                if (count == 16777216)
                {
                    count = 1;
                }

                if (count != 1)
                {
                    throw new Exception("BFSTM files with more than one track data section are not supported.");
                }

                ptr += 1 + count * 2;
                return (FSTMReferenceList*) ptr;
            }
        }

        private VoidPtr ChannelInfoRefTableEnd
        {
            get
            {
                FSTMReferenceList* prevTable = ChannelInfoRefTable;
                int count = prevTable->_numEntries;
                if (count == 0)
                {
                    throw new Exception(
                        "Channel info's ref table must be populated before track info can be accessed.");
                }

                int* ptr = (int*) prevTable;
                ptr += 1 + count * 2;
                return ptr;
            }
        }

        public FSTMTrackInfoStub* TrackInfo
        {
            get
            {
                if (_trackInfoRefTableRef._dataOffset == -1)
                {
                    return null;
                }

                return (FSTMTrackInfoStub*) ChannelInfoRefTableEnd;
            }
        }

        public FSTMReference* ChannelInfoEntries
        {
            get
            {
                if (_trackInfoRefTableRef._dataOffset == -1)
                {
                    return (FSTMReference*) ChannelInfoRefTableEnd;
                }

                int* ptr = (int*) (TrackInfo + 1);
                return (FSTMReference*) ptr;
            }
        }

        public FSTMADPCMInfo* GetChannelInfo(int index)
        {
            if (ChannelInfoEntries[index]._dataOffset == 0)
            {
                throw new Exception(
                    "Channel info entries must be populated with references to ADPCM data before ADPCM data can be accessed.");
            }

            return (FSTMADPCMInfo*) ((byte*) (ChannelInfoEntries + index) + ChannelInfoEntries[index]._dataOffset);
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

        public void Set(int size, int channels)
        {
            _tag = Tag;
            _size = size;

            _trackInfoRefTableRef._type = 0;
            _trackInfoRefTableRef._dataOffset = -1;

            TrackInfoRefTable->_numEntries = 16777216;
            ChannelInfoRefTable->_numEntries = channels;

            _streamInfoRef._type = (short) FSTMReference.RefType.StreamInfo;
            _streamInfoRef._dataOffset = 0x18;
            _channelInfoRefTableRef._type = (short) FSTMReference.RefType.ReferenceTable;
            _channelInfoRefTableRef._dataOffset = (byte*) ChannelInfoRefTable - (Address + 8);

            //Don't set track info
            TrackInfoRefTable->Entries[0]._type = 0;
            TrackInfoRefTable->Entries[0]._dataOffset = -1;

            //Set adpcm infos
            for (int i = 0; i < channels; i++)
            {
                ChannelInfoEntries[i]._dataOffset = 0x8 * channels + 0x26 * i;
                ChannelInfoEntries[i]._type = (short) FSTMReference.RefType.DSPADPCMInfo;

                //Set initial pointer
                ChannelInfoRefTable->Entries[i]._dataOffset = ChannelInfoEntries + i - (VoidPtr) ChannelInfoRefTable;
                ChannelInfoRefTable->Entries[i]._type = (short) FSTMReference.RefType.ChannelInfo;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct FSTMDataInfo
    {
        public AudioFormatInfo _format;
        public bint _sampleRate;
        public bint _loopStartSample;
        public bint _numSamples;

        public bint _numBlocks;
        public bint _blockSize;
        public bint _samplesPerBlock;
        public bint _lastBlockSize;

        public bint _lastBlockSamples;
        public bint _lastBlockTotal; //Includes padding
        public bint _bitsPerSample;
        public bint _dataInterval;

        public FSTMReference _sampleDataRef;

        public FSTMDataInfo(StrmDataInfo o, int dataOffset = 0x18)
        {
            _format = o._format;
            _sampleRate = (ushort) o._sampleRate;
            _loopStartSample = o._loopStartSample;
            _numSamples = o._numSamples;

            _numBlocks = o._numBlocks;
            _blockSize = o._blockSize;
            _samplesPerBlock = o._samplesPerBlock;
            _lastBlockSize = o._lastBlockSize;

            _lastBlockSamples = o._lastBlockSamples;
            _lastBlockTotal = o._lastBlockTotal;
            _bitsPerSample = o._bitsPerSample;
            _dataInterval = o._dataInterval;

            _sampleDataRef._type = (short) FSTMReference.RefType.SampleData;
            _sampleDataRef._padding = 0;
            _sampleDataRef._dataOffset = dataOffset;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct FSTMSEEKHeader
    {
        public const uint Tag = 0x4B454553;

        public uint _tag;
        public bint _length;
        private bint _pad1, _pad2;

        public void Set(int length)
        {
            _tag = Tag;
            _length = length;
            _pad1 = _pad2 = 0;
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

        public VoidPtr Data => Address + 0x10;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct FSTMDATAHeader
    {
        public const int Size = 0x20;
        public const uint Tag = 0x41544144;

        public uint _tag;
        public bint _length;
        public bint _dataOffset;
        public bint _pad1;

        public void Set(int length)
        {
            _tag = Tag;
            _length = length;
            _dataOffset = 0;
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

        public VoidPtr Data => Address + 0x20;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FSTMADPCMInfo
    {
        public const int Size = 0x2E;

        // Actually big endian, but you can't use fixed on structs here
        public fixed short _coefs[16];

        public bushort _gain;

        public bshort
            _ps; //Predictor and scale. This will be initialized to the predictor and scale value of the sample's first frame.

        public bshort _yn1; //History data; used to maintain decoder state during sample playback.
        public bshort _yn2; //History data; used to maintain decoder state during sample playback.

        public bshort
            _lps; //Predictor/scale for the loop point frame. If the sample does not loop, this value is zero.

        public bshort _lyn1; //History data for the loop point. If the sample does not loop, this value is zero.
        public bshort _lyn2; //History data for the loop point. If the sample does not loop, this value is zero.
        public bshort _pad;

        public FSTMADPCMInfo(ADPCMInfo o)
        {
            short[] c = o.Coefs;
            fixed (short* ptr = _coefs)
            {
                for (int i = 0; i < 16; i++)
                {
                    ptr[i] = c[i].Reverse();
                }
            }

            _gain = o._gain;
            _ps = o._ps;
            _yn1 = o._yn1;
            _yn2 = o._yn2;
            _lps = o._lps;
            _lyn1 = o._lyn1;
            _lyn2 = o._lyn2;
            _pad = o._pad;
        }

        public short[] Coefs
        {
            get
            {
                short[] arr = new short[16];
                fixed (short* ptr = _coefs)
                {
                    bshort* sPtr = (bshort*) ptr;
                    for (int i = 0; i < 16; i++)
                    {
                        arr[i] = sPtr[i];
                    }
                }

                return arr;
            }
        }
    }
}