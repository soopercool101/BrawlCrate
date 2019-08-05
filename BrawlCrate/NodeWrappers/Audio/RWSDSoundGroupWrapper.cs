using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSARFileSoundGroup)]
    public class RWSDSoundGroupWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        static RWSDSoundGroupWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Import New Sound", null, CreateAction, Keys.Control | Keys.I));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(replaceToolStripMenuItem = new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(moveUpToolStripMenuItem = new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(moveDownToolStripMenuItem = new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void CreateAction(object sender, EventArgs e)
        {
            GetInstance<RWSDSoundGroupWrapper>().Import();
        }

        private unsafe void Import()
        {
            if (Program.OpenFile("PCM Audio (*.wav)|*.wav", out string path) > 0)
            {
                RSARFileAudioNode n;

                if ((_resource.Parent as NW4RNode).VersionMinor >= 3)
                {
                    n = new RWAVNode();
                }
                else
                {
                    n = new WAVESoundNode();
                }

                _resource.AddChild(n);
                n.Replace(path);

                BaseWrapper res = FindResource(n, true);
                res.EnsureVisible();
                res.TreeView.SelectedNode = res;
            }
        }

        #endregion

        public RWSDSoundGroupWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }
}