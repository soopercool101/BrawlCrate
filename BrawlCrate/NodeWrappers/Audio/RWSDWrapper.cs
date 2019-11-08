using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RWSD)]
    public class RWSDWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.RWSD;
    }
}