using BrawlLib.Internal.IO;
using BrawlLib.SSBB.ResourceNodes;
using Microsoft.Scripting.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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

        private bool _converted;

        public void Execute()
        {
            try
            {
                Script.Execute(Scope);
            }
            catch (Exception e)
            {
                if (!_converted)
                {
                    _converted = true;
                    if (BrawlAPI.DepreciatedReplacementStrings.Keys.Any(s => e.Message.Contains(s)))
                    {
                        BrawlAPI.ConvertPlugin(Script.Path);
                        Execute();
                        return;
                    }
                }

                string msg = $"Error running plugin \"{Path.GetFileName(Script.Path)}\"\n{e.Message}";
                MessageBox.Show(msg, Path.GetFileName(Script.Path));
            }
        }
    }

    public class PluginResourceParser : ResourceNode
    {
        internal static ResourceNode TryParse(DataSource source)
        {
            ResourceNode n = null;
            using (UnsafeStream s = new UnsafeStream(source.Address, (uint) source.Length))
            {
                foreach (PluginResourceParser p in BrawlAPI.ResourceParsers)
                {
                    if ((n = p.TryParse(s)) != null)
                    {
                        break;
                    }
                }
            }

            return n;
        }

        public virtual ResourceNode TryParse(Stream stream)
        {
            return null;
        }
    }
}