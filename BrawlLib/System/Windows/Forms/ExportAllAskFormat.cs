using BrawlLib;

namespace System.Windows.Forms
{
    public partial class ExportAllFormatDialog : Form
    {
        public ExportAllFormatDialog()
        {
            InitializeComponent();
            var source = FileFilters.TEX0.Split('|');
            for (var i = 0; i < source.Length; i += 2)
                if (source[i] == "NW4R Texture (*.tex0)")
                    comboBox1.Items.Add(new FormatForExportAllDialog("NW4R Texture (*.tex0 / *.plt0)", source[i + 1]));
                else if (!source[i].StartsWith("All"))
                    comboBox1.Items.Add(new FormatForExportAllDialog(source[i], source[i + 1]));
            comboBox1.SelectedIndex = 0;
        }

        public ExportAllFormatDialog(bool isModels)
        {
            if (!isModels) return;
            InitializeComponent();
            label1.Text = "Output format for models:";
            var source = FileFilters.MDL0Export.Split('|');
            for (var i = 0; i < source.Length; i += 2)
                if (!source[i].StartsWith("All"))
                    comboBox1.Items.Add(new FormatForExportAllDialog(source[i], source[i + 1]));
            comboBox1.SelectedIndex = 0;
        }

        public string SelectedExtension =>
            ((FormatForExportAllDialog) comboBox1.SelectedItem).extension.Replace("*", "");
    }

    public class FormatForExportAllDialog
    {
        public FormatForExportAllDialog(string description, string extension)
        {
            this.description = description;
            var locationOfSemicolon = extension.IndexOf(';');
            if (locationOfSemicolon < 0)
                this.extension = extension;
            else
                this.extension = extension.Substring(0, locationOfSemicolon);
        }

        public string description { get; set; }
        public string extension { get; set; }

        public override string ToString()
        {
            return description;
        }
    }
}