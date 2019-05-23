using BrawlLib.Wii.Animations;

namespace System.Windows.Forms
{
    public class EditAllKeyframesDialog : Form
    {
        private int _type;
        private IKeyframeSource _target;

        public EditAllKeyframesDialog() { InitializeComponent(); }

        public DialogResult ShowDialog(IWin32Window owner, int type, IKeyframeSource target)
        {
            _target = target;
            _type = type;
            comboBox1.Items.Add("Add");
            comboBox1.Items.Add("Subtract");
            comboBox1.SelectedIndex = 0;
            return base.ShowDialog(owner);
        }

        private unsafe void btnOkay_Click(object sender, EventArgs e)
        {
            KeyframeEntry kfe;
            if (amount.Text != null)
            {
                if (comboBox1.SelectedIndex == 0)
                    for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                        if ((kfe = _target.KeyArrays[_type].GetKeyframe(x)) != null) //Check for a keyframe
                            kfe._value += Convert.ToSingle(amount.Text);
                if (comboBox1.SelectedIndex == 1)
                    for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                        if ((kfe = _target.KeyArrays[_type].GetKeyframe(x)) != null) //Check for a keyframe
                            kfe._value -= Convert.ToSingle(amount.Text);
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }

        #region Designer

        private TextBox amount;
        private Button btnCancel;
        private Label label1;
        private ComboBox comboBox1;
        private Button btnOkay;

        private void InitializeComponent()
        {
            this.amount = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // amount
            // 
            this.amount.HideSelection = false;
            this.amount.Location = new System.Drawing.Point(116, 12);
            this.amount.Name = "amount";
            this.amount.Size = new System.Drawing.Size(107, 20);
            this.amount.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(197, 38);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.Location = new System.Drawing.Point(116, 38);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 1;
            this.btnOkay.Text = "&Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(229, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "from all.";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 11);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(98, 21);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // EditDialog
            // 
            this.AcceptButton = this.btnOkay;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 69);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.amount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit All Keyframes";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                label1.Text = "to all.";
            if (comboBox1.SelectedIndex == 1)
                label1.Text = "from all.";
        }
    }
}
