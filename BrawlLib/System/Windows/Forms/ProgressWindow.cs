using System.Threading;

namespace System.Windows.Forms
{
    public partial class ProgressWindow : Form, IProgressTracker
    {
        private bool _canCancel = false, _cancelled = false;
        public bool CanCancel { get { return _canCancel; } set { btnCancel.Enabled = _canCancel = value; } }
        public string Caption { get { return label1.Text; } set { label1.Text = value; } }

        public ProgressWindow() { InitializeComponent(); }
        public ProgressWindow(Form owner, string title, string caption, bool canCancel) : this()
        {
            Owner = owner;
            Text = title; 
            Caption = caption;
            CanCancel = canCancel; 
        }
        private Control controlOwner;
        public ProgressWindow(Control owner, string title, string caption, bool canCancel) : this()
        {
            controlOwner = owner;
            Text = title;
            Caption = caption;
            CanCancel = canCancel;
        }

        private void btnCancel_Click(object sender, EventArgs e) { Cancel(); }

        public void Begin(float min, float max, float current)
        {
            progressBar1.MinValue = min;
            progressBar1.MaxValue = max;
            progressBar1.CurrentValue = current;

            if (Owner != null)
                if (Owner.InvokeRequired)
                    Invoke(new MethodInvoker(() => Owner.Enabled = false));
                else
                    Owner.Enabled = false;

            Show();

            if (Owner != null)
                CenterToParent();

            Application.DoEvents();
        }
        public void Update(float value)
        {
            progressBar1.CurrentValue = value;
            Application.DoEvents();
            Thread.Sleep(0);
        }
        public void Finish()
        {
            if (Owner != null)
                Owner.Enabled = true;

            Close();
        }
        public void Cancel() { _cancelled = true; }
        public float MinValue { get { return progressBar1.MinValue; } set { progressBar1.MinValue = value; } }
        public float MaxValue { get { return progressBar1.MaxValue; } set { progressBar1.MaxValue = value; } }
        public float CurrentValue { get { return progressBar1.CurrentValue; } set { progressBar1.CurrentValue = value; } }
        public bool Cancelled { get { return _cancelled; } set { _cancelled = true; } }
    }
}
