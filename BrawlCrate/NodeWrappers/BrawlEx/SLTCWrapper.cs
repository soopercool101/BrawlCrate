using BrawlLib;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers.BrawlEx
{
    [NodeWrapper(ResourceType.SLTC)]
    internal class SLTCWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.SLTC;
    }
}