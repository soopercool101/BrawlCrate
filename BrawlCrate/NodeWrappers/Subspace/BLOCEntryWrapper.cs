using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.BLOCEntry)]
    [NodeWrapper(ResourceType.GDOR)]
    [NodeWrapper(ResourceType.GDBF)]
    public class BLOCEntryWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        static BLOCEntryWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Add New Entry", null, NewEntryAction, Keys.Control | Keys.J));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<BLOCEntryWrapper>().NewEntry();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[0].Enabled = _menu.Items[0].Visible = _menu.Items[1].Visible =
                _menu.Items[3].Enabled = _menu.Items[4].Enabled = _menu.Items[6].Enabled =
                _menu.Items[7].Enabled = _menu.Items[10].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            BLOCEntryWrapper w = GetInstance<BLOCEntryWrapper>();
            _menu.Items[0].Enabled = _menu.Items[0].Visible = _menu.Items[1].Visible = (w._resource as BLOCEntryNode)?.SubEntryType != typeof(RawDataNode);
            _menu.Items[3].Enabled = _menu.Items[10].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = w._resource.IsDirty || w._resource.IsBranch;
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }

        #endregion

        public void NewEntry()
        {
            ResourceNode node = Activator.CreateInstance((_resource as BLOCEntryNode)?.SubEntryType) as ResourceNode;
            if(node != null)
            {
                if (node._name == null)
                {
                    node.Name = "Entry [" + _resource.Children.Count + "]";
                }
                _resource.AddChild(node);
            }
        }

        public BLOCEntryWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }
}