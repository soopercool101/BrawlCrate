using BrawlLib.Internal;
using OpenTK.Graphics.OpenGL;
using System;

namespace BrawlLib.OpenGL
{
    public unsafe class GLCamera
    {
        public Matrix _matrix;
        public Matrix _matrixInverse;
        public Matrix _projectionMatrix;
        public Matrix _projectionInverse;

        public Vector3 _rotation;
        public Vector3 _scale;

        public bool _ortho, _restrictXRot, _restrictYRot, _restrictZRot;

        public float
            _fovY = 45.0f,
            _nearZ = 1.0f,
            _farZ = 200000.0f,
            _width = 1,
            _height = 1,
            _aspect = 1;

        public Vector4 _orthoDimensions = new Vector4(0, 1, 0, 1);

        public Vector3 _defaultTranslate;
        public Vector3 _defaultRotate;
        public Vector3 _defaultScale = new Vector3(1);

        public void SetProjectionParams(float aspect, float fovy, float farz, float nearz)
        {
            _aspect = aspect;
            _fovY = fovy;
            _farZ = farz;
            _nearZ = nearz;

            CalculateProjection();
        }

        public float VerticalFieldOfView
        {
            get => _fovY;
            set
            {
                _fovY = value;
                CalculateProjection();
            }
        }

        public float NearDepth
        {
            get => _nearZ;
            set
            {
                _nearZ = value;
                CalculateProjection();
            }
        }

        public float FarDepth
        {
            get => _farZ;
            set
            {
                _farZ = value;
                CalculateProjection();
            }
        }

        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                CalculateProjection();
            }
        }

        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                CalculateProjection();
            }
        }

        public float Aspect
        {
            get => _aspect;
            set
            {
                _aspect = value;
                CalculateProjection();
            }
        }

        public bool Orthographic
        {
            get => _ortho;
            set
            {
                if (_ortho == value)
                {
                    return;
                }

                _ortho = value;
                CalculateProjection();
            }
        }

        public GLCamera()
        {
            Reset();
        }

        public GLCamera(float width, float height, Vector3 defaultTranslate, Vector3 defaultRotate,
                        Vector3 defaultScale)
        {
            _width = width;
            _height = height;

            _orthoDimensions = new Vector4(-_width / 2.0f, _width / 2.0f, _height / 2.0f, -_height / 2.0f);

            _scale = _defaultScale = defaultScale;
            _rotation = _defaultRotate = defaultRotate;
            _defaultTranslate = defaultTranslate;

            _matrix = Matrix.ReverseTransformMatrix(_scale, _rotation, _defaultTranslate);
            _matrixInverse = Matrix.TransformMatrix(_scale, _rotation, _defaultTranslate);
        }

        public Vector3 GetPoint()
        {
            return _matrixInverse.Multiply(new Vector3());
        }

        public void Scale(float x, float y, float z)
        {
            Scale(new Vector3(x, y, z));
        }

        public void Scale(Vector3 v)
        {
            _scale *= v;
            Apply();
        }

        public void Zoom(float amt)
        {
            if (_ortho)
            {
                float scale = amt >= 0 ? amt / 2.0f : 2.0f / -amt;
                Scale(scale, scale, scale);
            }
            else
            {
                Translate(0.0f, 0.0f, amt);
            }
        }

        public void Translate(Vector3 v)
        {
            Translate(v._x, v._y, v._z);
        }

        public void Translate(float x, float y, float z)
        {
            _matrix = Matrix.TranslationMatrix(-x, -y, -z) * _matrix;
            _matrixInverse.Translate(x, y, z);

            PositionChanged();
        }

        public void Rotate(float x, float y, float z)
        {
            Rotate(new Vector3(x, y, z));
        }

        public void Rotate(Vector3 v)
        {
            //Fix for left and right dragging when the camera is upside down
            if (_rotation._x < -90.0f || _rotation._x > 90.0f)
            {
                v._y = -v._y;
            }

            if (_restrictXRot)
            {
                v._x = 0.0f;
            }

            if (_restrictYRot)
            {
                v._y = 0.0f;
            }

            if (_restrictZRot)
            {
                v._z = 0.0f;
            }

            _rotation = (_rotation + v).RemappedToRange(-180.0f, 180.0f);

            Apply();
        }

        private void Apply()
        {
            //Grab vertex from matrix
            Vector3 point = GetPoint();

            //Reset matrices
            _matrix = Matrix.ReverseTransformMatrix(_scale, _rotation, point);
            _matrixInverse = Matrix.TransformMatrix(_scale, _rotation, point);

            PositionChanged();
        }

        public void Rotate(float x, float y)
        {
            Rotate(x, y, 0);
        }

        public void Pivot(float radius, float x, float y)
        {
            _updating = true;
            Translate(0, 0, -radius);
            Rotate(x, y);
            Translate(0, 0, radius);
            _updating = false;
            PositionChanged();
        }

        public void Set(Vector3 translate, Vector3 rotate, Vector3 scale)
        {
            _scale = scale;
            _rotation = rotate;

            _matrix = Matrix.ReverseTransformMatrix(_scale, _rotation, translate);
            _matrixInverse = Matrix.TransformMatrix(_scale, _rotation, translate);

            PositionChanged();
        }

        public void Reset()
        {
            _rotation = _defaultRotate;
            _scale = _defaultScale;
            _matrix = Matrix.ReverseTransformMatrix(_scale, _rotation, _defaultTranslate);
            _matrixInverse = Matrix.TransformMatrix(_scale, _rotation, _defaultTranslate);

            PositionChanged();
        }

        private bool _updating;

        private void PositionChanged()
        {
            if (!_updating)
            {
                OnPositionChanged?.Invoke();
            }
        }

        public void CalculateProjection()
        {
            if (_ortho)
            {
                _projectionMatrix = Matrix.OrthographicMatrix(_orthoDimensions, _nearZ, _farZ);
                _projectionInverse = Matrix.ReverseOrthographicMatrix(_orthoDimensions, _nearZ, _farZ);
            }
            else
            {
                _projectionMatrix = Matrix.PerspectiveMatrix(_fovY, _aspect, _nearZ, _farZ);
                _projectionInverse = Matrix.ReversePerspectiveMatrix(_fovY, _aspect, _nearZ, _farZ);
            }
        }

        public event Action OnDimensionsChanged, OnPositionChanged;

        public void SetDimensions(float width, float height)
        {
            _width = width;
            _height = height;
            _aspect = _width / _height;

            _orthoDimensions = new Vector4(-_width / 2.0f, _width / 2.0f, _height / 2.0f, -_height / 2.0f);

            CalculateProjection();

            OnDimensionsChanged?.Invoke();
        }

        /// <summary>
        /// Projects a screen point to world coordinates.
        /// </summary>
        /// <returns>3D world point perpendicular to the camera with a depth value of z (z is not a distance value!)</returns>
        public Vector3 UnProject(Vector3 point)
        {
            return UnProject(point._x, point._y, point._z);
        }

        /// <summary>
        /// Projects a screen point to world coordinates.
        /// </summary>
        /// <returns>3D world point perpendicular to the camera with a depth value of z (z is not a distance value!)</returns>
        public Vector3 UnProject(float x, float y, float z)
        {
            //This needs to be a Vector4 converted to a Vector3 in order to work
            //Also the order of the matrix multiplication matters
            return (Vector3) (_matrixInverse * _projectionInverse * new Vector4(
                2.0f * (x / Width) - 1.0f,
                2.0f * ((Height - y) / Height) - 1.0f,
                2.0f * z - 1.0f,
                1.0f));
        }

        /// <summary>
        /// Projects a world point to screen coordinates.
        /// </summary>
        /// <returns>2D coordinate on the screen with z as depth (z is not a distance value!)</returns>
        public Vector3 Project(float x, float y, float z)
        {
            return Project(new Vector3(x, y, z));
        }

        /// <summary>
        /// Projects a world point to screen coordinates.
        /// </summary>
        /// <returns>2D coordinate on the screen with z as depth (z is not a distance value!)</returns>
        public Vector3 Project(Vector3 source)
        {
            //This needs to be converted to a Vector4 in order to work
            //Also the order of the matrix multiplication matters
            Vector4 t1 = _matrix * (Vector4) source;
            Vector4 t2 = _projectionMatrix * t1;
            if (t2._w == 0)
            {
                return new Vector3();
            }

            Vector3 v = (Vector3) t2;
            return new Vector3(
                (v._x / 2.0f + 0.5f) * Width,
                Height - (v._y / 2.0f + 0.5f) * Height,
                (v._z + 1.0f) / 2.0f);
        }

        public Vector3 ProjectCameraSphere(Vector2 screenPoint, Vector3 center, float radius, bool clamp)
        {
            //Get ray points
            Vector3 ray1 = UnProject(screenPoint._x, screenPoint._y, 0.0f);
            Vector3 ray2 = UnProject(screenPoint._x, screenPoint._y, 1.0f);

            if (!Maths.LineSphereIntersect(ray1, ray2, center, radius, out Vector3 point))
            {
                //If no intersect is found, project the ray through the plane perpendicular to the camera.
                Maths.LinePlaneIntersect(ray1, ray2, center, GetPoint().Normalize(center), out point);

                //Clamp the point to edge of the sphere
                if (clamp)
                {
                    point = Maths.PointAtLineDistance(center, point, radius);
                }
            }

            return point;
        }

        public void ProjectCameraPlanes(Vector2 screenPoint, Matrix transform, out Vector3 xy, out Vector3 yz,
                                        out Vector3 xz)
        {
            Vector3 ray1 = UnProject(screenPoint._x, screenPoint._y, 0.0f);
            Vector3 ray2 = UnProject(screenPoint._x, screenPoint._y, 1.0f);

            Vector3 center = transform.GetPoint();

            Maths.LinePlaneIntersect(ray1, ray2, center, (transform * Vector3.UnitX).Normalize(center), out yz);
            Maths.LinePlaneIntersect(ray1, ray2, center, (transform * Vector3.UnitY).Normalize(center), out xz);
            Maths.LinePlaneIntersect(ray1, ray2, center, (transform * Vector3.UnitZ).Normalize(center), out xy);
        }

        public void LoadProjection()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            fixed (Matrix* p = &_projectionMatrix)
            {
                GL.LoadMatrix((float*) p);
            }
        }

        public void LoadModelView()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            fixed (Matrix* p = &_matrix)
            {
                GL.LoadMatrix((float*) p);
            }
        }

        public void ZoomExtents(Vector3 point, float distance)
        {
            if (!_ortho)
            {
                _rotation = new Vector3();
            }

            Vector3 position = point + new Vector3(0.0f, 0.0f, distance);
            _matrix = Matrix.ReverseTransformMatrix(_scale, _rotation, position);
            _matrixInverse = Matrix.TransformMatrix(_scale, _rotation, position);
        }

        public void SaveDefaults()
        {
            _defaultTranslate = GetPoint();
            _defaultRotate = _rotation;
            _defaultScale = _scale;
        }
    }
}