using System;
using System.Drawing;
using System.Windows.Forms;
using BrawlLib.Imaging;
using BrawlLib.Internal.Windows.Controls;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.OpenGL;

namespace BrawlCrate.UI.Collision_Editor
{
	public class CollisionEditorSettings : Form
	{
		#region Designer
		// GEN = General Section
		// COPYPASTE = Copy/Paste Options Section
		// COPY = Copy/Paste - Copy Section
		// PASTE = Copy/Paste - Paste Section
		// GB = GroupBox, CB = CheckBox

		private TabControl tabC;
		private TabPage GEN;
		private TabPage COPYPASTE;

		private Panel main_bottomPanel;
		private Button OkayButton;
		private Button ResetButton;
		
		private Label GEN_maxUndoRedo_Label;
		private NumericInputBox GEN_maxUndoRedo_Value;
		
		private GroupBox GEN_GB_Scaling;
		private CheckBox GEN_GB_Scaling_Display;
		private CheckBox GEN_GB_Scaling_Selection;
		private CheckBox GEN_Check_OnlySelectIfObjectEquals;
		private CheckBox GEN_Check_ReplaceSingleButtonCam;
		private CheckBox GEN_Check_AlwaysShowUndoRedoMenuOnStart;
		private GroupBox GEN_GB_Viewport;
		private ComboBox GEN_GB_Viewport_DefaultProjection;
		private Label GEN_GB_Viewport_DefaultProjectionLabel;
		private GroupBox GEN_GB_Viewport_Colors;
		private Panel GEN_GB_Viewport_Colors_Background_Color;
		private Label GEN_GB_Viewport_Colors_Background_ColorText;
		private Label GEN_GB_Viewport_Colors_Background_Label;
		private Panel GEN_GB_Viewport_Colors__Color;
		private Label GEN_GB_Viewport_Colors__ColorText;
		private Label GEN_GB_Viewport_Colors__Label;
		
		private GroupBox COPY_GB_Copy;
		private CheckBox COPY_GB_Copy_CB_SelObjectEquals;
		private GroupBox PASTE_GB_Paste;
		private CheckBox PASTE_GB_Paste_CB_RemSelCollsWhenPasting;
		private CheckBox PASTE_GB_Paste_CB_UseWorldLinkValues;
		

		private void InitializeComponent()
		{
			this.main_bottomPanel = new Panel();
			
			this.tabC = new TabControl();
			
			this.GEN = new TabPage();
			this.COPYPASTE = new TabPage();
			
			this.OkayButton = new Button();
			this.ResetButton = new Button();
			
			this.GEN_GB_Scaling = new GroupBox();
			this.GEN_GB_Scaling_Display = new CheckBox();
			this.GEN_GB_Scaling_Selection = new CheckBox();
			this.GEN_Check_OnlySelectIfObjectEquals = new CheckBox();
			this.GEN_Check_ReplaceSingleButtonCam = new CheckBox();
			this.GEN_Check_AlwaysShowUndoRedoMenuOnStart = new CheckBox();
			this.GEN_maxUndoRedo_Label = new Label();
			this.GEN_maxUndoRedo_Value = new NumericInputBox();
			this.GEN_GB_Viewport = new GroupBox();
			this.GEN_GB_Viewport_DefaultProjection = new ComboBox();
			this.GEN_GB_Viewport_DefaultProjectionLabel = new Label();
			this.GEN_GB_Viewport_Colors = new GroupBox();
			this.GEN_GB_Viewport_Colors_Background_Color = new Panel();
			this.GEN_GB_Viewport_Colors_Background_ColorText = new Label();
			this.GEN_GB_Viewport_Colors_Background_Label = new Label();
			this.GEN_GB_Viewport_Colors__Color = new Panel();
			this.GEN_GB_Viewport_Colors__ColorText = new Label();
			this.GEN_GB_Viewport_Colors__Label = new Label();

			this.COPY_GB_Copy = new GroupBox();
			this.COPY_GB_Copy_CB_SelObjectEquals = new CheckBox();

			this.PASTE_GB_Paste = new GroupBox();
			this.PASTE_GB_Paste_CB_RemSelCollsWhenPasting = new CheckBox();
			this.PASTE_GB_Paste_CB_UseWorldLinkValues = new CheckBox();
			
			
			this.main_bottomPanel.SuspendLayout();
			this.tabC.SuspendLayout();
			
			this.GEN.SuspendLayout();
			this.GEN_GB_Scaling.SuspendLayout();
			this.GEN_GB_Viewport.SuspendLayout();
			this.GEN_GB_Viewport_Colors.SuspendLayout();
			
			this.COPYPASTE.SuspendLayout();
			this.COPY_GB_Copy.SuspendLayout();
			this.PASTE_GB_Paste.SuspendLayout();
			
			this.SuspendLayout();



			// 
			// main_bottomPanel
			// 
			this.main_bottomPanel.Controls.Add(this.ResetButton);
			this.main_bottomPanel.Controls.Add(this.OkayButton);
			this.main_bottomPanel.Dock = DockStyle.Bottom;
			this.main_bottomPanel.Location = new Point(0, 419);
			this.main_bottomPanel.Name = "main_bottomPanel";
			this.main_bottomPanel.Size = new Size(352, 35);
			this.main_bottomPanel.TabIndex = 0;
			// 
			// tabC (Tab Control)
			// 
			this.tabC.Controls.Add(this.GEN);
			this.tabC.Controls.Add(this.COPYPASTE);
			this.tabC.Dock = DockStyle.Fill;
			this.tabC.Location = new Point(0, 0);
			this.tabC.Name = "tabC";
			this.tabC.SelectedIndex = 0;
			this.tabC.Size = new Size(352, 419);
			this.tabC.TabIndex = 1;
			// 
			// GEN (General Tab Page)
			// 
			this.GEN.Controls.Add(this.GEN_GB_Scaling);
			this.GEN.Controls.Add(this.GEN_GB_Viewport);
			this.GEN.Controls.Add(this.GEN_Check_OnlySelectIfObjectEquals);
			this.GEN.Controls.Add(this.GEN_Check_ReplaceSingleButtonCam);
			this.GEN.Controls.Add(this.GEN_Check_AlwaysShowUndoRedoMenuOnStart);
			this.GEN.Controls.Add(this.GEN_maxUndoRedo_Value);
			this.GEN.Controls.Add(this.GEN_maxUndoRedo_Label);
			this.GEN.Location = new Point(4, 22);
			this.GEN.Name = "GEN";
			this.GEN.Padding = new Padding(3);
			this.GEN.Size = new Size(344, 393);
			this.GEN.TabIndex = 0;
			this.GEN.Text = "General";
			this.GEN.UseVisualStyleBackColor = true;
			// 
			// COPYPASTE (Copy/Paste Options Tab Page)
			// 
			this.COPYPASTE.Controls.Add(this.PASTE_GB_Paste);
			this.COPYPASTE.Controls.Add(this.COPY_GB_Copy);
			this.COPYPASTE.Location = new Point(4, 22);
			this.COPYPASTE.Name = "COPYPASTE";
			this.COPYPASTE.Padding = new Padding(3);
			this.COPYPASTE.Size = new Size(344, 393);
			this.COPYPASTE.TabIndex = 1;
			this.COPYPASTE.Text = "Copy/Paste Options";
			this.COPYPASTE.UseVisualStyleBackColor = true;

			// 
			// OkayButton 
			// 
			this.OkayButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.OkayButton.Location = new Point(271, 6);
			this.OkayButton.Name = "OkayButton";
			this.OkayButton.Size = new Size(75, 23);
			this.OkayButton.TabIndex = 0;
			this.OkayButton.Text = "&Okay";
			this.OkayButton.UseVisualStyleBackColor = true;
			this.OkayButton.Click += OkayButton_Click;
			// 
			// ResetButton
			// 
			this.ResetButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.ResetButton.Location = new Point(6, 6);
			this.ResetButton.Name = "ResetButton";
			this.ResetButton.Size = new Size(75, 23);
			this.ResetButton.TabIndex = 0;
			this.ResetButton.Text = "&Reset";
			this.ResetButton.UseVisualStyleBackColor = true;
			this.ResetButton.Click += ResetButton_Click;

			// 
			// GEN_GB_Scaling (GEN - General, GB - GroupBox)
			// 
			this.GEN_GB_Scaling.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.GEN_GB_Scaling.Controls.Add(this.GEN_GB_Scaling_Selection);
			this.GEN_GB_Scaling.Controls.Add(this.GEN_GB_Scaling_Display);
			this.GEN_GB_Scaling.Location = new Point(6, 6);
			this.GEN_GB_Scaling.Name = "GEN_GB_Scaling";
			this.GEN_GB_Scaling.Size = new Size(332, 43);
			this.GEN_GB_Scaling.TabIndex = 3;
			this.GEN_GB_Scaling.TabStop = false;
			this.GEN_GB_Scaling.Text = "Scale Points With Camera";
			// 
			// GEN_GB_Scaling_Display
			// 
			this.GEN_GB_Scaling_Display.AutoSize = true;
			this.GEN_GB_Scaling_Display.Location = new Point(6, 19);
			this.GEN_GB_Scaling_Display.Name = "GEN_GB_Scaling_Display";
			this.GEN_GB_Scaling_Display.Size = new Size(60, 17);
			this.GEN_GB_Scaling_Display.TabIndex = 2;
			this.GEN_GB_Scaling_Display.Text = "Display";
			this.GEN_GB_Scaling_Display.UseVisualStyleBackColor = true;
			this.GEN_GB_Scaling_Display.CheckedChanged += GEN_GB_Scaling_Display_CheckedChanged;
			// 
			// GEN_GB_Scaling_Selection
			// 
			this.GEN_GB_Scaling_Selection.AutoSize = true;
			this.GEN_GB_Scaling_Selection.Location = new Point(149, 19);
			this.GEN_GB_Scaling_Selection.Name = "GEN_GB_Scaling_Selection";
			this.GEN_GB_Scaling_Selection.Size = new Size(70, 17);
			this.GEN_GB_Scaling_Selection.TabIndex = 2;
			this.GEN_GB_Scaling_Selection.Text = "Selection";
			this.GEN_GB_Scaling_Selection.UseVisualStyleBackColor = true;
			this.GEN_GB_Scaling_Selection.CheckedChanged += GEN_GB_Scaling_Selection_CheckedChanged;


			// 
			// GEN_GB_Viewport
			// 
			this.GEN_GB_Viewport.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.GEN_GB_Viewport.Controls.Add(this.GEN_GB_Viewport_DefaultProjection);
			this.GEN_GB_Viewport.Controls.Add(this.GEN_GB_Viewport_DefaultProjectionLabel);
			this.GEN_GB_Viewport.Controls.Add(this.GEN_GB_Viewport_Colors);
			this.GEN_GB_Viewport.Location = new Point(6, 52);
			this.GEN_GB_Viewport.Size = new Size(332, 120);
			this.GEN_GB_Viewport.Name = "GEN_GB_Viewport";
			this.GEN_GB_Viewport.TabIndex = 3;
			this.GEN_GB_Viewport.TabStop = false;
			this.GEN_GB_Viewport.Text = "All Viewports";
			//
			// GEN_GB_Viewport_DefaultProjection
			//
			this.GEN_GB_Viewport_DefaultProjection.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.GEN_GB_Viewport_DefaultProjection.Location = new Point(79, 19);
			this.GEN_GB_Viewport_DefaultProjection.Name = "GEN_GB_Viewport_DefaultProjection";
			this.GEN_GB_Viewport_DefaultProjection.Size = new Size(241, 19);
			this.GEN_GB_Viewport_DefaultProjection.TabIndex = 3;
			this.GEN_GB_Viewport_DefaultProjection.TabStop = true;
			this.GEN_GB_Viewport_DefaultProjection.DropDownStyle = ComboBoxStyle.DropDownList;

			//
			// GEN_GB_Viewport_DefaultProjectionLabel
			//
			this.GEN_GB_Viewport_DefaultProjectionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
			this.GEN_GB_Viewport_DefaultProjectionLabel.Location = new Point(10, 19);
			this.GEN_GB_Viewport_DefaultProjectionLabel.BorderStyle = BorderStyle.FixedSingle;
			this.GEN_GB_Viewport_DefaultProjectionLabel.Name = "GEN_GB_Viewport_DefaultProjectionLabel";
			this.GEN_GB_Viewport_DefaultProjectionLabel.Size = new Size(70, 21);
			this.GEN_GB_Viewport_DefaultProjectionLabel.TextAlign = ContentAlignment.MiddleRight;
			this.GEN_GB_Viewport_DefaultProjectionLabel.TabIndex = 3;
			this.GEN_GB_Viewport_DefaultProjectionLabel.Text = "Projection:";
			this.GEN_GB_Viewport_DefaultProjectionLabel.TabStop = false;
			// 
			// GEN_GB_Viewport_Colors
			// 
			this.GEN_GB_Viewport_Colors.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
			this.GEN_GB_Viewport_Colors.Controls.Add(this.GEN_GB_Viewport_Colors_Background_Color);
			this.GEN_GB_Viewport_Colors.Controls.Add(this.GEN_GB_Viewport_Colors_Background_ColorText);
			this.GEN_GB_Viewport_Colors.Controls.Add(this.GEN_GB_Viewport_Colors_Background_Label);
			this.GEN_GB_Viewport_Colors.Controls.Add(this.GEN_GB_Viewport_Colors__Color);
			this.GEN_GB_Viewport_Colors.Controls.Add(this.GEN_GB_Viewport_Colors__ColorText);
			this.GEN_GB_Viewport_Colors.Controls.Add(this.GEN_GB_Viewport_Colors__Label);
			this.GEN_GB_Viewport_Colors.Location = new Point(6, 48);
			this.GEN_GB_Viewport_Colors.Name = "GEN_GB_Viewport_Colors";
			this.GEN_GB_Viewport_Colors.Size = new Size(320, 66);
			this.GEN_GB_Viewport_Colors.TabIndex = 3;
			this.GEN_GB_Viewport_Colors.TabStop = false;
			this.GEN_GB_Viewport_Colors.Text = "Colors";
			// 
			// GEN_GB_Viewport_Colors_Background_Color
			// 
			this.GEN_GB_Viewport_Colors_Background_Color.AutoSize = false;
			this.GEN_GB_Viewport_Colors_Background_Color.BorderStyle = BorderStyle.FixedSingle;
			this.GEN_GB_Viewport_Colors_Background_Color.Location = new Point(269, 19);
			this.GEN_GB_Viewport_Colors_Background_Color.Name = "GEN_GB_Viewport_Colors_Background_Color";
			this.GEN_GB_Viewport_Colors_Background_Color.Size = new Size(40, 20);
			this.GEN_GB_Viewport_Colors_Background_Color.TabIndex = 0;
			this.GEN_GB_Viewport_Colors_Background_Color.Click += GEN_GB_Viewport_Colors_Background_UpdateColor_Click;
			// 
			// GEN_GB_Viewport_Colors_Background_ColorText
			// 
			this.GEN_GB_Viewport_Colors_Background_ColorText.AutoSize = false;
			this.GEN_GB_Viewport_Colors_Background_ColorText.TextAlign = ContentAlignment.MiddleCenter;
			this.GEN_GB_Viewport_Colors_Background_ColorText.BorderStyle = BorderStyle.FixedSingle;
			this.GEN_GB_Viewport_Colors_Background_ColorText.Location = new Point(79, 19);
			this.GEN_GB_Viewport_Colors_Background_ColorText.Name = "GEN_GB_Viewport_Colors_Background_ColorText";
			this.GEN_GB_Viewport_Colors_Background_ColorText.Size = new Size(191, 20);
			this.GEN_GB_Viewport_Colors_Background_ColorText.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.GEN_GB_Viewport_Colors_Background_ColorText.TabIndex = 0;
			this.GEN_GB_Viewport_Colors_Background_ColorText.Text = "";
			this.GEN_GB_Viewport_Colors_Background_ColorText.Click += GEN_GB_Viewport_Colors_Background_UpdateColor_Click;
			// 
			// GEN_GB_Viewport_Colors_Background_Label
			// 
			this.GEN_GB_Viewport_Colors_Background_Label.AutoSize = false;
			this.GEN_GB_Viewport_Colors_Background_Label.TextAlign = ContentAlignment.MiddleRight;
			this.GEN_GB_Viewport_Colors_Background_Label.BorderStyle = BorderStyle.FixedSingle;
			this.GEN_GB_Viewport_Colors_Background_Label.Location = new Point(10, 19);
			this.GEN_GB_Viewport_Colors_Background_Label.Name = "GEN_GB_Viewport_Colors_Background_Label";
			this.GEN_GB_Viewport_Colors_Background_Label.Size = new Size(70, 20);
			this.GEN_GB_Viewport_Colors_Background_Label.TabIndex = 0;
			this.GEN_GB_Viewport_Colors_Background_Label.Text = "Background:";
			this.GEN_GB_Viewport_Colors_Background_Label.Click += GEN_GB_Viewport_Colors_Background_UpdateColor_Click;
			// 
			// GEN_GB_Viewport_Colors__Color
			// 
			this.GEN_GB_Viewport_Colors__Color.AutoSize = false;
			this.GEN_GB_Viewport_Colors__Color.BorderStyle = BorderStyle.FixedSingle;
			this.GEN_GB_Viewport_Colors__Color.Location = new Point(269, 38);
			this.GEN_GB_Viewport_Colors__Color.Name = "GEN_GB_Viewport_Colors__Color";
			this.GEN_GB_Viewport_Colors__Color.Size = new Size(40, 20);
			this.GEN_GB_Viewport_Colors__Color.TabIndex = 0;
			//this.GEN_GB_Viewport_Colors__Color.Click += GEN_GB_Viewport_Colors__UpdateColor_Click;
			// 
			// GEN_GB_Viewport_Colors__ColorText
			// 
			this.GEN_GB_Viewport_Colors__ColorText.AutoSize = false;
			this.GEN_GB_Viewport_Colors__ColorText.TextAlign = ContentAlignment.MiddleCenter;
			this.GEN_GB_Viewport_Colors__ColorText.BorderStyle = BorderStyle.FixedSingle;
			this.GEN_GB_Viewport_Colors__ColorText.Location = new Point(79, 38);
			this.GEN_GB_Viewport_Colors__ColorText.Name = "GEN_GB_Viewport_Colors__ColorText";
			this.GEN_GB_Viewport_Colors__ColorText.Size = new Size(191, 20);
			this.GEN_GB_Viewport_Colors__ColorText.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.GEN_GB_Viewport_Colors__ColorText.TabIndex = 0;
			this.GEN_GB_Viewport_Colors__ColorText.Text = "";
			//this.GEN_GB_Viewport_Colors__ColorText.Click += GEN_GB_Viewport_Colors__UpdateColor_Click;
			// 
			// GEN_GB_Viewport_Colors__Label
			// 
			this.GEN_GB_Viewport_Colors__Label.AutoSize = false;
			this.GEN_GB_Viewport_Colors__Label.TextAlign = ContentAlignment.MiddleRight;
			this.GEN_GB_Viewport_Colors__Label.BorderStyle = BorderStyle.FixedSingle;
			this.GEN_GB_Viewport_Colors__Label.Location = new Point(10, 38);
			this.GEN_GB_Viewport_Colors__Label.Name = "GEN_GB_Viewport_Colors__Label";
			this.GEN_GB_Viewport_Colors__Label.Size = new Size(70, 20);
			this.GEN_GB_Viewport_Colors__Label.TabIndex = 0;
			this.GEN_GB_Viewport_Colors__Label.Text = "";
			//this.GEN_GB_Viewport_Colors__Label.Click += GEN_GB_Viewport_Colors__UpdateColor_Click;


			// 
			// GEN_Check_OnlySelectIfObjectEquals (Check - CheckBox)
			// 
			this.GEN_Check_OnlySelectIfObjectEquals.AutoSize = true;
			this.GEN_Check_OnlySelectIfObjectEquals.Location = new Point(6, 180);
			this.GEN_Check_OnlySelectIfObjectEquals.Name = "GEN_Check_OnlySelectIfObjectEquals";
			this.GEN_Check_OnlySelectIfObjectEquals.Size = new Size(279, 17);
			this.GEN_Check_OnlySelectIfObjectEquals.TabIndex = 2;
			this.GEN_Check_OnlySelectIfObjectEquals.Text = "Only select if collision object equals to selected object";
			this.GEN_Check_OnlySelectIfObjectEquals.UseVisualStyleBackColor = true;
			this.GEN_Check_OnlySelectIfObjectEquals.CheckedChanged += GEN_Check_OnlySelectIfObjectEquals_CheckedChanged;
			// 
			// GEN_Check_ReplaceSingleButtonCam
			// 
			this.GEN_Check_ReplaceSingleButtonCam.AutoSize = true;
			this.GEN_Check_ReplaceSingleButtonCam.Location = new Point(6, 203);
			this.GEN_Check_ReplaceSingleButtonCam.Name = "GEN_Check_ReplaceSingleButtonCam";
			this.GEN_Check_ReplaceSingleButtonCam.Size = new Size(270, 30);
			this.GEN_Check_ReplaceSingleButtonCam.TabIndex = 2;
			this.GEN_Check_ReplaceSingleButtonCam.Text = "Replace single-button camera perspective type with\r\ntwo buttons";
			this.GEN_Check_ReplaceSingleButtonCam.UseVisualStyleBackColor = true;
			this.GEN_Check_ReplaceSingleButtonCam.CheckedChanged += GEN_Check_ReplaceSingleButtonCam_CheckedChanged;
			// 
			// GEN_Check_AlwaysShowUndoRedoMenuOnStart
			// 
			this.GEN_Check_AlwaysShowUndoRedoMenuOnStart.AutoSize = true;
			this.GEN_Check_AlwaysShowUndoRedoMenuOnStart.Location = new Point(6, 236);
			this.GEN_Check_AlwaysShowUndoRedoMenuOnStart.Name = "GEN_Check_AlwaysShowUndoRedoMenuOnStart";
			this.GEN_Check_AlwaysShowUndoRedoMenuOnStart.Size = new Size(270, 30);
			this.GEN_Check_AlwaysShowUndoRedoMenuOnStart.TabIndex = 2;
			this.GEN_Check_AlwaysShowUndoRedoMenuOnStart.Text = "Always show Undo/Redo Menu at start";
			this.GEN_Check_AlwaysShowUndoRedoMenuOnStart.UseVisualStyleBackColor = true;
			this.GEN_Check_AlwaysShowUndoRedoMenuOnStart.CheckedChanged += GEN_Check_AlwaysShowUndoRedoMenuOnStart_CheckedChanged;

			
			
			// 
			// GEN_maxUndoRedo_Label
			// 
			this.GEN_maxUndoRedo_Label.AutoSize = true;
			this.GEN_maxUndoRedo_Label.Location = new Point(4, 367);
			this.GEN_maxUndoRedo_Label.Name = "GEN_maxUndoRedo_Label";
			this.GEN_maxUndoRedo_Label.Size = new Size(145, 13);
			this.GEN_maxUndoRedo_Label.TabIndex = 0;
			this.GEN_maxUndoRedo_Label.Text = "Maximum Undo/Redo Count:";
			// 
			// GEN_maxUndoRedo_Value
			// 
			this.GEN_maxUndoRedo_Value.Integer = true;
			this.GEN_maxUndoRedo_Value.Integral = true;
			this.GEN_maxUndoRedo_Value.Location = new Point(155, 364);
			this.GEN_maxUndoRedo_Value.MaximumValue = 3.402823E+38F;
			this.GEN_maxUndoRedo_Value.MinimumValue = 0;
			this.GEN_maxUndoRedo_Value.Name = "GEN_maxUndoRedo_Value";
			this.GEN_maxUndoRedo_Value.Size = new Size(66, 20);
			this.GEN_maxUndoRedo_Value.TabIndex = 1;
			this.GEN_maxUndoRedo_Value.Text = "0";
			this.GEN_maxUndoRedo_Value.ValueChanged += GEN_maxUndoRedo_Value_ValueChanged;


			// 
			// COPY_GB_Copy
			// 
			this.COPY_GB_Copy.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.COPY_GB_Copy.Controls.Add(this.COPY_GB_Copy_CB_SelObjectEquals);
			this.COPY_GB_Copy.Location = new Point(6, 6);
			this.COPY_GB_Copy.Name = "COPY_GB_Copy";
			this.COPY_GB_Copy.Size = new Size(332, 42);
			this.COPY_GB_Copy.TabIndex = 4;
			this.COPY_GB_Copy.TabStop = false;
			this.COPY_GB_Copy.Text = "Copy Options";
			// 
			// COPY_GB_Copy_CB_SelObjectEquals
			// 
			this.COPY_GB_Copy_CB_SelObjectEquals.AutoSize = true;
			this.COPY_GB_Copy_CB_SelObjectEquals.Location = new Point(6, 19);
			this.COPY_GB_Copy_CB_SelObjectEquals.Name = "COPY_GB_Copy_CB_SelObjectEquals";
			this.COPY_GB_Copy_CB_SelObjectEquals.Size = new Size(258, 17);
			this.COPY_GB_Copy_CB_SelObjectEquals.TabIndex = 2;
			this.COPY_GB_Copy_CB_SelObjectEquals.Text = "Copy if selected object equals to collision\'s object";
			this.COPY_GB_Copy_CB_SelObjectEquals.UseVisualStyleBackColor = true;
			this.COPY_GB_Copy_CB_SelObjectEquals.CheckedChanged += COPY_GB_Copy_CB_SelObjectEquals_CheckedChanged;

			// 
			// PASTE_GB_Paste
			// 
			this.PASTE_GB_Paste.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.PASTE_GB_Paste.Controls.Add(this.PASTE_GB_Paste_CB_RemSelCollsWhenPasting);
			this.PASTE_GB_Paste.Controls.Add(this.PASTE_GB_Paste_CB_UseWorldLinkValues);
			this.PASTE_GB_Paste.Location = new Point(6, 54);
			this.PASTE_GB_Paste.Name = "PASTE_GB_Paste";
			this.PASTE_GB_Paste.Size = new Size(332, 66);
			this.PASTE_GB_Paste.TabIndex = 4;
			this.PASTE_GB_Paste.TabStop = false;
			this.PASTE_GB_Paste.Text = "Paste Options";
			// 
			// PASTE_GB_Paste_CB_RemSelCollsWhenPasting
			// 
			this.PASTE_GB_Paste_CB_RemSelCollsWhenPasting.AutoSize = true;
			this.PASTE_GB_Paste_CB_RemSelCollsWhenPasting.Location = new Point(6, 42);
			this.PASTE_GB_Paste_CB_RemSelCollsWhenPasting.Name = "checkBox3";
			this.PASTE_GB_Paste_CB_RemSelCollsWhenPasting.Size = new Size(220, 17);
			this.PASTE_GB_Paste_CB_RemSelCollsWhenPasting.TabIndex = 2;
			this.PASTE_GB_Paste_CB_RemSelCollsWhenPasting.Text = "Remove selected collisions when pasting";
			this.PASTE_GB_Paste_CB_RemSelCollsWhenPasting.UseVisualStyleBackColor = true;
			this.PASTE_GB_Paste_CB_RemSelCollsWhenPasting.CheckedChanged += PASTE_GB_Paste_CB_RemSelCollsWhenPasting_CheckedChanged;
			// 
			// PASTE_GB_Paste_CB_UseWorldLinkValues
			// 
			this.PASTE_GB_Paste_CB_UseWorldLinkValues.AutoSize = true;
			this.PASTE_GB_Paste_CB_UseWorldLinkValues.Location = new Point(6, 19);
			this.PASTE_GB_Paste_CB_UseWorldLinkValues.Name = "PASTE_GB_Paste_CB_UseWorldLinkValues";
			this.PASTE_GB_Paste_CB_UseWorldLinkValues.Size = new Size(239, 17);
			this.PASTE_GB_Paste_CB_UseWorldLinkValues.TabIndex = 2;
			this.PASTE_GB_Paste_CB_UseWorldLinkValues.Text = "Use actual link values instead of raw variants";
			this.PASTE_GB_Paste_CB_UseWorldLinkValues.UseVisualStyleBackColor = true;
			this.PASTE_GB_Paste_CB_UseWorldLinkValues.CheckedChanged += PASTE_GB_Paste_CB_UseWorldLinkValues_CheckedChanged;


			// 
			// BrawlCrate_Settings_CollisionEditor
			// 
			this.ClientSize = new Size(352, 454);
			this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			this.Icon = BrawlLib.Properties.Resources.Icon;
			this.Controls.Add(this.tabC);
			this.Controls.Add(this.main_bottomPanel);
			this.Name = "CollisionEditorSettings";
			this.Text = "Collision Editor Settings";
			this.MinimumSize = this.Size;
			this.MaximumSize = this.Size;
			this.MaximizeBox = false;

			this.main_bottomPanel.ResumeLayout(false);
			
			this.tabC.ResumeLayout(false);
			
			this.GEN.ResumeLayout(false);
			this.GEN.PerformLayout();
			this.GEN_GB_Scaling.ResumeLayout(false);
			this.GEN_GB_Scaling.PerformLayout();
			
			this.COPYPASTE.ResumeLayout(false);
			this.COPY_GB_Copy.ResumeLayout(false);
			this.COPY_GB_Copy.PerformLayout();
			this.PASTE_GB_Paste.ResumeLayout(false);
			this.PASTE_GB_Paste.PerformLayout();
			
			this.ResumeLayout(false);
		}

	



		#endregion

		private readonly CollisionEditor ParentEditor;
		private readonly GoodColorDialog ColorDialog;

		private bool Updating;

		public CollisionEditorSettings(CollisionEditor editor)
		{
			ParentEditor = editor;
			ColorDialog = new GoodColorDialog();

			Updating = false;
			InitializeComponent();

			GEN_GB_Viewport_DefaultProjection.DataSource = Enum.GetNames(typeof(ViewportProjection));
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			UpdateSettings(true);
		}

		private void OkayButton_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void ResetButton_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(this, "Are you sure you want to reset all settings to its default state?", "Reset Settings",
				MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
			{
				return;
			}

			Properties.Settings.Default.ViewerSettings = null;
			Properties.Settings.Default.ViewerSettingsSet = false;

			ParentEditor.DistributeSettings(CollisionEditorSettings_Data.DefaultValues());
		}
		private void GEN_maxUndoRedo_Value_ValueChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			ParentEditor.maxSaveCount = (int)GEN_maxUndoRedo_Value.Value;
		}
		private void GEN_GB_Viewport_Colors_R_ValueChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			UpdateViewportBGColor();
		}
		private void GEN_GB_Viewport_Colors_G_ValueChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			UpdateViewportBGColor();
		}
		private void GEN_GB_Viewport_Colors_B_ValueChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			UpdateViewportBGColor();
		}
		private void GEN_GB_Viewport_Colors_A_ValueChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			UpdateViewportBGColor();
		}
		private void GEN_GB_Viewport_Colors_Background_UpdateColor_Click(object sender, EventArgs e)
		{
			ColorDialog.Color = (Color)ParentEditor.RetrieveSettings().BackgroundColor;

			if (ColorDialog.ShowDialog(this) == DialogResult.OK)
			{
				ParentEditor.CreateCollisionEditorSettingsIfNotExists();

				Properties.Settings.Default.CollisionEditorSettings.BackgroundColor = (ARGBPixel)ColorDialog.Color;
				Properties.Settings.Default.Save();

				UpdateViewportBGColor();
			}
		}
		private void UpdateViewportBGColor()
		{
			ParentEditor.CreateCollisionEditorSettingsIfNotExists();

			ARGBPixel bgColorP = Properties.Settings.Default.CollisionEditorSettings.BackgroundColor;
			Color bgColor = (Color)bgColorP;

			GEN_GB_Viewport_Colors_Background_ColorText.Text = bgColorP.ToString();
			GEN_GB_Viewport_Colors_Background_Color.BackColor = Color.FromArgb(bgColor.R, bgColor.G, bgColor.B);

			ParentEditor.UpdateViewportBackgroundColor(bgColor.A, bgColor.R, bgColor.G, bgColor.B);
		}

		private void GEN_GB_Scaling_Display_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			Updating = true;
			ParentEditor.toolsStrip_Options_ScalePointsWithCamera_DisplayOnly.Checked = GEN_GB_Scaling_Display.Checked;
			Updating = false;
		}

		private void GEN_GB_Scaling_Selection_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			Updating = true;
			ParentEditor.toolsStrip_Options_ScalePointsWithCamera_SelectOnly.Checked = GEN_GB_Scaling_Selection.Checked;
			Updating = false;
		}

		private void GEN_Check_OnlySelectIfObjectEquals_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			Updating = true;
			ParentEditor.toolsStrip_Options_SelectOnlyIfObjectEquals.Checked = GEN_Check_OnlySelectIfObjectEquals.Checked;
			Updating = false;
		}

		private void GEN_Check_ReplaceSingleButtonCam_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			ParentEditor.ToggleViewportButtonVisibility(GEN_Check_ReplaceSingleButtonCam.Checked);
		}

		private void GEN_Check_AlwaysShowUndoRedoMenuOnStart_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			ParentEditor.CreateCollisionEditorSettingsIfNotExists();

			Properties.Settings.Default.CollisionEditorSettings.AlwaysShowUndoRedoMenuOnStart = GEN_Check_AlwaysShowUndoRedoMenuOnStart.Checked;
			Properties.Settings.Default.Save();
		}

		private void COPY_GB_Copy_CB_SelObjectEquals_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			Updating = true;
			ParentEditor.clipboardCopyOptions_OnlySelectObjectIfCollisionObjectEquals.Checked = COPY_GB_Copy_CB_SelObjectEquals.Checked;
			Updating = false;
		}

		private void PASTE_GB_Paste_CB_RemSelCollsWhenPasting_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			Updating = true;
			ParentEditor.clipboardPasteOptions_PasteRemoveSelected.Checked = PASTE_GB_Paste_CB_RemSelCollsWhenPasting.Checked;
			Updating = false;
		}

		private void PASTE_GB_Paste_CB_UseWorldLinkValues_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			Updating = true;
			ParentEditor.clipboardPasteOptions_ActualPointsValuesAreUsed.Checked = PASTE_GB_Paste_CB_UseWorldLinkValues.Checked;
			Updating = false;
		}

		private void GEN_GB_Viewport_DefaultProjection_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			Updating = true;
			ParentEditor.UpdateViewportProjection((ViewportProjection)GEN_GB_Viewport_DefaultProjection.SelectedIndex);
			Updating = false;
		}

		#region CollisionEditorSettings_Data Read/Write
		public void UpdateSettings(bool LoadingSettings)
		{
			if (Updating)
				return;


			Updating = true;

			CollisionEditorSettings_Data settings = ParentEditor.RetrieveSettings();

			GEN_GB_Scaling_Display.Checked = ParentEditor.toolsStrip_Options_ScalePointsWithCamera_DisplayOnly.Checked;
			GEN_GB_Scaling_Selection.Checked = ParentEditor.toolsStrip_Options_ScalePointsWithCamera_SelectOnly.Checked;

			GEN_Check_ReplaceSingleButtonCam.Checked = !ParentEditor.toolsStrip_TogglePerspectiveOrthographicCam.Visible;
			GEN_Check_AlwaysShowUndoRedoMenuOnStart.Checked = settings.AlwaysShowUndoRedoMenuOnStart;

			GEN_maxUndoRedo_Value.Value = ParentEditor.maxSaveCount;

			COPY_GB_Copy_CB_SelObjectEquals.Checked = ParentEditor.clipboardCopyOptions_OnlySelectObjectIfCollisionObjectEquals.Checked;
			PASTE_GB_Paste_CB_UseWorldLinkValues.Checked = ParentEditor.clipboardPasteOptions_ActualPointsValuesAreUsed.Checked;
			PASTE_GB_Paste_CB_RemSelCollsWhenPasting.Checked = ParentEditor.clipboardPasteOptions_PasteRemoveSelected.Checked;

			GEN_GB_Viewport_DefaultProjection.SelectedIndex = (int)ParentEditor._modelPanel.CurrentViewport.ViewType;

			if (LoadingSettings)
			{
				// To combat the issue in the viewport being updated when we are just updating the settings, we add the event after it.
				this.GEN_GB_Viewport_DefaultProjection.SelectedIndexChanged += GEN_GB_Viewport_DefaultProjection_SelectedIndexChanged;
			}

			UpdateViewportBGColor();

			Updating = false;
		}
		#endregion

		#region Form Events
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			this.ParentEditor.NullifyEditorSettings();
		}

		#endregion
	}
}
