using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Hazards;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GWNDNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GWNDEntryNode);
        protected override string baseName => "Winds";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GWND" ? new GWNDNode() : null;
        }
    }

    public unsafe class GWNDEntryNode : ResourceNode
    {
        protected internal GWNDEntry Data;

        [DisplayName("Unknown0x00 (uint)")]
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

        [DisplayName("Unknown0x04 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x04
        {
            get => Data._unknown0x04;
            set
            {
                Data._unknown0x04 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x08 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x08
        {
            get => Data._unknown0x08;
            set
            {
                Data._unknown0x08 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0C
        {
            get => Data._unknown0x0C;
            set
            {
                Data._unknown0x0C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x10 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x10
        {
            get => Data._unknown0x10;
            set
            {
                Data._unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x14 (uint)")]
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

        [DisplayName("Unknown0x18 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x18
        {
            get => Data._unknown0x18;
            set
            {
                Data._unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1C (float)")]
        [Category("Unknown")]
        public float Unknown0x1C
        {
            get => Data._unknown0x1C;
            set
            {
                Data._unknown0x1C = value;
                SignalPropertyChange();
            }
        }

        [Category("Wind")]
        public float WindWidth
        {
            get => Data._unknown0x20; 
            set
            {
                Data._unknown0x20 = value;
                SignalPropertyChange();
            }
        }

        [Category("Wind")]
        public float WindHeight 
        {
            get => Data._unknown0x24;
            set
            {
                Data._unknown0x24 = value;
                SignalPropertyChange();
            }
        }

        [Category("Wind")]
        public float WindCenterX
        {
            get => Data._unknown0x28;
            set
            {
                Data._unknown0x28 = value;
                SignalPropertyChange();
            }
        }

        [Category("Wind")]
        public float WindCenterY //WindHeight divided by 2 is the distance the windbox extends vertically in both directions from the center
        {
            get => Data._unknown0x2C;
            set
            {
                Data._unknown0x2C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x30 (uint)")]
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

        [DisplayName("Unknown0x34 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x34
        {
            get => Data._unknown0x34;
            set
            {
                Data._unknown0x34 = value;
                SignalPropertyChange();
            }
        }

        [Category("Wind")]
        public float WindAngle
        {
            get => Data._unknown0x38;
            set
            {
                Data._unknown0x38 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x3C;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x3C (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x3C
        {
            get => _unknown0x3C ?? new TriggerDataClass(this);
            set
            {
                _unknown0x3C = value;
                Data._unknown0x3C = value;
                SignalPropertyChange();
            }
        }

        public DifficultyRatiosClass _difficultyMotionRatios;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [Category("Wind")]
        public DifficultyRatiosClass DifficultyMotionRatios
        {
            get => _difficultyMotionRatios ?? new DifficultyRatiosClass(this);
            set
            {
                _difficultyMotionRatios = value;
                Data._difficultyMotionRatios = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GWNDEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GWNDEntry*) WorkingUncompressed.Address;
            _unknown0x3C = new TriggerDataClass(this, Data._unknown0x3C);
            _difficultyMotionRatios = new DifficultyRatiosClass(this, Data._difficultyMotionRatios);

            if (_name == null)
            {
                _name = $"Wind [{Index}]";
            }

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GWNDEntry* hdr = (GWNDEntry*)address;
            Data._unknown0x3C = _unknown0x3C;
            Data._difficultyMotionRatios = _difficultyMotionRatios;
            *hdr = Data;
        }
    }
}
