using BrawlLib.Internal;
using BrawlLib.Internal.Audio;
using BrawlLib.Internal.IO;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Audio;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Audio
{
    public static class RWAVConverter
    {
        public static unsafe FileMap Encode(IAudioStream stream, IProgressTracker progress)
        {
            int tmp;
            bool looped = stream.IsLooping;
            int channels = stream.Channels;
            int samples;
            int blocks;
            int sampleRate = stream.Frequency;
            int lbSamples, lbSize, lbTotal;
            int /*loopPadding, */
                loopStart, totalSamples;
            short* tPtr;

            int blockLen, samplesPerBlock;

            if (looped)
            {
                loopStart = stream.LoopStartSample;
                samples = stream
                    .LoopEndSample; //Set sample size to end sample. That way the audio gets cut off when encoding.

                blockLen = samples.Align(14) / 14 * 8;
                samplesPerBlock = blockLen / 8 * 14;

                //If loop point doesn't land on a block, pad the stream so that it does.
                //if ((tmp = loopStart % samplesPerBlock) != 0)
                //{
                //    loopPadding = samplesPerBlock - tmp;
                //    loopStart += loopPadding;
                //}
                //else
                //    loopPadding = 0;

                totalSamples = /*loopPadding + */samples;
            }
            else
            {
                //loopPadding = 0;
                loopStart = 0;
                totalSamples = samples = stream.Samples;

                blockLen = samples.Align(14) / 14 * 8;
                samplesPerBlock = blockLen / 8 * 14;
            }

            progress?.Begin(0, totalSamples * channels * 3, 0);

            blocks = (totalSamples + (samplesPerBlock - 1)) / samplesPerBlock;

            //Initialize stream info
            if ((tmp = totalSamples % samplesPerBlock) != 0)
            {
                lbSamples = tmp;
                lbSize = (lbSamples + 13) / 14 * 8;
                lbTotal = lbSize.Align(0x20);
            }
            else
            {
                lbSamples = samplesPerBlock;
                lbTotal = lbSize = blockLen;
            }

            //Get section sizes
            int headerSize = RWAV.Size,
                infoSize = 8,
                waveSize = 0x1C,
                tableSize = channels * 4,
                channelSize = channels * 0x1C,
                adpcmInfoSize = channels * 0x30,
                entrySize = (infoSize + waveSize + tableSize + channelSize + adpcmInfoSize).Align(0x20) - 8,
                dataSize = ((blocks - 1) * blockLen + lbTotal) * channels + 8;

            //Create file map
            FileMap map = FileMap.FromTempFile(headerSize + entrySize + 8 + dataSize);

            //Get section pointers
            RWAV* header = (RWAV*) map.Address;
            header->_header._tag = RWAV.Tag;
            header->_header.Endian = Endian.Big;
            header->_header._version = 0x102;
            header->_header._length = map.Length;
            header->_header._firstOffset = 0x20;
            header->_header._numEntries = 2;

            header->_infoOffset = 0x20;
            header->_infoLength = entrySize + 8;
            header->_dataOffset = 0x20 + entrySize + 8;
            header->_dataLength = dataSize;

            RWAVInfo* infoBlock = header->Info;
            infoBlock->_header._tag = RWAVInfo.Tag;
            infoBlock->_header._length = entrySize + 8;

            WaveInfo* wave = &infoBlock->_info;

            wave->_format = new AudioFormatInfo(2, (byte) (looped ? 1 : 0), (byte) channels, 0);
            wave->_sampleRate = (ushort) sampleRate;
            wave->_channelInfoTableOffset = 0x1C;
            wave->_dataLocation = (uint) (header->Data->Data - map.Address);

            wave->LoopSample = loopStart;
            wave->NumSamples = totalSamples;

            RWAVData* dataBlock = header->Data;
            dataBlock->_header._tag = RWAVData.Tag;
            dataBlock->_header._length = dataSize;

            //Create one ChannelInfo for each channel
            buint* table = (buint*) ((VoidPtr) wave + 0x1C);
            ChannelInfo* channelInfo = (ChannelInfo*) ((VoidPtr) wave + waveSize + tableSize);
            for (int i = 0; i < channels; i++)
            {
                table[i] = (uint) &channelInfo[i] - (uint) wave;
                channelInfo[i] = new ChannelInfo
                {
                    _volBackLeft = 1,
                    _volBackRight = 1,
                    _volFrontLeft = 1,
                    _volFrontRight = 1,
                    _adpcmInfoOffset = waveSize + tableSize + channelSize + i * 0x30
                };
            }

            //Create one ADPCMInfo for each channel
            int* adpcData = stackalloc int[channels];
            ADPCMInfo** pAdpcm = (ADPCMInfo**) adpcData;
            for (int i = 0; i < channels; i++)
            {
                *(pAdpcm[i] = wave->GetADPCMInfo(i)) = new ADPCMInfo();
            }

            //Create buffer for each channel
            int* bufferData = stackalloc int[channels];
            short** channelBuffers = (short**) bufferData;
            int bufferSamples = totalSamples + 2; //Add two samples for initial yn values
            for (int i = 0; i < channels; i++)
            {
                channelBuffers[i] = tPtr = (short*) Marshal.AllocHGlobal(bufferSamples * 2); //Two bytes per sample

                //Zero padding samples and initial yn values
                //for (int x = 0; x < (loopPadding + 2); x++)
                //    *tPtr++ = 0;
            }

            //Fill buffers
            stream.SamplePosition = 0;
            short* sampleBuffer = stackalloc short[channels];

            for (int i = 2; i < bufferSamples; i++)
            {
                //if (stream.SamplePosition == stream.LoopEndSample && looped)
                //    stream.SamplePosition = stream.LoopStartSample;

                stream.ReadSamples(sampleBuffer, 1);
                for (int x = 0; x < channels; x++)
                {
                    channelBuffers[x][i] = sampleBuffer[x];
                }
            }

            //Calculate coefs
            for (int i = 0; i < channels; i++)
            {
                AudioConverter.CalcCoefs(channelBuffers[i] + 2, totalSamples, (short*) pAdpcm[i], progress);
            }

            //Encode blocks
            byte* dPtr = (byte*) dataBlock->Data;
            for (int sIndex = 0, bIndex = 1; sIndex < totalSamples; sIndex += samplesPerBlock, bIndex++)
            {
                int blockSamples = Math.Min(totalSamples - sIndex, samplesPerBlock);
                for (int x = 0; x < channels; x++)
                {
                    channelInfo[x]._channelDataOffset = (int) (dPtr - (byte*) dataBlock->Data);
                    short* sPtr = channelBuffers[x] + sIndex;

                    //Set block yn values
                    if (bIndex != blocks)
                    {
                        pAdpcm[x]->_yn1 = sPtr[samplesPerBlock + 1];
                        pAdpcm[x]->_yn2 = sPtr[samplesPerBlock];
                    }

                    //Encode block (include yn in sPtr)
                    AudioConverter.EncodeBlock(sPtr, blockSamples, dPtr, (short*) pAdpcm[x]);

                    //Set initial ps
                    if (bIndex == 1)
                    {
                        pAdpcm[x]->_ps = *dPtr;
                    }

                    //Advance output pointer
                    if (bIndex == blocks)
                    {
                        //Fill remaining
                        dPtr += lbSize;
                        for (int i = lbSize; i < lbTotal; i++)
                        {
                            *dPtr++ = 0;
                        }
                    }
                    else
                    {
                        dPtr += blockLen;
                    }
                }

                if (progress != null && sIndex % samplesPerBlock == 0)
                {
                    progress.Update(progress.CurrentValue + samplesPerBlock * 2 * channels);
                }
            }

            //Reverse coefs
            for (int i = 0; i < channels; i++)
            {
                short* p = pAdpcm[i]->_coefs;
                for (int x = 0; x < 16; x++, p++)
                {
                    *p = p->Reverse();
                }
            }

            //Write loop states
            if (looped)
            {
                //Can't we just use block states?
                int loopBlock = loopStart / samplesPerBlock;
                int loopChunk = (loopStart - loopBlock * samplesPerBlock) / 14;
                dPtr = (byte*) dataBlock->Data + loopBlock * blockLen * channels + loopChunk * 8;
                tmp = loopBlock == blocks - 1 ? lbTotal : blockLen;

                for (int i = 0; i < channels; i++, dPtr += tmp)
                {
                    //Use adjusted samples for yn values
                    tPtr = channelBuffers[i] + loopStart;
                    pAdpcm[i]->_lps = *dPtr;
                    pAdpcm[i]->_lyn2 = *tPtr++;
                    pAdpcm[i]->_lyn1 = *tPtr;
                }
            }

            //Free memory
            for (int i = 0; i < channels; i++)
            {
                Marshal.FreeHGlobal((IntPtr) channelBuffers[i]);
            }

            progress?.Finish();

            return map;
        }
    }
}