using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.COSC)]
    internal class COSCWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.COSC;
    }
}