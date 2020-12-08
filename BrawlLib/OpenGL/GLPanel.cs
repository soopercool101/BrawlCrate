using BrawlLib.Internal;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

namespace BrawlLib.OpenGL
{
    public static class ControlExtension
    {
        [ReflectionPermission(SecurityAction.Demand, MemberAccess = true)]
        public static void Reset(this Control c)
        {
            typeof(Control).InvokeMember("SetState", BindingFlags.NonPublic |
                                                     BindingFlags.InvokeMethod | BindingFlags.Instance, null,
                c, new object[] {0x400000, false});
        }
    }

    public delegate void ViewportAction(GLViewport p);

    public abstract class GLPanel : UserControl, IEnumerable<GLViewport>
    {
        public static GLPanel Current
        {
            get
            {
                if (_currentPanel == null && TKContext.CurrentContext != null)
                {
                    TKContext.CurrentContext.Capture(true);
                }

                return _currentPanel;
            }
        }

        private static GLPanel _currentPanel;

        public TKContext Context => _ctx;
        protected TKContext _ctx;

        public event ViewportAction OnCurrentViewportChanged;

        protected int _updateCounter;
        protected GLViewport _currentViewport;
        protected GLViewport _highlightedViewport;
        protected List<GLViewport> _viewports = new List<GLViewport>();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GLViewport HighlightedViewport => _viewports.Count > 1 ? _highlightedViewport :
            _viewports.Count > 0 ? _viewports[0] :
            CurrentViewport;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GLViewport CurrentViewport
        {
            get
            {
                if (_currentViewport == null)
                {
                    if (_viewports.Count == 0)
                    {
                        CreateDefaultViewport();
                    }

                    _currentViewport = _viewports[0];
                }

                if (_currentViewport._owner != this)
                {
                    _currentViewport._owner = this;
                }

                return _currentViewport;
            }
            set
            {
                _currentViewport = value;

                OnCurrentViewportChanged?.Invoke(_currentViewport);

                if (!_viewports.Contains(_currentViewport) && _currentViewport != null)
                {
                    AddViewport(_currentViewport);
                }
            }
        }

        public GLPanel()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.Opaque |
                ControlStyles.ResizeRedraw,
                true);

            _viewports = new List<GLViewport>();
        }

        public void ClearViewports()
        {
            foreach (GLViewport v in _viewports)
            {
                v._owner = null;
                v.OnInvalidate -= Invalidate;
            }

            _viewports.Clear();
        }

        public void AddViewport(GLViewport v)
        {
            _viewports.Add(v);
            v._owner = this;
            v.Resize();
            v.OnInvalidate += Invalidate;
        }

        public void RemoveViewport(GLViewport v)
        {
            if (_viewports.Contains(v))
            {
                v._owner = null;
                v.OnInvalidate -= Invalidate;
                _viewports.Remove(v);
                FitViewports();
                Invalidate();
            }
        }

        public void RemoveViewport(int index)
        {
            if (index < 0 || index >= _viewports.Count)
            {
                return;
            }

            GLViewport v = _viewports[index];
            v._owner = null;
            v.OnInvalidate -= Invalidate;
            _viewports.RemoveAt(index);
        }

        protected override void Dispose(bool disposing)
        {
            DisposeContext();
            base.Dispose(disposing);
        }

        private void DisposeContext()
        {
            if (_ctx != null)
            {
                _ctx.Unbind();
                _ctx.Dispose();
                _ctx = null;
            }
        }

        public void FitViewports()
        {
            //TODO: this doesn't work right in every circumstance

            foreach (GLViewport v in _viewports)
            {
                Vector4 p = v.Percentages;
                GLViewport x = null, y = null, z = null, w = null;
                Vector4 diff = new Vector4(float.MaxValue);
                foreach (GLViewport v2 in _viewports)
                {
                    if (v2 == v)
                    {
                        continue;
                    }

                    float diff2 = Math.Abs(p._x - v2.Percentages._x);
                    if (diff2 <= diff._x)
                    {
                        x = v2;
                        diff._x = diff2;
                    }

                    diff2 = Math.Abs(p._y - v2.Percentages._y);
                    if (diff2 <= diff._y)
                    {
                        y = v2;
                        diff._y = diff2;
                    }

                    diff2 = Math.Abs(p._z - v2.Percentages._z);
                    if (diff2 <= diff._z)
                    {
                        z = v2;
                        diff._z = diff2;
                    }

                    diff2 = Math.Abs(p._w - v2.Percentages._w);
                    if (diff2 <= diff._w)
                    {
                        w = v2;
                        diff._w = diff2;
                    }
                }

                Vector4 average = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
                if (diff._x > 0.0f && x != null)
                {
                    average._x = (v.Percentages._x + x.Percentages._x) / 2.0f;
                    x.SetXPercentage(average._x);
                }

                if (diff._y > 0.0f && y != null)
                {
                    average._y = (v.Percentages._y + y.Percentages._y) / 2.0f;
                    y.SetYPercentage(average._y);
                }

                if (diff._z > 0.0f && z != null)
                {
                    average._z = (v.Percentages._z + z.Percentages._z) / 2.0f;
                    z.SetZPercentage(average._z);
                }

                if (diff._w > 0.0f && w != null)
                {
                    average._w = (v.Percentages._w + w.Percentages._w) / 2.0f;
                    w.SetWPercentage(average._w);
                }

                v.SetPercentages(average);
            }
        }

        public void BeginUpdate()
        {
            _updateCounter++;
        }

        public void EndUpdate()
        {
            if ((_updateCounter = Math.Max(_updateCounter - 1, 0)) == 0)
            {
                Invalidate();
            }
        }

        public new void Capture()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(Capture));
                return;
            }

            if (_ctx == null)
            {
                _ctx = new TKContext(this);
            }

            _ctx.Capture();
        }

        public void Release()
        {
            _ctx?.Release();
        }

        protected override void OnLoad(EventArgs e)
        {
            _ctx = new TKContext(this);

            Capture();

            Vector3 v = (Vector3) BackColor;
            GL.ClearColor(v._x, v._y, v._z, 0.0f);
            GL.ClearDepth(1.0);

            OnInit(_ctx);

            _ctx.ContextChanged += OnContextChanged;
            _ctx.ResetOccured += OnReset;

            base.OnLoad(e);
        }

        protected virtual void OnReset(object sender, EventArgs e)
        {
            OnInit(_ctx);
        }

        protected virtual void OnContextChanged(bool isNowCurrent)
        {
            //Don't update anything if this context has just been released
            if (isNowCurrent)
            {
                OnResize(EventArgs.Empty);
            }

            _currentPanel = isNowCurrent ? this : null;
        }

        protected override void DestroyHandle()
        {
            DisposeContext();
            base.DestroyHandle();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            DisposeContext();
            base.OnHandleDestroyed(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        public GLViewport GetViewport(int x, int y)
        {
            if (_viewports.Count == 1)
            {
                return _viewports[0];
            }

            x = x.Clamp(0, Width);
            y = Height - y.Clamp(0, Height);

            foreach (GLViewport w in _viewports)
            {
                if (x >= w.Region.X &&
                    x <= w.Region.X + w.Region.Width &&
                    y >= w.Region.Y &&
                    y <= w.Region.Y + w.Region.Height)
                {
                    return w;
                }
            }

            if (_viewports.Count == 0)
            {
                CreateDefaultViewport();
            }

            return _viewports[0];
        }

        public virtual void CreateDefaultViewport()
        {
            AddViewport(GLViewport.DefaultPerspective);
        }

        public float GetDepth(Vector2 v)
        {
            return GetDepth((int) (v._x + 0.5f), (int) (v._y + 0.5f));
        }

        public virtual float GetDepth(int x, int y)
        {
            float val = 0;
            GL.ReadPixels(x, Height - y, 1, 1, PixelFormat.DepthComponent, PixelType.Float,
                ref val);
            return val;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_updateCounter > 0)
            {
                return;
            }

            if (_ctx == null)
            {
                base.OnPaint(e);
            }
            else if (Monitor.TryEnter(_ctx))
            {
                try
                {
                    //Direct OpenGL calls to this panel
                    Capture();

                    OnRender(e);

                    //Display newly rendered back buffer
                    _ctx.Swap();

                    //Check for errors
                    ErrorCode code = GL.GetError();
                    if (code != ErrorCode.NoError)
                    {
                        this.Reset(); //Stops the red X of death in its tracks
                    }
                }
                finally
                {
                    Monitor.Exit(_ctx);
                }
            }
        }

        internal virtual void OnInit(TKContext ctx)
        {
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_viewports.Count > 1)
            {
                _highlightedViewport = GetViewport(e.X, e.Y);
            }

            base.OnMouseMove(e);
        }

        public Vector3 TraceZ(Vector3 point, float z)
        {
            GLCamera camera = CurrentViewport.Camera;
            if (camera == null)
            {
                return new Vector3();
            }

            double a = point._z - z;
            //Perform trig functions, using camera for angles

            //Get angle, truncating to MOD 180
            //double angleX = _camera._rotation._y - ((int)(_camera._rotation._y / 180.0) * 180);

            double angleX = Math.IEEERemainder(-camera._rotation._y, 180.0);
            if (angleX < -90.0f)
            {
                angleX = -180.0f - angleX;
            }

            if (angleX > 90.0f)
            {
                angleX = 180.0f - angleX;
            }

            double angleY = Math.IEEERemainder(camera._rotation._x, 180.0);
            if (angleY < -90.0f)
            {
                angleY = -180.0f - angleY;
            }

            if (angleY > 90.0f)
            {
                angleY = 180.0f - angleY;
            }

            float lenX = (float) (Math.Tan(angleX * Math.PI / 180.0) * a);
            float lenY = (float) (Math.Tan(angleY * Math.PI / 180.0) * a);

            return new Vector3(point._x + lenX, point._y + lenY, z);
        }

        protected override void OnResize(EventArgs e)
        {
            _ctx?.Update();

            foreach (GLViewport v in _viewports)
            {
                v.Resize();
            }

            Invalidate();
        }

        protected virtual void OnRender(PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            foreach (GLViewport v in _viewports)
            {
                OnRenderViewport(e, v);
            }

            Wii.Graphics.ShaderGenerator._forceRecompile = false;
        }

        protected virtual void OnRenderViewport(PaintEventArgs e, GLViewport v)
        {
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // GLPanel
            // 
            Name = "GLPanel";
            ResumeLayout(false);
        }

        public IEnumerator<GLViewport> GetEnumerator()
        {
            return _viewports.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}