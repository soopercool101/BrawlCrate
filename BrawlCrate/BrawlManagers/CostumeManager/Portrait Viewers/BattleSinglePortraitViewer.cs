using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.IO;

namespace BrawlCrate.BrawlManagers.CostumeManager.Portrait_Viewers
{
    public class BattleSinglePortraitViewer : SinglePortraitViewer
    {
        public override int PortraitWidth => 48;

        public override int PortraitHeight => 56;

        public override ResourceNode PortraitRootFor(int charNum, int costumeNum)
        {
            if (costumeNum < 0)
            {
                return null;
            }

            string tex_number = (charNum * 10 + costumeNum + 1).ToString("D3");
            int index = charNum * 10 + costumeNum + 1;
            ResourceNode bres;
            if (!bres_cache.TryGetValue(index, out bres))
            {
                string f = "../info/portrite/InfFace" + tex_number + ".brres";
                if (new FileInfo(f).Exists)
                {
                    bres_cache[index] = bres = (BRRESNode) NodeFactory.FromFile(null, f);
                }

                if (bres == null)
                {
                    label1.Text = "InfFace" + tex_number + ".brres: not found";
                    return null;
                }
            }

            return bres;
        }

        public override ResourceNode MainTEX0For(ResourceNode node, int charNum, int costumeNum)
        {
            return node.FindChild("Textures(NW4R)", false).Children[0];
        }

        private Dictionary<int, ResourceNode> bres_cache;

        public BattleSinglePortraitViewer() : base()
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
                    bres_cache[i].Export("../info/portrite/InfFace" + i.ToString("D3") + ".brres");
                }
            }
        }
    }
}