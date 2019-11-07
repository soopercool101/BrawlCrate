using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
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