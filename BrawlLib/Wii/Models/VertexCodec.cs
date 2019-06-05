using System;
using System.Runtime.InteropServices;
using BrawlLib.SSBBTypes;

namespace BrawlLib.Wii.Models
{
    public unsafe class VertexCodec : IDisposable
    {
        private const float _maxError = 0.0005f;

        private static readonly VertEncoder _byteEncoder = (float value, ref byte* pOut) =>
        {
            //int val = (int)(value + (value > 0 ? 0.5f : -0.5f));
            *pOut = (byte) value;
            pOut++;
        };

        private static readonly VertEncoder _shortEncoder = (float value, ref byte* pOut) =>
        {
            //int val = (int)(value + (value > 0 ? 0.5f : -0.5f));
            *(bushort*) pOut = (ushort) value;
            pOut += 2;
        };

        private static readonly VertEncoder _floatEncoder = (float value, ref byte* pOut) =>
        {
            *(bfloat*) pOut = value;
            pOut += 4;
        };

        private readonly bool _forceFloat;
        public int _dataLen;
        public int _dstElements, _dstCount, _dstStride;
        private VertEncoder _enc;

        private GCHandle _handle;
        public bool _hasZ;
        public Vector3 _min, _max;
        private float* _pData;
        private float _quantScale;

        private Remapper _remap;

        public int _scale;

        public int _srcElements, _srcCount;
        public WiiVertexComponentType _type;

        public Vector2* Address;

        private VertexCodec()
        {
        }

        public VertexCodec(Vector3[] vertices, bool removeZ, bool forceFloat)
        {
            _forceFloat = forceFloat;
            _srcCount = vertices.Length;
            _srcElements = 3;
            _handle = GCHandle.Alloc(vertices, GCHandleType.Pinned);
            _pData = (float*) _handle.AddrOfPinnedObject();
            Evaluate(removeZ);
        }

        public VertexCodec(Vector3[] vertices, bool removeZ)
        {
            _srcCount = vertices.Length;
            _srcElements = 3;
            _handle = GCHandle.Alloc(vertices, GCHandleType.Pinned);
            _pData = (float*) _handle.AddrOfPinnedObject();
            Evaluate(removeZ);
        }

        public VertexCodec(Vector2[] vertices, bool forceFloat)
        {
            _forceFloat = forceFloat;
            _srcCount = vertices.Length;
            _srcElements = 2;
            _handle = GCHandle.Alloc(vertices, GCHandleType.Pinned);
            _pData = (float*) _handle.AddrOfPinnedObject();
            Evaluate(false);
        }

        public VertexCodec(Vector2[] vertices)
        {
            _srcCount = vertices.Length;
            _srcElements = 2;
            _handle = GCHandle.Alloc(vertices, GCHandleType.Pinned);
            _pData = (float*) _handle.AddrOfPinnedObject();
            Evaluate(false);
        }

        public VertexCodec(Vector3* sPtr, int count, bool removeZ)
        {
            _srcCount = count;
            _srcElements = 3;
            _pData = (float*) sPtr;
            Evaluate(removeZ);
        }

        public VertexCodec(Vector2* sPtr, int count)
        {
            _srcCount = count;
            _srcElements = 2;
            _pData = (float*) sPtr;
            Evaluate(false);
        }

        public void Dispose()
        {
            if (_handle.IsAllocated) _handle.Free();

            _pData = null;
        }

        ~VertexCodec()
        {
            Dispose();
        }

        private void Evaluate(bool removeZ)
        {
            float* fPtr;
            var bestScale = 0;
            bool sign;

            Vector3 min = new Vector3(float.MaxValue), max = new Vector3(float.MinValue);
            float* pMin = (float*) &min, pMax = (float*) &max;
            float vMin = float.MaxValue, vMax = float.MinValue, vDist;
            float val;

            //Currently, remapping provides to benefit for vertices/normals
            //Maybe when merging is supported this will change

            _remap = new Remapper();
            //if (_srcElements == 3)
            //    _remap.Remap<Vector3>(new MemoryList<Vector3>(_pData, _srcCount), null);
            //if (_srcElements == 2)
            //    _remap.Remap<Vector2>(new MemoryList<Vector2>(_pData, _srcCount), null);

            int[] imp = null;
            var impLen = _srcCount;

            //Remapping is useless if there is no savings
            if (_remap._impTable != null && _remap._impTable.Length < _srcCount)
            {
                imp = _remap._impTable;
                impLen = imp.Length;
            }

            _min = new Vector3(float.MaxValue);
            _max = new Vector3(float.MinValue);
            _dstCount = impLen;
            _type = 0;

            //Get extents
            fPtr = _pData;
            for (var i = 0; i < impLen; i++)
            {
                if (imp != null) fPtr = &_pData[imp[i] * _srcElements];

                for (var x = 0; x < _srcElements; x++)
                {
                    val = *fPtr++;
                    if (val < pMin[x]) pMin[x] = val;

                    if (val > pMax[x]) pMax[x] = val;

                    if (val < vMin) vMin = val;

                    if (val > vMax) vMax = val;
                }
            }

            _min = min;
            _max = max;
            if (removeZ && _srcElements == 3 && min._z == 0 && max._z == 0)
                _dstElements = 2;
            else
                _dstElements = _srcElements;

            _hasZ = _dstElements > 2;

            vDist = Math.Max(Math.Abs(vMin), Math.Abs(vMax));

            if (!_forceFloat)
            {
                //Is signed? If so, increase type
                if (sign = vMin < 0) _type++;

                var divisor = 0;
                float rMin = 0.0f, rMax;
                for (var i = 0; i < 2; i++)
                {
                    var bestError = _maxError;
                    float scale, maxVal;

                    if (i == 0)
                    {
                        if (sign)
                        {
                            rMax = 127.0f;
                            rMin = -128.0f;
                        }
                        else
                        {
                            rMax = 255.0f;
                        }
                    }
                    else if (sign)
                    {
                        rMax = 32767.0f;
                        rMin = -32768.0f;
                    }
                    else
                    {
                        rMax = 65535.0f;
                    }

                    maxVal = rMax / vDist;
                    while (divisor < 32 && (scale = VQuant.QuantTable[divisor]) <= maxVal)
                    {
                        var worstError = float.MinValue;

                        fPtr = _pData;
                        for (var y = 0; y < impLen; y++)
                        {
                            if (imp != null) fPtr = &_pData[imp[i] * _srcElements];

                            for (var z = 0; z < _srcElements; z++)
                            {
                                if ((val = *fPtr++) == 0) continue;

                                val *= scale;
                                if (val > rMax)
                                    val = rMax;
                                else if (val < rMin) val = rMin;

                                var step = (int) (val * scale);
                                //int step = (int)((val * scale) + (val > 0 ? 0.5f : -0.5f));
                                var error = Math.Abs(step / scale - val);

                                if (error > worstError) worstError = error;

                                if (error > bestError) goto Check;
                            }
                        }

                        //for (fPtr = sPtr; fPtr < fCeil; )
                        //{
                        //    if ((val = *fPtr++) == 0)
                        //        continue;

                        //    val *= scale;
                        //    if (val > rMax) val = rMax;
                        //    else if (val < rMin) val = rMin;

                        //    int step = (int)((val * scale);// + (val > 0 ? 0.5f : -0.5f));
                        //    float error = Math.Abs((step / scale) - val);

                        //    if (error > worstError)
                        //        worstError = error;

                        //    if (error > bestError)
                        //        break;
                        //}

                        Check:

                        if (worstError < bestError)
                        {
                            bestScale = divisor;
                            bestError = worstError;
                            if (bestError == 0) goto Next;
                        }

                        divisor++;
                    }

                    if (bestError < _maxError) goto Next;

                    _type += 2;
                }
            }

            _type = WiiVertexComponentType.Float;
            _scale = 0;

            Next:

            _scale = bestScale;
            _dstStride = _dstElements << ((int) _type >> 1);
            _dataLen = _dstCount * _dstStride;

            _quantScale = VQuant.QuantTable[_scale];
            GetEncoder();
        }

        public void GetEncoder()
        {
            switch (_type)
            {
                case WiiVertexComponentType.Int8:
                case WiiVertexComponentType.UInt8:
                    _enc = _byteEncoder;
                    break;
                case WiiVertexComponentType.Int16:
                case WiiVertexComponentType.UInt16:
                    _enc = _shortEncoder;
                    break;
                default:
                    _enc = _floatEncoder;
                    break;
            }
        }

        public void Write(byte* pOut)
        {
            try
            {
                var imp = _remap._impTable;

                //Copy elements using encoder
                var pTemp = _pData;
                for (var i = 0; i < _dstCount; i++)
                {
                    if (imp != null) pTemp = &_pData[imp[i] * _srcElements];

                    for (var x = 0; x < _srcElements; x++, pTemp++)
                        if (x < _dstElements)
                            _enc(*pTemp * _quantScale, ref pOut);
                }

                //Zero remaining
                for (var i = _dataLen; (i & 0x1F) != 0; i++) *pOut++ = 0;
            }
            finally
            {
                Dispose();
            }
        }

        public void Write(ref byte* pOut, int index)
        {
            var imp = _remap._impTable;

            //Copy element using encoder
            var pTemp = _pData;
            if (imp != null) pTemp = &_pData[imp[index] * _srcElements];

            for (var x = 0; x < _srcElements; x++, pTemp++)
                if (x < _dstElements)
                    _enc(*pTemp * _quantScale, ref pOut);
        }

        public void Write(Vector2[] vertices, byte* pOut)
        {
            fixed (Vector2* p = vertices)
            {
                Write((float*) p, pOut);
            }
        }

        public void Write(Vector3[] vertices, byte* pOut)
        {
            fixed (Vector3* p = vertices)
            {
                Write((float*) p, pOut);
            }
        }

        public void Write(float* pIn, byte* pOut)
        {
            var imp = _remap._impTable;
            var pCeil = pOut + _dataLen.Align(0x20);

            //Copy elements using encoder
            var pTemp = pIn;
            for (var i = 0; i < _dstCount; i++)
            {
                if (imp != null) pTemp = &pIn[imp[i] * _srcElements];

                for (var x = 0; x < _srcElements; x++, pTemp++)
                    if (x < _dstElements)
                        _enc(*pTemp * _quantScale, ref pOut);
            }

            //Zero remaining
            while (pOut < pCeil) *pOut++ = 0;
        }

        private delegate void VertEncoder(float value, ref byte* pOut);

        #region Decoding

        public static UnsafeBuffer Decode(MDL0UVData* header)
        {
            int count = header->_numEntries;
            var scale = VQuant.DeQuantTable[header->_divisor];
            var type = (header->_isST == 0 ? (int) ElementCodec.CodecType.S : (int) ElementCodec.CodecType.ST) +
                       header->_format;
            var decoder = ElementCodec.Decoders[type];
            var buffer = new UnsafeBuffer(count * 8);

            byte* pIn = (byte*) header->Entries, pOut = (byte*) buffer.Address;
            for (var i = 0; i < count; i++) decoder(ref pIn, ref pOut, scale);

            return buffer;
        }

        public static UnsafeBuffer Decode(VoidPtr data, byte divisor, uint length, WiiVertexComponentType componentType,
            bool UVs, int isSpecial)
        {
            var scale = VQuant.DeQuantTable[divisor];
            var type = isSpecial * 5 + (UVs ? 0 : 10) + (int) componentType;
            var decoder = ElementCodec.Decoders[type];

            var bytesPerVal = (UVs ? isSpecial + 1 : isSpecial + 2) * (componentType < WiiVertexComponentType.UInt16
                                  ?
                                  1
                                  : componentType < WiiVertexComponentType.Float
                                      ? 2
                                      : 4);

            var count = (int) (length / bytesPerVal);

            var buffer = new UnsafeBuffer(count * (UVs ? 8 : 12));

            byte* pIn = (byte*) data, pOut = (byte*) buffer.Address;
            for (var i = 0; i < count; i++) decoder(ref pIn, ref pOut, scale);

            return buffer;
        }

        public static UnsafeBuffer Decode(MDL0VertexData* header)
        {
            int count = header->_numVertices;
            var scale = VQuant.DeQuantTable[header->_divisor];
            var type = (header->_isXYZ == 0 ? (int) ElementCodec.CodecType.XY : (int) ElementCodec.CodecType.XYZ) +
                       header->_type;
            var decoder = ElementCodec.Decoders[type];
            var buffer = new UnsafeBuffer(count * 12);

            byte* pIn = (byte*) header->Data, pOut = (byte*) buffer.Address;
            for (var i = 0; i < count; i++) decoder(ref pIn, ref pOut, scale);

            return buffer;
        }

        public static UnsafeBuffer Decode(MDL0NormalData* header)
        {
            int count = header->_numVertices;
            var scale = VQuant.DeQuantTable[header->_divisor]; //Should always be zero?
            var type = (int) ElementCodec.CodecType.XYZ + header->_type;
            var decoder = ElementCodec.Decoders[type];
            UnsafeBuffer buffer;

            if (header->_isNBT != 0) count *= 3; //Format is the same, just with three Vectors each

            buffer = new UnsafeBuffer(count * 12);

            byte* pIn = (byte*) header->Data, pOut = (byte*) buffer.Address;
            for (var i = 0; i < count; i++) decoder(ref pIn, ref pOut, scale);

            return buffer;
        }

        #endregion

        #region Extracting

        public static Vector3[] ExtractVertices(MDL0VertexData* vertices)
        {
            if (vertices != null)
                return ExtractVertices(vertices->Data, vertices->_numVertices, vertices->_isXYZ != 0, vertices->Type,
                    1 << vertices->_divisor);
            return null;
        }

        public static Vector3[] ExtractVertices(MDL0FurPosData* vertices)
        {
            if (vertices != null)
                return ExtractVertices(vertices->Data, vertices->_numVertices, vertices->_isXYZ != 0, vertices->Type,
                    1 << vertices->_divisor);
            return null;
        }

        public static Vector3[] ExtractNormals(MDL0NormalData* normals)
        {
            return ExtractVertices(normals->Data, normals->_numVertices, true, normals->Type, 1 << normals->_divisor);
        }

        public static Vector2[] ExtractUVs(MDL0UVData* uvs)
        {
            return ExtractPoints(uvs->Entries, uvs->_numEntries, uvs->Type, 1 << uvs->_divisor);
        }

        private static Vector2[] ExtractPoints(VoidPtr address, int count, WiiVertexComponentType type, float divisor)
        {
            var points = new Vector2[count];

            fixed (Vector2* p = points)
            {
                var dPtr = (float*) p;
                for (var i = 0; i < count; i++)
                {
                    *dPtr++ = ReadValue(ref address, type, divisor);
                    *dPtr++ = ReadValue(ref address, type, divisor);
                }
            }

            return points;
        }

        private static Vector3[] ExtractVertices(VoidPtr address, int count, bool isXYZ, WiiVertexComponentType type,
            float divisor)
        {
            var verts = new Vector3[count];
            fixed (Vector3* p = verts)
            {
                var dPtr = (float*) p;
                for (var i = 0; i < count; i++)
                {
                    *dPtr++ = ReadValue(ref address, type, divisor);
                    *dPtr++ = ReadValue(ref address, type, divisor);
                    if (isXYZ)
                        *dPtr++ = ReadValue(ref address, type, divisor);
                    else
                        *dPtr++ = 0.0f;
                }
            }

            return verts;
        }

        private static float ReadValue(ref VoidPtr addr, WiiVertexComponentType type, float divisor)
        {
            switch (type)
            {
                case WiiVertexComponentType.UInt8:
                    addr += 1;
                    return ((byte*) addr)[-1] / divisor;
                case WiiVertexComponentType.Int8:
                    addr += 1;
                    return ((sbyte*) addr)[-1] / divisor;
                case WiiVertexComponentType.UInt16:
                    addr += 2;
                    return ((bushort*) addr)[-1] / divisor;
                case WiiVertexComponentType.Int16:
                    addr += 2;
                    return ((bshort*) addr)[-1] / divisor;
                case WiiVertexComponentType.Float:
                    addr += 4;
                    return ((bfloat*) addr)[-1];
            }

            return 0.0f;
        }

        #endregion
    }
}