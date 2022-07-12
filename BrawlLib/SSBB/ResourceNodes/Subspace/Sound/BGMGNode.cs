using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Sound;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class BGMGNode : ARCEntryNode
    {
        public override ResourceType ResourceFileType => ResourceType.BGMG;
        internal BGMG* Header => (BGMG*) WorkingUncompressed.Address;

        protected override string GetName()
        {
            return base.GetName("BGMG");
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            return Header->_count > 0;
        }


        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                //source decleration
                DataSource source;

                //Enumerate datasources for each child node
                if (i == Header->_count - 1)
                {
                    source = new DataSource((*Header)[i],
                        WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]);
                }
                else
                {
                    source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]);
                }

                new BGMGEntryNode().Initialize(this, source);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = BGMG.Size + Children.Count * 4;
            foreach (ResourceNode node in Children)
            {
                size += node.CalculateSize(force);
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            BGMG* header = (BGMG*) address;
            *header = new BGMG(Children.Count);
            uint offset = (uint) (0x10 + Children.Count * 4);
            for (int i = 0; i < Children.Count; i++)
            {
                int size = _children[i].CalculateSize(true);

                *(buint*) (address + 0x10 + i * 4) = offset;
                _children[i].Rebuild(address + offset, size, true);
                offset += (uint)size;
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((BGMG*) source.Address)->_tag == BGMG.Tag ? new BGMGNode() : null;
        }
    }

    public unsafe class BGMGEntryNode : ResourceNode
    {
        internal BGMGEntry* Header => (BGMGEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        private string _stageID;

        [Category("BGMG")]
        [DisplayName("Stage ID")]
        public string StageID
        {
            get => _stageID;
            set
            {
                _stageID = value;
                SignalPropertyChange();
            }
        }


        private int _songID;

        [Category("BGMG")]
        [DisplayName("Song ID")]
        [TypeConverter(typeof(HexIntConverter))]
        public int SongID
        {
            get => _songID;
            set
            {
                _songID = value;
                SignalPropertyChange();
            }
        }


        private int _volume;

        [Category("BGMG")]
        [DisplayName("Volume")]
        public int Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                SignalPropertyChange();
            }
        }

        private float _playOffsetFrame;

        [Category("BGMG")]
        [DisplayName("Play Offset Frame")]
        public float PlayOffsetFrame
        {
            get => _playOffsetFrame;
            set
            {
                _playOffsetFrame = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _stageID = Header->StageID;
            _songID = Header->_songID;
            _volume = Header->_volume;
            _playOffsetFrame = Header->_playOffsetFrame;

            if (_name == null)
            {
                _name = $"Song [{Index}]";
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return BGMGEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            BGMGEntry* header = (BGMGEntry*)address;
            *header = new BGMGEntry(StageID, SongID, Volume, PlayOffsetFrame);
        }
    }
}