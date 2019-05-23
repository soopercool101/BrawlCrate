using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RBNK)]
    public class RBNKWrapper : GenericWrapper
    {
        public override string ExportFilter { get { return FileFilters.RBNK; } }
    }
}
