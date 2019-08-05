using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.REFT)]
    public class REFTWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        static REFTWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Import Texture", null, NewEntryAction, Keys.Control | Keys.I));
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
            GetInstance<REFTWrapper>().ImportTexture();
        }

        #endregion

        public REFTWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.REFT;

        public void ImportTexture()
        {
            int index = Program.OpenFile(FileFilters.Images, out string path);
            if (index > 0)
            {
                using (TextureConverterDialog dlg = new TextureConverterDialog())
                {
                    dlg.ImageSource = path;
                    if (dlg.ShowDialog(MainForm.Instance, Resource as REFTNode) == DialogResult.OK)
                    {
                        BaseWrapper w = FindResource(dlg.REFTTextureNode, true);
                        w.EnsureVisible();
                        w.TreeView.SelectedNode = w;
                    }
                }
            }
        }
    }
}