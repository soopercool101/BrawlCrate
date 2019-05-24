using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class AnimEditControl : UserControl
    {
        private int _numFrames;

        private int _currentPage = 1;
        private CHRAnimationFrame _currentFrame = CHRAnimationFrame.Identity;
        private readonly NumericInputBox[] _boxes = new NumericInputBox[9];
        private Panel panel1;

        private CHR0EntryNode _target;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CHR0EntryNode TargetSequence
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

        public AnimEditControl()
        {
            InitializeComponent();
            _boxes[0] = numScaleX;
            _boxes[1] = numScaleY;
            _boxes[2] = numScaleZ;
            _boxes[3] = numRotX;
            _boxes[4] = numRotY;
            _boxes[5] = numRotZ;
            _boxes[6] = numTransX;
            _boxes[7] = numTransY;
            _boxes[8] = numTransZ;

            for (int i = 0; i < 9; i++)
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
                    CHRAnimationFrame ain, aout;
                    for (int x = 0; x < _target.FrameCount; x++)
                    {
                        if ((ain = _target.GetAnimFrame(x)).HasKeys)
                        {
                            listKeyframes.Items.Add(ain);
                            aout = _target.GetAnimFrame(x, true);
                            if (!ain.Equals(aout))
                            {
                                listKeyframes.Items.Add(aout);
                            }
                        }
                    }

                    _numFrames = _target.FrameCount;

                    _currentPage = 0;
                    numFrame.Value = 1;
                    numFrame.Maximum = _numFrames;
                    lblFrameCount.Text = string.Format("/ {0}", _numFrames);
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

                numScaleX.Value = _currentFrame.Scale._x;
                numScaleY.Value = _currentFrame.Scale._y;
                numScaleZ.Value = _currentFrame.Scale._z;

                numRotX.Value = _currentFrame.Rotation._x;
                numRotY.Value = _currentFrame.Rotation._y;
                numRotZ.Value = _currentFrame.Rotation._z;

                numTransX.Value = _currentFrame.Translation._x;
                numTransY.Value = _currentFrame.Translation._y;
                numTransZ.Value = _currentFrame.Translation._z;

                for (int i = 0; i < 9; i++)
                {
                    UpdateBox(i);
                }

                btnPrev.Enabled = _currentPage > 0;
                btnNext.Enabled = _currentPage < (_numFrames - 1);

                listKeyframes.SelectedIndex = FindKeyframe(_currentPage);
            }
        }

        private int FindKeyframe(int index)
        {
            int count = listKeyframes.Items.Count;
            for (int i = 0; i < count; i++)
            {
                if (((CHRAnimationFrame)listKeyframes.Items[i]).Index == index)
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
            CHRAnimationFrame kf;
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
                        kf = (CHRAnimationFrame)listKeyframes.Items[kfIndex];
                        kf.SetBool(index, false);
                        pkf[index] = val;
                        for (x = 0; (x < 9) && float.IsNaN(pkf[x]); x++)
                        {
                            ;
                        }

                        if (x == 9)
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
                        kf = (CHRAnimationFrame)listKeyframes.Items[kfIndex];
                        kf.SetBool(index, true);
                        pkf[index] = val;
                        listKeyframes.Items[kfIndex] = kf;
                    }
                    else
                    {
                        kf = CHRAnimationFrame.Empty;
                        kf.SetBool(index, true);
                        kf.Index = _currentPage;
                        pkf[index] = val;

                        int count = listKeyframes.Items.Count;
                        for (x = 0; (x < count) && (((CHRAnimationFrame)listKeyframes.Items[x]).Index < _currentPage); x++)
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
                CHRAnimationFrame f = (CHRAnimationFrame)listKeyframes.SelectedItem;
                numFrame.Value = f.Index + 1;
            }
        }

        private void btnPrev_Click(object sender, EventArgs e) { numFrame.Value--; }
        private void btnNext_Click(object sender, EventArgs e) { numFrame.Value++; }

        #region Designer

        private Label label1;
        private Label label2;
        private Label label3;
        private NumericInputBox numScaleX;
        private Label label4;
        private NumericInputBox numRotX;
        private NumericInputBox numTransX;
        private Label label5;
        private Label label6;
        private NumericInputBox numScaleY;
        private NumericInputBox numTransY;
        private NumericInputBox numRotY;
        private NumericInputBox numScaleZ;
        private NumericInputBox numTransZ;
        private Label label7;
        private NumericUpDown numFrame;
        private Label lblFrameCount;
        private Button btnPrev;
        private Button btnNext;
        private NumericInputBox numRotZ;
        private ListBox listKeyframes;
        private GroupBox groupBox1;

        private void InitializeComponent()
        {
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            numScaleX = new System.Windows.Forms.NumericInputBox();
            label4 = new System.Windows.Forms.Label();
            numRotX = new System.Windows.Forms.NumericInputBox();
            numTransX = new System.Windows.Forms.NumericInputBox();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            numScaleY = new System.Windows.Forms.NumericInputBox();
            numTransY = new System.Windows.Forms.NumericInputBox();
            numRotY = new System.Windows.Forms.NumericInputBox();
            numScaleZ = new System.Windows.Forms.NumericInputBox();
            numTransZ = new System.Windows.Forms.NumericInputBox();
            numRotZ = new System.Windows.Forms.NumericInputBox();
            label7 = new System.Windows.Forms.Label();
            numFrame = new System.Windows.Forms.NumericUpDown();
            lblFrameCount = new System.Windows.Forms.Label();
            btnPrev = new System.Windows.Forms.Button();
            btnNext = new System.Windows.Forms.Button();
            listKeyframes = new System.Windows.Forms.ListBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(numFrame)).BeginInit();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label1.Location = new System.Drawing.Point(10, 51);
            label1.Margin = new System.Windows.Forms.Padding(0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(70, 20);
            label1.TabIndex = 0;
            label1.Text = "Scale";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label2.Location = new System.Drawing.Point(10, 89);
            label2.Margin = new System.Windows.Forms.Padding(0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(70, 20);
            label2.TabIndex = 1;
            label2.Text = "Translation";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label3.Location = new System.Drawing.Point(10, 70);
            label3.Margin = new System.Windows.Forms.Padding(0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(70, 20);
            label3.TabIndex = 2;
            label3.Text = "Rotation";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numScaleX
            // 
            numScaleX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numScaleX.Location = new System.Drawing.Point(79, 51);
            numScaleX.Margin = new System.Windows.Forms.Padding(0);
            numScaleX.Name = "numScaleX";
            numScaleX.Size = new System.Drawing.Size(70, 20);
            numScaleX.TabIndex = 3;
            numScaleX.Text = "0";
            numScaleX.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // label4
            // 
            label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label4.Location = new System.Drawing.Point(79, 32);
            label4.Margin = new System.Windows.Forms.Padding(0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(70, 20);
            label4.TabIndex = 4;
            label4.Text = "X";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numRotX
            // 
            numRotX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numRotX.Location = new System.Drawing.Point(79, 70);
            numRotX.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numRotX.Name = "numRotX";
            numRotX.Size = new System.Drawing.Size(70, 20);
            numRotX.TabIndex = 6;
            numRotX.Text = "0";
            numRotX.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // numTransX
            // 
            numTransX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numTransX.Location = new System.Drawing.Point(79, 89);
            numTransX.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numTransX.Name = "numTransX";
            numTransX.Size = new System.Drawing.Size(70, 20);
            numTransX.TabIndex = 9;
            numTransX.Text = "0";
            numTransX.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // label5
            // 
            label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label5.Location = new System.Drawing.Point(148, 32);
            label5.Margin = new System.Windows.Forms.Padding(0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(70, 20);
            label5.TabIndex = 7;
            label5.Text = "Y";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label6.Location = new System.Drawing.Point(217, 32);
            label6.Margin = new System.Windows.Forms.Padding(0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(70, 20);
            label6.TabIndex = 8;
            label6.Text = "Z";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numScaleY
            // 
            numScaleY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numScaleY.Location = new System.Drawing.Point(148, 51);
            numScaleY.Margin = new System.Windows.Forms.Padding(0);
            numScaleY.Name = "numScaleY";
            numScaleY.Size = new System.Drawing.Size(70, 20);
            numScaleY.TabIndex = 4;
            numScaleY.Text = "0";
            numScaleY.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // numTransY
            // 
            numTransY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numTransY.Location = new System.Drawing.Point(148, 89);
            numTransY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numTransY.Name = "numTransY";
            numTransY.Size = new System.Drawing.Size(70, 20);
            numTransY.TabIndex = 10;
            numTransY.Text = "0";
            numTransY.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // numRotY
            // 
            numRotY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numRotY.Location = new System.Drawing.Point(148, 70);
            numRotY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numRotY.Name = "numRotY";
            numRotY.Size = new System.Drawing.Size(70, 20);
            numRotY.TabIndex = 7;
            numRotY.Text = "0";
            numRotY.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // numScaleZ
            // 
            numScaleZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numScaleZ.Location = new System.Drawing.Point(217, 51);
            numScaleZ.Margin = new System.Windows.Forms.Padding(0);
            numScaleZ.Name = "numScaleZ";
            numScaleZ.Size = new System.Drawing.Size(70, 20);
            numScaleZ.TabIndex = 5;
            numScaleZ.Text = "0";
            numScaleZ.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // numTransZ
            // 
            numTransZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numTransZ.Location = new System.Drawing.Point(217, 89);
            numTransZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numTransZ.Name = "numTransZ";
            numTransZ.Size = new System.Drawing.Size(70, 20);
            numTransZ.TabIndex = 11;
            numTransZ.Text = "0";
            numTransZ.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // numRotZ
            // 
            numRotZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numRotZ.Location = new System.Drawing.Point(217, 70);
            numRotZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numRotZ.Name = "numRotZ";
            numRotZ.Size = new System.Drawing.Size(70, 20);
            numRotZ.TabIndex = 8;
            numRotZ.Text = "0";
            numRotZ.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // label7
            // 
            label7.Location = new System.Drawing.Point(37, 5);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(61, 20);
            label7.TabIndex = 15;
            label7.Text = "Frame:";
            label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numFrame
            // 
            numFrame.Location = new System.Drawing.Point(104, 5);
            numFrame.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            numFrame.Name = "numFrame";
            numFrame.Size = new System.Drawing.Size(58, 20);
            numFrame.TabIndex = 0;
            numFrame.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            numFrame.ValueChanged += new System.EventHandler(numFrame_ValueChanged);
            // 
            // lblFrameCount
            // 
            lblFrameCount.Location = new System.Drawing.Point(168, 5);
            lblFrameCount.Name = "lblFrameCount";
            lblFrameCount.Size = new System.Drawing.Size(45, 20);
            lblFrameCount.TabIndex = 17;
            lblFrameCount.Text = "/ 10";
            lblFrameCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPrev
            // 
            btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnPrev.Location = new System.Drawing.Point(219, 4);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new System.Drawing.Size(23, 23);
            btnPrev.TabIndex = 1;
            btnPrev.Text = "<";
            btnPrev.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            btnPrev.UseVisualStyleBackColor = true;
            btnPrev.Click += new System.EventHandler(btnPrev_Click);
            // 
            // btnNext
            // 
            btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnNext.Location = new System.Drawing.Point(244, 4);
            btnNext.Name = "btnNext";
            btnNext.Size = new System.Drawing.Size(23, 23);
            btnNext.TabIndex = 2;
            btnNext.Text = ">";
            btnNext.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += new System.EventHandler(btnNext_Click);
            // 
            // listKeyframes
            // 
            listKeyframes.Dock = System.Windows.Forms.DockStyle.Fill;
            listKeyframes.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            listKeyframes.FormattingEnabled = true;
            listKeyframes.IntegralHeight = false;
            listKeyframes.ItemHeight = 14;
            listKeyframes.Location = new System.Drawing.Point(3, 16);
            listKeyframes.Name = "listKeyframes";
            listKeyframes.Size = new System.Drawing.Size(294, 76);
            listKeyframes.TabIndex = 18;
            listKeyframes.SelectedIndexChanged += new System.EventHandler(listKeyframes_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(listKeyframes);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(300, 95);
            groupBox1.TabIndex = 21;
            groupBox1.TabStop = false;
            groupBox1.Text = "Keyframes";
            // 
            // panel1
            // 
            panel1.Controls.Add(label7);
            panel1.Controls.Add(btnNext);
            panel1.Controls.Add(numRotY);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(numTransY);
            panel1.Controls.Add(btnPrev);
            panel1.Controls.Add(numScaleX);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(numScaleY);
            panel1.Controls.Add(lblFrameCount);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(numRotZ);
            panel1.Controls.Add(numFrame);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(numTransZ);
            panel1.Controls.Add(numRotX);
            panel1.Controls.Add(numTransX);
            panel1.Controls.Add(numScaleZ);
            panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel1.Location = new System.Drawing.Point(0, 95);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(300, 117);
            panel1.TabIndex = 23;
            // 
            // AnimEditControl
            // 
            Controls.Add(groupBox1);
            Controls.Add(panel1);
            Name = "AnimEditControl";
            Size = new System.Drawing.Size(300, 212);
            ((System.ComponentModel.ISupportInitialize)(numFrame)).EndInit();
            groupBox1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion
    }
}
