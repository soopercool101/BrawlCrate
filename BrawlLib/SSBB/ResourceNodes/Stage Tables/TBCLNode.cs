using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Stage_Tables;
using System.ComponentModel;

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