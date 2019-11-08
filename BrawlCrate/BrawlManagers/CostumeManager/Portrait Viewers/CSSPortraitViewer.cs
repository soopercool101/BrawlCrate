using BrawlLib.BrawlManagerLib;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.CostumeManager.Portrait_Viewers
{
    public class CSSPortraitViewer : PortraitViewer
    {
        private int _charNum, _costumeNum;

        private bool _namePortraitPreview;

        public bool NamePortraitPreview
        {
            get => _namePortraitPreview;
            set
            {
                _namePortraitPreview = value;
                UpdateImage(_charNum, _costumeNum);
            }
        }

        private static PortraitViewerTextureData[] textureData =
        {
            new PortraitViewerTextureData(128, 160,
                (i, j) => "char_bust_tex_lz77/Misc Data [" + i + "]/Textures(NW4R)/MenSelchrFaceB." +
                          (i * 10 + j + 1).ToString("D3")),
            new PortraitViewerTextureData(128, 32,
                (i, j) => "Misc Data [30]/Textures(NW4R)/MenSelchrChrNm." + i.ToString("D2") + "1"),
            new PortraitViewerTextureData(80, 56,
                (i, j) => "Misc Data [70]/Textures(NW4R)/MenSelchrChrFace." + (i < 47 ? i + 1 : i).ToString("D3")),
            new PortraitViewerTextureData(32, 32,
                (i, j) => "Misc Data [90]/Textures(NW4R)/InfStc." + (i * 10 + j + 1).ToString("D3")),
            new PortraitViewerTextureData(56, 14,
                (i, j) => "Misc Data [70]/Textures(NW4R)/MenSelchrChrNmS." + (i < 47 ? i + 1 : i).ToString("D3"))
        };

        private string _openFilePath;

        /// <summary>
        /// The common5 currently being used. If using sc_selcharacter.pac instead, this will be null.
        /// </summary>
        private ResourceNode common5;

        private Label label1;
        private FlowLayoutPanel additionalTexturesPanel;
        private Button saveButton;

        /// <summary>
        /// Either the sc_selcharacter_en archive within common5.pac or the sc_selcharacter.pac file.
        /// </summary>
        private ResourceNode sc_selcharacter;

        public CSSPortraitViewer()
        {
            InitializeComponent();
            foreach (PortraitViewerTextureData atd in textureData)
            {
                additionalTexturesPanel.Controls.Add(atd.TexturePanel);
                atd.OnUpdate = delegate(PortraitViewerTextureData sender) { UpdateImage(_charNum, _costumeNum); };

                ToolStripMenuItem copyPreview = new ToolStripMenuItem("Copy preview");
                copyPreview.Click += (o, e) => Clipboard.SetImage(atd.TexturePanel.BackgroundImage);
                atd.TexturePanel.ContextMenuStrip.Items.Add(copyPreview);
            }

            UpdateDirectory();
            label1.Text = common5 != null ? "common5" : "sc_selcharacter";
        }

        public override bool UpdateImage(int charNum, int costumeNum)
        {
            _charNum = charNum;
            _costumeNum = costumeNum;
            foreach (PortraitViewerTextureData atd in textureData)
            {
                atd.TextureFrom(sc_selcharacter, charNum, costumeNum);
            }

            if (NamePortraitPreview && textureData[0].Texture != null)
            {
                OverlayName();
            }

            return true;
        }

        public void ReplaceMain(string filename, bool useTextureConverter)
        {
            textureData[0].Replace(filename, useTextureConverter);
        }

        private void OverlayName()
        {
            Image orig = textureData[0].TexturePanel.BackgroundImage;

            Bitmap name = new Bitmap(textureData[1].Texture.GetImage(0));
            Bitmap swapped = BitmapUtilities.AlphaSwap(name);
            Bitmap blurred = BitmapUtilities.BlurCombine(swapped, Color.Black);

            Bitmap overlaid = new Bitmap(orig.Width, orig.Height);
            Graphics g = Graphics.FromImage(overlaid);
            g.DrawImage(orig,
                new Rectangle(0, 0, 128, 128),
                new Rectangle(0, 0, 128, 128),
                GraphicsUnit.Pixel);
            g.DrawImage(blurred, new Point[]
            {
                new Point(0, 98),
                new Point(131, 98),
                new Point(-3, 127)
            });
            textureData[0].TexturePanel.BackgroundImage = overlaid;
        }

        public override void UpdateDirectory()
        {
            if (File.Exists("../menu2/sc_selcharacter.pac"))
            {
                string path = "../menu2/sc_selcharacter.pac";
                common5 = null;
                sc_selcharacter = NodeFactory.FromFile(null, path);
                _openFilePath = path;
            }
            else if (File.Exists("../menu2/sc_selcharacter_en.pac"))
            {
                string path = "../menu2/sc_selcharacter_en.pac";
                common5 = null;
                sc_selcharacter = NodeFactory.FromFile(null, path);
                _openFilePath = path;
            }
            else if (File.Exists("../system/common5.pac"))
            {
                string path = "../system/common5.pac";
                common5 = NodeFactory.FromFile(null, path);
                sc_selcharacter = common5.FindChild("sc_selcharacter_en", false);
                _openFilePath = path;
            }
            else if (File.Exists("../system/common5_en.pac"))
            {
                string path = "../system/common5_en.pac";
                common5 = NodeFactory.FromFile(null, path);
                sc_selcharacter = common5.FindChild("sc_selcharacter_en", false);
                _openFilePath = path;
            }
            else
            {
                common5 = null;
                sc_selcharacter = null;
            }

            label1.Text = sc_selcharacter != null
                ? Path.GetFileName(_openFilePath)
                : "Could not load common5 or sc_selcharacter.";
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (sc_selcharacter == null)
            {
                return;
            }

            if (common5 != null)
            {
                common5.Merge();
                common5.Export(_openFilePath);
            }
            else
            {
                sc_selcharacter.Merge();
                sc_selcharacter.Export(_openFilePath);
            }
        }

        public void UpdateSSSStockIcons()
        {
            if (common5 == null)
            {
                MessageBox.Show(FindForm(), "common5.pac is not loaded - can't update automatically.\n" +
                                            "After saving sc_selcharacter.pac,  update the icons manually by replacing sc_selmap's " +
                                            "Misc Data [40] with sc_selcharacter's Misc Data [90].",
                    "Cannot perform operation",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ResourceNode css_stockicons = sc_selcharacter.FindChild("Misc Data [90]", false);
                string tempFile = Path.GetTempPath() + Guid.NewGuid() + ".brres";
                css_stockicons.Export(tempFile);
                ResourceNode sss_stockicons = common5.FindChild("sc_selmap_en/Misc Data [40]", false);
                sss_stockicons.Replace(tempFile);
                try
                {
                    File.Delete(tempFile);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void InitializeComponent()
        {
            label1 = new Label();
            additionalTexturesPanel = new FlowLayoutPanel();
            saveButton = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Dock = DockStyle.Top;
            label1.Location = new Point(0, 0);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(128, 40);
            label1.TabIndex = 0;
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // additionalTexturesPanel
            // 
            additionalTexturesPanel.AutoSize = true;
            additionalTexturesPanel.Dock = DockStyle.Top;
            additionalTexturesPanel.Location = new Point(0, 40);
            additionalTexturesPanel.Margin = new Padding(0);
            additionalTexturesPanel.Name = "additionalTexturesPanel";
            additionalTexturesPanel.Size = new Size(128, 0);
            additionalTexturesPanel.TabIndex = 1;
            // 
            // saveButton
            // 
            saveButton.Dock = DockStyle.Top;
            saveButton.Location = new Point(0, 40);
            saveButton.Margin = new Padding(0);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(128, 23);
            saveButton.TabIndex = 2;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += new EventHandler(saveButton_Click);
            // 
            // CSSPortraitViewer
            // 
            Controls.Add(saveButton);
            Controls.Add(additionalTexturesPanel);
            Controls.Add(label1);
            Name = "CSSPortraitViewer";
            Size = new Size(128, 326);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}