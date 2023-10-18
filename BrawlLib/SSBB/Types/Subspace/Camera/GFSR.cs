using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Camera
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GFSREntry
    {
        public const int Size = 0x50;

        public bfloat _unknown0x00;
        public byte _unknown0x04;
        public bool8 _unknown0x05;
        public bool8 _unknown0x06;
        public bool8 _unknown0x07;
        public TriggerData _unknown0x08;
        public TriggerData _unknown0x0C;
        public buint _unknown0x10;
        public DifficultyRatios _difficultyRatio; // 0x14
    }
}
