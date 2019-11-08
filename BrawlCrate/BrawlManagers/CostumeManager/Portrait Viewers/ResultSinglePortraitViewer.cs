using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.IO;

namespace BrawlCrate.BrawlManagers.CostumeManager.Portrait_Viewers
{
    public class ResultSinglePortraitViewer : SinglePortraitViewer
    {
        public override int PortraitWidth => 128;

        public override int PortraitHeight => 160;

        public override ResourceNode PortraitRootFor(int charNum, int costumeNum)
        {
            ResourceNode bres;
            if (!bres_cache.TryGetValue(charNum, out bres))
            {
                string f = "../menu/common/char_bust_tex/MenSelchrFaceB" + charNum.ToString("D2") + "0.brres";
                if (new FileInfo(f).Exists)
                {
                    bres_cache[charNum] = bres = (BRRESNode) NodeFactory.FromFile(null, f);
                }

                if (bres == null)
                {
                    label1.Text = "MenSelchrFaceB" + charNum.ToString("D2") + "0: not found";
                    return null;
                }
            }

            return bres;
        }

        public override ResourceNode MainTEX0For(ResourceNode brres, int charNum, int costumeNum)
        {
            string path = "Textures(NW4R)/MenSelchrFaceB." + (charNum * 10 + costumeNum + 1).ToString("D3");
            return brres.FindChild(path, false);
        }

        private Dictionary<int, ResourceNode> bres_cache;

        public ResultSinglePortraitViewer() : base()
        {
            UpdateDirectory();
        }

        public override void UpdateDirectory()
        {
            if (bres_cache != null)
            {
                foreach (ResourceNode node in bres_cache.Values)
                {
                    node?.Dispose();
                }
            }

            bres_cache = new Dictionary<int, ResourceNode>();
        }

        protected override void saveButton_Click(object sender, EventArgs e)
        {
            foreach (int i in bres_cache.Keys)
            {
                if (bres_cache[i] != null && bres_cache[i].IsDirty)
                {
                    bres_cache[i].Merge();
                    bres_cache[i].Export("../menu/common/char_bust_tex/MenSelchrFaceB" + i.ToString("D2") + "0.brres");
                }
            }
        }
    }
}