using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.REFTImage)]
    public class REFTImageWrapper : GenericWrapper
    {
        private static readonly ContextMenuStrip _menu;

        static REFTImageWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Re-Encode", null, ReEncodeAction));
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

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (replaceToolStripMenuItem != null)
            {
                replaceToolStripMenuItem.Enabled = true;
            }

            if (restoreToolStripMenuItem != null)
            {
                restoreToolStripMenuItem.Enabled = true;
            }

            if (moveUpToolStripMenuItem != null)
            {
                moveUpToolStripMenuItem.Enabled = true;
            }

            if (moveDownToolStripMenuItem != null)
            {
                moveDownToolStripMenuItem.Enabled = true;
            }

            if (deleteToolStripMenuItem != null)
            {
                deleteToolStripMenuItem.Enabled = true;
            }
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<REFTImageWrapper>();

            if (replaceToolStripMenuItem != null)
            {
                replaceToolStripMenuItem.Enabled = w.Parent != null;
            }

            if (restoreToolStripMenuItem != null)
            {
                restoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            }

            if (moveUpToolStripMenuItem != null)
            {
                moveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            }

            if (moveDownToolStripMenuItem != null)
            {
                moveDownToolStripMenuItem.Enabled = w.NextNode != null;
            }

            if (deleteToolStripMenuItem != null)
            {
                deleteToolStripMenuItem.Enabled = w.Parent != null;
            }
        }

        protected static void ReEncodeAction(object sender, EventArgs e)
        {
            GetInstance<REFTImageWrapper>().ReEncode();
        }

        public REFTImageWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void ReEncode()
        {
            using (TextureConverterDialog dlg = new TextureConverterDialog())
            {
                dlg.LoadImages((Resource as REFTEntryNode).GetImage(0));
                dlg.ShowDialog(MainForm.Instance, Resource as REFTEntryNode);
            }
        }

        public override string ExportFilter => FileFilters.Images;

        public override void OnReplace(string inStream, int filterIndex)
        {
            if (filterIndex == 8)
            {
                base.OnReplace(inStream, filterIndex);
            }
            else
            {
                using (TextureConverterDialog dlg = new TextureConverterDialog())
                {
                    dlg.ImageSource = inStream;
                    dlg.ShowDialog(MainForm.Instance, (REFTEntryNode) _resource);
                }
            }
        }
    }
}