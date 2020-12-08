using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Animation
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GLK2Entry
    {
        public static readonly int SIZE = 0x18;

        public bint _unknown0x00;
        public byte _modelDataID;
        public byte _unkflag2;
        public byte _unkflag3;
        public byte _unkflag4;
        public buint _trigger1;
        public buint _trigger2;
        public buint _trigger3;
        public buint _trigger4;
    }
}