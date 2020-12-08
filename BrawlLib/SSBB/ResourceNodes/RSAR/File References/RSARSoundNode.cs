using BrawlLib.Internal;
using BrawlLib.Internal.Audio;
using BrawlLib.SSBB.Types.Audio;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARSoundNode : RSAREntryNode, IAudioSource
    {
        internal INFOSoundEntry* Header => (INFOSoundEntry*) WorkingUncompressed.Address;

        public RSARSoundNode()
        {
            _volume = 50;
            _soundType = (byte) SndType.WAVE;
            _playerPriority = 64;
            _panMode = (byte) PanMode.Dual;
            _panCurve = (byte) PanCurve.Sqrt;

            _sound3dParam._flags = (uint) (Sound3DFlags.NotVolume | Sound3DFlags.NotSurroundPan);
            _sound3dParam._decayCurve = (byte) DecayCurve.Logarithmic;
            _sound3dParam._decayRatio = 128;

            _waveInfo._allocTrack = 1;
            _waveInfo._channelPriority = 64;
        }

        [Category("Data")]
        [DisplayName("Sound ID")]
        public override int StringId => Header == null ? -1 : (int) Header->_stringId;

        public override ResourceType ResourceFileType => ResourceType.RSARSound;

        public Sound3DParam _sound3dParam;
        public WaveSoundInfo _waveInfo;
        public StrmSoundInfo _strmInfo;
        public SeqSoundInfo _seqInfo;

        public RSEQLabelNode _seqLabl;
        public RSARFileNode _soundFileNode;
        public RWSDDataNode _waveDataNode;

        [Browsable(false)]
        public RSARBankNode RBNKNode
        {
            get
            {
                if (_seqInfo._bankId < RSARNode._infoCache[1].Count)
                {
                    return RSARNode._infoCache[1][_seqInfo._bankId] as RSARBankNode;
                }

                return null;
            }
        }

        public int _fileId;
        public int _playerId;
        public byte _volume;
        public byte _playerPriority;
        public byte _soundType = 3;
        public byte _remoteFilter;
        public byte _panMode;
        public byte _panCurve;
        public byte _actorPlayerId;
        public int _p1, _p2;

        public enum SndType
        {
            //Invalid = 0,

            SEQ = 1,
            STRM = 2,
            WAVE = 3
        }

        [Browsable(false)]
        public RSARFileNode SoundFileNode
        {
            get => _soundFileNode;
            set
            {
                if (_soundFileNode != value)
                {
                    if (_waveDataNode != null)
                    {
                        _waveInfo._soundIndex = -1;
                        _waveDataNode._refs.Remove(this);
                        _waveDataNode.GetName();
                        _waveDataNode = null;
                    }

                    if (_seqLabl != null)
                    {
                        _seqInfo._dataID = 0;
                        _seqLabl = null;
                    }

                    _soundFileNode = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("RSAR Sound")]
        [Browsable(true)]
        [TypeConverter(typeof(DropDownListNonBankFiles))]
        public string SoundFile
        {
            get => _soundFileNode == null ? "<null>" : _soundFileNode._name;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    SoundFileNode = null;
                }
                else
                {
                    Type e;
                    switch (SoundType)
                    {
                        default: return;
                        case SndType.SEQ:
                            e = typeof(RSEQNode);
                            break;
                        case SndType.STRM:
                            e = typeof(RSTMNode);
                            break;
                        case SndType.WAVE:
                            e = typeof(RWSDNode);
                            break;
                    }

                    int t = 0;
                    RSARFileNode node = null;
                    Type ext = typeof(RSARExtFileNode);
                    foreach (RSARFileNode r in RSARNode.Files)
                    {
                        if (r.Name == value)
                        {
                            Type thisType = r.GetType();
                            if (thisType == e || thisType == ext)
                            {
                                node = r;
                                break;
                            }
                        }

                        ++t;
                    }

                    if (node != null)
                    {
                        SoundFileNode = node;
                        _fileId = t;
                    }
                }
            }
        }

        [Category("RSAR Sound")]
        public int PlayerId
        {
            get => _playerId;
            set
            {
                _playerId = value;
                SignalPropertyChange();
            }
        }

        [Category("RSAR Sound")]
        public byte Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                SignalPropertyChange();
            }
        }

        [Category("RSAR Sound")]
        public byte PlayerPriority
        {
            get => _playerPriority;
            set
            {
                _playerPriority = value;
                SignalPropertyChange();
            }
        }

        [Category("RSAR Sound")]
        public SndType SoundType
        {
            get => (SndType) _soundType;
            set
            {
                _soundType = ((byte) value).Clamp(1, 3);
                SignalPropertyChange();
            }
        }

        [Category("RSAR Sound")]
        public byte RemoteFilter
        {
            get => _remoteFilter;
            set
            {
                _remoteFilter = value;
                SignalPropertyChange();
            }
        }

        [Category("RSAR Sound")]
        public PanMode PanMode
        {
            get => (PanMode) _panMode;
            set
            {
                _panMode = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("RSAR Sound")]
        public PanCurve PanCurve
        {
            get => (PanCurve) _panCurve;
            set
            {
                _panCurve = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("RSAR Sound")]
        public byte ActorPlayerId
        {
            get => _actorPlayerId;
            set
            {
                _actorPlayerId = value;
                SignalPropertyChange();
            }
        }

        [Category("RSAR Sound")]
        public int UserParam1
        {
            get => _p1;
            set
            {
                _p1 = value;
                SignalPropertyChange();
            }
        }

        [Category("RSAR Sound")]
        public int UserParam2
        {
            get => _p2;
            set
            {
                _p2 = value;
                SignalPropertyChange();
            }
        }

        [Flags]
        public enum Sound3DFlags
        {
            NotVolume = 1,
            NotPan = 2,
            NotSurroundPan = 4,
            NotPriority = 8,
            Filter = 16
        }

        [Category("Sound 3D Param")]
        public Sound3DFlags Flags
        {
            get => (Sound3DFlags) (uint) _sound3dParam._flags;
            set
            {
                _sound3dParam._flags = (uint) value;
                SignalPropertyChange();
            }
        }

        [Category("Sound 3D Param")]
        public DecayCurve DecayCurve
        {
            get => (DecayCurve) _sound3dParam._decayCurve;
            set
            {
                _sound3dParam._decayCurve = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Sound 3D Param")]
        public byte DecayRatio
        {
            get => _sound3dParam._decayRatio;
            set
            {
                _sound3dParam._decayRatio = value;
                SignalPropertyChange();
            }
        }

        [Category("Sound 3D Param")]
        public byte DopplerFactor
        {
            get => _sound3dParam._dopplerFactor;
            set
            {
                _sound3dParam._dopplerFactor = value;
                SignalPropertyChange();
            }
        }

        [Category("SEQ Params")]
        [Browsable(true)]
        [TypeConverter(typeof(DropDownListRSARInfoSeqLabls))]
        public string SeqLabelEntry
        {
            get => _seqLabl == null
                ? _soundFileNode is RSARExtFileNode && SoundType == SndType.SEQ ? "<null>" :
                _seqInfo._dataID.ToString()
                : _seqLabl._name;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _seqInfo._dataID = 0;
                }
                else
                {
                    if (SoundFileNode is RSEQNode)
                    {
                        foreach (RSEQLabelNode r in SoundFileNode.Children)
                        {
                            if (r.Name == value)
                            {
                                _seqInfo._dataID = r.Id;
                                SignalPropertyChange();
                                break;
                            }

                            if (SoundFileNode is RSARExtFileNode)
                            {
                                if (uint.TryParse(value, out uint id))
                                {
                                    _seqInfo._dataID = id;
                                }
                            }
                        }
                    }
                }
            }
        }

        [Category("SEQ Params")]
        [Browsable(true)]
        [TypeConverter(typeof(DropDownListRSARBankRefFiles))]
        public string BankFile
        {
            get => RBNKNode == null ? "<null>" : RBNKNode.TreePath;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _seqInfo._bankId = -1;
                }
                else
                {
                    RSARBankNode node = null;
                    int t = 0;
                    foreach (ResourceNode r in RSARNode._infoCache[1])
                    {
                        if (r.Name == value && r is RSARBankNode)
                        {
                            node = r as RSARBankNode;
                            break;
                        }

                        t++;
                    }

                    if (node != null)
                    {
                        _seqInfo._bankId = t;
                        SignalPropertyChange();
                    }
                }
            }
        }

        [Flags]
        public enum AllocTrackEnum
        {
            None = 0x0,
            Track1 = 0x1,
            Track2 = 0x2,
            Track3 = 0x4,
            Track4 = 0x8,
            Track5 = 0x10,
            Track6 = 0x20,
            Track7 = 0x40,
            Track8 = 0x80,
            Track9 = 0x100,
            Track10 = 0x200,
            Track11 = 0x400,
            Track12 = 0x800,
            Track13 = 0x1000,
            Track14 = 0x2000,
            Track15 = 0x4000,
            Track16 = 0x8000
        }

        [Category("SEQ Params")]
        public AllocTrackEnum RSEQAllocTrack
        {
            get => (AllocTrackEnum) (uint) _seqInfo._allocTrack;
            set
            {
                _seqInfo._allocTrack = (uint) value;
                SignalPropertyChange();
            }
        }

        [Category("SEQ Params")]
        public byte SeqChannelPriority
        {
            get => _seqInfo._channelPriority;
            set
            {
                _seqInfo._channelPriority = value;
                SignalPropertyChange();
            }
        }

        [Category("SEQ Params")]
        public byte SeqReleasePriorityFix
        {
            get => _seqInfo._releasePriorityFix;
            set
            {
                _seqInfo._releasePriorityFix = value;
                SignalPropertyChange();
            }
        }

        [Category("STRM Params")]
        public uint StartPosition
        {
            get => _strmInfo._startPosition;
            set
            {
                _strmInfo._startPosition = value;
                SignalPropertyChange();
            }
        }

        [Category("STRM Params")]
        public ushort AllocChannelCount
        {
            get => _strmInfo._allocChannelCount;
            set
            {
                _strmInfo._allocChannelCount = value;
                SignalPropertyChange();
            }
        }

        [Category("STRM Params")]
        public AllocTrackEnum RSTMAllocTrack
        {
            get => (AllocTrackEnum) (ushort) _strmInfo._allocTrackFlag;
            set
            {
                _strmInfo._allocTrackFlag = (ushort) value;
                SignalPropertyChange();
            }
        }

        [Category("WAVE Params")]
        [Browsable(true)]
        [TypeConverter(typeof(DropDownListRSARInfoSound))]
        public string SoundDataNode
        {
            get => _waveDataNode == null ? "<null>" : _waveDataNode._name;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _waveInfo._soundIndex = -1;
                    if (_waveDataNode != null)
                    {
                        _waveDataNode._refs.Remove(this);
                        _waveDataNode.GetName();
                        _waveDataNode = null;
                    }
                }
                else
                {
                    if (SoundFileNode == null || SoundType != SndType.WAVE)
                    {
                        return;
                    }

                    RWSDDataNode node = null;
                    int t = 0;

                    if (value != null)
                    {
                        foreach (RWSDDataNode r in SoundFileNode.Children[0].Children)
                        {
                            if (r.Name == value)
                            {
                                node = r;
                                break;
                            }

                            t++;
                        }
                    }

                    if (node != null)
                    {
                        _waveInfo._soundIndex = t;

                        if (_waveDataNode != null)
                        {
                            _waveDataNode._refs.Remove(this);
                            _waveDataNode.GetName();
                        }

                        _waveDataNode = node;
                        _waveDataNode._refs.Add(this);
                        _waveDataNode.GetName();
                    }
                    else
                    {
                        _waveInfo._soundIndex = -1;
                        _waveDataNode = null;
                    }

                    SignalPropertyChange();
                }
            }
        }

        //[Category("WAVE Params")]
        //public int PackIndex { get { return _waveInfo._soundIndex; } set { _waveInfo._soundIndex = value; SignalPropertyChange(); } }
        [Category("WAVE Params")]
        public AllocTrackEnum RWSDAllocTrack
        {
            get => (AllocTrackEnum) (uint) _waveInfo._allocTrack;
            set
            {
                _waveInfo._allocTrack = (uint) value;
                SignalPropertyChange();
            }
        }

        [Category("WAVE Params")]
        public byte ChannelPriority
        {
            get => _waveInfo._channelPriority;
            set
            {
                _waveInfo._channelPriority = value;
                SignalPropertyChange();
            }
        }

        [Category("WAVE Params")]
        public byte ReleasePriorityFix
        {
            get => _waveInfo._releasePriorityFix;
            set
            {
                _waveInfo._releasePriorityFix = value;
                SignalPropertyChange();
            }
        }

        [Category("Audio Stream")]
        public WaveEncoding Encoding => _waveDataNode == null ? WaveEncoding.ADPCM : _waveDataNode.Encoding;

        [Category("Audio Stream")] public int Channels => _waveDataNode == null ? 0 : _waveDataNode.Channels;
        [Category("Audio Stream")] public bool IsLooped => _waveDataNode == null ? false : _waveDataNode.IsLooped;
        [Category("Audio Stream")] public int SampleRate => _waveDataNode == null ? 0 : _waveDataNode.SampleRate;

        [Category("Audio Stream")]
        public int LoopStartSample => _waveDataNode == null ? 0 : _waveDataNode.LoopStartSample;

        [Category("Audio Stream")] public int NumSamples => _waveDataNode == null ? 0 : _waveDataNode.NumSamples;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _fileId = Header->_fileId;
            _playerId = Header->_playerId;
            _volume = Header->_volume;
            _playerPriority = Header->_playerPriority;
            _soundType = Header->_soundType;
            _remoteFilter = Header->_remoteFilter;
            _panMode = Header->_panMode;
            _panCurve = Header->_panCurve;
            _actorPlayerId = Header->_actorPlayerId;
            _p1 = Header->_userParam1;
            _p2 = Header->_userParam2;

            INFOHeader* info = RSARNode.Header->INFOBlock;
            _sound3dParam = *Header->GetParam3dRef(&info->_collection);

            VoidPtr addr = Header->GetSoundInfoRef(&info->_collection);
            switch (Header->_soundInfoRef._dataType)
            {
                case 1:
                    _seqInfo = *(SeqSoundInfo*) addr;
                    break;
                case 2:
                    _strmInfo = *(StrmSoundInfo*) addr;
                    break;
                case 3:
                    _waveInfo = *(WaveSoundInfo*) addr;
                    break;
            }

            _soundFileNode = RSARNode.Files[_fileId];
            _soundFileNode.AddSoundRef(this);

            if (_soundFileNode is RSEQNode)
            {
                foreach (RSEQLabelNode r in _soundFileNode.Children)
                {
                    if (_seqInfo._dataID == r.Id)
                    {
                        _seqLabl = r;
                        break;
                    }
                }
            }

            if (_waveInfo._soundIndex >= 0 &&
                _soundFileNode is RWSDNode &&
                _soundFileNode.Children.Count > 0 &&
                _soundFileNode.Children[0].Children.Count > _waveInfo._soundIndex)
            {
                _waveDataNode = _soundFileNode.Children[0].Children[_waveInfo._soundIndex] as RWSDDataNode;
                _waveDataNode?._refs.Add(this);
            }

            return false;
        }

        public IAudioStream _stream;

        public IAudioStream[] CreateStreams()
        {
            _stream = null;
            if (_soundFileNode is RWSDNode)
            {
                RWSDDataNode d = _waveDataNode;
                if (d != null && _soundFileNode.Children.Count > 1 &&
                    _soundFileNode.Children[1].Children.Count > d._part3._waveIndex && d._part3._waveIndex >= 0)
                {
                    _stream = (_soundFileNode.Children[1].Children[d._part3._waveIndex] as RSARFileAudioNode)
                        .CreateStreams()[0];
                }
            }

            return new IAudioStream[] {_stream};
        }

        public override int OnCalculateSize(bool force)
        {
            int size = INFOSoundEntry.Size + Sound3DParam.Size;
            switch (SoundType)
            {
                case SndType.SEQ:
                    size += SeqSoundInfo.Size;
                    break;
                case SndType.STRM:
                    size += StrmSoundInfo.Size;
                    break;
                case SndType.WAVE:
                    size += WaveSoundInfo.Size;
                    break;
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            INFOSoundEntry* header = (INFOSoundEntry*) address;
            VoidPtr addr = address + INFOSoundEntry.Size;
            header->_stringId = _rebuildStringId;
            header->_fileId = _fileId;
            header->_playerId = _playerId;
            header->_volume = _volume;
            header->_playerPriority = _playerPriority;
            header->_soundType = _soundType;
            header->_remoteFilter = _remoteFilter;
            header->_panMode = _panMode;
            header->_panCurve = _panCurve;
            header->_actorPlayerId = _actorPlayerId;
            header->_soundInfoRef = (uint) (addr - _rebuildBase);
            header->_userParam1 = _p1;
            header->_userParam2 = _p2;
            switch (SoundType)
            {
                case SndType.SEQ:
                    *(SeqSoundInfo*) addr = _seqInfo;
                    header->_soundInfoRef._dataType = 1;
                    addr += SeqSoundInfo.Size;
                    break;
                case SndType.STRM:
                    *(StrmSoundInfo*) addr = _strmInfo;
                    header->_soundInfoRef._dataType = 2;
                    addr += StrmSoundInfo.Size;
                    break;
                case SndType.WAVE:
                    *(WaveSoundInfo*) addr = _waveInfo;
                    header->_soundInfoRef._dataType = 3;
                    addr += WaveSoundInfo.Size;
                    break;
            }

            header->_param3dRefOffset = (uint) (addr - _rebuildBase);
            *(Sound3DParam*) addr = _sound3dParam;
        }

        public override void Export(string outPath)
        {
            if (outPath.EndsWith(".wav"))
            {
                IAudioStream stream = CreateStreams()[0];
                if (stream == null)
                {
                    MessageBox.Show($"{_soundFileNode?.GetType().Name ?? ""} \"{Name}\" has no associated sound data and cannot be exported to WAV.");
                    return;
                }

                WAV.ToFile(stream, outPath);
            }
            else
            {
                base.Export(outPath);
            }
        }
    }
}