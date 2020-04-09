using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Animation;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GMOTNode : BLOCEntryNode
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
        internal GMOTEntry* Header => (GMOTEntry*) WorkingUncompressed.Address;
        public override bool supportsCompression => false;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        [Category("Animated Object")]
        [DisplayName("Model Index")]
        public int ModelIndex => *(byte*) (WorkingUncompressed.Address + 0x3C);

        [Category("Animated Object")]
        [DisplayName("Collision Index")]
        public int CollisionIndex
        {
            get
            {
                int CID = *(byte*) (WorkingUncompressed.Address + 0x3D);
                if (CID == 0xFF)
                {
                    return -1;
                }

                return CID;
            }
        }

        [Category("Sound")]
        [DisplayName("Info Index")]
        public bint InfoIndex => *(bint*) (WorkingUncompressed.Address + 0x118);

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
            {
                _name = $"Object [{Index}]";
            }

            return false;
        }
    }
}