using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.BrawlEx;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.CSSC)]
    internal class CSSCWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;
        private static ToolStripMenuItem _newEntryToolStripMenuItem;
        private static ToolStripMenuItem _replaceToolStripMenuItem;
        private static ToolStripMenuItem _restoreToolStripMenuItem;
        private static ToolStripMenuItem _moveUpToolStripMenuItem;
        private static ToolStripMenuItem _moveDownToolStripMenuItem;
        private static ToolStripMenuItem _deleteToolStripMenuItem;

        static CSSCWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(_newEntryToolStripMenuItem = new ToolStripMenuItem("Add New Entry", null, NewEntryAction, Keys.Control | Keys.J));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(_replaceToolStripMenuItem = new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(_restoreToolStripMenuItem = new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_moveUpToolStripMenuItem = new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(_moveDownToolStripMenuItem = new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<CSSCWrapper>().NewEntry();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _newEntryToolStripMenuItem.Enabled = true;
            _replaceToolStripMenuItem.Enabled = true;
            _deleteToolStripMenuItem.Enabled = true;
            _restoreToolStripMenuItem.Enabled = true;
            _moveUpToolStripMenuItem.Enabled = true;
            _moveDownToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            CSSCWrapper w = GetInstance<CSSCWrapper>();
            _newEntryToolStripMenuItem.Enabled = w._resource.Children.Count < 50;
            _replaceToolStripMenuItem.Enabled = w.Parent != null;
            _deleteToolStripMenuItem.Enabled = w.Parent != null;
            _restoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            _moveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            _moveDownToolStripMenuItem.Enabled = w.NextNode != null;
        }

        #endregion

        public override string ExportFilter => FileFilters.CSSC;

        public void NewEntry()
        {
            if (_resource.Children.Count >= 50)
            {
                return;
            }

            CSSCEntryNode node = new CSSCEntryNode
            {
                _colorID = 0x0B
            };
            if (_resource.HasChildren)
            {
                node._costumeID =
                    (byte) (((CSSCEntryNode) _resource.Children[_resource.Children.Count - 1])._costumeID + 1);
            }

            node._name =
                "Fit" + BrawlLib.BrawlCrate.FighterNameGenerators.InternalNameFromID(
                    ((CSSCNode) _resource)._cosmeticSlot, BrawlLib.BrawlCrate.FighterNameGenerators.cosmeticIDIndex,
                    "+S") + node._costumeID.ToString("00") + (BrawlExColorID.Colors.Length > node._colorID
                    ? " - " + BrawlExColorID.Colors[node._colorID].Name
                    : "");
            _resource.AddChild(node);
        }

        public CSSCWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }

    [NodeWrapper(ResourceType.CSSCEntry)]
    internal class CSSCEntryWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        private static ToolStripMenuItem _openCostumeToolStripMenuItem;
        private static ToolStripSeparator _openCostumeToolStripSeparator;
        private static ToolStripMenuItem _replaceToolStripMenuItem;
        private static ToolStripMenuItem _restoreToolStripMenuItem;
        private static ToolStripMenuItem _moveUpToolStripMenuItem;
        private static ToolStripMenuItem _moveDownToolStripMenuItem;
        private static ToolStripMenuItem _deleteToolStripMenuItem;

        static CSSCEntryWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(_openCostumeToolStripMenuItem = new ToolStripMenuItem("Open Costume File", null, OpenCostumeAction));
            _menu.Items.Add(_openCostumeToolStripSeparator = new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(_replaceToolStripMenuItem = new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(_restoreToolStripMenuItem = new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_moveUpToolStripMenuItem = new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(_moveDownToolStripMenuItem = new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void OpenCostumeAction(object sender, EventArgs e)
        {
            GetInstance<CSSCEntryWrapper>().OpenCostume();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _openCostumeToolStripMenuItem.Enabled = true;
            _openCostumeToolStripMenuItem.Visible = true;
            _openCostumeToolStripSeparator.Visible = true;
            _replaceToolStripMenuItem.Enabled = true;
            _deleteToolStripMenuItem.Enabled = true;
            _restoreToolStripMenuItem.Enabled = true;
            _moveUpToolStripMenuItem.Enabled = true;
            _moveDownToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            CSSCEntryWrapper w = GetInstance<CSSCEntryWrapper>();

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

            _replaceToolStripMenuItem.Enabled = w.Parent != null;
            _deleteToolStripMenuItem.Enabled = w.Parent != null;
            _restoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            _moveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            _moveDownToolStripMenuItem.Enabled = w.NextNode != null;
        }

        #endregion

        public void OpenCostume()
        {
            List<string> files = ((CSSCEntryNode) _resource).GetCostumeFilePath(Program.RootPath);
            foreach (string s in files)
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = AppDomain.CurrentDomain.BaseDirectory + "\\BrawlCrate.exe",
                    Arguments = "\"" + s + "\"",
                });
            }
        }

        public CSSCEntryWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }
}