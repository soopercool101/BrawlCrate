using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Modeling;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Animations;
using BrawlLib.Wii.Animations;
using BrawlLib.Wii.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe partial class MDL0MaterialNode : MDL0EntryNode, IImageSource
    {
        internal MDL0Material* Header => (MDL0Material*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.MDL0Material;
        public override bool AllowDuplicateNames => true;

        public bool _updating;

        public MDL0MaterialNode()
        {
            _chan1 = new LightChannel(
                63,
                new RGBAPixel(128, 128, 128, 255),
                new RGBAPixel(255, 255, 255, 255),
                new LightChannelControl(true, GXColorSrc.Register, GXColorSrc.Register, GXDiffuseFn.Clamped,
                    GXAttnFn.Spotlight),
                new LightChannelControl(true, GXColorSrc.Register, GXColorSrc.Register, GXDiffuseFn.Clamped,
                    GXAttnFn.Spotlight),
                this);
            _chan2 = new LightChannel(
                15,
                new RGBAPixel(0, 0, 0, 255),
                new RGBAPixel(),
                0,
                0,
                this);
        }

        #region Variables

        [Category("User Data")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public UserDataCollection UserEntries
        {
            get => _userEntries;
            set
            {
                _userEntries = value;
                SignalPropertyChange();
            }
        }

        internal UserDataCollection _userEntries = new UserDataCollection();

        public byte
            _indirectMethod1,
            _indirectMethod2,
            _indirectMethod3,
            _indirectMethod4,
            _activeStages,
            _activeIndStages,
            _depthTestBeforeTexture;

        public sbyte
            _normMapRefLight1 = -1,
            _normMapRefLight2 = -1,
            _normMapRefLight3 = -1,
            _normMapRefLight4 = -1,
            _lightSetIndex = 20,
            _fogIndex = 4;

        private Bin32 _usageFlags;
        public CullMode _cull = CullMode.Cull_Inside;
        public uint _texMtxFlags;

        public LightChannel _chan1, _chan2;
        public List<MDL0ObjectNode> _objects = new List<MDL0ObjectNode>();
        public List<XFData> XFCmds = new List<XFData>();

        public GXAlphaFunction _alphaFunc = GXAlphaFunction.Default;
        public ZMode _zMode = ZMode.Default;
        public BlendMode _blendMode = BlendMode.Default;
        public ConstantAlpha _constantAlpha = ConstantAlpha.Default;
        public MatTevColorBlock _tevColorBlock = MatTevColorBlock.Default;
        public MatTevKonstBlock _tevKonstBlock = MatTevKonstBlock.Default;
        public MatIndMtxBlock _indMtx = MatIndMtxBlock.Default;

        #endregion

        #region Attributes

        public MDL0ObjectNode[] Objects
        {
            get
            {
                if (!IsMetal)
                {
                    return _objects.ToArray();
                }

                return MetalMaterial == null ? null : MetalMaterial._objects.ToArray();
            }
        }

        #region Konstant Block

        private const string KonstDesc = @"
This color is used by the linked shader. 
In each shader stage, there are properties called ConstantColorSelection and ConstantAlphaSelection.
Those properties can use this color as an argument. This color is referred to as ";

        [Category("Shader Constant Color Block")]
        [DisplayName("Constant Color 0")]
        [TypeConverter(typeof(GXColorS10StringConverter))]
        [Description(KonstDesc + "KSel_0.")]
        public GXColorS10 ConstantColor0
        {
            get => new GXColorS10
            {
                R = _tevKonstBlock.TevReg0Lo.RB, A = _tevKonstBlock.TevReg0Lo.AG, B = _tevKonstBlock.TevReg0Hi.RB,
                G = _tevKonstBlock.TevReg0Hi.AG
            };
            set
            {
                if (!CheckIfMetal())
                {
                    _tevKonstBlock.TevReg0Lo.RB = value.R;
                    _tevKonstBlock.TevReg0Lo.AG = value.A;
                    _tevKonstBlock.TevReg0Hi.RB = value.B;
                    _tevKonstBlock.TevReg0Hi.AG = value.G;
                    k1 = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Shader Constant Color Block")]
        [DisplayName("Constant Color 1")]
        [TypeConverter(typeof(GXColorS10StringConverter))]
        [Description(KonstDesc + "KSel_1.")]
        public GXColorS10 ConstantColor1
        {
            get => new GXColorS10
            {
                R = _tevKonstBlock.TevReg1Lo.RB, A = _tevKonstBlock.TevReg1Lo.AG, B = _tevKonstBlock.TevReg1Hi.RB,
                G = _tevKonstBlock.TevReg1Hi.AG
            };
            set
            {
                if (!CheckIfMetal())
                {
                    _tevKonstBlock.TevReg1Lo.RB = value.R;
                    _tevKonstBlock.TevReg1Lo.AG = value.A;
                    _tevKonstBlock.TevReg1Hi.RB = value.B;
                    _tevKonstBlock.TevReg1Hi.AG = value.G;
                    k2 = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Shader Constant Color Block")]
        [DisplayName("Constant Color 2")]
        [TypeConverter(typeof(GXColorS10StringConverter))]
        [Description(KonstDesc + "KSel_2.")]
        public GXColorS10 ConstantColor2
        {
            get => new GXColorS10
            {
                R = _tevKonstBlock.TevReg2Lo.RB, A = _tevKonstBlock.TevReg2Lo.AG, B = _tevKonstBlock.TevReg2Hi.RB,
                G = _tevKonstBlock.TevReg2Hi.AG
            };
            set
            {
                if (!CheckIfMetal())
                {
                    _tevKonstBlock.TevReg2Lo.RB = value.R;
                    _tevKonstBlock.TevReg2Lo.AG = value.A;
                    _tevKonstBlock.TevReg2Hi.RB = value.B;
                    _tevKonstBlock.TevReg2Hi.AG = value.G;
                    k3 = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Shader Constant Color Block")]
        [DisplayName("Constant Color 3")]
        [TypeConverter(typeof(GXColorS10StringConverter))]
        [Description(KonstDesc + "KSel_3.")]
        public GXColorS10 ConstantColor3
        {
            get => new GXColorS10
            {
                R = _tevKonstBlock.TevReg3Lo.RB, A = _tevKonstBlock.TevReg3Lo.AG, B = _tevKonstBlock.TevReg3Hi.RB,
                G = _tevKonstBlock.TevReg3Hi.AG
            };
            set
            {
                if (!CheckIfMetal())
                {
                    _tevKonstBlock.TevReg3Lo.RB = value.R;
                    _tevKonstBlock.TevReg3Lo.AG = value.A;
                    _tevKonstBlock.TevReg3Hi.RB = value.B;
                    _tevKonstBlock.TevReg3Hi.AG = value.G;
                    k4 = value;
                    SignalPropertyChange();
                }
            }
        }

        #endregion

        #region Color Block

        private const string ColorDesc = @"
This color is used by the linked shader. 
In each shader stage, there are properties called Color/Alpha Selection A, B, C and D.
Those properties can use this color as an argument. This color is referred to as ";

        [Category("Shader Color Block")]
        [DisplayName("Color 0")]
        [TypeConverter(typeof(GXColorS10StringConverter))]
        [Description(ColorDesc + "Color0 and Alpha0.")]
        public GXColorS10 Color0
        {
            get => new GXColorS10
            {
                R = _tevColorBlock.TevReg1Lo.RB, A = _tevColorBlock.TevReg1Lo.AG, B = _tevColorBlock.TevReg1Hi0.RB,
                G = _tevColorBlock.TevReg1Hi0.AG
            };
            set
            {
                if (!CheckIfMetal())
                {
                    _tevColorBlock.TevReg1Lo.RB = value.R;
                    _tevColorBlock.TevReg1Lo.AG = value.A;

                    //High values are always the same...
                    _tevColorBlock.TevReg1Hi0.RB =
                        _tevColorBlock.TevReg1Hi1.RB =
                            _tevColorBlock.TevReg1Hi2.RB = value.B;
                    _tevColorBlock.TevReg1Hi0.AG =
                        _tevColorBlock.TevReg1Hi1.AG =
                            _tevColorBlock.TevReg1Hi2.AG = value.G;

                    c1 = value;

                    SignalPropertyChange();
                }
            }
        }

        [Category("Shader Color Block")]
        [DisplayName("Color 1")]
        [TypeConverter(typeof(GXColorS10StringConverter))]
        [Description(ColorDesc + "Color1 and Alpha1.")]
        public GXColorS10 Color1
        {
            get => new GXColorS10
            {
                R = _tevColorBlock.TevReg2Lo.RB, A = _tevColorBlock.TevReg2Lo.AG, B = _tevColorBlock.TevReg2Hi0.RB,
                G = _tevColorBlock.TevReg2Hi0.AG
            };
            set
            {
                if (!CheckIfMetal())
                {
                    _tevColorBlock.TevReg2Lo.RB = value.R;
                    _tevColorBlock.TevReg2Lo.AG = value.A;

                    //High values are always the same...
                    _tevColorBlock.TevReg2Hi0.RB =
                        _tevColorBlock.TevReg2Hi1.RB =
                            _tevColorBlock.TevReg2Hi2.RB = value.B;
                    _tevColorBlock.TevReg2Hi0.AG =
                        _tevColorBlock.TevReg2Hi1.AG =
                            _tevColorBlock.TevReg2Hi2.AG = value.G;

                    c2 = value;

                    SignalPropertyChange();
                }
            }
        }

        [Category("Shader Color Block")]
        [DisplayName("Color 2")]
        [TypeConverter(typeof(GXColorS10StringConverter))]
        [Description(ColorDesc + "Color2 and Alpha2.")]
        public GXColorS10 Color2
        {
            get => new GXColorS10
            {
                R = _tevColorBlock.TevReg3Lo.RB, A = _tevColorBlock.TevReg3Lo.AG, B = _tevColorBlock.TevReg3Hi0.RB,
                G = _tevColorBlock.TevReg3Hi0.AG
            };
            set
            {
                if (!CheckIfMetal())
                {
                    _tevColorBlock.TevReg3Lo.RB = value.R;
                    _tevColorBlock.TevReg3Lo.AG = value.A;

                    //High values are always the same...
                    _tevColorBlock.TevReg3Hi0.RB =
                        _tevColorBlock.TevReg3Hi1.RB =
                            _tevColorBlock.TevReg3Hi2.RB = value.B;
                    _tevColorBlock.TevReg3Hi0.AG =
                        _tevColorBlock.TevReg3Hi1.AG =
                            _tevColorBlock.TevReg3Hi2.AG = value.G;

                    c3 = value;

                    SignalPropertyChange();
                }
            }
        }

        #endregion

        #region Shader linkage

        internal MDL0ShaderNode _shader;

        [Browsable(false)]
        public MDL0ShaderNode ShaderNode
        {
            get => _shader;
            set
            {
                if (_shader == value)
                {
                    return;
                }

                _shader?._materials.Remove(this);

                if ((_shader = value) != null)
                {
                    _shader._materials.Add(this);
                    ActiveShaderStages = _shader.Stages;
                }
            }
        }

        [Browsable(true)]
        [TypeConverter(typeof(DropDownListShaders))]
        public string Shader
        {
            get => _shader == null ? null : _shader.Name;
            set
            {
                if (CheckIfMetal())
                {
                    return;
                }

                if (string.IsNullOrEmpty(value))
                {
                    ShaderNode = null;
                }
                else
                {
                    MDL0ShaderNode node = Model.FindChild($"Shaders/{value}", false) as MDL0ShaderNode;
                    if (node != null)
                    {
                        ShaderNode = node;
                    }
                }

                SignalPropertyChange();
            }
        }

        #endregion

        #region Alpha Func

        [Category("Alpha Function")]
        public byte Ref0
        {
            get => _alphaFunc._ref0;
            set
            {
                if (!CheckIfMetal())
                {
                    _alphaFunc._ref0 = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Alpha Function")]
        public AlphaCompare Comp0
        {
            get => _alphaFunc.Comp0;
            set
            {
                if (!CheckIfMetal())
                {
                    _alphaFunc.Comp0 = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Alpha Function")]
        public AlphaOp Logic
        {
            get => _alphaFunc.Logic;
            set
            {
                if (!CheckIfMetal())
                {
                    _alphaFunc.Logic = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Alpha Function")]
        public byte Ref1
        {
            get => _alphaFunc._ref1;
            set
            {
                if (!CheckIfMetal())
                {
                    _alphaFunc._ref1 = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Alpha Function")]
        public AlphaCompare Comp1
        {
            get => _alphaFunc.Comp1;
            set
            {
                if (!CheckIfMetal())
                {
                    _alphaFunc.Comp1 = value;
                    SignalPropertyChange();
                }
            }
        }

        #endregion

        #region Depth Func

        [Category("Z Mode")]
        [Description(
            "Generally this should be false if using alpha function (transparency), as transparent pixels will change the depth.")]
        public bool CompareBeforeTexture
        {
            get => _depthTestBeforeTexture != 0;
            set
            {
                if (!CheckIfMetal())
                {
                    _depthTestBeforeTexture = (byte) (value ? 1 : 0);
                    SignalPropertyChange();
                }
            }
        }

        [Category("Z Mode")]
        [Description(
            "Determines if this material's pixels should be compared to other pixels in order to obscure or be obscured.")]
        public bool EnableDepthTest
        {
            get => _zMode.EnableDepthTest;
            set
            {
                if (!CheckIfMetal())
                {
                    _zMode.EnableDepthTest = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Z Mode")]
        public bool EnableDepthUpdate
        {
            get => _zMode.EnableDepthUpdate;
            set
            {
                if (!CheckIfMetal())
                {
                    _zMode.EnableDepthUpdate = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Z Mode")]
        [Description("How this material should be compared to other materials.")]
        public GXCompare DepthFunction
        {
            get => _zMode.DepthFunction;
            set
            {
                if (!CheckIfMetal())
                {
                    _zMode.DepthFunction = value;
                    SignalPropertyChange();
                }
            }
        }

        #endregion

        #region Blend Mode

        [Category("Blend Mode")]
        [Description("This allows the textures to be semi-transparent. Cannot be used with alpha function.")]
        public bool EnableBlend
        {
            get => _blendMode.EnableBlend;
            set
            {
                if (!CheckIfMetal())
                {
                    _blendMode.EnableBlend = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Blend Mode")]
        public bool EnableBlendLogic
        {
            get => _blendMode.EnableLogicOp;
            set
            {
                if (!CheckIfMetal())
                {
                    _blendMode.EnableLogicOp = value;
                    SignalPropertyChange();
                }
            }
        }

        //These are disabled via mask
        //[Category("Blend Mode")]
        //public bool EnableDither { get { return _blendMode.EnableDither; } }
        //[Category("Blend Mode")]
        //public bool EnableColorUpdate { get { return _blendMode.EnableColorUpdate; } }
        //[Category("Blend Mode")]
        //public bool EnableAlphaUpdate { get { return _blendMode.EnableAlphaUpdate; } }

        [Category("Blend Mode")]
        public BlendFactor SrcFactor
        {
            get => _blendMode.SrcFactor;
            set
            {
                if (!CheckIfMetal())
                {
                    _blendMode.SrcFactor = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Blend Mode")]
        public GXLogicOp BlendLogicOp
        {
            get => _blendMode.LogicOp;
            set
            {
                if (!CheckIfMetal())
                {
                    _blendMode.LogicOp = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Blend Mode")]
        public BlendFactor DstFactor
        {
            get => _blendMode.DstFactor;
            set
            {
                if (!CheckIfMetal())
                {
                    _blendMode.DstFactor = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Blend Mode")]
        public bool Subtract
        {
            get => _blendMode.Subtract;
            set
            {
                if (!CheckIfMetal())
                {
                    _blendMode.Subtract = value;
                    SignalPropertyChange();
                }
            }
        }

        #endregion

        #region Constant Alpha

        [Category("Constant Alpha")]
        [DisplayName("Enabled")]
        public bool ConstantAlphaEnabled
        {
            get => _constantAlpha.Enable != 0;
            set
            {
                if (!CheckIfMetal())
                {
                    _constantAlpha.Enable = (byte) (value ? 1 : 0);
                    SignalPropertyChange();
                }
            }
        }

        [Category("Constant Alpha")]
        [DisplayName("Value")]
        public byte ConstantAlphaValue
        {
            get => _constantAlpha.Value;
            set
            {
                if (!CheckIfMetal())
                {
                    _constantAlpha.Value = value;
                    SignalPropertyChange();
                }
            }
        }

        #endregion

        #region Indirect Texturing

        [Category("Indirect Texturing")]
        public IndirectMethod IndirectMethodTex1
        {
            get => (IndirectMethod) _indirectMethod1;
            set
            {
                if (!CheckIfMetal())
                {
                    _indirectMethod1 = (byte) value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Indirect Texturing")]
        public IndirectMethod IndirectMethodTex2
        {
            get => (IndirectMethod) _indirectMethod2;
            set
            {
                if (!CheckIfMetal())
                {
                    _indirectMethod2 = (byte) value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Indirect Texturing")]
        public IndirectMethod IndirectMethodTex3
        {
            get => (IndirectMethod) _indirectMethod3;
            set
            {
                if (!CheckIfMetal())
                {
                    _indirectMethod3 = (byte) value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Indirect Texturing")]
        public IndirectMethod IndirectMethodTex4
        {
            get => (IndirectMethod) _indirectMethod4;
            set
            {
                if (!CheckIfMetal())
                {
                    _indirectMethod4 = (byte) value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Indirect Texturing")]
        public IndTexScale IndirectTex1ScaleS
        {
            get => _indMtx.SS0val.S_Scale0;
            set
            {
                if (!CheckIfMetal())
                {
                    _indMtx.SS0val.S_Scale0 = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Indirect Texturing")]
        public IndTexScale IndirectTex1ScaleT
        {
            get => _indMtx.SS0val.T_Scale0;
            set
            {
                if (!CheckIfMetal())
                {
                    _indMtx.SS0val.T_Scale0 = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Indirect Texturing")]
        public IndTexScale IndirectTex2ScaleS
        {
            get => _indMtx.SS0val.S_Scale1;
            set
            {
                if (!CheckIfMetal())
                {
                    _indMtx.SS0val.S_Scale1 = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Indirect Texturing")]
        public IndTexScale IndirectTex2ScaleT
        {
            get => _indMtx.SS0val.T_Scale1;
            set
            {
                if (!CheckIfMetal())
                {
                    _indMtx.SS0val.T_Scale1 = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Indirect Texturing")]
        public IndTexScale IndirectTex3ScaleS
        {
            get => _indMtx.SS1val.S_Scale0;
            set
            {
                if (!CheckIfMetal())
                {
                    _indMtx.SS1val.S_Scale0 = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Indirect Texturing")]
        public IndTexScale IndirectTex3ScaleT
        {
            get => _indMtx.SS1val.T_Scale0;
            set
            {
                if (!CheckIfMetal())
                {
                    _indMtx.SS1val.T_Scale0 = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Indirect Texturing")]
        public IndTexScale IndirectTex4ScaleS
        {
            get => _indMtx.SS1val.S_Scale1;
            set
            {
                if (!CheckIfMetal())
                {
                    _indMtx.SS1val.S_Scale1 = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("Indirect Texturing")]
        public IndTexScale IndirectTex4ScaleT
        {
            get => _indMtx.SS1val.T_Scale1;
            set
            {
                if (!CheckIfMetal())
                {
                    _indMtx.SS1val.T_Scale1 = value;
                    SignalPropertyChange();
                }
            }
        }

        public enum IndirectMethod
        {
            Warp = 0,
            NormalMap,
            NormalMapSpecular,
            Fur,
            Reserved0,
            Reserved1,
            User0,
            User1
        }

        #endregion

        #region Lighting Channels

        //        [Category("Lighting Channels"), Browsable(false), Description(@"
        //This is how many light channels this material uses. Minimum of 0, maximum of 2.
        //If this number is 0, all light channels in this material are ignored.
        //If this number is 1, only Light Channel 1 is applied. 
        //If this number is 2, both channels are applied.")]
        //        public int ActiveLightChannels { get { return _numLights; } set { if (!CheckIfMetal()) _numLights = (byte)value.Clamp(0, 2); } }
        [Category("Lighting Channels")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [Description(
            @"Takes light input from the SCN0 and blends it with color input taken from the Material Source (if register) or from color nodes attached to the object (if vertex), then passes it to ColorChannel0 in each shader stage.")]
        public LightChannel LightChannel0 => _chan1;

        [Category("Lighting Channels")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [Description(
            @"Takes light input from the SCN0 and blends it with color input taken from the Material Source (if register) or from color nodes attached to the object (if vertex), then passes it to ColorChannel1 in each shader stage.")]
        public LightChannel LightChannel1 => _chan2;

        [Category("Lighting Channel 1")]
        [Browsable(false)]
        public LightingChannelFlags C1Flags
        {
            get => _chan1.Flags;
            set => _chan1.Flags = value;
        }

        [Category("Lighting Channel 1")]
        [Browsable(false)]
        [TypeConverter(typeof(RGBAStringConverter))]
        public RGBAPixel C1MaterialColor
        {
            get => _chan1.MaterialColor;
            set => _chan1.MaterialColor = value;
        }

        [Category("Lighting Channel 1")]
        [Browsable(false)]
        [TypeConverter(typeof(RGBAStringConverter))]
        public RGBAPixel C1AmbientColor
        {
            get => _chan1.AmbientColor;
            set => _chan1.AmbientColor = value;
        }

        [Category("Lighting Channel 1")]
        [Browsable(false)]
        public GXColorSrc C1ColorMaterialSource
        {
            get => _chan1.ColorMaterialSource;
            set => _chan1.ColorMaterialSource = value;
        }

        [Category("Lighting Channel 1")]
        [Browsable(false)]
        public bool C1ColorEnabled
        {
            get => _chan1.ColorEnabled;
            set => _chan1.ColorEnabled = value;
        }

        [Category("Lighting Channel 1")]
        [Browsable(false)]
        public GXColorSrc C1ColorAmbientSource
        {
            get => _chan1.ColorAmbientSource;
            set => _chan1.ColorAmbientSource = value;
        }

        [Category("Lighting Channel 1")]
        [Browsable(false)]
        public GXDiffuseFn C1ColorDiffuseFunction
        {
            get => _chan1.ColorDiffuseFunction;
            set => _chan1.ColorDiffuseFunction = value;
        }

        [Category("Lighting Channel 1")]
        [Browsable(false)]
        public GXAttnFn C1ColorAttenuation
        {
            get => _chan1.ColorAttenuation;
            set => _chan1.ColorAttenuation = value;
        }

        [Category("Lighting Channel 1")]
        [Browsable(false)]
        public MatChanLights C1ColorLights
        {
            get => _chan1.ColorLights;
            set => _chan1.ColorLights = value;
        }

        [Category("Lighting Channel 1")]
        [Browsable(false)]
        public GXColorSrc C1AlphaMaterialSource
        {
            get => _chan1.AlphaMaterialSource;
            set => _chan1.AlphaMaterialSource = value;
        }

        [Category("Lighting Channel 1")]
        [Browsable(false)]
        public bool C1AlphaEnabled
        {
            get => _chan1.AlphaEnabled;
            set => _chan1.AlphaEnabled = value;
        }

        [Category("Lighting Channel 1")]
        [Browsable(false)]
        public GXColorSrc C1AlphaAmbientSource
        {
            get => _chan1.AlphaAmbientSource;
            set => _chan1.AlphaAmbientSource = value;
        }

        [Category("Lighting Channel 1")]
        [Browsable(false)]
        public GXDiffuseFn C1AlphaDiffuseFunction
        {
            get => _chan1.AlphaDiffuseFunction;
            set => _chan1.AlphaDiffuseFunction = value;
        }

        [Category("Lighting Channel 1")]
        [Browsable(false)]
        public GXAttnFn C1AlphaAttenuation
        {
            get => _chan1.AlphaAttenuation;
            set => _chan1.AlphaAttenuation = value;
        }

        [Category("Lighting Channel 1")]
        [Browsable(false)]
        public MatChanLights C1AlphaLights
        {
            get => _chan1.AlphaLights;
            set => _chan1.AlphaLights = value;
        }

        [Category("Lighting Channel 2")]
        [Browsable(false)]
        public LightingChannelFlags C2Flags
        {
            get => _chan2.Flags;
            set => _chan2.Flags = value;
        }

        [Category("Lighting Channel 2")]
        [Browsable(false)]
        [TypeConverter(typeof(RGBAStringConverter))]
        public RGBAPixel C2MaterialColor
        {
            get => _chan2.MaterialColor;
            set => _chan2.MaterialColor = value;
        }

        [Category("Lighting Channel 2")]
        [Browsable(false)]
        [TypeConverter(typeof(RGBAStringConverter))]
        public RGBAPixel C2AmbientColor
        {
            get => _chan2.AmbientColor;
            set => _chan2.AmbientColor = value;
        }

        [Category("Lighting Channel 2")]
        [Browsable(false)]
        public GXColorSrc C2ColorMaterialSource
        {
            get => _chan2.ColorMaterialSource;
            set => _chan2.ColorMaterialSource = value;
        }

        [Category("Lighting Channel 2")]
        [Browsable(false)]
        public bool C2ColorEnabled
        {
            get => _chan2.ColorEnabled;
            set => _chan2.ColorEnabled = value;
        }

        [Category("Lighting Channel 2")]
        [Browsable(false)]
        public GXColorSrc C2ColorAmbientSource
        {
            get => _chan2.ColorAmbientSource;
            set => _chan2.ColorAmbientSource = value;
        }

        [Category("Lighting Channel 2")]
        [Browsable(false)]
        public GXDiffuseFn C2ColorDiffuseFunction
        {
            get => _chan2.ColorDiffuseFunction;
            set => _chan2.ColorDiffuseFunction = value;
        }

        [Category("Lighting Channel 2")]
        [Browsable(false)]
        public GXAttnFn C2ColorAttenuation
        {
            get => _chan2.ColorAttenuation;
            set => _chan2.ColorAttenuation = value;
        }

        [Category("Lighting Channel 2")]
        [Browsable(false)]
        public MatChanLights C2ColorLights
        {
            get => _chan2.ColorLights;
            set => _chan2.ColorLights = value;
        }

        [Category("Lighting Channel 2")]
        [Browsable(false)]
        public GXColorSrc C2AlphaMaterialSource
        {
            get => _chan2.AlphaMaterialSource;
            set => _chan2.AlphaMaterialSource = value;
        }

        [Category("Lighting Channel 2")]
        [Browsable(false)]
        public bool C2AlphaEnabled
        {
            get => _chan2.AlphaEnabled;
            set => _chan2.AlphaEnabled = value;
        }

        [Category("Lighting Channel 2")]
        [Browsable(false)]
        public GXColorSrc C2AlphaAmbientSource
        {
            get => _chan2.AlphaAmbientSource;
            set => _chan2.AlphaAmbientSource = value;
        }

        [Category("Lighting Channel 2")]
        [Browsable(false)]
        public GXDiffuseFn C2AlphaDiffuseFunction
        {
            get => _chan2.AlphaDiffuseFunction;
            set => _chan2.AlphaDiffuseFunction = value;
        }

        [Category("Lighting Channel 2")]
        [Browsable(false)]
        public GXAttnFn C2AlphaAttenuation
        {
            get => _chan2.AlphaAttenuation;
            set => _chan2.AlphaAttenuation = value;
        }

        [Category("Lighting Channel 2")]
        [Browsable(false)]
        public MatChanLights C2AlphaLights
        {
            get => _chan2.AlphaLights;
            set => _chan2.AlphaLights = value;
        }

        #endregion

        #region General Material

        [Category("Material")]
        [Description(MDL0Node._textureMatrixModeDescription)]
        public TexMatrixMode TextureMatrixMode
        {
            get => (TexMatrixMode) _texMtxFlags;
            set
            {
                if (!CheckIfMetal())
                {
                    _texMtxFlags = (uint) value;
                    foreach (MDL0MaterialRefNode r in Children)
                    {
                        r._bindState.MatrixMode = r._frameState.MatrixMode = value;
                    }

                    SignalPropertyChange();
                }
            }
        }

        [Category("Material")]
        [Description("True if this material has transparency (alpha function) enabled.")]
        public bool XLUMaterial
        {
            get => _usageFlags[31];
            set
            {
                if (!CheckIfMetal())
                {
                    //Forget all these checks.
                    //We'll let the user have complete freedom, as I've seen objects use materials
                    //as XLU when this bool wasn't on anyway.

                    //bool prev = _usageFlags[31];
                    _usageFlags[31] = value;

                    //string message = "";
                    //for (int i = 0; i < _objects.Count; i++)
                    //    _objects[i].EvalMaterials(ref message);

                    //if (message.Length != 0)
                    //    if (MessageBox.Show(null, "Are you sure you want to continue?\nThe following objects will no longer use this material:\n" + message, "Continue?", MessageBoxButtons.YesNo) == DialogResult.No)
                    //    {
                    //        _changed = false;
                    //        _usageFlags[31] = prev;
                    //        return;
                    //    }

                    //message = "";
                    //for (int i = 0; i < _objects.Count; i++)
                    //    _objects[i].FixMaterials(ref message);

                    //if (message.Length != 0)
                    //    MessageBox.Show("The following objects no longer use this material:\n" + message);

                    SignalPropertyChange();
                }
            }
        }

        [Category("Material")] public int ID => Index;

        [Category("Material")]
        [Description(@"
This dictates how many consecutive stages in the attached shader should be applied to produce the final output color.
For example, if the shader has two stages but this number is 1, the second stage in the shader will be ignored.")]
        public int ActiveShaderStages
        {
            get => _activeStages;
            set
            {
                if (!CheckIfMetal())
                {
                    _activeStages = (byte) (value > ShaderNode.Stages ? ShaderNode.Stages : value < 1 ? 1 : value);
                    SignalPropertyChange();
                }
            }
        }

        [Category("Material")]
        [Description("The number of active indirect textures in the shader.")]
        public int IndirectShaderStages
        {
            get => _activeIndStages;
            set
            {
                if (!CheckIfMetal())
                {
                    _activeIndStages = (byte) (value > 4 ? 4 : value < 0 ? 0 : value);
                    SignalPropertyChange();
                }
            }
        }

        [Category("Material")]
        [Description("This will make one, neither or both sides of the linked objects' mesh invisible." +
                     "\n- Cull_Inside: Makes inside (back faces) of model invisible" +
                     "\n- Cull_Outside: Makes outside(front faces) of model invisible" +
                     "\n- Cull_None: Makes both sides visible" +
                     "\n- Cull_All: Makes both sides invisible")]
        public CullMode CullMode
        {
            get => _cull;
            set
            {
                if (!CheckIfMetal())
                {
                    _cull = value;
                    SignalPropertyChange();
                }
            }
        }

        #endregion

        #region SCN0 References

        [Category("SCN0 References")]
        [Description(
            "This is the index of the SCN0 LightSet that should be applied to this model. Set to -1 if unused.")]
        public sbyte LightSetIndex
        {
            get => _lightSetIndex;
            set
            {
                if (!CheckIfMetal())
                {
                    _lightSetIndex = value;

                    MetalMaterial?.UpdateAsMetal();

                    SignalPropertyChange();
                }
            }
        }

        [Category("SCN0 References")]
        [Description("This is the index of the SCN0 Fog that should be applied to this model. Set to -1 if unused.")]
        public sbyte FogIndex
        {
            get => _fogIndex;
            set
            {
                if (!CheckIfMetal())
                {
                    _fogIndex = value;
                    MetalMaterial?.UpdateAsMetal();

                    SignalPropertyChange();
                }
            }
        }

        [Category("SCN0 References")]
        [Description(
            "This is the index of the SCN0 Light that should be used for indirect texture 1 if it is a normal map. Set to -1 if unused.")]
        public sbyte NormMapRefLight1
        {
            get => _normMapRefLight1;
            set
            {
                if (!CheckIfMetal())
                {
                    _normMapRefLight1 = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("SCN0 References")]
        [Description(
            "This is the index of the SCN0 Light that should be used for indirect texture 2 if it is a normal map. Set to -1 if unused.")]
        public sbyte NormMapRefLight2
        {
            get => _normMapRefLight2;
            set
            {
                if (!CheckIfMetal())
                {
                    _normMapRefLight1 = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("SCN0 References")]
        [Description(
            "This is the index of the SCN0 Light that should be used for indirect texture 3 if it is a normal map. Set to -1 if unused.")]
        public sbyte NormMapRefLight3
        {
            get => _normMapRefLight3;
            set
            {
                if (!CheckIfMetal())
                {
                    _normMapRefLight1 = value;
                    SignalPropertyChange();
                }
            }
        }

        [Category("SCN0 References")]
        [Description(
            "This is the index of the SCN0 Light that should be used for indirect texture 4 if it is a normal map. Set to -1 if unused.")]
        public sbyte NormMapRefLight4
        {
            get => _normMapRefLight4;
            set
            {
                if (!CheckIfMetal())
                {
                    _normMapRefLight1 = value;
                    SignalPropertyChange();
                }
            }
        }

        #endregion

        #endregion

        #region Metal

        public void UpdateAsMetal()
        {
            if (!IsMetal)
            {
                return;
            }

            _updating = true;
            if (ShaderNode != null && ShaderNode._autoMetal && ShaderNode._texCount == Children.Count)
            {
                //ShaderNode.DefaultAsMetal(Children.Count); 
            }
            else
            {
                bool found = false;
                foreach (MDL0ShaderNode s in Model._shadList)
                {
                    if (s._autoMetal && s._texCount == Children.Count)
                    {
                        ShaderNode = s;
                        found = true;
                    }
                    else
                    {
                        if (s.Stages == 4)
                        {
                            foreach (MDL0MaterialNode y in s._materials)
                            {
                                if (!y.IsMetal || y.Children.Count != Children.Count)
                                {
                                    goto NotFound;
                                }
                            }

                            ShaderNode = s;
                            found = true;
                            goto End;
                            NotFound:
                            continue;
                        }
                    }
                }

                End:
                if (!found)
                {
                    MDL0ShaderNode shader = new MDL0ShaderNode();
                    Model._shadGroup.AddChild(shader);
                    shader.DefaultAsMetal(Children.Count);
                    ShaderNode = shader;
                }
            }

            if (MetalMaterial != null)
            {
                Name = MetalMaterial.Name + "_ExtMtl";
                _activeStages = 4;

                if (Children.Count - 1 != MetalMaterial.Children.Count)
                {
                    //Remove all children
                    for (int i = 0; i < Children.Count; i++)
                    {
                        ((MDL0MaterialRefNode) Children[i]).TextureNode = null;
                        ((MDL0MaterialRefNode) Children[i]).PaletteNode = null;
                        RemoveChild(Children[i--]);
                    }

                    //Start over
                    for (int i = 0; i <= MetalMaterial.Children.Count; i++)
                    {
                        MDL0MaterialRefNode mr = new MDL0MaterialRefNode();

                        AddChild(mr);
                        mr.Texture = "metal00";

                        mr._bindState = TextureFrameState.Neutral;
                        mr._texMatrixEffect.TextureMatrix = Matrix34.Identity;
                        mr._texMatrixEffect.SCNCamera = -1;
                        mr._texMatrixEffect.SCNLight = -1;

                        XFTexMtxInfo info = new XFTexMtxInfo
                        {
                            Projection = TexProjection.STQ,
                            InputForm = TexInputForm.ABC1,
                            TexGenType = TexTexgenType.Regular,
                            SourceRow = TexSourceRow.Normals,
                            EmbossSource = 5,
                            EmbossLight = 0
                        };

                        if (i == MetalMaterial.Children.Count)
                        {
                            mr._minFltr = 5;
                            mr._magFltr = 1;
                            mr._lodBias = -2;
                            mr.HasTextureMatrix = true;
                            info.Projection = TexProjection.STQ;
                            info.InputForm = TexInputForm.ABC1;
                            info.SourceRow = TexSourceRow.Normals;
                            mr.Normalize = true;
                            mr.MapMode = MappingMethod.EnvCamera;
                        }
                        else
                        {
                            info.Projection = TexProjection.ST;
                            info.InputForm = TexInputForm.AB11;
                            info.SourceRow = TexSourceRow.TexCoord0 + i;
                            mr.Normalize = false;
                            mr.MapMode = MappingMethod.TexCoord;
                        }

                        info.TexGenType = TexTexgenType.Regular;
                        info.EmbossSource = 5;
                        info.EmbossLight = 0;

                        mr._texMtxFlags = info;
                    }

                    _chan1 = new LightChannel(63, new RGBAPixel(128, 128, 128, 255), new RGBAPixel(255, 255, 255, 255),
                        0, 0, this);
                    C1ColorEnabled = true;
                    C1ColorDiffuseFunction = GXDiffuseFn.Clamped;
                    C1ColorAttenuation = GXAttnFn.Spotlight;
                    C1AlphaEnabled = true;
                    C1AlphaDiffuseFunction = GXDiffuseFn.Clamped;
                    C1AlphaAttenuation = GXAttnFn.Spotlight;

                    _chan2 = new LightChannel(63, new RGBAPixel(255, 255, 255, 255), new RGBAPixel(), 0, 0, this);
                    C2ColorEnabled = true;
                    C2ColorDiffuseFunction = GXDiffuseFn.Disabled;
                    C2ColorAttenuation = GXAttnFn.Specular;
                    C2AlphaDiffuseFunction = GXDiffuseFn.Disabled;
                    C2AlphaAttenuation = GXAttnFn.Specular;

                    _lightSetIndex = MetalMaterial._lightSetIndex;
                    _fogIndex = MetalMaterial._fogIndex;

                    _cull = MetalMaterial._cull;
                    //_numLights = 2;
                    CompareBeforeTexture = true;
                    _normMapRefLight1 =
                        _normMapRefLight2 =
                            _normMapRefLight3 =
                                _normMapRefLight4 = -1;

                    SignalPropertyChange();
                }
            }

            _updating = false;
        }

        public bool CheckIfMetal()
        {
            if (Model != null && Model._autoMetal)
            {
                if (!_updating)
                {
                    if (IsMetal)
                    {
                        if (MessageBox.Show(null,
                            "This model is currently set to automatically modify metal materials.\nYou cannot make changes unless you turn it off.\nDo you want to turn it off?",
                            "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Model._autoMetal = false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        [Browsable(false)] public bool IsMetal => Name.EndsWith("_ExtMtl");

        [Browsable(false)]
        public MDL0MaterialNode MetalMaterial
        {
            get
            {
                foreach (MDL0MaterialNode t in Model._matList)
                {
                    if (!IsMetal)
                    {
                        if (t.Name.StartsWith(Name) && t.IsMetal && Name.Length + 7 == t.Name.Length)
                        {
                            return t;
                        }
                    }
                    else if (Name.StartsWith(t.Name) && !t.IsMetal && t.Name.Length + 7 == Name.Length)
                    {
                        return t;
                    }
                }

                return null;
            }
        }

        #endregion

        #region Reading & Writing

        internal int _initVersion;

        public override bool OnInitialize()
        {
            MDL0Material* header = Header;

            _initVersion = header->_pad != 0 && _replaced ? header->_pad : Model._version;

            if (_name == null && header->_stringOffset != 0)
            {
                _name = header->ResourceString;
            }

            //Get XF Commands
            XFCmds = XFData.Parse((byte*) header->DisplayLists(_initVersion) + 0xE0);

            _usageFlags = header->_usageFlags;

            _indirectMethod1 = header->_indirectMethod1;
            _indirectMethod2 = header->_indirectMethod2;
            _indirectMethod3 = header->_indirectMethod3;
            _indirectMethod4 = header->_indirectMethod4;

            _normMapRefLight1 = header->_normMapRefLight1;
            _normMapRefLight2 = header->_normMapRefLight2;
            _normMapRefLight3 = header->_normMapRefLight3;
            _normMapRefLight4 = header->_normMapRefLight4;

            _activeStages = header->_activeTEVStages;
            _activeIndStages = header->_numIndTexStages;
            _depthTestBeforeTexture = header->_depthTestBeforeTexture;

            _lightSetIndex = header->_lightSet;
            _fogIndex = header->_fogSet;

            _cull = (CullMode) (int) header->_cull;

            if (!_replaced && (-header->_mdl0Offset + header->DisplayListOffset(_initVersion)) % 0x20 != 0)
            {
                Model._errors.Add("Material " + Index + " has an improper align offset.");
            }

            MatModeBlock* mode = header->DisplayLists(_initVersion);
            _alphaFunc = mode->AlphaFunction;
            _zMode = mode->ZMode;
            _blendMode = mode->BlendMode;
            _constantAlpha = mode->ConstantAlpha;

            _tevColorBlock = *header->TevColorBlock(_initVersion);
            _tevKonstBlock = *header->TevKonstBlock(_initVersion);
            _indMtx = *header->IndMtxBlock(_initVersion);

            MDL0TexSRTData* TexMatrices = header->TexMatrices(_initVersion);
            _texMtxFlags = TexMatrices->_mtxFlags;

            MDL0MaterialLighting* Light = header->Light(_initVersion);

            (_chan1 = Light->Channel1)._parent = this;
            (_chan2 = Light->Channel2)._parent = this;

            (_userEntries = new UserDataCollection()).Read(header->UserData(_initVersion), WorkingUncompressed);

            return true;
        }

        public override void OnPopulate()
        {
            MDL0TextureRef* first = Header->First;
            for (int i = 0; i < Header->_numTextures; i++)
            {
                new MDL0MaterialRefNode().Initialize(this, first++, MDL0TextureRef.Size);
            }
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            foreach (UserDataClass s in _userEntries)
            {
                table.Add(s._name);
                if (s._type == UserValueType.String && s._entries.Count > 0)
                {
                    table.Add(s._entries[0]);
                }
            }

            foreach (MDL0MaterialRefNode n in Children)
            {
                n.GetStrings(table);
            }
        }

        internal int _dataAlign, _mdlOffset = 0;

        public override int OnCalculateSize(bool force)
        {
            int temp, size;

            //Add header and tex matrices size at start
            size = 0x414 + (Model._version >= 10 ? 4 : 0);

            //Add children size
            size += Children.Count * MDL0TextureRef.Size;

            //Add user entries, if there are any
            size += _userEntries.GetSize();

            //Set temp align offset
            temp = size;

            //Align data to an offset divisible by 0x20 using data length.
            size = size.Align(0x10) + _dataAlign;
            if ((size + _mdlOffset) % 0x20 != 0)
            {
                size += size - 0x10 >= temp ? -0x10 : 0x10;
            }

            //Reset data alignment
            _dataAlign = 0;

            //Add display list and XF flags
            size += 0x180;

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            MDL0Node model = Model;
            MDL0Material* header = (MDL0Material*) address;

            //Set offsets
            header->_dataLen = length;

            int addr = 0x414, displayListOffset = length - 0x180;
            if (model._version >= 10)
            {
                addr += 4;

                header->_dataOffset2 = 0; //Fur Data not supported
                header->_dataOffset3 = displayListOffset;
            }
            else
            {
                header->_dataOffset2 = displayListOffset;
            }

            header->_matRefOffset = Children.Count > 0 ? addr : 0;

            //Check for user entries
            if (_userEntries.Count > 0)
            {
                addr += Children.Count * 0x34;
                if (model._version >= 10)
                {
                    header->_dataOffset2 = addr;
                }
                else
                {
                    header->_dataOffset1 = addr;
                }

                _userEntries.Write(header->UserData(model._version));
            }
            else
            {
                header->_dataOffset1 = 0;
            }

            ushort i1 = 0x1040, i2 = 0x1050, mtx = 0;
            if (Model._isImport)
            {
                //Set default texgen flags
                for (int i = 0; i < Children.Count; i++, mtx += 3, i1++, i2++)
                {
                    MDL0MaterialRefNode node = (MDL0MaterialRefNode) Children[i];

                    node._bindState = TextureFrameState.Neutral;
                    node._texMatrixEffect.TextureMatrix = Matrix34.Identity;
                    node._texMatrixEffect.SCNCamera = -1;
                    node._texMatrixEffect.SCNLight = -1;
                    node._texMatrixEffect.MapMode = MappingMethod.TexCoord;

                    node._dualTexFlags = new XFDualTex(mtx, 0);
                    node._texMtxFlags = new XFTexMtxInfo(
                        TexProjection.ST,
                        TexInputForm.AB11,
                        TexTexgenType.Regular,
                        TexSourceRow.TexCoord0,
                        5, 0);

                    XFCmds.Add(new XFData(i1, node._texMtxFlags._data));
                    XFCmds.Add(new XFData(i2, node._dualTexFlags.Value));
                }
            }

            //Set header values
            header->_numTextures = Children.Count;
            header->_numTexGens = (byte) Children.Count;
            header->_index = Index;
            header->_activeTEVStages = _activeStages;
            header->_numIndTexStages = _activeIndStages;
            header->_depthTestBeforeTexture = _depthTestBeforeTexture;

            byte lights = 0;
            if (_chan1.RasterColorEnabled || _chan1.RasterAlphaEnabled)
            {
                lights++;
            }

            if (_chan2.RasterColorEnabled || _chan2.RasterAlphaEnabled)
            {
                lights++;
            }

            header->_numLightChans = lights;

            header->_lightSet = _lightSetIndex;
            header->_fogSet = _fogIndex;
            header->_pad = 0;

            header->_cull = (int) _cull;
            header->_usageFlags = _usageFlags;

            header->_indirectMethod1 = _indirectMethod1;
            header->_indirectMethod2 = _indirectMethod2;
            header->_indirectMethod3 = _indirectMethod3;
            header->_indirectMethod4 = _indirectMethod4;

            header->_normMapRefLight1 = _normMapRefLight1;
            header->_normMapRefLight2 = _normMapRefLight2;
            header->_normMapRefLight3 = _normMapRefLight3;
            header->_normMapRefLight4 = _normMapRefLight4;

            //Generate layer flags and write texture matrices
            MDL0TexSRTData* TexSettings = header->TexMatrices(model._version);
            *TexSettings = MDL0TexSRTData.Default;

            //Usage flags. Each set of 4 bits represents one texture layer.
            uint layerFlags = 0;

            //Loop through references in reverse so that layerflags is set in the right order
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                MDL0MaterialRefNode node = (MDL0MaterialRefNode) Children[i];

                TexFlags flags = TexFlags.Enabled;

                //Check for non-default values
                if (node._bindState.Scale == new Vector2(1))
                {
                    flags |= TexFlags.FixedScale;
                }

                if (node._bindState.Rotate == 0)
                {
                    flags |= TexFlags.FixedRot;
                }

                if (node._bindState.Translate == new Vector2(0))
                {
                    flags |= TexFlags.FixedTrans;
                }

                TexSettings->SetTexSRT(node._bindState, node.Index);
                TexSettings->SetTexMatrices(node._texMatrixEffect, node.Index);

                layerFlags = (layerFlags << 4) | (byte) flags;
            }

            TexSettings->_layerFlags = layerFlags;
            TexSettings->_mtxFlags = _texMtxFlags;

            //Write lighting flags
            MDL0MaterialLighting* Light = header->Light(model._version);

            Light->Channel1 = _chan1;
            Light->Channel2 = _chan2;

            //The shader offset will be written later

            //Rebuild references
            MDL0TextureRef* mRefs = header->First;
            foreach (MDL0MaterialRefNode n in Children)
            {
                n.Rebuild(mRefs++, MDL0TextureRef.Size, true);
            }

            //Set Display Lists
            *header->TevKonstBlock(model._version) = _tevKonstBlock;
            *header->TevColorBlock(model._version) = _tevColorBlock;
            *header->IndMtxBlock(model._version) = _indMtx;

            MatModeBlock* mode = header->DisplayLists(model._version);
            *mode = MatModeBlock.Default;
            if (model._isImport)
            {
                _alphaFunc = mode->AlphaFunction;
                _zMode = mode->ZMode;
                _blendMode = mode->BlendMode;
                _constantAlpha = mode->ConstantAlpha;
            }
            else
            {
                mode->AlphaFunction = _alphaFunc;
                mode->ZMode = _zMode;
                mode->BlendMode = _blendMode;
                mode->ConstantAlpha = _constantAlpha;
            }

            //Write XF flags
            i1 = 0x1040;
            i2 = 0x1050;
            mtx = 0;
            byte* xfData = (byte*) header->DisplayLists(model._version) + 0xE0;
            foreach (MDL0MaterialRefNode mr in Children)
            {
                //Tex Mtx
                *xfData++ = 0x10;
                *(bushort*) xfData = 0;
                xfData += 2;
                *(bushort*) xfData = i1++;
                xfData += 2;
                *(buint*) xfData = mr._texMtxFlags._data._data;
                xfData += 4;

                //Dual Tex
                *xfData++ = 0x10;
                *(bushort*) xfData = 0;
                xfData += 2;
                *(bushort*) xfData = i2++;
                xfData += 2;
                *(buint*) xfData = new XFDualTex(mtx, mr._dualTexFlags._normalEnable).Value;
                xfData += 4;

                mtx += 3;
            }
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0Material* header = (MDL0Material*) dataAddress;
            header->_mdl0Offset = (int) mdlAddress - (int) dataAddress;
            header->_stringOffset = (int) stringTable[Name] + 4 - (int) dataAddress;
            header->_index = Index;

            _userEntries.PostProcess(header->UserData(Model._version), stringTable);

            MDL0TextureRef* first = header->First;
            foreach (MDL0MaterialRefNode n in Children)
            {
                n.PostProcess(mdlAddress, first++, stringTable);
            }
        }

        public override void Export(string outPath)
        {
            StringTable table = new StringTable();
            GetStrings(table);
            int dataLen = CalculateSize(false);
            int totalLen = dataLen + table.GetTotalSize();

            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                FileShare.None, 8, FileOptions.RandomAccess))
            {
                stream.SetLength(totalLen);
                using (FileMap map = FileMap.FromStream(stream))
                {
                    Rebuild(map.Address, dataLen, false);
                    table.WriteTable(map.Address + dataLen);
                    PostProcess(map.Address, map.Address, table);
                    ((MDL0Material*) map.Address)->_pad = (byte) Model._version;
                }
            }
        }

        #endregion

        #region Rendering

        #region GLSL

        public override void SignalPropertyChange()
        {
            _fragShaderSource = null;
            _vertexShaderSource = null;

            if (Model != null && Model.AutoMetalMaterials && !IsMetal)
            {
                Model.GenerateMetalMaterials();
            }

            base.SignalPropertyChange();
        }

        public string _vertexShaderSource;
        public int _vertexShaderHandle;
        public string _fragShaderSource;
        public int _fragShaderHandle;
        public int _programHandle;
        public bool _internalForceRemake;

        public void UseProgram(MDL0ObjectNode node, bool forceRemake = false)
        {
            forceRemake = forceRemake || ShaderGenerator._forceRecompile;

            //If a rendering property has been changed, the shaders need to be regenerated
            bool updateVert = string.IsNullOrEmpty(_vertexShaderSource) || forceRemake;
            bool updateMatFrag = string.IsNullOrEmpty(_fragShaderSource) || forceRemake;
            bool updateShaderFrag = ShaderNode != null && ShaderNode._fragShaderSource == null || forceRemake;
            bool updateProgram = _programHandle <= 0 ||
                                 updateVert || _vertexShaderHandle <= 0 ||
                                 updateMatFrag || updateShaderFrag || _fragShaderHandle <= 0 ||
                                 _internalForceRemake;

            if (updateShaderFrag)
            {
                foreach (MDL0MaterialNode m in ShaderNode.Materials)
                {
                    m._internalForceRemake = true;
                }
            }

            _internalForceRemake = false;

            if (updateProgram)
            {
#if !DEBUG
                try
                {
#endif
                if (_programHandle > 0)
                {
                    if (_vertexShaderHandle > 0)
                    {
                        DeleteShader(ref _vertexShaderHandle);
                    }

                    if (_fragShaderHandle > 0)
                    {
                        DeleteShader(ref _fragShaderHandle);
                    }

                    GL.DeleteProgram(_programHandle);
                    _programHandle = 0;
                }

                ShaderGenerator.SetTarget(this);

                if (updateShaderFrag)
                {
                    ShaderNode._fragShaderSource = ShaderGenerator.GenTEVFragShader();
                }

                if (updateVert)
                {
                    _vertexShaderSource = ShaderGenerator.GenVertexShader();
                }

                if (updateMatFrag)
                {
                    _fragShaderSource = ShaderGenerator.GenMaterialFragShader();
                }

                string combineFrag = ShaderGenerator.CombineFragShader(
                    _fragShaderSource,
                    ShaderNode == null ? null : ShaderNode._fragShaderSource,
                    ActiveShaderStages);

                GenShader(ref _vertexShaderHandle, _vertexShaderSource, true);
                GenShader(ref _fragShaderHandle, combineFrag, false);

                ShaderGenerator.ClearTarget();

                _programHandle = GL.CreateProgram();

                GL.AttachShader(_programHandle, _vertexShaderHandle);
                GL.AttachShader(_programHandle, _fragShaderHandle);

                GL.LinkProgram(_programHandle);

#if DEBUG
                GL.GetProgram(_programHandle, ProgramParameter.LinkStatus, out int status);
                if (status == 0)
                {
                    string log = GL.GetProgramInfoLog(_programHandle);
                    Console.WriteLine(log);
                }
#else
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
#endif
            }

            GL.UseProgram(_programHandle);
            ShaderGenerator.SetUniforms(this);
        }

        private void GenShader(ref int shaderHandle, string source, bool vertexShader)
        {
            shaderHandle = GL.CreateShader(vertexShader ? ShaderType.VertexShader : ShaderType.FragmentShader);
            ShaderGenerator.TryCompile(_programHandle, shaderHandle, source);
        }

        private void DeleteShader(ref int handle)
        {
            if (handle <= 0)
            {
                return;
            }

            GL.DetachShader(_programHandle, handle);
            GL.DeleteShader(handle);

            handle = 0;
        }

        internal override void Bind()
        {
        }

        internal override void Unbind()
        {
            DeleteShader(ref _vertexShaderHandle);
            DeleteShader(ref _fragShaderHandle);

            if (_programHandle > 0)
            {
                GL.DeleteProgram(_programHandle);
            }

            _programHandle = 0;

            foreach (MDL0MaterialRefNode m in Children)
            {
                m.Unbind();
            }
        }

        //public override void Dispose()
        //{
        //    DeleteShader(ref _vertexShaderHandle);
        //    DeleteShader(ref _fragShaderHandle);

        //    if (_programHandle > 0)
        //        GL.DeleteProgram(_programHandle);
        //    _programHandle = 0;

        //    base.Dispose();
        //}

        #endregion

        public TextureFrameState[] _indirectFrameStates = new TextureFrameState[3];

        internal void ApplySRT0(SRT0Node node, float index)
        {
            SRT0EntryNode e;

            if (node == null || index < 1)
            {
                foreach (MDL0MaterialRefNode r in Children)
                {
                    r.ApplySRT0Texture(null);
                }
            }
            else if ((e = node.FindChild(Name, false) as SRT0EntryNode) != null)
            {
                foreach (SRT0TextureNode t in e.Children)
                {
                    if (!t.Indirect)
                    {
                        if (t._textureIndex < Children.Count)
                        {
                            ((MDL0MaterialRefNode) Children[t._textureIndex]).ApplySRT0Texture(t, index,
                                node.MatrixMode);
                        }
                    }
                    else if (t._textureIndex < _indirectFrameStates.Length)
                    {
                        fixed (TextureFrameState* state = &_indirectFrameStates[t._textureIndex])
                        {
                            if (node != null && index >= 1)
                            {
                                float* f = (float*) state;
                                for (int i = 0; i < 5; i++)
                                {
                                    if (t.Keyframes[i]._keyCount > 0)
                                    {
                                        f[i] = t.GetFrameValue(i, index - 1);
                                    }
                                }

                                state->MatrixMode = node.MatrixMode;
                                state->CalcTransforms();
                            }
                            else
                            {
                                *state = TextureFrameState.Neutral;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (MDL0MaterialRefNode r in Children)
                {
                    r.ApplySRT0Texture(null);
                }
            }
        }

        //Use these for streaming values into the shader
        public Vector4 amb1, amb2, clr1, clr2, k1, k2, k3, k4, c1, c2, c3;

        internal void ApplyCLR0(CLR0Node node, float index)
        {
            if (node == null || index < 1)
            {
                clr1 = C1MaterialColor;
                clr2 = C2MaterialColor;
                amb1 = C1AmbientColor;
                amb2 = C2AmbientColor;
                c1 = Color0;
                c2 = Color1;
                c3 = Color2;
                k1 = ConstantColor0;
                k2 = ConstantColor1;
                k3 = ConstantColor2;
                k4 = ConstantColor3;
                return;
            }

            CLR0MaterialNode mat = node.FindChild(Name, false) as CLR0MaterialNode;
            if (mat != null)
            {
                foreach (CLR0MaterialEntryNode e in mat.Children)
                {
                    RGBAPixel color = e.Constant
                        ? e.SolidColor
                        : e.Colors[((int) Math.Truncate(index - 1)).Clamp(0, e.Colors.Count - 1)];
                    RGBAPixel mask = e.ColorMask;
                    switch (e.Target)
                    {
                        case EntryTarget.LightChannel0AmbientColor:
                            amb1 = (C1AmbientColor & mask) | color;
                            break;
                        case EntryTarget.LightChannel1AmbientColor:
                            amb2 = (C2AmbientColor & mask) | color;
                            break;
                        case EntryTarget.LightChannel0MaterialColor:
                            clr1 = (C1MaterialColor & mask) | color;
                            break;
                        case EntryTarget.LightChannel1MaterialColor:
                            clr2 = (C2MaterialColor & mask) | color;
                            break;
                        case EntryTarget.ColorRegister0:
                            c1 = ((RGBAPixel) Color0 & mask) | color;
                            break;
                        case EntryTarget.ColorRegister1:
                            c2 = ((RGBAPixel) Color1 & mask) | color;
                            break;
                        case EntryTarget.ColorRegister2:
                            c3 = ((RGBAPixel) Color2 & mask) | color;
                            break;
                        case EntryTarget.ConstantColorRegister0:
                            k1 = ((RGBAPixel) ConstantColor0 & mask) | color;
                            break;
                        case EntryTarget.ConstantColorRegister1:
                            k2 = ((RGBAPixel) ConstantColor1 & mask) | color;
                            break;
                        case EntryTarget.ConstantColorRegister2:
                            k3 = ((RGBAPixel) ConstantColor2 & mask) | color;
                            break;
                        case EntryTarget.ConstantColorRegister3:
                            k4 = ((RGBAPixel) ConstantColor3 & mask) | color;
                            break;
                    }
                }
            }
        }

        internal void ApplyPAT0(PAT0Node node, float index)
        {
            PAT0EntryNode e;

            if (node == null || index < 1)
            {
                foreach (MDL0MaterialRefNode r in Children)
                {
                    r.ApplyPAT0Texture(null, 0);
                }
            }
            else if ((e = node.FindChild(Name, false) as PAT0EntryNode) != null)
            {
                foreach (PAT0TextureNode t in e.Children)
                {
                    if (t._textureIndex < Children.Count)
                    {
                        ((MDL0MaterialRefNode) Children[t._textureIndex]).ApplyPAT0Texture(t, index);
                    }
                }
            }
            else
            {
                foreach (MDL0MaterialRefNode r in Children)
                {
                    r.ApplyPAT0Texture(null, 0);
                }
            }
        }

        public bool _scn0Applied;
        public Vector4 _ambientLight;
        public FogAnimationFrame _fog;
        public GLSLLightFrame[] _lights = new GLSLLightFrame[8];

        internal void ApplySCN(SCN0Node node, float index)
        {
            if (node == null || index <= 0)
            {
                _scn0Applied = false;

                //Substitute fixed pipeline lighting later
                _ambientLight = new Vector4();
                _fog = new FogAnimationFrame();
                _lights = new GLSLLightFrame[8];
            }
            else
            {
                index--;

                _scn0Applied = true;

                SCN0GroupNode lightSetGroup = node.LightSetGroup;
                SCN0GroupNode lightGroup = node.LightGroup;
                SCN0GroupNode fogGroup = node.FogGroup;
                SCN0GroupNode cameraGroup = node.CameraGroup;

                if (lightSetGroup != null)
                {
                    SCN0LightSetNode lightSet = LightSetIndex < lightSetGroup.Children.Count && LightSetIndex >= 0
                        ? lightSetGroup.Children[LightSetIndex] as SCN0LightSetNode
                        : null;
                    if (lightSet != null)
                    {
                        SCN0AmbientLightNode ambLight = lightSet._ambient;
                        if (ambLight != null)
                        {
                            _ambientLight = ambLight.GetAmbientColorFrame(index);
                        }

                        for (int i = 0; i < lightSet._lights.Length; i++)
                        {
                            SCN0LightNode light = lightSet._lights[i];
                            if (light != null)
                            {
                                _lights[i] = light.GetGLSLAnimFrame(index);
                            }
                        }
                    }
                }

                if (fogGroup != null)
                {
                    SCN0FogNode fog = FogIndex < fogGroup.Children.Count && FogIndex >= 0
                        ? fogGroup.Children[FogIndex] as SCN0FogNode
                        : null;
                    if (fog != null)
                    {
                        _fog = fog.GetAnimFrame(index);
                    }
                }
            }
        }

        public void ApplyViewportLighting(ModelPanelViewport viewport)
        {
            //TODO: support 8 built-in lights instead of just one, and fog?
            //(like an SCN0 built into the program)
            if (!_scn0Applied)
            {
                bool pointLight = viewport._posLight._w != 0.0f;
                _lights[0] = new GLSLLightFrame(
                    viewport._lightEnabled,
                    pointLight ? LightType.Point : LightType.Directional,
                    new Vector3(viewport._posLight._x, viewport._posLight._y, viewport._posLight._z),
                    new Vector3(),
                    viewport.Diffuse,
                    pointLight
                        ? SCN0LightNode.GetLightDistCoefs(viewport.LightPosition._x, 0.5f, DistAttnFn.Medium)
                        : new Vector3(1.0f, 0.0f, 0.0f),
                    new Vector3(1.0f, 0.0f, 0.0f),
                    true,
                    viewport.Specular,
                    SCN0LightNode.GetSpecShineDistCoefs(64.0f));
                _ambientLight = viewport.Ambient;
            }
        }

        #endregion

        #region Special Materials

        public void GenerateShadowMaterial()
        {
            //HardcodedFiles.CreateShadowMaterial();
            //ReplaceRaw(FileMap.FromFile(ShadowMaterial.Name));
            Name = "MShadow1";

            EnableBlend = true;
            LightChannel0.MaterialColor = new Vector4(255 * 255, 255 * 255, 255 * 255, 255 * 255);
            LightChannel0.Color.Enabled = false;
            LightChannel0.Color.MaterialSource = (GXColorSrc) 1;
            LightChannel0.Alpha.Enabled = false;
            LightChannel0.Alpha.MaterialSource = (GXColorSrc) 1;
            XLUMaterial = true;
            LightSetIndex = -1;
            _tevColorBlock.TevReg1Lo.AG = 70;
            CompareBeforeTexture = true;
            EnableDepthUpdate = false;

            Children?.Clear();

            addShadowReference();
        }

        private void addShadowReference()
        {
            MDL0MaterialRefNode mr = new MDL0MaterialRefNode();
            AddChild(mr);
            mr.Name = "TShadow1";
            mr.SCN0RefCamera = 7;
            mr.MapMode = MappingMethod.Projection;
            mr.UWrapMode = MatWrapMode.Clamp;
            mr.VWrapMode = MatWrapMode.Clamp;
            mr.Projection = TexProjection.STQ;
            mr.InputForm = TexInputForm.ABC1;
            mr.EmbossSource = 5;


            UserDataClass shadowData = new UserDataClass
            {
                _name = "shadow",
                DataType = UserValueType.Int,
                Entries = new string[1]
            };
            shadowData.Entries[0] = Children.Count.ToString();
            UserEntries.Add(shadowData);
        }

        public void GenerateSpyMaterial()
        {
            //HardcodedFiles.CreateSpyMaterial();
            //ReplaceRaw(FileMap.FromFile(SpyMaterial.Name));
            Name = "Spycloak";

            LightSetIndex = -1;
            FogIndex = -1;
            _tevColorBlock.TevReg1Lo.RB = 255;
            _tevColorBlock.TevReg1Hi0.RB = 255;
            _tevColorBlock.TevReg1Hi0.AG = 255;
            IndirectShaderStages = 1;

            LightChannel1._matColor.Red = 255;
            LightChannel1._matColor.Blue = 255;
            LightChannel1._matColor.Green = 255;

            LightChannel0.Color.Enabled = false;
            LightChannel0.Alpha.Enabled = false;

            CompareBeforeTexture = true;

            Children?.Clear();

            addSpyReferences();
        }

        private void addSpyReferences()
        {
            MDL0MaterialRefNode fbref = new MDL0MaterialRefNode();
            MDL0MaterialRefNode spycloakref = new MDL0MaterialRefNode();
            AddChild(fbref);
            AddChild(spycloakref);
            fbref.Name = "FB";
            spycloakref.Name = "spycloak00";
            fbref.HasTextureMatrix = spycloakref.HasTextureMatrix = true;
            fbref.Projection = spycloakref.Projection = TexProjection.STQ;
            fbref.UWrapMode = spycloakref.UWrapMode = fbref.VWrapMode = spycloakref.VWrapMode = MatWrapMode.Clamp;
            fbref.SCN0RefCamera = 0;
            fbref.MapMode = MappingMethod.Projection;
            spycloakref.Scale = new Vector2(1, (float) 0.125);
            spycloakref.MapMode = MappingMethod.EnvCamera;
            spycloakref.MinFilter = MatTextureMinFilter.Linear_Mipmap_Linear;
            fbref.LODBias = -1;
            spycloakref.LODBias = -4;
            fbref.InputForm = spycloakref.InputForm = TexInputForm.ABC1;
            fbref.Coordinates = TexSourceRow.Geometry;
            spycloakref.Coordinates = TexSourceRow.Normals;
            spycloakref.Normalize = true;
        }

        #endregion

        public override void Replace(string fileName)
        {
            base.Replace(fileName);

            Model?.CheckTextures();
        }

        public override void Remove()
        {
            Remove(ShaderNode != null && ShaderNode.Materials.Length == 1 &&
                   MessageBox.Show("Do you want to remove this material's shader?", "", MessageBoxButtons.YesNo) ==
                   DialogResult.Yes);
        }

        public void Remove(bool removeAttached)
        {
            if (Parent != null)
            {
                if (removeAttached)
                {
                    ShaderNode?.Remove();
                }

                ShaderNode = null;

                foreach (MDL0MaterialRefNode r in Children)
                {
                    r.TextureNode = null;
                    r.PaletteNode = null;
                }

                Model.CheckTextures();
            }

            base.Remove();
        }

        //public override void RemoveChild(ResourceNode child)
        //{
        //    base.RemoveChild(child);

        //    if (!_updating && Model._autoMetal && MetalMaterial != null && !this.isMetal)
        //        MetalMaterial.UpdateAsMetal();
        //}
        public int ImageCount => Children?.Where(o => o is IImageSource i && i.ImageCount > 0).Count() ?? 0;

        public Bitmap GetImage(int index)
        {
            return ((IImageSource) Children?.Where(o => o is IImageSource i && i.ImageCount > 0).ToArray()[index])
                .GetImage(0);
        }
    }

    #region Light Channel Info

    public class LightChannel
    {
        public MDL0MaterialNode _parent;
        public LightingChannelFlags _flags;
        public RGBAPixel _matColor, _ambColor;
        public LightChannelControl _color, _alpha;

        public LightChannel(uint flags, RGBAPixel mat, RGBAPixel amb, uint color, uint alpha, MDL0MaterialNode material)
            : this((LightingChannelFlags) flags, mat, amb, color, alpha, material)
        {
        }

        public LightChannel(LightingChannelFlags flags, RGBAPixel mat, RGBAPixel amb, uint color, uint alpha,
                            MDL0MaterialNode material)
            : this(flags, mat, amb, new LightChannelControl(color, null), new LightChannelControl(alpha, null),
                material)
        {
        }

        public LightChannel(uint flags, RGBAPixel mat, RGBAPixel amb, LightChannelControl color,
                            LightChannelControl alpha, MDL0MaterialNode material)
            : this((LightingChannelFlags) flags, mat, amb, color, alpha, material)
        {
        }

        public LightChannel(
            LightingChannelFlags flags,
            RGBAPixel mat,
            RGBAPixel amb,
            LightChannelControl color,
            LightChannelControl alpha,
            MDL0MaterialNode material)
        {
            _parent = material;
            _flags = flags;
            _matColor = mat;
            _ambColor = amb;
            _color = color;
            _color._parent = this;
            _alpha = alpha;
            _alpha._parent = this;
        }

        [Category("Lighting Channel")]
        [Description(
            "Determines if this channel should pass a color value to the LightChannel property in shader stages.")]
        public bool RasterColorEnabled
        {
            get => _flags.HasFlag(LightingChannelFlags.UseChanColor);
            set
            {
                if (value)
                {
                    _flags |= LightingChannelFlags.UseChanColor;
                }
                else
                {
                    _flags &= ~LightingChannelFlags.UseChanColor;
                }

                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Description(
            "Determines if this channel should pass a color value to the LightChannel property in shader stages.")]
        public bool RasterAlphaEnabled
        {
            get => _flags.HasFlag(LightingChannelFlags.UseChanAlpha);
            set
            {
                if (value)
                {
                    _flags |= LightingChannelFlags.UseChanAlpha;
                }
                else
                {
                    _flags &= ~LightingChannelFlags.UseChanAlpha;
                }

                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Description(
            "Determines if this channel should multiply the ambient color by the attached SCN0 lightset's ambient color.")]
        public bool AmbientColorEnabled
        {
            get => _flags.HasFlag(LightingChannelFlags.UseAmbColor);
            set
            {
                if (value)
                {
                    _flags |= LightingChannelFlags.UseAmbColor;
                }
                else
                {
                    _flags &= ~LightingChannelFlags.UseAmbColor;
                }

                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Description(
            "Determines if this channel should multiply the ambient alpha by the attached SCN0 lightset's ambient alpha.")]
        public bool AmbientAlphaEnabled
        {
            get => _flags.HasFlag(LightingChannelFlags.UseAmbAlpha);
            set
            {
                if (value)
                {
                    _flags |= LightingChannelFlags.UseAmbAlpha;
                }
                else
                {
                    _flags &= ~LightingChannelFlags.UseAmbAlpha;
                }

                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Description(
            "Determines if this channel should multiply the material color by the attached SCN0 lightset's final lighting color.")]
        public bool MaterialColorEnabled
        {
            get => _flags.HasFlag(LightingChannelFlags.UseMatColor);
            set
            {
                if (value)
                {
                    _flags |= LightingChannelFlags.UseMatColor;
                }
                else
                {
                    _flags &= ~LightingChannelFlags.UseMatColor;
                }

                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Description(
            "Determines if this channel should multiply the material alpha by the attached SCN0 lightset's final lighting alpha.")]
        public bool MaterialAlphaEnabled
        {
            get => _flags.HasFlag(LightingChannelFlags.UseMatAlpha);
            set
            {
                if (value)
                {
                    _flags |= LightingChannelFlags.UseMatAlpha;
                }
                else
                {
                    _flags &= ~LightingChannelFlags.UseMatAlpha;
                }

                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Browsable(false)]
        public LightingChannelFlags Flags
        {
            get => _flags;
            set
            {
                _flags = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [TypeConverter(typeof(RGBAStringConverter))]
        public RGBAPixel MaterialColor
        {
            get => _matColor;
            set
            {
                _matColor = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [TypeConverter(typeof(RGBAStringConverter))]
        public RGBAPixel AmbientColor
        {
            get => _ambColor;
            set
            {
                _ambColor = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public LightChannelControl Color
        {
            get => _color;
            set
            {
                _color = value;
                _color._parent = this;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public LightChannelControl Alpha
        {
            get => _alpha;
            set
            {
                _alpha = value;
                _alpha._parent = this;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Browsable(false)]
        public GXColorSrc ColorMaterialSource
        {
            get => _color.MaterialSource;
            set
            {
                _color.MaterialSource = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Browsable(false)]
        public bool ColorEnabled
        {
            get => _color.Enabled;
            set
            {
                _color.Enabled = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Browsable(false)]
        public GXColorSrc ColorAmbientSource
        {
            get => _color.AmbientSource;
            set
            {
                _color.AmbientSource = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Browsable(false)]
        public GXDiffuseFn ColorDiffuseFunction
        {
            get => _color.DiffuseFunction;
            set
            {
                _color.DiffuseFunction = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Browsable(false)]
        public GXAttnFn ColorAttenuation
        {
            get => _color.Attenuation;
            set
            {
                _color.Attenuation = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Browsable(false)]
        public MatChanLights ColorLights
        {
            get => _color.Lights;
            set
            {
                _color.Lights = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Browsable(false)]
        public GXColorSrc AlphaMaterialSource
        {
            get => _alpha.MaterialSource;
            set
            {
                _alpha.MaterialSource = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Browsable(false)]
        public bool AlphaEnabled
        {
            get => _alpha.Enabled;
            set
            {
                _alpha.Enabled = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Browsable(false)]
        public GXColorSrc AlphaAmbientSource
        {
            get => _alpha.AmbientSource;
            set
            {
                _alpha.AmbientSource = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Browsable(false)]
        public GXDiffuseFn AlphaDiffuseFunction
        {
            get => _alpha.DiffuseFunction;
            set
            {
                _alpha.DiffuseFunction = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Browsable(false)]
        public GXAttnFn AlphaAttenuation
        {
            get => _alpha.Attenuation;
            set
            {
                _alpha.Attenuation = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Channel")]
        [Browsable(false)]
        public MatChanLights AlphaLights
        {
            get => _alpha.Lights;
            set
            {
                _alpha.Lights = value;
                _parent.SignalPropertyChange();
            }
        }
    }

    public enum GXColorSrc
    {
        Register,
        Vertex
    }

    [Flags]
    public enum MatChanLights
    {
        None = 0x0,
        Light0 = 0x1,
        Light1 = 0x2,
        Light2 = 0x4,
        Light3 = 0x8,
        Light4 = 0x10,
        Light5 = 0x20,
        Light6 = 0x40,
        Light7 = 0x80
    }

    public enum GXDiffuseFn
    {
        Disabled,
        Enabled,
        Clamped
    }

    public enum GXAttnFn
    {
        Specular,
        Spotlight,
        None
    }

    [Flags]
    public enum LightingChannelFlags
    {
        UseMatColor = 0x1,
        UseMatAlpha = 0x2,
        UseAmbColor = 0x4,
        UseAmbAlpha = 0x8,
        UseChanColor = 0x10,
        UseChanAlpha = 0x20
    }

    public class LightChannelControl
    {
        internal LightChannel _parent;

        public LightChannelControl(uint ctrl)
        {
            _parent = null;
            _binary = new Bin32(ctrl);
        }

        public LightChannelControl(uint ctrl, LightChannel parent)
        {
            _parent = parent;
            _binary = new Bin32(ctrl);
        }

        public LightChannelControl(
            bool enabled,
            GXColorSrc matSource,
            GXColorSrc ambSource,
            GXDiffuseFn diffuseFunc,
            GXAttnFn atten)
        {
            _binary = 0;
            Enabled = enabled;
            MaterialSource = matSource;
            AmbientSource = ambSource;
            DiffuseFunction = diffuseFunc;
            Attenuation = atten;
        }

        public Bin32 _binary;

        //0000 0000 0000 0000 0000 0000 0000 0001   Material Source (GXColorSrc)
        //0000 0000 0000 0000 0000 0000 0000 0010   Light Enabled
        //0000 0000 0000 0000 0000 0000 0011 1100   Light 0123
        //0000 0000 0000 0000 0000 0000 0100 0000   Ambient Source (GXColorSrc)
        //0000 0000 0000 0000 0000 0001 1000 0000   Diffuse Func
        //0000 0000 0000 0000 0000 0010 0000 0000   Attenuation Enable
        //0000 0000 0000 0000 0000 0100 0000 0000   Attenuation Function (0 = Specular)
        //0000 0000 0000 0000 0111 1000 0000 0000   Light 4567

        [Category("Lighting Control")]
        public bool Enabled
        {
            get => _binary[1];
            set
            {
                _binary[1] = value;
                _parent?._parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Control")]
        public GXColorSrc MaterialSource
        {
            get => (GXColorSrc) (_binary[0] ? 1 : 0);
            set
            {
                _binary[0] = value != 0;
                _parent?._parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Control")]
        public GXColorSrc AmbientSource
        {
            get => (GXColorSrc) (_binary[6] ? 1 : 0);
            set
            {
                _binary[6] = value != 0;
                _parent?._parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Control")]
        public GXDiffuseFn DiffuseFunction
        {
            get => (GXDiffuseFn) _binary[7, 2];
            set
            {
                _binary[7, 2] = (uint) value;
                _parent?._parent.SignalPropertyChange();
            }
        }

        [Category("Lighting Control")]
        public GXAttnFn Attenuation
        {
            get
            {
                if (!_binary[9])
                {
                    return GXAttnFn.None;
                }

                return (GXAttnFn) (_binary[10] ? 1 : 0);
            }
            set
            {
                if (value != GXAttnFn.None)
                {
                    _binary[9] = true;
                    _binary[10] = value != 0;
                }
                else
                {
                    _binary[9] = false;
                    _binary[10] = false;
                }

                _parent?._parent.SignalPropertyChange();
            }
        }

        //These bits are dynamically set at runtime by the SCN0
        [Category("Lighting Control")]
        [Browsable(false)]
        public MatChanLights Lights
        {
            get => (MatChanLights) (_binary[11, 4] | (_binary[2, 4] << 4));
            set
            {
                uint val = (uint) value;
                _binary[11, 4] = val & 0xF;
                _binary[2, 4] = (val >> 4) & 0xF;

                _parent?._parent.SignalPropertyChange();
            }
        }
    }

    #endregion
}