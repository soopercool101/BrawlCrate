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
        private CheckBox GEN_Check_OnlySelectIfObjectEquals;
        private CheckBox GEN_Check_ReplaceSingleButtonCam;
        private CheckBox GEN_Check_AlwaysShowUndoRedoMenuOnStart;
        private CheckBox GEN_Check_ResetPerspectiveOrthoCamToZeroZero;

        private GroupBox COPY_GB_Copy;
        private CheckBox COPY_GB_Copy_CB_SelObjectEquals;
        private GroupBox PASTE_GB_Paste;
        private CheckBox PASTE_GB_Paste_CB_RemSelCollsWhenPasting;
        private CheckBox PASTE_GB_Paste_CB_UseWorldLinkValues;


        private void InitializeComponent()
        {
            main_bottomPanel = new Panel();

            tabC = new TabControl();

            GEN = new TabPage();
            COPYPASTE = new TabPage();

            OkayButton = new Button();
            ResetButton = new Button();

            GEN_GB_Scaling = new GroupBox();
            GEN_GB_Scaling_Display = new CheckBox();
            GEN_GB_Scaling_Selection = new CheckBox();
            GEN_maxUndoRedo_Label = new Label();
            GEN_maxUndoRedo_Value = new NumericInputBox();
            GEN_GB_Viewport = new GroupBox();
            GEN_GB_Viewport_DefaultProjection = new ComboBox();
            GEN_GB_Viewport_DefaultProjectionLabel = new Label();
            GEN_GB_Viewport_Colors = new GroupBox();
            GEN_GB_Viewport_Colors_Background_Color = new Panel();
            GEN_GB_Viewport_Colors_Background_ColorText = new Label();
            GEN_GB_Viewport_Colors_Background_Label = new Label();
            GEN_GB_Viewport_Colors__Color = new Panel();
            GEN_GB_Viewport_Colors__ColorText = new Label();
            GEN_GB_Viewport_Colors__Label = new Label();
            GEN_Check_OnlySelectIfObjectEquals = new CheckBox();
            GEN_Check_ReplaceSingleButtonCam = new CheckBox();
            GEN_Check_AlwaysShowUndoRedoMenuOnStart = new CheckBox();
            GEN_Check_ResetPerspectiveOrthoCamToZeroZero = new CheckBox();

            COPY_GB_Copy = new GroupBox();
            COPY_GB_Copy_CB_SelObjectEquals = new CheckBox();

            PASTE_GB_Paste = new GroupBox();
            PASTE_GB_Paste_CB_RemSelCollsWhenPasting = new CheckBox();
            PASTE_GB_Paste_CB_UseWorldLinkValues = new CheckBox();


            main_bottomPanel.SuspendLayout();
            tabC.SuspendLayout();

            GEN.SuspendLayout();
            GEN_GB_Scaling.SuspendLayout();
            GEN_GB_Viewport.SuspendLayout();
            GEN_GB_Viewport_Colors.SuspendLayout();

            COPYPASTE.SuspendLayout();
            COPY_GB_Copy.SuspendLayout();
            PASTE_GB_Paste.SuspendLayout();

            SuspendLayout();


            // 
            // main_bottomPanel
            // 
            main_bottomPanel.Controls.Add(ResetButton);
            main_bottomPanel.Controls.Add(OkayButton);
            main_bottomPanel.Dock = DockStyle.Bottom;
            main_bottomPanel.Location = new Point(0, 419);
            main_bottomPanel.Name = "main_bottomPanel";
            main_bottomPanel.Size = new Size(352, 35);
            main_bottomPanel.TabIndex = 0;
            // 
            // tabC (Tab Control)
            // 
            tabC.Controls.Add(GEN);
            tabC.Controls.Add(COPYPASTE);
            tabC.Dock = DockStyle.Fill;
            tabC.Location = new Point(0, 0);
            tabC.Name = "tabC";
            tabC.SelectedIndex = 0;
            tabC.Size = new Size(352, 419);
            tabC.TabIndex = 1;
            // 
            // GEN (General Tab Page)
            // 
            GEN.Controls.Add(GEN_GB_Scaling);
            GEN.Controls.Add(GEN_GB_Viewport);
            GEN.Controls.Add(GEN_Check_OnlySelectIfObjectEquals);
            GEN.Controls.Add(GEN_Check_ReplaceSingleButtonCam);
            GEN.Controls.Add(GEN_Check_AlwaysShowUndoRedoMenuOnStart);
            GEN.Controls.Add(GEN_Check_ResetPerspectiveOrthoCamToZeroZero);
            GEN.Controls.Add(GEN_maxUndoRedo_Value);
            GEN.Controls.Add(GEN_maxUndoRedo_Label);
            GEN.Location = new Point(4, 22);
            GEN.Name = "GEN";
            GEN.Padding = new Padding(3);
            GEN.Size = new Size(344, 393);
            GEN.TabIndex = 0;
            GEN.Text = "General";
            GEN.UseVisualStyleBackColor = true;
            // 
            // COPYPASTE (Copy/Paste Options Tab Page)
            // 
            COPYPASTE.Controls.Add(PASTE_GB_Paste);
            COPYPASTE.Controls.Add(COPY_GB_Copy);
            COPYPASTE.Location = new Point(4, 22);
            COPYPASTE.Name = "COPYPASTE";
            COPYPASTE.Padding = new Padding(3);
            COPYPASTE.Size = new Size(344, 393);
            COPYPASTE.TabIndex = 1;
            COPYPASTE.Text = "Copy/Paste Options";
            COPYPASTE.UseVisualStyleBackColor = true;

            // 
            // OkayButton 
            // 
            OkayButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            OkayButton.Location = new Point(271, 6);
            OkayButton.Name = "OkayButton";
            OkayButton.Size = new Size(75, 23);
            OkayButton.TabIndex = 0;
            OkayButton.Text = "&Okay";
            OkayButton.UseVisualStyleBackColor = true;
            OkayButton.Click += OkayButton_Click;
            // 
            // ResetButton
            // 
            ResetButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ResetButton.Location = new Point(6, 6);
            ResetButton.Name = "ResetButton";
            ResetButton.Size = new Size(75, 23);
            ResetButton.TabIndex = 0;
            ResetButton.Text = "&Reset";
            ResetButton.UseVisualStyleBackColor = true;
            ResetButton.Click += ResetButton_Click;

            // 
            // GEN_GB_Scaling (GEN - General, GB - GroupBox)
            // 
            GEN_GB_Scaling.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            GEN_GB_Scaling.Controls.Add(GEN_GB_Scaling_Selection);
            GEN_GB_Scaling.Controls.Add(GEN_GB_Scaling_Display);
            GEN_GB_Scaling.Location = new Point(6, 6);
            GEN_GB_Scaling.Name = "GEN_GB_Scaling";
            GEN_GB_Scaling.Size = new Size(332, 43);
            GEN_GB_Scaling.TabIndex = 3;
            GEN_GB_Scaling.TabStop = false;
            GEN_GB_Scaling.Text = "Scale Points With Camera";
            // 
            // GEN_GB_Scaling_Display
            // 
            GEN_GB_Scaling_Display.AutoSize = true;
            GEN_GB_Scaling_Display.Location = new Point(6, 19);
            GEN_GB_Scaling_Display.Name = "GEN_GB_Scaling_Display";
            GEN_GB_Scaling_Display.Size = new Size(60, 17);
            GEN_GB_Scaling_Display.TabIndex = 2;
            GEN_GB_Scaling_Display.Text = "Display";
            GEN_GB_Scaling_Display.UseVisualStyleBackColor = true;
            GEN_GB_Scaling_Display.CheckedChanged += GEN_GB_Scaling_Display_CheckedChanged;
            // 
            // GEN_GB_Scaling_Selection
            // 
            GEN_GB_Scaling_Selection.AutoSize = true;
            GEN_GB_Scaling_Selection.Location = new Point(149, 19);
            GEN_GB_Scaling_Selection.Name = "GEN_GB_Scaling_Selection";
            GEN_GB_Scaling_Selection.Size = new Size(70, 17);
            GEN_GB_Scaling_Selection.TabIndex = 2;
            GEN_GB_Scaling_Selection.Text = "Selection";
            GEN_GB_Scaling_Selection.UseVisualStyleBackColor = true;
            GEN_GB_Scaling_Selection.CheckedChanged += GEN_GB_Scaling_Selection_CheckedChanged;


            // 
            // GEN_GB_Viewport
            // 
            GEN_GB_Viewport.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            GEN_GB_Viewport.Controls.Add(GEN_GB_Viewport_DefaultProjection);
            GEN_GB_Viewport.Controls.Add(GEN_GB_Viewport_DefaultProjectionLabel);
            GEN_GB_Viewport.Controls.Add(GEN_GB_Viewport_Colors);
            GEN_GB_Viewport.Location = new Point(6, 52);
            GEN_GB_Viewport.Size = new Size(332, 120);
            GEN_GB_Viewport.Name = "GEN_GB_Viewport";
            GEN_GB_Viewport.TabIndex = 3;
            GEN_GB_Viewport.TabStop = false;
            GEN_GB_Viewport.Text = "Viewport";
            //
            // GEN_GB_Viewport_DefaultProjection
            //
            GEN_GB_Viewport_DefaultProjection.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            GEN_GB_Viewport_DefaultProjection.Location = new Point(79, 19);
            GEN_GB_Viewport_DefaultProjection.Name = "GEN_GB_Viewport_DefaultProjection";
            GEN_GB_Viewport_DefaultProjection.Size = new Size(241, 19);
            GEN_GB_Viewport_DefaultProjection.TabIndex = 3;
            GEN_GB_Viewport_DefaultProjection.TabStop = true;
            GEN_GB_Viewport_DefaultProjection.DropDownStyle = ComboBoxStyle.DropDownList;

            //
            // GEN_GB_Viewport_DefaultProjectionLabel
            //
            GEN_GB_Viewport_DefaultProjectionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            GEN_GB_Viewport_DefaultProjectionLabel.Location = new Point(10, 19);
            GEN_GB_Viewport_DefaultProjectionLabel.BorderStyle = BorderStyle.FixedSingle;
            GEN_GB_Viewport_DefaultProjectionLabel.Name = "GEN_GB_Viewport_DefaultProjectionLabel";
            GEN_GB_Viewport_DefaultProjectionLabel.Size = new Size(70, 21);
            GEN_GB_Viewport_DefaultProjectionLabel.TextAlign = ContentAlignment.MiddleRight;
            GEN_GB_Viewport_DefaultProjectionLabel.TabIndex = 3;
            GEN_GB_Viewport_DefaultProjectionLabel.Text = "Projection:";
            GEN_GB_Viewport_DefaultProjectionLabel.TabStop = false;
            // 
            // GEN_GB_Viewport_Colors
            // 
            GEN_GB_Viewport_Colors.Anchor =
                AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            GEN_GB_Viewport_Colors.Controls.Add(GEN_GB_Viewport_Colors_Background_Color);
            GEN_GB_Viewport_Colors.Controls.Add(GEN_GB_Viewport_Colors_Background_ColorText);
            GEN_GB_Viewport_Colors.Controls.Add(GEN_GB_Viewport_Colors_Background_Label);
            GEN_GB_Viewport_Colors.Controls.Add(GEN_GB_Viewport_Colors__Color);
            GEN_GB_Viewport_Colors.Controls.Add(GEN_GB_Viewport_Colors__ColorText);
            GEN_GB_Viewport_Colors.Controls.Add(GEN_GB_Viewport_Colors__Label);
            GEN_GB_Viewport_Colors.Location = new Point(6, 48);
            GEN_GB_Viewport_Colors.Name = "GEN_GB_Viewport_Colors";
            GEN_GB_Viewport_Colors.Size = new Size(320, 66);
            GEN_GB_Viewport_Colors.TabIndex = 3;
            GEN_GB_Viewport_Colors.TabStop = false;
            GEN_GB_Viewport_Colors.Text = "Colors";
            // 
            // GEN_GB_Viewport_Colors_Background_Color
            // 
            GEN_GB_Viewport_Colors_Background_Color.AutoSize = false;
            GEN_GB_Viewport_Colors_Background_Color.BorderStyle = BorderStyle.FixedSingle;
            GEN_GB_Viewport_Colors_Background_Color.Location = new Point(269, 19);
            GEN_GB_Viewport_Colors_Background_Color.Name = "GEN_GB_Viewport_Colors_Background_Color";
            GEN_GB_Viewport_Colors_Background_Color.Size = new Size(40, 20);
            GEN_GB_Viewport_Colors_Background_Color.TabIndex = 0;
            GEN_GB_Viewport_Colors_Background_Color.Click += GEN_GB_Viewport_Colors_Background_UpdateColor_Click;
            // 
            // GEN_GB_Viewport_Colors_Background_ColorText
            // 
            GEN_GB_Viewport_Colors_Background_ColorText.AutoSize = false;
            GEN_GB_Viewport_Colors_Background_ColorText.TextAlign = ContentAlignment.MiddleCenter;
            GEN_GB_Viewport_Colors_Background_ColorText.BorderStyle = BorderStyle.FixedSingle;
            GEN_GB_Viewport_Colors_Background_ColorText.Location = new Point(79, 19);
            GEN_GB_Viewport_Colors_Background_ColorText.Name = "GEN_GB_Viewport_Colors_Background_ColorText";
            GEN_GB_Viewport_Colors_Background_ColorText.Size = new Size(191, 20);
            GEN_GB_Viewport_Colors_Background_ColorText.Font =
                new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
            GEN_GB_Viewport_Colors_Background_ColorText.TabIndex = 0;
            GEN_GB_Viewport_Colors_Background_ColorText.Text = "";
            GEN_GB_Viewport_Colors_Background_ColorText.Click += GEN_GB_Viewport_Colors_Background_UpdateColor_Click;
            // 
            // GEN_GB_Viewport_Colors_Background_Label
            // 
            GEN_GB_Viewport_Colors_Background_Label.AutoSize = false;
            GEN_GB_Viewport_Colors_Background_Label.TextAlign = ContentAlignment.MiddleRight;
            GEN_GB_Viewport_Colors_Background_Label.BorderStyle = BorderStyle.FixedSingle;
            GEN_GB_Viewport_Colors_Background_Label.Location = new Point(10, 19);
            GEN_GB_Viewport_Colors_Background_Label.Name = "GEN_GB_Viewport_Colors_Background_Label";
            GEN_GB_Viewport_Colors_Background_Label.Size = new Size(70, 20);
            GEN_GB_Viewport_Colors_Background_Label.TabIndex = 0;
            GEN_GB_Viewport_Colors_Background_Label.Text = "Background:";
            GEN_GB_Viewport_Colors_Background_Label.Click += GEN_GB_Viewport_Colors_Background_UpdateColor_Click;
            // 
            // GEN_GB_Viewport_Colors__Color
            // 
            GEN_GB_Viewport_Colors__Color.AutoSize = false;
            GEN_GB_Viewport_Colors__Color.BorderStyle = BorderStyle.FixedSingle;
            GEN_GB_Viewport_Colors__Color.Location = new Point(269, 38);
            GEN_GB_Viewport_Colors__Color.Name = "GEN_GB_Viewport_Colors__Color";
            GEN_GB_Viewport_Colors__Color.Size = new Size(40, 20);
            GEN_GB_Viewport_Colors__Color.TabIndex = 0;
            //this.GEN_GB_Viewport_Colors__Color.Click += GEN_GB_Viewport_Colors__UpdateColor_Click;
            // 
            // GEN_GB_Viewport_Colors__ColorText
            // 
            GEN_GB_Viewport_Colors__ColorText.AutoSize = false;
            GEN_GB_Viewport_Colors__ColorText.TextAlign = ContentAlignment.MiddleCenter;
            GEN_GB_Viewport_Colors__ColorText.BorderStyle = BorderStyle.FixedSingle;
            GEN_GB_Viewport_Colors__ColorText.Location = new Point(79, 38);
            GEN_GB_Viewport_Colors__ColorText.Name = "GEN_GB_Viewport_Colors__ColorText";
            GEN_GB_Viewport_Colors__ColorText.Size = new Size(191, 20);
            GEN_GB_Viewport_Colors__ColorText.Font =
                new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
            GEN_GB_Viewport_Colors__ColorText.TabIndex = 0;
            GEN_GB_Viewport_Colors__ColorText.Text = "";
            //this.GEN_GB_Viewport_Colors__ColorText.Click += GEN_GB_Viewport_Colors__UpdateColor_Click;
            // 
            // GEN_GB_Viewport_Colors__Label
            // 
            GEN_GB_Viewport_Colors__Label.AutoSize = false;
            GEN_GB_Viewport_Colors__Label.TextAlign = ContentAlignment.MiddleRight;
            GEN_GB_Viewport_Colors__Label.BorderStyle = BorderStyle.FixedSingle;
            GEN_GB_Viewport_Colors__Label.Location = new Point(10, 38);
            GEN_GB_Viewport_Colors__Label.Name = "GEN_GB_Viewport_Colors__Label";
            GEN_GB_Viewport_Colors__Label.Size = new Size(70, 20);
            GEN_GB_Viewport_Colors__Label.TabIndex = 0;
            GEN_GB_Viewport_Colors__Label.Text = "";
            //this.GEN_GB_Viewport_Colors__Label.Click += GEN_GB_Viewport_Colors__UpdateColor_Click;


            // 
            // GEN_Check_OnlySelectIfObjectEquals (Check - CheckBox)
            // 
            GEN_Check_OnlySelectIfObjectEquals.AutoSize = true;
            GEN_Check_OnlySelectIfObjectEquals.Location = new Point(6, 180);
            GEN_Check_OnlySelectIfObjectEquals.Name = "GEN_Check_OnlySelectIfObjectEquals";
            GEN_Check_OnlySelectIfObjectEquals.Size = new Size(279, 17);
            GEN_Check_OnlySelectIfObjectEquals.TabIndex = 2;
            GEN_Check_OnlySelectIfObjectEquals.Text = "Only select if collision object equals to selected object";
            GEN_Check_OnlySelectIfObjectEquals.UseVisualStyleBackColor = true;
            GEN_Check_OnlySelectIfObjectEquals.CheckedChanged += GEN_Check_OnlySelectIfObjectEquals_CheckedChanged;
            // 
            // GEN_Check_ReplaceSingleButtonCam
            // 
            GEN_Check_ReplaceSingleButtonCam.AutoSize = true;
            GEN_Check_ReplaceSingleButtonCam.Location = new Point(6, 203);
            GEN_Check_ReplaceSingleButtonCam.Name = "GEN_Check_ReplaceSingleButtonCam";
            GEN_Check_ReplaceSingleButtonCam.Size = new Size(270, 30);
            GEN_Check_ReplaceSingleButtonCam.TabIndex = 2;
            GEN_Check_ReplaceSingleButtonCam.Text = "Replace single-button camera perspective type with\r\ntwo buttons";
            GEN_Check_ReplaceSingleButtonCam.UseVisualStyleBackColor = true;
            GEN_Check_ReplaceSingleButtonCam.CheckedChanged += GEN_Check_ReplaceSingleButtonCam_CheckedChanged;
            // 
            // GEN_Check_AlwaysShowUndoRedoMenuOnStart
            // 
            GEN_Check_AlwaysShowUndoRedoMenuOnStart.AutoSize = true;
            GEN_Check_AlwaysShowUndoRedoMenuOnStart.Location = new Point(6, 236);
            GEN_Check_AlwaysShowUndoRedoMenuOnStart.Name = "GEN_Check_AlwaysShowUndoRedoMenuOnStart";
            GEN_Check_AlwaysShowUndoRedoMenuOnStart.Size = new Size(270, 30);
            GEN_Check_AlwaysShowUndoRedoMenuOnStart.TabIndex = 2;
            GEN_Check_AlwaysShowUndoRedoMenuOnStart.Text = "Always show Undo/Redo Menu at start";
            GEN_Check_AlwaysShowUndoRedoMenuOnStart.UseVisualStyleBackColor = true;
            GEN_Check_AlwaysShowUndoRedoMenuOnStart.CheckedChanged +=
                GEN_Check_AlwaysShowUndoRedoMenuOnStart_CheckedChanged;
            // 
            // GEN_Check_ResetPerspectiveOrthoCamToZeroZero
            // 
            GEN_Check_ResetPerspectiveOrthoCamToZeroZero.AutoSize = true;
            GEN_Check_ResetPerspectiveOrthoCamToZeroZero.Location = new Point(6, 259);
            GEN_Check_ResetPerspectiveOrthoCamToZeroZero.Name = "GEN_Check_ResetPerspectiveOrthoCamToZeroZero";
            GEN_Check_ResetPerspectiveOrthoCamToZeroZero.Size = new Size(270, 30);
            GEN_Check_ResetPerspectiveOrthoCamToZeroZero.TabIndex = 2;
            GEN_Check_ResetPerspectiveOrthoCamToZeroZero.Text =
                "Reset Perspective and Orthographic to its default location\r\n(will not affect saved camera points)";
            GEN_Check_ResetPerspectiveOrthoCamToZeroZero.UseVisualStyleBackColor = true;
            GEN_Check_ResetPerspectiveOrthoCamToZeroZero.CheckedChanged +=
                GEN_Check_ResetPerspectiveOrthoCamToZeroZero_CheckedChanged;


            // 
            // GEN_maxUndoRedo_Label
            // 
            GEN_maxUndoRedo_Label.AutoSize = true;
            GEN_maxUndoRedo_Label.Location = new Point(4, 367);
            GEN_maxUndoRedo_Label.Name = "GEN_maxUndoRedo_Label";
            GEN_maxUndoRedo_Label.Size = new Size(145, 13);
            GEN_maxUndoRedo_Label.TabIndex = 0;
            GEN_maxUndoRedo_Label.Text = "Maximum Undo/Redo Count:";
            // 
            // GEN_maxUndoRedo_Value
            // 
            GEN_maxUndoRedo_Value.Integer = true;
            GEN_maxUndoRedo_Value.Integral = true;
            GEN_maxUndoRedo_Value.Location = new Point(155, 364);
            GEN_maxUndoRedo_Value.MaximumValue = 3.402823E+38F;
            GEN_maxUndoRedo_Value.MinimumValue = 0;
            GEN_maxUndoRedo_Value.Name = "GEN_maxUndoRedo_Value";
            GEN_maxUndoRedo_Value.Size = new Size(66, 20);
            GEN_maxUndoRedo_Value.TabIndex = 1;
            GEN_maxUndoRedo_Value.Text = "0";
            GEN_maxUndoRedo_Value.ValueChanged += GEN_maxUndoRedo_Value_ValueChanged;


            // 
            // COPY_GB_Copy
            // 
            COPY_GB_Copy.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            COPY_GB_Copy.Controls.Add(COPY_GB_Copy_CB_SelObjectEquals);
            COPY_GB_Copy.Location = new Point(6, 6);
            COPY_GB_Copy.Name = "COPY_GB_Copy";
            COPY_GB_Copy.Size = new Size(332, 42);
            COPY_GB_Copy.TabIndex = 4;
            COPY_GB_Copy.TabStop = false;
            COPY_GB_Copy.Text = "Copy Options";
            // 
            // COPY_GB_Copy_CB_SelObjectEquals
            // 
            COPY_GB_Copy_CB_SelObjectEquals.AutoSize = true;
            COPY_GB_Copy_CB_SelObjectEquals.Location = new Point(6, 19);
            COPY_GB_Copy_CB_SelObjectEquals.Name = "COPY_GB_Copy_CB_SelObjectEquals";
            COPY_GB_Copy_CB_SelObjectEquals.Size = new Size(258, 17);
            COPY_GB_Copy_CB_SelObjectEquals.TabIndex = 2;
            COPY_GB_Copy_CB_SelObjectEquals.Text = "Copy if selected object equals to collision\'s object";
            COPY_GB_Copy_CB_SelObjectEquals.UseVisualStyleBackColor = true;
            COPY_GB_Copy_CB_SelObjectEquals.CheckedChanged += COPY_GB_Copy_CB_SelObjectEquals_CheckedChanged;

            // 
            // PASTE_GB_Paste
            // 
            PASTE_GB_Paste.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PASTE_GB_Paste.Controls.Add(PASTE_GB_Paste_CB_RemSelCollsWhenPasting);
            PASTE_GB_Paste.Controls.Add(PASTE_GB_Paste_CB_UseWorldLinkValues);
            PASTE_GB_Paste.Location = new Point(6, 54);
            PASTE_GB_Paste.Name = "PASTE_GB_Paste";
            PASTE_GB_Paste.Size = new Size(332, 66);
            PASTE_GB_Paste.TabIndex = 4;
            PASTE_GB_Paste.TabStop = false;
            PASTE_GB_Paste.Text = "Paste Options";
            // 
            // PASTE_GB_Paste_CB_RemSelCollsWhenPasting
            // 
            PASTE_GB_Paste_CB_RemSelCollsWhenPasting.AutoSize = true;
            PASTE_GB_Paste_CB_RemSelCollsWhenPasting.Location = new Point(6, 42);
            PASTE_GB_Paste_CB_RemSelCollsWhenPasting.Name = "checkBox3";
            PASTE_GB_Paste_CB_RemSelCollsWhenPasting.Size = new Size(220, 17);
            PASTE_GB_Paste_CB_RemSelCollsWhenPasting.TabIndex = 2;
            PASTE_GB_Paste_CB_RemSelCollsWhenPasting.Text = "Remove selected collisions when pasting";
            PASTE_GB_Paste_CB_RemSelCollsWhenPasting.UseVisualStyleBackColor = true;
            PASTE_GB_Paste_CB_RemSelCollsWhenPasting.CheckedChanged +=
                PASTE_GB_Paste_CB_RemSelCollsWhenPasting_CheckedChanged;
            // 
            // PASTE_GB_Paste_CB_UseWorldLinkValues
            // 
            PASTE_GB_Paste_CB_UseWorldLinkValues.AutoSize = true;
            PASTE_GB_Paste_CB_UseWorldLinkValues.Location = new Point(6, 19);
            PASTE_GB_Paste_CB_UseWorldLinkValues.Name = "PASTE_GB_Paste_CB_UseWorldLinkValues";
            PASTE_GB_Paste_CB_UseWorldLinkValues.Size = new Size(239, 17);
            PASTE_GB_Paste_CB_UseWorldLinkValues.TabIndex = 2;
            PASTE_GB_Paste_CB_UseWorldLinkValues.Text = "Use actual link values instead of raw variants";
            PASTE_GB_Paste_CB_UseWorldLinkValues.UseVisualStyleBackColor = true;
            PASTE_GB_Paste_CB_UseWorldLinkValues.CheckedChanged += PASTE_GB_Paste_CB_UseWorldLinkValues_CheckedChanged;


            // 
            // BrawlCrate_Settings_CollisionEditor
            // 
            ClientSize = new Size(352, 454);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = BrawlLib.Properties.Resources.Icon;
            Controls.Add(tabC);
            Controls.Add(main_bottomPanel);
            Name = "CollisionEditorSettings";
            Text = "Collision Editor Settings";
            MinimumSize = Size;
            MaximumSize = Size;
            MaximizeBox = false;

            main_bottomPanel.ResumeLayout(false);

            tabC.ResumeLayout(false);

            GEN.ResumeLayout(false);
            GEN.PerformLayout();
            GEN_GB_Scaling.ResumeLayout(false);
            GEN_GB_Scaling.PerformLayout();

            COPYPASTE.ResumeLayout(false);
            COPY_GB_Copy.ResumeLayout(false);
            COPY_GB_Copy.PerformLayout();
            PASTE_GB_Paste.ResumeLayout(false);
            PASTE_GB_Paste.PerformLayout();

            ResumeLayout(false);
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
            if (MessageBox.Show(this, "Are you sure you want to reset all settings to its default state?",
                "Reset Settings",
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
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
            {
                return;
            }

            ParentEditor.maxSaveCount = (int) GEN_maxUndoRedo_Value.Value;
        }

        private void GEN_GB_Viewport_Colors_R_ValueChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            UpdateViewportBGColor();
        }

        private void GEN_GB_Viewport_Colors_G_ValueChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            UpdateViewportBGColor();
        }

        private void GEN_GB_Viewport_Colors_B_ValueChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            UpdateViewportBGColor();
        }

        private void GEN_GB_Viewport_Colors_A_ValueChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            UpdateViewportBGColor();
        }

        private void GEN_GB_Viewport_Colors_Background_UpdateColor_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = (Color) ParentEditor.RetrieveSettings().BackgroundColor;

            if (ColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                ParentEditor.CreateCollisionEditorSettingsIfNotExists();

                Properties.Settings.Default.CollisionEditorSettings.BackgroundColor = (ARGBPixel) ColorDialog.Color;
                Properties.Settings.Default.Save();

                UpdateViewportBGColor();
            }
        }

        private void UpdateViewportBGColor()
        {
            ParentEditor.CreateCollisionEditorSettingsIfNotExists();

            ARGBPixel bgColorP = Properties.Settings.Default.CollisionEditorSettings.BackgroundColor;
            Color bgColor = (Color) bgColorP;

            GEN_GB_Viewport_Colors_Background_ColorText.Text = bgColorP.ToString();
            GEN_GB_Viewport_Colors_Background_Color.BackColor = Color.FromArgb(bgColor.R, bgColor.G, bgColor.B);

            ParentEditor.UpdateViewportBackgroundColor(bgColor.A, bgColor.R, bgColor.G, bgColor.B);
        }

        private void GEN_GB_Scaling_Display_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Updating = true;
            ParentEditor.toolsStrip_Options_ScalePointsWithCamera_DisplayOnly.Checked = GEN_GB_Scaling_Display.Checked;
            Updating = false;
        }

        private void GEN_GB_Scaling_Selection_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Updating = true;
            ParentEditor.toolsStrip_Options_ScalePointsWithCamera_SelectOnly.Checked = GEN_GB_Scaling_Selection.Checked;
            Updating = false;
        }

        private void GEN_Check_OnlySelectIfObjectEquals_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Updating = true;
            ParentEditor.toolsStrip_Options_SelectOnlyIfObjectEquals.Checked =
                GEN_Check_OnlySelectIfObjectEquals.Checked;
            Updating = false;
        }

        private void GEN_Check_ReplaceSingleButtonCam_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            ParentEditor.ToggleViewportButtonVisibility(GEN_Check_ReplaceSingleButtonCam.Checked);
        }

        private void GEN_Check_AlwaysShowUndoRedoMenuOnStart_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            ParentEditor.CreateCollisionEditorSettingsIfNotExists();

            Properties.Settings.Default.CollisionEditorSettings.AlwaysShowUndoRedoMenuOnStart =
                GEN_Check_AlwaysShowUndoRedoMenuOnStart.Checked;
            Properties.Settings.Default.Save();
        }

        private void GEN_Check_ResetPerspectiveOrthoCamToZeroZero_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            ParentEditor.CreateCollisionEditorSettingsIfNotExists();

            ParentEditor.toolsStrip_CameraOptions_SaveCameraPoint.Enabled =
                !(Properties.Settings.Default.CollisionEditorSettings.ResetPerspectiveOrthographicCameraToZeroZero =
                    GEN_Check_ResetPerspectiveOrthoCamToZeroZero.Checked);

            Properties.Settings.Default.Save();
        }

        private void COPY_GB_Copy_CB_SelObjectEquals_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Updating = true;
            ParentEditor.clipboardCopyOptions_OnlySelectObjectIfCollisionObjectEquals.Checked =
                COPY_GB_Copy_CB_SelObjectEquals.Checked;
            Updating = false;
        }

        private void PASTE_GB_Paste_CB_RemSelCollsWhenPasting_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Updating = true;
            ParentEditor.clipboardPasteOptions_PasteRemoveSelected.Checked =
                PASTE_GB_Paste_CB_RemSelCollsWhenPasting.Checked;
            Updating = false;
        }

        private void PASTE_GB_Paste_CB_UseWorldLinkValues_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Updating = true;
            ParentEditor.clipboardPasteOptions_ActualPointsValuesAreUsed.Checked =
                PASTE_GB_Paste_CB_UseWorldLinkValues.Checked;
            Updating = false;
        }

        private void GEN_GB_Viewport_DefaultProjection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Updating = true;
            ParentEditor.UpdateViewportProjection((ViewportProjection) GEN_GB_Viewport_DefaultProjection.SelectedIndex);
            Updating = false;
        }

        #region CollisionEditorSettings_Data Read/Write

        public void UpdateSettings(bool LoadingSettings)
        {
            if (Updating)
            {
                return;
            }


            Updating = true;

            CollisionEditorSettings_Data settings = ParentEditor.RetrieveSettings();

            GEN_GB_Scaling_Display.Checked = ParentEditor.toolsStrip_Options_ScalePointsWithCamera_DisplayOnly.Checked;
            GEN_GB_Scaling_Selection.Checked = ParentEditor.toolsStrip_Options_ScalePointsWithCamera_SelectOnly.Checked;

            GEN_Check_ReplaceSingleButtonCam.Checked =
                !ParentEditor.toolsStrip_TogglePerspectiveOrthographicCam.Visible;
            GEN_Check_AlwaysShowUndoRedoMenuOnStart.Checked = settings.AlwaysShowUndoRedoMenuOnStart;

            GEN_maxUndoRedo_Value.Value = ParentEditor.maxSaveCount;

            COPY_GB_Copy_CB_SelObjectEquals.Checked =
                ParentEditor.clipboardCopyOptions_OnlySelectObjectIfCollisionObjectEquals.Checked;
            PASTE_GB_Paste_CB_UseWorldLinkValues.Checked =
                ParentEditor.clipboardPasteOptions_ActualPointsValuesAreUsed.Checked;
            PASTE_GB_Paste_CB_RemSelCollsWhenPasting.Checked =
                ParentEditor.clipboardPasteOptions_PasteRemoveSelected.Checked;

            GEN_GB_Viewport_DefaultProjection.SelectedIndex = (int) ParentEditor._modelPanel.CurrentViewport.ViewType;

            if (LoadingSettings)
            {
                // To combat the issue in the viewport being updated when we are just updating the settings, we add the event after it.
                GEN_GB_Viewport_DefaultProjection.SelectedIndexChanged +=
                    GEN_GB_Viewport_DefaultProjection_SelectedIndexChanged;
            }

            GEN_Check_ResetPerspectiveOrthoCamToZeroZero.Checked =
                settings.ResetPerspectiveOrthographicCameraToZeroZero;

            UpdateViewportBGColor();

            Updating = false;
        }

        #endregion

        #region Form Events

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            ParentEditor.NullifyEditorSettings();
        }

        #endregion
    }
}