namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBGDNode : TBNode
    {
        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBGD" ? new TBGDNode() : null;
        }
    }
}