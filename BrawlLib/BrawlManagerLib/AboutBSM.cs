using System.Windows.Forms;
using System.Reflection;
using System;
using System.Drawing;

namespace BrawlManagerLib
{
    public partial class AboutBSM : Form
    {
        public string AboutText
        {
            set => textBox1.Text = value;
        }

        public AboutBSM(Icon icon, Assembly a)
        {
            InitializeComponent();

            if (icon != null)
            {
                Icon = icon;
            }

            if (a != null)
            {
                software_title.Text =
                    ((AssemblyTitleAttribute) a.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
                version.Text = a.GetName().Version.ToString();
                copyright.Text =
                    ((AssemblyCopyrightAttribute) a.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0])
                    .Copyright;

                Assembly b = Assembly.GetAssembly(GetType());
                library_title.Text =
                    ((AssemblyTitleAttribute) b.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
                library_version.Text = b.GetName().Version.ToString();
                library_copyright.Text =
                    ((AssemblyCopyrightAttribute) b.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0])
                    .Copyright;

                Assembly dll = Assembly.GetAssembly(typeof(BrawlLib.StringTable));
                brawllib.Text = "Using " +
                                ((AssemblyTitleAttribute) dll.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)
                                    [0]).Title + "\r\n" +
                                ((AssemblyCopyrightAttribute) dll.GetCustomAttributes(
                                    typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;

                textBox1.Text =
                    "Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the \"Software\"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:\r\n" +
                    "\r\n" +
                    "The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.\r\n" +
                    "\r\n" +
                    "THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";
            }
        }
    }
}