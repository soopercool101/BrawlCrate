using BrawlCrate.NodeWrappers;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Windows.Forms;

namespace BrawlCrate.UI
{
    internal class CloneSoundDialog : Form
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
            components = new System.ComponentModel.Container();
            label1 = new Label();
            txtName = new TextBox();
            btnOk = new Button();
            btnCancel = new Button();
            treeResource = new ResourceTree();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(48, 20);
            label1.TabIndex = 1;
            label1.Text = "Name:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                              | AnchorStyles.Right;
            txtName.Location = new System.Drawing.Point(66, 9);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(99, 20);
            txtName.TabIndex = 2;
            txtName.TextChanged += new EventHandler(txtName_TextChanged);
            // 
            // btnOk
            // 
            btnOk.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOk.Location = new System.Drawing.Point(171, 8);
            btnOk.Name = "btnOk";
            btnOk.Size = new System.Drawing.Size(63, 20);
            btnOk.TabIndex = 3;
            btnOk.Text = "Okay";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += new EventHandler(btnOk_Click);
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(240, 8);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(63, 20);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // treeResource
            // 
            treeResource.AllowContextMenus = false;
            treeResource.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                                   | AnchorStyles.Left
                                                   | AnchorStyles.Right;
            treeResource.HideSelection = false;
            treeResource.ImageIndex = 0;
            treeResource.Location = new System.Drawing.Point(12, 35);
            treeResource.Name = "treeResource";
            treeResource.SelectedImageIndex = 0;
            treeResource.ShowIcons = true;
            treeResource.Size = new System.Drawing.Size(291, 200);
            treeResource.TabIndex = 0;
            treeResource.SelectionChanged += new EventHandler(treeResource_SelectionChanged);
            // 
            // CloneSoundDialog
            // 
            ClientSize = new System.Drawing.Size(315, 247);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(txtName);
            Controls.Add(label1);
            Controls.Add(treeResource);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "CloneSoundDialog";
            Text = "Sound Cloner";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RSARFolderNode _parentNode;
        private RSARSoundNode _newNode;

        public CloneSoundDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IWin32Window owner, RSARFolderNode parent)
        {
            _parentNode = parent;
            _newNode = null;

            treeResource.BeginUpdate();
            foreach (ResourceNode node in parent.RSARNode.Children)
            {
                treeResource.Nodes.Add(BaseWrapper.Wrap(this, node));
            }

            BaseWrapper w = treeResource.FindResource(parent);
            treeResource.SelectedNode = w;
            w.EnsureVisible();
            w.Expand();

            treeResource.EndUpdate();

            try
            {
                return ShowDialog(owner);
            }
            finally
            {
                _parentNode = null;
                treeResource.Clear();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //Check name
            string name = txtName.Text;

            foreach (ResourceNode c in _parentNode.Children)
            {
                if (c.Name == name && !(c is RSARFolderNode))
                {
                    MessageBox.Show(this, "A resource with that name already exists!", "What the...");
                    return;
                }
            }

            _newNode = new RSARSoundNode
            {
                Name = name
            };
            _parentNode.AddChild(_newNode);

            RSARSoundNode existing = ((BaseWrapper) treeResource.SelectedNode)?.Resource as RSARSoundNode;
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

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckState()
        {
            if (txtName.Text != "")
            {
                GenericWrapper node = treeResource.SelectedNode as GenericWrapper;
                btnOk.Enabled = node?.Resource is RSARSoundNode;
            }
            else
            {
                btnOk.Enabled = false;
            }
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