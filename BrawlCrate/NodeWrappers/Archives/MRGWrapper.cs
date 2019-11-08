using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.MRG)]
    public class MRGWrapper : GenericWrapper
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

        static MRGWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Ne&w", null,
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
            GetInstance<ARCWrapper>().NewBRES();
        }

        protected static void NewARCAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().NewARC();
        }

        protected static void NewMSBinAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().NewMSBin();
        }

        protected static void ImportBRESAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().ImportBRES();
        }

        protected static void ImportARCAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().ImportARC();
        }

        protected static void ImportMSBinAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().ImportMSBin();
        }

        protected static void ExportAllAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().ExportAll();
        }

        protected static void ReplaceAllAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().ReplaceAll();
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
            MRGWrapper w = GetInstance<MRGWrapper>();

            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
        }

        #endregion

        public override string ExportFilter => FileFilters.MRGExport;
        public override string ImportFilter => FileFilters.MRGImport;

        public MRGWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public MRGNode NewMRG()
        {
            MRGNode node = new MRGNode {Name = _resource.FindName("NewMRG")};
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
            if (Program.OpenFile(FileFilters.MRGImport, out string path))
            {
                NewMRG().Replace(path);
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
            ((MRGNode) _resource).Export(outPath);
        }

        public void ExportAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
            {
                return;
            }

            ((MRGNode) _resource).ExtractToFolder(path);
        }

        public void ReplaceAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
            {
                return;
            }

            ((MRGNode) _resource).ReplaceFromFolder(path);
        }
    }
}