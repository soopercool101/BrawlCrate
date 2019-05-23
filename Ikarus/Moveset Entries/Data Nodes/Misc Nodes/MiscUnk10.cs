using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class MiscUnk10 : MovesetEntryNode
    {
        public List<MiscUnk10Entry> _entries;

        int _haveNBoneIndex1, _haveNBoneIndex2, _haveNBoneIndex3, _throwNBoneIndex, _unkCount, _unkOffset, _pad;

        [Category("Bone References"), Browsable(true), TypeConverter(typeof(DropDownListBonesMDef))]
        public string HaveNBone1
        {
            get { return HaveNBoneNode1 == null ? _haveNBoneIndex1.ToString() : HaveNBoneNode1.Name; }
            set
            {
                if (Model == null)
                    _haveNBoneIndex1 = Convert.ToInt32(value);
                else
                    HaveNBoneNode1 = String.IsNullOrEmpty(value) ? HaveNBoneNode1 : Model.FindBone(value);
                SignalPropertyChange();
            }
        }
        [Category("Bone References"), Browsable(true), TypeConverter(typeof(DropDownListBonesMDef))]
        public string HaveNBone2
        {
            get { return HaveNBoneNode2 == null ? _haveNBoneIndex2.ToString() : HaveNBoneNode2.Name; }
            set
            {
                if (Model == null)
                    _haveNBoneIndex2 = Convert.ToInt32(value);
                else
                    HaveNBoneNode2 = String.IsNullOrEmpty(value) ? HaveNBoneNode2 : Model.FindBone(value);
                SignalPropertyChange();
            }
        }
        [Category("Bone References"), Browsable(true), TypeConverter(typeof(DropDownListBonesMDef))]
        public string ThrowNBone
        {
            get { return ThrowNBoneNode == null ? _throwNBoneIndex.ToString() : ThrowNBoneNode.Name; }
            set
            {
                if (Model == null)
                    _throwNBoneIndex = Convert.ToInt32(value);
                else
                    ThrowNBoneNode = String.IsNullOrEmpty(value) ? ThrowNBoneNode : Model.FindBone(value);
                SignalPropertyChange();
            }
        }
        [Category("Bone References"), Browsable(true), TypeConverter(typeof(DropDownListBonesMDef))]
        public string HaveNBone3
        {
            get { return HaveNBoneNode3 == null ? _haveNBoneIndex3.ToString() : HaveNBoneNode3.Name; }
            set
            {
                if (Model == null)
                    _haveNBoneIndex3 = Convert.ToInt32(value);
                else
                    HaveNBoneNode3 = String.IsNullOrEmpty(value) ? HaveNBoneNode3 : Model.FindBone(value);
                SignalPropertyChange();
            }
        }

        [Browsable(false)]
        public MDL0BoneNode HaveNBoneNode1
        {
            get { if (Model == null) return null; if (_haveNBoneIndex1 >= Model._linker.BoneCache.Length || _haveNBoneIndex1 < 0) return null; return (MDL0BoneNode)Model._linker.BoneCache[_haveNBoneIndex1]; }
            set { _haveNBoneIndex1 = value.BoneIndex; }
        }
        [Browsable(false)]
        public MDL0BoneNode HaveNBoneNode2
        {
            get { if (Model == null) return null; if (_haveNBoneIndex2 >= Model._linker.BoneCache.Length || _haveNBoneIndex2 < 0) return null; return (MDL0BoneNode)Model._linker.BoneCache[_haveNBoneIndex2]; }
            set { _haveNBoneIndex2 = value.BoneIndex; }
        }
        [Browsable(false)]
        public MDL0BoneNode ThrowNBoneNode
        {
            get { if (Model == null) return null; if (_throwNBoneIndex >= Model._linker.BoneCache.Length || _throwNBoneIndex < 0) return null; return (MDL0BoneNode)Model._linker.BoneCache[_throwNBoneIndex]; }
            set { _throwNBoneIndex = value.BoneIndex; }
        }
        [Browsable(false)]
        public MDL0BoneNode HaveNBoneNode3
        {
            get { if (Model == null) return null; if (_haveNBoneIndex3 >= Model._linker.BoneCache.Length || _haveNBoneIndex3 < 0) return null; return (MDL0BoneNode)Model._linker.BoneCache[_haveNBoneIndex3]; }
            set { _haveNBoneIndex3 = value.BoneIndex; }
        }

        protected override void OnParse(VoidPtr address)
        {
            _entries = new List<MiscUnk10Entry>();
            sMiscUnknown10* hdr = (sMiscUnknown10*)address;

            _haveNBoneIndex1 = hdr->_haveNBoneIndex1;
            _haveNBoneIndex2 = hdr->_haveNBoneIndex2;
            _throwNBoneIndex = hdr->_throwNBoneIndex;
            _unkCount = hdr->_list._startOffset;
            _unkOffset = hdr->_list._listCount;
            _pad = hdr->_pad;
            _haveNBoneIndex3 = hdr->_haveNBoneIndex3;

            //if ((_offset - _unkOffset) / 16 != _unkCount)
            //    throw new Exception("Count is incorrect");

            for (int i = 0; i < _unkCount; i++)
                _entries.Add(Parse<MiscUnk10Entry>(_unkOffset + i * 0x10));
        }

        protected override int OnGetLookupCount()
        {
            return _entries.Count > 0 ? 1 : 0;
        }

        protected override int OnGetSize()
        {
            return 28 + _entries.Count * 0x10;
        }

        protected override void OnWrite(VoidPtr address)
        {
            sMiscUnknown10Entry* data = (sMiscUnknown10Entry*)address;
            foreach (MiscUnk10Entry e in _entries)
                e.Write(data++);

            RebuildAddress = data;

            sMiscUnknown10* header = (sMiscUnknown10*)data;
            header->_haveNBoneIndex1 = _haveNBoneIndex1;
            header->_haveNBoneIndex2 = _haveNBoneIndex2;
            header->_throwNBoneIndex = _throwNBoneIndex;
            header->_pad = _pad;
            header->_haveNBoneIndex3 = _haveNBoneIndex3;

            //Values are switched on purpose
            header->_list._startOffset = _entries.Count;
            header->_list._listCount = _entries.Count > 0 ? Offset(address) : 0;

            if (header->_list._listCount > 0)
                Lookup(&header->_list._listCount);
        }
    }

    public unsafe class MiscUnk10Entry : MovesetEntryNode
    {
        int _unk1, _unk2, _pad1, _pad2;

        [Category("Unk Section 3 Entry")]
        public int Unk1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("Unk Section 3 Entry")]
        public int Unk2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("Unk Section 3 Entry")]
        public int Pad1 { get { return _pad1; } }
        [Category("Unk Section 3 Entry")]
        public int Pad2 { get { return _pad2; } }

        protected override void OnParse(VoidPtr address)
        {
            sMiscUnknown10Entry* hdr = (sMiscUnknown10Entry*)address;
            _unk1 = hdr->_unk1;
            _unk2 = hdr->_unk2;
            _pad1 = hdr->_pad1;
            _pad2 = hdr->_pad2;
        }

        protected override int OnGetSize() { return 16; }

        protected override void OnWrite(VoidPtr address)
        {
            RebuildAddress = address;
            sMiscUnknown10Entry* header = (sMiscUnknown10Entry*)address;
            header->_unk1 = _unk1;
            header->_unk2 = _unk2;
            header->_pad1 = _pad1;
            header->_pad2 = _pad2;
        }
    }
}
