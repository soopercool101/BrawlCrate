using BrawlLib.SSBB.ResourceNodes;
using Microsoft.Scripting.Hosting;
using System.IO;

namespace BrawlCrate.API
{
    public class PluginScript
    {
        public PluginScript(string name, ScriptSource script, ScriptScope scope)
        {
            Name = name;
            Script = script;
            Scope = scope;
        }

        public string Name { get; set; }
        public ScriptSource Script { get; set; }
        public ScriptScope Scope { get; set; }

        public void Execute()
        {
            Script.Execute(Scope);
        }
    }

    public class PluginLoader : ResourceNode
    {
        internal static ResourceNode TryParse(DataSource source)
        {
            ResourceNode n = null;
            foreach (PluginLoader ldr in bboxapi.Loaders)
            {
                if ((n = ldr.TryParse(new UnsafeStream(source.Address, (uint)source.Length))) != null)
                {
                    break;
                }
            }

            return n;

        }

        public virtual ResourceNode TryParse(Stream stream) { return null; }
    }
}
