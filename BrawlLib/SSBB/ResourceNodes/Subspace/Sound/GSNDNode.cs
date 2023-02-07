using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Sound;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GSNDNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GSNDEntryNode);
        public override ResourceType ResourceFileType => ResourceType.GSND;
        protected override string baseName => "Sound Effects";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GSND" ? new GSNDNode() : null;
        }
    }

    public unsafe class GSNDEntryNode : ResourceNode
    {
        internal GSNDEntry* Header => (GSNDEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        internal string _Bname;

        [Category("Sound")]
        [DisplayName("Bone Name")]
        public string BName
        {
            get => _Bname;
            set
            {
                _Bname = value;
                SignalPropertyChange();
            }
        }

        internal int _infoindex;

        [Category("Sound")]
        [DisplayName("Info Index")]
        public int infoIndex
        {
            get => _infoindex;
            set
            {
                _infoindex = value;
                SignalPropertyChange();
            }
        }

        internal float _unkfloat0;

        [Category("Misc")]
        [DisplayName("unknown")]
        public float unkFloat0
        {
            get => _unkfloat0;
            set
            {
                _unkfloat0 = value;
                SignalPropertyChange();
            }
        }

        internal float _unkfloat1;

        [Category("Misc")]
        [DisplayName("unknown")]
        public float unkFloat1
        {
            get => _unkfloat1;
            set
            {
                _unkfloat1 = value;
                SignalPropertyChange();
            }
        }

        internal string _trigger;

        [Category("Misc")]
        [DisplayName("Trigger ID?")]
        public string Trigger
        {
            get => _trigger;
            set
            {
                _trigger = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
            {
                _name = "Sound [" + Index + ']';
            }

            _Bname = Header->Name;
            _infoindex = Header->_infoIndex;
            _unkfloat0 = Header->_unkFloat0;
            _unkfloat1 = Header->_unkFloat1;
            _trigger = Header->Trigger;
            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x50;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GSNDEntry* header = (GSNDEntry*) address;
            *header = new GSNDEntry();
            header->_infoIndex = _infoindex;
            header->_pad0 = header->_pad1 =
                header->_pad2 = header->Pad4 = 0;
            header->_unkFloat0 = _unkfloat0;
            header->_unkFloat1 = _unkfloat1;
            header->Name = _Bname;
            header->Trigger = _trigger;
            header->unk0 = 0x00000001;
        }
    }
}