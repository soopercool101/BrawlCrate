using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using System.Drawing;

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
            components = new System.ComponentModel.Container();
            dlgOpen = new System.Windows.Forms.OpenFileDialog();
            dlgSave = new System.Windows.Forms.SaveFileDialog();
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            pnlKeyframes = new System.Windows.Forms.Panel();
            pnlBones = new System.Windows.Forms.Panel();
            lstBones = new System.Windows.Forms.CheckedListBox();
            boneTree = new System.Windows.Forms.TreeView();
            panel1 = new System.Windows.Forms.Panel();
            txtSearchBone = new System.Windows.Forms.TextBox();
            chkContains = new System.Windows.Forms.CheckBox();
            chkFlat = new System.Windows.Forms.CheckBox();
            ctxBones = new System.Windows.Forms.ContextMenuStrip(components);
            nameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            boneIndex = new System.Windows.Forms.ToolStripMenuItem();
            renameBoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ctxBonesDivider1 = new System.Windows.Forms.ToolStripSeparator();
            addToParentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addToNextUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addToNextDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ctxBonesDivider2 = new System.Windows.Forms.ToolStripSeparator();
            moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            imageList1 = new System.Windows.Forms.ImageList(components);
            pnlKeyframes.SuspendLayout();
            pnlBones.SuspendLayout();
            panel1.SuspendLayout();
            ctxBones.SuspendLayout();
            SuspendLayout();
            // 
            // pnlKeyframes
            // 
            pnlKeyframes.AutoScroll = true;
            pnlKeyframes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            pnlKeyframes.Controls.Add(pnlBones);
            pnlKeyframes.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlKeyframes.Location = new System.Drawing.Point(0, 0);
            pnlKeyframes.Name = "pnlKeyframes";
            pnlKeyframes.Size = new System.Drawing.Size(164, 398);
            pnlKeyframes.TabIndex = 26;
            // 
            // pnlBones
            // 
            pnlBones.Controls.Add(lstBones);
            pnlBones.Controls.Add(boneTree);
            pnlBones.Controls.Add(panel1);
            pnlBones.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlBones.Location = new System.Drawing.Point(0, 0);
            pnlBones.Name = "pnlBones";
            pnlBones.Size = new System.Drawing.Size(160, 394);
            pnlBones.TabIndex = 10;
            // 
            // lstBones
            // 
            lstBones.Dock = System.Windows.Forms.DockStyle.Fill;
            lstBones.FormattingEnabled = true;
            lstBones.IntegralHeight = false;
            lstBones.Location = new System.Drawing.Point(0, 21);
            lstBones.Name = "lstBones";
            lstBones.Size = new System.Drawing.Size(160, 373);
            lstBones.TabIndex = 32;
            lstBones.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(lstBones_ItemCheck);
            lstBones.SelectedValueChanged += new System.EventHandler(lstBones_SelectedValueChanged);
            lstBones.KeyDown += new System.Windows.Forms.KeyEventHandler(lstBones_KeyDown);
            lstBones.MouseDown += new System.Windows.Forms.MouseEventHandler(lstBones_MouseDown);
            // 
            // boneTree
            // 
            boneTree.CheckBoxes = true;
            boneTree.Dock = System.Windows.Forms.DockStyle.Fill;
            boneTree.FullRowSelect = true;
            boneTree.HideSelection = false;
            boneTree.HotTracking = true;
            boneTree.Indent = 14;
            boneTree.ItemHeight = 16;
            boneTree.Location = new System.Drawing.Point(0, 21);
            boneTree.Name = "boneTree";
            boneTree.Size = new System.Drawing.Size(160, 373);
            boneTree.TabIndex = 29;
            boneTree.Visible = false;
            boneTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(boneTree_AfterCheck);
            boneTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(boneTree_AfterSelect);
            boneTree.MouseDown += new System.Windows.Forms.MouseEventHandler(lstBones_MouseDown);
            // 
            // panel1
            // 
            panel1.Controls.Add(txtSearchBone);
            panel1.Controls.Add(chkContains);
            panel1.Controls.Add(chkFlat);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(160, 21);
            panel1.TabIndex = 31;
            // 
            // txtSearchBone
            // 
            txtSearchBone.Dock = System.Windows.Forms.DockStyle.Fill;
            txtSearchBone.ForeColor = System.Drawing.Color.Gray;
            txtSearchBone.Location = new System.Drawing.Point(44, 0);
            txtSearchBone.Name = "txtSearchBone";
            txtSearchBone.Size = new System.Drawing.Size(46, 20);
            txtSearchBone.TabIndex = 30;
            txtSearchBone.Text = "Search for a bone...";
            txtSearchBone.Visible = false;
            txtSearchBone.TextChanged += new System.EventHandler(textBox1_TextChanged);
            txtSearchBone.Enter += new System.EventHandler(textBox1_Enter);
            txtSearchBone.Leave += new System.EventHandler(textBox1_Leave);
            // 
            // chkContains
            // 
            chkContains.AutoSize = true;
            chkContains.Dock = System.Windows.Forms.DockStyle.Right;
            chkContains.Location = new System.Drawing.Point(90, 0);
            chkContains.Margin = new System.Windows.Forms.Padding(0);
            chkContains.Name = "chkContains";
            chkContains.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            chkContains.Size = new System.Drawing.Size(70, 21);
            chkContains.TabIndex = 32;
            chkContains.Text = "Contains";
            chkContains.UseVisualStyleBackColor = false;
            chkContains.Visible = false;
            chkContains.CheckedChanged += new System.EventHandler(chkContains_CheckedChanged);
            // 
            // chkFlat
            // 
            chkFlat.AutoSize = true;
            chkFlat.Checked = true;
            chkFlat.CheckState = System.Windows.Forms.CheckState.Checked;
            chkFlat.Dock = System.Windows.Forms.DockStyle.Left;
            chkFlat.Location = new System.Drawing.Point(0, 0);
            chkFlat.Margin = new System.Windows.Forms.Padding(0);
            chkFlat.Name = "chkFlat";
            chkFlat.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            chkFlat.Size = new System.Drawing.Size(44, 21);
            chkFlat.TabIndex = 31;
            chkFlat.Text = "Flat";
            chkFlat.UseVisualStyleBackColor = false;
            chkFlat.CheckedChanged += new System.EventHandler(chkFlat_CheckedChanged);
            // 
            // ctxBones
            // 
            ctxBones.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            nameToolStripMenuItem,
            boneIndex,
            renameBoneToolStripMenuItem,
            ctxBonesDivider1,
            addToParentToolStripMenuItem,
            addToNextUpToolStripMenuItem,
            addToNextDownToolStripMenuItem,
            ctxBonesDivider2,
            moveUpToolStripMenuItem,
            moveDownToolStripMenuItem});
            ctxBones.Name = "ctxBones";
            ctxBones.Size = new System.Drawing.Size(175, 192);
            // 
            // nameToolStripMenuItem
            // 
            nameToolStripMenuItem.Enabled = false;
            nameToolStripMenuItem.Name = "nameToolStripMenuItem";
            nameToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            nameToolStripMenuItem.Text = "<name>";
            // 
            // boneIndex
            // 
            boneIndex.Enabled = false;
            boneIndex.Name = "boneIndex";
            boneIndex.Size = new System.Drawing.Size(174, 22);
            boneIndex.Text = "Bone Index";
            // 
            // renameBoneToolStripMenuItem
            // 
            renameBoneToolStripMenuItem.Name = "renameBoneToolStripMenuItem";
            renameBoneToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            renameBoneToolStripMenuItem.Text = "Rename";
            renameBoneToolStripMenuItem.Click += new System.EventHandler(renameBoneToolStripMenuItem_Click);
            // 
            // ctxBonesDivider1
            // 
            ctxBonesDivider1.Name = "ctxBonesDivider1";
            ctxBonesDivider1.Size = new System.Drawing.Size(171, 6);
            // 
            // addToParentToolStripMenuItem
            // 
            addToParentToolStripMenuItem.Name = "addToParentToolStripMenuItem";
            addToParentToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            addToParentToolStripMenuItem.Text = "Add To Parent";
            addToParentToolStripMenuItem.Click += new System.EventHandler(addToParentToolStripMenuItem_Click);
            // 
            // addToNextUpToolStripMenuItem
            // 
            addToNextUpToolStripMenuItem.Name = "addToNextUpToolStripMenuItem";
            addToNextUpToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            addToNextUpToolStripMenuItem.Text = "Add To Next Up";
            addToNextUpToolStripMenuItem.Click += new System.EventHandler(addToNextUpToolStripMenuItem_Click);
            // 
            // addToNextDownToolStripMenuItem
            // 
            addToNextDownToolStripMenuItem.Name = "addToNextDownToolStripMenuItem";
            addToNextDownToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            addToNextDownToolStripMenuItem.Text = "Add To Next Down";
            addToNextDownToolStripMenuItem.Click += new System.EventHandler(addToNextDownToolStripMenuItem_Click);
            // 
            // ctxBonesDivider2
            // 
            ctxBonesDivider2.Name = "ctxBonesDivider2";
            ctxBonesDivider2.Size = new System.Drawing.Size(171, 6);
            // 
            // moveUpToolStripMenuItem
            // 
            moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            moveUpToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            moveUpToolStripMenuItem.Text = "Move Up";
            moveUpToolStripMenuItem.Click += new System.EventHandler(moveUpToolStripMenuItem_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            moveDownToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            moveDownToolStripMenuItem.Text = "Move Down";
            moveDownToolStripMenuItem.Click += new System.EventHandler(moveDownToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageList1.ImageSize = new System.Drawing.Size(16, 16);
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // BonesPanel
            // 
            Controls.Add(pnlKeyframes);
            Name = "BonesPanel";
            Size = new System.Drawing.Size(164, 398);
            SizeChanged += new System.EventHandler(BonesPanel_SizeChanged);
            pnlKeyframes.ResumeLayout(false);
            pnlBones.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ctxBones.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        public BonesPanel() { InitializeComponent(); }

        public ModelEditorBase _mainWindow;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get => _mainWindow.TargetModel;
            set => _mainWindow.TargetModel = value;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBoneNode SelectedBone
        {
            get => _mainWindow.SelectedBone;
            set => _mainWindow.SelectedBone = value;
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
                {
                    foreach (IBoneNode bone in TargetModel.BoneCache)
                    {
                        lstBones.Items.Add(bone, bone.IsRendering);
                    }
                }

                lstBones.EndUpdate();
            }
            _updating = false;
        }

        private void ResetList()
        {
            string text = txtSearchBone.Text;
            bool addAll = string.IsNullOrEmpty(text) || txtSearchBone.ForeColor == Color.Gray;

            _updating = true;
            lstBones.BeginUpdate();
            lstBones.Items.Clear();

            if (TargetModel != null)
            {
                foreach (IBoneNode bone in TargetModel.BoneCache)
                {
                    if (addAll || (chkContains.Checked && bone.Name.Contains(text, StringComparison.OrdinalIgnoreCase)) || bone.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                    {
                        lstBones.Items.Add(bone, bone.IsRendering);
                    }
                }
            }

            lstBones.EndUpdate();
            _updating = false;
        }

        private void PopulateBoneTree()
        {
            if (TargetModel != null)
            {
                _treeNodes = new TreeNode[TargetModel.BoneCache.Length];
                foreach (IBoneNode bone in TargetModel.RootBones)
                {
                    RecursivePopulate(bone, boneTree.Nodes);
                }
            }

            boneTree.ExpandAll();
        }

        private void RecursivePopulate(IBoneNode bone, TreeNodeCollection nodes)
        {
            TreeNode node = new TreeNode() { Tag = bone, Text = bone.Name, Checked = bone.IsRendering };

            _treeNodes[bone.BoneIndex] = node;
            nodes.Add(node);

            foreach (IBoneNode b in ((ResourceNode)bone).Children)
            {
                RecursivePopulate(b, node.Nodes);
            }
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
                {
                    ContextMenuStrip = null;
                }
            }
        }

        private void lstBones_SelectedValueChanged(object sender, EventArgs e)
        {
            SetBone(lstBones.SelectedItem as IBoneNode);
        }
        private void SetBone(IBoneNode bone)
        {
            if (SelectedBone != null)
            {
                SelectedBone.BoneColor = SelectedBone.NodeColor = Color.Transparent;
            }

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
            {
                new RenameDialog().ShowDialog(this, (ResourceNode)SelectedBone);
            }
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
            if (txtSearchBone.Text == string.Empty)
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
            {
                SetBone(boneTree.SelectedNode.Tag as IBoneNode);
            }
        }

        public void SetSelectedBone(IBoneNode bone)
        {
            if (_treeNodes != null && bone != null && bone.BoneIndex < _treeNodes.Length)
            {
                boneTree.SelectedNode = _treeNodes[bone.BoneIndex];
            }

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
                {
                    parent = bone.Parent;
                }
                else
                {
                    return;
                }

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
                {
                    prev = bone.PrevNode;
                }
                else
                {
                    return;
                }

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
                {
                    next = bone.NextNode;
                }
                else
                {
                    return;
                }

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
                {
                    prev = bone.PrevVisibleNode;
                }
                else
                {
                    return;
                }

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
                {
                    next = bone.NextVisibleNode;
                }
                else
                {
                    return;
                }

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
            {
                if (Width < 190)
                {
                    chkContains.Visible = false;
                    txtSearchBone.Visible = Width > 90;
                }
                else
                {
                    chkContains.Visible = txtSearchBone.Visible = true;
                }
            }
        }

        private void BonesPanel_SizeChanged(object sender, EventArgs e)
        {
            UpdateListToolDisplay();
        }

        private void lstBones_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_updating)
            {
                return;
            }

            int i = e.Index;
            if (i < 0 || i >= TargetModel.BoneCache.Length)
            {
                return;
            }

            TargetModel.BoneCache[i].IsRendering = e.NewValue == CheckState.Checked;
            _mainWindow.ModelPanel.Invalidate();
        }

        private void boneTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (e.Node != null && e.Node.Tag is IBoneNode)
            {
                (e.Node.Tag as IBoneNode).IsRendering = e.Node.Checked;
                _mainWindow.ModelPanel.Invalidate();
            }
        }
    }
}