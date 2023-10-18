using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Camera;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GFSRNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GFSREntryNode);
        protected override string baseName => "Camera Scrolls";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GFSR" ? new GFSRNode() : null;
        }
    }

    public unsafe class GFSREntryNode : ResourceNode
    {
        protected internal GFSREntry Data;

        [DisplayName("Unknown0x00 (float)")]
        [Category("Unknown")]
        public float Unknown0x00
        {
            get => Data._unknown0x00;
            set
            {
                Data._unknown0x00 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x04 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x04
        {
            get => Data._unknown0x04;
            set
            {
                Data._unknown0x04 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x05 (bool)")]
        [Category("Unknown")]
        public bool Unknown0x05
        {
            get => Data._unknown0x05;
            set
            {
                Data._unknown0x05 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x06 (bool)")]
        [Category("Unknown")]
        public bool Unknown0x06
        {
            get => Data._unknown0x06;
            set
            {
                Data._unknown0x06 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x07 (bool)")]
        [Category("Unknown")]
        public bool Unknown0x07
        {
            get => Data._unknown0x07;
            set
            {
                Data._unknown0x07 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x08;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x08 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x08
        {
            get => _unknown0x08 ?? new TriggerDataClass(this);
            set
            {
                _unknown0x08 = value;
                Data._unknown0x08 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x0C;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x0C (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x0C
        {
            get => _unknown0x0C ?? new TriggerDataClass(this);
            set
            {
                _unknown0x0C = value;
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

        public DifficultyRatiosClass _difficultyRatio;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [Category("Camera Scroll")]
        public DifficultyRatiosClass DifficultyRatio
        {
            get => _difficultyRatio ?? new DifficultyRatiosClass(this);
            set
            {
                _difficultyRatio = value;
                Data._difficultyRatio = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GFSREntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GFSREntry*) WorkingUncompressed.Address;
            _unknown0x08 = new TriggerDataClass(this, Data._unknown0x08);
            _unknown0x0C = new TriggerDataClass(this, Data._unknown0x0C);
            _difficultyRatio = new DifficultyRatiosClass(this, Data._difficultyRatio);

            if (_name == null)
            {
                _name = $"Camera Scroll [{Index}]";
            }

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GFSREntry* hdr = (GFSREntry*)address;
            Data._unknown0x08 = _unknown0x08;
            Data._unknown0x0C = _unknown0x0C;
            Data._difficultyRatio = _difficultyRatio;
            *hdr = Data;
        }
    }
}
