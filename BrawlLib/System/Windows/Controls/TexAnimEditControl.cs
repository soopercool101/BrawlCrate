using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class TexAnimEditControl : UserControl
    {
        private int _numFrames;

        private int _currentPage = 1;
        private SRTAnimationFrame _currentFrame = SRTAnimationFrame.Identity;
        private NumericInputBox[] _boxes = new NumericInputBox[5];
        private Panel panel1;
        private Label label6;
        private Label label5;
        private Label label9;
        private Label label8;

        private SRT0TextureNode _target;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SRT0TextureNode TargetSequence
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

        public TexAnimEditControl() 
        {
            InitializeComponent();
            _boxes[0] = numScaleX;
            _boxes[1] = numScaleY;
            _boxes[2] = numRot;
            _boxes[3] = numTransX;
            _boxes[4] = numTransY;
            
            for (int i = 0; i < 5; i++)
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
                        if (check == true)
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
                //_currentPage = (int)numFrame.Value - 1;

                _currentFrame = _target.GetAnimFrame(_currentPage);

                numScaleX.Value = _currentFrame.Scale._x;
                numScaleY.Value = _currentFrame.Scale._y;
                numRot.Value = _currentFrame.Rotation;
                numTransX.Value = _currentFrame.Translation._x;
                numTransY.Value = _currentFrame.Translation._y;

                for (int i = 0; i < 5; i++)
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
                if (((SRTAnimationFrame)listKeyframes.Items[i]).Index == index)
                    return i;
            return -1;
        }

        private void UpdateBox(int index)
        {
            if (_target.GetKeyframe(index, _currentPage) != null)
                _boxes[index].BackColor = Color.Yellow;
            else
                _boxes[index].BackColor = Color.White;
        }

        private unsafe void BoxChanged(object sender, EventArgs e)
        {
            NumericInputBox box = sender as NumericInputBox;
            SRTAnimationFrame kf;
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
                        kf = (SRTAnimationFrame)listKeyframes.Items[kfIndex];
                        kf.SetBool(index, false);
                        pkf[index] = val;
                        for (x = 0; (x < 5) && float.IsNaN(pkf[x]); x++) ;
                        if (x == 5)
                        {
                            listKeyframes.Items.RemoveAt(kfIndex);
                            listKeyframes.SelectedIndex = -1;
                        }
                        else
                            listKeyframes.Items[kfIndex] = kf;
                    }

                    _target.RemoveKeyframe(index, _currentPage);
                    val = _target.GetAnimFrame(_currentPage)[index];
                    box.Value = val;
                }
                else
                {
                    if (kfIndex >= 0)
                    {
                        kf = (SRTAnimationFrame)listKeyframes.Items[kfIndex];
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
                        for (x = 0; (x < count) && (((SRTAnimationFrame)listKeyframes.Items[x]).Index < _currentPage); x++) ;

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
                SRTAnimationFrame f = (SRTAnimationFrame)listKeyframes.SelectedItem;
                numFrame.Value = (decimal)(f.Index + 1);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e) { numFrame.Value -= 1; }
        private void btnNext_Click(object sender, EventArgs e) { numFrame.Value += 1; }

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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numScaleY = new System.Windows.Forms.NumericInputBox();
            this.numRot = new System.Windows.Forms.NumericInputBox();
            this.numTransX = new System.Windows.Forms.NumericInputBox();
            this.numTransY = new System.Windows.Forms.NumericInputBox();
            this.numScaleX = new System.Windows.Forms.NumericInputBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numFrame = new System.Windows.Forms.NumericUpDown();
            this.lblFrameCount = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.listKeyframes = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(10, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Scale";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(10, 49);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Translation";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(10, 68);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Rotation";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numScaleY
            // 
            this.numScaleY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numScaleY.Location = new System.Drawing.Point(184, 30);
            this.numScaleY.Margin = new System.Windows.Forms.Padding(0);
            this.numScaleY.Name = "numScaleY";
            this.numScaleY.Size = new System.Drawing.Size(70, 20);
            this.numScaleY.TabIndex = 3;
            this.numScaleY.Text = "0";
            this.numScaleY.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numRot
            // 
            this.numRot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRot.Location = new System.Drawing.Point(97, 68);
            this.numRot.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numRot.Name = "numRot";
            this.numRot.Size = new System.Drawing.Size(70, 20);
            this.numRot.TabIndex = 6;
            this.numRot.Text = "0";
            this.numRot.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numTransX
            // 
            this.numTransX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTransX.Location = new System.Drawing.Point(97, 49);
            this.numTransX.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numTransX.Name = "numTransX";
            this.numTransX.Size = new System.Drawing.Size(70, 20);
            this.numTransX.TabIndex = 9;
            this.numTransX.Text = "0";
            this.numTransX.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numTransY
            // 
            this.numTransY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTransY.Location = new System.Drawing.Point(184, 49);
            this.numTransY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numTransY.Name = "numTransY";
            this.numTransY.Size = new System.Drawing.Size(70, 20);
            this.numTransY.TabIndex = 10;
            this.numTransY.Text = "0";
            this.numTransY.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numScaleX
            // 
            this.numScaleX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numScaleX.Location = new System.Drawing.Point(97, 30);
            this.numScaleX.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numScaleX.Name = "numScaleX";
            this.numScaleX.Size = new System.Drawing.Size(70, 20);
            this.numScaleX.TabIndex = 11;
            this.numScaleX.Text = "0";
            this.numScaleX.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(37, 5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "Frame:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numFrame
            // 
            this.numFrame.Location = new System.Drawing.Point(104, 5);
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
            this.lblFrameCount.Location = new System.Drawing.Point(168, 5);
            this.lblFrameCount.Name = "lblFrameCount";
            this.lblFrameCount.Size = new System.Drawing.Size(45, 20);
            this.lblFrameCount.TabIndex = 17;
            this.lblFrameCount.Text = "/ 10";
            this.lblFrameCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPrev
            // 
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.Location = new System.Drawing.Point(219, 4);
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
            this.btnNext.Location = new System.Drawing.Point(244, 4);
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
            this.listKeyframes.Size = new System.Drawing.Size(294, 96);
            this.listKeyframes.TabIndex = 18;
            this.listKeyframes.SelectedIndexChanged += new System.EventHandler(this.listKeyframes_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listKeyframes);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 115);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Keyframes";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.numTransY);
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Controls.Add(this.numScaleY);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lblFrameCount);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.numFrame);
            this.panel1.Controls.Add(this.numScaleX);
            this.panel1.Controls.Add(this.numRot);
            this.panel1.Controls.Add(this.numTransX);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 115);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 97);
            this.panel1.TabIndex = 23;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(166, 30);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 20);
            this.label9.TabIndex = 22;
            this.label9.Text = "Y";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(79, 30);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 20);
            this.label8.TabIndex = 21;
            this.label8.Text = "X";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(166, 49);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 20);
            this.label6.TabIndex = 20;
            this.label6.Text = "Y";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(79, 49);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 20);
            this.label5.TabIndex = 19;
            this.label5.Text = "X";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TexAnimEditControl
            // 
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "TexAnimEditControl";
            this.Size = new System.Drawing.Size(300, 212);
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
