using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class BLOCNode : ARCEntryNode
    {
        public override ResourceType ResourceFileType => ResourceType.BLOC;
        internal BLOC* Header => (BLOC*) WorkingUncompressed.Address;

        public int Version
        {
            get => _version;
            set
            {
                _version = value;
                SignalPropertyChange();
            }
        }

        private int _version = 0x80;

        public int ExtParam
        {
            get => _extParam;
            set
            {
                _extParam = value;
                SignalPropertyChange();
            }
        }

        private int _extParam = 0;

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _version = Header->_version;
            _extParam = Header->_extParam;
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

                //Call NodeFactory on datasource to initiate various files
                if (NodeFactory.FromSource(this, source) == null)
                {
                    new BLOCEntryNode().Initialize(this, source);
                }
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = BLOC.Size + Children.Count * 4;
            foreach (ResourceNode node in Children)
            {
                size += node.CalculateSize(force);
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            BLOC* header = (BLOC*) address;
            *header = new BLOC();
            header->_tag = BLOC.Tag;
            header->_count = Children.Count;
            header->_version = Version;
            header->_extParam = ExtParam;

            uint offset = (uint) (0x10 + Children.Count * 4);
            for (int i = 0; i < Children.Count; i++)
            {
                if (i > 0)
                {
                    offset += (uint) Children[i - 1].CalculateSize(false);
                }

                *(buint*) (address + 0x10 + i * 4) = offset;
                _children[i].Rebuild(address + offset, _children[i].CalculateSize(false), true);
            }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((BLOC*) source.Address)->_tag == BLOC.Tag ? new BLOCNode() : null;
        }
    }

    public unsafe class BLOCEntryNode : ResourceNode
    {
        public override ResourceType ResourceFileType => ResourceType.Unknown;
        public int Entries { get; private set; }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            byte* _NumFiles = (byte*) WorkingUncompressed.Address + 0x07;
            if (_name == null)
            {
                _name = new string((sbyte*) WorkingUncompressed.Address);
            }

            Entries = *(int*) _NumFiles;
            return false;
        }
    }
}