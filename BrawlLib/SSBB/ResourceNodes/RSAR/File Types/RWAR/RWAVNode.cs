using BrawlLib.Internal;
using BrawlLib.Internal.Audio;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.Types.Audio;
using BrawlLib.Wii.Audio;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARFileAudioNode : RSARFileEntryNode, IAudioSource, IDisposable
    {
        public override ResourceType ResourceFileType => ResourceType.RSARFileAudioEntry;

        [Browsable(false)]
        public WaveInfo Info
        {
            get => _info;
            set
            {
                _info = value;
                _encoding = _info._format._encoding;
                _channels = _info._format._channels;
                _looped = _info._format._looped != 0;
                _sampleRate = _info._sampleRate;
                _loopStart = _info.LoopSample;
                _numSamples = _info.NumSamples;
            }
        }

        private WaveInfo _info;

        public DataSource _audioSource;

        internal int _encoding;
        internal int _channels;
        internal bool _looped;
        internal int _sampleRate;
        internal int _loopStart;
        internal int _numSamples;

        [Category("Audio Stream")] public WaveEncoding Encoding => (WaveEncoding) _encoding;
        [Category("Audio Stream")] public int Channels => _channels;
        [Category("Audio Stream")] public bool IsLooped => _looped;
        [Category("Audio Stream")] public int SampleRate => _sampleRate;
        [Category("Audio Stream")] public int LoopStartSample => _loopStart;
        [Category("Audio Stream")] public int NumSamples => _numSamples;

        internal IAudioStream _stream;
        internal UnsafeBuffer _streamBuffer;

        ~RSARFileAudioNode()
        {
            Dispose();
        }

        public override void Dispose()
        {
            _audioSource.Close();

            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }

            if (_streamBuffer != null)
            {
                _streamBuffer.Dispose();
                _streamBuffer = null;
            }

            base.Dispose();
        }

        public void Init(VoidPtr strmAddr, int strmLen, WaveInfo* info)
        {
            Info = *info;

            _streamBuffer = new UnsafeBuffer(strmLen);
            Memory.Move(_streamBuffer.Address, strmAddr, (uint) strmLen);
            _audioSource = new DataSource(_streamBuffer.Address, _streamBuffer.Length);

            if (info->_format._encoding == 2)
            {
                _stream = new ADPCMStream(info, _audioSource.Address);
            }
            else
            {
                _stream = new PCMStream(info, _audioSource.Address);
            }
        }

        //public int GetSize(bool RWAR)
        //{
        //    if (RWAR)
        //    {
        //        if (!(this is RWAVNode))
        //        {

        //        }
        //        else
        //        {
        //            return WorkingUncompressed.Length;
        //        }
        //    }
        //    else
        //    {

        //    }
        //}

        //public int CalcSizeAsRWAV()
        //{

        //}

        //public int CalcSizeAsRSARSound()
        //{

        //}

        //public void RebuildAsRWAV(VoidPtr address, int length)
        //{

        //}

        //public void RebuildAsRSARSound(VoidPtr address, int length)
        //{

        //}

        public virtual IAudioStream[] CreateStreams()
        {
            return new IAudioStream[] {_stream};
        }
    }

    public unsafe class RWAVNode : RSARFileAudioNode
    {
        internal RWAV* Header => (RWAV*) WorkingUncompressed.Address;

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = $"Audio[{Index}]";
            }

            Init(Header->Data->Data, Header->Data->_header._length, &Header->Info->_info);

            SetSizeInternal(Header->_header._length);

            return false;
        }

        public override void Replace(string fileName)
        {
            if (fileName.EndsWith(".wav"))
            {
                using (BrstmConverterDialog dlg = new BrstmConverterDialog())
                {
                    dlg.Type = 2;
                    dlg.AudioSource = fileName;
                    if (dlg.ShowDialog(null) == DialogResult.OK)
                    {
                        ReplaceRaw(dlg.AudioData);
                    }
                }
            }
            else
            {
                base.Replace(fileName);
            }

            //Init(Header->Data->Data, Header->Data->_header._length, &Header->Info->_info);

            UpdateCurrentControl();
            SignalPropertyChange();
            Parent.Parent.SignalPropertyChange();
            RSARNode?.SignalPropertyChange();
        }

        public override void Export(string outPath)
        {
            if (outPath.EndsWith(".wav"))
            {
                WAV.ToFile(CreateStreams()[0], outPath);
            }
            else
            {
                base.Export(outPath);
            }
        }
    }
}