using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GBLTNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GBLTEntryNode);
        protected override string baseName => "Conveyor Belts";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GBLT" ? new GBLTNode() : null;
        }
    }

    public unsafe class GBLTEntryNode : ResourceNode
    {
        protected internal GBLTEntry Data;

        [Category("Unknown")]
        public int Unknown0x00
        {
            get => Data._unknown0x00;
            set
            {
                Data._unknown0x00 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x04
        {
            get => Data._unknown0x04;
            set
            {
                Data._unknown0x04 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x08
        {
            get => Data._unknown0x08;
            set
            {
                Data._unknown0x08 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0C
        {
            get => Data._unknown0x0C;
            set
            {
                Data._unknown0x0C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x10
        {
            get => Data._unknown0x10;
            set
            {
                Data._unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x14
        {
            get => Data._unknown0x14;
            set
            {
                Data._unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x18
        {
            get => Data._unknown0x18;
            set
            {
                Data._unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x1C
        {
            get => Data._unknown0x1C;
            set
            {
                Data._unknown0x1C = value;
                SignalPropertyChange();
            }
        }

        [Category("GBLT")]
        public float EffectAreaWidth
        {
            get => Data._areaWidth;
            set
            {
                Data._areaWidth = value;
                SignalPropertyChange();
            }
        }

        [Category("GBLT")]
        public float EffectAreaHeight
        {
            get => Data._areaHeight;
            set
            {
                Data._areaHeight = value;
                SignalPropertyChange();
            }
        }

        [Category("GBLT")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 EffectAreaCenter
        {
            get => new Vector2(Data._areaCenterX, Data._areaCenterY);
            set
            {
                Data._areaCenterX = value._x;
                Data._areaCenterY = value._y;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x30
        {
            get => Data._unknown0x30;
            set
            {
                Data._unknown0x30 = value;
                SignalPropertyChange();
            }
        }

        [Category("GBLT")]
        public float Speed
        {
            get => Data._speed;
            set
            {
                Data._speed = value;
                SignalPropertyChange();
            }
        }

        [Category("GBLT")]
        public Directionality Direction
        {
            get => Data._directionality;
            set
            {
                Data._directionality = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x39
        {
            get => Data._unknown0x39;
            set
            {
                Data._unknown0x39 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x3A
        {
            get => Data._unknown0x3A;
            set
            {
                Data._unknown0x3A = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x3B
        {
            get => Data._unknown0x3B;
            set
            {
                Data._unknown0x3B = value;
                SignalPropertyChange();
            }
        }

        private TriggerDataClass _trigger;
        [Category("GBLT")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger
        {
            get => _trigger ?? (_trigger = new TriggerDataClass(this));
            set
            {
                Data._trigger = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            Data = *(GBLTEntry*)WorkingUncompressed.Address;
            _trigger = new TriggerDataClass(this, Data._trigger);
            return base.OnInitialize();
        }

        public override int OnCalculateSize(bool force)
        {
            return GBLTEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            Data._trigger = _trigger;
            GBLTEntry* header = (GBLTEntry*)address;
            *header = Data;
        }
    }
}
