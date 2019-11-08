using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.ResourceNodes.MDL0;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers.MDL0
{
    [NodeWrapper(ResourceType.MDL0Bone)]
    public class MDL0BoneWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        private static readonly ToolStripMenuItem _addToParentToolStripMenuItem;
        private static readonly ToolStripMenuItem _addToNextUpToolStripMenuItem;
        private static readonly ToolStripMenuItem _addToNextDownToolStripMenuItem;

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

        static MDL0BoneWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(DuplicateToolStripMenuItem);
            _menu.Items.Add(ReplaceToolStripMenuItem);
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(MoveUpToolStripMenuItem);
            _menu.Items.Add(MoveDownToolStripMenuItem);
            _menu.Items.Add(_addToParentToolStripMenuItem = new ToolStripMenuItem("Add To &Parent", null,
                AddToParentAction,
                Keys.Control | Keys.Shift | Keys.Up));
            _menu.Items.Add(_addToNextUpToolStripMenuItem = new ToolStripMenuItem("Add To Next &Up", null, AddUpAction,
                Keys.Control | Keys.Alt | Keys.Up));
            _menu.Items.Add(_addToNextDownToolStripMenuItem = new ToolStripMenuItem("Add To Next D&own", null,
                AddDownAction,
                Keys.Control | Keys.Alt | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move to end of bone array", null, RemapAction));
            _menu.Items.Add(new ToolStripMenuItem("Regenerate bone array", null, RegenAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(
                new ToolStripMenuItem("Add New Child", null, CreateAction, Keys.Control | Keys.Alt | Keys.N));
            _menu.Items.Add(DeleteToolStripMenuItem);
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void AddToParentAction(object sender, EventArgs e)
        {
            GetInstance<MDL0BoneWrapper>().AddToParent();
        }

        protected static void AddUpAction(object sender, EventArgs e)
        {
            GetInstance<MDL0BoneWrapper>().AddUp();
        }

        protected static void AddDownAction(object sender, EventArgs e)
        {
            GetInstance<MDL0BoneWrapper>().AddDown();
        }

        protected static void CreateAction(object sender, EventArgs e)
        {
            GetInstance<MDL0BoneWrapper>().CreateNode();
        }

        protected static void RemapAction(object sender, EventArgs e)
        {
            GetInstance<MDL0BoneWrapper>().Remap();
        }

        protected static void RegenAction(object sender, EventArgs e)
        {
            GetInstance<MDL0BoneWrapper>().Regen();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            MoveUpToolStripMenuItem.Enabled = true;
            MoveDownToolStripMenuItem.Enabled = true;
            _addToParentToolStripMenuItem.Enabled = true;
            _addToNextUpToolStripMenuItem.Enabled = true;
            _addToNextDownToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDL0BoneWrapper w = GetInstance<MDL0BoneWrapper>();
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            _addToParentToolStripMenuItem.Enabled = w.Parent != null && w._resource.Parent is MDL0BoneNode;
            _addToNextUpToolStripMenuItem.Enabled = w.PrevNode != null;
            _addToNextDownToolStripMenuItem.Enabled = w.NextNode != null;
        }

        public void Regen()
        {
            MDL0BoneNode b = _resource as MDL0BoneNode;
            MDL0Node m = b?.Model;
            if (m != null)
            {
                m._linker.RegenerateBoneCache(true);
                OnUpdateProperties(null, null);
                b.SignalPropertyChange();
            }
        }

        public void Remap()
        {
            MDL0BoneNode b = _resource as MDL0BoneNode;
            MDL0Node m = b?.Model;
            if (m != null)
            {
                b._entryIndex = m._linker.BoneCache.Length;
                m._linker.RegenerateBoneCache();
                OnUpdateProperties(null, null);
                b.SignalPropertyChange();
            }
        }

        public unsafe void AddUp()
        {
            //try
            //{
            if (PrevNode == null)
            {
                return;
            }

            if (_resource.AddUp())
            {
                TreeNode prev = PrevNode;
                TreeView.BeginUpdate();
                Remove();
                prev.Nodes.Add(this);
                _resource.Parent = _resource.Parent.Children[_resource.Index - 1];
                _resource.OnMoved();
                TreeView.EndUpdate();
                EnsureVisible();
                //TreeView.SelectedNode = this;
            }
            else
            {
                return;
            }

            //}
            //catch { return; }
        }

        public void AddDown()
        {
            //try
            //{
            if (NextNode == null)
            {
                return;
            }

            if (_resource.AddDown())
            {
                TreeNode next = NextNode;
                TreeView.BeginUpdate();
                Remove();
                next.Nodes.Add(this);
                _resource.Parent = _resource.Parent.Children[_resource.Index + 1];
                _resource.OnMoved();
                TreeView.EndUpdate();
                EnsureVisible();
                //TreeView.SelectedNode = this;
            }
            else
            {
                return;
            }

            //}
            //catch { return; }
        }

        public void AddToParent()
        {
            //try
            //{
            if (Parent == null)
            {
                return;
            }

            if (_resource.ToParent())
            {
                TreeNode parent = Parent;
                TreeView.BeginUpdate();
                Remove();
                parent.Parent.Nodes.Add(this);
                _resource.Parent = _resource.Parent.Parent;
                _resource.OnMoved();
                TreeView.EndUpdate();
                EnsureVisible();
                //TreeView.SelectedNode = this;
            }

            //}
            //catch { return; }
        }

        private void CreateNode()
        {
            TreeView.BeginUpdate();

            int id = 1;
            string name = "NewBone0";
            MDL0Node model = ((MDL0BoneNode) _resource).Model;
            Top:
            foreach (MDL0BoneNode b in model._linker.BoneCache)
            {
                if (b.Name == name)
                {
                    name = "NewBone" + id++;
                    goto Top;
                }
            }

            MDL0BoneNode bone = new MDL0BoneNode {Name = name, _entryIndex = model._linker.BoneCache.Length};
            bone.FrameState = bone.BindState = FrameState.Neutral;
            _resource.AddChild(bone, true);

            bone.RecalcFrameState();
            bone.RecalcBindState(false, false, false);

            model._linker.RegenerateBoneCache();

            TreeView.EndUpdate();

            Nodes[Nodes.Count - 1].EnsureVisible();
            //TreeView.SelectedNode = Nodes[Nodes.Count - 1];
        }

        #endregion

        public MDL0BoneWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }
}