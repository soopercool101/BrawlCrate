using BrawlLib.Internal.Drawing;
using BrawlLib.Internal.IO;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Textures;
using System.Drawing;

namespace BrawlCrate.BrawlManagers.StageManager
{
    public static class TEX0Extension
    {
        public static void Replace(this TEX0Node tex0, Bitmap bmp, int paletteSize)
        {
            FileMap tMap, pMap;
            if (tex0.HasPalette)
            {
                PLT0Node pn = tex0.GetPaletteNode();
                tMap = TextureConverter.Get(tex0.Format).EncodeTextureIndexed(bmp, tex0.LevelOfDetail, paletteSize,
                    pn.Format, QuantizationAlgorithm.MedianCut, out pMap);
                pn.ReplaceRaw(pMap);
            }
            else
            {
                tMap = TextureConverter.Get(tex0.Format).EncodeTEX0Texture(bmp, tex0.LevelOfDetail);
            }

            tex0.ReplaceRaw(tMap);
        }

        public static string ToSizeString(this TEX0Node tex0)
        {
            if (tex0 == null)
            {
                return "null";
            }

            return tex0.Width + "x" + tex0.Height;
        }
    }
}