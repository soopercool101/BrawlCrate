using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.ResourceNodes.Subspace.SSEEX;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.SELCTeam)]
    public class SELCTeamWrapper : GenericWrapper
    {
        private static readonly ContextMenuStrip _menu;

        private static readonly ToolStripMenuItem AddEntryToolStripMenuItem =
            new ToolStripMenuItem("&New Entry", null, NewEntryAction, Keys.Control | Keys.J);

        private static readonly ToolStripMenuItem ReplaceToolStripMenuItem =
            new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R);

        public static readonly int MaxEntries = 163;

        private bool CanAddEntries()
        {
            return Parent is BaseWrapper b && b.Resource is SELCNode s && s.Entries < MaxEntries;
        }

        static SELCTeamWrapper()
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
            SELCTeamWrapper w = GetInstance<SELCTeamWrapper>();

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
            GetInstance<SELCTeamWrapper>().NewEntry();
        }

        public SELCTeamWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void NewEntry()
        {
            if (!CanAddEntries())
            {
                return;
            }
            SELCEntryNode node = new SELCEntryNode();
            node.CSSID = 0;
            _resource.AddChild(node);
        }
    }
}