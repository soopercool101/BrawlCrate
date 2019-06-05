using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace BrawlLib.OpenGL
{
    public delegate T GLCreateHandler<T>();

    public class TKContext : IDisposable
    {
        public delegate void ContextChangedEventHandler(bool isCurrent);

        public static List<TKContext> _boundContexts;
        public static TKContext CurrentContext;

        public static bool _shadersSupported = true;
        public static int _versionMax;
        public static int _versionMin;
        public static bool _anyContextInitialized;
        private IGraphicsContext _context;

        internal Dictionary<string, object> _states = new Dictionary<string, object>();

        public Control _window;

        public TKContext(Control window)
        {
            _window = window;
            WindowInfo = Utilities.CreateWindowsWindowInfo(_window.Handle);
            _context = new GraphicsContext(GraphicsMode.Default, WindowInfo);
            _context.MakeCurrent(WindowInfo);
            _context.LoadAll();

            if (!_anyContextInitialized)
            {
                // Check for GLSL support
                var version = GL.GetString(StringName.Version).Split('.', ' ');
                _versionMax = int.Parse(version[0]);
                _versionMin = int.Parse(version[1]);

                //Need OpenGL 2.1 to use GLSL 120
                _shadersSupported = !(_versionMax < 2 || _versionMax == 2 && _versionMin < 1);
                _anyContextInitialized = true;
            }

            BoundContexts.Add(this);
        }

        public IWindowInfo WindowInfo { get; private set; }

        //These provide a way to manage which context is in use to avoid errors
        public static List<TKContext> BoundContexts => _boundContexts ?? (_boundContexts = new List<TKContext>());

        public bool Resetting { get; private set; }

        public void Dispose()
        {
            Release();
            if (_context != null) _context.Dispose();

            if (BoundContexts.Contains(this)) BoundContexts.Remove(this);
        }

        public static T FindOrCreate<T>(string name, GLCreateHandler<T> handler)
        {
            if (CurrentContext == null) return default;

            if (CurrentContext._states.ContainsKey(name)) return (T) CurrentContext._states[name];

            var obj = handler();
            CurrentContext._states[name] = obj;
            return obj;
        }

        public void Unbind()
        {
            try
            {
                foreach (var o in _states.Values)
                    if (o is GLDisplayList)
                        (o as GLDisplayList).Delete();
                    else if (o is GLTexture) (o as GLTexture).Delete();
            }
            catch
            {
            }

            _states.Clear();
        }

        public event ContextChangedEventHandler ContextChanged;
        public event EventHandler ResetOccured;

        public void CheckErrors()
        {
            var code = GL.GetError();
            if (code == ErrorCode.NoError) return;

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
                    if (CurrentContext != null) CurrentContext.Release();

                    //Make this context the current one
                    CurrentContext = this;
                    if (!_context.IsCurrent) _context.MakeCurrent(WindowInfo);

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
                    _context.SwapBuffers();
            }
            catch //(Exception x)
            {
                //MessageBox.Show(x.ToString());
                Reset();
            }
        }

        public void Reset()
        {
            if (Resetting) //Prevent a possible infinite loop
                return;

            Resetting = true;
            _window.Reset();
            Dispose();

            WindowInfo = Utilities.CreateWindowsWindowInfo(_window.Handle);
            _context = new GraphicsContext(GraphicsMode.Default, WindowInfo);
            Capture(true);
            _context.LoadAll();
            Update();

            ResetOccured?.Invoke(this, EventArgs.Empty);

            Resetting = false;
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
            if (CurrentContext == this) _context.Update(WindowInfo);
        }

        public static void InvalidateModelPanels(IRenderedObject obj)
        {
            if (_boundContexts != null)
                foreach (var x in _boundContexts)
                    if (x._window is ModelPanel /* && ((ModelPanel)x._window)._renderList.Contains(obj)*/)
                        x._window.Invalidate();
        }

        public static void DrawWireframeBox(Box value)
        {
            DrawWireframeBox(value.Min, value.Max);
        }

        public static void DrawWireframeBox(Vector3 min, Vector3 max)
        {
            GL.Begin(PrimitiveType.LineStrip);

            GL.Vertex3(max._x, max._y, max._z);
            GL.Vertex3(max._x, max._y, min._z);
            GL.Vertex3(min._x, max._y, min._z);
            GL.Vertex3(min._x, min._y, min._z);
            GL.Vertex3(min._x, min._y, max._z);
            GL.Vertex3(max._x, min._y, max._z);
            GL.Vertex3(max._x, max._y, max._z);

            GL.End();

            GL.Begin(PrimitiveType.Lines);

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
            GL.Begin(PrimitiveType.QuadStrip);

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

            GL.Begin(PrimitiveType.Quads);

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
            GL.Begin(PrimitiveType.QuadStrip);

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

            GL.Begin(PrimitiveType.Quads);

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
            var p1 = new Vector3(p._x + radius, p._y + radius, p._z + radius);
            var p2 = new Vector3(p._x - radius, p._y - radius, p._z - radius);
            DrawBox(p2, p1);
        }

        public static void DrawInvertedCube(Vector3 p, float radius)
        {
            var p1 = new Vector3(p._x + radius, p._y + radius, p._z + radius);
            var p2 = new Vector3(p._x - radius, p._y - radius, p._z - radius);
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
            var list = new GLDisplayList();
            GL.Begin(PrimitiveType.Lines);

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
            var list = new GLDisplayList();
            list.Begin();

            GL.Begin(PrimitiveType.LineLoop);

            var angle = 0.0f;
            for (var i = 0; i < 360; i++, angle = i * Maths._deg2radf) GL.Vertex2(Math.Cos(angle), Math.Sin(angle));

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
            var list = new GLDisplayList();
            list.Begin();

            GL.Begin(PrimitiveType.LineLoop);

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
            var list = new GLDisplayList();
            list.Begin();

            GL.Begin(PrimitiveType.Lines);

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
            var list = new GLDisplayList();
            list.Begin();

            GL.Begin(PrimitiveType.QuadStrip);

            var p1 = new Vector3(0);
            var p2 = new Vector3(0.99f);

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

            GL.Begin(PrimitiveType.Quads);

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
            var list = new GLDisplayList();
            list.Begin();

            GL.Begin(PrimitiveType.TriangleFan);

            GL.Vertex3(0.0f, 0.0f, 0.0f);

            var angle = 0.0f;
            for (var i = 0; i < 361; i++, angle = i * Maths._deg2radf) GL.Vertex2(Math.Cos(angle), Math.Sin(angle));

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
            var dl = new GLDisplayList();

            dl.Begin();
            DrawSphere(new Vector3(), 1.0f, 40);
            dl.End();

            return dl;
        }

        public static void DrawSphere(Vector3 center, float radius, uint precision)
        {
            if (radius < 0.0f) radius = -radius;

            if (radius == 0.0f) throw new DivideByZeroException("DrawSphere: Radius cannot be zero.");

            if (precision == 0) throw new DivideByZeroException("DrawSphere: Precision of 8 or greater is required.");

            var halfPI = (float) (Math.PI * 0.5);
            var oneThroughPrecision = 1.0f / precision;
            var twoPIThroughPrecision = (float) (Math.PI * 2.0 * oneThroughPrecision);

            float theta1, theta2, theta3;
            Vector3 norm, pos;

            for (uint j = 0; j < precision / 2; j++)
            {
                theta1 = j * twoPIThroughPrecision - halfPI;
                theta2 = (j + 1) * twoPIThroughPrecision - halfPI;

                GL.Begin(PrimitiveType.TriangleStrip);
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