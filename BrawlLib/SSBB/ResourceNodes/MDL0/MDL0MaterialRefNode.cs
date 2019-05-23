using System;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.IO;
using BrawlLib.Modeling;
using BrawlLib.Wii.Graphics;
using System.Windows.Forms;
using BrawlLib.Wii.Models;
using BrawlLib.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0MaterialRefNode : MDL0EntryNode
    {
        internal MDL0TextureRef* Header { get { return (MDL0TextureRef*)_origSource.Address; } set { _origSource.Address = value; } }
        public override bool AllowDuplicateNames { get { return true; } }

        [Browsable(false)]
        public MDL0MaterialNode Material { get { return Parent as MDL0MaterialNode; } }

        public MDL0MaterialRefNode()
        {
            _uWrap = (int)MatWrapMode.Repeat;
            _vWrap = (int)MatWrapMode.Repeat;
            _bindState = TextureFrameState.Neutral;
            _texMatrixEffect = TexMtxEffect.Default;
            _minFltr = 1;
            _magFltr = 1;
        }

        public override string Name
        {
            get { return _texture != null ? _texture.Name : base.Name; }
            set { Texture = value; base.Name = value; }
        }

        #region Variables

        public TexMtxEffect _texMatrixEffect;
        public XFDualTex _dualTexFlags;
        public XFTexMtxInfo _texMtxFlags;

        internal int _uWrap;
        internal int _vWrap;
        internal int _minFltr;
        internal int _magFltr;
        internal float _lodBias;
        internal int _maxAniso;
        internal bool _clampBias;
        internal bool _texelInterp;

        #endregion

        #region Properties

        public bool HasTextureMatrix
        {
            get
            {
                bool allsinglebinds = true;
                if (Material.Objects != null)
                {
                    foreach (MDL0ObjectNode n in Material.Objects)
                        if (n.Weighted)
                        {
                            allsinglebinds = false;
                            if (!n._manager.HasTextureMatrix[Index])
                                return false;
                        }
                }
                else return false;

                if (allsinglebinds)
                    return false;

                return true;
            }
            set
            {
                foreach (MDL0ObjectNode n in Material.Objects)
                    if (n.Weighted)
                    {
                        n._manager.HasTextureMatrix[Index] = value;
                        n._forceRebuild = true;
                        n.SignalPropertyChange();

                        if (n._vertexNode.Format != WiiVertexComponentType.Float)
                            n._vertexNode._forceRebuild = n._vertexNode._forceFloat = value;
                    }
                SignalPropertyChange();
            }
        }

        [Category("Texture Coordinates"), TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 Scale
        {
            get { return _bindState.Scale; }
            set
            {
                if (!CheckIfMetal())
                {
                    _bindState.Scale = value;
                    SignalPropertyChange();
                    UpdateCurrentControl();
                }
            }
        }
        [Category("Texture Coordinates")]
        public float Rotation
        {
            get { return _bindState.Rotate; }
            set
            {
                if (!CheckIfMetal())
                {
                    _bindState.Rotate = value;
                    SignalPropertyChange();
                    UpdateCurrentControl();
                }
            }
        }
        [Category("Texture Coordinates"), TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 Translation
        {
            get { return _bindState.Translate; }
            set
            {
                if (!CheckIfMetal())
                {
                    _bindState.Translate = value;
                    SignalPropertyChange();
                    UpdateCurrentControl();
                }
            }
        }

        [Category("Texture Matrix Effect")]
        public sbyte SCN0RefCamera
        {
            get { return _texMatrixEffect.SCNCamera; }
            set
            {
                if (!CheckIfMetal())
                {
                    _texMatrixEffect.SCNCamera = value;
                    SignalPropertyChange(); 
                }
            }
        }
        [Category("Texture Matrix Effect")]
        public sbyte SCN0RefLight
        {
            get { return _texMatrixEffect.SCNLight; }
            set
            {
                if (!CheckIfMetal())
                {
                    _texMatrixEffect.SCNLight = value;
                    SignalPropertyChange(); 
                }
            }
        }
        [Category("Texture Matrix Effect")]
        public MappingMethod MapMode
        {
            get { return _texMatrixEffect.MapMode; }
            set
            {
                if (!CheckIfMetal())
                {
                    _texMatrixEffect.MapMode = value;
                    SignalPropertyChange();
                    UpdateCurrentControl();
                }
            }
        }
        [Category("Texture Matrix Effect"), TypeConverter(typeof(Matrix43StringConverter)), Browsable(false)]
        public Matrix34 EffectMatrix
        {
            get { return _texMatrixEffect.TextureMatrix; }
            set
            {
                if (!CheckIfMetal())
                {
                    _texMatrixEffect.TextureMatrix = value;
                    SignalPropertyChange();
                }
            }
        }
        
        [Category("XF TexGen Flags")]
        public TexProjection Projection
        {
            get { return _texMtxFlags.Projection; }
            set
            {
                if (!CheckIfMetal())
                {
                    _texMtxFlags.Projection = value;
                    SignalPropertyChange(); 
                }
            }
        }
        [Category("XF TexGen Flags")]
        public TexInputForm InputForm
        {
            get { return _texMtxFlags.InputForm; }
            set
            {
                if (!CheckIfMetal())
                {
                    _texMtxFlags.InputForm = value;
                    SignalPropertyChange(); 
                }
            }
        }
        [Category("XF TexGen Flags")]
        public TexTexgenType Type
        {
            get { return _texMtxFlags.TexGenType; }
            set
            {
                if (!CheckIfMetal())
                {
                    _texMtxFlags.TexGenType = value;
                    SignalPropertyChange(); 
                }
            }
        }
        [Category("XF TexGen Flags")]
        public TexSourceRow Coordinates
        {
            get { return _texMtxFlags.SourceRow; }
            set
            {
                if (!CheckIfMetal())
                {
                    _texMtxFlags.SourceRow = value;
                    SignalPropertyChange(); 
                    UpdateCurrentControl();
                }
            }
        }
        [Category("XF TexGen Flags")]
        public int EmbossSource
        {
            get { return _texMtxFlags.EmbossSource; }
            set
            {
                if (!CheckIfMetal())
                {
                    _texMtxFlags.EmbossSource = value;
                    SignalPropertyChange(); 
                }
            }
        }
        [Category("XF TexGen Flags")]
        public int EmbossLight
        {
            get { return _texMtxFlags.EmbossLight; }
            set
            {
                if (!CheckIfMetal())
                {
                    _texMtxFlags.EmbossLight = value;
                    SignalPropertyChange(); 
                }
            }
        }
        [Category("XF TexGen Flags")]
        public bool Normalize
        {
            get { return _dualTexFlags._normalEnable != 0; }
            set
            {
                if (!CheckIfMetal())
                {
                    _dualTexFlags._normalEnable = (byte)(value ? 1 : 0);
                    SignalPropertyChange(); 
                }
            }
        }

        [Category("Texture Reference")]
        public MatWrapMode UWrapMode
        {
            get { return (MatWrapMode)_uWrap; }
            set
            {
                if (!CheckIfMetal())
                {
                    _uWrap = (int)value;
                    SignalPropertyChange();
                    UpdateCurrentControl();
                }
            }
        }
        [Category("Texture Reference")]
        public MatWrapMode VWrapMode
        {
            get { return (MatWrapMode)_vWrap; }
            set
            {
                if (!CheckIfMetal())
                {
                    _vWrap = (int)value;
                    SignalPropertyChange();
                    UpdateCurrentControl();
                }
            }
        }
        [Category("Texture Reference")]
        public MatTextureMinFilter MinFilter
        {
            get { return (MatTextureMinFilter)_minFltr; }
            set
            {
                if (!CheckIfMetal())
                {
                    _minFltr = (int)value;
                    SignalPropertyChange();
                    UpdateCurrentControl();
                }
            }
        }
        [Category("Texture Reference")]
        public MatTextureMagFilter MagFilter
        {
            get { return (MatTextureMagFilter)_magFltr; }
            set
            {
                if (!CheckIfMetal())
                {
                    _magFltr = (int)value;
                    SignalPropertyChange();
                    UpdateCurrentControl();
                }
            }
        }
        [Category("Texture Reference")]
        public float LODBias
        {
            get { return _lodBias; }
            set
            {
                if (!CheckIfMetal())
                {
                    _lodBias = value;
                    SignalPropertyChange();
                    UpdateCurrentControl();
                }
            }
        }
        [Category("Texture Reference")]
        public MatAnisotropy MaxAnisotropy
        {
            get { return (MatAnisotropy)_maxAniso; }
            set
            {
                if (!CheckIfMetal())
                {
                    _maxAniso = (int)value;
                    SignalPropertyChange();
                }
            }
        }
        [Category("Texture Reference")]
        public bool ClampBias
        {
            get { return _clampBias; }
            set
            {
                if (!CheckIfMetal())
                {
                    _clampBias = value;
                    SignalPropertyChange();
                }
            }
        }
        [Category("Texture Reference")]
        public bool TexelInterpolate
        {
            get { return _texelInterp; }
            set
            {
                if (!CheckIfMetal())
                {
                    _texelInterp = value;
                    SignalPropertyChange();
                }
            }
        }

        #endregion

        #region Texture linkage
        internal MDL0TextureNode _texture;
        [Browsable(false)]
        public MDL0TextureNode TextureNode
        {
            get { return _texture; }
            set
            {
                if (_texture == value)
                    return;
                if (_texture != null)
                {
                    _texture._references.Remove(this);
                    if (_texture._references.Count == 0)
                        _texture.Remove();
                }
                if ((_texture = value) != null)
                {
                    _texture._references.Add(this);

                    Name = _texture.Name;

                    if (_texture.Source == null)
                        _texture.GetSource();

                    if (_texture.Source is TEX0Node && ((TEX0Node)_texture.Source).HasPalette)
                        PaletteNode = Model.FindOrCreatePalette(_texture.Name);
                    else
                        PaletteNode = null;
                }
            }
        }
        [Browsable(true), TypeConverter(typeof(DropDownListTextures))]
        public string Texture
        {
            get { return _texture == null ? null : _texture.Name; }
            set { TextureNode = String.IsNullOrEmpty(value) || Model == null ? null : Model.FindOrCreateTexture(value); SignalPropertyChange(); UpdateCurrentControl(); }
        }
        #endregion

        #region Palette linkage
        internal MDL0TextureNode _palette;
        [Browsable(false)]
        public MDL0TextureNode PaletteNode
        {
            get { return _palette; }
            set
            {
                if (_palette == value)
                    return;
                if (_palette != null)
                {
                    _palette._references.Remove(this);
                    if (_palette._references.Count == 0)
                        _palette.Remove();
                }
                if ((_palette = value) != null)
                    _palette._references.Add(this);
            }
        }
        [Browsable(true), TypeConverter(typeof(DropDownListTextures))]
        public string Palette
        {
            get { return _palette == null ? null : _palette.Name; }
            set { PaletteNode = String.IsNullOrEmpty(value) ? null : Model.FindOrCreatePalette(value); SignalPropertyChange(); UpdateCurrentControl(); }
        }
        #endregion

        [Browsable(false)]
        public int TextureCoordId
        {
            get
            {
                if ((int)Coordinates >= (int)TexSourceRow.TexCoord0)
                    return (int)Coordinates - (int)TexSourceRow.TexCoord0;
                else
                    return -1 - (int)Coordinates;
            }
        }
        public bool CheckIfMetal()
        {
            if (Material != null && Material.CheckIfMetal())
                return true;

            return false;
        }

        public override bool OnInitialize()
        {
            MDL0TextureRef* header = Header;

            _uWrap = header->_uWrap;
            _vWrap = header->_vWrap;
            _minFltr = header->_minFltr;
            _magFltr = header->_magFltr;
            _lodBias = header->_lodBias;
            _maxAniso = header->_maxAniso;
            _clampBias = header->_clampBias == 1;
            _texelInterp = header->_texelInterp == 1;

            if (header->_texOffset != 0)
            {
                if (_replaced && header->_texOffset >= Parent.WorkingUncompressed.Length)
                    Name = null;
                else
                {
                    if (_replaced)
                        Name = header->TextureName;
                    else
                        _name = header->TextureName;
                    _texture = Model.FindOrCreateTexture(_name);
                    _texture._references.Add(this);
                }
            }
            if (header->_pltOffset != 0)
            {
                if (_replaced && header->_pltOffset >= Parent.WorkingUncompressed.Length)
                    _palette = null;
                else
                {
                    string name = header->PaletteName;
                    _palette = Model.FindOrCreatePalette(name);
                    _palette._references.Add(this);
                }
            }

            int len = Material.XFCmds.Count;
            if (len != 0 && Index * 2 + 1 < len)
            {
                _texMtxFlags = new XFTexMtxInfo(Material.XFCmds[Index * 2]._values[0]);
                _dualTexFlags = new XFDualTex(Material.XFCmds[Index * 2 + 1]._values[0]);
            }

            //if (PaletteNode == null && TextureNode != null)
            //{
            //    if (TextureNode.Source == null)
            //        TextureNode.GetSource();

            //    if (TextureNode.Source is TEX0Node && ((TEX0Node)TextureNode.Source).HasPalette)
            //    {
            //        Model._errors.Add("A palette was not set to texture reference " + Index + " in material " + Parent.Index + " (" + Parent.Name + ").");
            //        PaletteNode = Model.FindOrCreatePalette(TextureNode.Name);

            //        SignalPropertyChange();
            //    }
            //}

            MDL0TexSRTData* TexSettings = Material.Header->TexMatrices(Material._initVersion);

            _texMatrixEffect = TexSettings->GetTexMatrices(Index);
            _bindState = TexSettings->GetTexSRT(Index);
            _bindState.MatrixMode = Material.TextureMatrixMode;

            return false;
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            if (_palette != null)
                table.Add(_palette.Name);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            MDL0TextureRef* header = (MDL0TextureRef*)address;
            header->_texPtr = 0;
            header->_pltPtr = 0;
            header->_index1 = Index;
            header->_index2 = Index;
            header->_uWrap = _uWrap;
            header->_vWrap = _vWrap;
            header->_minFltr = _minFltr;
            header->_magFltr = _magFltr;
            header->_lodBias = _lodBias;
            header->_maxAniso = _maxAniso;
            header->_clampBias = (byte)(_clampBias ? 1 : 0);
            header->_texelInterp = (byte)(_texelInterp ? 1 : 0);
            header->_pad = 0;
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0TextureRef* header = (MDL0TextureRef*)dataAddress;

            header->_texOffset = _texture != null ? (int)stringTable[_texture.Name] + 4 - (int)dataAddress : 0;
            header->_pltOffset = _palette != null ? (int)stringTable[_palette.Name] + 4 - (int)dataAddress : 0;
            header->_texPtr = 0;
            header->_pltPtr = 0;
            header->_index1 = Index;
            header->_index2 = Index;
            header->_uWrap = _uWrap;
            header->_vWrap = _vWrap;
            header->_minFltr = _minFltr;
            header->_magFltr = _magFltr;
            header->_lodBias = _lodBias;
            header->_maxAniso = _maxAniso;
            header->_clampBias = (byte)(_clampBias ? 1 : 0);
            header->_texelInterp = (byte)(_texelInterp ? 1 : 0);
            header->_pad = (short)0;
        }

        internal void Bind(int prog)
        {
            if (!String.IsNullOrEmpty(PAT0Texture))
            {
                if (!PAT0Textures.ContainsKey(PAT0Texture))
                    PAT0Textures[PAT0Texture] = new MDL0TextureNode(PAT0Texture) { Source = null, _palette = !String.IsNullOrEmpty(PAT0Palette) ? RootNode.FindChildByType(PAT0Palette, true, ResourceNodes.ResourceType.PLT0) as PLT0Node : null };
                MDL0TextureNode t = PAT0Textures[PAT0Texture];
                t.Bind();
                t.Prepare(this, prog, PAT0Palette);
            }
            else if (_texture != null)
                _texture.Prepare(this, prog);
        }

        internal override void Unbind()
        {
            if (_texture != null)
                _texture.Unbind();
            
            foreach (MDL0TextureNode t in PAT0Textures.Values)
                t.Unbind();
        }

        public TextureFrameState _frameState, _bindState;
        internal void ApplySRT0Texture(SRT0TextureNode node, float index = 0, TexMatrixMode matrixMode = TexMatrixMode.MatrixMaya)
        {
            _frameState = _bindState;

            if (node != null && index >= 1)
            {
                fixed (TextureFrameState* v = &_frameState)
                {
                    float* f = (float*)v;
                    for (int i = 0; i < 5; i++)
                        if (node.Keyframes[i]._keyCount > 0)
                            f[i] = node.GetFrameValue(i, index - 1);
                    _frameState.MatrixMode = matrixMode;
                    _frameState.CalcTransforms();
                }
            }
        }

        public Dictionary<string, MDL0TextureNode> PAT0Textures = new Dictionary<string, MDL0TextureNode>(); 
        public string PAT0Texture, PAT0Palette;
        internal void ApplyPAT0Texture(PAT0TextureNode node, float index)
        {
            PAT0TextureEntryNode prev = null;
            if (node != null && index >= 1 && node.Children.Count > 0)
            {
                foreach (PAT0TextureEntryNode next in node.Children)
                {
                    if (next.Index == 0)
                    {
                        prev = next;
                        continue;
                    }
                    if (prev._frame <= index - 1 && next._frame > index - 1)
                        break;
                    prev = next;
                }

                PAT0Texture = prev.Texture;
                PAT0Palette = prev.Palette;
                if (PAT0Texture != null && !PAT0Textures.ContainsKey(PAT0Texture))
                {
                    TEX0Node texture = RootNode.FindChildByType(PAT0Texture, true, ResourceNodes.ResourceType.TEX0) as TEX0Node;
                    if (texture != null)
                        PAT0Textures[PAT0Texture] = new MDL0TextureNode(texture.Name) { Source = texture, _palette = !String.IsNullOrEmpty(PAT0Palette) ? RootNode.FindChildByType(PAT0Palette, true, ResourceNodes.ResourceType.PLT0) as PLT0Node : null };
                }
                return;
            }
            else PAT0Texture = PAT0Palette = null;
        }

        public void SetEffectMatrix(SCN0Node node, ModelPanelViewport v, float frame)
        {
            if (MapMode != MappingMethod.TexCoord)
            {
                switch (MapMode)
                {
                    case MappingMethod.Projection:
                        _effectMatrix = Matrix34.ProjectionMapping(SCN0RefCamera, node, v, frame);
                        break;
                    case MappingMethod.EnvSpec:
                        _effectMatrix = Matrix34.EnvSpecMap(SCN0RefCamera, SCN0RefLight, node, v, frame);
                        break;
                    case MappingMethod.EnvLight:
                        _effectMatrix = Matrix34.EnvLightMap(SCN0RefLight, node, v, frame);
                        break;
                    case MappingMethod.EnvCamera:
                        _effectMatrix = Matrix34.EnvCamMap(SCN0RefCamera, node, v, frame);
                        break;
                    default:
                        _effectMatrix = Matrix.Identity;
                        break;
                }
            }
            else
                _effectMatrix = Matrix.Identity;
        }

        Matrix _effectMatrix = Matrix.Identity;
        public Matrix GetTransform(bool useEffectMtx)
        {
            if (!useEffectMtx || _effectMatrix == Matrix.Identity)
                return _frameState._transform;
            else
                return _frameState._transform * _effectMatrix;
        }

        public void Default()
        {
            Name = "NewRef";
            _minFltr = 1;
            _magFltr = 1;
            UWrapMode = MatWrapMode.Repeat;
            VWrapMode = MatWrapMode.Repeat;

            _bindState = TextureFrameState.Neutral;
            _texMatrixEffect.TextureMatrix = Matrix34.Identity;
            _texMatrixEffect.SCNCamera = -1;
            _texMatrixEffect.SCNLight = -1;
            _texMatrixEffect.MapMode = MappingMethod.TexCoord;

            _texMtxFlags = new XFTexMtxInfo()
            {
                Projection = TexProjection.ST,
                InputForm = TexInputForm.AB11,
                TexGenType = TexTexgenType.Regular,
                SourceRow = TexSourceRow.TexCoord0,
                EmbossSource = 5,
                EmbossLight = 0
            };

            _texture = Model.FindOrCreateTexture(_name);
            _texture._references.Add(this);
        }

        public override void Remove()
        {
            if (_parent != null && !CheckIfMetal())
            {
                TextureNode = null;
                PaletteNode = null;
                base.Remove();
            }
        }

        public override bool MoveUp()
        {
            if (Parent == null)
                return false;

            if (CheckIfMetal())
                return false;

            int index = Index - 1;
            if (index < 0)
                return false;

            Parent.Children.Remove(this);
            Parent.Children.Insert(index, this);
            Parent._changed = true;

            return true;
        }

        public override bool MoveDown()
        {
            if (Parent == null)
                return false;

            if (CheckIfMetal())
                return false;

            int index = Index + 1;
            if (index >= Parent.Children.Count)
                return false;

            Parent.Children.Remove(this);
            Parent.Children.Insert(index, this);
            Parent._changed = true;

            return true;
        }

        public override unsafe void Replace(string fileName)
        {
            base.Replace(fileName);

            Model.CheckTextures();
        }

        public override unsafe void Export(string outPath)
        {
            StringTable table = new StringTable();
            GetStrings(table);
            int dataLen = OnCalculateSize(true);
            int totalLen = dataLen + table.GetTotalSize();

            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.RandomAccess))
            {
                stream.SetLength(totalLen);
                using (FileMap map = FileMap.FromStream(stream))
                {
                    Rebuild(map.Address, dataLen, false);
                    table.WriteTable(map.Address + dataLen);
                    PostProcess(map.Address, map.Address, table);
                }
            }
        }
    }

    public enum MatAnisotropy
    {
        One, //No anisotropic filter.
        Two, //Filters a maximum of two samples.
        Four //Filters a maximum of four samples.
    }
    public enum MatWrapMode
    {
        Clamp,
        Repeat,
        Mirror
    }
    public enum MatTextureMinFilter : uint
    {
        Nearest = 0,
        Linear,
        Nearest_Mipmap_Nearest,
        Linear_Mipmap_Nearest,
        Nearest_Mipmap_Linear,
        Linear_Mipmap_Linear
    }
    public enum MatTextureMagFilter : uint
    {
        Nearest = 0,
        Linear,
    }
    public enum MappingMethod
    {
        TexCoord = 0x00,
        EnvCamera = 0x01,
        Projection = 0x02,
        EnvLight = 0x03,
        EnvSpec = 0x04
    }
}
