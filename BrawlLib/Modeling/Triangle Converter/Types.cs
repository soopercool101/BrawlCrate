//This has been converted from C++ and modified.
//Original source:
//http://users.telenet.be/tfautre/softdev/tristripper/

using System;
using System.Collections.Generic;

namespace BrawlLib.Modeling.Triangle_Converter
{
    public class Triangle
    {
        public Triangle() { }
        public Triangle(uint A, uint B, uint C)
        {
            m_A = A;
            m_B = B;
            m_C = C;
            m_StripID = 0;
        }

        public void ResetStripID() { m_StripID = 0; }
        public void SetStripID(uint StripID) { m_StripID = StripID; }
        public uint StripID { get { return m_StripID; } }

        public uint A { get { return m_A; } }
        public uint B { get { return m_B; } }
        public uint C { get { return m_C; } }

        private uint m_A;
        private uint m_B;
        private uint m_C;

        private uint m_StripID;
        public uint m_Index;
    }

    public class TriangleEdge
    {
        public TriangleEdge(uint A, uint B) { m_A = A; m_B = B; }

        public uint A { get { return m_A; } }
        public uint B { get { return m_B; } }

        public static bool operator ==(TriangleEdge left, TriangleEdge right)
        {
            return ((left.A == right.A) && (left.B == right.B));
        }
        public static bool operator !=(TriangleEdge left, TriangleEdge right)
        {
            return ((left.A != right.A) || (left.B != right.B));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is TriangleEdge))
                return false;

            return this == obj as TriangleEdge;
        }

        public override int GetHashCode()
        {
            return m_A.GetHashCode() ^ m_B.GetHashCode();
        }

        public uint m_A;
        public uint m_B;

        public override string ToString()
        {
            return String.Format("{0} {1}", m_A, m_B);
        }
    }

    public class TriEdge : TriangleEdge
    {
        public TriEdge(uint A, uint B, uint TriPos) : base(A, B) { m_TriPos = TriPos; }
        public uint TriPos { get { return m_TriPos; } }
        private uint m_TriPos;

        public static bool operator ==(TriEdge left, TriEdge right)
        {
            return ((left.A == right.A) && (left.B == right.B));
        }
        public static bool operator !=(TriEdge left, TriEdge right)
        {
            return ((left.A != right.A) || (left.B != right.B));
        }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is TriEdge))
                return false;

            return this == obj as TriEdge;
        }

        public override int GetHashCode()
        {
            return m_A.GetHashCode() ^ m_B.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2}", m_A, m_B, m_TriPos);
        }
    }

    public enum TriOrder { ABC, BCA, CAB };
    public class Strip
    {
        public Strip()
        {
            m_Start = uint.MaxValue;
            m_Order = TriOrder.ABC;
            m_Size = 0;
        }

        public Strip(uint Start, TriOrder Order, uint Size)
        {
            m_Start = Start;
            m_Order = Order;
            m_Size = Size;
        }

        public uint Start { get { return m_Start; } }
        public TriOrder Order { get { return m_Order; } }
        public uint Size { get { return m_Size; } }

        private uint m_Start;
        private TriOrder m_Order;
        private uint m_Size;

        public List<ushort> _nodes = new List<ushort>();
    }
}
