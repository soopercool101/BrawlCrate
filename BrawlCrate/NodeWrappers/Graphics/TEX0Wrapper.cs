using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        static TEX0Wrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Re-Encode", null, ReEncodeAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(GeneratePAT0ToolStripMenuItem);
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
            MultiSelectMenu.Opening += MultiMenuOpening;
            MultiSelectMenu.Closing += MultiMenuClosing;
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            GeneratePAT0ToolStripMenuItem.Enabled = true;
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
        }

        private static void MultiMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            ColorSmashSelectedToolStripMenuItem.Enabled = true;
        }

        private static void MultiMenuOpening(object sender, CancelEventArgs e)
        {
            TEX0Wrapper w = GetInstance<TEX0Wrapper>();
            if (!ColorSmash.CanRunColorSmash)
            {
                ColorSmashSelectedToolStripMenuItem.Enabled = false;
            }
            else
            {
                foreach (TreeNode n in MainForm.Instance.resourceTree.SelectedNodes)
                {
                    if (!(n is TEX0Wrapper t) || t._resource.Parent == null || t._resource.Parent != w._resource.Parent)
                    {
                        ColorSmashSelectedToolStripMenuItem.Enabled = false;
                        break;
                    }
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
            if (((TEX0Node)_resource).HasPalette)
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
                                       .FindChildrenByName(newPat0.Name).Length > 0)
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
                MessageBox.Show("Would you like to delete the associated PLT0?", "Deleting TEX0",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                p.Dispose();
                p.Remove();
            }
            
            base.Delete();
        }
    }
}