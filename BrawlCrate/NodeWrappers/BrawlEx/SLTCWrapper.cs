using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.SLTC)]
    public class SLTCWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.SLTC;
    }
}