using BrawlLib.Internal;
using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Modeling;
using BrawlLib.Modeling.Collada;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Graphics;
using BrawlLib.Wii.Models;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe partial class MDL0ObjectNode : MDL0EntryNode, IMatrixNodeUser, IObject
    {
        internal MDL0Object* Header => (MDL0Object*) WorkingUncompressed.Address;

        public override ResourceType ResourceFileType => ResourceType.MDL0Object;

        #region Attributes

        public List<Vertex3> Vertices => _manager != null ? _manager._vertices : null;

        internal bool Weighted => _nodeId == -1 || _matrixNode == null;

        internal bool HasTexMtx
        {
            get
            {
                if (_manager != null)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (_manager.HasTextureMatrix[i])
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        internal CPVertexFormat _vertexFormat;
        internal XFVertexSpecs _vertexSpecs;

        [Category("Texture Matrices")]
        public bool TextureMatrix0Enabled
        {
            get => _manager != null && _manager.HasTextureMatrix[0];
            set => SetTexMtx(0, value);
        }

        [Category("Texture Matrices")]
        public bool TextureMatrix1Enabled
        {
            get => _manager != null && _manager.HasTextureMatrix[1];
            set => SetTexMtx(1, value);
        }

        [Category("Texture Matrices")]
        public bool TextureMatrix2Enabled
        {
            get => _manager != null && _manager.HasTextureMatrix[2];
            set => SetTexMtx(2, value);
        }

        [Category("Texture Matrices")]
        public bool TextureMatrix3Enabled
        {
            get => _manager != null && _manager.HasTextureMatrix[3];
            set => SetTexMtx(3, value);
        }

        [Category("Texture Matrices")]
        public bool TextureMatrix4Enabled
        {
            get => _manager != null && _manager.HasTextureMatrix[4];
            set => SetTexMtx(4, value);
        }

        [Category("Texture Matrices")]
        public bool TextureMatrix5Enabled
        {
            get => _manager != null && _manager.HasTextureMatrix[5];
            set => SetTexMtx(5, value);
        }

        [Category("Texture Matrices")]
        public bool TextureMatrix6Enabled
        {
            get => _manager != null && _manager.HasTextureMatrix[6];
            set => SetTexMtx(6, value);
        }

        [Category("Texture Matrices")]
        public bool TextureMatrix7Enabled
        {
            get => _manager != null && _manager.HasTextureMatrix[7];
            set => SetTexMtx(7, value);
        }

        [Category("Texture Matrices")]
        public bool TextureMatrix0Identity
        {
            get => _manager != null && _manager.UseIdentityTexMtx[0];
            set => SetTexMtxIdentity(0, value);
        }

        [Category("Texture Matrices")]
        public bool TextureMatrix1Identity
        {
            get => _manager != null && _manager.UseIdentityTexMtx[1];
            set => SetTexMtxIdentity(1, value);
        }

        [Category("Texture Matrices")]
        public bool TextureMatrix2Identity
        {
            get => _manager != null && _manager.UseIdentityTexMtx[2];
            set => SetTexMtxIdentity(2, value);
        }

        [Category("Texture Matrices")]
        public bool TextureMatrix3Identity
        {
            get => _manager != null && _manager.UseIdentityTexMtx[3];
            set => SetTexMtxIdentity(3, value);
        }

        [Category("Texture Matrices")]
        public bool TextureMatrix4Identity
        {
            get => _manager != null && _manager.UseIdentityTexMtx[4];
            set => SetTexMtxIdentity(4, value);
        }

        [Category("Texture Matrices")]
        public bool TextureMatrix5Identity
        {
            get => _manager != null && _manager.UseIdentityTexMtx[5];
            set => SetTexMtxIdentity(5, value);
        }

        [Category("Texture Matrices")]
        public bool TextureMatrix6Identity
        {
            get => _manager != null && _manager.UseIdentityTexMtx[6];
            set => SetTexMtxIdentity(6, value);
        }

        [Category("Texture Matrices")]
        public bool TextureMatrix7Identity
        {
            get => _manager != null && _manager.UseIdentityTexMtx[7];
            set => SetTexMtxIdentity(7, value);
        }

        private void SetTexMtx(int index, bool value)
        {
            if (!Weighted || _manager == null)
            {
                return;
            }

            _manager.HasTextureMatrix[index] = value;
            SignalPropertyChange();
            _forceRebuild = true;
        }

        private void SetTexMtxIdentity(int index, bool value)
        {
            if (!Weighted || _manager == null)
            {
                return;
            }

            _manager.UseIdentityTexMtx[index] = value;
            SignalPropertyChange();
            _forceRebuild = true;
        }

        [Category("Object Data")]
        public ObjFlag Flags
        {
            get => (ObjFlag) _flag;
            set
            {
                _flag = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("Object Data")] public int ID => _entryIndex;
        [Category("Object Data")] public int FacepointCount => _numFacepoints;

        [Category("Object Data")] public int VertexCount => _manager?._vertices == null ? 0 : _manager._vertices.Count;

        [Category("Object Data")] public int FaceCount => _numFaces;

        [Browsable(false)] public List<IMatrixNode> Influences => _influences;

        #endregion

        #region Linked Sets

        #region Vertices & Normals

        public MDL0VertexNode _vertexNode;
        public MDL0NormalNode _normalNode;

        [Category("Mesh Assets")]
        [TypeConverter(typeof(DropDownListVertices))]
        public string VertexNode
        {
            get => _vertexNode == null ? null : _vertexNode._name;
            set
            {
                //Can't remove the vertex node!
                //Do nothing if attempting to set to null
                if (!string.IsNullOrEmpty(value))
                {
                    MDL0VertexNode newNode =
                        Model.FindChild($"Vertices/{value}", false) as MDL0VertexNode;
                    if (newNode != null)
                    {
                        if (_vertexNode != null)
                        {
                            if (newNode.NumVertices < _vertexNode.NumVertices)
                            {
                                MessageBox.Show(
                                    $"Not enough vertices.\nNeeds: {_vertexNode.NumVertices}\nSelection: {newNode.NumVertices}");
                                return;
                            }

                            if (_vertexNode._objects.Contains(this))
                            {
                                _vertexNode._objects.Remove(this);
                            }
                        }

                        (_vertexNode = newNode)._objects.Add(this);
                        _elementIndices[0] = (short) newNode.Index;
                    }
                    else
                    {
                        MessageBox.Show("A vertex node with this name was not found.");
                        return;
                    }
                }

                SignalPropertyChange();
            }
        }

        [Category("Mesh Assets")]
        [TypeConverter(typeof(DropDownListNormals))]
        public string NormalNode
        {
            get => _normalNode == null ? null : _normalNode._name;
            set
            {
                MDL0NormalNode oldNode = _normalNode;
                if (string.IsNullOrEmpty(value))
                {
                    if (oldNode != null && MessageBox.Show(RootNode._mainForm,
                        "Are you sure you want to remove this reference?", "Continue?",
                        MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        if (oldNode._objects.Contains(this))
                        {
                            oldNode._objects.Remove(this);
                        }

                        _normalNode = null;
                        _elementIndices[1] = -1;
                        _forceRebuild = true;
                    }
                    else
                    {
                        return; //No point setting a null node to null
                    }
                }
                else
                {
                    MDL0NormalNode newNode =
                        Model.FindChild($"Normals/{value}", false) as MDL0NormalNode;
                    if (newNode != null)
                    {
                        if (_normalNode != null)
                        {
                            if (newNode.NumEntries < _normalNode.NumEntries)
                            {
                                MessageBox.Show(
                                    $"Not enough normals.\nNeeds: {_normalNode.NumEntries}\nSelection: {newNode.NumEntries}");
                                return;
                            }

                            if (_normalNode._objects.Contains(this))
                            {
                                _normalNode._objects.Remove(this);
                            }
                        }

                        (_normalNode = newNode)._objects.Add(this);
                        _elementIndices[1] = (short) newNode.Index;
                    }
                    else
                    {
                        MessageBox.Show("A normal node with this name was not found.");
                        return;
                    }
                }

                SignalPropertyChange();
            }
        }

        #endregion

        #region Colors

        public MDL0ColorNode[] _colorSet = new MDL0ColorNode[2];

        public void SetColors(int id, string value)
        {
            SetColors(id, value, false);
        }

        public void SetColors(int id, string value, bool skipDialog)
        {
            MDL0ColorNode oldNode = _colorSet[id];
            if (string.IsNullOrEmpty(value))
            {
                if (oldNode != null && (skipDialog || MessageBox.Show(RootNode._mainForm,
                        "Are you sure you want to remove this reference?", "Continue?",
                        MessageBoxButtons.OKCancel) ==
                    DialogResult.OK))
                {
                    if (oldNode._objects.Contains(this))
                    {
                        oldNode._objects.Remove(this);
                    }

                    _colorSet[id] = null;
                    _elementIndices[id + 2] = -1;
                    _forceRebuild = true;
                }
                else
                {
                    return; //No point setting a null node to null
                }
            }
            else
            {
                MDL0ColorNode newNode = Model.FindChild($"Colors/{value}", false) as MDL0ColorNode;
                if (newNode != null)
                {
                    if (oldNode != null)
                    {
                        if (newNode.NumEntries < oldNode.NumEntries)
                        {
                            if (!skipDialog && MessageBox.Show(null,
                                "This node has less colors than in the originally linked color node.\nAny colors that cannot be found will use the first color instead.\nIs this okay?",
                                "", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                return;
                            }

                            _forceRebuild = true;
                        }

                        if (oldNode._objects.Contains(this))
                        {
                            oldNode._objects.Remove(this);
                        }
                    }

                    (_colorSet[id] = newNode)._objects.Add(this);
                    _elementIndices[id + 2] = (short) newNode.Index;
                }
                else
                {
                    MessageBox.Show("A color node with this name was not found.");
                    return;
                }
            }

            SignalPropertyChange();
        }

        [Category("Mesh Assets")]
        [TypeConverter(typeof(DropDownListColors))]
        public string ColorNode0
        {
            get => _colorSet[0] == null ? null : _colorSet[0]._name;
            set => SetColors(0, value);
        }

        [Category("Mesh Assets")]
        [TypeConverter(typeof(DropDownListColors))]
        public string ColorNode1
        {
            get => _colorSet[1] == null ? null : _colorSet[1]._name;
            set => SetColors(1, value);
        }

        #endregion

        #region UVs

        public MDL0UVNode[] _uvSet = new MDL0UVNode[8];

        public void SetUVs(int id, string value)
        {
            SetUVs(id, value, false);
        }

        public void SetUVs(int id, string value, bool skipDialog)
        {
            MDL0UVNode oldNode = _uvSet[id];
            if (string.IsNullOrEmpty(value))
            {
                if (oldNode != null && (skipDialog || MessageBox.Show(RootNode._mainForm,
                        "Are you sure you want to remove this reference?", "Continue?",
                        MessageBoxButtons.OKCancel) ==
                    DialogResult.OK))
                {
                    if (oldNode._objects.Contains(this))
                    {
                        oldNode._objects.Remove(this);
                    }

                    _uvSet[id] = null;
                    _elementIndices[id + 4] = -1;
                    _forceRebuild = true;
                }
                else
                {
                    return; //No point setting a null node to null
                }
            }
            else
            {
                MDL0UVNode newNode = Model.FindChild($"UVs/{value}", false) as MDL0UVNode;
                if (newNode != null)
                {
                    if (oldNode != null)
                    {
                        if (newNode.NumEntries < oldNode.NumEntries)
                        {
                            MessageBox.Show(
                                $"Not enough UVs.\nNeeds: {oldNode.NumEntries}\nSelection: {newNode.NumEntries}");
                            return;
                        }

                        if (oldNode._objects.Contains(this))
                        {
                            oldNode._objects.Remove(this);
                        }
                    }

                    (_uvSet[id] = newNode)._objects.Add(this);
                    _elementIndices[id + 4] = (short) newNode.Index;
                }
                else
                {
                    return;
                }
            }

            SignalPropertyChange();
        }

        [Category("Mesh Assets")]
        [TypeConverter(typeof(DropDownListUVs))]
        public string TexCoord0
        {
            get => _uvSet[0] == null ? null : _uvSet[0]._name;
            set => SetUVs(0, value);
        }

        [Category("Mesh Assets")]
        [TypeConverter(typeof(DropDownListUVs))]
        public string TexCoord1
        {
            get => _uvSet[1] == null ? null : _uvSet[1]._name;
            set => SetUVs(1, value);
        }

        [Category("Mesh Assets")]
        [TypeConverter(typeof(DropDownListUVs))]
        public string TexCoord2
        {
            get => _uvSet[2] == null ? null : _uvSet[2]._name;
            set => SetUVs(2, value);
        }

        [Category("Mesh Assets")]
        [TypeConverter(typeof(DropDownListUVs))]
        public string TexCoord3
        {
            get => _uvSet[3] == null ? null : _uvSet[3]._name;
            set => SetUVs(3, value);
        }

        [Category("Mesh Assets")]
        [TypeConverter(typeof(DropDownListUVs))]
        public string TexCoord4
        {
            get => _uvSet[4] == null ? null : _uvSet[4]._name;
            set => SetUVs(4, value);
        }

        [Category("Mesh Assets")]
        [TypeConverter(typeof(DropDownListUVs))]
        public string TexCoord5
        {
            get => _uvSet[5] == null ? null : _uvSet[5]._name;
            set => SetUVs(5, value);
        }

        [Category("Mesh Assets")]
        [TypeConverter(typeof(DropDownListUVs))]
        public string TexCoord6
        {
            get => _uvSet[6] == null ? null : _uvSet[6]._name;
            set => SetUVs(6, value);
        }

        [Category("Mesh Assets")]
        [TypeConverter(typeof(DropDownListUVs))]
        public string TexCoord7
        {
            get => _uvSet[7] == null ? null : _uvSet[7]._name;
            set => SetUVs(7, value);
        }

        #endregion

        #region Fur

        internal MDL0FurPosNode _furPosNode;
        internal MDL0FurVecNode _furVecNode;

        [Category("Mesh Assets")]
        [TypeConverter(typeof(DropDownListFurPos))]
        public string FurLayerCoordNode
        {
            get => _furPosNode == null ? null : _furPosNode._name;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    MDL0FurPosNode node =
                        Model.FindChild($"FurLayerCoords/{value}", false) as MDL0FurPosNode;
                    if (node != null && _furPosNode != null && node.NumVertices >= _furPosNode.NumVertices)
                    {
                        if (_furPosNode != null && _furPosNode._objects.Contains(this))
                        {
                            _furPosNode._objects.Remove(this);
                        }

                        (_furPosNode = node)._objects.Add(this);
                        _elementIndices[12] = (short) node.Index;
                    }
                }

                SignalPropertyChange();
            }
        }

        [Category("Mesh Assets")]
        [TypeConverter(typeof(DropDownListFurVec))]
        public string FurVectorNode
        {
            get => _furVecNode == null ? null : _furVecNode._name;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    MDL0FurVecNode node =
                        Model.FindChild($"FurVectors/{value}", false) as MDL0FurVecNode;
                    if (node != null && _furVecNode != null && node.NumEntries >= _furVecNode.NumEntries)
                    {
                        if (_furVecNode != null && _furVecNode._objects.Contains(this))
                        {
                            _furVecNode._objects.Remove(this);
                        }

                        (_furVecNode = node)._objects.Add(this);
                        _elementIndices[13] = (short) node.Index;
                    }
                }

                SignalPropertyChange();
            }
        }

        #endregion

        #endregion

        #region Variables

        public int _numFacepoints;
        public int _numFaces;
        public int _nodeId;
        public int _defBufferSize = 0xE0;
        public int _flag;

        public int[] _nodeCache;
        private int _tableLen;

        internal short[] _elementIndices = new short[14];
        public bool _forceRebuild, _reOptimized;

        internal List<IMatrixNode> _influences;
        public BindingList<DrawCall> _drawCalls = new BindingList<DrawCall>();

        #endregion

        #region Single Bind linkage

        [Category("Object Data")]
        [TypeConverter(typeof(DropDownListBones))]
        public string SingleBind
        {
            get => _matrixNode == null ? "(none)" :
                _matrixNode.IsPrimaryNode ? ((MDL0BoneNode) _matrixNode)._name : "(multiple)";
            set
            {
                MatrixNode = string.IsNullOrEmpty(value) ? null : Model.FindBone(value);
                Model.SignalPropertyChange();
            }
        }

        internal IMatrixNode _matrixNode;

        [Browsable(false)]
        public IMatrixNode MatrixNode
        {
            get => _matrixNode;
            set
            {
                if (_matrixNode == value)
                {
                    return;
                }

                if (_manager != null)
                {
                    foreach (Vertex3 v in _manager._vertices)
                    {
                        v.DeferUpdateAssets();
                        v.ChangeInfluence(value);

                        if (v._matrixNode != null && v._matrixNode.Users.Contains(v))
                        {
                            v._matrixNode.Users.Remove(v);
                        }
                    }

                    if (_updateAssets)
                    {
                        SetEditedAssets(false, true, true);
                    }
                }

                if (_matrixNode != null)
                {
                    if (_matrixNode is MDL0BoneNode && ((MDL0BoneNode) _matrixNode)._singleBindObjects.Contains(this))
                    {
                        ((MDL0BoneNode) _matrixNode)._singleBindObjects.Remove(this);
                    }
                    else if (_matrixNode.Users.Contains(this))
                    {
                        _matrixNode.Users.Remove(this);
                    }
                }

                if ((_matrixNode = value) != null)
                {
                    //Singlebind bones aren't added to NodeMix, but its node id is still built as influenced
                    if (_matrixNode is MDL0BoneNode && !((MDL0BoneNode) _matrixNode)._singleBindObjects.Contains(this))
                    {
                        ((MDL0BoneNode) _matrixNode)._singleBindObjects.Add(this);
                    }
                    else if (!_matrixNode.Users.Contains(this))
                    {
                        _matrixNode.Users.Add(this);
                    }
                }

                _updateAssets = true;
            }
        }

        #endregion

        #region Reading & Writing

        public PrimitiveManager _manager;

        public override void Dispose()
        {
            if (_manager != null && _parent != null && _parent._disposed)
            {
                _manager.Dispose();
                _manager = null;
            }

            base.Dispose();
        }

        public override bool OnInitialize()
        {
            MDL0Object* header = Header;
            _nodeId = header->_nodeId;

            SetSizeInternal(header->_totalLength);

            ModelLinker linker = Model._linker;

            MatrixNode = _nodeId >= 0 && _nodeId < Model._linker.NodeCache.Length
                ? Model._linker.NodeCache[_nodeId]
                : null;

            _vertexFormat = header->_vertexFormat;
            _vertexSpecs = header->_vertexSpecs;
            _numFacepoints = header->_numVertices;
            _numFaces = header->_numFaces;
            _flag = header->_flag;
            _defBufferSize = header->_defintions._bufferSize;
            _entryIndex = header->_index;

            //Conditional name assignment
            if (_name == null && header->_stringOffset != 0)
            {
                if (!_replaced)
                {
                    _name = header->ResourceString;
                }
                else
                {
                    _name = "polygon" + Index;
                }
            }

            //Link nodes
            if (header->_vertexId >= 0 && Model._vertList != null)
            {
                foreach (MDL0VertexNode v in Model._vertList)
                {
                    if (header->_vertexId == v.ID)
                    {
                        (_vertexNode = v)._objects.Add(this);
                        break;
                    }
                }
            }

            if (header->_normalId >= 0 && Model._normList != null)
            {
                foreach (MDL0NormalNode n in Model._normList)
                {
                    if (header->_normalId == n.ID)
                    {
                        (_normalNode = n)._objects.Add(this);
                        break;
                    }
                }
            }

            int id;
            for (int i = 0; i < 2; i++)
            {
                if ((id = ((bshort*) header->_colorIds)[i]) >= 0 && Model._colorList != null)
                {
                    foreach (MDL0ColorNode c in Model._colorList)
                    {
                        if (id == c.ID)
                        {
                            (_colorSet[i] = c)._objects.Add(this);
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < 8; i++)
            {
                if ((id = ((bshort*) header->_uids)[i]) >= 0 && Model._uvList != null)
                {
                    foreach (MDL0UVNode u in Model._uvList)
                    {
                        if (id == u.ID)
                        {
                            (_uvSet[i] = u)._objects.Add(this);
                            break;
                        }
                    }
                }
            }

            if (Model._version > 9)
            {
                if (header->_furVectorId >= 0)
                {
                    foreach (MDL0FurVecNode v in Model._furVecList)
                    {
                        if (header->_furVectorId == v.ID)
                        {
                            (_furVecNode = v)._objects.Add(this);
                            break;
                        }
                    }
                }

                if (header->_furLayerCoordId >= 0)
                {
                    foreach (MDL0FurPosNode n in Model._furPosList)
                    {
                        if (header->_furLayerCoordId == n.ID)
                        {
                            (_furPosNode = n)._objects.Add(this);
                            break;
                        }
                    }
                }
            }

            //Link element indices for rebuild
            _elementIndices[0] = (short) (_vertexNode != null ? _vertexNode.Index : -1);
            _elementIndices[1] = (short) (_normalNode != null ? _normalNode.Index : -1);
            for (int i = 2; i < 4; i++)
            {
                _elementIndices[i] = (short) (_colorSet[i - 2] != null ? _colorSet[i - 2].Index : -1);
            }

            for (int i = 4; i < 12; i++)
            {
                _elementIndices[i] = (short) (_uvSet[i - 4] != null ? _uvSet[i - 4].Index : -1);
            }

            _elementIndices[12] = (short) (_furVecNode != null ? _furVecNode.Index : -1);
            _elementIndices[13] = (short) (_furPosNode != null ? _furPosNode.Index : -1);

            //Create primitive manager
            _manager = new PrimitiveManager(header, Model._assets, linker.NodeCache, this);

            //Read internal object node cache and read influence list
            if (Model._linker.NodeCache != null && _matrixNode == null)
            {
                _influences = new List<IMatrixNode>();
                bushort* weights = header->WeightIndices(Model._version);
                int count = *(bint*) weights;
                weights += 2;
                for (int i = 0; i < count; i++, weights++)
                {
                    if (*weights < Model._linker.NodeCache.Length)
                    {
                        _influences.Add(Model._linker.NodeCache[*weights]);
                    }
                }
            }

            //Check for errors
            if (header->_totalLength % 0x20 != 0)
            {
                Model._errors.Add("Object " + Index + " has an improper data length.");
                SignalPropertyChange();
                _forceRebuild = true;
            }

            if ((0x24 + header->_primitives._offset) % 0x20 != 0)
            {
                Model._errors.Add("Object " + Index + " has an improper primitives start offset.");
                SignalPropertyChange();
                _forceRebuild = true;
            }

            if (CheckVertexFormat())
            {
                Model._errors.Add("Object " + Index +
                                  " has a facepoint descriptor that does not match its linked nodes.");
                SignalPropertyChange();
                _forceRebuild = true;

                for (int i = 0; i < 2; i++)
                {
                    if (_colorSet[i] != null && _manager._faceData[i + 2] == null)
                    {
                        _manager._faceData[i + 2] = new UnsafeBuffer(_manager._pointCount * 4);
                    }
                }
            }

            if (HasTexMtx && !Weighted)
            {
                Model._errors.Add("Object " + Index + " has texture matrices but is not weighted.");
                for (int i = 0; i < 8; i++)
                {
                    _manager.HasTextureMatrix[i] = false;
                }

                SignalPropertyChange();
                _forceRebuild = true;
            }

            //if (!Weighted)
            //{
            //    bool notFloat = HasANonFloatAsset;
            //    foreach (PrimitiveGroup p in _primGroups)
            //    {
            //        bool o = false;
            //        foreach (PrimitiveHeader ph in p._headers)
            //            if (ph.Type != WiiBeginMode.TriangleList && notFloat)
            //            {
            //                Model._errors.Add("Object " + Index + " will explode in-game due to assets that are not written as float.");
            //                SignalPropertyChange();

            //                if (_vertexNode.Format != WiiVertexComponentType.Float)
            //                    _vertexNode._forceRebuild = _vertexNode._forceFloat = true;

            //                if (_normalNode != null && _normalNode.Format != WiiVertexComponentType.Float)
            //                    _normalNode._forceRebuild = _normalNode._forceFloat = true;

            //                for (int i = 4; i < 12; i++)
            //                    if (_uvSet[i - 4] != null && _uvSet[i - 4].Format != WiiVertexComponentType.Float)
            //                        _uvSet[i - 4]._forceRebuild = _uvSet[i - 4]._forceFloat = true;

            //                o = true;
            //                break;
            //            }
            //        if (o)
            //            break;
            //    }
            //}

            return false;
        }

        #region Rebuilding

        public void RecalcIndices()
        {
            _elementIndices[0] = (short) (_vertexNode != null ? _vertexNode.Index : _elementIndices[0]);
            _elementIndices[1] = (short) (_normalNode != null ? _normalNode.Index : _elementIndices[1]);
            for (int i = 2; i < 4; i++)
            {
                _elementIndices[i] = (short) (_colorSet[i - 2] != null ? _colorSet[i - 2].Index : _elementIndices[i]);
            }

            for (int i = 4; i < 12; i++)
            {
                _elementIndices[i] = (short) (_uvSet[i - 4] != null ? _uvSet[i - 4].Index : _elementIndices[i]);
            }

            _elementIndices[12] = (short) (_furVecNode != null ? _furVecNode.Index : _elementIndices[0]);
            _elementIndices[13] = (short) (_furPosNode != null ? _furPosNode.Index : _elementIndices[1]);
        }

        /// <summary>
        /// Returns true if the facepoint descriptor does not match the linked nodes, meaning this object must be rebuilt.
        /// </summary>
        public bool CheckVertexFormat()
        {
            bool b1 = _vertexFormat.PosFormat != XFDataFormat.None;
            bool b2 = _elementIndices[0] != -1;
            if (b1 != b2)
            {
                return true;
            }

            b1 = _vertexFormat.NormalFormat != XFDataFormat.None;
            b2 = _elementIndices[1] != -1;
            if (b1 != b2)
            {
                return true;
            }

            for (int i = 0; i < 2; i++)
            {
                b1 = _vertexFormat.GetColorFormat(i) != XFDataFormat.None;
                b2 = _elementIndices[i + 2] != -1;
                if (b1 != b2)
                {
                    return true;
                }
            }

            for (int i = 0; i < 8; i++)
            {
                b1 = _vertexFormat.GetUVFormat(i) != XFDataFormat.None;
                b2 = _elementIndices[i + 4] != -1;
                if (b1 != b2)
                {
                    return true;
                }
            }

            return false;
        }

        public void GenerateNodeCache()
        {
            if (MatrixNode != null)
            {
                _nodeCache = new int[0];
                return;
            }

            //Create node table
            HashSet<int> nodes = new HashSet<int>();
            foreach (Vertex3 v in _manager._vertices)
            {
                if (v.MatrixNode != null)
                {
                    nodes.Add(v.MatrixNode.NodeIndex);
                }
            }

            //Copy to array and sort
            _nodeCache = new int[nodes.Count];
            nodes.CopyTo(_nodeCache);
            Array.Sort(_nodeCache);
        }

        //This should be done after node indices have been assigned
        public override int OnCalculateSize(bool force)
        {
            MDL0Node model = Model;
            if (model._isImport)
            {
                //Continue checking for single bind
                if (_nodeId == -2 && MatrixNode == null)
                {
                    bool first = true;
                    foreach (Vertex3 v in _manager._vertices)
                    {
                        if (first)
                        {
                            if (v.MatrixNode != null)
                            {
                                DeferUpdateAssets();
                                MatrixNode = model._linker.NodeCache[v.MatrixNode.NodeIndex];
                            }

                            first = false;
                        }

                        v.MatrixNode = null;
                    }

                    //Check if a single bind was even found.
                    if (MatrixNode == null)
                    {
                        if (model._boneList != null && model._boneList.Count > 0)
                        {
                            MatrixNode = model._boneList[0] as IMatrixNode;
                        }
                        else if (model._linker.BoneCache != null && model._linker.BoneCache.Length > 0)
                        {
                            MatrixNode = model._linker.BoneCache[0];
                        }
                        else if (model._linker.NodeCache != null && model._linker.NodeCache.Length > 0)
                        {
                            MatrixNode = model._linker.NodeCache[0];
                        }

                        //At this point, if a single bind still hasn't been found
                        //it doesn't even matter because the model doesn't have any matrices at all
                    }
                }
            }

            //Collect vertex node ids
            GenerateNodeCache();

            //Recollect indices of linked nodes
            RecalcIndices();

            //Check that the facepoint descriptor matches the linked nodes
            if (!_forceRebuild)
            {
                _forceRebuild = CheckVertexFormat();
            }

            //Primitives need to be 
            _manager._remakePrimitives = model._isImport || _reOptimized || _manager.AssetsChanged;

            //Rebuild only under certain circumstances
            if (_manager._remakePrimitives || _forceRebuild)
            {
                _manager._isWeighted = Weighted;

                int size = (int) MDL0Object.Size;
                if (model._version >= 10)
                {
                    size += 4; //Add extra -1 value
                }

                //Set vertex descriptor
                _manager.SetVertexDescList(
                    _elementIndices,
                    model._linker._vertices,
                    model._linker._colors,
                    model._linker._forceDirectAssets);

                //Add table length
                size += _nodeCache.Length * 2 + 4;
                _tableLen = (size.Align(0x10) + 0xE0) % 0x20 == 0 ? size.Align(0x10) : size.Align(0x20);

                //Add def length
                size = _tableLen + 0xE0;

                //If assets have been changed, linked asset nodes must be recreated!
                //This will merge internally stored face data buffers into facepoints,
                //then group them and store the groups in _primGroups
                if (model._isImport || _manager.AssetsChanged)
                {
                    _manager.GroupPrimitives(
                        Collada._importOptions._useTristrips,
                        Collada._importOptions._cacheSize,
                        Collada._importOptions._minStripLen,
                        Collada._importOptions._pushCacheHits,
                        !model._isImport ? false : Collada._importOptions._forceCCW,
                        out _numFacepoints,
                        out _numFaces);
                }

                size += _manager.GetDisplayListSize();
                int align = size.Align(0x10) % 0x20 == 0 ? 0x10 : 0x20;
                int diff = size.Align(align) - size;
                size += diff;
                _manager._primitiveSize += diff;

                return size;
            }

            return base.OnCalculateSize(force);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            MDL0Object* header = (MDL0Object*) address;

            MDL0Node model = Model;

            header->_defintions._size = 0x80 +
                                        (_uvSet[2] == null
                                            ? 0
                                            : 0x20 +
                                              (_uvSet[5] == null
                                                  ? 0
                                                  : 0x20 +
                                                    (_uvSet[7] == null ? 0 : 0x20)));

            if (_manager._remakePrimitives || _forceRebuild)
            {
                //Set header values
                header->_totalLength = length;
                header->_numVertices = _numFacepoints;
                header->_numFaces = _numFaces;
                header->_primitives._bufferSize = _manager._primitiveSize;
                header->_primitives._size = _manager._primitiveSize;
                header->_primitives._offset = _tableLen + 0xBC;
                header->_defintions._bufferSize = _defBufferSize;
                header->_defintions._offset = _tableLen - 0x18;
                header->_flag = _flag;
                header->_index = _entryIndex;

                //Set node table offset
                if (model._version < 10)
                {
                    header->_nodeTableOffset = 0x64;
                }
                else
                {
                    header->_furVectorId = _elementIndices[12];
                    header->_furLayerCoordId = _elementIndices[13];
                    *((byte*) header + 0x67) = 0x68;
                }

                //Set the node id
                header->_nodeId = _nodeId = _matrixNode != null ? _matrixNode.NodeIndex : -1;

                //Set asset ids
                header->_vertexId = model._isImport && model._linker._forceDirectAssets[0]
                    ? (short) -1
                    : (short) (_elementIndices[0] >= 0 ? _elementIndices[0] : -1);
                header->_normalId = model._isImport && model._linker._forceDirectAssets[1]
                    ? (short) -1
                    : (short) (_elementIndices[1] >= 0 ? _elementIndices[1] : -1);
                for (int i = 2; i < 4; i++)
                {
                    *(bshort*) &header->_colorIds[i - 2] = model._isImport && model._linker._forceDirectAssets[i]
                        ? (short) -1
                        : (short) (_elementIndices[i] >= 0 ? _elementIndices[i] : -1);
                }

                for (int i = 4; i < 12; i++)
                {
                    *(bshort*) &header->_uids[i - 4] = model._isImport && model._linker._forceDirectAssets[i]
                        ? (short) -1
                        : (short) (_elementIndices[i] >= 0 ? _elementIndices[i] : -1);
                }

                //Write def list
                MDL0PolygonDefs* Defs = header->DefList;
                *Defs = MDL0PolygonDefs.Default;

                //Array flags are already set
                header->_arrayFlags = _manager._arrayFlags;

                //Set vertex flags using descriptor list (sets the flags to this object)
                _manager.WriteVertexDescriptor(out _vertexFormat, out _vertexSpecs);

                //Set UVAT groups using format list (writes directly to header)
                _manager.WriteVertexFormat(header);

                //Write newly set flags
                header->_vertexFormat._lo = Defs->VtxFmtLo = _vertexFormat._lo;
                header->_vertexFormat._hi = Defs->VtxFmtHi = _vertexFormat._hi;
                header->_vertexSpecs = Defs->VtxSpecs = _vertexSpecs;

                //Write weight table only if the object is weighted
                if (_matrixNode == null)
                {
                    WriteWeightTable(header->WeightIndices(model._version));
                }

                //Write primitives
                _manager.WritePrimitives(header, model._linker.NodeCache);

                _manager.AssetsChanged = false;
            }
            else
            {
                //Move raw data over
                base.OnRebuild(address, length, force);

                CorrectNodeIds(header);

                header->_vertexId = _elementIndices[0];
                header->_normalId = _elementIndices[1];
                for (int i = 2; i < 4; i++)
                {
                    *(bshort*) &header->_colorIds[i - 2] = (short) (_elementIndices[i] >= 0 ? _elementIndices[i] : -1);
                }

                for (int i = 4; i < 12; i++)
                {
                    *(bshort*) &header->_uids[i - 4] = (short) (_elementIndices[i] >= 0 ? _elementIndices[i] : -1);
                }

                if (model._version >= 10)
                {
                    *(bshort*) ((byte*) header + 0x60) = _elementIndices[12];
                    *(bshort*) ((byte*) header + 0x62) = _elementIndices[13];
                }
            }

            _forceRebuild = _reOptimized = false;
        }

        private void WriteWeightTable(VoidPtr addr)
        {
            if (_nodeCache == null)
            {
                GenerateNodeCache();
            }

            bushort* ptr = (bushort*) addr;
            *(buint*) ptr = (uint) _nodeCache.Length;
            ptr += 2;
            foreach (int n in _nodeCache)
            {
                *ptr++ = (ushort) n;
            }
        }

        private void CorrectNodeIds(MDL0Object* header)
        {
            WriteWeightTable(header->WeightIndices(Model._version));

            if (_matrixNode != null)
            {
                header->_nodeId = _nodeId = (ushort) _matrixNode.NodeIndex;
            }
            else
            {
                header->_nodeId = _nodeId = -1;

                VoidPtr primAddr = header->PrimitiveData;
                foreach (PrimitiveGroup p in _manager._primGroups)
                {
                    p.SetNodeIds(primAddr);
                }
            }
        }

        public override void Export(string outPath)
        {
            if (outPath.EndsWith(".obj"))
            {
                Wavefront.Serialize(outPath, this);
            }
            else
            {
                base.Export(outPath);
            }
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0Object* header = (MDL0Object*) dataAddress;
            header->_mdl0Offset = (int) mdlAddress - (int) dataAddress;
            header->_stringOffset = (int) stringTable[Name] + 4 - (int) dataAddress;
            header->_index = Index;
        }

        #endregion

        #endregion

        #region Rendering

        [Browsable(false)]
        public bool IsRendering
        {
            get
            {
                foreach (DrawCall c in _drawCalls)
                {
                    if (c._render)
                    {
                        return true;
                    }
                }

                return false;
            }
            set
            {
                foreach (DrawCall c in _drawCalls)
                {
                    c._render = value;
                }
            }
        }

        [Browsable(false)]
        public bool Attached
        {
            get => _attached;
            set => _attached = value;
        }

        public bool _attached;

        public Box GetBox()
        {
            if (_manager?._vertices == null)
            {
                return new Box();
            }

            Box box = Box.ExpandableVolume;
            foreach (Vertex3 vertex in _manager._vertices)
            {
                box.ExpandVolume(vertex.WeightedPosition);
            }

            return box;
        }

        private BlendingFactorSrc[] _blendSrc =
        {
            BlendingFactorSrc.Zero, BlendingFactorSrc.One,
            BlendingFactorSrc.DstColor, BlendingFactorSrc.OneMinusDstColor,
            BlendingFactorSrc.SrcAlpha, BlendingFactorSrc.OneMinusSrcAlpha,
            BlendingFactorSrc.DstAlpha, BlendingFactorSrc.OneMinusDstAlpha
        };

        private BlendingFactorDest[] _blendDst =
        {
            BlendingFactorDest.Zero, BlendingFactorDest.One,
            BlendingFactorDest.SrcColor, BlendingFactorDest.OneMinusSrcColor,
            BlendingFactorDest.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha,
            BlendingFactorDest.DstAlpha, BlendingFactorDest.OneMinusDstAlpha
        };

        private readonly LogicOp[] _logicOp =
        {
            LogicOp.Clear, LogicOp.And, LogicOp.AndReverse, LogicOp.Copy,
            LogicOp.AndInverted, LogicOp.Noop, LogicOp.Xor, LogicOp.Or,
            LogicOp.Nor, LogicOp.Equiv, LogicOp.Invert, LogicOp.OrReverse,
            LogicOp.CopyInverted, LogicOp.OrInverted, LogicOp.Nand, LogicOp.Set
        };

        private readonly CullFaceMode[] _cullMode =
        {
            CullFaceMode.Front,
            CullFaceMode.Back,
            CullFaceMode.FrontAndBack
        };

        private readonly DepthFunction[] _depthFunc =
        {
            DepthFunction.Never,
            DepthFunction.Less,
            DepthFunction.Equal,
            DepthFunction.Lequal,
            DepthFunction.Greater,
            DepthFunction.Notequal,
            DepthFunction.Gequal,
            DepthFunction.Always
        };

        private readonly AlphaFunction[] _alphaFunc =
        {
            AlphaFunction.Never,
            AlphaFunction.Less,
            AlphaFunction.Equal,
            AlphaFunction.Lequal,
            AlphaFunction.Greater,
            AlphaFunction.Notequal,
            AlphaFunction.Gequal,
            AlphaFunction.Always
        };

        internal void Render(bool wireframe, bool useShaders, MDL0MaterialNode material)
        {
            if (_manager == null)
            {
                return;
            }

            if (!TKContext._shadersSupported)
            {
                useShaders = false;
            }

            _manager.PrepareStream();

            if (wireframe)
            {
                GL.Enable(EnableCap.CullFace);
                GL.CullFace(CullFaceMode.Back);
                GL.Color4(Color.Black);

                _manager.BindStream();
                _manager.DisableTextures();
                _manager.RenderMesh();
                _manager.DetachStreams();

                return;
            }

            GL.MatrixMode(MatrixMode.Texture);

            bool anyRendered = false;

            if (material != null)
            {
                if (!useShaders)
                {
                    AlphaTest(material);
                }
                else
                {
                    material.UseProgram(this);
                    Blend(material);
                }

                //Blend(material);
                Cull(material);
                DepthTest(material);

                if (useShaders)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        GL.ClientActiveTexture(TextureUnit.Texture0 + i);
                        GL.ActiveTexture(TextureUnit.Texture0 + i);

                        if (i >= material.Children.Count)
                        {
                            GL.DisableClientState(ArrayCap.TextureCoordArray);
                            GL.Disable(EnableCap.Texture2D);
                            continue;
                        }

                        GL.Enable(EnableCap.Texture2D);

                        MDL0MaterialRefNode mr = (MDL0MaterialRefNode) material.Children[i];
                        if (mr._texture == null || !mr._texture.Enabled)
                        {
                            continue;
                        }

                        Matrix m = mr.GetTransform(_matrixNode != null || _manager.HasTextureMatrix[i]);
                        GL.LoadMatrix((float*) &m);

                        mr.Bind(material._programHandle);
                        _manager.ApplyTexture(mr.Coordinates);
                    }

                    anyRendered = true;
                    _manager.BindStream();
                    _manager.RenderMesh();
                }
                else //Fixed functionality
                {
                    GL.Enable(EnableCap.Texture2D);

                    _manager.BindStream();
                    for (int i = material.Children.Count - 1; i >= 0; i--)
                    {
                        MDL0MaterialRefNode mr = (MDL0MaterialRefNode) material.Children[i];
                        if (mr._texture == null || !mr._texture.Enabled)
                        {
                            continue;
                        }

                        anyRendered = true;

                        fixed (Matrix* m = &mr._frameState._transform)
                        {
                            GL.LoadMatrix((float*) m);
                        }

                        mr.Bind(material._programHandle);
                        _manager.ApplyTexture(mr.Coordinates);

                        _manager.RenderMesh();
                    }
                }
            }

            if (!anyRendered)
            {
                _manager.DisableTextures();
                _manager.RenderMesh();
            }

            _manager.DetachStreams();
        }

        private void AlphaTest(MDL0MaterialNode material)
        {
            GXAlphaFunction alpha = material._alphaFunc;

            AlphaOp logic = alpha.Logic;
            AlphaCompare func0 = alpha.Comp0;
            AlphaCompare func1 = alpha.Comp1;

            GL.Enable(EnableCap.AlphaTest);
            if (logic == AlphaOp.Or && (func0 == AlphaCompare.Always || func1 == AlphaCompare.Always) ||
                logic == AlphaOp.And && func0 == AlphaCompare.Always && func1 == AlphaCompare.Always)
            {
                GL.AlphaFunc(AlphaFunction.Always, 0.0f);
                GL.Disable(EnableCap.AlphaTest);
            }
            else if (logic == AlphaOp.And && (func0 == AlphaCompare.Never || func0 == AlphaCompare.Never) ||
                     logic == AlphaOp.Or && func0 == AlphaCompare.Never && func0 == AlphaCompare.Never)
            {
                GL.Enable(EnableCap.AlphaTest);
                GL.AlphaFunc(AlphaFunction.Never, 0.0f);
            }
            else
            {
                GL.Enable(EnableCap.AlphaTest);
                if (logic == AlphaOp.Or && func0 == AlphaCompare.Never ||
                    logic == AlphaOp.And && func0 == AlphaCompare.Always)
                {
                    GL.AlphaFunc(_alphaFunc[(int) func1], alpha._ref1 / 255.0f);
                }
                else
                {
                    GL.AlphaFunc(_alphaFunc[(int) func0], alpha._ref0 / 255.0f);
                }
            }
        }

        private void Blend(MDL0MaterialNode material)
        {
            BlendMode b = material._blendMode;
            if (b.EnableBlend /* && !b.EnableLogicOp*/)
            {
                GL.Enable(EnableCap.Blend);
                if (b.Subtract)
                {
                    GL.BlendEquation(BlendEquationMode.FuncSubtract);
                }
                else
                {
                    GL.BlendEquation(BlendEquationMode.FuncAdd);
                }

                GL.BlendFunc(_blendSrc[(int) b.SrcFactor], _blendDst[(int) b.DstFactor]);
            }
            else
            {
                GL.Disable(EnableCap.Blend);
            }

            if (b.EnableLogicOp /*&& !b.EnableBlend*/)
            {
                GL.Enable(EnableCap.ColorLogicOp);
                GL.LogicOp(_logicOp[(int) b.LogicOp]);
            }
            else
            {
                GL.Disable(EnableCap.ColorLogicOp);
            }
        }

        private void Cull(MDL0MaterialNode material)
        {
            if (material.CullMode == 0)
            {
                GL.Disable(EnableCap.CullFace);
            }
            else
            {
                GL.Enable(EnableCap.CullFace);
                GL.CullFace(_cullMode[(int) material.CullMode - 1]);
            }
        }

        private void DepthTest(MDL0MaterialNode material)
        {
            if (material._zMode.EnableDepthTest)
            {
                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(_depthFunc[(int) material._zMode.DepthFunction]);
                GL.DepthMask(material._zMode.EnableDepthUpdate);
            }
            else
            {
                GL.Disable(EnableCap.DepthTest);
            }
        }

        public void DrawBox()
        {
            TKContext.DrawWireframeBox(GetBox());
        }

        internal void Weight()
        {
            _manager?.Weight();
        }

        internal void Unweight(bool updateAssets)
        {
            _manager?.Unweight(updateAssets);
        }

        internal override void Bind()
        {
            //foreach (DrawCall c in _drawCalls)
            //    c.Bind();
        }

        internal override void Unbind()
        {
        }

        public void Attach()
        {
            _attached = true;
            Model.Attach();
        }

        public void Detach()
        {
            _attached = false;

            Model?.Detach();
        }

        public void Refresh()
        {
            Model?.Refresh();
        }

        public void PreRender(ModelPanelViewport v)
        {
            Model?.PreRender(v);
        }

        #endregion

        #region Etc

        public override void SignalPropertyChange()
        {
            base.SignalPropertyChange();

            TKContext.InvalidateModelPanels(this);
        }

        public void TryConvertMatrixToVertex()
        {
            if (_matrixNode != null)
            {
                IMatrixNode m = MatrixNode;
                DeferUpdateAssets();
                MatrixNode = null;
                foreach (Vertex3 v in _manager._vertices)
                {
                    v.DeferUpdateAssets();
                    v.MatrixNode = m;
                }
            }
        }

        public void TryConvertMatrixToObject()
        {
            if (_matrixNode == null)
            {
                IMatrixNode mtxNode = null;
                bool singlebind = true;

                int i = 0;
                foreach (Vertex3 v in _manager._vertices)
                {
                    i++;

                    if (mtxNode == null)
                    {
                        mtxNode = v.MatrixNode;
                    }

                    if (v.MatrixNode != mtxNode)
                    {
                        singlebind = false;
                        break;
                    }
                }

                if (singlebind)
                {
                    DeferUpdateAssets();
                    MatrixNode = mtxNode;
                }
            }
        }

        private bool _updateAssets = true;

        public void DeferUpdateAssets()
        {
            _updateAssets = false;
        }

        public void SetEditedAssets(bool forceNewNode, params bool[] types)
        {
            if (types.Length > 0 && types[0])
            {
                SetEditedVertices(forceNewNode);
            }

            if (types.Length > 1 && types[1])
            {
                SetEditedNormals(forceNewNode);
            }

            if (types.Length > 2 && types[2])
            {
                SetEditedColors(0, forceNewNode);
            }

            if (types.Length > 3 && types[3])
            {
                SetEditedColors(1, forceNewNode);
            }

            for (int i = 0; i < 8; i++)
            {
                if (types.Length > i + 4 && types[i + 4])
                {
                    SetEditedUVs(i, forceNewNode);
                }
            }
        }

        public void SetEditedVertices(bool forceNewNode = false)
        {
            _manager.PositionsChanged(this, forceNewNode);
        }

        public void SetEditedNormals(bool forceNewNode = false)
        {
            _manager.NormalsChanged(this, forceNewNode);
        }

        public void SetEditedColors(int id, bool forceNewNode = false)
        {
            _manager.ColorsChanged(this, id, forceNewNode);
        }

        public void SetEditedUVs(int id, bool forceNewNode = false)
        {
            _manager.UVsChanged(this, id, forceNewNode);
        }

        public MDL0ObjectNode HardCopy()
        {
            MDL0ObjectNode o = new MDL0ObjectNode
            {
                _manager = _manager.HardCopy(),
                Name = Name,
                _vertexNode = _vertexNode,
                _normalNode = _normalNode,
                _furVecNode = _furVecNode,
                _furPosNode = _furPosNode,
                _matrixNode = _matrixNode,
                _elementIndices = _elementIndices,
                _numFacepoints = _numFacepoints,
                _numFaces = _numFaces,
                _drawCalls = new BindingList<DrawCall>()
            };
            for (int i = 0; i < 2; i++)
            {
                o._colorSet[i] = _colorSet[i];
            }

            for (int i = 0; i < 8; i++)
            {
                o._uvSet[i] = _uvSet[i];
            }

            foreach (DrawCall c in _drawCalls)
            {
                o._drawCalls.Add(new DrawCall(o)
                {
                    MaterialNode = c.MaterialNode,
                    VisibilityBoneNode = c.VisibilityBoneNode,
                    DrawPriority = c.DrawPriority,
                    DrawPass = c.DrawPass
                });
            }

            o.SignalPropertyChange();
            return o;
        }

        public MDL0ObjectNode SoftCopy()
        {
            return MemberwiseClone() as MDL0ObjectNode;
        }

        public bool Deleting { get; private set; }

        public void Remove(bool v, bool n, bool c1, bool c2, params bool[] uv)
        {
            Deleting = true;
            MDL0Node node = Model;

            if (node == null)
            {
                base.Remove();
                return;
            }

            for (int i = 0; i < 2; i++)
            {
                if (_colorSet[i] != null)
                {
                    _colorSet[i]._objects.Remove(this);
                    if (_colorSet[i]._objects.Count == 0 && (i == 0 ? c1 : c2))
                    {
                        _colorSet[i].Remove();
                    }
                }
            }

            for (int i = 0; i < 8; i++)
            {
                if (_uvSet[i] != null)
                {
                    _uvSet[i]._objects.Remove(this);
                    if (_uvSet[i]._objects.Count == 0 && uv[i])
                    {
                        _uvSet[i].Remove();
                    }
                }
            }

            if (_vertexNode != null)
            {
                if (_vertexNode._objects.Count == 1 && v)
                {
                    _vertexNode.Remove();
                }
                else
                {
                    _vertexNode._objects.Remove(this);
                }
            }

            if (_normalNode != null)
            {
                if (_normalNode._objects.Count == 1 && n)
                {
                    _normalNode.Remove();
                }
                else
                {
                    _normalNode._objects.Remove(this);
                }
            }

            MatrixNode = null;

            foreach (DrawCall c in _drawCalls)
            {
                c.VisibilityBoneNode = null;
                c.MaterialNode = null;
            }

            _drawCalls.Clear();

            DrawCallsChanged?.Invoke(null, null);

            if (_manager != null)
            {
                foreach (Vertex3 vertex in _manager._vertices)
                {
                    vertex.MatrixNode?.Users.Remove(vertex);
                }
            }

            Dispose();
            base.Remove();
        }

        public override void Remove()
        {
            int j = 0;
            foreach (DrawCall dc in _drawCalls)
            {
                ++j;
                if (dc.MaterialNode._objects.Count == 1 &&
                    MessageBox.Show(
                        $"Do you want to remove this object's material{(_drawCalls.Count > 1 ? " " + j : "")}?",
                        "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dc.MaterialNode.Remove();
                }
            }

            Remove(
                _vertexNode != null && _vertexNode._objects.Count == 1 &&
                MessageBox.Show("Do you want to remove this object's vertex node?", "", MessageBoxButtons.YesNo) ==
                DialogResult.Yes,
                _normalNode != null && _normalNode._objects.Count == 1 &&
                MessageBox.Show("Do you want to remove this object's normal node?", "", MessageBoxButtons.YesNo) ==
                DialogResult.Yes,
                _colorSet[0] != null && _colorSet[0]._objects.Count == 1 &&
                MessageBox.Show("Do you want to remove this object's color node 1?", "", MessageBoxButtons.YesNo) ==
                DialogResult.Yes,
                _colorSet[1] != null && _colorSet[1]._objects.Count == 1 &&
                MessageBox.Show("Do you want to remove this object's color node 2?", "", MessageBoxButtons.YesNo) ==
                DialogResult.Yes,
                _uvSet[0] != null && _uvSet[0]._objects.Count == 1 &&
                MessageBox.Show("Do you want to remove this object's uv node 1?", "", MessageBoxButtons.YesNo) ==
                DialogResult.Yes,
                _uvSet[1] != null && _uvSet[1]._objects.Count == 1 &&
                MessageBox.Show("Do you want to remove this object's uv node 2?", "", MessageBoxButtons.YesNo) ==
                DialogResult.Yes,
                _uvSet[2] != null && _uvSet[2]._objects.Count == 1 &&
                MessageBox.Show("Do you want to remove this object's uv node 3?", "", MessageBoxButtons.YesNo) ==
                DialogResult.Yes,
                _uvSet[3] != null && _uvSet[3]._objects.Count == 1 &&
                MessageBox.Show("Do you want to remove this object's uv node 4?", "", MessageBoxButtons.YesNo) ==
                DialogResult.Yes,
                _uvSet[4] != null && _uvSet[4]._objects.Count == 1 &&
                MessageBox.Show("Do you want to remove this object's uv node 5?", "", MessageBoxButtons.YesNo) ==
                DialogResult.Yes,
                _uvSet[5] != null && _uvSet[5]._objects.Count == 1 &&
                MessageBox.Show("Do you want to remove this object's uv node 6?", "", MessageBoxButtons.YesNo) ==
                DialogResult.Yes,
                _uvSet[6] != null && _uvSet[6]._objects.Count == 1 &&
                MessageBox.Show("Do you want to remove this object's uv node 7?", "", MessageBoxButtons.YesNo) ==
                DialogResult.Yes,
                _uvSet[7] != null && _uvSet[7]._objects.Count == 1 &&
                MessageBox.Show("Do you want to remove this object's uv node 8?", "", MessageBoxButtons.YesNo) ==
                DialogResult.Yes);
        }

        public void Remove(bool removeAttached)
        {
            int j = 0;
            foreach (DrawCall dc in _drawCalls)
            {
                ++j;
                if (dc.MaterialNode._objects.Count == 1 && removeAttached)
                {
                    dc.MaterialNode.Remove();
                }
            }

            Remove(removeAttached, removeAttached, removeAttached, removeAttached, removeAttached, removeAttached,
                removeAttached, removeAttached, removeAttached, removeAttached, removeAttached, removeAttached);
        }

        #endregion

        public event EventHandler DrawCallsChanged;

        public void OnDrawCallsChanged()
        {
            if (_attached)
            {
                DrawCallsChanged?.Invoke(this, null);
            }
            else if (Model != null && Model.Attached)
            {
                Model.OnDrawCallsChanged();
            }
        }

        [Browsable(false)] public List<DrawCallBase> DrawCalls => _drawCalls.Select(x => x as DrawCallBase).ToList();
    }

    public unsafe class DrawCall : DrawCallBase
    {
        public DrawCall(MDL0ObjectNode parent)
        {
            _parentObject = parent;
        }

        [Browsable(false)] public override IObject Parent => _parentObject;

        public MDL0ObjectNode _parentObject;

        public override string ToString()
        {
            return
                $"{(_isXLU ? "XLU" : "OPA")} {_drawOrder.ToString()}: {(_material == null ? "(null)" : _material.Name)} {(_visBoneNode == null ? "(null)" : _visBoneNode.Name)}";
        }

        //Data stored in actual MDL0 opa/xlu draw call
        internal MDL0BoneNode _visBoneNode;
        internal MDL0MaterialNode _material;
        internal bool _isXLU;
        internal byte _drawOrder;

        [Category("Object Draw Call")]
        [TypeConverter(typeof(DropDownListMaterialsDrawCall))]
        public string Material
        {
            get => _material == null ? null : _material._name;
            set
            {
                MaterialNode = string.IsNullOrEmpty(value) ? null :
                    _isXLU ? _parentObject.Model.FindOrCreateXluMaterial(value) :
                    _parentObject.Model.FindOrCreateOpaMaterial(value);
                _parentObject.SignalPropertyChange();
            }
        }

        [Category("Object Draw Call")]
        [TypeConverter(typeof(DropDownListBonesDrawCall))]
        public string VisibilityBone //This attaches the object to a bone controlled by a VIS0
        {
            get => _visBoneNode == null ? null : _visBoneNode._name;
            set
            {
                VisibilityBoneNode = string.IsNullOrEmpty(value) ? null : _parentObject.Model.FindBone(value);
                _parentObject.Model.SignalPropertyChange();
            }
        }

        public enum DrawPassType
        {
            Opaque,
            Transparent
        }

        [Category("Object Draw Call")]
        public DrawPassType DrawPass
        {
            get => _isXLU ? DrawPassType.Transparent : DrawPassType.Opaque;
            set
            {
                _isXLU = value == DrawPassType.Opaque ? false : true;
                _parentObject.SignalPropertyChange();
            }
        }

        [Category("Object Draw Call")]
        public byte DrawPriority
        {
            get => _drawOrder;
            set
            {
                _drawOrder = value;
                _parentObject.SignalPropertyChange();
            }
        }

        /// <summary>
        /// Does not signal a property change!
        /// </summary>
        [Browsable(false)]
        public MDL0MaterialNode MaterialNode
        {
            get => _material;
            set
            {
                if (_material == value)
                {
                    return;
                }

                _material?._objects.Remove(_parentObject);

                if ((_material = value) != null)
                {
                    _material._objects.Add(_parentObject);
                }
            }
        }

        /// <summary>
        /// Does not signal a property change!
        /// </summary>
        [Browsable(false)]
        public MDL0BoneNode VisibilityBoneNode
        {
            get => _visBoneNode;
            set
            {
                if (_visBoneNode == value)
                {
                    return;
                }

                if (_visBoneNode != null && _visBoneNode._visDrawCalls.Contains(this))
                {
                    _visBoneNode._visDrawCalls.Remove(this);
                }

                if ((_visBoneNode = value) != null)
                {
                    if (!_visBoneNode._visDrawCalls.Contains(this))
                    {
                        _visBoneNode._visDrawCalls.Add(this);
                    }

                    _render = _visBoneNode._boneFlags.HasFlag(BoneFlags.Visible);
                }
            }
        }

        public override int CompareTo(DrawCallBase y)
        {
            return DrawCompare(this, y as DrawCall);
        }

        public static int DrawCompare(DrawCall n1, DrawCall n2)
        {
            DrawPassType __n1DrawPass = n1.MaterialNode == null ? DrawPassType.Opaque : n1.DrawPass;
            DrawPassType __n2DrawPass = n2.MaterialNode == null ? DrawPassType.Opaque : n2.DrawPass;

            //First compare with render pass
            if (__n1DrawPass == DrawPassType.Opaque && __n2DrawPass == DrawPassType.Transparent)
            {
                return -1;
            }

            if (__n2DrawPass == DrawPassType.Opaque && __n1DrawPass == DrawPassType.Transparent)
            {
                return 1;
            }

            Debug.Assert(__n1DrawPass == __n2DrawPass);
            DrawPassType __drawPass = __n1DrawPass;
            Debug.Assert(__drawPass == DrawPassType.Opaque || __drawPass == DrawPassType.Transparent);

            //Compare draw priorities
            if (n1.DrawPriority < n2.DrawPriority)
            {
                return -1;
            }

            if (n1.DrawPriority > n2.DrawPriority)
            {
                return 1;
            }

            if (__drawPass == DrawPassType.Transparent) // Not doing this for both render pass types is probably a bug in NW4R
            {
                int __n1MaterialNodeIndex = n1.MaterialNode == null ? -1 : n1.MaterialNode.Index;
                int __n2MaterialNodeIndex = n2.MaterialNode == null ? -1 : n2.MaterialNode.Index;

                //Now check material draw index
                if (__n1MaterialNodeIndex < __n2MaterialNodeIndex)
                {
                    return -1;
                }

                if (__n1MaterialNodeIndex > __n2MaterialNodeIndex)
                {
                    return 1;
                }
            }

            return 1;
        }

        public override void Bind()
        {
            if (_visBoneNode == null)
            {
                _render = true;
            }
            else if (_visBoneNode._name == "EyeYellowM")
            {
                _render = false;
            }
            else
            {
                _render = _visBoneNode._boneFlags.HasFlag(BoneFlags.Visible);
            }

            if (DrawPass == DrawPassType.Transparent &&
                MaterialNode != null &&
                MaterialNode.Children.Count != 0 &&
                MaterialNode.Children[0].Name == "TShadow1")
            {
                _render = false;
            }
        }

        public override void Render(ModelPanelViewport viewport)
        {
            if (!_render || _parentObject == null)
            {
                return;
            }

            MDL0Node model = _parentObject.Model;
            bool usesOffset = model?._matrixOffset != null;
            if (usesOffset)
            {
                GL.PushMatrix();
                Matrix m = model._matrixOffset.Value;
                GL.MultMatrix((float*) &m);
            }

            ModelRenderAttributes attrib = viewport._renderAttrib;
            MDL0MaterialNode mat = MaterialNode;
            if (attrib._renderMetal && !mat.IsMetal && mat.MetalMaterial != null)
            {
                mat = mat.MetalMaterial;
            }

            if (attrib._renderPolygons)
            {
                bool shaders = attrib._renderShaders && mat != null;
                if (shaders && !mat._scn0Applied)
                {
                    mat.ApplyViewportLighting(viewport);
                }

                GL.Enable(EnableCap.PolygonOffsetFill);
                GL.PolygonOffset(0, 0f);

                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                _parentObject.Render(false, shaders, mat);
            }

            if (attrib._renderWireframe)
            {
                GL.Disable(EnableCap.PolygonOffsetFill);
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                GL.LineWidth(0.5f);
                _parentObject.Render(true, false, mat);
            }

            if (usesOffset)
            {
                GL.PopMatrix();
            }
        }
    }

    public interface IMatrixNodeUser //Objects and Vertices
    {
        //The next time MatrixNode is set, vertices and normals will not be updated.
        //This is so that assets aren't updated until after vertex loops complete.
        //Resets after MatrixNode is set.
        void DeferUpdateAssets();
        IMatrixNode MatrixNode { get; set; }
    }
}