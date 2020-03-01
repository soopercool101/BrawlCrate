using BrawlLib.Internal.Windows.Controls.EditAllDialog;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    public class EditAllDialog : Form
    {
        private CHR0Node[] _nodes;
        private CHR0EntryNode[] _entries;
        private EditAllCHR0Editor editAllCHR0Editor1;

        public EditAllDialog()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #region Designer

        private Button btnCancel;
        private Button btnOkay;

        private void InitializeComponent()
        {
            btnCancel = new Button();
            btnOkay = new Button();
            editAllCHR0Editor1 = new EditAllCHR0Editor();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Right);
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(325, 344);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // btnOkay
            // 
            btnOkay.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Right);
            btnOkay.Location = new System.Drawing.Point(244, 344);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new System.Drawing.Size(75, 23);
            btnOkay.TabIndex = 1;
            btnOkay.Text = "&Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new EventHandler(btnOkay_Click);
            // 
            // editAllCHR0Editor1
            // 
            editAllCHR0Editor1.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Bottom
                                                                         | AnchorStyles.Left
                                                                         | AnchorStyles.Right);
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
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "EditAllDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
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
            ShowDialog(owner);
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