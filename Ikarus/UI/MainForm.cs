using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace Ikarus.UI
{
    public partial class MainForm : Form
    {
        private static MainForm _instance;
        public static MainForm Instance { get { return _instance == null ? _instance = new MainForm(null) : _instance; } }
        
        public static void UpdateModelPanel() { Instance._mainControl.ModelPanel.Invalidate(); }

        SplashForm _splashForm;
        public MainForm(SplashForm s)
        {
            _splashForm = s;
            InitializeComponent();
            Text = Program.AssemblyTitle;
            _instance = this;
            Load += MainForm_Load;
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            if (_splashForm != null)
            {
                _splashForm.Close();
                _splashForm.Dispose();
            }
            WindowState = FormWindowState.Maximized;
        }

        private delegate bool DelegateOpenFile(String s);
        private DelegateOpenFile m_DelegateOpenFile;

        public void Reset()
        {
            UpdateName();
        }

        public void UpdateName()
        {
            Text = Program.AssemblyTitle;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!Program.Close()) 
                e.Cancel = true;

            base.OnClosing(e);
        }

        #region File Menu
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) { Close(); }
        #endregion

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) { AboutForm.Instance.ShowDialog(this); }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
            if (a != null)
            {
                string s = null;
                for (int i = 0; i < a.Length; i++)
                {
                    s = a.GetValue(i).ToString();
                    this.BeginInvoke(m_DelegateOpenFile, new Object[] { s });
                }
            }
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=3T2HNHK5BM8LL&lc=US&item_name=Brawlbox&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donate_LG%2egif%3aNonHosted");
        }
    }
}
