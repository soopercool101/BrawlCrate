using System;
using System.ComponentModel;
using System.Windows.Forms;
using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.TPL)]
    public class TPLWrapper : GenericWrapper
    {
        public TPLWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.TPL;

        public void ImportTexture()
        {
            var index = Program.OpenFile(FileFilters.Images, out var path);
            if (index == 8)
            {
                var t = new TPLTextureNode {Name = "Texture"};
                _resource.AddChild(t);
                t.Replace(path);

                var w = FindResource(t, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
            else if (index > 0)
            {
                using (var dlg = new TextureConverterDialog())
                {
                    dlg.ImageSource = path;
                    if (dlg.ShowDialog(MainForm.Instance, Resource as TPLNode) == DialogResult.OK)
                    {
                        var w = FindResource(dlg.TPLTextureNode, true);
                        w.EnsureVisible();
                        w.TreeView.SelectedNode = w;
                    }
                }
            }
        }

        #region Menu

        private static readonly ContextMenuStrip _menu;

        static TPLWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Import Texture", null, NewEntryAction, Keys.Control | Keys.I));
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

        protected static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<TPLWrapper>().ImportTexture();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[3].Enabled = _menu.Items[4].Enabled =
                _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[9].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<TPLWrapper>();
            _menu.Items[3].Enabled = _menu.Items[9].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = w._resource.IsDirty || w._resource.IsBranch;
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }

        #endregion
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

        public TPLTextureNodeWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.Images;

        protected static void ReEncodeAction(object sender, EventArgs e)
        {
            GetInstance<TPLTextureNodeWrapper>().ReEncode();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[3].Enabled = _menu.Items[4].Enabled =
                _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[9].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<TPLTextureNodeWrapper>();
            _menu.Items[3].Enabled = _menu.Items[9].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = w._resource.IsDirty || w._resource.IsBranch;
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }

        public void ReEncode()
        {
            using (var dlg = new TextureConverterDialog())
            {
                dlg.LoadImages((Resource as TPLTextureNode).GetImage(0));
                dlg.ShowDialog(MainForm.Instance, Resource as TPLTextureNode);
            }
        }

        public override void OnReplace(string inStream, int filterIndex)
        {
            if (filterIndex == 8)
                base.OnReplace(inStream, filterIndex);
            else
                using (var dlg = new TextureConverterDialog())
                {
                    dlg.ImageSource = inStream;
                    dlg.ShowDialog(MainForm.Instance, (TPLTextureNode) _resource);
                }
        }
    }
}