using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSAR)]
    public class RSARWrapper : GenericWrapper
    {
        public override string ExportFilter { get { return FileFilters.RSAR; } }
    }
}
