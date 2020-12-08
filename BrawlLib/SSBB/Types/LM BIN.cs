using BrawlLib.Internal;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    //Luigi's Mansion Bin File

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct LMBin
    {
        public const int Size = 0x60;

        public byte _unk1;
        public fixed byte _name[11];
        public buint _textureOffset;
        public buint _materialOffset;
        public buint _vertexOffset;
        public buint _normalOffset;
        public buint _unk4; //0
        public buint _unk5; //0
        public buint _texCoord0Offset;
        public buint _texCoord1Offset; //Usually 0
        public buint _unk8;            //0
        public buint _unk9;            //0
        public buint _shaderOffset;
        public buint _batchOffset;
        public buint _bonesOffset;
        public buint _unk13; //0
        public buint _unk14; //0
        public buint _unk15; //0
        public buint _unk16; //0
        public buint _unk17; //0
        public buint _unk18; //0
        public buint _unk19; //0
        public buint _unk20; //0

        public LMBinTexture* Textures => (LMBinTexture*) (Address + _textureOffset);

        public string Name => new string((sbyte*) Address + 1);

        private VoidPtr Address
        {
            get
            {
                fixed (void* p = &this)
                {
                    return p;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LMBinTexture
    {
        public const int Size = 0xC;

        public bshort _width;
        public bshort _height;
        public byte _format;
        public byte _unk1;
        public bshort _unk2;
        public buint _dataOffset;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct LMBinBone
    {
        public const int Size = 108;

        public bshort _parentIndex;
        public bshort _childIndex;
        public bshort _nextIndex;
        public bshort _prevIndex;
        public byte _pad1;
        public byte _renderFlags;
        public bshort _pad2;
        public BVec3 _scale;
        public BVec3 _rot;
        public BVec3 _trans;
        public BVec3 _boxMin;
        public BVec3 _boxMax;
        public bfloat _unk1;
        public bshort _count;
        public bshort _pad3;
        public bint _num2;
        public fixed uint _pad4[7];

        private VoidPtr Address
        {
            get
            {
                fixed (void* p = &this)
                {
                    return p;
                }
            }
        }
    }

    [Flags]
    internal enum LMBinBoneRenderFlags : byte
    {
        Ceiling = 0x80,
        FourthWall = 4,
        FullBright = 0x40,
        Transparent = 8
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LMBinPart
    {
        public const int Size = 4;

        public bshort _shaderIndex;
        public bshort _batchIndex;
    }
}