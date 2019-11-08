using BrawlCrate.UI;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System.Collections.Generic;

namespace BrawlCrate.NodeWrappers
{
    public class StageTableWrapper : GenericWrapper
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

        static StageTableWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Resize", null, ResizeAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(DuplicateToolStripMenuItem);
            _menu.Items.Add(ReplaceToolStripMenuItem);
            _menu.Items.Add(RestoreToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(MoveUpToolStripMenuItem);
            _menu.Items.Add(MoveDownToolStripMenuItem);
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(DeleteToolStripMenuItem);
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void ResizeAction(object sender, EventArgs e)
        {
            GetInstance<STDTWrapper>().Resize();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            DuplicateToolStripMenuItem.Enabled = true;
            ReplaceToolStripMenuItem.Enabled = true;
            RestoreToolStripMenuItem.Enabled = true;
            MoveUpToolStripMenuItem.Enabled = true;
            MoveDownToolStripMenuItem.Enabled = true;
            DeleteToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            StageTableWrapper w = GetInstance<StageTableWrapper>();

            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
        }

        #endregion

        public StageTableWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void Resize()
        {
            NumericInputForm n = new NumericInputForm();
            if (_resource is StageTableNode t &&
                n.ShowDialog("Resize", "Enter the new size:", t.NumEntries) == DialogResult.OK)
            {
                List<byte> newList = new List<byte>();
                for (int i = 0; i < n.NewValue * t.NumEntries; i++)
                {
                    newList.Add(i < t.EntryList.Count ? t.EntryList[i] : (byte) 0);
                }

                t.EntryList = newList;
                MainForm.Instance.resourceTree_SelectionChanged(this, null);
            }
        }
    }

    [NodeWrapper(ResourceType.STDT)]
    public class STDTWrapper : StageTableWrapper
    {
        public override string ExportFilter => FileFilters.STDT;
    }

    [NodeWrapper(ResourceType.TBCL)]
    public class TBCLWrapper : StageTableWrapper
    {
        public override string ExportFilter => FileFilters.TBCL;
    }

    [NodeWrapper(ResourceType.TBGC)]
    public class TBGCWrapper : StageTableWrapper
    {
        public override string ExportFilter => FileFilters.TBGC;
    }

    [NodeWrapper(ResourceType.TBGD)]
    public class TBGDWrapper : StageTableWrapper
    {
        public override string ExportFilter => FileFilters.TBGD;
    }

    [NodeWrapper(ResourceType.TBGM)]
    public class TBGMWrapper : StageTableWrapper
    {
        public override string ExportFilter => FileFilters.TBGM;
    }

    [NodeWrapper(ResourceType.TBLV)]
    public class TBLVWrapper : StageTableWrapper
    {
        public override string ExportFilter => FileFilters.TBLV;
    }

    [NodeWrapper(ResourceType.TBRM)]
    public class TBRMWrapper : StageTableWrapper
    {
        public override string ExportFilter => FileFilters.TBRM;
    }

    [NodeWrapper(ResourceType.TBST)]
    public class TBSTWrapper : StageTableWrapper
    {
        public override string ExportFilter => FileFilters.TBST;
    }
}