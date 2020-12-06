namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBLVNode : TBNode
    {
        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBLV" ? new TBLVNode() : null;
        }
    }
}