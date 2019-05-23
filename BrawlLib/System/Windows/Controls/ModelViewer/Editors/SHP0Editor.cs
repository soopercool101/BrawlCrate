using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;
using BrawlLib.Wii.Animations;
using System.Collections.Generic;
using BrawlLib.Modeling;

namespace System.Windows.Forms
{
    public class SHP0Editor : UserControl
    {
        #region Designer
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.NumericInputBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(256, 49);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedValueChanged += new System.EventHandler(this.listBox1_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Target Vertex Sets";
            // 
            // listBox2
            // 
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.IntegralHeight = false;
            this.listBox2.Location = new System.Drawing.Point(3, 0);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(261, 49);
            this.listBox2.TabIndex = 2;
            this.listBox2.SelectedValueChanged += new System.EventHandler(this.listBox2_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(267, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Destination Vertex Sets";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(429, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(37, 20);
            this.button1.TabIndex = 4;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Morph Percentage:";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(126, 4);
            this.trackBar1.Maximum = 1000;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(204, 45);
            this.trackBar1.TabIndex = 6;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(108, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "0%";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(326, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "100%";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(365, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Value: ";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(472, 27);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(56, 20);
            this.button2.TabIndex = 11;
            this.button2.Text = "Remove";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(446, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "%";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(209, 27);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(56, 20);
            this.button3.TabIndex = 14;
            this.button3.Text = "Remove";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(166, 27);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(37, 20);
            this.button4.TabIndex = 13;
            this.button4.Text = "Add";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 49);
            this.splitter1.TabIndex = 15;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listBox2);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(256, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(264, 49);
            this.panel1.TabIndex = 16;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.listBox1);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Location = new System.Drawing.Point(7, 49);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(520, 49);
            this.panel2.TabIndex = 17;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(472, 5);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(55, 20);
            this.button5.TabIndex = 18;
            this.button5.Text = "Set";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBox1
            // 
            this.textBox1.Integral = false;
            this.textBox1.Location = new System.Drawing.Point(404, 5);
            this.textBox1.MaximumValue = 3.402823E+38F;
            this.textBox1.MaxLength = 999999;
            this.textBox1.MinimumValue = -3.402823E+38F;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(41, 20);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = "0";
            this.textBox1.ValueChanged += new System.EventHandler(this.PercentChanged);
            // 
            // SHP0Editor
            // 
            this.Controls.Add(this.button5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(533, 106);
            this.Name = "SHP0Editor";
            this.Size = new System.Drawing.Size(533, 106);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox listBox1;
        private Label label1;
        private ListBox listBox2;
        private Label label2;
        private Button button1;
        private Label label3;
        private TrackBar trackBar1;
        private Label label4;
        private Label label5;
        private NumericInputBox textBox1;
        private Label label6;
        private Button button2;
        private Label label7;
        private Button button3;
        private Button button4;
        private Splitter splitter1;
        private Panel panel1;
        private Panel panel2;
        private Button button5;

        public ModelEditorBase _mainWindow;

        public SHP0Editor()
        {
            InitializeComponent(); 
        }
        public void UpdatePropDisplay()
        {
            if (!Enabled)
                return;

            ResetBox();

            if (_mainWindow.InterpolationEditor != null && _mainWindow.InterpolationEditor._targetNode != VertexSetDest)
                _mainWindow.InterpolationEditor.SetTarget(VertexSetDest);
        }
        private string _selectedSource;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedSource
        {
            get { return _selectedSource; }
            set { _selectedSource = value; }
        }
        private string _selectedDest;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedDestination
        {
            get { return _selectedDest; }
            set
            {
                _selectedDest = value; 
                ResetBox(); 
                if (_mainWindow.InterpolationEditor != null && _mainWindow.InterpolationEditor._targetNode != VertexSetDest)
                    _mainWindow.InterpolationEditor.SetTarget(VertexSetDest);
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get { return _mainWindow.CurrentFrame; }
            set { _mainWindow.CurrentFrame = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get { return _mainWindow.TargetModel; }
            set { _mainWindow.TargetModel = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SHP0Node SelectedAnimation
        {
            get { return _mainWindow.SelectedSHP0; }
            set { _mainWindow.SelectedSHP0 = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CHR0Node SelectedCHR0
        {
            get { return _mainWindow.SelectedCHR0; }
            set { _mainWindow.SelectedCHR0 = value; }
        }

        public SHP0VertexSetNode VertexSetDest
        {
            get
            {
                if (SelectedSource == null || SelectedDestination == null || SelectedAnimation == null) return null;
                ResourceNode set = SelectedAnimation.FindChild(SelectedSource, false);
                if (set == null) return null;
                return set.FindChild(SelectedDestination, false) as SHP0VertexSetNode;
            }
        }

        public void UpdateVertexSetList()
        {
            //listBox1.Items.Clear();
            //if (TargetModel == null)
            //    return;

            //listBox1.BeginUpdate();
            //foreach (MDL0VertexNode n in TargetModel._vertList)
            //    listBox1.Items.Add(n);
            //listBox1.EndUpdate();
        }

        public void UpdateVertexSetDestList()
        {
            //listBox2.Items.Clear();
            //if (TargetModel == null)
            //    return;

            //listBox2.BeginUpdate();
            //foreach (MDL0VertexNode n in TargetModel._vertList)
            //    listBox2.Items.Add(n);
            //listBox2.EndUpdate();
        }

        private Dictionary<int, List<int>> SHP0Indices;
        public void AnimationChanged()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox1.BeginUpdate();
            listBox2.BeginUpdate();

            SHP0Indices = new Dictionary<int, List<int>>();

            if (SelectedAnimation != null && TargetModel != null && TargetModel is MDL0Node)
            {
                List<string> names1 = new List<string>(), names2 = new List<string>();
                MDL0Node model = TargetModel as MDL0Node;
                foreach (SHP0EntryNode e in SelectedAnimation.Children)
                {
                    if (model._vertList != null)
                        foreach (MDL0VertexNode v1 in model._vertList)
                            if (e.Name == v1.Name && !names1.Contains(e.Name))
                            {
                                names1.Add(e.Name);
                                List<int> indices = new List<int>();
                                foreach (SHP0VertexSetNode n in e.Children)
                                    foreach (MDL0VertexNode v2 in model._vertList)
                                        if (n.Name == v2.Name && !names2.Contains(n.Name))
                                        {
                                            indices.Add(v2.Index);
                                            names2.Add(n.Name);
                                        }
                                SHP0Indices[v1.Index] = indices;
                                break;
                            }

                    if (model._normList != null)
                        foreach (MDL0NormalNode v1 in model._normList)
                            if (e.Name == v1.Name && !names1.Contains(e.Name))
                            {
                                names1.Add(e.Name);
                                List<int> indices = new List<int>();
                                foreach (SHP0VertexSetNode n in e.Children)
                                    foreach (MDL0NormalNode v2 in model._normList)
                                        if (n.Name == v2.Name && !names2.Contains(n.Name))
                                        {
                                            indices.Add(v2.Index);
                                            names2.Add(n.Name);
                                        }
                                SHP0Indices[v1.Index] = indices;
                                break;
                            }

                    if (model._colorList != null)
                        foreach (MDL0ColorNode v1 in model._colorList)
                            if (e.Name == v1.Name && !names1.Contains(e.Name))
                            {
                                names1.Add(e.Name);
                                List<int> indices = new List<int>();
                                foreach (SHP0VertexSetNode n in e.Children)
                                    foreach (MDL0ColorNode v2 in model._colorList)
                                        if (n.Name == v2.Name && !names2.Contains(n.Name))
                                        {
                                            indices.Add(v2.Index);
                                            names2.Add(n.Name);
                                        }
                                SHP0Indices[v1.Index] = indices;
                                break;
                            }
                }

                listBox1.Items.AddRange(names1.ToArray());
                listBox2.Items.AddRange(names2.ToArray());
            }

            listBox1.EndUpdate();
            listBox2.EndUpdate();
        }

        internal unsafe void PercentChanged(object sender, EventArgs e)
        {
            if (SelectedSource == null || SelectedDestination == null || updating)
                return;

            if ((SelectedAnimation != null) && (CurrentFrame >= 1))
            {
                SHP0EntryNode entry = SelectedAnimation.FindChild(SelectedSource, false) as SHP0EntryNode;
                SHP0VertexSetNode v;

                if (entry == null)
                    (v = (entry = SelectedAnimation.FindOrCreateEntry(SelectedSource)).Children[0] as SHP0VertexSetNode).Name = SelectedDestination;
                else if ((v = entry.FindChild(SelectedDestination, false) as SHP0VertexSetNode) == null)
                {
                    if (!float.IsNaN(textBox1.Value))
                    {
                        v = entry.FindOrCreateEntry(SelectedDestination);
                        v.SetKeyframe(CurrentFrame - 1, textBox1.Value / 100.0f);
                    }
                }
                else
                    if (float.IsNaN(textBox1.Value))
                        v.RemoveKeyframe(CurrentFrame - 1);
                    else
                        v.SetKeyframe(CurrentFrame - 1, textBox1.Value / 100.0f);

                if (_mainWindow.InterpolationEditor != null && _mainWindow.InterpolationEditor._targetNode != VertexSetDest)
                    _mainWindow.InterpolationEditor.SetTarget(VertexSetDest);
            }
            ResetBox();
            _mainWindow.KeyframePanel.UpdateKeyframe(CurrentFrame - 1);
            _mainWindow.UpdateModel();
        }
        bool updating = false;
        public unsafe void ResetBox()
        {
            SHP0EntryNode entry;
            SHP0VertexSetNode v;
            if (SelectedSource == null || SelectedDestination == null)
                return;
            if ((SelectedAnimation != null) && (CurrentFrame >= 1) && 
                ((entry = SelectedAnimation.FindChild(SelectedSource, false) as SHP0EntryNode) != null) && 
                (v = entry.FindChild(SelectedDestination, false) as SHP0VertexSetNode) != null)
            {
                KeyframeEntry e = v.Keyframes.GetKeyframe(CurrentFrame - 1);
                if (e == null)
                {
                    textBox1.Value = v.Keyframes[CurrentFrame - 1] * 100.0f;
                    textBox1.BackColor = Color.White;
                }
                else
                {
                    textBox1.Value = e._value * 100.0f;
                    textBox1.BackColor = Color.Yellow;
                }
            }
            else
            {
                textBox1.Value = 0;
                textBox1.BackColor = Color.White;
            }
            updating = true;
            trackBar1.Value = ((int)(textBox1.Value * 10.0f)).Clamp(trackBar1.Minimum, trackBar1.Maximum);
            updating = false;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Value = (float)trackBar1.Value / 10.0f;
            PercentChanged(null, null);
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            SelectedSource = listBox1.SelectedItem as string;
            _mainWindow.KeyframePanel.TargetSequence = VertexSetDest;
        }

        private void listBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            button2.Enabled = listBox2.SelectedItem != null;
            _selectedDest = listBox2.SelectedItem as string;
            _mainWindow.KeyframePanel.TargetSequence = VertexSetDest;
            _mainWindow.UpdatePropDisplay();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private unsafe void button5_Click(object sender, EventArgs e)
        {
            //Set vertices (0), normals (1), and/or colors (2, 3)
            //UVs are not morphed so there's no need to set them

            if (SelectedAnimation == null || 
                SelectedSource == null || 
                TargetModel == null || 
                TargetModel.Objects == null ||
                TargetModel.Objects.Length == 0)
                return;

            SHP0EntryNode entry = SelectedAnimation.FindChild(SelectedSource, false) as SHP0EntryNode;
            if (entry == null)
                return;

            if (MessageBox.Show(this, "Are you sure you want to continue?\nThis will edit the model and make the selected object's vertices, normals and/or colors default to the current morph.", "Are you sure?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            //Set the model to be only the bind pose with the SHP0 applied
            //This is so when the data is unweighted,
            //only the influence of the SHP0 will be set to the model.
            //Otherwise the entire CHR0 pose would be set as well
            float frame = CurrentFrame;
            SHP0Node shp = _mainWindow.SelectedSHP0;
            CHR0Node chr = _mainWindow.SelectedCHR0;
            if (TargetModel != null)
            {
                TargetModel.ApplyCHR(null, 0);
                TargetModel.ApplySHP(shp, frame);
            }

            ResourceNode[] nodes = ((ResourceNode)TargetModel).FindChildrenByName(SelectedSource);
            foreach (ResourceNode n in nodes)
            {
                if (n is MDL0VertexNode)
                {
                    MDL0VertexNode node = (MDL0VertexNode)n;
                    MDL0ObjectNode[] o = new MDL0ObjectNode[node._objects.Count]; 
                    node._objects.CopyTo(o);
                    foreach (MDL0ObjectNode obj in o)
                    {
                        //Set the unweighted positions using the weighted positions
                        //Created using the SHP0
                        obj.Unweight(false);
                        obj.SetEditedAssets(true, true, false, false, false);
                    }
                }
                else if (n is MDL0NormalNode)
                {
                    MDL0NormalNode node = (MDL0NormalNode)n;
                    MDL0ObjectNode[] o = new MDL0ObjectNode[node._objects.Count];
                    node._objects.CopyTo(o);
                    foreach (MDL0ObjectNode obj in o)
                        obj.SetEditedAssets(true, false, true, false, false);
                }
                else if (n is MDL0ColorNode)
                {
                    MDL0ColorNode node = (MDL0ColorNode)n;
                    MDL0ObjectNode[] o = new MDL0ObjectNode[node._objects.Count];
                    node._objects.CopyTo(o);
                    foreach (MDL0ObjectNode obj in o)
                        obj.SetEditedAssets(true, false, false, true, true);
                }
            }

            if (TargetModel != null)
            {
                TargetModel.ApplyCHR(chr, frame);
                TargetModel.ApplySHP(shp, frame);
            }
        }
    }
}
