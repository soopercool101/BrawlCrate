using System.ComponentModel;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.TEVStage)]
    public class TEVStageWrapper : GenericWrapper
    {
        public TEVStageWrapper()
        {
            ContextMenuStrip = _menu;
        }

        #region Menu

        private static readonly ContextMenuStrip _menu;

        static TEVStageWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[0].Enabled = _menu.Items[1].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<TEVStageWrapper>();
            _menu.Items[0].Enabled = w.PrevNode != null;
            _menu.Items[1].Enabled = w.NextNode != null;
        }

        public override void MoveUp(bool select)
        {
            if (PrevNode == null) return;

            if (_resource.MoveUp())
            {
                var index = Index - 1;
                var parent = Parent;
                TreeView.BeginUpdate();
                Remove();
                parent.Nodes.Insert(index, this);
                if (select) TreeView.SelectedNode = this;

                TreeView.EndUpdate();

                foreach (var n in _resource.Parent.Children) n.Name = "Stage" + n.Index;
            }
        }

        public override void MoveDown(bool select)
        {
            if (NextNode == null) return;

            if (_resource.MoveDown())
            {
                var index = Index + 1;
                var parent = Parent;
                TreeView.BeginUpdate();
                Remove();
                parent.Nodes.Insert(index, this);
                if (select) TreeView.SelectedNode = this;

                TreeView.EndUpdate();

                foreach (var n in _resource.Parent.Children) n.Name = "Stage" + n.Index;
            }
        }

        #endregion
    }
}