using System;
using System.ComponentModel;
using System.Windows.Forms;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.MDL0Bone)]
    public class MDL0BoneWrapper : GenericWrapper
    {
        public MDL0BoneWrapper()
        {
            ContextMenuStrip = _menu;
        }

        #region Menu

        private static readonly ContextMenuStrip _menu;

        static MDL0BoneWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Add To &Parent", null, AddToParentAction,
                Keys.Control | Keys.Shift | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Add To Next &Up", null, AddUpAction,
                Keys.Control | Keys.Alt | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Add To Next D&own", null, AddDownAction,
                Keys.Control | Keys.Alt | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move to end of bone array", null, RemapAction));
            _menu.Items.Add(new ToolStripMenuItem("Regenerate bone array", null, RegenAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(
                new ToolStripMenuItem("Add New Child", null, CreateAction, Keys.Control | Keys.Alt | Keys.N));
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
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
            _menu.Items[4].Enabled = _menu.Items[5].Enabled =
                _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[8].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<MDL0BoneWrapper>();
            _menu.Items[4].Enabled = w.PrevNode != null;
            _menu.Items[5].Enabled = w.NextNode != null;
            _menu.Items[6].Enabled = w.Parent != null && w._resource.Parent is MDL0BoneNode;
            _menu.Items[7].Enabled = w.PrevNode != null;
            _menu.Items[8].Enabled = w.NextNode != null;
        }

        public void Regen()
        {
            var b = _resource as MDL0BoneNode;
            if (b != null)
            {
                var m = b.Model;
                if (m != null)
                {
                    m._linker.RegenerateBoneCache(true);
                    OnUpdateProperties(null, null);
                    b.SignalPropertyChange();
                }
            }
        }

        public void Remap()
        {
            var b = _resource as MDL0BoneNode;
            if (b != null)
            {
                var m = b.Model;
                if (m != null)
                {
                    b._entryIndex = m._linker.BoneCache.Length;
                    m._linker.RegenerateBoneCache();
                    OnUpdateProperties(null, null);
                    b.SignalPropertyChange();
                }
            }
        }

        public void AddUp()
        {
            //try
            //{
            if (PrevNode == null) return;

            if (_resource.AddUp())
            {
                var prev = PrevNode;
                TreeView.BeginUpdate();
                Remove();
                prev.Nodes.Add(this);
                _resource.Parent = _resource.Parent.Children[_resource.Index - 1];
                _resource.OnMoved();
                TreeView.EndUpdate();
                EnsureVisible();
                //TreeView.SelectedNode = this;
            }

            //}
            //catch { return; }
        }

        public void AddDown()
        {
            //try
            //{
            if (NextNode == null) return;

            if (_resource.AddDown())
            {
                var next = NextNode;
                TreeView.BeginUpdate();
                Remove();
                next.Nodes.Add(this);
                _resource.Parent = _resource.Parent.Children[_resource.Index + 1];
                _resource.OnMoved();
                TreeView.EndUpdate();
                EnsureVisible();
                //TreeView.SelectedNode = this;
            }

            //}
            //catch { return; }
        }

        public void AddToParent()
        {
            //try
            //{
            if (Parent == null) return;

            if (_resource.ToParent())
            {
                var parent = Parent;
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

            var id = 1;
            var name = "NewBone0";
            var model = ((MDL0BoneNode) _resource).Model;
            Top:
            foreach (var b in model._linker.BoneCache)
                if (b.Name == name)
                {
                    name = "NewBone" + id++;
                    goto Top;
                }

            var bone = new MDL0BoneNode {Name = name, _entryIndex = model._linker.BoneCache.Length};
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
    }
}