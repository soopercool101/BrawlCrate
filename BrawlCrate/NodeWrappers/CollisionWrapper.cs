using BrawlCrate.UI;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.CollisionDef)]
    public class CollisionWrapper : GenericWrapper
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

        static CollisionWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Preview / Edit", null, EditAction, Keys.Control | Keys.P));
            _menu.Items.Add(new ToolStripMenuItem("&Advanced Editor", null, AdvancedEditAction,
                Keys.Control | Keys.Shift | Keys.P));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Merge", null, MergeAction, Keys.Control | Keys.M));
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

        private static void EditAction(object sender, EventArgs e)
        {
            GetInstance<CollisionWrapper>().Preview();
        }

        private static void AdvancedEditAction(object sender, EventArgs e)
        {
            GetInstance<CollisionWrapper>().AdvancedEdit();
        }

        private static void MergeAction(object sender, EventArgs e)
        {
            GetInstance<CollisionWrapper>().Merge();
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
            CollisionWrapper w = GetInstance<CollisionWrapper>();

            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
        }

        #endregion

        public override string ExportFilter => FileFilters.CollisionDef;

        public CollisionWrapper()
        {
            ContextMenuStrip = _menu;
        }

        private void Preview()
        {
            using (CollisionForm frm = new CollisionForm())
            {
                frm.ShowDialog(null, _resource as CollisionNode);
            }
        }

        private void Merge()
        {
            ((CollisionNode) _resource).MergeWith();
            MainForm.Instance.resourceTree_SelectionChanged(this, null);
        }

        private void AdvancedEdit()
        {
            DialogResult CollisionResult = MessageBox.Show(
                @"Please note: The advanced collision editor is for experimental purposes only.
Unless you really know what you're doing, the regular collision editor is overall better for the same purposes.

Are you sure you'd like to open in the Advanced Editor?",
                "Open Advanced Editor", MessageBoxButtons.YesNo);
            if (CollisionResult == DialogResult.Yes)
            {
                using (AdvancedCollisionForm frm = new AdvancedCollisionForm())
                {
                    frm.ShowDialog(null, _resource as CollisionNode);
                }
            }
        }
    }
}