using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using BrawlLib.IO;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Compression;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ARCNode : ARCEntryNode
    {
        private readonly Dictionary<ResourceNode, ARCFileHeader> _originalHeaders =
            new Dictionary<ResourceNode, ARCFileHeader>();

        internal ARCHeader* Header => (ARCHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.ARC;
        public override Type[] AllowedChildTypes => new[] {typeof(ARCEntryNode)};

        [Browsable(false)] public bool IsPair { get; set; }

        [Browsable(false)]
        public bool IsStage => Parent == null && _name.StartsWith("STG", StringComparison.OrdinalIgnoreCase);


        [Browsable(false)]
        public bool IsFighter => Parent == null && _name.StartsWith("FIT", StringComparison.OrdinalIgnoreCase);

        public override void OnPopulate()
        {
            _originalHeaders.Clear();
            var entry = Header->First;
            for (var i = 0; i < Header->_numFiles; i++, entry = entry->Next)
            {
                var source = new DataSource(entry->Data, entry->Length);
                var createdNode = entry->Length == 0
                    ? null
                    : NodeFactory.FromSource(this, source);
                if (createdNode == null)
                {
                    createdNode = new ARCEntryNode();
                    createdNode.Initialize(this, source);
                }

                _originalHeaders.Add(createdNode, *entry);
            }
        }

        public override void Initialize(ResourceNode parent, DataSource origSource, DataSource uncompSource)
        {
            base.Initialize(parent, origSource, uncompSource);
            if (_origPath != null)
            {
                var path = Path.Combine(Path.GetDirectoryName(_origPath), Path.GetFileNameWithoutExtension(_origPath));
                IsPair = File.Exists(path + ".pac") && File.Exists(path + ".pcs");
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _name = Header->Name;
            return Header->_numFiles > 0;
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
            if (!Directory.Exists(outFolder)) Directory.CreateDirectory(outFolder);

            var directChildrenExportedPaths = new List<string>();
            foreach (ARCEntryNode entry in Children)
                if (entry is ARCNode)
                {
                    ((ARCNode) entry).ExtractToFolder(
                        Path.Combine(outFolder,
                            entry.Name == null ||
                            entry.Name.Contains("<Null>", StringComparison.InvariantCultureIgnoreCase)
                                ? "Null"
                                : entry.Name), imageExtension, modelExtension);
                }
                else if (entry is BRRESNode)
                {
                    ((BRRESNode) entry).ExportToFolder(
                        Path.Combine(outFolder,
                            entry.Name == null ||
                            entry.Name.Contains("<Null>", StringComparison.InvariantCultureIgnoreCase)
                                ? "Null"
                                : entry.Name), imageExtension, modelExtension);
                }
                else
                {
                    if (entry.WorkingSource.Length == 0) continue;

                    var ext = FileFilters.GetDefaultExportAllExtension(entry.GetType());
                    var path = Path.Combine(outFolder, entry.Name + ext);

                    if (directChildrenExportedPaths.Contains(path))
                        throw new Exception(
                            $"There is more than one node underneath {Name} with the name {entry.Name}.");

                    directChildrenExportedPaths.Add(path);
                    entry.Export(path);
                }
        }

        public void ReplaceFromFolder(string inFolder)
        {
            var dir = new DirectoryInfo(inFolder);
            var files = dir.GetFiles();
            DirectoryInfo[] dirs;
            foreach (ARCEntryNode entry in Children)
                if (entry is ARCNode)
                {
                    dirs = dir.GetDirectories(entry.Name);
                    if (dirs.Length > 0) ((ARCNode) entry).ReplaceFromFolder(dirs[0].FullName);
                }
                else if (entry is BRRESNode)
                {
                    dirs = dir.GetDirectories(entry.Name);
                    if (dirs.Length > 0)
                        ((BRRESNode) entry).ReplaceFromFolder(dirs[0].FullName);
                    else
                        ((BRRESNode) entry).ReplaceFromFolder(inFolder);
                }
                else
                {
                    var ext = FileFilters.GetDefaultExportAllExtension(entry.GetType());
                    foreach (var info in files)
                        if (info.Extension.Equals(ext, StringComparison.OrdinalIgnoreCase) &&
                            info.Name.Equals(entry.Name + ext, StringComparison.OrdinalIgnoreCase))
                        {
                            entry.Replace(info.FullName);
                            break;
                        }
                }
        }

        public override int OnCalculateSize(bool force)
        {
            var size = ARCHeader.Size + Children.Count * 0x20;
            foreach (var node in Children) size += node.CalculateSize(force).Align(0x20);

            return size;
        }

        public override void OnRebuild(VoidPtr address, int size, bool force)
        {
            var header = (ARCHeader*) address;
            *header = new ARCHeader((ushort) Children.Count, Name);

            var entry = header->First;
            foreach (var child in Children)
            {
                if (child is ARCEntryNode node)
                    *entry = new ARCFileHeader(node.FileType, node.FileIndex, node._calcSize, node.GroupID,
                        node._redirectIndex);
                else if (_originalHeaders.TryGetValue(child, out var origHeader))
                    *entry = new ARCFileHeader(origHeader.FileType, origHeader.Index, child._calcSize,
                        origHeader.GroupIndex, origHeader.ID);
                else
                    throw new NotSupportedException("Cannot build a new ARCFileHeader for this node (not supported)");
                child.Rebuild(entry->Data, entry->Length, force);
                entry = entry->Next;
            }
        }

        public override void Export(string outPath)
        {
            if (outPath.EndsWith(".pair", StringComparison.OrdinalIgnoreCase))
                ExportPair(outPath);
            else if (outPath.EndsWith(".mrg", StringComparison.OrdinalIgnoreCase))
                ExportAsMRG(outPath);
            else if (outPath.EndsWith(".pcs", StringComparison.OrdinalIgnoreCase))
                ExportPCS(outPath);
            //else if (outPath.EndsWith(".pac", StringComparison.OrdinalIgnoreCase))
            //    ExportPAC(outPath);
            else
                base.Export(outPath);
        }

        public void ExportAsMRG(string path)
        {
            var node = new MRGNode
            {
                _children = Children,
                _changed = true
            };
            node.Export(path);
        }

        public void ExportPair(string path)
        {
            if (Path.HasExtension(path)) path = path.Substring(0, path.LastIndexOf('.'));

            ExportPAC(path + ".pac");
            ExportPCS(path + ".pcs");
        }

        public void ExportPAC(string outPath)
        {
            Rebuild();
            ExportUncompressed(outPath);
        }

        public void ExportPCS(string outPath)
        {
            Rebuild();
            if (_compression != CompressionType.None)
                base.Export(outPath);
            else
                using (var inStream = new FileStream(Path.GetTempFileName(), FileMode.OpenOrCreate,
                    FileAccess.ReadWrite, FileShare.None, 0x8, FileOptions.SequentialScan | FileOptions.DeleteOnClose))
                using (var outStream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                    FileShare.None, 8, FileOptions.SequentialScan))
                {
                    Compressor.Compact(CompressionType.ExtendedLZ77, WorkingUncompressed.Address,
                        WorkingUncompressed.Length, inStream, this);
                    outStream.SetLength(inStream.Length);
                    using (var map = FileMap.FromStream(inStream))
                    using (var outMap = FileMap.FromStream(outStream))
                    {
                        Memory.Move(outMap.Address, map.Address, (uint) map.Length);
                    }
                }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((ARCHeader*) source.Address)->_tag == ARCHeader.Tag ? new ARCNode() : null;
        }
    }

    public class ARCEntryGroup : ResourceNode
    {
        internal byte _group;

        public ARCEntryGroup(byte group)
        {
            _group = group;
            UpdateName();
        }

        [Category("ARC Group")]
        public byte GroupID
        {
            get => _group;
            set
            {
                _group = value;
                SignalPropertyChange();
                UpdateName();
            }
        }

        protected void UpdateName()
        {
            Name = string.Format("[{0}]Group", _group);
        }
    }

    public unsafe class ARCEntryNode : U8EntryNode
    {
        internal short _fileIndex;

        internal ARCFileType _fileType;

        internal byte _group;

        internal short _redirectIndex = -1;
        public ResourceType _resourceType = ResourceType.ARCEntry;
        public override ResourceType ResourceFileType => _resourceType;

        [Browsable(true)]
        [TypeConverter(typeof(DropDownListCompression))]
        public override string Compression
        {
            get => base.Compression;
            set => base.Compression = value;
        }

        [Category("ARC Entry")]
        public ARCFileType FileType
        {
            get => _fileType;
            set
            {
                _fileType = value;
                SignalPropertyChange();
                UpdateName();
            }
        }

        [Category("ARC Entry")]
        public short FileIndex
        {
            get => _fileIndex;
            set
            {
                _fileIndex = value;
                SignalPropertyChange();
                UpdateName();
            }
        }

        [Category("ARC Entry")]
        public byte GroupID
        {
            get => _group;
            set
            {
                _group = value;
                SignalPropertyChange();
                UpdateName();
            }
        }

        [Category("ARC Entry")]
        [Browsable(true)]
        public int AbsoluteIndex => Index;

        [Category("ARC Entry")]
        public short RedirectIndex
        {
            get => _redirectIndex;
            set
            {
                if (value == Index || value == _redirectIndex) return;

                if ((_redirectIndex = (short) ((int) value).Clamp(-1, Parent.Children.Count - 1)) < 0)
                {
                    _resourceType = ResourceType.ARCEntry;
                    Name = GetName();
                }
                else
                {
                    _resourceType = ResourceType.Redirect;
                    Name = "Redirect → " + _redirectIndex;
                }
            }
        }

        protected virtual string GetName()
        {
            return GetName(_fileType.ToString());
        }

        protected virtual string GetName(string fileType)
        {
            if (fileType.Length > 4 && !fileType.Contains(' ')) fileType = Regex.Replace(fileType, "(\\B[A-Z])", " $1");
            var s = string.Format("{0} [{1}]", fileType, _fileIndex);
            if (_group != 0) s += " [Group " + _group + "]";

            return s;
        }

        protected void UpdateName()
        {
            if (!(this is ARCNode)) Name = GetName();
        }

        public override void Initialize(ResourceNode parent, DataSource origSource, DataSource uncompSource)
        {
            base.Initialize(parent, origSource, uncompSource);

            if (parent != null && (parent is MRGNode || RootNode is U8Node))
            {
                _fileType = 0;
                _fileIndex = (short) Parent._children.IndexOf(this);
                _group = 0;
                _redirectIndex = 0;

                if (_name == null) _name = GetName();
            }
            else if (parent != null && !(parent is FileScanNode))
            {
                var header = (ARCFileHeader*) (origSource.Address - 0x20);
                _fileType = header->FileType;
                _fileIndex = header->_index;
                _group = header->_groupIndex;
                _redirectIndex = header->_redirectIndex;

                if (_name == null)
                {
                    if (_redirectIndex != -1)
                    {
                        _resourceType = ResourceType.Redirect;
                        _name = "Redirect → " + _redirectIndex;
                    }
                    else
                    {
                        _name = GetName();
                    }
                }
            }
            else if (_name == null)
            {
                _name = Path.GetFileName(_origPath);
            }
        }

        //public override unsafe void Export(string outPath)
        //{
        //    ExportUncompressed(outPath);
        //}
    }
}