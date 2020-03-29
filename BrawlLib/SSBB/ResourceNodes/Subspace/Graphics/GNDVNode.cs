using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Graphics;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GNDVNode : BLOCEntryNode
    {
        protected override Type SubEntryType => typeof(GNDVEntryNode);

        internal static ResourceNode TryParse(DataSource source)
        {
            return source.Tag == "GNDV" ? new GNDVNode() : null;
        }
    }

    public unsafe class GNDVEntryNode : ResourceNode
    {
        protected internal GNDVEntry* Entry => (GNDVEntry*) WorkingUncompressed.Address;

        [Category("General")]
        public uint Unk1
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
        [DisplayName("TriggerID")]
        public uint TriggerID
        {
            get => _triggerID;
            set
            {
                _triggerID = value;
                SignalPropertyChange();
            }
        }

        private uint _triggerID;

        public override bool OnInitialize()
        {
            _unk1 = Entry->_unk1;
            _boneName = Entry->BoneName;
            _sfx = Entry->_sfx;
            _gfx = Entry->_gfx;
            _triggerID = Entry->_triggerID;

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
            header->_unk1 = Unk1;
            header->BoneName = BoneName;
            header->_sfx = SFXInfoIndex;
            header->_gfx = Graphic;
            header->_triggerID = TriggerID;
        }
    }
}