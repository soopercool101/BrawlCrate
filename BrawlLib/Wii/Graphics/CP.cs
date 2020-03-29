using BrawlLib.Internal;
using BrawlLib.Wii.Models;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Graphics
{
    public enum CPCommand : byte
    {
        SetMatricesA = 3,
        SetMatricesB = 4,
        SetVertexDescLo = 5,
        SetVertexDescHi = 6,
        SetUVATGroup0 = 7,
        SetUVATGroup1 = 8,
        SetUVATGroup2 = 9
    }

    //Vertex Format Lo (lower 17 bits of vtx format register)
    //0000 0000 0000 0000 0000 0000 0000 0001     - Vertex/Normal matrix index
    //0000 0000 0000 0000 0000 0000 0000 0010     - Texture Matrix 0
    //0000 0000 0000 0000 0000 0000 0000 0100     - Texture Matrix 1
    //0000 0000 0000 0000 0000 0000 0000 1000     - Texture Matrix 2
    //0000 0000 0000 0000 0000 0000 0001 0000     - Texture Matrix 3
    //0000 0000 0000 0000 0000 0000 0010 0000     - Texture Matrix 4
    //0000 0000 0000 0000 0000 0000 0100 0000     - Texture Matrix 5
    //0000 0000 0000 0000 0000 0000 1000 0000     - Texture Matrix 6
    //0000 0000 0000 0000 0000 0001 0000 0000     - Texture Matrix 7
    //0000 0000 0000 0000 0000 0110 0000 0000     - Vertex format
    //0000 0000 0000 0000 0001 1000 0000 0000     - Normal format
    //0000 0000 0000 0000 0110 0000 0000 0000     - Color0 format
    //0000 0000 0000 0001 1000 0000 0000 0000     - Color1 format

    //Vertex Format Hi (shifted left 17 bits before storing)
    //0000 0000 0000 0000 0110 0000 0000 0000 0000     - Tex0 format
    //0000 0000 0000 0001 1000 0000 0000 0000 0000     - Tex1 format
    //0000 0000 0000 0110 0000 0000 0000 0000 0000     - Tex2 format
    //0000 0000 0001 1000 0000 0000 0000 0000 0000     - Tex3 format
    //0000 0000 0110 0000 0000 0000 0000 0000 0000     - Tex4 format
    //0000 0001 1000 0000 0000 0000 0000 0000 0000     - Tex5 format
    //0000 0110 0000 0000 0000 0000 0000 0000 0000     - Tex6 format
    //0001 1000 0000 0000 0000 0000 0000 0000 0000     - Tex7 format

    //Vertex Format Hi
    //0000 0000 0000 0000 0000 0000 0000 0011     - Tex0 format
    //0000 0000 0000 0000 0000 0000 0000 1100     - Tex1 format
    //0000 0000 0000 0000 0000 0000 0011 0000     - Tex2 format
    //0000 0000 0000 0000 0000 0000 1100 0000     - Tex3 format
    //0000 0000 0000 0000 0000 0011 0000 0000     - Tex4 format
    //0000 0000 0000 0000 0000 1100 0000 0000     - Tex5 format
    //0000 0000 0000 0000 0011 0000 0000 0000     - Tex6 format
    //0000 0000 0000 0000 1100 0000 0000 0000     - Tex7 format

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CPVertexFormat
    {
        public buint _lo, _hi;

        public bool HasPosMatrix
        {
            get => (_lo & 1) != 0;
            set => _lo = (_lo & 0xFFFFFFFE) | (uint) (value ? 1 : 0);
        }

        public XFDataFormat PosFormat
        {
            get => (XFDataFormat) ((_lo >> 9) & 3);
            set => _lo = (_lo & 0xFFFFF9FF) | (uint) value;
        }

        public XFDataFormat NormalFormat
        {
            get => (XFDataFormat) ((_lo >> 11) & 3);
            set => _lo = (_lo & 0xFFFFE7FF) | (uint) value;
        }

        public CPVertexFormat(uint lo, uint hi)
        {
            _lo = lo;
            _hi = hi;
        }

        //public XFDataFormat GetColorFormat(int index) { return (XFDataFormat)(_lo >> 13 & 3); }
        public XFDataFormat GetColorFormat(int index)
        {
            return (XFDataFormat) ((_lo >> (index * 2 + 13)) & 3);
        }

        public void SetColorFormat(int index, XFDataFormat format)
        {
            _lo = (_lo & ~((uint) 3 << (index * 2 + 13))) | ((uint) format << (index * 2 + 13));
        }

        public bool GetHasTexMatrix(int index)
        {
            return ((_lo >> (index + 1)) & 1) != 0;
        }

        public void SetHasTexMatrix(int index, bool value)
        {
            _lo = (_lo & ~(uint) (1 << (index + 1))) | ((uint) (value ? 1 : 0) << (index + 1));
        }

        public XFDataFormat GetUVFormat(int index)
        {
            return (XFDataFormat) ((_hi >> (index * 2)) & 3);
        }

        public void SetUVFormat(int index, XFDataFormat format)
        {
            _hi = (_hi & ~(uint) (3 << (index * 2))) | (uint) ((int) format << (index * 2));
        }

        public override string ToString()
        {
            XFDataFormat f;

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
                if ((f = GetUVFormat(i)) != XFDataFormat.None)
                {
                    hasUVs = true;
                    uvs += i + ":" + f + " ";
                }
            }

            string colors = "";
            bool hasColors = false;
            for (int i = 0; i < 2; i++)
            {
                if ((f = GetColorFormat(i)) != XFDataFormat.None)
                {
                    hasColors = true;
                    colors += i + ":" + f + " ";
                }
            }

            return string.Format(
                "PosMtx: {0} | TexMtx: {1}| PosFmt: {2} | NormFmt: {3} | ColorFmt: {5}| UVFmt: {4}[Lo: {6}, Hi: {7} ]",
                HasPosMatrix ? "True" : "False", hasTex ? texmtx : "False ", PosFormat.ToString(),
                NormalFormat.ToString(), hasUVs ? uvs : "None ", hasColors ? colors : "None ", (int) _lo, (int) _hi);
        }

        //#region ElementFlags
        //public bool HasVertexData { get { return (_lo & 0x400) != 0; } }
        //public int VertexEntryLength { get { return (HasVertexData) ? (((_lo & 0x200) != 0) ? 2 : 1) : 0; } }
        //public bool HasNormalData { get { return (_lo & 0x1000) != 0; } }
        //public int NormalEntryLength { get { return (HasNormalData) ? (((_lo & 0x800) != 0) ? 2 : 1) : 0; } }

        //public bool HasColorData { get { return (_lo & 0x4000) != 0; } }
        //public int ColorEntryLength { get { return (HasColorData) ? (((_lo & 0x2000) != 0) ? 2 : 1) : 0; } }

        //public bool HasColor(int index) { return (_lo & (0x4000 << (index * 2))) != 0; }
        //public int ColorLength(int index) { return HasColor(index) ? (((_lo & (0x2000 << (index * 2))) != 0) ? 2 : 1) : 0; }
        //public int ColorTotalLength { get { int len = 0; for (int i = 0; i < 2; )len += ColorLength(i++); return len; } }

        //public bool HasUV(int index) { return (_hi & (2 << (index * 2))) != 0; }
        //public int UVLength(int index) { return HasUV(index) ? (((_hi & (1 << (index * 2))) != 0) ? 2 : 1) : 0; }
        //public int UVTotalLength { get { int len = 0; for (int i = 0; i < 8; )len += UVLength(i++); return len; } }

        //public bool HasExtra(int index) { return (_lo & (1 << index)) != 0; }
        //public int ExtraLength { get { int len = 0; for (int i = 0; i < 8; ) if (HasExtra(i++))len++; return len; } }

        //public bool HasWeights { get { return (_lo & 0xFF) != 0; } }
        //public int WeightLength { get { return ExtraLength; } }
        //#endregion
    }

    //UVAT Group 0
    //0000 0000 0000 0000 0000 0000 0000 0001   - Pos elements
    //0000 0000 0000 0000 0000 0000 0000 1110   - Pos format
    //0000 0000 0000 0000 0000 0001 1111 0000   - Pos frac
    //0000 0000 0000 0000 0000 0010 0000 0000   - Normal elements
    //0000 0000 0000 0000 0001 1100 0000 0000   - Normal format
    //0000 0000 0000 0000 0010 0000 0000 0000   - Color0 elements
    //0000 0000 0000 0001 1100 0000 0000 0000   - Color0 Comp
    //0000 0000 0000 0010 0000 0000 0000 0000   - Color1 elements
    //0000 0000 0001 1100 0000 0000 0000 0000   - Color1 Comp
    //0000 0000 0010 0000 0000 0000 0000 0000   - Tex0 coord elements
    //0000 0001 1100 0000 0000 0000 0000 0000   - Tex0 coord format
    //0011 1110 0000 0000 0000 0000 0000 0000   - Tex0 coord frac
    //0100 0000 0000 0000 0000 0000 0000 0000   - Byte dequant
    //1000 0000 0000 0000 0000 0000 0000 0000   - Normal index 3

    //UVAT Group 1
    //0000 0000 0000 0000 0000 0000 0000 0001   - Tex1 coord elements
    //0000 0000 0000 0000 0000 0000 0000 1110   - Tex1 coord format
    //0000 0000 0000 0000 0000 0001 1111 0000   - Tex1 coord frac
    //0000 0000 0000 0000 0000 0010 0000 0000   - Tex2 coord elements
    //0000 0000 0000 0000 0001 1100 0000 0000   - Tex2 coord format
    //0000 0000 0000 0011 1110 0000 0000 0000   - Tex2 coord frac
    //0000 0000 0000 0100 0000 0000 0000 0000   - Tex3 coord elements
    //0000 0000 0011 1000 0000 0000 0000 0000   - Tex3 coord format
    //0000 0111 1100 0000 0000 0000 0000 0000   - Tex3 coord frac
    //0000 1000 0000 0000 0000 0000 0000 0000   - Tex4 coord elements
    //0111 0000 0000 0000 0000 0000 0000 0000   - Tex4 coord format

    //UVAT Group 2
    //0000 0000 0000 0000 0000 0000 0001 1111   - Tex4 coord frac
    //0000 0000 0000 0000 0000 0000 0010 0000   - Tex5 coord elements
    //0000 0000 0000 0000 0000 0001 1100 0000   - Tex5 coord format
    //0000 0000 0000 0000 0011 1110 0000 0000   - Tex5 coord frac
    //0000 0000 0000 0000 0100 0000 0000 0000   - Tex6 coord elements
    //0000 0000 0000 0011 1000 0000 0000 0000   - Tex6 coord format
    //0000 0000 0111 1100 0000 0000 0000 0000   - Tex6 coord frac
    //0000 0000 1000 0000 0000 0000 0000 0000   - Tex7 coord elements
    //0000 0111 0000 0000 0000 0000 0000 0000   - Tex7 coord format
    //1111 1000 0000 0000 0000 0000 0000 0000   - Tex7 coord frac

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CPElementSpec
    {
        internal uint _grp0;
        internal ulong _grp12;

        public bool ByteDequant
        {
            get => (_grp0 & 0x40000000) != 0;
            set => _grp0 = (_grp0 & 0xBFFFFFFF) | (uint) (value ? 0x40000000 : 0);
        }

        public bool NormalIndex3
        {
            get => (_grp0 & 0x80000000) != 0;
            set => _grp0 = (_grp0 & 0x7FFFFFFF) | (value ? 0x80000000 : 0);
        }

        public CPElementDef PositionDef
        {
            get => new CPElementDef(_grp0 & 0x1FF);
            set => _grp0 = (_grp0 & 0xFFFFFE00) | (value._data & 0x1FF);
        }

        public CPElementDef NormalDef
        {
            get => new CPElementDef((_grp0 >> 9) & 0xF);
            set => _grp0 = (_grp0 & 0xFFFFE1FF) | ((value._data & 0xF) << 9);
        }

        public CPElementSpec(uint grp0, uint grp1, uint grp2)
        {
            _grp0 = grp0;
            _grp12 = ((ulong) grp2 << 31) | grp1;
        }

        public CPElementDef GetColorDef(int index)
        {
            return new CPElementDef((_grp0 >> (index * 4 + 13)) & 0xF);
        }

        public void SetColorDef(int index, CPElementDef def)
        {
            _grp0 = (_grp0 & ~((uint) 0xF << (index * 4 + 13))) | ((def._data & 0xF) << (index * 4 + 13));
        }

        public CPElementDef GetUVDef(int index)
        {
            if (index == 0)
            {
                return new CPElementDef((_grp0 >> 21) & 0x1FF);
            }

            return new CPElementDef((uint) ((_grp12 >> (--index * 9)) & 0x1FF));
        }

        public void SetUVDef(int index, CPElementDef def)
        {
            if (index == 0)
            {
                _grp0 = (_grp0 & 0xC01FFFFF) | ((def._data & 0x1FF) << 21);
            }
            else
            {
                _grp12 = (_grp12 & ~((ulong) 0x1FF << (--index * 9))) | (((ulong) def._data & 0x1FF) << (index * 9));
            }
        }
    }

    public struct CPElementDef
    {
        internal uint _data;

        public bool IsSpecial
        {
            get => (_data & 1) != 0;
            set => _data = (_data & 0xFFFFFFFE) | (uint) (value ? 1 : 0);
        }

        public int Scale
        {
            get => (int) ((_data >> 4) & 0x1F);
            set => _data = (_data & 0xFFFFFE0F) | (uint) (value << 4);
        }

        public WiiVertexComponentType DataFormat
        {
            get => (WiiVertexComponentType) ((_data >> 1) & 7);
            set => _data = (_data & 0xFFFFFFF1) | ((uint) value << 1);
        }

        public WiiColorComponentType ColorFormat
        {
            get => (WiiColorComponentType) DataFormat;
            set => DataFormat = (WiiVertexComponentType) value;
        }

        public CPElementDef(uint raw)
        {
            _data = raw;
        }

        public CPElementDef(bool isSpecial, int format, int scale)
        {
            _data = (uint) (((scale & 0x1F) << 4) | ((format & 0x7) << 1) | (isSpecial ? 1 : 0));
        }

        public string asColor()
        {
            return
                $"IsSpecial: {(IsSpecial ? "True" : "False")} | Scale: {Scale.ToString()} | Color Format: {ColorFormat}";
        }

        public override string ToString()
        {
            return
                $"IsSpecial: {(IsSpecial ? "True" : "False")} | Scale: {Scale.ToString()} | Data Format: {DataFormat}";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexAttributeFormat
    {
        public VertexAttributeFormat(GXAttribute attr, GXCompType type, GXCompCnt cnt, byte frac)
        {
            _attr = attr;
            _cnt = cnt;
            _type = type;
            _frac = frac;
        }

        public GXAttribute _attr;
        public GXCompCnt _cnt;
        public GXCompType _type;
        public byte _frac; //scale/divisor
    }

    public enum GXVtxFmt
    {
        GX_VTXFMT0 = 0,
        GX_VTXFMT1,
        GX_VTXFMT2,
        GX_VTXFMT3,
        GX_VTXFMT4,
        GX_VTXFMT5,
        GX_VTXFMT6,
        GX_VTXFMT7,
        GX_MAX_VTXFMT
    }

    public enum GXCompCnt
    {
        PosXY = 0,
        PosXYZ = 1,
        NrmXYZ = 0,
        NrmNBT = 1,  // one index per NBT
        NrmNBT3 = 2, // one index per each of N/B/T
        ClrRGB = 0,
        ClrRGBA = 1,
        TexS = 0,
        TexST = 1
    }

    public enum GXCompType
    {
        UInt8 = 0,
        Int8 = 1,
        UInt16 = 2,
        Int16 = 3,
        Float = 4,

        RGB565 = 0,
        RGB8 = 1,
        RGBX8 = 2,
        RGBA4 = 3,
        RGBA6 = 4,
        RGBA8 = 5
    }
}