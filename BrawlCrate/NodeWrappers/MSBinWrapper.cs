using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.MSBin)]
    public class MSBinWrapper : GenericWrapper
    {
        public override string ExportFilter { get { return FileFilters.MSBin; } }
    }
}
