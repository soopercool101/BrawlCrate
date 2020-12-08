using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Audio
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct RWAV
    {
        public const string Tag = "RWAV";
        public const int Size = 0x20;

        public NW4RCommonHeader _header;

        public bint _infoOffset;
        public bint _infoLength;
        public bint _dataOffset;
        public bint _dataLength;

        public RWAVInfo* Info => (RWAVInfo*) (Address + _infoOffset);
        public RWAVData* Data => (RWAVData*) (Address + _dataOffset);

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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct RWAVInfo
    {
        public const string Tag = "INFO";

        public SSBBEntryHeader _header;
        public WaveInfo _info;

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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct RWAVData
    {
        public const string Tag = "DATA";

        public SSBBEntryHeader _header;

        //Audio Samples, align to 0x20

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

    internal enum WaveDataLocationType
    {
        Offset = 0,
        Address = 1
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct WaveInfo
    {
        public const int Size = 0x1C;

        public AudioFormatInfo _format;

        public bushort _sampleRate;
        public byte _dataLocationType; //WaveDataLocationType
        public byte _pad;
        public bint _loopStartSample;
        public bint _nibbles;                 //Includes ALL data, not just samples
        public buint _channelInfoTableOffset; //0x1C, offset to bint offset array for each channel
        public buint _dataLocation;
        public buint _reserved; //0

        public buint* OffsetTable => (buint*) (Address + _channelInfoTableOffset);

        public ChannelInfo* GetChannelInfo(int index)
        {
            return (ChannelInfo*) (Address + OffsetTable[index]);
        }

        public ADPCMInfo* GetADPCMInfo(int index)
        {
            return (ADPCMInfo*) (Address + GetChannelInfo(index)->_adpcmInfoOffset);
        }

        public int NumSamples
        {
            get => Get(_nibbles);
            set => _nibbles = Set(value);
        }

        public int LoopSample
        {
            get => Get(_loopStartSample);
            set => _loopStartSample = Set(value);
        }

        public int Get(int value)
        {
            return _format._encoding == 2 ? value / 16 * 14 + (value % 16 - 2) : value;
        }

        public int Set(int value)
        {
            return _format._encoding == 2 ? (8 * value + 16) / 7 : value;
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

        public int GetSize()
        {
            return Size + _format._channels * (0x20 + _format._encoding == 2 ? 0x30 : 0);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ChannelInfo
    {
        public const int Size = 0x1C;

        public bint _channelDataOffset; //offset to samples to read
        public bint _adpcmInfoOffset;   //offset to ADPCMInfo from start of WaveInfo struct
        public int _volFrontLeft;       //1
        public int _volFrontRight;      //1
        public int _volBackLeft;        //1
        public int _volBackRight;       //1
        public bint _reserved;          //0

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

    public enum WaveEncoding
    {
        PCM8 = 0,
        PCM16 = 1,
        ADPCM = 2
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AudioFormatInfo
    {
        public byte _encoding;
        public byte _looped;
        public byte _channels;
        public byte _sampleRate24;

        public AudioFormatInfo(byte encoding, byte looped, byte channels, byte unk)
        {
            _encoding = encoding;
            _looped = looped;
            _channels = channels;
            _sampleRate24 = unk;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ADPCMInfo
    {
        public const int Size = 0x30;

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
        public short _pad;

        public ADPCMInfo(CSTMADPCMInfo o, ushort gain = 0)
        {
            fixed (short* ptr = _coefs)
            {
                bshort* swap_ptr = (bshort*) ptr;
                for (int i = 0; i < 16; i++)
                {
                    swap_ptr[i] = o._coefs[i];
                }
            }

            _gain = gain;
            _ps = o._ps;
            _yn1 = o._yn1;
            _yn2 = o._yn2;
            _lps = o._lps;
            _lyn1 = o._lyn1;
            _lyn2 = o._lyn2;
            _pad = o._pad;
        }

        public ADPCMInfo(FSTMADPCMInfo o, ushort gain = 0)
        {
            fixed (short* ptr = _coefs)
            {
                for (int i = 0; i < 16; i++)
                {
                    ptr[i] = o._coefs[i];
                }
            }

            _gain = gain;
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