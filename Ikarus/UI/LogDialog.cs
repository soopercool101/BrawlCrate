using Ikarus.ModelViewer;

namespace System.Windows.Forms
{
    public partial class LogDialog : Form
    {
        public LogDialog()
        {
            InitializeComponent();
            listBox1.DataSource = RunTime._log;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RunTime.ClearLog();
        }
    }
}
