using BrawlCrate.BrawlManagers.CostumeManager.Portrait_Viewers;
using BrawlCrate.UI;
using BrawlLib.BrawlManagerLib;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#if !MONO
using BrawlLib.Internal.Windows.Forms.Ookii.Dialogs;
#endif

namespace BrawlCrate.BrawlManagers.CostumeManager
{
    public partial class CostumeManagerForm : Form
    {
        private static string _title = "";

        private List<PortraitViewer> portraitViewers = new List<PortraitViewer>();
        private PortraitMap pmap;

        public bool Swap_Wario;

        /// <summary>
        ///     Replacement for Environment.CurrentDirectory to be more localized and not break functionality of other open managers.
        /// </summary>
        public string CurrentDirectory
        {
            get => curDir;
            set => curDir = value;
        }

        private string curDir;

        public CostumeManagerForm()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.BuildPath))
            {
                CurrentDirectory = Properties.Settings.Default.BuildPath;
            }
            
            if(string.IsNullOrWhiteSpace(CurrentDirectory))
            {
                CurrentDirectory = Environment.CurrentDirectory;
            }

            _title = "BrawlCrate Costume Manager" +
                     Program.AssemblyTitleShort.Substring(
                         Program.AssemblyTitleShort.IndexOf(" ", StringComparison.Ordinal));
            InitializeComponent();
            Icon = BrawlLib.Properties.Resources.Icon;

            portraitViewers = new List<PortraitViewer>
                {cssPortraitViewer1, resultPortraitViewer1, battlePortraitViewer1, infoStockIconViewer1};

            cssPortraitViewer1.NamePortraitPreview = nameportraitPreviewToolStripMenuItem.Checked;
            modelManager1.ZoomOut = defaultZoomLevelToolStripMenuItem.Checked;
            pmap = new PortraitMap(this);
            readDir();
        }

        private void readDir()
        {
            if (!new DirectoryInfo(Path.Combine(CurrentDirectory, "fighter")).Exists)
            {
                if (new DirectoryInfo(Path.Combine(CurrentDirectory, "private/wii/app/RSBE/pf/fighter")).Exists)
                {
                    CurrentDirectory = Path.Combine(CurrentDirectory, "private/wii/app/RSBE/pf/");
                }
                else if (new DirectoryInfo(Path.Combine(CurrentDirectory, "projectm/pf/fighter")).Exists)
                {
                    CurrentDirectory = Path.Combine(CurrentDirectory, "projectm/pf/");
                }
                else if (new DirectoryInfo(Path.Combine(CurrentDirectory, "pf/fighter")).Exists)
                {
                    CurrentDirectory = Path.Combine(CurrentDirectory, "pf/");
                }
            }

            Text = _title + " - " + CurrentDirectory;

            pmap.ClearAll();
            pmap.BrawlExScan(Path.Combine(CurrentDirectory, "BrawlEx"));

            int selectedIndex = listBox1.SelectedIndex;
            listBox1.Items.Clear();
            listBox1.Items.Add("-");
            foreach (string charname in pmap.GetKnownFighterNames())
            {
                if (charname != null)
                {
                    listBox1.Items.Add(charname);
                }
            }

            foreach (PortraitViewer p in portraitViewers)
            {
                p.UpdateDirectory(CurrentDirectory);
            }

            if (selectedIndex >= 0)
            {
                listBox1.SelectedIndex = selectedIndex;
            }
        }

        public void LoadFile(string path)
        {
            modelManager1.LoadFile(path);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            FighterFile ff = (FighterFile) listBox2.SelectedItem;
            string path = ff.FullName;
            LoadFile(path);
            RefreshPortraits();
        }

        public void RefreshPortraits()
        {
            FighterFile ff = (FighterFile) listBox2.SelectedItem;
            if (ff == null)
            {
                return;
            }

            int portraitNum = ff.CostumeNum;
            bool confident = false;

            if (pmap.ContainsMapping(ff.CharNum))
            {
                int[] mappings = pmap.GetPortraitMappings(ff.CharNum);
                int index = Array.IndexOf(mappings, ff.CostumeNum);
                if (index >= 0)
                {
                    portraitNum = index;
                    confident = true;
                }
            }

            if (Swap_Wario && ff.CharNum == pmap.CharBustTexFor("wario"))
            {
                portraitNum = (portraitNum + 6) % 12;
            }

            foreach (PortraitViewer p in portraitViewers)
            {
                p.UpdateImage(ff.CharNum, portraitNum);
            }

            costumeNumberLabel.UpdateImage(ff.CharNum, portraitNum, confident);
        }

        public void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateCostumeSelectionPane();
        }

        public void updateCostumeSelectionPane()
        {
            int selectedIndex = listBox2.SelectedIndex;

            string charname = listBox1.SelectedItem.ToString();
            listBox2.Items.Clear();
            if (charname != "-")
            {
                int charNum = pmap.CharBustTexFor(charname);
                DirectoryInfo dir = new DirectoryInfo(Path.Combine(CurrentDirectory, "fighter", charname));
                if (!dir.Exists)
                {
                    return;
                }
                foreach (FileInfo f in new DirectoryInfo(Path.Combine(CurrentDirectory, "fighter", charname))
                                       .GetFiles().Where(o =>
                                           o.Extension.Equals(".pac", StringComparison.OrdinalIgnoreCase) ||
                                           o.Extension.Equals(".pcs", StringComparison.OrdinalIgnoreCase)))
                {
                    // Ignore non-costume files
                    if (f.Name.Equals($"fit{charname}.pac", StringComparison.OrdinalIgnoreCase) ||
                        f.Name.StartsWith($"fit{charname}motion", StringComparison.OrdinalIgnoreCase) ||
                        f.Name.StartsWith($"fit{charname}final", StringComparison.OrdinalIgnoreCase) ||
                        f.Name.StartsWith($"fit{charname}entry", StringComparison.OrdinalIgnoreCase) ||
                        f.Name.StartsWith($"fit{charname}etc", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    int costumeNum = -1;
                    try
                    {
                        if (int.TryParse(f.Name.Substring(f.Name.Length - 6, 2), out int i))
                        {
                            costumeNum = i;
                        }
                    }
                    catch
                    {
                        // Ignore, not necessary
                        costumeNum = -1;
                    }

                    listBox2.Items.Add(new FighterFile(f.FullName, charNum, costumeNum));
                }

                listBox2.SelectedIndex = selectedIndex < listBox2.Items.Count ? selectedIndex : 0;
            }
        }

        private void changeDirectory_Click(object sender, EventArgs e)
        {
#if !MONO
            VistaFolderBrowserDialog fbd = new VistaFolderBrowserDialog { UseDescriptionForTitle = true };
#else
            FolderBrowserDialog fbd = new FolderBrowserDialog();
#endif
            fbd.SelectedPath = CurrentDirectory; // Uncomment this if you want the "change directory" dialog to start with the current directory selected
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                CurrentDirectory = fbd.SelectedPath;
                readDir();
            }
        }

        private void hidePolygonsCheckbox_Click(object sender, EventArgs e)
        {
            modelManager1.UseExceptions = hidePolygonsCheckbox.Checked;
            modelManager1.RefreshModel();
        }

        private void cBlissCheckbox_Click(object sender, EventArgs e)
        {
            projectMCheckbox.Checked = false;
            pmap = cBlissCheckbox.Checked
                ? new PortraitMap.CBliss(this)
                : new PortraitMap(this);
            pmap.BrawlExScan(Path.Combine(CurrentDirectory, "BrawlEx"));
            foreach (PortraitViewer p in portraitViewers)
            {
                RefreshPortraits();
            }
        }

        private void projectMCheckbox_Click(object sender, EventArgs e)
        {
            cBlissCheckbox.Checked = false;
            pmap = projectMCheckbox.Checked
                ? new PortraitMap.ProjectM(this)
                : new PortraitMap(this);
            pmap.BrawlExScan(Path.Combine(CurrentDirectory, "BrawlEx"));
            RefreshPortraits();
        }

        private void swapPortraitsForWarioStylesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Swap_Wario = swapPortraitsForWarioStylesToolStripMenuItem.Checked;
            RefreshPortraits();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            listBox2.SelectedIndex = listBox2.IndexFromPoint(listBox2.PointToClient(Cursor.Position));
        }

        private void _deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string toDelete = (listBox2.SelectedItem as FighterFile).FullName;
            if (Path.HasExtension(toDelete))
            {
                toDelete = toDelete.Substring(0, toDelete.LastIndexOf('.'));
            }

            FileInfo pac = new FileInfo(toDelete + ".pac");
            FileInfo pcs = new FileInfo(toDelete + ".pcs");
            if (DialogResult.Yes == MessageBox.Show(
                    "Are you sure you want to delete " + pac.Name + "/" + pcs.Name + "?",
                    "Confirm", MessageBoxButtons.YesNo))
            {
                modelManager1.LoadFile(null);
                if (pac.Exists)
                {
                    pac.Delete();
                }

                if (pcs.Exists)
                {
                    pcs.Delete();
                }

                updateCostumeSelectionPane();
            }
        }

        private void copyToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "PAC Archive (*.pac)|*.pac|" +
                             "Compressed PAC Archive (*.pcs)|*.pcs|" +
                             "Archive Pair (*.pair)|*.pair";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    modelManager1.WorkingRoot.Export(dlg.FileName);
                }
            }
        }

        private void updateSSSStockIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cssPortraitViewer1.UpdateSSSStockIcons();
        }

        private void copyToOtherPacpcsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string charfile = ((FighterFile) listBox2.SelectedItem).FullName;
            if (charfile.EndsWith(".pac", StringComparison.InvariantCultureIgnoreCase))
            {
                ((ARCNode) modelManager1.WorkingRoot)
                    .ExportPCS(charfile.Substring(0, charfile.Length - 4) + ".pcs");
                updateCostumeSelectionPane();
            }
            else if (charfile.EndsWith(".pcs", StringComparison.InvariantCultureIgnoreCase))
            {
                ((ARCNode) modelManager1.WorkingRoot)
                    .ExportPAC(charfile.Substring(0, charfile.Length - 4) + ".pac");
                updateCostumeSelectionPane();
            }
            else
            {
                MessageBox.Show("Not a .pac or .pcs file");
            }
        }

        private void nameportraitPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cssPortraitViewer1.NamePortraitPreview = nameportraitPreviewToolStripMenuItem.Checked;
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog())
            {
                cd.Color = cssPortraitViewer1.BackColor;
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    foreach (PortraitViewer pv in portraitViewers)
                    {
                        pv.BackColor = cd.Color;
                    }
                }
            }
        }

        private void screenshotPortraitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap screenshot = modelManager1.GrabScreenshot(true);

            int size = Math.Min(screenshot.Width, screenshot.Height);
            Bitmap rect = new Bitmap(size, (int) (size * 160.0 / 128.0));
            using (Graphics g = Graphics.FromImage(rect))
            {
                int x = (screenshot.Width - rect.Width) / -2;
                int y = (screenshot.Height - rect.Height) / -2;
                g.DrawImage(screenshot, x, y);
            }

            string iconFile = Path.GetTempPath() + Guid.NewGuid() + ".png";

            BitmapUtilities.Resize(rect, new Size(128, 160)).Save(iconFile);
            cssPortraitViewer1.ReplaceMain(iconFile, false);

            try
            {
                File.Delete(iconFile);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not delete temporary file " + iconFile);
            }
        }

        private void limitModelViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modelManager1.ModelPreviewSize = limitModelViewerToolStripMenuItem.Checked
                ? (Size?) new Size(256, 320)
                : null;
        }

        private void defaultZoomLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modelManager1.ZoomOut = defaultZoomLevelToolStripMenuItem.Checked;
            modelManager1.RefreshModel();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            AboutForm.Instance.ShowDialog(this);
        }

        private void updateMewtwoHatForCurrentKirbyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string kirby, hat;

            FighterFile ff = (FighterFile) listBox2.SelectedItem;
            if (ff == null || pmap.CharBustTexFor("kirby") != ff.CharNum)
            {
                MessageBox.Show(this, "Select a Kirby costume before using this feature.");
                return;
            }

            string p = ff.FullName.ToLower();
            string nn = ff.CostumeNum.ToString("D2");
            if (p.Contains("fitkirbymewtwo"))
            {
                kirby = Path.Combine(CurrentDirectory, "fighter", "kirby/FitKirby" + nn + ".pcs");
                hat = ff.FullName;
            }
            else
            {
                kirby = ff.FullName;
                hat = Path.Combine(CurrentDirectory, "fighter", "kirby/FitKirbyMewtwo" + nn + ".pac");
            }

            if (!File.Exists(kirby))
            {
                MessageBox.Show(this, "Could not find file: " + kirby);
                return;
            }

            if (!File.Exists(hat))
            {
                MessageBox.Show(this, "Could not find file: " + hat);
                return;
            }

            if (DialogResult.OK == MessageBox.Show(this, "Copy from " + kirby + " to " + hat + "?", Text,
                    MessageBoxButtons.OKCancel))
            {
                KirbyCopy.Copy(kirby, hat);
            }
        }

        private void use16ptFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float? size = use16ptFontToolStripMenuItem.Checked
                ? 16f
                : (float?) null;

            Font = new Font(Font.FontFamily, size ?? 8.25f, Font.Style);
            toolStrip1.Font = new Font(toolStrip1.Font.FontFamily, size ?? 9f, toolStrip1.Font.Style);
        }
    }
}