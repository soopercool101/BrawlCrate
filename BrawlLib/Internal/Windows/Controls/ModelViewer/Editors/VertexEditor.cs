using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.Editors
{
    public class VertexEditor : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            label3 = new Label();
            numPosZ = new NumericInputBox();
            label2 = new Label();
            numPosY = new NumericInputBox();
            label1 = new Label();
            numPosX = new NumericInputBox();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            numNormZ = new NumericInputBox();
            label4 = new Label();
            numNormX = new NumericInputBox();
            label5 = new Label();
            label6 = new Label();
            numNormY = new NumericInputBox();
            groupBox4 = new GroupBox();
            colorBox = new Label();
            colorIndex = new ComboBox();
            btnAverage = new Button();
            label7 = new Label();
            comboBox1 = new ComboBox();
            label8 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // label3
            // 
            label3.BorderStyle = BorderStyle.FixedSingle;
            label3.Location = new System.Drawing.Point(6, 54);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(22, 20);
            label3.TabIndex = 7;
            label3.Text = "Z: ";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numPosZ
            // 
            numPosZ.BorderStyle = BorderStyle.FixedSingle;
            numPosZ.Integral = false;
            numPosZ.Location = new System.Drawing.Point(27, 54);
            numPosZ.MaximumValue = 3.402823E+38F;
            numPosZ.MinimumValue = -3.402823E+38F;
            numPosZ.Name = "numPosZ";
            numPosZ.Size = new System.Drawing.Size(78, 20);
            numPosZ.TabIndex = 6;
            numPosZ.Text = "0";
            numPosZ.ValueChanged += new EventHandler(numPosZ_TextChanged);
            // 
            // label2
            // 
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Location = new System.Drawing.Point(6, 35);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(22, 20);
            label2.TabIndex = 5;
            label2.Text = "Y: ";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numPosY
            // 
            numPosY.BorderStyle = BorderStyle.FixedSingle;
            numPosY.Integral = false;
            numPosY.Location = new System.Drawing.Point(27, 35);
            numPosY.MaximumValue = 3.402823E+38F;
            numPosY.MinimumValue = -3.402823E+38F;
            numPosY.Name = "numPosY";
            numPosY.Size = new System.Drawing.Size(78, 20);
            numPosY.TabIndex = 4;
            numPosY.Text = "0";
            numPosY.ValueChanged += new EventHandler(numPosY_TextChanged);
            // 
            // label1
            // 
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Location = new System.Drawing.Point(6, 16);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(22, 20);
            label1.TabIndex = 3;
            label1.Text = "X: ";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numPosX
            // 
            numPosX.BorderStyle = BorderStyle.FixedSingle;
            numPosX.Integral = false;
            numPosX.Location = new System.Drawing.Point(27, 16);
            numPosX.MaximumValue = 3.402823E+38F;
            numPosX.MinimumValue = -3.402823E+38F;
            numPosX.Name = "numPosX";
            numPosX.Size = new System.Drawing.Size(78, 20);
            numPosX.TabIndex = 0;
            numPosX.Text = "0";
            numPosX.ValueChanged += new EventHandler(numPosX_TextChanged);
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            groupBox1.Controls.Add(numPosZ);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(numPosX);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(numPosY);
            groupBox1.Enabled = false;
            groupBox1.Location = new System.Drawing.Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(111, 82);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "Position";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            groupBox2.Controls.Add(numNormZ);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(numNormX);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(numNormY);
            groupBox2.Location = new System.Drawing.Point(231, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(111, 82);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "Normal";
            groupBox2.Visible = false;
            // 
            // numNormZ
            // 
            numNormZ.BorderStyle = BorderStyle.FixedSingle;
            numNormZ.Integral = false;
            numNormZ.Location = new System.Drawing.Point(27, 54);
            numNormZ.MaximumValue = 3.402823E+38F;
            numNormZ.MinimumValue = -3.402823E+38F;
            numNormZ.Name = "numNormZ";
            numNormZ.Size = new System.Drawing.Size(78, 20);
            numNormZ.TabIndex = 6;
            numNormZ.Text = "0";
            numNormZ.ValueChanged += new EventHandler(numNormZ_ValueChanged);
            // 
            // label4
            // 
            label4.BorderStyle = BorderStyle.FixedSingle;
            label4.Location = new System.Drawing.Point(6, 54);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(22, 20);
            label4.TabIndex = 7;
            label4.Text = "Z: ";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numNormX
            // 
            numNormX.BorderStyle = BorderStyle.FixedSingle;
            numNormX.Integral = false;
            numNormX.Location = new System.Drawing.Point(27, 16);
            numNormX.MaximumValue = 3.402823E+38F;
            numNormX.MinimumValue = -3.402823E+38F;
            numNormX.Name = "numNormX";
            numNormX.Size = new System.Drawing.Size(78, 20);
            numNormX.TabIndex = 0;
            numNormX.Text = "0";
            numNormX.ValueChanged += new EventHandler(numNormX_ValueChanged);
            // 
            // label5
            // 
            label5.BorderStyle = BorderStyle.FixedSingle;
            label5.Location = new System.Drawing.Point(6, 16);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(22, 20);
            label5.TabIndex = 3;
            label5.Text = "X: ";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            label6.BorderStyle = BorderStyle.FixedSingle;
            label6.Location = new System.Drawing.Point(6, 35);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(22, 20);
            label6.TabIndex = 5;
            label6.Text = "Y: ";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numNormY
            // 
            numNormY.BorderStyle = BorderStyle.FixedSingle;
            numNormY.Integral = false;
            numNormY.Location = new System.Drawing.Point(27, 35);
            numNormY.MaximumValue = 3.402823E+38F;
            numNormY.MinimumValue = -3.402823E+38F;
            numNormY.Name = "numNormY";
            numNormY.Size = new System.Drawing.Size(78, 20);
            numNormY.TabIndex = 4;
            numNormY.Text = "0";
            numNormY.ValueChanged += new EventHandler(numNormY_ValueChanged);
            // 
            // groupBox4
            // 
            groupBox4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            groupBox4.Controls.Add(colorBox);
            groupBox4.Controls.Add(colorIndex);
            groupBox4.Location = new System.Drawing.Point(348, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new System.Drawing.Size(111, 82);
            groupBox4.TabIndex = 11;
            groupBox4.TabStop = false;
            groupBox4.Text = "Color";
            groupBox4.Visible = false;
            // 
            // colorBox
            // 
            colorBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            colorBox.BorderStyle = BorderStyle.FixedSingle;
            colorBox.Cursor = Cursors.Hand;
            colorBox.Location = new System.Drawing.Point(6, 36);
            colorBox.Name = "colorBox";
            colorBox.Size = new System.Drawing.Size(99, 38);
            colorBox.TabIndex = 12;
            colorBox.DoubleClick += new EventHandler(colorBox_Click);
            // 
            // colorIndex
            // 
            colorIndex.DropDownStyle = ComboBoxStyle.DropDownList;
            colorIndex.FormattingEnabled = true;
            colorIndex.Items.AddRange(new object[]
            {
                "Color 0",
                "Color 1"
            });
            colorIndex.Location = new System.Drawing.Point(6, 14);
            colorIndex.Name = "colorIndex";
            colorIndex.Size = new System.Drawing.Size(99, 21);
            colorIndex.TabIndex = 7;
            colorIndex.SelectedIndexChanged += new EventHandler(colorIndex_SelectedIndexChanged);
            // 
            // btnAverage
            // 
            btnAverage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAverage.Enabled = false;
            btnAverage.Location = new System.Drawing.Point(120, 54);
            btnAverage.Name = "btnAverage";
            btnAverage.Size = new System.Drawing.Size(105, 23);
            btnAverage.TabIndex = 12;
            btnAverage.Text = "Average";
            btnAverage.UseVisualStyleBackColor = true;
            btnAverage.Click += new EventHandler(btnAverage_Click);
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(120, 38);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(104, 13);
            label7.TabIndex = 13;
            label7.Text = "No vertices selected";
            // 
            // comboBox1
            // 
            comboBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new System.Drawing.Point(98, -22);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(121, 21);
            comboBox1.TabIndex = 14;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(6, -19);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(86, 13);
            label8.TabIndex = 15;
            label8.Text = "Facepoint Index:";
            // 
            // VertexEditor
            // 
            Controls.Add(label8);
            Controls.Add(comboBox1);
            Controls.Add(label7);
            Controls.Add(btnAverage);
            Controls.Add(groupBox4);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            MinimumSize = new System.Drawing.Size(230, 85);
            Name = "VertexEditor";
            Size = new System.Drawing.Size(230, 85);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox4.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public VertexEditor()
        {
            InitializeComponent();
            //uvIndex.SelectedIndex = 0; 
            colorIndex.SelectedIndex = 0;
            _dlgColor = new GoodColorDialog();
        }

        public ModelEditorBase _mainWindow;

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
        public IBoneNode TargetBone
        {
            get => _mainWindow.SelectedBone;
            set => _mainWindow.SelectedBone = value;
        }

        private Label label3;
        public NumericInputBox numPosZ;
        private Label label2;
        public NumericInputBox numPosY;
        private Label label1;
        public NumericInputBox numPosX;
        private int _colorIndex;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        public NumericInputBox numNormZ;
        private Label label4;
        public NumericInputBox numNormX;
        private Label label5;
        private Label label6;
        public NumericInputBox numNormY;
        private GroupBox groupBox4;
        private ComboBox colorIndex;
        private Label colorBox;
        public MDL0BoneNode _targetBone;

        public bool _updating;
        private Button btnAverage;
        private Label label7;
        private ComboBox comboBox1;
        private Label label8;

        private readonly GoodColorDialog _dlgColor;

        private void colorBox_Click(object sender, EventArgs e)
        {
            if (TargetVertex == null)
            {
                return;
            }

            //RGBAPixel p = TargetVertex._colors[_colorIndex];
            //_dlgColor.Color = (Color)(ARGBPixel)p;
            //if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            //{
            //    p = (RGBAPixel)(ARGBPixel)_dlgColor.Color;
            //    colorBox.BackColor = Color.FromArgb(p.A, p.R, p.G, p.B);

            //    TargetVertex._colors[_colorIndex] = p;
            //    TargetVertex.SetColor(_colorIndex);
            //    TargetVertex._object._manager._dirty[_colorIndex + 2] = true;
            //    _mainWindow.UpdateModel();
            //}
        }

        public Vertex3 TargetVertex
        {
            get
            {
                if (_targetVertices != null && _targetVertices.Count != 0)
                {
                    return _targetVertices[0];
                }

                return null;
            }
        }

        private void colorIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            _colorIndex = colorIndex.SelectedIndex;
            UpdatePropDisplay();
        }

        //private void uvIndex_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    _uvIndex = uvIndex.SelectedIndex;
        //    UpdatePropDisplay();
        //}

        public void UpdatePropDisplay()
        {
            _updating = true;

            if (_targetVertices != null && _targetVertices.Count != 0)
            {
                if (Enabled != true)
                {
                    Enabled = true;
                }
            }
            else
            {
                if (Enabled)
                {
                    Enabled = false;
                }
            }

            Vertex3 vertex = TargetVertex;
            if (vertex == null || _targetVertices.Count > 1)
            {
                numPosX.Value = 0;
                numPosY.Value = 0;
                numPosZ.Value = 0;

                bool nonNull = vertex != null;
                btnAverage.Enabled = groupBox1.Enabled = nonNull;

                groupBox1.Text = nonNull ? "Offset" : "Position";

                if (nonNull)
                {
                    label7.Text = $"{_targetVertices.Count} vertices selected";
                }
                else
                {
                    label7.Text = "No vertices selected";
                }

                //numNormX.Value = 0;
                //numNormY.Value = 0;
                //numNormZ.Value = 0;

                //numTexX.Value = 0;
                //numTexY.Value = 0;

                //colorBox.BackColor = Color.FromKnownColor(KnownColor.Control);
            }
            else
            {
                Vector3 v3 = vertex.WeightedPosition;
                numPosX.Value = v3._x;
                numPosY.Value = v3._y;
                numPosZ.Value = v3._z;

                groupBox1.Text = "Position";
                groupBox1.Enabled = true;
                btnAverage.Enabled = false;

                label7.Text = "1 vertex selected";

                //v3 = vertex.WeightedNormal;
                //numNormX.Value = v3._x;
                //numNormY.Value = v3._y;
                //numNormZ.Value = v3._z;

                //Vector2 v2 = vertex._uvs[_uvIndex];
                //numTexX.Value = v2._x;
                //numTexY.Value = v2._y;

                //RGBAPixel p = vertex._colors[_colorIndex];
                //colorBox.BackColor = Color.FromArgb(p.A, p.R, p.G, p.B);
            }

            _updating = false;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Vertex3> TargetVertices
        {
            get => _targetVertices;
            set
            {
                if (_targetVertices != value)
                {
                    _targetVertices = value.ToList();
                    UpdatePropDisplay();
                }
            }
        }

        public List<Vertex3> _targetVertices;

        private void numPosX_TextChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (_targetVertices != null)
            {
                _mainWindow.VertexChange(_targetVertices);
                if (_targetVertices.Count == 1)
                {
                    TargetVertex._weightedPosition._x = numPosX.Value;
                    TargetVertex.Unweight();
                }
                else
                {
                    foreach (Vertex3 v in _targetVertices)
                    {
                        v._weightedPosition._x += numPosX.Value - numPosX._previousValue;
                        v.Unweight();
                    }
                }

                _mainWindow.VertexChange(_targetVertices);
                _mainWindow.UpdateModel();
            }
        }

        private void numPosY_TextChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (_targetVertices != null)
            {
                _mainWindow.VertexChange(_targetVertices);
                if (_targetVertices.Count == 1)
                {
                    TargetVertex._weightedPosition._y = numPosY.Value;
                    TargetVertex.Unweight();
                }
                else
                {
                    foreach (Vertex3 v in _targetVertices)
                    {
                        v._weightedPosition._y += numPosY.Value - numPosY._previousValue;
                        v.Unweight();
                    }
                }

                _mainWindow.VertexChange(_targetVertices);
                _mainWindow.UpdateModel();
            }
        }

        private void numPosZ_TextChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (_targetVertices != null)
            {
                _mainWindow.VertexChange(_targetVertices);
                if (_targetVertices.Count == 1)
                {
                    TargetVertex._weightedPosition._z = numPosZ.Value;
                    TargetVertex.Unweight();
                }
                else
                {
                    foreach (Vertex3 v in _targetVertices)
                    {
                        v._weightedPosition._z += numPosZ.Value - numPosZ._previousValue;
                        v.Unweight();
                    }
                }

                _mainWindow.VertexChange(_targetVertices);
                _mainWindow.UpdateModel();
            }
        }

        private void numNormX_ValueChanged(object sender, EventArgs e)
        {
            //if (_updating)
            //    return;

            //if (TargetVertex != null)
            //{
            //    TargetVertex._weightedNormal._x = numNormX.Value;
            //    TargetVertex.UnweightNormal();
            //    TargetVertex.SetNormal();
            //    _mainWindow.UpdateModel();
            //}
        }

        private void numNormY_ValueChanged(object sender, EventArgs e)
        {
            //if (_updating)
            //    return;

            //if (TargetVertex != null)
            //{
            //    TargetVertex._weightedNormal._y = numNormY.Value;
            //    TargetVertex.UnweightNormal();
            //    TargetVertex.SetNormal();
            //    _mainWindow.UpdateModel();
            //}
        }

        private void numNormZ_ValueChanged(object sender, EventArgs e)
        {
            //if (_updating)
            //    return;

            //if (TargetVertex != null)
            //{
            //    TargetVertex._weightedNormal._z = numNormZ.Value;
            //    TargetVertex.UnweightNormal();
            //    TargetVertex.SetNormal();
            //    _mainWindow.UpdateModel();
            //}
        }

        private void btnAverage_Click(object sender, EventArgs e)
        {
            _mainWindow.VertexChange(_targetVertices);
            Vector3 point = new Vector3();
            foreach (Vertex3 v in _targetVertices)
            {
                point += v.WeightedPosition;
            }

            point /= _targetVertices.Count;
            foreach (Vertex3 v in _targetVertices)
            {
                v.WeightedPosition = point;
            }

            _mainWindow.VertexChange(_targetVertices);
            _mainWindow.UpdateModel();
        }

        //private void numTexX_ValueChanged(object sender, EventArgs e)
        //{
        //    if (_updating)
        //        return;

        //    if (TargetVertex != null)
        //    {
        //        TargetVertex._uvs[_uvIndex]._x = numTexX.Value;
        //        TargetVertex.SetUV(_uvIndex);
        //        TargetVertex._object._manager._dirty[_uvIndex + 4] = true;
        //        _mainWindow.UpdateModel();
        //    }
        //}

        //private void numTexY_ValueChanged(object sender, EventArgs e)
        //{
        //    if (_updating)
        //        return;

        //    if (TargetVertex != null)
        //    {
        //        TargetVertex._uvs[_uvIndex]._y = numTexY.Value;
        //        TargetVertex.SetUV(_uvIndex);
        //        TargetVertex._object._manager._dirty[_uvIndex + 4] = true;
        //        _mainWindow.UpdateModel();
        //    }
        //}
    }
}