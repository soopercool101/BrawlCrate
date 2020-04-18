using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Animation;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GLK2Node : BLOCEntryNode
    {
        protected override Type SubEntryType => typeof(GLK2EntryNode);
        public override ResourceType ResourceFileType => ResourceType.Unknown;
        protected override string baseName => "Camera Locks";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GLK2" ? new GLK2Node() : null;
        }
    }

    public unsafe class GLK2EntryNode : ResourceNode
    {
        internal GLK2Entry* Header => (GLK2Entry*)WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        private int _unkown0x00;
        private byte _unkflag1;
        private byte _unkflag2;
        private byte _unkflag3;
        private byte _unkflag4;
        private uint _trigger1;
        private uint _trigger2;
        private uint _trigger3;
        private uint _trigger4;

        [Category("Camera")]
        [DisplayName("Unk0")]
        public int Unk1
        {
            get => _unkown0x00;
            set
            {
                _unkown0x00 = value;
                SignalPropertyChange();
            }
        }
        [Category("Camera")]
        [DisplayName("Model Data Node")]
        [Description("File Index to stgposition model to use for this lock.")]
        public byte ModelDataID
        {
            get => _unkflag1;
            set
            {
                _unkflag1 = value;
                SignalPropertyChange();
            }
        }
        [Category("Camera")]
        [DisplayName("Unk2")]
        public byte Unk2
        {
            get => _unkflag2;
            set
            {
                _unkflag2 = value;
                SignalPropertyChange();
            }
        }
        [Category("Camera")]
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
        [Category("Camera")]
        [DisplayName("Unk4")]
        public byte Unk4
        {
            get => _unkflag4;
            set
            {
                _unkflag4 = value;
                SignalPropertyChange();
            }
        }
        [Category("Camera")]
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
        [Category("Camera")]
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
        [Category("Camera")]
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
        [Category("Camera")]
        [DisplayName("Trigger4")]
        [TypeConverter(typeof(HexTypeConverter))]
        public uint Trigger4
        {
            get => _trigger4;
            set
            {
                _trigger4 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            _unkown0x00 = Header->_unknown0x00;
            _unkflag1 = Header->_unkflag1;
            _unkflag2 = Header->_unkflag2;
            _unkflag3 = Header->_unkflag3;
            _unkflag4 = Header->_unkflag4;
            _trigger1 = Header->_trigger1;
            _trigger2 = Header->_trigger2;
            _trigger3 = Header->_trigger3;
            _trigger4 = Header->_trigger4;

            if (_name == null)
            {
                _name = "Locked Zone [" + Index + "]";
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return GLK2Entry.SIZE;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GLK2Entry* hdr = (GLK2Entry*)address;
            hdr->_unknown0x00 = _unkown0x00;
            hdr->_unkflag1 = _unkflag1;
            hdr->_unkflag2 = _unkflag2;
            hdr->_unkflag3 = _unkflag3;
            hdr->_unkflag4 = _unkflag4;
            hdr->_trigger1 = _trigger1;
            hdr->_trigger2 = _trigger2;
            hdr->_trigger3 = _trigger3;
            hdr->_trigger4 = _trigger4;
        }
    }
}