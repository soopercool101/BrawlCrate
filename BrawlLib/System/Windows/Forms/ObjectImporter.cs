using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Modeling;
using BrawlLib.Wii.Models;

namespace System.Windows.Forms
{
    public class ObjectImporter : Form
    {
        MDL0Node _internalModel;
        private Label label1;
        private ComboBox comboBox1;
        private Label label2;
        private ComboBox comboBox2;
        MDL0Node _externalModel;
        MDL0ObjectNode _node;
        IMatrixNode _baseInf;
        private ComboBox comboBox3;
        private CheckBox checkBox1;
        MDL0BoneNode _parent;
        private Label label3;
        private Label label4;
        private Label baseBone;
        private ModelPanel modelPanel1;
        private Panel panel1;
        bool _mergeModels = false;
        
        public ObjectImporter()
        {
            InitializeComponent();
            //modelPanel1.AddViewport(ModelPanelViewport.DefaultPerspective);
        }

        public DialogResult ShowDialog(MDL0Node internalModel, MDL0Node externalModel)
        {
            _internalModel = internalModel;
            _externalModel = externalModel;

            comboBox1.Items.AddRange(_externalModel.FindChild("Objects", true).Children.ToArray());
            comboBox2.Items.AddRange(_internalModel._linker.BoneCache);

            comboBox1.SelectedIndex = comboBox2.SelectedIndex = comboBox3.SelectedIndex = 0;
            _parent = (MDL0BoneNode)comboBox2.SelectedItem;

            return base.ShowDialog(null);
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
                if (b1.Name == child.Name)
                {
                    found = true;
                    bone = b1;
                    foreach (MDL0BoneNode b in child.Children)
                        MergeChildren(b1, b, res);
                    break;
                }
            if (!found)
            {
                MDL0BoneNode b = child.Clone();
                parent.InsertChild(b, true, child.Index);
                bone = b;
            }
            else
                found = false;
            
            if (res is MDL0ObjectNode)
            {
                MDL0ObjectNode poly = res as MDL0ObjectNode;
                foreach (Vertex3 v in poly._manager._vertices)
                    if (v.MatrixNode == child)
                        v.MatrixNode = bone;
            }
            else if (res is MDL0Node)
            {
                MDL0Node mdl = res as MDL0Node;
                foreach (MDL0ObjectNode poly in mdl.FindChild("Objects", true).Children)
                {
                    foreach (Vertex3 v in poly._manager._vertices)
                        if (v.MatrixNode == child)
                            v.MatrixNode = bone;
                }
            }
        }

        private void ImportObject(MDL0ObjectNode node)
        {
            _internalModel.ReplaceOrAddMesh(node, false, false, true);
        }

        private unsafe void btnOkay_Click(object sender, EventArgs e)
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    foreach (MDL0BoneNode b in (_baseInf as MDL0BoneNode).Children)
                        MergeChildren(_parent, (MDL0BoneNode)b, _mergeModels ? (ResourceNode)_externalModel : (ResourceNode)_node);
                    break;
                case 1:
                    _parent.Parent.InsertChild((ResourceNode)_baseInf, true, _parent.Index);
                    _parent.Remove();
                    break;
                case 2:
                    _parent.AddChild((ResourceNode)_baseInf);
                    break;
            }

            _internalModel.ApplyCHR(null, 0);
            if (_mergeModels)
            {
                _externalModel.ApplyCHR(null, 0);
                foreach (MDL0ObjectNode poly in _externalModel.FindChild("Objects", true).Children)
                    ImportObject(poly);
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

        private void btnCancel_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }

        private void GetBaseInfluence()
        {
            if (_node != null)
                modelPanel1.RemoveReference(_node);

            MDL0BoneNode[] boneCache = _externalModel._linker.BoneCache;
            if ((_node = (MDL0ObjectNode)comboBox1.SelectedItem).Weighted)
            {
                int least = int.MaxValue;
                foreach (IMatrixNode inf in _node.Influences)
                    if (inf is MDL0BoneNode && ((MDL0BoneNode)inf).BoneIndex < least)
                        least = ((MDL0BoneNode)inf).BoneIndex;

                if (least != int.MaxValue)
                {
                    MDL0BoneNode temp = (MDL0BoneNode)boneCache[least];
                    _baseInf = (IMatrixNode)temp.Parent;
                }
            }
            else _baseInf = _node.MatrixNode;

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
            modelPanel1.AddTarget(_node);
            modelPanel1.SetCamWithBox(_node.GetBox());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;

            modelPanel1.ClearTargets();
            GetBaseInfluence();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;

            _parent = (MDL0BoneNode)comboBox2.SelectedItem;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            modelPanel1.ClearTargets();

            _mergeModels = checkBox1.Checked;
            if (_mergeModels)
            {
                label1.Hide();
                comboBox1.Hide();
                _baseInf = (IMatrixNode)_externalModel._linker.BoneCache[0];
                baseBone.Text = _baseInf.ToString();

                modelPanel1.AddTarget(_externalModel);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectImporter));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.baseBone = new System.Windows.Forms.Label();
            this.modelPanel1 = new System.Windows.Forms.ModelPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(139, 131);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.Location = new System.Drawing.Point(58, 131);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 1;
            this.btnOkay.Text = "&Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Import:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(92, 9);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(3, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "Skeleton Root:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(92, 55);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 6;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // comboBox3
            // 
            this.comboBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "Merge",
            "Replace",
            "Add As Child"});
            this.comboBox3.Location = new System.Drawing.Point(92, 82);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 21);
            this.comboBox3.TabIndex = 7;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(92, 109);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(116, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Merge both models";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Base Skeleton:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Base Bone:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // baseBone
            // 
            this.baseBone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.baseBone.AutoSize = true;
            this.baseBone.Location = new System.Drawing.Point(92, 35);
            this.baseBone.Name = "baseBone";
            this.baseBone.Size = new System.Drawing.Size(37, 13);
            this.baseBone.TabIndex = 11;
            this.baseBone.Text = "(none)";
            this.baseBone.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // modelPanel1
            // 
            this.modelPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel1.Location = new System.Drawing.Point(0, 0);
            this.modelPanel1.Name = "modelPanel1";
            this.modelPanel1.Size = new System.Drawing.Size(203, 166);
            this.modelPanel1.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.baseBone);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.comboBox3);
            this.panel1.Controls.Add(this.comboBox2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnOkay);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(203, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(226, 166);
            this.panel1.TabIndex = 13;
            // 
            // ObjectImporter
            // 
            this.AcceptButton = this.btnOkay;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(429, 166);
            this.Controls.Add(this.modelPanel1);
            this.Controls.Add(this.panel1);
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(242, 204);
            this.Name = "ObjectImporter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Object";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
    }
}
