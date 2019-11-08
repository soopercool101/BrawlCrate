using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.NoEdit)]
    internal class MDefUneditableWrapper : GenericWrapper
    {
        private static ContextMenuStrip _menu;

        static MDefUneditableWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
        }

        public MDefUneditableWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }
}