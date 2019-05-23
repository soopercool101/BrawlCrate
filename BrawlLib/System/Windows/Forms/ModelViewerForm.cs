namespace System.Windows.Forms
{
    public partial class ModelViewerForm : Form
    {
        public ModelEditorBase _mainWindow;
        public ModelViewerForm(ModelEditorBase mainWindow)
        {
            InitializeComponent();
            TopMost = true;
        }
    }
}
