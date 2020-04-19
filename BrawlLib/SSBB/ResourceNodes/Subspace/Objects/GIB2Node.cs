using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GIB2Node : BLOCEntryNode
    {
        protected override Type SubEntryType => typeof(GIB2EntryNode);
        public override ResourceType ResourceFileType => ResourceType.GIB2;
        protected override string baseName => "Item Boxes";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GIB2" ? new GIB2Node() : null;
        }
    }

    public unsafe class GIB2EntryNode : ResourceNode
    {
        internal GIB2Entry* Header => (GIB2Entry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;
        public override bool supportsCompression => false;

        public bfloat _header;
        public byte _unknown0x04;
        public byte _unkflag0;
        public byte _unkflag1;
        public byte _unknown0x07;
        public byte _unknown0x08;
        public byte _unknown0x09;
        public byte _unknown0x0A;
        public byte _unknown0x0B;
        public byte _unknown0x0C;
        public byte _unknown0x0D;
        public byte _unknown0x0E;
        public byte _unknown0x0F;
        public byte _unknown0x10;
        public byte _unknown0x11;
        public byte _unknown0x12;
        public byte _unknown0x13;
        public byte _unknown0x14;
        public byte _unknown0x15;
        public byte _unknown0x16;
        public byte _unknown0x17;
        public byte _unknown0x18;
        public byte _unknown0x19;
        public float _unkflag2;
        public byte _unknown0x1E;
        public byte _unknown0x1F;
        public byte _unknown0x20;
        public byte _unknown0x21;
        public byte _unknown0x22;
        public byte _unknown0x23;
        public byte _unknown0x24;
        public byte _unknown0x25;
        public byte _unknown0x26;
        public byte _unknown0x27;
        public byte _unknown0x28;
        public byte _unknown0x29;
        public byte _unknown0x2A;
        public byte _unkflag3;
        public byte _modeldataid;
        public byte _collisiondataid;
        public byte _unknown0x2E;
        public byte _unknown0x2F;
        public float _posX;
        public float _posY;
        public int _itemspawngroup;
        public byte _unknown0x3C;
        public byte _unknown0x3D;
        public byte _unknown0x3E;
        public byte _unknown0x3F;
        public byte _unknown0x40;
        public byte _unknown0x41;
        public byte _unknown0x42;
        public byte _unknown0x43;
        public short _unkflag9;
        public byte _unkflag10;
        public byte _unkflag11;
        public uint _trigger1;
        public uint _trigger2;
        public uint _trigger3;

        [Category("Item Box")]
        [DisplayName("Unk0")]
        public byte Unk0
        {
            get => _unkflag0;
            set
            {
                _unkflag0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk1")]
        public byte Unk1
        {
            get => _unkflag1;
            set
            {
                _unkflag1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk2")]
        public float Unk2
        {
            get => _unkflag2;
            set
            {
                _unkflag2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk3")]
        public byte Unk3
        {
            get => _unkflag3;
            set
            {
                _unkflag3 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Model Data ID")]
        [Description("File Index for corrosponding Model Data node.")]
        public byte ModelDataID
        {
            get => _modeldataid;
            set
            {
                _modeldataid = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Collision Data ID")]
        [Description("File Index for corrosponding collision data node.")]
        public byte CollisionDataID
        {
            get => _collisiondataid;
            set
            {
                _collisiondataid = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("PositionX")]
        public float PosX
        {
            get => _posX;
            set
            {
                PosX = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("PositionY")]
        public float PosY
        {
            get => _posY;
            set
            {
                _posY = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Item Group")]
        [Description("Group to spawn items from in the stage's ItemGen table")]
        public int ItemSpawnGroup
        {
            get => _itemspawngroup;
            set
            {
                _itemspawngroup = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk9")]
        public short Unk9
        {
            get => _unkflag9;
            set
            {
                _unkflag9 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk10")]
        public byte Unk10
        {
            get => _unkflag10;
            set
            {
                _unkflag10 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk11")]
        public byte Unk11
        {
            get => _unkflag11;
            set
            {
                _unkflag11 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Trigger1")]
        [TypeConverter(typeof(HexTypeConverter))]
        public uint Trigger1
        {
            get => _trigger1;
            set
            {
                _trigger1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Trigger2")]
        [TypeConverter(typeof(HexTypeConverter))]
        public uint Trigger2
        {
            get => _trigger2;
            set
            {
                _trigger2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Trigger3")]
        [TypeConverter(typeof(HexTypeConverter))]
        public uint Trigger3
        {
            get => _trigger3;
            set
            {
                _trigger3 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            _header = Header->_header;
            _unknown0x04 = Header->_unknown0x04;
            _unkflag0 = Header->_unkflag0;
            _unkflag1 = Header->_unkflag1;
            _unknown0x07 = Header->_unknown0x07;
            _unknown0x08 = Header->_unknown0x08;
            _unknown0x09 = Header->_unknown0x09;
            _unknown0x0A = Header->_unknown0x0A;
            _unknown0x0B = Header->_unknown0x0B;
            _unknown0x0C = Header->_unknown0x0C;
            _unknown0x0D = Header->_unknown0x0D;
            _unknown0x0E = Header->_unknown0x0E;
            _unknown0x0F = Header->_unknown0x0F;
            _unknown0x10 = Header->_unknown0x10;
            _unknown0x11 = Header->_unknown0x11;
            _unknown0x12 = Header->_unknown0x12;
            _unknown0x13 = Header->_unknown0x13;
            _unknown0x14 = Header->_unknown0x14;
            _unknown0x15 = Header->_unknown0x15;
            _unknown0x16 = Header->_unknown0x16;
            _unknown0x17 = Header->_unknown0x17;
            _unknown0x18 = Header->_unknown0x18;
            _unknown0x19 = Header->_unknown0x19;
            _unkflag2 = Header->_unkflag2;
            _unknown0x1E = Header->_unknown0x1E;
            _unknown0x1F = Header->_unknown0x1F;
            _unknown0x20 = Header->_unknown0x20;
            _unknown0x21 = Header->_unknown0x21;
            _unknown0x22 = Header->_unknown0x22;
            _unknown0x23 = Header->_unknown0x23;
            _unknown0x24 = Header->_unknown0x24;
            _unknown0x25 = Header->_unknown0x25;
            _unknown0x26 = Header->_unknown0x26;
            _unknown0x27 = Header->_unknown0x27;
            _unknown0x28 = Header->_unknown0x28;
            _unknown0x29 = Header->_unknown0x29;
            _unknown0x2A = Header->_unknown0x2A;
            _unkflag3 = Header->_unkflag3;
            _modeldataid = Header->_modeldataid;
            _collisiondataid = Header->_collisiondataid;
            _unknown0x2E = Header->_unknown0x2E;
            _unknown0x2F = Header->_unknown0x2F;
            _posX = Header->_posX;
            _posY = Header->_posY;
            _itemspawngroup = Header->_itemspawngroup;
            _unknown0x3C = Header->_unknown0x3C;
            _unknown0x3D = Header->_unknown0x3D;
            _unknown0x3E = Header->_unknown0x3E;
            _unknown0x3F = Header->_unknown0x3F;
            _unknown0x40 = Header->_unknown0x40;
            _unknown0x41 = Header->_unknown0x41;
            _unknown0x42 = Header->_unknown0x42;
            _unknown0x43 = Header->_unknown0x43;
            _unkflag9 = Header->_unkflag9;
            _unkflag10 = Header->_unkflag10;
            _unkflag11 = Header->_unkflag11;
            _trigger1 = Header->_trigger1;
            _trigger2 = Header->_trigger2;
            _trigger3 = Header->_trigger3;

            if (_name == null)
            {
                _name = "Item Box [" + Index + "]";
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x54;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GIB2Entry* hdr = (GIB2Entry*) address;
            hdr->_header = _header;
            hdr->_unkflag0 = _unkflag0;
            hdr->_unkflag1 = _unkflag1;
            hdr->_unknown0x07 = _unknown0x07;
            hdr->_unknown0x08 = _unknown0x08;
            hdr->_unknown0x09 = _unknown0x09;
            hdr->_unknown0x0A = _unknown0x0A;
            hdr->_unknown0x0B = _unknown0x0B;
            hdr->_unknown0x0C = _unknown0x0C;
            hdr->_unknown0x0D = _unknown0x0D;
            hdr->_unknown0x0E = _unknown0x0E;
            hdr->_unknown0x0F = _unknown0x0F;
            hdr->_unknown0x10 = _unknown0x10;
            hdr->_unknown0x11 = _unknown0x11;
            hdr->_unknown0x12 = _unknown0x12;
            hdr->_unknown0x13 = _unknown0x13;
            hdr->_unknown0x14 = _unknown0x14;
            hdr->_unknown0x15 = _unknown0x15;
            hdr->_unknown0x16 = _unknown0x16;
            hdr->_unknown0x17 = _unknown0x17;
            hdr->_unknown0x18 = _unknown0x18;
            hdr->_unknown0x19 = _unknown0x19;
            hdr->_unkflag2 = _unkflag2;
            hdr->_unknown0x1E = _unknown0x1E;
            hdr->_unknown0x1F = _unknown0x1F;
            hdr->_unknown0x20 = _unknown0x20;
            hdr->_unknown0x21 = _unknown0x21;
            hdr->_unknown0x22 = _unknown0x22;
            hdr->_unknown0x23 = _unknown0x23;
            hdr->_unknown0x24 = _unknown0x24;
            hdr->_unknown0x25 = _unknown0x25;
            hdr->_unknown0x26 = _unknown0x26;
            hdr->_unknown0x27 = _unknown0x27;
            hdr->_unknown0x28 = _unknown0x28;
            hdr->_unknown0x29 = _unknown0x29;
            hdr->_unknown0x2A = _unknown0x2A;
            hdr->_unkflag3 = _unkflag3;
            hdr->_modeldataid = _modeldataid;
            hdr->_collisiondataid = _collisiondataid;
            hdr->_unknown0x2E = _unknown0x2E;
            hdr->_unknown0x2F = _unknown0x2F;
            hdr->_posX = _posX;
            hdr->_posY = _posY;
            hdr->_itemspawngroup = _itemspawngroup;
            hdr->_unknown0x3C = _unknown0x3C;
            hdr->_unknown0x3D = _unknown0x3D;
            hdr->_unknown0x3E = _unknown0x3E;
            hdr->_unknown0x3F = _unknown0x3F;
            hdr->_unknown0x40 = _unknown0x40;
            hdr->_unknown0x41 = _unknown0x41;
            hdr->_unknown0x42 = _unknown0x42;
            hdr->_unknown0x43 = _unknown0x43;
            hdr->_unkflag9 = _unkflag9;
            hdr->_unkflag10 = _unkflag10;
            hdr->_unkflag11 = _unkflag11;
            hdr->_trigger1 = _trigger1;
            hdr->_trigger2 = _trigger2;
            hdr->_trigger3 = _trigger3;
        }
    }
}