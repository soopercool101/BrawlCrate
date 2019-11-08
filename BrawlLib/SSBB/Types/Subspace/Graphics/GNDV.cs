using BrawlLib.Internal;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Graphics
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GNDV
    {
        public const uint TAG = 0x56444E47; // GNDV
        public const int SIZE = 0x08;

        public uint _tag;
        public bint _entryCount;

        public VoidPtr this[int index] => (byte*) Address + Offsets(index);

        public uint Offsets(int index)
        {
            return *(buint*) ((byte*) Address + 0x08 + index * 4);
        }

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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GNDVEntry
    {
        public const int SIZE = 0x30;

        public buint _unk1;
        private fixed sbyte _boneName[0x20];
        public bint _sfx;
        public buint _gfx;
        public buint _triggerID;

        public string BoneName
        {
            get => new string((sbyte*) Address + 0x4);
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                fixed (sbyte* ptr = _boneName)
                {
                    int i = 0;
                    while (i <= 0x29 && i < value.Length)
                    {
                        ptr[i] = (sbyte) value[i++];
                    }

                    while (i <= 0x30)
                    {
                        ptr[i++] = 0;
                    }
                }
            }
        }

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