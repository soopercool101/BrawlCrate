using System;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class MiscLedgeGrab : MovesetEntryNode
    {
        internal float _width, height;
        Vector2 _xy;

        [Category("LedgeGrab"), TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 XY { get { return _xy; } set { _xy = value; SignalPropertyChange(); } }
        [Category("LedgeGrab")]
        public float Height { get { return height; } set { height = value; SignalPropertyChange(); } }
        [Category("LedgeGrab")]
        public float Width { get { return _width; } set { _width = value; SignalPropertyChange(); } }

        protected override void OnParse(VoidPtr address)
        {
            sLedgegrab* hdr = (sLedgegrab*)address;
            _xy = hdr->_xy;
            _width = hdr->_width;
            height = hdr->_height;
        }

        protected override int OnGetSize() { return 0x10; }

        protected override void OnWrite(VoidPtr address)
        {
            RebuildAddress = address;
            sLedgegrab* header = (sLedgegrab*)address;
            header->_height = height;
            header->_width = _width;
            header->_xy = _xy;
        }
    }
}
