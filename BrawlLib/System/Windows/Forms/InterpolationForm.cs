namespace System.Windows.Forms
{
    public partial class InterpolationForm : Form
    {
        public InterpolationEditor _interpolationEditor;
        public InterpolationForm(ModelEditorBase mainWindow)
        {
            InitializeComponent();
            _interpolationEditor = new InterpolationEditor(mainWindow);
            TopMost = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Controls.Add(_interpolationEditor);
            _interpolationEditor.Dock = DockStyle.Fill;
        }
    }
}
