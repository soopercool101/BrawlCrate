using System;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class MiscTether : MovesetEntryNode
    {
        internal int _numHangFrame = 0;
        internal float _unknown;

        [Category("Tether")]
        public int HangFrameCount { get { return _numHangFrame; } set { _numHangFrame = value; SignalPropertyChange(); } }
        [Category("Tether")]
        public float Unknown { get { return _unknown; } set { _unknown = value; SignalPropertyChange(); } }

        protected override void OnParse(VoidPtr address)
        {
            sTether* hdr = (sTether*)address;
            _numHangFrame = hdr->_numHangFrame;
            _unknown = hdr->_unk1;
        }

        protected override int OnGetSize() { return 8; }

        protected override void OnWrite(VoidPtr address)
        {
            sTether* header = (sTether*)(RebuildAddress = address);
            header->_numHangFrame = _numHangFrame;
            header->_unk1 = _unknown;
        }
    }
}