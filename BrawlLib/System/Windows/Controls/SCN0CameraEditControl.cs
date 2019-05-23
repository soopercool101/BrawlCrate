using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class SCN0CameraEditControl : UserControl
    {
        private int _numFrames;

        private int _currentPage = 1;
        private CameraAnimationFrame _currentFrame = CameraAnimationFrame.Empty;
        private NumericInputBox[] _boxes = new NumericInputBox[15];
        private GroupBox groupBox1;
        private ListBox listKeyframes;
        private Panel panel2;
        private NumericInputBox numAimZ;
        private Label label7;
        private Label label6;
        private NumericInputBox numFovY;
        private Label label3;
        private Label label2;
        private Button btnNext;
        private Label label5;
        private Label label4;
        private Button btnPrev;
        private NumericInputBox numPosY;
        private NumericInputBox numRotZ;
        private Label lblFrameCount;
        private NumericInputBox numPosX;
        private NumericInputBox numTwist;
        private NumericInputBox numRotX;
        private NumericUpDown numFrame;
        private NumericInputBox numRotY;
        private NumericInputBox numAimX;
        private NumericInputBox numPosZ;
        private Label label9;
        private Label label1;
        private Label label8;
        private NumericInputBox numAimY;
        private NumericInputBox numAspect;
        private NumericInputBox numHeight;
        private Label label10;
        private Label label11;
        private NumericInputBox numFarZ;
        private NumericInputBox numNearZ;
        private Label label12;
        private Label label13;

        private SCN0CameraNode _target;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SCN0CameraNode TargetSequence
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

        public SCN0CameraEditControl() 
        { 
            InitializeComponent();
            _boxes[0] = numPosX;
            _boxes[1] = numPosY;
            _boxes[2] = numPosZ;
            _boxes[3] = numAspect;
            _boxes[4] = numNearZ;
            _boxes[5] = numFarZ;
            _boxes[6] = numRotX;
            _boxes[7] = numRotY;
            _boxes[8] = numRotZ;
            _boxes[9] = numAimX;
            _boxes[10] = numAimY;
            _boxes[11] = numAimZ;
            _boxes[12] = numTwist;
            _boxes[13] = numFovY;
            _boxes[14] = numHeight;

            for (int i = 0; i < 15; i++)
                _boxes[i].Tag = i;
        }

        private void UpdateTarget()
        {
            listKeyframes.BeginUpdate();
            listKeyframes.Items.Clear();
            if (_target != null)
            {
                if (_target.FrameCount > 0)
                {
                    CameraAnimationFrame a;
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
                _currentFrame = _target.GetAnimFrame(_currentPage);

                numPosX.Value = _currentFrame.Pos._x;
                numPosY.Value = _currentFrame.Pos._y;
                numPosZ.Value = _currentFrame.Pos._z;

                numRotX.Value = _currentFrame.Rot._x;
                numRotY.Value = _currentFrame.Rot._y;
                numRotZ.Value = _currentFrame.Rot._z;

                numAimX.Value = _currentFrame.Aim._x;
                numAimY.Value = _currentFrame.Aim._y;
                numAimZ.Value = _currentFrame.Aim._z;

                numTwist.Value = _currentFrame.Twist;
                numFovY.Value = _currentFrame.FovY;
                numHeight.Value = _currentFrame.Height;
                numAspect.Value = _currentFrame.Aspect;
                numNearZ.Value = _currentFrame.NearZ;
                numFarZ.Value = _currentFrame.FarZ;

                for (int i = 0; i < 15; i++)
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
                if (((CameraAnimationFrame)listKeyframes.Items[i]).Index == index)
                    return i;
            return -1;
        }

        private void UpdateBox(int index)
        {
            if (_target.GetKeyframe((CameraKeyframeMode)index, _currentPage) != null)
                _boxes[index].BackColor = Color.Yellow;
            else
                _boxes[index].BackColor = Color.White;
        }

        private unsafe void BoxChanged(object sender, EventArgs e)
        {
            NumericInputBox box = sender as NumericInputBox;
            CameraAnimationFrame kf;
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
                        kf = (CameraAnimationFrame)listKeyframes.Items[kfIndex];
                        kf.SetBools(index, false);
                        pkf[index] = val;
                        for (x = 0; (x < 15) && float.IsNaN(pkf[x]); x++) ;
                        if (x == 15)
                        {
                            listKeyframes.Items.RemoveAt(kfIndex);
                            listKeyframes.SelectedIndex = -1;
                        }
                        else
                            listKeyframes.Items[kfIndex] = kf;
                    }

                    _target.RemoveKeyframe((CameraKeyframeMode)index, _currentPage);
                    val = _target.GetAnimFrame(_currentPage)[index];
                    box.Value = val;
                }
                else
                {
                    if (kfIndex >= 0)
                    {
                        kf = (CameraAnimationFrame)listKeyframes.Items[kfIndex];
                        kf.SetBools(index, true);
                        pkf[index] = val;
                        listKeyframes.Items[kfIndex] = kf;
                    }
                    else
                    {
                        kf = CameraAnimationFrame.Empty;
                        kf.SetBools(index, true);
                        kf.Index = _currentPage;
                        pkf[index] = val;

                        int count = listKeyframes.Items.Count;
                        for (x = 0; (x < count) && (((CameraAnimationFrame)listKeyframes.Items[x]).Index < _currentPage); x++) ;

                        listKeyframes.Items.Insert(x, kf);
                        listKeyframes.SelectedIndex = x;
                    }

                    _target.SetKeyframe((CameraKeyframeMode)index, _currentPage, val);
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
                CameraAnimationFrame f = (CameraAnimationFrame)listKeyframes.SelectedItem;
                numFrame.Value = (decimal)(f.Index + 1);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e) { numFrame.Value--; }
        private void btnNext_Click(object sender, EventArgs e) { numFrame.Value++; }

        #region Designer
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listKeyframes = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numFarZ = new System.Windows.Forms.NumericInputBox();
            this.numNearZ = new System.Windows.Forms.NumericInputBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.numAspect = new System.Windows.Forms.NumericInputBox();
            this.numHeight = new System.Windows.Forms.NumericInputBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.numAimY = new System.Windows.Forms.NumericInputBox();
            this.numAimZ = new System.Windows.Forms.NumericInputBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numFovY = new System.Windows.Forms.NumericInputBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.numPosY = new System.Windows.Forms.NumericInputBox();
            this.numRotZ = new System.Windows.Forms.NumericInputBox();
            this.lblFrameCount = new System.Windows.Forms.Label();
            this.numPosX = new System.Windows.Forms.NumericInputBox();
            this.numTwist = new System.Windows.Forms.NumericInputBox();
            this.numRotX = new System.Windows.Forms.NumericInputBox();
            this.numFrame = new System.Windows.Forms.NumericUpDown();
            this.numRotY = new System.Windows.Forms.NumericInputBox();
            this.numAimX = new System.Windows.Forms.NumericInputBox();
            this.numPosZ = new System.Windows.Forms.NumericInputBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listKeyframes);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(286, 109);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Keyframes";
            // 
            // listKeyframes
            // 
            this.listKeyframes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listKeyframes.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listKeyframes.FormattingEnabled = true;
            this.listKeyframes.HorizontalScrollbar = true;
            this.listKeyframes.IntegralHeight = false;
            this.listKeyframes.ItemHeight = 14;
            this.listKeyframes.Location = new System.Drawing.Point(3, 16);
            this.listKeyframes.Name = "listKeyframes";
            this.listKeyframes.Size = new System.Drawing.Size(280, 90);
            this.listKeyframes.TabIndex = 18;
            this.listKeyframes.SelectedIndexChanged += new System.EventHandler(this.listKeyframes_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.numFarZ);
            this.panel2.Controls.Add(this.numNearZ);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.numAspect);
            this.panel2.Controls.Add(this.numHeight);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.numAimY);
            this.panel2.Controls.Add(this.numAimZ);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.numFovY);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.btnNext);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.btnPrev);
            this.panel2.Controls.Add(this.numPosY);
            this.panel2.Controls.Add(this.numRotZ);
            this.panel2.Controls.Add(this.lblFrameCount);
            this.panel2.Controls.Add(this.numPosX);
            this.panel2.Controls.Add(this.numTwist);
            this.panel2.Controls.Add(this.numRotX);
            this.panel2.Controls.Add(this.numFrame);
            this.panel2.Controls.Add(this.numRotY);
            this.panel2.Controls.Add(this.numAimX);
            this.panel2.Controls.Add(this.numPosZ);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 109);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(286, 167);
            this.panel2.TabIndex = 25;
            // 
            // numFarZ
            // 
            this.numFarZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numFarZ.Location = new System.Drawing.Point(211, 141);
            this.numFarZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numFarZ.Name = "numFarZ";
            this.numFarZ.Size = new System.Drawing.Size(70, 20);
            this.numFarZ.TabIndex = 29;
            this.numFarZ.Text = "0";
            this.numFarZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numNearZ
            // 
            this.numNearZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numNearZ.Location = new System.Drawing.Point(73, 141);
            this.numNearZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numNearZ.Name = "numNearZ";
            this.numNearZ.Size = new System.Drawing.Size(70, 20);
            this.numNearZ.TabIndex = 28;
            this.numNearZ.Text = "0";
            this.numNearZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(142, 141);
            this.label12.Margin = new System.Windows.Forms.Padding(0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(70, 20);
            this.label12.TabIndex = 27;
            this.label12.Text = "Far Z";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(4, 141);
            this.label13.Margin = new System.Windows.Forms.Padding(0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 20);
            this.label13.TabIndex = 26;
            this.label13.Text = "Near Z";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numAspect
            // 
            this.numAspect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numAspect.Location = new System.Drawing.Point(211, 122);
            this.numAspect.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numAspect.Name = "numAspect";
            this.numAspect.Size = new System.Drawing.Size(70, 20);
            this.numAspect.TabIndex = 25;
            this.numAspect.Text = "0";
            this.numAspect.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numHeight
            // 
            this.numHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numHeight.Location = new System.Drawing.Point(73, 122);
            this.numHeight.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(70, 20);
            this.numHeight.TabIndex = 24;
            this.numHeight.Text = "0";
            this.numHeight.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(142, 122);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 20);
            this.label10.TabIndex = 23;
            this.label10.Text = "Aspect";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(4, 122);
            this.label11.Margin = new System.Windows.Forms.Padding(0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 20);
            this.label11.TabIndex = 22;
            this.label11.Text = "Height";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numAimY
            // 
            this.numAimY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numAimY.Location = new System.Drawing.Point(142, 84);
            this.numAimY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numAimY.Name = "numAimY";
            this.numAimY.Size = new System.Drawing.Size(70, 20);
            this.numAimY.TabIndex = 21;
            this.numAimY.Text = "0";
            this.numAimY.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numAimZ
            // 
            this.numAimZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numAimZ.Location = new System.Drawing.Point(211, 84);
            this.numAimZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numAimZ.Name = "numAimZ";
            this.numAimZ.Size = new System.Drawing.Size(70, 20);
            this.numAimZ.TabIndex = 19;
            this.numAimZ.Text = "0";
            this.numAimZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(70, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "Frame:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // numFovY
            // 
            this.numFovY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numFovY.Location = new System.Drawing.Point(211, 103);
            this.numFovY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numFovY.Name = "numFovY";
            this.numFovY.Size = new System.Drawing.Size(70, 20);
            this.numFovY.TabIndex = 13;
            this.numFovY.Text = "0";
            this.numFovY.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(4, 65);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Rotation";
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
            this.label2.Text = "Aim";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(241, 2);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(23, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = ">";
            this.btnNext.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
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
            // btnPrev
            // 
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.Location = new System.Drawing.Point(216, 2);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(23, 23);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "<";
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // numPosY
            // 
            this.numPosY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosY.Location = new System.Drawing.Point(142, 46);
            this.numPosY.Margin = new System.Windows.Forms.Padding(0);
            this.numPosY.Name = "numPosY";
            this.numPosY.Size = new System.Drawing.Size(70, 20);
            this.numPosY.TabIndex = 4;
            this.numPosY.Text = "0";
            this.numPosY.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numRotZ
            // 
            this.numRotZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotZ.Location = new System.Drawing.Point(211, 65);
            this.numRotZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numRotZ.Name = "numRotZ";
            this.numRotZ.Size = new System.Drawing.Size(70, 20);
            this.numRotZ.TabIndex = 8;
            this.numRotZ.Text = "0";
            this.numRotZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // lblFrameCount
            // 
            this.lblFrameCount.Location = new System.Drawing.Point(181, 4);
            this.lblFrameCount.Name = "lblFrameCount";
            this.lblFrameCount.Size = new System.Drawing.Size(29, 20);
            this.lblFrameCount.TabIndex = 17;
            this.lblFrameCount.Text = "/ 10";
            this.lblFrameCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numPosX
            // 
            this.numPosX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosX.Location = new System.Drawing.Point(73, 46);
            this.numPosX.Margin = new System.Windows.Forms.Padding(0);
            this.numPosX.Name = "numPosX";
            this.numPosX.Size = new System.Drawing.Size(70, 20);
            this.numPosX.TabIndex = 3;
            this.numPosX.Text = "0";
            this.numPosX.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numTwist
            // 
            this.numTwist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTwist.Location = new System.Drawing.Point(73, 103);
            this.numTwist.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numTwist.Name = "numTwist";
            this.numTwist.Size = new System.Drawing.Size(70, 20);
            this.numTwist.TabIndex = 12;
            this.numTwist.Text = "0";
            this.numTwist.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numRotX
            // 
            this.numRotX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotX.Location = new System.Drawing.Point(73, 65);
            this.numRotX.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numRotX.Name = "numRotX";
            this.numRotX.Size = new System.Drawing.Size(70, 20);
            this.numRotX.TabIndex = 6;
            this.numRotX.Text = "0";
            this.numRotX.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numFrame
            // 
            this.numFrame.Location = new System.Drawing.Point(117, 4);
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
            // numRotY
            // 
            this.numRotY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotY.Location = new System.Drawing.Point(142, 65);
            this.numRotY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numRotY.Name = "numRotY";
            this.numRotY.Size = new System.Drawing.Size(70, 20);
            this.numRotY.TabIndex = 7;
            this.numRotY.Text = "0";
            this.numRotY.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numAimX
            // 
            this.numAimX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numAimX.Location = new System.Drawing.Point(73, 84);
            this.numAimX.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numAimX.Name = "numAimX";
            this.numAimX.Size = new System.Drawing.Size(70, 20);
            this.numAimX.TabIndex = 11;
            this.numAimX.Text = "0";
            this.numAimX.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numPosZ
            // 
            this.numPosZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosZ.Location = new System.Drawing.Point(211, 46);
            this.numPosZ.Margin = new System.Windows.Forms.Padding(0);
            this.numPosZ.Name = "numPosZ";
            this.numPosZ.Size = new System.Drawing.Size(70, 20);
            this.numPosZ.TabIndex = 5;
            this.numPosZ.Text = "0";
            this.numPosZ.ValueChanged += new System.EventHandler(this.BoxChanged);
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
            this.label9.Text = "Fov Y";
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
            this.label1.Text = "Position";
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
            this.label8.Text = "Twist";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SCN0CameraEditControl
            // 
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Name = "SCN0CameraEditControl";
            this.Size = new System.Drawing.Size(286, 276);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
