using BrawlCrate.BrawlManagers.StageManager.SingleUseDialogs;
using BrawlLib.Internal;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.StageManager
{
    public class ImagePreviewPanel : Panel
    {
        private ToolStripMenuItem borderChange;

        public bool BorderChangeItemEnabled
        {
            get => ContextMenuStrip.Items.Contains(borderChange);
            set
            {
                if (value)
                {
                    ContextMenuStrip.Items.Add(borderChange);
                }
                else
                {
                    ContextMenuStrip.Items.Remove(borderChange);
                }
            }
        }

        public ImagePreviewPanel()
        {
            AllowDrop = true;
            ContextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem replace = new ToolStripMenuItem("Replace");
            replace.Click += new EventHandler(replace_Click);
            ToolStripMenuItem copy = new ToolStripMenuItem("Copy");
            copy.Click += copy_Click;
            ToolStripMenuItem export = new ToolStripMenuItem("Export");
            export.Click += new EventHandler(export_Click);

            ContextMenuStrip.Items.Add(replace);
            ContextMenuStrip.Items.Add(copy);
            ContextMenuStrip.Items.Add(export);

            borderChange = new ToolStripMenuItem("Repaint border ([)");
            borderChange.Click += new EventHandler(borderChange_Click);
        }

        private PortraitViewer getPVParent()
        {
            Control p = Parent;
            while (p != null)
            {
                if (p is PortraitViewer)
                {
                    return (PortraitViewer) p;
                }
                else
                {
                    p = p.Parent;
                }
            }

            return null;
        }

        private void replace_Click(object sender, EventArgs e)
        {
            PortraitViewer pv = getPVParent();
            if (pv == null)
            {
                return;
            }

            using (OpenFileDialog OpenDialog = new OpenFileDialog())
            {
                OpenDialog.Filter = FileFilters.TEX0;
                if (OpenDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = OpenDialog.FileName;
                    pv.Replace(this, fileName);
                }
            }
        }

        private void copy_Click(object sender, EventArgs e)
        {
            PortraitViewer pv = getPVParent();
            if (pv != null)
            {
                Bitmap bmp = pv.GetTexInfoFor(this).tex0.GetImage(0);
                Clipboard.SetImage(bmp);
            }
        }

        private void export_Click(object sender, EventArgs e)
        {
            PortraitViewer pv = getPVParent();
            if (pv == null)
            {
                return;
            }

            using (SaveFileDialog SaveDialog = new SaveFileDialog())
            {
                SaveDialog.Filter = FileFilters.TEX0;
                SaveDialog.FilterIndex = 1;
                if (SaveDialog.ShowDialog() == DialogResult.OK)
                {
                    int fIndex = SaveDialog.FilterIndex;

                    //Fix extension
                    string fileName = ApplyExtension(SaveDialog.FileName, SaveDialog.Filter, fIndex - 1);
                    pv.GetTexInfoFor(this).tex0.Export(fileName);
                }
            }
        }

        public void changeBorder()
        {
            TEX0Node tex0 = getPVParent().GetTexInfoFor(this).tex0;
            Bitmap icon = tex0.GetImage(0);
            Bitmap newIcon = replaceBorder(icon);
            if (newIcon == null)
            {
                return;
            }

            using (ConfirmIconReplaceDialog dialog = new ConfirmIconReplaceDialog())
            {
                dialog.CurrentImage = icon;
                dialog.NewImage = newIcon;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    tex0.Replace(newIcon);
                    getPVParent().UpdateImage();
                }
            }
        }

        private void borderChange_Click(object sender, EventArgs e)
        {
            changeBorder();
        }

        private static Bitmap replaceBorder(Bitmap icon)
        {
            Bitmap border;
            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (File.Exists(exeDir + "\\border.png"))
            {
                border = new Bitmap(exeDir + "\\border.png");
            }
            else if (File.Exists("border.png"))
            {
                border = new Bitmap("border.png");
            }
            else
            {
                Stream stream = Assembly.GetExecutingAssembly()
                                        .GetManifestResourceStream("BrawlCrate.StageManager.border.png");
                if (stream == null)
                {
                    MessageBox.Show(
                        "No border.png found.\nPlease place a 64x56 PNG image named \"border.png\" in the current folder or the program folder.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                border = Image.FromStream(stream) as Bitmap;
            }

            if (border.Width < 64 || border.Height < 56)
            {
                MessageBox.Show("The border.png is not big enough - it must be 64x56 pixels.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                border = null;
                return null;
            }

            bool[,] mask = new bool[64, 56];
            for (int y = 4; y < 52; y++)
            {
                for (int x = 4; x < 60; x++)
                {
                    mask[x, y] = border.GetPixel(x, y).A != 255;
                }
            }

            Bitmap newIcon = new Bitmap(64, 56);
            try
            {
                for (int y = 0; y < 56; y++)
                {
                    for (int x = 0; x < 64; x++)
                    {
                        newIcon.SetPixel(x, y, (mask[x, y] ? icon : border).GetPixel(x, y));
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
            }

            return newIcon;
        }

        /// <summary>
        /// Method lifted directly from BrawlBox.
        /// </summary>
        public static string ApplyExtension(string path, string filter, int filterIndex)
        {
            int tmp;
            if (Path.HasExtension(path) && !int.TryParse(Path.GetExtension(path), out tmp))
            {
                return path;
            }

            int index = filter.IndexOfOccurance('|', filterIndex * 2);
            if (index == -1)
            {
                return path;
            }

            index = filter.IndexOf('.', index);
            int len = Math.Max(filter.Length, filter.IndexOfAny(new char[] {';', '|'})) - index;

            string ext = filter.Substring(index, len);

            if (ext.IndexOf('*') >= 0)
            {
                return path;
            }

            return path + ext;
        }

        public override string ToString()
        {
            return Name + " " + base.ToString();
        }
    }
}