using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrawlManagerLib
{
    public partial class TextBoxDialog : Form
    {
        public string DisplayText
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }

        public TextBoxDialog()
        {
            InitializeComponent();
        }

        public static void ShowDialog(string text, string title = "")
        {
            new TextBoxDialog
            {
                DisplayText = text,
                Text = title
            }.ShowDialog();
        }
    }
}