namespace BrawlStageManager {
	partial class NameCreatorDialog {
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblCurrentFont = new System.Windows.Forms.Label();
			this.btnImpact = new System.Windows.Forms.Button();
			this.btnEdo = new System.Windows.Forms.Button();
			this.btnCustom = new System.Windows.Forms.Button();
			this.nudOffset = new System.Windows.Forms.NumericUpDown();
			this.lblImpactOffset = new System.Windows.Forms.Label();
			this.lblEdoOffset = new System.Windows.Forms.Label();
			this.btnClearFont = new System.Windows.Forms.Button();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.btnOkay = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.chkCorner = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOffset)).BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel1.Controls.Add(this.lblCurrentFont, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnImpact, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnEdo, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnCustom, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.nudOffset, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblImpactOffset, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblEdoOffset, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnClearFont, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.chkCorner, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.label1, 1, 4);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 6;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(254, 201);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// lblCurrentFont
			// 
			this.lblCurrentFont.AutoSize = true;
			this.lblCurrentFont.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblCurrentFont.Location = new System.Drawing.Point(3, 0);
			this.lblCurrentFont.Name = "lblCurrentFont";
			this.lblCurrentFont.Size = new System.Drawing.Size(188, 33);
			this.lblCurrentFont.TabIndex = 0;
			this.lblCurrentFont.Text = "No font selected";
			this.lblCurrentFont.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnImpact
			// 
			this.btnImpact.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnImpact.Location = new System.Drawing.Point(3, 36);
			this.btnImpact.Name = "btnImpact";
			this.btnImpact.Size = new System.Drawing.Size(188, 27);
			this.btnImpact.TabIndex = 1;
			this.btnImpact.Text = "22.5pt Impact";
			this.btnImpact.UseVisualStyleBackColor = true;
			this.btnImpact.Click += new System.EventHandler(this.btnImpact_Click);
			// 
			// btnEdo
			// 
			this.btnEdo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnEdo.Location = new System.Drawing.Point(3, 69);
			this.btnEdo.Name = "btnEdo";
			this.btnEdo.Size = new System.Drawing.Size(188, 27);
			this.btnEdo.TabIndex = 2;
			this.btnEdo.Text = "22pt Edo SZ Bold";
			this.btnEdo.UseVisualStyleBackColor = true;
			this.btnEdo.Click += new System.EventHandler(this.btnEdo_Click);
			// 
			// btnCustom
			// 
			this.btnCustom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnCustom.Location = new System.Drawing.Point(3, 102);
			this.btnCustom.Name = "btnCustom";
			this.btnCustom.Size = new System.Drawing.Size(188, 27);
			this.btnCustom.TabIndex = 3;
			this.btnCustom.Text = "Custom font...";
			this.btnCustom.UseVisualStyleBackColor = true;
			this.btnCustom.Click += new System.EventHandler(this.btnCustom_Click);
			// 
			// nudOffset
			// 
			this.nudOffset.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nudOffset.Location = new System.Drawing.Point(197, 102);
			this.nudOffset.Maximum = new decimal(new int[] {
            56,
            0,
            0,
            0});
			this.nudOffset.Minimum = new decimal(new int[] {
            56,
            0,
            0,
            -2147483648});
			this.nudOffset.Name = "nudOffset";
			this.nudOffset.Size = new System.Drawing.Size(54, 20);
			this.nudOffset.TabIndex = 4;
			// 
			// lblImpactOffset
			// 
			this.lblImpactOffset.AutoSize = true;
			this.lblImpactOffset.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblImpactOffset.Location = new System.Drawing.Point(197, 33);
			this.lblImpactOffset.Name = "lblImpactOffset";
			this.lblImpactOffset.Size = new System.Drawing.Size(54, 33);
			this.lblImpactOffset.TabIndex = 5;
			this.lblImpactOffset.Text = "-1 px";
			this.lblImpactOffset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblEdoOffset
			// 
			this.lblEdoOffset.AutoSize = true;
			this.lblEdoOffset.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblEdoOffset.Location = new System.Drawing.Point(197, 66);
			this.lblEdoOffset.Name = "lblEdoOffset";
			this.lblEdoOffset.Size = new System.Drawing.Size(54, 33);
			this.lblEdoOffset.TabIndex = 6;
			this.lblEdoOffset.Text = "+2 px";
			this.lblEdoOffset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnClearFont
			// 
			this.btnClearFont.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnClearFont.Location = new System.Drawing.Point(227, 3);
			this.btnClearFont.Name = "btnClearFont";
			this.btnClearFont.Size = new System.Drawing.Size(24, 27);
			this.btnClearFont.TabIndex = 7;
			this.btnClearFont.Text = "X";
			this.btnClearFont.UseVisualStyleBackColor = true;
			this.btnClearFont.Click += new System.EventHandler(this.btnClearFont_Click);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this.btnOkay, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.btnCancel, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 168);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(248, 30);
			this.tableLayoutPanel2.TabIndex = 8;
			// 
			// btnOkay
			// 
			this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOkay.Location = new System.Drawing.Point(127, 3);
			this.btnOkay.Name = "btnOkay";
			this.btnOkay.Size = new System.Drawing.Size(75, 22);
			this.btnOkay.TabIndex = 0;
			this.btnOkay.Text = "OK";
			this.btnOkay.UseVisualStyleBackColor = true;
			this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnCancel.Location = new System.Drawing.Point(46, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 24);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// chkCorner
			// 
			this.chkCorner.AutoSize = true;
			this.chkCorner.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkCorner.Location = new System.Drawing.Point(3, 135);
			this.chkCorner.Name = "chkCorner";
			this.chkCorner.Size = new System.Drawing.Size(188, 27);
			this.chkCorner.TabIndex = 9;
			this.chkCorner.Text = "Put text in lower-left corner";
			this.chkCorner.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(197, 132);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(54, 33);
			this.label1.TabIndex = 10;
			this.label1.DoubleClick += new System.EventHandler(this.label1_DoubleClick);
			// 
			// NameCreatorDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(254, 201);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "NameCreatorDialog";
			this.Text = "Choose a font";
			this.Load += new System.EventHandler(this.NameCreatorDialog_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOffset)).EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblCurrentFont;
		private System.Windows.Forms.Button btnImpact;
		private System.Windows.Forms.Button btnEdo;
		private System.Windows.Forms.Button btnCustom;
		private System.Windows.Forms.NumericUpDown nudOffset;
		private System.Windows.Forms.Label lblImpactOffset;
		private System.Windows.Forms.Label lblEdoOffset;
		private System.Windows.Forms.Button btnClearFont;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button btnOkay;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox chkCorner;
		private System.Windows.Forms.Label label1;
	}
}