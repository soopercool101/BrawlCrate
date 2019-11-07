using BrawlLib.BrawlManagerLib;
using BrawlLib.BrawlManagerLib.GCT.ReadWrite;
using BrawlLib.SSBB.ResourceNodes;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace BrawlCrate.BrawlManagers.StageManager
{
    public static class IconsToMenumain
    {
        public static void Copy(ResourceNode scSelmap, ResourceNode muMenumain, CustomSSSCodeset sss)
        {
            ResourceNode miscData0 = muMenumain.FindChild("Misc Data [0]", false);
            List<ResourceNode> chrToKeep = miscData0.FindChild("AnmChr(NW4R)", false).Children;
            Dictionary<string, string> tempFiles = new Dictionary<string, string>(chrToKeep.Count);
            foreach (ResourceNode n in chrToKeep)
            {
                string file = TempFiles.Create(".chr0");
                tempFiles.Add(n.Name, file);
                n.Export(file);
            }

            ResourceNode miscData80 = scSelmap.FindChild("Misc Data [80]", false);
            miscData0.ReplaceRaw(miscData80.WorkingSource.Address, miscData80.WorkingSource.Length);
            miscData0.SignalPropertyChange();

            List<ResourceNode> chrToReplace = miscData0.FindChild("AnmChr(NW4R)", false).Children;
            foreach (ResourceNode n in chrToReplace)
            {
                string file = tempFiles[n.Name];
                n.Replace(file);
            }

            string xx_png = TempFiles.Create(".png");
            ResourceNode xx = miscData0.FindChild("Textures(NW4R)/MenSelmapIcon.XX", false);
            bool found = false;
            if (xx != null)
            {
                xx.Export(xx_png);
                found = true;
            }
            else
            {
                Stream stream = Assembly.GetExecutingAssembly()
                                        .GetManifestResourceStream("BrawlCrate.StageManager.XX.png");
                if (stream != null)
                {
                    Image.FromStream(stream).Save(xx_png);
                    found = true;
                }
            }

            if (found)
            {
                foreach (ResourceNode tex in miscData0.FindChild("Textures(NW4R)", false).Children)
                {
                    byte icon_id;
                    if (tex.Name.StartsWith("MenSelmapIcon.") && byte.TryParse(tex.Name.Substring(14, 2), out icon_id))
                    {
                        byte stage_id = sss.StageForIcon(icon_id);
                        if (icon_id != 100 && (stage_id == 0x25 || stage_id > 0x33))
                        {
                            tex.Replace(xx_png);
                        }
                    }
                }
            }

            File.Delete(xx_png);
        }
    }
}