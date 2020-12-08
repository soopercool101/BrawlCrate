using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using BrawlLib.Wii.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.Editors
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
        private IContainer components;
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
            components = new Container();
            grpTransform = new GroupBox();
            FrameScale = new CheckBox();
            btnPaste = new Button();
            chkUpdateBindPose = new CheckBox();
            FrameRot = new CheckBox();
            btnCopy = new Button();
            FrameTrans = new CheckBox();
            btnCut = new Button();
            lblTrans = new Label();
            lblRot = new Label();
            lblScale = new Label();
            chkMoveBoneOnly = new CheckBox();
            grpTransAll = new GroupBox();
            btnInsert = new Button();
            btnClean = new Button();
            btnPasteAll = new Button();
            btnCopyAll = new Button();
            btnClearAll = new Button();
            btnDelete = new Button();
            AllScale = new CheckBox();
            AllRot = new CheckBox();
            AllTrans = new CheckBox();
            ctxTools = new ContextMenuStrip(components);
            bakeVertexPositionsToolStripMenuItem = new ToolStripMenuItem();
            ctxBox = new ContextMenuStrip(components);
            Source = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            add = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripMenuItem();
            toolStripMenuItem7 = new ToolStripMenuItem();
            subtract = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripMenuItem();
            toolStripMenuItem6 = new ToolStripMenuItem();
            toolStripMenuItem8 = new ToolStripMenuItem();
            removeAllToolStripMenuItem = new ToolStripMenuItem();
            addCustomAmountToolStripMenuItem = new ToolStripMenuItem();
            numScaleX = new NumericInputBox();
            numScaleY = new NumericInputBox();
            numScaleZ = new NumericInputBox();
            numRotX = new NumericInputBox();
            numRotY = new NumericInputBox();
            numRotZ = new NumericInputBox();
            numTransX = new NumericInputBox();
            numTransY = new NumericInputBox();
            numTransZ = new NumericInputBox();
            grpTransform.SuspendLayout();
            grpTransAll.SuspendLayout();
            ctxTools.SuspendLayout();
            ctxBox.SuspendLayout();
            SuspendLayout();
            // 
            // grpTransform
            // 
            grpTransform.Controls.Add(FrameScale);
            grpTransform.Controls.Add(btnPaste);
            grpTransform.Controls.Add(chkUpdateBindPose);
            grpTransform.Controls.Add(chkMoveBoneOnly);
            grpTransform.Controls.Add(FrameRot);
            grpTransform.Controls.Add(btnCopy);
            grpTransform.Controls.Add(FrameTrans);
            grpTransform.Controls.Add(btnCut);
            grpTransform.Controls.Add(numScaleX);
            grpTransform.Controls.Add(numScaleY);
            grpTransform.Controls.Add(numScaleZ);
            grpTransform.Controls.Add(numRotX);
            grpTransform.Controls.Add(numRotY);
            grpTransform.Controls.Add(numRotZ);
            grpTransform.Controls.Add(numTransX);
            grpTransform.Controls.Add(numTransY);
            grpTransform.Controls.Add(numTransZ);
            grpTransform.Controls.Add(lblTrans);
            grpTransform.Controls.Add(lblRot);
            grpTransform.Controls.Add(lblScale);
            grpTransform.Dock = DockStyle.Left;
            grpTransform.Enabled = false;
            grpTransform.Location = new Point(0, 0);
            grpTransform.Name = "grpTransform";
            grpTransform.Padding = new Padding(0);
            grpTransform.Size = new Size(422, 78);
            grpTransform.TabIndex = 23;
            grpTransform.TabStop = false;
            grpTransform.Text = "Transform Frame";
            // 
            // FrameScale
            // 
            FrameScale.AutoSize = true;
            FrameScale.Checked = true;
            FrameScale.CheckState = CheckState.Checked;
            FrameScale.Location = new Point(367, 56);
            FrameScale.Name = "FrameScale";
            FrameScale.Size = new Size(53, 17);
            FrameScale.TabIndex = 35;
            FrameScale.Text = "Scale";
            FrameScale.UseVisualStyleBackColor = true;
            // 
            // btnPaste
            // 
            btnPaste.Location = new Point(318, 54);
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new Size(48, 20);
            btnPaste.TabIndex = 23;
            btnPaste.Text = "Paste";
            btnPaste.UseVisualStyleBackColor = true;
            btnPaste.Click += new EventHandler(btnPaste_Click);
            // 
            // chkUpdateBindPose
            // 
            chkUpdateBindPose.Location = new Point(154, -1);
            chkUpdateBindPose.Name = "chkUpdateBindPose";
            chkUpdateBindPose.Size = new Size(146, 17);
            chkUpdateBindPose.TabIndex = 37;
            chkUpdateBindPose.Text = "Update Mesh Bind Pose";
            chkUpdateBindPose.UseVisualStyleBackColor = true;
            chkUpdateBindPose.CheckedChanged += new EventHandler(chkUpdateBindPose_CheckedChanged);
            // 
            // FrameRot
            // 
            FrameRot.AutoSize = true;
            FrameRot.Checked = true;
            FrameRot.CheckState = CheckState.Checked;
            FrameRot.Location = new Point(367, 38);
            FrameRot.Name = "FrameRot";
            FrameRot.Size = new Size(43, 17);
            FrameRot.TabIndex = 34;
            FrameRot.Text = "Rot";
            FrameRot.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            btnCopy.Location = new Point(318, 35);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(48, 20);
            btnCopy.TabIndex = 22;
            btnCopy.Text = "Copy";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += new EventHandler(btnCopy_Click);
            // 
            // FrameTrans
            // 
            FrameTrans.AutoSize = true;
            FrameTrans.Checked = true;
            FrameTrans.CheckState = CheckState.Checked;
            FrameTrans.Location = new Point(367, 20);
            FrameTrans.Name = "FrameTrans";
            FrameTrans.Size = new Size(53, 17);
            FrameTrans.TabIndex = 33;
            FrameTrans.Text = "Trans";
            FrameTrans.UseVisualStyleBackColor = true;
            // 
            // btnCut
            // 
            btnCut.FlatAppearance.BorderColor = Color.DimGray;
            btnCut.Location = new Point(318, 16);
            btnCut.Name = "btnCut";
            btnCut.Size = new Size(48, 20);
            btnCut.TabIndex = 21;
            btnCut.Text = "Cut";
            btnCut.UseVisualStyleBackColor = true;
            btnCut.Click += new EventHandler(btnCut_Click);
            // 
            // lblTrans
            // 
            lblTrans.BorderStyle = BorderStyle.FixedSingle;
            lblTrans.Location = new Point(4, 16);
            lblTrans.Name = "lblTrans";
            lblTrans.Size = new Size(70, 20);
            lblTrans.TabIndex = 4;
            lblTrans.Text = "Translation:";
            lblTrans.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblRot
            // 
            lblRot.BorderStyle = BorderStyle.FixedSingle;
            lblRot.Location = new Point(4, 35);
            lblRot.Name = "lblRot";
            lblRot.Size = new Size(70, 20);
            lblRot.TabIndex = 5;
            lblRot.Text = "Rotation:";
            lblRot.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblScale
            // 
            lblScale.BorderStyle = BorderStyle.FixedSingle;
            lblScale.Location = new Point(4, 54);
            lblScale.Name = "lblScale";
            lblScale.Size = new Size(70, 20);
            lblScale.TabIndex = 6;
            lblScale.Text = "Scale:";
            lblScale.TextAlign = ContentAlignment.MiddleRight;
            // 
            // chkMoveBoneOnly
            // 
            chkMoveBoneOnly.Enabled = false;
            chkMoveBoneOnly.Location = new Point(302, -1);
            chkMoveBoneOnly.Name = "chkMoveBoneOnly";
            chkMoveBoneOnly.Size = new Size(113, 17);
            chkMoveBoneOnly.TabIndex = 36;
            chkMoveBoneOnly.Text = "Move Bone Only";
            chkMoveBoneOnly.UseVisualStyleBackColor = true;
            chkMoveBoneOnly.CheckedChanged += new EventHandler(chkBoneEdit_CheckedChanged);
            // 
            // grpTransAll
            // 
            grpTransAll.Controls.Add(btnInsert);
            grpTransAll.Controls.Add(btnClean);
            grpTransAll.Controls.Add(btnPasteAll);
            grpTransAll.Controls.Add(btnCopyAll);
            grpTransAll.Controls.Add(btnClearAll);
            grpTransAll.Controls.Add(btnDelete);
            grpTransAll.Controls.Add(AllScale);
            grpTransAll.Controls.Add(AllRot);
            grpTransAll.Controls.Add(AllTrans);
            grpTransAll.Dock = DockStyle.Fill;
            grpTransAll.Enabled = false;
            grpTransAll.Location = new Point(422, 0);
            grpTransAll.Name = "grpTransAll";
            grpTransAll.Size = new Size(160, 78);
            grpTransAll.TabIndex = 26;
            grpTransAll.TabStop = false;
            grpTransAll.Text = "Transform All";
            // 
            // btnInsert
            // 
            btnInsert.Location = new Point(106, 54);
            btnInsert.Name = "btnInsert";
            btnInsert.Size = new Size(50, 20);
            btnInsert.TabIndex = 24;
            btnInsert.Text = "Insert";
            btnInsert.UseVisualStyleBackColor = true;
            btnInsert.Click += new EventHandler(btnInsert_Click);
            // 
            // btnClean
            // 
            btnClean.Location = new Point(106, 35);
            btnClean.Name = "btnClean";
            btnClean.Size = new Size(50, 20);
            btnClean.TabIndex = 29;
            btnClean.Text = "Clean";
            btnClean.UseVisualStyleBackColor = true;
            btnClean.Click += new EventHandler(btnClean_Click);
            // 
            // btnPasteAll
            // 
            btnPasteAll.FlatStyle = FlatStyle.System;
            btnPasteAll.Location = new Point(57, 35);
            btnPasteAll.Name = "btnPasteAll";
            btnPasteAll.Size = new Size(50, 20);
            btnPasteAll.TabIndex = 28;
            btnPasteAll.Text = "Paste";
            btnPasteAll.UseVisualStyleBackColor = true;
            btnPasteAll.Click += new EventHandler(btnPasteAll_Click);
            // 
            // btnCopyAll
            // 
            btnCopyAll.Location = new Point(57, 54);
            btnCopyAll.Name = "btnCopyAll";
            btnCopyAll.Size = new Size(50, 20);
            btnCopyAll.TabIndex = 27;
            btnCopyAll.Text = "Copy";
            btnCopyAll.UseVisualStyleBackColor = true;
            btnCopyAll.Click += new EventHandler(btnCopyAll_Click);
            // 
            // btnClearAll
            // 
            btnClearAll.Location = new Point(106, 16);
            btnClearAll.Name = "btnClearAll";
            btnClearAll.Size = new Size(50, 20);
            btnClearAll.TabIndex = 26;
            btnClearAll.Text = "Clear";
            btnClearAll.UseVisualStyleBackColor = true;
            btnClearAll.Click += new EventHandler(btnClear_Click);
            // 
            // btnDelete
            // 
            btnDelete.FlatStyle = FlatStyle.System;
            btnDelete.Location = new Point(57, 16);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(50, 20);
            btnDelete.TabIndex = 25;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += new EventHandler(btnDelete_Click);
            // 
            // AllScale
            // 
            AllScale.AutoSize = true;
            AllScale.Checked = true;
            AllScale.CheckState = CheckState.Checked;
            AllScale.Location = new Point(6, 56);
            AllScale.Name = "AllScale";
            AllScale.Size = new Size(53, 17);
            AllScale.TabIndex = 32;
            AllScale.Text = "Scale";
            AllScale.UseVisualStyleBackColor = true;
            // 
            // AllRot
            // 
            AllRot.AutoSize = true;
            AllRot.Checked = true;
            AllRot.CheckState = CheckState.Checked;
            AllRot.Location = new Point(6, 38);
            AllRot.Name = "AllRot";
            AllRot.Size = new Size(43, 17);
            AllRot.TabIndex = 31;
            AllRot.Text = "Rot";
            AllRot.UseVisualStyleBackColor = true;
            // 
            // AllTrans
            // 
            AllTrans.AutoSize = true;
            AllTrans.Checked = true;
            AllTrans.CheckState = CheckState.Checked;
            AllTrans.Location = new Point(6, 20);
            AllTrans.Name = "AllTrans";
            AllTrans.Size = new Size(53, 17);
            AllTrans.TabIndex = 30;
            AllTrans.Text = "Trans";
            AllTrans.UseVisualStyleBackColor = true;
            // 
            // ctxTools
            // 
            ctxTools.Items.AddRange(new ToolStripItem[]
            {
                bakeVertexPositionsToolStripMenuItem
            });
            ctxTools.Name = "ctxBox";
            ctxTools.Size = new Size(186, 26);
            // 
            // bakeVertexPositionsToolStripMenuItem
            // 
            bakeVertexPositionsToolStripMenuItem.Name = "bakeVertexPositionsToolStripMenuItem";
            bakeVertexPositionsToolStripMenuItem.Size = new Size(185, 22);
            bakeVertexPositionsToolStripMenuItem.Text = "Bake Vertex Positions";
            bakeVertexPositionsToolStripMenuItem.Click += new EventHandler(bakeVertexPositionsToolStripMenuItem_Click);
            // 
            // ctxBox
            // 
            ctxBox.Items.AddRange(new ToolStripItem[]
            {
                Source,
                toolStripSeparator1,
                add,
                subtract,
                removeAllToolStripMenuItem,
                addCustomAmountToolStripMenuItem
            });
            ctxBox.Name = "ctxBox";
            ctxBox.Size = new Size(167, 120);
            // 
            // Source
            // 
            Source.Enabled = false;
            Source.Name = "Source";
            Source.Size = new Size(166, 22);
            Source.Text = "Source";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(163, 6);
            // 
            // add
            // 
            add.DropDownItems.AddRange(new ToolStripItem[]
            {
                toolStripMenuItem3,
                toolStripMenuItem4,
                toolStripMenuItem7
            });
            add.Name = "add";
            add.Size = new Size(166, 22);
            add.Text = "Add To All";
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(100, 22);
            toolStripMenuItem3.Text = "+180";
            toolStripMenuItem3.Click += new EventHandler(toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(100, 22);
            toolStripMenuItem4.Text = "+90";
            toolStripMenuItem4.Click += new EventHandler(toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem7
            // 
            toolStripMenuItem7.Name = "toolStripMenuItem7";
            toolStripMenuItem7.Size = new Size(100, 22);
            toolStripMenuItem7.Text = "+45";
            toolStripMenuItem7.Click += new EventHandler(toolStripMenuItem7_Click);
            // 
            // subtract
            // 
            subtract.DropDownItems.AddRange(new ToolStripItem[]
            {
                toolStripMenuItem5,
                toolStripMenuItem6,
                toolStripMenuItem8
            });
            subtract.Name = "subtract";
            subtract.Size = new Size(166, 22);
            subtract.Text = "Subtract From All";
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new Size(97, 22);
            toolStripMenuItem5.Text = "-180";
            toolStripMenuItem5.Click += new EventHandler(toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new Size(97, 22);
            toolStripMenuItem6.Text = "-90";
            toolStripMenuItem6.Click += new EventHandler(toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem8
            // 
            toolStripMenuItem8.Name = "toolStripMenuItem8";
            toolStripMenuItem8.Size = new Size(97, 22);
            toolStripMenuItem8.Text = "-45";
            toolStripMenuItem8.Click += new EventHandler(toolStripMenuItem8_Click);
            // 
            // removeAllToolStripMenuItem
            // 
            removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
            removeAllToolStripMenuItem.Size = new Size(166, 22);
            removeAllToolStripMenuItem.Text = "Remove All";
            removeAllToolStripMenuItem.Click += new EventHandler(removeAllToolStripMenuItem_Click);
            // 
            // addCustomAmountToolStripMenuItem
            // 
            addCustomAmountToolStripMenuItem.Name = "addCustomAmountToolStripMenuItem";
            addCustomAmountToolStripMenuItem.Size = new Size(166, 22);
            addCustomAmountToolStripMenuItem.Text = "Edit All...";
            addCustomAmountToolStripMenuItem.Click += new EventHandler(addCustomAmountToolStripMenuItem_Click);
            // 
            // numScaleX
            // 
            numScaleX.BorderStyle = BorderStyle.FixedSingle;
            numScaleX.Integral = false;
            numScaleX.Location = new Point(73, 54);
            numScaleX.MaximumValue = 3.402823E+38F;
            numScaleX.MinimumValue = -3.402823E+38F;
            numScaleX.Name = "numScaleX";
            numScaleX.Size = new Size(82, 20);
            numScaleX.TabIndex = 18;
            numScaleX.Text = "0";
            numScaleX.ValueChanged += new EventHandler(BoxChangedCreateUndo);
            numScaleX.MouseDown += new MouseEventHandler(box_MouseDown);
            // 
            // numScaleY
            // 
            numScaleY.BorderStyle = BorderStyle.FixedSingle;
            numScaleY.Integral = false;
            numScaleY.Location = new Point(154, 54);
            numScaleY.MaximumValue = 3.402823E+38F;
            numScaleY.MinimumValue = -3.402823E+38F;
            numScaleY.Name = "numScaleY";
            numScaleY.Size = new Size(82, 20);
            numScaleY.TabIndex = 19;
            numScaleY.Text = "0";
            numScaleY.ValueChanged += new EventHandler(BoxChangedCreateUndo);
            numScaleY.MouseDown += new MouseEventHandler(box_MouseDown);
            // 
            // numScaleZ
            // 
            numScaleZ.BorderStyle = BorderStyle.FixedSingle;
            numScaleZ.Integral = false;
            numScaleZ.Location = new Point(235, 54);
            numScaleZ.MaximumValue = 3.402823E+38F;
            numScaleZ.MinimumValue = -3.402823E+38F;
            numScaleZ.Name = "numScaleZ";
            numScaleZ.Size = new Size(82, 20);
            numScaleZ.TabIndex = 20;
            numScaleZ.Text = "0";
            numScaleZ.ValueChanged += new EventHandler(BoxChangedCreateUndo);
            numScaleZ.MouseDown += new MouseEventHandler(box_MouseDown);
            // 
            // numRotX
            // 
            numRotX.BorderStyle = BorderStyle.FixedSingle;
            numRotX.Integral = false;
            numRotX.Location = new Point(73, 35);
            numRotX.MaximumValue = 3.402823E+38F;
            numRotX.MinimumValue = -3.402823E+38F;
            numRotX.Name = "numRotX";
            numRotX.Size = new Size(82, 20);
            numRotX.TabIndex = 15;
            numRotX.Text = "0";
            numRotX.ValueChanged += new EventHandler(BoxChangedCreateUndo);
            numRotX.MouseDown += new MouseEventHandler(box_MouseDown);
            // 
            // numRotY
            // 
            numRotY.BorderStyle = BorderStyle.FixedSingle;
            numRotY.Integral = false;
            numRotY.Location = new Point(154, 35);
            numRotY.MaximumValue = 3.402823E+38F;
            numRotY.MinimumValue = -3.402823E+38F;
            numRotY.Name = "numRotY";
            numRotY.Size = new Size(82, 20);
            numRotY.TabIndex = 16;
            numRotY.Text = "0";
            numRotY.ValueChanged += new EventHandler(BoxChangedCreateUndo);
            numRotY.MouseDown += new MouseEventHandler(box_MouseDown);
            // 
            // numRotZ
            // 
            numRotZ.BorderStyle = BorderStyle.FixedSingle;
            numRotZ.Integral = false;
            numRotZ.Location = new Point(235, 35);
            numRotZ.MaximumValue = 3.402823E+38F;
            numRotZ.MinimumValue = -3.402823E+38F;
            numRotZ.Name = "numRotZ";
            numRotZ.Size = new Size(82, 20);
            numRotZ.TabIndex = 17;
            numRotZ.Text = "0";
            numRotZ.ValueChanged += new EventHandler(BoxChangedCreateUndo);
            numRotZ.MouseDown += new MouseEventHandler(box_MouseDown);
            // 
            // numTransX
            // 
            numTransX.BorderStyle = BorderStyle.FixedSingle;
            numTransX.Integral = false;
            numTransX.Location = new Point(73, 16);
            numTransX.MaximumValue = 3.402823E+38F;
            numTransX.MinimumValue = -3.402823E+38F;
            numTransX.Name = "numTransX";
            numTransX.Size = new Size(82, 20);
            numTransX.TabIndex = 3;
            numTransX.Text = "0";
            numTransX.ValueChanged += new EventHandler(BoxChangedCreateUndo);
            numTransX.MouseDown += new MouseEventHandler(box_MouseDown);
            // 
            // numTransY
            // 
            numTransY.BorderStyle = BorderStyle.FixedSingle;
            numTransY.Integral = false;
            numTransY.Location = new Point(154, 16);
            numTransY.MaximumValue = 3.402823E+38F;
            numTransY.MinimumValue = -3.402823E+38F;
            numTransY.Name = "numTransY";
            numTransY.Size = new Size(82, 20);
            numTransY.TabIndex = 13;
            numTransY.Text = "0";
            numTransY.ValueChanged += new EventHandler(BoxChangedCreateUndo);
            numTransY.MouseDown += new MouseEventHandler(box_MouseDown);
            // 
            // numTransZ
            // 
            numTransZ.BorderStyle = BorderStyle.FixedSingle;
            numTransZ.Integral = false;
            numTransZ.Location = new Point(235, 16);
            numTransZ.MaximumValue = 3.402823E+38F;
            numTransZ.MinimumValue = -3.402823E+38F;
            numTransZ.Name = "numTransZ";
            numTransZ.Size = new Size(82, 20);
            numTransZ.TabIndex = 14;
            numTransZ.Text = "0";
            numTransZ.ValueChanged += new EventHandler(BoxChangedCreateUndo);
            numTransZ.MouseDown += new MouseEventHandler(box_MouseDown);
            // 
            // CHR0Editor
            // 
            Controls.Add(grpTransAll);
            Controls.Add(grpTransform);
            MinimumSize = new Size(582, 78);
            Name = "CHR0Editor";
            Size = new Size(582, 78);
            grpTransform.ResumeLayout(false);
            grpTransform.PerformLayout();
            grpTransAll.ResumeLayout(false);
            grpTransAll.PerformLayout();
            ctxTools.ResumeLayout(false);
            ctxBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        public ModelEditorBase _mainWindow;

        public NumericInputBox[] _transBoxes = new NumericInputBox[9];
        //private AnimationFrame _tempFrame = AnimationFrame.Identity;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBoneNode TargetBone
        {
            get => _mainWindow.SelectedBone;
            set => _mainWindow.SelectedBone = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0MaterialRefNode TargetTexRef
        {
            get => _mainWindow.TargetTexRef;
            set => _mainWindow.TargetTexRef = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get => _mainWindow.CurrentFrame;
            set => _mainWindow.CurrentFrame = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get => _mainWindow.TargetModel;
            set => _mainWindow.TargetModel = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CHR0Node SelectedAnimation
        {
            get => _mainWindow.SelectedCHR0;
            set => _mainWindow.SelectedCHR0 = value;
        }

        public CHR0Editor()
        {
            InitializeComponent();
            _transBoxes[0] = numScaleX;
            numScaleX.Tag = 0;
            _transBoxes[1] = numScaleY;
            numScaleY.Tag = 1;
            _transBoxes[2] = numScaleZ;
            numScaleZ.Tag = 2;
            _transBoxes[3] = numRotX;
            numRotX.Tag = 3;
            _transBoxes[4] = numRotY;
            numRotY.Tag = 4;
            _transBoxes[5] = numRotZ;
            numRotZ.Tag = 5;
            _transBoxes[6] = numTransX;
            numTransX.Tag = 6;
            _transBoxes[7] = numTransY;
            numTransY.Tag = 7;
            _transBoxes[8] = numTransZ;
            numTransZ.Tag = 8;

            foreach (NumericInputBox box in _transBoxes)
            {
                box.KeyUp += (sender, e) =>
                {
                    // If the user has selected the whole text and wants to replace it with a negative number, allow them to enter a minus sign.
                    if (e.KeyCode == Keys.OemMinus)
                    {
                        NumericInputBox n = (NumericInputBox) sender;
                        if (n.SelectionLength == n.Text.Length)
                        {
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
            {
                return;
            }

            chkMoveBoneOnly.Visible =
                chkUpdateBindPose.Visible = chkUpdateBindPose.Enabled =
                    CurrentFrame < 1;

            chkMoveBoneOnly.Enabled = chkUpdateBindPose.Checked;

            grpTransAll.Enabled = TargetModel != null;
            btnInsert.Enabled =
                btnDelete.Enabled = btnClearAll.Enabled = CurrentFrame >= 1 && SelectedAnimation != null;
            grpTransform.Enabled = TargetBone != null;

            //9 transforms xyz for scale/rot/trans
            for (int i = 0; i < 9; i++)
            {
                ResetBox(i);
            }

            if (_mainWindow.InterpolationEditor != null &&
                _mainWindow.InterpolationEditor.Visible &&
                _mainWindow.TargetAnimType == NW4RAnimType.CHR)
            {
                if (_mainWindow.InterpolationEditor._targetNode != Entry)
                {
                    _mainWindow.InterpolationEditor.SetTarget(Entry);
                }
                else
                {
                    _mainWindow.InterpolationEditor.interpolationViewer.Invalidate();
                }
            }
        }

        public CHR0EntryNode Entry
        {
            get
            {
                CHR0EntryNode entry;
                if (TargetBone != null && SelectedAnimation != null && CurrentFrame >= 1 &&
                    (entry = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode) != null)
                {
                    return entry;
                }

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
                if (SelectedAnimation != null && CurrentFrame >= 1 &&
                    (entry = SelectedAnimation.FindChild(bone.Name, false) as CHR0EntryNode) != null)
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
                    box.Value = ((float*) &state)[index];
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
            float* p = (float*) &state;
            for (int i = 0; i < 9; i++)
            {
                if (_transBoxes[i].Value != p[i])
                {
                    _transBoxes[i].Value = p[i];
                    BoxChanged(_transBoxes[i], null);
                }
            }
        }

        public void BoxChangedCreateUndo(object sender, EventArgs e)
        {
            if (TargetBone == null)
            {
                return;
            }

            _mainWindow.BoneChange(TargetBone);

            //Only update for input boxes: Methods affecting multiple values call BoxChanged on their own.
            if (sender.GetType() == typeof(NumericInputBox))
            {
                BoxChanged(sender, null);
            }

            _mainWindow.BoneChange(TargetBone);
        }

        public unsafe void BoxChanged(object sender, EventArgs e)
        {
            if (TargetBone == null)
            {
                return;
            }

            NumericInputBox box = sender as NumericInputBox;
            int index = (int) box.Tag;

            IBoneNode bone = TargetBone;
            CHRAnimationFrame kf;
            float* pkf = (float*) &kf;

            if (SelectedAnimation != null && CurrentFrame >= 1)
            {
                CHR0EntryNode entry = SelectedAnimation.FindChild(bone.Name, false) as CHR0EntryNode;
                if (entry == null)
                {
                    if (!float.IsNaN(box.Value))
                    {
                        entry = SelectedAnimation.CreateEntry();
                        entry.Name = bone.Name;

                        FrameState state = bone.BindState;
                        float* p = (float*) &state;
                        for (int i = 0; i < 3; i++)
                        {
                            if (p[i] != 1.0f)
                            {
                                entry.SetKeyframe(i, 0, p[i]);
                            }
                        }

                        for (int i = 3; i < 9; i++)
                        {
                            if (p[i] != 0.0f)
                            {
                                entry.SetKeyframe(i, 0, p[i]);
                            }
                        }

                        entry.SetKeyframe(index, CurrentFrame - 1, box.Value);
                    }
                }
                else if (float.IsNaN(box.Value))
                {
                    entry.RemoveKeyframe(index, CurrentFrame - 1);
                }
                else
                {
                    entry.SetKeyframe(index, CurrentFrame - 1, box.Value);
                }

                if (_mainWindow.InterpolationEditor != null &&
                    _mainWindow.InterpolationEditor.Visible &&
                    _mainWindow.TargetAnimType == NW4RAnimType.CHR)
                {
                    if (_mainWindow.InterpolationEditor._targetNode != Entry)
                    {
                        _mainWindow.InterpolationEditor.SetTarget(Entry);
                    }
                    else
                    {
                        _mainWindow.InterpolationEditor.interpolationViewer.Invalidate();
                    }
                }
            }
            else
            {
                //Change base transform
                FrameState state = bone.BindState;
                float* p = (float*) &state;
                p[index] = float.IsNaN(box.Value) ? index > 2 ? 0.0f : 1.0f : box.Value;
                state.CalcTransforms();
                bone.BindState = state;

                //This will make the model not move with the bone
                //This will recalculate matrices and vertices/normals
                //AFTER a drag change is made, not during
                if (chkUpdateBindPose.Checked && !_mainWindow._boneSelection.IsMoving())
                {
                    bone.RecalcBindState(true, !chkMoveBoneOnly.Checked);
                }

                ((ResourceNode) bone).SignalPropertyChange();
            }

            _mainWindow.UpdateModel();

            ResetBox(index);
            //_mainWindow.KeyframePanel.UpdateKeyframe(CurrentFrame - 1);
            UpdateInterpolationEditor(box);

            if (_mainWindow.InterpolationEditor != null && _mainWindow.InterpolationEditor.Visible)
            {
                _mainWindow.InterpolationEditor.KeyframeChanged();
            }
        }

        private static readonly Dictionary<string, CHRAnimationFrame> _copyAllState =
            new Dictionary<string, CHRAnimationFrame>();

        private void btnCopyAll_Click(object sender, EventArgs e)
        {
            _copyAllState.Clear();

            if (CurrentFrame < 1)
            {
                foreach (MDL0BoneNode bone in TargetModel.BoneCache)
                {
                    CHRAnimationFrame frame = (CHRAnimationFrame) bone._bindState;
                    if (!AllTrans.Checked)
                    {
                        frame.Translation = new Vector3();
                    }

                    if (!AllRot.Checked)
                    {
                        frame.Rotation = new Vector3();
                    }

                    if (!AllScale.Checked)
                    {
                        frame.Scale = new Vector3(1);
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        frame.SetBool(i, true);
                    }

                    _copyAllState[bone._name] = frame;
                }
            }
            else
            {
                foreach (CHR0EntryNode entry in SelectedAnimation.Children)
                {
                    CHRAnimationFrame frame = entry.GetAnimFrame(CurrentFrame - 1);
                    if (!AllTrans.Checked)
                    {
                        frame.Translation = new Vector3();
                    }

                    if (!AllRot.Checked)
                    {
                        frame.Rotation = new Vector3();
                    }

                    if (!AllScale.Checked)
                    {
                        frame.Scale = new Vector3(1);
                    }

                    _copyAllState[entry._name] = frame;
                }
            }
        }

        public bool _onlyKeys;

        private unsafe void btnPasteAll_Click(object sender, EventArgs e)
        {
            if (_copyAllState.Count == 0)
            {
                return;
            }

            List<IBoneNode> o = new List<IBoneNode>();
            foreach (MDL0BoneNode bone in TargetModel.BoneCache)
            {
                if (_copyAllState.ContainsKey(bone._name))
                {
                    o.Add(bone);
                }
            }

            _mainWindow.BoneChange(o.ToArray());
            if (CurrentFrame < 1)
            {
                foreach (MDL0BoneNode bone in o)
                {
                    CHRAnimationFrame f = _copyAllState[bone._name];
                    if (!_onlyKeys)
                    {
                        if (AllTrans.Checked)
                        {
                            bone._bindState._translate = f.Translation;
                        }

                        if (AllRot.Checked)
                        {
                            bone._bindState._rotate = f.Rotation;
                        }

                        if (AllScale.Checked)
                        {
                            bone._bindState._scale = f.Scale;
                        }
                    }
                    else
                    {
                        FrameState s = bone._bindState;
                        float* sPtr = (float*) &f;
                        float* dPtr = (float*) &s;
                        for (int x = 0; x < 9; x++)
                        {
                            if (f.GetBool(x))
                            {
                                dPtr[x] = sPtr[x];
                            }
                        }

                        bone._bindState = s;
                    }

                    bone._bindState.CalcTransforms();
                    if (chkUpdateBindPose.Checked)
                    {
                        bone.RecalcBindState(true, !chkMoveBoneOnly.Checked);
                    }

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
                        entry = new CHR0EntryNode {Name = name};
                        SelectedAnimation.AddChild(entry);
                        entry.SetSize(SelectedAnimation.FrameCount, SelectedAnimation.Loop);
                    }

                    CHRAnimationFrame f = _copyAllState[entry._name];
                    int i = CurrentFrame - 1;
                    float* ptr = (float*) &f;
                    if (!_onlyKeys)
                    {
                        if (AllTrans.Checked)
                        {
                            entry.SetKeyframeOnlyTrans(i, f);
                        }

                        if (AllRot.Checked)
                        {
                            entry.SetKeyframeOnlyRot(i, f);
                        }

                        if (AllScale.Checked)
                        {
                            entry.SetKeyframeOnlyScale(i, f);
                        }
                    }
                    else
                    {
                        for (int x = 0; x < 9; x++)
                        {
                            if (f.GetBool(x))
                            {
                                entry.SetKeyframe(x, i, ptr[x]);
                            }
                        }
                    }
                }
            }

            _mainWindow.BoneChange(o.ToArray());
            _mainWindow.UpdateModel();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (CurrentFrame < 1)
            {
                return;
            }

            foreach (CHR0EntryNode entry in SelectedAnimation.Children)
            {
                if (AllTrans.Checked)
                {
                    entry.RemoveKeyframeOnlyTrans(CurrentFrame - 1);
                }

                if (AllRot.Checked)
                {
                    entry.RemoveKeyframeOnlyRot(CurrentFrame - 1);
                }

                if (AllScale.Checked)
                {
                    entry.RemoveKeyframeOnlyScale(CurrentFrame - 1);
                }
            }

            _mainWindow.UpdateModel();
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            ResourceNode group = (TargetModel as MDL0Node)?._boneGroup;
            if (@group == null)
            {
                return;
            }

            int keyCount = 0;
            List<CHR0EntryNode> badNodes = new List<CHR0EntryNode>();
            foreach (CHR0EntryNode entry in SelectedAnimation.Children)
            {
                if (group.FindChild(entry._name, true) == null)
                {
                    badNodes.Add(entry);
                }
                else
                {
                    keyCount += entry.Keyframes.Clean();
                }
            }

            int nodeCount = badNodes.Count;
            foreach (CHR0EntryNode n in badNodes)
            {
                n.Remove();
                n.Dispose();
            }

            MessageBox.Show(string.Format("{0} unused bone entr{2} and {1} redundant keyframe value{3} removed.",
                nodeCount, keyCount, nodeCount == 1 ? "y" : "ies", keyCount == 1 ? " was" : "s were"));
            UpdatePropDisplay();
        }

        private void ctxBox_Opening(object sender, CancelEventArgs e)
        {
            if (SelectedAnimation == null || numRotX.Enabled == false || numRotY.Enabled == false ||
                numRotZ.Enabled == false)
            {
                e.Cancel = true;
            }
        }

        public void UpdateInterpolationEditor(NumericInputBox box)
        {
            if (_mainWindow.InterpolationEditor == null || !_mainWindow.InterpolationEditor.Visible)
            {
                return;
            }

            _mainWindow.InterpolationEditor.interpolationViewer._updating = true;
            if (box.BackColor == Color.Yellow)
            {
                CHR0EntryNode entry = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
                KeyframeEntry kfe = entry.GetKeyframe((int) box.Tag, CurrentFrame - 1);
                _mainWindow.InterpolationEditor.SelectedKeyframe = kfe;
            }
            else
            {
                _mainWindow.InterpolationEditor.SelectedKeyframe = null;
            }

            _mainWindow.InterpolationEditor.interpolationViewer._updating = false;
        }

        public int type;

        private void box_MouseDown(object sender, MouseEventArgs e)
        {
            NumericInputBox box = sender as NumericInputBox;

            type = (int) box.Tag;

            if (_mainWindow.InterpolationEditor != null && _mainWindow.InterpolationEditor.Visible)
            {
                if (_mainWindow.InterpolationEditor.SelectedMode != type)
                {
                    _mainWindow.InterpolationEditor.SelectedMode = type;
                }

                UpdateInterpolationEditor(box);
            }

            if (e.Button == MouseButtons.Right)
            {
                if (box.Enabled)
                {
                    box.ContextMenuStrip = ctxBox;
                    Source.Text = box.Text;
                }
                else
                {
                    box.ContextMenuStrip = null;
                }
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
            {
                return;
            }

            KeyframeEntry kfe;
            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                {
                    if ((kfe = _target.GetKeyframe(type, x)) != null) //Check for a keyframe
                    {
                        kfe._value += 180;
                    }
                }

                ResetBox(type);
                _mainWindow.UpdateModel();
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
            {
                return;
            }

            KeyframeEntry kfe;
            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                {
                    if ((kfe = _target.GetKeyframe(type, x)) != null) //Check for a keyframe
                    {
                        kfe._value += 90;
                    }
                }

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
            {
                return;
            }

            KeyframeEntry kfe;
            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                {
                    if ((kfe = _target.GetKeyframe(type, x)) != null) //Check for a keyframe
                    {
                        kfe._value -= 90;
                    }
                }

                ResetBox(type);
                _mainWindow.UpdateModel();
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
            {
                return;
            }

            KeyframeEntry kfe;
            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                {
                    if ((kfe = _target.GetKeyframe(type, x)) != null) //Check for a keyframe
                    {
                        kfe._value += 45;
                    }
                }

                ResetBox(type);
                _mainWindow.UpdateModel();
            }
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
            {
                return;
            }

            KeyframeEntry kfe;
            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                {
                    if ((kfe = _target.GetKeyframe(type, x)) != null) //Check for a keyframe
                    {
                        kfe._value -= 45;
                    }
                }

                ResetBox(type);
                _mainWindow.UpdateModel();
            }
        }

        private void removeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHR0EntryNode _target = SelectedAnimation?.FindChild(TargetBone.Name, false) as CHR0EntryNode;
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
            {
                return;
            }

            EditAllKeyframesDialog ed = new EditAllKeyframesDialog();
            ed.ShowDialog(this, type, SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode);
            ResetBox(type);
            _mainWindow.UpdateModel();
        }

        private unsafe void btnCut_Click(object sender, EventArgs e)
        {
            CHRAnimationFrame frame = new CHRAnimationFrame();
            float* p = (float*) &frame;
            //copy transform from boxes instead of the actual animation.
            BoxChangedCreateUndo(this, null);

            for (int i = 0; i < 9; i++)
            {
                frame.SetBool(i, false);
                if (!FrameScale.Checked && i < 3)
                {
                    p[i] = 1;
                }
                else if (
                    FrameScale.Checked && i < 3 ||
                    FrameRot.Checked && i >= 3 && i < 6 ||
                    FrameTrans.Checked && i >= 6)
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
            float* p = (float*) &frame;

            for (int i = 0; i < 9; i++)
            {
                frame.SetBool(i, false);
                if (!FrameScale.Checked && i < 3)
                {
                    p[i] = 1;
                }
                else if (
                    FrameScale.Checked && i < 3 ||
                    FrameRot.Checked && i >= 3 && i < 6 ||
                    FrameTrans.Checked && i >= 6)
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
                if (o is CHRAnimationFrame)
                {
                    CHRAnimationFrame frame = (CHRAnimationFrame) o;

                    float* p = (float*) &frame;

                    BoxChangedCreateUndo(this, null);

                    for (int i = 0; i < 9; i++)
                    {
                        if (FrameScale.Checked && i < 3 ||
                            FrameRot.Checked && i >= 3 && i < 6 ||
                            FrameTrans.Checked && i >= 6)
                        {
                            if (_transBoxes[i].Value != p[i] && (!_onlyKeys || frame.GetBool(i)))
                            {
                                _transBoxes[i].Value = p[i];
                                BoxChanged(_transBoxes[i], null);
                            }
                        }
                    }
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null || CurrentFrame < 1)
            {
                return;
            }

            SelectedAnimation.InsertKeyframe(CurrentFrame - 1);
            _mainWindow.UpdateModel();

            _mainWindow.Updating = true;
            _mainWindow.PlaybackPanel.numFrameIndex.Maximum++;
            _mainWindow.PlaybackPanel.numTotalFrames.Value++;
            _mainWindow._maxFrame++;
            _mainWindow.Updating = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null || CurrentFrame < 1)
            {
                return;
            }

            SelectedAnimation.DeleteKeyframe(CurrentFrame - 1);
            _mainWindow.UpdateModel();

            _mainWindow.Updating = true;
            _mainWindow.PlaybackPanel.numFrameIndex.Maximum--;
            _mainWindow.PlaybackPanel.numTotalFrames.Value--;
            _mainWindow._maxFrame--;
            _mainWindow.Updating = false;
        }

        private void bakeVertexPositionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TargetModel is MDL0Node && TargetModel.Objects != null)
            {
                foreach (MDL0ObjectNode o in TargetModel.Objects)
                {
                    for (int i = 0; i < o._manager._vertices.Count; i++)
                    {
                        Vertex3 vec = o._manager._vertices[i];
                        o._vertexNode.Vertices[o._manager._vertices[i].Facepoints[0]._vertexIndex] =
                            vec.WeightedPosition;
                    }

                    o._vertexNode.ForceRebuild = true;
                    if (o._vertexNode.Format == WiiVertexComponentType.Float)
                    {
                        o._vertexNode.ForceFloat = true;
                    }
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