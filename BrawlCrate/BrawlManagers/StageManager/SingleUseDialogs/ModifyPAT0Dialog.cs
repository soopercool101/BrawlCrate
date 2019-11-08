using BrawlLib.BrawlManagerLib;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.StageManager.SingleUseDialogs
{
    public partial class ModifyPAT0Dialog : Form
    {
        private TextureContainer textures;

        private WiiPixelFormat[] SelmapMarkFormats = {WiiPixelFormat.I4, WiiPixelFormat.IA4, WiiPixelFormat.CMPR};

        private List<string> selchrList, selmapList;

        public ModifyPAT0Dialog(TextureContainer tx)
        {
            InitializeComponent();
            textures = tx;
            AcceptButton = btnOkay;

            if (textures.seriesicon.pat0 == null)
            {
                selchrBox.Enabled = false;
            }
            else
            {
                IEnumerable<string> i4 = from tex in textures.TEX0Folder.Children
                                         where tex is TEX0Node && ((TEX0Node) tex).Format == WiiPixelFormat.I4
                                         orderby !tex.Name.Contains("MenSelchrMark") &&
                                                 !tex.Name.Contains("SeriesIcon"), tex.Name
                                         select tex.Name;
                selchrList = i4.ToList();
                selchrBox.DataSource = selchrList;
                if (textures.seriesicon.tex0 != null)
                {
                    selchrBox.SelectedItem = textures.seriesicon.tex0.Name;
                }

                selchrBox.Enabled = true;
            }

            if (textures.selmap_mark.pat0 == null)
            {
                selmapBox.Enabled = false;
            }
            else
            {
                IEnumerable<string> ia4 = from tex in textures.TEX0Folder.Children
                                          where tex is TEX0Node && SelmapMarkFormats.Contains(((TEX0Node) tex).Format)
                                          orderby !tex.Name.Contains("MenSelmapMark"), tex.Name
                                          select tex.Name;
                selmapList = ia4.ToList();
                selmapBox.DataSource = selmapList;
                if (textures.selmap_mark.tex0 != null)
                {
                    selmapBox.SelectedItem = textures.selmap_mark.tex0.Name;
                }

                selmapBox.Enabled = true;
            }
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            if (selchrBox.SelectedItem == null)
            {
                selchrBox.SelectedIndex = selchrList.IndexOf(selchrBox.Text);
            }

            if (selmapBox.SelectedItem == null)
            {
                selmapBox.SelectedIndex = selmapList.IndexOf(selmapBox.Text);
            }

            if (selchrBox.Enabled && selchrBox.SelectedItem != null &&
                selchrBox.SelectedItem.ToString() != textures.seriesicon.pat0.Texture)
            {
                textures.seriesicon.pat0.Texture = selchrBox.SelectedItem.ToString();
                textures.seriesicon.pat0.IsDirty = true;
            }

            if (selmapBox.Enabled && selmapBox.SelectedItem != null &&
                selmapBox.SelectedItem.ToString() != textures.selmap_mark.pat0.Texture)
            {
                textures.selmap_mark.pat0.Texture = selmapBox.SelectedItem.ToString();
                textures.selmap_mark.pat0.IsDirty = true;
            }

            //Close();
        }
    }
}