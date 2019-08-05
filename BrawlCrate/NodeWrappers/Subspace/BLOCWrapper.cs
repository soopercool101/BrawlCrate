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

        static BLOCWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Ne&w", null,
                new ToolStripMenuItem("GSND Archive", null, NewGSNDAction),
                new ToolStripMenuItem("ADSJ Stepjump File", null, NewADSJAction),
                new ToolStripMenuItem("GDOR Adventure Door File", null, NewGDORAction)
            ));

            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(replaceToolStripMenuItem = new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(restoreToolStripMenuItem = new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(moveUpToolStripMenuItem = new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(moveDownToolStripMenuItem = new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
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

        #endregion

        public override string ExportFilter => BrawlLib.FileFilters.BLOC;

        public BLOCWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public GSNDNode NewGSND()
        {
            GSNDNode node = new GSNDNode() {Name = _resource.FindName("NewGSND")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public ADSJNode NewADSJ()
        {
            ADSJNode node = new ADSJNode() {Name = _resource.FindName("NewADSJ")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public GDORNode NewGDOR()
        {
            GDORNode node = new GDORNode() {Name = _resource.FindName("NewGDOR")};
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