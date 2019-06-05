using System;
using System.ComponentModel;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.ClassicStageTbl)]
    public class ClassicStageTblWrapper : GenericWrapper
    {
        private static readonly ContextMenuStrip _menu;

        static ClassicStageTblWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&New Entry", null, NewEntryAction, Keys.Control | Keys.H));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            //_menu.Items.Add(new ToolStripSeparator());
            //_menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            //_menu.Items.Add(new ToolStripMenuItem("Move &Down", null, MoveDownAction, Keys.Control | Keys.Down));
            //_menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            //_menu.Items.Add(new ToolStripSeparator());
            //_menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        public ClassicStageTblWrapper()
        {
            ContextMenuStrip = _menu;
        }

        private static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<ClassicStageTblWrapper>().NewEntry();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
        }

        public void NewEntry()
        {
            ((ClassicStageTblNode) _resource).CreateEntry();
        }
    }
}