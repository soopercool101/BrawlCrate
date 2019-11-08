using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Graphics;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GNDVNode : ResourceNode
    {
        protected internal GNDV* Header => (GNDV*) WorkingUncompressed.Address;

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "GNDV";
            }

            return Header->_entryCount > 0;
        }

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_entryCount; i++)
            {
                DataSource source;
                if (i == Header->_entryCount - 1)
                {
                    source = new DataSource((*Header)[i],
                        WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]);
                }
                else
                {
                    source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]);
                }

                new GNDVEntryNode().Initialize(this, source);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GNDV.SIZE + Children.Count * 4 + Children.Count * GNDVEntry.SIZE;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GNDV* header = (GNDV*) address;
            *header = new GNDV();
            header->_tag = GNDV.TAG;
            header->_entryCount = Children.Count;

            uint offset = (uint) (0x08 + Children.Count * 4);
            for (int i = 0; i < Children.Count; i++)
            {
                if (i > 0)
                {
                    offset += (uint) Children[i - 1].CalculateSize(false);
                }

                *(buint*) (address + 0x08 + i * 4) = offset;
                _children[i].Rebuild(address + offset, _children[i].CalculateSize(false), true);
            }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((GNDV*) source.Address)->_tag == GNDV.TAG ? new GNDVNode() : null;
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