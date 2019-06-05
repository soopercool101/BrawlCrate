using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class ModelViewerHelp : Form
    {
        private RichTextBox richTextBox1;

        public ModelViewerHelp()
        {
            InitializeComponent();
        }

        public void Show(IWin32Window owner, bool collisionEditor)
        {
            var resources = new ComponentResourceManager(typeof(ModelViewerHelp));
            if (collisionEditor)
            {
                richTextBox1.Text = resources.GetString("richTextBox1.Text2");
                Width = 425;
                Height = 425;
            }
            else
            {
                richTextBox1.Text = resources.GetString("richTextBox1.Text");
            }

            Show(owner);
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        #region Designer

        private Button btnOkay;

        private void InitializeComponent()
        {
            var resources = new ComponentResourceManager(typeof(ModelViewerHelp));
            btnOkay = new Button();
            richTextBox1 = new RichTextBox();
            SuspendLayout();
            // 
            // btnOkay
            // 
            btnOkay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOkay.Location = new Drawing.Point(594, 575);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new Drawing.Size(75, 23);
            btnOkay.TabIndex = 1;
            btnOkay.Text = "&Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += btnOkay_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                                   | AnchorStyles.Left
                                                   | AnchorStyles.Right;
            richTextBox1.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBox1.ForeColor = Color.Black;
            richTextBox1.Location = new Drawing.Point(12, 12);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            richTextBox1.Size = new Drawing.Size(657, 557);
            richTextBox1.TabIndex = 2;
            // 
            // ModelViewerHelp
            // 
            AcceptButton = btnOkay;
            ClientSize = new Drawing.Size(681, 610);
            Controls.Add(richTextBox1);
            Controls.Add(btnOkay);
            Name = "ModelViewerHelp";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Viewer Help";
            ResumeLayout(false);
        }

        #endregion
    }
}