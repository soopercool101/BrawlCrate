using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSTCGroup)]
    internal class RSTCGroupWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        static RSTCGroupWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Add New Entry", null, NewEntryAction, Keys.Control | Keys.J));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Clear List", null, ClearAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<RSTCGroupWrapper>().NewEntry();
        }

        protected static void ClearAction(object sender, EventArgs e)
        {
            GetInstance<RSTCGroupWrapper>().Clear();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[0].Enabled = _menu.Items[2].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            RSTCGroupWrapper w = GetInstance<RSTCGroupWrapper>();
            _menu.Items[0].Enabled = w._resource.Children.Count < 100;
            _menu.Items[2].Enabled = w._resource.HasChildren;
        }

        #endregion

        //public override string ExportFilter { get { return FileFilters.RSTCGroup; } }

        public void NewEntry()
        {
            if (_resource.Children.Count >= 100)
            {
                return;
            }

            RSTCEntryNode node = new RSTCEntryNode
            {
                FighterID = 0x0,
                _name = "Mario"
            };
            _resource.AddChild(node);
            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
        }

        public void Clear()
        {
            while (_resource.HasChildren)
            {
                _resource.RemoveChild(_resource.Children[0]);
            }

            BaseWrapper w = FindResource(_resource, false);
            w.EnsureVisible();
            w.Expand();
        }

        public RSTCGroupWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }
}