using BrawlLib.Internal.Windows.Controls;
using BrawlLib.Internal.Windows.Controls.Model_Panel;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    partial class MDL0ObjectControl
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
            ModelPanelViewport modelPanelViewport1 = new ModelPanelViewport();
            BrawlLib.OpenGL.GLCamera glCamera1 = new BrawlLib.OpenGL.GLCamera();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lstDrawCalls = new System.Windows.Forms.CheckedListBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.numDrawOrder = new NumericInputBox();
            this.chkDoesntMatter = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.cboDrawPass = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cboVisBone = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cboMaterial = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.shadersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bonesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.floorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.modelPanel = new ModelPanel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lstDrawCalls);
            this.splitContainer2.Panel1.Controls.Add(this.panel5);
            this.splitContainer2.Panel1.Controls.Add(this.panel1);
            this.splitContainer2.Panel1MinSize = 140;
            this.splitContainer2.Size = new System.Drawing.Size(214, 315);
            this.splitContainer2.SplitterDistance = 140;
            this.splitContainer2.TabIndex = 0;
            // 
            // lstDrawCalls
            // 
            this.lstDrawCalls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDrawCalls.FormattingEnabled = true;
            this.lstDrawCalls.IntegralHeight = false;
            this.lstDrawCalls.Location = new System.Drawing.Point(0, 23);
            this.lstDrawCalls.Name = "lstDrawCalls";
            this.lstDrawCalls.Size = new System.Drawing.Size(214, 33);
            this.lstDrawCalls.TabIndex = 0;
            this.lstDrawCalls.SelectedIndexChanged += new System.EventHandler(this.lstDrawCalls_SelectedIndexChanged);
            this.lstDrawCalls.DoubleClick += new System.EventHandler(this.lstDrawCalls_DoubleClick);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnDelete);
            this.panel5.Controls.Add(this.btnNew);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(214, 23);
            this.panel5.TabIndex = 4;
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDelete.Location = new System.Drawing.Point(75, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnNew
            // 
            this.btnNew.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnNew.Location = new System.Drawing.Point(0, 0);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(214, 84);
            this.panel1.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.numDrawOrder);
            this.panel4.Controls.Add(this.chkDoesntMatter);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 63);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(214, 21);
            this.panel4.TabIndex = 2;
            // 
            // numDrawOrder
            // 
            this.numDrawOrder.Dock = System.Windows.Forms.DockStyle.Left;
            this.numDrawOrder.Enabled = false;
            this.numDrawOrder.Integral = true;
            this.numDrawOrder.Location = new System.Drawing.Point(85, 0);
            this.numDrawOrder.MaximumValue = 255F;
            this.numDrawOrder.MinimumValue = 1F;
            this.numDrawOrder.Name = "numDrawOrder";
            this.numDrawOrder.Size = new System.Drawing.Size(31, 20);
            this.numDrawOrder.TabIndex = 5;
            this.numDrawOrder.Text = "0";
            this.numDrawOrder.ValueChanged += new System.EventHandler(this.numDrawOrder_ValueChanged);
            // 
            // chkDoesntMatter
            // 
            this.chkDoesntMatter.AutoSize = true;
            this.chkDoesntMatter.Location = new System.Drawing.Point(118, 3);
            this.chkDoesntMatter.Name = "chkDoesntMatter";
            this.chkDoesntMatter.Size = new System.Drawing.Size(94, 17);
            this.chkDoesntMatter.TabIndex = 6;
            this.chkDoesntMatter.Text = "Doesn\'t matter";
            this.chkDoesntMatter.UseVisualStyleBackColor = true;
            this.chkDoesntMatter.CheckedChanged += new System.EventHandler(this.chkDoesntMatter_CheckedChanged);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = "Draw Priority:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.cboDrawPass);
            this.panel6.Controls.Add(this.label4);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 42);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(214, 21);
            this.panel6.TabIndex = 7;
            // 
            // cboDrawPass
            // 
            this.cboDrawPass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboDrawPass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDrawPass.FormattingEnabled = true;
            this.cboDrawPass.Items.AddRange(new object[] {
            "OPA (Opaque)",
            "XLU (Transparent)"});
            this.cboDrawPass.Location = new System.Drawing.Point(85, 0);
            this.cboDrawPass.Name = "cboDrawPass";
            this.cboDrawPass.Size = new System.Drawing.Size(129, 21);
            this.cboDrawPass.TabIndex = 5;
            this.cboDrawPass.SelectedIndexChanged += new System.EventHandler(this.cboDrawPass_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 21);
            this.label4.TabIndex = 4;
            this.label4.Text = "Draw Pass:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cboVisBone);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 21);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(214, 21);
            this.panel3.TabIndex = 1;
            // 
            // cboVisBone
            // 
            this.cboVisBone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboVisBone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVisBone.FormattingEnabled = true;
            this.cboVisBone.Location = new System.Drawing.Point(85, 0);
            this.cboVisBone.Name = "cboVisBone";
            this.cboVisBone.Size = new System.Drawing.Size(129, 21);
            this.cboVisBone.TabIndex = 3;
            this.cboVisBone.SelectedIndexChanged += new System.EventHandler(this.cboVisBone_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "Visibility Bone:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cboMaterial);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(214, 21);
            this.panel2.TabIndex = 0;
            // 
            // cboMaterial
            // 
            this.cboMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMaterial.FormattingEnabled = true;
            this.cboMaterial.Location = new System.Drawing.Point(85, 0);
            this.cboMaterial.Name = "cboMaterial";
            this.cboMaterial.Size = new System.Drawing.Size(129, 21);
            this.cboMaterial.TabIndex = 1;
            this.cboMaterial.SelectedIndexChanged += new System.EventHandler(this.cboMaterial_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Material:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shadersToolStripMenuItem,
            this.bonesToolStripMenuItem,
            this.floorToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(254, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // shadersToolStripMenuItem
            // 
            this.shadersToolStripMenuItem.Name = "shadersToolStripMenuItem";
            this.shadersToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.shadersToolStripMenuItem.Text = "Shaders";
            this.shadersToolStripMenuItem.Click += new System.EventHandler(this.shadersToolStripMenuItem_Click);
            // 
            // bonesToolStripMenuItem
            // 
            this.bonesToolStripMenuItem.Name = "bonesToolStripMenuItem";
            this.bonesToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.bonesToolStripMenuItem.Text = "Bones";
            this.bonesToolStripMenuItem.Click += new System.EventHandler(this.bonesToolStripMenuItem_Click);
            // 
            // floorToolStripMenuItem
            // 
            this.floorToolStripMenuItem.Name = "floorToolStripMenuItem";
            this.floorToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.floorToolStripMenuItem.Text = "Floor";
            this.floorToolStripMenuItem.Click += new System.EventHandler(this.floorToolStripMenuItem_Click);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.splitContainer2);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.MinimumSize = new System.Drawing.Size(214, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(214, 315);
            this.panel7.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.modelPanel);
            this.panel8.Controls.Add(this.menuStrip1);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(217, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(254, 315);
            this.panel8.TabIndex = 0;
            // 
            // modelPanel
            // 
            modelPanelViewport1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            modelPanelViewport1.BackgroundImage = null;
            modelPanelViewport1.BackgroundImageType = BrawlLib.OpenGL.BGImageType.Stretch;
            glCamera1.Aspect = 0.8063492F;
            glCamera1.FarDepth = 200000F;
            glCamera1.Height = 315F;
            glCamera1.NearDepth = 1F;
            glCamera1.Orthographic = false;
            glCamera1.VerticalFieldOfView = 45F;
            glCamera1.Width = 254F;
            modelPanelViewport1.Camera = glCamera1;
            modelPanelViewport1.Enabled = true;
            modelPanelViewport1.Region = new System.Drawing.Rectangle(0, 0, 254, 315);
            modelPanelViewport1.RotationScale = 0.4F;
            modelPanelViewport1.TranslationScale = 0.05F;
            modelPanelViewport1.ViewType = BrawlLib.OpenGL.ViewportProjection.Perspective;
            modelPanelViewport1.ZoomScale = 2.5F;
            this.modelPanel.CurrentViewport = modelPanelViewport1;
            this.modelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel.Location = new System.Drawing.Point(0, 0);
            this.modelPanel.Name = "modelPanel";
            this.modelPanel.Size = new System.Drawing.Size(254, 315);
            this.modelPanel.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(214, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 315);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // MDL0ObjectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel7);
            this.Name = "MDL0ObjectControl";
            this.Size = new System.Drawing.Size(471, 315);
            this.splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ModelPanel modelPanel;
        private SplitContainer splitContainer2;
        private CheckedListBox lstDrawCalls;
        private Panel panel5;
        private Button btnDelete;
        private Button btnNew;
        private Panel panel1;
        private Panel panel4;
        private NumericInputBox numDrawOrder;
        private Label label3;
        private Panel panel3;
        private ComboBox cboVisBone;
        private Label label2;
        private Panel panel2;
        private ComboBox cboMaterial;
        private Label label1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem shadersToolStripMenuItem;
        private ToolStripMenuItem bonesToolStripMenuItem;
        private ToolStripMenuItem floorToolStripMenuItem;
        private Panel panel6;
        private CheckBox chkDoesntMatter;
        private Panel panel7;
        private Panel panel8;
        private Splitter splitter1;
        private ComboBox cboDrawPass;
        private Label label4;
    }
}
