using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MRGNode : ResourceNode
    {
        internal MRGHeader* Header => (MRGHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.MRG;

        public override void OnPopulate()
        {
            uint numFiles = 0;
            MRGFileHeader* entry = Header->First;
            for (int i = 0; i < (numFiles = Header->_numFiles); i++, entry = entry->Next)
            {
                if (NodeFactory.FromAddress(this, Header + entry->Data, entry->Length) == null)
                {
                    new ARCEntryNode().Initialize(this, Header + entry->Data, entry->Length);
                }
            }
        }

        public override void Initialize(ResourceNode parent, DataSource origSource, DataSource uncompSource)
        {
            base.Initialize(parent, origSource, uncompSource);
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _name = Path.GetFileNameWithoutExtension(_origPath);
            return Header->_numFiles > 0;
        }

        public void ExtractToFolder(string outFolder)
        {
            if (!Directory.Exists(outFolder))
            {
                Directory.CreateDirectory(outFolder);
            }

            foreach (ARCEntryNode entry in Children)
            {
                if (entry is ARCNode)
                {
                    ((ARCNode) entry).ExtractToFolder(Path.Combine(outFolder, entry.Name));
                }
                else
                {
                    (entry as BRRESNode)?.ExportToFolder(outFolder);
                }
            }
        }

        public void ReplaceFromFolder(string inFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(inFolder);
            FileInfo[] files;
            DirectoryInfo[] dirs;
            foreach (ARCEntryNode entry in Children)
            {
                if (entry is ARCNode)
                {
                    dirs = dir.GetDirectories(entry.Name);
                    if (dirs.Length > 0)
                    {
                        ((ARCNode) entry).ReplaceFromFolder(dirs[0].FullName);
                        continue;
                    }
                }
                else
                {
                    (entry as BRRESNode)?.ReplaceFromFolder(inFolder);
                }

                //Find file name for entry
                files = dir.GetFiles(entry.Name + ".*");
                if (files.Length > 0)
                {
                    entry.Replace(files[0].FullName);
                    continue;
                }
            }
        }

        private int offset;

        public override int OnCalculateSize(bool force)
        {
            int size = offset = 0x20 + Children.Count * 0x20;
            foreach (ResourceNode node in Children)
            {
                size += node.CalculateSize(force);
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int size, bool force)
        {
            MRGHeader* header = (MRGHeader*) address;
            *header = new MRGHeader((uint) Children.Count);

            MRGFileHeader* entry = header->First;
            foreach (ARCEntryNode node in Children)
            {
                *entry = new MRGFileHeader(node._calcSize, offset);
                node.Rebuild(header + entry->Data, node._calcSize, force);
                offset += node._calcSize;
                entry = entry->Next;
            }
        }

        public void ExportAsARC(string path)
        {
            ARCNode node = new ARCNode
            {
                _children = _children,
                Name = _name
            };
            node.Export(path);
        }

        public override void Export(string outPath)
        {
            if (outPath.EndsWith(".pac", StringComparison.OrdinalIgnoreCase) ||
                outPath.EndsWith(".pcs", StringComparison.OrdinalIgnoreCase) ||
                outPath.EndsWith(".pair", StringComparison.OrdinalIgnoreCase))
            {
                ExportAsARC(outPath);
            }
            else
            {
                base.Export(outPath);
            }
        }

        //internal static ResourceNode TryParse(DataSource source, ResourceNode parent) 
        //{
        //    buint* addr = (buint*)source.Address;

        //    //if (addr[0] >= source.Length)
        //    //    return null;

        //    for (int i = 0; i < 7; i++)
        //        if (addr[i + 1] != 0)
        //            return null;

        //    uint headerSize = 0x20 + 0x20 * addr[0];
        //    //if (headerSize >= source.Length)
        //    //    return null;

        //    uint prevOff = headerSize;
        //    uint prevSize = 0;

        //    //if (prevSize >= source.Length)
        //    //    return null;

        //    uint count = addr[0];
        //    for (int i = 0; i < 2; i++)
        //    {
        //        int file = i * 8 + 8;

        //        uint c = addr[file];
        //        uint l = (prevOff + prevSize) + (uint)(i == 0 ? 0 : 0x20);

        //        if (/*c >= source.Length || */c != l)
        //            return null;

        //        prevOff = addr[file];
        //        prevSize = addr[file + 1];

        //        for (int x = 0; x < 6; x++)
        //            if (addr[x + 2] != 0)
        //                return null;

        //        //if (i == count - 1)
        //        //    if (prevOff + prevSize != source.Length)
        //        //        return null;
        //    }

        //    return new MRGNode();
        //}
    }
}