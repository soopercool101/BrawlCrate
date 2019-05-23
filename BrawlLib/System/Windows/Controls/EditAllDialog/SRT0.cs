namespace System.Windows.Forms
{
    public class EditAllSRT0Editor : UserControl
    {
        private CheckBox srtTexRename;
        private CheckBox srtModMat;
        private TextBox textBox7;
        private TextBox srtMatName;
        private TextBox srtTexName;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox6;
        private TextBox textBox9;
        private TextBox textBox10;
        private TextBox textBox12;
        private CheckBox srtLoopEnable;
        private Label label2;
        private Label srtScaleX;
        private Label srtScaleY;
        private Label srtRot;
        private Label srtTransX;
        private Label srtTransY;
        private CheckBox srtScaleSubtract;
        private CheckBox srtScaleAdd;
        private CheckBox srtScaleReplace;
        private CheckBox srtRotSubtract;
        private CheckBox srtRotAdd;
        private CheckBox srtRotReplace;
        private CheckBox srtTransSubtract;
        private CheckBox srtTransAdd;
        private CheckBox srtTransReplace;
        private CheckBox srtCopyKF;
        private CheckBox chkSrtVersion;
        private ComboBox srtVersion;
        private CheckBox srtEditLoop;
        #region Designer

        private GroupBox groupBox1;
        

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.srtTexRename = new System.Windows.Forms.CheckBox();
            this.srtModMat = new System.Windows.Forms.CheckBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.srtMatName = new System.Windows.Forms.TextBox();
            this.srtTexName = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.srtLoopEnable = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.srtScaleX = new System.Windows.Forms.Label();
            this.srtScaleY = new System.Windows.Forms.Label();
            this.srtRot = new System.Windows.Forms.Label();
            this.srtTransX = new System.Windows.Forms.Label();
            this.srtTransY = new System.Windows.Forms.Label();
            this.srtScaleSubtract = new System.Windows.Forms.CheckBox();
            this.srtScaleAdd = new System.Windows.Forms.CheckBox();
            this.srtScaleReplace = new System.Windows.Forms.CheckBox();
            this.srtRotSubtract = new System.Windows.Forms.CheckBox();
            this.srtRotAdd = new System.Windows.Forms.CheckBox();
            this.srtRotReplace = new System.Windows.Forms.CheckBox();
            this.srtTransSubtract = new System.Windows.Forms.CheckBox();
            this.srtTransAdd = new System.Windows.Forms.CheckBox();
            this.srtTransReplace = new System.Windows.Forms.CheckBox();
            this.srtCopyKF = new System.Windows.Forms.CheckBox();
            this.chkSrtVersion = new System.Windows.Forms.CheckBox();
            this.srtVersion = new System.Windows.Forms.ComboBox();
            this.srtEditLoop = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.srtTexRename);
            this.groupBox1.Controls.Add(this.srtModMat);
            this.groupBox1.Controls.Add(this.textBox7);
            this.groupBox1.Controls.Add(this.srtMatName);
            this.groupBox1.Controls.Add(this.srtTexName);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.textBox6);
            this.groupBox1.Controls.Add(this.textBox9);
            this.groupBox1.Controls.Add(this.textBox10);
            this.groupBox1.Controls.Add(this.textBox12);
            this.groupBox1.Controls.Add(this.srtLoopEnable);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.srtScaleX);
            this.groupBox1.Controls.Add(this.srtScaleY);
            this.groupBox1.Controls.Add(this.srtRot);
            this.groupBox1.Controls.Add(this.srtTransX);
            this.groupBox1.Controls.Add(this.srtTransY);
            this.groupBox1.Controls.Add(this.srtScaleSubtract);
            this.groupBox1.Controls.Add(this.srtScaleAdd);
            this.groupBox1.Controls.Add(this.srtScaleReplace);
            this.groupBox1.Controls.Add(this.srtRotSubtract);
            this.groupBox1.Controls.Add(this.srtRotAdd);
            this.groupBox1.Controls.Add(this.srtRotReplace);
            this.groupBox1.Controls.Add(this.srtTransSubtract);
            this.groupBox1.Controls.Add(this.srtTransAdd);
            this.groupBox1.Controls.Add(this.srtTransReplace);
            this.groupBox1.Controls.Add(this.srtCopyKF);
            this.groupBox1.Controls.Add(this.chkSrtVersion);
            this.groupBox1.Controls.Add(this.srtVersion);
            this.groupBox1.Controls.Add(this.srtEditLoop);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(396, 243);
            this.groupBox1.TabIndex = 86;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SRT0";
            // 
            // srtTexRename
            // 
            this.srtTexRename.AutoSize = true;
            this.srtTexRename.Location = new System.Drawing.Point(201, 103);
            this.srtTexRename.Name = "srtTexRename";
            this.srtTexRename.Size = new System.Drawing.Size(69, 17);
            this.srtTexRename.TabIndex = 112;
            this.srtTexRename.Text = "Rename:";
            this.srtTexRename.UseVisualStyleBackColor = true;
            // 
            // srtModMat
            // 
            this.srtModMat.AutoSize = true;
            this.srtModMat.Location = new System.Drawing.Point(200, 15);
            this.srtModMat.Name = "srtModMat";
            this.srtModMat.Size = new System.Drawing.Size(196, 17);
            this.srtModMat.TabIndex = 111;
            this.srtModMat.Text = "Only modify materials with the name:";
            this.srtModMat.UseVisualStyleBackColor = true;
            // 
            // textBox7
            // 
            this.textBox7.HideSelection = false;
            this.textBox7.Location = new System.Drawing.Point(200, 120);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(189, 20);
            this.textBox7.TabIndex = 110;
            // 
            // srtMatName
            // 
            this.srtMatName.HideSelection = false;
            this.srtMatName.Location = new System.Drawing.Point(200, 32);
            this.srtMatName.Name = "srtMatName";
            this.srtMatName.Size = new System.Drawing.Size(189, 20);
            this.srtMatName.TabIndex = 108;
            // 
            // srtTexName
            // 
            this.srtTexName.HideSelection = false;
            this.srtTexName.Location = new System.Drawing.Point(7, 32);
            this.srtTexName.Name = "srtTexName";
            this.srtTexName.Size = new System.Drawing.Size(187, 20);
            this.srtTexName.TabIndex = 82;
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(77, 80);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(119, 20);
            this.textBox3.TabIndex = 84;
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Location = new System.Drawing.Point(77, 101);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(119, 20);
            this.textBox4.TabIndex = 85;
            // 
            // textBox6
            // 
            this.textBox6.Enabled = false;
            this.textBox6.Location = new System.Drawing.Point(78, 146);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(119, 20);
            this.textBox6.TabIndex = 88;
            // 
            // textBox9
            // 
            this.textBox9.Enabled = false;
            this.textBox9.Location = new System.Drawing.Point(77, 195);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(119, 20);
            this.textBox9.TabIndex = 89;
            // 
            // textBox10
            // 
            this.textBox10.Enabled = false;
            this.textBox10.Location = new System.Drawing.Point(77, 216);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(119, 20);
            this.textBox10.TabIndex = 90;
            // 
            // textBox12
            // 
            this.textBox12.Enabled = false;
            this.textBox12.Location = new System.Drawing.Point(200, 80);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(189, 20);
            this.textBox12.TabIndex = 104;
            // 
            // srtLoopEnable
            // 
            this.srtLoopEnable.AutoSize = true;
            this.srtLoopEnable.Enabled = false;
            this.srtLoopEnable.Location = new System.Drawing.Point(275, 173);
            this.srtLoopEnable.Name = "srtLoopEnable";
            this.srtLoopEnable.Size = new System.Drawing.Size(92, 17);
            this.srtLoopEnable.TabIndex = 109;
            this.srtLoopEnable.Text = "Loop Enabled";
            this.srtLoopEnable.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 13);
            this.label2.TabIndex = 83;
            this.label2.Text = "Change all textures with the name:";
            // 
            // srtScaleX
            // 
            this.srtScaleX.AutoSize = true;
            this.srtScaleX.Location = new System.Drawing.Point(8, 83);
            this.srtScaleX.Name = "srtScaleX";
            this.srtScaleX.Size = new System.Drawing.Size(47, 13);
            this.srtScaleX.TabIndex = 86;
            this.srtScaleX.Text = "Scale X:";
            // 
            // srtScaleY
            // 
            this.srtScaleY.AutoSize = true;
            this.srtScaleY.Location = new System.Drawing.Point(7, 104);
            this.srtScaleY.Name = "srtScaleY";
            this.srtScaleY.Size = new System.Drawing.Size(47, 13);
            this.srtScaleY.TabIndex = 87;
            this.srtScaleY.Text = "Scale Y:";
            // 
            // srtRot
            // 
            this.srtRot.AutoSize = true;
            this.srtRot.Location = new System.Drawing.Point(8, 149);
            this.srtRot.Name = "srtRot";
            this.srtRot.Size = new System.Drawing.Size(50, 13);
            this.srtRot.TabIndex = 91;
            this.srtRot.Text = "Rotation:";
            // 
            // srtTransX
            // 
            this.srtTransX.AutoSize = true;
            this.srtTransX.Location = new System.Drawing.Point(7, 199);
            this.srtTransX.Name = "srtTransX";
            this.srtTransX.Size = new System.Drawing.Size(64, 13);
            this.srtTransX.TabIndex = 92;
            this.srtTransX.Text = "Translate X:";
            // 
            // srtTransY
            // 
            this.srtTransY.AutoSize = true;
            this.srtTransY.Location = new System.Drawing.Point(7, 221);
            this.srtTransY.Name = "srtTransY";
            this.srtTransY.Size = new System.Drawing.Size(64, 13);
            this.srtTransY.TabIndex = 93;
            this.srtTransY.Text = "Translate Y:";
            // 
            // srtScaleSubtract
            // 
            this.srtScaleSubtract.AutoSize = true;
            this.srtScaleSubtract.Location = new System.Drawing.Point(119, 58);
            this.srtScaleSubtract.Name = "srtScaleSubtract";
            this.srtScaleSubtract.Size = new System.Drawing.Size(66, 17);
            this.srtScaleSubtract.TabIndex = 96;
            this.srtScaleSubtract.Text = "Subtract";
            this.srtScaleSubtract.UseVisualStyleBackColor = true;
            // 
            // srtScaleAdd
            // 
            this.srtScaleAdd.AutoSize = true;
            this.srtScaleAdd.Location = new System.Drawing.Point(73, 58);
            this.srtScaleAdd.Name = "srtScaleAdd";
            this.srtScaleAdd.Size = new System.Drawing.Size(45, 17);
            this.srtScaleAdd.TabIndex = 95;
            this.srtScaleAdd.Text = "Add";
            this.srtScaleAdd.UseVisualStyleBackColor = true;
            // 
            // srtScaleReplace
            // 
            this.srtScaleReplace.AutoSize = true;
            this.srtScaleReplace.Location = new System.Drawing.Point(8, 58);
            this.srtScaleReplace.Name = "srtScaleReplace";
            this.srtScaleReplace.Size = new System.Drawing.Size(66, 17);
            this.srtScaleReplace.TabIndex = 94;
            this.srtScaleReplace.Text = "Replace";
            this.srtScaleReplace.UseVisualStyleBackColor = true;
            // 
            // srtRotSubtract
            // 
            this.srtRotSubtract.AutoSize = true;
            this.srtRotSubtract.Location = new System.Drawing.Point(119, 125);
            this.srtRotSubtract.Name = "srtRotSubtract";
            this.srtRotSubtract.Size = new System.Drawing.Size(66, 17);
            this.srtRotSubtract.TabIndex = 99;
            this.srtRotSubtract.Text = "Subtract";
            this.srtRotSubtract.UseVisualStyleBackColor = true;
            // 
            // srtRotAdd
            // 
            this.srtRotAdd.AutoSize = true;
            this.srtRotAdd.Location = new System.Drawing.Point(73, 125);
            this.srtRotAdd.Name = "srtRotAdd";
            this.srtRotAdd.Size = new System.Drawing.Size(45, 17);
            this.srtRotAdd.TabIndex = 98;
            this.srtRotAdd.Text = "Add";
            this.srtRotAdd.UseVisualStyleBackColor = true;
            // 
            // srtRotReplace
            // 
            this.srtRotReplace.AutoSize = true;
            this.srtRotReplace.Location = new System.Drawing.Point(8, 125);
            this.srtRotReplace.Name = "srtRotReplace";
            this.srtRotReplace.Size = new System.Drawing.Size(66, 17);
            this.srtRotReplace.TabIndex = 97;
            this.srtRotReplace.Text = "Replace";
            this.srtRotReplace.UseVisualStyleBackColor = true;
            // 
            // srtTransSubtract
            // 
            this.srtTransSubtract.AutoSize = true;
            this.srtTransSubtract.Location = new System.Drawing.Point(119, 173);
            this.srtTransSubtract.Name = "srtTransSubtract";
            this.srtTransSubtract.Size = new System.Drawing.Size(66, 17);
            this.srtTransSubtract.TabIndex = 102;
            this.srtTransSubtract.Text = "Subtract";
            this.srtTransSubtract.UseVisualStyleBackColor = true;
            // 
            // srtTransAdd
            // 
            this.srtTransAdd.AutoSize = true;
            this.srtTransAdd.Location = new System.Drawing.Point(73, 173);
            this.srtTransAdd.Name = "srtTransAdd";
            this.srtTransAdd.Size = new System.Drawing.Size(45, 17);
            this.srtTransAdd.TabIndex = 101;
            this.srtTransAdd.Text = "Add";
            this.srtTransAdd.UseVisualStyleBackColor = true;
            // 
            // srtTransReplace
            // 
            this.srtTransReplace.AutoSize = true;
            this.srtTransReplace.Location = new System.Drawing.Point(8, 173);
            this.srtTransReplace.Name = "srtTransReplace";
            this.srtTransReplace.Size = new System.Drawing.Size(66, 17);
            this.srtTransReplace.TabIndex = 100;
            this.srtTransReplace.Text = "Replace";
            this.srtTransReplace.UseVisualStyleBackColor = true;
            // 
            // srtCopyKF
            // 
            this.srtCopyKF.AutoSize = true;
            this.srtCopyKF.Location = new System.Drawing.Point(201, 58);
            this.srtCopyKF.Name = "srtCopyKF";
            this.srtCopyKF.Size = new System.Drawing.Size(127, 17);
            this.srtCopyKF.TabIndex = 103;
            this.srtCopyKF.Text = "Copy keyframes from:";
            this.srtCopyKF.UseVisualStyleBackColor = true;
            // 
            // chkSrtVersion
            // 
            this.chkSrtVersion.AutoSize = true;
            this.chkSrtVersion.Location = new System.Drawing.Point(201, 148);
            this.chkSrtVersion.Name = "chkSrtVersion";
            this.chkSrtVersion.Size = new System.Drawing.Size(103, 17);
            this.chkSrtVersion.TabIndex = 105;
            this.chkSrtVersion.Text = "Change version:";
            this.chkSrtVersion.UseVisualStyleBackColor = true;
            // 
            // srtVersion
            // 
            this.srtVersion.Enabled = false;
            this.srtVersion.FormattingEnabled = true;
            this.srtVersion.Items.AddRange(new object[] {
            "4",
            "5"});
            this.srtVersion.Location = new System.Drawing.Point(310, 146);
            this.srtVersion.Name = "srtVersion";
            this.srtVersion.Size = new System.Drawing.Size(79, 21);
            this.srtVersion.TabIndex = 106;
            // 
            // srtEditLoop
            // 
            this.srtEditLoop.AutoSize = true;
            this.srtEditLoop.Location = new System.Drawing.Point(201, 173);
            this.srtEditLoop.Name = "srtEditLoop";
            this.srtEditLoop.Size = new System.Drawing.Size(74, 17);
            this.srtEditLoop.TabIndex = 107;
            this.srtEditLoop.Text = "Edit Loop:";
            this.srtEditLoop.UseVisualStyleBackColor = true;
            // 
            // EditAllSRT0Editor
            // 
            this.Controls.Add(this.groupBox1);
            this.Name = "EditAllSRT0Editor";
            this.Size = new System.Drawing.Size(396, 243);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public EditAllSRT0Editor() { InitializeComponent(); }
    }
}
