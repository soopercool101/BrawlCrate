using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrawlCrate.UI
{
    public class CollisionEditor_PasteOptions : Form
    {
        #region Designer

        private TabControl advancedPO_TabC;
        private TabPage advancedPO_TabC_ManipulateCollisionTab;
        private TabPage advancedPO_TabC_AllCollsProps;
        private GroupBox ManiColl_RotateGroup;
        private NumericUpDown ManiColl_Rotate_DegreesValue;
        private Button ManiColl_Rotate_SetTo0;
        private Button ManiColl_Rotate_SetTo270;
        private Button ManiColl_Rotate_SetTo180;
        private Button ManiColl_Rotate_SetTo90;
        private GroupBox ManiColl_FlipGroup;
        private CheckBox ManiColl_Flip_FlipY;
        private CheckBox ManiColl_Flip_FlipX;
        private GroupBox ManiColl_ScaleGroup;
        private NumericUpDown ManiColl_Scale_ScalarValue;
        private Label ManiColl_Scale_XLabel;
        private Button ManiColl_Scale_ResetScale;
        private Button ManiColl_Rotate_SetCP;
        private GroupBox ManiColl_PlacementGroup;
        private RadioButton ManiColl_Placement_RadioSpefPlacement;
        private RadioButton ManiColl_Placement_RadioFreeplace;
        private RadioButton ManiColl_Placement_RadioCenterStage;
        private Panel BottomPanel;
        private Button Cancel;
        private Button PasteCollision;
        private Label ManiColl_Placement_SpefPlacement_YLabel;
        private Label ManiColl_Placement_SpefPlacement_XLabel;
        private NumericInputBox ManiColl_Placement_SpefPlacement_LocY;
        private NumericInputBox ManiColl_Placement_SpefPlacement_LocX;
        private CheckBox ACP_Type_SelectedOverrides;
        private ToolTip pasteOptions_tipper;
        private Label ACP_MaterialLabel;
        private Label ACP_TypeLabel;
        private ComboBox ACP_MaterialComboBox;
        private ComboBox ACP_TypeComboBox;
        private Button ACP_ResetValuesToDef;
        private CheckBox ACP_Material_SelectedOverrides;
        private GroupBox ACP_CollTargetsGroup;
        private CheckBox ACP_CollTargets_SelectedOverrides;
        private GroupBox ACP_CollFlagsGroup;
        private CheckBox ACP_CollFlags_NoWallJump;
        private CheckBox ACP_CollFlags_Rotating;
        private CheckBox ACP_CollFlags_LeftLedge;
        private CheckBox ACP_CollFlags_RightLedge;
        private CheckBox ACP_CollFlags_FallThrough;
        private CheckBox ACP_CollFlags_SelectedOverrides;
        private CheckBox ACP_CollTargets_Items;
        private CheckBox ACP_CollTargets_PKMNTrainer;
        private CheckBox ACP_CollTargets_Everything;
        private GroupBox ACP_UnkFlagsGroup;
        private CheckBox ACP_UnkFlags_Unk1;
        private CheckBox ACP_UnkFlags_Unk4;
        private CheckBox ACP_UnkFlags_Unk3;
        private CheckBox ACP_UnkFlags_Unk2;
        private CheckBox ACP_UnkFlags_SelectedOverrides;
        private Panel advancedPO_TabC_AllCollsProps_Panel;
        private LinkLabel AllCollsProps_WarningLink;
        private Label ManiColl_Rotate_DegreesLabel;
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
            this.ManiColl_PlacementGroup = new GroupBox();
            this.ManiColl_Placement_SpefPlacement_YLabel = new Label();
            this.ManiColl_Placement_SpefPlacement_XLabel = new Label();
            this.ManiColl_Placement_SpefPlacement_LocY = new NumericInputBox();
            this.ManiColl_Placement_SpefPlacement_LocX = new NumericInputBox();
            this.ManiColl_Placement_RadioSpefPlacement = new RadioButton();
            this.ManiColl_Placement_RadioFreeplace = new RadioButton();
            this.ManiColl_Placement_RadioCenterStage = new RadioButton();
            this.ManiColl_ScaleGroup = new GroupBox();
            this.ManiColl_Scale_ScalarValue = new NumericUpDown();
            this.ManiColl_Scale_XLabel = new Label();
            this.ManiColl_Scale_ResetScale = new Button();
            this.ManiColl_FlipGroup = new GroupBox();
            this.ManiColl_Flip_FlipY = new CheckBox();
            this.ManiColl_Flip_FlipX = new CheckBox();
            this.ManiColl_RotateGroup = new GroupBox();
            this.ManiColl_Rotate_DegreesValue = new NumericUpDown();
            this.ManiColl_Rotate_SetTo0 = new Button();
            this.ManiColl_Rotate_SetCP = new Button();
            this.ManiColl_Rotate_SetTo270 = new Button();
            this.ManiColl_Rotate_SetTo180 = new Button();
            this.ManiColl_Rotate_SetTo90 = new Button();
            this.advancedPO_TabC_AllCollsProps = new TabPage();
            this.advancedPO_TabC_AllCollsProps_Panel = new Panel();
            this.AllCollsProps_WarningLink = new LinkLabel();
            this.ACP_UnkFlagsGroup = new GroupBox();
            this.ACP_UnkFlags_Unk1 = new CheckBox();
            this.ACP_UnkFlags_Unk4 = new CheckBox();
            this.ACP_UnkFlags_Unk3 = new CheckBox();
            this.ACP_UnkFlags_Unk2 = new CheckBox();
            this.ACP_UnkFlags_SelectedOverrides = new CheckBox();
            this.ACP_CollFlagsGroup = new GroupBox();
            this.ACP_CollFlags_NoWallJump = new CheckBox();
            this.ACP_CollFlags_Rotating = new CheckBox();
            this.ACP_CollFlags_LeftLedge = new CheckBox();
            this.ACP_CollFlags_RightLedge = new CheckBox();
            this.ACP_CollFlags_FallThrough = new CheckBox();
            this.ACP_CollFlags_SelectedOverrides = new CheckBox();
            this.ACP_TypeComboBox = new ComboBox();
            this.ACP_CollTargetsGroup = new GroupBox();
            this.ACP_CollTargets_Items = new CheckBox();
            this.ACP_CollTargets_PKMNTrainer = new CheckBox();
            this.ACP_CollTargets_Everything = new CheckBox();
            this.ACP_CollTargets_SelectedOverrides = new CheckBox();
            this.ACP_MaterialComboBox = new ComboBox();
            this.ACP_Material_SelectedOverrides = new CheckBox();
            this.ACP_TypeLabel = new Label();
            this.ACP_Type_SelectedOverrides = new CheckBox();
            this.ACP_MaterialLabel = new Label();
            this.ACP_ResetValuesToDef = new Button();
            this.BottomPanel = new Panel();
            this.Cancel = new Button();
            this.PasteCollision = new Button();
            this.pasteOptions_tipper = new ToolTip(this.components);
            this.ManiColl_Rotate_DegreesLabel = new Label();
            this.advancedPO_TabC.SuspendLayout();
            this.advancedPO_TabC_ManipulateCollisionTab.SuspendLayout();
            this.ManiColl_PlacementGroup.SuspendLayout();
            this.ManiColl_ScaleGroup.SuspendLayout();
            ((ISupportInitialize)(this.ManiColl_Scale_ScalarValue)).BeginInit();
            this.ManiColl_FlipGroup.SuspendLayout();
            this.ManiColl_RotateGroup.SuspendLayout();
            ((ISupportInitialize)(this.ManiColl_Rotate_DegreesValue)).BeginInit();
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
            this.advancedPO_TabC.Location = new Point(0, 0);
            this.advancedPO_TabC.Name = "advancedPO_TabC";
            this.advancedPO_TabC.SelectedIndex = 0;
            this.advancedPO_TabC.Size = new Size(236, 407);
            this.advancedPO_TabC.TabIndex = 0;
            // 
            // advancedPO_TabC_ManipulateCollisionTab
            // 
            this.advancedPO_TabC_ManipulateCollisionTab.AutoScroll = true;
            this.advancedPO_TabC_ManipulateCollisionTab.Controls.Add(this.ManiColl_PlacementGroup);
            this.advancedPO_TabC_ManipulateCollisionTab.Controls.Add(this.ManiColl_ScaleGroup);
            this.advancedPO_TabC_ManipulateCollisionTab.Controls.Add(this.ManiColl_FlipGroup);
            this.advancedPO_TabC_ManipulateCollisionTab.Controls.Add(this.ManiColl_RotateGroup);
            this.advancedPO_TabC_ManipulateCollisionTab.Location = new Point(4, 22);
            this.advancedPO_TabC_ManipulateCollisionTab.Name = "advancedPO_TabC_ManipulateCollisionTab";
            this.advancedPO_TabC_ManipulateCollisionTab.Padding = new Padding(3);
            this.advancedPO_TabC_ManipulateCollisionTab.Size = new Size(228, 381);
            this.advancedPO_TabC_ManipulateCollisionTab.TabIndex = 0;
            this.advancedPO_TabC_ManipulateCollisionTab.Text = "Manipulate Collision";
            this.advancedPO_TabC_ManipulateCollisionTab.UseVisualStyleBackColor = true;
            // 
            // ManiColl_PlacementGroup
            // 
            this.ManiColl_PlacementGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ManiColl_PlacementGroup.Controls.Add(this.ManiColl_Placement_SpefPlacement_YLabel);
            this.ManiColl_PlacementGroup.Controls.Add(this.ManiColl_Placement_SpefPlacement_XLabel);
            this.ManiColl_PlacementGroup.Controls.Add(this.ManiColl_Placement_SpefPlacement_LocY);
            this.ManiColl_PlacementGroup.Controls.Add(this.ManiColl_Placement_SpefPlacement_LocX);
            this.ManiColl_PlacementGroup.Controls.Add(this.ManiColl_Placement_RadioSpefPlacement);
            this.ManiColl_PlacementGroup.Controls.Add(this.ManiColl_Placement_RadioFreeplace);
            this.ManiColl_PlacementGroup.Controls.Add(this.ManiColl_Placement_RadioCenterStage);
            this.ManiColl_PlacementGroup.Location = new Point(4, 222);
            this.ManiColl_PlacementGroup.Name = "ManiColl_PlacementGroup";
            this.ManiColl_PlacementGroup.Size = new Size(218, 127);
            this.ManiColl_PlacementGroup.TabIndex = 3;
            this.ManiColl_PlacementGroup.TabStop = false;
            this.ManiColl_PlacementGroup.Text = "Location/Placement";
            // 
            // ManiColl_Placement_SpefPlacement_YLabel
            // 
            this.ManiColl_Placement_SpefPlacement_YLabel.BorderStyle = BorderStyle.FixedSingle;
            this.ManiColl_Placement_SpefPlacement_YLabel.Location = new Point(33, 98);
            this.ManiColl_Placement_SpefPlacement_YLabel.Name = "ManiColl_Placement_SpefPlacement_YLabel";
            this.ManiColl_Placement_SpefPlacement_YLabel.Size = new Size(20, 20);
            this.ManiColl_Placement_SpefPlacement_YLabel.TabIndex = 2;
            this.ManiColl_Placement_SpefPlacement_YLabel.Text = "Y";
            this.ManiColl_Placement_SpefPlacement_YLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ManiColl_Placement_SpefPlacement_XLabel
            // 
            this.ManiColl_Placement_SpefPlacement_XLabel.BorderStyle = BorderStyle.FixedSingle;
            this.ManiColl_Placement_SpefPlacement_XLabel.Location = new Point(33, 79);
            this.ManiColl_Placement_SpefPlacement_XLabel.Name = "ManiColl_Placement_SpefPlacement_XLabel";
            this.ManiColl_Placement_SpefPlacement_XLabel.Size = new Size(20, 20);
            this.ManiColl_Placement_SpefPlacement_XLabel.TabIndex = 2;
            this.ManiColl_Placement_SpefPlacement_XLabel.Text = "X";
            this.ManiColl_Placement_SpefPlacement_XLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ManiColl_Placement_SpefPlacement_LocY
            // 
            this.ManiColl_Placement_SpefPlacement_LocY.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ManiColl_Placement_SpefPlacement_LocY.BorderStyle = BorderStyle.FixedSingle;
            this.ManiColl_Placement_SpefPlacement_LocY.Integer = false;
            this.ManiColl_Placement_SpefPlacement_LocY.Integral = false;
            this.ManiColl_Placement_SpefPlacement_LocY.Location = new Point(52, 98);
            this.ManiColl_Placement_SpefPlacement_LocY.MaximumValue = 3.402823E+38F;
            this.ManiColl_Placement_SpefPlacement_LocY.MinimumValue = -3.402823E+38F;
            this.ManiColl_Placement_SpefPlacement_LocY.Name = "ManiColl_Placement_SpefPlacement_LocY";
            this.ManiColl_Placement_SpefPlacement_LocY.Size = new Size(135, 20);
            this.ManiColl_Placement_SpefPlacement_LocY.TabIndex = 1;
            this.ManiColl_Placement_SpefPlacement_LocY.Text = "0";
            this.ManiColl_Placement_SpefPlacement_LocY.ValueChanged += new EventHandler(this.ManiColl_Placement_SpefPlacement_LocY_ValueChanged);
            // 
            // ManiColl_Placement_SpefPlacement_LocX
            // 
            this.ManiColl_Placement_SpefPlacement_LocX.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ManiColl_Placement_SpefPlacement_LocX.BorderStyle = BorderStyle.FixedSingle;
            this.ManiColl_Placement_SpefPlacement_LocX.Integer = false;
            this.ManiColl_Placement_SpefPlacement_LocX.Integral = false;
            this.ManiColl_Placement_SpefPlacement_LocX.Location = new Point(52, 79);
            this.ManiColl_Placement_SpefPlacement_LocX.MaximumValue = 3.402823E+38F;
            this.ManiColl_Placement_SpefPlacement_LocX.MinimumValue = -3.402823E+38F;
            this.ManiColl_Placement_SpefPlacement_LocX.Name = "ManiColl_Placement_SpefPlacement_LocX";
            this.ManiColl_Placement_SpefPlacement_LocX.Size = new Size(135, 20);
            this.ManiColl_Placement_SpefPlacement_LocX.TabIndex = 1;
            this.ManiColl_Placement_SpefPlacement_LocX.Text = "0";
            this.ManiColl_Placement_SpefPlacement_LocX.ValueChanged += new EventHandler(this.ManiColl_Placement_SpefPlacement_LocX_ValueChanged);
            // 
            // ManiColl_Placement_RadioSpefPlacement
            // 
            this.ManiColl_Placement_RadioSpefPlacement.AutoSize = true;
            this.ManiColl_Placement_RadioSpefPlacement.Location = new Point(12, 56);
            this.ManiColl_Placement_RadioSpefPlacement.Name = "ManiColl_Placement_RadioSpefPlacement";
            this.ManiColl_Placement_RadioSpefPlacement.Size = new Size(113, 17);
            this.ManiColl_Placement_RadioSpefPlacement.TabIndex = 0;
            this.ManiColl_Placement_RadioSpefPlacement.Text = "Specified Location";
            this.pasteOptions_tipper.SetToolTip(this.ManiColl_Placement_RadioSpefPlacement, "If you like being specific, you can specify the location by inputting the two val" +
        "ues.");
            this.ManiColl_Placement_RadioSpefPlacement.UseVisualStyleBackColor = true;
            this.ManiColl_Placement_RadioSpefPlacement.CheckedChanged += new EventHandler(this.ManiColl_Placement_RadioSpefPlacement_CheckedChanged);
            // 
            // ManiColl_Placement_RadioFreeplace
            // 
            this.ManiColl_Placement_RadioFreeplace.AutoSize = true;
            this.ManiColl_Placement_RadioFreeplace.Location = new Point(12, 38);
            this.ManiColl_Placement_RadioFreeplace.Name = "ManiColl_Placement_RadioFreeplace";
            this.ManiColl_Placement_RadioFreeplace.Size = new Size(72, 17);
            this.ManiColl_Placement_RadioFreeplace.TabIndex = 0;
            this.ManiColl_Placement_RadioFreeplace.Text = "Freeplace";
            this.pasteOptions_tipper.SetToolTip(this.ManiColl_Placement_RadioFreeplace, "If this is selected, you can move the center location of copied collisions anywhe" +
        "re.");
            this.ManiColl_Placement_RadioFreeplace.UseVisualStyleBackColor = true;
            this.ManiColl_Placement_RadioFreeplace.CheckedChanged += new EventHandler(this.ManiColl_Placement_RadioFreeplace_CheckedChanged);
            // 
            // ManiColl_Placement_RadioCenterStage
            // 
            this.ManiColl_Placement_RadioCenterStage.AutoSize = true;
            this.ManiColl_Placement_RadioCenterStage.Checked = true;
            this.ManiColl_Placement_RadioCenterStage.Location = new Point(12, 19);
            this.ManiColl_Placement_RadioCenterStage.Name = "ManiColl_Placement_RadioCenterStage";
            this.ManiColl_Placement_RadioCenterStage.Size = new Size(117, 17);
            this.ManiColl_Placement_RadioCenterStage.TabIndex = 0;
            this.ManiColl_Placement_RadioCenterStage.TabStop = true;
            this.ManiColl_Placement_RadioCenterStage.Text = "Center of the Stage";
            this.pasteOptions_tipper.SetToolTip(this.ManiColl_Placement_RadioCenterStage, "It is placed at the center of the stage (0, 0).");
            this.ManiColl_Placement_RadioCenterStage.UseVisualStyleBackColor = true;
            this.ManiColl_Placement_RadioCenterStage.CheckedChanged += new EventHandler(this.ManiColl_Placement_RadioCenterStage_CheckedChanged);
            // 
            // ManiColl_ScaleGroup
            // 
            this.ManiColl_ScaleGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ManiColl_ScaleGroup.Controls.Add(this.ManiColl_Scale_ScalarValue);
            this.ManiColl_ScaleGroup.Controls.Add(this.ManiColl_Scale_XLabel);
            this.ManiColl_ScaleGroup.Controls.Add(this.ManiColl_Scale_ResetScale);
            this.ManiColl_ScaleGroup.Location = new Point(4, 166);
            this.ManiColl_ScaleGroup.Name = "ManiColl_ScaleGroup";
            this.ManiColl_ScaleGroup.Size = new Size(218, 50);
            this.ManiColl_ScaleGroup.TabIndex = 2;
            this.ManiColl_ScaleGroup.TabStop = false;
            this.ManiColl_ScaleGroup.Text = "Scale";
            // 
            // ManiColl_Scale_ScalarValue
            // 
            this.ManiColl_Scale_ScalarValue.Anchor = AnchorStyles.Top;
            this.ManiColl_Scale_ScalarValue.DecimalPlaces = 3;
            this.ManiColl_Scale_ScalarValue.Location = new Point(39, 19);
            this.ManiColl_Scale_ScalarValue.Maximum = new decimal(new int[] { 10000, 0, 0, 0});
            this.ManiColl_Scale_ScalarValue.Minimum = new decimal(new int[] { 1, 0, 0, 196608});
            this.ManiColl_Scale_ScalarValue.Name = "ManiColl_Scale_ScalarValue";
            this.ManiColl_Scale_ScalarValue.Size = new Size(79, 20);
            this.ManiColl_Scale_ScalarValue.TabIndex = 1;
            this.ManiColl_Scale_ScalarValue.Value = new decimal(new int[] { 1, 0, 0, 0});
            this.ManiColl_Scale_ScalarValue.ValueChanged += new EventHandler(this.ManiColl_Scale_ScalarValue_ValueChanged);
            // 
            // ManiColl_Scale_XLabel
            // 
            this.ManiColl_Scale_XLabel.Anchor = AnchorStyles.Top;
            this.ManiColl_Scale_XLabel.AutoSize = true;
            this.ManiColl_Scale_XLabel.Location = new Point(25, 21);
            this.ManiColl_Scale_XLabel.Name = "ManiColl_Scale_XLabel";
            this.ManiColl_Scale_XLabel.Size = new Size(12, 13);
            this.ManiColl_Scale_XLabel.TabIndex = 2;
            this.ManiColl_Scale_XLabel.Text = "x";
            // 
            // ManiColl_Scale_ResetScale
            // 
            this.ManiColl_Scale_ResetScale.Anchor = AnchorStyles.Top;
            this.ManiColl_Scale_ResetScale.Location = new Point(123, 17);
            this.ManiColl_Scale_ResetScale.Name = "ManiColl_Scale_ResetScale";
            this.ManiColl_Scale_ResetScale.Size = new Size(66, 23);
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
            this.ManiColl_FlipGroup.Location = new Point(4, 116);
            this.ManiColl_FlipGroup.Name = "ManiColl_FlipGroup";
            this.ManiColl_FlipGroup.Size = new Size(218, 46);
            this.ManiColl_FlipGroup.TabIndex = 1;
            this.ManiColl_FlipGroup.TabStop = false;
            this.ManiColl_FlipGroup.Text = "Flip";
            // 
            // ManiColl_Flip_FlipY
            // 
            this.ManiColl_Flip_FlipY.AutoSize = true;
            this.ManiColl_Flip_FlipY.Location = new Point(113, 19);
            this.ManiColl_Flip_FlipY.Name = "ManiColl_Flip_FlipY";
            this.ManiColl_Flip_FlipY.Size = new Size(74, 17);
            this.ManiColl_Flip_FlipY.TabIndex = 2;
            this.ManiColl_Flip_FlipY.Text = "Flip Y-Axis";
            this.ManiColl_Flip_FlipY.UseVisualStyleBackColor = true;
            this.ManiColl_Flip_FlipY.CheckedChanged += new EventHandler(this.ManiColl_Flip_FlipY_CheckedChanged);
            // 
            // ManiColl_Flip_FlipX
            // 
            this.ManiColl_Flip_FlipX.AutoSize = true;
            this.ManiColl_Flip_FlipX.Location = new Point(12, 19);
            this.ManiColl_Flip_FlipX.Name = "ManiColl_Flip_FlipX";
            this.ManiColl_Flip_FlipX.Size = new Size(74, 17);
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
            this.ManiColl_RotateGroup.Controls.Add(this.ManiColl_Rotate_SetCP);
            this.ManiColl_RotateGroup.Controls.Add(this.ManiColl_Rotate_SetTo270);
            this.ManiColl_RotateGroup.Controls.Add(this.ManiColl_Rotate_SetTo180);
            this.ManiColl_RotateGroup.Controls.Add(this.ManiColl_Rotate_SetTo90);
            this.ManiColl_RotateGroup.Location = new Point(4, 4);
            this.ManiColl_RotateGroup.Name = "ManiColl_RotateGroup";
            this.ManiColl_RotateGroup.Size = new Size(218, 108);
            this.ManiColl_RotateGroup.TabIndex = 0;
            this.ManiColl_RotateGroup.TabStop = false;
            this.ManiColl_RotateGroup.Text = "Rotate";
            // 
            // ManiColl_Rotate_DegreesValue
            // 
            this.ManiColl_Rotate_DegreesValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.ManiColl_Rotate_DegreesValue.DecimalPlaces = 3;
            this.ManiColl_Rotate_DegreesValue.Location = new Point(145, 0);
            this.ManiColl_Rotate_DegreesValue.Maximum = new decimal(new int[] { 360, 0, 0, 0});
            this.ManiColl_Rotate_DegreesValue.Name = "ManiColl_Rotate_DegreesValue";
            this.ManiColl_Rotate_DegreesValue.Size = new Size(62, 20);
            this.ManiColl_Rotate_DegreesValue.TabIndex = 1;
            this.ManiColl_Rotate_DegreesValue.ValueChanged += new EventHandler(this.ManiColl_Rotate_DegreesValue_ValueChanged);
            // 
            // ManiColl_Rotate_SetTo0
            // 
            this.ManiColl_Rotate_SetTo0.Location = new Point(52, 20);
            this.ManiColl_Rotate_SetTo0.Name = "ManiColl_Rotate_SetTo0";
            this.ManiColl_Rotate_SetTo0.Size = new Size(38, 23);
            this.ManiColl_Rotate_SetTo0.TabIndex = 1;
            this.ManiColl_Rotate_SetTo0.Text = "0º";
            this.ManiColl_Rotate_SetTo0.UseVisualStyleBackColor = true;
            this.ManiColl_Rotate_SetTo0.Click += new EventHandler(this.ManiColl_Rotate_SetTo0_Click);
            // 
            // ManiColl_Rotate_SetCP
            // 
            this.ManiColl_Rotate_SetCP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.ManiColl_Rotate_SetCP.Location = new Point(142, 37);
            this.ManiColl_Rotate_SetCP.Name = "ManiColl_Rotate_SetCP";
            this.ManiColl_Rotate_SetCP.Size = new Size(66, 48);
            this.ManiColl_Rotate_SetCP.TabIndex = 1;
            this.ManiColl_Rotate_SetCP.Text = "Set Center Point";
            this.pasteOptions_tipper.SetToolTip(this.ManiColl_Rotate_SetCP, "If selected, you are required to provide a centered point for rotation.");
            this.ManiColl_Rotate_SetCP.UseVisualStyleBackColor = true;
            this.ManiColl_Rotate_SetCP.Click += new EventHandler(this.ManiColl_Rotate_SetCP_Click);
            // 
            // ManiColl_Rotate_SetTo270
            // 
            this.ManiColl_Rotate_SetTo270.Location = new Point(11, 46);
            this.ManiColl_Rotate_SetTo270.Name = "ManiColl_Rotate_SetTo270";
            this.ManiColl_Rotate_SetTo270.Size = new Size(38, 23);
            this.ManiColl_Rotate_SetTo270.TabIndex = 1;
            this.ManiColl_Rotate_SetTo270.Text = "270º";
            this.ManiColl_Rotate_SetTo270.UseVisualStyleBackColor = true;
            this.ManiColl_Rotate_SetTo270.Click += new EventHandler(this.ManiColl_Rotate_SetTo270_Click);
            // 
            // ManiColl_Rotate_SetTo180
            // 
            this.ManiColl_Rotate_SetTo180.Location = new Point(52, 72);
            this.ManiColl_Rotate_SetTo180.Name = "ManiColl_Rotate_SetTo180";
            this.ManiColl_Rotate_SetTo180.Size = new Size(38, 23);
            this.ManiColl_Rotate_SetTo180.TabIndex = 1;
            this.ManiColl_Rotate_SetTo180.Text = "180º";
            this.ManiColl_Rotate_SetTo180.UseVisualStyleBackColor = true;
            this.ManiColl_Rotate_SetTo180.Click += new EventHandler(this.ManiColl_Rotate_SetTo180_Click);
            // 
            // ManiColl_Rotate_SetTo90
            // 
            this.ManiColl_Rotate_SetTo90.Location = new Point(93, 46);
            this.ManiColl_Rotate_SetTo90.Name = "ManiColl_Rotate_SetTo90";
            this.ManiColl_Rotate_SetTo90.Size = new Size(38, 23);
            this.ManiColl_Rotate_SetTo90.TabIndex = 1;
            this.ManiColl_Rotate_SetTo90.Text = "90º";
            this.ManiColl_Rotate_SetTo90.UseVisualStyleBackColor = true;
            this.ManiColl_Rotate_SetTo90.Click += new EventHandler(this.ManiColl_Rotate_SetTo90_Click);
            // 
            // advancedPO_TabC_AllCollsProps
            // 
            this.advancedPO_TabC_AllCollsProps.Controls.Add(this.advancedPO_TabC_AllCollsProps_Panel);
            this.advancedPO_TabC_AllCollsProps.Controls.Add(this.ACP_ResetValuesToDef);
            this.advancedPO_TabC_AllCollsProps.Location = new Point(4, 22);
            this.advancedPO_TabC_AllCollsProps.Name = "advancedPO_TabC_AllCollsProps";
            this.advancedPO_TabC_AllCollsProps.Padding = new Padding(3);
            this.advancedPO_TabC_AllCollsProps.Size = new Size(228, 381);
            this.advancedPO_TabC_AllCollsProps.TabIndex = 1;
            this.advancedPO_TabC_AllCollsProps.Text = "All Collision Properties";
            this.advancedPO_TabC_AllCollsProps.UseVisualStyleBackColor = true;
            // 
            // advancedPO_TabC_AllCollsProps_Panel
            // 
            this.advancedPO_TabC_AllCollsProps_Panel.AutoScroll = true;
            this.advancedPO_TabC_AllCollsProps_Panel.AutoScrollMargin = new Size(0, 3);
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
            this.advancedPO_TabC_AllCollsProps_Panel.Location = new Point(3, 3);
            this.advancedPO_TabC_AllCollsProps_Panel.Name = "advancedPO_TabC_AllCollsProps_Panel";
            this.advancedPO_TabC_AllCollsProps_Panel.Size = new Size(222, 352);
            this.advancedPO_TabC_AllCollsProps_Panel.TabIndex = 6;
            // 
            // AllCollsProps_WarningLink
            // 
            this.AllCollsProps_WarningLink.AutoSize = false;
            this.AllCollsProps_WarningLink.Dock = DockStyle.Top;
            this.AllCollsProps_WarningLink.Location = new Point(0, 0);
            this.AllCollsProps_WarningLink.Name = "AllCollsProps_WarningLink";
            this.AllCollsProps_WarningLink.Size = new Size(222, 43);
            this.AllCollsProps_WarningLink.TabIndex = 6;
            this.AllCollsProps_WarningLink.TabStop = true;
            this.AllCollsProps_WarningLink.Text = "Note: This will replace all of the collision properties that were copied. If you " +
    "do not want this, press \"Reset\". More info...";
            this.AllCollsProps_WarningLink.LinkClicked += new LinkLabelLinkClickedEventHandler(this.AllCollsProps_WarningLink_LinkClicked);
            // 
            // ACP_UnkFlagsGroup
            // 
            this.ACP_UnkFlagsGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ACP_UnkFlagsGroup.Controls.Add(this.ACP_UnkFlags_Unk1);
            this.ACP_UnkFlagsGroup.Controls.Add(this.ACP_UnkFlags_Unk2);
            this.ACP_UnkFlagsGroup.Controls.Add(this.ACP_UnkFlags_Unk3);
            this.ACP_UnkFlagsGroup.Controls.Add(this.ACP_UnkFlags_Unk4);
            this.ACP_UnkFlagsGroup.Controls.Add(this.ACP_UnkFlags_SelectedOverrides);
            this.ACP_UnkFlagsGroup.Location = new Point(6, 288);
            this.ACP_UnkFlagsGroup.Name = "ACP_UnkFlagsGroup";
            this.ACP_UnkFlagsGroup.Size = new Size(211, 58);
            this.ACP_UnkFlagsGroup.TabIndex = 5;
            this.ACP_UnkFlagsGroup.TabStop = false;
            this.ACP_UnkFlagsGroup.Text = "Unknown Flags";
            // 
            // ACP_UnkFlags_Unk1
            // 
            this.ACP_UnkFlags_Unk1.AutoSize = true;
            this.ACP_UnkFlags_Unk1.Location = new Point(7, 18);
            this.ACP_UnkFlags_Unk1.Name = "ACP_UnkFlags_Unk1";
            this.ACP_UnkFlags_Unk1.Size = new Size(81, 17);
            this.ACP_UnkFlags_Unk1.TabIndex = 4;
            this.ACP_UnkFlags_Unk1.Text = "Unknown 1";
            this.ACP_UnkFlags_Unk1.UseVisualStyleBackColor = true;
            this.ACP_UnkFlags_Unk1.CheckedChanged += new EventHandler(this.ACP_UnkFlags_Unk1_CheckedChanged);
            // 
            // ACP_UnkFlags_Unk2
            // 
            this.ACP_UnkFlags_Unk2.AutoSize = true;
            this.ACP_UnkFlags_Unk2.Location = new Point(97, 18);
            this.ACP_UnkFlags_Unk2.Name = "ACP_UnkFlags_Unk2";
            this.ACP_UnkFlags_Unk2.Size = new Size(81, 17);
            this.ACP_UnkFlags_Unk2.TabIndex = 4;
            this.ACP_UnkFlags_Unk2.Text = "Unknown 2";
            this.ACP_UnkFlags_Unk2.UseVisualStyleBackColor = true;
            this.ACP_UnkFlags_Unk2.CheckedChanged += new EventHandler(this.ACP_UnkFlags_Unk2_CheckedChanged);
            // 
            // ACP_UnkFlags_Unk3
            // 
            this.ACP_UnkFlags_Unk3.AutoSize = true;
            this.ACP_UnkFlags_Unk3.Location = new Point(7, 36);
            this.ACP_UnkFlags_Unk3.Name = "ACP_UnkFlags_Unk3";
            this.ACP_UnkFlags_Unk3.Size = new Size(81, 17);
            this.ACP_UnkFlags_Unk3.TabIndex = 4;
            this.ACP_UnkFlags_Unk3.Text = "Unknown 3";
            this.ACP_UnkFlags_Unk3.UseVisualStyleBackColor = true;
            this.ACP_UnkFlags_Unk3.CheckedChanged += new EventHandler(this.ACP_UnkFlags_Unk3_CheckedChanged);
            // 
            // ACP_UnkFlags_Unk4
            // 
            this.ACP_UnkFlags_Unk4.AutoSize = true;
            this.ACP_UnkFlags_Unk4.Location = new Point(97, 36);
            this.ACP_UnkFlags_Unk4.Name = "ACP_UnkFlags_Unk4";
            this.ACP_UnkFlags_Unk4.Size = new Size(81, 17);
            this.ACP_UnkFlags_Unk4.TabIndex = 4;
            this.ACP_UnkFlags_Unk4.Text = "Unknown 4";
            this.ACP_UnkFlags_Unk4.UseVisualStyleBackColor = true;
            this.ACP_UnkFlags_Unk4.CheckedChanged += new EventHandler(this.ACP_UnkFlags_Unk4_CheckedChanged);
            // 
            // ACP_UnkFlags_SelectedOverrides
            // 
            this.ACP_UnkFlags_SelectedOverrides.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.ACP_UnkFlags_SelectedOverrides.AutoSize = true;
            this.ACP_UnkFlags_SelectedOverrides.Checked = true;
            this.ACP_UnkFlags_SelectedOverrides.CheckState = CheckState.Checked;
            this.ACP_UnkFlags_SelectedOverrides.Location = new Point(191, 0);
            this.ACP_UnkFlags_SelectedOverrides.Name = "ACP_UnkFlags_SelectedOverrides";
            this.ACP_UnkFlags_SelectedOverrides.Size = new Size(15, 14);
            this.ACP_UnkFlags_SelectedOverrides.TabIndex = 4;
            this.pasteOptions_tipper.SetToolTip(this.ACP_UnkFlags_SelectedOverrides, "If checked, selected/nonselected unknown values will override all copied properti" +
        "es\' unknown values.");
            this.ACP_UnkFlags_SelectedOverrides.UseVisualStyleBackColor = true;
            this.ACP_UnkFlags_SelectedOverrides.CheckedChanged += new EventHandler(this.ACP_UnkFlags_SelectedOverrides_CheckedChanged);
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
            this.ACP_CollFlagsGroup.Location = new Point(6, 205);
            this.ACP_CollFlagsGroup.Name = "ACP_CollFlagsGroup";
            this.ACP_CollFlagsGroup.Size = new Size(211, 77);
            this.ACP_CollFlagsGroup.TabIndex = 5;
            this.ACP_CollFlagsGroup.TabStop = false;
            this.ACP_CollFlagsGroup.Text = "Collision Flags";
            // 
            // ACP_CollFlags_NoWallJump
            // 
            this.ACP_CollFlags_NoWallJump.AutoSize = true;
            this.ACP_CollFlags_NoWallJump.Location = new Point(97, 37);
            this.ACP_CollFlags_NoWallJump.Name = "ACP_CollFlags_NoWallJump";
            this.ACP_CollFlags_NoWallJump.Size = new Size(86, 17);
            this.ACP_CollFlags_NoWallJump.TabIndex = 4;
            this.ACP_CollFlags_NoWallJump.Text = "No Walljump";
            this.ACP_CollFlags_NoWallJump.UseVisualStyleBackColor = true;
            this.ACP_CollFlags_NoWallJump.CheckedChanged += new EventHandler(this.ACP_CollFlags_NoWallJump_CheckedChanged);
            // 
            // ACP_CollFlags_Rotating
            // 
            this.ACP_CollFlags_Rotating.AutoSize = true;
            this.ACP_CollFlags_Rotating.Location = new Point(6, 55);
            this.ACP_CollFlags_Rotating.Name = "ACP_CollFlags_Rotating";
            this.ACP_CollFlags_Rotating.Size = new Size(66, 17);
            this.ACP_CollFlags_Rotating.TabIndex = 4;
            this.ACP_CollFlags_Rotating.Text = "Rotating";
            this.ACP_CollFlags_Rotating.UseVisualStyleBackColor = true;
            this.ACP_CollFlags_Rotating.CheckedChanged += new EventHandler(this.ACP_CollFlags_Rotating_CheckedChanged);
            // 
            // ACP_CollFlags_LeftLedge
            // 
            this.ACP_CollFlags_LeftLedge.AutoSize = true;
            this.ACP_CollFlags_LeftLedge.Location = new Point(97, 19);
            this.ACP_CollFlags_LeftLedge.Name = "ACP_CollFlags_LeftLedge";
            this.ACP_CollFlags_LeftLedge.Size = new Size(77, 17);
            this.ACP_CollFlags_LeftLedge.TabIndex = 4;
            this.ACP_CollFlags_LeftLedge.Text = "Left Ledge";
            this.ACP_CollFlags_LeftLedge.UseVisualStyleBackColor = true;
            this.ACP_CollFlags_LeftLedge.CheckedChanged += new EventHandler(this.ACP_CollFlags_LeftLedge_CheckedChanged);
            // 
            // ACP_CollFlags_RightLedge
            // 
            this.ACP_CollFlags_RightLedge.AutoSize = true;
            this.ACP_CollFlags_RightLedge.Location = new Point(6, 37);
            this.ACP_CollFlags_RightLedge.Name = "ACP_CollFlags_RightLedge";
            this.ACP_CollFlags_RightLedge.Size = new Size(84, 17);
            this.ACP_CollFlags_RightLedge.TabIndex = 4;
            this.ACP_CollFlags_RightLedge.Text = "Right Ledge";
            this.ACP_CollFlags_RightLedge.UseVisualStyleBackColor = true;
            this.ACP_CollFlags_RightLedge.CheckedChanged += new EventHandler(this.ACP_CollFlags_RightLedge_CheckedChanged);
            // 
            // ACP_CollFlags_FallThrough
            // 
            this.ACP_CollFlags_FallThrough.AutoSize = true;
            this.ACP_CollFlags_FallThrough.Location = new Point(6, 19);
            this.ACP_CollFlags_FallThrough.Name = "ACP_CollFlags_FallThrough";
            this.ACP_CollFlags_FallThrough.Size = new Size(85, 17);
            this.ACP_CollFlags_FallThrough.TabIndex = 4;
            this.ACP_CollFlags_FallThrough.Text = "Fall Through";
            this.ACP_CollFlags_FallThrough.UseVisualStyleBackColor = true;
            this.ACP_CollFlags_FallThrough.CheckedChanged += new EventHandler(this.ACP_CollFlags_FallThrough_CheckedChanged);
            // 
            // ACP_CollFlags_SelectedOverrides
            // 
            this.ACP_CollFlags_SelectedOverrides.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.ACP_CollFlags_SelectedOverrides.AutoSize = true;
            this.ACP_CollFlags_SelectedOverrides.Checked = true;
            this.ACP_CollFlags_SelectedOverrides.CheckState = CheckState.Checked;
            this.ACP_CollFlags_SelectedOverrides.Location = new Point(191, 1);
            this.ACP_CollFlags_SelectedOverrides.Name = "ACP_CollFlags_SelectedOverrides";
            this.ACP_CollFlags_SelectedOverrides.Size = new Size(15, 14);
            this.ACP_CollFlags_SelectedOverrides.TabIndex = 4;
            this.pasteOptions_tipper.SetToolTip(this.ACP_CollFlags_SelectedOverrides, "If checked, selected/nonselected collision flags will override all copied propert" +
        "ies\' collision flags.");
            this.ACP_CollFlags_SelectedOverrides.UseVisualStyleBackColor = true;
            this.ACP_CollFlags_SelectedOverrides.CheckedChanged += new EventHandler(this.ACP_CollFlags_SelectedOverrides_CheckedChanged);
            // 
            // ACP_TypeComboBox
            // 
            this.ACP_TypeComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ACP_TypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.ACP_TypeComboBox.FormattingEnabled = true;
            this.ACP_TypeComboBox.Location = new Point(6, 67);
            this.ACP_TypeComboBox.Name = "ACP_TypeComboBox";
            this.ACP_TypeComboBox.Size = new Size(190, 21);
            this.ACP_TypeComboBox.TabIndex = 2;
            this.ACP_TypeComboBox.SelectedIndexChanged += new EventHandler(this.ACP_TypeComboBox_SelectedIndexChanged);
            // 
            // ACP_Type_SelectedOverrides
            // 
            this.ACP_Type_SelectedOverrides.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.ACP_Type_SelectedOverrides.AutoSize = true;
            this.ACP_Type_SelectedOverrides.Checked = true;
            this.ACP_Type_SelectedOverrides.CheckState = CheckState.Checked;
            this.ACP_Type_SelectedOverrides.Location = new Point(202, 70);
            this.ACP_Type_SelectedOverrides.Name = "ACP_Type_SelectedOverrides";
            this.ACP_Type_SelectedOverrides.Size = new Size(15, 14);
            this.ACP_Type_SelectedOverrides.TabIndex = 4;
            this.pasteOptions_tipper.SetToolTip(this.ACP_Type_SelectedOverrides, "If checked, the selected type will override all copied properties\' type.");
            this.ACP_Type_SelectedOverrides.UseVisualStyleBackColor = true;
            this.ACP_Type_SelectedOverrides.CheckedChanged += new EventHandler(this.ACP_Type_SelectedOverrides_CheckedChanged);
            // 
            // ACP_MaterialComboBox
            // 
            this.ACP_MaterialComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ACP_MaterialComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.ACP_MaterialComboBox.FormattingEnabled = true;
            this.ACP_MaterialComboBox.Location = new Point(6, 110);
            this.ACP_MaterialComboBox.Name = "ACP_MaterialComboBox";
            this.ACP_MaterialComboBox.Size = new Size(190, 21);
            this.ACP_MaterialComboBox.TabIndex = 2;
            // 
            // ACP_Material_SelectedOverrides
            // 
            this.ACP_Material_SelectedOverrides.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.ACP_Material_SelectedOverrides.AutoSize = true;
            this.ACP_Material_SelectedOverrides.Checked = true;
            this.ACP_Material_SelectedOverrides.CheckState = CheckState.Checked;
            this.ACP_Material_SelectedOverrides.Location = new Point(202, 113);
            this.ACP_Material_SelectedOverrides.Name = "ACP_Material_SelectedOverrides";
            this.ACP_Material_SelectedOverrides.Size = new Size(15, 14);
            this.ACP_Material_SelectedOverrides.TabIndex = 4;
            this.pasteOptions_tipper.SetToolTip(this.ACP_Material_SelectedOverrides, "If checked, the selected material will override all copied properties\' material.");
            this.ACP_Material_SelectedOverrides.UseVisualStyleBackColor = true;
            // 
            // ACP_CollTargetsGroup
            // 
            this.ACP_CollTargetsGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ACP_CollTargetsGroup.Controls.Add(this.ACP_CollTargets_Items);
            this.ACP_CollTargetsGroup.Controls.Add(this.ACP_CollTargets_PKMNTrainer);
            this.ACP_CollTargetsGroup.Controls.Add(this.ACP_CollTargets_Everything);
            this.ACP_CollTargetsGroup.Controls.Add(this.ACP_CollTargets_SelectedOverrides);
            this.ACP_CollTargetsGroup.Location = new Point(6, 139);
            this.ACP_CollTargetsGroup.Name = "ACP_CollTargetsGroup";
            this.ACP_CollTargetsGroup.Size = new Size(211, 60);
            this.ACP_CollTargetsGroup.TabIndex = 5;
            this.ACP_CollTargetsGroup.TabStop = false;
            this.ACP_CollTargetsGroup.Text = "Collision Targets?";
            // 
            // ACP_CollTargets_Items
            // 
            this.ACP_CollTargets_Items.AutoSize = true;
            this.ACP_CollTargets_Items.Location = new Point(113, 19);
            this.ACP_CollTargets_Items.Name = "ACP_CollTargets_Items";
            this.ACP_CollTargets_Items.Size = new Size(51, 17);
            this.ACP_CollTargets_Items.TabIndex = 4;
            this.ACP_CollTargets_Items.Text = "Items";
            this.ACP_CollTargets_Items.UseVisualStyleBackColor = true;
            this.ACP_CollTargets_Items.CheckedChanged += new EventHandler(this.ACP_CollTargets_Items_CheckedChanged);
            // 
            // ACP_CollTargets_PKMNTrainer
            // 
            this.ACP_CollTargets_PKMNTrainer.AutoSize = true;
            this.ACP_CollTargets_PKMNTrainer.Location = new Point(6, 38);
            this.ACP_CollTargets_PKMNTrainer.Name = "ACP_CollTargets_PKMNTrainer";
            this.ACP_CollTargets_PKMNTrainer.Size = new Size(107, 17);
            this.ACP_CollTargets_PKMNTrainer.TabIndex = 4;
            this.ACP_CollTargets_PKMNTrainer.Text = "Pokémon Trainer";
            this.ACP_CollTargets_PKMNTrainer.UseVisualStyleBackColor = true;
            this.ACP_CollTargets_PKMNTrainer.CheckedChanged += new EventHandler(this.ACP_CollTargets_PKMNTrainer_CheckedChanged);
            // 
            // ACP_CollTargets_Everything
            // 
            this.ACP_CollTargets_Everything.AutoSize = true;
            this.ACP_CollTargets_Everything.Location = new Point(6, 19);
            this.ACP_CollTargets_Everything.Name = "ACP_CollTargets_Everything";
            this.ACP_CollTargets_Everything.Size = new Size(76, 17);
            this.ACP_CollTargets_Everything.TabIndex = 4;
            this.ACP_CollTargets_Everything.Text = "Everything";
            this.ACP_CollTargets_Everything.UseVisualStyleBackColor = true;
            this.ACP_CollTargets_Everything.CheckedChanged += new EventHandler(this.ACP_CollTargets_Everything_CheckedChanged);
            // 
            // ACP_CollTargets_SelectedOverrides
            // 
            this.ACP_CollTargets_SelectedOverrides.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.ACP_CollTargets_SelectedOverrides.AutoSize = true;
            this.ACP_CollTargets_SelectedOverrides.Checked = true;
            this.ACP_CollTargets_SelectedOverrides.CheckState = CheckState.Checked;
            this.ACP_CollTargets_SelectedOverrides.Location = new Point(191, 0);
            this.ACP_CollTargets_SelectedOverrides.Name = "ACP_CollTargets_SelectedOverrides";
            this.ACP_CollTargets_SelectedOverrides.Size = new Size(15, 14);
            this.ACP_CollTargets_SelectedOverrides.TabIndex = 4;
            this.pasteOptions_tipper.SetToolTip(this.ACP_CollTargets_SelectedOverrides, "If checked, the selected collision targets will override all copied properties\' c" +
        "ollision targets.");
            this.ACP_CollTargets_SelectedOverrides.UseVisualStyleBackColor = true;
            this.ACP_CollTargets_SelectedOverrides.CheckedChanged += new EventHandler(this.ACP_CollTargets_SelectedOverrides_CheckedChanged);
            // 
            // ACP_TypeLabel
            // 
            this.ACP_TypeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ACP_TypeLabel.Location = new Point(6, 51);
            this.ACP_TypeLabel.Name = "ACP_TypeLabel";
            this.ACP_TypeLabel.Size = new Size(206, 13);
            this.ACP_TypeLabel.TabIndex = 3;
            this.ACP_TypeLabel.Text = "Type";
            // 
            // ACP_MaterialLabel
            // 
            this.ACP_MaterialLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ACP_MaterialLabel.Location = new Point(6, 94);
            this.ACP_MaterialLabel.Name = "ACP_MaterialLabel";
            this.ACP_MaterialLabel.Size = new Size(206, 13);
            this.ACP_MaterialLabel.TabIndex = 3;
            this.ACP_MaterialLabel.Text = "Material";
            // 
            // ACP_ResetValuesToDef
            // 
            this.ACP_ResetValuesToDef.Dock = DockStyle.Bottom;
            this.ACP_ResetValuesToDef.Enabled = false;
            this.ACP_ResetValuesToDef.Location = new Point(3, 355);
            this.ACP_ResetValuesToDef.Name = "ACP_ResetValuesToDef";
            this.ACP_ResetValuesToDef.Size = new Size(222, 23);
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
            this.BottomPanel.Location = new Point(0, 407);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new Size(236, 32);
            this.BottomPanel.TabIndex = 1;
            // 
            // Cancel
            // 
            this.Cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.Cancel.DialogResult = DialogResult.Cancel;
            this.Cancel.Location = new Point(156, 4);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new Size(75, 23);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new EventHandler(this.Cancel_Click);
            // 
            // PasteCollision
            // 
            this.PasteCollision.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.PasteCollision.Location = new Point(49, 4);
            this.PasteCollision.Name = "PasteCollision";
            this.PasteCollision.Size = new Size(104, 23);
            this.PasteCollision.TabIndex = 0;
            this.PasteCollision.Text = "Paste Collision";
            this.PasteCollision.UseVisualStyleBackColor = true;
            this.PasteCollision.Click += new EventHandler(this.PasteCollision_Click);
            // 
            // ManiColl_Rotate_DegreesLabel
            // 
            this.ManiColl_Rotate_DegreesLabel.AutoSize = true;
            this.ManiColl_Rotate_DegreesLabel.Location = new Point(95, 1);
            this.ManiColl_Rotate_DegreesLabel.Name = "ManiColl_Rotate_DegreesLabel";
            this.ManiColl_Rotate_DegreesLabel.Size = new Size(50, 13);
            this.ManiColl_Rotate_DegreesLabel.TabIndex = 2;
            this.ManiColl_Rotate_DegreesLabel.Text = "Degrees:";
            // 
            // BrawlCrate_PasteOptions_UI
            // 
            this.AcceptButton = this.PasteCollision;
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new Size(236, 439);
            this.Controls.Add(this.advancedPO_TabC);
            this.Controls.Add(this.BottomPanel);
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.MaximumSize = new Size(252, 478);
            this.MinimumSize = new Size(252, 252);
            this.Name = "CollisionEditor_Paste";
            this.Text = "Advanced Paste Options";
            this.advancedPO_TabC.ResumeLayout(false);
            this.advancedPO_TabC_ManipulateCollisionTab.ResumeLayout(false);
            this.ManiColl_PlacementGroup.ResumeLayout(false);
            this.ManiColl_PlacementGroup.PerformLayout();
            this.ManiColl_ScaleGroup.ResumeLayout(false);
            this.ManiColl_ScaleGroup.PerformLayout();
            ((ISupportInitialize)(this.ManiColl_Scale_ScalarValue)).EndInit();
            this.ManiColl_FlipGroup.ResumeLayout(false);
            this.ManiColl_FlipGroup.PerformLayout();
            this.ManiColl_RotateGroup.ResumeLayout(false);
            this.ManiColl_RotateGroup.PerformLayout();
            ((ISupportInitialize)(this.ManiColl_Rotate_DegreesValue)).EndInit();
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

        CollisionEditor parentMenu = null;

        public CollisionEditor_PasteOptions(CollisionEditor ce)
        {
            parentMenu = ce;
            InitializeComponent();

            this.ACP_MaterialComboBox.DataSource = ce.getMaterials();
            this.ACP_TypeComboBox.DataSource = ce.getCollisionPlaneTypes();
        }

        private void PasteCollision_Click(object sender, EventArgs e)
        {

        }

        private void Cancel_Click(object sender, EventArgs e)
        {

        }

        #region Manipulate Collision UI
        #region Rotate Section
        private void ManiColl_Rotate_DegreesValue_ValueChanged(object sender, EventArgs e)
        {

        }

        private void ManiColl_Rotate_SetTo0_Click(object sender, EventArgs e)
        {

        }

        private void ManiColl_Rotate_SetTo90_Click(object sender, EventArgs e)
        {

        }

        private void ManiColl_Rotate_SetTo180_Click(object sender, EventArgs e)
        {

        }

        private void ManiColl_Rotate_SetTo270_Click(object sender, EventArgs e)
        {

        }

        private void ManiColl_Rotate_SetCP_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Flip Section
        private void ManiColl_Flip_FlipX_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ManiColl_Flip_FlipY_CheckedChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region ScaleSection
        private void ManiColl_Scale_ScalarValue_ValueChanged(object sender, EventArgs e)
        {

        }

        private void ManiColl_Scale_ResetScale_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Location/Placement
        private void ManiColl_Placement_RadioCenterStage_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ManiColl_Placement_RadioFreeplace_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ManiColl_Placement_RadioSpefPlacement_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ManiColl_Placement_SpefPlacement_LocX_ValueChanged(object sender, EventArgs e)
        {

        }

        private void ManiColl_Placement_SpefPlacement_LocY_ValueChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #endregion

        #region All Collisions Properties UI
        #region General
        private void AllCollsProps_WarningLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
        private void ACP_ResetValuesToDef_Click(object sender, EventArgs e)
        {

        }
        private void ACP_TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ACP_Type_SelectedOverrides_CheckedChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Collision Targets Section
        private void ACP_CollTargets_Everything_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void ACP_CollTargets_Items_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void ACP_CollTargets_PKMNTrainer_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void ACP_CollTargets_SelectedOverrides_CheckedChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Collision Flags Section
        private void ACP_CollFlags_FallThrough_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void ACP_CollFlags_LeftLedge_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void ACP_CollFlags_RightLedge_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void ACP_CollFlags_NoWallJump_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void ACP_CollFlags_Rotating_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void ACP_CollFlags_SelectedOverrides_CheckedChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Unknown Flags Section
        private void ACP_UnkFlags_Unk1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ACP_UnkFlags_Unk2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ACP_UnkFlags_Unk3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ACP_UnkFlags_Unk4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ACP_UnkFlags_SelectedOverrides_CheckedChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #endregion
    }
}

