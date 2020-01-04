using BrawlCrate.ExternalInterfacing;
using BrawlCrate.UI;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types.Animations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.TEX0)]
    [NodeWrapper(ResourceType.SharedTEX0)]
    public class TEX0Wrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;
        private static readonly ContextMenuStrip MultiSelectMenu;

        private static readonly ToolStripMenuItem GeneratePAT0ToolStripMenuItem =
            new ToolStripMenuItem("Generate &PAT0", null, GeneratePAT0Action);

        private static readonly ToolStripMenuItem ConvertStocksToolStripMenuItem =
            new ToolStripMenuItem("Convert Stock System", null, ConvertStockAction);

        private static readonly ToolStripMenuItem DuplicateToolStripMenuItem =
            new ToolStripMenuItem("&Duplicate", null, DuplicateAction, Keys.Control | Keys.D);

        private static readonly ToolStripMenuItem ReplaceToolStripMenuItem =
            new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R);

        private static readonly ToolStripMenuItem RestoreToolStripMenuItem =
            new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T);

        private static readonly ToolStripMenuItem MoveUpToolStripMenuItem =
            new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up);

        private static readonly ToolStripMenuItem MoveDownToolStripMenuItem =
            new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down);

        private static readonly ToolStripMenuItem DeleteToolStripMenuItem =
            new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete);

        private static readonly ToolStripMenuItem ColorSmashSelectedToolStripMenuItem =
            new ToolStripMenuItem("&Color Smash", null, ColorSmash.ColorSmashTex0, Keys.Control | Keys.Shift | Keys.C);

        private static readonly ToolStripMenuItem ExportSelectedToolStripMenuItem =
            new ToolStripMenuItem("&Export Selected", null, ExportSelectedAction, Keys.Control | Keys.E);

        private static readonly ToolStripMenuItem DeleteSelectedToolStripMenuItem =
            new ToolStripMenuItem("&Delete Selected", null, DeleteSelectedAction, Keys.Control | Keys.Delete);

        static TEX0Wrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Re-Encode", null, ReEncodeAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(GeneratePAT0ToolStripMenuItem);
            _menu.Items.Add(ConvertStocksToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(DuplicateToolStripMenuItem);
            _menu.Items.Add(ReplaceToolStripMenuItem);
            _menu.Items.Add(RestoreToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(MoveUpToolStripMenuItem);
            _menu.Items.Add(MoveDownToolStripMenuItem);
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(DeleteToolStripMenuItem);
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;

            MultiSelectMenu = new ContextMenuStrip();
            MultiSelectMenu.Items.Add(ColorSmashSelectedToolStripMenuItem);
            MultiSelectMenu.Items.Add(new ToolStripSeparator());
            MultiSelectMenu.Items.Add(ExportSelectedToolStripMenuItem);
            MultiSelectMenu.Items.Add(DeleteSelectedToolStripMenuItem);
            MultiSelectMenu.Opening += MultiMenuOpening;
            MultiSelectMenu.Closing += MultiMenuClosing;
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            GeneratePAT0ToolStripMenuItem.Enabled = true;
            ConvertStocksToolStripMenuItem.Enabled = true;
            ConvertStocksToolStripMenuItem.Visible = true;
            DuplicateToolStripMenuItem.Enabled = true;
            ReplaceToolStripMenuItem.Enabled = true;
            RestoreToolStripMenuItem.Enabled = true;
            MoveUpToolStripMenuItem.Enabled = true;
            MoveDownToolStripMenuItem.Enabled = true;
            DeleteToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            TEX0Wrapper w = GetInstance<TEX0Wrapper>();

            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
            if (w._resource.Name.StartsWith("InfStc.") && Regex.Match(w._resource.Name, @"(\.\d+)?$").Success &&
                w._resource.Name.LastIndexOf(".") > 0 && w._resource.Name.LastIndexOf(".") <= w._resource.Name.Length &&
                int.TryParse(
                    w._resource.Name.Substring(w._resource.Name.LastIndexOf(".") + 1,
                        w._resource.Name.Length - (w._resource.Name.LastIndexOf(".") + 1)), out int _))
            {
                ConvertStocksToolStripMenuItem.Enabled = true;
                ConvertStocksToolStripMenuItem.Visible = true;
                ConvertStocksToolStripMenuItem.Text = w._resource.Name.Length == 10
                    ? "Convert to Expanded 50-Stock System"
                    : "Convert to Default Stock System";
            }
            else
            {
                ConvertStocksToolStripMenuItem.Enabled = false;
                ConvertStocksToolStripMenuItem.Visible = false;
            }
        }

        private static void MultiMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            ColorSmashSelectedToolStripMenuItem.Enabled = true;
            DeleteSelectedToolStripMenuItem.Visible = true;
            DeleteSelectedToolStripMenuItem.Enabled = true;
        }

        private static void MultiMenuOpening(object sender, CancelEventArgs e)
        {
            TEX0Wrapper w = GetInstance<TEX0Wrapper>();
            if (!ColorSmash.CanRunColorSmash)
            {
                ColorSmashSelectedToolStripMenuItem.Enabled = false;
            }
            foreach (TreeNode n in MainForm.Instance.resourceTree.SelectedNodes)
            {
                if (((TEX0Wrapper)n)?._resource.Parent == null)
                {
                    DeleteSelectedToolStripMenuItem.Visible = false;
                    DeleteSelectedToolStripMenuItem.Enabled = false;
                    ColorSmashSelectedToolStripMenuItem.Enabled = false;
                    break;
                }
                if (((TEX0Wrapper)n)?._resource.Parent != w._resource.Parent)
                {
                    ColorSmashSelectedToolStripMenuItem.Enabled = false;
                }
            }
        }

        protected static void ReEncodeAction(object sender, EventArgs e)
        {
            GetInstance<TEX0Wrapper>().ReEncode();
        }

        protected static void GeneratePAT0Action(object sender, EventArgs e)
        {
            GetInstance<TEX0Wrapper>().GeneratePAT0(false);
        }

        protected static void ConvertStockAction(object sender, EventArgs e)
        {
            GetInstance<TEX0Wrapper>().ConvertStocks();
        }

        #endregion

        public override ContextMenuStrip MultiSelectMenuStrip => MultiSelectMenu;

        public TEX0Wrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.TEX0;

        public override void OnReplace(string inStream)
        {
            if (inStream.EndsWith(".tex0", StringComparison.OrdinalIgnoreCase) || !inStream.Contains("."))
            {
                base.OnReplace(inStream);
            }
            else
            {
                using (TextureConverterDialog dlg = new TextureConverterDialog())
                {
                    dlg.ImageSource = inStream;
                    dlg.ShowDialog(MainForm.Instance, Resource as TEX0Node);
                }
            }
        }

        public void ReEncode()
        {
            PLT0Node plt = null;
            if (((TEX0Node) _resource).HasPalette)
            {
                plt = ((TEX0Node) _resource).GetPaletteNode();
            }

            using (TextureConverterDialog dlg = new TextureConverterDialog())
            {
                dlg.LoadImages((Resource as TEX0Node).GetImage(0));
                dlg.ShowDialog(MainForm.Instance, Resource as TEX0Node);
            }

            if (plt != null && !((TEX0Node) _resource).HasPalette)
            {
                plt.Dispose();
                plt.Remove();
            }
        }

        protected internal override void OnPropertyChanged(ResourceNode node)
        {
            RefreshView(node);
        }

        public PAT0Node GeneratePAT0(bool force)
        {
            if (Parent == null)
            {
                return null;
            }

            if (_resource.Parent is BRESGroupNode && _resource.Parent.Parent != null &&
                _resource.Parent.Parent is BRRESNode)
            {
                // Check if this is part of a sequence
                if (Regex.Match(_resource.Name, @"(\.\d+)?$").Success && _resource.Name.LastIndexOf(".") > 0 &&
                    _resource.Name.LastIndexOf(".") <= _resource.Name.Length && int.TryParse(
                        _resource.Name.Substring(_resource.Name.LastIndexOf(".") + 1,
                            _resource.Name.Length - (_resource.Name.LastIndexOf(".") + 1)), out int n))
                {
                    //Console.WriteLine(_resource.Name.Substring(0, _resource.Name.LastIndexOf(".")) + " is part of a sequence");
                    //Console.WriteLine(_resource.Name.Substring(_resource.Name.LastIndexOf(".") + 1, _resource.Name.Length - (_resource.Name.LastIndexOf(".") + 1)));
                    // Determine the name to match
                    string matchName = _resource.Name.Substring(0, _resource.Name.LastIndexOf(".")) + ".";
                    BRESGroupNode paletteGroup = ((BRRESNode) _resource.Parent.Parent).GetFolder<PLT0Node>();
                    List<string> textureList = new List<string>();
                    List<PLT0Node> paletteList = new List<PLT0Node>();
                    int highestNum = -1;
                    bool isStock = false;
                    bool isStockEx = false;
                    if (matchName.Equals("InfStc.", StringComparison.OrdinalIgnoreCase))
                    {
                        isStock = true;
                        if (_resource.Name.Length >= 11)
                        {
                            isStockEx = true;
                        }
                    }

                    foreach (TEX0Node tx0 in _resource.Parent.Children)
                    {
                        if (tx0.Name.StartsWith(matchName) && tx0.Name.LastIndexOf(".") > 0 &&
                            tx0.Name.LastIndexOf(".") <= tx0.Name.Length &&
                            int.TryParse(
                                tx0.Name.Substring(tx0.Name.LastIndexOf(".") + 1,
                                    tx0.Name.Length - (tx0.Name.LastIndexOf(".") + 1)), out int n2) && n2 >= 0 &&
                            !textureList.Contains(tx0.Name))
                        {
                            if (isStock)
                            {
                                if (isStockEx && tx0.Name.Length < 11)
                                {
                                    continue;
                                }
                            }

                            // Add the matching texture to the texture list for the PAT0
                            textureList.Add(tx0.Name);
                            // Determine the highest number used
                            if (n2 > highestNum)
                            {
                                highestNum = n2;
                            }

                            Console.WriteLine(tx0.Name);
                            Console.WriteLine(tx0.HasPalette);
                            if (tx0.HasPalette)
                            {
                                paletteList.Add(tx0.GetPaletteNode());
                            }
                            else
                            {
                                paletteList.Add(null);
                            }
                        }
                    }

                    if (textureList.Count <= 0)
                    {
                        return null;
                    }

                    PAT0Node newPat0 = new PAT0Node
                    {
                        Name = _resource.Name.Substring(0, _resource.Name.LastIndexOf(".")).Equals("InfStc")
                            ? "InfStockface_TopN__0"
                            : _resource.Name.Substring(0, _resource.Name.LastIndexOf(".")),
                        _numFrames = highestNum + 1
                    };
                    PAT0EntryNode pat0Entry = new PAT0EntryNode();
                    newPat0.AddChild(pat0Entry);
                    pat0Entry.Name = _resource.Name.Substring(0, _resource.Name.LastIndexOf(".")).Equals("InfStc")
                        ? "lambert87"
                        : _resource.Name.Substring(0, _resource.Name.LastIndexOf("."));
                    PAT0TextureNode pat0Tex = new PAT0TextureNode((PAT0Flags) 7, 0);
                    pat0Entry.AddChild(pat0Tex);
                    if (((BRRESNode) _resource.Parent.Parent).GetFolder<PAT0Node>() != null &&
                        ((BRRESNode) _resource.Parent.Parent).GetFolder<PAT0Node>().FindChildrenByName(newPat0.Name)
                                                             .Length > 0)
                    {
                        if (force)
                        {
                            while (((BRRESNode) _resource.Parent.Parent).GetFolder<PAT0Node>()
                                                                        .FindChildrenByName(newPat0.Name).Length > 0)
                            {
                                ((BRRESNode) _resource.Parent.Parent).GetFolder<PAT0Node>()
                                                                     .FindChildrenByName(newPat0.Name)[0].Remove();
                            }
                        }
                        else
                        {
                            DialogResult d = MessageBox.Show(
                                "Would you like to replace the currently existing \"" + newPat0.Name +
                                "\" PAT0 animation?", "PAT0 Generator", MessageBoxButtons.YesNoCancel);
                            if (d == DialogResult.Cancel || d == DialogResult.Abort)
                            {
                                return null;
                            }

                            if (d == DialogResult.Yes)
                            {
                                while (((BRRESNode) _resource.Parent.Parent).GetFolder<PAT0Node>()
                                                                            .FindChildrenByName(newPat0.Name).Length >
                                       0)
                                {
                                    ((BRRESNode) _resource.Parent.Parent).GetFolder<PAT0Node>()
                                                                         .FindChildrenByName(newPat0.Name)[0].Remove();
                                }
                            }
                        }
                    }

                    if (isStock && !isStockEx && !textureList.Contains("InfStc.000"))
                    {
                        textureList.Add("InfStc.000");
                        paletteList.Add(null);
                    }
                    else if (isStock && isStockEx && !textureList.Contains("InfStc.0000"))
                    {
                        textureList.Add("InfStc.0000");
                        paletteList.Add(null);
                    }

                    //foreach(string s in textureList)
                    for (int i = 0; i < textureList.Count; ++i)
                    {
                        string s = textureList[i];
                        if (float.TryParse(s.Substring(s.LastIndexOf(".") + 1, s.Length - (s.LastIndexOf(".") + 1)),
                            out float fr))
                        {
                            PAT0TextureEntryNode pat0texEntry = new PAT0TextureEntryNode();
                            pat0Tex.AddChild(pat0texEntry);
                            pat0texEntry.Name = s;
                            pat0texEntry._frame = fr;
                            if (paletteList[i] != null)
                            {
                                pat0Tex.HasPalette = true;
                                pat0texEntry._plt = paletteList[i].Name;
                            }
                            else if ((s == "InfStc.000" || s == "InfStc.0000") && pat0Tex.HasPalette)
                            {
                                pat0texEntry._plt = s;
                            }

                            if (fr == 0)
                            {
                                PAT0TextureEntryNode pat0texEntryFinal = new PAT0TextureEntryNode();
                                pat0Tex.AddChild(pat0texEntryFinal);
                                pat0texEntryFinal.Name = s;
                                pat0texEntryFinal._frame = highestNum + 1;
                                if (paletteList[i] != null)
                                {
                                    pat0Tex.HasPalette = true;
                                    pat0texEntryFinal.Palette = paletteList[i].Name;
                                }
                                else if ((s == "InfStc.000" || s == "InfStc.0000") && pat0Tex.HasPalette)
                                {
                                    pat0texEntryFinal._plt = s;
                                }
                            }
                        }

                        //newPat0.AddChild
                    }

                    pat0Tex._children = pat0Tex._children.OrderBy(o => ((PAT0TextureEntryNode) o)._frame).ToList();
                    if (isStock && !isStockEx && newPat0.FrameCount < 501)
                    {
                        newPat0.FrameCount = 501;
                    }
                    else if (isStockEx && newPat0.FrameCount < 9201)
                    {
                        newPat0.FrameCount = 9201;
                    }

                    ((BRRESNode) _resource.Parent.Parent).GetOrCreateFolder<PAT0Node>().AddChild(newPat0);
                    if (!force)
                    {
                        MainForm.Instance.TargetResource(newPat0);
                    }

                    return newPat0;
                }
                else
                {
                    PAT0Node newPat0 = new PAT0Node
                    {
                        Name = _resource.Name,
                        _numFrames = 1
                    };
                    PAT0EntryNode pat0Entry = new PAT0EntryNode();
                    newPat0.AddChild(pat0Entry);
                    pat0Entry.Name = _resource.Name;
                    PAT0TextureNode pat0Tex = new PAT0TextureNode((PAT0Flags) 7, 0);
                    pat0Entry.AddChild(pat0Tex);
                    PAT0TextureEntryNode pat0texEntry = new PAT0TextureEntryNode();
                    pat0Tex.AddChild(pat0texEntry);
                    pat0texEntry.Name = _resource.Name;
                    pat0texEntry._frame = 0;
                    if (((TEX0Node) _resource).HasPalette)
                    {
                        pat0Tex.HasPalette = true;
                        pat0texEntry.Palette = ((TEX0Node) _resource).GetPaletteNode().Name;
                    }

                    ((BRRESNode) _resource.Parent.Parent).GetOrCreateFolder<PAT0Node>().AddChild(newPat0);
                    MainForm.Instance.TargetResource(newPat0);
                    return newPat0;
                }
            }

            return null;
        }

        public override void Delete()
        {
            if (Parent == null)
            {
                return;
            }

            PLT0Node p = ((TEX0Node) _resource).GetPaletteNode();
            if (((TEX0Node) _resource).HasPalette && p != null &&
                MessageBox.Show("Would you like to delete the associated PLT0?", $"Deleting {_resource.Name}",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                p.Dispose();
                p.Remove();
            }

            base.Delete();
        }

        public void ConvertStocks()
        {
            if (Parent == null)
            {
                return;
            }

            if (_resource.Parent is BRESGroupNode && _resource.Parent.Parent != null &&
                _resource.Parent.Parent is BRRESNode)
            {
                // Check if this is part of a sequence
                if (Regex.Match(_resource.Name, @"(\.\d+)?$").Success && _resource.Name.LastIndexOf(".") > 0 &&
                    _resource.Name.LastIndexOf(".") <= _resource.Name.Length && int.TryParse(
                        _resource.Name.Substring(_resource.Name.LastIndexOf(".") + 1,
                            _resource.Name.Length - (_resource.Name.LastIndexOf(".") + 1)), out int n))
                {
                    if (_resource.Name.Substring(_resource.Name.LastIndexOf(".") + 1,
                            _resource.Name.Length - (_resource.Name.LastIndexOf(".") + 1)).Length == 3)
                    {
                        ConvertToStock50();
                        return;
                    }
                    else if (_resource.Name.Substring(_resource.Name.LastIndexOf(".") + 1,
                                 _resource.Name.Length - (_resource.Name.LastIndexOf(".") + 1)).Length == 4)
                    {
                        ConvertToStockDefault();
                        return;
                    }
                }
            }
        }

        public void ConvertToStock50()
        {
            string matchName = _resource.Name.Substring(0, _resource.Name.LastIndexOf(".")) + ".";
            string matchNameX = _resource.Name.Substring(0, _resource.Name.LastIndexOf(".")) + "X.";
            List<TEX0Node> texList = new List<TEX0Node>();
            for (int i = _resource.Parent.Children.Count - 1; i >= 0; i--)
            {
                if (!(_resource.Parent.Children[i] is TEX0Node))
                {
                    continue;
                }

                TEX0Node tx0 = (TEX0Node) _resource.Parent.Children[i];
                if (tx0.Name.StartsWith(matchName) && tx0.Name.LastIndexOf(".") > 0 &&
                    tx0.Name.LastIndexOf(".") < tx0.Name.Length &&
                    int.TryParse(
                        tx0.Name.Substring(tx0.Name.LastIndexOf(".") + 1,
                            tx0.Name.Length - (tx0.Name.LastIndexOf(".") + 1)), out int x) && x >= 0)
                {
                    if (x <= 0) // 0 edge case
                    {
                        tx0.texSortNum = 0;
                    }
                    else if (x == 475) // WarioMan edge case (should pre-program)
                    {
                        tx0.texSortNum = 9001 + x % 475;
                    }
                    else
                    {
                        tx0.texSortNum = (int) Math.Floor(((double) x - 1) / 10.0) * 50 + x % 10;

                        if (x % 10 == 0)
                        {
                            tx0.texSortNum += 10;
                        }

                        if (x >= 201 && x <= 205 || // Ganon Edge Case
                            x >= 351 && x <= 355 || // ROB Edge Case
                            x >= 381 && x <= 384 || // Wario Edge Case
                            x >= 411 && x <= 415 || // Toon Link Edge Case
                            x >= 471 && x <= 474)   // Sonic Edge Case
                        {
                            tx0.texSortNum -= 40;
                        }
                    }

                    if (tx0.HasPalette)
                    {
                        tx0.GetPaletteNode().Name = "InfStc." + tx0.texSortNum.ToString("0000");
                    }

                    tx0.Name = "InfStc." + tx0.texSortNum.ToString("0000");
                    if (((BRRESNode) _resource.Parent.Parent).GetFolder<PLT0Node>()
                                                             .FindChildrenByName("InfStc." + x.ToString("000"))
                                                             .Count() > 0)
                    {
                        foreach (PLT0Node p in ((BRRESNode) _resource.Parent.Parent).GetFolder<PLT0Node>()
                                                                                    .FindChildrenByName(
                                                                                        "InfStc." + x.ToString("000")))
                        {
                            p.Remove();
                        }
                    }
                }
                else if (tx0.Name.StartsWith(matchNameX) && tx0.Name.LastIndexOf(".") > 0 &&
                         tx0.Name.LastIndexOf(".") < tx0.Name.Length &&
                         int.TryParse(
                             tx0.Name.Substring(tx0.Name.LastIndexOf(".") + 1,
                                 tx0.Name.Length - (tx0.Name.LastIndexOf(".") + 1)), out int x2) && x2 >= 0)
                {
                    if (tx0.HasPalette)
                    {
                        tx0.GetPaletteNode().Name = "InfStc." + x2.ToString("0000");
                    }

                    tx0.Name = "InfStc." + x2.ToString("0000");
                    if (((BRRESNode) _resource.Parent.Parent).GetFolder<PLT0Node>()
                                                             .FindChildrenByName("InfStcX." + x2.ToString("0000"))
                                                             .Count() > 0)
                    {
                        foreach (PLT0Node p in ((BRRESNode) _resource.Parent.Parent).GetFolder<PLT0Node>()
                                                                                    .FindChildrenByName(
                                                                                        "InfStcX." +
                                                                                        x2.ToString("0000")))
                        {
                            p.Remove();
                        }
                    }
                }
            }

            PAT0Node newPat0 = GeneratePAT0(true);
            if (((BRRESNode) _resource.Parent.Parent).GetFolder<CHR0Node>() != null)
            {
                ResourceNode[] temp = ((BRRESNode) _resource.Parent.Parent).GetFolder<CHR0Node>()
                                                                           .FindChildrenByName(newPat0.Name);
                if (temp.Length > 0)
                {
                    foreach (CHR0Node cn in temp)
                    {
                        cn.FrameCount = newPat0.FrameCount;
                    }
                }
            }

            if (((BRRESNode) _resource.Parent.Parent).GetFolder<CLR0Node>() != null)
            {
                ResourceNode[] temp = ((BRRESNode) _resource.Parent.Parent).GetFolder<CLR0Node>()
                                                                           .FindChildrenByName(newPat0.Name);
                if (temp.Length > 0)
                {
                    foreach (CLR0Node cn in temp)
                    {
                        cn.FrameCount = newPat0.FrameCount;
                    }
                }
            }

            if (MessageBox.Show(
                    "Would you like to convert the InfFace portraits to the new system as well at this time?",
                    "Convert InfFace?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string infFaceFolder = "";
                bool autoFoundFolder = false;
                if (Program.RootPath.EndsWith("\\info2\\info.pac", StringComparison.OrdinalIgnoreCase))
                {
                    string autoFolder =
                        Program.RootPath.Substring(0, Program.RootPath.LastIndexOf("\\info2\\info.pac")) +
                        "\\info\\portrite";
                    if (Directory.Exists(autoFolder))
                    {
                        if (MessageBox.Show(
                                "The folder for InfFace was autodetected to be: \n" + autoFolder +
                                "\n\nIs this correct?", "InfFace Converter", MessageBoxButtons.YesNo) ==
                            DialogResult.Yes)
                        {
                            infFaceFolder = autoFolder;
                            autoFoundFolder = true;
                        }
                    }
                }

                if (!autoFoundFolder)
                {
                    FolderBrowserDialog f = new FolderBrowserDialog
                    {
                        Description = "Select the \"portrite\" folder"
                    };
                    DialogResult dr = f.ShowDialog();
                    infFaceFolder = f.SelectedPath;
                    if (dr != DialogResult.OK || infFaceFolder == null || infFaceFolder == "")
                    {
                        return;
                    }
                }

                try
                {
                    DirectoryInfo d = Directory.CreateDirectory(infFaceFolder);
                    DirectoryInfo d2 = Directory.CreateDirectory(infFaceFolder + '\\' + "temp");
                    Console.WriteLine(infFaceFolder);
                    int count = 0;
                    foreach (FileInfo infFace in d.GetFiles())
                    {
                        Console.WriteLine(infFaceFolder + '\\' + infFace.Name);
                        int properlength = infFace.Name.EndsWith(".brres", StringComparison.OrdinalIgnoreCase)
                            ? infFace.Name.Length - ".brres".Length
                            : infFace.Name.Length;
                        Console.WriteLine(infFace.Name.Substring(7, properlength - 7));
                        if (infFace.Name.StartsWith("InfFaceX") &&
                            infFace.Name.EndsWith(".brres", StringComparison.CurrentCultureIgnoreCase) &&
                            int.TryParse(infFace.Name.Substring(8, properlength - 8), out int x2) && x2 >= 0)
                        {
                            infFace.MoveTo(infFaceFolder + '\\' + "temp" + '\\' + "InfFace" + x2.ToString("0000") +
                                           ".brres");
                            count++;
                        }
                        else if (!infFace.Name.StartsWith("InfFaceX") && infFace.Name.StartsWith("InfFace") &&
                                 infFace.Name.EndsWith(".brres", StringComparison.CurrentCultureIgnoreCase) &&
                                 int.TryParse(infFace.Name.Substring(7, properlength - 7), out int x) && x >= 0)
                        {
                            int n = x;
                            if (x <= 0) // 0 edge case
                            {
                                n = 0;
                            }
                            else if (x >= 661 && x <= 674) // WarioMan edge case (should pre-program)
                            {
                                n = 9001 + x % 661;
                            }
                            else
                            {
                                n = (int) Math.Floor(((double) x - 1) / 10.0) * 50 + x % 10;

                                if (x % 10 == 0)
                                {
                                    n += 10;
                                }

                                if (x >= 201 && x <= 205 || // Ganon Edge Case
                                    x >= 351 && x <= 355 || // ROB Edge Case
                                    x >= 381 && x <= 384 || // Wario Edge Case
                                    x >= 411 && x <= 415 || // Toon Link Edge Case
                                    x >= 471 && x <= 474)   // Sonic Edge Case
                                {
                                    n -= 40;
                                }
                            }

                            infFace.MoveTo(infFaceFolder + '\\' + "temp" + '\\' + "InfFace" + n.ToString("0000") +
                                           ".brres");
                            count++;
                        }
                    }

                    foreach (FileInfo infFace in d2.GetFiles())
                    {
                        infFace.MoveTo(infFaceFolder + '\\' + infFace.Name +
                                       (infFace.Name.EndsWith(".brres", StringComparison.OrdinalIgnoreCase)
                                           ? ""
                                           : ".brres"));
                    }

                    d2.Delete();
                    if (count > 0)
                    {
                        MessageBox.Show("InfFace conversion successful!");
                    }
                    else
                    {
                        MessageBox.Show("No convertable InfFace portraits found in " + infFaceFolder);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return;
                }
            }
        }

        public void ConvertToStockDefault()
        {
            string matchName = _resource.Name.Substring(0, _resource.Name.LastIndexOf(".")) + ".";
            string matchNameX = _resource.Name.Substring(0, _resource.Name.LastIndexOf(".")) + "X.";
            List<TEX0Node> texList = new List<TEX0Node>();
            for (int i = 0; i < _resource.Parent.Children.Count; i++)
            {
                if (!(_resource.Parent.Children[i] is TEX0Node))
                {
                    continue;
                }

                TEX0Node tx0 = (TEX0Node) _resource.Parent.Children[i];
                if (tx0.Name.StartsWith(matchName) && tx0.Name.LastIndexOf(".") > 0 &&
                    tx0.Name.LastIndexOf(".") < tx0.Name.Length &&
                    int.TryParse(
                        tx0.Name.Substring(tx0.Name.LastIndexOf(".") + 1,
                            tx0.Name.Length - (tx0.Name.LastIndexOf(".") + 1)), out int x) && x >= 0)
                {
                    tx0.texSortNum = x;
                    if (x <= 0) // 0 edge case
                    {
                        tx0.texSortNum = 0;
                        if (tx0.HasPalette)
                        {
                            tx0.GetPaletteNode().Name = "InfStc." + tx0.texSortNum.ToString("000");
                        }

                        tx0.Name = "InfStc." + tx0.texSortNum.ToString("000");
                        if (((BRRESNode) _resource.Parent.Parent).GetFolder<PLT0Node>()
                                                                 .FindChildrenByName("InfStc." + x.ToString("0000"))
                                                                 .Count() > 0)
                        {
                            foreach (PLT0Node p in ((BRRESNode) _resource.Parent.Parent).GetFolder<PLT0Node>()
                                                                                        .FindChildrenByName(
                                                                                            "InfStc." + x.ToString(
                                                                                                "0000")))
                            {
                                p.Remove();
                            }
                        }
                    }
                    else if (x == 9001) // WarioMan edge case (should pre-program)
                    {
                        tx0.texSortNum = 475 + x % 9001;
                        if (tx0.HasPalette)
                        {
                            tx0.GetPaletteNode().Name = "InfStc." + tx0.texSortNum.ToString("000");
                        }

                        tx0.Name = "InfStc." + tx0.texSortNum.ToString("000");
                        if (((BRRESNode) _resource.Parent.Parent).GetFolder<PLT0Node>()
                                                                 .FindChildrenByName("InfStc." + x.ToString("0000"))
                                                                 .Count() > 0)
                        {
                            foreach (PLT0Node p in ((BRRESNode) _resource.Parent.Parent).GetFolder<PLT0Node>()
                                                                                        .FindChildrenByName(
                                                                                            "InfStc." + x.ToString(
                                                                                                "0000")))
                            {
                                p.Remove();
                            }
                        }
                    }
                    else if (x % 50 <= 10 && x % 50 != 0 ||
                             x >= 0961 && x <= 0965 || // Ganon Edge Case
                             x >= 1711 && x <= 1715 || // ROB Edge Case
                             x >= 1861 && x <= 1864 || // Wario Edge Case
                             x >= 2011 && x <= 2015 || // Toon Link Edge Case
                             x >= 2311 && x <= 2314)   // Sonic Edge Case
                    {
                        tx0.texSortNum = (int) Math.Floor(((double) x + 1) / 50.0) * 10 + x % 10;

                        if (x % 10 == 0 ||
                            x >= 0961 && x <= 0965 || // Ganon Edge Case
                            x >= 1711 && x <= 1715 || // ROB Edge Case
                            x >= 1861 && x <= 1864 || // Wario Edge Case
                            x >= 2011 && x <= 2015 || // Toon Link Edge Case
                            x >= 2311 && x <= 2314)   // Sonic Edge Case
                        {
                            tx0.texSortNum += 10;
                        }

                        if (tx0.HasPalette)
                        {
                            tx0.GetPaletteNode().Name = "InfStc." + tx0.texSortNum.ToString("000");
                        }

                        tx0.Name = "InfStc." + tx0.texSortNum.ToString("000");
                        if (((BRRESNode) _resource.Parent.Parent).GetFolder<PLT0Node>()
                                                                 .FindChildrenByName("InfStc." + x.ToString("0000"))
                                                                 .Count() > 0)
                        {
                            foreach (PLT0Node p in ((BRRESNode) _resource.Parent.Parent).GetFolder<PLT0Node>()
                                                                                        .FindChildrenByName(
                                                                                            "InfStc." + x.ToString(
                                                                                                "0000")))
                            {
                                p.Remove();
                            }
                        }
                    }
                    else
                    {
                        if (tx0.HasPalette)
                        {
                            tx0.GetPaletteNode().Name = "InfStcX." + tx0.texSortNum.ToString("0000");
                        }

                        tx0.Name = "InfStcX." + tx0.texSortNum.ToString("0000");
                        if (((BRRESNode) _resource.Parent.Parent).GetFolder<PLT0Node>()
                                                                 .FindChildrenByName("InfStc." + x.ToString("0000"))
                                                                 .Count() > 0)
                        {
                            foreach (PLT0Node p in ((BRRESNode) _resource.Parent.Parent).GetFolder<PLT0Node>()
                                                                                        .FindChildrenByName(
                                                                                            "InfStc." + x.ToString(
                                                                                                "0000")))
                            {
                                p.Remove();
                            }
                        }
                    }
                }
            }

            PAT0Node newPat0 = GeneratePAT0(true);
            if (((BRRESNode) _resource.Parent.Parent).GetFolder<CHR0Node>() != null)
            {
                ResourceNode[] temp = ((BRRESNode) _resource.Parent.Parent).GetFolder<CHR0Node>()
                                                                           .FindChildrenByName(newPat0.Name);
                if (temp.Length > 0)
                {
                    foreach (CHR0Node cn in temp)
                    {
                        cn.FrameCount = newPat0.FrameCount;
                    }
                }
            }

            if (((BRRESNode) _resource.Parent.Parent).GetFolder<CLR0Node>() != null)
            {
                ResourceNode[] temp = ((BRRESNode) _resource.Parent.Parent).GetFolder<CLR0Node>()
                                                                           .FindChildrenByName(newPat0.Name);
                if (temp.Length > 0)
                {
                    foreach (CLR0Node cn in temp)
                    {
                        cn.FrameCount = newPat0.FrameCount;
                    }
                }
            }

            if (MessageBox.Show(
                    "Would you like to convert the InfFace portraits to the new system as well at this time?",
                    "Convert InfFace?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string infFaceFolder = "";
                bool autoFoundFolder = false;
                if (Program.RootPath.EndsWith("\\info2\\info.pac", StringComparison.OrdinalIgnoreCase))
                {
                    string autoFolder =
                        Program.RootPath.Substring(0, Program.RootPath.LastIndexOf("\\info2\\info.pac")) +
                        "\\info\\portrite";
                    if (Directory.Exists(autoFolder))
                    {
                        if (MessageBox.Show(
                                "The folder for InfFace was autodetected to be: \n" + autoFolder +
                                "\n\nIs this correct?", "InfFace Converter", MessageBoxButtons.YesNo) ==
                            DialogResult.Yes)
                        {
                            infFaceFolder = autoFolder;
                            autoFoundFolder = true;
                        }
                    }
                }

                if (!autoFoundFolder)
                {
                    FolderBrowserDialog f = new FolderBrowserDialog
                    {
                        Description = "Select the \"portrite\" folder"
                    };
                    DialogResult dr = f.ShowDialog();
                    infFaceFolder = f.SelectedPath;
                    if (dr != DialogResult.OK || infFaceFolder == null || infFaceFolder == "")
                    {
                        return;
                    }
                }

                try
                {
                    DirectoryInfo d = Directory.CreateDirectory(infFaceFolder);
                    DirectoryInfo d2 = Directory.CreateDirectory(infFaceFolder + '\\' + "temp");
                    Console.WriteLine(infFaceFolder);
                    int count = 0;
                    foreach (FileInfo infFace in d.GetFiles().Reverse())
                    {
                        Console.WriteLine(infFaceFolder + '\\' + infFace.Name);
                        int properlength = infFace.Name.EndsWith(".brres", StringComparison.OrdinalIgnoreCase)
                            ? infFace.Name.Length - ".brres".Length
                            : infFace.Name.Length;

                        if (infFace.Name.StartsWith("InfFace") && !infFace.Name.StartsWith("InfFaceX") &&
                            infFace.Name.EndsWith(".brres", StringComparison.CurrentCultureIgnoreCase) &&
                            int.TryParse(infFace.Name.Substring(7, properlength - 7), out int x) && x >= 0)
                        {
                            int n = x;
                            if (x <= 0) // 0 edge case
                            {
                                n = 0;
                                infFace.MoveTo(infFaceFolder + '\\' + "temp" + '\\' + "InfFace" + n.ToString("000") +
                                               ".brres");
                            }
                            else if (x >= 9001 && x <= 9014) // WarioMan edge case (should pre-program)
                            {
                                n = 661 + x % 9001;
                                infFace.MoveTo(infFaceFolder + '\\' + "temp" + '\\' + "InfFace" + n.ToString("000") +
                                               ".brres");
                            }
                            else if (x % 50 <= 10 && x % 50 != 0 ||
                                     x >= 0961 && x <= 0965 || // Ganon Edge Case
                                     x >= 1711 && x <= 1715 || // ROB Edge Case
                                     x >= 1861 && x <= 1864 || // Wario Edge Case
                                     x >= 2011 && x <= 2015 || // Toon Link Edge Case
                                     x >= 2311 && x <= 2314)   // Sonic Edge Case
                            {
                                n = (int) Math.Floor(((double) x + 1) / 50.0) * 10 + x % 10;

                                if (x % 10 == 0 ||
                                    x >= 0961 && x <= 0965 || // Ganon Edge Case
                                    x >= 1711 && x <= 1715 || // ROB Edge Case
                                    x >= 1861 && x <= 1864 || // Wario Edge Case
                                    x >= 2011 && x <= 2015 || // Toon Link Edge Case
                                    x >= 2311 && x <= 2314)   // Sonic Edge Case
                                {
                                    n += 10;
                                }

                                infFace.MoveTo(infFaceFolder + '\\' + "temp" + '\\' + "InfFace" + n.ToString("000") +
                                               ".brres");
                                count++;
                            }
                            else
                            {
                                infFace.MoveTo(infFaceFolder + '\\' + "temp" + '\\' + "InfFaceX" + n.ToString("0000") +
                                               ".brres");
                                count++;
                            }
                        }
                    }

                    foreach (FileInfo infFace in d2.GetFiles())
                    {
                        infFace.MoveTo(infFaceFolder + '\\' + infFace.Name +
                                       (infFace.Name.EndsWith(".brres", StringComparison.OrdinalIgnoreCase)
                                           ? ""
                                           : ".brres"));
                    }

                    d2.Delete();
                    if (count > 0)
                    {
                        MessageBox.Show("InfFace conversion successful!");
                    }
                    else
                    {
                        MessageBox.Show("No convertable InfFace portraits found in " + infFaceFolder);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
    }
}