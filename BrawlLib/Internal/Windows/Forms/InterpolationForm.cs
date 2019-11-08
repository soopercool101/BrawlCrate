using BrawlLib.Internal.Windows.Controls;
using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using System;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
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