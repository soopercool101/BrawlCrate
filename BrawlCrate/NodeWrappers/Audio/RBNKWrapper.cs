using BrawlLib;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers.Audio
{
    [NodeWrapper(ResourceType.RBNK)]
    public class RBNKWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.RBNK;
    }
}