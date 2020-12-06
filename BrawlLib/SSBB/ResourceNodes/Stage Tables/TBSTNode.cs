namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBSTNode : TBNode
    {
        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBST" ? new TBSTNode() : null;
        }
    }
}