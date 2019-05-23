using System;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using BrawlBox.NodeWrappers;

namespace BrawlBox
{
    class CloneSoundDialog : Form
    {
        #region Designer

        private ResourceTree treeResource;
        private Label label1;
        private TextBox txtName;
        private Button btnOk;
        private System.ComponentModel.IContainer components;
        private Button btnCancel;
    
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.treeResource = new BrawlBox.ResourceTree();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(66, 9);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(99, 20);
            this.txtName.TabIndex = 2;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(171, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(63, 20);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Okay";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(240, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(63, 20);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // treeResource
            // 
            this.treeResource.AllowContextMenus = false;
            this.treeResource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeResource.HideSelection = false;
            this.treeResource.ImageIndex = 0;
            this.treeResource.Location = new System.Drawing.Point(12, 35);
            this.treeResource.Name = "treeResource";
            this.treeResource.SelectedImageIndex = 0;
            this.treeResource.ShowIcons = true;
            this.treeResource.Size = new System.Drawing.Size(291, 200);
            this.treeResource.TabIndex = 0;
            this.treeResource.SelectionChanged += new System.EventHandler(this.treeResource_SelectionChanged);
            // 
            // CloneSoundDialog
            // 
            this.ClientSize = new System.Drawing.Size(315, 247);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeResource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "CloneSoundDialog";
            this.Text = "Sound Cloner";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RSARFolderNode _parentNode;
        private RSARSoundNode _newNode;

        public CloneSoundDialog() { InitializeComponent(); }

        public DialogResult ShowDialog(IWin32Window owner, RSARFolderNode parent)
        {
            _parentNode = parent;
            _newNode = null;

            treeResource.BeginUpdate();
            foreach (ResourceNode node in parent.RSARNode.Children)
                treeResource.Nodes.Add(BaseWrapper.Wrap(this, node));

            BaseWrapper w = treeResource.FindResource(parent);
            treeResource.SelectedNode = w;
            w.EnsureVisible();
            w.Expand();

            treeResource.EndUpdate();

            try { return base.ShowDialog(owner); }
            finally { _parentNode = null; treeResource.Clear(); }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //Check name
            string name = txtName.Text;

            foreach (ResourceNode c in _parentNode.Children)
                if ((c.Name == name) && !(c is RSARFolderNode))
                {
                    MessageBox.Show(this, "A resource with that name already exists!", "What the...");
                    return;
                }

            _newNode = new RSARSoundNode();
            _newNode.Name = name;
            _parentNode.AddChild(_newNode);

            if (treeResource.SelectedNode != null)
            {
                RSARSoundNode existing = ((BaseWrapper)treeResource.SelectedNode).Resource as RSARSoundNode;
                if (existing != null)
                {
                    _newNode._sound3dParam = existing._sound3dParam;
                    _newNode._waveInfo = existing._waveInfo;
                    _newNode._seqInfo = existing._seqInfo;
                    _newNode._strmInfo = existing._strmInfo;

                    _newNode._fileId = existing._fileId;
                    _newNode._playerId = existing._playerId;
                    _newNode._volume = existing._volume;
                    _newNode._playerPriority = existing._playerPriority;
                    _newNode._soundType = existing._soundType;
                    _newNode._remoteFilter = existing._remoteFilter;
                    _newNode._panMode = existing._panMode;
                    _newNode._panCurve = existing._panCurve;
                    _newNode._actorPlayerId = existing._actorPlayerId;
                }
            }

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckState()
        {
            if (txtName.Text != "")
            {
                GenericWrapper node = treeResource.SelectedNode as GenericWrapper;
                btnOk.Enabled = (node != null) && (node.Resource is RSARSoundNode);
            }
            else
                btnOk.Enabled = false;
        }

        private void treeResource_SelectionChanged(object sender, EventArgs e)
        {
            CheckState();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            CheckState();
        }
    }
}
