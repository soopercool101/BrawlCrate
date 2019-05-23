using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.PLT0)]
    public class PLT0Wrapper : GenericWrapper
    {
        public override string ExportFilter { get { return FileFilters.PLT0; } }
    }
}
