using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.COSC)]
    public class COSCWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.COSC;
    }
}