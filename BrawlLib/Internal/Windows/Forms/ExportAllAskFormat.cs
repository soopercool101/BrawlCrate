using System.Windows.Forms;
using System;

namespace BrawlLib.Internal.Windows.Forms
{
    public partial class ExportAllFormatDialog : Form
    {
        public ExportAllFormatDialog(string title, Type t, string filters)
        {
            InitializeComponent();
            Icon = Properties.Resources.Icon;
            Text = title;
            label1.Text = $"Output format for {t.Name}:";
            string[] source = filters.Split('|');
            for (int i = 0; i < source.Length; i += 2)
            {
                if (!source[i].StartsWith("All"))
                {
                    comboBox1.Items.Add(new FormatForExportAllDialog(source[i], source[i + 1]));
                }
            }

            if (comboBox1.Items.Count == 0)
            {
                return;
            }

            comboBox1.SelectedIndex = 0;
        }

        public string SelectedExtension =>
            ((FormatForExportAllDialog) comboBox1.SelectedItem).extension.Replace("*", "");

        public bool Valid => comboBox1.Items.Count > 0;
        public bool AutoSelect => comboBox1.Items.Count == 1;
    }

    public class FormatForExportAllDialog
    {
        public string description { get; set; }
        public string extension { get; set; }

        public FormatForExportAllDialog(string description, string extension)
        {
            this.description = description;
            int locationOfSemicolon = extension.IndexOf(';');
            if (locationOfSemicolon < 0)
            {
                this.extension = extension;
            }
            else
            {
                this.extension = extension.Substring(0, locationOfSemicolon);
            }
        }

        public override string ToString()
        {
            return description;
        }
    }
}