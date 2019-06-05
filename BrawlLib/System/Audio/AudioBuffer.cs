namespace System.Audio
{
    public abstract class AudioBuffer : IDisposable
    {
        //Buffer will be valid for two seconds. The application MUST update/fill before then.
        //This is plenty of time, as timer updates should occur every 10 - 100 ms.
        internal const int DefaultBufferSpan = 2;

        internal int _bitsPerSample;

        //Number of bytes in each sample. (_bitsPerSample * _channels / 8)
        internal int _blockAlign;

        internal int _channels;

        //Total byte length of the buffer.
        internal int _dataLength;

        internal WaveFormatTag _format;

        internal int _frequency;

        //Sets whether the buffer manages looping.
        //Use this with Source.
        internal bool _loop;

        internal AudioProvider _owner;

        //Byte offset within buffer in which reading is currently commencing.
        //The application must call Update (or Fill) to update this value.
        internal int _readOffset;

        //Cumulative sample position in which the buffer is currently reading.
        //This value is updated as Update is called.
        internal int _readSample;

        //Number of samples that can be stored inside the buffer.
        internal int _sampleLength;

        internal IAudioStream _source;

        //Byte offset within buffer in which to continue writing.
        //Read-only. It is the responsibility of the application to update the audio data in a timely manner.
        //As data is written, this is updated automatically.
        internal int _writeOffset;

        //Cumulative sample position in which to continue writing.
        //This value is updated automatically when fill is called.
        internal int _writeSample;
        public AudioProvider Owner => _owner;
        public IAudioStream Source => _source;
        public WaveFormatTag Format => _format;
        public int Frequency => _frequency;
        public int Channels => _channels;
        public int BitsPerSample => _bitsPerSample;
        public int SampleLength => _sampleLength;
        public int DataLength => _dataLength;
        public int BlockAlign => _blockAlign;
        public int WriteOffset => _writeOffset;
        public int ReadOffset => _readOffset;
        public int WriteSample => _writeSample;
        public int ReadSample => _readSample;

        public bool Loop
        {
            get => _loop;
            set => _loop = value;
        }

        //internal bool _playing = false;
        //public bool IsPlaying { get { return _playing; } }

        //Byte offset within buffer in which playback is commencing.
        internal abstract int PlayCursor { get; set; }

        public abstract int Volume { get; set; }
        public abstract int Pan { get; set; }

        public virtual void Dispose()
        {
            if (_owner != null)
            {
                _owner._buffers.Remove(this);
                _owner = null;
            }

            GC.SuppressFinalize(this);
        }

        ~AudioBuffer()
        {
            Dispose();
        }

        public abstract void Play();
        public abstract void Stop();
        public abstract BufferData Lock(int offset, int length);
        public abstract void Unlock(BufferData data);

        //Should only be used while playback is stopped
        public void Seek(int samplePos)
        {
            _readOffset = _writeOffset = PlayCursor;
            _readSample = _writeSample = samplePos;

            if (_source != null) _source.SamplePosition = samplePos;
        }

        public void Reset()
        {
            _readOffset = _writeOffset = PlayCursor;
        }

        public virtual void Update()
        {
            //Get current sample offset.
            var sampleOffset = PlayCursor / _blockAlign;
            //Get current byte offset
            var byteOffset = sampleOffset * _blockAlign;
            //Get sample difference since last update, taking into account circular wrapping.
            var sampleDifference = ((byteOffset < _readOffset ? byteOffset + _dataLength : byteOffset) - _readOffset) /
                                   _blockAlign;
            //Get byte difference
            //int byteDifference = sampleDifference * _blockAlign;

            //If no change, why continue?
            if (sampleDifference == 0) return;

            //Set new read offset.
            _readOffset = byteOffset;

            //Update looping
            if (_source != null)
            {
                if (_loop && _source.IsLooping)
                {
                    var start = _source.LoopStartSample;
                    var end = _source.LoopEndSample;
                    var newSample = _readSample + sampleDifference;

                    if (newSample >= end && _writeSample < _readSample)
                        _readSample = start + (newSample - start) % (end - start);
                    else
                        _readSample = Math.Min(newSample, _source.Samples);
                }
                else
                {
                    _readSample = Math.Min(_readSample + sampleDifference, _source.Samples);
                    //if (_readSample >= _source.Samples)
                    //    Stop();
                }
            }
            else
            {
                _readSample += sampleDifference;
            }
        }

        public virtual void Fill()
        {
            //This only works if a source has been assigned!
            if (_source == null) return;

            //Update read position
            Update();

            //Get number of samples available for writing. 
            var sampleCount = ((_readOffset <= _writeOffset ? _readOffset + _dataLength : _readOffset) - _writeOffset) /
                              _blockAlign / 8;

            //Fill samples
            Fill(_source, sampleCount, _loop);
        }

        public virtual void Fill(IAudioStream source, int samples, bool loop)
        {
            var byteCount = samples * _blockAlign;

            //Lock buffer and fill
            var data = Lock(_writeOffset, byteCount);
            try
            {
                data.Fill(source, loop);
            }
            finally
            {
                Unlock(data);
            }

            //Advance offsets
            _writeOffset = (_writeOffset + byteCount) % _dataLength;
            _writeSample = source.SamplePosition;
        }
    }
}