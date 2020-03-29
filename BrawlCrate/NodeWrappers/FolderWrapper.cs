using System.ComponentModel;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.Folder)]
    public class FolderWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        private static readonly ToolStripMenuItem DeleteToolStripMenuItem =
            new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete);

        static FolderWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(DeleteToolStripMenuItem);
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            DeleteToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            FolderWrapper w = GetInstance<FolderWrapper>();

            DeleteToolStripMenuItem.Enabled = w.Parent != null;
        }

        #endregion

        public FolderWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string Export()
        {
            if (Program.SaveFolder(out string outPath))
            {
                _resource.Export(outPath);
            }

            return outPath;
        }

        public override void Delete()
        {
            if (Parent == null)
            {
                return;
            }

            if (MessageBox.Show("Are you sure you would like to delete this folder and all files contained within it?",
                "Delete Folder", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            string dir = ((FolderNode) _resource).FolderPath;
            base.Delete();
            Directory.Delete(dir, true);
        }
    }
}