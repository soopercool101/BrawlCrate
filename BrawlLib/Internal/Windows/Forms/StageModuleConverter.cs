using BrawlLib.Internal.IO;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    //Class coded by libertyernie.
    public class StageModuleConverter : Form
    {
        public const string FILTER = "Module files (*.rel)|*.rel";

        #region Definition of "Stage" inner class

        public class Stage
        {
            private readonly byte id;
            private readonly string name;
            private readonly string filename;

            public byte ID => id;
            public string Name => name;
            public string Filename => filename;

            public Stage(byte id, string name, string filename)
            {
                this.id = id;
                this.name = name;
                this.filename = filename;
            }

            public override string ToString()
            {
                return name;
            }
        }

        #endregion

        private static readonly Stage[] stageList = new Stage[]
        {
            new Stage(0, "STGCUSTOM##.pac", "st_custom##.rel"),
            new Stage(1, "Battlefield", "st_battle.rel"),
            new Stage(2, "Final Destination", "st_final.rel"),
            new Stage(3, "Delfino Plaza", "st_dolpic.rel"),
            new Stage(4, "Luigi's Mansion", "st_mansion.rel"),
            new Stage(5, "Mushroomy Kingdom", "st_mariopast.rel"),
            new Stage(6, "Mario Circuit", "st_kart.rel"),
            new Stage(7, "75 m", "st_donkey.rel"),
            new Stage(8, "Rumble Falls", "st_jungle.rel"),
            new Stage(9, "Pirate Ship", "st_pirates.rel"),
            new Stage(11, "Norfair", "st_norfair.rel"),
            new Stage(12, "Frigate Orpheon", "st_orpheon.rel"),
            new Stage(13, "Yoshi's Island (Brawl)", "st_crayon.rel"),
            new Stage(14, "Halberd", "st_halberd.rel"),
            new Stage(19, "Lylat Cruise", "st_starfox.rel"),
            new Stage(20, "Pokemon Stadium 2", "st_stadium.rel"),
            new Stage(21, "Spear Pillar", "st_tengan.rel"),
            new Stage(22, "Port Town Aero Dive", "st_fzero.rel"),
            new Stage(23, "Summit", "st_ice.rel"),
            new Stage(24, "Flat Zone 2", "st_gw.rel"),
            new Stage(25, "Castle Siege", "st_emblem.rel"),
            new Stage(28, "WarioWare Inc.", "st_madein.rel"),
            new Stage(29, "Distant Planet", "st_earth.rel"),
            new Stage(30, "Skyworld", "st_palutena.rel"),
            new Stage(31, "Mario Bros.", "st_famicom.rel"),
            new Stage(32, "New Pork City", "st_newpork.rel"),
            new Stage(33, "Smashville", "st_village.rel"),
            new Stage(34, "Shadow Moses Island", "st_metalgear.rel"),
            new Stage(35, "Green Hill Zone", "st_greenhill.rel"),
            new Stage(36, "PictoChat", "st_pictchat.rel"),
            new Stage(37, "Hanenbow", "st_plankton.rel"),
            new Stage(38, "ConfigTest", "st_config.rel"),
            new Stage(41, "Temple", "st_dxshrine.rel"),
            new Stage(42, "Yoshi's Island (Melee)", "st_dxyorster.rel"),
            new Stage(43, "Jungle Japes", "st_dxgarden.rel"),
            new Stage(44, "Onett", "st_dxonett.rel"),
            new Stage(45, "Green Greens", "st_dxgreens.rel"),
            new Stage(46, "Pokemon Stadium", "st_dxpstadium.rel"),
            new Stage(47, "Rainbow Cruise", "st_dxrcruise.rel"),
            new Stage(48, "Corneria", "st_dxcorneria.rel"),
            new Stage(49, "Big Blue", "st_dxbigblue.rel"),
            new Stage(50, "Brinstar", "st_dxzebes.rel"),
            new Stage(51, "Bridge of Eldin", "st_oldin.rel"),
            new Stage(52, "Homerun", "st_homerun.rel"),
            new Stage(53, "Edit", "st_stageedit.rel"),
            new Stage(54, "Heal", "st_heal.rel"),
            new Stage(55, "Online Training", "st_otrain.rel"),
            new Stage(56, "TargetBreak", "st_tbreak.rel")
        };

        private static readonly int[] indicesToIgnore =
        {
            2959, // st_croll (PAL)
            431,  // st_onett, st_metalgear
            387,  // st_dxyorster
            2519, // st_croll (NTSC)
            419,  // st_donkey
            423   // st_halberd, st_jungle, st_mansion
        };

        public static ReadOnlyCollection<Stage> StageList => Array.AsReadOnly(stageList);
        public static ReadOnlyCollection<int> IndicesToIgnore => Array.AsReadOnly(indicesToIgnore);

        #region Designer

        private Label lblOffsetValue;
        private Label lblOffsetDesc;
        private ComboBox stageSelection;
        private Label lblCurrentStageDesc;
        private GroupBox groupBox1;
        private Label lblCurrentStage;
        private Label lblNewStageDesc;
        private Button btnOkay;
        private Button btnCancel;
        private TextBox txtPath;
        private Panel pnlInfo;
        private Panel pnlEdit;
        private GroupBox groupBox2;
        private Panel panel3;
        private Panel panel4;
        private OpenFileDialog dlgOpen;
        private Timer tmrUpdate;
        private IContainer components;
        private Label lblSizeValue;
        private Label lblSizeDesc;
        private Label lblNameValue;
        private Button btnBrowse;
        private Label lblIDValue;
        private Label lblIDDesc;
        private Label label3;
        private ComboBox itemSelection;
        private Label lblItemDesc;

        private void InitializeComponent()
        {
            components = new Container();
            btnOkay = new Button();
            btnCancel = new Button();
            txtPath = new TextBox();
            btnBrowse = new Button();
            lblOffsetValue = new Label();
            lblOffsetDesc = new Label();
            lblNameValue = new Label();
            lblSizeValue = new Label();
            lblSizeDesc = new Label();
            pnlInfo = new Panel();
            groupBox1 = new GroupBox();
            lblIDValue = new Label();
            lblIDDesc = new Label();
            panel4 = new Panel();
            pnlEdit = new Panel();
            groupBox2 = new GroupBox();
            itemSelection = new ComboBox();
            lblItemDesc = new Label();
            label3 = new Label();
            lblCurrentStage = new Label();
            lblNewStageDesc = new Label();
            stageSelection = new ComboBox();
            lblCurrentStageDesc = new Label();
            panel3 = new Panel();
            dlgOpen = new OpenFileDialog();
            tmrUpdate = new Timer(components);
            pnlInfo.SuspendLayout();
            groupBox1.SuspendLayout();
            panel4.SuspendLayout();
            pnlEdit.SuspendLayout();
            groupBox2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // btnOkay
            // 
            btnOkay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOkay.Enabled = false;
            btnOkay.Location = new System.Drawing.Point(3, 3);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new System.Drawing.Size(75, 23);
            btnOkay.TabIndex = 0;
            btnOkay.Text = "Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new EventHandler(btnOkay_Click);
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(80, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // txtPath
            // 
            txtPath.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                              | AnchorStyles.Right;
            txtPath.Location = new System.Drawing.Point(0, 0);
            txtPath.Name = "txtPath";
            txtPath.ReadOnly = true;
            txtPath.Size = new System.Drawing.Size(222, 20);
            txtPath.TabIndex = 2;
            // 
            // btnBrowse
            // 
            btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowse.Location = new System.Drawing.Point(227, 0);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new System.Drawing.Size(25, 20);
            btnBrowse.TabIndex = 3;
            btnBrowse.Text = "...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += new EventHandler(btnBrowse_Click);
            // 
            // lblOffsetValue
            // 
            lblOffsetValue.Location = new System.Drawing.Point(56, 56);
            lblOffsetValue.Name = "lblOffsetValue";
            lblOffsetValue.Size = new System.Drawing.Size(96, 20);
            lblOffsetValue.TabIndex = 5;
            lblOffsetValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOffsetDesc
            // 
            lblOffsetDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point, 0);
            lblOffsetDesc.Location = new System.Drawing.Point(6, 56);
            lblOffsetDesc.Name = "lblOffsetDesc";
            lblOffsetDesc.Size = new System.Drawing.Size(48, 20);
            lblOffsetDesc.TabIndex = 4;
            lblOffsetDesc.Text = "Offset:";
            lblOffsetDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNameValue
            // 
            lblNameValue.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, 0);
            lblNameValue.Location = new System.Drawing.Point(7, 16);
            lblNameValue.Name = "lblNameValue";
            lblNameValue.Size = new System.Drawing.Size(145, 20);
            lblNameValue.TabIndex = 3;
            lblNameValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSizeValue
            // 
            lblSizeValue.Location = new System.Drawing.Point(56, 36);
            lblSizeValue.Name = "lblSizeValue";
            lblSizeValue.Size = new System.Drawing.Size(96, 20);
            lblSizeValue.TabIndex = 1;
            lblSizeValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSizeDesc
            // 
            lblSizeDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point, 0);
            lblSizeDesc.Location = new System.Drawing.Point(6, 36);
            lblSizeDesc.Name = "lblSizeDesc";
            lblSizeDesc.Size = new System.Drawing.Size(48, 20);
            lblSizeDesc.TabIndex = 0;
            lblSizeDesc.Text = "Size:";
            lblSizeDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlInfo
            // 
            pnlInfo.Controls.Add(groupBox1);
            pnlInfo.Controls.Add(panel4);
            pnlInfo.Dock = DockStyle.Right;
            pnlInfo.Location = new System.Drawing.Point(256, 0);
            pnlInfo.Name = "pnlInfo";
            pnlInfo.Size = new System.Drawing.Size(158, 132);
            pnlInfo.TabIndex = 9;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lblIDValue);
            groupBox1.Controls.Add(lblIDDesc);
            groupBox1.Controls.Add(lblOffsetValue);
            groupBox1.Controls.Add(lblOffsetDesc);
            groupBox1.Controls.Add(lblNameValue);
            groupBox1.Controls.Add(lblSizeValue);
            groupBox1.Controls.Add(lblSizeDesc);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(158, 103);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "File Info";
            // 
            // lblIDValue
            // 
            lblIDValue.Location = new System.Drawing.Point(56, 76);
            lblIDValue.Name = "lblIDValue";
            lblIDValue.Size = new System.Drawing.Size(96, 20);
            lblIDValue.TabIndex = 7;
            lblIDValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblIDDesc
            // 
            lblIDDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point, 0);
            lblIDDesc.Location = new System.Drawing.Point(6, 76);
            lblIDDesc.Name = "lblIDDesc";
            lblIDDesc.Size = new System.Drawing.Size(48, 20);
            lblIDDesc.TabIndex = 6;
            lblIDDesc.Text = "ID:";
            lblIDDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel4
            // 
            panel4.Controls.Add(btnOkay);
            panel4.Controls.Add(btnCancel);
            panel4.Dock = DockStyle.Bottom;
            panel4.Location = new System.Drawing.Point(0, 103);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(158, 29);
            panel4.TabIndex = 6;
            // 
            // pnlEdit
            // 
            pnlEdit.Controls.Add(groupBox2);
            pnlEdit.Controls.Add(panel3);
            pnlEdit.Dock = DockStyle.Fill;
            pnlEdit.Location = new System.Drawing.Point(0, 0);
            pnlEdit.Name = "pnlEdit";
            pnlEdit.Size = new System.Drawing.Size(256, 132);
            pnlEdit.TabIndex = 10;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(itemSelection);
            groupBox2.Controls.Add(lblItemDesc);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(lblCurrentStage);
            groupBox2.Controls.Add(lblNewStageDesc);
            groupBox2.Controls.Add(stageSelection);
            groupBox2.Controls.Add(lblCurrentStageDesc);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new System.Drawing.Point(0, 20);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(256, 112);
            groupBox2.TabIndex = 13;
            groupBox2.TabStop = false;
            groupBox2.Text = "Options";
            // 
            // itemSelection
            // 
            itemSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            itemSelection.Enabled = false;
            itemSelection.FormattingEnabled = true;
            itemSelection.Items.AddRange(new object[]
            {
                "Assist Trophy",
                "Franklin Badge",
                "Banana Peel",
                "Barrel",
                "Beam Sword",
                "Bill (coin mode)",
                "Bob-Omb",
                "Crate",
                "Bumper",
                "Capsule",
                "Rolling Crate",
                "CD",
                "Gooey Bomb",
                "Cracker Launcher",
                "Cracker Launcher Shot",
                "Coin",
                "Superspicy Curry",
                "Superspice Curry Shot",
                "Deku Nut",
                "Mr. Saturn",
                "Dragoon Part",
                "Dragoon Set",
                "Dragoon Sight",
                "Trophy",
                "Fire Flower",
                "Fire Flower Shot",
                "Freezie",
                "Golden Hammer",
                "Green Shell",
                "Hammer",
                "Hammer Head",
                "Fan",
                "Heart Container",
                "Homerun Bat",
                "Party Ball",
                "Manaphy Heart",
                "Maxim Tomato",
                "Poison Mushroom",
                "Super Mushroom",
                "Metal Box",
                "Hothead",
                "Pitfall",
                "Pokéball",
                "Blast Box",
                "Ray Gun",
                "Ray Gun Shot",
                "Lipstick",
                "Lipstick Flower",
                "Lipstick Shot",
                "Sandbag",
                "Screw Attack",
                "Sticker",
                "Motion-Sensor Bomb",
                "Timer",
                "Smart Bomb",
                "Smash Ball",
                "Smoke Screen",
                "Spring",
                "Star Rod",
                "Star Rod Shot",
                "Soccer Ball",
                "Super Scope",
                "Super Scope shot",
                "Star",
                "Food",
                "Team Healer",
                "Lightning",
                "Unira",
                "Bunny Hood",
                "Warpstar"
            });
            itemSelection.Location = new System.Drawing.Point(84, 62);
            itemSelection.Name = "itemSelection";
            itemSelection.Size = new System.Drawing.Size(166, 21);
            itemSelection.TabIndex = 6;
            // 
            // lblItemDesc
            // 
            lblItemDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point, 0);
            lblItemDesc.Location = new System.Drawing.Point(6, 62);
            lblItemDesc.Name = "lblItemDesc";
            lblItemDesc.Size = new System.Drawing.Size(72, 21);
            lblItemDesc.TabIndex = 5;
            lblItemDesc.Text = "Item:";
            lblItemDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = System.Drawing.SystemColors.ControlDark;
            label3.Location = new System.Drawing.Point(4, 93);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(209, 13);
            label3.TabIndex = 4;
            label3.Text = "Mouse over the labels for more information.";
            // 
            // lblCurrentStage
            // 
            lblCurrentStage.Location = new System.Drawing.Point(84, 14);
            lblCurrentStage.Name = "lblCurrentStage";
            lblCurrentStage.Size = new System.Drawing.Size(166, 21);
            lblCurrentStage.TabIndex = 3;
            lblCurrentStage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNewStageDesc
            // 
            lblNewStageDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point, 0);
            lblNewStageDesc.Location = new System.Drawing.Point(6, 38);
            lblNewStageDesc.Name = "lblNewStageDesc";
            lblNewStageDesc.Size = new System.Drawing.Size(72, 21);
            lblNewStageDesc.TabIndex = 2;
            lblNewStageDesc.Text = "New stage:";
            lblNewStageDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // stageSelection
            // 
            stageSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            stageSelection.Enabled = false;
            stageSelection.FormattingEnabled = true;
            stageSelection.Location = new System.Drawing.Point(84, 38);
            stageSelection.Name = "stageSelection";
            stageSelection.Size = new System.Drawing.Size(166, 21);
            stageSelection.TabIndex = 1;
            // 
            // lblCurrentStageDesc
            // 
            lblCurrentStageDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point, 0);
            lblCurrentStageDesc.Location = new System.Drawing.Point(6, 14);
            lblCurrentStageDesc.Name = "lblCurrentStageDesc";
            lblCurrentStageDesc.Size = new System.Drawing.Size(72, 21);
            lblCurrentStageDesc.TabIndex = 0;
            lblCurrentStageDesc.Text = "Current:";
            lblCurrentStageDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            panel3.Controls.Add(txtPath);
            panel3.Controls.Add(btnBrowse);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new System.Drawing.Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(256, 20);
            panel3.TabIndex = 14;
            // 
            // tmrUpdate
            // 
            tmrUpdate.Interval = 10;
            // 
            // StageRelSwitcherDialog
            // 
            ClientSize = new System.Drawing.Size(414, 132);
            Controls.Add(pnlEdit);
            Controls.Add(pnlInfo);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "StageRelSwitcherDialog";
            Text = "Stage REL Switcher";
            pnlInfo.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            pnlEdit.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private string _path;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Path
        {
            get => _path;
            set => _path = value;
        }

        private byte[] _data;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public byte[] Data => _data;

        public unsafe FileMap ToFileMap()
        {
            FileMap map = FileMap.FromTempFile(_data.Length);
            byte* ptr = (byte*) map.Address;
            for (int i = 0; i < _data.Length; i++)
            {
                ptr[i] = _data[i];
            }

            return map;
        }

        private string _outputName;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string OutputName => _outputName;

        private int _stageIDOffset = -1;

        public StageModuleConverter()
        {
            InitializeComponent();

            stageSelection.Items.AddRange(stageList);
            stageSelection.SelectedIndex = 0;

            #region Tooltips

            ToolTip tooltip = new ToolTip(new Container());
            tooltip.SetToolTip(lblSizeDesc, "The size of the REL file, in bytes.");
            tooltip.SetToolTip(lblSizeValue, "The size of the REL file, in bytes.");
            tooltip.SetToolTip(lblNameValue, "The internal name of the REL file (pointer at 0x74.)");
            tooltip.SetToolTip(lblOffsetDesc, "The offset of the stage ID, in bytes.");
            tooltip.SetToolTip(lblOffsetValue, "The offset of the stage ID, in bytes.");
            tooltip.SetToolTip(lblIDDesc,
                "The stage ID (automatically detected, with certain hard-coded exceptions for NTSC-U and PAL files.)");
            tooltip.SetToolTip(lblIDValue,
                "The stage ID (automatically detected, with certain hard-coded exceptions for NTSC-U and PAL files.)");
            tooltip.SetToolTip(lblCurrentStageDesc,
                "The current target stage of this REL file, as determined by its ID.");
            tooltip.SetToolTip(lblCurrentStage, "The current target stage of this REL file, as determined by its ID.");
            tooltip.SetToolTip(lblNewStageDesc, "The new target stage for this REL file.");
            tooltip.SetToolTip(stageSelection, "The new target stage for this REL file.");
            tooltip.SetToolTip(lblItemDesc,
                "The item to auto-spawn on the stage. Enabled when you use a StOnlineTrainning base. Replaces four bytes (offsets are hard-coded.)");
            tooltip.SetToolTip(itemSelection,
                "The item to auto-spawn on the stage. Enabled when you use a StOnlineTrainning base. Replaces four bytes (offsets are hard-coded.)");

            #endregion

            dlgOpen.Filter = FILTER;
        }

        public new DialogResult ShowDialog(IWin32Window owner)
        {
            DialogResult = DialogResult.Cancel;
            try
            {
                return base.ShowDialog(owner);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
                return DialogResult.Cancel;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            if (_path == null)
            {
                // Do not try to load a file if this object was made without defining a path. Just show an empty dialog.
            }
            else if (!LoadFile(_path))
            {
                // If it was made with a path but the file doesn't work, then close.
                // Note that the dialog won't close if you use the browse button to select an invalid file after it has been opened with a valid one.
                Close();
                return;
            }

            base.OnShown(e);
        }

        private bool LoadFile()
        {
            if (dlgOpen.ShowDialog(this) == DialogResult.OK)
            {
                return LoadFile(dlgOpen.FileName);
            }

            return false;
        }

        private bool LoadFile(string path)
        {
            // reset stuff
            _data = null;
            _stageIDOffset = -1;
            stageSelection.Enabled = false;
            itemSelection.Enabled = false;
            lblCurrentStage.Text = lblIDValue.Text = lblOffsetValue.Text = "?";
            btnOkay.Enabled = false;

            _path = path;

            txtPath.Text = _path;
            _data = new byte[new FileInfo(_path).Length];
            FileStream input = new FileStream(_path, FileMode.Open,
                FileAccess.Read, FileShare.Read, 8, FileOptions.SequentialScan);
            input.Read(_data, 0, _data.Length);
            input.Dispose();

            lblSizeValue.Text = _data.Length.ToString();
            lblNameValue.Text = getModuleName();
            _stageIDOffset = findStageIDOffset();
            if (_stageIDOffset < 0)
            {
                MessageBox.Show("Could not find the stage ID offset.");
                return false;
            }

            lblOffsetValue.Text = "0x" + Convert.ToString(_stageIDOffset, 16);
            byte currentID = findCurrentID();
            lblIDValue.Text = "0x" + Convert.ToString(currentID, 16);

            foreach (Stage s in stageList)
            {
                if (s.ID == currentID)
                {
                    lblCurrentStage.Text = s.Name;
                    stageSelection.SelectedItem = s;
                    break;
                }
            }

            if (lblNameValue.Text.StartsWith("stOnline"))
            {
                itemSelection.Enabled = true;
                itemSelection.SelectedIndex = _data[1223];
                lblCurrentStage.Text += " / " + itemSelection.Text;
            }
            else
            {
                itemSelection.Enabled = false;
            }

            stageSelection.Enabled = true;
            btnOkay.Enabled = true;
            return true;
        }

        private byte findCurrentID()
        {
            return _data[_stageIDOffset];
        }

        private string getModuleName()
        {
            int offset = _data[116];
            for (int i = 117; i < 120; i++)
            {
                offset *= 256;
                offset += _data[i];
            }

            StringBuilder sb = new StringBuilder();
            while (_data[offset] == 0)
            {
                offset++;
            }

            while (_data[offset] != 0)
            {
                sb.Append((char) _data[offset]);
                offset++;
            }

            return sb.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            Stage s = (Stage) stageSelection.SelectedItem;
            _data[_stageIDOffset] = s.ID;
            if (itemSelection.Enabled)
            {
                byte b = (byte) itemSelection.SelectedIndex;
                int[] offsets = {1223, 1371, 1347, 1627};
                for (int i = 0; i < offsets.Length; i++)
                {
                    _data[offsets[i]] = b;
                }
            }

            _outputName = s.Filename;

            DialogResult = DialogResult.OK;
            Close();
        }

        private int findStageIDOffset()
        {
            // search through pointer
            int length = _data.Length;
            byte[] searchFor = {0x38, 0xa5, 0x00, 0x00, 0x38, 0x80, 0x00};
            int indexToCheck = 0;
            bool found = false;

            int i = 0;
            while (!found && i < length)
            {
                if (_data[i] == searchFor[indexToCheck])
                {
                    indexToCheck++;
                    if (indexToCheck == searchFor.Length)
                    {
                        if (IndicesToIgnore.Contains(i + 1))
                        {
                            //MessageBox.Show("ignored " + (i + 1));
                            indexToCheck = 0;
                        }
                        else
                        {
                            found = true;
                        }
                    }
                }
                else
                {
                    indexToCheck = 0;
                }

                i++;
            }

            if (found)
            {
                return i;
            }

            return -1;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            LoadFile();
        }
    }
}