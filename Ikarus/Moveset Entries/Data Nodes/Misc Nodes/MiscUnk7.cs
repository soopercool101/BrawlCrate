using System;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class MiscUnk7Entry : MovesetEntryNode
    {
        sMiscUnknown7 hdr;

        [Category("Misc Unknown 7 Entry")]
        public byte Unknown1 { get { return hdr.unk1; } set { hdr.unk1 = value; SignalPropertyChange(); } }
        [Category("Misc Unknown 7 Entry")]
        public byte Unknown2 { get { return hdr.unk2; } set { hdr.unk2 = value; SignalPropertyChange(); } }
        [Category("Misc Unknown 7 Entry")]
        public byte Unknown3 { get { return hdr.unk3; } set { hdr.unk3 = value; SignalPropertyChange(); } }
        [Category("Misc Unknown 7 Entry")]
        public byte Unknown4 { get { return hdr.unk4; } set { hdr.unk4 = value; SignalPropertyChange(); } }

        [Category("Misc Unknown 7 Entry")]
        public byte Unknown5 { get { return hdr.unk5; } set { hdr.unk5 = value; SignalPropertyChange(); } }
        [Category("Misc Unknown 7 Entry")]
        public byte Unknown6 { get { return hdr.unk6; } set { hdr.unk6 = value; SignalPropertyChange(); } }
        [Category("Misc Unknown 7 Entry")]
        public byte Unknown7 { get { return hdr.unk7; } set { hdr.unk7 = value; SignalPropertyChange(); } }
        [Category("Misc Unknown 7 Entry")]
        public byte Unknown8 { get { return hdr.unk8; } set { hdr.unk8 = value; SignalPropertyChange(); } }

        [Category("Misc Unknown 7 Entry")]
        public float Unknown9 { get { return hdr.unk9; } set { hdr.unk9 = value; SignalPropertyChange(); } }
        [Category("Misc Unknown 7 Entry")]
        public float Unknown10 { get { return hdr.unk10; } set { hdr.unk10 = value; SignalPropertyChange(); } }

        [Category("Misc Unknown 7 Entry")]
        public float Unknown11 { get { return hdr.unk11; } set { hdr.unk11 = value; SignalPropertyChange(); } }
        [Category("Misc Unknown 7 Entry")]
        public float Unknown12 { get { return hdr.unk12; } set { hdr.unk12 = value; SignalPropertyChange(); } }
        [Category("Misc Unknown 7 Entry")]
        public float Unknown13 { get { return hdr.unk13; } set { hdr.unk13 = value; SignalPropertyChange(); } }
        [Category("Misc Unknown 7 Entry")]
        public float Unknown14 { get { return hdr.unk14; } set { hdr.unk14 = value; SignalPropertyChange(); } }

        protected override void OnParse(VoidPtr address)
        {
            hdr = *(sMiscUnknown7*)address;
        }
        protected override int OnGetSize() { return 32; }
        protected override void OnWrite(VoidPtr address)
        {
            *(sMiscUnknown7*)(RebuildAddress = address) = hdr;
        }
    }
}
