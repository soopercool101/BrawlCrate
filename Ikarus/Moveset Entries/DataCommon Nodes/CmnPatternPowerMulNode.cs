using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class CmnPatternPowerMulNode : MovesetEntryNode
    {
        public List<Script> _scripts;

        public float _unk1;
        public float _unk2;
        public float _unk3;
        public float _unk4;
        public float _unk5;
        public float _unk6;
        public float _unk7;
        public float _unk8;
        public float _unk9;
        public float _unk10;

        [Category("Pattern Power Mul")]
        public float Unknown1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("Pattern Power Mul")]
        public float Unknown2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("Pattern Power Mul")]
        public float Unknown3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }
        [Category("Pattern Power Mul")]
        public float Unknown4 { get { return _unk4; } set { _unk4 = value; SignalPropertyChange(); } }
        [Category("Pattern Power Mul")]
        public float Unknown5 { get { return _unk5; } set { _unk5 = value; SignalPropertyChange(); } }
        [Category("Pattern Power Mul")]
        public float Unknown6 { get { return _unk6; } set { _unk6 = value; SignalPropertyChange(); } }
        [Category("Pattern Power Mul")]
        public float Unknown7 { get { return _unk7; } set { _unk7 = value; SignalPropertyChange(); } }
        [Category("Pattern Power Mul")]
        public float Unknown8 { get { return _unk8; } set { _unk8 = value; SignalPropertyChange(); } }
        [Category("Pattern Power Mul")]
        public float Unknown9 { get { return _unk9; } set { _unk9 = value; SignalPropertyChange(); } }
        [Category("Pattern Power Mul")]
        public float Unknown10 { get { return _unk10; } set { _unk10 = value; SignalPropertyChange(); } }

        protected override void OnParse(VoidPtr address)
        {
            sPatternPowerMul* hdr = (sPatternPowerMul*)address;
            _unk1 = hdr->_unk1;
            _unk2 = hdr->_unk2;
            _unk3 = hdr->_unk3;
            _unk4 = hdr->_unk4;
            _unk5 = hdr->_unk5;
            _unk6 = hdr->_unk6;
            _unk7 = hdr->_unk7;
            _unk8 = hdr->_unk8;
            _unk9 = hdr->_unk9;
            _unk10 = hdr->_unk10;

            Script prev;
            VoidPtr addr = &hdr->_first;

            _scripts = new List<Script>();

            //Event parameters for events in this node are built elsewhere
            _scripts.Add(prev = Parse<Script>(addr));
            for (int i = 0; i < 3; i++)
            {
                addr += prev.Count * 8 + 8;
                _scripts.Add(prev = Parse<Script>(addr));
            }
        }

        protected override int OnGetLookupCount()
        {
            int i = 0;
            foreach (Script p in _scripts)
                i += p.GetLookupCount();
            return i;
        }

        protected override int OnGetSize()
        {
            _entryLength = 40;
            _childLength = 0;
            foreach (Script p in _scripts)
                _childLength += p.GetSize();
            return _entryLength + _childLength;
        }

        protected override void OnWrite(VoidPtr address)
        {
            VoidPtr addr = address;
            RebuildAddress = addr;
            sPatternPowerMul* header = (sPatternPowerMul*)addr;
            header->_unk1 = _unk1;
            header->_unk2 = _unk2;
            header->_unk3 = _unk3;
            header->_unk4 = _unk4;
            header->_unk5 = _unk5;
            header->_unk6 = _unk6;
            header->_unk7 = _unk7;
            header->_unk8 = _unk8;
            header->_unk9 = _unk9;
            header->_unk10 = _unk10;
            foreach (Script p in _scripts)
            {
                p.Write(addr);
                Lookup(p.LookupAddresses);
                addr += p._calcSize;
            }
        }
    }

    //public unsafe class MoveDefPatternPowerMulEntryNode : MovesetEntry
    //{
    //    internal bint* Header { get { return (bint*)WorkingUncompressed.Address; } }

    //    public override bool OnInitialize()
    //    {
    //        base.OnInitialize();
    //        return true;
    //    }

    //    public override void Parse(VoidPtr address)
    //    {
    //        new ActionScript("1", false, this).Initialize(this, &Header[0], 8);
    //        new ActionScript("2", false, this).Initialize(this, &Header[2], 8);
    //        new ActionScript("3", false, this).Initialize(this, &Header[4], 8);
    //    }

    //    public override int GetSize()
    //    {
    //        _lookupCount = (Children.Count > 0 ? 1 : 0);
    //        _entryLength = 8;
    //        _childLength = 0;
    //        foreach (RawParamList p in Children)
    //            _childLength += p.CalculateSize(true);
    //        return _entryLength + _childLength;
    //    }

    //    protected override void Write(VoidPtr address)
    //    {
    //        VoidPtr addr = address;
    //        foreach (RawParamList p in Children)
    //        {
    //            p.Rebuild(addr, p._calcSize, true);
    //            addr += p._calcSize;
    //        }
    //        _rebuildAddr = addr;
    //        FDefListOffset* header = (FDefListOffset*)addr;
    //        if (Children.Count > 0)
    //        {
    //            header->_startOffset = (int)address - (int)RebuildBase;
    //            _lookupOffsets.Add(header->_startOffset.Address);
    //        }
    //        header->_listCount = Children.Count;
    //    }
    //}
}
