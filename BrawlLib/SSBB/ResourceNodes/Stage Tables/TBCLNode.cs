using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Stage_Tables;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBCLNode : TBNode
    {
        public override ResourceType ResourceFileType => ResourceType.TBCL;
        public override Type SubEntryType => typeof(TBCLEntryNode);

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBCL" ? new TBCLNode() : null;
        }
    }

    public unsafe class TBCLEntryNode : ResourceNode
    {
        public byte _count;

        [Category("TBCL Entry")]
        public byte Count
        {
            get => _count;
            set
            {
                _count = value;
                SignalPropertyChange();
            }
        }

        public float _collObj1;

        [Category("TBCL Entry")]
        [DisplayName("Collision Object 1")]
        public float CollisionObject1
        {
            get => _collObj1;
            set
            {
                _collObj1 = value;
                SignalPropertyChange();
            }
        }

        public float _collObj2;

        [Category("TBCL Entry")]
        [DisplayName("Collision Object 2")]
        public float CollisionObject2
        {
            get => _collObj2;
            set
            {
                _collObj2 = value;
                SignalPropertyChange();
            }
        }

        public float _collObj3;

        [Category("TBCL Entry")]
        [DisplayName("Collision Object 3")]
        public float CollisionObject3
        {
            get => _collObj3;
            set
            {
                _collObj3 = value;
                SignalPropertyChange();
            }
        }

        public float _collObj4;

        [Category("TBCL Entry")]
        [DisplayName("Collision Object 4")]
        public float CollisionObject4
        {
            get => _collObj4;
            set
            {
                _collObj4 = value;
                SignalPropertyChange();
            }
        }

        public float _collObj5;

        [Category("TBCL Entry")]
        [DisplayName("Collision Object 5")]
        public float CollisionObject5
        {
            get => _collObj5;
            set
            {
                _collObj5 = value;
                SignalPropertyChange();
            }
        }

        public float _collObj6;

        [Category("TBCL Entry")]
        [DisplayName("Collision Object 6")]
        public float CollisionObject6
        {
            get => _collObj6;
            set
            {
                _collObj6 = value;
                SignalPropertyChange();
            }
        }

        public float _collObj7;

        [Category("TBCL Entry")]
        [DisplayName("Collision Object 7")]
        public float CollisionObject7
        {
            get => _collObj7;
            set
            {
                _collObj7 = value;
                SignalPropertyChange();
            }
        }

        public float _collObj8;

        [Category("TBCL Entry")]
        [DisplayName("Collision Object 8")]
        public float CollisionObject8
        {
            get => _collObj8;
            set
            {
                _collObj8 = value;
                SignalPropertyChange();
            }
        }

        public float _collObj9;

        [Category("TBCL Entry")]
        [DisplayName("Collision Object 9")]
        public float CollisionObject9
        {
            get => _collObj9;
            set
            {
                _collObj9 = value;
                SignalPropertyChange();
            }
        }

        public float _collObj10;

        [Category("TBCL Entry")]
        [DisplayName("Collision Object 10")]
        public float CollisionObject10
        {
            get => _collObj10;
            set
            {
                _collObj10 = value;
                SignalPropertyChange();
            }
        }

        public float _collObj11;

        [Category("TBCL Entry")]
        [DisplayName("Collision Object 11")]
        public float CollisionObject11
        {
            get => _collObj11;
            set
            {
                _collObj11 = value;
                SignalPropertyChange();
            }
        }

        public float _collObj12;

        [Category("TBCL Entry")]
        [DisplayName("Collision Object 12")]
        public float CollisionObject12
        {
            get => _collObj12;
            set
            {
                _collObj12 = value;
                SignalPropertyChange();
            }
        }

        public float _collObj13;

        [Category("TBCL Entry")]
        [DisplayName("Collision Object 13")]
        public float CollisionObject13
        {
            get => _collObj13;
            set
            {
                _collObj13 = value;
                SignalPropertyChange();
            }
        }

        public float _collObj14;

        [Category("TBCL Entry")]
        [DisplayName("Collision Object 14")]
        public float CollisionObject14
        {
            get => _collObj14;
            set
            {
                _collObj14 = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x1;

        [Category("TBCL Entry")]
        public byte Unknown0x1
        {
            get => _unk0x1;
            set
            {
                _unk0x1 = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x2;

        [Category("TBCL Entry")]
        public byte Unknown0x2
        {
            get => _unk0x2;
            set
            {
                _unk0x2 = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x3;

        [Category("TBCL Entry")]
        public byte Unknown0x3
        {
            get => _unk0x3;
            set
            {
                _unk0x3 = value;
                SignalPropertyChange();
            }
        }

        public TBCLEntryNode()
        {
            _collObj1 = -1;
            _collObj2 = -1;
            _collObj3 = -1;
            _collObj4 = -1;
            _collObj5 = -1;
            _collObj6 = -1;
            _collObj7 = -1;
            _collObj8 = -1;
            _collObj9 = -1;
            _collObj10 = -1;
            _collObj11 = -1;
            _collObj12 = -1;
            _collObj13 = -1;
            _collObj14 = -1;
        }

        public override int OnCalculateSize(bool force)
        {
            return TBCLEntry.Size;
        }

        public override bool OnInitialize()
        {
            TBCLEntry* header = (TBCLEntry*) WorkingUncompressed.Address;

            _count = header->_count;
            _unk0x1 = header->_unk0x1;
            _unk0x2 = header->_unk0x2;
            _unk0x3 = header->_unk0x3;
            _collObj1 = header->_collObj1;
            _collObj2 = header->_collObj2;
            _collObj3 = header->_collObj3;
            _collObj4 = header->_collObj4;
            _collObj5 = header->_collObj5;
            _collObj6 = header->_collObj6;
            _collObj7 = header->_collObj7;
            _collObj8 = header->_collObj8;
            _collObj9 = header->_collObj9;
            _collObj10 = header->_collObj10;
            _collObj11 = header->_collObj11;
            _collObj12 = header->_collObj12;
            _collObj13 = header->_collObj13;
            _collObj14 = header->_collObj14;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TBCLEntry* hdr = (TBCLEntry*) address;
            hdr->_count = _count;
            hdr->_unk0x1 = _unk0x1;
            hdr->_unk0x2 = _unk0x2;
            hdr->_unk0x3 = _unk0x3;
            hdr->_collObj1 = _collObj1;
            hdr->_collObj2 = _collObj2;
            hdr->_collObj3 = _collObj3;
            hdr->_collObj4 = _collObj4;
            hdr->_collObj5 = _collObj5;
            hdr->_collObj6 = _collObj6;
            hdr->_collObj7 = _collObj7;
            hdr->_collObj8 = _collObj8;
            hdr->_collObj9 = _collObj9;
            hdr->_collObj10 = _collObj10;
            hdr->_collObj11 = _collObj11;
            hdr->_collObj12 = _collObj12;
            hdr->_collObj13 = _collObj13;
            hdr->_collObj14 = _collObj14;
        }
    }
}