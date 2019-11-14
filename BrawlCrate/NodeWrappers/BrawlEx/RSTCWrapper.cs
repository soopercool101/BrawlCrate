using BrawlLib.CustomLists;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSTC)]
    internal class RSTCWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        private static readonly ToolStripMenuItem _newEntryToolStripMenuItem;
        private static readonly ToolStripMenuItem _clearListToolStripMenuItem;

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

        static RSTCWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(_newEntryToolStripMenuItem = new ToolStripMenuItem("Add New Entry (Both Lists)", null,
                NewEntryAction,
                Keys.Control | Keys.J));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Sync Lists", null,
                new ToolStripMenuItem("From CSS List to Random List", null, SyncRandomAction),
                new ToolStripMenuItem("From Random List to CSS List", null, SyncCSSAction)));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(DuplicateToolStripMenuItem);
            _menu.Items.Add(ReplaceToolStripMenuItem);
            _menu.Items.Add(RestoreToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_clearListToolStripMenuItem =
                new ToolStripMenuItem("Clear Lists", null, ClearAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void NewEntryAction(object sender, EventArgs e)
        {
            RSTCWrapper w = GetInstance<RSTCWrapper>();
            ResourceNode r = w._resource;
            if (((RSTCNode) r).cssList.Children.Count >= 256 || ((RSTCNode) r).randList.Children.Count >= 256)
            {
                return;
            }

            HexEntryBox entryID = new HexEntryBox();
            if (entryID.ShowDialog("New Fighter", "CSS Slot ID:", 2) == DialogResult.OK)
            {
                if (entryID.NewValue != -1)
                {
                    GetInstance<RSTCWrapper>().NewEntry((byte) entryID.NewValue);
                }
            }
        }

        protected static void ClearAction(object sender, EventArgs e)
        {
            GetInstance<RSTCWrapper>().Clear();
        }

        protected static void SyncRandomAction(object sender, EventArgs e)
        {
            GetInstance<RSTCWrapper>().SyncRandom();
        }

        protected static void SyncCSSAction(object sender, EventArgs e)
        {
            GetInstance<RSTCWrapper>().SyncCSS();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _newEntryToolStripMenuItem.Enabled = true;
            DuplicateToolStripMenuItem.Enabled = true;
            ReplaceToolStripMenuItem.Enabled = true;
            DeleteToolStripMenuItem.Enabled = true;
            RestoreToolStripMenuItem.Enabled = true;
            _clearListToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            RSTCWrapper w = GetInstance<RSTCWrapper>();
            ResourceNode r = w._resource;
            _newEntryToolStripMenuItem.Enabled = ((RSTCNode) r).cssList.Children.Count < 256 &&
                                                 ((RSTCNode) r).randList.Children.Count < 256;
            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            _clearListToolStripMenuItem.Enabled =
                ((RSTCNode) r).cssList.Children.Count > 0 || ((RSTCNode) r).randList.Children.Count > 0;
        }

        #endregion

        public override string ExportFilter => FileFilters.RSTC;

        public void NewEntry(byte cssID)
        {
            if (((RSTCNode) _resource).cssList.entries >= 256 ||
                ((RSTCNode) _resource).randList.entries >= 256)
            {
                return;
            }

            RSTCEntryNode node1 = new RSTCEntryNode
            {
                FighterID = cssID,
                _name = FighterNameGenerators.FromID(cssID,
                    FighterNameGenerators.cssSlotIDIndex, "+S")
            };
            ((RSTCNode) _resource).cssList.AddChild(node1);
            RSTCEntryNode node2 = new RSTCEntryNode
            {
                FighterID = cssID,
                _name = FighterNameGenerators.FromID(cssID,
                    FighterNameGenerators.cssSlotIDIndex, "+S")
            };
            ((RSTCNode) _resource).randList.AddChild(node2);
        }

        public void Clear()
        {
            while (((RSTCNode) _resource).cssList.HasChildren)
            {
                ((RSTCNode) _resource).cssList.RemoveChild(((RSTCNode) _resource).cssList.Children[0]);
            }

            while (((RSTCNode) _resource).randList.HasChildren)
            {
                ((RSTCNode) _resource).randList.RemoveChild(((RSTCNode) _resource).randList.Children[0]);
            }

            BaseWrapper w = FindResource(((RSTCNode) _resource).cssList, false);
            w.EnsureVisible();
            w.Expand();
            w = FindResource(((RSTCNode) _resource).randList, false);
            w.EnsureVisible();
            w.Expand();
        }

        public void SyncRandom()
        {
            while (((RSTCNode) _resource).randList.HasChildren)
            {
                ((RSTCNode) _resource).randList.RemoveChild(((RSTCNode) _resource).randList.Children[0]);
            }

            foreach (ResourceNode r in ((RSTCNode) _resource).cssList.Children)
            {
                // Disallow syncing of 0x28 (None) and 0x29 (Random)
                if (((RSTCEntryNode) r).FighterID != 0x29 && ((RSTCEntryNode) r).FighterID != 0x28)
                {
                    RSTCEntryNode temp = new RSTCEntryNode
                    {
                        FighterID = ((RSTCEntryNode) r).FighterID,
                        Name = r.Name
                    };
                    ((RSTCNode) _resource).randList.AddChild(temp);
                }
            }
        }

        public void SyncCSS()
        {
            while (((RSTCNode) _resource).cssList.HasChildren)
            {
                ((RSTCNode) _resource).cssList.RemoveChild(((RSTCNode) _resource).cssList.Children[0]);
            }

            foreach (ResourceNode r in ((RSTCNode) _resource).randList.Children)
            {
                RSTCEntryNode temp = new RSTCEntryNode
                {
                    FighterID = ((RSTCEntryNode) r).FighterID,
                    Name = r.Name
                };
                ((RSTCNode) _resource).cssList.AddChild(temp);
            }
        }

        public RSTCWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }
}