using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting;
using IronPython.Runtime.Exceptions;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;

namespace BrawlBox.API
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
            foreach (var ldr in bboxapi.Loaders)
                if ((n = ldr.TryParse(new UnsafeStream(source.Address, (uint)source.Length))) != null)
                    break;
            return n;

        }

        virtual public ResourceNode TryParse(Stream stream) { return null; }
    }
}
