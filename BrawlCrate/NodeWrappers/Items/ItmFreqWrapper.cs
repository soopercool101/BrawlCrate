using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.ItemFreqNode)]
    internal class ItemFreqWrapper : GenericWrapper
    {
        private static readonly ContextMenuStrip _menu;

        private static readonly ToolStripMenuItem DuplicateToolStripMenuItem =
            new ToolStripMenuItem("&Duplicate", null, DuplicateAction, Keys.Control | Keys.D);

        private static readonly ToolStripMenuItem ReplaceToolStripMenuItem =
            new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R);

        static ItemFreqWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(DuplicateToolStripMenuItem);
            _menu.Items.Add(ReplaceToolStripMenuItem);
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        private static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<ItemFreqWrapper>().NewEntry();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
        }

        public ItemFreqWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void NewEntry()
        {
            //((ItmFreqNode)_resource).CreateEntry();
        }
    }
}