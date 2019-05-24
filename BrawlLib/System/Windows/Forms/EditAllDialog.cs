using BrawlLib.SSBB.ResourceNodes;
using System.Collections.Generic;
using System.Linq;

namespace System.Windows.Forms
{
    public class EditAllDialog : Form
    {
        private CHR0Node[] _nodes;
        private CHR0EntryNode[] _entries;
        private EditAllCHR0Editor editAllCHR0Editor1;
        private readonly CHR0EntryNode _copyNode = null;

        public EditAllDialog() { InitializeComponent(); }

        private void btnCancel_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }

        #region Designer

        private Button btnCancel;
        private Button btnOkay;

        private void InitializeComponent()
        {
            btnCancel = new System.Windows.Forms.Button();
            btnOkay = new System.Windows.Forms.Button();
            editAllCHR0Editor1 = new System.Windows.Forms.EditAllCHR0Editor();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(325, 344);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new System.EventHandler(btnCancel_Click);
            // 
            // btnOkay
            // 
            btnOkay.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            btnOkay.Location = new System.Drawing.Point(244, 344);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new System.Drawing.Size(75, 23);
            btnOkay.TabIndex = 1;
            btnOkay.Text = "&Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new System.EventHandler(btnOkay_Click);
            // 
            // editAllCHR0Editor1
            // 
            editAllCHR0Editor1.Dock = System.Windows.Forms.DockStyle.Top;
            editAllCHR0Editor1.Location = new System.Drawing.Point(0, 0);
            editAllCHR0Editor1.Name = "editAllCHR0Editor1";
            editAllCHR0Editor1.Size = new System.Drawing.Size(404, 338);
            editAllCHR0Editor1.TabIndex = 3;
            // 
            // EditAllDialog
            // 
            AcceptButton = btnOkay;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(404, 374);
            Controls.Add(editAllCHR0Editor1);
            Controls.Add(btnCancel);
            Controls.Add(btnOkay);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "EditAllDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Edit All Animations";
            ResumeLayout(false);

        }
        #endregion

        public bool[] _enabled = new bool[5];

        private void btnOkay_Click(object sender, EventArgs e)
        {
            //if (_enabled[0])
            if (_entries != null)
            {
                editAllCHR0Editor1.Apply(_entries);
            }
            else
            {
                editAllCHR0Editor1.Apply(_nodes);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        public void ShowDialog(IWin32Window owner, ResourceNode resource)
        {
            ShowDialog(owner, resource.FindChildrenByType(null, ResourceType.CHR0));
        }

        public void ShowDialog(IWin32Window owner, IEnumerable<ResourceNode> nodes)
        {
            _nodes = nodes
                .Select(n => n as CHR0Node)
                .Where(n => n != null)
                .ToArray();
            if (!_nodes.Any())
            {
                editAllCHR0Editor1.OnlyEntryNodesSelected();
                _entries = nodes
                    .Select(n => n as CHR0EntryNode)
                    .Where(n => n != null)
                    .ToArray();
            }
            _enabled = new bool[5];
            base.ShowDialog(owner);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //checkBox1.Checked = _enabled[tabControl1.SelectedIndex];
            //switch (tabControl1.SelectedIndex)
            //{
            //    case 0:
            //        editAllCHR0Editor1.Enabled = checkBox1.Checked;
            //        break;
            //    //case 1:
            //    //    editAllSRT0Editor1.Enabled = checkBox1.Checked;
            //    //    break;
            //    //case 2:
            //    //    editAllSHP0Editor1.Enabled = checkBox1.Checked;
            //    //    break;
            //    //case 3:
            //    //    editAllPAT0Editor1.Enabled = checkBox1.Checked;
            //    //    break;
            //    //case 4:
            //    //    editAllVIS0Editor1.Enabled = checkBox1.Checked;
            //    //    break;
            //}
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //_enabled[tabControl1.SelectedIndex] = checkBox1.Checked;
            //tabControl1_SelectedIndexChanged(null, null);
        }
    }
}
