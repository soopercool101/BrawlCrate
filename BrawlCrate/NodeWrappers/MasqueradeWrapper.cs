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
        static MasqueradeWrapper()
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
        protected static void NewEntryAction(object sender, EventArgs e) { GetInstance<MasqueradeWrapper>().NewEntry(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[0].Enabled = _menu.Items[3].Enabled = _menu.Items[4].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[10].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MasqueradeWrapper w = GetInstance<MasqueradeWrapper>();
            _menu.Items[0].Enabled = (w._resource.Children.Count < 50);
            _menu.Items[3].Enabled = _menu.Items[10].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
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
                byte nextID = (byte)(((MasqueradeEntryNode)(_resource.Children[_resource.Children.Count - 1]))._costumeID + 1);
                if (((MasqueradeNode)_resource)._cosmeticSlot == 21 && (
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

        public MasqueradeWrapper() { ContextMenuStrip = _menu; }
    }
    [NodeWrapper(ResourceType.MASQEntry)]
    internal class MasqueradeEntryWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;
        static MasqueradeEntryWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Open Costume File", null, OpenCostumeAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void OpenCostumeAction(object sender, EventArgs e) { GetInstance<MasqueradeEntryWrapper>().OpenCostume(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[1].Visible = _menu.Items[0].Visible = _menu.Items[0].Enabled = _menu.Items[3].Enabled = _menu.Items[4].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[9].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MasqueradeEntryWrapper w = GetInstance<MasqueradeEntryWrapper>();
            List<string> files = ((MasqueradeEntryNode)w._resource).GetCostumeFilePath(Program.RootPath);
            _menu.Items[0].Enabled = _menu.Items[1].Visible = _menu.Items[0].Visible = files.Count != 0;
            if (files.Count >= 1)
            {
                _menu.Items[0].Text = "Open ";
                for (int i = 0; i < files.Count; i++)
                {
                    _menu.Items[0].Text += files[i].Substring(files[i].LastIndexOf('\\') + 1);
                    if (i + 1 < files.Count)
                    {
                        _menu.Items[0].Text += " and ";
                    }
                }
            }
            _menu.Items[3].Enabled = _menu.Items[9].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }
        #endregion

        public void OpenCostume()
        {
            List<string> files = ((MasqueradeEntryNode)_resource).GetCostumeFilePath(Program.RootPath);
            foreach (string s in files)
            {
                Process BrawlCrate = Process.Start(new ProcessStartInfo()
                {
                    FileName = AppDomain.CurrentDomain.BaseDirectory + "\\BrawlCrate.exe",
                    Arguments = "\"" + s + "\"",
                });
            }
        }

        public MasqueradeEntryWrapper() { ContextMenuStrip = _menu; }
    }
}