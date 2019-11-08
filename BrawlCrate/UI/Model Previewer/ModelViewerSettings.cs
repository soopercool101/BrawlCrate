using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.Internal.Windows.Controls;
using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Graphics;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlCrate.UI.Model_Previewer
{
    public class ModelViewerSettingsDialog : Form
    {
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private NumericInputBox ax;
        private NumericInputBox radius;
        private NumericInputBox dx;
        private NumericInputBox sx;
        private NumericInputBox sy;
        private NumericInputBox dy;
        private NumericInputBox azimuth;
        private NumericInputBox ay;
        private NumericInputBox sz;
        private NumericInputBox dz;
        private NumericInputBox elevation;
        private NumericInputBox az;
        private GroupBox grpLighting;
        private GroupBox grpProjection;
        private Label label10;
        private Label label9;
        private Label label11;
        private Label label13;
        private Label label12;
        private Label label14;
        private NumericInputBox farZ;
        private NumericInputBox nearZ;
        private NumericInputBox yFov;
        private NumericInputBox zScale;
        private NumericInputBox tScale;
        private NumericInputBox rScale;
        private Label label17;
        private Label label16;
        private GroupBox grpColors;
        private Label lblLineColor;
        private Label lblLineText;
        private Label label20;
        private Label lblOrbColor;
        private Label lblOrbText;
        private Label label15;
        private Label lblCol1Color;
        private Label lblCol1Text;
        private Label label24;
        private NumericInputBox maxUndoCount;
        private Label label18;
        private NumericInputBox ez;
        private NumericInputBox ey;
        private Label label8;
        private NumericInputBox ex;
        private Label label23;
        private Label label22;
        private Label label21;
        private Label label19;
        private Label label25;
        private ComboBox cboProjection;
        private CheckBox chkRetrieveCorrAnims;
        private CheckBox chkSyncTexToObj;
        private CheckBox chkSyncObjToVIS;
        private CheckBox chkDisableBonesOnPlay;
        private CheckBox chkDisableHighlight;
        private CheckBox chkSnapBonesToFloor;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Panel panel1;
        private CheckBox chkMaximize;
        private CheckBox chkPrecalcBoxes;
        private CheckBox chkTextOverlays;
        private TabPage tabPage3;
        private GroupBox groupBox1;
        private CheckBox chkTanCam;
        private CheckBox chkTanFog;
        private CheckBox chkTanLight;
        private CheckBox chkTanSHP;
        private CheckBox chkTanSRT;
        private CheckBox chkTanCHR;
        private Button btnResetSettings;
        private Button btnImportSettings;
        private Button btnExportSettings;
        private Label label28;
        private Label label27;
        private Label label26;
        private NumericInputBox numPosRX;
        private NumericInputBox numPosSZ;
        private NumericInputBox numPosSY;
        private NumericInputBox numPosSX;
        private NumericInputBox numPosTX;
        private NumericInputBox numPosTY;
        private NumericInputBox numPosTZ;
        private NumericInputBox numPosRZ;
        private NumericInputBox numPosRY;
        private GroupBox groupBox2;
        private RadioButton chkDefaultPos;
        private RadioButton chkCurrentPos;
        private CheckBox chkSaveWindowPosition;
        private CheckBox chkLightDirectional;
        private CheckBox chkLightEnabled;
        private CheckBox chkUsePointsAsBones;
        private CheckBox chkScaleBones;
        private CheckBox chkHideMainWindow;
        private CheckBox chkPixelLighting;
        private CheckBox chkContextLoop;
        private Label lblStgBGColor;
        private Label lblBGColor;
        private Label lblStgBGColorText;
        private Label lblBGColorText;
        private Label label33;
        private Label label34;
        private ModelEditControl.ModelEditControl _form;

        public ModelViewerSettingsDialog()
        {
            InitializeComponent();
            _dlgColor = new GoodColorDialog();
            maxUndoCount._integral = true;
            _boxes[0] = ax;
            _boxes[1] = ay;
            _boxes[2] = az;
            _boxes[3] = ex;
            _boxes[4] = radius;
            _boxes[5] = azimuth;
            _boxes[6] = elevation;
            _boxes[7] = dx;
            _boxes[8] = dy;
            _boxes[9] = dz;
            _boxes[10] = ey;
            _boxes[11] = sx;
            _boxes[12] = sy;
            _boxes[13] = sz;
            _boxes[14] = ez;
            _boxes[15] = tScale;
            _boxes[16] = rScale;
            _boxes[17] = zScale;
            _boxes[18] = yFov;
            _boxes[19] = nearZ;
            _boxes[20] = farZ;

            _boxes[21] = numPosSX;
            _boxes[22] = numPosSY;
            _boxes[23] = numPosSZ;
            _boxes[24] = numPosRX;
            _boxes[25] = numPosRY;
            _boxes[26] = numPosRZ;
            _boxes[27] = numPosTX;
            _boxes[28] = numPosTY;
            _boxes[29] = numPosTZ;

            _boxes[30] = maxUndoCount;

            for (int i = 0; i < 15; i++)
            {
                if (i < 4 || i > 6)
                {
                    _boxes[i]._maxValue = 255;
                    _boxes[i]._minValue = 0;
                }
            }

            foreach (NumericInputBox b in _boxes)
            {
                b.ValueChanged += BoxValueChanged;
            }

            _updating = true;
            cboProjection.DataSource = Enum.GetNames(typeof(ViewportProjection));
            _updating = false;
        }

        private readonly NumericInputBox[] _boxes = new NumericInputBox[31];
        private readonly float[] _origValues = new float[31];
        private Color _origNode, _origBone, _origFloor;
        private readonly CheckBox[] _checkBoxes = new CheckBox[9];
        private readonly bool[] _origChecks = new bool[9];

        public void Show(ModelEditControl.ModelEditControl owner)
        {
            _form = owner;
            _form.RenderLightDisplay = true;
            _form.ModelPanel.OnCurrentViewportChanged += UpdateViewport;

            UpdateAll();

            base.Show(_form as IWin32Window);
        }

        private ModelPanelViewport current;

        public void UpdateAll()
        {
            _updating = true;

            maxUndoCount.Value = _form.AllowedUndos;
            _origValues[30] = _boxes[30].Value;
            _boxes[30].Tag = 30;

            _origBone = MDL0BoneNode.DefaultLineColor;
            _origNode = MDL0BoneNode.DefaultNodeColor;
            _origFloor = ModelEditorBase._floorHue;

            chkHideMainWindow.Checked = _form._hideMainWindow;
            chkDisableBonesOnPlay.Checked = _form.DisableBonesWhenPlaying;
            chkDisableHighlight.Checked = _form.DoNotHighlightOnMouseMove;
            chkMaximize.Checked = _form._maximize;
            chkPrecalcBoxes.Checked = _form.UseBindStateBoxes;
            chkRetrieveCorrAnims.Checked = _form.RetrieveCorrespondingAnimations;
            chkSaveWindowPosition.Checked = _form._savePosition;
            chkSnapBonesToFloor.Checked = _form.SnapBonesToCollisions;
            chkSyncObjToVIS.Checked = _form.SyncVIS0;
            chkSyncTexToObj.Checked = _form.SyncTexturesToObjectList;
            chkTanCam.Checked = SCN0CameraNode._generateTangents;
            chkTanCHR.Checked = CHR0EntryNode._generateTangents;
            chkTanFog.Checked = SCN0FogNode._generateTangents;
            chkTanLight.Checked = SCN0LightNode._generateTangents;
            chkTanSHP.Checked = SHP0VertexSetNode._generateTangents;
            chkTanSRT.Checked = SRT0TextureNode._generateTangents;
            chkPixelLighting.Checked = ShaderGenerator.UsePixelLighting;
            chkContextLoop.Checked = BrawlLib.Properties.Settings.Default.ContextualLoopAnimation;

            UpdateOrb();
            UpdateLine();
            UpdateCol1();
            UpdateBGColor();
            UpdateStgBGColor();

            _updating = false;

            UpdateViewport(_form.ModelPanel.CurrentViewport);
        }

        private void Camera_OnPositionChanged()
        {
            if (_updating)
            {
                return;
            }

            if (!chkDefaultPos.Checked)
            {
                chkCurrentPos_CheckedChanged(null, null);
            }
        }

        public void UpdateViewport(GLViewport viewport)
        {
            if (viewport as ModelPanelViewport != current)
            {
                if (current != null)
                {
                    current.Camera.OnPositionChanged -= Camera_OnPositionChanged;
                }

                if (viewport != null)
                {
                    viewport.Camera.OnPositionChanged += Camera_OnPositionChanged;
                }
            }

            current = viewport as ModelPanelViewport;

            if (chkDefaultPos.Checked)
            {
                chkDefaultPos_CheckedChanged(null, null);
            }
            else
            {
                chkCurrentPos_CheckedChanged(null, null);
            }

            _updating = true;

            ax.Value = current.Ambient._x * 255.0f;
            ay.Value = current.Ambient._y * 255.0f;
            az.Value = current.Ambient._z * 255.0f;

            radius.Value = current.LightPosition._x;
            azimuth.Value = current.LightPosition._y;
            elevation.Value = current.LightPosition._z;

            dx.Value = current.Diffuse._x * 255.0f;
            dy.Value = current.Diffuse._y * 255.0f;
            dz.Value = current.Diffuse._z * 255.0f;

            sx.Value = current.Specular._x * 255.0f;
            sy.Value = current.Specular._y * 255.0f;
            sz.Value = current.Specular._z * 255.0f;

            ex.Value = current.Emission._x * 255.0f;
            ey.Value = current.Emission._y * 255.0f;
            ez.Value = current.Emission._z * 255.0f;

            tScale.Value = current.TranslationScale;
            rScale.Value = current.RotationScale;
            zScale.Value = current.ZoomScale;

            yFov.Value = current.Camera._fovY;
            nearZ.Value = current.Camera._nearZ;
            farZ.Value = current.Camera._farZ;

            chkLightEnabled.Checked = current.LightEnabled;
            chkLightDirectional.Checked = current.LightDirectional;
            chkScaleBones.Checked = current.ScaleBones;
            chkUsePointsAsBones.Checked = current.RenderBonesAsPoints;

            cboProjection.SelectedIndex = (int) current.ViewType;
            chkTextOverlays.Checked = current.TextOverlaysEnabled;

            for (int i = 0; i < 30; i++)
            {
                _origValues[i] = _boxes[i].Value;
                _boxes[i].Tag = i;
            }

            UpdateAmb();
            UpdateDif();
            UpdateSpe();
            UpdateEmi();

            _updating = false;
        }

        private void BoxValueChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _updating = true;

            _boxes[5].Value = _boxes[5].Value.RemapToRange(-180.0f, 180.0f);
            _boxes[6].Value = _boxes[6].Value.RemapToRange(-180.0f, 180.0f);

            current.Ambient = new Vector4(_boxes[0].Value / 255.0f, _boxes[1].Value / 255.0f, _boxes[2].Value / 255.0f,
                1.0f);
            current.LightPosition = new Vector4(_boxes[4].Value, _boxes[5].Value, _boxes[6].Value, 1.0f);
            current.Diffuse = new Vector4(_boxes[7].Value / 255.0f, _boxes[8].Value / 255.0f, _boxes[9].Value / 255.0f,
                1.0f);
            current.Specular = new Vector4(_boxes[11].Value / 255.0f, _boxes[12].Value / 255.0f,
                _boxes[13].Value / 255.0f, 1.0f);
            current.Emission = new Vector4(_boxes[3].Value / 255.0f, _boxes[10].Value / 255.0f,
                _boxes[14].Value / 255.0f, 1.0f);

            current.TranslationScale = _boxes[15].Value;
            current.RotationScale = _boxes[16].Value;
            current.ZoomScale = _boxes[17].Value;

            current.Camera._fovY = _boxes[18].Value;
            current.Camera._nearZ = _boxes[19].Value;
            current.Camera._farZ = _boxes[20].Value;
            current.Camera.CalculateProjection();

            _form.AllowedUndos = (uint) Math.Abs(_boxes[21].Value);

            int i = (int) (sender as NumericInputBox).Tag;

            if (i == 3 || i == 10 || i == 14)
            {
                UpdateEmi();
            }
            else if (i < 3)
            {
                UpdateAmb();
            }
            else if (i < 10)
            {
                UpdateDif();
            }
            else
            {
                UpdateSpe();
            }

            if (chkCurrentPos.Checked)
            {
                current.Camera.Set(
                    new Vector3(numPosTX.Value, numPosTY.Value, numPosTZ.Value),
                    new Vector3(numPosRX.Value, numPosRY.Value, numPosRZ.Value),
                    new Vector3(numPosSX.Value, numPosSY.Value, numPosSZ.Value));
            }
            else
            {
                current.Camera._defaultTranslate =
                    new Vector3(numPosTX.Value, numPosTY.Value, numPosTZ.Value);
                current.Camera._defaultRotate =
                    new Vector3(numPosRX.Value, numPosRY.Value, numPosRZ.Value);
                current.Camera._defaultScale =
                    new Vector3(numPosSX.Value, numPosSY.Value, numPosSZ.Value);
            }

            _form.ModelPanel.Invalidate();

            _updating = false;
        }

        private unsafe void btnOkay_Click(object sender, EventArgs e)
        {
            //if (Math.Abs(_boxes[5].Value) == Math.Abs(_boxes[6].Value) &&
            //    _boxes[5].Value % 180.0f == 0 &&
            //    _boxes[6].Value % 180.0f == 0)
            //{
            //    _boxes[5].Value = 0;
            //    _boxes[6].Value = 0;
            //}

            //current.Ambient = new Vector4(ax.Value / 255.0f, ay.Value / 255.0f, az.Value / 255.0f, 1.0f);
            //current.LightPosition = new Vector4(radius.Value, azimuth.Value, elevation.Value, 1.0f);
            //current.Diffuse = new Vector4(dx.Value / 255.0f, dy.Value / 255.0f, dz.Value / 255.0f, 1.0f);
            //current.Specular = new Vector4(sx.Value / 255.0f, sy.Value / 255.0f, sz.Value / 255.0f, 1.0f);
            //current.Emission = new Vector4(_boxes[3].Value / 255.0f, _boxes[10].Value / 255.0f, _boxes[14].Value / 255.0f, 1.0f);

            //current.TranslationScale = tScale.Value;
            //current.RotationScale = rScale.Value;
            //current.ZoomScale = zScale.Value;

            //current.Camera._fovY = yFov.Value;
            //current.Camera._nearZ = nearZ.Value;
            //current.Camera._farZ = farZ.Value;

            //_form.AllowedUndos = (uint)Math.Abs(maxUndoCount.Value);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //current.Ambient = new Vector4(_origValues[0] / 255.0f, _origValues[1] / 255.0f, _origValues[2] / 255.0f, 1.0f);
            //current.LightPosition = new Vector4(_origValues[4], _origValues[5], _origValues[6], 1.0f);
            //current.Diffuse = new Vector4(_origValues[7] / 255.0f, _origValues[8] / 255.0f, _origValues[9] / 255.0f, 1.0f);
            //current.Specular = new Vector4(_origValues[11] / 255.0f, _origValues[12] / 255.0f, _origValues[13] / 255.0f, 1.0f);
            //current.Emission = new Vector4(_origValues[3] / 255.0f, _origValues[10] / 255.0f, _origValues[14] / 255.0f, 1.0f);

            //current.TranslationScale = _origValues[15];
            //current.RotationScale = _origValues[16];
            //current.ZoomScale = _origValues[17];

            //current.Camera._fovY = _origValues[18];
            //current.Camera._nearZ = _origValues[19];
            //current.Camera._farZ = _origValues[20];

            //_form.AllowedUndos = (uint)Math.Abs(_origValues[21]);

            //ModelEditorBase._floorHue = _origFloor;
            //MDL0BoneNode.DefaultLineColor = _origBone;
            //MDL0BoneNode.DefaultNodeColor = _origNode;

            DialogResult = DialogResult.Cancel;
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            _form.RenderLightDisplay = false;
            _form.ModelPanel.OnCurrentViewportChanged -= UpdateViewport;
            _form.ModelPanel.Invalidate();
        }

        #region Designer

        private Button btnCancel;
        private Button btnOkay;

        private void InitializeComponent()
        {
            btnCancel = new Button();
            btnOkay = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            grpLighting = new GroupBox();
            chkLightDirectional = new CheckBox();
            chkLightEnabled = new CheckBox();
            label23 = new Label();
            label22 = new Label();
            label21 = new Label();
            label19 = new Label();
            ez = new NumericInputBox();
            ey = new NumericInputBox();
            label8 = new Label();
            ex = new NumericInputBox();
            label17 = new Label();
            label16 = new Label();
            sz = new NumericInputBox();
            dz = new NumericInputBox();
            radius = new NumericInputBox();
            az = new NumericInputBox();
            elevation = new NumericInputBox();
            sy = new NumericInputBox();
            azimuth = new NumericInputBox();
            dy = new NumericInputBox();
            ay = new NumericInputBox();
            sx = new NumericInputBox();
            dx = new NumericInputBox();
            ax = new NumericInputBox();
            grpProjection = new GroupBox();
            groupBox2 = new GroupBox();
            chkDefaultPos = new RadioButton();
            chkCurrentPos = new RadioButton();
            label26 = new Label();
            numPosTX = new NumericInputBox();
            label27 = new Label();
            numPosTY = new NumericInputBox();
            label28 = new Label();
            numPosTZ = new NumericInputBox();
            numPosSX = new NumericInputBox();
            numPosRZ = new NumericInputBox();
            numPosSY = new NumericInputBox();
            numPosRY = new NumericInputBox();
            numPosSZ = new NumericInputBox();
            numPosRX = new NumericInputBox();
            label25 = new Label();
            cboProjection = new ComboBox();
            farZ = new NumericInputBox();
            nearZ = new NumericInputBox();
            yFov = new NumericInputBox();
            zScale = new NumericInputBox();
            tScale = new NumericInputBox();
            rScale = new NumericInputBox();
            label14 = new Label();
            label13 = new Label();
            label12 = new Label();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            grpColors = new GroupBox();
            lblStgBGColor = new Label();
            lblBGColor = new Label();
            lblStgBGColorText = new Label();
            lblBGColorText = new Label();
            label33 = new Label();
            label34 = new Label();
            lblCol1Color = new Label();
            lblLineColor = new Label();
            lblCol1Text = new Label();
            lblLineText = new Label();
            label24 = new Label();
            label20 = new Label();
            lblOrbColor = new Label();
            lblOrbText = new Label();
            label15 = new Label();
            label18 = new Label();
            chkRetrieveCorrAnims = new CheckBox();
            chkSyncTexToObj = new CheckBox();
            chkSyncObjToVIS = new CheckBox();
            chkDisableBonesOnPlay = new CheckBox();
            chkDisableHighlight = new CheckBox();
            chkSnapBonesToFloor = new CheckBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            chkTextOverlays = new CheckBox();
            tabPage2 = new TabPage();
            chkPixelLighting = new CheckBox();
            chkHideMainWindow = new CheckBox();
            chkUsePointsAsBones = new CheckBox();
            chkScaleBones = new CheckBox();
            chkSaveWindowPosition = new CheckBox();
            chkMaximize = new CheckBox();
            maxUndoCount = new NumericInputBox();
            tabPage3 = new TabPage();
            chkContextLoop = new CheckBox();
            groupBox1 = new GroupBox();
            chkTanCam = new CheckBox();
            chkTanFog = new CheckBox();
            chkTanLight = new CheckBox();
            chkTanSHP = new CheckBox();
            chkTanSRT = new CheckBox();
            chkTanCHR = new CheckBox();
            chkPrecalcBoxes = new CheckBox();
            panel1 = new Panel();
            btnResetSettings = new Button();
            btnImportSettings = new Button();
            btnExportSettings = new Button();
            grpLighting.SuspendLayout();
            grpProjection.SuspendLayout();
            groupBox2.SuspendLayout();
            grpColors.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Right);
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Enabled = false;
            btnCancel.Location = new System.Drawing.Point(222, 6);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(60, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Visible = false;
            btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // btnOkay
            // 
            btnOkay.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Right);
            btnOkay.Location = new System.Drawing.Point(288, 6);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new System.Drawing.Size(60, 23);
            btnOkay.TabIndex = 1;
            btnOkay.Text = "&Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new EventHandler(btnOkay_Click);
            // 
            // label1
            // 
            label1.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Location = new System.Drawing.Point(33, 81);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(56, 20);
            label1.TabIndex = 7;
            label1.Text = "Ambient:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Location = new System.Drawing.Point(26, 18);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(66, 20);
            label2.TabIndex = 8;
            label2.Text = "Radius";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            label3.BorderStyle = BorderStyle.FixedSingle;
            label3.Location = new System.Drawing.Point(33, 100);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(56, 20);
            label3.TabIndex = 9;
            label3.Text = "Diffuse:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            label4.BorderStyle = BorderStyle.FixedSingle;
            label4.Location = new System.Drawing.Point(33, 119);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(56, 20);
            label4.TabIndex = 10;
            label4.Text = "Specular:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            label5.BorderStyle = BorderStyle.FixedSingle;
            label5.Location = new System.Drawing.Point(88, 62);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(50, 20);
            label5.TabIndex = 19;
            label5.Text = "R";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            label6.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            label6.BorderStyle = BorderStyle.FixedSingle;
            label6.Location = new System.Drawing.Point(137, 62);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(50, 20);
            label6.TabIndex = 20;
            label6.Text = "G";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            label7.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            label7.BorderStyle = BorderStyle.FixedSingle;
            label7.Location = new System.Drawing.Point(186, 62);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(50, 20);
            label7.TabIndex = 21;
            label7.Text = "B";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpLighting
            // 
            grpLighting.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                  | AnchorStyles.Right);
            grpLighting.Controls.Add(chkLightDirectional);
            grpLighting.Controls.Add(chkLightEnabled);
            grpLighting.Controls.Add(label23);
            grpLighting.Controls.Add(label22);
            grpLighting.Controls.Add(label21);
            grpLighting.Controls.Add(label19);
            grpLighting.Controls.Add(ez);
            grpLighting.Controls.Add(ey);
            grpLighting.Controls.Add(label8);
            grpLighting.Controls.Add(ex);
            grpLighting.Controls.Add(label17);
            grpLighting.Controls.Add(label16);
            grpLighting.Controls.Add(sz);
            grpLighting.Controls.Add(dz);
            grpLighting.Controls.Add(label2);
            grpLighting.Controls.Add(radius);
            grpLighting.Controls.Add(az);
            grpLighting.Controls.Add(elevation);
            grpLighting.Controls.Add(sy);
            grpLighting.Controls.Add(azimuth);
            grpLighting.Controls.Add(dy);
            grpLighting.Controls.Add(ay);
            grpLighting.Controls.Add(label7);
            grpLighting.Controls.Add(label6);
            grpLighting.Controls.Add(label5);
            grpLighting.Controls.Add(label4);
            grpLighting.Controls.Add(label3);
            grpLighting.Controls.Add(label1);
            grpLighting.Controls.Add(sx);
            grpLighting.Controls.Add(dx);
            grpLighting.Controls.Add(ax);
            grpLighting.Location = new System.Drawing.Point(6, 195);
            grpLighting.Name = "grpLighting";
            grpLighting.Size = new System.Drawing.Size(330, 167);
            grpLighting.TabIndex = 35;
            grpLighting.TabStop = false;
            grpLighting.Text = "Lighting";
            // 
            // chkLightDirectional
            // 
            chkLightDirectional.AutoSize = true;
            chkLightDirectional.Checked = true;
            chkLightDirectional.CheckState = CheckState.Checked;
            chkLightDirectional.Location = new System.Drawing.Point(245, 38);
            chkLightDirectional.Name = "chkLightDirectional";
            chkLightDirectional.Size = new System.Drawing.Size(76, 17);
            chkLightDirectional.TabIndex = 45;
            chkLightDirectional.Text = "Directional";
            chkLightDirectional.UseVisualStyleBackColor = true;
            chkLightDirectional.Visible = false;
            chkLightDirectional.CheckedChanged += new EventHandler(chkLightDirectional_CheckedChanged);
            // 
            // chkLightEnabled
            // 
            chkLightEnabled.AutoSize = true;
            chkLightEnabled.Location = new System.Drawing.Point(245, 21);
            chkLightEnabled.Name = "chkLightEnabled";
            chkLightEnabled.Size = new System.Drawing.Size(65, 17);
            chkLightEnabled.TabIndex = 44;
            chkLightEnabled.Text = "Enabled";
            chkLightEnabled.UseVisualStyleBackColor = true;
            chkLightEnabled.CheckedChanged += new EventHandler(chkLightEnabled_CheckedChanged);
            // 
            // label23
            // 
            label23.BorderStyle = BorderStyle.FixedSingle;
            label23.Location = new System.Drawing.Point(235, 138);
            label23.Name = "label23";
            label23.Size = new System.Drawing.Size(40, 20);
            label23.TabIndex = 43;
            label23.Click += new EventHandler(label23_Click);
            // 
            // label22
            // 
            label22.BorderStyle = BorderStyle.FixedSingle;
            label22.Location = new System.Drawing.Point(235, 119);
            label22.Name = "label22";
            label22.Size = new System.Drawing.Size(40, 20);
            label22.TabIndex = 42;
            label22.Click += new EventHandler(label22_Click);
            // 
            // label21
            // 
            label21.BorderStyle = BorderStyle.FixedSingle;
            label21.Location = new System.Drawing.Point(235, 100);
            label21.Name = "label21";
            label21.Size = new System.Drawing.Size(40, 20);
            label21.TabIndex = 41;
            label21.Click += new EventHandler(label21_Click);
            // 
            // label19
            // 
            label19.BorderStyle = BorderStyle.FixedSingle;
            label19.Location = new System.Drawing.Point(235, 81);
            label19.Name = "label19";
            label19.Size = new System.Drawing.Size(40, 20);
            label19.TabIndex = 11;
            label19.Click += new EventHandler(label19_Click);
            // 
            // ez
            // 
            ez.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            ez.BorderStyle = BorderStyle.FixedSingle;
            ez.Integer = false;
            ez.Integral = false;
            ez.Location = new System.Drawing.Point(186, 138);
            ez.MaximumValue = 3.402823E+38F;
            ez.MinimumValue = -3.402823E+38F;
            ez.Name = "ez";
            ez.Size = new System.Drawing.Size(50, 20);
            ez.TabIndex = 40;
            ez.Text = "0";
            // 
            // ey
            // 
            ey.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            ey.BorderStyle = BorderStyle.FixedSingle;
            ey.Integer = false;
            ey.Integral = false;
            ey.Location = new System.Drawing.Point(137, 138);
            ey.MaximumValue = 3.402823E+38F;
            ey.MinimumValue = -3.402823E+38F;
            ey.Name = "ey";
            ey.Size = new System.Drawing.Size(50, 20);
            ey.TabIndex = 39;
            ey.Text = "0";
            // 
            // label8
            // 
            label8.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            label8.BorderStyle = BorderStyle.FixedSingle;
            label8.Location = new System.Drawing.Point(33, 138);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(56, 20);
            label8.TabIndex = 38;
            label8.Text = "Emission:";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ex
            // 
            ex.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            ex.BorderStyle = BorderStyle.FixedSingle;
            ex.Integer = false;
            ex.Integral = false;
            ex.Location = new System.Drawing.Point(88, 138);
            ex.MaximumValue = 3.402823E+38F;
            ex.MinimumValue = -3.402823E+38F;
            ex.Name = "ex";
            ex.Size = new System.Drawing.Size(50, 20);
            ex.TabIndex = 37;
            ex.Text = "0";
            // 
            // label17
            // 
            label17.BorderStyle = BorderStyle.FixedSingle;
            label17.Location = new System.Drawing.Point(163, 18);
            label17.Name = "label17";
            label17.Size = new System.Drawing.Size(66, 20);
            label17.TabIndex = 36;
            label17.Text = "Elevation";
            label17.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            label16.BorderStyle = BorderStyle.FixedSingle;
            label16.Location = new System.Drawing.Point(95, 18);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(66, 20);
            label16.TabIndex = 35;
            label16.Text = "Azimuth";
            label16.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // sz
            // 
            sz.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            sz.BorderStyle = BorderStyle.FixedSingle;
            sz.Integer = false;
            sz.Integral = false;
            sz.Location = new System.Drawing.Point(186, 119);
            sz.MaximumValue = 3.402823E+38F;
            sz.MinimumValue = -3.402823E+38F;
            sz.Name = "sz";
            sz.Size = new System.Drawing.Size(50, 20);
            sz.TabIndex = 30;
            sz.Text = "0";
            // 
            // dz
            // 
            dz.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            dz.BorderStyle = BorderStyle.FixedSingle;
            dz.Integer = false;
            dz.Integral = false;
            dz.Location = new System.Drawing.Point(186, 100);
            dz.MaximumValue = 3.402823E+38F;
            dz.MinimumValue = -3.402823E+38F;
            dz.Name = "dz";
            dz.Size = new System.Drawing.Size(50, 20);
            dz.TabIndex = 29;
            dz.Text = "0";
            // 
            // radius
            // 
            radius.BorderStyle = BorderStyle.FixedSingle;
            radius.Integer = false;
            radius.Integral = false;
            radius.Location = new System.Drawing.Point(26, 37);
            radius.MaximumValue = 3.402823E+38F;
            radius.MinimumValue = -3.402823E+38F;
            radius.Name = "radius";
            radius.Size = new System.Drawing.Size(66, 20);
            radius.TabIndex = 4;
            radius.Text = "0";
            // 
            // az
            // 
            az.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            az.BorderStyle = BorderStyle.FixedSingle;
            az.Integer = false;
            az.Integral = false;
            az.Location = new System.Drawing.Point(186, 81);
            az.MaximumValue = 3.402823E+38F;
            az.MinimumValue = -3.402823E+38F;
            az.Name = "az";
            az.Size = new System.Drawing.Size(50, 20);
            az.TabIndex = 27;
            az.Text = "0";
            // 
            // elevation
            // 
            elevation.BorderStyle = BorderStyle.FixedSingle;
            elevation.Integer = false;
            elevation.Integral = false;
            elevation.Location = new System.Drawing.Point(163, 37);
            elevation.MaximumValue = 3.402823E+38F;
            elevation.MinimumValue = -3.402823E+38F;
            elevation.Name = "elevation";
            elevation.Size = new System.Drawing.Size(66, 20);
            elevation.TabIndex = 28;
            elevation.Text = "0";
            // 
            // sy
            // 
            sy.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            sy.BorderStyle = BorderStyle.FixedSingle;
            sy.Integer = false;
            sy.Integral = false;
            sy.Location = new System.Drawing.Point(137, 119);
            sy.MaximumValue = 3.402823E+38F;
            sy.MinimumValue = -3.402823E+38F;
            sy.Name = "sy";
            sy.Size = new System.Drawing.Size(50, 20);
            sy.TabIndex = 26;
            sy.Text = "0";
            // 
            // azimuth
            // 
            azimuth.BorderStyle = BorderStyle.FixedSingle;
            azimuth.Integer = false;
            azimuth.Integral = false;
            azimuth.Location = new System.Drawing.Point(95, 37);
            azimuth.MaximumValue = 3.402823E+38F;
            azimuth.MinimumValue = -3.402823E+38F;
            azimuth.Name = "azimuth";
            azimuth.Size = new System.Drawing.Size(66, 20);
            azimuth.TabIndex = 24;
            azimuth.Text = "0";
            // 
            // dy
            // 
            dy.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            dy.BorderStyle = BorderStyle.FixedSingle;
            dy.Integer = false;
            dy.Integral = false;
            dy.Location = new System.Drawing.Point(137, 100);
            dy.MaximumValue = 3.402823E+38F;
            dy.MinimumValue = -3.402823E+38F;
            dy.Name = "dy";
            dy.Size = new System.Drawing.Size(50, 20);
            dy.TabIndex = 25;
            dy.Text = "0";
            // 
            // ay
            // 
            ay.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            ay.BorderStyle = BorderStyle.FixedSingle;
            ay.Integer = false;
            ay.Integral = false;
            ay.Location = new System.Drawing.Point(137, 81);
            ay.MaximumValue = 3.402823E+38F;
            ay.MinimumValue = -3.402823E+38F;
            ay.Name = "ay";
            ay.Size = new System.Drawing.Size(50, 20);
            ay.TabIndex = 23;
            ay.Text = "0";
            // 
            // sx
            // 
            sx.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            sx.BorderStyle = BorderStyle.FixedSingle;
            sx.Integer = false;
            sx.Integral = false;
            sx.Location = new System.Drawing.Point(88, 119);
            sx.MaximumValue = 3.402823E+38F;
            sx.MinimumValue = -3.402823E+38F;
            sx.Name = "sx";
            sx.Size = new System.Drawing.Size(50, 20);
            sx.TabIndex = 6;
            sx.Text = "0";
            // 
            // dx
            // 
            dx.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            dx.BorderStyle = BorderStyle.FixedSingle;
            dx.Integer = false;
            dx.Integral = false;
            dx.Location = new System.Drawing.Point(88, 100);
            dx.MaximumValue = 3.402823E+38F;
            dx.MinimumValue = -3.402823E+38F;
            dx.Name = "dx";
            dx.Size = new System.Drawing.Size(50, 20);
            dx.TabIndex = 5;
            dx.Text = "0";
            // 
            // ax
            // 
            ax.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            ax.BorderStyle = BorderStyle.FixedSingle;
            ax.Integer = false;
            ax.Integral = false;
            ax.Location = new System.Drawing.Point(88, 81);
            ax.MaximumValue = 3.402823E+38F;
            ax.MinimumValue = -3.402823E+38F;
            ax.Name = "ax";
            ax.Size = new System.Drawing.Size(50, 20);
            ax.TabIndex = 3;
            ax.Text = "0";
            // 
            // grpProjection
            // 
            grpProjection.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                    | AnchorStyles.Right);
            grpProjection.Controls.Add(groupBox2);
            grpProjection.Controls.Add(label25);
            grpProjection.Controls.Add(cboProjection);
            grpProjection.Controls.Add(farZ);
            grpProjection.Controls.Add(nearZ);
            grpProjection.Controls.Add(yFov);
            grpProjection.Controls.Add(zScale);
            grpProjection.Controls.Add(tScale);
            grpProjection.Controls.Add(rScale);
            grpProjection.Controls.Add(label14);
            grpProjection.Controls.Add(label13);
            grpProjection.Controls.Add(label12);
            grpProjection.Controls.Add(label11);
            grpProjection.Controls.Add(label10);
            grpProjection.Controls.Add(label9);
            grpProjection.Location = new System.Drawing.Point(6, 6);
            grpProjection.Name = "grpProjection";
            grpProjection.Size = new System.Drawing.Size(330, 187);
            grpProjection.TabIndex = 36;
            grpProjection.TabStop = false;
            grpProjection.Text = "Camera";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                | AnchorStyles.Right);
            groupBox2.Controls.Add(chkDefaultPos);
            groupBox2.Controls.Add(chkCurrentPos);
            groupBox2.Controls.Add(label26);
            groupBox2.Controls.Add(numPosTX);
            groupBox2.Controls.Add(label27);
            groupBox2.Controls.Add(numPosTY);
            groupBox2.Controls.Add(label28);
            groupBox2.Controls.Add(numPosTZ);
            groupBox2.Controls.Add(numPosSX);
            groupBox2.Controls.Add(numPosRZ);
            groupBox2.Controls.Add(numPosSY);
            groupBox2.Controls.Add(numPosRY);
            groupBox2.Controls.Add(numPosSZ);
            groupBox2.Controls.Add(numPosRX);
            groupBox2.Location = new System.Drawing.Point(6, 105);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(317, 76);
            groupBox2.TabIndex = 26;
            groupBox2.TabStop = false;
            groupBox2.Text = "Position";
            // 
            // chkDefaultPos
            // 
            chkDefaultPos.AutoSize = true;
            chkDefaultPos.Location = new System.Drawing.Point(239, 44);
            chkDefaultPos.Name = "chkDefaultPos";
            chkDefaultPos.Size = new System.Drawing.Size(59, 17);
            chkDefaultPos.TabIndex = 27;
            chkDefaultPos.Text = "Default";
            chkDefaultPos.UseVisualStyleBackColor = true;
            chkDefaultPos.CheckedChanged += new EventHandler(chkDefaultPos_CheckedChanged);
            // 
            // chkCurrentPos
            // 
            chkCurrentPos.AutoSize = true;
            chkCurrentPos.Checked = true;
            chkCurrentPos.Location = new System.Drawing.Point(239, 21);
            chkCurrentPos.Name = "chkCurrentPos";
            chkCurrentPos.Size = new System.Drawing.Size(59, 17);
            chkCurrentPos.TabIndex = 26;
            chkCurrentPos.TabStop = true;
            chkCurrentPos.Text = "Current";
            chkCurrentPos.UseVisualStyleBackColor = true;
            chkCurrentPos.CheckedChanged += new EventHandler(chkCurrentPos_CheckedChanged);
            // 
            // label26
            // 
            label26.BorderStyle = BorderStyle.FixedSingle;
            label26.Location = new System.Drawing.Point(6, 13);
            label26.Name = "label26";
            label26.Size = new System.Drawing.Size(71, 20);
            label26.TabIndex = 14;
            label26.Text = "Scale: ";
            label26.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numPosTX
            // 
            numPosTX.BorderStyle = BorderStyle.FixedSingle;
            numPosTX.Integer = false;
            numPosTX.Integral = false;
            numPosTX.Location = new System.Drawing.Point(76, 51);
            numPosTX.MaximumValue = 3.402823E+38F;
            numPosTX.MinimumValue = -3.402823E+38F;
            numPosTX.Name = "numPosTX";
            numPosTX.Size = new System.Drawing.Size(50, 20);
            numPosTX.TabIndex = 25;
            numPosTX.Text = "0";
            // 
            // label27
            // 
            label27.BorderStyle = BorderStyle.FixedSingle;
            label27.Location = new System.Drawing.Point(6, 32);
            label27.Name = "label27";
            label27.Size = new System.Drawing.Size(71, 20);
            label27.TabIndex = 15;
            label27.Text = "Rotation:";
            label27.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numPosTY
            // 
            numPosTY.BorderStyle = BorderStyle.FixedSingle;
            numPosTY.Integer = false;
            numPosTY.Integral = false;
            numPosTY.Location = new System.Drawing.Point(125, 51);
            numPosTY.MaximumValue = 3.402823E+38F;
            numPosTY.MinimumValue = -3.402823E+38F;
            numPosTY.Name = "numPosTY";
            numPosTY.Size = new System.Drawing.Size(50, 20);
            numPosTY.TabIndex = 24;
            numPosTY.Text = "0";
            // 
            // label28
            // 
            label28.BorderStyle = BorderStyle.FixedSingle;
            label28.Location = new System.Drawing.Point(6, 51);
            label28.Name = "label28";
            label28.Size = new System.Drawing.Size(71, 20);
            label28.TabIndex = 16;
            label28.Text = "Translation:";
            label28.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numPosTZ
            // 
            numPosTZ.BorderStyle = BorderStyle.FixedSingle;
            numPosTZ.Integer = false;
            numPosTZ.Integral = false;
            numPosTZ.Location = new System.Drawing.Point(174, 51);
            numPosTZ.MaximumValue = 3.402823E+38F;
            numPosTZ.MinimumValue = -3.402823E+38F;
            numPosTZ.Name = "numPosTZ";
            numPosTZ.Size = new System.Drawing.Size(50, 20);
            numPosTZ.TabIndex = 23;
            numPosTZ.Text = "0";
            // 
            // numPosSX
            // 
            numPosSX.BorderStyle = BorderStyle.FixedSingle;
            numPosSX.Integer = false;
            numPosSX.Integral = false;
            numPosSX.Location = new System.Drawing.Point(76, 13);
            numPosSX.MaximumValue = 3.402823E+38F;
            numPosSX.MinimumValue = -3.402823E+38F;
            numPosSX.Name = "numPosSX";
            numPosSX.Size = new System.Drawing.Size(50, 20);
            numPosSX.TabIndex = 17;
            numPosSX.Text = "0";
            // 
            // numPosRZ
            // 
            numPosRZ.BorderStyle = BorderStyle.FixedSingle;
            numPosRZ.Integer = false;
            numPosRZ.Integral = false;
            numPosRZ.Location = new System.Drawing.Point(174, 32);
            numPosRZ.MaximumValue = 3.402823E+38F;
            numPosRZ.MinimumValue = -3.402823E+38F;
            numPosRZ.Name = "numPosRZ";
            numPosRZ.Size = new System.Drawing.Size(50, 20);
            numPosRZ.TabIndex = 22;
            numPosRZ.Text = "0";
            // 
            // numPosSY
            // 
            numPosSY.BorderStyle = BorderStyle.FixedSingle;
            numPosSY.Integer = false;
            numPosSY.Integral = false;
            numPosSY.Location = new System.Drawing.Point(125, 13);
            numPosSY.MaximumValue = 3.402823E+38F;
            numPosSY.MinimumValue = -3.402823E+38F;
            numPosSY.Name = "numPosSY";
            numPosSY.Size = new System.Drawing.Size(50, 20);
            numPosSY.TabIndex = 18;
            numPosSY.Text = "0";
            // 
            // numPosRY
            // 
            numPosRY.BorderStyle = BorderStyle.FixedSingle;
            numPosRY.Integer = false;
            numPosRY.Integral = false;
            numPosRY.Location = new System.Drawing.Point(125, 32);
            numPosRY.MaximumValue = 3.402823E+38F;
            numPosRY.MinimumValue = -3.402823E+38F;
            numPosRY.Name = "numPosRY";
            numPosRY.Size = new System.Drawing.Size(50, 20);
            numPosRY.TabIndex = 21;
            numPosRY.Text = "0";
            // 
            // numPosSZ
            // 
            numPosSZ.BorderStyle = BorderStyle.FixedSingle;
            numPosSZ.Integer = false;
            numPosSZ.Integral = false;
            numPosSZ.Location = new System.Drawing.Point(174, 13);
            numPosSZ.MaximumValue = 3.402823E+38F;
            numPosSZ.MinimumValue = -3.402823E+38F;
            numPosSZ.Name = "numPosSZ";
            numPosSZ.Size = new System.Drawing.Size(50, 20);
            numPosSZ.TabIndex = 19;
            numPosSZ.Text = "0";
            // 
            // numPosRX
            // 
            numPosRX.BorderStyle = BorderStyle.FixedSingle;
            numPosRX.Integer = false;
            numPosRX.Integral = false;
            numPosRX.Location = new System.Drawing.Point(76, 32);
            numPosRX.MaximumValue = 3.402823E+38F;
            numPosRX.MinimumValue = -3.402823E+38F;
            numPosRX.Name = "numPosRX";
            numPosRX.Size = new System.Drawing.Size(50, 20);
            numPosRX.TabIndex = 20;
            numPosRX.Text = "0";
            // 
            // label25
            // 
            label25.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                              | AnchorStyles.Right);
            label25.BorderStyle = BorderStyle.FixedSingle;
            label25.Location = new System.Drawing.Point(6, 21);
            label25.Name = "label25";
            label25.Size = new System.Drawing.Size(120, 21);
            label25.TabIndex = 13;
            label25.Text = "Projection:";
            label25.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cboProjection
            // 
            cboProjection.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            cboProjection.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProjection.FormattingEnabled = true;
            cboProjection.Location = new System.Drawing.Point(125, 21);
            cboProjection.Name = "cboProjection";
            cboProjection.Size = new System.Drawing.Size(198, 21);
            cboProjection.TabIndex = 12;
            cboProjection.SelectedIndexChanged += new EventHandler(cboProjection_SelectedIndexChanged);
            // 
            // farZ
            // 
            farZ.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            farZ.BorderStyle = BorderStyle.FixedSingle;
            farZ.Integer = false;
            farZ.Integral = false;
            farZ.Location = new System.Drawing.Point(263, 79);
            farZ.MaximumValue = 3.402823E+38F;
            farZ.MinimumValue = -3.402823E+38F;
            farZ.Name = "farZ";
            farZ.Size = new System.Drawing.Size(60, 20);
            farZ.TabIndex = 11;
            farZ.Text = "0";
            // 
            // nearZ
            // 
            nearZ.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            nearZ.BorderStyle = BorderStyle.FixedSingle;
            nearZ.Integer = false;
            nearZ.Integral = false;
            nearZ.Location = new System.Drawing.Point(263, 60);
            nearZ.MaximumValue = 3.402823E+38F;
            nearZ.MinimumValue = -3.402823E+38F;
            nearZ.Name = "nearZ";
            nearZ.Size = new System.Drawing.Size(60, 20);
            nearZ.TabIndex = 10;
            nearZ.Text = "0";
            // 
            // yFov
            // 
            yFov.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            yFov.BorderStyle = BorderStyle.FixedSingle;
            yFov.Integer = false;
            yFov.Integral = false;
            yFov.Location = new System.Drawing.Point(263, 41);
            yFov.MaximumValue = 3.402823E+38F;
            yFov.MinimumValue = -3.402823E+38F;
            yFov.Name = "yFov";
            yFov.Size = new System.Drawing.Size(60, 20);
            yFov.TabIndex = 9;
            yFov.Text = "0";
            // 
            // zScale
            // 
            zScale.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            zScale.BorderStyle = BorderStyle.FixedSingle;
            zScale.Integer = false;
            zScale.Integral = false;
            zScale.Location = new System.Drawing.Point(125, 79);
            zScale.MaximumValue = 3.402823E+38F;
            zScale.MinimumValue = -3.402823E+38F;
            zScale.Name = "zScale";
            zScale.Size = new System.Drawing.Size(50, 20);
            zScale.TabIndex = 8;
            zScale.Text = "0";
            // 
            // tScale
            // 
            tScale.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            tScale.BorderStyle = BorderStyle.FixedSingle;
            tScale.Integer = false;
            tScale.Integral = false;
            tScale.Location = new System.Drawing.Point(125, 60);
            tScale.MaximumValue = 3.402823E+38F;
            tScale.MinimumValue = -3.402823E+38F;
            tScale.Name = "tScale";
            tScale.Size = new System.Drawing.Size(50, 20);
            tScale.TabIndex = 7;
            tScale.Text = "0";
            // 
            // rScale
            // 
            rScale.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            rScale.BorderStyle = BorderStyle.FixedSingle;
            rScale.Integer = false;
            rScale.Integral = false;
            rScale.Location = new System.Drawing.Point(125, 41);
            rScale.MaximumValue = 3.402823E+38F;
            rScale.MinimumValue = -3.402823E+38F;
            rScale.Name = "rScale";
            rScale.Size = new System.Drawing.Size(50, 20);
            rScale.TabIndex = 6;
            rScale.Text = "0";
            // 
            // label14
            // 
            label14.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            label14.BorderStyle = BorderStyle.FixedSingle;
            label14.Location = new System.Drawing.Point(174, 79);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(90, 20);
            label14.TabIndex = 5;
            label14.Text = "Far Z: ";
            label14.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            label13.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            label13.BorderStyle = BorderStyle.FixedSingle;
            label13.Location = new System.Drawing.Point(174, 60);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(90, 20);
            label13.TabIndex = 4;
            label13.Text = "Near Z: ";
            label13.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            label12.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            label12.BorderStyle = BorderStyle.FixedSingle;
            label12.Location = new System.Drawing.Point(174, 41);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(90, 20);
            label12.TabIndex = 3;
            label12.Text = "Y Field Of View: ";
            label12.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            label11.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                              | AnchorStyles.Right);
            label11.BorderStyle = BorderStyle.FixedSingle;
            label11.Location = new System.Drawing.Point(6, 79);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(120, 20);
            label11.TabIndex = 2;
            label11.Text = "Zoom Scale: ";
            label11.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            label10.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                              | AnchorStyles.Right);
            label10.BorderStyle = BorderStyle.FixedSingle;
            label10.Location = new System.Drawing.Point(6, 60);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(120, 20);
            label10.TabIndex = 1;
            label10.Text = "Translation Scale: ";
            label10.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            label9.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                             | AnchorStyles.Right);
            label9.BorderStyle = BorderStyle.FixedSingle;
            label9.Location = new System.Drawing.Point(6, 41);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(120, 20);
            label9.TabIndex = 0;
            label9.Text = "Rotation Scale: ";
            label9.TextAlign = ContentAlignment.MiddleRight;
            // 
            // grpColors
            // 
            grpColors.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                | AnchorStyles.Right);
            grpColors.Controls.Add(lblStgBGColor);
            grpColors.Controls.Add(lblBGColor);
            grpColors.Controls.Add(lblStgBGColorText);
            grpColors.Controls.Add(lblBGColorText);
            grpColors.Controls.Add(label33);
            grpColors.Controls.Add(label34);
            grpColors.Controls.Add(lblCol1Color);
            grpColors.Controls.Add(lblLineColor);
            grpColors.Controls.Add(lblCol1Text);
            grpColors.Controls.Add(lblLineText);
            grpColors.Controls.Add(label24);
            grpColors.Controls.Add(label20);
            grpColors.Controls.Add(lblOrbColor);
            grpColors.Controls.Add(lblOrbText);
            grpColors.Controls.Add(label15);
            grpColors.Location = new System.Drawing.Point(6, 6);
            grpColors.Name = "grpColors";
            grpColors.Size = new System.Drawing.Size(331, 119);
            grpColors.TabIndex = 37;
            grpColors.TabStop = false;
            grpColors.Text = "Display Colors";
            // 
            // lblStgBGColor
            // 
            lblStgBGColor.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            lblStgBGColor.BorderStyle = BorderStyle.FixedSingle;
            lblStgBGColor.Location = new System.Drawing.Point(283, 92);
            lblStgBGColor.Name = "lblStgBGColor";
            lblStgBGColor.Size = new System.Drawing.Size(40, 20);
            lblStgBGColor.TabIndex = 11;
            lblStgBGColor.Click += new EventHandler(lblStgBGColor_Click);
            // 
            // lblBGColor
            // 
            lblBGColor.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            lblBGColor.BorderStyle = BorderStyle.FixedSingle;
            lblBGColor.Location = new System.Drawing.Point(283, 73);
            lblBGColor.Name = "lblBGColor";
            lblBGColor.Size = new System.Drawing.Size(40, 20);
            lblBGColor.TabIndex = 14;
            lblBGColor.Click += new EventHandler(lblBGColor_Click);
            // 
            // lblStgBGColorText
            // 
            lblStgBGColorText.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                        | AnchorStyles.Right);
            lblStgBGColorText.BackColor = Color.White;
            lblStgBGColorText.BorderStyle = BorderStyle.FixedSingle;
            lblStgBGColorText.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
            lblStgBGColorText.Location = new System.Drawing.Point(105, 92);
            lblStgBGColorText.Name = "lblStgBGColorText";
            lblStgBGColorText.Size = new System.Drawing.Size(179, 20);
            lblStgBGColorText.TabIndex = 13;
            lblStgBGColorText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblBGColorText
            // 
            lblBGColorText.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                     | AnchorStyles.Right);
            lblBGColorText.BackColor = Color.White;
            lblBGColorText.BorderStyle = BorderStyle.FixedSingle;
            lblBGColorText.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
            lblBGColorText.Location = new System.Drawing.Point(105, 73);
            lblBGColorText.Name = "lblBGColorText";
            lblBGColorText.Size = new System.Drawing.Size(179, 20);
            lblBGColorText.TabIndex = 16;
            lblBGColorText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label33
            // 
            label33.BorderStyle = BorderStyle.FixedSingle;
            label33.Location = new System.Drawing.Point(6, 92);
            label33.Name = "label33";
            label33.Size = new System.Drawing.Size(100, 20);
            label33.TabIndex = 12;
            label33.Text = "Stage BG Color:";
            label33.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label34
            // 
            label34.BorderStyle = BorderStyle.FixedSingle;
            label34.Location = new System.Drawing.Point(6, 73);
            label34.Name = "label34";
            label34.Size = new System.Drawing.Size(100, 20);
            label34.TabIndex = 15;
            label34.Text = "Default BG Color:";
            label34.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCol1Color
            // 
            lblCol1Color.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            lblCol1Color.BorderStyle = BorderStyle.FixedSingle;
            lblCol1Color.Location = new System.Drawing.Point(283, 54);
            lblCol1Color.Name = "lblCol1Color";
            lblCol1Color.Size = new System.Drawing.Size(40, 20);
            lblCol1Color.TabIndex = 5;
            lblCol1Color.Click += new EventHandler(lblCol1Color_Click);
            // 
            // lblLineColor
            // 
            lblLineColor.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            lblLineColor.BorderStyle = BorderStyle.FixedSingle;
            lblLineColor.Location = new System.Drawing.Point(283, 35);
            lblLineColor.Name = "lblLineColor";
            lblLineColor.Size = new System.Drawing.Size(40, 20);
            lblLineColor.TabIndex = 8;
            lblLineColor.Click += new EventHandler(lblLineColor_Click);
            // 
            // lblCol1Text
            // 
            lblCol1Text.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                  | AnchorStyles.Right);
            lblCol1Text.BackColor = Color.White;
            lblCol1Text.BorderStyle = BorderStyle.FixedSingle;
            lblCol1Text.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
            lblCol1Text.Location = new System.Drawing.Point(105, 54);
            lblCol1Text.Name = "lblCol1Text";
            lblCol1Text.Size = new System.Drawing.Size(179, 20);
            lblCol1Text.TabIndex = 7;
            lblCol1Text.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblLineText
            // 
            lblLineText.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                  | AnchorStyles.Right);
            lblLineText.BackColor = Color.White;
            lblLineText.BorderStyle = BorderStyle.FixedSingle;
            lblLineText.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
            lblLineText.Location = new System.Drawing.Point(105, 35);
            lblLineText.Name = "lblLineText";
            lblLineText.Size = new System.Drawing.Size(179, 20);
            lblLineText.TabIndex = 10;
            lblLineText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label24
            // 
            label24.BorderStyle = BorderStyle.FixedSingle;
            label24.Location = new System.Drawing.Point(6, 54);
            label24.Name = "label24";
            label24.Size = new System.Drawing.Size(100, 20);
            label24.TabIndex = 6;
            label24.Text = "Floor Color:";
            label24.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label20
            // 
            label20.BorderStyle = BorderStyle.FixedSingle;
            label20.Location = new System.Drawing.Point(6, 35);
            label20.Name = "label20";
            label20.Size = new System.Drawing.Size(100, 20);
            label20.TabIndex = 9;
            label20.Text = "Bone Line Color:";
            label20.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblOrbColor
            // 
            lblOrbColor.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            lblOrbColor.BorderStyle = BorderStyle.FixedSingle;
            lblOrbColor.Location = new System.Drawing.Point(283, 16);
            lblOrbColor.Name = "lblOrbColor";
            lblOrbColor.Size = new System.Drawing.Size(40, 20);
            lblOrbColor.TabIndex = 5;
            lblOrbColor.Click += new EventHandler(lblOrbColor_Click);
            // 
            // lblOrbText
            // 
            lblOrbText.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                 | AnchorStyles.Right);
            lblOrbText.BackColor = Color.White;
            lblOrbText.BorderStyle = BorderStyle.FixedSingle;
            lblOrbText.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
            lblOrbText.Location = new System.Drawing.Point(105, 16);
            lblOrbText.Name = "lblOrbText";
            lblOrbText.Size = new System.Drawing.Size(179, 20);
            lblOrbText.TabIndex = 7;
            lblOrbText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            label15.BorderStyle = BorderStyle.FixedSingle;
            label15.Location = new System.Drawing.Point(6, 16);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(100, 20);
            label15.TabIndex = 6;
            label15.Text = "Bone Orb Color:";
            label15.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            label18.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            label18.AutoSize = true;
            label18.Location = new System.Drawing.Point(4, 367);
            label18.Name = "label18";
            label18.Size = new System.Drawing.Size(145, 13);
            label18.TabIndex = 39;
            label18.Text = "Maximum Undo/Redo Count:";
            // 
            // chkRetrieveCorrAnims
            // 
            chkRetrieveCorrAnims.AutoSize = true;
            chkRetrieveCorrAnims.Location = new System.Drawing.Point(6, 99);
            chkRetrieveCorrAnims.Name = "chkRetrieveCorrAnims";
            chkRetrieveCorrAnims.Size = new System.Drawing.Size(240, 17);
            chkRetrieveCorrAnims.TabIndex = 40;
            chkRetrieveCorrAnims.Text = "Retrieve animations with corresponding name";
            chkRetrieveCorrAnims.UseVisualStyleBackColor = true;
            chkRetrieveCorrAnims.CheckedChanged += new EventHandler(chkRetrieveCorrAnims_CheckedChanged);
            // 
            // chkSyncTexToObj
            // 
            chkSyncTexToObj.AutoSize = true;
            chkSyncTexToObj.Location = new System.Drawing.Point(6, 131);
            chkSyncTexToObj.Name = "chkSyncTexToObj";
            chkSyncTexToObj.Size = new System.Drawing.Size(197, 17);
            chkSyncTexToObj.TabIndex = 41;
            chkSyncTexToObj.Text = "Sync texture list with selected object";
            chkSyncTexToObj.UseVisualStyleBackColor = true;
            chkSyncTexToObj.CheckedChanged += new EventHandler(chkSyncTexToObj_CheckedChanged);
            // 
            // chkSyncObjToVIS
            // 
            chkSyncObjToVIS.AutoSize = true;
            chkSyncObjToVIS.Location = new System.Drawing.Point(6, 122);
            chkSyncObjToVIS.Name = "chkSyncObjToVIS";
            chkSyncObjToVIS.Size = new System.Drawing.Size(272, 17);
            chkSyncObjToVIS.TabIndex = 42;
            chkSyncObjToVIS.Text = "Sync object list checkbox changes to selected VIS0";
            chkSyncObjToVIS.UseVisualStyleBackColor = true;
            chkSyncObjToVIS.CheckedChanged += new EventHandler(chkSyncObjToVIS_CheckedChanged);
            // 
            // chkDisableBonesOnPlay
            // 
            chkDisableBonesOnPlay.AutoSize = true;
            chkDisableBonesOnPlay.Location = new System.Drawing.Point(6, 145);
            chkDisableBonesOnPlay.Name = "chkDisableBonesOnPlay";
            chkDisableBonesOnPlay.Size = new System.Drawing.Size(204, 17);
            chkDisableBonesOnPlay.TabIndex = 43;
            chkDisableBonesOnPlay.Text = "Disable bones when playing animaton";
            chkDisableBonesOnPlay.UseVisualStyleBackColor = true;
            chkDisableBonesOnPlay.CheckedChanged += new EventHandler(chkDisableBonesOnPlay_CheckedChanged);
            // 
            // chkDisableHighlight
            // 
            chkDisableHighlight.AutoSize = true;
            chkDisableHighlight.Location = new System.Drawing.Point(6, 154);
            chkDisableHighlight.Name = "chkDisableHighlight";
            chkDisableHighlight.Size = new System.Drawing.Size(210, 17);
            chkDisableHighlight.TabIndex = 44;
            chkDisableHighlight.Text = "Disable realtime highlighting in viewport";
            chkDisableHighlight.UseVisualStyleBackColor = true;
            chkDisableHighlight.CheckedChanged += new EventHandler(chkDisableHighlight_CheckedChanged);
            // 
            // chkSnapBonesToFloor
            // 
            chkSnapBonesToFloor.AutoSize = true;
            chkSnapBonesToFloor.Location = new System.Drawing.Point(6, 177);
            chkSnapBonesToFloor.Name = "chkSnapBonesToFloor";
            chkSnapBonesToFloor.Size = new System.Drawing.Size(205, 17);
            chkSnapBonesToFloor.TabIndex = 46;
            chkSnapBonesToFloor.Text = "Snap dragged bones to floor collisions";
            chkSnapBonesToFloor.UseVisualStyleBackColor = true;
            chkSnapBonesToFloor.CheckedChanged += new EventHandler(chkSnapBonesToFloor_CheckedChanged);
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(352, 419);
            tabControl1.TabIndex = 47;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.Transparent;
            tabPage1.Controls.Add(chkTextOverlays);
            tabPage1.Controls.Add(grpProjection);
            tabPage1.Controls.Add(grpLighting);
            tabPage1.Location = new System.Drawing.Point(4, 22);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new System.Drawing.Size(344, 393);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Current Viewport";
            // 
            // chkTextOverlays
            // 
            chkTextOverlays.AutoSize = true;
            chkTextOverlays.Location = new System.Drawing.Point(8, 369);
            chkTextOverlays.Name = "chkTextOverlays";
            chkTextOverlays.Size = new System.Drawing.Size(121, 17);
            chkTextOverlays.TabIndex = 46;
            chkTextOverlays.Text = "Enable text overlays";
            chkTextOverlays.UseVisualStyleBackColor = true;
            chkTextOverlays.CheckedChanged += new EventHandler(chkTextOverlays_CheckedChanged);
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(chkPixelLighting);
            tabPage2.Controls.Add(chkHideMainWindow);
            tabPage2.Controls.Add(chkUsePointsAsBones);
            tabPage2.Controls.Add(chkScaleBones);
            tabPage2.Controls.Add(chkSaveWindowPosition);
            tabPage2.Controls.Add(chkMaximize);
            tabPage2.Controls.Add(grpColors);
            tabPage2.Controls.Add(chkSyncTexToObj);
            tabPage2.Controls.Add(chkDisableHighlight);
            tabPage2.Controls.Add(chkSnapBonesToFloor);
            tabPage2.Controls.Add(label18);
            tabPage2.Controls.Add(maxUndoCount);
            tabPage2.Location = new System.Drawing.Point(4, 22);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new System.Drawing.Size(344, 393);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "General";
            // 
            // chkPixelLighting
            // 
            chkPixelLighting.AutoSize = true;
            chkPixelLighting.Location = new System.Drawing.Point(6, 315);
            chkPixelLighting.Name = "chkPixelLighting";
            chkPixelLighting.Size = new System.Drawing.Size(228, 17);
            chkPixelLighting.TabIndex = 52;
            chkPixelLighting.Text = "Per pixel lighting (as opposed to per vertex)";
            chkPixelLighting.UseVisualStyleBackColor = true;
            chkPixelLighting.CheckedChanged += new EventHandler(chkPixelLighting_CheckedChanged);
            // 
            // chkHideMainWindow
            // 
            chkHideMainWindow.AutoSize = true;
            chkHideMainWindow.Location = new System.Drawing.Point(6, 292);
            chkHideMainWindow.Name = "chkHideMainWindow";
            chkHideMainWindow.Size = new System.Drawing.Size(112, 17);
            chkHideMainWindow.TabIndex = 51;
            chkHideMainWindow.Text = "Hide main window";
            chkHideMainWindow.UseVisualStyleBackColor = true;
            chkHideMainWindow.CheckedChanged += new EventHandler(chkHideMainWindow_CheckedChanged);
            // 
            // chkUsePointsAsBones
            // 
            chkUsePointsAsBones.AutoSize = true;
            chkUsePointsAsBones.Location = new System.Drawing.Point(6, 269);
            chkUsePointsAsBones.Name = "chkUsePointsAsBones";
            chkUsePointsAsBones.Size = new System.Drawing.Size(137, 17);
            chkUsePointsAsBones.TabIndex = 50;
            chkUsePointsAsBones.Text = "Display bones as points";
            chkUsePointsAsBones.UseVisualStyleBackColor = true;
            chkUsePointsAsBones.CheckedChanged += new EventHandler(chkUsePointsAsBones_CheckedChanged);
            // 
            // chkScaleBones
            // 
            chkScaleBones.AutoSize = true;
            chkScaleBones.Location = new System.Drawing.Point(6, 246);
            chkScaleBones.Name = "chkScaleBones";
            chkScaleBones.Size = new System.Drawing.Size(145, 17);
            chkScaleBones.TabIndex = 49;
            chkScaleBones.Text = "Scale bones with camera";
            chkScaleBones.UseVisualStyleBackColor = true;
            chkScaleBones.CheckedChanged += new EventHandler(chkScaleBones_CheckedChanged);
            // 
            // chkSaveWindowPosition
            // 
            chkSaveWindowPosition.AutoSize = true;
            chkSaveWindowPosition.Location = new System.Drawing.Point(6, 223);
            chkSaveWindowPosition.Name = "chkSaveWindowPosition";
            chkSaveWindowPosition.Size = new System.Drawing.Size(205, 17);
            chkSaveWindowPosition.TabIndex = 48;
            chkSaveWindowPosition.Text = "Save window position and dimensions";
            chkSaveWindowPosition.UseVisualStyleBackColor = true;
            chkSaveWindowPosition.CheckedChanged += new EventHandler(chkSaveWindowPosition_CheckedChanged);
            // 
            // chkMaximize
            // 
            chkMaximize.AutoSize = true;
            chkMaximize.Location = new System.Drawing.Point(6, 200);
            chkMaximize.Name = "chkMaximize";
            chkMaximize.Size = new System.Drawing.Size(176, 17);
            chkMaximize.TabIndex = 47;
            chkMaximize.Text = "Maximize window upon opening";
            chkMaximize.UseVisualStyleBackColor = true;
            chkMaximize.CheckedChanged += new EventHandler(chkMaximize_CheckedChanged);
            // 
            // maxUndoCount
            // 
            maxUndoCount.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            maxUndoCount.BorderStyle = BorderStyle.FixedSingle;
            maxUndoCount.Integer = false;
            maxUndoCount.Integral = false;
            maxUndoCount.Location = new System.Drawing.Point(155, 365);
            maxUndoCount.MaximumValue = 3.402823E+38F;
            maxUndoCount.MinimumValue = -3.402823E+38F;
            maxUndoCount.Name = "maxUndoCount";
            maxUndoCount.Size = new System.Drawing.Size(66, 20);
            maxUndoCount.TabIndex = 37;
            maxUndoCount.Text = "0";
            maxUndoCount.ValueChanged += new EventHandler(maxUndoCount_ValueChanged);
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(chkContextLoop);
            tabPage3.Controls.Add(groupBox1);
            tabPage3.Controls.Add(chkPrecalcBoxes);
            tabPage3.Controls.Add(chkDisableBonesOnPlay);
            tabPage3.Controls.Add(chkRetrieveCorrAnims);
            tabPage3.Controls.Add(chkSyncObjToVIS);
            tabPage3.Location = new System.Drawing.Point(4, 22);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new System.Drawing.Size(344, 393);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Animation";
            // 
            // chkContextLoop
            // 
            chkContextLoop.AutoSize = true;
            chkContextLoop.Location = new System.Drawing.Point(6, 191);
            chkContextLoop.Name = "chkContextLoop";
            chkContextLoop.Size = new System.Drawing.Size(221, 17);
            chkContextLoop.TabIndex = 50;
            chkContextLoop.Text = "Change loop preview based on animation";
            chkContextLoop.UseVisualStyleBackColor = true;
            chkContextLoop.CheckedChanged += new EventHandler(chkContextLoop_CheckedChanged);
            // 
            // groupBox1
            // 
            groupBox1.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                | AnchorStyles.Right);
            groupBox1.Controls.Add(chkTanCam);
            groupBox1.Controls.Add(chkTanFog);
            groupBox1.Controls.Add(chkTanLight);
            groupBox1.Controls.Add(chkTanSHP);
            groupBox1.Controls.Add(chkTanSRT);
            groupBox1.Controls.Add(chkTanCHR);
            groupBox1.Location = new System.Drawing.Point(6, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(329, 87);
            groupBox1.TabIndex = 49;
            groupBox1.TabStop = false;
            groupBox1.Text = "Generate Tangents";
            // 
            // chkTanCam
            // 
            chkTanCam.AutoSize = true;
            chkTanCam.Location = new System.Drawing.Point(67, 65);
            chkTanCam.Name = "chkTanCam";
            chkTanCam.Size = new System.Drawing.Size(93, 17);
            chkTanCam.TabIndex = 55;
            chkTanCam.Text = "SCN0 Camera";
            chkTanCam.UseVisualStyleBackColor = true;
            chkTanCam.CheckedChanged += new EventHandler(chkTanCam_CheckedChanged);
            // 
            // chkTanFog
            // 
            chkTanFog.AutoSize = true;
            chkTanFog.Location = new System.Drawing.Point(67, 42);
            chkTanFog.Name = "chkTanFog";
            chkTanFog.Size = new System.Drawing.Size(75, 17);
            chkTanFog.TabIndex = 54;
            chkTanFog.Text = "SCN0 Fog";
            chkTanFog.UseVisualStyleBackColor = true;
            chkTanFog.CheckedChanged += new EventHandler(chkTanFog_CheckedChanged);
            // 
            // chkTanLight
            // 
            chkTanLight.AutoSize = true;
            chkTanLight.Location = new System.Drawing.Point(67, 19);
            chkTanLight.Name = "chkTanLight";
            chkTanLight.Size = new System.Drawing.Size(80, 17);
            chkTanLight.TabIndex = 53;
            chkTanLight.Text = "SCN0 Light";
            chkTanLight.UseVisualStyleBackColor = true;
            chkTanLight.CheckedChanged += new EventHandler(chkTanLight_CheckedChanged);
            // 
            // chkTanSHP
            // 
            chkTanSHP.AutoSize = true;
            chkTanSHP.Location = new System.Drawing.Point(6, 65);
            chkTanSHP.Name = "chkTanSHP";
            chkTanSHP.Size = new System.Drawing.Size(54, 17);
            chkTanSHP.TabIndex = 52;
            chkTanSHP.Text = "SHP0";
            chkTanSHP.UseVisualStyleBackColor = true;
            chkTanSHP.CheckedChanged += new EventHandler(chkTanSHP_CheckedChanged);
            // 
            // chkTanSRT
            // 
            chkTanSRT.AutoSize = true;
            chkTanSRT.Location = new System.Drawing.Point(6, 42);
            chkTanSRT.Name = "chkTanSRT";
            chkTanSRT.Size = new System.Drawing.Size(54, 17);
            chkTanSRT.TabIndex = 51;
            chkTanSRT.Text = "SRT0";
            chkTanSRT.UseVisualStyleBackColor = true;
            chkTanSRT.CheckedChanged += new EventHandler(chkTanSRT_CheckedChanged);
            // 
            // chkTanCHR
            // 
            chkTanCHR.AutoSize = true;
            chkTanCHR.Location = new System.Drawing.Point(6, 19);
            chkTanCHR.Name = "chkTanCHR";
            chkTanCHR.Size = new System.Drawing.Size(55, 17);
            chkTanCHR.TabIndex = 50;
            chkTanCHR.Text = "CHR0";
            chkTanCHR.UseVisualStyleBackColor = true;
            chkTanCHR.CheckedChanged += new EventHandler(chkTanCHR_CheckedChanged);
            // 
            // chkPrecalcBoxes
            // 
            chkPrecalcBoxes.AutoSize = true;
            chkPrecalcBoxes.Location = new System.Drawing.Point(6, 168);
            chkPrecalcBoxes.Name = "chkPrecalcBoxes";
            chkPrecalcBoxes.Size = new System.Drawing.Size(258, 17);
            chkPrecalcBoxes.TabIndex = 48;
            chkPrecalcBoxes.Text = "Display precalculated bounding boxes on frame 0";
            chkPrecalcBoxes.UseVisualStyleBackColor = true;
            chkPrecalcBoxes.CheckedChanged += new EventHandler(chkPrecalcBoxes_CheckedChanged);
            // 
            // panel1
            // 
            panel1.Controls.Add(btnResetSettings);
            panel1.Controls.Add(btnImportSettings);
            panel1.Controls.Add(btnExportSettings);
            panel1.Controls.Add(btnCancel);
            panel1.Controls.Add(btnOkay);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new System.Drawing.Point(0, 419);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(352, 35);
            panel1.TabIndex = 47;
            // 
            // btnResetSettings
            // 
            btnResetSettings.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left
                                                                          | AnchorStyles.Right);
            btnResetSettings.Location = new System.Drawing.Point(136, 6);
            btnResetSettings.Name = "btnResetSettings";
            btnResetSettings.Size = new System.Drawing.Size(80, 23);
            btnResetSettings.TabIndex = 5;
            btnResetSettings.Text = "Reset";
            btnResetSettings.UseVisualStyleBackColor = true;
            btnResetSettings.Click += new EventHandler(btnResetSettings_Click);
            // 
            // btnImportSettings
            // 
            btnImportSettings.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            btnImportSettings.Location = new System.Drawing.Point(70, 6);
            btnImportSettings.Name = "btnImportSettings";
            btnImportSettings.Size = new System.Drawing.Size(60, 23);
            btnImportSettings.TabIndex = 4;
            btnImportSettings.Text = "Import";
            btnImportSettings.UseVisualStyleBackColor = true;
            btnImportSettings.Visible = false;
            btnImportSettings.Click += new EventHandler(btnImportSettings_Click);
            // 
            // btnExportSettings
            // 
            btnExportSettings.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            btnExportSettings.DialogResult = DialogResult.Cancel;
            btnExportSettings.Location = new System.Drawing.Point(4, 6);
            btnExportSettings.Name = "btnExportSettings";
            btnExportSettings.Size = new System.Drawing.Size(60, 23);
            btnExportSettings.TabIndex = 3;
            btnExportSettings.Text = "Export";
            btnExportSettings.UseVisualStyleBackColor = true;
            btnExportSettings.Visible = false;
            btnExportSettings.Click += new EventHandler(btnExportSettings_Click);
            // 
            // ModelViewerSettingsDialog
            // 
            ClientSize = new System.Drawing.Size(352, 454);
            Controls.Add(tabControl1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MinimizeBox = false;
            Name = "ModelViewerSettingsDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Model Editor Settings";
            grpLighting.ResumeLayout(false);
            grpLighting.PerformLayout();
            grpProjection.ResumeLayout(false);
            grpProjection.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            grpColors.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private readonly GoodColorDialog _dlgColor;

        private void lblOrbColor_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = MDL0BoneNode.DefaultNodeColor;
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                MDL0BoneNode.DefaultNodeColor = _dlgColor.Color;
                UpdateOrb();
            }
        }

        private void lblLineColor_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = MDL0BoneNode.DefaultLineColor;
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                MDL0BoneNode.DefaultLineColor = _dlgColor.Color;
                UpdateLine();
            }
        }

        private void lblCol1Color_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = ModelEditorBase._floorHue;
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                ModelEditorBase._floorHue = _dlgColor.Color;
                UpdateCol1();
            }
        }

        private void lblBGColor_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = (Color) BrawlCrate.Properties.Settings.Default.ViewerSettings._bgColor;
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                BrawlCrate.Properties.Settings.Default.ViewerSettings._bgColor = (ARGBPixel) _dlgColor.Color;
                BrawlCrate.Properties.Settings.Default.Save();
                UpdateBGColor();
            }
        }

        private void lblStgBGColor_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = (Color) BrawlCrate.Properties.Settings.Default.ViewerSettings._stgBgColor;
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                BrawlCrate.Properties.Settings.Default.ViewerSettings._stgBgColor = (ARGBPixel) _dlgColor.Color;
                BrawlCrate.Properties.Settings.Default.Save();
                UpdateStgBGColor();
            }
        }

        private void UpdateOrb()
        {
            lblOrbText.Text = ((ARGBPixel) MDL0BoneNode.DefaultNodeColor).ToString();
            lblOrbColor.BackColor = Color.FromArgb(MDL0BoneNode.DefaultNodeColor.R, MDL0BoneNode.DefaultNodeColor.G,
                MDL0BoneNode.DefaultNodeColor.B);

            if (!_updating)
            {
                _form?.ModelPanel.Invalidate();
            }
        }

        private void UpdateLine()
        {
            lblLineText.Text = ((ARGBPixel) MDL0BoneNode.DefaultLineColor).ToString();
            lblLineColor.BackColor = Color.FromArgb(MDL0BoneNode.DefaultLineColor.R, MDL0BoneNode.DefaultLineColor.G,
                MDL0BoneNode.DefaultLineColor.B);

            if (!_updating)
            {
                _form?.ModelPanel.Invalidate();
            }
        }

        private void UpdateCol1()
        {
            lblCol1Text.Text = ((ARGBPixel) ModelEditorBase._floorHue).ToString();
            lblCol1Color.BackColor = Color.FromArgb(ModelEditorBase._floorHue.R, ModelEditorBase._floorHue.G,
                ModelEditorBase._floorHue.B);

            if (!_updating)
            {
                _form?.ModelPanel.Invalidate();
            }
        }

        private void UpdateBGColor()
        {
            ARGBPixel bgColor = BrawlCrate.Properties.Settings.Default.ViewerSettings._bgColor;
            lblBGColorText.Text = bgColor.ToString();
            lblBGColor.BackColor = Color.FromArgb(bgColor.R, bgColor.G, bgColor.B);

            if (!_updating)
            {
                _form?.ModelPanel.Invalidate();
            }
        }

        private void UpdateStgBGColor()
        {
            ARGBPixel bgColor = BrawlCrate.Properties.Settings.Default.ViewerSettings._stgBgColor;
            lblStgBGColorText.Text = bgColor.ToString();
            lblStgBGColor.BackColor = Color.FromArgb(bgColor.R, bgColor.G, bgColor.B);

            if (!_updating)
            {
                _form?.ModelPanel.Invalidate();
            }
        }

        private void UpdateAmb()
        {
            label19.BackColor = Color.FromArgb(255, (int) ax.Value, (int) ay.Value, (int) az.Value);

            if (!_updating && _form != null)
            {
                _form.ModelPanel.CurrentViewport.Ambient =
                    new Vector4(ax.Value / 255.0f, ay.Value / 255.0f, az.Value / 255.0f, 1.0f);
            }
        }

        private void UpdateDif()
        {
            label21.BackColor = Color.FromArgb(255, (int) dx.Value, (int) dy.Value, (int) dz.Value);

            if (!_updating && _form != null)
            {
                _form.ModelPanel.CurrentViewport.Diffuse =
                    new Vector4(dx.Value / 255.0f, dy.Value / 255.0f, dz.Value / 255.0f, 1.0f);
            }
        }

        private void UpdateSpe()
        {
            label22.BackColor = Color.FromArgb(255, (int) sx.Value, (int) sy.Value, (int) sz.Value);

            if (!_updating && _form != null)
            {
                _form.ModelPanel.CurrentViewport.Specular =
                    new Vector4(sx.Value / 255.0f, sy.Value / 255.0f, sz.Value / 255.0f, 1.0f);
            }
        }

        private void UpdateEmi()
        {
            label23.BackColor = Color.FromArgb(255, (int) ex.Value, (int) ey.Value, (int) ez.Value);

            if (!_updating && _form != null)
            {
                _form.ModelPanel.CurrentViewport.Emission =
                    new Vector4(ex.Value / 255.0f, ey.Value / 255.0f, ez.Value / 255.0f, 1.0f);
            }
        }

        public bool _updating = false;

        private void label19_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = Color.FromArgb(255, (int) ax.Value, (int) ay.Value, (int) az.Value);
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                _updating = true;
                ax.Value = _dlgColor.Color.R;
                ay.Value = _dlgColor.Color.G;
                az.Value = _dlgColor.Color.B;
                _updating = false;
                UpdateAmb();
            }
        }

        private void label21_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = Color.FromArgb(255, (int) dx.Value, (int) dy.Value, (int) dz.Value);
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                _updating = true;
                dx.Value = _dlgColor.Color.R;
                dy.Value = _dlgColor.Color.G;
                dz.Value = _dlgColor.Color.B;
                _updating = false;
                UpdateDif();
            }
        }

        private void label22_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = Color.FromArgb(255, (int) sx.Value, (int) sy.Value, (int) sz.Value);
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                _updating = true;
                sx.Value = _dlgColor.Color.R;
                sy.Value = _dlgColor.Color.G;
                sz.Value = _dlgColor.Color.B;
                _updating = false;
                UpdateSpe();
            }
        }

        private void label23_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = Color.FromArgb(255, (int) ex.Value, (int) ey.Value, (int) ez.Value);
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                _updating = true;
                ex.Value = _dlgColor.Color.R;
                ey.Value = _dlgColor.Color.G;
                ez.Value = _dlgColor.Color.B;
                _updating = false;
                UpdateEmi();
            }
        }

        private void cboProjection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_updating && _form != null)
            {
                _form.ModelPanel.CurrentViewport.SetProjectionType((ViewportProjection) cboProjection.SelectedIndex);
                UpdateViewport(_form.ModelPanel.CurrentViewport);
            }
        }

        private void chkRetrieveCorrAnims_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating && _form != null)
            {
                if (_form.RetrieveCorrespondingAnimations = chkRetrieveCorrAnims.Checked)
                {
                    _form.GetFiles(_form.TargetAnimType);
                }
                else
                {
                    _form.GetFiles(NW4RAnimType.None);
                }
            }
        }

        private void chkSyncTexToObj_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating && _form != null)
            {
                _form.SyncTexturesToObjectList = chkSyncTexToObj.Checked;
            }
        }

        private void chkSyncObjToVIS_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating && _form != null)
            {
                _form.SyncVIS0 = chkSyncObjToVIS.Checked;
            }
        }

        private void chkDisableBonesOnPlay_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating && _form != null)
            {
                _form.DisableBonesWhenPlaying = chkDisableBonesOnPlay.Checked;
            }
        }

        private void chkDisableHighlight_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating && _form != null)
            {
                _form.DoNotHighlightOnMouseMove = chkDisableHighlight.Checked;
            }
        }

        private void chkSnapBonesToFloor_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating && _form != null)
            {
                _form.SnapBonesToCollisions = chkSnapBonesToFloor.Checked;
            }
        }

        private void chkMaximize_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating && _form != null)
            {
                _form._maximize = chkMaximize.Checked;
            }
        }

        private void chkPrecalcBoxes_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating && _form != null)
            {
                _form.UseBindStateBoxes = chkPrecalcBoxes.Checked;
            }
        }

        private void chkTanCHR_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                CHR0EntryNode._generateTangents = chkTanCHR.Checked;
            }
        }

        private void chkTanSRT_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                SRT0TextureNode._generateTangents = chkTanSRT.Checked;
            }
        }

        private void chkTanSHP_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                SHP0VertexSetNode._generateTangents = chkTanSHP.Checked;
            }
        }

        private void chkTanCam_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                SCN0CameraNode._generateTangents = chkTanCam.Checked;
            }
        }

        private void chkTanFog_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                SCN0FogNode._generateTangents = chkTanFog.Checked;
            }
        }

        private void chkTanLight_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                SCN0LightNode._generateTangents = chkTanLight.Checked;
            }
        }

        private void btnResetSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure you want to reset all settings to default?", "Reset?",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
            {
                return;
            }

            BrawlCrate.Properties.Settings.Default.ViewerSettings = null;
            BrawlCrate.Properties.Settings.Default.ViewerSettingsSet = false;
            _form.SetDefaultSettings();
            UpdateAll();
        }

        private void btnImportSettings_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog
            {
                Filter = "BrawlCrate Settings (*.settings)|*.settings",
                FileName = Application.StartupPath
            };
            if (od.ShowDialog() == DialogResult.OK)
            {
                string path = od.FileName;
                ModelEditorSettings settings = Serializer.DeserializeObject(path) as ModelEditorSettings;
                _form.DistributeSettings(settings);
            }
        }

        private void btnExportSettings_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog
            {
                Filter = "BrawlCrate Settings (*.settings)|*.settings",
                FileName = Application.StartupPath
            };
            if (sd.ShowDialog() == DialogResult.OK)
            {
                string path = sd.FileName;
                ModelEditorSettings settings = _form.CollectSettings();

                Serializer.SerializeObject(path, settings);
                MessageBox.Show("Settings successfully saved to " + path);
            }
        }

        private void chkTextOverlays_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating && _form != null)
            {
                _form.ModelPanel.CurrentViewport.TextOverlaysEnabled = chkTextOverlays.Checked;
            }
        }

        private void maxUndoCount_ValueChanged(object sender, EventArgs e)
        {
            if (!_updating && _form != null)
            {
                _form._allowedUndos = (uint) maxUndoCount.Value;
            }
        }

        private void chkSaveWindowPosition_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating && _form != null)
            {
                _form._savePosition = chkSaveWindowPosition.Checked;
            }
        }

        private void chkDefaultPos_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkDefaultPos.Checked)
            {
                return;
            }

            _updating = true;

            numPosSX.Value = current.Camera._defaultScale._x;
            numPosSY.Value = current.Camera._defaultScale._y;
            numPosSZ.Value = current.Camera._defaultScale._z;

            numPosRX.Value = current.Camera._defaultRotate._x;
            numPosRY.Value = current.Camera._defaultRotate._y;
            numPosRZ.Value = current.Camera._defaultRotate._z;

            numPosTX.Value = current.Camera._defaultTranslate._x;
            numPosTY.Value = current.Camera._defaultTranslate._y;
            numPosTZ.Value = current.Camera._defaultTranslate._z;

            _updating = false;
        }

        private void chkCurrentPos_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkCurrentPos.Checked)
            {
                return;
            }

            _updating = true;

            numPosSX.Value = current.Camera._scale._x;
            numPosSY.Value = current.Camera._scale._y;
            numPosSZ.Value = current.Camera._scale._z;

            numPosRX.Value = current.Camera._rotation._x;
            numPosRY.Value = current.Camera._rotation._y;
            numPosRZ.Value = current.Camera._rotation._z;

            Vector3 trans = current.Camera.GetPoint();
            numPosTX.Value = trans._x;
            numPosTY.Value = trans._y;
            numPosTZ.Value = trans._z;

            _updating = false;
        }

        private void chkLightEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating && _form != null)
            {
                _form.ModelPanel.CurrentViewport.LightEnabled = chkLightEnabled.Checked;
            }
        }

        private void chkLightDirectional_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating && _form != null)
            {
                _form.ModelPanel.CurrentViewport.LightDirectional = chkLightDirectional.Checked;
            }
        }

        private void chkScaleBones_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating && _form != null)
            {
                _form.ModelPanel.CurrentViewport.ScaleBones = chkScaleBones.Checked;
            }
        }

        private void chkUsePointsAsBones_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating && _form != null)
            {
                _form.ModelPanel.CurrentViewport.RenderBonesAsPoints = chkUsePointsAsBones.Checked;
            }
        }

        private void chkContextLoop_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            BrawlLib.Properties.Settings.Default.ContextualLoopAnimation = chkContextLoop.Checked;
            BrawlLib.Properties.Settings.Default.Save();
        }

        private void chkHideMainWindow_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            MainForm.Instance.Visible = !(_form._hideMainWindow = chkHideMainWindow.Checked);
            foreach (ModelEditControl.ModelEditControl c in ModelEditControl.ModelEditControl.Instances)
            {
                c._hideMainWindow = _form._hideMainWindow;
            }

            _form.SaveSettings();
        }

        private void chkPixelLighting_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            BrawlCrate.Properties.Settings.Default.PixelLighting =
                ShaderGenerator.UsePixelLighting = chkPixelLighting.Checked;
            _form.ModelPanel.Invalidate();
            BrawlCrate.Properties.Settings.Default.Save();
        }
    }
}