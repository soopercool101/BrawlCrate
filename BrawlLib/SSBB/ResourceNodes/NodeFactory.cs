using System;
using System.Reflection;
using System.Collections.Generic;
using BrawlLib.IO;
using BrawlLib.Wii.Compression;
using System.IO;

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
        
        private static List<ResourceParser> _parsers = new List<ResourceParser>();
        static NodeFactory()
        {
            Delegate del;
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
                if (t.IsSubclassOf(typeof(ResourceNode)))
                    if ((del = Delegate.CreateDelegate(typeof(ResourceParser), t, "TryParse", false, false)) != null)
                        _parsers.Add(del as ResourceParser);

            foreach(Type t in Assembly.GetEntryAssembly().GetTypes())
            {
                if (t.IsSubclassOf(typeof(ResourceNode)))
                    if ((del = Delegate.CreateDelegate(typeof(ResourceParser), t, "TryParse", false, false)) != null)
                        _parsers.Add(del as ResourceParser);
            }
        }

        private static readonly Dictionary<string, Type> Forced = new Dictionary<string, Type>()
        {
            { "MRG", typeof(MRGNode) },
            { "MRGC", typeof(MRGNode) }, //Compressed MRG
            { "DOL", typeof(DOLNode) },
            { "REL", typeof(RELNode) },
        };

        //Parser commands must initialize the node before returning.
        public unsafe static ResourceNode FromFile(ResourceNode parent, string path, FileOptions options = FileOptions.RandomAccess)
        {
            ResourceNode node = null;
            FileMap map = FileMap.FromFile(path, FileMapProtect.Read, 0, 0, options);
            try
            {
                DataSource source = new DataSource(map);
                if ((node = FromSource(parent, source)) == null)
                {
                    string ext = path.Substring(path.LastIndexOf('.') + 1).ToUpper();
                    if (Forced.ContainsKey(ext))
                    {
                        node = Activator.CreateInstance(Forced[ext]) as ResourceNode;
                        FileMap uncomp = Compressor.TryExpand(ref source, false);
                        if (uncomp != null)
                            node.Initialize(parent, source, new DataSource(uncomp));
                        else
                            node.Initialize(parent, source);
                    }
                    else if (UseRawDataNode)
                        (node = new RawDataNode(Path.GetFileNameWithoutExtension(path))).Initialize(parent, source);
                }
            }
            finally
            {
                if (node == null)
                    map.Dispose();
            }
            return node;
        }
        public static ResourceNode FromAddress(ResourceNode parent, VoidPtr address, int length)
        {
            return FromSource(parent, new DataSource(address, length));
        }
        public static unsafe ResourceNode FromSource(ResourceNode parent, DataSource source)
        {
            ResourceNode n = null;
            if ((n = GetRaw(source)) != null)
                n.Initialize(parent, source);
            else
            {
                FileMap uncomp = Compressor.TryExpand(ref source);
                DataSource d;
                if (uncomp != null && (n = NodeFactory.GetRaw(d = new DataSource(uncomp))) != null)
                    n.Initialize(parent, source, d);
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
            foreach (ResourceParser d in _parsers)
                if ((n = d(source)) != null)
                    break;
            return n;
        }
    }
}
