using BrawlLib.IO;
using BrawlLib.Wii.Compression;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace BrawlLib.SSBB.ResourceNodes
{
    internal delegate ResourceNode ResourceParser(DataSource source);

    //Factory is for initializing root node, and unknown child nodes.
    public static class NodeFactory
    {
        private static readonly List<ResourceParser> _parsers = new List<ResourceParser>();

        private static readonly Dictionary<string, Type> Forced = new Dictionary<string, Type>
        {
            {"MRG", typeof(MRGNode)},
            {"MRGC", typeof(MRGNode)}, //Compressed MRG
            {"DOL", typeof(DOLNode)},
            {"REL", typeof(RELNode)},
            {"MASQ", typeof(MasqueradeNode)}
        };

        static NodeFactory()
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                AddParser(t);
            }

            foreach (Type t in Assembly.GetEntryAssembly()?.GetTypes())
            {
                AddParser(t);
            }
        }

        public static void AddParser(Type t)
        {
            if (t.IsSubclassOf(typeof(ResourceNode)))
            {
                Delegate del = Delegate.CreateDelegate(typeof(ResourceParser), t, "TryParse", false, false);
                if (del != null)
                {
                    _parsers.Add(del as ResourceParser);
                }
            }
        }

        //Parser commands must initialize the node before returning.
        public static ResourceNode FromFile(ResourceNode parent, string path)
        {
            return FromFile(parent, path, FileOptions.RandomAccess);
        }

        public static ResourceNode FromFile(ResourceNode parent, string path, FileOptions options)
        {
            ResourceNode node = null;
            FileMap map = FileMap.FromFile(path, FileMapProtect.Read, 0, 0, options);
            try
            {
                DataSource source = new DataSource(map);
                if ((node = FromSource(parent, source)) == null)
                {
                    string ext = path.Substring(path.LastIndexOf('.') + 1).ToUpper(CultureInfo.InvariantCulture);

                    if (Forced.ContainsKey(ext) &&
                        (node = Activator.CreateInstance(Forced[ext]) as ResourceNode) != null)
                    {
                        FileMap uncompressedMap = Compressor.TryExpand(ref source, false);
                        if (uncompressedMap != null)
                        {
                            node.Initialize(parent, source, new DataSource(uncompressedMap));
                        }
                        else
                        {
                            node.Initialize(parent, source);
                        }
                    }
#if DEBUG
                    else
                    {
                        node = new RawDataNode(Path.GetFileNameWithoutExtension(path));
                        node.Initialize(parent, source);
                    }
#endif
                }
            }
            finally
            {
                if (node == null)
                {
                    map.Dispose();
                }
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
                FileMap uncomp = Compressor.TryExpand(ref source);
                if (uncomp != null)
                {
                    DataSource d = new DataSource(uncomp);
                    n = GetRaw(d);
                    n?.Initialize(parent, source, d);
                }
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
            {
                if ((n = d(source)) != null)
                {
                    break;
                }
            }

            return n;
        }
    }
}