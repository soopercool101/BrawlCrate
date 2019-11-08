using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RBNK)]
    public class RBNKWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.RBNK;
    }
}