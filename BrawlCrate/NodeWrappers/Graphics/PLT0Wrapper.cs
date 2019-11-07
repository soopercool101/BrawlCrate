using BrawlLib;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers.Graphics
{
    [NodeWrapper(ResourceType.PLT0)]
    public class PLT0Wrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.PLT0;
    }
}