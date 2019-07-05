using BrawlLib;
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

        static RSTCWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Add New Entry (Both Lists)", null, NewEntryAction,
                Keys.Control | Keys.J));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Sync Lists", null,
                new ToolStripMenuItem("From CSS List to Random List", null, SyncRandomAction),
                new ToolStripMenuItem("From Random List to CSS List", null, SyncCSSAction)));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Clear Lists", null, ClearAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void NewEntryAction(object sender, EventArgs e)
        {
            RSTCWrapper w = GetInstance<RSTCWrapper>();
            ResourceNode r = w._resource;
            if (((RSTCNode) r).cssList.Children.Count >= 100 || ((RSTCNode) r).randList.Children.Count >= 100)
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
            _menu.Items[0].Enabled = _menu.Items[5].Enabled = _menu.Items[6].Enabled = _menu.Items[10].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            RSTCWrapper w = GetInstance<RSTCWrapper>();
            ResourceNode r = w._resource;
            _menu.Items[0].Enabled = ((RSTCNode) r).cssList.Children.Count <= 100 ||
                                     ((RSTCNode) r).randList.Children.Count <= 100;
            _menu.Items[5].Enabled = w.Parent != null;
            _menu.Items[6].Enabled = w._resource.IsDirty || w._resource.IsBranch;
            _menu.Items[10].Enabled =
                ((RSTCNode) r).cssList.Children.Count > 0 || ((RSTCNode) r).randList.Children.Count > 0;
        }

        #endregion

        public override string ExportFilter => FileFilters.RSTC;

        public void NewEntry(byte cssID)
        {
            if (((RSTCNode) _resource).cssList.Children.Count >= 100 ||
                ((RSTCNode) _resource).randList.Children.Count >= 100)
            {
                return;
            }

            RSTCEntryNode node1 = new RSTCEntryNode
            {
                FighterID = cssID,
                _name = BrawlLib.BrawlCrate.FighterNameGenerators.FromID(cssID,
                    BrawlLib.BrawlCrate.FighterNameGenerators.cssSlotIDIndex, "+S")
            };
            ((RSTCNode) _resource).cssList.AddChild(node1);
            RSTCEntryNode node2 = new RSTCEntryNode
            {
                FighterID = cssID,
                _name = BrawlLib.BrawlCrate.FighterNameGenerators.FromID(cssID,
                    BrawlLib.BrawlCrate.FighterNameGenerators.cssSlotIDIndex, "+S")
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