using BrawlLib.Imaging;
using BrawlLib.Internal.Drawing;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.Types;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0TextureNode : MDL0EntryNode, IComparable, IImageSource
    {
        public override ResourceType ResourceFileType => ResourceType.MDL0Texture;
        static MDL0TextureNode()
        {
            _folderWatcher = new FileSystemWatcher {Filter = "*.*", IncludeSubdirectories = false};
        }

        //internal MDL0Texture* Header { get { return (MDL0Texture*)WorkingUncompressed.Address; } }

        //[Category("Texture Data")]
        //public int MDL0Offset
        //{
        //    get
        //    {
        //        if (Header != null) return Header->Address - (VoidPtr)Model.Header;
        //        else
        //            return 0;
        //    }
        //}

        [Category("Texture Data")] public string[] References => _references.Select(n => n.Parent.ToString()).ToArray();

        //[Category("Texture Data")]
        //public int NumEntries
        //{
        //    get
        //    {
        //        //if (Header != null) return Header->_numEntries;
        //        //else 
        //        return _references.Count;
        //    }
        //}
        //[Category("Texture Data")]
        //public int DataLen
        //{
        //    get
        //    {
        //        //if (Header != null) return Header->_numEntries * 8 + 4;
        //        //else
        //        return _references.Count * 8 + 4;
        //    }
        //}

        //[Category("Texture Data")]
        //public string[] Entries { get { return _entries; } }
        //internal string[] _entries;

        public static string TextureOverrideDirectory
        {
            get => _folderWatcher.Path;
            set
            {
                if (!string.IsNullOrEmpty(value) && Directory.Exists(value))
                {
                    _folderWatcher.Path = Path.GetFullPath(value);
                }
                else
                {
                    _folderWatcher.Path = "";
                }
            }
        }

        public static FileSystemWatcher _folderWatcher;

        //public override bool OnInitialize()
        //{
        //    //_entries = new string[NumEntries];

        //    return false;
        //}

        //public void DoStuff()
        //{
        //    for (int i = 0; i < NumEntries; i++)
        //    {
        //        MDL0TextureEntry e = Header->Entries[i];
        //        int mat = MDL0Offset + e._mat;
        //        bool done = false;
        //        foreach (MDL0MaterialNode m in Model._matList)
        //        {
        //            if ((-mat) == m._mdl0Offset)
        //            {
        //                _entries[i] = m.Name;
        //                done = true;
        //                break;
        //            }
        //        }
        //        if (!done)
        //            _entries[i] = mat.ToString();
        //    }
        //}

        public GLTexture Texture;
        public bool Reset;
        public bool Selected;
        public bool ObjOnly;
        public bool Enabled = true;
        public object Source;

        public List<MDL0MaterialRefNode> _references = new List<MDL0MaterialRefNode>();

        public MDL0TextureNode()
        {
        }

        public MDL0TextureNode(string name)
        {
            _name = name;
        }

        public void SetPalette(PLT0Node plt)
        {
            _palette = plt;
            Texture?.SetPalette(_palette);
        }

        public PLT0Node _palette;

        internal void Prepare(MDL0MaterialRefNode mRef, int shaderProgramHandle, MDL0Node model = null,
                              string palette = null)
        {
            string plt = !string.IsNullOrEmpty(palette) ? palette : mRef.Palette;
            if (!string.IsNullOrEmpty(plt))
            {
                if (_palette == null || _palette.Name != plt)
                {
                    SetPalette(mRef.RootNode.FindChild("Palettes(NW4R)/" + plt, true) as PLT0Node);
                }
            }
            else if (_palette != null)
            {
                SetPalette(null);
            }

            if (Texture != null)
            {
                Texture.Bind(mRef.Index, shaderProgramHandle);
            }
            else
            {
                Load(mRef.Index, shaderProgramHandle, model ?? Model, mRef.Parent?.Name.EndsWith("_ExtMtl") ?? false);
            }

            ApplyGLTextureParameters(mRef);

            if (shaderProgramHandle <= 0)
            {
                float* p = stackalloc float[4];
                p[0] = p[1] = p[2] = p[3] = 1.0f;
                if (Selected && !ObjOnly)
                {
                    p[0] = -1.0f;
                }

                GL.Light(LightName.Light0, LightParameter.Specular, p);
                GL.Light(LightName.Light0, LightParameter.Diffuse, p);
            }
        }

        public static readonly TextureMagFilter[] _magFilters = {TextureMagFilter.Nearest, TextureMagFilter.Linear};

        public static readonly TextureMinFilter[] _minFilters =
        {
            TextureMinFilter.Nearest, TextureMinFilter.Linear, TextureMinFilter.NearestMipmapNearest,
            TextureMinFilter.NearestMipmapLinear, TextureMinFilter.LinearMipmapNearest,
            TextureMinFilter.LinearMipmapLinear
        };

        public static readonly TextureWrapMode[] _wraps =
            {TextureWrapMode.ClampToEdge, TextureWrapMode.Repeat, TextureWrapMode.MirroredRepeat};

        public static void ApplyGLTextureParameters(MDL0MaterialRefNode mr)
        {
            //GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName)ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, 1 << mr._maxAniso);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureLodBias, mr.LODBias);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) _magFilters[(int) mr.MagFilter]);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) _minFilters[(int) mr.MinFilter]);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                (int) _wraps[(int) mr.UWrapMode]);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                (int) _wraps[(int) mr.VWrapMode]);
        }

        public void GetSource()
        {
            Source = BRESNode?.FindChild("Textures(NW4R)/" + Name, true, StringComparison.Ordinal) as TEX0Node;
        }

        public void Reload(MDL0Node model, bool isMetal)
        {
            if (TKContext.CurrentContext == null)
            {
                return;
            }

            TKContext.CurrentContext.Capture();
            Load(_index, _program, model, isMetal);
        }

        private int _index = -1;
        private int _program = -1;

        private void Load(int index, int program, MDL0Node model, bool isMetal)
        {
            _index = index;
            _program = program;
            if (TKContext.CurrentContext == null)
            {
                return;
            }

            Texture?.Delete();

            Texture = new GLTexture();
            Texture.Bind(index, program);

            Bitmap bmp = null;
            BRRESNode bres = model?.BRESNode;

            if (!(_folderWatcher.EnableRaisingEvents && !string.IsNullOrEmpty(_folderWatcher.Path) &&
                  (bmp = SearchDirectory(Path.Combine(_folderWatcher.Path, Name))) != null))
            {
                GetSource();
                if (Source != null && Source is TEX0Node t)
                {
                    Texture.Attach(t, _palette);
                    return;
                }
            }

            TEX0Node tNode;
            if (bmp == null && !(Source is TEX0Node) &&
                TKContext.CurrentContext._states.ContainsKey("_Node_Refs"))
            {
                List<ResourceNode> nodes;
                if (model?.BRESNode?.Parent != null)
                {
                    nodes = model.BRESNode.Parent.Children;
                }
                else
                {
                    nodes = model.RootNode.GetChildrenRecursive();
                }

                //Search parent BRES first
                if (bres != null && (tNode = bres.FindChild("Textures(NW4R)/" + Name, false, StringComparison.Ordinal) as TEX0Node) !=
                    null)
                {
                    Source = tNode;
                    Texture.Attach(tNode, _palette);
                    return;
                }

                foreach (ResourceNode n in nodes)
                {
                    ARCEntryNode a;
                    BRRESNode b;
                    bool redirect = false;
                    if ((a = n as ARCEntryNode) != null && bres != null &&
                        (bres.GroupID == a.GroupID || a.GroupID == 0) &&
                        (b = a.RedirectNode as BRRESNode) != null)
                    {
                        redirect = true;
                    }
                    else if (a == null || (b = n as BRRESNode) == null)
                    {
                        continue;
                    }

                    if (a.FileType != ARCFileType.TextureData ||
                        !redirect && bres != null && bres.GroupID != b.GroupID && b.GroupID != 0)
                    {
                        continue;
                    }

                    //Search node itself first
                    if ((tNode = b.FindChild("Textures(NW4R)/" + Name, false, StringComparison.Ordinal) as TEX0Node) !=
                        null)
                    {
                        Source = tNode;
                        Texture.Attach(tNode, _palette);
                        return;
                    }

                    //Then search the directory
                    bmp = SearchDirectory(n._origPath);

                    if (bmp != null)
                    {
                        break;
                    }
                }
            }

            if (bmp != null)
            {
                Texture.Attach(bmp);
                return;
            }

            if (isMetal && RootNode is ARCNode arc && arc.IsFighter && Name.Equals("metal00", StringComparison.Ordinal))
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string resourceName = "BrawlLib.HardcodedFiles.Textures.metal00.tex0";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        Texture.Default();
                        return;
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        tNode = NodeFactory.FromSource(null, new DataSource(ms)) as TEX0Node;
                        Texture.Attach(tNode, _palette);
                    }
                }

                return;
            }

            Texture.Default();
        }

        private Bitmap SearchDirectory(string path)
        {
            Bitmap bmp = null;
            if (!string.IsNullOrEmpty(path))
            {
                DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(path));
                if (dir.Exists && Name != "<null>")
                {
                    foreach (FileInfo file in dir.GetFiles(Name + ".*"))
                    {
                        if (File.Exists(file.FullName))
                        {
                            if (file.Name.EndsWith(".tga"))
                            {
                                Source = file.FullName;
                                bmp = TGA.FromFile(file.FullName);
                                break;
                            }

                            if (
                                file.Name.EndsWith(".png") ||
                                file.Name.EndsWith(".tiff") ||
                                file.Name.EndsWith(".tif") ||
                                file.Name.EndsWith(".jpg") ||
                                file.Name.EndsWith(".jpeg") ||
                                file.Name.EndsWith(".bmp") ||
                                file.Name.EndsWith(".gif"))
                            {
                                Source = file.FullName;
                                bmp = CopyImage(file.FullName);
                                break;
                            }
                        }
                    }
                }
            }

            return bmp;
        }

        public static Bitmap CopyImage(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            try
            {
                using (Bitmap sourceImage = (Bitmap) Image.FromFile(path))
                {
                    return sourceImage.Copy();
                }
            }
            catch
            {
                return null;
            }
        }

        public static int Compare(MDL0TextureNode t1, MDL0TextureNode t2)
        {
            return string.Compare(t1.Name, t2.Name, false);
        }

        public int CompareTo(object obj)
        {
            if (obj is MDL0TextureNode)
            {
                return Name.CompareTo(((MDL0TextureNode) obj).Name);
            }

            return 1;
        }

        internal override void Bind()
        {
            Selected = false;
        }

        internal override void Unbind()
        {
            if (Texture != null)
            {
                Texture.Delete();
                Texture = null;
            }
        }

        public int ImageCount
        {
            get
            {
                if (Source == null)
                {
                    Load(_index, _program, Model, true);
                }

                return Source is IImageSource i ? i.ImageCount : 0;
            }
        }

        public Bitmap GetImage(int index)
        {
            if (Source == null)
            {
                Load(_index, _program, Model, true);
            }

            if (Source is IImageSource i)
            {
                return i.GetImage(index);
            }

            return null;
        }
    }
}