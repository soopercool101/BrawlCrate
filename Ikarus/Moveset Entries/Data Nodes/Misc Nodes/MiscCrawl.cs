using System;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class MiscCrawl : MovesetEntryNode
    {
        internal float forward, backward;

        [Category("Crawl Acceleration")]
        public float Forward { get { return forward; } set { forward = value; SignalPropertyChange(); } }
        [Category("Crawl Acceleration")]
        public float Backward { get { return backward; } set { backward = value; SignalPropertyChange(); } }

        protected override void OnParse(VoidPtr address)
        {
            sCrawl* hdr = (sCrawl*)address;
            forward = hdr->_forward;
            backward = hdr->_backward;
        }

        protected override int OnGetSize() { return 8; }

        protected override void OnWrite(VoidPtr address)
        {
            RebuildAddress = address;
            sCrawl* header = (sCrawl*)address;
            header->_forward = forward;
            header->_backward = backward;
        }
    }
}
