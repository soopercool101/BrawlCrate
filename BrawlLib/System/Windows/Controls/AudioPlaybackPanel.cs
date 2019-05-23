using System.Audio;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class AudioPlaybackPanel : UserControl
    {
        #region Designer

        private CustomTrackBar trackBarPosition;
        private Button btnPlay;
        private Button btnRewind;
        private Label lblProgress;
        private Timer tmrUpdate;
        private System.ComponentModel.IContainer components;
        private ComboBox lstStreams;
        private CustomTrackBar trackBarVolume;
        private Panel panel2;
        private CheckBox chkLoop;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnRewind = new System.Windows.Forms.Button();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.lstStreams = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.trackBarVolume = new System.Windows.Forms.CustomTrackBar();
            this.trackBarPosition = new System.Windows.Forms.CustomTrackBar();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosition)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(35, 4);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(73, 20);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnRewind
            // 
            this.btnRewind.Location = new System.Drawing.Point(5, 4);
            this.btnRewind.Name = "btnRewind";
            this.btnRewind.Size = new System.Drawing.Size(24, 20);
            this.btnRewind.TabIndex = 2;
            this.btnRewind.Text = "|<";
            this.btnRewind.UseVisualStyleBackColor = true;
            this.btnRewind.Click += new System.EventHandler(this.btnRewind_Click);
            // 
            // chkLoop
            // 
            this.chkLoop.Location = new System.Drawing.Point(114, 5);
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(50, 20);
            this.chkLoop.TabIndex = 3;
            this.chkLoop.Text = "Loop";
            this.chkLoop.UseVisualStyleBackColor = true;
            this.chkLoop.CheckedChanged += new System.EventHandler(this.chkLoop_CheckedChanged);
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.Location = new System.Drawing.Point(0, 31);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(377, 23);
            this.lblProgress.TabIndex = 4;
            this.lblProgress.Text = "0/0";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 10;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // lstStreams
            // 
            this.lstStreams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lstStreams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstStreams.FormattingEnabled = true;
            this.lstStreams.Location = new System.Drawing.Point(219, 34);
            this.lstStreams.Name = "lstStreams";
            this.lstStreams.Size = new System.Drawing.Size(73, 21);
            this.lstStreams.TabIndex = 5;
            this.lstStreams.SelectedIndexChanged += new System.EventHandler(this.lstStreams_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel2.Controls.Add(this.btnPlay);
            this.panel2.Controls.Add(this.btnRewind);
            this.panel2.Controls.Add(this.chkLoop);
            this.panel2.Location = new System.Drawing.Point(116, 61);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(167, 30);
            this.panel2.TabIndex = 9;
            // 
            // trackBarVolume
            // 
            this.trackBarVolume.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarVolume.Location = new System.Drawing.Point(329, 31);
            this.trackBarVolume.Maximum = 100;
            this.trackBarVolume.Name = "trackBarVolume";
            this.trackBarVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarVolume.Size = new System.Drawing.Size(45, 89);
            this.trackBarVolume.TabIndex = 6;
            this.trackBarVolume.TickFrequency = 25;
            this.trackBarVolume.Value = 100;
            this.trackBarVolume.ValueChanged += new System.EventHandler(this.customTrackBar1_ValueChanged);
            // 
            // trackBarPosition
            // 
            this.trackBarPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarPosition.Location = new System.Drawing.Point(0, 4);
            this.trackBarPosition.Name = "trackBarPosition";
            this.trackBarPosition.Size = new System.Drawing.Size(377, 45);
            this.trackBarPosition.TabIndex = 0;
            this.trackBarPosition.TickFrequency = 2;
            this.trackBarPosition.UserSeek += new System.EventHandler(this.trackBar1_UserSeek);
            this.trackBarPosition.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // AudioPlaybackPanel
            // 
            this.Controls.Add(this.lstStreams);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.trackBarVolume);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.trackBarPosition);
            this.Name = "AudioPlaybackPanel";
            this.Size = new System.Drawing.Size(377, 120);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosition)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private bool _updating = false;
        private bool _loop = false;
        private bool _isPlaying = false;
        //private bool _isScrolling = false;

        private DateTime _sampleTime;
        private IAudioStream[] _targetStreams;

        private IAudioStream _targetStream 
        {
            get 
            {
                if (_targetIndex < 0)
                    return null;

                return _targetStreams[_targetIndex]; 
            } 
        }

        public IAudioStream[] TargetStreams
        {
            get { return _targetStreams; }
            set
            {
                if (value == _targetStreams)
                    return;

                lstStreams.Items.Clear(); 
                if (_targetStreams != null)
                    for (int x = 0; x < _targetStreams.Length; x++)
                    {
                        _targetStreams[x].Dispose();
                        _targetStreams[x] = null;
                    }

                if ((_targetStreams = value) != null)
                {
                    _buffers = new AudioBuffer[_targetStreams.Length];
                    if (lstStreams.Visible = _targetStreams.Length > 1)
                    {
                        int i = 1;
                        foreach (IAudioStream s in _targetStreams)
                            lstStreams.Items.Add("Stream" + i++);
                    }
                    else
                        lstStreams.Items.Add("");
                }
                else
                {
                    lstStreams.Visible = false;
                    lstStreams.Items.Add("");
                }
                _updating = true;
                lstStreams.SelectedIndex = _targetIndex = 0;
                _updating = false;
            }
        }

        private IAudioSource _targetSource;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IAudioSource TargetSource
        {
            get { return _targetSource; }
            set { TargetChanged(value); }
        }

        private int? _volume;
        /// <summary>
        /// How much quieter to make the audio output, in hundredths of a decibel.
        /// Custom Song Volume code's difference between 7F (max) and 3F seems to be 1/4 volume - about -6dB. In other words, both the maximum and minimum amplitudes are halved.
        /// See: http://msdn.microsoft.com/en-us/library/windows/desktop/bb280955%28v=vs.85%29.aspx
        /// </summary>
        public int? Volume {
            get {
                return _volume;
            }
            set {
                _volume = value;
                if (CurrentBuffer != null && _volume != null) {
                    CurrentBuffer.Volume = _volume.Value;
                }
            }
        }

        /// <summary>
        /// Sets Volume appropriately.
        /// Range: above .00001, below or at 1. Anything lower than .00001 will set Volume to -10000.
        /// </summary>
        public double? VolumePercent {
            set {
                if (value == null) {
                    Volume = null;
                } else {
                    Volume = Math.Max(-10000, (int)(Math.Log10(value.Value) * 2000));
                }

                //This should really be in the volume property, but this way I don't need to do more calculations
                BrawlLib.Properties.Settings.Default.AudioVolumePercent = value;
                BrawlLib.Properties.Settings.Default.Save();

                if (!_updating)
                    trackBarVolume.Value = (int)(value * 100d + 0.5d);
            }
        }

        private AudioProvider _provider;

        private AudioBuffer[] _buffers;
        private AudioBuffer CurrentBuffer 
        {
            get
            {
                if (_targetIndex < 0 || _buffers == null)
                    return null;

                return _buffers[_targetIndex]; 
            }
        }

        public event EventHandler AudioEnded;

        public AudioPlaybackPanel()
        {
            InitializeComponent();
            VolumePercent = BrawlLib.Properties.Settings.Default.AudioVolumePercent;

            chkLoop.CheckedChanged += (o, e) =>
            {
                if (chkLoop.Checked && _provider is alAudioProvider)
                {
                    chkLoop.Checked = false;
                    MessageBox.Show("Looping audio with the OpenAL audio backend is not currently supported.");
                }
            };
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

            //Dispose of buffer
            if (_buffers != null)
                for (int i = 0; i < _buffers.Length; i++)
                    if (_buffers[i] != null)
                    {
                        _buffers[i].Dispose();
                        _buffers[i] = null;
                    }

            if (_targetStreams != null)
            {
                for (int i = 0; i < _targetStreams.Length; i++)
                    if (_targetStreams[i] != null)
                    {
                        _targetStreams[i].Dispose();
                        _targetStreams[i] = null;
                    }
                _targetStreams = null;
            }

            _targetSource = null;

            //Reset fields
            chkLoop.Checked = false;
        }

        private void TargetChanged(IAudioSource newTarget)
        {
            if (_targetSource == newTarget)
                return;
            
            Close();
            
            if ((_targetSource = newTarget) == null)
                return;
            if ((TargetStreams = _targetSource.CreateStreams()) == null)
                return;
            if (_targetStream == null)
                return;
            
            //Create provider
            if (_provider == null)
            {
                _provider = AudioProvider.Create(null);
                if (_provider != null)
                    _provider.Attach(this);
            }
            
            chkLoop.Checked = false;
            
            //Create buffer for stream
            if (_provider != null)
                for (int i = 0; i < _buffers.Length; i++)
                    _buffers[i] = _provider.CreateBuffer(_targetStreams[i]);
            
            if (_targetStream.Frequency > 0)
                _sampleTime = new DateTime((long)_targetStream.Samples * 10000000 / _targetStream.Frequency);
            
            trackBarPosition.Value = 0;
            trackBarPosition.TickStyle = TickStyle.None;
            trackBarPosition.Maximum = _targetStream.Samples;
            trackBarPosition.TickFrequency = _targetStream.Samples / 8;
            trackBarPosition.TickStyle = TickStyle.BottomRight;
            
            if (_targetStream.Frequency > 0)
                UpdateTimeDisplay();
            
            Enabled = _targetStream.Samples > 0;
        }

        private void UpdateTimeDisplay()
        {
            if (_targetStream == null) return;
            DateTime t = new DateTime((long)trackBarPosition.Value * 10000000 / _targetStream.Frequency);
            lblProgress.Text = String.Format("{0:mm:ss.ff} / {1:mm:ss.ff}", t, _sampleTime);
        }

        private void Seek(int sample)
        {
            trackBarPosition.Value = sample;

            //Only seek the buffer when playing.
            if (_isPlaying)
            {
                Stop();
                if (CurrentBuffer != null)
                    CurrentBuffer.Seek(sample);
                Play();
            }
        }

        public void Play()
        {
            if ((_isPlaying) || (CurrentBuffer == null))
                return;

            _isPlaying = true;

            //Start from beginning if at end
            if (trackBarPosition.Value == _targetStream.Samples)
                trackBarPosition.Value = 0;

            //Seek buffer to current sample
            CurrentBuffer.Seek(trackBarPosition.Value);

            //Fill initial buffer
            tmrUpdate_Tick(null, null);
            //Start timer
            tmrUpdate.Start();

            //Change volume
            if (Volume != null) CurrentBuffer.Volume = Volume.Value;
            //Begin playback
            CurrentBuffer.Play();

            btnPlay.Text = "Stop";
        }
        private void Stop()
        {
            if (!_isPlaying)
                return;

            _isPlaying = false;

            //Stop timer
            tmrUpdate.Stop();

            //Stop device
            if (CurrentBuffer != null)
                CurrentBuffer.Stop();

            btnPlay.Text = "Play";
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            if ((_isPlaying) && (CurrentBuffer != null))
            {
                CurrentBuffer.Fill();

                trackBarPosition.Value = CurrentBuffer.ReadSample;

                if (!_loop)
                {
                    if (CurrentBuffer.ReadSample >= _targetStream.Samples)
                    {
                        Stop();
                        AudioEnded?.Invoke(this, new EventArgs());
                    }
                }
            }
        }

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
            if (CurrentBuffer != null)
                CurrentBuffer.Loop = _loop;
        }

        private void btnRewind_Click(object sender, EventArgs e) { Seek(0); }
        private void trackBar1_ValueChanged(object sender, EventArgs e) { UpdateTimeDisplay(); }
        private void trackBar1_UserSeek(object sender, EventArgs e) { Seek(trackBarPosition.Value); }

        int _targetIndex = 0;
        private void lstStreams_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            bool temp = _isPlaying;

            if (temp)
                Stop();

            _targetIndex = lstStreams.SelectedIndex;

            if (CurrentBuffer != null)
            {
                CurrentBuffer.Fill();
                CurrentBuffer.Loop = _loop;
            }

            if (temp)
                Play();
        }

        private void customTrackBar1_ValueChanged(object sender, EventArgs e)
        {
            _updating = true;
            VolumePercent = (double)trackBarVolume.Value / 100.0d;
            _updating = false;
        }
    }
}
