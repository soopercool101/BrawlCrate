using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.FCFG)]
    public class FCFGWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.FCFG;
    }
}