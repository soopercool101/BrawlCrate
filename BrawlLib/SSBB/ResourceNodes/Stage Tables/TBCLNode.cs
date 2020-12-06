namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBCLNode : TBNode
    {
        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBCL" ? new TBCLNode() : null;
        }
    }
}