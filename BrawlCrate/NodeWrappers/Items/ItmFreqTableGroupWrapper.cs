using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers.Items
{
    [NodeWrapper(ResourceType.ItemFreqTableGroupNode)]
    internal class ItmFreqTableGroupWrapper : GenericWrapper
    {
        private static readonly ContextMenuStrip _menu;

        private static readonly ToolStripMenuItem DuplicateToolStripMenuItem =
            new ToolStripMenuItem("&Duplicate", null, DuplicateAction, Keys.Control | Keys.D);

        private static readonly ToolStripMenuItem ReplaceToolStripMenuItem =
            new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R);

        static ItmFreqTableGroupWrapper()
        {
            _menu = new ContextMenuStrip();
            //_menu.Items.Add(new ToolStripMenuItem("&New Entry", null, NewEntryAction, Keys.Control | Keys.H));
            //_menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(DuplicateToolStripMenuItem);
            _menu.Items.Add(ReplaceToolStripMenuItem);
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        private static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<ItmFreqTableGroupWrapper>().NewEntry();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
        }

        public ItmFreqTableGroupWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public ItmFreqEntryNode NewEntry()
        {
            ItmFreqEntryNode node = new ItmFreqEntryNode();
            node.UpdateName();
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
    }
}