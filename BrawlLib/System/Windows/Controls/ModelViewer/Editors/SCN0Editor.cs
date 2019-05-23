using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System.Drawing;
using BrawlLib.Wii.Graphics;
using BrawlLib.SSBBTypes;
using BrawlLib.Modeling;

namespace System.Windows.Forms
{
    public class SCN0Editor : UserControl
    {
        #region Designer
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.LightSets = new System.Windows.Forms.TabPage();
            this.cboLight7 = new System.Windows.Forms.ComboBox();
            this.cboLight6 = new System.Windows.Forms.ComboBox();
            this.cboLight5 = new System.Windows.Forms.ComboBox();
            this.cboLight4 = new System.Windows.Forms.ComboBox();
            this.cboLight3 = new System.Windows.Forms.ComboBox();
            this.cboLight2 = new System.Windows.Forms.ComboBox();
            this.cboLight1 = new System.Windows.Forms.ComboBox();
            this.cboLight0 = new System.Windows.Forms.ComboBox();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.cboAmb = new System.Windows.Forms.ComboBox();
            this.AmbLights = new System.Windows.Forms.TabPage();
            this.chkAmbAlpha = new System.Windows.Forms.CheckBox();
            this.chkAmbClr = new System.Windows.Forms.CheckBox();
            this.Lights = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lightCut = new System.Windows.Forms.Button();
            this.lightPaste = new System.Windows.Forms.Button();
            this.lightCopy = new System.Windows.Forms.Button();
            this.lightClear = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.numSpotBright = new System.Windows.Forms.NumericInputBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.numStartZ = new System.Windows.Forms.NumericInputBox();
            this.numRefBright = new System.Windows.Forms.NumericInputBox();
            this.numSpotCut = new System.Windows.Forms.NumericInputBox();
            this.label19 = new System.Windows.Forms.Label();
            this.numEndY = new System.Windows.Forms.NumericInputBox();
            this.label20 = new System.Windows.Forms.Label();
            this.numEndX = new System.Windows.Forms.NumericInputBox();
            this.label21 = new System.Windows.Forms.Label();
            this.numRefDist = new System.Windows.Forms.NumericInputBox();
            this.label22 = new System.Windows.Forms.Label();
            this.numStartX = new System.Windows.Forms.NumericInputBox();
            this.numStartY = new System.Windows.Forms.NumericInputBox();
            this.numEndZ = new System.Windows.Forms.NumericInputBox();
            this.label23 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cboSpotFunc = new System.Windows.Forms.ComboBox();
            this.cboDistFunc = new System.Windows.Forms.ComboBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.chkLightSpec = new System.Windows.Forms.CheckBox();
            this.chkLightAlpha = new System.Windows.Forms.CheckBox();
            this.chkLightClr = new System.Windows.Forms.CheckBox();
            this.label27 = new System.Windows.Forms.Label();
            this.cboLightType = new System.Windows.Forms.ComboBox();
            this.Fog = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.numFogEndZ = new System.Windows.Forms.NumericInputBox();
            this.numFogStartZ = new System.Windows.Forms.NumericInputBox();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cboFogType = new System.Windows.Forms.ComboBox();
            this.Cameras = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.numPosZ = new System.Windows.Forms.NumericInputBox();
            this.btnCut = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.numAimX = new System.Windows.Forms.NumericInputBox();
            this.numFarZ = new System.Windows.Forms.NumericInputBox();
            this.numRotY = new System.Windows.Forms.NumericInputBox();
            this.numNearZ = new System.Windows.Forms.NumericInputBox();
            this.numRotX = new System.Windows.Forms.NumericInputBox();
            this.label12 = new System.Windows.Forms.Label();
            this.numTwist = new System.Windows.Forms.NumericInputBox();
            this.label13 = new System.Windows.Forms.Label();
            this.numPosX = new System.Windows.Forms.NumericInputBox();
            this.numAspect = new System.Windows.Forms.NumericInputBox();
            this.numRotZ = new System.Windows.Forms.NumericInputBox();
            this.numHeight = new System.Windows.Forms.NumericInputBox();
            this.numPosY = new System.Windows.Forms.NumericInputBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numAimY = new System.Windows.Forms.NumericInputBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numAimZ = new System.Windows.Forms.NumericInputBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numFovY = new System.Windows.Forms.NumericInputBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUseCamera = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboCamProj = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cboCamType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.nodeType = new System.Windows.Forms.Label();
            this.cboNodeList = new System.Windows.Forms.ComboBox();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.LightSets.SuspendLayout();
            this.AmbLights.SuspendLayout();
            this.Lights.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.Fog.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.Cameras.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.LightSets);
            this.tabControl1.Controls.Add(this.AmbLights);
            this.tabControl1.Controls.Add(this.Lights);
            this.tabControl1.Controls.Add(this.Fog);
            this.tabControl1.Controls.Add(this.Cameras);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(659, 126);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // LightSets
            // 
            this.LightSets.Controls.Add(this.cboLight7);
            this.LightSets.Controls.Add(this.cboLight6);
            this.LightSets.Controls.Add(this.cboLight5);
            this.LightSets.Controls.Add(this.cboLight4);
            this.LightSets.Controls.Add(this.cboLight3);
            this.LightSets.Controls.Add(this.cboLight2);
            this.LightSets.Controls.Add(this.cboLight1);
            this.LightSets.Controls.Add(this.cboLight0);
            this.LightSets.Controls.Add(this.label38);
            this.LightSets.Controls.Add(this.label37);
            this.LightSets.Controls.Add(this.label34);
            this.LightSets.Controls.Add(this.label35);
            this.LightSets.Controls.Add(this.label36);
            this.LightSets.Controls.Add(this.label32);
            this.LightSets.Controls.Add(this.label31);
            this.LightSets.Controls.Add(this.label33);
            this.LightSets.Controls.Add(this.label30);
            this.LightSets.Controls.Add(this.cboAmb);
            this.LightSets.Location = new System.Drawing.Point(4, 25);
            this.LightSets.Name = "LightSets";
            this.LightSets.Padding = new System.Windows.Forms.Padding(3);
            this.LightSets.Size = new System.Drawing.Size(651, 97);
            this.LightSets.TabIndex = 0;
            this.LightSets.Text = "LightSets";
            this.LightSets.UseVisualStyleBackColor = true;
            // 
            // cboLight7
            // 
            this.cboLight7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLight7.FormattingEnabled = true;
            this.cboLight7.Location = new System.Drawing.Point(536, 20);
            this.cboLight7.Name = "cboLight7";
            this.cboLight7.Size = new System.Drawing.Size(82, 21);
            this.cboLight7.TabIndex = 19;
            this.cboLight7.SelectedIndexChanged += new System.EventHandler(this.lstLight7_SelectedIndexChanged);
            // 
            // cboLight6
            // 
            this.cboLight6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLight6.FormattingEnabled = true;
            this.cboLight6.Location = new System.Drawing.Point(536, 0);
            this.cboLight6.Name = "cboLight6";
            this.cboLight6.Size = new System.Drawing.Size(82, 21);
            this.cboLight6.TabIndex = 18;
            this.cboLight6.SelectedIndexChanged += new System.EventHandler(this.lstLight6_SelectedIndexChanged);
            // 
            // cboLight5
            // 
            this.cboLight5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLight5.FormattingEnabled = true;
            this.cboLight5.Location = new System.Drawing.Point(402, 20);
            this.cboLight5.Name = "cboLight5";
            this.cboLight5.Size = new System.Drawing.Size(82, 21);
            this.cboLight5.TabIndex = 17;
            this.cboLight5.SelectedIndexChanged += new System.EventHandler(this.lstLight5_SelectedIndexChanged);
            // 
            // cboLight4
            // 
            this.cboLight4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLight4.FormattingEnabled = true;
            this.cboLight4.Location = new System.Drawing.Point(402, 0);
            this.cboLight4.Name = "cboLight4";
            this.cboLight4.Size = new System.Drawing.Size(82, 21);
            this.cboLight4.TabIndex = 16;
            this.cboLight4.SelectedIndexChanged += new System.EventHandler(this.lstLight4_SelectedIndexChanged);
            // 
            // cboLight3
            // 
            this.cboLight3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLight3.FormattingEnabled = true;
            this.cboLight3.Location = new System.Drawing.Point(268, 20);
            this.cboLight3.Name = "cboLight3";
            this.cboLight3.Size = new System.Drawing.Size(82, 21);
            this.cboLight3.TabIndex = 15;
            this.cboLight3.SelectedIndexChanged += new System.EventHandler(this.lstLight3_SelectedIndexChanged);
            // 
            // cboLight2
            // 
            this.cboLight2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLight2.FormattingEnabled = true;
            this.cboLight2.Location = new System.Drawing.Point(268, 0);
            this.cboLight2.Name = "cboLight2";
            this.cboLight2.Size = new System.Drawing.Size(82, 21);
            this.cboLight2.TabIndex = 14;
            this.cboLight2.SelectedIndexChanged += new System.EventHandler(this.lstLight2_SelectedIndexChanged);
            // 
            // cboLight1
            // 
            this.cboLight1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLight1.FormattingEnabled = true;
            this.cboLight1.Location = new System.Drawing.Point(134, 20);
            this.cboLight1.Name = "cboLight1";
            this.cboLight1.Size = new System.Drawing.Size(82, 21);
            this.cboLight1.TabIndex = 13;
            this.cboLight1.SelectedIndexChanged += new System.EventHandler(this.lstLight1_SelectedIndexChanged);
            // 
            // cboLight0
            // 
            this.cboLight0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLight0.FormattingEnabled = true;
            this.cboLight0.Location = new System.Drawing.Point(134, 0);
            this.cboLight0.Name = "cboLight0";
            this.cboLight0.Size = new System.Drawing.Size(82, 21);
            this.cboLight0.TabIndex = 12;
            this.cboLight0.SelectedIndexChanged += new System.EventHandler(this.lstLight0_SelectedIndexChanged);
            // 
            // label38
            // 
            this.label38.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label38.Location = new System.Drawing.Point(483, 20);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(54, 21);
            this.label38.TabIndex = 10;
            this.label38.Text = "Light 7:";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label37
            // 
            this.label37.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label37.Location = new System.Drawing.Point(483, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(54, 21);
            this.label37.TabIndex = 9;
            this.label37.Text = "Light 6:";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label34
            // 
            this.label34.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label34.Location = new System.Drawing.Point(349, 20);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(54, 21);
            this.label34.TabIndex = 8;
            this.label34.Text = "Light 5:";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label35
            // 
            this.label35.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label35.Location = new System.Drawing.Point(349, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(54, 21);
            this.label35.TabIndex = 7;
            this.label35.Text = "Light 4:";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label36
            // 
            this.label36.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label36.Location = new System.Drawing.Point(215, 20);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(54, 21);
            this.label36.TabIndex = 6;
            this.label36.Text = "Light 3:";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label32
            // 
            this.label32.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label32.Location = new System.Drawing.Point(215, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(54, 21);
            this.label32.TabIndex = 5;
            this.label32.Text = "Light 2:";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label31
            // 
            this.label31.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label31.Location = new System.Drawing.Point(81, 20);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(54, 21);
            this.label31.TabIndex = 4;
            this.label31.Text = "Light 1:";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label33
            // 
            this.label33.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label33.Location = new System.Drawing.Point(81, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(54, 21);
            this.label33.TabIndex = 3;
            this.label33.Text = "Light 0:";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label30
            // 
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label30.Location = new System.Drawing.Point(0, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(82, 21);
            this.label30.TabIndex = 0;
            this.label30.Text = "Ambient: ";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboAmb
            // 
            this.cboAmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAmb.FormattingEnabled = true;
            this.cboAmb.Location = new System.Drawing.Point(0, 20);
            this.cboAmb.Name = "cboAmb";
            this.cboAmb.Size = new System.Drawing.Size(82, 21);
            this.cboAmb.TabIndex = 11;
            this.cboAmb.SelectedIndexChanged += new System.EventHandler(this.lstAmb_SelectedIndexChanged);
            // 
            // AmbLights
            // 
            this.AmbLights.Controls.Add(this.chkAmbAlpha);
            this.AmbLights.Controls.Add(this.chkAmbClr);
            this.AmbLights.Location = new System.Drawing.Point(4, 25);
            this.AmbLights.Name = "AmbLights";
            this.AmbLights.Padding = new System.Windows.Forms.Padding(3);
            this.AmbLights.Size = new System.Drawing.Size(651, 97);
            this.AmbLights.TabIndex = 1;
            this.AmbLights.Text = "AmbLights";
            this.AmbLights.UseVisualStyleBackColor = true;
            // 
            // chkAmbAlpha
            // 
            this.chkAmbAlpha.AutoSize = true;
            this.chkAmbAlpha.Location = new System.Drawing.Point(6, 24);
            this.chkAmbAlpha.Name = "chkAmbAlpha";
            this.chkAmbAlpha.Size = new System.Drawing.Size(95, 17);
            this.chkAmbAlpha.TabIndex = 6;
            this.chkAmbAlpha.Text = "Alpha Enabled";
            this.chkAmbAlpha.UseVisualStyleBackColor = true;
            this.chkAmbAlpha.CheckedChanged += new System.EventHandler(this.chkAmbAlpha_CheckedChanged);
            // 
            // chkAmbClr
            // 
            this.chkAmbClr.AutoSize = true;
            this.chkAmbClr.Location = new System.Drawing.Point(6, 6);
            this.chkAmbClr.Name = "chkAmbClr";
            this.chkAmbClr.Size = new System.Drawing.Size(92, 17);
            this.chkAmbClr.TabIndex = 5;
            this.chkAmbClr.Text = "Color Enabled";
            this.chkAmbClr.UseVisualStyleBackColor = true;
            this.chkAmbClr.CheckedChanged += new System.EventHandler(this.chkAmbClr_CheckedChanged);
            // 
            // Lights
            // 
            this.Lights.Controls.Add(this.groupBox6);
            this.Lights.Controls.Add(this.groupBox5);
            this.Lights.Location = new System.Drawing.Point(4, 25);
            this.Lights.Name = "Lights";
            this.Lights.Size = new System.Drawing.Size(651, 97);
            this.Lights.TabIndex = 2;
            this.Lights.Text = "Lights";
            this.Lights.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lightCut);
            this.groupBox6.Controls.Add(this.lightPaste);
            this.groupBox6.Controls.Add(this.lightCopy);
            this.groupBox6.Controls.Add(this.lightClear);
            this.groupBox6.Controls.Add(this.label25);
            this.groupBox6.Controls.Add(this.label26);
            this.groupBox6.Controls.Add(this.numSpotBright);
            this.groupBox6.Controls.Add(this.label24);
            this.groupBox6.Controls.Add(this.label18);
            this.groupBox6.Controls.Add(this.numStartZ);
            this.groupBox6.Controls.Add(this.numRefBright);
            this.groupBox6.Controls.Add(this.numSpotCut);
            this.groupBox6.Controls.Add(this.label19);
            this.groupBox6.Controls.Add(this.numEndY);
            this.groupBox6.Controls.Add(this.label20);
            this.groupBox6.Controls.Add(this.numEndX);
            this.groupBox6.Controls.Add(this.label21);
            this.groupBox6.Controls.Add(this.numRefDist);
            this.groupBox6.Controls.Add(this.label22);
            this.groupBox6.Controls.Add(this.numStartX);
            this.groupBox6.Controls.Add(this.numStartY);
            this.groupBox6.Controls.Add(this.numEndZ);
            this.groupBox6.Controls.Add(this.label23);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(204, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(447, 97);
            this.groupBox6.TabIndex = 40;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Light Keyframes";
            // 
            // lightCut
            // 
            this.lightCut.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.lightCut.Location = new System.Drawing.Point(162, 73);
            this.lightCut.Name = "lightCut";
            this.lightCut.Size = new System.Drawing.Size(59, 20);
            this.lightCut.TabIndex = 39;
            this.lightCut.Text = "Cut";
            this.lightCut.UseVisualStyleBackColor = true;
            this.lightCut.Click += new System.EventHandler(this.lightCut_Click);
            // 
            // lightPaste
            // 
            this.lightPaste.Location = new System.Drawing.Point(104, 73);
            this.lightPaste.Name = "lightPaste";
            this.lightPaste.Size = new System.Drawing.Size(59, 20);
            this.lightPaste.TabIndex = 41;
            this.lightPaste.Text = "Paste";
            this.lightPaste.UseVisualStyleBackColor = true;
            this.lightPaste.Click += new System.EventHandler(this.lightPaste_Click);
            // 
            // lightCopy
            // 
            this.lightCopy.Location = new System.Drawing.Point(46, 73);
            this.lightCopy.Name = "lightCopy";
            this.lightCopy.Size = new System.Drawing.Size(59, 20);
            this.lightCopy.TabIndex = 40;
            this.lightCopy.Text = "Copy";
            this.lightCopy.UseVisualStyleBackColor = true;
            this.lightCopy.Click += new System.EventHandler(this.lightCopy_Click);
            // 
            // lightClear
            // 
            this.lightClear.Location = new System.Drawing.Point(220, 73);
            this.lightClear.Name = "lightClear";
            this.lightClear.Size = new System.Drawing.Size(59, 20);
            this.lightClear.TabIndex = 42;
            this.lightClear.Text = "Clear";
            this.lightClear.UseVisualStyleBackColor = true;
            this.lightClear.Click += new System.EventHandler(this.lightClear_Click);
            // 
            // label25
            // 
            this.label25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label25.Location = new System.Drawing.Point(3, 34);
            this.label25.Margin = new System.Windows.Forms.Padding(0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(70, 20);
            this.label25.TabIndex = 20;
            this.label25.Text = "Start Points";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label26
            // 
            this.label26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(279, 53);
            this.label26.Margin = new System.Windows.Forms.Padding(0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(70, 20);
            this.label26.TabIndex = 32;
            this.label26.Text = "Ref Dist";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numSpotBright
            // 
            this.numSpotBright.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numSpotBright.Integral = false;
            this.numSpotBright.Location = new System.Drawing.Point(348, 34);
            this.numSpotBright.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numSpotBright.MaximumValue = 3.402823E+38F;
            this.numSpotBright.MinimumValue = -3.402823E+38F;
            this.numSpotBright.Name = "numSpotBright";
            this.numSpotBright.Size = new System.Drawing.Size(70, 20);
            this.numSpotBright.TabIndex = 38;
            this.numSpotBright.Text = "0";
            this.numSpotBright.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numSpotBright.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // label24
            // 
            this.label24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(279, 72);
            this.label24.Margin = new System.Windows.Forms.Padding(0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(70, 20);
            this.label24.TabIndex = 33;
            this.label24.Text = "Ref Bright";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            this.label18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label18.Location = new System.Drawing.Point(210, 15);
            this.label18.Margin = new System.Windows.Forms.Padding(0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(70, 20);
            this.label18.TabIndex = 31;
            this.label18.Text = "Z";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numStartZ
            // 
            this.numStartZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numStartZ.Integral = false;
            this.numStartZ.Location = new System.Drawing.Point(210, 34);
            this.numStartZ.Margin = new System.Windows.Forms.Padding(0);
            this.numStartZ.MaximumValue = 3.402823E+38F;
            this.numStartZ.MinimumValue = -3.402823E+38F;
            this.numStartZ.Name = "numStartZ";
            this.numStartZ.Size = new System.Drawing.Size(70, 20);
            this.numStartZ.TabIndex = 26;
            this.numStartZ.Text = "0";
            this.numStartZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numStartZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numRefBright
            // 
            this.numRefBright.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRefBright.Integral = false;
            this.numRefBright.Location = new System.Drawing.Point(348, 72);
            this.numRefBright.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numRefBright.MaximumValue = 1F;
            this.numRefBright.MinimumValue = 0F;
            this.numRefBright.Name = "numRefBright";
            this.numRefBright.Size = new System.Drawing.Size(70, 20);
            this.numRefBright.TabIndex = 36;
            this.numRefBright.Text = "0";
            this.numRefBright.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numRefBright.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numSpotCut
            // 
            this.numSpotCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numSpotCut.Integral = false;
            this.numSpotCut.Location = new System.Drawing.Point(348, 15);
            this.numSpotCut.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numSpotCut.MaximumValue = 90F;
            this.numSpotCut.MinimumValue = 0F;
            this.numSpotCut.Name = "numSpotCut";
            this.numSpotCut.Size = new System.Drawing.Size(70, 20);
            this.numSpotCut.TabIndex = 34;
            this.numSpotCut.Text = "0";
            this.numSpotCut.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numSpotCut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // label19
            // 
            this.label19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label19.Location = new System.Drawing.Point(3, 53);
            this.label19.Margin = new System.Windows.Forms.Padding(0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(70, 20);
            this.label19.TabIndex = 22;
            this.label19.Text = "End Points";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numEndY
            // 
            this.numEndY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numEndY.Integral = false;
            this.numEndY.Location = new System.Drawing.Point(141, 53);
            this.numEndY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numEndY.MaximumValue = 3.402823E+38F;
            this.numEndY.MinimumValue = -3.402823E+38F;
            this.numEndY.Name = "numEndY";
            this.numEndY.Size = new System.Drawing.Size(70, 20);
            this.numEndY.TabIndex = 28;
            this.numEndY.Text = "0";
            this.numEndY.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numEndY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // label20
            // 
            this.label20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label20.Location = new System.Drawing.Point(279, 15);
            this.label20.Margin = new System.Windows.Forms.Padding(0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(70, 20);
            this.label20.TabIndex = 21;
            this.label20.Text = "Spot Cutoff";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numEndX
            // 
            this.numEndX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numEndX.Integral = false;
            this.numEndX.Location = new System.Drawing.Point(72, 53);
            this.numEndX.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numEndX.MaximumValue = 3.402823E+38F;
            this.numEndX.MinimumValue = -3.402823E+38F;
            this.numEndX.Name = "numEndX";
            this.numEndX.Size = new System.Drawing.Size(70, 20);
            this.numEndX.TabIndex = 27;
            this.numEndX.Text = "0";
            this.numEndX.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numEndX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // label21
            // 
            this.label21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label21.Location = new System.Drawing.Point(141, 15);
            this.label21.Margin = new System.Windows.Forms.Padding(0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(70, 20);
            this.label21.TabIndex = 29;
            this.label21.Text = "Y";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numRefDist
            // 
            this.numRefDist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRefDist.Integral = false;
            this.numRefDist.Location = new System.Drawing.Point(348, 53);
            this.numRefDist.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numRefDist.MaximumValue = 3.402823E+38F;
            this.numRefDist.MinimumValue = 0F;
            this.numRefDist.Name = "numRefDist";
            this.numRefDist.Size = new System.Drawing.Size(70, 20);
            this.numRefDist.TabIndex = 35;
            this.numRefDist.Text = "0";
            this.numRefDist.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numRefDist.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(72, 15);
            this.label22.Margin = new System.Windows.Forms.Padding(0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(70, 20);
            this.label22.TabIndex = 24;
            this.label22.Text = "X";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numStartX
            // 
            this.numStartX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numStartX.Integral = false;
            this.numStartX.Location = new System.Drawing.Point(72, 34);
            this.numStartX.Margin = new System.Windows.Forms.Padding(0);
            this.numStartX.MaximumValue = 3.402823E+38F;
            this.numStartX.MinimumValue = -3.402823E+38F;
            this.numStartX.Name = "numStartX";
            this.numStartX.Size = new System.Drawing.Size(70, 20);
            this.numStartX.TabIndex = 23;
            this.numStartX.Text = "0";
            this.numStartX.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numStartX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numStartY
            // 
            this.numStartY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numStartY.Integral = false;
            this.numStartY.Location = new System.Drawing.Point(141, 34);
            this.numStartY.Margin = new System.Windows.Forms.Padding(0);
            this.numStartY.MaximumValue = 3.402823E+38F;
            this.numStartY.MinimumValue = -3.402823E+38F;
            this.numStartY.Name = "numStartY";
            this.numStartY.Size = new System.Drawing.Size(70, 20);
            this.numStartY.TabIndex = 25;
            this.numStartY.Text = "0";
            this.numStartY.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numStartY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numEndZ
            // 
            this.numEndZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numEndZ.Integral = false;
            this.numEndZ.Location = new System.Drawing.Point(210, 53);
            this.numEndZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numEndZ.MaximumValue = 3.402823E+38F;
            this.numEndZ.MinimumValue = -3.402823E+38F;
            this.numEndZ.Name = "numEndZ";
            this.numEndZ.Size = new System.Drawing.Size(70, 20);
            this.numEndZ.TabIndex = 30;
            this.numEndZ.Text = "0";
            this.numEndZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numEndZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(279, 34);
            this.label23.Margin = new System.Windows.Forms.Padding(0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(70, 20);
            this.label23.TabIndex = 37;
            this.label23.Text = "Spec Bright";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cboSpotFunc);
            this.groupBox5.Controls.Add(this.cboDistFunc);
            this.groupBox5.Controls.Add(this.label29);
            this.groupBox5.Controls.Add(this.label28);
            this.groupBox5.Controls.Add(this.chkLightSpec);
            this.groupBox5.Controls.Add(this.chkLightAlpha);
            this.groupBox5.Controls.Add(this.chkLightClr);
            this.groupBox5.Controls.Add(this.label27);
            this.groupBox5.Controls.Add(this.cboLightType);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(204, 97);
            this.groupBox5.TabIndex = 39;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Light Settings";
            // 
            // cboSpotFunc
            // 
            this.cboSpotFunc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpotFunc.FormattingEnabled = true;
            this.cboSpotFunc.Location = new System.Drawing.Point(128, 72);
            this.cboSpotFunc.Name = "cboSpotFunc";
            this.cboSpotFunc.Size = new System.Drawing.Size(70, 21);
            this.cboSpotFunc.TabIndex = 9;
            this.cboSpotFunc.SelectedIndexChanged += new System.EventHandler(this.lstSpotFunc_SelectedIndexChanged);
            // 
            // cboDistFunc
            // 
            this.cboDistFunc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDistFunc.FormattingEnabled = true;
            this.cboDistFunc.Location = new System.Drawing.Point(128, 34);
            this.cboDistFunc.Name = "cboDistFunc";
            this.cboDistFunc.Size = new System.Drawing.Size(70, 21);
            this.cboDistFunc.TabIndex = 8;
            this.cboDistFunc.SelectedIndexChanged += new System.EventHandler(this.lstDistFunc_SelectedIndexChanged);
            // 
            // label29
            // 
            this.label29.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label29.Location = new System.Drawing.Point(128, 53);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(70, 20);
            this.label29.TabIndex = 7;
            this.label29.Text = "Spot Func:";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label28
            // 
            this.label28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label28.Location = new System.Drawing.Point(128, 15);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(70, 20);
            this.label28.TabIndex = 6;
            this.label28.Text = "Dist Func:";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkLightSpec
            // 
            this.chkLightSpec.AutoSize = true;
            this.chkLightSpec.Location = new System.Drawing.Point(9, 76);
            this.chkLightSpec.Name = "chkLightSpec";
            this.chkLightSpec.Size = new System.Drawing.Size(110, 17);
            this.chkLightSpec.TabIndex = 5;
            this.chkLightSpec.Text = "Specular Enabled";
            this.chkLightSpec.UseVisualStyleBackColor = true;
            this.chkLightSpec.CheckedChanged += new System.EventHandler(this.chkLightSpec_CheckedChanged);
            // 
            // chkLightAlpha
            // 
            this.chkLightAlpha.AutoSize = true;
            this.chkLightAlpha.Location = new System.Drawing.Point(9, 58);
            this.chkLightAlpha.Name = "chkLightAlpha";
            this.chkLightAlpha.Size = new System.Drawing.Size(95, 17);
            this.chkLightAlpha.TabIndex = 4;
            this.chkLightAlpha.Text = "Alpha Enabled";
            this.chkLightAlpha.UseVisualStyleBackColor = true;
            this.chkLightAlpha.CheckedChanged += new System.EventHandler(this.chkLightAlpha_CheckedChanged);
            // 
            // chkLightClr
            // 
            this.chkLightClr.AutoSize = true;
            this.chkLightClr.Location = new System.Drawing.Point(9, 40);
            this.chkLightClr.Name = "chkLightClr";
            this.chkLightClr.Size = new System.Drawing.Size(92, 17);
            this.chkLightClr.TabIndex = 3;
            this.chkLightClr.Text = "Color Enabled";
            this.chkLightClr.UseVisualStyleBackColor = true;
            this.chkLightClr.CheckedChanged += new System.EventHandler(this.chkLightClr_CheckedChanged);
            // 
            // label27
            // 
            this.label27.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label27.Location = new System.Drawing.Point(6, 15);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(46, 21);
            this.label27.TabIndex = 2;
            this.label27.Text = "Type:";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboLightType
            // 
            this.cboLightType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLightType.FormattingEnabled = true;
            this.cboLightType.Location = new System.Drawing.Point(51, 15);
            this.cboLightType.Name = "cboLightType";
            this.cboLightType.Size = new System.Drawing.Size(73, 21);
            this.cboLightType.TabIndex = 0;
            this.cboLightType.SelectedIndexChanged += new System.EventHandler(this.lstLightType_SelectedIndexChanged);
            // 
            // Fog
            // 
            this.Fog.Controls.Add(this.groupBox4);
            this.Fog.Controls.Add(this.groupBox3);
            this.Fog.Location = new System.Drawing.Point(4, 25);
            this.Fog.Name = "Fog";
            this.Fog.Size = new System.Drawing.Size(651, 97);
            this.Fog.TabIndex = 3;
            this.Fog.Text = "Fog";
            this.Fog.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button3);
            this.groupBox4.Controls.Add(this.button4);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.numFogEndZ);
            this.groupBox4.Controls.Add(this.numFogStartZ);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(185, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(466, 97);
            this.groupBox4.TabIndex = 40;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Edit Frame";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(311, 15);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(59, 20);
            this.button3.TabIndex = 35;
            this.button3.Text = "Paste";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(253, 15);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(59, 20);
            this.button4.TabIndex = 34;
            this.button4.Text = "Copy";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Location = new System.Drawing.Point(6, 15);
            this.label16.Margin = new System.Windows.Forms.Padding(0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(55, 20);
            this.label16.TabIndex = 7;
            this.label16.Text = "Start Z:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numFogEndZ
            // 
            this.numFogEndZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numFogEndZ.Integral = false;
            this.numFogEndZ.Location = new System.Drawing.Point(183, 15);
            this.numFogEndZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numFogEndZ.MaximumValue = 3.402823E+38F;
            this.numFogEndZ.MinimumValue = -3.402823E+38F;
            this.numFogEndZ.Name = "numFogEndZ";
            this.numFogEndZ.Size = new System.Drawing.Size(70, 20);
            this.numFogEndZ.TabIndex = 10;
            this.numFogEndZ.Text = "0";
            this.numFogEndZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numFogEndZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numFogStartZ
            // 
            this.numFogStartZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numFogStartZ.Integral = false;
            this.numFogStartZ.Location = new System.Drawing.Point(60, 15);
            this.numFogStartZ.Margin = new System.Windows.Forms.Padding(0);
            this.numFogStartZ.MaximumValue = 3.402823E+38F;
            this.numFogStartZ.MinimumValue = -3.402823E+38F;
            this.numFogStartZ.Name = "numFogStartZ";
            this.numFogStartZ.Size = new System.Drawing.Size(70, 20);
            this.numFogStartZ.TabIndex = 9;
            this.numFogStartZ.Text = "0";
            this.numFogStartZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numFogStartZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // label17
            // 
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label17.Location = new System.Drawing.Point(129, 15);
            this.label17.Margin = new System.Windows.Forms.Padding(0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(55, 20);
            this.label17.TabIndex = 8;
            this.label17.Text = "End Z:";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.cboFogType);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(185, 97);
            this.groupBox3.TabIndex = 39;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Fog Settings";
            // 
            // label15
            // 
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(6, 15);
            this.label15.Margin = new System.Windows.Forms.Padding(0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(42, 21);
            this.label15.TabIndex = 38;
            this.label15.Text = "Type:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboFogType
            // 
            this.cboFogType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFogType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFogType.FormattingEnabled = true;
            this.cboFogType.Location = new System.Drawing.Point(47, 15);
            this.cboFogType.Name = "cboFogType";
            this.cboFogType.Size = new System.Drawing.Size(132, 21);
            this.cboFogType.TabIndex = 37;
            this.cboFogType.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // Cameras
            // 
            this.Cameras.Controls.Add(this.panel2);
            this.Cameras.Location = new System.Drawing.Point(4, 25);
            this.Cameras.Name = "Cameras";
            this.Cameras.Size = new System.Drawing.Size(651, 97);
            this.Cameras.TabIndex = 4;
            this.Cameras.Text = "Cameras";
            this.Cameras.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(651, 97);
            this.panel2.TabIndex = 27;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.numPosZ);
            this.groupBox2.Controls.Add(this.btnCut);
            this.groupBox2.Controls.Add(this.btnPaste);
            this.groupBox2.Controls.Add(this.btnCopy);
            this.groupBox2.Controls.Add(this.btnClear);
            this.groupBox2.Controls.Add(this.numAimX);
            this.groupBox2.Controls.Add(this.numFarZ);
            this.groupBox2.Controls.Add(this.numRotY);
            this.groupBox2.Controls.Add(this.numNearZ);
            this.groupBox2.Controls.Add(this.numRotX);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.numTwist);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.numPosX);
            this.groupBox2.Controls.Add(this.numAspect);
            this.groupBox2.Controls.Add(this.numRotZ);
            this.groupBox2.Controls.Add(this.numHeight);
            this.groupBox2.Controls.Add(this.numPosY);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.numAimY);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.numAimZ);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.numFovY);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(149, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(502, 97);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Edit Frame";
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(266, 49);
            this.label11.Margin = new System.Windows.Forms.Padding(0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 20);
            this.label11.TabIndex = 22;
            this.label11.Text = "Height:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(266, 11);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 20);
            this.label8.TabIndex = 9;
            this.label8.Text = "Twist:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(5, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Position:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(266, 30);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 20);
            this.label9.TabIndex = 10;
            this.label9.Text = "Fov Y:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPosZ
            // 
            this.numPosZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosZ.Integral = false;
            this.numPosZ.Location = new System.Drawing.Point(197, 30);
            this.numPosZ.Margin = new System.Windows.Forms.Padding(0);
            this.numPosZ.MaximumValue = 3.402823E+38F;
            this.numPosZ.MinimumValue = -3.402823E+38F;
            this.numPosZ.Name = "numPosZ";
            this.numPosZ.Size = new System.Drawing.Size(70, 20);
            this.numPosZ.TabIndex = 5;
            this.numPosZ.Text = "0";
            this.numPosZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numPosZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // btnCut
            // 
            this.btnCut.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnCut.Location = new System.Drawing.Point(382, 50);
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(59, 20);
            this.btnCut.TabIndex = 30;
            this.btnCut.Text = "Cut";
            this.btnCut.UseVisualStyleBackColor = true;
            this.btnCut.Click += new System.EventHandler(this.btnCut_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(440, 50);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(59, 20);
            this.btnPaste.TabIndex = 32;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(382, 69);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(59, 20);
            this.btnCopy.TabIndex = 31;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(440, 69);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(59, 20);
            this.btnClear.TabIndex = 33;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // numAimX
            // 
            this.numAimX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numAimX.Integral = false;
            this.numAimX.Location = new System.Drawing.Point(59, 68);
            this.numAimX.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numAimX.MaximumValue = 3.402823E+38F;
            this.numAimX.MinimumValue = -3.402823E+38F;
            this.numAimX.Name = "numAimX";
            this.numAimX.Size = new System.Drawing.Size(70, 20);
            this.numAimX.TabIndex = 11;
            this.numAimX.Text = "0";
            this.numAimX.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numAimX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numFarZ
            // 
            this.numFarZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numFarZ.Integral = false;
            this.numFarZ.Location = new System.Drawing.Point(428, 30);
            this.numFarZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numFarZ.MaximumValue = 3.402823E+38F;
            this.numFarZ.MinimumValue = -3.402823E+38F;
            this.numFarZ.Name = "numFarZ";
            this.numFarZ.Size = new System.Drawing.Size(70, 20);
            this.numFarZ.TabIndex = 29;
            this.numFarZ.Text = "0";
            this.numFarZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numFarZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numRotY
            // 
            this.numRotY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotY.Integral = false;
            this.numRotY.Location = new System.Drawing.Point(128, 49);
            this.numRotY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numRotY.MaximumValue = 3.402823E+38F;
            this.numRotY.MinimumValue = -3.402823E+38F;
            this.numRotY.Name = "numRotY";
            this.numRotY.Size = new System.Drawing.Size(70, 20);
            this.numRotY.TabIndex = 7;
            this.numRotY.Text = "0";
            this.numRotY.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numRotY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numNearZ
            // 
            this.numNearZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numNearZ.Integral = false;
            this.numNearZ.Location = new System.Drawing.Point(428, 11);
            this.numNearZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numNearZ.MaximumValue = 3.402823E+38F;
            this.numNearZ.MinimumValue = -3.402823E+38F;
            this.numNearZ.Name = "numNearZ";
            this.numNearZ.Size = new System.Drawing.Size(70, 20);
            this.numNearZ.TabIndex = 28;
            this.numNearZ.Text = "0";
            this.numNearZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numNearZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numRotX
            // 
            this.numRotX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotX.Integral = false;
            this.numRotX.Location = new System.Drawing.Point(59, 49);
            this.numRotX.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numRotX.MaximumValue = 3.402823E+38F;
            this.numRotX.MinimumValue = -3.402823E+38F;
            this.numRotX.Name = "numRotX";
            this.numRotX.Size = new System.Drawing.Size(70, 20);
            this.numRotX.TabIndex = 6;
            this.numRotX.Text = "0";
            this.numRotX.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numRotX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(381, 30);
            this.label12.Margin = new System.Windows.Forms.Padding(0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(48, 20);
            this.label12.TabIndex = 27;
            this.label12.Text = "Far Z:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTwist
            // 
            this.numTwist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTwist.Integral = false;
            this.numTwist.Location = new System.Drawing.Point(312, 11);
            this.numTwist.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numTwist.MaximumValue = 3.402823E+38F;
            this.numTwist.MinimumValue = -3.402823E+38F;
            this.numTwist.Name = "numTwist";
            this.numTwist.Size = new System.Drawing.Size(70, 20);
            this.numTwist.TabIndex = 12;
            this.numTwist.Text = "0";
            this.numTwist.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numTwist.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(381, 11);
            this.label13.Margin = new System.Windows.Forms.Padding(0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(48, 20);
            this.label13.TabIndex = 26;
            this.label13.Text = "Near Z:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPosX
            // 
            this.numPosX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosX.Integral = false;
            this.numPosX.Location = new System.Drawing.Point(59, 30);
            this.numPosX.Margin = new System.Windows.Forms.Padding(0);
            this.numPosX.MaximumValue = 3.402823E+38F;
            this.numPosX.MinimumValue = -3.402823E+38F;
            this.numPosX.Name = "numPosX";
            this.numPosX.Size = new System.Drawing.Size(70, 20);
            this.numPosX.TabIndex = 3;
            this.numPosX.Text = "0";
            this.numPosX.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numPosX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numAspect
            // 
            this.numAspect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numAspect.Integral = false;
            this.numAspect.Location = new System.Drawing.Point(312, 68);
            this.numAspect.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numAspect.MaximumValue = 3.402823E+38F;
            this.numAspect.MinimumValue = -3.402823E+38F;
            this.numAspect.Name = "numAspect";
            this.numAspect.Size = new System.Drawing.Size(70, 20);
            this.numAspect.TabIndex = 25;
            this.numAspect.Text = "0";
            this.numAspect.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numAspect.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numRotZ
            // 
            this.numRotZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotZ.Integral = false;
            this.numRotZ.Location = new System.Drawing.Point(197, 49);
            this.numRotZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numRotZ.MaximumValue = 3.402823E+38F;
            this.numRotZ.MinimumValue = -3.402823E+38F;
            this.numRotZ.Name = "numRotZ";
            this.numRotZ.Size = new System.Drawing.Size(70, 20);
            this.numRotZ.TabIndex = 8;
            this.numRotZ.Text = "0";
            this.numRotZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numRotZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numHeight
            // 
            this.numHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numHeight.Integral = false;
            this.numHeight.Location = new System.Drawing.Point(312, 49);
            this.numHeight.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numHeight.MaximumValue = 3.402823E+38F;
            this.numHeight.MinimumValue = -3.402823E+38F;
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(70, 20);
            this.numHeight.TabIndex = 24;
            this.numHeight.Text = "0";
            this.numHeight.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numHeight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // numPosY
            // 
            this.numPosY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosY.Integral = false;
            this.numPosY.Location = new System.Drawing.Point(128, 30);
            this.numPosY.Margin = new System.Windows.Forms.Padding(0);
            this.numPosY.MaximumValue = 3.402823E+38F;
            this.numPosY.MinimumValue = -3.402823E+38F;
            this.numPosY.Name = "numPosY";
            this.numPosY.Size = new System.Drawing.Size(70, 20);
            this.numPosY.TabIndex = 4;
            this.numPosY.Text = "0";
            this.numPosY.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numPosY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(266, 68);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 20);
            this.label10.TabIndex = 23;
            this.label10.Text = "Aspect:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(59, 11);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "X";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(128, 11);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 20);
            this.label5.TabIndex = 7;
            this.label5.Text = "Y";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numAimY
            // 
            this.numAimY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numAimY.Integral = false;
            this.numAimY.Location = new System.Drawing.Point(128, 68);
            this.numAimY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numAimY.MaximumValue = 3.402823E+38F;
            this.numAimY.MinimumValue = -3.402823E+38F;
            this.numAimY.Name = "numAimY";
            this.numAimY.Size = new System.Drawing.Size(70, 20);
            this.numAimY.TabIndex = 21;
            this.numAimY.Text = "0";
            this.numAimY.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numAimY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(5, 68);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Aim:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numAimZ
            // 
            this.numAimZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numAimZ.Integral = false;
            this.numAimZ.Location = new System.Drawing.Point(197, 68);
            this.numAimZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numAimZ.MaximumValue = 3.402823E+38F;
            this.numAimZ.MinimumValue = -3.402823E+38F;
            this.numAimZ.Name = "numAimZ";
            this.numAimZ.Size = new System.Drawing.Size(70, 20);
            this.numAimZ.TabIndex = 19;
            this.numAimZ.Text = "0";
            this.numAimZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numAimZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(5, 49);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Rotation:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(197, 11);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 20);
            this.label6.TabIndex = 8;
            this.label6.Text = "Z";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numFovY
            // 
            this.numFovY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numFovY.Integral = false;
            this.numFovY.Location = new System.Drawing.Point(312, 30);
            this.numFovY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numFovY.MaximumValue = 3.402823E+38F;
            this.numFovY.MinimumValue = -3.402823E+38F;
            this.numFovY.Name = "numFovY";
            this.numFovY.Size = new System.Drawing.Size(70, 20);
            this.numFovY.TabIndex = 13;
            this.numFovY.Text = "0";
            this.numFovY.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numFovY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnUseCamera);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(149, 97);
            this.panel1.TabIndex = 40;
            // 
            // btnUseCamera
            // 
            this.btnUseCamera.Location = new System.Drawing.Point(3, 62);
            this.btnUseCamera.Name = "btnUseCamera";
            this.btnUseCamera.Size = new System.Drawing.Size(138, 23);
            this.btnUseCamera.TabIndex = 38;
            this.btnUseCamera.Text = "Use Current Camera";
            this.btnUseCamera.UseVisualStyleBackColor = true;
            this.btnUseCamera.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboCamProj);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.cboCamType);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(147, 60);
            this.groupBox1.TabIndex = 38;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Camera Settings";
            // 
            // cboCamProj
            // 
            this.cboCamProj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCamProj.FormattingEnabled = true;
            this.cboCamProj.Location = new System.Drawing.Point(72, 35);
            this.cboCamProj.Name = "cboCamProj";
            this.cboCamProj.Size = new System.Drawing.Size(70, 21);
            this.cboCamProj.TabIndex = 35;
            this.cboCamProj.SelectedIndexChanged += new System.EventHandler(this.lstCamProj_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(72, 16);
            this.label14.Margin = new System.Windows.Forms.Padding(0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(70, 20);
            this.label14.TabIndex = 37;
            this.label14.Text = "Projection";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboCamType
            // 
            this.cboCamType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCamType.FormattingEnabled = true;
            this.cboCamType.Location = new System.Drawing.Point(3, 35);
            this.cboCamType.Name = "cboCamType";
            this.cboCamType.Size = new System.Drawing.Size(70, 21);
            this.cboCamType.TabIndex = 34;
            this.cboCamType.SelectedIndexChanged += new System.EventHandler(this.lstCamType_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 16);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 20);
            this.label7.TabIndex = 36;
            this.label7.Text = "Type";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nodeType
            // 
            this.nodeType.AutoSize = true;
            this.nodeType.BackColor = System.Drawing.SystemColors.Control;
            this.nodeType.Location = new System.Drawing.Point(310, 3);
            this.nodeType.Name = "nodeType";
            this.nodeType.Size = new System.Drawing.Size(49, 13);
            this.nodeType.TabIndex = 0;
            this.nodeType.Text = "LightSet:";
            this.nodeType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboNodeList
            // 
            this.cboNodeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNodeList.FormattingEnabled = true;
            this.cboNodeList.Location = new System.Drawing.Point(359, 1);
            this.cboNodeList.Name = "cboNodeList";
            this.cboNodeList.Size = new System.Drawing.Size(72, 21);
            this.cboNodeList.TabIndex = 0;
            this.cboNodeList.SelectedIndexChanged += new System.EventHandler(this.nodeList_SelectedIndexChanged);
            // 
            // btnRename
            // 
            this.btnRename.Location = new System.Drawing.Point(432, 0);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(64, 23);
            this.btnRename.TabIndex = 20;
            this.btnRename.Text = "Rename";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(496, 0);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(45, 23);
            this.btnNew.TabIndex = 21;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(541, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(59, 23);
            this.btnDelete.TabIndex = 22;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // SCN0Editor
            // 
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.nodeType);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.cboNodeList);
            this.Controls.Add(this.tabControl1);
            this.Name = "SCN0Editor";
            this.Size = new System.Drawing.Size(659, 126);
            this.tabControl1.ResumeLayout(false);
            this.LightSets.ResumeLayout(false);
            this.AmbLights.ResumeLayout(false);
            this.AmbLights.PerformLayout();
            this.Lights.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.Fog.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.Cameras.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TabControl tabControl1;
        private TabPage LightSets;
        private TabPage AmbLights;
        private TabPage Lights;
        private TabPage Fog;
        private TabPage Cameras;
        private Label nodeType;
        private ComboBox cboNodeList;
        private Panel panel2;
        private NumericInputBox numFarZ;
        private NumericInputBox numNearZ;
        private Label label12;
        private Label label13;
        private NumericInputBox numAspect;
        private NumericInputBox numHeight;
        private Label label10;
        private Label label11;
        private NumericInputBox numAimY;
        private NumericInputBox numAimZ;
        private NumericInputBox numFovY;
        private Label label3;
        private Label label2;
        private NumericInputBox numPosY;
        private NumericInputBox numRotZ;
        private NumericInputBox numPosX;
        private NumericInputBox numTwist;
        private NumericInputBox numRotX;
        private NumericInputBox numRotY;
        private NumericInputBox numAimX;
        private NumericInputBox numPosZ;
        private Label label9;
        private Label label1;
        private Label label8;
        private Label label6;
        private Label label5;
        private Label label4;
        private Button btnPaste;
        private Button btnCopy;
        private Button btnCut;
        private Button btnClear;
        private ComboBox cboCamProj;
        private ComboBox cboCamType;
        private Label label14;
        private Label label7;
        private Label label15;
        private ComboBox cboFogType;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private GroupBox groupBox4;
        private GroupBox groupBox3;
        private Label label16;
        private NumericInputBox numFogEndZ;
        private NumericInputBox numFogStartZ;
        private Label label17;
        private NumericInputBox numSpotBright;
        private Label label18;
        private NumericInputBox numRefBright;
        private Label label19;
        private Label label20;
        private Label label21;
        private Label label22;
        private NumericInputBox numStartY;
        private Label label23;
        private NumericInputBox numEndZ;
        private NumericInputBox numStartX;
        private NumericInputBox numRefDist;
        private NumericInputBox numEndX;
        private NumericInputBox numEndY;
        private NumericInputBox numSpotCut;
        private NumericInputBox numStartZ;
        private Label label24;
        private Label label25;
        private Label label26;
        private GroupBox groupBox6;
        private GroupBox groupBox5;
        public SCN0LightSetNode _lightSet;
        public SCN0FogNode _fog;
        public SCN0AmbientLightNode _ambLight;
        public SCN0LightNode _light;
        public SCN0CameraNode _camera;
        private ComboBox cboLightType;
        private Label label27;
        private CheckBox chkLightClr;
        private CheckBox chkLightAlpha;
        private CheckBox chkLightSpec;
        private ComboBox cboSpotFunc;
        private ComboBox cboDistFunc;
        private Label label29;
        private Label label28;
        private ComboBox cboLight7;
        private ComboBox cboLight6;
        private ComboBox cboLight5;
        private ComboBox cboLight4;
        private ComboBox cboLight3;
        private ComboBox cboLight2;
        private ComboBox cboLight1;
        private ComboBox cboLight0;
        private Label label38;
        private Label label37;
        private Label label34;
        private Label label35;
        private Label label36;
        private Label label32;
        private Label label31;
        private Label label33;
        private Label label30;
        private ComboBox cboAmb;
        private Button btnRename;
        private CheckBox chkAmbAlpha;
        private CheckBox chkAmbClr;
        private Button btnUseCamera;

        public ModelEditorBase _mainWindow;
        private Panel panel1;
        private Button button3;
        private Button button4;
        private Button lightCut;
        private Button lightPaste;
        private Button lightCopy;
        private Button btnNew;
        private Button btnDelete;
        private Button lightClear;

        public SCN0Editor()
        {
            InitializeComponent();

            _transBoxes[0] = new NumericInputBox[10];
            _transBoxes[1] = new NumericInputBox[2];
            _transBoxes[2] = new NumericInputBox[15];

            _transBoxes[0][0] = numStartX; numStartX.Tag = 0;
            _transBoxes[0][1] = numStartY; numStartY.Tag = 1;
            _transBoxes[0][2] = numStartZ; numStartZ.Tag = 2;
            _transBoxes[0][3] = numEndX; numEndX.Tag = 3;
            _transBoxes[0][4] = numEndY; numEndY.Tag = 4;
            _transBoxes[0][5] = numEndZ; numEndZ.Tag = 5;
            _transBoxes[0][6] = numRefDist; numRefDist.Tag = 6;
            _transBoxes[0][7] = numRefBright; numRefBright.Tag = 7;
            _transBoxes[0][8] = numSpotCut; numSpotCut.Tag = 8;
            _transBoxes[0][9] = numSpotBright; numSpotBright.Tag = 9;

            _transBoxes[1][0] = numFogStartZ; numFogStartZ.Tag = 0;
            _transBoxes[1][1] = numFogEndZ; numFogEndZ.Tag = 1;

            _transBoxes[2][0] = numPosX; numPosX.Tag = 0;
            _transBoxes[2][1] = numPosY; numPosY.Tag = 1;
            _transBoxes[2][2] = numPosZ; numPosZ.Tag = 2;
            _transBoxes[2][3] = numAspect; numAspect.Tag = 3;
            _transBoxes[2][4] = numNearZ; numNearZ.Tag = 4;
            _transBoxes[2][5] = numFarZ; numFarZ.Tag = 5;
            _transBoxes[2][6] = numRotX; numRotX.Tag = 6;
            _transBoxes[2][7] = numRotY; numRotY.Tag = 7;
            _transBoxes[2][8] = numRotZ; numRotZ.Tag = 8;
            _transBoxes[2][9] = numAimX; numAimX.Tag = 9;
            _transBoxes[2][10] = numAimY; numAimY.Tag = 10;
            _transBoxes[2][11] = numAimZ; numAimZ.Tag = 11;
            _transBoxes[2][12] = numTwist; numTwist.Tag = 12;
            _transBoxes[2][13] = numFovY; numFovY.Tag = 13;
            _transBoxes[2][14] = numHeight; numHeight.Tag = 14;

            cboFogType.DataSource = Enum.GetValues(typeof(FogType));
            cboLightType.DataSource = Enum.GetValues(typeof(LightType));
            cboCamProj.DataSource = Enum.GetValues(typeof(ProjectionType));
            cboCamType.DataSource = Enum.GetValues(typeof(SCN0CameraType));
            cboDistFunc.DataSource = Enum.GetValues(typeof(DistAttnFn));
            cboSpotFunc.DataSource = Enum.GetValues(typeof(SpotFn));
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SCN0Node SelectedAnimation
        {
            get { return _mainWindow.SelectedSCN0; }
            set { _mainWindow.SelectedSCN0 = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0MaterialRefNode TargetTexRef { get { return _mainWindow.TargetTexRef; } set { _mainWindow.TargetTexRef = value; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get { return _mainWindow.CurrentFrame; }
            set { _mainWindow.CurrentFrame = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get { return (MDL0Node)_mainWindow.TargetModel; }
            set { _mainWindow.TargetModel = value; }
        }

        public NumericInputBox[][] _transBoxes = new NumericInputBox[3][];

        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public bool EnableTransformEdit
        //{
        //    get { return _mainWindow._enableTransform; }
        //    set { grpTransform.Enabled = grpTransAll.Enabled = (_mainWindow.EnableTransformEdit = value) && (TargetBone != null); }
        //}

        private IKeyframeSource Entry
        {
            get
            {
                switch (_tabIndex)
                {
                    case 2: return _light;
                    case 3: return _fog;
                    case 4: return _camera;
                }
                return null;
            }
        }

        public void UpdatePropDisplay()
        {
            if (!Enabled)
                return;

            for (int i = 0; i < (_tabIndex == 2 ? 10 : _tabIndex == 3 ? 2 : _tabIndex == 4 ? 15 : 0); i++)
                ResetBox(i);

            if (_mainWindow.InterpolationEditor != null &&
                _mainWindow.InterpolationEditor.Visible &&
                Entry != null && SelectedAnimation != null &&
                CurrentFrame > 0 &&
                _mainWindow.InterpolationEditor._targetNode != Entry)
                    _mainWindow.InterpolationEditor.SetTarget(Entry);
        }

        public unsafe void ResetBox(int index)
        {
            NumericInputBox box = _transBoxes[_tabIndex - 2][index];

            if (SelectedAnimation != null && CurrentFrame >= 1 && Entry != null)
            {
                KeyframeArray array = Entry.KeyArrays[index];
                KeyframeEntry kf = array.GetKeyframe(CurrentFrame - 1);
                if (kf == null)
                {
                    box.Value = array[CurrentFrame - 1];
                    box.BackColor = Color.White;
                }
                else
                {
                    box.Value = kf._value;
                    box.BackColor = Color.Yellow;
                }
            }
        }
        internal unsafe void BoxChanged(object sender, EventArgs e)
        {
            NumericInputBox box = sender as NumericInputBox;
            int index = (int)box.Tag;

            if (SelectedAnimation != null && CurrentFrame >= 1 && Entry != null)
            {
                KeyframeEntry key = float.IsNaN(box.Value) ?
                    Entry.KeyArrays[index].Remove(CurrentFrame - 1) :
                    Entry.KeyArrays[index].SetFrameValue(CurrentFrame - 1, box.Value);

                if (key != null)
                {
                    key._prev.GenerateTangent();
                    key._next.GenerateTangent();
                    ((ResourceNode)Entry).SignalPropertyChange();
                }
            }

            ResetBox(index);
            _mainWindow.KeyframePanel.UpdateKeyframe(CurrentFrame - 1);

            if (_mainWindow.ModelPanel.FirstPersonCamera && 
                _mainWindow._SCN0Camera != null && 
                _tabIndex == 4)
                _mainWindow._SCN0Camera.SetCamera(_mainWindow.ModelPanel.CurrentViewport, CurrentFrame - 1, true);

            _mainWindow.UpdateModel();
        }

        bool _updating = false;

        public void UpdateSelectedLightSets()
        {
            _updating = true;
            cboAmb.SelectedIndex = _lightSet._ambient != null ? _lightSet._ambient.Index + 1 : 0;
            cboLight0.SelectedIndex = _lightSet._lights[0] != null ? _lightSet._lights[0].Index + 1 : 0;
            cboLight1.SelectedIndex = _lightSet._lights[1] != null ? _lightSet._lights[1].Index + 1 : 0;
            cboLight2.SelectedIndex = _lightSet._lights[2] != null ? _lightSet._lights[2].Index + 1 : 0;
            cboLight3.SelectedIndex = _lightSet._lights[3] != null ? _lightSet._lights[3].Index + 1 : 0;
            cboLight4.SelectedIndex = _lightSet._lights[4] != null ? _lightSet._lights[4].Index + 1 : 0;
            cboLight5.SelectedIndex = _lightSet._lights[5] != null ? _lightSet._lights[5].Index + 1 : 0;
            cboLight6.SelectedIndex = _lightSet._lights[6] != null ? _lightSet._lights[6].Index + 1 : 0;
            cboLight7.SelectedIndex = _lightSet._lights[7] != null ? _lightSet._lights[7].Index + 1 : 0;
            _updating = false;

            _mainWindow.UpdateModel();
        }

        private void nodeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboNodeList.SelectedItem == null)
            {
                btnRename.Enabled = false;
                return;
            }
            else if (btnRename.Enabled == false)
                btnRename.Enabled = true;

            _mainWindow.KeyframePanel.TargetSequence = cboNodeList.SelectedItem as ResourceNode;

            switch (_tabIndex)
            {
                case 0:

                    _mainWindow._SCN0LightSet = _lightSet = cboNodeList.SelectedItem as SCN0LightSetNode;

                    SCN0GroupNode amb = SelectedAnimation.GetFolder<SCN0AmbientLightNode>();

                    cboAmb.Items.Clear();
                    cboAmb.Items.Add("<null>");
                    if (amb != null)
                        cboAmb.Items.AddRange(amb.Children.ToArray());

                    SCN0GroupNode lights = SelectedAnimation.GetFolder<SCN0LightNode>();

                    cboLight0.Items.Clear();
                    cboLight0.Items.Add("<null>");
                    if (lights != null)
                        foreach (SCN0LightNode s in lights.Children)
                            cboLight0.Items.Add(s);
                    
                    cboLight1.Items.Clear();
                    cboLight1.Items.Add("<null>");
                    if (lights != null)
                        foreach (SCN0LightNode s in lights.Children)
                            cboLight1.Items.Add(s);
                    
                    cboLight2.Items.Clear();
                    cboLight2.Items.Add("<null>");
                    if (lights != null)
                        foreach (SCN0LightNode s in lights.Children)
                            cboLight2.Items.Add(s);
                    
                    cboLight3.Items.Clear();
                    cboLight3.Items.Add("<null>");
                    if (lights != null)
                        foreach (SCN0LightNode s in lights.Children)
                            cboLight3.Items.Add(s);
                    
                    cboLight4.Items.Clear();
                    cboLight4.Items.Add("<null>");
                    if (lights != null)
                        foreach (SCN0LightNode s in lights.Children)
                            cboLight4.Items.Add(s);
                    
                    cboLight5.Items.Clear();
                    cboLight5.Items.Add("<null>");
                    if (lights != null)
                        foreach (SCN0LightNode s in lights.Children)
                            cboLight5.Items.Add(s);

                    cboLight6.Items.Clear();
                    cboLight6.Items.Add("<null>");
                    if (lights != null)
                        foreach (SCN0LightNode s in lights.Children)
                            cboLight6.Items.Add(s);

                    cboLight7.Items.Clear();
                    cboLight7.Items.Add("<null>");
                    if (lights != null)
                        foreach (SCN0LightNode s in lights.Children)
                            cboLight7.Items.Add(s);

                    UpdateSelectedLightSets();

                    break;
                case 1:
                    _ambLight = cboNodeList.SelectedItem as SCN0AmbientLightNode;
                    chkAmbClr.Checked = _ambLight.ColorEnabled;
                    chkAmbAlpha.Checked = _ambLight.AlphaEnabled;
                    break;
                case 2:
                    _light = cboNodeList.SelectedItem as SCN0LightNode;
                    chkLightClr.Checked = _light.ColorEnabled;
                    chkLightAlpha.Checked = _light.AlphaEnabled;
                    chkLightSpec.Checked = _light.SpecularEnabled;
                    cboLightType.SelectedIndex = (int)_light.LightType;
                    cboDistFunc.SelectedIndex = (int)_light.DistanceFunction;
                    cboSpotFunc.SelectedIndex = (int)_light.SpotFunction;
                    break;
                case 3:
                    _mainWindow._SCN0Fog = _fog = cboNodeList.SelectedItem as SCN0FogNode;
                    cboFogType.SelectedIndex = Array.IndexOf(Enum.GetValues(typeof(FogType)), _fog.Type);
                    break;
                case 4:
                    _mainWindow._SCN0Camera = _camera = cboNodeList.SelectedItem as SCN0CameraNode;
                    cboCamType.SelectedIndex = (int)_camera.Type;
                    cboCamProj.SelectedIndex = (int)_camera.ProjectionType;
                    break;
            }
            _mainWindow.KeyframePanel.TargetSequence = cboNodeList.SelectedItem as ResourceNode;
        }

        public Drawing.Size GetDimensions()
        {
            //TODO: automate this with minimum size
            switch (_tabIndex)
            {
                case 0: return new Drawing.Size(626, 70);
                case 1: return new Drawing.Size(566, 72);
                case 2: return new Drawing.Size(634, 128);
                case 3: return new Drawing.Size(566, 70);
                case 4: return new Drawing.Size(660, 120);
            }
            return new Drawing.Size(0, 0);
        }

        void UpdateNodeList()
        {
            cboNodeList.Items.Clear();
            SCN0GroupNode g;
            if (SelectedAnimation != null && (g = SelectedAnimation.GetFolder((SCN0GroupNode.GroupType)_tabIndex)) != null)
            {
                Enabled = true;
                foreach (SCN0EntryNode s in g.Children)
                    cboNodeList.Items.Add(s);
            }
            else
                Enabled = false;
            if (cboNodeList.Items.Count > 0)
                cboNodeList.SelectedIndex = 0;
            else
                btnRename.Enabled = false;
        }

        public int _tabIndex = 0;
        public void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            _tabIndex = e.TabPageIndex;
            
            _mainWindow.KeyframePanel.listKeyframes.Items.Clear();
            string s = SCN0GroupNode._names[_tabIndex];
            nodeType.Text = s.Substring(0, s.Length - 7);

            UpdateNodeList();

            _mainWindow.UpdateAnimationPanelDimensions();
            //UpdatePropDisplay();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cboNodeList.SelectedItem != null)
            {
                using (RenameDialog d = new RenameDialog())
                    d.ShowDialog(this, cboNodeList.SelectedItem as ResourceNode);
                int i = cboNodeList.SelectedIndex;
                UpdateNodeList();
                cboNodeList.SelectedIndex = i;
            }
        }

        private void lstAmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            _lightSet.Ambience = cboAmb.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void lstLight0_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            _lightSet.Light0 = cboLight0.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void lstLight1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            _lightSet.Light1 = cboLight1.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void lstLight2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            _lightSet.Light2 = cboLight2.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void lstLight3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            _lightSet.Light3 = cboLight3.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void lstLight4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            _lightSet.Light4 = cboLight4.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void lstLight5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            _lightSet.Light5 = cboLight5.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void lstLight6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            _lightSet.Light6 = cboLight6.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void lstLight7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            _lightSet.Light7 = cboLight7.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ModelPanel panel = _mainWindow.ModelPanel;

            //Get the position of the current camera
            Vector3 camPos = panel.Camera.GetPoint();

            numPosX.Value = camPos._x;
            numPosY.Value = camPos._y;
            numPosZ.Value = camPos._z;

            BoxChanged(numPosX, null);
            BoxChanged(numPosY, null);
            BoxChanged(numPosZ, null);

            Vector3 rot = panel.Camera._rotation;
            if (_camera.Type == SCN0CameraType.Rotate)
            {
                //Easy

                numRotX.Value = rot._x;
                numRotY.Value = rot._y;
                numRotZ.Value = rot._z;

                BoxChanged(numRotX, null);
                BoxChanged(numRotY, null);
                BoxChanged(numRotZ, null);
            }
            else
            {
                //TODO: Thoroughly test this.
                //Should work, but what if the point is calculated behind the camera?
                if (SelectedAnimation != null && CurrentFrame >= 1 && Entry != null)
                {
                    KeyframeArray arr = Entry.KeyArrays[(int)numAimX.Tag];
                    float x = arr.GetFrameValue(CurrentFrame);

                    arr = Entry.KeyArrays[(int)numAimY.Tag];
                    float y = arr.GetFrameValue(CurrentFrame);

                    arr = Entry.KeyArrays[(int)numAimZ.Tag];
                    float z = arr.GetFrameValue(CurrentFrame);

                    Vector3 interpRefPoint = new Vector3(x, y, z);
                    Vector2 screenMidPt = new Vector2(panel.CurrentViewport.Region.Width / 2, panel.CurrentViewport.Region.Height / 2);

                    Vector3 ray1 = panel.CurrentViewport.Camera.UnProject(screenMidPt._x, screenMidPt._y, 0.0f);
                    Vector3 ray2 = panel.CurrentViewport.Camera.UnProject(screenMidPt._x, screenMidPt._y, 1.0f);

                    Vector3 u = ray2 - ray1;
                    Vector3 pq = interpRefPoint - ray1;
                    Vector3 w2 = pq - u * (Vector3.Dot(pq, u) / u.Dot());

                    Vector3 point = interpRefPoint - w2;

                    numAimX.Value = point._x;
                    numAimY.Value = point._y;
                    numAimZ.Value = point._z;

                    BoxChanged(numAimX, null);
                    BoxChanged(numAimY, null);
                    BoxChanged(numAimZ, null);
                }
            }

            _mainWindow.ModelPanel.Invalidate();
        }

        private void lstCamProj_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProjectionType p = (ProjectionType)cboCamProj.SelectedIndex;

            if (_camera != null)
            {
                _camera.ProjectionType = p;
                _mainWindow.UpdateModel();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_fog != null)
            {
                _fog.Type = (FogType)cboFogType.SelectedItem;
                _mainWindow.UpdateModel();
            }
        }

        private void lstCamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_camera != null)
            {
                _camera.Type = (SCN0CameraType)cboCamType.SelectedIndex;
                bool aim = _camera.Type == SCN0CameraType.Aim;

                numAimX.ForeColor = 
                numAimY.ForeColor = 
                numAimZ.ForeColor = 
                numTwist.ForeColor = 
                aim ? Color.Black : Color.LightGray;

                numRotX.ForeColor = 
                numRotY.ForeColor = 
                numRotZ.ForeColor =
                aim ? Color.LightGray : Color.Black;
            }
        }

        private void lstLightType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LightType t = (LightType)cboLightType.SelectedIndex;
            switch (t)
            {
                case LightType.Directional:
                    numSpotCut.ForeColor =
                    numRefDist.ForeColor =
                    numRefBright.ForeColor =
                    cboDistFunc.ForeColor =
                    cboSpotFunc.ForeColor = Color.LightGray;
                    break;
                case LightType.Point:
                    numSpotCut.ForeColor = Color.LightGray;
                    numRefDist.ForeColor = Color.Black;
                    numRefBright.ForeColor = Color.Black;
                    cboDistFunc.ForeColor = Color.Black;
                    cboSpotFunc.ForeColor = Color.LightGray;
                    break;
                case LightType.Spotlight:
                    numSpotCut.ForeColor =
                    numRefDist.ForeColor =
                    numRefBright.ForeColor =
                    cboDistFunc.ForeColor =
                    cboSpotFunc.ForeColor = Color.Black;
                    break;
            }

            if (_light != null)
            {
                _light.LightType = t;
                _mainWindow.UpdateModel();
            }
        }

        private void lstDistFunc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_light != null)
            {
                _light.DistanceFunction = (DistAttnFn)cboDistFunc.SelectedIndex;
                _mainWindow.UpdateModel();
            }
        }

        private void lstSpotFunc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_light != null)
            {
                _light.SpotFunction = (SpotFn)cboSpotFunc.SelectedIndex;
                _mainWindow.UpdateModel();
            }
        }

        private void chkAmbClr_CheckedChanged(object sender, EventArgs e)
        {
            if (_ambLight != null)
            {
                _ambLight.ColorEnabled = chkAmbClr.Checked;
                _mainWindow.UpdateModel();
            }
        }

        private void chkAmbAlpha_CheckedChanged(object sender, EventArgs e)
        {
            if (_ambLight != null)
            {
                _ambLight.AlphaEnabled = chkAmbAlpha.Checked;
                _mainWindow.UpdateModel();
            }
        }

        private void chkLightClr_CheckedChanged(object sender, EventArgs e)
        {
            if (_light != null)
            {
                _light.ColorEnabled = chkLightClr.Checked;
                _mainWindow.UpdateModel();
            }
        }

        private void chkLightAlpha_CheckedChanged(object sender, EventArgs e)
        {
            if (_light != null)
            {
                _light.AlphaEnabled = chkLightAlpha.Checked;
                _mainWindow.UpdateModel();
            }
        }

        private void chkLightSpec_CheckedChanged(object sender, EventArgs e)
        {
            numSpotBright.ForeColor = chkLightSpec.Checked ? Color.Black : Color.LightGray;

            if (_light != null)
            {
                _light.SpecularEnabled = chkLightSpec.Checked;
                _mainWindow.UpdateModel();
            }
        }
        CameraAnimationFrame _tempCameraFrame;
        private unsafe void btnCut_Click(object sender, EventArgs e)
        {
            CameraAnimationFrame frame = new CameraAnimationFrame();
            float* p = (float*)&frame;

            for (int i = 0; i < 15; i++)
            {
                p[i] = _transBoxes[2][i].Value;
                _transBoxes[2][i].Value = float.NaN;
                BoxChanged(_transBoxes[2][i], null);
            }

            _tempCameraFrame = frame;
        }

        private unsafe void btnCopy_Click(object sender, EventArgs e)
        {
            CameraAnimationFrame frame = new CameraAnimationFrame();
            float* p = (float*)&frame;

            for (int i = 0; i < 15; i++)
                p[i] = _transBoxes[2][i].Value;

            _tempCameraFrame = frame;
        }

        private unsafe void btnPaste_Click(object sender, EventArgs e)
        {
            CameraAnimationFrame frame = _tempCameraFrame;
            float* p = (float*)&frame;

            for (int i = 0; i < 15; i++)
            {
                if (_transBoxes[2][i].Value != p[i])
                    _transBoxes[2][i].Value = p[i];
                BoxChanged(_transBoxes[2][i], null);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 15; i++)
            {
                _transBoxes[2][i].Value = float.NaN;
                BoxChanged(_transBoxes[2][i], null);
            }
        }
        FogAnimationFrame _tempFogFrame;
        private unsafe void button4_Click(object sender, EventArgs e)
        {
            FogAnimationFrame frame = new FogAnimationFrame();
            float* p = (float*)&frame;

            for (int i = 0; i < 2; i++)
                p[i] = _transBoxes[1][i].Value;

            _tempFogFrame = frame;
        }

        private unsafe void button3_Click(object sender, EventArgs e)
        {
            FogAnimationFrame frame = _tempFogFrame;
            float* p = (float*)&frame;

            for (int i = 0; i < 2; i++)
            {
                if (_transBoxes[1][i].Value != p[i])
                    _transBoxes[1][i].Value = p[i];
                BoxChanged(_transBoxes[1][i], null);
            }
        }
        LightAnimationFrame _tempLightFrame;
        private unsafe void lightCut_Click(object sender, EventArgs e)
        {
            LightAnimationFrame frame = new LightAnimationFrame();
            float* p = (float*)&frame;

            for (int i = 0; i < 10; i++)
            {
                p[i] = _transBoxes[0][i].Value;
                _transBoxes[0][i].Value = float.NaN;
                BoxChanged(_transBoxes[0][i], null);
            }

            _tempLightFrame = frame;
        }

        private unsafe void lightPaste_Click(object sender, EventArgs e)
        {
            LightAnimationFrame frame = _tempLightFrame;
            float* p = (float*)&frame;

            for (int i = 0; i < 10; i++)
            {
                if (_transBoxes[0][i].Value != p[i])
                    _transBoxes[0][i].Value = p[i];
                BoxChanged(_transBoxes[0][i], null);
            }
        }

        private unsafe void lightCopy_Click(object sender, EventArgs e)
        {
            LightAnimationFrame frame = new LightAnimationFrame();
            float* p = (float*)&frame;

            for (int i = 0; i < 10; i++)
                p[i] = _transBoxes[0][i].Value;

            _tempLightFrame = frame;
        }

        private void lightClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                _transBoxes[0][i].Value = float.NaN;
                BoxChanged(_transBoxes[0][i], null);
            }
        }

        public void UpdateInterpolationEditor(NumericInputBox box)
        {
            if (_mainWindow.InterpolationEditor == null || !_mainWindow.InterpolationEditor.Visible)
                return;

            _mainWindow.InterpolationEditor.interpolationViewer._updating = true;
            if (box.BackColor == Color.Yellow)
            {
                KeyframeEntry kfe = Entry.KeyArrays[(int)box.Tag].GetKeyframe(CurrentFrame - 1);
                if (kfe != null)
                    _mainWindow.InterpolationEditor.SelectedKeyframe = kfe;
            }
            else
                _mainWindow.InterpolationEditor.SelectedKeyframe = null;
            _mainWindow.InterpolationEditor.interpolationViewer._updating = false;
        }

        private void box_MouseDown(object sender, MouseEventArgs e)
        {
            NumericInputBox box = sender as NumericInputBox;

            int type = (int)box.Tag;
            if (_mainWindow.InterpolationEditor != null && _mainWindow.InterpolationEditor.Visible)
            {
                if (_mainWindow.InterpolationEditor.SelectedMode != type)
                    _mainWindow.InterpolationEditor.SelectedMode = type;
                UpdateInterpolationEditor(box);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation != null)
            {
                SCN0EntryNode x = SelectedAnimation.CreateResource((SCN0GroupNode.GroupType)_tabIndex, null);
                UpdateNodeList();
                cboNodeList.SelectedIndex = x.Index;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SCN0EntryNode x = cboNodeList.SelectedItem as SCN0EntryNode;
            x.Remove();
            UpdateNodeList();
        }
    }
}
