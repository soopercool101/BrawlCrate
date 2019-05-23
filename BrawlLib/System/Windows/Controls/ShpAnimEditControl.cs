using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class ShpAnimEditControl : UserControl
    {
        private int _numFrames;

        private int _currentPage = 1;
        private float _currentValue = 0;
        private NumericInputBox[] _boxes = new NumericInputBox[1];
        private Panel panel1;
        private Label label2;

        private SHP0VertexSetNode _target;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SHP0VertexSetNode TargetSequence
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

        public ShpAnimEditControl() 
        {
            InitializeComponent();
            _boxes[0] = numScale;
            _boxes[0].Tag = 0;
        }

        private void UpdateTarget()
        {
            KeyframeEntry kfe = null;
            listKeyframes.BeginUpdate();
            listKeyframes.Items.Clear();
            if (_target != null)
            {
                if (_target.FrameCount > 0)
                {
                    for (int x = 0; x < _target.FrameCount; x++)
                        if ((kfe = _target.GetKeyframe(x)) != null)
                            listKeyframes.Items.Add(new FloatKeyframe(kfe));

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
                _currentValue = _target.Keyframes.GetFrameValue(_currentPage);
                numScale.Value = _currentValue * 100.0f;
                UpdateBox(0);

                btnPrev.Enabled = _currentPage > 0;
                btnNext.Enabled = _currentPage < (_numFrames - 1);

                listKeyframes.SelectedIndex = FindKeyframe(_currentPage);
            }
        }

        private int FindKeyframe(int index)
        {
            int count = listKeyframes.Items.Count;
            for (int i = 0; i < count; i++)
                if (((FloatKeyframe)listKeyframes.Items[i]).Index == index)
                    return i;
            return -1;
        }

        private void UpdateBox(int index)
        {
            if (_target.GetKeyframe(_currentPage) != null)
                _boxes[index].BackColor = Color.Yellow;
            else
                _boxes[index].BackColor = Color.White;
        }

        private unsafe void BoxChanged(object sender, EventArgs e)
        {
            NumericInputBox box = sender as NumericInputBox;
            FloatKeyframe kf;
            float val = box.Value / 100.0f;
            int index = (int)box.Tag;
            int x;

            if (val != _currentValue)
            {
                int kfIndex = FindKeyframe(_currentPage);

                if (float.IsNaN(val))
                {
                    if (kfIndex >= 0)
                    {
                        listKeyframes.Items.RemoveAt(kfIndex);
                        listKeyframes.SelectedIndex = -1;
                    }

                    _target.RemoveKeyframe(_currentPage);
                    val = _target.Keyframes.GetFrameValue(_currentPage);
                    box.Value = val * 100.0f;
                }
                else
                {
                    if (kfIndex >= 0)
                    {
                        kf = (FloatKeyframe)listKeyframes.Items[kfIndex];
                        kf.Value = val;
                        listKeyframes.Items[kfIndex] = kf;
                    }
                    else
                    {
                        kf = new FloatKeyframe();
                        kf.Index = _currentPage;
                        kf.Value = val;

                        int count = listKeyframes.Items.Count;
                        for (x = 0; (x < count) && (((FloatKeyframe)listKeyframes.Items[x]).Index < _currentPage); x++) ;

                        listKeyframes.Items.Insert(x, kf);
                        listKeyframes.SelectedIndex = x;
                    }

                    _target.SetKeyframe(_currentPage, val);
                }

                _currentValue = val;
                UpdateBox(index);
            }
        }

        private void listKeyframes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listKeyframes.SelectedIndex;
            if (index >= 0)
            {
                FloatKeyframe f = (FloatKeyframe)listKeyframes.SelectedItem;
                numFrame.Value = f.Index + 1;
            }
        }

        private void btnPrev_Click(object sender, EventArgs e) { numFrame.Value -= 1; }
        private void btnNext_Click(object sender, EventArgs e) { numFrame.Value += 1; }

        #region Designer

        private Label label1;
        private NumericInputBox numScale;
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
            this.numScale = new System.Windows.Forms.NumericInputBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numFrame = new System.Windows.Forms.NumericUpDown();
            this.lblFrameCount = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.listKeyframes = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(10, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Percentage:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numScale
            // 
            this.numScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numScale.Integral = false;
            this.numScale.Location = new System.Drawing.Point(76, 32);
            this.numScale.Margin = new System.Windows.Forms.Padding(0);
            this.numScale.MaximumValue = 3.402823E+38F;
            this.numScale.MinimumValue = -3.402823E+38F;
            this.numScale.Name = "numScale";
            this.numScale.Size = new System.Drawing.Size(37, 20);
            this.numScale.TabIndex = 3;
            this.numScale.Text = "0";
            this.numScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numScale.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(7, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "Frame:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numFrame
            // 
            this.numFrame.Location = new System.Drawing.Point(55, 3);
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
            this.lblFrameCount.Location = new System.Drawing.Point(119, 3);
            this.lblFrameCount.Name = "lblFrameCount";
            this.lblFrameCount.Size = new System.Drawing.Size(49, 20);
            this.lblFrameCount.TabIndex = 17;
            this.lblFrameCount.Text = "/ 10";
            this.lblFrameCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPrev
            // 
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.Location = new System.Drawing.Point(166, 2);
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
            this.btnNext.Location = new System.Drawing.Point(191, 2);
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
            this.listKeyframes.Size = new System.Drawing.Size(224, 119);
            this.listKeyframes.TabIndex = 18;
            this.listKeyframes.SelectedIndexChanged += new System.EventHandler(this.listKeyframes_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listKeyframes);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 138);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Keyframes";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Controls.Add(this.numScale);
            this.panel1.Controls.Add(this.lblFrameCount);
            this.panel1.Controls.Add(this.numFrame);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 138);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(230, 64);
            this.panel1.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(112, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 20);
            this.label2.TabIndex = 18;
            this.label2.Text = "%";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ShpAnimEditControl
            // 
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "ShpAnimEditControl";
            this.Size = new System.Drawing.Size(230, 202);
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
    public class FloatKeyframe
    {
        KeyframeEntry _entry;

        public FloatKeyframe() { _entry = new KeyframeEntry(-1, 0); }
        public FloatKeyframe(KeyframeEntry e) { _entry = e; }

        public int Index { get { return _entry._index; } set { _entry._index = value; } }
        public float Value { get { return _entry._value; } set { _entry._value = value; } }
        public float Tangent { get { return _entry._tangent; } set { _entry._tangent = value; } }

        public override string ToString()
        {
            return String.Format("[{0}] {1}%", (_entry._index + 1).ToString().PadLeft(5), _entry._value * 100.0f);
        }
    }
}
