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
        internal PluginScript(string path, CompiledCode script, ScriptScope scope)
        {
            ScriptPath = path;
            Name = Path.GetFileNameWithoutExtension(path);
            Code = script;
            Scope = scope;
        }

        internal string ScriptPath { get; set; }
        internal string Name { get; set; }
        internal CompiledCode Code { get; set; }
        internal ScriptScope Scope { get; set; }

        private bool _converted;

        internal void Execute()
        {
            if (BrawlAPIInternal.Engine == null)
                return;
            try
            {
#if DEBUG
                Code = BrawlAPIInternal.Engine.CreateScriptSourceFromFile(ScriptPath).Compile();
#endif
                Code.Execute(Scope);
            }
            catch (Exception e)
            {
                if (!_converted)
                {
                    _converted = true;
                    if (BrawlAPIInternal.DepreciatedReplacementStrings.Keys.Any(s => e.Message.Contains(s)))
                    {
                        BrawlAPIInternal.ConvertPlugin(ScriptPath);
                        Code = BrawlAPIInternal.Engine.CreateScriptSourceFromFile(ScriptPath).Compile();
                        Execute();
                        return;
                    }
                }

                string msg = $"Error running plugin \"{ScriptPath}\"\n{e.Message}";
                MessageBox.Show(msg, Name);
            }
        }
    }

    /// <summary>
    ///   Internal class used as a proxy for plugin-defined resource node parsers
    /// </summary>
    internal class PluginResourceParsers : ResourceNode
    {
        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
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