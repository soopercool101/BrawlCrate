using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    public partial class RSARGroupEditor : UserControl
    {
        public RSARGroupEditor()
        {
            InitializeComponent();
        }

        private RSARGroupNode _targetGroup;

        public void LoadGroup(RSARGroupNode group)
        {
            if ((_targetGroup = group) != null)
            {
                lstFiles.DataSource = group._files;
                cboAllFiles.DataSource = group.RSARNode.Files;
            }
            else
            {
                lstFiles.DataSource = null;
                cboAllFiles.DataSource = null;
            }
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sIndex = lstFiles.SelectedIndex;
            int count = lstFiles.Items.Count;
            btnMoveDown.Enabled = sIndex < count - 1 && sIndex >= 0;
            btnMoveUp.Enabled = sIndex > 0 && sIndex < count;
            btnRemove.Enabled = btnEdit.Enabled = sIndex >= 0 && sIndex < count;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cboAllFiles.SelectedIndex != -1)
            {
                RSARFileNode file = cboAllFiles.Items[cboAllFiles.SelectedIndex] as RSARFileNode;
                file._groupRefs.Add(_targetGroup);
                _targetGroup._files.Add(file);
                file.GetName();
                lstFiles.SelectedIndex = lstFiles.Items.Count - 1;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedIndex != -1)
            {
                RSARFileNode file = cboAllFiles.Items[cboAllFiles.SelectedIndex] as RSARFileNode;
                file._groupRefs.RemoveAt(lstFiles.SelectedIndex);
                _targetGroup._files.RemoveAt(lstFiles.SelectedIndex);
                file.GetName();
                lstFiles_SelectedIndexChanged(null, null);
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            int sIndex = lstFiles.SelectedIndex;
            RSARFileNode file = _targetGroup._files[sIndex];
            _targetGroup._files.Insert(sIndex - 1, file);
            _targetGroup._files.RemoveAt(sIndex + 1);
            lstFiles.SelectedIndex = sIndex - 1;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            int sIndex = lstFiles.SelectedIndex;
            RSARFileNode file = _targetGroup._files[sIndex];
            _targetGroup._files.RemoveAt(sIndex);
            _targetGroup._files.Insert(sIndex + 1, file);
            lstFiles.SelectedIndex = sIndex + 1;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            RSARFileNode file = lstFiles.SelectedItem as RSARFileNode;
            if (file is RSARExtFileNode)
            {
                RSARExtFileNode m = file as RSARExtFileNode;
                if (File.Exists(m.FullExtPath))
                {
                    Process.Start(m.FullExtPath);
                }
                else
                {
                    using (SoundPathChanger dlg = new SoundPathChanger())
                    {
                        RSARNode rsar = m.RSARNode;
                        dlg.FilePath = m.FullExtPath;
                        dlg.dlg.InitialDirectory = rsar._origPath.Substring(0, rsar._origPath.LastIndexOf('\\'));
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            m.FullExtPath = dlg.FilePath;
                        }
                    }
                }
            }
            else
            {
                new EditRSARFileDialog().ShowDialog(this, file);
            }
        }
    }
}