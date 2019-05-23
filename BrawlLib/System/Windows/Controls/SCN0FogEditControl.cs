using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class SCN0FogEditControl : UserControl
    {
        private int _numFrames;

        private int _currentPage = 1;
        private FogAnimationFrame _currentFrame = FogAnimationFrame.Empty;
        private NumericInputBox[] _boxes = new NumericInputBox[2];
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
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SCN0FogNode TargetSequence
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

        public SCN0FogEditControl() 
        {
            InitializeComponent();
            _boxes[0] = numStart;
            _boxes[1] = numEnd;

            for (int i = 0; i < 2; i++)
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
                    FogAnimationFrame a;
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
            lightCtrl.ColorSource = TargetSequence;
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

                numStart.Value = _currentFrame.Start;
                numEnd.Value = _currentFrame.End;

                for (int i = 0; i < 2; i++)
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
                if (((FogAnimationFrame)listKeyframes.Items[i]).Index == index)
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
            FogAnimationFrame kf;
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
                        kf = (FogAnimationFrame)listKeyframes.Items[kfIndex];
                        kf.SetBools(index, false);
                        pkf[index] = val;
                        for (x = 0; (x < 2) && float.IsNaN(pkf[x]); x++) ;
                        if (x == 2)
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
                        kf = (FogAnimationFrame)listKeyframes.Items[kfIndex];
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
                        for (x = 0; (x < count) && (((FogAnimationFrame)listKeyframes.Items[x]).Index < _currentPage); x++) ;

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
                FogAnimationFrame f = (FogAnimationFrame)listKeyframes.SelectedItem;
                numFrame.Value = (decimal)(f.Index + 1);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e) { numFrame.Value--; }
        private void btnNext_Click(object sender, EventArgs e) { numFrame.Value++; }

        #region Designer


        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listKeyframes = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblFrameCount = new System.Windows.Forms.Label();
            this.numStart = new System.Windows.Forms.NumericInputBox();
            this.numEnd = new System.Windows.Forms.NumericInputBox();
            this.numFrame = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lightCtrl = new System.Windows.Forms.CLRControl();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(221, 200);
            this.panel1.TabIndex = 23;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(221, 200);
            this.tabControl1.TabIndex = 18;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(213, 174);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Lighting";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listKeyframes);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(213, 102);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Keyframes";
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
            this.listKeyframes.Size = new System.Drawing.Size(207, 83);
            this.listKeyframes.TabIndex = 18;
            this.listKeyframes.SelectedIndexChanged += new System.EventHandler(this.listKeyframes_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.btnNext);
            this.panel2.Controls.Add(this.btnPrev);
            this.panel2.Controls.Add(this.lblFrameCount);
            this.panel2.Controls.Add(this.numStart);
            this.panel2.Controls.Add(this.numEnd);
            this.panel2.Controls.Add(this.numFrame);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 102);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(213, 72);
            this.panel2.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(3, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "Frame:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(6, 45);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "End Point Z";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(187, 1);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(23, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = ">";
            this.btnNext.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.Location = new System.Drawing.Point(162, 1);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(23, 23);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "<";
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblFrameCount
            // 
            this.lblFrameCount.Location = new System.Drawing.Point(114, 3);
            this.lblFrameCount.Name = "lblFrameCount";
            this.lblFrameCount.Size = new System.Drawing.Size(51, 20);
            this.lblFrameCount.TabIndex = 17;
            this.lblFrameCount.Text = "/ 10";
            this.lblFrameCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numStart
            // 
            this.numStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numStart.Location = new System.Drawing.Point(75, 26);
            this.numStart.Margin = new System.Windows.Forms.Padding(0);
            this.numStart.Name = "numStart";
            this.numStart.Size = new System.Drawing.Size(70, 20);
            this.numStart.TabIndex = 3;
            this.numStart.Text = "0";
            this.numStart.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numEnd
            // 
            this.numEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numEnd.Location = new System.Drawing.Point(75, 45);
            this.numEnd.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numEnd.Name = "numEnd";
            this.numEnd.Size = new System.Drawing.Size(70, 20);
            this.numEnd.TabIndex = 6;
            this.numEnd.Text = "0";
            this.numEnd.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numFrame
            // 
            this.numFrame.Location = new System.Drawing.Point(50, 3);
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
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start Point Z";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lightCtrl);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(213, 174);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Light Color";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lightCtrl
            // 
            this.lightCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightCtrl.Location = new System.Drawing.Point(0, 0);
            this.lightCtrl.Name = "lightCtrl";
            this.lightCtrl.Size = new System.Drawing.Size(213, 174);
            this.lightCtrl.TabIndex = 0;
            // 
            // SCN0FogEditControl
            // 
            this.Controls.Add(this.panel1);
            this.Name = "SCN0FogEditControl";
            this.Size = new System.Drawing.Size(221, 200);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
    }
}
