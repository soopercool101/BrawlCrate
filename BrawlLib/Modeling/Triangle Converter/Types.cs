//This has been converted from C++ and modified.
//Original source:
//http://users.telenet.be/tfautre/softdev/tristripper/

using System.Collections.Generic;

namespace BrawlLib.Modeling.Triangle_Converter
{
    public class Triangle
    {
        public Triangle()
        {
        }

        public Triangle(uint A, uint B, uint C)
        {
            m_A = A;
            m_B = B;
            m_C = C;
            m_StripID = 0;
        }

        public void ResetStripID()
        {
            m_StripID = 0;
        }

        public void SetStripID(uint StripID)
        {
            m_StripID = StripID;
        }

        public uint StripID => m_StripID;

        public uint A => m_A;
        public uint B => m_B;
        public uint C => m_C;

        private readonly uint m_A;
        private readonly uint m_B;
        private readonly uint m_C;

        private uint m_StripID;
        public uint m_Index;
    }

    public class TriangleEdge
    {
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
            if (!(obj is TriangleEdge))
            {
                return false;
            }

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
            return $"{m_A} {m_B}";
        }
    }

    public class TriEdge : TriangleEdge
    {
        public TriEdge(uint A, uint B, uint TriPos) : base(A, B)
        {
            m_TriPos = TriPos;
        }

        public uint TriPos => m_TriPos;
        private readonly uint m_TriPos;

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
            if (!(obj is TriEdge))
            {
                return false;
            }

            return this == obj as TriEdge;
        }

        public override int GetHashCode()
        {
            return m_A.GetHashCode() ^ m_B.GetHashCode();
        }

        public override string ToString()
        {
            return $"{m_A} {m_B} {m_TriPos}";
        }
    }

    public enum TriOrder
    {
        ABC,
        BCA,
        CAB
    };

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

        public uint Start => m_Start;
        public TriOrder Order => m_Order;
        public uint Size => m_Size;

        private readonly uint m_Start;
        private readonly TriOrder m_Order;
        private readonly uint m_Size;

        public List<ushort> _nodes = new List<ushort>();
    }
}