using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public partial class InterpolationEditor : UserControl
    {
        public ModelEditorBase _mainWindow;

        public InterpolationEditor()
        {
            _mainWindow = null;
        }

        public InterpolationEditor(ModelEditorBase mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            numFrameVal._integral = true;
            cbTransform.DataSource = _modes;

            interpolationViewer.SelectedKeyframeChanged += interpolationViewer1_SelectedKeyframeChanged;
            interpolationViewer.FrameChanged += interpolationViewer1_FrameChanged;
            interpolationViewer.SignalChange += interpolationViewer_SignalChange;

            nibTanLen.Value = interpolationViewer.TangentLength;
        }

        private void interpolationViewer_SignalChange(object sender, EventArgs e)
        {
            ((ResourceNode) _targetNode).SignalPropertyChange();
            if (_mainWindow != null)
            {
                _mainWindow.UpdatePropDisplay();
                _mainWindow.UpdateModel();
            }
        }

        private void interpolationViewer1_UpdateProps(object sender, EventArgs e)
        {
            if (_mainWindow != null)
            {
                _mainWindow.UpdatePropDisplay();
                _mainWindow.UpdateModel();
            }
        }

        private void interpolationViewer1_FrameChanged(object sender, EventArgs e)
        {
            if (_mainWindow != null && _mainWindow.CurrentFrame - 1 != interpolationViewer.FrameIndex)
            {
                _mainWindow.SetFrame((interpolationViewer.FrameIndex + 1).Clamp(1,
                    (int) _mainWindow.PlaybackPanel.numTotalFrames.Value));
            }
        }

        public BindingList<string> _modes = new BindingList<string>();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public KeyframeEntry SelectedKeyframe
        {
            get => interpolationViewer.SelectedKeyframe;
            set => interpolationViewer.SelectedKeyframe = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedMode
        {
            get
            {
                if (cbTransform.SelectedIndex == -1 || _modes.Count == 0)
                {
                    return -1;
                }

                return cbTransform.SelectedIndex;
            }
            set => cbTransform.SelectedIndex =
                value.Clamp(cbTransform.Items.Count == 0 ? -1 : 0, cbTransform.Items.Count - 1);
        }

        public void KeyframeChanged()
        {
            interpolationViewer.Invalidate();
        }

        public void RootChanged()
        {
            if (_targetNode == null || SelectedMode >= _targetNode.KeyArrays.Length || SelectedMode < 0)
            {
                return;
            }

            KeyframeArray array = _targetNode.KeyArrays[SelectedMode];
            if (array != null)
            {
                interpolationViewer.FrameLimit = array.FrameLimit;
                interpolationViewer.KeyRoot = array._keyRoot;
            }
        }

        public IKeyframeSource _targetNode;

        public void SetTarget(IKeyframeSource node)
        {
            int selectedCB = cbTransform.SelectedIndex;
            if ((_targetNode = node) != null)
            {
                panel1.Enabled = true;
                cbTransform.Enabled = true;
                if (node is SCN0EntryNode)
                {
                    if (node is SCN0LightNode)
                    {
                        //chkGenTans.Checked = SCN0LightNode._generateTangents;

                        if (_modes.Count != 10)
                        {
                            _updating = true;
                            _modes.Clear();
                            _modes.Add("Start X");
                            _modes.Add("Start Y");
                            _modes.Add("Start Z");
                            _modes.Add("End X");
                            _modes.Add("End Y");
                            _modes.Add("End Z");
                            _modes.Add("Spotlight Cutoff");
                            _modes.Add("Spotlight Brightness");
                            _modes.Add("Reference Distance");
                            _modes.Add("Reference Brightness");

                            cbTransform.SelectedIndex = 0;
                            _updating = false;
                        }
                    }
                    else if (node is SCN0FogNode)
                    {
                        //chkGenTans.Checked = SCN0FogNode._generateTangents;

                        if (_modes.Count != 2)
                        {
                            _updating = true;
                            _modes.Clear();
                            _modes.Add("Start Z");
                            _modes.Add("End Z");

                            cbTransform.SelectedIndex = 0;
                            _updating = false;
                        }
                    }
                    else if (node is SCN0CameraNode)
                    {
                        //chkGenTans.Checked = SCN0CameraNode._generateTangents;   

                        if (_modes.Count != 15)
                        {
                            _updating = true;
                            _modes.Clear();
                            _modes.Add("Position X");
                            _modes.Add("Position Y");
                            _modes.Add("Position Z");
                            _modes.Add("Aspect");
                            _modes.Add("Near Z");
                            _modes.Add("Far Z");
                            _modes.Add("Rotation X");
                            _modes.Add("Rotation Y");
                            _modes.Add("Rotation Z");
                            _modes.Add("Aim X");
                            _modes.Add("Aim Y");
                            _modes.Add("Aim Z");
                            _modes.Add("Twist");
                            _modes.Add("Vertical Field of View");
                            _modes.Add("Height");

                            cbTransform.SelectedIndex = 0;
                            _updating = false;
                        }
                    }
                }
                else if (node is IKeyframeSource)
                {
                    if (node is CHR0EntryNode)
                    {
                        //chkGenTans.Checked = CHR0EntryNode._generateTangents;

                        if (_modes.Count != 9)
                        {
                            _updating = true;
                            _modes.Clear();
                            _modes.Add("Scale X");
                            _modes.Add("Scale Y");
                            _modes.Add("Scale Z");
                            _modes.Add("Rotation X");
                            _modes.Add("Rotation Y");
                            _modes.Add("Rotation Z");
                            _modes.Add("Translation X");
                            _modes.Add("Translation Y");
                            _modes.Add("Translation Z");

                            cbTransform.SelectedIndex = 0;
                            _updating = false;
                        }
                    }
                    else if (node is SRT0TextureNode)
                    {
                        //chkGenTans.Checked = SRT0TextureNode._generateTangents;

                        if (_modes.Count != 5)
                        {
                            _updating = true;
                            _modes.Clear();
                            _modes.Add("Scale X");
                            _modes.Add("Scale Y");
                            _modes.Add("Rotation");
                            _modes.Add("Translation X");
                            _modes.Add("Translation Y");

                            cbTransform.SelectedIndex = 0;
                            _updating = false;
                        }
                    }
                    else if (node is SHP0VertexSetNode)
                    {
                        //chkGenTans.Checked = SHP0VertexSetNode._generateTangents;

                        if (_modes.Count != 1)
                        {
                            _updating = true;

                            _modes.Clear();
                            _modes.Add("Percentage");

                            cbTransform.SelectedIndex = 0;

                            _updating = false;
                        }
                    }
                }

                if (cbTransform.Items.Count > selectedCB && selectedCB >= 0)
                {
                    cbTransform.SelectedIndex = selectedCB;
                }
                else if (cbTransform.Items.Count > 0)
                {
                    cbTransform.SelectedIndex = 0;
                }
                else
                {
                    cbTransform.SelectedIndex = -1;
                }

                cbTransform.Enabled = cbTransform.Items.Count > 1;
                RootChanged();
            }
            else
            {
                interpolationViewer.KeyRoot = null;
                panel1.Enabled = false;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Frame
        {
            get => interpolationViewer.FrameIndex + 1;
            set
            {
                if (interpolationViewer.FrameIndex == value - 1)
                {
                    return;
                }

                interpolationViewer._updating = true;
                numFrameVal.Value = value;
                interpolationViewer.FrameIndex = value - 1;
                interpolationViewer._updating = false;
            }
        }

        private bool _updating;

        private void interpolationViewer1_SelectedKeyframeChanged(object sender, EventArgs e)
        {
            _updating = true;
            KeyframeEntry key = interpolationViewer.SelectedKeyframe;
            if (key != null)
            {
                bool indexChanged = numFrameVal.Value != key._index + 1;

                groupBox1.Enabled = true;
                numFrameVal.Value = key._index + 1;

                bool prevIn = key._prev._index == key._index;
                bool nextOut = key._next._index == key._index;

                if (!prevIn && !nextOut)
                {
                    chkUseOut.Checked = false;
                    numInTan.Value = key._tangent;
                    numInValue.Value = key._value;
                    numInFrame.Value = key._index + 1;

                    numOutTan.Value = 0.0f;
                    numOutVal.Value = 0.0f;
                    numOutFrame.Value = 0.0f;
                }
                else if (prevIn)
                {
                    chkUseOut.Checked = true;

                    numOutTan.Value = key._tangent;
                    numOutVal.Value = key._value;
                    numInFrame.Value = key._index + 1;

                    numInTan.Value = key._prev._tangent;
                    numInValue.Value = key._prev._value;
                    numOutFrame.Value = key._prev._index + 1;
                }
                else
                {
                    chkUseOut.Checked = true;

                    numInTan.Value = key._tangent;
                    numInValue.Value = key._value;
                    numInFrame.Value = key._index + 1;

                    numOutTan.Value = key._next._tangent;
                    numOutVal.Value = key._next._value;
                    numOutFrame.Value = key._next._index + 1;
                }

                if (chkSetFrame.Checked)
                {
                    interpolationViewer1_FrameChanged(this, null);
                }

                if (_mainWindow?.KeyframePanel != null)
                {
                    if (indexChanged)
                    {
                        _mainWindow.KeyframePanel.UpdateKeyframes();
                    }
                    else
                    {
                        _mainWindow.KeyframePanel.UpdateKeyframe(interpolationViewer.SelectedKeyframe._index);
                    }
                }

                //_mainWindow.UpdatePropDisplay();
                //_mainWindow.UpdateModel();
            }
            else
            {
                groupBox1.Enabled = false;
                numInTan.Value = 0;
                numInValue.Value = 0;
                numFrameVal.Value = 0;
            }

            _updating = false;
        }

        private void numInTan_ValueChanged(object sender, EventArgs e)
        {
            if (_updating || interpolationViewer.SelectedKeyframe == null)
            {
                return;
            }

            KeyframeEntry kf = interpolationViewer.SelectedKeyframe;
            if (kf.Second != null && kf._prev._index == kf._index)
            {
                kf = kf._prev;
            }

            kf._tangent = numInTan.Value;

            interpolationViewer.Invalidate();
            ((ResourceNode) _targetNode).SignalPropertyChange();

            if (chkSyncStartEnd.Checked)
            {
                if (SelectedKeyframe._prev._index == -1 && SelectedKeyframe._prev._prev != SelectedKeyframe)
                {
                    SelectedKeyframe._prev._prev._tangent = SelectedKeyframe._tangent;
                    SelectedKeyframe._prev._prev._value = SelectedKeyframe._value;
                }

                if (SelectedKeyframe._next._index == -1 && SelectedKeyframe._next._next != SelectedKeyframe)
                {
                    SelectedKeyframe._next._next._tangent = SelectedKeyframe._tangent;
                    SelectedKeyframe._next._next._value = SelectedKeyframe._value;
                }
            }
        }

        private void numFrameVal_ValueChanged(object sender, EventArgs e)
        {
            if (_updating || interpolationViewer.SelectedKeyframe == null)
            {
                return;
            }

            int end = 0;
            if (_mainWindow != null)
            {
                end = (int) _mainWindow.PlaybackPanel.numTotalFrames.Value - 1;
            }
            else
            {
                end = _targetNode.FrameCount - 1;
            }

            int index = ((int) numFrameVal.Value - 1).Clamp(0, end);

            Frame = index + 1;
        }

        private void numInValue_ValueChanged(object sender, EventArgs e)
        {
            if (_updating || interpolationViewer.SelectedKeyframe == null)
            {
                return;
            }

            KeyframeEntry kf = interpolationViewer.SelectedKeyframe;
            if (kf.Second != null && kf._prev._index == kf._index)
            {
                kf = kf._prev;
            }

            kf._value = numInValue.Value;

            interpolationViewer.Invalidate();
            ((ResourceNode) _targetNode).SignalPropertyChange();
            _mainWindow?.KeyframePanel.UpdateKeyframe(interpolationViewer.SelectedKeyframe._index);

            if (chkSyncStartEnd.Checked)
            {
                if (SelectedKeyframe._prev._index == -1 && SelectedKeyframe._prev._prev != SelectedKeyframe)
                {
                    SelectedKeyframe._prev._prev._tangent = SelectedKeyframe._tangent;
                    SelectedKeyframe._prev._prev._value = SelectedKeyframe._value;
                }

                if (SelectedKeyframe._next._index == -1 && SelectedKeyframe._next._next != SelectedKeyframe)
                {
                    SelectedKeyframe._next._next._tangent = SelectedKeyframe._tangent;
                    SelectedKeyframe._next._next._value = SelectedKeyframe._value;
                }
            }

            if (_mainWindow != null)
            {
                _mainWindow.UpdateModel();
                _mainWindow.UpdatePropDisplay();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            RootChanged();

            if (SelectedKeyframe != null && interpolationViewer.KeyRoot != null)
            {
                KeyframeEntry prev = SelectedKeyframe;
                for (KeyframeEntry entry = interpolationViewer.KeyRoot._next;
                    entry != interpolationViewer.KeyRoot;
                    entry = entry._next)
                {
                    if (entry._index == SelectedKeyframe._index)
                    {
                        SelectedKeyframe = entry;
                        break;
                    }
                }

                if (SelectedKeyframe == prev)
                {
                    SelectedKeyframe = null;
                }
            }
        }

        private void chkAllKeys_CheckedChanged(object sender, EventArgs e)
        {
            interpolationViewer.DisplayAllKeyframes = !chkViewOne.Checked;
        }

        private void chkGenTans_CheckedChanged(object sender, EventArgs e)
        {
            interpolationViewer.GenerateTangents = chkGenTans.Checked;

            //if (_targetNode is CHR0EntryNode)
            //    CHR0EntryNode._generateTangents = chkGenTans.Checked;
            //else if (_targetNode is SRT0TextureNode)
            //    SRT0TextureNode._generateTangents = chkGenTans.Checked;
            //else if (_targetNode is SHP0VertexSetNode)
            //    SHP0VertexSetNode._generateTangents = chkGenTans.Checked;
            //else if (_targetNode is SCN0LightNode)
            //    SCN0LightNode._generateTangents = chkGenTans.Checked;
            //else if (_targetNode is SCN0CameraNode)
            //    SCN0CameraNode._generateTangents = chkGenTans.Checked;
            //else if (_targetNode is SCN0FogNode)
            //    SCN0FogNode._generateTangents = chkGenTans.Checked;
        }

        private void chkKeyDrag_CheckedChanged(object sender, EventArgs e)
        {
            interpolationViewer.KeyDraggingAllowed = chkKeyDrag.Checked;
        }

        private void chkRenderTans_CheckedChanged(object sender, EventArgs e)
        {
            interpolationViewer.DrawTangents = chkRenderTans.Checked;
        }

        private void chkSyncStartEnd_CheckedChanged(object sender, EventArgs e)
        {
            interpolationViewer.SyncStartEnd = chkSyncStartEnd.Checked;
        }

        private void numTanLen_ValueChanged(object sender, EventArgs e)
        {
            interpolationViewer.TangentLength = nibTanLen.Value;
        }

        private void mItem_genTan_alterSelTanOnDrag_CheckedChanged(object sender, EventArgs e)
        {
            interpolationViewer.AlterSelectedTangent_OnDrag =
                mItem_genTan_alterSelTanOnDrag.Checked && CHR0EntryNode._generateTangents;
        }

        private void mItem_genTan_alterAdjTan_CheckedChanged(object sender, EventArgs e)
        {
            CHR0EntryNode._alterAdjTangents = mItem_genTan_alterAdjTan.Checked;

            interpolationViewer.AlterAdjTangent_OnSelectedDrag =
                CHR0EntryNode._generateTangents && CHR0EntryNode._alterAdjTangents &&
                mItem_genTan_alterAdjTan_OnDrag.Checked;
        }

        private void mItem_genTan_alterAdjTan_OnSet_CheckedChanged(object sender, EventArgs e)
        {
            CHR0EntryNode._alterAdjTangents_KeyFrame_Set = mItem_genTan_alterAdjTan_OnSet.Checked;
        }

        private void mItem_genTan_alterAdjTan_OnDel_CheckedChanged(object sender, EventArgs e)
        {
            CHR0EntryNode._alterAdjTangents_KeyFrame_Del = mItem_genTan_alterAdjTan_OnDel.Checked;
        }

        private void mItem_genTan_alterAdjTan_OnDrag_CheckedChanged(object sender, EventArgs e)
        {
            interpolationViewer.AlterAdjTangent_OnSelectedDrag =
                CHR0EntryNode._generateTangents && CHR0EntryNode._alterAdjTangents &&
                mItem_genTan_alterAdjTan_OnDrag.Checked;
        }

        private void chkLinear_Click(object sender, EventArgs e)
        {
        }

        private void chkSmooth_Click(object sender, EventArgs e)
        {
        }

        private void chkFlat_Click(object sender, EventArgs e)
        {
        }

        private void chkBreakKey_Click(object sender, EventArgs e)
        {
            chkUseOut.Checked = !chkUseOut.Checked;
        }

        private void numPrecision_ValueChanged(object sender, EventArgs e)
        {
            interpolationViewer.Precision = numPrecision.Value;
        }

        private void numOutTan_ValueChanged(object sender, EventArgs e)
        {
            if (_updating || interpolationViewer.SelectedKeyframe == null)
            {
                return;
            }

            KeyframeEntry kf = interpolationViewer.SelectedKeyframe;
            if (kf.Second != null && kf._next._index == kf._index)
            {
                kf = kf._next;
            }

            kf._tangent = numOutTan.Value;
            interpolationViewer.Invalidate();
            ((ResourceNode) _targetNode).SignalPropertyChange();

            if (chkSyncStartEnd.Checked)
            {
                if (SelectedKeyframe._prev._index == -1 && SelectedKeyframe._prev._prev != SelectedKeyframe)
                {
                    SelectedKeyframe._prev._prev._tangent = SelectedKeyframe._tangent;
                    SelectedKeyframe._prev._prev._value = SelectedKeyframe._value;
                }

                if (SelectedKeyframe._next._index == -1 && SelectedKeyframe._next._next != SelectedKeyframe)
                {
                    SelectedKeyframe._next._next._tangent = SelectedKeyframe._tangent;
                    SelectedKeyframe._next._next._value = SelectedKeyframe._value;
                }
            }
        }

        private void numOutVal_ValueChanged(object sender, EventArgs e)
        {
            if (_updating || interpolationViewer.SelectedKeyframe == null)
            {
                return;
            }

            KeyframeEntry kf = interpolationViewer.SelectedKeyframe;
            if (kf.Second != null && kf._next._index == kf._index)
            {
                kf = kf._next;
            }

            kf._value = numOutVal.Value;
            interpolationViewer.Invalidate();
            ((ResourceNode) _targetNode).SignalPropertyChange();
            _mainWindow?.KeyframePanel.UpdateKeyframe(interpolationViewer.SelectedKeyframe._index);

            if (chkSyncStartEnd.Checked)
            {
                if (SelectedKeyframe._prev._index == -1 && SelectedKeyframe._prev._prev != SelectedKeyframe)
                {
                    SelectedKeyframe._prev._prev._tangent = SelectedKeyframe._tangent;
                    SelectedKeyframe._prev._prev._value = SelectedKeyframe._value;
                }

                if (SelectedKeyframe._next._index == -1 && SelectedKeyframe._next._next != SelectedKeyframe)
                {
                    SelectedKeyframe._next._next._tangent = SelectedKeyframe._tangent;
                    SelectedKeyframe._next._next._value = SelectedKeyframe._value;
                }
            }

            if (_mainWindow != null)
            {
                _mainWindow.UpdateModel();
                _mainWindow.UpdatePropDisplay();
            }
        }

        private void chkTanStrength_Click(object sender, EventArgs e)
        {
        }

        private void chkTanAngle_Click(object sender, EventArgs e)
        {
        }

        private void chkUseOut_CheckedChanged(object sender, EventArgs e)
        {
            numOutVal.Enabled = numOutTan.Enabled = numOutFrame.Enabled = chkUseOut.Checked;

            if (!_updating)
            {
                if (chkUseOut.Checked)
                {
                    SelectedKeyframe.InsertAfter(new KeyframeEntry(SelectedKeyframe._index, SelectedKeyframe._value)
                        {_tangent = SelectedKeyframe._tangent});
                }
                else
                {
                    KeyframeEntry second = SelectedKeyframe.Second;
                    second?.Remove();
                }

                interpolationViewer.Invalidate();
            }
        }

        private void numInFrame_ValueChanged(object sender, EventArgs e)
        {
            if (_updating || interpolationViewer.SelectedKeyframe == null)
            {
                return;
            }

            KeyframeEntry kf = interpolationViewer.SelectedKeyframe;
            if (kf.Second != null && kf._prev._index == kf._index)
            {
                kf = kf._prev;
            }

            int prev = kf._prev._index + 1;
            int next = kf._next._index - 1;

            if (next < 0)
            {
                if (_mainWindow != null)
                {
                    next = (int) _mainWindow.PlaybackPanel.numTotalFrames.Value - 1;
                }
                else
                {
                    next = _targetNode.FrameCount - 1;
                }
            }

            int index = ((int) numFrameVal.Value - 1).Clamp(prev, next);

            kf._index = index;

            interpolationViewer.Invalidate();
            ((ResourceNode) _targetNode).SignalPropertyChange();
        }

        private void numOutFrame_ValueChanged(object sender, EventArgs e)
        {
            if (_updating || interpolationViewer.SelectedKeyframe == null)
            {
                return;
            }

            KeyframeEntry kf = interpolationViewer.SelectedKeyframe;
            if (kf.Second != null && kf._next._index == kf._index)
            {
                kf = kf._next;
            }

            int prev = kf._prev._index + 1;
            int next = kf._next._index - 1;

            if (next < 0)
            {
                if (_mainWindow != null)
                {
                    next = (int) _mainWindow.PlaybackPanel.numTotalFrames.Value - 1;
                }
                else
                {
                    next = _targetNode.FrameCount - 1;
                }
            }

            int index = ((int) numFrameVal.Value - 1).Clamp(prev, next);

            kf._index = index;

            interpolationViewer.Invalidate();
            ((ResourceNode) _targetNode).SignalPropertyChange();
        }
    }
}