//This has been converted from C++ and modified.
//Original source:
//http://users.telenet.be/tfautre/softdev/tristripper/

using BrawlLib.Modeling.Triangle_Converter.Deque;
using System;

namespace BrawlLib.Modeling.Triangle_Converter
{
    internal class CacheSimulator
    {
        protected Deque<uint> m_Cache;
        protected uint m_NbHits;
        protected bool m_PushHits;

        public CacheSimulator()
        {
            m_NbHits = 0;
            m_PushHits = true;
            m_Cache = new Deque<uint>();
        }

        public void Clear()
        {
            ResetHitCount();
            m_Cache.Clear();
        }

        public void Resize(uint Size)
        {
            //m_Cache.Resize(Size, uint.MaxValue);

            m_Cache.Clear();
            for (int x = 0; x < Size; x++)
            {
                m_Cache.PushFront(uint.MaxValue);
            }
        }

        public void Reset()
        {
            m_Cache.Clear();

            for (int x = 0; x < m_Cache.Count; x++)
            {
                m_Cache.PushFront(uint.MaxValue);
            }

            ResetHitCount();
        }

        public void PushCacheHits(bool Enabled = true)
        {
            m_PushHits = Enabled;
        }

        public uint Size => (uint) m_Cache.Count;

        public void Push(uint i, bool CountCacheHit = false)
        {
            if ((CountCacheHit || m_PushHits) && m_Cache.Contains(i))
            {
                // Should we count the cache hits?
                if (CountCacheHit)
                {
                    m_NbHits++;
                }

                // Should we not push the index into the cache if it's a cache hit?
                if (!m_PushHits)
                {
                    return;
                }
            }

            // Manage the indices cache as a FIFO structure
            m_Cache.PushFront(i);
            m_Cache.PopBack();
        }

        public void Merge(CacheSimulator Backward, uint PossibleOverlap)
        {
            uint Overlap = Math.Min(PossibleOverlap, Size);

            for (uint i = 0; i < Overlap; ++i)
            {
                Push(Backward.m_Cache[i], true);
            }

            m_NbHits += Backward.m_NbHits;
        }

        public void ResetHitCount()
        {
            m_NbHits = 0;
        }

        public uint HitCount => m_NbHits;
    }
}