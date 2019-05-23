using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;
using BrawlLib.Imaging;
using BrawlLib.OpenGL;
using BrawlBox;
using BrawlLib.Wii.Graphics;

namespace System.Windows.Forms
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
        private ModelEditControl _form;

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
                if (i < 4 || i > 6)
                {
                    _boxes[i]._maxValue = 255;
                    _boxes[i]._minValue = 0;
                }

            foreach (NumericInputBox b in _boxes)
                b.ValueChanged += new System.EventHandler(this.BoxValueChanged);

            _updating = true;
            cboProjection.DataSource = Enum.GetNames(typeof(ViewportProjection));
            _updating = false;
        }

        private NumericInputBox[] _boxes = new NumericInputBox[31];
        private float[] _origValues = new float[31];
        private Color _origNode, _origBone, _origFloor;
        private CheckBox[] _checkBoxes = new CheckBox[9];
        private bool[] _origChecks = new bool[9];

        public void Show(ModelEditControl owner)
        {
            _form = owner;
            _form.RenderLightDisplay = true;
            _form.ModelPanel.OnCurrentViewportChanged += UpdateViewport;

            UpdateAll();

            base.Show(_form as IWin32Window);
        }

        ModelPanelViewport current;

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

            UpdateOrb();
            UpdateLine();
            UpdateCol1();

            _updating = false;

            UpdateViewport(_form.ModelPanel.CurrentViewport);
        }

        void Camera_OnPositionChanged()
        {
            if (_updating)
                return;

            if (!chkDefaultPos.Checked)
                chkCurrentPos_CheckedChanged(null, null);
        }

        public void UpdateViewport(GLViewport viewport)
        {
            if (viewport as ModelPanelViewport != current)
            {
                if (current != null)
                    current.Camera.OnPositionChanged -= Camera_OnPositionChanged;
                if (viewport != null)
                    viewport.Camera.OnPositionChanged += Camera_OnPositionChanged;
            }

            current = viewport as ModelPanelViewport;

            if (chkDefaultPos.Checked)
                chkDefaultPos_CheckedChanged(null, null);
            else
                chkCurrentPos_CheckedChanged(null, null);

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

            cboProjection.SelectedIndex = (int)current.ViewType;
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
                return;

            _updating = true;

            _boxes[5].Value = _boxes[5].Value.RemapToRange(-180.0f, 180.0f);
            _boxes[6].Value = _boxes[6].Value.RemapToRange(-180.0f, 180.0f);

            current.Ambient = new Vector4(_boxes[0].Value / 255.0f, _boxes[1].Value / 255.0f, _boxes[2].Value / 255.0f, 1.0f);
            current.LightPosition = new Vector4(_boxes[4].Value, _boxes[5].Value, _boxes[6].Value, 1.0f);
            current.Diffuse = new Vector4(_boxes[7].Value / 255.0f, _boxes[8].Value / 255.0f, _boxes[9].Value / 255.0f, 1.0f);
            current.Specular = new Vector4(_boxes[11].Value / 255.0f, _boxes[12].Value / 255.0f, _boxes[13].Value / 255.0f, 1.0f);
            current.Emission = new Vector4(_boxes[3].Value / 255.0f, _boxes[10].Value / 255.0f, _boxes[14].Value / 255.0f, 1.0f);

            current.TranslationScale = _boxes[15].Value;
            current.RotationScale = _boxes[16].Value;
            current.ZoomScale = _boxes[17].Value;

            current.Camera._fovY = _boxes[18].Value;
            current.Camera._nearZ = _boxes[19].Value;
            current.Camera._farZ = _boxes[20].Value;
            current.Camera.CalculateProjection();

            _form.AllowedUndos = (uint)Math.Abs(_boxes[21].Value);

            int i = (int)(sender as NumericInputBox).Tag;

            if (i == 3 || i == 10 || i == 14)
                UpdateEmi();
            else if (i < 3)
                UpdateAmb();
            else if (i < 10)
                UpdateDif();
            else
                UpdateSpe();

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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.grpLighting = new System.Windows.Forms.GroupBox();
            this.chkLightDirectional = new System.Windows.Forms.CheckBox();
            this.chkLightEnabled = new System.Windows.Forms.CheckBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.ez = new System.Windows.Forms.NumericInputBox();
            this.ey = new System.Windows.Forms.NumericInputBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ex = new System.Windows.Forms.NumericInputBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.sz = new System.Windows.Forms.NumericInputBox();
            this.dz = new System.Windows.Forms.NumericInputBox();
            this.radius = new System.Windows.Forms.NumericInputBox();
            this.az = new System.Windows.Forms.NumericInputBox();
            this.elevation = new System.Windows.Forms.NumericInputBox();
            this.sy = new System.Windows.Forms.NumericInputBox();
            this.azimuth = new System.Windows.Forms.NumericInputBox();
            this.dy = new System.Windows.Forms.NumericInputBox();
            this.ay = new System.Windows.Forms.NumericInputBox();
            this.sx = new System.Windows.Forms.NumericInputBox();
            this.dx = new System.Windows.Forms.NumericInputBox();
            this.ax = new System.Windows.Forms.NumericInputBox();
            this.grpProjection = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkDefaultPos = new System.Windows.Forms.RadioButton();
            this.chkCurrentPos = new System.Windows.Forms.RadioButton();
            this.label26 = new System.Windows.Forms.Label();
            this.numPosTX = new System.Windows.Forms.NumericInputBox();
            this.label27 = new System.Windows.Forms.Label();
            this.numPosTY = new System.Windows.Forms.NumericInputBox();
            this.label28 = new System.Windows.Forms.Label();
            this.numPosTZ = new System.Windows.Forms.NumericInputBox();
            this.numPosSX = new System.Windows.Forms.NumericInputBox();
            this.numPosRZ = new System.Windows.Forms.NumericInputBox();
            this.numPosSY = new System.Windows.Forms.NumericInputBox();
            this.numPosRY = new System.Windows.Forms.NumericInputBox();
            this.numPosSZ = new System.Windows.Forms.NumericInputBox();
            this.numPosRX = new System.Windows.Forms.NumericInputBox();
            this.label25 = new System.Windows.Forms.Label();
            this.cboProjection = new System.Windows.Forms.ComboBox();
            this.farZ = new System.Windows.Forms.NumericInputBox();
            this.nearZ = new System.Windows.Forms.NumericInputBox();
            this.yFov = new System.Windows.Forms.NumericInputBox();
            this.zScale = new System.Windows.Forms.NumericInputBox();
            this.tScale = new System.Windows.Forms.NumericInputBox();
            this.rScale = new System.Windows.Forms.NumericInputBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.grpColors = new System.Windows.Forms.GroupBox();
            this.lblCol1Color = new System.Windows.Forms.Label();
            this.lblLineColor = new System.Windows.Forms.Label();
            this.lblCol1Text = new System.Windows.Forms.Label();
            this.lblLineText = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lblOrbColor = new System.Windows.Forms.Label();
            this.lblOrbText = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.chkRetrieveCorrAnims = new System.Windows.Forms.CheckBox();
            this.chkSyncTexToObj = new System.Windows.Forms.CheckBox();
            this.chkSyncObjToVIS = new System.Windows.Forms.CheckBox();
            this.chkDisableBonesOnPlay = new System.Windows.Forms.CheckBox();
            this.chkDisableHighlight = new System.Windows.Forms.CheckBox();
            this.chkSnapBonesToFloor = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkTextOverlays = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkPixelLighting = new System.Windows.Forms.CheckBox();
            this.chkHideMainWindow = new System.Windows.Forms.CheckBox();
            this.chkUsePointsAsBones = new System.Windows.Forms.CheckBox();
            this.chkScaleBones = new System.Windows.Forms.CheckBox();
            this.chkSaveWindowPosition = new System.Windows.Forms.CheckBox();
            this.chkMaximize = new System.Windows.Forms.CheckBox();
            this.maxUndoCount = new System.Windows.Forms.NumericInputBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkTanCam = new System.Windows.Forms.CheckBox();
            this.chkTanFog = new System.Windows.Forms.CheckBox();
            this.chkTanLight = new System.Windows.Forms.CheckBox();
            this.chkTanSHP = new System.Windows.Forms.CheckBox();
            this.chkTanSRT = new System.Windows.Forms.CheckBox();
            this.chkTanCHR = new System.Windows.Forms.CheckBox();
            this.chkPrecalcBoxes = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnResetSettings = new System.Windows.Forms.Button();
            this.btnImportSettings = new System.Windows.Forms.Button();
            this.btnExportSettings = new System.Windows.Forms.Button();
            this.grpLighting.SuspendLayout();
            this.grpProjection.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpColors.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(136, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.Location = new System.Drawing.Point(268, 6);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(60, 23);
            this.btnOkay.TabIndex = 1;
            this.btnOkay.Text = "&Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(33, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Ambient:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "Radius";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(33, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Diffuse:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(33, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "Specular:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(88, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 20);
            this.label5.TabIndex = 19;
            this.label5.Text = "R";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(137, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 20);
            this.label6.TabIndex = 20;
            this.label6.Text = "G";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(186, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 20);
            this.label7.TabIndex = 21;
            this.label7.Text = "B";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpLighting
            // 
            this.grpLighting.Controls.Add(this.chkLightDirectional);
            this.grpLighting.Controls.Add(this.chkLightEnabled);
            this.grpLighting.Controls.Add(this.label23);
            this.grpLighting.Controls.Add(this.label22);
            this.grpLighting.Controls.Add(this.label21);
            this.grpLighting.Controls.Add(this.label19);
            this.grpLighting.Controls.Add(this.ez);
            this.grpLighting.Controls.Add(this.ey);
            this.grpLighting.Controls.Add(this.label8);
            this.grpLighting.Controls.Add(this.ex);
            this.grpLighting.Controls.Add(this.label17);
            this.grpLighting.Controls.Add(this.label16);
            this.grpLighting.Controls.Add(this.sz);
            this.grpLighting.Controls.Add(this.dz);
            this.grpLighting.Controls.Add(this.label2);
            this.grpLighting.Controls.Add(this.radius);
            this.grpLighting.Controls.Add(this.az);
            this.grpLighting.Controls.Add(this.elevation);
            this.grpLighting.Controls.Add(this.sy);
            this.grpLighting.Controls.Add(this.azimuth);
            this.grpLighting.Controls.Add(this.dy);
            this.grpLighting.Controls.Add(this.ay);
            this.grpLighting.Controls.Add(this.label7);
            this.grpLighting.Controls.Add(this.label6);
            this.grpLighting.Controls.Add(this.label5);
            this.grpLighting.Controls.Add(this.label4);
            this.grpLighting.Controls.Add(this.label3);
            this.grpLighting.Controls.Add(this.label1);
            this.grpLighting.Controls.Add(this.sx);
            this.grpLighting.Controls.Add(this.dx);
            this.grpLighting.Controls.Add(this.ax);
            this.grpLighting.Location = new System.Drawing.Point(6, 195);
            this.grpLighting.Name = "grpLighting";
            this.grpLighting.Size = new System.Drawing.Size(310, 167);
            this.grpLighting.TabIndex = 35;
            this.grpLighting.TabStop = false;
            this.grpLighting.Text = "Lighting";
            // 
            // chkLightDirectional
            // 
            this.chkLightDirectional.AutoSize = true;
            this.chkLightDirectional.Checked = true;
            this.chkLightDirectional.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLightDirectional.Location = new System.Drawing.Point(215, 36);
            this.chkLightDirectional.Name = "chkLightDirectional";
            this.chkLightDirectional.Size = new System.Drawing.Size(76, 17);
            this.chkLightDirectional.TabIndex = 45;
            this.chkLightDirectional.Text = "Directional";
            this.chkLightDirectional.UseVisualStyleBackColor = true;
            this.chkLightDirectional.Visible = false;
            this.chkLightDirectional.CheckedChanged += new System.EventHandler(this.chkLightDirectional_CheckedChanged);
            // 
            // chkLightEnabled
            // 
            this.chkLightEnabled.AutoSize = true;
            this.chkLightEnabled.Location = new System.Drawing.Point(215, 19);
            this.chkLightEnabled.Name = "chkLightEnabled";
            this.chkLightEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkLightEnabled.TabIndex = 44;
            this.chkLightEnabled.Text = "Enabled";
            this.chkLightEnabled.UseVisualStyleBackColor = true;
            this.chkLightEnabled.CheckedChanged += new System.EventHandler(this.chkLightEnabled_CheckedChanged);
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(235, 138);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(40, 20);
            this.label23.TabIndex = 43;
            this.label23.Click += new System.EventHandler(this.label23_Click);
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(235, 119);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(40, 20);
            this.label22.TabIndex = 42;
            this.label22.Click += new System.EventHandler(this.label22_Click);
            // 
            // label21
            // 
            this.label21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label21.Location = new System.Drawing.Point(235, 100);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(40, 20);
            this.label21.TabIndex = 41;
            this.label21.Click += new System.EventHandler(this.label21_Click);
            // 
            // label19
            // 
            this.label19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label19.Location = new System.Drawing.Point(235, 81);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(40, 20);
            this.label19.TabIndex = 11;
            this.label19.Click += new System.EventHandler(this.label19_Click);
            // 
            // ez
            // 
            this.ez.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ez.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ez.Integral = false;
            this.ez.Location = new System.Drawing.Point(186, 138);
            this.ez.MaximumValue = 3.402823E+38F;
            this.ez.MinimumValue = -3.402823E+38F;
            this.ez.Name = "ez";
            this.ez.Size = new System.Drawing.Size(50, 20);
            this.ez.TabIndex = 40;
            this.ez.Text = "0";
            // 
            // ey
            // 
            this.ey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ey.Integral = false;
            this.ey.Location = new System.Drawing.Point(137, 138);
            this.ey.MaximumValue = 3.402823E+38F;
            this.ey.MinimumValue = -3.402823E+38F;
            this.ey.Name = "ey";
            this.ey.Size = new System.Drawing.Size(50, 20);
            this.ey.TabIndex = 39;
            this.ey.Text = "0";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(33, 138);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 20);
            this.label8.TabIndex = 38;
            this.label8.Text = "Emission:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ex
            // 
            this.ex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ex.Integral = false;
            this.ex.Location = new System.Drawing.Point(88, 138);
            this.ex.MaximumValue = 3.402823E+38F;
            this.ex.MinimumValue = -3.402823E+38F;
            this.ex.Name = "ex";
            this.ex.Size = new System.Drawing.Size(50, 20);
            this.ex.TabIndex = 37;
            this.ex.Text = "0";
            // 
            // label17
            // 
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label17.Location = new System.Drawing.Point(143, 16);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(66, 20);
            this.label17.TabIndex = 36;
            this.label17.Text = "Elevation";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Location = new System.Drawing.Point(75, 16);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(66, 20);
            this.label16.TabIndex = 35;
            this.label16.Text = "Azimuth";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sz
            // 
            this.sz.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sz.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sz.Integral = false;
            this.sz.Location = new System.Drawing.Point(186, 119);
            this.sz.MaximumValue = 3.402823E+38F;
            this.sz.MinimumValue = -3.402823E+38F;
            this.sz.Name = "sz";
            this.sz.Size = new System.Drawing.Size(50, 20);
            this.sz.TabIndex = 30;
            this.sz.Text = "0";
            // 
            // dz
            // 
            this.dz.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dz.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dz.Integral = false;
            this.dz.Location = new System.Drawing.Point(186, 100);
            this.dz.MaximumValue = 3.402823E+38F;
            this.dz.MinimumValue = -3.402823E+38F;
            this.dz.Name = "dz";
            this.dz.Size = new System.Drawing.Size(50, 20);
            this.dz.TabIndex = 29;
            this.dz.Text = "0";
            // 
            // radius
            // 
            this.radius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.radius.Integral = false;
            this.radius.Location = new System.Drawing.Point(6, 35);
            this.radius.MaximumValue = 3.402823E+38F;
            this.radius.MinimumValue = -3.402823E+38F;
            this.radius.Name = "radius";
            this.radius.Size = new System.Drawing.Size(66, 20);
            this.radius.TabIndex = 4;
            this.radius.Text = "0";
            // 
            // az
            // 
            this.az.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.az.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.az.Integral = false;
            this.az.Location = new System.Drawing.Point(186, 81);
            this.az.MaximumValue = 3.402823E+38F;
            this.az.MinimumValue = -3.402823E+38F;
            this.az.Name = "az";
            this.az.Size = new System.Drawing.Size(50, 20);
            this.az.TabIndex = 27;
            this.az.Text = "0";
            // 
            // elevation
            // 
            this.elevation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.elevation.Integral = false;
            this.elevation.Location = new System.Drawing.Point(143, 35);
            this.elevation.MaximumValue = 3.402823E+38F;
            this.elevation.MinimumValue = -3.402823E+38F;
            this.elevation.Name = "elevation";
            this.elevation.Size = new System.Drawing.Size(66, 20);
            this.elevation.TabIndex = 28;
            this.elevation.Text = "0";
            // 
            // sy
            // 
            this.sy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sy.Integral = false;
            this.sy.Location = new System.Drawing.Point(137, 119);
            this.sy.MaximumValue = 3.402823E+38F;
            this.sy.MinimumValue = -3.402823E+38F;
            this.sy.Name = "sy";
            this.sy.Size = new System.Drawing.Size(50, 20);
            this.sy.TabIndex = 26;
            this.sy.Text = "0";
            // 
            // azimuth
            // 
            this.azimuth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.azimuth.Integral = false;
            this.azimuth.Location = new System.Drawing.Point(75, 35);
            this.azimuth.MaximumValue = 3.402823E+38F;
            this.azimuth.MinimumValue = -3.402823E+38F;
            this.azimuth.Name = "azimuth";
            this.azimuth.Size = new System.Drawing.Size(66, 20);
            this.azimuth.TabIndex = 24;
            this.azimuth.Text = "0";
            // 
            // dy
            // 
            this.dy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dy.Integral = false;
            this.dy.Location = new System.Drawing.Point(137, 100);
            this.dy.MaximumValue = 3.402823E+38F;
            this.dy.MinimumValue = -3.402823E+38F;
            this.dy.Name = "dy";
            this.dy.Size = new System.Drawing.Size(50, 20);
            this.dy.TabIndex = 25;
            this.dy.Text = "0";
            // 
            // ay
            // 
            this.ay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ay.Integral = false;
            this.ay.Location = new System.Drawing.Point(137, 81);
            this.ay.MaximumValue = 3.402823E+38F;
            this.ay.MinimumValue = -3.402823E+38F;
            this.ay.Name = "ay";
            this.ay.Size = new System.Drawing.Size(50, 20);
            this.ay.TabIndex = 23;
            this.ay.Text = "0";
            // 
            // sx
            // 
            this.sx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sx.Integral = false;
            this.sx.Location = new System.Drawing.Point(88, 119);
            this.sx.MaximumValue = 3.402823E+38F;
            this.sx.MinimumValue = -3.402823E+38F;
            this.sx.Name = "sx";
            this.sx.Size = new System.Drawing.Size(50, 20);
            this.sx.TabIndex = 6;
            this.sx.Text = "0";
            // 
            // dx
            // 
            this.dx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dx.Integral = false;
            this.dx.Location = new System.Drawing.Point(88, 100);
            this.dx.MaximumValue = 3.402823E+38F;
            this.dx.MinimumValue = -3.402823E+38F;
            this.dx.Name = "dx";
            this.dx.Size = new System.Drawing.Size(50, 20);
            this.dx.TabIndex = 5;
            this.dx.Text = "0";
            // 
            // ax
            // 
            this.ax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ax.Integral = false;
            this.ax.Location = new System.Drawing.Point(88, 81);
            this.ax.MaximumValue = 3.402823E+38F;
            this.ax.MinimumValue = -3.402823E+38F;
            this.ax.Name = "ax";
            this.ax.Size = new System.Drawing.Size(50, 20);
            this.ax.TabIndex = 3;
            this.ax.Text = "0";
            // 
            // grpProjection
            // 
            this.grpProjection.Controls.Add(this.groupBox2);
            this.grpProjection.Controls.Add(this.label25);
            this.grpProjection.Controls.Add(this.cboProjection);
            this.grpProjection.Controls.Add(this.farZ);
            this.grpProjection.Controls.Add(this.nearZ);
            this.grpProjection.Controls.Add(this.yFov);
            this.grpProjection.Controls.Add(this.zScale);
            this.grpProjection.Controls.Add(this.tScale);
            this.grpProjection.Controls.Add(this.rScale);
            this.grpProjection.Controls.Add(this.label14);
            this.grpProjection.Controls.Add(this.label13);
            this.grpProjection.Controls.Add(this.label12);
            this.grpProjection.Controls.Add(this.label11);
            this.grpProjection.Controls.Add(this.label10);
            this.grpProjection.Controls.Add(this.label9);
            this.grpProjection.Location = new System.Drawing.Point(6, 6);
            this.grpProjection.Name = "grpProjection";
            this.grpProjection.Size = new System.Drawing.Size(310, 187);
            this.grpProjection.TabIndex = 36;
            this.grpProjection.TabStop = false;
            this.grpProjection.Text = "Camera";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkDefaultPos);
            this.groupBox2.Controls.Add(this.chkCurrentPos);
            this.groupBox2.Controls.Add(this.label26);
            this.groupBox2.Controls.Add(this.numPosTX);
            this.groupBox2.Controls.Add(this.label27);
            this.groupBox2.Controls.Add(this.numPosTY);
            this.groupBox2.Controls.Add(this.label28);
            this.groupBox2.Controls.Add(this.numPosTZ);
            this.groupBox2.Controls.Add(this.numPosSX);
            this.groupBox2.Controls.Add(this.numPosRZ);
            this.groupBox2.Controls.Add(this.numPosSY);
            this.groupBox2.Controls.Add(this.numPosRY);
            this.groupBox2.Controls.Add(this.numPosSZ);
            this.groupBox2.Controls.Add(this.numPosRX);
            this.groupBox2.Location = new System.Drawing.Point(6, 105);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(297, 76);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Position";
            // 
            // chkDefaultPos
            // 
            this.chkDefaultPos.AutoSize = true;
            this.chkDefaultPos.Location = new System.Drawing.Point(230, 44);
            this.chkDefaultPos.Name = "chkDefaultPos";
            this.chkDefaultPos.Size = new System.Drawing.Size(59, 17);
            this.chkDefaultPos.TabIndex = 27;
            this.chkDefaultPos.Text = "Default";
            this.chkDefaultPos.UseVisualStyleBackColor = true;
            this.chkDefaultPos.CheckedChanged += new System.EventHandler(this.chkDefaultPos_CheckedChanged);
            // 
            // chkCurrentPos
            // 
            this.chkCurrentPos.AutoSize = true;
            this.chkCurrentPos.Checked = true;
            this.chkCurrentPos.Location = new System.Drawing.Point(230, 21);
            this.chkCurrentPos.Name = "chkCurrentPos";
            this.chkCurrentPos.Size = new System.Drawing.Size(59, 17);
            this.chkCurrentPos.TabIndex = 26;
            this.chkCurrentPos.TabStop = true;
            this.chkCurrentPos.Text = "Current";
            this.chkCurrentPos.UseVisualStyleBackColor = true;
            this.chkCurrentPos.CheckedChanged += new System.EventHandler(this.chkCurrentPos_CheckedChanged);
            // 
            // label26
            // 
            this.label26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label26.Location = new System.Drawing.Point(6, 13);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(71, 20);
            this.label26.TabIndex = 14;
            this.label26.Text = "Scale: ";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPosTX
            // 
            this.numPosTX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosTX.Integral = false;
            this.numPosTX.Location = new System.Drawing.Point(76, 51);
            this.numPosTX.MaximumValue = 3.402823E+38F;
            this.numPosTX.MinimumValue = -3.402823E+38F;
            this.numPosTX.Name = "numPosTX";
            this.numPosTX.Size = new System.Drawing.Size(50, 20);
            this.numPosTX.TabIndex = 25;
            this.numPosTX.Text = "0";
            // 
            // label27
            // 
            this.label27.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label27.Location = new System.Drawing.Point(6, 32);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(71, 20);
            this.label27.TabIndex = 15;
            this.label27.Text = "Rotation:";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPosTY
            // 
            this.numPosTY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosTY.Integral = false;
            this.numPosTY.Location = new System.Drawing.Point(125, 51);
            this.numPosTY.MaximumValue = 3.402823E+38F;
            this.numPosTY.MinimumValue = -3.402823E+38F;
            this.numPosTY.Name = "numPosTY";
            this.numPosTY.Size = new System.Drawing.Size(50, 20);
            this.numPosTY.TabIndex = 24;
            this.numPosTY.Text = "0";
            // 
            // label28
            // 
            this.label28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label28.Location = new System.Drawing.Point(6, 51);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(71, 20);
            this.label28.TabIndex = 16;
            this.label28.Text = "Translation:";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPosTZ
            // 
            this.numPosTZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosTZ.Integral = false;
            this.numPosTZ.Location = new System.Drawing.Point(174, 51);
            this.numPosTZ.MaximumValue = 3.402823E+38F;
            this.numPosTZ.MinimumValue = -3.402823E+38F;
            this.numPosTZ.Name = "numPosTZ";
            this.numPosTZ.Size = new System.Drawing.Size(50, 20);
            this.numPosTZ.TabIndex = 23;
            this.numPosTZ.Text = "0";
            // 
            // numPosSX
            // 
            this.numPosSX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosSX.Integral = false;
            this.numPosSX.Location = new System.Drawing.Point(76, 13);
            this.numPosSX.MaximumValue = 3.402823E+38F;
            this.numPosSX.MinimumValue = -3.402823E+38F;
            this.numPosSX.Name = "numPosSX";
            this.numPosSX.Size = new System.Drawing.Size(50, 20);
            this.numPosSX.TabIndex = 17;
            this.numPosSX.Text = "0";
            // 
            // numPosRZ
            // 
            this.numPosRZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosRZ.Integral = false;
            this.numPosRZ.Location = new System.Drawing.Point(174, 32);
            this.numPosRZ.MaximumValue = 3.402823E+38F;
            this.numPosRZ.MinimumValue = -3.402823E+38F;
            this.numPosRZ.Name = "numPosRZ";
            this.numPosRZ.Size = new System.Drawing.Size(50, 20);
            this.numPosRZ.TabIndex = 22;
            this.numPosRZ.Text = "0";
            // 
            // numPosSY
            // 
            this.numPosSY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosSY.Integral = false;
            this.numPosSY.Location = new System.Drawing.Point(125, 13);
            this.numPosSY.MaximumValue = 3.402823E+38F;
            this.numPosSY.MinimumValue = -3.402823E+38F;
            this.numPosSY.Name = "numPosSY";
            this.numPosSY.Size = new System.Drawing.Size(50, 20);
            this.numPosSY.TabIndex = 18;
            this.numPosSY.Text = "0";
            // 
            // numPosRY
            // 
            this.numPosRY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosRY.Integral = false;
            this.numPosRY.Location = new System.Drawing.Point(125, 32);
            this.numPosRY.MaximumValue = 3.402823E+38F;
            this.numPosRY.MinimumValue = -3.402823E+38F;
            this.numPosRY.Name = "numPosRY";
            this.numPosRY.Size = new System.Drawing.Size(50, 20);
            this.numPosRY.TabIndex = 21;
            this.numPosRY.Text = "0";
            // 
            // numPosSZ
            // 
            this.numPosSZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosSZ.Integral = false;
            this.numPosSZ.Location = new System.Drawing.Point(174, 13);
            this.numPosSZ.MaximumValue = 3.402823E+38F;
            this.numPosSZ.MinimumValue = -3.402823E+38F;
            this.numPosSZ.Name = "numPosSZ";
            this.numPosSZ.Size = new System.Drawing.Size(50, 20);
            this.numPosSZ.TabIndex = 19;
            this.numPosSZ.Text = "0";
            // 
            // numPosRX
            // 
            this.numPosRX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPosRX.Integral = false;
            this.numPosRX.Location = new System.Drawing.Point(76, 32);
            this.numPosRX.MaximumValue = 3.402823E+38F;
            this.numPosRX.MinimumValue = -3.402823E+38F;
            this.numPosRX.Name = "numPosRX";
            this.numPosRX.Size = new System.Drawing.Size(50, 20);
            this.numPosRX.TabIndex = 20;
            this.numPosRX.Text = "0";
            // 
            // label25
            // 
            this.label25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label25.Location = new System.Drawing.Point(6, 21);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(100, 21);
            this.label25.TabIndex = 13;
            this.label25.Text = "Projection:";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboProjection
            // 
            this.cboProjection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjection.FormattingEnabled = true;
            this.cboProjection.Location = new System.Drawing.Point(105, 21);
            this.cboProjection.Name = "cboProjection";
            this.cboProjection.Size = new System.Drawing.Size(198, 21);
            this.cboProjection.TabIndex = 12;
            this.cboProjection.SelectedIndexChanged += new System.EventHandler(this.cboProjection_SelectedIndexChanged);
            // 
            // farZ
            // 
            this.farZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.farZ.Integral = false;
            this.farZ.Location = new System.Drawing.Point(243, 79);
            this.farZ.MaximumValue = 3.402823E+38F;
            this.farZ.MinimumValue = -3.402823E+38F;
            this.farZ.Name = "farZ";
            this.farZ.Size = new System.Drawing.Size(60, 20);
            this.farZ.TabIndex = 11;
            this.farZ.Text = "0";
            // 
            // nearZ
            // 
            this.nearZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nearZ.Integral = false;
            this.nearZ.Location = new System.Drawing.Point(243, 60);
            this.nearZ.MaximumValue = 3.402823E+38F;
            this.nearZ.MinimumValue = -3.402823E+38F;
            this.nearZ.Name = "nearZ";
            this.nearZ.Size = new System.Drawing.Size(60, 20);
            this.nearZ.TabIndex = 10;
            this.nearZ.Text = "0";
            // 
            // yFov
            // 
            this.yFov.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.yFov.Integral = false;
            this.yFov.Location = new System.Drawing.Point(243, 41);
            this.yFov.MaximumValue = 3.402823E+38F;
            this.yFov.MinimumValue = -3.402823E+38F;
            this.yFov.Name = "yFov";
            this.yFov.Size = new System.Drawing.Size(60, 20);
            this.yFov.TabIndex = 9;
            this.yFov.Text = "0";
            // 
            // zScale
            // 
            this.zScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.zScale.Integral = false;
            this.zScale.Location = new System.Drawing.Point(105, 79);
            this.zScale.MaximumValue = 3.402823E+38F;
            this.zScale.MinimumValue = -3.402823E+38F;
            this.zScale.Name = "zScale";
            this.zScale.Size = new System.Drawing.Size(50, 20);
            this.zScale.TabIndex = 8;
            this.zScale.Text = "0";
            // 
            // tScale
            // 
            this.tScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tScale.Integral = false;
            this.tScale.Location = new System.Drawing.Point(105, 60);
            this.tScale.MaximumValue = 3.402823E+38F;
            this.tScale.MinimumValue = -3.402823E+38F;
            this.tScale.Name = "tScale";
            this.tScale.Size = new System.Drawing.Size(50, 20);
            this.tScale.TabIndex = 7;
            this.tScale.Text = "0";
            // 
            // rScale
            // 
            this.rScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rScale.Integral = false;
            this.rScale.Location = new System.Drawing.Point(105, 41);
            this.rScale.MaximumValue = 3.402823E+38F;
            this.rScale.MinimumValue = -3.402823E+38F;
            this.rScale.Name = "rScale";
            this.rScale.Size = new System.Drawing.Size(50, 20);
            this.rScale.TabIndex = 6;
            this.rScale.Text = "0";
            // 
            // label14
            // 
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Location = new System.Drawing.Point(154, 79);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(90, 20);
            this.label14.TabIndex = 5;
            this.label14.Text = "Far Z: ";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Location = new System.Drawing.Point(154, 60);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(90, 20);
            this.label13.TabIndex = 4;
            this.label13.Text = "Near Z: ";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Location = new System.Drawing.Point(154, 41);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(90, 20);
            this.label12.TabIndex = 3;
            this.label12.Text = "Y Field Of View: ";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Location = new System.Drawing.Point(6, 79);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 20);
            this.label11.TabIndex = 2;
            this.label11.Text = "Zoom Scale: ";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(6, 60);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 20);
            this.label10.TabIndex = 1;
            this.label10.Text = "Translation Scale: ";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(6, 41);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 20);
            this.label9.TabIndex = 0;
            this.label9.Text = "Rotation Scale: ";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpColors
            // 
            this.grpColors.Controls.Add(this.lblCol1Color);
            this.grpColors.Controls.Add(this.lblLineColor);
            this.grpColors.Controls.Add(this.lblCol1Text);
            this.grpColors.Controls.Add(this.lblLineText);
            this.grpColors.Controls.Add(this.label24);
            this.grpColors.Controls.Add(this.label20);
            this.grpColors.Controls.Add(this.lblOrbColor);
            this.grpColors.Controls.Add(this.lblOrbText);
            this.grpColors.Controls.Add(this.label15);
            this.grpColors.Location = new System.Drawing.Point(6, 6);
            this.grpColors.Name = "grpColors";
            this.grpColors.Size = new System.Drawing.Size(311, 84);
            this.grpColors.TabIndex = 37;
            this.grpColors.TabStop = false;
            this.grpColors.Text = "Display Colors";
            // 
            // lblCol1Color
            // 
            this.lblCol1Color.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCol1Color.Location = new System.Drawing.Point(263, 54);
            this.lblCol1Color.Name = "lblCol1Color";
            this.lblCol1Color.Size = new System.Drawing.Size(40, 20);
            this.lblCol1Color.TabIndex = 5;
            this.lblCol1Color.Click += new System.EventHandler(this.lblCol1Color_Click);
            // 
            // lblLineColor
            // 
            this.lblLineColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLineColor.Location = new System.Drawing.Point(263, 35);
            this.lblLineColor.Name = "lblLineColor";
            this.lblLineColor.Size = new System.Drawing.Size(40, 20);
            this.lblLineColor.TabIndex = 8;
            this.lblLineColor.Click += new System.EventHandler(this.lblLineColor_Click);
            // 
            // lblCol1Text
            // 
            this.lblCol1Text.BackColor = System.Drawing.Color.White;
            this.lblCol1Text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCol1Text.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCol1Text.Location = new System.Drawing.Point(105, 54);
            this.lblCol1Text.Name = "lblCol1Text";
            this.lblCol1Text.Size = new System.Drawing.Size(159, 20);
            this.lblCol1Text.TabIndex = 7;
            this.lblCol1Text.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLineText
            // 
            this.lblLineText.BackColor = System.Drawing.Color.White;
            this.lblLineText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLineText.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLineText.Location = new System.Drawing.Point(105, 35);
            this.lblLineText.Name = "lblLineText";
            this.lblLineText.Size = new System.Drawing.Size(159, 20);
            this.lblLineText.TabIndex = 10;
            this.lblLineText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label24
            // 
            this.label24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label24.Location = new System.Drawing.Point(6, 54);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(100, 20);
            this.label24.TabIndex = 6;
            this.label24.Text = "Floor Color:";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label20
            // 
            this.label20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label20.Location = new System.Drawing.Point(6, 35);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(100, 20);
            this.label20.TabIndex = 9;
            this.label20.Text = "Bone Line Color:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOrbColor
            // 
            this.lblOrbColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOrbColor.Location = new System.Drawing.Point(263, 16);
            this.lblOrbColor.Name = "lblOrbColor";
            this.lblOrbColor.Size = new System.Drawing.Size(40, 20);
            this.lblOrbColor.TabIndex = 5;
            this.lblOrbColor.Click += new System.EventHandler(this.lblOrbColor_Click);
            // 
            // lblOrbText
            // 
            this.lblOrbText.BackColor = System.Drawing.Color.White;
            this.lblOrbText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOrbText.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrbText.Location = new System.Drawing.Point(105, 16);
            this.lblOrbText.Name = "lblOrbText";
            this.lblOrbText.Size = new System.Drawing.Size(159, 20);
            this.lblOrbText.TabIndex = 7;
            this.lblOrbText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Location = new System.Drawing.Point(6, 16);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(100, 20);
            this.label15.TabIndex = 6;
            this.label15.Text = "Bone Orb Color:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(4, 367);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(145, 13);
            this.label18.TabIndex = 39;
            this.label18.Text = "Maximum Undo/Redo Count:";
            // 
            // chkRetrieveCorrAnims
            // 
            this.chkRetrieveCorrAnims.AutoSize = true;
            this.chkRetrieveCorrAnims.Location = new System.Drawing.Point(6, 99);
            this.chkRetrieveCorrAnims.Name = "chkRetrieveCorrAnims";
            this.chkRetrieveCorrAnims.Size = new System.Drawing.Size(246, 17);
            this.chkRetrieveCorrAnims.TabIndex = 40;
            this.chkRetrieveCorrAnims.Text = "Retreieve animations with corresponding name";
            this.chkRetrieveCorrAnims.UseVisualStyleBackColor = true;
            this.chkRetrieveCorrAnims.CheckedChanged += new System.EventHandler(this.chkRetrieveCorrAnims_CheckedChanged);
            // 
            // chkSyncTexToObj
            // 
            this.chkSyncTexToObj.AutoSize = true;
            this.chkSyncTexToObj.Location = new System.Drawing.Point(7, 96);
            this.chkSyncTexToObj.Name = "chkSyncTexToObj";
            this.chkSyncTexToObj.Size = new System.Drawing.Size(197, 17);
            this.chkSyncTexToObj.TabIndex = 41;
            this.chkSyncTexToObj.Text = "Sync texture list with selected object";
            this.chkSyncTexToObj.UseVisualStyleBackColor = true;
            this.chkSyncTexToObj.CheckedChanged += new System.EventHandler(this.chkSyncTexToObj_CheckedChanged);
            // 
            // chkSyncObjToVIS
            // 
            this.chkSyncObjToVIS.AutoSize = true;
            this.chkSyncObjToVIS.Location = new System.Drawing.Point(6, 122);
            this.chkSyncObjToVIS.Name = "chkSyncObjToVIS";
            this.chkSyncObjToVIS.Size = new System.Drawing.Size(272, 17);
            this.chkSyncObjToVIS.TabIndex = 42;
            this.chkSyncObjToVIS.Text = "Sync object list checkbox changes to selected VIS0";
            this.chkSyncObjToVIS.UseVisualStyleBackColor = true;
            this.chkSyncObjToVIS.CheckedChanged += new System.EventHandler(this.chkSyncObjToVIS_CheckedChanged);
            // 
            // chkDisableBonesOnPlay
            // 
            this.chkDisableBonesOnPlay.AutoSize = true;
            this.chkDisableBonesOnPlay.Location = new System.Drawing.Point(6, 145);
            this.chkDisableBonesOnPlay.Name = "chkDisableBonesOnPlay";
            this.chkDisableBonesOnPlay.Size = new System.Drawing.Size(204, 17);
            this.chkDisableBonesOnPlay.TabIndex = 43;
            this.chkDisableBonesOnPlay.Text = "Disable bones when playing animaton";
            this.chkDisableBonesOnPlay.UseVisualStyleBackColor = true;
            this.chkDisableBonesOnPlay.CheckedChanged += new System.EventHandler(this.chkDisableBonesOnPlay_CheckedChanged);
            // 
            // chkDisableHighlight
            // 
            this.chkDisableHighlight.AutoSize = true;
            this.chkDisableHighlight.Location = new System.Drawing.Point(7, 119);
            this.chkDisableHighlight.Name = "chkDisableHighlight";
            this.chkDisableHighlight.Size = new System.Drawing.Size(210, 17);
            this.chkDisableHighlight.TabIndex = 44;
            this.chkDisableHighlight.Text = "Disable realtime highlighting in viewport";
            this.chkDisableHighlight.UseVisualStyleBackColor = true;
            this.chkDisableHighlight.CheckedChanged += new System.EventHandler(this.chkDisableHighlight_CheckedChanged);
            // 
            // chkSnapBonesToFloor
            // 
            this.chkSnapBonesToFloor.AutoSize = true;
            this.chkSnapBonesToFloor.Location = new System.Drawing.Point(7, 142);
            this.chkSnapBonesToFloor.Name = "chkSnapBonesToFloor";
            this.chkSnapBonesToFloor.Size = new System.Drawing.Size(205, 17);
            this.chkSnapBonesToFloor.TabIndex = 46;
            this.chkSnapBonesToFloor.Text = "Snap dragged bones to floor collisions";
            this.chkSnapBonesToFloor.UseVisualStyleBackColor = true;
            this.chkSnapBonesToFloor.CheckedChanged += new System.EventHandler(this.chkSnapBonesToFloor_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(332, 419);
            this.tabControl1.TabIndex = 47;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.chkTextOverlays);
            this.tabPage1.Controls.Add(this.grpProjection);
            this.tabPage1.Controls.Add(this.grpLighting);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(324, 393);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Current Viewport";
            // 
            // chkTextOverlays
            // 
            this.chkTextOverlays.AutoSize = true;
            this.chkTextOverlays.Location = new System.Drawing.Point(8, 369);
            this.chkTextOverlays.Name = "chkTextOverlays";
            this.chkTextOverlays.Size = new System.Drawing.Size(121, 17);
            this.chkTextOverlays.TabIndex = 46;
            this.chkTextOverlays.Text = "Enable text overlays";
            this.chkTextOverlays.UseVisualStyleBackColor = true;
            this.chkTextOverlays.CheckedChanged += new System.EventHandler(this.chkTextOverlays_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chkPixelLighting);
            this.tabPage2.Controls.Add(this.chkHideMainWindow);
            this.tabPage2.Controls.Add(this.chkUsePointsAsBones);
            this.tabPage2.Controls.Add(this.chkScaleBones);
            this.tabPage2.Controls.Add(this.chkSaveWindowPosition);
            this.tabPage2.Controls.Add(this.chkMaximize);
            this.tabPage2.Controls.Add(this.grpColors);
            this.tabPage2.Controls.Add(this.chkSyncTexToObj);
            this.tabPage2.Controls.Add(this.chkDisableHighlight);
            this.tabPage2.Controls.Add(this.chkSnapBonesToFloor);
            this.tabPage2.Controls.Add(this.label18);
            this.tabPage2.Controls.Add(this.maxUndoCount);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(324, 393);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "General";
            // 
            // chkPixelLighting
            // 
            this.chkPixelLighting.AutoSize = true;
            this.chkPixelLighting.Location = new System.Drawing.Point(7, 280);
            this.chkPixelLighting.Name = "chkPixelLighting";
            this.chkPixelLighting.Size = new System.Drawing.Size(228, 17);
            this.chkPixelLighting.TabIndex = 52;
            this.chkPixelLighting.Text = "Per pixel lighting (as opposed to per vertex)";
            this.chkPixelLighting.UseVisualStyleBackColor = true;
            this.chkPixelLighting.CheckedChanged += new System.EventHandler(this.chkPixelLighting_CheckedChanged);
            // 
            // chkHideMainWindow
            // 
            this.chkHideMainWindow.AutoSize = true;
            this.chkHideMainWindow.Location = new System.Drawing.Point(7, 257);
            this.chkHideMainWindow.Name = "chkHideMainWindow";
            this.chkHideMainWindow.Size = new System.Drawing.Size(112, 17);
            this.chkHideMainWindow.TabIndex = 51;
            this.chkHideMainWindow.Text = "Hide main window";
            this.chkHideMainWindow.UseVisualStyleBackColor = true;
            this.chkHideMainWindow.CheckedChanged += new System.EventHandler(this.chkHideMainWindow_CheckedChanged);
            // 
            // chkUsePointsAsBones
            // 
            this.chkUsePointsAsBones.AutoSize = true;
            this.chkUsePointsAsBones.Location = new System.Drawing.Point(7, 234);
            this.chkUsePointsAsBones.Name = "chkUsePointsAsBones";
            this.chkUsePointsAsBones.Size = new System.Drawing.Size(137, 17);
            this.chkUsePointsAsBones.TabIndex = 50;
            this.chkUsePointsAsBones.Text = "Display bones as points";
            this.chkUsePointsAsBones.UseVisualStyleBackColor = true;
            this.chkUsePointsAsBones.CheckedChanged += new System.EventHandler(this.chkUsePointsAsBones_CheckedChanged);
            // 
            // chkScaleBones
            // 
            this.chkScaleBones.AutoSize = true;
            this.chkScaleBones.Location = new System.Drawing.Point(7, 211);
            this.chkScaleBones.Name = "chkScaleBones";
            this.chkScaleBones.Size = new System.Drawing.Size(145, 17);
            this.chkScaleBones.TabIndex = 49;
            this.chkScaleBones.Text = "Scale bones with camera";
            this.chkScaleBones.UseVisualStyleBackColor = true;
            this.chkScaleBones.CheckedChanged += new System.EventHandler(this.chkScaleBones_CheckedChanged);
            // 
            // chkSaveWindowPosition
            // 
            this.chkSaveWindowPosition.AutoSize = true;
            this.chkSaveWindowPosition.Location = new System.Drawing.Point(7, 188);
            this.chkSaveWindowPosition.Name = "chkSaveWindowPosition";
            this.chkSaveWindowPosition.Size = new System.Drawing.Size(205, 17);
            this.chkSaveWindowPosition.TabIndex = 48;
            this.chkSaveWindowPosition.Text = "Save window position and dimensions";
            this.chkSaveWindowPosition.UseVisualStyleBackColor = true;
            this.chkSaveWindowPosition.CheckedChanged += new System.EventHandler(this.chkSaveWindowPosition_CheckedChanged);
            // 
            // chkMaximize
            // 
            this.chkMaximize.AutoSize = true;
            this.chkMaximize.Location = new System.Drawing.Point(7, 165);
            this.chkMaximize.Name = "chkMaximize";
            this.chkMaximize.Size = new System.Drawing.Size(176, 17);
            this.chkMaximize.TabIndex = 47;
            this.chkMaximize.Text = "Maximize window upon opening";
            this.chkMaximize.UseVisualStyleBackColor = true;
            this.chkMaximize.CheckedChanged += new System.EventHandler(this.chkMaximize_CheckedChanged);
            // 
            // maxUndoCount
            // 
            this.maxUndoCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.maxUndoCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxUndoCount.Integral = false;
            this.maxUndoCount.Location = new System.Drawing.Point(155, 365);
            this.maxUndoCount.MaximumValue = 3.402823E+38F;
            this.maxUndoCount.MinimumValue = -3.402823E+38F;
            this.maxUndoCount.Name = "maxUndoCount";
            this.maxUndoCount.Size = new System.Drawing.Size(66, 20);
            this.maxUndoCount.TabIndex = 37;
            this.maxUndoCount.Text = "0";
            this.maxUndoCount.ValueChanged += new System.EventHandler(this.maxUndoCount_ValueChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Controls.Add(this.chkPrecalcBoxes);
            this.tabPage3.Controls.Add(this.chkDisableBonesOnPlay);
            this.tabPage3.Controls.Add(this.chkRetrieveCorrAnims);
            this.tabPage3.Controls.Add(this.chkSyncObjToVIS);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(324, 393);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Animation";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkTanCam);
            this.groupBox1.Controls.Add(this.chkTanFog);
            this.groupBox1.Controls.Add(this.chkTanLight);
            this.groupBox1.Controls.Add(this.chkTanSHP);
            this.groupBox1.Controls.Add(this.chkTanSRT);
            this.groupBox1.Controls.Add(this.chkTanCHR);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(309, 87);
            this.groupBox1.TabIndex = 49;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Generate Tangents";
            // 
            // chkTanCam
            // 
            this.chkTanCam.AutoSize = true;
            this.chkTanCam.Location = new System.Drawing.Point(67, 65);
            this.chkTanCam.Name = "chkTanCam";
            this.chkTanCam.Size = new System.Drawing.Size(93, 17);
            this.chkTanCam.TabIndex = 55;
            this.chkTanCam.Text = "SCN0 Camera";
            this.chkTanCam.UseVisualStyleBackColor = true;
            this.chkTanCam.CheckedChanged += new System.EventHandler(this.chkTanCam_CheckedChanged);
            // 
            // chkTanFog
            // 
            this.chkTanFog.AutoSize = true;
            this.chkTanFog.Location = new System.Drawing.Point(67, 42);
            this.chkTanFog.Name = "chkTanFog";
            this.chkTanFog.Size = new System.Drawing.Size(75, 17);
            this.chkTanFog.TabIndex = 54;
            this.chkTanFog.Text = "SCN0 Fog";
            this.chkTanFog.UseVisualStyleBackColor = true;
            this.chkTanFog.CheckedChanged += new System.EventHandler(this.chkTanFog_CheckedChanged);
            // 
            // chkTanLight
            // 
            this.chkTanLight.AutoSize = true;
            this.chkTanLight.Location = new System.Drawing.Point(67, 19);
            this.chkTanLight.Name = "chkTanLight";
            this.chkTanLight.Size = new System.Drawing.Size(80, 17);
            this.chkTanLight.TabIndex = 53;
            this.chkTanLight.Text = "SCN0 Light";
            this.chkTanLight.UseVisualStyleBackColor = true;
            this.chkTanLight.CheckedChanged += new System.EventHandler(this.chkTanLight_CheckedChanged);
            // 
            // chkTanSHP
            // 
            this.chkTanSHP.AutoSize = true;
            this.chkTanSHP.Location = new System.Drawing.Point(6, 65);
            this.chkTanSHP.Name = "chkTanSHP";
            this.chkTanSHP.Size = new System.Drawing.Size(54, 17);
            this.chkTanSHP.TabIndex = 52;
            this.chkTanSHP.Text = "SHP0";
            this.chkTanSHP.UseVisualStyleBackColor = true;
            this.chkTanSHP.CheckedChanged += new System.EventHandler(this.chkTanSHP_CheckedChanged);
            // 
            // chkTanSRT
            // 
            this.chkTanSRT.AutoSize = true;
            this.chkTanSRT.Location = new System.Drawing.Point(6, 42);
            this.chkTanSRT.Name = "chkTanSRT";
            this.chkTanSRT.Size = new System.Drawing.Size(54, 17);
            this.chkTanSRT.TabIndex = 51;
            this.chkTanSRT.Text = "SRT0";
            this.chkTanSRT.UseVisualStyleBackColor = true;
            this.chkTanSRT.CheckedChanged += new System.EventHandler(this.chkTanSRT_CheckedChanged);
            // 
            // chkTanCHR
            // 
            this.chkTanCHR.AutoSize = true;
            this.chkTanCHR.Location = new System.Drawing.Point(6, 19);
            this.chkTanCHR.Name = "chkTanCHR";
            this.chkTanCHR.Size = new System.Drawing.Size(55, 17);
            this.chkTanCHR.TabIndex = 50;
            this.chkTanCHR.Text = "CHR0";
            this.chkTanCHR.UseVisualStyleBackColor = true;
            this.chkTanCHR.CheckedChanged += new System.EventHandler(this.chkTanCHR_CheckedChanged);
            // 
            // chkPrecalcBoxes
            // 
            this.chkPrecalcBoxes.AutoSize = true;
            this.chkPrecalcBoxes.Location = new System.Drawing.Point(6, 168);
            this.chkPrecalcBoxes.Name = "chkPrecalcBoxes";
            this.chkPrecalcBoxes.Size = new System.Drawing.Size(258, 17);
            this.chkPrecalcBoxes.TabIndex = 48;
            this.chkPrecalcBoxes.Text = "Display precalculated bounding boxes on frame 0";
            this.chkPrecalcBoxes.UseVisualStyleBackColor = true;
            this.chkPrecalcBoxes.CheckedChanged += new System.EventHandler(this.chkPrecalcBoxes_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnResetSettings);
            this.panel1.Controls.Add(this.btnImportSettings);
            this.panel1.Controls.Add(this.btnExportSettings);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOkay);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 419);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(332, 35);
            this.panel1.TabIndex = 47;
            // 
            // btnResetSettings
            // 
            this.btnResetSettings.Location = new System.Drawing.Point(202, 6);
            this.btnResetSettings.Name = "btnResetSettings";
            this.btnResetSettings.Size = new System.Drawing.Size(60, 23);
            this.btnResetSettings.TabIndex = 5;
            this.btnResetSettings.Text = "Reset";
            this.btnResetSettings.UseVisualStyleBackColor = true;
            this.btnResetSettings.Click += new System.EventHandler(this.btnResetSettings_Click);
            // 
            // btnImportSettings
            // 
            this.btnImportSettings.Location = new System.Drawing.Point(70, 6);
            this.btnImportSettings.Name = "btnImportSettings";
            this.btnImportSettings.Size = new System.Drawing.Size(60, 23);
            this.btnImportSettings.TabIndex = 4;
            this.btnImportSettings.Text = "Import";
            this.btnImportSettings.UseVisualStyleBackColor = true;
            this.btnImportSettings.Visible = false;
            this.btnImportSettings.Click += new System.EventHandler(this.btnImportSettings_Click);
            // 
            // btnExportSettings
            // 
            this.btnExportSettings.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExportSettings.Location = new System.Drawing.Point(4, 6);
            this.btnExportSettings.Name = "btnExportSettings";
            this.btnExportSettings.Size = new System.Drawing.Size(60, 23);
            this.btnExportSettings.TabIndex = 3;
            this.btnExportSettings.Text = "Export";
            this.btnExportSettings.UseVisualStyleBackColor = true;
            this.btnExportSettings.Visible = false;
            this.btnExportSettings.Click += new System.EventHandler(this.btnExportSettings_Click);
            // 
            // ModelViewerSettingsDialog
            // 
            this.ClientSize = new System.Drawing.Size(332, 454);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.Name = "ModelViewerSettingsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Model Editor Settings";
            this.grpLighting.ResumeLayout(false);
            this.grpLighting.PerformLayout();
            this.grpProjection.ResumeLayout(false);
            this.grpProjection.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpColors.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private GoodColorDialog _dlgColor;
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

        private void UpdateOrb()
        {
            lblOrbText.Text = ((ARGBPixel)MDL0BoneNode.DefaultNodeColor).ToString();
            lblOrbColor.BackColor = Color.FromArgb(MDL0BoneNode.DefaultNodeColor.R, MDL0BoneNode.DefaultNodeColor.G, MDL0BoneNode.DefaultNodeColor.B);

            if (!_updating)
                _form.ModelPanel.Invalidate();
        }
        private void UpdateLine()
        {
            lblLineText.Text = ((ARGBPixel)MDL0BoneNode.DefaultLineColor).ToString();
            lblLineColor.BackColor = Color.FromArgb(MDL0BoneNode.DefaultLineColor.R, MDL0BoneNode.DefaultLineColor.G, MDL0BoneNode.DefaultLineColor.B);

            if (!_updating)
                _form.ModelPanel.Invalidate();
        }
        private void UpdateCol1()
        {
            lblCol1Text.Text = ((ARGBPixel)ModelEditorBase._floorHue).ToString();
            lblCol1Color.BackColor = Color.FromArgb(ModelEditorBase._floorHue.R, ModelEditorBase._floorHue.G, ModelEditorBase._floorHue.B);
            
            if (!_updating)
                _form.ModelPanel.Invalidate();
        }
        private void UpdateAmb()
        {
            label19.BackColor = Color.FromArgb(255, (int)(ax.Value), (int)(ay.Value), (int)(az.Value));
            
            if (!_updating)
                _form.ModelPanel.CurrentViewport.Ambient = new Vector4(ax.Value / 255.0f, ay.Value / 255.0f, az.Value / 255.0f, 1.0f);
        }
        private void UpdateDif()
        {
            label21.BackColor = Color.FromArgb(255, (int)(dx.Value), (int)(dy.Value), (int)(dz.Value));
            
            if (!_updating)
                _form.ModelPanel.CurrentViewport.Diffuse = new Vector4(dx.Value / 255.0f, dy.Value / 255.0f, dz.Value / 255.0f, 1.0f);
        }
        private void UpdateSpe()
        {
            label22.BackColor = Color.FromArgb(255, (int)(sx.Value), (int)(sy.Value), (int)(sz.Value));
            
            if (!_updating)
                _form.ModelPanel.CurrentViewport.Specular = new Vector4(sx.Value / 255.0f, sy.Value / 255.0f, sz.Value / 255.0f, 1.0f);
        }
        private void UpdateEmi()
        {
            label23.BackColor = Color.FromArgb(255, (int)(ex.Value), (int)(ey.Value), (int)(ez.Value));
            
            if (!_updating)
                _form.ModelPanel.CurrentViewport.Emission = new Vector4(ex.Value / 255.0f, ey.Value / 255.0f, ez.Value / 255.0f, 1.0f);
        }
        public bool _updating = false;
        private void label19_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = Color.FromArgb(255, (int)(ax.Value), (int)(ay.Value), (int)(az.Value));
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                _updating = true;
                ax.Value = (float)_dlgColor.Color.R;
                ay.Value = (float)_dlgColor.Color.G;
                az.Value = (float)_dlgColor.Color.B;
                _updating = false;
                UpdateAmb();
            }
        }

        private void label21_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = Color.FromArgb(255, (int)(dx.Value), (int)(dy.Value), (int)(dz.Value));
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                _updating = true;
                dx.Value = (float)_dlgColor.Color.R;
                dy.Value = (float)_dlgColor.Color.G;
                dz.Value = (float)_dlgColor.Color.B;
                _updating = false;
                UpdateDif();
            }
        }

        private void label22_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = Color.FromArgb(255, (int)(sx.Value), (int)(sy.Value), (int)(sz.Value));
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                _updating = true;
                sx.Value = (float)_dlgColor.Color.R;
                sy.Value = (float)_dlgColor.Color.G;
                sz.Value = (float)_dlgColor.Color.B;
                _updating = false;
                UpdateSpe();
            }
        }

        private void label23_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = Color.FromArgb(255, (int)(ex.Value), (int)(ey.Value), (int)(ez.Value));
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                _updating = true;
                ex.Value = (float)_dlgColor.Color.R;
                ey.Value = (float)_dlgColor.Color.G;
                ez.Value = (float)_dlgColor.Color.B;
                _updating = false;
                UpdateEmi();
            }
        }

        private void cboProjection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                _form.ModelPanel.CurrentViewport.SetProjectionType((BrawlLib.OpenGL.ViewportProjection)cboProjection.SelectedIndex);
                UpdateViewport(_form.ModelPanel.CurrentViewport);
            }
        }

        private void chkRetrieveCorrAnims_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                if (_form.RetrieveCorrespondingAnimations = chkRetrieveCorrAnims.Checked)
                    _form.GetFiles(_form.TargetAnimType);
                else
                    _form.GetFiles(NW4RAnimType.None);
        }

        private void chkSyncTexToObj_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                _form.SyncTexturesToObjectList = chkSyncTexToObj.Checked;
        }

        private void chkSyncObjToVIS_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                _form.SyncVIS0 = chkSyncObjToVIS.Checked;
        }

        private void chkDisableBonesOnPlay_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                _form.DisableBonesWhenPlaying = chkDisableBonesOnPlay.Checked;
        }

        private void chkDisableHighlight_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                _form.DoNotHighlightOnMouseMove = chkDisableHighlight.Checked;
        }

        private void chkSnapBonesToFloor_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                _form.SnapBonesToCollisions = chkSnapBonesToFloor.Checked;
        }

        private void chkMaximize_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                _form._maximize = chkMaximize.Checked;
        }

        private void chkPrecalcBoxes_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                _form.UseBindStateBoxes = chkPrecalcBoxes.Checked;
        }

        private void chkTanCHR_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                CHR0EntryNode._generateTangents = chkTanCHR.Checked;
        }

        private void chkTanSRT_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                SRT0TextureNode._generateTangents = chkTanSRT.Checked;
        }

        private void chkTanSHP_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                SHP0VertexSetNode._generateTangents = chkTanSHP.Checked;
        }

        private void chkTanCam_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                SCN0CameraNode._generateTangents = chkTanCam.Checked;
        }

        private void chkTanFog_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                SCN0FogNode._generateTangents = chkTanFog.Checked;
        }

        private void chkTanLight_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                SCN0LightNode._generateTangents = chkTanLight.Checked;
        }

        private void btnResetSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure you want to reset all settings to default?", "Reset?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != Forms.DialogResult.OK)
                return;

            BrawlBox.Properties.Settings.Default.ViewerSettings = null;
            BrawlBox.Properties.Settings.Default.ViewerSettingsSet = false;
            _form.SetDefaultSettings();
            UpdateAll();
        }

        private void btnImportSettings_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            od.Filter = "Brawlbox Settings (*.settings)|*.settings";
            od.FileName = Application.StartupPath;
            if (od.ShowDialog() == DialogResult.OK)
            {
                string path = od.FileName;
                ModelEditorSettings settings = Serializer.DeserializeObject(path) as ModelEditorSettings;
                _form.DistributeSettings(settings);
            }
        }

        private void btnExportSettings_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "Brawlbox Settings (*.settings)|*.settings";
            sd.FileName = Application.StartupPath;
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
            if (!_updating)
                _form.ModelPanel.CurrentViewport.TextOverlaysEnabled = chkTextOverlays.Checked;
        }

        private void maxUndoCount_ValueChanged(object sender, EventArgs e)
        {
            if (!_updating)
                _form._allowedUndos = (uint)maxUndoCount.Value;
        }

        private void chkSaveWindowPosition_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                _form._savePosition = chkSaveWindowPosition.Checked;
        }

        private void chkDefaultPos_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkDefaultPos.Checked)
                return;

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
                return;

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
            if (!_updating)
                _form.ModelPanel.CurrentViewport.LightEnabled = chkLightEnabled.Checked;
        }

        private void chkLightDirectional_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                _form.ModelPanel.CurrentViewport.LightDirectional = chkLightDirectional.Checked;
        }

        private void chkScaleBones_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                _form.ModelPanel.CurrentViewport.ScaleBones = chkScaleBones.Checked;
        }

        private void chkUsePointsAsBones_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                _form.ModelPanel.CurrentViewport.RenderBonesAsPoints = chkUsePointsAsBones.Checked;
        }

        private void chkHideMainWindow_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            
            MainForm.Instance.Visible = !(_form._hideMainWindow = chkHideMainWindow.Checked);
            foreach (ModelEditControl c in ModelEditControl.Instances)
                c._hideMainWindow = _form._hideMainWindow;
            _form.SaveSettings();
        }

        private void chkPixelLighting_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            BrawlBox.Properties.Settings.Default.PixelLighting = 
                ShaderGenerator.UsePixelLighting = chkPixelLighting.Checked;
            _form.ModelPanel.Invalidate();
            BrawlBox.Properties.Settings.Default.Save();
        }
    }
}
