using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System.Collections.Generic;

namespace System.Windows.Forms
{
    public class EditAllCHR0Editor : UserControl
    {
        private GroupBox groupBox2;
        private GroupBox groupBox4;
        private GroupBox groupBox3;
        private GroupBox groupBox5;
        private Label label7;
        private Label label6;
        private Label label8;
        private TextBox ScaleZ;
        private TextBox ScaleY;
        private TextBox ScaleX;
        private Label label1;
        private TextBox keyframeCopy;
        private TextBox name;
        private CheckBox copyKeyframes;
        private GroupBox groupBox1;
        private CheckBox NameContains;
        private TextBox targetName;
        private CheckBox Rename;
        private TextBox newName;
        private CheckBox editLoop;
        private CheckBox enableLoop;
        private CheckBox Port;
        private ComboBox Version;
        private RadioButton ScaleDivide;
        private RadioButton ScaleMultiply;
        private RadioButton ScaleSubtract;
        private RadioButton ScaleAdd;
        private RadioButton ScaleClear;
        private RadioButton ScaleReplace;
        private RadioButton TranslateDivide;
        private RadioButton TranslateMultiply;
        private RadioButton TranslateSubtract;
        private RadioButton TranslateAdd;
        private RadioButton TranslateClear;
        private RadioButton TranslateReplace;
        private Label label5;
        private Label label9;
        private Label label10;
        private TextBox TranslateZ;
        private TextBox TranslateY;
        private TextBox TranslateX;
        private RadioButton RotateDivide;
        private RadioButton RotateMultiply;
        private RadioButton RotateSubtract;
        private RadioButton RotateAdd;
        private RadioButton RotateClear;
        private RadioButton RotateReplace;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox RotateZ;
        private TextBox RotateY;
        private TextBox RotateX;
        private RadioButton TranslateDoNotChange;
        private RadioButton RotateDoNotChange;
        private RadioButton ScaleDoNotChange;
        private CheckBox ChangeVersion;
        #region Designer


        private void InitializeComponent()
        {
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.TranslateDoNotChange = new System.Windows.Forms.RadioButton();
            this.TranslateDivide = new System.Windows.Forms.RadioButton();
            this.TranslateMultiply = new System.Windows.Forms.RadioButton();
            this.TranslateSubtract = new System.Windows.Forms.RadioButton();
            this.TranslateAdd = new System.Windows.Forms.RadioButton();
            this.TranslateClear = new System.Windows.Forms.RadioButton();
            this.TranslateReplace = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.TranslateZ = new System.Windows.Forms.TextBox();
            this.TranslateY = new System.Windows.Forms.TextBox();
            this.TranslateX = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.RotateDoNotChange = new System.Windows.Forms.RadioButton();
            this.RotateDivide = new System.Windows.Forms.RadioButton();
            this.RotateMultiply = new System.Windows.Forms.RadioButton();
            this.RotateSubtract = new System.Windows.Forms.RadioButton();
            this.RotateAdd = new System.Windows.Forms.RadioButton();
            this.RotateClear = new System.Windows.Forms.RadioButton();
            this.RotateReplace = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.RotateZ = new System.Windows.Forms.TextBox();
            this.RotateY = new System.Windows.Forms.TextBox();
            this.RotateX = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.ScaleDoNotChange = new System.Windows.Forms.RadioButton();
            this.ScaleDivide = new System.Windows.Forms.RadioButton();
            this.ScaleMultiply = new System.Windows.Forms.RadioButton();
            this.ScaleSubtract = new System.Windows.Forms.RadioButton();
            this.ScaleAdd = new System.Windows.Forms.RadioButton();
            this.ScaleClear = new System.Windows.Forms.RadioButton();
            this.ScaleReplace = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ScaleZ = new System.Windows.Forms.TextBox();
            this.ScaleY = new System.Windows.Forms.TextBox();
            this.ScaleX = new System.Windows.Forms.TextBox();
            this.keyframeCopy = new System.Windows.Forms.TextBox();
            this.copyKeyframes = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.targetName = new System.Windows.Forms.TextBox();
            this.NameContains = new System.Windows.Forms.CheckBox();
            this.newName = new System.Windows.Forms.TextBox();
            this.Rename = new System.Windows.Forms.CheckBox();
            this.enableLoop = new System.Windows.Forms.CheckBox();
            this.editLoop = new System.Windows.Forms.CheckBox();
            this.Port = new System.Windows.Forms.CheckBox();
            this.Version = new System.Windows.Forms.ComboBox();
            this.ChangeVersion = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.keyframeCopy);
            this.groupBox2.Controls.Add(this.copyKeyframes);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.name);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 89);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(405, 250);
            this.groupBox2.TabIndex = 87;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CHR0 Bone Entries";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.TranslateDoNotChange);
            this.groupBox4.Controls.Add(this.TranslateDivide);
            this.groupBox4.Controls.Add(this.TranslateMultiply);
            this.groupBox4.Controls.Add(this.TranslateSubtract);
            this.groupBox4.Controls.Add(this.TranslateAdd);
            this.groupBox4.Controls.Add(this.TranslateClear);
            this.groupBox4.Controls.Add(this.TranslateReplace);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.TranslateZ);
            this.groupBox4.Controls.Add(this.TranslateY);
            this.groupBox4.Controls.Add(this.TranslateX);
            this.groupBox4.Location = new System.Drawing.Point(272, 68);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(126, 176);
            this.groupBox4.TabIndex = 39;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Translate";
            // 
            // TranslateDoNotChange
            // 
            this.TranslateDoNotChange.AutoSize = true;
            this.TranslateDoNotChange.Checked = true;
            this.TranslateDoNotChange.Location = new System.Drawing.Point(5, 152);
            this.TranslateDoNotChange.Name = "TranslateDoNotChange";
            this.TranslateDoNotChange.Size = new System.Drawing.Size(96, 17);
            this.TranslateDoNotChange.TabIndex = 48;
            this.TranslateDoNotChange.TabStop = true;
            this.TranslateDoNotChange.Text = "Do not change";
            this.TranslateDoNotChange.UseVisualStyleBackColor = true;
            this.TranslateDoNotChange.CheckedChanged += new System.EventHandler(this.TranslateClear_CheckedChanged);
            // 
            // TranslateDivide
            // 
            this.TranslateDivide.AutoSize = true;
            this.TranslateDivide.Location = new System.Drawing.Point(66, 129);
            this.TranslateDivide.Name = "TranslateDivide";
            this.TranslateDivide.Size = new System.Drawing.Size(55, 17);
            this.TranslateDivide.TabIndex = 47;
            this.TranslateDivide.Text = "Divide";
            this.TranslateDivide.UseVisualStyleBackColor = true;
            this.TranslateDivide.CheckedChanged += new System.EventHandler(this.TranslateClear_CheckedChanged);
            // 
            // TranslateMultiply
            // 
            this.TranslateMultiply.AutoSize = true;
            this.TranslateMultiply.Location = new System.Drawing.Point(5, 129);
            this.TranslateMultiply.Name = "TranslateMultiply";
            this.TranslateMultiply.Size = new System.Drawing.Size(60, 17);
            this.TranslateMultiply.TabIndex = 46;
            this.TranslateMultiply.Text = "Multiply";
            this.TranslateMultiply.UseVisualStyleBackColor = true;
            this.TranslateMultiply.CheckedChanged += new System.EventHandler(this.TranslateClear_CheckedChanged);
            // 
            // TranslateSubtract
            // 
            this.TranslateSubtract.AutoSize = true;
            this.TranslateSubtract.Location = new System.Drawing.Point(55, 106);
            this.TranslateSubtract.Name = "TranslateSubtract";
            this.TranslateSubtract.Size = new System.Drawing.Size(65, 17);
            this.TranslateSubtract.TabIndex = 45;
            this.TranslateSubtract.Text = "Subtract";
            this.TranslateSubtract.UseVisualStyleBackColor = true;
            this.TranslateSubtract.CheckedChanged += new System.EventHandler(this.TranslateClear_CheckedChanged);
            // 
            // TranslateAdd
            // 
            this.TranslateAdd.AutoSize = true;
            this.TranslateAdd.Location = new System.Drawing.Point(5, 106);
            this.TranslateAdd.Name = "TranslateAdd";
            this.TranslateAdd.Size = new System.Drawing.Size(44, 17);
            this.TranslateAdd.TabIndex = 44;
            this.TranslateAdd.Text = "Add";
            this.TranslateAdd.UseVisualStyleBackColor = true;
            this.TranslateAdd.CheckedChanged += new System.EventHandler(this.TranslateClear_CheckedChanged);
            // 
            // TranslateClear
            // 
            this.TranslateClear.AutoSize = true;
            this.TranslateClear.Location = new System.Drawing.Point(72, 83);
            this.TranslateClear.Name = "TranslateClear";
            this.TranslateClear.Size = new System.Drawing.Size(49, 17);
            this.TranslateClear.TabIndex = 43;
            this.TranslateClear.Text = "Clear";
            this.TranslateClear.UseVisualStyleBackColor = true;
            this.TranslateClear.CheckedChanged += new System.EventHandler(this.TranslateClear_CheckedChanged);
            // 
            // TranslateReplace
            // 
            this.TranslateReplace.AutoSize = true;
            this.TranslateReplace.Location = new System.Drawing.Point(5, 83);
            this.TranslateReplace.Name = "TranslateReplace";
            this.TranslateReplace.Size = new System.Drawing.Size(65, 17);
            this.TranslateReplace.TabIndex = 42;
            this.TranslateReplace.Text = "Replace";
            this.TranslateReplace.UseVisualStyleBackColor = true;
            this.TranslateReplace.CheckedChanged += new System.EventHandler(this.TranslateClear_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 40;
            this.label5.Text = "Y:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 13);
            this.label9.TabIndex = 39;
            this.label9.Text = "X:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 60);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 13);
            this.label10.TabIndex = 41;
            this.label10.Text = "Z:";
            // 
            // TranslateZ
            // 
            this.TranslateZ.Enabled = false;
            this.TranslateZ.Location = new System.Drawing.Point(24, 57);
            this.TranslateZ.Name = "TranslateZ";
            this.TranslateZ.Size = new System.Drawing.Size(96, 20);
            this.TranslateZ.TabIndex = 38;
            // 
            // TranslateY
            // 
            this.TranslateY.Enabled = false;
            this.TranslateY.Location = new System.Drawing.Point(24, 36);
            this.TranslateY.Name = "TranslateY";
            this.TranslateY.Size = new System.Drawing.Size(96, 20);
            this.TranslateY.TabIndex = 37;
            // 
            // TranslateX
            // 
            this.TranslateX.Enabled = false;
            this.TranslateX.Location = new System.Drawing.Point(24, 15);
            this.TranslateX.Name = "TranslateX";
            this.TranslateX.Size = new System.Drawing.Size(96, 20);
            this.TranslateX.TabIndex = 36;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.RotateDoNotChange);
            this.groupBox3.Controls.Add(this.RotateDivide);
            this.groupBox3.Controls.Add(this.RotateMultiply);
            this.groupBox3.Controls.Add(this.RotateSubtract);
            this.groupBox3.Controls.Add(this.RotateAdd);
            this.groupBox3.Controls.Add(this.RotateClear);
            this.groupBox3.Controls.Add(this.RotateReplace);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.RotateZ);
            this.groupBox3.Controls.Add(this.RotateY);
            this.groupBox3.Controls.Add(this.RotateX);
            this.groupBox3.Location = new System.Drawing.Point(140, 68);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(126, 176);
            this.groupBox3.TabIndex = 38;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Rotate";
            // 
            // RotateDoNotChange
            // 
            this.RotateDoNotChange.AutoSize = true;
            this.RotateDoNotChange.Checked = true;
            this.RotateDoNotChange.Location = new System.Drawing.Point(5, 152);
            this.RotateDoNotChange.Name = "RotateDoNotChange";
            this.RotateDoNotChange.Size = new System.Drawing.Size(96, 17);
            this.RotateDoNotChange.TabIndex = 37;
            this.RotateDoNotChange.TabStop = true;
            this.RotateDoNotChange.Text = "Do not change";
            this.RotateDoNotChange.UseVisualStyleBackColor = true;
            this.RotateDoNotChange.CheckedChanged += new System.EventHandler(this.RotateClear_CheckedChanged);
            // 
            // RotateDivide
            // 
            this.RotateDivide.AutoSize = true;
            this.RotateDivide.Location = new System.Drawing.Point(66, 129);
            this.RotateDivide.Name = "RotateDivide";
            this.RotateDivide.Size = new System.Drawing.Size(55, 17);
            this.RotateDivide.TabIndex = 47;
            this.RotateDivide.Text = "Divide";
            this.RotateDivide.UseVisualStyleBackColor = true;
            this.RotateDivide.CheckedChanged += new System.EventHandler(this.RotateClear_CheckedChanged);
            // 
            // RotateMultiply
            // 
            this.RotateMultiply.AutoSize = true;
            this.RotateMultiply.Location = new System.Drawing.Point(5, 129);
            this.RotateMultiply.Name = "RotateMultiply";
            this.RotateMultiply.Size = new System.Drawing.Size(60, 17);
            this.RotateMultiply.TabIndex = 46;
            this.RotateMultiply.Text = "Multiply";
            this.RotateMultiply.UseVisualStyleBackColor = true;
            this.RotateMultiply.CheckedChanged += new System.EventHandler(this.RotateClear_CheckedChanged);
            // 
            // RotateSubtract
            // 
            this.RotateSubtract.AutoSize = true;
            this.RotateSubtract.Location = new System.Drawing.Point(55, 106);
            this.RotateSubtract.Name = "RotateSubtract";
            this.RotateSubtract.Size = new System.Drawing.Size(65, 17);
            this.RotateSubtract.TabIndex = 45;
            this.RotateSubtract.Text = "Subtract";
            this.RotateSubtract.UseVisualStyleBackColor = true;
            this.RotateSubtract.CheckedChanged += new System.EventHandler(this.RotateClear_CheckedChanged);
            // 
            // RotateAdd
            // 
            this.RotateAdd.AutoSize = true;
            this.RotateAdd.Location = new System.Drawing.Point(5, 106);
            this.RotateAdd.Name = "RotateAdd";
            this.RotateAdd.Size = new System.Drawing.Size(44, 17);
            this.RotateAdd.TabIndex = 44;
            this.RotateAdd.Text = "Add";
            this.RotateAdd.UseVisualStyleBackColor = true;
            this.RotateAdd.CheckedChanged += new System.EventHandler(this.RotateClear_CheckedChanged);
            // 
            // RotateClear
            // 
            this.RotateClear.AutoSize = true;
            this.RotateClear.Location = new System.Drawing.Point(72, 83);
            this.RotateClear.Name = "RotateClear";
            this.RotateClear.Size = new System.Drawing.Size(49, 17);
            this.RotateClear.TabIndex = 43;
            this.RotateClear.Text = "Clear";
            this.RotateClear.UseVisualStyleBackColor = true;
            this.RotateClear.CheckedChanged += new System.EventHandler(this.RotateClear_CheckedChanged);
            // 
            // RotateReplace
            // 
            this.RotateReplace.AutoSize = true;
            this.RotateReplace.Location = new System.Drawing.Point(5, 83);
            this.RotateReplace.Name = "RotateReplace";
            this.RotateReplace.Size = new System.Drawing.Size(65, 17);
            this.RotateReplace.TabIndex = 42;
            this.RotateReplace.Text = "Replace";
            this.RotateReplace.UseVisualStyleBackColor = true;
            this.RotateReplace.CheckedChanged += new System.EventHandler(this.RotateClear_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 40;
            this.label2.Text = "Y:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 39;
            this.label3.Text = "X:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "Z:";
            // 
            // RotateZ
            // 
            this.RotateZ.Enabled = false;
            this.RotateZ.Location = new System.Drawing.Point(24, 57);
            this.RotateZ.Name = "RotateZ";
            this.RotateZ.Size = new System.Drawing.Size(96, 20);
            this.RotateZ.TabIndex = 38;
            // 
            // RotateY
            // 
            this.RotateY.Enabled = false;
            this.RotateY.Location = new System.Drawing.Point(24, 36);
            this.RotateY.Name = "RotateY";
            this.RotateY.Size = new System.Drawing.Size(96, 20);
            this.RotateY.TabIndex = 37;
            // 
            // RotateX
            // 
            this.RotateX.Enabled = false;
            this.RotateX.Location = new System.Drawing.Point(24, 15);
            this.RotateX.Name = "RotateX";
            this.RotateX.Size = new System.Drawing.Size(96, 20);
            this.RotateX.TabIndex = 36;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.ScaleDoNotChange);
            this.groupBox5.Controls.Add(this.ScaleDivide);
            this.groupBox5.Controls.Add(this.ScaleMultiply);
            this.groupBox5.Controls.Add(this.ScaleSubtract);
            this.groupBox5.Controls.Add(this.ScaleAdd);
            this.groupBox5.Controls.Add(this.ScaleClear);
            this.groupBox5.Controls.Add(this.ScaleReplace);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.ScaleZ);
            this.groupBox5.Controls.Add(this.ScaleY);
            this.groupBox5.Controls.Add(this.ScaleX);
            this.groupBox5.Location = new System.Drawing.Point(8, 68);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(126, 176);
            this.groupBox5.TabIndex = 37;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Scale";
            // 
            // ScaleDoNotChange
            // 
            this.ScaleDoNotChange.AutoSize = true;
            this.ScaleDoNotChange.Checked = true;
            this.ScaleDoNotChange.Location = new System.Drawing.Point(5, 152);
            this.ScaleDoNotChange.Name = "ScaleDoNotChange";
            this.ScaleDoNotChange.Size = new System.Drawing.Size(96, 17);
            this.ScaleDoNotChange.TabIndex = 36;
            this.ScaleDoNotChange.TabStop = true;
            this.ScaleDoNotChange.Text = "Do not change";
            this.ScaleDoNotChange.UseVisualStyleBackColor = true;
            this.ScaleDoNotChange.CheckedChanged += new System.EventHandler(this.ScaleClear_CheckedChanged);
            // 
            // ScaleDivide
            // 
            this.ScaleDivide.AutoSize = true;
            this.ScaleDivide.Location = new System.Drawing.Point(66, 129);
            this.ScaleDivide.Name = "ScaleDivide";
            this.ScaleDivide.Size = new System.Drawing.Size(55, 17);
            this.ScaleDivide.TabIndex = 35;
            this.ScaleDivide.Text = "Divide";
            this.ScaleDivide.UseVisualStyleBackColor = true;
            this.ScaleDivide.CheckedChanged += new System.EventHandler(this.ScaleClear_CheckedChanged);
            // 
            // ScaleMultiply
            // 
            this.ScaleMultiply.AutoSize = true;
            this.ScaleMultiply.Location = new System.Drawing.Point(5, 129);
            this.ScaleMultiply.Name = "ScaleMultiply";
            this.ScaleMultiply.Size = new System.Drawing.Size(60, 17);
            this.ScaleMultiply.TabIndex = 34;
            this.ScaleMultiply.Text = "Multiply";
            this.ScaleMultiply.UseVisualStyleBackColor = true;
            this.ScaleMultiply.CheckedChanged += new System.EventHandler(this.ScaleClear_CheckedChanged);
            // 
            // ScaleSubtract
            // 
            this.ScaleSubtract.AutoSize = true;
            this.ScaleSubtract.Location = new System.Drawing.Point(55, 106);
            this.ScaleSubtract.Name = "ScaleSubtract";
            this.ScaleSubtract.Size = new System.Drawing.Size(65, 17);
            this.ScaleSubtract.TabIndex = 33;
            this.ScaleSubtract.Text = "Subtract";
            this.ScaleSubtract.UseVisualStyleBackColor = true;
            this.ScaleSubtract.CheckedChanged += new System.EventHandler(this.ScaleClear_CheckedChanged);
            // 
            // ScaleAdd
            // 
            this.ScaleAdd.AutoSize = true;
            this.ScaleAdd.Location = new System.Drawing.Point(5, 106);
            this.ScaleAdd.Name = "ScaleAdd";
            this.ScaleAdd.Size = new System.Drawing.Size(44, 17);
            this.ScaleAdd.TabIndex = 32;
            this.ScaleAdd.Text = "Add";
            this.ScaleAdd.UseVisualStyleBackColor = true;
            this.ScaleAdd.CheckedChanged += new System.EventHandler(this.ScaleClear_CheckedChanged);
            // 
            // ScaleClear
            // 
            this.ScaleClear.AutoSize = true;
            this.ScaleClear.Location = new System.Drawing.Point(72, 83);
            this.ScaleClear.Name = "ScaleClear";
            this.ScaleClear.Size = new System.Drawing.Size(49, 17);
            this.ScaleClear.TabIndex = 31;
            this.ScaleClear.Text = "Clear";
            this.ScaleClear.UseVisualStyleBackColor = true;
            this.ScaleClear.CheckedChanged += new System.EventHandler(this.ScaleClear_CheckedChanged);
            // 
            // ScaleReplace
            // 
            this.ScaleReplace.AutoSize = true;
            this.ScaleReplace.Location = new System.Drawing.Point(5, 83);
            this.ScaleReplace.Name = "ScaleReplace";
            this.ScaleReplace.Size = new System.Drawing.Size(65, 17);
            this.ScaleReplace.TabIndex = 30;
            this.ScaleReplace.Text = "Replace";
            this.ScaleReplace.UseVisualStyleBackColor = true;
            this.ScaleReplace.CheckedChanged += new System.EventHandler(this.ScaleClear_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Y:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "X:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Z:";
            // 
            // ScaleZ
            // 
            this.ScaleZ.Enabled = false;
            this.ScaleZ.Location = new System.Drawing.Point(24, 57);
            this.ScaleZ.Name = "ScaleZ";
            this.ScaleZ.Size = new System.Drawing.Size(96, 20);
            this.ScaleZ.TabIndex = 14;
            // 
            // ScaleY
            // 
            this.ScaleY.Enabled = false;
            this.ScaleY.Location = new System.Drawing.Point(24, 36);
            this.ScaleY.Name = "ScaleY";
            this.ScaleY.Size = new System.Drawing.Size(96, 20);
            this.ScaleY.TabIndex = 13;
            // 
            // ScaleX
            // 
            this.ScaleX.Enabled = false;
            this.ScaleX.Location = new System.Drawing.Point(24, 15);
            this.ScaleX.Name = "ScaleX";
            this.ScaleX.Size = new System.Drawing.Size(96, 20);
            this.ScaleX.TabIndex = 12;
            // 
            // keyframeCopy
            // 
            this.keyframeCopy.Enabled = false;
            this.keyframeCopy.Location = new System.Drawing.Point(208, 43);
            this.keyframeCopy.Name = "keyframeCopy";
            this.keyframeCopy.Size = new System.Drawing.Size(189, 20);
            this.keyframeCopy.TabIndex = 34;
            // 
            // copyKeyframes
            // 
            this.copyKeyframes.AutoSize = true;
            this.copyKeyframes.Location = new System.Drawing.Point(208, 26);
            this.copyKeyframes.Name = "copyKeyframes";
            this.copyKeyframes.Size = new System.Drawing.Size(127, 17);
            this.copyKeyframes.TabIndex = 33;
            this.copyKeyframes.Text = "Copy keyframes from:";
            this.copyKeyframes.UseVisualStyleBackColor = true;
            this.copyKeyframes.CheckedChanged += new System.EventHandler(this.copyKeyframes_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Change all bone entries with the name:";
            // 
            // name
            // 
            this.name.HideSelection = false;
            this.name.Location = new System.Drawing.Point(11, 42);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(187, 20);
            this.name.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.targetName);
            this.groupBox1.Controls.Add(this.NameContains);
            this.groupBox1.Controls.Add(this.newName);
            this.groupBox1.Controls.Add(this.Rename);
            this.groupBox1.Controls.Add(this.enableLoop);
            this.groupBox1.Controls.Add(this.editLoop);
            this.groupBox1.Controls.Add(this.Port);
            this.groupBox1.Controls.Add(this.Version);
            this.groupBox1.Controls.Add(this.ChangeVersion);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(405, 89);
            this.groupBox1.TabIndex = 86;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CHR0";
            // 
            // targetName
            // 
            this.targetName.Enabled = false;
            this.targetName.HideSelection = false;
            this.targetName.Location = new System.Drawing.Point(171, 16);
            this.targetName.Name = "targetName";
            this.targetName.Size = new System.Drawing.Size(151, 20);
            this.targetName.TabIndex = 84;
            // 
            // NameContains
            // 
            this.NameContains.Location = new System.Drawing.Point(8, 18);
            this.NameContains.Name = "NameContains";
            this.NameContains.Size = new System.Drawing.Size(163, 17);
            this.NameContains.TabIndex = 85;
            this.NameContains.Text = "Modify only if name contains: ";
            this.NameContains.UseVisualStyleBackColor = true;
            this.NameContains.CheckedChanged += new System.EventHandler(this.NameContains_CheckedChanged);
            // 
            // newName
            // 
            this.newName.Enabled = false;
            this.newName.HideSelection = false;
            this.newName.Location = new System.Drawing.Point(203, 39);
            this.newName.Name = "newName";
            this.newName.Size = new System.Drawing.Size(119, 20);
            this.newName.TabIndex = 82;
            // 
            // Rename
            // 
            this.Rename.Location = new System.Drawing.Point(136, 41);
            this.Rename.Name = "Rename";
            this.Rename.Size = new System.Drawing.Size(69, 17);
            this.Rename.TabIndex = 83;
            this.Rename.Text = "Rename:";
            this.Rename.UseVisualStyleBackColor = true;
            this.Rename.CheckedChanged += new System.EventHandler(this.Rename_CheckedChanged);
            // 
            // enableLoop
            // 
            this.enableLoop.AutoSize = true;
            this.enableLoop.Enabled = false;
            this.enableLoop.Location = new System.Drawing.Point(76, 41);
            this.enableLoop.Name = "enableLoop";
            this.enableLoop.Size = new System.Drawing.Size(59, 17);
            this.enableLoop.TabIndex = 39;
            this.enableLoop.Text = "Enable";
            this.enableLoop.UseVisualStyleBackColor = true;
            // 
            // editLoop
            // 
            this.editLoop.AutoSize = true;
            this.editLoop.Location = new System.Drawing.Point(8, 41);
            this.editLoop.Name = "editLoop";
            this.editLoop.Size = new System.Drawing.Size(70, 17);
            this.editLoop.TabIndex = 38;
            this.editLoop.Text = "Edit loop:";
            this.editLoop.UseVisualStyleBackColor = true;
            this.editLoop.CheckedChanged += new System.EventHandler(this.editLoop_CheckedChanged);
            // 
            // Port
            // 
            this.Port.AutoSize = true;
            this.Port.Location = new System.Drawing.Point(202, 63);
            this.Port.Name = "Port";
            this.Port.Size = new System.Drawing.Size(59, 17);
            this.Port.TabIndex = 37;
            this.Port.Text = "Port All";
            this.Port.UseVisualStyleBackColor = true;
            // 
            // Version
            // 
            this.Version.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Version.Enabled = false;
            this.Version.FormattingEnabled = true;
            this.Version.Items.AddRange(new object[] {
            "4",
            "5"});
            this.Version.Location = new System.Drawing.Point(114, 61);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(80, 21);
            this.Version.TabIndex = 36;
            // 
            // ChangeVersion
            // 
            this.ChangeVersion.AutoSize = true;
            this.ChangeVersion.Location = new System.Drawing.Point(8, 64);
            this.ChangeVersion.Name = "ChangeVersion";
            this.ChangeVersion.Size = new System.Drawing.Size(103, 17);
            this.ChangeVersion.TabIndex = 35;
            this.ChangeVersion.Text = "Change version:";
            this.ChangeVersion.UseVisualStyleBackColor = true;
            this.ChangeVersion.CheckedChanged += new System.EventHandler(this.ChangeVersion_CheckedChanged);
            // 
            // EditAllCHR0Editor
            // 
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "EditAllCHR0Editor";
            this.Size = new System.Drawing.Size(405, 339);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public EditAllCHR0Editor() { InitializeComponent(); }

        private void editLoop_CheckedChanged(object sender, EventArgs e)
        {
            enableLoop.Enabled = editLoop.Checked;
        }

        public void OnlyEntryNodesSelected() {
            groupBox1.Enabled =
                name.Enabled =
                keyframeCopy.Enabled =
                copyKeyframes.Enabled =
                false;
        }

        public static PartialVector3 Vec3From(TextBox x, TextBox y, TextBox z) {
            return Vec3From(x.Text, y.Text, z.Text);
        }

        public static PartialVector3 Vec3From(string x, string y, string z) {
            return new PartialVector3(
                string.IsNullOrEmpty(x) ? (float?)null : float.Parse(x),
                string.IsNullOrEmpty(y) ? (float?)null : float.Parse(y),
                string.IsNullOrEmpty(z) ? (float?)null : float.Parse(z));
        }

        public void Apply(IEnumerable<CHR0Node> CHR0)
        {
            string _name = name.Text;
            MDL0Node model = null;
            MDL0Node _targetModel = null;
            if (Port.Checked)
            {
                MessageBox.Show("Please open the model you want to port the animations to.\nThen open the model the animations work normally for.");
                OpenFileDialog dlgOpen = new OpenFileDialog();
                OpenFileDialog dlgOpen2 = new OpenFileDialog();
                dlgOpen.Filter = dlgOpen2.Filter = "MDL0 Model (*.mdl0)|*.mdl0";
                dlgOpen.Title = "Select the model to port the animations to...";
                dlgOpen2.Title = "Select the model the animations are for...";
                if (dlgOpen.ShowDialog() == DialogResult.OK)
                {
                    _targetModel = (MDL0Node)NodeFactory.FromFile(null, dlgOpen.FileName);
                    if (dlgOpen2.ShowDialog() == DialogResult.OK)
                        model = (MDL0Node)NodeFactory.FromFile(null, dlgOpen2.FileName);
                }
            }
            PartialVector3 scale = Vec3From(ScaleX, ScaleY, ScaleZ);
            PartialVector3 rot = Vec3From(RotateX, RotateY, RotateZ);
            PartialVector3 trans = Vec3From(TranslateX, TranslateY, TranslateZ);
            
            foreach (CHR0Node n in CHR0)
            {
                if (NameContains.Checked && !n.Name.Contains(targetName.Text))
                    continue;

                if (editLoop.Checked) {
                    n.SignalPropertyChange();
                    n.Loop = enableLoop.Checked;
                }

                if (Rename.Checked) {
                    n.SignalPropertyChange();
                    n.Name = n.Parent.FindName(newName.Text);
                }

                if (ChangeVersion.Checked) {
                    n.SignalPropertyChange();
                    n.Version = Version.SelectedIndex + 4;
                }

                if (Port.Checked && _targetModel != null && model != null) {
                    n.SignalPropertyChange();
                    n.Port(_targetModel, model);
                }

                if (copyKeyframes.Checked)
                {
                    CHR0EntryNode _copyNode = n.FindChild(keyframeCopy.Text, false) as CHR0EntryNode;

                    if (n.FindChild(_name, false) == null)
                    {
                        if (!String.IsNullOrEmpty(_name))
                        {
                            KeyframeEntry kfe = null;
                            CHR0EntryNode c = new CHR0EntryNode();
                            c.SetSize(n.FrameCount, n.Loop);
                            c.Name = _name;

                            if (_copyNode != null)
                                for (int x = 0; x < _copyNode.FrameCount; x++)
                                    for (int i = 0; i < 9; i++)
                                        if ((kfe = _copyNode.GetKeyframe(i, x)) != null)
                                            c.SetKeyframe(i, x, kfe._value);

                            n.AddChild(c);
                        }
                    }
                }
                
                CHR0EntryNode entry = n.FindChild(_name, false) as CHR0EntryNode;
                if (entry == null)
                {
                    entry = new CHR0EntryNode() { _name = _name };
                    n.AddChild(entry);
                }
                Apply(entry, scale, rot, trans);
            }
        }

        public void Apply(IEnumerable<CHR0EntryNode> CHR0)
        {
            PartialVector3 scale = Vec3From(ScaleX, ScaleY, ScaleZ);
            PartialVector3 rot = Vec3From(RotateX, RotateY, RotateZ);
            PartialVector3 trans = Vec3From(TranslateX, TranslateY, TranslateZ);
            
            foreach (CHR0EntryNode entry in CHR0)
            {
                Apply(entry, scale, rot, trans);
            }
        }

        private void Apply(CHR0EntryNode entry, PartialVector3 scaleVec, PartialVector3 rotVec, PartialVector3 transVec)
        {
            KeyframeEntry kfe = null;
            CHRAnimationFrame anim;
            bool hasKeyframe = false;
            int numFrames = entry.FrameCount;
            int low = 0, high = 3;
            if (ScaleReplace.Checked)
            {
                var scale = (Vector3)scaleVec;
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if (entry.GetKeyframe(i, x) != null)
                            entry.RemoveKeyframe(i, x);
                        
                entry.SetKeyframeOnlyScale(0, scale);
            }
            else if (ScaleClear.Checked)
            {
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if (entry.GetKeyframe(i, x) != null)
                            entry.RemoveKeyframe(i, x);
            }
            else if (ScaleAdd.Checked)
            {
                var scale = new Vector3(
                    scaleVec._x ?? 0,
                    scaleVec._y ?? 0,
                    scaleVec._z ?? 0);
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if ((kfe = entry.GetKeyframe(i, x)) != null)
                        {
                            if (i == low)
                                kfe._value += scale._x;
                            else if (i == low + 1)
                                kfe._value += scale._y;
                            else if (i == high - 1)
                                kfe._value += scale._z;
                            hasKeyframe = true;
                        }
                if (!hasKeyframe)
                {
                    anim = entry.GetAnimFrame(0);
                    Vector3 newScale = anim.Scale;
                    scale._x += newScale._x;
                    scale._y += newScale._y;
                    scale._z += newScale._z;
                    entry.SetKeyframeOnlyScale(0, scale);
                }
            }
            else if (ScaleSubtract.Checked)
            {
                var scale = new Vector3(
                    scaleVec._x ?? 0,
                    scaleVec._y ?? 0,
                    scaleVec._z ?? 0);
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if ((kfe = entry.GetKeyframe(i, x)) != null)
                        {
                            if (i == low)
                                kfe._value -= scale._x;
                            else if (i == low + 1)
                                kfe._value -= scale._y;
                            else if (i == high - 1)
                                kfe._value -= scale._z;
                            hasKeyframe = true;
                        }
                if (!hasKeyframe)
                {
                    anim = entry.GetAnimFrame(0);
                    Vector3 newScale = anim.Scale;
                    scale._x = newScale._x - scale._x;
                    scale._y = newScale._y - scale._y;
                    scale._z = newScale._z - scale._z;
                    entry.SetKeyframeOnlyScale(0, scale);
                }
            }
            else if (ScaleMultiply.Checked)
            {
                var scale = new Vector3(
                    scaleVec._x ?? 1,
                    scaleVec._y ?? 1,
                    scaleVec._z ?? 1);
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if ((kfe = entry.GetKeyframe(i, x)) != null)
                        {
                            if (i == low)
                                kfe._value *= scale._x;
                            else if (i == low + 1)
                                kfe._value *= scale._y;
                            else if (i == high - 1)
                                kfe._value *= scale._z;
                            hasKeyframe = true;
                        }
                if (!hasKeyframe)
                {
                    anim = entry.GetAnimFrame(0);
                    Vector3 newScale = anim.Scale;
                    scale._x *= newScale._x;
                    scale._y *= newScale._y;
                    scale._z *= newScale._z;
                    entry.SetKeyframeOnlyScale(0, scale);
                }
            }
            else if (ScaleDivide.Checked)
            {
                var scale = new Vector3(
                    scaleVec._x ?? 1,
                    scaleVec._y ?? 1,
                    scaleVec._z ?? 1);
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if ((kfe = entry.GetKeyframe(i, x)) != null)
                        {
                            if (i == low && scale._x != 0)
                                kfe._value /= scale._x;
                            else if (i == low + 1 && scale._y != 0)
                                kfe._value /= scale._y;
                            else if (i == high - 1 && scale._z != 0)
                                kfe._value /= scale._z;
                            hasKeyframe = true;
                        }
                if (!hasKeyframe)
                {
                    anim = entry.GetAnimFrame(0);
                    Vector3 newScale = anim.Scale;
                    if (scale._x != 0)
                        scale._x = newScale._x / scale._x;
                    if (scale._y != 0)
                        scale._y = newScale._y / scale._y;
                    if (scale._z != 0)
                        scale._z = newScale._z / scale._z;
                    entry.SetKeyframeOnlyScale(0, scale);
                }
            }

            low = 3; high = 6;
            if (RotateReplace.Checked)
            {
                var rot = (Vector3)rotVec;
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if (entry.GetKeyframe(i, x) != null)
                            entry.RemoveKeyframe(i, x);
                        
                entry.SetKeyframeOnlyRot(0, rot);
            }
            else if (RotateClear.Checked)
            {
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if (entry.GetKeyframe(i, x) != null)
                            entry.RemoveKeyframe(i, x);
            }
            else if (RotateAdd.Checked)
            {
                var rot = new Vector3(
                    rotVec._x ?? 0,
                    rotVec._y ?? 0,
                    rotVec._z ?? 0);
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if ((kfe = entry.GetKeyframe(i, x)) != null)
                        {
                            if (i == low)
                                kfe._value += rot._x;
                            else if (i == low + 1)
                                kfe._value += rot._y;
                            else if (i == high - 1)
                                kfe._value += rot._z;
                            hasKeyframe = true;
                        }
                if (!hasKeyframe)
                {
                    anim = entry.GetAnimFrame(0);
                    Vector3 newRotate = anim.Rotation;
                    rot._x += newRotate._x;
                    rot._y += newRotate._y;
                    rot._z += newRotate._z;
                    entry.SetKeyframeOnlyRot(0, rot);
                }
            }
            else if (RotateSubtract.Checked)
            {
                var rot = new Vector3(
                    rotVec._x ?? 0,
                    rotVec._y ?? 0,
                    rotVec._z ?? 0);
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if ((kfe = entry.GetKeyframe(i, x)) != null)
                        {
                            if (i == low)
                                kfe._value -= rot._x;
                            else if (i == low + 1)
                                kfe._value -= rot._y;
                            else if (i == high - 1)
                                kfe._value -= rot._z;
                            hasKeyframe = true;
                        }
                if (!hasKeyframe)
                {
                    anim = entry.GetAnimFrame(0);
                    Vector3 newRotate = anim.Rotation;
                    rot._x = newRotate._x - rot._x;
                    rot._y = newRotate._y - rot._y;
                    rot._z = newRotate._z - rot._z;
                    entry.SetKeyframeOnlyRot(0, rot);
                }
            }
            else if (RotateMultiply.Checked)
            {
                var rot = new Vector3(
                    rotVec._x ?? 1,
                    rotVec._y ?? 1,
                    rotVec._z ?? 1);
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if ((kfe = entry.GetKeyframe(i, x)) != null)
                        {
                            if (i == low)
                                kfe._value *= rot._x;
                            else if (i == low + 1)
                                kfe._value *= rot._y;
                            else if (i == high - 1)
                                kfe._value *= rot._z;
                            hasKeyframe = true;
                        }
                if (!hasKeyframe)
                {
                    anim = entry.GetAnimFrame(0);
                    Vector3 newRotate = anim.Rotation;
                    rot._x *= newRotate._x;
                    rot._y *= newRotate._y;
                    rot._z *= newRotate._z;
                    entry.SetKeyframeOnlyRot(0, rot);
                }
            }
            else if (RotateDivide.Checked)
            {
                var rot = new Vector3(
                    rotVec._x ?? 1,
                    rotVec._y ?? 1,
                    rotVec._z ?? 1);
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if ((kfe = entry.GetKeyframe(i, x)) != null)
                        {
                            if (i == low && rot._x != 0)
                                kfe._value /= rot._x;
                            else if (i == low + 1 && rot._y != 0)
                                kfe._value /= rot._y;
                            else if (i == high - 1 && rot._z != 0)
                                kfe._value /= rot._z;
                            hasKeyframe = true;
                        }
                if (!hasKeyframe)
                {
                    anim = entry.GetAnimFrame(0);
                    Vector3 newRotate = anim.Rotation;
                    if (rot._x != 0)
                        rot._x = newRotate._x / rot._x;
                    if (rot._y != 0)
                        rot._y = newRotate._y / rot._y;
                    if (rot._z != 0)
                        rot._z = newRotate._z / rot._z;
                    entry.SetKeyframeOnlyRot(0, rot);
                }
            }

            low = 6; high = 9;
            if (TranslateReplace.Checked)
            {
                var trans = (Vector3)transVec;
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = 0x10; i < high; i++)
                        if (entry.GetKeyframe(i, x) != null)
                            entry.RemoveKeyframe(i, x);

                entry.SetKeyframeOnlyTrans(0, trans);
            }
            else if (TranslateClear.Checked)
            {
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if (entry.GetKeyframe(i, x) != null)
                            entry.RemoveKeyframe(i, x);
            }
            else if (TranslateAdd.Checked)
            {
                var trans = new Vector3(
                    transVec._x ?? 0,
                    transVec._y ?? 0,
                    transVec._z ?? 0);
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if ((kfe = entry.GetKeyframe(i, x)) != null)
                        {
                            if (i == low)
                                kfe._value += trans._x;
                            else if (i == low + 1)
                                kfe._value += trans._y;
                            else if (i == high - 1)
                                kfe._value += trans._z;
                            hasKeyframe = true;
                        }
                if (!hasKeyframe)
                {
                    anim = entry.GetAnimFrame(0);
                    Vector3 newTranslate = anim.Translation;
                    trans._x += newTranslate._x;
                    trans._y += newTranslate._y;
                    trans._z += newTranslate._z;
                    entry.SetKeyframeOnlyTrans(0, trans);
                }
            }
            else if (TranslateSubtract.Checked)
            {
                var trans = new Vector3(
                    transVec._x ?? 0,
                    transVec._y ?? 0,
                    transVec._z ?? 0);
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if ((kfe = entry.GetKeyframe(i, x)) != null)
                        {
                            if (i == low)
                                kfe._value -= trans._x;
                            else if (i == low + 1)
                                kfe._value -= trans._y;
                            else if (i == high - 1)
                                kfe._value -= trans._z;
                            hasKeyframe = true;
                        }
                if (!hasKeyframe)
                {
                    anim = entry.GetAnimFrame(0);
                    Vector3 newTranslate = anim.Translation;
                    trans._x = newTranslate._x - trans._x;
                    trans._y = newTranslate._y - trans._y;
                    trans._z = newTranslate._z - trans._z;
                    entry.SetKeyframeOnlyTrans(0, trans);
                }
            }
            else if (TranslateMultiply.Checked)
            {
                var trans = new Vector3(
                    transVec._x ?? 1,
                    transVec._y ?? 1,
                    transVec._z ?? 1);
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if ((kfe = entry.GetKeyframe(i, x)) != null)
                        {
                            if (i == low)
                                kfe._value *= trans._x;
                            else if (i == low + 1)
                                kfe._value *= trans._y;
                            else if (i == high - 1)
                                kfe._value *= trans._z;
                            hasKeyframe = true;
                        }
                if (!hasKeyframe)
                {
                    anim = entry.GetAnimFrame(0);
                    Vector3 newTranslate = anim.Translation;
                    trans._x *= newTranslate._x;
                    trans._y *= newTranslate._y;
                    trans._z *= newTranslate._z;
                    entry.SetKeyframeOnlyTrans(0, trans);
                }
            }
            else if (TranslateDivide.Checked)
            {
                var trans = new Vector3(
                    transVec._x ?? 1,
                    transVec._y ?? 1,
                    transVec._z ?? 1);
                entry.SignalPropertyChange();
                for (int x = 0; x < numFrames; x++)
                    for (int i = low; i < high; i++)
                        if ((kfe = entry.GetKeyframe(i, x)) != null)
                        {
                            if (i == low && trans._x != 0)
                                kfe._value /= trans._x;
                            else if (i == low + 1 && trans._y != 0)
                                kfe._value /= trans._y;
                            else if (i == high - 1 && trans._z != 0)
                                kfe._value /= trans._z;
                            hasKeyframe = true;
                        }
                if (!hasKeyframe)
                {
                    anim = entry.GetAnimFrame(0);
                    Vector3 newTranslate = anim.Translation;
                    if (trans._x != 0)
                        trans._x = newTranslate._x / trans._x;
                    if (trans._y != 0)
                        trans._y = newTranslate._y / trans._y;
                    if (trans._z != 0)
                        trans._z = newTranslate._z / trans._z;
                    entry.SetKeyframeOnlyTrans(0, trans);
                }
            }
        }

        private void copyKeyframes_CheckedChanged(object sender, EventArgs e)
        {
            keyframeCopy.Enabled = copyKeyframes.Checked;
        }

        private void NameContains_CheckedChanged(object sender, EventArgs e)
        {
            targetName.Enabled = NameContains.Checked;
        }

        private void Rename_CheckedChanged(object sender, EventArgs e)
        {
            newName.Enabled = Rename.Checked;
        }

        private void ChangeVersion_CheckedChanged(object sender, EventArgs e)
        {
            Version.Enabled = ChangeVersion.Checked;
        }

        private void ScaleClear_CheckedChanged(object sender, EventArgs e)
        {
            ScaleX.Enabled = ScaleY.Enabled = ScaleZ.Enabled = (!ScaleClear.Checked && !ScaleDoNotChange.Checked);
        }
        private void RotateClear_CheckedChanged(object sender, EventArgs e)
        {
            RotateX.Enabled = RotateY.Enabled = RotateZ.Enabled = (!RotateClear.Checked && !RotateDoNotChange.Checked);
        }
        private void TranslateClear_CheckedChanged(object sender, EventArgs e)
        {
            TranslateX.Enabled = TranslateY.Enabled = TranslateZ.Enabled = (!TranslateClear.Checked && !TranslateDoNotChange.Checked);
        }
    }
}
