using BrawlCrate.BrawlManagers.StageManager.SingleUseDialogs;
using BrawlLib.BrawlManagerLib;
using BrawlLib.BrawlManagerLib.GCT.ReadWrite;
using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Textures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.StageManager
{
    public partial class PortraitViewer : UserControl
    {
        #region Public fields and properties

        public Size? prevbaseResizeTo;
        public Size? frontstnameResizeTo;
        public Size? selmapMarkResizeTo;
        public NameCreatorSettings fontSettings;

        public bool useExistingAsFallback = true;
        public WiiPixelFormat? selmapMarkFormat;
        public bool selmapMarkPreview;
        public bool useTextureConverter;

        public bool IsDirty =>
            common5 != null ? common5.IsDirty
            : sc_selmap != null ? sc_selmap.IsDirty
            : false;

        public bool SelmapLoaded => sc_selmap != null;

        public TextureContainer.Texture GetTexInfoFor(object sender)
        {
            return
                sender == prevbase ? textures.prevbase :
                sender == icon ? textures.icon :
                sender == frontstname ? textures.frontstname :
                sender == seriesicon ? textures.seriesicon :
                sender == selmap_mark ? textures.selmap_mark :
                null;
        }

        public string OpenFilePath => _openFilePath;

        #endregion

        #region Private fields

        /// <summary>
        /// The common5 currently being used. If using sc_selcharacter.pac instead, this will be null.
        /// </summary>
        private ResourceNode common5;

        /// <summary>
        /// Either the sc_selmap_en archive within common5.pac or the sc_selmap.pac file.
        /// </summary>
        private ResourceNode sc_selmap;

        /// <summary>
        /// The mu_menumain path to copy changes to, if one was found and the feature is enabled.
        /// </summary>
        private string mu_menumain_path;

        private TextureContainer textures;

        private string _openFilePath;

        private Bitmap scribble;

        // In case the image needs to be reloaded after replacing the texture
        private int _iconNum;

        #endregion

        #region Custom SSS

        public CustomSSSCodeset DefaultSSS, _autoSSS, _manualSSS;

        public CustomSSSCodeset AutoSSS
        {
            get => _autoSSS;
            set
            {
                _autoSSS = value;
                label1.Text = (ManualSSS ?? AutoSSS) != null ? "Loaded " + BestSSS : "No custom SSS loaded";
            }
        }

        public CustomSSSCodeset ManualSSS
        {
            get => _manualSSS;
            set
            {
                _manualSSS = value;
                label1.Text = (ManualSSS ?? AutoSSS) != null ? "Loaded " + BestSSS : "No custom SSS loaded";
            }
        }

        public CustomSSSCodeset BestSSS => ManualSSS ?? AutoSSS ?? DefaultSSS;

        #endregion

        public PortraitViewer()
        {
            InitializeComponent();

            #region default sss

            string s = @"Pretty generic custom SSS (based on CEP 5.5)
* 046B8F5C 7C802378
* 046B8F64 7C6300AE
* 040AF618 5460083C
* 040AF68C 38840002
* 040AF6AC 5463083C
* 040AF6C0 88030001
* 040AF6E8 3860FFFF
* 040AF59C 3860000C
* 060B91C8 00000018
* BFA10014 7CDF3378
* 7CBE2B78 7C7D1B78
* 2D05FFFF 418A0014
* 006B929C 00000027
* 066B99D8 00000027
* 00010203 04050709
* 080A0B0C 0D0E0F10
* 11141516 1A191217
* 0618131D 1E1B1C1F
* 20212223 24252600
* 006B92A4 00000027
* 066B9A58 00000027
* 27282A2B 2C2D2E2F
* 30313233 34353637
* 38393A3B 3C3D3E3F
* 40414243 44454647
* 48494A4B 4C4D4E00
* 06407AAC 0000009E
* 01010202 03030404
* 05050606 07070808
* 0909330A 0B0B0C0C
* 0D0D0E0E 130F1410
* 15111612 17131814
* 19151C16 1D171E18
* 1F19201A 211B221C
* 231D241E 251F2932
* 2A332B34 2C352D36
* 2F373038 3139323A
* 2E3BFFFF 40204121
* 42224323 44244525
* 46264727 48284929
* 4A2A4B2B 4C2C4D2D
* 4E2E4F2F 50305131
* 523D533E 543F5540
* 56415742 58435944
* 5A455B46 5C475D48
* 5E495F4A 604B614C
* 624D634E 644F0000";
            DefaultSSS = new CustomSSSCodeset(s.Split('\n'));

            #endregion

            _iconNum = -1;
            fileSizeBar.Style = ProgressBarStyle.Continuous;

            foreach (Control child in flowLayoutPanel1.Controls)
            {
                if (child is ImagePreviewPanel)
                {
                    (child as ImagePreviewPanel).DragEnter += panel1_DragEnter;
                    (child as ImagePreviewPanel).DragDrop += panel1_DragDrop;
                }
            }

            //UpdateDirectory();
        }

        #region Core public methods

        public void UpdateImage()
        {
            UpdateImage(_iconNum);
        }

        public void UpdateImage(int iconNum)
        {
            if (btnGenerateName.InvokeRequired)
            {
                Invoke((MethodInvoker) (() => { UpdateImage(iconNum); }));
                return;
            }

            prevbase.BackgroundImage = null;
            _iconNum = -1;

            TextureContainer retval = get_icons(iconNum);
            if (retval == null)
            {
            }
            else
            {
                textures = retval ?? new TextureContainer();
                foreach (Control child in flowLayoutPanel1.Controls)
                {
                    if (child is ImagePreviewPanel)
                    {
                        setBG(child as ImagePreviewPanel);
                    }
                }

                btnGenerateName.Visible = textures.prevbase.tex0 != null;
                lblIconTex.Text = "Icon no.: " + iconNum;

                TEX0Node tex = textures.icon.tex0;
                PLT0Node pal = tex == null ? null : textures.icon.tex0.GetPaletteNode();
                if (textures.prevbase.tex0 != null && textures.frontstname.tex0 != null)
                {
                    label1.Text = "P: " + textures.prevbase.tex0.ToSizeString()
                                        + ", F: " + textures.frontstname.tex0.ToSizeString()
                                        + ", icon: " + (pal == null ? "null" : pal.Colors + "c")
                                        + "\nmark: " + textures.selmap_mark.tex0.ToSizeString();
                    if (textures.selmap_mark.tex0 != null)
                    {
                        label1.Text += " " + textures.selmap_mark.tex0.Format;
                    }
                }

                _iconNum = iconNum;
            }
        }

        public void UpdateDirectory()
        {
            Console.WriteLine(Environment.CurrentDirectory);
            sc_selmap?.Dispose();

            common5?.Dispose();

            _openFilePath = null;
            fileSizeBar.Maximum = 1214283;
            if (File.Exists("../../menu2/sc_selmap.pac"))
            {
                common5 = null;
                sc_selmap = TempFiles.MakeTempNode("../../menu2/sc_selmap.pac");
                _openFilePath = "../../menu2/sc_selmap.pac";
            }
            else if (File.Exists("../../menu2/sc_selmap_en.pac"))
            {
                common5 = null;
                sc_selmap = TempFiles.MakeTempNode("../../menu2/sc_selmap_en.pac");
                _openFilePath = "../../menu2/sc_selmap_en.pac";
            }
            else if (File.Exists("../../system/common5.pac"))
            {
                common5 = TempFiles.MakeTempNode("../../system/common5.pac");
                sc_selmap = common5.FindChild("sc_selmap_en", false);
                _openFilePath = "../../system/common5.pac";
            }
            else if (File.Exists("../../system/common5_en.pac"))
            {
                common5 = TempFiles.MakeTempNode("../../system/common5_en.pac");
                sc_selmap = common5.FindChild("sc_selmap_en", false);
                _openFilePath = "../../system/common5_en.pac";
            }
            else
            {
                common5 = null;
                sc_selmap = null;
                label1.Text = "Could not load sc_selmap(_en) or common5(_en).";
            }

            if (_openFilePath != null)
            {
                updateFileSize();
                TEX0Node tex0 =
                    sc_selmap.FindChild("Misc Data [80]/Textures(NW4R)/MenSelmapFrontBg", false) as TEX0Node;
                if (tex0 != null)
                {
                    scribble = tex0.GetImage(0);
                }

                FindMuMenumain();
            }
            else
            {
                fileSizeBar.Value = 0;
                fileSizeLabel.Text = "";
            }

            // Find and load GCT, if it exists
            AutoSSS = null;
            DirectoryInfo directory = new DirectoryInfo(Environment.CurrentDirectory);
            while (directory != null)
            {
                if (File.Exists(directory.FullName + "/data/gecko/codes/RSBE01.gct"))
                {
                    AutoSSS = new CustomSSSCodeset(
                        File.ReadAllBytes(directory.FullName + "/data/gecko/codes/RSBE01.gct"));
                    break;
                }
                else if (File.Exists(directory.FullName + "/codes/RSBE01.gct"))
                {
                    AutoSSS = new CustomSSSCodeset(File.ReadAllBytes(directory.FullName + "/codes/RSBE01.gct"));
                    break;
                }
                else if (File.Exists(directory.FullName + "/LegacyTE/RSBE01.gct"))
                {
                    AutoSSS = new CustomSSSCodeset(File.ReadAllBytes(directory.FullName + "/LegacyTE/RSBE01.gct"));
                    break;
                }
                else if (File.Exists(directory.FullName + "/LegacyXP/RSBE01.gct"))
                {
                    AutoSSS = new CustomSSSCodeset(File.ReadAllBytes(directory.FullName + "/LegacyXP/RSBE01.gct"));
                    break;
                }

                directory = directory.Parent;
            }

            if (sc_selmap != null)
            {
                ResourceNode pat0Folder = sc_selmap.FindChild("Misc Data [80]/AnmTexPat(NW4R)", false);
                PopulateByPrintingNames("PAT0: ", pat0Folder);
            }
        }

        private static void PopulateByPrintingNames(string prefix, ResourceNode node)
        {
            foreach (ResourceNode c in node.Children)
            {
                Debug.WriteLine(prefix + c.Name);
                PopulateByPrintingNames(prefix + "  ", c);
            }
        }

        public void LoadCustomSSS(string file)
        {
            if (File.Exists(file))
            {
                ManualSSS = new CustomSSSCodeset(file);
                MessageBox.Show(
                    "You will need to quit and restart this program if you want to go back to loading the GCT codeset automatically.");
            }
        }

        public void Replace(object sender, string filename)
        {
            StringComparison ig = StringComparison.CurrentCultureIgnoreCase;
            if (filename.EndsWith(".tex0", ig))
            {
                TEX0Node tex0 = GetTexInfoFor(sender).tex0;
                if (tex0 == null)
                {
                    return;
                }

                tex0.Replace(filename);
                tex0.IsDirty = true;
                UpdateImage();
            }
            else if (filename.EndsWith(".brres", ig))
            {
                using (ResourceNode node = NodeFactory.FromFile(null, filename))
                {
                    TEX0Node tex0;
                    if (node is TEX0Node)
                    {
                        tex0 = (TEX0Node) node;
                    }
                    else
                    {
                        tex0 = (TEX0Node) node.FindChild("Textures(NW4R)", false).Children[0];
                    }

                    string tempFile = TempFiles.Create(".png");
                    tex0.Export(tempFile);
                    Replace(sender, tempFile); // call self with new file
                }
            }
            else
            {
                TEX0Node tex0 = GetTexInfoFor(sender).tex0;
                if (tex0 == null)
                {
                    AddNewTEX0(sender, filename);
                    return;
                }
                else if (useTextureConverter)
                {
                    using (TextureConverterDialog dlg = new TextureConverterDialog())
                    {
                        dlg.ImageSource = filename;
                        dlg.InitialSize =
                            sender == prevbase ? prevbaseResizeTo
                            : sender == frontstname ? frontstnameResizeTo
                            : sender == selmap_mark ? selmapMarkResizeTo
                            : null;
                        dlg.FormBorderStyle = FormBorderStyle.FixedSingle;
                        dlg.ShowInTaskbar = true;
                        if (dlg.ShowDialog(null, tex0) == DialogResult.OK)
                        {
                            tex0.IsDirty = true;
                            UpdateImage();
                        }
                    }
                }
                else
                {
                    Bitmap bmp = new Bitmap(filename);
                    if (sender == prevbase && prevbaseResizeTo != null)
                    {
                        bmp = BitmapUtilities.Resize(bmp, prevbaseResizeTo.Value);
                    }
                    else if (sender == frontstname && frontstnameResizeTo != null)
                    {
                        bmp = BitmapUtilities.Resize(bmp, frontstnameResizeTo.Value);
                    }
                    else if (sender == selmap_mark && selmapMarkResizeTo != null)
                    {
                        bmp = BitmapUtilities.Resize(bmp, selmapMarkResizeTo.Value);
                    }

                    if (sender == selmap_mark)
                    {
                        ReplaceSelmapMark(bmp, tex0, false);
                    }
                    else
                    {
                        int colorCount = BitmapUtilities.CountColors(bmp, 256).Align(16);
                        tex0.Replace(bmp, colorCount);
                    }

                    tex0.IsDirty = true;
                    UpdateImage();
                }
            }
        }

        private void AddNewTEX0(object sender, string filename)
        {
            BRRESNode md80 = sc_selmap.FindChild("Misc Data [80]", false) as BRRESNode;
            string dir = Path.Combine(Path.GetTempPath(), "BrawlCrate.StageManager-newimage-" + Guid.NewGuid());
            Directory.CreateDirectory(dir);
            string temp = Path.Combine(dir, GetTexInfoFor(sender).pat0.Texture + Path.GetExtension(filename));
            File.Copy(filename, temp);
            using (TextureConverterDialog dlg = new TextureConverterDialog())
            {
                dlg.ImageSource = temp;
                dlg.InitialFormat =
                    sender == prevbase ? WiiPixelFormat.CMPR
                    : sender == frontstname ? WiiPixelFormat.I4
                    : sender == icon ? WiiPixelFormat.CI8
                    : sender == seriesicon ? WiiPixelFormat.I4
                    : sender == selmap_mark ? WiiPixelFormat.IA4
                    : (WiiPixelFormat?) null;
                dlg.InitialSize =
                    sender == prevbase ? prevbaseResizeTo
                    : sender == frontstname ? frontstnameResizeTo
                    : sender == selmap_mark ? selmapMarkResizeTo
                    : null;
                dlg.FormBorderStyle = FormBorderStyle.FixedSingle;
                dlg.ShowInTaskbar = true;
                if (dlg.ShowDialog(null, md80) == DialogResult.OK)
                {
                    md80.IsDirty = true; // do this to be safe
                    UpdateImage();
                }
            }

            Directory.Delete(dir, true);
        }

        public void save()
        {
            if (sc_selmap == null)
            {
                return;
            }

            ResourceNode toSave = common5 ?? sc_selmap;
            try
            {
                toSave.Merge();
                toSave.Export(_openFilePath);
            }
            catch (IOException)
            {
                toSave.Export(_openFilePath + ".out.pac");
                MessageBox.Show(
                    _openFilePath + " could not be accessed.\nFile written to " + _openFilePath + ".out.pac");
            }

            updateFileSize();
        }

        public void exportAll(string folder)
        {
            BRRESNode bres = sc_selmap?.FindChild("Misc Data [80]", false) as BRRESNode;
            bres?.ExportToFolder(folder, ".png");
        }

        #endregion

        #region Private methods

        private void setBG(Panel panel)
        {
            TextureContainer.Texture texInfo = GetTexInfoFor(panel);
            Bitmap bgi = null;
            if (texInfo.tex0 == null)
            {
                Bitmap b = new Bitmap(1, 1);
                b.SetPixel(0, 0, Color.Brown);
                bgi = b;
            }
            else
            {
                Bitmap image = new Bitmap(texInfo.tex0.GetImage(0));
                if (panel == seriesicon && selmapMarkPreview)
                {
                    bgi = BitmapUtilities.Invert(BitmapUtilities.AlphaSwap(image));
                }
                else if (panel == prevbase && selmapMarkPreview && scribble != null)
                {
                    bgi = BitmapUtilities.ApplyMask(image, scribble);
                }
                else
                {
                    bgi = image;
                }

                if (bgi.Size != panel.Size)
                {
                    bgi = BitmapUtilities.Resize(bgi, panel.Size);
                }
            }

            if (!texInfo.ForThisFrameIndex)
            {
                bgi = BitmapUtilities.Border(bgi, Color.Brown, 2);
            }

            panel.BackgroundImage = bgi;
        }

        private void updateFileSize()
        {
            long length;
            if (common5 != null)
            {
                string tempfile = Path.GetTempFileName();
                sc_selmap.Export(tempfile);
                length = new FileInfo(tempfile).Length;
                File.Delete(tempfile);
            }
            else
            {
                length = new FileInfo(_openFilePath).Length;
            }

            fileSizeBar.Value = Math.Min((int) fileSizeBar.Maximum, (int) length);
            fileSizeLabel.Text = length + " / " + fileSizeBar.Maximum;
        }

        private TextureContainer get_icons(int iconNum)
        {
            if (common5 != null)
            {
                saveButton.Text = "Save common5";
            }
            else if (sc_selmap != null)
            {
                saveButton.Text = "Save sc_selmap";
            }
            else
            {
                return null;
            }

            TextureContainer result = new TextureContainer(sc_selmap, iconNum);
            return result;
        }

        private bool FindMuMenumain()
        {
            mu_menumain_path = null;
            string[] lookIn =
            {
                "../../menu2/mu_menumain.pac",
                "../../menu2/mu_menumain_en.pac",
                "../../../pfmenu2/mu_menumain.pac",
                "../../../pfmenu2/mu_menumain_en.pac"
            };
            foreach (string path in lookIn)
            {
                if (File.Exists(path))
                {
                    mu_menumain_path = path;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Replace the MenSelmapMark texture in toReplace with the image in newBitmap, flipping the channels if the image has solid black in all four corners.
        /// </summary>
        /// <param name="newBitmap">The new texture to use</param>
        /// <param name="toReplace">The TEX0 to insert the texture in</param>
        /// <param name="createNew">If true, the format of the existing texture will not be used as a fallback, even if useExistingAsFallback is true</param>
        private void ReplaceSelmapMark(Bitmap newBitmap, TEX0Node toReplace, bool createNew)
        {
            WiiPixelFormat format =
                selmapMarkFormat != null
                    ? selmapMarkFormat.Value
                    : useExistingAsFallback && !createNew
                        ? toReplace.Format
                        : BitmapUtilities.HasAlpha(newBitmap) & BitmapUtilities.HasNonAlpha(newBitmap)
                            ? WiiPixelFormat.IA4
                            : WiiPixelFormat.I4;
            Bitmap toEncode = format == WiiPixelFormat.IA4 && BitmapUtilities.HasSolidCorners(newBitmap)
                ? BitmapUtilities.AlphaSwap(newBitmap)
                : newBitmap;
            FileMap tMap = TextureConverter.Get(format).EncodeTEX0Texture(toEncode, 1);
            toReplace.ReplaceRaw(tMap);
        }

        /// <summary>
        /// Adds PAT0 entries for each stage to the given PAT0TextureNode.
        /// </summary>
        /// <param name="pathToPAT0TextureNode">Path relative to sc_selmap_en</param>
        /// <param name="addNew">Whether to add new textures that match the PAT0 entries if those textures don't exist.</param>
        /// <param name="defaultName">The texture name to be used for new PAT0 entries. If null, the name will be taken from the first entry, with the number at the end replaced with the icon number.</param>
        public void AddPAT0(string pathToPAT0TextureNode, bool addNew, string defaultName = null)
        {
            ResourceNode look = sc_selmap.FindChild(pathToPAT0TextureNode, false).Children[0];
            if (!(look is PAT0TextureNode))
            {
                throw new FormatException(look.Name);
            }

            bool icon = look.Parent.Name == "iconM";

            PAT0TextureNode tn = look as PAT0TextureNode;

            List<PAT0TextureEntryNode> childrenList = (from c in tn.Children
                                                       where c is PAT0TextureEntryNode
                                                       select (PAT0TextureEntryNode) c).ToList();
            if ((from c in childrenList where c.FrameIndex >= 40 && c.FrameIndex < 50 select c).Count() >= 10)
            {
                MessageBox.Show("Skipping " +
                                pathToPAT0TextureNode.Substring(pathToPAT0TextureNode.LastIndexOf('/') + 1) +
                                " - mappings for icon numbers 40-49 already exist.");
                return;
            }

            string texToCopyName = null;
            List<Tuple<string, float>> entries = new List<Tuple<string, float>>();
            foreach (PAT0TextureEntryNode entry in childrenList)
            {
                if (entry.Texture == null)
                {
                    MessageBox.Show("BrawlLib cannot read PAT0 texture name(s) from " + pathToPAT0TextureNode);
                    return;
                }

                entries.Add(new Tuple<string, float>(entry.Texture, entry.FrameIndex));
                if (entry.FrameIndex == 1)
                {
                    texToCopyName = entry.Texture;
                }

                if (entry.FrameIndex != 0)
                {
                    tn.RemoveChild(entry);
                }
            }

            string basename = (from e in entries
                               where e.Item1.Contains('.')
                               select e.Item1).First();
            basename = basename.Substring(0, basename.LastIndexOf('.'));

            Func<int, string> getTexStringByIconNumber = iconNum =>
            {
                if (iconNum < 32 || iconNum >= 50 && iconNum < 60 || iconNum == 80)
                {
                    string previousTexture = null;
                    foreach (Tuple<string, float> entry in entries)
                    {
                        if (entry.Item2 > iconNum)
                        {
                            break;
                        }

                        previousTexture = entry.Item1;
                    }

                    return previousTexture;
                }
                else
                {
                    return defaultName ?? basename + "." + iconNum.ToString("D2");
                }
            };

            for (int i = 1; i <= 80; i++)
            {
                string texname = getTexStringByIconNumber(i);
                PAT0TextureEntryNode entry = new PAT0TextureEntryNode();
                tn.AddChild(entry);
                entry.FrameIndex = i;
                entry.Texture = texname;
                if (icon)
                {
                    entry.Palette = entry.Texture;
                }
            }

            IOrderedEnumerable<Tuple<string, float>> moreThan79query = from e in entries
                                                                       where e.Item2 > 80
                                                                       orderby e.Item2 ascending
                                                                       select e;
            foreach (Tuple<string, float> tuple in moreThan79query)
            {
                PAT0TextureEntryNode entry = new PAT0TextureEntryNode();
                tn.AddChild(entry);
                entry.FrameIndex = tuple.Item2;
                entry.Texture = tuple.Item1;
                if (icon)
                {
                    entry.Palette = entry.Texture;
                }
            }

            if (addNew)
            {
                ResourceNode brres = tn;
                while (brres != null && !(brres is BRRESNode))
                {
                    brres = brres.Parent;
                }

                if (brres != null)
                {
                    ResourceNode folder = brres.FindChild("Textures(NW4R)", false);
                    TEX0Node texToCopy = texToCopyName == null
                        ? null
                        : folder.FindChild(texToCopyName, false) as TEX0Node;
                    PLT0Node pltToCopy = texToCopyName == null
                        ? null
                        : brres.FindChild("Palettes(NW4R)", false).FindChild(texToCopyName, false) as PLT0Node;

                    foreach (ResourceNode c in tn.Children)
                    {
                        PAT0TextureEntryNode p = c as PAT0TextureEntryNode;
                        if (p == null)
                        {
                            continue;
                        }

                        ResourceNode texture = folder.FindChild(p.Texture, false);
                        if (texture == null)
                        {
                            if (texToCopy != null)
                            {
                                TEX0Node tex0 = ((BRRESNode) brres).CreateResource<TEX0Node>(p.Texture);
                                tex0.ReplaceRaw(texToCopy.WorkingUncompressed.Address,
                                    texToCopy.WorkingUncompressed.Length);
                            }

                            if (pltToCopy != null)
                            {
                                PLT0Node plt0 = ((BRRESNode) brres).CreateResource<PLT0Node>(p.Texture);
                                plt0.ReplaceRaw(pltToCopy.WorkingUncompressed.Address,
                                    pltToCopy.WorkingUncompressed.Length);
                            }
                        }
                        else if (texture.Index == 1)
                        {
                            texToCopy = texture as TEX0Node;
                        }
                    }
                }
            }
        }

        #endregion

        #region Public methods - special operations

        public bool AddMenSelmapMark(string path, bool ask)
        {
            string tmp = null;
            if (path.EndsWith(".tex0", StringComparison.InvariantCultureIgnoreCase))
            {
                tmp = TempFiles.Create(".png");
                NodeFactory.FromFile(null, path).Export(tmp);
            }

            Bitmap bitmap = new Bitmap(tmp ?? path);
            string name = Path.GetFileNameWithoutExtension(path);
            if (ask)
            {
                using (AskNameDialog nameDialog = new AskNameDialog(bitmap))
                {
                    nameDialog.Text = name;
                    if (nameDialog.ShowDialog() != DialogResult.OK)
                    {
                        return false;
                    }
                    else
                    {
                        name = nameDialog.NameText;
                    }
                }
            }

            BRRESNode bres = sc_selmap.FindChild("Misc Data [80]", false) as BRRESNode;
            TEX0Node tex0 = bres.CreateResource<TEX0Node>(name);
            ReplaceSelmapMark(bitmap, tex0, true);
            return true;
        }

        public void copyIconsToSelcharacter2()
        {
            string fileToSaveTo = null;

            ResourceNode s2 = null;
            if (common5 != null)
            {
                s2 = common5.FindChild("sc_selcharacter2_en", false);
            }
            else if (sc_selmap != null)
            {
                if (File.Exists("../../menu2/sc_selcharacter2.pac"))
                {
                    fileToSaveTo = "../../menu2/sc_selcharacter2.pac";
                    s2 = TempFiles.MakeTempNode(fileToSaveTo);
                }
                else if (File.Exists("../../menu2/sc_selcharacter2_en.pac"))
                {
                    fileToSaveTo = "../../menu2/sc_selcharacter2_en.pac";
                    s2 = TempFiles.MakeTempNode(fileToSaveTo);
                }
            }

            if (s2 == null)
            {
                return;
            }

            ResourceNode md0 = s2.FindChild("MenuRule_en/ModelData[0]", false);
            MSBinNode md1 = s2.FindChild("MenuRule_en/Misc Data [1]", false) as MSBinNode;
            ResourceNode md80 = sc_selmap.FindChild("Misc Data [80]", false);
            if (md0 == null || md80 == null)
            {
                return;
            }

            Image[] icons = new Image[41];
            Image[] frontstnames = new Image[41];
            for (int i = 1; i < 60; i++)
            {
                if (i == 32)
                {
                    i = 50;
                }

                int sssPos = StageIDMap.sssPositionForSelcharacter2Icon(i);
                string nameSelmap = BestSSS[sssPos].Item2.ToString("D2");
                icons[sssPos] = (md80.FindChild("Textures(NW4R)/MenSelmapIcon." + nameSelmap, false) as TEX0Node)
                    .GetImage(0);
                frontstnames[sssPos] =
                    (md80.FindChild("Textures(NW4R)/MenSelmapFrontStname." + nameSelmap, false) as TEX0Node)
                    .GetImage(0);
            }

            RandomSelectEditNamesDialog d = new RandomSelectEditNamesDialog(md1._strings, icons, frontstnames);
            d.Message = "When finished, press OK to continue.";
            if (d.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < md1._strings.Count; i++)
                {
                    md1._strings[i] = d[i].ToString();
                }

                md1.Rebuild(true);
            }
            else
            {
                return;
            }

            using (ProgressWindow w = new ProgressWindow {CanCancel = false})
            {
                w.Begin(0, 60, 0);
                for (int i = 1; i < 60; i++)
                {
                    if (i == 32)
                    {
                        i = 50;
                    }

                    int sssPos = StageIDMap.sssPositionForSelcharacter2Icon(i);
                    string tempFile1 = TempFiles.Create(".tex0");
                    string tempFile2 = TempFiles.Create(".plt0");
                    string nameSelcharacter2 = i.ToString("D2");
                    string nameSelmap = BestSSS[sssPos].Item2.ToString("D2");
                    Console.WriteLine($"{nameSelcharacter2}: sss pos {sssPos}, icon {nameSelmap}");
                    TEX0Node iconFrom = md80.FindChild("Textures(NW4R)/MenSelmapIcon." + nameSelmap, false) as TEX0Node;
                    TEX0Node iconTo =
                        md0.FindChild("Textures(NW4R)/MenSelmapIcon." + nameSelcharacter2, false) as TEX0Node;
                    ResourceNode palFrom = md80.FindChild("Palettes(NW4R)/MenSelmapIcon." + nameSelmap, false);
                    ResourceNode palTo = md0.FindChild("Palettes(NW4R)/MenSelmapIcon." + nameSelcharacter2, false);
                    if (iconFrom != null && iconTo != null && palFrom != null && palTo != null)
                    {
                        iconFrom.Export(tempFile1);
                        iconTo.Replace(tempFile1);
                        palFrom.Export(tempFile2);
                        palTo.Replace(tempFile2);
                    }

                    TEX0Node prevbase =
                        md80.FindChild("Textures(NW4R)/MenSelmapPrevbase." + nameSelmap, false) as TEX0Node;
                    TEX0Node stageswitch =
                        md0.FindChild("Textures(NW4R)/MenStageSwitch." + nameSelcharacter2, false) as TEX0Node;
                    if (prevbase != null && stageswitch != null)
                    {
                        Bitmap thumbnail = new Bitmap(112, 56);
                        using (Graphics g = Graphics.FromImage(thumbnail))
                        {
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.DrawImage(prevbase.GetImage(0), 0, -28, 112, 112);
                        }

                        stageswitch.Replace(thumbnail);
                    }

                    w.Update(i);
                }
            }

            if (fileToSaveTo != null)
            {
                s2.Export(fileToSaveTo);
            }
        }

        public void ExportImages(int p, string thisdir)
        {
            TextureContainer texs = get_icons(p);
            if (texs == null)
            {
                return;
            }

            texs.prevbase.tex0?.Export(thisdir + "/MenSelmapPrevbase.png");

            texs.icon.tex0?.Export(thisdir + "/MenSelmapIcon.png");

            texs.frontstname.tex0?.Export(thisdir + "/MenSelmapFrontStname.png");

            texs.seriesicon.tex0?.Export(thisdir + "/MenSelchrMark.png");

            texs.selmap_mark.tex0?.Export(thisdir + "/MenSelmapMark.png");
        }

        public void openModifyPAT0Dialog()
        {
            modifyPAT0.PerformClick();
        }

        public void generateName()
        {
            using (NameDialog n = new NameDialog())
            {
                n.EntryText = "Battlefield";
                n.LabelText =
                    "Enter the stage name. (Use \\n for a line break.)\nType just ] to launch genname.bat/genname.exe instead.\nPrefix with } to put message in bottom 14px rows.";
                if (n.ShowDialog() == DialogResult.OK)
                {
                    if (n.EntryText == "]")
                    {
                        generateNameExternal();
                    }
                    else if (n.EntryText.StartsWith("}"))
                    {
                        Bitmap img = textures.frontstname.tex0.GetImage(0);
                        Bitmap bmp = NameCreator.putMessageInBottomRow(
                            new Font("Lucida Console", 18, FontStyle.Bold, GraphicsUnit.Pixel), img,
                            n.EntryText.Substring(1));
                        string tempfile = TempFiles.Create(".png");
                        bmp.Save(tempfile);
                        Replace(frontstname, tempfile);
                    }
                    else
                    {
                        if (fontSettings == null)
                        {
                            changeFrontStnameFont();
                        }

                        if (fontSettings == null)
                        {
                            return;
                        }

                        Bitmap bmp = NameCreator.createImage(fontSettings, n.EntryText);
                        string tempfile = TempFiles.Create(".png");
                        bmp.Save(tempfile);
                        Replace(frontstname, tempfile);
                    }
                }
            }
        }

        public void generateNameExternal()
        {
            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string tempfile = TempFiles.Create(".png");
            ProcessStartInfo start = new ProcessStartInfo
            {
                WorkingDirectory = exeDir,
                FileName = File.Exists(exeDir + "\\genname.bat") ? "genname.bat" : "genname.exe",
                Arguments = tempfile
            };
            if (!File.Exists(exeDir + "\\" + start.FileName))
            {
                MessageBox.Show(this,
                    "Could not find genname.bat or genname.exe.\nCreate a program or batch file that takes an output PNG file path as its only argument. It should write to this path before closing.");
                return;
            }

            using (Process p = Process.Start(start))
            {
                p?.WaitForExit();
                if (!File.Exists(tempfile))
                {
                    MessageBox.Show(this,
                        "The program did not write to the temporary file path. Make sure it's using %1 (first argument) as the output filename.");
                    return;
                }
            }

            Replace(frontstname, tempfile);
        }

        public void repaintIconBorder()
        {
            icon.changeBorder();
        }

        public void changeFrontStnameFont()
        {
            fontSettings = NameCreator.selectFont(fontSettings) ?? fontSettings;
        }

        public string MenSelmapMarkUsageReport()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            string pathToPAT0TextureNode = "Misc Data [80]/AnmTexPat(NW4R)/MenSelmapPreview/pasted__stnamelogoM";
            ResourceNode look = sc_selmap.FindChild(pathToPAT0TextureNode, false).Children[0];
            if (!(look is PAT0TextureNode))
            {
                throw new FormatException(look.Name);
            }

            PAT0TextureNode tn = look as PAT0TextureNode;

            string pathToTextures = "Misc Data [80]/Textures(NW4R)";
            ResourceNode textures = sc_selmap.FindChild(pathToTextures, false);
            List<string> marks = (from c in textures.Children
                                  where c.Name.Contains("MenSelmapMark")
                                  select c.Name).ToList();

            var q = from c in
                        from c in tn.Children
                        where c is PAT0TextureEntryNode
                        select (PAT0TextureEntryNode) c
                    where marks.Contains(c.Texture)
                    group c by c.Texture
                    into g
                    let count = g.Count()
                    let stageID = BestSSS.StageForIcon((int) g.First().FrameIndex)
                    let one = StageIDMap.PacBasenameForStageID(stageID)
                    orderby count, g.Key
                    select new {count, g.Key, one};
            StringBuilder sb = new StringBuilder();
            foreach (var line in q)
            {
                sb.AppendLine(line.Key + ": " +
                              (line.count == 1 ? line.one : line.count.ToString()));
                marks.Remove(line.Key);
            }

            foreach (string name in marks)
            {
                sb.AppendLine(name + ": NOT USED");
            }

            return sb.ToString();
        }

        public void updateMuMenumain(string msBinPath = null)
        {
            if (DialogResult.OK == MessageBox.Show("Overwrite the current mu_menumain?", "Overwrite File",
                    MessageBoxButtons.OKCancel))
            {
                using (ResourceNode mu_menumain = TempFiles.MakeTempNode(mu_menumain_path))
                {
                    IconsToMenumain.Copy(sc_selmap, mu_menumain, BestSSS);
                    if (msBinPath != null)
                    {
                        mu_menumain.FindChild("Misc Data [7]", false).Replace(msBinPath);
                    }

                    mu_menumain.Export(mu_menumain_path);
                }

                byte absent_stage_id = BestSSS[0x1E].Item1;
                int sss2_count = BestSSS.sss2.Where(b => b != 0x1E).Count() + 1;
                string warn = sss2_count <= 39
                    ? ""
                    : "\nWARNING: screen 2 of the SSS " +
                      (AutoSSS == null ? "may have " : "has ") +
                      "more than 39 stages, causing My Music to crash on page 2.";
                IEnumerable<Stage> q = StageIDMap.Stages.Where(s => s.ID == absent_stage_id);
                string absent = q.Any()
                    ? q.First().Name
                    : "STGCUSTOM" + (absent_stage_id - 0x3f).ToString("X2");
                MessageBox.Show("Done. " +
                                (msBinPath != null ? "(Song titles copied too.) " : "") +
                                (AutoSSS == null ? "Without a Custom SSS code, " : "Based on your current SSS code, ") +
                                absent + " will be missing; and Menu will be added to the end of screen 2." + warn);
            }
        }

        public void ResizeAllPrevbases(Size newSize)
        {
            if (sc_selmap == null)
            {
                return;
            }

            IEnumerable<TEX0Node> prevbases =
                from c in sc_selmap.FindChild("Misc Data [80]/Textures(NW4R)", false).Children
                where c is TEX0Node && c.Name.Contains("MenSelmapPrevbase")
                select (TEX0Node) c;
            int i = 0;
            foreach (TEX0Node node in prevbases)
            {
                Bitmap origImage = node.GetImage(0);
                if (origImage.Width <= newSize.Width && origImage.Height <= newSize.Height)
                {
                    continue;
                }

                string file = TempFiles.Create(".png");
                if (useTextureConverter)
                {
                    origImage.Save(file);

                    TextureConverterDialog d = new TextureConverterDialog();
                    d.ImageSource = file;
                    d.InitialSize = newSize;
                    if (d.ShowDialog(null, node) == DialogResult.OK)
                    {
                        node.IsDirty = true;
                        Console.WriteLine("Resized " + node);
                        i++;
                    }
                    else if (MessageBox.Show(this, "Stop resizing textures here?", Text, MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        break;
                    }
                }
                else
                {
                    Bitmap b = BitmapUtilities.Resize(origImage, newSize);
                    b.Save(file);

                    node.Replace(file);
                    Console.WriteLine("Resized " + node);
                    i++;
                }

                try
                {
                    File.Delete(file);
                }
                catch (IOException e)
                {
                    Console.Error.WriteLine("Warning: " + e.Message);
                }
            }

            MessageBox.Show("Resized " + i + " images.");
            UpdateImage();
        }

        #endregion

        #region event handlers

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] s = (string[]) e.Data.GetData(DataFormats.FileDrop);
                if (s.Length == 1)
                {
                    // Can only drag and drop one file
                    string filename = s[0].ToLower();
                    if (filename.EndsWith(".png") || filename.EndsWith(".gif")
                                                  || filename.EndsWith(".tex0") || filename.EndsWith(".brres"))
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                }
            }
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Copy)
            {
                string s = (e.Data.GetData(DataFormats.FileDrop) as string[])[0];
                new System.Threading.ThreadStart(() => { Replace(sender, s); }).BeginInvoke(null, null);
            }
        }

        private void AltImage_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] s = (string[]) e.Data.GetData(DataFormats.FileDrop);
                if (s.Length == 1)
                {
                    // Can only drag and drop one file
                    string filename = s[0].ToLower();
                    if (filename.EndsWith(".png") || filename.EndsWith(".gif"))
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                }
            }
        }

        private void lblPMTop_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Copy)
            {
                string s = (e.Data.GetData(DataFormats.FileDrop) as string[])[0];
                new Task(() =>
                {
                    Bitmap existing = GetTexInfoFor(prevbase).tex0.GetImage(0);
                    Image newBitmap = Image.FromFile(s);
                    Bitmap upperPortion = new Bitmap(176, 103);
                    using (Graphics g = Graphics.FromImage(upperPortion))
                    {
                        int height = (int) ((double) newBitmap.Height / newBitmap.Width * 176);
                        int offset = (103 - height) / 2;
                        g.DrawImage(newBitmap, 0, offset, 176, height);
                    }

                    Bitmap canvas = new Bitmap(176, 176);
                    using (Graphics g = Graphics.FromImage(canvas))
                    {
                        g.DrawImage(existing, 0, 0, 176, 176);
                        g.DrawImage(upperPortion, 0, 23, 176, 103);
                        g.FillRectangle(Brushes.Black, 0, 0, 176, 23);
                    }

                    string temp = TempFiles.Create(".png");
                    canvas.Save(temp);
                    Replace(prevbase, temp);
                }).Start();
            }
        }

        private void lblPMAlt_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Copy)
            {
                string s = (e.Data.GetData(DataFormats.FileDrop) as string[])[0];
                new Task(() =>
                {
                    Bitmap existing = GetTexInfoFor(prevbase).tex0.GetImage(0);
                    Image newBitmap = Image.FromFile(s);
                    Bitmap lowerPortion = new Bitmap(106, 24);
                    using (Graphics g = Graphics.FromImage(lowerPortion))
                    {
                        int height = (int) ((double) newBitmap.Height / newBitmap.Width * 106);
                        int offset = (24 - height) / 2;
                        g.DrawImage(newBitmap, 0, offset, 106, height);
                    }

                    Bitmap canvas = new Bitmap(176, 176);
                    using (Graphics g = Graphics.FromImage(canvas))
                    {
                        g.DrawImage(existing, 0, 0, 176, 176);
                        g.FillRectangle(Brushes.Black, 0, 126, 176, 50);
                        g.DrawImage(lowerPortion, 52, 131, 106, 24);
                    }

                    string temp = TempFiles.Create(".png");
                    canvas.Save(temp);
                    Replace(prevbase, temp);
                }).Start();
            }
        }

        private void legacyImageDrop(object sender, DragEventArgs e, int x, int y, int width, int height,
                                     string overlayFile = null)
        {
            if (e.Effect == DragDropEffects.Copy)
            {
                string s = (e.Data.GetData(DataFormats.FileDrop) as string[])[0];
                new Task(() =>
                {
                    try
                    {
                        Bitmap existing = GetTexInfoFor(prevbase).tex0.GetImage(0);
                        Image newBitmap = Image.FromFile(s);
                        Bitmap scaled = new Bitmap(width, height);
                        using (Graphics g = Graphics.FromImage(scaled))
                        {
                            int h = (int) ((double) newBitmap.Height / newBitmap.Width * scaled.Width);
                            int offset = (scaled.Height - h) / 2;
                            g.DrawImage(newBitmap, 0, offset, scaled.Width, h);
                        }

                        Bitmap canvas = new Bitmap(176, 176);
                        using (Graphics g = Graphics.FromImage(canvas))
                        {
                            g.DrawImage(existing, 0, 0, 176, 176);
                            g.DrawImage(scaled, x, y, scaled.Width, scaled.Height);
                            if (overlayFile != null)
                            {
                                Stream stream = Assembly.GetExecutingAssembly()
                                                        .GetManifestResourceStream(
                                                            "BrawlCrate.StageManager." + overlayFile);
                                if (stream != null)
                                {
                                    Image overlayImage = Image.FromStream(stream) as Bitmap;
                                    g.DrawImage(overlayImage, x, y);
                                }
                            }

                            // Keep a pittle bit of padding
                            g.FillRectangle(Brushes.Black, 0, 46, 176, 2);
                            g.FillRectangle(Brushes.Black, 0, 127, 176, 2);
                            g.FillRectangle(Brushes.Black, 87, 128, 2, 48);
                        }

                        string temp = TempFiles.Create(".png");
                        canvas.Save(temp);
                        Replace(prevbase, temp);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }).Start();
            }
        }

        private void lblLegacyTop_DragDrop(object sender, DragEventArgs e)
        {
            legacyImageDrop(sender, e, 0, 0, 176, 48, "button-start.png");
        }

        private void lblLegacyCenter_DragDrop(object sender, DragEventArgs e)
        {
            legacyImageDrop(sender, e, 0, 48, 176, 80);
        }

        private void lblLegacyBottomLeft_DragDrop(object sender, DragEventArgs e)
        {
            legacyImageDrop(sender, e, 0, 128, 88, 48, "button-l.png");
        }

        private void lblLegacyBottomRight_DragDrop(object sender, DragEventArgs e)
        {
            legacyImageDrop(sender, e, 88, 128, 88, 48, "button-z.png");
        }

        protected void saveButton_Click(object sender, EventArgs e)
        {
            save();
        }

        private void modifyPAT0_Click(object sender, EventArgs e)
        {
            if (textures == null)
            {
                return;
            }

            DialogResult result = new ModifyPAT0Dialog(textures).ShowDialog();
            if (result == DialogResult.OK)
            {
                // The dialog will mark the pat0 as dirty if changed
                UpdateImage();
            }
        }

        private void btnGenerateName_Click(object sender, EventArgs e)
        {
            generateName();
        }

        #endregion
    }
}