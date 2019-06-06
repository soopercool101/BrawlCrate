//This has been converted from C++ and modified.
//Original source:
//http://users.telenet.be/tfautre/softdev/tristripper/


namespace BrawlLib.Modeling.Triangle_Converter
{
    public class Policy
    {
        public Policy(uint MinStripSize, bool Cache)
        {
            m_MinStripSize = MinStripSize;
            m_Cache = Cache;
        }

        public Strip BestStrip => m_Strip;

        public void Challenge(Strip Strip, uint Degree, uint CacheHits)
        {
            if (Strip.Size < m_MinStripSize)
            {
                return;
            }

            if (!m_Cache)
            {
                //Cache is disabled, take the longest strip
                if (Strip.Size > m_Strip.Size)
                {
                    m_Strip = Strip;
                }
            }
            else
            {
                //Cache simulator enabled
                if (CacheHits > m_CacheHits)
                {
                    //Priority 1: Keep the strip with the best cache hit count
                    m_Strip = Strip;
                    m_Degree = Degree;
                    m_CacheHits = CacheHits;
                }
                else if (CacheHits == m_CacheHits &&
                         (m_Strip.Size != 0 && Degree < m_Degree || Strip.Size > m_Strip.Size))
                {
                    //Priority 2: Keep the strip with the loneliest start triangle
                    //Priority 3: Keep the longest strip
                    m_Strip = Strip;
                    m_Degree = Degree;
                }
            }
        }

        private Strip m_Strip = new Strip();
        private uint m_Degree;
        private uint m_CacheHits;

        private readonly uint m_MinStripSize;
        private readonly bool m_Cache;
    }
}