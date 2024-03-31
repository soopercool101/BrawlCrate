using BrawlLib.Internal;
using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Models;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0BoneNode : MDL0EntryNode, IBoneNode
    {
        internal MDL0Bone* Header => (MDL0Bone*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.MDL0Bone;
        public override bool AllowDuplicateNames => true;
        public override bool RetainChildrenOnReplace => true;

        private MDL0BoneNode _overrideBone;

        [Browsable(false)]
        public MDL0BoneNode OverrideBone
        {
            get => _overrideBone;
            set => _overrideBone = value;
        }

        public MDL0BoneNode Clone()
        {
            MDL0BoneNode b = new MDL0BoneNode
            {
                _name = _name,
                _bindState = new FrameState(_bindState._scale, _bindState._rotate, _bindState._translate),
                _billboardFlags = _billboardFlags,
                _boneFlags = _boneFlags,
                _extents = _extents
            };
            return b;
        }

        public bool _locked; //For the weight editor

        public Box _extents;
        public BoneFlags _boneFlags = (BoneFlags) 0x11F;
        public BillboardFlags _billboardFlags;
        public MDL0BoneNode _bbRefNode;

        public List<MDL0ObjectNode> _singleBindObjects = new List<MDL0ObjectNode>();
        public List<DrawCall> _visDrawCalls = new List<DrawCall>();

        public FrameState _bindState = FrameState.Neutral;
        public Matrix _bindMatrix = Matrix.Identity, _inverseBindMatrix = Matrix.Identity;
        public FrameState _frameState = FrameState.Neutral;
        public Matrix _frameMatrix = Matrix.Identity, _inverseFrameMatrix = Matrix.Identity;

        public int _nodeIndex, _weightCount; //For rebuilding only

        #region IBoneNode Implementation

        [Browsable(false)] public IModel IModel => Model;

        [Browsable(false)]
        public FrameState FrameState
        {
            get => _frameState;
            set => _frameState = value;
        }

        [Browsable(false)]
        public FrameState BindState
        {
            get => _bindState;
            set => _bindState = value;
        }

        [Browsable(false)]
        public Color BoneColor
        {
            get => _boneColor;
            set => _boneColor = value;
        }

        [Browsable(false)]
        public Color NodeColor
        {
            get => _nodeColor;
            set => _nodeColor = value;
        }

        [Browsable(false)]
        public int WeightCount
        {
            get => _weightCount;
            set => _weightCount = value;
        }

        [Browsable(false)]
        public bool Locked
        {
            get => _locked;
            set => _locked = value;
        }

        [Category("Bone")]
        [Browsable(false)]
        [TypeConverter(typeof(MatrixStringConverter))]
        public Matrix BindMatrix
        {
            get => _bindMatrix;
            set
            {
                _bindMatrix = value;
                SignalPropertyChange();
            }
        }

        [Category("Bone")]
        [Browsable(false)]
        [TypeConverter(typeof(MatrixStringConverter))]
        public Matrix InverseBindMatrix
        {
            get => _inverseBindMatrix;
            set
            {
                _inverseBindMatrix = value;
                SignalPropertyChange();
            }
        }

        #region IMatrixNode Implementation

        [Category("Bone")] [Browsable(false)] public Matrix Matrix => _frameMatrix;
        [Category("Bone")] [Browsable(false)] public Matrix InverseMatrix => _inverseFrameMatrix;

        [Browsable(false)] public int NodeIndex => _nodeIndex;
        [Browsable(false)] public bool IsPrimaryNode => true;

        private List<BoneWeight> _weightRef;

        [Browsable(false)]
        public List<BoneWeight> Weights =>
            _weightRef ?? (_weightRef = new List<BoneWeight> {new BoneWeight(this, 1.0f)});

        [Browsable(false)]
        public List<IMatrixNodeUser> Users
        {
            get => _users;
            set => _users = value;
        }

        internal List<IMatrixNodeUser> _users = new List<IMatrixNodeUser>();

        #endregion

        #endregion

        #region Properties

        [Category("Bone")]
        [Description("These draw calls use this bone to control their visibility.")]
        public string[] VisibilityDrawCalls =>
            _visDrawCalls.Select(x => x._parentObject + " " + x).ToArray();

        [Category("Bone")]
        [Description("These objects use this bone as a single-bind influence (the only bone they're rigged to).")]
        public MDL0ObjectNode[] SingleBindObjects => _singleBindObjects.ToArray();

        [Category("Bone")]
        [Description("Determines if any objects that use this bone for visibility should be visible by default.")]
        public bool Visible
        {
            get => _boneFlags.HasFlag(BoneFlags.Visible);
            set
            {
                if (value)
                {
                    _boneFlags |= BoneFlags.Visible;
                }
                else
                {
                    _boneFlags &= ~BoneFlags.Visible;
                }

                SignalPropertyChange();
            }
        }

        [Category("Bone")]
        public bool SegScaleCompApply
        {
            get => _boneFlags.HasFlag(BoneFlags.SegScaleCompApply);
            set
            {
                if (value)
                {
                    _boneFlags |= BoneFlags.SegScaleCompApply;
                }
                else
                {
                    _boneFlags &= ~BoneFlags.SegScaleCompApply;
                }

                SignalPropertyChange();
            }
        }

        [Category("Bone")]
        public bool SegScaleCompParent
        {
            get => _boneFlags.HasFlag(BoneFlags.SegScaleCompParent);
            set
            {
                if (value)
                {
                    _boneFlags |= BoneFlags.SegScaleCompParent;
                }
                else
                {
                    _boneFlags &= ~BoneFlags.SegScaleCompParent;
                }

                SignalPropertyChange();
            }
        }

        [Category("Bone")]
        public bool ClassicScale
        {
            get => !_boneFlags.HasFlag(BoneFlags.ClassicScaleOff);
            set
            {
                if (!value)
                {
                    _boneFlags |= BoneFlags.ClassicScaleOff;
                }
                else
                {
                    _boneFlags &= ~BoneFlags.ClassicScaleOff;
                }

                SignalPropertyChange();
            }
        }

        [Category("Bone")]
        [Description("The index of this bone in the raw array of bones in the file.")]
        public int BoneIndex
        {
            get => _entryIndex;
            set
            {
                if (_entryIndex == value)
                {
                    return;
                }

                bool down = value < _entryIndex;

                _entryIndex = value;

                MDL0Node model = Model;
                if (model?._linker?.BoneCache != null)
                {
                    if (down)
                    {
                        foreach (MDL0BoneNode b in model._linker.BoneCache)
                        {
                            if (b._entryIndex >= _entryIndex && b != this)
                            {
                                b._entryIndex++;
                            }
                        }
                    }

                    model._linker.RegenerateBoneCache();
                    UpdateProperties();
                }

                SignalPropertyChange();
            }
        }

        [Category("Bone")]
        [Description(@"This setting will rotate the bone and all influenced geometry in relation to the camera.
If the setting is 'Perspective', the bone's Z axis points at the camera's position.
Otherwise, the bone's Z axis is parallel to the camera's Z axis.

Standard: Default; no rotation restrictions. Is affected by the parent bone's rotation.
Rotation: The bone's Y axis is parallel to the camera's Y axis. Is NOT affected by the parent bone's rotation.
Y: Only the Y axis is allowed to rotate. Is affected by the parent bone's rotation.")]
        public BillboardFlags BillboardSetting
        {
            get => _billboardFlags;
            set
            {
                if (_billboardFlags == value)
                {
                    return;
                }

                MDL0Node model = Model;
                if (_billboardFlags != BillboardFlags.Off && model._billboardBones.Contains(this))
                {
                    model._billboardBones.Remove(this);
                }

                if ((_billboardFlags = value) != BillboardFlags.Off && !model._billboardBones.Contains(this))
                {
                    model._billboardBones.Add(this);
                }

                OnBillboardModeChanged();
                SignalPropertyChange();
            }
        }

        private void OnBillboardModeChanged()
        {
            if (BillboardSetting == BillboardFlags.Off)
            {
                MDL0BoneNode n = this;
                while ((n = n.Parent as MDL0BoneNode) != null)
                {
                    if (n.BillboardSetting != BillboardFlags.Off)
                    {
                        break;
                    }
                }

                if (n != null)
                {
                    BBRefNode = n;
                    foreach (MDL0BoneNode b in Children)
                    {
                        b.RecursiveSetBillboard(BBRefNode);
                    }
                }
                else
                {
                    BBRefNode = null;
                    foreach (MDL0BoneNode b in Children)
                    {
                        b.RecursiveSetBillboard(null);
                    }
                }
            }
            else
            {
                BBRefNode = null;
                foreach (MDL0BoneNode b in Children)
                {
                    b.RecursiveSetBillboard(this);
                }
            }
        }

        private void RecursiveSetBillboard(MDL0BoneNode node)
        {
            if (BillboardSetting == BillboardFlags.Off)
            {
                BBRefNode = node;
                foreach (MDL0BoneNode b in Children)
                {
                    b.RecursiveSetBillboard(node);
                }
            }
        }

        [Category("Bone")]
        [TypeConverter(typeof(DropDownListBones))]
        public string BillboardRefBone
        {
            get => _bbRefNode == null ? string.Empty : _bbRefNode.Name;
            set
            {
                BBRefNode = string.IsNullOrEmpty(value) ? null : Model.FindBone(value);
                SignalPropertyChange();
            }
        }

        [Browsable(false)]
        public MDL0BoneNode BBRefNode
        {
            get => _bbRefNode;
            set
            {
                _bbRefNode = value;

                if (_bbRefNode != null)
                {
                    _boneFlags |= BoneFlags.HasBillboardParent;
                }
                else
                {
                    _boneFlags &= ~BoneFlags.HasBillboardParent;
                }

                SignalPropertyChange();
            }
        }

        [Category("Bone")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Scale
        {
            get => _bindState._scale;
            set
            {
                _bindState.Scale = value;

                if (value == new Vector3(1))
                {
                    _boneFlags |= BoneFlags.FixedScale;
                }
                else
                {
                    _boneFlags &= ~BoneFlags.FixedScale;
                }

                if (value._x == value._y && value._y == value._z)
                {
                    _boneFlags |= BoneFlags.ScaleEqual;
                }
                else
                {
                    _boneFlags &= ~BoneFlags.ScaleEqual;
                }

                //RecalcBindState();

                if (Parent is MDL0BoneNode)
                {
                    if (BindMatrix == ((MDL0BoneNode) Parent).BindMatrix &&
                        InverseBindMatrix == ((MDL0BoneNode) Parent).InverseBindMatrix)
                    {
                        _boneFlags |= BoneFlags.NoTransform;
                    }
                    else
                    {
                        _boneFlags &= ~BoneFlags.NoTransform;
                    }
                }
                else if (BindMatrix == Matrix.Identity && InverseBindMatrix == Matrix.Identity)
                {
                    _boneFlags |= BoneFlags.NoTransform;
                }
                else
                {
                    _boneFlags &= ~BoneFlags.NoTransform;
                }

                SignalPropertyChange();
            }
        }

        [Category("Bone")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Rotation
        {
            get => _bindState._rotate;
            set
            {
                _bindState.Rotate = value;

                if (value == new Vector3())
                {
                    _boneFlags |= BoneFlags.FixedRotation;
                }
                else
                {
                    _boneFlags &= ~BoneFlags.FixedRotation;
                }

                //RecalcBindState();

                if (Parent is MDL0BoneNode)
                {
                    if (BindMatrix == ((MDL0BoneNode) Parent).BindMatrix &&
                        InverseBindMatrix == ((MDL0BoneNode) Parent).InverseBindMatrix)
                    {
                        _boneFlags |= BoneFlags.NoTransform;
                    }
                    else
                    {
                        _boneFlags &= ~BoneFlags.NoTransform;
                    }
                }
                else if (BindMatrix == Matrix.Identity && InverseBindMatrix == Matrix.Identity)
                {
                    _boneFlags |= BoneFlags.NoTransform;
                }
                else
                {
                    _boneFlags &= ~BoneFlags.NoTransform;
                }

                SignalPropertyChange();
            }
        }

        [Category("Bone")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Translation
        {
            get => _bindState._translate;
            set
            {
                _bindState.Translate = value;

                if (value == new Vector3())
                {
                    _boneFlags |= BoneFlags.FixedTranslation;
                }
                else
                {
                    _boneFlags &= ~BoneFlags.FixedTranslation;
                }

                //RecalcBindState();

                if (Parent is MDL0BoneNode)
                {
                    if (BindMatrix == ((MDL0BoneNode) Parent).BindMatrix &&
                        InverseBindMatrix == ((MDL0BoneNode) Parent).InverseBindMatrix)
                    {
                        _boneFlags |= BoneFlags.NoTransform;
                    }
                    else
                    {
                        _boneFlags &= ~BoneFlags.NoTransform;
                    }
                }
                else if (BindMatrix == Matrix.Identity && InverseBindMatrix == Matrix.Identity)
                {
                    _boneFlags |= BoneFlags.NoTransform;
                }
                else
                {
                    _boneFlags &= ~BoneFlags.NoTransform;
                }

                SignalPropertyChange();
            }
        }

        [Category("Bone")]
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

        [Category("Bone")]
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

        [Category("Bone")]
        public BoneFlags Flags
        {
            get => _boneFlags;
            set
            {
                _boneFlags = value;
                SignalPropertyChange();
            }
        }

        //[Category("Kinect Settings"), Browsable(true)]
        //public SkeletonJoint Joint
        //{
        //    get { return _joint; }
        //    set { _joint = value; }
        //}
        //public SkeletonJoint _joint;

        [Category("User Data")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public UserDataCollection UserEntries
        {
            get => _userEntries;
            set
            {
                _userEntries = value;
                SignalPropertyChange();
            }
        }

        internal UserDataCollection _userEntries = new UserDataCollection();

        #endregion

        public override void OnMoved()
        {
            Model._linker.RegenerateBoneCache();
            SignalPropertyChange();
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            foreach (MDL0BoneNode n in Children)
            {
                n.GetStrings(table);
            }

            _userEntries.GetStrings(table);
        }

        //Initialize should only be called from parent group during parse.
        //Bones need not be imported/exported anyways
        public override bool OnInitialize()
        {
            MDL0Bone* header = Header;

            SetSizeInternal(header->_headerLen);

            //Conditional name assignment
            if (_name == null && header->_stringOffset != 0)
            {
                _name = header->ResourceString;
            }

            //Assign fields
            _boneFlags = (BoneFlags) (uint) header->_flags;
            _billboardFlags = (BillboardFlags) (uint) header->_bbFlags;
            _nodeIndex = header->_nodeId;
            _entryIndex = header->_index;

            //Bone cache isn't done parsing yet, so set billboard ref node later

            if (_billboardFlags != BillboardFlags.Off)
            {
                Model._billboardBones.Add(this); //Update mesh in T-Pose
            }

            _bindState = _frameState = new FrameState(header->_scale, header->_rotation, header->_translation);
            _bindMatrix = _frameMatrix = header->_transform;
            _inverseBindMatrix = _inverseFrameMatrix = header->_transformInv;

            _extents = header->_extents;

            (_userEntries = new UserDataCollection()).Read(header->UserDataAddress, RootNode.WorkingUncompressed);

            //We don't want to process children because not all have been parsed yet.
            //Child assignments will be handled by the parent group.
            return false;
        }

        //Use MoveRaw without processing children.
        //Prevents addresses from changing before completion.
        //internal override void MoveRaw(VoidPtr address, int length)
        //{
        //    Memory.Move(address, WorkingSource.Address, (uint)length);
        //    DataSource newsrc = new DataSource(address, length);
        //    if (_compression == CompressionType.None)
        //    {
        //        _replSrc.Close();
        //        _replUncompSrc.Close();
        //        _replSrc = _replUncompSrc = newsrc;
        //    }
        //    else
        //    {
        //        _replSrc.Close();
        //        _replSrc = newsrc;
        //    }
        //}

        public override int OnCalculateSize(bool force)
        {
            return 0xD0 + _userEntries.GetSize();
        }

        public override void RemoveChild(ResourceNode child)
        {
            base.RemoveChild(child);
            OnMoved();
        }

        public void CalculateOffsets()
        {
            MDL0Bone* header = (MDL0Bone*) WorkingUncompressed.Address;

            //Set first child
            header->_firstChildOffset =
                _children.Count > 0 ? (int) Children[0].WorkingUncompressed.Address - (int) header : 0;

            if (Parent != null)
            {
                int index = Index;

                //Parent
                header->_parentOffset =
                    Parent is MDL0BoneNode ? (int) Parent.WorkingUncompressed.Address - (int) header : 0;

                //Previous sibling
                header->_prevOffset = index == 0
                    ? 0
                    : (int) Parent._children[index - 1].WorkingUncompressed.Address - (int) header;

                //Next sibling
                header->_nextOffset = index == Parent._children.Count - 1
                    ? 0
                    : (int) Parent._children[index + 1].WorkingUncompressed.Address - (int) header;
            }
        }

        public void CalcFlags()
        {
            _boneFlags = BoneFlags.Visible;

            if (Scale._x == Scale._y && Scale._y == Scale._z)
            {
                _boneFlags |= BoneFlags.ScaleEqual;
            }

            if (Users.Count > 0)
            {
                _boneFlags |= BoneFlags.HasGeometry;
            }

            if (Scale == new Vector3(1))
            {
                _boneFlags |= BoneFlags.FixedScale;
            }

            if (Rotation == new Vector3(0))
            {
                _boneFlags |= BoneFlags.FixedRotation;
            }

            if (Translation == new Vector3(0))
            {
                _boneFlags |= BoneFlags.FixedTranslation;
            }

            if (Parent is MDL0BoneNode)
            {
                if (BindMatrix == ((MDL0BoneNode) Parent).BindMatrix &&
                    InverseBindMatrix == ((MDL0BoneNode) Parent).InverseBindMatrix)
                {
                    _boneFlags |= BoneFlags.NoTransform;
                }
            }
            else if (BindMatrix == Matrix.Identity && InverseBindMatrix == Matrix.Identity)
            {
                _boneFlags |= BoneFlags.NoTransform;
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            MDL0Bone* header = (MDL0Bone*) address;

            if (Users.Count > 0 || SingleBindObjects.Length > 0)
            {
                _boneFlags |= BoneFlags.HasGeometry;
            }
            else
            {
                _boneFlags &= ~BoneFlags.HasGeometry;
            }

            header->_headerLen = length;
            header->_index = _entryIndex;
            header->_nodeId = _nodeIndex;
            header->_flags = (uint) _boneFlags;
            header->_bbFlags = (uint) _billboardFlags;
            header->_bbIndex = _bbRefNode == null ? 0 : (uint) _bbRefNode._entryIndex;
            header->_scale = _bindState._scale;
            header->_rotation = _bindState._rotate;
            header->_translation = _bindState._translate;
            header->_extents = _extents;
            header->_transform = _bindMatrix;
            header->_transformInv = _inverseBindMatrix;

            if (_userEntries.Count > 0)
            {
                header->_userDataOffset = 0xD0;
                _userEntries.Write(address + 0xD0);
            }
            else
            {
                header->_userDataOffset = 0;
            }
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0Bone* header = (MDL0Bone*) dataAddress;
            header->MDL0Address = mdlAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;

            _userEntries.PostProcess(dataAddress + 0xD0, stringTable);
        }

        //Only updates single-bind vertices and objects!
        //Influences using this bone must be handled separately
        private void InfluenceAssets(bool influence)
        {
            Matrix m = influence ? InverseBindMatrix : BindMatrix;

            Matrix rm = m.GetRotationMatrix();
            foreach (IMatrixNodeUser user in Users)
            {
                if (user is MDL0ObjectNode)
                {
                    Vector3* pData = null;
                    MDL0ObjectNode o = user as MDL0ObjectNode;

                    if (o._manager._faceData[1] != null)
                    {
                        pData = (Vector3*) o._manager._faceData[1].Address;
                    }

                    foreach (Vertex3 v in o._manager._vertices)
                    {
                        v.Position *= m;

                        if (pData != null)
                        {
                            foreach (int i in v.FaceDataIndices)
                            {
                                pData[i] *= rm;
                            }
                        }
                    }
                }
                else if (user is Vertex3)
                {
                    Vertex3 v = user as Vertex3;
                    MDL0ObjectNode o = v.Parent as MDL0ObjectNode;

                    v.Position *= m;

                    if (o._manager._faceData[1] != null)
                    {
                        Vector3* pData = (Vector3*) o._manager._faceData[1].Address;
                        foreach (int i in v.FaceDataIndices)
                        {
                            pData[i] *= rm;
                        }
                    }
                }
            }

            foreach (MDL0ObjectNode o in _singleBindObjects)
            {
                Vector3* pData = null;
                if (o._manager._faceData[1] != null)
                {
                    pData = (Vector3*) o._manager._faceData[1].Address;
                }

                foreach (Vertex3 v in o._manager._vertices)
                {
                    v.Position *= m;

                    if (pData != null)
                    {
                        foreach (int i in v.FaceDataIndices)
                        {
                            pData[i] *= rm;
                        }
                    }
                }
            }
        }

        //Change has been made to bind state, need to recalculate matrices
        public void RecalcBindState(bool updateMesh, bool moveMeshWithBone, bool updateAssetLists = true)
        {
            if (!updateMesh)
            {
                RecursiveRecalcBindState(true, false);
            }
            else
            {
                //Get all objects that are influenced by these bones
                List<MDL0ObjectNode> changed = new List<MDL0ObjectNode>();
                if (moveMeshWithBone || updateAssetLists)
                {
                    RecursiveGetInfluencedObjects(ref changed);
                }

                if (!moveMeshWithBone) //Need to stop vertices rigged to one bone from moving
                {
                    RecursiveRecalcBindState(false, false);
                }
                else //Need to move vertices rigged to influences to the new position
                {
                    //Note: vertex position precision goes down over time for influences
                    //Try not to call this too much, or manual vertex fixes will be needed

                    Model.ApplyCHR(null, 0);
                    foreach (MDL0ObjectNode o in changed)
                    {
                        if (o._manager != null)
                        {
                            Vector3* pData = (Vector3*) o._manager._faceData[1].Address;
                            if (o.MatrixNode != null)
                            {
                                if (o.MatrixNode is Influence)
                                {
                                    Influence inf = (Influence) o.MatrixNode;
                                    foreach (Vertex3 v in o._manager._vertices)
                                    {
                                        v._position *= inf.Matrix;
                                        foreach (int i in v.FaceDataIndices)
                                        {
                                            pData[i] *= inf.Matrix.GetRotationMatrix();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (Vertex3 v in o._manager._vertices)
                                {
                                    if (v.MatrixNode is Influence)
                                    {
                                        Influence inf = (Influence) v.MatrixNode;
                                        v._position *= inf.Matrix;
                                        foreach (int i in v.FaceDataIndices)
                                        {
                                            pData[i] *= inf.Matrix.GetRotationMatrix();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    RecursiveRecalcBindState(true, false);
                    foreach (Influence inf in Model._influences._influences)
                    {
                        inf.CalcMatrix();
                    }

                    foreach (MDL0ObjectNode o in changed)
                    {
                        if (o._manager != null)
                        {
                            Vector3* pData = (Vector3*) o._manager._faceData[1].Address;
                            if (o.MatrixNode != null)
                            {
                                if (o.MatrixNode is Influence)
                                {
                                    Influence inf = (Influence) o.MatrixNode;
                                    foreach (Vertex3 v in o._manager._vertices)
                                    {
                                        v._position *= inf.InverseMatrix;
                                        foreach (int i in v.FaceDataIndices)
                                        {
                                            pData[i] *= inf.InverseMatrix.GetRotationMatrix();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (Vertex3 v in o._manager._vertices)
                                {
                                    if (v.MatrixNode is Influence)
                                    {
                                        Influence inf = (Influence) v.MatrixNode;
                                        v._position *= inf.InverseMatrix;
                                        foreach (int i in v.FaceDataIndices)
                                        {
                                            pData[i] *= inf.InverseMatrix.GetRotationMatrix();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //Update the external arrays with the new unweighted vertices and normals
                if (updateAssetLists)
                {
                    foreach (MDL0ObjectNode o in changed)
                    {
                        o.SetEditedAssets(false, true, true);
                    }
                }
            }
        }

        private void RecursiveGetInfluencedObjects(ref List<MDL0ObjectNode> changed)
        {
            foreach (IMatrixNodeUser user in Users)
            {
                if (user is MDL0ObjectNode)
                {
                    MDL0ObjectNode o = user as MDL0ObjectNode;

                    if (!changed.Contains(o))
                    {
                        changed.Add(o);
                    }
                }
                else if (user is Vertex3)
                {
                    Vertex3 v = user as Vertex3;
                    MDL0ObjectNode o = v.Parent as MDL0ObjectNode;

                    if (!changed.Contains(o))
                    {
                        changed.Add(o);
                    }
                }
            }

            foreach (MDL0ObjectNode o in _singleBindObjects)
            {
                if (!changed.Contains(o))
                {
                    changed.Add(o);
                }
            }

            foreach (MDL0BoneNode bone in Children)
            {
                bone.RecursiveGetInfluencedObjects(ref changed);
            }
        }

        private void RecursiveRecalcBindState(bool updateMesh, bool animPose)
        {
            if (!updateMesh)
            {
                InfluenceAssets(false);
            }

            if (_parent is MDL0BoneNode)
            {
                _bindMatrix = ((MDL0BoneNode) _parent)._bindMatrix * _bindState._transform;
                _inverseBindMatrix = _bindState._iTransform * ((MDL0BoneNode) _parent)._inverseBindMatrix;
            }
            else
            {
                _bindMatrix = _bindState._transform;
                _inverseBindMatrix = _bindState._iTransform;
            }

            if (!updateMesh)
            {
                InfluenceAssets(true);
            }

            foreach (MDL0BoneNode bone in Children)
            {
                bone.RecursiveRecalcBindState(updateMesh, animPose);
            }

            SignalPropertyChange();
        }

        public Vector3 _overrideLocalTranslate;

        public void RecalcFrameState(ModelPanelViewport v = null)
        {
            if (_overrideBone != null)
            {
                _frameMatrix = _overrideBone._frameMatrix;
                _inverseFrameMatrix = _overrideBone._inverseFrameMatrix;
            }
            else
            {
                if (_overrideLocalTranslate != new Vector3())
                {
                    _frameState = new FrameState(_frameState.Scale, _frameState.Rotate,
                        _frameState.Translate + _overrideLocalTranslate);
                }

                if (_parent is MDL0BoneNode)
                {
                    _frameMatrix = ((MDL0BoneNode) _parent)._frameMatrix * _frameState._transform;
                    _inverseFrameMatrix = _frameState._iTransform * ((MDL0BoneNode) _parent)._inverseFrameMatrix;
                }
                else
                {
                    _frameMatrix = _frameState._transform;
                    _inverseFrameMatrix = _frameState._iTransform;
                }
            }

            if (BillboardSetting != BillboardFlags.Off &&
                v != null &&
                v.ApplyBillboardBones)
            {
                ApplyBillboard(v.Camera);
            }

            foreach (MDL0BoneNode bone in Children)
            {
                bone.RecalcFrameState(v);
            }
        }

        public void ApplyBillboard(GLCamera camera)
        {
            if (BillboardSetting == BillboardFlags.Off)
            {
                return;
            }

            Vector3 camPoint = camera.GetPoint();
            Vector3 camRot = camera._rotation;

            FrameState worldState = _frameMatrix.Derive();

            Matrix m = Matrix.Identity, mInv = Matrix.Identity;

            Vector3 rot = ((int) BillboardSetting & 1) == 0
                ? //If perspective
                worldState.Translate.LookatAngles(camPoint) * Maths._rad2degf
                :       //Point at camera position
                camRot; //Set parallel to the camera

            switch (BillboardSetting)
            {
                case BillboardFlags.Standard:
                case BillboardFlags.StandardPerspective:

                    //Is affected by parent rotation
                    m = Matrix.RotationMatrix(worldState.Rotate);
                    mInv = Matrix.ReverseRotationMatrix(worldState.Rotate);

                    //No restrictions to apply
                    break;

                case BillboardFlags.Rotation:
                case BillboardFlags.RotationPerspective:

                    //Is not affected by parent rotation
                    m = Matrix.RotationMatrix(_frameState.Rotate);
                    mInv = Matrix.ReverseRotationMatrix(_frameState.Rotate);

                    //TODO: apply restrictions?
                    break;

                case BillboardFlags.Y:
                case BillboardFlags.YPerspective:

                    //Is affected by parent rotation
                    m = Matrix.RotationMatrix(worldState.Rotate);
                    mInv = Matrix.ReverseRotationMatrix(worldState.Rotate);

                    //Only Y is allowed to rotate automatically
                    rot._x = 0;
                    rot._z = 0;

                    break;

                default: //Not a valid billboard type
                    return;
            }

            worldState.Rotate = rot;

            _frameMatrix = worldState._transform * m;
            _inverseFrameMatrix = worldState._iTransform * mInv;
        }

        public void DrawBox(bool drawChildren, bool bindBox)
        {
            Box box = bindBox ? _extents : GetBox();

            if (bindBox)
            {
                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                fixed (Matrix* m = &_frameMatrix)
                {
                    GL.MultMatrix((float*) m);
                }
            }

            TKContext.DrawWireframeBox(box);

            if (bindBox)
            {
                GL.PopMatrix();
            }

            if (drawChildren)
            {
                foreach (MDL0BoneNode b in Children)
                {
                    b.DrawBox(true, bindBox);
                }
            }
        }

        public Box GetBox()
        {
            if (_visDrawCalls.Count == 0)
            {
                return new Box();
            }

            Box box = Box.ExpandableVolume;
            foreach (DrawCall o in _visDrawCalls)
            {
                box.ExpandVolume(o._parentObject.GetBox());
            }

            return box;
        }

        internal void SetBox()
        {
            _extents = GetBox();
            foreach (MDL0BoneNode b in Children)
            {
                b.SetBox();
            }
        }

        public List<MDL0BoneNode> ChildTree(List<MDL0BoneNode> list)
        {
            list.Add(this);
            foreach (MDL0BoneNode c in _children)
            {
                c.ChildTree(list);
            }

            return list;
        }

        /// <summary>
        /// Find the MD5 checksum of this node's data.
        /// Basically, don't check string offset and instead hash the name itself
        /// </summary>
        public override byte[] MD5()
        {
            if (WorkingUncompressed.Address == null || WorkingUncompressed.Length == 0)
            {
                // skip fix. This should probably never happen but whatever
                return base.MD5();
            }

            int size = WorkingUncompressed.Length + Name.UTF8Length();
            byte[] data = new byte[size];
            fixed (byte* ptr = data)
            {
                // Write the initial data
                Memory.Move(ptr, WorkingUncompressed.Address, (uint)WorkingUncompressed.Length);
                // Write the name string
                ((VoidPtr) ptr).WriteUTF8String(Name, false, (uint)WorkingUncompressed.Length);
            }
            // 0 out the name offset
            data[0x8] = data[0x9] = data[0xA] = data[0xB] = 0;

            return MD5Provider.ComputeHash(data);
        }

        [Browsable(false)] public List<Influence> LinkedInfluences => _linkedInfluences;

        private readonly List<Influence> _linkedInfluences = new List<Influence>();

        #region Rendering

        public static Color DefaultLineColor = Color.FromArgb(255, 0, 0, 128);
        public static Color DefaultLineDeselectedColor = Color.FromArgb(115, 128, 0, 0);
        public static Color DefaultNodeColor = Color.FromArgb(0, 128, 0);

        public Color _boneColor = Color.Transparent;
        public Color _nodeColor = Color.Transparent;

        public const float _nodeRadius = 0.20f;

        internal void ApplyCHR0(CHR0Node node, float index)
        {
            CHR0EntryNode e;

            _frameState = _bindState;

            if (node != null && index >= 1 && (e = node.FindChild(Name, false) as CHR0EntryNode) != null
            ) //Set to anim pose
            {
                fixed (FrameState* v = &_frameState)
                {
                    float* f = (float*) v;
                    for (int i = 0; i < 9; i++)
                    {
                        if (e.Keyframes[i]._keyCount > 0)
                        {
                            f[i] = e.GetFrameValue(i, index - 1);
                        }
                    }

                    _frameState.CalcTransforms();
                }
            }

            foreach (MDL0BoneNode b in Children)
            {
                b.ApplyCHR0(node, index);
            }
        }

        internal override void Bind()
        {
            _render = true;
            _boneColor = Color.Transparent;
            _nodeColor = Color.Transparent;
        }

        [Browsable(false)]
        public bool IsRendering
        {
            get => _render;
            set => _render = value;
        }

        public bool _render = true;

        public void Render(bool targetModel, ModelPanelViewport viewport, Vector3 parentPos = new Vector3())
        {
            if (!_render)
            {
                return;
            }

            //Draw name if selected
            if (_nodeColor != Color.Transparent && viewport != null)
            {
                Vector3 screenPos = viewport.Camera.Project(_frameMatrix.GetPoint());
                viewport.SettingsScreenText[Name] = new Vector3(screenPos._x, screenPos._y - 9.0f, screenPos._z);
            }

            float alpha = targetModel ? 1.0f : 0.45f;

            //Set bone line color
            if (_boneColor != Color.Transparent)
            {
                GL.Color4(_boneColor.R / 255.0f, _boneColor.G / 255.0f, _boneColor.B / 255.0f, alpha);
            }
            else
            {
                GL.Color4(targetModel ? DefaultLineColor : DefaultLineDeselectedColor);
            }

            //Draw bone line
            Vector3 currentPos = _frameMatrix.GetPoint();
            GL.Begin(BeginMode.Lines);
            GL.Vertex3((float*) &parentPos);
            GL.Vertex3((float*) &currentPos);
            GL.End();

            //Set bone orb color
            if (_nodeColor != Color.Transparent)
            {
                GL.Color4(_nodeColor.R / 255.0f, _nodeColor.G / 255.0f, _nodeColor.B / 255.0f, alpha);
            }
            else
            {
                GL.Color4(DefaultNodeColor.R / 255.0f, DefaultNodeColor.G / 255.0f, DefaultNodeColor.B / 255.0f, alpha);
            }

            //Draw bone orb
            GL.PushMatrix();

            bool ignoreBoneScale = true;
            bool scaleBones = viewport != null && viewport._renderAttrib._scaleBones;
            Matrix transform = _frameMatrix;
            if (ignoreBoneScale)
            {
                transform = Matrix.TranslationMatrix(currentPos) *
                            _frameMatrix.GetRotationMatrix() *
                            Matrix.ScaleMatrix(new Vector3(1.0f));
            }

            if (viewport._renderAttrib._renderBonesAsPoints)
            {
                GL.MultMatrix((float*) &transform);

                if (!scaleBones)
                {
                    GL.PointSize(1.0f / ModelEditorBase.OrbRadius(this, viewport.Camera) * 10.0f);
                }
                else
                {
                    GL.PointSize(10.0f);
                }

                GL.Enable(EnableCap.PointSmooth);
                GL.Begin(BeginMode.Points);
                GL.Vertex3(0, 0, 0);
                GL.End();
            }
            else
            {
                if (scaleBones)
                {
                    transform.Scale(new Vector3(ModelEditorBase.OrbRadius(this, viewport.Camera)));
                }

                GL.MultMatrix((float*) &transform);

                //Orb
                TKContext.FindOrCreate("BoneNodeOrb", CreateNodeOrb).Call();

                //Axes
                DrawNodeOrients(alpha);
            }

            GL.PopMatrix();

            //Render children
            foreach (MDL0BoneNode n in Children)
            {
                n.Render(targetModel, viewport, currentPos);
            }
        }

        public static GLDisplayList CreateNodeOrb()
        {
            GLDisplayList circle = TKContext.GetRingList();
            GLDisplayList orb = new GLDisplayList();

            orb.Begin();
            GL.PushMatrix();

            GL.Scale(_nodeRadius, _nodeRadius, _nodeRadius);
            circle.Call();
            GL.Rotate(90.0f, 0.0f, 1.0f, 0.0f);
            circle.Call();
            GL.Rotate(90.0f, 1.0f, 0.0f, 0.0f);
            circle.Call();

            GL.PopMatrix();
            orb.End();
            return orb;
        }

        public static void DrawNodeOrients(float alpha = 1.0f)
        {
            GL.Begin(BeginMode.Lines);

            GL.Color4(1.0f, 0.0f, 0.0f, alpha);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(_nodeRadius * 2, 0.0f, 0.0f);

            GL.Color4(0.0f, 1.0f, 0.0f, alpha);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, _nodeRadius * 2, 0.0f);

            GL.Color4(0.0f, 0.0f, 1.0f, alpha);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, _nodeRadius * 2);

            GL.End();
        }

        #endregion
    }
}