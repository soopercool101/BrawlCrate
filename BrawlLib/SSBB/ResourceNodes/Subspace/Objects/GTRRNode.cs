using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes.Subspace.Objects
{
    public class GTRRNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GTRREntryNode);
        protected override string baseName => "Minecart Triggers";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GTRR" ? new GTRRNode() : null;
        }
    }
    
    public unsafe class GTRREntryNode : ResourceNode
    {
        internal GTRREntry Data;
        public override bool supportsCompression => false;

        [Category("GTRR")]
        [Description("If true, Position2 becomes range")]
        public bool UseTwoPoints
        {
            get => Data._areaData._useTwoPoints;
            set
            {
                Data._areaData._useTwoPoints = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x01
        {
            get => Data._areaData._unknown0x01;
            set
            {
                Data._areaData._unknown0x01 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x02
        {
            get => Data._areaData._unknown0x02;
            set
            {
                Data._areaData._unknown0x02 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x03
        {
            get => Data._areaData._unknown0x03;
            set
            {
                Data._areaData._unknown0x03 = value;
                SignalPropertyChange();
            }
        }

        [Category("GTRR")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 Position
        {
            get => new Vector2(Data._areaData._pos1X, Data._areaData._pos1Y);
            set
            {
                Data._areaData._pos1X = value.X;
                Data._areaData._pos1Y = value.Y;
                SignalPropertyChange();
            }
        }

        [Category("GTRR")]
        [Description("Used for Range if UseTwoPoints is false")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 Position2
        {
            get => new Vector2(Data._areaData._pos2X, Data._areaData._pos2Y);
            set
            {
                Data._areaData._pos2X = value.X;
                Data._areaData._pos2Y = value.Y;
                SignalPropertyChange();
            }
        }
        
        public TriggerDataClass _trigger;

        [Category("GTRR")]
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

        public override int OnCalculateSize(bool force)
        {
            return GTRREntry.Size;
        }

        public GTRREntryNode()
        {
            _trigger = new TriggerDataClass(this);
        }

        public override bool OnInitialize()
        {
            Data = *(GTRREntry*)WorkingUncompressed.Address;
            _trigger = new TriggerDataClass(this, Data._triggerData);
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GTRREntry* hdr = (GTRREntry*)address;
            Data._triggerData = _trigger;
            *hdr = Data;
        }
    }
}
