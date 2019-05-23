using System;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class Unknown22 : MovesetEntryNode
    {
        int _unk1, _unk2, _actionOffset;
        public Script _script;

        [Category("Unknown Offset 22")]
        public int Unknown1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("Unknown Offset 22")]
        public int Unknown2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("Unknown Offset 22")]
        public int ActionOffset { get { return _actionOffset; } }

        protected override void OnParse(VoidPtr address)
        {
            sDataUnknown22* hdr = (sDataUnknown22*)address;
            _unk1 = hdr->_unk1;
            _unk2 = hdr->_unk2;
            _actionOffset = hdr->_actionOffset;

            if (_actionOffset > 0)
                _script = Parse<Script>(_actionOffset);
        }

        protected override int OnGetLookupCount()
        {
            //Script lookup count is retrieved separately
            return _script != null && _script.Count > 0 ? 1 : 0;
        }

        protected override int OnGetSize()
        {
            //Get script size separately
            return 12;
        }

        protected override void OnWrite(VoidPtr address)
        {
            RebuildAddress = address;

            sDataUnknown22* data = (sDataUnknown22*)address;
            data->_unk1 = _unk1;
            data->_unk2 = _unk2;

            if (_script != null && _script.Count > 0)
            {
                data->_actionOffset = Offset(_script.RebuildAddress);
                Lookup(&data->_actionOffset);
            }
        }
    }
}