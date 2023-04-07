using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;

namespace BrawlLib.SSBB
{
    public static class FileFilters
    {
        public static string ARCExport =
            SupportedFilesHandler.GetCompleteFilter("pac", "pcs", "pair", "mrg", "mariopast", "metalgear", "tengan",
                "village");

        public static string ARCImport =
            SupportedFilesHandler.GetCompleteFilter("pac", "pcs", "mrg");

        public static string MRGExport =
            SupportedFilesHandler.GetCompleteFilter("mrg", "pac", "pcs", "pair");

        public static string MRGImport =
            SupportedFilesHandler.GetCompleteFilter("mrg", "pac", "pcs");

        public static string U8Export =
            SupportedFilesHandler.GetCompleteFilter("arc", "szs", "pair");

        public static string U8Import =
            SupportedFilesHandler.GetCompleteFilter("arc", "szs");

        public static string BRES =
            SupportedFilesHandler.GetCompleteFilter("brres", "branm", "brmdl", "brtex", "brplt", "brcha", "brtsa",
                "brsha", "brvia", "brtpa", "brcla", "brsca");

        public static string MDL0Import =
            SupportedFilesHandler.GetCompleteFilter("mdl0", "pmd", "dae");

        public static string MDL0Export =
            SupportedFilesHandler.GetCompleteFilter("mdl0", "dae");

        public static string TEX0 =
            SupportedFilesHandler.GetCompleteFilter("png", "tga", "tif", "bmp", "jpg", "gif", "tex0");

        public static string PLT0 =
            SupportedFilesHandler.GetCompleteFilter("plt0");

        public static string CHR0Import =
            SupportedFilesHandler.GetCompleteFilter("chr0", "anim", "txt", "json");

        public static string CHR0Export =
            SupportedFilesHandler.GetCompleteFilter("chr0", "anim");

        public static string CLR0 =
            SupportedFilesHandler.GetCompleteFilter("clr0");

        public static string PAT0 =
            SupportedFilesHandler.GetCompleteFilter("pat0");

        public static string VIS0 =
            SupportedFilesHandler.GetCompleteFilter("vis0");

        public static string SRT0 =
            SupportedFilesHandler.GetCompleteFilter("srt0");

        public static string SCN0 =
            SupportedFilesHandler.GetCompleteFilter("scn0");

        public static string SHP0 =
            SupportedFilesHandler.GetCompleteFilter("shp0");

        public static string MSBin =
            SupportedFilesHandler.GetCompleteFilter("msbin", "txt");

        public static string RSTM =
            SupportedFilesHandler.GetCompleteFilter("brstm", "bcstm", "bfstm", "wav");

        public static string RWSD =
            SupportedFilesHandler.GetCompleteFilter("brwsd");

        public static string RBNK =
            SupportedFilesHandler.GetCompleteFilter("brbnk");

        public static string RSEQ =
            SupportedFilesHandler.GetCompleteFilter("brseq");

        public static string REFF =
            SupportedFilesHandler.GetCompleteFilter("breff");

        public static string REFT =
            SupportedFilesHandler.GetCompleteFilter("breft");

        public static string Images =
            SupportedFilesHandler.GetCompleteFilter("png", "tga", "tif", "bmp", "jpg", "gif");

        public static string EFLS =
            SupportedFilesHandler.GetCompleteFilter("efls");

        public static string CollisionDef =
            SupportedFilesHandler.GetCompleteFilter("coll");

        public static string REL =
            SupportedFilesHandler.GetCompleteFilter("rel");

        public static string DOL =
            SupportedFilesHandler.GetCompleteFilter("dol");

        public static string RSAR =
            SupportedFilesHandler.GetCompleteFilter("brsar");

        public static string TPL =
            SupportedFilesHandler.GetCompleteFilter("tpl");

        public static string Object =
            SupportedFilesHandler.GetCompleteFilter("obj");

        public static string WAV =
            SupportedFilesHandler.GetCompleteFilter("wav");

        public static string BLOC =
            SupportedFilesHandler.GetCompleteFilter("bloc");

        public static string FMDL =
            SupportedFilesHandler.GetCompleteFilter("fmdl");

        // Brawl Stage Files
        public static string STPM =
            SupportedFilesHandler.GetCompleteFilter("stpm");

        public static string STDT =
            SupportedFilesHandler.GetCompleteFilter("stdt");

        public static string SCLA =
            SupportedFilesHandler.GetCompleteFilter("scla");

        public static string TBCL =
            SupportedFilesHandler.GetCompleteFilter("tbcl");

        public static string TBGC =
            SupportedFilesHandler.GetCompleteFilter("tbgc");

        public static string TBGD =
            SupportedFilesHandler.GetCompleteFilter("tbgd");

        public static string TBGM =
            SupportedFilesHandler.GetCompleteFilter("tbgm");

        public static string TBLV =
            SupportedFilesHandler.GetCompleteFilter("tblv");

        public static string TBRM =
            SupportedFilesHandler.GetCompleteFilter("tbrm");

        public static string TBST =
            SupportedFilesHandler.GetCompleteFilter("tbst");

        // Subspace files
        public static string ADSJ =
            SupportedFilesHandler.GetCompleteFilter("adsj");

        public static string GEG1 =
            SupportedFilesHandler.GetCompleteFilter("geg1");

        // MDL0 subnodes
        public static string MDL0Material =
            SupportedFilesHandler.GetCompleteFilter("mdl0mat");

        public static string MDL0Shader =
            SupportedFilesHandler.GetCompleteFilter("mdl0shade");

        public static string Raw =
            SupportedFilesHandler.GetCompleteFilter("*");

        //Some files already have an extension in their name,
        //or sometimes the user will want to add the extension themselves.
        //Not only that, but '.dat' might be assigned to something else on their computer.
        //It's possible to assign a program (like a hex editor) to open files without extensions.

        public static string Havok =
            SupportedFilesHandler.GetCompleteFilter("hkx", "xml");

        public static string COSC =
            SupportedFilesHandler.GetCompleteFilter("dat", "bx");

        public static string CSSC =
            SupportedFilesHandler.GetCompleteFilter("dat", "bx");

        public static string FCFG =
            SupportedFilesHandler.GetCompleteFilter("dat", "bx");

        public static string RSTC =
            SupportedFilesHandler.GetCompleteFilter("bx", "dat");

        public static string SLTC =
            SupportedFilesHandler.GetCompleteFilter("dat", "bx");

        public static string MASQ =
            SupportedFilesHandler.GetCompleteFilter("masq");

        public static string CMM =
            SupportedFilesHandler.GetCompleteFilter("cmm");

        public static string MDef =
            SupportedFilesHandler.GetCompleteFilter("moveset");

        public static string GCT =
            SupportedFilesHandler.GetCompleteFilter("gct", "txt");

        public static string ASLS =
            SupportedFilesHandler.GetCompleteFilter("asl");

        public static string STEX =
            SupportedFilesHandler.GetCompleteFilter("param");

        public static string TLST =
            SupportedFilesHandler.GetCompleteFilter("tlst");

        public static string ITOV =
            SupportedFilesHandler.GetCompleteFilter("itov");

        public static string APIScripts =
            SupportedFilesHandler.GetCompleteFilter("py", "fsx");

        /// <summary>
        /// Maps node types to the default extension when using Export All.
        /// Nodes that are inside a BRES do not need to be defined here - they will get an extension assigned in BRRESNode.cs.
        /// </summary>
        private static readonly Dictionary<Type, string> DefaultExportAllExtensions = new Dictionary<Type, string>
        {
            [typeof(MDL0Node)] = "mdl0",
            [typeof(TEX0Node)] = "png",
            [typeof(PLT0Node)] = "plt0",
            [typeof(CHR0Node)] = "chr0",
            [typeof(CLR0Node)] = "clr0",
            [typeof(PAT0Node)] = "pat0",
            [typeof(VIS0Node)] = "vis0",
            [typeof(SRT0Node)] = "srt0",
            [typeof(SCN0Node)] = "scn0",
            [typeof(SHP0Node)] = "shp0",
            [typeof(MSBinNode)] = "msbin",
            [typeof(RSTMNode)] = "brstm",
            [typeof(RWSDNode)] = "brwsd",
            [typeof(RBNKNode)] = "brbnk",
            [typeof(RSEQNode)] = "brseq",
            [typeof(REFFNode)] = "breff",
            [typeof(REFTNode)] = "breft",
            [typeof(EFLSNode)] = "efls",
            [typeof(CollisionNode)] = "coll",
            [typeof(RELNode)] = "rel",
            [typeof(DOLNode)] = "dol",
            [typeof(RSARNode)] = "brsar",
            [typeof(TPLNode)] = "tpl",
            [typeof(MDL0ObjectNode)] = "obj",
            [typeof(BLOCNode)] = "bloc",
            [typeof(STPMNode)] = "stpm",
            [typeof(STDTNode)] = "stdt",
            [typeof(SCLANode)] = "scla",
            [typeof(HavokNode)] = "hkx"
        };

        public static string GetDefaultExportAllExtension(Type type)
        {
            string ext = null;
            while (type != null)
            {
                if (DefaultExportAllExtensions.TryGetValue(type, out ext))
                {
                    if (!ext.StartsWith("."))
                    {
                        ext = "." + ext;
                    }

                    break;
                }

                type = type.BaseType;
            }

            return ext ?? "";
        }
    }
}