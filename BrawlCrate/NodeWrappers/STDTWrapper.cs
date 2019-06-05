using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.STDT)]
    public class STDTWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.STDT;
    }
}