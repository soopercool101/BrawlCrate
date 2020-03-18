using BrawlLib.Imaging;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.ResourceNodes.ProjectPlus;
using BrawlLib.SSBB.Types;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public unsafe class ASLIndicator : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.controllerTabs = new System.Windows.Forms.TabControl();
            this.tabGameCube = new System.Windows.Forms.TabPage();
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
            this.tabWiimote = new System.Windows.Forms.TabPage();
            this.tabClassic = new System.Windows.Forms.TabPage();
            this.chkBoxClassicB = new System.Windows.Forms.CheckBox();
            this.chkBoxClassicA = new System.Windows.Forms.CheckBox();
            this.chkBoxClassicX = new System.Windows.Forms.CheckBox();
            this.chkBoxClassicY = new System.Windows.Forms.CheckBox();
            this.chkBoxClassicZL = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkBoxClassic0x0800 = new System.Windows.Forms.CheckBox();
            this.chkBoxClassic0x0100 = new System.Windows.Forms.CheckBox();
            this.chkBoxClassicDown = new System.Windows.Forms.CheckBox();
            this.chkBoxClassicRight = new System.Windows.Forms.CheckBox();
            this.chkBoxClassicUp = new System.Windows.Forms.CheckBox();
            this.chkBoxClassicLeft = new System.Windows.Forms.CheckBox();
            this.chkBoxClassicZR = new System.Windows.Forms.CheckBox();
            this.chkBoxClassicR = new System.Windows.Forms.CheckBox();
            this.chkBoxClassicL = new System.Windows.Forms.CheckBox();
            this.chkBoxClassicPlus = new System.Windows.Forms.CheckBox();
            this.chkBoxClassicMinus = new System.Windows.Forms.CheckBox();
            this.controllerTabs.SuspendLayout();
            this.tabGameCube.SuspendLayout();
            this.grpBoxUnusedGCN.SuspendLayout();
            this.tabClassic.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            // controllerTabs
            // 
            this.controllerTabs.Controls.Add(this.tabGameCube);
            this.controllerTabs.Controls.Add(this.tabWiimote);
            this.controllerTabs.Controls.Add(this.tabClassic);
            this.controllerTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controllerTabs.Location = new System.Drawing.Point(0, 0);
            this.controllerTabs.Name = "controllerTabs";
            this.controllerTabs.SelectedIndex = 0;
            this.controllerTabs.Size = new System.Drawing.Size(479, 302);
            this.controllerTabs.TabIndex = 8;
            // 
            // tabGameCube
            // 
            this.tabGameCube.BackColor = System.Drawing.SystemColors.Control;
            this.tabGameCube.Controls.Add(this.grpBoxUnusedGCN);
            this.tabGameCube.Controls.Add(this.chkBoxGCNB);
            this.tabGameCube.Controls.Add(this.chkBoxGCNX);
            this.tabGameCube.Controls.Add(this.chkBoxGCNY);
            this.tabGameCube.Controls.Add(this.chkBoxGCNDown);
            this.tabGameCube.Controls.Add(this.chkBoxGCNRight);
            this.tabGameCube.Controls.Add(this.chkBoxGCNUp);
            this.tabGameCube.Controls.Add(this.chkBoxGCNLeft);
            this.tabGameCube.Controls.Add(this.chkBoxGCNZ);
            this.tabGameCube.Controls.Add(this.chkBoxGCNR);
            this.tabGameCube.Controls.Add(this.chkBoxGCNL);
            this.tabGameCube.Controls.Add(this.chkBoxGCNA);
            this.tabGameCube.Controls.Add(this.chkBoxGCNStart);
            this.tabGameCube.Location = new System.Drawing.Point(4, 22);
            this.tabGameCube.Name = "tabGameCube";
            this.tabGameCube.Padding = new System.Windows.Forms.Padding(3);
            this.tabGameCube.Size = new System.Drawing.Size(471, 276);
            this.tabGameCube.TabIndex = 0;
            this.tabGameCube.Text = "GameCube";
            // 
            // grpBoxUnusedGCN
            // 
            this.grpBoxUnusedGCN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxUnusedGCN.Controls.Add(this.chkBoxGCN0x8000);
            this.grpBoxUnusedGCN.Controls.Add(this.chkBoxGCN0x4000);
            this.grpBoxUnusedGCN.Controls.Add(this.chkBoxGCN0x2000);
            this.grpBoxUnusedGCN.Controls.Add(this.chkBoxGCN0x0080);
            this.grpBoxUnusedGCN.Location = new System.Drawing.Point(169, 162);
            this.grpBoxUnusedGCN.Name = "grpBoxUnusedGCN";
            this.grpBoxUnusedGCN.Size = new System.Drawing.Size(139, 111);
            this.grpBoxUnusedGCN.TabIndex = 13;
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
            // 
            // chkBoxGCNB
            // 
            this.chkBoxGCNB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxGCNB.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxGCNB.Location = new System.Drawing.Point(377, 224);
            this.chkBoxGCNB.Name = "chkBoxGCNB";
            this.chkBoxGCNB.Size = new System.Drawing.Size(18, 43);
            this.chkBoxGCNB.TabIndex = 12;
            this.chkBoxGCNB.Text = "\r\n\r\nB";
            this.chkBoxGCNB.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkBoxGCNB.UseVisualStyleBackColor = true;
            // 
            // chkBoxGCNX
            // 
            this.chkBoxGCNX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxGCNX.Location = new System.Drawing.Point(433, 222);
            this.chkBoxGCNX.Name = "chkBoxGCNX";
            this.chkBoxGCNX.Size = new System.Drawing.Size(33, 17);
            this.chkBoxGCNX.TabIndex = 10;
            this.chkBoxGCNX.Text = "X";
            this.chkBoxGCNX.UseVisualStyleBackColor = true;
            // 
            // chkBoxGCNY
            // 
            this.chkBoxGCNY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxGCNY.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxGCNY.Location = new System.Drawing.Point(406, 186);
            this.chkBoxGCNY.Name = "chkBoxGCNY";
            this.chkBoxGCNY.Size = new System.Drawing.Size(18, 43);
            this.chkBoxGCNY.TabIndex = 9;
            this.chkBoxGCNY.Text = "Y\r\n\r\n\r\n";
            this.chkBoxGCNY.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkBoxGCNY.UseVisualStyleBackColor = true;
            // 
            // chkBoxGCNDown
            // 
            this.chkBoxGCNDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxGCNDown.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxGCNDown.Location = new System.Drawing.Point(49, 229);
            this.chkBoxGCNDown.Name = "chkBoxGCNDown";
            this.chkBoxGCNDown.Size = new System.Drawing.Size(17, 43);
            this.chkBoxGCNDown.TabIndex = 7;
            this.chkBoxGCNDown.Text = "\r\n\r\n↓";
            this.chkBoxGCNDown.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkBoxGCNDown.UseVisualStyleBackColor = true;
            // 
            // chkBoxGCNRight
            // 
            this.chkBoxGCNRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxGCNRight.Location = new System.Drawing.Point(71, 222);
            this.chkBoxGCNRight.Name = "chkBoxGCNRight";
            this.chkBoxGCNRight.Size = new System.Drawing.Size(37, 17);
            this.chkBoxGCNRight.TabIndex = 6;
            this.chkBoxGCNRight.Text = "→";
            this.chkBoxGCNRight.UseVisualStyleBackColor = true;
            // 
            // chkBoxGCNUp
            // 
            this.chkBoxGCNUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxGCNUp.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxGCNUp.Location = new System.Drawing.Point(49, 189);
            this.chkBoxGCNUp.Name = "chkBoxGCNUp";
            this.chkBoxGCNUp.Size = new System.Drawing.Size(17, 43);
            this.chkBoxGCNUp.TabIndex = 5;
            this.chkBoxGCNUp.Text = "↑\r\n\r\n\r\n";
            this.chkBoxGCNUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkBoxGCNUp.UseVisualStyleBackColor = true;
            // 
            // chkBoxGCNLeft
            // 
            this.chkBoxGCNLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxGCNLeft.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBoxGCNLeft.Location = new System.Drawing.Point(6, 222);
            this.chkBoxGCNLeft.Name = "chkBoxGCNLeft";
            this.chkBoxGCNLeft.Size = new System.Drawing.Size(37, 17);
            this.chkBoxGCNLeft.TabIndex = 4;
            this.chkBoxGCNLeft.Text = "←";
            this.chkBoxGCNLeft.UseVisualStyleBackColor = true;
            // 
            // chkBoxGCNZ
            // 
            this.chkBoxGCNZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxGCNZ.Location = new System.Drawing.Point(426, 29);
            this.chkBoxGCNZ.Name = "chkBoxGCNZ";
            this.chkBoxGCNZ.Size = new System.Drawing.Size(33, 17);
            this.chkBoxGCNZ.TabIndex = 2;
            this.chkBoxGCNZ.Text = "Z";
            this.chkBoxGCNZ.UseVisualStyleBackColor = true;
            // 
            // chkBoxGCNR
            // 
            this.chkBoxGCNR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxGCNR.Location = new System.Drawing.Point(426, 6);
            this.chkBoxGCNR.Name = "chkBoxGCNR";
            this.chkBoxGCNR.Size = new System.Drawing.Size(34, 17);
            this.chkBoxGCNR.TabIndex = 1;
            this.chkBoxGCNR.Text = "R";
            this.chkBoxGCNR.UseVisualStyleBackColor = true;
            this.chkBoxGCNR.CheckedChanged += new System.EventHandler(this.chkBoxGCNR_CheckedChanged);
            // 
            // chkBoxGCNL
            // 
            this.chkBoxGCNL.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBoxGCNL.Location = new System.Drawing.Point(9, 6);
            this.chkBoxGCNL.Name = "chkBoxGCNL";
            this.chkBoxGCNL.Size = new System.Drawing.Size(32, 17);
            this.chkBoxGCNL.TabIndex = 0;
            this.chkBoxGCNL.Text = "L";
            this.chkBoxGCNL.UseVisualStyleBackColor = true;
            this.chkBoxGCNL.CheckedChanged += new System.EventHandler(this.chkBoxGCNL_CheckedChanged);
            // 
            // chkBoxGCNA
            // 
            this.chkBoxGCNA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxGCNA.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxGCNA.Location = new System.Drawing.Point(403, 215);
            this.chkBoxGCNA.Name = "chkBoxGCNA";
            this.chkBoxGCNA.Size = new System.Drawing.Size(18, 43);
            this.chkBoxGCNA.TabIndex = 11;
            this.chkBoxGCNA.Text = "\r\n\r\nA";
            this.chkBoxGCNA.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkBoxGCNA.UseVisualStyleBackColor = true;
            // 
            // chkBoxGCNStart
            // 
            this.chkBoxGCNStart.AutoSize = true;
            this.chkBoxGCNStart.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxGCNStart.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkBoxGCNStart.Location = new System.Drawing.Point(3, 3);
            this.chkBoxGCNStart.Name = "chkBoxGCNStart";
            this.chkBoxGCNStart.Size = new System.Drawing.Size(465, 43);
            this.chkBoxGCNStart.TabIndex = 3;
            this.chkBoxGCNStart.Text = "Start\r\n\r\n\r\n";
            this.chkBoxGCNStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxGCNStart.UseVisualStyleBackColor = true;
            // 
            // tabWiimote
            // 
            this.tabWiimote.Location = new System.Drawing.Point(4, 22);
            this.tabWiimote.Name = "tabWiimote";
            this.tabWiimote.Padding = new System.Windows.Forms.Padding(3);
            this.tabWiimote.Size = new System.Drawing.Size(471, 276);
            this.tabWiimote.TabIndex = 1;
            this.tabWiimote.Text = "Wii Remote/Nunchuk";
            this.tabWiimote.UseVisualStyleBackColor = true;
            // 
            // tabClassic
            // 
            this.tabClassic.BackColor = System.Drawing.SystemColors.Control;
            this.tabClassic.Controls.Add(this.chkBoxClassicMinus);
            this.tabClassic.Controls.Add(this.chkBoxClassicPlus);
            this.tabClassic.Controls.Add(this.chkBoxClassicB);
            this.tabClassic.Controls.Add(this.chkBoxClassicA);
            this.tabClassic.Controls.Add(this.chkBoxClassicX);
            this.tabClassic.Controls.Add(this.chkBoxClassicY);
            this.tabClassic.Controls.Add(this.chkBoxClassicZL);
            this.tabClassic.Controls.Add(this.groupBox1);
            this.tabClassic.Controls.Add(this.chkBoxClassicDown);
            this.tabClassic.Controls.Add(this.chkBoxClassicRight);
            this.tabClassic.Controls.Add(this.chkBoxClassicUp);
            this.tabClassic.Controls.Add(this.chkBoxClassicLeft);
            this.tabClassic.Controls.Add(this.chkBoxClassicZR);
            this.tabClassic.Controls.Add(this.chkBoxClassicR);
            this.tabClassic.Controls.Add(this.chkBoxClassicL);
            this.tabClassic.Location = new System.Drawing.Point(4, 22);
            this.tabClassic.Name = "tabClassic";
            this.tabClassic.Padding = new System.Windows.Forms.Padding(3);
            this.tabClassic.Size = new System.Drawing.Size(471, 276);
            this.tabClassic.TabIndex = 2;
            this.tabClassic.Text = "Classic Controller";
            // 
            // chkBoxClassicB
            // 
            this.chkBoxClassicB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxClassicB.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxClassicB.Location = new System.Drawing.Point(406, 229);
            this.chkBoxClassicB.Name = "chkBoxClassicB";
            this.chkBoxClassicB.Size = new System.Drawing.Size(18, 43);
            this.chkBoxClassicB.TabIndex = 31;
            this.chkBoxClassicB.Text = "\r\n\r\nB";
            this.chkBoxClassicB.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkBoxClassicB.UseVisualStyleBackColor = true;
            // 
            // chkBoxClassicA
            // 
            this.chkBoxClassicA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxClassicA.Location = new System.Drawing.Point(431, 222);
            this.chkBoxClassicA.Name = "chkBoxClassicA";
            this.chkBoxClassicA.Size = new System.Drawing.Size(33, 17);
            this.chkBoxClassicA.TabIndex = 30;
            this.chkBoxClassicA.Text = "A";
            this.chkBoxClassicA.UseVisualStyleBackColor = true;
            // 
            // chkBoxClassicX
            // 
            this.chkBoxClassicX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxClassicX.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxClassicX.Location = new System.Drawing.Point(406, 189);
            this.chkBoxClassicX.Name = "chkBoxClassicX";
            this.chkBoxClassicX.Size = new System.Drawing.Size(18, 43);
            this.chkBoxClassicX.TabIndex = 29;
            this.chkBoxClassicX.Text = "X\r\n\r\n\r\n";
            this.chkBoxClassicX.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkBoxClassicX.UseVisualStyleBackColor = true;
            // 
            // chkBoxClassicY
            // 
            this.chkBoxClassicY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxClassicY.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBoxClassicY.Location = new System.Drawing.Point(366, 222);
            this.chkBoxClassicY.Name = "chkBoxClassicY";
            this.chkBoxClassicY.Size = new System.Drawing.Size(33, 17);
            this.chkBoxClassicY.TabIndex = 28;
            this.chkBoxClassicY.Text = "Y";
            this.chkBoxClassicY.UseVisualStyleBackColor = true;
            // 
            // chkBoxClassicZL
            // 
            this.chkBoxClassicZL.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBoxClassicZL.Location = new System.Drawing.Point(2, 29);
            this.chkBoxClassicZL.Name = "chkBoxClassicZL";
            this.chkBoxClassicZL.Size = new System.Drawing.Size(39, 17);
            this.chkBoxClassicZL.TabIndex = 27;
            this.chkBoxClassicZL.Text = "ZL";
            this.chkBoxClassicZL.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkBoxClassic0x0800);
            this.groupBox1.Controls.Add(this.chkBoxClassic0x0100);
            this.groupBox1.Location = new System.Drawing.Point(169, 204);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(139, 69);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Unused";
            // 
            // chkBoxClassic0x0800
            // 
            this.chkBoxClassic0x0800.AutoSize = true;
            this.chkBoxClassic0x0800.Location = new System.Drawing.Point(6, 42);
            this.chkBoxClassic0x0800.Name = "chkBoxClassic0x0800";
            this.chkBoxClassic0x0800.Size = new System.Drawing.Size(61, 17);
            this.chkBoxClassic0x0800.TabIndex = 1;
            this.chkBoxClassic0x0800.Text = "0x0800";
            this.chkBoxClassic0x0800.UseVisualStyleBackColor = true;
            // 
            // chkBoxClassic0x0100
            // 
            this.chkBoxClassic0x0100.AutoSize = true;
            this.chkBoxClassic0x0100.Location = new System.Drawing.Point(6, 19);
            this.chkBoxClassic0x0100.Name = "chkBoxClassic0x0100";
            this.chkBoxClassic0x0100.Size = new System.Drawing.Size(61, 17);
            this.chkBoxClassic0x0100.TabIndex = 0;
            this.chkBoxClassic0x0100.Text = "0x0100";
            this.chkBoxClassic0x0100.UseVisualStyleBackColor = true;
            // 
            // chkBoxClassicDown
            // 
            this.chkBoxClassicDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxClassicDown.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxClassicDown.Location = new System.Drawing.Point(49, 229);
            this.chkBoxClassicDown.Name = "chkBoxClassicDown";
            this.chkBoxClassicDown.Size = new System.Drawing.Size(17, 43);
            this.chkBoxClassicDown.TabIndex = 21;
            this.chkBoxClassicDown.Text = "\r\n\r\n↓";
            this.chkBoxClassicDown.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkBoxClassicDown.UseVisualStyleBackColor = true;
            // 
            // chkBoxClassicRight
            // 
            this.chkBoxClassicRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxClassicRight.Location = new System.Drawing.Point(71, 222);
            this.chkBoxClassicRight.Name = "chkBoxClassicRight";
            this.chkBoxClassicRight.Size = new System.Drawing.Size(37, 17);
            this.chkBoxClassicRight.TabIndex = 20;
            this.chkBoxClassicRight.Text = "→";
            this.chkBoxClassicRight.UseVisualStyleBackColor = true;
            // 
            // chkBoxClassicUp
            // 
            this.chkBoxClassicUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxClassicUp.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxClassicUp.Location = new System.Drawing.Point(49, 189);
            this.chkBoxClassicUp.Name = "chkBoxClassicUp";
            this.chkBoxClassicUp.Size = new System.Drawing.Size(17, 43);
            this.chkBoxClassicUp.TabIndex = 19;
            this.chkBoxClassicUp.Text = "↑\r\n\r\n\r\n";
            this.chkBoxClassicUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkBoxClassicUp.UseVisualStyleBackColor = true;
            // 
            // chkBoxClassicLeft
            // 
            this.chkBoxClassicLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxClassicLeft.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBoxClassicLeft.Location = new System.Drawing.Point(6, 222);
            this.chkBoxClassicLeft.Name = "chkBoxClassicLeft";
            this.chkBoxClassicLeft.Size = new System.Drawing.Size(37, 17);
            this.chkBoxClassicLeft.TabIndex = 18;
            this.chkBoxClassicLeft.Text = "←";
            this.chkBoxClassicLeft.UseVisualStyleBackColor = true;
            // 
            // chkBoxClassicZR
            // 
            this.chkBoxClassicZR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxClassicZR.Location = new System.Drawing.Point(426, 29);
            this.chkBoxClassicZR.Name = "chkBoxClassicZR";
            this.chkBoxClassicZR.Size = new System.Drawing.Size(41, 17);
            this.chkBoxClassicZR.TabIndex = 16;
            this.chkBoxClassicZR.Text = "ZR";
            this.chkBoxClassicZR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxClassicZR.UseVisualStyleBackColor = true;
            // 
            // chkBoxClassicR
            // 
            this.chkBoxClassicR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxClassicR.Location = new System.Drawing.Point(426, 6);
            this.chkBoxClassicR.Name = "chkBoxClassicR";
            this.chkBoxClassicR.Size = new System.Drawing.Size(34, 17);
            this.chkBoxClassicR.TabIndex = 15;
            this.chkBoxClassicR.Text = "R";
            this.chkBoxClassicR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxClassicR.UseVisualStyleBackColor = true;
            // 
            // chkBoxClassicL
            // 
            this.chkBoxClassicL.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBoxClassicL.Location = new System.Drawing.Point(9, 6);
            this.chkBoxClassicL.Name = "chkBoxClassicL";
            this.chkBoxClassicL.Size = new System.Drawing.Size(32, 17);
            this.chkBoxClassicL.TabIndex = 14;
            this.chkBoxClassicL.Text = "L";
            this.chkBoxClassicL.UseVisualStyleBackColor = true;
            // 
            // chkBoxClassicPlus
            // 
            this.chkBoxClassicPlus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxClassicPlus.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBoxClassicPlus.Location = new System.Drawing.Point(366, 16);
            this.chkBoxClassicPlus.Name = "chkBoxClassicPlus";
            this.chkBoxClassicPlus.Size = new System.Drawing.Size(34, 17);
            this.chkBoxClassicPlus.TabIndex = 32;
            this.chkBoxClassicPlus.Text = "+";
            this.chkBoxClassicPlus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxClassicPlus.UseVisualStyleBackColor = true;
            // 
            // chkBoxClassicMinus
            // 
            this.chkBoxClassicMinus.Location = new System.Drawing.Point(76, 16);
            this.chkBoxClassicMinus.Name = "chkBoxClassicMinus";
            this.chkBoxClassicMinus.Size = new System.Drawing.Size(32, 17);
            this.chkBoxClassicMinus.TabIndex = 33;
            this.chkBoxClassicMinus.Text = "-";
            this.chkBoxClassicMinus.UseVisualStyleBackColor = true;
            // 
            // ASLIndicator
            // 
            this.Controls.Add(this.controllerTabs);
            this.Controls.Add(this.splitter1);
            this.Name = "ASLIndicator";
            this.Size = new System.Drawing.Size(479, 305);
            this.controllerTabs.ResumeLayout(false);
            this.tabGameCube.ResumeLayout(false);
            this.tabGameCube.PerformLayout();
            this.grpBoxUnusedGCN.ResumeLayout(false);
            this.grpBoxUnusedGCN.PerformLayout();
            this.tabClassic.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public ASLIndicator()
        {
            InitializeComponent();
        }

        private Splitter splitter1;
        public bool called;

        private ASLSEntryNode _targetNode;
        private TabControl controllerTabs;
        private TabPage tabGameCube;
        private TabPage tabWiimote;
        private TabPage tabClassic;
        private CheckBox chkBoxGCNStart;
        private CheckBox chkBoxGCNR;
        private CheckBox chkBoxGCNL;
        private CheckBox chkBoxGCNX;
        private CheckBox chkBoxGCNY;
        private CheckBox chkBoxGCNDown;
        private CheckBox chkBoxGCNRight;
        private CheckBox chkBoxGCNUp;
        private CheckBox chkBoxGCNLeft;
        private CheckBox chkBoxGCNZ;
        private CheckBox chkBoxGCNA;
        private CheckBox chkBoxGCNB;
        private GroupBox grpBoxUnusedGCN;
        private CheckBox chkBoxGCN0x4000;
        private CheckBox chkBoxGCN0x2000;
        private CheckBox chkBoxGCN0x0080;
        private CheckBox chkBoxGCN0x8000;
        private CheckBox chkBoxClassicZL;
        private GroupBox groupBox1;
        private CheckBox chkBoxClassic0x0800;
        private CheckBox chkBoxClassic0x0100;
        private CheckBox chkBoxClassicDown;
        private CheckBox chkBoxClassicRight;
        private CheckBox chkBoxClassicUp;
        private CheckBox chkBoxClassicLeft;
        private CheckBox chkBoxClassicZR;
        private CheckBox chkBoxClassicR;
        private CheckBox chkBoxClassicL;
        private CheckBox chkBoxClassicB;
        private CheckBox chkBoxClassicA;
        private CheckBox chkBoxClassicX;
        private CheckBox chkBoxClassicY;
        private CheckBox chkBoxClassicMinus;
        private CheckBox chkBoxClassicPlus;
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
            // Wii Remote + Nunchuk

            // Classic Controller
            chkBoxClassicLeft.Checked = ((ASLSEntryNode.ClassicButtons)_targetNode.ButtonFlags & ASLSEntryNode.ClassicButtons.Left) != 0;
            chkBoxClassicRight.Checked = ((ASLSEntryNode.ClassicButtons)_targetNode.ButtonFlags & ASLSEntryNode.ClassicButtons.Right) != 0;
            chkBoxClassicUp.Checked = ((ASLSEntryNode.ClassicButtons)_targetNode.ButtonFlags & ASLSEntryNode.ClassicButtons.Up) != 0;
            chkBoxClassicDown.Checked = ((ASLSEntryNode.ClassicButtons)_targetNode.ButtonFlags & ASLSEntryNode.ClassicButtons.Down) != 0;
            chkBoxClassicA.Checked = ((ASLSEntryNode.ClassicButtons)_targetNode.ButtonFlags & ASLSEntryNode.ClassicButtons.A) != 0;
            chkBoxClassicB.Checked = ((ASLSEntryNode.ClassicButtons)_targetNode.ButtonFlags & ASLSEntryNode.ClassicButtons.B) != 0;
            chkBoxClassicX.Checked = ((ASLSEntryNode.ClassicButtons)_targetNode.ButtonFlags & ASLSEntryNode.ClassicButtons.X) != 0;
            chkBoxClassicY.Checked = ((ASLSEntryNode.ClassicButtons)_targetNode.ButtonFlags & ASLSEntryNode.ClassicButtons.Y) != 0;
            chkBoxClassicL.Checked = ((ASLSEntryNode.ClassicButtons)_targetNode.ButtonFlags & ASLSEntryNode.ClassicButtons.L) != 0;
            chkBoxClassicZL.Checked = ((ASLSEntryNode.ClassicButtons)_targetNode.ButtonFlags & ASLSEntryNode.ClassicButtons.ZL) != 0;
            chkBoxClassicR.Checked = ((ASLSEntryNode.ClassicButtons)_targetNode.ButtonFlags & ASLSEntryNode.ClassicButtons.R) != 0;
            chkBoxClassicZR.Checked = ((ASLSEntryNode.ClassicButtons)_targetNode.ButtonFlags & ASLSEntryNode.ClassicButtons.ZR) != 0;
            chkBoxClassicPlus.Checked = ((ASLSEntryNode.ClassicButtons)_targetNode.ButtonFlags & ASLSEntryNode.ClassicButtons.Plus) != 0;
            chkBoxClassicMinus.Checked = ((ASLSEntryNode.ClassicButtons)_targetNode.ButtonFlags & ASLSEntryNode.ClassicButtons.Minus) != 0;
            chkBoxClassic0x0100.Checked = ((ASLSEntryNode.ClassicButtons)_targetNode.ButtonFlags & ASLSEntryNode.ClassicButtons.Unused0x0100) != 0;
            chkBoxClassic0x0800.Checked = ((ASLSEntryNode.ClassicButtons)_targetNode.ButtonFlags & ASLSEntryNode.ClassicButtons.Unused0x0800) != 0;

            _updating = false;
        }

        private void chkBoxGCNL_CheckedChanged(object sender, EventArgs e)
        {
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.L) |
                      (chkBoxGCNL.Checked ? ASLSEntryNode.GameCubeButtons.L : 0));
        }

        private void chkBoxGCNR_CheckedChanged(object sender, EventArgs e)
        {
            TargetNode.ButtonFlags = (ushort)(((ASLSEntryNode.GameCubeButtons)TargetNode.ButtonFlags & ~ASLSEntryNode.GameCubeButtons.R) |
                                              (chkBoxGCNR.Checked ? ASLSEntryNode.GameCubeButtons.R : 0));
        }
    }
}