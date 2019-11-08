using BrawlCrate.UI;
using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.BRES)]
    public class BRESWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

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

        static BRESWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Ne&w", null,
                new ToolStripMenuItem("Model", null, NewModelAction),
                new ToolStripMenuItem("Model Animation", null, NewChrAction),
                new ToolStripMenuItem("Texture Animation", null, NewSrtAction),
                new ToolStripMenuItem("Texture Pattern", null, NewPatAction),
                new ToolStripMenuItem("Visibility Sequence", null, NewVisAction),
                new ToolStripMenuItem("Vertex Morph", null, NewShpAction),
                new ToolStripMenuItem("Color Sequence", null, NewClrAction),
                new ToolStripMenuItem("Scene Settings", null, NewScnAction)
            ));
            _menu.Items.Add(new ToolStripMenuItem("&Import", null,
                new ToolStripMenuItem("Textures", null, ImportTextureAction),
                new ToolStripMenuItem("Color Smashable Textures", null, ImportColorSmashTextureAction),
                new ToolStripMenuItem("Models", null, ImportModelAction),
                new ToolStripMenuItem("Model Animations", null, ImportChrAction),
                new ToolStripMenuItem("Texture Animations", null, ImportSrtAction),
                new ToolStripMenuItem("Texture Patterns", null, ImportPatAction),
                new ToolStripMenuItem("Visibility Sequences", null, ImportVisAction),
                new ToolStripMenuItem("Vertex Morphs", null, ImportShpAction),
                new ToolStripMenuItem("Color Sequences", null, ImportClrAction),
                new ToolStripMenuItem("Scene Settings", null, ImportScnAction),
                new ToolStripMenuItem("Folder", null, ImportFolderAction),
                new ToolStripMenuItem("Animated GIFs", null, ImportGIFAction),
                new ToolStripMenuItem("Special", null,
                    new ToolStripMenuItem("Clear Mode (Spy) Textures", null, ImportCommonTextureSpyAction),
                    new ToolStripMenuItem("Metal Texture", null, ImportCommonTextureMetalAction),
                    new ToolStripMenuItem("Stage Shadow Texture", null, ImportCommonTextureShadowAction),
                    new ToolStripMenuItem("Static (Empty) Model", null, ImportCommonModelStaticAction)
                )
            ));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Preview All Models", null, PreviewAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Export All", null, ExportAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Replace All", null, ReplaceAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Edit All", null, EditAllAction));
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
        }

        protected static void ImportTextureAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ImportTexture();
        }

        protected static void ImportColorSmashTextureAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ImportColorSmashTexture();
        }

        protected static void ImportModelAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ImportModel();
        }

        protected static void ImportChrAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ImportChr();
        }

        protected static void ImportSrtAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ImportSrt();
        }

        protected static void ImportPatAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ImportPat();
        }

        protected static void ImportVisAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ImportVis();
        }

        protected static void ImportShpAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ImportShp();
        }

        protected static void ImportScnAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ImportScn();
        }

        protected static void ImportClrAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ImportClr();
        }

        protected static void NewModelAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().NewModel();
        }

        protected static void NewChrAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().NewChr();
        }

        protected static void NewSrtAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().NewSrt();
        }

        protected static void NewPatAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().NewPat();
        }

        protected static void NewVisAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().NewVis();
        }

        protected static void NewShpAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().NewShp();
        }

        protected static void NewScnAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().NewScn();
        }

        protected static void NewClrAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().NewClr();
        }

        protected static void ExportAllAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ExportAll();
        }

        protected static void ImportFolderAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ImportFolder();
        }

        protected static void ReplaceAllAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ReplaceAll();
        }

        protected static void EditAllAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().EditAll();
        }

        protected static void PreviewAllAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().PreviewAll();
        }

        protected static void ImportGIFAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ImportGIF();
        }

        protected static void ImportCommonModelStaticAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ImportCommonResources<MDL0Node>("BrawlLib.HardcodedFiles.Models.Static.mdl0");
        }

        protected static void ImportCommonTextureSpyAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ImportCommonResources<TEX0Node>("BrawlLib.HardcodedFiles.Textures.FB.tex0",
                "BrawlLib.HardcodedFiles.Textures.spycloak00.tex0");
        }

        protected static void ImportCommonTextureShadowAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>()
                .ImportCommonResources<TEX0Node>("BrawlLib.HardcodedFiles.Textures.TShadow1.tex0");
        }

        protected static void ImportCommonTextureMetalAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ImportCommonResources<TEX0Node>("BrawlLib.HardcodedFiles.Textures.metal00.tex0");
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            DuplicateToolStripMenuItem.Enabled = true;
            ReplaceToolStripMenuItem.Enabled = true;
            RestoreToolStripMenuItem.Enabled = true;
            MoveUpToolStripMenuItem.Enabled = true;
            MoveDownToolStripMenuItem.Enabled = true;
            DeleteToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            BRESWrapper w = GetInstance<BRESWrapper>();

            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
        }

        #endregion

        public override string ExportFilter => FileFilters.BRES;

        public BRESWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void ImportGIF()
        {
            if (Program.OpenFiles("Animated GIF (*.gif)|*.gif", out string[] paths) > 0)
            {
                foreach (string path in paths)
                {
                    ((BRRESNode) _resource).ImportGIF(path);
                }
            }
        }

        public void ImportColorSmashTexture()
        {
            ExternalInterfacing.ColorSmash.ColorSmashImport(_resource as BRRESNode);
        }

        public void ImportTexture()
        {
            if (Program.OpenFiles(FileFilters.TEX0, out string[] paths) > 0)
            {
                foreach (string path in paths)
                {
                    if (path.EndsWith(".tex0", StringComparison.OrdinalIgnoreCase))
                    {
                        TEX0Node node = NodeFactory.FromFile(null, path) as TEX0Node;
                        if (node == null)
                        {
                            continue;
                        }

                        ((BRRESNode) _resource).GetOrCreateFolder<TEX0Node>().AddChild(node);

                        string palette = Path.ChangeExtension(path, ".plt0");
                        if (File.Exists(palette) && node.HasPalette)
                        {
                            PLT0Node n = NodeFactory.FromFile(null, palette) as PLT0Node;
                            ((BRRESNode) _resource).GetOrCreateFolder<PLT0Node>().AddChild(n);
                        }

                        BaseWrapper w = FindResource(node, true);
                        w.EnsureVisible();
                        w.TreeView.SelectedNode = w;
                    }
                    else
                    {
                        using (TextureConverterDialog dlg = new TextureConverterDialog())
                        {
                            dlg.ImageSource = path;
                            if (dlg.ShowDialog(MainForm.Instance, Resource as BRRESNode) == DialogResult.OK)
                            {
                                BaseWrapper w = FindResource(dlg.TEX0TextureNode, true);
                                w.EnsureVisible();
                                w.TreeView.SelectedNode = w;
                            }
                        }
                    }
                }
            }
        }

        public void ImportModel()
        {
            if (Program.OpenFiles(FileFilters.MDL0Import, out string[] paths) > 0)
            {
                foreach (string path in paths)
                {
                    MDL0Node node = MDL0Node.FromFile(path);
                    if (node != null)
                    {
                        ((BRRESNode) _resource).GetOrCreateFolder<MDL0Node>().AddChild(node);

                        BaseWrapper w = FindResource(node, true);
                        w.EnsureVisible();
                        w.TreeView.SelectedNode = w;
                    }
                }
            }
        }

        public void ImportChr()
        {
            if (Program.OpenFiles(FileFilters.CHR0Import, out string[] paths) > 0)
            {
                foreach (string path in paths)
                {
                    CHR0Node node = CHR0Node.FromFile(path);
                    ((BRRESNode) _resource).GetOrCreateFolder<CHR0Node>().AddChild(node);

                    BaseWrapper w = FindResource(node, true);
                    w.EnsureVisible();
                    w.TreeView.SelectedNode = w;
                }
            }
        }

        public void ImportVis()
        {
            if (Program.OpenFiles(FileFilters.VIS0, out string[] paths) > 0)
            {
                foreach (string path in paths)
                {
                    VIS0Node node = NodeFactory.FromFile(null, path) as VIS0Node;
                    ((BRRESNode) _resource).GetOrCreateFolder<VIS0Node>().AddChild(node);

                    BaseWrapper w = FindResource(node, true);
                    w.EnsureVisible();
                    w.TreeView.SelectedNode = w;
                }
            }
        }

        public void ImportShp()
        {
            if (Program.OpenFiles(FileFilters.SHP0, out string[] paths) > 0)
            {
                foreach (string path in paths)
                {
                    SHP0Node node = NodeFactory.FromFile(null, path) as SHP0Node;
                    ((BRRESNode) _resource).GetOrCreateFolder<SHP0Node>().AddChild(node);

                    BaseWrapper w = FindResource(node, true);
                    w.EnsureVisible();
                    w.TreeView.SelectedNode = w;
                }
            }
        }

        public void ImportSrt()
        {
            if (Program.OpenFiles(FileFilters.SRT0, out string[] paths) > 0)
            {
                foreach (string path in paths)
                {
                    SRT0Node node = NodeFactory.FromFile(null, path) as SRT0Node;
                    ((BRRESNode) _resource).GetOrCreateFolder<SRT0Node>().AddChild(node);

                    BaseWrapper w = FindResource(node, true);
                    w.EnsureVisible();
                    w.TreeView.SelectedNode = w;
                }
            }
        }

        public void ImportPat()
        {
            if (Program.OpenFiles(FileFilters.PAT0, out string[] paths) > 0)
            {
                foreach (string path in paths)
                {
                    PAT0Node node = NodeFactory.FromFile(null, path) as PAT0Node;
                    ((BRRESNode) _resource).GetOrCreateFolder<PAT0Node>().AddChild(node);

                    BaseWrapper w = FindResource(node, true);
                    w.EnsureVisible();
                    w.TreeView.SelectedNode = w;
                }
            }
        }

        public void ImportScn()
        {
            if (Program.OpenFiles(FileFilters.SCN0, out string[] paths) > 0)
            {
                foreach (string path in paths)
                {
                    SCN0Node node = NodeFactory.FromFile(null, path) as SCN0Node;
                    ((BRRESNode) _resource).GetOrCreateFolder<SCN0Node>().AddChild(node);

                    BaseWrapper w = FindResource(node, true);
                    w.EnsureVisible();
                    w.TreeView.SelectedNode = w;
                }
            }
        }

        public void ImportClr()
        {
            if (Program.OpenFiles(FileFilters.CLR0, out string[] paths) > 0)
            {
                foreach (string path in paths)
                {
                    CLR0Node node = NodeFactory.FromFile(null, path) as CLR0Node;
                    ((BRRESNode) _resource).GetOrCreateFolder<CLR0Node>().AddChild(node);

                    BaseWrapper w = FindResource(node, true);
                    w.EnsureVisible();
                    w.TreeView.SelectedNode = w;
                }
            }
        }

        public void NewChr()
        {
            CHR0Node node = ((BRRESNode) _resource).CreateResource<CHR0Node>("NewCHR");
            node.Version = 4;
            BaseWrapper res = FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewSrt()
        {
            SRT0Node node = ((BRRESNode) _resource).CreateResource<SRT0Node>("NewSRT");
            node.Version = 4;
            BaseWrapper res = FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewPat()
        {
            PAT0Node node = ((BRRESNode) _resource).CreateResource<PAT0Node>("NewPAT");
            node.Version = 3;
            BaseWrapper res = FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewShp()
        {
            SHP0Node node = ((BRRESNode) _resource).CreateResource<SHP0Node>("NewSHP");
            node.Version = 3;
            BaseWrapper res = FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewVis()
        {
            VIS0Node node = ((BRRESNode) _resource).CreateResource<VIS0Node>("NewVIS");
            node.Version = 3;
            BaseWrapper res = FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewScn()
        {
            SCN0Node node = ((BRRESNode) _resource).CreateResource<SCN0Node>("NewSCN");
            BaseWrapper res = FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewClr()
        {
            CLR0Node node = ((BRRESNode) _resource).CreateResource<CLR0Node>("NewCLR");
            node.Version = 3;
            BaseWrapper res = FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewModel()
        {
            MDL0Node node = ((BRRESNode) _resource).CreateResource<MDL0Node>("NewModel");
            BaseWrapper res = FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void ExportAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
            {
                return;
            }

            bool hasTextures = false;
            foreach (BRESGroupNode e in _resource.Children)
            {
                if (e.Type == BRESGroupNode.BRESGroupType.Textures)
                {
                    hasTextures = true;
                    break;
                }
            }

            if (hasTextures)
            {
                ExportAllFormatDialog dialog = new ExportAllFormatDialog(typeof(TEX0Node), FileFilters.TEX0);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ((BRRESNode) _resource).ExportToFolder(path, dialog.SelectedExtension);
                }
            }
            else
            {
                ((BRRESNode) _resource).ExportToFolder(path);
            }
        }

        public void EditAll()
        {
            EditAllDialog ctd = new EditAllDialog();
            ctd.ShowDialog(_owner, _resource);
        }

        public void ReplaceAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
            {
                return;
            }

            ExportAllFormatDialog dialog = new ExportAllFormatDialog(typeof(TEX0Node), FileFilters.TEX0);
            dialog.label1.Text = "Input format for textures:";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ((BRRESNode) _resource).ReplaceFromFolder(path, dialog.SelectedExtension);
            }
        }

        public void ImportFolder()
        {
            string path = Program.ChooseFolder();
            if (path == null)
            {
                return;
            }

            ((BRRESNode) _resource).ImportFolder(path);
        }

        private void LoadModels(ResourceNode node, List<MDL0Node> models)
        {
            switch (node.ResourceFileType)
            {
                case ResourceType.ARC:
                case ResourceType.U8:
                case ResourceType.MRG:
                case ResourceType.BRES:
                case ResourceType.U8Folder:
                case ResourceType.BRESGroup:
                    foreach (ResourceNode n in node.Children)
                    {
                        LoadModels(n, models);
                    }

                    break;
                case ResourceType.MDL0:
                    models.Add((MDL0Node) node);
                    break;
            }
        }

        public void PreviewAll()
        {
            new ModelForm().Show(_owner, ModelPanel.CollectModels(_resource));
        }

        public void ImportCommonResources<T>(params string[] resourceNames) where T : BRESEntryNode
        {
            foreach (string resourceName in resourceNames)
            {
                Assembly assembly = Assembly.GetAssembly(typeof(ResourceNode));
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        return;
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        ResourceNode node = NodeFactory.FromSource(null, new DataSource(ms), typeof(T));
                        ((BRRESNode) _resource).GetOrCreateFolder<T>().AddChild(node);

                        BaseWrapper w = FindResource(node, true);
                        w.EnsureVisible();
                        w.TreeView.SelectedNode = w;
                    }
                }
            }
        }
    }
}