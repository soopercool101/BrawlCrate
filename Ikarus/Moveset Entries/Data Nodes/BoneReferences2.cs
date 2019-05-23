using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class BoneReferences2 : MovesetEntryNode
    {
        public List<BoneIndexValue> _bones;
        private int _handNBoneIndex1, _handNBoneIndex2, _handNBoneIndex3, _handNBoneIndex4, _dataOffset, _count;

        [Category("Bone References 2")]
        public int HandNBoneIndex1 { get { return _handNBoneIndex1; } set { _handNBoneIndex1 = value; SignalPropertyChange(); } }
        [Category("Bone References 2")]
        public int HandNBoneIndex2 { get { return _handNBoneIndex2; } set { _handNBoneIndex2 = value; SignalPropertyChange(); } }
        [Category("Bone References 2")]
        public int HandNBoneIndex3 { get { return _handNBoneIndex3; } set { _handNBoneIndex3 = value; SignalPropertyChange(); } }
        [Category("Bone References 2")]
        public int HandNBoneIndex4 { get { return _handNBoneIndex4; } set { _handNBoneIndex4 = value; SignalPropertyChange(); } }
        [Category("Bone References 2")]
        public int EntryOffset { get { return _dataOffset; } }
        [Category("Bone References 2")]
        public int EntryCount { get { return _count; } }

        protected override void OnParse(VoidPtr address)
        {
            _bones = new List<BoneIndexValue>();

            sDataBoneRef2* hdr = (sDataBoneRef2*)address;
            _handNBoneIndex1 = hdr->_handNBoneIndex1;
            _handNBoneIndex2 = hdr->_handNBoneIndex2;
            _handNBoneIndex3 = hdr->_handNBoneIndex3;
            _handNBoneIndex4 = hdr->_handNBoneIndex4;

            _dataOffset = hdr->_offset;
            _count = hdr->_count;

            for (int i = 0; i < EntryCount; i++)
                _bones.Add(Parse<BoneIndexValue>(EntryOffset + i * 4));
        }

        protected override int OnGetLookupCount() { return _bones.Count > 0 ? 1 : 0; }
        protected override int OnGetSize() { return 24 + _bones.Count * 4; }
        protected override void OnWrite(VoidPtr address)
        {
            bint* addr = (bint*)address;

            foreach (BoneIndexValue b in _bones)
                b.Write(addr++);

            RebuildAddress = addr;

            sDataBoneRef2* header = (sDataBoneRef2*)addr;
            header->_handNBoneIndex1 = HandNBoneIndex1;
            header->_handNBoneIndex2 = HandNBoneIndex2;
            header->_handNBoneIndex3 = HandNBoneIndex3;
            header->_handNBoneIndex4 = HandNBoneIndex4;
            header->_count = _bones.Count;

            if (_bones.Count > 0)
            {
                header->_offset = Offset(address);
                Lookup(&header->_offset);
            }
        }
    }
}
