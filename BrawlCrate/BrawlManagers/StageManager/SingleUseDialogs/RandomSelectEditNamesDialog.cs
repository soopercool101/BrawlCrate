using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.StageManager.SingleUseDialogs
{
    public partial class RandomSelectEditNamesDialog : Form
    {
        public RandomSelectEditNamesDialog(IList<string> names, IList<Image> icons, IList<Image> frontstnames)
        {
            InitializeComponent();
            foreach (string s in names)
            {
                nameList.Items.Add(s);
            }

            nameList.SelectedIndexChanged += (o, e) =>
            {
                int index = nameList.SelectedIndex;
                icon.Image = index < icons.Count && index >= 0
                    ? icons[index]
                    : null;
                frontstname.Image = index < frontstnames.Count && index >= 0
                    ? frontstnames[index]
                    : null;
                textBox1.Text = (nameList.SelectedItem ?? "").ToString();
                textBox1.SelectAll();
            };
            textBox1.KeyPress += (o, e) =>
            {
                if (e.KeyChar == (char) Keys.Enter)
                {
                    int index = nameList.SelectedIndex;
                    if (index >= 0)
                    {
                        nameList.Items[index] = textBox1.Text;
                    }

                    if (nameList.Items.Count > index + 1)
                    {
                        nameList.SelectedIndex = index + 1;
                    }

                    e.Handled = true;
                }
            };
            textBox1.LostFocus += (o, e) =>
            {
                int index = nameList.SelectedIndex;
                if (index >= 0)
                {
                    nameList.Items[index] = textBox1.Text;
                }
            };
            btnReset.Click += (o, e) =>
            {
                int index = nameList.SelectedIndex;
                if (index >= 0)
                {
                    nameList.Items[index] = names[index];
                }
            };
            Shown += (o, e) =>
            {
                if (nameList.SelectedIndex < 0)
                {
                    nameList.SelectedIndex = 0;
                }
            };
        }

        public object this[int index] => nameList.Items[index];

        public string Message
        {
            get => lblMessage.Text;
            set => lblMessage.Text = value;
        }
    }
}