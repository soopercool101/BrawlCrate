using BrawlLib.OpenGL;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Modeling;
using System.Drawing;
using System.Collections.Generic;

namespace System.Windows.Forms
{
    public partial class ModelEditControl : ModelEditorBase
    {
        #region Model Viewer Properties
        void firstPersonCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_updating)
                FirstPersonCamera = !FirstPersonCamera;
        }
        private void shadersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_updating)
                RenderShaders = !RenderShaders;
        }
        private void scaleBonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_updating)
                ScaleBones = !ScaleBones;
        }
        private void modelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!_updating)
                RenderModelBox = !RenderModelBox;
        }
        private void objectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_updating)
                RenderObjectBox = !RenderObjectBox;
        }
        private void visibilityBonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_updating)
                RenderVisBoneBox = !RenderVisBoneBox;
        }
        private void displayBindBoundingBoxesOn0FrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_updating)
                UseBindStateBoxes = !UseBindStateBoxes;
        }
        private void chkBillboardBones_Click(object sender, EventArgs e)
        {
            if (!_updating)
                ApplyBillboardBones = !ApplyBillboardBones;
        }
        private void toggleRenderBones_Event(object sender, EventArgs e)
        {
            if (!_updating)
                RenderBones = !RenderBones;
        }
        private void toggleRenderPolygons_Event(object sender, EventArgs e)
        {
            if (!_updating)
                RenderPolygons = !RenderPolygons;
        }
        private void toggleRenderVertices_Event(object sender, EventArgs e)
        {
            if (!_updating)
                RenderVertices = !RenderVertices;
        }
        private void toggleRenderCollisions_Event(object sender, EventArgs e)
        {
            if (!_updating)
                RenderCollisions = !RenderCollisions;
        }
        private void toggleRenderFloor_Event(object sender, EventArgs e)
        {
            if (!_updating)
                RenderFloor = !RenderFloor;
        }
        private void wireframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_updating)
                RenderWireframe = !RenderWireframe;
        }
        private void toggleNormals_Click(object sender, EventArgs e)
        {
            if (!_updating)
                RenderNormals = !RenderNormals;
        }
        #endregion

        #region Screen Capture
        private void ScreenCapBgLocText_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog d = new FolderBrowserDialog())
            {
                d.SelectedPath = ScreenCapBgLocText.Text;
                d.Description = "Choose a place to automatically save screen captures.";
                if (d.ShowDialog(this) == DialogResult.OK)
                    ScreenCapBgLocText.Text = d.SelectedPath;
            }
            if (String.IsNullOrEmpty(ScreenCapBgLocText.Text))
                ScreenCapBgLocText.Text = Application.StartupPath + "\\ScreenCaptures";
        }
        private ImageType _imgType = ImageType.png;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ImageType ScreenCaptureType
        {
            get { return _imgType; }
            set
            {
                _imgType = value;
                imageFormatToolStripMenuItem.Text = "Image Format: " + _imgType.ToString().ToUpper();
            }
        }
        private void imageFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Just use an existing dialog with the same basic function
            using (ExportAllFormatDialog d = new ExportAllFormatDialog())
            {
                d.Text = "Choose texture format";
                d.comboBox1.Items.RemoveAt(6); //TEX0
                if (d.ShowDialog(this) == DialogResult.OK)
                {
                    _imgType = (ImageType)d.comboBox1.SelectedIndex;
                    imageFormatToolStripMenuItem.Text = "Image Format: " + _imgType.ToString().ToUpper();
                }
            }
        }
        private void btnExportToImgWithTransparency_Click(object sender, EventArgs e)
        {
            SaveBitmap(ModelPanel.GetScreenshot(ModelPanel.ClientRectangle, true), ScreenCapBgLocText.Text, "." + _imgType);
        }
        private void btnExportToImgNoTransparency_Click(object sender, EventArgs e)
        {
            SaveBitmap(ModelPanel.GetScreenshot(ModelPanel.ClientRectangle, false), ScreenCapBgLocText.Text, "." + _imgType);
        }
        private void btnExportToAnimatedGIF_Click(object sender, EventArgs e)
        {
            //SetFrame(1);
            _images = new List<Image>();
            _loop = false;
            _capture = true;
            //Enabled = false;
            ModelPanel.Enabled = false;
            if (InterpolationEditor != null)
                InterpolationEditor.Enabled = false;
            if (DisableBonesWhenPlaying)
            {
                if (RenderBones == false)
                    _bonesWereOff = true;
                RenderBones = false;
            }
            TogglePlay();
        }
        #endregion

        #region Model Viewer Detaching
        private void detachViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_updating)
                return;

            if (_viewerForm == null)
            {
                modelPanel.Visible = false;
                modelPanel.Enabled = false;
                detachViewerToolStripMenuItem.Text = "Attach";

                _viewerForm = new ModelViewerForm(this);

                _viewerForm.modelPanel1._resourceList = modelPanel._resourceList;
                _viewerForm.modelPanel1._renderList = modelPanel._renderList;
                foreach (ModelPanelViewport v in modelPanel)
                    _viewerForm.modelPanel1.AddViewport(v);

                _viewerForm.modelPanel1.CurrentViewport = modelPanel.CurrentViewport;
                _viewerForm.FormClosed += _viewerForm_FormClosed;
                _viewerForm.modelPanel1.EventProcessKeyMessage += ProcessKeyPreview;

                UnlinkModelPanel(modelPanel);
                LinkModelPanel(_viewerForm.modelPanel1);

                OnModelPanelChanged();

                _viewerForm.Show();
                _viewerForm.modelPanel1.Invalidate();

                _interpolationEditor.Visible = true;
                InterpolationFormOpen = false;
                interpolationEditorToolStripMenuItem.Enabled = false;

                if (_interpolationForm != null)
                    _interpolationForm.Close();
            }
            else
                _viewerForm.Close();
        }
        void _viewerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            modelPanel.Visible = true;
            modelPanel.Enabled = true;
            detachViewerToolStripMenuItem.Text = "Detach";
            
            _viewerForm.modelPanel1.EventProcessKeyMessage -= ProcessKeyPreview;

            UnlinkModelPanel(_viewerForm.modelPanel1);
            LinkModelPanel(modelPanel);

            _viewerForm = null;
            _interpolationEditor.Visible = false;
            interpolationEditorToolStripMenuItem.Enabled = true;

            OnModelPanelChanged();
        }
        #endregion

        #region Panel Toggles

        private void btnLeftToggle_Click(object sender, EventArgs e) { showLeft.Checked = !showLeft.Checked; }
        private void btnTopToggle_Click(object sender, EventArgs e) { showTop.Checked = !showTop.Checked; }
        private void btnBottomToggle_Click(object sender, EventArgs e) { showBottom.Checked = !showBottom.Checked; }
        private void btnRightToggle_Click(object sender, EventArgs e) { showRight.Checked = !showRight.Checked; }

        #endregion

        private void btnSaveCam_Click(object sender, EventArgs e)
        {
            if (btnSaveCam.Text == "Save Camera")
            {
                ModelPanel.CurrentViewport.Camera.SaveDefaults();
                btnSaveCam.Text = "Clear Camera";
            }
            else
            {
                ModelPanel.CurrentViewport.ClearCameraDefaults();
                btnSaveCam.Text = "Save Camera";
            }
        }
        private void leftToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ModelPanelViewport curViewport = modelPanel.CurrentViewport;
            ModelPanelViewport newViewport = ModelPanelViewport.DefaultPerspective;
            newViewport.BackgroundColor = curViewport.BackgroundColor;
            ModelPanel.AddViewport(newViewport);

            float xMin = curViewport.Percentages._x;
            float yMin = curViewport.Percentages._y;
            float xMax = curViewport.Percentages._z;
            float yMax = curViewport.Percentages._w;
            float averageX = (xMin + xMax) / 2.0f;

            curViewport.SetPercentages(averageX, yMin, xMax, yMax);
            newViewport.SetPercentages(xMin, yMin, averageX, yMax);

            ModelPanel.Invalidate();
        }
        private void topToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ModelPanelViewport curViewport = modelPanel.CurrentViewport;
            ModelPanelViewport newViewport = ModelPanelViewport.DefaultPerspective;
            newViewport.BackgroundColor = curViewport.BackgroundColor;
            ModelPanel.AddViewport(newViewport);

            float xMin = curViewport.Percentages._x;
            float yMin = curViewport.Percentages._y;
            float xMax = curViewport.Percentages._z;
            float yMax = curViewport.Percentages._w;
            float averageY = (yMin + yMax) / 2.0f;

            curViewport.SetPercentages(xMin, averageY, xMax, yMax);
            newViewport.SetPercentages(xMin, yMin, xMax, averageY);

            ModelPanel.Invalidate();
        }
        private void LiveTextureFolderPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog d = new FolderBrowserDialog())
            {
                d.SelectedPath = LiveTextureFolderPath.Text;
                d.Description = "Choose a place to automatically scan for textures to apply when modified.";
                if (d.ShowDialog(this) == DialogResult.OK)
                    LiveTextureFolderPath.Text = MDL0TextureNode.TextureOverrideDirectory = d.SelectedPath;
            }
            if (String.IsNullOrEmpty(LiveTextureFolderPath.Text))
                LiveTextureFolderPath.Text = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            modelPanel.RefreshReferences();
        }
        private void chkZoomExtents_Click(object sender, EventArgs e)
        {
            //TODO: different handling based on if viewport is perspective, front, side, or top
            ModelPanel.Camera.ZoomExtents(SelectedBone.Matrix.GetPoint(), 27.0f);
            ModelPanel.Invalidate();
        }
        private void chkBoundaries_Click(object sender, EventArgs e)
        {
            ModelPanel.Invalidate();
        }
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rightPanel.pnlOpenedFiles.LoadExternal(true, false, false);
        }
        private void newSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure you want to clear the current scene?\nYou will lose any unsaved data.", "Continue?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            TargetModel = null;
            _targetModels.Clear();

            ModelPanel.ClearAll();
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ParentForm.Close();
        }

        #region Rendered Models

        private void hideFromSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelPanel.RemoveTarget(TargetModel);

            if (_targetModels != null && _targetModels.Count != 0)
            {
                _resetCamera = false;
                TargetModel = _targetModels[0];
            }

            ModelPanel.Invalidate();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelPanel.RemoveTarget(TargetModel);
            _targetModels.Remove(TargetModel);

            if (_targetModels != null && _targetModels.Count != 0)
            {
                _resetCamera = false;
                TargetModel = _targetModels[0];
            }

            ModelPanel.Invalidate();
        }

        private void hideAllOtherModelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (IModel node in _targetModels)
                if (node != TargetModel)
                    ModelPanel.RemoveTarget(node);

            ModelPanel.Invalidate();
        }

        private void deleteAllOtherModelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (IModel node in _targetModels)
                if (node != TargetModel)
                {
                    _targetModels.Remove(node);
                    ModelPanel.RemoveTarget(node);
                }

            ModelPanel.Invalidate();
        }

        #endregion

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ModelViewerHelp().Show(this, false);
        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ModelViewerSettingsDialog().Show(this);
        }

        private void resetCameraToolStripMenuItem_Click_1(object sender, EventArgs e) { ModelPanel.ResetCamera(); }

        private void interpolationEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_interpolationForm == null)
            {
                _interpolationForm = new InterpolationForm(this);
                _interpolationForm.FormClosed += _interpolationForm_FormClosed;
                _interpolationForm.Show();
                InterpolationFormOpen = true;
                UpdatePropDisplay();
            }
            else
                _interpolationForm.Close();
        }

        private void portToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TargetModel == null || !(TargetModel is MDL0Node))
                return;

            NW4RAnimationNode node = TargetAnimation;
            if (node is CHR0Node)
                (node as CHR0Node).Port((MDL0Node)TargetModel);

            AnimChanged();
        }

        private void mergeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TargetModel == null)
                return;

            NW4RAnimationNode node = TargetAnimation;
            if (node is CHR0Node)
                (node as CHR0Node).MergeWith();

            AnimChanged();
        }

        private void appendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TargetModel == null)
                return;

            NW4RAnimationNode node = TargetAnimation;
            if (node is CHR0Node)
                (node as CHR0Node).Append();
            else if (node is SRT0Node)
                (node as SRT0Node).Append();
            else if (node is SHP0Node)
                (node as SHP0Node).Append();
            else if (node is PAT0Node)
                (node as PAT0Node).Append();
            else if (node is VIS0Node)
                (node as VIS0Node).Append();

            AnimChanged();
        }

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TargetModel == null)
                return;

            NW4RAnimationNode node = TargetAnimation;
            if (node is CHR0Node)
                (node as CHR0Node).Resize();
            else if (node is SRT0Node)
                (node as SRT0Node).Resize();
            else if (node is SHP0Node)
                (node as SHP0Node).Resize();
            else if (node is PAT0Node)
                (node as PAT0Node).Resize();
            else if (node is VIS0Node)
                (node as VIS0Node).Resize();

            AnimChanged();
        }

        private void averageAllStartEndTangentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NW4RAnimationNode n = TargetAnimation;
            if (n is CHR0Node)
                ((CHR0Node)n).AverageKeys();
            if (n is SRT0Node)
                ((SRT0Node)n).AverageKeys();
            if (n is SHP0Node)
                ((SHP0Node)n).AverageKeys();
        }

        private void averageboneStartendTangentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NW4RAnimationNode n = TargetAnimation;
            if (n is CHR0Node && SelectedBone != null)
                ((CHR0Node)n).AverageKeys(SelectedBone.Name);
            if (n is SRT0Node && TargetTexRef != null)
                ((SRT0Node)n).AverageKeys(TargetTexRef.Parent.Name, TargetTexRef.Index);
            if (n is SHP0Node && SHP0Editor.SelectedDestination != null && SHP0Editor.VertexSetDest != null)
                ((SHP0Node)n).AverageKeys(SHP0Editor.SelectedDestination, SHP0Editor.VertexSetDest.Name);
        }

        public void setColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChooseBackgroundColor();
        }

        public void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChooseOrClearBackgroundImage();

            loadImageToolStripMenuItem.Text = ModelPanel.CurrentViewport.BackgroundImage == null ? "Load Image" : "Clear Image";
        }

        protected void btnUndo_Click(object sender, EventArgs e) { Undo(); }
        protected void btnRedo_Click(object sender, EventArgs e) { Redo(); }

        private void playCHR0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_chr0 != null && CurrentFrame != 0)
                UpdateModel();
        }

        private void playSRT0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_srt0 != null && CurrentFrame != 0)
                UpdateModel();
        }

        private void playSHP0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_shp0 != null && CurrentFrame != 0)
                UpdateModel();
        }

        private void playPAT0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_pat0 != null && CurrentFrame != 0)
                UpdateModel();
        }

        private void playVIS0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_vis0 != null && CurrentFrame != 0)
                UpdateModel();
        }

        private void playCLR0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_clr0 != null && CurrentFrame != 0)
                UpdateModel();
        }

        private void playSCN0ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_scn0 != null && CurrentFrame != 0)
                UpdateModel();
        }

        ToolStripMenuItem _currentProjBox;

        private void perspectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentProjBox.Checked = false;
            _currentProjBox = perspectiveToolStripMenuItem;
            _currentProjBox.Checked = true;
            ModelPanel.CurrentViewport.ViewType = ViewportProjection.Perspective;
        }

        private void orthographicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentProjBox.Checked = false;
            _currentProjBox = orthographicToolStripMenuItem;
            _currentProjBox.Checked = true;
            ModelPanel.CurrentViewport.ViewType = ViewportProjection.Orthographic;
        }

        private void frontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentProjBox.Checked = false;
            _currentProjBox = frontToolStripMenuItem;
            _currentProjBox.Checked = true;
            ModelPanel.CurrentViewport.ViewType = ViewportProjection.Front;
        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentProjBox.Checked = false;
            _currentProjBox = backToolStripMenuItem;
            _currentProjBox.Checked = true;
            ModelPanel.CurrentViewport.ViewType = ViewportProjection.Back;
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentProjBox.Checked = false;
            _currentProjBox = leftToolStripMenuItem;
            _currentProjBox.Checked = true;
            ModelPanel.CurrentViewport.ViewType = ViewportProjection.Left;
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentProjBox.Checked = false;
            _currentProjBox = rightToolStripMenuItem;
            _currentProjBox.Checked = true;
            ModelPanel.CurrentViewport.ViewType = ViewportProjection.Right;
        }

        private void topToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentProjBox.Checked = false;
            _currentProjBox = topToolStripMenuItem;
            _currentProjBox.Checked = true;
            ModelPanel.CurrentViewport.ViewType = ViewportProjection.Top;
        }

        private void bottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentProjBox.Checked = false;
            _currentProjBox = bottomToolStripMenuItem;
            _currentProjBox.Checked = true;
            ModelPanel.CurrentViewport.ViewType = ViewportProjection.Bottom;
        }

        private void chkEditAll_Click(object sender, EventArgs e)
        {
            EditingAll = !EditingAll;
        }

        private void stretchToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            centerToolStripMenuItem1.Checked = resizeToolStripMenuItem.Checked = false;
            stretchToolStripMenuItem1.Checked = true;

            ModelPanel.CurrentViewport.BackgroundImageType = BGImageType.Stretch;
        }

        private void centerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            stretchToolStripMenuItem1.Checked = resizeToolStripMenuItem.Checked = false;
            centerToolStripMenuItem1.Checked = true;

            ModelPanel.CurrentViewport.BackgroundImageType = BGImageType.Center;
        }

        private void resizeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            centerToolStripMenuItem1.Checked = stretchToolStripMenuItem1.Checked = false;
            resizeToolStripMenuItem.Checked = true;

            ModelPanel.CurrentViewport.BackgroundImageType = BGImageType.ResizeWithBars;
        }

        private void scaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlType = ControlType != TransformType.Scale ? TransformType.Scale : TransformType.None;
        }

        private void rotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlType = ControlType != TransformType.Rotation ? TransformType.Rotation : TransformType.None;
        }

        private void translationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlType = ControlType != TransformType.Translation ? TransformType.Translation : TransformType.None;
        }

        private void showCameraCoordinatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showCameraCoordinatesToolStripMenuItem.Checked = ModelPanel.CurrentViewport._showCamCoords = !ModelPanel.CurrentViewport._showCamCoords;
        }

        private void EnableLiveTextureFolder_Click(object sender, EventArgs e)
        {
            EnableLiveTextureFolder.Checked = (MDL0TextureNode._folderWatcher.EnableRaisingEvents = !MDL0TextureNode._folderWatcher.EnableRaisingEvents);
            ModelPanel.RefreshReferences();
        }
    }
}
