using System.ComponentModel;
using System.Drawing;
using BrawlLib.Internal;
using BrawlLib.Internal.Windows.Controls;
using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using OpenTK.Graphics.OpenGL;

namespace System.Windows.Forms
{
    public class CollisionEditor_PasteOptions : Form
    {
        #region Designer

        // The main tab control that the user controls.
        private TabControl advancedPO_TabC;
        private TabPage advancedPO_TabC_ManipulateCollisionTab;
        private TabPage advancedPO_TabC_AllCollsProps;

        private Panel BottomPanel;
        private Button PasteCollision;
        private Button Cancel;

        private ToolTip pasteOptions_tipper;


        // ManiColl is Manipulate Collision. 
        private GroupBox ManiColl_RotateGroup;
        private Label ManiColl_Rotate_DegreesLabel;
        private NumericUpDown ManiColl_Rotate_DegreesValue;
        private Button ManiColl_Rotate_SetRotationLocation;
        private Button ManiColl_Rotate_SetTo0;
        private Button ManiColl_Rotate_SetTo270;
        private Button ManiColl_Rotate_SetTo180;
        private Button ManiColl_Rotate_SetTo90;

        private GroupBox ManiColl_FlipGroup;
        private CheckBox ManiColl_Flip_FlipY;
        private CheckBox ManiColl_Flip_FlipX;

        private GroupBox ManiColl_ScaleGroup;
        private Label ManiColl_Scale_XLabel;
        private Button ManiColl_Scale_ResetScale;
        private NumericUpDown ManiColl_Scale_ScalarValue;

        private GroupBox ManiColl_PlacementGroup;
        private RadioButton ManiColl_Placement_RadioSpefPlacement;
        private RadioButton ManiColl_Placement_RadioFreeplace;
        private RadioButton ManiColl_Placement_RadioZeroZero;
        private Label ManiColl_Placement_SpefPlacement_YLabel;
        private Label ManiColl_Placement_SpefPlacement_XLabel;
        private NumericInputBox ManiColl_Placement_SpefPlacement_LocY;
        private NumericInputBox ManiColl_Placement_SpefPlacement_LocX;


        // AllCollsProps and ACP are All Collision Properties
        private Panel advancedPO_TabC_AllCollsProps_Panel;

        private LinkLabel AllCollsProps_WarningLink;
        private Button ACP_ResetValuesToDef;


        private Label ACP_TypeLabel;
        private CheckBox ACP_Type_SelectedOverrides;
        private ComboBox ACP_TypeComboBox;

        private Label ACP_MaterialLabel;
        private CheckBox ACP_Material_SelectedOverrides;
        private ComboBox ACP_MaterialComboBox;

        private GroupBox ACP_CollTargetsGroup;
        private CheckBox ACP_CollTargets_SelectedOverrides;
        private CheckBox ACP_CollTargets_Items;
        private CheckBox ACP_CollTargets_PKMNTrainer;
        private CheckBox ACP_CollTargets_Everything;

        private GroupBox ACP_CollFlagsGroup;
        private CheckBox ACP_CollFlags_SelectedOverrides;
        private CheckBox ACP_CollFlags_NoWallJump;
        private CheckBox ACP_CollFlags_Rotating;
        private CheckBox ACP_CollFlags_LeftLedge;
        private CheckBox ACP_CollFlags_RightLedge;
        private CheckBox ACP_CollFlags_FallThrough;

        private GroupBox ACP_UnkFlagsGroup;
        private CheckBox ACP_UnkFlags_SelectedOverrides;
        private CheckBox ACP_UnkFlags_Unk1;
        private CheckBox ACP_UnkFlags_Unk4;
        private CheckBox ACP_UnkFlags_SuperSoft;
        private CheckBox ACP_UnkFlags_Unk2;


        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();

            advancedPO_TabC = new TabControl();
            advancedPO_TabC_ManipulateCollisionTab = new TabPage();

            BottomPanel = new Panel();

            PasteCollision = new Button();
            Cancel = new Button();

            pasteOptions_tipper = new ToolTip(components);

            ManiColl_RotateGroup = new GroupBox();
            ManiColl_Rotate_DegreesValue = new NumericUpDown();
            ManiColl_Rotate_DegreesLabel = new Label();
            ManiColl_Rotate_SetTo0 = new Button();
            ManiColl_Rotate_SetTo90 = new Button();
            ManiColl_Rotate_SetTo180 = new Button();
            ManiColl_Rotate_SetTo270 = new Button();
            ManiColl_Rotate_SetRotationLocation = new Button();

            ManiColl_ScaleGroup = new GroupBox();
            ManiColl_Scale_ScalarValue = new NumericUpDown();
            ManiColl_Scale_XLabel = new Label();
            ManiColl_Scale_ResetScale = new Button();

            ManiColl_FlipGroup = new GroupBox();
            ManiColl_Flip_FlipY = new CheckBox();
            ManiColl_Flip_FlipX = new CheckBox();

            ManiColl_PlacementGroup = new GroupBox();
            ManiColl_Placement_SpefPlacement_YLabel = new Label();
            ManiColl_Placement_SpefPlacement_XLabel = new Label();
            ManiColl_Placement_SpefPlacement_LocY = new NumericInputBox();
            ManiColl_Placement_SpefPlacement_LocX = new NumericInputBox();
            ManiColl_Placement_RadioSpefPlacement = new RadioButton();
            ManiColl_Placement_RadioFreeplace = new RadioButton();
            ManiColl_Placement_RadioZeroZero = new RadioButton();


            advancedPO_TabC_AllCollsProps = new TabPage();
            advancedPO_TabC_AllCollsProps_Panel = new Panel();

            AllCollsProps_WarningLink = new LinkLabel();
            ACP_ResetValuesToDef = new Button();

            ACP_TypeLabel = new Label();
            ACP_Type_SelectedOverrides = new CheckBox();
            ACP_TypeComboBox = new ComboBox();

            ACP_MaterialLabel = new Label();
            ACP_Material_SelectedOverrides = new CheckBox();
            ACP_MaterialComboBox = new ComboBox();

            ACP_CollTargetsGroup = new GroupBox();
            ACP_CollTargets_SelectedOverrides = new CheckBox();
            ACP_CollTargets_Items = new CheckBox();
            ACP_CollTargets_PKMNTrainer = new CheckBox();
            ACP_CollTargets_Everything = new CheckBox();

            ACP_CollFlagsGroup = new GroupBox();
            ACP_CollFlags_SelectedOverrides = new CheckBox();
            ACP_CollFlags_NoWallJump = new CheckBox();
            ACP_CollFlags_Rotating = new CheckBox();
            ACP_CollFlags_LeftLedge = new CheckBox();
            ACP_CollFlags_RightLedge = new CheckBox();
            ACP_CollFlags_FallThrough = new CheckBox();

            ACP_UnkFlagsGroup = new GroupBox();
            ACP_UnkFlags_SelectedOverrides = new CheckBox();
            ACP_UnkFlags_Unk1 = new CheckBox();
            ACP_UnkFlags_Unk4 = new CheckBox();
            ACP_UnkFlags_SuperSoft = new CheckBox();
            ACP_UnkFlags_Unk2 = new CheckBox();


            advancedPO_TabC.SuspendLayout();
            advancedPO_TabC_ManipulateCollisionTab.SuspendLayout();

            ManiColl_PlacementGroup.SuspendLayout();
            ManiColl_ScaleGroup.SuspendLayout();

            ((ISupportInitialize) ManiColl_Scale_ScalarValue).BeginInit();
            ((ISupportInitialize) ManiColl_Rotate_DegreesValue).BeginInit();

            ManiColl_FlipGroup.SuspendLayout();
            ManiColl_RotateGroup.SuspendLayout();

            advancedPO_TabC_AllCollsProps.SuspendLayout();
            advancedPO_TabC_AllCollsProps_Panel.SuspendLayout();

            ACP_UnkFlagsGroup.SuspendLayout();
            ACP_CollFlagsGroup.SuspendLayout();
            ACP_CollTargetsGroup.SuspendLayout();

            BottomPanel.SuspendLayout();
            SuspendLayout();

            // 
            // advancedPO_TabC
            // 
            advancedPO_TabC.Controls.Add(advancedPO_TabC_ManipulateCollisionTab);
            advancedPO_TabC.Controls.Add(advancedPO_TabC_AllCollsProps);
            advancedPO_TabC.Dock = DockStyle.Fill;
            advancedPO_TabC.Location = new Drawing.Point(0, 0);
            advancedPO_TabC.Name = "advancedPO_TabC";
            advancedPO_TabC.SelectedIndex = 0;
            advancedPO_TabC.Size = new Drawing.Size(236, 407);
            advancedPO_TabC.TabIndex = 0;
            // 
            // advancedPO_TabC_ManipulateCollisionTab
            // 
            advancedPO_TabC_ManipulateCollisionTab.AutoScroll = true;
            advancedPO_TabC_ManipulateCollisionTab.Controls.Add(ManiColl_PlacementGroup);
            advancedPO_TabC_ManipulateCollisionTab.Controls.Add(ManiColl_ScaleGroup);
            advancedPO_TabC_ManipulateCollisionTab.Controls.Add(ManiColl_FlipGroup);
            advancedPO_TabC_ManipulateCollisionTab.Controls.Add(ManiColl_RotateGroup);
            advancedPO_TabC_ManipulateCollisionTab.Location = new Drawing.Point(4, 22);
            advancedPO_TabC_ManipulateCollisionTab.Name = "advancedPO_TabC_ManipulateCollisionTab";
            advancedPO_TabC_ManipulateCollisionTab.Padding = new Padding(3);
            advancedPO_TabC_ManipulateCollisionTab.Size = new Drawing.Size(228, 381);
            advancedPO_TabC_ManipulateCollisionTab.TabIndex = 0;
            advancedPO_TabC_ManipulateCollisionTab.Text = "Manipulate Collision";
            advancedPO_TabC_ManipulateCollisionTab.UseVisualStyleBackColor = true;
            // 
            // ManiColl_PlacementGroup
            // 
            ManiColl_PlacementGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ManiColl_PlacementGroup.Controls.Add(ManiColl_Placement_SpefPlacement_XLabel);
            ManiColl_PlacementGroup.Controls.Add(ManiColl_Placement_SpefPlacement_YLabel);
            ManiColl_PlacementGroup.Controls.Add(ManiColl_Placement_SpefPlacement_LocX);
            ManiColl_PlacementGroup.Controls.Add(ManiColl_Placement_SpefPlacement_LocY);
            ManiColl_PlacementGroup.Controls.Add(ManiColl_Placement_RadioSpefPlacement);
            ManiColl_PlacementGroup.Controls.Add(ManiColl_Placement_RadioFreeplace);
            ManiColl_PlacementGroup.Controls.Add(ManiColl_Placement_RadioZeroZero);
            ManiColl_PlacementGroup.Location = new Drawing.Point(4, 222);
            ManiColl_PlacementGroup.Name = "ManiColl_PlacementGroup";
            ManiColl_PlacementGroup.Size = new Drawing.Size(218, 127);
            ManiColl_PlacementGroup.TabIndex = 3;
            ManiColl_PlacementGroup.TabStop = false;
            ManiColl_PlacementGroup.Text = "Location/Placement";
            // 
            // ManiColl_Placement_RadioZeroZero
            // 
            ManiColl_Placement_RadioZeroZero.AutoSize = true;
            ManiColl_Placement_RadioZeroZero.Checked = true;
            ManiColl_Placement_RadioZeroZero.Location = new Drawing.Point(12, 19);
            ManiColl_Placement_RadioZeroZero.Name = "ManiColl_Placement_RadioZeroZero";
            ManiColl_Placement_RadioZeroZero.Size = new Drawing.Size(117, 17);
            ManiColl_Placement_RadioZeroZero.TabIndex = 0;
            ManiColl_Placement_RadioZeroZero.TabStop = true;
            ManiColl_Placement_RadioZeroZero.Text = "Center (0, 0)";
            ManiColl_Placement_RadioZeroZero.UseVisualStyleBackColor = true;
            ManiColl_Placement_RadioZeroZero.CheckedChanged +=
                new EventHandler(ManiColl_Placement_RadioZeroZero_CheckedChanged);
            pasteOptions_tipper.SetToolTip(ManiColl_Placement_RadioZeroZero,
                "It is placed at the center of the stage (0, 0).");
            // 
            // ManiColl_Placement_RadioSpefPlacement
            // 
            ManiColl_Placement_RadioSpefPlacement.AutoSize = true;
            ManiColl_Placement_RadioSpefPlacement.Location = new Drawing.Point(12, 56);
            ManiColl_Placement_RadioSpefPlacement.Name = "ManiColl_Placement_RadioSpefPlacement";
            ManiColl_Placement_RadioSpefPlacement.Size = new Drawing.Size(113, 17);
            ManiColl_Placement_RadioSpefPlacement.TabIndex = 0;
            ManiColl_Placement_RadioSpefPlacement.Text = "Specified Location";
            ManiColl_Placement_RadioSpefPlacement.UseVisualStyleBackColor = true;
            ManiColl_Placement_RadioSpefPlacement.CheckedChanged +=
                new EventHandler(ManiColl_Placement_RadioSpefPlacement_CheckedChanged);
            pasteOptions_tipper.SetToolTip(ManiColl_Placement_RadioSpefPlacement,
                "If you like being specific, you can specify the location by inputting the two values.");
            // 
            // ManiColl_Placement_RadioFreeplace
            // 
            ManiColl_Placement_RadioFreeplace.AutoSize = true;
            ManiColl_Placement_RadioFreeplace.Location = new Drawing.Point(12, 38);
            ManiColl_Placement_RadioFreeplace.Name = "ManiColl_Placement_RadioFreeplace";
            ManiColl_Placement_RadioFreeplace.Size = new Drawing.Size(72, 17);
            ManiColl_Placement_RadioFreeplace.TabIndex = 0;
            ManiColl_Placement_RadioFreeplace.Text = "Freeplace";
            ManiColl_Placement_RadioFreeplace.UseVisualStyleBackColor = true;
            ManiColl_Placement_RadioFreeplace.CheckedChanged +=
                new EventHandler(ManiColl_Placement_RadioFreeplace_CheckedChanged);
            pasteOptions_tipper.SetToolTip(ManiColl_Placement_RadioFreeplace,
                "If this is selected, you can move the center location of copied collisions anywhere.");
            // 
            // ManiColl_Placement_SpefPlacement_XLabel
            // 
            ManiColl_Placement_SpefPlacement_XLabel.BorderStyle = BorderStyle.FixedSingle;
            ManiColl_Placement_SpefPlacement_XLabel.Location = new Drawing.Point(33, 79);
            ManiColl_Placement_SpefPlacement_XLabel.Name = "ManiColl_Placement_SpefPlacement_XLabel";
            ManiColl_Placement_SpefPlacement_XLabel.Size = new Drawing.Size(20, 20);
            ManiColl_Placement_SpefPlacement_XLabel.TabIndex = 2;
            ManiColl_Placement_SpefPlacement_XLabel.Text = "X";
            ManiColl_Placement_SpefPlacement_XLabel.TextAlign = ContentAlignment.MiddleCenter;
            ManiColl_Placement_SpefPlacement_XLabel.Enabled = false;
            // 
            // ManiColl_Placement_SpefPlacement_LocX
            // 
            ManiColl_Placement_SpefPlacement_LocX.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ManiColl_Placement_SpefPlacement_LocX.BorderStyle = BorderStyle.FixedSingle;
            ManiColl_Placement_SpefPlacement_LocX.Integer = false;
            ManiColl_Placement_SpefPlacement_LocX.Integral = false;
            ManiColl_Placement_SpefPlacement_LocX.Location = new Drawing.Point(52, 79);
            ManiColl_Placement_SpefPlacement_LocX.MaximumValue = 3.402823E+38F;
            ManiColl_Placement_SpefPlacement_LocX.MinimumValue = -3.402823E+38F;
            ManiColl_Placement_SpefPlacement_LocX.Name = "ManiColl_Placement_SpefPlacement_LocX";
            ManiColl_Placement_SpefPlacement_LocX.Size = new Drawing.Size(135, 20);
            ManiColl_Placement_SpefPlacement_LocX.TabIndex = 1;
            ManiColl_Placement_SpefPlacement_LocX.Text = "0";
            ManiColl_Placement_SpefPlacement_LocX.ValueChanged +=
                new EventHandler(ManiColl_Placement_SpefPlacement_LocX_ValueChanged);
            ManiColl_Placement_SpefPlacement_LocX.Enabled = false;
            // 
            // ManiColl_Placement_SpefPlacement_YLabel
            // 
            ManiColl_Placement_SpefPlacement_YLabel.BorderStyle = BorderStyle.FixedSingle;
            ManiColl_Placement_SpefPlacement_YLabel.Location = new Drawing.Point(33, 98);
            ManiColl_Placement_SpefPlacement_YLabel.Name = "ManiColl_Placement_SpefPlacement_YLabel";
            ManiColl_Placement_SpefPlacement_YLabel.Size = new Drawing.Size(20, 20);
            ManiColl_Placement_SpefPlacement_YLabel.TabIndex = 2;
            ManiColl_Placement_SpefPlacement_YLabel.Text = "Y";
            ManiColl_Placement_SpefPlacement_YLabel.TextAlign = ContentAlignment.MiddleCenter;
            ManiColl_Placement_SpefPlacement_YLabel.Enabled = false;
            // 
            // ManiColl_Placement_SpefPlacement_LocY
            // 
            ManiColl_Placement_SpefPlacement_LocY.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ManiColl_Placement_SpefPlacement_LocY.BorderStyle = BorderStyle.FixedSingle;
            ManiColl_Placement_SpefPlacement_LocY.Integer = false;
            ManiColl_Placement_SpefPlacement_LocY.Integral = false;
            ManiColl_Placement_SpefPlacement_LocY.Location = new Drawing.Point(52, 98);
            ManiColl_Placement_SpefPlacement_LocY.MaximumValue = 3.402823E+38F;
            ManiColl_Placement_SpefPlacement_LocY.MinimumValue = -3.402823E+38F;
            ManiColl_Placement_SpefPlacement_LocY.Name = "ManiColl_Placement_SpefPlacement_LocY";
            ManiColl_Placement_SpefPlacement_LocY.Size = new Drawing.Size(135, 20);
            ManiColl_Placement_SpefPlacement_LocY.TabIndex = 1;
            ManiColl_Placement_SpefPlacement_LocY.Text = "0";
            ManiColl_Placement_SpefPlacement_LocY.ValueChanged +=
                new EventHandler(ManiColl_Placement_SpefPlacement_LocY_ValueChanged);
            ManiColl_Placement_SpefPlacement_LocY.Enabled = false;
            // 
            // ManiColl_ScaleGroup
            // 
            ManiColl_ScaleGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ManiColl_ScaleGroup.Controls.Add(ManiColl_Scale_ScalarValue);
            ManiColl_ScaleGroup.Controls.Add(ManiColl_Scale_XLabel);
            ManiColl_ScaleGroup.Controls.Add(ManiColl_Scale_ResetScale);
            ManiColl_ScaleGroup.Location = new Drawing.Point(4, 166);
            ManiColl_ScaleGroup.Name = "ManiColl_ScaleGroup";
            ManiColl_ScaleGroup.Size = new Drawing.Size(218, 50);
            ManiColl_ScaleGroup.TabIndex = 2;
            ManiColl_ScaleGroup.TabStop = false;
            ManiColl_ScaleGroup.Text = "Scale";
            // 
            // ManiColl_Scale_ScalarValue
            // 
            ManiColl_Scale_ScalarValue.Anchor = AnchorStyles.Top;
            ManiColl_Scale_ScalarValue.DecimalPlaces = 3;
            ManiColl_Scale_ScalarValue.Location = new Drawing.Point(39, 19);
            ManiColl_Scale_ScalarValue.Maximum = new decimal(new int[] {10000, 0, 0, 0});
            ManiColl_Scale_ScalarValue.Minimum = new decimal(new int[] {1, 0, 0, 196608});
            ManiColl_Scale_ScalarValue.Name = "ManiColl_Scale_ScalarValue";
            ManiColl_Scale_ScalarValue.Size = new Drawing.Size(79, 20);
            ManiColl_Scale_ScalarValue.TabIndex = 1;
            ManiColl_Scale_ScalarValue.Value = new decimal(new int[] {1, 0, 0, 0});
            ManiColl_Scale_ScalarValue.ValueChanged += new EventHandler(ManiColl_Scale_ScalarValue_ValueChanged);
            // 
            // ManiColl_Scale_XLabel
            // 
            ManiColl_Scale_XLabel.Anchor = AnchorStyles.Top;
            ManiColl_Scale_XLabel.AutoSize = true;
            ManiColl_Scale_XLabel.Location = new Drawing.Point(25, 21);
            ManiColl_Scale_XLabel.Name = "ManiColl_Scale_XLabel";
            ManiColl_Scale_XLabel.Size = new Drawing.Size(12, 13);
            ManiColl_Scale_XLabel.TabIndex = 2;
            ManiColl_Scale_XLabel.Text = "x";
            // 
            // ManiColl_Scale_ResetScale
            // 
            ManiColl_Scale_ResetScale.Anchor = AnchorStyles.Top;
            ManiColl_Scale_ResetScale.Location = new Drawing.Point(123, 17);
            ManiColl_Scale_ResetScale.Name = "ManiColl_Scale_ResetScale";
            ManiColl_Scale_ResetScale.Size = new Drawing.Size(66, 23);
            ManiColl_Scale_ResetScale.TabIndex = 1;
            ManiColl_Scale_ResetScale.Text = "Reset";
            ManiColl_Scale_ResetScale.UseVisualStyleBackColor = true;
            ManiColl_Scale_ResetScale.Click += new EventHandler(ManiColl_Scale_ResetScale_Click);
            // 
            // ManiColl_FlipGroup
            // 
            ManiColl_FlipGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ManiColl_FlipGroup.Controls.Add(ManiColl_Flip_FlipY);
            ManiColl_FlipGroup.Controls.Add(ManiColl_Flip_FlipX);
            ManiColl_FlipGroup.Location = new Drawing.Point(4, 116);
            ManiColl_FlipGroup.Name = "ManiColl_FlipGroup";
            ManiColl_FlipGroup.Size = new Drawing.Size(218, 46);
            ManiColl_FlipGroup.TabIndex = 1;
            ManiColl_FlipGroup.TabStop = false;
            ManiColl_FlipGroup.Text = "Flip";
            // 
            // ManiColl_Flip_FlipY
            // 
            ManiColl_Flip_FlipY.AutoSize = true;
            ManiColl_Flip_FlipY.Location = new Drawing.Point(113, 19);
            ManiColl_Flip_FlipY.Name = "ManiColl_Flip_FlipY";
            ManiColl_Flip_FlipY.Size = new Drawing.Size(74, 17);
            ManiColl_Flip_FlipY.TabIndex = 2;
            ManiColl_Flip_FlipY.Text = "Flip Y-Axis";
            ManiColl_Flip_FlipY.UseVisualStyleBackColor = true;
            ManiColl_Flip_FlipY.CheckedChanged += new EventHandler(ManiColl_Flip_FlipY_CheckedChanged);
            // 
            // ManiColl_Flip_FlipX
            // 
            ManiColl_Flip_FlipX.AutoSize = true;
            ManiColl_Flip_FlipX.Location = new Drawing.Point(12, 19);
            ManiColl_Flip_FlipX.Name = "ManiColl_Flip_FlipX";
            ManiColl_Flip_FlipX.Size = new Drawing.Size(74, 17);
            ManiColl_Flip_FlipX.TabIndex = 2;
            ManiColl_Flip_FlipX.Text = "Flip X-Axis";
            ManiColl_Flip_FlipX.UseVisualStyleBackColor = true;
            ManiColl_Flip_FlipX.CheckedChanged += new EventHandler(ManiColl_Flip_FlipX_CheckedChanged);
            // 
            // ManiColl_RotateGroup
            // 
            ManiColl_RotateGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ManiColl_RotateGroup.Controls.Add(ManiColl_Rotate_DegreesLabel);
            ManiColl_RotateGroup.Controls.Add(ManiColl_Rotate_DegreesValue);
            ManiColl_RotateGroup.Controls.Add(ManiColl_Rotate_SetTo0);
            ManiColl_RotateGroup.Controls.Add(ManiColl_Rotate_SetTo90);
            ManiColl_RotateGroup.Controls.Add(ManiColl_Rotate_SetTo180);
            ManiColl_RotateGroup.Controls.Add(ManiColl_Rotate_SetTo270);
            ManiColl_RotateGroup.Controls.Add(ManiColl_Rotate_SetRotationLocation);
            ManiColl_RotateGroup.Location = new Drawing.Point(4, 4);
            ManiColl_RotateGroup.Name = "ManiColl_RotateGroup";
            ManiColl_RotateGroup.Size = new Drawing.Size(218, 108);
            ManiColl_RotateGroup.TabIndex = 0;
            ManiColl_RotateGroup.TabStop = false;
            ManiColl_RotateGroup.Text = "Rotate";
            // 
            // ManiColl_Rotate_DegreesValue
            // 
            ManiColl_Rotate_DegreesValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ManiColl_Rotate_DegreesValue.DecimalPlaces = 3;
            ManiColl_Rotate_DegreesValue.Location = new Drawing.Point(145, 0);
            ManiColl_Rotate_DegreesValue.Minimum = new decimal(-1);
            ManiColl_Rotate_DegreesValue.Maximum = new decimal(new int[] {360, 0, 0, 0});
            ManiColl_Rotate_DegreesValue.Name = "ManiColl_Rotate_DegreesValue";
            ManiColl_Rotate_DegreesValue.Size = new Drawing.Size(62, 20);
            ManiColl_Rotate_DegreesValue.TabIndex = 1;
            ManiColl_Rotate_DegreesValue.ValueChanged += new EventHandler(ManiColl_Rotate_DegreesValue_ValueChanged);
            // 
            // ManiColl_Rotate_DegreesLabel
            // 
            ManiColl_Rotate_DegreesLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ManiColl_Rotate_DegreesLabel.AutoSize = true;
            ManiColl_Rotate_DegreesLabel.Location = new Drawing.Point(95, 1);
            ManiColl_Rotate_DegreesLabel.Name = "ManiColl_Rotate_DegreesLabel";
            ManiColl_Rotate_DegreesLabel.Size = new Drawing.Size(50, 13);
            ManiColl_Rotate_DegreesLabel.TabIndex = 2;
            ManiColl_Rotate_DegreesLabel.Text = "Degrees:";
            ManiColl_Rotate_DegreesLabel.BackColor = ManiColl_RotateGroup.BackColor;
            // 
            // ManiColl_Rotate_SetTo0
            // 
            ManiColl_Rotate_SetTo0.Location = new Drawing.Point(52, 20);
            ManiColl_Rotate_SetTo0.Name = "ManiColl_Rotate_SetTo0";
            ManiColl_Rotate_SetTo0.Size = new Drawing.Size(38, 23);
            ManiColl_Rotate_SetTo0.TabIndex = 1;
            ManiColl_Rotate_SetTo0.Text = "0º";
            ManiColl_Rotate_SetTo0.UseVisualStyleBackColor = true;
            ManiColl_Rotate_SetTo0.Click += new EventHandler(ManiColl_Rotate_SetTo0_Click);
            // 
            // ManiColl_Rotate_SetTo90
            // 
            ManiColl_Rotate_SetTo90.Location = new Drawing.Point(93, 46);
            ManiColl_Rotate_SetTo90.Name = "ManiColl_Rotate_SetTo90";
            ManiColl_Rotate_SetTo90.Size = new Drawing.Size(38, 23);
            ManiColl_Rotate_SetTo90.TabIndex = 1;
            ManiColl_Rotate_SetTo90.Text = "90º";
            ManiColl_Rotate_SetTo90.UseVisualStyleBackColor = true;
            ManiColl_Rotate_SetTo90.Click += new EventHandler(ManiColl_Rotate_SetTo90_Click);
            // 
            // ManiColl_Rotate_SetTo180
            // 
            ManiColl_Rotate_SetTo180.Location = new Drawing.Point(52, 72);
            ManiColl_Rotate_SetTo180.Name = "ManiColl_Rotate_SetTo180";
            ManiColl_Rotate_SetTo180.Size = new Drawing.Size(38, 23);
            ManiColl_Rotate_SetTo180.TabIndex = 1;
            ManiColl_Rotate_SetTo180.Text = "180º";
            ManiColl_Rotate_SetTo180.UseVisualStyleBackColor = true;
            ManiColl_Rotate_SetTo180.Click += new EventHandler(ManiColl_Rotate_SetTo180_Click);
            // 
            // ManiColl_Rotate_SetTo270
            // 
            ManiColl_Rotate_SetTo270.Location = new Drawing.Point(11, 46);
            ManiColl_Rotate_SetTo270.Name = "ManiColl_Rotate_SetTo270";
            ManiColl_Rotate_SetTo270.Size = new Drawing.Size(38, 23);
            ManiColl_Rotate_SetTo270.TabIndex = 1;
            ManiColl_Rotate_SetTo270.Text = "270º";
            ManiColl_Rotate_SetTo270.UseVisualStyleBackColor = true;
            ManiColl_Rotate_SetTo270.Click += new EventHandler(ManiColl_Rotate_SetTo270_Click);
            // 
            // ManiColl_Rotate_SetRotationLocation
            // 
            ManiColl_Rotate_SetRotationLocation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ManiColl_Rotate_SetRotationLocation.Location = new Drawing.Point(142, 37);
            ManiColl_Rotate_SetRotationLocation.Name = "ManiColl_Rotate_SetRotationLocation";
            ManiColl_Rotate_SetRotationLocation.Size = new Drawing.Size(66, 48);
            ManiColl_Rotate_SetRotationLocation.TabIndex = 1;
            ManiColl_Rotate_SetRotationLocation.Text = "Set Rotation Point";
            ManiColl_Rotate_SetRotationLocation.UseVisualStyleBackColor = true;
            ManiColl_Rotate_SetRotationLocation.Click += new EventHandler(ManiColl_Rotate_SetRotationLocation_Click);
            pasteOptions_tipper.SetToolTip(ManiColl_Rotate_SetRotationLocation,
                "Set a rotation point for this collision.");
            // 
            // advancedPO_TabC_AllCollsProps
            // 
            advancedPO_TabC_AllCollsProps.Controls.Add(advancedPO_TabC_AllCollsProps_Panel);
            advancedPO_TabC_AllCollsProps.Controls.Add(ACP_ResetValuesToDef);
            advancedPO_TabC_AllCollsProps.Location = new Drawing.Point(4, 22);
            advancedPO_TabC_AllCollsProps.Name = "advancedPO_TabC_AllCollsProps";
            advancedPO_TabC_AllCollsProps.Padding = new Padding(3);
            advancedPO_TabC_AllCollsProps.Size = new Drawing.Size(228, 381);
            advancedPO_TabC_AllCollsProps.TabIndex = 1;
            advancedPO_TabC_AllCollsProps.Text = "All Collision Properties";
            advancedPO_TabC_AllCollsProps.UseVisualStyleBackColor = true;
            // 
            // advancedPO_TabC_AllCollsProps_Panel
            // 
            advancedPO_TabC_AllCollsProps_Panel.AutoScroll = true;
            advancedPO_TabC_AllCollsProps_Panel.AutoScrollMargin = new Drawing.Size(0, 3);
            advancedPO_TabC_AllCollsProps_Panel.Controls.Add(AllCollsProps_WarningLink);
            advancedPO_TabC_AllCollsProps_Panel.Controls.Add(ACP_UnkFlagsGroup);
            advancedPO_TabC_AllCollsProps_Panel.Controls.Add(ACP_CollFlagsGroup);
            advancedPO_TabC_AllCollsProps_Panel.Controls.Add(ACP_TypeComboBox);
            advancedPO_TabC_AllCollsProps_Panel.Controls.Add(ACP_CollTargetsGroup);
            advancedPO_TabC_AllCollsProps_Panel.Controls.Add(ACP_MaterialComboBox);
            advancedPO_TabC_AllCollsProps_Panel.Controls.Add(ACP_Material_SelectedOverrides);
            advancedPO_TabC_AllCollsProps_Panel.Controls.Add(ACP_TypeLabel);
            advancedPO_TabC_AllCollsProps_Panel.Controls.Add(ACP_Type_SelectedOverrides);
            advancedPO_TabC_AllCollsProps_Panel.Controls.Add(ACP_MaterialLabel);
            advancedPO_TabC_AllCollsProps_Panel.Dock = DockStyle.Fill;
            advancedPO_TabC_AllCollsProps_Panel.Location = new Drawing.Point(3, 3);
            advancedPO_TabC_AllCollsProps_Panel.Name = "advancedPO_TabC_AllCollsProps_Panel";
            advancedPO_TabC_AllCollsProps_Panel.Size = new Drawing.Size(222, 352);
            advancedPO_TabC_AllCollsProps_Panel.TabIndex = 6;
            // 
            // AllCollsProps_WarningLink
            // 
            AllCollsProps_WarningLink.AutoSize = false;
            AllCollsProps_WarningLink.Dock = DockStyle.Top;
            AllCollsProps_WarningLink.Location = new Drawing.Point(0, 0);
            AllCollsProps_WarningLink.Name = "AllCollsProps_WarningLink";
            AllCollsProps_WarningLink.Size = new Drawing.Size(222, 43);
            AllCollsProps_WarningLink.TabIndex = 6;
            AllCollsProps_WarningLink.TabStop = true;
            AllCollsProps_WarningLink.Text =
                "Note: This will replace all of the collision properties that were copied. If you do not want this, press \"Reset\". More info...";
            AllCollsProps_WarningLink.LinkClicked +=
                new LinkLabelLinkClickedEventHandler(AllCollsProps_WarningLink_LinkClicked);
            // 
            // ACP_UnkFlagsGroup
            // 
            ACP_UnkFlagsGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ACP_UnkFlagsGroup.Controls.Add(ACP_UnkFlags_Unk1);
            ACP_UnkFlagsGroup.Controls.Add(ACP_UnkFlags_Unk2);
            ACP_UnkFlagsGroup.Controls.Add(ACP_UnkFlags_SuperSoft);
            ACP_UnkFlagsGroup.Controls.Add(ACP_UnkFlags_Unk4);
            ACP_UnkFlagsGroup.Controls.Add(ACP_UnkFlags_SelectedOverrides);
            ACP_UnkFlagsGroup.Location = new Drawing.Point(6, 288);
            ACP_UnkFlagsGroup.Name = "ACP_UnkFlagsGroup";
            ACP_UnkFlagsGroup.Size = new Drawing.Size(211, 58);
            ACP_UnkFlagsGroup.TabIndex = 5;
            ACP_UnkFlagsGroup.TabStop = false;
            ACP_UnkFlagsGroup.Text = "Unknown Flags";
            // 
            // ACP_UnkFlags_Unk1
            // 
            ACP_UnkFlags_Unk1.AutoSize = true;
            ACP_UnkFlags_Unk1.Location = new Drawing.Point(7, 18);
            ACP_UnkFlags_Unk1.Name = "ACP_UnkFlags_Unk1";
            ACP_UnkFlags_Unk1.Size = new Drawing.Size(81, 17);
            ACP_UnkFlags_Unk1.TabIndex = 4;
            ACP_UnkFlags_Unk1.Text = "Unknown 1";
            ACP_UnkFlags_Unk1.UseVisualStyleBackColor = true;
            ACP_UnkFlags_Unk1.CheckedChanged += new EventHandler(ACP_UnkFlags_Unk1_CheckedChanged);
            // 
            // ACP_UnkFlags_Unk2
            // 
            ACP_UnkFlags_Unk2.AutoSize = true;
            ACP_UnkFlags_Unk2.Location = new Drawing.Point(97, 18);
            ACP_UnkFlags_Unk2.Name = "ACP_UnkFlags_Unk2";
            ACP_UnkFlags_Unk2.Size = new Drawing.Size(81, 17);
            ACP_UnkFlags_Unk2.TabIndex = 4;
            ACP_UnkFlags_Unk2.Text = "Unknown 2";
            ACP_UnkFlags_Unk2.UseVisualStyleBackColor = true;
            ACP_UnkFlags_Unk2.CheckedChanged += new EventHandler(ACP_UnkFlags_Unk2_CheckedChanged);
            // 
            // ACP_UnkFlags_SuperSoft
            // 
            ACP_UnkFlags_SuperSoft.AutoSize = true;
            ACP_UnkFlags_SuperSoft.Location = new Drawing.Point(7, 36);
            ACP_UnkFlags_SuperSoft.Name = "ACP_UnkFlags_SuperSoft";
            ACP_UnkFlags_SuperSoft.Size = new Drawing.Size(81, 17);
            ACP_UnkFlags_SuperSoft.TabIndex = 4;
            ACP_UnkFlags_SuperSoft.Text = "Super Soft";
            ACP_UnkFlags_SuperSoft.UseVisualStyleBackColor = true;
            ACP_UnkFlags_SuperSoft.CheckedChanged += new EventHandler(ACP_UnkFlags_SuperSoft_CheckedChanged);
            // 
            // ACP_UnkFlags_Unk4
            // 
            ACP_UnkFlags_Unk4.AutoSize = true;
            ACP_UnkFlags_Unk4.Location = new Drawing.Point(97, 36);
            ACP_UnkFlags_Unk4.Name = "ACP_UnkFlags_Unk4";
            ACP_UnkFlags_Unk4.Size = new Drawing.Size(81, 17);
            ACP_UnkFlags_Unk4.TabIndex = 4;
            ACP_UnkFlags_Unk4.Text = "Unknown 4";
            ACP_UnkFlags_Unk4.UseVisualStyleBackColor = true;
            ACP_UnkFlags_Unk4.CheckedChanged += new EventHandler(ACP_UnkFlags_Unk4_CheckedChanged);
            // 
            // ACP_UnkFlags_SelectedOverrides
            // 
            ACP_UnkFlags_SelectedOverrides.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ACP_UnkFlags_SelectedOverrides.AutoSize = true;
            ACP_UnkFlags_SelectedOverrides.Checked = false;
            ACP_UnkFlags_SelectedOverrides.CheckState = CheckState.Unchecked;
            ACP_UnkFlags_SelectedOverrides.Location = new Drawing.Point(191, 0);
            ACP_UnkFlags_SelectedOverrides.Name = "ACP_UnkFlags_SelectedOverrides";
            ACP_UnkFlags_SelectedOverrides.Size = new Drawing.Size(15, 14);
            ACP_UnkFlags_SelectedOverrides.TabIndex = 4;
            ACP_UnkFlags_SelectedOverrides.UseVisualStyleBackColor = true;
            ACP_UnkFlags_SelectedOverrides.CheckedChanged +=
                new EventHandler(ACP_UnkFlags_SelectedOverrides_CheckedChanged);
            pasteOptions_tipper.SetToolTip(ACP_UnkFlags_SelectedOverrides,
                "If checked, selected/nonselected unknown values will override all copied properties\' unknown values.");
            // 
            // ACP_CollFlagsGroup
            // 
            ACP_CollFlagsGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ACP_CollFlagsGroup.Controls.Add(ACP_CollFlags_NoWallJump);
            ACP_CollFlagsGroup.Controls.Add(ACP_CollFlags_Rotating);
            ACP_CollFlagsGroup.Controls.Add(ACP_CollFlags_LeftLedge);
            ACP_CollFlagsGroup.Controls.Add(ACP_CollFlags_RightLedge);
            ACP_CollFlagsGroup.Controls.Add(ACP_CollFlags_FallThrough);
            ACP_CollFlagsGroup.Controls.Add(ACP_CollFlags_SelectedOverrides);
            ACP_CollFlagsGroup.Location = new Drawing.Point(6, 205);
            ACP_CollFlagsGroup.Name = "ACP_CollFlagsGroup";
            ACP_CollFlagsGroup.Size = new Drawing.Size(211, 77);
            ACP_CollFlagsGroup.TabIndex = 5;
            ACP_CollFlagsGroup.TabStop = false;
            ACP_CollFlagsGroup.Text = "Collision Flags";
            // 
            // ACP_CollFlags_NoWallJump
            // 
            ACP_CollFlags_NoWallJump.AutoSize = true;
            ACP_CollFlags_NoWallJump.Location = new Drawing.Point(97, 37);
            ACP_CollFlags_NoWallJump.Name = "ACP_CollFlags_NoWallJump";
            ACP_CollFlags_NoWallJump.Size = new Drawing.Size(86, 17);
            ACP_CollFlags_NoWallJump.TabIndex = 4;
            ACP_CollFlags_NoWallJump.Text = "No Walljump";
            ACP_CollFlags_NoWallJump.UseVisualStyleBackColor = true;
            ACP_CollFlags_NoWallJump.CheckedChanged += new EventHandler(ACP_CollFlags_NoWallJump_CheckedChanged);
            // 
            // ACP_CollFlags_Rotating
            // 
            ACP_CollFlags_Rotating.AutoSize = true;
            ACP_CollFlags_Rotating.Location = new Drawing.Point(6, 55);
            ACP_CollFlags_Rotating.Name = "ACP_CollFlags_Rotating";
            ACP_CollFlags_Rotating.Size = new Drawing.Size(66, 17);
            ACP_CollFlags_Rotating.TabIndex = 4;
            ACP_CollFlags_Rotating.Text = "Rotating";
            ACP_CollFlags_Rotating.UseVisualStyleBackColor = true;
            ACP_CollFlags_Rotating.CheckedChanged += new EventHandler(ACP_CollFlags_Rotating_CheckedChanged);
            // 
            // ACP_CollFlags_LeftLedge
            // 
            ACP_CollFlags_LeftLedge.AutoSize = true;
            ACP_CollFlags_LeftLedge.Location = new Drawing.Point(97, 19);
            ACP_CollFlags_LeftLedge.Name = "ACP_CollFlags_LeftLedge";
            ACP_CollFlags_LeftLedge.Size = new Drawing.Size(77, 17);
            ACP_CollFlags_LeftLedge.TabIndex = 4;
            ACP_CollFlags_LeftLedge.Text = "Left Ledge";
            ACP_CollFlags_LeftLedge.UseVisualStyleBackColor = true;
            ACP_CollFlags_LeftLedge.CheckedChanged += new EventHandler(ACP_CollFlags_LeftLedge_CheckedChanged);
            // 
            // ACP_CollFlags_RightLedge
            // 
            ACP_CollFlags_RightLedge.AutoSize = true;
            ACP_CollFlags_RightLedge.Location = new Drawing.Point(6, 37);
            ACP_CollFlags_RightLedge.Name = "ACP_CollFlags_RightLedge";
            ACP_CollFlags_RightLedge.Size = new Drawing.Size(84, 17);
            ACP_CollFlags_RightLedge.TabIndex = 4;
            ACP_CollFlags_RightLedge.Text = "Right Ledge";
            ACP_CollFlags_RightLedge.UseVisualStyleBackColor = true;
            ACP_CollFlags_RightLedge.CheckedChanged += new EventHandler(ACP_CollFlags_RightLedge_CheckedChanged);
            // 
            // ACP_CollFlags_FallThrough
            // 
            ACP_CollFlags_FallThrough.AutoSize = true;
            ACP_CollFlags_FallThrough.Location = new Drawing.Point(6, 19);
            ACP_CollFlags_FallThrough.Name = "ACP_CollFlags_FallThrough";
            ACP_CollFlags_FallThrough.Size = new Drawing.Size(85, 17);
            ACP_CollFlags_FallThrough.TabIndex = 4;
            ACP_CollFlags_FallThrough.Text = "Fall Through";
            ACP_CollFlags_FallThrough.UseVisualStyleBackColor = true;
            ACP_CollFlags_FallThrough.CheckedChanged += new EventHandler(ACP_CollFlags_FallThrough_CheckedChanged);
            // 
            // ACP_CollFlags_SelectedOverrides
            // 
            ACP_CollFlags_SelectedOverrides.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ACP_CollFlags_SelectedOverrides.AutoSize = true;
            ACP_CollFlags_SelectedOverrides.Checked = false;
            ACP_CollFlags_SelectedOverrides.CheckState = CheckState.Unchecked;
            ACP_CollFlags_SelectedOverrides.Location = new Drawing.Point(191, 1);
            ACP_CollFlags_SelectedOverrides.Name = "ACP_CollFlags_SelectedOverrides";
            ACP_CollFlags_SelectedOverrides.Size = new Drawing.Size(15, 14);
            ACP_CollFlags_SelectedOverrides.TabIndex = 4;
            ACP_CollFlags_SelectedOverrides.UseVisualStyleBackColor = true;
            ACP_CollFlags_SelectedOverrides.CheckedChanged +=
                new EventHandler(ACP_CollFlags_SelectedOverrides_CheckedChanged);
            pasteOptions_tipper.SetToolTip(ACP_CollFlags_SelectedOverrides,
                "If checked, selected/nonselected collision flags will override all copied properties\' collision flags.");
            // 
            // ACP_Type_SelectedOverrides
            // 
            ACP_Type_SelectedOverrides.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ACP_Type_SelectedOverrides.AutoSize = true;
            ACP_Type_SelectedOverrides.Checked = false;
            ACP_Type_SelectedOverrides.CheckState = CheckState.Unchecked;
            ACP_Type_SelectedOverrides.Location = new Drawing.Point(202, 70);
            ACP_Type_SelectedOverrides.Name = "ACP_Type_SelectedOverrides";
            ACP_Type_SelectedOverrides.Size = new Drawing.Size(15, 14);
            ACP_Type_SelectedOverrides.TabIndex = 4;
            ACP_Type_SelectedOverrides.UseVisualStyleBackColor = true;
            ACP_Type_SelectedOverrides.CheckedChanged += new EventHandler(ACP_Type_SelectedOverrides_CheckedChanged);
            pasteOptions_tipper.SetToolTip(ACP_Type_SelectedOverrides,
                "If checked, the selected type will override all copied properties\' type.");
            // 
            // ACP_TypeComboBox
            // 
            ACP_TypeComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ACP_TypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ACP_TypeComboBox.FormattingEnabled = true;
            ACP_TypeComboBox.Location = new Drawing.Point(6, 67);
            ACP_TypeComboBox.Name = "ACP_TypeComboBox";
            ACP_TypeComboBox.Size = new Drawing.Size(190, 21);
            ACP_TypeComboBox.TabIndex = 2;
            ACP_TypeComboBox.SelectedIndexChanged += new EventHandler(ACP_TypeComboBox_SelectedIndexChanged);
            // 
            // ACP_Material_SelectedOverrides
            // 
            ACP_Material_SelectedOverrides.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ACP_Material_SelectedOverrides.AutoSize = true;
            ACP_Material_SelectedOverrides.Checked = false;
            ACP_Material_SelectedOverrides.CheckState = CheckState.Unchecked;
            ACP_Material_SelectedOverrides.Location = new Drawing.Point(202, 113);
            ACP_Material_SelectedOverrides.Name = "ACP_Material_SelectedOverrides";
            ACP_Material_SelectedOverrides.Size = new Drawing.Size(15, 14);
            ACP_Material_SelectedOverrides.TabIndex = 4;
            ACP_Material_SelectedOverrides.UseVisualStyleBackColor = true;
            ACP_Material_SelectedOverrides.CheckedChanged +=
                new EventHandler(ACP_Material_SelectedOverrides_CheckedChanged);
            pasteOptions_tipper.SetToolTip(ACP_Material_SelectedOverrides,
                "If checked, the selected material will override all copied properties\' material.");
            // 
            // ACP_MaterialComboBox
            // 
            ACP_MaterialComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ACP_MaterialComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ACP_MaterialComboBox.FormattingEnabled = true;
            ACP_MaterialComboBox.Location = new Drawing.Point(6, 110);
            ACP_MaterialComboBox.Name = "ACP_MaterialComboBox";
            ACP_MaterialComboBox.Size = new Drawing.Size(190, 21);
            ACP_MaterialComboBox.TabIndex = 2;
            ACP_MaterialComboBox.SelectedIndexChanged += new EventHandler(ACP_MaterialComboBox_SelectedIndexChanged);
            // 
            // ACP_CollTargetsGroup
            // 
            ACP_CollTargetsGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ACP_CollTargetsGroup.Controls.Add(ACP_CollTargets_Items);
            ACP_CollTargetsGroup.Controls.Add(ACP_CollTargets_PKMNTrainer);
            ACP_CollTargetsGroup.Controls.Add(ACP_CollTargets_Everything);
            ACP_CollTargetsGroup.Controls.Add(ACP_CollTargets_SelectedOverrides);
            ACP_CollTargetsGroup.Location = new Drawing.Point(6, 139);
            ACP_CollTargetsGroup.Name = "ACP_CollTargetsGroup";
            ACP_CollTargetsGroup.Size = new Drawing.Size(211, 60);
            ACP_CollTargetsGroup.TabIndex = 5;
            ACP_CollTargetsGroup.TabStop = false;
            ACP_CollTargetsGroup.Text = "Collision Targets?";
            // 
            // ACP_CollTargets_SelectedOverrides
            // 
            ACP_CollTargets_SelectedOverrides.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ACP_CollTargets_SelectedOverrides.AutoSize = true;
            ACP_CollTargets_SelectedOverrides.Checked = false;
            ACP_CollTargets_SelectedOverrides.CheckState = CheckState.Unchecked;
            ACP_CollTargets_SelectedOverrides.Location = new Drawing.Point(191, 0);
            ACP_CollTargets_SelectedOverrides.Name = "ACP_CollTargets_SelectedOverrides";
            ACP_CollTargets_SelectedOverrides.Size = new Drawing.Size(15, 14);
            ACP_CollTargets_SelectedOverrides.TabIndex = 4;
            ACP_CollTargets_SelectedOverrides.UseVisualStyleBackColor = true;
            ACP_CollTargets_SelectedOverrides.CheckedChanged +=
                new EventHandler(ACP_CollTargets_SelectedOverrides_CheckedChanged);
            pasteOptions_tipper.SetToolTip(ACP_CollTargets_SelectedOverrides,
                "If checked, the selected collision targets will override all copied properties\' collision targets.");
            // 
            // ACP_CollTargets_Everything
            // 
            ACP_CollTargets_Everything.AutoSize = true;
            ACP_CollTargets_Everything.Location = new Drawing.Point(6, 19);
            ACP_CollTargets_Everything.Name = "ACP_CollTargets_Everything";
            ACP_CollTargets_Everything.Size = new Drawing.Size(76, 17);
            ACP_CollTargets_Everything.TabIndex = 4;
            ACP_CollTargets_Everything.Text = "Everything";
            ACP_CollTargets_Everything.UseVisualStyleBackColor = true;
            ACP_CollTargets_Everything.CheckedChanged += new EventHandler(ACP_CollTargets_Everything_CheckedChanged);
            // 
            // ACP_CollTargets_Items
            // 
            ACP_CollTargets_Items.AutoSize = true;
            ACP_CollTargets_Items.Location = new Drawing.Point(113, 19);
            ACP_CollTargets_Items.Name = "ACP_CollTargets_Items";
            ACP_CollTargets_Items.Size = new Drawing.Size(51, 17);
            ACP_CollTargets_Items.TabIndex = 4;
            ACP_CollTargets_Items.Text = "Items";
            ACP_CollTargets_Items.UseVisualStyleBackColor = true;
            ACP_CollTargets_Items.CheckedChanged += new EventHandler(ACP_CollTargets_Items_CheckedChanged);
            // 
            // ACP_CollTargets_PKMNTrainer
            // 
            ACP_CollTargets_PKMNTrainer.AutoSize = true;
            ACP_CollTargets_PKMNTrainer.Location = new Drawing.Point(6, 38);
            ACP_CollTargets_PKMNTrainer.Name = "ACP_CollTargets_PKMNTrainer";
            ACP_CollTargets_PKMNTrainer.Size = new Drawing.Size(107, 17);
            ACP_CollTargets_PKMNTrainer.TabIndex = 4;
            ACP_CollTargets_PKMNTrainer.Text = "Pokémon Trainer";
            ACP_CollTargets_PKMNTrainer.UseVisualStyleBackColor = true;
            ACP_CollTargets_PKMNTrainer.CheckedChanged += new EventHandler(ACP_CollTargets_PKMNTrainer_CheckedChanged);
            // 
            // ACP_TypeLabel
            // 
            ACP_TypeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ACP_TypeLabel.Location = new Drawing.Point(6, 51);
            ACP_TypeLabel.Name = "ACP_TypeLabel";
            ACP_TypeLabel.Size = new Drawing.Size(206, 13);
            ACP_TypeLabel.TabIndex = 3;
            ACP_TypeLabel.Text = "Type";
            // 
            // ACP_MaterialLabel
            // 
            ACP_MaterialLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ACP_MaterialLabel.Location = new Drawing.Point(6, 94);
            ACP_MaterialLabel.Name = "ACP_MaterialLabel";
            ACP_MaterialLabel.Size = new Drawing.Size(206, 13);
            ACP_MaterialLabel.TabIndex = 3;
            ACP_MaterialLabel.Text = "Material";
            // 
            // ACP_ResetValuesToDef
            // 
            ACP_ResetValuesToDef.Dock = DockStyle.Bottom;
            ACP_ResetValuesToDef.Enabled = false;
            ACP_ResetValuesToDef.Location = new Drawing.Point(3, 355);
            ACP_ResetValuesToDef.Name = "ACP_ResetValuesToDef";
            ACP_ResetValuesToDef.Size = new Drawing.Size(222, 23);
            ACP_ResetValuesToDef.TabIndex = 1;
            ACP_ResetValuesToDef.Text = "Reset";
            ACP_ResetValuesToDef.UseVisualStyleBackColor = true;
            ACP_ResetValuesToDef.Click += new EventHandler(ACP_ResetValuesToDef_Click);
            // 
            // BottomPanel
            // 
            BottomPanel.Controls.Add(Cancel);
            BottomPanel.Controls.Add(PasteCollision);
            BottomPanel.Dock = DockStyle.Bottom;
            BottomPanel.Location = new Drawing.Point(0, 407);
            BottomPanel.Name = "BottomPanel";
            BottomPanel.Size = new Drawing.Size(236, 32);
            BottomPanel.TabIndex = 1;
            // 
            // PasteCollision
            // 
            PasteCollision.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            PasteCollision.Location = new Drawing.Point(49, 4);
            PasteCollision.Name = "PasteCollision";
            PasteCollision.Size = new Drawing.Size(104, 23);
            PasteCollision.TabIndex = 0;
            PasteCollision.Text = "Paste Collision";
            PasteCollision.UseVisualStyleBackColor = true;
            PasteCollision.Click += new EventHandler(PasteCollision_Click);
            // 
            // Cancel
            // 
            Cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Cancel.DialogResult = DialogResult.Cancel;
            Cancel.Location = new Drawing.Point(156, 4);
            Cancel.Name = "Cancel";
            Cancel.Size = new Drawing.Size(75, 23);
            Cancel.TabIndex = 1;
            Cancel.Text = "Cancel";
            Cancel.UseVisualStyleBackColor = true;
            Cancel.Click += new EventHandler(Cancel_Click);
            // 
            // BrawlCrate_PasteOptions_UI
            // 
            Name = "CollisionEditor_PasteOptions";
            Text = "Advanced Paste Options";
            CancelButton = Cancel;
            AutoScaleMode = AutoScaleMode.Font;
            AutoScaleDimensions = new SizeF(6F, 13F);
            ClientSize = new Drawing.Size(236, 439);
            MaximumSize = new Drawing.Size(252, 478);
            MinimumSize = new Drawing.Size(252, 252);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Controls.Add(advancedPO_TabC);
            Controls.Add(BottomPanel);
            Icon = BrawlLib.Properties.Resources.Icon;

            advancedPO_TabC.ResumeLayout(false);
            advancedPO_TabC_ManipulateCollisionTab.ResumeLayout(false);
            ManiColl_PlacementGroup.ResumeLayout(false);
            ManiColl_PlacementGroup.PerformLayout();
            ManiColl_ScaleGroup.ResumeLayout(false);
            ManiColl_ScaleGroup.PerformLayout();
            ((ISupportInitialize) ManiColl_Scale_ScalarValue).EndInit();
            ManiColl_FlipGroup.ResumeLayout(false);
            ManiColl_FlipGroup.PerformLayout();
            ManiColl_RotateGroup.ResumeLayout(false);
            ManiColl_RotateGroup.PerformLayout();
            ((ISupportInitialize) ManiColl_Rotate_DegreesValue).EndInit();
            advancedPO_TabC_AllCollsProps.ResumeLayout(false);
            advancedPO_TabC_AllCollsProps_Panel.ResumeLayout(false);
            advancedPO_TabC_AllCollsProps_Panel.PerformLayout();
            ACP_UnkFlagsGroup.ResumeLayout(false);
            ACP_UnkFlagsGroup.PerformLayout();
            ACP_CollFlagsGroup.ResumeLayout(false);
            ACP_CollFlagsGroup.PerformLayout();
            ACP_CollTargetsGroup.ResumeLayout(false);
            ACP_CollTargetsGroup.PerformLayout();
            BottomPanel.ResumeLayout(false);

            ResumeLayout(false);
        }

        #endregion

        // The main parent menu in which it will use.
        private CollisionEditor parentEditor = null;

        public bool OnFocus { get; private set; } = false;

        public CopiedLinkPlaneState copiedState;
        public CollisionObject tinkeringObject { get; private set; }

        private float[] OriginalLinksDistance;
        private float[] OriginalLinksRadians;
        private float[] LinksDistance;
        private float[] LinksRadians;
        private float CurrentRadian = 0;
        private float CurrentDistance = 0;

        private Vector2 CenterRotationPoint = new Vector2(0, 0);
        private Vector2 OffsetLocation = new Vector2(0, 0);

        public bool SetPointMode = false;

        // 0 = None, 1 = Set Center Point, 
        // 2 = Set Location (Moves the collision location to where the user wants it to go)
        public byte PointModeType = 0;

        // 0 = Specific Placement, 1 = Free Placement, 
        // 2 = Zero Zero (0, 0)
        private byte SetRotationLocation_RadioSelectedValue = 2;

        // Creates new points with location modified and scaling applied.
        public Vector2[] CopiedLinkPointsNewLocation = null;

        // A plane used with extra data so that CollisionEditor class can check what
        // values this plane has.
        public CollisionPlane_S AllCollisionPropertiesValues;

        // A variable that checks if something is being updated so that others that do get
        // updated do not get updated.
        private bool Updating = false;

        private CollisionEditor_PasteOptions_SetLocationForm LocationForm;

        public CollisionEditor_PasteOptions(CollisionEditor ce, CopiedLinkPlaneState target)
        {
            parentEditor = ce;
            copiedState = target;

            InitializeComponent();

            ACP_MaterialComboBox.DataSource = ce.getMaterials();
            ACP_TypeComboBox.DataSource = ce.getCollisionPlaneTypes();

            // Create a AllCollisionProperties class.
            ResetACPValues();
        }

        private void PasteCollision_Click(object sender, EventArgs e)
        {
            if (parentEditor.PasteCopiedCollisions(true))
            {
                Close();
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        // This will be called everytime this dialog is loaded, or the center point
        // has been changed for rotation and scale.
        private void BuildCollisionObject()
        {
            tinkeringObject = new CollisionObject();
            tinkeringObject.CollisionObjectColorRepresentation = Color.FromArgb(200, Color.Pink);

            copiedState.CreateLinksAndPlanes(tinkeringObject, false);
            copiedState.ClearLinksAndPlanes();

            CreateRadianAndRadius();
        }

        #region Link/Point Radian and Radius (Distance)

        // This takes all our copied links and calculate from the center point. 
        // It helps us in the rotation and scale system.
        private void CreateRadianAndRadius()
        {
            int size = copiedState.CopiedLinks.Length;

            OriginalLinksDistance = new float[size];
            OriginalLinksRadians = new float[size];

            for (int d = size - 1; d >= 0; --d)
            {
                CollisionLink_S point = copiedState.CopiedLinks[d];

                // Start by calculating our point's location minus center point's location.
                Vector2 ZeroPoint =
                    (parentEditor.clipboardPasteOptions_ActualPointsValuesAreUsed.Checked
                        ? point.Value
                        : point.RawValue) - CenterRotationPoint;

                // Then start getting the distance.
                OriginalLinksDistance[d] = (float) Math.Sqrt(ZeroPoint._x * ZeroPoint._x + ZeroPoint._y * ZeroPoint._y);
                // This is an alternative, just wondering if this really makes any difference.
                //OriginalLinksDistance[d] = CenterPoint.TrueDistance((parentEditor.clipboardPasteOptions_ActualPointsValuesAreUsed.Checked ? point.Value : point.RawValue));

                // Then we get our radians.
                OriginalLinksRadians[d] = (float) Math.Atan2(ZeroPoint._y, ZeroPoint._x);
            }

            // Prevent references by cloning both original points distance and radians.
            LinksDistance = (float[]) OriginalLinksDistance.Clone();
            LinksRadians = (float[]) OriginalLinksRadians.Clone();

            UpdateLinksValues();
        }

        private void UpdateLinksRadians(double radians)
        {
            CurrentRadian = (float) radians;

            for (int r = copiedState.CopiedLinks.Length - 1; r >= 0; --r)
            {
                LinksRadians[r] = OriginalLinksRadians[r] - (float) radians;

                float X = (float) Math.Cos(LinksRadians[r]) * LinksDistance[r];
                float Y = (float) Math.Sin(LinksRadians[r]) * LinksDistance[r];
            }

            Updating = true;
            double degrees = RadiansToDegrees(radians);

            if (degrees < 0)
            {
                degrees = 360.0 + degrees;
            }
            else if (degrees >= 360)
            {
                degrees = degrees - 360;
            }

            ManiColl_Rotate_DegreesValue.Value = (decimal) degrees;
            Updating = false;

            UpdateLinksValues();
        }

        private void UpdateLinksDistance(double distance, bool scale)
        {
            if (!scale)
            {
                CurrentDistance = (float) distance;
            }

            for (int r = copiedState.CopiedLinks.Length - 1; r >= 0; --r)
            {
                if (scale)
                {
                    LinksDistance[r] = OriginalLinksDistance[r] * (float) distance;
                }
                else
                {
                    LinksDistance[r] = OriginalLinksDistance[r] + (float) distance;
                }
            }

            if (scale)
            {
                Updating = true;
                ManiColl_Scale_ScalarValue.Value = (decimal) distance;
                Updating = false;
            }

            UpdateLinksValues();
        }

        private void UpdateLinksValues()
        {
            int length = copiedState.CopiedLinks.Length;

            CopiedLinkPointsNewLocation = new Vector2[length];

            for (int r = length - 1; r >= 0; --r)
            {
                float X = (float) Math.Cos(LinksRadians[r]) * LinksDistance[r];
                float Y = (float) Math.Sin(LinksRadians[r]) * LinksDistance[r];

                if (ManiColl_Flip_FlipX.Checked)
                {
                    X = -X;
                }

                if (ManiColl_Flip_FlipY.Checked)
                {
                    Y = -Y;
                }

                Vector2 v = CenterRotationPoint + OffsetLocation + new Vector2(X, Y);
                CopiedLinkPointsNewLocation[r] = v;
            }

            if (length > tinkeringObject._points.Count)
            {
                length = tinkeringObject._points.Count;
            }

            for (int r = length - 1; r >= 0; --r)
            {
                tinkeringObject._points[r].Value = CopiedLinkPointsNewLocation[r];
            }


            parentEditor._modelPanel.Invalidate();
        }

        #endregion

        #region Collision Editor To Paste Options

        public void Render(ModelPanelViewport Viewport)
        {
            GLCamera cam = Viewport.Camera;

            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            CollisionObject.CollisionObjectRenderInfo re =
                new CollisionObject.CollisionObjectRenderInfo(false, OnFocus, ref cam);
            tinkeringObject.Render(re);

            // Draws the center point of Paste Options.
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            //Color4 c4f = CollisionEditor.convertTo4(tinkeringObject.CollisionObjectColorRepresentation);
            //GL.Color4(c4f.R, c4f.G, c4f.B, c4f.A);
            GL.Color4(tinkeringObject.CollisionObjectColorRepresentation);

            //Draws the centered location combined with the offset location.
            float x = CenterRotationPoint._x + OffsetLocation._x;
            float y = CenterRotationPoint._x + OffsetLocation._y;

            TKContext.DrawBox(
                new Vector3(x - 1.0f, y - 1.0f, -3.0f),
                new Vector3(x + 1.0f, y + 1.0f, 3.0f));
        }

        public void CustomUserPointSet(ModelPanelViewport Viewport, Drawing.Point Location, float Depth)
        {
            if (LocationForm != null && PointModeType == 1)
            {
                LocationForm.CustomUserPointSet(Viewport, Location, Depth);
                return;
            }

            Vector3 Point3D = Viewport.UnProject(Location.X, Location.Y, -Depth);

            // Trace ray to Z axis
            Point3D = Vector3.IntersectZ(Point3D, Viewport.UnProject(Location.X, Location.Y, 0.0f), 0.0f);
            Vector2 Point = (Vector2) Point3D;

            switch (PointModeType)
            {
                // Sets the center point for rotation which is already handled by LocationForm.
                case 1: break;

                // Sets the center point for location.
                case 2:
                    OffsetLocation = Point;
                    break;

                default: break;
            }

            if (PointModeType > 0)
            {
                BuildCollisionObject();

                UpdateLinksRadians(CurrentRadian);
                UpdateLinksValues();
            }

            Visible = Enabled = true;
            SetPointMode = false;
            PointModeType = 0;

            if (LocationForm != null)
            {
                LocationForm.Visible = LocationForm.Enabled = true;
            }
        }

        #endregion


        #region Form Events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CenterRotationPoint = new Vector2(0, 0);

            BuildCollisionObject();
            parentEditor._modelPanel.Invalidate();
        }

        protected override void OnActivated(EventArgs e)
        {
            OnFocus = true;
            base.OnActivated(e);

            // Set the window opacity full if active (focused).
            // It only takes percentages or 1.0 (100%) as maximum.
            Opacity = 1.0f;

            parentEditor._modelPanel.Invalidate();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            OnFocus = false;
            base.OnDeactivate(e);

            // Set the window opacity 60% if not active (focused).
            if (!IsDisposed && !FormIsClosing)
            {
                Opacity = 0.6f;
            }
        }

        private bool FormIsClosing = false;

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);


            if (LocationForm != null)
            {
                LocationForm.Close();
            }

            FormIsClosing = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            parentEditor._modelPanel.Invalidate();
            parentEditor.NullifyAdvancedPasteOptions();

            base.OnClosed(e);
        }

        #endregion

        #region Location Form - Read and Write

        private void NullifyLocationForm()
        {
            if (LocationForm == null)
            {
                return;
            }

            LocationForm.Dispose();
            LocationForm = null;
        }

        //private void SetLocationFromLocationForm()
        //{
        //	UpdateLocationFromLocationForm(LocationForm.CurrentLocation, );
        //}
        // Called everytime the location form performs an update to the location.
        // This is not limited by when Specific Location updates or by free location
        // itself when a point is set from the user, it also updates when the user
        // presses "Set Location".
        private void UpdateLocationFromLocationForm(Vector2 Location, byte PointModeInUse)
        {
            switch (PointModeInUse)
            {
                case 1:
                    CenterRotationPoint = Location;
                    break;
            }

            if (PointModeInUse > 0)
            {
                BuildCollisionObject();

                UpdateLinksRadians(CurrentRadian);
                UpdateLinksValues();
            }
        }

        #endregion

        #region Manipulate Collision Tab

        #region Rotate Section

        private void ManiColl_Rotate_DegreesValue_ValueChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            UpdateLinksRadians(DegreesToRadians((double) ManiColl_Rotate_DegreesValue.Value));
        }

        private void ManiColl_Rotate_SetTo0_Click(object sender, EventArgs e)
        {
            UpdateLinksRadians(DegreesToRadians(0));
        }

        private void ManiColl_Rotate_SetTo90_Click(object sender, EventArgs e)
        {
            UpdateLinksRadians(DegreesToRadians(90));
        }

        private void ManiColl_Rotate_SetTo180_Click(object sender, EventArgs e)
        {
            UpdateLinksRadians(DegreesToRadians(180));
        }

        private void ManiColl_Rotate_SetTo270_Click(object sender, EventArgs e)
        {
            UpdateLinksRadians(DegreesToRadians(270));
        }

        // This is quite misleading-- Setting center point NOW affects rotation AND distance.
        private void ManiColl_Rotate_SetRotationLocation_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Setting a center point is buggy!");

            if (LocationForm == null)
            {
                LocationForm = new CollisionEditor_PasteOptions_SetLocationForm(this,
                    SetRotationLocation_RadioSelectedValue, CenterRotationPoint);
                LocationForm.Show();

                LocationForm.PointMode = 1;
            }

            LocationForm.BringToFront();
        }

        #endregion

        #region Flip Section

        private void ManiColl_Flip_FlipX_CheckedChanged(object sender, EventArgs e)
        {
            UpdateLinksValues();
        }

        private void ManiColl_Flip_FlipY_CheckedChanged(object sender, EventArgs e)
        {
            UpdateLinksValues();
        }

        #endregion

        #region Scale Section

        private void ManiColl_Scale_ScalarValue_ValueChanged(object sender, EventArgs e)
        {
            if (!Updating)
            {
                UpdateLinksDistance((double) ManiColl_Scale_ScalarValue.Value, true);
            }
        }

        private void ManiColl_Scale_ResetScale_Click(object sender, EventArgs e)
        {
            UpdateLinksDistance(1, true);
        }

        #endregion

        #region Location/Placement

        private void ManiColl_Placement_RadioZeroZero_CheckedChanged(object sender, EventArgs e)
        {
            if (ManiColl_Placement_RadioZeroZero.Checked)
            {
                OffsetLocation = new Vector2(0, 0);
                UpdateLinksValues();
            }
        }

        private void ManiColl_Placement_RadioFreeplace_CheckedChanged(object sender, EventArgs e)
        {
            if (ManiColl_Placement_RadioFreeplace.Checked)
            {
                SetPointMode = true;
                PointModeType = 2;

                if (LocationForm != null)
                {
                    LocationForm.Visible = LocationForm.Enabled = false;
                }

                Visible = Enabled = false;
            }
        }

        private void ManiColl_Placement_RadioSpefPlacement_CheckedChanged(object sender, EventArgs e)
        {
            ManiColl_Placement_SpefPlacement_XLabel.Enabled = ManiColl_Placement_SpefPlacement_YLabel.Enabled =
                ManiColl_Placement_SpefPlacement_LocX.Enabled = ManiColl_Placement_SpefPlacement_LocY.Enabled =
                    ManiColl_Placement_RadioSpefPlacement.Checked;

            if (ManiColl_Placement_RadioSpefPlacement.Checked)
            {
                OffsetLocation = new Vector2(ManiColl_Placement_SpefPlacement_LocX.Value,
                    ManiColl_Placement_SpefPlacement_LocY.Value);
                UpdateLinksValues();
            }
        }

        private void ManiColl_Placement_SpefPlacement_LocX_ValueChanged(object sender, EventArgs e)
        {
            OffsetLocation = new Vector2(ManiColl_Placement_SpefPlacement_LocX.Value,
                ManiColl_Placement_SpefPlacement_LocY.Value);
            UpdateLinksValues();
        }

        private void ManiColl_Placement_SpefPlacement_LocY_ValueChanged(object sender, EventArgs e)
        {
            OffsetLocation = new Vector2(ManiColl_Placement_SpefPlacement_LocX.Value,
                ManiColl_Placement_SpefPlacement_LocY.Value);
            UpdateLinksValues();
        }

        #endregion

        #endregion

        #region All Collisions Properties Tab

        #region General

        private void AllCollsProps_WarningLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(
                "If you check/hover over the checkboxes that do not have a text (are on the group boxes or next to them) " +
                "then you are allowing Advanced Paste Options to override all of the collision planes's properties in that area.\n\n" +
                "For example, if you check \"Collision Targets\" empty checkbox, then what happens is that once pasted, " +
                "all of the new planes collision targets gets replaced with what was selected, even if they are not " +
                "checked."
                , string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ACP_ResetValuesToDef_Click(object sender, EventArgs e)
        {
            ResetACPValues();

            Updating = true;

            ACP_TypeComboBox.SelectedIndex = 0;
            ACP_MaterialComboBox.SelectedIndex = 0;

            ACP_Type_SelectedOverrides.Checked =
                ACP_Material_SelectedOverrides.Checked =
                    ACP_CollTargets_SelectedOverrides.Checked =
                        ACP_CollFlags_SelectedOverrides.Checked =
                            ACP_UnkFlags_SelectedOverrides.Checked =
                                ACP_CollTargets_Everything.Checked =
                                    ACP_CollTargets_Items.Checked =
                                        ACP_CollTargets_PKMNTrainer.Checked =
                                            ACP_CollFlags_LeftLedge.Checked =
                                                ACP_CollFlags_RightLedge.Checked =
                                                    ACP_CollFlags_FallThrough.Checked =
                                                        ACP_CollFlags_NoWallJump.Checked =
                                                            ACP_CollFlags_Rotating.Checked =
                                                                ACP_UnkFlags_Unk1.Checked =
                                                                    ACP_UnkFlags_Unk2.Checked =
                                                                        ACP_UnkFlags_SuperSoft.Checked =
                                                                            ACP_UnkFlags_Unk4.Checked =
                                                                                false;

            Updating = false;

            ACP_ResetValuesToDef.Enabled = false;
        }

        private void ACP_TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Updating = true;

            CollisionPlaneType PlaneType = (CollisionPlaneType) ACP_TypeComboBox.SelectedItem;
            AllCollisionPropertiesValues.Type = PlaneType;

            if (PlaneType != CollisionPlaneType.Floor)
            {
                ACP_CollFlags_FallThrough.Checked = false;
                ACP_CollFlags_RightLedge.Checked = false;
                ACP_CollFlags_LeftLedge.Checked = false;
            }

            if (PlaneType != CollisionPlaneType.LeftWall || PlaneType != CollisionPlaneType.RightWall)
            {
                ACP_CollFlags_NoWallJump.Checked = false;
            }

            Updating = false;
            EnableDisableResetButton();
        }

        private void ACP_MaterialComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            AllCollisionPropertiesValues.Material = ((CollisionTerrain) ACP_MaterialComboBox.SelectedItem).ID;
            EnableDisableResetButton();
        }

        #endregion

        #region Collision Targets Section

        private void ACP_CollTargets_Everything_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Updating = true;

            AllCollisionPropertiesValues.SetFlag2(CollisionPlaneFlags2.Characters, ACP_CollTargets_Everything.Checked);

            if (ACP_CollTargets_Everything.Checked)
            {
                ACP_CollTargets_Items.Checked = false;
                ACP_CollTargets_PKMNTrainer.Checked = false;
            }
            else
            {
                ACP_CollFlags_FallThrough.Checked = false;
                ACP_CollFlags_NoWallJump.Checked = false;
                ACP_CollFlags_LeftLedge.Checked = false;
                ACP_CollFlags_RightLedge.Checked = false;
            }

            Updating = false;
            EnableDisableResetButton();
        }

        private void ACP_CollTargets_Items_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Updating = true;

            AllCollisionPropertiesValues.SetFlag2(CollisionPlaneFlags2.Items, ACP_CollTargets_Items.Checked);

            if (ACP_CollTargets_Items.Checked)
            {
                ACP_CollTargets_Everything.Checked = false;

                ACP_CollFlags_FallThrough.Checked = false;
                ACP_CollFlags_NoWallJump.Checked = false;
                ACP_CollFlags_RightLedge.Checked = false;
                ACP_CollFlags_LeftLedge.Checked = false;
            }

            Updating = false;
            EnableDisableResetButton();
        }

        private void ACP_CollTargets_PKMNTrainer_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Updating = true;

            AllCollisionPropertiesValues.SetFlag2(CollisionPlaneFlags2.PokemonTrainer,
                ACP_CollTargets_PKMNTrainer.Checked);

            if (ACP_CollTargets_PKMNTrainer.Checked)
            {
                ACP_CollTargets_Everything.Checked = false;

                ACP_CollFlags_FallThrough.Checked = false;
                ACP_CollFlags_NoWallJump.Checked = false;
                ACP_CollFlags_RightLedge.Checked = false;
                ACP_CollFlags_LeftLedge.Checked = false;
            }

            Updating = false;
            EnableDisableResetButton();
        }

        #endregion

        #region Collision Flags Section

        private void ACP_CollFlags_FallThrough_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            AllCollisionPropertiesValues.SetFlag(CollisionPlaneFlags.DropThrough, ACP_CollFlags_FallThrough.Checked);
            EnableDisableResetButton();
        }

        private void ACP_CollFlags_LeftLedge_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Updating = true;

            if (ACP_CollFlags_NoWallJump.Checked)
            {
                ACP_CollFlags_NoWallJump.Checked = false;
            }

            Updating = false;

            AllCollisionPropertiesValues.SetFlag(CollisionPlaneFlags.LeftLedge, ACP_CollFlags_LeftLedge.Checked);
            EnableDisableResetButton();
        }

        private void ACP_CollFlags_RightLedge_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Updating = true;

            if (ACP_CollFlags_NoWallJump.Checked)
            {
                ACP_CollFlags_NoWallJump.Checked = false;
            }

            Updating = false;

            AllCollisionPropertiesValues.SetFlag(CollisionPlaneFlags.RightLedge, ACP_CollFlags_RightLedge.Checked);
            EnableDisableResetButton();
        }

        private void ACP_CollFlags_NoWallJump_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Updating = true;

            if (ACP_CollFlags_LeftLedge.Checked || ACP_CollFlags_RightLedge.Checked)
            {
                ACP_CollFlags_LeftLedge.Checked = false;
                ACP_CollFlags_RightLedge.Checked = false;
            }

            Updating = false;

            AllCollisionPropertiesValues.SetFlag(CollisionPlaneFlags.NoWalljump, ACP_CollFlags_NoWallJump.Checked);
            EnableDisableResetButton();
        }

        private void ACP_CollFlags_Rotating_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            AllCollisionPropertiesValues.SetFlag(CollisionPlaneFlags.Rotating, ACP_CollFlags_Rotating.Checked);
            EnableDisableResetButton();
        }

        #endregion

        #region Unknown Flags Section

        private void ACP_UnkFlags_Unk1_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            AllCollisionPropertiesValues.SetFlag2(CollisionPlaneFlags2.UnknownSSE, ACP_UnkFlags_Unk1.Checked);
            EnableDisableResetButton();
        }

        private void ACP_UnkFlags_Unk2_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            AllCollisionPropertiesValues.SetFlag(CollisionPlaneFlags.Unknown1, ACP_UnkFlags_Unk2.Checked);
            EnableDisableResetButton();
        }

        private void ACP_UnkFlags_SuperSoft_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            AllCollisionPropertiesValues.SetFlag(CollisionPlaneFlags.SuperSoft, ACP_UnkFlags_SuperSoft.Checked);
            EnableDisableResetButton();
        }

        private void ACP_UnkFlags_Unk4_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            AllCollisionPropertiesValues.SetFlag(CollisionPlaneFlags.Unknown4, ACP_UnkFlags_Unk4.Checked);
            EnableDisableResetButton();
        }

        #endregion

        #region SelectedOverrides part

        private void ACP_Type_SelectedOverrides_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            ACP_SelectedOverrides_ApplyOverrideSection(0, ACP_Type_SelectedOverrides.Checked);
        }

        private void ACP_Material_SelectedOverrides_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            ACP_SelectedOverrides_ApplyOverrideSection(1, ACP_Material_SelectedOverrides.Checked);
        }

        private void ACP_CollTargets_SelectedOverrides_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            ACP_SelectedOverrides_ApplyOverrideSection(2, ACP_CollTargets_SelectedOverrides.Checked);
        }

        private void ACP_CollFlags_SelectedOverrides_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            ACP_SelectedOverrides_ApplyOverrideSection(3, ACP_CollFlags_SelectedOverrides.Checked);
        }

        private void ACP_UnkFlags_SelectedOverrides_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            ACP_SelectedOverrides_ApplyOverrideSection(4, ACP_UnkFlags_SelectedOverrides.Checked);
        }

        // Since structure classes are not considered reference, the Values variable is first taken
        // then overriden and then after that we apply it to ExtraData again so that it is overwritten.
        private void ACP_SelectedOverrides_ApplyOverrideSection(int Index, bool Value)
        {
            bool[] Values = (bool[]) AllCollisionPropertiesValues.ExtraData;

            Values[Index] = Value;

            AllCollisionPropertiesValues.ExtraData = Values;
            EnableDisableResetButton();
        }

        #endregion

        #region Miscellaneous

        private void EnableDisableResetButton()
        {
            ACP_ResetValuesToDef.Enabled =
                ACP_TypeComboBox.SelectedIndex > 0 ||
                ACP_MaterialComboBox.SelectedIndex > 0 ||
                ACP_Type_SelectedOverrides.Checked ||
                ACP_Material_SelectedOverrides.Checked ||
                ACP_CollTargets_SelectedOverrides.Checked ||
                ACP_CollFlags_SelectedOverrides.Checked ||
                ACP_UnkFlags_SelectedOverrides.Checked ||
                ACP_CollTargets_Everything.Checked ||
                ACP_CollTargets_Items.Checked ||
                ACP_CollTargets_PKMNTrainer.Checked ||
                ACP_CollFlags_LeftLedge.Checked ||
                ACP_CollFlags_RightLedge.Checked ||
                ACP_CollFlags_FallThrough.Checked ||
                ACP_CollFlags_NoWallJump.Checked ||
                ACP_CollFlags_Rotating.Checked ||
                ACP_UnkFlags_Unk1.Checked ||
                ACP_UnkFlags_Unk2.Checked ||
                ACP_UnkFlags_SuperSoft.Checked ||
                ACP_UnkFlags_Unk4.Checked;
        }

        #endregion

        #endregion

        #region Miscellaneous

        private void ResetACPValues()
        {
            AllCollisionPropertiesValues = new CollisionPlane_S
            {
                // Extra data is used as a tag in which it is carried over to the object, making it useful
                // as a form of "extra metadata".
                // All booleans represent the default values of "All Collision Properties" selected item overrides
                // used for pasting. They are: Type, Material, Targets (Target to), Flag Properties, 
                // and Unknown Flag Properties.
                ExtraData = new bool[] {false, false, false, false, false},
                Material = CollisionTerrain.Terrains[0].ID,
                Type = CollisionPlaneType.None
            };
        }

        // Get AllCollisionPropertiesValues as a form of reference, even though it is a value reference.
        // This is dumb, but it can throw an error since it is a field that 
        // is done by a marshal-by-reference class.
        public CollisionPlane_S GetACPV()
        {
            return AllCollisionPropertiesValues;
        }

        #endregion

        #region Utility

        private static double DegreesToRadians(double Angle)
        {
            return Math.PI * Angle / 180.0;
        }

        private static double RadiansToDegrees(double Radians)
        {
            return Radians * (180.0 / Math.PI);
        }

        #endregion

        #region Set Rotation Location Form

        private class CollisionEditor_PasteOptions_SetLocationForm : Form
        {
            #region Designer

            private IContainer components = null;
            private ToolTip Tips;

            private Label SpecificLocationX_Label;
            private Label SpecificLocationY_Label;
            private NumericInputBox SpecificLocationX_Value;
            private NumericInputBox SpecificLocationY_Value;

            private Label FreeLocationX_Label;
            private Label FreeLocationY_Label;
            private NumericInputBox FreeLocationX_Value;
            private NumericInputBox FreeLocationY_Value;

            private RadioButton SpecificLocationRadio_SpecificPlacement;
            private RadioButton SpecificLocationRadio_FreePlacement;
            private RadioButton SpecificLocationRadio_ZeroZero;

            private Button Button_SetLocation;
            private Button Button_ResetToOriginalValue;
            private Button Button_Cancel;


            protected override void Dispose(bool disposing)
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }

                base.Dispose(disposing);
            }

            private void InitializeComponent()
            {
                components = new Container();
                Tips = new ToolTip(components);

                SpecificLocationX_Label = new Label();
                SpecificLocationY_Label = new Label();
                SpecificLocationX_Value = new NumericInputBox();
                SpecificLocationY_Value = new NumericInputBox();

                SpecificLocationRadio_SpecificPlacement = new RadioButton();
                SpecificLocationRadio_FreePlacement = new RadioButton();
                SpecificLocationRadio_ZeroZero = new RadioButton();

                Button_SetLocation = new Button();
                Button_ResetToOriginalValue = new Button();
                Button_Cancel = new Button();


                SuspendLayout();

                // 
                // SpecificLocationX_Label
                //
                SpecificLocationX_Label = CreateNumericalLabel(new Drawing.Point(30, 110), new Drawing.Size(20, 20),
                    "SpecificLocationX_Label", "X");
                SpecificLocationX_Label.TabIndex = 4;
                // 
                // SpecificLocationY_Label
                // 
                SpecificLocationY_Label = CreateNumericalLabel(new Drawing.Point(30, 129), new Drawing.Size(20, 20),
                    "SpecificLocationY_Label", "Y");
                SpecificLocationY_Label.TabIndex = 5;
                // 
                // SpecificLocationX_Value
                // 
                SpecificLocationX_Value = CreateNumericalInput(new Drawing.Point(49, 110), new Drawing.Size(155, 20),
                    "SpecificLocationX_Value", "0");
                SpecificLocationX_Value.ValueChanged += new EventHandler(SpecificLocationX_Value_ValueChanged);
                SpecificLocationX_Value.TabIndex = 4;
                // 
                // SpecificLocationY_Value
                // 
                SpecificLocationY_Value = CreateNumericalInput(new Drawing.Point(49, 129), new Drawing.Size(155, 20),
                    "SpecificLocationY_Value", "0");
                SpecificLocationY_Value.ValueChanged += new EventHandler(SpecificLocationY_Value_ValueChanged);
                SpecificLocationY_Value.TabIndex = 5;

                //
                // FreeLocationX_Label
                //
                FreeLocationX_Label = CreateNumericalLabel(SpecificLocationX_Label, "FreeLocationX_Label");
                FreeLocationX_Label.Location = new Drawing.Point(30, 50);
                FreeLocationX_Label.TabIndex = 7;
                //
                // FreeLocationY_Label
                //
                FreeLocationY_Label = CreateNumericalLabel(SpecificLocationY_Label, "FreeLocationY_Label");
                FreeLocationY_Label.Location = new Drawing.Point(30, 69);
                FreeLocationY_Label.TabIndex = 8;
                //
                // FreeLocationX_Value
                //
                FreeLocationX_Value = CreateNumericalInput(SpecificLocationX_Value, "FreeLocationX_Value");
                FreeLocationX_Value.ValueChanged += new EventHandler(FreeLocationX_Value_ValueChanged);
                FreeLocationX_Value.Location = new Drawing.Point(49, 50);
                FreeLocationX_Value.TabIndex = 7;
                FreeLocationX_Value.ReadOnly = true;
                //
                // FreeLocationY_Value
                //
                FreeLocationY_Value = CreateNumericalInput(SpecificLocationY_Value, "FreeLocationY_Value");
                FreeLocationY_Value.ValueChanged += new EventHandler(FreeLocationY_Value_ValueChanged);
                FreeLocationY_Value.Location = new Drawing.Point(49, 69);
                FreeLocationY_Value.TabIndex = 8;
                FreeLocationY_Value.ReadOnly = true;


                // 
                // SpecificLocationRadio_SpecificPlacementSpecificLocationRadio_SpecificPlacement
                // 
                SpecificLocationRadio_SpecificPlacement.AutoSize = true;
                SpecificLocationRadio_SpecificPlacement.Checked = true;
                SpecificLocationRadio_SpecificPlacement.Location = new Drawing.Point(12, 91);
                SpecificLocationRadio_SpecificPlacement.Name = "ManiColl_Placement_RadioSpefPlacement";
                SpecificLocationRadio_SpecificPlacement.Size = new Drawing.Size(113, 17);
                SpecificLocationRadio_SpecificPlacement.TabIndex = 3;
                SpecificLocationRadio_SpecificPlacement.Text = "Specified Location";
                SpecificLocationRadio_SpecificPlacement.UseVisualStyleBackColor = true;
                SpecificLocationRadio_SpecificPlacement.CheckedChanged +=
                    new EventHandler(SpecificLocationRadio_SpecificPlacement_CheckedChanged);
                Tips.SetToolTip(SpecificLocationRadio_SpecificPlacement,
                    "If you like being specific, you can specify the location by inputting the two values.");
                // 
                // SpecificLocationRadio_FreePlacement
                // 
                SpecificLocationRadio_FreePlacement.AutoSize = true;
                SpecificLocationRadio_FreePlacement.Location = new Drawing.Point(12, 31);
                SpecificLocationRadio_FreePlacement.Name = "SpecificLocationRadio_FreePlacement";
                SpecificLocationRadio_FreePlacement.Size = new Drawing.Size(72, 17);
                SpecificLocationRadio_FreePlacement.TabIndex = 6;
                SpecificLocationRadio_FreePlacement.Text = "Freeplace";
                SpecificLocationRadio_FreePlacement.UseVisualStyleBackColor = true;
                SpecificLocationRadio_FreePlacement.CheckedChanged +=
                    new EventHandler(SpecificLocationRadio_FreePlacement_CheckedChanged);
                Tips.SetToolTip(SpecificLocationRadio_FreePlacement,
                    "If this is selected, you can move the center location of copied collisions anywhere.");
                // 
                // SpecificLocationRadio_ZeroZero
                // 
                SpecificLocationRadio_ZeroZero.AutoSize = true;
                SpecificLocationRadio_ZeroZero.Location = new Drawing.Point(12, 12);
                SpecificLocationRadio_ZeroZero.Size = new Drawing.Size(117, 17);
                SpecificLocationRadio_ZeroZero.TabIndex = 9;
                SpecificLocationRadio_ZeroZero.TabStop = true;
                SpecificLocationRadio_ZeroZero.Name = "SpecificLocationRadio_ZeroZero";
                SpecificLocationRadio_ZeroZero.Text = "Center (0, 0)";
                SpecificLocationRadio_ZeroZero.UseVisualStyleBackColor = true;
                SpecificLocationRadio_ZeroZero.CheckedChanged +=
                    new EventHandler(SpecificLocationRadio_ZeroZero_CheckedChanged);
                Tips.SetToolTip(SpecificLocationRadio_ZeroZero, "It is placed at the center of the stage (0, 0).");


                // 
                // Button_SetLocation
                // 
                Button_SetLocation.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
                Button_SetLocation.Location = new Drawing.Point(6, 164);
                Button_SetLocation.Size = new Drawing.Size(224, 23);
                Button_SetLocation.TabIndex = 0;
                Button_SetLocation.Name = "Button_SetLocation";
                Button_SetLocation.Text = "Set Location";
                Button_SetLocation.UseVisualStyleBackColor = true;
                Button_SetLocation.DialogResult = DialogResult.OK;
                Button_SetLocation.Click += new EventHandler(Button_SetLocation_Click);
                // 
                // Button_ResetToOriginalValue
                // 
                Button_ResetToOriginalValue.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                Button_ResetToOriginalValue.Location = new Drawing.Point(6, 190);
                Button_ResetToOriginalValue.Size = new Drawing.Size(110, 23);
                Button_ResetToOriginalValue.TabIndex = 1;
                Button_ResetToOriginalValue.Name = "Button_ResetToOriginalValue";
                Button_ResetToOriginalValue.Text = "Reset Value";
                Button_ResetToOriginalValue.UseVisualStyleBackColor = true;
                Button_ResetToOriginalValue.Click += new EventHandler(Button_ResetToOriginalValue_Click);
                // 
                // Button_Cancel
                // 
                Button_Cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                Button_Cancel.Location = new Drawing.Point(120, 190);
                Button_Cancel.Size = new Drawing.Size(110, 23);
                Button_Cancel.TabIndex = 2;
                Button_Cancel.Name = "Button_Cancel";
                Button_Cancel.Text = "Cancel";
                Button_Cancel.UseVisualStyleBackColor = true;
                Button_Cancel.DialogResult = DialogResult.Cancel;
                Button_Cancel.Click += new EventHandler(Button_Cancel_Click);

                //
                // The form itself (this)
                //
                Name = "CollisionEditor_PasteOptions_SetLocationForm";
                Text = "Set Location";
                AcceptButton = Button_SetLocation;
                CancelButton = Button_Cancel;
                AutoScaleMode = AutoScaleMode.Font;
                AutoScaleDimensions = new SizeF(6F, 13F);
                ClientSize = new Drawing.Size(236, 219);
                MaximumSize = Size;
                MinimumSize = Size;
                FormBorderStyle = FormBorderStyle.FixedToolWindow;
                StartPosition = FormStartPosition.CenterParent;

                Controls.Add(SpecificLocationX_Label);
                Controls.Add(SpecificLocationY_Label);
                Controls.Add(SpecificLocationX_Value);
                Controls.Add(SpecificLocationY_Value);
                Controls.Add(FreeLocationX_Label);
                Controls.Add(FreeLocationY_Label);
                Controls.Add(FreeLocationX_Value);
                Controls.Add(FreeLocationY_Value);
                Controls.Add(SpecificLocationRadio_SpecificPlacement);
                Controls.Add(SpecificLocationRadio_FreePlacement);
                Controls.Add(SpecificLocationRadio_ZeroZero);

                Controls.Add(Button_SetLocation);
                Controls.Add(Button_ResetToOriginalValue);
                Controls.Add(Button_Cancel);

                Icon = ParentOptions.Icon;

                ResumeLayout();
            }

            #endregion

            private CollisionEditor_PasteOptions ParentOptions;

            private readonly Vector2 LastLocation;
            private readonly byte LastRadioValue;

            public Vector2 CurrentLocation;
            public byte PointMode = 0;

            public CollisionEditor_PasteOptions_SetLocationForm(CollisionEditor_PasteOptions Parent,
                                                                byte LastRadioSelectedValue, Vector2 LocationValue)
            {
                ParentOptions = Parent;
                LastLocation = LocationValue;
                CurrentLocation = LastLocation;

                LastRadioValue = LastRadioSelectedValue;

                InitializeComponent();
            }

            private void Button_SetLocation_Click(object sender, EventArgs e)
            {
                ParentOptions.UpdateLocationFromLocationForm(CurrentLocation, PointMode);
                Close();
            }

            private void Button_ResetToOriginalValue_Click(object sender, EventArgs e)
            {
                ResetToSetValue();
                CurrentLocation = LastLocation;
                ParentOptions.UpdateLocationFromLocationForm(LastLocation, PointMode);
            }

            private void Button_Cancel_Click(object sender, EventArgs e)
            {
                ParentOptions.UpdateLocationFromLocationForm(LastLocation, PointMode);
                Close();
            }

            private void SpecificLocationX_Value_ValueChanged(object Sender, EventArgs e)
            {
                if (ParentOptions.Updating)
                {
                    return;
                }

                CurrentLocation = new Vector2(SpecificLocationX_Value.Value, SpecificLocationY_Value.Value);
                ParentOptions.UpdateLocationFromLocationForm(CurrentLocation, PointMode);
            }

            private void SpecificLocationY_Value_ValueChanged(object Sender, EventArgs e)
            {
                if (ParentOptions.Updating)
                {
                    return;
                }

                CurrentLocation = new Vector2(SpecificLocationX_Value.Value, SpecificLocationY_Value.Value);
                ParentOptions.UpdateLocationFromLocationForm(CurrentLocation, PointMode);
            }

            private void FreeLocationX_Value_ValueChanged(object Sender, EventArgs e)
            {
                if (ParentOptions.Updating)
                {
                    return;
                }

                CurrentLocation = new Vector2(FreeLocationX_Value.Value, FreeLocationY_Value.Value);
                ParentOptions.UpdateLocationFromLocationForm(CurrentLocation, PointMode);
            }

            private void FreeLocationY_Value_ValueChanged(object Sender, EventArgs e)
            {
                if (ParentOptions.Updating)
                {
                    return;
                }

                CurrentLocation = new Vector2(FreeLocationX_Value.Value, FreeLocationY_Value.Value);
                ParentOptions.UpdateLocationFromLocationForm(CurrentLocation, PointMode);
            }

            private void SpecificLocationRadio_ZeroZero_CheckedChanged(object sender, EventArgs e)
            {
                if (ParentOptions.Updating)
                {
                    return;
                }

                CurrentLocation = new Vector2(0, 0);
                ParentOptions.UpdateLocationFromLocationForm(CurrentLocation, PointMode);
            }

            private void SpecificLocationRadio_FreePlacement_CheckedChanged(object sender, EventArgs e)
            {
                FreeLocationX_Value.Enabled =
                    FreeLocationY_Value.Enabled =
                        SpecificLocationRadio_FreePlacement.Checked;

                if (ParentOptions.Updating)
                {
                    return;
                }

                if (SpecificLocationRadio_FreePlacement.Checked)
                {
                    ParentOptions.SetPointMode = true;
                    ParentOptions.PointModeType = PointMode;

                    ParentOptions.Visible = ParentOptions.Enabled =
                        Visible = Enabled = false;
                }
            }

            private void SpecificLocationRadio_SpecificPlacement_CheckedChanged(object sender, EventArgs e)
            {
                SpecificLocationX_Value.Enabled =
                    SpecificLocationY_Value.Enabled =
                        SpecificLocationRadio_SpecificPlacement.Checked;

                if (ParentOptions.Updating)
                {
                    return;
                }

                CurrentLocation = new Vector2(SpecificLocationX_Value.Value, SpecificLocationY_Value.Value);
                ParentOptions.UpdateLocationFromLocationForm(CurrentLocation, PointMode);
            }

            #region Paste Options to SetLocationForm

            public void CustomUserPointSet(ModelPanelViewport Viewport, Drawing.Point Location, float Depth)
            {
                Vector3 Point3D = Viewport.UnProject(Location.X, Location.Y, -Depth);

                // Trace ray to Z axis
                Point3D = Vector3.IntersectZ(Point3D, Viewport.UnProject(Location.X, Location.Y, 0.0f), 0.0f);
                Vector2 Point = (Vector2) Point3D;

                switch (PointMode)
                {
                    // Sets the center point for rotation.
                    case 1:
                        CurrentLocation = Point;

                        break;

                    // Sets the center point for location, which does not use the location form.
                    case 2: break;

                    default: break;
                }

                ParentOptions.UpdateLocationFromLocationForm(CurrentLocation, PointMode);

                FreeLocationX_Value.Value = CurrentLocation._x;
                FreeLocationY_Value.Value = CurrentLocation._y;

                ParentOptions.Visible = ParentOptions.Enabled =
                    Visible = Enabled = true;

                ParentOptions.PointModeType = 0;
                ParentOptions.SetPointMode = false;
            }

            #endregion

            #region Form Events

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);

                ResetToSetValue();
                TopMost = true;
            }

            protected override void OnActivated(EventArgs e)
            {
                base.OnActivated(e);

                // Set the window opacity full if active (focused).
                // It only takes percentages or 1.0 (100%) as maximum.
                Opacity = 1.0f;
            }

            protected override void OnDeactivate(EventArgs e)
            {
                base.OnDeactivate(e);

                // Set the window opacity 60% if not active (focused).
                if (!IsDisposed && !FormIsClosing)
                {
                    Opacity = 0.6f;
                }
            }

            private bool FormIsClosing = false;

            protected override void OnClosing(CancelEventArgs e)
            {
                base.OnClosing(e);

                FormIsClosing = true;
            }

            protected override void OnClosed(EventArgs e)
            {
                base.OnClosed(e);

                ParentOptions.NullifyLocationForm();
            }

            #endregion

            #region Control Creation

            private NumericInputBox CreateNumericalInput(NumericInputBox ToTakeFrom, string InputBoxName)
            {
                return CreateNumericalInput(ToTakeFrom.Location, ToTakeFrom.Size, InputBoxName, ToTakeFrom.Text);
            }

            private NumericInputBox CreateNumericalInput(Drawing.Point Location, Drawing.Size Size, string InputBoxName,
                                                         string DefaultValue)
            {
                NumericInputBox NumericControl = new NumericInputBox();

                NumericControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                NumericControl.BorderStyle = BorderStyle.FixedSingle;
                NumericControl.Integer = false;
                NumericControl.Integral = false;
                NumericControl.Location = Location;
                NumericControl.Size = Size;
                NumericControl.Name = InputBoxName;
                NumericControl.Text = DefaultValue;
                NumericControl.MaximumValue = 3.402823E+38F;
                NumericControl.MinimumValue = -3.402823E+38F;
                NumericControl.Enabled = false;

                return NumericControl;
            }

            private Label CreateNumericalLabel(Label ToTakeFrom, string InputBoxName)
            {
                return CreateNumericalLabel(ToTakeFrom.Location, ToTakeFrom.Size, InputBoxName, ToTakeFrom.Text);
            }

            private Label CreateNumericalLabel(Drawing.Point Location, Drawing.Size Size, string InputBoxName,
                                               string DefaultText)
            {
                Label NumericControl = new Label();

                NumericControl.BorderStyle = BorderStyle.FixedSingle;
                NumericControl.TextAlign = ContentAlignment.MiddleCenter;
                NumericControl.Location = Location;
                NumericControl.Size = Size;
                NumericControl.Name = InputBoxName;
                NumericControl.Text = DefaultText;
                NumericControl.Enabled = false;

                return NumericControl;
            }

            #endregion

            #region Miscellaneous

            private void ResetToSetValue()
            {
                // Apply the initial values for specific location.
                ParentOptions.Updating = true;

                switch (LastRadioValue)
                {
                    case 0:
                        SpecificLocationRadio_SpecificPlacement.Checked = true;
                        break;
                    case 1:
                        SpecificLocationRadio_FreePlacement.Checked = true;

                        FreeLocationX_Value.Value = LastLocation._x;
                        FreeLocationY_Value.Value = LastLocation._y;

                        break;
                    case 2:
                        SpecificLocationRadio_ZeroZero.Checked = true;

                        SpecificLocationX_Value.Value = LastLocation._x;
                        SpecificLocationY_Value.Value = LastLocation._y;
                        break;
                }

                ParentOptions.Updating = false;
            }

            #endregion
        }

        #endregion
    }
}