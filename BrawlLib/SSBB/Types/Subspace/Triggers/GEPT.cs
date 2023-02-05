using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Triggers
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GEPTEntry
    {
        public const int Size = 0x1C;

        public buint _trigger1; // 0x00
        public buint _trigger2; // 0x04
        public buint _trigger3; // 0x08
        public buint _trigger4; // 0x0C
        public buint _trigger5; // 0x10
        public buint _trigger6; // 0x14
        public buint _trigger7; // 0x18
    }
}
