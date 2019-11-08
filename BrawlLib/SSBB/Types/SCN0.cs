using BrawlLib.Imaging;
using BrawlLib.Internal;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0v4
    {
        public const uint Tag = 0x304E4353;
        public const int Size = 0x44;

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _lightSetOffset;
        public bint _ambLightOffset;
        public bint _lightOffset;
        public bint _fogOffset;
        public bint _cameraOffset;
        public bint _stringOffset;
        public bint _origPathOffset;
        public bushort _frameCount;
        public bushort _specLightCount;
        public bint _loop;
        public bshort _lightSetCount;
        public bshort _ambientCount;
        public bshort _lightCount;
        public bshort _fogCount;
        public bshort _cameraCount;
        public bshort _pad;

        public void Set(int groupLen, int lightSetLen, int ambLightLen, int lightLen, int fogLen, int cameraLen)
        {
            _dataOffset = Size;

            _header._tag = Tag;
            _header._version = 4;
            _header._bresOffset = 0;

            _lightSetOffset = _dataOffset + groupLen;
            _ambLightOffset = _lightSetOffset + lightSetLen;
            _lightOffset = _ambLightOffset + ambLightLen;
            _fogOffset = _lightOffset + lightLen;
            _cameraOffset = _fogOffset + fogLen;
            _header._size = _cameraOffset + cameraLen;

            if (lightSetLen == 0)
            {
                _lightSetOffset = 0;
            }

            if (ambLightLen == 0)
            {
                _ambLightOffset = 0;
            }

            if (lightLen == 0)
            {
                _lightOffset = 0;
            }

            if (fogLen == 0)
            {
                _fogOffset = 0;
            }

            if (cameraLen == 0)
            {
                _cameraOffset = 0;
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

        public ResourceGroup* Group => (ResourceGroup*) (Address + _dataOffset);

        public SCN0LightSet* LightSets => (SCN0LightSet*) (Address + _lightSetOffset);
        public SCN0AmbientLight* AmbientLights => (SCN0AmbientLight*) (Address + _ambLightOffset);
        public SCN0Light* Lights => (SCN0Light*) (Address + _lightOffset);
        public SCN0Fog* Fogs => (SCN0Fog*) (Address + _fogOffset);
        public SCN0Camera* Cameras => (SCN0Camera*) (Address + _cameraOffset);

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }

        public string OrigPath => new string((sbyte*) OrigPathAddress);

        public VoidPtr OrigPathAddress
        {
            get => Address + _origPathOffset;
            set => _origPathOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0v5
    {
        public const uint Tag = 0x304E4353;
        public const int Size = 0x48;

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _lightSetOffset;
        public bint _ambLightOffset;
        public bint _lightOffset;
        public bint _fogOffset;
        public bint _cameraOffset;
        public bint _userDataOffset;
        public bint _stringOffset;
        public bint _origPathOffset;
        public bushort _frameCount;
        public bushort _specLightCount;
        public bint _loop;
        public bshort _lightSetCount;
        public bshort _ambientCount;
        public bshort _lightCount;
        public bshort _fogCount;
        public bshort _cameraCount;
        public bshort _pad;

        public void Set(int groupLen, int lightSetLen, int ambLightLen, int lightLen, int fogLen, int cameraLen)
        {
            _dataOffset = Size;

            _header._tag = Tag;
            _header._version = 5;
            _header._bresOffset = 0;

            _lightSetOffset = _dataOffset + groupLen;
            _ambLightOffset = _lightSetOffset + lightSetLen;
            _lightOffset = _ambLightOffset + ambLightLen;
            _fogOffset = _lightOffset + lightLen;
            _cameraOffset = _fogOffset + fogLen;
            _header._size = _cameraOffset + cameraLen;

            if (lightSetLen == 0)
            {
                _lightSetOffset = 0;
            }

            if (ambLightLen == 0)
            {
                _ambLightOffset = 0;
            }

            if (lightLen == 0)
            {
                _lightOffset = 0;
            }

            if (fogLen == 0)
            {
                _fogOffset = 0;
            }

            if (cameraLen == 0)
            {
                _cameraOffset = 0;
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

        public ResourceGroup* Group => (ResourceGroup*) (Address + _dataOffset);

        public VoidPtr UserData
        {
            get => _userDataOffset == 0 ? null : Address + _userDataOffset;
            set => _userDataOffset = (int) value - (int) Address;
        }

        public SCN0LightSet* LightSets => (SCN0LightSet*) (Address + _lightSetOffset);
        public SCN0AmbientLight* AmbientLights => (SCN0AmbientLight*) (Address + _ambLightOffset);
        public SCN0Light* Lights => (SCN0Light*) (Address + _lightOffset);
        public SCN0Fog* Fogs => (SCN0Fog*) (Address + _fogOffset);
        public SCN0Camera* Cameras => (SCN0Camera*) (Address + _cameraOffset);

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }

        public string OrigPath => new string((sbyte*) OrigPathAddress);

        public VoidPtr OrigPathAddress
        {
            get => Address + _origPathOffset;
            set => _origPathOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0CommonHeader
    {
        public const int Size = 0x14;

        public bint _length;
        public bint _scn0Offset;
        public bint _stringOffset;
        public bint _nodeIndex;
        public bint _realIndex;

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

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0LightSet
    {
        public const int Size = 76;

        public SCN0CommonHeader _header;

        public bint _ambNameOffset;
        public bshort _id; //ambient set here as id at runtime
        public byte _numLights;
        public byte _pad;
        public fixed int _entries[8];    //string offsets
        public fixed short _lightIds[8]; //entry ids are set here at runtime

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

        public bint* Offsets
        {
            get
            {
                fixed (void* ptr = _entries)
                {
                    return (bint*) ptr;
                }
            }
        }

        public string AmbientString => new string((sbyte*) AmbientStringAddress);

        public VoidPtr AmbientStringAddress
        {
            get => Address + _ambNameOffset;
            set => _ambNameOffset = (int) value - (int) Address;
        }

        public bint* StringOffsets => (bint*) (Address + 0x1C);
        public bshort* IDs => (bshort*) (Address + 0x3C);
    }

    [Flags]
    public enum SCN0AmbLightFixedFlags
    {
        None = 0,
        FixedLighting = 128
    }

    [Flags]
    public enum SCN0AmbLightFlags
    {
        None = 0,
        ColorEnabled = 1,
        AlphaEnabled = 2
    }

    [Flags]
    public enum SCN0LightsKeyframes
    {
        FixedX = 0,
        FixedY = 1,
        FixedZ = 2
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0AmbientLight
    {
        public const int Size = 28;

        public SCN0CommonHeader _header;

        public byte _fixedFlags;
        public byte _pad1;
        public byte _pad2;
        public byte _flags;

        public RGBAPixel _lighting;

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

    [Flags]
    public enum UsageFlags : ushort
    {
        Enabled = 0x1,
        SpecularEnabled = 0x2, //Use NonSpecLightId, SpecularColor, Brightness
        ColorEnabled = 0x4,
        AlphaEnabled = 0x8
    }

    [Flags]
    public enum LightType : ushort
    {
        //All use pos and color
        Point = 0x0,       //Don't use aim, use dist func
        Directional = 0x1, //Use aim
        Spotlight = 0x2    //Use aim, spot func and dist func
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0Light
    {
        public const int Size = 92;

        public SCN0CommonHeader _header;

        public bint _nonSpecLightId;
        public bint _userDataOffset;

        public bushort _fixedFlags;
        public bushort _usageFlags;

        public bint _visOffset;
        public BVec3 _startPoint;
        public RGBAPixel _lightColor;
        public BVec3 _endPoint;

        //Used for point or spotlight
        public bint _distFunc;
        public bfloat _refDistance;
        public bfloat _refBrightness;

        //Used for spotlight
        public bint _spotFunc;
        public bfloat _cutoff;

        //Used when specular enabled
        public RGBAPixel _specularColor;
        public bfloat _shininess;

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

        public byte* VisBitEntries => (byte*) _visOffset.Address + _visOffset;
        public RGBAPixel* LightColorEntries => (RGBAPixel*) (_lightColor.Address + *(bint*) _lightColor.Address);

        public RGBAPixel* SpecularColorEntries =>
            (RGBAPixel*) (_specularColor.Address + *(bint*) _specularColor.Address);

        public VoidPtr UserData
        {
            get => _userDataOffset == 0 ? null : Address + _userDataOffset;
            set => _userDataOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0Fog
    {
        public const int Size = 40;

        public SCN0CommonHeader _header;

        public byte _flags;
        public BUInt24 _pad;
        public bint _type;

        public bfloat _start;
        public bfloat _end;
        public RGBAPixel _color;

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

        public SCN0KeyframesHeader* StartKeyframes => (SCN0KeyframesHeader*) (_start.Address + *(bint*) _start.Address);
        public SCN0KeyframesHeader* EndKeyframes => (SCN0KeyframesHeader*) (_end.Address + *(bint*) _end.Address);
        public RGBAPixel* ColorEntries => (RGBAPixel*) (_color.Address + *(bint*) _color.Address);
    }

    [Flags]
    public enum SCN0FogFlags
    {
        None = 0,
        FixedStart = 0x20,
        FixedEnd = 0x40,
        FixedColor = 0x80
    }

    [Flags]
    public enum SCN0CameraFlags
    {
        PosXConstant = 0x2,
        PosYConstant = 0x4,
        PosZConstant = 0x8,
        AspectConstant = 0x10,
        NearConstant = 0x20,
        FarConstant = 0x40,
        PerspFovYConstant = 0x80,
        OrthoHeightConstant = 0x100,
        AimXConstant = 0x200,
        AimYConstant = 0x400,
        AimZConstant = 0x800,
        TwistConstant = 0x1000,
        RotXConstant = 0x2000,
        RotYConstant = 0x4000,
        RotZConstant = 0x8000
    }

    [Flags]
    public enum SCN0CameraFlags2
    {
        None = 0,
        CameraTypeMask = 1,
        AlwaysOn = 2
    }

    public enum SCN0CameraType
    {
        Rotate = 0,
        Aim = 1
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0Camera
    {
        public const int Size = 92;

        public SCN0CommonHeader _header;

        public bint _projectionType;
        public bushort _flags1;
        public bushort _flags2;
        public bint _userDataOffset;

        public BVec3 _position;
        public bfloat _aspect;
        public bfloat _nearZ;
        public bfloat _farZ;
        public BVec3 _rotate;
        public BVec3 _aim;
        public bfloat _twist;
        public bfloat _perspFovY;
        public bfloat _orthoHeight;

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

        public VoidPtr UserData
        {
            get => _userDataOffset == 0 ? null : Address + _userDataOffset;
            set => _userDataOffset = (int) value - (int) Address;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0KeyframesHeader
    {
        public const int Size = 4;

        public bushort _numFrames;
        public bushort _unk;

        public SCN0KeyframesHeader(int entries)
        {
            _numFrames = (ushort) entries;
            _unk = 0;
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

        public SCN0KeyframeStruct* Data => (SCN0KeyframeStruct*) (Address + Size);
    }

    public struct SCN0KeyframeStruct
    {
        public const int Size = 12;

        public bfloat _tangent, _index, _value;

        public static implicit operator SCN0Keyframe(SCN0KeyframeStruct v)
        {
            return new SCN0Keyframe(v._tangent, v._index, v._value);
        }

        public static implicit operator SCN0KeyframeStruct(SCN0Keyframe v)
        {
            return new SCN0KeyframeStruct(v._tangent, v._index, v._value);
        }

        public SCN0KeyframeStruct(float tan, float index, float value)
        {
            _index = index;
            _value = value;
            _tangent = tan;
        }

        public float Index
        {
            get => _index;
            set => _index = value;
        }

        public float Value
        {
            get => _value;
            set => _value = value;
        }

        public float Tangent
        {
            get => _tangent;
            set => _tangent = value;
        }

        public override string ToString()
        {
            return $"Tangent={_tangent}, Index={_index}, Value={_value}";
        }
    }

    public class SCN0Keyframe
    {
        public float _tangent, _index, _value;

        public static implicit operator SCN0Keyframe(Vector3 v)
        {
            return new SCN0Keyframe(v._x, v._y, v._z);
        }

        public static implicit operator Vector3(SCN0Keyframe v)
        {
            return new Vector3(v._tangent, v._index, v._value);
        }

        public SCN0Keyframe(float tan, float index, float value)
        {
            _index = index;
            _value = value;
            _tangent = tan;
        }

        public SCN0Keyframe()
        {
        }

        public float Index
        {
            get => _index;
            set => _index = value;
        }

        public float Value
        {
            get => _value;
            set => _value = value;
        }

        public float Tangent
        {
            get => _tangent;
            set => _tangent = value;
        }

        public override string ToString()
        {
            return $"Tangent={_tangent}, Index={_index}, Value={_value}";
        }
    }
}