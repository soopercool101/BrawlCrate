using BrawlLib.SSBBTypes;
using System;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class CmnUnknown11Node : MovesetEntryNode
    {
        EntryList<Unknown11EntryNode> _entries;

        protected override void OnParse(VoidPtr address)
        {
            sListOffset* offset = (sListOffset*)address;
            _entries = Parse<EntryList<Unknown11EntryNode>>(offset->_startOffset, offset->_listCount);
        }

        protected override int OnGetSize()
        {
            return sListOffset.Size + _entries.GetSize();
        }

        protected override void OnWrite(VoidPtr address)
        {
            
        }

        //protected override int OnCalculateSize(bool force)
        //{
        //    _lookupCount = _entries.GetLookupCount();
        //    _entryLength = sListOffset.Size;
        //    _childLength = 0;
        //    foreach (MoveDefSectionParamNode p in Children)
        //        _childLength += p.CalculateSize(true);
        //    return _entryLength + _childLength;
        //}

        //protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        //{
        //    VoidPtr addr = address;
        //    foreach (MoveDefSectionParamNode p in Children)
        //    {
        //        p.Rebuild(addr, p._calcSize, true);
        //        addr += p._calcSize;
        //    }
        //    _entryOffset = addr;
        //    FDefListOffset* header = (FDefListOffset*)addr;
        //    if (Children.Count > 0)
        //    {
        //        header->_startOffset = (int)address - (int)_rebuildBase;
        //        _lookupOffsets.Add((int)header->_startOffset.Address - (int)_rebuildBase);
        //    }
        //    header->_listCount = Children.Count;
        //}
    }

    public unsafe class Unknown11EntryNode : MovesetEntryNode
    {
        public int _unknown;

        [Category("List Offset")]
        public int Unknown { get { return _unknown; } set { _unknown = value; SignalPropertyChange(); } }

        public EntryList<IndexValue> _entries;

        protected override void OnParse(VoidPtr address)
        {
            sCommonUnknown11Entry* hdr = (sCommonUnknown11Entry*)address;
            _unknown = hdr->_unk1;
            _entries = Parse<EntryList<IndexValue>>(hdr->_list._startOffset, 4, (int)hdr->_list._listCount);
        }

        protected override int OnGetSize()
        {
            return sCommonUnknown11Entry.Size + _entries.Count * 4;
        }

        protected override void OnWrite(VoidPtr address)
        {
            bint* value = (bint*)address;
            foreach (IndexValue v in _entries)
                v.Write(value++);
            sCommonUnknown11Entry* hdr = (sCommonUnknown11Entry*)value;
            hdr->_unk1 = _unknown;
            hdr->_list._startOffset = Offset(address);
            hdr->_list._listCount = _entries.Count;
        }
    }
}
