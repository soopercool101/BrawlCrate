using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.ResourceNodes.Subspace.SSEEX;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.SELB)]
    public class SELBWrapper : GenericWrapper
    {
        private static readonly ContextMenuStrip _menu;

        private static readonly ToolStripMenuItem AddEntryToolStripMenuItem =
            new ToolStripMenuItem("&New Entry", null, NewEntryAction, Keys.Control | Keys.J);

        private static readonly ToolStripMenuItem ReplaceToolStripMenuItem =
            new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R);

        public static readonly int MaxEntries = 5;

        private bool CanAddEntries()
        {
            return Resource.Children.Count < MaxEntries;
        }

        static SELBWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(AddEntryToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(ReplaceToolStripMenuItem);
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            SELBWrapper w = GetInstance<SELBWrapper>();

            AddEntryToolStripMenuItem.Enabled = w.CanAddEntries();
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            AddEntryToolStripMenuItem.Enabled = true;
            ReplaceToolStripMenuItem.Enabled = true;
        }

        private static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<SELBWrapper>().NewEntry();
        }

        public SELBWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void NewEntry()
        {
            if (!CanAddEntries())
            {
                return;
            }
            SELBEntryNode node = new SELBEntryNode();
            node.CSSID = 0;
            _resource.AddChild(node);
        }
    }
}