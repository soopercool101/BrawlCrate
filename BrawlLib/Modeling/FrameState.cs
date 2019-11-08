using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Animations;
using System.Runtime.InteropServices;

namespace BrawlLib.Modeling
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FrameState
    {
        //These three variables MUST be at the start of the struct!!!
        public Vector3 _scale;
        public Vector3 _rotate;
        public Vector3 _translate;

        public Matrix _transform, _iTransform;

        public unsafe float this[int index]
        {
            get
            {
                fixed (FrameState* f = &this)
                {
                    return ((float*) f)[index];
                }
            }
            set
            {
                fixed (FrameState* f = &this)
                {
                    ((float*) f)[index] = value;
                }
            }
        }

        public Vector3 Translate
        {
            get => _translate;
            set
            {
                _translate = value;
                CalcTransforms();
            }
        }

        public Vector3 Rotate
        {
            get => _rotate;
            set
            {
                _rotate = value;
                CalcTransforms();
            }
        }

        public Vector3 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                CalcTransforms();
            }
        }

        public FrameState(CHRAnimationFrame frame)
        {
            _scale = frame.Scale;
            _rotate = frame.Rotation;
            _translate = frame.Translation;

            CalcTransforms();
        }

        public FrameState(Vector3 scale, Vector3 rotation, Vector3 translation)
        {
            _scale = scale;
            _rotate = rotation;
            _translate = translation;

            CalcTransforms();
        }

        public void CalcTransforms()
        {
            _transform = Matrix.TransformMatrix(_scale, _rotate, _translate);
            _iTransform = Matrix.ReverseTransformMatrix(_scale, _rotate, _translate);
        }

        public override string ToString()
        {
            return $"{_scale.ToString()}{_rotate.ToString()}{_translate.ToString()}";
        }

        public static readonly FrameState Neutral = new FrameState(new Vector3(1.0f), new Vector3(), new Vector3());

        public static explicit operator CHRAnimationFrame(FrameState state)
        {
            return new CHRAnimationFrame(state._scale, state._rotate, state._translate);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TextureFrameState
    {
        //These three variables MUST be at the start of the struct!!!
        private Vector2 _scale;
        private float _rotate;
        private Vector2 _translate;

        public Matrix _transform;
        private TexMatrixMode _matrixMode;
        private int _flags;
        private bool _indirect;

        public unsafe float this[int index]
        {
            get
            {
                fixed (TextureFrameState* f = &this)
                {
                    return ((float*) f)[index];
                }
            }
            set
            {
                fixed (TextureFrameState* f = &this)
                {
                    ((float*) f)[index] = value;
                }
            }
        }

        public Vector2 Translate
        {
            get => _translate;
            set
            {
                _translate = value;
                CalcTransforms();
            }
        }

        public float Rotate
        {
            get => _rotate;
            set
            {
                _rotate = value;
                CalcTransforms();
            }
        }

        public Vector2 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                CalcTransforms();
            }
        }

        /// <summary>
        /// Bit flags used for texture matrix calculation
        /// </summary>
        public int Flags => _flags;

        public TexMatrixMode MatrixMode
        {
            get => _matrixMode;
            set
            {
                _matrixMode = value;
                CalcTransforms();
            }
        }

        public bool Indirect
        {
            get => _indirect;
            set
            {
                _indirect = value;
                CalcTransforms();
            }
        }

        public TextureFrameState(SRTAnimationFrame frame, TexMatrixMode matrixMode, bool indirect)
        {
            _scale = frame.Scale;
            _rotate = frame.Rotation;
            _translate = frame.Translation;
            _matrixMode = matrixMode;
            _flags = 0;
            _indirect = indirect;

            CalcTransforms();
        }

        public TextureFrameState(Vector2 scale, float rotation, Vector2 translation, TexMatrixMode matrixMode,
                                 bool indirect)
        {
            _scale = scale;
            _rotate = rotation;
            _translate = translation;
            _matrixMode = matrixMode;
            _flags = 0;
            _indirect = indirect;

            CalcTransforms();
        }

        private void CalcFlags()
        {
            _flags = 0;
            if (Scale == new Vector2(1))
            {
                _flags |= 1;
            }

            if (Rotate == 0)
            {
                _flags |= 2;
            }

            if (Translate == new Vector2(0))
            {
                _flags |= 4;
            }
        }

        public void CalcTransforms()
        {
            CalcFlags();
            _transform = (Matrix) Matrix34.TextureMatrix(this);
        }

        public override string ToString()
        {
            return $"{_scale.ToString()}, {_rotate.ToString()}, {_translate.ToString()}";
        }

        public static readonly TextureFrameState Neutral =
            new TextureFrameState(new Vector2(1.0f), 0, new Vector2(), TexMatrixMode.MatrixMaya, false);

        public static explicit operator SRTAnimationFrame(TextureFrameState state)
        {
            return new SRTAnimationFrame(state._scale, state._rotate, state._translate);
        }
    }
}