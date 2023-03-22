using BrawlLib.Internal;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Navigation
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ADSJ
    {
        public const uint Tag = 0x4A534441;
        public const int Size = 0x10;

        public uint _tag;
        public bint _count;
        public int _pad0;
        public int _pad1;

        public ADSJ(int Count)
        {
            _tag = Tag;
            _count = Count;
            _pad0 = _pad1 = 0;
        }

        public VoidPtr this[int index] => (byte*) Address + Offsets(index);

        public uint Offsets(int index)
        {
            return *(buint*) ((byte*) Address + 0x10 + index * 4);
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

    public unsafe struct ADSJEntry
    {
        public const int Size = 0x2C;
        private fixed byte _doorID[4];
        public byte _unk0;
        public byte _unk1;
        public byte _unk2;
        public sbyte _unk3;
        private fixed byte _sendingID[4];
        private fixed sbyte _jumpBone[0x20];

        public string DoorID
        {
            get
            {
                byte[] bytes = new byte[4];
                string s1 = "";
                for (int i = 0; i < 4; i++)
                {
                    bytes[i] = *(byte*) (Address + i);
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

                fixed (byte* ptr = _doorID)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        ptr[i / 2] = Convert.ToByte(value.Substring(i++, 2), 16);
                    }
                }
            }
        }

        public string SendStage
        {
            get
            {
                byte[] bytes = new byte[4];
                string s1 = "";
                for (int i = 0; i < 4; i++)
                {
                    bytes[i] = *(byte*) (Address + 0x08 + i);
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

                fixed (byte* ptr = _sendingID)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        ptr[i / 2] = Convert.ToByte(value.Substring(i++, 2), 16);
                    }
                }
            }
        }

        public string JumpBone
        {
            get => Address.GetUTF8String(0x0C, 0x20);
            set => Address.WriteUTF8String(value, true, 0x0C, 0x20);
        }

        public ADSJEntry(string Stage, string SendingID, string Bone)
        {
            _unk0 = _unk1 = _unk2 = 0;
            _unk3 = 0;
            DoorID = Stage;
            SendStage = SendingID;
            JumpBone = Bone;
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