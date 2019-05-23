using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSTM)]
    public class RSTMWrapper : GenericWrapper
    {
        public override string ExportFilter { get { return FileFilters.RSTM; } }
    }
}
