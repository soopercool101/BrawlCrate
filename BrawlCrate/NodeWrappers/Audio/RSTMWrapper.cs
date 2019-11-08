using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSTM)]
    public class RSTMWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.RSTM;
    }
}