using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.SndBgmTitleDataFolder)]
    public class SndBgmTitleDataFolderWrapper : GenericWrapper
    {
        private static ContextMenuStrip _menu;
        static SndBgmTitleDataFolderWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&New Entry", null, NewEntryAction, Keys.Control | Keys.H));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        private static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<SndBgmTitleDataFolderWrapper>().NewEntry();
        }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {

        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {

        }
        public SndBgmTitleDataFolderWrapper() { ContextMenuStrip = _menu; }
        public void NewEntry()
        {
            ((SndBgmTitleDataNode)_resource).CreateEntry();
        }
    }
}
