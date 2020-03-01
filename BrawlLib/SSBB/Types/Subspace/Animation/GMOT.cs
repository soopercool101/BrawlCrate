using BrawlLib.Internal;

namespace BrawlLib.SSBB.Types.Subspace.Animation
{
    public unsafe struct GMOTEntry
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