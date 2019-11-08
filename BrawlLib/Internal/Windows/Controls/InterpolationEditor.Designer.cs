using BrawlLib.Internal.Windows.Controls;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    partial class InterpolationEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkUseOut = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.numOutFrame = new NumericInputBox();
            this.label10 = new System.Windows.Forms.Label();
            this.numInFrame = new NumericInputBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.numOutTan = new NumericInputBox();
            this.numOutVal = new NumericInputBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numInTan = new NumericInputBox();
            this.numInValue = new NumericInputBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkViewOne = new System.Windows.Forms.ToolStripMenuItem();
            this.chkRenderTans = new System.Windows.Forms.ToolStripMenuItem();
            this.chkTanStrength = new System.Windows.Forms.ToolStripMenuItem();
            this.chkTanAngle = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkSetFrame = new System.Windows.Forms.ToolStripMenuItem();
            this.chkSyncStartEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.chkKeyDrag = new System.Windows.Forms.ToolStripMenuItem();
            this.chkGenTans = new System.Windows.Forms.ToolStripMenuItem();
            this.mItem_genTan_alterSelTanOnDrag = new System.Windows.Forms.ToolStripMenuItem();
            this.mItem_genTan_alterAdjTan = new System.Windows.Forms.ToolStripMenuItem();
            this.mItem_genTan_alterAdjTan_OnSet = new System.Windows.Forms.ToolStripMenuItem();
            this.mItem_genTan_alterAdjTan_OnDel = new System.Windows.Forms.ToolStripMenuItem();
            this.mItem_genTan_alterAdjTan_OnDrag = new System.Windows.Forms.ToolStripMenuItem();
            this.editKeyframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.slopeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkLinear = new System.Windows.Forms.ToolStripMenuItem();
            this.chkSmooth = new System.Windows.Forms.ToolStripMenuItem();
            this.chkFlat = new System.Windows.Forms.ToolStripMenuItem();
            this.BreakKey = new System.Windows.Forms.ToolStripMenuItem();
            this.cbTransform = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.numPrecision = new NumericInputBox();
            this.nibTanLen = new NumericInputBox();
            this.numFrameVal = new NumericInputBox();
            this.interpolationViewer = new InterpolationViewer();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(570, 63);
            this.panel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkUseOut);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.numOutFrame);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.numInFrame);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.numOutTan);
            this.groupBox1.Controls.Add(this.numOutVal);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numInTan);
            this.groupBox1.Controls.Add(this.numInValue);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(98, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(472, 63);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Edit Keyframe";
            // 
            // chkUseOut
            // 
            this.chkUseOut.AutoSize = true;
            this.chkUseOut.Location = new System.Drawing.Point(243, 21);
            this.chkUseOut.Name = "chkUseOut";
            this.chkUseOut.Size = new System.Drawing.Size(15, 14);
            this.chkUseOut.TabIndex = 16;
            this.chkUseOut.UseVisualStyleBackColor = true;
            this.chkUseOut.CheckedChanged += new System.EventHandler(this.chkUseOut_CheckedChanged);
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Location = new System.Drawing.Point(351, 17);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 20);
            this.label11.TabIndex = 15;
            this.label11.Text = "Frame:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numOutFrame
            // 
            this.numOutFrame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numOutFrame.Integral = true;
            this.numOutFrame.Location = new System.Drawing.Point(407, 17);
            this.numOutFrame.MaximumValue = 3.402823E+38F;
            this.numOutFrame.MinimumValue = -3.402823E+38F;
            this.numOutFrame.Name = "numOutFrame";
            this.numOutFrame.Size = new System.Drawing.Size(60, 20);
            this.numOutFrame.TabIndex = 14;
            this.numOutFrame.Text = "0";
            this.numOutFrame.ValueChanged += new System.EventHandler(this.numOutFrame_ValueChanged);
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(121, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 20);
            this.label10.TabIndex = 13;
            this.label10.Text = "Frame:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numInFrame
            // 
            this.numInFrame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numInFrame.Integral = true;
            this.numInFrame.Location = new System.Drawing.Point(177, 17);
            this.numInFrame.MaximumValue = 3.402823E+38F;
            this.numInFrame.MinimumValue = -3.402823E+38F;
            this.numInFrame.Name = "numInFrame";
            this.numInFrame.Size = new System.Drawing.Size(60, 20);
            this.numInFrame.TabIndex = 12;
            this.numInFrame.Text = "0";
            this.numInFrame.ValueChanged += new System.EventHandler(this.numInFrame_ValueChanged);
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(236, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 20);
            this.label6.TabIndex = 11;
            this.label6.Text = "Out";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(351, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 20);
            this.label7.TabIndex = 10;
            this.label7.Text = "Tangent:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(236, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 20);
            this.label8.TabIndex = 9;
            this.label8.Text = "Value:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numOutTan
            // 
            this.numOutTan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numOutTan.Integral = false;
            this.numOutTan.Location = new System.Drawing.Point(407, 36);
            this.numOutTan.MaximumValue = 3.402823E+38F;
            this.numOutTan.MinimumValue = -3.402823E+38F;
            this.numOutTan.Name = "numOutTan";
            this.numOutTan.Size = new System.Drawing.Size(60, 20);
            this.numOutTan.TabIndex = 7;
            this.numOutTan.Text = "0";
            this.numOutTan.ValueChanged += new System.EventHandler(this.numOutTan_ValueChanged);
            // 
            // numOutVal
            // 
            this.numOutVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numOutVal.Integral = false;
            this.numOutVal.Location = new System.Drawing.Point(292, 36);
            this.numOutVal.MaximumValue = 3.402823E+38F;
            this.numOutVal.MinimumValue = -3.402823E+38F;
            this.numOutVal.Name = "numOutVal";
            this.numOutVal.Size = new System.Drawing.Size(60, 20);
            this.numOutVal.TabIndex = 8;
            this.numOutVal.Text = "0";
            this.numOutVal.ValueChanged += new System.EventHandler(this.numOutVal_ValueChanged);
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(6, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "In";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(121, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Tangent:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(6, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Value:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numInTan
            // 
            this.numInTan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numInTan.Integral = false;
            this.numInTan.Location = new System.Drawing.Point(177, 36);
            this.numInTan.MaximumValue = 3.402823E+38F;
            this.numInTan.MinimumValue = -3.402823E+38F;
            this.numInTan.Name = "numInTan";
            this.numInTan.Size = new System.Drawing.Size(60, 20);
            this.numInTan.TabIndex = 0;
            this.numInTan.Text = "0";
            this.numInTan.ValueChanged += new System.EventHandler(this.numInTan_ValueChanged);
            // 
            // numInValue
            // 
            this.numInValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numInValue.Integral = false;
            this.numInValue.Location = new System.Drawing.Point(62, 36);
            this.numInValue.MaximumValue = 3.402823E+38F;
            this.numInValue.MinimumValue = -3.402823E+38F;
            this.numInValue.Name = "numInValue";
            this.numInValue.Size = new System.Drawing.Size(60, 20);
            this.numInValue.TabIndex = 2;
            this.numInValue.Text = "0";
            this.numInValue.ValueChanged += new System.EventHandler(this.numInValue_ValueChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.editKeyframeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(98, 63);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkViewOne,
            this.chkRenderTans,
            this.chkTanStrength,
            this.chkTanAngle});
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(85, 19);
            this.displayToolStripMenuItem.Text = "Display";
            // 
            // chkViewOne
            // 
            this.chkViewOne.CheckOnClick = true;
            this.chkViewOne.Name = "chkViewOne";
            this.chkViewOne.Size = new System.Drawing.Size(294, 22);
            this.chkViewOne.Text = "View keyframe only";
            this.chkViewOne.CheckedChanged += new System.EventHandler(this.chkAllKeys_CheckedChanged);
            // 
            // chkRenderTans
            // 
            this.chkRenderTans.Checked = true;
            this.chkRenderTans.CheckOnClick = true;
            this.chkRenderTans.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRenderTans.Name = "chkRenderTans";
            this.chkRenderTans.Size = new System.Drawing.Size(294, 22);
            this.chkRenderTans.Text = "Show tangents";
            this.chkRenderTans.CheckedChanged += new System.EventHandler(this.chkRenderTans_CheckedChanged);
            // 
            // chkTanStrength
            // 
            this.chkTanStrength.Name = "chkTanStrength";
            this.chkTanStrength.Size = new System.Drawing.Size(294, 22);
            this.chkTanStrength.Text = "Show tangent strength";
            this.chkTanStrength.Visible = false;
            this.chkTanStrength.Click += new System.EventHandler(this.chkTanStrength_Click);
            // 
            // chkTanAngle
            // 
            this.chkTanAngle.Name = "chkTanAngle";
            this.chkTanAngle.Size = new System.Drawing.Size(294, 22);
            this.chkTanAngle.Text = "Tangent angle (deg) instead of slope (y/x)";
            this.chkTanAngle.Visible = false;
            this.chkTanAngle.Click += new System.EventHandler(this.chkTanAngle_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkSetFrame,
            this.chkSyncStartEnd,
            this.chkKeyDrag,
            this.chkGenTans});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(85, 19);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // chkSetFrame
            // 
            this.chkSetFrame.Checked = true;
            this.chkSetFrame.CheckOnClick = true;
            this.chkSetFrame.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSetFrame.Name = "chkSetFrame";
            this.chkSetFrame.Size = new System.Drawing.Size(228, 22);
            this.chkSetFrame.Text = "Set frame on keyframe select";
            // 
            // chkSyncStartEnd
            // 
            this.chkSyncStartEnd.CheckOnClick = true;
            this.chkSyncStartEnd.Name = "chkSyncStartEnd";
            this.chkSyncStartEnd.Size = new System.Drawing.Size(228, 22);
            this.chkSyncStartEnd.Text = "Sync start and end keyframes";
            this.chkSyncStartEnd.CheckedChanged += new System.EventHandler(this.chkSyncStartEnd_CheckedChanged);
            // 
            // chkKeyDrag
            // 
            this.chkKeyDrag.CheckOnClick = true;
            this.chkKeyDrag.Name = "chkKeyDrag";
            this.chkKeyDrag.Size = new System.Drawing.Size(228, 22);
            this.chkKeyDrag.Text = "Draggable keyframes";
            this.chkKeyDrag.CheckedChanged += new System.EventHandler(this.chkKeyDrag_CheckedChanged);
            // 
            // chkGenTans
            // 
            this.chkGenTans.CheckOnClick = true;
            this.chkGenTans.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mItem_genTan_alterSelTanOnDrag,
            this.mItem_genTan_alterAdjTan});
            this.chkGenTans.Name = "chkGenTans";
            this.chkGenTans.Size = new System.Drawing.Size(228, 22);
            this.chkGenTans.Text = "Generate tangents";
            this.chkGenTans.CheckedChanged += new System.EventHandler(this.chkGenTans_CheckedChanged);
            // 
            // mItem_genTan_alterSelTanOnDrag
            // 
            this.mItem_genTan_alterSelTanOnDrag.Checked = true;
            this.mItem_genTan_alterSelTanOnDrag.CheckOnClick = true;
            this.mItem_genTan_alterSelTanOnDrag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mItem_genTan_alterSelTanOnDrag.Name = "mItem_genTan_alterSelTanOnDrag";
            this.mItem_genTan_alterSelTanOnDrag.Size = new System.Drawing.Size(240, 22);
            this.mItem_genTan_alterSelTanOnDrag.Text = "Alter Selected Tangent On Drag";
            this.mItem_genTan_alterSelTanOnDrag.CheckedChanged += new System.EventHandler(this.mItem_genTan_alterSelTanOnDrag_CheckedChanged);
            // 
            // mItem_genTan_alterAdjTan
            // 
            this.mItem_genTan_alterAdjTan.Checked = true;
            this.mItem_genTan_alterAdjTan.CheckOnClick = true;
            this.mItem_genTan_alterAdjTan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mItem_genTan_alterAdjTan.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mItem_genTan_alterAdjTan_OnSet,
            this.mItem_genTan_alterAdjTan_OnDel,
            this.mItem_genTan_alterAdjTan_OnDrag});
            this.mItem_genTan_alterAdjTan.Name = "mItem_genTan_alterAdjTan";
            this.mItem_genTan_alterAdjTan.Size = new System.Drawing.Size(240, 22);
            this.mItem_genTan_alterAdjTan.Text = "Alter Adjacent Tangents";
            this.mItem_genTan_alterAdjTan.CheckedChanged += new System.EventHandler(this.mItem_genTan_alterAdjTan_CheckedChanged);
            // 
            // mItem_genTan_alterAdjTan_OnSet
            // 
            this.mItem_genTan_alterAdjTan_OnSet.Checked = true;
            this.mItem_genTan_alterAdjTan_OnSet.CheckOnClick = true;
            this.mItem_genTan_alterAdjTan_OnSet.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mItem_genTan_alterAdjTan_OnSet.Name = "mItem_genTan_alterAdjTan_OnSet";
            this.mItem_genTan_alterAdjTan_OnSet.Size = new System.Drawing.Size(163, 22);
            this.mItem_genTan_alterAdjTan_OnSet.Text = "KeyFrame Create";
            this.mItem_genTan_alterAdjTan_OnSet.CheckedChanged += new System.EventHandler(this.mItem_genTan_alterAdjTan_OnSet_CheckedChanged);
            // 
            // mItem_genTan_alterAdjTan_OnDel
            // 
            this.mItem_genTan_alterAdjTan_OnDel.Checked = true;
            this.mItem_genTan_alterAdjTan_OnDel.CheckOnClick = true;
            this.mItem_genTan_alterAdjTan_OnDel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mItem_genTan_alterAdjTan_OnDel.Name = "mItem_genTan_alterAdjTan_OnDel";
            this.mItem_genTan_alterAdjTan_OnDel.Size = new System.Drawing.Size(163, 22);
            this.mItem_genTan_alterAdjTan_OnDel.Text = "KeyFrame Delete";
            this.mItem_genTan_alterAdjTan_OnDel.CheckedChanged += new System.EventHandler(this.mItem_genTan_alterAdjTan_OnDel_CheckedChanged);
            // 
            // mItem_genTan_alterAdjTan_OnDrag
            // 
            this.mItem_genTan_alterAdjTan_OnDrag.Checked = true;
            this.mItem_genTan_alterAdjTan_OnDrag.CheckOnClick = true;
            this.mItem_genTan_alterAdjTan_OnDrag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mItem_genTan_alterAdjTan_OnDrag.Name = "mItem_genTan_alterAdjTan_OnDrag";
            this.mItem_genTan_alterAdjTan_OnDrag.Size = new System.Drawing.Size(163, 22);
            this.mItem_genTan_alterAdjTan_OnDrag.Text = "KeyFrame Drag";
            this.mItem_genTan_alterAdjTan_OnDrag.CheckedChanged += new System.EventHandler(this.mItem_genTan_alterAdjTan_OnDrag_CheckedChanged);
            // 
            // editKeyframeToolStripMenuItem
            // 
            this.editKeyframeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slopeToolStripMenuItem,
            this.BreakKey});
            this.editKeyframeToolStripMenuItem.Name = "editKeyframeToolStripMenuItem";
            this.editKeyframeToolStripMenuItem.Size = new System.Drawing.Size(85, 19);
            this.editKeyframeToolStripMenuItem.Text = "Edit Keyframe";
            // 
            // slopeToolStripMenuItem
            // 
            this.slopeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkLinear,
            this.chkSmooth,
            this.chkFlat});
            this.slopeToolStripMenuItem.Name = "slopeToolStripMenuItem";
            this.slopeToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.slopeToolStripMenuItem.Text = "Slope";
            this.slopeToolStripMenuItem.Visible = false;
            // 
            // chkLinear
            // 
            this.chkLinear.Name = "chkLinear";
            this.chkLinear.Size = new System.Drawing.Size(148, 22);
            this.chkLinear.Text = "Make Linear";
            this.chkLinear.Click += new System.EventHandler(this.chkLinear_Click);
            // 
            // chkSmooth
            // 
            this.chkSmooth.Name = "chkSmooth";
            this.chkSmooth.Size = new System.Drawing.Size(148, 22);
            this.chkSmooth.Text = "Make Smooth";
            this.chkSmooth.Click += new System.EventHandler(this.chkSmooth_Click);
            // 
            // chkFlat
            // 
            this.chkFlat.Name = "chkFlat";
            this.chkFlat.Size = new System.Drawing.Size(148, 22);
            this.chkFlat.Text = "Make Flat";
            this.chkFlat.Click += new System.EventHandler(this.chkFlat_Click);
            // 
            // BreakKey
            // 
            this.BreakKey.Name = "BreakKey";
            this.BreakKey.Size = new System.Drawing.Size(141, 22);
            this.BreakKey.Text = "Break In/Out";
            this.BreakKey.Click += new System.EventHandler(this.chkBreakKey_Click);
            // 
            // cbTransform
            // 
            this.cbTransform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTransform.FormattingEnabled = true;
            this.cbTransform.Items.AddRange(new object[] {
            "Scale X",
            "Scale Y",
            "Scale Z",
            "Rotation X",
            "Rotation Y",
            "Rotation Z",
            "Translation X",
            "Translation Y",
            "Translation Z"});
            this.cbTransform.Location = new System.Drawing.Point(3, 4);
            this.cbTransform.Name = "cbTransform";
            this.cbTransform.Size = new System.Drawing.Size(121, 21);
            this.cbTransform.TabIndex = 6;
            this.cbTransform.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(476, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Frame:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(350, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Tangent Scale:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.numPrecision);
            this.panel2.Controls.Add(this.cbTransform);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.nibTanLen);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.numFrameVal);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 171);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(570, 29);
            this.panel2.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(178, 5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(134, 20);
            this.label9.TabIndex = 9;
            this.label9.Text = "Interpolation Precision:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPrecision
            // 
            this.numPrecision.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numPrecision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPrecision.Integral = false;
            this.numPrecision.Location = new System.Drawing.Point(311, 5);
            this.numPrecision.MaximumValue = 3.402823E+38F;
            this.numPrecision.MinimumValue = 0F;
            this.numPrecision.Name = "numPrecision";
            this.numPrecision.Size = new System.Drawing.Size(40, 20);
            this.numPrecision.TabIndex = 8;
            this.numPrecision.Text = "4";
            this.numPrecision.ValueChanged += new System.EventHandler(this.numPrecision_ValueChanged);
            // 
            // nibTanLen
            // 
            this.nibTanLen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nibTanLen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nibTanLen.Integral = false;
            this.nibTanLen.Location = new System.Drawing.Point(437, 5);
            this.nibTanLen.MaximumValue = 3.402823E+38F;
            this.nibTanLen.MinimumValue = 0F;
            this.nibTanLen.Name = "nibTanLen";
            this.nibTanLen.Size = new System.Drawing.Size(40, 20);
            this.nibTanLen.TabIndex = 6;
            this.nibTanLen.Text = "3";
            this.nibTanLen.ValueChanged += new System.EventHandler(this.numTanLen_ValueChanged);
            // 
            // numFrameVal
            // 
            this.numFrameVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numFrameVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numFrameVal.Integral = false;
            this.numFrameVal.Location = new System.Drawing.Point(525, 5);
            this.numFrameVal.MaximumValue = 65536F;
            this.numFrameVal.MinimumValue = 0F;
            this.numFrameVal.Name = "numFrameVal";
            this.numFrameVal.Size = new System.Drawing.Size(40, 20);
            this.numFrameVal.TabIndex = 4;
            this.numFrameVal.Text = "0";
            this.numFrameVal.ValueChanged += new System.EventHandler(this.numFrameVal_ValueChanged);
            // 
            // interpolationViewer
            // 
            this.interpolationViewer.AlterAdjTangent_OnSelectedDrag = true;
            this.interpolationViewer.AlterSelectedTangent_OnDrag = true;
            this.interpolationViewer.DisplayAllKeyframes = true;
            this.interpolationViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.interpolationViewer.DrawTangents = true;
            this.interpolationViewer.GenerateTangents = false;
            this.interpolationViewer.KeyDraggingAllowed = false;
            this.interpolationViewer.Location = new System.Drawing.Point(0, 63);
            this.interpolationViewer.Name = "interpolationViewer";
            this.interpolationViewer.Size = new System.Drawing.Size(570, 108);
            this.interpolationViewer.SyncStartEnd = false;
            this.interpolationViewer.TabIndex = 3;
            this.interpolationViewer.TangentLength = 5F;
            // 
            // InterpolationEditor
            // 
            this.Controls.Add(this.interpolationViewer);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(570, 200);
            this.Name = "InterpolationEditor";
            this.Size = new System.Drawing.Size(570, 200);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private global::System.Windows.Forms.Panel panel1;
        private global::System.Windows.Forms.GroupBox groupBox1;
        private global::System.Windows.Forms.Label label1;
        private global::BrawlLib.Internal.Windows.Controls.NumericInputBox numFrameVal;
        private global::System.Windows.Forms.Label label3;
        private global::System.Windows.Forms.Label label2;
        private global::BrawlLib.Internal.Windows.Controls.NumericInputBox numInTan;
        private global::BrawlLib.Internal.Windows.Controls.NumericInputBox numInValue;
        private global::System.Windows.Forms.ComboBox cbTransform;
        public InterpolationViewer interpolationViewer;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem displayToolStripMenuItem;
        private ToolStripMenuItem chkViewOne;
        private ToolStripMenuItem chkRenderTans;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem chkSetFrame;
        private ToolStripMenuItem chkSyncStartEnd;
        private ToolStripMenuItem chkKeyDrag;
        private ToolStripMenuItem chkGenTans;
        private NumericInputBox nibTanLen;
        private ToolStripMenuItem mItem_genTan_alterSelTanOnDrag;
        private ToolStripMenuItem mItem_genTan_alterAdjTan;
        private ToolStripMenuItem mItem_genTan_alterAdjTan_OnSet;
        private ToolStripMenuItem mItem_genTan_alterAdjTan_OnDel;
        private ToolStripMenuItem mItem_genTan_alterAdjTan_OnDrag;
        private Label label6;
        private Label label7;
        private Label label8;
        private NumericInputBox numOutTan;
        private NumericInputBox numOutVal;
        private Label label5;
        private ToolStripMenuItem chkTanStrength;
        private Label label4;
        private Panel panel2;
        private ToolStripMenuItem chkTanAngle;
        private ToolStripMenuItem editKeyframeToolStripMenuItem;
        private ToolStripMenuItem slopeToolStripMenuItem;
        private ToolStripMenuItem chkLinear;
        private ToolStripMenuItem chkSmooth;
        private ToolStripMenuItem chkFlat;
        private ToolStripMenuItem BreakKey;
        private NumericInputBox numPrecision;
        private Label label9;
        private CheckBox chkUseOut;
        private Label label11;
        private NumericInputBox numOutFrame;
        private Label label10;
        private NumericInputBox numInFrame;
    }
}
