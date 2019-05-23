using BrawlLib.SSBB.ResourceNodes;

namespace System.Windows.Forms
{
    public class RenameDialog : Form
    {
        private ResourceNode _node;

        public RenameDialog() { InitializeComponent(); }

        public DialogResult ShowDialog(IWin32Window owner, ResourceNode node)
        {
            _node = node;

            if (_node is ARCNode)
                txtName.MaxLength = 47;
            else
                txtName.MaxLength = 255;

            txtName.Text = node.Name;

            try { return base.ShowDialog(owner); }
            finally { _node = null; }
        }
        private unsafe void btnOkay_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            if (name.Length == 0)
                name = "<null>";

            if (name.Equals("<null>", StringComparison.OrdinalIgnoreCase))
            {
                if (!_node.AllowNullNames)
                {
                    MessageBox.Show(this, "Name cannot be null!", "What the...");
                    return;
                }
            }
            else if ((!_node.AllowDuplicateNames) && (_node.Parent != null))
            {
                //No duplicates
                foreach (ResourceNode c in _node.Parent.Children)
                {
                    if ((c.Name == name) && (c.GetType() == _node.GetType()) && (c != _node))
                    {
                        MessageBox.Show(this, "A resource with that name already exists!", "What the...");
                        return;
                    }
                }
            }

            //Also change palette node
            if (_node is TEX0Node)
            {
                PLT0Node plt = ((TEX0Node)_node).GetPaletteNode();
                if (plt != null)
                    plt.Name = name;
            }

            _node.Name = name;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }


        #region Designer

        private TextBox txtName;
        private Button btnCancel;
        private Button btnOkay;

        private void InitializeComponent()
        {
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.HideSelection = false;
            this.txtName.Location = new System.Drawing.Point(12, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(260, 20);
            this.txtName.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(197, 38);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.Location = new System.Drawing.Point(116, 38);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 1;
            this.btnOkay.Text = "&Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // RenameDialog
            // 
            this.AcceptButton = this.btnOkay;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 69);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RenameDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rename Node";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


    }
}
