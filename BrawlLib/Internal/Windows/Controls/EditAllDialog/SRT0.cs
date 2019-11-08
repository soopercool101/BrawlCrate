using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.EditAllDialog
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
            groupBox1 = new GroupBox();
            srtTexRename = new CheckBox();
            srtModMat = new CheckBox();
            textBox7 = new TextBox();
            srtMatName = new TextBox();
            srtTexName = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            textBox6 = new TextBox();
            textBox9 = new TextBox();
            textBox10 = new TextBox();
            textBox12 = new TextBox();
            srtLoopEnable = new CheckBox();
            label2 = new Label();
            srtScaleX = new Label();
            srtScaleY = new Label();
            srtRot = new Label();
            srtTransX = new Label();
            srtTransY = new Label();
            srtScaleSubtract = new CheckBox();
            srtScaleAdd = new CheckBox();
            srtScaleReplace = new CheckBox();
            srtRotSubtract = new CheckBox();
            srtRotAdd = new CheckBox();
            srtRotReplace = new CheckBox();
            srtTransSubtract = new CheckBox();
            srtTransAdd = new CheckBox();
            srtTransReplace = new CheckBox();
            srtCopyKF = new CheckBox();
            chkSrtVersion = new CheckBox();
            srtVersion = new ComboBox();
            srtEditLoop = new CheckBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(srtTexRename);
            groupBox1.Controls.Add(srtModMat);
            groupBox1.Controls.Add(textBox7);
            groupBox1.Controls.Add(srtMatName);
            groupBox1.Controls.Add(srtTexName);
            groupBox1.Controls.Add(textBox3);
            groupBox1.Controls.Add(textBox4);
            groupBox1.Controls.Add(textBox6);
            groupBox1.Controls.Add(textBox9);
            groupBox1.Controls.Add(textBox10);
            groupBox1.Controls.Add(textBox12);
            groupBox1.Controls.Add(srtLoopEnable);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(srtScaleX);
            groupBox1.Controls.Add(srtScaleY);
            groupBox1.Controls.Add(srtRot);
            groupBox1.Controls.Add(srtTransX);
            groupBox1.Controls.Add(srtTransY);
            groupBox1.Controls.Add(srtScaleSubtract);
            groupBox1.Controls.Add(srtScaleAdd);
            groupBox1.Controls.Add(srtScaleReplace);
            groupBox1.Controls.Add(srtRotSubtract);
            groupBox1.Controls.Add(srtRotAdd);
            groupBox1.Controls.Add(srtRotReplace);
            groupBox1.Controls.Add(srtTransSubtract);
            groupBox1.Controls.Add(srtTransAdd);
            groupBox1.Controls.Add(srtTransReplace);
            groupBox1.Controls.Add(srtCopyKF);
            groupBox1.Controls.Add(chkSrtVersion);
            groupBox1.Controls.Add(srtVersion);
            groupBox1.Controls.Add(srtEditLoop);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(396, 243);
            groupBox1.TabIndex = 86;
            groupBox1.TabStop = false;
            groupBox1.Text = "SRT0";
            // 
            // srtTexRename
            // 
            srtTexRename.AutoSize = true;
            srtTexRename.Location = new System.Drawing.Point(201, 103);
            srtTexRename.Name = "srtTexRename";
            srtTexRename.Size = new System.Drawing.Size(69, 17);
            srtTexRename.TabIndex = 112;
            srtTexRename.Text = "Rename:";
            srtTexRename.UseVisualStyleBackColor = true;
            // 
            // srtModMat
            // 
            srtModMat.AutoSize = true;
            srtModMat.Location = new System.Drawing.Point(200, 15);
            srtModMat.Name = "srtModMat";
            srtModMat.Size = new System.Drawing.Size(196, 17);
            srtModMat.TabIndex = 111;
            srtModMat.Text = "Only modify materials with the name:";
            srtModMat.UseVisualStyleBackColor = true;
            // 
            // textBox7
            // 
            textBox7.HideSelection = false;
            textBox7.Location = new System.Drawing.Point(200, 120);
            textBox7.Name = "textBox7";
            textBox7.Size = new System.Drawing.Size(189, 20);
            textBox7.TabIndex = 110;
            // 
            // srtMatName
            // 
            srtMatName.HideSelection = false;
            srtMatName.Location = new System.Drawing.Point(200, 32);
            srtMatName.Name = "srtMatName";
            srtMatName.Size = new System.Drawing.Size(189, 20);
            srtMatName.TabIndex = 108;
            // 
            // srtTexName
            // 
            srtTexName.HideSelection = false;
            srtTexName.Location = new System.Drawing.Point(7, 32);
            srtTexName.Name = "srtTexName";
            srtTexName.Size = new System.Drawing.Size(187, 20);
            srtTexName.TabIndex = 82;
            // 
            // textBox3
            // 
            textBox3.Enabled = false;
            textBox3.Location = new System.Drawing.Point(77, 80);
            textBox3.Name = "textBox3";
            textBox3.Size = new System.Drawing.Size(119, 20);
            textBox3.TabIndex = 84;
            // 
            // textBox4
            // 
            textBox4.Enabled = false;
            textBox4.Location = new System.Drawing.Point(77, 101);
            textBox4.Name = "textBox4";
            textBox4.Size = new System.Drawing.Size(119, 20);
            textBox4.TabIndex = 85;
            // 
            // textBox6
            // 
            textBox6.Enabled = false;
            textBox6.Location = new System.Drawing.Point(78, 146);
            textBox6.Name = "textBox6";
            textBox6.Size = new System.Drawing.Size(119, 20);
            textBox6.TabIndex = 88;
            // 
            // textBox9
            // 
            textBox9.Enabled = false;
            textBox9.Location = new System.Drawing.Point(77, 195);
            textBox9.Name = "textBox9";
            textBox9.Size = new System.Drawing.Size(119, 20);
            textBox9.TabIndex = 89;
            // 
            // textBox10
            // 
            textBox10.Enabled = false;
            textBox10.Location = new System.Drawing.Point(77, 216);
            textBox10.Name = "textBox10";
            textBox10.Size = new System.Drawing.Size(119, 20);
            textBox10.TabIndex = 90;
            // 
            // textBox12
            // 
            textBox12.Enabled = false;
            textBox12.Location = new System.Drawing.Point(200, 80);
            textBox12.Name = "textBox12";
            textBox12.Size = new System.Drawing.Size(189, 20);
            textBox12.TabIndex = 104;
            // 
            // srtLoopEnable
            // 
            srtLoopEnable.AutoSize = true;
            srtLoopEnable.Enabled = false;
            srtLoopEnable.Location = new System.Drawing.Point(275, 173);
            srtLoopEnable.Name = "srtLoopEnable";
            srtLoopEnable.Size = new System.Drawing.Size(92, 17);
            srtLoopEnable.TabIndex = 109;
            srtLoopEnable.Text = "Loop Enabled";
            srtLoopEnable.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 16);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(169, 13);
            label2.TabIndex = 83;
            label2.Text = "Change all textures with the name:";
            // 
            // srtScaleX
            // 
            srtScaleX.AutoSize = true;
            srtScaleX.Location = new System.Drawing.Point(8, 83);
            srtScaleX.Name = "srtScaleX";
            srtScaleX.Size = new System.Drawing.Size(47, 13);
            srtScaleX.TabIndex = 86;
            srtScaleX.Text = "Scale X:";
            // 
            // srtScaleY
            // 
            srtScaleY.AutoSize = true;
            srtScaleY.Location = new System.Drawing.Point(7, 104);
            srtScaleY.Name = "srtScaleY";
            srtScaleY.Size = new System.Drawing.Size(47, 13);
            srtScaleY.TabIndex = 87;
            srtScaleY.Text = "Scale Y:";
            // 
            // srtRot
            // 
            srtRot.AutoSize = true;
            srtRot.Location = new System.Drawing.Point(8, 149);
            srtRot.Name = "srtRot";
            srtRot.Size = new System.Drawing.Size(50, 13);
            srtRot.TabIndex = 91;
            srtRot.Text = "Rotation:";
            // 
            // srtTransX
            // 
            srtTransX.AutoSize = true;
            srtTransX.Location = new System.Drawing.Point(7, 199);
            srtTransX.Name = "srtTransX";
            srtTransX.Size = new System.Drawing.Size(64, 13);
            srtTransX.TabIndex = 92;
            srtTransX.Text = "Translate X:";
            // 
            // srtTransY
            // 
            srtTransY.AutoSize = true;
            srtTransY.Location = new System.Drawing.Point(7, 221);
            srtTransY.Name = "srtTransY";
            srtTransY.Size = new System.Drawing.Size(64, 13);
            srtTransY.TabIndex = 93;
            srtTransY.Text = "Translate Y:";
            // 
            // srtScaleSubtract
            // 
            srtScaleSubtract.AutoSize = true;
            srtScaleSubtract.Location = new System.Drawing.Point(119, 58);
            srtScaleSubtract.Name = "srtScaleSubtract";
            srtScaleSubtract.Size = new System.Drawing.Size(66, 17);
            srtScaleSubtract.TabIndex = 96;
            srtScaleSubtract.Text = "Subtract";
            srtScaleSubtract.UseVisualStyleBackColor = true;
            // 
            // srtScaleAdd
            // 
            srtScaleAdd.AutoSize = true;
            srtScaleAdd.Location = new System.Drawing.Point(73, 58);
            srtScaleAdd.Name = "srtScaleAdd";
            srtScaleAdd.Size = new System.Drawing.Size(45, 17);
            srtScaleAdd.TabIndex = 95;
            srtScaleAdd.Text = "Add";
            srtScaleAdd.UseVisualStyleBackColor = true;
            // 
            // srtScaleReplace
            // 
            srtScaleReplace.AutoSize = true;
            srtScaleReplace.Location = new System.Drawing.Point(8, 58);
            srtScaleReplace.Name = "srtScaleReplace";
            srtScaleReplace.Size = new System.Drawing.Size(66, 17);
            srtScaleReplace.TabIndex = 94;
            srtScaleReplace.Text = "Replace";
            srtScaleReplace.UseVisualStyleBackColor = true;
            // 
            // srtRotSubtract
            // 
            srtRotSubtract.AutoSize = true;
            srtRotSubtract.Location = new System.Drawing.Point(119, 125);
            srtRotSubtract.Name = "srtRotSubtract";
            srtRotSubtract.Size = new System.Drawing.Size(66, 17);
            srtRotSubtract.TabIndex = 99;
            srtRotSubtract.Text = "Subtract";
            srtRotSubtract.UseVisualStyleBackColor = true;
            // 
            // srtRotAdd
            // 
            srtRotAdd.AutoSize = true;
            srtRotAdd.Location = new System.Drawing.Point(73, 125);
            srtRotAdd.Name = "srtRotAdd";
            srtRotAdd.Size = new System.Drawing.Size(45, 17);
            srtRotAdd.TabIndex = 98;
            srtRotAdd.Text = "Add";
            srtRotAdd.UseVisualStyleBackColor = true;
            // 
            // srtRotReplace
            // 
            srtRotReplace.AutoSize = true;
            srtRotReplace.Location = new System.Drawing.Point(8, 125);
            srtRotReplace.Name = "srtRotReplace";
            srtRotReplace.Size = new System.Drawing.Size(66, 17);
            srtRotReplace.TabIndex = 97;
            srtRotReplace.Text = "Replace";
            srtRotReplace.UseVisualStyleBackColor = true;
            // 
            // srtTransSubtract
            // 
            srtTransSubtract.AutoSize = true;
            srtTransSubtract.Location = new System.Drawing.Point(119, 173);
            srtTransSubtract.Name = "srtTransSubtract";
            srtTransSubtract.Size = new System.Drawing.Size(66, 17);
            srtTransSubtract.TabIndex = 102;
            srtTransSubtract.Text = "Subtract";
            srtTransSubtract.UseVisualStyleBackColor = true;
            // 
            // srtTransAdd
            // 
            srtTransAdd.AutoSize = true;
            srtTransAdd.Location = new System.Drawing.Point(73, 173);
            srtTransAdd.Name = "srtTransAdd";
            srtTransAdd.Size = new System.Drawing.Size(45, 17);
            srtTransAdd.TabIndex = 101;
            srtTransAdd.Text = "Add";
            srtTransAdd.UseVisualStyleBackColor = true;
            // 
            // srtTransReplace
            // 
            srtTransReplace.AutoSize = true;
            srtTransReplace.Location = new System.Drawing.Point(8, 173);
            srtTransReplace.Name = "srtTransReplace";
            srtTransReplace.Size = new System.Drawing.Size(66, 17);
            srtTransReplace.TabIndex = 100;
            srtTransReplace.Text = "Replace";
            srtTransReplace.UseVisualStyleBackColor = true;
            // 
            // srtCopyKF
            // 
            srtCopyKF.AutoSize = true;
            srtCopyKF.Location = new System.Drawing.Point(201, 58);
            srtCopyKF.Name = "srtCopyKF";
            srtCopyKF.Size = new System.Drawing.Size(127, 17);
            srtCopyKF.TabIndex = 103;
            srtCopyKF.Text = "Copy keyframes from:";
            srtCopyKF.UseVisualStyleBackColor = true;
            // 
            // chkSrtVersion
            // 
            chkSrtVersion.AutoSize = true;
            chkSrtVersion.Location = new System.Drawing.Point(201, 148);
            chkSrtVersion.Name = "chkSrtVersion";
            chkSrtVersion.Size = new System.Drawing.Size(103, 17);
            chkSrtVersion.TabIndex = 105;
            chkSrtVersion.Text = "Change version:";
            chkSrtVersion.UseVisualStyleBackColor = true;
            // 
            // srtVersion
            // 
            srtVersion.Enabled = false;
            srtVersion.FormattingEnabled = true;
            srtVersion.Items.AddRange(new object[]
            {
                "4",
                "5"
            });
            srtVersion.Location = new System.Drawing.Point(310, 146);
            srtVersion.Name = "srtVersion";
            srtVersion.Size = new System.Drawing.Size(79, 21);
            srtVersion.TabIndex = 106;
            // 
            // srtEditLoop
            // 
            srtEditLoop.AutoSize = true;
            srtEditLoop.Location = new System.Drawing.Point(201, 173);
            srtEditLoop.Name = "srtEditLoop";
            srtEditLoop.Size = new System.Drawing.Size(74, 17);
            srtEditLoop.TabIndex = 107;
            srtEditLoop.Text = "Edit Loop:";
            srtEditLoop.UseVisualStyleBackColor = true;
            // 
            // EditAllSRT0Editor
            // 
            Controls.Add(groupBox1);
            Name = "EditAllSRT0Editor";
            Size = new System.Drawing.Size(396, 243);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        public EditAllSRT0Editor()
        {
            InitializeComponent();
        }
    }
}