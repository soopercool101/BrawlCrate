using BrawlLib.Internal;
using BrawlLib.Modeling;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Models
{
    public unsafe delegate void ElementDecoder(ref byte* pIn, ref byte* pOut, float scale);

    public unsafe class ElementCodec
    {
        public enum CodecType : int
        {
            S = 0,
            ST = 5,
            XY = 10,
            XYZ = 15
        }

        #region Decoders

        public static ElementDecoder[] Decoders = new ElementDecoder[]
        {
            //Element_Input_Output
            Element_Byte_Float2, //S
            Element_SByte_Float2,
            Element_wUShort_Float2,
            Element_wShort_Float2,
            Element_wFloat_Float2,
            Element_Byte2_Float2, //ST
            Element_SByte2_Float2,
            Element_wUShort2_Float2,
            Element_wShort2_Float2,
            Element_wFloat2_Float2,
            Element_Byte2_Float3, //XY
            Element_SByte2_Float3,
            Element_wUShort2_Float3,
            Element_wShort2_Float3,
            Element_wFloat2_Float3,
            Element_Byte3_Float3, //XYZ
            Element_SByte3_Float3,
            Element_wUShort3_Float3,
            Element_wShort3_Float3,
            Element_wFloat3_Float3
        };

        public static void Element_Byte_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*) pOut)[0] = *pIn++ * scale;
            ((float*) pOut)[1] = 0.0f;
            pOut += 8;
        }

        public static void Element_SByte_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*) pOut)[0] = *(sbyte*) pIn++ * scale;
            ((float*) pOut)[1] = 0.0f;
            pOut += 8;
        }

        public static void Element_wUShort_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*) pOut)[0] = (ushort) ((*pIn++ << 8) | *pIn++) * scale;
            ((float*) pOut)[1] = 0.0f;
            pOut += 8;
        }

        public static void Element_wShort_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*) pOut)[0] = (short) ((*pIn++ << 8) | *pIn++) * scale;
            ((float*) pOut)[1] = 0.0f;
            pOut += 8;
        }

        public static void Element_wFloat_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            float val;
            byte* p = (byte*) &val;
            p[3] = *pIn++;
            p[2] = *pIn++;
            p[1] = *pIn++;
            p[0] = *pIn++;

            ((float*) pOut)[0] = val * scale;
            ((float*) pOut)[1] = 0.0f;
            pOut += 8;
        }

        public static void Element_Byte2_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*) pOut)[0] = *pIn++ * scale;
            ((float*) pOut)[1] = *pIn++ * scale;
            pOut += 8;
        }

        public static void Element_SByte2_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*) pOut)[0] = *(sbyte*) pIn++ * scale;
            ((float*) pOut)[1] = *(sbyte*) pIn++ * scale;
            pOut += 8;
        }

        public static void Element_wUShort2_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*) pOut)[0] = (ushort) ((*pIn++ << 8) | *pIn++) * scale;
            ((float*) pOut)[1] = (ushort) ((*pIn++ << 8) | *pIn++) * scale;
            pOut += 8;
        }

        public static void Element_wShort2_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*) pOut)[0] = (short) ((*pIn++ << 8) | *pIn++) * scale;
            ((float*) pOut)[1] = (short) ((*pIn++ << 8) | *pIn++) * scale;
            pOut += 8;
        }

        public static void Element_wFloat2_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            float val;
            byte* p = (byte*) &val;

            for (int i = 0; i < 2; i++)
            {
                p[3] = *pIn++;
                p[2] = *pIn++;
                p[1] = *pIn++;
                p[0] = *pIn++;
                ((float*) pOut)[i] = val * scale;
            }

            pOut += 8;
        }

        public static void Element_wShort2_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            float* f = (float*) pOut;

            *f++ = (short) ((*pIn++ << 8) | *pIn++) * scale;
            *f++ = (short) ((*pIn++ << 8) | *pIn++) * scale;
            *f = 0.0f;

            pOut += 12;
        }

        public static void Element_wShort3_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            short temp;
            byte* p = (byte*) &temp;
            for (int i = 0; i < 3; i++)
            {
                p[1] = *pIn++;
                p[0] = *pIn++;
                *(float*) pOut = temp * scale;
                pOut += 4;
            }
        }

        public static void Element_wUShort2_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            ushort temp;
            byte* p = (byte*) &temp;
            for (int i = 0; i < 3; i++)
            {
                if (i == 2)
                {
                    *(float*) pOut = 0.0f;
                }
                else
                {
                    p[1] = *pIn++;
                    p[0] = *pIn++;
                    *(float*) pOut = temp * scale;
                }

                pOut += 4;
            }
        }

        public static void Element_wUShort3_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            ushort temp;
            byte* p = (byte*) &temp;
            for (int i = 0; i < 3; i++)
            {
                p[1] = *pIn++;
                p[0] = *pIn++;
                *(float*) pOut = temp * scale;
                pOut += 4;
            }
        }

        public static void Element_Byte2_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            for (int i = 0; i < 3; i++)
            {
                *(float*) pOut = i == 2 ? 0.0f : *pIn++ * scale;
                pOut += 4;
            }
        }

        public static void Element_Byte3_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            for (int i = 0; i < 3; i++)
            {
                *(float*) pOut = *pIn++ * scale;
                pOut += 4;
            }
        }

        public static void Element_SByte2_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            for (int i = 0; i < 3; i++)
            {
                *(float*) pOut = i == 2 ? 0.0f : *(sbyte*) pIn++ * scale;
                pOut += 4;
            }
        }

        public static void Element_SByte3_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            for (int i = 0; i < 3; i++)
            {
                *(float*) pOut = *(sbyte*) pIn++ * scale;
                pOut += 4;
            }
        }

        public static void Element_wFloat2_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            float temp;
            byte* p = (byte*) &temp;
            for (int i = 0; i < 3; i++)
            {
                if (i == 2)
                {
                    *(float*) pOut = 0.0f;
                }
                else
                {
                    p[3] = *pIn++;
                    p[2] = *pIn++;
                    p[1] = *pIn++;
                    p[0] = *pIn++;
                    *(float*) pOut = temp;
                }

                pOut += 4;
            }
        }

        public static void Element_wFloat3_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            float val;
            byte* p = (byte*) &val;
            for (int i = 0; i < 3; i++)
            {
                p[3] = *pIn++;
                p[2] = *pIn++;
                p[1] = *pIn++;
                p[0] = *pIn++;
                *(float*) pOut = val;
                pOut += 4;
            }
        }

        #endregion
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe class ElementDescriptor
    {
        public int Stride;
        public bool Weighted;
        public bool[] HasData;
        public byte[] Commands;
        public int[] Defs;
        private readonly ushort[] Nodes;
        public List<int> RemapTable;
        public List<List<Facepoint>> _points;

        public ElementDescriptor()
        {
            RemapTable = new List<int>();
            Stride = 0;
            HasData = new bool[12];
            Nodes = new ushort[16];
            Commands = new byte[31];
            Defs = new int[12];
            Weighted = false;
            _points = new List<List<Facepoint>>();
        }

        //public void SetupBMD(BMDObjectAttrib* attrib)
        //{
        //    byte* pCom;
        //    ElementDef* pDef;

        //    WiiVertexComponentType format;
        //    fixed (int* pDefData = Defs)
        //    fixed (byte* pComData = Commands)
        //    {
        //        pCom = pComData;
        //        pDef = (ElementDef*)pDefData;

        //        while (attrib->ArrayType != GXAttribute.Null)
        //        {
        //            format = attrib->DataFormat;
        //            switch (attrib->ArrayType)
        //            {
        //                case GXAttribute.PosNrmMtxId:
        //                    Weighted = true;
        //                    *pCom++ = (byte)DecodeOp.PosWeight;
        //                    Stride++;
        //                    break;
        //                case GXAttribute.Tex0MtxId:
        //                case GXAttribute.Tex1MtxId:
        //                case GXAttribute.Tex2MtxId:
        //                case GXAttribute.Tex3MtxId:
        //                case GXAttribute.Tex4MtxId:
        //                case GXAttribute.Tex5MtxId:
        //                case GXAttribute.Tex6MtxId:
        //                case GXAttribute.Tex7MtxId:
        //                    *pCom++ = (byte)(DecodeOp.TexMtx0 + (int)(attrib->ArrayType - GXAttribute.Tex0MtxId));
        //                    Stride++;
        //                    break;
        //                case GXAttribute.Position:
        //                    HasData[0] = true;
        //                    pDef->Type = 0;
        //                    Stride += (pDef->Format = (byte)((int)format < 2 ? 1 : 2));
        //                    pDef->Output = 12;
        //                    *pCom++ = (byte)DecodeOp.ElementIndexed;
        //                    pDef++;
        //                    break;
        //                case GXAttribute.Normal:
        //                    HasData[1] = true;
        //                    pDef->Type = 1;
        //                    Stride += (pDef->Format = (byte)((int)format < 2 ? 1 : 2));
        //                    pDef->Output = 12;
        //                    *pCom++ = (byte)DecodeOp.ElementIndexed;
        //                    pDef++;
        //                    break;
        //                case GXAttribute.Color0:
        //                case GXAttribute.Color1:
        //                    int cIndex = (int)(attrib->ArrayType - GXAttribute.Color0);
        //                    HasData[cIndex + 2] = true;
        //                    pDef->Type = (byte)(cIndex + 2);
        //                    Stride += (pDef->Format = (byte)((int)format < 2 ? 1 : 2));
        //                    pDef->Output = 4;
        //                    *pCom++ = (byte)DecodeOp.ElementIndexed;
        //                    pDef++;
        //                    break;
        //                case GXAttribute.Tex0:
        //                case GXAttribute.Tex1:
        //                case GXAttribute.Tex2:
        //                case GXAttribute.Tex3:
        //                case GXAttribute.Tex4:
        //                case GXAttribute.Tex5:
        //                case GXAttribute.Tex6:
        //                case GXAttribute.Tex7:
        //                    int uIndex = (int)(attrib->ArrayType - GXAttribute.Tex0);
        //                    HasData[uIndex + 4] = true;
        //                    pDef->Type = (byte)(uIndex + 4);
        //                    Stride += (pDef->Format = (byte)((int)format < 2 ? 1 : 2));
        //                    pDef->Output = 8;
        //                    *pCom++ = (byte)DecodeOp.ElementIndexed;
        //                    pDef++;
        //                    break;
        //            }
        //            attrib++;
        //        }
        //        *pCom = 0;
        //    }
        //}

        public void SetupMDL0(MDL0Object* polygon)
        {
            byte* pCom;
            ElementDef* pDef;

            CPElementSpec UVATGroups;
            int format; //0 for direct, 1 for byte, 2 for short

            //Read element descriptor from polygon display list
            MDL0PolygonDefs* Definitons = polygon->DefList;

            int fmtLo = (int) Definitons->VtxFmtLo;
            int fmtHi = (int) Definitons->VtxFmtHi;

            UVATGroups = new CPElementSpec(
                Definitons->UVATA,
                Definitons->UVATB,
                Definitons->UVATC);

            //Build extract script.
            //What we're doing is assigning extract commands for elements in the polygon, in true order.
            //This allows us to process the polygon blindly, assuming that the definition is accurate.
            //Theoretically, this should offer a significant speed bonus.
            fixed (int* pDefData = Defs)
            {
                fixed (byte* pComData = Commands)
                {
                    pCom = pComData;
                    pDef = (ElementDef*) pDefData;

                    //Pos/Norm weight
                    if (Weighted = (fmtLo & 1) != 0)
                    {
                        //Set the first command as the weight
                        *pCom++ = (byte) DecodeOp.PosWeight;
                        Stride++; //Increment stride by a byte (the length of the facepoints)
                    }

                    //Tex matrix
                    for (int i = 0; i < 8; i++)
                    {
                        if (((fmtLo >> (i + 1)) & 1) != 0)
                        {
                            //Set the command for each texture matrix
                            *pCom++ = (byte) (DecodeOp.TexMtx0 + i);
                            Stride++; //Increment stride by a byte (the length of the facepoints)
                        }
                    }

                    //Positions
                    format = ((fmtLo >> 9) & 3) - 1;
                    if (format >= 0)
                    {
                        HasData[0] = true;

                        //Set the definitions input
                        pDef->Format = (byte) format;
                        //Set the type to Positions
                        pDef->Type = 0;
                        if (format == 0)
                        {
                            int f = (int) UVATGroups.PositionDef.DataFormat;

                            //Clamp format to even value and add length to stride
                            Stride += f.RoundDownToEven().Clamp(1, 4) * (!UVATGroups.PositionDef.IsSpecial ? 2 : 3);

                            pDef->Scale = (byte) UVATGroups.PositionDef.Scale;
                            pDef->Output =
                                (byte) ((!UVATGroups.PositionDef.IsSpecial
                                            ? (int) ElementCodec.CodecType.XY
                                            : (int) ElementCodec.CodecType.XYZ) +
                                        (byte) UVATGroups.PositionDef.DataFormat);
                            *pCom++ = (byte) DecodeOp.ElementDirect;
                        }
                        else
                        {
                            Stride += format;  //Add to stride (the length of the facepoints)
                            pDef->Output = 12; //Set the output
                            *pCom++ = (byte) DecodeOp.ElementIndexed;
                        }

                        pDef++;
                    }

                    //Normals
                    format = ((fmtLo >> 11) & 3) - 1;
                    if (format >= 0)
                    {
                        HasData[1] = true;

                        //Set the definitions input
                        pDef->Format = (byte) format;
                        //Set the type to Normals
                        pDef->Type = 1;
                        if (format == 0)
                        {
                            int f = (int) UVATGroups.NormalDef.DataFormat;
                            Stride += f.RoundDownToEven().Clamp(1, 4) * 3;

                            pDef->Scale = (byte) UVATGroups.NormalDef.Scale;
                            pDef->Output =
                                (byte) ((int) ElementCodec.CodecType.XYZ + (byte) UVATGroups.NormalDef.DataFormat);
                            *pCom++ = (byte) DecodeOp.ElementDirect;
                        }
                        else
                        {
                            Stride += format;  //Add to stride (the length of the facepoints)
                            pDef->Output = 12; //Set the output
                            *pCom++ = (byte) DecodeOp.ElementIndexed;
                        }

                        pDef++;
                    }

                    //Colors
                    for (int i = 0; i < 2; i++)
                    {
                        format = ((fmtLo >> (i * 2 + 13)) & 3) - 1;
                        if (format >= 0)
                        {
                            HasData[i + 2] = true;

                            //Set the definitions input
                            pDef->Format = (byte) format;
                            //Set the type to Colors
                            pDef->Type = (byte) (i + 2);
                            if (format == 0)
                            {
                                //pDef->Output = 
                                pDef->Scale = 0;
                                *pCom++ = (byte) DecodeOp.ElementDirect;
                            }
                            else
                            {
                                Stride += format; //Add to stride (the length of the facepoints)
                                pDef->Output = 4; //Set the output
                                *pCom++ = (byte) DecodeOp.ElementIndexed;
                            }

                            pDef++;
                        }
                    }

                    //UVs
                    for (int i = 0; i < 8; i++)
                    {
                        format = ((fmtHi >> (i * 2)) & 3) - 1;
                        if (format >= 0)
                        {
                            HasData[i + 4] = true;

                            //Set the definitions input
                            pDef->Format = (byte) format;
                            //Set the type to UVs
                            pDef->Type = (byte) (i + 4);
                            if (format == 0)
                            {
                                int f = (int) UVATGroups.GetUVDef(i).DataFormat;
                                Stride += f.RoundDownToEven().Clamp(1, 4);

                                pDef->Output =
                                    (byte) ((!UVATGroups.GetUVDef(i).IsSpecial
                                                ? (int) ElementCodec.CodecType.S
                                                : (int) ElementCodec.CodecType.ST) +
                                            (byte) UVATGroups.GetUVDef(i).DataFormat);
                                pDef->Scale = (byte) UVATGroups.GetUVDef(i).Scale;
                                *pCom++ = (byte) DecodeOp.ElementDirect;
                            }
                            else
                            {
                                Stride += format; //Add to stride (the length of the facepoints)
                                pDef->Output = 8; //Set the output
                                *pCom++ = (byte) DecodeOp.ElementIndexed;
                            }

                            pDef++;
                        }
                    }

                    *pCom = 0;
                }
            }
        }

        //Set node ID/Index using specified command block
        public void SetNode(byte* pIn)
        {
            //Get node ID
            ushort node = *(bushort*) pIn;

            //Get cache index.
            //Wii memory assigns data using offsets of 4-byte values.
            //In this case, each matrix takes up 12 floats (4 bytes each)

            //Divide by 12, the number of float values per 4x3 matrix, to get the actual index
            int index = (*(bushort*) (pIn + 2) & 0xFFF) / 12;
            //Assign node ID to cache, using index
            fixed (ushort* n = Nodes)
            {
                n[index] = node;
            }
        }

        public void SetNode(ushort index, ushort node)
        {
            fixed (ushort* n = Nodes)
            {
                n[index] = node;
            }
        }

        //Decode a single primitive using command list
        public void Run(
            ref byte* pIn,
            byte** pAssets,
            byte** pOut,
            int count,
            PrimitiveGroup group,
            ref ushort* indices,
            IMatrixNode[] nodeTable,
            bool[] textureMatrixIdentity)
        {
            //pIn is the address in the primitives
            //pOut is address of the face data buffers
            //pAssets is the address of the raw asset buffers

            int weight = 0;

            int index = 0, outSize;
            DecodeOp o;
            ElementDef* pDef;
            byte* p;
            byte[] pTexMtx = new byte[8];

            byte* tIn, tOut;

            group._facePoints.Add(new List<Facepoint>());

            //Iterate commands in list
            fixed (ushort* pNode = Nodes)
            {
                fixed (int* pDefData = Defs)
                {
                    fixed (byte* pCmd = Commands)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            pDef = (ElementDef*) pDefData;
                            p = pCmd;

                            Facepoint f = new Facepoint();

                            Continue:
                            o = (DecodeOp) (*p++);
                            switch (o)
                            {
                                //Process weight using cache
                                case DecodeOp.PosWeight:
                                    weight = pNode[*pIn++ / 3];
                                    goto Continue;

                                case DecodeOp.TexMtx0:
                                case DecodeOp.TexMtx1:
                                case DecodeOp.TexMtx2:
                                case DecodeOp.TexMtx3:
                                case DecodeOp.TexMtx4:
                                case DecodeOp.TexMtx5:
                                case DecodeOp.TexMtx6:
                                case DecodeOp.TexMtx7:
                                    index = (int) o - (int) DecodeOp.TexMtx0;
                                    byte value = *pIn++;
                                    if (value % 3 != 0)
                                    {
                                        Console.WriteLine("Raw texture matrix value is 0x{0}", value.ToString());
                                    }
                                    else
                                    {
                                        value /= 3;
                                        if (value < 10)
                                        {
                                            textureMatrixIdentity[index] = true;
                                            //Console.WriteLine("Texture matrix value is {0}", value.ToString());
                                        }
                                        else if (value >= 20)
                                        {
                                            Console.WriteLine("Texture matrix value is {0}", value.ToString());
                                        }
                                        else
                                        {
                                            pTexMtx[index] = (byte) (value - 10);
                                            textureMatrixIdentity[index] = false;
                                        }
                                    }

                                    goto Continue;

                                case DecodeOp.ElementDirect:
                                    ElementCodec.Decoders[pDef->Output](ref pIn, ref pOut[pDef->Type],
                                        VQuant.DeQuantTable[pDef->Scale]);
                                    goto Continue;

                                case DecodeOp.ElementIndexed:

                                    //Get asset index
                                    if (pDef->Format == 2)
                                    {
                                        index = *(bushort*) pIn;
                                        pIn += 2;
                                    }
                                    else
                                    {
                                        index = *pIn++;
                                    }

                                    switch (pDef->Type)
                                    {
                                        case 0:
                                            f._vertexIndex = index;
                                            break;
                                        case 1:
                                            f._normalIndex = index;
                                            break;
                                        case 2:
                                        case 3:
                                            f._colorIndices[pDef->Type - 2] = index;
                                            break;
                                        default:
                                            f._UVIndices[pDef->Type - 4] = index;
                                            break;
                                    }

                                    if (pDef->Type == 0) //Special processing for vertices
                                    {
                                        //Match weight and index with remap table
                                        int mapEntry = (weight << 16) | index;

                                        //Find matching index, starting at end of list
                                        //Lower index until a match is found at that index or index is less than 0
                                        index = RemapTable.Count;
                                        while (--index >= 0 && RemapTable[index] != mapEntry)
                                        {
                                            ;
                                        }

                                        //No match, create new entry
                                        //Will be processed into vertices at the end!
                                        if (index < 0)
                                        {
                                            index = RemapTable.Count;
                                            RemapTable.Add(mapEntry);
                                            _points.Add(new List<Facepoint>());
                                        }

                                        //Write index
                                        *indices++ = (ushort) index;

                                        _points[index].Add(f);
                                    }
                                    else
                                    {
                                        //Copy data from buffer
                                        outSize = pDef->Output;

                                        //Input data from asset cache
                                        tIn = pAssets[pDef->Type] + index * outSize;
                                        tOut = pOut[pDef->Type];

                                        if (tIn != null && tOut != null)
                                        {
                                            //Copy data to output
                                            while (outSize-- > 0)
                                            {
                                                *tOut++ = *tIn++;
                                            }

                                            //Increment element output pointer
                                            pOut[pDef->Type] = tOut;
                                        }
                                    }

                                    pDef++;
                                    goto Continue;

                                default: break; //End
                            }

                            @group._facePoints[@group._facePoints.Count - 1].Add(f);
                        }
                    }
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ElementDef
        {
            public byte Format; //Input format
            public byte Output; //Output size/decoder
            public byte Type;
            public byte Scale;
        }

        public enum DecodeOp : int
        {
            End = 0,
            PosWeight,
            TexMtx0,
            TexMtx1,
            TexMtx2,
            TexMtx3,
            TexMtx4,
            TexMtx5,
            TexMtx6,
            TexMtx7,
            ElementDirect,
            ElementIndexed
        }

        internal List<Vertex3> Finish(Vector3* pVert, IMatrixNode[] nodeTable)
        {
            //Create vertex list from remap table
            List<Vertex3> list = new List<Vertex3>(RemapTable.Count);

            if (!Weighted)
            {
                //Add vertex to list using raw value.
                for (int i = 0; i < RemapTable.Count; i++)
                {
                    Vertex3 v = new Vertex3(pVert[RemapTable[i]]) {Facepoints = _points[i]};
                    foreach (Facepoint f in v.Facepoints)
                    {
                        f._vertex = v;
                    }

                    list.Add(v);
                }
            }
            else if (nodeTable != null)
            {
                for (int i = 0; i < RemapTable.Count; i++)
                {
                    int x = RemapTable[i];
                    //Create new vertex, assigning the value + influence from the remap table
                    int node = (x >> 16) & 0xFFFF;
                    IMatrixNode mtx = null;
                    if (node < nodeTable.Length && node >= 0)
                    {
                        mtx = nodeTable[node];
                    }

                    Vertex3 v = new Vertex3(pVert[x & 0xFFFF], mtx) {Facepoints = _points[i]};
                    foreach (Facepoint f in v.Facepoints)
                    {
                        f._vertex = v;
                    }

                    //Add vertex to list
                    list.Add(v);
                }
            }

            return list;
        }
    }
}