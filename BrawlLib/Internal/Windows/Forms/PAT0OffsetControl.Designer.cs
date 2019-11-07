﻿﻿using System.Windows.Forms;
  using BrawlLib.Internal.Windows.Controls;

  namespace BrawlLib.Internal.Windows.Forms
{
    partial class PAT0OffsetControl
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
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.chkOffsetOtherTextures = new System.Windows.Forms.CheckBox();
            this.chkIncreaseFrames = new System.Windows.Forms.CheckBox();
            this.numNewCount = new NumericInputBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Offset Frames:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(175, 80);
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
            this.btnOkay.Location = new System.Drawing.Point(94, 80);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 5;
            this.btnOkay.Text = "&Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // chkOffsetOtherTextures
            // 
            this.chkOffsetOtherTextures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkOffsetOtherTextures.AutoSize = true;
            this.chkOffsetOtherTextures.Location = new System.Drawing.Point(34, 57);
            this.chkOffsetOtherTextures.Name = "chkOffsetOtherTextures";
            this.chkOffsetOtherTextures.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkOffsetOtherTextures.Size = new System.Drawing.Size(212, 17);
            this.chkOffsetOtherTextures.TabIndex = 9;
            this.chkOffsetOtherTextures.Text = "Offset frames in other textures/materials";
            this.chkOffsetOtherTextures.UseVisualStyleBackColor = true;
            // 
            // chkIncreaseFrames
            // 
            this.chkIncreaseFrames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIncreaseFrames.AutoSize = true;
            this.chkIncreaseFrames.Checked = true;
            this.chkIncreaseFrames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncreaseFrames.Location = new System.Drawing.Point(58, 37);
            this.chkIncreaseFrames.Name = "chkIncreaseFrames";
            this.chkIncreaseFrames.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkIncreaseFrames.Size = new System.Drawing.Size(188, 17);
            this.chkIncreaseFrames.TabIndex = 10;
            this.chkIncreaseFrames.Text = "Increase frame count by this offset";
            this.chkIncreaseFrames.UseVisualStyleBackColor = true;
            // 
            // numNewCount
            // 
            this.numNewCount.Integral = true;
            this.numNewCount.Location = new System.Drawing.Point(146, 12);
            this.numNewCount.MaximumValue = 3.402823E+38F;
            this.numNewCount.MinimumValue = -3.402823E+38F;
            this.numNewCount.Name = "numNewCount";
            this.numNewCount.Size = new System.Drawing.Size(100, 20);
            this.numNewCount.TabIndex = 3;
            this.numNewCount.Text = "0";
            // 
            // PAT0OffsetControl
            // 
            this.AcceptButton = this.btnOkay;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(258, 111);
            this.Controls.Add(this.chkIncreaseFrames);
            this.Controls.Add(this.chkOffsetOtherTextures);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.numNewCount);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PAT0OffsetControl";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PAT0 Offset Control";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private global::System.Windows.Forms.Label label2;
        private global::BrawlLib.Internal.Windows.Controls.NumericInputBox numNewCount;
        private Button btnCancel;
        private Button btnOkay;
        private CheckBox chkOffsetOtherTextures;
        private CheckBox chkIncreaseFrames;
    }
}