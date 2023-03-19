using BrawlLib.Internal;
using BrawlLib.SSBB.Types;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RASDNode : ARCEntryNode
    {
        internal RASD* Header => (RASD*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        //[Category("RASD")]
        //public int Entries { get { return Header->_numEntries; } }

        //RASD is found in "External" bres group nodes
        public override bool OnInitialize()
        {
            base.OnInitialize();
            //SetSizeInternal(Header->_header._length);
            return false;
        }

        public override void OnPopulate()
        {
            //DATA Entries
        }

        public override int OnCalculateSize(bool force)
        {
            return base.OnCalculateSize(force);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((RASD*) source.Address)->_header._tag == RASD.Tag ? new RASDNode() : null;
        }
    }

    //public unsafe class RASDEntryNode : ResourceNode
    //{
    //    internal RASDDataEntry* Header { get { return (RASDDataEntry*)WorkingUncompressed.Address; } }

    //    public override bool OnInitialize()
    //    {
    //        return false;
    //    }
    //}
}