using BrawlLib.Internal.Windows.Controls.Hex_Editor;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms.Section_Editor
{
    /// <summary>
    /// Summary description for FormFind.
    /// </summary>
    public class FormFind : Form
    {
        private HexBox hexFind;
        private TextBox txtFind;
        private RadioButton rbString;
        private RadioButton rbHex;
        private Label label1;
        private Button btnOK;
        private Button btnCancel;
        private GroupBox groupBox1;
        private Label lblPercent;
        private Label lblFinding;
        private CheckBox chkMatchCase;
        private Timer timerPercent;
        private Timer timer;
        private FlowLayoutPanel flowLayoutPanel1;
        private IContainer components;
        private RadioButton rdoAnnotations;
        private readonly SectionEditor _mainWindow;

        public FormFind(SectionEditor mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            HexBox = _mainWindow.hexBox1;
            FindOptions = _mainWindow._findOptions;
            rdoAnnotations.CheckedChanged += new EventHandler(rb_CheckedChanged);
            rbString.CheckedChanged += new EventHandler(rb_CheckedChanged);
            rbHex.CheckedChanged += new EventHandler(rb_CheckedChanged);
        }

        private void ByteProvider_Changed(object sender, EventArgs e)
        {
            ValidateFind();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(FormFind));
            txtFind = new System.Windows.Forms.TextBox();
            rbString = new System.Windows.Forms.RadioButton();
            rbHex = new System.Windows.Forms.RadioButton();
            label1 = new System.Windows.Forms.Label();
            btnOK = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            lblPercent = new System.Windows.Forms.Label();
            lblFinding = new System.Windows.Forms.Label();
            chkMatchCase = new System.Windows.Forms.CheckBox();
            timerPercent = new System.Windows.Forms.Timer(components);
            timer = new System.Windows.Forms.Timer(components);
            hexFind = new HexBox();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            rdoAnnotations = new System.Windows.Forms.RadioButton();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // txtFind
            // 
            resources.ApplyResources(txtFind, "txtFind");
            txtFind.Name = "txtFind";
            txtFind.TextChanged += new EventHandler(txtString_TextChanged);
            // 
            // rbString
            // 
            rbString.Checked = true;
            resources.ApplyResources(rbString, "rbString");
            rbString.Name = "rbString";
            rbString.TabStop = true;
            // 
            // rbHex
            // 
            resources.ApplyResources(rbHex, "rbHex");
            rbHex.Name = "rbHex";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.ForeColor = System.Drawing.Color.Blue;
            label1.Name = "label1";
            // 
            // btnOK
            // 
            resources.ApplyResources(btnOK, "btnOK");
            btnOK.Name = "btnOK";
            btnOK.Click += new EventHandler(btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(btnCancel, "btnCancel");
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Name = "btnCancel";
            btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // lblPercent
            // 
            resources.ApplyResources(lblPercent, "lblPercent");
            lblPercent.Name = "lblPercent";
            // 
            // lblFinding
            // 
            resources.ApplyResources(lblFinding, "lblFinding");
            lblFinding.ForeColor = System.Drawing.Color.Blue;
            lblFinding.Name = "lblFinding";
            // 
            // chkMatchCase
            // 
            resources.ApplyResources(chkMatchCase, "chkMatchCase");
            chkMatchCase.Name = "chkMatchCase";
            chkMatchCase.UseVisualStyleBackColor = true;
            // 
            // timerPercent
            // 
            timerPercent.Tick += new EventHandler(timerPercent_Tick);
            // 
            // timer
            // 
            timer.Interval = 50;
            timer.Tick += new EventHandler(timer_Tick);
            // 
            // hexFind
            // 
            resources.ApplyResources(hexFind, "hexFind");
            hexFind.BlrColor = System.Drawing.Color.FromArgb(255, 255, 100);
            hexFind.BranchOffsetColor = System.Drawing.Color.Plum;
            hexFind.ColumnDividerColor = System.Drawing.Color.Empty;
            hexFind.CommandColor = System.Drawing.Color.FromArgb(200, 255, 200);
            hexFind.InfoForeColor = System.Drawing.Color.Empty;
            hexFind.LinkedBranchColor = System.Drawing.Color.Orange;
            hexFind.Name = "hexFind";
            hexFind.SectionEditor = null;
            hexFind.SelectedColor = System.Drawing.Color.FromArgb(200, 255, 255);
            hexFind.ShadowSelectionColor = System.Drawing.Color.FromArgb(100, 60, 188, 255);
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(groupBox1);
            resources.ApplyResources(flowLayoutPanel1, "flowLayoutPanel1");
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // rdoAnnotations
            // 
            resources.ApplyResources(rdoAnnotations, "rdoAnnotations");
            rdoAnnotations.Name = "rdoAnnotations";
            // 
            // FormFind
            // 
            AcceptButton = btnOK;
            resources.ApplyResources(this, "$this");
            BackColor = System.Drawing.SystemColors.Control;
            CancelButton = btnCancel;
            Controls.Add(rdoAnnotations);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(chkMatchCase);
            Controls.Add(lblPercent);
            Controls.Add(lblFinding);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(rbHex);
            Controls.Add(rbString);
            Controls.Add(txtFind);
            Controls.Add(hexFind);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormFind";
            ShowIcon = false;
            ShowInTaskbar = false;
            Activated += new EventHandler(FormFind_Activated);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FindOptions _findOptions;

        public FindOptions FindOptions
        {
            get => _findOptions;
            set
            {
                _findOptions = value;
                Reinitialize();
            }
        }

        public HexBox HexBox { get; set; }

        private void Reinitialize()
        {
            rbString.Checked = _findOptions.Type == FindType.Text;
            txtFind.Text = _findOptions.Text;
            chkMatchCase.Checked = _findOptions.MatchCase;

            rbHex.Checked = _findOptions.Type == FindType.Hex;
            rdoAnnotations.Checked = _findOptions.Type == FindType.Annotations;

            if (hexFind.ByteProvider != null)
            {
                hexFind.ByteProvider.Changed -= new EventHandler(ByteProvider_Changed);
            }

            byte[] hex = _findOptions.Hex != null ? _findOptions.Hex : new byte[0];
            hexFind.ByteProvider = new DynamicByteProvider(hex);
            hexFind.ByteProvider.Changed += new EventHandler(ByteProvider_Changed);

            txtFind.Enabled = rbString.Checked || rdoAnnotations.Checked;
            hexFind.Enabled = !txtFind.Enabled;
            if (txtFind.Enabled)
            {
                txtFind.Focus();
            }
            else
            {
                hexFind.Focus();
            }

            ValidateFind();
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            txtFind.Enabled = rbString.Checked || rdoAnnotations.Checked;
            hexFind.Enabled = !txtFind.Enabled;

            if (txtFind.Enabled)
            {
                txtFind.Focus();
            }
            else
            {
                hexFind.Focus();
            }
        }

        private void rbString_Enter(object sender, EventArgs e)
        {
            txtFind.Focus();
        }

        private void rbHex_Enter(object sender, EventArgs e)
        {
            hexFind.Focus();
        }

        private void FormFind_Activated(object sender, EventArgs e)
        {
            if (rbString.Checked || rdoAnnotations.Checked)
            {
                txtFind.Focus();
            }
            else
            {
                hexFind.Focus();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _findOptions.MatchCase = chkMatchCase.Checked;

            DynamicByteProvider provider = hexFind.ByteProvider as DynamicByteProvider;
            _findOptions.Hex = provider.Bytes.ToArray();
            _findOptions.Text = txtFind.Text;
            _findOptions.Type = rbHex.Checked ? FindType.Hex : rbString.Checked ? FindType.Text : FindType.Annotations;
            _findOptions.MatchCase = chkMatchCase.Checked;
            _findOptions.IsValid = true;

            bool empty = rbHex.Checked ? _findOptions.Hex.Length == 0 : _findOptions.Text.Length == 0;
            if (empty)
            {
                return;
            }

            _mainWindow._findOptions = _findOptions;
            _mainWindow.Find(false);
            Close();
        }

        private bool _finding;

        private void UpdateUIToNormalState()
        {
            timer.Stop();
            timerPercent.Stop();
            _finding = false;
            txtFind.Enabled = chkMatchCase.Enabled = rbHex.Enabled = rbString.Enabled
                = rdoAnnotations.Enabled = hexFind.Enabled = btnOK.Enabled = true;
        }

        private void UpdateUIToFindingState()
        {
            _finding = true;
            timer.Start();
            timerPercent.Start();
            txtFind.Enabled = chkMatchCase.Enabled = rbHex.Enabled = rbString.Enabled
                = rdoAnnotations.Enabled = hexFind.Enabled = btnOK.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_finding)
            {
                HexBox.AbortFind();
            }
            else
            {
                Close();
            }
        }

        private void txtString_TextChanged(object sender, EventArgs e)
        {
            ValidateFind();
        }

        private void ValidateFind()
        {
            btnOK.Enabled = (rbString.Checked || rdoAnnotations.Checked) && txtFind.Text.Length > 0 ||
                            rbHex.Checked && hexFind.ByteProvider.Length > 0;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (lblFinding.Text.Length == 13)
            {
                lblFinding.Text = "";
            }

            lblFinding.Text += ".";
        }

        private void timerPercent_Tick(object sender, EventArgs e)
        {
            long pos = HexBox.CurrentFindingPosition;
            long length = HexBox.ByteProvider.Length;
            double percent = pos / (double) length * 100;

            System.Globalization.NumberFormatInfo nfi =
                new System.Globalization.CultureInfo("en-US").NumberFormat;

            string text = percent.ToString("0.00", nfi) + " %";
            lblPercent.Text = text;
        }
    }
}