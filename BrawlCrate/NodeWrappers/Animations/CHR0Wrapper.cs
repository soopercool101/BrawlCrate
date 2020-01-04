using BrawlCrate.UI;
using BrawlLib.Internal.Windows.Controls;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.CHR0)]
    public class CHR0Wrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;
        private static readonly ContextMenuStrip MultiSelectMenu;

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

        private static readonly ToolStripMenuItem ExportSelectedToolStripMenuItem =
            new ToolStripMenuItem("&Export Selected", null, ExportSelectedAction, Keys.Control | Keys.E);

        private static readonly ToolStripMenuItem DeleteSelectedToolStripMenuItem =
            new ToolStripMenuItem("&Delete Selected", null, DeleteSelectedAction, Keys.Control | Keys.Delete);

        static CHR0Wrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Ne&w Bone Target", null, NewBoneAction, Keys.Control | Keys.W));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Edit", null,
                new ToolStripMenuItem("&Merge Animation", null, MergeAction),
                new ToolStripMenuItem("&Append Animation", null, AppendAction),
                new ToolStripMenuItem("Repeat Animation", null, LoopAction),
                new ToolStripMenuItem("Reverse Animation", null, ReverseAction),
                new ToolStripMenuItem("Reverse to Loop", null, ReverseLoopAction),
                new ToolStripMenuItem("Sort Entries", null, SortAction),
                new ToolStripMenuItem("Res&ize", null, ResizeAction)));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(DuplicateToolStripMenuItem);
            _menu.Items.Add(ReplaceToolStripMenuItem);
            _menu.Items.Add(RestoreToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(MoveUpToolStripMenuItem);
            _menu.Items.Add(new ToolStripMenuItem("Move &Down", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(DeleteToolStripMenuItem);
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;

            MultiSelectMenu = new ContextMenuStrip();
            MultiSelectMenu.Items.Add(new ToolStripMenuItem("Edit All", null, EditAllAction));
            MultiSelectMenu.Items.Add(new ToolStripSeparator());
            MultiSelectMenu.Items.Add(ExportSelectedToolStripMenuItem);
            MultiSelectMenu.Items.Add(DeleteSelectedToolStripMenuItem);
            MultiSelectMenu.Opening += MultiMenuOpening;
            MultiSelectMenu.Closing += MultiMenuClosing;
        }

        protected static void NewBoneAction(object sender, EventArgs e)
        {
            GetInstance<CHR0Wrapper>().NewBone();
        }

        protected static void MergeAction(object sender, EventArgs e)
        {
            GetInstance<CHR0Wrapper>().Merge();
        }

        protected static void AppendAction(object sender, EventArgs e)
        {
            GetInstance<CHR0Wrapper>().Append();
        }

        protected static void ResizeAction(object sender, EventArgs e)
        {
            GetInstance<CHR0Wrapper>().Resize();
        }

        protected static void EditAllAction(object sender, EventArgs e)
        {
            EditAll(GetInstances<CHR0Wrapper>());
        }

        protected static void LoopAction(object sender, EventArgs e)
        {
            NumericInputForm entryCount = new NumericInputForm();
            if (entryCount.ShowDialog("Animation Looper", "Number of runthroughs:") == DialogResult.OK)
            {
                GetInstance<CHR0Wrapper>().Loop(entryCount.NewValue - 1);
            }
        }

        protected static void ReverseAction(object sender, EventArgs e)
        {
            GetInstance<CHR0Wrapper>().Reverse(false);
        }

        protected static void ReverseLoopAction(object sender, EventArgs e)
        {
            GetInstance<CHR0Wrapper>().Reverse(true);
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
            CHR0Wrapper w = GetInstance<CHR0Wrapper>();

            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
        }
        
        private static void MultiMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            DeleteSelectedToolStripMenuItem.Visible = true;
            DeleteSelectedToolStripMenuItem.Enabled = true;
        }

        private static void MultiMenuOpening(object sender, CancelEventArgs e)
        {
            foreach (TreeNode n in MainForm.Instance.resourceTree.SelectedNodes)
            {
                if (!(n is CHR0Wrapper w) || w._resource.Parent == null)
                {
                    DeleteSelectedToolStripMenuItem.Visible = false;
                    DeleteSelectedToolStripMenuItem.Enabled = false;
                    break;
                }
            }
        }

        #endregion

        public CHR0Wrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.CHR0Export;
        public override string ImportFilter => FileFilters.CHR0Import;

        public override ContextMenuStrip MultiSelectMenuStrip => MultiSelectMenu;

        public void NewBone()
        {
            CHR0EntryNode node = ((CHR0Node) _resource).CreateEntry();
            BaseWrapper res = FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void Merge()
        {
            ((CHR0Node) _resource).MergeWith();
            BaseWrapper res = FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void Resize()
        {
            ((CHR0Node) _resource).Resize();
            BaseWrapper res = FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void Append()
        {
            ((CHR0Node) _resource).Append();
            BaseWrapper res = FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void Loop(int timesToLoop)
        {
            ((CHR0Node) _resource).Append((CHR0Node) _resource, timesToLoop);
            BaseWrapper res = FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public static void EditAll(IEnumerable<BaseWrapper> wrappers)
        {
            EditAllDialog ctd = new EditAllDialog();
            ctd.ShowDialog(_owner, wrappers.Select(n => n.Resource));
        }

        public void Reverse(bool appendReverse)
        {
            ((CHR0Node) _resource).Reverse(appendReverse);
            BaseWrapper res = FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
    }

    [NodeWrapper(ResourceType.CHR0Entry)]
    public class CHR0EntryWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;
        private static readonly ContextMenuStrip MultiSelectMenu;

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

        private static readonly ToolStripMenuItem ExportSelectedToolStripMenuItem =
            new ToolStripMenuItem("&Export Selected", null, ExportSelectedAction, Keys.Control | Keys.E);

        private static readonly ToolStripMenuItem DeleteSelectedToolStripMenuItem =
            new ToolStripMenuItem("&Delete Selected", null, DeleteSelectedAction, Keys.Control | Keys.Delete);

        static CHR0EntryWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("View Interpolation", null, ViewInterpolation, Keys.Control | Keys.T));
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

            MultiSelectMenu = new ContextMenuStrip();
            MultiSelectMenu.Items.Add(new ToolStripMenuItem("Edit All", null, EditAllAction));
            MultiSelectMenu.Items.Add(new ToolStripSeparator());
            MultiSelectMenu.Items.Add(ExportSelectedToolStripMenuItem);
            MultiSelectMenu.Items.Add(DeleteSelectedToolStripMenuItem);
            MultiSelectMenu.Opening += MultiMenuOpening;
            MultiSelectMenu.Closing += MultiMenuClosing;
        }

        protected static void EditAllAction(object sender, EventArgs e)
        {
            EditAll(GetInstances<CHR0EntryWrapper>());
        }

        protected static void ViewInterpolation(object sender, EventArgs e)
        {
            GetInstance<CHR0EntryWrapper>().ViewInterpolation();
        }

        private void ViewInterpolation()
        {
            InterpolationForm f = MainForm.Instance.InterpolationForm;
            InterpolationEditor e = f?._interpolationEditor;
            e?.SetTarget(_resource as IKeyframeSource);
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
            CHR0EntryWrapper w = GetInstance<CHR0EntryWrapper>();

            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
        }
        
        private static void MultiMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            DeleteSelectedToolStripMenuItem.Visible = true;
            DeleteSelectedToolStripMenuItem.Enabled = true;
        }

        private static void MultiMenuOpening(object sender, CancelEventArgs e)
        {
            foreach (TreeNode n in MainForm.Instance.resourceTree.SelectedNodes)
            {
                if (!(n is CHR0EntryWrapper w) || w._resource.Parent == null)
                {
                    DeleteSelectedToolStripMenuItem.Visible = false;
                    DeleteSelectedToolStripMenuItem.Enabled = false;
                    break;
                }
            }
        }

        #endregion

        public CHR0EntryWrapper(IWin32Window owner)
        {
            _owner = owner;
            ContextMenuStrip = _menu;
        }

        public CHR0EntryWrapper()
        {
            _owner = null;
            ContextMenuStrip = _menu;
        }

        public override ContextMenuStrip MultiSelectMenuStrip => MultiSelectMenu;

        private static void EditAll(IEnumerable<BaseWrapper> wrappers)
        {
            EditAllDialog ctd = new EditAllDialog();
            ctd.ShowDialog(_owner, wrappers.Select(n => n.Resource));
        }
    }
}