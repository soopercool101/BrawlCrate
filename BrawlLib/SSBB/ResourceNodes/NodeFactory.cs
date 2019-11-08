using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using BrawlLib.Wii;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Linq;

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
            {"MASQ", typeof(MasqueradeNode)},
            {"CMM", typeof(CMMNode)}
        };

        static NodeFactory()
        {
            // Add any BrawlCrate-side parsers (currently only BrawlAPI stuff)
            foreach (Type t in Assembly.GetEntryAssembly()?.GetTypes()
                                       ?.Where(t => t.IsSubclassOf(typeof(ResourceNode))))
            {
                AddParser(t);
            }

            // Add all BrawlLib parsers (excluding MoveDefs, as explained below)
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()
                                       .Where(t => t.IsSubclassOf(typeof(ResourceNode)) && t != typeof(MoveDefNode)))
            {
                AddParser(t);
            }

            // Add MoveDef. MoveDef is very generalized as a parser and currently encounters many false positives.
            // Prevent this by trying everything else first
            AddParser(typeof(MoveDefNode));
        }

        public static void AddParser(Type t)
        {
            Delegate del = Delegate.CreateDelegate(typeof(ResourceParser), t, "TryParse", false, false);
            if (del != null)
            {
                _parsers.Add(del as ResourceParser);
            }
        }

        //Parser commands must initialize the node before returning.
        public static ResourceNode FromFile(ResourceNode parent, string path)
        {
            return FromFile(parent, path, FileOptions.RandomAccess, null);
        }

        public static ResourceNode FromFile(ResourceNode parent, string path, Type t)
        {
            return FromFile(parent, path, FileOptions.RandomAccess, t);
        }

        public static ResourceNode FromFile(ResourceNode parent, string path, FileOptions options)
        {
            return FromFile(parent, path, options, null);
        }

        public static ResourceNode FromFile(ResourceNode parent, string path, FileOptions options, Type t)
        {
            ResourceNode node = null;
            FileMap map = FileMap.FromFile(path, FileMapProtect.Read, 0, 0, options);
            try
            {
                DataSource source = new DataSource(map);
                if ((node = FromSource(parent, source)) == null)
                {
                    string ext = path.Substring(path.LastIndexOf('.') + 1).ToUpper(CultureInfo.InvariantCulture);

                    if (!(t is null) && (node = Activator.CreateInstance(t) as ResourceNode) != null
                        || Forced.ContainsKey(ext) &&
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
                    else
                    {
                        node = new RawDataNode(Path.GetFileName(path));
                        node.Initialize(parent, source);
                    }
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
            return FromSource(parent, source, null);
        }

        public static ResourceNode FromSource(ResourceNode parent, DataSource source, Type t)
        {
            ResourceNode n = null;

            if (t != null && (n = Activator.CreateInstance(t) as ResourceNode) != null)
            {
                FileMap uncompressedMap = Compressor.TryExpand(ref source, false);
                if (uncompressedMap != null)
                {
                    n.Initialize(parent, source, new DataSource(uncompressedMap));
                }
                else
                {
                    n.Initialize(parent, source);
                }
            }
            else if ((n = GetRaw(source)) != null)
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

        public static ResourceNode FromFolder(ResourceNode parent, string path)
        {
            FolderNode node = new FolderNode();
            node._origPath = path;
            node.Initialize(parent, new VoidPtr(), 0);
            node.OnPopulate();
            return node;
        }
    }
}