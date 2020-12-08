using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public class ShpAnimEditControl : UserControl
    {
        private int _numFrames;

        private int _currentPage = 1;
        private float _currentValue;
        private readonly NumericInputBox[] _boxes = new NumericInputBox[1];
        private Panel panel1;
        private Label label2;

        private SHP0VertexSetNode _target;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SHP0VertexSetNode TargetSequence
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
                    {
                        if ((kfe = _target.GetKeyframe(x)) != null)
                        {
                            listKeyframes.Items.Add(new FloatKeyframe(kfe));
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
                //_currentPage = (int)numFrame.Value - 1;
                _currentValue = _target.Keyframes.GetFrameValue(_currentPage);
                numScale.Value = _currentValue * 100.0f;
                UpdateBox(0);

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
                if (((FloatKeyframe) listKeyframes.Items[i]).Index == index)
                {
                    return i;
                }
            }

            return -1;
        }

        private void UpdateBox(int index)
        {
            if (_target.GetKeyframe(_currentPage) != null)
            {
                _boxes[index].BackColor = Color.Yellow;
            }
            else
            {
                _boxes[index].BackColor = Color.White;
            }
        }

        private void BoxChanged(object sender, EventArgs e)
        {
            NumericInputBox box = sender as NumericInputBox;
            FloatKeyframe kf;
            float val = box.Value / 100.0f;
            int index = (int) box.Tag;
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
                        kf = (FloatKeyframe) listKeyframes.Items[kfIndex];
                        kf.Value = val;
                        listKeyframes.Items[kfIndex] = kf;
                    }
                    else
                    {
                        kf = new FloatKeyframe
                        {
                            Index = _currentPage,
                            Value = val
                        };

                        int count = listKeyframes.Items.Count;
                        for (x = 0; x < count && ((FloatKeyframe) listKeyframes.Items[x]).Index < _currentPage; x++)
                        {
                            ;
                        }

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
                FloatKeyframe f = (FloatKeyframe) listKeyframes.SelectedItem;
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
            label1 = new Label();
            numScale = new NumericInputBox();
            label7 = new Label();
            numFrame = new NumericUpDown();
            lblFrameCount = new Label();
            btnPrev = new Button();
            btnNext = new Button();
            listKeyframes = new ListBox();
            groupBox1 = new GroupBox();
            panel1 = new Panel();
            label2 = new Label();
            ((ISupportInitialize) numFrame).BeginInit();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Location = new Point(10, 32);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(67, 20);
            label1.TabIndex = 0;
            label1.Text = "Percentage:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numScale
            // 
            numScale.BorderStyle = BorderStyle.FixedSingle;
            numScale.Integral = false;
            numScale.Location = new Point(76, 32);
            numScale.Margin = new Padding(0);
            numScale.MaximumValue = 3.402823E+38F;
            numScale.MinimumValue = -3.402823E+38F;
            numScale.Name = "numScale";
            numScale.Size = new Size(37, 20);
            numScale.TabIndex = 3;
            numScale.Text = "0";
            numScale.TextAlign = HorizontalAlignment.Right;
            numScale.ValueChanged += new EventHandler(BoxChanged);
            // 
            // label7
            // 
            label7.Location = new Point(7, 3);
            label7.Name = "label7";
            label7.Size = new Size(42, 20);
            label7.TabIndex = 15;
            label7.Text = "Frame:";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numFrame
            // 
            numFrame.Location = new Point(55, 3);
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
            lblFrameCount.Location = new Point(119, 3);
            lblFrameCount.Name = "lblFrameCount";
            lblFrameCount.Size = new Size(49, 20);
            lblFrameCount.TabIndex = 17;
            lblFrameCount.Text = "/ 10";
            lblFrameCount.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnPrev
            // 
            btnPrev.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPrev.Location = new Point(166, 2);
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
            btnNext.Location = new Point(191, 2);
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
            listKeyframes.Size = new Size(224, 119);
            listKeyframes.TabIndex = 18;
            listKeyframes.SelectedIndexChanged += new EventHandler(listKeyframes_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(listKeyframes);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(230, 138);
            groupBox1.TabIndex = 21;
            groupBox1.TabStop = false;
            groupBox1.Text = "Keyframes";
            // 
            // panel1
            // 
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(btnNext);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnPrev);
            panel1.Controls.Add(numScale);
            panel1.Controls.Add(lblFrameCount);
            panel1.Controls.Add(numFrame);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 138);
            panel1.Name = "panel1";
            panel1.Size = new Size(230, 64);
            panel1.TabIndex = 23;
            // 
            // label2
            // 
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Location = new Point(112, 32);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(19, 20);
            label2.TabIndex = 18;
            label2.Text = "%";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ShpAnimEditControl
            // 
            Controls.Add(groupBox1);
            Controls.Add(panel1);
            Name = "ShpAnimEditControl";
            Size = new Size(230, 202);
            ((ISupportInitialize) numFrame).EndInit();
            groupBox1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }

    public class FloatKeyframe
    {
        private readonly KeyframeEntry _entry;

        public FloatKeyframe()
        {
            _entry = new KeyframeEntry(-1, 0);
        }

        public FloatKeyframe(KeyframeEntry e)
        {
            _entry = e;
        }

        public int Index
        {
            get => _entry._index;
            set => _entry._index = value;
        }

        public float Value
        {
            get => _entry._value;
            set => _entry._value = value;
        }

        public float Tangent
        {
            get => _entry._tangent;
            set => _entry._tangent = value;
        }

        public override string ToString()
        {
            return $"[{(_entry._index + 1).ToString().PadLeft(5)}] {_entry._value * 100.0f}%";
        }
    }
}