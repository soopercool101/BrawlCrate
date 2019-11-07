using BrawlLib;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers.Audio
{
    [NodeWrapper(ResourceType.RSEQ)]
    public class RSEQWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.RSEQ;
    }
}