using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.RWSD)]
    public class RWSDWrapper : GenericWrapper
    {
        public override string ExportFilter { get { return FileFilters.RWSD; } }
    }
}
