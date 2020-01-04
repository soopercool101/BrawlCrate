using BrawlCrate.UI;
using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.U8)]
    public class U8Wrapper : GenericWrapper
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
            DuplicateToolStripMenuItem.Enabled = true;
            ReplaceToolStripMenuItem.Enabled = true;
            RestoreToolStripMenuItem.Enabled = true;
            MoveUpToolStripMenuItem.Enabled = true;
            MoveDownToolStripMenuItem.Enabled = true;
            DeleteToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            U8Wrapper w = GetInstance<U8Wrapper>();

            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
        }

        #endregion

        public override string ExportFilter => FileFilters.U8Export;
        public override string ImportFilter => FileFilters.U8Import;

        public U8Wrapper()
        {
            ContextMenuStrip = _menu;
        }

        public U8FolderNode NewFolder()
        {
            U8FolderNode node = new U8FolderNode {Name = _resource.FindName("NewFolder")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public ARCNode NewARC()
        {
            ARCNode node = new ARCNode {Name = _resource.FindName("NewARChive"), FileType = ARCFileType.MiscData};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public BRRESNode NewBRES()
        {
            BRRESNode node = new BRRESNode {FileType = ARCFileType.MiscData};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public MSBinNode NewMSBin()
        {
            MSBinNode node = new MSBinNode {FileType = ARCFileType.MiscData};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public void ImportARC()
        {
            if (Program.OpenFile(FileFilters.ARCImport, out string path))
            {
                NewARC().Replace(path);
            }
        }

        public void ImportBRES()
        {
            if (Program.OpenFile(FileFilters.BRES, out string path))
            {
                NewBRES().Replace(path);
            }
        }

        public void ImportMSBin()
        {
            if (Program.OpenFile(FileFilters.MSBin, out string path))
            {
                NewMSBin().Replace(path);
            }
        }

        public override void OnExport(string outPath, int filterIndex)
        {
            if (outPath.Contains("."))
            {
                switch (outPath.Substring(outPath.ToLowerInvariant().LastIndexOf(".")))
                {
                    case ".szs":
                        ((U8Node) _resource).ExportSZS(outPath);
                        break;
                    case ".pair":
                        ((U8Node) _resource).ExportPair(outPath);
                        break;
                    default:
                        ((U8Node) _resource).Export(outPath);
                        break;
                }
            }
            else
            {
                ((U8Node) _resource).Export(outPath);
            }
        }

        public void PreviewAll()
        {
            new ModelForm().Show(_owner, ModelPanel.CollectModels(_resource));
        }

        public void ExportAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
            {
                return;
            }

            bool hasModels = false;
            bool hasTextures = false;
            foreach (ResourceNode r in _resource.Children)
            {
                if (hasModels && hasTextures)
                {
                    break;
                }

                if (r is BRRESNode)
                {
                    foreach (BRESGroupNode e in r.Children)
                    {
                        if (e.Type == BRESGroupNode.BRESGroupType.Textures)
                        {
                            hasTextures = true;
                        }
                        else if (e.Type == BRESGroupNode.BRESGroupType.Models)
                        {
                            hasModels = true;
                        }

                        if (hasModels && hasTextures)
                        {
                            break;
                        }
                    }
                }
                else if (r is U8FolderNode)
                {
                    searchU8Folder((U8FolderNode) r, out bool hasModelsTemp, out bool hasTexturesTemp);
                    hasModels = hasModels || hasModelsTemp;
                    hasTextures = hasTextures || hasTexturesTemp;
                }
            }

            string extensionTEX0 = ".tex0";
            string extensionMDL0 = ".mdl0";

            if (hasTextures)
            {
                ExportAllFormatDialog dialog = new ExportAllFormatDialog("Export All", typeof(TEX0Node), FileFilters.TEX0);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    extensionTEX0 = dialog.SelectedExtension;
                }
                else
                {
                    return;
                }
            }

            if (hasModels)
            {
                ExportAllFormatDialog dialog = new ExportAllFormatDialog("Export All", typeof(MDL0Node), FileFilters.MDL0Export);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    extensionMDL0 = dialog.SelectedExtension;
                }
                else
                {
                    return;
                }
            }

            ((U8Node) _resource).ExtractToFolder(path, extensionTEX0, extensionMDL0);
        }

        public void searchU8Folder(U8FolderNode u8, out bool hasModels, out bool hasTextures)
        {
            hasModels = false;
            hasTextures = false;
            foreach (ResourceNode r in u8.Children)
            {
                if (hasModels && hasTextures)
                {
                    break;
                }

                if (r is BRRESNode)
                {
                    foreach (BRESGroupNode e in r.Children)
                    {
                        if (e.Type == BRESGroupNode.BRESGroupType.Textures)
                        {
                            hasTextures = true;
                        }
                        else if (e.Type == BRESGroupNode.BRESGroupType.Models)
                        {
                            hasModels = true;
                        }

                        if (hasModels && hasTextures)
                        {
                            break;
                        }
                    }
                }
                else if (r is U8FolderNode)
                {
                    searchU8Folder((U8FolderNode) r, out bool hasModelsTemp, out bool hasTexturesTemp);
                    hasModels = hasModels || hasModelsTemp;
                    hasTextures = hasTextures || hasTexturesTemp;
                }
            }
        }

        public void ReplaceAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
            {
                return;
            }

            ((ARCNode) _resource).ReplaceFromFolder(path);
        }
    }
}