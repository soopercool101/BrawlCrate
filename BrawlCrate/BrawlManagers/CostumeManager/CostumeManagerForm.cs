using BrawlCrate.BrawlManagers.CostumeManager.Portrait_Viewers;
using BrawlCrate.UI;
using BrawlLib.BrawlManagerLib;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
        private PortraitMap pmap = new PortraitMap();

        public bool Swap_Wario;

        public CostumeManagerForm()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.BuildPath))
            {
                Environment.CurrentDirectory = Properties.Settings.Default.BuildPath;
            }

            _title = "BrawlCrate Costume Manager" +
                     Program.AssemblyTitleShort.Substring(
                         Program.AssemblyTitleShort.IndexOf(" ", StringComparison.Ordinal));
            InitializeComponent();
            Icon = BrawlLib.Properties.Resources.Icon;

            portraitViewers = new List<PortraitViewer>
                {cssPortraitViewer1, resultPortraitViewer1, battlePortraitViewer1, infoStockIconViewer1};


            if (!new DirectoryInfo("fighter").Exists)
            {
                if (new DirectoryInfo("/private/wii/app/RSBE/pf/fighter").Exists)
                {
                    Environment.CurrentDirectory = "/private/wii/app/RSBE/pf";
                }
                else if (new DirectoryInfo("/projectm/pf/fighter").Exists)
                {
                    Environment.CurrentDirectory = "/projectm/pf";
                }
                else if (new DirectoryInfo("/pf/fighter").Exists)
                {
                    Environment.CurrentDirectory = "/pf";
                }
            }

            cssPortraitViewer1.NamePortraitPreview = nameportraitPreviewToolStripMenuItem.Checked;
            modelManager1.ZoomOut = defaultZoomLevelToolStripMenuItem.Checked;

            readDir();
        }

        private void readDir()
        {
            if (!Directory.Exists("mario"))
            {
                foreach (string path in new[]
                {
                    "/private/wii/app/RSBE/pf/fighter",
                    "/projectm/pf/fighter",
                    "/fighter"
                })
                {
                    if (Directory.Exists(Environment.CurrentDirectory + path))
                    {
                        Environment.CurrentDirectory += path;
                        readDir();
                        return;
                    }
                }

                string findFighterFolder(string dir)
                {
                    if (dir.EndsWith("\\fighter"))
                    {
                        return dir;
                    }

                    try
                    {
                        foreach (string subdir in Directory.EnumerateDirectories(dir))
                        {
                            string possible = findFighterFolder(subdir);
                            if (Directory.Exists(possible + "\\mario"))
                            {
                                return possible;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    return null;
                }

                string fighterDir = findFighterFolder(Environment.CurrentDirectory);
                if (fighterDir != null)
                {
                    Environment.CurrentDirectory = Path.GetDirectoryName(fighterDir);
                    readDir();
                    return;
                }
            }

            Text = _title + " - " + Environment.CurrentDirectory;

            pmap.ClearAll();
            pmap.BrawlExScan("../BrawlEx");

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
                p.UpdateDirectory();
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
            if (charname == "-")
            {
                foreach (FileInfo f in new DirectoryInfo(".").EnumerateFiles())
                {
                    string name = f.Name.ToLower();
                    if (name.EndsWith(".pac") || name.EndsWith(".pcs"))
                    {
                        listBox2.Items.Add(new FighterFile(f.Name, 1, 1));
                    }
                }
            }
            else
            {
                int charNum = pmap.CharBustTexFor(charname);
                int upperBound = 12;
                for (int i = 0; i < upperBound; i++)
                {
                    string pathNoExt = charname + "/fit" + charname + i.ToString("D2");
                    listBox2.Items.Add(new FighterFile(pathNoExt + ".pac", charNum, i));
                    listBox2.Items.Add(new FighterFile(pathNoExt + ".pcs", charNum, i));
                    if (charname.ToLower() == "kirby")
                    {
                        foreach (string hatchar in PortraitMap.KirbyHats)
                        {
                            listBox2.Items.Add(new FighterFile("kirby/fitkirby" + hatchar + i.ToString("D2") + ".pac",
                                charNum, i));
                        }
                    }
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
            //            fbd.SelectedPath = CurrentDirectory; // Uncomment this if you want the "change directory" dialog to start with the current directory selected
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                Environment.CurrentDirectory = fbd.SelectedPath;
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
                ? new PortraitMap.CBliss()
                : new PortraitMap();
            pmap.BrawlExScan("../BrawlEx");
            foreach (PortraitViewer p in portraitViewers)
            {
                RefreshPortraits();
            }
        }

        private void projectMCheckbox_Click(object sender, EventArgs e)
        {
            cBlissCheckbox.Checked = false;
            pmap = projectMCheckbox.Checked
                ? new PortraitMap.ProjectM()
                : new PortraitMap();
            pmap.BrawlExScan("../BrawlEx");
            foreach (PortraitViewer p in portraitViewers)
            {
                RefreshPortraits();
            }
        }

        private void swapPortraitsForWarioStylesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Swap_Wario = swapPortraitsForWarioStylesToolStripMenuItem.Checked;
            foreach (PortraitViewer p in portraitViewers)
            {
                RefreshPortraits();
            }
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
                kirby = "kirby/FitKirby" + nn + ".pcs";
                hat = ff.FullName;
            }
            else
            {
                kirby = ff.FullName;
                hat = "kirby/FitKirbyMewtwo" + nn + ".pac";
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