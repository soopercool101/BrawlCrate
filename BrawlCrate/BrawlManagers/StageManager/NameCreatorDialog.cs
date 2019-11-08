using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.StageManager
{
    public partial class NameCreatorDialog : Form
    {
        private NameCreatorSettings _settings;

        public NameCreatorSettings Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                updateText();
            }
        }

        public NameCreatorDialog()
        {
            InitializeComponent();
        }

        private void btnImpact_Click(object sender, EventArgs e)
        {
            Settings = new NameCreatorSettings
            {
                Font = new Font("Impact", 22.5f),
                VerticalOffset = -1
            };
        }

        private void btnEdo_Click(object sender, EventArgs e)
        {
            Settings = new NameCreatorSettings
            {
                Font = new Font("Edo SZ", 22f, FontStyle.Bold),
                VerticalOffset = 2
            };
        }

        private void btnCustom_Click(object sender, EventArgs e)
        {
            using (FontDialog d = new FontDialog())
            {
                if (Settings?.Font != null)
                {
                    d.Font = Settings.Font;
                }

                try
                {
                    DialogResult dr = d.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        Settings = new NameCreatorSettings
                        {
                            Font = d.Font,
                            VerticalOffset = (int) nudOffset.Value
                        };
                    }
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void NameCreatorDialog_Load(object sender, EventArgs e)
        {
            //updateText();
        }

        private void updateText()
        {
            lblCurrentFont.Text = Settings == null
                ? "No font selected"
                : Settings.ToString();
            chkCorner.Checked = Settings == null ? false : Settings.Corner;
        }

        private void btnClearFont_Click(object sender, EventArgs e)
        {
            Settings = null;
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            if (Settings != null)
            {
                Settings.Corner = chkCorner.Checked;
            }
        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
            Settings = new NameCreatorSettings
            {
                Font = new Font("Impact", 18f, FontStyle.Regular),
                Corner = true
            };
        }
    }
}