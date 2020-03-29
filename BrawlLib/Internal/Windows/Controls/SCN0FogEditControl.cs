using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public class SCN0FogEditControl : UserControl
    {
        private int _numFrames;

        private int _currentPage = 1;
        private FogAnimationFrame _currentFrame = FogAnimationFrame.Empty;
        private readonly NumericInputBox[] _boxes = new NumericInputBox[2];
        private Panel panel1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private GroupBox groupBox1;
        private ListBox listKeyframes;
        private Panel panel2;
        private Label label7;
        private Label label3;
        private Button btnNext;
        private Button btnPrev;
        private Label lblFrameCount;
        private NumericInputBox numStart;
        private NumericInputBox numEnd;
        private NumericUpDown numFrame;
        private Label label1;
        private TabPage tabPage3;
        private CLRControl lightCtrl;

        private SCN0FogNode _target;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SCN0FogNode TargetSequence
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

        public SCN0FogEditControl()
        {
            InitializeComponent();
            _boxes[0] = numStart;
            _boxes[1] = numEnd;

            for (int i = 0; i < 2; i++)
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
                    FogAnimationFrame a;
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
            lightCtrl.ColorSource = TargetSequence;
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

                numStart.Value = _currentFrame.Start;
                numEnd.Value = _currentFrame.End;

                for (int i = 0; i < 2; i++)
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
                if (((FogAnimationFrame) listKeyframes.Items[i]).Index == index)
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
            FogAnimationFrame kf;
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
                        kf = (FogAnimationFrame) listKeyframes.Items[kfIndex];
                        kf.SetBools(index, false);
                        pkf[index] = val;
                        for (x = 0; x < 2 && float.IsNaN(pkf[x]); x++)
                        {
                            ;
                        }

                        if (x == 2)
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
                        kf = (FogAnimationFrame) listKeyframes.Items[kfIndex];
                        kf.SetBools(index, true);
                        pkf[index] = val;
                        listKeyframes.Items[kfIndex] = kf;
                    }
                    else
                    {
                        kf = FogAnimationFrame.Empty;
                        kf.SetBools(index, true);
                        kf.Index = _currentPage;
                        pkf[index] = val;

                        int count = listKeyframes.Items.Count;
                        for (x = 0; x < count && ((FogAnimationFrame) listKeyframes.Items[x]).Index < _currentPage; x++)
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
                FogAnimationFrame f = (FogAnimationFrame) listKeyframes.SelectedItem;
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
            panel1 = new Panel();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            groupBox1 = new GroupBox();
            listKeyframes = new ListBox();
            panel2 = new Panel();
            label7 = new Label();
            label3 = new Label();
            btnNext = new Button();
            btnPrev = new Button();
            lblFrameCount = new Label();
            numStart = new NumericInputBox();
            numEnd = new NumericInputBox();
            numFrame = new NumericUpDown();
            label1 = new Label();
            tabPage3 = new TabPage();
            lightCtrl = new CLRControl();
            panel1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox1.SuspendLayout();
            panel2.SuspendLayout();
            ((ISupportInitialize) numFrame).BeginInit();
            tabPage3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(tabControl1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(221, 200);
            panel1.TabIndex = 23;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(221, 200);
            tabControl1.TabIndex = 18;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Controls.Add(panel2);
            tabPage1.Location = new Point(4, 22);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(213, 174);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Lighting";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(listKeyframes);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(213, 102);
            groupBox1.TabIndex = 21;
            groupBox1.TabStop = false;
            groupBox1.Text = "Keyframes";
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
            listKeyframes.Size = new Size(207, 83);
            listKeyframes.TabIndex = 18;
            listKeyframes.SelectedIndexChanged += new EventHandler(listKeyframes_SelectedIndexChanged);
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.Control;
            panel2.Controls.Add(label7);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(btnNext);
            panel2.Controls.Add(btnPrev);
            panel2.Controls.Add(lblFrameCount);
            panel2.Controls.Add(numStart);
            panel2.Controls.Add(numEnd);
            panel2.Controls.Add(numFrame);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 102);
            panel2.Name = "panel2";
            panel2.Size = new Size(213, 72);
            panel2.TabIndex = 22;
            // 
            // label7
            // 
            label7.Location = new Point(3, 3);
            label7.Name = "label7";
            label7.Size = new Size(41, 20);
            label7.TabIndex = 15;
            label7.Text = "Frame:";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.BorderStyle = BorderStyle.FixedSingle;
            label3.Location = new Point(6, 45);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(70, 20);
            label3.TabIndex = 2;
            label3.Text = "End Point Z";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnNext
            // 
            btnNext.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnNext.Location = new Point(187, 1);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(23, 23);
            btnNext.TabIndex = 2;
            btnNext.Text = ">";
            btnNext.TextAlign = ContentAlignment.TopCenter;
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += new EventHandler(btnNext_Click);
            // 
            // btnPrev
            // 
            btnPrev.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPrev.Location = new Point(162, 1);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(23, 23);
            btnPrev.TabIndex = 1;
            btnPrev.Text = "<";
            btnPrev.TextAlign = ContentAlignment.TopCenter;
            btnPrev.UseVisualStyleBackColor = true;
            btnPrev.Click += new EventHandler(btnPrev_Click);
            // 
            // lblFrameCount
            // 
            lblFrameCount.Location = new Point(114, 3);
            lblFrameCount.Name = "lblFrameCount";
            lblFrameCount.Size = new Size(51, 20);
            lblFrameCount.TabIndex = 17;
            lblFrameCount.Text = "/ 10";
            lblFrameCount.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numStart
            // 
            numStart.BorderStyle = BorderStyle.FixedSingle;
            numStart.Location = new Point(75, 26);
            numStart.Margin = new Padding(0);
            numStart.Name = "numStart";
            numStart.Size = new Size(70, 20);
            numStart.TabIndex = 3;
            numStart.Text = "0";
            numStart.ValueChanged += new EventHandler(BoxChanged);
            // 
            // numEnd
            // 
            numEnd.BorderStyle = BorderStyle.FixedSingle;
            numEnd.Location = new Point(75, 45);
            numEnd.Margin = new Padding(0, 10, 0, 10);
            numEnd.Name = "numEnd";
            numEnd.Size = new Size(70, 20);
            numEnd.TabIndex = 6;
            numEnd.Text = "0";
            numEnd.ValueChanged += new EventHandler(BoxChanged);
            // 
            // numFrame
            // 
            numFrame.Location = new Point(50, 3);
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
            // label1
            // 
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Location = new Point(6, 26);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(70, 20);
            label1.TabIndex = 0;
            label1.Text = "Start Point Z";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(lightCtrl);
            tabPage3.Location = new Point(4, 22);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(213, 174);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Light Color";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // lightCtrl
            // 
            lightCtrl.Dock = DockStyle.Fill;
            lightCtrl.Location = new Point(0, 0);
            lightCtrl.Name = "lightCtrl";
            lightCtrl.Size = new Size(213, 174);
            lightCtrl.TabIndex = 0;
            // 
            // SCN0FogEditControl
            // 
            Controls.Add(panel1);
            Name = "SCN0FogEditControl";
            Size = new Size(221, 200);
            panel1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((ISupportInitialize) numFrame).EndInit();
            tabPage3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
}