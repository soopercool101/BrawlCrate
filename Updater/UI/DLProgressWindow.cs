using System.ComponentModel;
using System.Threading;
using System.Net;

namespace System.Windows.Forms
{
    public partial class DLProgressWindow : Form, DLProgressTracker
    {
        private bool _canCancel = false, _cancelled = false;
        public bool CanCancel { get { return _canCancel; } set { btnCancel.Visible = btnCancel.Enabled = _canCancel = value; } }
        public string Caption { get { return label1.Text; } set { label1.Text = value; } }
        public static bool started = false;
        public static bool finished = false;
        public string Version;
        public DLProgressWindow() { InitializeComponent(); }
        //private Control controlOwner;
        public DLProgressWindow(Control owner, string version, string appPath, string dlLink) : this()
        {
            //controlOwner = owner;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            Version = version;
            Text = "Downloading Update";
            Caption = "Downloading " + Version + ": ";
            CanCancel = false;
            started = false;
            finished = false;
            MinValue = 0;
            progressBar1.MinValue = 0;
            CurrentValue = 0;
            MaxValue = 1;
            //MessageBox.Show(version + '\n' + appPath + '\n' + dlLink);
            startDownload(appPath, dlLink);
            Thread.Sleep(50);
            Update(0);
            Show();
            Focus();
            while (!finished)
                Update(0);
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
            progressBar1.CurrentValue = CurrentValue;
            progressBar1.MaxValue = MaxValue;
            if (!Caption.Equals("Download Completed"))
                Caption = "Downloading " + Version + ": " + (CurrentValue / 1048576.0).ToString("0.##") + "MB of " + (MaxValue / 1048576.0).ToString("0.##") + "MB";
            Application.DoEvents();
            Thread.Sleep(0);
        }
        public void Finish()
        {
            if (Owner != null)
                Owner.Enabled = true;
            
        }
        private void startDownload(string AppPath, string dlLink)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("User-Agent: Other");
                        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                        client.DownloadFileAsync(new Uri(dlLink), (AppPath + "\\temp.exe"));
                        Application.DoEvents();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERROR: " + e);
                }
            });
            thread.Start();
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                if (MaxValue == 1)
                {
                    MaxValue = float.Parse(e.TotalBytesToReceive.ToString());
                    started = true;
                }
                CurrentValue = float.Parse(e.BytesReceived.ToString());
            });
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                Caption = "Download Completed";
                Thread.Sleep(10);
                finished = true;
            });
        }
        public void Cancel() { _cancelled = true; }
        public static float MinValue = 0;
        public static float MaxValue = 1;
        public static float CurrentValue = 0;
        public bool Cancelled { get { return _cancelled; } set { _cancelled = true; } }
    }
}
