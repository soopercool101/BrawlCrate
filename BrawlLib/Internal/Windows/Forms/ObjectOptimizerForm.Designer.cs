using BrawlLib.Internal.Windows.Controls;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    partial class ObjectOptimizerForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblPrevCount = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.chkPushCacheHits = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblPercentChange = new System.Windows.Forms.Label();
            this.chkUseStrips = new System.Windows.Forms.CheckBox();
            this.lblOldCount = new System.Windows.Forms.Label();
            this.lblNewCount = new System.Windows.Forms.Label();
            this.chkForceCCW = new System.Windows.Forms.CheckBox();
            this.chkAllowIncrease = new System.Windows.Forms.CheckBox();
            this.numMinStripLen = new NumericInputBox();
            this.numCacheSize = new NumericInputBox();
            this.SuspendLayout();
            // 
            // lblPrevCount
            // 
            this.lblPrevCount.Location = new System.Drawing.Point(126, 9);
            this.lblPrevCount.Name = "lblPrevCount";
            this.lblPrevCount.Size = new System.Drawing.Size(52, 20);
            this.lblPrevCount.TabIndex = 4;
            this.lblPrevCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(218, 98);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.Location = new System.Drawing.Point(137, 98);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 5;
            this.btnOkay.Text = "&Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // chkPushCacheHits
            // 
            this.chkPushCacheHits.AutoSize = true;
            this.chkPushCacheHits.Checked = true;
            this.chkPushCacheHits.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPushCacheHits.Location = new System.Drawing.Point(188, 8);
            this.chkPushCacheHits.Name = "chkPushCacheHits";
            this.chkPushCacheHits.Size = new System.Drawing.Size(105, 17);
            this.chkPushCacheHits.TabIndex = 7;
            this.chkPushCacheHits.Text = "Push Cache Hits";
            this.chkPushCacheHits.UseVisualStyleBackColor = true;
            this.chkPushCacheHits.CheckedChanged += new System.EventHandler(this.Update);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(56, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Cache Size:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(1, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 14);
            this.label2.TabIndex = 9;
            this.label2.Text = "Minimum Strip Length:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(36, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Old Point Count:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(30, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "New Point Count:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPercentChange
            // 
            this.lblPercentChange.AutoSize = true;
            this.lblPercentChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPercentChange.Location = new System.Drawing.Point(12, 98);
            this.lblPercentChange.Name = "lblPercentChange";
            this.lblPercentChange.Size = new System.Drawing.Size(90, 16);
            this.lblPercentChange.TabIndex = 14;
            this.lblPercentChange.Text = "0% Decrease";
            this.lblPercentChange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkUseStrips
            // 
            this.chkUseStrips.AutoSize = true;
            this.chkUseStrips.Checked = true;
            this.chkUseStrips.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseStrips.Location = new System.Drawing.Point(188, 29);
            this.chkUseStrips.Name = "chkUseStrips";
            this.chkUseStrips.Size = new System.Drawing.Size(84, 17);
            this.chkUseStrips.TabIndex = 15;
            this.chkUseStrips.Text = "Use Tristrips";
            this.chkUseStrips.UseVisualStyleBackColor = true;
            this.chkUseStrips.CheckedChanged += new System.EventHandler(this.Update);
            // 
            // lblOldCount
            // 
            this.lblOldCount.AutoSize = true;
            this.lblOldCount.Location = new System.Drawing.Point(129, 54);
            this.lblOldCount.Name = "lblOldCount";
            this.lblOldCount.Size = new System.Drawing.Size(13, 13);
            this.lblOldCount.TabIndex = 16;
            this.lblOldCount.Text = "0";
            this.lblOldCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNewCount
            // 
            this.lblNewCount.AutoSize = true;
            this.lblNewCount.Location = new System.Drawing.Point(129, 77);
            this.lblNewCount.Name = "lblNewCount";
            this.lblNewCount.Size = new System.Drawing.Size(13, 13);
            this.lblNewCount.TabIndex = 17;
            this.lblNewCount.Text = "0";
            this.lblNewCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkForceCCW
            // 
            this.chkForceCCW.AutoSize = true;
            this.chkForceCCW.Checked = true;
            this.chkForceCCW.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkForceCCW.Location = new System.Drawing.Point(188, 50);
            this.chkForceCCW.Name = "chkForceCCW";
            this.chkForceCCW.Size = new System.Drawing.Size(81, 17);
            this.chkForceCCW.TabIndex = 18;
            this.chkForceCCW.Text = "Force CCW";
            this.chkForceCCW.UseVisualStyleBackColor = true;
            this.chkForceCCW.CheckedChanged += new System.EventHandler(this.Update);
            // 
            // chkAllowIncrease
            // 
            this.chkAllowIncrease.AutoSize = true;
            this.chkAllowIncrease.Location = new System.Drawing.Point(188, 72);
            this.chkAllowIncrease.Name = "chkAllowIncrease";
            this.chkAllowIncrease.Size = new System.Drawing.Size(95, 17);
            this.chkAllowIncrease.TabIndex = 19;
            this.chkAllowIncrease.Text = "Allow Increase";
            this.chkAllowIncrease.UseVisualStyleBackColor = true;
            this.chkAllowIncrease.CheckedChanged += new System.EventHandler(this.Update);
            // 
            // numMinStripLen
            // 
            this.numMinStripLen.Integral = true;
            this.numMinStripLen.Location = new System.Drawing.Point(129, 29);
            this.numMinStripLen.MaximumValue = 3.402823E+38F;
            this.numMinStripLen.MinimumValue = 2F;
            this.numMinStripLen.Name = "numMinStripLen";
            this.numMinStripLen.Size = new System.Drawing.Size(51, 20);
            this.numMinStripLen.TabIndex = 12;
            this.numMinStripLen.Text = "2";
            this.numMinStripLen.ValueChanged += new System.EventHandler(this.Update);
            // 
            // numCacheSize
            // 
            this.numCacheSize.Integral = true;
            this.numCacheSize.Location = new System.Drawing.Point(129, 6);
            this.numCacheSize.MaximumValue = 3.402823E+38F;
            this.numCacheSize.MinimumValue = 0F;
            this.numCacheSize.Name = "numCacheSize";
            this.numCacheSize.Size = new System.Drawing.Size(51, 20);
            this.numCacheSize.TabIndex = 11;
            this.numCacheSize.Text = "52";
            this.numCacheSize.ValueChanged += new System.EventHandler(this.Update);
            // 
            // ObjectOptimizerForm
            // 
            this.AcceptButton = this.btnOkay;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(305, 126);
            this.Controls.Add(this.chkAllowIncrease);
            this.Controls.Add(this.chkForceCCW);
            this.Controls.Add(this.lblNewCount);
            this.Controls.Add(this.lblOldCount);
            this.Controls.Add(this.chkUseStrips);
            this.Controls.Add(this.lblPercentChange);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numMinStripLen);
            this.Controls.Add(this.numCacheSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkPushCacheHits);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.lblPrevCount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ObjectOptimizerForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mesh Optimizer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private global::System.Windows.Forms.Label lblPrevCount;
        private Button btnCancel;
        private Button btnOkay;
        private CheckBox chkPushCacheHits;
        private Label label1;
        private Label label2;
        private Label label3;
        private NumericInputBox numCacheSize;
        private NumericInputBox numMinStripLen;
        private Label label4;
        private Label lblPercentChange;
        private CheckBox chkUseStrips;
        private Label lblOldCount;
        private Label lblNewCount;
        private CheckBox chkForceCCW;
        private CheckBox chkAllowIncrease;
    }
}