using BrawlLib.Internal.Audio;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public class AudioPlaybackPanel : UserControl
    {
        #region Designer

        private CustomTrackBar trackBarPosition;
        private Button btnPlay;
        private Button btnRewind;
        private Label lblProgress;
        private Timer tmrUpdate;
        private IContainer components;
        private ComboBox lstStreams;
        private CustomTrackBar trackBarVolume;
        private Panel panel2;
        public CheckBox chkLoop;

        private void InitializeComponent()
        {
            components = new Container();
            btnPlay = new Button();
            btnRewind = new Button();
            chkLoop = new CheckBox();
            lblProgress = new Label();
            tmrUpdate = new Timer(components);
            lstStreams = new ComboBox();
            panel2 = new Panel();
            trackBarVolume = new CustomTrackBar();
            trackBarPosition = new CustomTrackBar();
            panel2.SuspendLayout();
            ((ISupportInitialize) trackBarVolume).BeginInit();
            ((ISupportInitialize) trackBarPosition).BeginInit();
            SuspendLayout();
            // 
            // btnPlay
            // 
            btnPlay.Location = new System.Drawing.Point(35, 4);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new System.Drawing.Size(73, 20);
            btnPlay.TabIndex = 1;
            btnPlay.Text = "Play";
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += new EventHandler(btnPlay_Click);
            // 
            // btnRewind
            // 
            btnRewind.Location = new System.Drawing.Point(5, 4);
            btnRewind.Name = "btnRewind";
            btnRewind.Size = new System.Drawing.Size(24, 20);
            btnRewind.TabIndex = 2;
            btnRewind.Text = "|<";
            btnRewind.UseVisualStyleBackColor = true;
            btnRewind.Click += new EventHandler(btnRewind_Click);
            // 
            // chkLoop
            // 
            chkLoop.Location = new System.Drawing.Point(114, 5);
            chkLoop.Name = "chkLoop";
            chkLoop.Size = new System.Drawing.Size(50, 20);
            chkLoop.TabIndex = 3;
            chkLoop.Text = "Loop";
            chkLoop.UseVisualStyleBackColor = true;
            chkLoop.CheckedChanged += new EventHandler(chkLoop_CheckedChanged);
            // 
            // lblProgress
            // 
            lblProgress.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                  | AnchorStyles.Right;
            lblProgress.Location = new System.Drawing.Point(0, 31);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new System.Drawing.Size(377, 23);
            lblProgress.TabIndex = 4;
            lblProgress.Text = "0/0";
            lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrUpdate
            // 
            tmrUpdate.Interval = 10;
            tmrUpdate.Tick += new EventHandler(tmrUpdate_Tick);
            // 
            // lstStreams
            // 
            lstStreams.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lstStreams.DropDownStyle = ComboBoxStyle.DropDownList;
            lstStreams.FormattingEnabled = true;
            lstStreams.Location = new System.Drawing.Point(219, 34);
            lstStreams.Name = "lstStreams";
            lstStreams.Size = new System.Drawing.Size(73, 21);
            lstStreams.TabIndex = 5;
            lstStreams.SelectedIndexChanged += new EventHandler(lstStreams_SelectedIndexChanged);
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top;
            panel2.Controls.Add(btnPlay);
            panel2.Controls.Add(btnRewind);
            panel2.Controls.Add(chkLoop);
            panel2.Location = new System.Drawing.Point(116, 61);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(167, 30);
            panel2.TabIndex = 9;
            // 
            // trackBarVolume
            // 
            trackBarVolume.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                                     | AnchorStyles.Right;
            trackBarVolume.Location = new System.Drawing.Point(329, 31);
            trackBarVolume.Maximum = 100;
            trackBarVolume.Name = "trackBarVolume";
            trackBarVolume.Orientation = Orientation.Vertical;
            trackBarVolume.Size = new System.Drawing.Size(45, 89);
            trackBarVolume.TabIndex = 6;
            trackBarVolume.TickFrequency = 25;
            trackBarVolume.Value = 100;
            trackBarVolume.ValueChanged += new EventHandler(customTrackBar1_ValueChanged);
            // 
            // trackBarPosition
            // 
            trackBarPosition.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                       | AnchorStyles.Right;
            trackBarPosition.Location = new System.Drawing.Point(0, 4);
            trackBarPosition.Name = "trackBarPosition";
            trackBarPosition.Size = new System.Drawing.Size(377, 45);
            trackBarPosition.TabIndex = 0;
            trackBarPosition.TickFrequency = 2;
            trackBarPosition.UserSeek += new EventHandler(trackBar1_UserSeek);
            trackBarPosition.ValueChanged += new EventHandler(trackBar1_ValueChanged);
            // 
            // AudioPlaybackPanel
            // 
            Controls.Add(lstStreams);
            Controls.Add(panel2);
            Controls.Add(trackBarVolume);
            Controls.Add(lblProgress);
            Controls.Add(trackBarPosition);
            Name = "AudioPlaybackPanel";
            Size = new System.Drawing.Size(377, 120);
            panel2.ResumeLayout(false);
            ((ISupportInitialize) trackBarVolume).EndInit();
            ((ISupportInitialize) trackBarPosition).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private bool _updating;
        private bool _loop;

        private bool _isPlaying;
        //private bool _isScrolling = false;

        private DateTime _sampleTime;
        private IAudioStream[] _targetStreams;

        private IAudioStream _targetStream
        {
            get
            {
                if (_targetIndex < 0)
                {
                    return null;
                }

                return _targetStreams[_targetIndex];
            }
        }

        public IAudioStream[] TargetStreams
        {
            get => _targetStreams;
            set
            {
                if (value == _targetStreams)
                {
                    return;
                }

                lstStreams.Items.Clear();
                if (_targetStreams != null)
                {
                    for (int x = 0; x < _targetStreams.Length; x++)
                    {
                        _targetStreams[x].Dispose();
                        _targetStreams[x] = null;
                    }
                }

                if ((_targetStreams = value) != null)
                {
                    _buffers = new AudioBuffer[_targetStreams.Length];
                    if (lstStreams.Visible = _targetStreams.Length > 1)
                    {
                        int i = 1;
                        foreach (IAudioStream s in _targetStreams)
                        {
                            lstStreams.Items.Add("Stream" + i++);
                        }
                    }
                    else
                    {
                        lstStreams.Items.Add("");
                    }
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

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IAudioSource TargetSource
        {
            get => _targetSource;
            set => TargetChanged(value);
        }

        private int? _volume;

        /// <summary>
        /// How much quieter to make the audio output, in hundredths of a decibel.
        /// Custom Song Volume code's difference between 7F (max) and 3F seems to be 1/4 volume - about -6dB. In other words, both the maximum and minimum amplitudes are halved.
        /// See: http://msdn.microsoft.com/en-us/library/windows/desktop/bb280955%28v=vs.85%29.aspx
        /// </summary>
        private int? Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                if (CurrentBuffer != null && _volume != null)
                {
                    CurrentBuffer.Volume = _volume.Value;
                }
            }
        }

        /// <summary>
        /// Sets Volume appropriately.
        /// Range: above .00001, below or at 1. Anything lower than .00001 will set Volume to -10000.
        /// </summary>
        public double? VolumePercent
        {
            set
            {
                if (value == null)
                {
                    Volume = null;
                }
                else
                {
                    Volume = Math.Max(-10000, (int) (Math.Log10(value.Value) * 2000));
                }

                //This should really be in the volume property, but this way I don't need to do more calculations
                Properties.Settings.Default.AudioVolumePercent = value;
                Properties.Settings.Default.Save();

                if (!_updating)
                {
                    trackBarVolume.Value = (int) (value * 100d + 0.5d);
                }
            }
        }

        private AudioProvider _provider;

        private AudioBuffer[] _buffers;

        private AudioBuffer CurrentBuffer
        {
            get
            {
                if (_targetIndex < 0 || _buffers == null)
                {
                    return null;
                }

                return _buffers[_targetIndex];
            }
        }

        public event EventHandler AudioEnded;

        public AudioPlaybackPanel()
        {
            InitializeComponent();
            VolumePercent = Properties.Settings.Default.AudioVolumePercent;

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
            {
                for (int i = 0; i < _buffers.Length; i++)
                {
                    if (_buffers[i] != null)
                    {
                        _buffers[i].Dispose();
                        _buffers[i] = null;
                    }
                }
            }

            if (_targetStreams != null)
            {
                for (int i = 0; i < _targetStreams.Length; i++)
                {
                    if (_targetStreams[i] != null)
                    {
                        _targetStreams[i].Dispose();
                        _targetStreams[i] = null;
                    }
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
            {
                return;
            }

            Close();

            if ((_targetSource = newTarget) == null)
            {
                return;
            }

            if ((TargetStreams = _targetSource.CreateStreams()) == null)
            {
                return;
            }

            if (_targetStream == null)
            {
                return;
            }

            //Create provider
            if (_provider == null)
            {
                _provider = AudioProvider.Create(null);
                _provider?.Attach(this);
            }

            chkLoop.Checked = false;

            //Create buffer for stream
            if (_provider != null)
            {
                for (int i = 0; i < _buffers.Length; i++)
                {
                    _buffers[i] = _provider.CreateBuffer(_targetStreams[i]);
                }
            }

            if (_targetStream.Frequency > 0)
            {
                _sampleTime = new DateTime((long) _targetStream.Samples * 10000000 / _targetStream.Frequency);
            }

            trackBarPosition.Value = 0;
            trackBarPosition.TickStyle = TickStyle.None;
            trackBarPosition.Maximum = _targetStream.Samples;
            trackBarPosition.TickFrequency = _targetStream.Samples / 8;
            trackBarPosition.TickStyle = TickStyle.BottomRight;

            if (_targetStream.Frequency > 0)
            {
                UpdateTimeDisplay();
            }

            Enabled = _targetStream.Samples > 0;

            chkLoop.Checked = Properties.Settings.Default.ContextualLoopAudio && (_targetSource?.IsLooped ?? false);

            if (Enabled && Properties.Settings.Default.AutoPlayAudio)
            {
                Play();
            }
        }

        private void UpdateTimeDisplay()
        {
            if (_targetStream == null)
            {
                return;
            }

            DateTime t = new DateTime((long) trackBarPosition.Value * 10000000 / _targetStream.Frequency);
            lblProgress.Text = $"{t:mm:ss.ff} / {_sampleTime:mm:ss.ff}";
        }

        private void Seek(int sample)
        {
            trackBarPosition.Value = sample;

            //Only seek the buffer when playing.
            if (_isPlaying)
            {
                Stop();
                CurrentBuffer?.Seek(sample);

                Play();
            }
        }

        public void Play()
        {
            if (_isPlaying || CurrentBuffer == null)
            {
                return;
            }

            _isPlaying = true;

            //Start from beginning if at end
            if (trackBarPosition.Value == _targetStream.Samples)
            {
                trackBarPosition.Value = 0;
            }

            //Seek buffer to current sample
            CurrentBuffer.Seek(trackBarPosition.Value);

            //Fill initial buffer
            tmrUpdate_Tick(null, null);
            //Start timer
            tmrUpdate.Start();

            //Change volume
            if (Volume != null)
            {
                CurrentBuffer.Volume = Volume.Value;
            }

            //Begin playback
            CurrentBuffer.Play();

            btnPlay.Text = "Stop";
        }

        private void Stop()
        {
            if (!_isPlaying)
            {
                return;
            }

            _isPlaying = false;

            //Stop timer
            tmrUpdate.Stop();

            //Stop device
            CurrentBuffer?.Stop();

            btnPlay.Text = "Play";
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            if (_isPlaying && CurrentBuffer != null)
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
                else if (CurrentBuffer.ReadSample >= _targetStream.Samples)
                {
                    Seek(0);
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (_isPlaying)
            {
                Stop();
            }
            else
            {
                Play();
            }
        }

        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
            _loop = chkLoop.Checked;
            if (CurrentBuffer != null)
            {
                CurrentBuffer.Loop = _loop;
            }
        }

        private void btnRewind_Click(object sender, EventArgs e)
        {
            Seek(0);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            UpdateTimeDisplay();
        }

        private void trackBar1_UserSeek(object sender, EventArgs e)
        {
            Seek(trackBarPosition.Value);
        }

        private int _targetIndex;

        private void lstStreams_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            bool temp = _isPlaying;

            if (temp)
            {
                Stop();
            }

            _targetIndex = lstStreams.SelectedIndex;

            if (CurrentBuffer != null)
            {
                CurrentBuffer.Fill();
                CurrentBuffer.Loop = _loop;
            }

            if (temp)
            {
                Play();
            }
        }

        private void customTrackBar1_ValueChanged(object sender, EventArgs e)
        {
            _updating = true;
            VolumePercent = trackBarVolume.Value / 100.0d;
            _updating = false;
        }
    }
}