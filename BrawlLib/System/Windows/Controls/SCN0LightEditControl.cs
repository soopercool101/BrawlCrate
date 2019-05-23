using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class SCN0LightEditControl : UserControl
    {
        private int _numFrames;

        private int _currentPage = 1;
        private LightAnimationFrame _currentFrame = LightAnimationFrame.Empty;
        private NumericInputBox[] _boxes = new NumericInputBox[10];
        private Panel panel1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private Label label9;
        private Label label8;
        private Label label1;
        private NumericInputBox numStartZ;
        private NumericInputBox numEndY;
        private NumericInputBox numEndX;
        private NumericInputBox numStartX;
        private NumericInputBox numEndZ;
        private NumericInputBox numStartY;
        private Label label4;
        private Label label5;
        private Label label2;
        private Label label3;
        private Label label6;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private Label label10;
        private NumericInputBox numRefBright;
        private NumericInputBox numRefDist;
        private NumericInputBox numSpotCut;
        private CLRControl lightCtrl;
        private CLRControl specCtrl;
        private Panel panel2;
        private NumericInputBox numSpotBright;
        private TabPage tabPage2;
        private VisEditor visEditor1;

        private SCN0LightNode _target;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SCN0LightNode TargetSequence
        {
            get { return _target; }
            set
            {
                if (_target == value)
                    return;

                _target = value;
                UpdateTarget();
            }
        }

        public SCN0LightEditControl() 
        {
            InitializeComponent();
            _boxes[0] = numStartX;
            _boxes[1] = numStartY;
            _boxes[2] = numStartZ;
            _boxes[3] = numEndX;
            _boxes[4] = numEndY;
            _boxes[5] = numEndZ;
            _boxes[6] = numRefDist;
            _boxes[7] = numRefBright;
            _boxes[8] = numSpotCut;
            _boxes[9] = numSpotBright;

            for (int i = 0; i < 10; i++)
                _boxes[i].Tag = i;

            specCtrl._colorId = 1;
        }

        private void UpdateTarget()
        {
            listKeyframes.BeginUpdate();
            listKeyframes.Items.Clear();
            if (_target != null)
            {
                if (_target.FrameCount > 0)
                {
                    LightAnimationFrame a;
                    for (int x = 0; x < _target.FrameCount; x++)
                        if ((a = _target.GetAnimFrame(x)).HasKeys)
                            listKeyframes.Items.Add(a);

                    _numFrames = _target.FrameCount;

                    _currentPage = 0;
                    numFrame.Value = 1;
                    numFrame.Maximum = _numFrames;
                    lblFrameCount.Text = String.Format("/ {0}", _numFrames);
                }
                else
                    numFrame.Value = 1;
            }
            listKeyframes.EndUpdate();
            visEditor1.TargetNode = TargetSequence;
            lightCtrl.ColorSource = TargetSequence;
            specCtrl.ColorSource = TargetSequence;
            RefreshPage();
        }

        private void numFrame_ValueChanged(object sender, EventArgs e)
        {
            int page = (int)numFrame.Value - 1;
            if (_currentPage != page)
            {
                _currentPage = page;
                RefreshPage();
            }
        }

        private void RefreshPage()
        {
            if (_target != null)
            {
                //_currentPage = (int)numFrame.Value - 1;

                _currentFrame = _target.GetAnimFrame(_currentPage);

                numStartX.Value = _currentFrame.Start._x;
                numStartY.Value = _currentFrame.Start._y;
                numStartZ.Value = _currentFrame.Start._z;

                numEndX.Value = _currentFrame.End._x;
                numEndY.Value = _currentFrame.End._y;
                numEndZ.Value = _currentFrame.End._z;

                numSpotCut.Value = _currentFrame.SpotCutoff;
                numSpotBright.Value = _currentFrame.SpotBright;
                numRefDist.Value = _currentFrame.RefDist;
                numRefBright.Value = _currentFrame.RefBright;

                for (int i = 0; i < 10; i++)
                    UpdateBox(i);

                btnPrev.Enabled = _currentPage > 0;
                btnNext.Enabled = _currentPage < (_numFrames - 1);

                listKeyframes.SelectedIndex = FindKeyframe(_currentPage);
            }
        }

        private int FindKeyframe(int index)
        {
            int count = listKeyframes.Items.Count;
            for (int i = 0; i < count; i++)
                if (((LightAnimationFrame)listKeyframes.Items[i]).Index == index)
                    return i;
            return -1;
        }

        private void UpdateBox(int index)
        {
            if (_target.GetKeyframe((LightKeyframeMode)index, _currentPage) != null)
                _boxes[index].BackColor = Color.Yellow;
            else
                _boxes[index].BackColor = Color.White;
        }

        private unsafe void BoxChanged(object sender, EventArgs e)
        {
            NumericInputBox box = sender as NumericInputBox;
            LightAnimationFrame kf;
            float* pkf = (float*)&kf;
            float val = box.Value;
            int index = (int)box.Tag;
            int x;

            if (val != _currentFrame[index])
            {
                int kfIndex = FindKeyframe(_currentPage);

                if (float.IsNaN(val))
                {
                    //Value removed find keyframe and zero it out
                    if (kfIndex >= 0)
                    {
                        kf = (LightAnimationFrame)listKeyframes.Items[kfIndex];
                        kf.SetBools(index, false);
                        pkf[index] = val;
                        for (x = 0; (x < 10) && float.IsNaN(pkf[x]); x++) ;
                        if (x == 10)
                        {
                            listKeyframes.Items.RemoveAt(kfIndex);
                            listKeyframes.SelectedIndex = -1;
                        }
                        else
                            listKeyframes.Items[kfIndex] = kf;
                    }

                    _target.RemoveKeyframe((LightKeyframeMode)index, _currentPage);
                    val = _target.GetAnimFrame(_currentPage)[index];
                    box.Value = val;
                }
                else
                {
                    if (kfIndex >= 0)
                    {
                        kf = (LightAnimationFrame)listKeyframes.Items[kfIndex];
                        kf.SetBools(index, true);
                        pkf[index] = val;
                        listKeyframes.Items[kfIndex] = kf;
                    }
                    else
                    {
                        kf = LightAnimationFrame.Empty;
                        kf.SetBools(index, true);
                        kf.Index = _currentPage;
                        pkf[index] = val;

                        int count = listKeyframes.Items.Count;
                        for (x = 0; (x < count) && (((LightAnimationFrame)listKeyframes.Items[x]).Index < _currentPage); x++) ;

                        listKeyframes.Items.Insert(x, kf);
                        listKeyframes.SelectedIndex = x;
                    }

                    _target.SetKeyframe((LightKeyframeMode)index, _currentPage, val);
                }

                _currentFrame[index] = val;
                UpdateBox(index);
            }
        }

        private void listKeyframes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listKeyframes.SelectedIndex;
            if (index >= 0)
            {
                LightAnimationFrame f = (LightAnimationFrame)listKeyframes.SelectedItem;
                numFrame.Value = (decimal)(f.Index + 1);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e) { numFrame.Value--; }
        private void btnNext_Click(object sender, EventArgs e) { numFrame.Value++; }

        #region Designer

        private Label label7;
        private NumericUpDown numFrame;
        private Label lblFrameCount;
        private Button btnPrev;
        private Button btnNext;
        private ListBox listKeyframes;
        private GroupBox groupBox1;

        private void InitializeComponent()
        {
            this.label7 = new System.Windows.Forms.Label();
            this.numFrame = new System.Windows.Forms.NumericUpDown();
            this.lblFrameCount = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.listKeyframes = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numSpotBright = new System.Windows.Forms.NumericInputBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numRefBright = new System.Windows.Forms.NumericInputBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numStartY = new System.Windows.Forms.NumericInputBox();
            this.label10 = new System.Windows.Forms.Label();
            this.numEndZ = new System.Windows.Forms.NumericInputBox();
            this.numStartX = new System.Windows.Forms.NumericInputBox();
            this.numRefDist = new System.Windows.Forms.NumericInputBox();
            this.numEndX = new System.Windows.Forms.NumericInputBox();
            this.numEndY = new System.Windows.Forms.NumericInputBox();
            this.numSpotCut = new System.Windows.Forms.NumericInputBox();
            this.numStartZ = new System.Windows.Forms.NumericInputBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.visEditor1 = new System.Windows.Forms.VisEditor();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lightCtrl = new System.Windows.Forms.CLRControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.specCtrl = new System.Windows.Forms.CLRControl();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(33, 5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "Frame:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numFrame
            // 
            this.numFrame.Location = new System.Drawing.Point(80, 5);
            this.numFrame.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFrame.Name = "numFrame";
            this.numFrame.Size = new System.Drawing.Size(58, 20);
            this.numFrame.TabIndex = 0;
            this.numFrame.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFrame.ValueChanged += new System.EventHandler(this.numFrame_ValueChanged);
            // 
            // lblFrameCount
            // 
            this.lblFrameCount.Location = new System.Drawing.Point(144, 5);
            this.lblFrameCount.Name = "lblFrameCount";
            this.lblFrameCount.Size = new System.Drawing.Size(51, 20);
            this.lblFrameCount.TabIndex = 17;
            this.lblFrameCount.Text = "/ 10";
            this.lblFrameCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPrev
            // 
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.Location = new System.Drawing.Point(192, 3);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(23, 23);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "<";
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(217, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(23, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = ">";
            this.btnNext.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // listKeyframes
            // 
            this.listKeyframes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listKeyframes.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listKeyframes.FormattingEnabled = true;
            this.listKeyframes.IntegralHeight = false;
            this.listKeyframes.ItemHeight = 14;
            this.listKeyframes.Location = new System.Drawing.Point(3, 16);
            this.listKeyframes.Name = "listKeyframes";
            this.listKeyframes.Size = new System.Drawing.Size(279, 104);
            this.listKeyframes.TabIndex = 18;
            this.listKeyframes.SelectedIndexChanged += new System.EventHandler(this.listKeyframes_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listKeyframes);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(285, 123);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Keyframes";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(293, 276);
            this.panel1.TabIndex = 23;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(293, 276);
            this.tabControl1.TabIndex = 18;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(285, 250);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Lighting";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.numSpotBright);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.numRefBright);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.btnNext);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.btnPrev);
            this.panel2.Controls.Add(this.numStartY);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.numEndZ);
            this.panel2.Controls.Add(this.lblFrameCount);
            this.panel2.Controls.Add(this.numStartX);
            this.panel2.Controls.Add(this.numRefDist);
            this.panel2.Controls.Add(this.numEndX);
            this.panel2.Controls.Add(this.numFrame);
            this.panel2.Controls.Add(this.numEndY);
            this.panel2.Controls.Add(this.numSpotCut);
            this.panel2.Controls.Add(this.numStartZ);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 123);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(285, 127);
            this.panel2.TabIndex = 22;
            // 
            // numSpotBright
            // 
            this.numSpotBright.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numSpotBright.Integral = false;
            this.numSpotBright.Location = new System.Drawing.Point(211, 84);
            this.numSpotBright.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numSpotBright.MaximumValue = 3.402823E+38F;
            this.numSpotBright.MinimumValue = -3.402823E+38F;
            this.numSpotBright.Name = "numSpotBright";
            this.numSpotBright.Size = new System.Drawing.Size(70, 20);
            this.numSpotBright.TabIndex = 19;
            this.numSpotBright.Text = "0";
            this.numSpotBright.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(211, 27);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 20);
            this.label6.TabIndex = 8;
            this.label6.Text = "Z";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numRefBright
            // 
            this.numRefBright.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRefBright.Integral = false;
            this.numRefBright.Location = new System.Drawing.Point(211, 103);
            this.numRefBright.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numRefBright.MaximumValue = 3.402823E+38F;
            this.numRefBright.MinimumValue = -3.402823E+38F;
            this.numRefBright.Name = "numRefBright";
            this.numRefBright.Size = new System.Drawing.Size(70, 20);
            this.numRefBright.TabIndex = 13;
            this.numRefBright.Text = "0";
            this.numRefBright.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(4, 65);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "End Points";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(4, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Spot Cutoff";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(142, 27);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 20);
            this.label5.TabIndex = 7;
            this.label5.Text = "Y";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(73, 27);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "X";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numStartY
            // 
            this.numStartY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numStartY.Integral = false;
            this.numStartY.Location = new System.Drawing.Point(142, 46);
            this.numStartY.Margin = new System.Windows.Forms.Padding(0);
            this.numStartY.MaximumValue = 3.402823E+38F;
            this.numStartY.MinimumValue = -3.402823E+38F;
            this.numStartY.Name = "numStartY";
            this.numStartY.Size = new System.Drawing.Size(70, 20);
            this.numStartY.TabIndex = 4;
            this.numStartY.Text = "0";
            this.numStartY.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(142, 84);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 20);
            this.label10.TabIndex = 14;
            this.label10.Text = "Spec Shine";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numEndZ
            // 
            this.numEndZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numEndZ.Integral = false;
            this.numEndZ.Location = new System.Drawing.Point(211, 65);
            this.numEndZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numEndZ.MaximumValue = 3.402823E+38F;
            this.numEndZ.MinimumValue = -3.402823E+38F;
            this.numEndZ.Name = "numEndZ";
            this.numEndZ.Size = new System.Drawing.Size(70, 20);
            this.numEndZ.TabIndex = 8;
            this.numEndZ.Text = "0";
            this.numEndZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numStartX
            // 
            this.numStartX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numStartX.Integral = false;
            this.numStartX.Location = new System.Drawing.Point(73, 46);
            this.numStartX.Margin = new System.Windows.Forms.Padding(0);
            this.numStartX.MaximumValue = 3.402823E+38F;
            this.numStartX.MinimumValue = -3.402823E+38F;
            this.numStartX.Name = "numStartX";
            this.numStartX.Size = new System.Drawing.Size(70, 20);
            this.numStartX.TabIndex = 3;
            this.numStartX.Text = "0";
            this.numStartX.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numRefDist
            // 
            this.numRefDist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRefDist.Integral = false;
            this.numRefDist.Location = new System.Drawing.Point(73, 103);
            this.numRefDist.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numRefDist.MaximumValue = 3.402823E+38F;
            this.numRefDist.MinimumValue = -3.402823E+38F;
            this.numRefDist.Name = "numRefDist";
            this.numRefDist.Size = new System.Drawing.Size(70, 20);
            this.numRefDist.TabIndex = 12;
            this.numRefDist.Text = "0";
            this.numRefDist.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numEndX
            // 
            this.numEndX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numEndX.Integral = false;
            this.numEndX.Location = new System.Drawing.Point(73, 65);
            this.numEndX.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numEndX.MaximumValue = 3.402823E+38F;
            this.numEndX.MinimumValue = -3.402823E+38F;
            this.numEndX.Name = "numEndX";
            this.numEndX.Size = new System.Drawing.Size(70, 20);
            this.numEndX.TabIndex = 6;
            this.numEndX.Text = "0";
            this.numEndX.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numEndY
            // 
            this.numEndY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numEndY.Integral = false;
            this.numEndY.Location = new System.Drawing.Point(142, 65);
            this.numEndY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numEndY.MaximumValue = 3.402823E+38F;
            this.numEndY.MinimumValue = -3.402823E+38F;
            this.numEndY.Name = "numEndY";
            this.numEndY.Size = new System.Drawing.Size(70, 20);
            this.numEndY.TabIndex = 7;
            this.numEndY.Text = "0";
            this.numEndY.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numSpotCut
            // 
            this.numSpotCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numSpotCut.Integral = false;
            this.numSpotCut.Location = new System.Drawing.Point(73, 84);
            this.numSpotCut.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numSpotCut.MaximumValue = 3.402823E+38F;
            this.numSpotCut.MinimumValue = -3.402823E+38F;
            this.numSpotCut.Name = "numSpotCut";
            this.numSpotCut.Size = new System.Drawing.Size(70, 20);
            this.numSpotCut.TabIndex = 11;
            this.numSpotCut.Text = "0";
            this.numSpotCut.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numStartZ
            // 
            this.numStartZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numStartZ.Integral = false;
            this.numStartZ.Location = new System.Drawing.Point(211, 46);
            this.numStartZ.Margin = new System.Windows.Forms.Padding(0);
            this.numStartZ.MaximumValue = 3.402823E+38F;
            this.numStartZ.MinimumValue = -3.402823E+38F;
            this.numStartZ.Name = "numStartZ";
            this.numStartZ.Size = new System.Drawing.Size(70, 20);
            this.numStartZ.TabIndex = 5;
            this.numStartZ.Text = "0";
            this.numStartZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(142, 103);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 20);
            this.label9.TabIndex = 10;
            this.label9.Text = "Ref Bright";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(4, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start Points";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(4, 103);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 20);
            this.label8.TabIndex = 9;
            this.label8.Text = "Ref Dist";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.visEditor1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(285, 250);
            this.tabPage2.TabIndex = 4;
            this.tabPage2.Text = "Enabled";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // visEditor1
            // 
            this.visEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visEditor1.Location = new System.Drawing.Point(0, 0);
            this.visEditor1.Name = "visEditor1";
            this.visEditor1.Size = new System.Drawing.Size(285, 250);
            this.visEditor1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lightCtrl);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(285, 250);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Light Color";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lightCtrl
            // 
            this.lightCtrl.ColorID = 0;
            this.lightCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightCtrl.Location = new System.Drawing.Point(0, 0);
            this.lightCtrl.Name = "lightCtrl";
            this.lightCtrl.Size = new System.Drawing.Size(285, 250);
            this.lightCtrl.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.specCtrl);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(285, 250);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Specular Color";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // specCtrl
            // 
            this.specCtrl.ColorID = 0;
            this.specCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.specCtrl.Location = new System.Drawing.Point(0, 0);
            this.specCtrl.Name = "specCtrl";
            this.specCtrl.Size = new System.Drawing.Size(285, 250);
            this.specCtrl.TabIndex = 0;
            // 
            // SCN0LightEditControl
            // 
            this.Controls.Add(this.panel1);
            this.Name = "SCN0LightEditControl";
            this.Size = new System.Drawing.Size(293, 276);
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
