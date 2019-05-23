using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Ikarus.MovesetFile
{
    public unsafe class MiscMultiJump : MovesetEntryNode
    {
        internal float _unk1, _unk2, _unk3, _horizontalBoost, _turnFrames;
        internal List<float> _hops, _unks;

        [Category("MultiJump Attribute")]
        public float Unk1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("MultiJump Attribute")]
        public float Unk2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("MultiJump Attribute")]
        public float Unk3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }
        [Category("MultiJump Attribute")]
        public float HorizontalBoost { get { return _horizontalBoost; } set { _horizontalBoost = value; SignalPropertyChange(); } }
        [Category("MultiJump Attribute")]
        public float TurnFrames { get { return _turnFrames; } set { _turnFrames = value; SignalPropertyChange(); } }
        [Category("MultiJump Attribute")]
        public float[] Hops { get { return _hops.ToArray(); } set { _hops = value.ToList<float>(); SignalPropertyChange(); } }
        [Category("MultiJump Attribute")]
        public float[] Unks { get { return _unks.ToArray(); } set { _unks = value.ToList<float>(); SignalPropertyChange(); } }

        protected override void OnParse(VoidPtr address)
        {
            _unks = new List<float>();
            _hops = new List<float>();

            sMultiJump* hdr = (sMultiJump*)address;
            _unk1 = hdr->_unk1;
            _unk2 = hdr->_unk2;
            _unk3 = hdr->_unk3;
            _horizontalBoost = hdr->_horizontalBoost;

            if (hdr->hopFixed)
                _hops.Add(*(bfloat*)hdr->_hopListOffset.Address);
            else
            {
                bfloat* addr = (bfloat*)(BaseAddress + hdr->_hopListOffset);
                for (int i = 0; i < (_offset - hdr->_hopListOffset) / 4; i++)
                    _hops.Add(*addr++);
            }
            if (hdr->unkFixed)
                _unks.Add(*(bfloat*)hdr->_unkListOffset.Address);
            else
            {
                bfloat* addr = (bfloat*)(BaseAddress + hdr->_unkListOffset);
                for (int i = 0; i < ((hdr->hopFixed ? _offset : (int)hdr->_hopListOffset) - hdr->_unkListOffset) / 4; i++)
                    _unks.Add(*addr++);
            }
            if (hdr->turnFixed)
                _turnFrames = *(bfloat*)hdr->_turnFrameOffset.Address;
            else
                _turnFrames = hdr->_turnFrameOffset;
        }

        protected override int OnGetLookupCount()
        {
            return (_hops.Count > 0 ? 1 : 0) + (_unks.Count > 0 ? 1 : 0);
        }

        protected override int OnGetSize()
        {
            int size = 28;
            if (_hops.Count > 1)
                size += _hops.Count * 4;
            if (_unks.Count > 1)
                size += _unks.Count * 4;
            return size;
        }

        protected override void OnWrite(VoidPtr address)
        {
            int off = 0;
            if (_hops.Count > 1)
                off += _hops.Count * 4;
            if (_unks.Count > 1)
                off += _unks.Count * 4;

            sMultiJump* header = (sMultiJump*)(address + off);
            RebuildAddress = header;

            bfloat* addr = (bfloat*)address;

            if (_unks.Count > 1)
            {
                header->_unkListOffset = Offset(addr);
                if (header->_unkListOffset > 0)
                    Lookup(&header->_unkListOffset);

                foreach (float f in _unks)
                    *addr++ = f;
            }
            else if (_unks.Count == 1)
                *((bfloat*)&header->_unkListOffset) = _unks[0];
            else
                *((bfloat*)&header->_unkListOffset) = 0;

            if (_hops.Count > 1)
            {
                header->_hopListOffset = Offset(addr);
                if (header->_hopListOffset > 0)
                    Lookup(&header->_hopListOffset);

                foreach (float f in _hops)
                    *addr++ = f;
            }
            else if (_hops.Count == 1)
                *((bfloat*)&header->_hopListOffset) = _hops[0];
            else
                *((bfloat*)&header->_hopListOffset) = 0;

            header->_unk1 = _unk1;
            header->_unk2 = _unk2;
            header->_unk3 = _unk3;
            header->_horizontalBoost = _horizontalBoost;

            if (header->turnFixed)
                *(bfloat*)&header->_turnFrameOffset = _turnFrames;
            else
                header->_turnFrameOffset = (int)_turnFrames;
        }
    }
}
