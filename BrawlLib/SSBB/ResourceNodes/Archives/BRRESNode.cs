using BrawlLib.Imaging;
using BrawlLib.Imaging.GIF;
using BrawlLib.Internal;
using BrawlLib.Internal.Drawing;
using BrawlLib.Internal.IO;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.Types;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class BRRESNode : ARCEntryNode, IImageSource
    {
        internal BRESHeader* Header => (BRESHeader*) WorkingUncompressed.Address;

        internal ROOTHeader* RootHeader => Header->First;
        internal ResourceGroup* Group => &RootHeader->_master;

        public override ResourceType ResourceFileType => ResourceType.BRES;

        public override Type[] AllowedChildTypes => new[] {typeof(BRESGroupNode)};

        [DisplayName("Texture Count")] public int ImageCount => GetFolder<TEX0Node>()?.Children.Count ?? 0;

        public Bitmap GetImage(int index)
        {
            if (GetFolder<TEX0Node>() == null || GetFolder<TEX0Node>().Children.Count <= index)
            {
                return null;
            }

            return (GetFolder<TEX0Node>().Children[index] as IImageSource)?.GetImage(0);
        }

        #region Model Counters

        [Category("Models")]
        public int NumModels
        {
            get
            {
                if (Properties.Settings.Default.CompatibilityMode)
                {
                    return -1;
                }

                //Populate();
                if (GetFolder<MDL0Node>() == null)
                {
                    return 0;
                }

                int count = 0;
                foreach (MDL0Node m in GetFolder<MDL0Node>().Children)
                {
                    count += 1;
                }

                return count;
            }
        }

        [Category("Models")]
        [Description(
            "How many points are stored in the models in this BRRES and sent to the GPU every frame. A lower value is better.")]
        public int NumFacepoints
        {
            get
            {
                if (Properties.Settings.Default.CompatibilityMode)
                {
                    return -1;
                }

                //Populate();
                if (GetFolder<MDL0Node>() == null)
                {
                    return 0;
                }

                int count = 0;
                foreach (MDL0Node m in GetFolder<MDL0Node>().Children)
                {
                    count += m.NumFacepoints;
                }

                return count;
            }
        }


        [Category("Models")]
        [Description(
            "How many individual vertices models in this BRRES have. A vertex in this case is only a point in space with its associated influence.")]
        public int NumVertices
        {
            get
            {
                if (Properties.Settings.Default.CompatibilityMode)
                {
                    return -1;
                }

                //Populate();
                if (GetFolder<MDL0Node>() == null)
                {
                    return 0;
                }

                int count = 0;
                foreach (MDL0Node m in GetFolder<MDL0Node>().Children)
                {
                    count += m.NumVertices;
                }

                return count;
            }
        }

        [Category("Models")]
        [Description("The total number of individual triangle faces models in this BRRES have.")]
        public int NumTriangles
        {
            get
            {
                if (Properties.Settings.Default.CompatibilityMode)
                {
                    return -1;
                }

                //Populate();
                if (GetFolder<MDL0Node>() == null)
                {
                    return 0;
                }

                int count = 0;
                foreach (MDL0Node m in GetFolder<MDL0Node>().Children)
                {
                    count += m.NumTriangles;
                }

                return count;
            }
        }

        [Category("Models")]
        [Description("The total number of matrices used in this BRRES (bones + weighted influences).")]
        public int NumNodes
        {
            get
            {
                if (Properties.Settings.Default.CompatibilityMode)
                {
                    return -1;
                }

                //Populate();
                if (GetFolder<MDL0Node>() == null)
                {
                    return 0;
                }

                int count = 0;
                foreach (MDL0Node m in GetFolder<MDL0Node>().Children)
                {
                    count += m.NumNodes;
                }

                return count;
            }
        }

        #endregion

        public override void OnPopulate()
        {
            ResourceGroup* group = Group;
            for (int i = 0; i < group->_numEntries; i++)
            {
                new BRESGroupNode(new string((sbyte*) group + group->First[i]._stringOffset)).Initialize(this,
                    (VoidPtr) group + group->First[i]._dataOffset, 0);
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            return Group->_numEntries > 0;
        }

        public bool HasFolder<T>() where T : BRESEntryNode
        {
            if (!HasChildren)
            {
                return false;
            }

            foreach (BRESGroupNode gr in Children)
            {
                if (gr.HasChildren)
                {
                    if (gr.Children[0] is T)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public BRESGroupNode GetFolder<T>() where T : BRESEntryNode
        {
            if (!HasChildren)
            {
                return null;
            }

            foreach (BRESGroupNode gr in Children)
            {
                if (gr.HasChildren)
                {
                    if (gr.Children[0] is T)
                    {
                        return gr;
                    }
                }
            }

            return null;
        }

        public BRESGroupNode GetOrCreateFolder<T>() where T : BRESEntryNode
        {
            string groupName;
            BRESGroupNode.BRESGroupType type;
            if (typeof(T) == typeof(TEX0Node))
            {
                type = BRESGroupNode.BRESGroupType.Textures;
                groupName = "Textures(NW4R)";
            }
            else if (typeof(T) == typeof(PLT0Node))
            {
                type = BRESGroupNode.BRESGroupType.Palettes;
                groupName = "Palettes(NW4R)";
            }
            else if (typeof(T) == typeof(MDL0Node))
            {
                type = BRESGroupNode.BRESGroupType.Models;
                groupName = "3DModels(NW4R)";
            }
            else if (typeof(T) == typeof(CHR0Node))
            {
                type = BRESGroupNode.BRESGroupType.CHR0;
                groupName = "AnmChr(NW4R)";
            }
            else if (typeof(T) == typeof(CLR0Node))
            {
                type = BRESGroupNode.BRESGroupType.CLR0;
                groupName = "AnmClr(NW4R)";
            }
            else if (typeof(T) == typeof(SRT0Node))
            {
                type = BRESGroupNode.BRESGroupType.SRT0;
                groupName = "AnmTexSrt(NW4R)";
            }
            else if (typeof(T) == typeof(PAT0Node))
            {
                type = BRESGroupNode.BRESGroupType.PAT0;
                groupName = "AnmTexPat(NW4R)";
            }
            else if (typeof(T) == typeof(SHP0Node))
            {
                type = BRESGroupNode.BRESGroupType.SHP0;
                groupName = "AnmShp(NW4R)";
            }
            else if (typeof(T) == typeof(VIS0Node))
            {
                type = BRESGroupNode.BRESGroupType.VIS0;
                groupName = "AnmVis(NW4R)";
            }
            else if (typeof(T) == typeof(SCN0Node))
            {
                type = BRESGroupNode.BRESGroupType.SCN0;
                groupName = "AnmScn(NW4R)";
            }
            else if (typeof(T) == typeof(RASDNode))
            {
                type = BRESGroupNode.BRESGroupType.External;
                groupName = "External";
            }
            else
            {
                return null;
            }

            BRESGroupNode group = null;
            foreach (BRESGroupNode node in Children)
            {
                if (node.Type == type)
                {
                    group = node;
                    break;
                }
            }

            if (group == null)
            {
                AddChild(group = new BRESGroupNode(groupName, type));
            }

            return group;
        }

        public T CreateResource<T>(string name) where T : BRESEntryNode
        {
            BRESGroupNode group = GetOrCreateFolder<T>();
            if (group == null)
            {
                return null;
            }

            T n = Activator.CreateInstance<T>();
            n.Name = group.FindName(name);
            group.AddChild(n);

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

            string ext = "";
            bool extract = true;
            foreach (BRESGroupNode group in Children)
            {
                extract = true;
                if (group.Type == BRESGroupNode.BRESGroupType.Textures)
                {
                    ext = imageExtension;
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.Models)
                {
                    ext = modelExtension;
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.CHR0)
                {
                    ext = ".chr0";
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.CLR0)
                {
                    ext = ".clr0";
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.SRT0)
                {
                    ext = ".srt0";
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.SHP0)
                {
                    ext = ".shp0";
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.PAT0)
                {
                    ext = ".pat0";
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.VIS0)
                {
                    ext = ".vis0";
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.SCN0)
                {
                    ext = ".scn0";
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.Palettes)
                {
                    ext = ".plt0";
                    if (imageExtension != ".tex0")
                    {
                        extract = false;
                    }
                }
                else
                {
                    extract = false;
                }

                if (extract)
                {
                    foreach (BRESEntryNode entry in group.Children)
                    {
                        entry.Export(Path.Combine(outFolder, entry.Name + ext));
                    }
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
            FileInfo[] files = dir.GetFiles();
            string ext = "*";
            foreach (BRESGroupNode group in Children)
            {
                if (group.Type == BRESGroupNode.BRESGroupType.Textures)
                {
                    ext = imageExtension;
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.Palettes)
                {
                    ext = ".plt0";
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.Models)
                {
                    ext = ".mdl0";
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.CHR0)
                {
                    ext = ".chr0";
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.CLR0)
                {
                    ext = ".clr0";
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.SRT0)
                {
                    ext = ".srt0";
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.SHP0)
                {
                    ext = ".shp0";
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.PAT0)
                {
                    ext = ".pat0";
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.VIS0)
                {
                    ext = ".vis0";
                }
                else if (group.Type == BRESGroupNode.BRESGroupType.SCN0)
                {
                    ext = ".scn0";
                }

                foreach (BRESEntryNode entry in group.Children)
                {
                    //Find file name for entry
                    foreach (FileInfo info in files)
                    {
                        if (info.Extension.Equals(ext, StringComparison.OrdinalIgnoreCase) &&
                            info.Name.Equals(entry.Name + ext, StringComparison.OrdinalIgnoreCase))
                        {
                            entry.Replace(info.FullName);
                            break;
                        }
                    }
                }
            }
        }

        public void ImportFolder(string inFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(inFolder);
            FileInfo[] files;

            files = dir.GetFiles();
            foreach (FileInfo info in files)
            {
                string ext = Path.GetExtension(info.FullName).ToUpper();
                if (ext == ".PNG" || ext == ".TGA" || ext == ".BMP" || ext == ".JPG" || ext == ".JPEG" ||
                    ext == ".GIF" || ext == ".TIF" || ext == ".TIFF")
                {
                    using (TextureConverterDialog dlg = new TextureConverterDialog())
                    {
                        dlg.ImageSource = info.FullName;
                        dlg.ShowDialog(null, this);
                    }
                }
                else if (ext == ".TEX0")
                {
                    TEX0Node node = NodeFactory.FromFile(null, info.FullName) as TEX0Node;
                    GetOrCreateFolder<TEX0Node>().AddChild(node);
                }
                else if (ext == ".MDL0")
                {
                    MDL0Node node = NodeFactory.FromFile(null, info.FullName) as MDL0Node;
                    GetOrCreateFolder<MDL0Node>().AddChild(node);
                }
                else if (ext == ".CHR0")
                {
                    CHR0Node node = NodeFactory.FromFile(null, info.FullName) as CHR0Node;
                    GetOrCreateFolder<CHR0Node>().AddChild(node);
                }
                else if (ext == ".CLR0")
                {
                    CLR0Node node = NodeFactory.FromFile(null, info.FullName) as CLR0Node;
                    GetOrCreateFolder<CLR0Node>().AddChild(node);
                }
                else if (ext == ".SRT0")
                {
                    SRT0Node node = NodeFactory.FromFile(null, info.FullName) as SRT0Node;
                    GetOrCreateFolder<SRT0Node>().AddChild(node);
                }
                else if (ext == ".SHP0")
                {
                    SHP0Node node = NodeFactory.FromFile(null, info.FullName) as SHP0Node;
                    GetOrCreateFolder<SHP0Node>().AddChild(node);
                }
                else if (ext == ".PAT0")
                {
                    PAT0Node node = NodeFactory.FromFile(null, info.FullName) as PAT0Node;
                    GetOrCreateFolder<PAT0Node>().AddChild(node);
                }
                else if (ext == ".VIS0")
                {
                    VIS0Node node = NodeFactory.FromFile(null, info.FullName) as VIS0Node;
                    GetOrCreateFolder<VIS0Node>().AddChild(node);
                }
            }
        }

        private int _numEntries, _strOffset, _rootSize;
        private readonly StringTable _stringTable = new StringTable();

        public override int OnCalculateSize(bool force)
        {
            int size = BRESHeader.Size;
            _rootSize = 0x20 + Children.Count * 0x10;

            //Get entry count and data start
            _numEntries = 0;
            //Children.Sort(NodeComparer.Instance);
            foreach (BRESGroupNode n in Children)
            {
                //n.Children.Sort(NodeComparer.Instance);
                _rootSize += n.Children.Count * 0x10 + 0x18;
                _numEntries += n.Children.Count;
            }

            size += _rootSize;

            //Get strings and advance entry offset
            _stringTable.Clear();
            foreach (BRESGroupNode n in Children)
            {
                _stringTable.Add(n.Name);
                foreach (BRESEntryNode c in n.Children)
                {
                    size = size.Align(c.DataAlign) + c.CalculateSize(force);
                    c.GetStrings(_stringTable);
                }
            }

            _strOffset = size = size.Align(4);

            size += _stringTable.GetTotalSize();

            return size.Align(0x80);
        }

        public override void OnRebuild(VoidPtr address, int size, bool force)
        {
            BRESHeader* header = (BRESHeader*) address;
            *header = new BRESHeader(size, _numEntries + 1);

            ROOTHeader* rootHeader = header->First;
            *rootHeader = new ROOTHeader(_rootSize, Children.Count);

            ResourceGroup* pMaster = &rootHeader->_master;
            ResourceGroup* rGroup = (ResourceGroup*) pMaster->EndAddress;

            //Write string table
            _stringTable.WriteTable(address + _strOffset);

            VoidPtr dataAddr = (VoidPtr) rootHeader + _rootSize;

            int gIndex = 1;
            foreach (BRESGroupNode g in Children)
            {
                ResourceEntry.Build(pMaster, gIndex++, rGroup, (BRESString*) _stringTable[g.Name]);

                *rGroup = new ResourceGroup(g.Children.Count);
                ResourceEntry* nEntry = rGroup->First;

                int rIndex = 1;
                foreach (BRESEntryNode n in g.Children)
                {
                    //Align data
                    dataAddr = ((int) dataAddr).Align(n.DataAlign);

                    ResourceEntry.Build(rGroup, rIndex++, dataAddr, (BRESString*) _stringTable[n.Name]);

                    //Rebuild entry
                    int len = n._calcSize;
                    n.Rebuild(dataAddr, len, force);
                    n.PostProcess(address, dataAddr, len, _stringTable);
                    dataAddr += len;
                }

                g._changed = false;

                //Advance to next group
                rGroup = (ResourceGroup*) rGroup->EndAddress;
            }

            _stringTable.Clear();
        }

        public void ImportGIF(string file)
        {
            Action<object, DoWorkEventArgs> work = (object sender, DoWorkEventArgs e) =>
            {
                GifDecoder decoder = new GifDecoder();
                decoder.Read(file, null);
                e.Result = decoder;
            };
            Action<object, RunWorkerCompletedEventArgs> completed = (object sender, RunWorkerCompletedEventArgs e) =>
            {
                GifDecoder decoder = e.Result as GifDecoder;
                string s = Path.GetFileNameWithoutExtension(file);
                PAT0Node p = CreateResource<PAT0Node>(s);
                p._loop = true;
                p.CreateEntry();

                PAT0TextureNode textureNode = p.Children[0].Children[0] as PAT0TextureNode;
                PAT0TextureEntryNode entry = textureNode.Children[0] as PAT0TextureEntryNode;

                //Get the number of images in the file
                int frames = decoder.GetFrameCount();

                //The framecount will be created by adding up each image delay.
                float frameCount = 0;

                bool resized = false;
                int w = 0, h = 0;
                Action<int, int> onResized = (newW, newH) =>
                {
                    if (resized != true)
                    {
                        w = newW;
                        h = newH;
                        resized = true;
                    }
                };

                using (TextureConverterDialog dlg = new TextureConverterDialog())
                {
                    using (ProgressWindow progress = new ProgressWindow(RootNode._mainForm, "GIF to PAT0 converter",
                        "Converting, please wait...", true))
                    {
                        Bitmap prev = null;

                        progress.Begin(0, frames, 0);
                        for (int i = 0; i < frames; i++, entry = new PAT0TextureEntryNode())
                        {
                            if (progress.Cancelled)
                            {
                                break;
                            }

                            string name = s + "." + i;

                            dlg.ImageSource = name + ".";

                            using (Bitmap img = (Bitmap) decoder.GetFrame(i))
                            {
                                if (i == 0)
                                {
                                    dlg.LoadImages(img.Copy());
                                    dlg.Resized += onResized;
                                    if (dlg.ShowDialog(null, this) != DialogResult.OK)
                                    {
                                        return;
                                    }

                                    textureNode._hasTex = dlg.TextureData != null;
                                    textureNode._hasPlt = dlg.PaletteData != null;

                                    prev = img.Copy();
                                }
                                else
                                {
                                    //Draw the current image over the previous
                                    //This is because some GIFs use pixels of the previous frame
                                    //in order to compress the overall image data
                                    using (Graphics graphics = Graphics.FromImage(prev))
                                    {
                                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                                        graphics.CompositingMode = CompositingMode.SourceOver;
                                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                                        graphics.DrawImage(img, 0, 0, prev.Width, prev.Height);
                                    }

                                    dlg.LoadImages(prev.Copy());
                                    if (resized)
                                    {
                                        dlg.ResizeImage(w, h);
                                    }
                                    else
                                    {
                                        dlg.UpdatePreview();
                                    }

                                    dlg.EncodeSource();

                                    textureNode.AddChild(entry);
                                }
                            }

                            entry._frame = (float) Math.Round(frameCount, 2);
                            frameCount += decoder.GetDelay(i) / 1000.0f * 60.0f;

                            if (textureNode._hasTex)
                            {
                                entry.Texture = name;
                            }

                            if (textureNode._hasPlt)
                            {
                                entry.Palette = name;
                            }

                            progress.Update(progress.CurrentValue + 1);
                        }

                        progress.Finish();
                        prev?.Dispose();
                    }
                }

                p._numFrames = (ushort) (frameCount + 0.5f);
            };

            using (BackgroundWorker b = new BackgroundWorker())
            {
                b.DoWork += new DoWorkEventHandler(work);
                b.RunWorkerCompleted += new RunWorkerCompletedEventHandler(completed);
                b.RunWorkerAsync();
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((BRESHeader*) source.Address)->_tag == BRESHeader.Tag ? new BRRESNode() : null;
        }
    }

    public unsafe class BRESGroupNode : ResourceNode, IImageSource
    {
        internal ResourceGroup* Group => (ResourceGroup*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.BRESGroup;

        [Browsable(false)]
        public int ImageCount => Children.Count > 0 && Children[0] is IImageSource ? Children.Count : 0;

        public Bitmap GetImage(int index)
        {
            if (Children.Count <= index || !(Children[index] is IImageSource))
            {
                return null;
            }

            return (Children[index] as IImageSource).GetImage(0);
        }

        public override Type[] AllowedChildTypes
        {
            get
            {
                switch (Type)
                {
                    case BRESGroupType.Textures:
                        return new Type[] {typeof(TEX0Node)};
                    case BRESGroupType.Palettes:
                        return new Type[] {typeof(PLT0Node)};
                    case BRESGroupType.Models:
                        return new Type[] {typeof(MDL0Node)};
                    case BRESGroupType.CHR0:
                        return new Type[] {typeof(CHR0Node)};
                    case BRESGroupType.CLR0:
                        return new Type[] {typeof(CLR0Node)};
                    case BRESGroupType.SRT0:
                        return new Type[] {typeof(SRT0Node)};
                    case BRESGroupType.SHP0:
                        return new Type[] {typeof(SHP0Node)};
                    case BRESGroupType.VIS0:
                        return new Type[] {typeof(VIS0Node)};
                    case BRESGroupType.SCN0:
                        return new Type[] {typeof(SCN0Node)};
                    case BRESGroupType.PAT0:
                        return new Type[] {typeof(PAT0Node)};
                    default:
                        return new Type[] { };
                }
            }
        }

        [Browsable(false)]
        public BRESGroupType Type
        {
            get
            {
                if (_type == BRESGroupType.None)
                {
                    GetFileType();
                }

                return _type;
            }
            set => _type = value;
        }

        public BRESGroupNode() : base()
        {
        }

        public BRESGroupNode(string name) : base()
        {
            _name = name;
        }

        public BRESGroupNode(string name, BRESGroupType type) : base()
        {
            _name = name;
            Type = type;
        }

        public enum BRESGroupType
        {
            Textures,
            Palettes,
            Models,
            CHR0,
            CLR0,
            SRT0,
            SHP0,
            VIS0,
            SCN0,
            PAT0,
            External,
            None
        }

        public BRESGroupType _type = BRESGroupType.None;

        public override void RemoveChild(ResourceNode child)
        {
            base.RemoveChild(child);
            if (Children.Count == 0)
            {
                Parent?.RemoveChild(this);
            }
        }

        public override bool OnInitialize()
        {
            return Group->_numEntries > 0;
        }

        public void GetFileType()
        {
            if (_name == "Textures(NW4R)" || (Children.Count > 0 && Children[0] is TEX0Node))
            {
                Type = BRESGroupType.Textures;
            }
            else if (_name == "Palettes(NW4R)" || (Children.Count > 0 && Children[0] is PLT0Node))
            {
                Type = BRESGroupType.Palettes;
            }
            else if (_name == "3DModels(NW4R)" || (Children.Count > 0 && Children[0] is MDL0Node))
            {
                Type = BRESGroupType.Models;
            }
            else if (_name == "AnmChr(NW4R)" || (Children.Count > 0 && Children[0] is CHR0Node))
            {
                Type = BRESGroupType.CHR0;
            }
            else if (_name == "AnmClr(NW4R)" || (Children.Count > 0 && Children[0] is CLR0Node))
            {
                Type = BRESGroupType.CLR0;
            }
            else if (_name == "AnmTexSrt(NW4R)" || (Children.Count > 0 && Children[0] is SRT0Node))
            {
                Type = BRESGroupType.SRT0;
            }
            else if (_name == "AnmShp(NW4R)" || (Children.Count > 0 && Children[0] is SHP0Node))
            {
                Type = BRESGroupType.SHP0;
            }
            else if (_name == "AnmVis(NW4R)" || (Children.Count > 0 && Children[0] is VIS0Node))
            {
                Type = BRESGroupType.VIS0;
            }
            else if (_name == "AnmScn(NW4R)" || (Children.Count > 0 && Children[0] is SCN0Node))
            {
                Type = BRESGroupType.SCN0;
            }
            else if (_name == "AnmPat(NW4R)" || (Children.Count > 0 && Children[0] is PAT0Node))
            {
                Type = BRESGroupType.PAT0;
            }
            else if (_name == "External" || (Children.Count > 0 && Children[0] is RASDNode))
            {
                Type = BRESGroupType.External;
            }
        }

        public override void OnPopulate()
        {
            ResourceGroup* group = Group;
            for (int i = 0; i < group->_numEntries; i++)
            {
                BRESCommonHeader* hdr = (BRESCommonHeader*) group->First[i].DataAddress;
                if (NodeFactory.FromAddress(this, hdr, hdr->_size) == null)
                {
                    switch (Type)
                    {
                        case BRESGroupType.Textures:
                            new TEX0Node().Initialize(this, hdr, hdr->_size);
                            break;
                        case BRESGroupType.Models:
                            new MDL0Node().Initialize(this, hdr, hdr->_size);
                            break;
                        case BRESGroupType.Palettes:
                            new PLT0Node().Initialize(this, hdr, hdr->_size);
                            break;
                        case BRESGroupType.CHR0:
                            new CHR0Node().Initialize(this, hdr, hdr->_size);
                            break;
                        case BRESGroupType.CLR0:
                            new CLR0Node().Initialize(this, hdr, hdr->_size);
                            break;
                        case BRESGroupType.SHP0:
                            new SHP0Node().Initialize(this, hdr, hdr->_size);
                            break;
                        case BRESGroupType.VIS0:
                            new VIS0Node().Initialize(this, hdr, hdr->_size);
                            break;
                        case BRESGroupType.PAT0:
                            new PAT0Node().Initialize(this, hdr, hdr->_size);
                            break;
                        case BRESGroupType.SCN0:
                            new SCN0Node().Initialize(this, hdr, hdr->_size);
                            break;
                        case BRESGroupType.SRT0:
                            new SRT0Node().Initialize(this, hdr, hdr->_size);
                            break;
                        default:
                            new BRESEntryNode().Initialize(this, hdr, hdr->_size);
                            break;
                    }
                }
            }

            if (Type == BRESGroupType.None)
            {
                GetFileType();
            }
        }
    }

    public unsafe class NW4RAnimationNode : BRESEntryNode
    {
        internal BRESCommonHeader* Header => (BRESCommonHeader*) WorkingUncompressed.Address;

        public int _numFrames = 1;
        public bool _loop;

        [Browsable(true)]
        public virtual int FrameCount
        {
            get => _numFrames;
            set
            {
                if (_numFrames == value || value < 1)
                {
                    return;
                }

                _numFrames = value;
                UpdateChildFrameLimits();
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        public virtual bool Loop
        {
            get => _loop;
            set
            {
                if (_loop != value)
                {
                    _loop = value;
                    SignalPropertyChange();
                }
            }
        }

        protected virtual void UpdateChildFrameLimits()
        {
        }
    }

    public unsafe class BRESEntryNode : ResourceNode
    {
        internal BRESCommonHeader* CommonHeader => (BRESCommonHeader*) WorkingSource.Address;
        [Browsable(false)] public virtual int DataAlign => 4;

        [Browsable(false)]
        public BRRESNode BRESNode => _parent != null && Parent.Parent is BRRESNode ? Parent.Parent as BRRESNode : null;

        [Browsable(false)] public virtual int[] SupportedVersions => new int[0];

        public virtual string Tag { get; }

        public int _version;
        public string _originalPath;
        internal UserDataCollection _userEntries = new UserDataCollection();

        [Category("G3D Node")]
        [Browsable(true)]
        public int Version
        {
            get => _version;
            set
            {
                if (SupportedVersions.Contains(value))
                {
                    int previous = _version;
                    _version = value;
                    SignalPropertyChange();
                    OnVersionChanged(previous);
                }
                else
                {
                    string message =
                        "The version entered for this node is either invalid or unsupported.\nSupported versions are: ";
                    foreach (int i in SupportedVersions)
                    {
                        message += " " + i;
                    }

                    MessageBox.Show(RootNode._mainForm, message, "Unsupported version", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
            }
        }

        [Category("G3D Node")]
        [Browsable(true)]
        public string OriginalPath
        {
            get => _originalPath;
            set
            {
                _originalPath = value;
                SignalPropertyChange();
            }
        }

        [Category("User Data")]
        [Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public UserDataCollection UserEntries
        {
            get => _userEntries;
            set
            {
                _userEntries = value;
                SignalPropertyChange();
            }
        }

        protected virtual void OnVersionChanged(int previousVersion)
        {
        }

        public override bool OnInitialize()
        {
            _userEntries = new UserDataCollection();
            _version = CommonHeader->_version;
            SetSizeInternal(CommonHeader->_size);
            if (WorkingUncompressed.Length >= 4 && !string.IsNullOrEmpty(Tag) && !WorkingUncompressed.Tag.Equals(Tag))
            {
                SignalPropertyChange();
            }
            return false;
        }

        internal virtual void GetStrings(StringTable strings)
        {
            strings.Add(Name);
        }

        public override void Export(string outPath)
        {
            Rebuild();

            StringTable table = new StringTable();
            GetStrings(table);

            int dataLen = WorkingUncompressed.Length.Align(4);
            int size = dataLen + table.GetTotalSize();

            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                FileShare.None, 8, FileOptions.RandomAccess))
            {
                stream.SetLength(size);
                using (FileMap map = FileMap.FromStream(stream))
                {
                    Memory.Move(map.Address, WorkingUncompressed.Address, (uint) WorkingUncompressed.Length);
                    table.WriteTable(map.Address + dataLen);
                    PostProcess(null, map.Address, WorkingUncompressed.Length, table);
                }
            }

            table.Clear();
        }

        protected internal virtual void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength,
                                                    StringTable stringTable)
        {
            BRESCommonHeader* header = (BRESCommonHeader*) dataAddress;

            if (bresAddress)
            {
                header->_bresOffset = (int) bresAddress - (int) header;
            }
            else
            {
                // AFAIK only the Export method calls this method with a bresAddress of null - it should be OK to put 0 in here
                header->_bresOffset = 0;
            }

            header->_size = dataLength;
        }

        /// <summary>
        /// Find the MD5 checksum of this node's data.
        /// Before calculating the checksum, the data will be copied to a
        /// temporary area in memory and PostProcess will be run just as in Export().
        /// </summary>
        public override byte[] MD5()
        {
            if (WorkingUncompressed.Address == null || WorkingUncompressed.Length == 0)
            {
                // skip bres fix
                return base.MD5();
            }

            Rebuild();

            StringTable table = new StringTable();
            GetStrings(table);

            int dataLen = WorkingUncompressed.Length.Align(4);
            int size = dataLen + table.GetTotalSize();

            byte[] datacopy = new byte[size];
            fixed (byte* ptr = datacopy)
            {
                Memory.Move(ptr, WorkingUncompressed.Address, (uint) WorkingUncompressed.Length);
                table.WriteTable(ptr + dataLen);
                PostProcess(null, ptr, WorkingUncompressed.Length, table);
            }

            return MD5Provider.ComputeHash(datacopy);
        }
    }
}