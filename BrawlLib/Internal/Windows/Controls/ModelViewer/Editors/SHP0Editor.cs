using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.Editors
{
    public class SHP0Editor : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            listBox1 = new ListBox();
            label1 = new Label();
            listBox2 = new ListBox();
            label2 = new Label();
            button1 = new Button();
            label3 = new Label();
            trackBar1 = new TrackBar();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            button2 = new Button();
            label7 = new Label();
            button3 = new Button();
            button4 = new Button();
            splitter1 = new Splitter();
            panel1 = new Panel();
            panel2 = new Panel();
            button5 = new Button();
            textBox1 = new NumericInputBox();
            ((ISupportInitialize) trackBar1).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.IntegralHeight = false;
            listBox1.Location = new Point(0, 0);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(256, 49);
            listBox1.TabIndex = 0;
            listBox1.SelectedValueChanged += new EventHandler(listBox1_SelectedValueChanged);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(4, 31);
            label1.Name = "label1";
            label1.Size = new Size(95, 13);
            label1.TabIndex = 1;
            label1.Text = "Target Vertex Sets";
            // 
            // listBox2
            // 
            listBox2.Dock = DockStyle.Fill;
            listBox2.FormattingEnabled = true;
            listBox2.IntegralHeight = false;
            listBox2.Location = new Point(3, 0);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(261, 49);
            listBox2.TabIndex = 2;
            listBox2.SelectedValueChanged += new EventHandler(listBox2_SelectedValueChanged);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(267, 31);
            label2.Name = "label2";
            label2.Size = new Size(117, 13);
            label2.TabIndex = 3;
            label2.Text = "Destination Vertex Sets";
            // 
            // button1
            // 
            button1.Location = new Point(429, 27);
            button1.Name = "button1";
            button1.Size = new Size(37, 20);
            button1.TabIndex = 4;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = true;
            button1.Visible = false;
            button1.Click += new EventHandler(button1_Click);
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(4, 8);
            label3.Name = "label3";
            label3.Size = new Size(98, 13);
            label3.TabIndex = 5;
            label3.Text = "Morph Percentage:";
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(126, 4);
            trackBar1.Maximum = 1000;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(204, 45);
            trackBar1.TabIndex = 6;
            trackBar1.TickStyle = TickStyle.None;
            trackBar1.Scroll += new EventHandler(trackBar1_Scroll);
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(108, 8);
            label4.Name = "label4";
            label4.Size = new Size(21, 13);
            label4.TabIndex = 7;
            label4.Text = "0%";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(326, 8);
            label5.Name = "label5";
            label5.Size = new Size(33, 13);
            label5.TabIndex = 8;
            label5.Text = "100%";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(365, 8);
            label6.Name = "label6";
            label6.Size = new Size(40, 13);
            label6.TabIndex = 10;
            label6.Text = "Value: ";
            // 
            // button2
            // 
            button2.Location = new Point(472, 27);
            button2.Name = "button2";
            button2.Size = new Size(56, 20);
            button2.TabIndex = 11;
            button2.Text = "Remove";
            button2.UseVisualStyleBackColor = true;
            button2.Visible = false;
            button2.Click += new EventHandler(button2_Click);
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(446, 8);
            label7.Name = "label7";
            label7.Size = new Size(15, 13);
            label7.TabIndex = 12;
            label7.Text = "%";
            // 
            // button3
            // 
            button3.Location = new Point(209, 27);
            button3.Name = "button3";
            button3.Size = new Size(56, 20);
            button3.TabIndex = 14;
            button3.Text = "Remove";
            button3.UseVisualStyleBackColor = true;
            button3.Visible = false;
            // 
            // button4
            // 
            button4.Location = new Point(166, 27);
            button4.Name = "button4";
            button4.Size = new Size(37, 20);
            button4.TabIndex = 13;
            button4.Text = "Add";
            button4.UseVisualStyleBackColor = true;
            button4.Visible = false;
            button4.Click += new EventHandler(button4_Click);
            // 
            // splitter1
            // 
            splitter1.Location = new Point(0, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(3, 49);
            splitter1.TabIndex = 15;
            splitter1.TabStop = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(listBox2);
            panel1.Controls.Add(splitter1);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(256, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(264, 49);
            panel1.TabIndex = 16;
            // 
            // panel2
            // 
            panel2.Controls.Add(listBox1);
            panel2.Controls.Add(panel1);
            panel2.Location = new Point(7, 49);
            panel2.Name = "panel2";
            panel2.Size = new Size(520, 49);
            panel2.TabIndex = 17;
            // 
            // button5
            // 
            button5.Location = new Point(472, 5);
            button5.Name = "button5";
            button5.Size = new Size(55, 20);
            button5.TabIndex = 18;
            button5.Text = "Set";
            button5.UseVisualStyleBackColor = true;
            button5.Click += new EventHandler(button5_Click);
            // 
            // textBox1
            // 
            textBox1.Integral = false;
            textBox1.Location = new Point(404, 5);
            textBox1.MaximumValue = 3.402823E+38F;
            textBox1.MaxLength = 999999;
            textBox1.MinimumValue = -3.402823E+38F;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(41, 20);
            textBox1.TabIndex = 9;
            textBox1.Text = "0";
            textBox1.ValueChanged += new EventHandler(PercentChanged);
            // 
            // SHP0Editor
            // 
            Controls.Add(button5);
            Controls.Add(panel2);
            Controls.Add(button3);
            Controls.Add(button4);
            Controls.Add(label7);
            Controls.Add(button2);
            Controls.Add(textBox1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(trackBar1);
            Controls.Add(label6);
            Controls.Add(label1);
            MinimumSize = new Size(533, 106);
            Name = "SHP0Editor";
            Size = new Size(533, 106);
            ((ISupportInitialize) trackBar1).EndInit();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
            {
                return;
            }

            ResetBox();

            if (_mainWindow.InterpolationEditor != null && _mainWindow.InterpolationEditor._targetNode != VertexSetDest)
            {
                _mainWindow.InterpolationEditor.SetTarget(VertexSetDest);
            }
        }

        private string _selectedSource;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedSource
        {
            get => _selectedSource;
            set => _selectedSource = value;
        }

        private string _selectedDest;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedDestination
        {
            get => _selectedDest;
            set
            {
                _selectedDest = value;
                ResetBox();
                if (_mainWindow.InterpolationEditor != null &&
                    _mainWindow.InterpolationEditor._targetNode != VertexSetDest)
                {
                    _mainWindow.InterpolationEditor.SetTarget(VertexSetDest);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get => _mainWindow.CurrentFrame;
            set => _mainWindow.CurrentFrame = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get => _mainWindow.TargetModel;
            set => _mainWindow.TargetModel = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SHP0Node SelectedAnimation
        {
            get => _mainWindow.SelectedSHP0;
            set => _mainWindow.SelectedSHP0 = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CHR0Node SelectedCHR0
        {
            get => _mainWindow.SelectedCHR0;
            set => _mainWindow.SelectedCHR0 = value;
        }

        public SHP0VertexSetNode VertexSetDest
        {
            get
            {
                if (SelectedSource == null || SelectedDestination == null || SelectedAnimation == null)
                {
                    return null;
                }

                ResourceNode set = SelectedAnimation.FindChild(SelectedSource, false);

                return set?.FindChild(SelectedDestination, false) as SHP0VertexSetNode;
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

            if (SelectedAnimation != null && TargetModel is MDL0Node)
            {
                List<string> names1 = new List<string>(), names2 = new List<string>();
                MDL0Node model = TargetModel as MDL0Node;
                foreach (SHP0EntryNode e in SelectedAnimation.Children)
                {
                    if (model._vertList != null)
                    {
                        foreach (MDL0VertexNode v1 in model._vertList)
                        {
                            if (e.Name == v1.Name && !names1.Contains(e.Name))
                            {
                                names1.Add(e.Name);
                                List<int> indices = new List<int>();
                                foreach (SHP0VertexSetNode n in e.Children)
                                {
                                    foreach (MDL0VertexNode v2 in model._vertList)
                                    {
                                        if (n.Name == v2.Name && !names2.Contains(n.Name))
                                        {
                                            indices.Add(v2.Index);
                                            names2.Add(n.Name);
                                        }
                                    }
                                }

                                SHP0Indices[v1.Index] = indices;
                                break;
                            }
                        }
                    }

                    if (model._normList != null)
                    {
                        foreach (MDL0NormalNode v1 in model._normList)
                        {
                            if (e.Name == v1.Name && !names1.Contains(e.Name))
                            {
                                names1.Add(e.Name);
                                List<int> indices = new List<int>();
                                foreach (SHP0VertexSetNode n in e.Children)
                                {
                                    foreach (MDL0NormalNode v2 in model._normList)
                                    {
                                        if (n.Name == v2.Name && !names2.Contains(n.Name))
                                        {
                                            indices.Add(v2.Index);
                                            names2.Add(n.Name);
                                        }
                                    }
                                }

                                SHP0Indices[v1.Index] = indices;
                                break;
                            }
                        }
                    }

                    if (model._colorList != null)
                    {
                        foreach (MDL0ColorNode v1 in model._colorList)
                        {
                            if (e.Name == v1.Name && !names1.Contains(e.Name))
                            {
                                names1.Add(e.Name);
                                List<int> indices = new List<int>();
                                foreach (SHP0VertexSetNode n in e.Children)
                                {
                                    foreach (MDL0ColorNode v2 in model._colorList)
                                    {
                                        if (n.Name == v2.Name && !names2.Contains(n.Name))
                                        {
                                            indices.Add(v2.Index);
                                            names2.Add(n.Name);
                                        }
                                    }
                                }

                                SHP0Indices[v1.Index] = indices;
                                break;
                            }
                        }
                    }
                }

                listBox1.Items.AddRange(names1.ToArray());
                listBox2.Items.AddRange(names2.ToArray());
            }

            listBox1.EndUpdate();
            listBox2.EndUpdate();
        }

        internal void PercentChanged(object sender, EventArgs e)
        {
            if (SelectedSource == null || SelectedDestination == null || updating)
            {
                return;
            }

            if (SelectedAnimation != null && CurrentFrame >= 1)
            {
                SHP0EntryNode entry = SelectedAnimation.FindChild(SelectedSource, false) as SHP0EntryNode;
                SHP0VertexSetNode v;

                if (entry == null)
                {
                    (v = (entry = SelectedAnimation.FindOrCreateEntry(SelectedSource)).Children[0] as SHP0VertexSetNode)
                        .Name = SelectedDestination;
                }
                else if ((v = entry.FindChild(SelectedDestination, false) as SHP0VertexSetNode) == null)
                {
                    if (!float.IsNaN(textBox1.Value))
                    {
                        v = entry.FindOrCreateEntry(SelectedDestination);
                        v.SetKeyframe(CurrentFrame - 1, textBox1.Value / 100.0f);
                    }
                }
                else if (float.IsNaN(textBox1.Value))
                {
                    v.RemoveKeyframe(CurrentFrame - 1);
                }
                else
                {
                    v.SetKeyframe(CurrentFrame - 1, textBox1.Value / 100.0f);
                }

                if (_mainWindow.InterpolationEditor != null &&
                    _mainWindow.InterpolationEditor._targetNode != VertexSetDest)
                {
                    _mainWindow.InterpolationEditor.SetTarget(VertexSetDest);
                }
            }

            ResetBox();
            _mainWindow.KeyframePanel.UpdateKeyframe(CurrentFrame - 1);
            _mainWindow.UpdateModel();
        }

        private bool updating;

        public void ResetBox()
        {
            SHP0EntryNode entry;
            SHP0VertexSetNode v;
            if (SelectedSource == null || SelectedDestination == null)
            {
                return;
            }

            if (SelectedAnimation != null && CurrentFrame >= 1 &&
                (entry = SelectedAnimation.FindChild(SelectedSource, false) as SHP0EntryNode) != null &&
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
            trackBar1.Value = ((int) (textBox1.Value * 10.0f)).Clamp(trackBar1.Minimum, trackBar1.Maximum);
            updating = false;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Value = trackBar1.Value / 10.0f;
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

        private void button5_Click(object sender, EventArgs e)
        {
            //Set vertices (0), normals (1), and/or colors (2, 3)
            //UVs are not morphed so there's no need to set them

            if (SelectedAnimation == null ||
                SelectedSource == null || TargetModel?.Objects == null || TargetModel.Objects.Length == 0)
            {
                return;
            }

            SHP0EntryNode entry = SelectedAnimation.FindChild(SelectedSource, false) as SHP0EntryNode;
            if (entry == null)
            {
                return;
            }

            if (MessageBox.Show(this,
                "Are you sure you want to continue?\nThis will edit the model and make the selected object's vertices, normals and/or colors default to the current morph.",
                "Are you sure?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }

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

            ResourceNode[] nodes = ((ResourceNode) TargetModel).FindChildrenByName(SelectedSource);
            foreach (ResourceNode n in nodes)
            {
                if (n is MDL0VertexNode)
                {
                    MDL0VertexNode node = (MDL0VertexNode) n;
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
                    MDL0NormalNode node = (MDL0NormalNode) n;
                    MDL0ObjectNode[] o = new MDL0ObjectNode[node._objects.Count];
                    node._objects.CopyTo(o);
                    foreach (MDL0ObjectNode obj in o)
                    {
                        obj.SetEditedAssets(true, false, true, false, false);
                    }
                }
                else if (n is MDL0ColorNode)
                {
                    MDL0ColorNode node = (MDL0ColorNode) n;
                    MDL0ObjectNode[] o = new MDL0ObjectNode[node._objects.Count];
                    node._objects.CopyTo(o);
                    foreach (MDL0ObjectNode obj in o)
                    {
                        obj.SetEditedAssets(true, false, false, true, true);
                    }
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