using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    public abstract class TBWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        private static readonly ToolStripMenuItem AddEntryToolStripMenuItem =
            new ToolStripMenuItem("Add New Entry", null, NewEntryAction, Keys.Control | Keys.J);

        private static readonly ToolStripSeparator AddEntryToolStripSeparator =
            new ToolStripSeparator();

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

        static TBWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(AddEntryToolStripMenuItem);
            _menu.Items.Add(AddEntryToolStripSeparator);
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

        protected static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<TBWrapper>().NewEntry();
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
            TBWrapper w = GetInstance<TBWrapper>();

            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
        }

        #endregion

        public virtual void NewEntry()
        {
            // Redirect to NewEntry<T> for children
        }

        public void NewEntry<T>() where T : ResourceNode, new()
        {
            if (typeof(T) != typeof(RawDataNode))
            {
                T node = new T { Name = "NewEntry" };
                _resource.AddChild(node);
            }
        }

        public TBWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }


    [NodeWrapper(ResourceType.TBCL)]
    public class TBCLWrapper : TBWrapper
    {
        public override string ExportFilter => FileFilters.TBCL;

        public override void NewEntry()
        {
            NewEntry<TBCLEntryNode>();
        }
    }

    [NodeWrapper(ResourceType.TBGC)]
    public class TBGCWrapper : TBWrapper
    {
        public override string ExportFilter => FileFilters.TBGC;
    }

    [NodeWrapper(ResourceType.TBGD)]
    public class TBGDWrapper : TBWrapper
    {
        public override string ExportFilter => FileFilters.TBGD;

        public override void NewEntry()
        {
            NewEntry<TBGDEntryNode>();
        }
    }

    [NodeWrapper(ResourceType.TBGM)]
    public class TBGMWrapper : TBWrapper
    {
        public override string ExportFilter => FileFilters.TBGM;

        public override void NewEntry()
        {
            NewEntry<TBGMEntryNode>();
        }
    }

    [NodeWrapper(ResourceType.TBLV)]
    public class TBLVWrapper : TBWrapper
    {
        public override string ExportFilter => FileFilters.TBLV;

        public override void NewEntry()
        {
            NewEntry<TBLVEntryNode>();
        }
    }

    [NodeWrapper(ResourceType.TBRM)]
    public class TBRMWrapper : TBWrapper
    {
        public override string ExportFilter => FileFilters.TBRM;
    }

    [NodeWrapper(ResourceType.TBST)]
    public class TBSTWrapper : TBWrapper
    {
        public override string ExportFilter => FileFilters.TBST;
    }
}
