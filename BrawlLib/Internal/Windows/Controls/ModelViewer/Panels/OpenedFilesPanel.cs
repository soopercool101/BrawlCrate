using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using BrawlLib.OpenGL;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    public partial class OpenedFilesControl : UserControl
    {
        public ModelEditorBase _mainWindow;
        public ResourceNode SelectedFile => (ResourceNode) listBox1.SelectedItem;

        public BindingList<ResourceNode> OpenedFiles =>
            _mainWindow != null ? _mainWindow._openedFiles : new BindingList<ResourceNode>();

        public OpenedFilesControl()
        {
            InitializeComponent();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            _mainWindow.OpenInMainForm(SelectedFile);
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.X, e.Y);
            if (listBox1.SelectedIndex != index)
            {
                listBox1.SelectedIndex = index;
            }

            if (e.Button == MouseButtons.Right)
            {
                if (listBox1.SelectedIndex >= 0)
                {
                    listBox1.ContextMenuStrip = ctxFile;
                }
                else
                {
                    listBox1.ContextMenuStrip = null;
                }
            }
        }

        private void ctxFile_Opening(object sender, CancelEventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                e.Cancel = true;
            }
            else
            {
                saveToolStripMenuItem.Enabled = SelectedFile.IsDirty;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                label1.Text = "";
                return;
            }

            string s = Path.GetFileName(listBox1.SelectedItem.ToString() == "<null>"
                ? "null"
                : listBox1.SelectedItem.ToString());
            label1.Text = $"{s} - Has {(SelectedFile.IsDirty ? "" : "not ")}changed";
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                listBox1.SelectedIndex = -1;
            }
        }

        private bool Save(ResourceNode r)
        {
            if (r._origPath == null)
            {
                return SaveAs(r);
            }

            r.Merge(ModifierKeys == (Keys.Control | Keys.Shift));
            r.Export(r._origPath);
            r.IsDirty = false;
            return true;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceNode r = SelectedFile;
            if (MessageBox.Show(this,
                $"Are you sure you want to save {Path.GetFileName(r._origPath)}?", "Are you sure?",
                MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            Save(r);
        }

        private bool SaveAs(ResourceNode r)
        {
            using (SaveFileDialog d = new SaveFileDialog())
            {
                d.InitialDirectory = r._origPath.Substring(0, r._origPath.LastIndexOf('\\'));
                d.Filter = string.Format("(*{0})|*{0}", Path.GetExtension(r._origPath));
                d.Title = "Please choose a location to save this file.";
                if (d.ShowDialog(this) == DialogResult.OK)
                {
                    r.Merge();
                    r.Export(d.FileName);
                    return true;
                }
            }

            return false;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs(SelectedFile);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFile(listBox1.SelectedIndex, true);
            _mainWindow.ModelPanel.RefreshReferences();
        }

        private bool CloseFile(int i, bool user)
        {
            if (OpenedFiles == null || i < 0 || i >= OpenedFiles.Count)
            {
                return true;
            }

            ResourceNode r = OpenedFiles[i];

            if (r == null)
            {
                return true;
            }

            bool shouldClose = _mainWindow.ShouldCloseFile(r);
            if (r.IsDirty && shouldClose)
            {
                string s = user
                    ? "Save changes?"
                    : "You have made changes to the file \"" + r._origPath +
                      "\". Would you like to save those changes?";

                DialogResult res = MessageBox.Show(this, s, "Closing external file.", MessageBoxButtons.YesNoCancel);

                if (res == DialogResult.Cancel)
                {
                    return false;
                }

                if (res == DialogResult.Yes && !SaveExternal(r, false))
                {
                    DialogResult res2 = MessageBox.Show(this,
                        "Unable to save this file. Close it anyway?",
                        "Closing external file.",
                        MessageBoxButtons.YesNo);

                    if (res2 == DialogResult.No)
                    {
                        return false;
                    }
                }
            }

            _mainWindow.Updating = true;
            for (int m = 0; m < _mainWindow.ModelPanel._renderList.Count; m++)
            {
                ResourceNode o = _mainWindow.ModelPanel._renderList[m] as ResourceNode;
                if (o != null && o.RootNode == r)
                {
                    _mainWindow.ModelPanel.RemoveTarget(o as IRenderedObject, false);
                    m--;
                }
            }

            _mainWindow.ModelPanel.RemoveReference(r, false);
            _mainWindow.UnloadAnimations(r);
            _mainWindow.Updating = false;

            if (shouldClose)
            {
                r.Dispose();
            }

            OpenedFiles.RemoveAt(i);

            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadExternal();
        }

        public bool LoadExternal(bool models = true, bool animations = true, bool etc = true)
        {
            _mainWindow._dlgOpen.Filter = SupportedFilesHandler.GetCompleteFilter(
                "pac", "pcs",
                "brres",
                "mrg", "mrgc",
                "arc", "szs",
                "chr0", "srt0", "pat0", "vis0", "shp0", "clr0", "scn0");

            if (_mainWindow._dlgOpen.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in _mainWindow._dlgOpen.FileNames)
                {
                    _mainWindow.OpenFile(file, models, animations, etc);
                }
            }

            return false;
        }

        public bool CloseAllFiles()
        {
            if (OpenedFiles != null)
            {
                while (OpenedFiles.Count > 0)
                {
                    if (!CloseFile(0, false))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool SaveExternal(ResourceNode current, bool As)
        {
            if (current == null || !current.IsDirty && !As)
            {
                return true;
            }

#if !DEBUG
            try
            {
#endif
            if (As)
            {
                using (SaveFileDialog d = new SaveFileDialog())
                {
                    d.InitialDirectory = current._origPath.Substring(0, current._origPath.LastIndexOf('\\'));
                    d.Filter = string.Format("(*{0})|*{0}", Path.GetExtension(current._origPath));
                    d.Title = "Please choose a location to save this file.";
                    if (d.ShowDialog(this) == DialogResult.OK)
                    {
                        current.Merge();
                        current.Export(d.FileName);
                    }
                }
            }
            else
            {
                current.Merge();
                current.Export(current._origPath);
            }

            return true;
#if !DEBUG
            }
            catch (Exception x)
            {
                MessageBox.Show(this, x.ToString());
            }

            return false;
#endif
        }

        public int Sort(DrawCallBase x, DrawCallBase y)
        {
            ResourceNode r1 = x.Parent as ResourceNode;
            ResourceNode r2 = y.Parent as ResourceNode;

            if (r1 == null && r2 != null)
            {
                return -1;
            }

            if (r2 == null && r1 != null)
            {
                return 1;
            }

            if (r1 == null && r2 == null)
            {
                return 0;
            }

            r1 = r1.RootNode;
            r2 = r2.RootNode;

            if (r1 == null && r2 != null)
            {
                return -1;
            }

            if (r2 == null && r1 != null)
            {
                return 1;
            }

            if (r1 == null && r2 == null)
            {
                return 0;
            }

            int xindex = OpenedFiles.IndexOf(r1);
            int yindex = OpenedFiles.IndexOf(r2);

            if (xindex < 0 && yindex >= 0)
            {
                return -1;
            }

            if (yindex < 0 && xindex >= 0)
            {
                return 1;
            }

            if (xindex < 0 && yindex < 0)
            {
                return 0;
            }

            if (xindex > yindex)
            {
                return 1;
            }

            return x.CompareTo(y);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index >= OpenedFiles.Count - 1)
            {
                return;
            }

            ResourceNode temp = OpenedFiles[index + 1];
            OpenedFiles[index + 1] = OpenedFiles[index];
            OpenedFiles[index] = temp;

            listBox1.SelectedIndex = index + 1;

            _mainWindow.ModelPanel.target_DrawCallsChanged(this, EventArgs.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index <= 0)
            {
                return;
            }

            ResourceNode temp = OpenedFiles[index - 1];
            OpenedFiles[index - 1] = OpenedFiles[index];
            OpenedFiles[index] = temp;

            listBox1.SelectedIndex = index - 1;

            _mainWindow.ModelPanel.target_DrawCallsChanged(this, EventArgs.Empty);
        }
    }
}