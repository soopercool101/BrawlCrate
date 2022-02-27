using BrawlLib.Internal.Audio;
using BrawlLib.Internal.IO;
using BrawlLib.Internal.Windows.Controls;
using BrawlLib.SSBB.Types.Audio;
using BrawlLib.Wii.Audio;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    public class BrstmConverterDialog : Form
    {
        internal class InitialStreamWrapper : IAudioStream
        {
            public readonly IAudioStream BaseStream;

            public InitialStreamWrapper(IAudioStream baseStream)
            {
                BaseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));

                IsLooping = BaseStream.IsLooping;
                LoopStartSample = BaseStream.LoopStartSample;
                LoopEndSample = BaseStream.LoopEndSample;
            }

            public WaveFormatTag Format => BaseStream.Format;
            public int BitsPerSample => BaseStream.BitsPerSample;
            public int Samples => BaseStream.Samples;
            public int Channels => BaseStream.Channels;
            public int Frequency => BaseStream.Frequency;

            public bool IsLooping { get; set; }
            public int LoopStartSample { get; set; }
            public int LoopEndSample { get; set; }

            public int SamplePosition
            {
                get => BaseStream.SamplePosition;
                set => BaseStream.SamplePosition = value;
            }

            public unsafe int ReadSamples(VoidPtr destAddr, int numSamples) {
                if (BaseStream.Channels <= 2) {
                    return BaseStream.ReadSamples(destAddr, numSamples);
                } else {
                    int r = 0;
                    short* out_ptr = (short*)(void*)destAddr;
                    short[] buffer = new short[BaseStream.Channels];
                    fixed (short* buffer_ptr = buffer) {
                        while (r < numSamples) {
                            int q = BaseStream.ReadSamples(buffer_ptr, 1);
                            r += q;
                            *out_ptr++ = buffer[0];
                            *out_ptr++ = buffer[1];
                            if (q == 0) break;
                        }
                    }
                    return r;
                }
            }

            public void Wrap()
            {
                SamplePosition = LoopStartSample;
            }

            public void Dispose()
            {
            }
        }

        #region Designer

        private Button btnOkay;
        private Button btnCancel;
        private TextBox txtPath;
        private CustomTrackBar customTrackBar1;
        private GroupBox groupBox1;
        private Label lblText2;
        private Label lblText1;
        private Label lblPlayTime;
        private Button btnPlay;
        private Button btnRewind;
        private Panel pnlInfo;
        private Panel pnlEdit;
        private Panel pnlLoop;
        private Splitter spltEnd;
        private Panel pnlLoopEnd;
        private Splitter spltStart;
        private Panel pnlLoopStart;
        private Label lblStart;
        private Label lblEnd;
        private NumericUpDown numLoopEnd;
        private NumericUpDown numLoopStart;
        private GroupBox groupBox2;
        private Panel panel3;
        private Panel panel4;
        private GroupBox grpLoop;
        private CheckBox chkLoop;
        private CheckBox chkLoopEnable;
        private OpenFileDialog dlgOpen;
        private Timer tmrUpdate;
        private IContainer components;
        private Label lblSamples;
        private Label lblFrequency;
        private Button btnEndSet;
        private Button btnStartSet;
        private Button btnLoopRW;
        private Button btnFFwd;
        private Button btnSeekEnd;
        private GroupBox groupBox3;
        private ComboBox ddlEncoding;
        private Label label1;
        private Button btnBrowse;

        private void InitializeComponent()
        {
            components = new Container();
            btnOkay = new Button();
            btnCancel = new Button();
            txtPath = new TextBox();
            btnBrowse = new Button();
            groupBox1 = new GroupBox();
            lblSamples = new Label();
            lblFrequency = new Label();
            lblText2 = new Label();
            lblText1 = new Label();
            lblPlayTime = new Label();
            btnPlay = new Button();
            btnRewind = new Button();
            pnlInfo = new Panel();
            panel4 = new Panel();
            pnlEdit = new Panel();
            groupBox2 = new GroupBox();
            btnSeekEnd = new Button();
            btnLoopRW = new Button();
            btnFFwd = new Button();
            chkLoop = new CheckBox();
            pnlLoop = new Panel();
            spltEnd = new Splitter();
            pnlLoopEnd = new Panel();
            spltStart = new Splitter();
            pnlLoopStart = new Panel();
            grpLoop = new GroupBox();
            btnEndSet = new Button();
            btnStartSet = new Button();
            numLoopStart = new NumericUpDown();
            numLoopEnd = new NumericUpDown();
            lblEnd = new Label();
            lblStart = new Label();
            panel3 = new Panel();
            chkLoopEnable = new CheckBox();
            dlgOpen = new OpenFileDialog();
            tmrUpdate = new Timer(components);
            customTrackBar1 = new CustomTrackBar();
            groupBox3 = new GroupBox();
            label1 = new Label();
            ddlEncoding = new ComboBox();
            groupBox1.SuspendLayout();
            pnlInfo.SuspendLayout();
            panel4.SuspendLayout();
            pnlEdit.SuspendLayout();
            groupBox2.SuspendLayout();
            pnlLoop.SuspendLayout();
            grpLoop.SuspendLayout();
            ((ISupportInitialize) numLoopStart).BeginInit();
            ((ISupportInitialize) numLoopEnd).BeginInit();
            panel3.SuspendLayout();
            ((ISupportInitialize) customTrackBar1).BeginInit();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // btnOkay
            // 
            btnOkay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOkay.Enabled = false;
            btnOkay.Location = new System.Drawing.Point(3, 3);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new System.Drawing.Size(75, 23);
            btnOkay.TabIndex = 0;
            btnOkay.Text = "Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new EventHandler(btnOkay_Click);
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(80, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // txtPath
            // 
            txtPath.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                              | AnchorStyles.Right;
            txtPath.Location = new System.Drawing.Point(0, 0);
            txtPath.Name = "txtPath";
            txtPath.ReadOnly = true;
            txtPath.Size = new System.Drawing.Size(292, 20);
            txtPath.TabIndex = 2;
            // 
            // btnBrowse
            // 
            btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowse.Location = new System.Drawing.Point(297, 0);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new System.Drawing.Size(25, 20);
            btnBrowse.TabIndex = 3;
            btnBrowse.Text = "...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += new EventHandler(btnBrowse_Click);
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lblSamples);
            groupBox1.Controls.Add(lblFrequency);
            groupBox1.Controls.Add(lblText2);
            groupBox1.Controls.Add(lblText1);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new System.Drawing.Point(0, 57);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(158, 96);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "File Info";
            // 
            // lblSamples
            // 
            lblSamples.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                 | AnchorStyles.Right;
            lblSamples.Location = new System.Drawing.Point(84, 36);
            lblSamples.Name = "lblSamples";
            lblSamples.Size = new System.Drawing.Size(68, 20);
            lblSamples.TabIndex = 3;
            lblSamples.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFrequency
            // 
            lblFrequency.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                   | AnchorStyles.Right;
            lblFrequency.Location = new System.Drawing.Point(84, 16);
            lblFrequency.Name = "lblFrequency";
            lblFrequency.Size = new System.Drawing.Size(68, 20);
            lblFrequency.TabIndex = 2;
            lblFrequency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblText2
            // 
            lblText2.Location = new System.Drawing.Point(6, 36);
            lblText2.Name = "lblText2";
            lblText2.Size = new System.Drawing.Size(72, 20);
            lblText2.TabIndex = 1;
            lblText2.Text = "Samples :";
            lblText2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblText1
            // 
            lblText1.Location = new System.Drawing.Point(6, 16);
            lblText1.Name = "lblText1";
            lblText1.Size = new System.Drawing.Size(72, 20);
            lblText1.TabIndex = 0;
            lblText1.Text = "Frequency :";
            lblText1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPlayTime
            // 
            lblPlayTime.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                  | AnchorStyles.Right;
            lblPlayTime.Location = new System.Drawing.Point(6, 63);
            lblPlayTime.Name = "lblPlayTime";
            lblPlayTime.Size = new System.Drawing.Size(314, 20);
            lblPlayTime.TabIndex = 6;
            lblPlayTime.Text = "0 / 0";
            lblPlayTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPlay
            // 
            btnPlay.Anchor = AnchorStyles.Top;
            btnPlay.Location = new System.Drawing.Point(126, 86);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new System.Drawing.Size(75, 20);
            btnPlay.TabIndex = 7;
            btnPlay.Text = "Play";
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += new EventHandler(btnPlay_Click);
            // 
            // btnRewind
            // 
            btnRewind.Anchor = AnchorStyles.Top;
            btnRewind.Location = new System.Drawing.Point(72, 86);
            btnRewind.Name = "btnRewind";
            btnRewind.Size = new System.Drawing.Size(26, 20);
            btnRewind.TabIndex = 8;
            btnRewind.Text = "|<";
            btnRewind.UseVisualStyleBackColor = true;
            btnRewind.Click += new EventHandler(btnRewind_Click);
            // 
            // pnlInfo
            // 
            pnlInfo.Controls.Add(groupBox1);
            pnlInfo.Controls.Add(groupBox3);
            pnlInfo.Controls.Add(panel4);
            pnlInfo.Dock = DockStyle.Right;
            pnlInfo.Location = new System.Drawing.Point(326, 0);
            pnlInfo.Name = "pnlInfo";
            pnlInfo.Size = new System.Drawing.Size(158, 182);
            pnlInfo.TabIndex = 9;
            // 
            // panel4
            // 
            panel4.Controls.Add(btnOkay);
            panel4.Controls.Add(btnCancel);
            panel4.Dock = DockStyle.Bottom;
            panel4.Location = new System.Drawing.Point(0, 153);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(158, 29);
            panel4.TabIndex = 6;
            // 
            // pnlEdit
            // 
            pnlEdit.Controls.Add(groupBox2);
            pnlEdit.Controls.Add(grpLoop);
            pnlEdit.Controls.Add(panel3);
            pnlEdit.Dock = DockStyle.Fill;
            pnlEdit.Location = new System.Drawing.Point(0, 0);
            pnlEdit.Name = "pnlEdit";
            pnlEdit.Size = new System.Drawing.Size(326, 182);
            pnlEdit.TabIndex = 10;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnSeekEnd);
            groupBox2.Controls.Add(btnLoopRW);
            groupBox2.Controls.Add(btnFFwd);
            groupBox2.Controls.Add(chkLoop);
            groupBox2.Controls.Add(lblPlayTime);
            groupBox2.Controls.Add(pnlLoop);
            groupBox2.Controls.Add(btnRewind);
            groupBox2.Controls.Add(btnPlay);
            groupBox2.Controls.Add(customTrackBar1);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new System.Drawing.Point(0, 65);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(326, 117);
            groupBox2.TabIndex = 13;
            groupBox2.TabStop = false;
            groupBox2.Text = "Playback";
            // 
            // btnSeekEnd
            // 
            btnSeekEnd.Anchor = AnchorStyles.Top;
            btnSeekEnd.Location = new System.Drawing.Point(229, 86);
            btnSeekEnd.Name = "btnSeekEnd";
            btnSeekEnd.Size = new System.Drawing.Size(26, 20);
            btnSeekEnd.TabIndex = 13;
            btnSeekEnd.Text = ">|";
            btnSeekEnd.UseVisualStyleBackColor = true;
            btnSeekEnd.Click += new EventHandler(btnSeekEnd_Click);
            // 
            // btnLoopRW
            // 
            btnLoopRW.Anchor = AnchorStyles.Top;
            btnLoopRW.Enabled = false;
            btnLoopRW.Location = new System.Drawing.Point(99, 86);
            btnLoopRW.Name = "btnLoopRW";
            btnLoopRW.Size = new System.Drawing.Size(26, 20);
            btnLoopRW.TabIndex = 12;
            btnLoopRW.Text = "<";
            btnLoopRW.UseVisualStyleBackColor = true;
            btnLoopRW.Click += new EventHandler(btnLoopRW_Click);
            // 
            // btnFFwd
            // 
            btnFFwd.Anchor = AnchorStyles.Top;
            btnFFwd.Enabled = false;
            btnFFwd.Location = new System.Drawing.Point(202, 86);
            btnFFwd.Name = "btnFFwd";
            btnFFwd.Size = new System.Drawing.Size(26, 20);
            btnFFwd.TabIndex = 11;
            btnFFwd.Text = ">";
            btnFFwd.UseVisualStyleBackColor = true;
            btnFFwd.Click += new EventHandler(btnFFwd_Click);
            // 
            // chkLoop
            // 
            chkLoop.Enabled = false;
            chkLoop.Location = new System.Drawing.Point(10, 86);
            chkLoop.Name = "chkLoop";
            chkLoop.Size = new System.Drawing.Size(52, 20);
            chkLoop.TabIndex = 10;
            chkLoop.Text = "Loop";
            chkLoop.UseVisualStyleBackColor = true;
            chkLoop.CheckedChanged += new EventHandler(chkLoop_CheckedChanged);
            // 
            // pnlLoop
            // 
            pnlLoop.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                              | AnchorStyles.Right;
            pnlLoop.BackColor = System.Drawing.Color.FromArgb(192, 192, 255);
            pnlLoop.Controls.Add(spltEnd);
            pnlLoop.Controls.Add(pnlLoopEnd);
            pnlLoop.Controls.Add(spltStart);
            pnlLoop.Controls.Add(pnlLoopStart);
            pnlLoop.Location = new System.Drawing.Point(18, 50);
            pnlLoop.Name = "pnlLoop";
            pnlLoop.Size = new System.Drawing.Size(290, 12);
            pnlLoop.TabIndex = 9;
            pnlLoop.Visible = false;
            // 
            // spltEnd
            // 
            spltEnd.BackColor = System.Drawing.Color.Red;
            spltEnd.Dock = DockStyle.Right;
            spltEnd.Location = new System.Drawing.Point(287, 0);
            spltEnd.MinExtra = 0;
            spltEnd.MinSize = 0;
            spltEnd.Name = "spltEnd";
            spltEnd.Size = new System.Drawing.Size(3, 12);
            spltEnd.TabIndex = 3;
            spltEnd.TabStop = false;
            // 
            // pnlLoopEnd
            // 
            pnlLoopEnd.BackColor = System.Drawing.Color.FromArgb(255, 192, 128);
            pnlLoopEnd.Dock = DockStyle.Right;
            pnlLoopEnd.Location = new System.Drawing.Point(290, 0);
            pnlLoopEnd.Name = "pnlLoopEnd";
            pnlLoopEnd.Size = new System.Drawing.Size(0, 12);
            pnlLoopEnd.TabIndex = 2;
            pnlLoopEnd.SizeChanged += new EventHandler(pnlLoopEnd_SizeChanged);
            // 
            // spltStart
            // 
            spltStart.BackColor = System.Drawing.Color.Yellow;
            spltStart.Location = new System.Drawing.Point(0, 0);
            spltStart.MinExtra = 0;
            spltStart.MinSize = 0;
            spltStart.Name = "spltStart";
            spltStart.Size = new System.Drawing.Size(3, 12);
            spltStart.TabIndex = 0;
            spltStart.TabStop = false;
            // 
            // pnlLoopStart
            // 
            pnlLoopStart.BackColor = System.Drawing.Color.YellowGreen;
            pnlLoopStart.Dock = DockStyle.Left;
            pnlLoopStart.Location = new System.Drawing.Point(0, 0);
            pnlLoopStart.Name = "pnlLoopStart";
            pnlLoopStart.Size = new System.Drawing.Size(0, 12);
            pnlLoopStart.TabIndex = 1;
            pnlLoopStart.SizeChanged += new EventHandler(pnlLoopStart_SizeChanged);
            // 
            // grpLoop
            // 
            grpLoop.Controls.Add(btnEndSet);
            grpLoop.Controls.Add(btnStartSet);
            grpLoop.Controls.Add(numLoopStart);
            grpLoop.Controls.Add(numLoopEnd);
            grpLoop.Controls.Add(lblEnd);
            grpLoop.Controls.Add(lblStart);
            grpLoop.Dock = DockStyle.Top;
            grpLoop.Enabled = false;
            grpLoop.Location = new System.Drawing.Point(0, 20);
            grpLoop.Name = "grpLoop";
            grpLoop.Size = new System.Drawing.Size(326, 45);
            grpLoop.TabIndex = 15;
            grpLoop.TabStop = false;
            grpLoop.Text = "Loop";
            // 
            // btnEndSet
            // 
            btnEndSet.Location = new System.Drawing.Point(289, 19);
            btnEndSet.Name = "btnEndSet";
            btnEndSet.Size = new System.Drawing.Size(15, 20);
            btnEndSet.TabIndex = 13;
            btnEndSet.Text = "*";
            btnEndSet.UseVisualStyleBackColor = true;
            btnEndSet.Click += new EventHandler(btnEndSet_Click);
            // 
            // btnStartSet
            // 
            btnStartSet.Location = new System.Drawing.Point(141, 19);
            btnStartSet.Name = "btnStartSet";
            btnStartSet.Size = new System.Drawing.Size(15, 20);
            btnStartSet.TabIndex = 4;
            btnStartSet.Text = "*";
            btnStartSet.UseVisualStyleBackColor = true;
            btnStartSet.Click += new EventHandler(btnStartSet_Click);
            // 
            // numLoopStart
            // 
            numLoopStart.Increment = new decimal(new int[]
            {
                14,
                0,
                0,
                0
            });
            numLoopStart.Location = new System.Drawing.Point(59, 19);
            numLoopStart.Name = "numLoopStart";
            numLoopStart.Size = new System.Drawing.Size(81, 20);
            numLoopStart.TabIndex = 10;
            numLoopStart.ValueChanged += new EventHandler(numLoopStart_ValueChanged);
            // 
            // numLoopEnd
            // 
            numLoopEnd.Increment = new decimal(new int[]
            {
                14,
                0,
                0,
                0
            });
            numLoopEnd.Location = new System.Drawing.Point(207, 19);
            numLoopEnd.Name = "numLoopEnd";
            numLoopEnd.Size = new System.Drawing.Size(81, 20);
            numLoopEnd.TabIndex = 11;
            numLoopEnd.ValueChanged += new EventHandler(numLoopEnd_ValueChanged);
            // 
            // lblEnd
            // 
            lblEnd.Location = new System.Drawing.Point(160, 19);
            lblEnd.Name = "lblEnd";
            lblEnd.Size = new System.Drawing.Size(41, 20);
            lblEnd.TabIndex = 2;
            lblEnd.Text = "End:";
            lblEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStart
            // 
            lblStart.Location = new System.Drawing.Point(13, 19);
            lblStart.Name = "lblStart";
            lblStart.Size = new System.Drawing.Size(40, 20);
            lblStart.TabIndex = 12;
            lblStart.Text = "Start:";
            lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            panel3.Controls.Add(txtPath);
            panel3.Controls.Add(btnBrowse);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new System.Drawing.Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(326, 20);
            panel3.TabIndex = 14;
            // 
            // chkLoopEnable
            // 
            chkLoopEnable.Location = new System.Drawing.Point(49, 18);
            chkLoopEnable.Name = "chkLoopEnable";
            chkLoopEnable.Size = new System.Drawing.Size(64, 20);
            chkLoopEnable.TabIndex = 13;
            chkLoopEnable.Text = "Enable";
            chkLoopEnable.UseVisualStyleBackColor = true;
            chkLoopEnable.CheckedChanged += new EventHandler(chkLoopEnable_CheckedChanged);
            // 
            // tmrUpdate
            // 
            tmrUpdate.Interval = 17;
            tmrUpdate.Tick += new EventHandler(tmrUpdate_Tick);
            // 
            // customTrackBar1
            // 
            customTrackBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                      | AnchorStyles.Right;
            customTrackBar1.Location = new System.Drawing.Point(6, 19);
            customTrackBar1.Name = "customTrackBar1";
            customTrackBar1.Size = new System.Drawing.Size(314, 45);
            customTrackBar1.TabIndex = 4;
            customTrackBar1.UserSeek += new EventHandler(customTrackBar1_UserSeek);
            customTrackBar1.ValueChanged += new EventHandler(customTrackBar1_ValueChanged);
            // 
            // groupBox3
            // 
            groupBox3.AutoSize = true;
            groupBox3.Controls.Add(ddlEncoding);
            groupBox3.Controls.Add(label1);
            groupBox3.Dock = DockStyle.Top;
            groupBox3.Location = new System.Drawing.Point(0, 0);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(158, 57);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "Parameters";
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(6, 16);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(64, 20);
            label1.TabIndex = 13;
            label1.Text = "Encoding:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlEncoding
            // 
            ddlEncoding.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlEncoding.FormattingEnabled = true;
            ddlEncoding.Location = new System.Drawing.Point(76, 17);
            ddlEncoding.Name = "ddlEncoding";
            ddlEncoding.Size = new System.Drawing.Size(70, 21);
            ddlEncoding.TabIndex = 14;
            // 
            // BrstmConverterDialog
            // 
            ClientSize = new System.Drawing.Size(484, 182);
            Controls.Add(chkLoopEnable);
            Controls.Add(pnlEdit);
            Controls.Add(pnlInfo);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            MaximizeBox = false;
            MinimumSize = new System.Drawing.Size(500, 216);
            Name = "BrstmConverterDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Brstm Import";
            groupBox1.ResumeLayout(false);
            pnlInfo.ResumeLayout(false);
            pnlInfo.PerformLayout();
            panel4.ResumeLayout(false);
            pnlEdit.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            pnlLoop.ResumeLayout(false);
            grpLoop.ResumeLayout(false);
            ((ISupportInitialize) numLoopStart).EndInit();
            ((ISupportInitialize) numLoopEnd).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((ISupportInitialize) customTrackBar1).EndInit();
            groupBox3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private string _audioSource;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string AudioSource
        {
            get => _audioSource;
            set => _audioSource = value;
        }

        private FileMap _audioData;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileMap AudioData
        {
            get => _audioData;
            set => _audioData = value;
        }

        private static WaveEncoding PreviousEncoding = WaveEncoding.ADPCM;

        private AudioProvider _provider;
        private AudioBuffer _buffer;

        private readonly IAudioStream _initialStream;
        private IAudioStream _sourceStream;

        private DateTime _sampleTime;
        private bool _playing;
        private bool _updating;

        public BrstmConverterDialog()
        {
            InitializeComponent();
            ddlEncoding.Items.Clear();
            ddlEncoding.Items.Add(WaveEncoding.ADPCM);
            ddlEncoding.Items.Add(WaveEncoding.PCM16);
            ddlEncoding.SelectedItem = PreviousEncoding;
            tmrUpdate.Interval = 1000 / 60;
            dlgOpen.Filter = "PCM Audio (*.wav)|*.wav";
            MaximumSize = new System.Drawing.Size(int.MaxValue, 216);
        }

        public BrstmConverterDialog(IAudioStream audioStream)
        {
            _initialStream = audioStream;
            Text = "Loop Point Definition";
            InitializeComponent();
            tmrUpdate.Interval = 1000 / 60;
            MaximumSize = new System.Drawing.Size(int.MaxValue, 216);
        }

        public new DialogResult ShowDialog(IWin32Window owner)
        {
            _audioData = null;
            DialogResult = DialogResult.Cancel;
            //try 
            //{ 
            return base.ShowDialog(owner);
            //}
            //catch (Exception x)
            //{
            //    DisposeProvider();  
            //    MessageBox.Show(x.ToString());
            //    return DialogResult.Cancel;
            //}
        }

        protected override void OnShown(EventArgs e)
        {
            if (_provider == null)
            {
                _provider = AudioProvider.Create(null);
                if (_provider != null)
                {
                    _provider.Attach(this);
                }
                else
                {
                    btnPlay.Enabled = false;
                }
            }

            if (_initialStream != null)
            {
                LoadAudio("Internal audio");
                btnBrowse.Visible = false;
            }
            else if (_audioSource == null)
            {
                if (!LoadAudio())
                {
                    Close();
                    return;
                }
            }
            else if (!LoadAudio(_audioSource))
            {
                Close();
                return;
            }

            base.OnShown(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            DisposeProvider();
            base.OnClosed(e);
        }

        private void DisposeProvider()
        {
            DisposeSource();
            if (_provider != null)
            {
                _provider.Dispose();
                _provider = null;
            }
        }

        private void DisposeSource()
        {
            //Stop playback
            Stop();

            //Dispose buffer
            if (_buffer != null)
            {
                _buffer.Dispose();
                _buffer = null;
            }

            if (_initialStream != _sourceStream)
            {
                //Dispose stream
                if (_sourceStream != null)
                {
                    _sourceStream.Dispose();
                    _sourceStream = null;
                }

                chkLoopEnable.Checked = chkLoop.Checked = chkLoop.Enabled = false;
            }

            btnOkay.Enabled = false;
        }

        private bool LoadAudio()
        {
            if (dlgOpen.ShowDialog(this) != DialogResult.OK)
            {
                return false;
            }

            return LoadAudio(dlgOpen.FileName);
        }

        private bool LoadAudio(string path)
        {
            DisposeSource();

            //Get audio stream
            _sourceStream = _initialStream != null
                ? new InitialStreamWrapper(_initialStream)
                : WAV.FromFile(path);

            _audioSource = path;

            //Create buffer for stream
            if (_provider != null)
            {
                _buffer = _provider.CreateBuffer(_sourceStream);
                _buffer.Loop = chkLoop.Checked;
            }

            //Set controls
            _sampleTime = new DateTime((long) _sourceStream.Samples * 10000000 / _sourceStream.Frequency);

            txtPath.Text = _initialStream != null ? "Internal audio" : path;
            lblFrequency.Text = $"{_sourceStream.Frequency} Hz";
            lblSamples.Text = $"{_sourceStream.Samples}";

            customTrackBar1.Value = 0;
            customTrackBar1.TickStyle = TickStyle.None;
            customTrackBar1.Maximum = _sourceStream.Samples;
            customTrackBar1.TickFrequency = _sourceStream.Samples / 8;
            customTrackBar1.TickStyle = TickStyle.BottomRight;

            numLoopStart.Maximum = numLoopEnd.Maximum = _sourceStream.Samples;
            if (!_sourceStream.IsLooping)
            {
                numLoopStart.Value = 0;
                numLoopEnd.Value = _sourceStream.Samples;

                pnlLoopStart.Width = 0;
                pnlLoopEnd.Width = 0;
            }
            else
            {
                numLoopStart.Value = _sourceStream.LoopStartSample;
                numLoopEnd.Value = _sourceStream.LoopEndSample;
            }

            btnOkay.Enabled = true;

            if (_type == 0)
            {
                chkLoopEnable.Checked = true;
            }

            if (_type != 0)
            {
                groupBox3.Visible = false;
            }

            if (_initialStream != null)
            {
                groupBox3.Visible = false;
            }

            UpdateTimeDisplay();

            return true;
        }

        private void UpdateTimeDisplay()
        {
            if (_sourceStream != null)
            {
                DateTime t = new DateTime((long) customTrackBar1.Value * 10000000 / _sourceStream.Frequency);
                lblPlayTime.Text = $"{t:mm:ss.ff} / {_sampleTime:mm:ss.ff}";
            }
            else
            {
                lblPlayTime.Text = "";
            }
        }

        private void Play()
        {
            if (_playing || _buffer == null)
            {
                return;
            }

            _playing = true;

            if (customTrackBar1.Value == _sourceStream.Samples)
            {
                customTrackBar1.Value = 0;
            }

            _buffer.Seek(customTrackBar1.Value);

            tmrUpdate_Tick(null, null);
            tmrUpdate.Start();

            _buffer.Play();

            btnPlay.Text = "Stop";
        }

        private void Stop()
        {
            if (!_playing)
            {
                return;
            }

            _playing = false;

            tmrUpdate.Stop();

            _buffer?.Stop();

            btnPlay.Text = "Play";
        }

        private void Seek(int sample)
        {
            customTrackBar1.Value = sample;

            //Only seek the buffer when playing.
            if (_playing)
            {
                Stop();
                _buffer.Seek(sample);
                Play();
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (_playing)
            {
                Stop();
            }
            else
            {
                Play();
            }
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            if (_playing && _buffer != null)
            {
                _buffer.Fill();

                customTrackBar1.Value = _buffer.ReadSample;

                if (_buffer.ReadSample >= _sourceStream.Samples)
                {
                    Stop();
                }
            }
        }

        private void btnRewind_Click(object sender, EventArgs e)
        {
            Seek(0);
        }

        private void customTrackBar1_ValueChanged(object sender, EventArgs e)
        {
            UpdateTimeDisplay();
        }

        private void customTrackBar1_UserSeek(object sender, EventArgs e)
        {
            Seek(customTrackBar1.Value);
        }

        private void pnlLoopStart_SizeChanged(object sender, EventArgs e)
        {
            if (_sourceStream == null || _updating)
            {
                return;
            }

            //Get approximate sample number from start of audio.
            float percent = (float) pnlLoopStart.Width / pnlLoop.Width;

            //Should we align to a chunk, or block?
            int startSample = (int) (_sourceStream.Samples * percent);

            _updating = true;
            numLoopStart.Value = startSample;
            _updating = false;
        }

        private void pnlLoopEnd_SizeChanged(object sender, EventArgs e)
        {
            if (_sourceStream == null || _updating)
            {
                return;
            }

            //Get approximate sample number from start of audio.
            float percent = 1.0f - (float) pnlLoopEnd.Width / pnlLoop.Width;

            //End sample doesn't need to be aligned
            int endSample = (int) (_sourceStream.Samples * percent);

            _updating = true;
            numLoopEnd.Value = endSample;
            _updating = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public int Type
        {
            get => _type;
            set
            {
                _type = value;
                Text = $"{(_type == 0 ? "Brstm" : "Wave")} Import";
            }
        }

        public int _type;

        private void btnOkay_Click(object sender, EventArgs e)
        {
            Stop();

            if (_sourceStream is InitialStreamWrapper w && w.BaseStream == _initialStream)
            {
                _initialStream.LoopStartSample = _sourceStream.LoopStartSample;
                _initialStream.LoopEndSample = _sourceStream.LoopEndSample;
                _initialStream.IsLooping = _sourceStream.IsLooping;
            }

            if (_initialStream == null)
            {
                using (ProgressWindow progress = new ProgressWindow(this,
                    $"{(_type == 0 ? "Brstm" : "Wave")} Converter", "Encoding, please wait...", false))
                {
                    switch (_type)
                    {
                        case 0:
                            WaveEncoding encoding = (WaveEncoding) ddlEncoding.SelectedItem;
                            PreviousEncoding = encoding;
                            _audioData = RSTMConverter.Encode(_sourceStream, progress, encoding);
                            break;
                        case 1:
                            _audioData = RSARWaveConverter.Encode(_sourceStream, progress);
                            break;
                        case 2:
                            _audioData = RWAVConverter.Encode(_sourceStream, progress);
                            break;
                    }
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
            if (_buffer != null)
            {
                _buffer.Loop = chkLoop.Checked;
            }
        }

        private void chkLoopEnable_CheckedChanged(object sender, EventArgs e)
        {
            pnlLoop.Visible = grpLoop.Enabled =
                chkLoop.Enabled = btnFFwd.Enabled = btnLoopRW.Enabled = chkLoopEnable.Checked;
            if (!chkLoopEnable.Checked)
            {
                chkLoop.Checked = false;
            }

            if (_sourceStream != null)
            {
                if (chkLoopEnable.Checked)
                {
                    _sourceStream.IsLooping = true;
                    _sourceStream.LoopStartSample = (int) numLoopStart.Value;
                    _sourceStream.LoopEndSample = (int) numLoopEnd.Value;
                }
                else
                {
                    _sourceStream.IsLooping = false;
                    _sourceStream.LoopStartSample = 0;
                    _sourceStream.LoopEndSample = 0;
                }
            }
        }

        private void numLoopStart_ValueChanged(object sender, EventArgs e)
        {
            if (_sourceStream == null)
            {
                return;
            }

            if (!_updating)
            {
                float percent = (float) numLoopStart.Value / _sourceStream.Samples;

                _updating = true;
                pnlLoopStart.Width = (int) (pnlLoop.Width * percent);
                _updating = false;
            }

            if (_sourceStream.IsLooping)
            {
                _sourceStream.LoopStartSample = (int) numLoopStart.Value;
            }
        }

        private void numLoopEnd_ValueChanged(object sender, EventArgs e)
        {
            if (_sourceStream == null)
            {
                return;
            }

            if (!_updating)
            {
                float percent = 1.0f - (float) numLoopEnd.Value / _sourceStream.Samples;

                _updating = true;
                pnlLoopEnd.Width = (int) (pnlLoop.Width * percent);
                _updating = false;
            }

            if (_sourceStream.IsLooping)
            {
                _sourceStream.LoopEndSample = (int) numLoopEnd.Value;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            LoadAudio();
        }

        private void btnStartSet_Click(object sender, EventArgs e)
        {
            numLoopStart.Value = customTrackBar1.Value;
        }

        private void btnEndSet_Click(object sender, EventArgs e)
        {
            numLoopEnd.Value = customTrackBar1.Value;
        }

        private void btnLoopRW_Click(object sender, EventArgs e)
        {
            Seek((int) numLoopStart.Value);
        }

        private void btnFFwd_Click(object sender, EventArgs e)
        {
            Seek((int) numLoopEnd.Value);
        }

        private void btnSeekEnd_Click(object sender, EventArgs e)
        {
            Seek(customTrackBar1.Maximum);
        }
    }
}