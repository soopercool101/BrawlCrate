using BrawlCrate.NodeWrappers;

namespace BrawlCrate.API
{
    public class PluginWrapper : GenericWrapper
    {
        public virtual BaseWrapper GetInstance()
        {
            return new GenericWrapper();
        }
    }
}