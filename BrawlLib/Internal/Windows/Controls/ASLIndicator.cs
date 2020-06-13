using BrawlLib.SSBB.ResourceNodes.ProjectPlus;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public unsafe class ASLIndicator : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            splitter1 = new Splitter();
            grpBoxUnusedGCN = new GroupBox();
            chkBoxGCN0x8000 = new CheckBox();
            chkBoxGCN0x4000 = new CheckBox();
            chkBoxGCN0x2000 = new CheckBox();
            chkBoxGCN0x0080 = new CheckBox();
            chkBoxGCNB = new CheckBox();
            chkBoxGCNX = new CheckBox();
            chkBoxGCNY = new CheckBox();
            chkBoxGCNDown = new CheckBox();
            chkBoxGCNRight = new CheckBox();
            chkBoxGCNUp = new CheckBox();
            chkBoxGCNLeft = new CheckBox();
            chkBoxGCNZ = new CheckBox();
            chkBoxGCNR = new CheckBox();
            chkBoxGCNL = new CheckBox();
            chkBoxGCNA = new CheckBox();
            chkBoxGCNStart = new CheckBox();
            grpBoxUnusedGCN.SuspendLayout();
            SuspendLayout();
            // 
            // splitter1
            // 
            splitter1.Dock = DockStyle.Bottom;
            splitter1.Location = new System.Drawing.Point(0, 302);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(479, 3);
            splitter1.TabIndex = 7;
            splitter1.TabStop = false;
            // 
            // grpBoxUnusedGCN
            // 
            grpBoxUnusedGCN.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left
                                                                         | AnchorStyles.Right);
            grpBoxUnusedGCN.Controls.Add(chkBoxGCN0x8000);
            grpBoxUnusedGCN.Controls.Add(chkBoxGCN0x4000);
            grpBoxUnusedGCN.Controls.Add(chkBoxGCN0x2000);
            grpBoxUnusedGCN.Controls.Add(chkBoxGCN0x0080);
            grpBoxUnusedGCN.Location = new System.Drawing.Point(172, 185);
            grpBoxUnusedGCN.Name = "grpBoxUnusedGCN";
            grpBoxUnusedGCN.Size = new System.Drawing.Size(139, 111);
            grpBoxUnusedGCN.TabIndex = 26;
            grpBoxUnusedGCN.TabStop = false;
            grpBoxUnusedGCN.Text = "Unused";
            // 
            // chkBoxGCN0x8000
            // 
            chkBoxGCN0x8000.AutoSize = true;
            chkBoxGCN0x8000.Location = new System.Drawing.Point(6, 87);
            chkBoxGCN0x8000.Name = "chkBoxGCN0x8000";
            chkBoxGCN0x8000.Size = new System.Drawing.Size(61, 17);
            chkBoxGCN0x8000.TabIndex = 3;
            chkBoxGCN0x8000.Text = "0x8000";
            chkBoxGCN0x8000.UseVisualStyleBackColor = true;
            chkBoxGCN0x8000.CheckedChanged += new EventHandler(chkBoxGCN0x8000_CheckedChanged);
            // 
            // chkBoxGCN0x4000
            // 
            chkBoxGCN0x4000.AutoSize = true;
            chkBoxGCN0x4000.Location = new System.Drawing.Point(6, 64);
            chkBoxGCN0x4000.Name = "chkBoxGCN0x4000";
            chkBoxGCN0x4000.Size = new System.Drawing.Size(61, 17);
            chkBoxGCN0x4000.TabIndex = 2;
            chkBoxGCN0x4000.Text = "0x4000";
            chkBoxGCN0x4000.UseVisualStyleBackColor = true;
            chkBoxGCN0x4000.CheckedChanged += new EventHandler(chkBoxGCN0x4000_CheckedChanged);
            // 
            // chkBoxGCN0x2000
            // 
            chkBoxGCN0x2000.AutoSize = true;
            chkBoxGCN0x2000.Location = new System.Drawing.Point(6, 42);
            chkBoxGCN0x2000.Name = "chkBoxGCN0x2000";
            chkBoxGCN0x2000.Size = new System.Drawing.Size(61, 17);
            chkBoxGCN0x2000.TabIndex = 1;
            chkBoxGCN0x2000.Text = "0x2000";
            chkBoxGCN0x2000.UseVisualStyleBackColor = true;
            chkBoxGCN0x2000.CheckedChanged += new EventHandler(chkBoxGCN0x2000_CheckedChanged);
            // 
            // chkBoxGCN0x0080
            // 
            chkBoxGCN0x0080.AutoSize = true;
            chkBoxGCN0x0080.Location = new System.Drawing.Point(6, 19);
            chkBoxGCN0x0080.Name = "chkBoxGCN0x0080";
            chkBoxGCN0x0080.Size = new System.Drawing.Size(61, 17);
            chkBoxGCN0x0080.TabIndex = 0;
            chkBoxGCN0x0080.Text = "0x0080";
            chkBoxGCN0x0080.UseVisualStyleBackColor = true;
            chkBoxGCN0x0080.CheckedChanged += new EventHandler(chkBoxGCN0x0080_CheckedChanged);
            // 
            // chkBoxGCNB
            // 
            chkBoxGCNB.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Right);
            chkBoxGCNB.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            chkBoxGCNB.Location = new System.Drawing.Point(380, 250);
            chkBoxGCNB.Name = "chkBoxGCNB";
            chkBoxGCNB.Size = new System.Drawing.Size(18, 43);
            chkBoxGCNB.TabIndex = 25;
            chkBoxGCNB.Text = "\r\n\r\nB";
            chkBoxGCNB.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            chkBoxGCNB.UseVisualStyleBackColor = true;
            chkBoxGCNB.CheckedChanged += new EventHandler(chkBoxGCNB_CheckedChanged);
            // 
            // chkBoxGCNX
            // 
            chkBoxGCNX.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Right);
            chkBoxGCNX.Location = new System.Drawing.Point(436, 245);
            chkBoxGCNX.Name = "chkBoxGCNX";
            chkBoxGCNX.Size = new System.Drawing.Size(33, 17);
            chkBoxGCNX.TabIndex = 23;
            chkBoxGCNX.Text = "X";
            chkBoxGCNX.UseVisualStyleBackColor = true;
            chkBoxGCNX.CheckedChanged += new EventHandler(chkBoxGCNX_CheckedChanged);
            // 
            // chkBoxGCNY
            // 
            chkBoxGCNY.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Right);
            chkBoxGCNY.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            chkBoxGCNY.Location = new System.Drawing.Point(409, 209);
            chkBoxGCNY.Name = "chkBoxGCNY";
            chkBoxGCNY.Size = new System.Drawing.Size(18, 43);
            chkBoxGCNY.TabIndex = 22;
            chkBoxGCNY.Text = "Y\r\n\r\n\r\n";
            chkBoxGCNY.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            chkBoxGCNY.UseVisualStyleBackColor = true;
            chkBoxGCNY.CheckedChanged += new EventHandler(chkBoxGCNY_CheckedChanged);
            // 
            // chkBoxGCNDown
            // 
            chkBoxGCNDown.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            chkBoxGCNDown.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            chkBoxGCNDown.Location = new System.Drawing.Point(52, 252);
            chkBoxGCNDown.Name = "chkBoxGCNDown";
            chkBoxGCNDown.Size = new System.Drawing.Size(17, 43);
            chkBoxGCNDown.TabIndex = 21;
            chkBoxGCNDown.Text = "\r\n\r\n↓";
            chkBoxGCNDown.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            chkBoxGCNDown.UseVisualStyleBackColor = true;
            chkBoxGCNDown.CheckedChanged += new EventHandler(chkBoxGCNDown_CheckedChanged);
            // 
            // chkBoxGCNRight
            // 
            chkBoxGCNRight.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            chkBoxGCNRight.Location = new System.Drawing.Point(74, 245);
            chkBoxGCNRight.Name = "chkBoxGCNRight";
            chkBoxGCNRight.Size = new System.Drawing.Size(37, 17);
            chkBoxGCNRight.TabIndex = 20;
            chkBoxGCNRight.Text = "→";
            chkBoxGCNRight.UseVisualStyleBackColor = true;
            chkBoxGCNRight.CheckedChanged += new EventHandler(chkBoxGCNRight_CheckedChanged);
            // 
            // chkBoxGCNUp
            // 
            chkBoxGCNUp.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            chkBoxGCNUp.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            chkBoxGCNUp.Location = new System.Drawing.Point(52, 212);
            chkBoxGCNUp.Name = "chkBoxGCNUp";
            chkBoxGCNUp.Size = new System.Drawing.Size(17, 43);
            chkBoxGCNUp.TabIndex = 19;
            chkBoxGCNUp.Text = "↑\r\n\r\n\r\n";
            chkBoxGCNUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            chkBoxGCNUp.UseVisualStyleBackColor = true;
            chkBoxGCNUp.CheckedChanged += new EventHandler(chkBoxGCNUp_CheckedChanged);
            // 
            // chkBoxGCNLeft
            // 
            chkBoxGCNLeft.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            chkBoxGCNLeft.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            chkBoxGCNLeft.Location = new System.Drawing.Point(9, 245);
            chkBoxGCNLeft.Name = "chkBoxGCNLeft";
            chkBoxGCNLeft.Size = new System.Drawing.Size(37, 17);
            chkBoxGCNLeft.TabIndex = 18;
            chkBoxGCNLeft.Text = "←";
            chkBoxGCNLeft.UseVisualStyleBackColor = true;
            chkBoxGCNLeft.CheckedChanged += new EventHandler(chkBoxGCNLeft_CheckedChanged);
            // 
            // chkBoxGCNZ
            // 
            chkBoxGCNZ.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            chkBoxGCNZ.Location = new System.Drawing.Point(429, 33);
            chkBoxGCNZ.Name = "chkBoxGCNZ";
            chkBoxGCNZ.Size = new System.Drawing.Size(33, 17);
            chkBoxGCNZ.TabIndex = 16;
            chkBoxGCNZ.Text = "Z";
            chkBoxGCNZ.UseVisualStyleBackColor = true;
            chkBoxGCNZ.CheckedChanged += new EventHandler(chkBoxGCNZ_CheckedChanged);
            // 
            // chkBoxGCNR
            // 
            chkBoxGCNR.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            chkBoxGCNR.Location = new System.Drawing.Point(429, 10);
            chkBoxGCNR.Name = "chkBoxGCNR";
            chkBoxGCNR.Size = new System.Drawing.Size(34, 17);
            chkBoxGCNR.TabIndex = 15;
            chkBoxGCNR.Text = "R";
            chkBoxGCNR.UseVisualStyleBackColor = true;
            chkBoxGCNR.CheckedChanged += new EventHandler(chkBoxGCNR_CheckedChanged);
            // 
            // chkBoxGCNL
            // 
            chkBoxGCNL.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            chkBoxGCNL.Location = new System.Drawing.Point(12, 10);
            chkBoxGCNL.Name = "chkBoxGCNL";
            chkBoxGCNL.Size = new System.Drawing.Size(32, 17);
            chkBoxGCNL.TabIndex = 14;
            chkBoxGCNL.Text = "L";
            chkBoxGCNL.UseVisualStyleBackColor = true;
            chkBoxGCNL.CheckedChanged += new EventHandler(chkBoxGCNL_CheckedChanged);
            // 
            // chkBoxGCNA
            // 
            chkBoxGCNA.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Right);
            chkBoxGCNA.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            chkBoxGCNA.Location = new System.Drawing.Point(406, 238);
            chkBoxGCNA.Name = "chkBoxGCNA";
            chkBoxGCNA.Size = new System.Drawing.Size(18, 43);
            chkBoxGCNA.TabIndex = 24;
            chkBoxGCNA.Text = "\r\n\r\nA";
            chkBoxGCNA.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            chkBoxGCNA.UseVisualStyleBackColor = true;
            chkBoxGCNA.CheckedChanged += new EventHandler(chkBoxGCNA_CheckedChanged);
            // 
            // chkBoxGCNStart
            // 
            chkBoxGCNStart.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Right);
            chkBoxGCNStart.AutoSize = true;
            chkBoxGCNStart.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            chkBoxGCNStart.Location = new System.Drawing.Point(336, 199);
            chkBoxGCNStart.Name = "chkBoxGCNStart";
            chkBoxGCNStart.Size = new System.Drawing.Size(33, 43);
            chkBoxGCNStart.TabIndex = 17;
            chkBoxGCNStart.Text = "Start\r\n\r\n\r\n";
            chkBoxGCNStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            chkBoxGCNStart.UseVisualStyleBackColor = true;
            chkBoxGCNStart.CheckedChanged += new EventHandler(chkBoxGCNStart_CheckedChanged);
            // 
            // ASLIndicator
            // 
            Controls.Add(grpBoxUnusedGCN);
            Controls.Add(chkBoxGCNB);
            Controls.Add(chkBoxGCNX);
            Controls.Add(chkBoxGCNY);
            Controls.Add(chkBoxGCNDown);
            Controls.Add(chkBoxGCNRight);
            Controls.Add(chkBoxGCNUp);
            Controls.Add(chkBoxGCNLeft);
            Controls.Add(chkBoxGCNZ);
            Controls.Add(chkBoxGCNR);
            Controls.Add(chkBoxGCNL);
            Controls.Add(chkBoxGCNA);
            Controls.Add(chkBoxGCNStart);
            Controls.Add(splitter1);
            Name = "ASLIndicator";
            Size = new System.Drawing.Size(479, 305);
            grpBoxUnusedGCN.ResumeLayout(false);
            grpBoxUnusedGCN.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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

        public unsafe void TargetChanged()
        {
            _updating = true;

            // Gamecube buttons
            chkBoxGCNLeft.Checked =
                ((ASLSEntryNode.GameCubeButtons) _targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.Left) != 0;
            chkBoxGCNRight.Checked = ((ASLSEntryNode.GameCubeButtons) _targetNode.ButtonFlags &
                                      ASLSEntryNode.GameCubeButtons.Right) != 0;
            chkBoxGCNUp.Checked =
                ((ASLSEntryNode.GameCubeButtons) _targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.Up) != 0;
            chkBoxGCNDown.Checked =
                ((ASLSEntryNode.GameCubeButtons) _targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.Down) != 0;
            chkBoxGCNA.Checked =
                ((ASLSEntryNode.GameCubeButtons) _targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.A) != 0;
            chkBoxGCNB.Checked =
                ((ASLSEntryNode.GameCubeButtons) _targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.B) != 0;
            chkBoxGCNX.Checked =
                ((ASLSEntryNode.GameCubeButtons) _targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.X) != 0;
            chkBoxGCNY.Checked =
                ((ASLSEntryNode.GameCubeButtons) _targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.Y) != 0;
            chkBoxGCNL.Checked =
                ((ASLSEntryNode.GameCubeButtons) _targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.L) != 0;
            chkBoxGCNR.Checked =
                ((ASLSEntryNode.GameCubeButtons) _targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.R) != 0;
            chkBoxGCNZ.Checked =
                ((ASLSEntryNode.GameCubeButtons) _targetNode.ButtonFlags & ASLSEntryNode.GameCubeButtons.Z) != 0;
            chkBoxGCNStart.Checked = ((ASLSEntryNode.GameCubeButtons) _targetNode.ButtonFlags &
                                      ASLSEntryNode.GameCubeButtons.Start) != 0;
            chkBoxGCN0x0080.Checked = ((ASLSEntryNode.GameCubeButtons) _targetNode.ButtonFlags &
                                       ASLSEntryNode.GameCubeButtons.Unused0x0080) != 0;
            chkBoxGCN0x2000.Checked = ((ASLSEntryNode.GameCubeButtons) _targetNode.ButtonFlags &
                                       ASLSEntryNode.GameCubeButtons.Unused0x2000) != 0;
            chkBoxGCN0x4000.Checked = ((ASLSEntryNode.GameCubeButtons) _targetNode.ButtonFlags &
                                       ASLSEntryNode.GameCubeButtons.Unused0x4000) != 0;
            chkBoxGCN0x8000.Checked = ((ASLSEntryNode.GameCubeButtons) _targetNode.ButtonFlags &
                                       ASLSEntryNode.GameCubeButtons.Unused0x8000) != 0;

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

            TargetNode.ButtonFlags = (ushort) (((ASLSEntryNode.GameCubeButtons) TargetNode.ButtonFlags &
                                                ~ASLSEntryNode.GameCubeButtons.L) |
                                               (chkBoxGCNL.Checked ? ASLSEntryNode.GameCubeButtons.L : 0));
        }

        private void chkBoxGCNR_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.ButtonFlags = (ushort) (((ASLSEntryNode.GameCubeButtons) TargetNode.ButtonFlags &
                                                ~ASLSEntryNode.GameCubeButtons.R) |
                                               (chkBoxGCNR.Checked ? ASLSEntryNode.GameCubeButtons.R : 0));
        }

        private void chkBoxGCNZ_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.ButtonFlags = (ushort) (((ASLSEntryNode.GameCubeButtons) TargetNode.ButtonFlags &
                                                ~ASLSEntryNode.GameCubeButtons.Z) |
                                               (chkBoxGCNZ.Checked ? ASLSEntryNode.GameCubeButtons.Z : 0));
        }

        private void chkBoxGCNStart_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.ButtonFlags = (ushort) (((ASLSEntryNode.GameCubeButtons) TargetNode.ButtonFlags &
                                                ~ASLSEntryNode.GameCubeButtons.Start) |
                                               (chkBoxGCNStart.Checked ? ASLSEntryNode.GameCubeButtons.Start : 0));
        }

        private void chkBoxGCNUp_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.ButtonFlags = (ushort) (((ASLSEntryNode.GameCubeButtons) TargetNode.ButtonFlags &
                                                ~ASLSEntryNode.GameCubeButtons.Up) |
                                               (chkBoxGCNUp.Checked ? ASLSEntryNode.GameCubeButtons.Up : 0));
        }

        private void chkBoxGCNDown_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.ButtonFlags = (ushort) (((ASLSEntryNode.GameCubeButtons) TargetNode.ButtonFlags &
                                                ~ASLSEntryNode.GameCubeButtons.Down) |
                                               (chkBoxGCNDown.Checked ? ASLSEntryNode.GameCubeButtons.Down : 0));
        }

        private void chkBoxGCNLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.ButtonFlags = (ushort) (((ASLSEntryNode.GameCubeButtons) TargetNode.ButtonFlags &
                                                ~ASLSEntryNode.GameCubeButtons.Left) |
                                               (chkBoxGCNLeft.Checked ? ASLSEntryNode.GameCubeButtons.Left : 0));
        }

        private void chkBoxGCNRight_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.ButtonFlags = (ushort) (((ASLSEntryNode.GameCubeButtons) TargetNode.ButtonFlags &
                                                ~ASLSEntryNode.GameCubeButtons.Right) |
                                               (chkBoxGCNRight.Checked ? ASLSEntryNode.GameCubeButtons.Right : 0));
        }

        private void chkBoxGCNA_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.ButtonFlags = (ushort) (((ASLSEntryNode.GameCubeButtons) TargetNode.ButtonFlags &
                                                ~ASLSEntryNode.GameCubeButtons.A) |
                                               (chkBoxGCNA.Checked ? ASLSEntryNode.GameCubeButtons.A : 0));
        }

        private void chkBoxGCNB_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.ButtonFlags = (ushort) (((ASLSEntryNode.GameCubeButtons) TargetNode.ButtonFlags &
                                                ~ASLSEntryNode.GameCubeButtons.B) |
                                               (chkBoxGCNB.Checked ? ASLSEntryNode.GameCubeButtons.B : 0));
        }

        private void chkBoxGCNX_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.ButtonFlags = (ushort) (((ASLSEntryNode.GameCubeButtons) TargetNode.ButtonFlags &
                                                ~ASLSEntryNode.GameCubeButtons.X) |
                                               (chkBoxGCNX.Checked ? ASLSEntryNode.GameCubeButtons.X : 0));
        }

        private void chkBoxGCNY_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.ButtonFlags = (ushort) (((ASLSEntryNode.GameCubeButtons) TargetNode.ButtonFlags &
                                                ~ASLSEntryNode.GameCubeButtons.Y) |
                                               (chkBoxGCNY.Checked ? ASLSEntryNode.GameCubeButtons.Y : 0));
        }

        private void chkBoxGCN0x0080_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.ButtonFlags = (ushort) (((ASLSEntryNode.GameCubeButtons) TargetNode.ButtonFlags &
                                                ~ASLSEntryNode.GameCubeButtons.Unused0x0080) |
                                               (chkBoxGCN0x0080.Checked
                                                   ? ASLSEntryNode.GameCubeButtons.Unused0x0080
                                                   : 0));
        }

        private void chkBoxGCN0x2000_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.ButtonFlags = (ushort) (((ASLSEntryNode.GameCubeButtons) TargetNode.ButtonFlags &
                                                ~ASLSEntryNode.GameCubeButtons.Unused0x2000) |
                                               (chkBoxGCN0x2000.Checked
                                                   ? ASLSEntryNode.GameCubeButtons.Unused0x2000
                                                   : 0));
        }

        private void chkBoxGCN0x4000_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.ButtonFlags = (ushort) (((ASLSEntryNode.GameCubeButtons) TargetNode.ButtonFlags &
                                                ~ASLSEntryNode.GameCubeButtons.Unused0x4000) |
                                               (chkBoxGCN0x4000.Checked
                                                   ? ASLSEntryNode.GameCubeButtons.Unused0x4000
                                                   : 0));
        }

        private void chkBoxGCN0x8000_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.ButtonFlags = (ushort) (((ASLSEntryNode.GameCubeButtons) TargetNode.ButtonFlags &
                                                ~ASLSEntryNode.GameCubeButtons.Unused0x8000) |
                                               (chkBoxGCN0x8000.Checked
                                                   ? ASLSEntryNode.GameCubeButtons.Unused0x8000
                                                   : 0));
        }
    }
}