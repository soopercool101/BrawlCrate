using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Animation;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GQUKNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GQUKEntryNode);
        protected override string baseName => "Quakes";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GQUK" ? new GQUKNode() : null;
        }
    }

    public unsafe class GQUKEntryNode : ResourceNode
    {
        protected internal GQUKEntry Data;

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

        [DisplayName("Unknown0x0C (short)")]
        [Category("Unknown")]
        public short Unknown0x0C
        {
            get => Data._unknown0x0C;
            set
            {
                Data._unknown0x0C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0E (short)")]
        [Category("Unknown")]
        public short Unknown0x0E
        {
            get => Data._unknown0x0E;
            set
            {
                Data._unknown0x0E = value;
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

        [DisplayName("Unknown0x14 (int)")]
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

        public override int OnCalculateSize(bool force)
        {
            return GQUKEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GQUKEntry*) WorkingUncompressed.Address;
            _unknown0x10 = new TriggerDataClass(this, Data._unknown0x10);

            if (_name == null)
            {
                _name = $"Quake [{Index}]";
            }

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GQUKEntry* hdr = (GQUKEntry*)address;
            Data._unknown0x10 = _unknown0x10;
            *hdr = Data;
        }
    }
}