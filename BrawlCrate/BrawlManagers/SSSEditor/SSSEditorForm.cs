using BrawlLib.SSBB.ResourceNodes;
using BrawlManagerLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RazorEngine.Templating;
using System.Drawing.Imaging;
using Newtonsoft.Json;

namespace BrawlCrate.SSSEditor
{
    public partial class BrawlCrate.SSSEditorForm : Form
    {
        // Source data
        private CustomSSSCodeset sss;
        private BRRESNode md80;

        private string html = "";

        #region Collect data from controls

        private List<StagePair> getDefinitions()
        {
            return JsonConvert.DeserializeObject<List<StagePair>>(
                (string) webBrowser1.Document.InvokeScript("getDefinitions"));
        }

        private List<int> getScreen1()
        {
            return JsonConvert.DeserializeObject<List<int>>((string) webBrowser1.Document.InvokeScript("getScreen1"));
        }

        private List<int> getScreen2()
        {
            return JsonConvert.DeserializeObject<List<int>>((string) webBrowser1.Document.InvokeScript("getScreen2"));
        }

        #endregion

        public BrawlCrate.SSSEditorForm(string gct, string pac)
        {
            InitializeComponent();
            foreach (Control c in tblColorCodeKeys.Controls)
            {
                c.DoubleClick += (o, e) => { tblColorCodeKeys.Visible = false; };
            }

            try
            {
                Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetCallingAssembly().Location);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;

            sss = gct != null
                ? new CustomSSSCodeset(gct)
                : new CustomSSSCodeset();

            if (pac != null)
            {
                ReloadIfValidPac(pac);
            }
            else
            {
                md80 = new BRRESNode();
                ReloadData();
            }

            FormClosed += (o, e) => TempFiles.DeleteAll();
        }

        private void ReloadData()
        {
            if (sss.OtherCodesIgnoredInSameFile > 0)
            {
                MessageBox.Show(this,
                    "More than one Custom SSS code found in the codeset. All but the last one will be ignored.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            List<StagePair> screen1 = new List<StagePair>();
            List<StagePair> screen2 = new List<StagePair>();
            List<StagePair> definitions = new List<StagePair>();
            for (int i = 0; i < sss.sss3.Length; i += 2)
            {
                definitions.Add(new StagePair
                {
                    stage = sss.sss3[i],
                    icon = sss.sss3[i + 1],
                });
            }

            foreach (byte b in sss.sss1)
            {
                screen1.Add(definitions[b]);
            }

            foreach (byte b in sss.sss2)
            {
                screen2.Add(definitions[b]);
            }

            PairListModel model = new PairListModel();
            for (int i = 0; i < model.songsByStage.Length; i++)
            {
                model.songsByStage[i] = sss.SongLoaders.GetSong(i);
            }

            for (int i = 0; i < model.icons.Length; i++)
            {
                TextureContainer tex = new TextureContainer(md80, i);
                if (tex.icon_tex0 != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        tex.icon_tex0.GetImage(0).Save(ms, ImageFormat.Png);
                        model.icons[i] = ms.ToArray();
                    }
                }
            }

            for (int i = 0; i < definitions.Count; i++)
            {
                StagePair pair = definitions[i];
                model.pairs.Add(new ModelPair
                {
                    icon = pair.icon,
                    stage = pair.stage,
                    origId = i
                });
            }

            model.screen1 = sss.sss1;
            model.screen2 = sss.sss2;

            html = webBrowser1.DocumentText =
                RazorEngine.Engine.Razor.RunCompile(Resources.PairList, "PairList", typeof(PairListModel), model);
        }

        private void ReloadIfValidPac(string file, CustomSSSCodeset sssIfOtherFileValid = null)
        {
            ResourceNode node = NodeFactory.FromFile(null, file);
            ResourceNode p1icon = node.FindChild("MenSelmapCursorPly.1", true);
            BRRESNode candidate = p1icon != null ? p1icon.Parent.Parent as BRRESNode : null;
            if (candidate == null)
            {
                MessageBox.Show(this, "No SSS icons were found in the selected file.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (md80 != null)
                {
                    md80.Dispose();
                }

                md80 = candidate;
                sss = sssIfOtherFileValid ?? sss;
                ReloadData();
            }
        }

        #region Conversion to code text

        private string ToCodeLines(List<int> list)
        {
            StringBuilder sb = new StringBuilder();
            string[] s = list.Select(i => i.ToString("X2")).ToArray();
            for (int i = 0; i < s.Length; i += 8)
            {
                sb.Append("* ");
                for (int j = i; j < i + 4; j++)
                {
                    sb.Append(j < s.Length ? s[j] : "00");
                }

                sb.Append(" ");
                for (int j = i + 4; j < i + 8; j++)
                {
                    sb.Append(j < s.Length ? s[j] : "00");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        private string ToCodeLines(List<StagePair> definitions)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < definitions.Count; i += 4)
            {
                sb.Append("* ");
                for (int j = i; j < i + 2; j++)
                {
                    sb.Append(j < definitions.Count ? definitions[j].ToUshort().ToString("X4") : "0000");
                }

                sb.Append(" ");
                for (int j = i + 2; j < i + 4; j++)
                {
                    sb.Append(j < definitions.Count ? definitions[j].ToUshort().ToString("X4") : "0000");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string ToCode()
        {
            List<StagePair> definitions = getDefinitions();
            List<int> screen1 = getScreen1();
            List<int> screen2 = getScreen2();
            return string.Format(
                @"* 046B8F5C 7C802378
* 046B8F64 7C6300AE
* 040AF618 5460083C
* 040AF68C 38840002
* 040AF6AC 5463083C
* 040AF6C0 88030001
* 040AF6E8 3860FFFF
* 040AF59C 3860000C
* 060B91C8 00000018
* BFA10014 7CDF3378
* 7CBE2B78 7C7D1B78
* 2D05FFFF 418A0014
* 006B929C 000000{0}
* 066B99D8 000000{0}
{1}* 006B92A4 000000{2}
* 066B9A58 000000{2}
{3}* 06407AAC 000000{4}
{5}".Replace("\r\n", "\n").Replace("\n", Environment.NewLine),
                screen1.Count.ToString("X2"), ToCodeLines(screen1),
                screen2.Count.ToString("X2"), ToCodeLines(screen2),
                (2 * definitions.Count).ToString("X2"), ToCodeLines(definitions));
        }

        #endregion

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<StagePair> definitions = getDefinitions();
            if (tabControl1.SelectedTab == tabPreview1)
            {
                sssPrev1.MiscData80 = md80;
                sssPrev1.IconOrder = (from p in getScreen1()
                                      select definitions[p].icon).ToArray();
                sssPrev1.NumIcons = sssPrev1.IconOrder.Length;
            }
            else if (tabControl1.SelectedTab == tabPreview2)
            {
                sssPrev2.MiscData80 = md80;
                sssPrev2.IconOrder = (from p in getScreen2()
                                      select definitions[p].icon).ToArray();
                sssPrev2.NumIcons = sssPrev2.IconOrder.Length;
            }
            else if (tabControl1.SelectedTab == tabMyMusic1)
            {
                myMusic1.MiscData80 = md80;
                myMusic1.IconOrder = (from p in getScreen1()
                                      where p != 0x1E
                                      select definitions[p].icon).ToArray();
                myMusic1.NumIcons = myMusic1.IconOrder.Length;
            }
            else if (tabControl1.SelectedTab == tabMyMusic2)
            {
                myMusic2.MiscData80 = md80;
                myMusic2.IconOrder = (from p in getScreen2()
                                      where p != 0x1E
                                      select definitions[p].icon).Concat(
                    from b in new byte[] {0x64}
                    select b).ToArray();
                myMusic2.NumIcons = myMusic2.IconOrder.Length;
            }
        }

        private void openCodesetgcttxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Ocarina codes (*.gct, *.txt)|*.gct;*.txt";
                dialog.Multiselect = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sss = new CustomSSSCodeset(dialog.FileName);
                    ReloadData();
                }
            }
        }

        private void openStageIconspacbrresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Brawl data files (*.pac, *.brres)|*.pac;*.brres";
                dialog.Multiselect = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ReloadIfValidPac(dialog.FileName);
                }
            }
        }

        private void openSDCardRootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    CustomSSSCodeset candidateSSS = null;

                    string[] gctPaths = new string[]
                    {
                        "codes/RSBE01.gct",
                        "data/gecko/codes/RSBE01.gct",
                        "LegacyTE/RSBE01.gct",
                        "LegacyXP/RSBE01.gct",
                        "RSBE01.gct"
                    };
                    foreach (string gctPath in gctPaths)
                    {
                        if (File.Exists(dialog.SelectedPath + "/" + gctPath))
                        {
                            candidateSSS = new CustomSSSCodeset(dialog.SelectedPath + "/" + gctPath);
                            break;
                        }
                    }

                    if (candidateSSS == null)
                    {
                        MessageBox.Show(this, "Could not find one of: " + string.Join(", ", gctPaths),
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string root = null;
                    foreach (string folder in new string[]
                        {"/private/wii/app/RSBE/pf", "/projectm/pf", "/minusery/pf", "/LegacyTE/pf", "/LegacyXP/pf"})
                    {
                        foreach (string file in new string[]
                        {
                            "/menu2/sc_selmap.pac", "/menu2/sc_selmap_en.pac", "system/common5.pac",
                            "system/common5_en.pac"
                        })
                        {
                            if (File.Exists(dialog.SelectedPath + folder + "/" + file))
                            {
                                root = dialog.SelectedPath + folder + "/" + file;
                                break;
                            }
                        }
                    }

                    if (root == null)
                    {
                        MessageBox.Show(this, "Could not find common5 or sc_selmap.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    ReloadIfValidPac(root, candidateSSS);
                }
            }
        }

        private void saveCodesetgctToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sss.IgnoredMetadata)
            {
                MessageBox.Show(
                    "Warning: extra data was found after the GCT footer (probably code titles placed there by BrawlBox) and will be discarded if you continue to save.");
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Ocarina codes (*.gct)|*.gct";
                dialog.OverwritePrompt = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream fs = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
                    {
                        foreach (byte[] b in new byte[][]
                        {
                            sss.DataBefore, ByteUtilities.StringToByteArray(ToCode()), sss.DataAfter
                        })
                        {
                            fs.Write(b, 0, b.Length);
                        }
                    }
                }
            }
        }

        private void saveSSSCodeOnlytxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Text files (*.txt)|*.txt";
                dialog.OverwritePrompt = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(dialog.FileName, ToCode());
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void viewCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Form f = new Form() {Text = "Custom SSS Code"})
            {
                TextBox t = new TextBox()
                {
                    Multiline = true,
                    Dock = DockStyle.Fill,
                    ScrollBars = ScrollBars.Vertical,
                    Text = ToCode(),
                    Font = new Font("Consolas", 12)
                };
                f.Controls.Add(t);
                f.ShowDialog(this);
            }
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Form f = new Form())
            {
                using (WebBrowser w = new WebBrowser())
                {
                    w.Dock = DockStyle.Fill;
                    f.Controls.Add(w);
                    w.DocumentText = Resources.About;
                    f.Text = "About SSS Editor";
                    f.Width = 600;
                    f.Height = 400;
                    f.ShowDialog();
                }
            }
        }

        private void pasteAnSSSCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Form f = new Form() {Text = "Custom SSS Code"})
            {
                Button ok = new Button()
                {
                    Text = "OK",
                    Dock = DockStyle.Bottom,
                    DialogResult = DialogResult.OK
                };
                f.Controls.Add(ok);
                TextBox t = new TextBox()
                {
                    Multiline = true,
                    Dock = DockStyle.Fill,
                    ScrollBars = ScrollBars.Vertical,
                    Font = new Font("Consolas", 12),
                };
                f.Controls.Add(t);
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    sss = new CustomSSSCodeset(t.Lines);
                    ReloadData();
                }
            }
        }

        private void exportHTMLToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog d = new SaveFileDialog())
            {
                d.AddExtension = true;
                d.Filter = "HTML files|*.htm;*.html";
                if (d.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(d.FileName, html);
                }
            }
        }
    }
}