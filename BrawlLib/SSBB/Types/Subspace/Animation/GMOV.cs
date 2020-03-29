using BrawlLib.Internal;

namespace BrawlLib.SSBB.Types.Subspace.Animation
{
    public unsafe struct GMOVEntry
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