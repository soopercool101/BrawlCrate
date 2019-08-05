using BrawlLib;
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

        private static ToolStripMenuItem _newEntryToolStripMenuItem;

        static MasqueradeWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(_newEntryToolStripMenuItem = new ToolStripMenuItem("Add New Entry", null, NewEntryAction, Keys.Control | Keys.J));
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

        protected static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<MasqueradeWrapper>().NewEntry();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _newEntryToolStripMenuItem.Enabled = true;
            replaceToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
            restoreToolStripMenuItem.Enabled = true;
            moveUpToolStripMenuItem.Enabled = true;
            moveDownToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MasqueradeWrapper w = GetInstance<MasqueradeWrapper>();
            _newEntryToolStripMenuItem.Enabled = w._resource.Children.Count < 50;
            replaceToolStripMenuItem.Enabled = w.Parent != null;
            deleteToolStripMenuItem.Enabled = w.Parent != null;
            restoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            moveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            moveDownToolStripMenuItem.Enabled = w.NextNode != null;
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

        private static ToolStripMenuItem _openCostumeToolStripMenuItem;
        private static ToolStripSeparator _openCostumeToolStripSeparator;

        static MasqueradeEntryWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(_openCostumeToolStripMenuItem = new ToolStripMenuItem("Open Costume File", null, OpenCostumeAction));
            _menu.Items.Add(_openCostumeToolStripSeparator = new ToolStripSeparator());
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

        protected static void OpenCostumeAction(object sender, EventArgs e)
        {
            GetInstance<MasqueradeEntryWrapper>().OpenCostume();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _openCostumeToolStripMenuItem.Enabled = true;
            _openCostumeToolStripMenuItem.Visible = true;
            _openCostumeToolStripSeparator.Visible = true;
            replaceToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
            restoreToolStripMenuItem.Enabled = true;
            moveUpToolStripMenuItem.Enabled = true;
            moveDownToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MasqueradeEntryWrapper w = GetInstance<MasqueradeEntryWrapper>();

            List<string> files = ((CSSCEntryNode)w._resource).GetCostumeFilePath(Program.RootPath);

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

            replaceToolStripMenuItem.Enabled = w.Parent != null;
            deleteToolStripMenuItem.Enabled = w.Parent != null;
            restoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            moveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            moveDownToolStripMenuItem.Enabled = w.NextNode != null;
        }

        #endregion

        public void OpenCostume()
        {
            List<string> files = ((MasqueradeEntryNode) _resource).GetCostumeFilePath(Program.RootPath);
            foreach (string s in files)
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = AppDomain.CurrentDomain.BaseDirectory + "\\BrawlCrate.exe",
                    Arguments = "\"" + s + "\"",
                });
            }
        }

        public MasqueradeEntryWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }
}