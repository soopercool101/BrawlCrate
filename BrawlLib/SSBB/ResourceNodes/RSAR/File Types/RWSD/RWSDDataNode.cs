using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Audio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RWSDDataNode : RSARFileEntryNode
    {
        internal RWSD_DATAEntry* Header => (RWSD_DATAEntry*) WorkingUncompressed.Address;

        public RWSD_WSDEntry _part1;
        public RWSD_NoteEvent _part2;
        public RWSD_NoteInfo _part3;

        public List<RSARSoundNode> _refs = new List<RSARSoundNode>();
        public string[] References => _refs.Select(x => x.TreePath).ToArray();

        [Category("WSD Info")]
        public float Pitch
        {
            get => _part1._pitch;
            set
            {
                _part1._pitch = value;
                SignalPropertyChange();
            }
        }

        [Category("WSD Info")]
        public byte Pan
        {
            get => _part1._pan;
            set
            {
                _part1._pan = value;
                SignalPropertyChange();
            }
        }

        [Category("WSD Info")]
        public byte SurroundPan
        {
            get => _part1._surroundPan;
            set
            {
                _part1._surroundPan = value;
                SignalPropertyChange();
            }
        }

        [Category("WSD Info")]
        public byte FxSendA
        {
            get => _part1._fxSendA;
            set
            {
                _part1._fxSendA = value;
                SignalPropertyChange();
            }
        }

        [Category("WSD Info")]
        public byte FxSendB
        {
            get => _part1._fxSendB;
            set
            {
                _part1._fxSendB = value;
                SignalPropertyChange();
            }
        }

        [Category("WSD Info")]
        public byte FxSendC
        {
            get => _part1._fxSendC;
            set
            {
                _part1._fxSendC = value;
                SignalPropertyChange();
            }
        }

        [Category("WSD Info")]
        public byte MainSend
        {
            get => _part1._mainSend;
            set
            {
                _part1._mainSend = value;
                SignalPropertyChange();
            }
        }

        [Category("Note Event")]
        public float Position
        {
            get => _part2.position;
            set
            {
                _part2.position = value;
                SignalPropertyChange();
            }
        }

        [Category("Note Event")]
        public float Length
        {
            get => _part2.length;
            set
            {
                _part2.length = value;
                SignalPropertyChange();
            }
        }

        [Category("Note Event")]
        public uint Decay
        {
            get => _part2.noteIndex;
            set
            {
                _part2.noteIndex = value;
                SignalPropertyChange();
            }
        }

        private RSARFileAudioNode _soundNode;

        [Browsable(false)]
        public RSARFileAudioNode Sound
        {
            get => _soundNode;
            set
            {
                if (_soundNode != value)
                {
                    _soundNode = value;
                }
            }
        }

        [Category("Note Info")]
        [Browsable(true)]
        [TypeConverter(typeof(DropDownListRWSDSounds))]
        public string Wave
        {
            get => _soundNode == null ? null : _soundNode._name;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Sound = null;
                }
                else
                {
                    RSARFileAudioNode node = null;
                    int t = 0;
                    foreach (RSARFileAudioNode r in Parent.Parent.Children[1].Children)
                    {
                        if (r.Name == value)
                        {
                            node = r;
                            break;
                        }

                        t++;
                    }

                    if (node != null)
                    {
                        Sound = node;
                        _part3._waveIndex = t;
                        SignalPropertyChange();
                    }
                }
            }
        }

        //[Category("Note Info")]
        //public int WaveIndex { get { return _part3._waveIndex; } set { _part3._waveIndex = value; } }
        [Category("Note Info")]
        public byte Attack
        {
            get => _part3._attack;
            set
            {
                _part3._attack = value;
                SignalPropertyChange();
            }
        }

        [Category("Note Info")]
        public byte InfoDecay
        {
            get => _part3._decay;
            set
            {
                _part3._decay = value;
                SignalPropertyChange();
            }
        }

        [Category("Note Info")]
        public byte Sustain
        {
            get => _part3._sustain;
            set
            {
                _part3._sustain = value;
                SignalPropertyChange();
            }
        }

        [Category("Note Info")]
        public byte Release
        {
            get => _part3._release;
            set
            {
                _part3._release = value;
                SignalPropertyChange();
            }
        }

        [Category("Note Info")]
        public byte Hold
        {
            get => _part3._hold;
            set
            {
                _part3._hold = value;
                SignalPropertyChange();
            }
        }

        [Category("Note Info")]
        public byte OriginalKey
        {
            get => _part3._originalKey;
            set
            {
                _part3._originalKey = value;
                SignalPropertyChange();
            }
        }

        [Category("Note Info")]
        public byte Volume
        {
            get => _part3._volume;
            set
            {
                _part3._volume = value;
                SignalPropertyChange();
            }
        }

        [Category("Note Info")]
        public byte InfoPan
        {
            get => _part3._pan;
            set
            {
                _part3._pan = value;
                SignalPropertyChange();
            }
        }

        [Category("Note Info")]
        public byte InfoSurroundPan
        {
            get => _part3._surroundPan;
            set
            {
                _part3._surroundPan = value;
                SignalPropertyChange();
            }
        }

        [Category("Note Info")]
        public float InfoPitch
        {
            get => _part3._pitch;
            set
            {
                _part3._pitch = value;
                SignalPropertyChange();
            }
        }

        [Category("Audio Stream")]
        public WaveEncoding Encoding => _soundNode == null ? WaveEncoding.ADPCM : _soundNode.Encoding;

        [Category("Audio Stream")] public int Channels => _soundNode == null ? 0 : _soundNode.Channels;
        [Category("Audio Stream")] public bool IsLooped => _soundNode == null ? false : _soundNode.IsLooped;
        [Category("Audio Stream")] public int SampleRate => _soundNode == null ? 0 : _soundNode.SampleRate;
        [Category("Audio Stream")] public int LoopStartSample => _soundNode == null ? 0 : _soundNode.LoopStartSample;
        [Category("Audio Stream")] public int NumSamples => _soundNode == null ? 0 : _soundNode.NumSamples;

        //[Category("Data Note Event")]
        //public List<RWSD_NoteEvent> Part2 { get { return _part2; } }
        //[Category("Data Note Info")]
        //public List<RWSD_NoteInfo> Part3 { get { return _part3; } }

        public override bool OnInitialize()
        {
            RuintList* list;

            _part1 = *Header->GetWsdInfo(_offset);

            list = Header->GetTrackTable(_offset); //Count is always 1
            ruint* r = (ruint*) list->Get(_offset, 0);
            RuintList* l = (RuintList*) r->Offset(_offset);
            _part2 = *(RWSD_NoteEvent*) l->Get(_offset, 0);

            list = Header->GetNoteTable(_offset); //Count is always 1
            _part3 = *(RWSD_NoteInfo*) list->Get(_offset, 0);

            if (_name == null)
            {
                _name = $"[{Index}]Data";
            }

            if (Parent.Parent.Children.Count > 1 && _part3._waveIndex < Parent.Parent.Children[1].Children.Count)
            {
                _soundNode = Parent.Parent.Children[1].Children[_part3._waveIndex] as RSARFileAudioNode;
            }

            SetSizeInternal(RWSD_DATAEntry.Size + RWSD_WSDEntry.Size + 0x20 + RWSD_NoteEvent.Size + 12 +
                            RWSD_NoteInfo.Size);

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return RWSD_DATAEntry.Size + RWSD_WSDEntry.Size + 0x20 + RWSD_NoteEvent.Size + 12 + RWSD_NoteInfo.Size;
        }

        public VoidPtr _baseAddr;

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            VoidPtr addr = address;

            RWSD_DATAEntry* header = (RWSD_DATAEntry*) addr;

            addr += RWSD_DATAEntry.Size;

            header->_wsdInfo = addr - _baseAddr;
            RWSD_WSDEntry* wsd = (RWSD_WSDEntry*) addr;
            *wsd = _part1;
            addr += RWSD_WSDEntry.Size;

            header->_trackTable = addr - _baseAddr;
            RuintList* list = (RuintList*) addr;
            addr += 12;

            list->_numEntries = 1;
            list->Entries[0] = addr - _baseAddr;

            ruint* r = (ruint*) addr;
            addr += 8;

            *r = addr - _baseAddr;

            RuintList* list2 = (RuintList*) addr;
            addr += 12;

            list2->_numEntries = 1;
            list2->Entries[0] = addr - _baseAddr;

            RWSD_NoteEvent* ev = (RWSD_NoteEvent*) addr;
            *ev = _part2;
            addr += RWSD_NoteEvent.Size;

            header->_noteTable = addr - _baseAddr;
            RuintList* list3 = (RuintList*) addr;
            addr += 12;

            list3->_numEntries = 1;
            list3->Entries[0] = addr - _baseAddr;

            RWSD_NoteInfo* info = (RWSD_NoteInfo*) addr;
            *info = _part3;
            addr += RWSD_NoteInfo.Size;
        }

        public override void Remove()
        {
            foreach (RSARSoundNode n in _refs)
            {
                n.SoundDataNode = null;
            }

            base.Remove();
        }

        internal void GetName()
        {
            string closestMatch = "";
            foreach (string s in References)
            {
                if (closestMatch == "")
                {
                    closestMatch = s;
                }
                else
                {
                    int one = closestMatch.Length;
                    int two = s.Length;
                    int min = Math.Min(one, two);
                    for (int i = 0; i < min; i++)
                    {
                        if (char.ToLower(s[i]) != char.ToLower(closestMatch[i]) && i > 1)
                        {
                            closestMatch = closestMatch.Substring(0, i - 1);
                            break;
                        }
                    }
                }
            }

            _name = $"{(string.IsNullOrEmpty(closestMatch) ? "[" + Index + "]Data" : closestMatch)}";
        }
    }
}