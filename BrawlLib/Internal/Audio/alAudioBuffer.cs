using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;

namespace BrawlLib.Internal.Audio
{
    /// <summary>
    /// The alAudioBuffer presents itself to BrawlLib as one buffer big enough
    /// to hold the whole IAudioSource, while maintaining OpenAL's buffers
    /// internally.
    /// </summary>
    internal class alAudioBuffer : AudioBuffer
    {
        // This class stores the source id and the length of discarded buffers.
        private class alSourceLock
        {
            public int currentSource;

            // Total length of discarded buffers. Maybe there's a better way to do this?
            public int addToCursor;
        }

        private readonly alAudioProvider _parent;

        //Lock on this before using what's inside of it.
        private readonly alSourceLock sourceLock;

        // Buffers that need to be added to the stream once it starts playing.
        private readonly List<int> buffersToQueue;

        private UnsafeBuffer _internalBuffer;

        internal override int PlayCursor
        {
            get
            {
                lock (sourceLock)
                {
                    if (sourceLock.currentSource == 0)
                    {
                        return 0;
                    }

                    AL.GetSource(sourceLock.currentSource, ALGetSourcei.SampleOffset, out int v);
                    return 4 * v + sourceLock.addToCursor;
                }
            }
            set
            {
                lock (sourceLock)
                {
                    if (sourceLock.currentSource == 0)
                    {
                        return;
                    }

                    AL.Source(sourceLock.currentSource, ALSourcei.SampleOffset, value - sourceLock.addToCursor);
                }
            }
        }

        private int _volume;

        public override int Volume
        {
            get
            {
                lock (sourceLock)
                {
                    if (sourceLock.currentSource == 0)
                    {
                        return _volume;
                    }

                    AL.GetSource((uint) sourceLock.currentSource, ALSourcef.Gain, out float v);
                    return Math.Max(-10000, (int) (Math.Log10(v) * 2000));
                }
            }
            set
            {
                lock (sourceLock)
                {
                    _volume = value;
                    if (sourceLock.currentSource == 0)
                    {
                        return;
                    }

                    double pct = Math.Pow(10, (double) value / 2000);
                    AL.Source(sourceLock.currentSource, ALSourcef.Gain, (float) pct);
                }
            }
        }

        public override int Pan
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        internal alAudioBuffer(alAudioProvider parent, WaveFormatEx fmt, int sampleSize)
        {
            _parent = parent;

            buffersToQueue = new List<int>();
            sourceLock = new alSourceLock();

            int size = sampleSize * fmt.nChannels * fmt.wBitsPerSample / 8;
            if (size == 0)
            {
                return;
            }

            _internalBuffer = new UnsafeBuffer(size);

            _format = fmt.wFormatTag;
            _frequency = (int) fmt.nSamplesPerSec;
            _channels = fmt.nChannels;
            _bitsPerSample = fmt.wBitsPerSample;
            _dataLength = size;
            _blockAlign = _bitsPerSample * _channels / 8;
            _sampleLength = _dataLength / _blockAlign;
        }

        public override void Dispose()
        {
            _internalBuffer?.Dispose();
            _internalBuffer = null;
            base.Dispose();
        }

        public override BufferData Lock(int offset, int length)
        {
            BufferData data = new BufferData();

            offset = offset.Align(_blockAlign);
            length = length.Align(_blockAlign);

            data._dataOffset = offset;
            data._dataLength = length;
            data._sampleOffset = offset / _blockAlign;
            data._sampleLength = length / _blockAlign;

            if (length != 0)
            {
                // Create a "fake" BufferData that does not point to an actual sound buffer.
                // We'll populate the real buffer when we unlock it.
                if (_internalBuffer == null || _internalBuffer.Length < length)
                {
                    throw new Exception("alAudioBuffer not big enough");
                }

                data._part1Address = _internalBuffer.Address;
                data._part1Length = length;
                data._part1Samples = length / _blockAlign;

                data._part2Address = IntPtr.Zero;
                data._part2Length = 0;
                data._part2Samples = 0;
            }

            return data;
        }

        public override void Unlock(BufferData data)
        {
            int buffer = AL.GenBuffer();
            AL.BufferData(buffer, GetSoundFormat(_channels, _bitsPerSample), data.Part1Address, data.Part1Length,
                _frequency);

            lock (sourceLock)
            {
                if (sourceLock.currentSource != 0)
                {
                    // Already playing - add buffer to source
                    AL.SourceQueueBuffer(sourceLock.currentSource, buffer);
                }
                else
                {
                    // This buffer will need to be added once the source is created
                    lock (buffersToQueue)
                    {
                        buffersToQueue.Add(buffer);
                    }
                }
            }

            Dequeue();
        }

        private void Dequeue()
        {
            lock (sourceLock)
            {
                AL.GetSource(sourceLock.currentSource, ALGetSourcei.BuffersProcessed, out int dequeuedBuffers);
                if (dequeuedBuffers > 0)
                {
                    int[] bufferids = new int[dequeuedBuffers];
                    AL.SourceUnqueueBuffers(sourceLock.currentSource, dequeuedBuffers, bufferids);
                    foreach (int id in bufferids)
                    {
                        AL.GetBuffer(id, ALGetBufferi.Size, out int length);
                        sourceLock.addToCursor += length;
                        AL.DeleteBuffer(id);
                    }
                }
            }
        }

        public static ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1:  return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2:  return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }

        public override void Play()
        {
            lock (sourceLock)
            {
                if (sourceLock.currentSource != 0)
                {
                    throw new Exception("Cannot start when already playing");
                }

                sourceLock.currentSource = AL.GenSource();
                lock (buffersToQueue)
                {
                    foreach (int buffer in buffersToQueue)
                    {
                        AL.SourceQueueBuffer(sourceLock.currentSource, buffer);
                    }

                    buffersToQueue.Clear();
                }

                Volume = _volume;
                AL.SourcePlay(sourceLock.currentSource);
            }
        }

        public override void Stop()
        {
            lock (sourceLock)
            {
                if (sourceLock.currentSource == 0)
                {
                    return;
                }

                AL.SourceStop(sourceLock.currentSource);

                Dequeue();

                sourceLock.addToCursor = 0;

                AL.DeleteSource(sourceLock.currentSource);
                sourceLock.currentSource = 0;
            }
        }
    }
}