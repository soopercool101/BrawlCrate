namespace BrawlCrate.NodeWrappers
{
    public class PluginWrapper : GenericWrapper
    {
        public virtual BaseWrapper GetInstance()
        {
            return new GenericWrapper();
        }
    }
}
