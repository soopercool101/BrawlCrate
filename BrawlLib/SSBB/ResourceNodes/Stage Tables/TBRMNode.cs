namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBRMNode : TBNode
    {
        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBRM" ? new TBRMNode() : null;
        }
    }
}