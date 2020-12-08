using BrawlLib.SSBB.Types.Subspace.Objects;
using System;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GMPSNode : BLOCEntryNode
    {
        protected override Type SubEntryType => typeof(GMPSEntryNode);
        public override ResourceType ResourceFileType => ResourceType.GMPS;
        protected override string baseName => "Trackballs";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GMPS" ? new GMPSNode() : null;
        }
    }

    public unsafe class GMPSEntryNode : ResourceNode
    {
        internal GMPSEntry* Header => (GMPSEntry*) WorkingUncompressed.Address;

        public override ResourceType ResourceFileType => ResourceType.Unknown;
        //[Category("Animated Object")]
        //[DisplayName("Model Index")]
        //public int ModelIndex { get { return *(byte*)(WorkingUncompressed.Address + 0x3C); } }

        //[Category("Animated Object")]
        //[DisplayName("Collision Index")]
        //public int CollisionIndex
        //{
        //    get
        //    {
        //        int CollisionIndex = *(byte*)(WorkingUncompressed.Address + 0x3D);
        //        if (CollisionIndex == 0xFF)
        //            return -1;
        //        else
        //            return CollisionIndex;
        //    }
        //}

        //[Category("Sound")]
        //[DisplayName("Info Index")]
        //public bint InfoIndex { get { return *(bint*)(WorkingUncompressed.Address + 0x118); } }
        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
            {
                _name = new string((sbyte*) (Header + 0xFC));
            }

            return false;
        }
    }
}