using BrawlLib.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Graphics
{
    // This is for sending immediate XF (transform unit) commands.
    // All immediate XF commands have the form:
    //
    // | 8 bits     | 16 bits    | 16 bits    | 32 bits * length |
    // | cmd. token | length - 1 | 1st addr.  | reg. value(s)    |
    //
    // Length (the number of values being sent) can be up to 16 only.
    //
    // XF has a different register address space than BP or CP.

    public struct XFCommand
    {
        public bushort _length; //Reads one 32bit value for each (length + 1). 0 means one value follows
        public bushort _address;
    }

    public class XFData
    {
        public XFMemoryAddr _addr;
        public List<uint> _values = new List<uint>();

        public XFData()
        {
        }

        public XFData(XFMemoryAddr mem)
        {
            _addr = mem;
        }

        public XFData(ushort mem)
        {
            _addr = (XFMemoryAddr) mem;
        }

        public XFData(XFMemoryAddr mem, List<uint> values)
        {
            _addr = mem;
            _values = values;
        }

        public XFData(ushort mem, List<uint> values)
        {
            _addr = (XFMemoryAddr) mem;
            _values = values;
        }

        public XFData(XFMemoryAddr mem, params uint[] values)
        {
            _addr = mem;
            _values = values.ToList();
        }

        public XFData(ushort mem, params uint[] values)
        {
            _addr = (XFMemoryAddr) mem;
            _values = values.ToList();
        }

        public static unsafe List<XFData> Parse(VoidPtr address)
        {
            List<XFData> XFCmds = new List<XFData>();
            byte* pData = (byte*) address;
            Top:
            if (*pData++ == 0x10)
            {
                XFData dat = new XFData();
                int count = *(bushort*) pData;
                pData += 2;
                dat._addr = (XFMemoryAddr) (ushort) *(bushort*) pData;
                pData += 2;
                dat._values = new List<uint>();
                for (int i = 0; i < count + 1; i++)
                {
                    dat._values.Add(*(buint*) pData);
                    pData += 4;
                }

                XFCmds.Add(dat);
                goto Top;
            }

            return XFCmds;
        }

        public int Size => 5 + _values.Count * 4;

        public override string ToString()
        {
            string str = "Address: " + _addr;
            if (_values != null)
            {
                for (int i = 0; i < _values.Count; i++)
                {
                    if (_addr >= XFMemoryAddr.XF_TEX0_ID && _addr <= XFMemoryAddr.XF_TEX7_ID)
                    {
                        str += " | Value " + i + ": (" + new XFTexMtxInfo(_values[i]) + ")";
                    }
                    else if (_addr >= XFMemoryAddr.XF_DUALTEX0_ID && _addr <= XFMemoryAddr.XF_DUALTEX7_ID)
                    {
                        str += " | Value " + i + ": (" + new XFDualTex(_values[i]) + ")";
                    }
                }
            }

            return str;
        }
    }

    //TexMtxInfo
    //0000 0000 0000 0001   Padding
    //0000 0000 0000 0010   Projection
    //0000 0000 0000 1100   Input Form
    //0000 0000 0111 0000   Texgen Type
    //0000 1111 1000 0000   Source Row
    //0111 0000 0000 0000   Emboss Source
    //1000 0000 0000 0000   Emboss Light (all remaining bits)

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct XFTexMtxInfo
    {
        internal Bin32 _data;

        public TexProjection Projection
        {
            get => (TexProjection) (_data[1] ? 1 : 0);
            set => _data[1] = (uint) value != 0;
        }

        public TexInputForm InputForm
        {
            get => (TexInputForm) _data[2, 2];
            set => _data[2, 2] = (uint) value;
        }

        public TexTexgenType TexGenType
        {
            get => (TexTexgenType) _data[4, 3];
            set => _data[4, 3] = (uint) value;
        }

        public TexSourceRow SourceRow
        {
            get => (TexSourceRow) _data[7, 5];
            set => _data[7, 5] = (uint) value;
        }

        public int EmbossSource
        {
            get => (int) _data[12, 3];
            set => _data[12, 3] = (uint) value;
        }

        public int EmbossLight
        {
            get => (int) _data[15, 17];
            set => _data[15, 17] = (uint) value;
        }

        public XFTexMtxInfo(uint value)
        {
            _data = value;
        }

        public XFTexMtxInfo(
            TexProjection projection,
            TexInputForm inputForm,
            TexTexgenType genType,
            TexSourceRow sourceRow,
            int embossSource,
            int embossLight)
        {
            _data = 0;
            Projection = projection;
            InputForm = inputForm;
            TexGenType = genType;
            SourceRow = sourceRow;
            EmbossSource = embossSource;
            EmbossLight = embossLight;
        }

        public override string ToString()
        {
            return
                $"Projection: {Projection} | Input Form: {InputForm} | Texgen Type: {TexGenType} | Source Row: {SourceRow} | Emboss Source: {EmbossSource} | Emboss Light: {EmbossLight}";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct XFDualTex
    {
        public byte
            _pad0,
            _pad1,
            _normalEnable,
            _dualMtx;

        //Normal enable is true when projection is XF_TEX_STQ
        //DualMtx starts at 0 and increases by 3 for each texture (for every 3 matrix rows)

        public XFDualTex(uint value)
        {
            _pad0 = _pad1 = 0;
            _dualMtx = (byte) (value & 0xFF);
            _normalEnable = (byte) ((value >> 8) & 0xFF);
        }

        public XFDualTex(int mtx, int norm)
        {
            _pad0 = _pad1 = 0;
            _dualMtx = (byte) mtx;
            _normalEnable = (byte) norm;
        }

        public uint Value => (uint) ((_normalEnable << 8) | _dualMtx);

        public override string ToString()
        {
            return $"Normal Enable: {(_normalEnable != 0 ? "True" : "False")} | Dual Matrix: {_dualMtx}";
        }
    }

    public enum XFMemoryAddr : ushort
    {
        PosMatrices = 0x0000,

        Size = 0x8000,
        Error = 0x1000,
        Diag = 0x1001,
        State0 = 0x1002,
        State1 = 0x1003,
        Clock = 0x1004,
        ClipDisable = 0x1005,
        SetGPMetric = 0x1006,

        //VTXSpecs = 0x1008,
        //SetNumChan = 0x1009,
        SetChan0AmbColor = 0x100A,
        SetChan1AmbColor = 0x100B,
        SetChan0MatColor = 0x100C,
        SetChan1MatColor = 0x100D,
        SetChan0Color = 0x100E,
        SetChan1Color = 0x100F,
        SetChan0Alpha = 0x1010,
        SetChan1Alpha = 0x1011,
        DualTex = 0x1012,
        SetMatrixIndA = 0x1018,
        SetMatrixIndB = 0x1019,
        SetViewport = 0x101A,
        SetZScale = 0x101C,
        SetZOffset = 0x101F,
        SetProjection = 0x1020,
        //SetNumTexGens = 0x103F,
        //SetTexMtxInfo = 0x1040,
        //SetPosMtxInfo = 0x1050,

        XF_INVTXSPEC_ID = 0x1008,
        XF_NUMCOLORS_ID = 0x1009,
        XF_NUMTEX_ID = 0x103f,

        XF_TEX0_ID = 0x1040,
        XF_TEX1_ID = 0x1041,
        XF_TEX2_ID = 0x1042,
        XF_TEX3_ID = 0x1043,
        XF_TEX4_ID = 0x1044,
        XF_TEX5_ID = 0x1045,
        XF_TEX6_ID = 0x1046,
        XF_TEX7_ID = 0x1047,

        XF_DUALTEX0_ID = 0x1050,
        XF_DUALTEX1_ID = 0x1051,
        XF_DUALTEX2_ID = 0x1052,
        XF_DUALTEX3_ID = 0x1053,
        XF_DUALTEX4_ID = 0x1054,
        XF_DUALTEX5_ID = 0x1055,
        XF_DUALTEX6_ID = 0x1056,
        XF_DUALTEX7_ID = 0x1057
    }

    public enum XFNormalFormat
    {
        None = 0, //I've seen models with no normals.
        XYZ = 1,
        NBT = 2
    }

    //VTXSpecs
    //0000 0000 0000 0011   - Num colors
    //0000 0000 0000 1100   - Normal type (0 = none, 1 = normals, 2 = normals + binormals)
    //0000 0000 1111 0000   - Num textures
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct XFVertexSpecs
    {
        internal buint _data;

        public int ColorCount
        {
            get => (int) (_data & 3);
            set => _data = (_data & 0xFFFFFFFC) | ((uint) value & 3);
        }

        public int TextureCount
        {
            get => (int) ((_data >> 4) & 0xF);
            set => _data = (_data & 0xFFFFFF0F) | (((uint) value & 0xF) << 4);
        }

        public XFNormalFormat NormalFormat
        {
            get => (XFNormalFormat) ((_data >> 2) & 3);
            set => _data = (_data & 0xFFFFFFF3) | (((uint) value & 3) << 2);
        }

        public XFVertexSpecs(uint raw)
        {
            _data = raw;
        }

        public XFVertexSpecs(int colors, int textures, XFNormalFormat normalFormat)
        {
            _data = (((uint) textures & 0xF) << 4) | (((uint) normalFormat & 3) << 2) | ((uint) colors & 3);
        }

        public override string ToString()
        {
            return
                $"ColorCount: {ColorCount.ToString()} | TextureCount: {TextureCount.ToString()} | Normal Format: {NormalFormat.ToString()} [Data: {(int) _data}] ";
        }
    }

    //This is used by polygons to enable element arrays (I believe)
    //There doesn't seem to be a native spec for this
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct XFArrayFlags
    {
        internal Bin32 _data;

        //0000 0000 0000 0000 0000 0001 Pos Matrix
        //0000 0000 0000 0000 0000 0010 TexMtx0
        //0000 0000 0000 0000 0000 0100 TexMtx1
        //0000 0000 0000 0000 0000 1000 TexMtx2
        //0000 0000 0000 0000 0001 0000 TexMtx3
        //0000 0000 0000 0000 0010 0000 TexMtx4
        //0000 0000 0000 0000 0100 0000 TexMtx5
        //0000 0000 0000 0000 1000 0000 TexMtx6
        //0000 0000 0000 0001 0000 0000 TexMtx7
        //0000 0000 0000 0010 0000 0000 Positions
        //0000 0000 0000 0100 0000 0000 Normals
        //0000 0000 0000 1000 0000 0000 Color0
        //0000 0000 0001 0000 0000 0000 Color1
        //0000 0000 0010 0000 0000 0000 Tex0
        //0000 0000 0100 0000 0000 0000 Tex1
        //0000 0000 1000 0000 0000 0000 Tex2
        //0000 0001 0000 0000 0000 0000 Tex3
        //0000 0010 0000 0000 0000 0000 Tex4
        //0000 0100 0000 0000 0000 0000 Tex5
        //0000 1000 0000 0000 0000 0000 Tex6
        //0001 0000 0000 0000 0000 0000 Tex7

        public bool HasPosMatrix
        {
            get => _data[0];
            set => _data[0] = value;
        }

        public bool HasPositions
        {
            get => _data[9];
            set => _data[9] = value;
        }

        public bool HasNormals
        {
            get => _data[10];
            set => _data[10] = value;
        }

        public bool GetHasTexMatrix(int index)
        {
            return _data[index + 1];
        }

        public void SetHasTexMatrix(int index, bool exists)
        {
            _data[index + 1] = exists;
        }

        public bool GetHasColor(int index)
        {
            return _data[index + 11];
        }

        public void SetHasColor(int index, bool exists)
        {
            _data[index + 11] = exists;
        }

        public bool GetHasUVs(int index)
        {
            return _data[index + 13];
        }

        public void SetHasUVs(int index, bool exists)
        {
            _data[index + 13] = exists;
        }

        public override string ToString()
        {
            string texmtx = "";
            bool hasTex = false;
            for (int i = 0; i < 8; i++)
            {
                if (GetHasTexMatrix(i))
                {
                    hasTex = true;
                    texmtx += i + " ";
                }
            }

            string uvs = "";
            bool hasUVs = false;
            for (int i = 0; i < 8; i++)
            {
                if (GetHasUVs(i))
                {
                    hasUVs = true;
                    uvs += i + " ";
                }
            }

            string colors = "";
            bool hasColors = false;
            for (int i = 0; i < 2; i++)
            {
                if (GetHasUVs(i))
                {
                    hasColors = true;
                    colors += i + " ";
                }
            }

            return string.Format("PosMtx: {0} | TexMtx: {1}| Positions: {2} | Normals: {3} | Colors: {5} | UVs: {4}",
                HasPosMatrix ? "True" : "False", hasTex ? texmtx : "False ", HasPositions ? "True" : "False",
                HasNormals ? "True" : "False", hasUVs ? uvs : "False ", hasColors ? colors : "False ");
        }
    }

    public enum XFDataFormat : byte
    {
        None = 0,
        Direct = 1,
        Index8 = 2,
        Index16 = 3
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FacepointAttribute
    {
        public FacepointAttribute(GXAttribute attr, XFDataFormat fmt)
        {
            _attr = attr;
            _type = fmt;
        }

        public GXAttribute _attr;
        public XFDataFormat _type;
    }

    public enum GXAttribute
    {
        PosNrmMtxId = 0,
        Tex0MtxId,
        Tex1MtxId,
        Tex2MtxId,
        Tex3MtxId,
        Tex4MtxId,
        Tex5MtxId,
        Tex6MtxId,
        Tex7MtxId,
        Position = 9,
        Normal,
        Color0,
        Color1,
        Tex0,
        Tex1,
        Tex2,
        Tex3,
        Tex4,
        Tex5,
        Tex6,
        Tex7,

        PosMtxArray,
        NrmMtxArray,
        TexMtxArray,
        LightArray,
        NBT, //normal, bi-normal, tangent

        Max,
        Null = 0xFF
    }
}