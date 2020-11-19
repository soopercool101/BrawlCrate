using BrawlLib.SSBB.Types.Subspace.Animation;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GMOVNode : BLOCEntryNode
    {
        protected override Type SubEntryType => typeof(GMOVEntryNode);
        public override ResourceType ResourceFileType => ResourceType.GMOV;
        protected override string baseName => "Movable Platforms";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GMOV" ? new GMOVNode() : null;
        }
    }

    public unsafe class GMOVEntryNode : ResourceNode
    {
        internal GMOVEntry* Header => (GMOVEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        [Category("Movable Platform")]
        [DisplayName("Model Index")]
        public int ModelIndex => *(byte*) (WorkingUncompressed.Address + 0x44);

        [Category("Movable Platform")]
        [DisplayName("Collision Index")]
        public int CollisionIndex
        {
            get
            {
                int CID = *(byte*) (WorkingUncompressed.Address + 0x45);
                if (CID == 0xFF)
                {
                    return -1;
                }

                return CID;
            }
        }

        [Category("Movable Platform")]
        [DisplayName("Path Index")]
        public int PathIndex => *(byte*) (WorkingUncompressed.Address + 0x06);

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
            {
                _name = "Platform [" + Index + ']';
            }

            return false;
        }
    }
}