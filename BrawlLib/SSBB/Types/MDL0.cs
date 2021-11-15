using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Graphics;
using BrawlLib.Wii.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Header
    {
        public const uint Size = 16;
        public const string Tag = "MDL0";

        public BRESCommonHeader _header;

        public MDL0Header(int length, int version)
        {
            _header._tag = Tag;
            _header._size = length;
            _header._version = version;
            _header._bresOffset = 0;
        }

        internal byte* Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return (byte*) ptr;
                }
            }
        }

        public bint* Offsets => (bint*) (Address + 0x10);

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + StringOffset;
            set => StringOffset = (int) value - (int) Address;
        }

        public ResourceGroup* GetEntry(int index)
        {
            int offset = Offsets[index];
            if (offset == 0)
            {
                return null;
            }

            return (ResourceGroup*) (Address + offset);
        }

        public VoidPtr UserData => UserDataOffset > 0 ? Address + UserDataOffset : null;

        public int UserDataOffset
        {
            get
            {
                switch (_header._version)
                {
                    //case 0x08:
                    //case 0x09:
                    //    return *(bint*)(Address + 0x38);
                    case 0x0A:
                        return *(bint*) (Address + 0x40);
                    case 0x0B:
                        return *(bint*) (Address + 0x44);
                    default:
                        return 0;
                }
            }
            set
            {
                switch (_header._version)
                {
                    //case 0x08:
                    //case 0x09:
                    //    *(bint*)(Address + 0x38) = value; break;
                    case 0x0A:
                        *(bint*) (Address + 0x40) = value;
                        break;
                    case 0x0B:
                        *(bint*) (Address + 0x44) = value;
                        break;
                }
            }
        }

        public int StringOffset
        {
            get
            {
                switch (_header._version)
                {
                    default:
                    case 0x08:
                    case 0x09:
                        return *(bint*) (Address + 0x3C);
                    case 0x0A:
                        return *(bint*) (Address + 0x44);
                    case 0x0B:
                        return *(bint*) (Address + 0x48);
                }
            }
            set
            {
                switch (_header._version)
                {
                    case 0x08:
                    case 0x09:
                        *(bint*) (Address + 0x3C) = value;
                        break;
                    case 0x0A:
                        *(bint*) (Address + 0x44) = value;
                        break;
                    case 0x0B:
                        *(bint*) (Address + 0x48) = value;
                        break;
                }
            }
        }

        public MDL0Props* Properties
        {
            get
            {
                switch (_header._version)
                {
                    case 0x08:
                    case 0x09:
                        return (MDL0Props*) (Address + 0x40);
                    case 0x0A:
                        return (MDL0Props*) (Address + 0x48);
                    case 0x0B:
                        return (MDL0Props*) (Address + 0x4C);
                    default:
                        return null;
                }
            }
            set
            {
                switch (_header._version)
                {
                    case 0x08:
                    case 0x09:
                        *(MDL0Props*) (Address + 0x40) = *value;
                        break;
                    case 0x0A:
                        *(MDL0Props*) (Address + 0x48) = *value;
                        break;
                    case 0x0B:
                        *(MDL0Props*) (Address + 0x4C) = *value;
                        break;
                }
            }
        }

        public void* GetResource(MDLResourceType type, int entryId)
        {
            if (entryId < 0)
            {
                return null;
            }

            int groupId = ModelLinker.IndexBank[_header._version].IndexOf(type);
            if (groupId < 0)
            {
                return null;
            }

            byte* addr;
            fixed (void* p = &this)
            {
                addr = (byte*) p;
            }

            int offset = *((bint*) addr + 4 + groupId);
            if (offset > 0)
            {
                ResourceGroup* pGroup = (ResourceGroup*) (addr + offset);
                return (byte*) pGroup + (&pGroup->_first)[entryId + 1]._dataOffset;
            }

            return null;
        }
    }

    public enum MDLScalingRule
    {
        Standard = 0,
        SoftImage = 1,
        Maya = 2
    }

    public enum TexMatrixMode
    {
        MatrixMaya = 0,
        MatrixXSI = 1,
        Matrix3dsMax = 2
    }

    //Calculation method for the normal matrix and texture matrix that makes up an envelope
    public enum MDLEnvelopeMatrixMode
    {
        Normal = 0,
        Approximate = 1,
        Exact = 2
    }

    //Immediately after header, separate entity
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Props
    {
        public const uint Size = 0x40;

        public buint _headerLen; //0x40
        public bint _mdl0Offset;
        public bint _scalingRule;
        public bint _texMatrixMode;
        public bint _numVertices;
        public bint _numTriangles;
        public bint _origPathOffset;
        public bint _numNodes;
        public byte _needNrmMtxArray;
        public byte _needTexMtxArray;
        public byte _enableExtents;
        public byte _envMtxMode;
        public buint _dataOffset; //0x40
        public BBox _extents;

        public MDL0Props(int version, int vertices, int faces, int nodes, int scalingRule, int texMtxMode,
                         bool needsNrmArr, bool needsTexArr, bool enableExtents, byte envMtxMode, Vector3 min,
                         Vector3 max)
        {
            _headerLen = 0x40;
            if (version == 9 || version == 8)
            {
                _mdl0Offset = -64;
            }
            else
            {
                _mdl0Offset = -76;
            }

            _scalingRule = scalingRule;
            _texMatrixMode = texMtxMode;
            _numVertices = vertices;
            _numTriangles = faces;
            _origPathOffset = 0;
            _numNodes = nodes;
            _needNrmMtxArray = (byte) (needsNrmArr ? 1 : 0);
            _needTexMtxArray = (byte) (needsTexArr ? 1 : 0);
            _enableExtents = (byte) (enableExtents ? 1 : 0);
            _envMtxMode = envMtxMode;
            _dataOffset = 0x40;
            _extents = new BBox(min, max);
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

        public MDL0Header* MDL0 => (MDL0Header*) (Address + _mdl0Offset);

        public string OrigPath => new string((sbyte*) OrigPathAddress);

        public VoidPtr OrigPathAddress
        {
            get => Address + _origPathOffset;
            set => _origPathOffset = (int) value - (int) Address;
        }

        public MDL0NodeTable* IndexTable => (MDL0NodeTable*) (Address + _dataOffset);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0CommonHeader
    {
        public bint _size;
        public bint _mdlOffset;

        internal void* Address
        {
            get
            {
                fixed (void* p = &this)
                {
                    return p;
                }
            }
        }

        public MDL0Header* MDL0Header => (MDL0Header*) ((byte*) Address + _mdlOffset);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NodeTable
    {
        public bint _numEntries;

        private void* Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public bint* First => (bint*) Address + 1;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0DefEntry
    {
        public byte _type;

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

        public MDL0NodeType2* Type2Data => (MDL0NodeType2*) (Address + 1);
        public MDL0NodeType3* Type3Data => (MDL0NodeType3*) (Address + 1);
        public MDL0NodeType4* Type4Data => (MDL0NodeType4*) (Address + 1);
        public MDL0NodeType5* Type5Data => (MDL0NodeType5*) (Address + 1);

        public MDL0DefEntry* Next
        {
            get
            {
                switch (_type)
                {
                    case 2: return (MDL0DefEntry*) (Type2Data + 1);
                    case 3: return (MDL0DefEntry*) (Type3Data + 1);
                    case 4: return (MDL0DefEntry*) (Type4Data + 1);
                    case 5: return (MDL0DefEntry*) (Type5Data + 1);
                }

                return null;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public abstract unsafe class MDL0NodeClass
    {
        public static object Create(ref VoidPtr addr)
        {
            object n = null;
            byte oldByte = addr.Byte;
            addr += 1;

            switch (oldByte)
            {
                case 2:
                    n = Marshal.PtrToStructure(addr, typeof(MDL0Node2Class));
                    addr += MDL0Node2Class.Size;
                    break;
                case 3:
                    n = new MDL0Node3Class((MDL0NodeType3*) addr);
                    addr += ((MDL0Node3Class) n).GetSize();
                    break;
                case 4:
                    n = Marshal.PtrToStructure(addr, typeof(MDL0NodeType4));
                    addr += MDL0NodeType4.Size;
                    break;
                case 5:
                    n = Marshal.PtrToStructure(addr, typeof(MDL0NodeType5));
                    addr += MDL0NodeType5.Size;
                    break;
                case 6:
                    addr += 4;
                    break;
            }

            return n;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MDL0Node2Class : MDL0NodeClass
    {
        public const uint Size = 0x04;

        public bushort _boneIndex;
        public bushort _parentNodeIndex;

        public ushort BoneIndex
        {
            get => _boneIndex;
            set => _boneIndex = value;
        }

        public ushort ParentNodeIndex
        {
            get => _parentNodeIndex;
            set => _parentNodeIndex = value;
        }

        public override string ToString()
        {
            return $"Node (Bone Index:{BoneIndex}, Parent Node Index:{ParentNodeIndex})";
        }
    }

    public unsafe class MDL0Node3Class
    {
        public bushort _id;
        public List<MDL0NodeType3Entry> _entries = new List<MDL0NodeType3Entry>();

        public MDL0Node3Class(MDL0NodeType3* ptr)
        {
            _id = ptr->_id;
            for (int i = 0; i < ptr->_numEntries; i++)
            {
                _entries.Add(ptr->Entries[i]);
            }
        }

        public ushort Id
        {
            get => _id;
            set => _id = value;
        }

        public MDL0NodeType3Entry[] Entries => _entries.ToArray();

        public int GetSize()
        {
            return 3 + _entries.Count * MDL0NodeType3Entry.Size;
        }

        public override string ToString()
        {
            return $"NodeMix (ID:{Id})";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MDL0NodeType2
    {
        public const int Size = 0x04;

        public bushort _index;
        public bushort _parentId;

        public ushort Index
        {
            get => _index;
            set => _index = value;
        }

        public ushort ParentId
        {
            get => _parentId;
            set => _parentId = value;
        }

        public override string ToString()
        {
            return $"Node (Index:{Index},ParentID:{ParentId})";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NodeType3
    {
        public const int Size = 0x03;

        public bushort _id;
        public byte _numEntries;

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

        public MDL0NodeType3Entry* Entries => (MDL0NodeType3Entry*) (Address + 3);

        public ushort Id
        {
            get => _id;
            set => _id = value;
        }

        public byte NumEntries
        {
            get => _numEntries;
            set => _numEntries = value;
        }

        public override string ToString()
        {
            return $"NodeMix (ID:{Id},Entries:{NumEntries})";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MDL0NodeType3Entry
    {
        public const int Size = 0x06;

        public bushort _id;
        public bfloat _value;

        public ushort Id
        {
            get => _id;
            set => _id = value;
        }

        public float Value
        {
            get => _value;
            set => _value = value;
        }

        public override string ToString()
        {
            return $"NodeWeight (ID:{Id},Weight:{Value})";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MDL0NodeType4
    {
        public const uint Size = 0x07;

        public bushort _materialIndex;
        public bushort _polygonIndex;
        public bushort _boneIndex;
        public byte _zIndex;

        public ushort MaterialId
        {
            get => _materialIndex;
            set => _materialIndex = value;
        }

        public ushort PolygonId
        {
            get => _polygonIndex;
            set => _polygonIndex = value;
        }

        public ushort BoneIndex
        {
            get => _boneIndex;
            set => _boneIndex = value;
        }

        public byte ZIndex
        {
            get => _zIndex;
            set => _zIndex = value;
        }

        public override string ToString()
        {
            return $"Draw (MatID:{MaterialId},PolyID:{PolygonId},BoneIndex:{BoneIndex},ZIndex:{ZIndex})";
        }
    }

    //Links node IDs with indexes
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MDL0NodeType5
    {
        public const uint Size = 0x04;

        public bushort _id;    //Node Id
        public bushort _index; //Node Index

        public int Id
        {
            get => _id;
            set => _id = (ushort) value;
        }

        public int Index
        {
            get => _index;
            set => _index = (ushort) value;
        }

        public override string ToString()
        {
            return $"EnvMtx (ID:{Id},Index:{Index})";
        }
    }

    [Flags]
    public enum BoneFlags : uint
    {
        None = 0x0,
        NoTransform = 0x1,
        FixedTranslation = 0x2,
        FixedRotation = 0x4,
        FixedScale = 0x8,
        ScaleEqual = 0x10,
        SegScaleCompApply = 0x20,
        SegScaleCompParent = 0x40,
        ClassicScaleOff = 0x80,
        Visible = 0x100,
        HasGeometry = 0x200,
        HasBillboardParent = 0x400
    }

    public enum BillboardFlags : uint
    {
        Off = 0,
        Standard,
        StandardPerspective,
        Rotation,
        RotationPerspective,
        Y,
        YPerspective
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Bone
    {
        public bint _headerLen;
        public bint _mdl0Offset;
        public bint _stringOffset;
        public bint _index;

        public bint _nodeId;
        public buint _flags;
        public buint _bbFlags;
        public buint _bbIndex;

        public BVec3 _scale;
        public BVec3 _rotation;
        public BVec3 _translation;
        public BBox _extents;

        public bint _parentOffset;
        public bint _firstChildOffset;
        public bint _nextOffset;
        public bint _prevOffset;
        public bint _userDataOffset;

        public bMatrix43 _transform;
        public bMatrix43 _transformInv;

        public MDL0Bone* Next => (MDL0Bone*) (Address + _headerLen);

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public UserData* UserDataAddress
        {
            get
            {
                if (_userDataOffset <= 0)
                {
                    return null;
                }

                return (UserData*) (Address + _userDataOffset);
            }
        }

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }

        public VoidPtr MDL0Address
        {
            get => Address + _mdl0Offset;
            set => _mdl0Offset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0VertexData
    {
        public bint _dataLen; //including header
        public bint _mdl0Offset;
        public bint _dataOffset; //0x40
        public bint _stringOffset;
        public bint _index;
        public bint _isXYZ;
        public bint _type;
        public byte _divisor;
        public byte _entryStride;
        public bushort _numVertices;
        public BVec3 _eMin;
        public BVec3 _eMax;
        public bint _pad1;
        public bint _pad2;

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

        public VoidPtr Data =>
            //if (_dataOffset != null)
            //    return Address + _dataOffset;
            //else
            Address + 0x40;

        public WiiVertexComponentType Type => (WiiVertexComponentType) (int) _type;

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NormalData
    {
        public bint _dataLen; //includes header/padding
        public bint _mdl0Offset;
        public bint _dataOffset; //0x20
        public bint _stringOffset;
        public bint _index;
        public bint _isNBT;
        public bint _type;
        public byte _divisor;
        public byte _entryStride;
        public bushort _numVertices;

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

        public VoidPtr Data => Address + _dataOffset;

        public WiiVertexComponentType Type => (WiiVertexComponentType) (int) _type;

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0ColorData
    {
        public bint _dataLen; //includes header/padding
        public bint _mdl0Offset;
        public bint _dataOffset; //0x20
        public bint _stringOffset;
        public bint _index;
        public bint _isRGBA;
        public bint _format;
        public byte _entryStride;
        public byte _pad;
        public bushort _numEntries;

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

        public VoidPtr Data => Address + _dataOffset;

        public WiiColorComponentType Type => (WiiColorComponentType) (int) _format;

        public string ResourceString => ResourceStringAddress == null ? "" : new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0UVData
    {
        public bint _dataLen; //includes header/padding
        public bint _mdl0Offset;
        public bint _dataOffset; //0x40
        public bint _stringOffset;
        public bint _index;
        public bint _isST;
        public bint _format;
        public byte _divisor;
        public byte _entryStride;
        public bushort _numEntries;
        public BVec2 _min;
        public BVec2 _max;
        public int _pad1, _pad2, _pad3, _pad4;

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

        public BVec2* Entries => (BVec2*) (Address + _dataOffset);

        public WiiVertexComponentType Type => (WiiVertexComponentType) (int) _format;

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0FurVecData
    {
        public bint _dataLen; //includes header/padding
        public bint _mdl0Offset;
        public bint _dataOffset; //0x20
        public bint _stringOffset;

        public bint _index;
        public bushort _numEntries;
        public fixed byte pad[10];

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

        public BVec3* Entries => (BVec3*) (Address + _dataOffset);

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0FurPosData
    {
        public bint _dataLen; //including header
        public bint _mdl0Offset;
        public bint _dataOffset; //0x40
        public bint _stringOffset;

        public bint _index;
        public bint _isXYZ;
        public bint _type;
        public byte _divisor;
        public byte _entryStride;
        public bshort _numVertices;

        public bint _numLayers;
        public bint _offsetOfLayer;
        public fixed byte pad[24];

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

        public VoidPtr Data => Address + _dataOffset;

        public WiiVertexComponentType Type => (WiiVertexComponentType) (int) _type;

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    public enum CullMode : int
    {
        Cull_None = 0,
        Cull_Outside = 1,
        Cull_Inside = 2,
        Cull_All = 3
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TextureSRT
    {
        public const int Size = 0x20;

        public BVec2 TexScale;
        public bfloat TexRotation;
        public BVec2 TexTranslation;

        public static readonly TextureSRT Default = new TextureSRT
        {
            TexScale = new Vector2(1),
            TexRotation = 0,
            TexTranslation = new Vector2(0)
        };

        public static implicit operator TextureFrameState(TextureSRT val)
        {
            return new TextureFrameState(val.TexScale, val.TexRotation, val.TexTranslation, TexMatrixMode.MatrixMaya,
                false);
        }

        public static implicit operator TextureSRT(TextureFrameState val)
        {
            return new TextureSRT {TexScale = val.Scale, TexRotation = val.Rotate, TexTranslation = val.Translate};
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TexMtxEffect
    {
        public sbyte SCNCamera;
        public sbyte SCNLight;
        private byte _mapMode;
        private byte _identity;
        private bMatrix43 _texMtx;

        public MappingMethod MapMode
        {
            get => (MappingMethod) _mapMode;
            set => _mapMode = (byte) value;
        }

        public bool IdentityMatrix
        {
            get => _identity == 0 ? false : true;
            set => _identity = (byte) (value ? 1 : 0);
        }

        public Matrix34 TextureMatrix
        {
            get => _texMtx;
            set
            {
                _texMtx = value;
                IdentityMatrix = value == Matrix34.Identity;
            }
        }

        public static readonly TexMtxEffect Default = new TexMtxEffect
        {
            SCNCamera = -1,
            SCNLight = -1,
            _mapMode = 0, //TexCoord
            _identity = 1,
            _texMtx = Matrix34.Identity
        };
    }


    [Flags]
    public enum TexFlags
    {
        Enabled = 0x1,
        FixedScale = 0x2,
        FixedRot = 0x4,
        FixedTrans = 0x8
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0TexSRTData
    {
        public static readonly MDL0TexSRTData Default = new MDL0TexSRTData
        {
            Tex1Flags = TextureSRT.Default,
            Tex2Flags = TextureSRT.Default,
            Tex3Flags = TextureSRT.Default,
            Tex4Flags = TextureSRT.Default,
            Tex5Flags = TextureSRT.Default,
            Tex6Flags = TextureSRT.Default,
            Tex7Flags = TextureSRT.Default,
            Tex8Flags = TextureSRT.Default,

            Tex1Matrices = TexMtxEffect.Default,
            Tex2Matrices = TexMtxEffect.Default,
            Tex3Matrices = TexMtxEffect.Default,
            Tex4Matrices = TexMtxEffect.Default,
            Tex5Matrices = TexMtxEffect.Default,
            Tex6Matrices = TexMtxEffect.Default,
            Tex7Matrices = TexMtxEffect.Default,
            Tex8Matrices = TexMtxEffect.Default
        };

        public buint _layerFlags;
        public buint _mtxFlags;

        public TextureSRT Tex1Flags;
        public TextureSRT Tex2Flags;
        public TextureSRT Tex3Flags;
        public TextureSRT Tex4Flags;
        public TextureSRT Tex5Flags;
        public TextureSRT Tex6Flags;
        public TextureSRT Tex7Flags;
        public TextureSRT Tex8Flags;

        public TexMtxEffect Tex1Matrices;
        public TexMtxEffect Tex2Matrices;
        public TexMtxEffect Tex3Matrices;
        public TexMtxEffect Tex4Matrices;
        public TexMtxEffect Tex5Matrices;
        public TexMtxEffect Tex6Matrices;
        public TexMtxEffect Tex7Matrices;
        public TexMtxEffect Tex8Matrices;

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

        public TextureSRT GetTexSRT(int Index)
        {
            return *(TextureSRT*) ((byte*) Address + 8 + Index * 20);
        }

        public void SetTexSRT(TextureSRT value, int Index)
        {
            *(TextureSRT*) ((byte*) Address + 8 + Index * 20) = value;
        }

        public TexMtxEffect GetTexMatrices(int Index)
        {
            return *(TexMtxEffect*) ((byte*) Address + 168 + Index * 52);
        }

        public void SetTexMatrices(TexMtxEffect value, int Index)
        {
            *(TexMtxEffect*) ((byte*) Address + 168 + Index * 52) = value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MDL0MaterialLighting
    {
        public buint flags0;
        public RGBAPixel c00;
        public RGBAPixel c01;
        public buint _colorCtrl00;
        public buint _colorCtrl01;

        public buint flags1;
        public RGBAPixel c10;
        public RGBAPixel c11;
        public buint _colorCtrl10;
        public buint _colorCtrl11;

        public LightChannel Channel1
        {
            get => new LightChannel(flags0, c00, c01, _colorCtrl00, _colorCtrl01, null);
            set
            {
                flags0 = (uint) value._flags;
                c00 = value.MaterialColor;
                c01 = value.AmbientColor;
                _colorCtrl00 = value._color._binary._data;
                _colorCtrl01 = value._alpha._binary._data;
            }
        }

        public LightChannel Channel2
        {
            get => new LightChannel(flags1, c10, c11, _colorCtrl10, _colorCtrl11, null);
            set
            {
                flags1 = (uint) value._flags;
                c10 = value.MaterialColor;
                c11 = value.AmbientColor;
                _colorCtrl10 = value._color._binary._data;
                _colorCtrl11 = value._alpha._binary._data;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Material
    {
        public bint _dataLen;
        public bint _mdl0Offset;
        public bint _stringOffset;
        public bint _index;
        public Bin32 _usageFlags;

        //DO_NOT_SEND_XXX is not set as long as it is not set at runtime.
        //Specifying it will forcibly omit sending the display list(s) held by this material (even if this material is animated).
        public enum MatUsageFlags : uint
        {
            DO_NOT_SEND_PIXDL = 0x00000001,            // Does not send the data in ResPixDL
            DO_NOT_SEND_TEVCOLORDL = 0x00000002,       // Does not send the data in ResTevColorDL
            DO_NOT_SEND_TEXCOORDSCALEDL = 0x00000004,  // Does not send the data in ResTexCoordScaleDL
            DO_NOT_SEND_INDMTXANDSCALEDL = 0x00000008, // Does not send the data in ResIndMtxAndScaleDL
            DO_NOT_SEND_CHANDL = 0x00000010,           // Does not send the data in ResChanDL
            DO_NOT_SEND_GENMODE2DL = 0x00000020,       // Does not send the data in ResGenMode2DL
            DO_NOT_SEND_TEXCOORDGENDL = 0x00000040,    // Does not send the data in ResTexCoordGenDL
            DO_NOT_SEND_TEXMTXDL = 0x00000080,         // Does not send the data in ResTexMtxDL
            MASK_DO_NOT_SENDDL = 0x000000ff,           // Mask

            TRANSPARENCY_MODE_XLU =
                0x80000000 // This is set when transparency_mode has been set to xlu in the intermediate file.
        }

        //GenModeData
        public byte _numTexGens;
        public byte _numLightChans;
        public byte _activeTEVStages;
        public byte _numIndTexStages;
        public bint _cull;

        //MiscData
        public byte _depthTestBeforeTexture;
        public sbyte _lightSet;
        public sbyte _fogSet;
        public byte _pad; //Use this as a temporary location to store the model version
        public byte _indirectMethod1;
        public byte _indirectMethod2;
        public byte _indirectMethod3;
        public byte _indirectMethod4;
        public sbyte _normMapRefLight1;
        public sbyte _normMapRefLight2;
        public sbyte _normMapRefLight3;
        public sbyte _normMapRefLight4;

        public bint _shaderOffset;

        public bint _numTextures;
        public bint _matRefOffset;

        //These next three offsets are to the display list, user data and fur data.
        //Which is which depends on the model version. Don't access these directly.
        internal bint _dataOffset1;
        internal bint _dataOffset2;
        internal bint _dataOffset3; //Not here in v9 MDL0 or lower

        public MDL0Header* Parent => (MDL0Header*) (Address + _mdl0Offset);
        public MDL0TextureRef* First => _matRefOffset != 0 ? (MDL0TextureRef*) (Address + _matRefOffset) : null;

        public int DisplayListOffset(int version)
        {
            switch (version)
            {
                case 10:
                case 11:
                    return _dataOffset3;
                default:
                    return _dataOffset2;
            }
        }

        public int UserDataOffset(int version)
        {
            switch (version)
            {
                case 10:
                case 11:
                    return _dataOffset2;
                default:
                    return _dataOffset1;
            }
        }

        public int FurDataOffset(int version)
        {
            switch (version)
            {
                case 10:
                case 11:
                    return _dataOffset1;
                default:
                    return 0;
            }
        }

        public MDL0TexSRTData* TexMatrices(int version)
        {
            return (MDL0TexSRTData*) (Address + 0x1A0 +
                                      (_matRefOffset == 0 ? version < 10 ? 4 : 8 : _matRefOffset & 0xF));
        }

        public MDL0MaterialLighting* Light(int version)
        {
            return (MDL0MaterialLighting*) (Address + 0x3E8 +
                                            (_matRefOffset == 0 ? version < 10 ? 4 : 8 : _matRefOffset & 0xF));
        }

        public UserData* UserData(int version)
        {
            if (UserDataOffset(version) > 0)
            {
                return (UserData*) (Address + UserDataOffset(version));
            }

            return null;
        }

        public MatModeBlock* DisplayLists(int version)
        {
            return (MatModeBlock*) (Address + DisplayListOffset(version));
        }

        public MatTevColorBlock* TevColorBlock(int version)
        {
            return (MatTevColorBlock*) (Address + DisplayListOffset(version) + MatModeBlock.Size);
        }

        public MatTevKonstBlock* TevKonstBlock(int version)
        {
            return (MatTevKonstBlock*) (Address + DisplayListOffset(version) + MatModeBlock.Size +
                                        MatTevColorBlock.Size);
        }

        public MatIndMtxBlock* IndMtxBlock(int version)
        {
            return (MatIndMtxBlock*) (Address + DisplayListOffset(version) + MatModeBlock.Size + MatTevColorBlock.Size +
                                      MatTevKonstBlock.Size);
        }

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
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
    public struct MatDLData
    {
        public static readonly MatDLData Default = new MatDLData
        {
            _mode = MatModeBlock.Default,
            _color = MatTevColorBlock.Default,
            _konst = MatTevKonstBlock.Default,
            _indMtx = MatIndMtxBlock.Default
        };

        public MatModeBlock _mode;
        public MatTevColorBlock _color;
        public MatTevKonstBlock _konst;
        public MatIndMtxBlock _indMtx;
        public byte XFCmds;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MatIndMtxBlock
    {
        public static readonly MatIndMtxBlock Default = new MatIndMtxBlock
        {
            bpReg1 = 0x61,
            RAS1_SS0 = BPMemory.BPMEM_RAS1_SS0,
            bpReg2 = 0x61,
            RAS1_SS1 = BPMemory.BPMEM_RAS1_SS1
        };

        private byte bpReg1;
        public BPMemory RAS1_SS0;
        public RAS1_SS SS0val;
        private byte bpReg2;
        public BPMemory RAS1_SS1;
        public RAS1_SS SS1val;
        public fixed byte Mtx0[15];
        public fixed byte pad1[7];
        public fixed byte Mtx1[15];
        public fixed byte Mtx2[15];
        public fixed byte pad2[2];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TexInit
    {
        public ResTexObjData _texData;
        public ResTlutObjData _pltData;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ResTexObjData
    {
        //0x01: GX_TEXMAP0
        //0x02: GX_TEXMAP1
        //0x04: GX_TEXMAP2
        //0x08: GX_TEXMAP3
        //0x10: GX_TEXMAP4
        //0x20: GX_TEXMAP5
        //0x40: GX_TEXMAP6
        //0x80: GX_TEXMAP7
        //Whether or not to load texture objects is determined by looking at this bitmap.
        //This decides whether or not GXTexObj is enabled.
        //Set during initialization.
        private buint _flagUsedTexMapID;

        //sizeof(GXTexObj) == 32 is assumed
        //32 * 8 = 256
        public fixed byte _texObj[256];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ResTlutObjData
    {
        //0x01: GX_TLUT0
        //0x02: GX_TLUT1
        //0x04: GX_TLUT2
        //0x08: GX_TLUT3
        //0x10: GX_TLUT4
        //0x20: GX_TLUT5
        //0x40: GX_TLUT6
        //0x80: GX_TLUT7
        //Whether or not to load TLUTs is determined by looking at this bitmap.
        //This decides whether or not GXTlutObj is enabled.
        //Set during initialization.
        private buint _flagUsedTlutID;

        //sizeof(GXTlutObj) == 12 is assumed
        //12 * 8 = 96
        public fixed byte _tlutObj[96];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MatModeBlock
    {
        public const int Size = 32;

        public static readonly MatModeBlock Default = new MatModeBlock
        {
            _alphafuncCmd = 0xF361,
            AlphaFunction = GXAlphaFunction.Default,
            _zmodeCmd = 0x4061,
            ZMode = ZMode.Default,
            _maskCmd = 0xFE61,
            _mask1 = 0xFF,
            _mask2 = 0xE3,
            _blendmodeCmd = 0x4161,
            BlendMode = BlendMode.Default,
            _constAlphaCmd = 0x4261,
            ConstantAlpha = ConstantAlpha.Default
        };

        private ushort _alphafuncCmd;
        public GXAlphaFunction AlphaFunction;
        private ushort _zmodeCmd;
        public ZMode ZMode;
        private ushort _maskCmd;
        private readonly byte _mask0;
        private byte _mask1;
        private byte _mask2;
        private ushort _blendmodeCmd;
        public BlendMode BlendMode;
        private ushort _constAlphaCmd;
        public ConstantAlpha ConstantAlpha;
        private fixed byte _pad[7];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MatTevColorBlock
    {
        public const int Size = 64;

        public static readonly MatTevColorBlock Default = new MatTevColorBlock
        {
            _tr1LCmd = 0xE261,
            _tr1HCmd0 = 0xE361,
            _tr1HCmd1 = 0xE361,
            _tr1HCmd2 = 0xE361,
            _tr2LCmd = 0xE461,
            _tr2HCmd0 = 0xE561,
            _tr2HCmd1 = 0xE561,
            _tr2HCmd2 = 0xE561,
            _tr3LCmd = 0xE661,
            _tr3HCmd0 = 0xE761,
            _tr3HCmd1 = 0xE761,
            _tr3HCmd2 = 0xE761
        };

        private ushort _tr1LCmd;
        public ColorReg TevReg1Lo;
        private ushort _tr1HCmd0;
        public ColorReg TevReg1Hi0;
        private ushort _tr1HCmd1;
        public ColorReg TevReg1Hi1;
        private ushort _tr1HCmd2;
        public ColorReg TevReg1Hi2;

        private ushort _tr2LCmd;
        public ColorReg TevReg2Lo;
        private ushort _tr2HCmd0;
        public ColorReg TevReg2Hi0;
        private ushort _tr2HCmd1;
        public ColorReg TevReg2Hi1;
        private ushort _tr2HCmd2;
        public ColorReg TevReg2Hi2;

        private ushort _tr3LCmd;
        public ColorReg TevReg3Lo;
        private ushort _tr3HCmd0;
        public ColorReg TevReg3Hi0;
        private ushort _tr3HCmd1;
        public ColorReg TevReg3Hi1;
        private ushort _tr3HCmd2;
        public ColorReg TevReg3Hi2;

        private fixed byte _pad[4];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MatTevKonstBlock
    {
        public const int Size = 64;

        public static readonly MatTevKonstBlock Default = new MatTevKonstBlock
        {
            _tr0LoCmd = 0xE061,
            TevReg0Lo = ColorReg.Konstant,
            _tr0HiCmd = 0xE161,
            TevReg0Hi = ColorReg.Konstant,
            _tr1LoCmd = 0xE261,
            TevReg1Lo = ColorReg.Konstant,
            _tr1HiCmd = 0xE361,
            TevReg1Hi = ColorReg.Konstant,
            _tr2LoCmd = 0xE461,
            TevReg2Lo = ColorReg.Konstant,
            _tr2HiCmd = 0xE561,
            TevReg2Hi = ColorReg.Konstant,
            _tr3LoCmd = 0xE661,
            TevReg3Lo = ColorReg.Konstant,
            _tr3HiCmd = 0xE761,
            TevReg3Hi = ColorReg.Konstant
        };

        private ushort _tr0LoCmd;
        public ColorReg TevReg0Lo;
        private ushort _tr0HiCmd;
        public ColorReg TevReg0Hi;
        private ushort _tr1LoCmd;
        public ColorReg TevReg1Lo;
        private ushort _tr1HiCmd;
        public ColorReg TevReg1Hi;
        private ushort _tr2LoCmd;
        public ColorReg TevReg2Lo;
        private ushort _tr2HiCmd;
        public ColorReg TevReg2Hi;
        private ushort _tr3LoCmd;
        public ColorReg TevReg3Lo;
        private ushort _tr3HiCmd;
        public ColorReg TevReg3Hi;

        private fixed byte _pad1[24];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct KSelSwapBlock
    {
        public const int Size = 64;

        public static readonly KSelSwapBlock Default = new KSelSwapBlock
        {
            Reg00 = 0x61,
            Reg01 = 0x61,
            Reg02 = 0x61,
            Reg03 = 0x61,
            Reg04 = 0x61,
            Reg05 = 0x61,
            Reg06 = 0x61,
            Reg07 = 0x61,
            Reg08 = 0x61,
            Reg09 = 0x61,
            Reg10 = 0x61,
            Reg11 = 0x61,
            Reg12 = 0x61,
            Reg13 = 0x61,
            Reg14 = 0x61,
            Reg15 = 0x61,
            Reg16 = 0x61,

            Mem00 = (BPMemory) 0xFE,
            _Value00 = new BUInt24(0xF),
            Mem01 = (BPMemory) 0xF6,
            _Value01 = new KSel(0x4),
            Mem02 = (BPMemory) 0xFE,
            _Value02 = new BUInt24(0xF),
            Mem03 = (BPMemory) 0xF7,
            _Value03 = new KSel(0xE),
            Mem04 = (BPMemory) 0xFE,
            _Value04 = new BUInt24(0xF),
            Mem05 = (BPMemory) 0xF8,
            _Value05 = new KSel(0x0),
            Mem06 = (BPMemory) 0xFE,
            _Value06 = new BUInt24(0xF),
            Mem07 = (BPMemory) 0xF9,
            _Value07 = new KSel(0xC),
            Mem08 = (BPMemory) 0xFE,
            _Value08 = new BUInt24(0xF),
            Mem09 = (BPMemory) 0xFA,
            _Value09 = new KSel(0x5),
            Mem10 = (BPMemory) 0xFE,
            _Value10 = new BUInt24(0xF),
            Mem11 = (BPMemory) 0xFB,
            _Value11 = new KSel(0xD),
            Mem12 = (BPMemory) 0xFE,
            _Value12 = new BUInt24(0xF),
            Mem13 = (BPMemory) 0xFC,
            _Value13 = new KSel(0xA),
            Mem14 = (BPMemory) 0xFE,
            _Value14 = new BUInt24(0xF),
            Mem15 = (BPMemory) 0xFD,
            _Value15 = new KSel(0xE),
            Mem16 = (BPMemory) 0x27,
            _Value16 = new RAS1_IRef(0xFFFFFF)
        };

        public byte Reg00; //0x61
        public BPMemory Mem00;
        public BUInt24 _Value00; //KSel Mask - Swap Mode

        public byte Reg01; //0x61
        public BPMemory Mem01;
        public KSel _Value01; //KSel 0 - RG

        public byte Reg02; //0x61
        public BPMemory Mem02;
        public BUInt24 _Value02; //KSel Mask - Swap Mode

        public byte Reg03; //0x61
        public BPMemory Mem03;
        public KSel _Value03; //KSel 1 - BA

        public byte Reg04; //0x61
        public BPMemory Mem04;
        public BUInt24 _Value04; //KSel Mask - Swap Mode

        public byte Reg05; //0x61
        public BPMemory Mem05;
        public KSel _Value05; //KSel 2 - RR

        public byte Reg06; //0x61
        public BPMemory Mem06;
        public BUInt24 _Value06; //KSel Mask - Swap Mode

        public byte Reg07; //0x61
        public BPMemory Mem07;
        public KSel _Value07; //KSel 3 - RA

        public byte Reg08; //0x61
        public BPMemory Mem08;
        public BUInt24 _Value08; //KSel Mask - Swap Mode

        public byte Reg09; //0x61
        public BPMemory Mem09;
        public KSel _Value09; //KSel 4 - GG

        public byte Reg10; //0x61
        public BPMemory Mem10;
        public BUInt24 _Value10; //KSel Mask - Swap Mode

        public byte Reg11; //0x61
        public BPMemory Mem11;
        public KSel _Value11; //KSel 5 - GA

        public byte Reg12; //0x61
        public BPMemory Mem12;
        public BUInt24 _Value12; //KSel Mask - Swap Mode

        public byte Reg13; //0x61
        public BPMemory Mem13;
        public KSel _Value13; //KSel 6 - BB

        public byte Reg14; //0x61
        public BPMemory Mem14;
        public BUInt24 _Value14; //KSel Mask - Swap Mode

        public byte Reg15; //0x61
        public BPMemory Mem15;
        public KSel _Value15; //KSel 7 - BA

        public byte Reg16;     //0x61
        public BPMemory Mem16; //IREF
        public RAS1_IRef _Value16;

        private fixed byte _pad[11];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct StageGroup
    {
        //Carries an even and an odd stage.
        //TRef and KSel modify both stages.
        //The odd stage does not need to be used.

        public const int Size = 0x30;

        public BPCommand _mask;         //KSel Mask - Selection Mode (XRB = 0, XGA = 0)
        public BPCommand _ksel;         //KSel
        public BPCommand _tref;         //TRef
        public BPCommand _evenColorEnv; //Color Env Even
        public BPCommand _oddColorEnv;  //Color Env Odd (Optional)
        public BPCommand _evenAlphaEnv; //Alpha Env Even
        public BPCommand _oddAlphaEnv;  //Alpha Env Odd (Optional)
        public BPCommand _evenCmd;      //CMD (Indirect Texture) Even
        public BPCommand _oddCmd;       //CMD (Indirect Texture) Odd (Optional)

        public static readonly StageGroup Default = new StageGroup
        {
            _mask = new BPCommand(true) {Mem = BPMemory.BPMEM_BP_MASK, Data = new BUInt24(0xFFFFF0)},
            _ksel = new BPCommand(true) {Mem = BPMemory.BPMEM_TEV_KSEL0},
            _tref = new BPCommand(true) {Mem = BPMemory.BPMEM_TREF0},
            _evenColorEnv = new BPCommand(true) {Mem = BPMemory.BPMEM_TEV_COLOR_ENV_0},
            _oddColorEnv = new BPCommand(false) {Mem = BPMemory.BPMEM_GENMODE},
            _evenAlphaEnv = new BPCommand(true) {Mem = BPMemory.BPMEM_TEV_ALPHA_ENV_0},
            _oddAlphaEnv = new BPCommand(false) {Mem = BPMemory.BPMEM_GENMODE},
            _evenCmd = new BPCommand(true) {Mem = BPMemory.BPMEM_IND_CMD0},
            _oddCmd = new BPCommand(false) {Mem = BPMemory.BPMEM_GENMODE}
        };

        public void SetGroup(int index)
        {
            _ksel.Mem = (BPMemory) ((int) BPMemory.BPMEM_TEV_KSEL0 + index);
            _tref.Mem = (BPMemory) ((int) BPMemory.BPMEM_TREF0 + index);
        }

        public void SetStage(int index)
        {
            BPCommand* clrEnv, alphaEnv, cmd;
            if ((index & 1) == 0) //Even
            {
                clrEnv = (BPCommand*) _evenColorEnv.Address;
                alphaEnv = (BPCommand*) _evenAlphaEnv.Address;
                cmd = (BPCommand*) _evenCmd.Address;
            }
            else //Odd
            {
                _oddColorEnv.Reg =
                    _oddAlphaEnv.Reg =
                        _oddCmd.Reg = 0x61;

                clrEnv = (BPCommand*) _oddColorEnv.Address;
                alphaEnv = (BPCommand*) _oddAlphaEnv.Address;
                cmd = (BPCommand*) _oddCmd.Address;
            }

            clrEnv->Mem = (BPMemory) ((int) BPMemory.BPMEM_TEV_COLOR_ENV_0 + (index << 1));
            alphaEnv->Mem = (BPMemory) ((int) BPMemory.BPMEM_TEV_ALPHA_ENV_0 + (index << 1));
            cmd->Mem = (BPMemory) ((int) BPMemory.BPMEM_IND_CMD0 + index);
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

        public StageGroup* Next => (StageGroup*) (Address + 0x30);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0TextureRef
    {
        public const int Size = 0x34;

        public bint _texOffset;
        public bint _pltOffset;
        public bint _texPtr; //Set at runtime
        public bint _pltPtr; //Set at runtime
        public bint _index1;
        public bint _index2;
        public bint _uWrap;
        public bint _vWrap;
        public bint _minFltr;
        public bint _magFltr;
        public bfloat _lodBias;
        public bint _maxAniso;
        public byte _clampBias;
        public byte _texelInterp;
        public bshort _pad;

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

        public string TextureName => _texOffset == 0 ? null : new string((sbyte*) TextureNameAddress);

        public VoidPtr TextureNameAddress
        {
            get => Address + _texOffset;
            set => _texOffset = (int) value - (int) Address;
        }

        public string PaletteName => _pltOffset == 0 ? null : new string((sbyte*) PaletteNameAddress);

        public VoidPtr PaletteNameAddress
        {
            get => Address + _pltOffset;
            set => _pltOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Shader
    {
        public const int Size = 32;

        public bint _dataLength; //Always 512
        public bint _mdl0Offset;
        public bint _index;
        public byte _stages;
        public byte _pad0, _pad1, _pad2;
        public sbyte _ref0, _ref1, _ref2, _ref3, _ref4, _ref5, _ref6, _ref7;
        public int _pad3, _pad4;

        public KSelSwapBlock* SwapBlock => (KSelSwapBlock*) (Address + Size);

        //There are 8 groups max following the display list, each 0x30 in length.
        public StageGroup* First => (StageGroup*) (Address + 0x80);

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
    public unsafe struct PrimDataGroup
    {
        public bint _bufferSize; //The amount of bytes usable
        public bint _size;       //The amount of bytes actually used
        public bint _offset;     //Offset to the data. Relative to this struct

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

        public VoidPtr Data => Address + _offset;
    }

    [Flags]
    public enum ObjFlag
    {
        None = 0,
        ChangeCurrentMatrix = 1, // When rewriting the current matrix (including texture matrix)

        // In other words, when a matrix index is included in the primitive
        Invisible = 2 // When this is turned ON, shape is not sent (this is always OFF at time of conversion)
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Object
    {
        public const uint Size = 0x64;

        public bint _totalLength;
        public bint _mdl0Offset;
        public bint _nodeId; //Single-bind node

        public CPVertexFormat _vertexFormat;
        public XFVertexSpecs _vertexSpecs;

        public PrimDataGroup _defintions; //Pre-Prim data
        public PrimDataGroup _primitives; //Prim data

        public XFArrayFlags _arrayFlags; //Used to enable element arrays

        public bint _flag;
        public bint _stringOffset;
        public bint _index;

        public bint _numVertices;
        public bint _numFaces;

        public bshort _vertexId;
        public bshort _normalId;
        public fixed short _colorIds[2];
        public fixed short _uids[8];

        //ids used in v10+ only
        public short _furVectorId
        {
            get => *(bshort*) (Address + 0x60);
            set => *(bshort*) (Address + 0x60) = value;
        }

        public short _furLayerCoordId
        {
            get => *(bshort*) (Address + 0x62);
            set => *(bshort*) (Address + 0x62) = value;
        }

        public bint _nodeTableOffset;

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

        public MDL0Header* Parent => (MDL0Header*) (Address + _mdl0Offset);
        public bshort* ColorIds => (bshort*) (Address + 0x4C);
        public bshort* UVIds => (bshort*) (Address + 0x50);

        public MDL0PolygonDefs* DefList => (MDL0PolygonDefs*) _defintions.Data;

        public bushort* WeightIndices(int version)
        {
            if (version <= 9)
            {
                return (bushort*) (Address + 0x64);
            }

            return (bushort*) (Address + 0x68);
        }

        public VoidPtr PrimitiveData => _primitives.Data;

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0PolygonDefs
    {
        private fixed byte pad[10];

        //CP Vertex Format
        private bushort CPSetFmtLo; //0x0850
        public buint VtxFmtLo;
        private bushort CPSetFmtHi; //0x0860
        public buint VtxFmtHi;

        //XF Vertex Specs
        public byte XFCmd;            //0x10
        public bushort Length;        //0x0000
        public bushort XFSetVtxSpecs; //0x1008
        public XFVertexSpecs VtxSpecs;
        public byte pad0;

        //CP Vertex Format Flags. Used for direct primitives only?
        public bushort CPSetUVATA; //0x0870
        public buint UVATA;
        public bushort CPSetUVATB; //0x0880
        public buint UVATB;
        public bushort CPSetUVATC; //0x0890
        public buint UVATC;

        public static readonly MDL0PolygonDefs Default = new MDL0PolygonDefs
        {
            CPSetFmtLo = 0x0850,
            CPSetFmtHi = 0x0860,
            XFCmd = 0x10,
            XFSetVtxSpecs = 0x1008,
            CPSetUVATA = 0x0870,
            CPSetUVATB = 0x0880,
            CPSetUVATC = 0x0890
        };
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Texture
    {
        public bint _numEntries;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public MDL0TextureEntry* Entries => (MDL0TextureEntry*) (Address + 4);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MDL0TextureEntry
    {
        public bint _mat; //Material offset
        public bint _ref; //Reference offset

        public override string ToString()
        {
            return $"(Material: 0x{(int) _mat:X}, MatRef: 0x{(int) _ref:X})";
        }
    }
}