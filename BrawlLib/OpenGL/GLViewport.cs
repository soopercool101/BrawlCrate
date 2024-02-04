using BrawlLib.Internal;
using System;
using System.Drawing;

namespace BrawlLib.OpenGL
{
    public class GLViewport
    {
        public event Action OnInvalidate;

        internal GLPanel _owner;
        private bool _enabled = true;

        protected GLCamera _camera;
        protected Vector4 _percentages = new Vector4(0, 0, 1, 1);
        internal ViewportProjection _type;
        protected Rectangle _region;

        protected GLTexture _bgImage = null;
        protected bool _updateImage;
        protected BGImageType _bgType = BGImageType.Stretch;
        protected Image _backImg;
        protected Color _backColor = Color.FromKnownColor(KnownColor.Control);

        public bool _grabbing = false;
        public bool _scrolling = false;
        public int _lastX, _lastY;

        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        public ViewportProjection ViewType
        {
            get => _type;
            set => SetProjectionType(value);
        }

        public Vector4 Percentages => _percentages;

        public BGImageType BackgroundImageType
        {
            get => _bgType;
            set
            {
                _bgType = value;
                Invalidate();
            }
        }

        public GLCamera Camera
        {
            get => _camera;
            set => _camera = value;
        }

        public Rectangle Region
        {
            get => _region;
            set
            {
                _region = value;
                _camera.SetDimensions(_region.Width, _region.Height);
                Invalidate();
            }
        }

        public Image BackgroundImage
        {
            get => _backImg;
            set
            {
                _backImg?.Dispose();

                _backImg = value;
                _updateImage = true;

                Invalidate();
            }
        }

        public Color BackgroundColor
        {
            get => _backColor;
            set
            {
                _backColor = Color.FromArgb(0, value.R, value.G, value.B);
                Invalidate();
            }
        }

        public void SetPercentages(Vector4 v)
        {
            SetPercentages(v._x, v._y, v._z, v._w);
        }

        public void SetPercentages(float xMin, float yMin, float xMax, float yMax, bool resize = true)
        {
            _percentages = new Vector4(xMin.Clamp(0.0f, 1.0f), yMin.Clamp(0.0f, 1.0f), xMax.Clamp(0.0f, 1.0f),
                yMax.Clamp(0.0f, 1.0f));

            if (resize)
            {
                Resize();
            }
        }

        internal void SetPercentageIndex(int p, float v, bool resize = true)
        {
            _percentages[p.Clamp(0, 3)] = v.Clamp(0.0f, 1.0f);

            if (resize)
            {
                Resize();
            }
        }

        public virtual void ResetCamera()
        {
            _camera.Reset();
        }

        public int WorldToLocalY(int y)
        {
            return y - _owner.Height + Region.Y + Region.Height;
        }

        public int LocalToWorldY(int y)
        {
            return y + _owner.Height - Region.Height - Region.Y;
        }

        public float WorldToLocalYf(float y)
        {
            return y - _owner.Height + Region.Y + Region.Height;
        }

        public float LocalToWorldYf(float y)
        {
            return y + _owner.Height - Region.Height - Region.Y;
        }

        /// <summary>
        /// Input is a world point.
        /// Output is a viewport-screen point with a depth value of z.
        /// </summary>
        public Vector3 Project(float x, float y, float z)
        {
            return Project(new Vector3(x, y, z));
        }

        public Vector3 Project(Vector3 source)
        {
            Vector3 vec = Camera.Project(source);
            //No need to correct, the screen texture is relative to the viewport
            //vec._x += _region.X;
            //vec._y = LocalToWorldYf(vec._y);
            return vec;
        }

        /// <summary>
        /// Input is a full-screen point with a depth value of z. 
        /// Output is a world point
        /// </summary>
        public Vector3 UnProject(Vector3 source)
        {
            return UnProject(source._x, source._y, source._z);
        }

        public Vector3 UnProject(float x, float y, float z)
        {
            return Camera.UnProject(x - _region.X, WorldToLocalYf(y), z);
        }

        public void Invalidate()
        {
            OnInvalidate?.Invoke();
        }

        public void Resize()
        {
            if (_owner != null)
            {
                Resize(_owner.Width, _owner.Height);
            }
        }

        public void Resize(int width, int height)
        {
            int xMin = (int) (width * _percentages[0] + 0.5f),
                yMin = (int) (height * _percentages[1] + 0.5f),
                xMax = (int) (width * _percentages[2] + 0.5f),
                yMax = (int) (height * _percentages[3] + 0.5f);

            _region = new Rectangle(xMin, yMin, xMax - xMin, yMax - yMin);
            _camera.SetDimensions(_region.Width, _region.Height);
        }

        public ViewportProjection GetProjectionType()
        {
            return Camera.Orthographic ? ViewportProjection.Orthographic : ViewportProjection.Perspective;
        }

        public virtual void SetProjectionType(ViewportProjection type)
        {
            bool diff = type == ViewportProjection.Orthographic && _type != ViewportProjection.Perspective;

            Camera.Orthographic = (_type = type) != ViewportProjection.Perspective;
            Camera._defaultTranslate = new Vector3();

            if (_type == ViewportProjection.Perspective)
            {
                Camera._nearZ = 1;
                Camera._farZ = 200000;
                Camera._ortho = false;
                Camera._restrictXRot = false;
                Camera._restrictYRot = false;
                Camera._defaultScale = new Vector3(1, 1, 1);
                Camera._defaultRotate = new Vector3();
            }
            else
            {
                Camera._nearZ = -100000;
                Camera._farZ = 100000;
                Camera._ortho = true;
                Camera._restrictXRot =
                    Camera._restrictYRot = _type != ViewportProjection.Orthographic;
                Camera._defaultScale = GetDefaultScale();
                Camera._defaultRotate = GetDefaultRotate();
            }

            if (!diff)
            {
                Camera.Reset();
            }

            Camera.CalculateProjection();
            Invalidate();
        }

        public static GLViewport DefaultPerspective => new GLViewport
        {
            _type = ViewportProjection.Perspective,
            _camera = new GLCamera(),
            _percentages = new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
        };

        public static GLViewport BaseOrtho => new GLViewport
        {
            _type = ViewportProjection.Orthographic,
            _camera = new GLCamera
            {
                _ortho = true,
                _nearZ = -10000.0f,
                _farZ = 10000.0f,
                _defaultScale = new Vector3(0.035f, 0.035f, 0.035f)
            },
            _percentages = new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
        };

        public static GLViewport DefaultOrtho
        {
            get
            {
                GLViewport p = BaseOrtho;
                p._type = ViewportProjection.Orthographic;
                p.Camera.Reset();
                return p;
            }
        }

        public static GLViewport DefaultFront
        {
            get
            {
                GLViewport p = BaseOrtho;
                p._type = ViewportProjection.Front;
                p.Camera._restrictXRot = true;
                p.Camera._restrictYRot = true;
                p.Camera.Reset();
                return p;
            }
        }

        public static GLViewport DefaultBack
        {
            get
            {
                GLViewport p = BaseOrtho;
                p._type = ViewportProjection.Back;
                p.Camera._restrictXRot = true;
                p.Camera._restrictYRot = true;
                p.Camera._defaultRotate = new Vector3(0.0f, 180.0f, 0.0f);
                p.Camera.Reset();
                return p;
            }
        }

        public static GLViewport DefaultLeft
        {
            get
            {
                GLViewport p = BaseOrtho;
                p._type = ViewportProjection.Left;
                p.Camera._restrictXRot = true;
                p.Camera._restrictYRot = true;
                p.Camera._defaultRotate = new Vector3(0.0f, 90.0f, 0.0f);
                p.Camera.Reset();
                return p;
            }
        }

        public static GLViewport DefaultRight
        {
            get
            {
                GLViewport p = BaseOrtho;
                p._type = ViewportProjection.Right;
                p.Camera._restrictXRot = true;
                p.Camera._restrictYRot = true;
                p.Camera._defaultRotate = new Vector3(0.0f, -90.0f, 0.0f);
                p.Camera.Reset();
                return p;
            }
        }

        public static GLViewport DefaultTop
        {
            get
            {
                GLViewport p = BaseOrtho;
                p._type = ViewportProjection.Top;
                p.Camera._restrictXRot = true;
                p.Camera._restrictYRot = true;
                p.Camera._defaultRotate = new Vector3(-90.0f, 0.0f, 0.0f);
                p.Camera.Reset();
                return p;
            }
        }

        public static GLViewport DefaultBottom
        {
            get
            {
                GLViewport p = BaseOrtho;
                p._type = ViewportProjection.Top;
                p.Camera._restrictXRot = true;
                p.Camera._restrictYRot = true;
                p.Camera._defaultRotate = new Vector3(90.0f, 0.0f, 0.0f);
                p.Camera.Reset();
                return p;
            }
        }

        public virtual Vector3 GetDefaultScale()
        {
            return new Vector3(1.0f);
        }

        public virtual Vector3 GetDefaultRotate()
        {
            switch (ViewType)
            {
                default:
                    return new Vector3();
                case ViewportProjection.Left:
                    return new Vector3(0.0f, 90.0f, 0.0f);
                case ViewportProjection.Right:
                    return new Vector3(0.0f, -90.0f, 0.0f);
                case ViewportProjection.Top:
                    return new Vector3(-90.0f, 0.0f, 0.0f);
                case ViewportProjection.Bottom:
                    return new Vector3(90.0f, 0.0f, 0.0f);
                case ViewportProjection.Back:
                    return new Vector3(0.0f, 180.0f, 0.0f);
            }
        }

        public void ClearCameraDefaults()
        {
            _camera._defaultTranslate = new Vector3();
            _camera._defaultRotate = GetDefaultRotate();
            _camera._defaultScale = GetDefaultScale();
            _camera.Reset();
        }

        public void SetXPercentage(float p)
        {
            _percentages._x = p;
        }

        public void SetYPercentage(float p)
        {
            _percentages._y = p;
        }

        public void SetZPercentage(float p)
        {
            _percentages._z = p;
        }

        public void SetWPercentage(float p)
        {
            _percentages._w = p;
        }
    }

    public enum BGImageType
    {
        Stretch,
        Center,
        ResizeWithBars
    }

    public enum ViewportProjection : uint
    {
        Perspective,
        Orthographic,
        Front,
        Back,
        Left,
        Right,
        Top,
        Bottom
    }
}