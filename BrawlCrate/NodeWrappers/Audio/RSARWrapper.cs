using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSAR)]
    public class RSARWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.RSAR;
    }
}