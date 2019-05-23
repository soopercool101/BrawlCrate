using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class BGMGNode : ARCEntryNode
    {
        public override ResourceType ResourceType { get { return ResourceType.BGMG; } }
        internal BGMG* Header { get { return (BGMG*)WorkingUncompressed.Address; } }

        private int _entries;
        public int Entries { get { return _entries; } }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _entries = Header->_count;

            if (_name == null)
                _name = "BGMG";

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
                    source = new DataSource((*Header)[i], WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]);
                else { source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]); }

                new BGMGEntryNode().Initialize(this, source);

            }
        }
        public override int OnCalculateSize(bool force)
        {
            int size = BGMG.Size + (Children.Count * 4);
            foreach (ResourceNode node in Children)
                size += node.CalculateSize(force);
            return size;
        }
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            BGMG* header = (BGMG*)address;
            *header = new BGMG(Children.Count);
            uint offset = (uint)(0x10 + (Children.Count * 4));
            for (int i = 0; i < Children.Count; i++)
            {
                if (i > 0) { offset += (uint)(Children[i - 1].CalculateSize(false)); }
                *(buint*)((VoidPtr)address + 0x10 + i * 4) = offset;
                _children[i].Rebuild((VoidPtr)address + offset, _children[i].CalculateSize(false), true);
            }
        }

        internal static ResourceNode TryParse(DataSource source) { return ((BGMG*)source.Address)->_tag == BGMG.Tag ? new BGMGNode() : null; }
    }

    public unsafe class BGMGEntryNode : ResourceNode
    {
        internal BGMGEntry* Header { get { return (BGMGEntry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }
        public int Entries { get; private set; }


        private string _stageID;
        [Category("BGMG")]
        [DisplayName("Stage ID")]
        public string StageID { get { return _stageID; } set { _stageID = value; SignalPropertyChange(); } }


        private int _infoIndex;
        [Category("BGMG")]
        [DisplayName("Infoindex")]
        public int InfoIndex { get { return _infoIndex; } set { _infoIndex = value; SignalPropertyChange(); } }


        private int _volume;
        [Category("BGMG")]
        [DisplayName("Volume")]
        public int Volume { get { return _volume; } set { _volume = value; SignalPropertyChange(); } }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _stageID = Header->StageID;
            _infoIndex = Header->_infoIndex;
            _volume = Header->_volume;

            if (_name == null)
                _name = String.Format("Song[{0}]", Index);

            return false;
        }
    }
}
