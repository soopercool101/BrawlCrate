using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.CLR0)]
    public class CLR0Wrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        static CLR0Wrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("New Material Entry", null, NewCLRAction));
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

        protected static void NewCLRAction(object sender, EventArgs e)
        {
            GetInstance<CLR0Wrapper>().NewCLR();
        }

        #endregion

        public override string ExportFilter => FileFilters.CLR0;

        public CLR0Wrapper()
        {
            ContextMenuStrip = _menu;
        }

        private void NewCLR()
        {
            CLR0MaterialEntryNode n = ((CLR0Node) _resource).CreateEntry();
            if (n != null)
            {
                BaseWrapper b = FindResource(n, true);
                if (b != null)
                {
                    b.EnsureVisible();
                }
            }
        }
    }

    [NodeWrapper(ResourceType.CLR0Material)]
    public class CLR0MaterialWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        static CLR0MaterialWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("New Color Sequence", null, NewCLRAction));
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

        protected static void NewCLRAction(object sender, EventArgs e)
        {
            GetInstance<CLR0MaterialWrapper>().NewCLR();
        }

        #endregion

        public CLR0MaterialWrapper()
        {
            ContextMenuStrip = _menu;
        }

        private void NewCLR()
        {
            CLR0MaterialEntryNode n = ((CLR0MaterialNode) _resource).CreateEntry();
            if (n != null)
            {
                BaseWrapper b = FindResource(n, true);
                if (b != null)
                {
                    b.EnsureVisible();
                }
            }
        }
    }

    [NodeWrapper(ResourceType.CLR0MaterialEntry)]
    public class CLR0MaterialEntryWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        static CLR0MaterialEntryWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(replaceToolStripMenuItem = new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(restoreToolStripMenuItem = new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(moveUpToolStripMenuItem = new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(moveDownToolStripMenuItem = new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        #endregion

        public CLR0MaterialEntryWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }
}