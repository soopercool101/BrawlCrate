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
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();

			this.advancedPO_TabC = new TabControl();
			this.advancedPO_TabC_ManipulateCollisionTab = new TabPage();

			this.BottomPanel = new Panel();

			this.PasteCollision = new Button();
			this.Cancel = new Button();

			this.pasteOptions_tipper = new ToolTip(this.components);

			this.ManiColl_RotateGroup = new GroupBox();
			this.ManiColl_Rotate_DegreesValue = new NumericUpDown();
			this.ManiColl_Rotate_DegreesLabel = new Label();
			this.ManiColl_Rotate_SetTo0 = new Button();
			this.ManiColl_Rotate_SetTo90 = new Button();
			this.ManiColl_Rotate_SetTo180 = new Button();
			this.ManiColl_Rotate_SetTo270 = new Button();
			this.ManiColl_Rotate_SetRotationLocation = new Button();

			this.ManiColl_ScaleGroup = new GroupBox();
			this.ManiColl_Scale_ScalarValue = new NumericUpDown();
			this.ManiColl_Scale_XLabel = new Label();
			this.ManiColl_Scale_ResetScale = new Button();

			this.ManiColl_FlipGroup = new GroupBox();
			this.ManiColl_Flip_FlipY = new CheckBox();
			this.ManiColl_Flip_FlipX = new CheckBox();

			this.ManiColl_PlacementGroup = new GroupBox();
			this.ManiColl_Placement_SpefPlacement_YLabel = new Label();
			this.ManiColl_Placement_SpefPlacement_XLabel = new Label();
			this.ManiColl_Placement_SpefPlacement_LocY = new NumericInputBox();
			this.ManiColl_Placement_SpefPlacement_LocX = new NumericInputBox();
			this.ManiColl_Placement_RadioSpefPlacement = new RadioButton();
			this.ManiColl_Placement_RadioFreeplace = new RadioButton();
			this.ManiColl_Placement_RadioZeroZero = new RadioButton();



			this.advancedPO_TabC_AllCollsProps = new TabPage();
			this.advancedPO_TabC_AllCollsProps_Panel = new Panel();

			this.AllCollsProps_WarningLink = new LinkLabel();
			this.ACP_ResetValuesToDef = new Button();

			this.ACP_TypeLabel = new Label();
			this.ACP_Type_SelectedOverrides = new CheckBox();
			this.ACP_TypeComboBox = new ComboBox();

			this.ACP_MaterialLabel = new Label();
			this.ACP_Material_SelectedOverrides = new CheckBox();
			this.ACP_MaterialComboBox = new ComboBox();

			this.ACP_CollTargetsGroup = new GroupBox();
			this.ACP_CollTargets_SelectedOverrides = new CheckBox();
			this.ACP_CollTargets_Items = new CheckBox();
			this.ACP_CollTargets_PKMNTrainer = new CheckBox();
			this.ACP_CollTargets_Everything = new CheckBox();

			this.ACP_CollFlagsGroup = new GroupBox();
			this.ACP_CollFlags_SelectedOverrides = new CheckBox();
			this.ACP_CollFlags_NoWallJump = new CheckBox();
			this.ACP_CollFlags_Rotating = new CheckBox();
			this.ACP_CollFlags_LeftLedge = new CheckBox();
			this.ACP_CollFlags_RightLedge = new CheckBox();
			this.ACP_CollFlags_FallThrough = new CheckBox();

			this.ACP_UnkFlagsGroup = new GroupBox();
			this.ACP_UnkFlags_SelectedOverrides = new CheckBox();
			this.ACP_UnkFlags_Unk1 = new CheckBox();
			this.ACP_UnkFlags_Unk4 = new CheckBox();
			this.ACP_UnkFlags_SuperSoft = new CheckBox();
			this.ACP_UnkFlags_Unk2 = new CheckBox();


			this.advancedPO_TabC.SuspendLayout();
			this.advancedPO_TabC_ManipulateCollisionTab.SuspendLayout();

			this.ManiColl_PlacementGroup.SuspendLayout();
			this.ManiColl_ScaleGroup.SuspendLayout();

			((ISupportInitialize)this.ManiColl_Scale_ScalarValue).BeginInit();
			((ISupportInitialize)this.ManiColl_Rotate_DegreesValue).BeginInit();

			this.ManiColl_FlipGroup.SuspendLayout();
			this.ManiColl_RotateGroup.SuspendLayout();

			this.advancedPO_TabC_AllCollsProps.SuspendLayout();
			this.advancedPO_TabC_AllCollsProps_Panel.SuspendLayout();

			this.ACP_UnkFlagsGroup.SuspendLayout();
			this.ACP_CollFlagsGroup.SuspendLayout();
			this.ACP_CollTargetsGroup.SuspendLayout();

			this.BottomPanel.SuspendLayout();
			this.SuspendLayout();

			// 
			// advancedPO_TabC
			// 
			this.advancedPO_TabC.Controls.Add(this.advancedPO_TabC_ManipulateCollisionTab);
			this.advancedPO_TabC.Controls.Add(this.advancedPO_TabC_AllCollsProps);
			this.advancedPO_TabC.Dock = DockStyle.Fill;
			this.advancedPO_TabC.Location = new Drawing.Point(0, 0);
			this.advancedPO_TabC.Name = "advancedPO_TabC";
			this.advancedPO_TabC.SelectedIndex = 0;
			this.advancedPO_TabC.Size = new Drawing.Size(236, 407);
			this.advancedPO_TabC.TabIndex = 0;
			// 
			// advancedPO_TabC_ManipulateCollisionTab
			// 
			this.advancedPO_TabC_ManipulateCollisionTab.AutoScroll = true;
			this.advancedPO_TabC_ManipulateCollisionTab.Controls.Add(this.ManiColl_PlacementGroup);
			this.advancedPO_TabC_ManipulateCollisionTab.Controls.Add(this.ManiColl_ScaleGroup);
			this.advancedPO_TabC_ManipulateCollisionTab.Controls.Add(this.ManiColl_FlipGroup);
			this.advancedPO_TabC_ManipulateCollisionTab.Controls.Add(this.ManiColl_RotateGroup);
			this.advancedPO_TabC_ManipulateCollisionTab.Location = new Drawing.Point(4, 22);
			this.advancedPO_TabC_ManipulateCollisionTab.Name = "advancedPO_TabC_ManipulateCollisionTab";
			this.advancedPO_TabC_ManipulateCollisionTab.Padding = new Padding(3);
			this.advancedPO_TabC_ManipulateCollisionTab.Size = new Drawing.Size(228, 381);
			this.advancedPO_TabC_ManipulateCollisionTab.TabIndex = 0;
			this.advancedPO_TabC_ManipulateCollisionTab.Text = "Manipulate Collision";
			this.advancedPO_TabC_ManipulateCollisionTab.UseVisualStyleBackColor = true;
			// 
			// ManiColl_PlacementGroup
			// 
			this.ManiColl_PlacementGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.ManiColl_PlacementGroup.Controls.Add(this.ManiColl_Placement_SpefPlacement_XLabel);
			this.ManiColl_PlacementGroup.Controls.Add(this.ManiColl_Placement_SpefPlacement_YLabel);
			this.ManiColl_PlacementGroup.Controls.Add(this.ManiColl_Placement_SpefPlacement_LocX);
			this.ManiColl_PlacementGroup.Controls.Add(this.ManiColl_Placement_SpefPlacement_LocY);
			this.ManiColl_PlacementGroup.Controls.Add(this.ManiColl_Placement_RadioSpefPlacement);
			this.ManiColl_PlacementGroup.Controls.Add(this.ManiColl_Placement_RadioFreeplace);
			this.ManiColl_PlacementGroup.Controls.Add(this.ManiColl_Placement_RadioZeroZero);
			this.ManiColl_PlacementGroup.Location = new Drawing.Point(4, 222);
			this.ManiColl_PlacementGroup.Name = "ManiColl_PlacementGroup";
			this.ManiColl_PlacementGroup.Size = new Drawing.Size(218, 127);
			this.ManiColl_PlacementGroup.TabIndex = 3;
			this.ManiColl_PlacementGroup.TabStop = false;
			this.ManiColl_PlacementGroup.Text = "Location/Placement";
			// 
			// ManiColl_Placement_RadioZeroZero
			// 
			this.ManiColl_Placement_RadioZeroZero.AutoSize = true;
			this.ManiColl_Placement_RadioZeroZero.Checked = true;
			this.ManiColl_Placement_RadioZeroZero.Location = new Drawing.Point(12, 19);
			this.ManiColl_Placement_RadioZeroZero.Name = "ManiColl_Placement_RadioZeroZero";
			this.ManiColl_Placement_RadioZeroZero.Size = new Drawing.Size(117, 17);
			this.ManiColl_Placement_RadioZeroZero.TabIndex = 0;
			this.ManiColl_Placement_RadioZeroZero.TabStop = true;
			this.ManiColl_Placement_RadioZeroZero.Text = "Center (0, 0)";
			this.ManiColl_Placement_RadioZeroZero.UseVisualStyleBackColor = true;
			this.ManiColl_Placement_RadioZeroZero.CheckedChanged += new EventHandler(this.ManiColl_Placement_RadioZeroZero_CheckedChanged);
			this.pasteOptions_tipper.SetToolTip(this.ManiColl_Placement_RadioZeroZero, "It is placed at the center of the stage (0, 0).");
			// 
			// ManiColl_Placement_RadioSpefPlacement
			// 
			this.ManiColl_Placement_RadioSpefPlacement.AutoSize = true;
			this.ManiColl_Placement_RadioSpefPlacement.Location = new Drawing.Point(12, 56);
			this.ManiColl_Placement_RadioSpefPlacement.Name = "ManiColl_Placement_RadioSpefPlacement";
			this.ManiColl_Placement_RadioSpefPlacement.Size = new Drawing.Size(113, 17);
			this.ManiColl_Placement_RadioSpefPlacement.TabIndex = 0;
			this.ManiColl_Placement_RadioSpefPlacement.Text = "Specified Location";
			this.ManiColl_Placement_RadioSpefPlacement.UseVisualStyleBackColor = true;
			this.ManiColl_Placement_RadioSpefPlacement.CheckedChanged += new EventHandler(this.ManiColl_Placement_RadioSpefPlacement_CheckedChanged);
			this.pasteOptions_tipper.SetToolTip(this.ManiColl_Placement_RadioSpefPlacement, "If you like being specific, you can specify the location by inputting the two values.");
			// 
			// ManiColl_Placement_RadioFreeplace
			// 
			this.ManiColl_Placement_RadioFreeplace.AutoSize = true;
			this.ManiColl_Placement_RadioFreeplace.Location = new Drawing.Point(12, 38);
			this.ManiColl_Placement_RadioFreeplace.Name = "ManiColl_Placement_RadioFreeplace";
			this.ManiColl_Placement_RadioFreeplace.Size = new Drawing.Size(72, 17);
			this.ManiColl_Placement_RadioFreeplace.TabIndex = 0;
			this.ManiColl_Placement_RadioFreeplace.Text = "Freeplace";
			this.ManiColl_Placement_RadioFreeplace.UseVisualStyleBackColor = true;
			this.ManiColl_Placement_RadioFreeplace.CheckedChanged += new EventHandler(this.ManiColl_Placement_RadioFreeplace_CheckedChanged);
			this.pasteOptions_tipper.SetToolTip(this.ManiColl_Placement_RadioFreeplace, "If this is selected, you can move the center location of copied collisions anywhere.");
			// 
			// ManiColl_Placement_SpefPlacement_XLabel
			// 
			this.ManiColl_Placement_SpefPlacement_XLabel.BorderStyle = BorderStyle.FixedSingle;
			this.ManiColl_Placement_SpefPlacement_XLabel.Location = new Drawing.Point(33, 79);
			this.ManiColl_Placement_SpefPlacement_XLabel.Name = "ManiColl_Placement_SpefPlacement_XLabel";
			this.ManiColl_Placement_SpefPlacement_XLabel.Size = new Drawing.Size(20, 20);
			this.ManiColl_Placement_SpefPlacement_XLabel.TabIndex = 2;
			this.ManiColl_Placement_SpefPlacement_XLabel.Text = "X";
			this.ManiColl_Placement_SpefPlacement_XLabel.TextAlign = ContentAlignment.MiddleCenter;
			this.ManiColl_Placement_SpefPlacement_XLabel.Enabled = false;
			// 
			// ManiColl_Placement_SpefPlacement_LocX
			// 
			this.ManiColl_Placement_SpefPlacement_LocX.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.ManiColl_Placement_SpefPlacement_LocX.BorderStyle = BorderStyle.FixedSingle;
			this.ManiColl_Placement_SpefPlacement_LocX.Integer = false;
			this.ManiColl_Placement_SpefPlacement_LocX.Integral = false;
			this.ManiColl_Placement_SpefPlacement_LocX.Location = new Drawing.Point(52, 79);
			this.ManiColl_Placement_SpefPlacement_LocX.MaximumValue = 3.402823E+38F;
			this.ManiColl_Placement_SpefPlacement_LocX.MinimumValue = -3.402823E+38F;
			this.ManiColl_Placement_SpefPlacement_LocX.Name = "ManiColl_Placement_SpefPlacement_LocX";
			this.ManiColl_Placement_SpefPlacement_LocX.Size = new Drawing.Size(135, 20);
			this.ManiColl_Placement_SpefPlacement_LocX.TabIndex = 1;
			this.ManiColl_Placement_SpefPlacement_LocX.Text = "0";
			this.ManiColl_Placement_SpefPlacement_LocX.ValueChanged += new EventHandler(this.ManiColl_Placement_SpefPlacement_LocX_ValueChanged);
			this.ManiColl_Placement_SpefPlacement_LocX.Enabled = false;
			// 
			// ManiColl_Placement_SpefPlacement_YLabel
			// 
			this.ManiColl_Placement_SpefPlacement_YLabel.BorderStyle = BorderStyle.FixedSingle;
			this.ManiColl_Placement_SpefPlacement_YLabel.Location = new Drawing.Point(33, 98);
			this.ManiColl_Placement_SpefPlacement_YLabel.Name = "ManiColl_Placement_SpefPlacement_YLabel";
			this.ManiColl_Placement_SpefPlacement_YLabel.Size = new Drawing.Size(20, 20);
			this.ManiColl_Placement_SpefPlacement_YLabel.TabIndex = 2;
			this.ManiColl_Placement_SpefPlacement_YLabel.Text = "Y";
			this.ManiColl_Placement_SpefPlacement_YLabel.TextAlign = ContentAlignment.MiddleCenter;
			this.ManiColl_Placement_SpefPlacement_YLabel.Enabled = false;
			// 
			// ManiColl_Placement_SpefPlacement_LocY
			// 
			this.ManiColl_Placement_SpefPlacement_LocY.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.ManiColl_Placement_SpefPlacement_LocY.BorderStyle = BorderStyle.FixedSingle;
			this.ManiColl_Placement_SpefPlacement_LocY.Integer = false;
			this.ManiColl_Placement_SpefPlacement_LocY.Integral = false;
			this.ManiColl_Placement_SpefPlacement_LocY.Location = new Drawing.Point(52, 98);
			this.ManiColl_Placement_SpefPlacement_LocY.MaximumValue = 3.402823E+38F;
			this.ManiColl_Placement_SpefPlacement_LocY.MinimumValue = -3.402823E+38F;
			this.ManiColl_Placement_SpefPlacement_LocY.Name = "ManiColl_Placement_SpefPlacement_LocY";
			this.ManiColl_Placement_SpefPlacement_LocY.Size = new Drawing.Size(135, 20);
			this.ManiColl_Placement_SpefPlacement_LocY.TabIndex = 1;
			this.ManiColl_Placement_SpefPlacement_LocY.Text = "0";
			this.ManiColl_Placement_SpefPlacement_LocY.ValueChanged += new EventHandler(this.ManiColl_Placement_SpefPlacement_LocY_ValueChanged);
			this.ManiColl_Placement_SpefPlacement_LocY.Enabled = false;
			// 
			// ManiColl_ScaleGroup
			// 
			this.ManiColl_ScaleGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.ManiColl_ScaleGroup.Controls.Add(this.ManiColl_Scale_ScalarValue);
			this.ManiColl_ScaleGroup.Controls.Add(this.ManiColl_Scale_XLabel);
			this.ManiColl_ScaleGroup.Controls.Add(this.ManiColl_Scale_ResetScale);
			this.ManiColl_ScaleGroup.Location = new Drawing.Point(4, 166);
			this.ManiColl_ScaleGroup.Name = "ManiColl_ScaleGroup";
			this.ManiColl_ScaleGroup.Size = new Drawing.Size(218, 50);
			this.ManiColl_ScaleGroup.TabIndex = 2;
			this.ManiColl_ScaleGroup.TabStop = false;
			this.ManiColl_ScaleGroup.Text = "Scale";
			// 
			// ManiColl_Scale_ScalarValue
			// 
			this.ManiColl_Scale_ScalarValue.Anchor = AnchorStyles.Top;
			this.ManiColl_Scale_ScalarValue.DecimalPlaces = 3;
			this.ManiColl_Scale_ScalarValue.Location = new Drawing.Point(39, 19);
			this.ManiColl_Scale_ScalarValue.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
			this.ManiColl_Scale_ScalarValue.Minimum = new decimal(new int[] { 1, 0, 0, 196608 });
			this.ManiColl_Scale_ScalarValue.Name = "ManiColl_Scale_ScalarValue";
			this.ManiColl_Scale_ScalarValue.Size = new Drawing.Size(79, 20);
			this.ManiColl_Scale_ScalarValue.TabIndex = 1;
			this.ManiColl_Scale_ScalarValue.Value = new decimal(new int[] { 1, 0, 0, 0 });
			this.ManiColl_Scale_ScalarValue.ValueChanged += new EventHandler(this.ManiColl_Scale_ScalarValue_ValueChanged);
			// 
			// ManiColl_Scale_XLabel
			// 
			this.ManiColl_Scale_XLabel.Anchor = AnchorStyles.Top;
			this.ManiColl_Scale_XLabel.AutoSize = true;
			this.ManiColl_Scale_XLabel.Location = new Drawing.Point(25, 21);
			this.ManiColl_Scale_XLabel.Name = "ManiColl_Scale_XLabel";
			this.ManiColl_Scale_XLabel.Size = new Drawing.Size(12, 13);
			this.ManiColl_Scale_XLabel.TabIndex = 2;
			this.ManiColl_Scale_XLabel.Text = "x";
			// 
			// ManiColl_Scale_ResetScale
			// 
			this.ManiColl_Scale_ResetScale.Anchor = AnchorStyles.Top;
			this.ManiColl_Scale_ResetScale.Location = new Drawing.Point(123, 17);
			this.ManiColl_Scale_ResetScale.Name = "ManiColl_Scale_ResetScale";
			this.ManiColl_Scale_ResetScale.Size = new Drawing.Size(66, 23);
			this.ManiColl_Scale_ResetScale.TabIndex = 1;
			this.ManiColl_Scale_ResetScale.Text = "Reset";
			this.ManiColl_Scale_ResetScale.UseVisualStyleBackColor = true;
			this.ManiColl_Scale_ResetScale.Click += new EventHandler(this.ManiColl_Scale_ResetScale_Click);
			// 
			// ManiColl_FlipGroup
			// 
			this.ManiColl_FlipGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.ManiColl_FlipGroup.Controls.Add(this.ManiColl_Flip_FlipY);
			this.ManiColl_FlipGroup.Controls.Add(this.ManiColl_Flip_FlipX);
			this.ManiColl_FlipGroup.Location = new Drawing.Point(4, 116);
			this.ManiColl_FlipGroup.Name = "ManiColl_FlipGroup";
			this.ManiColl_FlipGroup.Size = new Drawing.Size(218, 46);
			this.ManiColl_FlipGroup.TabIndex = 1;
			this.ManiColl_FlipGroup.TabStop = false;
			this.ManiColl_FlipGroup.Text = "Flip";
			// 
			// ManiColl_Flip_FlipY
			// 
			this.ManiColl_Flip_FlipY.AutoSize = true;
			this.ManiColl_Flip_FlipY.Location = new Drawing.Point(113, 19);
			this.ManiColl_Flip_FlipY.Name = "ManiColl_Flip_FlipY";
			this.ManiColl_Flip_FlipY.Size = new Drawing.Size(74, 17);
			this.ManiColl_Flip_FlipY.TabIndex = 2;
			this.ManiColl_Flip_FlipY.Text = "Flip Y-Axis";
			this.ManiColl_Flip_FlipY.UseVisualStyleBackColor = true;
			this.ManiColl_Flip_FlipY.CheckedChanged += new EventHandler(this.ManiColl_Flip_FlipY_CheckedChanged);
			// 
			// ManiColl_Flip_FlipX
			// 
			this.ManiColl_Flip_FlipX.AutoSize = true;
			this.ManiColl_Flip_FlipX.Location = new Drawing.Point(12, 19);
			this.ManiColl_Flip_FlipX.Name = "ManiColl_Flip_FlipX";
			this.ManiColl_Flip_FlipX.Size = new Drawing.Size(74, 17);
			this.ManiColl_Flip_FlipX.TabIndex = 2;
			this.ManiColl_Flip_FlipX.Text = "Flip X-Axis";
			this.ManiColl_Flip_FlipX.UseVisualStyleBackColor = true;
			this.ManiColl_Flip_FlipX.CheckedChanged += new EventHandler(this.ManiColl_Flip_FlipX_CheckedChanged);
			// 
			// ManiColl_RotateGroup
			// 
			this.ManiColl_RotateGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.ManiColl_RotateGroup.Controls.Add(this.ManiColl_Rotate_DegreesLabel);
			this.ManiColl_RotateGroup.Controls.Add(this.ManiColl_Rotate_DegreesValue);
			this.ManiColl_RotateGroup.Controls.Add(this.ManiColl_Rotate_SetTo0);
			this.ManiColl_RotateGroup.Controls.Add(this.ManiColl_Rotate_SetTo90);
			this.ManiColl_RotateGroup.Controls.Add(this.ManiColl_Rotate_SetTo180);
			this.ManiColl_RotateGroup.Controls.Add(this.ManiColl_Rotate_SetTo270);
			this.ManiColl_RotateGroup.Controls.Add(this.ManiColl_Rotate_SetRotationLocation);
			this.ManiColl_RotateGroup.Location = new Drawing.Point(4, 4);
			this.ManiColl_RotateGroup.Name = "ManiColl_RotateGroup";
			this.ManiColl_RotateGroup.Size = new Drawing.Size(218, 108);
			this.ManiColl_RotateGroup.TabIndex = 0;
			this.ManiColl_RotateGroup.TabStop = false;
			this.ManiColl_RotateGroup.Text = "Rotate";
			// 
			// ManiColl_Rotate_DegreesValue
			// 
			this.ManiColl_Rotate_DegreesValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.ManiColl_Rotate_DegreesValue.DecimalPlaces = 3;
			this.ManiColl_Rotate_DegreesValue.Location = new Drawing.Point(145, 0);
			this.ManiColl_Rotate_DegreesValue.Minimum = new decimal(-1);
			this.ManiColl_Rotate_DegreesValue.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
			this.ManiColl_Rotate_DegreesValue.Name = "ManiColl_Rotate_DegreesValue";
			this.ManiColl_Rotate_DegreesValue.Size = new Drawing.Size(62, 20);
			this.ManiColl_Rotate_DegreesValue.TabIndex = 1;
			this.ManiColl_Rotate_DegreesValue.ValueChanged += new EventHandler(this.ManiColl_Rotate_DegreesValue_ValueChanged);
			// 
			// ManiColl_Rotate_DegreesLabel
			// 
			this.ManiColl_Rotate_DegreesLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.ManiColl_Rotate_DegreesLabel.AutoSize = true;
			this.ManiColl_Rotate_DegreesLabel.Location = new Drawing.Point(95, 1);
			this.ManiColl_Rotate_DegreesLabel.Name = "ManiColl_Rotate_DegreesLabel";
			this.ManiColl_Rotate_DegreesLabel.Size = new Drawing.Size(50, 13);
			this.ManiColl_Rotate_DegreesLabel.TabIndex = 2;
			this.ManiColl_Rotate_DegreesLabel.Text = "Degrees:";
			this.ManiColl_Rotate_DegreesLabel.BackColor = this.ManiColl_RotateGroup.BackColor;
			// 
			// ManiColl_Rotate_SetTo0
			// 
			this.ManiColl_Rotate_SetTo0.Location = new Drawing.Point(52, 20);
			this.ManiColl_Rotate_SetTo0.Name = "ManiColl_Rotate_SetTo0";
			this.ManiColl_Rotate_SetTo0.Size = new Drawing.Size(38, 23);
			this.ManiColl_Rotate_SetTo0.TabIndex = 1;
			this.ManiColl_Rotate_SetTo0.Text = "0º";
			this.ManiColl_Rotate_SetTo0.UseVisualStyleBackColor = true;
			this.ManiColl_Rotate_SetTo0.Click += new EventHandler(this.ManiColl_Rotate_SetTo0_Click);
			// 
			// ManiColl_Rotate_SetTo90
			// 
			this.ManiColl_Rotate_SetTo90.Location = new Drawing.Point(93, 46);
			this.ManiColl_Rotate_SetTo90.Name = "ManiColl_Rotate_SetTo90";
			this.ManiColl_Rotate_SetTo90.Size = new Drawing.Size(38, 23);
			this.ManiColl_Rotate_SetTo90.TabIndex = 1;
			this.ManiColl_Rotate_SetTo90.Text = "90º";
			this.ManiColl_Rotate_SetTo90.UseVisualStyleBackColor = true;
			this.ManiColl_Rotate_SetTo90.Click += new EventHandler(this.ManiColl_Rotate_SetTo90_Click);
			// 
			// ManiColl_Rotate_SetTo180
			// 
			this.ManiColl_Rotate_SetTo180.Location = new Drawing.Point(52, 72);
			this.ManiColl_Rotate_SetTo180.Name = "ManiColl_Rotate_SetTo180";
			this.ManiColl_Rotate_SetTo180.Size = new Drawing.Size(38, 23);
			this.ManiColl_Rotate_SetTo180.TabIndex = 1;
			this.ManiColl_Rotate_SetTo180.Text = "180º";
			this.ManiColl_Rotate_SetTo180.UseVisualStyleBackColor = true;
			this.ManiColl_Rotate_SetTo180.Click += new EventHandler(this.ManiColl_Rotate_SetTo180_Click);
			// 
			// ManiColl_Rotate_SetTo270
			// 
			this.ManiColl_Rotate_SetTo270.Location = new Drawing.Point(11, 46);
			this.ManiColl_Rotate_SetTo270.Name = "ManiColl_Rotate_SetTo270";
			this.ManiColl_Rotate_SetTo270.Size = new Drawing.Size(38, 23);
			this.ManiColl_Rotate_SetTo270.TabIndex = 1;
			this.ManiColl_Rotate_SetTo270.Text = "270º";
			this.ManiColl_Rotate_SetTo270.UseVisualStyleBackColor = true;
			this.ManiColl_Rotate_SetTo270.Click += new EventHandler(this.ManiColl_Rotate_SetTo270_Click);
			// 
			// ManiColl_Rotate_SetRotationLocation
			// 
			this.ManiColl_Rotate_SetRotationLocation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.ManiColl_Rotate_SetRotationLocation.Location = new Drawing.Point(142, 37);
			this.ManiColl_Rotate_SetRotationLocation.Name = "ManiColl_Rotate_SetRotationLocation";
			this.ManiColl_Rotate_SetRotationLocation.Size = new Drawing.Size(66, 48);
			this.ManiColl_Rotate_SetRotationLocation.TabIndex = 1;
			this.ManiColl_Rotate_SetRotationLocation.Text = "Set Rotation Point";
			this.ManiColl_Rotate_SetRotationLocation.UseVisualStyleBackColor = true;
			this.ManiColl_Rotate_SetRotationLocation.Click += new EventHandler(this.ManiColl_Rotate_SetRotationLocation_Click);
			this.pasteOptions_tipper.SetToolTip(this.ManiColl_Rotate_SetRotationLocation, "Set a rotation point for this collision.");
			// 
			// advancedPO_TabC_AllCollsProps
			// 
			this.advancedPO_TabC_AllCollsProps.Controls.Add(this.advancedPO_TabC_AllCollsProps_Panel);
			this.advancedPO_TabC_AllCollsProps.Controls.Add(this.ACP_ResetValuesToDef);
			this.advancedPO_TabC_AllCollsProps.Location = new Drawing.Point(4, 22);
			this.advancedPO_TabC_AllCollsProps.Name = "advancedPO_TabC_AllCollsProps";
			this.advancedPO_TabC_AllCollsProps.Padding = new Padding(3);
			this.advancedPO_TabC_AllCollsProps.Size = new Drawing.Size(228, 381);
			this.advancedPO_TabC_AllCollsProps.TabIndex = 1;
			this.advancedPO_TabC_AllCollsProps.Text = "All Collision Properties";
			this.advancedPO_TabC_AllCollsProps.UseVisualStyleBackColor = true;
			// 
			// advancedPO_TabC_AllCollsProps_Panel
			// 
			this.advancedPO_TabC_AllCollsProps_Panel.AutoScroll = true;
			this.advancedPO_TabC_AllCollsProps_Panel.AutoScrollMargin = new Drawing.Size(0, 3);
			this.advancedPO_TabC_AllCollsProps_Panel.Controls.Add(this.AllCollsProps_WarningLink);
			this.advancedPO_TabC_AllCollsProps_Panel.Controls.Add(this.ACP_UnkFlagsGroup);
			this.advancedPO_TabC_AllCollsProps_Panel.Controls.Add(this.ACP_CollFlagsGroup);
			this.advancedPO_TabC_AllCollsProps_Panel.Controls.Add(this.ACP_TypeComboBox);
			this.advancedPO_TabC_AllCollsProps_Panel.Controls.Add(this.ACP_CollTargetsGroup);
			this.advancedPO_TabC_AllCollsProps_Panel.Controls.Add(this.ACP_MaterialComboBox);
			this.advancedPO_TabC_AllCollsProps_Panel.Controls.Add(this.ACP_Material_SelectedOverrides);
			this.advancedPO_TabC_AllCollsProps_Panel.Controls.Add(this.ACP_TypeLabel);
			this.advancedPO_TabC_AllCollsProps_Panel.Controls.Add(this.ACP_Type_SelectedOverrides);
			this.advancedPO_TabC_AllCollsProps_Panel.Controls.Add(this.ACP_MaterialLabel);
			this.advancedPO_TabC_AllCollsProps_Panel.Dock = DockStyle.Fill;
			this.advancedPO_TabC_AllCollsProps_Panel.Location = new Drawing.Point(3, 3);
			this.advancedPO_TabC_AllCollsProps_Panel.Name = "advancedPO_TabC_AllCollsProps_Panel";
			this.advancedPO_TabC_AllCollsProps_Panel.Size = new Drawing.Size(222, 352);
			this.advancedPO_TabC_AllCollsProps_Panel.TabIndex = 6;
			// 
			// AllCollsProps_WarningLink
			// 
			this.AllCollsProps_WarningLink.AutoSize = false;
			this.AllCollsProps_WarningLink.Dock = DockStyle.Top;
			this.AllCollsProps_WarningLink.Location = new Drawing.Point(0, 0);
			this.AllCollsProps_WarningLink.Name = "AllCollsProps_WarningLink";
			this.AllCollsProps_WarningLink.Size = new Drawing.Size(222, 43);
			this.AllCollsProps_WarningLink.TabIndex = 6;
			this.AllCollsProps_WarningLink.TabStop = true;
			this.AllCollsProps_WarningLink.Text = "Note: This will replace all of the collision properties that were copied. If you do not want this, press \"Reset\". More info...";
			this.AllCollsProps_WarningLink.LinkClicked += new LinkLabelLinkClickedEventHandler(this.AllCollsProps_WarningLink_LinkClicked);
			// 
			// ACP_UnkFlagsGroup
			// 
			this.ACP_UnkFlagsGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.ACP_UnkFlagsGroup.Controls.Add(this.ACP_UnkFlags_Unk1);
			this.ACP_UnkFlagsGroup.Controls.Add(this.ACP_UnkFlags_Unk2);
			this.ACP_UnkFlagsGroup.Controls.Add(this.ACP_UnkFlags_SuperSoft);
			this.ACP_UnkFlagsGroup.Controls.Add(this.ACP_UnkFlags_Unk4);
			this.ACP_UnkFlagsGroup.Controls.Add(this.ACP_UnkFlags_SelectedOverrides);
			this.ACP_UnkFlagsGroup.Location = new Drawing.Point(6, 288);
			this.ACP_UnkFlagsGroup.Name = "ACP_UnkFlagsGroup";
			this.ACP_UnkFlagsGroup.Size = new Drawing.Size(211, 58);
			this.ACP_UnkFlagsGroup.TabIndex = 5;
			this.ACP_UnkFlagsGroup.TabStop = false;
			this.ACP_UnkFlagsGroup.Text = "Unknown Flags";
			// 
			// ACP_UnkFlags_Unk1
			// 
			this.ACP_UnkFlags_Unk1.AutoSize = true;
			this.ACP_UnkFlags_Unk1.Location = new Drawing.Point(7, 18);
			this.ACP_UnkFlags_Unk1.Name = "ACP_UnkFlags_Unk1";
			this.ACP_UnkFlags_Unk1.Size = new Drawing.Size(81, 17);
			this.ACP_UnkFlags_Unk1.TabIndex = 4;
			this.ACP_UnkFlags_Unk1.Text = "Unknown 1";
			this.ACP_UnkFlags_Unk1.UseVisualStyleBackColor = true;
			this.ACP_UnkFlags_Unk1.CheckedChanged += new EventHandler(this.ACP_UnkFlags_Unk1_CheckedChanged);
			// 
			// ACP_UnkFlags_Unk2
			// 
			this.ACP_UnkFlags_Unk2.AutoSize = true;
			this.ACP_UnkFlags_Unk2.Location = new Drawing.Point(97, 18);
			this.ACP_UnkFlags_Unk2.Name = "ACP_UnkFlags_Unk2";
			this.ACP_UnkFlags_Unk2.Size = new Drawing.Size(81, 17);
			this.ACP_UnkFlags_Unk2.TabIndex = 4;
			this.ACP_UnkFlags_Unk2.Text = "Unknown 2";
			this.ACP_UnkFlags_Unk2.UseVisualStyleBackColor = true;
			this.ACP_UnkFlags_Unk2.CheckedChanged += new EventHandler(this.ACP_UnkFlags_Unk2_CheckedChanged);
			// 
			// ACP_UnkFlags_SuperSoft
			// 
			this.ACP_UnkFlags_SuperSoft.AutoSize = true;
			this.ACP_UnkFlags_SuperSoft.Location = new Drawing.Point(7, 36);
			this.ACP_UnkFlags_SuperSoft.Name = "ACP_UnkFlags_SuperSoft";
			this.ACP_UnkFlags_SuperSoft.Size = new Drawing.Size(81, 17);
			this.ACP_UnkFlags_SuperSoft.TabIndex = 4;
			this.ACP_UnkFlags_SuperSoft.Text = "Super Soft";
			this.ACP_UnkFlags_SuperSoft.UseVisualStyleBackColor = true;
			this.ACP_UnkFlags_SuperSoft.CheckedChanged += new EventHandler(this.ACP_UnkFlags_SuperSoft_CheckedChanged);
			// 
			// ACP_UnkFlags_Unk4
			// 
			this.ACP_UnkFlags_Unk4.AutoSize = true;
			this.ACP_UnkFlags_Unk4.Location = new Drawing.Point(97, 36);
			this.ACP_UnkFlags_Unk4.Name = "ACP_UnkFlags_Unk4";
			this.ACP_UnkFlags_Unk4.Size = new Drawing.Size(81, 17);
			this.ACP_UnkFlags_Unk4.TabIndex = 4;
			this.ACP_UnkFlags_Unk4.Text = "Unknown 4";
			this.ACP_UnkFlags_Unk4.UseVisualStyleBackColor = true;
			this.ACP_UnkFlags_Unk4.CheckedChanged += new EventHandler(this.ACP_UnkFlags_Unk4_CheckedChanged);
			// 
			// ACP_UnkFlags_SelectedOverrides
			// 
			this.ACP_UnkFlags_SelectedOverrides.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.ACP_UnkFlags_SelectedOverrides.AutoSize = true;
			this.ACP_UnkFlags_SelectedOverrides.Checked = false;
			this.ACP_UnkFlags_SelectedOverrides.CheckState = CheckState.Unchecked;
			this.ACP_UnkFlags_SelectedOverrides.Location = new Drawing.Point(191, 0);
			this.ACP_UnkFlags_SelectedOverrides.Name = "ACP_UnkFlags_SelectedOverrides";
			this.ACP_UnkFlags_SelectedOverrides.Size = new Drawing.Size(15, 14);
			this.ACP_UnkFlags_SelectedOverrides.TabIndex = 4;
			this.ACP_UnkFlags_SelectedOverrides.UseVisualStyleBackColor = true;
			this.ACP_UnkFlags_SelectedOverrides.CheckedChanged += new EventHandler(this.ACP_UnkFlags_SelectedOverrides_CheckedChanged);
			this.pasteOptions_tipper.SetToolTip(this.ACP_UnkFlags_SelectedOverrides, "If checked, selected/nonselected unknown values will override all copied properties\' unknown values.");
			// 
			// ACP_CollFlagsGroup
			// 
			this.ACP_CollFlagsGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.ACP_CollFlagsGroup.Controls.Add(this.ACP_CollFlags_NoWallJump);
			this.ACP_CollFlagsGroup.Controls.Add(this.ACP_CollFlags_Rotating);
			this.ACP_CollFlagsGroup.Controls.Add(this.ACP_CollFlags_LeftLedge);
			this.ACP_CollFlagsGroup.Controls.Add(this.ACP_CollFlags_RightLedge);
			this.ACP_CollFlagsGroup.Controls.Add(this.ACP_CollFlags_FallThrough);
			this.ACP_CollFlagsGroup.Controls.Add(this.ACP_CollFlags_SelectedOverrides);
			this.ACP_CollFlagsGroup.Location = new Drawing.Point(6, 205);
			this.ACP_CollFlagsGroup.Name = "ACP_CollFlagsGroup";
			this.ACP_CollFlagsGroup.Size = new Drawing.Size(211, 77);
			this.ACP_CollFlagsGroup.TabIndex = 5;
			this.ACP_CollFlagsGroup.TabStop = false;
			this.ACP_CollFlagsGroup.Text = "Collision Flags";
			// 
			// ACP_CollFlags_NoWallJump
			// 
			this.ACP_CollFlags_NoWallJump.AutoSize = true;
			this.ACP_CollFlags_NoWallJump.Location = new Drawing.Point(97, 37);
			this.ACP_CollFlags_NoWallJump.Name = "ACP_CollFlags_NoWallJump";
			this.ACP_CollFlags_NoWallJump.Size = new Drawing.Size(86, 17);
			this.ACP_CollFlags_NoWallJump.TabIndex = 4;
			this.ACP_CollFlags_NoWallJump.Text = "No Walljump";
			this.ACP_CollFlags_NoWallJump.UseVisualStyleBackColor = true;
			this.ACP_CollFlags_NoWallJump.CheckedChanged += new EventHandler(this.ACP_CollFlags_NoWallJump_CheckedChanged);
			// 
			// ACP_CollFlags_Rotating
			// 
			this.ACP_CollFlags_Rotating.AutoSize = true;
			this.ACP_CollFlags_Rotating.Location = new Drawing.Point(6, 55);
			this.ACP_CollFlags_Rotating.Name = "ACP_CollFlags_Rotating";
			this.ACP_CollFlags_Rotating.Size = new Drawing.Size(66, 17);
			this.ACP_CollFlags_Rotating.TabIndex = 4;
			this.ACP_CollFlags_Rotating.Text = "Rotating";
			this.ACP_CollFlags_Rotating.UseVisualStyleBackColor = true;
			this.ACP_CollFlags_Rotating.CheckedChanged += new EventHandler(this.ACP_CollFlags_Rotating_CheckedChanged);
			// 
			// ACP_CollFlags_LeftLedge
			// 
			this.ACP_CollFlags_LeftLedge.AutoSize = true;
			this.ACP_CollFlags_LeftLedge.Location = new Drawing.Point(97, 19);
			this.ACP_CollFlags_LeftLedge.Name = "ACP_CollFlags_LeftLedge";
			this.ACP_CollFlags_LeftLedge.Size = new Drawing.Size(77, 17);
			this.ACP_CollFlags_LeftLedge.TabIndex = 4;
			this.ACP_CollFlags_LeftLedge.Text = "Left Ledge";
			this.ACP_CollFlags_LeftLedge.UseVisualStyleBackColor = true;
			this.ACP_CollFlags_LeftLedge.CheckedChanged += new EventHandler(this.ACP_CollFlags_LeftLedge_CheckedChanged);
			// 
			// ACP_CollFlags_RightLedge
			// 
			this.ACP_CollFlags_RightLedge.AutoSize = true;
			this.ACP_CollFlags_RightLedge.Location = new Drawing.Point(6, 37);
			this.ACP_CollFlags_RightLedge.Name = "ACP_CollFlags_RightLedge";
			this.ACP_CollFlags_RightLedge.Size = new Drawing.Size(84, 17);
			this.ACP_CollFlags_RightLedge.TabIndex = 4;
			this.ACP_CollFlags_RightLedge.Text = "Right Ledge";
			this.ACP_CollFlags_RightLedge.UseVisualStyleBackColor = true;
			this.ACP_CollFlags_RightLedge.CheckedChanged += new EventHandler(this.ACP_CollFlags_RightLedge_CheckedChanged);
			// 
			// ACP_CollFlags_FallThrough
			// 
			this.ACP_CollFlags_FallThrough.AutoSize = true;
			this.ACP_CollFlags_FallThrough.Location = new Drawing.Point(6, 19);
			this.ACP_CollFlags_FallThrough.Name = "ACP_CollFlags_FallThrough";
			this.ACP_CollFlags_FallThrough.Size = new Drawing.Size(85, 17);
			this.ACP_CollFlags_FallThrough.TabIndex = 4;
			this.ACP_CollFlags_FallThrough.Text = "Fall Through";
			this.ACP_CollFlags_FallThrough.UseVisualStyleBackColor = true;
			this.ACP_CollFlags_FallThrough.CheckedChanged += new EventHandler(this.ACP_CollFlags_FallThrough_CheckedChanged);
			// 
			// ACP_CollFlags_SelectedOverrides
			// 
			this.ACP_CollFlags_SelectedOverrides.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.ACP_CollFlags_SelectedOverrides.AutoSize = true;
			this.ACP_CollFlags_SelectedOverrides.Checked = false;
			this.ACP_CollFlags_SelectedOverrides.CheckState = CheckState.Unchecked;
			this.ACP_CollFlags_SelectedOverrides.Location = new Drawing.Point(191, 1);
			this.ACP_CollFlags_SelectedOverrides.Name = "ACP_CollFlags_SelectedOverrides";
			this.ACP_CollFlags_SelectedOverrides.Size = new Drawing.Size(15, 14);
			this.ACP_CollFlags_SelectedOverrides.TabIndex = 4;
			this.ACP_CollFlags_SelectedOverrides.UseVisualStyleBackColor = true;
			this.ACP_CollFlags_SelectedOverrides.CheckedChanged += new EventHandler(this.ACP_CollFlags_SelectedOverrides_CheckedChanged);
			this.pasteOptions_tipper.SetToolTip(this.ACP_CollFlags_SelectedOverrides, "If checked, selected/nonselected collision flags will override all copied properties\' collision flags.");
			// 
			// ACP_Type_SelectedOverrides
			// 
			this.ACP_Type_SelectedOverrides.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.ACP_Type_SelectedOverrides.AutoSize = true;
			this.ACP_Type_SelectedOverrides.Checked = false;
			this.ACP_Type_SelectedOverrides.CheckState = CheckState.Unchecked;
			this.ACP_Type_SelectedOverrides.Location = new Drawing.Point(202, 70);
			this.ACP_Type_SelectedOverrides.Name = "ACP_Type_SelectedOverrides";
			this.ACP_Type_SelectedOverrides.Size = new Drawing.Size(15, 14);
			this.ACP_Type_SelectedOverrides.TabIndex = 4;
			this.ACP_Type_SelectedOverrides.UseVisualStyleBackColor = true;
			this.ACP_Type_SelectedOverrides.CheckedChanged += new EventHandler(this.ACP_Type_SelectedOverrides_CheckedChanged);
			this.pasteOptions_tipper.SetToolTip(this.ACP_Type_SelectedOverrides, "If checked, the selected type will override all copied properties\' type.");
			// 
			// ACP_TypeComboBox
			// 
			this.ACP_TypeComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.ACP_TypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.ACP_TypeComboBox.FormattingEnabled = true;
			this.ACP_TypeComboBox.Location = new Drawing.Point(6, 67);
			this.ACP_TypeComboBox.Name = "ACP_TypeComboBox";
			this.ACP_TypeComboBox.Size = new Drawing.Size(190, 21);
			this.ACP_TypeComboBox.TabIndex = 2;
			this.ACP_TypeComboBox.SelectedIndexChanged += new EventHandler(this.ACP_TypeComboBox_SelectedIndexChanged);
			// 
			// ACP_Material_SelectedOverrides
			// 
			this.ACP_Material_SelectedOverrides.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.ACP_Material_SelectedOverrides.AutoSize = true;
			this.ACP_Material_SelectedOverrides.Checked = false;
			this.ACP_Material_SelectedOverrides.CheckState = CheckState.Unchecked;
			this.ACP_Material_SelectedOverrides.Location = new Drawing.Point(202, 113);
			this.ACP_Material_SelectedOverrides.Name = "ACP_Material_SelectedOverrides";
			this.ACP_Material_SelectedOverrides.Size = new Drawing.Size(15, 14);
			this.ACP_Material_SelectedOverrides.TabIndex = 4;
			this.ACP_Material_SelectedOverrides.UseVisualStyleBackColor = true;
			this.ACP_Material_SelectedOverrides.CheckedChanged += new EventHandler(this.ACP_Material_SelectedOverrides_CheckedChanged);
			this.pasteOptions_tipper.SetToolTip(this.ACP_Material_SelectedOverrides, "If checked, the selected material will override all copied properties\' material.");
			// 
			// ACP_MaterialComboBox
			// 
			this.ACP_MaterialComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.ACP_MaterialComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.ACP_MaterialComboBox.FormattingEnabled = true;
			this.ACP_MaterialComboBox.Location = new Drawing.Point(6, 110);
			this.ACP_MaterialComboBox.Name = "ACP_MaterialComboBox";
			this.ACP_MaterialComboBox.Size = new Drawing.Size(190, 21);
			this.ACP_MaterialComboBox.TabIndex = 2;
			this.ACP_MaterialComboBox.SelectedIndexChanged += new EventHandler(this.ACP_MaterialComboBox_SelectedIndexChanged);
			// 
			// ACP_CollTargetsGroup
			// 
			this.ACP_CollTargetsGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.ACP_CollTargetsGroup.Controls.Add(this.ACP_CollTargets_Items);
			this.ACP_CollTargetsGroup.Controls.Add(this.ACP_CollTargets_PKMNTrainer);
			this.ACP_CollTargetsGroup.Controls.Add(this.ACP_CollTargets_Everything);
			this.ACP_CollTargetsGroup.Controls.Add(this.ACP_CollTargets_SelectedOverrides);
			this.ACP_CollTargetsGroup.Location = new Drawing.Point(6, 139);
			this.ACP_CollTargetsGroup.Name = "ACP_CollTargetsGroup";
			this.ACP_CollTargetsGroup.Size = new Drawing.Size(211, 60);
			this.ACP_CollTargetsGroup.TabIndex = 5;
			this.ACP_CollTargetsGroup.TabStop = false;
			this.ACP_CollTargetsGroup.Text = "Collision Targets?";
			// 
			// ACP_CollTargets_SelectedOverrides
			// 
			this.ACP_CollTargets_SelectedOverrides.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.ACP_CollTargets_SelectedOverrides.AutoSize = true;
			this.ACP_CollTargets_SelectedOverrides.Checked = false;
			this.ACP_CollTargets_SelectedOverrides.CheckState = CheckState.Unchecked;
			this.ACP_CollTargets_SelectedOverrides.Location = new Drawing.Point(191, 0);
			this.ACP_CollTargets_SelectedOverrides.Name = "ACP_CollTargets_SelectedOverrides";
			this.ACP_CollTargets_SelectedOverrides.Size = new Drawing.Size(15, 14);
			this.ACP_CollTargets_SelectedOverrides.TabIndex = 4;
			this.ACP_CollTargets_SelectedOverrides.UseVisualStyleBackColor = true;
			this.ACP_CollTargets_SelectedOverrides.CheckedChanged += new EventHandler(this.ACP_CollTargets_SelectedOverrides_CheckedChanged);
			this.pasteOptions_tipper.SetToolTip(this.ACP_CollTargets_SelectedOverrides, "If checked, the selected collision targets will override all copied properties\' collision targets.");
			// 
			// ACP_CollTargets_Everything
			// 
			this.ACP_CollTargets_Everything.AutoSize = true;
			this.ACP_CollTargets_Everything.Location = new Drawing.Point(6, 19);
			this.ACP_CollTargets_Everything.Name = "ACP_CollTargets_Everything";
			this.ACP_CollTargets_Everything.Size = new Drawing.Size(76, 17);
			this.ACP_CollTargets_Everything.TabIndex = 4;
			this.ACP_CollTargets_Everything.Text = "Everything";
			this.ACP_CollTargets_Everything.UseVisualStyleBackColor = true;
			this.ACP_CollTargets_Everything.CheckedChanged += new EventHandler(this.ACP_CollTargets_Everything_CheckedChanged);
			// 
			// ACP_CollTargets_Items
			// 
			this.ACP_CollTargets_Items.AutoSize = true;
			this.ACP_CollTargets_Items.Location = new Drawing.Point(113, 19);
			this.ACP_CollTargets_Items.Name = "ACP_CollTargets_Items";
			this.ACP_CollTargets_Items.Size = new Drawing.Size(51, 17);
			this.ACP_CollTargets_Items.TabIndex = 4;
			this.ACP_CollTargets_Items.Text = "Items";
			this.ACP_CollTargets_Items.UseVisualStyleBackColor = true;
			this.ACP_CollTargets_Items.CheckedChanged += new EventHandler(this.ACP_CollTargets_Items_CheckedChanged);
			// 
			// ACP_CollTargets_PKMNTrainer
			// 
			this.ACP_CollTargets_PKMNTrainer.AutoSize = true;
			this.ACP_CollTargets_PKMNTrainer.Location = new Drawing.Point(6, 38);
			this.ACP_CollTargets_PKMNTrainer.Name = "ACP_CollTargets_PKMNTrainer";
			this.ACP_CollTargets_PKMNTrainer.Size = new Drawing.Size(107, 17);
			this.ACP_CollTargets_PKMNTrainer.TabIndex = 4;
			this.ACP_CollTargets_PKMNTrainer.Text = "Pokémon Trainer";
			this.ACP_CollTargets_PKMNTrainer.UseVisualStyleBackColor = true;
			this.ACP_CollTargets_PKMNTrainer.CheckedChanged += new EventHandler(this.ACP_CollTargets_PKMNTrainer_CheckedChanged);
			// 
			// ACP_TypeLabel
			// 
			this.ACP_TypeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.ACP_TypeLabel.Location = new Drawing.Point(6, 51);
			this.ACP_TypeLabel.Name = "ACP_TypeLabel";
			this.ACP_TypeLabel.Size = new Drawing.Size(206, 13);
			this.ACP_TypeLabel.TabIndex = 3;
			this.ACP_TypeLabel.Text = "Type";
			// 
			// ACP_MaterialLabel
			// 
			this.ACP_MaterialLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.ACP_MaterialLabel.Location = new Drawing.Point(6, 94);
			this.ACP_MaterialLabel.Name = "ACP_MaterialLabel";
			this.ACP_MaterialLabel.Size = new Drawing.Size(206, 13);
			this.ACP_MaterialLabel.TabIndex = 3;
			this.ACP_MaterialLabel.Text = "Material";
			// 
			// ACP_ResetValuesToDef
			// 
			this.ACP_ResetValuesToDef.Dock = DockStyle.Bottom;
			this.ACP_ResetValuesToDef.Enabled = false;
			this.ACP_ResetValuesToDef.Location = new Drawing.Point(3, 355);
			this.ACP_ResetValuesToDef.Name = "ACP_ResetValuesToDef";
			this.ACP_ResetValuesToDef.Size = new Drawing.Size(222, 23);
			this.ACP_ResetValuesToDef.TabIndex = 1;
			this.ACP_ResetValuesToDef.Text = "Reset";
			this.ACP_ResetValuesToDef.UseVisualStyleBackColor = true;
			this.ACP_ResetValuesToDef.Click += new EventHandler(this.ACP_ResetValuesToDef_Click);
			// 
			// BottomPanel
			// 
			this.BottomPanel.Controls.Add(this.Cancel);
			this.BottomPanel.Controls.Add(this.PasteCollision);
			this.BottomPanel.Dock = DockStyle.Bottom;
			this.BottomPanel.Location = new Drawing.Point(0, 407);
			this.BottomPanel.Name = "BottomPanel";
			this.BottomPanel.Size = new Drawing.Size(236, 32);
			this.BottomPanel.TabIndex = 1;
			// 
			// PasteCollision
			// 
			this.PasteCollision.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.PasteCollision.Location = new Drawing.Point(49, 4);
			this.PasteCollision.Name = "PasteCollision";
			this.PasteCollision.Size = new Drawing.Size(104, 23);
			this.PasteCollision.TabIndex = 0;
			this.PasteCollision.Text = "Paste Collision";
			this.PasteCollision.UseVisualStyleBackColor = true;
			this.PasteCollision.Click += new EventHandler(this.PasteCollision_Click);
			// 
			// Cancel
			// 
			this.Cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.Cancel.DialogResult = DialogResult.Cancel;
			this.Cancel.Location = new Drawing.Point(156, 4);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new Drawing.Size(75, 23);
			this.Cancel.TabIndex = 1;
			this.Cancel.Text = "Cancel";
			this.Cancel.UseVisualStyleBackColor = true;
			this.Cancel.Click += new EventHandler(this.Cancel_Click);
			// 
			// BrawlCrate_PasteOptions_UI
			// 
			this.Name = "CollisionEditor_PasteOptions";
			this.Text = "Advanced Paste Options";
			this.CancelButton = this.Cancel;
			this.AutoScaleMode = AutoScaleMode.Font;
			this.AutoScaleDimensions = new SizeF(6F, 13F);
			this.ClientSize = new Drawing.Size(236, 439);
			this.MaximumSize = new Drawing.Size(252, 478);
			this.MinimumSize = new Drawing.Size(252, 252);
			this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
			this.Controls.Add(this.advancedPO_TabC);
			this.Controls.Add(this.BottomPanel);
			this.Icon = BrawlLib.Properties.Resources.Icon;

			this.advancedPO_TabC.ResumeLayout(false);
			this.advancedPO_TabC_ManipulateCollisionTab.ResumeLayout(false);
			this.ManiColl_PlacementGroup.ResumeLayout(false);
			this.ManiColl_PlacementGroup.PerformLayout();
			this.ManiColl_ScaleGroup.ResumeLayout(false);
			this.ManiColl_ScaleGroup.PerformLayout();
			((ISupportInitialize)this.ManiColl_Scale_ScalarValue).EndInit();
			this.ManiColl_FlipGroup.ResumeLayout(false);
			this.ManiColl_FlipGroup.PerformLayout();
			this.ManiColl_RotateGroup.ResumeLayout(false);
			this.ManiColl_RotateGroup.PerformLayout();
			((ISupportInitialize)this.ManiColl_Rotate_DegreesValue).EndInit();
			this.advancedPO_TabC_AllCollsProps.ResumeLayout(false);
			this.advancedPO_TabC_AllCollsProps_Panel.ResumeLayout(false);
			this.advancedPO_TabC_AllCollsProps_Panel.PerformLayout();
			this.ACP_UnkFlagsGroup.ResumeLayout(false);
			this.ACP_UnkFlagsGroup.PerformLayout();
			this.ACP_CollFlagsGroup.ResumeLayout(false);
			this.ACP_CollFlagsGroup.PerformLayout();
			this.ACP_CollTargetsGroup.ResumeLayout(false);
			this.ACP_CollTargetsGroup.PerformLayout();
			this.BottomPanel.ResumeLayout(false);

			this.ResumeLayout(false);

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
			parentEditor.PasteCopiedCollisions(true);
			Close();
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
				Vector2 ZeroPoint = (parentEditor.clipboardPasteOptions_ActualPointsValuesAreUsed.Checked ? point.Value : point.RawValue) - CenterRotationPoint;

				// Then start getting the distance.
				OriginalLinksDistance[d] = (float)Math.Sqrt((ZeroPoint._x * ZeroPoint._x) + (ZeroPoint._y * ZeroPoint._y));
				// This is an alternative, just wondering if this really makes any difference.
				//OriginalLinksDistance[d] = CenterPoint.TrueDistance((parentEditor.clipboardPasteOptions_ActualPointsValuesAreUsed.Checked ? point.Value : point.RawValue));

				// Then we get our radians.
				OriginalLinksRadians[d] = (float)Math.Atan2(ZeroPoint._y, ZeroPoint._x);
			}

			// Prevent references by cloning both original points distance and radians.
			LinksDistance = (float[])OriginalLinksDistance.Clone();
			LinksRadians = (float[])OriginalLinksRadians.Clone();

			UpdateLinksValues();
		}

		private void UpdateLinksRadians(double radians)
		{
			CurrentRadian = (float)radians;

			for (int r = copiedState.CopiedLinks.Length - 1; r >= 0; --r)
			{
				LinksRadians[r] = OriginalLinksRadians[r] - (float)radians;

				float X = (float)Math.Cos(LinksRadians[r]) * LinksDistance[r];
				float Y = (float)Math.Sin(LinksRadians[r]) * LinksDistance[r];
			}

			Updating = true;
			double degrees = RadiansToDegrees(radians);

			if (degrees < 0) { degrees = 360.0 + degrees; }
			else if (degrees >= 360) { degrees = degrees - 360; }

			ManiColl_Rotate_DegreesValue.Value = (decimal)degrees;
			Updating = false;

			UpdateLinksValues();
		}
		private void UpdateLinksDistance(double distance, bool scale)
		{
			if (!scale)
				CurrentDistance = (float)distance;

			for (int r = copiedState.CopiedLinks.Length - 1; r >= 0; --r)
			{
				if (scale)
					LinksDistance[r] = OriginalLinksDistance[r] * (float)distance;
				else
					LinksDistance[r] = OriginalLinksDistance[r] + (float)distance;
			}

			if (scale)
			{
				Updating = true;
				ManiColl_Scale_ScalarValue.Value = (decimal)distance;
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
				float X = (float)Math.Cos(LinksRadians[r]) * LinksDistance[r];
				float Y = (float)Math.Sin(LinksRadians[r]) * LinksDistance[r];

				if (ManiColl_Flip_FlipX.Checked) { X = -X; }
				if (ManiColl_Flip_FlipY.Checked) { Y = -Y; }

				Vector2 v = CenterRotationPoint + OffsetLocation + new Vector2(X, Y);
				CopiedLinkPointsNewLocation[r] = v;
			}

			if (length > tinkeringObject._points.Count)
				length = tinkeringObject._points.Count;

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

			var re = new CollisionObject.CollisionObjectRenderInfo(false, OnFocus, ref cam);
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
			Vector2 Point = (Vector2)Point3D;

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
				Opacity = 0.6f;
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
				return;

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
				return;

			UpdateLinksRadians(DegreesToRadians((double)ManiColl_Rotate_DegreesValue.Value));
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
				LocationForm = new CollisionEditor_PasteOptions_SetLocationForm(this, SetRotationLocation_RadioSelectedValue, CenterRotationPoint);
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
				UpdateLinksDistance((double)ManiColl_Scale_ScalarValue.Value, true);
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
				OffsetLocation = new Vector2(ManiColl_Placement_SpefPlacement_LocX.Value, ManiColl_Placement_SpefPlacement_LocY.Value);
				UpdateLinksValues();
			}
		}

		private void ManiColl_Placement_SpefPlacement_LocX_ValueChanged(object sender, EventArgs e)
		{
			OffsetLocation = new Vector2(ManiColl_Placement_SpefPlacement_LocX.Value, ManiColl_Placement_SpefPlacement_LocY.Value);
			UpdateLinksValues();
		}

		private void ManiColl_Placement_SpefPlacement_LocY_ValueChanged(object sender, EventArgs e)
		{
			OffsetLocation = new Vector2(ManiColl_Placement_SpefPlacement_LocX.Value, ManiColl_Placement_SpefPlacement_LocY.Value);
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
				return;

			Updating = true;

			var PlaneType = (CollisionPlaneType)ACP_TypeComboBox.SelectedItem;
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
				return;

			AllCollisionPropertiesValues.Material = ((CollisionTerrain)ACP_MaterialComboBox.SelectedItem).ID;
			EnableDisableResetButton();
		}

		#endregion

		#region Collision Targets Section
		private void ACP_CollTargets_Everything_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

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
				return;

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
				return;

			Updating = true;

			AllCollisionPropertiesValues.SetFlag2(CollisionPlaneFlags2.PokemonTrainer, ACP_CollTargets_PKMNTrainer.Checked);

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
				return;

			AllCollisionPropertiesValues.SetFlag(CollisionPlaneFlags.DropThrough, ACP_CollFlags_FallThrough.Checked);
			EnableDisableResetButton();

		}
		private void ACP_CollFlags_LeftLedge_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			Updating = true;

			if (ACP_CollFlags_NoWallJump.Checked)
				ACP_CollFlags_NoWallJump.Checked = false;

			Updating = false;

			AllCollisionPropertiesValues.SetFlag(CollisionPlaneFlags.LeftLedge, ACP_CollFlags_LeftLedge.Checked);
			EnableDisableResetButton();
		}
		private void ACP_CollFlags_RightLedge_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			Updating = true;

			if (ACP_CollFlags_NoWallJump.Checked)
				ACP_CollFlags_NoWallJump.Checked = false;

			Updating = false;

			AllCollisionPropertiesValues.SetFlag(CollisionPlaneFlags.RightLedge, ACP_CollFlags_RightLedge.Checked);
			EnableDisableResetButton();
		}
		private void ACP_CollFlags_NoWallJump_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

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
				return;

			AllCollisionPropertiesValues.SetFlag(CollisionPlaneFlags.Rotating, ACP_CollFlags_Rotating.Checked);
			EnableDisableResetButton();
		}
		#endregion

		#region Unknown Flags Section
		private void ACP_UnkFlags_Unk1_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			AllCollisionPropertiesValues.SetFlag2(CollisionPlaneFlags2.UnknownSSE, ACP_UnkFlags_Unk1.Checked);
			EnableDisableResetButton();
		}

		private void ACP_UnkFlags_Unk2_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			AllCollisionPropertiesValues.SetFlag(CollisionPlaneFlags.Unknown1, ACP_UnkFlags_Unk2.Checked);
			EnableDisableResetButton();
		}

		private void ACP_UnkFlags_SuperSoft_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			AllCollisionPropertiesValues.SetFlag(CollisionPlaneFlags.SuperSoft, ACP_UnkFlags_SuperSoft.Checked);
			EnableDisableResetButton();
		}

		private void ACP_UnkFlags_Unk4_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			AllCollisionPropertiesValues.SetFlag(CollisionPlaneFlags.Unknown4, ACP_UnkFlags_Unk4.Checked);
			EnableDisableResetButton();
		}
		#endregion

		#region SelectedOverrides part
		private void ACP_Type_SelectedOverrides_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			ACP_SelectedOverrides_ApplyOverrideSection(0, ACP_Type_SelectedOverrides.Checked);
		}
		private void ACP_Material_SelectedOverrides_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			ACP_SelectedOverrides_ApplyOverrideSection(1, ACP_Material_SelectedOverrides.Checked);
		}
		private void ACP_CollTargets_SelectedOverrides_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			ACP_SelectedOverrides_ApplyOverrideSection(2, ACP_CollTargets_SelectedOverrides.Checked);
		}
		private void ACP_CollFlags_SelectedOverrides_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			ACP_SelectedOverrides_ApplyOverrideSection(3, ACP_CollFlags_SelectedOverrides.Checked);
		}
		private void ACP_UnkFlags_SelectedOverrides_CheckedChanged(object sender, EventArgs e)
		{
			if (Updating)
				return;

			ACP_SelectedOverrides_ApplyOverrideSection(4, ACP_UnkFlags_SelectedOverrides.Checked);
		}

		// Since structure classes are not considered reference, the Values variable is first taken
		// then overriden and then after that we apply it to ExtraData again so that it is overwritten.
		private void ACP_SelectedOverrides_ApplyOverrideSection(int Index, bool Value)
		{
			bool[] Values = (bool[])AllCollisionPropertiesValues.ExtraData;

			Values[Index] = Value;

			AllCollisionPropertiesValues.ExtraData = Values;
			this.EnableDisableResetButton();
		}
		#endregion

		#region Miscellaneous
		private void EnableDisableResetButton()
		{
			ACP_ResetValuesToDef.Enabled =
			(
				(ACP_TypeComboBox.SelectedIndex > 0) ||
				(ACP_MaterialComboBox.SelectedIndex > 0) ||

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
				ACP_UnkFlags_Unk4.Checked
			);
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
				ExtraData = new bool[] { false, false, false, false, false },
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
			return (Math.PI * Angle) / 180.0;
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
				if (disposing && (components != null))
				{
					components.Dispose();
				}
				base.Dispose(disposing);
			}

			private void InitializeComponent()
			{
				this.components = new Container();
				this.Tips = new ToolTip(this.components);

				this.SpecificLocationX_Label = new Label();
				this.SpecificLocationY_Label = new Label();
				this.SpecificLocationX_Value = new NumericInputBox();
				this.SpecificLocationY_Value = new NumericInputBox();

				this.SpecificLocationRadio_SpecificPlacement = new RadioButton();
				this.SpecificLocationRadio_FreePlacement = new RadioButton();
				this.SpecificLocationRadio_ZeroZero = new RadioButton();

				this.Button_SetLocation = new Button();
				this.Button_ResetToOriginalValue = new Button();
				this.Button_Cancel = new Button();


				this.SuspendLayout();

				// 
				// SpecificLocationX_Label
				//
				this.SpecificLocationX_Label = this.CreateNumericalLabel(new Drawing.Point(30, 110), new Drawing.Size(20, 20), "SpecificLocationX_Label", "X");
				this.SpecificLocationX_Label.TabIndex = 4;
				// 
				// SpecificLocationY_Label
				// 
				this.SpecificLocationY_Label = this.CreateNumericalLabel(new Drawing.Point(30, 129), new Drawing.Size(20, 20), "SpecificLocationY_Label", "Y");
				this.SpecificLocationY_Label.TabIndex = 5;
				// 
				// SpecificLocationX_Value
				// 
				this.SpecificLocationX_Value = this.CreateNumericalInput(new Drawing.Point(49, 110), new Drawing.Size(155, 20), "SpecificLocationX_Value", "0");
				this.SpecificLocationX_Value.ValueChanged += new EventHandler(this.SpecificLocationX_Value_ValueChanged);
				this.SpecificLocationX_Value.TabIndex = 4;
				// 
				// SpecificLocationY_Value
				// 
				this.SpecificLocationY_Value = this.CreateNumericalInput(new Drawing.Point(49, 129), new Drawing.Size(155, 20), "SpecificLocationY_Value", "0");
				this.SpecificLocationY_Value.ValueChanged += new EventHandler(this.SpecificLocationY_Value_ValueChanged);
				this.SpecificLocationY_Value.TabIndex = 5;

				//
				// FreeLocationX_Label
				//
				this.FreeLocationX_Label = CreateNumericalLabel(this.SpecificLocationX_Label, "FreeLocationX_Label");
				this.FreeLocationX_Label.Location = new Drawing.Point(30, 50);
				this.FreeLocationX_Label.TabIndex = 7;
				//
				// FreeLocationY_Label
				//
				this.FreeLocationY_Label = CreateNumericalLabel(this.SpecificLocationY_Label, "FreeLocationY_Label");
				this.FreeLocationY_Label.Location = new Drawing.Point(30, 69);
				this.FreeLocationY_Label.TabIndex = 8;
				//
				// FreeLocationX_Value
				//
				this.FreeLocationX_Value = CreateNumericalInput(this.SpecificLocationX_Value, "FreeLocationX_Value");
				this.FreeLocationX_Value.ValueChanged += new EventHandler(this.FreeLocationX_Value_ValueChanged);
				this.FreeLocationX_Value.Location = new Drawing.Point(49, 50);
				this.FreeLocationX_Value.TabIndex = 7;
				this.FreeLocationX_Value.ReadOnly = true;
				//
				// FreeLocationY_Value
				//
				this.FreeLocationY_Value = CreateNumericalInput(this.SpecificLocationY_Value, "FreeLocationY_Value");
				this.FreeLocationY_Value.ValueChanged += new EventHandler(this.FreeLocationY_Value_ValueChanged);
				this.FreeLocationY_Value.Location = new Drawing.Point(49, 69);
				this.FreeLocationY_Value.TabIndex = 8;
				this.FreeLocationY_Value.ReadOnly = true;


				// 
				// SpecificLocationRadio_SpecificPlacementSpecificLocationRadio_SpecificPlacement
				// 
				this.SpecificLocationRadio_SpecificPlacement.AutoSize = true;
				this.SpecificLocationRadio_SpecificPlacement.Checked = true;
				this.SpecificLocationRadio_SpecificPlacement.Location = new Drawing.Point(12, 91);
				this.SpecificLocationRadio_SpecificPlacement.Name = "ManiColl_Placement_RadioSpefPlacement";
				this.SpecificLocationRadio_SpecificPlacement.Size = new Drawing.Size(113, 17);
				this.SpecificLocationRadio_SpecificPlacement.TabIndex = 3;
				this.SpecificLocationRadio_SpecificPlacement.Text = "Specified Location";
				this.SpecificLocationRadio_SpecificPlacement.UseVisualStyleBackColor = true;
				this.SpecificLocationRadio_SpecificPlacement.CheckedChanged += new EventHandler(this.SpecificLocationRadio_SpecificPlacement_CheckedChanged);
				this.Tips.SetToolTip(this.SpecificLocationRadio_SpecificPlacement, "If you like being specific, you can specify the location by inputting the two values.");
				// 
				// SpecificLocationRadio_FreePlacement
				// 
				this.SpecificLocationRadio_FreePlacement.AutoSize = true;
				this.SpecificLocationRadio_FreePlacement.Location = new Drawing.Point(12, 31);
				this.SpecificLocationRadio_FreePlacement.Name = "SpecificLocationRadio_FreePlacement";
				this.SpecificLocationRadio_FreePlacement.Size = new Drawing.Size(72, 17);
				this.SpecificLocationRadio_FreePlacement.TabIndex = 6;
				this.SpecificLocationRadio_FreePlacement.Text = "Freeplace";
				this.SpecificLocationRadio_FreePlacement.UseVisualStyleBackColor = true;
				this.SpecificLocationRadio_FreePlacement.CheckedChanged += new EventHandler(this.SpecificLocationRadio_FreePlacement_CheckedChanged);
				this.Tips.SetToolTip(this.SpecificLocationRadio_FreePlacement, "If this is selected, you can move the center location of copied collisions anywhere.");
				// 
				// SpecificLocationRadio_ZeroZero
				// 
				this.SpecificLocationRadio_ZeroZero.AutoSize = true;
				this.SpecificLocationRadio_ZeroZero.Location = new Drawing.Point(12, 12);
				this.SpecificLocationRadio_ZeroZero.Size = new Drawing.Size(117, 17);
				this.SpecificLocationRadio_ZeroZero.TabIndex = 9;
				this.SpecificLocationRadio_ZeroZero.TabStop = true;
				this.SpecificLocationRadio_ZeroZero.Name = "SpecificLocationRadio_ZeroZero";
				this.SpecificLocationRadio_ZeroZero.Text = "Center (0, 0)";
				this.SpecificLocationRadio_ZeroZero.UseVisualStyleBackColor = true;
				this.SpecificLocationRadio_ZeroZero.CheckedChanged += new EventHandler(this.SpecificLocationRadio_ZeroZero_CheckedChanged);
				this.Tips.SetToolTip(this.SpecificLocationRadio_ZeroZero, "It is placed at the center of the stage (0, 0).");


				// 
				// Button_SetLocation
				// 
				this.Button_SetLocation.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
				this.Button_SetLocation.Location = new Drawing.Point(6, 164);
				this.Button_SetLocation.Size = new Drawing.Size(224, 23);
				this.Button_SetLocation.TabIndex = 0;
				this.Button_SetLocation.Name = "Button_SetLocation";
				this.Button_SetLocation.Text = "Set Location";
				this.Button_SetLocation.UseVisualStyleBackColor = true;
				this.Button_SetLocation.DialogResult = DialogResult.OK;
				this.Button_SetLocation.Click += new EventHandler(this.Button_SetLocation_Click);
				// 
				// Button_ResetToOriginalValue
				// 
				this.Button_ResetToOriginalValue.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
				this.Button_ResetToOriginalValue.Location = new Drawing.Point(6, 190);
				this.Button_ResetToOriginalValue.Size = new Drawing.Size(110, 23);
				this.Button_ResetToOriginalValue.TabIndex = 1;
				this.Button_ResetToOriginalValue.Name = "Button_ResetToOriginalValue";
				this.Button_ResetToOriginalValue.Text = "Reset Value";
				this.Button_ResetToOriginalValue.UseVisualStyleBackColor = true;
				this.Button_ResetToOriginalValue.Click += new EventHandler(this.Button_ResetToOriginalValue_Click);
				// 
				// Button_Cancel
				// 
				this.Button_Cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
				this.Button_Cancel.Location = new Drawing.Point(120, 190);
				this.Button_Cancel.Size = new Drawing.Size(110, 23);
				this.Button_Cancel.TabIndex = 2;
				this.Button_Cancel.Name = "Button_Cancel";
				this.Button_Cancel.Text = "Cancel";
				this.Button_Cancel.UseVisualStyleBackColor = true;
				this.Button_Cancel.DialogResult = DialogResult.Cancel;
				this.Button_Cancel.Click += new EventHandler(this.Button_Cancel_Click);

				//
				// The form itself (this)
				//
				this.Name = "CollisionEditor_PasteOptions_SetLocationForm";
				this.Text = "Set Location";
				this.AcceptButton = Button_SetLocation;
				this.CancelButton = Button_Cancel;
				this.AutoScaleMode = AutoScaleMode.Font;
				this.AutoScaleDimensions = new SizeF(6F, 13F);
				this.ClientSize = new Drawing.Size(236, 219);
				this.MaximumSize = this.Size;
				this.MinimumSize = this.Size;
				this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
				this.StartPosition = FormStartPosition.CenterParent;

				this.Controls.Add(this.SpecificLocationX_Label);
				this.Controls.Add(this.SpecificLocationY_Label);
				this.Controls.Add(this.SpecificLocationX_Value);
				this.Controls.Add(this.SpecificLocationY_Value);
				this.Controls.Add(this.FreeLocationX_Label);
				this.Controls.Add(this.FreeLocationY_Label);
				this.Controls.Add(this.FreeLocationX_Value);
				this.Controls.Add(this.FreeLocationY_Value);
				this.Controls.Add(this.SpecificLocationRadio_SpecificPlacement);
				this.Controls.Add(this.SpecificLocationRadio_FreePlacement);
				this.Controls.Add(this.SpecificLocationRadio_ZeroZero);

				this.Controls.Add(this.Button_SetLocation);
				this.Controls.Add(this.Button_ResetToOriginalValue);
				this.Controls.Add(this.Button_Cancel);

				this.Icon = ParentOptions.Icon;

				this.ResumeLayout();
			}

			#endregion

			CollisionEditor_PasteOptions ParentOptions;

			private readonly Vector2 LastLocation;
			private readonly byte LastRadioValue;

			public Vector2 CurrentLocation;
			public byte PointMode = 0;

			public CollisionEditor_PasteOptions_SetLocationForm(CollisionEditor_PasteOptions Parent, byte LastRadioSelectedValue, Vector2 LocationValue)
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
					return;

				CurrentLocation = new Vector2(SpecificLocationX_Value.Value, SpecificLocationY_Value.Value);
				ParentOptions.UpdateLocationFromLocationForm(CurrentLocation, PointMode);
			}
			private void SpecificLocationY_Value_ValueChanged(object Sender, EventArgs e)
			{
				if (ParentOptions.Updating)
					return;
				
				CurrentLocation = new Vector2(SpecificLocationX_Value.Value, SpecificLocationY_Value.Value);
				ParentOptions.UpdateLocationFromLocationForm(CurrentLocation, PointMode);
			}
			private void FreeLocationX_Value_ValueChanged(object Sender, EventArgs e)
			{
				if (ParentOptions.Updating)
					return;

				CurrentLocation = new Vector2(FreeLocationX_Value.Value, FreeLocationY_Value.Value);
				ParentOptions.UpdateLocationFromLocationForm(CurrentLocation, PointMode);
			}
			private void FreeLocationY_Value_ValueChanged(object Sender, EventArgs e)
			{
				if (ParentOptions.Updating)
					return;

				CurrentLocation = new Vector2(FreeLocationX_Value.Value, FreeLocationY_Value.Value);
				ParentOptions.UpdateLocationFromLocationForm(CurrentLocation, PointMode);
			}

			private void SpecificLocationRadio_ZeroZero_CheckedChanged(object sender, EventArgs e)
			{
				if (ParentOptions.Updating)
					return;

				CurrentLocation = new Vector2(0, 0);
				ParentOptions.UpdateLocationFromLocationForm(CurrentLocation, PointMode);
			}

			private void SpecificLocationRadio_FreePlacement_CheckedChanged(object sender, EventArgs e)
			{
				FreeLocationX_Value.Enabled = 
				FreeLocationY_Value.Enabled = 
				SpecificLocationRadio_FreePlacement.Checked;
				
				if (ParentOptions.Updating)
					return;

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
					return;

				CurrentLocation = new Vector2(SpecificLocationX_Value.Value, SpecificLocationY_Value.Value);
				ParentOptions.UpdateLocationFromLocationForm(CurrentLocation, PointMode);
			}

			#region Paste Options to SetLocationForm
			public void CustomUserPointSet(ModelPanelViewport Viewport, Drawing.Point Location, float Depth)
			{
				Vector3 Point3D = Viewport.UnProject(Location.X, Location.Y, -Depth);

				// Trace ray to Z axis
				Point3D = Vector3.IntersectZ(Point3D, Viewport.UnProject(Location.X, Location.Y, 0.0f), 0.0f);
				Vector2 Point = (Vector2)Point3D;

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
					Opacity = 0.6f;
			}

			bool FormIsClosing = false;
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
			private NumericInputBox CreateNumericalInput(NumericInputBox ToTakeFrom, String InputBoxName)
			{
				return CreateNumericalInput(ToTakeFrom.Location, ToTakeFrom.Size, InputBoxName, ToTakeFrom.Text);
			}
			private NumericInputBox CreateNumericalInput(Drawing.Point Location, Drawing.Size Size, String InputBoxName, String DefaultValue)
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
			private Label CreateNumericalLabel(Label ToTakeFrom, String InputBoxName)
			{
				return CreateNumericalLabel(ToTakeFrom.Location, ToTakeFrom.Size, InputBoxName, ToTakeFrom.Text);
			}
			private Label CreateNumericalLabel(Drawing.Point Location, Drawing.Size Size, String InputBoxName, String DefaultText)
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