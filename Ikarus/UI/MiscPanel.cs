using System;
using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using Ikarus.MovesetFile;

namespace Ikarus.UI
{
    public class MiscPanel : UserControl
    {
        public delegate void ReferenceEventHandler(ResourceNode node);

        #region Designer

        private GroupBox grpHurtBox;
        private Label OffsetX;
        internal NumericInputBox numOffX;
        private Label OffsetY;
        private NumericInputBox numRadius;
        private Label OffsetZ;
        internal NumericInputBox numStrZ;
        private Label StretchX;
        internal NumericInputBox numStrY;
        private Label StretchY;
        internal NumericInputBox numStrX;
        private Label StretchZ;
        internal NumericInputBox numOffZ;
        private Label BoxRadius;
        internal NumericInputBox numOffY;
        private Label BoxZone;
        private OpenFileDialog dlgOpen;
        private ToolStripMenuItem add;
        private ToolStripMenuItem subtract;
        private ToolStripMenuItem sourceToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private ToolStripMenuItem portToolStripMenuItem;
        private SaveFileDialog dlgSave;
        private IContainer components;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem7;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem Source;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem removeAllToolStripMenuItem;
        private ToolStripMenuItem addCustomAmountToolStripMenuItem;
        private ComboBox SelectedBone;
        private ComboBox SelectedZone;
        private CheckBox BoxEnabled;
        private NumericInputBox numRegion;
        private Label BoxRegion;
        private Panel ControlPanel;
        public EventModifier eventModifier1;
        private Label BoxBone;

        private void InitializeComponent()
        {
            this.grpHurtBox = new System.Windows.Forms.GroupBox();
            this.numRegion = new System.Windows.Forms.NumericInputBox();
            this.BoxRegion = new System.Windows.Forms.Label();
            this.BoxEnabled = new System.Windows.Forms.CheckBox();
            this.SelectedZone = new System.Windows.Forms.ComboBox();
            this.SelectedBone = new System.Windows.Forms.ComboBox();
            this.OffsetX = new System.Windows.Forms.Label();
            this.numOffX = new System.Windows.Forms.NumericInputBox();
            this.OffsetY = new System.Windows.Forms.Label();
            this.numRadius = new System.Windows.Forms.NumericInputBox();
            this.OffsetZ = new System.Windows.Forms.Label();
            this.numStrZ = new System.Windows.Forms.NumericInputBox();
            this.StretchX = new System.Windows.Forms.Label();
            this.numStrY = new System.Windows.Forms.NumericInputBox();
            this.StretchY = new System.Windows.Forms.Label();
            this.numStrX = new System.Windows.Forms.NumericInputBox();
            this.StretchZ = new System.Windows.Forms.Label();
            this.numOffZ = new System.Windows.Forms.NumericInputBox();
            this.BoxRadius = new System.Windows.Forms.Label();
            this.numOffY = new System.Windows.Forms.NumericInputBox();
            this.BoxZone = new System.Windows.Forms.Label();
            this.BoxBone = new System.Windows.Forms.Label();
            this.sourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.portToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.addCustomAmountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.eventModifier1 = new System.Windows.Forms.EventModifier();
            this.grpHurtBox.SuspendLayout();
            this.ControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpHurtBox
            // 
            this.grpHurtBox.Controls.Add(this.numRegion);
            this.grpHurtBox.Controls.Add(this.BoxRegion);
            this.grpHurtBox.Controls.Add(this.BoxEnabled);
            this.grpHurtBox.Controls.Add(this.SelectedZone);
            this.grpHurtBox.Controls.Add(this.SelectedBone);
            this.grpHurtBox.Controls.Add(this.OffsetX);
            this.grpHurtBox.Controls.Add(this.numOffX);
            this.grpHurtBox.Controls.Add(this.OffsetY);
            this.grpHurtBox.Controls.Add(this.numRadius);
            this.grpHurtBox.Controls.Add(this.OffsetZ);
            this.grpHurtBox.Controls.Add(this.numStrZ);
            this.grpHurtBox.Controls.Add(this.StretchX);
            this.grpHurtBox.Controls.Add(this.numStrY);
            this.grpHurtBox.Controls.Add(this.StretchY);
            this.grpHurtBox.Controls.Add(this.numStrX);
            this.grpHurtBox.Controls.Add(this.StretchZ);
            this.grpHurtBox.Controls.Add(this.numOffZ);
            this.grpHurtBox.Controls.Add(this.BoxRadius);
            this.grpHurtBox.Controls.Add(this.numOffY);
            this.grpHurtBox.Controls.Add(this.BoxZone);
            this.grpHurtBox.Controls.Add(this.BoxBone);
            this.grpHurtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpHurtBox.Location = new System.Drawing.Point(0, 0);
            this.grpHurtBox.Name = "grpHurtBox";
            this.grpHurtBox.Size = new System.Drawing.Size(229, 264);
            this.grpHurtBox.TabIndex = 22;
            this.grpHurtBox.TabStop = false;
            this.grpHurtBox.Text = "Edit Hurtbox";
            this.grpHurtBox.Visible = false;
            // 
            // numRegion
            // 
            this.numRegion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numRegion.Integral = false;
            this.numRegion.Location = new System.Drawing.Point(69, 171);
            this.numRegion.MaximumValue = 3.402823E+38F;
            this.numRegion.MinimumValue = -3.402823E+38F;
            this.numRegion.Name = "numRegion";
            this.numRegion.Size = new System.Drawing.Size(155, 20);
            this.numRegion.TabIndex = 25;
            this.numRegion.Text = "0";
            this.numRegion.TextChanged += new System.EventHandler(this.numRegion_TextChanged);
            // 
            // BoxRegion
            // 
            this.BoxRegion.Location = new System.Drawing.Point(-10, 170);
            this.BoxRegion.Name = "BoxRegion";
            this.BoxRegion.Size = new System.Drawing.Size(74, 20);
            this.BoxRegion.TabIndex = 24;
            this.BoxRegion.Text = "Region:";
            this.BoxRegion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BoxEnabled
            // 
            this.BoxEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BoxEnabled.AutoSize = true;
            this.BoxEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BoxEnabled.Location = new System.Drawing.Point(158, 13);
            this.BoxEnabled.Name = "BoxEnabled";
            this.BoxEnabled.Size = new System.Drawing.Size(65, 17);
            this.BoxEnabled.TabIndex = 23;
            this.BoxEnabled.Text = "Enabled";
            this.BoxEnabled.UseVisualStyleBackColor = true;
            this.BoxEnabled.CheckedChanged += new System.EventHandler(this.BoxEnabled_CheckedChanged);
            // 
            // SelectedZone
            // 
            this.SelectedZone.FormattingEnabled = true;
            this.SelectedZone.Location = new System.Drawing.Point(70, 232);
            this.SelectedZone.Name = "SelectedZone";
            this.SelectedZone.Size = new System.Drawing.Size(126, 21);
            this.SelectedZone.TabIndex = 22;
            this.SelectedZone.Tag = "";
            this.SelectedZone.SelectedIndexChanged += new System.EventHandler(this.SelectedZone_SelectedIndexChanged);
            // 
            // SelectedBone
            // 
            this.SelectedBone.FormattingEnabled = true;
            this.SelectedBone.Location = new System.Drawing.Point(70, 211);
            this.SelectedBone.Name = "SelectedBone";
            this.SelectedBone.Size = new System.Drawing.Size(126, 21);
            this.SelectedBone.TabIndex = 21;
            this.SelectedBone.Tag = "";
            this.SelectedBone.SelectedIndexChanged += new System.EventHandler(this.SelectedBone_SelectedIndexChanged);
            // 
            // OffsetX
            // 
            this.OffsetX.Location = new System.Drawing.Point(-10, 34);
            this.OffsetX.Name = "OffsetX";
            this.OffsetX.Size = new System.Drawing.Size(74, 20);
            this.OffsetX.TabIndex = 4;
            this.OffsetX.Text = "Offset X:";
            this.OffsetX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numOffX
            // 
            this.numOffX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numOffX.Integral = false;
            this.numOffX.Location = new System.Drawing.Point(69, 35);
            this.numOffX.MaximumValue = 3.402823E+38F;
            this.numOffX.MinimumValue = -3.402823E+38F;
            this.numOffX.Name = "numOffX";
            this.numOffX.Size = new System.Drawing.Size(155, 20);
            this.numOffX.TabIndex = 3;
            this.numOffX.Text = "0";
            this.numOffX.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numOffX.TextChanged += new System.EventHandler(this.numOffX_TextChanged);
            // 
            // OffsetY
            // 
            this.OffsetY.Location = new System.Drawing.Point(-10, 54);
            this.OffsetY.Name = "OffsetY";
            this.OffsetY.Size = new System.Drawing.Size(74, 20);
            this.OffsetY.TabIndex = 5;
            this.OffsetY.Text = "Offset Y:";
            this.OffsetY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numRadius
            // 
            this.numRadius.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numRadius.Integral = false;
            this.numRadius.Location = new System.Drawing.Point(70, 191);
            this.numRadius.MaximumValue = 3.402823E+38F;
            this.numRadius.MinimumValue = -3.402823E+38F;
            this.numRadius.Name = "numRadius";
            this.numRadius.Size = new System.Drawing.Size(154, 20);
            this.numRadius.TabIndex = 18;
            this.numRadius.Text = "0";
            this.numRadius.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numRadius.TextChanged += new System.EventHandler(this.numRadius_TextChanged);
            // 
            // OffsetZ
            // 
            this.OffsetZ.Location = new System.Drawing.Point(-11, 74);
            this.OffsetZ.Name = "OffsetZ";
            this.OffsetZ.Size = new System.Drawing.Size(74, 20);
            this.OffsetZ.TabIndex = 6;
            this.OffsetZ.Text = "Offset Z:";
            this.OffsetZ.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numStrZ
            // 
            this.numStrZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numStrZ.Integral = false;
            this.numStrZ.Location = new System.Drawing.Point(69, 143);
            this.numStrZ.MaximumValue = 3.402823E+38F;
            this.numStrZ.MinimumValue = -3.402823E+38F;
            this.numStrZ.Name = "numStrZ";
            this.numStrZ.Size = new System.Drawing.Size(155, 20);
            this.numStrZ.TabIndex = 17;
            this.numStrZ.Text = "0";
            this.numStrZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numStrZ.TextChanged += new System.EventHandler(this.numStrZ_TextChanged);
            // 
            // StretchX
            // 
            this.StretchX.Location = new System.Drawing.Point(-10, 102);
            this.StretchX.Name = "StretchX";
            this.StretchX.Size = new System.Drawing.Size(74, 20);
            this.StretchX.TabIndex = 7;
            this.StretchX.Text = "Stretch X:";
            this.StretchX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numStrY
            // 
            this.numStrY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numStrY.Integral = false;
            this.numStrY.Location = new System.Drawing.Point(69, 123);
            this.numStrY.MaximumValue = 3.402823E+38F;
            this.numStrY.MinimumValue = -3.402823E+38F;
            this.numStrY.Name = "numStrY";
            this.numStrY.Size = new System.Drawing.Size(155, 20);
            this.numStrY.TabIndex = 16;
            this.numStrY.Text = "0";
            this.numStrY.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numStrY.TextChanged += new System.EventHandler(this.numStrY_TextChanged);
            // 
            // StretchY
            // 
            this.StretchY.Location = new System.Drawing.Point(-10, 122);
            this.StretchY.Name = "StretchY";
            this.StretchY.Size = new System.Drawing.Size(74, 20);
            this.StretchY.TabIndex = 8;
            this.StretchY.Text = "Stretch Y:";
            this.StretchY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numStrX
            // 
            this.numStrX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numStrX.Integral = false;
            this.numStrX.Location = new System.Drawing.Point(69, 103);
            this.numStrX.MaximumValue = 3.402823E+38F;
            this.numStrX.MinimumValue = -3.402823E+38F;
            this.numStrX.Name = "numStrX";
            this.numStrX.Size = new System.Drawing.Size(155, 20);
            this.numStrX.TabIndex = 15;
            this.numStrX.Text = "0";
            this.numStrX.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numStrX.TextChanged += new System.EventHandler(this.numStrX_TextChanged);
            // 
            // StretchZ
            // 
            this.StretchZ.Location = new System.Drawing.Point(-10, 142);
            this.StretchZ.Name = "StretchZ";
            this.StretchZ.Size = new System.Drawing.Size(74, 20);
            this.StretchZ.TabIndex = 9;
            this.StretchZ.Text = "Stretch Z:";
            this.StretchZ.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numOffZ
            // 
            this.numOffZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numOffZ.Integral = false;
            this.numOffZ.Location = new System.Drawing.Point(69, 75);
            this.numOffZ.MaximumValue = 3.402823E+38F;
            this.numOffZ.MinimumValue = -3.402823E+38F;
            this.numOffZ.Name = "numOffZ";
            this.numOffZ.Size = new System.Drawing.Size(155, 20);
            this.numOffZ.TabIndex = 14;
            this.numOffZ.Text = "0";
            this.numOffZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numOffZ.TextChanged += new System.EventHandler(this.numOffZ_TextChanged);
            // 
            // BoxRadius
            // 
            this.BoxRadius.Location = new System.Drawing.Point(-10, 190);
            this.BoxRadius.Name = "BoxRadius";
            this.BoxRadius.Size = new System.Drawing.Size(74, 20);
            this.BoxRadius.TabIndex = 10;
            this.BoxRadius.Text = "Radius:";
            this.BoxRadius.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numOffY
            // 
            this.numOffY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numOffY.Integral = false;
            this.numOffY.Location = new System.Drawing.Point(69, 55);
            this.numOffY.MaximumValue = 3.402823E+38F;
            this.numOffY.MinimumValue = -3.402823E+38F;
            this.numOffY.Name = "numOffY";
            this.numOffY.Size = new System.Drawing.Size(155, 20);
            this.numOffY.TabIndex = 13;
            this.numOffY.Text = "0";
            this.numOffY.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numOffY.TextChanged += new System.EventHandler(this.numOffY_TextChanged);
            // 
            // BoxZone
            // 
            this.BoxZone.Location = new System.Drawing.Point(-10, 231);
            this.BoxZone.Name = "BoxZone";
            this.BoxZone.Size = new System.Drawing.Size(74, 20);
            this.BoxZone.TabIndex = 11;
            this.BoxZone.Text = "Zone:";
            this.BoxZone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BoxBone
            // 
            this.BoxBone.Location = new System.Drawing.Point(-10, 210);
            this.BoxBone.Name = "BoxBone";
            this.BoxBone.Size = new System.Drawing.Size(74, 20);
            this.BoxBone.TabIndex = 12;
            this.BoxBone.Text = "Bone:";
            this.BoxBone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sourceToolStripMenuItem
            // 
            this.sourceToolStripMenuItem.Name = "sourceToolStripMenuItem";
            this.sourceToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(6, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // portToolStripMenuItem
            // 
            this.portToolStripMenuItem.Name = "portToolStripMenuItem";
            this.portToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // Source
            // 
            this.Source.Name = "Source";
            this.Source.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 6);
            // 
            // add
            // 
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(32, 19);
            // 
            // subtract
            // 
            this.subtract.Name = "subtract";
            this.subtract.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(32, 19);
            // 
            // removeAllToolStripMenuItem
            // 
            this.removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
            this.removeAllToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // addCustomAmountToolStripMenuItem
            // 
            this.addCustomAmountToolStripMenuItem.Name = "addCustomAmountToolStripMenuItem";
            this.addCustomAmountToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // ControlPanel
            // 
            this.ControlPanel.Controls.Add(this.grpHurtBox);
            this.ControlPanel.Controls.Add(this.eventModifier1);
            this.ControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlPanel.Location = new System.Drawing.Point(0, 0);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(229, 264);
            this.ControlPanel.TabIndex = 26;
            this.ControlPanel.Visible = false;
            // 
            // eventModifier1
            // 
            this.eventModifier1.AutoSize = true;
            this.eventModifier1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventModifier1.Location = new System.Drawing.Point(0, 0);
            this.eventModifier1.Name = "eventModifier1";
            this.eventModifier1.Size = new System.Drawing.Size(229, 264);
            this.eventModifier1.TabIndex = 37;
            this.eventModifier1.Visible = false;
            // 
            // MiscPanel
            // 
            this.Controls.Add(this.ControlPanel);
            this.Name = "MiscPanel";
            this.Size = new System.Drawing.Size(229, 264);
            this.grpHurtBox.ResumeLayout(false);
            this.grpHurtBox.PerformLayout();
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanel.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        public MainControl _mainWindow;

        public event EventHandler FileChanged;
        public event ReferenceEventHandler ReferenceLoaded;
        public event ReferenceEventHandler ReferenceClosed;

        private object _selectedObject = null;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedObject
        {
            get { return _selectedObject; }
            set { _selectedObject = value; UpdateCurrentControl(); }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0Node TargetModel
        {
            get { return _mainWindow.TargetModel as MDL0Node; }
            set { _mainWindow.TargetModel = value; UpdateModel(); }
        }

        internal NumericInputBox[] _hurtboxBoxes = new NumericInputBox[8];

        public MiscPanel() 
        {
            InitializeComponent();
            _hurtboxBoxes[0] = numOffX; numOffX.Tag = 0;
            _hurtboxBoxes[1] = numOffY; numOffY.Tag = 1;
            _hurtboxBoxes[2] = numOffZ; numOffZ.Tag = 2;
            _hurtboxBoxes[3] = numStrX; numStrX.Tag = 3;
            _hurtboxBoxes[4] = numStrY; numStrY.Tag = 4;
            _hurtboxBoxes[5] = numStrZ; numStrZ.Tag = 5;
            _hurtboxBoxes[6] = numRadius; numRadius.Tag = 6;
            _hurtboxBoxes[7] = numRegion; numRegion.Tag = 7;
        }

        private void UpdateModel()
        {
            if (TargetModel == null)
                return;

            UpdateCurrentControl();

            _mainWindow.ModelPanel.Invalidate();
        }
        
        Control currentControl = null;
        private void UpdateCurrentControl()
        {
            Control newControl = null;

            if (_selectedObject is MiscHurtBox)
            {
                MiscHurtBox node = (MiscHurtBox)_selectedObject;

                newControl = grpHurtBox;

                numOffX.Text = node.PosOffset._x.ToString();
                numOffY.Text = node.PosOffset._y.ToString();
                numOffZ.Text = node.PosOffset._z.ToString();

                numStrX.Text = node._stretch._x.ToString();
                numStrY.Text = node._stretch._y.ToString();
                numStrZ.Text = node._stretch._z.ToString();

                numRadius.Text = node._radius.ToString();
                numRegion.Text = node.Region.ToString();
                SelectedBone.SelectedIndex = node.BoneNode.BoneIndex;
                SelectedZone.SelectedIndex = (int)node.Zone;

                BoxEnabled.Checked = node.Enabled;
            }

            if (currentControl != newControl)
            {
                if (currentControl != null)
                    currentControl.Visible = false;
                currentControl = newControl;
                if (currentControl != null)
                    currentControl.Visible = true;
            }

            if (ControlPanel.Visible != (currentControl != null))
                ControlPanel.Visible = (currentControl != null);
        }

        internal unsafe void BoxChanged(object sender, EventArgs e)
        {
            
        }

        public unsafe void ResetBox(int index)
        {
            
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MiscHurtBox SelectedHurtbox
        {
            get { return _mainWindow._selectedHurtbox; }
            set { _mainWindow._selectedHurtbox = value; }
        }

        public bool _updating = false;
        
        private void SelectedBone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedHurtbox == null)
                return;

            SelectedHurtbox.BoneNode = (MDL0BoneNode)TargetModel._linker.BoneCache[SelectedBone.SelectedIndex];
            
            if (!_updating)
                _mainWindow.ModelPanel.Invalidate();
        }

        private void SelectedZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedHurtbox == null)
                return;

            SelectedHurtbox.Zone = (HurtBoxZone)SelectedZone.SelectedIndex;

            if (!_updating)
                _mainWindow.ModelPanel.Invalidate();
        }

        private void numOffX_TextChanged(object sender, EventArgs e)
        {
            if (SelectedHurtbox == null)
                return;

            float.TryParse(numOffX.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out SelectedHurtbox._posOffset._x);
            SelectedHurtbox.SignalPropertyChange();

            if (!_updating)
                _mainWindow.ModelPanel.Invalidate();
        }

        private void numOffY_TextChanged(object sender, EventArgs e)
        {
            if (SelectedHurtbox == null)
                return;

            float.TryParse(numOffY.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out SelectedHurtbox._posOffset._y);
            SelectedHurtbox.SignalPropertyChange();

            if (!_updating)
                _mainWindow.ModelPanel.Invalidate();
        }

        private void numOffZ_TextChanged(object sender, EventArgs e)
        {
            if (SelectedHurtbox == null)
                return;

            float.TryParse(numOffZ.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out SelectedHurtbox._posOffset._z);
            SelectedHurtbox.SignalPropertyChange();

            if (!_updating)
                _mainWindow.ModelPanel.Invalidate();
        }

        private void numStrX_TextChanged(object sender, EventArgs e)
        {
            if (SelectedHurtbox == null)
                return;

            float.TryParse(numStrX.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out SelectedHurtbox._stretch._x);
            SelectedHurtbox.SignalPropertyChange();

            if (!_updating)
                _mainWindow.ModelPanel.Invalidate();
        }

        private void numStrY_TextChanged(object sender, EventArgs e)
        {
            if (SelectedHurtbox == null)
                return;

            float.TryParse(numStrY.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out SelectedHurtbox._stretch._y);
            SelectedHurtbox.SignalPropertyChange();

            if (!_updating)
                _mainWindow.ModelPanel.Invalidate();
        }

        private void numStrZ_TextChanged(object sender, EventArgs e)
        {
            if (SelectedHurtbox == null)
                return;

            float.TryParse(numStrZ.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out SelectedHurtbox._stretch._z);
            SelectedHurtbox.SignalPropertyChange();

            if (!_updating)
                _mainWindow.ModelPanel.Invalidate();
        }

        private void numRegion_TextChanged(object sender, EventArgs e)
        {
            if (SelectedHurtbox == null)
                return;

            int r;
            int.TryParse(numRegion.Text, out r);
            SelectedHurtbox.Region = r;
        }

        private void numRadius_TextChanged(object sender, EventArgs e)
        {
            if (SelectedHurtbox == null)
                return;

            float.TryParse(numRadius.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out SelectedHurtbox._radius);
            SelectedHurtbox.SignalPropertyChange();

            if (!_updating)
                _mainWindow.ModelPanel.Invalidate();
        }

        private void BoxEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectedHurtbox == null)
                return;

            SelectedHurtbox.Enabled = BoxEnabled.Checked;
        }
    }
}
