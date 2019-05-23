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
            new SupportedFileInfo(true, "ARC File Archive", "arc"),
            //new SupportedFileInfo(true, "RARC File Archive", "rarc"),
            new SupportedFileInfo(true, "MRG Resource Group", "mrg"),
            new SupportedFileInfo(true, "MRG Compressed Resource Group", "mrgc"),
            new SupportedFileInfo(true, "SZS Compressed Archive", "szs"),
            //new SupportedFileInfo(true, "SZP Compressed Archive", "szp"),
#if DEBUG
            new SupportedFileInfo(true, "Wii/GC Disc Image", "iso"),
#endif

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

            //NW4R XML
            //new SupportedFileInfo(true, "NW4R XML Model", "rmdl"),
            //new SupportedFileInfo(true, "NW4R XML Texture", "rtex"),
            //new SupportedFileInfo(true, "NW4R XML Palette", "rplt"),
            //new SupportedFileInfo(true, "NW4R XML Bone Animation", "rcha"),
            //new SupportedFileInfo(true, "NW4R XML Texture Animation", "rtsa"),
            //new SupportedFileInfo(true, "NW4R XML Vertex Morph", "rsha"),
            //new SupportedFileInfo(true, "NW4R XML Visibility Sequence", "rvia"),
            //new SupportedFileInfo(true, "NW4R XML Texture Pattern", "rtpa"),
            //new SupportedFileInfo(true, "NW4R XML Color Sequence", "rcla"),
            //new SupportedFileInfo(true, "NW4R XML Scene Settings", "rsca"),

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
            new SupportedFileInfo(true, "Brawl Stage Collision File", "coll"),
            new SupportedFileInfo(true, "Brawl Stage Parameters File", "stpm"),
            new SupportedFileInfo(true, "Brawl STDT File", "stdt"),
            new SupportedFileInfo(true, "Brawl Stage Collision Attributes File", "scla"),

            //Gamecube
            //new SupportedFileInfo(true, "J3D v3 Model", "bmd"),
            //new SupportedFileInfo(true, "J3D v4 Model", "bdl"),
            //new SupportedFileInfo(true, "Luigi's Mansion GC model", "bin"),

            //Wii U
            //new SupportedFileInfo(true, "Wii U Archive", "sarc"),
            //new SupportedFileInfo(true, "Wii U Resource Pack", "bfres"),
            //new SupportedFileInfo(true, "Wii U Model", "fmdl"),
            //new SupportedFileInfo(true, "Wii U Texture", "ftex"),
            //new SupportedFileInfo(true, "Wii U Skeletal Animation", "fska"),

            new SupportedFileInfo(true, "GCT Code List", "gct"),

            //The following files are not for direct editing

            //Images
            new SupportedFileInfo(false, "Portable Network Graphics", "png"),
            new SupportedFileInfo(false, "Truevision TARGA", "tga"),
            new SupportedFileInfo(false, "Tagged Image File Format", "tif", "tiff"),
            new SupportedFileInfo(false, "Bitmap", "bmp"),
            new SupportedFileInfo(false, "JPEG Image", "jpg", "jpeg"),
            new SupportedFileInfo(false, "Graphics Interchange Format", "gif"),

            //Misc
            new SupportedFileInfo(false, "Text File", "txt"),
            new SupportedFileInfo(false, "Uncompressed PCM", "wav"),
            new SupportedFileInfo(false, "3D Object Mesh", "obj"),
            new SupportedFileInfo(false, "JSON File", "json"),
            new SupportedFileInfo(false, "Raw Data File", "*"),
        };

        private static string _allSupportedFilter = null;
        private static string _filterList = null;

        private static string _allSupportedFilterEditable = null;
        private static string _filterListEditable = null;
        
        public static SupportedFileInfo[] GetInfo(params string[] extensions)
        {
            SupportedFileInfo[] infoArray = new SupportedFileInfo[extensions.Length];
            foreach (SupportedFileInfo fileInfo in Files)
                foreach (string ext in fileInfo._extensions)
                {
                    int index = extensions.IndexOf(ext);
                    if (index >= 0 && !infoArray.Contains(fileInfo))
                    {
                        infoArray[index] = fileInfo;
                        extensions[index] = null;
                    }
                }

            //Add remaining extensions not included in the supported formats array
            string s;
            for (int i = 0; i < extensions.Length; i++)
                if (!String.IsNullOrEmpty(s = extensions[i]))
                    infoArray[i] = new SupportedFileInfo(false, String.Format("{0} File", s.ToUpper()), s);
            
            return infoArray;
        }

        public static string CompleteFilterEditableOnly { get { return GetAllSupportedFilter(true) + "|" + GetListFilter(true); } }

        public static string CompleteFilter { get { return GetAllSupportedFilter(false) + "|" + GetListFilter(false); } }

        public static string GetCompleteFilter(params string[] extensions)
        {
            return GetCompleteFilter(GetInfo(extensions));
        }

        public static string GetCompleteFilter(SupportedFileInfo[] files)
        {
            if (files.Length == 0)
                return "All Files (*.*)|*.*";

            //No need for the all filter if there's only one filter
            if (files.Length == 1)
                return GetListFilter(files);

            return GetAllSupportedFilter(files) + "|" + GetListFilter(files);
        }

        public static string GetAllSupportedFilter(bool editableOnly)
        {
            if (editableOnly && _allSupportedFilterEditable != null)
                return _allSupportedFilterEditable;
            else if (!editableOnly && _allSupportedFilter != null)
                return _allSupportedFilter;

            if (editableOnly)
                return _allSupportedFilterEditable = GetAllSupportedFilter(Files, true);
            else
                return _allSupportedFilter = GetAllSupportedFilter(Files, false);
        }

        const int MaxExtensionsInAllFilter = 5;

        public static string GetAllSupportedFilter(SupportedFileInfo[] files, bool editableOnly = false)
        {
            string filter = "All Supported Formats (";
            string filter2 = "|";

            //The "all supported formats" string needs (*.*) in it
            //or else the window will display EVERY SINGLE FILTER
            bool doNotAdd = files.Length > MaxExtensionsInAllFilter;
            if (doNotAdd)
                filter += "*.*";

            IEnumerable<SupportedFileInfo> e;
            if (editableOnly)
                e = files.Where(x => x._forEditing);
            else
                e = files;

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
                    filter += (i != 0 ? ", " : "") + n;
            }
            return filter + ")" + filter2;
        }

        public static string GetListFilter(bool editableOnly)
        {
            if (editableOnly && _filterListEditable != null)
                return _filterListEditable;
            else if (!editableOnly && _filterList != null)
                return _filterList;

            if (editableOnly)
                return _filterListEditable = GetListFilter(Files, true);
            else
                return _filterList = GetListFilter(Files, false);
        }

        public static string GetListFilter(SupportedFileInfo[] files, bool editableOnly = false)
        {
            string filter = "";

            IEnumerable<SupportedFileInfo> e;
            if (editableOnly)
                e = files.Where(x => x._forEditing);
            else
                e = files;

            string[] fileTypeExtensions = e.Select(x => x.Filter).ToArray();
            for (int i = 0; i < fileTypeExtensions.Length; i++)
                filter += fileTypeExtensions[i] + (i == fileTypeExtensions.Length - 1 ? "" : "|");
            return filter;
        }
    }

    public class SupportedFileInfo
    {
        public string _name;
        public string[] _extensions;
        public bool _forEditing;
        
        public SupportedFileInfo(bool forEditing, string name, params string[] extensions)
        {
            _forEditing = forEditing;
            _name = name;
            if (extensions == null || extensions.Length == 0)
                throw new Exception("No extensions for file type \"" + _name + "\".");
            _extensions = extensions;
        }

        public string Filter { get { string s = ExtensionsFilter; return _name + " (" + s.Replace(";", ", ") + ")|" + s; } }
        public string ExtensionsFilter
        {
            get
            {
                string filter = "";
                bool first = true;
                foreach (string ext in _extensions)
                {
                    if (!first)
                        filter += ";";

                    //In case of a specific file name
                    if (!ext.Contains('.'))
                        filter += "*.";

                    filter += ext;

                    first = false;
                }
                return filter;
            }
        }
    }
}
