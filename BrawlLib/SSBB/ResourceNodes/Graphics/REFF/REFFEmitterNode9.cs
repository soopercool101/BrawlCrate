using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Graphics;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class REFFEmitterNode9 : ResourceNode
    {
        internal EmitterDesc* Descriptor => (EmitterDesc*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        private EmitterDesc _desc;
        private EmitterDrawSetting9 _drawSetting;

        [Category("Emitter Descriptor")]
        public EmitterDesc.EmitterCommonFlag CommonFlag
        {
            get => (EmitterDesc.EmitterCommonFlag) (uint) _desc._commonFlag;
            set
            {
                _desc._commonFlag = (uint) value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public uint EmitFlag
        {
            get => _desc._emitFlag;
            set
            {
                _desc._emitFlag = value;
                SignalPropertyChange();
            }
        } // EmitFormType - value & 0xFF

        [Category("Emitter Descriptor")]
        public ushort EmitLife
        {
            get => _desc._emitLife;
            set
            {
                _desc._emitLife = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public ushort PtclLife
        {
            get => _desc._ptclLife;
            set
            {
                _desc._ptclLife = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public sbyte PtclLifeRandom
        {
            get => _desc._ptclLifeRandom;
            set
            {
                _desc._ptclLifeRandom = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public sbyte InheritChildPtclTranslate
        {
            get => _desc._inheritChildPtclTranslate;
            set
            {
                _desc._inheritChildPtclTranslate = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public sbyte EmitIntervalRandom
        {
            get => _desc._emitEmitIntervalRandom;
            set
            {
                _desc._emitEmitIntervalRandom = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public sbyte EmitRandom
        {
            get => _desc._emitEmitRandom;
            set
            {
                _desc._emitEmitRandom = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public float Emit
        {
            get => _desc._emitEmit;
            set
            {
                _desc._emitEmit = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public ushort EmitStart
        {
            get => _desc._emitEmitStart;
            set
            {
                _desc._emitEmitStart = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public ushort EmitPast
        {
            get => _desc._emitEmitPast;
            set
            {
                _desc._emitEmitPast = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public ushort EmitInterval
        {
            get => _desc._emitEmitInterval;
            set
            {
                _desc._emitEmitInterval = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public sbyte InheritPtclTranslate
        {
            get => _desc._inheritPtclTranslate;
            set
            {
                _desc._inheritPtclTranslate = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public sbyte InheritChildEmitTranslate
        {
            get => _desc._inheritChildEmitTranslate;
            set
            {
                _desc._inheritChildEmitTranslate = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public float CommonParam1
        {
            get => _desc._commonParam1;
            set
            {
                _desc._commonParam1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public float CommonParam2
        {
            get => _desc._commonParam2;
            set
            {
                _desc._commonParam2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public float CommonParam3
        {
            get => _desc._commonParam3;
            set
            {
                _desc._commonParam3 = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public float CommonParam4
        {
            get => _desc._commonParam4;
            set
            {
                _desc._commonParam4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public float CommonParam5
        {
            get => _desc._commonParam5;
            set
            {
                _desc._commonParam5 = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public float CommonParam6
        {
            get => _desc._commonParam6;
            set
            {
                _desc._commonParam6 = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public ushort EmitEmitDiv
        {
            get => _desc._emitEmitDiv;
            set
            {
                _desc._emitEmitDiv = value;
                SignalPropertyChange();
            }
        } //aka orig tick

        [Category("Emitter Descriptor")]
        public sbyte VelInitVelocityRandom
        {
            get => _desc._velInitVelocityRandom;
            set
            {
                _desc._velInitVelocityRandom = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public sbyte VelMomentumRandom
        {
            get => _desc._velMomentumRandom;
            set
            {
                _desc._velMomentumRandom = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public float VelPowerRadiationDir
        {
            get => _desc._velPowerRadiationDir;
            set
            {
                _desc._velPowerRadiationDir = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public float VelPowerYAxis
        {
            get => _desc._velPowerYAxis;
            set
            {
                _desc._velPowerYAxis = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public float VelPowerRandomDir
        {
            get => _desc._velPowerRandomDir;
            set
            {
                _desc._velPowerRandomDir = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public float VelPowerNormalDir
        {
            get => _desc._velPowerNormalDir;
            set
            {
                _desc._velPowerNormalDir = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public float VelDiffusionEmitterNormal
        {
            get => _desc._velDiffusionEmitterNormal;
            set
            {
                _desc._velDiffusionEmitterNormal = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public float VelPowerSpecDir
        {
            get => _desc._velPowerSpecDir;
            set
            {
                _desc._velPowerSpecDir = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public float VelDiffusionSpecDir
        {
            get => _desc._velDiffusionSpecDir;
            set
            {
                _desc._velDiffusionSpecDir = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 VelSpecDir
        {
            get => _desc._velSpecDir;
            set
            {
                _desc._velSpecDir = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Scale
        {
            get => _desc._scale;
            set
            {
                _desc._scale = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Rotate
        {
            get => _desc._rotate;
            set
            {
                _desc._rotate = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Translate
        {
            get => _desc._translate;
            set
            {
                _desc._translate = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public byte LodNear
        {
            get => _desc._lodNear;
            set
            {
                _desc._lodNear = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public byte LodFar
        {
            get => _desc._lodFar;
            set
            {
                _desc._lodFar = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public byte LodMinEmit
        {
            get => _desc._lodMinEmit;
            set
            {
                _desc._lodMinEmit = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public byte LodAlpha
        {
            get => _desc._lodAlpha;
            set
            {
                _desc._lodAlpha = value;
                SignalPropertyChange();
            }
        }

        [Category("Emitter Descriptor")]
        public uint RandomSeed
        {
            get => _desc._randomSeed;
            set
            {
                _desc._randomSeed = value;
                SignalPropertyChange();
            }
        }

        //[Category("Emitter Descriptor")]
        //public byte userdata1 { get { fixed (byte* dat = desc.userdata) return dat[0]; } set { fixed (byte* dat = desc.userdata) dat[0] = value; SignalPropertyChange(); } }
        //[Category("Emitter Descriptor")]
        //public byte userdata2 { get { fixed (byte* dat = desc.userdata) return dat[1]; } set { fixed (byte* dat = desc.userdata) dat[1] = value; SignalPropertyChange(); } }
        //[Category("Emitter Descriptor")]
        //public byte userdata3 { get { fixed (byte* dat = desc.userdata) return dat[2]; } set { fixed (byte* dat = desc.userdata) dat[2] = value; SignalPropertyChange(); } }
        //[Category("Emitter Descriptor")]
        //public byte userdata4 { get { fixed (byte* dat = desc.userdata) return dat[3]; } set { fixed (byte* dat = desc.userdata) dat[3] = value; SignalPropertyChange(); } }
        //[Category("Emitter Descriptor")]
        //public byte userdata5 { get { fixed (byte* dat = desc.userdata) return dat[4]; } set { fixed (byte* dat = desc.userdata) dat[4] = value; SignalPropertyChange(); } }
        //[Category("Emitter Descriptor")]
        //public byte userdata6 { get { fixed (byte* dat = desc.userdata) return dat[5]; } set { fixed (byte* dat = desc.userdata) dat[5] = value; SignalPropertyChange(); } }
        //[Category("Emitter Descriptor")]
        //public byte userdata7 { get { fixed (byte* dat = desc.userdata) return dat[6]; } set { fixed (byte* dat = desc.userdata) dat[6] = value; SignalPropertyChange(); } }
        //[Category("Emitter Descriptor")]
        //public byte userdata8 { get { fixed (byte* dat = desc.userdata) return dat[7]; } set { fixed (byte* dat = desc.userdata) dat[7] = value; SignalPropertyChange(); } }

        #region Draw Settings

        [Category("Draw Settings")]
        public DrawFlag mFlags
        {
            get => (DrawFlag) (ushort) _drawSetting.mFlags;
            set
            {
                _drawSetting.mFlags = (ushort) value;
                SignalPropertyChange();
            }
        }

        [Category("Draw Settings")]
        public AlphaCompare AlphaComparison0
        {
            get => (AlphaCompare) _drawSetting.mACmpComp0;
            set
            {
                _drawSetting.mACmpComp0 = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Draw Settings")]
        public AlphaCompare AlphaComparison1
        {
            get => (AlphaCompare) _drawSetting.mACmpComp1;
            set
            {
                _drawSetting.mACmpComp1 = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Draw Settings")]
        public AlphaOp AlphaCompareOperation
        {
            get => (AlphaOp) _drawSetting.mACmpOp;
            set
            {
                _drawSetting.mACmpOp = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Draw Settings")]
        public byte TevStageCount
        {
            get => _drawSetting.mNumTevs;
            set
            {
                _drawSetting.mNumTevs = value;
                SignalPropertyChange();
            }
        }

        [Category("Draw Settings")]
        public bool FlagClamp
        {
            get => _drawSetting.mFlagClamp != 0;
            set
            {
                _drawSetting.mFlagClamp = (byte) (value ? 1 : 0);
                SignalPropertyChange();
            }
        }

        [Category("Draw Settings")]
        public IndirectTargetStage IndirectTargetStage
        {
            get => (IndirectTargetStage) _drawSetting.mIndirectTargetStage;
            set
            {
                _drawSetting.mIndirectTargetStage = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("TEV")]
        public byte mTevTexture1
        {
            get => _drawSetting.mTevTexture1;
            set
            {
                _drawSetting.mTevTexture1 = value;
                SignalPropertyChange();
            }
        }

        [Category("TEV")]
        public byte mTevTexture2
        {
            get => _drawSetting.mTevTexture2;
            set
            {
                _drawSetting.mTevTexture2 = value;
                SignalPropertyChange();
            }
        }

        [Category("TEV")]
        public byte mTevTexture3
        {
            get => _drawSetting.mTevTexture3;
            set
            {
                _drawSetting.mTevTexture3 = value;
                SignalPropertyChange();
            }
        }

        [Category("TEV")]
        public byte mTevTexture4
        {
            get => _drawSetting.mTevTexture4;
            set
            {
                _drawSetting.mTevTexture4 = value;
                SignalPropertyChange();
            }
        }

        //BlendMode
        [Category("Blend Mode")]
        public GXBlendMode BlendType
        {
            get => (GXBlendMode) _drawSetting.mBlendMode.mType;
            set
            {
                _drawSetting.mBlendMode.mType = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Blend Mode")]
        public BlendFactor SrcFactor
        {
            get => (BlendFactor) _drawSetting.mBlendMode.mSrcFactor;
            set
            {
                _drawSetting.mBlendMode.mSrcFactor = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Blend Mode")]
        public BlendFactor DstFactor
        {
            get => (BlendFactor) _drawSetting.mBlendMode.mDstFactor;
            set
            {
                _drawSetting.mBlendMode.mDstFactor = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Blend Mode")]
        public GXLogicOp Operation
        {
            get => (GXLogicOp) _drawSetting.mBlendMode.mOp;
            set
            {
                _drawSetting.mBlendMode.mOp = (byte) value;
                SignalPropertyChange();
            }
        }

        //Color
        [Category("Color Input")]
        public ColorInput.RasColor RasterColor
        {
            get => (ColorInput.RasColor) _drawSetting.mColorInput.mRasColor;
            set
            {
                _drawSetting.mColorInput.mRasColor = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Color Input")]
        public ColorInput.TevColor TevColor1
        {
            get => (ColorInput.TevColor) _drawSetting.mColorInput.mTevColor1;
            set
            {
                _drawSetting.mColorInput.mTevColor1 = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Color Input")]
        public ColorInput.TevColor TevColor2
        {
            get => (ColorInput.TevColor) _drawSetting.mColorInput.mTevColor2;
            set
            {
                _drawSetting.mColorInput.mTevColor2 = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Color Input")]
        public ColorInput.TevColor TevColor3
        {
            get => (ColorInput.TevColor) _drawSetting.mColorInput.mTevColor3;
            set
            {
                _drawSetting.mColorInput.mTevColor3 = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Color Input")]
        public ColorInput.TevColor TevKColor1
        {
            get => (ColorInput.TevColor) _drawSetting.mColorInput.mTevKColor1;
            set
            {
                _drawSetting.mColorInput.mTevKColor1 = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Color Input")]
        public ColorInput.TevColor TevKColor2
        {
            get => (ColorInput.TevColor) _drawSetting.mColorInput.mTevKColor2;
            set
            {
                _drawSetting.mColorInput.mTevKColor2 = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Color Input")]
        public ColorInput.TevColor TevKColor3
        {
            get => (ColorInput.TevColor) _drawSetting.mColorInput.mTevKColor3;
            set
            {
                _drawSetting.mColorInput.mTevKColor3 = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Color Input")]
        public ColorInput.TevColor TevKColor4
        {
            get => (ColorInput.TevColor) _drawSetting.mColorInput.mTevKColor4;
            set
            {
                _drawSetting.mColorInput.mTevKColor4 = (byte) value;
                SignalPropertyChange();
            }
        }

        //Alpha
        [Category("Alpha Input")]
        public ColorInput.RasColor AlphaRasColor
        {
            get => (ColorInput.RasColor) _drawSetting.mAlphaInput.mRasColor;
            set
            {
                _drawSetting.mAlphaInput.mRasColor = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Alpha Input")]
        public ColorInput.TevColor AlphaTevColor1
        {
            get => (ColorInput.TevColor) _drawSetting.mAlphaInput.mTevColor1;
            set
            {
                _drawSetting.mAlphaInput.mTevColor1 = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Alpha Input")]
        public ColorInput.TevColor AlphaTevColor2
        {
            get => (ColorInput.TevColor) _drawSetting.mAlphaInput.mTevColor2;
            set
            {
                _drawSetting.mAlphaInput.mTevColor2 = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Alpha Input")]
        public ColorInput.TevColor AlphaTevColor3
        {
            get => (ColorInput.TevColor) _drawSetting.mAlphaInput.mTevColor3;
            set
            {
                _drawSetting.mAlphaInput.mTevColor3 = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Alpha Input")]
        public ColorInput.TevColor AlphaTevKColor1
        {
            get => (ColorInput.TevColor) _drawSetting.mAlphaInput.mTevKColor1;
            set
            {
                _drawSetting.mAlphaInput.mTevKColor1 = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Alpha Input")]
        public ColorInput.TevColor AlphaTevKColor2
        {
            get => (ColorInput.TevColor) _drawSetting.mAlphaInput.mTevKColor2;
            set
            {
                _drawSetting.mAlphaInput.mTevKColor2 = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Alpha Input")]
        public ColorInput.TevColor AlphaTevKColor3
        {
            get => (ColorInput.TevColor) _drawSetting.mAlphaInput.mTevKColor3;
            set
            {
                _drawSetting.mAlphaInput.mTevKColor3 = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Alpha Input")]
        public ColorInput.TevColor AlphaTevKColor4
        {
            get => (ColorInput.TevColor) _drawSetting.mAlphaInput.mTevKColor4;
            set
            {
                _drawSetting.mAlphaInput.mTevKColor4 = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Draw Settings")]
        public GXCompare ZCompareFunc
        {
            get => (GXCompare) _drawSetting.mZCompareFunc;
            set
            {
                _drawSetting.mZCompareFunc = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Draw Settings")]
        public AlphaFlickType AlphaFlickType
        {
            get => (AlphaFlickType) _drawSetting.mAlphaFlickType;
            set
            {
                _drawSetting.mAlphaFlickType = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Draw Settings")]
        public ushort AlphaFlickCycle
        {
            get => _drawSetting.mAlphaFlickCycle;
            set
            {
                _drawSetting.mAlphaFlickCycle = value;
                SignalPropertyChange();
            }
        }

        [Category("Draw Settings")]
        public byte AlphaFlickRandom
        {
            get => _drawSetting.mAlphaFlickRandom;
            set
            {
                _drawSetting.mAlphaFlickRandom = value;
                SignalPropertyChange();
            }
        }

        [Category("Draw Settings")]
        public byte AlphaFlickAmplitude
        {
            get => _drawSetting.mAlphaFlickAmplitude;
            set
            {
                _drawSetting.mAlphaFlickAmplitude = value;
                SignalPropertyChange();
            }
        }

        //mLighting 
        [Category("Lighting")]
        public Lighting.Mode Mode
        {
            get => (Lighting.Mode) _drawSetting.mLighting.mMode;
            set
            {
                _drawSetting.mLighting.mMode = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Lighting")]
        public Lighting.Type LightType
        {
            get => (Lighting.Type) _drawSetting.mLighting.mType;
            set
            {
                _drawSetting.mLighting.mMode = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Lighting")]
        [TypeConverter(typeof(RGBAStringConverter))]
        public RGBAPixel Ambient
        {
            get => _drawSetting.mLighting.mAmbient;
            set
            {
                _drawSetting.mLighting.mAmbient = value;
                SignalPropertyChange();
            }
        }

        [Category("Lighting")]
        [TypeConverter(typeof(RGBAStringConverter))]
        public RGBAPixel Diffuse
        {
            get => _drawSetting.mLighting.mDiffuse;
            set
            {
                _drawSetting.mLighting.mDiffuse = value;
                SignalPropertyChange();
            }
        }

        [Category("Lighting")]
        public float Radius
        {
            get => _drawSetting.mLighting.mRadius;
            set
            {
                _drawSetting.mLighting.mRadius = value;
                SignalPropertyChange();
            }
        }

        [Category("Lighting")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Position
        {
            get => _drawSetting.mLighting.mPosition;
            set
            {
                _drawSetting.mLighting.mPosition = value;
                SignalPropertyChange();
            }
        }

        //[Category("Draw Settings")]
        //public fixed float mIndTexOffsetMtx[6] { get { return drawSetting.mFlags; } set { drawSetting.mFlags = value; SignalPropertyChange(); } } //2x3 Matrix
        [Category("Draw Settings")]
        public sbyte IndTexScaleExp
        {
            get => _drawSetting.mIndTexScaleExp;
            set
            {
                _drawSetting.mIndTexScaleExp = value;
                SignalPropertyChange();
            }
        }

        [Category("Draw Settings")]
        public sbyte PivotX
        {
            get => _drawSetting.pivotX;
            set
            {
                _drawSetting.pivotX = value;
                SignalPropertyChange();
            }
        }

        [Category("Draw Settings")]
        public sbyte PivotY
        {
            get => _drawSetting.pivotY;
            set
            {
                _drawSetting.pivotY = value;
                SignalPropertyChange();
            }
        }

        //[Category("Draw Settings")]
        //public byte padding { get { return drawSetting.padding; } set { drawSetting.padding = value; SignalPropertyChange(); } }
        [Category("Particle Settings")]
        public ReffType ParticleType
        {
            get => (ReffType) _drawSetting.ptcltype;
            set
            {
                if (!(ParticleType >= ReffType.Stripe && value >= ReffType.Stripe))
                {
                    typeOption2._data = 0;
                }

                _drawSetting.ptcltype = (byte) value;

                SignalPropertyChange();
                UpdateProperties();
            }
        }

        [Category("Particle Settings")]
        [TypeConverter(typeof(DropDownListReffBillboardAssist))]
        public string BillboardAssist
        {
            get
            {
                if (ParticleType == ReffType.Billboard)
                {
                    return ((BillboardAssist) _drawSetting.typeOption).ToString();
                }

                return "";
            }
            set
            {
                if (ParticleType == ReffType.Billboard && !string.IsNullOrEmpty(value))
                {
                    _drawSetting.typeOption = (byte) (BillboardAssist) Enum.Parse(typeof(BillboardAssist), value);
                    SignalPropertyChange();
                }
            }
        }

        [Category("Particle Settings")]
        [TypeConverter(typeof(DropDownListReffStripeAssist))]
        public string StripeAssist
        {
            get
            {
                if (ParticleType >= ReffType.Stripe)
                {
                    return ((StripeAssist) _drawSetting.typeOption).ToString();
                }

                return "";
            }
            set
            {
                if (ParticleType >= ReffType.Stripe && !string.IsNullOrEmpty(value))
                {
                    _drawSetting.typeOption = (byte) (StripeAssist) Enum.Parse(typeof(StripeAssist), value);
                    SignalPropertyChange();
                }
            }
        }

        [Category("Particle Settings")]
        [TypeConverter(typeof(DropDownListReffAssist))]
        public string Assist
        {
            get
            {
                if (ParticleType != ReffType.Billboard)
                {
                    return ((Assist) _drawSetting.typeOption).ToString();
                }

                return "";
            }
            set
            {
                if (ParticleType != ReffType.Billboard && !string.IsNullOrEmpty(value))
                {
                    _drawSetting.typeOption = (byte) (Assist) Enum.Parse(typeof(Assist), value);
                    SignalPropertyChange();
                }
            }
        }

        [Category("Particle Settings")]
        [TypeConverter(typeof(DropDownListReffBillboardDirection))]
        public string BillboardDirection
        {
            get
            {
                if (ParticleType == ReffType.Billboard)
                {
                    return ((BillboardAhead) _drawSetting.typeDir).ToString();
                }

                return "";
            }
            set
            {
                if (ParticleType == ReffType.Billboard && !string.IsNullOrEmpty(value))
                {
                    _drawSetting.typeDir = (byte) (BillboardAhead) Enum.Parse(typeof(BillboardAhead), value);
                    SignalPropertyChange();
                }
            }
        }

        [Category("Particle Settings")]
        [TypeConverter(typeof(DropDownListReffDirection))]
        public string Direction
        {
            get
            {
                if (ParticleType != ReffType.Billboard)
                {
                    return ((Ahead) _drawSetting.typeOption).ToString();
                }

                return "";
            }
            set
            {
                if (ParticleType != ReffType.Billboard && !string.IsNullOrEmpty(value))
                {
                    _drawSetting.typeOption = (byte) (Ahead) Enum.Parse(typeof(Ahead), value);
                    SignalPropertyChange();
                }
            }
        }

        [Category("Particle Settings")]
        public RotateAxis TypeAxis
        {
            get => (RotateAxis) _drawSetting.typeAxis;
            set
            {
                _drawSetting.typeAxis = (byte) value;
                SignalPropertyChange();
            }
        }

        private Bin8 typeOption2;

        [Category("Particle Settings")]
        [TypeConverter(typeof(DropDownListReffStripeConnect))]
        public string StripeConnect
        {
            get
            {
                if (ParticleType >= ReffType.Stripe)
                {
                    return ((StripeConnect) typeOption2[0, 3]).ToString();
                }

                return "";
            }
            set
            {
                if (ParticleType >= ReffType.Stripe && !string.IsNullOrEmpty(value))
                {
                    typeOption2[0, 3] = (byte) (StripeConnect) Enum.Parse(typeof(StripeConnect), value);
                    SignalPropertyChange();
                }
            }
        }

        [Category("Particle Settings")]
        [TypeConverter(typeof(DropDownListReffStripeInitialPrevAxis))]
        public string StripeInitialPrevAxis
        {
            get
            {
                if (ParticleType >= ReffType.Stripe)
                {
                    return ((StripeInitialPrevAxis) typeOption2[3, 3]).ToString();
                }

                return "";
            }
            set
            {
                if (ParticleType >= ReffType.Stripe && !string.IsNullOrEmpty(value))
                {
                    typeOption2[3, 3] = (byte) (StripeInitialPrevAxis) Enum.Parse(typeof(StripeInitialPrevAxis), value);
                    SignalPropertyChange();
                }
            }
        }

        [Category("Particle Settings")]
        [TypeConverter(typeof(DropDownListReffStripeTexmapType))]
        public string StripeTexmapType
        {
            get
            {
                if (ParticleType >= ReffType.Stripe)
                {
                    return ((StripeTexmapType) typeOption2[6, 1]).ToString();
                }

                return "";
            }
            set
            {
                if (ParticleType >= ReffType.Stripe && !string.IsNullOrEmpty(value))
                {
                    typeOption2[6, 1] = (byte) (StripeTexmapType) Enum.Parse(typeof(StripeTexmapType), value);
                    SignalPropertyChange();
                }
            }
        }

        [Category("Particle Settings")]
        [TypeConverter(typeof(DropDownListReffDirectionalPivot))]
        public string DirectionalPivot
        {
            get
            {
                if (ParticleType == ReffType.Directional)
                {
                    return ((DirectionalPivot) typeOption2._data).ToString();
                }

                return "";
            }
            set
            {
                if (ParticleType == ReffType.Directional && !string.IsNullOrEmpty(value))
                {
                    typeOption2._data = (byte) (DirectionalPivot) Enum.Parse(typeof(StripeTexmapType), value);
                    SignalPropertyChange();
                }
            }
        }

        [Category("Particle Settings")]
        public string DirectionalChangeYBySpeed
        {
            get
            {
                if (ParticleType == ReffType.Directional)
                {
                    return (_drawSetting.typeOption0 != 0).ToString();
                }

                return "";
            }
            set
            {
                if (ParticleType == ReffType.Directional && !string.IsNullOrEmpty(value))
                {
                    bool.TryParse(value, out bool b);
                    _drawSetting.typeOption0 = (byte) (b ? 1 : 0);
                    SignalPropertyChange();
                }
            }
        }

        [Category("Particle Settings")]
        public string StripeTubeVertexCount
        {
            get
            {
                if (ParticleType >= ReffType.Stripe)
                {
                    return _drawSetting.typeOption0.ToString();
                }

                return "";
            }
            set
            {
                if (ParticleType >= ReffType.Stripe && !string.IsNullOrEmpty(value))
                {
                    byte.TryParse(value, out byte b);
                    if (b >= 3)
                    {
                        _drawSetting.typeOption0 = b;
                    }

                    SignalPropertyChange();
                }
            }
        }

        [Category("Particle Settings")]
        [TypeConverter(typeof(DropDownListReffDirectionalFace))]
        public string DirectionalFace
        {
            get
            {
                if (ParticleType == ReffType.Directional)
                {
                    return ((Face) _drawSetting.typeOption1).ToString();
                }

                return "";
            }
            set
            {
                if (ParticleType == ReffType.Directional && !string.IsNullOrEmpty(value))
                {
                    _drawSetting.typeOption1 = (byte) (Face) Enum.Parse(typeof(Face), value);
                    SignalPropertyChange();
                }
            }
        }

        [Category("Particle Settings")]
        public string StripeInterpDivisionCount
        {
            get
            {
                if (ParticleType == ReffType.SmoothStripe)
                {
                    return _drawSetting.typeOption1.ToString();
                }

                return "";
            }
            set
            {
                if (ParticleType == ReffType.SmoothStripe && !string.IsNullOrEmpty(value))
                {
                    byte.TryParse(value, out byte b);
                    if (b >= 1)
                    {
                        _drawSetting.typeOption1 = b;
                    }

                    SignalPropertyChange();
                }
            }
        }

        //[Category("Draw Settings")]
        //public byte padding4 { get { return drawSetting.padding4; } set { drawSetting.padding4 = value; SignalPropertyChange(); } }
        [Category("Draw Settings")]
        public float ZOffset
        {
            get => _drawSetting.zOffset;
            set
            {
                _drawSetting.zOffset = value;
                SignalPropertyChange();
            }
        }

        #endregion

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _name = "Emitter";

            _desc = *Descriptor;
            _drawSetting = _desc._drawSetting;
            typeOption2 = _drawSetting.typeOption2;

            return TevStageCount > 0;
        }

        public override void OnPopulate()
        {
            int col1 = 0;
            int colop1 = col1 + 16;
            int alpha1 = colop1 + 20;
            int alphaop1 = alpha1 + 16;
            int csel1 = alphaop1 + 20;
            for (int i = 0; i < 4; i++)
            {
                REFFTEVStage s = new REFFTEVStage(i);

                byte* p = (byte*) _drawSetting.mTevColor1.Address;
                {
                    s.kcsel = p[csel1 + i];
                    s.kasel = p[csel1 + 4 + i];

                    s.cseld = p[col1 + 4 * i + 3];
                    s.cselc = p[col1 + 4 * i + 2];
                    s.cselb = p[col1 + 4 * i + 1];
                    s.csela = p[col1 + 4 * i + 0];

                    s.cop = p[colop1 + 5 * i + 0];
                    s.cbias = p[colop1 + 5 * i + 1];
                    s.cshift = p[colop1 + 5 * i + 2];
                    s.cclamp = p[colop1 + 5 * i + 3];
                    s.cdest = p[colop1 + 5 * i + 4];

                    s.aseld = p[alpha1 + 4 * i + 3];
                    s.aselc = p[alpha1 + 4 * i + 2];
                    s.aselb = p[alpha1 + 4 * i + 1];
                    s.asela = p[alpha1 + 4 * i + 0];

                    s.aop = p[alphaop1 + 5 * i + 0];
                    s.abias = p[alphaop1 + 5 * i + 1];
                    s.ashift = p[alphaop1 + 5 * i + 2];
                    s.aclamp = p[alphaop1 + 5 * i + 3];
                    s.adest = p[alphaop1 + 5 * i + 4];
                }

                s.ti = 0;
                s.tc = 0;
                s.cc = 0;
                s.te = false;

                s.Parent = this;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x14C;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            EmitterDesc* hdr = (EmitterDesc*) address;
            *hdr = _desc;
            hdr->_drawSetting = _drawSetting;
            hdr->_drawSetting.typeOption2 = typeOption2._data;
            int col1 = 0;
            int colop1 = col1 + 16;
            int alpha1 = colop1 + 20;
            int alphaop1 = alpha1 + 16;
            int csel1 = alphaop1 + 20;
            for (int i = 0; i < 4; i++)
            {
                REFFTEVStage s = (REFFTEVStage) Children[i];

                byte* p = (byte*) hdr->_drawSetting.mTevColor1.Address;
                {
                    p[csel1 + i] = (byte) s.kcsel;
                    p[csel1 + 4 + i] = (byte) s.kasel;

                    p[col1 + 4 * i + 3] = (byte) s.cseld;
                    p[col1 + 4 * i + 2] = (byte) s.cselc;
                    p[col1 + 4 * i + 1] = (byte) s.cselb;
                    p[col1 + 4 * i + 0] = (byte) s.csela;

                    p[colop1 + 5 * i + 0] = (byte) s.cop;
                    p[colop1 + 5 * i + 1] = (byte) s.cbias;
                    p[colop1 + 5 * i + 2] = (byte) s.cshift;
                    p[colop1 + 5 * i + 3] = (byte) s.cclamp;
                    p[colop1 + 5 * i + 4] = (byte) s.cdest;

                    p[alpha1 + 4 * i + 3] = (byte) s.aseld;
                    p[alpha1 + 4 * i + 2] = (byte) s.aselc;
                    p[alpha1 + 4 * i + 1] = (byte) s.aselb;
                    p[alpha1 + 4 * i + 0] = (byte) s.asela;

                    p[alphaop1 + 5 * i + 0] = (byte) s.aop;
                    p[alphaop1 + 5 * i + 1] = (byte) s.abias;
                    p[alphaop1 + 5 * i + 2] = (byte) s.ashift;
                    p[alphaop1 + 5 * i + 3] = (byte) s.aclamp;
                    p[alphaop1 + 5 * i + 4] = (byte) s.adest;
                }
            }
        }
    }
}