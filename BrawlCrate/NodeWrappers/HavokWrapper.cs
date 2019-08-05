using BrawlLib;
using BrawlLib.IO;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

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

        protected static void ExportPatchedAction(object sender, EventArgs e)
        {
            GetInstance<HavokWrapper>().ExportPatched();
        }

        public HavokWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.Havok;
        public override string ImportFilter => FileFilters.Havok;

        public void ExportPatched()
        {
            int index = Program.SaveFile(ExportFilter, Text, out string outPath);
            if (index != 0)
            {
                if (Parent == null)
                {
                    _resource.Merge(Control.ModifierKeys == (Keys.Control | Keys.Shift));
                }

                //_resource.Rebuild();
                HavokNode p = _resource as HavokNode;
                using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                    FileShare.ReadWrite, 8, FileOptions.SequentialScan))
                {
                    stream.SetLength(p._buffer.Length);
                    using (FileMap map = FileMap.FromStream(stream))
                    {
                        Memory.Move(map.Address, p._buffer.Address, (uint) p._buffer.Length);
                    }
                }
            }
        }
    }
}