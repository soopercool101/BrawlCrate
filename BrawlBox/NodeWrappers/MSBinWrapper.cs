using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.MSBin)]
    public class MSBinWrapper : GenericWrapper
    {
        public override string ExportFilter { get { return FileFilters.MSBin; } }
    }
}
