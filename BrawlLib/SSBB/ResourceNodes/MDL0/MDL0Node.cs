using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Modeling;
using BrawlLib.Modeling.Collada;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Animations;
using BrawlLib.Wii.Graphics;
using BrawlLib.Wii.Models;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0Node : BRESEntryNode, IModel
    {
        internal MDL0Header* Header => (MDL0Header*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.MDL0;
        public override int DataAlign => 0x20;
        public override int[] SupportedVersions => new int[] {8, 9, 10, 11};
        public override string Tag => "MDL0";

        public MDL0Node()
        {
            _version = 9;
            _linker = ModelLinker.Prepare(this);
        }

        #region Variables and Attributes

        internal int _scalingRule, _texMtxMode;
        public byte _envMtxMode;
        public bool _needsNrmMtxArray, _needsTexMtxArray, _enableExtents;
        public int _numFacepoints, _numTriangles, _numNodes;
        public Box _extents;

        public ModelLinker _linker;
        public AssetStorage _assets;
        public bool _hasTree, _hasMix, _hasOpa, _hasXlu, _isImport, _autoMetal;

        public List<MDL0BoneNode> _billboardBones = new List<MDL0BoneNode>();
        public InfluenceManager _influences = new InfluenceManager();
        public List<string> _errors = new List<string>();

        internal const string _textureMatrixModeDescription = @"";

        [Browsable(false)] public InfluenceManager Influences => _influences;

        [Category("Metal Materials")]
        [Browsable(true)]
        [Description(
            @"This feature is for Super Smash Bros Brawl models specifically.
        When true, metal materials and shaders will be added and modulated as you edit your own custom materials and shaders.")]
        public bool AutoMetalMaterials
        {
            get => _autoMetal;
            set
            {
                _autoMetal = value;
                GenerateMetalMaterials(_metalMat);
            }
        }

        [Category("Metal Materials")]
        [Browsable(true)]
        [Description(
            @"The texture name used by metal materials for this model. Editing this will automatically regenerate metal materials.")]
        public string MetalTexture
        {
            get => _metalMat;
            set
            {
                if (_matList != null)
                {
                    foreach (MDL0MaterialNode m in _matList)
                    {
                        if (m.IsMetal)
                        {
                            foreach (MDL0MaterialRefNode mr in m.Children)
                            {
                                if (mr.Name == _metalMat)
                                {
                                    mr.Name = value;
                                }
                            }
                        }
                    }
                }

                _metalMat = value;
            }
        }

        public string _metalMat;

        [Category("G3D Model")]
        public MDLScalingRule ScalingRule
        {
            get => (MDLScalingRule) _scalingRule;
            set
            {
                _scalingRule = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("G3D Model")]
        [Description(_textureMatrixModeDescription)]
        public TexMatrixMode TextureMatrixMode
        {
            get => (TexMatrixMode) _texMtxMode;
            set
            {
                _texMtxMode = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("G3D Model")]
        [Description(
            "How many points are stored in the model file and sent to the GPU every frame. A lower value is better.")]
        public int NumFacepoints
        {
            get
            {
                if (_objList == null)
                {
                    return 0;
                }

                int i = 0;
                foreach (MDL0ObjectNode n in _objList)
                {
                    i += n._numFacepoints;
                }

                return i;
            }
        }

        [Category("G3D Model")]
        [Description(
            "How many individual vertices this model has. A vertex in this case is only a point in space with its associated influence.")]
        public int NumVertices
        {
            get
            {
                if (_objList == null)
                {
                    return 0;
                }

                int i = 0;
                foreach (MDL0ObjectNode n in _objList)
                {
                    i += n.VertexCount;
                }

                return i;
            }
        }

        [Category("G3D Model")]
        [Description("The number of individual triangle faces this model has.")]
        public int NumTriangles
        {
            get
            {
                if (_objList == null)
                {
                    return 0;
                }

                int i = 0;
                foreach (MDL0ObjectNode n in _objList)
                {
                    i += n._numFaces;
                }

                return i;
            }
        }

        [Category("G3D Model")]
        [Description("The number of matrices used in this model (bones + weighted influences).")]
        public int NumNodes => _influences.Count + _linker.BoneCache.Length;

        protected override void OnVersionChanged(int previousVersion)
        {
            bool
                convertingDown = previousVersion > 9 && _version <= 9,
                convertingUp = previousVersion <= 9 && _version > 9;

            if (convertingDown)
            {
                //TODO: alert user to information that will be lost after converting down to v8 or v9,  saving and closing.
                //No need to allow cancelling the version change here, as the user can simply change the version back before saving and closing.
            }

            //Be sure the model is populated so that the object list is filled
            if (_children == null)
            {
                Populate(0);
            }

            //Version 10 and 11 objects are slighly different from 8 and 9
            if (_objList != null && (convertingDown || convertingUp))
            {
                foreach (MDL0ObjectNode o in _objList)
                {
                    o._forceRebuild = true;
                }
            }
        }

        [Category("G3D Model")]
        [Description(
            "True when one or more objects has normals and is rigged to more than one influence (the object's single bind property says '(none)').")]
        public bool NeedsNormalMtxArray => _needsNrmMtxArray;

        [Category("G3D Model")]
        [Description(
            "True when one or more objects has a texture matrix turned on and is rigged to more than one influence (the object's single bind property says '(none)').")]
        public bool NeedsTextureMtxArray => _needsTexMtxArray;

        [Category("G3D Model")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 BoxMin
        {
            get => _extents.Min;
            set
            {
                _extents.Min = value;
                SignalPropertyChange();
            }
        }

        [Category("G3D Model")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 BoxMax
        {
            get => _extents.Max;
            set
            {
                _extents.Max = value;
                SignalPropertyChange();
            }
        }

        [Category("G3D Model")]
        public bool EnableBoundingBox
        {
            get => _enableExtents;
            set
            {
                _enableExtents = value;
                SignalPropertyChange();
            }
        }

        [Category("G3D Model")]
        public MDLEnvelopeMatrixMode EnvelopeMatrixMode
        {
            get => (MDLEnvelopeMatrixMode) _envMtxMode;
            set
            {
                _envMtxMode = (byte) value;
                SignalPropertyChange();
            }
        }

        public bool IsStagePosition =>
            (Name.StartsWith("stgposition", StringComparison.OrdinalIgnoreCase) ||
             Name.StartsWith("stageposition", StringComparison.OrdinalIgnoreCase)) && NumFacepoints == 0 &&
            NumVertices == 0 && NumTriangles == 0;

        #endregion

        #region Immediate accessors

        public MDL0GroupNode _boneGroup,
            _matGroup,
            _shadGroup,
            _objGroup,
            _texGroup,
            _pltGroup,
            _vertGroup,
            _normGroup,
            _uvGroup,
            _defGroup,
            _colorGroup,
            _furPosGroup,
            _furVecGroup;

        public List<ResourceNode> _boneList,
            _matList,
            _shadList,
            _objList,
            _texList,
            _pltList,
            _vertList,
            _normList,
            _uvList,
            _defList,
            _colorList,
            _furPosList,
            _furVecList;

        [Browsable(false)] public ResourceNode DefinitionsGroup => _defGroup;
        [Browsable(false)] public ResourceNode BoneGroup => _boneGroup;
        [Browsable(false)] public ResourceNode MaterialGroup => _matGroup;
        [Browsable(false)] public ResourceNode ShaderGroup => _shadGroup;
        [Browsable(false)] public ResourceNode VertexGroup => _vertGroup;
        [Browsable(false)] public ResourceNode NormalGroup => _normGroup;
        [Browsable(false)] public ResourceNode UVGroup => _uvGroup;
        [Browsable(false)] public ResourceNode ColorGroup => _colorGroup;
        [Browsable(false)] public ResourceNode PolygonGroup => _objGroup;
        [Browsable(false)] public ResourceNode TextureGroup => _texGroup;
        [Browsable(false)] public ResourceNode PaletteGroup => _pltGroup;
        [Browsable(false)] public ResourceNode FurVecGroup => _furVecGroup;
        [Browsable(false)] public ResourceNode FurPosGroup => _furPosGroup;

        [Browsable(false)] public List<ResourceNode> DefinitionsList => _defList;
        [Browsable(false)] public List<ResourceNode> BoneList => _boneList;

        [Browsable(false)]
        public List<MDL0BoneNode> AllBones
        {
            get
            {
                List<MDL0BoneNode> bones = new List<MDL0BoneNode>();
                if (BoneGroup != null)
                {
                    foreach (ResourceNode r in BoneGroup.GetChildrenRecursive())
                    {
                        if (r is MDL0BoneNode b)
                        {
                            bones.Add(b);
                        }
                    }

                    bones = bones.OrderBy(o => o.BoneIndex).ToList();
                }

                return bones;
            }
        }

        [Browsable(false)] public List<ResourceNode> MaterialList => _matList;
        [Browsable(false)] public List<ResourceNode> ShaderList => _shadList;
        [Browsable(false)] public List<ResourceNode> VertexList => _vertList;
        [Browsable(false)] public List<ResourceNode> NormalList => _normList;
        [Browsable(false)] public List<ResourceNode> UVList => _uvList;
        [Browsable(false)] public List<ResourceNode> ColorList => _colorList;
        [Browsable(false)] public List<ResourceNode> PolygonList => _objList;
        [Browsable(false)] public List<ResourceNode> TextureList => _texList;
        [Browsable(false)] public List<ResourceNode> PaletteList => _pltList;
        [Browsable(false)] public List<ResourceNode> FurVecList => _colorList;
        [Browsable(false)] public List<ResourceNode> FurPosList => _colorList;

        #endregion

        #region Functions

        /// <summary>
        /// Call ApplyCHR0 before calling this
        /// </summary>
        public Box GetBox()
        {
            if (_objList == null)
            {
                return new Box();
            }

            Box box = Box.ExpandableVolume;
            foreach (MDL0ObjectNode o in _objList)
            {
                if (o._manager?._vertices != null)
                {
                    foreach (Vertex3 vertex in o._manager._vertices)
                    {
                        box.ExpandVolume(vertex.WeightedPosition);
                    }
                }
            }

            return box;
        }

        public void CalculateBoundingBoxes()
        {
            ApplyCHR(null, 0);
            _extents = GetBox();
            if (_boneList != null)
            {
                foreach (MDL0BoneNode b in _boneList)
                {
                    b.SetBox();
                }
            }

            SignalPropertyChange();
            UpdateProperties();
        }

        public void CheckTextures()
        {
            if (_texList != null)
            {
                for (int i = 0; i < _texList.Count; i++)
                {
                    MDL0TextureNode t = (MDL0TextureNode) _texList[i];
                    for (int x = 0; x < t._references.Count; x++)
                    {
                        if (t._references[x].Parent == null)
                        {
                            t._references.RemoveAt(x--);
                        }
                    }

                    if (t._references.Count == 0)
                    {
                        _texList.RemoveAt(i--);
                    }
                }
            }
        }

        public List<ResourceNode> GetUsedShaders()
        {
            List<ResourceNode> shaders = new List<ResourceNode>();
            if (_shadList != null)
            {
                foreach (MDL0ShaderNode s in _shadList)
                {
                    if (s._materials.Count > 0)
                    {
                        shaders.Add(s);
                    }
                }
            }

            return shaders;
        }

        public void GenerateMetalMaterials()
        {
            GenerateMetalMaterials(string.IsNullOrEmpty(_metalMat) ? "metal00" : _metalMat);
        }

        public void GenerateMetalMaterials(string metalTextureName)
        {
            if (_children == null)
            {
                Populate();
            }

            if (_matList == null)
            {
                return;
            }

            List<MDL0MaterialNode> metalMats = new List<MDL0MaterialNode>();
            foreach (MDL0MaterialNode m in _matList)
            {
                if (m.IsMetal)
                {
                    metalMats.Add(m);
                }
            }

            foreach (MDL0MaterialNode m in metalMats)
            {
                m.Remove(true);
            }

            for (int x = 0; x < _matList.Count; x++)
            {
                MDL0MaterialNode n = (MDL0MaterialNode) _matList[x];
                if (!n.IsMetal && n.MetalMaterial == null)
                {
                    MDL0MaterialNode node = new MDL0MaterialNode
                    {
                        _updating = true,
                        Name = n.Name + "_ExtMtl",
                        _activeStages = 4
                    };

                    _matGroup.AddChild(node);
                    for (int i = 0; i <= n.Children.Count; i++)
                    {
                        if (i != n.Children.Count &&
                            ((MDL0MaterialRefNode) n.Children[i]).MapMode == MappingMethod.EnvCamera)
                        {
                            continue;
                        }

                        MDL0MaterialRefNode mr = new MDL0MaterialRefNode();
                        node.AddChild(mr);
                        mr.Texture = metalTextureName;

                        mr._uWrap = 0;
                        mr._vWrap = 0;
                        mr._minFltr = 0;
                        mr._magFltr = 0;
                        mr._texMtxFlags.SourceRow = TexSourceRow.TexCoord0;
                        mr.EmbossSource = 5;

                        if (i == n.Children.Count || ((MDL0MaterialRefNode) n.Children[i]).HasTextureMatrix)
                        {
                            mr._minFltr = 5;
                            mr._magFltr = 1;
                            mr._lodBias = -2;

                            mr.HasTextureMatrix = true;
                            node.Rebuild(true);

                            mr._texMtxFlags = new XFTexMtxInfo
                            {
                                Projection = TexProjection.STQ,
                                InputForm = TexInputForm.ABC1,
                                TexGenType = TexTexgenType.Regular,
                                SourceRow = TexSourceRow.Normals,
                                EmbossSource = 5,
                                EmbossLight = 0
                            };

                            mr._dualTexFlags._normalEnable = 1;
                            mr._texMatrixEffect.MapMode = MappingMethod.EnvCamera;

                            break;
                        }
                    }

                    node._chan1 = new LightChannel(63, new RGBAPixel(128, 128, 128, 255),
                        new RGBAPixel(255, 255, 255, 255), 0, 0, node);
                    node.C1ColorEnabled = true;
                    node.C1ColorDiffuseFunction = GXDiffuseFn.Clamped;
                    node.C1ColorAttenuation = GXAttnFn.Spotlight;
                    node.C1AlphaEnabled = true;
                    node.C1AlphaDiffuseFunction = GXDiffuseFn.Clamped;
                    node.C1AlphaAttenuation = GXAttnFn.Spotlight;

                    node._chan2 = new LightChannel(63, new RGBAPixel(255, 255, 255, 255), new RGBAPixel(), 0, 0, node);
                    node.C2ColorEnabled = true;
                    node.C2ColorDiffuseFunction = GXDiffuseFn.Disabled;
                    node.C2ColorAttenuation = GXAttnFn.Specular;
                    node.C2AlphaDiffuseFunction = GXDiffuseFn.Disabled;
                    node.C2AlphaAttenuation = GXAttnFn.Specular;

                    node._lightSetIndex = n._lightSetIndex;
                    node._fogIndex = n._fogIndex;

                    node._cull = n._cull;
                    node.CompareBeforeTexture = true;
                    node._normMapRefLight1 =
                        node._normMapRefLight2 =
                            node._normMapRefLight3 =
                                node._normMapRefLight4 = -1;
                }
            }

            foreach (MDL0MaterialNode node in _matList)
            {
                if (!node.IsMetal)
                {
                    continue;
                }

                if (node.ShaderNode != null)
                {
                    if (node.ShaderNode._autoMetal && node.ShaderNode._texCount == node.Children.Count)
                    {
                        node._updating = false;
                        continue;
                    }

                    if (node.ShaderNode.Stages == 4)
                    {
                        foreach (MDL0MaterialNode y in node.ShaderNode._materials)
                        {
                            if (!y.IsMetal || y.Children.Count != node.Children.Count)
                            {
                                goto Next;
                            }
                        }

                        node.ShaderNode.DefaultAsMetal(node.Children.Count);
                        continue;
                    }
                }

                Next:
                bool found = false;
                foreach (MDL0ShaderNode s in _shadGroup.Children)
                {
                    if (s._autoMetal && s._texCount == node.Children.Count)
                    {
                        node.ShaderNode = s;
                        found = true;
                    }
                    else
                    {
                        if (s.Stages == 4)
                        {
                            foreach (MDL0MaterialNode y in s._materials)
                            {
                                if (!y.IsMetal || y.Children.Count != node.Children.Count)
                                {
                                    goto NotFound;
                                }
                            }

                            node.ShaderNode = s;
                            found = true;
                            goto End;
                            NotFound:
                            continue;
                        }
                    }
                }

                End:
                if (!found)
                {
                    MDL0ShaderNode shader = new MDL0ShaderNode();
                    _shadGroup.AddChild(shader);
                    shader.DefaultAsMetal(node.Children.Count);
                    node.ShaderNode = shader;
                }
            }

            foreach (MDL0MaterialNode m in _matList)
            {
                m._updating = false;
            }
        }

        public void CleanTextures()
        {
            if (_texList != null)
            {
                int i = 0;
                while (i < _texList.Count)
                {
                    MDL0TextureNode texture = (MDL0TextureNode) _texList[i];

                    at1:
                    foreach (MDL0MaterialRefNode r in texture._references)
                    {
                        if (_matList.IndexOf(r.Parent) == -1)
                        {
                            texture._references.Remove(r);
                            goto at1;
                        }
                    }

                    if (texture._references.Count == 0)
                    {
                        _texList.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            if (_pltList != null)
            {
                int i = 0;
                while (i < _pltList.Count)
                {
                    MDL0TextureNode palette = (MDL0TextureNode) _pltList[i];

                    bt1:
                    foreach (MDL0MaterialRefNode r in palette._references)
                    {
                        if (_matList.IndexOf(r.Parent) == -1)
                        {
                            palette._references.Remove(r);
                            goto bt1;
                        }
                    }

                    if (palette._references.Count == 0)
                    {
                        _pltList.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        public MDL0TextureNode FindOrCreateTexture(string name)
        {
            if (_texGroup == null)
            {
                AddChild(_texGroup = new MDL0GroupNode(MDLResourceType.Textures), false);
            }
            else
            {
                foreach (MDL0TextureNode n in _texGroup.Children)
                {
                    if (n._name == name)
                    {
                        return n;
                    }
                }
            }

            MDL0TextureNode node = new MDL0TextureNode(name);
            _texGroup.AddChild(node, false);

            return node;
        }

        public MDL0TextureNode FindOrCreatePalette(string name)
        {
            if (_pltGroup == null)
            {
                AddChild(_pltGroup = new MDL0GroupNode(MDLResourceType.Palettes), false);
            }
            else
            {
                foreach (MDL0TextureNode n in _pltGroup.Children)
                {
                    if (n._name == name)
                    {
                        return n;
                    }
                }
            }

            MDL0TextureNode node = new MDL0TextureNode(name);
            _pltGroup.AddChild(node, false);

            return node;
        }

        public MDL0BoneNode FindBone(string name)
        {
            foreach (MDL0BoneNode b in _linker.BoneCache)
            {
                if (b.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return b;
                }
            }

            return null;
        }

        public MDL0BoneNode FindBoneByIndex(int givenIndex)
        {
            // Generate bones if the model hasn't been seen yet
            if (_boneGroup == null)
            {
                Populate();
                _linker.RegenerateBoneCache();
            }

            foreach (MDL0BoneNode b in BoneCache)
            {
                if (b.BoneIndex == givenIndex)
                {
                    return b;
                }
            }

            return null;
        }

        public MDL0MaterialNode FindOrCreateOpaMaterial(string name)
        {
            foreach (MDL0MaterialNode m in _matList)
            {
                if (m.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return m;
                }
            }

            MDL0MaterialNode node = new MDL0MaterialNode {_name = _matGroup.FindName(name)};
            _matGroup.AddChild(node, false);

            SignalPropertyChange();

            return node;
        }

        public MDL0MaterialNode FindOrCreateXluMaterial(string name)
        {
            foreach (MDL0MaterialNode m in _matList)
            {
                if (m.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return m;
                }
            }

            MDL0MaterialNode node = new MDL0MaterialNode {_name = _matGroup.FindName(name), XLUMaterial = true};
            _matGroup.AddChild(node, false);

            SignalPropertyChange();

            return node;
        }

        public override void AddChild(ResourceNode child, bool change)
        {
            if (child is MDL0GroupNode)
            {
                LinkGroup(child as MDL0GroupNode);
            }

            base.AddChild(child, change);
        }

        public override void RemoveChild(ResourceNode child)
        {
            if (child is MDL0GroupNode)
            {
                UnlinkGroup(child as MDL0GroupNode);
            }

            base.RemoveChild(child);
        }

        private MDL0BoneNode FindOrAddBoneCopy(MDL0BoneNode bone)
        {
            MDL0BoneNode newBone = FindBone(bone.Name);
            if (newBone == null)
            {
                if (bone.Parent is MDL0GroupNode)
                {
                    BoneGroup.AddChild(newBone = bone.Clone());
                }
                else
                {
                    ResourceNode parent = bone.Parent;
                    if (parent != null)
                    {
                        List<ResourceNode> parentChain = new List<ResourceNode>();

                        //First parent will always be a bone, group handled above
                        while ((newBone = FindBone(parent.Name)) == null)
                        {
                            parentChain.Add(parent);

                            //Break before assigning the parent
                            //That way we can add the parent as a child to the group later
                            if (parent.Parent is MDL0GroupNode)
                            {
                                break;
                            }

                            parent = parent.Parent;
                        }

                        //Handle group parent
                        if (newBone == null)
                        {
                            BoneGroup.AddChild(newBone = ((MDL0BoneNode) parent).Clone());
                        }

                        MDL0BoneNode root = newBone;

                        //Add parents as children
                        //use reverse order
                        if (parentChain.Count > 0)
                        {
                            for (int i = parentChain.Count - 1; i >= 0; i--)
                            {
                                MDL0BoneNode b = ((MDL0BoneNode) parentChain[i]).Clone();
                                newBone.AddChild(b);
                                newBone = b;
                            }
                        }

                        MDL0BoneNode n = bone.Clone();
                        newBone.AddChild(n);
                        newBone = n;

                        //Recalculate the bind matrices of the newly added bone chain
                        root.RecalcBindState(false, false);
                    }
                }

                //Clean influence of possible unused users, just in case
                for (int i = 0; i < newBone.Users.Count; i++)
                {
                    IMatrixNodeUser u = newBone.Users[i];
                    if (u is Vertex3)
                    {
                        Vertex3 vert = u as Vertex3;
                        if (vert.Parent is MDL0ObjectNode)
                        {
                            MDL0ObjectNode obj = vert.Parent as MDL0ObjectNode;
                            if (obj.Model != this)
                            {
                                newBone.Users.RemoveAt(i--);
                            }
                        }
                    }
                    else if (u is MDL0ObjectNode)
                    {
                        MDL0ObjectNode obj = u as MDL0ObjectNode;
                        if (obj.Model != this)
                        {
                            newBone.Users.RemoveAt(i--);
                        }
                    }
                }

                for (int i = 0; i < newBone._singleBindObjects.Count; i++)
                {
                    if (newBone._singleBindObjects[i].Model != this)
                    {
                        newBone._singleBindObjects.RemoveAt(i--);
                    }
                }
            }
            else
            {
                //Update the bone's data
                newBone._bindState = new FrameState(bone._bindState._scale, bone._bindState._rotate,
                    bone._bindState._translate);
                newBone._billboardFlags = bone._billboardFlags;
                newBone._boneFlags = bone._boneFlags;
                newBone._extents = bone._extents;
                newBone.RecalcBindState(false, false);
            }

            //Regenerate bone cache, the FindBone function uses it
            _linker.RegenerateBoneCache();

            return newBone;
        }

        private Influence CleanAndAddInfluence(Influence inf)
        {
            //Clean influence of possible unused users, just in case
            for (int i = 0; i < inf.Users.Count; i++)
            {
                IMatrixNodeUser u = inf.Users[i];
                if (u is Vertex3)
                {
                    Vertex3 vert = u as Vertex3;
                    if (vert.Parent is MDL0ObjectNode)
                    {
                        MDL0ObjectNode obj = vert.Parent as MDL0ObjectNode;
                        if (obj.Model != this)
                        {
                            inf.Users.RemoveAt(i--);
                        }
                    }
                }
                else if (u is MDL0ObjectNode)
                {
                    MDL0ObjectNode obj = u as MDL0ObjectNode;
                    if (obj.Model != this)
                    {
                        inf.Users.RemoveAt(i--);
                    }
                }
            }

            return _influences.FindOrCreate(inf);
        }

        /// <summary>
        /// Don't call this, use ReplaceOrAddMesh instead
        /// </summary>
        private void ReplaceOrAddMeshInternal(
            MDL0ObjectNode repObj,
            ref bool[] addGroup,
            bool doSearch,
            bool replaceIfFound,
            bool addIfNotFound)
        {
            //Force a rebuild, just in case.
            //This will also avoid the rebuilder trying to copy the soure data over,
            //which has been disposed of.
            repObj._forceRebuild = true;

            //Find a matching object to replace using the object's name
            bool found = false;
            if (doSearch)
            {
                for (int i = 0; i < _objList.Count; i++)
                {
                    MDL0ObjectNode currObj = _objList[i] as MDL0ObjectNode;
                    if (repObj.Name == currObj.Name)
                    {
                        DrawCall[] drawCalls = currObj._drawCalls.ToArray();

                        //Copy the replaced object's draw calls to the new object
                        repObj._drawCalls = new BindingList<DrawCall>();
                        foreach (DrawCall c in drawCalls)
                        {
                            repObj._drawCalls.Add(new DrawCall(repObj)
                            {
                                //No need to duplicate anything, they're already a part of this model
                                MaterialNode = c.MaterialNode,
                                VisibilityBoneNode = c.VisibilityBoneNode,
                                DrawPriority = c.DrawPriority,
                                DrawPass = c.DrawPass
                            });
                        }

                        currObj.Remove(true, true, true, true, true, true, true, true, true, true, true, true);

                        found = true;
                        break;
                    }
                }
            }

            if (!found)
            {
                if (!addIfNotFound)
                {
                    return;
                }

                //Add visibility bone and material for each draw call in new object
                for (int i = 0; i < repObj._drawCalls.Count; i++)
                {
                    repObj._drawCalls[i].VisibilityBoneNode =
                        FindOrAddBoneCopy(repObj._drawCalls[i].VisibilityBoneNode);

                    if (repObj._drawCalls[i].MaterialNode != null &&
                        repObj._drawCalls[i].MaterialNode.Parent != MaterialGroup)
                    {
                        MDL0MaterialNode material = repObj._drawCalls[i].MaterialNode;
                        bool cont = true;

                        if (MaterialGroup == null)
                        {
                            addGroup[5] = true;
                            LinkGroup(new MDL0GroupNode(MDLResourceType.Materials));
                            _matGroup._parent = this;
                        }
                        else
                        {
                            foreach (MDL0MaterialNode mat in MaterialList)
                            {
                                if (mat == material)
                                {
                                    cont = false;
                                    break;
                                }
                            }
                        }

                        if (cont)
                        {
                            MaterialGroup.AddChild(material);
                            material.SignalPropertyChange();

                            if (material.ShaderNode != null &&
                                material.ShaderNode.Parent != ShaderGroup)
                            {
                                if (ShaderGroup == null)
                                {
                                    addGroup[6] = true;
                                    LinkGroup(new MDL0GroupNode(MDLResourceType.Shaders));
                                    _shadGroup._parent = this;
                                }

                                ShaderGroup.AddChild(material.ShaderNode);
                                material.ShaderNode.SignalPropertyChange();
                            }
                        }

                        repObj._drawCalls[i].MaterialNode = material;
                    }
                }
            }
            else if (!replaceIfFound)
            {
                return;
            }

            //Remove object from external, add to internal
            repObj.Parent?.RemoveChild(repObj);

            if (_objGroup == null)
            {
                addGroup[4] = true;
                LinkGroup(new MDL0GroupNode(MDLResourceType.Objects));
                _objGroup._parent = this;
            }

            _objGroup.AddChild(repObj);

            if (BoneGroup == null)
            {
                addGroup[7] = true;
                LinkGroup(new MDL0GroupNode(MDLResourceType.Bones));
                _boneGroup._parent = this;
            }

            //Set copied vertices' parent object (this is so single-bound objects are updated)
            foreach (Vertex3 v in repObj.Vertices)
            {
                v.Parent = repObj;
            }

            //Reassign bone influences to the current bone tree
            if (repObj.MatrixNode == null)
            {
                //Have to update each vertex
                foreach (Vertex3 v in repObj.Vertices)
                {
                    if (v.MatrixNode != null)
                    {
                        v.DeferUpdateAssets();
                        if (v.MatrixNode is Influence)
                        {
                            for (int x = 0; x < v.MatrixNode.Weights.Count; x++)
                            {
                                MDL0BoneNode bone = v.MatrixNode.Weights[x].Bone as MDL0BoneNode;
                                if (bone != null)
                                {
                                    v.MatrixNode.Weights[x].Bone = FindOrAddBoneCopy(bone);
                                }
                            }

                            v.MatrixNode = CleanAndAddInfluence(v.MatrixNode as Influence);
                        }
                        else
                        {
                            v.MatrixNode = FindOrAddBoneCopy((MDL0BoneNode) v.MatrixNode);
                        }
                    }
                }
            }
            else
            {
                //Make sure the replaced object's single bind belongs to the current model
                //Don't use the original object's single bind, as the rigging may have changed

                repObj.DeferUpdateAssets();
                if (repObj.MatrixNode is MDL0BoneNode)
                {
                    repObj.MatrixNode = FindOrAddBoneCopy(repObj.MatrixNode as MDL0BoneNode);
                }
                else
                {
                    Influence inf = repObj.MatrixNode as Influence;
                    repObj.MatrixNode = CleanAndAddInfluence(inf);
                }
            }

            //Make a copy of the manager so it isn't disposed of
            repObj._manager = repObj._manager.HardCopy();

            if (repObj._vertexNode != null && repObj._vertexNode.Parent != VertexGroup)
            {
                if (VertexGroup == null)
                {
                    addGroup[0] = true;
                    LinkGroup(new MDL0GroupNode(MDLResourceType.Vertices));
                    _vertGroup._parent = this;
                }

                MDL0VertexNode node = repObj._vertexNode;
                VertexGroup.AddChild(node);

                //Extract points from header, which will be disposed of
                Vector3[] v = node.Vertices;
                node.ForceRebuild = true;
                if (node.Format == WiiVertexComponentType.Float)
                {
                    node.ForceFloat = true;
                }
            }

            if (repObj._normalNode != null && repObj._normalNode.Parent != NormalGroup)
            {
                if (NormalGroup == null)
                {
                    addGroup[1] = true;
                    LinkGroup(new MDL0GroupNode(MDLResourceType.Normals));
                    _normGroup._parent = this;
                }

                MDL0NormalNode node = repObj._normalNode;
                NormalGroup.AddChild(node);

                //Extract points from header, which will be disposed of
                Vector3[] v = node.Normals;
                node._forceRebuild = true;
                if (node.Format == WiiVertexComponentType.Float)
                {
                    node._forceFloat = true;
                }
            }

            for (int x = 0; x < 2; x++)
            {
                if (repObj._colorSet[x] != null && repObj._colorSet[x].Parent != ColorGroup)
                {
                    if (ColorGroup == null)
                    {
                        addGroup[2] = true;
                        LinkGroup(new MDL0GroupNode(MDLResourceType.Colors));
                        _colorGroup._parent = this;
                    }

                    MDL0ColorNode node = repObj._colorSet[x];
                    ColorGroup.AddChild(node);

                    //Extract colors from header, which will be disposed of
                    RGBAPixel[] v = node.Colors;
                    node._changed = true;
                }
            }

            for (int x = 0; x < 8; x++)
            {
                if (repObj._uvSet[x] != null && repObj._uvSet[x].Parent != UVGroup)
                {
                    if (UVGroup == null)
                    {
                        addGroup[3] = true;
                        LinkGroup(new MDL0GroupNode(MDLResourceType.UVs));
                        _uvGroup._parent = this;
                    }

                    MDL0UVNode node = repObj._uvSet[x];
                    UVGroup.AddChild(node);

                    //Extract points from header, which will be disposed of
                    Vector2[] v = node.Points;
                    node._forceRebuild = true;
                    if (node.Format == WiiVertexComponentType.Float)
                    {
                        node._forceFloat = true;
                    }
                }
            }

            repObj.SignalPropertyChange();
        }

        public void ReplaceOrAddMesh(
            MDL0ObjectNode replacement,
            bool doSearch,
            bool replaceIfFound,
            bool addIfNotFound)
        {
            if (replacement == null)
            {
                return;
            }

            MDL0Node model = replacement.Model;
            if (model != null)
            {
                model.Populate();
                model.ResetToBindState();
            }

            bool[] addGroup = new bool[8];
            ReplaceOrAddMeshInternal(
                replacement,
                ref addGroup,
                doSearch,
                replaceIfFound,
                addIfNotFound);

            FinishReplace(addGroup);
        }

        public void ReplaceMeshes(
            MDL0Node replacement,
            bool doSearch,
            bool replaceIfFound,
            bool addIfNotFound)
        {
            if (replacement == null)
            {
                return;
            }

            replacement.Populate();
            replacement.ResetToBindState();

            // Save texture matrix settings
            Dictionary<string, bool[]> texMatrixSettings = new Dictionary<string, bool[]>();
            if (_matGroup != null && _matGroup.Children.Count > 0)
            {
                foreach (MDL0MaterialNode m in _matGroup.Children.Where(m => m.HasChildren))
                {
                    bool[] texRefSettings = new bool[m.Children.Count];
                    foreach (MDL0MaterialRefNode r in m.Children)
                    {
                        texRefSettings[r.Index] = r.HasTextureMatrix;
                    }

                    if (!texMatrixSettings.ContainsKey(m.Name))
                    {
                        texMatrixSettings.Add(m.Name, texRefSettings);
                    }
                }
            }

            bool[] addGroup = new bool[8];
            while (replacement._objList != null && replacement._objList.Count > 0)
            {
                ReplaceOrAddMeshInternal(
                    replacement._objList[0] as MDL0ObjectNode,
                    ref addGroup,
                    doSearch,
                    replaceIfFound,
                    addIfNotFound);
            }

            FinishReplace(addGroup);

            // Copy texture matrix settings from before
            if (_matGroup != null && _matGroup.Children.Count > 0)
            {
                foreach (MDL0MaterialNode m in _matGroup.Children.Where(m => m.HasChildren && texMatrixSettings.ContainsKey(m.Name)))
                {
                    if (texMatrixSettings[m.Name].Length == m.Children.Count)
                    {
                        foreach (MDL0MaterialRefNode r in m.Children)
                        {
                            r.HasTextureMatrix = texMatrixSettings[m.Name][r.Index];
                        }
                    }
                }
            }
        }

        private void FinishReplace(bool[] addGroup)
        {
            if (addGroup[0])
            {
                if (_vertGroup != null && _vertGroup.Children.Count > 0)
                {
                    _children.Add(_vertGroup);
                }
                else
                {
                    UnlinkGroup(_vertGroup);
                }
            }

            if (addGroup[1])
            {
                if (_normGroup != null && _normGroup.Children.Count > 0)
                {
                    _children.Add(_normGroup);
                }
                else
                {
                    UnlinkGroup(_normGroup);
                }
            }

            if (addGroup[2])
            {
                if (_colorGroup != null && _colorGroup.Children.Count > 0)
                {
                    _children.Add(_colorGroup);
                }
                else
                {
                    UnlinkGroup(_colorGroup);
                }
            }

            if (addGroup[3])
            {
                if (_uvGroup != null && _uvGroup.Children.Count > 0)
                {
                    _children.Add(_uvGroup);
                }
                else
                {
                    UnlinkGroup(_uvGroup);
                }
            }

            if (addGroup[4])
            {
                if (_objGroup != null && _objGroup.Children.Count > 0)
                {
                    _children.Add(_objGroup);
                }
                else
                {
                    UnlinkGroup(_objGroup);
                }
            }

            if (addGroup[5])
            {
                if (_matGroup != null && _matGroup.Children.Count > 0)
                {
                    _children.Add(_matGroup);
                }
                else
                {
                    UnlinkGroup(_matGroup);
                }
            }

            if (addGroup[6])
            {
                if (_shadGroup != null && _shadGroup.Children.Count > 0)
                {
                    _children.Add(_shadGroup);
                }
                else
                {
                    UnlinkGroup(_shadGroup);
                }
            }

            if (addGroup[7])
            {
                if (_boneGroup != null && _boneGroup.Children.Count > 0)
                {
                    _children.Add(_boneGroup);
                }
                else
                {
                    UnlinkGroup(_boneGroup);
                }
            }

            Influences.Clean();
            Influences.Sort();
        }

        public void ConvertToShadowModel()
        {
            if (_matGroup == null)
            {
                Populate();
                if (_matGroup == null || _boneGroup == null || _objGroup == null)
                {
                    return;
                }
            }

            // Implementation with support for multiple culling types
            MDL0MaterialNode mat1 = new MDL0MaterialNode();
            _matGroup.AddChild(mat1);
            mat1.GenerateShadowMaterial();
            //mat1.Name += "_CullNone";
            mat1.CullMode = CullMode.Cull_None;

            MDL0MaterialNode mat2 = new MDL0MaterialNode();
            _matGroup.AddChild(mat2);
            mat2.GenerateShadowMaterial();
            //mat2.Name += "_CullInside";
            mat2.CullMode = CullMode.Cull_Inside;

            MDL0MaterialNode mat3 = new MDL0MaterialNode();
            _matGroup.AddChild(mat3);
            mat3.GenerateShadowMaterial();
            //mat3.Name += "_CullOutside";
            mat3.CullMode = CullMode.Cull_Outside;

            MDL0MaterialNode mat4 = new MDL0MaterialNode();
            _matGroup.AddChild(mat4);
            mat4.GenerateShadowMaterial();
            //mat4.Name += "_CullAll";
            mat4.CullMode = CullMode.Cull_All;

            // Properly remove all shaders
            if (ShaderList != null)
            {
                int shaderNum = ShaderGroup.Children.Count;
                for (int i = 0; i < shaderNum; i++)
                {
                    ResourceNode s = ShaderGroup.Children[0];
                    ShaderGroup.RemoveChild(s);
                }

                //Children.Remove(ShaderGroup);
            }

            // Add a new shader
            if (_shadGroup == null)
            {
                MDL0GroupNode g = _shadGroup;
                if (g == null)
                {
                    AddChild(g = new MDL0GroupNode(MDLResourceType.Shaders), true);
                    _shadGroup = g;
                    _shadList = g.Children;
                }
            }

            if (_shadList.Count == 0)
            {
                if (_shadList != null && _matList != null && _shadList.Count < _matList.Count)
                {
                    MDL0ShaderNode shader = new MDL0ShaderNode();
                    _shadGroup.AddChild(shader);
                    shader.DefaultAsShadow();
                    shader.Rebuild(true);
                }
            }

            mat1.ShaderNode = (MDL0ShaderNode) _shadList[0];
            mat2.ShaderNode = (MDL0ShaderNode) _shadList[0];
            mat3.ShaderNode = (MDL0ShaderNode) _shadList[0];
            mat4.ShaderNode = (MDL0ShaderNode) _shadList[0];
            mat1.Rebuild(true);
            mat2.Rebuild(true);
            mat3.Rebuild(true);
            mat4.Rebuild(true);

            if (_objGroup == null)
            {
                return;
            }

            bool usesCullOutside = false;
            bool usesCullInside = false;
            bool usesCullNone = false;
            bool usesCullAll = false;
            foreach (MDL0ObjectNode m in _objGroup.Children)
            {
                m.TextureMatrix0Enabled = false;
                m.TextureMatrix1Enabled = false;
                m.TextureMatrix2Enabled = false;
                m.TextureMatrix3Enabled = false;
                m.TextureMatrix4Enabled = false;
                m.TextureMatrix5Enabled = false;
                m.TextureMatrix6Enabled = false;
                m.TextureMatrix7Enabled = false;

                m.TextureMatrix0Identity = false;
                m.TextureMatrix1Identity = false;
                m.TextureMatrix2Identity = false;
                m.TextureMatrix3Identity = false;
                m.TextureMatrix4Identity = false;
                m.TextureMatrix5Identity = false;
                m.TextureMatrix6Identity = false;
                m.TextureMatrix7Identity = false;

                foreach (DrawCall c in m.DrawCalls)
                {
                    CullMode cullToUse = c.MaterialNode.CullMode;

                    switch (cullToUse)
                    {
                        case CullMode.Cull_None:
                            c.MaterialNode = mat1;
                            usesCullNone = true;
                            break;
                        case CullMode.Cull_Inside:
                            c.MaterialNode = mat2;
                            usesCullInside = true;
                            break;
                        case CullMode.Cull_Outside:
                            c.MaterialNode = mat3;
                            usesCullOutside = true;
                            break;
                        case CullMode.Cull_All:
                            c.MaterialNode = mat4;
                            usesCullAll = true;
                            break;
                        default: break;
                    }

                    c.DrawPass = DrawCall.DrawPassType.Transparent;
                }
            }

            // Delete unused materials
            while (MaterialGroup.Children.Count > 4)
            {
                MaterialGroup.RemoveChild(MaterialGroup.Children[0]);
            }

            if (!usesCullAll)
            {
                MaterialGroup.RemoveChild(mat4);
            }

            if (!usesCullOutside)
            {
                MaterialGroup.RemoveChild(mat3);
            }

            if (!usesCullInside)
            {
                MaterialGroup.RemoveChild(mat2);
            }

            if (!usesCullNone)
            {
                MaterialGroup.RemoveChild(mat1);
            }

            if (MaterialGroup.Children.Count == 1 && MaterialGroup.Children[0].Name.IndexOf("_") > 0)
            {
                MaterialGroup.Children[0].Name = MaterialGroup.Children[0].Name
                    .Substring(0,
                        MaterialGroup.Children[0].Name.IndexOf("_"));
            }

            if (BoneGroup == null)
            {
                Populate();
            }

            MDL0BoneNode b = (MDL0BoneNode) BoneGroup.Children[0];
            b.Name = b.Name + "_NShadow";
            b.Scale = new Vector3(b.Scale._x * 1.01f, b.Scale._y * 1.01f, b.Scale._z * 1.01f);

            if (Name.Length > 4)
            {
                if (Name.Substring(Name.Length - 4) == " (2)")
                {
                    Name = Name.Substring(0, Name.Length - 4);
                }
            }

            Name = Name + "_Shadow";
        }

        public static bool MultiTypeWorks = false;

        public void ConvertToSpyModel()
        {
            if (_matGroup == null)
            {
                return;
            }

            Name = Name + "Spy";

            if (MultiTypeWorks)
            {
                ConvertToSpyModelMultiType();
                return;
            }

            // Material only works properly with the name SpyCloak, so it needs to select the correct culling type
            MDL0MaterialNode mat1 = new MDL0MaterialNode();
            _matGroup.AddChild(mat1);
            mat1.GenerateSpyMaterial();
            mat1.CullMode = CullMode.Cull_None;

            MDL0MaterialNode matCulled = new MDL0MaterialNode();
            _matGroup.AddChild(matCulled);
            matCulled.GenerateSpyMaterial();
            matCulled.Name += "_CullAll";
            matCulled.CullMode = CullMode.Cull_All;

            // Properly remove all shaders
            if (ShaderList != null)
            {
                int shaderNum = ShaderGroup.Children.Count;
                for (int i = 0; i < shaderNum; i++)
                {
                    ResourceNode s = ShaderGroup.Children[0];
                    ShaderGroup.RemoveChild(s);
                }

                //Children.Remove(ShaderGroup);
            }

            // Add a new shader
            if (_shadGroup == null)
            {
                MDL0GroupNode g = _shadGroup;
                if (g == null)
                {
                    AddChild(g = new MDL0GroupNode(MDLResourceType.Shaders), true);
                    _shadGroup = g;
                    _shadList = g.Children;
                }
            }

            if (_shadList.Count == 0)
            {
                if (_shadList != null && _matList != null && _shadList.Count < _matList.Count)
                {
                    MDL0ShaderNode shader = new MDL0ShaderNode();
                    _shadGroup.AddChild(shader);
                    shader.DefaultAsSpy();
                    shader.Rebuild(true);
                }
            }

            // Add the color
            MDL0GroupNode colorG = _colorGroup;
            if (colorG == null)
            {
                AddChild(colorG = new MDL0GroupNode(MDLResourceType.Colors), true);
                _colorGroup = colorG;
                _colorList = colorG.Children;
            }

            MDL0ColorNode colorNode = new MDL0ColorNode
            {
                Name = Name + "_BodyM__" + Name + "_Spycloak",
                Colors = new RGBAPixel[] {new RGBAPixel {A = 255, R = 132, G = 130, B = 132}}
            };
            colorG.AddChild(colorNode, true);

            mat1.ShaderNode = (MDL0ShaderNode) _shadList[0];
            mat1.Rebuild(true);

            if (_objGroup == null)
            {
                return;
            }

            bool usesCullOutside = false;
            bool usesCullInside = false;
            bool usesCullNone = false;
            bool usesCullAll = false;
            foreach (MDL0ObjectNode m in _objGroup.Children)
            {
                foreach (DrawCall c in m.DrawCalls)
                {
                    CullMode cullToUse = c.MaterialNode.CullMode;
                    c.MaterialNode = mat1;
                    switch (cullToUse)
                    {
                        case CullMode.Cull_None:
                            usesCullNone = true;
                            break;
                        case CullMode.Cull_Inside:
                            usesCullInside = true;
                            break;
                        case CullMode.Cull_Outside:
                            usesCullOutside = true;
                            break;
                        case CullMode.Cull_All:
                            c.MaterialNode = matCulled;
                            usesCullAll = true;
                            break;
                        default: break;
                    }

                    c.DrawPass = DrawCall.DrawPassType.Opaque;
                }

                for (int i = 0; i < 8; i++)
                {
                    m.SetUVs(i, null, true);
                }

                m.SetColors(0, colorNode.Name, true);
                m.SetColors(1, null, true);

                m.TextureMatrix0Enabled = true;
                m.TextureMatrix1Enabled = true;
                m.TextureMatrix2Enabled = false;
                m.TextureMatrix3Enabled = false;
                m.TextureMatrix4Enabled = false;
                m.TextureMatrix5Enabled = false;
                m.TextureMatrix6Enabled = false;
                m.TextureMatrix7Enabled = false;

                m.TextureMatrix0Identity = true;
                m.TextureMatrix1Identity = false;
                m.TextureMatrix2Identity = false;
                m.TextureMatrix3Identity = false;
                m.TextureMatrix4Identity = false;
                m.TextureMatrix5Identity = false;
                m.TextureMatrix6Identity = false;
                m.TextureMatrix7Identity = false;
            }

            // Delete Unused Colors
            while (ColorGroup.Children.Count > 1)
            {
                ColorGroup.RemoveChild(ColorGroup.Children[0]);
            }


            // Delete unused materials
            while (MaterialGroup.Children.Count > 2)
            {
                MaterialGroup.RemoveChild(MaterialGroup.Children[0]);
            }

            // Choose proper culling based on best usage
            if (usesCullNone || usesCullInside && usesCullOutside)
            {
                mat1.CullMode = CullMode.Cull_None;
            }
            else if (usesCullInside)
            {
                mat1.CullMode = CullMode.Cull_Inside;
            }
            else if (usesCullOutside)
            {
                mat1.CullMode = CullMode.Cull_Outside;
            }

            if (!usesCullAll)
            {
                MaterialGroup.RemoveChild(matCulled);
            }

            foreach (MDL0MaterialNode mat in _matGroup.Children)
            {
                foreach (MDL0MaterialRefNode matref in mat.Children)
                {
                    matref.HasTextureMatrix = true;
                }
            }
        }

        public void ConvertToSpyModelMultiType()
        {
            if (_matGroup == null)
            {
                return;
            }

            // Implementation with support for multiple culling types
            // Will all be named the same as the material appears to need to be named "Spycloak"
            MDL0MaterialNode mat1 = new MDL0MaterialNode();
            _matGroup.AddChild(mat1);
            mat1.GenerateSpyMaterial();
            //mat1.Name += "_CullNone";
            mat1.CullMode = CullMode.Cull_None;

            MDL0MaterialNode mat2 = new MDL0MaterialNode();
            _matGroup.AddChild(mat2);
            mat2.GenerateSpyMaterial();
            //mat2.Name += "_CullInside";
            mat2.CullMode = CullMode.Cull_Inside;

            MDL0MaterialNode mat3 = new MDL0MaterialNode();
            _matGroup.AddChild(mat3);
            mat3.GenerateSpyMaterial();
            //mat3.Name += "_CullOutside";
            mat3.CullMode = CullMode.Cull_Outside;

            MDL0MaterialNode mat4 = new MDL0MaterialNode();
            _matGroup.AddChild(mat4);
            mat4.GenerateSpyMaterial();
            //mat4.Name += "_CullAll";
            mat4.CullMode = CullMode.Cull_All;

            // Properly remove all shaders
            if (ShaderList != null)
            {
                int shaderNum = ShaderGroup.Children.Count;
                for (int i = 0; i < shaderNum; i++)
                {
                    ResourceNode s = ShaderGroup.Children[0];
                    ShaderGroup.RemoveChild(s);
                }

                //Children.Remove(ShaderGroup);
            }

            // Add a new shader
            if (_shadGroup == null)
            {
                MDL0GroupNode g = _shadGroup;
                if (g == null)
                {
                    AddChild(g = new MDL0GroupNode(MDLResourceType.Shaders), true);
                    _shadGroup = g;
                    _shadList = g.Children;
                }
            }

            if (_shadList.Count == 0)
            {
                if (_shadList != null && _matList != null && _shadList.Count < _matList.Count)
                {
                    MDL0ShaderNode shader = new MDL0ShaderNode();
                    _shadGroup.AddChild(shader);
                    shader.DefaultAsSpy();
                    shader.Rebuild(true);
                }
            }

            // Add the color
            MDL0GroupNode colorG = _colorGroup;
            if (colorG == null)
            {
                AddChild(colorG = new MDL0GroupNode(MDLResourceType.Colors), true);
                _colorGroup = colorG;
                _colorList = colorG.Children;
            }

            MDL0ColorNode colorNode = new MDL0ColorNode
            {
                Name = Name + "_BodyM__" + Name + "_Spycloak",
                Colors = new RGBAPixel[] {new RGBAPixel {A = 255, R = 132, G = 130, B = 132}}
            };
            colorG.AddChild(colorNode, true);

            mat1.ShaderNode = (MDL0ShaderNode) _shadList[0];
            mat2.ShaderNode = (MDL0ShaderNode) _shadList[0];
            mat3.ShaderNode = (MDL0ShaderNode) _shadList[0];
            mat4.ShaderNode = (MDL0ShaderNode) _shadList[0];
            mat1.Rebuild(true);
            mat2.Rebuild(true);
            mat3.Rebuild(true);
            mat4.Rebuild(true);

            if (_objGroup == null)
            {
                return;
            }

            bool usesCullOutside = false;
            bool usesCullInside = false;
            bool usesCullNone = false;
            bool usesCullAll = false;
            foreach (MDL0ObjectNode m in _objGroup.Children)
            {
                foreach (DrawCall c in m.DrawCalls)
                {
                    CullMode cullToUse = c.MaterialNode.CullMode;
                    switch (cullToUse)
                    {
                        case CullMode.Cull_None:
                            c.MaterialNode = mat1;
                            usesCullNone = true;
                            break;
                        case CullMode.Cull_Inside:
                            c.MaterialNode = mat2;
                            usesCullInside = true;
                            break;
                        case CullMode.Cull_Outside:
                            c.MaterialNode = mat3;
                            usesCullOutside = true;
                            break;
                        case CullMode.Cull_All:
                            c.MaterialNode = mat4;
                            usesCullAll = true;
                            break;
                        default: break;
                    }

                    c.DrawPass = DrawCall.DrawPassType.Opaque;
                }

                for (int i = 0; i < 8; i++)
                {
                    m.SetUVs(i, null, true);
                }

                m.SetColors(0, colorNode.Name, true);
                m.SetColors(1, null, true);

                m.TextureMatrix0Enabled = true;
                m.TextureMatrix1Enabled = true;
                m.TextureMatrix2Enabled = false;
                m.TextureMatrix3Enabled = false;
                m.TextureMatrix4Enabled = false;
                m.TextureMatrix5Enabled = false;
                m.TextureMatrix6Enabled = false;
                m.TextureMatrix7Enabled = false;

                m.TextureMatrix0Identity = true;
                m.TextureMatrix1Identity = false;
                m.TextureMatrix2Identity = false;
                m.TextureMatrix3Identity = false;
                m.TextureMatrix4Identity = false;
                m.TextureMatrix5Identity = false;
                m.TextureMatrix6Identity = false;
                m.TextureMatrix7Identity = false;
            }

            // Delete Unused Colors
            while (ColorGroup.Children.Count > 1)
            {
                ColorGroup.RemoveChild(ColorGroup.Children[0]);
            }

            // Delete unused materials
            while (MaterialGroup.Children.Count > 4)
            {
                MaterialGroup.RemoveChild(MaterialGroup.Children[0]);
            }

            if (!usesCullAll)
            {
                MaterialGroup.RemoveChild(mat4);
            }

            if (!usesCullOutside)
            {
                MaterialGroup.RemoveChild(mat3);
            }

            if (!usesCullInside)
            {
                MaterialGroup.RemoveChild(mat2);
            }

            if (!usesCullNone)
            {
                MaterialGroup.RemoveChild(mat1);
            }

            if (MaterialGroup.Children.Count == 1 && MaterialGroup.Children[0].Name.IndexOf("_") > 0)
            {
                MaterialGroup.Children[0].Name = MaterialGroup.Children[0].Name
                    .Substring(0,
                        MaterialGroup.Children[0].Name.IndexOf("_"));
            }

            foreach (MDL0MaterialNode mat in _matGroup.Children)
            {
                foreach (MDL0MaterialRefNode matref in mat.Children)
                {
                    matref.HasTextureMatrix = true;
                }
            }
        }

        public void StripModel()
        {
            Populate();
            while (_objGroup != null && _objGroup.HasChildren)
            {
                ((MDL0ObjectNode) _objGroup.Children[0]).Remove(true);
            }

            while (_matGroup != null && _matGroup.HasChildren)
            {
                ((MDL0MaterialNode) _matGroup.Children[0]).Remove(true);
            }

            while (_shadGroup != null && _shadGroup.HasChildren)
            {
                _shadGroup.Children[0].Remove();
            }

            while (_uvGroup != null && _uvGroup.HasChildren)
            {
                _uvGroup.Children[0].Remove();
            }

            while (_normGroup != null && _normGroup.HasChildren)
            {
                _normGroup.Children[0].Remove();
            }

            while (_vertGroup != null && _vertGroup.HasChildren)
            {
                _vertGroup.Children[0].Remove();
            }

            while (_colorGroup != null && _colorGroup.HasChildren)
            {
                _colorGroup.Children[0].Remove();
            }

            while (_pltGroup != null && _pltGroup.HasChildren)
            {
                _pltGroup.Children[0].Remove();
            }
        }

        #endregion

        #region Linking

        public void LinkGroup(MDL0GroupNode group)
        {
            switch (group._type)
            {
                case MDLResourceType.Definitions:
                {
                    _defGroup = group;
                    _defList = group._children;
                    break;
                }

                case MDLResourceType.Bones:
                {
                    _boneGroup = group;
                    _boneList = group._children;
                    break;
                }

                case MDLResourceType.Materials:
                {
                    _matGroup = group;
                    _matList = group._children;
                    break;
                }

                case MDLResourceType.Shaders:
                {
                    _shadGroup = group;
                    _shadList = group._children;
                    break;
                }

                case MDLResourceType.Vertices:
                {
                    _vertGroup = group;
                    _vertList = group._children;
                    break;
                }

                case MDLResourceType.Normals:
                {
                    _normGroup = group;
                    _normList = group._children;
                    break;
                }

                case MDLResourceType.UVs:
                {
                    _uvGroup = group;
                    _uvList = group._children;
                    break;
                }

                case MDLResourceType.Colors:
                {
                    _colorGroup = group;
                    _colorList = group._children;
                    break;
                }

                case MDLResourceType.Objects:
                {
                    _objGroup = group;
                    _objList = group._children;
                    break;
                }

                case MDLResourceType.Textures:
                {
                    _texGroup = group;
                    _texList = group._children;
                    break;
                }

                case MDLResourceType.Palettes:
                {
                    _pltGroup = group;
                    _pltList = group._children;
                    break;
                }

                case MDLResourceType.FurLayerCoords:
                {
                    _furPosGroup = group;
                    _furPosList = group._children;
                    break;
                }

                case MDLResourceType.FurVectors:
                {
                    _furVecGroup = group;
                    _furVecList = group._children;
                    break;
                }
            }
        }

        public void UnlinkGroup(MDL0GroupNode group)
        {
            if (group != null)
            {
                switch (group._type)
                {
                    case MDLResourceType.Definitions:
                    {
                        _defGroup = null;
                        _defList = null;
                        break;
                    }

                    case MDLResourceType.Bones:
                    {
                        _boneGroup = null;
                        _boneList = null;
                        break;
                    }

                    case MDLResourceType.Materials:
                    {
                        _matGroup = null;
                        _matList = null;
                        break;
                    }

                    case MDLResourceType.Shaders:
                    {
                        _shadGroup = null;
                        _shadList = null;
                        break;
                    }

                    case MDLResourceType.Vertices:
                    {
                        _vertGroup = null;
                        _vertList = null;
                        break;
                    }

                    case MDLResourceType.Normals:
                    {
                        _normGroup = null;
                        _normList = null;
                        break;
                    }

                    case MDLResourceType.UVs:
                    {
                        _uvGroup = null;
                        _uvList = null;
                        break;
                    }

                    case MDLResourceType.Colors:
                    {
                        _colorGroup = null;
                        _colorList = null;
                        break;
                    }

                    case MDLResourceType.Objects:
                    {
                        _objGroup = null;
                        _objList = null;
                        break;
                    }

                    case MDLResourceType.Textures:
                    {
                        _texGroup = null;
                        _texList = null;
                        break;
                    }

                    case MDLResourceType.Palettes:
                    {
                        _pltGroup = null;
                        _pltList = null;
                        break;
                    }

                    case MDLResourceType.FurLayerCoords:
                    {
                        _furPosGroup = null;
                        _furPosList = null;
                        break;
                    }

                    case MDLResourceType.FurVectors:
                    {
                        _furVecGroup = null;
                        _furVecList = null;
                        break;
                    }
                }
            }
        }

        internal void InitGroups()
        {
            LinkGroup(new MDL0GroupNode(MDLResourceType.Definitions));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Bones));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Materials));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Shaders));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Vertices));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Normals));
            LinkGroup(new MDL0GroupNode(MDLResourceType.UVs));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Colors));
            LinkGroup(new MDL0GroupNode(MDLResourceType.FurVectors));
            LinkGroup(new MDL0GroupNode(MDLResourceType.FurLayerCoords));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Objects));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Textures));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Palettes));

            _defGroup._parent = this;
            _boneGroup._parent = this;
            _matGroup._parent = this;
            _shadGroup._parent = this;
            _vertGroup._parent = this;
            _normGroup._parent = this;
            _uvGroup._parent = this;
            _colorGroup._parent = this;
            _furPosGroup._parent = this;
            _furVecGroup._parent = this;
            _objGroup._parent = this;
            _texGroup._parent = this;
            _pltGroup._parent = this;
        }

        internal void CleanGroups()
        {
            if (_defList.Count > 0)
            {
                _children.Add(_defGroup);
            }
            else
            {
                UnlinkGroup(_defGroup);
            }

            if (_boneList.Count > 0)
            {
                _children.Add(_boneGroup);
            }
            else
            {
                UnlinkGroup(_boneGroup);
            }

            if (_matList.Count > 0)
            {
                _children.Add(_matGroup);
            }
            else
            {
                UnlinkGroup(_matGroup);
            }

            if (_shadList.Count > 0)
            {
                _children.Add(_shadGroup);
            }
            else
            {
                UnlinkGroup(_shadGroup);
            }

            if (_vertList.Count > 0)
            {
                _children.Add(_vertGroup);
            }
            else
            {
                UnlinkGroup(_vertGroup);
            }

            if (_normList.Count > 0)
            {
                _children.Add(_normGroup);
            }
            else
            {
                UnlinkGroup(_normGroup);
            }

            if (_uvList.Count > 0)
            {
                _children.Add(_uvGroup);
            }
            else
            {
                UnlinkGroup(_uvGroup);
            }

            if (_colorList.Count > 0)
            {
                _children.Add(_colorGroup);
            }
            else
            {
                UnlinkGroup(_colorGroup);
            }

            if (_furPosList.Count > 0)
            {
                _children.Add(_furPosGroup);
            }
            else
            {
                UnlinkGroup(_furPosGroup);
            }

            if (_furVecList.Count > 0)
            {
                _children.Add(_furVecGroup);
            }
            else
            {
                UnlinkGroup(_furVecGroup);
            }

            if (_objList.Count > 0)
            {
                _children.Add(_objGroup);
            }
            else
            {
                UnlinkGroup(_objGroup);
            }

            if (_texList.Count > 0)
            {
                _children.Add(_texGroup);
            }
            else
            {
                UnlinkGroup(_texGroup);
            }

            if (_pltList.Count > 0)
            {
                _children.Add(_pltGroup);
            }
            else
            {
                UnlinkGroup(_pltGroup);
            }
        }

        #endregion

        #region Parsing

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _billboardBones = new List<MDL0BoneNode>();
            _errors = new List<string>();
            _influences = new InfluenceManager();

            MDL0Header* header = Header;

            if (_name == null && header->StringOffset != 0)
            {
                _name = header->ResourceString;
            }

            MDL0Props* props = header->Properties;

            if (props != null)
            {
                _scalingRule = props->_scalingRule;
                _texMtxMode = props->_texMatrixMode;
                _numFacepoints = props->_numVertices;
                _numTriangles = props->_numTriangles;
                _numNodes = props->_numNodes;
                _needsNrmMtxArray = props->_needNrmMtxArray != 0;
                _needsTexMtxArray = props->_needTexMtxArray != 0;
                _extents = props->_extents;
                _enableExtents = props->_enableExtents != 0;
                _envMtxMode = props->_envMtxMode;

                if (props->_origPathOffset > 0 && props->_origPathOffset < WorkingUncompressed.Length)
                {
                    _originalPath = props->OrigPath;
                }
                else if (props->_origPathOffset > 0)
                {
                    _errors.Add("Original path was found to be out of range");
                }
            }

            (_userEntries = new UserDataCollection()).Read(header->UserData, RootNode.WorkingUncompressed);

            return true;
        }

        public override void OnPopulate()
        {
            try
            {
                InitGroups();
                _linker = new ModelLinker(Header) {Model = this};
                _assets = new AssetStorage(_linker);

                //Set def flags
                _hasMix = _hasOpa = _hasTree = _hasXlu = false;
                if (_linker.Defs != null)
                {
                    foreach (ResourcePair p in *_linker.Defs)
                    {
                        if (p.Name == "NodeTree")
                        {
                            _hasTree = true;
                        }
                        else if (p.Name == "NodeMix")
                        {
                            _hasMix = true;
                        }
                        else if (p.Name == "DrawOpa")
                        {
                            _hasOpa = true;
                        }
                        else if (p.Name == "DrawXlu")
                        {
                            _hasXlu = true;
                        }
                    }
                }

                //These cause some complications if not parsed...
                _texGroup.Parse(this);
                _pltGroup.Parse(this);

                _defGroup.Parse(this);
                _boneGroup.Parse(this);
                _matGroup.Parse(this);
                _shadGroup.Parse(this);
                _vertGroup.Parse(this);
                _normGroup.Parse(this);
                _uvGroup.Parse(this);
                _colorGroup.Parse(this);

                if (Version >= 10)
                {
                    _furVecGroup.Parse(this);
                    _furPosGroup.Parse(this);
                }

                _objGroup.Parse(this); //Parse objects last!

                _texList.Sort();
                _pltList.Sort();

                if (_matGroup != null)
                {
                    foreach (MDL0MaterialNode m in _matGroup.Children)
                    {
                        if (m.IsMetal && m.Children != null && m.Children.Count > 0)
                        {
                            _metalMat = m.Children[m.Children.Count - 1].Name;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _errors.Add("Something went wrong parsing the model: " + ex);
            }
            finally //Clean up!
            {
                //We'll use the linker to access the bone cache
                //_linker = null;

                //Don't dispose assets, in case an object is replaced
                //_assets.Dispose();
                //_assets = null;

                CleanGroups();

                //Check for model errors
                if (_errors.Count > 0)
                {
                    if (!SupportedVersions.Contains(_version))
                    {
                        MessageBox.Show("The model " + _name + " has a version of " + _version +
                                        " which is not supported. The model may be corrupt and data maybe be lost if you save the model.");
                    }
                    else
                    {
                        string message = _errors.Count + (_errors.Count > 1 ? " errors have" : " error has") +
                                         " been found in the model " + _name + ".\n" +
                                         (_errors.Count > 1 ? "These errors" : "This error") +
                                         " will be fixed when you save:";
                        foreach (string s in _errors)
                        {
                            message += "\n - " + s;
                        }

                        if (!Properties.Settings.Default.HideMDL0Errors)
                        {
                            MessageBox.Show(message);
                        }

                        SignalPropertyChange();
                    }
                }
            }
        }

        public void BeginImport()
        {
            _isImport = true;
            InitGroups();
        }

        public void FinishImport(Collada form = null)
        {
            //Prepare for rebuild
            CleanTextures();
            CleanGroups();
            _influences.Clean();
            _influences.Sort();
            _linker = ModelLinker.Prepare(this);

            //Calculate size and get strings
            int size = (_calcSize = ModelEncoder.CalcSize(form, _linker)).Align(4);
            StringTable table = new StringTable();
            GetStrings(table);

            //Create temp file and write model and string table, then post process strings, etc
            FileMap uncompMap = FileMap.FromTempFile(size + table.GetTotalSize());
            ModelEncoder.Build(form, _linker, (MDL0Header*) uncompMap.Address, _calcSize, true);
            table.WriteTable(uncompMap.Address + size);
            PostProcess(null, uncompMap.Address, _calcSize, table);

            table.Clear();
            _isImport = false;

            _origSource = _uncompSource = new DataSource(uncompMap);

            if (_children != null)
            {
                foreach (ResourceNode node in _children)
                {
                    node.Dispose();
                }

                _children.Clear();
                _children = null;
            }

            if (!OnInitialize())
            {
                _children = new List<ResourceNode>();
            }

            IsDirty = false;
        }

        public static MDL0Node FromFile(string path, FileOptions options = FileOptions.RandomAccess)
        {
            if (path.EndsWith(".dae", StringComparison.OrdinalIgnoreCase))
            {
                return new Collada {Text = $"Import Settings - {Path.GetFileName(path)}"}.ShowDialog(path,
                    Collada.ImportType.MDL0) as MDL0Node;
            }

            if (path.EndsWith(".pmd", StringComparison.OrdinalIgnoreCase))
            {
                return PMDModel.ImportModel(path);
            }

            return NodeFactory.FromFile(null, path, options) as MDL0Node;
        }

        #endregion

        #region Saving

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (MDL0GroupNode n in Children)
            {
                n.GetStrings(table);
            }

            _hasOpa = _hasXlu = false;

            //Can't use XLUMaterial bool in materials
            //because it's not guaranteed on when an object uses the material as XLU
            if (_objList != null)
            {
                foreach (MDL0ObjectNode n in _objList)
                {
                    if (_hasOpa && _hasXlu)
                    {
                        break;
                    }

                    foreach (DrawCall c in n._drawCalls)
                    {
                        if (c.DrawPass == DrawCall.DrawPassType.Transparent)
                        {
                            _hasXlu = true;
                        }
                        else
                        {
                            _hasOpa = true;
                        }
                    }
                }
            }

            //Add def names
            if (_hasTree)
            {
                table.Add("NodeTree");
            }

            if (_hasMix)
            {
                table.Add("NodeMix");
            }

            if (_hasOpa)
            {
                table.Add("DrawOpa");
            }

            if (_hasXlu)
            {
                table.Add("DrawXlu");
            }

            if (_version > 9)
            {
                _userEntries.GetStrings(table);
            }

            if (!string.IsNullOrEmpty(_originalPath))
            {
                table.Add(_originalPath);
            }

            if (_isImport)
            {
                int index = 0;
                if (_linker._vertices != null)
                {
                    foreach (VertexCodec c in _linker._vertices)
                    {
                        string name = Name + "_" + _objList[index]._name;
                        MDL0ObjectNode n = (MDL0ObjectNode)_objList[index];
                        if (n._drawCalls.Count > 0 && n._drawCalls[0].MaterialNode != null)
                        {
                            name += "_" + ((MDL0ObjectNode)_objList[index])._drawCalls[0].MaterialNode._name;
                        }

                        table.Add(name);
                        index++;
                    }
                }

                index = 0;
                if (_linker._uvs != null)
                {
                    foreach (VertexCodec c in _linker._uvs)
                    {
                        table.Add("#" + index++);
                    }
                }
            }
        }

        public override void Replace(string fileName, FileMapProtect prot, FileOptions options)
        {
            MDL0Node node = FromFile(fileName, FileOptions.SequentialScan);
            if (node == null)
            {
                return;
            }

            //Get the original data source from the newly created model
            //and clear the reference to it so it's not disposed of
            //when the model is disposed
            DataSource m = node._uncompSource;
            node._uncompSource = DataSource.Empty;
            node._origSource = DataSource.Empty;

            node.Dispose();
            ReplaceRaw(m.Map);
        }

        public override void Export(string outPath)
        {
            if (outPath.ToUpper().EndsWith(".DAE"))
            {
                Collada.Serialize(this, outPath);
            }
            //else if (outPath.ToUpper().EndsWith(".PMD"))
            //    PMDModel.Export(this, outPath);
            //else if (outPath.ToUpper().EndsWith(".RMDL"))
            //    XMLExporter.ExportRMDL(this, outPath);
            else
            {
                base.Export(outPath);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            //Clean and sort influence list
            _influences.Clean();
            //_influences.Sort();

            //Clean texture list
            CleanTextures();

            _linker = ModelLinker.Prepare(this);
            return ModelEncoder.CalcSize(_linker);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ModelEncoder.Build(_linker, (MDL0Header*) address, length, force);
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength,
                                                     StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            MDL0Header* header = (MDL0Header*) dataAddress;
            ResourceGroup* pGroup, sGroup;
            ResourceEntry* pEntry, sEntry;
            bint* offsets = header->Offsets;
            int index, sIndex;

            //Model name
            header->ResourceStringAddress = stringTable[Name] + 4;

            if (!string.IsNullOrEmpty(_originalPath))
            {
                header->Properties->OrigPathAddress = stringTable[_originalPath] + 4;
            }

            //Post-process groups, using linker lists
            List<MDLResourceType> gList = ModelLinker.IndexBank[_version];
            foreach (MDL0GroupNode node in Children)
            {
                MDLResourceType type = (MDLResourceType) Enum.Parse(typeof(MDLResourceType), node.Name);
                if ((index = gList.IndexOf(type)) >= 0 && type != MDLResourceType.Shaders)
                {
                    int offset = offsets[index];
                    if (offset > 0)
                    {
                        node.PostProcess(dataAddress, dataAddress + offset, stringTable);
                    }
                }
            }

            //Post-process definitions
            index = gList.IndexOf(MDLResourceType.Definitions);
            pGroup = (ResourceGroup*) (dataAddress + offsets[index]);
            pGroup->_first = new ResourceEntry(0xFFFF, 0, 0, 0);
            pEntry = &pGroup->_first + 1;
            index = 1;
            if (_hasTree)
            {
                ResourceEntry.Build(pGroup, index++, (byte*) pGroup + (pEntry++)->_dataOffset,
                    (BRESString*) stringTable["NodeTree"]);
            }

            if (_hasMix)
            {
                ResourceEntry.Build(pGroup, index++, (byte*) pGroup + (pEntry++)->_dataOffset,
                    (BRESString*) stringTable["NodeMix"]);
            }

            if (_hasOpa)
            {
                ResourceEntry.Build(pGroup, index++, (byte*) pGroup + (pEntry++)->_dataOffset,
                    (BRESString*) stringTable["DrawOpa"]);
            }

            if (_hasXlu)
            {
                ResourceEntry.Build(pGroup, index++, (byte*) pGroup + (pEntry++)->_dataOffset,
                    (BRESString*) stringTable["DrawXlu"]);
            }

            //Link shader names using material list
            index = offsets[gList.IndexOf(MDLResourceType.Materials)];
            sIndex = offsets[gList.IndexOf(MDLResourceType.Shaders)];
            if (index > 0 && sIndex > 0)
            {
                pGroup = (ResourceGroup*) (dataAddress + index);
                sGroup = (ResourceGroup*) (dataAddress + sIndex);
                pEntry = &pGroup->_first + 1;
                sEntry = &sGroup->_first + 1;

                sGroup->_first = new ResourceEntry(0xFFFF, 0, 0, 0);
                index = pGroup->_numEntries;
                for (int i = 1; i <= index; i++)
                {
                    VoidPtr dataAddr = (VoidPtr) sGroup + (sEntry++)->_dataOffset;
                    ResourceEntry.Build(sGroup, i, dataAddr,
                        (BRESString*) ((byte*) pGroup + (pEntry++)->_stringOffset - 4));
                    ((MDL0Shader*) dataAddr)->_mdl0Offset = (int) dataAddress - (int) dataAddr;
                }
            }

            //Write part2 entries
            if (Version > 9)
            {
                _userEntries.PostProcess((VoidPtr) header + header->UserDataOffset, stringTable);
            }
        }

        #endregion

        #region Rendering

        [Browsable(false)]
        public IBoneNode[] BoneCache
        {
            get
            {
                if (_linker?.BoneCache != null)
                {
                    return _linker.BoneCache.Select(x => x as IBoneNode).ToArray();
                }

                return new IBoneNode[0];
            }
        }

        [Browsable(false)]
        public IBoneNode[] RootBones =>
            _boneList == null ? new IBoneNode[0] : _boneList.Select(x => x as IBoneNode).ToArray();

        [Browsable(false)]
        public bool IsRendering
        {
            get => DrawCalls.Where(x => x._render).Count() > 0;
            set
            {
                foreach (DrawCallBase b in DrawCalls)
                {
                    b._render = value;
                }
            }
        }

        [Browsable(false)]
        public bool IsTargetModel
        {
            get => _isTargetModel;
            set => _isTargetModel = value;
        }

        private bool _isTargetModel;

        public ModelRenderAttributes _renderAttribs = new ModelRenderAttributes();
        public bool _ignoreModelViewerAttribs = false;

        public int _selectedObjectIndex = -1;

        [Browsable(false)] public bool Attached => _attached;
        private bool _attached;
        private SHP0Node _currentSHP;
        private float _currentSHPIndex;

        public Dictionary<string, Dictionary<int, List<int>>> VIS0Indices;

        public void Attach()
        {
            _attached = true;
            foreach (MDL0GroupNode g in Children)
            {
                g.Bind();
            }

            RegenerateVIS0Indices();
        }

        /// <summary>
        /// This only needs to be called when the model is
        /// currently attached to a model renderer and
        /// the amount of objects change or an object's visibility bone changes.
        /// </summary>
        public void RegenerateVIS0Indices()
        {
            int i = 0;
            VIS0Indices = new Dictionary<string, Dictionary<int, List<int>>>();
            if (_objList != null)
            {
                foreach (MDL0ObjectNode p in _objList)
                {
                    int x = 0;
                    foreach (DrawCall c in p._drawCalls)
                    {
                        if (c._visBoneNode != null && c._visBoneNode.BoneIndex != 0)
                        {
                            if (!VIS0Indices.ContainsKey(c._visBoneNode.Name))
                            {
                                VIS0Indices.Add(c._visBoneNode.Name,
                                    new Dictionary<int, List<int>> {{i, new List<int> {x}}});
                            }
                            else if (!VIS0Indices[c._visBoneNode.Name].ContainsKey(i))
                            {
                                VIS0Indices[c._visBoneNode.Name].Add(i, new List<int> {x});
                            }
                            else if (!VIS0Indices[c._visBoneNode.Name][i].Contains(x))
                            {
                                VIS0Indices[c._visBoneNode.Name][i].Add(x);
                            }
                        }

                        x++;
                    }

                    i++;
                }
            }
        }

        public void Detach()
        {
            _attached = false;
            foreach (MDL0GroupNode g in Children)
            {
                g.Unbind();
            }
        }

        public void Refresh()
        {
            if (_texList != null)
            {
                foreach (MDL0TextureNode t in _texList)
                {
                    t.Reload(this, t.Parent?.Name.EndsWith("_ExtMtl") ?? false);
                }
            }
        }

        private float _scn0Frame;
        private SCN0Node _scn0;

        public void PreRender(ModelPanelViewport v)
        {
            if (_billboardBones.Count > 0)
            {
                WeightMeshes(v);
                ApplySHP(_currentSHP, _currentSHPIndex);
            }

            if (_matList != null)
            {
                foreach (MDL0MaterialNode m in _matList)
                {
                    foreach (MDL0MaterialRefNode mr in m.Children)
                    {
                        mr.SetEffectMatrix(_scn0, v, _scn0Frame);
                    }
                }
            }
        }

        public Matrix? _matrixOffset = null;

        public void RenderVertices(bool depthPass, IBoneNode weightTarget, GLCamera camera)
        {
            if (_objList != null)
            {
                if (_selectedObjectIndex != -1)
                {
                    MDL0ObjectNode o = (MDL0ObjectNode) _objList[_selectedObjectIndex];
                    if (o.IsRendering && o._manager != null)
                    {
                        o._manager.RenderVertices(o._matrixNode, weightTarget, depthPass, camera);
                        return;
                    }
                }
                else
                {
                    foreach (MDL0ObjectNode p in _objList)
                    {
                        if (p.IsRendering)
                        {
                            p._manager?.RenderVertices(p._matrixNode, weightTarget, depthPass, camera);
                        }
                    }
                }
            }
        }

        public void RenderNormals()
        {
            if (_objList != null)
            {
                if (_selectedObjectIndex != -1)
                {
                    MDL0ObjectNode o = (MDL0ObjectNode) _objList[_selectedObjectIndex];
                    if (o.IsRendering)
                    {
                        o._manager.RenderNormals();
                    }
                }
                else
                {
                    foreach (MDL0ObjectNode p in _objList)
                    {
                        if (p.IsRendering)
                        {
                            p._manager.RenderNormals();
                        }
                    }
                }
            }
        }

        public void RenderBoxes(bool model, bool obj, bool bone, bool bindState)
        {
            if (model || obj || bone)
            {
                GL.PushAttrib(AttribMask.AllAttribBits);

                GL.Disable(EnableCap.Lighting);
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

                bindState = _bindFrame && bindState;

                if (model)
                {
                    GL.Color4(Color.Gray);
                    DrawBox(bindState);
                }

                if (obj && _objList != null)
                {
                    GL.Color4(Color.Purple);
                    if (_selectedObjectIndex != -1 && ((MDL0ObjectNode) _objList[_selectedObjectIndex]).IsRendering)
                    {
                        ((MDL0ObjectNode) _objList[_selectedObjectIndex]).DrawBox();
                    }
                    else
                    {
                        foreach (MDL0ObjectNode p in _objList)
                        {
                            if (p.IsRendering)
                            {
                                p.DrawBox();
                            }
                        }
                    }
                }

                if (bone)
                {
                    GL.Color4(Color.Orange);
                    foreach (MDL0BoneNode b in _boneList)
                    {
                        b.DrawBox(true, bindState);
                    }
                }

                GL.PopAttrib();
            }
        }

        public void RenderBones(ModelPanelViewport v)
        {
            GL.PushAttrib(AttribMask.AllAttribBits);

            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.DepthTest);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.LineWidth(1.5f);

            if (_boneList != null)
            {
                foreach (MDL0BoneNode bone in _boneList)
                {
                    bone.Render(_isTargetModel, v);
                }
            }

            GL.PopAttrib();
        }

        [Browsable(false)]
        public int SelectedObjectIndex
        {
            get => _selectedObjectIndex;
            set => _selectedObjectIndex = value;
        }

        [Browsable(false)]
        public IObject[] Objects => _objList == null ? new IObject[0] : _objList.Select(x => x as IObject).ToArray();

        public void ResetToBindState()
        {
            ApplyCHR(null, 0);
            ApplySRT(null, 0);
            ApplyVIS(null, 0);
            ApplyPAT(null, 0);
            ApplyCLR(null, 0);
            ApplySCN(null, 0);
        }

        public void DrawBox(bool bindState)
        {
            Box box = bindState ? _extents : GetBox();
            //if (box.IsValid)
            TKContext.DrawWireframeBox(box);
        }

        public bool _dontUpdateMesh = false;
        private bool _bindFrame = true;

        public void ApplyCHR(CHR0Node node, float index)
        {
            _bindFrame = node == null || index == 0;

            //Transform bones
            if (_boneList != null)
            {
                foreach (MDL0BoneNode b in _boneList)
                {
                    b.ApplyCHR0(node, index);
                }
            }

            WeightMeshes();
        }

        public void WeightMeshes(ModelPanelViewport v = null)
        {
            //Multiply matrices
            if (_boneList != null)
            {
                foreach (MDL0BoneNode b in _boneList)
                {
                    b.RecalcFrameState(v);
                }
            }

            foreach (Influence inf in _influences._influences)
            {
                inf.CalcMatrix();
            }

            //Weight vertices and normals
            if (!_dontUpdateMesh && _objList != null)
            {
                foreach (MDL0ObjectNode poly in _objList)
                {
                    poly.Weight();
                }
            }
        }

        public void ApplySRT(SRT0Node node, float index)
        {
            //Transform textures
            if (_matList != null)
            {
                foreach (MDL0MaterialNode m in _matList)
                {
                    m.ApplySRT0(node, index);
                }
            }
        }

        public void ApplyCLR(CLR0Node node, float index)
        {
            //Apply color changes
            if (_matList != null)
            {
                foreach (MDL0MaterialNode m in _matList)
                {
                    m.ApplyCLR0(node, index);
                }
            }
        }

        public void ApplyPAT(PAT0Node node, float index)
        {
            //Change textures
            if (_matList != null)
            {
                foreach (MDL0MaterialNode m in _matList)
                {
                    m.ApplyPAT0(node, index);
                }
            }
        }

        public void ApplyVIS(VIS0Node node, float index)
        {
            if (node == null || index < 1)
            {
                //if (_objList != null)
                //    foreach (MDL0ObjectNode o in _objList)
                //        if (o._visBoneNode != null)
                //            o._render = o._visBoneNode._boneFlags.HasFlag(BoneFlags.Visible);
                return;
            }

            if (VIS0Indices == null)
            {
                RegenerateVIS0Indices();
            }

            foreach (string boneName in VIS0Indices.Keys)
            {
                VIS0EntryNode entry = null;
                Dictionary<int, List<int>> objects = VIS0Indices[boneName];
                foreach (KeyValuePair<int, List<int>> objDrawCalls in objects)
                {
                    MDL0ObjectNode obj = (MDL0ObjectNode) _objList[objDrawCalls.Key];
                    for (int x = 0; x < objDrawCalls.Value.Count; x++)
                    {
                        DrawCall c = obj._drawCalls[objDrawCalls.Value[x]];
                        if ((entry = (VIS0EntryNode) node.FindChild(c._visBoneNode.Name, true)) != null)
                        {
                            if (entry._entryCount != 0 && index > 0)
                            {
                                c._render = entry.GetEntry((int) index - 1);
                            }
                            else
                            {
                                c._render = entry._flags.HasFlag(VIS0Flags.Enabled);
                            }
                        }
                    }
                }
            }
        }

        public void ApplySCN(SCN0Node node, float index)
        {
            _scn0 = node;
            _scn0Frame = index;

            if (node != null)
            {
                _scn0Frame = _scn0Frame.Clamp(1, node.FrameCount);
            }

            if (_matList != null)
            {
                foreach (MDL0MaterialNode mat in _matList)
                {
                    mat.ApplySCN(node, _scn0Frame);
                }
            }
        }

        //This only modifies vertices after ApplyCHR0 has weighted them.
        //It cannot be used without calling ApplyCHR0 first.
        //TODO: Find a more efficient way to store this data
        public void ApplySHP(SHP0Node node, float index)
        {
            _currentSHP = node;
            _currentSHPIndex = index;

            //Max amount of morphs allowed is technically 32

            SHP0EntryNode entry;

            if (_objList != null)
            {
                foreach (MDL0ObjectNode poly in _objList)
                {
                    PrimitiveManager p = poly._manager;
                    if (p?._vertices == null || p._faceData == null)
                    {
                        continue;
                    }

                    //Reset this object's normal buffer to default
                    //Vertices are already weighted in WeightMeshes
                    //and colors aren't influenced by matrices,
                    //so they can be retrieved directly from the external array later on
                    for (int i = 0; i < p._vertices.Count; i++)
                    {
                        Vertex3 v = p._vertices[i];
                        if (v.FaceDataIndices != null)
                        {
                            for (int m = 0; m < v.FaceDataIndices.Count; m++)
                            {
                                int fIndex = v.FaceDataIndices[m];
                                if (fIndex < p._pointCount && fIndex >= 0)
                                {
                                    if (p._faceData[1] != null && poly._normalNode != null)
                                    {
                                        int normalIndex = v.Facepoints[m]._normalIndex;
                                        if (normalIndex >= 0 && normalIndex < poly._normalNode.Normals.Length)
                                        {
                                            ((Vector3*) p._faceData[1].Address)[fIndex] =
                                                poly._normalNode.Normals[normalIndex];
                                        }
                                    }

                                    if ((node == null || index == 0) && poly._colorSet != null)
                                    {
                                        for (int c = 0; c < 2; c++)
                                        {
                                            if (p._faceData[c + 2] != null && poly._colorSet[c] != null)
                                            {
                                                int colorIndex = v.Facepoints[m]._colorIndices[c];
                                                if (colorIndex >= 0 && colorIndex < poly._colorSet[c].Colors.Length)
                                                {
                                                    ((RGBAPixel*) p._faceData[c + 2].Address)[fIndex] =
                                                        poly._colorSet[c].Colors[colorIndex];
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (node == null || index == 0)
                    {
                        continue;
                    }

                    if ((entry = node.FindChild(poly.VertexNode, true) as SHP0EntryNode) != null &&
                        entry.Enabled && entry.UpdateVertices)
                    {
                        float[] weights = new float[entry.Children.Count];
                        foreach (SHP0VertexSetNode shpSet in entry.Children)
                        {
                            weights[shpSet.Index] = shpSet.Keyframes.GetFrameValue(index - 1);
                        }

                        float totalWeight = 0;
                        foreach (float f in weights)
                        {
                            totalWeight += f;
                        }

                        float baseWeight = 1.0f - totalWeight;
                        float total = totalWeight + baseWeight;

                        MDL0VertexNode[] nodes = new MDL0VertexNode[entry.Children.Count];
                        foreach (SHP0VertexSetNode shpSet in entry.Children)
                        {
                            nodes[shpSet.Index] = _vertList.Find(x => x.Name == shpSet.Name) as MDL0VertexNode;
                        }

                        //Calculate barycenter per vertex and set as weighted pos
                        if (p._vertices != null)
                        {
                            for (int i = 0; i < p._vertices.Count; i++)
                            {
                                int x = 0;
                                Vertex3 v3 = p._vertices[i];
                                v3._weightedPosition *= baseWeight;

                                foreach (MDL0VertexNode vNode in nodes)
                                {
                                    if (vNode != null && v3.Facepoints[0]._vertexIndex < vNode.Vertices.Length)
                                    {
                                        v3._weightedPosition +=
                                            v3.GetMatrix() * vNode.Vertices[v3.Facepoints[0]._vertexIndex] *
                                            weights[x];
                                    }

                                    x++;
                                }

                                v3._weightedPosition /= total;

                                v3._weights = weights;
                                v3._nodes = nodes;
                                v3._baseWeight = baseWeight;
                                v3._bCenter = v3._weightedPosition;
                            }
                        }
                    }

                    if ((entry = node.FindChild(poly.NormalNode, true) as SHP0EntryNode) != null &&
                        entry.Enabled && entry.UpdateNormals)
                    {
                        float[] weights = new float[entry.Children.Count];
                        foreach (SHP0VertexSetNode shpSet in entry.Children)
                        {
                            weights[shpSet.Index] = shpSet.Keyframes.GetFrameValue(index - 1);
                        }

                        float totalWeight = 0;
                        foreach (float f in weights)
                        {
                            totalWeight += f;
                        }

                        float baseWeight = 1.0f - totalWeight;
                        float total = totalWeight + baseWeight;

                        MDL0NormalNode[] nodes = new MDL0NormalNode[entry.Children.Count];
                        foreach (SHP0VertexSetNode shpSet in entry.Children)
                        {
                            nodes[shpSet.Index] = _normList.Find(x => x.Name == shpSet.Name) as MDL0NormalNode;
                        }

                        UnsafeBuffer buf = p._faceData[1];
                        if (buf != null)
                        {
                            Vector3* pData = (Vector3*) buf.Address;

                            if (p._vertices != null)
                            {
                                for (int i = 0; i < p._vertices.Count; i++)
                                {
                                    Vertex3 v3 = p._vertices[i];
                                    int m = 0;
                                    foreach (Facepoint r in v3.Facepoints)
                                    {
                                        int nIndex = v3.FaceDataIndices[m++];

                                        Vector3 weightedNormal =
                                            v3.GetMatrix().GetRotationMatrix() * pData[nIndex] * baseWeight;

                                        int x = 0;
                                        foreach (MDL0NormalNode n in nodes)
                                        {
                                            if (n != null && r._normalIndex < n.Normals.Length)
                                            {
                                                weightedNormal +=
                                                    v3.GetMatrix().GetRotationMatrix() *
                                                    n.Normals[r._normalIndex] *
                                                    weights[x];
                                            }

                                            x++;
                                        }

                                        pData[nIndex] = v3.GetInvMatrix().GetRotationMatrix() *
                                                        (weightedNormal / total).Normalize();
                                    }
                                }
                            }
                        }

                        p._dirty[1] = true;
                    }

                    for (int x = 0; x < 2; x++)
                    {
                        if (poly._colorSet[x] != null &&
                            (entry = node.FindChild(poly._colorSet[x].Name, true) as SHP0EntryNode) != null &&
                            entry.Enabled && entry.UpdateColors)
                        {
                            float[] weights = new float[entry.Children.Count];
                            foreach (SHP0VertexSetNode shpSet in entry.Children)
                            {
                                weights[shpSet.Index] = shpSet.Keyframes.GetFrameValue(index - 1);
                            }

                            float totalWeight = 0;
                            foreach (float f in weights)
                            {
                                totalWeight += f;
                            }

                            float baseWeight = 1.0f - totalWeight;
                            float total = totalWeight + baseWeight;

                            MDL0ColorNode[] nodes = new MDL0ColorNode[entry.Children.Count];
                            foreach (SHP0VertexSetNode shpSet in entry.Children)
                            {
                                nodes[shpSet.Index] = _colorList.Find(b => b.Name == shpSet.Name) as MDL0ColorNode;
                            }

                            UnsafeBuffer buf = p._faceData[x + 2];
                            if (buf != null)
                            {
                                RGBAPixel* pData = (RGBAPixel*) buf.Address;

                                if (p._vertices != null)
                                {
                                    for (int i = 0; i < p._vertices.Count; i++)
                                    {
                                        Vertex3 v3 = p._vertices[i];
                                        int m = 0;
                                        foreach (Facepoint r in v3.Facepoints)
                                        {
                                            int cIndex = v3.FaceDataIndices[m++];
                                            if (cIndex < p._pointCount)
                                            {
                                                Vector4 color =
                                                    (Vector4) poly._colorSet[x].Colors[r._colorIndices[x]] * baseWeight;

                                                int w = 0;
                                                foreach (MDL0ColorNode n in nodes)
                                                {
                                                    if (n != null && r._colorIndices[x] < n.Colors.Length)
                                                    {
                                                        color += (Vector4) n.Colors[r._colorIndices[x]] * weights[w];
                                                    }

                                                    w++;
                                                }

                                                pData[cIndex] = color / total;
                                            }
                                        }
                                    }
                                }
                            }

                            p._dirty[x + 2] = true;
                        }
                    }
                }
            }
        }

        #endregion

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((MDL0Header*) source.Address)->_header._tag == MDL0Header.Tag ? new MDL0Node() : null;
        }

        public void OnDrawCallsChanged()
        {
            DrawCallsChanged?.Invoke(this, null);
        }

        public event EventHandler DrawCallsChanged;

        [Browsable(false)]
        public List<DrawCallBase> DrawCalls
        {
            get
            {
                Populate();
                return _objList == null
                    ? new List<DrawCallBase>()
                    : _objList.SelectMany(x => ((MDL0ObjectNode) x).DrawCalls).ToList();
            }
        }
    }
}