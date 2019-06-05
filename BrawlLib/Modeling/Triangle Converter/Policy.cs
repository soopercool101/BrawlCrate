//This has been converted from C++ and modified.
//Original source:
//http://users.telenet.be/tfautre/softdev/tristripper/


namespace BrawlLib.Modeling.Triangle_Converter
{
    public class Policy
    {
        private readonly bool m_Cache;

        private readonly uint m_MinStripSize;
        private uint m_CacheHits;
        private uint m_Degree;

        public Policy(uint MinStripSize, bool Cache)
        {
            m_MinStripSize = MinStripSize;
            m_Cache = Cache;
        }

        public Strip BestStrip { get; private set; } = new Strip();

        public void Challenge(Strip Strip, uint Degree, uint CacheHits)
        {
            if (Strip.Size < m_MinStripSize) return;

            if (!m_Cache)
            {
                //Cache is disabled, take the longest strip
                if (Strip.Size > BestStrip.Size) BestStrip = Strip;
            }
            else
            {
                //Cache simulator enabled
                if (CacheHits > m_CacheHits)
                {
                    //Priority 1: Keep the strip with the best cache hit count
                    BestStrip = Strip;
                    m_Degree = Degree;
                    m_CacheHits = CacheHits;
                }
                else if (CacheHits == m_CacheHits &&
                         (BestStrip.Size != 0 && Degree < m_Degree || Strip.Size > BestStrip.Size))
                {
                    //Priority 2: Keep the strip with the loneliest start triangle
                    //Priority 3: Keep the longest strip
                    BestStrip = Strip;
                    m_Degree = Degree;
                }
            }
        }
    }
}