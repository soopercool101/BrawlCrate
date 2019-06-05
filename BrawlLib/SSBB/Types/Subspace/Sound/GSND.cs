using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GSND
    {
        public const uint Tag = 0x444E5347;
        public const int Size = 0x14;
        public uint _tag;
        public bint _count;

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

        public GSND(int count)
        {
            _tag = Tag;
            _count = count;
        }
    }

    public unsafe struct GSNDEntry
    {
        public bint _infoIndex;
        public uint unk0;
        public int _pad0;
        public int _pad1;
        public bfloat _unkFloat0;
        public bfloat _unkFloat1;
        public int _pad2;
        private fixed sbyte _name[32];
        private fixed byte _Trigger[4];
        private fixed int _pad4[16];

        public GSNDEntry(float UnkFloat0, float UnkFloat1, string trigger, string name)
        {
            _infoIndex = 0;
            _pad0 = _pad1 = _pad2 = 0;
            _unkFloat0 = UnkFloat0;
            _unkFloat1 = UnkFloat1;
            unk0 = 0x00000001;
            Name = name;
            Trigger = trigger;
            Pad4 = 0;
        }

        public string Name
        {
            get => new string((sbyte*) Address + 0x1C);
            set
            {
                if (value == null) value = "";

                var i = 0;
                while (i < 31 && i < value.Length) _name[i] = (sbyte) value[i++];

                while (i < 32) _name[i++] = 0;
            }
        }

        public string Trigger
        {
            get
            {
                var bytes = new byte[4];
                var s1 = "";
                for (var i = 0; i < 4; i++)
                {
                    bytes[i] = *(byte*) (Address + 0x3C + i);
                    if (bytes[i].ToString("x").Length < 2)
                        s1 += bytes[i].ToString("x").PadLeft(2, '0');
                    else
                        s1 += bytes[i].ToString("x").ToUpper();
                }

                return s1;
            }
            set
            {
                if (value == null) value = "";

                for (var i = 0; i < value.Length; i++) _Trigger[i / 2] = Convert.ToByte(value.Substring(i++, 2), 16);
            }
        }

        public int Pad4
        {
            set
            {
                for (var i = 0; i <= 16; i++) _pad4[i] = 0;
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