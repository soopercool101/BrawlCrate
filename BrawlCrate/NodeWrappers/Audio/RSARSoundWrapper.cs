using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSARSound)]
    public class RSARSoundWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.WAV;

        #region Menu

        private static readonly ContextMenuStrip _menu;

        private static ToolStripMenuItem _changeSoundToolStripMenuItem;
        private static ToolStripMenuItem _viewFileToolStripMenuItem;

        static RSARSoundWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(_changeSoundToolStripMenuItem = new ToolStripMenuItem("Change Sound", null, ChangeSoundAction, Keys.Control | Keys.W));
            _menu.Items.Add(_viewFileToolStripMenuItem = new ToolStripMenuItem("View File", null, ViewFileAction, Keys.Control | Keys.I));
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

        protected static void ChangeSoundAction(object sender, EventArgs e)
        {
            GetInstance<RSARSoundWrapper>().ChangeSound();
        }

        protected static void ViewFileAction(object sender, EventArgs e)
        {
            GetInstance<RSARSoundWrapper>().ViewFile();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _changeSoundToolStripMenuItem.Enabled = true;
            _viewFileToolStripMenuItem.Enabled = true;
            replaceToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
            restoreToolStripMenuItem.Enabled = true;
            moveUpToolStripMenuItem.Enabled = true;
            moveDownToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            RSARSoundWrapper w = GetInstance<RSARSoundWrapper>();
            RSARSoundNode n = w._resource as RSARSoundNode;
            _changeSoundToolStripMenuItem.Enabled = n._waveDataNode != null;
            _viewFileToolStripMenuItem.Enabled = n.SoundFileNode != null;
            replaceToolStripMenuItem.Enabled = w.Parent != null;
            deleteToolStripMenuItem.Enabled = w.Parent != null;
            restoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
        }

        #endregion

        public RSARSoundWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void ChangeSound()
        {
            RSARSoundNode n = _resource as RSARSoundNode;
            if (n._waveDataNode != null)
            {
                if (n._waveDataNode._refs.Count > 1)
                {
                    string s = "The following entries also use this sound:\n";
                    foreach (RSARSoundNode x in n._waveDataNode._refs)
                    {
                        s += x.TreePath + "\n";
                    }

                    s += "\nDo you still want to replace this sound?";
                    if (MessageBox.Show(s, "Continue?", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }

                int index = Program.OpenFile(ReplaceFilter, out string inPath);
                if (index != 0)
                {
                    n._waveDataNode.Sound.Replace(inPath);
                    MainForm.Instance.resourceTree_SelectionChanged(null, null);
                }
            }
        }

        public void ViewFile()
        {
            RSARFileNode n;
            if ((n = (_resource as RSARSoundNode).SoundFileNode) == null)
            {
                return;
            }

            if (n is RSARExtFileNode)
            {
                RSARExtFileNode ext = n as RSARExtFileNode;
                if (File.Exists(ext.FullExtPath))
                {
                    Process.Start(ext.FullExtPath);
                }
            }
            else
            {
                new EditRSARFileDialog().ShowDialog(MainForm.Instance, n);
            }
        }
    }
}