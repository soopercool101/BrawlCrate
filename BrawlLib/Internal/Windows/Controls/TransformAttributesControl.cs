using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public partial class TransformAttributesControl : UserControl
    {
        private readonly NumericInputBox[] _boxes;

        public TransformAttributesControl()
        {
            InitializeComponent();

            _boxes = new NumericInputBox[]
            {
                numScaleX, numScaleY, numScaleZ,
                numRotX, numRotY, numRotZ,
                numTransX, numTransY, numTransZ
            };

            foreach (NumericInputBox box in _boxes)
            {
                box.Value = box.Value;
            }
        }

        private bool _twoDimensional;

        public bool TwoDimensional
        {
            get => _twoDimensional;
            set
            {
                _twoDimensional = value;
                numScaleZ.Visible = numTransZ.Visible = numRotX.Visible = numRotY.Visible = !value;
            }
        }

        public float this[int index]
        {
            get => _boxes[index].Value;
            set => _boxes[index].Value = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Vector3 ScaleVector
        {
            get => new Vector3(this[0], this[1], this[2]);
            set
            {
                this[0] = value._x;
                this[1] = value._y;
                this[2] = value._z;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Vector3 RotateVector
        {
            get => new Vector3(this[3], this[4], this[5]);
            set
            {
                this[3] = value._x;
                this[4] = value._y;
                this[5] = value._z;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Vector3 TranslateVector
        {
            get => new Vector3(this[6], this[7], this[8]);
            set
            {
                this[6] = value._x;
                this[7] = value._y;
                this[8] = value._z;
            }
        }

        public Matrix GetMatrix()
        {
            return Matrix.TransformMatrix(ScaleVector, RotateVector, TranslateVector);
        }
    }
}