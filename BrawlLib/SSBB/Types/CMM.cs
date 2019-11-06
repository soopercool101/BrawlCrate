using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CMMEntry
    {
        public const int Size = 0x8;

        public buint _songID;
        public bshort _unknown;     // 0x04 - Unknown. May be 2 separate bytes?
        public byte _trackListID;   // 0x07 - Set by parent
        public byte _sliderSetting; // 0x08 - 0-64; can be edited and saved in-game
    }
}