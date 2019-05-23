using System;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class MiscUnk12 : MovesetEntryNode
    {
        int _unk1, _unk2, _unk3, _unk4;

        [Category("Misc Section 5")]
        public int Unk1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("Misc Section 5")]
        public int Unk2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("Misc Section 5")]
        public int Unk3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }
        [Category("Misc Section 5")]
        public int Unk4 { get { return _unk4; } set { _unk4 = value; SignalPropertyChange(); } }

        protected override void OnParse(VoidPtr address)
        {
            sMiscUnknown12* hdr = (sMiscUnknown12*)address;
            _unk1 = hdr->_unk1;
            _unk2 = hdr->_unk2;
            _unk3 = hdr->_unk3;
            _unk4 = hdr->_unk4;
        }

        protected override int OnGetSize() { return 16; }
        protected override void OnWrite(VoidPtr address)
        {
            RebuildAddress = address;
            sMiscUnknown12* header = (sMiscUnknown12*)address;
            header->_unk1 = _unk1;
            header->_unk2 = _unk2;
            header->_unk3 = _unk3;
            header->_unk4 = _unk4;
        }
    }
}
