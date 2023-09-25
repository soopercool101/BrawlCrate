using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GET2Node : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GET2EntryNode);
        protected override string baseName => "Camera Area Triggers";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GET2" ? new GET2Node() : null;
        }
    }

    public unsafe class GET2EntryNode : ResourceNode
    {
        protected internal GET2Entry Data;

        [Category("Unknown")]
        public byte Unknown0x00
        {
            get => Data._unknown0x00;
            set
            {
                Data._unknown0x00 = value;
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
            set
            {
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

        [Category("GET2")]
        [DisplayName("Activation Coord 1")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 Point1
        {
            get => new Vector2(Data._x1, Data._y1);
            set
            {
                Data._x1 = value._x;
                Data._y1 = value._y;
                SignalPropertyChange();
            }
        }

        [Category("GET2")]
        [DisplayName("Activation Coord 2")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 Point2
        {
            get => new Vector2(Data._x2, Data._y2);
            set
            {
                Data._x2 = value._x;
                Data._y2 = value._y;
                SignalPropertyChange();
            }
        }

        [Category("GET2")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger
        {
            get => _trigger ?? (_trigger = new TriggerDataClass(this));
            set
            {
                _trigger = value;
                SignalPropertyChange();
            }
        }

        private TriggerDataClass _trigger;

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = $"Area [{Index}]";
            }

            Data = *(GET2Entry*) WorkingUncompressed.Address;
            _trigger = new TriggerDataClass(this, Data._trigger);

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return GET2Entry.SIZE;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            Data._trigger = _trigger;
            GET2Entry* header = (GET2Entry*) address;
            *header = Data;
        }
    }
}
