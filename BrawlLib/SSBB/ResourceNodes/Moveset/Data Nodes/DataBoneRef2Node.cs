using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefBoneRef2Node : MoveDefEntryNode
    {
        internal FDefBoneRef2* Header => (FDefBoneRef2*) WorkingUncompressed.Address;

        private int _handNBoneIndex1, _handNBoneIndex2, _handNBoneIndex3, _handNBoneIndex4;

        [Category("Bone References")]
        public int HandNBoneIndex1
        {
            get => _handNBoneIndex1;
            set
            {
                _handNBoneIndex1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Bone References")]
        public int HandNBoneIndex2
        {
            get => _handNBoneIndex2;
            set
            {
                _handNBoneIndex2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Bone References")]
        public int HandNBoneIndex3
        {
            get => _handNBoneIndex3;
            set
            {
                _handNBoneIndex3 = value;
                SignalPropertyChange();
            }
        }

        [Category("Bone References")]
        public int HandNBoneIndex4
        {
            get => _handNBoneIndex4;
            set
            {
                _handNBoneIndex4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Bone References")] public int EntryOffset => Header->_offset;

        [Category("Bone References")] public int EntryCount => Header->_count;

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Hand Bones";

            _handNBoneIndex1 = Header->_handNBoneIndex1;
            _handNBoneIndex2 = Header->_handNBoneIndex2;
            _handNBoneIndex3 = Header->_handNBoneIndex3;
            _handNBoneIndex4 = Header->_handNBoneIndex4;

            return EntryOffset > 0;
        }

        public override void OnPopulate()
        {
            bint* addr = (bint*) (BaseAddress + EntryOffset);
            for (int i = 0; i < EntryCount; i++)
            {
                new MoveDefBoneIndexNode().Initialize(this, addr++, 4);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            _lookupCount = Children.Count > 0 ? 1 : 0;
            return 24 + Children.Count * 4;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            bint* addr = (bint*) address;

            foreach (MoveDefBoneIndexNode b in Children)
            {
                b.Rebuild(addr++, 4, true);
            }

            _entryOffset = addr;

            FDefBoneRef2* header = (FDefBoneRef2*) addr;
            header->_handNBoneIndex1 = HandNBoneIndex1;
            header->_handNBoneIndex2 = HandNBoneIndex2;
            header->_handNBoneIndex3 = HandNBoneIndex3;
            header->_handNBoneIndex4 = HandNBoneIndex4;
            header->_count = Children.Count;

            if (Children.Count > 0)
            {
                header->_offset = (int) address - (int) _rebuildBase;
                _lookupOffsets.Add((int) header->_offset.Address - (int) _rebuildBase);
            }
        }
    }
}