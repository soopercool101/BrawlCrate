using System;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib.Wii.Animations;
using System.Collections.Generic;
using System.Linq;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.CHR0)]
    public class CHR0Wrapper : GenericWrapper, MultiSelectableWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu, _multiSelectMenu;
        static CHR0Wrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Ne&w Bone Target", null, NewBoneAction, Keys.Control | Keys.W));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Edit", null,
                new ToolStripMenuItem("&Merge Animation", null, MergeAction),
                new ToolStripMenuItem("&Append Animation", null, AppendAction),
                new ToolStripMenuItem("Res&ize", null, ResizeAction)));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move &Down", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;

            _multiSelectMenu = new ContextMenuStrip();
            _multiSelectMenu.Items.Add(new ToolStripMenuItem("Edit All", null, EditAllAction));
        }
        protected static void NewBoneAction(object sender, EventArgs e) { GetInstance<CHR0Wrapper>().NewBone(); }
        protected static void MergeAction(object sender, EventArgs e) { GetInstance<CHR0Wrapper>().Merge(); }
        protected static void AppendAction(object sender, EventArgs e) { GetInstance<CHR0Wrapper>().Append(); }
        protected static void ResizeAction(object sender, EventArgs e) { GetInstance<CHR0Wrapper>().Resize(); }
        protected static void EditAllAction(object sender, EventArgs e) { EditAll(GetInstances<CHR0Wrapper>()); }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[5].Enabled = _menu.Items[6].Enabled = _menu.Items[8].Enabled = _menu.Items[9].Enabled = _menu.Items[12].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            CHR0Wrapper w = GetInstance<CHR0Wrapper>();
            _menu.Items[5].Enabled = _menu.Items[12].Enabled = w.Parent != null;
            _menu.Items[6].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[8].Enabled = w.PrevNode != null;
            _menu.Items[9].Enabled = w.NextNode != null;
        }

        #endregion

        public CHR0Wrapper() { ContextMenuStrip = _menu; }

        public override string ExportFilter { get { return FileFilters.CHR0Export; } }
        public override string ImportFilter { get { return FileFilters.CHR0Import; } }

        public ContextMenuStrip MultiSelectMenuStrip { get { return _multiSelectMenu; } }

        public void NewBone()
        {
            CHR0EntryNode node = ((CHR0Node)_resource).CreateEntry();
            BaseWrapper res = this.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void Merge()
        {
            ((CHR0Node)_resource).MergeWith();
            BaseWrapper res = this.FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void Resize()
        {
            ((CHR0Node)_resource).Resize();
            BaseWrapper res = this.FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
        
        public void Append()
        {
            ((CHR0Node)_resource).Append();
            BaseWrapper res = this.FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        private static void EditAll(IEnumerable<BaseWrapper> wrappers)
        {
            EditAllDialog ctd = new EditAllDialog();
            ctd.ShowDialog(_owner, wrappers.Select(n => n.Resource));
        }
    }

    [NodeWrapper(ResourceType.CHR0Entry)]
    public class CHR0EntryWrapper : GenericWrapper, MultiSelectableWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu, _multiSelectMenu;
        static CHR0EntryWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("View Interpolation", null, ViewInterp, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;

            _multiSelectMenu = new ContextMenuStrip();
            _multiSelectMenu.Items.Add(new ToolStripMenuItem("Edit All", null, EditAllAction));
        }
        protected static void EditAllAction(object sender, EventArgs e) { EditAll(GetInstances<CHR0EntryWrapper>()); }
        protected static void ViewInterp(object sender, EventArgs e) { GetInstance<CHR0EntryWrapper>().ViewInterp(); }
        private void ViewInterp()
        {
            InterpolationForm f = MainForm.Instance.InterpolationForm;
            if (f != null)
            {
                InterpolationEditor e = f._interpolationEditor;
                if (e != null)
                    e.SetTarget(_resource as IKeyframeSource);
            }
        }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[3].Enabled = _menu.Items[4].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[10].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            CHR0EntryWrapper w = GetInstance<CHR0EntryWrapper>();
            _menu.Items[3].Enabled = _menu.Items[10].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }

        #endregion

        public CHR0EntryWrapper(IWin32Window owner) { _owner = owner; ContextMenuStrip = _menu; }
        public CHR0EntryWrapper() { _owner = null; ContextMenuStrip = _menu; }

        public ContextMenuStrip MultiSelectMenuStrip { get { return _multiSelectMenu; } }

        private static void EditAll(IEnumerable<BaseWrapper> wrappers)
        {
            EditAllDialog ctd = new EditAllDialog();
            ctd.ShowDialog(_owner, wrappers.Select(n => n.Resource));
        }
    }
}
