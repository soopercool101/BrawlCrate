using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.CostumeManager.Portrait_Viewers
{
    public class InfoStockIconViewer : PortraitViewer
    {
        private int _charNum, _costumeNum;

        private static PortraitViewerTextureData[] textureData =
        {
            new PortraitViewerTextureData(32, 32,
                (i, j) => "Misc Data [30]/Textures(NW4R)/InfStc." + (i * 10 + j + 1).ToString("D3"))
        };

        private string _openFilePath;
        private FlowLayoutPanel additionalTexturesPanel;
        private Button saveButton;

        /// <summary>
        /// The info.pac file currently being used.
        /// </summary>
        private ResourceNode info_en;

        public InfoStockIconViewer()
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
        }

        public override bool UpdateImage(int charNum, int costumeNum)
        {
            _charNum = charNum;
            _costumeNum = costumeNum;
            foreach (PortraitViewerTextureData atd in textureData)
            {
                atd.TextureFrom(info_en, charNum, costumeNum);
            }

            return true;
        }

        public override void UpdateDirectory()
        {
            if (File.Exists("../info2/info.pac"))
            {
                string path = "../info2/info.pac";
                info_en = NodeFactory.FromFile(null, path);
                _openFilePath = path;
            }
            else if (File.Exists("../info2/info_en.pac"))
            {
                string path = "../info2/info_en.pac";
                info_en = NodeFactory.FromFile(null, path);
                _openFilePath = path;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (info_en == null)
            {
                return;
            }

            info_en.Merge();
            info_en.Export(_openFilePath);
        }

        private void InitializeComponent()
        {
            additionalTexturesPanel = new FlowLayoutPanel();
            saveButton = new Button();
            SuspendLayout();
            // 
            // additionalTexturesPanel
            // 
            additionalTexturesPanel.Dock = DockStyle.Left;
            additionalTexturesPanel.Location = new Point(0, 0);
            additionalTexturesPanel.Margin = new Padding(0);
            additionalTexturesPanel.Name = "additionalTexturesPanel";
            additionalTexturesPanel.Size = new Size(72, 48);
            additionalTexturesPanel.TabIndex = 1;
            // 
            // saveButton
            // 
            saveButton.Dock = DockStyle.Fill;
            saveButton.Location = new Point(72, 0);
            saveButton.Margin = new Padding(0);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(56, 48);
            saveButton.TabIndex = 2;
            saveButton.Text = "Save info.pac";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += new EventHandler(saveButton_Click);
            // 
            // InfoStockIconViewer
            // 
            Controls.Add(saveButton);
            Controls.Add(additionalTexturesPanel);
            Name = "InfoStockIconViewer";
            Size = new Size(128, 48);
            ResumeLayout(false);
        }
    }
}