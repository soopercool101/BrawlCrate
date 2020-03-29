using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Point = System.Windows.Point;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase
{
    public partial class ModelEditorBase : UserControl
    {
        public const float _orbRadius = 1.0f;
        public const float _circRadius = 1.2f;
        public const float _axisSnapRange = 7.0f;
        public const float _selectRange = 0.03f;     //Selection error range for orb and circ
        public const float _axisSelectRange = 0.15f; //Selection error range for axes
        public const float _selectOrbScale = _selectRange / _orbRadius;
        public const float _circOrbScale = _circRadius / _orbRadius;

        public int _animFrame = 0, _maxFrame;
        public bool _updating, _loop;
        public CoolTimer _timer;

        public CHR0Node _chr0;
        public SRT0Node _srt0;
        public SHP0Node _shp0;
        public PAT0Node _pat0;
        public VIS0Node _vis0;
        public CLR0Node _clr0;
        public SCN0Node _scn0;

        public SCN0LightSetNode _SCN0LightSet;
        public SCN0FogNode _SCN0Fog;
        public SCN0CameraNode _SCN0Camera;
        public Point _lightStartPoint, _lightEndPoint, _cameraStartPoint, _cameraEndPoint;
        public bool _lightEndSelected = false, _lightStartSelected = false;

        public MDL0MaterialRefNode _targetTexRef = null;
        public VIS0EntryNode _targetVisEntry;
        public List<IModel> _targetModels = new List<IModel>();
        public IModel _targetModel;
        public IBoneNode _selectedBone = null;
        public List<Vertex3> _selectedVertices = new List<Vertex3>();
        public List<HotKeyInfo> _hotkeyList;
        public List<Image> _images = new List<Image>();

        public BindingList<ResourceNode> _openedFiles = new BindingList<ResourceNode>();

        //Bone Name - Attached Polygon Indices
        public Dictionary<string, Dictionary<int, List<int>>> VIS0Indices => (_targetModel as MDL0Node)?.VIS0Indices;

        protected NW4RAnimType _targetAnimType;

        protected Vector3? _vertexLoc = null;

        public class SelectionParams
        {
            public bool _rotating, _translating, _scaling;
            public bool _snapX, _snapY, _snapZ, _snapCirc;
            public bool _hiX, _hiY, _hiZ, _hiCirc, _hiSphere;
            public Vector3 _lastPointLocal, _lastPointWorld;
            public Vector3 _oldAngles, _oldPosition, _oldScale;

            public void ResetSnaps()
            {
                _snapX = _snapY = _snapZ = _snapCirc = false;
            }

            public void ResetActions()
            {
                _rotating = _translating = _scaling = false;
            }

            public void ResetHighlights()
            {
                _hiX = _hiY = _hiZ = _hiCirc = _hiSphere = false;
            }

            public void ResetAll()
            {
                ResetActions();
                ResetSnaps();
                ResetHighlights();
            }

            public bool IsMoving()
            {
                return _rotating || _translating || _scaling;
            }

            public bool SnappingAny()
            {
                return _snapX || _snapY || _snapZ || _snapCirc;
            }

            public bool HighlightingAny()
            {
                return _hiX || _hiY || _hiZ || _hiCirc || _hiSphere;
            }

            public void Update(
                TransformType type,
                Vector3 worldPoint,
                Vector3 localPoint,
                Vector3 oldAngles,
                Vector3 oldPosition,
                Vector3 oldScale)
            {
                _lastPointLocal = localPoint;
                _lastPointWorld = worldPoint;
                _rotating = type == TransformType.Rotation;
                _translating = type == TransformType.Translation;
                _scaling = type == TransformType.Scale;
                _oldAngles = oldAngles;
                _oldPosition = oldPosition;
                _oldScale = oldScale;
            }

            public void Update(
                TransformType type,
                Vector3 worldPoint,
                Vector3 localPoint,
                FrameState oldFrameState)
            {
                Update(type, worldPoint, localPoint, oldFrameState._rotate, oldFrameState._translate,
                    oldFrameState._scale);
            }

            public void ApplySnaps()
            {
                _snapX = _hiX;
                _snapY = _hiY;
                _snapZ = _hiZ;
                _snapCirc = _hiCirc;
            }
        }

        public SelectionParams
            _boneSelection = new SelectionParams(),
            _vertexSelection = new SelectionParams();

        public bool _resetCamera = true;
        protected bool _enableTransform = true;
        protected bool _playing = false;
        protected bool _bonesWereOff = false;
        protected bool _renderLightDisplay = false;
        public bool _capture = false;

        public static Color _floorHue = Color.FromArgb(255, 128, 128, 191);

        public static BindingList<NW4RAnimType> _editableAnimTypes = new BindingList<NW4RAnimType>
        {
            NW4RAnimType.CHR,
            NW4RAnimType.SRT,
            NW4RAnimType.SHP,
            NW4RAnimType.PAT,
            NW4RAnimType.VIS,
            NW4RAnimType.CLR,
            NW4RAnimType.SCN
        };

        public ModelViewerForm _viewerForm = null;
        public InterpolationEditor _interpolationEditor;
        public InterpolationForm _interpolationForm = null;
        public Control _currentControl = null;
        public OpenFileDialog _dlgOpen = new OpenFileDialog();

        #region Events

        public event GLRenderEventHandler EventPostRender, EventPreRender;
        public event MouseEventHandler EventMouseDown, EventMouseMove, EventMouseUp;
        public event EventHandler TargetModelChanged, ModelViewerChanged;

        #endregion

        #region Delegates

        protected delegate void DelegateOpenFile(string s);

        protected DelegateOpenFile _openFileDelegate;

        #endregion

        public static readonly Type[] AnimTypeList = new Type[]
        {
            typeof(CHR0Node),
            typeof(SRT0Node),
            typeof(SHP0Node),
            typeof(PAT0Node),
            typeof(VIS0Node),
            typeof(CLR0Node),
            typeof(SCN0Node)
        };
    }

    public enum NW4RAnimType : int
    {
        None = -1,
        CHR = 0,
        SRT = 1,
        SHP = 2,
        PAT = 3,
        VIS = 4,
        CLR = 5,
        SCN = 6
    }

    public enum J3DAnimType : int
    {
        None = -1,
        BCK = 0
    }

    public enum TransformType
    {
        Translation = 0,
        Rotation = 1,
        Scale = 2,
        None = 3
    }
}