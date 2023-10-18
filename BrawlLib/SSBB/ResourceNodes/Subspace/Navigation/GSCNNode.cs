using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Navigation;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GSCNNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GSCNEntryNode);
        protected override string baseName => "Scene Changes";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GSCN" ? new GSCNNode() : null;
        }
    }

    public unsafe class GSCNEntryNode : ResourceNode
    {
        protected internal GSCNEntry Data;

        [DisplayName("Unknown0x00 (short)")]
        [Category("Unknown")]
        public short Unknown0x00
        {
            get => Data._unknown0x00;
            set
            {
                Data._unknown0x00 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x02 (short)")]
        [Category("Unknown")]
        public short Unknown0x02
        {
            get => Data._unknown0x02;
            set
            {
                Data._unknown0x02 = value;
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

        [DisplayName("Unknown0x08 (int)")]
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

        [DisplayName("Unknown0x0C (int)")]
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

        public TriggerDataClass _unknown0x18;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x18 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x18
        {
            get => _unknown0x18 ?? new TriggerDataClass(this);
            set
            {
                _unknown0x18 = value;
                Data._unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GSCNEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GSCNEntry*) WorkingUncompressed.Address;
            _unknown0x18 = new TriggerDataClass(this, Data._unknown0x18);

            if (_name == null)
            {
                _name = $"Scene Change [{Index}]";
            }

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GSCNEntry* hdr = (GSCNEntry*)address;
            Data._unknown0x18 = _unknown0x18;
            *hdr = Data;
        }
    }
}