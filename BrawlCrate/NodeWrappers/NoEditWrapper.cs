using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.NoEditEntry)]
    public class NoEditEntryWrapper : GenericWrapper
    {
        private static readonly ContextMenuStrip _menu;

        static NoEditEntryWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            //_menu.Items.Add(replaceToolStripMenuItem = new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        public NoEditEntryWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }

    [NodeWrapper(ResourceType.NoEditFolder)]
    public class NoEditFolderWrapper : GenericWrapper
    {
        private static readonly ContextMenuStrip _menu;

        static NoEditFolderWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            //_menu.Items.Add(replaceToolStripMenuItem = new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        public NoEditFolderWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }
}