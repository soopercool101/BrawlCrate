using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Modeling;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace System.Windows.Forms
{
    public class VertexEditor : UserControl
    {
        #region Designer

        private System.ComponentModel.IContainer components;
        private void InitializeComponent()
        {
            this.label3 = new System.Windows.Forms.Label();
            this.numPosZ = new System.Windows.Forms.NumericInputBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numPosY = new System.Windows.Forms.NumericInputBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numPosX = new System.Windows.Forms.NumericInputBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numNormZ = new System.Windows.Forms.NumericInputBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numNormX = new System.Windows.Forms.NumericInputBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numNormY = new System.Windows.Forms.NumericInputBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.colorBox = new System.Windows.Forms.Label();
            this.colorIndex = new System.Windows.Forms.ComboBox();
            this.btnAverage = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(6, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Z: ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numPosZ
            // 
            this.numPosZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosZ.Integral = false;
            this.numPosZ.Location = new System.Drawing.Point(27, 54);
            this.numPosZ.MaximumValue = 3.402823E+38F;
            this.numPosZ.MinimumValue = -3.402823E+38F;
            this.numPosZ.Name = "numPosZ";
            this.numPosZ.Size = new System.Drawing.Size(78, 20);
            this.numPosZ.TabIndex = 6;
            this.numPosZ.Text = "0";
            this.numPosZ.ValueChanged += new System.EventHandler(this.numPosZ_TextChanged);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(6, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Y: ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numPosY
            // 
            this.numPosY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosY.Integral = false;
            this.numPosY.Location = new System.Drawing.Point(27, 35);
            this.numPosY.MaximumValue = 3.402823E+38F;
            this.numPosY.MinimumValue = -3.402823E+38F;
            this.numPosY.Name = "numPosY";
            this.numPosY.Size = new System.Drawing.Size(78, 20);
            this.numPosY.TabIndex = 4;
            this.numPosY.Text = "0";
            this.numPosY.ValueChanged += new System.EventHandler(this.numPosY_TextChanged);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "X: ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numPosX
            // 
            this.numPosX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosX.Integral = false;
            this.numPosX.Location = new System.Drawing.Point(27, 16);
            this.numPosX.MaximumValue = 3.402823E+38F;
            this.numPosX.MinimumValue = -3.402823E+38F;
            this.numPosX.Name = "numPosX";
            this.numPosX.Size = new System.Drawing.Size(78, 20);
            this.numPosX.TabIndex = 0;
            this.numPosX.Text = "0";
            this.numPosX.ValueChanged += new System.EventHandler(this.numPosX_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.numPosZ);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numPosX);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numPosY);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(111, 82);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Position";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.numNormZ);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.numNormX);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.numNormY);
            this.groupBox2.Location = new System.Drawing.Point(231, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(111, 82);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Normal";
            this.groupBox2.Visible = false;
            // 
            // numNormZ
            // 
            this.numNormZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numNormZ.Integral = false;
            this.numNormZ.Location = new System.Drawing.Point(27, 54);
            this.numNormZ.MaximumValue = 3.402823E+38F;
            this.numNormZ.MinimumValue = -3.402823E+38F;
            this.numNormZ.Name = "numNormZ";
            this.numNormZ.Size = new System.Drawing.Size(78, 20);
            this.numNormZ.TabIndex = 6;
            this.numNormZ.Text = "0";
            this.numNormZ.ValueChanged += new System.EventHandler(this.numNormZ_ValueChanged);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(6, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Z: ";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numNormX
            // 
            this.numNormX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numNormX.Integral = false;
            this.numNormX.Location = new System.Drawing.Point(27, 16);
            this.numNormX.MaximumValue = 3.402823E+38F;
            this.numNormX.MinimumValue = -3.402823E+38F;
            this.numNormX.Name = "numNormX";
            this.numNormX.Size = new System.Drawing.Size(78, 20);
            this.numNormX.TabIndex = 0;
            this.numNormX.Text = "0";
            this.numNormX.ValueChanged += new System.EventHandler(this.numNormX_ValueChanged);
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 20);
            this.label5.TabIndex = 3;
            this.label5.Text = "X: ";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(6, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "Y: ";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numNormY
            // 
            this.numNormY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numNormY.Integral = false;
            this.numNormY.Location = new System.Drawing.Point(27, 35);
            this.numNormY.MaximumValue = 3.402823E+38F;
            this.numNormY.MinimumValue = -3.402823E+38F;
            this.numNormY.Name = "numNormY";
            this.numNormY.Size = new System.Drawing.Size(78, 20);
            this.numNormY.TabIndex = 4;
            this.numNormY.Text = "0";
            this.numNormY.ValueChanged += new System.EventHandler(this.numNormY_ValueChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.colorBox);
            this.groupBox4.Controls.Add(this.colorIndex);
            this.groupBox4.Location = new System.Drawing.Point(348, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(111, 82);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Color";
            this.groupBox4.Visible = false;
            // 
            // colorBox
            // 
            this.colorBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.colorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.colorBox.Location = new System.Drawing.Point(6, 36);
            this.colorBox.Name = "colorBox";
            this.colorBox.Size = new System.Drawing.Size(99, 38);
            this.colorBox.TabIndex = 12;
            this.colorBox.DoubleClick += new System.EventHandler(this.colorBox_Click);
            // 
            // colorIndex
            // 
            this.colorIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorIndex.FormattingEnabled = true;
            this.colorIndex.Items.AddRange(new object[] {
            "Color 0",
            "Color 1"});
            this.colorIndex.Location = new System.Drawing.Point(6, 14);
            this.colorIndex.Name = "colorIndex";
            this.colorIndex.Size = new System.Drawing.Size(99, 21);
            this.colorIndex.TabIndex = 7;
            this.colorIndex.SelectedIndexChanged += new System.EventHandler(this.colorIndex_SelectedIndexChanged);
            // 
            // btnAverage
            // 
            this.btnAverage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAverage.Enabled = false;
            this.btnAverage.Location = new System.Drawing.Point(120, 54);
            this.btnAverage.Name = "btnAverage";
            this.btnAverage.Size = new System.Drawing.Size(105, 23);
            this.btnAverage.TabIndex = 12;
            this.btnAverage.Text = "Average";
            this.btnAverage.UseVisualStyleBackColor = true;
            this.btnAverage.Click += new System.EventHandler(this.btnAverage_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(120, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "No vertices selected";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(98, -22);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, -19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Facepoint Index:";
            // 
            // VertexEditor
            // 
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnAverage);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(230, 85);
            this.Name = "VertexEditor";
            this.Size = new System.Drawing.Size(230, 85);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
        public IBoneNode TargetBone { get { return _mainWindow.SelectedBone; } set { _mainWindow.SelectedBone = value; } }

        private Label label3;
        public NumericInputBox numPosZ;
        private Label label2;
        public NumericInputBox numPosY;
        private Label label1;
        public NumericInputBox numPosX;

        private int _uvIndex = 0, _colorIndex = 0;

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

        public bool _updating = false;
        private Button btnAverage;
        private Label label7;
        private ComboBox comboBox1;
        private Label label8;

        private GoodColorDialog _dlgColor;
        private void colorBox_Click(object sender, EventArgs e)
        {
            if (TargetVertex == null)
                return;

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
                    return _targetVertices[0];
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
                    Enabled = true;
            }
            else
            {
                if (Enabled != false)
                    Enabled = false;
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
                    label7.Text = String.Format("{0} vertices selected", _targetVertices.Count);
                else
                    label7.Text = "No vertices selected";

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

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Vertex3> TargetVertices 
        {
            get { return _targetVertices; }
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
                return;

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
                return;

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
                return;

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
                point += v.WeightedPosition;
            point /= _targetVertices.Count;
            foreach (Vertex3 v in _targetVertices)
                v.WeightedPosition = point;
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
