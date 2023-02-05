using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Animation;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GMOTNode : BLOCEntryNode
    {
        protected override Type SubEntryType => typeof(GMOTEntryNode);
        public override ResourceType ResourceFileType => ResourceType.GMOT;
        protected override string baseName => "Animated Objects";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GMOT" ? new GMOTNode() : null;
        }
    }

    public unsafe class GMOTEntryNode : ResourceNode
    {
        internal GMOTEntry Data;
        internal GMOTEntry* Header => (GMOTEntry*) WorkingUncompressed.Address;
        public override bool supportsCompression => false;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        [Category("Animated Object")]
        [DisplayName("Model Index")]
        public byte ModelIndex
        {
            get => Data._modelDataIndex;
            set
            {
                Data._modelDataIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("Animated Object")]
        [DisplayName("Collision Index")]
        public byte CollisionIndex
        {
            get => Data._collisionDataIndex;
            set
            {
                Data._collisionDataIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("Animated Object")]
        [DisplayName("Info Index")]
        public int SoundInfoIndex
        {
            get => Data._soundInfoIndex;
            set
            {
                Data._soundInfoIndex = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GMOTEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *Header;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GMOTEntry* hdr = (GMOTEntry*)address;
            *hdr = Data;
        }
    }
}