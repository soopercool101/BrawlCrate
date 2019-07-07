using BrawlLib.OpenGL;
using BrawlLib.SSBBTypes;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class CollisionNode : ARCEntryNode, IRenderedObject
    {
        internal CollisionHeader* Header => (CollisionHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.CollisionDef;
        public override Type[] AllowedChildTypes => new Type[] {typeof(CollisionObject)};

        [Browsable(false)]
        public bool IsRendering
        {
            get => _render;
            set => _render = value;
        }

        private bool _render = true;

        internal int _unk1;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _unk1 = Header->_unk1;

            return Header->_numObjects > 0;
        }

        public override void OnPopulate()
        {
            ColObject* obj = Header->Objects;
            for (int i = Header->_numObjects; i-- > 0;)
            {
                new CollisionObject().Initialize(this, new DataSource(obj++, ColObject.Size));
            }
        }

        protected override string GetName()
        {
            return base.GetName("Collision Data");
        }

        private int _pointCount, _planeCount;

        public override int OnCalculateSize(bool force)
        {
            _pointCount = _planeCount = 0;
            foreach (CollisionObject obj in Children)
            {
                _pointCount += obj._points.Count;
                _planeCount += obj._planes.Count;
            }

            return CollisionHeader.Size + _pointCount * 8 + _planeCount * ColPlane.Size +
                   Children.Count * ColObject.Size;
        }

        public void CalculateCamBoundaries(out float? minX, out float? minY, out float? maxX, out float? maxY)
        {
            minX = null;
            minY = null;
            maxX = null;
            maxY = null;
            foreach (CollisionObject o in Children)
            {
                foreach (CollisionLink l in o._points)
                {
                    if (minX == null || l.Value._x < minX)
                    {
                        minX = l.Value._x;
                    }

                    if (minY == null || l.Value._y < minY)
                    {
                        minY = l.Value._y;
                    }

                    if (maxX == null || l.Value._x > maxX)
                    {
                        maxX = l.Value._x;
                    }

                    if (maxY == null || l.Value._y > maxY)
                    {
                        maxY = l.Value._y;
                    }
                }
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            CollisionHeader* header = (CollisionHeader*) address;
            *header = new CollisionHeader(_pointCount, _planeCount, Children.Count, _unk1);

            BVec2* pPoint = header->Points;
            ColPlane* pPlane = header->Planes;
            ColObject* pObj = header->Objects;

            int iPoint = 0, iPlane = 0;
            int lind, rind, llink, rlink, tmp;
            int cPoint, cPlane;

            lind = 0;
            rind = 0;

            CollisionPlane current, next;
            CollisionLink link;
            foreach (CollisionObject obj in Children)
            {
                //Sets bounds and entry indices
                obj.Prepare();

                cPoint = iPoint;
                cPlane = iPlane;
                foreach (CollisionPlane plane in obj._planes)
                {
                    if (plane._encodeIndex != -1)
                    {
                        continue;
                    }

                    current = next = plane;

                    Top:
                    //Update variables, moving to next plane and links
                    llink = current._encodeIndex;
                    current = next;
                    next = null;
                    rlink = -1;

                    //Get left point index, and encode where necessary
                    if ((link = current._linkLeft)._encodeIndex == -1)
                    {
                        pPoint[link._encodeIndex = lind = iPoint++] = link._rawValue;
                    }
                    else
                    {
                        lind = link._encodeIndex;
                    }

                    //Get right point index and encode. 
                    if ((link = current._linkRight)._encodeIndex == -1)
                    {
                        pPoint[link._encodeIndex = rind = iPoint++] = link._rawValue;
                    }
                    else
                    {
                        rind = link._encodeIndex;
                    }

                    //Right-link planes by finding next available
                    if (link != null)
                    {
                        foreach (CollisionPlane p in link._members)
                        {
                            if (p == current || p._linkLeft != link)
                            {
                                continue; //We only want to go left-to-right!
                            }

                            //Determine if entry has been encoded yet
                            if ((tmp = p._encodeIndex) != -1)
                            {
                                if (pPlane[tmp]._link1 != -1)
                                {
                                    continue; //Already linked, try again
                                }
                                else
                                {
                                    pPlane[rlink = tmp]._link1 = (short) iPlane; //Left link, which means the end!
                                }
                            }
                            else
                            {
                                next = p;
                                rlink = iPlane + 1;
                            }

                            break;
                        }
                    }

                    //Create entry
                    pPlane[current._encodeIndex = iPlane++] = new ColPlane(lind, rind, llink, rlink, current._type,
                        current._flags2, current._flags, current._material);

                    //Traverse
                    if (next != null)
                    {
                        goto Top;
                    }
                }

                *pObj++ = new ColObject(cPlane, iPlane - cPlane, cPoint, iPoint - cPoint, obj._boxMin, obj._boxMax,
                    obj._modelName, obj._boneName,
                    obj._unk1, obj._unk2, obj._unk3, obj._flags, obj._unk5, obj._unk6, obj._boneIndex);
            }
        }

        #region Rendering

        public event EventHandler DrawCallsChanged;

        public List<DrawCallBase> DrawCalls
        {
            get
            {
                List<DrawCallBase> calls = new List<DrawCallBase>();
                foreach (CollisionObject o in Children)
                {
                    foreach (DrawCallBase d in o.DrawCalls)
                    {
                        calls.Add(d);
                    }
                }

                return calls;
            }
        }

        public bool Attached { get; }

        public void Attach()
        {

        }

        public void Detach()
        {

        }
        public void Refresh()
        {

        }
        public void PreRender(ModelPanelViewport v)
        {
            foreach (CollisionObject o in Children)
            {
                o._render = _render;
            }
        }

        public void Render()
        {
            GL.Disable(EnableCap.Lighting);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);

            foreach (CollisionObject obj in Children)
            {
                obj.Render();
            }
        }

        public Box GetBox()
        {
            Box box = Box.ExpandableVolume;
            foreach (CollisionObject obj in Children)
            {
                box.ExpandVolume(obj.GetBox());
            }

            return box;
        }
        
        #endregion

        internal static ResourceNode TryParse(DataSource source)
        {
            CollisionHeader* header = (CollisionHeader*) source.Address;

            if (header->_numPoints < 0 || header->_numPlanes < 0 || header->_numObjects < 0 || header->_unk1 < 0)
            {
                return null;
            }

            if (header->_pointOffset != 0x28 ||
                header->_planeOffset >= source.Length || header->_planeOffset <= header->_pointOffset ||
                header->_objectOffset >= source.Length || header->_objectOffset <= header->_planeOffset)
            {
                return null;
            }

            int* sPtr = header->_pad;
            for (int i = 0; i < 5; i++)
            {
                if (sPtr[i] != 0)
                {
                    return null;
                }
            }

            return new CollisionNode();
        }

        public void MergeWith()
        {
            CollisionNode external = null;
            OpenFileDialog o = new OpenFileDialog
            {
                Filter = FileFilters.CollisionDef,
                Title = "Please select a collision to merge with"
            };
            if (o.ShowDialog() == DialogResult.OK)
            {
                if ((external = (CollisionNode) NodeFactory.FromFile(null, o.FileName)) != null)
                {
                    MergeWith(external);
                }
            }
        }

        public void MergeWith(CollisionNode external)
        {
            foreach (CollisionObject co in external.Children)
            {
                AddChild(co);
            }

            SignalPropertyChange();
        }
    }

    public unsafe class CollisionObject : ResourceNode, IRenderedObject
    {
        [Browsable(false)]
        public MDL0BoneNode LinkedBone
        {
            get => _linkedBone;
            set
            {
                if ((_linkedBone = value) != null)
                {
                    _boneIndex = _linkedBone.BoneIndex;
                    _boneName = _linkedBone.Name;
                    _modelName = _linkedBone.Model.Name;
                    _flags[1] = false;
                }
                else
                {
                    _boneIndex = -1;
                    _boneName = "";
                    _modelName = "";
                    _flags[1] = true;
                }
            }
        }

        public MDL0BoneNode _linkedBone = null;

        public Vector2 _boxMin, _boxMax;
        public int _unk1, _unk2, _unk3, _unk5, _unk6, _boneIndex;
        public Bin16 _flags;

        public List<CollisionPlane> _planes = new List<CollisionPlane>();
        public bool _render = true;
        public string _modelName = "", _boneName = "";

        public enum Flags
        {
            None = 0,
            Unknown = 1,
            Independent = 2,
            ModuleControlled = 4,
            SSEUnknown = 8,
        }

        public List<CollisionLink> _points = new List<CollisionLink>();

        public CollisionObject()
        {
        }

        internal ColObject* Header => (ColObject*) WorkingUncompressed.Address;

        public override bool OnInitialize()
        {
            CollisionNode parentColl = Parent as CollisionNode;
            _modelName = Header->ModelName;
            _boneName = Header->BoneName;
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;
            _unk3 = Header->_unk3;
            _flags = (ushort) Header->_flags;
            _unk5 = Header->_unk5;
            _unk6 = Header->_unk6;
            _boneIndex = Header->_boneIndex;
            _boxMax = Header->_boxMax;
            _boxMin = Header->_boxMin;

            int pointCount = Header->_pointCount;
            int pointOffset = Header->_pointOffset;
            int planeCount = Header->_planeCount;
            int planeOffset = Header->_planeIndex;

            ColPlane* pPlane = &parentColl.Header->Planes[planeOffset];

            //Decode points
            BVec2* pPtr = &parentColl.Header->Points[pointOffset];
            for (int i = 0; i < pointCount; i++)
            {
                new CollisionLink(this, *pPtr++);
            }

            //CollisionPlane plane;
            for (int i = 0; i < planeCount; i++)
            {
                if (pPlane->_point1 != pPlane->_point2)
                {
                    new CollisionPlane(this, pPlane++, pointOffset);
                }
            }

            _name = $"Collision Object [{Parent.Children.Count}]";

            return false;
        }

        //Calculate bounds, and reset indices
        internal void Prepare()
        {
            Vector2 left, right;
            _boxMin = new Vector2(0);
            _boxMax = new Vector2(0);

            foreach (CollisionPlane plane in _planes)
            {
                //Normalize plane direction

                //Get bounds
                left = plane.PointLeft;
                right = plane.PointRight;

                _boxMin.Min(left);
                _boxMin.Min(right);
                _boxMax.Max(left);
                _boxMax.Max(right);

                //Reset encode indices
                plane._encodeIndex = -1;
                if (plane._linkLeft != null)
                {
                    plane._linkLeft._encodeIndex = -1;
                }

                if (plane._linkRight != null)
                {
                    plane._linkRight._encodeIndex = -1;
                }
            }
        }

        #region Rendering

        public event EventHandler DrawCallsChanged;

        public List<DrawCallBase> DrawCalls
        {
            get
            {
                List<DrawCallBase> calls = new List<DrawCallBase>();
                foreach (CollisionPlane p in _planes)
                {
                    calls.Add(p);
                }

                foreach (CollisionLink l in _points)
                {
                    calls.Add(l);
                }

                return calls;
            }
        }

        public bool IsRendering { get; set; }
        public bool Attached { get; }

        public void Attach()
        {

        }

        public void Detach()
        {

        }
        public void Refresh()
        {

        }
        public void PreRender(ModelPanelViewport v)
        {
            foreach (CollisionPlane p in _planes)
            {
                p._render = _render;
            }

            GL.PushAttrib(AttribMask.AllAttribBits);

            GL.Disable(EnableCap.DepthTest);
        }

        public Box GetBox()
        {
            Box box = Box.ExpandableVolume;
            foreach (CollisionLink link in _points)
            {
                box.ExpandVolume(new Vector3(link.Value._x, link.Value._y, 0));
            }
            return box;
        }

        // Depreciating
        internal unsafe void Render()
        {
            if (!_render)
            {
                return;
            }

            foreach (CollisionPlane p in _planes)
            {
                p.Render(null);
            }

            foreach (CollisionLink l in _points)
            {
                l.Render(null);
            }
        }

        #endregion


        public override unsafe void Export(string outPath)
        {
            if (outPath.EndsWith(".coll", StringComparison.OrdinalIgnoreCase))
            {
                CollisionNode node = new CollisionNode();
                node.Children.Add(this);
                node.Export(outPath);
                return;
            }

            base.Export(outPath);
        }
    }

    public unsafe class CollisionLink : DrawCallBase
    {
        private const float BoxRadius = 0.15f;
        private const float LineWidth = 11.0f;

        public CollisionObject _parent;
        public int _encodeIndex;
        public bool _highlight;

        public Vector2 _rawValue;

        public Vector2 Value
        {
            get
            {
                if (_parent == null || _parent.LinkedBone == null)
                {
                    return _rawValue;
                }

                return _parent.LinkedBone.Matrix * _rawValue;
            }
            set
            {
                if (_parent == null || _parent.LinkedBone == null)
                {
                    _rawValue = value;
                    return;
                }

                _rawValue = _parent.LinkedBone.InverseMatrix * value;
            }
        }

        public List<CollisionPlane> _members = new List<CollisionPlane>();

        public CollisionLink()
        {
        }

        public CollisionLink(CollisionObject parent, Vector2 value)
        {
            _parent = parent;
            Value = value;
            _parent._points.Add(this);
        }

        public CollisionLink Clone()
        {
            return new CollisionLink(_parent, _rawValue);
        }

        //internal CollisionLink Splinter()
        //{
        //    if (_members.Count <= 1)
        //        return null;

        //    CollisionPlane plane = _members[1];
        //    CollisionLink link = new CollisionLink(_parent, _value);

        //    if (plane._linkLeft == this)
        //        plane.LinkLeft = link;
        //    else
        //        plane.LinkRight = link;


        //    return link;
        //}

        public CollisionLink[] Split()
        {
            int count = _members.Count - 1;
            CollisionLink[] links = new CollisionLink[count];
            for (int i = 0; i < count; i++)
            {
                if (_members[0]._linkLeft == this)
                {
                    _members[0].LinkLeft = links[i] = Clone();
                }
                else
                {
                    _members[0].LinkRight = links[i] = Clone();
                }
            }

            return links;
        }

        public bool Merge(CollisionLink link)
        {
            if (_parent != link._parent)
            {
                return false;
            }

            CollisionPlane plane;
            for (int i = link._members.Count; --i >= 0;)
            {
                plane = link._members[0];

                if (plane._linkLeft == link)
                {
                    if (plane._linkRight == this)
                    {
                        plane.Delete();
                    }
                    else
                    {
                        plane.LinkLeft = this;
                    }
                }
                else
                {
                    if (plane._linkLeft == this)
                    {
                        plane.Delete();
                    }
                    else
                    {
                        plane.LinkRight = this;
                    }
                }
            }

            //_value = (_value + link._value) / 2.0f;

            return true;
        }

        //Connects two points together to create a plane
        public unsafe CollisionPlane Connect(CollisionLink p)
        {
            //Don't connect if not on same object
            if (p == this || _parent != p._parent)
            {
                return null;
            }

            //Don't connect if plane already exists
            foreach (CollisionPlane plane in _members)
            {
                if (p._members.Contains(plane))
                {
                    return null;
                }
            }

            return new CollisionPlane(_parent, this, p);
        }

        //Create new point/plane extending to target
        public CollisionLink Branch(Vector2 point)
        {
            CollisionLink link = new CollisionLink(_parent, point);
            CollisionPlane plane = new CollisionPlane(_parent, this, link);
            return link;
        }

        //Deletes link and all planes connected
        public void Delete()
        {
            while (_members.Count != 0)
            {
                _members[0].Delete();
            }
        }

        //Deletes link but re-links existing planes
        public void Pop()
        {
            CollisionLink link1, link2;
            CollisionPlane plane1, plane2;
            while (_members.Count != 0)
            {
                plane1 = _members[0];
                if (_members.Count > 1)
                {
                    plane2 = _members[1];
                    link1 = plane1._linkLeft == this ? plane1._linkRight : plane1._linkLeft;
                    link2 = plane2._linkLeft == this ? plane2._linkRight : plane2._linkLeft;

                    link1.Connect(link2);
                    plane1.Delete();
                    plane2.Delete();
                }
                else
                {
                    plane1.Delete();
                }
            }
        }

        public void RemoveMember(CollisionPlane plane)
        {
            _members.Remove(plane);
            if (_members.Count == 0)
            {
                _parent._points.Remove(this);
            }
        }

        public override void Render(ModelPanelViewport v)
        {
            if (!_render)
            {
                return;
            }
            Color4 clr = new Color4(1.0f, 1.0f, 1.0f, 1.0f);
            float mult = 1.0f;
            foreach (CollisionPlane p in _members)
            {
                if (p.LinkLeft == this && p.IsLeftLedge || p.LinkRight == this && p.IsRightLedge)
                {
                    clr = new Color4(1.0f, 0.0f, 1.0f, 1.0f);
                    mult = 3.0f;
                    break;
                }
            }

            if (_highlight)
            {
                GL.Color4(1.0f, 1.0f, 0.0f, 1.0f);
            }
            else
            {
                GL.Color4(clr);
            }

            Vector2 vec = Value;

            GL.Disable(EnableCap.CullFace);
            TKContext.DrawBox(
                new Vector3(vec._x - mult * BoxRadius, vec._y - mult * BoxRadius, LineWidth),
                new Vector3(vec._x + mult * BoxRadius, vec._y + mult * BoxRadius, -LineWidth));
            GL.Enable(EnableCap.CullFace);
        }
    }

    public unsafe class CollisionPlane : DrawCallBase
    {
        public int _encodeIndex;

        public CollisionLink _linkLeft, _linkRight;

        public byte _material;
        public CollisionPlaneFlags _flags;
        public CollisionPlaneType _type;
        public CollisionPlaneFlags2 _flags2;

        public CollisionObject _parent;

        public CollisionPlaneType Type
        {
            get => _type;
            set
            {
                switch (_type = value)
                {
                    case CollisionPlaneType.Floor:
                        if (PointLeft._x > PointRight._x && !IsRotating)
                        {
                            SwapLinks();
                        }

                        break;

                    case CollisionPlaneType.Ceiling:
                        if (PointLeft._x < PointRight._x && !IsRotating)
                        {
                            SwapLinks();
                        }

                        break;

                    case CollisionPlaneType.RightWall:
                        if (PointLeft._y < PointRight._y && !IsRotating)
                        {
                            SwapLinks();
                        }

                        break;

                    case CollisionPlaneType.LeftWall:
                        if (PointLeft._y > PointRight._y && !IsRotating)
                        {
                            SwapLinks();
                        }

                        break;
                }
            }
        }

        public bool IsMultipleTypes => GetPlaneType() == null;

        public bool IsNone => _type == 0;

        public bool IsFloor
        {
            get => (_type & CollisionPlaneType.Floor) != 0;
            set => _type = (_type & ~CollisionPlaneType.Floor) | (value ? CollisionPlaneType.Floor : 0);
        }

        public bool IsCeiling
        {
            get => (_type & CollisionPlaneType.Ceiling) != 0;
            set => _type = (_type & ~CollisionPlaneType.Ceiling) | (value ? CollisionPlaneType.Ceiling : 0);
        }

        public bool IsLeftWall
        {
            get => (_type & CollisionPlaneType.LeftWall) != 0;
            set => _type = (_type & ~CollisionPlaneType.LeftWall) | (value ? CollisionPlaneType.LeftWall : 0);
        }

        public bool IsRightWall
        {
            get => (_type & CollisionPlaneType.RightWall) != 0;
            set => _type = (_type & ~CollisionPlaneType.RightWall) | (value ? CollisionPlaneType.RightWall : 0);
        }

        public bool IsWall => IsLeftWall || IsRightWall;

        public CollisionPlaneType? GetPlaneType()
        {
            switch (Type)
            {
                case CollisionPlaneType.Ceiling:
                case CollisionPlaneType.Floor:
                case CollisionPlaneType.LeftWall:
                case CollisionPlaneType.RightWall:
                case CollisionPlaneType.None:
                    return Type;
                default:
                    return null; // If multiple types
            }
        }

        public CollisionPlaneType? GetCurrentType()
        {
            if (!IsRotating || IsNone)
            {
                switch (Type)
                {
                    case CollisionPlaneType.Ceiling:
                    case CollisionPlaneType.Floor:
                    case CollisionPlaneType.LeftWall:
                    case CollisionPlaneType.RightWall:
                    case CollisionPlaneType.None:
                        return Type;
                    default:
                        return null; // If multiple types
                }
            }

            double angle = GetAngleDegrees();

            if (Math.Abs(angle) <= 45)
            {
                return CollisionPlaneType.Floor;
            }

            if (Math.Abs(angle) >= 135)
            {
                return CollisionPlaneType.Ceiling;
            }

            return angle < 0 ? CollisionPlaneType.RightWall : CollisionPlaneType.LeftWall;
        }

        public bool CollidableByCharacters => !IsNone && (IsCharacters || !IsPokemonTrainer && !IsItems);

        public bool IsCharacters
        {
            get => (_flags2 & CollisionPlaneFlags2.Characters) != 0;
            set => _flags2 = (_flags2 & ~CollisionPlaneFlags2.Characters) |
                             (value ? CollisionPlaneFlags2.Characters : 0);
        }

        public bool IsItems
        {
            get => (_flags2 & CollisionPlaneFlags2.Items) != 0;
            set => _flags2 = (_flags2 & ~CollisionPlaneFlags2.Items) | (value ? CollisionPlaneFlags2.Items : 0);
        }

        public bool IsPokemonTrainer
        {
            get => (_flags2 & CollisionPlaneFlags2.PokemonTrainer) != 0;
            set => _flags2 = (_flags2 & ~CollisionPlaneFlags2.PokemonTrainer) |
                             (value ? CollisionPlaneFlags2.PokemonTrainer : 0);
        }

        public bool IsUnknownSSE
        {
            get => (_flags2 & CollisionPlaneFlags2.UnknownSSE) != 0;
            set => _flags2 = (_flags2 & ~CollisionPlaneFlags2.UnknownSSE) |
                             (value ? CollisionPlaneFlags2.UnknownSSE : 0);
        }

        public bool IsFallThrough
        {
            get => (_flags & CollisionPlaneFlags.DropThrough) != 0;
            set => _flags = (_flags & ~CollisionPlaneFlags.DropThrough) | (value ? CollisionPlaneFlags.DropThrough : 0);
        }

        public bool IsRightLedge
        {
            get => (_flags & CollisionPlaneFlags.RightLedge) != 0;
            set => _flags = (_flags & ~CollisionPlaneFlags.RightLedge) | (value ? CollisionPlaneFlags.RightLedge : 0);
        }

        public bool IsLeftLedge
        {
            get => (_flags & CollisionPlaneFlags.LeftLedge) != 0;
            set => _flags = (_flags & ~CollisionPlaneFlags.LeftLedge) | (value ? CollisionPlaneFlags.LeftLedge : 0);
        }

        public bool IsNoWalljump
        {
            get => (_flags & CollisionPlaneFlags.NoWalljump) != 0;
            set => _flags = (_flags & ~CollisionPlaneFlags.NoWalljump) | (value ? CollisionPlaneFlags.NoWalljump : 0);
        }

        public bool IsUnknownFlag1
        {
            get => (_flags & CollisionPlaneFlags.Unknown1) != 0;
            set => _flags = (_flags & ~CollisionPlaneFlags.Unknown1) | (value ? CollisionPlaneFlags.Unknown1 : 0);
        }

        public bool IsRotating
        {
            get => (_flags & CollisionPlaneFlags.Rotating) != 0;
            set => _flags = (_flags & ~CollisionPlaneFlags.Rotating) | (value ? CollisionPlaneFlags.Rotating : 0);
        }

        public bool IsUnknownFlag3
        {
            get => (_flags & CollisionPlaneFlags.Unknown3) != 0;
            set => _flags = (_flags & ~CollisionPlaneFlags.Unknown3) | (value ? CollisionPlaneFlags.Unknown3 : 0);
        }

        public bool IsUnknownFlag4
        {
            get => (_flags & CollisionPlaneFlags.Unknown4) != 0;
            set => _flags = (_flags & ~CollisionPlaneFlags.Unknown4) | (value ? CollisionPlaneFlags.Unknown4 : 0);
        }

        public bool HasUnknownFlag
        {
            get => (_flags & CollisionPlaneFlags.Unknown1) != 0 || (_flags & CollisionPlaneFlags.Unknown3) != 0 ||
                   (_flags & CollisionPlaneFlags.Unknown4) != 0;
            set { }
        }

        public double GetAngleRadians()
        {
            float xDiff = _linkRight.Value._x - _linkLeft.Value._x;
            float yDiff = _linkRight.Value._y - _linkLeft.Value._y;
            if (xDiff == 0.0f)
            {
                return yDiff > 0 ? Math.PI / 2 : -Math.PI / 2;
            }

            if (yDiff == 0.0f)
            {
                return xDiff > 0 ? 0 : Math.PI;
            }

            return Math.Atan2(yDiff, xDiff);
        }

        public double GetAngleDegrees()
        {
            return GetAngleRadians() * Maths._rad2deg;
        }

        public Vector2 PointLeft => _linkLeft.Value;
        public Vector2 PointRight => _linkRight.Value;

        public CollisionLink LinkLeft
        {
            get => _linkLeft;
            set
            {
                _linkLeft?.RemoveMember(this);

                if ((_linkLeft = value) != null)
                {
                    if (_linkLeft != _linkRight)
                    {
                        _linkLeft._members.Add(this);
                    }
                    else
                    {
                        _linkLeft = null;
                    }
                }
            }
        }

        public CollisionLink LinkRight
        {
            get => _linkRight;
            set
            {
                _linkRight?.RemoveMember(this);

                if ((_linkRight = value) != null)
                {
                    if (_linkRight != _linkLeft)
                    {
                        _linkRight._members.Add(this);
                    }
                    else
                    {
                        _linkRight = null;
                    }
                }
            }
        }

        public CollisionPlane(CollisionObject parent, CollisionLink left, CollisionLink right)
        {
            _parent = parent;
            _parent._planes.Add(this);

            LinkLeft = left;
            LinkRight = right;

            _render = true;
        }

        public CollisionPlane(CollisionObject parent, ColPlane* entry, int offset)
            : this(parent, parent._points[entry->_point1 - offset], parent._points[entry->_point2 - offset])
        {
            _material = entry->_material;
            _flags = entry->_flags;
            _type = entry->Type;
            _flags2 = entry->Flags2;

            _render = true;
        }

        public CollisionLink Split(Vector2 point)
        {
            CollisionLink link = new CollisionLink(_parent, point);
            CollisionPlane plane = new CollisionPlane(_parent, link, _linkRight)
            {
                _material = _material,
                _flags = _flags,
                _flags2 = _flags2,
                _type = _type
            };
            if (IsRightLedge)
            {
                IsRightLedge = false;
            }

            if (IsLeftLedge)
            {
                plane.IsLeftLedge = false;
            }

            LinkRight = link;
            return link;
        }

        public void SwapLinks()
        {
            CollisionLink l = _linkLeft;
            _linkLeft = _linkRight;
            _linkRight = l;
        }

        public void Delete()
        {
            LinkLeft = null;
            LinkRight = null;
            _parent._planes.Remove(this);
        }

        #region Rendering

        public override void Render(ModelPanelViewport viewport)
        {
            if (!_render)
            {
                return;
            }

            float alpha = 0.8f;

            if (!IsCharacters && (IsItems || IsPokemonTrainer))
            {
                alpha = 0.5f;
            }

            Vector2 l = _linkLeft.Value;
            Vector2 r = _linkRight.Value;

            int lev = 0;
            if (_linkLeft._highlight)
            {
                lev++;
            }

            if (_linkRight._highlight)
            {
                lev++;
            }

            if (lev == 1)
            {
                GL.Color4(1.0f, 0.5f, 0.5f, alpha);
            }
            else if (lev != 0)
            {
                GL.Color4(0.9f, 0.0f, 0.9f, alpha);
            }
            else if (!IsFallThrough)
            {
                switch (GetCurrentType())
                {
                    case CollisionPlaneType.None:
                        GL.Color4(1.0f, 1.0f, 1.0f, alpha);
                        break;
                    case CollisionPlaneType.Floor:
                        GL.Color4(0.0f, 0.9f, 0.9f, alpha);
                        break;
                    case CollisionPlaneType.Ceiling:
                        GL.Color4(0.9f, 0.0f, 0.0f, alpha);
                        break;
                    case CollisionPlaneType.LeftWall:
                    case CollisionPlaneType.RightWall:
                        GL.Color4(0.0f, 0.9f, 0.0f, alpha);
                        break;
                    default:
                        GL.Color4(0.0f, 0.0f, 0.0f, alpha);
                        break;
                }
            }
            else
            {
                switch (GetCurrentType())
                {
                    case CollisionPlaneType.None:
                        GL.Color4(0.65f, 0.65f, 0.35f, alpha);
                        break;
                    case CollisionPlaneType.Floor:
                        GL.Color4(1.0f, 1.0f, 0.0f, alpha);
                        break;
                    case CollisionPlaneType.Ceiling:
                        GL.Color4(0.9f, 0.3f, 0.0f, alpha);
                        break;
                    case CollisionPlaneType.LeftWall:
                    case CollisionPlaneType.RightWall:
                        GL.Color4(0.45f, 1.0f, 0.0f, alpha);
                        break;
                    default:
                        GL.Color4(0.5f, 0.5f, 0.0f, alpha);
                        break;
                }
            }

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(l._x, l._y, 10.0f);
            GL.Vertex3(l._x, l._y, -10.0f);
            GL.Vertex3(r._x, r._y, -10.0f);
            GL.Vertex3(r._x, r._y, 10.0f);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(l._x, l._y, 10.0f);
            GL.Vertex3(r._x, r._y, 10.0f);
            GL.Vertex3(l._x, l._y, -10.0f);
            GL.Vertex3(r._x, r._y, -10.0f);
            GL.End();
        }

        #endregion
    }
}