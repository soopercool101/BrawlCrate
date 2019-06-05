//This has been converted from C++ and modified.
//Original source:
//http://users.telenet.be/tfautre/softdev/tristripper/

using System.Collections.Generic;

namespace BrawlLib.Modeling.Triangle_Converter
{
    public class Triangle
    {
        public uint m_Index;

        public Triangle()
        {
        }

        public Triangle(uint A, uint B, uint C)
        {
            this.A = A;
            this.B = B;
            this.C = C;
            StripID = 0;
        }

        public uint StripID { get; private set; }

        public uint A { get; }

        public uint B { get; }

        public uint C { get; }

        public void ResetStripID()
        {
            StripID = 0;
        }

        public void SetStripID(uint StripID)
        {
            this.StripID = StripID;
        }
    }

    public class TriangleEdge
    {
        public uint m_A;
        public uint m_B;

        public TriangleEdge(uint A, uint B)
        {
            m_A = A;
            m_B = B;
        }

        public uint A => m_A;
        public uint B => m_B;

        public static bool operator ==(TriangleEdge left, TriangleEdge right)
        {
            return left.A == right.A && left.B == right.B;
        }

        public static bool operator !=(TriangleEdge left, TriangleEdge right)
        {
            return left.A != right.A || left.B != right.B;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is TriangleEdge)) return false;

            return this == obj as TriangleEdge;
        }

        public override int GetHashCode()
        {
            return m_A.GetHashCode() ^ m_B.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", m_A, m_B);
        }
    }

    public class TriEdge : TriangleEdge
    {
        public TriEdge(uint A, uint B, uint TriPos) : base(A, B)
        {
            this.TriPos = TriPos;
        }

        public uint TriPos { get; }

        public static bool operator ==(TriEdge left, TriEdge right)
        {
            return left.A == right.A && left.B == right.B;
        }

        public static bool operator !=(TriEdge left, TriEdge right)
        {
            return left.A != right.A || left.B != right.B;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is TriEdge)) return false;

            return this == obj as TriEdge;
        }

        public override int GetHashCode()
        {
            return m_A.GetHashCode() ^ m_B.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", m_A, m_B, TriPos);
        }
    }

    public enum TriOrder
    {
        ABC,
        BCA,
        CAB
    }

    public class Strip
    {
        public List<ushort> _nodes = new List<ushort>();

        public Strip()
        {
            Start = uint.MaxValue;
            Order = TriOrder.ABC;
            Size = 0;
        }

        public Strip(uint Start, TriOrder Order, uint Size)
        {
            this.Start = Start;
            this.Order = Order;
            this.Size = Size;
        }

        public uint Start { get; }

        public TriOrder Order { get; }

        public uint Size { get; }
    }
}