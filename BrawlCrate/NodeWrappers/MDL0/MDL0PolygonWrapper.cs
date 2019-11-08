using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.MDL0Object)]
    public class MDL0PolygonWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        private static readonly ToolStripMenuItem DuplicateToolStripMenuItem =
            new ToolStripMenuItem("&Duplicate", null, DuplicateAction, Keys.Control | Keys.D);

        private static readonly ToolStripMenuItem ReplaceToolStripMenuItem =
            new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R);

        private static readonly ToolStripMenuItem RestoreToolStripMenuItem =
            new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T);

        private static readonly ToolStripMenuItem MoveUpToolStripMenuItem =
            new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up);

        private static readonly ToolStripMenuItem MoveDownToolStripMenuItem =
            new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down);

        private static readonly ToolStripMenuItem DeleteToolStripMenuItem =
            new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete);

        static MDL0PolygonWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("O&ptimize Mesh", null, OptimizeAction, Keys.Control | Keys.P));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(DuplicateToolStripMenuItem);
            _menu.Items.Add(ReplaceToolStripMenuItem);
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(MoveUpToolStripMenuItem);
            _menu.Items.Add(MoveDownToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(DeleteToolStripMenuItem);
        }

        protected static void OptimizeAction(object sender, EventArgs e)
        {
            GetInstance<MDL0PolygonWrapper>().Optimize();
        }

        #endregion

        public override string ExportFilter => FileFilters.Object;
        public override string ImportFilter => FileFilters.Raw;

        public MDL0PolygonWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override void Duplicate()
        {
            MDL0ObjectNode node = ((MDL0ObjectNode) _resource).HardCopy();
            node.Name += " - Copy";
            ((MDL0ObjectNode) _resource).Model._objGroup.AddChild(node);
            //((MDL0ObjectNode)_resource).Model.Rebuild(true);
        }

        public void Optimize()
        {
            using (ObjectOptimizerForm opt = new ObjectOptimizerForm())
            {
                opt.ShowDialog((MDL0ObjectNode) _resource);
            }
        }
    }
}