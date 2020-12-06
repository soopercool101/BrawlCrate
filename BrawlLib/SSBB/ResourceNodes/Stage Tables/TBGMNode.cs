namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBGMNode : TBNode
    {
        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBGM" ? new TBGMNode() : null;
        }
    }
}