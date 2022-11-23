using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii;
using BrawlLib.Wii.Compression;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ARCNode : ARCEntryNode
    {
        internal ARCHeader* Header => (ARCHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.ARC;
        public override Type[] AllowedChildTypes => new Type[] {typeof(ARCEntryNode)};
        [Browsable(false)] public override int MaxNameLength => 47;

        #region SpecialNames

        public static List<string> SpecialName = new List<string>
        {
            "FitCaptain",
            "FitDedede",
            "FitDiddy",
            "FitDonkey",
            "FitFalco",
            "FitFox",
            "FitGameWatch",
            "FitGanon",
            "FitGKoopa",
            "FitIke",
            "FitKirby",
            "FitKoopa",
            "FitLink",
            "FitLucario",
            "FitLucas",
            "FitLuigi",
            "FitMario",
            "FitMarth",
            "FitMetaknight",
            "FitNess",
            "FitPeach",
            "FitPikachu",
            "FitPikmin",
            "FitPit",
            "FitPokeFushigisou",
            "FitPokeLizardon",
            "FitPokeTrainer",
            "FitPokeZenigame",
            "FitPopo",
            "FitPurin",
            "FitRobot",
            "FitSamus",
            "FitSheik",
            "FitSnake",
            "FitSonic",
            "FitSZerosuit",
            "FitToonLink",
            "FitWario",
            "FitWarioMan",
            "FitWolf",
            "FitYoshi",
            "FitZakoBall",
            "FitZakoBoy",
            "FitZakoChild",
            "FitZakoGirl",
            "FitZelda",
            "Fighter"
        };

        #endregion


#if !DEBUG
        [Browsable(false)]
#endif
        public bool IsPair
        {
            get => _isPair;
            set => _isPair = value;
        }

        private bool _isPair;

#if !DEBUG
        [Browsable(false)]
#endif
        public bool IsSubspace => _isSubspace;

        private bool _isSubspace;

#if !DEBUG
        [Browsable(false)]
#endif
        public bool IsStage => _isStage;

        private bool _isStage;

#if !DEBUG
        [Browsable(false)]
#endif
        public bool IsFighter => _isFighter;

        private bool _isFighter;

#if !DEBUG
        [Browsable(false)]
#endif
        public bool IsItemTable => _isItemTable;

        private bool _isItemTable;

        [Browsable(true)]
        public string SpecialARC
        {
            get
            {
                if (IsFighter)
                {
                    return "Fighter";
                }

                if (IsStage)
                {
                    if (IsSubspace)
                    {
                        return "Subspace Stage";
                    }

                    return "Stage";
                }

                if (IsItemTable)
                {
                    return "Item Table";
                }

                if (Parent is ARCNode)
                {
                    if (((ARCNode) Parent).SpecialARC.EndsWith("SubNode") ||
                        ((ARCNode) Parent).SpecialARC.Equals("<None>"))
                    {
                        return ((ARCNode) Parent).SpecialARC;
                    }

                    return ((ARCNode) Parent).SpecialARC + " SubNode";
                }

                return "<None>";
            }
        }

        [Category("Models")]
        public int NumModels
        {
            get
            {
                if (Properties.Settings.Default.CompatibilityMode)
                {
                    return -1;
                }

                if (_children == null)
                {
                    Populate();
                    if (_children == null)
                    {
                        return 0;
                    }
                }

                int count = 0;
                foreach (ResourceNode c in Children)
                {
                    if (c is ARCEntryNode b)
                    {
                        if (b is BRRESNode brresNode)
                        {
                            count += brresNode.NumModels;
                        }
                        else if (b is ARCNode node && node.NumModels > -1)
                        {
                            count += node.NumModels;
                        }
                    }
                }

                return count;
            }
        }

        [Category("Models")]
        [Description(
            "How many points are stored in the models in this ARC and sent to the GPU every frame. A lower value is better.")]
        public int NumFacepoints
        {
            get
            {
                if (Properties.Settings.Default.CompatibilityMode)
                {
                    return -1;
                }

                if (_children == null)
                {
                    Populate();
                    if (_children == null)
                    {
                        return 0;
                    }
                }

                int count = 0;
                foreach (ResourceNode c in Children)
                {
                    if (c is ARCEntryNode b)
                    {
                        if (b is BRRESNode node)
                        {
                            count += node.NumFacepoints;
                        }
                        else if (b is ARCNode arcNode && arcNode.NumModels > -1)
                        {
                            count += arcNode.NumFacepoints;
                        }
                    }
                }

                return count;
            }
        }

        [Browsable(true)]
        [Category("Models")]
        [Description(
            "How many individual vertices models in this ARC have. A vertex in this case is only a point in space with its associated influence.")]
        public int NumVertices
        {
            get
            {
                if (Properties.Settings.Default.CompatibilityMode)
                {
                    return -1;
                }

                if (_children == null)
                {
                    Populate();
                    if (_children == null)
                    {
                        return 0;
                    }
                }

                int count = 0;
                foreach (ResourceNode c in Children)
                {
                    if (c is ARCEntryNode b)
                    {
                        if (b is BRRESNode node)
                        {
                            count += node.NumVertices;
                        }
                        else if (b is ARCNode arcNode && arcNode.NumModels > -1)
                        {
                            count += arcNode.NumVertices;
                        }
                    }
                }

                return count;
            }
        }

        [Category("Models")]
        [Description("The total number of individual triangle faces models in this ARC have.")]
        public int NumTriangles
        {
            get
            {
                if (Properties.Settings.Default.CompatibilityMode)
                {
                    return -1;
                }

                if (_children == null)
                {
                    Populate();
                    if (_children == null)
                    {
                        return 0;
                    }
                }

                int count = 0;
                foreach (ResourceNode c in Children)
                {
                    if (c is ARCEntryNode b)
                    {
                        if (b is BRRESNode node)
                        {
                            count += node.NumTriangles;
                        }
                        else if (b is ARCNode arcNode && arcNode.NumModels > -1)
                        {
                            count += arcNode.NumTriangles;
                        }
                    }
                }

                return count;
            }
        }

        [Category("Models")]
        [Description("The total number of matrices used in this ARC (bones + weighted influences).")]
        public int NumNodes
        {
            get
            {
                if (Properties.Settings.Default.CompatibilityMode)
                {
                    return -1;
                }

                if (_children == null)
                {
                    Populate();
                    if (_children == null)
                    {
                        return 0;
                    }
                }

                int count = 0;
                foreach (ResourceNode c in Children)
                {
                    if (c is ARCEntryNode b)
                    {
                        if (b is BRRESNode node)
                        {
                            count += node.NumNodes;
                        }
                        else if (b is ARCNode arcNode && arcNode.NumModels > -1)
                        {
                            count += arcNode.NumNodes;
                        }
                    }
                }

                return count;
            }
        }


        private readonly Dictionary<ResourceNode, ARCFileHeader> _originalHeaders =
            new Dictionary<ResourceNode, ARCFileHeader>();

        public void SortChildrenByFileIndex()
        {
            if (Children == null || Children.Count <= 0)
            {
                return;
            }

            _children = _children.OrderBy(o => (o as ARCEntryNode)?.FileIndex ?? -1).ToList();
            SignalPropertyChange();
        }

        public override void OnPopulate()
        {
            _originalHeaders.Clear();
            ARCFileHeader* entry = Header->First;
            for (int i = 0; i < Header->_numFiles; i++, entry = entry->Next)
            {
                DataSource source = new DataSource(entry->Data, entry->Length);
                ResourceNode createdNode = entry->Length == 0
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
                string path = Path.Combine(Path.GetDirectoryName(_origPath),
                    Path.GetFileNameWithoutExtension(_origPath));
                _isPair = File.Exists(path + ".pac") && File.Exists(path + ".pcs");
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _name = Header->Name;
            _isStage = false;
            _isFighter = false;
            _isItemTable = false;

            if (RootNode == this)
            {
                if (_name.StartsWith("STG", StringComparison.OrdinalIgnoreCase))
                {
                    _isStage = true;
                    Console.WriteLine(_name + " Generating MetaData");
                }
                else if (_name.StartsWith("FIT", StringComparison.OrdinalIgnoreCase))
                {
                    _isFighter = true;
                }
                else if ((_name.Length == 6 || _name.Length == 7) && int.TryParse(_name.Substring(0, 6), out _))
                {
                    _isStage = true;
                    _isSubspace = true;
                }
            }

            if (_name.StartsWith("Itm", StringComparison.OrdinalIgnoreCase) &&
                _name.EndsWith("Gen", StringComparison.OrdinalIgnoreCase))
            {
                _isItemTable = true;
            }

            if (Compression == "LZ77" && Header->_numFiles > 0)
            {
                if (_parent is ARCNode)
                {
                    if (((ARCNode) _parent).IsStage && Properties.Settings.Default.AutoCompressStages)
                    {
                        // Console.WriteLine(_parent._name);
                        if (Enum.TryParse("ExtendedLZ77", out CompressionType type))
                        {
                            _compression = type;
                            SignalPropertyChange();
                        }
                    }
                }
            }

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

        public void ReplaceFromFolder(string inFolder)
        {
            ReplaceFromFolder(inFolder, ".tex0");
        }

        public void ReplaceFromFolder(string inFolder, string imageExtension)
        {
            DirectoryInfo dir = new DirectoryInfo(inFolder);
            DirectoryInfo[] dirs;
            foreach (ResourceNode entry in Children)
            {
                if (entry is ARCNode)
                {
                    dirs = dir.GetDirectories(entry.Name);
                    if (dirs.Length > 0)
                    {
                        ((ARCNode) entry).ReplaceFromFolder(dirs[0].FullName, imageExtension);
                    }
                }
                else
                {
                    (entry as BRRESNode)?.ReplaceFromFolder(inFolder, imageExtension);
                }
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return OnCalculateSize(force, true);
        }

        public int OnCalculateSize(bool force, bool rebuilding)
        {
            int size = ARCHeader.Size + Children.Count * 0x20;
            foreach (ResourceNode node in Children)
            {
                if (rebuilding)
                {
                    size += node.CalculateSize(force).Align(0x20);
                }
                else if (!(node is RELNode))
                {
                    size += node.OnCalculateSize(force).Align(0x20);
                }
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int size, bool force)
        {
            if (RedirectNode != null)
            {
                _redirectIndex = (short?) (RedirectNode as ARCEntryNode)?.AbsoluteIndex ?? -1;
            }

            ARCHeader* header = (ARCHeader*) address;
            *header = new ARCHeader((ushort) Children.Count, Name);

            ARCFileHeader* entry = header->First;
            foreach (ResourceNode child in Children)
            {
                if (child is ARCEntryNode node)
                {
                    *entry = new ARCFileHeader(node.FileType, node.FileIndex, node._calcSize, node.GroupID,
                        node._redirectIndex);
                }
                else if (_originalHeaders.TryGetValue(child, out ARCFileHeader origHeader))
                {
                    *entry = new ARCFileHeader(origHeader.FileType, origHeader.Index, child._calcSize,
                        origHeader.GroupIndex, origHeader.ID);
                }
                else
                {
                    throw new NotSupportedException("Cannot build a new ARCFileHeader for this node (not supported)");
                }

                child.Rebuild(entry->Data, entry->Length, force);
                entry = entry->Next;
            }
        }

        public override void Export(string outPath)
        {
            if (outPath.EndsWith(".pair", StringComparison.OrdinalIgnoreCase))
            {
                ExportPair(outPath);
            }
            else if (outPath.EndsWith(".mrg", StringComparison.OrdinalIgnoreCase))
            {
                ExportAsMRG(outPath);
            }
            else if (outPath.EndsWith(".pcs", StringComparison.OrdinalIgnoreCase))
            {
                ExportPCS(outPath);
            }
            else if (outPath.EndsWith(".mariopast", StringComparison.OrdinalIgnoreCase))
            {
                ExportMarioPast(outPath);
            }
            else if (outPath.EndsWith(".metalgear", StringComparison.OrdinalIgnoreCase))
            {
                ExportMetalGear(outPath);
            }
            else if (outPath.EndsWith(".village", StringComparison.OrdinalIgnoreCase))
            {
                ExportVillage(outPath);
            }
            else if (outPath.EndsWith(".tengan", StringComparison.OrdinalIgnoreCase))
            {
                ExportTengan(outPath);
            }
            else if (outPath.EndsWith(".pac", StringComparison.OrdinalIgnoreCase) && IsFighter &&
                     Properties.Settings.Default.AutoDecompressFighterPAC)
            {
                ExportPAC(outPath);
            }
            else
            {
                base.Export(outPath);
            }
        }

        public void ExportAsMRG(string path)
        {
            MRGNode node = new MRGNode
            {
                _children = Children,
                _changed = true
            };
            node.Export(path);
        }

        public void ExportPair(string path)
        {
            if (Path.HasExtension(path))
            {
                path = path.Substring(0, path.LastIndexOf('.'));
            }

            ExportPAC(path + ".pac");
            ExportPCS(path + ".pcs");
        }

        // STGMARIOPAST uses 00/01
        public void ExportMarioPast(string path)
        {
            if (Path.HasExtension(path))
            {
                path = path.Substring(0, path.LastIndexOf('.'));
            }

            char aslIndicator = '\0';
            // Check if ASL is used
            if (path.Contains("_") && path.Length >= 2 && path[path.Length - 2] == '_')
            {
                aslIndicator = path.ToCharArray()[path.Length - 1];
            }

            path = path.Substring(0, path.LastIndexOf('\\'));
            // Export with or without ASL depending on if the file used ASL or not
            if (aslIndicator != '\0')
            {
                ExportPAC(path + "\\STGMARIOPAST_00_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGMARIOPAST_01_" + aslIndicator + ".pac");
            }
            else
            {
                ExportPAC(path + "\\STGMARIOPAST_00.pac");
                ExportPAC(path + "\\STGMARIOPAST_01.pac");
            }
        }

        // STGMETALGEAR uses 00, 01, and 02
        public void ExportMetalGear(string path)
        {
            if (Path.HasExtension(path))
            {
                path = path.Substring(0, path.LastIndexOf('.'));
            }

            char aslIndicator = '\0';
            // Check if ASL is used
            if (path.Contains("_") && path.Length > 1 && path[path.Length - 2] == '_')
            {
                aslIndicator = path.ToCharArray()[path.Length - 1];
            }

            path = path.Substring(0, path.LastIndexOf('\\'));
            // Export with or without ASL depending on if the file used ASL or not
            if (aslIndicator != '\0')
            {
                ExportPAC(path + "\\STGMETALGEAR_00_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGMETALGEAR_01_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGMETALGEAR_02_" + aslIndicator + ".pac");
            }
            else
            {
                ExportPAC(path + "\\STGMETALGEAR_00.pac");
                ExportPAC(path + "\\STGMETALGEAR_01.pac");
                ExportPAC(path + "\\STGMETALGEAR_02.pac");
            }
        }

        // STGVILLAGE uses 00, 01, 02, and 03
        public void ExportVillage(string path)
        {
            if (Path.HasExtension(path))
            {
                path = path.Substring(0, path.LastIndexOf('.'));
            }

            char aslIndicator = '\0';
            // Check if ASL is used
            if (path.Contains("_") && path.Length >= 2 && path[path.Length - 2] == '_')
            {
                aslIndicator = path.ToCharArray()[path.Length - 1];
            }

            path = path.Substring(0, path.LastIndexOf('\\'));
            // Export with or without ASL depending on if the file used ASL or not
            if (aslIndicator != '\0')
            {
                ExportPAC(path + "\\STGVILLAGE_00_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGVILLAGE_01_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGVILLAGE_02_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGVILLAGE_03_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGVILLAGE_04_" + aslIndicator + ".pac");
            }
            else
            {
                ExportPAC(path + "\\STGVILLAGE_00.pac");
                ExportPAC(path + "\\STGVILLAGE_01.pac");
                ExportPAC(path + "\\STGVILLAGE_02.pac");
                ExportPAC(path + "\\STGVILLAGE_03.pac");
                ExportPAC(path + "\\STGVILLAGE_04.pac");
            }
        }

        // STGTENGAN uses 1, 2, 3
        public void ExportTengan(string path)
        {
            if (Path.HasExtension(path))
            {
                path = path.Substring(0, path.LastIndexOf('.'));
            }

            char aslIndicator = '\0';
            // Check if ASL is used
            if (path.Contains("_") && path.Length >= 2 && path[path.Length - 2] == '_')
            {
                aslIndicator = path.ToCharArray()[path.Length - 1];
            }

            // Check to make sure they meant this as ASL and not as a type indicator
            if (path.LastIndexOf('\\') < path.Length - 1 &&
                path.Substring(path.LastIndexOf('\\') + 1)
                    .StartsWith("STGTENGAN_", StringComparison.OrdinalIgnoreCase) &&
                (aslIndicator == '1' || aslIndicator == '2' || aslIndicator == '3') &&
                path.LastIndexOf('_') == path.IndexOf('_'))
            {
                if (MessageBox.Show(
                    "Would you like to use the detected '" + aslIndicator +
                    "' as the ASL indicator for the three files?", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    aslIndicator = '\0';
                }
            }

            path = path.Substring(0, path.LastIndexOf('\\'));
            // Export with or without ASL depending on if the file used ASL or not
            if (aslIndicator != '\0')
            {
                ExportPAC(path + "\\STGTENGAN_1_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGTENGAN_2_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGTENGAN_3_" + aslIndicator + ".pac");
            }
            else
            {
                ExportPAC(path + "\\STGTENGAN_1.pac");
                ExportPAC(path + "\\STGTENGAN_2.pac");
                ExportPAC(path + "\\STGTENGAN_3.pac");
            }
        }

        public void ExportPAC(string outPath)
        {
            Rebuild();
            ExportUncompressed(outPath);
        }

        public void ExportPCS(string outPath)
        {
            Rebuild();
            if (_compression != CompressionType.None || !Properties.Settings.Default.AutoCompressFighterPCS)
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
                        Compressor.Compact(CompressionType.ExtendedLZ77, WorkingUncompressed.Address,
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
            return ((ARCHeader*) source.Address)->_tag == ARCHeader.Tag ? new ARCNode() : null;
        }
    }

    public class ARCEntryGroup : ResourceNode
    {
        internal byte _group;

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

        public ARCEntryGroup(byte group)
        {
            _group = group;
            UpdateName();
        }

        protected void UpdateName()
        {
            Name = $"[{_group}]Group";
        }
    }

    public unsafe class ARCEntryNode : U8EntryNode
    {
        public override ResourceType ResourceFileType => _resourceType;
        public ResourceType _resourceType = ResourceType.ARCEntry;

        [Browsable(true)]
        [TypeConverter(typeof(DropDownListCompression))]
        public override string Compression
        {
            get => base.Compression;
            set => base.Compression = value;
        }

        internal ARCFileType _fileType;

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

        internal short _fileIndex;

        [Category("ARC Entry")]
        public short FileIndex
        {
            get => Parent is ARCNode ? _fileIndex : (short)Index;
            set
            {
                _fileIndex = value;
                SignalPropertyChange();
                UpdateName();
            }
        }

        internal byte _group;

        [Category("ARC Entry")]
        public byte GroupID
        {
            get => Parent is ARCNode ? _group : (byte)0;
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

        internal short _redirectIndex = -1;

        [Category("ARC Entry")]
        public short RedirectIndex
        {
            get => Parent is ARCNode ? _redirectIndex : (short)-1;
            set
            {
                if (value == _redirectIndex)
                {
                    return;
                }

                if (Parent == null || (_redirectIndex = (short) ((int) value).Clamp(-1, Parent.Children.Count - 1)) < 0)
                {
                    _resourceType = ResourceType.ARCEntry;
                }
                else
                {
                    _resourceType = ResourceType.Redirect;
                }

                UpdateRedirectTarget();
                UpdateName();
            }
        }

        [Category("ARC Entry")]
        [Browsable(false)]
        public string RedirectTargetName
        {
            get
            {
                if (redirectTargetNode == null)
                {
                    return "None";
                }

                if (redirectTargetNode is ARCEntryNode a)
                {
                    return $"{a.AbsoluteIndex.ToString()}. {redirectTargetNode.Name}";
                }

                return $"{redirectTargetNode.Index.ToString()}. {redirectTargetNode.Name}";
            }
        }

        [Category("ARC Entry")]
        [TypeConverter(typeof(DropDownListARCEntry))]
        public string RedirectTarget
        {
            get => RedirectTargetName;
            set
            {
                try
                {
                    if (short.TryParse(value.Substring(0, value.IndexOf(".")),
                        out short absIndex))
                    {
                        RedirectIndex = absIndex;
                        if (Parent.Children.Count < absIndex)
                        {
                            redirectTargetNode = Parent.Children[absIndex];
                        }

                        SignalPropertyChange();
                        UpdateName();
                        return;
                    }
                }
                catch
                {
                    if (short.TryParse(value, out short absIndex))
                    {
                        RedirectIndex = absIndex;
                        if (Parent.Children.Count < absIndex)
                        {
                            redirectTargetNode = Parent.Children[absIndex];
                        }

                        SignalPropertyChange();
                        UpdateName();
                        return;
                    }
                }

                redirectTargetNode = null;
                RedirectIndex = -1;
                SignalPropertyChange();
                UpdateName();
            }
        }

#if DEBUG
        [Browsable(true)]
#endif
        public ResourceNode RedirectNode => Parent is ARCNode ? redirectTargetNode : null;

        protected ResourceNode redirectTargetNode;

        /// <summary>
        ///     Sets the Redirect Target Node based on the RedirectIndex.
        ///
        ///     In the interest of keeping redirects node-centric, should be called only on load unless RedirectIndex is set manually.
        /// </summary>
        /// <returns>
        ///     Returns the new Redirect Target.
        /// </returns>
        private ResourceNode UpdateRedirectTarget()
        {
            if (RedirectIndex < 0 || Parent == null || !(Parent is ARCNode) || Parent.Children.Count <= RedirectIndex)
            {
                redirectTargetNode = null;
                UpdateProperties();
                return null;
            }

            redirectTargetNode = Parent?.Children[RedirectIndex];

            UpdateProperties();
            return redirectTargetNode;
        }

        protected virtual string GetName()
        {
            if (!(Parent is ARCNode))
            {
                return _name;
            }
            return GetName(_fileType.ToString());
        }

        protected virtual string GetName(string fileType)
        {
            if (fileType.Length > 4 && !fileType.Contains(' '))
            {
                fileType = System.Text.RegularExpressions.Regex.Replace(fileType, "(\\B[A-Z])", " $1");
            }

            string s = $"{fileType} [{_fileIndex}]";
            if (_group != 0)
            {
                s += $" [Group {_group}]";
            }

            if (_redirectIndex != -1)
            {
                s += $" (Redirect → {(redirectTargetNode == null ? _redirectIndex.ToString() : RedirectNode.Name)})";
            }

            return s;
        }

        public bool isModelData()
        {
            return FileType == ARCFileType.ModelData;
        }

        public bool isTextureData()
        {
            return FileType == ARCFileType.TextureData;
        }

        public void UpdateName()
        {
            if (!(this is ARCNode))
            {
                Name = GetName();
            }
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

                if (_name == null)
                {
                    _name = GetName();
                }
            }
            else if (parent != null && parent is ARCNode)
            {
                ARCFileHeader* header = (ARCFileHeader*) (origSource.Address - 0x20);
                _fileType = header->FileType;
                _fileIndex = header->_index;
                _group = header->_groupIndex;
                _redirectIndex = header->_redirectIndex;

                if (_name == null)
                {
                    if (_redirectIndex != -1 && _resourceType != ResourceType.MSBin)
                    {
                        _resourceType = ResourceType.Redirect;
                        UpdateRedirectTarget();
                    }

                    _name = GetName();
                }
            }
            else if (_name == null)
            {
                _name = Path.GetFileName(_origPath);
            }

            if (RedirectIndex != -1)
            {
                UpdateRedirectTarget();
            }
        }

        //public override unsafe void Export(string outPath)
        //{
        //    ExportUncompressed(outPath);
        //}
    }
}