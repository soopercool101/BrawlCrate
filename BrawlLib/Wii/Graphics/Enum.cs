namespace BrawlLib.Wii.Graphics
{
    public enum GXListCommand : byte
    {
        NOP = 0,

        LoadBPReg = 0x61,
        LoadCPReg = 0x08,
        LoadXFReg = 0x10,

        //LoadIndex - 32bit data
        //0000 0000 0000 0000 0000 1111 1111 1111   - XF Memory address
        //0000 0000 0000 0000 1111 0000 0000 0000   - Length (reads length + 1 uint values into XF memory)
        //1111 1111 1111 1111 0000 0000 0000 0000   - Index (for matrices, this is node index)

        LoadIndexA = 0x20, //Position matrices (4 x 3)
        LoadIndexB = 0x28, //Normal matrices (3 x 3)
        LoadIndexC = 0x30, //Post matrices (4 x 4)
        LoadIndexD = 0x38, //Lights

        CmdCallDL = 0x40,
        CmdUnknownMetrics = 0x44,
        CmdInvlVC = 0x48,

        DrawQuads = 0x80,
        DrawTriangles = 0x90,
        DrawTriangleStrip = 0x98,
        DrawTriangleFan = 0xA0,
        DrawLines = 0xA8,
        DrawLineStrip = 0xB0,
        DrawPoints = 0xB8
    }

    public enum GXCompare
    {
        Never,          //GX_NEVER,
        Less,           //GX_LESS,
        Equal,          //GX_EQUAL,
        LessOrEqual,    //GX_LEQUAL,
        Greater,        //GX_GREATER,
        NotEqual,       //GX_NEQUAL,
        GreaterOrEqual, //GX_GEQUAL,
        Always          //GX_ALWAYS
    }

    public enum GXBlendMode
    {
        None,
        Blend,
        Logic,
        Subtract
    }

    public enum GXLogicOp
    {
        Clear,       //GX_LO_CLEAR,
        And,         //GX_LO_AND,
        ReverseAnd,  //GX_LO_REVAND,
        Copy,        //GX_LO_COPY,
        InverseAnd,  //GX_LO_INVAND,
        NoOperation, //GX_LO_NOOP,
        ExclusiveOr, //GX_LO_XOR,
        Or,          //GX_LO_OR,
        NotOr,       //GX_LO_NOR,
        Equivalent,  //GX_LO_EQUIV,
        Inverse,     //GX_LO_INV,
        ReverseOr,   //GX_LO_REVOR,
        InverseCopy, //GX_LO_INVCOPY,
        InverseOr,   //GX_LO_INVOR,
        NotAnd,      //GX_LO_NAND,
        Set          //GX_LO_SET
    }

    public enum ColorChannel
    {
        Red = 0,
        Green,
        Blue,
        Alpha
    }

    public enum ColorArg
    {
        OutputColor,            //GX_CC_CPREV,
        OutputAlpha,            //GX_CC_APREV,
        Color0,                 //GX_CC_C0,
        Alpha0,                 //GX_CC_A0,
        Color1,                 //GX_CC_C1,
        Alpha1,                 //GX_CC_A1,
        Color2,                 //GX_CC_C2,
        Alpha2,                 //GX_CC_A2,
        TextureColor,           //GX_CC_TEXC,
        TextureAlpha,           //GX_CC_TEXA,
        RasterColor,            //GX_CC_RASC,
        RasterAlpha,            //GX_CC_RASA,
        One,                    //GX_CC_ONE, //1
        Half,                   //GX_CC_HALF, //0.5
        ConstantColorSelection, //GX_CC_KONST,
        Zero                    //GX_CC_ZERO //0
    }

    public enum AlphaArg
    {
        OutputAlpha,            //GX_CA_APREV,
        Alpha0,                 //GX_CA_A0,
        Alpha1,                 //GX_CA_A1,
        Alpha2,                 //GX_CA_A2,
        TextureAlpha,           //GX_CA_TEXA,
        RasterAlpha,            //GX_CA_RASA,
        ConstantAlphaSelection, //GX_CA_KONST,
        Zero                    //GX_CA_ZERO //0
    }

    public enum Bias
    {
        Zero,    //GX_TB_ZERO,
        AddHalf, //GX_TB_ADDHALF,
        SubHalf  //GX_TB_SUBHALF
    }

    public enum TevStageID
    {
        //GX_TEVSTAGE0,
        //GX_TEVSTAGE1,
        //GX_TEVSTAGE2,
        //GX_TEVSTAGE3,
        //GX_TEVSTAGE4,
        //GX_TEVSTAGE5,
        //GX_TEVSTAGE6,
        //GX_TEVSTAGE7,
        //GX_TEVSTAGE8,
        //GX_TEVSTAGE9,
        //GX_TEVSTAGE10,
        //GX_TEVSTAGE11,
        //GX_TEVSTAGE12,
        //GX_TEVSTAGE13,
        //GX_TEVSTAGE14,
        //GX_TEVSTAGE15,

        Stage0,
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
        Stage6,
        Stage7,
        Stage8,
        Stage9,
        Stage10,
        Stage11,
        Stage12,
        Stage13,
        Stage14,
        Stage15
    }

    public enum TevColorRegID
    {
        OutputColor,
        Color0,
        Color1,
        Color2
    }

    public enum TevAlphaRegID
    {
        OutputAlpha,
        Alpha0,
        Alpha1,
        Alpha2
    }

    public enum TevColorOp
    {
        Add = 0,
        Subtract = 1,

        CompR8Greater = 8,
        CompR8Equal = 9,
        CompGR16Greater = 10,
        CompGR16Equal = 11,
        CompBGR24Greater = 12,
        CompBGR24Equal = 13,
        CompRGB8Greater = 14,
        CompRGB8Equal = 15

        //GX_TEV_COMP_A8_GT = GX_TEV_COMP_RGB8_GT, // for alpha channel
        //GX_TEV_COMP_A8_EQ = GX_TEV_COMP_RGB8_EQ  // for alpha channel
    }

    public enum TevAlphaOp
    {
        Add = 0,
        Subtract = 1,

        CompR8Greater = 8,
        CompR8Equal = 9,
        CompGR16Greater = 10,
        CompGR16Equal = 11,
        CompBGR24Greater = 12,
        CompBGR24Equal = 13,
        CompA8Greater = 14,
        CompA8Equal = 15
    }

    public enum BlendFactor
    {
        //GX_BL_ZERO,
        //GX_BL_ONE,
        //GX_BL_SRCCLR,
        //GX_BL_INVSRCCLR,
        //GX_BL_SRCALPHA,
        //GX_BL_INVSRCALPHA,
        //GX_BL_DSTALPHA,
        //GX_BL_INVDSTALPHA,

        //GX_BL_DSTCLR = GX_BL_SRCCLR,
        //GX_BL_INVDSTCLR = GX_BL_INVSRCCLR

        Zero,
        One,
        SourceColor,
        InverseSourceColor,
        SourceAlpha,
        InverseSourceAlpha,
        DestinationAlpha,
        InverseDestinationAlpha

        //DestinationColor = SourceColor,
        //InverseDestinationColor = InverseSourceColor
    }

    public enum AlphaCompare
    {
        Never,          //COMPARE_NEVER = 0,
        Less,           //COMPARE_LESS,
        Equal,          //COMPARE_EQUAL,
        LessOrEqual,    //COMPARE_LEQUAL,
        Greater,        //COMPARE_GREATER,
        NotEqual,       //COMPARE_NEQUAL,
        GreaterOrEqual, //COMPARE_GEQUAL,
        Always          //COMPARE_ALWAYS
    }

    public enum TevSwapSel
    {
        Swap0, //GX_TEV_SWAP0 = 0,
        Swap1, //GX_TEV_SWAP1,
        Swap2, //GX_TEV_SWAP2,
        Swap3  //GX_TEV_SWAP3
    }

    public enum TevScale
    {
        MultiplyBy1, //GX_CS_SCALE_1,
        MultiplyBy2, //GX_CS_SCALE_2,
        MultiplyBy4, //GX_CS_SCALE_4,
        DivideBy2    //GX_CS_DIVIDE_2
    }

    public enum ProjectionType
    {
        Perspective = 0,
        Orthographic
    }

    public enum TevMode
    {
        MODULATE = 0,
        DECAL,
        BLEND,
        REPLACE,
        PASSCLR
    }

    public enum AlphaOp
    {
        And,               //ALPHAOP_AND = 0,
        Or,                //ALPHAOP_OR,
        ExclusiveOr,       //ALPHAOP_XOR,
        InverseExclusiveOr //ALPHAOP_XNOR
    }

    public enum TexMapID
    {
        TexMap0, //GX_TEXMAP0,
        TexMap1, //GX_TEXMAP1,
        TexMap2, //GX_TEXMAP2,
        TexMap3, //GX_TEXMAP3,
        TexMap4, //GX_TEXMAP4,
        TexMap5, //GX_TEXMAP5,
        TexMap6, //GX_TEXMAP6,

        TexMap7 //GX_TEXMAP7,
        //GX_MAX_TEXMAP,

        //GX_TEXMAP_NULL = 0xff,
        //GX_TEX_DISABLE = 0x100    // mask : disables texture look up
    }

    public enum ColorSelChan
    {
        //Rasterized color selections
        LightChannel0,           //RAS1_CC_0 = 0, // Color channel 0 
        LightChannel1,           //RAS1_CC_1 = 1, // Color channel 1 
        BumpAlpha = 5,           //RAS1_CC_B = 5, // Indirect texture bump alpha 
        NormalizedBumpAlpha = 6, //RAS1_CC_BN = 6, // Indirect texture bump alpha, normalized 0-255 
        Zero = 7                 //RAS1_CC_Z = 7 // Set color value to zero 
    }

    public enum ChannelID
    {
        GX_COLOR0,
        GX_COLOR1,
        GX_ALPHA0,
        GX_ALPHA1,
        GX_COLOR0A0,    // Color 0 + Alpha 0
        GX_COLOR1A1,    // Color 1 + Alpha 1
        GX_COLOR_ZERO,  // RGBA = 0
        GX_ALPHA_BUMP,  // bump alpha 0 - 248, RGB = 0
        GX_ALPHA_BUMPN, // normalized bump alpha, 0 - 255, RGB = 0
        GX_COLOR_NULL = 0xff
    }

    public enum TexCoordID
    {
        TexCoord0, //GX_TEXCOORD0 = 0x0, // generated texture coordinate 0
        TexCoord1, //GX_TEXCOORD1,         // generated texture coordinate 1
        TexCoord2, //GX_TEXCOORD2,         // generated texture coordinate 2
        TexCoord3, //GX_TEXCOORD3,         // generated texture coordinate 3
        TexCoord4, //GX_TEXCOORD4,         // generated texture coordinate 4
        TexCoord5, //GX_TEXCOORD5,         // generated texture coordinate 5
        TexCoord6, //GX_TEXCOORD6,         // generated texture coordinate 6

        TexCoord7 //GX_TEXCOORD7,         // generated texture coordinate 7
        //GX_MAX_TEXCOORD = 8,
        //GX_TEXCOORD_NULL = 0xff
    }

    public enum TevKAlphaSel
    {
        Constant1_1 /*GX_TEV_KASEL_8_8*/ = 0x00, //1.0f
        Constant7_8 /*GX_TEV_KASEL_7_8*/ = 0x01, //0.875f
        Constant3_4 /*GX_TEV_KASEL_6_8*/ = 0x02, //0.75f
        Constant5_8 /*GX_TEV_KASEL_5_8*/ = 0x03, //0.625f
        Constant1_2 /*GX_TEV_KASEL_4_8*/ = 0x04, //0.5f
        Constant3_8 /*GX_TEV_KASEL_3_8*/ = 0x05, //0.375f
        Constant1_4 /*GX_TEV_KASEL_2_8*/ = 0x06, //0.25f
        Constant1_8 /*GX_TEV_KASEL_1_8*/ = 0x07, //0.125f

        //GX_TEV_KASEL_1    = GX_TEV_KASEL_8_8,
        //GX_TEV_KASEL_3_4  = GX_TEV_KASEL_6_8,
        //GX_TEV_KASEL_1_2  = GX_TEV_KASEL_4_8,
        //GX_TEV_KASEL_1_4  = GX_TEV_KASEL_2_8,

        ConstantColor0_Red /*GX_TEV_KASEL_K0_R*/ = 0x10,
        ConstantColor1_Red /*GX_TEV_KASEL_K1_R*/ = 0x11,
        ConstantColor2_Red /*GX_TEV_KASEL_K2_R*/ = 0x12,
        ConstantColor3_Red /*GX_TEV_KASEL_K3_R*/ = 0x13,
        ConstantColor0_Green /*GX_TEV_KASEL_K0_G*/ = 0x14,
        ConstantColor1_Green /*GX_TEV_KASEL_K1_G*/ = 0x15,
        ConstantColor2_Green /*GX_TEV_KASEL_K2_G*/ = 0x16,
        ConstantColor3_Green /*GX_TEV_KASEL_K3_G*/ = 0x17,
        ConstantColor0_Blue /*GX_TEV_KASEL_K0_B*/ = 0x18,
        ConstantColor1_Blue /*GX_TEV_KASEL_K1_B*/ = 0x19,
        ConstantColor2_Blue /*GX_TEV_KASEL_K2_B*/ = 0x1A,
        ConstantColor3_Blue /*GX_TEV_KASEL_K3_B*/ = 0x1B,
        ConstantColor0_Alpha /*GX_TEV_KASEL_K0_A*/ = 0x1C,
        ConstantColor1_Alpha /*GX_TEV_KASEL_K1_A*/ = 0x1D,
        ConstantColor2_Alpha /*GX_TEV_KASEL_K2_A*/ = 0x1E,
        ConstantColor3_Alpha /*GX_TEV_KASEL_K3_A*/ = 0x1F
    }

    public enum TevKColorSel
    {
        Constant1_1 /*GX_TEV_KCSEL_8_8*/ = 0x00, //1.0f, 1.0f, 1.0f
        Constant7_8 /*GX_TEV_KCSEL_7_8*/ = 0x01, //0.875f, 0.875f, 0.875f
        Constant3_4 /*GX_TEV_KCSEL_6_8*/ = 0x02, //0.75f, 0.75f, 0.75f
        Constant5_8 /*GX_TEV_KCSEL_5_8*/ = 0x03, //0.625f, 0.625f, 0.625f
        Constant1_2 /*GX_TEV_KCSEL_4_8*/ = 0x04, //0.5f, 0.5f, 0.5f
        Constant3_8 /*GX_TEV_KCSEL_3_8*/ = 0x05, //0.375f, 0.375f, 0.375f
        Constant1_4 /*GX_TEV_KCSEL_2_8*/ = 0x06, //0.25f, 0.25f, 0.25f
        Constant1_8 /*GX_TEV_KCSEL_1_8*/ = 0x07, //0.125f, 0.125f, 0.125f

        //GX_TEV_KCSEL_1    = GX_TEV_KCSEL_8_8,
        //GX_TEV_KCSEL_3_4  = GX_TEV_KCSEL_6_8,
        //GX_TEV_KCSEL_1_2  = GX_TEV_KCSEL_4_8,
        //GX_TEV_KCSEL_1_4  = GX_TEV_KCSEL_2_8,

        ConstantColor0_RGB /*GX_TEV_KCSEL_K0*/ = 0x0C,
        ConstantColor1_RGB /*GX_TEV_KCSEL_K1*/ = 0x0D,
        ConstantColor2_RGB /*GX_TEV_KCSEL_K2*/ = 0x0E,
        ConstantColor3_RGB /*GX_TEV_KCSEL_K3*/ = 0x0F,
        ConstantColor0_RRR /*GX_TEV_KCSEL_K0_R*/ = 0x10,
        ConstantColor1_RRR /*GX_TEV_KCSEL_K1_R*/ = 0x11,
        ConstantColor2_RRR /*GX_TEV_KCSEL_K2_R*/ = 0x12,
        ConstantColor3_RRR /*GX_TEV_KCSEL_K3_R*/ = 0x13,
        ConstantColor0_GGG /*GX_TEV_KCSEL_K0_G*/ = 0x14,
        ConstantColor1_GGG /*GX_TEV_KCSEL_K1_G*/ = 0x15,
        ConstantColor2_GGG /*GX_TEV_KCSEL_K2_G*/ = 0x16,
        ConstantColor3_GGG /*GX_TEV_KCSEL_K3_G*/ = 0x17,
        ConstantColor0_BBB /*GX_TEV_KCSEL_K0_B*/ = 0x18,
        ConstantColor1_BBB /*GX_TEV_KCSEL_K1_B*/ = 0x19,
        ConstantColor2_BBB /*GX_TEV_KCSEL_K2_B*/ = 0x1A,
        ConstantColor3_BBB /*GX_TEV_KCSEL_K3_B*/ = 0x1B,
        ConstantColor0_AAA /*GX_TEV_KCSEL_K0_A*/ = 0x1C,
        ConstantColor1_AAA /*GX_TEV_KCSEL_K1_A*/ = 0x1D,
        ConstantColor2_AAA /*GX_TEV_KCSEL_K2_A*/ = 0x1E,
        ConstantColor3_AAA /*GX_TEV_KCSEL_K3_A*/ = 0x1F
    }

    public enum IndTexStageID
    {
        IndirectTexStg0, //GX_INDTEXSTAGE0,
        IndirectTexStg1, //GX_INDTEXSTAGE1,
        IndirectTexStg2, //GX_INDTEXSTAGE2,
        IndirectTexStg3  //GX_INDTEXSTAGE3
    }

    public enum IndTexAlphaSel
    {
        Off, //GX_ITBA_OFF,
        S,   //GX_ITBA_S,
        T,   //GX_ITBA_T,
        U    //GX_ITBA_U
    }

    public enum IndTexFormat
    {
        F_8_Bit_Offsets, //GX_ITF_8,        // 8 bit texture offsets.
        F_5_Bit_Offsets, //GX_ITF_5,        // 5 bit texture offsets.
        F_4_Bit_Offsets, //GX_ITF_4,        // 4 bit texture offsets.
        F_3_Bit_Offsets  //GX_ITF_3        // 3 bit texture offsets.
    }

    public enum IndTexBiasSel
    {
        None, //GX_ITB_NONE,
        S,    //GX_ITB_S,
        T,    //GX_ITB_T,
        ST,   //GX_ITB_ST,
        U,    //GX_ITB_U,
        SU,   //GX_ITB_SU,
        TU,   //GX_ITB_TU,
        STU   //GX_ITB_STU
    }

    public enum IndTexMtxID
    {
        NoMatrix,     //GX_ITM_OFF,
        Matrix0,      //GX_ITM_0,
        Matrix1,      //GX_ITM_1,
        Matrix2,      //GX_ITM_2,
        MatrixS0 = 5, //GX_ITM_S0 = 5,
        MatrixS1,     //GX_ITM_S1,
        MatrixS2,     //GX_ITM_S2,
        MatrixT0 = 9, //GX_ITM_T0 = 9,
        MatrixT1,     //GX_ITM_T1,
        MatrixT2      //GX_ITM_T2
    }

    public enum IndTexWrap
    {
        NoWrap,  //GX_ITW_OFF,        // no wrapping
        Wrap256, //GX_ITW_256,        // wrap 256
        Wrap128, //GX_ITW_128,        // wrap 128
        Wrap64,  //GX_ITW_64,         // wrap 64
        Wrap32,  //GX_ITW_32,         // wrap 32
        Wrap16,  //GX_ITW_16,         // wrap 16
        Wrap0    //GX_ITW_0,             // wrap 0
    }

    public enum IndTexScale
    {
        DivideBy1,   //GX_ITS_1,        // Scale by 1.
        DivideBy2,   //GX_ITS_2,        // Scale by 1/2.
        DivideBy4,   //GX_ITS_4,        // Scale by 1/4.
        DivideBy8,   //GX_ITS_8,        // Scale by 1/8.
        DivideBy16,  //GX_ITS_16,        // Scale by 1/16.
        DivideBy32,  //GX_ITS_32,        // Scale by 1/32.
        DivideBy64,  //GX_ITS_64,        // Scale by 1/64.
        DivideBy128, //GX_ITS_128,   // Scale by 1/128.
        DivideBy256  //GX_ITS_256    // Scale by 1/256.
    }

    public enum PosNrmMtx
    {
        GX_PNMTX0 = 0,
        GX_PNMTX1 = 3,
        GX_PNMTX2 = 6,
        GX_PNMTX3 = 9,
        GX_PNMTX4 = 12,
        GX_PNMTX5 = 15,
        GX_PNMTX6 = 18,
        GX_PNMTX7 = 21,
        GX_PNMTX8 = 24,
        GX_PNMTX9 = 27
    }

    public enum TexMtx
    {
        GX_TEXMTX0 = 30,
        GX_TEXMTX1 = 33,
        GX_TEXMTX2 = 36,
        GX_TEXMTX3 = 39,
        GX_TEXMTX4 = 42,
        GX_TEXMTX5 = 45,
        GX_TEXMTX6 = 48,
        GX_TEXMTX7 = 51,
        GX_TEXMTX8 = 54,
        GX_TEXMTX9 = 57,
        GX_IDENTITY = 60
    }

    public enum PTTexMtx
    {
        GX_PTTEXMTX0 = 64,
        GX_PTTEXMTX1 = 67,
        GX_PTTEXMTX2 = 70,
        GX_PTTEXMTX3 = 73,
        GX_PTTEXMTX4 = 76,
        GX_PTTEXMTX5 = 79,
        GX_PTTEXMTX6 = 82,
        GX_PTTEXMTX7 = 85,
        GX_PTTEXMTX8 = 88,
        GX_PTTEXMTX9 = 91,
        GX_PTTEXMTX10 = 94,
        GX_PTTEXMTX11 = 97,
        GX_PTTEXMTX12 = 100,
        GX_PTTEXMTX13 = 103,
        GX_PTTEXMTX14 = 106,
        GX_PTTEXMTX15 = 109,
        GX_PTTEXMTX16 = 112,
        GX_PTTEXMTX17 = 115,
        GX_PTTEXMTX18 = 118,
        GX_PTTEXMTX19 = 121,
        GX_PTIDENTITY = 125
    }

    public enum ClipMode
    {
        Clip_Enabled = 0,
        Clip_Disabled = 1
    }

    public enum TexMtxType
    {
        GX_MTX3x4 = 0,
        GX_MTX2x4
    }

    public enum TexProjection
    {
        ST, //XF_TEX_ST = 0x0,
        STQ //XF_TEX_STQ = 0x1
    }

    public enum TexInputForm
    {
        AB11, //XF_TEX_AB11 = 0x0,
        ABC1  //XF_TEX_ABC1 = 0x1
    }

    public enum TexTexgenType
    {
        Regular,   //XF_TEXGEN_REGULAR = 0x0,
        EmbossMap, //XF_TEXGEN_EMBOSS_MAP = 0x1,
        Color0,    //XF_TEXGEN_COLOR_STRGBC0 = 0x2,
        Color1     //XF_TEXGEN_COLOR_STRGBC1 = 0x3
    }

    public enum TexSourceRow
    {
        Geometry,   //XF_GEOM_INROW = 0x0,
        Normals,    //XF_NORMAL_INROW = 0x1,
        Colors,     //XF_COLORS_INROW = 0x2,
        BinormalsT, //XF_BINORMAL_T_INROW = 0x3,
        BinormalsB, //XF_BINORMAL_B_INROW = 0x4,
        TexCoord0,  //XF_TEX0_INROW = 0x5,
        TexCoord1,  //XF_TEX1_INROW  = 0x6,
        TexCoord2,  //XF_TEX2_INROW  = 0x7,
        TexCoord3,  //XF_TEX3_INROW =  0x8,
        TexCoord4,  //XF_TEX4_INROW  = 0x9,
        TexCoord5,  //XF_TEX5_INROW =  0xA,
        TexCoord6,  //XF_TEX6_INROW =  0xB,
        TexCoord7   //XF_TEX7_INROW =  0xC
    }

    public enum SpotFn
    {
        Off = 0,
        Flat,
        Cos,
        Cos2,
        Sharp,
        Ring,
        Ring2
    }

    public enum DistAttnFn
    {
        Off = 0,
        Gentle,
        Medium,
        Steep
    }

    public enum FogType
    {
        None = 0x00,

        PerspectiveLinear = 0x02,
        PerspectiveExp = 0x04,
        PerspectiveExp2 = 0x05,
        PerspectiveRevExp = 0x06,
        PerspectiveRevExp2 = 0x07,

        OrthographicLinear = 0x0A,
        OrthographicExp = 0x0C,
        OrthographicExp2 = 0x0D,
        OrthographicRevExp = 0x0E,
        OrthographicRevExp2 = 0x0F

        ////For compatibility with former versions
        //GX_FOG_LIN         = GX_FOG_PERSP_LIN,
        //GX_FOG_EXP        = GX_FOG_PERSP_EXP,
        //GX_FOG_EXP2        = GX_FOG_PERSP_EXP2,
        //GX_FOG_REVEXP      = GX_FOG_PERSP_REVEXP,
        //GX_FOG_REVEXP2     = GX_FOG_PERSP_REVEXP2
    }
}