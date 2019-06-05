using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BrawlLib.IO;
using BrawlLib.Wii.Compression;

namespace BrawlLib.SSBB.ResourceNodes
{
    internal delegate ResourceNode ResourceParser(DataSource source);

    //Factory is for initializing root node, and unknown child nodes.
    public static class NodeFactory
    {
        //#if DEBUG
        //        private const bool UseRawDataNode = true;
        //#else
        private const bool UseRawDataNode = false;
        //#endif

        private static readonly List<ResourceParser> _parsers = new List<ResourceParser>();

        private static readonly Dictionary<string, Type> Forced = new Dictionary<string, Type>
        {
            {"MRG", typeof(MRGNode)},
            {"MRGC", typeof(MRGNode)}, //Compressed MRG
            {"DOL", typeof(DOLNode)},
            {"REL", typeof(RELNode)}
        };

        static NodeFactory()
        {
            Delegate del;
            foreach (var t in Assembly.GetExecutingAssembly().GetTypes())
                if (t.IsSubclassOf(typeof(ResourceNode)))
                    if ((del = Delegate.CreateDelegate(typeof(ResourceParser), t, "TryParse", false, false)) != null)
                        _parsers.Add(del as ResourceParser);

            foreach (var t in Assembly.GetEntryAssembly().GetTypes())
                if (t.IsSubclassOf(typeof(ResourceNode)))
                    if ((del = Delegate.CreateDelegate(typeof(ResourceParser), t, "TryParse", false, false)) != null)
                        _parsers.Add(del as ResourceParser);
        }

        //Parser commands must initialize the node before returning.
        public static ResourceNode FromFile(ResourceNode parent, string path,
            FileOptions options = FileOptions.RandomAccess)
        {
            ResourceNode node = null;
            var map = FileMap.FromFile(path, FileMapProtect.Read, 0, 0, options);
            try
            {
                var source = new DataSource(map);
                if ((node = FromSource(parent, source)) == null)
                {
                    var ext = path.Substring(path.LastIndexOf('.') + 1).ToUpper();
                    if (Forced.ContainsKey(ext))
                    {
                        node = Activator.CreateInstance(Forced[ext]) as ResourceNode;
                        var uncomp = Compressor.TryExpand(ref source, false);
                        if (uncomp != null)
                            node.Initialize(parent, source, new DataSource(uncomp));
                        else
                            node.Initialize(parent, source);
                    }

                    //else if (UseRawDataNode)
                    //{
                    //    (node = new RawDataNode(Path.GetFileNameWithoutExtension(path))).Initialize(parent, source);
                    //}
                }
            }
            finally
            {
                if (node == null) map.Dispose();
            }

            return node;
        }

        public static ResourceNode FromAddress(ResourceNode parent, VoidPtr address, int length)
        {
            return FromSource(parent, new DataSource(address, length));
        }

        public static ResourceNode FromSource(ResourceNode parent, DataSource source)
        {
            ResourceNode n = null;
            if ((n = GetRaw(source)) != null)
            {
                n.Initialize(parent, source);
            }
            else
            {
                var uncomp = Compressor.TryExpand(ref source);
                DataSource d;
                if (uncomp != null && (n = GetRaw(d = new DataSource(uncomp))) != null) n.Initialize(parent, source, d);
            }

            return n;
        }

        public static ResourceNode GetRaw(VoidPtr address, int length)
        {
            return GetRaw(new DataSource(address, length));
        }

        public static ResourceNode GetRaw(DataSource source)
        {
            ResourceNode n = null;
            foreach (var d in _parsers)
                if ((n = d(source)) != null)
                    break;

            return n;
        }
    }
}