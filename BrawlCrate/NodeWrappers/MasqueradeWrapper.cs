using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.MASQ)]
    internal class MasqueradeWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        private static readonly ToolStripMenuItem _newEntryToolStripMenuItem;

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

        static MasqueradeWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(_newEntryToolStripMenuItem =
                new ToolStripMenuItem("Add New Entry", null, NewEntryAction, Keys.Control | Keys.J));
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

        protected static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<MasqueradeWrapper>().NewEntry();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _newEntryToolStripMenuItem.Enabled = true;
            DuplicateToolStripMenuItem.Enabled = true;
            ReplaceToolStripMenuItem.Enabled = true;
            DeleteToolStripMenuItem.Enabled = true;
            RestoreToolStripMenuItem.Enabled = true;
            MoveUpToolStripMenuItem.Enabled = true;
            MoveDownToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MasqueradeWrapper w = GetInstance<MasqueradeWrapper>();
            _newEntryToolStripMenuItem.Enabled = w._resource.Children.Count < 50;
            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
        }

        #endregion

        public override string ExportFilter => FileFilters.MASQ;

        public void NewEntry()
        {
            if (_resource.Children.Count >= 50)
            {
                return;
            }

            MasqueradeEntryNode node = new MasqueradeEntryNode
            {
                _colorID = 0x0B
            };
            if (_resource.HasChildren)
            {
                byte nextID =
                    (byte) (((MasqueradeEntryNode) _resource.Children[_resource.Children.Count - 1])._costumeID + 1);
                if (((MasqueradeNode) _resource)._cosmeticSlot == 21 && (
                        nextID == 15 ||
                        nextID == 31 ||
                        nextID == 47 ||
                        nextID == 63))
                {
                    ++nextID; // Prevent wario edge cases
                }

                node._costumeID = nextID;
            }

            _resource.AddChild(node);
            node.regenName();
        }

        public MasqueradeWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }

    [NodeWrapper(ResourceType.MASQEntry)]
    internal class MasqueradeEntryWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        private static readonly ToolStripMenuItem _openCostumeToolStripMenuItem;
        private static ToolStripSeparator _openCostumeToolStripSeparator;

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

        static MasqueradeEntryWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(_openCostumeToolStripMenuItem =
                new ToolStripMenuItem("Open Costume File", null, OpenCostumeAction));
            _menu.Items.Add(_openCostumeToolStripSeparator = new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(DuplicateToolStripMenuItem);
            _menu.Items.Add(ReplaceToolStripMenuItem);
            _menu.Items.Add(RestoreToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(MoveUpToolStripMenuItem);
            _menu.Items.Add(MoveDownToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(DeleteToolStripMenuItem);
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void OpenCostumeAction(object sender, EventArgs e)
        {
            GetInstance<MasqueradeEntryWrapper>().OpenCostume();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _openCostumeToolStripMenuItem.Enabled = true;
            _openCostumeToolStripMenuItem.Visible = true;
            _openCostumeToolStripSeparator.Visible = true;
            DuplicateToolStripMenuItem.Enabled = true;
            ReplaceToolStripMenuItem.Enabled = true;
            DeleteToolStripMenuItem.Enabled = true;
            RestoreToolStripMenuItem.Enabled = true;
            MoveUpToolStripMenuItem.Enabled = true;
            MoveDownToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MasqueradeEntryWrapper w = GetInstance<MasqueradeEntryWrapper>();

            List<string> files = ((CSSCEntryNode) w._resource).GetCostumeFilePath(Program.RootPath);

            _openCostumeToolStripMenuItem.Enabled = files.Count != 0;
            _openCostumeToolStripMenuItem.Visible = files.Count != 0;
            _openCostumeToolStripSeparator.Visible = files.Count != 0;
            if (files.Count >= 1)
            {
                _openCostumeToolStripMenuItem.Text = "Open ";
                for (int i = 0; i < files.Count; i++)
                {
                    _openCostumeToolStripMenuItem.Text += files[i].Substring(files[i].LastIndexOf('\\') + 1);
                    if (i + 1 < files.Count)
                    {
                        _openCostumeToolStripMenuItem.Text += " and ";
                    }
                    else
                    {
                        _openCostumeToolStripMenuItem.Text += ", ";
                    }
                }
            }

            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
        }

        #endregion

        public void OpenCostume()
        {
            List<string> files = ((MasqueradeEntryNode) _resource).GetCostumeFilePath(Program.RootPath);
            foreach (string s in files)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = AppDomain.CurrentDomain.BaseDirectory + "\\BrawlCrate.exe",
                    Arguments = "\"" + s + "\""
                });
            }
        }

        public MasqueradeEntryWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }
}