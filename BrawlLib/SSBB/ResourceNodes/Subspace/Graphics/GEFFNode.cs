using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Graphics;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GEFFNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GEFFEntryNode);
        protected override string baseName => "Effects";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GEFF" ? new GEFFNode() : null;
        }
    }

    public unsafe class GEFFEntryNode : ResourceNode
    {
        protected internal GEFFEntry Data;

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

        [DisplayName("Unknown0x04 (float)")]
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

        [DisplayName("Unknown0x08 (float)")]
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

        [DisplayName("Unknown0x0C (float)")]
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

        [DisplayName("Unknown0x10 (float)")]
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

        [DisplayName("Unknown0x14 (float)")]
        [Category("Unknown")]
        public float Unknown0x14
        {
            get => Data._unknown0x14;
            set
            {
                Data._unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x18 (bool)")]
        [Category("Unknown")]
        public bool Unknown0x18
        {
            get => Data._unknown0x18;
            set
            {
                Data._unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x19 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x19
        {
            get => Data._unknown0x19;
            set
            {
                Data._unknown0x19 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1A (byte)")]
        [Category("Unknown")]
        public byte Unknown0x1A
        {
            get => Data._unknown0x1A;
            set
            {
                Data._unknown0x1A = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1B (byte)")]
        [Category("Unknown")]
        public byte Unknown0x1B
        {
            get => Data._unknown0x1B;
            set
            {
                Data._unknown0x1B = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x1C;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x1C (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x1C
        {
            get => _unknown0x1C ?? new TriggerDataClass(this);
            set
            {
                _unknown0x1C = value;
                Data._unknown0x1C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x20 (short)")]
        [Category("Unknown")]
        public short Unknown0x20
        {
            get => Data._unknown0x20;
            set
            {
                Data._unknown0x20 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x22 (short)")]
        [Category("Unknown")]
        public short Unknown0x22
        {
            get => Data._unknown0x22;
            set
            {
                Data._unknown0x22 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x24 (uint)")]
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

        public override int OnCalculateSize(bool force)
        {
            return GEFFEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GEFFEntry*) WorkingUncompressed.Address;
            _unknown0x1C = new TriggerDataClass(this, Data._unknown0x1C);

            if (_name == null)
            {
                _name = $"Effect [{Index}]";
            }

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GEFFEntry* hdr = (GEFFEntry*)address;
            Data._unknown0x1C = _unknown0x1C;
            *hdr = Data;
        }
    }
}