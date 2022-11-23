using BrawlLib.Internal;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.Types;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class CollisionNode : ARCEntryNode
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

                                pPlane[rlink = tmp]._link1 = (short) iPlane; //Left link, which means the end!
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
                        current._typeFlags, current._materialFlags, current._material);

                    //Traverse
                    if (next != null)
                    {
                        goto Top;
                    }
                }

                *pObj++ = new ColObject(cPlane, iPlane - cPlane, cPoint, iPoint - cPoint, obj._boxMin, obj._boxMax,
                    obj._modelName, obj._boneName,
                    obj._unk1, obj._unk2, obj._unk3, (int) obj._flags, obj._unk5, obj._unk6, obj._boneIndex);
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
                foreach (CollisionPlane plane in obj._planes)
                {
                    box.ExpandVolume(new Vector3(plane.PointLeft._x, plane.PointLeft._y, 0));
                    box.ExpandVolume(new Vector3(plane.PointRight._x, plane.PointRight._y, 0));
                }
            }

            return box;
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
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
            OpenFileDialog o = new OpenFileDialog
            {
                Filter = FileFilters.CollisionDef,
                Multiselect = true,
                Title = "Please select a collision to merge with"
            };
            if (o.ShowDialog() == DialogResult.OK)
            {
                foreach (string f in o.FileNames)
                {
                    CollisionNode external = (CollisionNode)NodeFactory.FromFile(null, f, typeof(CollisionNode));
                    if (external != null)
                    {
                        MergeWith(external);
                    }
                }
            }
        }

        public void MergeWith(CollisionNode external)
        {
            foreach (ResourceNode co in external.Children)
            {
                co.Name = $"{external.Name} {co.Name}";
                AddChild(co);
            }

            SignalPropertyChange();
        }
    }

    public unsafe class CollisionObject : ResourceNode
    {
        public override ResourceType ResourceFileType => ResourceType.CollisionObj;

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
                    Independent = false;
                }
                else
                {
                    _boneIndex = -1;
                    _boneName = "";
                    _modelName = "";
                    Independent = true;
                }
            }
        }

        [Category("Collision Binding")]
        [DisplayName("Linked Model")]
        public string LinkModel
        {
            get => _modelName;
            set
            {
                _modelName = value;
                SignalPropertyChange();
            }
        }

        [Category("Collision Binding")]
        [DisplayName("Linked Bone Index")]
        public int LinkBoneIndex
        {
            get => _boneIndex;
            set
            {
                _boneIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("Collision Binding")]
        [DisplayName("Linked Bone")]
        public string LinkBone
        {
            get => _boneName;
            set
            {
                _boneName = value;
                SignalPropertyChange();
            }
        }

        [Category("Flags")]
        public bool UnknownFlag
        {
            get => (_flags & ColObjFlags.Unknown) != 0;
            set
            {
                _flags = (_flags & ~ColObjFlags.Unknown) | (value ? ColObjFlags.Unknown : 0);
                SignalPropertyChange();
            }
        }

        [Category("Flags")]
        [Description("Controls whether or not a collision will follow a linked bone")]
        public bool Independent
        {
            get => (_flags & ColObjFlags.Independent) != 0;
            set
            {
                _flags = (_flags & ~ColObjFlags.Independent) | (value ? ColObjFlags.Independent : 0);
                SignalPropertyChange();
            }
        }

        [Category("Flags")]
        public bool ModuleControlled
        {
            get => (_flags & ColObjFlags.ModuleControlled) != 0;
            set
            {
                _flags = (_flags & ~ColObjFlags.ModuleControlled) | (value ? ColObjFlags.ModuleControlled : 0);
                SignalPropertyChange();
            }
        }

        [Category("Flags")]
        public bool UnknownSSEFlag
        {
            get => (_flags & ColObjFlags.SSEUnknown) != 0;
            set
            {
                _flags = (_flags & ~ColObjFlags.SSEUnknown) | (value ? ColObjFlags.SSEUnknown : 0);
                SignalPropertyChange();
            }
        }

        public MDL0BoneNode _linkedBone;

        public Vector2 _boxMin, _boxMax;
        public int _unk1, _unk2, _unk3, _unk5, _unk6, _boneIndex;
        public ColObjFlags _flags;

        public List<CollisionPlane> _planes = new List<CollisionPlane>();
        public bool _render = true;
        public string _modelName = "", _boneName = "";

        public bool hasSingleLinkedCollisions;

        [Flags]
        public enum ColObjFlags : ushort
        {
            None = 0,
            Unknown = 1,
            Independent = 2,
            ModuleControlled = 4,
            SSEUnknown = 8
        }

        [Category("Collision Data")]
        public List<CollisionLink> Points => _points;

        public List<CollisionLink> _points = new List<CollisionLink>();

        public CollisionObject()
        {
        }

        internal ColObject* Header => (ColObject*) WorkingUncompressed.Address;

        public override bool OnInitialize()
        {
            CollisionNode parentColl = Parent as CollisionNode;
            _name = $"Collision Object [{Parent.Children.Count}]";
            _modelName = Header->ModelName;
            _boneName = Header->BoneName;
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;
            _unk3 = Header->_unk3;
            _flags = (ColObjFlags) (ushort) Header->_flags;
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
                new CollisionPlane(this, pPlane, pointOffset);
                if (pPlane->_point1 == pPlane->_point2)
                {
                    hasSingleLinkedCollisions = true;
                }

                ++pPlane;
            }

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

        internal void Render()
        {
            if (!_render)
            {
                return;
            }

            foreach (CollisionPlane p in _planes)
            {
                p.Render();
            }

            foreach (CollisionLink l in _points)
            {
                l.Render();
            }
        }

        public override void Export(string outPath)
        {
            if (outPath.EndsWith(".coll", StringComparison.OrdinalIgnoreCase))
            {
                CollisionNode node = new CollisionNode();
                node.Children.Add(this);
                node.SignalPropertyChange();
                node.Export(outPath);
                return;
            }

            base.Export(outPath);
        }

        // Utility functions

        public CollisionLink FindLink(CollisionLink l)
        {
            return FindLink(l._rawValue);
        }

        public CollisionLink FindLink(Vector2 position)
        {
            foreach (CollisionLink l in _points)
            {
                if (l.Value == position)
                {
                    return l;
                }
            }

            return null;
        }

        public void FixLedges()
        {
            foreach (CollisionPlane p in _planes.Where(o => o.IsFloor && o.IsLedge))
            {
                if (p.IsLeftLedge)
                {
                    foreach (CollisionPlane p2 in p.LinkLeft._members.Where(o => !o.Equals(p)))
                    {
                        if (!p2.IsLeftWall && !p2.IsCeiling)
                        {
                            p.IsLeftLedge = false;
                            break;
                        }
                    }
                }
                if (p.IsRightLedge)
                {
                    foreach (CollisionPlane p2 in p.LinkRight._members.Where(o => !o.Equals(p)))
                    {
                        if (!p2.IsRightWall && !p2.IsCeiling)
                        {
                            p.IsRightLedge = false;
                            break;
                        }
                    }
                }
            }
        }
    }

    public class CollisionLink
    {
        private const float BoxRadius = 0.15f;
        private const float LineWidth = 11.0f;

        public CollisionObject _parent;
        public int _encodeIndex;
        public bool _highlight;

        public Vector2 _rawValue;

        public override string ToString()
        {
            return $"{_rawValue}";
        }

        public Vector2 Value
        {
            get
            {
                if (_parent?.LinkedBone == null)
                {
                    return _rawValue;
                }

                return _parent.LinkedBone.Matrix * _rawValue;
            }
            set
            {
                if (_parent?.LinkedBone == null)
                {
                    _rawValue = value;
                    return;
                }

                _rawValue = _parent.LinkedBone.InverseMatrix * value;
            }
        }

        public List<CollisionPlane> _members = new List<CollisionPlane>();

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
        public CollisionPlane Connect(CollisionLink p)
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

        public void Render()
        {
            Color4 clr = new Color4(1.0f, 1.0f, 1.0f, 1.0f);
            float mult = 1.0f;
            float alpha = 1.0f;
            bool hasMultiple = false;
            bool hasLedge = false;
            foreach (CollisionPlane p in _members)
            {
                if (!hasLedge && (p.LinkLeft == this && p.IsLeftLedge || p.LinkRight == this && p.IsRightLedge))
                {
                    hasLedge = true;
                    if (!hasMultiple)
                    {
                        clr = new Color4(1.0f, 0.0f, 1.0f, 1.0f);
                    }

                    mult = 3.0f;
                    if (!_parent.hasSingleLinkedCollisions || hasMultiple)
                    {
                        break;
                    }
                }

                if (_parent.hasSingleLinkedCollisions && !hasMultiple && p.LinkLeft == p.LinkRight)
                {
                    hasMultiple = true;
                    if (!p.CollidableByCharacters)
                    {
                        alpha = 0.5f;
                    }

                    if (!p.IsFallThrough)
                    {
                        switch (p.Type)
                        {
                            case CollisionPlaneType.None:
                                clr = new Color4(1.0f, 1.0f, 1.0f, alpha);
                                break;
                            case CollisionPlaneType.Floor:
                                clr = new Color4(0.0f, 0.9f, 0.9f, alpha);
                                break;
                            case CollisionPlaneType.Ceiling:
                                clr = new Color4(0.9f, 0.0f, 0.0f, alpha);
                                break;
                            case CollisionPlaneType.LeftWall:
                            case CollisionPlaneType.RightWall:
                                clr = new Color4(0.0f, 0.9f, 0.0f, alpha);
                                break;
                            default:
                                clr = new Color4(0.0f, 0.0f, 0.0f, alpha);
                                break;
                        }
                    }
                    else
                    {
                        switch (p.Type)
                        {
                            case CollisionPlaneType.None:
                                clr = new Color4(0.65f, 0.65f, 0.35f, alpha);
                                break;
                            case CollisionPlaneType.Floor:
                                clr = new Color4(1.0f, 1.0f, 0.0f, alpha);
                                break;
                            case CollisionPlaneType.Ceiling:
                                clr = new Color4(0.9f, 0.3f, 0.0f, alpha);
                                break;
                            case CollisionPlaneType.LeftWall:
                            case CollisionPlaneType.RightWall:
                                clr = new Color4(0.45f, 1.0f, 0.0f, alpha);
                                break;
                            default:
                                clr = new Color4(0.5f, 0.5f, 0.0f, alpha);
                                break;
                        }
                    }

                    if (hasLedge)
                    {
                        break;
                    }
                }
            }

            if (_highlight)
            {
                if (hasMultiple)
                {
                    GL.Color4(0.9f, 0.0f, 0.9f, alpha);
                }
                else
                {
                    GL.Color4(1.0f, 1.0f, 0.0f, 1.0f);
                }
            }
            else
            {
                GL.Color4(clr);
            }

            Vector2 v = Value;

            GL.Disable(EnableCap.CullFace);
            TKContext.DrawBox(
                new Vector3(v._x - mult * BoxRadius, v._y - mult * BoxRadius, LineWidth),
                new Vector3(v._x + mult * BoxRadius, v._y + mult * BoxRadius, -LineWidth));
            GL.Enable(EnableCap.CullFace);
        }
    }

    public unsafe class CollisionPlane
    {
        public int _encodeIndex;

        public CollisionLink _linkLeft, _linkRight;

        public override string ToString()
        {
            return $"L: {_linkLeft} | R: {_linkRight}";
        }

        public byte _material;
        public CollisionPlaneMaterialFlags _materialFlags;
        public CollisionPlaneType _type;
        public CollisionPlaneTypeFlags _typeFlags;

        public bool _render = true;

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
                return GetPlaneType(); // Use the standard formula
            }

            // Check based on angle

            double angle = GetAngleDegrees();

            if (double.IsNaN(angle))
            {
                return GetPlaneType();
            }

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

        public bool CollidableByCharacters => IsCharacters || !IsPokemonTrainer && !IsItems;

        public bool IsCharacters
        {
            get => (_typeFlags & CollisionPlaneTypeFlags.Characters) != 0;
            set => _typeFlags = (_typeFlags & ~CollisionPlaneTypeFlags.Characters) |
                             (value ? CollisionPlaneTypeFlags.Characters : 0);
        }

        public bool IsItems
        {
            get => (_typeFlags & CollisionPlaneTypeFlags.Items) != 0;
            set => _typeFlags = (_typeFlags & ~CollisionPlaneTypeFlags.Items) | (value ? CollisionPlaneTypeFlags.Items : 0);
        }

        public bool IsPokemonTrainer
        {
            get => (_typeFlags & CollisionPlaneTypeFlags.PokemonTrainer) != 0;
            set => _typeFlags = (_typeFlags & ~CollisionPlaneTypeFlags.PokemonTrainer) |
                             (value ? CollisionPlaneTypeFlags.PokemonTrainer : 0);
        }
        public bool IsCrush
        {
            get => (_typeFlags & CollisionPlaneTypeFlags.Crush) != 0;
            set => _typeFlags = (_typeFlags & ~CollisionPlaneTypeFlags.Crush) |
                             (value ? CollisionPlaneTypeFlags.Crush : 0);
        }

        public bool IsBucculusBury
        {
            get => (_typeFlags & CollisionPlaneTypeFlags.Bucculus) != 0;
            set => _typeFlags = (_typeFlags & ~CollisionPlaneTypeFlags.Bucculus) |
                             (value ? CollisionPlaneTypeFlags.Bucculus : 0);
        }
        public bool IsTypeUnknown0x0200
        {
            get => (_typeFlags & CollisionPlaneTypeFlags.Unknown0x0200) != 0;
            set => _typeFlags = (_typeFlags & ~CollisionPlaneTypeFlags.Unknown0x0200) |
                             (value ? CollisionPlaneTypeFlags.Unknown0x0200 : 0);
        }
        public bool IsTypeUnknown0x0400
        {
            get => (_typeFlags & CollisionPlaneTypeFlags.Unknown0x0400) != 0;
            set => _typeFlags = (_typeFlags & ~CollisionPlaneTypeFlags.Unknown0x0400) |
                             (value ? CollisionPlaneTypeFlags.Unknown0x0400 : 0);
        }
        public bool IsTypeUnknown0x0800
        {
            get => (_typeFlags & CollisionPlaneTypeFlags.Unknown0x0800) != 0;
            set => _typeFlags = (_typeFlags & ~CollisionPlaneTypeFlags.Unknown0x0800) |
                             (value ? CollisionPlaneTypeFlags.Unknown0x0800 : 0);
        }
        public bool IsTypeUnknown0x1000
        {
            get => (_typeFlags & CollisionPlaneTypeFlags.Unknown0x1000) != 0;
            set => _typeFlags = (_typeFlags & ~CollisionPlaneTypeFlags.Unknown0x1000) |
                             (value ? CollisionPlaneTypeFlags.Unknown0x1000 : 0);
        }
        public bool IsTypeUnknown0x2000
        {
            get => (_typeFlags & CollisionPlaneTypeFlags.Unknown0x2000) != 0;
            set => _typeFlags = (_typeFlags & ~CollisionPlaneTypeFlags.Unknown0x2000) |
                             (value ? CollisionPlaneTypeFlags.Unknown0x2000 : 0);
        }
        public bool IsTypeUnknown0x4000
        {
            get => (_typeFlags & CollisionPlaneTypeFlags.Unknown0x4000) != 0;
            set => _typeFlags = (_typeFlags & ~CollisionPlaneTypeFlags.Unknown0x4000) |
                             (value ? CollisionPlaneTypeFlags.Unknown0x4000 : 0);
        }
        public bool IsTypeUnknown0x8000
        {
            get => (_typeFlags & CollisionPlaneTypeFlags.Unknown0x8000) != 0;
            set => _typeFlags = (_typeFlags & ~CollisionPlaneTypeFlags.Unknown0x8000) |
                             (value ? CollisionPlaneTypeFlags.Unknown0x8000 : 0);
        }

        public bool IsFallThrough
        {
            get => (_materialFlags & CollisionPlaneMaterialFlags.DropThrough) != 0;
            set => _materialFlags = (_materialFlags & ~CollisionPlaneMaterialFlags.DropThrough) | (value ? CollisionPlaneMaterialFlags.DropThrough : 0);
        }

        public bool IsLedge => IsRightLedge || IsLeftLedge;

        public bool IsRightLedge
        {
            get => (_materialFlags & CollisionPlaneMaterialFlags.RightLedge) != 0;
            set => _materialFlags = (_materialFlags & ~CollisionPlaneMaterialFlags.RightLedge) | (value ? CollisionPlaneMaterialFlags.RightLedge : 0);
        }

        public bool IsLeftLedge
        {
            get => (_materialFlags & CollisionPlaneMaterialFlags.LeftLedge) != 0;
            set => _materialFlags = (_materialFlags & ~CollisionPlaneMaterialFlags.LeftLedge) | (value ? CollisionPlaneMaterialFlags.LeftLedge : 0);
        }

        public bool IsNoWalljump
        {
            get => (_materialFlags & CollisionPlaneMaterialFlags.NoWalljump) != 0;
            set => _materialFlags = (_materialFlags & ~CollisionPlaneMaterialFlags.NoWalljump) | (value ? CollisionPlaneMaterialFlags.NoWalljump : 0);
        }

        public bool IsMatUnknown0x02
        {
            get => (_materialFlags & CollisionPlaneMaterialFlags.Unknown0x02) != 0;
            set => _materialFlags = (_materialFlags & ~CollisionPlaneMaterialFlags.Unknown0x02) | (value ? CollisionPlaneMaterialFlags.Unknown0x02 : 0);
        }

        public bool IsRotating
        {
            get => (_materialFlags & CollisionPlaneMaterialFlags.Rotating) != 0;
            set => _materialFlags = (_materialFlags & ~CollisionPlaneMaterialFlags.Rotating) | (value ? CollisionPlaneMaterialFlags.Rotating : 0);
        }

        public bool IsSuperSoft
        {
            get => (_materialFlags & CollisionPlaneMaterialFlags.SuperSoft) != 0;
            set => _materialFlags = (_materialFlags & ~CollisionPlaneMaterialFlags.SuperSoft) | (value ? CollisionPlaneMaterialFlags.SuperSoft : 0);
        }

        public bool IsMatUnknown0x10
        {
            get => (_materialFlags & CollisionPlaneMaterialFlags.Unknown0x10) != 0;
            set => _materialFlags = (_materialFlags & ~CollisionPlaneMaterialFlags.Unknown0x10) | (value ? CollisionPlaneMaterialFlags.Unknown0x10 : 0);
        }

        public bool HasUnknownFlag => IsTypeUnknown0x0200 || IsTypeUnknown0x0400 || IsTypeUnknown0x0800 ||
                                      IsTypeUnknown0x1000 || IsTypeUnknown0x2000 || IsTypeUnknown0x4000 ||
                                      IsTypeUnknown0x8000 || IsMatUnknown0x02 || IsMatUnknown0x10;

        public double GetAngleRadians()
        {
            if (LinkLeft == LinkRight)
            {
                return double.NaN;
            }

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

            _linkLeft = left;
            _linkLeft._members.Add(this);
            _linkRight = right;
            if (_linkLeft != _linkRight)
            {
                _linkRight._members.Add(this);
            }
        }

        public CollisionPlane(CollisionObject parent, ColPlane* entry, int offset)
            : this(parent, parent._points[entry->_point1 - offset], parent._points[entry->_point2 - offset])
        {
            _material = entry->_material;
            _materialFlags = entry->_materialFlags;
            _type = entry->Type;
            _typeFlags = entry->TypeFlags;
        }

        public CollisionLink Split(Vector2 point)
        {
            CollisionLink link = new CollisionLink(_parent, point);
            CollisionPlane plane = new CollisionPlane(_parent, link, _linkRight)
            {
                _material = _material,
                _materialFlags = _materialFlags,
                _typeFlags = _typeFlags,
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

        internal void Render()
        {
            if (!_render || LinkLeft == LinkRight)
            {
                return;
            }

            float alpha = 0.8f;

            if (!CollidableByCharacters)
            {
                alpha = 0.5f;
            }
            else if (IsSuperSoft)
            {
                alpha = 0.65f;
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

            GL.Begin(BeginMode.Quads);
            GL.Vertex3(l._x, l._y, 10.0f);
            GL.Vertex3(l._x, l._y, -10.0f);
            GL.Vertex3(r._x, r._y, -10.0f);
            GL.Vertex3(r._x, r._y, 10.0f);
            GL.End();

            GL.Begin(BeginMode.Lines);
            GL.Vertex3(l._x, l._y, 10.0f);
            GL.Vertex3(r._x, r._y, 10.0f);
            GL.Vertex3(l._x, l._y, -10.0f);
            GL.Vertex3(r._x, r._y, -10.0f);
            GL.End();
        }
    }
}
