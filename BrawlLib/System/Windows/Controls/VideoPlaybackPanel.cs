using BrawlLib.Imaging;
using System.Audio;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class VideoPlaybackPanel : UserControl
    {
        #region Designer

        private CustomTrackBar trackBar1;
        private Button btnPlay;
        private Button btnRewind;
        private Label lblProgress;
        private System.ComponentModel.IContainer components;
        private PreviewPanel previewPanel1;
        private CheckBox chkLoop;

        private void InitializeComponent()
        {
            this.trackBar1 = new System.Windows.Forms.CustomTrackBar();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnRewind = new System.Windows.Forms.Button();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.previewPanel1 = new System.Windows.Forms.PreviewPanel();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.Location = new System.Drawing.Point(0, 212);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(378, 45);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.TickFrequency = 2;
            this.trackBar1.UserSeek += new System.EventHandler(this.trackBar1_UserSeek);
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPlay.Location = new System.Drawing.Point(152, 263);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(75, 20);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnRewind
            // 
            this.btnRewind.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRewind.Location = new System.Drawing.Point(122, 263);
            this.btnRewind.Name = "btnRewind";
            this.btnRewind.Size = new System.Drawing.Size(24, 20);
            this.btnRewind.TabIndex = 2;
            this.btnRewind.Text = "|<";
            this.btnRewind.UseVisualStyleBackColor = true;
            this.btnRewind.Click += new System.EventHandler(this.btnRewind_Click);
            // 
            // chkLoop
            // 
            this.chkLoop.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.chkLoop.Location = new System.Drawing.Point(54, 263);
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(62, 20);
            this.chkLoop.TabIndex = 3;
            this.chkLoop.Text = "Loop";
            this.chkLoop.UseVisualStyleBackColor = true;
            this.chkLoop.CheckedChanged += new System.EventHandler(this.chkLoop_CheckedChanged);
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblProgress.Location = new System.Drawing.Point(-79, 239);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(536, 23);
            this.lblProgress.TabIndex = 4;
            this.lblProgress.Text = "0/0";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // previewPanel1
            // 
            this.previewPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewPanel1.CurrentIndex = 0;
            this.previewPanel1.DisposeImage = true;
            this.previewPanel1.Location = new System.Drawing.Point(3, 3);
            this.previewPanel1.Name = "previewPanel1";
            this.previewPanel1.RenderingTarget = null;
            this.previewPanel1.Size = new System.Drawing.Size(372, 203);
            this.previewPanel1.TabIndex = 5;
            // 
            // VideoPlaybackPanel
            // 
            this.Controls.Add(this.previewPanel1);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.chkLoop);
            this.Controls.Add(this.btnRewind);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.trackBar1);
            this.Name = "VideoPlaybackPanel";
            this.Size = new System.Drawing.Size(378, 289);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        private bool _loop = false;
        private bool _isPlaying = false;
        //private bool _isScrolling = false;

        private DateTime _frameTime;
        CoolTimer _timer;

        private IVideo _targetSource;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IVideo TargetSource
        {
            get { return _targetSource; }
            set { TargetChanged(value); }
        }

        private AudioProvider _provider;
        private AudioBuffer _buffer;

        public VideoPlaybackPanel() 
        {
            InitializeComponent();

            _timer = new CoolTimer();
            _timer.RenderFrame += new EventHandler<FrameEventArgs>(RenderUpdate);

            previewPanel1.LeftClicked += previewPanel1_LeftClicked;
            previewPanel1.RightClicked += previewPanel1_RightClicked;
        }

        void previewPanel1_RightClicked(object sender, EventArgs e)
        {
            Seek(_frame + 1);
        }

        void previewPanel1_LeftClicked(object sender, EventArgs e)
        {
            Seek(_frame - 1);
        }

        public void RenderUpdate(object sender, FrameEventArgs e)
        {
            if ((_isPlaying))
            {
                //TODO: Sync video to audio
                if (_buffer != null)
                    _buffer.Fill();

                trackBar1.Value = ++_frame;
                
                if (_frame >= _targetSource.NumFrames)
                    if (!_loop)
                        Stop();
                    else
                        _frame = 0;
            }
        }

        protected override void Dispose(bool disposing)
        {
            Close();
            if (_provider != null)
            {
                _provider.Dispose();
                _provider = null;
            }
            base.Dispose(disposing);
        }

        private void Close()
        {
            //Stop playback
            Stop();
            
            _targetSource = null;

            //Reset fields
            chkLoop.Checked = false;
        }

        private void TargetChanged(IVideo newTarget)
        {
            if (_targetSource == newTarget)
                return;

            Close();

            if ((_targetSource = newTarget) == null)
                return;

            previewPanel1.RenderingTarget = _targetSource;

            IAudioStream s = _targetSource.Audio;

            //Create provider
            if (_provider == null && s != null)
            {
                _provider = AudioProvider.Create(null);
                _provider.Attach(this);
            }

            chkLoop.Checked = false;

            //Create buffer for stream
            if (s != null)
                _buffer = _provider.CreateBuffer(s);

            if (_targetSource.FrameRate > 0)
                _frameTime = new DateTime((long)((float)_targetSource.NumFrames * 10000000.0f / _targetSource.FrameRate));
            
            trackBar1.TickStyle = TickStyle.None;
            trackBar1.Maximum = (int)_targetSource.NumFrames;
            trackBar1.Minimum = 1;
            trackBar1.Value = 1;

            if (_targetSource.FrameRate > 0)
                UpdateTimeDisplay();

            Enabled = _targetSource.NumFrames > 0;
        }

        private void UpdateTimeDisplay()
        {
            if (_targetSource == null) return;
            _frame = trackBar1.Value - 1;
            DateTime t = new DateTime((long)((float)(trackBar1.Value - 1) * 10000000.0f / _targetSource.FrameRate));
            lblProgress.Text = String.Format("{0:mm:ss.ff} / {1:mm:ss.ff} - Frame {2} of {3}", t, _frameTime, _frame + 1, TargetSource.NumFrames);

            previewPanel1.CurrentIndex = _targetSource.GetImageIndexAtFrame(_frame);
        }

        private void Seek(int frame)
        {
            bool temp = false;
            if (_isPlaying)
            {
                temp = true;
                Stop();
            }

            _frame = frame.Clamp(0, (int)_targetSource.NumFrames - 1);
            trackBar1.Value = _frame + 1;

            if (_buffer != null)
                _buffer.Seek((int)Math.Round(frame / _targetSource.FrameRate * _targetSource.Frequency, 0));

            if (temp)
                Play();
        }

        private void Play()
        {
            if (_targetSource == null)
                return;

            if ((_isPlaying))
                return;

            _isPlaying = true;

            //Start from beginning if at end
            if (trackBar1.Value == _targetSource.NumFrames)
                trackBar1.Value = 1;

            btnPlay.Text = "Stop";
            previewPanel1.btnLeft.Visible = previewPanel1.btnRight.Visible = false;

            if (_buffer != null)
            {
                //Seek buffer to current sample
                _buffer.Seek((int)Math.Round((trackBar1.Value - 1) / _targetSource.FrameRate * _targetSource.Frequency, 0));

                //Fill initial buffer
                _buffer.Fill();

                //Begin playback
                _buffer.Play();
            }

            _timer.Run(0, _targetSource.FrameRate);
        }
        private void Stop()
        {
            if (!_isPlaying)
                return;

            _isPlaying = false;

            //Stop timer
            _timer.Stop();

            //Stop device
            if (_buffer != null)
                _buffer.Stop();

            btnPlay.Text = "Play";
            previewPanel1.btnLeft.Visible = previewPanel1.btnRight.Visible = true;
        }
        
        int _frame = 0;
        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (_isPlaying)
                Stop();
            else
                Play();
        }

        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
            _loop = chkLoop.Checked;
            if (_buffer != null)
                _buffer.Loop = _loop;
        }

        private void btnRewind_Click(object sender, EventArgs e) { Seek(0); }
        private void trackBar1_ValueChanged(object sender, EventArgs e) { UpdateTimeDisplay(); }
        private void trackBar1_UserSeek(object sender, EventArgs e) { Seek(trackBar1.Value - 1); }
    }
}
