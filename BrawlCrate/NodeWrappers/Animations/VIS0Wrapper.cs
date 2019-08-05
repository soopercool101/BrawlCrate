using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.VIS0)]
    public class VIS0Wrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        static VIS0Wrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&New Entry", null, NewBoneAction, Keys.Control | Keys.H));
            _menu.Items.Add(new ToolStripMenuItem("Edit", null,
                //new ToolStripMenuItem("&Merge Animation", null, MergeAction),
                new ToolStripMenuItem("&Append Animation", null, AppendAction),
                new ToolStripMenuItem("Res&ize", null, ResizeAction)));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripSeparator());
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

        protected static void NewBoneAction(object sender, EventArgs e)
        {
            GetInstance<VIS0Wrapper>().NewBone();
        }

        protected static void MergeAction(object sender, EventArgs e)
        {
            GetInstance<VIS0Wrapper>().Merge();
        }

        protected static void AppendAction(object sender, EventArgs e)
        {
            GetInstance<VIS0Wrapper>().Append();
        }

        protected static void ResizeAction(object sender, EventArgs e)
        {
            GetInstance<VIS0Wrapper>().Resize();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (replaceToolStripMenuItem != null)
            {
                replaceToolStripMenuItem.Enabled = true;
            }

            if (restoreToolStripMenuItem != null)
            {
                restoreToolStripMenuItem.Enabled = true;
            }

            if (moveUpToolStripMenuItem != null)
            {
                moveUpToolStripMenuItem.Enabled = true;
            }

            if (moveDownToolStripMenuItem != null)
            {
                moveDownToolStripMenuItem.Enabled = true;
            }

            if (deleteToolStripMenuItem != null)
            {
                deleteToolStripMenuItem.Enabled = true;
            }
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<VIS0Wrapper>();

            if (replaceToolStripMenuItem != null)
            {
                replaceToolStripMenuItem.Enabled = w.Parent != null;
            }

            if (restoreToolStripMenuItem != null)
            {
                restoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            }

            if (moveUpToolStripMenuItem != null)
            {
                moveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            }

            if (moveDownToolStripMenuItem != null)
            {
                moveDownToolStripMenuItem.Enabled = w.NextNode != null;
            }

            if (deleteToolStripMenuItem != null)
            {
                deleteToolStripMenuItem.Enabled = w.Parent != null;
            }
        }

        #endregion

        public VIS0Wrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.VIS0;

        public void NewBone()
        {
            VIS0EntryNode node = ((VIS0Node) _resource).CreateEntry();
            BaseWrapper res = FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        private void Merge()
        {
        }

        private void Append()
        {
            ((VIS0Node) _resource).Append();
            BaseWrapper res = FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        private void Resize()
        {
            ((VIS0Node) _resource).Resize();
            BaseWrapper res = FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
    }
}