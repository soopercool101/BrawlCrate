using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace;
using System;
#if !DEBUG
using System.ComponentModel;
#endif

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class BLOCNode : ARCEntryNode
    {
        public override ResourceType ResourceFileType => ResourceType.BLOC;
        internal BLOC* Header => (BLOC*) WorkingUncompressed.Address;

        public override Type[] AllowedChildTypes => new[] {typeof(BLOCEntryNode)};

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

        private int _extParam;

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
                if (NodeFactory.FromSource(this, source, false) == null)
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

                *(buint*) (address + BLOC.Size + i * 4) = offset;
                _children[i].Rebuild(address + offset, _children[i].CalculateSize(false), true);
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((BLOC*) source.Address)->_tag == BLOC.Tag ? new BLOCNode() : null;
        }
    }

    public unsafe class BLOCEntryNode : ResourceNode
    {
        internal BLOCEntry* Header => (BLOCEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.BLOCEntry;
        public override bool supportsCompression => false;

#if !DEBUG
        [Browsable(false)]
#endif
        public int Buffer { get; set; }
        private uint _rawTag;

        protected virtual string baseName => UncompressedSource.Tag;

        public override string Name
        {
            get => $"{baseName} [{Index}]";
            set => base.Name = value;
        }

        public virtual Type SubEntryType => typeof(RawDataNode);

        public override Type[] AllowedChildTypes =>
            SubEntryType == typeof(RawDataNode) ? new Type[] { } : new[] {SubEntryType};

        public override bool OnInitialize()
        {
            _rawTag = Header->_tag;

            int entries = Header->_count;
            // Get Buffer
            Buffer = 0;
            for (int i = 0; i < entries + Buffer; i++)
            {
                if (Header->Offsets(i) == 0)
                {
                    Buffer++;
                }
                else
                {
                    break;
                }
            }

            if (_name == null)
            {
                _name = UncompressedSource.Tag;
            }

            return entries > 0;
        }

        public override void OnPopulate()
        {
            int entries = Header->_count;
            for (int i = Buffer, j = 0; i < entries + Buffer; i++, j++)
            {
                //source decleration
                DataSource source;

                //Enumerate datasources for each child node
                if (i - Buffer == entries - 1)
                {
                    source = new DataSource((*Header)[i],
                        WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]);
                }
                else
                {
                    source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]);
                }

                //Call NodeFactory on datasource to initiate various files
                NodeFactory.FromSource(this, source, SubEntryType, false);
                if (Children[j]._name == null || Children[j]._name == "<null>")
                {
                    Children[j]._name = $"Entry [{j}]";
                }
            }
        }


        public override int OnCalculateSize(bool force)
        {
            int size = BLOCEntry.Size + Children.Count * 4;
            foreach (ResourceNode node in Children)
            {
                size += node.OnCalculateSize(force);
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            BLOCEntry* header = (BLOCEntry*) address;
            *header = new BLOCEntry();
            header->_tag = _rawTag;
            header->_count = Children.Count;

            uint offset = (uint) (BLOCEntry.Size + Children.Count * 4);
            for (int i = 0, j = 0; j < Children.Count; i++)
            {
                if (i < Buffer)
                {
                    *(buint*) (address + BLOCEntry.Size + i * 4) = 0;
                    offset += 4;
                    continue;
                }

                if (j > 0)
                {
                    offset += (uint) Children[j - 1].CalculateSize(false);
                }

                *(buint*) (address + BLOCEntry.Size + i * 4) = offset;
                _children[j].Rebuild(address + offset, _children[j].CalculateSize(false), true);
                j++;
            }
        }
    }
}