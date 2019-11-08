using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSEQ)]
    public class RSEQWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.RSEQ;
    }
}