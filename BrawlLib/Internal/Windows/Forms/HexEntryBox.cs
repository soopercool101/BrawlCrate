using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    public partial class HexEntryBox : Form
    {
        public string title = "BrawlCrate Hex Entry Box";
        public string lowerText = "Enter hex:";
        public int numBytes;
        private static readonly Regex hexCheck = new Regex(@"^[0-9A-Fa-f\r\n]+$");

        public HexEntryBox()
        {
            InitializeComponent();
        }

        public long NewValue
        {
            get
            {
                if (numNewCount.Text.Length == 0)
                {
                    return -1;
                }

                if (!char.IsDigit(numNewCount.Text[0]))
                {
                    return -1;
                }

                int fromBase = numNewCount.Text.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase)
                    ? 16
                    : 10;
                if (fromBase == 10)
                {
                    // Disallow any non numeric characters
                    for (int i = 0; i < numNewCount.Text.Length; i++)
                    {
                        if (!char.IsDigit(numNewCount.Text[i]))
                        {
                            return -1;
                        }
                    }

                    // Only allow the max number based on the "numBytes" value
                    long maxValue = (long) Math.Pow(16, numBytes) - 1;
                    if (Convert.ToInt64(numNewCount.Value, 10) > maxValue)
                    {
                        return -1;
                    }
                }
                else if (fromBase == 16)
                {
                    // Disallow any amount above the number of bytes
                    if (numNewCount.Text.Length > numBytes + 2)
                    {
                        return -1;
                    }

                    // if it's just "0x", return 0
                    if (numNewCount.Text.Length == 2)
                    {
                        return 0;
                    }

                    // Disallow any non numeric characters that aren't A,B,C,D,E,F or lowercase variations
                    if (!hexCheck.Match(numNewCount.Text.Substring(2)).Success)
                    {
                        return -1;
                    }
                }

                return Convert.ToInt64(numNewCount.Text, fromBase);
            }
        }

        public new DialogResult ShowDialog()
        {
            Text = title;
            label2.Text = lowerText;
            numBytes = 2;
            numNewCount.Text = "0x00";
            return base.ShowDialog();
        }

        public DialogResult ShowDialog(string newTitle, string newLower, int bytes)
        {
            Text = newTitle;
            label2.Text = newLower;
            numBytes = bytes;
            numNewCount.Text = "0x";
            for (int i = 0; i < bytes; i++)
            {
                numNewCount.Text += "0";
            }

            return base.ShowDialog();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}