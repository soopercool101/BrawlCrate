using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Internal.Windows.Controls.ModelViewer.Editors;
using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Animations;
using BrawlLib.Wii.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BrawlCrate.UI.Model_Previewer.ModelEditControl
{
    public partial class ModelEditControl : ModelEditorBase
    {
        public override void UpdatePropDisplay()
        {
            //if (vertexEditor.Visible)
            //{
            //    vertexEditor.UpdatePropDisplay();
            //    return;
            //}
            base.UpdatePropDisplay();
        }

        public override void AppendTarget(IModel model)
        {
            if (!_targetModels.Contains(model))
            {
                _targetModels.Add(model);
            }

            ModelPanel.AddTarget(model, false);
            model.ResetToBindState();
        }

        private void cboToolSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            _updating = true;
            switch (ControlType)
            {
                case TransformType.Scale:
                    rotationToolStripMenuItem.Checked = false;
                    translationToolStripMenuItem.Checked = false;
                    scaleToolStripMenuItem.Checked = true;
                    break;
                case TransformType.Rotation:
                    translationToolStripMenuItem.Checked = false;
                    scaleToolStripMenuItem.Checked = false;
                    rotationToolStripMenuItem.Checked = true;
                    break;
                case TransformType.Translation:
                    rotationToolStripMenuItem.Checked = false;
                    scaleToolStripMenuItem.Checked = false;
                    translationToolStripMenuItem.Checked = true;
                    break;
                default: //TransformType.None
                    rotationToolStripMenuItem.Checked = false;
                    translationToolStripMenuItem.Checked = false;
                    scaleToolStripMenuItem.Checked = false;
                    break;
            }

            _updating = false;

            _boneSelection.ResetAll();
            _vertexSelection.ResetAll();
            ModelPanel.Invalidate();
        }

        protected override void OnModelChanged()
        {
            _updating = true;
            if (_targetModel != null && TargetCollision == null)
            {
                models.SelectedItem = _targetModel;
            }

            leftPanel.Reset();
            rightPanel.Reset();

            weightEditor.TargetVertices = _selectedVertices;
            vertexEditor.TargetVertices = _selectedVertices;

            _updating = false;
        }

        private void models_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }


            object item = models.SelectedItem;

            _resetCamera = false;
            TargetModel = item is IModel model ? model : null;
            TargetCollision = item is CollisionNode node ? node : null;

            _undoSaves.Clear();
            _redoSaves.Clear();
            _saveIndex = -1;
        }

        private void ModelEditControl_SizeChanged(object sender, EventArgs e)
        {
            CheckDimensions();
        }

        public void CheckDimensions()
        {
            int totalWidth = animEditors.Width;
            System.Drawing.Size s = new System.Drawing.Size(animCtrlPnl.Width, animEditors.Height);
            if (_currentControl != null && _currentControl.Visible)
            {
                s = _currentControl.Visible ? _currentControl is SCN0Editor ? scn0Editor.GetDimensions() :
                    _currentControl.MinimumSize :
                    !weightEditor.Visible && !vertexEditor.Visible ? new System.Drawing.Size(0, 0) : s;
            }
            else if (!weightEditor.Visible && !vertexEditor.Visible)
            {
                s = new System.Drawing.Size(0, 0);
            }
            else if (weightEditor.Visible)
            {
                s = weightEditor.MinimumSize;
            }
            else if (vertexEditor.Visible)
            {
                s = vertexEditor.MinimumSize;
            }

            //See if the scroll bar needs to be visible
            int addedHeight = 0;
            if (s.Width + pnlPlayback.MinimumSize.Width > totalWidth)
            {
                addedHeight = 17;
                animEditors.HorizontalScroll.Visible = true;
            }
            else
            {
                animEditors.HorizontalScroll.Visible = false;
            }


            animCtrlPnl.Width = s.Width;
            animEditors.Height = s.Height + addedHeight;

            //Dock playback panel if it reaches its minimum size
            if (pnlPlayback.Width <= pnlPlayback.MinimumSize.Width)
            {
                pnlPlayback.Dock = DockStyle.Left;
                pnlPlayback.Width = pnlPlayback.MinimumSize.Width;
            }
            else
            {
                pnlPlayback.Dock = DockStyle.Fill;
            }

            //Stretch playback panel if there's space
            if (animEditors.Width - animCtrlPnl.Width >= pnlPlayback.MinimumSize.Width)
            {
                pnlPlayback.Width += animEditors.Width - animCtrlPnl.Width - pnlPlayback.MinimumSize.Width;
                pnlPlayback.Dock = DockStyle.Fill;
            }
            else
            {
                pnlPlayback.Dock = DockStyle.Left;
            }
        }

        public bool Close()
        {
            StopAnim();

            if (!rightPanel.pnlOpenedFiles.CloseAllFiles())
            {
                return false;
            }

            ResetBoneColors();
            SaveSettings();

            _viewerForm?.Close();

            _interpolationForm?.Close();

            MDL0TextureNode._folderWatcher.SynchronizingObject = null;

            TargetModel = null;

            _targetModels.Clear();
            ModelPanel.ClearAll();

            if (Instances.Contains(this))
            {
                Instances.Remove(this);
            }

            return true;
        }

        protected override void OnSelectedBoneChanged()
        {
            weightEditor.BoneChanged();
            chkZoomExtents.Enabled = AllowZoomExtents;
        }

        public override void UpdateUndoButtons()
        {
            btnUndo.Enabled = CanUndo;
            btnRedo.Enabled = CanRedo;
        }

        public override void UpdateAnimationPanelDimensions()
        {
            if (_currentControl is SCN0Editor)
            {
                System.Drawing.Size s = scn0Editor.GetDimensions();
                animEditors.Height = s.Height;
                animCtrlPnl.Width = s.Width;
            }
        }

        public void SetCurrentControl()
        {
            Control newControl;
            SyncTexturesToObjectList = TargetAnimType == NW4RAnimType.SRT || TargetAnimType == NW4RAnimType.PAT;
            switch (TargetAnimType)
            {
                case NW4RAnimType.CHR:
                    newControl = chr0Editor;
                    break;
                case NW4RAnimType.SHP:
                    newControl = shp0Editor;
                    break;
                case NW4RAnimType.VIS:
                    newControl = vis0Editor;
                    break;
                case NW4RAnimType.SCN:
                    newControl = scn0Editor;
                    break;
                case NW4RAnimType.CLR:
                    newControl = clr0Editor;
                    break;
                case NW4RAnimType.SRT:
                    newControl = srt0Editor;
                    break;
                case NW4RAnimType.PAT:
                    newControl = pat0Editor;
                    break;
                default:
                    newControl = null;
                    break;
            }

            if (_currentControl != newControl)
            {
                if (_currentControl != null)
                {
                    _currentControl.Visible = false;
                }

                _currentControl = newControl;

                if (!(_currentControl is SRT0Editor) && !(_currentControl is PAT0Editor))
                {
                    SyncTexturesToObjectList = false;
                }

                if (_currentControl != null)
                {
                    _currentControl.Visible = true;
                }
            }

            AnimChanged();
            CheckDimensions();
            UpdateEditor();
            UpdatePropDisplay();
        }

        protected override void UpdateSRT0FocusControls(SRT0Node node)
        {
            leftPanel.UpdateSRT0Selection(node);
        }

        protected override void UpdatePAT0FocusControls(PAT0Node node)
        {
            leftPanel.UpdatePAT0Selection(node);
        }

        protected override unsafe void modelPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            base.modelPanel1_MouseMove(sender, e);

            if (_boneSelection._translating &&
                SelectedBone != null &&
                SnapBonesToCollisions)
            {
                SnapYIfClose();
            }
        }

        //protected override void modelPanel1_MouseUp(object sender, MouseEventArgs e)
        //{
        //    base.modelPanel1_MouseUp(sender, e);

        //    if (e.Button == Forms.MouseButtons.Left)
        //    {

        //    }
        //}

        public override void ApplyVIS0ToInterface()
        {
            //base.ApplyVIS0ToInterface();
            //return;

            if (_animFrame == 0 || leftPanel.lstObjects.Items.Count == 0)
            {
                return;
            }

            VIS0Updating = true;
            if (_vis0 != null)
            {
                //if (TargetAnimation != null && _vis0.FrameCount != TargetAnimation.tFrameCount)
                //    UpdateVis0(null, null);

                foreach (string boneName in VIS0Indices.Keys)
                {
                    MDL0ObjectNode obj;
                    VIS0EntryNode node = null;
                    Dictionary<int, List<int>> objects = VIS0Indices[boneName];
                    foreach (KeyValuePair<int, List<int>> objKey in objects)
                    {
                        obj = (MDL0ObjectNode) leftPanel.lstObjects.Items[objKey.Key];
                        foreach (int i in objKey.Value)
                        {
                            node = _vis0.FindChild(obj._drawCalls[i].VisibilityBone, true) as VIS0EntryNode;
                            if (node != null)
                            {
                                bool render = node._entryCount != 0 && _animFrame > 0
                                    ? node.GetEntry(_animFrame - 1)
                                    : node._flags.HasFlag(VIS0Flags.Enabled);

                                if (leftPanel.InvokeRequired)
                                {
                                    Action<int, int, bool, MDL0ObjectNode> d =
                                        new Action<int, int, bool, MDL0ObjectNode>(leftPanel.SetRenderState);
                                    Invoke(d, new object[] {objKey.Key, i, render, obj});
                                }
                                else
                                {
                                    leftPanel.SetRenderState(objKey.Key, i, render, obj);
                                }
                            }
                        }
                    }
                }
            }

            VIS0Updating = false;
        }

        #region Hotkeys

        private bool HotkeySelectAllVertices()
        {
            if (!ModelPanel.Focused)
            {
                return false;
            }

            ClearSelectedVertices();
            if (EditingAll)
            {
                if (_targetModels != null)
                {
                    foreach (IModel mdl in _targetModels)
                    {
                        SelectAllVertices(mdl);
                    }
                }
            }
            else if (TargetModel != null)
            {
                SelectAllVertices(TargetModel);
            }

            OnSelectedVerticesChanged();

            weightEditor.TargetVertices = _selectedVertices;
            vertexEditor.TargetVertices = _selectedVertices;

            ModelPanel.Invalidate();

            return true;
        }

        private bool HotkeyToggleLeftPanel()
        {
            if (ModelPanel.Focused)
            {
                btnLeftToggle_Click(this, EventArgs.Empty);
                return true;
            }

            return false;
        }

        private bool HotkeyToggleTopPanel()
        {
            if (ModelPanel.Focused)
            {
                btnTopToggle_Click(this, EventArgs.Empty);
                return true;
            }

            return false;
        }

        private bool HotkeyToggleRightPanel()
        {
            if (ModelPanel.Focused)
            {
                btnRightToggle_Click(this, EventArgs.Empty);
                return true;
            }

            return false;
        }

        private bool HotkeyToggleBottomPanel()
        {
            if (ModelPanel.Focused)
            {
                btnBottomToggle_Click(this, EventArgs.Empty);
                return true;
            }

            return false;
        }

        private bool HotkeyToggleAllPanels()
        {
            if (ModelPanel.Focused)
            {
                if (leftPanel.Visible || rightPanel.Visible || animEditors.Visible || controlPanel.Visible)
                {
                    showBottom.Checked = showRight.Checked = showLeft.Checked = showTop.Checked = false;
                }
                else
                {
                    showBottom.Checked = showRight.Checked = showLeft.Checked = showTop.Checked = true;
                }

                return true;
            }

            return false;
        }

        private bool HotkeyScaleTool()
        {
            if (ModelPanel.Focused)
            {
                ControlType = TransformType.Scale;
                return true;
            }

            return false;
        }

        private bool HotkeyRotateTool()
        {
            if (ModelPanel.Focused)
            {
                ControlType = TransformType.Rotation;
                return true;
            }

            return false;
        }

        private bool HotkeyTranslateTool()
        {
            if (ModelPanel.Focused)
            {
                ControlType = TransformType.Translation;
                return true;
            }

            return false;
        }

        private bool HotkeyWeightEditor()
        {
            if (ModelPanel.Focused)
            {
                ToggleWeightEditor();
                return true;
            }

            return false;
        }

        private bool HotkeyVertexEditor()
        {
            if (ModelPanel.Focused)
            {
                ToggleVertexEditor();
                return true;
            }

            return false;
        }

        private bool HotkeyOverlays()
        {
            if (ModelPanel.Focused)
            {
                chkAllOverlays.Checked = !chkAllOverlays.Checked;
                chkItems.Checked = chkAllOverlays.Checked;
                chkSpawns.Checked = chkAllOverlays.Checked;
                chkBoundaries.Checked = chkAllOverlays.Checked;
                ModelPanel.Invalidate();
                return true;
            }

            return false;
        }

        public override void InitHotkeyList()
        {
            base.InitHotkeyList();

            List<HotKeyInfo> temp = new List<HotKeyInfo>
            {
                new HotKeyInfo(Keys.A, true, false, false, HotkeySelectAllVertices),
                new HotKeyInfo(Keys.A, false, false, false, HotkeyToggleLeftPanel),
                new HotKeyInfo(Keys.D, false, false, false, HotkeyToggleRightPanel),
                new HotKeyInfo(Keys.W, false, false, false, HotkeyToggleTopPanel),
                new HotKeyInfo(Keys.S, false, false, false, HotkeyToggleBottomPanel),
                new HotKeyInfo(Keys.D, true, true, false, HotkeyToggleAllPanels),
                new HotKeyInfo(Keys.E, false, false, false, HotkeyScaleTool),
                new HotKeyInfo(Keys.R, false, false, false, HotkeyRotateTool),
                new HotKeyInfo(Keys.T, false, false, false, HotkeyTranslateTool),
                new HotKeyInfo(Keys.D0, false, false, false, HotkeyVertexEditor),
                new HotKeyInfo(Keys.D9, false, false, false, HotkeyWeightEditor),
                new HotKeyInfo(Keys.D5, false, false, false, HotkeyOverlays)
            };
            _hotkeyList.AddRange(temp);
        }

        #endregion

        #region Collisions

        private bool PointCollides(Vector3 point)
        {
            return PointCollides(point, out float f);
        }

        private bool PointCollides(Vector3 point, out float y_result)
        {
            y_result = float.MaxValue;
            Vector2 v2 = new Vector2(point._x, point._y);
            foreach (CollisionNode coll in _collisions)
            {
                foreach (CollisionObject obj in coll.Children)
                {
                    if (obj._render)
                    {
                        foreach (CollisionPlane plane in obj._planes)
                        {
                            if (plane._type == CollisionPlaneType.Floor && plane.IsCharacters)
                            {
                                if (plane.PointLeft._x <= v2._x && plane.PointRight._x >= v2._x)
                                {
                                    float x = v2._x;
                                    float m = (plane.PointLeft._y - plane.PointRight._y)
                                              / (plane.PointLeft._x - plane.PointRight._x);
                                    float b = plane.PointRight._y - m * plane.PointRight._x;
                                    float y_target = m * x + b;
                                    //Console.WriteLine(y_target);
                                    if (Math.Abs(y_target - v2._y) <= Math.Abs(y_result - v2._y))
                                    {
                                        y_result = y_target;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return Math.Abs(y_result - v2._y) <= 5;
        }

        private void SnapYIfClose()
        {
            if (PointCollides(
                new Vector3(chr0Editor._transBoxes[6].Value, chr0Editor._transBoxes[7].Value,
                    chr0Editor._transBoxes[8].Value), out float f))
            {
                ApplyTranslation(1, f - chr0Editor._transBoxes[7].Value);
            }
        }

        #endregion

        #region Settings

        public override void SaveSettings()
        {
            BrawlCrate.Properties.Settings.Default.ViewerSettings = CollectSettings();
            BrawlCrate.Properties.Settings.Default.ViewerSettingsSet = true;
            BrawlCrate.Properties.Settings.Default.Save();
        }

        public ModelEditorSettings CollectSettings()
        {
            ModelEditorSettings settings = new ModelEditorSettings
            {
                RetrieveCorrAnims = RetrieveCorrespondingAnimations,
                DisplayExternalAnims = chkExternalAnims.Checked,
                DisplayBRRESAnims = chkBRRESAnims.Checked,
                DisplayNonBRRESAnims = chkNonBRRESAnims.Checked,
                SyncTexToObj = SyncTexturesToObjectList,
                SyncObjToVIS0 = SyncVIS0,
                DisableBonesOnPlay = DisableBonesWhenPlaying,
                GenTansCHR = CHR0EntryNode._generateTangents,
                GenTansSRT = SRT0TextureNode._generateTangents,
                GenTansSHP = SHP0VertexSetNode._generateTangents,
                GenTansLight = SCN0LightNode._generateTangents,
                GenTansFog = SCN0FogNode._generateTangents,
                GenTansCam = SCN0CameraNode._generateTangents,
                FlatBoneList = rightPanel.pnlBones.chkFlat.Checked,
                BoneListContains = rightPanel.pnlBones.chkContains.Checked,
                SnapToColl = _snapToCollisions,
                Maximize = _maximize,
                UseBindStateBox = UseBindStateBoxes,
                UsePixelLighting = ShaderGenerator.UsePixelLighting,

                HideMainWindow = _hideMainWindow,
                SavePosition = _savePosition,
                _width = ParentForm.Width,
                _height = ParentForm.Height,
                _posX = ParentForm.Location.X,
                _posY = ParentForm.Location.Y,

                _orbColor = (ARGBPixel) MDL0BoneNode.DefaultNodeColor,
                _lineColor = (ARGBPixel) MDL0BoneNode.DefaultLineColor,
                _lineDeselectedColor = (ARGBPixel) MDL0BoneNode.DefaultLineDeselectedColor,
                _floorColor = (ARGBPixel) _floorHue,

                _undoCount = _allowedUndos,
                _imageCapFmt = _imgType,
                _rightPanelWidth = (uint) rightPanel.Width,

                _screenCapPath = ScreenCapBgLocText.Text,
                _liveTexFolderPath = LiveTextureFolderPath.Text,

                _bgColor = BrawlCrate.Properties.Settings.Default.ViewerSettings._bgColor,
                _stgBgColor = BrawlCrate.Properties.Settings.Default.ViewerSettings._stgBgColor,

                _viewports = ModelPanel.Select(x => ((ModelPanelViewport) x).GetInfo()).ToList()
            };
            return settings;
        }

        public override void SetDefaultSettings()
        {
            DistributeSettings(ModelEditorSettings.Default());
        }

        public void DistributeSettings(ModelEditorSettings settings)
        {
            if (settings == null)
            {
                return;
            }

            _updating = true;
            ModelPanel.BeginUpdate();

            RetrieveCorrespondingAnimations = settings.RetrieveCorrAnims;
            SyncTexturesToObjectList = settings.SyncTexToObj;
            SyncVIS0 = settings.SyncObjToVIS0;
            DisableBonesWhenPlaying = settings.DisableBonesOnPlay;
            _snapToCollisions = settings.SnapToColl;
            _maximize = settings.Maximize;
            _savePosition = settings.SavePosition;
            _hideMainWindow = settings.HideMainWindow;
            chkExternalAnims.Checked = settings.DisplayExternalAnims;
            chkBRRESAnims.Checked = settings.DisplayBRRESAnims;
            chkNonBRRESAnims.Checked = settings.DisplayNonBRRESAnims;
            rightPanel.pnlBones.chkFlat.Checked = settings.FlatBoneList;
            rightPanel.pnlBones.chkContains.Checked = settings.BoneListContains;
            UseBindStateBoxes = settings.UseBindStateBox;
            ShaderGenerator.UsePixelLighting = settings.UsePixelLighting;

            MDL0BoneNode.DefaultNodeColor = (Color) settings._orbColor;
            MDL0BoneNode.DefaultLineColor = (Color) settings._lineColor;
            MDL0BoneNode.DefaultLineDeselectedColor = (Color) settings._lineDeselectedColor;
            _floorHue = (Color) settings._floorColor;
            foreach (ModelPanelViewportInfo v in settings._viewports)
            {
                v._backColor = Program.RootNode is ARCNode a && a.IsStage ? settings._stgBgColor : settings._bgColor;
            }

            int w = (int) settings._rightPanelWidth;
            if (w >= 50)
            {
                rightPanel.Width = w;
            }

            _allowedUndos = settings._undoCount;
            ScreenCaptureType = settings._imageCapFmt;

            CHR0EntryNode._generateTangents = settings.GenTansCHR;
            SRT0TextureNode._generateTangents = settings.GenTansSRT;
            SHP0VertexSetNode._generateTangents = settings.GenTansSHP;
            SCN0LightNode._generateTangents = settings.GenTansLight;
            SCN0FogNode._generateTangents = settings.GenTansFog;
            SCN0CameraNode._generateTangents = settings.GenTansCam;

            string applicationFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            string t = settings._screenCapPath;
            ScreenCapBgLocText.Text = !string.IsNullOrEmpty(t) ? t : applicationFolder + "\\ScreenCaptures";

            t = settings._liveTexFolderPath;
            LiveTextureFolderPath.Text = MDL0TextureNode.TextureOverrideDirectory =
                !string.IsNullOrEmpty(t) ? t : applicationFolder;

            EnableLiveTextureFolder.Checked = MDL0TextureNode._folderWatcher.EnableRaisingEvents;

            ModelPanel b = ModelPanel;
            b.ClearViewports();

            foreach (ModelPanelViewportInfo s in settings._viewports)
            {
                b.AddViewport(s.AsViewport());
            }

            ModelPanel.EndUpdate();
            _updating = false;
        }

        #endregion

        #region Panel Toggles

        private void showLeft_CheckedChanged(object sender, EventArgs e)
        {
            leftPanel.Visible = spltLeft.Visible = showLeft.Checked;
            btnLeftToggle.Text = showLeft.Checked == false ? ">" : "<";
        }

        private void showRight_CheckedChanged(object sender, EventArgs e)
        {
            rightPanel.Visible = spltRight.Visible = showRight.Checked;
            btnRightToggle.Text = showRight.Checked == false ? "<" : ">";
        }

        private void showBottom_CheckedChanged(object sender, EventArgs e)
        {
            animEditors.Visible = !animEditors.Visible;
            CheckDimensions();
        }

        private void showTop_CheckedChanged(object sender, EventArgs e)
        {
            controlPanel.Visible = showTop.Checked;
        }

        #endregion

        protected override void OnSelectedVerticesChanged()
        {
            //weightEditor.TargetVertices = _selectedVertices;
            //vertexEditor.TargetVertices = _selectedVertices;

            base.OnSelectedVerticesChanged();
        }

        public void LinkZoom(ModelPanelViewport control, ModelPanelViewport affected)
        {
            control.Zoomed += affected.Zoom;
        }

        public void LinkTranslate(ModelPanelViewport control, ModelPanelViewport affected)
        {
            control.Translated += affected.Translate;
        }

        public void LinkScale(ModelPanelViewport control, ModelPanelViewport affected)
        {
            control.Scaled += affected.Scale;
        }

        public void LinkRotate(ModelPanelViewport control, ModelPanelViewport affected)
        {
            control.Rotated += affected.Rotate;
        }

        public void LinkPivot(ModelPanelViewport control, ModelPanelViewport affected)
        {
            control.Pivoted += affected.Pivot;
        }

        public void OnDragEnter(object sender, DragEventArgs e)
        {
            if (_openFileDelegate == null)
            {
                return;
            }

            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        public void OnDragDrop(object sender, DragEventArgs e)
        {
            if (_openFileDelegate == null)
            {
                return;
            }

            Array a = (Array) e.Data.GetData(DataFormats.FileDrop);
            if (a != null)
            {
                string s = null;
                for (int i = 0; i < a.Length; i++)
                {
                    s = a.GetValue(i).ToString();
                    BeginInvoke(_openFileDelegate, new object[] {s});
                }
            }
        }

        #region Changed Events

        private void modelPanel_OnCurrentViewportChanged(GLViewport p)
        {
            ModelPanelViewport v = p as ModelPanelViewport;

            stretchToolStripMenuItem1.Checked = p.BackgroundImageType == BGImageType.Stretch;
            centerToolStripMenuItem1.Checked = p.BackgroundImageType == BGImageType.Center;
            resizeToolStripMenuItem1.Checked = p.BackgroundImageType == BGImageType.ResizeWithBars;

            bool camDefaultSet =
                p.Camera._defaultRotate != p.GetDefaultRotate() ||
                p.Camera._defaultScale != p.GetDefaultScale() ||
                p.Camera._defaultTranslate != new Vector3();

            btnSaveCam.Text = camDefaultSet ? "Clear Camera" : "Save Camera";

            modelPanel_RenderBonesChanged(ModelPanel, v.RenderBones);
            modelPanel_RenderFloorChanged(ModelPanel, v.RenderFloor);
            modelPanel_RenderModelBoxChanged(ModelPanel, v.RenderModelBox);
            modelPanel_RenderNormalsChanged(ModelPanel, v.RenderNormals);
            modelPanel_RenderObjectBoxChanged(ModelPanel, v.RenderObjectBox);
            modelPanel_RenderOffscreenChanged(ModelPanel, v.DontRenderOffscreen);
            ModelPanel_RenderPolygonsChanged(ModelPanel, v.RenderPolygons);
            ModelPanel_RenderVerticesChanged(ModelPanel, v.RenderVertices);
            modelPanel_RenderVisBoneBoxChanged(ModelPanel, v.RenderVisBoneBox);
            ModelPanel_RenderWireframeChanged(ModelPanel, v.RenderWireframe);
            ModelPanel_ScaleBonesChanged(ModelPanel, v.ScaleBones);
            ModelPanel_ApplyBillboardBonesChanged(ModelPanel, v.ApplyBillboardBones);
            modelPanel_FirstPersonCameraChanged(ModelPanel, v._firstPersonCamera);
            ModelPanel_RenderShadersChanged(ModelPanel, v._renderAttrib._renderShaders);
            ModelPanel_UseBindStateBoxesChanged(ModelPanel, v.UseBindStateBoxes);
            sCN0ToolStripMenuItem.Checked = v.RenderSCN0Controls;

            _currentProjBox.Checked = false;
            switch (p.ViewType)
            {
                case ViewportProjection.Perspective:
                    _currentProjBox = perspectiveToolStripMenuItem;
                    break;
                case ViewportProjection.Orthographic:
                    _currentProjBox = orthographicToolStripMenuItem;
                    break;
                case ViewportProjection.Top:
                    _currentProjBox = topToolStripMenuItem;
                    break;
                case ViewportProjection.Bottom:
                    _currentProjBox = bottomToolStripMenuItem;
                    break;
                case ViewportProjection.Left:
                    _currentProjBox = leftToolStripMenuItem;
                    break;
                case ViewportProjection.Right:
                    _currentProjBox = rightToolStripMenuItem;
                    break;
                case ViewportProjection.Front:
                    _currentProjBox = frontToolStripMenuItem;
                    break;
                case ViewportProjection.Back:
                    _currentProjBox = backToolStripMenuItem;
                    break;
            }

            _currentProjBox.Checked = true;

            showCameraCoordinatesToolStripMenuItem.Checked = v._showCamCoords;
            loadImageToolStripMenuItem.Text = v.BackgroundImage == null ? "Load Image" : "Clear Image";
        }

        private void UpdateAnimList_Event(object sender, EventArgs e)
        {
            leftPanel.UpdateAnimations();
        }

        private void Invalidate_Event(object sender, EventArgs e)
        {
            ModelPanel.Invalidate();
        }

        public void SelectedPolygonChanged()
        {
            //We can't return here if the selected polygon is set to null.
            //If the target model is changed or the selected object is cleared,
            //things relying on the selected object must be updated to reflect that.
            //if (leftPanel.SelectedPolygon == null) return;

            //This sets the selected object index internally in the model.
            //This determines the target object for focus editing vertices, normals, etc in the viewer
            //If the selected object is set to null, the poly index will be set to -1 by IndexOf.
            //This means vertices, normals etc will be drawn for all objects, if enabled.
            _targetModel.SelectedObjectIndex = _targetModel.Objects.IndexOf(leftPanel.SelectedObject);

            //If this setting is enabled, we need to show the user what textures only this object uses.
            //If the polygon is set to null, all of the model's texture references will be shown.
            if (SyncTexturesToObjectList)
            {
                leftPanel.UpdateTextures();
            }

            //Update the VIS editor to show the entries for the selected object
            if (TargetAnimType == NW4RAnimType.VIS &&
                leftPanel.SelectedObject != null &&
                vis0Editor.listBox1.Items.Count != 0 &&
                leftPanel.SelectedObject is MDL0ObjectNode)
            {
                MDL0ObjectNode o = (MDL0ObjectNode) leftPanel.SelectedObject;

                int x = 0;
                foreach (object i in vis0Editor.listBox1.Items)
                {
                    if (o._drawCalls.Count > 0 && i.ToString() == o._drawCalls[0].VisibilityBone)
                    {
                        vis0Editor.listBox1.SelectedIndex = x;
                        break;
                    }
                    else
                    {
                        x++;
                    }
                }

                if (x == vis0Editor.listBox1.Items.Count)
                {
                    vis0Editor.listBox1.SelectedIndex = -1;
                }
            }

            ModelPanel.Invalidate();
        }

        private void ModelPanel_UseBindStateBoxesChanged(ModelPanel panel, bool value)
        {
            if (ModelPanel == panel && !_updating)
            {
                _updating = true;
                UseBindStateBoxes = value;
                _updating = false;
            }
        }

        private void ModelPanel_ApplyBillboardBonesChanged(ModelPanel panel, bool value)
        {
            if (ModelPanel == panel && !_updating)
            {
                _updating = true;
                chkBillboardBones.Checked = value;
                _updating = false;
            }
        }

        private void ModelPanel_RenderShadersChanged(ModelPanel panel, bool value)
        {
            //Only update if the focused panel triggered the event
            if (ModelPanel == panel && !_updating)
            {
                _updating = true;
                shadersToolStripMenuItem.Checked = value;
                _updating = false;
            }
        }

        private void ModelPanel_RenderWireframeChanged(ModelPanel panel, bool value)
        {
            if (ModelPanel == panel && !_updating)
            {
                _updating = true;
                wireframeToolStripMenuItem.Checked = value;
                _updating = false;
            }
        }

        private void ModelPanel_RenderVerticesChanged(ModelPanel panel, bool value)
        {
            if (ModelPanel == panel && !_updating)
            {
                _updating = true;
                chkVertices.Checked = toggleVertices.Checked = value;
                _updating = false;
            }
        }

        private void ModelPanel_RenderPolygonsChanged(ModelPanel panel, bool value)
        {
            if (ModelPanel == panel && !_updating)
            {
                _updating = true;
                chkPolygons.Checked = togglePolygons.Checked = value;
                _updating = false;
            }
        }

        private void modelPanel_RenderOffscreenChanged(ModelPanel panel, bool value)
        {
            if (ModelPanel == panel && !_updating)
            {
                _updating = true;
                DontRenderOffscreen = value;
                _updating = false;
            }
        }

        private void modelPanel_RenderNormalsChanged(ModelPanel panel, bool value)
        {
            if (ModelPanel == panel && !_updating)
            {
                _updating = true;
                toggleNormals.Checked = value;
                _updating = false;
            }
        }

        private void modelPanel_FirstPersonCameraChanged(ModelPanel panel, bool value)
        {
            if (ModelPanel == panel && !_updating)
            {
                _updating = true;
                firstPersonCameraToolStripMenuItem.Checked = value;
                _updating = false;
            }
        }

        private void modelPanel_RenderFloorChanged(ModelPanel panel, bool value)
        {
            if (ModelPanel == panel && !_updating)
            {
                _updating = true;
                chkFloor.Checked = toggleFloor.Checked = value;
                _updating = false;
            }
        }

        private void modelPanel_RenderModelBoxChanged(ModelPanel panel, bool value)
        {
            if (ModelPanel == panel && !_updating)
            {
                _updating = true;
                chkBBModels.Checked = value;
                _updating = false;
            }
        }

        private void modelPanel_RenderObjectBoxChanged(ModelPanel panel, bool value)
        {
            if (ModelPanel == panel && !_updating)
            {
                _updating = true;
                chkBBObjects.Checked = value;
                _updating = false;
            }
        }

        private void modelPanel_RenderVisBoneBoxChanged(ModelPanel panel, bool value)
        {
            if (ModelPanel == panel && !_updating)
            {
                _updating = true;
                chkBBVisBones.Checked = value;
                _updating = false;
            }
        }

        private void modelPanel_RenderBonesChanged(ModelPanel panel, bool value)
        {
            if (ModelPanel == panel && !_updating)
            {
                _updating = true;
                toggleBones.Checked = chkBones.Checked = value;
                _updating = false;
            }
        }

        private void ModelPanel_ScaleBonesChanged(ModelPanel panel, bool value)
        {
            if (ModelPanel == panel && !_updating)
            {
                _updating = true;
                //scaleBonesToolStripMenuItem.Checked = value;
                _updating = false;
            }
        }

        private void ModelPanel_RenderMetalsChanged(ModelPanel panel, bool value)
        {
            if (ModelPanel == panel && !_updating)
            {
                _updating = true;
                toggleMetals.Checked = value;
                _updating = false;
            }
        }

        private void OnRenderCollisionsChanged()
        {
            if (_updating)
            {
                return;
            }

            _updating = true;
            toggleCollisions.Checked = chkCollisions.Checked = _renderCollisions;
            if (EditingAll)
            {
                foreach (CollisionNode m in _collisions)
                {
                    foreach (CollisionObject o in m.Children)
                    {
                        o._render = RenderCollisions;
                    }
                }
            }
            else if (TargetCollision != null)
            {
                foreach (CollisionObject o in TargetCollision.Children)
                {
                    o._render = RenderCollisions;
                }

                for (int i = 0; i < leftPanel.lstObjects.Items.Count; i++)
                {
                    leftPanel.lstObjects.SetItemChecked(i, RenderCollisions);
                }
            }

            modelPanel.Invalidate();
            _updating = false;
        }

        #endregion
    }
}