using BrawlLib;
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
    public class CHR0Wrapper : GenericWrapper, MultiSelectableWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu, _multiSelectMenu;

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
            _menu.Items.Add(replaceToolStripMenuItem = new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(restoreToolStripMenuItem = new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(moveUpToolStripMenuItem = new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move &Down", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;

            _multiSelectMenu = new ContextMenuStrip();
            _multiSelectMenu.Items.Add(new ToolStripMenuItem("Edit All", null, EditAllAction));
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

        #endregion

        public CHR0Wrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.CHR0Export;
        public override string ImportFilter => FileFilters.CHR0Import;

        public ContextMenuStrip MultiSelectMenuStrip => _multiSelectMenu;

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

        private static readonly ContextMenuStrip _menu, _multiSelectMenu;

        static CHR0EntryWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("View Interpolation", null, ViewInterp, Keys.Control | Keys.T));
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

            _multiSelectMenu = new ContextMenuStrip();
            _multiSelectMenu.Items.Add(new ToolStripMenuItem("Edit All", null, EditAllAction));
        }

        protected static void EditAllAction(object sender, EventArgs e)
        {
            EditAll(GetInstances<CHR0EntryWrapper>());
        }

        protected static void ViewInterp(object sender, EventArgs e)
        {
            GetInstance<CHR0EntryWrapper>().ViewInterp();
        }

        private void ViewInterp()
        {
            InterpolationForm f = MainForm.Instance.InterpolationForm;
            if (f != null)
            {
                InterpolationEditor e = f._interpolationEditor;
                if (e != null)
                {
                    e.SetTarget(_resource as IKeyframeSource);
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

        public ContextMenuStrip MultiSelectMenuStrip => _multiSelectMenu;

        private static void EditAll(IEnumerable<BaseWrapper> wrappers)
        {
            EditAllDialog ctd = new EditAllDialog();
            ctd.ShowDialog(_owner, wrappers.Select(n => n.Resource));
        }
    }
}