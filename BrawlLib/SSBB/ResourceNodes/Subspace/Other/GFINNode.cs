using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Hazards;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GFINNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GFINEntryNode);
        protected override string baseName => "Final Destination Gimmicks";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GFIN" ? new GFINNode() : null;
        }
    }

    public unsafe class GFINEntryNode : ResourceNode
    {
        protected internal GFINEntry Data;

        [Category("GFIN")]
        public byte BackgroundModelData
        {
            get => Data._backgroundModelData;
            set
            {
                Data._backgroundModelData = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x01
        {
            get => Data._unknown0x01;
            set
            {
                Data._unknown0x01 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x02
        {
            get => Data._unknown0x02;
            set
            {
                Data._unknown0x02 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x03
        {
            get => Data._unknown0x03;
            set
            {
                Data._unknown0x03 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            Data = *(GFINEntry*)WorkingUncompressed.Address;

            return base.OnInitialize();
        }

        public override int OnCalculateSize(bool force)
        {
            return GFINEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GFINEntry* header = (GFINEntry*)address;
            *header = Data;
        }
    }
}
