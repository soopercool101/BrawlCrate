using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class AnimParamSection : TableEntryNode
    {
        AnimParamHeader hdr;

        [Category("Data Offsets")]
        public int SubactionFlags { get { return hdr.SubactionFlags; } }
        [Category("Data Offsets")]
        public int SubactionFlagsCount { get { return hdr.SubactionFlagsCount; } }
        [Category("Data Offsets")]
        public int ActionFlags { get { return hdr.ActionFlags; } }
        [Category("Data Offsets")]
        public int ActionFlagsCount { get { return hdr.ActionFlagsCount; } }
        [Category("Data Offsets")]
        public int Unk4 { get { return hdr.Unknown4; } }
        [Category("Data Offsets")]
        public int Unk5 { get { return hdr.Unknown5; } }
        [Category("Data Offsets")]
        public int Unk6 { get { return hdr.Unknown6; } }
        [Category("Data Offsets")]
        public int Unk7 { get { return hdr.Unknown7; } }
        [Category("Data Offsets")]
        public int Unk8 { get { return hdr.Unknown8; } }
        [Category("Data Offsets")]
        public int Unk9 { get { return hdr.Unknown9; } }
        [Category("Data Offsets")]
        public int Unk10 { get { return hdr.Unknown10; } }
        [Category("Data Offsets")]
        public int Unk11 { get { return hdr.Unknown11; } }
        [Category("Data Offsets")]
        public int HitData { get { return hdr.Hurtboxes; } }
        [Category("Data Offsets")]
        public int Unk13 { get { return hdr.Unknown13; } }
        [Category("Data Offsets")]
        public int CollisionData { get { return hdr.CollisionData; } }
        [Category("Data Offsets")]
        public int Unk15 { get { return hdr.Unknown15; } }

        protected override void OnParse(VoidPtr address)
        {
            AnimParamHeader* h = (AnimParamHeader*)address;

            hdr = *h;
        }
    }
}