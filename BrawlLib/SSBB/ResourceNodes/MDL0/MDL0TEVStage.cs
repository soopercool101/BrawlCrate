using BrawlLib.Wii.Graphics;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public partial class MDL0TEVStageNode : MDL0EntryNode
    {
        public override ResourceType ResourceFileType => ResourceType.TEVStage;

        public override string Name
        {
            get => $"Stage{Index}";
            set => base.Name = value;
        }

        public MDL0TEVStageNode()
        {
            Default(false);
        }

        public MDL0TEVStageNode(ColorEnv colEnv, AlphaEnv alphaEnv, CMD cmd, TevKColorSel kc, TevKAlphaSel ka,
                                TexMapID id, TexCoordID coord, ColorSelChan col, bool useTex)
        {
            _colorEnv = colEnv;
            _alphaEnv = alphaEnv;
            _cmd = cmd;
            _kcSel = kc;
            _kaSel = ka;
            _texMapID = id;
            _texCoord = coord;
            _colorChan = col;
            _texEnabled = useTex;
        }

        public ColorEnv _colorEnv;
        public AlphaEnv _alphaEnv;
        public CMD _cmd;
        public TevKColorSel _kcSel;
        public TevKAlphaSel _kaSel;
        public TexMapID _texMapID;
        public TexCoordID _texCoord;
        public ColorSelChan _colorChan;
        public bool _texEnabled;

        //Instead of letting the user copy raw values, there needs to be a way to copy and paste shaders.

        //[Category("f Raw Values")]
        //public string ColorEnv
        //{
        //    get { return "0x" + ((uint)_colorEnv).ToString("X"); }
        //    set
        //    {
        //        if (value.StartsWith("0x"))
        //            value = value.Substring(2, 8);
        //        _colorEnv = uint.Parse(value, System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture);

        //        UpdateProperties();
        //    }
        //}
        //[Category("f Raw Values")]
        //public string AlphaEnv
        //{
        //    get { return "0x" + ((uint)_alphaEnv).ToString("X"); }
        //    set
        //    {
        //        if (value.StartsWith("0x"))
        //            value = value.Substring(2, 8);
        //        _colorEnv = uint.Parse(value, System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture);

        //        UpdateProperties();
        //    }
        //}
        //[Category("f Raw Values")]
        //public string CMD
        //{
        //    get { return "0x" + ((uint)_cmd).ToString("X"); }
        //    set
        //    {
        //        if (value.StartsWith("0x"))
        //            value = value.Substring(2, 8);
        //        _colorEnv = uint.Parse(value, System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture);

        //        UpdateProperties();
        //    }
        //}

        private const string eqClr =
            @"Shader stages run this equation to blend colors and create the final color of each pixel fragment on a mesh.
Think of it like texture pixels: each pixel has an R, G, B and A channel (red, green, blue and alpha transparency, respectively).

Each shader stage processes color as 3 individual channels: RGB.
This equation is applied to each channel (R, G and B) individually.

Use this equation to predict the output for each channel (resulting in the final color for this stage).
'a' uses input selected by ColorSelectionA.
'b' uses input selected by ColorSelectionB.
'c' uses input selected by ColorSelectionC.
'd' uses input selected by ColorSelectionD.
If clamped, output is restrained to the range [0.0, 1.0].

Note that input is not limited to color values; you can use alpha for all three channels as well.

If you're having a hard time visualizing the output of this equation, think in terms of 0 and 1, where 0 is black and 1 is white.
If the input is 0, no color multiplied by it can change the color from black. If input is 1, any color value multiplied by it will pass through.";

        private const string eqAlpha =
            @"Shader stages run this equation to blend alpha values and create the final transparency value of each pixel fragment on a mesh.
Think of it like texture pixels: each pixel has an R, G, B and A channel (red, green, blue and alpha transparency, respectively).

Each shader stage processes alpha separately as one channel, 
as you will want to blend transparency differently than color.

Use this equation to predict the output for each channel (resulting in the final color for this stage).
'a' uses input selected by AlphaSelectionA.
'b' uses input selected by AlphaSelectionB.
'c' uses input selected by AlphaSelectionC.
'd' uses input selected by AlphaSelectionD.
If clamped, output is restrained to the range [0.0, 1.0].

Note that input is not limited to alpha; you can use Red, Green or Blue as well (separately).

If you're having a hard time visualizing the output of this equation, think in terms of 0 and 1, where 0 is invisible and 1 is fully visible.
If the input is 0, nothing multiplied by it can affect transparency. If input is 1, any value multiplied by it will affect transparency unless that value is also 1.";

        //[Category("c TEV Color Env"), Description(eqClr)]
        //public string ColorOutput { get { return (ColorClamp ? "clamp(" : "") + "(d " + (ColorSubtract ? "-" : "+") + " ((1 - c) * a + c * b)" + ((int)ColorBias == 1 ? " + 0.5" : (int)ColorBias == 2 ? " - 0.5" : "") + ") * " + ((int)ColorScale == 3 ? "0.5" : (int)ColorScale == 0 ? "1" : ((int)ColorScale * 2).ToString()) + (ColorClamp ? ");" : ";"); } }
        //[Category("d TEV Alpha Env"), Description(eqAlpha)]
        //public string AlphaOutput { get { return (AlphaClamp ? "clamp(" : "") + "(d " + (AlphaSubtract ? "-" : "+") + " ((1 - c) * a + c * b)" + ((int)AlphaBias == 1 ? " + 0.5" : (int)AlphaBias == 2 ? " - 0.5" : "") + ") * " + ((int)AlphaScale == 3 ? "0.5" : (int)AlphaScale == 0 ? "1" : ((int)AlphaScale * 2).ToString()) + (AlphaClamp ? ");" : ";"); } }

        [Category("b TEV Color Output")]
        [DisplayName("Output Function")]
        [Description(eqClr)]
        public string ColorOutput
        {
            get
            {
                string s = ColorRegister + (ColorClamp ? " = clamp(" : " = ");

                int op = (int) ColorOperation;
                if (op < 2)
                {
                    s += string.Format("{1}d {0} ((1 - c) * a + c * b){2}{3}",
                        op == 1 ? "-" : "+",
                        ColorScale != 0 ? "(" : "",
                        (int) ColorBias == 1 ? " + 0.5)" : (int) ColorBias == 2 ? " - 0.5)" : "",
                        (int) ColorScale == 3 ? ") / 2" :
                        ColorScale != 0 ? ") * " + (int) ColorScale * 2 : "");
                }
                else if (op > 13)
                {
                    s += $"d[x] + ((a[x] {(op % 2 == 0 ? ">" : "==")} b[x]) ? c[x] : 0)";
                }
                else
                {
                    s += string.Format("d + ((a[{1}] {0} b[{1}]) ? c : 0)", op % 2 == 0 ? ">" : "==",
                        op < 10 ? "R" : op < 12 ? "GR" : "BGR");
                }

                return s + (ColorClamp ? ");" : ";");
            }
        }

        [Category("c TEV Alpha Output")]
        [DisplayName("Output Function")]
        [Description(eqAlpha)]
        public string AlphaOutput
        {
            get
            {
                string s = AlphaRegister + (AlphaClamp ? " = clamp(" : " = ");

                int op = (int) AlphaOperation;
                if (op < 2)
                {
                    s += string.Format("{1}d {0} ((1 - c) * a + c * b){2}{3}",
                        op == 1 ? "-" : "+",
                        AlphaScale != 0 ? "(" : "",
                        (int) AlphaBias == 1 ? " + 0.5)" : (int) AlphaBias == 2 ? " - 0.5)" : "",
                        (int) AlphaScale == 3 ? ") / 2" :
                        AlphaScale != 0 ? ") * " + (int) AlphaScale * 2 : "");
                }
                else if (op > 13)
                {
                    s += $"d + ((a {(op % 2 == 0 ? ">" : "==")} b) ? c : 0)";
                }
                else
                {
                    s += string.Format("d + ((a[{1}] {0} b[{1}]) ? c : 0)", op % 2 == 0 ? ">" : "==",
                        op < 10 ? "R" : op < 12 ? "GR" : "BGR");
                }

                return s + (AlphaClamp ? ");" : ";");
            }
        }

        private const string konst =
            @"Constant1_1 equals 1/1 (1.000).
Constant7_8 equals 7/8 (0.875).
Constant3_4 equals 3/4 (0.750).
Constant5_8 equals 5/8 (0.625).
Constant1_2 equals 1/2 (0.500).
Constant3_8 equals 3/8 (0.375).
Constant1_4 equals 1/4 (0.250).
Constant1_8 equals 1/8 (0.125).";

        private const string kColor =
            @"This option provides a value to the 'KonstantColorSelection' option in each ColorSelection(A,B,C,D).
You can choose a preset constant or pull a constant value from the material's 'TEV Konstant Block'.
Each color fragment has 4 values: R, G, B, and A. Each shader stage processes color as 3 individual channels: RGB (alpha is separate).

The constants set all 3 channels with the same constant value:
" + konst + @"

The registers ('registers' are just RGBA colors) in the material's 'TEV Konstant Block' can be pulled as well.
You can also fill all channels with one value here, for example, if the value says Red, then RGB = RRR.";

        private const string kAlpha =
            @"This option provides a value to the 'KonstantColorSelection' option in each ColorSelection(A,B,C,D).
You can choose a preset constant or pull a constant value from the material's 'TEV Konstant Block'.
Each color fragment has 4 values: R, G, B, and A. Each shader stage processes color as 3 individual channels: RGB (alpha is separate).

The constants set all 3 channels with the same constant value:
" + konst + @"

You can also use a different channel as alpha here, for example, for example, if the value says Red, then the alpha value is set to the value stored in the red channel.";

        [Category("b TEV Color Output")]
        [DisplayName("Constant Selection")]
        [Description(kColor)]
        public TevKColorSel ConstantColorSelection
        {
            get => _kcSel;
            set
            {
                _kcSel = value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Alpha Output")]
        [DisplayName("Constant Selection")]
        [Description(kAlpha)]
        public TevKAlphaSel ConstantAlphaSelection
        {
            get => _kaSel;
            set
            {
                _kaSel = value;
                SignalPropertyChange();
            }
        }

        [Category("a TEV Fragment Sources")]
        [DisplayName("Texture Enabled")]
        [Description(
            @"Determines if a texture can be used as color input. 
This stage will grab a pixel fragment from the selected texture reference mapped on the model with the selected coordinates.")]
        public bool TextureEnabled
        {
            get => _texEnabled;
            set
            {
                _texEnabled = value;
                SignalPropertyChange();
            }
        }

        [Category("a TEV Fragment Sources")]
        [DisplayName("Texture Map ID")]
        [Description("This is the index of the texture reference in the material to use as texture input.")]
        public TexMapID TextureMapID
        {
            get => _texMapID;
            set
            {
                _texMapID = value;
                SignalPropertyChange();
            }
        }

        [Category("a TEV Fragment Sources")]
        [DisplayName("Texture Coordinates ID")]
        [Description("This is the index of the texture coordinate to map the texture on the model.")]
        public TexCoordID TextureCoordID
        {
            get => _texCoord;
            set
            {
                _texCoord = value;
                SignalPropertyChange();
            }
        }

        [Category("a TEV Fragment Sources")]
        [DisplayName("Texture Swap Selection")]
        public TevSwapSel TextureSwap
        {
            get => _alphaEnv.TSwap;
            set
            {
                _alphaEnv.TSwap = value;
                SignalPropertyChange();
            }
        }

        [Category("a TEV Fragment Sources")]
        [DisplayName("Raster Color")]
        [Description(
            @"Retrieves a color outputted from the material's light channels. This DOES NOT get a color straight from color nodes!
BumpAlpha retrieves a color from ???
NormalizedBumpAlpha is the same as Bump Alpha, but normalized to the range [0.0, 1.0].
Zero returns a color with all channels set to 0.")]
        public ColorSelChan RasterColor
        {
            get => _colorChan;
            set
            {
                _colorChan = value;
                SignalPropertyChange();
            }
        }

        [Category("a TEV Fragment Sources")]
        [DisplayName("Raster Swap Selection")]
        public TevSwapSel RasterSwap
        {
            get => _alphaEnv.RSwap;
            set
            {
                _alphaEnv.RSwap = value;
                SignalPropertyChange();
            }
        }

        private const string _colorArgDesc =
            @"

PreviousColor: returns the RGB of the final output color. This color starts off as 0, 0, 0, 0 (transparent black). This is usually the output of the last TEV stage, unless its ColorRegister property isn't set to 'PreviousRegister'.
PreviousAlpha: returns the alpha value of the final output color for all channels of RGB (RGB = AAA). This color starts off as 0, 0, 0, 0 (transparent black). This is usually the output of the last TEV stage, unless its ColorRegister property isn't set to 'PreviousRegister'.
Color0: returns RGB color of the 1st color register in the attached material's TEV Color Block.
Alpha0: returns the alpha (RGB = AAA) of the 1st color register in the attached material's TEV Color Block.
Color1: returns RGB color of the 2nd color register in the attached material's TEV Color Block.
Alpha1: returns the alpha (RGB = AAA) of the 2nd color register in the attached material's TEV Color Block.
Color2: returns RGB color of the 3rd color register in the attached material's TEV Color Block.
Alpha2: returns the alpha (RGB = AAA) of the 3rd color register in the attached material's TEV Color Block.
TextureColor: returns the RGB color sampled from the texture chosen with TextureMapID, using the coordinates chosen by TextureCoord. Can only be used if TextureEnabled is true.
TextureAlpha: returns the alpha value (RGB = AAA) sampled from the texture chosen with TextureMapID, using the coordinates chosen by TextureCoord. Can only be used if TextureEnabled is true.
RasterColor: returns the RGB color from the source selected by the ColorChannel property.
RasterAlpha: returns the alpha value (RGB = AAA) from the source selected by the ColorChannel property.
One: returns RGB = 1, 1, 1 (white). This color multiplied by another will equal the other color.
Half: returns RGB = 0.5, 0.5, 0.5 (gray). This color multiplied by another will darken the other by half.
KonstantColorSelection: returns the RGB color from the source selected by the KonstantColorSelection property.
Zero: returns RGB = 0, 0, 0 (black). This color multiplied by another will return black.

Note that CLR0 animations can stream RGBA colors into the material's Color and Konstant registers! You can use this property to input new color values into this shader every frame of an animation.";

        private const string _alphaArgDesc =
            @"

PreviousAlpha: returns the alpha value of the final output color. This color starts off as 0, 0, 0, 0 (transparent black). This is usually the output of the last TEV stage, unless its ColorRegister property isn't set to 'PreviousRegister'.
Alpha0: returns the alpha value of the 1st color register in the attached material's TEV Color Block.
Alpha1: returns the alpha value of the 2nd color register in the attached material's TEV Color Block.
Alpha2: returns the alpha value of the 3rd color register in the attached material's TEV Color Block.
TextureAlpha: returns the alpha value sampled from the texture chosen with TextureMapID, using the coordinates chosen by TextureCoord. Can only be used if TextureEnabled is true.
RasterAlpha: returns the alpha value from the source selected by the ColorChannel property.
KonstantAlphaSelection: returns the alpha value from the source selected by the KonstantAlphaSelection property.
Zero: returns 0 (transparent). This alpha multiplied by another will return fully transparent.

Note that CLR0 animations can stream RGBA colors into the material's Color and Konstant registers! You can use this property to input new alpha values into this shader every frame of an animation.";

        [Category("b TEV Color Output")]
        [DisplayName("Selection A")]
        [Description("This is color source for 'a' in the ColorOutput formula." + _colorArgDesc)]
        public ColorArg ColorSelectionA
        {
            get => _colorEnv.SelA;
            set
            {
                _colorEnv.SelA = value;
                SignalPropertyChange();
            }
        }

        [Category("b TEV Color Output")]
        [DisplayName("Selection B")]
        [Description("This is color source for 'b' in the ColorOutput formula." + _colorArgDesc)]
        public ColorArg ColorSelectionB
        {
            get => _colorEnv.SelB;
            set
            {
                _colorEnv.SelB = value;
                SignalPropertyChange();
            }
        }

        [Category("b TEV Color Output")]
        [DisplayName("Selection C")]
        [Description("This is color source for 'c' in the ColorOutput formula." + _colorArgDesc)]
        public ColorArg ColorSelectionC
        {
            get => _colorEnv.SelC;
            set
            {
                _colorEnv.SelC = value;
                SignalPropertyChange();
            }
        }

        [Category("b TEV Color Output")]
        [DisplayName("Selection D")]
        [Description("This is color source for 'd' in the ColorOutput formula." + _colorArgDesc)]
        public ColorArg ColorSelectionD
        {
            get => _colorEnv.SelD;
            set
            {
                _colorEnv.SelD = value;
                SignalPropertyChange();
            }
        }

        [Category("b TEV Color Output")]
        [DisplayName("Bias")]
        [Description("Refer to the ColorOutput property to see effect.")]
        public Bias ColorBias
        {
            get => _colorEnv.Bias;
            set
            {
                _colorEnv.Bias = value;
                SignalPropertyChange();
            }
        }

        [Category("b TEV Color Output")]
        [DisplayName("Operation")]
        [Description("Refer to the ColorOutput property to see effect.")]
        public TevColorOp ColorOperation
        {
            get => _colorEnv.Operation;
            set
            {
                _colorEnv.Operation = value;
                SignalPropertyChange();
            }
        }

        [Category("b TEV Color Output")]
        [DisplayName("Clamp")]
        [Description("If true, the final output color channels will be clamped to the range [0, 1].")]
        public bool ColorClamp
        {
            get => _colorEnv.Clamp;
            set
            {
                _colorEnv.Clamp = value;
                SignalPropertyChange();
            }
        }

        [Category("b TEV Color Output")]
        [DisplayName("Scale")]
        [Description(
            "Refer to the ColorOutput property to see effect. This will make the color brighter when multiplying, and darker when dividing.")]
        public TevScale ColorScale
        {
            get => _colorEnv.Shift;
            set
            {
                _colorEnv.Shift = value;
                SignalPropertyChange();
            }
        }

        [Category("b TEV Color Output")]
        [DisplayName("Destination")]
        [Description(
            @"This property dictates where the results of the ColorOutput formula should be stored.

PreviousRegister: sets the final output color of this fragment on the model. This is the color that will be seen in-game after the execution of the last shader stage.
Register0: sets the color of the 1st color register in the material's TEV Color Block. This can be used to store a color temporarily for use in one of the ColorSelection properties in a later shader stage.
Register1: sets the color of the 2nd color register in the material's TEV Color Block. This can be used to store a color temporarily for use in one of the ColorSelection properties in a later shader stage.
Register2: sets the color of the 3rd color register in the material's TEV Color Block. This can be used to store a color temporarily for use in one of the ColorSelection properties in a later shader stage.")]
        public TevColorRegID ColorRegister
        {
            get => _colorEnv.Dest;
            set
            {
                _colorEnv.Dest = value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Alpha Output")]
        [DisplayName("Selection A")]
        [Description("This is alpha source for 'a' in the AlphaOutput formula." + _alphaArgDesc)]
        public AlphaArg AlphaSelectionA
        {
            get => _alphaEnv.SelA;
            set
            {
                _alphaEnv.SelA = value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Alpha Output")]
        [DisplayName("Selection B")]
        [Description("This is alpha source for 'b' in the AlphaOutput formula." + _alphaArgDesc)]
        public AlphaArg AlphaSelectionB
        {
            get => _alphaEnv.SelB;
            set
            {
                _alphaEnv.SelB = value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Alpha Output")]
        [DisplayName("Selection C")]
        [Description("This is alpha source for 'c' in the AlphaOutput formula." + _alphaArgDesc)]
        public AlphaArg AlphaSelectionC
        {
            get => _alphaEnv.SelC;
            set
            {
                _alphaEnv.SelC = value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Alpha Output")]
        [DisplayName("Selection D")]
        [Description("This is alpha source for 'd' in the AlphaOutput formula." + _alphaArgDesc)]
        public AlphaArg AlphaSelectionD
        {
            get => _alphaEnv.SelD;
            set
            {
                _alphaEnv.SelD = value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Alpha Output")]
        [DisplayName("Bias")]
        [Description("Refer to the AlphaOutput property to see effect.")]
        public Bias AlphaBias
        {
            get => _alphaEnv.Bias;
            set
            {
                _alphaEnv.Bias = value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Alpha Output")]
        [DisplayName("Operation")]
        [Description("Refer to the AlphaOutput property to see effect.")]
        public TevAlphaOp AlphaOperation
        {
            get => _alphaEnv.Operation;
            set
            {
                _alphaEnv.Operation = value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Alpha Output")]
        [DisplayName("Clamp")]
        [Description("If true, the final output alpha will be clamped to the range [0, 1].")]
        public bool AlphaClamp
        {
            get => _alphaEnv.Clamp;
            set
            {
                _alphaEnv.Clamp = value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Alpha Output")]
        [DisplayName("Scale")]
        [Description("Refer to the AlphaOutput property to see effect.")]
        public TevScale AlphaScale
        {
            get => _alphaEnv.Shift;
            set
            {
                _alphaEnv.Shift = value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Alpha Output")]
        [DisplayName("Destination")]
        [Description(
            @"This property dictates where the results of the AlphaOutput formula should be stored.

PreviousRegister: sets the final output alpha of this fragment on the model. This is the transparency that will be seen in-game after the execution of the last shader stage.
Register0: sets the alpha of the 1st color register in the material's TEV Color Block. This can be used to store an alpha value temporarily for use in one of the AlphaSelection properties in a later shader stage.
Register1: sets the alpha of the 2nd color register in the material's TEV Color Block. This can be used to store an alpha value temporarily for use in one of the AlphaSelection properties in a later shader stage.
Register2: sets the alpha of the 3rd color register in the material's TEV Color Block. This can be used to store an alpha value temporarily for use in one of the AlphaSelection properties in a later shader stage.")]
        public TevAlphaRegID AlphaRegister
        {
            get => _alphaEnv.Dest;
            set
            {
                _alphaEnv.Dest = value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Indirect Texturing")]
        public IndTexStageID TexStage
        {
            get => _cmd.StageID;
            set
            {
                _cmd.StageID = value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Indirect Texturing")]
        public IndTexFormat TexFormat
        {
            get => _cmd.Format;
            set
            {
                _cmd.Format = value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Indirect Texturing")]
        public IndTexBiasSel Bias
        {
            get => _cmd.Bias;
            set
            {
                _cmd.Bias = value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Indirect Texturing")]
        public IndTexAlphaSel Alpha
        {
            get => _cmd.Alpha;
            set
            {
                _cmd.Alpha = value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Indirect Texturing")]
        public IndTexMtxID Matrix
        {
            get => _cmd.Matrix;
            set
            {
                _cmd.Matrix = value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Indirect Texturing")]
        public IndTexWrap SWrap
        {
            get => _cmd.SWrap;
            set
            {
                _cmd.SWrap = value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Indirect Texturing")]
        public IndTexWrap TWrap
        {
            get => _cmd.TWrap;
            set
            {
                _cmd.TWrap = value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Indirect Texturing")]
        public bool UsePrevStage
        {
            get => _cmd.UsePrevStage;
            set
            {
                _cmd.UsePrevStage = value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Indirect Texturing")]
        public bool UnmodifiedLOD
        {
            get => _cmd.UnmodifiedLOD;
            set
            {
                _cmd.UnmodifiedLOD = value;
                SignalPropertyChange();
            }
        }

        public void Default()
        {
            Default(true);
        }

        public void Default(bool change)
        {
            _alphaEnv.SelA = AlphaArg.Zero;
            _alphaEnv.SelB = AlphaArg.Zero;
            _alphaEnv.SelC = AlphaArg.Zero;
            _alphaEnv.SelD = AlphaArg.Zero;
            _alphaEnv.Bias = Wii.Graphics.Bias.Zero;
            _alphaEnv.Clamp = true;

            _colorEnv.SelA = ColorArg.Zero;
            _colorEnv.SelB = ColorArg.Zero;
            _colorEnv.SelC = ColorArg.Zero;
            _colorEnv.SelD = ColorArg.Zero;
            _colorEnv.Bias = Wii.Graphics.Bias.Zero;
            _colorEnv.Clamp = true;

            _texMapID = TexMapID.TexMap7;
            _texCoord = TexCoordID.TexCoord7;
            _colorChan = ColorSelChan.Zero;

            if (change)
            {
                SignalPropertyChange();
            }
        }

        public void DefaultAsMetal(int texIndex)
        {
            if (Index == 0)
            {
                _colorEnv = 0x28F8AF;
                _alphaEnv = 0x08F2F0;
                ConstantColorSelection = TevKColorSel.ConstantColor0_RGB;
                ConstantAlphaSelection = TevKAlphaSel.ConstantColor0_Alpha;
                _colorChan = 0;
                TextureCoordID = TexCoordID.TexCoord0 + texIndex;
                TextureMapID = TexMapID.TexMap0 + texIndex;
                TextureEnabled = true;
            }
            else if (Index == 1)
            {
                _colorEnv = 0x08AFF0;
                _alphaEnv = 0x08FF80;
                ConstantColorSelection = TevKColorSel.ConstantColor0_RGB;
                ConstantAlphaSelection = TevKAlphaSel.ConstantColor0_Alpha;
                _colorChan = (ColorSelChan) 1;
                TextureCoordID = TexCoordID.TexCoord7;
                TextureMapID = TexMapID.TexMap7;
                TextureEnabled = false;
            }
            else if (Index == 2)
            {
                _colorEnv = 0x08FEB0;
                _alphaEnv = 0x081FF0;
                ConstantColorSelection = TevKColorSel.ConstantColor1_RGB;
                ConstantAlphaSelection = TevKAlphaSel.ConstantColor0_Alpha;
                _colorChan = 0;
                TextureCoordID = TexCoordID.TexCoord7;
                TextureMapID = TexMapID.TexMap7;
                TextureEnabled = false;
            }
            else if (Index == 3)
            {
                _colorEnv = 0x0806EF;
                _alphaEnv = 0x081FF0;
                ConstantColorSelection = TevKColorSel.ConstantColor0_RGB;
                ConstantAlphaSelection = TevKAlphaSel.ConstantColor0_Alpha;
                _colorChan = (ColorSelChan) 7;
                TextureCoordID = TexCoordID.TexCoord7;
                TextureMapID = TexMapID.TexMap7;
                TextureEnabled = false;
            }
        }

        //Don't get any strings from this node!
        internal override void GetStrings(StringTable table)
        {
        }

        public override void SignalPropertyChange()
        {
            if (Parent != null)
            {
                ((MDL0ShaderNode) Parent)._fragShaderSource = null;
            }

            base.SignalPropertyChange();
        }

        public override void Remove()
        {
            MDL0ShaderNode parent = Parent as MDL0ShaderNode;
            base.Remove();
            if (parent != null)
            {
                parent._fragShaderSource = null;
            }

            SignalPropertyChange();
        }

        public override bool MoveDown()
        {
            bool b = base.MoveDown();
            MDL0ShaderNode parent = Parent as MDL0ShaderNode;
            if (parent != null)
            {
                parent._fragShaderSource = null;
            }

            SignalPropertyChange();
            return b;
        }

        public override bool MoveUp()
        {
            bool b = base.MoveUp();
            MDL0ShaderNode parent = Parent as MDL0ShaderNode;
            if (parent != null)
            {
                parent._fragShaderSource = null;
            }

            SignalPropertyChange();
            return b;
        }

        public override void DoMoveDown(bool select)
        {
            base.DoMoveDown(select);
            MDL0ShaderNode parent = Parent as MDL0ShaderNode;
            if (parent != null)
            {
                parent._fragShaderSource = null;
            }

            SignalPropertyChange();
        }

        public override void DoMoveUp(bool select)
        {
            base.DoMoveUp(select);
            MDL0ShaderNode parent = Parent as MDL0ShaderNode;
            if (parent != null)
            {
                parent._fragShaderSource = null;
            }

            SignalPropertyChange();
        }

        public bool AnyTextureSourceUsed()
        {
            return
                ColorSelectionA == ColorArg.TextureColor ||
                ColorSelectionA == ColorArg.TextureAlpha ||
                ColorSelectionB == ColorArg.TextureColor ||
                ColorSelectionB == ColorArg.TextureAlpha ||
                ColorSelectionC == ColorArg.TextureColor ||
                ColorSelectionC == ColorArg.TextureAlpha ||
                ColorSelectionD == ColorArg.TextureColor ||
                ColorSelectionD == ColorArg.TextureAlpha ||
                AlphaSelectionA == AlphaArg.TextureAlpha ||
                AlphaSelectionB == AlphaArg.TextureAlpha ||
                AlphaSelectionC == AlphaArg.TextureAlpha ||
                AlphaSelectionD == AlphaArg.TextureAlpha;
        }

        public bool AnyRasterSourceUsed()
        {
            return
                ColorSelectionA == ColorArg.RasterColor ||
                ColorSelectionA == ColorArg.RasterAlpha ||
                ColorSelectionB == ColorArg.RasterColor ||
                ColorSelectionB == ColorArg.RasterAlpha ||
                ColorSelectionC == ColorArg.RasterColor ||
                ColorSelectionC == ColorArg.RasterAlpha ||
                ColorSelectionD == ColorArg.RasterColor ||
                ColorSelectionD == ColorArg.RasterAlpha ||
                AlphaSelectionA == AlphaArg.RasterAlpha ||
                AlphaSelectionB == AlphaArg.RasterAlpha ||
                AlphaSelectionC == AlphaArg.RasterAlpha ||
                AlphaSelectionD == AlphaArg.RasterAlpha;
        }

        public bool AnyConstantColorSourceUsed()
        {
            return
                ColorSelectionA == ColorArg.ConstantColorSelection ||
                ColorSelectionB == ColorArg.ConstantColorSelection ||
                ColorSelectionC == ColorArg.ConstantColorSelection ||
                ColorSelectionD == ColorArg.ConstantColorSelection;
        }

        public bool AnyConstantAlphaSourceUsed()
        {
            return
                AlphaSelectionA == AlphaArg.ConstantAlphaSelection ||
                AlphaSelectionB == AlphaArg.ConstantAlphaSelection ||
                AlphaSelectionC == AlphaArg.ConstantAlphaSelection ||
                AlphaSelectionD == AlphaArg.ConstantAlphaSelection;
        }

        public bool AnyReg0Used()
        {
            return
                ColorSelectionA == ColorArg.Color0 ||
                ColorSelectionA == ColorArg.Alpha0 ||
                ColorSelectionB == ColorArg.Color0 ||
                ColorSelectionB == ColorArg.Alpha0 ||
                ColorSelectionC == ColorArg.Color0 ||
                ColorSelectionC == ColorArg.Alpha0 ||
                ColorSelectionD == ColorArg.Color0 ||
                ColorSelectionD == ColorArg.Alpha0 ||
                AlphaSelectionA == AlphaArg.Alpha0 ||
                AlphaSelectionB == AlphaArg.Alpha0 ||
                AlphaSelectionC == AlphaArg.Alpha0 ||
                AlphaSelectionD == AlphaArg.Alpha0 ||
                ColorRegister == TevColorRegID.Color0 ||
                AlphaRegister == TevAlphaRegID.Alpha0;
        }

        public bool AnyReg1Used()
        {
            return
                ColorSelectionA == ColorArg.Color1 ||
                ColorSelectionA == ColorArg.Alpha1 ||
                ColorSelectionB == ColorArg.Color1 ||
                ColorSelectionB == ColorArg.Alpha1 ||
                ColorSelectionC == ColorArg.Color1 ||
                ColorSelectionC == ColorArg.Alpha1 ||
                ColorSelectionD == ColorArg.Color1 ||
                ColorSelectionD == ColorArg.Alpha1 ||
                AlphaSelectionA == AlphaArg.Alpha1 ||
                AlphaSelectionB == AlphaArg.Alpha1 ||
                AlphaSelectionC == AlphaArg.Alpha1 ||
                AlphaSelectionD == AlphaArg.Alpha1 ||
                ColorRegister == TevColorRegID.Color1 ||
                AlphaRegister == TevAlphaRegID.Alpha1;
        }

        public bool AnyReg2Used()
        {
            return
                ColorSelectionA == ColorArg.Color2 ||
                ColorSelectionA == ColorArg.Alpha2 ||
                ColorSelectionB == ColorArg.Color2 ||
                ColorSelectionB == ColorArg.Alpha2 ||
                ColorSelectionC == ColorArg.Color2 ||
                ColorSelectionC == ColorArg.Alpha2 ||
                ColorSelectionD == ColorArg.Color2 ||
                ColorSelectionD == ColorArg.Alpha2 ||
                AlphaSelectionA == AlphaArg.Alpha2 ||
                AlphaSelectionB == AlphaArg.Alpha2 ||
                AlphaSelectionC == AlphaArg.Alpha2 ||
                AlphaSelectionD == AlphaArg.Alpha2 ||
                ColorRegister == TevColorRegID.Color2 ||
                AlphaRegister == TevAlphaRegID.Alpha2;
        }
    }
}