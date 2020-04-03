using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Internal.Windows.Controls.ModelViewer.Editors;
using BrawlLib.Internal.Windows.Controls.ModelViewer.Panels;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types.Animations;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase
{
    public partial class ModelEditorBase : UserControl
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FirstPersonCamera
        {
            get => ModelPanel.FirstPersonCamera;
            set => ModelPanel.FirstPersonCamera = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderFloor
        {
            get => ModelPanel.RenderFloor;
            set => ModelPanel.RenderFloor = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderMetals
        {
            get => ModelPanel.RenderMetals;
            set => ModelPanel.RenderMetals = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderBones
        {
            get => ModelPanel.RenderBones;
            set => ModelPanel.RenderBones = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ApplyBillboardBones
        {
            get => ModelPanel.ApplyBillboardBones;
            set => ModelPanel.ApplyBillboardBones = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderVertices
        {
            get => ModelPanel.RenderVertices;
            set => ModelPanel.RenderVertices = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderNormals
        {
            get => ModelPanel.RenderNormals;
            set => ModelPanel.RenderNormals = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderPolygons
        {
            get => ModelPanel.RenderPolygons;
            set => ModelPanel.RenderPolygons = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderWireframe
        {
            get => ModelPanel.RenderWireframe;
            set => ModelPanel.RenderWireframe = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderShaders
        {
            get => ModelPanel.RenderShaders;
            set => ModelPanel.RenderShaders = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderModelBox
        {
            get => ModelPanel.RenderModelBox;
            set => ModelPanel.RenderModelBox = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderObjectBox
        {
            get => ModelPanel.RenderObjectBox;
            set => ModelPanel.RenderObjectBox = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderVisBoneBox
        {
            get => ModelPanel.RenderVisBoneBox;
            set => ModelPanel.RenderVisBoneBox = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UseBindStateBoxes
        {
            get => ModelPanel.UseBindStateBoxes;
            set => ModelPanel.UseBindStateBoxes = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DontRenderOffscreen
        {
            get => ModelPanel.DontRenderOffscreen;
            set => ModelPanel.DontRenderOffscreen = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ScaleBones
        {
            get => ModelPanel.ScaleBones;
            set => ModelPanel.ScaleBones = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderLightDisplay
        {
            get => _renderLightDisplay;
            set
            {
                _renderLightDisplay = value;
                ModelPanel.Invalidate();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MaxFrame
        {
            get => _maxFrame;
            set => _maxFrame = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Updating
        {
            get => _updating;
            set => _updating = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Loop
        {
            get => _loop;
            set => _loop = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Playing
        {
            get => _playing;
            set => _playing = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual CHR0Editor CHR0Editor => null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual SRT0Editor SRT0Editor => null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual SHP0Editor SHP0Editor => null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual VIS0Editor VIS0Editor => null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual PAT0Editor PAT0Editor => null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual SCN0Editor SCN0Editor => null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual CLR0Editor CLR0Editor => null;

        //TODO: make all playback panel values individual and virtual and inherit them with the playback panel values
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ModelPlaybackPanel PlaybackPanel => null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ModelPanel ModelPanel => _viewerForm == null ? null : _viewerForm.modelPanel1;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ModelViewerForm ModelViewerForm => _viewerForm;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get => _targetModel;
            set => ModelChanged(value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CHR0Node SelectedCHR0
        {
            get => _chr0;
            set
            {
                _chr0 = value;

                if (!_updating)
                {
                    AnimChanged(NW4RAnimType.CHR);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SRT0Node SelectedSRT0
        {
            get => _srt0;
            set
            {
                _srt0 = value;

                if (!_updating)
                {
                    AnimChanged(NW4RAnimType.SRT);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SHP0Node SelectedSHP0
        {
            get => _shp0;
            set
            {
                _shp0 = value;

                if (!_updating)
                {
                    AnimChanged(NW4RAnimType.SHP);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PAT0Node SelectedPAT0
        {
            get => _pat0;
            set
            {
                _pat0 = value;

                if (!_updating)
                {
                    AnimChanged(NW4RAnimType.PAT);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public VIS0Node SelectedVIS0
        {
            get => _vis0;
            set
            {
                _vis0 = value;

                if (_updating)
                {
                    return;
                }

                if (!_updating)
                {
                    AnimChanged(NW4RAnimType.VIS);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SCN0Node SelectedSCN0
        {
            get => _scn0;
            set
            {
                _scn0 = value;

                if (!_updating)
                {
                    AnimChanged(NW4RAnimType.SCN);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CLR0Node SelectedCLR0
        {
            get => _clr0;
            set
            {
                _clr0 = value;

                if (!_updating)
                {
                    AnimChanged(NW4RAnimType.CLR);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AllowZoomExtents => _selectedBone != null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool EnableTransformEdit
        {
            get => _enableTransform;
            set
            {
                if (_enableTransform == value)
                {
                    return;
                }

                _enableTransform = value;

                if (CHR0Editor != null)
                {
                    CHR0Editor.Enabled = value;
                }

                if (SRT0Editor != null)
                {
                    SRT0Editor.Enabled = value;
                }

                if (SHP0Editor != null)
                {
                    SHP0Editor.Enabled = value;
                }

                if (VIS0Editor != null)
                {
                    VIS0Editor.Enabled = value;
                }

                if (PAT0Editor != null)
                {
                    PAT0Editor.Enabled = value;
                }

                if (SCN0Editor != null)
                {
                    SCN0Editor.Enabled = value;
                }

                if (CLR0Editor != null)
                {
                    CLR0Editor.Enabled = value;
                }

                if (KeyframePanel != null)
                {
                    KeyframePanel.Enabled = value;
                }

                if (InterpolationEditor != null && InterpolationEditor.Visible)
                {
                    InterpolationEditor.Enabled = value;
                }

                if (value)
                {
                    UpdatePropDisplay();
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint AllowedUndos
        {
            get => _allowedUndos;
            set => _allowedUndos = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public InterpolationEditor InterpolationEditor => _interpolationEditor.Visible ? _interpolationEditor :
            _interpolationForm != null ? _interpolationForm._interpolationEditor : null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0MaterialRefNode TargetTexRef
        {
            get => _targetTexRef;
            set
            {
                _targetTexRef = value;
                UpdatePropDisplay();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public VIS0EntryNode TargetVisEntry
        {
            get => _targetVisEntry;
            set
            {
                _targetVisEntry = value;
                UpdatePropDisplay();

                if (KeyframePanel != null)
                {
                    KeyframePanel.TargetSequence = _targetVisEntry;
                    KeyframePanel.chkConstant.Checked = _targetVisEntry._flags.HasFlag(VIS0Flags.Constant);
                    KeyframePanel.chkEnabled.Checked = _targetVisEntry._flags.HasFlag(VIS0Flags.Enabled);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual NW4RAnimType TargetAnimType
        {
            get => _targetAnimType;
            set
            {
                if (_targetAnimType == value)
                {
                    return;
                }

                _targetAnimType = value;
            }
        }

        public static float CamDistance(Vector3 v, GLViewport viewport, float radius = _orbRadius)
        {
            return CamDistance(v, viewport.Camera, radius);
        }

        public static float CamDistance(Vector3 v, GLCamera cam, float radius = _orbRadius)
        {
            if (!cam.Orthographic)
            {
                return v.TrueDistance(cam.GetPoint()) / radius * (cam.VerticalFieldOfView / 45.0f) * 0.1f;
            }

            return cam._scale._x * 80.0f;
        }

        public static float OrbRadius(IBoneNode b, GLViewport viewport, float radius = _orbRadius)
        {
            return CamDistance(BoneLoc(b), viewport.Camera, radius);
        }

        public static float OrbRadius(Vector3 b, GLViewport viewport, float radius = _orbRadius)
        {
            return CamDistance(b, viewport.Camera, radius);
        }

        public static float OrbRadius(IBoneNode b, GLCamera cam, float radius = _orbRadius)
        {
            return CamDistance(BoneLoc(b), cam, radius);
        }

        public static float OrbRadius(Vector3 b, GLCamera cam, float radius = _orbRadius)
        {
            return CamDistance(b, cam, radius);
        }
        //public float VertexOrbRadius(GLViewport viewport, float radius = _orbRadius) { if (VertexLoc().HasValue) return CamDistance(VertexLoc().Value, viewport.Camera, radius); else return 0; }
        //public float VertexOrbRadius(GLCamera cam, float radius = _orbRadius) { if (VertexLoc().HasValue) return CamDistance(VertexLoc().Value, cam, radius); else return 0; }

        public static Matrix CameraFacingRotationMatrix(GLViewport viewport, Vector3 pos = new Vector3())
        {
            return CameraFacingRotationMatrix(viewport.Camera, pos);
        }

        public static Matrix CameraFacingRotationMatrix(GLCamera camera, Vector3 pos = new Vector3())
        {
            return Matrix.RotationMatrix(CameraFacingRotation(camera, pos));
        }

        public static Vector3 CameraFacingRotation(GLViewport viewport, Vector3 pos = new Vector3())
        {
            return CameraFacingRotation(viewport.Camera, pos);
        }

        public static Vector3 CameraFacingRotation(GLCamera camera, Vector3 pos = new Vector3())
        {
            return camera.Orthographic ? camera._rotation : pos.LookatAngles(CamLoc(camera)) * Maths._rad2degf;
        }

        public static Vector3 CamLoc(GLCamera cam)
        {
            return cam == null ? new Vector3() : cam.GetPoint();
        }

        public static Vector3 BoneLoc(IBoneNode b)
        {
            return b == null ? new Vector3() : b.Matrix.GetPoint();
        }

        private readonly bool _moveVerticesWithVertexLoc = false;

        public Vector3? VertexLoc
        {
            get
            {
                if (_selectedVertices == null || _selectedVertices.Count == 0)
                {
                    return null;
                }

                if (_vertexLoc != null)
                {
                    return _vertexLoc;
                }

                Vector3 average = new Vector3();
                foreach (Vertex3 v in _selectedVertices)
                {
                    average += v.WeightedPosition;
                }

                average /= _selectedVertices.Count;
                return _vertexLoc = average;
            }
            set
            {
                if (VertexLoc == null)
                {
                    return;
                }

                if (value == null)
                {
                    return;
                }

                if (_moveVerticesWithVertexLoc)
                {
                    Vector3 previous = VertexLoc.Value;
                    Vector3 diff = value.Value - previous;
                    foreach (Vertex3 v in _selectedVertices)
                    {
                        v.WeightedPosition += diff;
                    }
                }

                _vertexLoc = value;
            }
        }

        #region Overridable

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(true)]
        public virtual bool EditingAll
        {
            get;
            //{
            //    return (!(models.SelectedItem is IRenderedObject) && 
            //        models.SelectedItem != null && 
            //        models.SelectedItem.ToString() == "All");
            //}
            set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual KeyframePanel KeyframePanel => null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual BonesPanel BonesPanel => null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IBoneNode SelectedBone
        {
            get => _selectedBone;
            set
            {
                if (_selectedBone != null)
                {
                    _selectedBone.BoneColor = _selectedBone.NodeColor = Color.Transparent;
                }

                bool boneSelected = (_selectedBone = value) != null;
                if (boneSelected)
                {
                    _selectedBone.BoneColor = Color.FromArgb(0, 128, 255);
                    _selectedBone.NodeColor = Color.FromArgb(255, 128, 0);
                }

                //Check if the user selected a bone from another model.
                if (EditingAll && boneSelected && TargetModel != _selectedBone.IModel)
                {
                    _resetCamera = false;
                    TargetModel = _selectedBone.IModel;
                }

                BonesPanel?.SetSelectedBone(_selectedBone);

                if (TargetAnimType == NW4RAnimType.CHR && KeyframePanel != null)
                {
                    KeyframePanel.TargetSequence =
                        _chr0 != null && _selectedBone != null ? _chr0.FindChild(_selectedBone.Name, false) : null;
                }

                OnSelectedBoneChanged();
                UpdatePropDisplay();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int CurrentFrame
        {
            get => _animFrame;
            set
            {
                _animFrame = value;
                UpdateModel();

                //The more frames there are in the animation, the more the viewer lags
                //if (InterpolationEditor != null)
                //    InterpolationEditor.Frame = CurrentFrame;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(true)]
        public virtual bool RetrieveCorrespondingAnimations { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(false)]
        public virtual bool SyncVIS0 { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(false)]
        public virtual bool DisableBonesWhenPlaying { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(false)]
        public virtual bool DoNotHighlightOnMouseMove { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string ScreenCaptureFolder { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(ImageType.png)]
        public virtual ImageType ScreenCaptureType { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(false)]
        public virtual bool InterpolationFormOpen { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(TransformType.Rotation)]
        public virtual TransformType ControlType { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(true)]
        public virtual bool PlayCHR0 { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(true)]
        public virtual bool PlaySRT0 { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(true)]
        public virtual bool PlayPAT0 { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(true)]
        public virtual bool PlayVIS0 { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(true)]
        public virtual bool PlayCLR0 { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(true)]
        public virtual bool PlaySCN0 { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(true)]
        public virtual bool PlaySHP0 { get; set; }

        #endregion
    }
}