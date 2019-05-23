using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;
using BrawlLib.Modeling;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class BonesPanel : UserControl
    {
        public delegate void ReferenceEventHandler(ResourceNode node);

        #region Designer

        private OpenFileDialog dlgOpen;
        private SaveFileDialog dlgSave;
        private IContainer components;
        private FolderBrowserDialog folderBrowserDialog1;
        private Panel pnlKeyframes;
        private ImageList imageList1;
        private ContextMenuStrip ctxBones;
        private ToolStripMenuItem boneIndex;
        private ToolStripMenuItem renameBoneToolStripMenuItem;
        private TreeView boneTree;
        private Panel panel1;
        private TextBox txtSearchBone;
        public CheckBox chkFlat;
        private CheckedListBox lstBones;
        public CheckBox chkContains;
        private ToolStripSeparator ctxBonesDivider1;
        private ToolStripMenuItem addToParentToolStripMenuItem;
        private ToolStripMenuItem addToNextUpToolStripMenuItem;
        private ToolStripMenuItem addToNextDownToolStripMenuItem;
        private ToolStripSeparator ctxBonesDivider2;
        private ToolStripMenuItem moveUpToolStripMenuItem;
        private ToolStripMenuItem moveDownToolStripMenuItem;
        private ToolStripMenuItem nameToolStripMenuItem;
        private Panel pnlBones;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.pnlKeyframes = new System.Windows.Forms.Panel();
            this.pnlBones = new System.Windows.Forms.Panel();
            this.lstBones = new System.Windows.Forms.CheckedListBox();
            this.boneTree = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSearchBone = new System.Windows.Forms.TextBox();
            this.chkContains = new System.Windows.Forms.CheckBox();
            this.chkFlat = new System.Windows.Forms.CheckBox();
            this.ctxBones = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.nameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boneIndex = new System.Windows.Forms.ToolStripMenuItem();
            this.renameBoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxBonesDivider1 = new System.Windows.Forms.ToolStripSeparator();
            this.addToParentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToNextUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToNextDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxBonesDivider2 = new System.Windows.Forms.ToolStripSeparator();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnlKeyframes.SuspendLayout();
            this.pnlBones.SuspendLayout();
            this.panel1.SuspendLayout();
            this.ctxBones.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlKeyframes
            // 
            this.pnlKeyframes.AutoScroll = true;
            this.pnlKeyframes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlKeyframes.Controls.Add(this.pnlBones);
            this.pnlKeyframes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKeyframes.Location = new System.Drawing.Point(0, 0);
            this.pnlKeyframes.Name = "pnlKeyframes";
            this.pnlKeyframes.Size = new System.Drawing.Size(164, 398);
            this.pnlKeyframes.TabIndex = 26;
            // 
            // pnlBones
            // 
            this.pnlBones.Controls.Add(this.lstBones);
            this.pnlBones.Controls.Add(this.boneTree);
            this.pnlBones.Controls.Add(this.panel1);
            this.pnlBones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBones.Location = new System.Drawing.Point(0, 0);
            this.pnlBones.Name = "pnlBones";
            this.pnlBones.Size = new System.Drawing.Size(160, 394);
            this.pnlBones.TabIndex = 10;
            // 
            // lstBones
            // 
            this.lstBones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstBones.FormattingEnabled = true;
            this.lstBones.IntegralHeight = false;
            this.lstBones.Location = new System.Drawing.Point(0, 21);
            this.lstBones.Name = "lstBones";
            this.lstBones.Size = new System.Drawing.Size(160, 373);
            this.lstBones.TabIndex = 32;
            this.lstBones.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstBones_ItemCheck);
            this.lstBones.SelectedValueChanged += new System.EventHandler(this.lstBones_SelectedValueChanged);
            this.lstBones.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstBones_KeyDown);
            this.lstBones.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstBones_MouseDown);
            // 
            // boneTree
            // 
            this.boneTree.CheckBoxes = true;
            this.boneTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boneTree.FullRowSelect = true;
            this.boneTree.HideSelection = false;
            this.boneTree.HotTracking = true;
            this.boneTree.Indent = 14;
            this.boneTree.ItemHeight = 16;
            this.boneTree.Location = new System.Drawing.Point(0, 21);
            this.boneTree.Name = "boneTree";
            this.boneTree.Size = new System.Drawing.Size(160, 373);
            this.boneTree.TabIndex = 29;
            this.boneTree.Visible = false;
            this.boneTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.boneTree_AfterCheck);
            this.boneTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.boneTree_AfterSelect);
            this.boneTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstBones_MouseDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSearchBone);
            this.panel1.Controls.Add(this.chkContains);
            this.panel1.Controls.Add(this.chkFlat);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(160, 21);
            this.panel1.TabIndex = 31;
            // 
            // txtSearchBone
            // 
            this.txtSearchBone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearchBone.ForeColor = System.Drawing.Color.Gray;
            this.txtSearchBone.Location = new System.Drawing.Point(44, 0);
            this.txtSearchBone.Name = "txtSearchBone";
            this.txtSearchBone.Size = new System.Drawing.Size(46, 20);
            this.txtSearchBone.TabIndex = 30;
            this.txtSearchBone.Text = "Search for a bone...";
            this.txtSearchBone.Visible = false;
            this.txtSearchBone.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.txtSearchBone.Enter += new System.EventHandler(this.textBox1_Enter);
            this.txtSearchBone.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // chkContains
            // 
            this.chkContains.AutoSize = true;
            this.chkContains.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkContains.Location = new System.Drawing.Point(90, 0);
            this.chkContains.Margin = new System.Windows.Forms.Padding(0);
            this.chkContains.Name = "chkContains";
            this.chkContains.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.chkContains.Size = new System.Drawing.Size(70, 21);
            this.chkContains.TabIndex = 32;
            this.chkContains.Text = "Contains";
            this.chkContains.UseVisualStyleBackColor = false;
            this.chkContains.Visible = false;
            this.chkContains.CheckedChanged += new System.EventHandler(this.chkContains_CheckedChanged);
            // 
            // chkFlat
            // 
            this.chkFlat.AutoSize = true;
            this.chkFlat.Checked = true;
            this.chkFlat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFlat.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkFlat.Location = new System.Drawing.Point(0, 0);
            this.chkFlat.Margin = new System.Windows.Forms.Padding(0);
            this.chkFlat.Name = "chkFlat";
            this.chkFlat.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkFlat.Size = new System.Drawing.Size(44, 21);
            this.chkFlat.TabIndex = 31;
            this.chkFlat.Text = "Flat";
            this.chkFlat.UseVisualStyleBackColor = false;
            this.chkFlat.CheckedChanged += new System.EventHandler(this.chkFlat_CheckedChanged);
            // 
            // ctxBones
            // 
            this.ctxBones.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nameToolStripMenuItem,
            this.boneIndex,
            this.renameBoneToolStripMenuItem,
            this.ctxBonesDivider1,
            this.addToParentToolStripMenuItem,
            this.addToNextUpToolStripMenuItem,
            this.addToNextDownToolStripMenuItem,
            this.ctxBonesDivider2,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem});
            this.ctxBones.Name = "ctxBones";
            this.ctxBones.Size = new System.Drawing.Size(175, 192);
            // 
            // nameToolStripMenuItem
            // 
            this.nameToolStripMenuItem.Enabled = false;
            this.nameToolStripMenuItem.Name = "nameToolStripMenuItem";
            this.nameToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.nameToolStripMenuItem.Text = "<name>";
            // 
            // boneIndex
            // 
            this.boneIndex.Enabled = false;
            this.boneIndex.Name = "boneIndex";
            this.boneIndex.Size = new System.Drawing.Size(174, 22);
            this.boneIndex.Text = "Bone Index";
            // 
            // renameBoneToolStripMenuItem
            // 
            this.renameBoneToolStripMenuItem.Name = "renameBoneToolStripMenuItem";
            this.renameBoneToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.renameBoneToolStripMenuItem.Text = "Rename";
            this.renameBoneToolStripMenuItem.Click += new System.EventHandler(this.renameBoneToolStripMenuItem_Click);
            // 
            // ctxBonesDivider1
            // 
            this.ctxBonesDivider1.Name = "ctxBonesDivider1";
            this.ctxBonesDivider1.Size = new System.Drawing.Size(171, 6);
            // 
            // addToParentToolStripMenuItem
            // 
            this.addToParentToolStripMenuItem.Name = "addToParentToolStripMenuItem";
            this.addToParentToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.addToParentToolStripMenuItem.Text = "Add To Parent";
            this.addToParentToolStripMenuItem.Click += new System.EventHandler(this.addToParentToolStripMenuItem_Click);
            // 
            // addToNextUpToolStripMenuItem
            // 
            this.addToNextUpToolStripMenuItem.Name = "addToNextUpToolStripMenuItem";
            this.addToNextUpToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.addToNextUpToolStripMenuItem.Text = "Add To Next Up";
            this.addToNextUpToolStripMenuItem.Click += new System.EventHandler(this.addToNextUpToolStripMenuItem_Click);
            // 
            // addToNextDownToolStripMenuItem
            // 
            this.addToNextDownToolStripMenuItem.Name = "addToNextDownToolStripMenuItem";
            this.addToNextDownToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.addToNextDownToolStripMenuItem.Text = "Add To Next Down";
            this.addToNextDownToolStripMenuItem.Click += new System.EventHandler(this.addToNextDownToolStripMenuItem_Click);
            // 
            // ctxBonesDivider2
            // 
            this.ctxBonesDivider2.Name = "ctxBonesDivider2";
            this.ctxBonesDivider2.Size = new System.Drawing.Size(171, 6);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // BonesPanel
            // 
            this.Controls.Add(this.pnlKeyframes);
            this.Name = "BonesPanel";
            this.Size = new System.Drawing.Size(164, 398);
            this.SizeChanged += new System.EventHandler(this.BonesPanel_SizeChanged);
            this.pnlKeyframes.ResumeLayout(false);
            this.pnlBones.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ctxBones.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public BonesPanel() { InitializeComponent(); }

        public ModelEditorBase _mainWindow;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get { return _mainWindow.TargetModel; }
            set { _mainWindow.TargetModel = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBoneNode SelectedBone
        {
            get { return _mainWindow.SelectedBone; }
            set { _mainWindow.SelectedBone = value; }
        }

        public TreeNode[] _treeNodes;

        public bool _updating;
        public void Reset()
        {
            _updating = true;
            //if (!chkFlat.Checked)
            {
                boneTree.BeginUpdate();
                boneTree.Nodes.Clear();

                PopulateBoneTree();

                boneTree.EndUpdate();
            }
            //else
            {
                lstBones.BeginUpdate();
                lstBones.Items.Clear();

                if (TargetModel != null)
                    foreach (IBoneNode bone in TargetModel.BoneCache)
                        lstBones.Items.Add(bone, bone.IsRendering);

                lstBones.EndUpdate();
            }
            _updating = false;
        }

        private void ResetList()
        {
            string text = txtSearchBone.Text;
            bool addAll = String.IsNullOrEmpty(text) || txtSearchBone.ForeColor == Color.Gray;

            _updating = true;
            lstBones.BeginUpdate();
            lstBones.Items.Clear();

            if (TargetModel != null)
                foreach (IBoneNode bone in TargetModel.BoneCache)
                    if (addAll || (chkContains.Checked && bone.Name.Contains(text, StringComparison.OrdinalIgnoreCase)) || bone.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                        lstBones.Items.Add(bone, bone.IsRendering);

            lstBones.EndUpdate();
            _updating = false;
        }

        private void PopulateBoneTree()
        {
            if (TargetModel != null)
            {
                _treeNodes = new TreeNode[TargetModel.BoneCache.Length];
                foreach (IBoneNode bone in TargetModel.RootBones)
                    RecursivePopulate(bone, boneTree.Nodes);
            }

            boneTree.ExpandAll();
        }

        private void RecursivePopulate(IBoneNode bone, TreeNodeCollection nodes)
        {
            TreeNode node = new TreeNode() { Tag = bone, Text = bone.Name, Checked = bone.IsRendering };

            _treeNodes[bone.BoneIndex] = node;
            nodes.Add(node);

            foreach (IBoneNode b in ((ResourceNode)bone).Children)
                RecursivePopulate(b, node.Nodes);
        }

        private void lstBones_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (SelectedBone != null)
                {
                    ContextMenuStrip = ctxBones;
                    nameToolStripMenuItem.Text = SelectedBone.Name;
                    boneIndex.Text = "Bone Index: " + SelectedBone.BoneIndex.ToString();
                }
                else
                    ContextMenuStrip = null;
            }
        }

        private void lstBones_SelectedValueChanged(object sender, EventArgs e)
        {
            SetBone(lstBones.SelectedItem as IBoneNode);
        }
        private void SetBone(IBoneNode bone)
        {
            if (SelectedBone != null)
                SelectedBone.BoneColor = SelectedBone.NodeColor = Color.Transparent;

            SelectedBone = bone;

            _mainWindow.ModelPanel.Invalidate();
        }

        private void lstBones_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                lstBones.SelectedItem = null;
                boneTree.SelectedNode = null;
            }
        }

        private void renameBoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedBone != null)
                new RenameDialog().ShowDialog(this, (ResourceNode)SelectedBone);
        }

        private void chkFlat_CheckedChanged(object sender, EventArgs e)
        {
            ctxBonesDivider1.Visible =
                ctxBonesDivider2.Visible =
                moveUpToolStripMenuItem.Visible =
                moveDownToolStripMenuItem.Visible =
                addToNextDownToolStripMenuItem.Visible =
                addToNextUpToolStripMenuItem.Visible =
                addToParentToolStripMenuItem.Visible =
                boneTree.Visible = !(txtSearchBone.Visible = chkContains.Visible = lstBones.Visible = chkFlat.Checked);

            UpdateListToolDisplay();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (txtSearchBone.ForeColor == Color.Gray)
            {
                txtSearchBone.Text = "";
                txtSearchBone.Font = new Font(txtSearchBone.Font, Drawing.FontStyle.Regular);
                txtSearchBone.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (txtSearchBone.Text == String.Empty)
            {
                txtSearchBone.Font = new Font(txtSearchBone.Font, Drawing.FontStyle.Italic);
                txtSearchBone.ForeColor = Color.Gray;
                txtSearchBone.Text = "Search for a bone...";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ResetList();
        }

        private void boneTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!chkFlat.Checked && boneTree.SelectedNode != null)
                SetBone(boneTree.SelectedNode.Tag as IBoneNode);
        }

        public void SetSelectedBone(IBoneNode bone)
        {
            if (_treeNodes != null && bone != null && bone.BoneIndex < _treeNodes.Length)
                boneTree.SelectedNode = _treeNodes[bone.BoneIndex];

            lstBones.SelectedItem = bone;
        }

        private void chkContains_CheckedChanged(object sender, EventArgs e)
        {
            ResetList();
        }

        private void addToParentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceNode node = SelectedBone as ResourceNode;
            if (node != null && node.ToParent())
            {
                TreeNode bone = _treeNodes[SelectedBone.BoneIndex], parent = null;
                if (bone != null && bone.Parent != null)
                    parent = bone.Parent;
                else
                    return;

                boneTree.BeginUpdate();
                bone.Remove();
                parent.Parent.Nodes.Add(bone);
                node.Parent = node.Parent.Parent;
                node.OnMoved();
                boneTree.EndUpdate();
                bone.EnsureVisible();
                _mainWindow.ModelPanel.Invalidate();
            }
        }

        private void addToNextUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceNode node = SelectedBone as ResourceNode;
            if (node != null && node.AddUp())
            {
                TreeNode bone = _treeNodes[SelectedBone.BoneIndex], prev = null;
                if (bone != null && bone.PrevNode != null)
                    prev = bone.PrevNode;
                else
                    return;

                boneTree.BeginUpdate();
                node.Parent = node.Parent.Children[node.Index - 1];
                bone.Remove();
                prev.Nodes.Add(bone);
                node.OnMoved();
                boneTree.EndUpdate();
                bone.EnsureVisible();
                _mainWindow.ModelPanel.Invalidate();
            }
        }

        private void addToNextDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceNode node = SelectedBone as ResourceNode;
            if (node != null && node.AddDown())
            {
                TreeNode bone = _treeNodes[SelectedBone.BoneIndex], next = null;
                if (bone != null && bone.NextNode != null)
                    next = bone.NextNode;
                else
                    return;

                boneTree.BeginUpdate();
                node.Parent = node.Parent.Children[node.Index + 1];
                bone.Remove();
                next.Nodes.Add(bone);
                node.OnMoved();
                boneTree.EndUpdate();
                bone.EnsureVisible();
                _mainWindow.ModelPanel.Invalidate();
            }
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceNode node = SelectedBone as ResourceNode;
            if (node != null && node.MoveUp())
            {
                TreeNode bone = _treeNodes[SelectedBone.BoneIndex], prev = null;
                if (bone != null && bone.PrevVisibleNode != null)
                    prev = bone.PrevVisibleNode;
                else
                    return;

                int index = bone.Index - 1;
                TreeNode parent = bone.Parent;
                boneTree.BeginUpdate();
                bone.Remove();
                parent.Nodes.Insert(index, bone);
                node.OnMoved();
                boneTree.SelectedNode = bone;
                boneTree.EndUpdate();
                _mainWindow.ModelPanel.Invalidate();
            }
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceNode node = SelectedBone as ResourceNode;
            if (node != null && node.MoveDown())
            {
                TreeNode bone = _treeNodes[SelectedBone.BoneIndex], next = null;
                if (bone != null && bone.NextVisibleNode != null)
                    next = bone.NextVisibleNode;
                else
                    return;

                int index = bone.Index + 1;
                TreeNode parent = bone.Parent;
                boneTree.BeginUpdate();
                bone.Remove();
                parent.Nodes.Insert(index, bone);
                node.OnMoved();
                boneTree.SelectedNode = bone;
                boneTree.EndUpdate();
                _mainWindow.ModelPanel.Invalidate();
            }
        }

        private void UpdateListToolDisplay()
        {
            if (lstBones.Visible)
                if (Width < 190)
                {
                    chkContains.Visible = false;
                    txtSearchBone.Visible = Width > 90;
                }
                else
                    chkContains.Visible = txtSearchBone.Visible = true;
        }

        private void BonesPanel_SizeChanged(object sender, EventArgs e)
        {
            UpdateListToolDisplay();
        }

        private void lstBones_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_updating)
                return;

            int i = e.Index;
            if (i < 0 || i >= TargetModel.BoneCache.Length)
                return;

            TargetModel.BoneCache[i].IsRendering = e.NewValue == CheckState.Checked;
            _mainWindow.ModelPanel.Invalidate();
        }

        private void boneTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_updating)
                return;

            if (e.Node != null && e.Node.Tag is IBoneNode)
            {
                (e.Node.Tag as IBoneNode).IsRendering = e.Node.Checked;
                _mainWindow.ModelPanel.Invalidate();
            }
        }
    }
}