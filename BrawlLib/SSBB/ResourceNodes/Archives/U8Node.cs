using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii;
using BrawlLib.Wii.Compression;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class U8Node : U8EntryNode
    {
        internal U8* Header => (U8*) WorkingUncompressed.Address;

        public override ResourceType ResourceFileType => ResourceType.U8;
        public override Type[] AllowedChildTypes => new Type[] {typeof(U8EntryNode)};

        [Browsable(true)]
        [TypeConverter(typeof(DropDownListCompression))]
        public override string Compression
        {
            get => base.Compression;
            set => base.Compression = value;
        }

        public override void OnPopulate()
        {
            U8Entry* first = Header->Entries;
            uint count = first->_dataLength - 1;
            U8Entry* entry = first + 1;
            sbyte* table = (sbyte*) entry + count * 12;
            List<U8EntryNode> nodes = new List<U8EntryNode>();
            U8EntryNode e = null;
            for (int i = 0; i < count; i++)
            {
                if (entry->isFolder)
                {
                    e = new U8FolderNode {_u8Index = i, _name = new string(table + entry->_stringOffset)};

                    e._name = new string(table + entry->_stringOffset);
                    e._u8Index = i;
                    e._u8Parent = (int) entry->_dataOffset;
                    e._u8FirstNotChild = (int) entry->_dataLength;
                    e._u8Type = entry->_type;

                    e.Initialize(this, entry, 12);

                    nodes.Add(e);
                }
                else
                {
                    DataSource source = new DataSource((VoidPtr) Header + entry->_dataOffset, (int) entry->_dataLength);

                    if (entry->_dataLength == 0 || (e = NodeFactory.FromSource(this, source) as U8EntryNode) == null)
                    {
                        e = new ARCEntryNode();
                        e._origSource = source;
                        e._uncompSource = source;
                    }

                    e._name = new string(table + entry->_stringOffset);
                    e._u8Index = i;
                    e._u8Parent = -1;
                    e._u8FirstNotChild = -1;
                    e._u8Type = entry->_type;

                    e.Initialize(this, e.OriginalSource, e.UncompressedSource);

                    nodes.Add(e);
                }

                entry++;
            }

            foreach (U8EntryNode x in nodes)
            {
                if (x._u8Type == 1)
                {
                    if (x._u8Parent == 0)
                    {
                        x.Parent = this;
                    }
                    else if (x._u8Parent < nodes.Count)
                    {
                        x.Parent = nodes[x._u8Parent - 1];
                    }

                    U8EntryNode t = null;
                    if (x._u8Index + 1 < nodes.Count)
                    {
                        t = nodes[x._u8Index + 1];
                    }

                    while (t != null)
                    {
                        t.Parent = x;
                        if (t._u8Index + 1 < nodes.Count && x.ChildEndIndex - 1 != nodes[t._u8Index + 1]._u8Index)
                        {
                            t = nodes[t._u8Index + 1];
                        }
                        else
                        {
                            t = null;
                        }
                    }
                }
            }

            IsDirty = false; //Clear up changes from parent reassignments
        }

        public override bool OnInitialize()
        {
            return true;
        }

        private int _entrySize;
        private OrderedStringTable _stringTable;

        private int GetEntrySize(U8EntryNode node, bool force, ref int id)
        {
            node._u8Index = id++;

            _stringTable.Add(node.Name);
            _entrySize += 12;

            int size = node is U8FolderNode ? 0 : node.CalculateSize(force).Align(0x20);
            foreach (ResourceNode r in node.Children)
            {
                if (r is U8EntryNode)
                {
                    size += GetEntrySize(r as U8EntryNode, force, ref id);
                }
            }

            return size;
        }

        public override int OnCalculateSize(bool force)
        {
            _entrySize = 12;
            int id = 1;

            _stringTable = new OrderedStringTable();
            _stringTable.Add("");

            int childSize = 0;
            foreach (ResourceNode e in Children)
            {
                if (e is U8EntryNode)
                {
                    childSize += GetEntrySize(e as U8EntryNode, force, ref id);
                }
            }

            return 0x20 + childSize + (entryLength = _stringTable.TotalSize + _entrySize).Align(0x20);
        }

        private void RebuildNode(VoidPtr header, U8EntryNode node, ref U8Entry* entry, VoidPtr sTableStart,
                                 ref VoidPtr dataAddr, bool force)
        {
            entry->_type = (byte)(node is U8FolderNode ? 1 : 0);
            entry->_stringOffset.Value = checked((uint)(_stringTable[node.Name] - sTableStart));
            if (entry->_type == 1)
            {
                int index = node.Index + 1, parentIndex = 0, endIndex = _entrySize / 12;

                if (node.Parent != this && node.Parent != null)
                {
                    parentIndex = ((U8EntryNode)node.Parent)._u8Index;
                }

                ResourceNode nextParent = node.Parent;
                ResourceNode rootFolder = nextParent;
                int curIndex = index;
                while (nextParent != null && nextParent != this)
                {
                    if (curIndex < nextParent?.Children.Count && nextParent.Children[curIndex] is U8EntryNode u8en) break;

                    curIndex = ((U8EntryNode)nextParent).Index + 1;
                    rootFolder = nextParent;
                    nextParent = nextParent.Parent;

                }

                int folderIndex = index;
                if (node.Parent.Children.Count == index)
                {
                    if (node.Parent is U8FolderNode)
                    {
                        folderIndex = rootFolder.Index + 1;
                    }
                    if (folderIndex < nextParent.Children.Count)
                    {
                        if (nextParent.Children[folderIndex] != null)
                        {
                            endIndex = ((U8EntryNode)nextParent.Children[folderIndex])._u8Index;
                        }
                    }
                }
                else
                {
                    endIndex = ((U8EntryNode)node.NextSibling())._u8Index;
                }

                entry->_dataLength = (uint)endIndex;
                entry->_dataOffset = (uint)parentIndex;
                entry++;

                foreach (ResourceNode b in node.Children)
                {
                    if (b is U8EntryNode)
                    {
                        RebuildNode(header, b as U8EntryNode, ref entry, sTableStart, ref dataAddr, force);
                    }
                }
            }
            else
            {
                entry->_dataOffset = checked((uint)(dataAddr - header));
                entry->_dataLength = checked((uint)node._calcSize);
                entry++;

                node.Rebuild(dataAddr, node._calcSize, force);
                dataAddr += node._calcSize.Align(0x20);
            }
        }

        private int entryLength;

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            U8* header = (U8*) address;
            header->_tag = U8.Tag;
            header->_entriesLength = (uint) entryLength;
            header->_entriesOffset = 0x20;

            VoidPtr dataAddress = address + 0x20 + entryLength.Align(0x20);
            U8Entry* entries = (U8Entry*) (address + 0x20);
            VoidPtr tableAddr = address + 0x20 + _entrySize;
            _stringTable.WriteTable(tableAddr);

            header->_firstOffset = (uint) (dataAddress - address);

            entries->_dataLength = (uint) (_entrySize / 12);
            entries->_type = 1;
            entries++;

            foreach (U8EntryNode b in Children)
            {
                RebuildNode(address, b, ref entries, tableAddr, ref dataAddress, force);
            }
        }

        public override void Export(string outPath)
        {
            ExportNonYaz0(outPath);
        }

        public void ExportSZS(string outPath)
        {
            ExportCompressed(outPath);
        }

        public void ExportPair(string outPath)
        {
            if (Path.HasExtension(outPath))
            {
                outPath = outPath.Substring(0, outPath.LastIndexOf('.'));
            }

            ExportNonYaz0(outPath + ".arc");
            ExportCompressed(outPath + ".szs");
        }

        public void ExportNonYaz0(string outPath)
        {
            base.Export(outPath);
        }

        public void ExportCompressed(string outPath)
        {
            if (_compression != CompressionType.None)
            {
                base.Export(outPath);
            }
            else
            {
                using (FileStream inStream = new FileStream(Path.GetTempFileName(), FileMode.OpenOrCreate,
                    FileAccess.ReadWrite, FileShare.None, 0x8, FileOptions.SequentialScan | FileOptions.DeleteOnClose))
                {
                    using (FileStream outStream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                        FileShare.None, 8, FileOptions.SequentialScan))
                    {
                        Compressor.Compact(CompressionType.RunLengthYAZ0, WorkingUncompressed.Address,
                            WorkingUncompressed.Length, inStream, this);
                        outStream.SetLength(inStream.Length);
                        using (FileMap map = FileMap.FromStream(inStream))
                        {
                            using (FileMap outMap = FileMap.FromStream(outStream))
                            {
                                Memory.Move(outMap.Address, map.Address, (uint) map.Length);
                            }
                        }
                    }
                }
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((U8*) source.Address)->_tag == U8.Tag ? new U8Node() : null;
        }

        public void ExtractToFolder(string outFolder)
        {
            ExtractToFolder(outFolder, ".tex0", ".mdl0");
        }

        public void ExtractToFolder(string outFolder, string imageExtension)
        {
            ExtractToFolder(outFolder, imageExtension, ".mdl0");
        }

        public void ExtractToFolder(string outFolder, string imageExtension, string modelExtension)
        {
            if (!Directory.Exists(outFolder))
            {
                Directory.CreateDirectory(outFolder);
            }

            List<string> directChildrenExportedPaths = new List<string>();
            foreach (ResourceNode entry in Children)
            {
                if (entry is ARCNode)
                {
                    ((ARCNode) entry).ExtractToFolder(
                        Path.Combine(outFolder,
                            entry.Name == null || entry.Name.Contains("<Null>", StringComparison.OrdinalIgnoreCase)
                                ? "Null"
                                : entry.Name), imageExtension, modelExtension);
                }
                else if (entry is BRRESNode)
                {
                    ((BRRESNode) entry).ExportToFolder(
                        Path.Combine(outFolder,
                            entry.Name == null || entry.Name.Contains("<Null>", StringComparison.OrdinalIgnoreCase)
                                ? "Null"
                                : entry.Name), imageExtension, modelExtension);
                }
                else if (entry is U8Node)
                {
                    ((U8Node) entry).ExtractToFolder(
                        Path.Combine(outFolder,
                            entry.Name == null || entry.Name.Contains("<Null>", StringComparison.OrdinalIgnoreCase)
                                ? "Null"
                                : entry.Name), imageExtension, modelExtension);
                }
                else if (entry is U8FolderNode)
                {
                    ((U8FolderNode) entry).ExportToFolder(
                        Path.Combine(outFolder,
                            entry.Name == null || entry.Name.Contains("<Null>", StringComparison.OrdinalIgnoreCase)
                                ? "Null"
                                : entry.Name), imageExtension, modelExtension);
                }
                else
                {
                    if (entry.WorkingSource.Length == 0)
                    {
                        continue;
                    }

                    string ext = FileFilters.GetDefaultExportAllExtension(entry.GetType());
                    string path = Path.Combine(outFolder, entry.Name + ext);

                    if (directChildrenExportedPaths.Contains(path))
                    {
                        throw new Exception(
                            $"There is more than one node underneath {Name} with the name {entry.Name}.");
                    }

                    directChildrenExportedPaths.Add(path);
                    entry.Export(path);
                }
            }
        }
    }

    public unsafe class U8EntryNode : ResourceNode
    {
        internal U8Entry* U8EntryHeader => (U8Entry*) WorkingSource.Address;

        public int _u8Parent, _u8FirstNotChild, _u8Type, _u8Index;

        [Browsable(false)] public int ParentIndex => _u8Parent;
        [Browsable(false)] public int ChildEndIndex => _u8FirstNotChild;
        [Browsable(false)] public int Type => _u8Type;
        [Browsable(false)] public int ID => _u8Index;

        public override bool OnInitialize()
        {
            base.OnInitialize();
            return this is U8FolderNode && _u8FirstNotChild - 1 > _u8Index;
        }
    }

    public class U8FolderNode : U8EntryNode
    {
        public override ResourceType ResourceFileType => ResourceType.U8Folder;
        public override Type[] AllowedChildTypes => new Type[] {typeof(U8EntryNode)};

        public override bool OnInitialize()
        {
            return base.OnInitialize();
        }

        public override int OnCalculateSize(bool force)
        {
            return 0;
        }

        public T CreateResource<T>(string name) where T : U8EntryNode
        {
            T n = Activator.CreateInstance<T>();
            n.Name = FindName(name);
            AddChild(n);

            return n;
        }

        public void ExportToFolder(string outFolder)
        {
            ExportToFolder(outFolder, ".tex0");
        }

        public void ExportToFolder(string outFolder, string imageExtension)
        {
            ExportToFolder(outFolder, imageExtension, ".mdl0");
        }

        public void ExportToFolder(string outFolder, string imageExtension, string modelExtension)
        {
            if (!Directory.Exists(outFolder))
            {
                Directory.CreateDirectory(outFolder);
            }

            List<string> directChildrenExportedPaths = new List<string>();
            foreach (ResourceNode entry in Children)
            {
                if (entry is ARCNode)
                {
                    ((ARCNode) entry).ExtractToFolder(
                        Path.Combine(outFolder,
                            entry.Name == null || entry.Name.Contains("<Null>", StringComparison.OrdinalIgnoreCase)
                                ? "Null"
                                : entry.Name), imageExtension, modelExtension);
                }
                else if (entry is BRRESNode)
                {
                    ((BRRESNode) entry).ExportToFolder(
                        Path.Combine(outFolder,
                            entry.Name == null || entry.Name.Contains("<Null>", StringComparison.OrdinalIgnoreCase)
                                ? "Null"
                                : entry.Name), imageExtension, modelExtension);
                }
                else if (entry is U8Node)
                {
                    ((U8Node) entry).ExtractToFolder(
                        Path.Combine(outFolder,
                            entry.Name == null || entry.Name.Contains("<Null>", StringComparison.OrdinalIgnoreCase)
                                ? "Null"
                                : entry.Name), imageExtension, modelExtension);
                }
                else if (entry is U8FolderNode)
                {
                    ((U8FolderNode) entry).ExportToFolder(
                        Path.Combine(outFolder,
                            entry.Name == null || entry.Name.Contains("<Null>", StringComparison.OrdinalIgnoreCase)
                                ? "Null"
                                : entry.Name), imageExtension, modelExtension);
                }
                else
                {
                    if (entry.WorkingSource.Length == 0)
                    {
                        continue;
                    }

                    string ext = FileFilters.GetDefaultExportAllExtension(entry.GetType());
                    string path = Path.Combine(outFolder, entry.Name + ext);

                    if (directChildrenExportedPaths.Contains(path))
                    {
                        throw new Exception(
                            $"There is more than one node underneath {Name} with the name {entry.Name}.");
                    }

                    directChildrenExportedPaths.Add(path);
                    entry.Export(path);
                }
            }
        }
    }

    public unsafe class OrderedStringTable
    {
        public List<string> _keys = new List<string>();
        public List<VoidPtr> _values = new List<VoidPtr>();

        public void Add(string s)
        {
            if (!_keys.Contains(s))
            {
                _keys.Add(s);
                _values.Add(0);
            }
        }

        public int TotalSize
        {
            get
            {
                int len = 0;
                foreach (string s in _keys)
                {
                    len += s.Length + 1;
                }

                return len;
            }
        }

        public void Clear()
        {
            _keys.Clear();
            _values.Clear();
        }

        public VoidPtr this[string s] => _values[_keys.IndexOf(s)];

        public void WriteTable(VoidPtr address)
        {
            CompactStringEntry* entry = (CompactStringEntry*) address;
            for (int i = 0; i < _keys.Count; i++)
            {
                string s = _keys[i];
                _values[i] = entry;
                entry->Value = s;
                entry = entry->Next;
            }
        }
    }
}