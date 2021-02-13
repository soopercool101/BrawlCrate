using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using System;
using System.Linq;
using System.Windows.Forms;

namespace BrawlCrate.UI
{
    public class AdvancedCollisionEditor : CollisionEditor
    {
        protected override bool ErrorChecking => false;

        #region Designer

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
            if (disposing)
            {
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        // Advanced collision type selectors
        private GroupBox groupBoxType;
        private CheckBox chkTypeFloor;
        private CheckBox chkTypeCeiling;
        private CheckBox chkTypeLeftWall;
        private CheckBox chkTypeRightWall;

        // Advanced collision editor unknown flag editors
        private GroupBox groupBoxUnknown;
        private Label flags0x0ELabel;
        private CheckBox chkFlagsMatUnk0x02;
        private CheckBox chkFlagsMatUnk0x10;
        private Label flags0x0CLabel;
        private CheckBox chkFlagsTypeUnk0x0200;
        private CheckBox chkFlagsTypeUnk0x0400;
        private CheckBox chkFlagsTypeUnk0x0800;
        private CheckBox chkFlagsTypeUnk0x1000;
        private CheckBox chkFlagsTypeUnk0x2000;
        private CheckBox chkFlagsTypeUnk0x4000;
        private CheckBox chkFlagsTypeUnk0x8000;

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private new void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // Advanced floor editor
            groupBoxType = new System.Windows.Forms.GroupBox();
            groupBoxType.SuspendLayout();
            chkTypeFloor = new System.Windows.Forms.CheckBox();
            chkTypeCeiling = new System.Windows.Forms.CheckBox();
            chkTypeLeftWall = new System.Windows.Forms.CheckBox();
            chkTypeRightWall = new System.Windows.Forms.CheckBox();
            // Advanced collision editor unknown flag editors
            groupBoxUnknown = new GroupBox();
            groupBoxUnknown.SuspendLayout();
            flags0x0ELabel = new Label();
            chkFlagsMatUnk0x02 = new CheckBox();
            chkFlagsMatUnk0x10 = new CheckBox();
            chkFlagsTypeUnk0x0200 = new CheckBox();
            chkFlagsTypeUnk0x0400 = new CheckBox();
            chkFlagsTypeUnk0x0800 = new CheckBox();
            flags0x0CLabel = new Label();
            chkFlagsTypeUnk0x1000 = new CheckBox();
            chkFlagsTypeUnk0x2000 = new CheckBox();
            chkFlagsTypeUnk0x4000 = new CheckBox();
            chkFlagsTypeUnk0x8000 = new CheckBox();

            pnlPlaneProps.SuspendLayout();

            // 
            // panel3
            // 
            panel3.Controls.Clear();
            panel3.Controls.Add(pnlPlaneProps);
            panel3.Controls.Add(pnlPointProps);
            panel3.Controls.Add(pnlObjProps);
            panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel3.Location = new System.Drawing.Point(0, 82);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(209, 115);
            // 
            // pnlPlaneProps
            // 
            pnlPlaneProps.Controls.Clear();
            pnlPlaneProps.Controls.Add(groupBoxUnknown);
            pnlPlaneProps.Controls.Add(groupBoxFlags2);
            pnlPlaneProps.Controls.Add(groupBoxFlags1);
            pnlPlaneProps.Controls.Add(groupBoxTargets);
            pnlPlaneProps.Controls.Add(cboMaterial);
            pnlPlaneProps.Controls.Add(label5);
            pnlPlaneProps.Controls.Add(groupBoxType);
            pnlPlaneProps.Dock = System.Windows.Forms.DockStyle.Top;
            pnlPlaneProps.Location = new System.Drawing.Point(0, -199);
            pnlPlaneProps.Name = "pnlPlaneProps";
            pnlPlaneProps.Size = new System.Drawing.Size(209, 514);
            pnlPlaneProps.Visible = false;
            // 
            // groupBoxType
            // 
            groupBoxType.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                   | System.Windows.Forms.AnchorStyles.Left);
            groupBoxType.Controls.Add(chkTypeFloor);
            groupBoxType.Controls.Add(chkTypeCeiling);
            groupBoxType.Controls.Add(chkTypeLeftWall);
            groupBoxType.Controls.Add(chkTypeRightWall);
            groupBoxType.Location = new System.Drawing.Point(0, 25);
            groupBoxType.Margin = new System.Windows.Forms.Padding(0);
            groupBoxType.Name = "groupBoxType";
            groupBoxType.Padding = new System.Windows.Forms.Padding(0);
            groupBoxType.Size = new System.Drawing.Size(208, 59);
            groupBoxType.TabStop = false;
            groupBoxType.Text = "Type";
            // 
            // chkTypeFloor
            // 
            chkTypeFloor.Location = new System.Drawing.Point(8, 17);
            chkTypeFloor.Margin = new System.Windows.Forms.Padding(0);
            chkTypeFloor.Name = "chkTypeFloor";
            chkTypeFloor.Size = new System.Drawing.Size(86, 18);
            chkTypeFloor.Text = "Floor";
            chkTypeFloor.UseVisualStyleBackColor = true;
            chkTypeFloor.CheckedChanged += new System.EventHandler(chkTypeFloor_CheckedChanged);
            // 
            // chkTypeCeiling
            // 
            chkTypeCeiling.Location = new System.Drawing.Point(112, 17);
            chkTypeCeiling.Margin = new System.Windows.Forms.Padding(0);
            chkTypeCeiling.Name = "chkTypeCeiling";
            chkTypeCeiling.Size = new System.Drawing.Size(86, 18);
            chkTypeCeiling.Text = "Ceiling";
            chkTypeCeiling.UseVisualStyleBackColor = true;
            chkTypeCeiling.CheckedChanged += new System.EventHandler(chkTypeCeiling_CheckedChanged);
            // 
            // chkTypeLeftWall
            // 
            chkTypeLeftWall.Location = new System.Drawing.Point(8, 33);
            chkTypeLeftWall.Margin = new System.Windows.Forms.Padding(0);
            chkTypeLeftWall.Name = "chkTypeLeftWall";
            chkTypeLeftWall.Size = new System.Drawing.Size(86, 18);
            chkTypeLeftWall.Text = "Left Wall";
            chkTypeLeftWall.UseVisualStyleBackColor = true;
            chkTypeLeftWall.CheckedChanged += new System.EventHandler(chkTypeLeftWall_CheckedChanged);
            // 
            // chkTypeRightWall
            // 
            chkTypeRightWall.Location = new System.Drawing.Point(112, 33);
            chkTypeRightWall.Margin = new System.Windows.Forms.Padding(0);
            chkTypeRightWall.Name = "chkTypeRightWall";
            chkTypeRightWall.Size = new System.Drawing.Size(86, 18);
            chkTypeRightWall.Text = "Right Wall";
            chkTypeRightWall.UseVisualStyleBackColor = true;
            chkTypeRightWall.CheckedChanged += new System.EventHandler(chkTypeRightWall_CheckedChanged);
            // 
            // groupBoxFlags1
            // 
            groupBoxFlags1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                     | System.Windows.Forms.AnchorStyles.Left);
            groupBoxFlags1.Location = new System.Drawing.Point(0, 128);
            groupBoxFlags1.Size = new System.Drawing.Size(104, 90);
            groupBoxFlags1.Name = "groupBoxFlags1";
            // 
            // groupBoxFlags2
            // 
            groupBoxFlags2.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                     | System.Windows.Forms.AnchorStyles.Left);
            groupBoxFlags2.Location = new System.Drawing.Point(104, 128);
            groupBoxFlags2.Size = new System.Drawing.Size(104, 90);
            groupBoxFlags2.Name = "groupBoxFlags2";
            // 
            // cboMaterial
            // 
            cboMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboMaterial.Location = new System.Drawing.Point(59, 4);
            cboMaterial.Name = "cboMaterial";
            cboMaterial.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)
                                  | System.Windows.Forms.AnchorStyles.Left);
            // 
            // label5
            // 
            label5.Location = new System.Drawing.Point(0, 4);
            label5.Name = "label5";
            //
            // groupBoxTargets
            // 
            groupBoxTargets.Location = new System.Drawing.Point(0, 76);
            groupBoxTargets.Name = "groupBoxTargets";
            // 
            // groupBoxUnknown
            // 
            groupBoxUnknown.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                      | System.Windows.Forms.AnchorStyles.Left);
            groupBoxUnknown.Controls.Add(chkFlagsMatUnk0x02);
            groupBoxUnknown.Controls.Add(chkFlagsMatUnk0x10);
            groupBoxUnknown.Controls.Add(chkFlagsTypeUnk0x0200);
            groupBoxUnknown.Controls.Add(chkFlagsTypeUnk0x0400);
            groupBoxUnknown.Controls.Add(chkFlagsTypeUnk0x0800);
            groupBoxUnknown.Controls.Add(chkFlagsTypeUnk0x1000);
            groupBoxUnknown.Controls.Add(chkFlagsTypeUnk0x2000);
            groupBoxUnknown.Controls.Add(chkFlagsTypeUnk0x4000);
            groupBoxUnknown.Controls.Add(chkFlagsTypeUnk0x8000);
            groupBoxUnknown.Controls.Add(flags0x0CLabel);
            groupBoxUnknown.Controls.Add(flags0x0ELabel);
            groupBoxUnknown.Location = new System.Drawing.Point(0, 210);
            groupBoxUnknown.Margin = new System.Windows.Forms.Padding(0);
            groupBoxUnknown.Name = "groupBoxUnknown";
            groupBoxUnknown.Padding = new System.Windows.Forms.Padding(0);
            groupBoxUnknown.Size = new System.Drawing.Size(208, 500);
            groupBoxUnknown.TabStop = false;
            groupBoxUnknown.Text = "Unknown/Unused Flags";
            // 
            // flags0x0CLabel
            // 
            flags0x0CLabel.Location = new System.Drawing.Point(0, 17);
            flags0x0CLabel.Name = "flags0x0CLabel";
            flags0x0CLabel.Size = new System.Drawing.Size(50, 18);
            flags0x0CLabel.Text = "0x0C:";
            flags0x0CLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkFlagsTypeUnk0x0200
            // 
            chkFlagsTypeUnk0x0200.Location = new System.Drawing.Point(35, 17);
            chkFlagsTypeUnk0x0200.Margin = new System.Windows.Forms.Padding(0);
            chkFlagsTypeUnk0x0200.Name = "chkFlagsTypeUnk0x0200";
            chkFlagsTypeUnk0x0200.Size = new System.Drawing.Size(43, 18);
            chkFlagsTypeUnk0x0200.Text = "bit1";
            chkFlagsTypeUnk0x0200.UseVisualStyleBackColor = true;
            chkFlagsTypeUnk0x0200.CheckedChanged += new System.EventHandler(chkFlagsTypeUnk0x0200_CheckedChanged);
            // 
            // chkFlagsTypeUnk0x0400
            // 
            chkFlagsTypeUnk0x0400.Location = new System.Drawing.Point(78, 17);
            chkFlagsTypeUnk0x0400.Margin = new System.Windows.Forms.Padding(0);
            chkFlagsTypeUnk0x0400.Name = "chkFlagsTypeUnk0x0400";
            chkFlagsTypeUnk0x0400.Size = new System.Drawing.Size(43, 18);
            chkFlagsTypeUnk0x0400.Text = "bit2";
            chkFlagsTypeUnk0x0400.UseVisualStyleBackColor = true;
            chkFlagsTypeUnk0x0400.CheckedChanged += new System.EventHandler(chkFlagsTypeUnk0x0400_CheckedChanged);
            // 
            // chkFlagsTypeUnk0x0800
            // 
            chkFlagsTypeUnk0x0800.Location = new System.Drawing.Point(121, 17);
            chkFlagsTypeUnk0x0800.Margin = new System.Windows.Forms.Padding(0);
            chkFlagsTypeUnk0x0800.Name = "chkFlagsTypeUnk0x0800";
            chkFlagsTypeUnk0x0800.Size = new System.Drawing.Size(43, 18);
            chkFlagsTypeUnk0x0800.Text = "bit3";
            chkFlagsTypeUnk0x0800.UseVisualStyleBackColor = true;
            chkFlagsTypeUnk0x0800.CheckedChanged += new System.EventHandler(chkFlagsTypeUnk0x0800_CheckedChanged);
            // 
            // chkFlagsTypeUnk0x1000
            // 
            chkFlagsTypeUnk0x1000.Location = new System.Drawing.Point(164, 17);
            chkFlagsTypeUnk0x1000.Margin = new System.Windows.Forms.Padding(0);
            chkFlagsTypeUnk0x1000.Name = "chkFlagsTypeUnk0x1000";
            chkFlagsTypeUnk0x1000.Size = new System.Drawing.Size(43, 18);
            chkFlagsTypeUnk0x1000.Text = "bit4";
            chkFlagsTypeUnk0x1000.UseVisualStyleBackColor = true;
            chkFlagsTypeUnk0x1000.CheckedChanged += new System.EventHandler(chkFlagsTypeUnk0x1000_CheckedChanged);
            // 
            // chkFlagsTypeUnk0x2000
            // 
            chkFlagsTypeUnk0x2000.Location = new System.Drawing.Point(35, 33);
            chkFlagsTypeUnk0x2000.Margin = new System.Windows.Forms.Padding(0);
            chkFlagsTypeUnk0x2000.Name = "chkFlagsTypeUnk0x2000";
            chkFlagsTypeUnk0x2000.Size = new System.Drawing.Size(43, 18);
            chkFlagsTypeUnk0x2000.Text = "bit5";
            chkFlagsTypeUnk0x2000.UseVisualStyleBackColor = true;
            chkFlagsTypeUnk0x2000.CheckedChanged += new System.EventHandler(chkFlagsTypeUnk0x2000_CheckedChanged);
            // 
            // chkFlagsTypeUnk0x4000
            // 
            chkFlagsTypeUnk0x4000.Location = new System.Drawing.Point(78, 33);
            chkFlagsTypeUnk0x4000.Margin = new System.Windows.Forms.Padding(0);
            chkFlagsTypeUnk0x4000.Name = "chkFlagsTypeUnk0x4000";
            chkFlagsTypeUnk0x4000.Size = new System.Drawing.Size(43, 18);
            chkFlagsTypeUnk0x4000.Text = "bit6";
            chkFlagsTypeUnk0x4000.UseVisualStyleBackColor = true;
            chkFlagsTypeUnk0x4000.CheckedChanged += new System.EventHandler(chkFlagsTypeUnk0x4000_CheckedChanged);
            // 
            // chkFlagsTypeUnk0x8000
            // 
            chkFlagsTypeUnk0x8000.Location = new System.Drawing.Point(121, 33);
            chkFlagsTypeUnk0x8000.Margin = new System.Windows.Forms.Padding(0);
            chkFlagsTypeUnk0x8000.Name = "chkFlagsTypeUnk0x8000";
            chkFlagsTypeUnk0x8000.Size = new System.Drawing.Size(43, 18);
            chkFlagsTypeUnk0x8000.Text = "bit7";
            chkFlagsTypeUnk0x8000.UseVisualStyleBackColor = true;
            chkFlagsTypeUnk0x8000.CheckedChanged += new System.EventHandler(chkFlagsTypeUnk0x8000_CheckedChanged);
            // 
            // flags0x0ELabel
            // 
            flags0x0ELabel.Location = new System.Drawing.Point(0, 49);
            flags0x0ELabel.Name = "flags0x0ELabel";
            flags0x0ELabel.Size = new System.Drawing.Size(50, 18);
            flags0x0ELabel.Text = "0x0E:";
            flags0x0ELabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkFlagsTypeUnk0x0200
            // 
            chkFlagsMatUnk0x02.Location = new System.Drawing.Point(35, 49);
            chkFlagsMatUnk0x02.Margin = new System.Windows.Forms.Padding(0);
            chkFlagsMatUnk0x02.Name = "chkFlagsMatUnk0x02";
            chkFlagsMatUnk0x02.Size = new System.Drawing.Size(43, 18);
            chkFlagsMatUnk0x02.Text = "bit1";
            chkFlagsMatUnk0x02.UseVisualStyleBackColor = true;
            chkFlagsMatUnk0x02.CheckedChanged += new System.EventHandler(chkFlagsMatUnk0x02_CheckedChanged);
            // 
            // chkFlagsTypeUnk0x0400
            // 
            chkFlagsMatUnk0x10.Location = new System.Drawing.Point(78, 49);
            chkFlagsMatUnk0x10.Margin = new System.Windows.Forms.Padding(0);
            chkFlagsMatUnk0x10.Name = "chkFlagsMatUnk0x10";
            chkFlagsMatUnk0x10.Size = new System.Drawing.Size(43, 18);
            chkFlagsMatUnk0x10.Text = "bit4";
            chkFlagsMatUnk0x10.UseVisualStyleBackColor = true;
            chkFlagsMatUnk0x10.CheckedChanged += new System.EventHandler(chkFlagsMatUnk0x10_CheckedChanged);

            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        }

        private void chkTypeFloor_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsFloor = chkTypeFloor.Checked;
            }
        }

        private void chkTypeCeiling_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsCeiling = chkTypeCeiling.Checked;
            }
        }

        private void chkTypeLeftWall_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsLeftWall = chkTypeLeftWall.Checked;
            }
        }

        private void chkTypeRightWall_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsRightWall = chkTypeRightWall.Checked;
            }
        }

        private void chkFlagsMatUnk0x02_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsMatUnknown0x02 = chkFlagsMatUnk0x02.Checked;
            }
        }

        private void chkFlagsMatUnk0x10_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsMatUnknown0x10 = chkFlagsMatUnk0x10.Checked;
            }
        }

        private void chkFlagsTypeUnk0x0200_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsTypeUnknown0x0200 = chkFlagsTypeUnk0x0200.Checked;
            }
        }

        private void chkFlagsTypeUnk0x0400_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsTypeUnknown0x0400 = chkFlagsTypeUnk0x0400.Checked;
            }
        }

        private void chkFlagsTypeUnk0x0800_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsTypeUnknown0x0800 = chkFlagsTypeUnk0x0800.Checked;
            }
        }

        private void chkFlagsTypeUnk0x1000_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsTypeUnknown0x1000 = chkFlagsTypeUnk0x1000.Checked;
            }
        }
        private void chkFlagsTypeUnk0x2000_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsTypeUnknown0x2000 = chkFlagsTypeUnk0x2000.Checked;
            }
        }
        private void chkFlagsTypeUnk0x4000_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsTypeUnknown0x4000 = chkFlagsTypeUnk0x4000.Checked;
            }
        }
        private void chkFlagsTypeUnk0x8000_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsTypeUnknown0x8000 = chkFlagsTypeUnk0x8000.Checked;
            }
        }

        #endregion

        #endregion

        public AdvancedCollisionEditor()
        {
            InitializeComponent();
            cboMaterial.DataSource = CollisionTerrain.Terrains.ToList();
        }

        protected override void SelectionModified()
        {
            _selectedPlanes.Clear();
            foreach (CollisionLink l in _selectedLinks)
            {
                foreach (CollisionPlane p in l._members)
                {
                    if (_selectedLinks.Contains(p._linkLeft) &&
                        _selectedLinks.Contains(p._linkRight) &&
                        !_selectedPlanes.Contains(p))
                    {
                        _selectedPlanes.Add(p);
                    }
                }
            }

            pnlPlaneProps.Visible = false;
            pnlObjProps.Visible = false;
            pnlPointProps.Visible = false;
            panel3.Height = 0;

            if (_selectedPlanes.Count > 0)
            {
                pnlPlaneProps.Visible = true;
                panel3.Height = 280; //230;
            }
            else if (_selectedLinks.Count == 1)
            {
                pnlPointProps.Visible = true;
                panel3.Height = 70;
            }

            UpdatePropPanels();
        }

        protected override void UpdatePropPanels()
        {
            _updating = true;

            if (pnlPlaneProps.Visible)
            {
                if (_selectedPlanes.Count <= 0)
                {
                    pnlPlaneProps.Visible = false;
                    return;
                }

                CollisionPlane p = _selectedPlanes[0];

                //Material
                cboMaterial.SelectedItem = cboMaterial.Items[p._material];
                //Type
                chkTypeFloor.Checked = p.IsFloor;
                chkTypeCeiling.Checked = p.IsCeiling;
                chkTypeLeftWall.Checked = p.IsLeftWall;
                chkTypeRightWall.Checked = p.IsRightWall;
                //Flags
                chkFallThrough.Checked = p.IsFallThrough;
                chkLeftLedge.Checked = p.IsLeftLedge;
                chkRightLedge.Checked = p.IsRightLedge;
                chkNoWalljump.Checked = p.IsNoWalljump;
                chkTypeCharacters.Checked = p.IsCharacters;
                chkTypeItems.Checked = p.IsItems;
                chkTypePokemonTrainer.Checked = p.IsPokemonTrainer;
                chkTypeRotating.Checked = p.IsRotating;
                chkFlagCrush.Checked = p.IsCrush;
                chkFlagSuperSoft.Checked = p.IsSuperSoft;
                chkFlagBucculus.Checked = p.IsBucculusBury;
                //UnknownFlags
                chkFlagsMatUnk0x02.Checked = p.IsMatUnknown0x02;
                chkFlagsMatUnk0x10.Checked = p.IsMatUnknown0x10;
                chkFlagsTypeUnk0x0200.Checked = p.IsTypeUnknown0x0200;
                chkFlagsTypeUnk0x0400.Checked = p.IsTypeUnknown0x0400;
                chkFlagsTypeUnk0x0800.Checked = p.IsTypeUnknown0x0800;
                chkFlagsTypeUnk0x1000.Checked = p.IsTypeUnknown0x1000;
                chkFlagsTypeUnk0x2000.Checked = p.IsTypeUnknown0x2000;
                chkFlagsTypeUnk0x4000.Checked = p.IsTypeUnknown0x4000;
                chkFlagsTypeUnk0x8000.Checked = p.IsTypeUnknown0x8000;
            }
            else if (pnlPointProps.Visible)
            {
                if (_selectedLinks.Count <= 0)
                {
                    pnlPointProps.Visible = false;
                    return;
                }

                numX.Value = _selectedLinks[0].Value._x;
                numY.Value = _selectedLinks[0].Value._y;
            }
            else if (pnlObjProps.Visible)
            {
                if (_selectedObject == null)
                {
                    pnlObjProps.Visible = false;
                    return;
                }

                txtModel.Text = _selectedObject._modelName;
                txtBone.Text = _selectedObject._boneName;
                chkObjUnk.Checked = _selectedObject.UnknownFlag;
                chkObjModule.Checked = _selectedObject.ModuleControlled;
                chkObjSSEUnk.Checked = _selectedObject.UnknownSSEFlag;
            }

            _updating = false;
        }
    }
}