using BrawlLib.SSBB.ResourceNodes.ProjectPlus;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public class ASLIndicator : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.grpBoxUnusedGCN = new System.Windows.Forms.GroupBox();
            this.chkBoxGCN0x8000 = new System.Windows.Forms.CheckBox();
            this.chkBoxGCN0x4000 = new System.Windows.Forms.CheckBox();
            this.chkBoxGCN0x2000 = new System.Windows.Forms.CheckBox();
            this.chkBoxGCN0x0080 = new System.Windows.Forms.CheckBox();
            this.chkBoxGCNB = new System.Windows.Forms.CheckBox();
            this.chkBoxGCNX = new System.Windows.Forms.CheckBox();
            this.chkBoxGCNY = new System.Windows.Forms.CheckBox();
            this.chkBoxGCNDown = new System.Windows.Forms.CheckBox();
            this.chkBoxGCNRight = new System.Windows.Forms.CheckBox();
            this.chkBoxGCNUp = new System.Windows.Forms.CheckBox();
            this.chkBoxGCNLeft = new System.Windows.Forms.CheckBox();
            this.chkBoxGCNZ = new System.Windows.Forms.CheckBox();
            this.chkBoxGCNR = new System.Windows.Forms.CheckBox();
            this.chkBoxGCNL = new System.Windows.Forms.CheckBox();
            this.chkBoxGCNA = new System.Windows.Forms.CheckBox();
            this.chkBoxGCNStart = new System.Windows.Forms.CheckBox();
            this.grpBoxUnusedGCN.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 302);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(479, 3);
            this.splitter1.TabIndex = 7;
            this.splitter1.TabStop = false;
            // 
            // grpBoxUnusedGCN
            // 
            this.grpBoxUnusedGCN.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left 
                                                                                                                       | System.Windows.Forms.AnchorStyles.Right);
            this.grpBoxUnusedGCN.Controls.Add(this.chkBoxGCN0x8000);
            this.grpBoxUnusedGCN.Controls.Add(this.chkBoxGCN0x4000);
            this.grpBoxUnusedGCN.Controls.Add(this.chkBoxGCN0x2000);
            this.grpBoxUnusedGCN.Controls.Add(this.chkBoxGCN0x0080);
            this.grpBoxUnusedGCN.Location = new System.Drawing.Point(172, 185);
            this.grpBoxUnusedGCN.Name = "grpBoxUnusedGCN";
            this.grpBoxUnusedGCN.Size = new System.Drawing.Size(139, 111);
            this.grpBoxUnusedGCN.TabIndex = 26;
            this.grpBoxUnusedGCN.TabStop = false;
            this.grpBoxUnusedGCN.Text = "Unused";
            // 
            // chkBoxGCN0x8000
            // 
            this.chkBoxGCN0x8000.AutoSize = true;
            this.chkBoxGCN0x8000.Location = new System.Drawing.Point(6, 87);
            this.chkBoxGCN0x8000.Name = "chkBoxGCN0x8000";
            this.chkBoxGCN0x8000.Size = new System.Drawing.Size(61, 17);
            this.chkBoxGCN0x8000.TabIndex = 3;
            this.chkBoxGCN0x8000.Text = "0x8000";
            this.chkBoxGCN0x8000.UseVisualStyleBackColor = true;
            this.chkBoxGCN0x8000.CheckedChanged += new System.EventHandler(this.chkBoxGCN0x8000_CheckedChanged);
            // 
            // chkBoxGCN0x4000
            // 
            this.chkBoxGCN0x4000.AutoSize = true;
            this.chkBoxGCN0x4000.Location = new System.Drawing.Point(6, 64);
            this.chkBoxGCN0x4000.Name = "chkBoxGCN0x4000";
            this.chkBoxGCN0x4000.Size = new System.Drawing.Size(61, 17);
            this.chkBoxGCN0x4000.TabIndex = 2;
            this.chkBoxGCN0x4000.Text = "0x4000";
            this.chkBoxGCN0x4000.UseVisualStyleBackColor = true;
            this.chkBoxGCN0x4000.CheckedChanged += new System.EventHandler(this.chkBoxGCN0x4000_CheckedChanged);
            // 
            // chkBoxGCN0x2000
            // 
            this.chkBoxGCN0x2000.AutoSize = true;
            this.chkBoxGCN0x2000.Location = new System.Drawing.Point(6, 42);
            this.chkBoxGCN0x2000.Name = "chkBoxGCN0x2000";
            this.chkBoxGCN0x2000.Size = new System.Drawing.Size(61, 17);
            this.chkBoxGCN0x2000.TabIndex = 1;
            this.chkBoxGCN0x2000.Text = "0x2000";
            this.chkBoxGCN0x2000.UseVisualStyleBackColor = true;
            this.chkBoxGCN0x2000.CheckedChanged += new System.EventHandler(this.chkBoxGCN0x2000_CheckedChanged);
            // 
            // chkBoxGCN0x0080
            // 
            this.chkBoxGCN0x0080.AutoSize = true;
            this.chkBoxGCN0x0080.Location = new System.Drawing.Point(6, 19);
            this.chkBoxGCN0x0080.Name = "chkBoxGCN0x0080";
            this.chkBoxGCN0x0080.Size = new System.Drawing.Size(61, 17);
            this.chkBoxGCN0x0080.TabIndex = 0;
            this.chkBoxGCN0x0080.Text = "0x0080";
            this.chkBoxGCN0x0080.UseVisualStyleBackColor = true;
            this.chkBoxGCN0x0080.CheckedChanged += new System.EventHandler(this.chkBoxGCN0x0080_CheckedChanged);
            // 
            // chkBoxGCNB
            // 
            this.chkBoxGCNB.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.chkBoxGCNB.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxGCNB.Location = new System.Drawing.Point(380, 250);
            this.chkBoxGCNB.Name = "chkBoxGCNB";
            this.chkBoxGCNB.Size = new System.Drawing.Size(18, 43);
            this.chkBoxGCNB.TabIndex = 25;
            this.chkBoxGCNB.Text = "\r\n\r\nB";
            this.chkBoxGCNB.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkBoxGCNB.UseVisualStyleBackColor = true;
            this.chkBoxGCNB.CheckedChanged += new System.EventHandler(this.chkBoxGCNB_CheckedChanged);
            // 
            // chkBoxGCNX
            // 
            this.chkBoxGCNX.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.chkBoxGCNX.Location = new System.Drawing.Point(436, 245);
            this.chkBoxGCNX.Name = "chkBoxGCNX";
            this.chkBoxGCNX.Size = new System.Drawing.Size(33, 17);
            this.chkBoxGCNX.TabIndex = 23;
            this.chkBoxGCNX.Text = "X";
            this.chkBoxGCNX.UseVisualStyleBackColor = true;
            this.chkBoxGCNX.CheckedChanged += new System.EventHandler(this.chkBoxGCNX_CheckedChanged);
            // 
            // chkBoxGCNY
            // 
            this.chkBoxGCNY.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.chkBoxGCNY.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxGCNY.Location = new System.Drawing.Point(409, 209);
            this.chkBoxGCNY.Name = "chkBoxGCNY";
            this.chkBoxGCNY.Size = new System.Drawing.Size(18, 43);
            this.chkBoxGCNY.TabIndex = 22;
            this.chkBoxGCNY.Text = "Y\r\n\r\n\r\n";
            this.chkBoxGCNY.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkBoxGCNY.UseVisualStyleBackColor = true;
            this.chkBoxGCNY.CheckedChanged += new System.EventHandler(this.chkBoxGCNY_CheckedChanged);
            // 
            // chkBoxGCNDown
            // 
            this.chkBoxGCNDown.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.chkBoxGCNDown.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxGCNDown.Location = new System.Drawing.Point(52, 252);
            this.chkBoxGCNDown.Name = "chkBoxGCNDown";
            this.chkBoxGCNDown.Size = new System.Drawing.Size(17, 43);
            this.chkBoxGCNDown.TabIndex = 21;
            this.chkBoxGCNDown.Text = "\r\n\r\n↓";
            this.chkBoxGCNDown.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkBoxGCNDown.UseVisualStyleBackColor = true;
            this.chkBoxGCNDown.CheckedChanged += new System.EventHandler(this.chkBoxGCNDown_CheckedChanged);
            // 
            // chkBoxGCNRight
            // 
            this.chkBoxGCNRight.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.chkBoxGCNRight.Location = new System.Drawing.Point(74, 245);
            this.chkBoxGCNRight.Name = "chkBoxGCNRight";
            this.chkBoxGCNRight.Size = new System.Drawing.Size(37, 17);
            this.chkBoxGCNRight.TabIndex = 20;
            this.chkBoxGCNRight.Text = "→";
            this.chkBoxGCNRight.UseVisualStyleBackColor = true;
            this.chkBoxGCNRight.CheckedChanged += new System.EventHandler(this.chkBoxGCNRight_CheckedChanged);
            // 
            // chkBoxGCNUp
            // 
            this.chkBoxGCNUp.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.chkBoxGCNUp.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxGCNUp.Location = new System.Drawing.Point(52, 212);
            this.chkBoxGCNUp.Name = "chkBoxGCNUp";
            this.chkBoxGCNUp.Size = new System.Drawing.Size(17, 43);
            this.chkBoxGCNUp.TabIndex = 19;
            this.chkBoxGCNUp.Text = "↑\r\n\r\n\r\n";
            this.chkBoxGCNUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkBoxGCNUp.UseVisualStyleBackColor = true;
            this.chkBoxGCNUp.CheckedChanged += new System.EventHandler(this.chkBoxGCNUp_CheckedChanged);
            // 
            // chkBoxGCNLeft
            // 
            this.chkBoxGCNLeft.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.chkBoxGCNLeft.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBoxGCNLeft.Location = new System.Drawing.Point(9, 245);
            this.chkBoxGCNLeft.Name = "chkBoxGCNLeft";
            this.chkBoxGCNLeft.Size = new System.Drawing.Size(37, 17);
            this.chkBoxGCNLeft.TabIndex = 18;
            this.chkBoxGCNLeft.Text = "←";
            this.chkBoxGCNLeft.UseVisualStyleBackColor = true;
            this.chkBoxGCNLeft.CheckedChanged += new System.EventHandler(this.chkBoxGCNLeft_CheckedChanged);
            // 
            // chkBoxGCNZ
            // 
            this.chkBoxGCNZ.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this.chkBoxGCNZ.Location = new System.Drawing.Point(429, 33);
            this.chkBoxGCNZ.Name = "chkBoxGCNZ";
            this.chkBoxGCNZ.Size = new System.Drawing.Size(33, 17);
            this.chkBoxGCNZ.TabIndex = 16;
            this.chkBoxGCNZ.Text = "Z";
            this.chkBoxGCNZ.UseVisualStyleBackColor = true;
            this.chkBoxGCNZ.CheckedChanged += new System.EventHandler(this.chkBoxGCNZ_CheckedChanged);
            // 
            // chkBoxGCNR
            // 
            this.chkBoxGCNR.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this.chkBoxGCNR.Location = new System.Drawing.Point(429, 10);
            this.chkBoxGCNR.Name = "chkBoxGCNR";
            this.chkBoxGCNR.Size = new System.Drawing.Size(34, 17);
            this.chkBoxGCNR.TabIndex = 15;
            this.chkBoxGCNR.Text = "R";
            this.chkBoxGCNR.UseVisualStyleBackColor = true;
            this.chkBoxGCNR.CheckedChanged += new System.EventHandler(this.chkBoxGCNR_CheckedChanged);
            // 
            // chkBoxGCNL
            // 
            this.chkBoxGCNL.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBoxGCNL.Location = new System.Drawing.Point(12, 10);
            this.chkBoxGCNL.Name = "chkBoxGCNL";
            this.chkBoxGCNL.Size = new System.Drawing.Size(32, 17);
            this.chkBoxGCNL.TabIndex = 14;
            this.chkBoxGCNL.Text = "L";
            this.chkBoxGCNL.UseVisualStyleBackColor = true;
            this.chkBoxGCNL.CheckedChanged += new System.EventHandler(this.chkBoxGCNL_CheckedChanged);
            // 
            // chkBoxGCNA
            // 
            this.chkBoxGCNA.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.chkBoxGCNA.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxGCNA.Location = new System.Drawing.Point(406, 238);
            this.chkBoxGCNA.Name = "chkBoxGCNA";
            this.chkBoxGCNA.Size = new System.Drawing.Size(18, 43);
            this.chkBoxGCNA.TabIndex = 24;
            this.chkBoxGCNA.Text = "\r\n\r\nA";
            this.chkBoxGCNA.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkBoxGCNA.UseVisualStyleBackColor = true;
            this.chkBoxGCNA.CheckedChanged += new System.EventHandler(this.chkBoxGCNA_CheckedChanged);
            // 
            // chkBoxGCNStart
            // 
            this.chkBoxGCNStart.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.chkBoxGCNStart.AutoSize = true;
            this.chkBoxGCNStart.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxGCNStart.Location = new System.Drawing.Point(336, 199);
            this.chkBoxGCNStart.Name = "chkBoxGCNStart";
            this.chkBoxGCNStart.Size = new System.Drawing.Size(33, 43);
            this.chkBoxGCNStart.TabIndex = 17;
            this.chkBoxGCNStart.Text = "Start\r\n\r\n\r\n";
            this.chkBoxGCNStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxGCNStart.UseVisualStyleBackColor = true;
            this.chkBoxGCNStart.CheckedChanged += new System.EventHandler(this.chkBoxGCNStart_CheckedChanged);
            // 
            // ASLIndicator
            // 
            this.Controls.Add(this.grpBoxUnusedGCN);
            this.Controls.Add(this.chkBoxGCNB);
            this.Controls.Add(this.chkBoxGCNX);
            this.Controls.Add(this.chkBoxGCNY);
            this.Controls.Add(this.chkBoxGCNDown);
            this.Controls.Add(this.chkBoxGCNRight);
            this.Controls.Add(this.chkBoxGCNUp);
            this.Controls.Add(this.chkBoxGCNLeft);
            this.Controls.Add(this.chkBoxGCNZ);
            this.Controls.Add(this.chkBoxGCNR);
            this.Controls.Add(this.chkBoxGCNL);
            this.Controls.Add(this.chkBoxGCNA);
            this.Controls.Add(this.chkBoxGCNStart);
            this.Controls.Add(this.splitter1);
            this.Name = "ASLIndicator";
            this.Size = new System.Drawing.Size(479, 305);
            this.grpBoxUnusedGCN.ResumeLayout(false);
            this.grpBoxUnusedGCN.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ASLIndicator()
        {
            InitializeComponent();
        }

        private Splitter splitter1;
        public bool called;

        private ASLSEntryNode _targetNode;
        private GroupBox grpBoxUnusedGCN;
        private CheckBox chkBoxGCN0x8000;
        private CheckBox chkBoxGCN0x4000;
        private CheckBox chkBoxGCN0x2000;
        private CheckBox chkBoxGCN0x0080;
        private CheckBox chkBoxGCNB;
        private CheckBox chkBoxGCNX;
        private CheckBox chkBoxGCNY;
        private CheckBox chkBoxGCNDown;
        private CheckBox chkBoxGCNRight;
        private CheckBox chkBoxGCNUp;
        private CheckBox chkBoxGCNLeft;
        private CheckBox chkBoxGCNZ;
        private CheckBox chkBoxGCNR;
        private CheckBox chkBoxGCNL;
        private CheckBox chkBoxGCNA;
        private CheckBox chkBoxGCNStart;
        private bool _updating;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ASLSEntryNode TargetNode
        {
            get => _targetNode;
            set
            {
                _targetNode = value;
                TargetChanged();
            }
        }

        public void TargetChanged()
        {
            _updating = true;

            // Gamecube buttons
            chkBoxGCNLeft.Checked = ((ASLSEntryNode.GameCubeButtons)_targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.Left) != 0;
            chkBoxGCNRight.Checked = ((ASLSEntryNode.GameCubeButtons)_targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.Right) != 0;
            chkBoxGCNUp.Checked = ((ASLSEntryNode.GameCubeButtons)_targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.Up) != 0;
            chkBoxGCNDown.Checked = ((ASLSEntryNode.GameCubeButtons)_targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.Down) != 0;
            chkBoxGCNA.Checked = ((ASLSEntryNode.GameCubeButtons)_targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.A) != 0;
            chkBoxGCNB.Checked = ((ASLSEntryNode.GameCubeButtons)_targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.B) != 0;
            chkBoxGCNX.Checked = ((ASLSEntryNode.GameCubeButtons)_targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.X) != 0;
            chkBoxGCNY.Checked = ((ASLSEntryNode.GameCubeButtons)_targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.Y) != 0;
            chkBoxGCNL.Checked = ((ASLSEntryNode.GameCubeButtons)_targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.L) != 0;
            chkBoxGCNR.Checked = ((ASLSEntryNode.GameCubeButtons)_targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.R) != 0;
            chkBoxGCNZ.Checked = ((ASLSEntryNode.GameCubeButtons)_targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.Z) != 0;
            chkBoxGCNStart.Checked = ((ASLSEntryNode.GameCubeButtons)_targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.Start) != 0;
            chkBoxGCN0x0080.Checked = ((ASLSEntryNode.GameCubeButtons)_targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.Unused0x0080) != 0;
            chkBoxGCN0x2000.Checked = ((ASLSEntryNode.GameCubeButtons)_targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.Unused0x2000) != 0;
            chkBoxGCN0x4000.Checked = ((ASLSEntryNode.GameCubeButtons)_targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.Unused0x4000) != 0;
            chkBoxGCN0x8000.Checked = ((ASLSEntryNode.GameCubeButtons)_targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.Unused0x8000) != 0;

#if !DEBUG
            grpBoxUnusedGCN.Visible = chkBoxGCN0x0080.Checked || chkBoxGCN0x2000.Checked || chkBoxGCN0x4000.Checked ||
                                      chkBoxGCN0x8000.Checked;
#endif

            _updating = false;
        }

        private void chkBoxGCNL_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.L) |
                      (chkBoxGCNL.Checked ? ASLSEntryNode.GameCubeButtons.L : 0));
        }

        private void chkBoxGCNR_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.R) |
                                              (chkBoxGCNR.Checked ? ASLSEntryNode.GameCubeButtons.R : 0));
        }

        private void chkBoxGCNZ_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.Z) |
                                              (chkBoxGCNZ.Checked ? ASLSEntryNode.GameCubeButtons.Z : 0));
        }

        private void chkBoxGCNStart_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.Start) |
                                              (chkBoxGCNStart.Checked ? ASLSEntryNode.GameCubeButtons.Start : 0));
        }

        private void chkBoxGCNUp_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.Up) |
                                              (chkBoxGCNUp.Checked ? ASLSEntryNode.GameCubeButtons.Up : 0));
        }

        private void chkBoxGCNDown_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.Down) |
                                              (chkBoxGCNDown.Checked ? ASLSEntryNode.GameCubeButtons.Down : 0));
        }

        private void chkBoxGCNLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.Left) |
                                              (chkBoxGCNLeft.Checked ? ASLSEntryNode.GameCubeButtons.Left : 0));
        }

        private void chkBoxGCNRight_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.Right) |
                                              (chkBoxGCNRight.Checked ? ASLSEntryNode.GameCubeButtons.Right : 0));
        }

        private void chkBoxGCNA_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.A) |
                                              (chkBoxGCNA.Checked ? ASLSEntryNode.GameCubeButtons.A : 0));
        }

        private void chkBoxGCNB_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.B) |
                                              (chkBoxGCNB.Checked ? ASLSEntryNode.GameCubeButtons.B : 0));
        }

        private void chkBoxGCNX_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.X) |
                                              (chkBoxGCNX.Checked ? ASLSEntryNode.GameCubeButtons.X : 0));
        }

        private void chkBoxGCNY_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.Y) |
                                              (chkBoxGCNY.Checked ? ASLSEntryNode.GameCubeButtons.Y : 0));
        }

        private void chkBoxGCN0x0080_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.Unused0x0080) |
                                              (chkBoxGCN0x0080.Checked ? ASLSEntryNode.GameCubeButtons.Unused0x0080 : 0));
        }

        private void chkBoxGCN0x2000_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.Unused0x2000) |
                                              (chkBoxGCN0x2000.Checked ? ASLSEntryNode.GameCubeButtons.Unused0x2000 : 0));
        }

        private void chkBoxGCN0x4000_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.Unused0x4000) |
                                              (chkBoxGCN0x4000.Checked ? ASLSEntryNode.GameCubeButtons.Unused0x4000 : 0));
        }

        private void chkBoxGCN0x8000_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.Unused0x8000) |
                                              (chkBoxGCN0x8000.Checked ? ASLSEntryNode.GameCubeButtons.Unused0x8000 : 0));
        }
    }
}