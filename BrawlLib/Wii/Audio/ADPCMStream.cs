using BrawlLib.Internal;
using BrawlLib.Internal.Audio;
using BrawlLib.SSBB.Types.Audio;
using System;
using System.IO;

namespace BrawlLib.Wii.Audio
{
    internal unsafe class ADPCMStream : IAudioStream
    {
        private readonly int _sampleRate;
        private readonly int _numSamples;
        private readonly int _numChannels;
        private readonly int _blockLen;
        private int _samplesPerBlock;
        private int _lastBlockSamples, _lastBlockSize;
        private int _numBlocks;
        private readonly int _loopStartSample, _loopEndSample;
        private readonly bool _isLooped;
        private bool _useLoop;
        private int _samplePos;

        private readonly ADPCMState[,] _blockStates;
        private readonly ADPCMState[] _loopStates;
        internal ADPCMState[] _currentStates;

        public static ADPCMStream[] GetStreams(RSTMHeader* pRSTM, VoidPtr dataAddr)
        {
            HEADHeader* pHeader = pRSTM->HEADData;
            StrmDataInfo* part1 = pHeader->Part1;
            int c = part1->_format._channels;
            ADPCMStream[] streams = new ADPCMStream[c.RoundUpToEven() / 2];

            for (int i = 0; i < streams.Length; i++)
            {
                int x = (i + 1) * 2 <= c ? 2 : 1;
                streams[i] = new ADPCMStream(pRSTM, x, i * 2, dataAddr);
            }

            return streams;
        }

        public ADPCMStream(RSTMHeader* pRSTM, int channels, int startChannel, VoidPtr dataAddr)
        {
            HEADHeader* pHeader = pRSTM->HEADData;
            StrmDataInfo* part1 = pHeader->Part1;
            bshort* ynCache = (bshort*) pRSTM->ADPCData->Data;
            byte* sPtr;
            short[][] coefs;
            ADPCMInfo* info;
            int loopBlock, loopChunk;
            short yn1 = 0, yn2 = 0;

            _numChannels = part1->_format._channels;
            _isLooped = part1->_format._looped != 0;
            _sampleRate = part1->_sampleRate;
            _numSamples = part1->_numSamples;
            _numBlocks = part1->_numBlocks;
            _blockLen = part1->_blockSize;
            _loopStartSample = part1->_loopStartSample;
            _lastBlockSamples = part1->_lastBlockSamples;
            _lastBlockSize = part1->_lastBlockTotal;
            _samplesPerBlock = part1->_samplesPerBlock;
            _loopEndSample = _numSamples;

            _blockStates = new ADPCMState[_numChannels, _numBlocks];
            _currentStates = new ADPCMState[_numChannels];
            _loopStates = new ADPCMState[_numChannels];
            coefs = new short[_numChannels][];

            loopBlock = _loopStartSample / _samplesPerBlock;
            loopChunk = (_loopStartSample - loopBlock * _samplesPerBlock) / 14;
            sPtr = (byte*) dataAddr + loopBlock * _blockLen * _numChannels + loopChunk * 8;

            //Get channel info
            for (int i = 0; i < _numChannels; i++)
            {
                info = pHeader->GetChannelInfo(i);
                //Get channel coefs
                coefs[i] = info->Coefs;
                //Fill loop state
                _loopStates[i] = new ADPCMState(sPtr, info->_lps, info->_lyn1, info->_lyn2, coefs[i]);
                //Advance source pointer for next channel
                sPtr += _blockLen;
            }

            for (int cIndex = 0; cIndex < _numChannels; cIndex++)
            {
                yn1 = *ynCache++;
                yn2 = *ynCache++;
            }

            //Fill block states in a linear fashion
            sPtr = (byte*) dataAddr;
            for (int sIndex = 0, bIndex = 0; sIndex < _numSamples; sIndex += _samplesPerBlock, bIndex++)
            {
                for (int cIndex = 0; cIndex < _numChannels; cIndex++)
                {
                    if (bIndex > 0) //yn values will be zero if first block
                    {
                        yn1 = *ynCache++;
                        yn2 = *ynCache++;
                    }

                    //Get block state
                    _blockStates[cIndex, bIndex] =
                        new ADPCMState(sPtr, *sPtr, yn1, yn2, coefs[cIndex]); //Use ps from data stream
                    //Advance address
                    sPtr += bIndex == _numBlocks - 1 ? _lastBlockSize : _blockLen;
                }
            }

            _numChannels = channels;
            _startChannel = startChannel;
        }

        public ADPCMStream(RSTMHeader* pRSTM, VoidPtr dataAddr)
        {
            HEADHeader* pHeader = pRSTM->HEADData;
            StrmDataInfo* part1 = pHeader->Part1;
            bshort* ynCache = (bshort*) pRSTM->ADPCData->Data;
            byte* sPtr;
            short[][] coefs;
            ADPCMInfo* info;
            int loopBlock, loopChunk;
            short yn1 = 0, yn2 = 0;

            _numChannels = part1->_format._channels;
            _isLooped = part1->_format._looped != 0;
            _sampleRate = part1->_sampleRate;
            _numSamples = part1->_numSamples;
            _numBlocks = part1->_numBlocks;
            _blockLen = part1->_blockSize;
            _loopStartSample = part1->_loopStartSample;
            _lastBlockSamples = part1->_lastBlockSamples;
            _lastBlockSize = part1->_lastBlockTotal;
            _samplesPerBlock = part1->_samplesPerBlock;
            _loopEndSample = _numSamples;

            _blockStates = new ADPCMState[_numChannels, _numBlocks];
            _currentStates = new ADPCMState[_numChannels];
            _loopStates = new ADPCMState[_numChannels];
            coefs = new short[_numChannels][];

            loopBlock = _loopStartSample / _samplesPerBlock;
            loopChunk = (_loopStartSample - loopBlock * _samplesPerBlock) / 14;
            sPtr = (byte*) dataAddr + loopBlock * _blockLen * _numChannels + loopChunk * 8;

            //Get channel info
            for (int i = 0; i < _numChannels; i++)
            {
                info = pHeader->GetChannelInfo(i);
                //Get channel coefs
                coefs[i] = info->Coefs;
                //Fill loop state
                _loopStates[i] = new ADPCMState(sPtr, info->_lps, info->_lyn1, info->_lyn2, coefs[i]);
                //Advance source pointer for next channel
                sPtr += _blockLen;
            }

            //Fill block states in a linear fashion
            sPtr = (byte*) dataAddr;
            for (int sIndex = 0, bIndex = 0; sIndex < _numSamples; sIndex += _samplesPerBlock, bIndex++)
            {
                for (int cIndex = 0; cIndex < _numChannels; cIndex++)
                {
                    if (bIndex > 0) //yn values will be zero if first block
                    {
                        yn1 = *ynCache++;
                        yn2 = *ynCache++;
                    }

                    //Get block state
                    _blockStates[cIndex, bIndex] =
                        new ADPCMState(sPtr, *sPtr, yn1, yn2, coefs[cIndex]); //Use ps from data stream
                    //Advance address
                    sPtr += bIndex == _numBlocks - 1 ? _lastBlockSize : _blockLen;
                }
            }
        }

        public void Init()
        {
            if (_blockLen <= 0)
            {
                _samplesPerBlock = 0;
                _numBlocks = 0;
                _lastBlockSamples = 0;
            }
            else
            {
                _samplesPerBlock = _blockLen / 8 * 14;
                _numBlocks = _numSamples.Align(_samplesPerBlock) / _samplesPerBlock;

                if (_numSamples % _samplesPerBlock != 0)
                {
                    _lastBlockSamples = _numSamples % _samplesPerBlock;
                }
                else
                {
                    _lastBlockSamples = _samplesPerBlock;
                }
            }

            _lastBlockSize = _lastBlockSamples.Align(14) / 14 * 8;
        }

        public VoidPtr _dataAddress;

        public ADPCMStream(WaveInfo* pWAVE, VoidPtr dataAddr)
        {
            _dataAddress = dataAddr;

            ADPCMInfo*[] info;
            int loopBlock, loopChunk;
            byte* sPtr;

            info = new ADPCMInfo*[_numChannels = pWAVE->_format._channels];
            _currentStates = new ADPCMState[_numChannels];
            _loopStates = new ADPCMState[_numChannels];
            _isLooped = pWAVE->_format._looped != 0;
            _sampleRate = pWAVE->_sampleRate;
            _numSamples = pWAVE->NumSamples;

            if (_numSamples <= 0)
            {
                return;
            }

            _blockLen = (_numSamples.Align(14) / 14 * 8).Align(0x20);
            _loopStartSample = pWAVE->LoopSample;
            _loopEndSample = _numSamples;

            Init();

            _blockStates = new ADPCMState[_numChannels, _numBlocks];

            loopBlock = _loopStartSample / _samplesPerBlock;
            loopChunk = (_loopStartSample - loopBlock * _samplesPerBlock) / 14;

            int x = loopBlock * _blockLen * _numChannels + loopChunk * 8;
            int y = _loopStartSample / 14 * 8;
            sPtr = (byte*) dataAddr + x;

            //Get channel info
            for (int i = 0; i < _numChannels; i++)
            {
                //{
                //    //sPtr = (byte*)dataAddr + pWAVE->GetChannelInfo(i)->_channelDataOffset + loopStart;
                info[i] = pWAVE->GetADPCMInfo(i);
            }
            //    //Fill loop state
            //    _loopStates[i] = new ADPCMState(sPtr, info[i]->_lps, info[i]->_lyn1, info[i]->_lyn2, info[i]->Coefs);
            //    //Advance source pointer for next channel
            //    sPtr += _blockLen;
            //}

            //Fill block states in a linear fashion
            sPtr = (byte*) dataAddr;
            for (int sIndex = 0, bIndex = 0; sIndex < _numSamples; sIndex += _samplesPerBlock, bIndex++)
            {
                for (int cIndex = 0; cIndex < _numChannels; cIndex++)
                {
                    //sPtr = (byte*)dataAddr + pWAVE->GetChannelInfo(cIndex)->_channelDataOffset;
                    //Get block state
                    ADPCMInfo* i = info[cIndex];
                    _blockStates[cIndex, bIndex] = new ADPCMState(sPtr, i->_ps, i->_yn1, i->_yn2, i->_lps, i->_lyn1,
                        i->_lyn2, i->Coefs); //Use ps from data stream
                    //Advance address
                    sPtr += bIndex == _numBlocks - 1 ? _lastBlockSize : _blockLen;
                }
            }
        }

        private readonly int _startChannel;

        private void RefreshStates()
        {
            int blockId = _samplePos / _samplesPerBlock;
            int samplePos = blockId * _samplesPerBlock;
            for (int i = 0; i < _numChannels; i++)
            {
                _currentStates[i] = _blockStates[i + _startChannel, blockId];

                if (_useLoop)
                {
                    _currentStates[i].InitLoop();
                }
                else
                {
                    _currentStates[i].InitBlock();
                }

                for (int x = samplePos; x < _samplePos; x++)
                {
                    _currentStates[i].ReadSample();
                }
            }

            _useLoop = false;
        }

        //private unsafe void RefreshStates()
        //{
        //    int num = _samplePos / _samplesPerBlock;
        //    int num2 = num * _samplesPerBlock;
        //    for (int i = 0; i < _numChannels; i++)
        //    {
        //        ADPCMState state = _currentStates[i];
        //        byte* sPtr = (byte*)((_dataAddress + i * _blockLen + num * _blockLen * _numChannels) - ((num == (_numBlocks - 1)) ? (i * (_blockLen - _lastBlockSize)) : 0));
        //        if (_useLoop)
        //            state.InitLoop(sPtr);
        //        else if (num2 == 0)
        //            state.InitStart(sPtr);
        //        else
        //            state.InitBlock(num - 1, sPtr);
        //        for (int j = num2; j < _samplePos; j++)
        //            state.ReadSample();
        //    }
        //    _useLoop = false;
        //}

        public RIFFHeader GetPCMHeader()
        {
            return new RIFFHeader(1, _numChannels, 16, _sampleRate, _numSamples);
        }

        public void WriteStream(Stream outStream)
        {
            int oldPos = _samplePos;
            short sample;

            //SamplePosition = 0;
            for (_samplePos = 0; _samplePos < _numSamples; _samplePos++)
            {
                if (_samplePos % _samplesPerBlock == 0)
                {
                    RefreshStates();
                }

                foreach (ADPCMState state in _currentStates)
                {
                    sample = state.ReadSample();
                    outStream.WriteByte((byte) (sample & 0xFF));
                    outStream.WriteByte((byte) ((sample >> 8) & 0xFF));
                }
            }

            SamplePosition = oldPos;
        }

        #region IAudioStream Members

        public WaveFormatTag Format => WaveFormatTag.WAVE_FORMAT_PCM;
        public int BitsPerSample => 16;
        public int Samples => _numSamples;
        public int Channels => _numChannels;
        public int Frequency => _sampleRate;

        public bool IsLooping
        {
            get => _isLooped;
            set { }
        }

        public int LoopStartSample
        {
            get => _loopStartSample;
            set { }
        }

        public int LoopEndSample
        {
            get => _loopEndSample;
            set { }
        }

        public int SamplePosition
        {
            get => _samplePos;
            set
            {
                value = Math.Min(Math.Max(value, 0), _numSamples);
                if (_samplePos == value)
                {
                    return;
                }

                _samplePos = value;

                //Refresh states up to sample pos. If first in block, will be updated on next read.
                if (_samplePos % _samplesPerBlock != 0)
                {
                    RefreshStates();
                }
            }
        }

        public int ReadSamples(VoidPtr destAddr, int numSamples)
        {
            short* dPtr = (short*) destAddr;
            int samples = Math.Min(numSamples, _numSamples - _samplePos);

            for (int i = 0; i < samples; i++, _samplePos++)
            {
                if (_samplePos % _samplesPerBlock == 0)
                {
                    RefreshStates();
                }

                for (int x = 0; x < _numChannels; x++)
                {
                    *dPtr++ = _currentStates[x].ReadSample();
                }
            }

            return samples;
        }

        public void Wrap()
        {
            _useLoop = true;
            if (SamplePosition == _loopStartSample)
            {
                return;
            }

            SamplePosition = _loopStartSample;
        }

        public void Dispose()
        {
        }

        #endregion
    }
}