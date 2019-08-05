using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.CollisionDef)]
    public class CollisionWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        static CollisionWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Preview", null, EditAction, Keys.Control | Keys.P));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(replaceToolStripMenuItem = new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(restoreToolStripMenuItem = new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(moveUpToolStripMenuItem = new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(moveDownToolStripMenuItem = new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void EditAction(object sender, EventArgs e)
        {
            GetInstance<CollisionWrapper>().Preview();
        }

        #endregion

        public override string ExportFilter => FileFilters.CollisionDef;

        public CollisionWrapper()
        {
            ContextMenuStrip = _menu;
        }

        private void Preview()
        {
            using (CollisionForm frm = new CollisionForm())
            {
                frm.ShowDialog(null, _resource as CollisionNode);
            }
        }
    }
}