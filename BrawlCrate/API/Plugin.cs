using BrawlLib.Internal.IO;
using BrawlLib.SSBB.ResourceNodes;
using Microsoft.Scripting.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlCrate.API
{
    internal class PluginScript
    {
        internal PluginScript(string name, ScriptSource script, ScriptScope scope)
        {
            Name = name;
            Script = script;
            Scope = scope;
        }

        internal string Name { get; set; }
        internal ScriptSource Script { get; set; }
        internal ScriptScope Scope { get; set; }

        private bool _converted;

        internal void Execute()
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
                    if (BrawlAPIInternal.DepreciatedReplacementStrings.Keys.Any(s => e.Message.Contains(s)))
                    {
                        BrawlAPIInternal.ConvertPlugin(Script.Path);
                        Execute();
                        return;
                    }
                }

                string msg = $"Error running plugin \"{Path.GetFileName(Script.Path)}\"\n{e.Message}";
                MessageBox.Show(msg, Path.GetFileName(Script.Path));
            }
        }
    }

    /// <summary>
    ///   Internal class used as a proxy for plugin-defined resource node parsers
    /// </summary>
    internal class PluginResourceParsers : ResourceNode
    {
        internal static ResourceNode TryParse(DataSource source)
        {
            ResourceNode n = null;
            using (UnsafeStream s = new UnsafeStream(source.Address, (uint) source.Length))
            {
                foreach (PluginResourceParser p in BrawlAPIInternal.ResourceParsers)
                {
                    if ((n = p.TryParse(s)) != null)
                    {
                        break;
                    }
                }
            }

            return n;
        }
    }

    /// <summary>
    ///   An interface which can be used to define plugin-parsed resource nodes.
    ///   Nodes must be derived from this and ResourceNode or a derivative of such.
    /// </summary>
    public interface PluginResourceParser
    {
        ResourceNode TryParse(Stream stream);
    }
}