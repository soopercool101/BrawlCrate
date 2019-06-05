using System;
using System.ComponentModel;
using System.Windows.Forms;
using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.SHP0)]
    public class SHP0Wrapper : GenericWrapper
    {
        public SHP0Wrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.SHP0;

        public void NewEntry()
        {
            var node = ((SHP0Node) _resource).FindOrCreateEntry(_resource.FindName(null));
            var res = FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        private void Merge()
        {
        }

        private void Append()
        {
            ((SHP0Node) _resource).Append();
            var res = FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        private void Resize()
        {
            ((SHP0Node) _resource).Resize();
            var res = FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        #region Menu

        private static readonly ContextMenuStrip _menu;

        static SHP0Wrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Ne&w Entry", null, NewEntryAction, Keys.Control | Keys.H));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Edit", null,
                //new ToolStripMenuItem("&Merge Animation", null, MergeAction),
                new ToolStripMenuItem("&Append Animation", null, AppendAction),
                new ToolStripMenuItem("Res&ize", null, ResizeAction)));
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
        }

        protected static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<SHP0Wrapper>().NewEntry();
        }

        protected static void MergeAction(object sender, EventArgs e)
        {
            GetInstance<SHP0Wrapper>().Merge();
        }

        protected static void AppendAction(object sender, EventArgs e)
        {
            GetInstance<SHP0Wrapper>().Append();
        }

        protected static void ResizeAction(object sender, EventArgs e)
        {
            GetInstance<SHP0Wrapper>().Resize();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[5].Enabled = _menu.Items[6].Enabled =
                _menu.Items[8].Enabled = _menu.Items[9].Enabled = _menu.Items[12].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<SHP0Wrapper>();
            _menu.Items[5].Enabled = _menu.Items[12].Enabled = w.Parent != null;
            _menu.Items[6].Enabled = w._resource.IsDirty || w._resource.IsBranch;
            _menu.Items[8].Enabled = w.PrevNode != null;
            _menu.Items[9].Enabled = w.NextNode != null;
        }

        #endregion
    }

    [NodeWrapper(ResourceType.SHP0Entry)]
    public class SHP0EntryWrapper : GenericWrapper
    {
        public SHP0EntryWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void NewEntry()
        {
            var n = (SHP0EntryNode) _resource;
            n.CreateEntry();
        }

        #region Menu

        private static readonly ContextMenuStrip _menu;

        static SHP0EntryWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&New Entry", null, NewEntryAction, Keys.Control | Keys.H));
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
        }

        protected static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<SHP0EntryWrapper>().NewEntry();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[3].Enabled = _menu.Items[4].Enabled =
                _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[10].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<SHP0EntryWrapper>();
            _menu.Items[3].Enabled = _menu.Items[10].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = w._resource.IsDirty || w._resource.IsBranch;
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }

        #endregion
    }

    [NodeWrapper(ResourceType.SHP0VertexSet)]
    public class SHP0VertexSetNodeWrapper : GenericWrapper
    {
        public SHP0VertexSetNodeWrapper(IWin32Window owner)
        {
            _owner = owner;
            ContextMenuStrip = _menu;
        }

        public SHP0VertexSetNodeWrapper()
        {
            _owner = null;
            ContextMenuStrip = _menu;
        }

        #region Menu

        private static readonly ContextMenuStrip _menu;

        static SHP0VertexSetNodeWrapper()
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
        }

        protected static void ViewInterp(object sender, EventArgs e)
        {
            GetInstance<SHP0VertexSetNodeWrapper>().ViewInterp();
        }

        private void ViewInterp()
        {
            var f = MainForm.Instance.InterpolationForm;
            if (f != null)
            {
                var e = f._interpolationEditor;
                if (e != null) e.SetTarget(_resource as IKeyframeSource);
            }
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[3].Enabled = _menu.Items[4].Enabled =
                _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[10].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<SHP0VertexSetNodeWrapper>();
            _menu.Items[3].Enabled = _menu.Items[10].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = w._resource.IsDirty || w._resource.IsBranch;
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }

        #endregion
    }
}