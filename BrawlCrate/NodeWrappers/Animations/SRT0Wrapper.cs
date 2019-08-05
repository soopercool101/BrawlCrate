using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.SRT0)]
    public class SRT0Wrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        static SRT0Wrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&New Entry", null, NewEntryAction, Keys.Control | Keys.H));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Edit", null,
                //new ToolStripMenuItem("&Merge Animation", null, MergeAction),
                new ToolStripMenuItem("&Append Animation", null, AppendAction),
                new ToolStripMenuItem("Res&ize", null, ResizeAction)));
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

        protected static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<SRT0Wrapper>().NewEntry();
        }

        protected static void MergeAction(object sender, EventArgs e)
        {
            GetInstance<SRT0Wrapper>().Merge();
        }

        protected static void AppendAction(object sender, EventArgs e)
        {
            GetInstance<SRT0Wrapper>().Append();
        }

        protected static void ResizeAction(object sender, EventArgs e)
        {
            GetInstance<SRT0Wrapper>().Resize();
        }

        #endregion

        public SRT0Wrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.SRT0;

        private void NewEntry()
        {
            ((SRT0Node) _resource).CreateEntry();
        }

        private void Merge()
        {
        }

        private void Append()
        {
            ((SRT0Node) _resource).Append();
            BaseWrapper res = FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        private void Resize()
        {
            ((SRT0Node) _resource).Resize();
            BaseWrapper res = FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
    }

    [NodeWrapper(ResourceType.SRT0Entry)]
    public class SRT0EntryWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        private static ToolStripMenuItem _newEntryToolStripMenuItem;
        static SRT0EntryWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(_newEntryToolStripMenuItem = new ToolStripMenuItem("&New Entry", null, NewEntryAction, Keys.Control | Keys.H));
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

        protected static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<SRT0EntryWrapper>().NewEntry();
        }

        protected new static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _newEntryToolStripMenuItem.Enabled = true;
            replaceToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
            restoreToolStripMenuItem.Enabled = true;
            moveUpToolStripMenuItem.Enabled = true;
            moveDownToolStripMenuItem.Enabled =true;
        }

        protected new static void MenuOpening(object sender, CancelEventArgs e)
        {
            SRT0EntryWrapper w = GetInstance<SRT0EntryWrapper>();
            _newEntryToolStripMenuItem.Enabled = w._resource.Children.Count < 11;
            replaceToolStripMenuItem.Enabled = w.Parent != null;
            deleteToolStripMenuItem.Enabled = w.Parent != null;
            restoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            moveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            moveDownToolStripMenuItem.Enabled = w.NextNode != null;
        }

        #endregion

        public SRT0EntryWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void NewEntry()
        {
            SRT0EntryNode n = (SRT0EntryNode) _resource;
            n.CreateEntry();
        }
    }

    [NodeWrapper(ResourceType.SRT0Texture)]
    public class SRT0TextureWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        static SRT0TextureWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("View Interpolation", null, ViewInterp, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(replaceToolStripMenuItem = new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(restoreToolStripMenuItem = new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void ViewInterp(object sender, EventArgs e)
        {
            GetInstance<SRT0TextureWrapper>().ViewInterp();
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

        public SRT0TextureWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }
}