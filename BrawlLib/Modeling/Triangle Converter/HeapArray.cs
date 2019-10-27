//This has been converted from C++ and modified.
//Original source:
//http://users.telenet.be/tfautre/softdev/tristripper/

using System.Collections.Generic;
using System.Diagnostics;

namespace BrawlLib.Modeling.Triangle_Converter
{
    internal enum CompareType
    {
        Greater,
        Less
    }

    internal class HeapArray
    {
        protected class Linker
        {
            public Linker(uint Elem, uint i)
            {
                m_Elem = Elem;
                m_Index = i;
            }

            public uint m_Elem;
            public uint m_Index;
        }

        protected List<Linker> m_Heap;
        protected List<uint> m_Finder;
        protected CompareType m_Compare = CompareType.Greater;
        protected bool m_Locked;

        // Pre = PreCondition, Post = PostCondition 
        public HeapArray(CompareType c) // Post: ((size() == 0) && ! locked())
        {
            m_Heap = new List<Linker>();
            m_Finder = new List<uint>();
            m_Locked = false;
            m_Compare = c;
        }

        public void Clear() // Post: ((size() == 0) && ! locked())
        {
            m_Heap.Clear();
            m_Finder.Clear();
            m_Locked = false;
        }

        public void Reserve(uint Size)
        {
            //m_Heap.Capacity = (int)Size;
            //m_Finder.Capacity = (int)Size;
        }

        public uint Size => (uint) m_Heap.Count;
        public bool Empty => m_Heap.Count == 0;
        public bool Locked => m_Locked;

        public bool Removed(uint i) // Pre: (valid(i))
        {
            Debug.Assert(Valid(i));
            return m_Finder[(int) i] >= m_Heap.Count;
        }

        public bool Valid(uint i)
        {
            return i < m_Finder.Count;
        }

        public uint Position(uint i) // Pre: (valid(i))
        {
            Debug.Assert(Valid(i));
            return m_Heap[(int) i].m_Index;
        }

        public uint Top // Pre: (! empty())
        {
            get
            {
                Debug.Assert(!Empty);
                return m_Heap[0].m_Elem;
            }
        }

        public uint this[uint i] // Pre: (! removed(i))
        {
            get
            {
                Debug.Assert(!Removed(i));
                return m_Heap[(int) m_Finder[(int) i]].m_Elem;
            }
        }

        public void Lock() // Pre: (! locked())   Post: (locked())
        {
            Debug.Assert(!Locked);
            m_Locked = true;
        }

        public uint Push(uint Elem) // Pre: (! locked())
        {
            Debug.Assert(!Locked);

            uint Id = Size;
            m_Finder.Add(Id);
            m_Heap.Add(new Linker(Elem, Id));
            Adjust(Id);

            return Id;
        }

        public void Pop() // Pre: (locked() && ! empty())
        {
            Debug.Assert(Locked);
            Debug.Assert(!Empty);

            Swap(0, (int) Size - 1);
            m_Heap.RemoveAt(m_Heap.Count - 1);

            if (!Empty)
            {
                Adjust(0);
            }
        }

        public void Erase(uint i) // Pre: (locked() && ! removed(i))
        {
            Debug.Assert(Locked);
            Debug.Assert(!Removed(i));

            uint j = m_Finder[(int) i];
            Swap((int) j, (int) Size - 1);
            m_Heap.RemoveAt(m_Heap.Count - 1);

            if (j != Size)
            {
                Adjust(j);
            }
        }

        public void Update(uint i, uint Elem) // Pre: (locked() && ! removed(i))
        {
            Debug.Assert(Locked);
            Debug.Assert(!Removed(i));

            uint j = m_Finder[(int) i];
            m_Heap[(int) j].m_Elem = Elem;
            Adjust(j);
        }

        protected void Adjust(uint z)
        {
            int i = (int) z;

            Debug.Assert(i < m_Heap.Count);

            int j;

            // Check the upper part of the heap
            for (j = i; j > 0 && Comp(m_Heap[(j - 1) / 2], m_Heap[j]); j = (j - 1) / 2)
            {
                Swap(j, (j - 1) / 2);
            }

            // Check the lower part of the heap
            for (i = j; (j = 2 * i + 1) < Size; i = j)
            {
                if (j + 1 < Size && Comp(m_Heap[j], m_Heap[j + 1]))
                {
                    ++j;
                }

                if (Comp(m_Heap[j], m_Heap[i]))
                {
                    return;
                }

                Swap(i, j);
            }
        }

        protected void Swap(int a, int b)
        {
            Linker r = m_Heap[b];
            m_Heap[b] = m_Heap[a];
            m_Heap[a] = r;

            m_Finder[(int) m_Heap[a].m_Index] = (uint) a;
            m_Finder[(int) m_Heap[b].m_Index] = (uint) b;
        }

        protected bool Comp(Linker a, Linker b)
        {
            switch (m_Compare)
            {
                case CompareType.Less:
                    return a.m_Elem < b.m_Elem;
                default:
                    return a.m_Elem > b.m_Elem;
            }
        }
    }
}