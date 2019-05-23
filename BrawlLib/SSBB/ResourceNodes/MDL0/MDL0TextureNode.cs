using System;
using System.ComponentModel;
using BrawlLib.OpenGL;
using BrawlLib.Imaging;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0TextureNode : MDL0EntryNode, IComparable
    {
        static MDL0TextureNode()
        {
            _folderWatcher = new FileSystemWatcher() { Filter = "*.*", IncludeSubdirectories = false };
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
        
        [Category("Texture Data")]
        public string[] References { get { return _references.Select(n => n.Parent.ToString()).ToArray(); } }

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
            get { return _folderWatcher.Path; }
            set
            {
                if (!String.IsNullOrEmpty(value) && Directory.Exists(value))
                    _folderWatcher.Path = value + "\\";
                else
                    _folderWatcher.Path = "";
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

        public MDL0TextureNode() { }
        public MDL0TextureNode(string name) 
        {
            _name = name;
        }

        public void SetPalette(PLT0Node plt)
        {
            _palette = plt;
            if (Texture != null)
                Texture.SetPalette(_palette);
        }

        public PLT0Node _palette = null;
        internal unsafe void Prepare(MDL0MaterialRefNode mRef, int shaderProgramHandle, string palette = null)
        {
            string plt = !String.IsNullOrEmpty(palette) ? palette : mRef.Palette;
            if (!String.IsNullOrEmpty(plt))
            {
                if (_palette == null || _palette.Name != plt)
                    SetPalette(mRef.RootNode.FindChild("Palettes(NW4R)/" + plt, true) as PLT0Node);
            }
            else if (_palette != null)
                SetPalette(null);

            if (Texture != null)
                Texture.Bind(mRef.Index, shaderProgramHandle);
            else
                Load(mRef.Index, shaderProgramHandle);

            ApplyGLTextureParameters(mRef);

            if (shaderProgramHandle <= 0)
            {
                float* p = stackalloc float[4];
                p[0] = p[1] = p[2] = p[3] = 1.0f;
                if (Selected && !ObjOnly)
                    p[0] = -1.0f;

                GL.Light(LightName.Light0, LightParameter.Specular, p);
                GL.Light(LightName.Light0, LightParameter.Diffuse, p);
            }
        }

        public static readonly TextureMagFilter[] _magFilters = { TextureMagFilter.Nearest, TextureMagFilter.Linear };
        public static readonly TextureMinFilter[] _minFilters = { TextureMinFilter.Nearest, TextureMinFilter.Linear, TextureMinFilter.NearestMipmapNearest, TextureMinFilter.NearestMipmapLinear, TextureMinFilter.LinearMipmapNearest, TextureMinFilter.LinearMipmapLinear, };
        public static readonly TextureWrapMode[] _wraps = { TextureWrapMode.ClampToEdge, TextureWrapMode.Repeat, TextureWrapMode.MirroredRepeat };

        public static void ApplyGLTextureParameters(MDL0MaterialRefNode mr)
        {
            //GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName)ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, 1 << mr._maxAniso);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureLodBias, mr.LODBias);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)_magFilters[(int)mr.MagFilter]);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)_minFilters[(int)mr.MinFilter]);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)_wraps[(int)mr.UWrapMode]);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)_wraps[(int)mr.VWrapMode]);
        }

        public void GetSource()
        {
            Source = BRESNode.FindChild("Textures(NW4R)/" + Name, true) as TEX0Node;
        }

        public void Reload()
        {
            if (TKContext.CurrentContext == null)
                return;

            TKContext.CurrentContext.Capture();
            Load();
        }

        private unsafe void Load() { Load(-1, -1); }
        private unsafe void Load(int index, int program)
        {
            if (TKContext.CurrentContext == null)
                return;

            Source = null;

            if (Texture != null)
                Texture.Delete();
            Texture = new GLTexture();
            Texture.Bind(index, program);

            Bitmap bmp = null;

            if (_folderWatcher.EnableRaisingEvents && !String.IsNullOrEmpty(_folderWatcher.Path))
                bmp = SearchDirectory(_folderWatcher.Path + Name);

            if (bmp == null && TKContext.CurrentContext._states.ContainsKey("_Node_Refs"))
            {
                List<ResourceNode> nodes = TKContext.CurrentContext._states["_Node_Refs"] as List<ResourceNode>;
                List<ResourceNode> searched = new List<ResourceNode>(nodes.Count);
                TEX0Node tNode = null;

                foreach (ResourceNode n in nodes)
                {
                    ResourceNode node = n.RootNode;
                    if (searched.Contains(node))
                        continue;
                    searched.Add(node);

                    //Search node itself first
                    if ((tNode = node.FindChild("Textures(NW4R)/" + Name, true) as TEX0Node) != null)
                    {
                        Source = tNode;
                        Texture.Attach(tNode, _palette);
                        return;
                    }
                    else //Then search the directory
                        bmp = SearchDirectory(node._origPath);

                    if (bmp != null)
                        break;
                }
                searched.Clear();
            }

            if (bmp != null)
                Texture.Attach(bmp);
            else
                Texture.Default();
        }

        private Bitmap SearchDirectory(string path)
        {
            Bitmap bmp = null;
            if (!String.IsNullOrEmpty(path))
            {
                DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(path));
                if (dir.Exists && Name != "<null>")
                    foreach (FileInfo file in dir.GetFiles(Name + ".*"))
                        if (File.Exists(file.FullName))
                            if (file.Name.EndsWith(".tga"))
                            {
                                Source = file.FullName;
                                bmp = TGA.FromFile(file.FullName);
                                break;
                            }
                            else if (
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
            return bmp;
        }

        public static Bitmap CopyImage(string path)
        {
            if (!File.Exists(path))
                return null;

            try
            {
                using (var sourceImage = (Bitmap)Image.FromFile(path))
                    return sourceImage.Copy();
            }
            catch
            {
                return null;
            }
        }  

        public static int Compare(MDL0TextureNode t1, MDL0TextureNode t2)
        {
            return String.Compare(t1.Name, t2.Name, false);
        }

        public int CompareTo(object obj)
        {
            if (obj is MDL0TextureNode)
                return this.Name.CompareTo(((MDL0TextureNode)obj).Name);
            else return 1;
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
    }
}
