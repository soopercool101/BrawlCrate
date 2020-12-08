using BrawlLib.Internal;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Audio
{
    /// <summary>
    /// A type/offset pair similar to ruint.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CSTMReference
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

        public RefType _type;
        public short _padding;
        public int _dataOffset;
    }

    /// <summary>
    /// A list of CSTMReferences, beginning with a length value. Similar to RuintList.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CSTMReferenceList
    {
        public int _numEntries;

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

        public CSTMReference* Entries => (CSTMReference*) (Address + 4);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct CSTMHeader
    {
        public const uint Tag = 0x4D545343;

        public BinTag _tag;
        public bushort _endian;
        public short _headerSize;
        public int _version;
        public int _length;
        public short _numBlocks;
        public short _reserved;
        public CSTMReference _infoBlockRef;
        public int _infoBlockSize;
        public CSTMReference _seekBlockRef;
        public int _seekBlockSize;
        public CSTMReference _dataBlockRef;
        public int _dataBlockSize;

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
            Endian = Endian.Little;
            _headerSize = (short) len;
            _version = 0x02000000;
            _numBlocks = 3;
            _reserved = 0;

            //Set offsets/lengths
            _infoBlockRef._type = CSTMReference.RefType.InfoBlock;
            _infoBlockRef._dataOffset = len;
            _infoBlockSize = infoLen;
            _seekBlockRef._type = CSTMReference.RefType.SeekBlock;
            _seekBlockRef._dataOffset = len += infoLen;
            _seekBlockSize = seekLen;
            _dataBlockRef._type = CSTMReference.RefType.DataBlock;
            _dataBlockRef._dataOffset = len += seekLen;
            _dataBlockSize = dataLen;

            _length = len + dataLen;
        }

        public CSTMINFOHeader* INFOData => (CSTMINFOHeader*) (Address + _infoBlockRef._dataOffset);
        public CSTMSEEKHeader* SEEKData => (CSTMSEEKHeader*) (Address + _seekBlockRef._dataOffset);
        public CSTMDATAHeader* DATAData => (CSTMDATAHeader*) (Address + _dataBlockRef._dataOffset);
    }

    /// <summary>
    /// Represents a single TrackInfo segment (with the assumption that there will be only one in a file.) Some unknown data (the Byte Table) is hardcoded in the constructor.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CSTMTrackInfoStub
    {
        public byte _volume;
        public byte _pan;
        public short _padding;
        public CSTMReference _byteTableReference;
        public int _byteTableCount;
        public bint _byteTable;

        public CSTMTrackInfoStub(byte volume, byte pan)
        {
            _volume = volume;
            _pan = pan;
            _padding = 0;
            _byteTableReference._type = CSTMReference.RefType.ByteTable;
            _byteTableReference._padding = 0;
            _byteTableReference._dataOffset = 12;
            _byteTableCount = 2;
            _byteTable = 0x00010000;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct CSTMINFOHeader
    {
        public const uint Tag = 0x4F464E49;

        public uint _tag;
        public int _size;
        public CSTMReference _streamInfoRef;
        public CSTMReference _trackInfoRefTableRef;
        public CSTMReference _channelInfoRefTableRef;
        public CSTMDataInfo _dataInfo;

        private VoidPtr DataInfoEnd
        {
            get
            {
                fixed (CSTMDataInfo* dataInfoPtr = &_dataInfo)
                {
                    byte* endptr = (byte*) (dataInfoPtr + 1);
                    return endptr;
                }
            }
        }

        // Below properties will find different parts of the INFO header, assuming that there are zero or one TrackInfo structures.

        public CSTMReferenceList* TrackInfoRefTable => (CSTMReferenceList*) DataInfoEnd;

        public CSTMReferenceList* ChannelInfoRefTable
        {
            get
            {
                if (_trackInfoRefTableRef._dataOffset == -1)
                {
                    fixed (CSTMReference* x = &_streamInfoRef)
                    {
                        // Look to see what the _channelInfoRefTableRef says - but if it's a file we're building, it may not have been filled in yet.
                        CSTMReferenceList* fromRef =
                            (CSTMReferenceList*) ((VoidPtr) x + _channelInfoRefTableRef._dataOffset);
                        // If it's not filled in, give a 0x0C gap to allow for the TrackInfoRefTable.
                        CSTMReferenceList* guess = (CSTMReferenceList*) (DataInfoEnd + 0x0C);
                        if (fromRef > guess)
                        {
                            Console.Error.WriteLine(
                                "There is extra data between the DataInfo and ChannelInfoRefTable that will be discarded.");
                            return fromRef;
                        }

                        return guess;
                    }
                }

                CSTMReferenceList* prevTable = TrackInfoRefTable;
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
                    throw new Exception("BCSTM files with more than one track data section are not supported.");
                }

                ptr += 1 + count * 2;
                return (CSTMReferenceList*) ptr;
            }
        }

        private VoidPtr ChannelInfoRefTableEnd
        {
            get
            {
                CSTMReferenceList* prevTable = ChannelInfoRefTable;
                int count = prevTable->_numEntries;
                if (count == 0)
                {
                    throw new Exception(
                        "Channel info's ref table must be populated before track info can be accessed.");
                }

                if (count > 255 || count < 0)
                {
                    throw new Exception("This file seems to have an absurd number of channels");
                }

                int* ptr = (int*) prevTable;
                ptr += 1 + count * 2;
                return ptr;
            }
        }

        public CSTMTrackInfoStub* TrackInfo
        {
            get
            {
                if (_trackInfoRefTableRef._dataOffset == -1)
                {
                    return null;
                }

                return (CSTMTrackInfoStub*) ChannelInfoRefTableEnd;
            }
        }

        public CSTMReference* ChannelInfoEntries
        {
            get
            {
                if (_trackInfoRefTableRef._dataOffset == -1)
                {
                    return (CSTMReference*) ChannelInfoRefTableEnd;
                }

                int* ptr = (int*) (TrackInfo + 1);
                return (CSTMReference*) ptr;
            }
        }

        public CSTMADPCMInfo* GetChannelInfo(int index)
        {
            if (ChannelInfoEntries[index]._dataOffset == 0)
            {
                throw new Exception(
                    "Channel info entries must be populated with references to ADPCM data before ADPCM data can be accessed.");
            }

            return (CSTMADPCMInfo*) ((byte*) (ChannelInfoEntries + index) + ChannelInfoEntries[index]._dataOffset);
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

            TrackInfoRefTable->_numEntries = 1;
            ChannelInfoRefTable->_numEntries = channels;

            _streamInfoRef._type = CSTMReference.RefType.StreamInfo;
            _streamInfoRef._dataOffset = 0x18;
            _trackInfoRefTableRef._type = CSTMReference.RefType.ReferenceTable;
            _trackInfoRefTableRef._dataOffset = (byte*) TrackInfoRefTable - (Address + 8);
            _channelInfoRefTableRef._type = CSTMReference.RefType.ReferenceTable;
            _channelInfoRefTableRef._dataOffset = (byte*) ChannelInfoRefTable - (Address + 8);

            //Set single track info
            *TrackInfo = new CSTMTrackInfoStub(0x7F, 0x40);
            TrackInfoRefTable->Entries[0]._type = CSTMReference.RefType.TrackInfo;
            TrackInfoRefTable->Entries[0]._dataOffset = (VoidPtr) TrackInfo - TrackInfoRefTable;

            //Set adpcm infos
            for (int i = 0; i < channels; i++)
            {
                ChannelInfoEntries[i]._dataOffset = 0x8 * channels + 0x26 * i;
                ChannelInfoEntries[i]._type = CSTMReference.RefType.DSPADPCMInfo;

                //Set initial pointer
                ChannelInfoRefTable->Entries[i]._dataOffset = ChannelInfoEntries + i - (VoidPtr) ChannelInfoRefTable;
                ChannelInfoRefTable->Entries[i]._type = CSTMReference.RefType.ChannelInfo;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct CSTMDataInfo
    {
        public AudioFormatInfo _format;
        public int _sampleRate;
        public int _loopStartSample;
        public int _numSamples;

        public int _numBlocks;
        public int _blockSize;
        public int _samplesPerBlock;
        public int _lastBlockSize;

        public int _lastBlockSamples;
        public int _lastBlockTotal; //Includes padding
        public int _bitsPerSample;
        public int _dataInterval;

        public CSTMReference _sampleDataRef;

        public CSTMDataInfo(StrmDataInfo o, int dataOffset = 0x18)
        {
            _format = o._format;
            _sampleRate = o._sampleRate;
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

            _sampleDataRef._type = CSTMReference.RefType.SampleData;
            _sampleDataRef._padding = 0;
            _sampleDataRef._dataOffset = dataOffset;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct CSTMSEEKHeader
    {
        public const uint Tag = 0x4B454553;

        public uint _tag;
        public int _length;
        private int _pad1, _pad2;

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
    internal unsafe struct CSTMDATAHeader
    {
        public const int Size = 0x20;
        public const uint Tag = 0x41544144;

        public uint _tag;
        public int _length;
        public int _dataOffset;
        public int _pad1;

        public void Set(int length)
        {
            _tag = Tag;
            _length = length;
            // _dataOffset = 0x18 matches froggestspirit's BRSTM2BCSTM converter.
            // To match the behavior of soneek's PHP converter, set this to 0.
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

        public VoidPtr Data => Address + 0x20;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CSTMADPCMInfo
    {
        public const int Size = 0x2E;

        public fixed short _coefs[16];

        public ushort _gain;

        public short
            _ps; //Predictor and scale. This will be initialized to the predictor and scale value of the sample's first frame.

        public short _yn1;  //History data; used to maintain decoder state during sample playback.
        public short _yn2;  //History data; used to maintain decoder state during sample playback.
        public short _lps;  //Predictor/scale for the loop point frame. If the sample does not loop, this value is zero.
        public short _lyn1; //History data for the loop point. If the sample does not loop, this value is zero.
        public short _lyn2; //History data for the loop point. If the sample does not loop, this value is zero.
        public short _pad;

        public CSTMADPCMInfo(ADPCMInfo o)
        {
            short[] c = o.Coefs;
            fixed (short* ptr = _coefs)
            {
                for (int i = 0; i < 16; i++)
                {
                    ptr[i] = c[i];
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
    }
}