using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Graphics;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GNDVNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GNDVEntryNode);

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GNDV" ? new GNDVNode() : null;
        }
    }

    public unsafe class GNDVEntryNode : ResourceNode
    {
        protected internal GNDVEntry* Entry => (GNDVEntry*) WorkingUncompressed.Address;

        [Category("General")]
        public uint ModelDataFileIndex
        {
            get => _unk1;
            set
            {
                _unk1 = value;
                SignalPropertyChange();
            }
        }

        private uint _unk1 = 0;

        [Category("General")]
        [DisplayName("Target Bone Name")]
        public string BoneName
        {
            get => _boneName;
            set
            {
                _boneName = value;
                Name = value;
                SignalPropertyChange();
            }
        }

        private string _boneName = string.Empty;

        [Category("General")]
        [DisplayName("SFX Info Index")]
        public int SFXInfoIndex
        {
            get => _sfx;
            set
            {
                _sfx = value;
                SignalPropertyChange();
            }
        }

        private int _sfx;

        [Category("General")]
        [DisplayName("Graphic ID")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint Graphic
        {
            get => _gfx;
            set
            {
                _gfx = value;
                SignalPropertyChange();
            }
        }

        private uint _gfx;

        [Category("Triggers")]
        [DisplayName("Trigger")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger
        {
            get => _trigger;
            set
            {
                _trigger = value;
                SignalPropertyChange();
            }
        }

        private TriggerDataClass _trigger;

        public GNDVEntryNode()
        {
            _trigger = new TriggerDataClass(this);
        }

        public override bool OnInitialize()
        {
            _unk1 = Entry->_unk1;
            _boneName = Entry->BoneName;
            _sfx = Entry->_sfx;
            _gfx = Entry->_gfx;
            _trigger = new TriggerDataClass(this, Entry->_trigger);

            if (_name == null)
            {
                _name = BoneName;
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return GNDVEntry.SIZE;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GNDVEntry* header = (GNDVEntry*) address;
            *header = new GNDVEntry();
            header->_unk1 = ModelDataFileIndex;
            header->BoneName = BoneName;
            header->_sfx = SFXInfoIndex;
            header->_gfx = Graphic;
            header->_trigger = Trigger;
        }
    }
}