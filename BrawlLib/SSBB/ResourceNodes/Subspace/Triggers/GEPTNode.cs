using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes.Subspace.Triggers
{
    public class GEPTNode : BLOCEntryNode
    {
        protected override Type SubEntryType => typeof(GEPTEntryNode);

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GEPT" ? new GEPTNode() : null;
        }
    }

    public unsafe class GEPTEntryNode : ResourceNode
    {
        internal GEPTEntry* Header => (GEPTEntry*)WorkingUncompressed.Address;
        internal GEPTEntry Data;
        public override bool supportsCompression => false;

        [Category("GEPT Entry")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint Trigger1
        {
            get => Data._trigger1;
            set
            {
                Data._trigger1 = value;
                SignalPropertyChange();
            }
        }

        [Category("GEPT Entry")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint Trigger2
        {
            get => Data._trigger2;
            set
            {
                Data._trigger2 = value;
                SignalPropertyChange();
            }
        }

        [Category("GEPT Entry")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint Trigger3
        {
            get => Data._trigger3;
            set
            {
                Data._trigger3 = value;
                SignalPropertyChange();
            }
        }

        [Category("GEPT Entry")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint Trigger4
        {
            get => Data._trigger4;
            set
            {
                Data._trigger4 = value;
                SignalPropertyChange();
            }
        }

        [Category("GEPT Entry")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint Trigger5
        {
            get => Data._trigger5;
            set
            {
                Data._trigger5 = value;
                SignalPropertyChange();
            }
        }

        [Category("GEPT Entry")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint Trigger6
        {
            get => Data._trigger6;
            set
            {
                Data._trigger6 = value;
                SignalPropertyChange();
            }
        }

        [Category("GEPT Entry")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint Trigger7
        {
            get => Data._trigger7;
            set
            {
                Data._trigger7 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GEPTEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *Header;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GEPTEntry* hdr = (GEPTEntry*)address;
            *hdr = Data;
        }
    }
}
