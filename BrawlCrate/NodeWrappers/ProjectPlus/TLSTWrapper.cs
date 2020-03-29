using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.ResourceNodes.ProjectPlus;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.TLST)]
    internal class TLSTWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;
        private static readonly ToolStripMenuItem _newEntryToolStripMenuItem;

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

        static TLSTWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(_newEntryToolStripMenuItem =
                new ToolStripMenuItem("Add New Entry", null, NewEntryAction, Keys.Control | Keys.J));
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

        protected static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<TLSTWrapper>().NewEntry();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _newEntryToolStripMenuItem.Enabled = true;
            DuplicateToolStripMenuItem.Enabled = true;
            ReplaceToolStripMenuItem.Enabled = true;
            DeleteToolStripMenuItem.Enabled = true;
            RestoreToolStripMenuItem.Enabled = true;
            MoveUpToolStripMenuItem.Enabled = true;
            MoveDownToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            TLSTWrapper w = GetInstance<TLSTWrapper>();
            _newEntryToolStripMenuItem.Enabled = w._resource.Children.Count < 50;
            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
        }

        #endregion

        public override string ExportFilter => FileFilters.TLST;

        public TLSTEntryNode NewEntry()
        {
            StringInputDialog d = new StringInputDialog("New TLST Entry", "");
            if (d.ShowDialog() == DialogResult.OK)
            {
                // Get an unused expanded song id
                uint songID = 0x0000F000;
                while (Resource.Children.Count(n => ((TLSTEntryNode)n)._songID == songID) > 0)
                {
                    songID++;
                }
                TLSTEntryNode node = new TLSTEntryNode
                {
                    _name = d.resultString,
                    _songID = songID
                };

                _resource.AddChild(node);
                BaseWrapper w = FindResource(node, false);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
                return node;
            }

            return null;
        }

        public TLSTWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }
}
