using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.MDL0Shader)]
    public class MDL0ShaderWrapper : GenericWrapper
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

        static MDL0ShaderWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(DuplicateToolStripMenuItem);
            _menu.Items.Add(ReplaceToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(MoveUpToolStripMenuItem);
            _menu.Items.Add(MoveDownToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(
                new ToolStripMenuItem("Add New Stage", null, CreateAction, Keys.Control | Keys.Alt | Keys.N));
            _menu.Items.Add(DeleteToolStripMenuItem);
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void CreateAction(object sender, EventArgs e)
        {
            GetInstance<MDL0ShaderWrapper>().CreateStage();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[6].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDL0ShaderWrapper w = GetInstance<MDL0ShaderWrapper>();
            _menu.Items[6].Enabled = w._resource.Children.Count < 16; //16 stages max!
        }

        private void CreateStage()
        {
            if (_resource.Children.Count < 16)
            {
                MDL0TEVStageNode stage = new MDL0TEVStageNode();
                _resource.AddChild(stage, true);

                Nodes[Nodes.Count - 1].EnsureVisible();
                //TreeView.SelectedNode = Nodes[Nodes.Count - 1];
            }
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
                    n.Name = "Shader" + n.Index;
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
                    n.Name = "Shader" + n.Index;
                }
            }
        }

        #endregion

        public override string ExportFilter => FileFilters.MDL0Shader;

        public MDL0ShaderWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }
}