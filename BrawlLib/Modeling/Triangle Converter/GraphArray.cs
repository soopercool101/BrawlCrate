//This has been converted from C++ and modified.
//Original source:
//http://users.telenet.be/tfautre/softdev/tristripper/

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace BrawlLib.Modeling.Triangle_Converter
{
    public class GraphArray<T> : IEnumerable
    {
        protected List<Arc> m_Arcs;
        protected List<Node> m_Nodes;

        public GraphArray()
        {
        }

        public GraphArray(uint NbNodes)
        {
            m_Nodes = new List<Node>();
            for (var i = 0; i < NbNodes; i++) m_Nodes.Add(new Node(this));

            m_Arcs = new List<Arc>();
        }

        //Node related member functions
        public bool Empty => m_Nodes.Count == 0;
        public uint Count => (uint) m_Nodes.Count;

        public Node this[uint i]
        {
            get
            {
                Debug.Assert(i < Count);
                return m_Nodes[(int) i];
            }
        }

        public IEnumerator GetEnumerator()
        {
            return m_Nodes.GetEnumerator();
        }

        // Arc related member functions
        public Arc InsertArc(uint Initial, uint Terminal)
        {
            Debug.Assert(Initial < Count, "Initial is greater than count");
            Debug.Assert(Terminal < Count, "Terminal is greater than count");

            var r = new Arc(m_Nodes[(int) Terminal]);
            m_Arcs.Add(r);

            var Node = m_Nodes[(int) Initial];
            if (Node.Empty)
            {
                Node.m_Begin = (uint) m_Arcs.Count - 1;
                Node.m_End = (uint) m_Arcs.Count;
            }
            else
            {
                Node.m_End++;

                // we optimise here for make_connectivity_graph()
                // we know all the arcs for a given node are successively and sequentially added
                Debug.Assert(Node.m_End == m_Arcs.Count);
            }

            return r;
        }

        // Optimized (overloaded) functions
        public void Swap(GraphArray<T> Right)
        {
            var n = m_Nodes;
            var a = m_Arcs;
            m_Nodes = Right.m_Nodes;
            m_Arcs = Right.m_Arcs;
            Right.m_Nodes = n;
            Right.m_Arcs = a;
        }

        public class Arc
        {
            public Node m_Terminal;

            public Arc(Node Terminal)
            {
                m_Terminal = Terminal;
            }

            public Node Terminal => m_Terminal;
        }

        public class Node
        {
            public uint m_Begin;

            public T m_Elem;
            public uint m_End;

            public GraphArray<T> m_Graph;

            public Node(GraphArray<T> graph)
            {
                m_Graph = graph;
                m_Begin = uint.MaxValue;
                m_End = uint.MaxValue;
                Marked = false;
            }

            public bool Marked { get; set; }

            public bool Empty => m_Begin == m_End;
            public uint Size => m_End - m_Begin;

            public List<Arc> Arcs => m_Graph.m_Arcs;
        }
    }
}