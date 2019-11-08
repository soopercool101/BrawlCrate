using BrawlLib.Internal;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct HKXHeader
    {
        public const uint Tag1 = 0x57E0E057;
        public const uint Tag2 = 0x10C0C010;

        public uint _tag1;    //0x57E0E057
        public uint _tag2;    //0x10C0C010
        public bint _userTag; //0
        public bint _classVersion;

        //4, 0, 1, 1 for the Wii
        public byte _bytesInPointer;
        public byte _littleEndian;
        public byte _emptyBaseClassOptimization;
        public byte _reusePaddingOptimization;

        public bint _sectionCount; //3

        //Index of the offset section for class data
        public bint _dataSectionIndex;  //1
        public bint _dataSectionOffset; //0

        //Index of the offset section for class names
        public bint _classNameSectionIndex; //0

        //Relative to class names data section start
        //This is the offset of the root class name
        public buint _rootClassNameOffset;

        public fixed byte _contentsVersion[0x10];

        public bint _flags;
        public bint _pad;

        //Three sections of offsets:
        //__classnames__
        //__data__
        //__types__

        public PhysicsOffsetSection* OffsetSections => (PhysicsOffsetSection*) (Address + 0x40);

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

        public string Name => new string((sbyte*) Address + 0x28);

        public VoidPtr ClassNamesData => Address + *(buint*) (Address + 0x54);
        public VoidPtr DataData => Address + *(buint*) (Address + 0x84);
        public VoidPtr TypesData => Address + *(buint*) (Address + 0xB4);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PhysicsOffsetSection
    {
        public fixed byte _name[19];

        public byte _pad;        //0xFF
        public bint _dataOffset; //Main header is the base

        //Offsets to indices struct. _dataOffset is the base for everything.
        public bint _localPatchesOffset; //This is technically the length of the data itself
        public bint _globalPatchesOffset;
        public bint _classNamePatchesOffset;
        public bint _exportsOffset;
        public bint _importsOffset;
        public bint _totalLength; //This is the length of data + patches

        public int DataLength => _localPatchesOffset;
        public int LocalPatchesLength => _globalPatchesOffset - _localPatchesOffset;
        public int GlobalPatchesLength => _classNamePatchesOffset - _globalPatchesOffset;
        public int ClassNamePatchesLength => _exportsOffset - _classNamePatchesOffset;
        public int ExportsLength => _importsOffset - _exportsOffset;
        public int ImportsLength => _totalLength - _importsOffset;

        public string Name => new string((sbyte*) Address);

        public VoidPtr Address
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
    public unsafe struct PhysicsClassName
    {
        public buint _signature; //How to calculate?
        public byte _nine;       //9

        public string Name => new string((sbyte*) Address + 5);

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

        //Align string table to 0x10 with 0xFF
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct LocalPatch
    {
        public bint _pointerOffset; //Offset to a uint pointer
        public bint _dataOffset;    //Offset to the pointer's data

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
    public unsafe struct GlobalPatch
    {
        public bint _pointerOffset;
        public bint _sectionIndex;
        public bint _dataOffset;

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
    public unsafe struct ClassNamePatch
    {
        public bint _dataOffset; //Data Offset
        public bint _unknown;    //0

        //Relative to start of class names data
        //points directly to string
        public bint _classNameOffset;

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
    public unsafe struct hkVariant
    {
        public bint _objectPtr; //void*
        public bint _classPtr;  //hkClass*

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
    public unsafe struct hkClass
    {
        public bint _namePtr;   //char*
        public bint _parentPtr; //hkClass*
        public bint _size;
        public bint _interfaceCount;
        public bint _enumPtr; //hkClassEnum*
        public bint _enumCount;
        public bint _propertyPtr; //hkClassMember*
        public bint _propertyCount;
        public bint _defaultsPtr; //void*
        public bint _attribPtr;   //hkCustomAttributes*
        public bint _flags;       //0 = none, 1 = not serializable
        public bint _version;

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
    public unsafe struct hkCustomAttributes
    {
        public bint _attribPtr; //HavokAttribute*
        public bint _count;

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
    public unsafe struct HavokAttribute
    {
        public bint _namePtr; //char*
        public hkVariant _value;

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
    public unsafe struct HavokQSTransform
    {
        //Translation and scale are stored as a vec4, but only 3 components are used

        public BVec3 _translate;
        public bint _pad1;
        public BVec4 _quaternion;
        public BVec3 _scale;
        public bint _pad2;

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
    public unsafe struct hkArray
    {
        public bint _dataPtr; //void*
        public bint _count;
        public Bin32 _capacityAndFlags;

        public int Capacity => (int) _capacityAndFlags[0, 30];
        public FlagsEnum Flags => (FlagsEnum) _capacityAndFlags[30, 2];

        [Flags]
        public enum FlagsEnum
        {
            DONT_DEALLOCATE_FLAG = 2, // Indicates that the storage is not the array's to delete

            LOCKED_FLAG =
                1, // Indicates that the array will never have its destructor called (read in from packfile for instance)
            None = 0
        }

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
    public unsafe struct HomogeneousArray
    {
        public bint _classID;
        public bint _dataPtr; //void*
        public bint _count;

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
    public unsafe struct hkClassEnum
    {
        public bint _namePtr;    //char*
        public bint _entriesPtr; //HavokClassEnumEntry*
        public bint _enumCount;

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
    public unsafe struct HavokClassEnumEntry
    {
        public bint _value;   //The number the string represents
        public bint _namePtr; //char*

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
    public unsafe struct hkClassMember
    {
        public bint _namePtr;     //char*
        public bint _classPtr;    //hkClass*
        public bint _enumPtr;     //hkClassEnum*
        public byte _type;        //The type of data in the struct itself
        public byte _pointedType; //The type of data being pointed to
        public bshort _arraySize;
        public bushort _flags;
        public bushort _structOffset; //Offset of this member in the class struct

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

        public enum Type
        {
            /// No type
            TYPE_VOID = 0,

            /// hkBool,  boolean type
            TYPE_BOOL,

            /// hkChar, signed char type
            TYPE_CHAR,

            /// hkInt8, 8 bit signed integer type
            TYPE_INT8,

            /// hkUint8, 8 bit unsigned integer type
            TYPE_UINT8,

            /// hkInt16, 16 bit signed integer type
            TYPE_INT16,

            /// hkUint16, 16 bit unsigned integer type
            TYPE_UINT16,

            /// hkInt32, 32 bit signed integer type
            TYPE_INT32,

            /// hkUint32, 32 bit unsigned integer type
            TYPE_UINT32,

            /// hkInt64, 64 bit signed integer type
            TYPE_INT64,

            /// hkUint64, 64 bit unsigned integer type
            TYPE_UINT64,

            /// hkReal, float type
            TYPE_REAL,

            /// hkVector4 type
            TYPE_VECTOR4,

            /// hkQuaternion type
            TYPE_QUATERNION,

            /// hkMatrix3 type
            TYPE_MATRIX3,

            /// hkRotation type
            TYPE_ROTATION,

            /// hkQsTransform type
            TYPE_QSTRANSFORM,

            /// hkMatrix4 type
            TYPE_MATRIX4,

            /// hkTransform type
            TYPE_TRANSFORM,

            /// Serialize as zero - deprecated.
            TYPE_ZERO,

            /// Generic pointer, see member flags for more info
            TYPE_POINTER,

            /// Function pointer
            TYPE_FUNCTIONPOINTER,

            /// hkArray<T>, array of items of type T
            TYPE_ARRAY,

            /// hkInplaceArray<T,N> or hkInplaceArrayAligned16<T,N>, array of N items of type T
            TYPE_INPLACEARRAY,

            /// hkEnum<ENUM,STORAGE> - enumerated values
            TYPE_ENUM,

            /// Object
            TYPE_STRUCT,

            /// Simple array (ptr(typed) and size only)
            TYPE_SIMPLEARRAY,

            /// Simple array of homogenous types, so is a class id followed by a void* ptr and size
            TYPE_HOMOGENEOUSARRAY,

            /// hkVariant (void* and hkClass*) type
            TYPE_VARIANT,

            /// char*, null terminated string
            TYPE_CSTRING,

            /// hkUlong, unsigned long, defined to always be the same size as a pointer
            TYPE_ULONG,

            /// hkFlags<ENUM,STORAGE> - 8,16,32 bits of named values.
            TYPE_FLAGS,
            TYPE_MAX
        }

        [Flags]
        public enum Flags : ushort
        {
            FLAGS_NONE = 0,

            //1, 2, 4?
            DEPRECATED_SIZE_8 = 8,   //Deprecated
            DEPRECATED_SIZE_16 = 16, //Deprecated
            DEPRECATED_SIZE_32 = 32, //Deprecated

            //64?
            //Member has forced 8 byte alignment.
            ALIGN_8 = 128,

            //Member has forced 16 byte alignment.
            ALIGN_16 = 256,

            //The members memory contents is not owned by this object
            NOT_OWNED = 512,

            //This member should be saved when serializing
            SERIALIZE_IGNORED = 1024
        }
    }

    //Below are classes that are also defined by meta data

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct hkShape
    {
        public buint _userData0;
        public buint _userData1;
        public bint _type;

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
    public unsafe struct hkSphereShape
    {
        public hkShape _shapeData;
        public bfloat _radius;

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
    public unsafe struct hkCapsuleShape
    {
        public hkSphereShape _sphereData;
        public BVec4 _point1;
        public BVec4 _point2;

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
    public unsafe struct hkConvexTranslateShape
    {
        public hkSphereShape _shapeData;
        public hkShape _childShape;
        public bint _childShapeSize;
        public BVec4 _translation;

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
    public unsafe struct hkRootLevelContainer
    {
        public bint _namedVariantPtr; //hkNamedVariant*, Align data start to 0x10
        public bint _count;

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
    public unsafe struct hkReferencedObject
    {
        public bushort _size;
        public bshort _referenceCount;

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
    public unsafe struct hkNamedVariant
    {
        public bint _namePtr;      //char*
        public bint _classNamePtr; //char*
        public hkVariant _variantInfo;

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
    public unsafe struct hkxScene
    {
        public bint _modellerPtr;   //char*
        public bint _assetPtr;      //char*
        public bfloat _sceneLength; //Time in seconds
        public bint _rootNodePtr;   //hkxNode*

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
    public unsafe struct hkxNode
    {
        public bint _namePtr; //char*

        //The object at this node, if one. (mesh, skin, light, camera, etc)
        //Check class (_classPtr) to find out if not null.
        public hkVariant _object;

        public bint _keyframesPtr; //bMatrix44*
        public bint _numKeyframes;

        public bint _childrenPtr; //bint*, each points to hkxNode*
        public bint _numChildren;

        public bint _annotationsPtr; //HavokAnnotation*
        public bint _numAnnotations;

        public bint _userPropertiesPtr; //char*
        public byte _selected;          //bool, 0 = false, anything else = true

        public fixed byte _pad[3];

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
    public unsafe struct HavokAnnotation
    {
        public bfloat _time;
        public bint _descriptionPtr; //char*

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
}