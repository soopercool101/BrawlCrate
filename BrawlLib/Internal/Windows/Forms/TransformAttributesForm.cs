using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    public partial class TransformAttributesForm : Form
    {
        public TransformAttributesForm()
        {
            InitializeComponent();
        }

        public bool TwoDimensional
        {
            get => transformAttributesControl1.TwoDimensional;
            set => transformAttributesControl1.TwoDimensional = value;
        }

        public float this[int index]
        {
            get => transformAttributesControl1[index];
            set => transformAttributesControl1[index] = value;
        }

        public Vector3 ScaleVector
        {
            get => transformAttributesControl1.ScaleVector;
            set => transformAttributesControl1.ScaleVector = value;
        }

        public Vector3 RotateVector
        {
            get => transformAttributesControl1.RotateVector;
            set => transformAttributesControl1.RotateVector = value;
        }

        public Vector3 TranslateVector
        {
            get => transformAttributesControl1.TranslateVector;
            set => transformAttributesControl1.TranslateVector = value;
        }

        public Matrix GetMatrix()
        {
            return transformAttributesControl1.GetMatrix();
        }
    }
}