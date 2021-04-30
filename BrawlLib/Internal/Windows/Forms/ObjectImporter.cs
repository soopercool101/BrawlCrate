using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Models;
using System;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    public class ObjectImporter : Form
    {
        private MDL0Node _internalModel;
        private Label label1;
        private ComboBox comboBox1;
        private Label label2;
        private ComboBox comboBox2;
        private MDL0Node _externalModel;
        private MDL0ObjectNode _node;
        private IMatrixNode _baseInf;
        private ComboBox comboBox3;
        private CheckBox checkBox1;
        private MDL0BoneNode _parent;
        private Label label3;
        private Label label4;
        private Label baseBone;
        private ModelPanel modelPanel1;
        private Panel panel1;
        private bool _mergeModels;

        public ObjectImporter()
        {
            InitializeComponent();
            //modelPanel1.AddViewport(ModelPanelViewport.DefaultPerspective);
        }

        public DialogResult ShowDialog(MDL0Node internalModel, MDL0Node externalModel)
        {
            if (internalModel?._linker?.BoneCache == null || internalModel._linker.BoneCache.Length == 0)
            {
                MessageBox.Show("The target model must have at least one bone.", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return DialogResult.Cancel;
            }
            ResourceNode[] objects = externalModel.FindChild("Objects", true)?.Children.ToArray();
            if (objects == null || objects.Length == 0)
            {
                MessageBox.Show("The imported model must have at least one object.", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return DialogResult.Cancel;
            }
            _internalModel = internalModel;
            _externalModel = externalModel;

            comboBox1.Items.AddRange(objects);
            comboBox2.Items.AddRange(_internalModel._linker.BoneCache);

            comboBox1.SelectedIndex = comboBox2.SelectedIndex = comboBox3.SelectedIndex = 0;
            _parent = (MDL0BoneNode) comboBox2.SelectedItem;

            return ShowDialog(null);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _externalModel.ResetToBindState();

            modelPanel1.ClearTargets();
            modelPanel1.AddReference(_internalModel);
            GetBaseInfluence();
        }

        private void MergeChildren(MDL0BoneNode parent, MDL0BoneNode child, ResourceNode res)
        {
            bool found = false;
            MDL0BoneNode bone = null;
            foreach (MDL0BoneNode b1 in parent.Children)
            {
                if (b1.Name == child.Name)
                {
                    found = true;
                    bone = b1;
                    foreach (MDL0BoneNode b in child.Children)
                    {
                        MergeChildren(b1, b, res);
                    }

                    break;
                }
            }

            if (!found)
            {
                MDL0BoneNode b = child.Clone();
                parent.InsertChild(b, true, child.Index);
                bone = b;
            }
            else
            {
                found = false;
            }

            if (res is MDL0ObjectNode)
            {
                MDL0ObjectNode poly = res as MDL0ObjectNode;
                foreach (Vertex3 v in poly._manager._vertices)
                {
                    if (v.MatrixNode == child)
                    {
                        v.MatrixNode = bone;
                    }
                }
            }
            else if (res is MDL0Node)
            {
                MDL0Node mdl = res as MDL0Node;
                foreach (MDL0ObjectNode poly in mdl.FindChild("Objects", true).Children)
                {
                    foreach (Vertex3 v in poly._manager._vertices)
                    {
                        if (v.MatrixNode == child)
                        {
                            v.MatrixNode = bone;
                        }
                    }
                }
            }
        }

        private void ImportObject(MDL0ObjectNode node)
        {
            _internalModel.ReplaceOrAddMesh(node, false, false, true);
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    foreach (MDL0BoneNode b in (_baseInf as MDL0BoneNode).Children)
                    {
                        MergeChildren(_parent, b, _mergeModels ? _externalModel : (ResourceNode) _node);
                    }

                    break;
                case 1:
                    _parent.Parent.InsertChild((ResourceNode) _baseInf, true, _parent.Index);
                    _parent.Remove();
                    break;
                case 2:
                    _parent.AddChild((ResourceNode) _baseInf);
                    break;
            }

            _internalModel.ApplyCHR(null, 0);
            if (_mergeModels)
            {
                _externalModel.ApplyCHR(null, 0);
                foreach (ResourceNode node in _externalModel?.FindChild("Objects", true)?.Children?.ToArray() ??
                                              new ResourceNode[0])
                {
                    if (node is MDL0ObjectNode poly)
                    {
                        ImportObject(poly);
                    }
                }
            }
            else
            {
                _node.Model.ApplyCHR(null, 0);
                ImportObject(_node);
            }

            _internalModel.SignalPropertyChange();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void GetBaseInfluence()
        {
            if (_node != null)
            {
                modelPanel1.RemoveReference(_node);
            }

            MDL0BoneNode[] boneCache = _externalModel._linker.BoneCache;
            if ((_node = (MDL0ObjectNode) comboBox1.SelectedItem).Weighted)
            {
                int least = int.MaxValue;
                foreach (IMatrixNode inf in _node.Influences)
                {
                    if (inf is MDL0BoneNode && ((MDL0BoneNode) inf).BoneIndex < least)
                    {
                        least = ((MDL0BoneNode) inf).BoneIndex;
                    }
                }

                if (least != int.MaxValue)
                {
                    MDL0BoneNode temp = boneCache[least];
                    if (temp?.Parent is IMatrixNode im)
                    {
                        _baseInf = im;
                    }
                    else if (temp is IMatrixNode im2)
                    {
                        _baseInf = im2;
                    }
                }
            }
            else
            {
                _baseInf = _node.MatrixNode;
            }

            if (_baseInf is Influence)
            {
                label2.Hide();
                comboBox2.Hide();
            }
            else if (_baseInf is MDL0BoneNode)
            {
                label2.Show();
                comboBox2.Show();
            }

            baseBone.Text = _baseInf.ToString();

            if (comboBox3.SelectedIndex == 0 && _baseInf is MDL0BoneNode)
            {
                int i = 0;
                foreach (MDL0BoneNode s in comboBox2.Items)
                {
                    if (s.Name == baseBone.Text)
                    {
                        comboBox2.SelectedIndex = i;
                        break;
                    }

                    i++;
                }
            }

            _node.IsRendering = true;
            modelPanel1.ClearTargets();
            modelPanel1.AddTarget(_node, false);
            modelPanel1.SetCamWithBox(_node.GetBox());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                return;
            }

            modelPanel1.ClearTargets();
            GetBaseInfluence();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                return;
            }

            _parent = (MDL0BoneNode) comboBox2.SelectedItem;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            modelPanel1.ClearTargets();

            _mergeModels = checkBox1.Checked;
            if (_mergeModels)
            {
                label1.Hide();
                comboBox1.Hide();
                _baseInf = _externalModel._linker.BoneCache[0];
                baseBone.Text = _baseInf.ToString();

                modelPanel1.AddTarget(_externalModel, false);
                modelPanel1.SetCamWithBox(_externalModel.GetBox());
            }
            else
            {
                label1.Show();
                comboBox1.Show();
                GetBaseInfluence();
            }
        }

        #region Designer

        private Button btnCancel;
        private Button btnOkay;

        private void InitializeComponent()
        {
            btnCancel = new Button();
            btnOkay = new Button();
            label1 = new Label();
            comboBox1 = new ComboBox();
            label2 = new Label();
            comboBox2 = new ComboBox();
            comboBox3 = new ComboBox();
            checkBox1 = new CheckBox();
            label3 = new Label();
            label4 = new Label();
            baseBone = new Label();
            modelPanel1 = new ModelPanel();
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(139, 131);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // btnOkay
            // 
            btnOkay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOkay.Location = new System.Drawing.Point(58, 131);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new System.Drawing.Size(75, 23);
            btnOkay.TabIndex = 1;
            btnOkay.Text = "&Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new EventHandler(btnOkay_Click);
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(47, 12);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(39, 13);
            label1.TabIndex = 3;
            label1.Text = "Import:";
            label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBox1
            // 
            comboBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new System.Drawing.Point(92, 9);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(121, 21);
            comboBox1.TabIndex = 4;
            comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label2.Location = new System.Drawing.Point(3, 58);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(83, 18);
            label2.TabIndex = 5;
            label2.Text = "Skeleton Root:";
            label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBox2
            // 
            comboBox2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new System.Drawing.Point(92, 55);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new System.Drawing.Size(121, 21);
            comboBox2.TabIndex = 6;
            comboBox2.SelectedIndexChanged += new EventHandler(comboBox2_SelectedIndexChanged);
            // 
            // comboBox3
            // 
            comboBox3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[]
            {
                "Merge",
                "Replace",
                "Add As Child"
            });
            comboBox3.Location = new System.Drawing.Point(92, 82);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new System.Drawing.Size(121, 21);
            comboBox3.TabIndex = 7;
            // 
            // checkBox1
            // 
            checkBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(92, 109);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(116, 17);
            checkBox1.TabIndex = 8;
            checkBox1.Text = "Merge both models";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += new EventHandler(checkBox1_CheckedChanged);
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(7, 85);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(79, 13);
            label3.TabIndex = 9;
            label3.Text = "Base Skeleton:";
            label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(24, 35);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(62, 13);
            label4.TabIndex = 10;
            label4.Text = "Base Bone:";
            label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // baseBone
            // 
            baseBone.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            baseBone.AutoSize = true;
            baseBone.Location = new System.Drawing.Point(92, 35);
            baseBone.Name = "baseBone";
            baseBone.Size = new System.Drawing.Size(37, 13);
            baseBone.TabIndex = 11;
            baseBone.Text = "(none)";
            baseBone.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // modelPanel1
            // 
            modelPanel1.Dock = DockStyle.Fill;
            modelPanel1.Location = new System.Drawing.Point(0, 0);
            modelPanel1.Name = "modelPanel1";
            modelPanel1.Size = new System.Drawing.Size(203, 166);
            modelPanel1.TabIndex = 12;
            // 
            // panel1
            // 
            panel1.Controls.Add(baseBone);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(checkBox1);
            panel1.Controls.Add(comboBox3);
            panel1.Controls.Add(comboBox2);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnOkay);
            panel1.Controls.Add(btnCancel);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new System.Drawing.Point(203, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(226, 166);
            panel1.TabIndex = 13;
            // 
            // ObjectImporter
            // 
            AcceptButton = btnOkay;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(429, 166);
            Controls.Add(modelPanel1);
            Controls.Add(panel1);
            //this.Icon = BrawlLib.Properties.Resources.Icon;
            MinimumSize = new System.Drawing.Size(242, 204);
            Name = "ObjectImporter";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Import Object";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}