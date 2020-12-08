using BrawlLib.Internal;
using BrawlLib.Wii.Animations;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Animations
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SRT0v4
    {
        public const string Tag = "SRT0";
        public const int Size = 0x28;

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _stringOffset;
        public bint _origPathOffset;
        public bushort _numFrames;
        public bushort _numEntries;
        public bint _matrixMode;
        public bint _loop;

        public SRT0v4(ushort frames, bool loop, ushort entries, int matrixMode)
        {
            _header._tag = Tag;
            _header._size = Size;
            _header._version = 4;
            _header._bresOffset = 0;

            _dataOffset = Size;
            _origPathOffset = 0;
            _matrixMode = matrixMode;
            _numFrames = frames;
            _loop = loop ? 1 : 0;
            _stringOffset = 0;
            _numEntries = entries;
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

        public ResourceGroup* Group => (ResourceGroup*) (Address + _dataOffset);

        public string OrigPath => new string((sbyte*) OrigPathAddress);

        public VoidPtr OrigPathAddress
        {
            get => Address + _origPathOffset;
            set => _origPathOffset = (int) value - (int) Address;
        }

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SRT0v5
    {
        public const string Tag = "SRT0";
        public const int Size = 0x2C;

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _userDataOffset;
        public bint _stringOffset;
        public bint _origPathOffset;
        public bushort _numFrames;
        public bushort _numEntries;
        public bint _matrixMode;
        public bint _loop;

        public SRT0v5(ushort frames, bool loop, ushort entries, int matrixMode)
        {
            _header._tag = Tag;
            _header._size = Size;
            _header._version = 5;
            _header._bresOffset = 0;

            _dataOffset = Size;
            _userDataOffset = _origPathOffset = 0;
            _matrixMode = matrixMode;
            _numFrames = frames;
            _loop = loop ? 1 : 0;
            _stringOffset = 0;
            _numEntries = entries;
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

        public ResourceGroup* Group => (ResourceGroup*) (Address + _dataOffset);

        public VoidPtr UserData
        {
            get => _userDataOffset == 0 ? null : Address + _userDataOffset;
            set => _userDataOffset = (int) value - (int) Address;
        }

        public string OrigPath => new string((sbyte*) OrigPathAddress);

        public VoidPtr OrigPathAddress
        {
            get => Address + _origPathOffset;
            set => _origPathOffset = (int) value - (int) Address;
        }

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SRT0Entry
    {
        public bint _stringOffset;
        public bint _textureIndices; //Sets which of the 8 texure references to animate with bits
        public bint _indirectIndices;

        //Entry offsets here for each texture

        public SRT0Entry(int textureIndices, int indirectIndices)
        {
            _textureIndices = textureIndices;
            _indirectIndices = indirectIndices;
            _stringOffset = 0;
        }

        public int DataSize()
        {
            int size = 12, index = 0;
            for (int i = 0; i < 8; i++)
            {
                if (((_textureIndices >> i) & 1) != 0)
                {
                    size += 4 + GetEntry(index++)->Code.DataSize();
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (((_indirectIndices >> i) & 1) != 0)
                {
                    size += 4 + GetEntry(index++)->Code.DataSize();
                }
            }

            return size;
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

        public SRT0TextureEntry* GetEntry(int index)
        {
            return (SRT0TextureEntry*) (Address + GetOffset(index));
        }

        public void SetEntry(int index, SRT0TextureEntry value)
        {
            *(SRT0TextureEntry*) (Address + GetOffset(index)) = value;
        }

        public int GetOffset(int index)
        {
            return *(bint*) (Address + 12 + index * 4);
        }

        public void SetOffset(int index, int value)
        {
            *(bint*) (Address + 12 + index * 4) = value;
        }

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [Flags]
    public enum TextureIndices
    {
        Texture0 = 0x01,
        Texture1 = 0x02,
        Texture2 = 0x04,
        Texture3 = 0x08,
        Texture4 = 0x10,
        Texture5 = 0x20,
        Texture6 = 0x40,
        Texture7 = 0x80
    }

    [Flags]
    public enum IndirectTextureIndices
    {
        Indirect0 = 0x01,
        Indirect1 = 0x02,
        Indirect2 = 0x04
    }

    [Flags]
    public enum IndTextureIndices
    {
        IndirectTexture0 = 0x01,
        IndirectTexture1 = 0x02,
        IndirectTexture2 = 0x04
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SRT0EntryType2
    {
        private bint _unk1; //entry count?
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SRT0TextureEntry
    {
        public buint _code;

        //These are either a float value or int offset, in this order:
        //Scale X
        //Scale Y
        //Rotation
        //X Trans
        //Y Trans

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

        public VoidPtr Data => Address + 4;

        public SRT0Code Code
        {
            get => new SRT0Code {_data = (uint) _code};
            set => _code = (uint) value._data;
        }

        //Uses same header as CHR0 animations
        public I12Header* Entry(int index)
        {
            return (I12Header*) (Address + 4 + 4 * index + GetOffset(index));
        }

        public float GetValue(int index)
        {
            return *(bfloat*) (Address + 4 + 4 * index);
        }

        public void SetValue(int index, float value)
        {
            *(bfloat*) (Address + 4 + 4 * index) = value;
        }

        public int GetOffset(int index)
        {
            return *(bint*) (Address + 4 + 4 * index);
        }

        public void SetOffset(int index, int value)
        {
            *(bint*) (Address + 4 + 4 * index) = value;
        }
    }

    public struct SRT0Code
    {
        public static SRT0Code Default = new SRT0Code {_data = 0x3FF};

        //0000 0000 0000 0000 0000 0000 0000 0001       Always set

        //0000 0000 0000 0000 0000 0000 0000 0010       Scale One
        //0000 0000 0000 0000 0000 0000 0000 0100       Rot Zero
        //0000 0000 0000 0000 0000 0000 0000 1000       Trans Zero
        //0000 0000 0000 0000 0000 0000 0001 0000        Scale Isotropic

        //0000 0000 0000 0000 0000 0000 0010 0000        Fixed Scale X
        //0000 0000 0000 0000 0000 0000 0100 0000        Fixed Scale Y
        //0000 0000 0000 0000 0000 0000 1000 0000        Fixed Rotation
        //0000 0000 0000 0000 0000 0001 0000 0000        Fixed X Translation
        //0000 0000 0000 0000 0000 0010 0000 0000        Fixed Y Translation

        public Bin32 _data;

        public bool AlwaysOn
        {
            get => _data[0];
            set => _data[0] = value;
        }

        public bool NoScale
        {
            get => _data[1];
            set => _data[1] = value;
        }

        public bool NoRotation
        {
            get => _data[2];
            set => _data[2] = value;
        }

        public bool NoTranslation
        {
            get => _data[3];
            set => _data[3] = value;
        }

        public bool ScaleIsotropic
        {
            get => _data[4];
            set => _data[4] = value;
        }

        public bool FixedScaleX
        {
            get => _data[5];
            set => _data[5] = value;
        }

        public bool FixedScaleY
        {
            get => _data[6];
            set => _data[6] = value;
        }

        public bool FixedRotation
        {
            get => _data[7];
            set => _data[7] = value;
        }

        public bool FixedX
        {
            get => _data[8];
            set => _data[8] = value;
        }

        public bool FixedY
        {
            get => _data[9];
            set => _data[9] = value;
        }

        public bool GetHas(int i)
        {
            return _data[i + 1] != true;
        }

        public void SetHas(int index, bool p)
        {
            _data[index + 1] = !p;
        }

        public bool GetFixed(int i)
        {
            return _data[i + 5];
        }

        public void SetFixed(int index, bool p)
        {
            _data[index + 5] = p;
        }

        public int DataSize()
        {
            int val = 4;
            for (int i = 0; i < 3; i++)
            {
                if (GetHas(i))
                {
                    if (i == 1 || i == 2 && ScaleIsotropic)
                    {
                        val += 4;
                    }
                    else
                    {
                        val += 8;
                    }
                }
            }

            return val;
        }

        public override string ToString()
        {
            string val = "None";
            if (AlwaysOn)
            {
                val = "Enabled";
            }

            if (!NoScale)
            {
                val += ", Has ";
            }
            else
            {
                val += ", No ";
            }

            val += "Scale";

            if (!NoRotation)
            {
                val += ", Has ";
            }
            else
            {
                val += ", No ";
            }

            val += "Rotation";

            if (!NoTranslation)
            {
                val += ", Has ";
            }
            else
            {
                val += ", No ";
            }

            val += "Translation";

            if (ScaleIsotropic)
            {
                val += ", Scale is isotropic";
            }

            if (FixedScaleX)
            {
                val += ", Fixed ";
            }
            else
            {
                val += ", Unfixed ";
            }

            val += "Scale X";

            if (FixedScaleY)
            {
                val += ", Fixed ";
            }
            else
            {
                val += ", Unfixed ";
            }

            val += "Scale Y";

            if (FixedRotation)
            {
                val += ", Fixed ";
            }
            else
            {
                val += ", Unfixed ";
            }

            val += "Rotation";

            if (FixedX)
            {
                val += ", Fixed ";
            }
            else
            {
                val += ", Unfixed ";
            }

            val += "X";

            if (FixedY)
            {
                val += ", Fixed ";
            }
            else
            {
                val += ", Unfixed ";
            }

            val += "Y";

            return val;
        }
    }
}