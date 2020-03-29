using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public class SCN0CameraEditControl : UserControl
    {
        private int _numFrames;

        private int _currentPage = 1;
        private CameraAnimationFrame _currentFrame = CameraAnimationFrame.Empty;
        private readonly NumericInputBox[] _boxes = new NumericInputBox[15];
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

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SCN0CameraNode TargetSequence
        {
            get => _target;
            set
            {
                if (_target == value)
                {
                    return;
                }

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
            {
                _boxes[i].Tag = i;
            }
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
                    {
                        if ((a = _target.GetAnimFrame(x)).HasKeys)
                        {
                            listKeyframes.Items.Add(a);
                        }
                    }

                    _numFrames = _target.FrameCount;

                    _currentPage = 0;
                    numFrame.Value = 1;
                    numFrame.Maximum = _numFrames;
                    lblFrameCount.Text = $"/ {_numFrames}";
                }
                else
                {
                    numFrame.Value = 1;
                }
            }

            listKeyframes.EndUpdate();

            RefreshPage();
        }

        private void numFrame_ValueChanged(object sender, EventArgs e)
        {
            int page = (int) numFrame.Value - 1;
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
                {
                    UpdateBox(i);
                }

                btnPrev.Enabled = _currentPage > 0;
                btnNext.Enabled = _currentPage < _numFrames - 1;

                listKeyframes.SelectedIndex = FindKeyframe(_currentPage);
            }
        }

        private int FindKeyframe(int index)
        {
            int count = listKeyframes.Items.Count;
            for (int i = 0; i < count; i++)
            {
                if (((CameraAnimationFrame) listKeyframes.Items[i]).Index == index)
                {
                    return i;
                }
            }

            return -1;
        }

        private void UpdateBox(int index)
        {
            if (_target.GetKeyframe((CameraKeyframeMode) index, _currentPage) != null)
            {
                _boxes[index].BackColor = Color.Yellow;
            }
            else
            {
                _boxes[index].BackColor = Color.White;
            }
        }

        private unsafe void BoxChanged(object sender, EventArgs e)
        {
            NumericInputBox box = sender as NumericInputBox;
            CameraAnimationFrame kf;
            float* pkf = (float*) &kf;
            float val = box.Value;
            int index = (int) box.Tag;
            int x;

            if (val != _currentFrame[index])
            {
                int kfIndex = FindKeyframe(_currentPage);

                if (float.IsNaN(val))
                {
                    //Value removed find keyframe and zero it out
                    if (kfIndex >= 0)
                    {
                        kf = (CameraAnimationFrame) listKeyframes.Items[kfIndex];
                        kf.SetBools(index, false);
                        pkf[index] = val;
                        for (x = 0; x < 15 && float.IsNaN(pkf[x]); x++)
                        {
                            ;
                        }

                        if (x == 15)
                        {
                            listKeyframes.Items.RemoveAt(kfIndex);
                            listKeyframes.SelectedIndex = -1;
                        }
                        else
                        {
                            listKeyframes.Items[kfIndex] = kf;
                        }
                    }

                    _target.RemoveKeyframe((CameraKeyframeMode) index, _currentPage);
                    val = _target.GetAnimFrame(_currentPage)[index];
                    box.Value = val;
                }
                else
                {
                    if (kfIndex >= 0)
                    {
                        kf = (CameraAnimationFrame) listKeyframes.Items[kfIndex];
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
                        for (x = 0;
                            x < count && ((CameraAnimationFrame) listKeyframes.Items[x]).Index < _currentPage;
                            x++)
                        {
                            ;
                        }

                        listKeyframes.Items.Insert(x, kf);
                        listKeyframes.SelectedIndex = x;
                    }

                    _target.SetKeyframe((CameraKeyframeMode) index, _currentPage, val);
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
                CameraAnimationFrame f = (CameraAnimationFrame) listKeyframes.SelectedItem;
                numFrame.Value = (decimal) (f.Index + 1);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            numFrame.Value--;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            numFrame.Value++;
        }

        #region Designer

        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            listKeyframes = new ListBox();
            panel2 = new Panel();
            numFarZ = new NumericInputBox();
            numNearZ = new NumericInputBox();
            label12 = new Label();
            label13 = new Label();
            numAspect = new NumericInputBox();
            numHeight = new NumericInputBox();
            label10 = new Label();
            label11 = new Label();
            numAimY = new NumericInputBox();
            numAimZ = new NumericInputBox();
            label7 = new Label();
            label6 = new Label();
            numFovY = new NumericInputBox();
            label3 = new Label();
            label2 = new Label();
            btnNext = new Button();
            label5 = new Label();
            label4 = new Label();
            btnPrev = new Button();
            numPosY = new NumericInputBox();
            numRotZ = new NumericInputBox();
            lblFrameCount = new Label();
            numPosX = new NumericInputBox();
            numTwist = new NumericInputBox();
            numRotX = new NumericInputBox();
            numFrame = new NumericUpDown();
            numRotY = new NumericInputBox();
            numAimX = new NumericInputBox();
            numPosZ = new NumericInputBox();
            label9 = new Label();
            label1 = new Label();
            label8 = new Label();
            groupBox1.SuspendLayout();
            panel2.SuspendLayout();
            ((ISupportInitialize) numFrame).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(listKeyframes);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(286, 109);
            groupBox1.TabIndex = 24;
            groupBox1.TabStop = false;
            groupBox1.Text = "Keyframes";
            // 
            // listKeyframes
            // 
            listKeyframes.Dock = DockStyle.Fill;
            listKeyframes.Font = new Font("Courier New", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listKeyframes.FormattingEnabled = true;
            listKeyframes.HorizontalScrollbar = true;
            listKeyframes.IntegralHeight = false;
            listKeyframes.ItemHeight = 14;
            listKeyframes.Location = new Point(3, 16);
            listKeyframes.Name = "listKeyframes";
            listKeyframes.Size = new Size(280, 90);
            listKeyframes.TabIndex = 18;
            listKeyframes.SelectedIndexChanged += new EventHandler(listKeyframes_SelectedIndexChanged);
            // 
            // panel2
            // 
            panel2.Controls.Add(numFarZ);
            panel2.Controls.Add(numNearZ);
            panel2.Controls.Add(label12);
            panel2.Controls.Add(label13);
            panel2.Controls.Add(numAspect);
            panel2.Controls.Add(numHeight);
            panel2.Controls.Add(label10);
            panel2.Controls.Add(label11);
            panel2.Controls.Add(numAimY);
            panel2.Controls.Add(numAimZ);
            panel2.Controls.Add(label7);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(numFovY);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(btnNext);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(btnPrev);
            panel2.Controls.Add(numPosY);
            panel2.Controls.Add(numRotZ);
            panel2.Controls.Add(lblFrameCount);
            panel2.Controls.Add(numPosX);
            panel2.Controls.Add(numTwist);
            panel2.Controls.Add(numRotX);
            panel2.Controls.Add(numFrame);
            panel2.Controls.Add(numRotY);
            panel2.Controls.Add(numAimX);
            panel2.Controls.Add(numPosZ);
            panel2.Controls.Add(label9);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(label8);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 109);
            panel2.Name = "panel2";
            panel2.Size = new Size(286, 167);
            panel2.TabIndex = 25;
            // 
            // numFarZ
            // 
            numFarZ.BorderStyle = BorderStyle.FixedSingle;
            numFarZ.Location = new Point(211, 141);
            numFarZ.Margin = new Padding(0, 10, 0, 10);
            numFarZ.Name = "numFarZ";
            numFarZ.Size = new Size(70, 20);
            numFarZ.TabIndex = 29;
            numFarZ.Text = "0";
            numFarZ.ValueChanged += new EventHandler(BoxChanged);
            // 
            // numNearZ
            // 
            numNearZ.BorderStyle = BorderStyle.FixedSingle;
            numNearZ.Location = new Point(73, 141);
            numNearZ.Margin = new Padding(0, 10, 0, 10);
            numNearZ.Name = "numNearZ";
            numNearZ.Size = new Size(70, 20);
            numNearZ.TabIndex = 28;
            numNearZ.Text = "0";
            numNearZ.ValueChanged += new EventHandler(BoxChanged);
            // 
            // label12
            // 
            label12.BorderStyle = BorderStyle.FixedSingle;
            label12.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label12.Location = new Point(142, 141);
            label12.Margin = new Padding(0);
            label12.Name = "label12";
            label12.Size = new Size(70, 20);
            label12.TabIndex = 27;
            label12.Text = "Far Z";
            label12.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            label13.BorderStyle = BorderStyle.FixedSingle;
            label13.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label13.Location = new Point(4, 141);
            label13.Margin = new Padding(0);
            label13.Name = "label13";
            label13.Size = new Size(70, 20);
            label13.TabIndex = 26;
            label13.Text = "Near Z";
            label13.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numAspect
            // 
            numAspect.BorderStyle = BorderStyle.FixedSingle;
            numAspect.Location = new Point(211, 122);
            numAspect.Margin = new Padding(0, 10, 0, 10);
            numAspect.Name = "numAspect";
            numAspect.Size = new Size(70, 20);
            numAspect.TabIndex = 25;
            numAspect.Text = "0";
            numAspect.ValueChanged += new EventHandler(BoxChanged);
            // 
            // numHeight
            // 
            numHeight.BorderStyle = BorderStyle.FixedSingle;
            numHeight.Location = new Point(73, 122);
            numHeight.Margin = new Padding(0, 10, 0, 10);
            numHeight.Name = "numHeight";
            numHeight.Size = new Size(70, 20);
            numHeight.TabIndex = 24;
            numHeight.Text = "0";
            numHeight.ValueChanged += new EventHandler(BoxChanged);
            // 
            // label10
            // 
            label10.BorderStyle = BorderStyle.FixedSingle;
            label10.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.Location = new Point(142, 122);
            label10.Margin = new Padding(0);
            label10.Name = "label10";
            label10.Size = new Size(70, 20);
            label10.TabIndex = 23;
            label10.Text = "Aspect";
            label10.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            label11.BorderStyle = BorderStyle.FixedSingle;
            label11.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.Location = new Point(4, 122);
            label11.Margin = new Padding(0);
            label11.Name = "label11";
            label11.Size = new Size(70, 20);
            label11.TabIndex = 22;
            label11.Text = "Height";
            label11.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numAimY
            // 
            numAimY.BorderStyle = BorderStyle.FixedSingle;
            numAimY.Location = new Point(142, 84);
            numAimY.Margin = new Padding(0, 10, 0, 10);
            numAimY.Name = "numAimY";
            numAimY.Size = new Size(70, 20);
            numAimY.TabIndex = 21;
            numAimY.Text = "0";
            numAimY.ValueChanged += new EventHandler(BoxChanged);
            // 
            // numAimZ
            // 
            numAimZ.BorderStyle = BorderStyle.FixedSingle;
            numAimZ.Location = new Point(211, 84);
            numAimZ.Margin = new Padding(0, 10, 0, 10);
            numAimZ.Name = "numAimZ";
            numAimZ.Size = new Size(70, 20);
            numAimZ.TabIndex = 19;
            numAimZ.Text = "0";
            numAimZ.ValueChanged += new EventHandler(BoxChanged);
            // 
            // label7
            // 
            label7.Location = new Point(70, 4);
            label7.Name = "label7";
            label7.Size = new Size(41, 20);
            label7.TabIndex = 15;
            label7.Text = "Frame:";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            label6.BorderStyle = BorderStyle.FixedSingle;
            label6.Location = new Point(211, 27);
            label6.Margin = new Padding(0);
            label6.Name = "label6";
            label6.Size = new Size(70, 20);
            label6.TabIndex = 8;
            label6.Text = "Z";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // numFovY
            // 
            numFovY.BorderStyle = BorderStyle.FixedSingle;
            numFovY.Location = new Point(211, 103);
            numFovY.Margin = new Padding(0, 10, 0, 10);
            numFovY.Name = "numFovY";
            numFovY.Size = new Size(70, 20);
            numFovY.TabIndex = 13;
            numFovY.Text = "0";
            numFovY.ValueChanged += new EventHandler(BoxChanged);
            // 
            // label3
            // 
            label3.BorderStyle = BorderStyle.FixedSingle;
            label3.Location = new Point(4, 65);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(70, 20);
            label3.TabIndex = 2;
            label3.Text = "Rotation";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Location = new Point(4, 84);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 1;
            label2.Text = "Aim";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnNext
            // 
            btnNext.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnNext.Location = new Point(241, 2);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(23, 23);
            btnNext.TabIndex = 2;
            btnNext.Text = ">";
            btnNext.TextAlign = ContentAlignment.TopCenter;
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += new EventHandler(btnNext_Click);
            // 
            // label5
            // 
            label5.BorderStyle = BorderStyle.FixedSingle;
            label5.Location = new Point(142, 27);
            label5.Margin = new Padding(0);
            label5.Name = "label5";
            label5.Size = new Size(70, 20);
            label5.TabIndex = 7;
            label5.Text = "Y";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.BorderStyle = BorderStyle.FixedSingle;
            label4.Location = new Point(73, 27);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(70, 20);
            label4.TabIndex = 4;
            label4.Text = "X";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnPrev
            // 
            btnPrev.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPrev.Location = new Point(216, 2);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(23, 23);
            btnPrev.TabIndex = 1;
            btnPrev.Text = "<";
            btnPrev.TextAlign = ContentAlignment.TopCenter;
            btnPrev.UseVisualStyleBackColor = true;
            btnPrev.Click += new EventHandler(btnPrev_Click);
            // 
            // numPosY
            // 
            numPosY.BorderStyle = BorderStyle.FixedSingle;
            numPosY.Location = new Point(142, 46);
            numPosY.Margin = new Padding(0);
            numPosY.Name = "numPosY";
            numPosY.Size = new Size(70, 20);
            numPosY.TabIndex = 4;
            numPosY.Text = "0";
            numPosY.ValueChanged += new EventHandler(BoxChanged);
            // 
            // numRotZ
            // 
            numRotZ.BorderStyle = BorderStyle.FixedSingle;
            numRotZ.Location = new Point(211, 65);
            numRotZ.Margin = new Padding(0, 10, 0, 10);
            numRotZ.Name = "numRotZ";
            numRotZ.Size = new Size(70, 20);
            numRotZ.TabIndex = 8;
            numRotZ.Text = "0";
            numRotZ.ValueChanged += new EventHandler(BoxChanged);
            // 
            // lblFrameCount
            // 
            lblFrameCount.Location = new Point(181, 4);
            lblFrameCount.Name = "lblFrameCount";
            lblFrameCount.Size = new Size(29, 20);
            lblFrameCount.TabIndex = 17;
            lblFrameCount.Text = "/ 10";
            lblFrameCount.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numPosX
            // 
            numPosX.BorderStyle = BorderStyle.FixedSingle;
            numPosX.Location = new Point(73, 46);
            numPosX.Margin = new Padding(0);
            numPosX.Name = "numPosX";
            numPosX.Size = new Size(70, 20);
            numPosX.TabIndex = 3;
            numPosX.Text = "0";
            numPosX.ValueChanged += new EventHandler(BoxChanged);
            // 
            // numTwist
            // 
            numTwist.BorderStyle = BorderStyle.FixedSingle;
            numTwist.Location = new Point(73, 103);
            numTwist.Margin = new Padding(0, 10, 0, 10);
            numTwist.Name = "numTwist";
            numTwist.Size = new Size(70, 20);
            numTwist.TabIndex = 12;
            numTwist.Text = "0";
            numTwist.ValueChanged += new EventHandler(BoxChanged);
            // 
            // numRotX
            // 
            numRotX.BorderStyle = BorderStyle.FixedSingle;
            numRotX.Location = new Point(73, 65);
            numRotX.Margin = new Padding(0, 10, 0, 10);
            numRotX.Name = "numRotX";
            numRotX.Size = new Size(70, 20);
            numRotX.TabIndex = 6;
            numRotX.Text = "0";
            numRotX.ValueChanged += new EventHandler(BoxChanged);
            // 
            // numFrame
            // 
            numFrame.Location = new Point(117, 4);
            numFrame.Minimum = new decimal(new int[]
            {
                1,
                0,
                0,
                0
            });
            numFrame.Name = "numFrame";
            numFrame.Size = new Size(58, 20);
            numFrame.TabIndex = 0;
            numFrame.Value = new decimal(new int[]
            {
                1,
                0,
                0,
                0
            });
            numFrame.ValueChanged += new EventHandler(numFrame_ValueChanged);
            // 
            // numRotY
            // 
            numRotY.BorderStyle = BorderStyle.FixedSingle;
            numRotY.Location = new Point(142, 65);
            numRotY.Margin = new Padding(0, 10, 0, 10);
            numRotY.Name = "numRotY";
            numRotY.Size = new Size(70, 20);
            numRotY.TabIndex = 7;
            numRotY.Text = "0";
            numRotY.ValueChanged += new EventHandler(BoxChanged);
            // 
            // numAimX
            // 
            numAimX.BorderStyle = BorderStyle.FixedSingle;
            numAimX.Location = new Point(73, 84);
            numAimX.Margin = new Padding(0, 10, 0, 10);
            numAimX.Name = "numAimX";
            numAimX.Size = new Size(70, 20);
            numAimX.TabIndex = 11;
            numAimX.Text = "0";
            numAimX.ValueChanged += new EventHandler(BoxChanged);
            // 
            // numPosZ
            // 
            numPosZ.BorderStyle = BorderStyle.FixedSingle;
            numPosZ.Location = new Point(211, 46);
            numPosZ.Margin = new Padding(0);
            numPosZ.Name = "numPosZ";
            numPosZ.Size = new Size(70, 20);
            numPosZ.TabIndex = 5;
            numPosZ.Text = "0";
            numPosZ.ValueChanged += new EventHandler(BoxChanged);
            // 
            // label9
            // 
            label9.BorderStyle = BorderStyle.FixedSingle;
            label9.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.Location = new Point(142, 103);
            label9.Margin = new Padding(0);
            label9.Name = "label9";
            label9.Size = new Size(70, 20);
            label9.TabIndex = 10;
            label9.Text = "Fov Y";
            label9.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Location = new Point(4, 46);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(70, 20);
            label1.TabIndex = 0;
            label1.Text = "Position";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            label8.BorderStyle = BorderStyle.FixedSingle;
            label8.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(4, 103);
            label8.Margin = new Padding(0);
            label8.Name = "label8";
            label8.Size = new Size(70, 20);
            label8.TabIndex = 9;
            label8.Text = "Twist";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // SCN0CameraEditControl
            // 
            Controls.Add(groupBox1);
            Controls.Add(panel2);
            Name = "SCN0CameraEditControl";
            Size = new Size(286, 276);
            groupBox1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((ISupportInitialize) numFrame).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}