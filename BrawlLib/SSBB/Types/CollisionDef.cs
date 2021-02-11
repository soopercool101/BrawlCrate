using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
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
            _numPoints = (short) numPoints;
            _numPlanes = (short) numPlanes;
            _numObjects = (short) numObjects;
            _unk1 = (short) unk1;
            _pointOffset = 0x28;
            _planeOffset = 0x28 + numPoints * 8;
            _objectOffset = 0x28 + numPoints * 8 + numPlanes * ColPlane.Size;

            fixed (int* p = _pad)
            {
                for (int i = 0; i < 5; i++)
                {
                    p[i] = 0;
                }
            }
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

        public BVec2* Points => (BVec2*) (Address + _pointOffset);
        public ColPlane* Planes => (ColPlane*) (Address + _planeOffset);
        public ColObject* Objects => (ColObject*) (Address + _objectOffset);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColPlane
    {
        public const int Size = 0x10;

        public bshort _point1;
        public bshort _point2;
        public bshort _link1;
        public bshort _link2;
        public bint _magic; //-1
        public bushort _type;
        public CollisionPlaneMaterialFlags _materialFlags;
        public byte _material;

        public ColPlane(int pInd1, int pInd2, int pLink1, int pLink2, CollisionPlaneType type,
                        CollisionPlaneTypeFlags typeFlags, CollisionPlaneMaterialFlags materialFlags, byte material)
        {
            _point1 = (short) pInd1;
            _point2 = (short) pInd2;
            _link1 = (short) pLink1;
            _link2 = (short) pLink2;
            _magic = -1;
            _type = (ushort) ((int) typeFlags | (int) type);
            _materialFlags = materialFlags;
            _material = material;
        }

        public CollisionPlaneType Type
        {
            get => (CollisionPlaneType) (_type & 0xF);
            set => _type = (ushort) ((_type & 0xFFF0) | (int) value);
        }

        public CollisionPlaneTypeFlags TypeFlags
        {
            get => (CollisionPlaneTypeFlags) (_type & 0xFFF0);
            set => _type = (ushort) ((_type & 0x000F) | (int) value);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ColObject
    {
        public const int Size = 0x6C;

        public bshort _planeIndex;
        public bshort _planeCount;
        public bint _unk1;     //0
        public bint _unk2;     //0
        public bint _unk3;     //0
        public bushort _flags; //2
        public bshort _unk5;   //0
        public BVec2 _boxMin;
        public BVec2 _boxMax;
        public bshort _pointOffset;
        public bshort _pointCount;
        public bshort _unk6; //0
        public bshort _boneIndex;
        public fixed byte _modelName[32];
        public fixed byte _boneName[32];

        public ColObject(int planeIndex, int planeCount, int pointOffset, int pointCount, Vector2 boxMin,
                         Vector2 boxMax, string modelName, string boneName,
                         int unk1, int unk2, int unk3, int flags, int unk5, int unk6, int boneIndex)
        {
            _planeIndex = (short) planeIndex;
            _planeCount = (short) planeCount;
            _unk1 = unk1;
            _unk2 = unk2;
            _unk3 = unk3;
            _flags = (ushort) flags;
            _unk5 = (short) unk5;
            _boxMin = boxMin;
            _boxMax = boxMax;
            _pointOffset = (short) pointOffset;
            _pointCount = (short) pointCount;
            _unk6 = (short) unk6;
            _boneIndex = (short) boneIndex;

            fixed (byte* p = _modelName)
            {
                SetStr(p, modelName);
            }

            fixed (byte* p = _boneName)
            {
                SetStr(p, boneName);
            }
        }

        public void Set(int planeIndex, int planeCount, Vector2 boxMin, Vector2 boxMax, string modelName,
                        string boneName)
        {
            _planeIndex = (short) planeIndex;
            _planeCount = (short) planeCount;
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

        public string ModelName
        {
            get => new string((sbyte*) Address + 0x2C);
            set => SetStr((byte*) Address + 0x2C, value);
        }

        public string BoneName
        {
            get => new string((sbyte*) Address + 0x4C);
            set => SetStr((byte*) Address + 0x4C, value);
        }

        private static void SetStr(byte* dPtr, string str)
        {
            int index = 0;
            if (str != null)
            {
                //Fill string
                int len = Math.Min(str.Length, 31);
                fixed (char* p = str)
                {
                    while (index < len)
                    {
                        *dPtr++ = (byte) p[index++];
                    }
                }
            }

            //Fill remaining
            while (index++ < 32)
            {
                *dPtr++ = 0;
            }
        }
    }

    public class CollisionTerrain
    {
        public byte ID { get; private set; }

        public string Name { get; private set; }

        public bool Valid { get; private set; }

        // Pikmin pluck variables (as strings for ease of display)
        public string PluckSpeed { get; private set; }
        public string RedPluckPercent { get; private set; }
        public string BluePluckPercent { get; private set; }
        public string YellowPluckPercent { get; private set; }
        public string PurplePluckPercent { get; private set; }
        public string WhitePluckPercent { get; private set; }

        // SCLA Variables. Can be changed.
        public float Traction { get; private set; }
        public uint HitDataSet { get; private set; }

        // SCLA Subentry: WalkRun
        public bool WalkCreatesDust { get; private set; }
        public byte WalkUnknown2 { get; private set; }
        public ushort WalkUnknown3 { get; private set; }
        public uint WalkGFXFlag { get; private set; }
        public int WalkSFXFlag { get; private set; }

        // SCLA Subentry: JumpLand
        public bool JumpCreatesDust { get; private set; }
        public byte JumpUnknown2 { get; private set; }
        public ushort JumpUnknown3 { get; private set; }
        public uint JumpGFXFlag { get; private set; }
        public int JumpSFXFlag { get; private set; }

        // SCLA Subentry: TumbleLand
        public bool TumbleCreatesDust { get; private set; }
        public byte TumbleUnknown2 { get; private set; }
        public ushort TumbleUnknown3 { get; private set; }
        public uint TumbleGFXFlag { get; private set; }
        public int TumbleSFXFlag { get; private set; }

        public CollisionTerrain(byte identification, string n)
        {
            ID = identification;
            Name = n;
            PluckSpeed = RedPluckPercent =
                BluePluckPercent = YellowPluckPercent = WhitePluckPercent = PurplePluckPercent = "?";
            Valid = true; // Assume valid unless otherwise defined
        }

        public CollisionTerrain(byte identification, string n, SCLANode s)
        {
            ID = identification;
            Name = n;
            LoadSCLA(s);
        }

        public CollisionTerrain(byte identification, string n, string pl, double r, double y, double b, double p,
                                double w)
        {
            ID = identification;
            Name = n;
            Valid = true; // Assume valid unless otherwise defined
            PluckSpeed = pl;
            RedPluckPercent = r.ToString();
            BluePluckPercent = b.ToString();
            YellowPluckPercent = y.ToString();
            PurplePluckPercent = p.ToString();
            WhitePluckPercent = w.ToString();
        }

        public CollisionTerrain(byte identification, string n, string pl, double r, double y, double b, double p,
                                double w, SCLANode s)
        {
            ID = identification;
            Name = n;
            PluckSpeed = pl;
            RedPluckPercent = r.ToString();
            BluePluckPercent = b.ToString();
            YellowPluckPercent = y.ToString();
            PurplePluckPercent = p.ToString();
            WhitePluckPercent = w.ToString();
            LoadSCLA(s);
        }

        public void LoadSCLA(SCLANode s)
        {
            SCLAEntryNode e = s.FindSCLAEntry(ID);
            if (e != null)
            {
                Valid = true;
                Traction = e.Traction;
                HitDataSet = e.HitDataSet;
                // WalkRun
                WalkCreatesDust = e.WalkRun.CreatesDust == 1;
                WalkUnknown2 = e.WalkRun.Unknown2;
                WalkUnknown3 = e.WalkRun.Unknown3;
                WalkGFXFlag = e.WalkRun.GFXFlag;
                WalkSFXFlag = e.WalkRun.SFXFlag;
                // JumpLand
                JumpCreatesDust = e.JumpLand.CreatesDust == 1;
                JumpUnknown2 = e.JumpLand.Unknown2;
                JumpUnknown3 = e.JumpLand.Unknown3;
                JumpGFXFlag = e.JumpLand.GFXFlag;
                JumpSFXFlag = e.JumpLand.SFXFlag;
                // TumbleLand
                TumbleCreatesDust = e.TumbleLand.CreatesDust == 1;
                TumbleUnknown2 = e.TumbleLand.Unknown2;
                TumbleUnknown3 = e.TumbleLand.Unknown3;
                TumbleGFXFlag = e.TumbleLand.GFXFlag;
                TumbleSFXFlag = e.TumbleLand.SFXFlag;
            }
            else
            {
                Valid = false;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public static readonly CollisionTerrain[] Terrains = new CollisionTerrain[]
        {
            //                   ID    Display Name                                                 Pluck Speed     R       Y       B       P       W
            new CollisionTerrain(0x00, Properties.Resources.Collision0x00, "100%", 1, 1, 1, 0.4, 0.5),
            new CollisionTerrain(0x01, Properties.Resources.Collision0x01, "40%", 1, 1, 1, 0.4, 0.5),
            new CollisionTerrain(0x02, Properties.Resources.Collision0x02, "100%", 1, 1, 1, 0.4, 0.5),
            new CollisionTerrain(0x03, Properties.Resources.Collision0x03, "75%", 1, 1, 1, 0.4, 0.5),
            new CollisionTerrain(0x04, Properties.Resources.Collision0x04, "100%", 1, 1, 1, 0.4, 0.5),
            new CollisionTerrain(0x05, Properties.Resources.Collision0x05, "80%", 1, 2, 1, 0.8, 0.5),
            new CollisionTerrain(0x06, Properties.Resources.Collision0x06, "80%", 1, 2, 1, 0.8, 0.5),
            new CollisionTerrain(0x07, Properties.Resources.Collision0x07, "120%", 2, 1, 1, 0.4, 0.5),
            new CollisionTerrain(0x08, Properties.Resources.Collision0x08, "80%", 1, 1, 2.5, 0.4, 0.5),
            new CollisionTerrain(0x09, Properties.Resources.Collision0x09, "100%", 1, 0.5, 1, 0.2, 1),
            new CollisionTerrain(0x0A, Properties.Resources.Collision0x0A, "100%", 0, 1, 4, 0.4, 0.5),
            new CollisionTerrain(0x0B, Properties.Resources.Collision0x0B, "100%", 1, 1, 1, 0.2, 0.5),
            new CollisionTerrain(0x0C, Properties.Resources.Collision0x0C, "100%", 1, 1, 1, 0.4, 0.5),
            new CollisionTerrain(0x0D, Properties.Resources.Collision0x0D, "100%", 0.5, 0.5, 1, 0.2, 1),
            new CollisionTerrain(0x0E, Properties.Resources.Collision0x0E, "75%", 0.5, 0.5, 1, 0.2, 0.5),
            new CollisionTerrain(0x0F, Properties.Resources.Collision0x0F, "100%", 0.5, 0.5, 0.5, 1, 0.5),
            new CollisionTerrain(0x10, Properties.Resources.Collision0x10, "80%", 0, 0, 0, 1, 0.5),
            new CollisionTerrain(0x11, Properties.Resources.Collision0x11, "100%", 1, 1, 1, 0.4, 0.5),
            new CollisionTerrain(0x12, Properties.Resources.Collision0x12, "70%", 1, 1, 1, 0.4, 0.5),
            new CollisionTerrain(0x13, Properties.Resources.Collision0x13, "70%", 1, 1, 1, 0.4, 0.5),
            new CollisionTerrain(0x14, Properties.Resources.Collision0x14, "70%", 1, 1, 1, 0.4, 0.5),
            new CollisionTerrain(0x15, Properties.Resources.Collision0x15, "100%", 1, 1, 1, 0.1, 0.1),
            new CollisionTerrain(0x16, Properties.Resources.Collision0x16, "100%", 1, 1, 2, 0.4, 1),
            new CollisionTerrain(0x17, Properties.Resources.Collision0x17, "100%", 0.3, 0.3, 0.3, 1, 1),
            new CollisionTerrain(0x18, Properties.Resources.Collision0x18, "100%", 1, 1, 0.5, 0.6, 0.5),
            new CollisionTerrain(0x19, Properties.Resources.Collision0x19, "100%", 1, 1, 1, 0.4, 0.5),
            new CollisionTerrain(0x1A, Properties.Resources.Collision0x1A, "100%", 0.6, 0.6, 1, 0.3, 0.4),
            new CollisionTerrain(0x1B, Properties.Resources.Collision0x1B, "100%", 1, 1, 1, 1, 0.5),
            new CollisionTerrain(0x1C, Properties.Resources.Collision0x1C, "100%", 1.5, 1.5, 0.5, 0.5, 0.2),
            new CollisionTerrain(0x1D, Properties.Resources.Collision0x1D, "100%", 0, 0, 0, 1, 0),
            new CollisionTerrain(0x1E, Properties.Resources.Collision0x1E),
            new CollisionTerrain(0x1F, Properties.Resources.Collision0x1F),
            new CollisionTerrain(0x20, Properties.Resources.CollisionExpansion + " 0x20"),
            new CollisionTerrain(0x21, Properties.Resources.CollisionExpansion + " 0x21"),
            new CollisionTerrain(0x22, Properties.Resources.CollisionExpansion + " 0x22"),
            new CollisionTerrain(0x23, Properties.Resources.CollisionExpansion + " 0x23"),
            new CollisionTerrain(0x24, Properties.Resources.CollisionExpansion + " 0x24"),
            new CollisionTerrain(0x25, Properties.Resources.CollisionExpansion + " 0x25"),
            new CollisionTerrain(0x26, Properties.Resources.CollisionExpansion + " 0x26"),
            new CollisionTerrain(0x27, Properties.Resources.CollisionExpansion + " 0x27"),
            new CollisionTerrain(0x28, Properties.Resources.CollisionExpansion + " 0x28"),
            new CollisionTerrain(0x29, Properties.Resources.CollisionExpansion + " 0x29"),
            new CollisionTerrain(0x2A, Properties.Resources.CollisionExpansion + " 0x2A"),
            new CollisionTerrain(0x2B, Properties.Resources.CollisionExpansion + " 0x2B"),
            new CollisionTerrain(0x2C, Properties.Resources.CollisionExpansion + " 0x2C"),
            new CollisionTerrain(0x2D, Properties.Resources.CollisionExpansion + " 0x2D"),
            new CollisionTerrain(0x2E, Properties.Resources.CollisionExpansion + " 0x2E"),
            new CollisionTerrain(0x2F, Properties.Resources.CollisionExpansion + " 0x2F"),
            new CollisionTerrain(0x30, Properties.Resources.CollisionExpansion + " 0x30"),
            new CollisionTerrain(0x31, Properties.Resources.CollisionExpansion + " 0x31"),
            new CollisionTerrain(0x32, Properties.Resources.CollisionExpansion + " 0x32"),
            new CollisionTerrain(0x33, Properties.Resources.CollisionExpansion + " 0x33"),
            new CollisionTerrain(0x34, Properties.Resources.CollisionExpansion + " 0x34"),
            new CollisionTerrain(0x35, Properties.Resources.CollisionExpansion + " 0x35"),
            new CollisionTerrain(0x36, Properties.Resources.CollisionExpansion + " 0x36"),
            new CollisionTerrain(0x37, Properties.Resources.CollisionExpansion + " 0x37"),
            new CollisionTerrain(0x38, Properties.Resources.CollisionExpansion + " 0x38"),
            new CollisionTerrain(0x39, Properties.Resources.CollisionExpansion + " 0x39"),
            new CollisionTerrain(0x3A, Properties.Resources.CollisionExpansion + " 0x3A"),
            new CollisionTerrain(0x3B, Properties.Resources.CollisionExpansion + " 0x3B"),
            new CollisionTerrain(0x3C, Properties.Resources.CollisionExpansion + " 0x3C"),
            new CollisionTerrain(0x3D, Properties.Resources.CollisionExpansion + " 0x3D"),
            new CollisionTerrain(0x3E, Properties.Resources.CollisionExpansion + " 0x3E"),
            new CollisionTerrain(0x3F, Properties.Resources.CollisionExpansion + " 0x3F"),
            new CollisionTerrain(0x40, Properties.Resources.CollisionExpansion + " 0x40"),
            new CollisionTerrain(0x41, Properties.Resources.CollisionExpansion + " 0x41"),
            new CollisionTerrain(0x42, Properties.Resources.CollisionExpansion + " 0x42"),
            new CollisionTerrain(0x43, Properties.Resources.CollisionExpansion + " 0x43"),
            new CollisionTerrain(0x44, Properties.Resources.CollisionExpansion + " 0x44"),
            new CollisionTerrain(0x45, Properties.Resources.CollisionExpansion + " 0x45"),
            new CollisionTerrain(0x46, Properties.Resources.CollisionExpansion + " 0x46"),
            new CollisionTerrain(0x47, Properties.Resources.CollisionExpansion + " 0x47"),
            new CollisionTerrain(0x48, Properties.Resources.CollisionExpansion + " 0x48"),
            new CollisionTerrain(0x49, Properties.Resources.CollisionExpansion + " 0x49"),
            new CollisionTerrain(0x4A, Properties.Resources.CollisionExpansion + " 0x4A"),
            new CollisionTerrain(0x4B, Properties.Resources.CollisionExpansion + " 0x4B"),
            new CollisionTerrain(0x4C, Properties.Resources.CollisionExpansion + " 0x4C"),
            new CollisionTerrain(0x4D, Properties.Resources.CollisionExpansion + " 0x4D"),
            new CollisionTerrain(0x4E, Properties.Resources.CollisionExpansion + " 0x4E"),
            new CollisionTerrain(0x4F, Properties.Resources.CollisionExpansion + " 0x4F"),
            new CollisionTerrain(0x50, Properties.Resources.CollisionExpansion + " 0x50"),
            new CollisionTerrain(0x51, Properties.Resources.CollisionExpansion + " 0x51"),
            new CollisionTerrain(0x52, Properties.Resources.CollisionExpansion + " 0x52"),
            new CollisionTerrain(0x53, Properties.Resources.CollisionExpansion + " 0x53"),
            new CollisionTerrain(0x54, Properties.Resources.CollisionExpansion + " 0x54"),
            new CollisionTerrain(0x55, Properties.Resources.CollisionExpansion + " 0x55"),
            new CollisionTerrain(0x56, Properties.Resources.CollisionExpansion + " 0x56"),
            new CollisionTerrain(0x57, Properties.Resources.CollisionExpansion + " 0x57"),
            new CollisionTerrain(0x58, Properties.Resources.CollisionExpansion + " 0x58"),
            new CollisionTerrain(0x59, Properties.Resources.CollisionExpansion + " 0x59"),
            new CollisionTerrain(0x5A, Properties.Resources.CollisionExpansion + " 0x5A"),
            new CollisionTerrain(0x5B, Properties.Resources.CollisionExpansion + " 0x5B"),
            new CollisionTerrain(0x5C, Properties.Resources.CollisionExpansion + " 0x5C"),
            new CollisionTerrain(0x5D, Properties.Resources.CollisionExpansion + " 0x5D"),
            new CollisionTerrain(0x5E, Properties.Resources.CollisionExpansion + " 0x5E"),
            new CollisionTerrain(0x5F, Properties.Resources.CollisionExpansion + " 0x5F"),
            new CollisionTerrain(0x60, Properties.Resources.CollisionExpansion + " 0x60"),
            new CollisionTerrain(0x61, Properties.Resources.CollisionExpansion + " 0x61"),
            new CollisionTerrain(0x62, Properties.Resources.CollisionExpansion + " 0x62"),
            new CollisionTerrain(0x63, Properties.Resources.CollisionExpansion + " 0x63"),
            new CollisionTerrain(0x64, Properties.Resources.CollisionExpansion + " 0x64"),
            new CollisionTerrain(0x65, Properties.Resources.CollisionExpansion + " 0x65"),
            new CollisionTerrain(0x66, Properties.Resources.CollisionExpansion + " 0x66"),
            new CollisionTerrain(0x67, Properties.Resources.CollisionExpansion + " 0x67"),
            new CollisionTerrain(0x68, Properties.Resources.CollisionExpansion + " 0x68"),
            new CollisionTerrain(0x69, Properties.Resources.CollisionExpansion + " 0x69"),
            new CollisionTerrain(0x6A, Properties.Resources.CollisionExpansion + " 0x6A"),
            new CollisionTerrain(0x6B, Properties.Resources.CollisionExpansion + " 0x6B"),
            new CollisionTerrain(0x6C, Properties.Resources.CollisionExpansion + " 0x6C"),
            new CollisionTerrain(0x6D, Properties.Resources.CollisionExpansion + " 0x6D"),
            new CollisionTerrain(0x6E, Properties.Resources.CollisionExpansion + " 0x6E"),
            new CollisionTerrain(0x6F, Properties.Resources.CollisionExpansion + " 0x6F"),
            new CollisionTerrain(0x70, Properties.Resources.CollisionExpansion + " 0x70"),
            new CollisionTerrain(0x71, Properties.Resources.CollisionExpansion + " 0x71"),
            new CollisionTerrain(0x72, Properties.Resources.CollisionExpansion + " 0x72"),
            new CollisionTerrain(0x73, Properties.Resources.CollisionExpansion + " 0x73"),
            new CollisionTerrain(0x74, Properties.Resources.CollisionExpansion + " 0x74"),
            new CollisionTerrain(0x75, Properties.Resources.CollisionExpansion + " 0x75"),
            new CollisionTerrain(0x76, Properties.Resources.CollisionExpansion + " 0x76"),
            new CollisionTerrain(0x77, Properties.Resources.CollisionExpansion + " 0x77"),
            new CollisionTerrain(0x78, Properties.Resources.CollisionExpansion + " 0x78"),
            new CollisionTerrain(0x79, Properties.Resources.CollisionExpansion + " 0x79"),
            new CollisionTerrain(0x7A, Properties.Resources.CollisionExpansion + " 0x7A"),
            new CollisionTerrain(0x7B, Properties.Resources.CollisionExpansion + " 0x7B"),
            new CollisionTerrain(0x7C, Properties.Resources.CollisionExpansion + " 0x7C"),
            new CollisionTerrain(0x7D, Properties.Resources.CollisionExpansion + " 0x7D"),
            new CollisionTerrain(0x7E, Properties.Resources.CollisionExpansion + " 0x7E"),
            new CollisionTerrain(0x7F, Properties.Resources.CollisionExpansion + " 0x7F"),
            new CollisionTerrain(0x80, Properties.Resources.CollisionExpansion + " 0x80"),
            new CollisionTerrain(0x81, Properties.Resources.CollisionExpansion + " 0x81"),
            new CollisionTerrain(0x82, Properties.Resources.CollisionExpansion + " 0x82"),
            new CollisionTerrain(0x83, Properties.Resources.CollisionExpansion + " 0x83"),
            new CollisionTerrain(0x84, Properties.Resources.CollisionExpansion + " 0x84"),
            new CollisionTerrain(0x85, Properties.Resources.CollisionExpansion + " 0x85"),
            new CollisionTerrain(0x86, Properties.Resources.CollisionExpansion + " 0x86"),
            new CollisionTerrain(0x87, Properties.Resources.CollisionExpansion + " 0x87"),
            new CollisionTerrain(0x88, Properties.Resources.CollisionExpansion + " 0x88"),
            new CollisionTerrain(0x89, Properties.Resources.CollisionExpansion + " 0x89"),
            new CollisionTerrain(0x8A, Properties.Resources.CollisionExpansion + " 0x8A"),
            new CollisionTerrain(0x8B, Properties.Resources.CollisionExpansion + " 0x8B"),
            new CollisionTerrain(0x8C, Properties.Resources.CollisionExpansion + " 0x8C"),
            new CollisionTerrain(0x8D, Properties.Resources.CollisionExpansion + " 0x8D"),
            new CollisionTerrain(0x8E, Properties.Resources.CollisionExpansion + " 0x8E"),
            new CollisionTerrain(0x8F, Properties.Resources.CollisionExpansion + " 0x8F"),
            new CollisionTerrain(0x90, Properties.Resources.CollisionExpansion + " 0x90"),
            new CollisionTerrain(0x91, Properties.Resources.CollisionExpansion + " 0x91"),
            new CollisionTerrain(0x92, Properties.Resources.CollisionExpansion + " 0x92"),
            new CollisionTerrain(0x93, Properties.Resources.CollisionExpansion + " 0x93"),
            new CollisionTerrain(0x94, Properties.Resources.CollisionExpansion + " 0x94"),
            new CollisionTerrain(0x95, Properties.Resources.CollisionExpansion + " 0x95"),
            new CollisionTerrain(0x96, Properties.Resources.CollisionExpansion + " 0x96"),
            new CollisionTerrain(0x97, Properties.Resources.CollisionExpansion + " 0x97"),
            new CollisionTerrain(0x98, Properties.Resources.CollisionExpansion + " 0x98"),
            new CollisionTerrain(0x99, Properties.Resources.CollisionExpansion + " 0x99"),
            new CollisionTerrain(0x9A, Properties.Resources.CollisionExpansion + " 0x9A"),
            new CollisionTerrain(0x9B, Properties.Resources.CollisionExpansion + " 0x9B"),
            new CollisionTerrain(0x9C, Properties.Resources.CollisionExpansion + " 0x9C"),
            new CollisionTerrain(0x9D, Properties.Resources.CollisionExpansion + " 0x9D"),
            new CollisionTerrain(0x9E, Properties.Resources.CollisionExpansion + " 0x9E"),
            new CollisionTerrain(0x9F, Properties.Resources.CollisionExpansion + " 0x9F"),
            new CollisionTerrain(0xA0, Properties.Resources.CollisionExpansion + " 0xA0"),
            new CollisionTerrain(0xA1, Properties.Resources.CollisionExpansion + " 0xA1"),
            new CollisionTerrain(0xA2, Properties.Resources.CollisionExpansion + " 0xA2"),
            new CollisionTerrain(0xA3, Properties.Resources.CollisionExpansion + " 0xA3"),
            new CollisionTerrain(0xA4, Properties.Resources.CollisionExpansion + " 0xA4"),
            new CollisionTerrain(0xA5, Properties.Resources.CollisionExpansion + " 0xA5"),
            new CollisionTerrain(0xA6, Properties.Resources.CollisionExpansion + " 0xA6"),
            new CollisionTerrain(0xA7, Properties.Resources.CollisionExpansion + " 0xA7"),
            new CollisionTerrain(0xA8, Properties.Resources.CollisionExpansion + " 0xA8"),
            new CollisionTerrain(0xA9, Properties.Resources.CollisionExpansion + " 0xA9"),
            new CollisionTerrain(0xAA, Properties.Resources.CollisionExpansion + " 0xAA"),
            new CollisionTerrain(0xAB, Properties.Resources.CollisionExpansion + " 0xAB"),
            new CollisionTerrain(0xAC, Properties.Resources.CollisionExpansion + " 0xAC"),
            new CollisionTerrain(0xAD, Properties.Resources.CollisionExpansion + " 0xAD"),
            new CollisionTerrain(0xAE, Properties.Resources.CollisionExpansion + " 0xAE"),
            new CollisionTerrain(0xAF, Properties.Resources.CollisionExpansion + " 0xAF"),
            new CollisionTerrain(0xB0, Properties.Resources.CollisionExpansion + " 0xB0"),
            new CollisionTerrain(0xB1, Properties.Resources.CollisionExpansion + " 0xB1"),
            new CollisionTerrain(0xB2, Properties.Resources.CollisionExpansion + " 0xB2"),
            new CollisionTerrain(0xB3, Properties.Resources.CollisionExpansion + " 0xB3"),
            new CollisionTerrain(0xB4, Properties.Resources.CollisionExpansion + " 0xB4"),
            new CollisionTerrain(0xB5, Properties.Resources.CollisionExpansion + " 0xB5"),
            new CollisionTerrain(0xB6, Properties.Resources.CollisionExpansion + " 0xB6"),
            new CollisionTerrain(0xB7, Properties.Resources.CollisionExpansion + " 0xB7"),
            new CollisionTerrain(0xB8, Properties.Resources.CollisionExpansion + " 0xB8"),
            new CollisionTerrain(0xB9, Properties.Resources.CollisionExpansion + " 0xB9"),
            new CollisionTerrain(0xBA, Properties.Resources.CollisionExpansion + " 0xBA"),
            new CollisionTerrain(0xBB, Properties.Resources.CollisionExpansion + " 0xBB"),
            new CollisionTerrain(0xBC, Properties.Resources.CollisionExpansion + " 0xBC"),
            new CollisionTerrain(0xBD, Properties.Resources.CollisionExpansion + " 0xBD"),
            new CollisionTerrain(0xBE, Properties.Resources.CollisionExpansion + " 0xBE"),
            new CollisionTerrain(0xBF, Properties.Resources.CollisionExpansion + " 0xBF"),
            new CollisionTerrain(0xC0, Properties.Resources.CollisionExpansion + " 0xC0"),
            new CollisionTerrain(0xC1, Properties.Resources.CollisionExpansion + " 0xC1"),
            new CollisionTerrain(0xC2, Properties.Resources.CollisionExpansion + " 0xC2"),
            new CollisionTerrain(0xC3, Properties.Resources.CollisionExpansion + " 0xC3"),
            new CollisionTerrain(0xC4, Properties.Resources.CollisionExpansion + " 0xC4"),
            new CollisionTerrain(0xC5, Properties.Resources.CollisionExpansion + " 0xC5"),
            new CollisionTerrain(0xC6, Properties.Resources.CollisionExpansion + " 0xC6"),
            new CollisionTerrain(0xC7, Properties.Resources.CollisionExpansion + " 0xC7"),
            new CollisionTerrain(0xC8, Properties.Resources.CollisionExpansion + " 0xC8"),
            new CollisionTerrain(0xC9, Properties.Resources.CollisionExpansion + " 0xC9"),
            new CollisionTerrain(0xCA, Properties.Resources.CollisionExpansion + " 0xCA"),
            new CollisionTerrain(0xCB, Properties.Resources.CollisionExpansion + " 0xCB"),
            new CollisionTerrain(0xCC, Properties.Resources.CollisionExpansion + " 0xCC"),
            new CollisionTerrain(0xCD, Properties.Resources.CollisionExpansion + " 0xCD"),
            new CollisionTerrain(0xCE, Properties.Resources.CollisionExpansion + " 0xCE"),
            new CollisionTerrain(0xCF, Properties.Resources.CollisionExpansion + " 0xCF"),
            new CollisionTerrain(0xD0, Properties.Resources.CollisionExpansion + " 0xD0"),
            new CollisionTerrain(0xD1, Properties.Resources.CollisionExpansion + " 0xD1"),
            new CollisionTerrain(0xD2, Properties.Resources.CollisionExpansion + " 0xD2"),
            new CollisionTerrain(0xD3, Properties.Resources.CollisionExpansion + " 0xD3"),
            new CollisionTerrain(0xD4, Properties.Resources.CollisionExpansion + " 0xD4"),
            new CollisionTerrain(0xD5, Properties.Resources.CollisionExpansion + " 0xD5"),
            new CollisionTerrain(0xD6, Properties.Resources.CollisionExpansion + " 0xD6"),
            new CollisionTerrain(0xD7, Properties.Resources.CollisionExpansion + " 0xD7"),
            new CollisionTerrain(0xD8, Properties.Resources.CollisionExpansion + " 0xD8"),
            new CollisionTerrain(0xD9, Properties.Resources.CollisionExpansion + " 0xD9"),
            new CollisionTerrain(0xDA, Properties.Resources.CollisionExpansion + " 0xDA"),
            new CollisionTerrain(0xDB, Properties.Resources.CollisionExpansion + " 0xDB"),
            new CollisionTerrain(0xDC, Properties.Resources.CollisionExpansion + " 0xDC"),
            new CollisionTerrain(0xDD, Properties.Resources.CollisionExpansion + " 0xDD"),
            new CollisionTerrain(0xDE, Properties.Resources.CollisionExpansion + " 0xDE"),
            new CollisionTerrain(0xDF, Properties.Resources.CollisionExpansion + " 0xDF"),
            new CollisionTerrain(0xE0, Properties.Resources.CollisionExpansion + " 0xE0"),
            new CollisionTerrain(0xE1, Properties.Resources.CollisionExpansion + " 0xE1"),
            new CollisionTerrain(0xE2, Properties.Resources.CollisionExpansion + " 0xE2"),
            new CollisionTerrain(0xE3, Properties.Resources.CollisionExpansion + " 0xE3"),
            new CollisionTerrain(0xE4, Properties.Resources.CollisionExpansion + " 0xE4"),
            new CollisionTerrain(0xE5, Properties.Resources.CollisionExpansion + " 0xE5"),
            new CollisionTerrain(0xE6, Properties.Resources.CollisionExpansion + " 0xE6"),
            new CollisionTerrain(0xE7, Properties.Resources.CollisionExpansion + " 0xE7"),
            new CollisionTerrain(0xE8, Properties.Resources.CollisionExpansion + " 0xE8"),
            new CollisionTerrain(0xE9, Properties.Resources.CollisionExpansion + " 0xE9"),
            new CollisionTerrain(0xEA, Properties.Resources.CollisionExpansion + " 0xEA"),
            new CollisionTerrain(0xEB, Properties.Resources.CollisionExpansion + " 0xEB"),
            new CollisionTerrain(0xEC, Properties.Resources.CollisionExpansion + " 0xEC"),
            new CollisionTerrain(0xED, Properties.Resources.CollisionExpansion + " 0xED"),
            new CollisionTerrain(0xEE, Properties.Resources.CollisionExpansion + " 0xEE"),
            new CollisionTerrain(0xEF, Properties.Resources.CollisionExpansion + " 0xEF"),
            new CollisionTerrain(0xF0, Properties.Resources.CollisionExpansion + " 0xF0"),
            new CollisionTerrain(0xF1, Properties.Resources.CollisionExpansion + " 0xF1"),
            new CollisionTerrain(0xF2, Properties.Resources.CollisionExpansion + " 0xF2"),
            new CollisionTerrain(0xF3, Properties.Resources.CollisionExpansion + " 0xF3"),
            new CollisionTerrain(0xF4, Properties.Resources.CollisionExpansion + " 0xF4"),
            new CollisionTerrain(0xF5, Properties.Resources.CollisionExpansion + " 0xF5"),
            new CollisionTerrain(0xF6, Properties.Resources.CollisionExpansion + " 0xF6"),
            new CollisionTerrain(0xF7, Properties.Resources.CollisionExpansion + " 0xF7"),
            new CollisionTerrain(0xF8, Properties.Resources.CollisionExpansion + " 0xF8"),
            new CollisionTerrain(0xF9, Properties.Resources.CollisionExpansion + " 0xF9"),
            new CollisionTerrain(0xFA, Properties.Resources.CollisionExpansion + " 0xFA"),
            new CollisionTerrain(0xFB, Properties.Resources.CollisionExpansion + " 0xFB"),
            new CollisionTerrain(0xFC, Properties.Resources.CollisionExpansion + " 0xFC"),
            new CollisionTerrain(0xFD, Properties.Resources.CollisionExpansion + " 0xFD"),
            new CollisionTerrain(0xFE, Properties.Resources.CollisionExpansion + " 0xFE"),
            new CollisionTerrain(0xFF, Properties.Resources.CollisionExpansion + " 0xFF")
        };
    }

    public class CollisionPlaneInteractionType
    {
        public int flags { get; private set; }
        public string Name { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        public CollisionPlaneInteractionType(int id, string n)
        {
            Name = n;
            flags = id;
        }

        public static readonly CollisionPlaneInteractionType[] Terrains = new CollisionPlaneInteractionType[]
        {
            //                                ID      Display Name
            new CollisionPlaneInteractionType(0x0000, Properties.Resources.CollisionTypeNone),
            new CollisionPlaneInteractionType(0x0001, Properties.Resources.CollisionTypeFloor),
            new CollisionPlaneInteractionType(0x0002, Properties.Resources.CollisionTypeCeiling),
            new CollisionPlaneInteractionType(0x0004, Properties.Resources.CollisionTypeRightWall),
            new CollisionPlaneInteractionType(0x0008, Properties.Resources.CollisionTypeLeftWall)
        };
    }

    [Flags]
    public enum CollisionPlaneType : byte
    {
        None = 0x0,      // 0000
        Floor = 0x1,     // 0001
        Ceiling = 0x2,   // 0010
        RightWall = 0x4, // 0100
        LeftWall = 0x8   // 1000
    }

    [Flags]
    public enum CollisionPlaneTypeFlags
    {
        None = 0x0000,
        Characters = 0x0010,     // Characters (Also allows Items and PT to interact)
        Items = 0x0020,          // Items
        PokemonTrainer = 0x0040, // Pokemon Trainer
        Bucculus = 0x0080,       // Allows the Bucculus subspace enemy to bury itself in this collision
        Crush = 0x0100,          // Crush collision when used in SSE
        Unknown0x0200 = 0x0200,
        Unknown0x0400 = 0x0400,
        Unknown0x0800 = 0x0800,
        Unknown0x1000 = 0x1000,
        Unknown0x2000 = 0x2000,
        Unknown0x4000 = 0x4000,
        Unknown0x8000 = 0x8000
    }

    [Flags]
    public enum CollisionPlaneMaterialFlags : byte
    {
        None = 0x00,
        DropThrough = 0x01, // Can fall through a floor by pressing down
        Unknown0x02 = 0x02, // 
        Rotating = 0x04,    // Automatically changes between floor/wall/ceiling based on angle
        SuperSoft = 0x08,   // Allows fighters to be knocked through this collision at high %
        Unknown0x10 = 0x10, //
        LeftLedge = 0x20,   // Can grab ledge from the left
        RightLedge = 0x40,  // Can grab ledge from the right
        NoWalljump = 0x80   // Cannot walljump off when set
    }
}