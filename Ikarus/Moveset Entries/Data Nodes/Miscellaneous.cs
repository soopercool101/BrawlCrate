using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class MiscSectionNode : MovesetEntryNode
    {
        sDataMisc _hdr;

        [Category("Misc Offsets")]
        public int Unknown0Offset { get { return _hdr.Unknown0; } }
        [Category("Misc Offsets")]
        public int FinalSmashAuraOffset { get { return _hdr.FinalSmashAura._startOffset; } }
        [Category("Misc Offsets")]
        public int FinalSmashAuraCount { get { return _hdr.FinalSmashAura._listCount; } }
        [Category("Misc Offsets")]
        public int HurtBoxOffset { get { return _hdr.HurtBoxes._startOffset; } }
        [Category("Misc Offsets")]
        public int HurtBoxCount { get { return _hdr.HurtBoxes._listCount; } }
        [Category("Misc Offsets")]
        public int LedgegrabOffset { get { return _hdr.Ledgegrabs._startOffset; } }
        [Category("Misc Offsets")]
        public int LedgegrabCount { get { return _hdr.Ledgegrabs._listCount; } }
        [Category("Misc Offsets")]
        public int Unknown7Offset { get { return _hdr.Unknown7._startOffset; } }
        [Category("Misc Offsets")]
        public int Unknown7Count { get { return _hdr.Unknown7._listCount; } }
        [Category("Misc Offsets")]
        public int BoneReferencesOffset { get { return _hdr.BoneReferences; } }
        [Category("Misc Offsets")]
        public int Unknown10Offset { get { return _hdr.Unknown10; } }
        [Category("Misc Offsets")]
        public int SoundDataOffset { get { return _hdr.SoundData; } }
        [Category("Misc Offsets")]
        public int Unknown9Offset { get { return _hdr.Unknown12; } }
        [Category("Misc Offsets")]
        public int MultiJumpOffset { get { return _hdr.MultiJump; } }
        [Category("Misc Offsets")]
        public int GlideOffset { get { return _hdr.Glide; } }
        [Category("Misc Offsets")]
        public int CrawlOffset { get { return _hdr.Crawl; } }
        [Category("Misc Offsets")]
        public int CollisionData { get { return _hdr.CollisionData; } }
        [Category("Misc Offsets")]
        public int TetherOffset { get { return _hdr.Tether; } }
        [Category("Misc Offsets")]
        public int Unknown18Offset { get { return _hdr.Unknown18; } }

        public EntryList<IndexValue> _unknown0;
        public EntryList<MiscFinalSmashAura> _finalSmashAura;
        public EntryList<MiscHurtBox> _hurtBoxes;
        public EntryList<MiscLedgeGrab> _ledgeGrabs;
        public EntryList<MiscUnk7Entry> _unknown7;
        public EntryList<BoneIndexValue> _boneRefs;
        public MiscUnk10 _unknown10;
        public MiscSoundData _soundData;
        public MiscUnk12 _unknown12;
        public MiscMultiJump _multiJump;
        public MiscGlide _glide;
        public MiscCrawl _crawl;
        public CollisionData _collisionData;
        public MiscTether _tether;
        public EntryList<IndexValue> _unknown18;

        protected override void OnParse(VoidPtr address)
        {
            //Get header values
            _hdr = *(sDataMisc*)address;

            //Parse all misc entries.
            //If an offset is 0, the entry will be set to null.
            _unknown0 = Parse<EntryList<IndexValue>>(_hdr[0], 4);
            _finalSmashAura = Parse<EntryList<MiscFinalSmashAura>>(_hdr[1], 0x14, _hdr[2]);
            _hurtBoxes = Parse<EntryList<MiscHurtBox>>(_hdr[3], 0x20, _hdr[4]);
            _ledgeGrabs = Parse<EntryList<MiscLedgeGrab>>(_hdr[5], 0x10, _hdr[6]);
            _unknown7 = Parse<EntryList<MiscUnk7Entry>>(_hdr[7], 0x20, _hdr[8]);
            _boneRefs = Parse<EntryList<BoneIndexValue>>(_hdr[9], 4, 10);
            _unknown10 = Parse<MiscUnk10>(_hdr[10]);
            _soundData = Parse<MiscSoundData>(_hdr[11]);
            _unknown12 = Parse<MiscUnk12>(_hdr[12]);
            _multiJump = Parse<MiscMultiJump>(_hdr[13]);
            _glide = Parse<MiscGlide>(_hdr[14]);
            _crawl = Parse<MiscCrawl>(_hdr[15]);
            _collisionData = Parse<CollisionData>(_hdr[16]);
            _tether = Parse<MiscTether>(_hdr[17]);
            if (_hdr[18] > 0)
            {
                sListOffset* o = (sListOffset*)Address(_hdr[18]);
                _unknown18 = Parse<EntryList<IndexValue>>(o->_startOffset, 4, (int)o->_listCount);
            }
        }
    }
}