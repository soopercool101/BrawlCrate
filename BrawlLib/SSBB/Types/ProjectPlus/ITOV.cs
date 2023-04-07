using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.ProjectPlus
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ITOV
    {
        public const int Size = 0x1C;
        public const string Tag = "ITOV";

        public buint _tag;
        public fixed byte _commonOverride[12];
        public fixed byte _pokeAssistOverride[12];

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

        public string CommonOverride
        {
            get => Address.GetUTF8String(0x4, 12);
            set => Address.WriteUTF8String(value, true, 0x4, 12);
        }

        public string PokeAssistOverride
        {
            get => Address.GetUTF8String(0x10, 12);
            set => Address.WriteUTF8String(value, true, 0x10, 12);
        }
    }
}
