using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Stage_Tables;
using System;

namespace BrawlLib.SSBB.ResourceNodes
{
    public abstract unsafe class TBNode : ARCEntryNode
    {
        internal TB* Header => (TB*) WorkingUncompressed.Address;

        private uint _rawTag;

        protected override string GetName()
        {
            return GetName(UncompressedSource.Tag);
        }

        private int _unknown0x8;
        public int Unknown0x8
        {
            get => _unknown0x8;
            set
            {
                _unknown0x8 = value;
                SignalPropertyChange();
            }
        }

        private int _unknown0xC;
        public int Unknown0xC
        {
            get => _unknown0xC;
            set
            {
                _unknown0xC = value;
                SignalPropertyChange();
            }
        }

        public virtual Type SubEntryType => typeof(RawDataNode);

        public override Type[] AllowedChildTypes =>
            SubEntryType == typeof(RawDataNode) ? new Type[] { } : new[] {SubEntryType};

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _rawTag = Header->_tag;

            int entries = Header->_count;

            _unknown0x8 = Header->_pad0x8;
            _unknown0xC = Header->_pad0xC;

            return entries > 0;
        }

        public override void OnPopulate()
        {
            int entries = Header->_count;
            for (int i = 0, j = 0; i < entries; i++, j++)
            {
                //source decleration
                DataSource source;

                //Enumerate datasources for each child node
                if (i == entries - 1)
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
            int size = TB.Size + Children.Count * 4;
            foreach (ResourceNode node in Children)
            {
                size += node.OnCalculateSize(force);
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TB* header = (TB*) address;
            *header = new TB();
            header->_tag = _rawTag;
            header->_count = Children.Count;
            header->_pad0x8 = 0;
            header->_pad0xC = 0;

            uint offset = (uint) (TB.Size + Children.Count * 4);
            for (int i = 0, j = 0; j < Children.Count; i++)
            {
                if (j > 0)
                {
                    offset += (uint) Children[j - 1].CalculateSize(false);
                }

                *(buint*) (address + TB.Size + i * 4) = offset;
                _children[j].Rebuild(address + offset, _children[j].CalculateSize(false), true);
                j++;
            }
        }
    }
}
