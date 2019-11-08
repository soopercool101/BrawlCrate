using System;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Updater.UI
{
    public partial class DLProgressWindow : Form
    {
        public static bool started;
        public static bool finished;

        public static long MinValue;
        public static long MaxValue = 1;
        public static long CurrentValue;
        private bool _canCancel, _cancelled;
        public string PackageName;

        public DLProgressWindow()
        {
            InitializeComponent();
        }

        //private Control controlOwner;
        public DLProgressWindow(string packageName, string appPath, string dlLink) : this()
        {
            //controlOwner = owner;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            PackageName = packageName;
            Text = "Downloading Update";
            Caption = "Downloading " + PackageName + ": ";
            CanCancel = false;
            started = false;
            finished = false;
            MinValue = 0;
            progressBar1.MinValue = 0;
            CurrentValue = 0;
            MaxValue = 1;
            //MessageBox.Show(version + '\n' + appPath + '\n' + dlLink);
            startDownload(dlLink, $"{appPath}\\temp.exe");
            Thread.Sleep(50);
            UpdateProgress();
            Show();
            Focus();
            while (!finished)
            {
                UpdateProgress();
            }
        }

        public DLProgressWindow(string packageName, string appPath, string dlLink, string downloadLocation) : this()
        {
            //controlOwner = owner;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            PackageName = packageName;
            Text = "Downloading Update";
            Caption = "Downloading " + PackageName + ": ";
            CanCancel = false;
            started = false;
            finished = false;
            MinValue = 0;
            progressBar1.MinValue = 0;
            CurrentValue = 0;
            MaxValue = 1;
            //MessageBox.Show(version + '\n' + appPath + '\n' + dlLink);
            startDownload(dlLink, downloadLocation);
            Thread.Sleep(50);
            UpdateProgress();
            Show();
            Focus();
            while (!finished)
            {
                UpdateProgress();
            }
        }

        public bool CanCancel
        {
            get => _canCancel;
            set => btnCancel.Visible = btnCancel.Enabled = _canCancel = value;
        }

        public string Caption
        {
            get => label1.Text;
            set => label1.Text = value;
        }

        public bool Cancelled
        {
            get => _cancelled;
            set => _cancelled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        public void Begin(long min, long max, float current)
        {
            progressBar1.MinValue = min;
            progressBar1.MaxValue = max;
            progressBar1.CurrentValue = current;

            if (Owner != null)
            {
                if (Owner.InvokeRequired)
                {
                    Invoke(new MethodInvoker(() => Owner.Enabled = false));
                }
                else
                {
                    Owner.Enabled = false;
                }
            }

            Show();

            if (Owner != null)
            {
                CenterToParent();
            }

            Application.DoEvents();
        }

        public void UpdateProgress()
        {
            progressBar1.CurrentValue = CurrentValue;
            progressBar1.MaxValue = MaxValue;
            if (!Caption.Equals("Download Completed"))
            {
                Caption = "Downloading " + PackageName + ": " + (CurrentValue / 1048576.0).ToString("0.##") + "MB of " +
                          (MaxValue / 1048576.0).ToString("0.##") + "MB";
            }

            Application.DoEvents();
            Thread.Sleep(0);
        }

        public void Finish()
        {
            if (Owner != null)
            {
                Owner.Enabled = true;
            }
        }

        private void startDownload(string dlLink, string downloadLocation)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("User-Agent: Other");
                        client.DownloadProgressChanged +=
                            client_DownloadProgressChanged;
                        client.DownloadFileCompleted += client_DownloadFileCompleted;
                        client.DownloadFileAsync(new Uri(dlLink), downloadLocation);
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

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            BeginInvoke((MethodInvoker) delegate
            {
                if (MaxValue == 1)
                {
                    MaxValue = e.TotalBytesToReceive;
                    started = true;
                }

                CurrentValue = e.BytesReceived;
            });
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            BeginInvoke((MethodInvoker) delegate
            {
                Caption = "Download Completed";
                Thread.Sleep(10);
                finished = true;
            });
        }

        public void Cancel()
        {
            _cancelled = true;
        }
    }
}