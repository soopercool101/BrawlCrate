using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using BrawlLib.IO;
using BrawlLib.SSBBTypes;

namespace System.Audio
{
    public unsafe class PCMStream : IAudioStream
    {
        private readonly short* _source;
        private IntPtr _allocatedHGlobal = IntPtr.Zero;

        private int _samplePos;
        private FileMap _sourceMap;

        public PCMStream(byte[] wavData)
        {
            _allocatedHGlobal = Marshal.AllocHGlobal(wavData.Length);
            Marshal.Copy(wavData, 0, _allocatedHGlobal, wavData.Length);

            var header = (RIFFHeader*) _allocatedHGlobal;
            BitsPerSample = header->_fmtChunk._bitsPerSample;
            Channels = header->_fmtChunk._channels;
            Frequency = (int) header->_fmtChunk._samplesSec;
            Samples = (int) (header->_dataChunk._chunkSize / header->_fmtChunk._blockAlign);

            _source = (short*) ((byte*) _allocatedHGlobal + header->GetSize());
            _samplePos = 0;

            IsLooping = false;
            LoopStartSample = 0;
            LoopEndSample = Samples;

            var loops = header->_smplLoops;
            if (loops.Length > 0)
            {
                IsLooping = true;
                LoopStartSample = (int) loops[0]._dwStart;
                LoopEndSample = (int) loops[0]._dwEnd;
            }
        }

        internal PCMStream(short* source, int samples, int sampleRate, int channels, int bps)
        {
            _sourceMap = null;

            BitsPerSample = bps; //16
            Channels = channels;
            Frequency = sampleRate;
            Samples = samples;

            _source = source;
            _samplePos = 0;
        }

        internal PCMStream(WaveInfo* pWAVE, VoidPtr dataAddr)
        {
            Frequency = pWAVE->_sampleRate;
            Samples = pWAVE->NumSamples;
            Channels = pWAVE->_format._channels;

            BitsPerSample = pWAVE->_format._encoding == 0 ? 8 : 16;

            if (Samples <= 0) return;

            LoopStartSample = pWAVE->LoopSample;
            LoopEndSample = Samples;

            _source = (short*) dataAddr;
            _samplePos = 0;
        }

        internal PCMStream(RSTMHeader* header, int channels, int startChannel, void* audioSource)
        {
            var info = header->HEADData->Part1;
            if (info->_format._channels < startChannel + channels) throw new Exception("Not enough channels");

            var blocksByChannel = new List<short[]>[info->_format._channels];
            for (var i = 0; i < blocksByChannel.Length; i++) blocksByChannel[i] = new List<short[]>();

            var from = (byte*) audioSource;

            for (var block = 0; block < info->_numBlocks; block++)
            {
                int blockSize = block == info->_numBlocks - 1
                    ? info->_lastBlockSize
                    : info->_blockSize;
                int blockTotal = block == info->_numBlocks - 1
                    ? info->_lastBlockTotal
                    : info->_blockSize;

                for (var channel = 0; channel < info->_format._channels; channel++)
                {
                    var b = new short[blockSize / sizeof(short)];
                    Marshal.Copy((IntPtr) from, b, 0, b.Length);
                    from += blockTotal;
                    blocksByChannel[channel].Add(b);
                }
            }

            var blocksToUseByChannel = new List<List<short[]>>();
            for (var i = 0; i < channels; i++) blocksToUseByChannel.Add(blocksByChannel[startChannel + i]);

            var size = info->_numSamples * channels * sizeof(short);
            //int size2 = blocksToUseByChannel.SelectMany(blocks => blocks.Select(block => block.Length)).Sum() * sizeof(short);
            //if (size != size2)
            //{
            //    throw new Exception($"{size} != {size2}");
            //}

            _allocatedHGlobal = Marshal.AllocHGlobal(size);
            var toPtr = (short*) _allocatedHGlobal;
            //short* endPtr = (short*)(_allocatedHGlobal + size);
            var blockCount = blocksToUseByChannel.Select(b => b.Count).Distinct().Single();
            for (var blockIndex = 0; blockIndex < blockCount; blockIndex++)
            {
                var blocks = blocksToUseByChannel.Select(l => l[blockIndex]).ToList();
                var blockShorts = blocks.Select(a => a.Length).Distinct().Single();
                for (var i = 0; i < blockShorts; i++)
                    foreach (var block in blocks)
                        *toPtr++ = block[i].Reverse();
            }
            //if (toPtr != endPtr) throw new Exception($"Not all data filled ({(long)_allocatedHGlobal}, {(long)toPtr}, {(long)endPtr})");

            BitsPerSample = 16;
            Channels = channels;
            Frequency = info->_sampleRate;
            Samples = info->_numSamples;

            _source = (short*) _allocatedHGlobal;
            _samplePos = 0;

            IsLooping = info->_format._looped != 0;
            LoopStartSample = info->_loopStartSample;
            LoopEndSample = Samples;
        }

        public WaveFormatTag Format => WaveFormatTag.WAVE_FORMAT_PCM;
        public int BitsPerSample { get; }

        public int Samples { get; }

        public int Channels { get; }

        public int Frequency { get; }

        public bool IsLooping { get; set; }

        public int LoopStartSample { get; set; }

        public int LoopEndSample { get; set; }

        public int SamplePosition
        {
            get => _samplePos;
            set => _samplePos = Math.Max(Math.Min(value, Samples), 0);
        }

        public int ReadSamples(VoidPtr destAddr, int numSamples)
        {
            var sPtr = _source + _samplePos * Channels;
            var dPtr = (short*) destAddr;

            var max = Math.Min(numSamples, Samples - _samplePos);

            for (var i = 0; i < max; i++)
            for (var x = 0; x < Channels; x++)
                *dPtr++ = *sPtr++;

            _samplePos += max;

            return max;
        }

        public void Wrap()
        {
            SamplePosition = LoopStartSample;
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

        internal static PCMStream[] GetStreams(RSTMHeader* pRSTM, VoidPtr dataAddr)
        {
            var pHeader = pRSTM->HEADData;
            var part1 = pHeader->Part1;
            int c = part1->_format._channels;
            var streams = new PCMStream[c.RoundUpToEven() / 2];

            for (var i = 0; i < streams.Length; i++)
            {
                var x = (i + 1) * 2 <= c ? 2 : 1;
                streams[i] = new PCMStream(pRSTM, x, i * 2, dataAddr);
            }

            return streams;
        }
    }
}