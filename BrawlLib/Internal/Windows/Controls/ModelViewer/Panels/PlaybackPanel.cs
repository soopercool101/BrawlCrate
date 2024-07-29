using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using System;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.Panels
{
    public class ModelPlaybackPanel : UserControl
    {
        #region Designer

        public Button btnPlay;
        public NumericUpDown numTotalFrames;
        public NumericUpDown numFPS;
        private Label label14;
        public CheckBox chkLoop;
        public NumericUpDown numFrameIndex;
        public Button btnPrevFrame;
        public Button btnNextFrame;
        public Button btnFirst;
        private Label label15;
        private Label label1;
        public Label lblLoopFrame;
        private TrackBar trbFrame;
        public Button btnLast;

        private void InitializeComponent()
        {
            btnPlay = new Button();
            numTotalFrames = new NumericUpDown();
            numFPS = new NumericUpDown();
            label14 = new Label();
            chkLoop = new CheckBox();
            numFrameIndex = new NumericUpDown();
            btnPrevFrame = new Button();
            btnNextFrame = new Button();
            btnFirst = new Button();
            btnLast = new Button();
            label15 = new Label();
            label1 = new Label();
            lblLoopFrame = new Label();
            trbFrame = new TrackBar();
            ((System.ComponentModel.ISupportInitialize) numTotalFrames).BeginInit();
            ((System.ComponentModel.ISupportInitialize) numFPS).BeginInit();
            ((System.ComponentModel.ISupportInitialize) numFrameIndex).BeginInit();
            ((System.ComponentModel.ISupportInitialize) trbFrame).BeginInit();
            SuspendLayout();
            // 
            // btnPlay
            // 
            btnPlay.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                              | AnchorStyles.Right;
            btnPlay.Location = new System.Drawing.Point(458, 28);
            btnPlay.Margin = new Padding(1);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new System.Drawing.Size(44, 32);
            btnPlay.TabIndex = 14;
            btnPlay.Text = "Play";
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += new EventHandler(btnPlay_Click);
            // 
            // numTotalFrames
            // 
            numTotalFrames.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numTotalFrames.Enabled = false;
            numTotalFrames.Location = new System.Drawing.Point(517, 5);
            numTotalFrames.Maximum = new decimal(new int[]
            {
                65536,
                0,
                0,
                0
            });
            numTotalFrames.Name = "numTotalFrames";
            numTotalFrames.Size = new System.Drawing.Size(52, 22);
            numTotalFrames.TabIndex = 19;
            numTotalFrames.ValueChanged += new EventHandler(numTotalFrames_ValueChanged);
            // 
            // numFPS
            // 
            numFPS.Location = new System.Drawing.Point(40, 41);
            numFPS.Maximum = new decimal(new int[]
            {
                2000,
                0,
                0,
                0
            });
            numFPS.Minimum = new decimal(new int[]
            {
                1,
                0,
                0,
                0
            });
            numFPS.Name = "numFPS";
            numFPS.Size = new System.Drawing.Size(39, 22);
            numFPS.TabIndex = 15;
            numFPS.Value = new decimal(new int[]
            {
                60,
                0,
                0,
                0
            });
            numFPS.ValueChanged += new EventHandler(numFPS_ValueChanged);
            // 
            // label14
            // 
            label14.Location = new System.Drawing.Point(3, 40);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(65, 20);
            label14.TabIndex = 17;
            label14.Text = "Speed:";
            label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkLoop
            // 
            chkLoop.Location = new System.Drawing.Point(85, 40);
            chkLoop.Name = "chkLoop";
            chkLoop.Size = new System.Drawing.Size(81, 20);
            chkLoop.TabIndex = 16;
            chkLoop.Text = "Loop";
            chkLoop.UseVisualStyleBackColor = true;
            chkLoop.CheckedChanged += new EventHandler(chkLoop_CheckedChanged);
            // 
            // numFrameIndex
            // 
            numFrameIndex.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numFrameIndex.Location = new System.Drawing.Point(454, 5);
            numFrameIndex.Maximum = new decimal(new int[]
            {
                0,
                0,
                0,
                0
            });
            numFrameIndex.Name = "numFrameIndex";
            numFrameIndex.Size = new System.Drawing.Size(52, 19);
            numFrameIndex.TabIndex = 12;
            numFrameIndex.ValueChanged += new EventHandler(numFrameIndex_ValueChanged);
            // 
            // btnPrevFrame
            // 
            btnPrevFrame.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                                   | AnchorStyles.Right;
            btnPrevFrame.Enabled = false;
            btnPrevFrame.Location = new System.Drawing.Point(424, 28);
            btnPrevFrame.Margin = new Padding(1);
            btnPrevFrame.Name = "btnPrevFrame";
            btnPrevFrame.Size = new System.Drawing.Size(32, 32);
            btnPrevFrame.TabIndex = 11;
            btnPrevFrame.Text = "<";
            btnPrevFrame.UseVisualStyleBackColor = true;
            btnPrevFrame.Click += new EventHandler(btnPrevFrame_Click);
            // 
            // btnNextFrame
            // 
            btnNextFrame.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                                   | AnchorStyles.Right;
            btnNextFrame.Enabled = false;
            btnNextFrame.Location = new System.Drawing.Point(504, 28);
            btnNextFrame.Margin = new Padding(1);
            btnNextFrame.Name = "btnNextFrame";
            btnNextFrame.Size = new System.Drawing.Size(32, 32);
            btnNextFrame.TabIndex = 10;
            btnNextFrame.Text = ">";
            btnNextFrame.UseVisualStyleBackColor = true;
            btnNextFrame.Click += new EventHandler(btnNextFrame_Click);
            // 
            // btnFirst
            // 
            btnFirst.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                               | AnchorStyles.Right;
            btnFirst.Enabled = false;
            btnFirst.Location = new System.Drawing.Point(390, 28);
            btnFirst.Margin = new Padding(1);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new System.Drawing.Size(32, 32);
            btnFirst.TabIndex = 20;
            btnFirst.Text = "|<";
            btnFirst.UseVisualStyleBackColor = true;
            btnFirst.Click += new EventHandler(btnFirst_Click);
            // 
            // btnLast
            // 
            btnLast.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                              | AnchorStyles.Right;
            btnLast.Enabled = false;
            btnLast.Location = new System.Drawing.Point(537, 28);
            btnLast.Margin = new Padding(1);
            btnLast.Name = "btnLast";
            btnLast.Size = new System.Drawing.Size(32, 32);
            btnLast.TabIndex = 21;
            btnLast.Text = ">|";
            btnLast.UseVisualStyleBackColor = true;
            btnLast.Click += new EventHandler(btnLast_Click);
            // 
            // label15
            // 
            label15.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label15.Location = new System.Drawing.Point(413, 4);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(55, 20);
            label15.TabIndex = 23;
            label15.Text = "Frame: ";
            label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.Location = new System.Drawing.Point(506, 4);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(14, 20);
            label1.TabIndex = 24;
            label1.Text = "/";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLoopFrame
            // 
            lblLoopFrame.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblLoopFrame.AutoSize = true;
            lblLoopFrame.ForeColor = System.Drawing.Color.DarkRed;
            lblLoopFrame.Location = new System.Drawing.Point(293, 43);
            lblLoopFrame.Name = "lblLoopFrame";
            lblLoopFrame.Size = new System.Drawing.Size(65, 12);
            lblLoopFrame.TabIndex = 25;
            lblLoopFrame.Text = "Loop Frame";
            lblLoopFrame.Visible = false;
            // 
            // trbFrame
            // 
            trbFrame.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                               | AnchorStyles.Right;
            trbFrame.Location = new System.Drawing.Point(3, 3);
            trbFrame.Name = "trbFrame";
            trbFrame.Size = new System.Drawing.Size(404, 45);
            trbFrame.TabIndex = 26;
            trbFrame.ValueChanged += new EventHandler(trbFrame_ValueChanged);
            // 
            // ModelPlaybackPanel
            // 
            Controls.Add(lblLoopFrame);
            Controls.Add(btnLast);
            Controls.Add(btnFirst);
            Controls.Add(btnPlay);
            Controls.Add(numTotalFrames);
            Controls.Add(numFPS);
            Controls.Add(label14);
            Controls.Add(numFrameIndex);
            Controls.Add(btnPrevFrame);
            Controls.Add(btnNextFrame);
            Controls.Add(label15);
            Controls.Add(chkLoop);
            Controls.Add(label1);
            Controls.Add(trbFrame);
            Name = "ModelPlaybackPanel";
            Size = new System.Drawing.Size(574, 65);
            ((System.ComponentModel.ISupportInitialize) numTotalFrames).EndInit();
            ((System.ComponentModel.ISupportInitialize) numFPS).EndInit();
            ((System.ComponentModel.ISupportInitialize) numFrameIndex).EndInit();
            ((System.ComponentModel.ISupportInitialize) trbFrame).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public ModelPlaybackPanel()
        {
            InitializeComponent();
        }

        public ModelEditorBase _mainWindow;

        public void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
            _mainWindow.PlaybackPanel_LoopChanged();
        }

        public void btnPlay_Click(object sender, EventArgs e)
        {
            _mainWindow.TogglePlay();
        }

        public void btnNextFrame_Click(object sender, EventArgs e)
        {
            if (numFrameIndex.Value < numFrameIndex.Maximum)
            {
                numFrameIndex.Value++;
            }
            else if (numFrameIndex.Value == numFrameIndex.Maximum && numFrameIndex.Maximum > 0)
            {
                numFrameIndex.Value = 1;
            }
        }

        public void btnLast_Click(object sender, EventArgs e)
        {
            numFrameIndex.Value = numTotalFrames.Value;
        }

        public void btnPrevFrame_Click(object sender, EventArgs e)
        {
            if (numFrameIndex.Value > numFrameIndex.Minimum)
            {
                numFrameIndex.Value--;
            }
            else
            {
                numFrameIndex.Value = numFrameIndex.Maximum;
            }
        }

        public void btnFirst_Click(object sender, EventArgs e)
        {
            numFrameIndex.Value = 1;
        }

        public void numFPS_ValueChanged(object sender, EventArgs e)
        {
            _mainWindow.numFPS_ValueChanged(sender, e);
        }

        public void numFrameIndex_ValueChanged(object sender, EventArgs e)
        {
            lblLoopFrame.Visible = numFrameIndex.Value > _mainWindow.MaxFrame;
            trbFrame.Value = (int)numFrameIndex.Value;
            _mainWindow.numFrameIndex_ValueChanged(sender, e);
        }

        public void numTotalFrames_ValueChanged(object sender, EventArgs e)
        {
            trbFrame.Maximum = (int)numTotalFrames.Value;
            _mainWindow.numTotalFrames_ValueChanged(sender, e);
        }

        private void trbFrame_ValueChanged(object sender, EventArgs e)
        {
            numFrameIndex.Value = trbFrame.Value;
        }

        internal void UpdateInterface(int animFrame, int loopMax)
        {
            btnNextFrame.Enabled = animFrame < loopMax;
            btnPrevFrame.Enabled = animFrame > 0;

            btnLast.Enabled = animFrame != loopMax;
            btnFirst.Enabled = animFrame > 1;

            if (animFrame <= (float) numFrameIndex.Maximum)
            {
                numFrameIndex.Value = animFrame;
            }
        }
    }
}