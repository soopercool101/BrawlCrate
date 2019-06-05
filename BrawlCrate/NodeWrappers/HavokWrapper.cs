using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using BrawlLib;
using BrawlLib.IO;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.Havok)]
    public class HavokWrapper : GenericWrapper
    {
        private static readonly ContextMenuStrip _menu;

        static HavokWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Export Patched", null, ExportPatchedAction));
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

        public HavokWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.Havok;
        public override string ImportFilter => FileFilters.Havok;

        protected static void ExportPatchedAction(object sender, EventArgs e)
        {
            GetInstance<HavokWrapper>().ExportPatched();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[3].Enabled = _menu.Items[4].Enabled =
                _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[10].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<HavokWrapper>();
            _menu.Items[3].Enabled = _menu.Items[10].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = w._resource.IsDirty || w._resource.IsBranch;
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }

        public void ExportPatched()
        {
            var index = Program.SaveFile(ExportFilter, Text, out var outPath);
            if (index != 0)
            {
                if (Parent == null) _resource.Merge(Control.ModifierKeys == (Keys.Control | Keys.Shift));
                //_resource.Rebuild();
                var p = _resource as HavokNode;
                using (var stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                    FileShare.ReadWrite, 8, FileOptions.SequentialScan))
                {
                    stream.SetLength(p._buffer.Length);
                    using (var map = FileMap.FromStream(stream))
                    {
                        Memory.Move(map.Address, p._buffer.Address, (uint) p._buffer.Length);
                    }
                }
            }
        }
    }
}