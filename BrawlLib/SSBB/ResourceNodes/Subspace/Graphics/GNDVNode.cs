using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Graphics;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GNDVNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GNDVEntryNode);
        protected override string baseName => "Node Visibility";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GNDV" ? new GNDVNode() : null;
        }
    }

    public unsafe class GNDVEntryNode : ResourceNode
    {
        private GNDVEntry Data;

        [Category("GNDV")]
        public byte ModelDataFileIndex
        {
            get => Data._modelDataFileIndex;
            set
            {
                Data._modelDataFileIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x01
        {
            get => Data._unknown0x01;
            set
            {
                Data._unknown0x01 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x02
        {
            get => Data._unknown0x02;
            set {
                Data._unknown0x02 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x03
        {
            get => Data._unknown0x03;
            set
            {
                Data._unknown0x03 = value;
                SignalPropertyChange();
            }
        }

        [Category("GNDV")]
        public string BoneName
        {
            get => Data.BoneName;
            set
            {
                Data.BoneName = value;
                Name = Data.BoneName;
                SignalPropertyChange();
            }
        }

        [Category("GNDV")]
        [TypeConverter(typeof(HexIntConverter))]
        public int SFXInfoIndex
        {
            get => Data._sfx;
            set
            {
                Data._sfx = value;
                SignalPropertyChange();
            }
        }

        [Category("GNDV")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint GraphicId
        {
            get => Data._gfx;
            set
            {
                Data._gfx = value;
                SignalPropertyChange();
            }
        }

        private TriggerDataClass _trigger;

        [Category("GNDV")]
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

        public override string Name
        {
            get => base.Name;
            set
            {
                BoneName = value;
                base.Name = BoneName;
            }
        }

        public override int MaxNameLength => 0x20;

        public GNDVEntryNode()
        {
            _trigger = new TriggerDataClass(this);
        }

        public override bool OnInitialize()
        {
            Data = *(GNDVEntry*)WorkingUncompressed.Address;
            _trigger = new TriggerDataClass(this, Data._trigger);

            if (_name == null)
            {
                _name = BoneName;
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return GNDVEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GNDVEntry* header = (GNDVEntry*) address;
            Data._trigger = _trigger;
            *header = Data;
        }
    }
}