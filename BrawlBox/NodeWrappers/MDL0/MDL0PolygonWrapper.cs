using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using BrawlLib;
using System.ComponentModel;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.MDL0Object)]
    public class MDL0PolygonWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static MDL0PolygonWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("O&ptimize Mesh", null, OptimizeAction, Keys.Control | Keys.P));
            _menu.Items.Add(new ToolStripMenuItem("&Duplicate", null, DuplicateAction, Keys.Control | Keys.D));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[7].Enabled = _menu.Items[8].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDL0PolygonWrapper w = GetInstance<MDL0PolygonWrapper>();
            _menu.Items[7].Enabled = w.PrevNode != null;
            _menu.Items[8].Enabled = w.NextNode != null;
        }

        protected static void OptimizeAction(object sender, EventArgs e) { GetInstance<MDL0PolygonWrapper>().Optimize(); }
        protected static void DuplicateAction(object sender, EventArgs e) { GetInstance<MDL0PolygonWrapper>().Duplicate(); }
        #endregion

        public override string ExportFilter { get { return FileFilters.Object; } }
        public override string ImportFilter { get { return FileFilters.Raw; } }

        public MDL0PolygonWrapper() { ContextMenuStrip = _menu; }

        public void Duplicate()
        {
            MDL0ObjectNode node = ((MDL0ObjectNode)_resource).HardCopy();
            node.Name += " - Copy";
            ((MDL0ObjectNode)_resource).Model._objGroup.AddChild(node);
            //((MDL0ObjectNode)_resource).Model.Rebuild(true);
        }

        public void Optimize()
        {
            new ObjectOptimizerForm().ShowDialog((MDL0ObjectNode)_resource);
        }
    }
}
