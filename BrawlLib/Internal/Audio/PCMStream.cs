using BrawlLib.Internal.IO;
using BrawlLib.SSBB.Types.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace BrawlLib.Internal.Audio
{
    public unsafe class PCMStream : IAudioStream
    {
        private IntPtr _allocatedHGlobal = IntPtr.Zero;
        private FileMap _sourceMap;

        private readonly short* _source;

        private readonly int _bps;
        private readonly int _numSamples;
        private readonly int _numChannels;
        private readonly int _frequency;
        private int _samplePos;

        private bool _looped;
        private int _loopStart;
        private int _loopEnd;

        public WaveFormatTag Format => WaveFormatTag.WAVE_FORMAT_PCM;
        public int BitsPerSample => _bps;
        public int Samples => _numSamples;
        public int Channels => _numChannels;
        public int Frequency => _frequency;

        public bool IsLooping
        {
            get => _looped;
            set => _looped = value;
        }

        public int LoopStartSample
        {
            get => _loopStart;
            set => _loopStart = value;
        }

        public int LoopEndSample
        {
            get => _loopEnd;
            set => _loopEnd = value;
        }

        public int SamplePosition
        {
            get => _samplePos;
            set => _samplePos = Math.Max(Math.Min(value, _numSamples), 0);
        }

        public PCMStream(byte[] wavData)
        {
            _allocatedHGlobal = Marshal.AllocHGlobal(wavData.Length);
            Marshal.Copy(wavData, 0, _allocatedHGlobal, wavData.Length);

            RIFFHeader* header = (RIFFHeader*) _allocatedHGlobal;
            _bps = header->_fmtChunk._bitsPerSample;
            _numChannels = header->_fmtChunk._channels;
            _frequency = (int) header->_fmtChunk._samplesSec;
            _numSamples = (int) (header->_dataChunk._chunkSize / header->_fmtChunk._blockAlign);

            _source = (short*) ((byte*) _allocatedHGlobal + header->GetSize());
            _samplePos = 0;

            _looped = false;
            _loopStart = 0;
            _loopEnd = _numSamples;

            smplLoop[] loops = header->_smplLoops;
            if (loops.Length > 0)
            {
                _looped = true;
                _loopStart = (int) loops[0]._dwStart;
                _loopEnd = (int) loops[0]._dwEnd;
            }
        }

        internal PCMStream(short* source, int samples, int sampleRate, int channels, int bps)
        {
            _sourceMap = null;

            _bps = bps; //16
            _numChannels = channels;
            _frequency = sampleRate;
            _numSamples = samples;

            _source = source;
            _samplePos = 0;
        }

        internal PCMStream(WaveInfo* pWAVE, VoidPtr dataAddr)
        {
            _frequency = pWAVE->_sampleRate;
            _numSamples = pWAVE->NumSamples;
            _numChannels = pWAVE->_format._channels;

            _bps = pWAVE->_format._encoding == 0 ? 8 : 16;

            if (_numSamples <= 0)
            {
                return;
            }

            _loopStart = pWAVE->LoopSample;
            _loopEnd = _numSamples;

            _source = (short*) dataAddr;
            _samplePos = 0;
        }

        internal static PCMStream[] GetStreams(RSTMHeader* pRSTM, VoidPtr dataAddr)
        {
            HEADHeader* pHeader = pRSTM->HEADData;
            StrmDataInfo* part1 = pHeader->Part1;
            int c = part1->_format._channels;
            PCMStream[] streams = new PCMStream[c.RoundUpToEven() / 2];

            for (int i = 0; i < streams.Length; i++)
            {
                int x = (i + 1) * 2 <= c ? 2 : 1;
                streams[i] = new PCMStream(pRSTM, x, i * 2, dataAddr);
            }

            return streams;
        }

        internal PCMStream(RSTMHeader* header, int channels, int startChannel, void* audioSource)
        {
            StrmDataInfo* info = header->HEADData->Part1;
            if (info->_format._channels < startChannel + channels)
            {
                throw new Exception("Not enough channels");
            }

            List<short[]>[] blocksByChannel = new List<short[]>[info->_format._channels];
            for (int i = 0; i < blocksByChannel.Length; i++)
            {
                blocksByChannel[i] = new List<short[]>();
            }

            byte* from = (byte*) audioSource;

            for (int block = 0; block < info->_numBlocks; block++)
            {
                int blockSize = block == info->_numBlocks - 1
                    ? info->_lastBlockSize
                    : info->_blockSize;
                int blockTotal = block == info->_numBlocks - 1
                    ? info->_lastBlockTotal
                    : info->_blockSize;

                for (int channel = 0; channel < info->_format._channels; channel++)
                {
                    short[] b = new short[blockSize / sizeof(short)];
                    Marshal.Copy((IntPtr) from, b, 0, b.Length);
                    from += blockTotal;
                    blocksByChannel[channel].Add(b);
                }
            }

            List<List<short[]>> blocksToUseByChannel = new List<List<short[]>>();
            for (int i = 0; i < channels; i++)
            {
                blocksToUseByChannel.Add(blocksByChannel[startChannel + i]);
            }

            int size = info->_numSamples * channels * sizeof(short);
            //int size2 = blocksToUseByChannel.SelectMany(blocks => blocks.Select(block => block.Length)).Sum() * sizeof(short);
            //if (size != size2)
            //{
            //    throw new Exception($"{size} != {size2}");
            //}

            _allocatedHGlobal = Marshal.AllocHGlobal(size);
            short* toPtr = (short*) _allocatedHGlobal;
            //short* endPtr = (short*)(_allocatedHGlobal + size);
            int blockCount = blocksToUseByChannel.Select(b => b.Count).Distinct().Single();
            for (int blockIndex = 0; blockIndex < blockCount; blockIndex++)
            {
                List<short[]> blocks = blocksToUseByChannel.Select(l => l[blockIndex]).ToList();
                int blockShorts = blocks.Select(a => a.Length).Distinct().Single();
                for (int i = 0; i < blockShorts; i++)
                {
                    foreach (short[] block in blocks)
                    {
                        *toPtr++ = block[i].Reverse();
                    }
                }
            }
            //if (toPtr != endPtr) throw new Exception($"Not all data filled ({(long)_allocatedHGlobal}, {(long)toPtr}, {(long)endPtr})");

            _bps = 16;
            _numChannels = channels;
            _frequency = info->_sampleRate;
            _numSamples = info->_numSamples;

            _source = (short*) _allocatedHGlobal;
            _samplePos = 0;

            _looped = info->_format._looped != 0;
            _loopStart = info->_loopStartSample;
            _loopEnd = _numSamples;
        }

        public int ReadSamples(VoidPtr destAddr, int numSamples)
        {
            short* sPtr = _source + _samplePos * _numChannels;
            short* dPtr = (short*) destAddr;

            int max = Math.Min(numSamples, _numSamples - _samplePos);

            for (int i = 0; i < max; i++)
            {
                for (int x = 0; x < _numChannels; x++)
                {
                    *dPtr++ = *sPtr++;
                }
            }

            _samplePos += max;

            return max;
        }

        public void Wrap()
        {
            SamplePosition = _loopStart;
        }

        public void Dispose()
        {
            if (_sourceMap != null)
            {
                _sourceMap.Dispose();
                _sourceMap = null;
            }

            if (_allocatedHGlobal != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_allocatedHGlobal);
                _allocatedHGlobal = IntPtr.Zero;
            }

            GC.SuppressFinalize(this);
        }
    }
}