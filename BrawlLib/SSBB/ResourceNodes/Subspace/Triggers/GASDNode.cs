using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Sound;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes.Subspace.Triggers
{
    public class GASDNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GASDEntryNode);
        protected override string baseName => "Area Sound Trigger";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GASD" ? new GASDNode() : null;
        }
    }

    public unsafe class GASDEntryNode : ResourceNode
    {
        private GASDEntry Data;
        internal GASDEntry* Header => (GASDEntry*)WorkingUncompressed.Address;
        public override bool supportsCompression => false;
        
        [Category("Unknown")]
        public uint Unknown0x00
        {
            get => Data._unknown0x00;
            set
            {
                Data._unknown0x00 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x04
        {
            get => Data._unknown0x04;
            set
            {
                Data._unknown0x04 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x08
        {
            get => Data._unknown0x08;
            set
            {
                Data._unknown0x08 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x0C
        {
            get => Data._unknown0x0C;
            set
            {
                Data._unknown0x0C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x10
        {
            get => Data._unknown0x10;
            set
            {
                Data._unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x14
        {
            get => Data._unknown0x14;
            set
            {
                Data._unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [TypeConverter(typeof(HexUShortConverter))]
        public ushort Unknown0x18
        {
            get => Data._unknown0x18;
            set
            {
                Data._unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [TypeConverter(typeof(HexUShortConverter))]
        public ushort Unknown0x1A
        {
            get => Data._unknown0x1A;
            set
            {
                Data._unknown0x1A = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _trigger1;
        [Category("GASD")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger1
        {
            get => _trigger1;
            set
            {
                _trigger1 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _trigger2;
        [Category("GASD")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger2
        {
            get => _trigger2;
            set
            {
                _trigger2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x24
        {
            get => Data._unknown0x24;
            set
            {
                Data._unknown0x24 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x28
        {
            get => Data._unknown0x28;
            set
            {
                Data._unknown0x28 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x2C
        {
            get => Data._unknown0x2C;
            set
            {
                Data._unknown0x2C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x30
        {
            get => Data._unknown0x30;
            set
            {
                Data._unknown0x30 = value;
                SignalPropertyChange();
            }
        }

        public GASDEntryNode()
        {
            _trigger1 = new TriggerDataClass(this);
            _trigger2 = new TriggerDataClass(this);
        }

        public override int OnCalculateSize(bool force)
        {
            return GASDEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GASDEntry*) WorkingUncompressed.Address;
            _trigger1 = new TriggerDataClass(this, Header->_trigger1);
            _trigger2 = new TriggerDataClass(this, Header->_trigger2);

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GASDEntry* hdr = (GASDEntry*)address;
            Data._trigger1 = _trigger1;
            Data._trigger2 = _trigger2;
            *hdr = Data;
        }
    }
}
