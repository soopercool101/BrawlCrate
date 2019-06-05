using System;
using System.ComponentModel;
using System.Windows.Forms;
using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.U8)]
    public class U8Wrapper : GenericWrapper
    {
        public U8Wrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => "U8 Archive (*.arc)|*.arc|" +
                                               "Compressed U8 Archive (*.szs)|*.szs|" +
                                               "Archive Pair (*.pair)|*.pair";

        public U8FolderNode NewFolder()
        {
            var node = new U8FolderNode {Name = _resource.FindName("NewFolder")};
            _resource.AddChild(node);

            var w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public ARCNode NewARC()
        {
            var node = new ARCNode {Name = _resource.FindName("NewARChive"), FileType = ARCFileType.MiscData};
            _resource.AddChild(node);

            var w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public BRRESNode NewBRES()
        {
            var node = new BRRESNode {FileType = ARCFileType.MiscData};
            _resource.AddChild(node);

            var w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public MSBinNode NewMSBin()
        {
            var node = new MSBinNode {FileType = ARCFileType.MiscData};
            _resource.AddChild(node);

            var w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public void ImportARC()
        {
            if (Program.OpenFile("ARChive (*.pac,*.pcs)|*.pac;*.pcs", out var path) > 0) NewARC().Replace(path);
        }

        public void ImportBRES()
        {
            if (Program.OpenFile(FileFilters.BRES, out var path) > 0) NewBRES().Replace(path);
        }

        public void ImportMSBin()
        {
            if (Program.OpenFile(FileFilters.MSBin, out var path) > 0) NewMSBin().Replace(path);
        }

        public override void OnExport(string outPath, int filterIndex)
        {
            switch (filterIndex)
            {
                case 1:
                    ((U8Node) _resource).Export(outPath);
                    break;
                case 2:
                    ((U8Node) _resource).ExportSZS(outPath);
                    break;
                case 3:
                    ((U8Node) _resource).ExportPair(outPath);
                    break;
            }
        }

        public void PreviewAll()
        {
            new ModelForm().Show(_owner, ModelPanel.CollectModels(_resource));
        }

        public void ExportAll()
        {
            var path = Program.ChooseFolder();
            if (path == null) return;

            var hasModels = false;
            var hasTextures = false;
            foreach (var r in _resource.Children)
            {
                if (hasModels && hasTextures) break;

                if (r is BRRESNode)
                {
                    foreach (BRESGroupNode e in r.Children)
                    {
                        if (e.Type == BRESGroupNode.BRESGroupType.Textures)
                            hasTextures = true;
                        else if (e.Type == BRESGroupNode.BRESGroupType.Models) hasModels = true;

                        if (hasModels && hasTextures) break;
                    }
                }
                else if (r is U8FolderNode)
                {
                    searchU8Folder((U8FolderNode) r, out var hasModelsTemp, out var hasTexturesTemp);
                    hasModels = hasModels || hasModelsTemp;
                    hasTextures = hasTextures || hasTexturesTemp;
                }
            }

            var extensionTEX0 = ".tex0";
            var extensionMDL0 = ".mdl0";

            if (hasTextures)
            {
                var dialog = new ExportAllFormatDialog();

                if (dialog.ShowDialog() == DialogResult.OK)
                    extensionTEX0 = dialog.SelectedExtension;
                else
                    return;
            }

            if (hasModels)
            {
                var dialog = new ExportAllFormatDialog(true);

                if (dialog.ShowDialog() == DialogResult.OK)
                    extensionMDL0 = dialog.SelectedExtension;
                else
                    return;
            }

            ((U8Node) _resource).ExtractToFolder(path, extensionTEX0, extensionMDL0);
        }

        public void searchU8Folder(U8FolderNode u8, out bool hasModels, out bool hasTextures)
        {
            hasModels = false;
            hasTextures = false;
            foreach (var r in u8.Children)
            {
                if (hasModels && hasTextures) break;

                if (r is BRRESNode)
                {
                    foreach (BRESGroupNode e in r.Children)
                    {
                        if (e.Type == BRESGroupNode.BRESGroupType.Textures)
                            hasTextures = true;
                        else if (e.Type == BRESGroupNode.BRESGroupType.Models) hasModels = true;

                        if (hasModels && hasTextures) break;
                    }
                }
                else if (r is U8FolderNode)
                {
                    searchU8Folder((U8FolderNode) r, out var hasModelsTemp, out var hasTexturesTemp);
                    hasModels = hasModels || hasModelsTemp;
                    hasTextures = hasTextures || hasTexturesTemp;
                }
            }
        }

        public void ReplaceAll()
        {
            var path = Program.ChooseFolder();
            if (path == null) return;
            ((ARCNode) _resource).ReplaceFromFolder(path);
        }

        #region Menu

        private static readonly ContextMenuStrip _menu;

        static U8Wrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Ne&w", null,
                new ToolStripMenuItem("Folder", null, NewFolderAction),
                new ToolStripMenuItem("ARChive", null, NewARCAction),
                new ToolStripMenuItem("BRResource Pack", null, NewBRESAction),
                new ToolStripMenuItem("MSBin", null, NewMSBinAction)
            ));
            _menu.Items.Add(new ToolStripMenuItem("&Import", null,
                new ToolStripMenuItem("ARChive", null, ImportARCAction),
                new ToolStripMenuItem("BRResource Pack", null, ImportBRESAction),
                new ToolStripMenuItem("MSBin", null, ImportMSBinAction)
            ));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Preview All Models", null, PreviewAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Export All", null, ExportAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Replace All", null, ReplaceAllAction));
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

        protected static void NewBRESAction(object sender, EventArgs e)
        {
            GetInstance<U8Wrapper>().NewBRES();
        }

        protected static void NewARCAction(object sender, EventArgs e)
        {
            GetInstance<U8Wrapper>().NewARC();
        }

        protected static void NewFolderAction(object sender, EventArgs e)
        {
            GetInstance<U8Wrapper>().NewFolder();
        }

        protected static void NewMSBinAction(object sender, EventArgs e)
        {
            GetInstance<U8Wrapper>().NewMSBin();
        }

        protected static void ImportBRESAction(object sender, EventArgs e)
        {
            GetInstance<U8Wrapper>().ImportBRES();
        }

        protected static void ImportARCAction(object sender, EventArgs e)
        {
            GetInstance<U8Wrapper>().ImportARC();
        }

        protected static void ImportMSBinAction(object sender, EventArgs e)
        {
            GetInstance<U8Wrapper>().ImportMSBin();
        }

        protected static void PreviewAllAction(object sender, EventArgs e)
        {
            GetInstance<U8Wrapper>().PreviewAll();
        }

        protected static void ExportAllAction(object sender, EventArgs e)
        {
            GetInstance<U8Wrapper>().ExportAll();
        }

        protected static void ReplaceAllAction(object sender, EventArgs e)
        {
            GetInstance<U8Wrapper>().ReplaceAll();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[8].Enabled = _menu.Items[9].Enabled =
                _menu.Items[11].Enabled = _menu.Items[12].Enabled = _menu.Items[15].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<U8Wrapper>();

            _menu.Items[8].Enabled = _menu.Items[15].Enabled = w.Parent != null;
            _menu.Items[9].Enabled = w._resource.IsDirty || w._resource.IsBranch;
            _menu.Items[11].Enabled = w.PrevNode != null;
            _menu.Items[12].Enabled = w.NextNode != null;
        }

        #endregion
    }
}