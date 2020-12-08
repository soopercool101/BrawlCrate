using BrawlLib.OpenGL;
using BrawlLib.Wii.Animations;
using OpenTK.Graphics.OpenGL;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public partial class InterpolationViewer : GLPanel
    {
        public InterpolationViewer()
        {
            InitializeComponent();
            CurrentViewport.ViewType = ViewportProjection.Front;
            CurrentViewport.Camera._orthoDimensions = new Vector4(0, Width, Height, 0);
            CurrentViewport.Camera.CalculateProjection();
        }

        #region Variables

        public event EventHandler SelectedKeyframeChanged, FrameChanged, SignalChange;

        internal bool _updating = false;

        private KeyframeEntry _selKey; //The currently selected keyframe
        private KeyframeEntry _hiKey;  //The keyframe the mouse is hovering over

        private Vector2? _slopePoint;
        private Vector2 _origPos;
        private KeyframeEntry _keyRoot; //The first keyframe in the array

        private const float _lineWidth = 1.5f;  //The size of lines
        private const float _pointWidth = 5.0f; //The size of points

        private int _frame;      //The current frame index
        private int _frameLimit; //The overall number of frames

        private float _xScale;                  //Width/Frames ratio
        private float _yScale;                  //Height/Values ratio
        private float _prevX;                   //Previous mouse point X
        private float _prevY;                   //Previous mouse point Y
        private float _tanLen = 5.0f;           //The length of tangent values
        private float _precision = 4.0f;        //The step value for lines drawn to represent interpolation
        private float _minVal = float.MaxValue; //The minimum value in all keyframes
        private float _maxVal = float.MinValue; //The maximum value in all keyframes

        private bool
            _keyDraggingAllowed; //Determines if the user can drag keyframe values across frames or to change the value

        private bool _drawTans = true; //Determines if tangents should be rendered
        private bool _syncStartEnd;    //If true, the first and last keyframe values and tangents will be synchronized

        private bool
            _allKeys = true; //Determines if all keyframes should be rendered, or only the selected one and its neighbors

        private bool
            _genTans; //True if tangents should be automatically generated when affected keyframes change

        private bool _lockIncs; //Determines if the scales can be changed on resize
        private bool _dragging; //True if the user is dragging a keyframe or tangent slope

        private static bool _alterSelTangent_Drag = true;
        private static bool _alterAdjTangents_KeyFrame_Drag = true;

        #endregion

        #region Properties

        public bool DisplayAllKeyframes
        {
            get => _allKeys;
            set
            {
                _allKeys = value;
                Invalidate();
            }
        }

        public float TangentLength
        {
            get => _tanLen;
            set
            {
                _tanLen = value;
                Invalidate();
            }
        }

        public bool GenerateTangents
        {
            get => _genTans;
            set => _genTans = value;
        }

        public bool KeyDraggingAllowed
        {
            get => _keyDraggingAllowed;
            set => _keyDraggingAllowed = value;
        }

        public bool DrawTangents
        {
            get => _drawTans;
            set
            {
                _drawTans = value;
                Invalidate();
            }
        }

        public bool SyncStartEnd
        {
            get => _syncStartEnd;
            set => _syncStartEnd = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public KeyframeEntry SelectedKeyframe
        {
            get => _selKey;
            set
            {
                _selKey = value;

                if (SelectedKeyframeChanged != null && !_updating)
                {
                    SelectedKeyframeChanged(this, null);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public KeyframeEntry HighlightedKeyframe => _hiKey;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public float Precision
        {
            get => _precision;
            set
            {
                _precision = value;
                Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public KeyframeEntry KeyRoot
        {
            get => _keyRoot;
            set
            {
                _keyRoot = value;
                Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int FrameIndex
        {
            get => _frame;
            set
            {
                _frame = value.Clamp(0, _frameLimit);

                Invalidate();

                if (!_updating)
                {
                    FrameChanged?.Invoke(this, null);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int FrameLimit
        {
            get => _frameLimit;
            set => _frameLimit = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        private int DisplayedFrameLimit
        {
            get
            {
                if (_allKeys)
                {
                    return _frameLimit - 1;
                }

                if (_selKey != null)
                {
                    return GetKeyframeMaxIndex() - GetKeyframeMinIndex();
                }

                return 0;
            }
        }

        public bool AlterSelectedTangent_OnDrag
        {
            get => _alterSelTangent_Drag;
            set
            {
                _alterSelTangent_Drag = value;
                if (!_updating)
                {
                    Invalidate();
                }
            }
        }

        public bool AlterAdjTangent_OnSelectedDrag
        {
            get => _alterAdjTangents_KeyFrame_Drag;
            set
            {
                _alterAdjTangents_KeyFrame_Drag = value;
                if (!_updating)
                {
                    Invalidate();
                }
            }
        }

        #endregion

        #region Functions

        private bool Has3PlusVals()
        {
            //Just don't resize while dragging at all
            return false;

            //HashSet<float> v = new HashSet<float>();
            //if (DisplayAllKeyframes)
            //{
            //    for (KeyframeEntry entry = _keyRoot._next; (entry != _keyRoot); entry = entry._next)
            //    {
            //        v.Add(entry._value);
            //        if (v.Count > 3)
            //            return true;
            //    }
            //}
            //else
            //{
            //    v.Add(SelectedKeyframe._prev._value);
            //    v.Add(SelectedKeyframe._value);
            //    v.Add(SelectedKeyframe._next._value);
            //    if (v.Count > 3)
            //        return true;
            //}
            //return false;
        }

        private int GetKeyframeMaxIndex()
        {
            if (!DisplayAllKeyframes)
            {
                if (_selKey != null)
                {
                    if (_selKey._next._index < 0)
                    {
                        return _selKey._index;
                    }

                    return _selKey._next._index;
                }
            }
            else
            {
                return _frameLimit - 1;
            }

            return 0;
        }

        private int GetKeyframeMinIndex()
        {
            if (!DisplayAllKeyframes)
            {
                if (_selKey != null)
                {
                    if (_selKey._prev._index < 0)
                    {
                        return _selKey._index;
                    }

                    return _selKey._prev._index;
                }
            }
            else
            {
                return 0;
            }

            return 0;
        }

        /// <summary>
        /// Returns true if two has a value
        /// </summary>
        /// <param name="index">Float value of current frame. Not normalized [0,1]</param>
        /// <returns></returns>
        private bool GetFrameValue(float index, out float one, out float two)
        {
            KeyframeEntry entry, root = _keyRoot;

            one = 0;
            two = 0;

            if (_keyRoot == null)
            {
                return false;
            }

            if (index > root._prev._index)
            {
                float span = FrameLimit - _keyRoot._prev._index.Clamp(0, FrameLimit) +
                             _keyRoot._next._index.Clamp(0, FrameLimit);
                float offset =
                    index > _keyRoot._prev._index && index < FrameLimit
                        ? index - _keyRoot._prev._index
                        : FrameLimit - _keyRoot._prev._index + index;

                one = _keyRoot._prev.Interpolate(offset, span, _keyRoot._next);
                return false;
            }

            if (index < root._next._index)
            {
                float span = FrameLimit - _keyRoot._prev._index.Clamp(0, FrameLimit) +
                             _keyRoot._next._index.Clamp(0, FrameLimit);
                float offset =
                    index > _keyRoot._prev._index.Clamp(0, FrameLimit) && index < FrameLimit
                        ? index - _keyRoot._prev._index.Clamp(0, FrameLimit)
                        : FrameLimit - _keyRoot._prev._index.Clamp(0, FrameLimit) + index;

                one = _keyRoot._prev.Interpolate(offset, span, _keyRoot._next);
                return false;
            }

            for (entry = root._next; entry != root && entry._index <= index; entry = entry._next)
            {
                if (entry._index == index)
                {
                    if (entry._next._index == entry._index)
                    {
                        one = entry._value;
                        two = entry._next._value;
                        return true;
                    }

                    one = entry._value;
                    return false;
                }
            }

            one = entry._prev.Interpolate(index - entry._prev._index, false);
            return false;
        }

        public void FindMaxMin()
        {
            if (!DisplayAllKeyframes && SelectedKeyframe == null)
            {
                return;
            }

            _minVal = float.MaxValue;
            _maxVal = float.MinValue;

            int start = GetKeyframeMinIndex();
            int end = GetKeyframeMaxIndex();

            for (float i = start; i <= end; i += 1 / _precision)
            {
                if (i >= 0 && i < _frameLimit)
                {
                    bool has2nd = GetFrameValue(i, out float one, out float two);
                    float v = one;

                    if (v < _minVal)
                    {
                        _minVal = v;
                    }

                    if (v > _maxVal)
                    {
                        _maxVal = v;
                    }

                    if (has2nd)
                    {
                        v = two;
                        if (v < _minVal)
                        {
                            _minVal = v;
                        }

                        if (v > _maxVal)
                        {
                            _maxVal = v;
                        }
                    }
                }
            }
        }

        private void CalcXY()
        {
            if (_lockIncs)
            {
                return;
            }

            int i = DisplayedFrameLimit;
            if (i == 0)
            {
                return;
            }

            //Calculate X Scale
            _xScale = Width / (float) i;

            FindMaxMin();

            //Calculate Y Scale
            _yScale = Height / (_maxVal - _minVal);
        }

        #endregion

        #region Mouse, Resize

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_keyRoot == null)
            {
                return;
            }

            float mouseY = (float) Height - e.Y;
            float mouseX = e.X;

            //Get nearest frame value
            int frameVal = (int) (mouseX / _xScale + 0.5f);

            int min = GetKeyframeMinIndex();
            if (!_dragging)
            {
                if (_hiKey != null)
                {
                    _hiKey = null;
                    Invalidate();
                }

                Cursor = Cursors.Default;

                if (DisplayAllKeyframes)
                {
                    for (KeyframeEntry entry = _keyRoot._next; entry != _keyRoot; entry = entry._next)
                    {
                        float frame = entry._index;
                        if (Math.Abs(mouseX - frame * _xScale) <= _pointWidth)
                        {
                            if (Math.Abs(mouseY - (entry._value - _minVal) * _yScale) <= _pointWidth)
                            {
                                _hiKey = entry;
                                Cursor = Cursors.Hand;
                                Invalidate();
                                return;
                            }
                        }
                    }
                }

                if ( /*_drawTans && */_selKey != null)
                {
                    if (!DisplayAllKeyframes)
                    {
                        if (SelectedKeyframe._prev._index != -1)
                        {
                            float frame = SelectedKeyframe._prev._index;
                            if (Math.Abs(mouseX - (frame - min) * _xScale) <= _pointWidth)
                            {
                                if (Math.Abs(mouseY - (SelectedKeyframe._prev._value - _minVal) * _yScale) <=
                                    _pointWidth)
                                {
                                    _hiKey = SelectedKeyframe._prev;
                                    Cursor = Cursors.Hand;
                                    Invalidate();
                                    return;
                                }
                            }
                        }

                        float frame1 = SelectedKeyframe._index;
                        if (Math.Abs(mouseX - (frame1 - min) * _xScale) <= _pointWidth)
                        {
                            if (Math.Abs(mouseY - (SelectedKeyframe._value - _minVal) * _yScale) <= _pointWidth)
                            {
                                _hiKey = SelectedKeyframe;
                                Cursor = Cursors.Hand;
                                Invalidate();
                                return;
                            }
                        }

                        if (SelectedKeyframe._next._index != -1)
                        {
                            float frame = SelectedKeyframe._next._index;
                            if (Math.Abs(mouseX - (frame - min) * _xScale) <= _pointWidth)
                            {
                                if (Math.Abs(mouseY - (SelectedKeyframe._next._value - _minVal) * _yScale) <=
                                    _pointWidth)
                                {
                                    _hiKey = SelectedKeyframe._next;
                                    Cursor = Cursors.Hand;
                                    Invalidate();
                                    return;
                                }
                            }
                        }
                    }

                    float i1 = -(_tanLen / 2);
                    float i2 = -i1;

                    int xVal2 = _selKey._index;
                    float yVal = _selKey._value;
                    float tan = _selKey._tangent;

                    float p = (float) Math.Sqrt(_precision / 4.0f);
                    Vector2 one = new Vector2((xVal2 + i1 * p - min) * _xScale,
                        (yVal - _minVal + tan * i1 * p) * _yScale);
                    Vector2 two = new Vector2((xVal2 + i2 * p - min) * _xScale,
                        (yVal - _minVal + tan * i2 * p) * _yScale);

                    _slopePoint = null;
                    if (Math.Abs(mouseX - one._x) <= _pointWidth && Math.Abs(mouseY - one._y) <= _pointWidth)
                    {
                        _slopePoint = new Vector2(mouseX, mouseY);
                        _origPos = new Vector2((xVal2 - min) * _xScale, (yVal - _minVal) * _yScale);
                        _hiKey = _selKey;
                        Cursor = Cursors.Hand;
                        Invalidate();
                        return;
                    }

                    if (Math.Abs(mouseX - two._x) <= _pointWidth)
                    {
                        if (Math.Abs(mouseY - two._y) <= _pointWidth)
                        {
                            _slopePoint = new Vector2(mouseX, mouseY);
                            _origPos = new Vector2((xVal2 - min) * _xScale, (yVal - _minVal) * _yScale);
                            _hiKey = _selKey;
                            Cursor = Cursors.Hand;
                            Invalidate();
                            return;
                        }
                    }
                }

                if (DisplayAllKeyframes)
                {
                    if (Math.Abs(mouseX - _frame * _xScale) <= _pointWidth)
                    {
                        Cursor = Cursors.VSplit;
                    }
                }
            }
            else if (_selKey != null && (_keyDraggingAllowed || _slopePoint != null))
            {
                if (_slopePoint != null)
                {
                    int xVal2 = _selKey._index;
                    float yVal = _selKey._value;

                    float xDiff = mouseX - ((Vector2) _slopePoint)._x;
                    float yDiff = mouseY - ((Vector2) _slopePoint)._y;

                    float x2 = ((Vector2) _slopePoint)._x + xDiff;
                    float y2 = ((Vector2) _slopePoint)._y + yDiff;

                    _slopePoint = new Vector2(x2 == _origPos._x ? ((Vector2) _slopePoint)._x : x2, y2);

                    Vector2 x = (Vector2) _slopePoint - _origPos;
                    _selKey._tangent = (float) Math.Round(x._y / _yScale / (x._x / _xScale), 5);

                    if (_genTans)
                    {
                        _selKey._prev.GenerateTangent();
                        _selKey._next.GenerateTangent();
                    }

                    if (_syncStartEnd)
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

                    Invalidate();

                    SelectedKeyframeChanged?.Invoke(this, null);

                    SignalChange?.Invoke(this, null);
                }
                else if (_keyDraggingAllowed)
                {
                    float yVal = mouseY / _yScale + _minVal;
                    int xv = frameVal;
                    int xDiff = xv - (int) (_prevX + 0.5f);
                    float yDiff = yVal - _prevY;

                    int newFrameIndex = _selKey._index + xDiff;
                    bool frameSet =
                        newFrameIndex >= 0 &&
                        newFrameIndex < FrameLimit &&
                        (newFrameIndex >= _selKey._prev._index || _selKey._prev._index < 0) &&
                        (newFrameIndex <= _selKey._next._index || _selKey._next._index < 0);

                    if (newFrameIndex == _selKey._prev._index)
                    {
                        if (_selKey._prev._prev._index == _selKey._prev._index)
                        {
                            frameSet = false;
                        }
                    }
                    else if (newFrameIndex == _selKey._next._index)
                    {
                        if (_selKey._next._next._index == _selKey._next._index)
                        {
                            frameSet = false;
                        }
                    }

                    _selKey._value = (float) Math.Round(_selKey._value + yDiff, 3);

                    if (frameSet)
                    {
                        FrameIndex = _selKey._index = newFrameIndex;
                    }

                    _prevX = xv;
                    _prevY = yVal;

                    if (_genTans)
                    {
                        if (_alterSelTangent_Drag)
                        {
                            _selKey.GenerateTangent();
                        }

                        if (_alterAdjTangents_KeyFrame_Drag)
                        {
                            _selKey._prev.GenerateTangent();
                            _selKey._next.GenerateTangent();
                        }
                    }

                    if (_syncStartEnd)
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

                    Invalidate();

                    SelectedKeyframeChanged?.Invoke(this, null);

                    SignalChange?.Invoke(this, null);
                }
            }
            else if (frameVal >= 0 && frameVal < _frameLimit && _selKey == null)
            {
                FrameIndex = frameVal;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            bool keyChanged = _selKey != _hiKey;
            _selKey = _hiKey;

            if (DisplayAllKeyframes)
            {
                _dragging = _selKey != null || Cursor == Cursors.VSplit || _slopePoint != null;
            }
            else
            {
                _dragging = _selKey != null && (_slopePoint != null || Cursor == Cursors.Hand);
            }

            if (_selKey != null)
            {
                if (_slopePoint == null)
                {
                    int min = GetKeyframeMinIndex();
                    _prevX = _selKey._index - min;
                    _prevY = _selKey._value;
                }

                _frame = _selKey._index;

                if (_dragging && !Has3PlusVals() || _slopePoint != null)
                {
                    _lockIncs = true;
                }
            }
            else if (!_dragging && ModifierKeys == Keys.Control)
            {
                int frameVal = (int) (e.X / _xScale + 0.5f);
                KeyframeEntry entry;
                for (entry = _keyRoot._next;
                    entry != _keyRoot && entry._index >= 0 && entry._index <= FrameLimit;
                    entry = entry._next)
                {
                    if (entry._index < frameVal)
                    {
                        if (entry.Second != null)
                        {
                            entry = entry.Second;
                        }

                        if (entry._next._index > frameVal || entry._next == _keyRoot)
                        {
                            break;
                        }
                    }
                }

                if (entry != _keyRoot)
                {
                    int min = GetKeyframeMinIndex();
                    KeyframeEntry r = new KeyframeEntry(frameVal + min, ((float) Height - e.Y) / _yScale + _minVal);
                    entry.InsertAfter(r);
                    r.GenerateTangent();
                    _selKey = r;
                    _dragging = true;
                    _prevX = _selKey._index - min;
                    _prevY = _selKey._value;
                    _frame = _selKey._index;
                    Invalidate();
                }

                SignalChange?.Invoke(this, null);
            }

            if (keyChanged)
            {
                Invalidate();
                SelectedKeyframeChanged?.Invoke(this, null);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_slopePoint != null || _dragging && _lockIncs)
            {
                Invalidate();
            }

            _dragging = false;
            _lockIncs = false;
            _slopePoint = null;
        }

        protected override void OnResize(EventArgs e)
        {
            _ctx?.Update();

            _precision = Width / 100.0f;

            BeginUpdate();
            CurrentViewport.Region = new Rectangle(0, 0, Width, Height);
            CurrentViewport.Camera._orthoDimensions = new Vector4(0, Width, Height, 0);
            CurrentViewport.Camera.CalculateProjection();
            EndUpdate();
        }

        #endregion

        #region Rendering

        internal override void OnInit(TKContext ctx)
        {
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.PointSmooth);
            GL.LineWidth(_lineWidth);
            GL.PointSize(_pointWidth);
        }

        private void DrawTangent(KeyframeEntry e, float xMin)
        {
            int xVal = e._index;
            float yVal = e._value;
            float tan = e._tangent;

            float i1 = -(_tanLen / 2);
            float i2 = _tanLen / 2;

            bool ignoreIn = false, ignoreOut = false;
            if (e.Second != null)
            {
                if (e._next._index == e._index)
                {
                    ignoreOut = true;
                    i2 = 0.0f;
                }
                else
                {
                    ignoreIn = true;
                    i1 = 0.0f;
                }
            }

            float p = (float) Math.Sqrt(_precision / 4.0f);
            Vector2 one = new Vector2((xVal + i1 * p - xMin) * _xScale, (yVal - _minVal + tan * i1 * p) * _yScale);
            Vector2 two = new Vector2((xVal + i2 * p - xMin) * _xScale, (yVal - _minVal + tan * i2 * p) * _yScale);

            if (e == _selKey)
            {
                GL.Color4(Color.Purple);
                GL.Begin(BeginMode.Points);

                if (!ignoreIn)
                {
                    GL.Vertex2(one._x, one._y);
                }

                if (!ignoreOut)
                {
                    GL.Vertex2(two._x, two._y);
                }

                GL.End();
            }
            else
            {
                GL.Color4(Color.Green);

                float angle = (float) Math.Atan(tan * _yScale / _xScale) * Maths._rad2degf;

                if (!ignoreIn)
                {
                    GL.PushMatrix();
                    GL.Translate(one._x, one._y, 0.0f);
                    GL.Rotate(angle - 180.0f, 0, 0, 1);

                    GL.Begin(BeginMode.LineStrip);
                    GL.Vertex2(-7.0f, 3.5f);
                    GL.Vertex2(0.0f, 0.0f);
                    GL.Vertex2(-7.0f, -3.5f);
                    GL.End();

                    GL.PopMatrix();
                }

                if (!ignoreOut)
                {
                    GL.PushMatrix();
                    GL.Translate(two._x, two._y, 0.0f);
                    GL.Rotate(angle, 0, 0, 1);

                    GL.Begin(BeginMode.LineStrip);
                    GL.Vertex2(-7.0f, 3.5f);
                    GL.Vertex2(0.0f, 0.0f);
                    GL.Vertex2(-7.0f, -3.5f);
                    GL.End();

                    GL.PopMatrix();
                }
            }

            GL.Begin(BeginMode.LineStrip);
            GL.Vertex2(one._x, one._y);
            GL.Vertex2(two._x, two._y);
            GL.End();
        }

        protected override void OnRender(PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.White);

            if (_keyRoot == null)
            {
                return;
            }

            Rectangle region = CurrentViewport.Region;

            GL.Viewport(region);
            GL.Enable(EnableCap.ScissorTest);
            GL.Scissor(region.X, region.Y, region.Width, region.Height);

            CalcXY();

            CurrentViewport.Camera.LoadProjection();
            CurrentViewport.Camera.LoadModelView();

            float one, two;
            bool has2nd;

            if (_allKeys)
            {
                //Draw lines
                //GL.Color4(Color.Black);
                //GL.Begin(PrimitiveType.Lines);
                //for (KeyframeEntry entry = _keyRoot._next; (entry != _keyRoot); entry = entry._next)
                //{
                //    float xv = entry._index * xinc;
                //    GL.Vertex2(xv, 0.0f);
                //    GL.Vertex2(xv, Height);

                //    float yv = (GetFrameValue(entry._index) - _minVal) * yinc;
                //    GL.Vertex2(0.0f, yv);
                //    GL.Vertex2(Width, yv);
                //}
                //GL.End();

                //Draw tangents
                if (_drawTans)
                {
                    for (KeyframeEntry entry = _keyRoot._next; entry != _keyRoot; entry = entry._next)
                    {
                        DrawTangent(entry, 0);
                    }
                }
                else if (_selKey != null /* && !Linear*/)
                {
                    DrawTangent(_selKey, 0);
                }

                //Draw interpolation
                GL.Color4(Color.Red);
                GL.Begin(BeginMode.LineStrip);

                for (KeyframeEntry entry = _keyRoot._next; entry != _keyRoot; entry = entry._next)
                {
                    for (float i = entry._index;
                        i < (entry._next == _keyRoot ? _frameLimit : entry._next._index);
                        i += 1.0f / _precision)
                    {
                        has2nd = GetFrameValue(i, out one, out two);
                        GL.Vertex2(i * _xScale, (one - _minVal) * _yScale);
                        if (has2nd)
                        {
                            GL.End();
                            GL.Begin(BeginMode.LineStrip);
                            GL.Vertex2(i * _xScale, (two - _minVal) * _yScale);
                        }
                    }
                }

                GL.End();

                //Draw frame indicator
                GL.Color4(Color.Blue);
                if (_frame >= 0 && _frame < _frameLimit)
                {
                    GL.Begin(BeginMode.Lines);

                    float r = _frame * _xScale;
                    GL.Vertex2(r, 0.0f);
                    GL.Vertex2(r, Height);

                    GL.End();
                }

                //Draw points
                GL.Color4(Color.Black);
                GL.Begin(BeginMode.Points);
                for (KeyframeEntry entry = _keyRoot._next; entry != _keyRoot; entry = entry._next)
                {
                    bool t = false;
                    if (t = _hiKey == entry || _selKey == entry)
                    {
                        GL.PointSize(_pointWidth * 4.0f);
                        GL.Color4(Color.Orange);
                    }

                    has2nd = GetFrameValue(entry._index, out one, out two);
                    GL.Vertex2(entry._index * _xScale, (one - _minVal) * _yScale);
                    if (has2nd)
                    {
                        GL.Vertex2(entry._index * _xScale, (two - _minVal) * _yScale);
                    }

                    if (t)
                    {
                        GL.PointSize(_pointWidth);
                        GL.Color4(Color.Black);
                    }
                }

                GL.End();
            }
            else if (SelectedKeyframe != null)
            {
                //Draw lines
                GL.Color4(Color.Black);
                GL.Begin(BeginMode.Lines);

                int min = GetKeyframeMinIndex();
                int max = GetKeyframeMaxIndex();

                float xv = (SelectedKeyframe._index - min) * _xScale;
                GL.Vertex2(xv, 0.0f);
                GL.Vertex2(xv, Height);

                //float yv = (GetFrameValue(SelectedKeyframe._index) - _minVal) * _yScale;
                //GL.Vertex2(0.0f, yv);
                //GL.Vertex2(Width, yv);

                GL.End();

                //Draw interpolation
                GL.Color4(Color.Red);
                //GL.Begin(PrimitiveType.LineStrip);
                //for (float i = 0; i <= (float)(max - min); i += (1 / _precision))
                //    GL.Vertex2(i * _xScale, (GetFrameValue(i + min) - _minVal) * _yScale);
                //GL.End();

                //Draw tangent
                DrawTangent(SelectedKeyframe, min);
                if (_drawTans)
                {
                    if (SelectedKeyframe._prev._index != -1)
                    {
                        DrawTangent(SelectedKeyframe._prev, min);
                    }

                    if (SelectedKeyframe._next._index != -1)
                    {
                        DrawTangent(SelectedKeyframe._next, min);
                    }
                }

                //Draw points
                GL.Color4(Color.Black);
                GL.Begin(BeginMode.Points);

                if (SelectedKeyframe._prev._index != -1)
                {
                    has2nd = GetFrameValue(SelectedKeyframe._prev._index, out one, out two);
                    GL.Vertex2((SelectedKeyframe._prev._index - min) * _xScale, (one - _minVal) * _yScale);
                    if (has2nd)
                    {
                        GL.Vertex2((SelectedKeyframe._prev._index - min) * _xScale, (two - _minVal) * _yScale);
                    }
                }

                if (SelectedKeyframe._index != -1)
                {
                    has2nd = GetFrameValue(SelectedKeyframe._index, out one, out two);
                    GL.Vertex2((SelectedKeyframe._index - min) * _xScale, (one - _minVal) * _yScale);
                    if (has2nd)
                    {
                        GL.Vertex2((SelectedKeyframe._index - min) * _xScale, (two - _minVal) * _yScale);
                    }
                }

                if (SelectedKeyframe._next._index != -1)
                {
                    has2nd = GetFrameValue(SelectedKeyframe._next._index, out one, out two);
                    GL.Vertex2((SelectedKeyframe._next._index - min) * _xScale, (one - _minVal) * _yScale);
                    if (has2nd)
                    {
                        GL.Vertex2((SelectedKeyframe._next._index - min) * _xScale, (two - _minVal) * _yScale);
                    }
                }

                GL.End();
            }

            GL.Disable(EnableCap.ScissorTest);
        }

        #endregion
    }
}