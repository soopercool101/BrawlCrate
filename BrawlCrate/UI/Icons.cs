using BrawlCrate.Properties;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlCrate.UI
{
    public static class Icons
    {
        private static ImageList _imgList;

        public static ImageList ImageList
        {
            get
            {
                if (_imgList == null)
                {
                    _imgList = new ImageList
                    {
                        ImageSize = new Size(24, 24),
                        ColorDepth = ColorDepth.Depth32Bit
                    };
                    foreach (KeyValuePair<ResourceType, Bitmap> entry in IconDictionary)
                    {
                        _imgList.Images.Add(Enum.GetName(typeof(ResourceType), entry.Key), entry.Value);
                    }
                }

                return _imgList;
            }
        }

        private static readonly Dictionary<ResourceType, Bitmap> IconDictionary = new Dictionary<ResourceType, Bitmap>
        {
            //Base Types
            {ResourceType.Unknown, Resources.Unknown},
            {ResourceType.Container, Resources.Folder},

            //Archives
            {ResourceType.ARC, Resources.ARC},
            {ResourceType.U8, Resources.U8},
            {ResourceType.U8Folder, Resources.Folder},
            {ResourceType.BRES, Resources.BRES},
            {ResourceType.BFRESGroup, Resources.Folder},
            {ResourceType.MRG, Resources.Folder},
            {ResourceType.BLOC, Resources.BLOC},
            {ResourceType.Redirect, Resources.Redirect},
            {ResourceType.RARCFolder, Resources.Folder},

            //Effects
            {ResourceType.EFLS, Resources.EFLS},
            {ResourceType.REFF, Resources.REFF},
            {ResourceType.REFFEntry, Resources.REFFEntry},
            {ResourceType.REFT, Resources.REFT},
            {ResourceType.REFTImage, Resources.IMG},

            //Modules
            {ResourceType.REL, Resources.REL},

            //Misc
            {ResourceType.CollisionDef, Resources.Coll},
            {ResourceType.MSBin, Resources.MSG},
            {ResourceType.STPM, Resources.STPM},
            {ResourceType.STDT, Resources.STDT},
            {ResourceType.SCLA, Resources.SCLA},
            {ResourceType.SndBgmTitleDataFolder, Resources.Folder},
            {ResourceType.ClassicStageTbl, Resources.Folder},

            //AI
            {ResourceType.AI, Resources.AI},
            {ResourceType.CE, Resources.CE},
            {ResourceType.AIPD, Resources.AIPD},
            {ResourceType.ATKD, Resources.ATKD},

            //Textures
            {ResourceType.TPL, Resources.TPL},
            {ResourceType.TPLTexture, Resources.IMG},
            {ResourceType.TPLPalette, Resources.Palette},

            //NW4R
            {ResourceType.TEX0, Resources.TEX0},
            {ResourceType.SharedTEX0, Resources.SharedTEX0},
            {ResourceType.PLT0, Resources.PLT0},

            {ResourceType.MDL0, Resources.MDL0},
            {ResourceType.MDL0Group, Resources.Folder},

            {ResourceType.CHR0, Resources.CHR},

            {ResourceType.CLR0, Resources.CLR},

            {ResourceType.VIS0, Resources.VIS},
            {ResourceType.SCN0, Resources.SCN0},

            {ResourceType.SHP0, Resources.SHP},

            {ResourceType.SRT0, Resources.SRT},

            {ResourceType.PAT0, Resources.PAT},

            //Audio
            {ResourceType.RSAR, Resources.RSAR},
            {ResourceType.RSTM, Resources.RSTM},
            {ResourceType.RSARFile, Resources.S},
            {ResourceType.RSARGroup, Resources.G},
            {ResourceType.RSARType, Resources.T},
            {ResourceType.RSARBank, Resources.B},

            //Groups
            {ResourceType.BRESGroup, Resources.Folder},
            {ResourceType.RSARFolder, Resources.Folder},
            {ResourceType.RSARFileSoundGroup, Resources.Folder},
            {ResourceType.RWSDDataGroup, Resources.Folder},
            {ResourceType.RSEQGroup, Resources.Folder},
            {ResourceType.RBNKGroup, Resources.Folder},

            //Moveset
            {ResourceType.MDef, Resources.MDef},
            {ResourceType.NoEditFolder, Resources.Folder},
            {ResourceType.NoEditEntry, Resources.Folder},
            {ResourceType.MDefAction, Resources.MDefAction},
            {ResourceType.MDefActionGroup, Resources.Folder},
            {ResourceType.MDefSubActionGroup, Resources.Folder},
            {ResourceType.MDefMdlVisGroup, Resources.Folder},
            {ResourceType.MDefMdlVisRef, Resources.Folder},
            {ResourceType.MDefMdlVisSwitch, Resources.Folder},
            {ResourceType.MDefActionList, Resources.Folder},
            {ResourceType.MDefSubroutineList, Resources.Folder},
            {ResourceType.MDefActionOverrideList, Resources.Folder},
            {ResourceType.MDefHurtboxList, Resources.Folder},
            {ResourceType.MDefRefList, Resources.Folder},
            {ResourceType.Event, Resources.Event},

            //Nintendo Disc Image
            {ResourceType.DiscImagePartition, Resources.Folder},

            //J3D
            {ResourceType.BMDGroup, Resources.Folder},

            //Subspace Emmisary
            {ResourceType.GDOR, Resources.GDOR},
            {ResourceType.GEG1, Resources.GEG1},
            {ResourceType.ENEMY, Resources.ENEMY},
            {ResourceType.GMOV, Resources.GMOV},
            {ResourceType.GSND, Resources.GSND},
            {ResourceType.GMOT, Resources.GMOT},
            {ResourceType.ADSJ, Resources.ADSJ},
            {ResourceType.GBLK, Resources.GBLK},
            {ResourceType.GMPS, Resources.GMPS},
            {ResourceType.BGMG, Resources.BGMG},
            {ResourceType.GDBF, Resources.GDBF},
            {ResourceType.GWAT, Resources.GWAT},
            {ResourceType.GCAM, Resources.GCAM},
            {ResourceType.GITM, Resources.GITM},
            {ResourceType.GIB2, Resources.GIB2},

            {ResourceType.HavokGroup, Resources.Folder},

            //BrawlEx
            {ResourceType.RSTCGroup, Resources.Folder},

            // Items
            {ResourceType.ItemFreqNode, Resources.Folder},
            {ResourceType.ItemFreqTableNode, Resources.Folder},
            {ResourceType.ItemFreqTableGroupNode, Resources.Folder},
            {ResourceType.ItemFreqEntryNode, Resources.itembox},

            {ResourceType.Folder, Resources.Folder}
        };

        public static int getImageIndex(ResourceType resResourceFileType)
        {
            int result = ImageList.Images.IndexOfKey(Enum.GetName(typeof(ResourceType), resResourceFileType));
            return result == -1 ? 0 : result;
        }
    }
}