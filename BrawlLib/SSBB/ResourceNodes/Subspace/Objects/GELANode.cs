using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GELANode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GELAEntryNode);
        protected override string baseName => "Elasticity";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GELA" ? new GELANode() : null;
        }
    }

    public unsafe class GELAEntryNode : ResourceNode
    {
        protected internal GELAEntry Data;

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

        [DisplayName("Unknown0x10 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x10
        {
            get => Data._unknown0x10;
            set
            {
                Data._unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x11 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x11
        {
            get => Data._unknown0x11;
            set
            {
                Data._unknown0x11 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x12 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x12
        {
            get => Data._unknown0x12;
            set
            {
                Data._unknown0x12 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x13 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x13
        {
            get => Data._unknown0x13;
            set
            {
                Data._unknown0x13 = value;
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

        [DisplayName("Unknown0x18 (int)")]
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

        [DisplayName("Unknown0x1C (byte)")]
        [Category("Unknown")]
        public byte Unknown0x1C
        {
            get => Data._unknown0x1C;
            set
            {
                Data._unknown0x1C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1D (byte)")]
        [Category("Unknown")]
        public byte Unknown0x1D
        {
            get => Data._unknown0x1D;
            set
            {
                Data._unknown0x1D = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1E (byte)")]
        [Category("Unknown")]
        public byte Unknown0x1E
        {
            get => Data._unknown0x1E;
            set
            {
                Data._unknown0x1E = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1F (byte)")]
        [Category("Unknown")]
        public byte Unknown0x1F
        {
            get => Data._unknown0x1F;
            set
            {
                Data._unknown0x1F = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x20 (int)")]
        [Category("Unknown")]
        public int Unknown0x20
        {
            get => Data._unknown0x20;
            set
            {
                Data._unknown0x20 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x24 (bool)")]
        [Category("Unknown")]
        public bool Unknown0x24
        {
            get => Data._unknown0x24;
            set
            {
                Data._unknown0x24 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x25 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x25
        {
            get => Data._unknown0x25;
            set
            {
                Data._unknown0x25 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x26 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x26
        {
            get => Data._unknown0x26;
            set
            {
                Data._unknown0x26 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x27 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x27
        {
            get => Data._unknown0x27;
            set
            {
                Data._unknown0x27 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x28;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x28 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x28
        {
            get => _unknown0x28 ?? new TriggerDataClass(this);
            set
            {
                _unknown0x28 = value;
                Data._unknown0x28 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GELAEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GELAEntry*) WorkingUncompressed.Address;
            _unknown0x28 = new TriggerDataClass(this, Data._unknown0x28);

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GELAEntry* hdr = (GELAEntry*)address;
            Data._unknown0x28 = _unknown0x28;
            *hdr = Data;
        }
    }
}