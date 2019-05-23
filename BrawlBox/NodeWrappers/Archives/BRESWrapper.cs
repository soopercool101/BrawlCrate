using System;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib;
using System.Collections.Generic;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.BRES)]
    public class BRESWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
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
        protected static void ImportTextureAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportTexture(); }
        protected static void ImportModelAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportModel(); }
        protected static void ImportChrAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportChr(); }
        protected static void ImportSrtAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportSrt(); }
        protected static void ImportPatAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportPat(); }
        protected static void ImportVisAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportVis(); }
        protected static void ImportShpAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportShp(); }
        protected static void ImportScnAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportScn(); }
        protected static void ImportClrAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportClr(); }
        
        protected static void NewModelAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewModel(); }
        protected static void NewChrAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewChr(); }
        protected static void NewSrtAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewSrt(); }
        protected static void NewPatAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewPat(); }
        protected static void NewVisAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewVis(); }
        protected static void NewShpAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewShp(); }
        protected static void NewScnAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewScn(); }
        protected static void NewClrAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewClr(); }
        
        protected static void ExportAllAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ExportAll(); }
        protected static void ImportFolderAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportFolder(); }
        protected static void ReplaceAllAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ReplaceAll(); }
        protected static void EditAllAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().EditAll(); }
        protected static void PreviewAllAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().PreviewAll(); }
        protected static void ImportGIFAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportGIF(); }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[9].Enabled = _menu.Items[10].Enabled = _menu.Items[12].Enabled = _menu.Items[13].Enabled = _menu.Items[16].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            BRESWrapper w = GetInstance<BRESWrapper>();

            _menu.Items[9].Enabled = _menu.Items[16].Enabled = w.Parent != null;
            _menu.Items[10].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[12].Enabled = w.PrevNode != null;
            _menu.Items[13].Enabled = w.NextNode != null;
        }

        #endregion

        public override string ExportFilter { get { return FileFilters.BRES; } }

        public BRESWrapper() { ContextMenuStrip = _menu; }

        public void ImportGIF()
        {
            string path;
            int index = Program.OpenFile("Animated GIF (*.gif)|*.gif", out path);
            if (index > 0)
                ((BRRESNode)_resource).ImportGIF(path);
        }

        public void ImportTexture()
        {
            string path;
            int index = Program.OpenFile(FileFilters.TEX0, out path);
            if (index == 8)
            {
                TEX0Node node = NodeFactory.FromFile(null, path) as TEX0Node;
                ((BRRESNode)_resource).GetOrCreateFolder<TEX0Node>().AddChild(node);

                string palette = Path.ChangeExtension(path, ".plt0");
                if (File.Exists(palette) && node.HasPalette)
                {
                    PLT0Node n = NodeFactory.FromFile(null, palette) as PLT0Node;
                    ((BRRESNode)_resource).GetOrCreateFolder<PLT0Node>().AddChild(n);
                }

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
            else if (index > 0)
                using (TextureConverterDialog dlg = new TextureConverterDialog())
                {
                    dlg.ImageSource = path;
                    if (dlg.ShowDialog(MainForm.Instance, Resource as BRRESNode) == DialogResult.OK)
                    {
                        BaseWrapper w = this.FindResource(dlg.TEX0TextureNode, true);
                        w.EnsureVisible();
                        w.TreeView.SelectedNode = w;
                    }
                }
        }

        public void ImportModel()
        {
            string path;
            if (Program.OpenFile(FileFilters.MDL0Import, out path) > 0)
            {
                MDL0Node node = MDL0Node.FromFile(path);
                if (node != null)
                {
                    ((BRRESNode)_resource).GetOrCreateFolder<MDL0Node>().AddChild(node);

                    BaseWrapper w = this.FindResource(node, true);
                    w.EnsureVisible();
                    w.TreeView.SelectedNode = w;
                }
            }
        }

        public void ImportChr()
        {
            string path;
            if (Program.OpenFile(FileFilters.CHR0Import, out path) > 0)
            {
                CHR0Node node = CHR0Node.FromFile(path);
                ((BRRESNode)_resource).GetOrCreateFolder<CHR0Node>().AddChild(node);

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportVis()
        {
            string path;
            if (Program.OpenFile(FileFilters.VIS0, out path) > 0)
            {
                VIS0Node node = NodeFactory.FromFile(null, path) as VIS0Node;
                ((BRRESNode)_resource).GetOrCreateFolder<VIS0Node>().AddChild(node);

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportShp()
        {
            string path;
            if (Program.OpenFile(FileFilters.SHP0, out path) > 0)
            {
                SHP0Node node = NodeFactory.FromFile(null, path) as SHP0Node;
                ((BRRESNode)_resource).GetOrCreateFolder<SHP0Node>().AddChild(node);

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportSrt()
        {
            string path;
            if (Program.OpenFile(FileFilters.SRT0, out path) > 0)
            {
                SRT0Node node = NodeFactory.FromFile(null, path) as SRT0Node;
                ((BRRESNode)_resource).GetOrCreateFolder<SRT0Node>().AddChild(node);

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportPat()
        {
            string path;
            if (Program.OpenFile(FileFilters.PAT0, out path) > 0)
            {
                PAT0Node node = NodeFactory.FromFile(null, path) as PAT0Node;
                ((BRRESNode)_resource).GetOrCreateFolder<PAT0Node>().AddChild(node);

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportScn()
        {
            string path;
            if (Program.OpenFile(FileFilters.SCN0, out path) > 0)
            {
                SCN0Node node = NodeFactory.FromFile(null, path) as SCN0Node;
                ((BRRESNode)_resource).GetOrCreateFolder<SCN0Node>().AddChild(node);

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportClr()
        {
            string path;
            if (Program.OpenFile(FileFilters.CLR0, out path) > 0)
            {
                CLR0Node node = NodeFactory.FromFile(null, path) as CLR0Node;
                ((BRRESNode)_resource).GetOrCreateFolder<CLR0Node>().AddChild(node);

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void NewChr()
        {
            CHR0Node node = ((BRRESNode)_resource).CreateResource<CHR0Node>("NewCHR");
            node.Version = 4;
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewSrt()
        {
            SRT0Node node = ((BRRESNode)_resource).CreateResource<SRT0Node>("NewSRT");
            node.Version = 4;
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewPat()
        {
            PAT0Node node = ((BRRESNode)_resource).CreateResource<PAT0Node>("NewPAT");
            node.Version = 3;
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewShp()
        {
            SHP0Node node = ((BRRESNode)_resource).CreateResource<SHP0Node>("NewSHP");
            node.Version = 3;
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewVis()
        {
            VIS0Node node = ((BRRESNode)_resource).CreateResource<VIS0Node>("NewVIS");
            node.Version = 3;
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewScn()
        {
            SCN0Node node = ((BRRESNode)_resource).CreateResource<SCN0Node>("NewSCN");
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewClr()
        {
            CLR0Node node = ((BRRESNode)_resource).CreateResource<CLR0Node>("NewCLR");
            node.Version = 3;
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewModel()
        {
            MDL0Node node = ((BRRESNode)_resource).CreateResource<MDL0Node>("NewModel");
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void ExportAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
                return;

            bool hasTextures = false;
            foreach (BRESGroupNode e in _resource.Children)
                if (e.Type == BRESGroupNode.BRESGroupType.Textures)
                {
                    hasTextures = true;
                    break;
                }

            if (hasTextures)
            {
                ExportAllFormatDialog dialog = new ExportAllFormatDialog();

                if (dialog.ShowDialog() == DialogResult.OK)
                    ((BRRESNode)_resource).ExportToFolder(path, dialog.SelectedExtension);
            }
            else
                ((BRRESNode)_resource).ExportToFolder(path);
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
                return;

            ExportAllFormatDialog dialog = new ExportAllFormatDialog();
            dialog.label1.Text = "Input format for textures:";

            if (dialog.ShowDialog() == DialogResult.OK)
                ((BRRESNode)_resource).ReplaceFromFolder(path, dialog.SelectedExtension);
        }

        public void ImportFolder()
        {
            string path = Program.ChooseFolder();
            if (path == null)
                return;

            ((BRRESNode)_resource).ImportFolder(path);
        }

        private void LoadModels(ResourceNode node, List<MDL0Node> models)
        {
            switch (node.ResourceType)
            {
                case ResourceType.ARC:
                case ResourceType.U8:
                case ResourceType.MRG:
                case ResourceType.BRES:
                case ResourceType.U8Folder:
                case ResourceType.BRESGroup:
                    foreach (ResourceNode n in node.Children)
                        LoadModels(n, models);
                    break;
                case ResourceType.MDL0:
                    models.Add((MDL0Node)node);
                    break;
            }
        }

        public void PreviewAll()
        {
            new ModelForm().Show(_owner, ModelPanel.CollectModels(_resource));
        }
    }
}
