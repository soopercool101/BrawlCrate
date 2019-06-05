using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.BRES)]
    public class BRESWrapper : GenericWrapper
    {
        public BRESWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.BRES;

        public void ImportGIF()
        {
            var index = Program.OpenFile("Animated GIF (*.gif)|*.gif", out var path);
            if (index > 0) ((BRRESNode) _resource).ImportGIF(path);
        }

        public void ImportTexture()
        {
            var index = Program.OpenFile(FileFilters.TEX0, out var path);
            if (index == 8)
            {
                var node = NodeFactory.FromFile(null, path) as TEX0Node;
                ((BRRESNode) _resource).GetOrCreateFolder<TEX0Node>().AddChild(node);

                var palette = Path.ChangeExtension(path, ".plt0");
                if (File.Exists(palette) && node.HasPalette)
                {
                    var n = NodeFactory.FromFile(null, palette) as PLT0Node;
                    ((BRRESNode) _resource).GetOrCreateFolder<PLT0Node>().AddChild(n);
                }

                var w = FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
            else if (index > 0)
            {
                using (var dlg = new TextureConverterDialog())
                {
                    dlg.ImageSource = path;
                    if (dlg.ShowDialog(MainForm.Instance, Resource as BRRESNode) == DialogResult.OK)
                    {
                        var w = FindResource(dlg.TEX0TextureNode, true);
                        w.EnsureVisible();
                        w.TreeView.SelectedNode = w;
                    }
                }
            }
        }

        public void ImportModel()
        {
            if (Program.OpenFile(FileFilters.MDL0Import, out var path) > 0)
            {
                var node = MDL0Node.FromFile(path);
                if (node != null)
                {
                    ((BRRESNode) _resource).GetOrCreateFolder<MDL0Node>().AddChild(node);

                    var w = FindResource(node, true);
                    w.EnsureVisible();
                    w.TreeView.SelectedNode = w;
                }
            }
        }

        public void ImportChr()
        {
            if (Program.OpenFile(FileFilters.CHR0Import, out var path) > 0)
            {
                var node = CHR0Node.FromFile(path);
                ((BRRESNode) _resource).GetOrCreateFolder<CHR0Node>().AddChild(node);

                var w = FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportVis()
        {
            if (Program.OpenFile(FileFilters.VIS0, out var path) > 0)
            {
                var node = NodeFactory.FromFile(null, path) as VIS0Node;
                ((BRRESNode) _resource).GetOrCreateFolder<VIS0Node>().AddChild(node);

                var w = FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportShp()
        {
            if (Program.OpenFile(FileFilters.SHP0, out var path) > 0)
            {
                var node = NodeFactory.FromFile(null, path) as SHP0Node;
                ((BRRESNode) _resource).GetOrCreateFolder<SHP0Node>().AddChild(node);

                var w = FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportSrt()
        {
            if (Program.OpenFile(FileFilters.SRT0, out var path) > 0)
            {
                var node = NodeFactory.FromFile(null, path) as SRT0Node;
                ((BRRESNode) _resource).GetOrCreateFolder<SRT0Node>().AddChild(node);

                var w = FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportPat()
        {
            if (Program.OpenFile(FileFilters.PAT0, out var path) > 0)
            {
                var node = NodeFactory.FromFile(null, path) as PAT0Node;
                ((BRRESNode) _resource).GetOrCreateFolder<PAT0Node>().AddChild(node);

                var w = FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportScn()
        {
            if (Program.OpenFile(FileFilters.SCN0, out var path) > 0)
            {
                var node = NodeFactory.FromFile(null, path) as SCN0Node;
                ((BRRESNode) _resource).GetOrCreateFolder<SCN0Node>().AddChild(node);

                var w = FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportClr()
        {
            if (Program.OpenFile(FileFilters.CLR0, out var path) > 0)
            {
                var node = NodeFactory.FromFile(null, path) as CLR0Node;
                ((BRRESNode) _resource).GetOrCreateFolder<CLR0Node>().AddChild(node);

                var w = FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void NewChr()
        {
            var node = ((BRRESNode) _resource).CreateResource<CHR0Node>("NewCHR");
            node.Version = 4;
            var res = FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewSrt()
        {
            var node = ((BRRESNode) _resource).CreateResource<SRT0Node>("NewSRT");
            node.Version = 4;
            var res = FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewPat()
        {
            var node = ((BRRESNode) _resource).CreateResource<PAT0Node>("NewPAT");
            node.Version = 3;
            var res = FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewShp()
        {
            var node = ((BRRESNode) _resource).CreateResource<SHP0Node>("NewSHP");
            node.Version = 3;
            var res = FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewVis()
        {
            var node = ((BRRESNode) _resource).CreateResource<VIS0Node>("NewVIS");
            node.Version = 3;
            var res = FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewScn()
        {
            var node = ((BRRESNode) _resource).CreateResource<SCN0Node>("NewSCN");
            var res = FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewClr()
        {
            var node = ((BRRESNode) _resource).CreateResource<CLR0Node>("NewCLR");
            node.Version = 3;
            var res = FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewModel()
        {
            var node = ((BRRESNode) _resource).CreateResource<MDL0Node>("NewModel");
            var res = FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void ExportAll()
        {
            var path = Program.ChooseFolder();
            if (path == null) return;

            var hasTextures = false;
            foreach (BRESGroupNode e in _resource.Children)
                if (e.Type == BRESGroupNode.BRESGroupType.Textures)
                {
                    hasTextures = true;
                    break;
                }

            if (hasTextures)
            {
                var dialog = new ExportAllFormatDialog();

                if (dialog.ShowDialog() == DialogResult.OK)
                    ((BRRESNode) _resource).ExportToFolder(path, dialog.SelectedExtension);
            }
            else
            {
                ((BRRESNode) _resource).ExportToFolder(path);
            }
        }

        public void EditAll()
        {
            var ctd = new EditAllDialog();
            ctd.ShowDialog(_owner, _resource);
        }

        public void ReplaceAll()
        {
            var path = Program.ChooseFolder();
            if (path == null) return;

            var dialog = new ExportAllFormatDialog();
            dialog.label1.Text = "Input format for textures:";

            if (dialog.ShowDialog() == DialogResult.OK)
                ((BRRESNode) _resource).ReplaceFromFolder(path, dialog.SelectedExtension);
        }

        public void ImportFolder()
        {
            var path = Program.ChooseFolder();
            if (path == null) return;
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
                    foreach (var n in node.Children) LoadModels(n, models);

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

        #region Menu

        private static readonly ContextMenuStrip _menu;

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
                new ToolStripMenuItem("Texture", null, ImportTextureAction),
                new ToolStripMenuItem("Model", null, ImportModelAction),
                new ToolStripMenuItem("Model Animation", null, ImportChrAction),
                new ToolStripMenuItem("Texture Animation", null, ImportSrtAction),
                new ToolStripMenuItem("Texture Pattern", null, ImportPatAction),
                new ToolStripMenuItem("Visibility Sequence", null, ImportVisAction),
                new ToolStripMenuItem("Vertex Morph", null, ImportShpAction),
                new ToolStripMenuItem("Color Sequence", null, ImportClrAction),
                new ToolStripMenuItem("Scene Settings", null, ImportScnAction),
                new ToolStripMenuItem("Folder", null, ImportFolderAction),
                new ToolStripMenuItem("Animated GIF", null, ImportGIFAction)
            ));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Preview All Models", null, PreviewAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Export All", null, ExportAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Replace All", null, ReplaceAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Edit All", null, EditAllAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void ImportTextureAction(object sender, EventArgs e)
        {
            GetInstance<BRESWrapper>().ImportTexture();
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

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[9].Enabled = _menu.Items[10].Enabled =
                _menu.Items[12].Enabled = _menu.Items[13].Enabled = _menu.Items[16].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<BRESWrapper>();

            _menu.Items[9].Enabled = _menu.Items[16].Enabled = w.Parent != null;
            _menu.Items[10].Enabled = w._resource.IsDirty || w._resource.IsBranch;
            _menu.Items[12].Enabled = w.PrevNode != null;
            _menu.Items[13].Enabled = w.NextNode != null;
        }

        #endregion
    }
}