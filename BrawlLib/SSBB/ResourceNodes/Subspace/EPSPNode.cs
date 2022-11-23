using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlLib.SSBB.ResourceNodes.Subspace
{
    public class EPSPNode : BLOCEntryNode
    {
        protected override Type SubEntryType => typeof(TyDataNode);
        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "EPSP" ? new EPSPNode() : null;
        }
    }
}
