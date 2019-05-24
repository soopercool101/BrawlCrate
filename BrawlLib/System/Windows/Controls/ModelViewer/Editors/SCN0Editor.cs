using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Animations;
using BrawlLib.Wii.Graphics;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class SCN0Editor : UserControl
    {
        #region Designer
        private void InitializeComponent()
        {
            tabControl1 = new System.Windows.Forms.TabControl();
            LightSets = new System.Windows.Forms.TabPage();
            cboLight7 = new System.Windows.Forms.ComboBox();
            cboLight6 = new System.Windows.Forms.ComboBox();
            cboLight5 = new System.Windows.Forms.ComboBox();
            cboLight4 = new System.Windows.Forms.ComboBox();
            cboLight3 = new System.Windows.Forms.ComboBox();
            cboLight2 = new System.Windows.Forms.ComboBox();
            cboLight1 = new System.Windows.Forms.ComboBox();
            cboLight0 = new System.Windows.Forms.ComboBox();
            label38 = new System.Windows.Forms.Label();
            label37 = new System.Windows.Forms.Label();
            label34 = new System.Windows.Forms.Label();
            label35 = new System.Windows.Forms.Label();
            label36 = new System.Windows.Forms.Label();
            label32 = new System.Windows.Forms.Label();
            label31 = new System.Windows.Forms.Label();
            label33 = new System.Windows.Forms.Label();
            label30 = new System.Windows.Forms.Label();
            cboAmb = new System.Windows.Forms.ComboBox();
            AmbLights = new System.Windows.Forms.TabPage();
            chkAmbAlpha = new System.Windows.Forms.CheckBox();
            chkAmbClr = new System.Windows.Forms.CheckBox();
            Lights = new System.Windows.Forms.TabPage();
            groupBox6 = new System.Windows.Forms.GroupBox();
            lightCut = new System.Windows.Forms.Button();
            lightPaste = new System.Windows.Forms.Button();
            lightCopy = new System.Windows.Forms.Button();
            lightClear = new System.Windows.Forms.Button();
            label25 = new System.Windows.Forms.Label();
            label26 = new System.Windows.Forms.Label();
            numSpotBright = new System.Windows.Forms.NumericInputBox();
            label24 = new System.Windows.Forms.Label();
            label18 = new System.Windows.Forms.Label();
            numStartZ = new System.Windows.Forms.NumericInputBox();
            numRefBright = new System.Windows.Forms.NumericInputBox();
            numSpotCut = new System.Windows.Forms.NumericInputBox();
            label19 = new System.Windows.Forms.Label();
            numEndY = new System.Windows.Forms.NumericInputBox();
            label20 = new System.Windows.Forms.Label();
            numEndX = new System.Windows.Forms.NumericInputBox();
            label21 = new System.Windows.Forms.Label();
            numRefDist = new System.Windows.Forms.NumericInputBox();
            label22 = new System.Windows.Forms.Label();
            numStartX = new System.Windows.Forms.NumericInputBox();
            numStartY = new System.Windows.Forms.NumericInputBox();
            numEndZ = new System.Windows.Forms.NumericInputBox();
            label23 = new System.Windows.Forms.Label();
            groupBox5 = new System.Windows.Forms.GroupBox();
            cboSpotFunc = new System.Windows.Forms.ComboBox();
            cboDistFunc = new System.Windows.Forms.ComboBox();
            label29 = new System.Windows.Forms.Label();
            label28 = new System.Windows.Forms.Label();
            chkLightSpec = new System.Windows.Forms.CheckBox();
            chkLightAlpha = new System.Windows.Forms.CheckBox();
            chkLightClr = new System.Windows.Forms.CheckBox();
            label27 = new System.Windows.Forms.Label();
            cboLightType = new System.Windows.Forms.ComboBox();
            Fog = new System.Windows.Forms.TabPage();
            groupBox4 = new System.Windows.Forms.GroupBox();
            button3 = new System.Windows.Forms.Button();
            button4 = new System.Windows.Forms.Button();
            label16 = new System.Windows.Forms.Label();
            numFogEndZ = new System.Windows.Forms.NumericInputBox();
            numFogStartZ = new System.Windows.Forms.NumericInputBox();
            label17 = new System.Windows.Forms.Label();
            groupBox3 = new System.Windows.Forms.GroupBox();
            label15 = new System.Windows.Forms.Label();
            cboFogType = new System.Windows.Forms.ComboBox();
            Cameras = new System.Windows.Forms.TabPage();
            panel2 = new System.Windows.Forms.Panel();
            groupBox2 = new System.Windows.Forms.GroupBox();
            label11 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            numPosZ = new System.Windows.Forms.NumericInputBox();
            btnCut = new System.Windows.Forms.Button();
            btnPaste = new System.Windows.Forms.Button();
            btnCopy = new System.Windows.Forms.Button();
            btnClear = new System.Windows.Forms.Button();
            numAimX = new System.Windows.Forms.NumericInputBox();
            numFarZ = new System.Windows.Forms.NumericInputBox();
            numRotY = new System.Windows.Forms.NumericInputBox();
            numNearZ = new System.Windows.Forms.NumericInputBox();
            numRotX = new System.Windows.Forms.NumericInputBox();
            label12 = new System.Windows.Forms.Label();
            numTwist = new System.Windows.Forms.NumericInputBox();
            label13 = new System.Windows.Forms.Label();
            numPosX = new System.Windows.Forms.NumericInputBox();
            numAspect = new System.Windows.Forms.NumericInputBox();
            numRotZ = new System.Windows.Forms.NumericInputBox();
            numHeight = new System.Windows.Forms.NumericInputBox();
            numPosY = new System.Windows.Forms.NumericInputBox();
            label10 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            numAimY = new System.Windows.Forms.NumericInputBox();
            label2 = new System.Windows.Forms.Label();
            numAimZ = new System.Windows.Forms.NumericInputBox();
            label3 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            numFovY = new System.Windows.Forms.NumericInputBox();
            panel1 = new System.Windows.Forms.Panel();
            btnUseCamera = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            cboCamProj = new System.Windows.Forms.ComboBox();
            label14 = new System.Windows.Forms.Label();
            cboCamType = new System.Windows.Forms.ComboBox();
            label7 = new System.Windows.Forms.Label();
            nodeType = new System.Windows.Forms.Label();
            cboNodeList = new System.Windows.Forms.ComboBox();
            btnRename = new System.Windows.Forms.Button();
            btnNew = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            tabControl1.SuspendLayout();
            LightSets.SuspendLayout();
            AmbLights.SuspendLayout();
            Lights.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox5.SuspendLayout();
            Fog.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            Cameras.SuspendLayout();
            panel2.SuspendLayout();
            groupBox2.SuspendLayout();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            tabControl1.Controls.Add(LightSets);
            tabControl1.Controls.Add(AmbLights);
            tabControl1.Controls.Add(Lights);
            tabControl1.Controls.Add(Fog);
            tabControl1.Controls.Add(Cameras);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(659, 126);
            tabControl1.TabIndex = 0;
            tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(tabControl1_Selected);
            // 
            // LightSets
            // 
            LightSets.Controls.Add(cboLight7);
            LightSets.Controls.Add(cboLight6);
            LightSets.Controls.Add(cboLight5);
            LightSets.Controls.Add(cboLight4);
            LightSets.Controls.Add(cboLight3);
            LightSets.Controls.Add(cboLight2);
            LightSets.Controls.Add(cboLight1);
            LightSets.Controls.Add(cboLight0);
            LightSets.Controls.Add(label38);
            LightSets.Controls.Add(label37);
            LightSets.Controls.Add(label34);
            LightSets.Controls.Add(label35);
            LightSets.Controls.Add(label36);
            LightSets.Controls.Add(label32);
            LightSets.Controls.Add(label31);
            LightSets.Controls.Add(label33);
            LightSets.Controls.Add(label30);
            LightSets.Controls.Add(cboAmb);
            LightSets.Location = new System.Drawing.Point(4, 25);
            LightSets.Name = "LightSets";
            LightSets.Padding = new System.Windows.Forms.Padding(3);
            LightSets.Size = new System.Drawing.Size(651, 97);
            LightSets.TabIndex = 0;
            LightSets.Text = "LightSets";
            LightSets.UseVisualStyleBackColor = true;
            // 
            // cboLight7
            // 
            cboLight7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLight7.FormattingEnabled = true;
            cboLight7.Location = new System.Drawing.Point(536, 20);
            cboLight7.Name = "cboLight7";
            cboLight7.Size = new System.Drawing.Size(82, 21);
            cboLight7.TabIndex = 19;
            cboLight7.SelectedIndexChanged += new System.EventHandler(lstLight7_SelectedIndexChanged);
            // 
            // cboLight6
            // 
            cboLight6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLight6.FormattingEnabled = true;
            cboLight6.Location = new System.Drawing.Point(536, 0);
            cboLight6.Name = "cboLight6";
            cboLight6.Size = new System.Drawing.Size(82, 21);
            cboLight6.TabIndex = 18;
            cboLight6.SelectedIndexChanged += new System.EventHandler(lstLight6_SelectedIndexChanged);
            // 
            // cboLight5
            // 
            cboLight5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLight5.FormattingEnabled = true;
            cboLight5.Location = new System.Drawing.Point(402, 20);
            cboLight5.Name = "cboLight5";
            cboLight5.Size = new System.Drawing.Size(82, 21);
            cboLight5.TabIndex = 17;
            cboLight5.SelectedIndexChanged += new System.EventHandler(lstLight5_SelectedIndexChanged);
            // 
            // cboLight4
            // 
            cboLight4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLight4.FormattingEnabled = true;
            cboLight4.Location = new System.Drawing.Point(402, 0);
            cboLight4.Name = "cboLight4";
            cboLight4.Size = new System.Drawing.Size(82, 21);
            cboLight4.TabIndex = 16;
            cboLight4.SelectedIndexChanged += new System.EventHandler(lstLight4_SelectedIndexChanged);
            // 
            // cboLight3
            // 
            cboLight3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLight3.FormattingEnabled = true;
            cboLight3.Location = new System.Drawing.Point(268, 20);
            cboLight3.Name = "cboLight3";
            cboLight3.Size = new System.Drawing.Size(82, 21);
            cboLight3.TabIndex = 15;
            cboLight3.SelectedIndexChanged += new System.EventHandler(lstLight3_SelectedIndexChanged);
            // 
            // cboLight2
            // 
            cboLight2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLight2.FormattingEnabled = true;
            cboLight2.Location = new System.Drawing.Point(268, 0);
            cboLight2.Name = "cboLight2";
            cboLight2.Size = new System.Drawing.Size(82, 21);
            cboLight2.TabIndex = 14;
            cboLight2.SelectedIndexChanged += new System.EventHandler(lstLight2_SelectedIndexChanged);
            // 
            // cboLight1
            // 
            cboLight1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLight1.FormattingEnabled = true;
            cboLight1.Location = new System.Drawing.Point(134, 20);
            cboLight1.Name = "cboLight1";
            cboLight1.Size = new System.Drawing.Size(82, 21);
            cboLight1.TabIndex = 13;
            cboLight1.SelectedIndexChanged += new System.EventHandler(lstLight1_SelectedIndexChanged);
            // 
            // cboLight0
            // 
            cboLight0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLight0.FormattingEnabled = true;
            cboLight0.Location = new System.Drawing.Point(134, 0);
            cboLight0.Name = "cboLight0";
            cboLight0.Size = new System.Drawing.Size(82, 21);
            cboLight0.TabIndex = 12;
            cboLight0.SelectedIndexChanged += new System.EventHandler(lstLight0_SelectedIndexChanged);
            // 
            // label38
            // 
            label38.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label38.Location = new System.Drawing.Point(483, 20);
            label38.Name = "label38";
            label38.Size = new System.Drawing.Size(54, 21);
            label38.TabIndex = 10;
            label38.Text = "Light 7:";
            label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label37
            // 
            label37.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label37.Location = new System.Drawing.Point(483, 0);
            label37.Name = "label37";
            label37.Size = new System.Drawing.Size(54, 21);
            label37.TabIndex = 9;
            label37.Text = "Light 6:";
            label37.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label34
            // 
            label34.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label34.Location = new System.Drawing.Point(349, 20);
            label34.Name = "label34";
            label34.Size = new System.Drawing.Size(54, 21);
            label34.TabIndex = 8;
            label34.Text = "Light 5:";
            label34.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label35
            // 
            label35.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label35.Location = new System.Drawing.Point(349, 0);
            label35.Name = "label35";
            label35.Size = new System.Drawing.Size(54, 21);
            label35.TabIndex = 7;
            label35.Text = "Light 4:";
            label35.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label36
            // 
            label36.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label36.Location = new System.Drawing.Point(215, 20);
            label36.Name = "label36";
            label36.Size = new System.Drawing.Size(54, 21);
            label36.TabIndex = 6;
            label36.Text = "Light 3:";
            label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label32
            // 
            label32.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label32.Location = new System.Drawing.Point(215, 0);
            label32.Name = "label32";
            label32.Size = new System.Drawing.Size(54, 21);
            label32.TabIndex = 5;
            label32.Text = "Light 2:";
            label32.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label31
            // 
            label31.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label31.Location = new System.Drawing.Point(81, 20);
            label31.Name = "label31";
            label31.Size = new System.Drawing.Size(54, 21);
            label31.TabIndex = 4;
            label31.Text = "Light 1:";
            label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label33
            // 
            label33.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label33.Location = new System.Drawing.Point(81, 0);
            label33.Name = "label33";
            label33.Size = new System.Drawing.Size(54, 21);
            label33.TabIndex = 3;
            label33.Text = "Light 0:";
            label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label30
            // 
            label30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label30.Location = new System.Drawing.Point(0, 0);
            label30.Name = "label30";
            label30.Size = new System.Drawing.Size(82, 21);
            label30.TabIndex = 0;
            label30.Text = "Ambient: ";
            label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboAmb
            // 
            cboAmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboAmb.FormattingEnabled = true;
            cboAmb.Location = new System.Drawing.Point(0, 20);
            cboAmb.Name = "cboAmb";
            cboAmb.Size = new System.Drawing.Size(82, 21);
            cboAmb.TabIndex = 11;
            cboAmb.SelectedIndexChanged += new System.EventHandler(lstAmb_SelectedIndexChanged);
            // 
            // AmbLights
            // 
            AmbLights.Controls.Add(chkAmbAlpha);
            AmbLights.Controls.Add(chkAmbClr);
            AmbLights.Location = new System.Drawing.Point(4, 25);
            AmbLights.Name = "AmbLights";
            AmbLights.Padding = new System.Windows.Forms.Padding(3);
            AmbLights.Size = new System.Drawing.Size(651, 97);
            AmbLights.TabIndex = 1;
            AmbLights.Text = "AmbLights";
            AmbLights.UseVisualStyleBackColor = true;
            // 
            // chkAmbAlpha
            // 
            chkAmbAlpha.AutoSize = true;
            chkAmbAlpha.Location = new System.Drawing.Point(6, 24);
            chkAmbAlpha.Name = "chkAmbAlpha";
            chkAmbAlpha.Size = new System.Drawing.Size(95, 17);
            chkAmbAlpha.TabIndex = 6;
            chkAmbAlpha.Text = "Alpha Enabled";
            chkAmbAlpha.UseVisualStyleBackColor = true;
            chkAmbAlpha.CheckedChanged += new System.EventHandler(chkAmbAlpha_CheckedChanged);
            // 
            // chkAmbClr
            // 
            chkAmbClr.AutoSize = true;
            chkAmbClr.Location = new System.Drawing.Point(6, 6);
            chkAmbClr.Name = "chkAmbClr";
            chkAmbClr.Size = new System.Drawing.Size(92, 17);
            chkAmbClr.TabIndex = 5;
            chkAmbClr.Text = "Color Enabled";
            chkAmbClr.UseVisualStyleBackColor = true;
            chkAmbClr.CheckedChanged += new System.EventHandler(chkAmbClr_CheckedChanged);
            // 
            // Lights
            // 
            Lights.Controls.Add(groupBox6);
            Lights.Controls.Add(groupBox5);
            Lights.Location = new System.Drawing.Point(4, 25);
            Lights.Name = "Lights";
            Lights.Size = new System.Drawing.Size(651, 97);
            Lights.TabIndex = 2;
            Lights.Text = "Lights";
            Lights.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(lightCut);
            groupBox6.Controls.Add(lightPaste);
            groupBox6.Controls.Add(lightCopy);
            groupBox6.Controls.Add(lightClear);
            groupBox6.Controls.Add(label25);
            groupBox6.Controls.Add(label26);
            groupBox6.Controls.Add(numSpotBright);
            groupBox6.Controls.Add(label24);
            groupBox6.Controls.Add(label18);
            groupBox6.Controls.Add(numStartZ);
            groupBox6.Controls.Add(numRefBright);
            groupBox6.Controls.Add(numSpotCut);
            groupBox6.Controls.Add(label19);
            groupBox6.Controls.Add(numEndY);
            groupBox6.Controls.Add(label20);
            groupBox6.Controls.Add(numEndX);
            groupBox6.Controls.Add(label21);
            groupBox6.Controls.Add(numRefDist);
            groupBox6.Controls.Add(label22);
            groupBox6.Controls.Add(numStartX);
            groupBox6.Controls.Add(numStartY);
            groupBox6.Controls.Add(numEndZ);
            groupBox6.Controls.Add(label23);
            groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox6.Location = new System.Drawing.Point(204, 0);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new System.Drawing.Size(447, 97);
            groupBox6.TabIndex = 40;
            groupBox6.TabStop = false;
            groupBox6.Text = "Light Keyframes";
            // 
            // lightCut
            // 
            lightCut.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            lightCut.Location = new System.Drawing.Point(162, 73);
            lightCut.Name = "lightCut";
            lightCut.Size = new System.Drawing.Size(59, 20);
            lightCut.TabIndex = 39;
            lightCut.Text = "Cut";
            lightCut.UseVisualStyleBackColor = true;
            lightCut.Click += new System.EventHandler(lightCut_Click);
            // 
            // lightPaste
            // 
            lightPaste.Location = new System.Drawing.Point(104, 73);
            lightPaste.Name = "lightPaste";
            lightPaste.Size = new System.Drawing.Size(59, 20);
            lightPaste.TabIndex = 41;
            lightPaste.Text = "Paste";
            lightPaste.UseVisualStyleBackColor = true;
            lightPaste.Click += new System.EventHandler(lightPaste_Click);
            // 
            // lightCopy
            // 
            lightCopy.Location = new System.Drawing.Point(46, 73);
            lightCopy.Name = "lightCopy";
            lightCopy.Size = new System.Drawing.Size(59, 20);
            lightCopy.TabIndex = 40;
            lightCopy.Text = "Copy";
            lightCopy.UseVisualStyleBackColor = true;
            lightCopy.Click += new System.EventHandler(lightCopy_Click);
            // 
            // lightClear
            // 
            lightClear.Location = new System.Drawing.Point(220, 73);
            lightClear.Name = "lightClear";
            lightClear.Size = new System.Drawing.Size(59, 20);
            lightClear.TabIndex = 42;
            lightClear.Text = "Clear";
            lightClear.UseVisualStyleBackColor = true;
            lightClear.Click += new System.EventHandler(lightClear_Click);
            // 
            // label25
            // 
            label25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label25.Location = new System.Drawing.Point(3, 34);
            label25.Margin = new System.Windows.Forms.Padding(0);
            label25.Name = "label25";
            label25.Size = new System.Drawing.Size(70, 20);
            label25.TabIndex = 20;
            label25.Text = "Start Points";
            label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label26
            // 
            label26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label26.Location = new System.Drawing.Point(279, 53);
            label26.Margin = new System.Windows.Forms.Padding(0);
            label26.Name = "label26";
            label26.Size = new System.Drawing.Size(70, 20);
            label26.TabIndex = 32;
            label26.Text = "Ref Dist";
            label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numSpotBright
            // 
            numSpotBright.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numSpotBright.Integral = false;
            numSpotBright.Location = new System.Drawing.Point(348, 34);
            numSpotBright.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numSpotBright.MaximumValue = 3.402823E+38F;
            numSpotBright.MinimumValue = -3.402823E+38F;
            numSpotBright.Name = "numSpotBright";
            numSpotBright.Size = new System.Drawing.Size(70, 20);
            numSpotBright.TabIndex = 38;
            numSpotBright.Text = "0";
            numSpotBright.ValueChanged += new System.EventHandler(BoxChanged);
            numSpotBright.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // label24
            // 
            label24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label24.Location = new System.Drawing.Point(279, 72);
            label24.Margin = new System.Windows.Forms.Padding(0);
            label24.Name = "label24";
            label24.Size = new System.Drawing.Size(70, 20);
            label24.TabIndex = 33;
            label24.Text = "Ref Bright";
            label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            label18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label18.Location = new System.Drawing.Point(210, 15);
            label18.Margin = new System.Windows.Forms.Padding(0);
            label18.Name = "label18";
            label18.Size = new System.Drawing.Size(70, 20);
            label18.TabIndex = 31;
            label18.Text = "Z";
            label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numStartZ
            // 
            numStartZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numStartZ.Integral = false;
            numStartZ.Location = new System.Drawing.Point(210, 34);
            numStartZ.Margin = new System.Windows.Forms.Padding(0);
            numStartZ.MaximumValue = 3.402823E+38F;
            numStartZ.MinimumValue = -3.402823E+38F;
            numStartZ.Name = "numStartZ";
            numStartZ.Size = new System.Drawing.Size(70, 20);
            numStartZ.TabIndex = 26;
            numStartZ.Text = "0";
            numStartZ.ValueChanged += new System.EventHandler(BoxChanged);
            numStartZ.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // numRefBright
            // 
            numRefBright.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numRefBright.Integral = false;
            numRefBright.Location = new System.Drawing.Point(348, 72);
            numRefBright.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numRefBright.MaximumValue = 1F;
            numRefBright.MinimumValue = 0F;
            numRefBright.Name = "numRefBright";
            numRefBright.Size = new System.Drawing.Size(70, 20);
            numRefBright.TabIndex = 36;
            numRefBright.Text = "0";
            numRefBright.ValueChanged += new System.EventHandler(BoxChanged);
            numRefBright.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // numSpotCut
            // 
            numSpotCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numSpotCut.Integral = false;
            numSpotCut.Location = new System.Drawing.Point(348, 15);
            numSpotCut.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numSpotCut.MaximumValue = 90F;
            numSpotCut.MinimumValue = 0F;
            numSpotCut.Name = "numSpotCut";
            numSpotCut.Size = new System.Drawing.Size(70, 20);
            numSpotCut.TabIndex = 34;
            numSpotCut.Text = "0";
            numSpotCut.ValueChanged += new System.EventHandler(BoxChanged);
            numSpotCut.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // label19
            // 
            label19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label19.Location = new System.Drawing.Point(3, 53);
            label19.Margin = new System.Windows.Forms.Padding(0);
            label19.Name = "label19";
            label19.Size = new System.Drawing.Size(70, 20);
            label19.TabIndex = 22;
            label19.Text = "End Points";
            label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numEndY
            // 
            numEndY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numEndY.Integral = false;
            numEndY.Location = new System.Drawing.Point(141, 53);
            numEndY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numEndY.MaximumValue = 3.402823E+38F;
            numEndY.MinimumValue = -3.402823E+38F;
            numEndY.Name = "numEndY";
            numEndY.Size = new System.Drawing.Size(70, 20);
            numEndY.TabIndex = 28;
            numEndY.Text = "0";
            numEndY.ValueChanged += new System.EventHandler(BoxChanged);
            numEndY.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // label20
            // 
            label20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label20.Location = new System.Drawing.Point(279, 15);
            label20.Margin = new System.Windows.Forms.Padding(0);
            label20.Name = "label20";
            label20.Size = new System.Drawing.Size(70, 20);
            label20.TabIndex = 21;
            label20.Text = "Spot Cutoff";
            label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numEndX
            // 
            numEndX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numEndX.Integral = false;
            numEndX.Location = new System.Drawing.Point(72, 53);
            numEndX.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numEndX.MaximumValue = 3.402823E+38F;
            numEndX.MinimumValue = -3.402823E+38F;
            numEndX.Name = "numEndX";
            numEndX.Size = new System.Drawing.Size(70, 20);
            numEndX.TabIndex = 27;
            numEndX.Text = "0";
            numEndX.ValueChanged += new System.EventHandler(BoxChanged);
            numEndX.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // label21
            // 
            label21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label21.Location = new System.Drawing.Point(141, 15);
            label21.Margin = new System.Windows.Forms.Padding(0);
            label21.Name = "label21";
            label21.Size = new System.Drawing.Size(70, 20);
            label21.TabIndex = 29;
            label21.Text = "Y";
            label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numRefDist
            // 
            numRefDist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numRefDist.Integral = false;
            numRefDist.Location = new System.Drawing.Point(348, 53);
            numRefDist.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numRefDist.MaximumValue = 3.402823E+38F;
            numRefDist.MinimumValue = 0F;
            numRefDist.Name = "numRefDist";
            numRefDist.Size = new System.Drawing.Size(70, 20);
            numRefDist.TabIndex = 35;
            numRefDist.Text = "0";
            numRefDist.ValueChanged += new System.EventHandler(BoxChanged);
            numRefDist.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // label22
            // 
            label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label22.Location = new System.Drawing.Point(72, 15);
            label22.Margin = new System.Windows.Forms.Padding(0);
            label22.Name = "label22";
            label22.Size = new System.Drawing.Size(70, 20);
            label22.TabIndex = 24;
            label22.Text = "X";
            label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numStartX
            // 
            numStartX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numStartX.Integral = false;
            numStartX.Location = new System.Drawing.Point(72, 34);
            numStartX.Margin = new System.Windows.Forms.Padding(0);
            numStartX.MaximumValue = 3.402823E+38F;
            numStartX.MinimumValue = -3.402823E+38F;
            numStartX.Name = "numStartX";
            numStartX.Size = new System.Drawing.Size(70, 20);
            numStartX.TabIndex = 23;
            numStartX.Text = "0";
            numStartX.ValueChanged += new System.EventHandler(BoxChanged);
            numStartX.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // numStartY
            // 
            numStartY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numStartY.Integral = false;
            numStartY.Location = new System.Drawing.Point(141, 34);
            numStartY.Margin = new System.Windows.Forms.Padding(0);
            numStartY.MaximumValue = 3.402823E+38F;
            numStartY.MinimumValue = -3.402823E+38F;
            numStartY.Name = "numStartY";
            numStartY.Size = new System.Drawing.Size(70, 20);
            numStartY.TabIndex = 25;
            numStartY.Text = "0";
            numStartY.ValueChanged += new System.EventHandler(BoxChanged);
            numStartY.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // numEndZ
            // 
            numEndZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numEndZ.Integral = false;
            numEndZ.Location = new System.Drawing.Point(210, 53);
            numEndZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numEndZ.MaximumValue = 3.402823E+38F;
            numEndZ.MinimumValue = -3.402823E+38F;
            numEndZ.Name = "numEndZ";
            numEndZ.Size = new System.Drawing.Size(70, 20);
            numEndZ.TabIndex = 30;
            numEndZ.Text = "0";
            numEndZ.ValueChanged += new System.EventHandler(BoxChanged);
            numEndZ.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // label23
            // 
            label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label23.Location = new System.Drawing.Point(279, 34);
            label23.Margin = new System.Windows.Forms.Padding(0);
            label23.Name = "label23";
            label23.Size = new System.Drawing.Size(70, 20);
            label23.TabIndex = 37;
            label23.Text = "Spec Bright";
            label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(cboSpotFunc);
            groupBox5.Controls.Add(cboDistFunc);
            groupBox5.Controls.Add(label29);
            groupBox5.Controls.Add(label28);
            groupBox5.Controls.Add(chkLightSpec);
            groupBox5.Controls.Add(chkLightAlpha);
            groupBox5.Controls.Add(chkLightClr);
            groupBox5.Controls.Add(label27);
            groupBox5.Controls.Add(cboLightType);
            groupBox5.Dock = System.Windows.Forms.DockStyle.Left;
            groupBox5.Location = new System.Drawing.Point(0, 0);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new System.Drawing.Size(204, 97);
            groupBox5.TabIndex = 39;
            groupBox5.TabStop = false;
            groupBox5.Text = "Light Settings";
            // 
            // cboSpotFunc
            // 
            cboSpotFunc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboSpotFunc.FormattingEnabled = true;
            cboSpotFunc.Location = new System.Drawing.Point(128, 72);
            cboSpotFunc.Name = "cboSpotFunc";
            cboSpotFunc.Size = new System.Drawing.Size(70, 21);
            cboSpotFunc.TabIndex = 9;
            cboSpotFunc.SelectedIndexChanged += new System.EventHandler(lstSpotFunc_SelectedIndexChanged);
            // 
            // cboDistFunc
            // 
            cboDistFunc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboDistFunc.FormattingEnabled = true;
            cboDistFunc.Location = new System.Drawing.Point(128, 34);
            cboDistFunc.Name = "cboDistFunc";
            cboDistFunc.Size = new System.Drawing.Size(70, 21);
            cboDistFunc.TabIndex = 8;
            cboDistFunc.SelectedIndexChanged += new System.EventHandler(lstDistFunc_SelectedIndexChanged);
            // 
            // label29
            // 
            label29.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label29.Location = new System.Drawing.Point(128, 53);
            label29.Name = "label29";
            label29.Size = new System.Drawing.Size(70, 20);
            label29.TabIndex = 7;
            label29.Text = "Spot Func:";
            label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label28
            // 
            label28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label28.Location = new System.Drawing.Point(128, 15);
            label28.Name = "label28";
            label28.Size = new System.Drawing.Size(70, 20);
            label28.TabIndex = 6;
            label28.Text = "Dist Func:";
            label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkLightSpec
            // 
            chkLightSpec.AutoSize = true;
            chkLightSpec.Location = new System.Drawing.Point(9, 76);
            chkLightSpec.Name = "chkLightSpec";
            chkLightSpec.Size = new System.Drawing.Size(110, 17);
            chkLightSpec.TabIndex = 5;
            chkLightSpec.Text = "Specular Enabled";
            chkLightSpec.UseVisualStyleBackColor = true;
            chkLightSpec.CheckedChanged += new System.EventHandler(chkLightSpec_CheckedChanged);
            // 
            // chkLightAlpha
            // 
            chkLightAlpha.AutoSize = true;
            chkLightAlpha.Location = new System.Drawing.Point(9, 58);
            chkLightAlpha.Name = "chkLightAlpha";
            chkLightAlpha.Size = new System.Drawing.Size(95, 17);
            chkLightAlpha.TabIndex = 4;
            chkLightAlpha.Text = "Alpha Enabled";
            chkLightAlpha.UseVisualStyleBackColor = true;
            chkLightAlpha.CheckedChanged += new System.EventHandler(chkLightAlpha_CheckedChanged);
            // 
            // chkLightClr
            // 
            chkLightClr.AutoSize = true;
            chkLightClr.Location = new System.Drawing.Point(9, 40);
            chkLightClr.Name = "chkLightClr";
            chkLightClr.Size = new System.Drawing.Size(92, 17);
            chkLightClr.TabIndex = 3;
            chkLightClr.Text = "Color Enabled";
            chkLightClr.UseVisualStyleBackColor = true;
            chkLightClr.CheckedChanged += new System.EventHandler(chkLightClr_CheckedChanged);
            // 
            // label27
            // 
            label27.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label27.Location = new System.Drawing.Point(6, 15);
            label27.Name = "label27";
            label27.Size = new System.Drawing.Size(46, 21);
            label27.TabIndex = 2;
            label27.Text = "Type:";
            label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboLightType
            // 
            cboLightType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLightType.FormattingEnabled = true;
            cboLightType.Location = new System.Drawing.Point(51, 15);
            cboLightType.Name = "cboLightType";
            cboLightType.Size = new System.Drawing.Size(73, 21);
            cboLightType.TabIndex = 0;
            cboLightType.SelectedIndexChanged += new System.EventHandler(lstLightType_SelectedIndexChanged);
            // 
            // Fog
            // 
            Fog.Controls.Add(groupBox4);
            Fog.Controls.Add(groupBox3);
            Fog.Location = new System.Drawing.Point(4, 25);
            Fog.Name = "Fog";
            Fog.Size = new System.Drawing.Size(651, 97);
            Fog.TabIndex = 3;
            Fog.Text = "Fog";
            Fog.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(button3);
            groupBox4.Controls.Add(button4);
            groupBox4.Controls.Add(label16);
            groupBox4.Controls.Add(numFogEndZ);
            groupBox4.Controls.Add(numFogStartZ);
            groupBox4.Controls.Add(label17);
            groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox4.Location = new System.Drawing.Point(185, 0);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new System.Drawing.Size(466, 97);
            groupBox4.TabIndex = 40;
            groupBox4.TabStop = false;
            groupBox4.Text = "Edit Frame";
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(311, 15);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(59, 20);
            button3.TabIndex = 35;
            button3.Text = "Paste";
            button3.UseVisualStyleBackColor = true;
            button3.Click += new System.EventHandler(button3_Click);
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(253, 15);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(59, 20);
            button4.TabIndex = 34;
            button4.Text = "Copy";
            button4.UseVisualStyleBackColor = true;
            button4.Click += new System.EventHandler(button4_Click);
            // 
            // label16
            // 
            label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label16.Location = new System.Drawing.Point(6, 15);
            label16.Margin = new System.Windows.Forms.Padding(0);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(55, 20);
            label16.TabIndex = 7;
            label16.Text = "Start Z:";
            label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numFogEndZ
            // 
            numFogEndZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numFogEndZ.Integral = false;
            numFogEndZ.Location = new System.Drawing.Point(183, 15);
            numFogEndZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numFogEndZ.MaximumValue = 3.402823E+38F;
            numFogEndZ.MinimumValue = -3.402823E+38F;
            numFogEndZ.Name = "numFogEndZ";
            numFogEndZ.Size = new System.Drawing.Size(70, 20);
            numFogEndZ.TabIndex = 10;
            numFogEndZ.Text = "0";
            numFogEndZ.ValueChanged += new System.EventHandler(BoxChanged);
            numFogEndZ.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // numFogStartZ
            // 
            numFogStartZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numFogStartZ.Integral = false;
            numFogStartZ.Location = new System.Drawing.Point(60, 15);
            numFogStartZ.Margin = new System.Windows.Forms.Padding(0);
            numFogStartZ.MaximumValue = 3.402823E+38F;
            numFogStartZ.MinimumValue = -3.402823E+38F;
            numFogStartZ.Name = "numFogStartZ";
            numFogStartZ.Size = new System.Drawing.Size(70, 20);
            numFogStartZ.TabIndex = 9;
            numFogStartZ.Text = "0";
            numFogStartZ.ValueChanged += new System.EventHandler(BoxChanged);
            numFogStartZ.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // label17
            // 
            label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label17.Location = new System.Drawing.Point(129, 15);
            label17.Margin = new System.Windows.Forms.Padding(0);
            label17.Name = "label17";
            label17.Size = new System.Drawing.Size(55, 20);
            label17.TabIndex = 8;
            label17.Text = "End Z:";
            label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label15);
            groupBox3.Controls.Add(cboFogType);
            groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            groupBox3.Location = new System.Drawing.Point(0, 0);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(185, 97);
            groupBox3.TabIndex = 39;
            groupBox3.TabStop = false;
            groupBox3.Text = "Fog Settings";
            // 
            // label15
            // 
            label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label15.Location = new System.Drawing.Point(6, 15);
            label15.Margin = new System.Windows.Forms.Padding(0);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(42, 21);
            label15.TabIndex = 38;
            label15.Text = "Type:";
            label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboFogType
            // 
            cboFogType.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right);
            cboFogType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboFogType.FormattingEnabled = true;
            cboFogType.Location = new System.Drawing.Point(47, 15);
            cboFogType.Name = "cboFogType";
            cboFogType.Size = new System.Drawing.Size(132, 21);
            cboFogType.TabIndex = 37;
            cboFogType.SelectedIndexChanged += new System.EventHandler(comboBox3_SelectedIndexChanged);
            // 
            // Cameras
            // 
            Cameras.Controls.Add(panel2);
            Cameras.Location = new System.Drawing.Point(4, 25);
            Cameras.Name = "Cameras";
            Cameras.Size = new System.Drawing.Size(651, 97);
            Cameras.TabIndex = 4;
            Cameras.Text = "Cameras";
            Cameras.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Controls.Add(groupBox2);
            panel2.Controls.Add(panel1);
            panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            panel2.Location = new System.Drawing.Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(651, 97);
            panel2.TabIndex = 27;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label11);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(numPosZ);
            groupBox2.Controls.Add(btnCut);
            groupBox2.Controls.Add(btnPaste);
            groupBox2.Controls.Add(btnCopy);
            groupBox2.Controls.Add(btnClear);
            groupBox2.Controls.Add(numAimX);
            groupBox2.Controls.Add(numFarZ);
            groupBox2.Controls.Add(numRotY);
            groupBox2.Controls.Add(numNearZ);
            groupBox2.Controls.Add(numRotX);
            groupBox2.Controls.Add(label12);
            groupBox2.Controls.Add(numTwist);
            groupBox2.Controls.Add(label13);
            groupBox2.Controls.Add(numPosX);
            groupBox2.Controls.Add(numAspect);
            groupBox2.Controls.Add(numRotZ);
            groupBox2.Controls.Add(numHeight);
            groupBox2.Controls.Add(numPosY);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(numAimY);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(numAimZ);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(numFovY);
            groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox2.Location = new System.Drawing.Point(149, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(502, 97);
            groupBox2.TabIndex = 39;
            groupBox2.TabStop = false;
            groupBox2.Text = "Edit Frame";
            // 
            // label11
            // 
            label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label11.Location = new System.Drawing.Point(266, 49);
            label11.Margin = new System.Windows.Forms.Padding(0);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(47, 20);
            label11.TabIndex = 22;
            label11.Text = "Height:";
            label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label8.Location = new System.Drawing.Point(266, 11);
            label8.Margin = new System.Windows.Forms.Padding(0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(47, 20);
            label8.TabIndex = 9;
            label8.Text = "Twist:";
            label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label1.Location = new System.Drawing.Point(5, 30);
            label1.Margin = new System.Windows.Forms.Padding(0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(55, 20);
            label1.TabIndex = 0;
            label1.Text = "Position:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label9.Location = new System.Drawing.Point(266, 30);
            label9.Margin = new System.Windows.Forms.Padding(0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(47, 20);
            label9.TabIndex = 10;
            label9.Text = "Fov Y:";
            label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPosZ
            // 
            numPosZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numPosZ.Integral = false;
            numPosZ.Location = new System.Drawing.Point(197, 30);
            numPosZ.Margin = new System.Windows.Forms.Padding(0);
            numPosZ.MaximumValue = 3.402823E+38F;
            numPosZ.MinimumValue = -3.402823E+38F;
            numPosZ.Name = "numPosZ";
            numPosZ.Size = new System.Drawing.Size(70, 20);
            numPosZ.TabIndex = 5;
            numPosZ.Text = "0";
            numPosZ.ValueChanged += new System.EventHandler(BoxChanged);
            numPosZ.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // btnCut
            // 
            btnCut.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            btnCut.Location = new System.Drawing.Point(382, 50);
            btnCut.Name = "btnCut";
            btnCut.Size = new System.Drawing.Size(59, 20);
            btnCut.TabIndex = 30;
            btnCut.Text = "Cut";
            btnCut.UseVisualStyleBackColor = true;
            btnCut.Click += new System.EventHandler(btnCut_Click);
            // 
            // btnPaste
            // 
            btnPaste.Location = new System.Drawing.Point(440, 50);
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new System.Drawing.Size(59, 20);
            btnPaste.TabIndex = 32;
            btnPaste.Text = "Paste";
            btnPaste.UseVisualStyleBackColor = true;
            btnPaste.Click += new System.EventHandler(btnPaste_Click);
            // 
            // btnCopy
            // 
            btnCopy.Location = new System.Drawing.Point(382, 69);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new System.Drawing.Size(59, 20);
            btnCopy.TabIndex = 31;
            btnCopy.Text = "Copy";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += new System.EventHandler(btnCopy_Click);
            // 
            // btnClear
            // 
            btnClear.Location = new System.Drawing.Point(440, 69);
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(59, 20);
            btnClear.TabIndex = 33;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += new System.EventHandler(btnClear_Click);
            // 
            // numAimX
            // 
            numAimX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numAimX.Integral = false;
            numAimX.Location = new System.Drawing.Point(59, 68);
            numAimX.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numAimX.MaximumValue = 3.402823E+38F;
            numAimX.MinimumValue = -3.402823E+38F;
            numAimX.Name = "numAimX";
            numAimX.Size = new System.Drawing.Size(70, 20);
            numAimX.TabIndex = 11;
            numAimX.Text = "0";
            numAimX.ValueChanged += new System.EventHandler(BoxChanged);
            numAimX.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // numFarZ
            // 
            numFarZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numFarZ.Integral = false;
            numFarZ.Location = new System.Drawing.Point(428, 30);
            numFarZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numFarZ.MaximumValue = 3.402823E+38F;
            numFarZ.MinimumValue = -3.402823E+38F;
            numFarZ.Name = "numFarZ";
            numFarZ.Size = new System.Drawing.Size(70, 20);
            numFarZ.TabIndex = 29;
            numFarZ.Text = "0";
            numFarZ.ValueChanged += new System.EventHandler(BoxChanged);
            numFarZ.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // numRotY
            // 
            numRotY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numRotY.Integral = false;
            numRotY.Location = new System.Drawing.Point(128, 49);
            numRotY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numRotY.MaximumValue = 3.402823E+38F;
            numRotY.MinimumValue = -3.402823E+38F;
            numRotY.Name = "numRotY";
            numRotY.Size = new System.Drawing.Size(70, 20);
            numRotY.TabIndex = 7;
            numRotY.Text = "0";
            numRotY.ValueChanged += new System.EventHandler(BoxChanged);
            numRotY.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // numNearZ
            // 
            numNearZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numNearZ.Integral = false;
            numNearZ.Location = new System.Drawing.Point(428, 11);
            numNearZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numNearZ.MaximumValue = 3.402823E+38F;
            numNearZ.MinimumValue = -3.402823E+38F;
            numNearZ.Name = "numNearZ";
            numNearZ.Size = new System.Drawing.Size(70, 20);
            numNearZ.TabIndex = 28;
            numNearZ.Text = "0";
            numNearZ.ValueChanged += new System.EventHandler(BoxChanged);
            numNearZ.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // numRotX
            // 
            numRotX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numRotX.Integral = false;
            numRotX.Location = new System.Drawing.Point(59, 49);
            numRotX.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numRotX.MaximumValue = 3.402823E+38F;
            numRotX.MinimumValue = -3.402823E+38F;
            numRotX.Name = "numRotX";
            numRotX.Size = new System.Drawing.Size(70, 20);
            numRotX.TabIndex = 6;
            numRotX.Text = "0";
            numRotX.ValueChanged += new System.EventHandler(BoxChanged);
            numRotX.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // label12
            // 
            label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label12.Location = new System.Drawing.Point(381, 30);
            label12.Margin = new System.Windows.Forms.Padding(0);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(48, 20);
            label12.TabIndex = 27;
            label12.Text = "Far Z:";
            label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTwist
            // 
            numTwist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numTwist.Integral = false;
            numTwist.Location = new System.Drawing.Point(312, 11);
            numTwist.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numTwist.MaximumValue = 3.402823E+38F;
            numTwist.MinimumValue = -3.402823E+38F;
            numTwist.Name = "numTwist";
            numTwist.Size = new System.Drawing.Size(70, 20);
            numTwist.TabIndex = 12;
            numTwist.Text = "0";
            numTwist.ValueChanged += new System.EventHandler(BoxChanged);
            numTwist.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // label13
            // 
            label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label13.Location = new System.Drawing.Point(381, 11);
            label13.Margin = new System.Windows.Forms.Padding(0);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(48, 20);
            label13.TabIndex = 26;
            label13.Text = "Near Z:";
            label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPosX
            // 
            numPosX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numPosX.Integral = false;
            numPosX.Location = new System.Drawing.Point(59, 30);
            numPosX.Margin = new System.Windows.Forms.Padding(0);
            numPosX.MaximumValue = 3.402823E+38F;
            numPosX.MinimumValue = -3.402823E+38F;
            numPosX.Name = "numPosX";
            numPosX.Size = new System.Drawing.Size(70, 20);
            numPosX.TabIndex = 3;
            numPosX.Text = "0";
            numPosX.ValueChanged += new System.EventHandler(BoxChanged);
            numPosX.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // numAspect
            // 
            numAspect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numAspect.Integral = false;
            numAspect.Location = new System.Drawing.Point(312, 68);
            numAspect.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numAspect.MaximumValue = 3.402823E+38F;
            numAspect.MinimumValue = -3.402823E+38F;
            numAspect.Name = "numAspect";
            numAspect.Size = new System.Drawing.Size(70, 20);
            numAspect.TabIndex = 25;
            numAspect.Text = "0";
            numAspect.ValueChanged += new System.EventHandler(BoxChanged);
            numAspect.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // numRotZ
            // 
            numRotZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numRotZ.Integral = false;
            numRotZ.Location = new System.Drawing.Point(197, 49);
            numRotZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numRotZ.MaximumValue = 3.402823E+38F;
            numRotZ.MinimumValue = -3.402823E+38F;
            numRotZ.Name = "numRotZ";
            numRotZ.Size = new System.Drawing.Size(70, 20);
            numRotZ.TabIndex = 8;
            numRotZ.Text = "0";
            numRotZ.ValueChanged += new System.EventHandler(BoxChanged);
            numRotZ.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // numHeight
            // 
            numHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numHeight.Integral = false;
            numHeight.Location = new System.Drawing.Point(312, 49);
            numHeight.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numHeight.MaximumValue = 3.402823E+38F;
            numHeight.MinimumValue = -3.402823E+38F;
            numHeight.Name = "numHeight";
            numHeight.Size = new System.Drawing.Size(70, 20);
            numHeight.TabIndex = 24;
            numHeight.Text = "0";
            numHeight.ValueChanged += new System.EventHandler(BoxChanged);
            numHeight.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // numPosY
            // 
            numPosY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numPosY.Integral = false;
            numPosY.Location = new System.Drawing.Point(128, 30);
            numPosY.Margin = new System.Windows.Forms.Padding(0);
            numPosY.MaximumValue = 3.402823E+38F;
            numPosY.MinimumValue = -3.402823E+38F;
            numPosY.Name = "numPosY";
            numPosY.Size = new System.Drawing.Size(70, 20);
            numPosY.TabIndex = 4;
            numPosY.Text = "0";
            numPosY.ValueChanged += new System.EventHandler(BoxChanged);
            numPosY.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // label10
            // 
            label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label10.Location = new System.Drawing.Point(266, 68);
            label10.Margin = new System.Windows.Forms.Padding(0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(47, 20);
            label10.TabIndex = 23;
            label10.Text = "Aspect:";
            label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label4.Location = new System.Drawing.Point(59, 11);
            label4.Margin = new System.Windows.Forms.Padding(0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(70, 20);
            label4.TabIndex = 4;
            label4.Text = "X";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label5.Location = new System.Drawing.Point(128, 11);
            label5.Margin = new System.Windows.Forms.Padding(0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(70, 20);
            label5.TabIndex = 7;
            label5.Text = "Y";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numAimY
            // 
            numAimY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numAimY.Integral = false;
            numAimY.Location = new System.Drawing.Point(128, 68);
            numAimY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numAimY.MaximumValue = 3.402823E+38F;
            numAimY.MinimumValue = -3.402823E+38F;
            numAimY.Name = "numAimY";
            numAimY.Size = new System.Drawing.Size(70, 20);
            numAimY.TabIndex = 21;
            numAimY.Text = "0";
            numAimY.ValueChanged += new System.EventHandler(BoxChanged);
            numAimY.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // label2
            // 
            label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label2.Location = new System.Drawing.Point(5, 68);
            label2.Margin = new System.Windows.Forms.Padding(0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(55, 20);
            label2.TabIndex = 1;
            label2.Text = "Aim:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numAimZ
            // 
            numAimZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numAimZ.Integral = false;
            numAimZ.Location = new System.Drawing.Point(197, 68);
            numAimZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numAimZ.MaximumValue = 3.402823E+38F;
            numAimZ.MinimumValue = -3.402823E+38F;
            numAimZ.Name = "numAimZ";
            numAimZ.Size = new System.Drawing.Size(70, 20);
            numAimZ.TabIndex = 19;
            numAimZ.Text = "0";
            numAimZ.ValueChanged += new System.EventHandler(BoxChanged);
            numAimZ.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // label3
            // 
            label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label3.Location = new System.Drawing.Point(5, 49);
            label3.Margin = new System.Windows.Forms.Padding(0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(55, 20);
            label3.TabIndex = 2;
            label3.Text = "Rotation:";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label6.Location = new System.Drawing.Point(197, 11);
            label6.Margin = new System.Windows.Forms.Padding(0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(70, 20);
            label6.TabIndex = 8;
            label6.Text = "Z";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numFovY
            // 
            numFovY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numFovY.Integral = false;
            numFovY.Location = new System.Drawing.Point(312, 30);
            numFovY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numFovY.MaximumValue = 3.402823E+38F;
            numFovY.MinimumValue = -3.402823E+38F;
            numFovY.Name = "numFovY";
            numFovY.Size = new System.Drawing.Size(70, 20);
            numFovY.TabIndex = 13;
            numFovY.Text = "0";
            numFovY.ValueChanged += new System.EventHandler(BoxChanged);
            numFovY.MouseDown += new System.Windows.Forms.MouseEventHandler(box_MouseDown);
            // 
            // panel1
            // 
            panel1.Controls.Add(btnUseCamera);
            panel1.Controls.Add(groupBox1);
            panel1.Dock = System.Windows.Forms.DockStyle.Left;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(149, 97);
            panel1.TabIndex = 40;
            // 
            // btnUseCamera
            // 
            btnUseCamera.Location = new System.Drawing.Point(3, 62);
            btnUseCamera.Name = "btnUseCamera";
            btnUseCamera.Size = new System.Drawing.Size(138, 23);
            btnUseCamera.TabIndex = 38;
            btnUseCamera.Text = "Use Current Camera";
            btnUseCamera.UseVisualStyleBackColor = true;
            btnUseCamera.Click += new System.EventHandler(button1_Click_1);
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cboCamProj);
            groupBox1.Controls.Add(label14);
            groupBox1.Controls.Add(cboCamType);
            groupBox1.Controls.Add(label7);
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(147, 60);
            groupBox1.TabIndex = 38;
            groupBox1.TabStop = false;
            groupBox1.Text = "Camera Settings";
            // 
            // cboCamProj
            // 
            cboCamProj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboCamProj.FormattingEnabled = true;
            cboCamProj.Location = new System.Drawing.Point(72, 35);
            cboCamProj.Name = "cboCamProj";
            cboCamProj.Size = new System.Drawing.Size(70, 21);
            cboCamProj.TabIndex = 35;
            cboCamProj.SelectedIndexChanged += new System.EventHandler(lstCamProj_SelectedIndexChanged);
            // 
            // label14
            // 
            label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label14.Location = new System.Drawing.Point(72, 16);
            label14.Margin = new System.Windows.Forms.Padding(0);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(70, 20);
            label14.TabIndex = 37;
            label14.Text = "Projection";
            label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboCamType
            // 
            cboCamType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboCamType.FormattingEnabled = true;
            cboCamType.Location = new System.Drawing.Point(3, 35);
            cboCamType.Name = "cboCamType";
            cboCamType.Size = new System.Drawing.Size(70, 21);
            cboCamType.TabIndex = 34;
            cboCamType.SelectedIndexChanged += new System.EventHandler(lstCamType_SelectedIndexChanged);
            // 
            // label7
            // 
            label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label7.Location = new System.Drawing.Point(3, 16);
            label7.Margin = new System.Windows.Forms.Padding(0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(70, 20);
            label7.TabIndex = 36;
            label7.Text = "Type";
            label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nodeType
            // 
            nodeType.AutoSize = true;
            nodeType.BackColor = System.Drawing.SystemColors.Control;
            nodeType.Location = new System.Drawing.Point(310, 3);
            nodeType.Name = "nodeType";
            nodeType.Size = new System.Drawing.Size(49, 13);
            nodeType.TabIndex = 0;
            nodeType.Text = "LightSet:";
            nodeType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboNodeList
            // 
            cboNodeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboNodeList.FormattingEnabled = true;
            cboNodeList.Location = new System.Drawing.Point(359, 1);
            cboNodeList.Name = "cboNodeList";
            cboNodeList.Size = new System.Drawing.Size(72, 21);
            cboNodeList.TabIndex = 0;
            cboNodeList.SelectedIndexChanged += new System.EventHandler(nodeList_SelectedIndexChanged);
            // 
            // btnRename
            // 
            btnRename.Location = new System.Drawing.Point(432, 0);
            btnRename.Name = "btnRename";
            btnRename.Size = new System.Drawing.Size(64, 23);
            btnRename.TabIndex = 20;
            btnRename.Text = "Rename";
            btnRename.UseVisualStyleBackColor = true;
            btnRename.Click += new System.EventHandler(button1_Click);
            // 
            // btnNew
            // 
            btnNew.Location = new System.Drawing.Point(496, 0);
            btnNew.Name = "btnNew";
            btnNew.Size = new System.Drawing.Size(45, 23);
            btnNew.TabIndex = 21;
            btnNew.Text = "New";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += new System.EventHandler(btnNew_Click);
            // 
            // btnDelete
            // 
            btnDelete.Location = new System.Drawing.Point(541, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(59, 23);
            btnDelete.TabIndex = 22;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += new System.EventHandler(btnDelete_Click);
            // 
            // SCN0Editor
            // 
            Controls.Add(btnDelete);
            Controls.Add(nodeType);
            Controls.Add(btnNew);
            Controls.Add(btnRename);
            Controls.Add(cboNodeList);
            Controls.Add(tabControl1);
            Name = "SCN0Editor";
            Size = new System.Drawing.Size(659, 126);
            tabControl1.ResumeLayout(false);
            LightSets.ResumeLayout(false);
            AmbLights.ResumeLayout(false);
            AmbLights.PerformLayout();
            Lights.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            Fog.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            Cameras.ResumeLayout(false);
            panel2.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            panel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

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
            get => _mainWindow.SelectedSCN0;
            set => _mainWindow.SelectedSCN0 = value;
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0MaterialRefNode TargetTexRef { get => _mainWindow.TargetTexRef; set => _mainWindow.TargetTexRef = value; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get => _mainWindow.CurrentFrame;
            set => _mainWindow.CurrentFrame = value;
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get => (MDL0Node)_mainWindow.TargetModel;
            set => _mainWindow.TargetModel = value;
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
            {
                return;
            }

            for (int i = 0; i < (_tabIndex == 2 ? 10 : _tabIndex == 3 ? 2 : _tabIndex == 4 ? 15 : 0); i++)
            {
                ResetBox(i);
            }

            if (_mainWindow.InterpolationEditor != null &&
                _mainWindow.InterpolationEditor.Visible &&
                Entry != null && SelectedAnimation != null &&
                CurrentFrame > 0 &&
                _mainWindow.InterpolationEditor._targetNode != Entry)
            {
                _mainWindow.InterpolationEditor.SetTarget(Entry);
            }
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
            {
                _mainWindow._SCN0Camera.SetCamera(_mainWindow.ModelPanel.CurrentViewport, CurrentFrame - 1, true);
            }

            _mainWindow.UpdateModel();
        }

        private bool _updating = false;

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
            {
                btnRename.Enabled = true;
            }

            _mainWindow.KeyframePanel.TargetSequence = cboNodeList.SelectedItem as ResourceNode;

            switch (_tabIndex)
            {
                case 0:

                    _mainWindow._SCN0LightSet = _lightSet = cboNodeList.SelectedItem as SCN0LightSetNode;

                    SCN0GroupNode amb = SelectedAnimation.GetFolder<SCN0AmbientLightNode>();

                    cboAmb.Items.Clear();
                    cboAmb.Items.Add("<null>");
                    if (amb != null)
                    {
                        cboAmb.Items.AddRange(amb.Children.ToArray());
                    }

                    SCN0GroupNode lights = SelectedAnimation.GetFolder<SCN0LightNode>();

                    cboLight0.Items.Clear();
                    cboLight0.Items.Add("<null>");
                    if (lights != null)
                    {
                        foreach (SCN0LightNode s in lights.Children)
                        {
                            cboLight0.Items.Add(s);
                        }
                    }

                    cboLight1.Items.Clear();
                    cboLight1.Items.Add("<null>");
                    if (lights != null)
                    {
                        foreach (SCN0LightNode s in lights.Children)
                        {
                            cboLight1.Items.Add(s);
                        }
                    }

                    cboLight2.Items.Clear();
                    cboLight2.Items.Add("<null>");
                    if (lights != null)
                    {
                        foreach (SCN0LightNode s in lights.Children)
                        {
                            cboLight2.Items.Add(s);
                        }
                    }

                    cboLight3.Items.Clear();
                    cboLight3.Items.Add("<null>");
                    if (lights != null)
                    {
                        foreach (SCN0LightNode s in lights.Children)
                        {
                            cboLight3.Items.Add(s);
                        }
                    }

                    cboLight4.Items.Clear();
                    cboLight4.Items.Add("<null>");
                    if (lights != null)
                    {
                        foreach (SCN0LightNode s in lights.Children)
                        {
                            cboLight4.Items.Add(s);
                        }
                    }

                    cboLight5.Items.Clear();
                    cboLight5.Items.Add("<null>");
                    if (lights != null)
                    {
                        foreach (SCN0LightNode s in lights.Children)
                        {
                            cboLight5.Items.Add(s);
                        }
                    }

                    cboLight6.Items.Clear();
                    cboLight6.Items.Add("<null>");
                    if (lights != null)
                    {
                        foreach (SCN0LightNode s in lights.Children)
                        {
                            cboLight6.Items.Add(s);
                        }
                    }

                    cboLight7.Items.Clear();
                    cboLight7.Items.Add("<null>");
                    if (lights != null)
                    {
                        foreach (SCN0LightNode s in lights.Children)
                        {
                            cboLight7.Items.Add(s);
                        }
                    }

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

        private void UpdateNodeList()
        {
            cboNodeList.Items.Clear();
            SCN0GroupNode g;
            if (SelectedAnimation != null && (g = SelectedAnimation.GetFolder((SCN0GroupNode.GroupType)_tabIndex)) != null)
            {
                Enabled = true;
                foreach (SCN0EntryNode s in g.Children)
                {
                    cboNodeList.Items.Add(s);
                }
            }
            else
            {
                Enabled = false;
            }

            if (cboNodeList.Items.Count > 0)
            {
                cboNodeList.SelectedIndex = 0;
            }
            else
            {
                btnRename.Enabled = false;
            }
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
                {
                    d.ShowDialog(this, cboNodeList.SelectedItem as ResourceNode);
                }

                int i = cboNodeList.SelectedIndex;
                UpdateNodeList();
                cboNodeList.SelectedIndex = i;
            }
        }

        private void lstAmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _lightSet.Ambience = cboAmb.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void lstLight0_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _lightSet.Light0 = cboLight0.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void lstLight1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _lightSet.Light1 = cboLight1.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void lstLight2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _lightSet.Light2 = cboLight2.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void lstLight3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _lightSet.Light3 = cboLight3.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void lstLight4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _lightSet.Light4 = cboLight4.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void lstLight5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _lightSet.Light5 = cboLight5.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void lstLight6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _lightSet.Light6 = cboLight6.SelectedItem.ToString(); UpdateSelectedLightSets();
        }

        private void lstLight7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

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

        private CameraAnimationFrame _tempCameraFrame;
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
            {
                p[i] = _transBoxes[2][i].Value;
            }

            _tempCameraFrame = frame;
        }

        private unsafe void btnPaste_Click(object sender, EventArgs e)
        {
            CameraAnimationFrame frame = _tempCameraFrame;
            float* p = (float*)&frame;

            for (int i = 0; i < 15; i++)
            {
                if (_transBoxes[2][i].Value != p[i])
                {
                    _transBoxes[2][i].Value = p[i];
                }

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

        private FogAnimationFrame _tempFogFrame;
        private unsafe void button4_Click(object sender, EventArgs e)
        {
            FogAnimationFrame frame = new FogAnimationFrame();
            float* p = (float*)&frame;

            for (int i = 0; i < 2; i++)
            {
                p[i] = _transBoxes[1][i].Value;
            }

            _tempFogFrame = frame;
        }

        private unsafe void button3_Click(object sender, EventArgs e)
        {
            FogAnimationFrame frame = _tempFogFrame;
            float* p = (float*)&frame;

            for (int i = 0; i < 2; i++)
            {
                if (_transBoxes[1][i].Value != p[i])
                {
                    _transBoxes[1][i].Value = p[i];
                }

                BoxChanged(_transBoxes[1][i], null);
            }
        }

        private LightAnimationFrame _tempLightFrame;
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
                {
                    _transBoxes[0][i].Value = p[i];
                }

                BoxChanged(_transBoxes[0][i], null);
            }
        }

        private unsafe void lightCopy_Click(object sender, EventArgs e)
        {
            LightAnimationFrame frame = new LightAnimationFrame();
            float* p = (float*)&frame;

            for (int i = 0; i < 10; i++)
            {
                p[i] = _transBoxes[0][i].Value;
            }

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
            {
                return;
            }

            _mainWindow.InterpolationEditor.interpolationViewer._updating = true;
            if (box.BackColor == Color.Yellow)
            {
                KeyframeEntry kfe = Entry.KeyArrays[(int)box.Tag].GetKeyframe(CurrentFrame - 1);
                if (kfe != null)
                {
                    _mainWindow.InterpolationEditor.SelectedKeyframe = kfe;
                }
            }
            else
            {
                _mainWindow.InterpolationEditor.SelectedKeyframe = null;
            }

            _mainWindow.InterpolationEditor.interpolationViewer._updating = false;
        }

        private void box_MouseDown(object sender, MouseEventArgs e)
        {
            NumericInputBox box = sender as NumericInputBox;

            int type = (int)box.Tag;
            if (_mainWindow.InterpolationEditor != null && _mainWindow.InterpolationEditor.Visible)
            {
                if (_mainWindow.InterpolationEditor.SelectedMode != type)
                {
                    _mainWindow.InterpolationEditor.SelectedMode = type;
                }

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
