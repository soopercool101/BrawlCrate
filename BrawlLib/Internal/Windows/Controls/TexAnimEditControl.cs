using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public class TexAnimEditControl : UserControl
    {
        private int _numFrames;

        private int _currentPage = 1;
        private SRTAnimationFrame _currentFrame = SRTAnimationFrame.Identity;
        private readonly NumericInputBox[] _boxes = new NumericInputBox[5];
        private Panel panel1;
        private Label label6;
        private Label label5;
        private Label label9;
        private Label label8;

        private SRT0TextureNode _target;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SRT0TextureNode TargetSequence
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

        public TexAnimEditControl()
        {
            InitializeComponent();
            _boxes[0] = numScaleX;
            _boxes[1] = numScaleY;
            _boxes[2] = numRot;
            _boxes[3] = numTransX;
            _boxes[4] = numTransY;

            for (int i = 0; i < 5; i++)
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
                    SRTAnimationFrame a = new SRTAnimationFrame();
                    bool check = false;
                    for (int x = 0; x < _target.FrameCount; x++)
                    {
                        a = _target.GetAnimFrame(x);
                        a.Index = x;
                        for (int i = 0; i < 5; i++)
                        {
                            if (_target.GetKeyframe(i, x) != null)
                            {
                                check = true;
                                a.SetBool(i, true);
                            }
                        }

                        if (check)
                        {
                            listKeyframes.Items.Add(a);
                            check = false;
                        }
                    }
                    //foreach (AnimationKeyframe f in _target.Keyframes.Keyframes)
                    //    listKeyframes.Items.Add(f);

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
                //_currentPage = (int)numFrame.Value - 1;

                _currentFrame = _target.GetAnimFrame(_currentPage);

                numScaleX.Value = _currentFrame.Scale._x;
                numScaleY.Value = _currentFrame.Scale._y;
                numRot.Value = _currentFrame.Rotation;
                numTransX.Value = _currentFrame.Translation._x;
                numTransY.Value = _currentFrame.Translation._y;

                for (int i = 0; i < 5; i++)
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
                if (((SRTAnimationFrame) listKeyframes.Items[i]).Index == index)
                {
                    return i;
                }
            }

            return -1;
        }

        private void UpdateBox(int index)
        {
            if (_target.GetKeyframe(index, _currentPage) != null)
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
            SRTAnimationFrame kf;
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
                        kf = (SRTAnimationFrame) listKeyframes.Items[kfIndex];
                        kf.SetBool(index, false);
                        pkf[index] = val;
                        for (x = 0; x < 5 && float.IsNaN(pkf[x]); x++)
                        {
                            ;
                        }

                        if (x == 5)
                        {
                            listKeyframes.Items.RemoveAt(kfIndex);
                            listKeyframes.SelectedIndex = -1;
                        }
                        else
                        {
                            listKeyframes.Items[kfIndex] = kf;
                        }
                    }

                    _target.RemoveKeyframe(index, _currentPage);
                    val = _target.GetAnimFrame(_currentPage)[index];
                    box.Value = val;
                }
                else
                {
                    if (kfIndex >= 0)
                    {
                        kf = (SRTAnimationFrame) listKeyframes.Items[kfIndex];
                        kf.SetBool(index, true);
                        pkf[index] = val;
                        listKeyframes.Items[kfIndex] = kf;
                    }
                    else
                    {
                        kf = SRTAnimationFrame.Empty;
                        kf.Index = _currentPage;
                        kf.SetBool(index, true);
                        pkf[index] = val;

                        int count = listKeyframes.Items.Count;
                        for (x = 0; x < count && ((SRTAnimationFrame) listKeyframes.Items[x]).Index < _currentPage; x++)
                        {
                            ;
                        }

                        listKeyframes.Items.Insert(x, kf);
                        listKeyframes.SelectedIndex = x;
                    }

                    _target.SetKeyframe(index, _currentPage, val);
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
                SRTAnimationFrame f = (SRTAnimationFrame) listKeyframes.SelectedItem;
                numFrame.Value = f.Index + 1;
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            numFrame.Value -= 1;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            numFrame.Value += 1;
        }

        #region Designer

        private Label label1;
        private Label label2;
        private Label label3;
        private NumericInputBox numScaleY;
        private NumericInputBox numRot;
        private NumericInputBox numTransX;
        private NumericInputBox numTransY;
        private NumericInputBox numScaleX;
        private Label label7;
        private NumericUpDown numFrame;
        private Label lblFrameCount;
        private Button btnPrev;
        private Button btnNext;
        private ListBox listKeyframes;
        private GroupBox groupBox1;

        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            numScaleY = new NumericInputBox();
            numRot = new NumericInputBox();
            numTransX = new NumericInputBox();
            numTransY = new NumericInputBox();
            numScaleX = new NumericInputBox();
            label7 = new Label();
            numFrame = new NumericUpDown();
            lblFrameCount = new Label();
            btnPrev = new Button();
            btnNext = new Button();
            listKeyframes = new ListBox();
            groupBox1 = new GroupBox();
            panel1 = new Panel();
            label9 = new Label();
            label8 = new Label();
            label6 = new Label();
            label5 = new Label();
            ((ISupportInitialize) numFrame).BeginInit();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Location = new Point(10, 30);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(70, 20);
            label1.TabIndex = 0;
            label1.Text = "Scale";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Location = new Point(10, 49);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 1;
            label2.Text = "Translation";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.BorderStyle = BorderStyle.FixedSingle;
            label3.Location = new Point(10, 68);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(88, 20);
            label3.TabIndex = 2;
            label3.Text = "Rotation";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numScaleY
            // 
            numScaleY.BorderStyle = BorderStyle.FixedSingle;
            numScaleY.Location = new Point(184, 30);
            numScaleY.Margin = new Padding(0);
            numScaleY.Name = "numScaleY";
            numScaleY.Size = new Size(70, 20);
            numScaleY.TabIndex = 101;
            numScaleY.Text = "0";
            numScaleY.ValueChanged += new EventHandler(BoxChanged);
            // 
            // numRot
            // 
            numRot.BorderStyle = BorderStyle.FixedSingle;
            numRot.Location = new Point(97, 68);
            numRot.Margin = new Padding(0, 10, 0, 10);
            numRot.Name = "numRot";
            numRot.Size = new Size(70, 20);
            numRot.TabIndex = 104;
            numRot.Text = "0";
            numRot.ValueChanged += new EventHandler(BoxChanged);
            // 
            // numTransX
            // 
            numTransX.BorderStyle = BorderStyle.FixedSingle;
            numTransX.Location = new Point(97, 49);
            numTransX.Margin = new Padding(0, 10, 0, 10);
            numTransX.Name = "numTransX";
            numTransX.Size = new Size(70, 20);
            numTransX.TabIndex = 102;
            numTransX.Text = "0";
            numTransX.ValueChanged += new EventHandler(BoxChanged);
            // 
            // numTransY
            // 
            numTransY.BorderStyle = BorderStyle.FixedSingle;
            numTransY.Location = new Point(184, 49);
            numTransY.Margin = new Padding(0, 10, 0, 10);
            numTransY.Name = "numTransY";
            numTransY.Size = new Size(70, 20);
            numTransY.TabIndex = 103;
            numTransY.Text = "0";
            numTransY.ValueChanged += new EventHandler(BoxChanged);
            // 
            // numScaleX
            // 
            numScaleX.BorderStyle = BorderStyle.FixedSingle;
            numScaleX.Location = new Point(97, 30);
            numScaleX.Margin = new Padding(0, 10, 0, 10);
            numScaleX.Name = "numScaleX";
            numScaleX.Size = new Size(70, 20);
            numScaleX.TabIndex = 100;
            numScaleX.Text = "0";
            numScaleX.ValueChanged += new EventHandler(BoxChanged);
            // 
            // label7
            // 
            label7.Location = new Point(37, 5);
            label7.Name = "label7";
            label7.Size = new Size(61, 20);
            label7.TabIndex = 15;
            label7.Text = "Frame:";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numFrame
            // 
            numFrame.Location = new Point(104, 5);
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
            // lblFrameCount
            // 
            lblFrameCount.Location = new Point(168, 5);
            lblFrameCount.Name = "lblFrameCount";
            lblFrameCount.Size = new Size(45, 20);
            lblFrameCount.TabIndex = 17;
            lblFrameCount.Text = "/ 10";
            lblFrameCount.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnPrev
            // 
            btnPrev.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPrev.Location = new Point(219, 4);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(23, 23);
            btnPrev.TabIndex = 1;
            btnPrev.Text = "<";
            btnPrev.TextAlign = ContentAlignment.TopCenter;
            btnPrev.UseVisualStyleBackColor = true;
            btnPrev.Click += new EventHandler(btnPrev_Click);
            // 
            // btnNext
            // 
            btnNext.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnNext.Location = new Point(244, 4);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(23, 23);
            btnNext.TabIndex = 2;
            btnNext.Text = ">";
            btnNext.TextAlign = ContentAlignment.TopCenter;
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += new EventHandler(btnNext_Click);
            // 
            // listKeyframes
            // 
            listKeyframes.Dock = DockStyle.Fill;
            listKeyframes.Font = new Font("Courier New", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listKeyframes.FormattingEnabled = true;
            listKeyframes.IntegralHeight = false;
            listKeyframes.ItemHeight = 14;
            listKeyframes.Location = new Point(3, 16);
            listKeyframes.Name = "listKeyframes";
            listKeyframes.Size = new Size(294, 96);
            listKeyframes.TabIndex = 18;
            listKeyframes.SelectedIndexChanged += new EventHandler(listKeyframes_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(listKeyframes);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(300, 115);
            groupBox1.TabIndex = 21;
            groupBox1.TabStop = false;
            groupBox1.Text = "Keyframes";
            // 
            // panel1
            // 
            panel1.Controls.Add(label9);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(btnNext);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(numTransY);
            panel1.Controls.Add(btnPrev);
            panel1.Controls.Add(numScaleY);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(lblFrameCount);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(numFrame);
            panel1.Controls.Add(numScaleX);
            panel1.Controls.Add(numRot);
            panel1.Controls.Add(numTransX);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 115);
            panel1.Name = "panel1";
            panel1.Size = new Size(300, 97);
            panel1.TabIndex = 23;
            // 
            // label9
            // 
            label9.BorderStyle = BorderStyle.FixedSingle;
            label9.Location = new Point(166, 30);
            label9.Margin = new Padding(0);
            label9.Name = "label9";
            label9.Size = new Size(19, 20);
            label9.TabIndex = 22;
            label9.Text = "Y";
            label9.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            label8.BorderStyle = BorderStyle.FixedSingle;
            label8.Location = new Point(79, 30);
            label8.Margin = new Padding(0);
            label8.Name = "label8";
            label8.Size = new Size(19, 20);
            label8.TabIndex = 21;
            label8.Text = "X";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            label6.BorderStyle = BorderStyle.FixedSingle;
            label6.Location = new Point(166, 49);
            label6.Margin = new Padding(0);
            label6.Name = "label6";
            label6.Size = new Size(19, 20);
            label6.TabIndex = 20;
            label6.Text = "Y";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.BorderStyle = BorderStyle.FixedSingle;
            label5.Location = new Point(79, 49);
            label5.Margin = new Padding(0);
            label5.Name = "label5";
            label5.Size = new Size(19, 20);
            label5.TabIndex = 19;
            label5.Text = "X";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // TexAnimEditControl
            // 
            Controls.Add(groupBox1);
            Controls.Add(panel1);
            Name = "TexAnimEditControl";
            Size = new Size(300, 212);
            ((ISupportInitialize) numFrame).EndInit();
            groupBox1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}