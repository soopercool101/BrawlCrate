namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBGCNode : TBNode
    {
        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBGC" ? new TBGCNode() : null;
        }
    }
}