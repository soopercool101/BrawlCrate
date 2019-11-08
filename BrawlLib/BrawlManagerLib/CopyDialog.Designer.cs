namespace BrawlLib.BrawlManagerLib {
    partial class CopyDialog {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.lblPacExistingName = new System.Windows.Forms.Label();
            this.lblPacExistingMD5 = new System.Windows.Forms.Label();
            this.lblPacNewMD5 = new System.Windows.Forms.Label();
            this.lblPacNewName = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnYes = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnNo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "From:";
            // 
            // lblPacExistingName
            // 
            this.lblPacExistingName.AutoSize = true;
            this.lblPacExistingName.Location = new System.Drawing.Point(64, 13);
            this.lblPacExistingName.Name = "lblPacExistingName";
            this.lblPacExistingName.Size = new System.Drawing.Size(83, 13);
            this.lblPacExistingName.TabIndex = 1;
            this.lblPacExistingName.Text = "STGFINAL.PAC";
            // 
            // lblPacExistingMD5
            // 
            this.lblPacExistingMD5.AutoSize = true;
            this.lblPacExistingMD5.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPacExistingMD5.Location = new System.Drawing.Point(65, 26);
            this.lblPacExistingMD5.Name = "lblPacExistingMD5";
            this.lblPacExistingMD5.Size = new System.Drawing.Size(229, 11);
            this.lblPacExistingMD5.TabIndex = 2;
            this.lblPacExistingMD5.Text = "88a848834c99b37a4f75312c96c1ca5e";
            // 
            // lblPacNewMD5
            // 
            this.lblPacNewMD5.AutoSize = true;
            this.lblPacNewMD5.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPacNewMD5.Location = new System.Drawing.Point(65, 66);
            this.lblPacNewMD5.Name = "lblPacNewMD5";
            this.lblPacNewMD5.Size = new System.Drawing.Size(229, 11);
            this.lblPacNewMD5.TabIndex = 7;
            this.lblPacNewMD5.Text = "88a848834c99b37a4f75312c96c1ca5e";
            // 
            // lblPacNewName
            // 
            this.lblPacNewName.AutoSize = true;
            this.lblPacNewName.Location = new System.Drawing.Point(64, 53);
            this.lblPacNewName.Margin = new System.Windows.Forms.Padding(3, 16, 3, 0);
            this.lblPacNewName.Name = "lblPacNewName";
            this.lblPacNewName.Size = new System.Drawing.Size(83, 13);
            this.lblPacNewName.TabIndex = 6;
            this.lblPacNewName.Text = "STGFINAL.PAC";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 53);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Existing:";
            // 
            // btnYes
            // 
            this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnYes.Location = new System.Drawing.Point(194, 126);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 23);
            this.btnYes.TabIndex = 10;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(240, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Are you sure you want to replace the existing file?";
            // 
            // btnNo
            // 
            this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNo.Location = new System.Drawing.Point(275, 126);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 23);
            this.btnNo.TabIndex = 13;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = true;
            // 
            // CopyDialog
            // 
            this.AcceptButton = this.btnYes;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 161);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.lblPacNewMD5);
            this.Controls.Add(this.lblPacNewName);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblPacExistingMD5);
            this.Controls.Add(this.lblPacExistingName);
            this.Controls.Add(this.label1);
            this.Name = "CopyDialog";
            this.Text = "Copy / Move";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPacExistingName;
        private System.Windows.Forms.Label lblPacExistingMD5;
        private System.Windows.Forms.Label lblPacNewMD5;
        private System.Windows.Forms.Label lblPacNewName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnNo;
    }
}