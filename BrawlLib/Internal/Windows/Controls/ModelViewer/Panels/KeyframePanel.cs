using BrawlLib.Imaging;
using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using BrawlLib.Modeling;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.Panels
{
    public enum FrameType
    {
        Keyframe,
        Boolean,
        Color,
        None
    }

    public partial class KeyframePanel : UserControl
    {
        public ModelEditorBase _mainWindow;

        public KeyframePanel()
        {
            InitializeComponent();
            clrControl.CurrentColorChanged += clrControl_CurrentColorChanged;
        }

        private void clrControl_CurrentColorChanged(object sender, EventArgs e)
        {
            _mainWindow.UpdateModel();
        }

        private int _currentPage = 1;
        private ResourceNode _target;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ResourceNode TargetSequence
        {
            get => _target;
            set
            {
                if (_target == value)
                {
                    return;
                }

                _target = value;
                SetTarget();
            }
        }

        public void UpdateKeyframes()
        {
            listKeyframes.Items.Clear();
            bool t = _updating;
            _updating = true;
            if (_target is CHR0EntryNode || _target is SRT0TextureNode)
            {
                IKeyframeSource entry = _target as IKeyframeSource;
                if (entry.FrameCount > 0)
                {
                    if (_target is CHR0EntryNode)
                    {
                        CHRAnimationFrame a;
                        for (int x = 0; x < entry.FrameCount; x++)
                        {
                            if ((a = ((CHR0EntryNode) entry).GetAnimFrame(x)).HasKeys)
                            {
                                listKeyframes.Items.Add(a);
                            }
                        }
                    }
                    else if (_target is SRT0TextureNode)
                    {
                        SRTAnimationFrame a;
                        for (int x = 0; x < entry.FrameCount; x++)
                        {
                            if ((a = ((SRT0TextureNode) entry).GetAnimFrame(x)).HasKeys)
                            {
                                listKeyframes.Items.Add(a);
                            }
                        }
                    }
                }
            }
            else if (_target is SHP0VertexSetNode)
            {
                SHP0VertexSetNode e = _target as SHP0VertexSetNode;
                if (e.FrameCount > 0)
                {
                    for (KeyframeEntry entry = e.Keyframes._keyRoot._next;
                        entry != e.Keyframes._keyRoot;
                        entry = entry._next)
                    {
                        listKeyframes.Items.Add(new FloatKeyframe(entry));
                    }
                }
            }
            else if (_target is SCN0EntryNode)
            {
                if (_target is SCN0CameraNode)
                {
                    CameraAnimationFrame a;
                    SCN0CameraNode entry = _target as SCN0CameraNode;
                    if (entry.FrameCount > 0)
                    {
                        for (int x = 0; x < entry.FrameCount; x++)
                        {
                            if ((a = entry.GetAnimFrame(x)).HasKeys)
                            {
                                listKeyframes.Items.Add(a);
                            }
                        }
                    }
                }
                else if (_target is SCN0LightNode)
                {
                    LightAnimationFrame a;
                    SCN0LightNode entry = _target as SCN0LightNode;
                    if (entry.FrameCount > 0)
                    {
                        for (int x = 0; x < entry.FrameCount; x++)
                        {
                            if ((a = entry.GetAnimFrame(x)).HasKeys)
                            {
                                listKeyframes.Items.Add(a);
                            }
                        }
                    }
                }
                else if (_target is SCN0FogNode)
                {
                    FogAnimationFrame a;
                    SCN0FogNode entry = _target as SCN0FogNode;
                    if (entry.FrameCount > 0)
                    {
                        for (int x = 0; x < entry.FrameCount; x++)
                        {
                            if ((a = entry.GetAnimFrame(x)).HasKeys)
                            {
                                listKeyframes.Items.Add(a);
                            }
                        }
                    }
                }
            }

            _updating = t;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            UpdateKeyframes();
        }

        public void UpdateKeyframe(int x)
        {
            if (!Visible)
            {
                return;
            }

            _updating = true;
            if (_target is CHR0EntryNode || _target is SRT0TextureNode)
            {
                IKeyframeSource entry = _target as IKeyframeSource;
                for (int w = 0; w < listKeyframes.Items.Count; w++)
                {
                    if (_target is CHR0EntryNode)
                    {
                        CHRAnimationFrame a = (CHRAnimationFrame) listKeyframes.Items[w];
                        if (a.Index == x)
                        {
                            CHRAnimationFrame r = ((CHR0EntryNode) entry).GetAnimFrame(x);

                            if (r.HasKeys)
                            {
                                listKeyframes.Items[w] = r;
                            }
                            else
                            {
                                listKeyframes.Items.RemoveAt(w);
                            }

                            _updating = false;
                            return;
                        }
                    }
                    else if (_target is SRT0TextureNode)
                    {
                        SRTAnimationFrame a = (SRTAnimationFrame) listKeyframes.Items[w];
                        if (a.Index == x)
                        {
                            SRTAnimationFrame r = ((SRT0TextureNode) entry).GetAnimFrame(x);

                            if (r.HasKeys)
                            {
                                listKeyframes.Items[w] = r;
                            }
                            else
                            {
                                listKeyframes.Items.RemoveAt(w);
                            }

                            _updating = false;
                            return;
                        }
                    }
                }

                UpdateKeyframes();
            }
            else if (_target is SHP0VertexSetNode)
            {
                SHP0VertexSetNode entry = _target as SHP0VertexSetNode;
                int w = 0;
                foreach (FloatKeyframe a in listKeyframes.Items)
                {
                    if (a.Index == x)
                    {
                        KeyframeEntry e = entry.GetKeyframe(x);

                        if (e != null)
                        {
                            listKeyframes.Items[w] = new FloatKeyframe(e);
                        }
                        else
                        {
                            listKeyframes.Items.RemoveAt(w);
                        }

                        _updating = false;
                        return;
                    }

                    w++;
                }

                UpdateKeyframes();
            }
            else if (_target is SCN0EntryNode)
            {
                if (_target is SCN0CameraNode)
                {
                    SCN0CameraNode entry = _target as SCN0CameraNode;
                    for (int w = 0; w < listKeyframes.Items.Count; w++)
                    {
                        CameraAnimationFrame a = (CameraAnimationFrame) listKeyframes.Items[w];
                        if (a.Index == x)
                        {
                            CameraAnimationFrame r = entry.GetAnimFrame(x);

                            if (r.HasKeys)
                            {
                                listKeyframes.Items[w] = r;
                            }
                            else
                            {
                                listKeyframes.Items.RemoveAt(w);
                            }

                            _updating = false;
                            return;
                        }
                    }

                    UpdateKeyframes();
                }
                else if (_target is SCN0LightNode)
                {
                    SCN0LightNode entry = _target as SCN0LightNode;
                    for (int w = 0; w < listKeyframes.Items.Count; w++)
                    {
                        LightAnimationFrame a = (LightAnimationFrame) listKeyframes.Items[w];
                        if (a.Index == x)
                        {
                            LightAnimationFrame r = entry.GetAnimFrame(x);

                            if (r.HasKeys)
                            {
                                listKeyframes.Items[w] = r;
                            }
                            else
                            {
                                listKeyframes.Items.RemoveAt(w);
                            }

                            _updating = false;
                            return;
                        }
                    }

                    UpdateKeyframes();
                }
                else if (_target is SCN0FogNode)
                {
                    SCN0FogNode entry = _target as SCN0FogNode;
                    for (int w = 0; w < listKeyframes.Items.Count; w++)
                    {
                        FogAnimationFrame a = (FogAnimationFrame) listKeyframes.Items[w];
                        if (a.Index == x)
                        {
                            FogAnimationFrame r = entry.GetAnimFrame(x);

                            if (r.HasKeys)
                            {
                                listKeyframes.Items[w] = r;
                            }
                            else
                            {
                                listKeyframes.Items.RemoveAt(w);
                            }

                            _updating = false;
                            return;
                        }
                    }

                    UpdateKeyframes();
                }
            }

            _updating = false;
        }

        private void SetTarget()
        {
            clrControl.ColorSource = null;
            visEditor.TargetNode = null;
            int temp = lstTypes.SelectedIndex;
            lstTypes.Items.Clear();
            listKeyframes.Items.Clear();

            if (_target is IKeyframeSource)
            {
                listKeyframes.BeginUpdate();
                if (_target != null)
                {
                    if (_target is CHR0EntryNode ||
                        _target is SRT0TextureNode ||
                        _target is SHP0VertexSetNode ||
                        _target is SCN0EntryNode)
                    {
                        lstTypes.Items.Add("Keyframes");
                        UpdateKeyframes();
                    }
                }

                listKeyframes.EndUpdate();
            }

            if (_target is IColorSource) //NOT else if
            {
                clrControl.ColorSource = _target as IColorSource;
                if (_target is SCN0LightNode)
                {
                    lstTypes.Items.Add("Color");
                    lstTypes.Items.Add("SpecularColor");
                }
                else
                {
                    for (int i = 0; i < clrControl.ColorSource.TypeCount; i++)
                    {
                        lstTypes.Items.Add($"ColorSource{i}");
                    }
                }
            }

            if (_target is IBoolArraySource) //NOT else if
            {
                visEditor.TargetNode = _target as IBoolArraySource;
                lstTypes.Items.Add("Visibility");
            }

            if (lstTypes.Items.Count > 0)
            {
                Enabled = true;
                temp = temp.Clamp(0, lstTypes.Items.Count - 1);
                if (lstTypes.SelectedIndex == temp)
                {
                    lstTypes_SelectedIndexChanged(this, null);
                }
                else
                {
                    lstTypes.SelectedIndex = temp;
                }
            }
            else
            {
                Enabled = false;
            }

            numFrame_ValueChanged();
            RefreshPage();
        }

        public int FrameIndex
        {
            get
            {
                if (_mainWindow != null)
                {
                    return _mainWindow.CurrentFrame;
                }

                return -1;
            }
            set
            {
                if (_mainWindow != null)
                {
                    _mainWindow.CurrentFrame = value;
                }
            }
        }

        public void numFrame_ValueChanged()
        {
            int page = FrameIndex - 1;
            if (_currentPage != page)
            {
                _currentPage = page;
                RefreshPage();
            }
        }

        private void RefreshPage()
        {
            if (_target != null)
            {
                listKeyframes.SelectedIndex = FindKeyframe(_currentPage);
            }
        }

        public int FindKeyframe(int index)
        {
            int count = listKeyframes.Items.Count;
            for (int i = 0; i < count; i++)
            {
                object x = listKeyframes.Items[i];
                if (x is CHRAnimationFrame)
                {
                    if (((CHRAnimationFrame) x).Index == index)
                    {
                        return i;
                    }
                }
                else if (x is FloatKeyframe)
                {
                    if (((FloatKeyframe) x).Index == index)
                    {
                        return i;
                    }
                }
                else if (x is CameraAnimationFrame)
                {
                    if (((CameraAnimationFrame) x).Index == index)
                    {
                        return i;
                    }
                }
                else if (x is LightAnimationFrame)
                {
                    if (((LightAnimationFrame) x).Index == index)
                    {
                        return i;
                    }
                }
                else if (x is FogAnimationFrame)
                {
                    if (((FogAnimationFrame) x).Index == index)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public bool _updating;

        private void listKeyframes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            int index = listKeyframes.SelectedIndex;
            if (index >= 0)
            {
                object x = listKeyframes.SelectedItem;
                float i = 0;
                if (x is CHRAnimationFrame)
                {
                    i = ((CHRAnimationFrame) listKeyframes.SelectedItem).Index + 1;
                }
                else if (x is FloatKeyframe)
                {
                    i = ((FloatKeyframe) listKeyframes.SelectedItem).Index + 1;
                }
                else if (x is CameraAnimationFrame)
                {
                    i = ((CameraAnimationFrame) listKeyframes.SelectedItem).Index + 1;
                }
                else if (x is LightAnimationFrame)
                {
                    i = ((LightAnimationFrame) listKeyframes.SelectedItem).Index + 1;
                }
                else if (x is FogAnimationFrame)
                {
                    i = ((FogAnimationFrame) listKeyframes.SelectedItem).Index + 1;
                }

                if (_mainWindow.CurrentFrame != i)
                {
                    _mainWindow.SetFrame((int) i);
                }
            }
        }

        public void UpdateVisEntry()
        {
            visEditor.listBox1.BeginUpdate();
            visEditor.listBox1.Items.Clear();

            if (visEditor.TargetNode != null && visEditor.TargetNode.EntryCount > -1)
            {
                for (int i = 0; i < visEditor.TargetNode.EntryCount; i++)
                {
                    visEditor.listBox1.Items.Add(visEditor.TargetNode.GetEntry(i));
                }
            }

            visEditor.listBox1.EndUpdate();
        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (visEditor.Visible && visEditor.TargetNode != null && !_updating)
            {
                visEditor.TargetNode.Enabled = chkEnabled.Checked;
                UpdateVisEntry();
                _mainWindow.UpdateModel();
            }
        }

        private void chkConstant_CheckedChanged(object sender, EventArgs e)
        {
            chkEnabled.Enabled = chkConstant.Checked;
            if (visEditor.Visible && visEditor.TargetNode != null && !_updating)
            {
                visEditor.TargetNode.Constant = chkConstant.Checked;
                UpdateVisEntry();
            }
            else if (clrControl.Visible && clrControl.ColorSource != null && !_updating)
            {
                clrControl.ColorSource.SetColorConstant(clrControl.ColorID, chkConstant.Checked);
                clrControl.ColorID = clrControl.ColorID;
            }

            if (!_updating)
            {
                _mainWindow.UpdateModel();
            }
        }

        private void lstTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTypes.Text.Contains("Color"))
            {
                int i = lstTypes.SelectedIndex;
                int z = 0;

                for (int x = 0; x < i; x++)
                {
                    if (lstTypes.Items[x].ToString().Contains("Color"))
                    {
                        z++;
                    }
                }

                clrControl.ColorID = z;

                ctrlPanel.Visible = true;

                visChkPanel.Visible = true;
                clrControl.Visible = true;
                chkEnabled.Visible = false;

                clrControl.Visible = true;
                listKeyframes.Visible = false;
                visEditor.Visible = false;

                if (clrControl.ColorSource != null)
                {
                    _updating = true;
                    chkConstant.Checked = clrControl.ColorSource.GetColorConstant(z);
                    _updating = false;
                }
            }
            else if (lstTypes.Text.Contains("Keyframes"))
            {
                visChkPanel.Visible = false;
                listKeyframes.Visible = true;

                ctrlPanel.Visible = lstTypes.Items.Count > 1;

                clrControl.Visible = false;
                listKeyframes.Visible = true;
                visEditor.Visible = false;
            }
            else if (lstTypes.Text.Contains("Visibility"))
            {
                visChkPanel.Visible = true;
                visEditor.Visible = true;
                chkEnabled.Visible = true;

                ctrlPanel.Visible = true;

                clrControl.Visible = false;
                listKeyframes.Visible = false;
                visEditor.Visible = true;

                if (visEditor.TargetNode != null)
                {
                    _updating = true;
                    chkConstant.Checked = visEditor.TargetNode.Constant;
                    chkEnabled.Checked = visEditor.TargetNode.Enabled;
                    _updating = false;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get => _mainWindow.TargetModel;
            set => _mainWindow.TargetModel = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CHR0Node SelectedCHR0
        {
            get => _mainWindow.SelectedCHR0;
            set => _mainWindow.SelectedCHR0 = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBoneNode SelectedBone
        {
            get => _mainWindow.SelectedBone;
            set => _mainWindow.SelectedBone = value;
        }

        public void UpdateCurrentFrame(int frame)
        {
            if (visEditor.TargetNode != null && !visEditor.TargetNode.Constant)
            {
                visEditor._updating = true;
                visEditor.listBox1.SelectedIndices.Clear();
                visEditor.listBox1.SelectedIndex = frame - 1;
                visEditor._updating = false;
            }
        }
    }
}