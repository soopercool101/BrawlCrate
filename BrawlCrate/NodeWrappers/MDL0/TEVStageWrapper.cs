using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.TEVStage)]
    public class TEVStageWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        static TEVStageWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu.Items.Add(moveUpToolStripMenuItem = new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(moveDownToolStripMenuItem = new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        public override void MoveUp(bool select)
        {
            if (PrevNode == null)
            {
                return;
            }

            if (_resource.MoveUp())
            {
                int index = Index - 1;
                TreeNode parent = Parent;
                TreeView.BeginUpdate();
                Remove();
                parent.Nodes.Insert(index, this);
                if (select)
                {
                    TreeView.SelectedNode = this;
                }

                TreeView.EndUpdate();

                foreach (ResourceNode n in _resource.Parent.Children)
                {
                    n.Name = "Stage" + n.Index;
                }
            }
        }

        public override void MoveDown(bool select)
        {
            if (NextNode == null)
            {
                return;
            }

            if (_resource.MoveDown())
            {
                int index = Index + 1;
                TreeNode parent = Parent;
                TreeView.BeginUpdate();
                Remove();
                parent.Nodes.Insert(index, this);
                if (select)
                {
                    TreeView.SelectedNode = this;
                }

                TreeView.EndUpdate();

                foreach (ResourceNode n in _resource.Parent.Children)
                {
                    n.Name = "Stage" + n.Index;
                }
            }
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
            var w = GetInstance<TEVStageWrapper>();

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

        public TEVStageWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }
}