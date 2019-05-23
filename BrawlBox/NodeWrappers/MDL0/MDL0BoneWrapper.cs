using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib.Modeling;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.MDL0Bone)]
    public class MDL0BoneWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static MDL0BoneWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Add To &Parent", null, AddToParentAction, Keys.Control | Keys.Shift | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Add To Next &Up", null, AddUpAction, Keys.Control | Keys.Alt | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Add To Next D&own", null, AddDownAction, Keys.Control | Keys.Alt | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move to end of bone array", null, RemapAction));
            _menu.Items.Add(new ToolStripMenuItem("Regenerate bone array", null, RegenAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Add New Child", null, CreateAction, Keys.Control | Keys.Alt | Keys.N));
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void AddToParentAction(object sender, EventArgs e) { GetInstance<MDL0BoneWrapper>().AddToParent(); }
        protected static void AddUpAction(object sender, EventArgs e) { GetInstance<MDL0BoneWrapper>().AddUp(); }
        protected static void AddDownAction(object sender, EventArgs e) { GetInstance<MDL0BoneWrapper>().AddDown(); }
        protected static void CreateAction(object sender, EventArgs e) { GetInstance<MDL0BoneWrapper>().CreateNode(); }
        protected static void RemapAction(object sender, EventArgs e) { GetInstance<MDL0BoneWrapper>().Remap(); }
        protected static void RegenAction(object sender, EventArgs e) { GetInstance<MDL0BoneWrapper>().Regen(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[4].Enabled = _menu.Items[5].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[8].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDL0BoneWrapper w = GetInstance<MDL0BoneWrapper>();
            _menu.Items[4].Enabled = w.PrevNode != null;
            _menu.Items[5].Enabled = w.NextNode != null;
            _menu.Items[6].Enabled = w.Parent != null && w._resource.Parent is MDL0BoneNode;
            _menu.Items[7].Enabled = w.PrevNode != null;
            _menu.Items[8].Enabled = w.NextNode != null;
        }

        public void Regen()
        {
            MDL0BoneNode b = _resource as MDL0BoneNode;
            if (b != null)
            {
                MDL0Node m = b.Model;
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
            MDL0BoneNode b = _resource as MDL0BoneNode;
            if (b != null)
            {
                MDL0Node m = b.Model;
                if (m != null)
                {
                    b._entryIndex = m._linker.BoneCache.Length;
                    m._linker.RegenerateBoneCache();
                    OnUpdateProperties(null, null);
                    b.SignalPropertyChange();
                }
            }
        }

        public unsafe void AddUp()
        {
            //try
            //{
                if (PrevNode == null)
                    return;

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
                    return;
            //}
            //catch { return; }
        }

        public void AddDown()
        {
            //try
            //{
                if (NextNode == null)
                    return;

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
                    return;
            //}
            //catch { return; }
        }

        public void AddToParent()
        {
            //try
            //{
                if (Parent == null)
                    return;

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
            MDL0Node model = ((MDL0BoneNode)_resource).Model;
            Top:
            foreach (MDL0BoneNode b in model._linker.BoneCache)
            {
                if (b.Name == name)
                {
                    name = "NewBone" + id++;
                    goto Top;
                }
            }

            MDL0BoneNode bone = new MDL0BoneNode() { Name = name, _entryIndex = model._linker.BoneCache.Length };
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

        public MDL0BoneWrapper() { ContextMenuStrip = _menu; }
    }
}
