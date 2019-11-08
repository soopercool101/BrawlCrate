using BrawlLib.SSBB.ResourceNodes;
using System.Collections.Generic;
using System.Linq;

namespace BrawlLib.BrawlManagerLib
{
    public class TextureContainer : IEnumerable<TEX0Node>
    {
        public class Texture
        {
            public TEX0Node tex0;
            public PAT0TextureEntryNode pat0;
            public bool ForThisFrameIndex;
        }

        public Texture prevbase;
        public Texture icon;
        public Texture frontstname;
        public Texture frontstname_shadow;
        public Texture seriesicon;
        public Texture selmap_mark;
        public Texture line;

        #region Backwards-compatibility - depreciated

        public TEX0Node prevbase_tex0 => prevbase.ForThisFrameIndex ? prevbase.tex0 : null;
        public TEX0Node icon_tex0 => icon.ForThisFrameIndex ? icon.tex0 : null;
        public TEX0Node frontstname_tex0 => frontstname.ForThisFrameIndex ? frontstname.tex0 : null;
        public TEX0Node seriesicon_tex0 => seriesicon.ForThisFrameIndex ? seriesicon.tex0 : null;
        public TEX0Node selmap_mark_tex0 => selmap_mark.ForThisFrameIndex ? selmap_mark.tex0 : null;
        public PAT0TextureEntryNode prevbase_pat0 => prevbase.pat0;
        public PAT0TextureEntryNode icon_pat0 => icon.pat0;
        public PAT0TextureEntryNode frontstname_pat0 => frontstname.pat0;
        public PAT0TextureEntryNode seriesicon_pat0 => seriesicon.pat0;
        public PAT0TextureEntryNode selmap_mark_pat0 => selmap_mark.pat0;

        #endregion

        public ResourceNode TEX0Folder { get; private set; }
        private ResourceNode PAT0Folder;
        private int iconNum;

        public IEnumerator<TEX0Node> GetEnumerator()
        {
            return new List<TEX0Node> {prevbase.tex0, icon.tex0, frontstname.tex0, seriesicon.tex0, selmap_mark.tex0}
                .GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TextureContainer()
        {
        }

        /// <summary>
        /// Finds the textures through their PAT0 entries, given an icon number and a ResourceNode
        /// </summary>
        /// <param name="sc_selmap">The sc_selmap *or* Misc Data [80] node.</param>
        /// <param name="iconNum">The icon index (also used in the third part of the Custom SSS code.)</param>
        public TextureContainer(ResourceNode node, int iconNum)
        {
            TEX0Folder = node.FindChild("Misc Data [80]/Textures(NW4R)", false)
                         ?? node.FindChild("Textures(NW4R)", false);
            PAT0Folder = node.FindChild("Misc Data [80]/AnmTexPat(NW4R)", false)
                         ?? node.FindChild("AnmTexPat(NW4R)", false);
            this.iconNum = iconNum;

            populate(out prevbase, "MenSelmapPreview/basebgM");
            populate(out icon, "MenSelmapIcon/iconM");
            populate(out frontstname, "MenSelmapPreview/pasted__stnameM");
            populate(out frontstname_shadow, "MenSelmapPreview/pasted__stnameshadowM");
            populate(out seriesicon, "MenSelmapPreview/lambert113");
            populate(out selmap_mark, "MenSelmapPreview/pasted__stnamelogoM");
            populate(out line, "MenSelmapPreview/base_vertexcolorM1");
        }

        private void populate(out Texture tex, string path)
        {
            tex = new Texture
            {
                tex0 = null,
                ForThisFrameIndex = false,
                pat0 = null
            };
            if (iconNum == 255)
            {
                return;
            }

            if (PAT0Folder == null)
            {
                return;
            }

            IEnumerable<PAT0TextureEntryNode> query = from n in PAT0Folder.FindChild(path, false).Children[0].Children
                                                      let p = (PAT0TextureEntryNode) n
                                                      orderby p.FrameIndex descending
                                                      where p != null
                                                            && p.FrameIndex <= iconNum
                                                      select p;
            if (query.Any())
            {
                PAT0TextureEntryNode first = query.First();
                tex.tex0 = TEX0Folder.FindChild(first.Name, false) as TEX0Node;
                tex.ForThisFrameIndex = first.FrameIndex == iconNum;
                tex.pat0 = tex.ForThisFrameIndex ? first : null;
            }
        }
    }
}