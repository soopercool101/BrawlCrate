using BrawlLib.Internal;
using BrawlLib.Wii.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace BrawlLib.Modeling
{
    public class Facepoint
    {
        public int _index;
        public Vertex3 _vertex;

        private IMatrixNode Node => _vertex != null ? _vertex.MatrixNode : null;

        public ushort NodeID
        {
            get
            {
                if (Node != null)
                {
                    return (ushort) Node.NodeIndex;
                }

                return ushort.MaxValue;
            }
        }

        public int _vertexIndex = -1;
        public int _normalIndex = -1;
        public int[] _colorIndices = new int[2] {-1, -1};
        public int[] _UVIndices = new int[8] {-1, -1, -1, -1, -1, -1, -1, -1};

        [Category("Facepoint")]
        [Browsable(true)]
        public int VertexIndex => _vertexIndex;

        [Category("Facepoint")]
        [Browsable(true)]
        public int NormalIndex => _normalIndex;

        [Category("Facepoint")]
        [Browsable(true)]
        public int[] ColorIndices => _colorIndices;

        [Category("Facepoint")]
        [Browsable(true)]
        public int[] UVIndices => _UVIndices;

        public override string ToString()
        {
            return string.Format("M({12}), V({0}), N({1}), C({2}, {3}), U({4}, {5}, {6}, {7}, {8}, {9}, {10}, {11})",
                _vertexIndex, _normalIndex, _colorIndices[0], _colorIndices[1], _UVIndices[0], _UVIndices[1],
                _UVIndices[2], _UVIndices[3], _UVIndices[4], _UVIndices[5], _UVIndices[6], _UVIndices[7], NodeID);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Facepoint))
            {
                return false;
            }

            return obj.ToString().Equals(ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PrimitiveHeader
    {
        public WiiBeginMode Type;
        public bushort Entries;

        public PrimitiveHeader(WiiBeginMode type, int entries)
        {
            Type = type;
            Entries = (ushort) entries;
        }

        internal VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public VoidPtr Data => Address + 3;
    }

    public class PrimitiveGroup
    {
        //This is the main group of primitives, all using a group of node ids.
        public List<ushort> _nodes = new List<ushort>();

        public void RegroupNodes()
        {
            _nodes.Clear();
            for (int i = 0; i < _headers.Count; i++)
            {
                //Re-assign node ids, just in case the nodes were moved
                foreach (Facepoint point in _facePoints[i])
                {
                    if (!_nodes.Contains(point.NodeID))
                    {
                        _nodes.Add(point.NodeID);
                    }
                }
            }
        }

        //For imports
        public PrimitiveHeader TriangleHeader =>
            new PrimitiveHeader(WiiBeginMode.TriangleList, _triangles.Count * 3);

        public PrimitiveHeader LineHeader => new PrimitiveHeader(WiiBeginMode.Lines, _lines.Count * 2);

        public List<PointTriangle> _triangles = new List<PointTriangle>();
        public List<PointTriangleStrip> _tristrips = new List<PointTriangleStrip>();
        public List<PointLine> _lines = new List<PointLine>();
        public List<PointLineStrip> _linestrips = new List<PointLineStrip>();
        public List<FPoint> _points = new List<FPoint>();

        //For existing models
        public List<PrimitiveHeader> _headers = new List<PrimitiveHeader>();
        public List<List<Facepoint>> _facePoints = new List<List<Facepoint>>();

        //Offset from the start of the primitives to this group.
        public uint _offset;

        //Cache for rebuilding in case nodes are moved
        public List<NodeOffset> _nodeOffsets = new List<NodeOffset>();

        internal const int _nodeCountMax = 10;

        public unsafe void SetNodeIds(VoidPtr primAddr)
        {
            byte* grpAddr = (byte*) (primAddr + _offset);
            for (int i = 0; i < _nodeOffsets.Count; i++)
            {
                *(bushort*) (grpAddr + _nodeOffsets[i]._offset) = (ushort) _nodeOffsets[i]._node.NodeIndex;
            }
        }

        private void AddTriangle(PointTriangle t)
        {
            _triangles.Add(t);
            if (!_nodes.Contains(t._x.NodeID))
            {
                _nodes.Add(t._x.NodeID);
            }

            if (!_nodes.Contains(t._y.NodeID))
            {
                _nodes.Add(t._y.NodeID);
            }

            if (!_nodes.Contains(t._z.NodeID))
            {
                _nodes.Add(t._z.NodeID);
            }
        }

        private void AddTristrip(PointTriangleStrip t)
        {
            _tristrips.Add(t);
            foreach (Facepoint p in t._points)
            {
                if (!_nodes.Contains(p.NodeID))
                {
                    _nodes.Add(p.NodeID);
                }
            }
        }

        private void AddLine(PointLine t)
        {
            _lines.Add(t);
            if (!_nodes.Contains(t._x.NodeID))
            {
                _nodes.Add(t._x.NodeID);
            }

            if (!_nodes.Contains(t._y.NodeID))
            {
                _nodes.Add(t._y.NodeID);
            }
        }

        private void AddLinestrip(PointLineStrip t)
        {
            _linestrips.Add(t);
            foreach (Facepoint p in t._points)
            {
                if (!_nodes.Contains(p.NodeID))
                {
                    _nodes.Add(p.NodeID);
                }
            }
        }

        private void AddPoint(FPoint t)
        {
            _points.Add(t);
            if (!_nodes.Contains(t._x.NodeID))
            {
                _nodes.Add(t._x.NodeID);
            }
        }

        public bool TryAdd(PrimitiveClass p)
        {
            if (p is PointTriangleStrip)
            {
                return TryAdd(p as PointTriangleStrip);
            }

            if (p is PointTriangle)
            {
                return TryAdd(p as PointTriangle);
            }

            if (p is PointLineStrip)
            {
                return TryAdd(p as PointLineStrip);
            }

            if (p is PointLine)
            {
                return TryAdd(p as PointLine);
            }

            if (p is FPoint)
            {
                return TryAdd(p as FPoint);
            }

            return false;
        }

        private bool TryAdd(PointTriangleStrip t)
        {
            List<ushort> newIds = new List<ushort>();
            foreach (Facepoint p in t._points)
            {
                ushort id = p.NodeID;
                if (!_nodes.Contains(id) && !newIds.Contains(id))
                {
                    newIds.Add(id);
                }
            }

            if (newIds.Count + _nodes.Count <= _nodeCountMax)
            {
                AddTristrip(t);
                return true;
            }

            return false;
        }

        private bool TryAdd(PointTriangle t)
        {
            List<ushort> newIds = new List<ushort>();

            ushort x = t._x.NodeID;
            ushort y = t._y.NodeID;
            ushort z = t._z.NodeID;

            if (!_nodes.Contains(x) && !newIds.Contains(x))
            {
                newIds.Add(x);
            }

            if (!_nodes.Contains(y) && !newIds.Contains(y))
            {
                newIds.Add(y);
            }

            if (!_nodes.Contains(z) && !newIds.Contains(z))
            {
                newIds.Add(z);
            }

            //There's a limit of 10 matrices per group...
            if (newIds.Count + _nodes.Count <= _nodeCountMax)
            {
                AddTriangle(t);
                return true;
            }

            return false;
        }

        private bool TryAdd(PointLineStrip t)
        {
            List<ushort> newIds = new List<ushort>();
            foreach (Facepoint p in t._points)
            {
                ushort id = p.NodeID;
                if (!_nodes.Contains(id) && !newIds.Contains(id))
                {
                    newIds.Add(id);
                }
            }

            if (newIds.Count + _nodes.Count <= _nodeCountMax)
            {
                AddLinestrip(t);
                return true;
            }

            return false;
        }

        private bool TryAdd(PointLine t)
        {
            List<ushort> newIds = new List<ushort>();

            ushort x = t._x.NodeID;
            ushort y = t._y.NodeID;

            if (!_nodes.Contains(x) && !newIds.Contains(x))
            {
                newIds.Add(x);
            }

            if (!_nodes.Contains(y) && !newIds.Contains(y))
            {
                newIds.Add(y);
            }

            //There's a limit of 10 matrices per group...
            if (newIds.Count + _nodes.Count <= _nodeCountMax)
            {
                AddLine(t);
                return true;
            }

            return false;
        }

        private bool TryAdd(FPoint t)
        {
            List<ushort> newIds = new List<ushort>();

            ushort x = t._x.NodeID;

            if (!_nodes.Contains(x) && !newIds.Contains(x))
            {
                newIds.Add(x);
            }

            //There's a limit of 10 matrices per group...
            if (newIds.Count + _nodes.Count <= _nodeCountMax)
            {
                AddPoint(t);
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"Nodes: {_nodes.Count} - Primitives: {_headers.Count}";
        }
    }

    public class NodeOffset
    {
        internal uint _offset;
        internal IMatrixNode _node;

        public NodeOffset(uint offset, IMatrixNode node)
        {
            _offset = offset;
            _node = node;
        }
    }

    public class PrimitiveClass
    {
        public virtual List<Facepoint> Points { get; set; }

        public static int Compare(PrimitiveClass p1, PrimitiveClass p2)
        {
            return p1.GetType() == p2.GetType() ? 0 : p1 is PointTriangleStrip ? -1 : 1;
        }
    }

    public class PointTriangleStrip : PrimitiveClass
    {
        public PrimitiveHeader Header => new PrimitiveHeader(WiiBeginMode.TriangleStrip, _points.Count);
        public List<Facepoint> _points = new List<Facepoint>();

        public override List<Facepoint> Points
        {
            get => _points;
            set => _points = value;
        }
    }

    public class PointTriangle : PrimitiveClass
    {
        public Facepoint _x;
        public Facepoint _y;
        public Facepoint _z;

        public Facepoint this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return _x;
                    case 1: return _y;
                    case 2: return _z;
                }

                return null;
            }
            set
            {
                switch (i)
                {
                    case 0:
                        _x = value;
                        break;
                    case 1:
                        _y = value;
                        break;
                    case 2:
                        _z = value;
                        break;
                }
            }
        }

        public override List<Facepoint> Points
        {
            get => new List<Facepoint> {_x, _y, _z};
            set
            {
                _x = value[0];
                _y = value[1];
                _z = value[2];
            }
        }

        public PointTriangle()
        {
        }

        public PointTriangle(Facepoint x, Facepoint y, Facepoint z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public bool Contains(Facepoint f)
        {
            if (_x == f)
            {
                return true;
            }

            if (_y == f)
            {
                return true;
            }

            if (_z == f)
            {
                return true;
            }

            return false;
        }
    }

    public class PointLineStrip : PrimitiveClass
    {
        public PrimitiveHeader Header => new PrimitiveHeader(WiiBeginMode.LineStrip, _points.Count);
        public List<Facepoint> _points = new List<Facepoint>();

        public override List<Facepoint> Points
        {
            get => _points;
            set => _points = value;
        }
    }

    public class PointLine : PrimitiveClass
    {
        public Facepoint _x;
        public Facepoint _y;

        public Facepoint this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return _x;
                    case 1: return _y;
                }

                return null;
            }
            set
            {
                switch (i)
                {
                    case 0:
                        _x = value;
                        break;
                    case 1:
                        _y = value;
                        break;
                }
            }
        }

        public override List<Facepoint> Points
        {
            get => new List<Facepoint> {_x, _y};
            set
            {
                _x = value[0];
                _y = value[1];
            }
        }

        public PointLine()
        {
        }

        public PointLine(Facepoint x, Facepoint y)
        {
            _x = x;
            _y = y;
        }

        public bool Contains(Facepoint f)
        {
            if (_x == f)
            {
                return true;
            }

            if (_y == f)
            {
                return true;
            }

            return false;
        }
    }

    public class FPoint : PrimitiveClass
    {
        public Facepoint _x;

        public FPoint(Facepoint x)
        {
            _x = x;
        }
    }
}