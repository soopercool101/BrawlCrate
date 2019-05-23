using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using BrawlLib.Imaging;

namespace BrawlLib.SSBBTypes
{
    //const   int     ResourceNameStrMax  = 16;
    //const   int     MaterialNameStrMax  = ResourceNameStrMax + 4;
    //const   int     UserDataStrMax      =  8;
    //const   int     TexMtxMax           = 10;
    //const   int     TevColorMax         =  3;
    //const   int     TevKColorMax        =  4;
    //const   int     IndTexMtxMax        =  3;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct Size
    {
        public bfloat width;
        public bfloat height;
    }

    struct BinaryFileHeader
    {
        public uint signature;
        public bushort byteOrder;
        public bushort version;
        public buint fileSize;
        public bushort headerSize;
        public bushort dataBlocks;
    }

    struct DataBlockHeader
    {
        public uint signature;
        public buint size;
    }

    struct Layout
    {
        DataBlockHeader blockHeader;

        public byte originType;
        public fixed byte padding[3];

        Size layoutSize;
    }

    struct Font
    {
        public buint nameStrOffset;

        public byte type;
        public fixed byte padding[3];
    }

    struct FontList
    {
        DataBlockHeader blockHeader;

        public bushort fontNum;
        public fixed byte padding[2];

        //  Font fonts[fontNum];
        //  public byte nameStrPool[];
    }

    struct Texture
    {
        public buint nameStrOffset;

        public byte type;
        public fixed byte padding[3];
    }

    struct TextureList
    {
        DataBlockHeader blockHeader;

        public bushort texNum;
        public fixed byte padding[2];

        //  Texture textures[texNum];
        //  public byte nameStrPool[];
    }

    struct TexMap
    {
    //                        TexMap()
    //                        :   texIdx(0),
    //                            wrapSflt(0),
    //                            wrapTflt(0)
    //                        {}

    //    GXTexWrapMode       GetWarpModeS() const                { return GXTexWrapMode(detail::GetBits(wrapSflt,  0, 2)); }
    //    GXTexWrapMode       GetWarpModeT() const                { return GXTexWrapMode(detail::GetBits(wrapTflt,  0, 2)); }

    //    GXTexFilter         GetMinFilter() const
    //    {
    //        const int bitLen = 3;
    //        const public byte bitData = detail::GetBits(wrapSflt,  2, bitLen);
    //        return GXTexFilter(detail::GetBits(bitData + GX_LINEAR, 0, bitLen));
    //    }

    //    GXTexFilter         GetMagFilter() const
    //    {
    //        const int bitLen = 1;
    //        const public byte bitData = detail::GetBits(wrapTflt,  2, bitLen);
    //        return GXTexFilter(detail::GetBits(bitData + GX_LINEAR, 0, bitLen));
    //    }

    //    void                SetWarpModeS(GXTexWrapMode value)   { NW4R_ASSERT(value < GX_MAX_TEXWRAPMODE);  detail::SetBits(&wrapSflt,  0, 2, public byte(value)); }
    //    void                SetWarpModeT(GXTexWrapMode value)   { NW4R_ASSERT(value < GX_MAX_TEXWRAPMODE);  detail::SetBits(&wrapTflt,  0, 2, public byte(value)); }

    //    void                SetMinFilter(GXTexFilter value)
    //    {
    //        NW4R_ASSERT(value <= GX_LIN_MIP_LIN);
    //        const int bitLen = 3;
    //        const public byte bitData = public byte(detail::GetBits(value - GX_LINEAR, 0, bitLen));   // Set to zero when (value = GX_LINEAR)
    //        detail::SetBits(&wrapSflt,  2, bitLen, bitData);
    //    }

    //    void                SetMagFilter(GXTexFilter value)
    //    {
    //        NW4R_ASSERT(value <= GX_LINEAR);
    //        const int bitLen = 1;
    //        const public byte bitData = public byte(detail::GetBits(value - GX_LINEAR, 0, bitLen));   // Set to zero when (value = GX_LINEAR)
    //        detail::SetBits(&wrapTflt,  2, bitLen, bitData);
    //    }

        public bushort texIdx;
        public byte wrapSflt;
        public byte wrapTflt;
    }

    struct MaterialResourceNum
    {
        //                    MaterialResourceNum()
        //                        :   bits(0)
        //                    {}

        //public byte                  GetTexMapNum() const            { return public byte(detail::GetBits(bits,  0, 4)); }
        //public byte                  GetTexSRTNum() const            { return public byte(detail::GetBits(bits,  4, 4)); }
        //public byte                  GetTexCoordGenNum() const       { return public byte(detail::GetBits(bits,  8, 4)); }
        //bool                HasTevSwapTable() const         { return detail::TestBit(bits, 12); }
        //public byte                  GetIndTexSRTNum() const         { return public byte(detail::GetBits(bits, 13, 2)); }
        //public byte                  GetIndTexStageNum() const       { return public byte(detail::GetBits(bits, 15, 3)); }
        //public byte                  GetTevStageNum() const          { return public byte(detail::GetBits(bits, 18, 5)); }
        //bool                HasAlphaCompare() const         { return detail::TestBit(bits, 23); }
        //bool                HasBlendMode() const            { return detail::TestBit(bits, 24); }
        //public byte                  GetChanCtrlNum() const          { return public byte(detail::GetBits(bits, 25, 1)); }
        //public byte                  GetMatColNum() const            { return public byte(detail::GetBits(bits, 27, 1)); }

        //void                SetTexMapNum(public buint value)         { NW4R_ASSERT(value <= GX_MAX_TEXMAP);      detail::SetBits(&bits,  0, 4, value); }
        //void                SetTexSRTNum(public buint value)         { NW4R_ASSERT(value <= TexMtxMax);          detail::SetBits(&bits,  4, 4, value); }
        //void                SetTexCoordGenNum(public buint value)    { NW4R_ASSERT(value <= GX_MAX_TEXCOORD);    detail::SetBits(&bits,  8, 4, value); }
        //void                SetTevSwapTable(bool b)         {                                           detail::SetBit( &bits, 12, b);        }
        //void                SetIndTexSRTNum(public buint value)      { NW4R_ASSERT(value <= IndTexMtxMax);       detail::SetBits(&bits, 13, 2, value); }
        //void                SetIndTexStageNum(public buint value)    { NW4R_ASSERT(value <= GX_MAX_INDTEXSTAGE); detail::SetBits(&bits, 15, 3, value); }
        //void                SetTevStageNum(public buint value)       { NW4R_ASSERT(value <= GX_MAX_TEVSTAGE);    detail::SetBits(&bits, 18, 5, value); }
        //void                SetAlphaCompare(bool b)         {                                           detail::SetBit( &bits, 23, b);        }
        //void                SetBlendMode(bool b)            {                                           detail::SetBit( &bits, 24, b);        }
        //void                SetChanCtrlNum(public buint value)       { NW4R_ASSERT(value <= 1);                  detail::SetBits(&bits, 25, 1, value); }
        //void                SetMatColNum(public buint value)         { NW4R_ASSERT(value <= 1);                  detail::SetBits(&bits, 27, 1, value); }

        public buint bits;
    }

    struct Material
    {
        public fixed char name[20];
        GXColorS10 tevCol1;
        GXColorS10 tevCol2;
        GXColorS10 tevCol3;
        RGBAPixel tevKCol1;
        RGBAPixel tevKCol2;
        RGBAPixel tevKCol3;
        RGBAPixel tevKCol4;

        MaterialResourceNum resNum;

        //  TexMap              resTexMaps[texNum];
        //  TexSRT              texSRTs[texSRTNum];
        //  TexCoordGen         texCoordGen[texCoordGenNum];

        //  ChanCtrl            chanCtrl;
        //  GXColor             matCol;
        //  TevSwapMode         tevSwaps[GX_MAX_TEVSWAP];
        //  TexSRT              indTexSRTs[indTexSRTNum];
        //  IndirectStage       indirectStages[indirectStageNum];
        //  TevStage            tevStages[tevStageNum];
        //  AlphaCompare        alphaCompare;
        //  BlendMode           blendMode;
    }

    struct MaterialList
    {
        DataBlockHeader     blockHeader;

        public bushort                 materialNum;
        public byte                  padding[2];

        //  public buint                 materialOffsetTable[materialNum];
        //  Material            materials[materialNum];
    };

    struct Pane
    {
        DataBlockHeader     blockHeader;

        public byte                  flag;
        public byte                  basePosition;
        public byte                  alpha;
        public byte                  padding;

        char                name[ResourceNameStrMax];
        char                userData[UserDataStrMax];
        math::VEC3          translate;
        math::VEC3          rotate;
        math::VEC2          scale;
        Size                size;
    };

    struct Picture : Pane
    {
        public buint                 vtxCols[VERTEXCOLOR_MAX];

        public bushort                 materialIdx;
        public byte                  texCoordNum;
        public byte                  padding[1];

        //  math::VEC2          texCoords[texCoordNum][VERTEX_MAX];
    };

    struct TextBox : Pane
    {
        public bushort                 textBufBytes;
        public bushort                 textStrBytes;

        public bushort                 materialIdx;
        public bushort                 fontIdx;                    // Index value to the font resource

        public byte                  textPosition;
        public byte                  textAlignment;
        public byte                  padding[2];

        public buint                 textStrOffset;              // offset to the default text string
        public buint                 textCols[TEXTCOLOR_MAX];
        Size                fontSize;
        public bfloat                 charSpace;
        public bfloat                 lineSpace;

        //  wchar_t             text[];                     // text
    };

    struct WindowFrame
    {
        public bushort                 materialIdx;
        public byte                  textureFlip;
        public byte                  padding1;
    };

    struct WindowContent
    {
        public buint                 vtxCols[VERTEXCOLOR_MAX];

        public bushort                 materialIdx;
        public byte                  texCoordNum;
        public byte                  padding[1];

        //  math::VEC2          texCoords[texCoordNum][VERTEX_MAX];
    };

    struct Window : Pane
    {
        InflationLRTB       inflation;

        public byte                  frameNum;
        public byte                  padding1;
        public byte                  padding2;
        public byte                  padding3;

        public buint                 contentOffset;
        public buint                 frameOffsetTableOffset;

        //  WindowContent       content;

        //  public buint                 frameOffsetTable[frameNum];
        //  WindowFrame         frames;
    };

    struct Bounding : Pane
    {
    };

    struct ExtUserDataList
    {
        DataBlockHeader     blockHeader;

        detail::Respublic bushort      num;
        public byte                  padding[2];

        //  ExtUserData         extUserData[num];
    };

    struct Group
    {
        DataBlockHeader     blockHeader;

        char                name[ResourceNameStrMax];

        public bushort                 paneNum;
        public byte                  padding[2];

        //  char                panes[paneNum][ResourceNameStrMax];
    };

    struct AnimationTagBlock
    {
        DataBlockHeader     blockHeader;

        detail::Respublic bushort      tagOrder;
        detail::Respublic bushort      groupNum;

        detail::Respublic buint      nameOffset;
        detail::Respublic buint      groupsOffset;

        detail::ResS16      startFrame;
        detail::ResS16      endFrame;

        public byte                  flag;
        public byte                  padding[3];

        //  char                name[];
        //  AnimationGroupRef   groups[groupNum];
    };

    struct AnimationShareBlock
    {
        DataBlockHeader     blockHeader;

        detail::Respublic buint      animShareInfoOffset;

        detail::Respublic bushort      shareNum;
        public byte                  padding[2];

        //  AnimationShareInfo  animShareInfos[shareNum];
    };

    struct AnimationBlock
    {
        DataBlockHeader     blockHeader;
        public bushort                 frameSize;
        public byte                  loop;
        public byte                  padding1;

        public bushort                 fileNum;
        public bushort                 animContNum;

        public buint                 animContOffsetsOffset;

        //  public buint                 fileNameOffsets[fileNum];
        //  public byte                  namePool[];
        //  public buint                 animContOffsets[paneNum];
    };

    struct AnimationContent
    {
        char name[MaterialNameStrMax];   // allocate the number of characters for the longer of the pane name and the material name

        public byte                  num;
        public byte                  type;
        public byte                  padding[2];

        //  public buint                 animInfoOffsets[num];
    }

    struct AnimationInfo
    {
        public buint                 kind;

        public byte                  num;
        public byte                  padding[3];

        //  public buint                 animTargetOffsets[num];
    }

    struct AnimationTarget
    {
        public byte                  id;
        public byte                  target;
        public byte                  curveType;
        public byte                  padding1;

        public bushort                 keyNum;
        public byte                  padding2[2];

        public buint                 keysOffset;

        //  Hermite             keys[keyNum];
    }

    struct HermiteKey
    {
        public bfloat                 frame;
        public bfloat                 value;
        public bfloat                 slope;
    }

    struct StepKey
    {
        public bfloat                 frame;
        public bushort                 value;
        public bushort                 padding;
    }
}