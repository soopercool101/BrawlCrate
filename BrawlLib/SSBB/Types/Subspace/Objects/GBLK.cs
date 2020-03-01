using BrawlLib.Internal;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    public unsafe struct GBLKEntry
    {
        private VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }
}