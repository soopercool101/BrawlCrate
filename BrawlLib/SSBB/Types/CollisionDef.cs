using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CollisionHeader
    {
        public const int Size = 0x28;

        public bshort _numPoints;
        public bshort _numPlanes;
        public bshort _numObjects;
        public bshort _unk1;
        public bint _pointOffset;
        public bint _planeOffset;
        public bint _objectOffset;
        internal fixed int _pad[5];

        public CollisionHeader(int numPoints, int numPlanes, int numObjects, int unk1)
        {
            _numPoints = (short)numPoints;
            _numPlanes = (short)numPlanes;
            _numObjects = (short)numObjects;
            _unk1 = (short)unk1;
            _pointOffset = 0x28;
            _planeOffset = 0x28 + (numPoints * 8);
            _objectOffset = 0x28 + (numPoints * 8) + (numPlanes * ColPlane.Size);

            fixed (int* p = _pad)
                for (int i = 0; i < 5; i++)
                    p[i] = 0;
        }

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

        public BVec2* Points { get { return (BVec2*)(Address + _pointOffset); } }
        public ColPlane* Planes { get { return (ColPlane*)(Address + _planeOffset); } }
        public ColObject* Objects { get { return (ColObject*)(Address + _objectOffset); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ColPlane
    {
        public const int Size = 0x10;

        public bshort _point1;
        public bshort _point2;
        public bshort _link1;
        public bshort _link2;
        public bint _magic; //-1
        public bushort _type;
        public CollisionPlaneFlags _flags;
        public CollisionPlaneMaterial _material;

        public ColPlane(int pInd1, int pInd2, int pLink1, int pLink2, CollisionPlaneType type, CollisionPlaneFlags2 flags2, CollisionPlaneFlags flags, CollisionPlaneMaterial material)
        {
            _point1 = (short)pInd1;
            _point2 = (short)pInd2;
            _link1 = (short)pLink1;
            _link2 = (short)pLink2;
            _magic = -1;
            _type = (ushort)((int)flags2 | (int)type);
            _flags = flags;
            _material = material;
        }

        public CollisionPlaneType Type { get { return (CollisionPlaneType)(_type & 0xF); } set { _type = (ushort)(_type & 0xFFF0 | (int)value); } }
        public CollisionPlaneFlags2 Flags2 { get { return (CollisionPlaneFlags2)(_type & 0xFFF0); } set { _type = (ushort)(_type & 0x000F | (int)value); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ColObject
    {
        public const int Size = 0x6C;

        public bshort _planeIndex;
        public bshort _planeCount;
        public bint _unk1; //0
        public bint _unk2; //0
        public bint _unk3; //0
        public bushort _flags; //2
        public bshort _unk5; //0
        public BVec2 _boxMin;
        public BVec2 _boxMax;
        public bshort _pointOffset;
        public bshort _pointCount;
        public bshort _unk6; //0
        public bshort _boneIndex;
        public fixed byte _modelName[32];
        public fixed byte _boneName[32];

        public ColObject(int planeIndex, int planeCount, int pointOffset, int pointCount, Vector2 boxMin, Vector2 boxMax, string modelName, string boneName,
            int unk1, int unk2, int unk3, int flags, int unk5, int unk6, int boneIndex)
        {
            _planeIndex = (short)planeIndex;
            _planeCount = (short)planeCount;
            _unk1 = unk1;
            _unk2 = unk2;
            _unk3 = unk3;
            _flags = (ushort)flags;
            _unk5 = (short)unk5;
            _boxMin = boxMin;
            _boxMax = boxMax;
            _pointOffset = (short)pointOffset;
            _pointCount = (short)pointCount;
            _unk6 = (short)unk6;
            _boneIndex = (short)boneIndex;

            fixed (byte* p = _modelName)
                SetStr(p, modelName);

            fixed (byte* p = _boneName)
                SetStr(p, boneName);
        }

        public void Set(int planeIndex, int planeCount, Vector2 boxMin, Vector2 boxMax, string modelName, string boneName)
        {
            _planeIndex = (short)planeIndex;
            _planeCount = (short)planeCount;
            _unk1 = 0;
            _unk2 = 0;
            _unk3 = 0;
            _flags = 0;
            _boxMin = boxMin;
            _boxMax = boxMax;
            _unk5 = 0;
            _unk6 = 0;

            ModelName = modelName;
            BoneName = boneName;
        }

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

        public string ModelName
        {
            get { return new String((sbyte*)Address + 0x2C); }
            set { SetStr((byte*)Address + 0x2C, value); }
        }
        public string BoneName
        {
            get { return new String((sbyte*)Address + 0x4C); }
            set { SetStr((byte*)Address + 0x4C, value); }
        }

        private static void SetStr(byte* dPtr, string str)
        {
            int index = 0;
            if (str != null)
            {
                //Fill string
                int len = Math.Min(str.Length, 31);
                fixed (char* p = str)
                    while (index < len)
                        *dPtr++ = (byte)p[index++];
            }

            //Fill remaining
            while (index++ < 32)
                *dPtr++ = 0;
        }
    }

    public enum CollisionPlaneMaterial : byte
    {
        Basic = 0,                      // Used for many different objects
        Rock = 1,                       // Used for Spear Pillar lower floor, PS1 Mountain
        Grass = 2,                      // Used for grass or leaves
        Soil = 3,                       // Used for PS2 mountain
        Wood = 4,                       // Used for trees (PS1 Fire) and logs/planks (Jungle Japes)
        LightMetal = 5,                 // Used for thin metal platforms
        HeavyMetal = 6,                 // Used for thick metal platforms
        Carpet = 7,                     // Used by Rainbow Cruise
        Alien = 8,                      // Only used for Brinstar side platforms
        Bulborb = 9,                    // Used for Bulborb collision in Distant Planet
        Water = 0x0A,                   // Used for splash effects (Summit when sunk)
        Rubber = 0x0B,                  // Used for the Trowlon subspace enemy
        Slippery = 0x0C,                // Unknown where this is used, but has ice traction
        Snow = 0x0D,                    // Used for snowy surfaces that aren't slippery (SSE)
        SnowIce = 0x0E,                 // Used for Summit and PS2 Ice Transformation
        GameWatch = 0x0F,               // Used for all Flat Zone platforms
        SubspaceIce = 0x10,             // Used some places in Subspace (Purple floor where the door to Tabuu is)
        Checkered = 0x11,               // Used for Green Greens's checkerboard platforms and the present skin of rolling crates
        SpikesTargetTestOnly = 0x12,    // Used for Spike Hazards in Target Test levels and collision hazard #1 for SSE stages. Crashes or has no effect on stages not using a target test module
        Hazard2SSEOnly = 0x13,          // Used for hitboxes on certain SSE levels (180002). Crashes or has no effect on versus stages.
        Hazard3SSEOnly = 0x14,          // Used for hitboxes on certain SSE levels. Crashes or has no effect on versus stages.
        Electroplankton = 0x15,         // Used for Hanenbow leaves
        Cloud = 0x16,                   // Used for clouds on Summit and Skyworld
        Subspace = 0x17,                // Used for Subspace levels, Tabuu's Residence
        Stone = 0x18,                   // Used for Spear Pillar upper level
        UnknownDustless = 0x19,         // Unknown, doesn't generate dust clouds when landing
        MarioBros = 0x1A,               // Used for Mario Bros.
        Grate = 0x1B,                   // Used for Delfino Plaza's main platform
        Sand = 0x1C,                    // Used for sand (Unknown where used)
        Homerun = 0x1D,                 // Used for Home Run Contest, makes Olimar only spawn Purple Pikmin
        WaterNoSplash = 0x1E,           // Used for Distant Planet slope during rain
        Unknown0x1F = 0x1F,             // 
    }

    public enum CollisionPlaneType
    {
        None = 0x0000,          // 0000
        Floor = 0x0001,         // 0001
        Ceiling = 0x0002,       // 0010
        RightWall = 0x0004,     // 0100
        LeftWall = 0x0008       // 1000
    }

    [Flags]
    public enum CollisionPlaneFlags2
    {
        None = 0x0000,
        Characters = 0x0010,        // Characters (Also allows Items and PT to interact)
        Items = 0x0020,             // Items
        PokemonTrainer = 0x0040,    // Pokemon Trainer
        UnknownStageBox = 0x0080    // Unknown, used in the SSE
    }

    [Flags]
    public enum CollisionPlaneFlags : byte
    {
        None = 0x00,
        DropThrough = 0x01,         // Can fall through a floor by pressing down
        Unknown1 = 0x02,            // 
        Rotating = 0x04,            // Automatically changes between floor/wall/ceiling based on angle
        Unknown3 = 0x08,            // 
        Unknown4 = 0x10,            //
        LeftLedge = 0x20,           // Can grab ledge from the left
        RightLedge = 0x40,          // Can grab ledge from the right
        NoWalljump = 0x80           // Cannot walljump off when set
    }
}
