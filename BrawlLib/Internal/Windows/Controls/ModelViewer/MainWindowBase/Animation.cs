using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types.Animations;
using System;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase
{
    public partial class ModelEditorBase : UserControl
    {
        #region Animation

        public NW4RAnimationNode TargetAnimation
        {
            get => GetAnimation(TargetAnimType);
            set => SetAnimation(TargetAnimType, value);
        }

        public NW4RAnimationNode GetAnimation(NW4RAnimType type)
        {
            switch (type)
            {
                case NW4RAnimType.CHR: return SelectedCHR0;
                case NW4RAnimType.SRT: return SelectedSRT0;
                case NW4RAnimType.SHP: return SelectedSHP0;
                case NW4RAnimType.PAT: return SelectedPAT0;
                case NW4RAnimType.VIS: return SelectedVIS0;
                case NW4RAnimType.SCN: return SelectedSCN0;
                case NW4RAnimType.CLR: return SelectedCLR0;
                default:               return null;
            }
        }

        public void SetAnimation(NW4RAnimType type, NW4RAnimationNode value)
        {
            switch (type)
            {
                case NW4RAnimType.CHR:
                    SelectedCHR0 = value as CHR0Node;
                    break;
                case NW4RAnimType.SRT:
                    SelectedSRT0 = value as SRT0Node;
                    break;
                case NW4RAnimType.SHP:
                    SelectedSHP0 = value as SHP0Node;
                    break;
                case NW4RAnimType.PAT:
                    SelectedPAT0 = value as PAT0Node;
                    break;
                case NW4RAnimType.VIS:
                    SelectedVIS0 = value as VIS0Node;
                    break;
                case NW4RAnimType.SCN:
                    SelectedSCN0 = value as SCN0Node;
                    break;
                case NW4RAnimType.CLR:
                    SelectedCLR0 = value as CLR0Node;
                    break;
            }
        }

        public void SetAnimation(NW4RAnimationNode value)
        {
            if (value is CHR0Node)
            {
                SelectedCHR0 = value as CHR0Node;
            }
            else if (value is SRT0Node)
            {
                SelectedSRT0 = value as SRT0Node;
            }
            else if (value is SHP0Node)
            {
                SelectedSHP0 = value as SHP0Node;
            }
            else if (value is PAT0Node)
            {
                SelectedPAT0 = value as PAT0Node;
            }
            else if (value is VIS0Node)
            {
                SelectedVIS0 = value as VIS0Node;
            }
            else if (value is SCN0Node)
            {
                SelectedSCN0 = value as SCN0Node;
            }
            else if (value is CLR0Node)
            {
                SelectedCLR0 = value as CLR0Node;
            }
        }

        /// <param name="index">0 = X, 1 = Y, 2 = Z</param>
        /// <param name="offset">The amount to add to the current rotation displayed in the CHR0 editor box.</param>
        protected void ApplyAngle(int index, float offset)
        {
            if (offset == 0.0f)
            {
                return;
            }

            NumericInputBox box = CHR0Editor._transBoxes[index.Clamp(0, 2) + 3];
            float newVal = (float) Math.Round(box.Value + offset, 3);
            if (box.Value != newVal)
            {
                box.Value = newVal;
                CHR0Editor.BoxChanged(box, null);
            }
        }

        /// <param name="index">0 = X, 1 = Y, 2 = Z</param>
        /// <param name="offset">The amount to add to the current translation displayed in the CHR0 editor box.</param>
        protected void ApplyTranslation(int index, float offset)
        {
            if (offset == 0.0f)
            {
                return;
            }

            NumericInputBox box = CHR0Editor._transBoxes[index.Clamp(0, 2) + 6];
            float newVal = (float) Math.Round(box.Value + offset, 3);
            if (box.Value != newVal)
            {
                box.Value = newVal;
                CHR0Editor.BoxChanged(box, null);
            }
        }

        /// <param name="index">0 = X, 1 = Y, 2 = Z</param>
        /// <param name="offset">The multiplier for the current scale displayed in the CHR0 editor box.</param>
        protected void ApplyScale(int index, float scale)
        {
            if (scale == 0.0f || scale == 1.0f)
            {
                return;
            }

            NumericInputBox box = CHR0Editor._transBoxes[index.Clamp(0, 2)];
            float newVal = (float) Math.Round(box.Value * scale, 3);
            if (box.Value != newVal && newVal != 0.0f)
            {
                box.Value = newVal;
                CHR0Editor.BoxChanged(box, null);
            }
        }

        public virtual void SetFrame(int index)
        {
            if (index < 0)
            {
                return;
            }

            NW4RAnimationNode node = TargetAnimation;
            int loopMax = _maxFrame + (node != null && node.Loop && Interpolated.Contains(node.GetType()) ? 1 : 0);

            if (index > loopMax)
            {
                return;
            }

            CurrentFrame = index;

            if (PlaybackPanel != null)
            {
                if (PlaybackPanel.InvokeRequired)
                {
                    Action<int, int> d = new Action<int, int>(PlaybackPanel.UpdateInterface);
                    Invoke(d, new object[] {_animFrame, loopMax});
                }
                else
                {
                    PlaybackPanel.UpdateInterface(_animFrame, loopMax);
                }
            }

            if (!_playing)
            {
                if (InterpolationEditor != null && InterpolationEditor.Visible)
                {
                    InterpolationEditor.Frame = CurrentFrame;
                }

                KeyframePanel?.numFrame_ValueChanged();
            }
        }

        protected virtual void _timer_RenderFrame(object sender, FrameEventArgs e)
        {
            if (TargetAnimation == null)
            {
                return;
            }

            if (_animFrame >= _maxFrame)
            {
                if (!_loop)
                {
                    StopAnim();
                }
                else
                {
                    SetFrame(1);
                }
            }
            else
            {
                SetFrame(_animFrame + 1);
            }

            if (_capture)
            {
                _images.Add(ModelPanel.GetScreenshot(ModelPanel.CurrentViewport.RegionNoBorder, false));
            }
        }

        public virtual void PlayAnim()
        {
            if (TargetAnimation == null || _maxFrame == 1)
            {
                return;
            }

            _playing = true;

            if (DisableBonesWhenPlaying)
            {
                if (RenderBones == false)
                {
                    _bonesWereOff = true;
                }

                RenderBones = false;
            }

            EnableTransformEdit = false;

            if (_animFrame >= _maxFrame)
            {
                SetFrame(1);
            }

            if (PlaybackPanel != null)
            {
                PlaybackPanel.btnPlay.Text = "Stop";
                _timer.Run(0, (double) PlaybackPanel.numFPS.Value);
            }
            else
            {
                _timer.Run(0, 60);
            }
        }

        public virtual void StopAnim()
        {
            if (!_playing && !_timer.IsRunning)
            {
                return;
            }

            _timer.Stop();

            _playing = false;

            if (DisableBonesWhenPlaying)
            {
                if (!_bonesWereOff)
                {
                    RenderBones = true;
                }

                _bonesWereOff = false;
            }

            if (PlaybackPanel != null)
            {
                if (PlaybackPanel.InvokeRequired)
                {
                    Action d = () => { PlaybackPanel.btnPlay.Text = "Play"; };
                    Invoke(d);
                }
                else
                {
                    PlaybackPanel.btnPlay.Text = "Play";
                }
            }

            EnableTransformEdit = true;

            if (InterpolationEditor != null && InterpolationEditor.Visible)
            {
                InterpolationEditor.Frame = CurrentFrame;
            }

            KeyframePanel?.numFrame_ValueChanged();

            if (_capture)
            {
                RenderToGIF(_images, ScreenCaptureFolder);

                _images.Clear();
                _capture = false;

                if (InterpolationEditor != null)
                {
                    InterpolationEditor.Enabled = true;
                }

                ModelPanel.Enabled = true;
                Enabled = true;
            }
        }

        public void TryInvoke(Control c, Action a, params object[] parameters)
        {
            if (c.InvokeRequired)
            {
                Invoke(a);
            }
            else
            {
                a();
            }
        }

        /// <summary>
        /// Called only when the animation frame or frame data changes
        /// </summary>
        public virtual void UpdatePropDisplay()
        {
            switch (TargetAnimType)
            {
                case NW4RAnimType.CHR:
                    if (CHR0Editor != null)
                    {
                        TryInvoke(CHR0Editor, CHR0Editor.UpdatePropDisplay);
                    }

                    break;
                case NW4RAnimType.SRT:
                    if (SRT0Editor != null)
                    {
                        TryInvoke(SRT0Editor, SRT0Editor.UpdatePropDisplay);
                    }

                    break;
                case NW4RAnimType.SHP:
                    if (SHP0Editor != null)
                    {
                        TryInvoke(SHP0Editor, SHP0Editor.UpdatePropDisplay);
                    }

                    break;
                case NW4RAnimType.PAT:
                    if (PAT0Editor != null)
                    {
                        TryInvoke(PAT0Editor, PAT0Editor.UpdatePropDisplay);
                    }

                    break;
                case NW4RAnimType.SCN:
                    if (SCN0Editor != null)
                    {
                        TryInvoke(SCN0Editor, SCN0Editor.UpdatePropDisplay);
                    }

                    break;
                case NW4RAnimType.CLR: break;
                case NW4RAnimType.VIS:
                    if (KeyframePanel != null)
                    {
                        if (KeyframePanel.InvokeRequired)
                        {
                            Action<int> d = new Action<int>(KeyframePanel.UpdateCurrentFrame);
                            Invoke(d, new object[] {_animFrame});
                        }

                        KeyframePanel.UpdateCurrentFrame(_animFrame);
                    }

                    break;
            }
        }

        /// <summary>
        /// Applies animations to all models and invalidates the viewport.
        /// Also updates animation controls if not playing.
        /// </summary>
        public void UpdateModel()
        {
            UpdateModel(_animFrame);
        }

        /// <summary>
        /// Applies animations to all models and invalidates the viewport.
        /// Also updates animation controls if not playing.
        /// </summary>
        public virtual void UpdateModel(float frame)
        {
            if (_updating)
            {
                return;
            }

            if (EditingAll)
            {
                foreach (IModel n in _targetModels)
                {
                    UpdateModel(n, frame);
                }
            }
            else if (TargetModel != null)
            {
                UpdateModel(TargetModel, frame);
            }

            if (!_playing)
            {
                UpdatePropDisplay();
            }

            ModelPanel.Invalidate();
        }

        /// <summary>
        /// Applies target animations to the given model at the given frame.
        /// Does not update any controls.
        /// </summary>
        public virtual void UpdateModel(IModel model, float frame)
        {
            //TODO: support for applying more than one animation per type

            model.ApplyCHR(PlayCHR0 ? _chr0 : null, frame);
            model.ApplySRT(PlaySRT0 ? _srt0 : null, frame);
            model.ApplySHP(PlaySHP0 ? _shp0 : null, frame);
            model.ApplyPAT(PlayPAT0 ? _pat0 : null, frame);
            model.ApplyCLR(PlayCLR0 ? _clr0 : null, frame);

            if (_vis0 != null && PlayVIS0)
            {
                if (model == TargetModel)
                {
                    ApplyVIS0ToInterface();
                }
                else
                {
                    model.ApplyVIS(_vis0, frame);
                }
            }
            else
            {
                model.ApplyVIS(null, 0);
            }

            model.ApplySCN(PlaySCN0 ? _scn0 : null, frame);
        }

        public void UpdateKeyframePanel()
        {
            UpdateKeyframePanel(TargetAnimType);
        }

        public virtual void UpdateKeyframePanel(NW4RAnimType type)
        {
            if (KeyframePanel == null)
            {
                return;
            }

            KeyframePanel.TargetSequence = null;

            //btnRightToggle.Enabled = true;
            switch (type)
            {
                case NW4RAnimType.CHR:
                    if (_chr0 != null && SelectedBone != null)
                    {
                        KeyframePanel.TargetSequence = _chr0.FindChild(SelectedBone.Name, false);
                    }

                    break;
                case NW4RAnimType.SRT:
                    if (_srt0 != null && TargetTexRef != null)
                    {
                        KeyframePanel.TargetSequence = SRT0Editor.TexEntry;
                    }

                    break;
                case NW4RAnimType.SHP:
                    if (_shp0 != null)
                    {
                        KeyframePanel.TargetSequence = SHP0Editor.VertexSetDest;
                    }

                    break;
            }
        }

        protected virtual void UpdateSRT0FocusControls(SRT0Node node)
        {
        }

        protected virtual void UpdatePAT0FocusControls(PAT0Node node)
        {
        }

        /// <summary>
        /// Called only when the animation changes
        /// </summary>
        public void UpdateEditor()
        {
            UpdateEditor(TargetAnimType);
        }

        /// <summary>
        /// Called only when the animation changes
        /// </summary>
        public virtual void UpdateEditor(NW4RAnimType type)
        {
            if (type != NW4RAnimType.SRT)
            {
                UpdateSRT0FocusControls(null);
            }

            if (type != NW4RAnimType.PAT)
            {
                UpdatePAT0FocusControls(null);
            }

            switch (type)
            {
                case NW4RAnimType.CHR:
                    break;
                case NW4RAnimType.SRT:
                    UpdateSRT0FocusControls(SelectedSRT0);
                    break;
                case NW4RAnimType.SHP:
                    SHP0Editor?.AnimationChanged();

                    break;
                case NW4RAnimType.PAT:
                    PAT0Editor?.UpdateBoxes();

                    UpdatePAT0FocusControls(SelectedPAT0);
                    break;
                case NW4RAnimType.VIS:
                    VIS0Editor?.AnimationChanged();

                    break;
                case NW4RAnimType.SCN:
                    SCN0Editor?.tabControl1_Selected(null,
                        new TabControlEventArgs(null, SCN0Editor._tabIndex, TabControlAction.Selected));

                    break;
                case NW4RAnimType.CLR:
                    CLR0Editor?.AnimationChanged();

                    break;
            }
        }

        /// <summary>
        /// Updates controls when the target animation has changed.
        /// </summary>
        public void AnimChanged()
        {
            AnimChanged(TargetAnimType);
        }

        /// <summary>
        /// Updates controls when the target animation has changed.
        /// Does nothing if the type does not match the current target type.
        /// </summary>
        public void AnimChanged(NW4RAnimType type)
        {
            if (type != TargetAnimType && type != NW4RAnimType.None)
            {
                return;
            }

            UpdateEditor();
            UpdateKeyframePanel();

            NW4RAnimationNode node = GetAnimation(type);
            if (node == null)
            {
                _maxFrame = 0;
                EnableTransformEdit = true;

                _updating = true;

                PlaybackPanel.numFrameIndex.Maximum = _maxFrame;
                PlaybackPanel.numTotalFrames.Minimum = 0;
                PlaybackPanel.numTotalFrames.Value = 0;
                PlaybackPanel.btnPlay.Enabled =
                    PlaybackPanel.numTotalFrames.Enabled =
                        PlaybackPanel.numFrameIndex.Enabled =
                            PlaybackPanel.btnLast.Enabled =
                                PlaybackPanel.btnFirst.Enabled =
                                    PlaybackPanel.Enabled = false;

                _updating = false;

                GetFiles(NW4RAnimType.None);
                SetFrame(0);
            }
            else
            {
                int loopBias = node.Loop && Interpolated.Contains(node.GetType()) ? 1 : 0;

                if (Properties.Settings.Default.ContextualLoopAnimation)
                {
                    PlaybackPanel.chkLoop.Checked = node.Loop;
                }

                _maxFrame = node.FrameCount;
                EnableTransformEdit = !_playing;

                _updating = true;

                PlaybackPanel.btnPlay.Enabled =
                    PlaybackPanel.numFrameIndex.Enabled =
                        PlaybackPanel.numTotalFrames.Enabled =
                            PlaybackPanel.Enabled = true;
                PlaybackPanel.numTotalFrames.Minimum = loopBias + 1;
                PlaybackPanel.numTotalFrames.Value = _maxFrame + loopBias;
                PlaybackPanel.numFrameIndex.Maximum = _maxFrame + loopBias;

                _updating = false;

                GetFiles(TargetAnimType);
                SetFrame(1);
            }

            //UpdateModel();
            //UpdatePropDisplay();
            OnAnimationChanged();
        }

        public virtual void OnAnimationChanged()
        {
        }

        public static readonly Type[] Mergeable = new Type[] {typeof(CHR0Node)};

        public static readonly Type[] Appendable = new Type[]
            {typeof(CHR0Node), typeof(SRT0Node), typeof(SHP0Node), typeof(VIS0Node), typeof(PAT0Node)};

        public static readonly Type[] Resizable = new Type[]
            {typeof(CHR0Node), typeof(SRT0Node), typeof(SHP0Node), typeof(VIS0Node), typeof(PAT0Node)};

        public static readonly Type[] Interpolated = new Type[]
            {typeof(CHR0Node), typeof(SRT0Node), typeof(SHP0Node), typeof(SCN0Node)};

        #endregion

        #region BRRES

        /// <summary>
        /// Retrieves corresponding animations to the target animation type.
        /// If NW4RAnimType.None is sent as the focus type, all animations but the target animation are cleared.
        /// </summary>
        /// <param name="focusType"></param>
        public virtual void GetFiles(NW4RAnimType focusType)
        {
            if (focusType == NW4RAnimType.None)
            {
                focusType = TargetAnimType;
                if (focusType == NW4RAnimType.SCN)
                {
                    return;
                }

                for (int i = 0; i < ResourceTypeList.Length; i++)
                {
                    if ((int) focusType != i && (NW4RAnimType) i != NW4RAnimType.SCN)
                    {
                        SetAnimation((NW4RAnimType) i, null);
                    }
                }
            }
            else
            {
                if (focusType == NW4RAnimType.SCN)
                {
                    return;
                }

                for (int i = 0; i < ResourceTypeList.Length; i++)
                {
                    if ((int) focusType != i && (NW4RAnimType) i != NW4RAnimType.SCN)
                    {
                        SetCorrespondingAnimation(focusType, (NW4RAnimType) i);
                    }
                }
            }
        }

        public ResourceType[] ResourceTypeList = new ResourceType[]
        {
            ResourceType.CHR0,
            ResourceType.SRT0,
            ResourceType.SHP0,
            ResourceType.PAT0,
            ResourceType.VIS0,
            ResourceType.CLR0,
            ResourceType.SCN0
        };

        public virtual void SetCorrespondingAnimation(NW4RAnimType focusType, NW4RAnimType targetType)
        {
            _updating = true;
            NW4RAnimationNode focusFile = GetAnimation(focusType);
            SetAnimation(targetType, focusFile == null ? null : FindCorrespondingAnimation(focusFile, targetType));
            _updating = false;
        }

        protected virtual NW4RAnimationNode FindCorrespondingAnimation(NW4RAnimationNode focusFile,
                                                                       NW4RAnimType targetType)
        {
            NW4RAnimationNode node;
            if (TargetModel != null &&
                (node = FindAnimation((ResourceNode) TargetModel, focusFile.Name, targetType)) != null)
            {
                return node;
            }

            foreach (ResourceNode r in _openedFiles)
            {
                if (r != null && (node = FindAnimation(r, focusFile.Name, targetType)) != null)
                {
                    return node;
                }
            }

            return null;
        }

        private NW4RAnimationNode FindAnimation(ResourceNode searchNode, string name, NW4RAnimType targetType)
        {
            return searchNode.RootNode.FindChildByType(name, true, ResourceTypeList[(int) targetType]) as
                NW4RAnimationNode;
        }

        public void CreateVIS0()
        {
            if (!(_targetModel is MDL0Node))
            {
                return;
            }

            BRRESNode group = null;
            BRESEntryNode n = null;
            if ((n = TargetAnimation) != null &&
                (group = n.Parent?.Parent as BRRESNode) != null)
            {
                _vis0 = group.CreateResource<VIS0Node>(SelectedCHR0.Name);
                foreach (string s in VIS0Indices.Keys)
                {
                    VIS0EntryNode node = null;
                    if ((node = (VIS0EntryNode) _vis0.FindChild(s, true)) == null &&
                        ((MDL0BoneNode) ((ResourceNode) _targetModel).FindChildByType(s, true, ResourceType.MDL0Bone))
                        .BoneIndex != 0 && s != "EyeYellowM")
                    {
                        node = _vis0.CreateEntry();
                        node.Name = s;
                        node.MakeConstant(true);
                    }
                }
            }
        }

        public void UpdateVis0(int objectIndex, int drawCallIndex, bool value)
        {
            BRESEntryNode n;
            if ((n = TargetAnimation) == null ||
                _animFrame == 0 ||
                TargetModel == null)
            {
                return;
            }

            Start:
            if (_vis0 != null)
            {
                if (objectIndex < 0 || objectIndex >= TargetModel.Objects.Length)
                {
                    return;
                }

                MDL0ObjectNode obj = (MDL0ObjectNode) TargetModel.Objects[objectIndex];

                if (drawCallIndex < 0 || drawCallIndex >= obj._drawCalls.Count)
                {
                    return;
                }

                DrawCall c = obj._drawCalls[drawCallIndex];
                MDL0BoneNode bone = c._visBoneNode;

                if (bone == null)
                {
                    return;
                }

                VIS0EntryNode node = null;
                if ((node = (VIS0EntryNode) _vis0.FindChild(bone.Name, true)) == null && bone.BoneIndex != 0 &&
                    bone.Name != "EyeYellowM")
                {
                    node = _vis0.CreateEntry();
                    node.Name = bone.Name;
                    node.MakeConstant(true);
                }

                bool ANIMval = value;

                bool nowAnimated = false, alreadyConstant = false;
                Top:
                if (node != null)
                {
                    if (node._entryCount != 0) //Node is animated
                    {
                        bool VIS0val = node.GetEntry(_animFrame - 1);

                        if (VIS0val != ANIMval)
                        {
                            node.SetEntry(_animFrame - 1, ANIMval);
                        }
                    }
                    else //Node is constant
                    {
                        alreadyConstant = true;

                        bool VIS0val = node._flags.HasFlag(VIS0Flags.Enabled);

                        if (VIS0val != ANIMval)
                        {
                            node.MakeAnimated();
                            nowAnimated = true;
                            goto Top;
                        }
                    }
                }

                //Check if the entry can be made constant.
                //No point if the entry has just been made animated or if the node is already constant.
                if (node != null && !alreadyConstant && !nowAnimated)
                {
                    bool constant = true;
                    for (int i = 0; i < node._entryCount; i++)
                    {
                        if (i == 0)
                        {
                            continue;
                        }

                        if (node.GetEntry(i - 1) != node.GetEntry(i))
                        {
                            constant = false;
                            break;
                        }
                    }

                    if (constant)
                    {
                        node.MakeConstant(node.GetEntry(0));
                    }
                }

                VIS0EntryNode t = (VIS0EntryNode) KeyframePanel.visEditor.TargetNode;
                if (node != null && t != null && t.Name == node.Name)
                {
                    VIS0Editor.UpdateEntry();
                }
            }
            else
            {
                CreateVIS0();
                if (_vis0 != null)
                {
                    goto Start;
                }
            }
        }

        public bool VIS0Updating
        {
            get => _vis0Updating;
            set => _vis0Updating = value;
        }

        private bool _vis0Updating;

        public virtual void ApplyVIS0ToInterface()
        {
            TargetModel.ApplyVIS(PlayVIS0 ? _vis0 : null, _animFrame);
        }

        #endregion

        public void _interpolationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _interpolationForm = null;
            InterpolationFormOpen = false;
        }

        public virtual void UpdateAnimationPanelDimensions()
        {
        }

        protected void VISEntryChanged(object sender, EventArgs e)
        {
            UpdateModel();
        }

        protected void VISIndexChanged(object sender, EventArgs e)
        {
            int i = (KeyframePanel.visEditor.listBox1.SelectedIndex + 1).Clamp(0, MaxFrame);
            if (i != CurrentFrame)
            {
                SetFrame(i);
            }
        }

        public virtual void OpenInMainForm(ResourceNode node)
        {
        }

        public virtual bool ShouldCloseFile(ResourceNode node)
        {
            return true;
        }

        public virtual void UnloadAnimations(ResourceNode r)
        {
        }
    }
}