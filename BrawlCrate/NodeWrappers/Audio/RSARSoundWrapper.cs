using BrawlCrate.UI;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB;
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

        private static readonly ToolStripMenuItem _changeSoundToolStripMenuItem;
        private static readonly ToolStripMenuItem _viewFileToolStripMenuItem;

        private static readonly ToolStripMenuItem DuplicateToolStripMenuItem =
            new ToolStripMenuItem("&Duplicate", null, DuplicateAction, Keys.Control | Keys.D);

        private static readonly ToolStripMenuItem ReplaceToolStripMenuItem =
            new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R);

        private static readonly ToolStripMenuItem RestoreToolStripMenuItem =
            new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T);

        private static readonly ToolStripMenuItem MoveUpToolStripMenuItem =
            new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up);

        private static readonly ToolStripMenuItem MoveDownToolStripMenuItem =
            new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down);

        private static readonly ToolStripMenuItem DeleteToolStripMenuItem =
            new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete);

        static RSARSoundWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(_changeSoundToolStripMenuItem =
                new ToolStripMenuItem("Change Sound", null, ChangeSoundAction, Keys.Control | Keys.W));
            _menu.Items.Add(_viewFileToolStripMenuItem =
                new ToolStripMenuItem("View File", null, ViewFileAction, Keys.Control | Keys.I));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(DuplicateToolStripMenuItem);
            _menu.Items.Add(RestoreToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(MoveUpToolStripMenuItem);
            _menu.Items.Add(MoveDownToolStripMenuItem);
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(DeleteToolStripMenuItem);
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
            DuplicateToolStripMenuItem.Enabled = true;
            ReplaceToolStripMenuItem.Enabled = true;
            DeleteToolStripMenuItem.Enabled = true;
            RestoreToolStripMenuItem.Enabled = true;
            MoveUpToolStripMenuItem.Enabled = true;
            MoveDownToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            RSARSoundWrapper w = GetInstance<RSARSoundWrapper>();
            RSARSoundNode n = w._resource as RSARSoundNode;
            _changeSoundToolStripMenuItem.Enabled = n._waveDataNode != null;
            _viewFileToolStripMenuItem.Enabled = n.SoundFileNode != null;
            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
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

                if (Program.OpenFile(ReplaceFilter, out string inPath))
                {
                    n._waveDataNode.Sound.Replace(inPath);
                    MainForm.Instance.resourceTree_SelectionChanged(null, null);
                }
            }
        }

        public void ViewFile()
        {
            RSARFileNode n;
            if ((n = (_resource as RSARSoundNode)?.SoundFileNode) == null)
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