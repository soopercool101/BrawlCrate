using BrawlLib.Imaging;
using BrawlLib.Internal;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct REFF
    {
        //Header + string is aligned to 4 bytes

        public const uint Tag = 0x46464552;

        public NW4RCommonHeader _header;
        public uint _tag;        //Same as header
        public bint _dataLength; //Size of second REFF block. (file size - 0x18)
        public bint _dataOffset; //Offset from itself. Begins first entry
        public bint _linkPrev;   //0
        public bint _linkNext;   //0
        public bshort _stringLen;
        public bshort _padding; //0

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

        public string IdString
        {
            get => new string((sbyte*) Address + 0x28);
            set
            {
                int len = value.Length + 1;
                _stringLen = (short) len;

                byte* dPtr = (byte*) Address + 0x28;
                fixed (char* sPtr = value)
                {
                    for (int i = 0; i < len; i++)
                    {
                        *dPtr++ = (byte) sPtr[i];
                    }
                }

                //Align to 4 bytes
                while ((len++ & 3) != 0)
                {
                    *dPtr++ = 0;
                }

                //Set data offset
                _dataOffset = 0x10 + len - 1;
            }
        }

        public REFTypeObjectTable* Table => (REFTypeObjectTable*) (Address + 0x18 + _dataOffset);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct REFFDataHeader
    {
        public buint _unknown; //0
        public buint _headerSize;

        public EmitterDesc _descriptor;

        public ParticleParameterHeader* Params => (ParticleParameterHeader*) (Address + _headerSize + 8);

        public bushort* PtclTrackCount => (bushort*) ((VoidPtr) Params + Params->headersize + 4);
        public bushort* PtclInitTrackCount => PtclTrackCount + 1;
        public bushort* EmitTrackCount => (bushort*) ((VoidPtr) PtclTrackCount + 4 + *PtclTrackCount * 8);
        public bushort* EmitInitTrackCount => EmitTrackCount + 1;

        public buint* PtclTrack => (buint*) ((VoidPtr) PtclTrackCount + 4);
        public buint* EmitTrack => (buint*) ((VoidPtr) EmitTrackCount + 4);

        public VoidPtr Animations => (VoidPtr) EmitTrackCount + 4 + *EmitTrackCount * 8;

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
    public unsafe struct EmitterDesc
    {
        [Flags]
        public enum EmitterCommonFlag : uint
        {
            SyncLife = 0x1,
            Invisible = 0x2,
            MaxLife = 0x4,
            InheritPtclScale = 0x20,
            InheritPtclRotate = 0x40,
            InheritChildEScale = 0x80,
            InheritChildERotate = 0x100,
            DisableCalc = 0x200,
            InheritPtclPivot = 0x400,
            InheritChildPivot = 0x800,
            InheritChildPScale = 0x1000,
            InheritChildPRotate = 0x2000,
            RelocateComplete = 0x80000000
        }

        public enum EmitFormType
        {
            Disc = 0,
            Line = 1,
            Cube = 5,
            Cylinder = 7,
            Sphere = 8,
            Point = 9,
            Torus = 10
        }

        public buint _commonFlag; // EmitterCommonFlag
        public buint _emitFlag;   // EmitFormType - value & 0xFF
        public bushort _emitLife;
        public bushort _ptclLife;
        public sbyte _ptclLifeRandom;
        public sbyte _inheritChildPtclTranslate;
        public sbyte _emitEmitIntervalRandom;

        public sbyte _emitEmitRandom;

        //0x10
        public bfloat _emitEmit;
        public bushort _emitEmitStart;
        public bushort _emitEmitPast;
        public bushort _emitEmitInterval;
        public sbyte _inheritPtclTranslate;
        public sbyte _inheritChildEmitTranslate;

        public bfloat _commonParam1;

        //0x20
        public bfloat _commonParam2;
        public bfloat _commonParam3;
        public bfloat _commonParam4;

        public bfloat _commonParam5;

        //0x30
        public bfloat _commonParam6;
        public bushort _emitEmitDiv;
        public sbyte _velInitVelocityRandom;
        public sbyte _velMomentumRandom;
        public bfloat _velPowerRadiationDir;

        public bfloat _velPowerYAxis;

        //0x40
        public bfloat _velPowerRandomDir;
        public bfloat _velPowerNormalDir;
        public bfloat _velDiffusionEmitterNormal;

        public bfloat _velPowerSpecDir;

        //0x50
        public bfloat _velDiffusionSpecDir;

        public BVec3 _velSpecDir;

        //0x60
        public BVec3 _scale;
        public BVec3 _rotate;

        public BVec3 _translate;

        //0x84
        public byte _lodNear;
        public byte _lodFar;
        public byte _lodMinEmit;
        public byte _lodAlpha;

        public buint _randomSeed;

        public fixed byte _userdata[8];

        //0x94
        public EmitterDrawSetting9 _drawSetting;

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
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct EmitterDrawSetting7
    {
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

        public bushort mFlags; // DrawFlag

        public byte mACmpComp0;
        public byte mACmpComp1;
        public byte mACmpOp;

        public byte mNumTevs;   // TEV uses stages 1 through 4
        public byte mFlagClamp; // Obsolete

        public byte mIndirectTargetStage;
        //0x8

        //public byte mTevTexture1;
        //public byte mTevTexture2;
        //public byte mTevTexture3;
        //public byte mTevTexture4;

        public TevStageColor mTevColor1;
        public TevStageColor mTevColor2;
        public TevStageColor mTevColor3;
        public TevStageColor mTevColor4;
        public TevStageColorOp mTevColorOp1;
        public TevStageColorOp mTevColorOp2;
        public TevStageColorOp mTevColorOp3;
        public TevStageColorOp mTevColorOp4;
        public TevStageColor mTevAlpha1;
        public TevStageColor mTevAlpha2;
        public TevStageColor mTevAlpha3;
        public TevStageColor mTevAlpha4;
        public TevStageColorOp mTevAlphaOp1;
        public TevStageColorOp mTevAlphaOp2;
        public TevStageColorOp mTevAlphaOp3;
        public TevStageColorOp mTevAlphaOp4;

        // Constant register selector: GXTevKColorSel
        public byte mTevKColorSel1;
        public byte mTevKColorSel2;
        public byte mTevKColorSel3;
        public byte mTevKColorSel4;

        // Constant register selector: GXTevKAlphaSel
        public byte mTevKAlphaSel1;
        public byte mTevKAlphaSel2;
        public byte mTevKAlphaSel3;
        public byte mTevKAlphaSel4;

        public ReffBlendMode mBlendMode;

        public ColorInput mColorInput;
        //public ColorInput mAlphaInput;

        //Length below is 0x48

        public byte mZCompareFunc; // GXCompare

        public byte mAlphaFlickType; // AlphaFlickType

        public bushort mAlphaFlickCycle;
        public byte mAlphaFlickRandom;
        public byte mAlphaFlickAmplitude;

        public Lighting mLighting;

        public fixed float mIndTexOffsetMtx[6]; //2x3 Matrix
        public sbyte mIndTexScaleExp;

        public sbyte pivotX;
        public sbyte pivotY;
        public byte padding;

        public byte ptcltype; // enum Type

        public byte typeOption; // Expression assistance
        // Billboard:
        //   enum BillboardAssist
        // Linear stripe/smooth stripe:
        //   enum StripeAssist
        // Other:
        //   enum Assist

        public byte typeDir; // Movement direction
        // Other:
        //   enum Ahead
        // Billboard:
        //   enum BillboardAhead

        public byte typeAxis; // enum RotateAxis

        public byte typeOption0; // Various types of parameters corresponding to the particle shapes
        // Directional:
        //   Change vertical (Y) based on speed : 0=off, 1=on
        // Linear stripe/smooth stripe:
        //   Number of vertices in the tube (3+)

        public byte typeOption1; // Various types of parameters corresponding to the particle shapes
        // Directional:
        //   enum Face
        // Smooth stripe
        //   Number of interpolation divisions (1+)

        public byte typeOption2; // Various types of parameters corresponding to the particle shapes

        // Linear stripe/smooth stripe:
        //   enum StripeConnect
        //   | enum StripeInitialPrevAxis
        //   | enum StripeTexmapType
        // Directional:
        //   enum DirectionalPivot
        public byte padding4;
        public bfloat zOffset;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct EmitterDrawSetting9
    {
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

        public bushort mFlags; // DrawFlag

        public byte mACmpComp0;
        public byte mACmpComp1;
        public byte mACmpOp;

        public byte mNumTevs;   // TEV uses stages 1 through 4
        public byte mFlagClamp; // Obsolete

        public byte mIndirectTargetStage;
        //0x8

        public byte mTevTexture1;
        public byte mTevTexture2;
        public byte mTevTexture3;
        public byte mTevTexture4;

        public TevStageColor mTevColor1;
        public TevStageColor mTevColor2;
        public TevStageColor mTevColor3;
        public TevStageColor mTevColor4;
        public TevStageColorOp mTevColorOp1;
        public TevStageColorOp mTevColorOp2;
        public TevStageColorOp mTevColorOp3;
        public TevStageColorOp mTevColorOp4;
        public TevStageColor mTevAlpha1;
        public TevStageColor mTevAlpha2;
        public TevStageColor mTevAlpha3;
        public TevStageColor mTevAlpha4;
        public TevStageColorOp mTevAlphaOp1;
        public TevStageColorOp mTevAlphaOp2;
        public TevStageColorOp mTevAlphaOp3;
        public TevStageColorOp mTevAlphaOp4;

        // Constant register selector: GXTevKColorSel
        public byte mTevKColorSel1;
        public byte mTevKColorSel2;
        public byte mTevKColorSel3;
        public byte mTevKColorSel4;

        // Constant register selector: GXTevKAlphaSel
        public byte mTevKAlphaSel1;
        public byte mTevKAlphaSel2;
        public byte mTevKAlphaSel3;
        public byte mTevKAlphaSel4;

        public ReffBlendMode mBlendMode;

        public ColorInput mColorInput;
        public ColorInput mAlphaInput;

        //Length below is 0x48

        public byte mZCompareFunc; // GXCompare

        public byte mAlphaFlickType; // AlphaFlickType

        public bushort mAlphaFlickCycle;
        public byte mAlphaFlickRandom;
        public byte mAlphaFlickAmplitude;

        public Lighting mLighting;

        public fixed float mIndTexOffsetMtx[6]; //2x3 Matrix
        public sbyte mIndTexScaleExp;

        public sbyte pivotX;
        public sbyte pivotY;
        public byte padding;

        public byte ptcltype; // enum Type

        public byte typeOption; // Expression assistance
        // Billboard:
        //   enum BillboardAssist
        // Linear stripe/smooth stripe:
        //   enum StripeAssist
        // Other:
        //   enum Assist

        public byte typeDir; // Movement direction
        // Other:
        //   enum Ahead
        // Billboard:
        //   enum BillboardAhead

        public byte typeAxis; // enum RotateAxis

        public byte typeOption0; // Various types of parameters corresponding to the particle shapes
        // Directional:
        //   Change vertical (Y) based on speed : 0=off, 1=on
        // Linear stripe/smooth stripe:
        //   Number of vertices in the tube (3+)

        public byte typeOption1; // Various types of parameters corresponding to the particle shapes
        // Directional:
        //   enum Face
        // Smooth stripe
        //   Number of interpolation divisions (1+)

        public byte typeOption2; // Various types of parameters corresponding to the particle shapes

        // Linear stripe/smooth stripe:
        //   enum StripeConnect
        //   | enum StripeInitialPrevAxis
        //   | enum StripeTexmapType
        // Directional:
        //   enum DirectionalPivot
        public byte padding4;
        public bfloat zOffset;
    }

    [Flags]
    public enum DrawFlag : ushort
    {
        ZCompEnable = 0x0001,     // 0x0001
        ZUpdate = 0x0002,         // 0x0002
        ZCompBeforeTex = 0x0004,  // 0x0004
        ClippingDisable = 0x0008, // 0x0008
        UseTex1 = 0x0010,         // 0x0010
        UseTex2 = 0x0020,         // 0x0020
        UseTexInd = 0x0040,       // 0x0040
        ProjTex1 = 0x0080,        // 0x0080
        ProjTex2 = 0x0100,        // 0x0100
        ProjTexInd = 0x0200,      // 0x0200
        Invisible = 0x0400,       // 0x0400 1: Does not render
        DrawOrder = 0x0800,       // 0x0800 0: normal order, 1: reverse order
        FogEnable = 0x1000,       // 0x1000
        XYLinkSize = 0x2000,      // 0x2000
        XYLinkScale = 0x4000      // 0x4000
    }

    [Flags]
    public enum IndirectTargetStage
    {
        None = 0,
        Stage0 = 1,
        Stage1 = 2,
        Stage2 = 4,
        Stage3 = 8
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TevStageColor
    {
        public byte mA; // GXTevColorArg / GXTevAlphaArg
        public byte mB; // GXTevColorArg / GXTevAlphaArg
        public byte mC; // GXTevColorArg / GXTevAlphaArg
        public byte mD; // GXTevColorArg / GXTevAlphaArg

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
    public unsafe struct TevStageColorOp
    {
        public byte mOp;     // GXTevOp
        public byte mBias;   // GXTevBias
        public byte mScale;  // GXTevScale
        public byte mClamp;  // GXBool
        public byte mOutReg; // GXTevRegID

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
    public struct ReffBlendMode
    {
        public byte mType;      // GXBlendMode
        public byte mSrcFactor; // GXBlendFactor
        public byte mDstFactor; // GXBlendFactor
        public byte mOp;        // GXLogicOp
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColorInput
    {
        public enum RasColor
        {
            Null = 0,    // No request
            Lighting = 1 // Color lit by lighting
        }

        public enum TevColor
        {
            Null = 0,            // No request
            Layer1Primary = 1,   // Layer 1 primary color
            Layer1Secondary = 2, // Layer 1 Secondary Color
            Layer2Primary = 3,   // Layer 2 primary color
            Layer2Secondary = 4, // Layer 2 Secondary Color
            Layer1Multi = 5,     // Layer 1 primary color x secondary color
            Layer2Multi = 6      // Layer 2 primary color x secondary color
        }

        public byte mRasColor; //Rasterize color (only channel 0): RasColor

        //TEV register: TevColor
        public byte mTevColor1;
        public byte mTevColor2;
        public byte mTevColor3;

        //Constant register: TevColor
        public byte mTevKColor1;
        public byte mTevKColor2;
        public byte mTevKColor3;
        public byte mTevKColor4;
    }

    // Alpha Swing
    public enum AlphaFlickType : byte
    {
        None = 0,
        Triangle,
        SawTooth1,
        SawTooth2,
        Square,
        Sine
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Lighting
    {
        public enum Mode
        {
            Off = 0,
            Simple,
            Hardware
        }

        public enum Type
        {
            None = 0,
            Ambient,
            Point
        }

        public byte mMode; // Mode
        public byte mType; // Type

        public RGBAPixel mAmbient;
        public RGBAPixel mDiffuse;
        public bfloat mRadius;
        public BVec3 mPosition;
    }

    // Expression
    //
    // Stored in ptcltype member.
    public enum ReffType
    {
        Point = 0,
        Line,
        Free,
        Billboard,
        Directional,
        Stripe,
        SmoothStripe
    }

    // Expression assistance -- everything except billboards
    //
    // Stored in typeOption member.
    public enum Assist
    {
        Normal = 0, // Render single Quad to Face surface
        Cross       // Add Quads so they are orthogonal to Normals.
    }

    // Expression assistance -- billboards
    //
    // Stored in typeOption member.
    public enum BillboardAssist
    {
        Normal = 0,  // Normal
        Y,           // Y-axis billboard
        Directional, // Billboard using the movement direction as its axis
        NormalNoRoll // Normal (no roll)
    }

    // Expression assistance -- stripes
    public enum StripeAssist
    {
        Normal = 0, // Normal.
        Cross,      // Add a surface orthogonal to the Normal.
        Billboard,  // Always faces the screen.
        Tube        // Expression of a tube shape.
    }

    // Movement direction (Y-axis) -- everything except billboard
    //
    // Stored in typeDir member.
    public enum Ahead
    {
        Speed = 0,     // Velocity vector direction
        EmitterCenter, // Relative position from the center of emitter
        EmitterDesign, // Emitter specified direction
        Particle,      // Difference in location from the previous particle
        User,          // User specified (unused)
        NoDesign,      // Unspecified
        ParticleBoth,  // Difference in position with both neighboring particles
        NoDesignYAxis  // Unspecified (initialized as the world Y-axis)
    }

    // Movement direction (Y-axis) -- billboards
    //
    // Stored in typeDir member.
    public enum BillboardAhead
    {
        Speed = 0,     // Velocity vector direction
        EmitterCenter, // Relative position from the center of emitter
        EmitterDesign, // Emitter specified direction
        Particle,      // Difference in location from the previous particle
        ParticleBoth   // Difference in position with both neighboring particles
    }

    // Rotational axis to take into account when rendering
    //
    // Stored in typeAxis member.
    public enum RotateAxis
    {
        OnlyX = 0, // X-axis rotation only
        OnlyY,     // Y-axis rotation only
        OnlyZ,     // Z-axis rotation only
        XYZ        // 3-axis rotation
    }

    // Base surface (polygon render surface)
    //
    // Stored in typeReference.
    public enum Face
    {
        XY = 0,
        XZ
    }

    // Stripe terminal connections
    //
    // Stored in typeOption2. >> 0 & 7
    public enum StripeConnect
    {
        None = 0, // Does not connect
        Ring,     // Both ends connected

        Emitter // Connect between the newest particle and the emitter
        //Mask = 0x07 // StripeConnect mask
    }

    // Initial value of the reference axis for stripes
    //
    // Stored in typeOption2. >> 3 & 7
    public enum StripeInitialPrevAxis
    {
        XAxis = 1, // X-axis of the emitter
        YAxis = 0, // Y-axis of the emitter (assigned to 0 for compatibility)
        ZAxis = 2, // Z-axis of the emitter

        XYZ = 3 // Direction in emitter coordinates (1, 1, 1)
        //STRIPE_INITIAL_PREV_AXIS__MASK = 0x07 << 3          // Bitmask
    }

    // Method of applying texture to stripes
    //
    // Stored in typeOption2. >> 6 & 3
    public enum StripeTexmapType
    {
        Stretch = 0, // Stretch the texture along the stripe's entire length.

        Repeat = 1 // Repeats the texture for each segment.
        //STRIPE_TEXMAP_TYPE__MASK = 0x03 << 6
    }

    // Directional axis processing
    //
    // Stored in typeOption2.
    [Flags]
    public enum DirectionalPivot
    {
        NoProcessing = 0 << 0, // No processing

        Billboard = 1 << 0 // Convert into a billboard, with the movement direction as its axis
        //DIRECTIONAL_PIVOT__MASK = 0x03 << 0
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ParticleParameterHeader
    {
        public buint headersize;
        public ParticleParameterDesc paramDesc;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ParticleParameterDesc
    {
        public RGBAPixel mColor11;
        public RGBAPixel mColor12;
        public RGBAPixel mColor21;
        public RGBAPixel mColor22;

        public BVec2 size;
        public BVec2 scale;
        public BVec3 rotate;

        public BVec2 textureScale1;
        public BVec2 textureScale2;
        public BVec2 textureScale3;

        public BVec3 textureRotate;

        public BVec2 textureTranslate1;
        public BVec2 textureTranslate2;
        public BVec2 textureTranslate3;

        //These three are texture data pointers
        public uint mTexture1; // 0..1: stage0,1, 2: Indirect
        public uint mTexture2;
        public uint mTexture3;

        public bushort textureWrap;
        public byte textureReverse;

        public byte mACmpRef0;
        public byte mACmpRef1;

        public byte rotateOffsetRandomX;
        public byte rotateOffsetRandomY;
        public byte rotateOffsetRandomZ;

        public BVec3 rotateOffset;

        public bushort textureNames; //align to 4 bytes
    }

    //The value is an offset into the particle param desc
    public enum v9AnimCurveTargetByteFloat //curve flag = 0, 3
    {
        //Updates: ParticleParam
        Color0Primary = 0,
        Alpha0Primary = 3,
        Color0Secondary = 4,
        Alpha0Secondary = 7,
        Color1Primary = 8,
        Alpha1Primary = 11,
        Color1Secondary = 12,
        Alpha1Secondary = 15,
        Size = 16,
        Scale = 24,
        Tex1Scale = 44,
        Tex1Rot = 68,
        Tex1Trans = 80,
        Tex2Scale = 52,
        Tex2Rot = 72,
        Tex2Trans = 88,
        TexIndScale = 60,
        TexIndRot = 76,
        TexIndTrans = 96,
        AlphaCompareRef0 = 119,
        AlphaCompareRef1 = 120
    }

    public enum v7AnimCurveTargetByteFloat //curve flag = 0, 3
    {
        //Updates: ParticleParam
        Color0Primary = 0,
        Unknown1 = 1,
        Unknown2 = 2,
        Alpha0Primary = 3,
        Color0Secondary = 4,
        Unknown5 = 5,
        Unknown6 = 6,
        Alpha0Secondary = 7,
        Color1Primary = 8,
        Unknown9 = 9,
        Unknown10 = 10,
        Alpha1Primary = 11,
        Color1Secondary = 12,
        Unknown13 = 13,
        Unknown14 = 14,
        Alpha1Secondary = 15,
        Size = 16,
        Unknown17 = 17,
        Unknown18 = 18,
        Unknown19 = 19,
        Unknown20 = 20,
        Unknown21 = 21,
        Unknown22 = 22,
        Unknown23 = 23,
        Scale = 24,
        AlphaCompareRef0 = 119,
        AlphaCompareRef1 = 120,
        Tex1Scale = 44,
        Tex1Rot = 68,
        Tex1Trans = 80,
        Tex2Scale = 52,
        Tex2Rot = 72,
        Tex2Trans = 88,
        TexIndScale = 60,
        TexIndRot = 76,
        TexIndTrans = 96
    }

    public enum v9AnimCurveTargetRotateFloat //curve flag = 6, 3 when baking
    {
        //Updates: ParticleParam
        Rotate = 32
    }

    public enum v9AnimCurveTargetPtclTex //curve flag = 4
    {
        //Updates: ParticleParam
        Tex1 = 104,
        Tex2 = 108,
        TexInd = 112
    }

    public enum v9AnimCurveTargetChild //curve flag = 5
    {
        //Updates: child
        Child = 0
    }

    public enum v7AnimCurveTargetChild2 //curve flag = 5
    {
        //Updates: child
        Child = 26
    }

    public enum v9AnimCurveTargetField //curve flag = 7
    {
        //Updates: Field
        Gravity = 0,
        Speed = 1,
        Magnet = 2,
        Newton = 3,
        Vortex = 4,
        Spin = 6,
        Random = 7,
        Tail = 8
    }

    public enum v9AnimCurveTargetPostField //curve flag = 2
    {
        //Updates: PostFieldInfo.AnimatableParams
        Size = 0,
        Rotate = 12,
        Translate = 24
    }

    public enum v9AnimCurveTargetEmitterFloat //curve flag = 11
    {
        //Updates: EmitterParam
        CommonParam = 44,
        Scale = 124,
        Rotate = 136,
        Translate = 112,
        SpeedOrig = 72,
        SpeedYAxis = 76,
        SpeedRandom = 80,
        SpeedNormal = 84,
        SpeedSpecDir = 92,
        Emission = 8
    }

    //This is the original enum
    public enum AnimCurveTarget
    {
        //Update target: ParticleParam
        //curveFlag = 0(u8) or 3(f32)
        COLOR0PRI = 0,
        ALPHA0PRI = 3,
        COLOR0SEC = 4,
        ALPHA0SEC = 7,
        COLOR1PRI = 8,
        ALPHA1PRI = 11,
        COLOR1SEC = 12,
        ALPHA1SEC = 15,
        SIZE = 16,
        SCALE = 24,
        ACMPREF0 = 119,
        ACMPREF1 = 120,
        TEXTURE1SCALE = 44,
        TEXTURE1ROTATE = 68,
        TEXTURE1TRANSLATE = 80,
        TEXTURE2SCALE = 52,
        TEXTURE2ROTATE = 72,
        TEXTURE2TRANSLATE = 88,
        TEXTUREINDSCALE = 60,
        TEXTUREINDROTATE = 76,
        TEXTUREINDTRANSLATE = 96,

        //curveFlag = 6 (3 when baking)
        ROTATE = 32,

        //curveFlag = 4
        TEXTURE1 = 104,
        TEXTURE2 = 108,
        TEXTUREIND = 112,

        //Update target: child
        //curveFlag = 5
        CHILD = 0,

        //Update target: Field
        //curveFlag = 7
        FIELD_GRAVITY = 0,
        FIELD_SPEED = 1,
        FIELD_MAGNET = 2,
        FIELD_NEWTON = 3,
        FIELD_VORTEX = 4,
        FIELD_SPIN = 6,
        FIELD_RANDOM = 7,
        FIELD_TAIL = 8,

        //Update target: PostFieldInfo::AnimatableParams
        //curveFlag = 2
        POSTFIELD_SIZE = 0,
        POSTFIELD_ROTATE = 12,
        POSTFIELD_TRANSLATE = 24,

        //Update target: EmitterParam
        //curveFlag = 11 (all f32)
        EMIT_COMMONPARAM = 44,
        EMIT_SCALE = 124,
        EMIT_ROTATE = 136,
        EMIT_TRANSLATE = 112,
        EMIT_SPEED_ORIG = 72,
        EMIT_SPEED_YAXIS = 76,
        EMIT_SPEED_RANDOM = 80,
        EMIT_SPEED_NORMAL = 84,
        EMIT_SPEED_SPECDIR = 92,
        EMIT_EMISSION = 8
    }

    public enum AnimCurveTarget7
    {
        //Update target: ParticleParam
        //curveFlag = 0(u8) or 3(f32)
        COLOR0PRI = 1,
        ALPHA0PRI = 2,
        COLOR0SEC = 3,
        ALPHA0SEC = 4,
        COLOR1PRI = 5,
        ALPHA1PRI = 6,
        COLOR1SEC = 7,
        ALPHA1SEC = 8,
        SIZE = 9,
        SCALE = 10,
        ACMPREF0 = 11,
        ACMPREF1 = 12,
        TEXTURE1SCALE = 13,
        TEXTURE1ROTATE = 14,
        TEXTURE1TRANSLATE = 15,
        TEXTURE2SCALE = 16,
        TEXTURE2ROTATE = 17,
        TEXTURE2TRANSLATE = 18,
        TEXTUREINDSCALE = 19,
        TEXTUREINDROTATE = 20,
        TEXTUREINDTRANSLATE = 21,

        //curveFlag = 6 (3 when baking)
        ROTATE = 22,

        //curveFlag = 4
        TEXTURE1 = 23,
        TEXTURE2 = 24,
        TEXTUREIND = 25,

        //Update target: child
        //curveFlag = 5
        CHILD = 26,

        //Update target: Field
        //curveFlag = 7
        FIELD_GRAVITY = 27,
        FIELD_SPEED = 28,
        FIELD_MAGNET = 29,
        FIELD_NEWTON = 30,
        FIELD_VORTEX = 31,
        FIELD_SPIN = 32,
        FIELD_RANDOM = 33,
        FIELD_TAIL = 34,

        //Update target: PostFieldInfo::AnimatableParams
        //curveFlag = 2
        POSTFIELD_SIZE = 35,
        POSTFIELD_ROTATE = 36,
        POSTFIELD_TRANSLATE = 37,

        //Update target: EmitterParam
        //curveFlag = 11 (all f32)
        EMIT_COMMONPARAM = 38,
        EMIT_SCALE = 39,
        EMIT_ROTATE = 40,
        EMIT_TRANSLATE = 41,
        EMIT_SPEED_ORIG = 42,
        EMIT_SPEED_YAXIS = 43,
        EMIT_SPEED_RANDOM = 44,
        EMIT_SPEED_NORMAL = 45,
        EMIT_SPEED_SPECDIR = 46,
        EMIT_EMISSION = 47
    }

    //This determines the target interpolation mode from one point to another
    public enum AnimCurveType
    {
        ParticleByte = 0,
        ParticleFloat = 3,
        ParticleRotate = 6,
        ParticleTexture = 4,
        Child = 5,
        Field = 7,
        PostField = 2,
        EmitterFloat = 11
    }

    [Flags]
    public enum AnimCurveHeaderProcessFlagType
    {
        SyncRAnd = 0x04,
        Stop = 0x08,
        EmitterTiming = 0x10,
        InfiniteLoop = 0x20,
        Turn = 0x40,
        Fitting = 0x80
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AnimCurveHeader
    {
        public const int Size = 0x20;

        public byte magic; //0xAC
        public byte kindType;
        public byte curveFlag;
        public Bin8 kindEnable;
        public byte processFlag;
        public byte loopCount;
        public bushort randomSeed;
        public bushort frameLength;
        public bushort padding;
        public buint keyTable;
        public buint rangeTable;
        public buint randomTable;
        public buint nameTable;
        public buint infoTable;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct AnimCurveTableHeader
    {
        public bushort _count;
        public bushort _pad;

        public AnimCurveKeyHeader* First => (AnimCurveKeyHeader*) (Address + 4);

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
    public unsafe struct AnimCurveKeyHeader
    {
        public bushort _count;
        public bushort _pad;

        public VoidPtr Data => Address + 8;

        public AnimCurveKeyHeader* Next(int typeCount, int typeSize)
        {
            return (AnimCurveKeyHeader*) (Data + typeCount * typeSize + 4);
        }

        public uint GetFrameIndex(int typeCount, int typeSize)
        {
            return *(buint*) (Data + typeCount * typeSize);
        }

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
    public unsafe struct PostFieldInfo
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct AnimatableParams
        {
            public BVec3 mSize;
            public BVec3 mRotate;
            public BVec3 mTranslate;
        }

        public AnimatableParams mAnimatableParams;

        public enum ControlSpeedType
        {
            None = 0,
            Limit = 1,
            Terminate = 2
        }

        public bfloat mReferenceSpeed;
        public byte mControlSpeedType;

        public enum CollisionShapeType
        {
            Plane = 0,
            Rectangle = 1,
            Circle = 2,
            Cube = 3,
            Sphere = 4,
            Cylinder = 5
        }

        public byte mCollisionShapeType;

        public enum ShapeOption
        {
            XZ = 0x00,
            XY = 0x01,
            YZ = 0x02,
            Whole = 0x40,
            Top = 0x41,
            Bottom = 0x42,
            None = 0x50
        }

        public enum ShapeOptionPlane
        {
            XZ = 0,
            XY = 1,
            YZ = 2
        }

        public enum ShapeOptionSphere
        {
            Whole = 0,
            Top = 1,
            Bottom = 2
        }

        public byte mCollisionShapeOption; // ShapeOptionPlane | ShapeOptionSphere

        public enum CollisionType
        {
            Border = 0, // Border
            Inner = 1,  // Inside, +X, +Y, +Z
            Outer = 2   // Outside, -X, -Y, -Z
        }

        public byte mCollisionType;

        [Flags]
        public enum CollisionOption
        {
            EmitterOriented = 0x1,  // Emitter center
            Bounce = 0x2,           // Reflection
            ControlSpeed = 0x8,     // When speed is controlled in some way other than through reflection or wraparound
            CreateChildPtcl = 0x10, // Child creation (particle creation)
            CreateChildEmit = 0x20, // Child creation (emitter creation)
            Delete = 0x40           // Delete
        }

        public bushort mCollisionOption;

        public bushort mStartFrame;

        public BVec3 mSpeedFactor; // (1,1,1) if not controlled

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ChildOption
        {
            private EmitterInheritSetting mInheritSetting;
            public bushort mNameIdx;
        }

        public ChildOption mChildOption;

        [Flags]
        public enum WrapOption
        {
            Enable = 1, // If 0, the Wrap feature is not used

            CenterOrigin = 0 << 1, // Center of the global origin
            CenterEmitter = 1 << 1 // Emitter center
        }

        public byte mWrapOption; // Bitwise OR of enum WrapOption

        public fixed byte padding[3];

        public BVec3 mWrapScale;
        public BVec3 mWrapRotate;
        public BVec3 mWrapTranslate;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EmitterInheritSetting
    {
        public enum EmitterInheritSettingFlag
        {
            FollowEmitter = 1,
            InheritRotate = 2
        }

        public bshort speed;
        public byte scale;
        public byte alpha;
        public byte color;
        public byte weight;
        public byte type;
        public byte flag;
        public byte alphaFuncPri;
        public byte alphaFuncSec;
    }
}