using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Graphics
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BPCommand
    {
        public BPCommand(bool enabled)
        {
            Reg = (byte) (enabled ? 0x61 : 0);
            Mem = BPMemory.BPMEM_GENMODE;
            Data = 0;
        }

        public byte Reg; //0x61
        public BPMemory Mem;
        public BUInt24 Data;

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
    public struct GXAlphaFunction
    {
        public static readonly GXAlphaFunction Default = new GXAlphaFunction {_data = 0x3F};
        //0000 0000 0000 0000 1111 1111   ref0
        //0000 0000 1111 1111 0000 0000   ref1
        //0000 0111 0000 0000 0000 0000   comp0
        //0011 1000 0000 0000 0000 0000   comp1
        //1100 0000 0000 0000 0000 0000   logic

        public Bin8 _data;
        public byte _ref1, _ref0;

        public AlphaCompare Comp0
        {
            get => (AlphaCompare) _data[0, 3];
            set => _data[0, 3] = (byte) value;
        }

        public AlphaCompare Comp1
        {
            get => (AlphaCompare) _data[3, 3];
            set => _data[3, 3] = (byte) value;
        }

        public AlphaOp Logic
        {
            get => (AlphaOp) _data[6, 2];
            set => _data[6, 2] = (byte) value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ZMode
    {
        public static readonly ZMode Default = new ZMode {_data = 0x17};

        //0000 0001    Enable Depth Test
        //0000 1110    Depth Function
        //0001 0000    Enable Depth Update

        public byte _pad0, _pad1;
        public Bin8 _data;

        public bool EnableDepthTest
        {
            get => _data[0];
            set => _data[0] = value;
        }

        public bool EnableDepthUpdate
        {
            get => _data[4];
            set => _data[4] = value;
        }

        public GXCompare DepthFunction
        {
            get => (GXCompare) _data[1, 3];
            set => _data[1, 3] = (byte) value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BlendMode
    {
        public static readonly BlendMode Default = new BlendMode {_data = 0x34A0};

        //0000 0000 0000 0001    EnableBlend
        //0000 0000 0000 0010    EnableLogic
        //0000 0000 0000 0100    EnableDither
        //0000 0000 0000 1000    UpdateColor
        //0000 0000 0001 0000    UpdateAlpha
        //0000 0000 1110 0000    DstFactor
        //0000 0111 0000 0000    SrcFactor
        //0000 1000 0000 0000    Subtract
        //1111 0000 0000 0000    LogicOp

        public byte _pad;
        public Bin16 _data;

        public bool EnableBlend
        {
            get => _data[0];
            set => _data[0] = value;
        }

        public bool EnableLogicOp
        {
            get => _data[1];
            set => _data[1] = value;
        }

        public bool EnableDither
        {
            get => _data[2];
            set => _data[2] = value;
        }

        public bool EnableColorUpdate
        {
            get => _data[3];
            set => _data[3] = value;
        }

        public bool EnableAlphaUpdate
        {
            get => _data[4];
            set => _data[4] = value;
        }

        public BlendFactor DstFactor
        {
            get => (BlendFactor) _data[5, 3];
            set => _data[5, 3] = (ushort) value;
        }

        public BlendFactor SrcFactor
        {
            get => (BlendFactor) _data[8, 3];
            set => _data[8, 3] = (ushort) value;
        }

        public bool Subtract
        {
            get => _data[11];
            set => _data[11] = value;
        }

        public GXLogicOp LogicOp
        {
            get => (GXLogicOp) _data[12, 4];
            set => _data[12, 4] = (ushort) value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColorEnv
    {
        public static implicit operator int(ColorEnv val)
        {
            return val._data;
        }

        public static implicit operator uint(ColorEnv val)
        {
            return val._data;
        }

        public static implicit operator ColorEnv(int val)
        {
            return new ColorEnv((uint) val);
        }

        public static implicit operator ColorEnv(uint val)
        {
            return new ColorEnv(val);
        }

        public static implicit operator BUInt24(ColorEnv val)
        {
            return val._data;
        }

        public static implicit operator ColorEnv(BUInt24 val)
        {
            return new ColorEnv(val);
        }

        public ColorEnv(BUInt24 value)
        {
            _data = value;
        }

        public ColorEnv(uint value)
        {
            _data = value;
        }

        public Bin24 _data;

        //0000 0000 0000 0000 0000 1111   SelD
        //0000 0000 0000 0000 1111 0000   SelC
        //0000 0000 0000 1111 0000 0000   SelB
        //0000 0000 1111 0000 0000 0000   SelA
        //0000 0011 0000 0000 0000 0000   Bias
        //0000 0100 0000 0000 0000 0000   Sub
        //0000 1000 0000 0000 0000 0000   Clamp
        //0011 0000 0000 0000 0000 0000   Shift
        //1100 0000 0000 0000 0000 0000   Dest

        public ColorArg SelD
        {
            get => (ColorArg) _data[0, 4];
            set => _data[0, 4] = (int) value;
        }

        public ColorArg SelC
        {
            get => (ColorArg) _data[4, 4];
            set => _data[4, 4] = (int) value;
        }

        public ColorArg SelB
        {
            get => (ColorArg) _data[8, 4];
            set => _data[8, 4] = (int) value;
        }

        public ColorArg SelA
        {
            get => (ColorArg) _data[12, 4];
            set => _data[12, 4] = (int) value;
        }

        public Bias Bias
        {
            get => _data[16, 2] == 3 ? Bias.Zero : (Bias) _data[16, 2];
            set
            {
                if (_data[16, 2] == 3)
                {
                    return;
                }

                _data[16, 2] = (int) value;
            }
        }

        //public bool Sub { get { return _data[18]; } set { _data[18] = value; } }
        public bool Clamp
        {
            get => _data[19];
            set => _data[19] = value;
        }

        public TevScale Shift
        {
            get => _data[16, 2] == 3 ? TevScale.MultiplyBy1 : (TevScale) _data[20, 2];
            set
            {
                if (_data[16, 2] == 3)
                {
                    return;
                }

                _data[20, 2] = (int) value;
            }
        }

        public TevColorRegID Dest
        {
            get => (TevColorRegID) _data[22, 2];
            set => _data[22, 2] = (int) value;
        }

        public TevColorOp Operation
        {
            get => _data[16, 2] == 3 ? (TevColorOp) (((_data[18] ? 1 : 0) | (_data[20, 2] << 1)) + 8) :
                _data[18] ? TevColorOp.Subtract : TevColorOp.Add;
            set
            {
                _data[18] = ((ushort) value & 1) != 0;
                if (value <= TevColorOp.Subtract)
                {
                    _data[16, 2] = 0;
                    _data[20, 2] = 0;
                }
                else
                {
                    _data[16, 2] = 3;
                    _data[20, 2] = ((ushort) value >> 1) & 3;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AlphaEnv
    {
        public static implicit operator int(AlphaEnv val)
        {
            return val._data;
        }

        public static implicit operator uint(AlphaEnv val)
        {
            return val._data;
        }

        public static implicit operator AlphaEnv(int val)
        {
            return new AlphaEnv((uint) val);
        }

        public static implicit operator AlphaEnv(uint val)
        {
            return new AlphaEnv(val);
        }

        public static implicit operator BUInt24(AlphaEnv val)
        {
            return val._data;
        }

        public static implicit operator AlphaEnv(BUInt24 val)
        {
            return new AlphaEnv(val);
        }

        public AlphaEnv(BUInt24 value)
        {
            _data = value;
        }

        public AlphaEnv(uint value)
        {
            _data = value;
        }

        public Bin24 _data;

        //0000 0000 0000 0000 0000 0011   RSwap
        //0000 0000 0000 0000 0000 1100   TSwap
        //0000 0000 0000 0000 0111 0000   SelD
        //0000 0000 0000 0011 1000 0000   SelC
        //0000 0000 0001 1100 0000 0000   SelB
        //0000 0000 1110 0000 0000 0000   SelA
        //0000 0011 0000 0000 0000 0000   Bias
        //0000 0100 0000 0000 0000 0000   Sub
        //0000 1000 0000 0000 0000 0000   Clamp
        //0011 0000 0000 0000 0000 0000   Shift
        //1100 0000 0000 0000 0000 0000   Dest

        public TevSwapSel RSwap
        {
            get => (TevSwapSel) _data[0, 2];
            set => _data[0, 2] = (int) value;
        }

        public TevSwapSel TSwap
        {
            get => (TevSwapSel) _data[2, 2];
            set => _data[2, 2] = (int) value;
        }

        public AlphaArg SelD
        {
            get => (AlphaArg) _data[4, 3];
            set => _data[4, 3] = (int) value;
        }

        public AlphaArg SelC
        {
            get => (AlphaArg) _data[7, 3];
            set => _data[7, 3] = (int) value;
        }

        public AlphaArg SelB
        {
            get => (AlphaArg) _data[10, 3];
            set => _data[10, 3] = (int) value;
        }

        public AlphaArg SelA
        {
            get => (AlphaArg) _data[13, 3];
            set => _data[13, 3] = (int) value;
        }

        public Bias Bias
        {
            get => _data[16, 2] == 3 ? Bias.Zero : (Bias) _data[16, 2];
            set
            {
                if (_data[16, 2] == 3)
                {
                    return;
                }

                _data[16, 2] = (int) value;
            }
        }

        //public bool Sub { get { return _data[18]; } set { _data[18] = value; } }
        public bool Clamp
        {
            get => _data[19];
            set => _data[19] = value;
        }

        public TevScale Shift
        {
            get => _data[16, 2] == 3 ? TevScale.MultiplyBy1 : (TevScale) _data[20, 2];
            set
            {
                if (_data[16, 2] == 3)
                {
                    return;
                }

                _data[20, 2] = (int) value;
            }
        }

        public TevAlphaRegID Dest
        {
            get => (TevAlphaRegID) _data[22, 2];
            set => _data[22, 2] = (int) value;
        }

        public TevAlphaOp Operation
        {
            get => _data[16, 2] == 3 ? (TevAlphaOp) (((_data[18] ? 1 : 0) | (_data[20, 2] << 1)) + 8) :
                _data[18] ? TevAlphaOp.Subtract : TevAlphaOp.Add;
            set
            {
                _data[18] = ((ushort) value & 1) != 0;
                if (value <= TevAlphaOp.Subtract)
                {
                    _data[16, 2] = 0;
                    _data[20, 2] = 0;
                }
                else
                {
                    _data[16, 2] = 3;
                    _data[20, 2] = ((ushort) value >> 1) & 3;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RAS1_IRef
    {
        public static implicit operator int(RAS1_IRef val)
        {
            return val._data;
        }

        public static implicit operator uint(RAS1_IRef val)
        {
            return val._data;
        }

        public static implicit operator RAS1_IRef(int val)
        {
            return new RAS1_IRef((uint) val);
        }

        public static implicit operator RAS1_IRef(uint val)
        {
            return new RAS1_IRef(val);
        }

        public static implicit operator BUInt24(RAS1_IRef val)
        {
            return val._data;
        }

        public static implicit operator RAS1_IRef(BUInt24 val)
        {
            return new RAS1_IRef(val);
        }

        //public RAS1_IRef(BUInt24 value) { _data = value; }
        public RAS1_IRef(uint value)
        {
            _data = value;
        }

        public Bin24 _data;

        //0000 0000 0000 0000 0000 0111   BI0
        //0000 0000 0000 0000 0011 1000   BC0
        //0000 0000 0000 0001 1100 0000   BI1
        //0000 0000 0000 1110 0000 0000   BC1
        //0000 0000 0111 0000 0000 0000   BI2
        //0000 0011 1000 0000 0000 0000   BC2
        //0001 1100 0000 0000 0000 0000   BI3
        //1110 0000 0000 0000 0000 0000   BC3

        public TexMapID TexMap0
        {
            get => (TexMapID) _data[0, 3];
            set => _data[0, 3] = (int) value;
        }

        public TexCoordID TexCoord0
        {
            get => (TexCoordID) _data[3, 3];
            set => _data[3, 3] = (int) value;
        }

        public TexMapID TexMap1
        {
            get => (TexMapID) _data[6, 3];
            set => _data[6, 3] = (int) value;
        }

        public TexCoordID TexCoord1
        {
            get => (TexCoordID) _data[9, 3];
            set => _data[9, 3] = (int) value;
        }

        public TexMapID TexMap2
        {
            get => (TexMapID) _data[12, 3];
            set => _data[12, 3] = (int) value;
        }

        public TexCoordID TexCoord2
        {
            get => (TexCoordID) _data[15, 3];
            set => _data[15, 3] = (int) value;
        }

        public TexMapID TexMap3
        {
            get => (TexMapID) _data[18, 3];
            set => _data[18, 3] = (int) value;
        }

        public TexCoordID TexCoord3
        {
            get => (TexCoordID) _data[21, 3];
            set => _data[21, 3] = (int) value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RAS1_TRef
    {
        public static implicit operator int(RAS1_TRef val)
        {
            return val._data;
        }

        public static implicit operator uint(RAS1_TRef val)
        {
            return val._data;
        }

        public static implicit operator RAS1_TRef(int val)
        {
            return new RAS1_TRef((uint) val);
        }

        public static implicit operator RAS1_TRef(uint val)
        {
            return new RAS1_TRef(val);
        }

        public static implicit operator BUInt24(RAS1_TRef val)
        {
            return val._data;
        }

        public static implicit operator RAS1_TRef(BUInt24 val)
        {
            return new RAS1_TRef(val);
        }

        public RAS1_TRef(TexMapID tm0, TexCoordID tc0, bool te0, ColorSelChan cc0, TexMapID tm1, TexCoordID tc1,
                         bool te1, ColorSelChan cc1)
        {
            _data =
                ((uint) tm0 << 0) |
                ((uint) tc0 << 3) |
                ((uint) (te0 ? 1 : 0) << 6) |
                ((uint) cc0 << 7) |
                ((uint) tm1 << 12) |
                ((uint) tc1 << 15) |
                ((uint) (te1 ? 1 : 0) << 18) |
                ((uint) cc1 << 19);
        }

        public RAS1_TRef(BUInt24 value)
        {
            _data = value;
        }

        public RAS1_TRef(uint value)
        {
            _data = value;
        }

        public Bin24 _data;

        //0000 0000 0000 0000 0000 0111   TI0
        //0000 0000 0000 0000 0011 1000   TC0
        //0000 0000 0000 0000 0100 0000   TE0
        //0000 0000 0000 0011 1000 0000   CC0
        //0000 0000 0000 1100 0000 0000   PAD0
        //0000 0000 0111 0000 0000 0000   TI1
        //0000 0011 1000 0000 0000 0000   TC1
        //0000 0100 0000 0000 0000 0000   TE1
        //0011 1000 0000 0000 0000 0000   CC1
        //1100 0000 0000 0000 0000 0000   PAD1

        public TexMapID TexMapID0
        {
            get => (TexMapID) _data[0, 3];
            set => _data[0, 3] = (int) value;
        }

        public TexCoordID TexCoord0
        {
            get => (TexCoordID) _data[3, 3];
            set => _data[3, 3] = (int) value;
        }

        public bool TexEnabled0
        {
            get => _data[6];
            set => _data[6] = value;
        }

        public ColorSelChan ColorChannel0
        {
            get => (ColorSelChan) _data[7, 3];
            set => _data[7, 3] = (int) value;
        }

        public int Pad0
        {
            get => _data[10, 2];
            set => _data[10, 2] = value;
        }

        public TexMapID TexMapID1
        {
            get => (TexMapID) _data[12, 3];
            set => _data[12, 3] = (int) value;
        }

        public TexCoordID TexCoord1
        {
            get => (TexCoordID) _data[15, 3];
            set => _data[15, 3] = (int) value;
        }

        public bool TexEnabled1
        {
            get => _data[18];
            set => _data[18] = value;
        }

        public ColorSelChan ColorChannel1
        {
            get => (ColorSelChan) _data[19, 3];
            set => _data[19, 3] = (int) value;
        }

        public int Pad1
        {
            get => _data[22, 2];
            set => _data[22, 2] = value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct KSel
    {
        public static implicit operator int(KSel val)
        {
            return val._data;
        }

        public static implicit operator uint(KSel val)
        {
            return val._data;
        }

        public static implicit operator KSel(int val)
        {
            return new KSel((uint) val);
        }

        public static implicit operator KSel(uint val)
        {
            return new KSel(val);
        }

        public static implicit operator BUInt24(KSel val)
        {
            return val._data;
        }

        public static implicit operator KSel(BUInt24 val)
        {
            return new KSel(val);
        }

        public KSel(ColorChannel xrb, ColorChannel xga, TevKColorSel kc0, TevKAlphaSel ka0, TevKColorSel kc1,
                    TevKAlphaSel ka1)
        {
            _data =
                ((uint) xrb << 0) |
                ((uint) xga << 2) |
                ((uint) kc0 << 4) |
                ((uint) ka0 << 9) |
                ((uint) kc1 << 14) |
                ((uint) ka1 << 19);
        }

        //public KSel(BUInt24 value) { _data = value; }
        public KSel(uint value)
        {
            _data = value;
        }

        public Bin24 _data;

        //0000 0000 0000 0000 0000 0011   XRB - Swap Mode Only
        //0000 0000 0000 0000 0000 1100   XGA - Swap Mode Only
        //0000 0000 0000 0001 1111 0000   KCSEL0 - Selection Mode Only
        //0000 0000 0011 1110 0000 0000   KASEL0 - Selection Mode Only
        //0000 0111 1100 0000 0000 0000   KCSEL1 - Selection Mode Only
        //1111 1000 0000 0000 0000 0000   KASEL1 - Selection Mode Only

        public ColorChannel XRB
        {
            get => (ColorChannel) _data[0, 2];
            set => _data[0, 2] = (int) value;
        }

        public ColorChannel XGA
        {
            get => (ColorChannel) _data[2, 2];
            set => _data[2, 2] = (int) value;
        }

        public TevKColorSel KCSel0
        {
            get => (TevKColorSel) _data[4, 5];
            set => _data[4, 5] = (int) value;
        }

        public TevKAlphaSel KASel0
        {
            get => (TevKAlphaSel) _data[9, 5];
            set => _data[9, 5] = (int) value;
        }

        public TevKColorSel KCSel1
        {
            get => (TevKColorSel) _data[14, 5];
            set => _data[14, 5] = (int) value;
        }

        public TevKAlphaSel KASel1
        {
            get => (TevKAlphaSel) _data[19, 5];
            set => _data[19, 5] = (int) value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CMD
    {
        public static implicit operator int(CMD val)
        {
            return val._data;
        }

        public static implicit operator uint(CMD val)
        {
            return val._data;
        }

        public static implicit operator CMD(int val)
        {
            return new CMD((uint) val);
        }

        public static implicit operator CMD(uint val)
        {
            return new CMD(val);
        }

        public static implicit operator BUInt24(CMD val)
        {
            return val._data;
        }

        public static implicit operator CMD(BUInt24 val)
        {
            return new CMD(val);
        }

        public CMD(BUInt24 value)
        {
            _data = value;
        }

        public CMD(uint value)
        {
            _data = value;
        }

        public Bin24 _data;

        //0000 0000 0000 0000 0000 0011   BT
        //0000 0000 0000 0000 0000 1100   Format
        //0000 0000 0000 0000 0111 0000   Bias
        //0000 0000 0000 0001 1000 0000   BS
        //0000 0000 0001 1110 0000 0000   M
        //0000 0000 1110 0000 0000 0000   SW
        //0000 0111 0000 0000 0000 0000   TW
        //0000 1000 0000 0000 0000 0000   LB
        //0001 0000 0000 0000 0000 0000   FB
        //1110 0000 0000 0000 0000 0000   Pad

        public IndTexStageID StageID
        {
            get => (IndTexStageID) _data[0, 2];
            set => _data[0, 2] = (int) value;
        }

        public IndTexFormat Format
        {
            get => (IndTexFormat) _data[2, 2];
            set => _data[2, 2] = (int) value;
        }

        public IndTexBiasSel Bias
        {
            get => (IndTexBiasSel) _data[4, 3];
            set => _data[4, 3] = (int) value;
        }

        public IndTexAlphaSel Alpha
        {
            get => (IndTexAlphaSel) _data[7, 2];
            set => _data[7, 2] = (int) value;
        }

        public IndTexMtxID Matrix
        {
            get => (IndTexMtxID) _data[9, 4];
            set => _data[9, 4] = (int) value;
        }

        public IndTexWrap SWrap
        {
            get => (IndTexWrap) _data[13, 3];
            set => _data[13, 3] = (int) value;
        }

        public IndTexWrap TWrap
        {
            get => (IndTexWrap) _data[16, 3];
            set => _data[16, 3] = (int) value;
        }

        public bool UsePrevStage
        {
            get => _data[19];
            set => _data[19] = value;
        }

        public bool UnmodifiedLOD
        {
            get => _data[20];
            set => _data[20] = value;
        }

        public int Pad
        {
            get => _data[21, 3];
            set => _data[21, 3] = value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RAS1_SS
    {
        //0000 0000 0000 0000 0000 1111   SS0
        //0000 0000 0000 0000 1111 0000   TS0
        //0000 0000 0000 1111 0000 0000   SS1
        //0000 0000 1111 0000 0000 0000   TS1

        public byte _pad, _dat1, _dat2;

        public IndTexScale S_Scale0
        {
            get => (IndTexScale) (_dat2 & 0xF);
            set => _dat2 = (byte) ((_dat2 & 0xF0) | ((int) value & 0xF));
        }

        public IndTexScale T_Scale0
        {
            get => (IndTexScale) ((_dat2 >> 4) & 0xF);
            set => _dat2 = (byte) ((_dat2 & 0xF0) | ((int) value & 0xF));
        }

        public IndTexScale S_Scale1
        {
            get => (IndTexScale) (_dat1 & 0xF);
            set => _dat1 = (byte) ((_dat1 & 0xF0) | ((int) value & 0xF));
        }

        public IndTexScale T_Scale1
        {
            get => (IndTexScale) ((_dat1 >> 4) & 0xF);
            set => _dat1 = (byte) ((_dat1 & 0xF0) | ((int) value & 0xF));
        }
    }

    public enum RegType
    {
        //TEV register type field
        TEV_COLOR_REG = 0,
        TEV_KONSTANT_REG = 1
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColorReg
    {
        public static readonly ColorReg Konstant = new ColorReg {_dat0 = 0x80};

        //0000 0000 0000 1111 1111 1111 Red (Lo) / Blue (Hi)
        //0111 1111 1111 0000 0000 0000 Alpha (Lo) /Green (Hi)
        //1000 0000 0000 0000 0000 0000 Register Type

        public byte _dat0, _dat1, _dat2;

        public short RB
        {
            get => (short) (((short) (_dat1 << 13) >> 5) | _dat2);
            set
            {
                _dat1 = (byte) ((_dat1 & 0xF8) | ((value >> 8) & 0x7));
                _dat2 = (byte) (value & 0xFF);
            }
        }

        public short AG
        {
            get => (short) (((short) (_dat0 << 9) >> 5) | (_dat1 >> 4));
            set
            {
                _dat0 = (byte) ((_dat0 & 0x80) | ((value >> 4) & 0x7F));
                _dat1 = (byte) ((_dat1 & 0xF) | (value << 4));
            }
        }

        public RegType Type
        {
            get => (RegType) ((_dat0 & 0x80) != 0 ? 1 : 0);
            set => _dat0 = (byte) ((_dat0 & 0x7F) | ((int) value == 1 ? 0x80 : 0));
        }

        public override string ToString()
        {
            return $"A:{RB}, B:{AG}, Type:{Type}";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ConstantAlpha
    {
        public static readonly ConstantAlpha Default;

        public byte Pad, Enable, Value;
    }

    public enum BPMemory : byte
    {
        BPMEM_GENMODE = 0x00,

        BPMEM_DISPLAYCOPYFILER0 = 0x01,
        BPMEM_DISPLAYCOPYFILER1 = 0x02,
        BPMEM_DISPLAYCOPYFILER2 = 0x03,
        BPMEM_DISPLAYCOPYFILER3 = 0x04,
        BPMEM_DISPLAYCOPYFILER4 = 0x05,

        BPMEM_IND_MTXA0 = 0x06,
        BPMEM_IND_MTXB0 = 0x07,
        BPMEM_IND_MTXC0 = 0x08,
        BPMEM_IND_MTXA1 = 0x09,
        BPMEM_IND_MTXB1 = 0x0A,
        BPMEM_IND_MTXC1 = 0x0B,
        BPMEM_IND_MTXA2 = 0x0C,
        BPMEM_IND_MTXB2 = 0x0D,
        BPMEM_IND_MTXC2 = 0x0E,
        BPMEM_IND_IMASK = 0x0F,

        BPMEM_IND_CMD0 = 0x10,
        BPMEM_IND_CMD1 = 0x11,
        BPMEM_IND_CMD2 = 0x12,
        BPMEM_IND_CMD3 = 0x13,
        BPMEM_IND_CMD4 = 0x14,
        BPMEM_IND_CMD5 = 0x15,
        BPMEM_IND_CMD6 = 0x16,
        BPMEM_IND_CMD7 = 0x17,
        BPMEM_IND_CMD8 = 0x18,
        BPMEM_IND_CMD9 = 0x19,
        BPMEM_IND_CMDA = 0x1A,
        BPMEM_IND_CMDB = 0x1B,
        BPMEM_IND_CMDC = 0x1C,
        BPMEM_IND_CMDD = 0x1D,
        BPMEM_IND_CMDE = 0x1E,
        BPMEM_IND_CMDF = 0x1F,

        BPMEM_SCISSORTL = 0x20,
        BPMEM_SCISSORBR = 0x21,
        BPMEM_LINEPTWIDTH = 0x22,
        BPMEM_PERF0_TRI = 0x23,
        BPMEM_PERF0_QUAD = 0x24,

        BPMEM_RAS1_SS0 = 0x25,
        BPMEM_RAS1_SS1 = 0x26,
        BPMEM_IREF = 0x27,

        BPMEM_TREF0 = 0x28,
        BPMEM_TREF1 = 0x29,
        BPMEM_TREF2 = 0x2A,
        BPMEM_TREF3 = 0x2B,
        BPMEM_TREF4 = 0x2C,
        BPMEM_TREF5 = 0x2D,
        BPMEM_TREF6 = 0x2E,
        BPMEM_TREF7 = 0x2F,

        BPMEM_SU_SSIZE0 = 0x30,
        BPMEM_SU_TSIZE0 = 0x31,
        BPMEM_SU_SSIZE1 = 0x32,
        BPMEM_SU_TSIZE1 = 0x33,
        BPMEM_SU_SSIZE2 = 0x34,
        BPMEM_SU_TSIZE2 = 0x35,
        BPMEM_SU_SSIZE3 = 0x36,
        BPMEM_SU_TSIZE3 = 0x37,
        BPMEM_SU_SSIZE4 = 0x38,
        BPMEM_SU_TSIZE4 = 0x39,
        BPMEM_SU_SSIZE5 = 0x3A,
        BPMEM_SU_TSIZE5 = 0x3B,
        BPMEM_SU_SSIZE6 = 0x3C,
        BPMEM_SU_TSIZE6 = 0x3D,
        BPMEM_SU_SSIZE7 = 0x3E,
        BPMEM_SU_TSIZE7 = 0x3F,

        BPMEM_ZMODE = 0x40,
        BPMEM_BLENDMODE = 0x41,
        BPMEM_CONSTANTALPHA = 0x42,
        BPMEM_ZCOMPARE = 0x43,
        BPMEM_FIELDMASK = 0x44,
        BPMEM_SETDRAWDONE = 0x45,
        BPMEM_BUSCLOCK0 = 0x46,
        BPMEM_PE_TOKEN_ID = 0x47,
        BPMEM_PE_TOKEN_INT_ID = 0x48,

        BPMEM_EFB_TL = 0x49,
        BPMEM_EFB_BR = 0x4A,
        BPMEM_EFB_ADDR = 0x4B,

        BPMEM_MIPMAP_STRIDE = 0x4D,
        BPMEM_COPYYSCALE = 0x4E,

        BPMEM_CLEAR_AR = 0x4F,
        BPMEM_CLEAR_GB = 0x50,
        BPMEM_CLEAR_Z = 0x51,

        BPMEM_TRIGGER_EFB_COPY = 0x52,
        BPMEM_COPYFILTER0 = 0x53,
        BPMEM_COPYFILTER1 = 0x54,
        BPMEM_CLEARBBOX1 = 0x55,
        BPMEM_CLEARBBOX2 = 0x56,

        BPMEM_UNKNOWN_57 = 0x57,

        BPMEM_REVBITS = 0x58,
        BPMEM_SCISSOROFFSET = 0x59,

        BPMEM_UNKNOWN_60 = 0x60,
        BPMEM_UNKNOWN_61 = 0x61,
        BPMEM_UNKNOWN_62 = 0x62,

        BPMEM_TEXMODESYNC = 0x63,
        BPMEM_LOADTLUT0 = 0x64,
        BPMEM_LOADTLUT1 = 0x65,
        BPMEM_TEXINVALIDATE = 0x66,
        BPMEM_PERF1 = 0x67,
        BPMEM_FIELDMODE = 0x68,
        BPMEM_BUSCLOCK1 = 0x69,

        BPMEM_TX_SETMODE0_A = 0x80,
        BPMEM_TX_SETMODE0_B = 0x81,
        BPMEM_TX_SETMODE0_C = 0x82,
        BPMEM_TX_SETMODE0_D = 0x83,

        BPMEM_TX_SETMODE1_A = 0x84,
        BPMEM_TX_SETMODE1_B = 0x85,
        BPMEM_TX_SETMODE1_C = 0x86,
        BPMEM_TX_SETMODE1_D = 0x87,

        BPMEM_TX_SETIMAGE0_A = 0x88,
        BPMEM_TX_SETIMAGE0_B = 0x89,
        BPMEM_TX_SETIMAGE0_C = 0x8A,
        BPMEM_TX_SETIMAGE0_D = 0x8B,

        BPMEM_TX_SETIMAGE1_A = 0x8C,
        BPMEM_TX_SETIMAGE1_B = 0x8D,
        BPMEM_TX_SETIMAGE1_C = 0x8E,
        BPMEM_TX_SETIMAGE1_D = 0x8F,

        BPMEM_TX_SETIMAGE2_A = 0x90,
        BPMEM_TX_SETIMAGE2_B = 0x91,
        BPMEM_TX_SETIMAGE2_C = 0x92,
        BPMEM_TX_SETIMAGE2_D = 0x93,

        BPMEM_TX_SETIMAGE3_A = 0x94,
        BPMEM_TX_SETIMAGE3_B = 0x95,
        BPMEM_TX_SETIMAGE3_C = 0x96,
        BPMEM_TX_SETIMAGE3_D = 0x97,

        BPMEM_TX_SETTLUT_A = 0x98,
        BPMEM_TX_SETTLUT_B = 0x99,
        BPMEM_TX_SETTLUT_C = 0x9A,
        BPMEM_TX_SETTLUT_D = 0x9B,

        BPMEM_TX_SETMODE0_4_A = 0xA0,
        BPMEM_TX_SETMODE0_4_B = 0xA1,
        BPMEM_TX_SETMODE0_4_C = 0xA2,
        BPMEM_TX_SETMODE0_4_D = 0xA3,

        BPMEM_TX_SETMODE1_4_A = 0xA4,
        BPMEM_TX_SETMODE1_4_B = 0xA5,
        BPMEM_TX_SETMODE1_4_C = 0xA6,
        BPMEM_TX_SETMODE1_4_D = 0xA7,

        BPMEM_TX_SETIMAGE0_4_A = 0xA8,
        BPMEM_TX_SETIMAGE0_4_B = 0xA9,
        BPMEM_TX_SETIMAGE0_4_C = 0xAA,
        BPMEM_TX_SETIMAGE0_4_D = 0xAB,

        BPMEM_TX_SETIMAGE1_4_A = 0xAC,
        BPMEM_TX_SETIMAGE1_4_B = 0xAD,
        BPMEM_TX_SETIMAGE1_4_C = 0xAE,
        BPMEM_TX_SETIMAGE1_4_D = 0xAF,

        BPMEM_TX_SETIMAGE2_4_A = 0xB0,
        BPMEM_TX_SETIMAGE2_4_B = 0xB1,
        BPMEM_TX_SETIMAGE2_4_C = 0xB2,
        BPMEM_TX_SETIMAGE2_4_D = 0xB3,

        BPMEM_TX_SETIMAGE3_4_A = 0xB4,
        BPMEM_TX_SETIMAGE3_4_B = 0xB5,
        BPMEM_TX_SETIMAGE3_4_C = 0xB6,
        BPMEM_TX_SETIMAGE3_4_D = 0xB7,

        BPMEM_TX_SETLUT_4_A = 0xB8,
        BPMEM_TX_SETLUT_4_B = 0xB9,
        BPMEM_TX_SETLUT_4_C = 0xBA,
        BPMEM_TX_SETLUT_4_D = 0xBB,

        BPMEM_UNKNOWN_BC = 0xBC,
        BPMEM_UNKNOWN_BB = 0xBB,
        BPMEM_UNKNOWN_BD = 0xBD,
        BPMEM_UNKNOWN_BE = 0xBE,
        BPMEM_UNKNOWN_BF = 0xBF,

        BPMEM_TEV_COLOR_ENV_0 = 0xC0,
        BPMEM_TEV_ALPHA_ENV_0 = 0xC1,
        BPMEM_TEV_COLOR_ENV_1 = 0xC2,
        BPMEM_TEV_ALPHA_ENV_1 = 0xC3,
        BPMEM_TEV_COLOR_ENV_2 = 0xC4,
        BPMEM_TEV_ALPHA_ENV_2 = 0xC5,
        BPMEM_TEV_COLOR_ENV_3 = 0xC6,
        BPMEM_TEV_ALPHA_ENV_3 = 0xC7,
        BPMEM_TEV_COLOR_ENV_4 = 0xC8,
        BPMEM_TEV_ALPHA_ENV_4 = 0xC9,
        BPMEM_TEV_COLOR_ENV_5 = 0xCA,
        BPMEM_TEV_ALPHA_ENV_5 = 0xCB,
        BPMEM_TEV_COLOR_ENV_6 = 0xCC,
        BPMEM_TEV_ALPHA_ENV_6 = 0xCD,
        BPMEM_TEV_COLOR_ENV_7 = 0xCE,
        BPMEM_TEV_ALPHA_ENV_7 = 0xCF,
        BPMEM_TEV_COLOR_ENV_8 = 0xD0,
        BPMEM_TEV_ALPHA_ENV_8 = 0xD1,
        BPMEM_TEV_COLOR_ENV_9 = 0xD2,
        BPMEM_TEV_ALPHA_ENV_9 = 0xD3,
        BPMEM_TEV_COLOR_ENV_A = 0xD4,
        BPMEM_TEV_ALPHA_ENV_A = 0xD5,
        BPMEM_TEV_COLOR_ENV_B = 0xD6,
        BPMEM_TEV_ALPHA_ENV_B = 0xD7,
        BPMEM_TEV_COLOR_ENV_C = 0xD8,
        BPMEM_TEV_ALPHA_ENV_C = 0xD9,
        BPMEM_TEV_COLOR_ENV_D = 0xDA,
        BPMEM_TEV_ALPHA_ENV_D = 0xDB,
        BPMEM_TEV_COLOR_ENV_E = 0xDC,
        BPMEM_TEV_ALPHA_ENV_E = 0xDD,
        BPMEM_TEV_COLOR_ENV_F = 0xDE,
        BPMEM_TEV_ALPHA_ENV_F = 0xDF,

        BPMEM_TEV_REGISTER_L_0 = 0xE0,
        BPMEM_TEV_REGISTER_H_0 = 0xE1,
        BPMEM_TEV_REGISTER_L_1 = 0xE2,
        BPMEM_TEV_REGISTER_H_1 = 0xE3,
        BPMEM_TEV_REGISTER_L_2 = 0xE4,
        BPMEM_TEV_REGISTER_H_2 = 0xE5,
        BPMEM_TEV_REGISTER_L_3 = 0xE6,
        BPMEM_TEV_REGISTER_H_3 = 0xE7,

        BPMEM_TEV_FOG_RANGE = 0xE8,
        BPMEM_TEV_FOG_PARAM_0 = 0xEE,
        BPMEM_TEV_FOG_B_MAGNITUDE = 0xEF,
        BPMEM_TEV_FOG_B_EXPONENT = 0xF0,
        BPMEM_TEV_FOG_PARAM_3 = 0xF1,
        BPMEM_TEV_FOG_COLOR = 0xF2,

        BPMEM_ALPHACOMPARE = 0xF3,
        BPMEM_BIAS = 0xF4,
        BPMEM_ZTEX2 = 0xF5,

        BPMEM_TEV_KSEL0 = 0xF6,
        BPMEM_TEV_KSEL1 = 0xF7,
        BPMEM_TEV_KSEL2 = 0xF8,
        BPMEM_TEV_KSEL3 = 0xF9,
        BPMEM_TEV_KSEL4 = 0xFA,
        BPMEM_TEV_KSEL5 = 0xFB,
        BPMEM_TEV_KSEL6 = 0xFC,
        BPMEM_TEV_KSEL7 = 0xFD,

        BPMEM_BP_MASK = 0xFE
    }
}