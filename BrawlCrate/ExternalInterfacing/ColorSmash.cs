using BrawlCrate.NodeWrappers;
using BrawlCrate.UI;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Textures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlCrate.ExternalInterfacing
{
    public static class ColorSmash
    {
        public static bool CanRunColorSmash => File.Exists($"{Program.AppPath}\\color_smash.exe");


        private static DirectoryInfo InputDir => Directory.CreateDirectory(Path.Combine(Program.AppPath, "cs"));
        private static DirectoryInfo OutputDir => Directory.CreateDirectory(Path.Combine(InputDir.FullName, "out"));

        public static void ColorSmashTex0(object sender, EventArgs e)
        {
            // If this was selected via keycode when it's invalid, return without error
            if (!CanRunColorSmash || MainForm.Instance.resourceTree.SelectedNodes.Count <= 1)
            {
                return;
            }

            short paletteCount = 0;
            foreach (TreeNode n in MainForm.Instance.resourceTree.SelectedNodes)
            {
                if (n is TEX0Wrapper tw)
                {
                    TEX0Node t = (TEX0Node) tw.Resource;
                    if (paletteCount < 256)
                    {
                        if (!t.HasPalette || t.GetPaletteNode() == null)
                        {
                            paletteCount = 256;
                        }
                        else if (t.HasPalette && t.GetPaletteNode() != null &&
                                 t.GetPaletteNode().Palette.Entries.Length > paletteCount)
                        {
                            paletteCount = (short) Math.Min(t.GetPaletteNode().Palette.Entries.Length, 256);
                        }
                    }
                }
            }

            if (paletteCount == 0)
            {
                paletteCount = 256;
            }

            using (NumericInputForm frm = new NumericInputForm())
            {
                if (frm.ShowDialog("Color Smasher", "How many colors?", paletteCount) == DialogResult.OK)
                {
                    ColorSmashTex0(frm.NewValue);
                }
            }
        }

        public static void ColorSmashTex0()
        {
            ColorSmashTex0(-1);
        }

        public static void ColorSmashTex0(int paletteCount)
        {
            // If this was selected via keycode when it's invalid, return without error
            if (!CanRunColorSmash || MainForm.Instance.resourceTree.SelectedNodes.Count <= 1)
            {
                return;
            }

            // Clear the directories. If they aren't empty, the implementation WILL NOT work
            ClearDirectories();

            // Throw error if color smash directory isn't empty
            if (InputDir.GetFiles().Length > 0 || OutputDir.GetFiles().Length > 0)
            {
                MessageBox.Show(
                    "One or more files exist in the required Color Smash folder. Please delete these nodes manually and try again",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearDirectories();
                return;
            }

            List<TEX0Node> textureList = new List<TEX0Node>();
            Dictionary<int, string> names = new Dictionary<int, string>();
            BRRESNode b = (((TEX0Wrapper) MainForm.Instance.resourceTree.SelectedNodes[0]).Resource as TEX0Node)?
                .BRESNode;
            if (b == null)
            {
                MessageBox.Show(
                    "The BRRES could not be found. Color Smashing is only supported for TEX0 nodes in BRRES groups",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearDirectories();
                return;
            }

            int index = int.MaxValue;
            bool detectPalettes = paletteCount <= 0;
            if (detectPalettes)
            {
                paletteCount = 0;
            }

            foreach (TreeNode n in MainForm.Instance.resourceTree.SelectedNodes)
            {
                // If this was selected via keycode when it's invalid, return without error
                if (!(n is TEX0Wrapper))
                {
                    ClearDirectories();
                    return;
                }

                if (((TEX0Wrapper) n).Resource is TEX0Node t)
                {
                    if (t.BRESNode != b)
                    {
                        MessageBox.Show("Color Smash is only supported for nodes in the same BRRES", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ClearDirectories();
                        return;
                    }

                    textureList.Add(t);
                    int placement = t.Parent.Children.IndexOf(t);
                    names.Add(placement, t.Name);
                    if (placement < index)
                    {
                        index = placement;
                    }

                    t.Export($"{InputDir.FullName}\\{placement:D5}.png");

                    if (detectPalettes && paletteCount < 256)
                    {
                        if (!t.HasPalette || t.GetPaletteNode() == null)
                        {
                            paletteCount = 256;
                        }
                        else if (t.HasPalette && t.GetPaletteNode() != null &&
                                 t.GetPaletteNode().Palette.Entries.Length > paletteCount)
                        {
                            paletteCount = (short) Math.Min(t.GetPaletteNode().Palette.Entries.Length, 256);
                        }
                    }
                }
            }

            if (index == int.MaxValue)
            {
                MessageBox.Show("Could not properly get the index of the images", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearDirectories();
                return;
            }

            if (detectPalettes && paletteCount == 0)
            {
                paletteCount = 256;
            }

            foreach (TEX0Node t in textureList)
            {
                if (t.HasPalette && t.GetPaletteNode() != null)
                {
                    t.GetPaletteNode().Remove();
                }

                t.Remove();
            }

            ColorSmasher(paletteCount, b, index, names);
        }

        public static void ColorSmashImport(BRRESNode b)
        {
            if (!CanRunColorSmash || b == null)
            {
                return;
            }

            using (NumericInputForm frm = new NumericInputForm())
            {
                if (Program.OpenFiles("Portable Network Graphics|*.png", out string[] fileNames) > 0 &&
                    frm.ShowDialog("Color Smasher", "How many colors?", 256) == DialogResult.OK)
                {
                    Dictionary<int, string> names = new Dictionary<int, string>();
                    foreach (string s in fileNames)
                    {
                        File.Copy(s, Path.Combine(InputDir.FullName, $"{names.Count:D5}.png"));
                        names.Add(names.Count, Path.GetFileNameWithoutExtension(s));
                    }

                    ColorSmasher(frm.NewValue, b, b.ImageCount, names);
                }
            }
        }

        public static void ColorSmasher(int paletteCount, BRRESNode b, int index, Dictionary<int, string> names)
        {
            Process cSmash = Process.Start(new ProcessStartInfo
            {
                FileName = Path.Combine(Program.AppPath, "color_smash.exe"),
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = $"-c RGB5A3 -n {paletteCount}"
            });
            cSmash?.WaitForExit();

            List<TEX0Node> textureList = new List<TEX0Node>();
            int count = 0;
            BRESGroupNode texGroup = b.GetOrCreateFolder<TEX0Node>();
            foreach (FileInfo f in OutputDir.GetFiles())
            {
                FileInfo f2 = new FileInfo($"{InputDir.FullName}\\{f.Name}");
                int i = int.Parse(f.Name.Substring(0, f.Name.IndexOf(".", StringComparison.Ordinal)));
                using (TextureConverterDialog dlg = new TextureConverterDialog())
                {
                    dlg.ImageSource = f.FullName;
                    dlg.ChkImportPalette.Checked = true;
                    dlg.Automatic = true;
                    dlg.StartingFormat = WiiPixelFormat.CI8;

                    if (dlg.ShowDialog(MainForm.Instance, b) != DialogResult.OK)
                    {
                        continue;
                    }

                    TEX0Node t = dlg.TEX0TextureNode;
                    t.Name = names[i];
                    textureList.Add(t);
                    dlg.Dispose();
                    t.OriginalPath = "";
                    if (texGroup.Children.Count > count + 1)
                    {
                        texGroup.RemoveChild(t);
                        texGroup.InsertChild(t, false, index + count);
                    }

                    count++;
                }

                f2.Delete();
            }

            if (textureList.Count > 0)
            {
                textureList.Remove(textureList.Last());
                foreach (TEX0Node t in textureList)
                {
                    t.SharesData = true;
                }
            }

            if (InputDir.GetFiles().Length > 0)
            {
                MessageBox.Show(
                    "One or more files threw an error. Please ensure all relevant textures can be Color Smashed together.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                foreach (FileInfo f in InputDir.GetFiles())
                {
                    int i = int.Parse(f.Name.Substring(0, f.Name.IndexOf(".", StringComparison.Ordinal)));
                    using (TextureConverterDialog dlg = new TextureConverterDialog())
                    {
                        dlg.ImageSource = f.FullName;
                        dlg.Automatic = true;

                        if (dlg.ShowDialog(MainForm.Instance, b) != DialogResult.OK)
                        {
                            continue;
                        }

                        TEX0Node t = dlg.TEX0TextureNode;
                        t.Name = names[i];

                        dlg.Dispose();
                        t.OriginalPath = "";
                        if (texGroup.Children.Count > count + 1)
                        {
                            texGroup.RemoveChild(t);
                            texGroup.InsertChild(t, false, index + count);
                        }

                        count++;
                    }
                }
            }

            ClearDirectories();
        }

        public static void ClearDirectories()
        {
            foreach (FileInfo f in OutputDir.GetFiles())
            {
                try
                {
                    f.Delete();
                }
                catch
                {
                    // ignored
                }
            }

            try
            {
                OutputDir.Delete();
            }
            catch
            {
                // ignored
            }

            foreach (FileInfo f in InputDir.GetFiles())
            {
                try
                {
                    f.Delete();
                }
                catch
                {
                    // ignored
                }
            }

            try
            {
                InputDir.Delete();
            }
            catch
            {
                // ignored
            }
        }
    }
}