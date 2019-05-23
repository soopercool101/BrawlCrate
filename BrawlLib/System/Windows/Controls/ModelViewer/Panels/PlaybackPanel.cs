namespace System.Windows.Forms
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
        public Button btnLast;
    
        private void InitializeComponent()
        {
            this.btnPlay = new System.Windows.Forms.Button();
            this.numTotalFrames = new System.Windows.Forms.NumericUpDown();
            this.numFPS = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.numFrameIndex = new System.Windows.Forms.NumericUpDown();
            this.btnPrevFrame = new System.Windows.Forms.Button();
            this.btnNextFrame = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLoopFrame = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalFrames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFPS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrameIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.Location = new System.Drawing.Point(69, 28);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(1);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(313, 32);
            this.btnPlay.TabIndex = 14;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // numTotalFrames
            // 
            this.numTotalFrames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numTotalFrames.Enabled = false;
            this.numTotalFrames.Location = new System.Drawing.Point(397, 5);
            this.numTotalFrames.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.numTotalFrames.Name = "numTotalFrames";
            this.numTotalFrames.Size = new System.Drawing.Size(52, 22);
            this.numTotalFrames.TabIndex = 19;
            this.numTotalFrames.ValueChanged += new System.EventHandler(this.numTotalFrames_ValueChanged);
            // 
            // numFPS
            // 
            this.numFPS.Location = new System.Drawing.Point(53, 4);
            this.numFPS.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numFPS.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFPS.Name = "numFPS";
            this.numFPS.Size = new System.Drawing.Size(39, 22);
            this.numFPS.TabIndex = 15;
            this.numFPS.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numFPS.ValueChanged += new System.EventHandler(this.numFPS_ValueChanged);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(3, 3);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 20);
            this.label14.TabIndex = 17;
            this.label14.Text = "Speed:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkLoop
            // 
            this.chkLoop.Location = new System.Drawing.Point(98, 5);
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(81, 20);
            this.chkLoop.TabIndex = 16;
            this.chkLoop.Text = "Loop";
            this.chkLoop.UseVisualStyleBackColor = true;
            this.chkLoop.CheckedChanged += new System.EventHandler(this.chkLoop_CheckedChanged);
            // 
            // numFrameIndex
            // 
            this.numFrameIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numFrameIndex.Location = new System.Drawing.Point(334, 5);
            this.numFrameIndex.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numFrameIndex.Name = "numFrameIndex";
            this.numFrameIndex.Size = new System.Drawing.Size(52, 22);
            this.numFrameIndex.TabIndex = 12;
            this.numFrameIndex.ValueChanged += new System.EventHandler(this.numFrameIndex_ValueChanged);
            // 
            // btnPrevFrame
            // 
            this.btnPrevFrame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrevFrame.Enabled = false;
            this.btnPrevFrame.Location = new System.Drawing.Point(36, 28);
            this.btnPrevFrame.Margin = new System.Windows.Forms.Padding(1);
            this.btnPrevFrame.Name = "btnPrevFrame";
            this.btnPrevFrame.Size = new System.Drawing.Size(32, 32);
            this.btnPrevFrame.TabIndex = 11;
            this.btnPrevFrame.Text = "<";
            this.btnPrevFrame.UseVisualStyleBackColor = true;
            this.btnPrevFrame.Click += new System.EventHandler(this.btnPrevFrame_Click);
            // 
            // btnNextFrame
            // 
            this.btnNextFrame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextFrame.Enabled = false;
            this.btnNextFrame.Location = new System.Drawing.Point(384, 28);
            this.btnNextFrame.Margin = new System.Windows.Forms.Padding(1);
            this.btnNextFrame.Name = "btnNextFrame";
            this.btnNextFrame.Size = new System.Drawing.Size(32, 32);
            this.btnNextFrame.TabIndex = 10;
            this.btnNextFrame.Text = ">";
            this.btnNextFrame.UseVisualStyleBackColor = true;
            this.btnNextFrame.Click += new System.EventHandler(this.btnNextFrame_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFirst.Enabled = false;
            this.btnFirst.Location = new System.Drawing.Point(3, 28);
            this.btnFirst.Margin = new System.Windows.Forms.Padding(1);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(32, 32);
            this.btnFirst.TabIndex = 20;
            this.btnFirst.Text = "|<";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnLast
            // 
            this.btnLast.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLast.Enabled = false;
            this.btnLast.Location = new System.Drawing.Point(417, 28);
            this.btnLast.Margin = new System.Windows.Forms.Padding(1);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(32, 32);
            this.btnLast.TabIndex = 21;
            this.btnLast.Text = ">|";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.Location = new System.Drawing.Point(281, 5);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(55, 20);
            this.label15.TabIndex = 23;
            this.label15.Text = "Frame: ";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(386, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 20);
            this.label1.TabIndex = 24;
            this.label1.Text = "/";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLoopFrame
            // 
            this.lblLoopFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLoopFrame.AutoSize = true;
            this.lblLoopFrame.ForeColor = System.Drawing.Color.DarkRed;
            this.lblLoopFrame.Location = new System.Drawing.Point(194, 7);
            this.lblLoopFrame.Name = "lblLoopFrame";
            this.lblLoopFrame.Size = new System.Drawing.Size(84, 17);
            this.lblLoopFrame.TabIndex = 25;
            this.lblLoopFrame.Text = "Loop Frame";
            this.lblLoopFrame.Visible = false;
            // 
            // ModelPlaybackPanel
            // 
            this.Controls.Add(this.lblLoopFrame);
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.numTotalFrames);
            this.Controls.Add(this.numFPS);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.numFrameIndex);
            this.Controls.Add(this.btnPrevFrame);
            this.Controls.Add(this.btnNextFrame);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.chkLoop);
            this.Controls.Add(this.label1);
            this.Name = "ModelPlaybackPanel";
            this.Size = new System.Drawing.Size(454, 65);
            ((System.ComponentModel.ISupportInitialize)(this.numTotalFrames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFPS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrameIndex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ModelPlaybackPanel() { InitializeComponent(); }

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
                numFrameIndex.Value++;
            else if (numFrameIndex.Value == numFrameIndex.Maximum && numFrameIndex.Maximum > 0)
                numFrameIndex.Value = 1;
        }

        public void btnLast_Click(object sender, EventArgs e)
        {
            numFrameIndex.Value = numTotalFrames.Value;
        }

        public void btnPrevFrame_Click(object sender, EventArgs e)
        {
            if (numFrameIndex.Value > numFrameIndex.Minimum)
                numFrameIndex.Value--;
            else
                numFrameIndex.Value = numFrameIndex.Maximum;
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
            _mainWindow.numFrameIndex_ValueChanged(sender, e);
        }

        public void numTotalFrames_ValueChanged(object sender, EventArgs e)
        {
            _mainWindow.numTotalFrames_ValueChanged(sender, e);
        }

        internal void UpdateInterface(int animFrame, int loopMax)
        {
            btnNextFrame.Enabled = animFrame < loopMax;
            btnPrevFrame.Enabled = animFrame > 0;

            btnLast.Enabled = animFrame != loopMax;
            btnFirst.Enabled = animFrame > 1;

            if (animFrame <= (float)numFrameIndex.Maximum)
                numFrameIndex.Value = (decimal)animFrame;
        }
    }
}
