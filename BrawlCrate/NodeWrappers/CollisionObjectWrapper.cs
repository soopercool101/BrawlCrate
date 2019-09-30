using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.CollisionObj)]
    public class CollisionObjectWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.CollisionDef;
    }
}