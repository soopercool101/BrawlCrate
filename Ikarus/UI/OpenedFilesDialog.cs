using BrawlLib.SSBB.ResourceNodes;
using Ikarus;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace System.Windows.Forms
{
    public partial class OpenedFilesDialog : Form
    {
        public OpenedFilesDialog()
        {
            InitializeComponent();
            listBox1.DataSource = Program.OpenedFilePaths;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Process.Start(Program.RootPath + listBox1.SelectedItem as string);
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.X, e.Y);
            if (listBox1.SelectedIndex != index)
                listBox1.SelectedIndex = index;

            if (e.Button == MouseButtons.Right)
                if (listBox1.SelectedIndex >= 0)
                    listBox1.ContextMenuStrip = ctxFile;
                else
                    listBox1.ContextMenuStrip = null;
        }

        private void ctxFile_Opening(object sender, CancelEventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                e.Cancel = true;
            else
                saveToolStripMenuItem.Enabled = Program.OpenedFiles[listBox1.SelectedIndex].IsDirty;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                label1.Text = "";
                return;
            }

            string s = Path.GetFileName(listBox1.SelectedItem.ToString());
            label1.Text = String.Format("{0} - Has {1}changed", s, Program.OpenedFiles[listBox1.SelectedIndex].IsDirty ? "" : "not ");
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                listBox1.SelectedIndex = -1;
        }

        private bool Save(ResourceNode r)
        {
            if (r._origPath == null)
                return SaveAs(r);
            
            r.Merge(Control.ModifierKeys == (Keys.Control | Keys.Shift));
            r.Export(r._origPath);
            r.IsDirty = false;
            return true;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceNode r = Program.OpenedFiles[listBox1.SelectedIndex];
            if (MessageBox.Show(this, String.Format("Are you sure you want to save {0}?", Path.GetFileName(r._origPath)), "Are you sure?", MessageBoxButtons.OKCancel) != Forms.DialogResult.OK)
                return;

            Save(r);
        }

        private bool SaveAs(ResourceNode r)
        {
            return true;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs(Program.OpenedFiles[listBox1.SelectedIndex]);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceNode r = Program.OpenedFiles[listBox1.SelectedIndex];
            if (r.IsDirty)
            {
                DialogResult res = MessageBox.Show("Save changes?", "Closing", MessageBoxButtons.YesNoCancel);
                if ((res == DialogResult.Yes && !Save(r)) || res == DialogResult.Cancel)
                    return;
            }
            Manager.RemoveFile(Program.OpenedFiles[listBox1.SelectedIndex]);
        }
    }
}