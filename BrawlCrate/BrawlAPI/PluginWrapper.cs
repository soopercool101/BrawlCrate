using BrawlCrate.NodeWrappers;

namespace BrawlCrate.BrawlAPI
{
    public class PluginWrapper : GenericWrapper
    {
        public virtual BaseWrapper GetInstance()
        {
            return new GenericWrapper();
        }
    }
}
