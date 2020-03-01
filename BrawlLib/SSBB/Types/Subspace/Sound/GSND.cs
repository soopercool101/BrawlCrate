using BrawlLib.Internal;
using System;

namespace BrawlLib.SSBB.Types.Subspace.Sound
{
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
                if (value == null)
                {
                    value = "";
                }

                fixed (sbyte* ptr = _name)
                {
                    int i = 0;
                    while (i < 31 && i < value.Length)
                    {
                        ptr[i] = (sbyte) value[i++];
                    }

                    while (i < 32)
                    {
                        ptr[i++] = 0;
                    }
                }
            }
        }

        public string Trigger
        {
            get
            {
                byte[] bytes = new byte[4];
                string s1 = "";
                for (int i = 0; i < 4; i++)
                {
                    bytes[i] = *(byte*) (Address + 0x3C + i);
                    if (bytes[i].ToString("x").Length < 2)
                    {
                        s1 += bytes[i].ToString("x").PadLeft(2, '0');
                    }
                    else
                    {
                        s1 += bytes[i].ToString("x").ToUpper();
                    }
                }

                return s1;
            }
            set
            {
                if (value == null)
                {
                    value = "";
                }

                fixed (byte* ptr = _Trigger)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        ptr[i / 2] = Convert.ToByte(value.Substring(i++, 2), 16);
                    }
                }
            }
        }

        public int Pad4
        {
            set
            {
                fixed (int* ptr = _pad4)
                {
                    for (int i = 0; i <= 16; i++)
                    {
                        ptr[i] = 0;
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