using BrawlLib.Internal;
using BrawlLib.SSBB.Types;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefUnk24Node : MoveDefEntryNode
    {
        internal FDefListOffset* Header => (FDefListOffset*) WorkingUncompressed.Address;
        internal int i = 0;

        public int DataOffset => Header->_startOffset;
        public int Count => Header->_listCount;

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
            {
                _name = "Unknown 24";
            }

            return Count > 0;
        }

        public override void OnPopulate()
        {
            bint* entry = (bint*) (BaseAddress + DataOffset);
            for (int i = 0; i < Count; i++)
            {
                new MoveDefIndexNode().Initialize(this, new DataSource((VoidPtr) entry++, 4));
            }
        }

        public override int OnCalculateSize(bool force)
        {
            _lookupCount = Children.Count > 0 ? 1 : 0;
            return Children.Count * 4 + 8;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            bint* addr = (bint*) address;
            foreach (MoveDefIndexNode b in Children)
            {
                b._entryOffset = addr;
                *addr++ = b.ItemIndex;
            }

            FDefListOffset* header = (FDefListOffset*) addr;

            _entryOffset = header;

            if (Children.Count > 0)
            {
                header->_startOffset = (int) address - (int) _rebuildBase;
                _lookupOffsets.Add((int) header->_startOffset.Address - (int) _rebuildBase);
            }

            header->_listCount = Children.Count;
        }
    }
}