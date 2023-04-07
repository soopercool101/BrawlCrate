using BrawlLib.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BrawlLib.SSBB
{
    public static class SupportedFilesHandler
    {
        public static readonly SupportedFileInfo[] Files =
        {
            //Archives
            new SupportedFileInfo(true, "PAC File Archive", "pac"),
            new SupportedFileInfo(true, "PCS Compressed File Archive", "pcs"),
            new SupportedFileInfo(true, "U8 ARC File Archive", "arc"),
            new SupportedFileInfo(true, "U8 Compressed ARC File Archive", "carc"),
            new SupportedFileInfo(false, "Archive Pair (*.pac & *.pcs)", "pair"),
            new SupportedFileInfo(true, "MRG Resource Group", "mrg"),
            new SupportedFileInfo(true, "MRG Compressed Resource Group", "mrgc"),
            new SupportedFileInfo(true, "SZS Compressed Archive", "szs"),

            //NW4R Model Resources
            new SupportedFileInfo(true, "NW4R Resource Pack", "brres"),
            new SupportedFileInfo(true, "NW4R Animation Pack", "branm"),
            new SupportedFileInfo(true, "NW4R Model Pack", "brmdl"),
            new SupportedFileInfo(true, "NW4R Texture Pack", "brtex"),
            new SupportedFileInfo(true, "NW4R Palette Pack", "brplt"),
            new SupportedFileInfo(true, "NW4R Bone Animation Pack", "brcha"),
            new SupportedFileInfo(true, "NW4R Texture Animation Pack", "brtsa"),
            new SupportedFileInfo(true, "NW4R Vertex Morph Pack", "brsha"),
            new SupportedFileInfo(true, "NW4R Visibility Sequence Pack", "brvia"),
            new SupportedFileInfo(true, "NW4R Texture Pattern Pack", "brtpa"),
            new SupportedFileInfo(true, "NW4R Color Sequence Pack", "brcla"),
            new SupportedFileInfo(true, "NW4R Scene Settings Pack", "brsca"),

            //NW4R Raw
            new SupportedFileInfo(true, "NW4R Model", "mdl0"),
            new SupportedFileInfo(true, "NW4R Texture", "tex0"),
            new SupportedFileInfo(true, "NW4R Palette", "plt0"),
            new SupportedFileInfo(true, "NW4R Bone Animation", "chr0"),
            new SupportedFileInfo(true, "NW4R Texture Animation", "srt0"),
            new SupportedFileInfo(true, "NW4R Vertex Morph", "shp0"),
            new SupportedFileInfo(true, "NW4R Visibility Sequence", "vis0"),
            new SupportedFileInfo(true, "NW4R Texture Pattern", "pat0"),
            new SupportedFileInfo(true, "NW4R Color Sequence", "clr0"),
            new SupportedFileInfo(true, "NW4R Scene Settings", "scn0"),

            //NW4R Audio
            new SupportedFileInfo(true, "NW4R Audio Stream", "brstm"),
            new SupportedFileInfo(true, "NW4C Audio Stream", "bcstm"),
            new SupportedFileInfo(true, "NW4F Audio Stream", "bfstm"),
            new SupportedFileInfo(true, "NW4R Sound Archive", "brsar"),
            new SupportedFileInfo(true, "NW4R RSAR Sound File", "brwsd"),
            new SupportedFileInfo(true, "NW4R RSAR Sound Bank File", "brbnk"),
            new SupportedFileInfo(true, "NW4R RSAR Sound Sequence", "brseq"),

            //NW4R Effects
            new SupportedFileInfo(true, "Particle Effect List", "efls"), //Used only by Brawl?
            new SupportedFileInfo(true, "NW4R Particle Effects", "breff"),
            new SupportedFileInfo(true, "NW4R Particle Textures", "breft"),

            //Modules
            new SupportedFileInfo(true, "Static Module", "dol"),
            new SupportedFileInfo(true, "Relocatable Module", "rel"),

            //Revolution files
            new SupportedFileInfo(true, "RVL Texture Archive", "tpl"),
            new SupportedFileInfo(true, "RVL Audio/Video", "thp"),

            //Brawl-specific files
            new SupportedFileInfo(true, "Brawl Message Pack", "msbin"),

            //Brawl stage files
            new SupportedFileInfo(true, "Brawl Stage Collision File", "coll"),
            new SupportedFileInfo(true, "Brawl Stage Parameters File", "stpm"),
            new SupportedFileInfo(true, "Brawl Stage Trap Data Table File", "stdt"),
            new SupportedFileInfo(true, "Brawl Stage Collision Attributes File", "scla"),
            new SupportedFileInfo(true, "Brawl TBCL File", "tbcl"),
            new SupportedFileInfo(true, "Brawl TBGC File", "tbgc"),
            new SupportedFileInfo(true, "Brawl TBGD File", "tbgd"),
            new SupportedFileInfo(true, "Brawl TBGM File", "tbgm"),
            new SupportedFileInfo(true, "Brawl TBLV File", "tblv"),
            new SupportedFileInfo(true, "Brawl TBRM File", "tbrm"),
            new SupportedFileInfo(true, "Brawl TBST File", "tbst"),

            //Multi-file stages
            new SupportedFileInfo(false, "Mushroomy Kingdom (STGMARIOPAST_00_*.pac & STGMARIOPAST_01_*.pac)",
                "mariopast"),
            new SupportedFileInfo(false,
                "Shadow Moses Island (STGMETALGEAR_00_*, STGMETALGEAR_01_*.pac, & STGMETALGEAR_02_*.pac)", "metalgear"),
            new SupportedFileInfo(false,
                "Smashville (STGVILLAGE_00_*.pac, STGVILLAGE_01_*.pac, STGVILLAGE_02_*.pac, STGVILLAGE_03_*.pac, & STGVILLAGE_04_*.pac)",
                "village"),
            new SupportedFileInfo(false, "Spear Pillar (STGTENGAN_1_*.pac, STGTENGAN_2_*.pac, & STGTENGAN_3_*.pac)",
                "tengan"),

            //Brawl Subspace Emissary files
            new SupportedFileInfo(true, "Adventure Stepjump", "adsj"),
            new SupportedFileInfo(true, "BLOC Adventure Archive", "bloc"),
            new SupportedFileInfo(true, "Brawl GEG1 File", "geg1"),

            //MDL0 subfiles
            new SupportedFileInfo(false, "MDL0 Material", "mdl0mat"),
            new SupportedFileInfo(false, "MDL0 Shader", "mdl0shade"),

            //Gecko Codes
            new SupportedFileInfo(true, "Gecko Code Table", "gct"),
            new SupportedFileInfo(true, false, "Text File", "txt"),

            //Brawl Mod Files
            new SupportedFileInfo(true, "Masquerade Costume File", "masq"),
            new SupportedFileInfo(true, "BrawlEx Configuration", "bx"),
            new SupportedFileInfo(true, "Custom My Music Tracklist", "cmm"),
            new SupportedFileInfo(true, "Alternate Stage Loader Data", "asl"),
            new SupportedFileInfo(true, "Stage Info Parameters", "param"),
            new SupportedFileInfo(true, "Stage Tracklist", "tlst"),
            new SupportedFileInfo(true, "Adventure Character Selection B", "selb"),
            new SupportedFileInfo(true, "Adventure Character Selection C", "selc"),
            new SupportedFileInfo(true, "Item Override", "itov"),

            //The following files are not for direct editing

            //Images
            new SupportedFileInfo(false, "Portable Network Graphics", "png"),
            new SupportedFileInfo(false, "Truevision TARGA", "tga"),
            new SupportedFileInfo(false, "Tagged Image File Format", "tif", "tiff"),
            new SupportedFileInfo(false, "Bitmap", "bmp"),
            new SupportedFileInfo(false, "JPEG Image", "jpg", "jpeg"),
            new SupportedFileInfo(false, "Graphics Interchange Format", "gif"),

            //Misc
            new SupportedFileInfo(false, "Uncompressed PCM", "wav"),
            new SupportedFileInfo(false, "3D Object Mesh", "obj"),
            new SupportedFileInfo(false, "JSON File", "json"),
            new SupportedFileInfo(true, false, "Data File", "dat"),
            new SupportedFileInfo(true, false, "Binary File", "bin"),
            new SupportedFileInfo(true, false, "Binary Backup File", "bak"),
            new SupportedFileInfo(false, "Raw Data File", "*"),

            // BrawlAPI
            new SupportedFileInfo(false, "Python File", ".py"),
            new SupportedFileInfo(false, "F# script", ".fsx")
        };

        private static string _allSupportedFilter;
        private static string _filterList;

        private static string _allSupportedFilterEditable;
        private static string _filterListEditable;

        public static SupportedFileInfo[] GetInfo(params string[] extensions)
        {
            SupportedFileInfo[] infoArray = new SupportedFileInfo[extensions.Length];
            foreach (SupportedFileInfo fileInfo in Files)
            {
                foreach (string ext in fileInfo.Extensions)
                {
                    int index = extensions.IndexOf(ext);
                    if (index >= 0 && !infoArray.Contains(fileInfo))
                    {
                        infoArray[index] = fileInfo;
                        extensions[index] = null;
                    }
                }
            }

            //Add remaining extensions not included in the supported formats array
            string s;
            for (int i = 0; i < extensions.Length; i++)
            {
                if (!string.IsNullOrEmpty(s = extensions[i]))
                {
                    infoArray[i] = new SupportedFileInfo(false, $"{s.ToUpper()} File", s);
                }
            }

            return infoArray;
        }

        public static string CompleteFilterEditableOnly =>
            GetAllSupportedFilter(true) + "|" + GetListFilter(true) + "|All Files (*.*)|*.*";

        public static string CompleteFilter =>
            GetAllSupportedFilter(false) + "|" + GetListFilter(false) + "|All Files (*.*)|*.*";

        public static string GetCompleteFilter(params string[] extensions)
        {
            return GetCompleteFilter(GetInfo(extensions));
        }

        public static string GetCompleteFilter(SupportedFileInfo[] files)
        {
            switch (files.Length)
            {
                case 0:
                case 1 when files[0].Extensions[0].Equals("*"):
                    return "All Files (*.*)|*.*";
                case 1: //No need for the "all supported" filter if there's only one supported filter
                    return GetListFilter(files) + "|All Files (*.*)|*.*";
                default:
                    return GetAllSupportedFilter(files) + "|" + GetListFilter(files) + "|All Files (*.*)|*.*";
            }
        }

        public static string GetAllSupportedFilter(bool editableOnly)
        {
            if (editableOnly && _allSupportedFilterEditable != null)
            {
                return _allSupportedFilterEditable;
            }

            if (!editableOnly && _allSupportedFilter != null)
            {
                return _allSupportedFilter;
            }

            if (editableOnly)
            {
                return _allSupportedFilterEditable = GetAllSupportedFilter(Files, true);
            }

            return _allSupportedFilter = GetAllSupportedFilter(Files, false);
        }

        private static readonly int MaxExtensionsInAllFilter = 10;

        public static string GetAllSupportedFilter(SupportedFileInfo[] files, bool editableOnly = false)
        {
            string filter = "All Supported Formats (";
            string filter2 = "|";

            //The "all supported formats" string needs (*.*) in it
            //or else the window will display EVERY SINGLE FILTER
            bool doNotAdd = files.Length > MaxExtensionsInAllFilter;
            if (doNotAdd)
            {
                filter += "*.*";
            }

            IEnumerable<SupportedFileInfo> e;
            if (editableOnly)
            {
                e = files.Where(x => x.ForEditing);
            }
            else
            {
                e = files;
            }

            string[] fileTypeExtensions = e.Select(x => x.ExtensionsFilter).ToArray();
            for (int i = 0; i < fileTypeExtensions.Length; i++)
            {
                string[] extensions = fileTypeExtensions[i].Split(';');
                string n = "";
                for (int x = 0; x < extensions.Length; x++)
                {
                    string ext = extensions[x];
                    string rawExtName = ext.Substring(ext.IndexOf('.') + 1);
                    //if (!rawExtName.Contains("*"))
                    n += (x != 0 ? ";" : "") + ext;
                }

                filter2 += (i != 0 ? ";" : "") + n;
                if (!doNotAdd)
                {
                    filter += (i != 0 ? ", " : "") + n;
                }
            }

            return filter + ")" + filter2;
        }

        public static string GetListFilter(bool editableOnly)
        {
            if (editableOnly && _filterListEditable != null)
            {
                return _filterListEditable;
            }

            if (!editableOnly && _filterList != null)
            {
                return _filterList;
            }

            if (editableOnly)
            {
                return _filterListEditable = GetListFilter(Files, true);
            }

            return _filterList = GetListFilter(Files, false);
        }

        public static string GetListFilter(SupportedFileInfo[] files, bool editableOnly = false)
        {
            string filter = "";

            IEnumerable<SupportedFileInfo> e;
            if (editableOnly)
            {
                e = files.Where(x => x.ForEditing);
            }
            else
            {
                e = files;
            }

            string[] fileTypeExtensions = e.Select(x => x.Filter).ToArray();
            for (int i = 0; i < fileTypeExtensions.Length; i++)
            {
                filter += fileTypeExtensions[i] + (i == fileTypeExtensions.Length - 1 ? "" : "|");
            }

            return filter;
        }
    }

    public class SupportedFileInfo
    {
        public string Name;
        public string[] Extensions;
        public bool ForEditing;
        public bool Associatable;

        public SupportedFileInfo(bool forEditing, string name, params string[] extensions)
        {
            ForEditing = forEditing;
            Associatable = forEditing;
            Name = name;
            if (extensions == null || extensions.Length == 0)
            {
                throw new ArgumentException("No extensions for file type \"" + Name + "\".");
            }

            Extensions = extensions;
        }

        public SupportedFileInfo(bool forEditing, bool associate, string name, params string[] extensions)
        {
            ForEditing = forEditing;
            Associatable = associate;
            Name = name;
            if (extensions == null || extensions.Length == 0)
            {
                throw new ArgumentException("No extensions for file type \"" + Name + "\".");
            }

            Extensions = extensions;
        }

        public string Filter
        {
            get
            {
                string s = ExtensionsFilter;
                return Name.EndsWith(")", StringComparison.OrdinalIgnoreCase)
                    ? $"{Name}|{s}"
                    : $"{Name} ({s.Replace(";", ", ")})|{s}";
            }
        }

        public string ExtensionsFilter
        {
            get
            {
                string filter = "";
                bool first = true;
                foreach (string ext in Extensions)
                {
                    if (!first)
                    {
                        filter += ";";
                    }

                    //In case of a specific file name
                    if (!ext.Contains('.'))
                    {
                        filter += "*.";
                    }

                    filter += ext;

                    first = false;
                }

                return filter;
            }
        }
    }
}