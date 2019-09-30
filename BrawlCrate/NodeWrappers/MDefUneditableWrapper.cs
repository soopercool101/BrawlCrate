using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using BrawlLib;
using System.ComponentModel;
using BrawlLib.IO;
using BrawlLib.SSBBTypes;
using BrawlCrate.NodeWrappers;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.NoEdit)]
    internal class MDefUneditableWrapper : GenericWrapper
    {
        private static ContextMenuStrip _menu;

        static MDefUneditableWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            //_menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
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