using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.MSBin)]
    public class MSBinWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.MSBin;
    }
}