using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.BLOC)]
    public class BLOCWrapper : GenericWrapper
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

        static BLOCWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Ne&w", null,
                new ToolStripMenuItem("GSND Archive", null, NewGSNDAction),
                new ToolStripMenuItem("ADSJ Stepjump File", null, NewADSJAction),
                new ToolStripMenuItem("GDOR Adventure Door File", null, NewGDORAction),
                new ToolStripMenuItem("GDBF Factory Door File", null, NewGDBFAction),
                new ToolStripMenuItem("GWAT Swimmable Water File", null, NewGWATAction),
                new ToolStripMenuItem("GEG1 Enemy File", null, NewGEG1Action),
                new ToolStripMenuItem("GCAM Animated Camera File", null, NewGCAMAction),
                new ToolStripMenuItem("GITM Fighter Trophy File", null, NewGITMAction)
            ));

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

        protected static void NewGSNDAction(object sender, EventArgs e)
        {
            GetInstance<BLOCWrapper>().NewGSND();
        }

        protected static void NewADSJAction(object sender, EventArgs e)
        {
            GetInstance<BLOCWrapper>().NewADSJ();
        }

        protected static void NewGDORAction(object sender, EventArgs e)
        {
            GetInstance<BLOCWrapper>().NewGDOR();
        }

        protected static void NewGDBFAction(object sender, EventArgs e)
        {
            GetInstance<BLOCWrapper>().NewGDBF();
        }

        protected static void NewGWATAction(object sender, EventArgs e)
        {
            GetInstance<BLOCWrapper>().NewGWAT();
        }

        protected static void NewGEG1Action(object sender, EventArgs e)
        {
            GetInstance<BLOCWrapper>().NewGEG1();
        }

        protected static void NewGCAMAction(object sender, EventArgs e)
        {
            GetInstance<BLOCWrapper>().NewGCAM();
        }

        protected static void NewGITMAction(object sender, EventArgs e)
        {
            GetInstance<BLOCWrapper>().NewGITM();
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
            BLOCWrapper w = GetInstance<BLOCWrapper>();

            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
        }

        #endregion

        public override string ExportFilter => FileFilters.BLOC;

        public BLOCWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public GSNDNode NewGSND()
        {
            GSNDNode node = new GSNDNode {Name = _resource.FindName("NewGSND")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public ADSJNode NewADSJ()
        {
            ADSJNode node = new ADSJNode {Name = _resource.FindName("NewADSJ")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public GDORNode NewGDOR()
        {
            GDORNode node = new GDORNode {Name = _resource.FindName("NewGDOR")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public GDBFNode NewGDBF()
        {
            GDBFNode node = new GDBFNode {Name = _resource.FindName("NewGDBF")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public GWATNode NewGWAT()
        {
            GWATNode node = new GWATNode {Name = _resource.FindName("NewGWAT")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public GEG1Node NewGEG1()
        {
            GEG1Node node = new GEG1Node {Name = _resource.FindName("NewGEG1")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public GCAMNode NewGCAM()
        {
            GCAMNode node = new GCAMNode {Name = _resource.FindName("NewGCAM")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public GITMNode NewGITM()
        {
            GITMNode node = new GITMNode {Name = _resource.FindName("NewGITM")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public override void OnExport(string outPath, int filterIndex)
        {
            ((BLOCNode) _resource).Export(outPath);
        }
    }
}