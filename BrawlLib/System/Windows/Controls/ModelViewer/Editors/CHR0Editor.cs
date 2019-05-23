using BrawlLib.Wii.Animations;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Modeling;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using BrawlLib.Wii.Models;

namespace System.Windows.Forms
{
    public class CHR0Editor : UserControl
    {
        #region Designer
        private GroupBox grpTransform;
        public Button btnPaste;
        public Button btnCopy;
        public Button btnCut;
        private Label lblTrans;
        public NumericInputBox numScaleZ;
        public NumericInputBox numTransX;
        public NumericInputBox numScaleY;
        private Label lblRot;
        public NumericInputBox numScaleX;
        private Label lblScale;
        public NumericInputBox numRotZ;
        public NumericInputBox numRotY;
        public NumericInputBox numRotX;
        public NumericInputBox numTransZ;
        public NumericInputBox numTransY;
        private GroupBox grpTransAll;
        private CheckBox AllScale;
        private CheckBox AllRot;
        private CheckBox AllTrans;
        public Button btnClean;
        public Button btnPasteAll;
        public Button btnCopyAll;
        public Button btnClearAll;
        public Button btnInsert;
        public Button btnDelete;
        private CheckBox FrameScale;
        private CheckBox FrameRot;
        private CheckBox FrameTrans;
        private ContextMenuStrip ctxBox;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem Source;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem add;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem7;
        private ToolStripMenuItem subtract;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem removeAllToolStripMenuItem;
        private ContextMenuStrip ctxTools;
        private ToolStripMenuItem bakeVertexPositionsToolStripMenuItem;
        public CheckBox chkMoveBoneOnly;
        public CheckBox chkUpdateBindPose;
        private ToolStripMenuItem addCustomAmountToolStripMenuItem;
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grpTransform = new System.Windows.Forms.GroupBox();
            this.FrameScale = new System.Windows.Forms.CheckBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.chkUpdateBindPose = new System.Windows.Forms.CheckBox();
            this.FrameRot = new System.Windows.Forms.CheckBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.FrameTrans = new System.Windows.Forms.CheckBox();
            this.btnCut = new System.Windows.Forms.Button();
            this.lblTrans = new System.Windows.Forms.Label();
            this.lblRot = new System.Windows.Forms.Label();
            this.lblScale = new System.Windows.Forms.Label();
            this.chkMoveBoneOnly = new System.Windows.Forms.CheckBox();
            this.grpTransAll = new System.Windows.Forms.GroupBox();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnClean = new System.Windows.Forms.Button();
            this.btnPasteAll = new System.Windows.Forms.Button();
            this.btnCopyAll = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.AllScale = new System.Windows.Forms.CheckBox();
            this.AllRot = new System.Windows.Forms.CheckBox();
            this.AllTrans = new System.Windows.Forms.CheckBox();
            this.ctxTools = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bakeVertexPositionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Source = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.add = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.subtract = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCustomAmountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.numScaleX = new System.Windows.Forms.NumericInputBox();
            this.numScaleY = new System.Windows.Forms.NumericInputBox();
            this.numScaleZ = new System.Windows.Forms.NumericInputBox();
            this.numRotX = new System.Windows.Forms.NumericInputBox();
            this.numRotY = new System.Windows.Forms.NumericInputBox();
            this.numRotZ = new System.Windows.Forms.NumericInputBox();
            this.numTransX = new System.Windows.Forms.NumericInputBox();
            this.numTransY = new System.Windows.Forms.NumericInputBox();
            this.numTransZ = new System.Windows.Forms.NumericInputBox();
            this.grpTransform.SuspendLayout();
            this.grpTransAll.SuspendLayout();
            this.ctxTools.SuspendLayout();
            this.ctxBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpTransform
            // 
            this.grpTransform.Controls.Add(this.FrameScale);
            this.grpTransform.Controls.Add(this.btnPaste);
            this.grpTransform.Controls.Add(this.chkUpdateBindPose);
            this.grpTransform.Controls.Add(this.chkMoveBoneOnly);
            this.grpTransform.Controls.Add(this.FrameRot);
            this.grpTransform.Controls.Add(this.btnCopy);
            this.grpTransform.Controls.Add(this.FrameTrans);
            this.grpTransform.Controls.Add(this.btnCut);
            this.grpTransform.Controls.Add(this.numScaleX);
            this.grpTransform.Controls.Add(this.numScaleY);
            this.grpTransform.Controls.Add(this.numScaleZ);
            this.grpTransform.Controls.Add(this.numRotX);
            this.grpTransform.Controls.Add(this.numRotY);
            this.grpTransform.Controls.Add(this.numRotZ);
            this.grpTransform.Controls.Add(this.numTransX);
            this.grpTransform.Controls.Add(this.numTransY);
            this.grpTransform.Controls.Add(this.numTransZ);
            this.grpTransform.Controls.Add(this.lblTrans);
            this.grpTransform.Controls.Add(this.lblRot);
            this.grpTransform.Controls.Add(this.lblScale);
            this.grpTransform.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpTransform.Enabled = false;
            this.grpTransform.Location = new System.Drawing.Point(0, 0);
            this.grpTransform.Name = "grpTransform";
            this.grpTransform.Padding = new System.Windows.Forms.Padding(0);
            this.grpTransform.Size = new System.Drawing.Size(422, 78);
            this.grpTransform.TabIndex = 23;
            this.grpTransform.TabStop = false;
            this.grpTransform.Text = "Transform Frame";
            // 
            // FrameScale
            // 
            this.FrameScale.AutoSize = true;
            this.FrameScale.Checked = true;
            this.FrameScale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FrameScale.Location = new System.Drawing.Point(367, 56);
            this.FrameScale.Name = "FrameScale";
            this.FrameScale.Size = new System.Drawing.Size(53, 17);
            this.FrameScale.TabIndex = 35;
            this.FrameScale.Text = "Scale";
            this.FrameScale.UseVisualStyleBackColor = true;
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(318, 54);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(48, 20);
            this.btnPaste.TabIndex = 23;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // chkUpdateBindPose
            // 
            this.chkUpdateBindPose.Location = new System.Drawing.Point(154, -1);
            this.chkUpdateBindPose.Name = "chkUpdateBindPose";
            this.chkUpdateBindPose.Size = new System.Drawing.Size(146, 17);
            this.chkUpdateBindPose.TabIndex = 37;
            this.chkUpdateBindPose.Text = "Update Mesh Bind Pose";
            this.chkUpdateBindPose.UseVisualStyleBackColor = true;
            this.chkUpdateBindPose.CheckedChanged += new System.EventHandler(this.chkUpdateBindPose_CheckedChanged);
            // 
            // FrameRot
            // 
            this.FrameRot.AutoSize = true;
            this.FrameRot.Checked = true;
            this.FrameRot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FrameRot.Location = new System.Drawing.Point(367, 38);
            this.FrameRot.Name = "FrameRot";
            this.FrameRot.Size = new System.Drawing.Size(43, 17);
            this.FrameRot.TabIndex = 34;
            this.FrameRot.Text = "Rot";
            this.FrameRot.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(318, 35);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(48, 20);
            this.btnCopy.TabIndex = 22;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // FrameTrans
            // 
            this.FrameTrans.AutoSize = true;
            this.FrameTrans.Checked = true;
            this.FrameTrans.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FrameTrans.Location = new System.Drawing.Point(367, 20);
            this.FrameTrans.Name = "FrameTrans";
            this.FrameTrans.Size = new System.Drawing.Size(53, 17);
            this.FrameTrans.TabIndex = 33;
            this.FrameTrans.Text = "Trans";
            this.FrameTrans.UseVisualStyleBackColor = true;
            // 
            // btnCut
            // 
            this.btnCut.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnCut.Location = new System.Drawing.Point(318, 16);
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(48, 20);
            this.btnCut.TabIndex = 21;
            this.btnCut.Text = "Cut";
            this.btnCut.UseVisualStyleBackColor = true;
            this.btnCut.Click += new System.EventHandler(this.btnCut_Click);
            // 
            // lblTrans
            // 
            this.lblTrans.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTrans.Location = new System.Drawing.Point(4, 16);
            this.lblTrans.Name = "lblTrans";
            this.lblTrans.Size = new System.Drawing.Size(70, 20);
            this.lblTrans.TabIndex = 4;
            this.lblTrans.Text = "Translation:";
            this.lblTrans.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRot
            // 
            this.lblRot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRot.Location = new System.Drawing.Point(4, 35);
            this.lblRot.Name = "lblRot";
            this.lblRot.Size = new System.Drawing.Size(70, 20);
            this.lblRot.TabIndex = 5;
            this.lblRot.Text = "Rotation:";
            this.lblRot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblScale
            // 
            this.lblScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblScale.Location = new System.Drawing.Point(4, 54);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(70, 20);
            this.lblScale.TabIndex = 6;
            this.lblScale.Text = "Scale:";
            this.lblScale.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkMoveBoneOnly
            // 
            this.chkMoveBoneOnly.Enabled = false;
            this.chkMoveBoneOnly.Location = new System.Drawing.Point(302, -1);
            this.chkMoveBoneOnly.Name = "chkMoveBoneOnly";
            this.chkMoveBoneOnly.Size = new System.Drawing.Size(113, 17);
            this.chkMoveBoneOnly.TabIndex = 36;
            this.chkMoveBoneOnly.Text = "Move Bone Only";
            this.chkMoveBoneOnly.UseVisualStyleBackColor = true;
            this.chkMoveBoneOnly.CheckedChanged += new System.EventHandler(this.chkBoneEdit_CheckedChanged);
            // 
            // grpTransAll
            // 
            this.grpTransAll.Controls.Add(this.btnInsert);
            this.grpTransAll.Controls.Add(this.btnClean);
            this.grpTransAll.Controls.Add(this.btnPasteAll);
            this.grpTransAll.Controls.Add(this.btnCopyAll);
            this.grpTransAll.Controls.Add(this.btnClearAll);
            this.grpTransAll.Controls.Add(this.btnDelete);
            this.grpTransAll.Controls.Add(this.AllScale);
            this.grpTransAll.Controls.Add(this.AllRot);
            this.grpTransAll.Controls.Add(this.AllTrans);
            this.grpTransAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTransAll.Enabled = false;
            this.grpTransAll.Location = new System.Drawing.Point(422, 0);
            this.grpTransAll.Name = "grpTransAll";
            this.grpTransAll.Size = new System.Drawing.Size(160, 78);
            this.grpTransAll.TabIndex = 26;
            this.grpTransAll.TabStop = false;
            this.grpTransAll.Text = "Transform All";
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(106, 54);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(50, 20);
            this.btnInsert.TabIndex = 24;
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnClean
            // 
            this.btnClean.Location = new System.Drawing.Point(106, 35);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(50, 20);
            this.btnClean.TabIndex = 29;
            this.btnClean.Text = "Clean";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // btnPasteAll
            // 
            this.btnPasteAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPasteAll.Location = new System.Drawing.Point(57, 35);
            this.btnPasteAll.Name = "btnPasteAll";
            this.btnPasteAll.Size = new System.Drawing.Size(50, 20);
            this.btnPasteAll.TabIndex = 28;
            this.btnPasteAll.Text = "Paste";
            this.btnPasteAll.UseVisualStyleBackColor = true;
            this.btnPasteAll.Click += new System.EventHandler(this.btnPasteAll_Click);
            // 
            // btnCopyAll
            // 
            this.btnCopyAll.Location = new System.Drawing.Point(57, 54);
            this.btnCopyAll.Name = "btnCopyAll";
            this.btnCopyAll.Size = new System.Drawing.Size(50, 20);
            this.btnCopyAll.TabIndex = 27;
            this.btnCopyAll.Text = "Copy";
            this.btnCopyAll.UseVisualStyleBackColor = true;
            this.btnCopyAll.Click += new System.EventHandler(this.btnCopyAll_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.Location = new System.Drawing.Point(106, 16);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(50, 20);
            this.btnClearAll.TabIndex = 26;
            this.btnClearAll.Text = "Clear";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDelete.Location = new System.Drawing.Point(57, 16);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(50, 20);
            this.btnDelete.TabIndex = 25;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // AllScale
            // 
            this.AllScale.AutoSize = true;
            this.AllScale.Checked = true;
            this.AllScale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AllScale.Location = new System.Drawing.Point(6, 56);
            this.AllScale.Name = "AllScale";
            this.AllScale.Size = new System.Drawing.Size(53, 17);
            this.AllScale.TabIndex = 32;
            this.AllScale.Text = "Scale";
            this.AllScale.UseVisualStyleBackColor = true;
            // 
            // AllRot
            // 
            this.AllRot.AutoSize = true;
            this.AllRot.Checked = true;
            this.AllRot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AllRot.Location = new System.Drawing.Point(6, 38);
            this.AllRot.Name = "AllRot";
            this.AllRot.Size = new System.Drawing.Size(43, 17);
            this.AllRot.TabIndex = 31;
            this.AllRot.Text = "Rot";
            this.AllRot.UseVisualStyleBackColor = true;
            // 
            // AllTrans
            // 
            this.AllTrans.AutoSize = true;
            this.AllTrans.Checked = true;
            this.AllTrans.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AllTrans.Location = new System.Drawing.Point(6, 20);
            this.AllTrans.Name = "AllTrans";
            this.AllTrans.Size = new System.Drawing.Size(53, 17);
            this.AllTrans.TabIndex = 30;
            this.AllTrans.Text = "Trans";
            this.AllTrans.UseVisualStyleBackColor = true;
            // 
            // ctxTools
            // 
            this.ctxTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bakeVertexPositionsToolStripMenuItem});
            this.ctxTools.Name = "ctxBox";
            this.ctxTools.Size = new System.Drawing.Size(186, 26);
            // 
            // bakeVertexPositionsToolStripMenuItem
            // 
            this.bakeVertexPositionsToolStripMenuItem.Name = "bakeVertexPositionsToolStripMenuItem";
            this.bakeVertexPositionsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.bakeVertexPositionsToolStripMenuItem.Text = "Bake Vertex Positions";
            this.bakeVertexPositionsToolStripMenuItem.Click += new System.EventHandler(this.bakeVertexPositionsToolStripMenuItem_Click);
            // 
            // ctxBox
            // 
            this.ctxBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Source,
            this.toolStripSeparator1,
            this.add,
            this.subtract,
            this.removeAllToolStripMenuItem,
            this.addCustomAmountToolStripMenuItem});
            this.ctxBox.Name = "ctxBox";
            this.ctxBox.Size = new System.Drawing.Size(167, 120);
            // 
            // Source
            // 
            this.Source.Enabled = false;
            this.Source.Name = "Source";
            this.Source.Size = new System.Drawing.Size(166, 22);
            this.Source.Text = "Source";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
            // 
            // add
            // 
            this.add.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem7});
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(166, 22);
            this.add.Text = "Add To All";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem3.Text = "+180";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem4.Text = "+90";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem7.Text = "+45";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // subtract
            // 
            this.subtract.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem8});
            this.subtract.Name = "subtract";
            this.subtract.Size = new System.Drawing.Size(166, 22);
            this.subtract.Text = "Subtract From All";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(97, 22);
            this.toolStripMenuItem5.Text = "-180";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(97, 22);
            this.toolStripMenuItem6.Text = "-90";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(97, 22);
            this.toolStripMenuItem8.Text = "-45";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem8_Click);
            // 
            // removeAllToolStripMenuItem
            // 
            this.removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
            this.removeAllToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.removeAllToolStripMenuItem.Text = "Remove All";
            this.removeAllToolStripMenuItem.Click += new System.EventHandler(this.removeAllToolStripMenuItem_Click);
            // 
            // addCustomAmountToolStripMenuItem
            // 
            this.addCustomAmountToolStripMenuItem.Name = "addCustomAmountToolStripMenuItem";
            this.addCustomAmountToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.addCustomAmountToolStripMenuItem.Text = "Edit All...";
            this.addCustomAmountToolStripMenuItem.Click += new System.EventHandler(this.addCustomAmountToolStripMenuItem_Click);
            // 
            // numScaleX
            // 
            this.numScaleX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numScaleX.Integral = false;
            this.numScaleX.Location = new System.Drawing.Point(73, 54);
            this.numScaleX.MaximumValue = 3.402823E+38F;
            this.numScaleX.MinimumValue = -3.402823E+38F;
            this.numScaleX.Name = "numScaleX";
            this.numScaleX.Size = new System.Drawing.Size(82, 20);
            this.numScaleX.TabIndex = 18;
            this.numScaleX.Text = "0";
            this.numScaleX.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            this.numScaleX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numScaleY
            // 
            this.numScaleY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numScaleY.Integral = false;
            this.numScaleY.Location = new System.Drawing.Point(154, 54);
            this.numScaleY.MaximumValue = 3.402823E+38F;
            this.numScaleY.MinimumValue = -3.402823E+38F;
            this.numScaleY.Name = "numScaleY";
            this.numScaleY.Size = new System.Drawing.Size(82, 20);
            this.numScaleY.TabIndex = 19;
            this.numScaleY.Text = "0";
            this.numScaleY.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            this.numScaleY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numScaleZ
            // 
            this.numScaleZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numScaleZ.Integral = false;
            this.numScaleZ.Location = new System.Drawing.Point(235, 54);
            this.numScaleZ.MaximumValue = 3.402823E+38F;
            this.numScaleZ.MinimumValue = -3.402823E+38F;
            this.numScaleZ.Name = "numScaleZ";
            this.numScaleZ.Size = new System.Drawing.Size(82, 20);
            this.numScaleZ.TabIndex = 20;
            this.numScaleZ.Text = "0";
            this.numScaleZ.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            this.numScaleZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numRotX
            // 
            this.numRotX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotX.Integral = false;
            this.numRotX.Location = new System.Drawing.Point(73, 35);
            this.numRotX.MaximumValue = 3.402823E+38F;
            this.numRotX.MinimumValue = -3.402823E+38F;
            this.numRotX.Name = "numRotX";
            this.numRotX.Size = new System.Drawing.Size(82, 20);
            this.numRotX.TabIndex = 15;
            this.numRotX.Text = "0";
            this.numRotX.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            this.numRotX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numRotY
            // 
            this.numRotY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotY.Integral = false;
            this.numRotY.Location = new System.Drawing.Point(154, 35);
            this.numRotY.MaximumValue = 3.402823E+38F;
            this.numRotY.MinimumValue = -3.402823E+38F;
            this.numRotY.Name = "numRotY";
            this.numRotY.Size = new System.Drawing.Size(82, 20);
            this.numRotY.TabIndex = 16;
            this.numRotY.Text = "0";
            this.numRotY.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            this.numRotY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numRotZ
            // 
            this.numRotZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotZ.Integral = false;
            this.numRotZ.Location = new System.Drawing.Point(235, 35);
            this.numRotZ.MaximumValue = 3.402823E+38F;
            this.numRotZ.MinimumValue = -3.402823E+38F;
            this.numRotZ.Name = "numRotZ";
            this.numRotZ.Size = new System.Drawing.Size(82, 20);
            this.numRotZ.TabIndex = 17;
            this.numRotZ.Text = "0";
            this.numRotZ.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            this.numRotZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numTransX
            // 
            this.numTransX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTransX.Integral = false;
            this.numTransX.Location = new System.Drawing.Point(73, 16);
            this.numTransX.MaximumValue = 3.402823E+38F;
            this.numTransX.MinimumValue = -3.402823E+38F;
            this.numTransX.Name = "numTransX";
            this.numTransX.Size = new System.Drawing.Size(82, 20);
            this.numTransX.TabIndex = 3;
            this.numTransX.Text = "0";
            this.numTransX.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            this.numTransX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numTransY
            // 
            this.numTransY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTransY.Integral = false;
            this.numTransY.Location = new System.Drawing.Point(154, 16);
            this.numTransY.MaximumValue = 3.402823E+38F;
            this.numTransY.MinimumValue = -3.402823E+38F;
            this.numTransY.Name = "numTransY";
            this.numTransY.Size = new System.Drawing.Size(82, 20);
            this.numTransY.TabIndex = 13;
            this.numTransY.Text = "0";
            this.numTransY.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            this.numTransY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numTransZ
            // 
            this.numTransZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTransZ.Integral = false;
            this.numTransZ.Location = new System.Drawing.Point(235, 16);
            this.numTransZ.MaximumValue = 3.402823E+38F;
            this.numTransZ.MinimumValue = -3.402823E+38F;
            this.numTransZ.Name = "numTransZ";
            this.numTransZ.Size = new System.Drawing.Size(82, 20);
            this.numTransZ.TabIndex = 14;
            this.numTransZ.Text = "0";
            this.numTransZ.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            this.numTransZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // CHR0Editor
            // 
            this.Controls.Add(this.grpTransAll);
            this.Controls.Add(this.grpTransform);
            this.MinimumSize = new System.Drawing.Size(582, 78);
            this.Name = "CHR0Editor";
            this.Size = new System.Drawing.Size(582, 78);
            this.grpTransform.ResumeLayout(false);
            this.grpTransform.PerformLayout();
            this.grpTransAll.ResumeLayout(false);
            this.grpTransAll.PerformLayout();
            this.ctxTools.ResumeLayout(false);
            this.ctxBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public ModelEditorBase _mainWindow;

        public NumericInputBox[] _transBoxes = new NumericInputBox[9];
        //private AnimationFrame _tempFrame = AnimationFrame.Identity;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBoneNode TargetBone { get { return _mainWindow.SelectedBone; } set { _mainWindow.SelectedBone = value; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0MaterialRefNode TargetTexRef { get { return _mainWindow.TargetTexRef; } set { _mainWindow.TargetTexRef = value; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get { return _mainWindow.CurrentFrame; }
            set { _mainWindow.CurrentFrame = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get { return _mainWindow.TargetModel; }
            set { _mainWindow.TargetModel = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CHR0Node SelectedAnimation
        {
            get { return _mainWindow.SelectedCHR0; }
            set { _mainWindow.SelectedCHR0 = value; }
        }
        public CHR0Editor()
        {
            InitializeComponent(); 
            _transBoxes[0] = numScaleX; numScaleX.Tag = 0;
            _transBoxes[1] = numScaleY; numScaleY.Tag = 1;
            _transBoxes[2] = numScaleZ; numScaleZ.Tag = 2;
            _transBoxes[3] = numRotX; numRotX.Tag = 3;
            _transBoxes[4] = numRotY; numRotY.Tag = 4;
            _transBoxes[5] = numRotZ; numRotZ.Tag = 5;
            _transBoxes[6] = numTransX; numTransX.Tag = 6;
            _transBoxes[7] = numTransY; numTransY.Tag = 7;
            _transBoxes[8] = numTransZ; numTransZ.Tag = 8;

            foreach (NumericInputBox box in _transBoxes) {
                box.KeyUp += (sender, e) => {
                    // If the user has selected the whole text and wants to replace it with a negative number, allow them to enter a minus sign.
                    if (e.KeyCode == Keys.OemMinus) {
                        NumericInputBox n = (NumericInputBox)sender;
                        if (n.SelectionLength == n.Text.Length) {
                            n.Text = "-";
                            n.Select(1, 1);
                        }
                    }
                };
            }
        }
        public void UpdatePropDisplay()
        {
            if (!Enabled)
                return;

            chkMoveBoneOnly.Visible =
            chkUpdateBindPose.Visible = chkUpdateBindPose.Enabled = 
            CurrentFrame < 1;

            chkMoveBoneOnly.Enabled = chkUpdateBindPose.Checked;

            grpTransAll.Enabled = TargetModel != null;
            btnInsert.Enabled = btnDelete.Enabled = btnClearAll.Enabled = CurrentFrame >= 1 && SelectedAnimation != null;
            grpTransform.Enabled = TargetBone != null;

            //9 transforms xyz for scale/rot/trans
            for (int i = 0; i < 9; i++)
                ResetBox(i);

            if (_mainWindow.InterpolationEditor != null &&
                _mainWindow.InterpolationEditor.Visible &&
                _mainWindow.TargetAnimType == NW4RAnimType.CHR)
            {
                if (_mainWindow.InterpolationEditor._targetNode != Entry)
                    _mainWindow.InterpolationEditor.SetTarget(Entry);
                else
                    _mainWindow.InterpolationEditor.interpolationViewer.Invalidate();
            }
        }

        public CHR0EntryNode Entry
        {
            get
            {
                CHR0EntryNode entry;
                if (TargetBone != null && SelectedAnimation != null && CurrentFrame >= 1 && ((entry = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode) != null))
                    return entry;
                else 
                    return null;
            }
        }

        public unsafe void ResetBox(int index)
        {
            NumericInputBox box = _transBoxes[index];
            IBoneNode bone = TargetBone;
            CHR0EntryNode entry;
            if (TargetBone != null)
            {
                if ((SelectedAnimation != null) && (CurrentFrame >= 1) && ((entry = SelectedAnimation.FindChild(bone.Name, false) as CHR0EntryNode) != null))
                {
                    KeyframeEntry e = entry.Keyframes.GetKeyframe(index, CurrentFrame - 1);
                    if (e == null)
                    {
                        box.Value = entry.Keyframes.GetFrameValue(index, CurrentFrame - 1);
                        box.BackColor = Color.White;
                    }
                    else
                    {
                        box.Value = e._value;
                        box.BackColor = Color.Yellow;
                    }
                }
                else
                {
                    FrameState state = bone.BindState;
                    box.Value = ((float*)&state)[index];
                    box.BackColor = Color.White;
                }
            }
            else
            {
                box.Value = index < 3 ? 1 : 0;
                box.BackColor = Color.White;
            }
        }
        public unsafe void ApplyState(FrameState state)
        {
            float* p = (float*)&state;
            for (int i = 0; i < 9; i++)
                if (_transBoxes[i].Value != p[i])
                {
                    _transBoxes[i].Value = p[i];
                    BoxChanged(_transBoxes[i], null);
                }
        }
        public unsafe void BoxChangedCreateUndo(object sender, EventArgs e)
        {
            if (TargetBone == null)
                return;

            _mainWindow.BoneChange(TargetBone);

            //Only update for input boxes: Methods affecting multiple values call BoxChanged on their own.
            if (sender.GetType() == typeof(NumericInputBox))
                BoxChanged(sender, null);

            _mainWindow.BoneChange(TargetBone);
        }

        public unsafe void BoxChanged(object sender, EventArgs e)
        {
            if (TargetBone == null)
                return;

            NumericInputBox box = sender as NumericInputBox;
            int index = (int)box.Tag;

            IBoneNode bone = TargetBone;
            CHRAnimationFrame kf; 
            float* pkf = (float*)&kf;

            if ((SelectedAnimation != null) && (CurrentFrame >= 1))
            {
                CHR0EntryNode entry = SelectedAnimation.FindChild(bone.Name, false) as CHR0EntryNode;
                if (entry == null)
                {
                    if (!float.IsNaN(box.Value))
                    {
                        entry = SelectedAnimation.CreateEntry();
                        entry.Name = bone.Name;

                        FrameState state = bone.BindState;
                        float* p = (float*)&state;
                        for (int i = 0; i < 3; i++)
                            if (p[i] != 1.0f)
                                entry.SetKeyframe(i, 0, p[i]);
                        for (int i = 3; i < 9; i++)
                            if (p[i] != 0.0f)
                                entry.SetKeyframe(i, 0, p[i]);

                        entry.SetKeyframe(index, CurrentFrame - 1, box.Value);
                    }
                }
                else
                    if (float.IsNaN(box.Value))
                        entry.RemoveKeyframe(index, CurrentFrame - 1);
                    else
                        entry.SetKeyframe(index, CurrentFrame - 1, box.Value);

                if (_mainWindow.InterpolationEditor != null &&
                    _mainWindow.InterpolationEditor.Visible &&
                    _mainWindow.TargetAnimType == NW4RAnimType.CHR)
                {
                    if (_mainWindow.InterpolationEditor._targetNode != Entry)
                        _mainWindow.InterpolationEditor.SetTarget(Entry);
                    else
                        _mainWindow.InterpolationEditor.interpolationViewer.Invalidate();
                }
            }
            else
            {
                //Change base transform
                FrameState state = bone.BindState;
                float* p = (float*)&state;
                p[index] = float.IsNaN(box.Value) ? (index > 2 ? 0.0f : 1.0f) : box.Value;
                state.CalcTransforms();
                bone.BindState = state;

                //This will make the model not move with the bone
                //This will recalculate matrices and vertices/normals
                //AFTER a drag change is made, not during
                if (chkUpdateBindPose.Checked && !_mainWindow._boneSelection.IsMoving())
                    bone.RecalcBindState(true, !chkMoveBoneOnly.Checked);

                ((ResourceNode)bone).SignalPropertyChange();
            }

            _mainWindow.UpdateModel();

            ResetBox(index);
            //_mainWindow.KeyframePanel.UpdateKeyframe(CurrentFrame - 1);
            UpdateInterpolationEditor(box);

            if (_mainWindow.InterpolationEditor != null && _mainWindow.InterpolationEditor.Visible)
                _mainWindow.InterpolationEditor.KeyframeChanged();
        }

        private static Dictionary<string, CHRAnimationFrame> _copyAllState = new Dictionary<string, CHRAnimationFrame>();
        
        private void btnCopyAll_Click(object sender, EventArgs e)
        {
            _copyAllState.Clear();

            if (CurrentFrame < 1)
                foreach (MDL0BoneNode bone in TargetModel.BoneCache)
                {
                    CHRAnimationFrame frame = (CHRAnimationFrame)bone._bindState;
                    if (!AllTrans.Checked)
                        frame.Translation = new Vector3();
                    if (!AllRot.Checked)
                        frame.Rotation = new Vector3();
                    if (!AllScale.Checked)
                        frame.Scale = new Vector3(1);
                    for (int i = 0; i < 9; i++)
                        frame.SetBool(i, true);
                    _copyAllState[bone._name] = frame;
                }
            else
                foreach (CHR0EntryNode entry in SelectedAnimation.Children)
                {
                    CHRAnimationFrame frame = entry.GetAnimFrame(CurrentFrame - 1);
                    if (!AllTrans.Checked)
                        frame.Translation = new Vector3();
                    if (!AllRot.Checked)
                        frame.Rotation = new Vector3();
                    if (!AllScale.Checked)
                        frame.Scale = new Vector3(1);
                    _copyAllState[entry._name] = frame;
                }
        }

        public bool _onlyKeys;
        private unsafe void btnPasteAll_Click(object sender, EventArgs e)
        {
            if (_copyAllState.Count == 0)
                return;

            List<IBoneNode> o = new List<IBoneNode>();
            foreach (MDL0BoneNode bone in TargetModel.BoneCache)
                if (_copyAllState.ContainsKey(bone._name))
                    o.Add(bone);

            _mainWindow.BoneChange(o.ToArray());
            if (CurrentFrame < 1)
            {
                foreach (MDL0BoneNode bone in o)
                {
                    CHRAnimationFrame f = _copyAllState[bone._name];
                    if (!_onlyKeys)
                    {
                        if (AllTrans.Checked)
                            bone._bindState._translate = f.Translation;
                        if (AllRot.Checked)
                            bone._bindState._rotate = f.Rotation;
                        if (AllScale.Checked)
                            bone._bindState._scale = f.Scale;
                    }
                    else
                    {
                        FrameState s = bone._bindState;
                        float* sPtr = (float*)&f;
                        float* dPtr = (float*)&s;
                        for (int x = 0; x < 9; x++)
                            if (f.GetBool(x))
                                dPtr[x] = sPtr[x];
                        bone._bindState = s;
                    }
                    bone._bindState.CalcTransforms();
                    if (chkUpdateBindPose.Checked)
                        bone.RecalcBindState(true, !chkMoveBoneOnly.Checked);
                    bone.SignalPropertyChange();
                }
            }
            else
            {
                foreach (string name in _copyAllState.Keys)
                {
                    CHR0EntryNode entry = null;
                    if ((entry = SelectedAnimation.FindChild(name, false) as CHR0EntryNode) == null)
                    {
                        entry = new CHR0EntryNode() { Name = name };
                        SelectedAnimation.AddChild(entry);
                        entry.SetSize(SelectedAnimation.FrameCount, SelectedAnimation.Loop);
                    }

                    CHRAnimationFrame f = _copyAllState[entry._name];
                    int i = CurrentFrame - 1;
                    float* ptr = (float*)&f;
                    if (!_onlyKeys)
                    {
                        if (AllTrans.Checked)
                            entry.SetKeyframeOnlyTrans(i, f);
                        if (AllRot.Checked)
                            entry.SetKeyframeOnlyRot(i, f);
                        if (AllScale.Checked)
                            entry.SetKeyframeOnlyScale(i, f);
                    }
                    else
                        for (int x = 0; x < 9; x++)
                            if (f.GetBool(x))
                                entry.SetKeyframe(x, i, ptr[x]);
                }
            }
            _mainWindow.BoneChange(o.ToArray());
            _mainWindow.UpdateModel();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (CurrentFrame < 1)
                return;

            foreach (CHR0EntryNode entry in SelectedAnimation.Children)
            {
                if (AllTrans.Checked)
                    entry.RemoveKeyframeOnlyTrans(CurrentFrame - 1);
                if (AllRot.Checked)
                    entry.RemoveKeyframeOnlyRot(CurrentFrame - 1);
                if (AllScale.Checked)
                    entry.RemoveKeyframeOnlyScale(CurrentFrame - 1);
            }

            _mainWindow.UpdateModel();
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            if (!(TargetModel is MDL0Node))
                return;

            ResourceNode group = ((MDL0Node)TargetModel)._boneGroup;
            if (group == null)
                return;

            int keyCount = 0;
            List<CHR0EntryNode> badNodes = new List<CHR0EntryNode>();
            foreach (CHR0EntryNode entry in SelectedAnimation.Children)
            {
                if (group.FindChild(entry._name, true) == null)
                    badNodes.Add(entry);
                else
                    keyCount += entry.Keyframes.Clean();
            }
            int nodeCount = badNodes.Count;
            foreach (CHR0EntryNode n in badNodes)
            {
                n.Remove();
                n.Dispose();
            }
            MessageBox.Show(String.Format("{0} unused bone entr{2} and {1} redundant keyframe value{3} removed.", nodeCount, keyCount, nodeCount == 1 ? "y" : "ies", keyCount == 1 ? " was" : "s were"));
            UpdatePropDisplay();
        }

        private void ctxBox_Opening(object sender, CancelEventArgs e)
        {
            if (SelectedAnimation == null || numRotX.Enabled == false || numRotY.Enabled == false || numRotZ.Enabled == false)
                e.Cancel = true;
        }

        public void UpdateInterpolationEditor(NumericInputBox box)
        {
            if (_mainWindow.InterpolationEditor == null || !_mainWindow.InterpolationEditor.Visible)
                return;

            _mainWindow.InterpolationEditor.interpolationViewer._updating = true;
            if (box.BackColor == Color.Yellow)
            {
                CHR0EntryNode entry = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
                KeyframeEntry kfe = entry.GetKeyframe((int)box.Tag, CurrentFrame - 1);
                _mainWindow.InterpolationEditor.SelectedKeyframe = kfe;
            }
            else
                _mainWindow.InterpolationEditor.SelectedKeyframe = null;
            _mainWindow.InterpolationEditor.interpolationViewer._updating = false;
        }

        public int type = 0;
        private void box_MouseDown(object sender, MouseEventArgs e)
        {
            NumericInputBox box = sender as NumericInputBox;

            type = (int)box.Tag;

            if (_mainWindow.InterpolationEditor != null && _mainWindow.InterpolationEditor.Visible)
            {
                if (_mainWindow.InterpolationEditor.SelectedMode != type)
                    _mainWindow.InterpolationEditor.SelectedMode = type;
                UpdateInterpolationEditor(box);
            }

            if (e.Button == Forms.MouseButtons.Right)
                if (box.Enabled == true)
                {
                    box.ContextMenuStrip = ctxBox;
                    Source.Text = box.Text;
                }
                else
                    box.ContextMenuStrip = null;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                    if ((kfe = _target.GetKeyframe(type, x)) != null) //Check for a keyframe
                    { kfe._value += 180; }
                ResetBox(type);
                _mainWindow.UpdateModel();
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                    if ((kfe = _target.GetKeyframe(type, x)) != null) //Check for a keyframe
                    { kfe._value += 90; }
                ResetBox(type);
                _mainWindow.UpdateModel();
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                    if ((kfe = _target.GetKeyframe(type, x)) != null) //Check for a keyframe
                    { kfe._value -= 90; }
                ResetBox(type);
                _mainWindow.UpdateModel();
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                    if ((kfe = _target.GetKeyframe(type, x)) != null) //Check for a keyframe
                    { kfe._value += 45; }
                ResetBox(type);
                _mainWindow.UpdateModel();
            }
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                    if ((kfe = _target.GetKeyframe(type, x)) != null) //Check for a keyframe
                    { kfe._value -= 45; }
                ResetBox(type);
                _mainWindow.UpdateModel();
            }
        }

        private void removeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
                return;

            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                _target.Keyframes._keyArrays[type]._keyRoot = new KeyframeEntry(-1, type <= 2 ? 1 : 0);
                _target.Keyframes._keyArrays[type]._keyCount = 0;
                _target.SignalPropertyChange();
                ResetBox(type);
                _mainWindow.UpdateModel();
            }
        }

        private void addCustomAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
                return;

            EditAllKeyframesDialog ed = new EditAllKeyframesDialog();
            ed.ShowDialog(this, type, SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode);
            ResetBox(type);
            _mainWindow.UpdateModel();
        }

        private unsafe void btnCut_Click(object sender, EventArgs e)
        {
            CHRAnimationFrame frame = new CHRAnimationFrame();
            float* p = (float*)&frame;
            //copy transform from boxes instead of the actual animation.
            BoxChangedCreateUndo(this, null);

            for (int i = 0; i < 9; i++)
            {
                frame.SetBool(i, false);
                if ((!FrameScale.Checked && i < 3))
                    p[i] = 1;
                else if (
                    (FrameScale.Checked && i < 3) ||
                    (FrameRot.Checked && i >= 3 && i < 6) ||
                    (FrameTrans.Checked && i >= 6))
                {
                    p[i] = _transBoxes[i].Value;
                    frame.SetBool(i, _transBoxes[i].BackColor == Color.Yellow);
                }
                _transBoxes[i].Value = float.NaN;
                BoxChanged(_transBoxes[i], null);
            }

            DataObject da = new DataObject();
            da.SetData("AnimationFrame", frame);
            Clipboard.SetDataObject(da, true);
        }

        public void ClearEntry()
        {
            BoxChangedCreateUndo(this, null);

            for (int i = 0; i < 9; i++)
            {
                _transBoxes[i].Value = float.NaN;
                BoxChanged(_transBoxes[i], null);
            }
        }

        private unsafe void btnCopy_Click(object sender, EventArgs e)
        {
            //copy the transform from the number boxes
            CHRAnimationFrame frame = new CHRAnimationFrame();
            float* p = (float*)&frame;

            for (int i = 0; i < 9; i++)
            {
                frame.SetBool(i, false);
                if ((!FrameScale.Checked && i < 3))
                    p[i] = 1;
                else if (
                    (FrameScale.Checked && i < 3) ||
                    (FrameRot.Checked && i >= 3 && i < 6) ||
                    (FrameTrans.Checked && i >= 6))
                {
                    p[i] = _transBoxes[i].Value;
                    frame.SetBool(i, _transBoxes[i].BackColor == Color.Yellow);
                }
            }
            DataObject da = new DataObject();
            da.SetData("AnimationFrame", frame);
            Clipboard.SetDataObject(da, true);
        }

        private unsafe void btnPaste_Click(object sender, EventArgs e)
        {
            IDataObject da = Clipboard.GetDataObject();
            if (da.GetDataPresent("AnimationFrame"))
            {
                object o = da.GetData("AnimationFrame");
                if (o != null && o is CHRAnimationFrame)
                {
                    CHRAnimationFrame frame = (CHRAnimationFrame)o;

                    float* p = (float*)&frame;

                    BoxChangedCreateUndo(this, null);

                    for (int i = 0; i < 9; i++)
                    {
                        if ((FrameScale.Checked && i < 3) ||
                            (FrameRot.Checked && i >= 3 && i < 6) ||
                            (FrameTrans.Checked && i >= 6))
                            if (_transBoxes[i].Value != p[i] && (!_onlyKeys || frame.GetBool(i)))
                            {
                                _transBoxes[i].Value = p[i];
                                BoxChanged(_transBoxes[i], null);
                            }
                    }
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if ((SelectedAnimation == null) || (CurrentFrame < 1))
                return;

            SelectedAnimation.InsertKeyframe(CurrentFrame - 1);
            _mainWindow.UpdateModel();

            _mainWindow.Updating = true;
            _mainWindow.PlaybackPanel.numTotalFrames.Value++;
            _mainWindow._maxFrame++;
            _mainWindow.Updating = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if ((SelectedAnimation == null) || (CurrentFrame < 1))
                return;

            SelectedAnimation.DeleteKeyframe(CurrentFrame - 1);
            _mainWindow.UpdateModel();

            _mainWindow.Updating = true;
            _mainWindow.PlaybackPanel.numTotalFrames.Value--;
            _mainWindow._maxFrame--;
            _mainWindow.Updating = false;
        }

        private void bakeVertexPositionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TargetModel != null && TargetModel is MDL0Node && TargetModel.Objects != null)
            {
                foreach (MDL0ObjectNode o in TargetModel.Objects)
                {
                    for (int i = 0; i < o._manager._vertices.Count; i++)
                    {
                        Vertex3 vec = o._manager._vertices[i];
                        o._vertexNode.Vertices[o._manager._vertices[i]._facepoints[0]._vertexIndex] = vec.WeightedPosition;
                    }

                    o._vertexNode.ForceRebuild = true;
                    if (o._vertexNode.Format == WiiVertexComponentType.Float)
                        o._vertexNode.ForceFloat = true;
                }
                _mainWindow.UpdateModel();
            }
        }

        private void chkBoneEdit_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void chkUpdateBindPose_CheckedChanged(object sender, EventArgs e)
        {
            chkMoveBoneOnly.Enabled = chkUpdateBindPose.Checked;
        }

        //private void chkLinear_CheckedChanged(object sender, EventArgs e)
        //{
        //    DialogResult r;
        //    if (SelectedAnimation != null)
        //        if (TargetBone != null)
        //        {
        //            if ((r = MessageBox.Show("Do you want to apply linear interpolation to only this bone?\nOtherwise, all bones in the animation will be set to linear.", "", MessageBoxButtons.YesNoCancel)) == DialogResult.Yes)
        //                (SelectedAnimation.FindChild(TargetBone.Name, true) as CHR0EntryNode).Keyframes.LinearRotation = chkLinear.Checked;
        //            else if (r == DialogResult.No)
        //                foreach (CHR0EntryNode n in SelectedAnimation.Children)
        //                    n.Keyframes.LinearRotation = chkLinear.Checked;
        //            else return;
        //        }
        //        else
        //            foreach (CHR0EntryNode n in SelectedAnimation.Children)
        //                n.Keyframes.LinearRotation = chkLinear.Checked;
        //}

        //private void chkLoop_CheckedChanged(object sender, EventArgs e)
        //{
        //    SelectedAnimation.Loop = chkLoop.Checked ? true : false;
        //}
    }
}
