using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using Ikarus.ModelViewer;

namespace Ikarus.UI
{
    public partial class MainControl : ModelEditorBase
    {
        private unsafe void storeSettingsExternallyToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            //if (_updating) return;
            //BrawlLib.Properties.Settings.Default.External = storeSettingsExternallyToolStripMenuItem.Checked;

            //BrawlBoxViewerSettings settings = new BrawlBoxViewerSettings();
            //if (BrawlLib.Properties.Settings.Default.External)
            //{
            //    settings = BrawlLib.Properties.Settings.Default.ViewerSettings;
            //    using (FileStream stream = new FileStream(Application.StartupPath + "/brawlbox.settings", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 8, FileOptions.SequentialScan))
            //    {
            //        CompactStringTable s = new CompactStringTable();
            //        s.Add(ScreenCapBgLocText.Text);
            //        stream.SetLength((long)BrawlBoxViewerSettings.Size + s.TotalSize);
            //        using (FileMap map = FileMap.FromStream(stream))
            //        {
            //            *(BrawlBoxViewerSettings*)map.Address = settings;
            //            s.WriteTable(map.Address + BrawlBoxViewerSettings.Size);
            //            ((BrawlBoxViewerSettings*)map.Address)->_screenCapPathOffset = (uint)s[ScreenCapBgLocText.Text] - (uint)map.Address;
            //        }
            //    }
            //}
            //else
            //{
            //    if (File.Exists(Application.StartupPath + "/brawlbox.settings"))
            //        using (FileMap map = FileMap.FromFile(Application.StartupPath + "/brawlbox.settings", FileMapProtect.Read))
            //            if (*(uint*)map.Address == BrawlBoxViewerSettings.Tag)
            //                settings = *(BrawlBoxViewerSettings*)map.Address;

            //    BrawlLib.Properties.Settings.Default.ViewerSettings = settings;
            //    BrawlLib.Properties.Settings.Default.ScreenCapBgLocText = ScreenCapBgLocText.Text;
            //    BrawlLib.Properties.Settings.Default.Save();
            //}
        }
        private void orthographicToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            _updating = true;
            if (orthographicToolStripMenuItem.Checked)
            {
                //modelPanel.SetProjectionType(true);
                perspectiveToolStripMenuItem.Checked = false;
            }
            else
            {
                //modelPanel.SetProjectionType(false);
                perspectiveToolStripMenuItem.Checked = true;
            }
            _updating = false;
        }

        private void perspectiveToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            _updating = true;
            if (perspectiveToolStripMenuItem.Checked)
            {
                //modelPanel.SetProjectionType(false);
                orthographicToolStripMenuItem.Checked = false;
            }
            else
            {
                //modelPanel.SetProjectionType(true);
                orthographicToolStripMenuItem.Checked = true;
            }
            _updating = false;
        }
        private void stretchToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating) return;
            if (stretchToolStripMenuItem1.Checked)
            {
                _updating = true;
                centerToolStripMenuItem1.Checked = resizeToolStripMenuItem1.Checked = false;
                //modelPanel.BackgroundImageType = GLPanel.BGImageType.Stretch;
                _updating = false;
                modelPanel.Invalidate();
            }
        }

        private void centerToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating) return;
            if (centerToolStripMenuItem1.Checked)
            {
                _updating = true;
                stretchToolStripMenuItem1.Checked = resizeToolStripMenuItem1.Checked = false;
                //modelPanel.BackgroundImageType = GLPanel.BGImageType.Center;
                _updating = false;
                modelPanel.Invalidate();
            }
        }

        private void resizeToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating) return;
            if (resizeToolStripMenuItem1.Checked)
            {
                _updating = true;
                centerToolStripMenuItem1.Checked = stretchToolStripMenuItem1.Checked = false;
                //modelPanel.BackgroundImageType = GLPanel.BGImageType.ResizeWithBars;
                _updating = false;
                modelPanel.Invalidate();
            }
        }
        private void chkShaders_CheckedChanged(object sender, EventArgs e)
        {
            //if (modelPanel.Context != null)
            //{
            //    if (modelPanel.Context._version < 2 && chkShaders.Checked)
            //    {
            //        MessageBox.Show("You need at least OpenGL 2.0 to view shaders.", "GLSL not supported",
            //        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //        chkShaders.Checked = false;
            //        return;
            //    }
            //    else
            //    {
            //        if (modelPanel.Context._shadersSupported && !chkShaders.Checked) { GL.UseProgram(0); GL.ActiveTexture(TextureUnit.Texture0); }
            //        modelPanel.Context._shadersSupported = chkShaders.Checked;
            //    }
            //}
            //modelPanel.Invalidate();
        }

        private void showCameraCoordinatesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            //modelPanel._showCamCoords = showCameraCoordinatesToolStripMenuItem.Checked;
        }

        private void enableTextOverlaysToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            //modelPanel.TextOverlaysEnabled = enableTextOverlaysToolStripMenuItem.Checked;
        }

        private void enablePointAndLineSmoothingToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            //modelPanel._enableSmoothing = enablePointAndLineSmoothingToolStripMenuItem.Checked;
        }

        private void stPersonToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void rotationToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            ControlType = rotationToolStripMenuItem.Checked ? TransformType.Rotation : TransformType.None;
        }

        private void translationToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            ControlType = translationToolStripMenuItem.Checked ? TransformType.Translation : TransformType.None;
        }

        private void scaleToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            ControlType = scaleToolStripMenuItem.Checked ? TransformType.Scale : TransformType.None;
        }
        //private void displayBRRESRelativeAnimationsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        //{
        //    pnlAssets.BRRESRelative = displayBRRESRelativeAnimationsToolStripMenuItem.CheckState;
        //    pnlAssets.UpdateAnimations(TargetAnimType);
        //    switch (pnlAssets.BRRESRelative)
        //    {
        //        case CheckState.Checked:
        //            displayBRRESRelativeAnimationsToolStripMenuItem.Text = "Displaying only BRRES animations"; break;
        //        case CheckState.Indeterminate:
        //            displayBRRESRelativeAnimationsToolStripMenuItem.Text = "Displaying BRRES and external animations"; break;
        //        case CheckState.Unchecked:
        //            displayBRRESRelativeAnimationsToolStripMenuItem.Text = "Displaying all animations"; break;
        //    }
        //}

        private void displayFrameCountDifferencesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            DialogResult d;
            if (!displayFrameCountDifferencesToolStripMenuItem.Checked)
            {
                if ((d = MessageBox.Show("Do you want to sync animation frame counts by default?", "Sync Frame Counts by Default", MessageBoxButtons.YesNo)) == DialogResult.Yes && !alwaysSyncFrameCountsToolStripMenuItem.Checked)
                    alwaysSyncFrameCountsToolStripMenuItem.Checked = true;
                else if (d == DialogResult.No)
                    alwaysSyncFrameCountsToolStripMenuItem.Checked = false;
            }
            else
                alwaysSyncFrameCountsToolStripMenuItem.Checked = false;
        }

        private void syncObjectsListToVIS0ToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            modelListsPanel1.chkSyncVis.Checked = syncObjectsListToVIS0ToolStripMenuItem.Checked;
        }

        private void syncAnimationsTogetherToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (syncAnimationsTogetherToolStripMenuItem.Checked)
                GetFiles(TargetAnimType);
            else
                GetFiles(NW4RAnimType.None);
        }
        public void pnlAnim_ReferenceLoaded(ResourceNode node) { modelPanel.AddReference(node); }

        private void pnlOptions_FloorRenderChanged(object sender, EventArgs e)
        {
            toggleFloor.Checked = RenderFloor;
            modelPanel.Invalidate();
        }

        private void Undo(object sender, EventArgs e)
        {
            if (btnUndo.Enabled)
                btnUndo_Click(null, null);
        }
        private void Redo(object sender, EventArgs e)
        {
            if (btnRedo.Enabled)
                btnRedo_Click(null, null);
        }
        //private void ApplySave(object sender, EventArgs e)
        //{
        //    SaveState save = _save;
        //    pnlAnim.ApplySave(save);
        //    SetFrame(save.frameIndex);
        //    modelPanel1.Invalidate();
        //}

        private void chkPolygons_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            RenderPolygons = chkPolygons.Checked;
        }

        public ScriptPanel MovesetPanel { get { return scriptPanel.scriptPanel; } }
        public override void numFPS_ValueChanged(object sender, EventArgs e) 
        {
            RunTime.Timer.TargetRenderFrequency = (double)PlaybackPanel.numFPS.Value; 
        }
        public override void PlaybackPanel_LoopChanged() 
        {
            RunTime.Loop = PlaybackPanel.chkLoop.Checked;
            if (syncLoopToAnimationToolStripMenuItem.Checked && !_updating)
                GetAnimation(TargetAnimType).Loop = Loop;
        }

        private void FileChanged(object sender, EventArgs e)
        {
            movesetToolStripMenuItem1.Visible = chkHurtboxes.Visible = chkHitboxes.Visible = chkHurtboxes.Checked = Manager.Moveset != null;
        }

        private void RenderStateChanged(object sender, EventArgs e)
        {
            modelPanel.Invalidate();
        }

        private void HtBoxesChanged(object sender, EventArgs e)
        {
            if (chkHurtboxes.Checked)
                hurtboxesOffToolStripMenuItem.Checked = true;
            else
                hurtboxesOffToolStripMenuItem.Checked = false;

            if (chkHitboxes.Checked)
                hitboxesOffToolStripMenuItem.Checked = true;
            else
                hitboxesOffToolStripMenuItem.Checked = false;
            
            modelPanel.Invalidate(); 
        }

        public void SelectedPolygonChanged(object sender, EventArgs e) 
        {
            _targetModel.SelectedObjectIndex = _targetModel.Objects.IndexOf(modelListsPanel1.SelectedPolygon);

            if (modelListsPanel1._syncObjTex)
                modelListsPanel1.UpdateTextures();

            //if (TargetAnimType == NW4RAnimType.VIS)
            //    if (listPanel.TargetObject != null && vis0Editor.listBox1.Items.Count != 0)
            //    {
            //        int x = 0;
            //        foreach (object i in vis0Editor.listBox1.Items)
            //            if (i.ToString() == listPanel.TargetObject.VisibilityBone)
            //            {
            //                vis0Editor.listBox1.SelectedIndex = x;
            //                break;
            //            }
            //            else
            //                x++;
            //        if (x == vis0Editor.listBox1.Items.Count)
            //            vis0Editor.listBox1.SelectedIndex = -1;
            //    }

            modelPanel.Invalidate(); 
        }

        //public void numTotalFrames_ValueChanged(object sender, EventArgs e)
        //{
        //    if ((GetAnimation(TargetAnimType) == null) || (_updating))
        //        return;

        //    MaxFrame = (int)pnlPlayback.numTotalFrames.Value;

        //    //AnimationNode n;
        //    //if (alwaysSyncFrameCountsToolStripMenuItem.Checked)
        //    //    for (int i = 0; i < 5; i++)
        //    //        if ((n = GetAnimation((AnimType)i)) != null) 
        //    //            //if (i == 5) ((BRESEntryNode)n).tFrameCount = _maxFrame - 1; else 
        //    //            n.FrameCount = _maxFrame;
        //    //        else { }
        //    //else
        //    //{
        //    //    if ((n = GetAnimation(TargetAnimType)) != null)
        //    //        n.FrameCount = _maxFrame;
        //    //    if (displayFrameCountDifferencesToolStripMenuItem.Checked)
        //    //        if (MessageBox.Show("Do you want to update the frame counts of the other animation types?", "Update Frame Counts?", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //    //        for (int i = 0; i < 5; i++)
        //    //            if (i != (int)TargetAnimType && (n = GetAnimation((AnimType)i)) != null)
        //    //                n.FrameCount = _maxFrame;
        //    //}

        //    pnlPlayback.numFrameIndex.Maximum = MaxFrame;
        //}
        private void showAssets_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Visible = spltLeft.Visible = showLeft.Checked;
        }
        private void showMoveset_CheckedChanged(object sender, EventArgs e)
        {
            modelListsPanel1.Visible = spltRight.Visible = showRight.Checked;
        }
        private void showPlay_CheckedChanged(object sender, EventArgs e) 
        {
            animEditors.Visible = !animEditors.Visible;
            //if (_currentControl is CHR0Editor)
            //{
            //    animEditors.Height =
            //    panel3.Height = 82;
            //    panel3.Width = 732;
            //}
            //else if (_currentControl is SRT0Editor)
            //{
            //    animEditors.Height =
            //    panel3.Height = 82;
            //    panel3.Width = 561;
            //}
            //else if (_currentControl is SHP0Editor)
            //{
            //    animEditors.Height =
            //    panel3.Height = 106;
            //    panel3.Width = 533;
            //}
            //else if (_currentControl is PAT0Editor)
            //{
            //    animEditors.Height =
            //    panel3.Height = 77;
            //    panel3.Width = 402;
            //}
            //else if (_currentControl is VIS0Editor)
            //{
            //    animEditors.Height =
            //    panel3.Height = 112;
            //    panel3.Width = 507;
            //}
            //else
            //    animEditors.Height = panel3.Width = 0;
            CheckDimensions();
        }
        private void showOptions_CheckedChanged(object sender, EventArgs e) { controlPanel.Visible = showOptions.Checked; }
        //private void undoToolStripMenuItem_EnabledChanged(object sender, EventArgs e) { Undo.Enabled = undoToolStripMenuItem.Enabled; }
        //private void redoToolStripMenuItem_EnabledChanged(object sender, EventArgs e) { Redo.Enabled = redoToolStripMenuItem.Enabled; }
        
        //private void checkBox3_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (!_updating)
        //        RenderVertices = chkVertices.Checked;
        //}

        private void comboCharacters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            _resetCamera = false;

            Manager.TargetCharacter = (CharName)Enum.Parse(typeof(CharName), comboCharacters.SelectedItem.ToString());

            _undoSaves.Clear();
            _redoSaves.Clear();
            _saveIndex = -1;
        }

        private void chkBones_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                RenderBones = chkBones.Checked;
        }

        private void chkFloor_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                RenderFloor = chkFloor.Checked;
        }

        private void boundingBoxToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                RenderModelBox = boundingBoxToolStripMenuItem.Checked;
        }

        private void chkDontRenderOffscreen_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                DontRenderOffscreen = chkDontRenderOffscreen.Checked;
        }

        private void chr0Editor_VisibleChanged(object sender, EventArgs e)
        {
            //pnlEditors.Height = pnlPlayback.Height + (chr0Editor.Visible ? chr0Editor.Height : 0);
        }
    }
}
