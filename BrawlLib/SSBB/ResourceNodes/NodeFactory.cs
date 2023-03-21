using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using BrawlLib.SSBB.ResourceNodes.Subspace.SSEEX;
using BrawlLib.Wii;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    internal delegate ResourceNode ResourceParser(DataSource source, ResourceNode parent);

    //Factory is for initializing root node, and unknown child nodes.
    public static class NodeFactory
    {
        private static readonly List<ResourceParser> _parsers = new List<ResourceParser>();
        private static readonly List<ResourceParser> _parsersGeneric = new List<ResourceParser>();

        private static readonly Dictionary<string, Type> ForcedExtensions = new Dictionary<string, Type>
        {
            {"MRG", typeof(MRGNode)},
            {"MRGC", typeof(MRGNode)}, //Compressed MRG
            {"DOL", typeof(DOLNode)},
            {"REL", typeof(RELNode)},
            {"MASQ", typeof(MasqueradeNode)},
            {"MSBIN", typeof(MSBinNode)},
            {"CMM", typeof(CMMNode)},
            {"SELB", typeof(SELBNode)},
            {"SELC", typeof(SELCNode)}
        };

        static NodeFactory()
        {
            // Add any BrawlCrate-side parsers (currently only BrawlAPI stuff)
            foreach (Type t in Assembly.GetEntryAssembly()?.GetTypes()
                ?.Where(t => t.IsSubclassOf(typeof(ResourceNode)))
                ?? Enumerable.Empty<Type>())
            {
                AddParser(t);
            }

            // Add all BrawlLib parsers (excluding MoveDefs, as explained below)
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ResourceNode))))
            {
                AddParser(t);
            }
        }

        public static void AddParser(Type t)
        {
            Delegate del = Delegate.CreateDelegate(typeof(ResourceParser), t, "TryParse", false, false);
            if (del != null)
            {
                _parsers.Add(del as ResourceParser);
            }

            Delegate del2 = Delegate.CreateDelegate(typeof(ResourceParser), t, "TryParseGeneric", false, false);
            if (del2 != null)
            {
                _parsersGeneric.Add(del2 as ResourceParser);
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

                bool supportsCompression = true;
                if(!(t is null))
                {
                    ResourceNode n = Activator.CreateInstance(t) as ResourceNode;
                    supportsCompression = n?.supportsCompression ?? true;
                }

                if ((node = FromSource(parent, source, t, supportsCompression)) == null)
                {
                    string ext = path.Substring(path.LastIndexOf('.') + 1).ToUpper(CultureInfo.InvariantCulture);

                    if (!(t is null) && (node = Activator.CreateInstance(t) as ResourceNode) != null
                        || ForcedExtensions.ContainsKey(ext) &&
                        (node = Activator.CreateInstance(ForcedExtensions[ext]) as ResourceNode) != null)
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
            return FromSource(parent, new DataSource(address, length), null, true);
        }

        public static ResourceNode FromSource(ResourceNode parent, DataSource source)
        {
            return FromSource(parent, source, null, true);
        }

        public static ResourceNode FromSource(ResourceNode parent, DataSource source, bool supportCompression)
        {
            return FromSource(parent, source, null, supportCompression);
        }

        public static ResourceNode FromSource(ResourceNode parent, DataSource source, Type t)
        {
            return FromSource(parent, source, t, true);
        }


        public static ResourceNode FromSource(ResourceNode parent, DataSource source, Type t, bool supportCompression)
        {
            ResourceNode n;

            if (t != null && (n = Activator.CreateInstance(t) as ResourceNode) != null)
            {
                FileMap uncompressedMap = null;
                if (supportCompression)
                {
                    uncompressedMap = Compressor.TryExpand(ref source, false);
                }

                if (uncompressedMap != null)
                {
                    n.Initialize(parent, source, new DataSource(uncompressedMap));
                }
                else
                {
                    n.Initialize(parent, source);
                }
            }
            else if ((n = GetRaw(source, parent)) != null)
            {
                n.Initialize(parent, source);
            }
            else
            {
                FileMap uncomp = Compressor.TryExpand(ref source);
                if (uncomp != null)
                {
                    DataSource d = new DataSource(uncomp);
                    n = GetRaw(d, parent);
                    n?.Initialize(parent, source, d);
                }
            }

            return n;
        }

        public static ResourceNode GetRaw(VoidPtr address, int length)
        {
            return GetRaw(new DataSource(address, length), null);
        }

        public static ResourceNode GetRaw(DataSource source, ResourceNode parent)
        {
            if (source.Length <= 0)
            {
                return null;
            }
            ResourceNode n;

            foreach (ResourceParser d in _parsers)
            {
                if ((n = d(source, parent)) != null)
                {
                    return n;
                }
            }

            foreach (ResourceParser d in _parsersGeneric)
            {
                if ((n = d(source, parent)) != null)
                {
                    return n;
                }
            }

            return null;
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