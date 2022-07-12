using BrawlLib.Internal;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Sound
{
    //Alot of this was reused from STPM
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BGMG
    {
        public const uint Tag = 0x474D4742;
        public const int Size = 0x10;

        public uint _tag;
        public bint _count;
        public bint _unk0;
        public int _pad1;

        public BGMG(int count)
        {
            _tag = Tag;
            _count = count;
            _unk0 = 0x80;
            _pad1 = 0x00;
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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BGMGEntry
    {
        public const int Size = 0x10;
        private fixed byte _stageID[4];
        public bint _songID;
        public bint _volume;
        public bfloat _playOffsetFrame;

        public BGMGEntry(string ID, int SongID, int Volume)
        {
            _songID = SongID;
            _volume = Volume;
            _playOffsetFrame = 0;
            StageID = ID;
        }

        public BGMGEntry(string ID, int SongID, int Volume, float playOffset)
        {
            _songID = SongID;
            _volume = Volume;
            _playOffsetFrame = playOffset;
            StageID = ID;
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

        public string StageID
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

                fixed (byte* ptr = _stageID)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        ptr[i / 2] = Convert.ToByte(value.Substring(i++, 2), 16);
                    }
                }
            }
        }
    }
}