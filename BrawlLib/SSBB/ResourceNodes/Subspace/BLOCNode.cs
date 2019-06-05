using System;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class BLOCNode : ARCEntryNode
    {
        public override ResourceType ResourceFileType => ResourceType.BLOC;
        internal BLOC* Header => (BLOC*) WorkingUncompressed.Address;

        public override bool OnInitialize()
        {
            base.OnInitialize();
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

                //Call NodeFactory on datasource to initiate various files
                if (NodeFactory.FromSource(this, source) == null) new BLOCEntryNode().Initialize(this, source);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            var size = BLOC.Size + Children.Count * 4;
            foreach (var node in Children) size += node.CalculateSize(force);

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var header = (BLOC*) address;
            *header = new BLOC(Children.Count);
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
            return ((BLOC*) source.Address)->_tag == BLOC.Tag ? new BLOCNode() : null;
        }
    }

    public unsafe class BLOCEntryNode : ResourceNode
    {
        internal BLOCEntry* Header => (BLOCEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;
        public int Entries { get; private set; }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            var _NumFiles = (byte*) WorkingUncompressed.Address + 0x07;
            if (_name == null) _name = new string((sbyte*) Header);

            Entries = *(int*) _NumFiles;
            return false;
        }
    }
}