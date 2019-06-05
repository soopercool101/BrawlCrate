using System;
using System.ComponentModel;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class BGMGNode : ARCEntryNode
    {
        public override ResourceType ResourceFileType => ResourceType.BGMG;
        internal BGMG* Header => (BGMG*) WorkingUncompressed.Address;
        public int Entries { get; private set; }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            Entries = Header->_count;

            if (_name == null) _name = "BGMG";

            return Header->_count > 0;
        }

        public override void OnPopulate()
        {
            for (var i = 0; i < Header->_count; i++)
            {
                //source decleration
                DataSource source;

                //Enumerate datasources for each child node
                if (i == Header->_count - 1)
                    source = new DataSource((*Header)[i],
                        WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]);
                else
                    source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]);

                new BGMGEntryNode().Initialize(this, source);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            var size = BGMG.Size + Children.Count * 4;
            foreach (var node in Children) size += node.CalculateSize(force);

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var header = (BGMG*) address;
            *header = new BGMG(Children.Count);
            var offset = (uint) (0x10 + Children.Count * 4);
            for (var i = 0; i < Children.Count; i++)
            {
                if (i > 0) offset += (uint) Children[i - 1].CalculateSize(false);
                *(buint*) (address + 0x10 + i * 4) = offset;
                _children[i].Rebuild(address + offset, _children[i].CalculateSize(false), true);
            }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((BGMG*) source.Address)->_tag == BGMG.Tag ? new BGMGNode() : null;
        }
    }

    public unsafe class BGMGEntryNode : ResourceNode
    {
        private int _infoIndex;


        private string _stageID;


        private int _volume;
        internal BGMGEntry* Header => (BGMGEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;
        public int Entries { get; private set; }

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

        [Category("BGMG")]
        [DisplayName("Infoindex")]
        public int InfoIndex
        {
            get => _infoIndex;
            set
            {
                _infoIndex = value;
                SignalPropertyChange();
            }
        }

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

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _stageID = Header->StageID;
            _infoIndex = Header->_infoIndex;
            _volume = Header->_volume;

            if (_name == null) _name = string.Format("Song[{0}]", Index);

            return false;
        }
    }
}