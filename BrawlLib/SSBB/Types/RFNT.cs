using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RFNT
    {
        private NW4RCommonHeader _header;
    }
}