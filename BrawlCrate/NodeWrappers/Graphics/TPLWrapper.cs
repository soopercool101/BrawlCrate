using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.TPL)]
    public class TPLWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        static TPLWrapper()
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
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<TPLWrapper>().ImportTexture();
        }

        #endregion

        public TPLWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.TPL;

        public void ImportTexture()
        {
            int index = Program.OpenFile(FileFilters.Images, out string path);
            if (index == 8)
            {
                TPLTextureNode t = new TPLTextureNode() {Name = "Texture"};
                _resource.AddChild(t);
                t.Replace(path);

                BaseWrapper w = FindResource(t, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
            else if (index > 0)
            {
                using (TextureConverterDialog dlg = new TextureConverterDialog())
                {
                    dlg.ImageSource = path;
                    if (dlg.ShowDialog(MainForm.Instance, Resource as TPLNode) == DialogResult.OK)
                    {
                        BaseWrapper w = FindResource(dlg.TPLTextureNode, true);
                        w.EnsureVisible();
                        w.TreeView.SelectedNode = w;
                    }
                }
            }
        }
    }

    [NodeWrapper(ResourceType.TPLTexture)]
    public class TPLTextureNodeWrapper : GenericWrapper
    {
        private static readonly ContextMenuStrip _menu;

        static TPLTextureNodeWrapper()
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
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void ReEncodeAction(object sender, EventArgs e)
        {
            GetInstance<TPLTextureNodeWrapper>().ReEncode();
        }

        public TPLTextureNodeWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void ReEncode()
        {
            using (TextureConverterDialog dlg = new TextureConverterDialog())
            {
                dlg.LoadImages((Resource as TPLTextureNode).GetImage(0));
                dlg.ShowDialog(MainForm.Instance, Resource as TPLTextureNode);
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
                    dlg.ShowDialog(MainForm.Instance, (TPLTextureNode) _resource);
                }
            }
        }
    }
}