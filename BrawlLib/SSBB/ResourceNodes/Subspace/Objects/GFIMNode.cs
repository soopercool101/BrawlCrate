using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GFIMNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GFIMEntryNode);
        protected override string baseName => "Fixed Item Spawns";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GFIM" ? new GFIMNode() : null;
        }
    }

    public unsafe class GFIMEntryNode : ResourceNode
    {
        protected internal GFIMEntry Data;

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

        public TriggerDataClass _unknown0x10;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x10 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x10
        {
            get => _unknown0x10 ?? new TriggerDataClass(this);
            set
            {
                _unknown0x10 = value;
                Data._unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x14 (short)")]
        [Category("Unknown")]
        public short Unknown0x14
        {
            get => Data._unknown0x14;
            set
            {
                Data._unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x16 (short)")]
        [Category("Unknown")]
        public short Unknown0x16
        {
            get => Data._unknown0x16;
            set
            {
                Data._unknown0x16 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GFIMEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GFIMEntry*) WorkingUncompressed.Address;
            _unknown0x10 = new TriggerDataClass(this, Data._unknown0x10);

            if (_name == null)
            {
                _name = $"Fixed Item [{Index}]";
            }

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GFIMEntry* hdr = (GFIMEntry*)address;
            Data._unknown0x10 = _unknown0x10;
            *hdr = Data;
        }
    }
}