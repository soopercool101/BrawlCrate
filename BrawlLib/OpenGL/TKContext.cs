using BrawlLib.Internal;
using BrawlLib.Internal.Windows.Controls.Model_Panel;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BrawlLib.OpenGL
{
    public delegate T GLCreateHandler<T>();

    public class TKContext : IDisposable
    {
        private IGraphicsContext _context;
        private IWindowInfo _winInfo;
        public IWindowInfo WindowInfo => _winInfo;

        //These provide a way to manage which context is in use to avoid errors
        public static List<TKContext> BoundContexts => _boundContexts ?? (_boundContexts = new List<TKContext>());
        public static List<TKContext> _boundContexts;
        public static TKContext CurrentContext;

        internal Dictionary<string, object> _states = new Dictionary<string, object>();

        public static T FindOrCreate<T>(string name, GLCreateHandler<T> handler)
        {
            if (CurrentContext == null)
            {
                return default(T);
            }

            if (CurrentContext._states.ContainsKey(name))
            {
                return (T) CurrentContext._states[name];
            }

            T obj = handler();
            CurrentContext._states[name] = obj;
            return obj;
        }

        public void Unbind()
        {
            try
            {
                foreach (object o in _states.Values)
                {
                    if (o is GLDisplayList)
                    {
                        (o as GLDisplayList).Delete();
                    }
                    else
                    {
                        (o as GLTexture)?.Delete();
                    }
                }
            }
            catch
            {
                // ignored
            }

            _states.Clear();
        }

        public static bool _shadersSupported = true;
        public static int _versionMax;
        public static int _versionMin;
        public static bool _anyContextInitialized;

        public Control _window;

        public TKContext(Control window)
        {
            _window = window;
            _winInfo = Utilities.CreateWindowsWindowInfo(_window.Handle);
            _context = new GraphicsContext(GraphicsMode.Default, _winInfo);
            _context.MakeCurrent(WindowInfo);
            _context.LoadAll();

            if (!_anyContextInitialized)
            {
                // Check for GLSL support
                string[] version = GL.GetString(StringName.Version).Split('.', ' ');
                _versionMax = int.Parse(version[0]);
                _versionMin = int.Parse(version[1]);

                //Need OpenGL 2.1 to use GLSL 120
                _shadersSupported = !(_versionMax < 2 || _versionMax == 2 && _versionMin < 1);
                _anyContextInitialized = true;
            }

            BoundContexts.Add(this);
        }

        public delegate void ContextChangedEventHandler(bool isCurrent);

        public event ContextChangedEventHandler ContextChanged;
        public event EventHandler ResetOccured;

        public void Dispose()
        {
            Release();
            _context?.Dispose();

            if (BoundContexts.Contains(this))
            {
                BoundContexts.Remove(this);
            }
        }

        public void CheckErrors()
        {
            ErrorCode code = GL.GetError();
            if (code == ErrorCode.NoError)
            {
                return;
            }

            //throw new Exception(code.ToString());
            Reset();
        }

        public void Capture(bool force = false)
        {
            try
            {
                //Only proceed if this window is not already current
                if (force || CurrentContext != this)
                {
                    //Release the current context if it exists
                    CurrentContext?.Release();

                    //Make this context the current one
                    CurrentContext = this;
                    if (!_context.IsCurrent)
                    {
                        _context.MakeCurrent(WindowInfo);
                    }

                    ContextChanged?.Invoke(true);
                }
            }
            catch //(Exception x)
            {
                //MessageBox.Show(x.ToString()); 
                Reset();
            }
        }

        public void Swap()
        {
            try
            {
                if (CurrentContext == this &&
                    _context != null &&
                    _context.IsCurrent &&
                    !_context.IsDisposed)
                {
                    _context.SwapBuffers();
                }
            }
            catch //(Exception x)
            {
                //MessageBox.Show(x.ToString());
                Reset();
            }
        }

        public bool Resetting => _resetting;
        private bool _resetting;

        public void Reset()
        {
            if (_resetting) //Prevent a possible infinite loop
            {
                return;
            }

            _resetting = true;
            _window.Reset();
            Dispose();

            _winInfo = Utilities.CreateWindowsWindowInfo(_window.Handle);
            _context = new GraphicsContext(GraphicsMode.Default, WindowInfo);
            Capture(true);
            _context.LoadAll();
            Update();

            ResetOccured?.Invoke(this, EventArgs.Empty);

            _resetting = false;
        }

        public void Release()
        {
            if (CurrentContext == this && !_context.IsDisposed && _context.IsCurrent)
            {
                CurrentContext = null;
                _context.MakeCurrent(null);

                ContextChanged?.Invoke(false);
            }
        }

        public void Update()
        {
            if (CurrentContext == this)
            {
                _context.Update(WindowInfo);
            }
        }

        public static void InvalidateModelPanels(IRenderedObject obj)
        {
            if (_boundContexts != null)
            {
                foreach (TKContext x in _boundContexts)
                {
                    if (x._window is ModelPanel /* && ((ModelPanel)x._window)._renderList.Contains(obj)*/)
                    {
                        x._window.Invalidate();
                    }
                }
            }
        }

        public static void DrawWireframeBox(Box value)
        {
            DrawWireframeBox(value.Min, value.Max);
        }

        public static void DrawWireframeBox(Vector3 min, Vector3 max)
        {
            GL.Begin(BeginMode.LineStrip);

            GL.Vertex3(max._x, max._y, max._z);
            GL.Vertex3(max._x, max._y, min._z);
            GL.Vertex3(min._x, max._y, min._z);
            GL.Vertex3(min._x, min._y, min._z);
            GL.Vertex3(min._x, min._y, max._z);
            GL.Vertex3(max._x, min._y, max._z);
            GL.Vertex3(max._x, max._y, max._z);

            GL.End();

            GL.Begin(BeginMode.Lines);

            GL.Vertex3(min._x, max._y, max._z);
            GL.Vertex3(max._x, max._y, max._z);
            GL.Vertex3(min._x, max._y, max._z);
            GL.Vertex3(min._x, min._y, max._z);
            GL.Vertex3(min._x, max._y, max._z);
            GL.Vertex3(min._x, max._y, min._z);
            GL.Vertex3(max._x, min._y, min._z);
            GL.Vertex3(min._x, min._y, min._z);
            GL.Vertex3(max._x, min._y, min._z);
            GL.Vertex3(max._x, max._y, min._z);
            GL.Vertex3(max._x, min._y, min._z);
            GL.Vertex3(max._x, min._y, max._z);

            GL.End();
        }

        public static void DrawBox(Box value)
        {
            DrawBox(value.Min, value.Max);
        }

        public static void DrawBox(Vector3 p1, Vector3 p2)
        {
            GL.Begin(BeginMode.QuadStrip);

            GL.Vertex3(p1._x, p1._y, p1._z);
            GL.Vertex3(p1._x, p2._y, p1._z);
            GL.Vertex3(p2._x, p1._y, p1._z);
            GL.Vertex3(p2._x, p2._y, p1._z);
            GL.Vertex3(p2._x, p1._y, p2._z);
            GL.Vertex3(p2._x, p2._y, p2._z);
            GL.Vertex3(p1._x, p1._y, p2._z);
            GL.Vertex3(p1._x, p2._y, p2._z);
            GL.Vertex3(p1._x, p1._y, p1._z);
            GL.Vertex3(p1._x, p2._y, p1._z);

            GL.End();

            GL.Begin(BeginMode.Quads);

            GL.Vertex3(p1._x, p2._y, p1._z);
            GL.Vertex3(p1._x, p2._y, p2._z);
            GL.Vertex3(p2._x, p2._y, p2._z);
            GL.Vertex3(p2._x, p2._y, p1._z);

            GL.Vertex3(p1._x, p1._y, p1._z);
            GL.Vertex3(p1._x, p1._y, p2._z);
            GL.Vertex3(p2._x, p1._y, p2._z);
            GL.Vertex3(p2._x, p1._y, p1._z);

            GL.End();
        }

        public static void DrawInvertedBox(Box value)
        {
            DrawInvertedBox(value.Min, value.Max);
        }

        public static void DrawInvertedBox(Vector3 p1, Vector3 p2)
        {
            GL.Begin(BeginMode.QuadStrip);

            GL.Vertex3(p1._x, p1._y, p1._z);
            GL.Vertex3(p1._x, p2._y, p1._z);
            GL.Vertex3(p1._x, p1._y, p2._z);
            GL.Vertex3(p1._x, p2._y, p2._z);
            GL.Vertex3(p2._x, p1._y, p2._z);
            GL.Vertex3(p2._x, p2._y, p2._z);
            GL.Vertex3(p2._x, p1._y, p1._z);
            GL.Vertex3(p2._x, p2._y, p1._z);
            GL.Vertex3(p1._x, p1._y, p1._z);
            GL.Vertex3(p1._x, p2._y, p1._z);

            GL.End();

            GL.Begin(BeginMode.Quads);

            GL.Vertex3(p2._x, p2._y, p1._z);
            GL.Vertex3(p2._x, p2._y, p2._z);
            GL.Vertex3(p1._x, p2._y, p2._z);
            GL.Vertex3(p1._x, p2._y, p1._z);

            GL.Vertex3(p1._x, p1._y, p1._z);
            GL.Vertex3(p1._x, p1._y, p2._z);
            GL.Vertex3(p2._x, p1._y, p2._z);
            GL.Vertex3(p2._x, p1._y, p1._z);

            GL.End();
        }

        public static void DrawCube(Vector3 p, float radius)
        {
            Vector3 p1 = new Vector3(p._x + radius, p._y + radius, p._z + radius);
            Vector3 p2 = new Vector3(p._x - radius, p._y - radius, p._z - radius);
            DrawBox(p2, p1);
        }

        public static void DrawInvertedCube(Vector3 p, float radius)
        {
            Vector3 p1 = new Vector3(p._x + radius, p._y + radius, p._z + radius);
            Vector3 p2 = new Vector3(p._x - radius, p._y - radius, p._z - radius);
            DrawInvertedBox(p2, p1);
        }

        public static void DrawRing(float radius)
        {
            GL.PushMatrix();
            GL.Scale(radius, radius, radius);
            GetRingList().Call();
            GL.PopMatrix();
        }

        public GLDisplayList GetLine()
        {
            return FindOrCreate("Line", CreateLine);
        }

        private static GLDisplayList CreateLine()
        {
            GLDisplayList list = new GLDisplayList();
            GL.Begin(BeginMode.Lines);

            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(2.0f, 0.0f, 0.0f);

            GL.End();

            list.End();
            return list;
        }

        public static GLDisplayList GetRingList()
        {
            return FindOrCreate("Ring", CreateRing);
        }

        private static GLDisplayList CreateRing()
        {
            GLDisplayList list = new GLDisplayList();
            list.Begin();

            GL.Begin(BeginMode.LineLoop);

            float angle = 0.0f;
            for (int i = 0; i < 360; i++, angle = i * Maths._deg2radf)
            {
                GL.Vertex2(Math.Cos(angle), Math.Sin(angle));
            }

            GL.End();

            list.End();
            return list;
        }

        public static GLDisplayList GetSquareList()
        {
            return FindOrCreate("Square", CreateSquare);
        }

        private static GLDisplayList CreateSquare()
        {
            GLDisplayList list = new GLDisplayList();
            list.Begin();

            GL.Begin(BeginMode.LineLoop);

            GL.Vertex2(0.0f, 0.0f);
            GL.Vertex2(0.0f, 1.0f);
            GL.Vertex2(1.0f, 1.0f);
            GL.Vertex2(1.0f, 0.0f);
            GL.Vertex2(0.0f, 0.0f);

            GL.End();

            list.End();
            return list;
        }

        public static GLDisplayList GetAxisList()
        {
            return FindOrCreate("Axes", CreateAxes);
        }

        private static GLDisplayList CreateAxes()
        {
            GLDisplayList list = new GLDisplayList();
            list.Begin();

            GL.Begin(BeginMode.Lines);

            GL.Color4(1.0f, 0.0f, 0.0f, 1.0f);

            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(2.0f, 0.0f, 0.0f);
            GL.Vertex3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(1.0f, 1.0f, 0.0f);
            GL.Vertex3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(1.0f, 0.0f, 1.0f);

            GL.Color4(0.0f, 1.0f, 0.0f, 1.0f);

            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 2.0f, 0.0f);
            GL.Vertex3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(1.0f, 1.0f, 0.0f);
            GL.Vertex3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(0.0f, 1.0f, 1.0f);

            GL.Color4(0.0f, 0.0f, 1.0f, 1.0f);

            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 2.0f);
            GL.Vertex3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(1.0f, 0.0f, 1.0f);
            GL.Vertex3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(0.0f, 1.0f, 1.0f);

            GL.End();

            list.End();
            return list;
        }

        public static GLDisplayList GetCubeList()
        {
            return FindOrCreate("Cube", CreateCube);
        }

        private static GLDisplayList CreateCube()
        {
            GLDisplayList list = new GLDisplayList();
            list.Begin();

            GL.Begin(BeginMode.QuadStrip);

            Vector3 p1 = new Vector3(0);
            Vector3 p2 = new Vector3(0.99f);

            GL.Vertex3(p1._x, p1._y, p1._z);
            GL.Vertex3(p1._x, p2._y, p1._z);
            GL.Vertex3(p2._x, p1._y, p1._z);
            GL.Vertex3(p2._x, p2._y, p1._z);
            GL.Vertex3(p2._x, p1._y, p2._z);
            GL.Vertex3(p2._x, p2._y, p2._z);
            GL.Vertex3(p1._x, p1._y, p2._z);
            GL.Vertex3(p1._x, p2._y, p2._z);
            GL.Vertex3(p1._x, p1._y, p1._z);
            GL.Vertex3(p1._x, p2._y, p1._z);

            GL.End();

            GL.Begin(BeginMode.Quads);

            GL.Vertex3(p1._x, p2._y, p1._z);
            GL.Vertex3(p1._x, p2._y, p2._z);
            GL.Vertex3(p2._x, p2._y, p2._z);
            GL.Vertex3(p2._x, p2._y, p1._z);

            GL.Vertex3(p1._x, p1._y, p1._z);
            GL.Vertex3(p1._x, p1._y, p2._z);
            GL.Vertex3(p2._x, p1._y, p2._z);
            GL.Vertex3(p2._x, p1._y, p1._z);

            GL.End();

            list.End();
            return list;
        }

        public static GLDisplayList GetCircleList()
        {
            return FindOrCreate("Circle", CreateCircle);
        }

        private static GLDisplayList CreateCircle()
        {
            GLDisplayList list = new GLDisplayList();
            list.Begin();

            GL.Begin(BeginMode.TriangleFan);

            GL.Vertex3(0.0f, 0.0f, 0.0f);

            float angle = 0.0f;
            for (int i = 0; i < 361; i++, angle = i * Maths._deg2radf)
            {
                GL.Vertex2(Math.Cos(angle), Math.Sin(angle));
            }

            GL.End();

            list.End();
            return list;
        }

        public static void DrawSphere(float radius)
        {
            GL.PushMatrix();
            GL.Scale(radius, radius, radius);
            GetSphereList().Call();
            GL.PopMatrix();
        }

        public static GLDisplayList GetSphereList()
        {
            return FindOrCreate("Sphere", CreateSphere);
        }

        public static GLDisplayList CreateSphere()
        {
            GLDisplayList dl = new GLDisplayList();

            dl.Begin();
            DrawSphere(new Vector3(), 1.0f, 40);
            dl.End();

            return dl;
        }

        public static void DrawSphere(Vector3 center, float radius, uint precision)
        {
            if (radius < 0.0f)
            {
                radius = -radius;
            }

            if (radius == 0.0f)
            {
                throw new DivideByZeroException("DrawSphere: Radius cannot be zero.");
            }

            if (precision == 0)
            {
                throw new DivideByZeroException("DrawSphere: Precision of 8 or greater is required.");
            }

            float halfPI = (float) (Math.PI * 0.5);
            float oneThroughPrecision = 1.0f / precision;
            float twoPIThroughPrecision = (float) (Math.PI * 2.0 * oneThroughPrecision);

            float theta1, theta2, theta3;
            Vector3 norm, pos;

            for (uint j = 0; j < precision / 2; j++)
            {
                theta1 = j * twoPIThroughPrecision - halfPI;
                theta2 = (j + 1) * twoPIThroughPrecision - halfPI;

                GL.Begin(BeginMode.TriangleStrip);
                for (uint i = 0; i <= precision; i++)
                {
                    theta3 = i * twoPIThroughPrecision;

                    norm._x = (float) (Math.Cos(theta2) * Math.Cos(theta3));
                    norm._y = (float) Math.Sin(theta2);
                    norm._z = (float) (Math.Cos(theta2) * Math.Sin(theta3));
                    pos._x = center._x + radius * norm._x;
                    pos._y = center._y + radius * norm._y;
                    pos._z = center._z + radius * norm._z;

                    GL.Normal3(norm._x, norm._y, norm._z);
                    GL.TexCoord2(i * oneThroughPrecision, 2.0f * (j + 1) * oneThroughPrecision);
                    GL.Vertex3(pos._x, pos._y, pos._z);

                    norm._x = (float) (Math.Cos(theta1) * Math.Cos(theta3));
                    norm._y = (float) Math.Sin(theta1);
                    norm._z = (float) (Math.Cos(theta1) * Math.Sin(theta3));
                    pos._x = center._x + radius * norm._x;
                    pos._y = center._y + radius * norm._y;
                    pos._z = center._z + radius * norm._z;

                    GL.Normal3(norm._x, norm._y, norm._z);
                    GL.TexCoord2(i * oneThroughPrecision, 2.0f * j * oneThroughPrecision);
                    GL.Vertex3(pos._x, pos._y, pos._z);
                }

                GL.End();
            }
        }
    }
}