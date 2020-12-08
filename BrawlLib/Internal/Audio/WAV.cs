using System;
using System.IO;
using System.Runtime.InteropServices;

namespace BrawlLib.Internal.Audio
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct RIFFHeader
    {
        public const uint RIFFTag = 0x46464952;
        public const uint WAVETag = 0x45564157;
        public const uint LISTTag = 0x5453494C;

        public uint _tag;
        public uint _length;
        public uint _waveTag;
        public fmtChunk _fmtChunk;
        public dataChunk _listChunk => *(dataChunk*) (Address + 12 + _fmtChunk.GetSize());

        public dataChunk _dataChunk
        {
            get => *(dataChunk*) (Address + 12 + _fmtChunk.GetSize() +
                                  (_listChunk._chunkTag == LISTTag ? 8 + _listChunk._chunkSize : 0));
            set => *(dataChunk*) (Address + 12 + _fmtChunk.GetSize()) = value;
        }

        public smplLoop[] _smplLoops
        {
            get
            {
                // Get first chunk in file
                byte* ptr = Address + 12;
                byte* end = Address + _length;
                while (ptr < end)
                {
                    // There is more data in the file
                    smplChunk* chunk = (smplChunk*) ptr;
                    if (chunk->_chunkTag == smplChunk.smplTag) // This is a real smpl chunk
                    {
                        return chunk->_smplLoops;
                    }

                    ptr += chunk->_chunkSize + 8;
                }

                return new smplLoop[0];
            }
        }

        public RIFFHeader(int format, int channels, int bitsPerSample, int sampleRate, int numSamples)
        {
            _tag = RIFFTag;
            _waveTag = WAVETag;
            _fmtChunk = new fmtChunk(format, channels, bitsPerSample, sampleRate);
            uint dataLen = (uint) (numSamples * _fmtChunk._blockAlign);
            _length = 0;
            _dataChunk = new dataChunk(dataLen);
            _length = dataLen + GetSize() - 8;
        }

        public uint GetSize()
        {
            return (uint) (12 + _fmtChunk.GetSize() + 8 +
                           (_listChunk._chunkTag == LISTTag ? 8 + _listChunk._chunkSize : 0));
        }

        internal byte* Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return (byte*) ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct fmtChunk
    {
        public const uint fmtTag = 0x20746D66;

        public uint _chunkTag;
        public uint _chunkSize;
        public ushort _format;
        public ushort _channels;
        public uint _samplesSec;
        public uint _avgBytesSec;
        public ushort _blockAlign;
        public ushort _bitsPerSample;
        public ushort _extraParamSize;

        //For some reason, if these aren't here, dataChunk bytes are 0
        public ushort _randomFiller1;
        public uint _randomFiller2;

        public int GetSize()
        {
            return (int) (8 + _chunkSize);
        }

        public fmtChunk(int format, int channels, int bitsPerSample, int sampleRate)
        {
            _chunkTag = fmtTag;
            _chunkSize = 0x10;
            _format = (ushort) format;
            _channels = (ushort) channels;
            _samplesSec = (uint) sampleRate;
            _blockAlign = (ushort) (bitsPerSample / 8 * channels);
            _avgBytesSec = (uint) (sampleRate * _blockAlign);
            _bitsPerSample = (ushort) bitsPerSample;
            _extraParamSize =
                _randomFiller1 =
                    0;
            _randomFiller2 = 0;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct dataChunk
    {
        public const uint dataTag = 0x61746164;
        public const int Size = 8;

        public uint _chunkTag;
        public uint _chunkSize;

        public dataChunk(uint dataLength)
        {
            _chunkTag = dataTag;
            _chunkSize = dataLength;
        }

        internal byte* Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return (byte*) ptr;
                }
            }
        }
    }

    // http://www.piclist.com/techref/io/serial/midi/wave.html

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct smplChunk
    {
        public const uint smplTag = 0x6c706d73;

        public uint _chunkTag;
        public uint _chunkSize;
        public uint _dwManufacturer;
        public uint _dwProduct;
        public uint _dwSamplePeriod;
        public uint _dwMIDIUnityNote;
        public uint _dwMIDIPitchFraction;
        public uint _dwSMPTEFormat;
        public uint _dwSMPTEOffset;
        public uint _cSampleLoops;
        public uint _cbSamplerData;

        internal byte* Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return (byte*) ptr;
                }
            }
        }

        public smplLoop[] _smplLoops
        {
            get
            {
                smplLoop[] arr = new smplLoop[_cSampleLoops];
                for (int i = 0; i < arr.Length; i++)
                {
                    byte* thisaddr = Address;
                    smplChunk* cc = (smplChunk*) Address;
                    byte* thataddr = Address + 0x2C + i * 0xC;
                    arr[i] = *(smplLoop*) thataddr;
                }

                return arr;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct smplLoop
    {
        public uint _dwIdentifier;
        public uint _dwType;
        public uint _dwStart;
        public uint _dwEnd;
        public uint _dwFraction;
        public uint _dwPlayCount;

        public override string ToString()
        {
            return "Loop from " + _dwStart + " to " + _dwEnd;
        }
    }

    public static unsafe class WAV
    {
        public static IAudioStream FromFile(string path)
        {
            return new PCMStream(File.ReadAllBytes(path));
        }

        public static void ToFile(IAudioStream source, string path, int samplePosition = 0,
                                  int maxSampleCount = int.MaxValue)
        {
            File.WriteAllBytes(path, ToByteArray(source, samplePosition, maxSampleCount));
        }

        public static byte[] ToByteArray(IAudioStream source, int samplePosition = 0, int maxSampleCount = int.MaxValue,
                                         bool appendSmplChunk = false)
        {
            int sampleCount = Math.Min(maxSampleCount, source.Samples - samplePosition);

            //Estimate size
            int outLen = 44 + sampleCount * source.Channels * 2;
            if (appendSmplChunk && source.IsLooping)
            {
                outLen += 68;
            }

            //Create byte array
            byte[] wavData = new byte[outLen];
            fixed (byte* address = wavData)
            {
                RIFFHeader* riff = (RIFFHeader*) address;
                *riff = new RIFFHeader(1, source.Channels, 16, source.Frequency, sampleCount);

                source.SamplePosition = samplePosition;
                source.ReadSamples(address + 44, sampleCount);

                if (appendSmplChunk && source.IsLooping)
                {
                    riff->_length += 68;
                    smplChunk* smpl = (smplChunk*) (address + outLen - 68);
                    *smpl = new smplChunk
                    {
                        _chunkTag = smplChunk.smplTag,
                        _chunkSize = 60,
                        _dwManufacturer = 0,
                        _dwProduct = 0,
                        _dwSamplePeriod = 0,
                        _dwMIDIUnityNote = 0,
                        _dwMIDIPitchFraction = 0,
                        _dwSMPTEFormat = 0,
                        _dwSMPTEOffset = 0,
                        _cSampleLoops = 1,
                        _cbSamplerData = 0
                    };
                    smplLoop* loop = (smplLoop*) (address + outLen - 24);
                    *loop = new smplLoop
                    {
                        _dwIdentifier = 0,
                        _dwType = 0,
                        _dwStart = (uint) source.LoopStartSample,
                        _dwEnd = (uint) source.LoopEndSample,
                        _dwFraction = 0,
                        _dwPlayCount = 0
                    };
                }
            }

            return wavData;
        }
    }
}