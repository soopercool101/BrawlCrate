namespace Ikarus.UI
{
    partial class HurtboxEditor
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
            this.components = new System.ComponentModel.Container();
            this.addCustomAmountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.editRawTangentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.numRadius = new System.Windows.Forms.NumericInputBox();
            this.numRegion = new System.Windows.Forms.NumericInputBox();
            this.numRotX = new System.Windows.Forms.NumericInputBox();
            this.numRotY = new System.Windows.Forms.NumericInputBox();
            this.numRotZ = new System.Windows.Forms.NumericInputBox();
            this.numTransX = new System.Windows.Forms.NumericInputBox();
            this.numTransY = new System.Windows.Forms.NumericInputBox();
            this.numTransZ = new System.Windows.Forms.NumericInputBox();
            this.lblTrans = new System.Windows.Forms.Label();
            this.lblRot = new System.Windows.Forms.Label();
            this.lblScale = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectedBone = new System.Windows.Forms.ComboBox();
            this.SelectedZone = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ctxBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // addCustomAmountToolStripMenuItem
            // 
            this.addCustomAmountToolStripMenuItem.Name = "addCustomAmountToolStripMenuItem";
            this.addCustomAmountToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.addCustomAmountToolStripMenuItem.Text = "Edit All...";
            // 
            // ctxBox
            // 
            this.ctxBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Source,
            this.toolStripSeparator1,
            this.add,
            this.subtract,
            this.removeAllToolStripMenuItem,
            this.addCustomAmountToolStripMenuItem,
            this.editRawTangentToolStripMenuItem});
            this.ctxBox.Name = "ctxBox";
            this.ctxBox.Size = new System.Drawing.Size(167, 142);
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
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem4.Text = "+90";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem7.Text = "+45";
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
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(97, 22);
            this.toolStripMenuItem6.Text = "-90";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(97, 22);
            this.toolStripMenuItem8.Text = "-45";
            // 
            // removeAllToolStripMenuItem
            // 
            this.removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
            this.removeAllToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.removeAllToolStripMenuItem.Text = "Remove All";
            // 
            // editRawTangentToolStripMenuItem
            // 
            this.editRawTangentToolStripMenuItem.Name = "editRawTangentToolStripMenuItem";
            this.editRawTangentToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.editRawTangentToolStripMenuItem.Text = "Edit Raw Tangent";
            // 
            // numRadius
            // 
            this.numRadius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRadius.Integral = false;
            this.numRadius.Location = new System.Drawing.Point(75, 54);
            this.numRadius.MaximumValue = 3.402823E+38F;
            this.numRadius.MinimumValue = -3.402823E+38F;
            this.numRadius.Name = "numRadius";
            this.numRadius.Size = new System.Drawing.Size(70, 20);
            this.numRadius.TabIndex = 45;
            this.numRadius.Text = "0";
            this.numRadius.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numRegion
            // 
            this.numRegion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRegion.Integral = false;
            this.numRegion.Location = new System.Drawing.Point(213, 54);
            this.numRegion.MaximumValue = 3.402823E+38F;
            this.numRegion.MinimumValue = -3.402823E+38F;
            this.numRegion.Name = "numRegion";
            this.numRegion.Size = new System.Drawing.Size(70, 20);
            this.numRegion.TabIndex = 47;
            this.numRegion.Text = "0";
            this.numRegion.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numRotX
            // 
            this.numRotX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotX.Integral = false;
            this.numRotX.Location = new System.Drawing.Point(75, 35);
            this.numRotX.MaximumValue = 3.402823E+38F;
            this.numRotX.MinimumValue = -3.402823E+38F;
            this.numRotX.Name = "numRotX";
            this.numRotX.Size = new System.Drawing.Size(70, 20);
            this.numRotX.TabIndex = 42;
            this.numRotX.Text = "0";
            this.numRotX.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numRotY
            // 
            this.numRotY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotY.Integral = false;
            this.numRotY.Location = new System.Drawing.Point(144, 35);
            this.numRotY.MaximumValue = 3.402823E+38F;
            this.numRotY.MinimumValue = -3.402823E+38F;
            this.numRotY.Name = "numRotY";
            this.numRotY.Size = new System.Drawing.Size(70, 20);
            this.numRotY.TabIndex = 43;
            this.numRotY.Text = "0";
            this.numRotY.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numRotZ
            // 
            this.numRotZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotZ.Integral = false;
            this.numRotZ.Location = new System.Drawing.Point(213, 35);
            this.numRotZ.MaximumValue = 3.402823E+38F;
            this.numRotZ.MinimumValue = -3.402823E+38F;
            this.numRotZ.Name = "numRotZ";
            this.numRotZ.Size = new System.Drawing.Size(70, 20);
            this.numRotZ.TabIndex = 44;
            this.numRotZ.Text = "0";
            this.numRotZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numTransX
            // 
            this.numTransX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTransX.Integral = false;
            this.numTransX.Location = new System.Drawing.Point(75, 16);
            this.numTransX.MaximumValue = 3.402823E+38F;
            this.numTransX.MinimumValue = -3.402823E+38F;
            this.numTransX.Name = "numTransX";
            this.numTransX.Size = new System.Drawing.Size(70, 20);
            this.numTransX.TabIndex = 36;
            this.numTransX.Text = "0";
            this.numTransX.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numTransY
            // 
            this.numTransY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTransY.Integral = false;
            this.numTransY.Location = new System.Drawing.Point(144, 16);
            this.numTransY.MaximumValue = 3.402823E+38F;
            this.numTransY.MinimumValue = -3.402823E+38F;
            this.numTransY.Name = "numTransY";
            this.numTransY.Size = new System.Drawing.Size(70, 20);
            this.numTransY.TabIndex = 40;
            this.numTransY.Text = "0";
            this.numTransY.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numTransZ
            // 
            this.numTransZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTransZ.Integral = false;
            this.numTransZ.Location = new System.Drawing.Point(213, 16);
            this.numTransZ.MaximumValue = 3.402823E+38F;
            this.numTransZ.MinimumValue = -3.402823E+38F;
            this.numTransZ.Name = "numTransZ";
            this.numTransZ.Size = new System.Drawing.Size(70, 20);
            this.numTransZ.TabIndex = 41;
            this.numTransZ.Text = "0";
            this.numTransZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // lblTrans
            // 
            this.lblTrans.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTrans.Location = new System.Drawing.Point(6, 16);
            this.lblTrans.Name = "lblTrans";
            this.lblTrans.Size = new System.Drawing.Size(70, 20);
            this.lblTrans.TabIndex = 37;
            this.lblTrans.Text = "Translation:";
            this.lblTrans.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRot
            // 
            this.lblRot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRot.Location = new System.Drawing.Point(6, 35);
            this.lblRot.Name = "lblRot";
            this.lblRot.Size = new System.Drawing.Size(70, 20);
            this.lblRot.TabIndex = 38;
            this.lblRot.Text = "Stretch:";
            this.lblRot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblScale
            // 
            this.lblScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblScale.Location = new System.Drawing.Point(6, 54);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(70, 20);
            this.lblScale.TabIndex = 39;
            this.lblScale.Text = "Radius:";
            this.lblScale.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(144, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 48;
            this.label1.Text = "Region:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(144, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 20);
            this.label2.TabIndex = 50;
            this.label2.Text = "Zone:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(6, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 20);
            this.label3.TabIndex = 49;
            this.label3.Text = "Bone:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SelectedBone
            // 
            this.SelectedBone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectedBone.FormattingEnabled = true;
            this.SelectedBone.Location = new System.Drawing.Point(75, 72);
            this.SelectedBone.Name = "SelectedBone";
            this.SelectedBone.Size = new System.Drawing.Size(70, 21);
            this.SelectedBone.TabIndex = 51;
            this.SelectedBone.SelectedIndexChanged += new System.EventHandler(this.BoxChanged);
            // 
            // SelectedZone
            // 
            this.SelectedZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectedZone.FormattingEnabled = true;
            this.SelectedZone.Location = new System.Drawing.Point(213, 72);
            this.SelectedZone.Name = "SelectedZone";
            this.SelectedZone.Size = new System.Drawing.Size(70, 21);
            this.SelectedZone.TabIndex = 52;
            this.SelectedZone.SelectedIndexChanged += new System.EventHandler(this.BoxChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(213, 0);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(65, 17);
            this.checkBox1.TabIndex = 53;
            this.checkBox1.Text = "Enabled";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblTrans);
            this.groupBox1.Controls.Add(this.lblScale);
            this.groupBox1.Controls.Add(this.lblRot);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numTransZ);
            this.groupBox1.Controls.Add(this.numRadius);
            this.groupBox1.Controls.Add(this.numTransY);
            this.groupBox1.Controls.Add(this.numRegion);
            this.groupBox1.Controls.Add(this.numTransX);
            this.groupBox1.Controls.Add(this.numRotX);
            this.groupBox1.Controls.Add(this.numRotZ);
            this.groupBox1.Controls.Add(this.numRotY);
            this.groupBox1.Controls.Add(this.SelectedBone);
            this.groupBox1.Controls.Add(this.SelectedZone);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(289, 99);
            this.groupBox1.TabIndex = 54;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Edit Hurtbox";
            // 
            // HurtboxEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "HurtboxEditor";
            this.Size = new System.Drawing.Size(289, 99);
            this.ctxBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem addCustomAmountToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ctxBox;
        private System.Windows.Forms.ToolStripMenuItem Source;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem add;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem subtract;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem removeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editRawTangentToolStripMenuItem;
        public System.Windows.Forms.NumericInputBox numRadius;
        public System.Windows.Forms.NumericInputBox numRegion;
        public System.Windows.Forms.NumericInputBox numRotX;
        public System.Windows.Forms.NumericInputBox numRotY;
        public System.Windows.Forms.NumericInputBox numRotZ;
        public System.Windows.Forms.NumericInputBox numTransX;
        public System.Windows.Forms.NumericInputBox numTransY;
        public System.Windows.Forms.NumericInputBox numTransZ;
        private System.Windows.Forms.Label lblTrans;
        private System.Windows.Forms.Label lblRot;
        private System.Windows.Forms.Label lblScale;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox SelectedBone;
        private System.Windows.Forms.ComboBox SelectedZone;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
