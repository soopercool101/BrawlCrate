//This has been converted from C++ and modified.
//Original source:
//http://users.telenet.be/tfautre/softdev/tristripper/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BrawlLib.Modeling.Triangle_Converter
{
    public enum PrimType
    {
        TriangleList = 0x0004,
        TriangleStrip = 0x0005
    }

    public class Primitive
    {
        public Primitive(PrimType type)
        {
            Type = type;
            Indices = new List<uint>();
            NodeIDs = new List<ushort>();
        }

        public List<ushort> NodeIDs;
        public List<uint> Indices;
        public PrimType Type;
    }

    public class TriStripper
    {
        private readonly List<Primitive> m_PrimitivesVector;
        private readonly GraphArray<Triangle> m_Triangles;
        private readonly HeapArray m_TriHeap;
        private readonly List<uint> m_Candidates;
        private CacheSimulator m_Cache;
        private readonly CacheSimulator m_BackCache;
        private uint m_StripID;
        private uint m_MinStripSize;
        private bool m_BackwardSearch;
        private bool m_FirstRun;
        private readonly ushort[] m_Nodes;
        private readonly int[] m_ImpTable;
        private List<ushort> m_CurrentNodes;

        public TriStripper(uint[] TriIndices, ushort[] NodeIds, int[] ImpTable)
        {
            m_ImpTable = ImpTable;
            m_Nodes = NodeIds;
            m_Triangles = new GraphArray<Triangle>((uint) TriIndices.Length / 3);
            m_StripID = 0;
            m_FirstRun = true;
            m_PrimitivesVector = new List<Primitive>();
            m_TriHeap = new HeapArray(CompareType.Less);
            m_Candidates = new List<uint>();
            m_Cache = new CacheSimulator();
            m_BackCache = new CacheSimulator();

            SetCacheSize();
            SetMinStripSize();
            SetBackwardSearch();
            SetPushCacheHits();

            MakeConnectivityGraph(m_Triangles, TriIndices);
        }

        private bool Cache => CacheSize != 0;
        private uint CacheSize => m_Cache.Size;

        public List<Primitive> Strip()
        {
            if (!m_FirstRun)
            {
                UnmarkNodes(m_Triangles);
                ResetStripIDs();
                m_Cache.Reset();
                m_TriHeap.Clear();
                m_Candidates.Clear();
                m_StripID = 0;
            }

            InitTriHeap();

            Stripify();
            AddRemainingTriangles();

            m_FirstRun = false;
            return m_PrimitivesVector.ToList();
        }

        #region Stripifier Algorithm Settings

        //Set the post-T&L cache size (0 disables the cache optimizer).
        public void SetCacheSize(uint CacheSize = 10)
        {
            m_Cache.Resize(CacheSize);
            m_BackCache.Resize(CacheSize);
        }

        //Set the minimum size of a triangle strip (should be at least 2 triangles).
        //The stripifier discard any candidate strips that does not satisfy the minimum size condition.
        public void SetMinStripSize(uint MinStripSize = 2)
        {
            if (MinStripSize < 2)
            {
                m_MinStripSize = 2;
            }
            else
            {
                m_MinStripSize = MinStripSize;
            }
        }

        //Set the backward search mode in addition to the forward search mode.
        //In forward mode, the candidate strips are built with the current candidate triangle being the first
        //triangle of the strip. When the backward mode is enabled, the stripifier also tests candidate strips
        //where the current candidate triangle is the last triangle of the strip.
        //Enable this if you want better results at the expense of being slightly slower.
        //Note: Do *NOT* use this when the cache optimizer is enabled; it only gives worse results.
        public void SetBackwardSearch(bool Enabled = false)
        {
            m_BackwardSearch = Enabled;
        }

        //Set the cache simulator FIFO behavior (does nothing if the cache optimizer is disabled).
        //When enabled, the cache is simulated as a simple FIFO structure. However, when
        //disabled, indices that trigger cache hits are not pushed into the FIFO structure.
        //This allows simulating some GPUs that do not duplicate cache entries (e.g. NV25 or greater).
        public void SetPushCacheHits(bool Enabled = true)
        {
            m_Cache.PushCacheHits(Enabled);
        }

        #endregion

        private void InitTriHeap()
        {
            m_TriHeap.Reserve(m_Triangles.Count);

            //Set up the triangles priority queue
            //The lower the number of available neighbour triangles, the higher the priority.
            for (uint i = 0; i < m_Triangles.Count; i++)
            {
                m_TriHeap.Push(m_Triangles[i].Size);
            }

            //We're not going to add new elements anymore
            m_TriHeap.Lock();

            //Remove useless triangles
            //Note: we had to put all of them into the heap before to ensure coherency of the heap_array object
            while (!m_TriHeap.Empty && m_TriHeap.Top == 0)
            {
                m_TriHeap.Pop();
            }
        }

        private void Stripify()
        {
            while (!m_TriHeap.Empty)
            {
                //There is no triangle in the candidates list, refill it with the loneliest triangle
                uint HeapTop = m_TriHeap.Position(0);
                m_Candidates.Add(HeapTop);

                while (m_Candidates.Count != 0)
                {
                    //Note: FindBestStrip empties the candidate list, while BuildStrip refills it
                    Strip TriStrip = FindBestStrip();

                    if (TriStrip.Size >= m_MinStripSize)
                    {
                        BuildStrip(TriStrip);
                    }
                }

                if (!m_TriHeap.Removed(HeapTop))
                {
                    m_TriHeap.Erase(HeapTop);
                }

                //Eliminate all the triangles that have now become useless
                while (!m_TriHeap.Empty && m_TriHeap.Top == 0)
                {
                    m_TriHeap.Pop();
                }
            }
        }

        private void AddRemainingTriangles()
        {
            //Create the last indices array and fill it with all the triangles that couldn't be stripped
            Primitive p = new Primitive(PrimType.TriangleList);

            for (uint i = 0; i < m_Triangles.Count; i++)
            {
                if (!m_Triangles[i].Marked)
                {
                    p.Indices.Add(m_Triangles[i].m_Elem.A);
                    p.Indices.Add(m_Triangles[i].m_Elem.B);
                    p.Indices.Add(m_Triangles[i].m_Elem.C);
                }
            }

            if (p.Indices.Count > 0)
            {
                m_PrimitivesVector.Add(p);
            }
        }

        private void ResetStripIDs()
        {
            foreach (Triangle r in m_Triangles)
            {
                r.ResetStripID();
            }
        }

        private Strip FindBestStrip()
        {
            //Allow to restore the cache (modified by ExtendTriToStrip) and implicitly reset the cache hit count
            CacheSimulator CacheBackup = m_Cache;

            Policy policy = new Policy(m_MinStripSize, Cache);

            while (m_Candidates.Count != 0)
            {
                uint Candidate = m_Candidates[m_Candidates.Count - 1];
                m_Candidates.RemoveAt(m_Candidates.Count - 1);

                //Discard useless triangles from the candidate list
                if (m_Triangles[Candidate].Marked || m_TriHeap[Candidate] == 0)
                {
                    continue;
                }

                //Try to extend the triangle in the 3 possible forward directions
                for (uint i = 0; i < 3; i++)
                {
                    Strip Strip = ExtendToStrip(Candidate, (TriOrder) i);
                    policy.Challenge(Strip, m_TriHeap[Strip.Start], m_Cache.HitCount);

                    m_Cache = CacheBackup;
                }

                //Try to extend the triangle in the 6 possible backward directions
                if (m_BackwardSearch)
                {
                    for (uint i = 0; i < 3; i++)
                    {
                        Strip Strip = BackExtendToStrip(Candidate, (TriOrder) i, false);
                        if (Strip != null)
                        {
                            policy.Challenge(Strip, m_TriHeap[Strip.Start], m_Cache.HitCount);
                        }

                        m_Cache = CacheBackup;
                    }

                    for (uint i = 0; i < 3; i++)
                    {
                        Strip Strip = BackExtendToStrip(Candidate, (TriOrder) i, true);
                        if (Strip != null)
                        {
                            policy.Challenge(Strip, m_TriHeap[Strip.Start], m_Cache.HitCount);
                        }

                        m_Cache = CacheBackup;
                    }
                }
            }

            return policy.BestStrip;
        }

        private Strip ExtendToStrip(uint Start, TriOrder Order)
        {
            TriOrder StartOrder = Order;

            m_CurrentNodes = new List<ushort>();
            _checkNodes = true;

            //Begin a new strip
            Triangle tri = m_Triangles[Start].m_Elem;
            tri.SetStripID(++m_StripID);
            AddTriangle(tri, Order, false);

            TryAddNode(tri.A);
            TryAddNode(tri.B);
            TryAddNode(tri.C);

            uint Size = 1;
            bool ClockWise = false;

            //Loop while we can further extend the strip
            for (uint i = Start; i < m_Triangles.Count && (!Cache || Size + 2 < CacheSize); Size++)
            {
                GraphArray<Triangle>.Node Node = m_Triangles[i];
                GraphArray<Triangle>.Arc Link = LinkToNeighbour(Node, ClockWise, ref Order, false);

                // Is it the end of the strip?
                if (Link == null)
                {
                    Size--;
                    i = m_Triangles.Count;
                }
                else
                {
                    i = (Node = Link.Terminal).m_Elem.m_Index;

                    Node.m_Elem.SetStripID(m_StripID);
                    ClockWise = !ClockWise;
                }
            }

            _checkNodes = false;
            m_CurrentNodes.Clear();

            return new Strip(Start, StartOrder, Size);
        }

        private Strip BackExtendToStrip(uint Start, TriOrder Order, bool ClockWise)
        {
            m_CurrentNodes = new List<ushort>();
            _checkNodes = true;

            //Begin a new strip
            Triangle tri = m_Triangles[Start].m_Elem;
            uint b = LastEdge(tri, Order).B;
            if (TryAddNode(b))
            {
                tri.SetStripID(++m_StripID);
                BackAddIndex(b);
            }
            else
            {
                _checkNodes = false;
                m_CurrentNodes.Clear();
                return null;
            }

            uint Size = 1;
            GraphArray<Triangle>.Node Node = null;

            //Loop while we can further extend the strip
            for (uint i = Start; !Cache || Size + 2 < CacheSize; Size++)
            {
                Node = m_Triangles[i];
                GraphArray<Triangle>.Arc Link = BackLinkToNeighbour(Node, ClockWise, ref Order);

                //Is it the end of the strip?
                if (Link == null)
                {
                    break;
                }

                i = (Node = Link.Terminal).m_Elem.m_Index;

                Node.m_Elem.SetStripID(m_StripID);
                ClockWise = !ClockWise;
            }

            _checkNodes = false;
            m_CurrentNodes.Clear();

            //We have to start from a counterclockwise triangle.
            //Simply return an empty strip in the case where the first triangle is clockwise.
            //Even though we could discard the first triangle and start from the next counterclockwise triangle,
            //this often leads to more lonely triangles afterward.
            if (ClockWise)
            {
                return null;
            }

            if (Cache)
            {
                m_Cache.Merge(m_BackCache, Size);
                m_BackCache.Reset();
            }

            return new Strip(Node.m_Elem.m_Index, Order, Size);
        }

        private bool _checkNodes;

        private bool TryAddNode(uint index)
        {
            if (!_checkNodes)
            {
                return true;
            }

            ushort node = m_Nodes[m_ImpTable[index]];
            if (!m_CurrentNodes.Contains(node))
            {
                if (m_CurrentNodes.Count == PrimitiveGroup._nodeCountMax)
                {
                    return false;
                }

                m_CurrentNodes.Add(node);
            }

            return true;
        }

        private GraphArray<Triangle>.Arc LinkToNeighbour(GraphArray<Triangle>.Node Node, bool ClockWise,
                                                         ref TriOrder Order, bool NotSimulation)
        {
            TriangleEdge Edge = LastEdge(Node.m_Elem, Order);
            for (uint i = Node.m_Begin; i < Node.m_End; i++)
            {
                GraphArray<Triangle>.Arc Link = Node.Arcs[(int) i];

                //Get the reference to the possible next triangle
                Triangle Tri = Link.Terminal.m_Elem;

                //Check whether it's already been used
                if ((NotSimulation || Tri.StripID != m_StripID) && !Link.Terminal.Marked)
                {
                    //Does the current candidate triangle match the required position for the strip?
                    if (Edge.B == Tri.A && Edge.A == Tri.B && TryAddNode(Tri.C))
                    {
                        Order = ClockWise ? TriOrder.ABC : TriOrder.BCA;
                        AddIndex(Tri.C, NotSimulation);
                        return Link;
                    }

                    if (Edge.B == Tri.B && Edge.A == Tri.C && TryAddNode(Tri.A))
                    {
                        Order = ClockWise ? TriOrder.BCA : TriOrder.CAB;
                        AddIndex(Tri.A, NotSimulation);
                        return Link;
                    }

                    if (Edge.B == Tri.C && Edge.A == Tri.A && TryAddNode(Tri.B))
                    {
                        Order = ClockWise ? TriOrder.CAB : TriOrder.ABC;
                        AddIndex(Tri.B, NotSimulation);
                        return Link;
                    }
                }
            }

            return null;
        }

        private GraphArray<Triangle>.Arc BackLinkToNeighbour(GraphArray<Triangle>.Node Node, bool ClockWise,
                                                             ref TriOrder Order)
        {
            TriangleEdge Edge = FirstEdge(Node.m_Elem, Order);
            for (uint i = Node.m_Begin; i < Node.m_End; i++)
            {
                GraphArray<Triangle>.Arc Link = Node.Arcs[(int) i];

                //Get the reference to the possible previous triangle
                Triangle Tri = Link.Terminal.m_Elem;

                //Check whether it's already been used
                if (Tri.StripID != m_StripID && !Link.Terminal.Marked)
                {
                    //Does the current candidate triangle match the required position for the strip?
                    if (Edge.B == Tri.A && Edge.A == Tri.B && TryAddNode(Tri.C))
                    {
                        Order = ClockWise ? TriOrder.CAB : TriOrder.BCA;
                        BackAddIndex(Tri.C);
                        return Link;
                    }

                    if (Edge.B == Tri.B && Edge.A == Tri.C && TryAddNode(Tri.A))
                    {
                        Order = ClockWise ? TriOrder.ABC : TriOrder.CAB;
                        BackAddIndex(Tri.A);
                        return Link;
                    }

                    if (Edge.B == Tri.C && Edge.A == Tri.A && TryAddNode(Tri.B))
                    {
                        Order = ClockWise ? TriOrder.BCA : TriOrder.ABC;
                        BackAddIndex(Tri.B);
                        return Link;
                    }
                }
            }

            return null;
        }

        private void BuildStrip(Strip Strip)
        {
            uint Start = Strip.Start;

            bool ClockWise = false;
            TriOrder Order = Strip.Order;

            //Create a new strip
            Primitive p = new Primitive(PrimType.TriangleStrip);
            m_PrimitivesVector.Add(p);
            AddTriangle(m_Triangles[Start].m_Elem, Order, true);
            MarkTriAsTaken(Start);

            //Loop while we can further extend the strip
            GraphArray<Triangle>.Node Node = m_Triangles[Start];

            for (uint Size = 1; Size < Strip.Size; Size++)
            {
                GraphArray<Triangle>.Arc Link = LinkToNeighbour(Node, ClockWise, ref Order, true);

                Debug.Assert(Link != null);

                //Go to the next triangle
                Node = Link.Terminal;
                MarkTriAsTaken(Node.m_Elem.m_Index);
                ClockWise = !ClockWise;
            }
        }

        private void MarkTriAsTaken(uint i)
        {
            //Mark the triangle node
            m_Triangles[i].Marked = true;

            //Remove triangle from priority queue if it isn't yet
            if (!m_TriHeap.Removed(i))
            {
                m_TriHeap.Erase(i);
            }

            //Adjust the degree of available neighbour triangles
            GraphArray<Triangle>.Node Node = m_Triangles[i];
            for (uint x = Node.m_Begin; x < Node.m_End; x++)
            {
                GraphArray<Triangle>.Arc Link = Node.Arcs[(int) x];

                uint j = Link.Terminal.m_Elem.m_Index;
                if (!m_Triangles[j].Marked && !m_TriHeap.Removed(j))
                {
                    uint NewDegree = m_TriHeap[j];
                    NewDegree = NewDegree - 1;
                    m_TriHeap.Update(j, NewDegree);

                    //Update the candidate list if cache is enabled
                    if (Cache && NewDegree > 0)
                    {
                        m_Candidates.Add(j);
                    }
                }
            }
        }

        private void AddIndex(uint i, bool NotSimulation)
        {
            if (Cache)
            {
                m_Cache.Push(i, !NotSimulation);
            }

            if (NotSimulation)
            {
                m_PrimitivesVector[m_PrimitivesVector.Count - 1].Indices.Add(i);
            }
        }

        private void BackAddIndex(uint i)
        {
            if (Cache)
            {
                m_BackCache.Push(i, true);
            }
        }

        private void AddTriangle(Triangle Tri, TriOrder Order, bool NotSimulation)
        {
            switch (Order)
            {
                case TriOrder.ABC:
                    AddIndex(Tri.A, NotSimulation);
                    AddIndex(Tri.B, NotSimulation);
                    AddIndex(Tri.C, NotSimulation);
                    break;

                case TriOrder.BCA:
                    AddIndex(Tri.B, NotSimulation);
                    AddIndex(Tri.C, NotSimulation);
                    AddIndex(Tri.A, NotSimulation);
                    break;

                case TriOrder.CAB:
                    AddIndex(Tri.C, NotSimulation);
                    AddIndex(Tri.A, NotSimulation);
                    AddIndex(Tri.B, NotSimulation);
                    break;
            }
        }

        private void BackAddTriangle(Triangle Tri, TriOrder Order)
        {
            switch (Order)
            {
                case TriOrder.ABC:
                    BackAddIndex(Tri.C);
                    BackAddIndex(Tri.B);
                    BackAddIndex(Tri.A);
                    break;

                case TriOrder.BCA:
                    BackAddIndex(Tri.A);
                    BackAddIndex(Tri.C);
                    BackAddIndex(Tri.B);
                    break;

                case TriOrder.CAB:
                    BackAddIndex(Tri.B);
                    BackAddIndex(Tri.A);
                    BackAddIndex(Tri.C);
                    break;
            }
        }

        private static TriangleEdge FirstEdge(Triangle Triangle, TriOrder Order)
        {
            switch (Order)
            {
                case TriOrder.ABC: return new TriangleEdge(Triangle.A, Triangle.B);
                case TriOrder.BCA: return new TriangleEdge(Triangle.B, Triangle.C);
                case TriOrder.CAB: return new TriangleEdge(Triangle.C, Triangle.A);
                default:           return new TriangleEdge(0, 0);
            }
        }

        private static TriangleEdge LastEdge(Triangle Triangle, TriOrder Order)
        {
            switch (Order)
            {
                case TriOrder.ABC: return new TriangleEdge(Triangle.B, Triangle.C);
                case TriOrder.BCA: return new TriangleEdge(Triangle.C, Triangle.A);
                case TriOrder.CAB: return new TriangleEdge(Triangle.A, Triangle.B);
                default:           return new TriangleEdge(0, 0);
            }
        }

        public void UnmarkNodes(GraphArray<Triangle> G)
        {
            foreach (GraphArray<Triangle>.Node t in G)
            {
                t.Marked = false;
            }
        }

        public static int EdgeComp(TriEdge x, TriEdge y)
        {
            uint xa = x.A;
            uint xb = x.B;
            uint ya = y.A;
            uint yb = y.B;

            return xa < ya || xa == ya && xb < yb ? -1 : 1;
        }

        private void MakeConnectivityGraph(GraphArray<Triangle> Triangles, uint[] Indices)
        {
            Debug.Assert(Triangles.Count == Indices.Length / 3);

            //Fill the triangle data
            for (int i = 0; i < Triangles.Count; i++)
            {
                Triangles[(uint) i].m_Elem = new Triangle(
                        Indices[i * 3 + 0],
                        Indices[i * 3 + 1],
                        Indices[i * 3 + 2])
                    {m_Index = (uint) i};
            }

            //Build an edge lookup table
            List<TriEdge> EdgeMap = new List<TriEdge>();
            for (uint i = 0; i < Triangles.Count; i++)
            {
                Triangle Tri = Triangles[i].m_Elem;
                EdgeMap.Add(new TriEdge(Tri.A, Tri.B, i));
                EdgeMap.Add(new TriEdge(Tri.B, Tri.C, i));
                EdgeMap.Add(new TriEdge(Tri.C, Tri.A, i));
            }

            EdgeMap.Sort(EdgeComp);

            //Link neighbour triangles together using the lookup table
            for (uint i = 0; i < Triangles.Count; i++)
            {
                Triangle Tri = Triangles[i].m_Elem;
                LinkNeighbours(Triangles, EdgeMap, new TriEdge(Tri.B, Tri.A, i));
                LinkNeighbours(Triangles, EdgeMap, new TriEdge(Tri.C, Tri.B, i));
                LinkNeighbours(Triangles, EdgeMap, new TriEdge(Tri.A, Tri.C, i));
            }
        }

        private static int BinarySearch<T>(IList<T> list, T value, Comparison<T> comp)
        {
            int lo = 0, hi = list.Count - 1;
            while (lo < hi)
            {
                int m = (hi + lo) / 2;
                if (comp(list[m], value) < 0)
                {
                    lo = m + 1;
                }
                else
                {
                    hi = m - 1;
                }
            }

            if (comp(list[lo], value) < 0)
            {
                lo++;
            }

            return lo;
        }

        private void LinkNeighbours(GraphArray<Triangle> Triangles, List<TriEdge> EdgeMap, TriEdge Edge)
        {
            //Find the first edge equal to Edge
            //See if there are any other edges that are equal
            //(if so, it means that more than 2 triangles are sharing the same edge,
            //which is unlikely but not impossible)
            for (int i = BinarySearch(EdgeMap, Edge, EdgeComp);
                i < EdgeMap.Count && Edge == EdgeMap[i];
                i++)
            {
                Triangles.InsertArc(Edge.TriPos, EdgeMap[i].TriPos);
            }

            //Note: degenerated triangles will also point themselves as neighbour triangles
        }
    }
}