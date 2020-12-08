using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.Modeling.Triangle_Converter;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Graphics;
using BrawlLib.Wii.Models;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using UnsafeBuffer = BrawlLib.Internal.UnsafeBuffer;
using Vector2 = BrawlLib.Internal.Vector2;
using Vector3 = BrawlLib.Internal.Vector3;

namespace BrawlLib.Modeling
{
    public unsafe class PrimitiveManager : IDisposable
    {
        #region Variables

        public bool AssetsChanged
        {
            get
            {
                foreach (bool b in _changed)
                {
                    if (b)
                    {
                        return true;
                    }
                }

                return false;
            }
            set
            {
                for (int i = 0; i < _changed.Length; i++)
                {
                    _changed[i] = false;
                }
            }
        }

        private readonly IObject _owner;

        public List<Vertex3> _vertices;
        public UnsafeBuffer _indices;

        public int _pointCount, _faceCount, _renderStride;

        //Face Data:
        //0 is Vertices
        //1 is Normals
        //2-3 is Colors
        //4-12 is UVs
        //The primitives indices match up to these values
        public UnsafeBuffer[] _faceData = new UnsafeBuffer[12];
        public bool[] _dirty = new bool[12];
        private readonly bool[] _changed = new bool[12];

        public void SetAssetChanged(int index)
        {
            //Signal that facepoints need to be reindexed with a new asset array
            _changed[index] = true;

            //Update render buffer
            _dirty[index] = true;
        }

        //Graphics buffer is a combination of the _faceData streams
        //Vertex (Vertex3 - 12 bytes), Normal (Vertex3 - 12 bytes),
        //Color1 & Color2 (4 bytes each), UVs 0 - 7 (Vertex2 - 8 bytes each),
        //Repeat 
        public UnsafeBuffer _graphicsBuffer;

        internal GLPrimitive _triangles, _lines, _points;

        public bool HasTexMtx
        {
            get
            {
                for (int i = 0; i < 8; i++)
                {
                    if (HasTextureMatrix[i])
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool[] HasTextureMatrix = new bool[8];
        public bool[] UseIdentityTexMtx = new bool[8];

        //Set these in OnCalculateSize!
        public bool _remakePrimitives; //Otherwise, copies previous raw primitive values
        public bool _isWeighted;
        public int _primitiveSize;
        public List<FacepointAttribute> _descList;
        public List<VertexAttributeFormat> _fmtList;
        public XFArrayFlags _arrayFlags;
        public int _fpStride;
        public List<PrimitiveGroup> _primGroups = new List<PrimitiveGroup>();

        internal int[] _newClrObj = new int[2];

        public int _arrayHandle;
        public int _arrayBufferHandle;
        public int _elementArrayBufferHandle;

        /// <summary>
        /// Returns an identical copy of this manager 
        /// but as a completely new instance with reallocated buffers.
        /// </summary>
        public PrimitiveManager HardCopy()
        {
            PrimitiveManager p = new PrimitiveManager
            {
                _vertices = _vertices,
                _indices = new UnsafeBuffer(_indices.Length),
                _pointCount = _pointCount,
                _faceCount = _faceCount,
                _faceData = new UnsafeBuffer[12],
                _triangles = _triangles == null
                    ? null
                    : new GLPrimitive(_triangles._indices.Length, BeginMode.Triangles),
                _lines = _lines == null ? null : new GLPrimitive(_lines._indices.Length, BeginMode.Lines),
                _points = _points == null ? null : new GLPrimitive(_points._indices.Length, BeginMode.Points),
                _dirty = new bool[] {true, true, true, true, true, true, true, true, true, true, true, true},
                _primGroups = _primGroups,
            };
            Memory.Move(p._indices.Address, _indices.Address, (uint) _indices.Length);
            for (int i = 0; i < 12; i++)
            {
                UnsafeBuffer b = _faceData[i];
                if (b == null)
                {
                    continue;
                }

                p._faceData[i] = new UnsafeBuffer(b.Length);
                Memory.Move(p._faceData[i].Address, b.Address, (uint) b.Length);
            }

            if (p._triangles != null)
            {
                _triangles._indices.CopyTo(p._triangles._indices, 0);
            }

            if (p._lines != null)
            {
                _lines._indices.CopyTo(p._lines._indices, 0);
            }

            if (p._points != null)
            {
                _points._indices.CopyTo(p._points._indices, 0);
            }

            p.HasTextureMatrix = new bool[8];
            HasTextureMatrix.CopyTo(p.HasTextureMatrix, 0);
            return p;
        }

        #endregion

        #region Asset Lists

        /// <summary>
        /// Returns vertex positions from each vertex.
        /// </summary>
        /// <param name="force">If not forced, will return values created by a previous call (if there was one).</param>
        /// <returns></returns>
        public Vector3[] GetVertices(bool force)
        {
            if (_rawVertices != null && _rawVertices.Length != 0 && !force)
            {
                return _rawVertices;
            }

            int i = 0;
            _rawVertices = new Vector3[_vertices.Count];
            foreach (Vertex3 v in _vertices)
            {
                _rawVertices[i++] = v.Position;
            }

            return _rawVertices;
        }

        private Vector3[] _rawVertices;

        /// <summary>
        /// Retrieves normals from raw facedata in a remapped array.
        /// </summary>
        /// <param name="force">If not forced, will return values created by a previous call (if there was one).</param>
        /// <returns></returns>
        public Vector3[] GetNormals(bool force)
        {
            if (_rawNormals != null && _rawNormals.Length != 0 && !force)
            {
                return _rawNormals;
            }

            HashSet<Vector3> list = new HashSet<Vector3>();
            if (_faceData[1] != null)
            {
                Vector3* pIn = (Vector3*) _faceData[1].Address;
                for (int i = 0; i < _pointCount; i++)
                {
                    list.Add(*pIn++);
                }
            }

            _rawNormals = new Vector3[list.Count];
            list.CopyTo(_rawNormals);

            return _rawNormals;
        }

        private Vector3[] _rawNormals;

        /// <summary>
        /// Retrieves texture coordinates from raw facedata in a remapped array.
        /// </summary>
        /// <param name="index">The UV set to retrieve. Values 0 - 7</param>
        /// <param name="force">If not forced, will return values created by a previous call (if there was one).</param>
        /// <returns></returns>
        public Vector2[] GetUVs(int index, bool force)
        {
            index = index.Clamp(0, 7);

            if (_uvs[index] != null && _uvs[index].Length != 0 && !force)
            {
                return _uvs[index];
            }

            HashSet<Vector2> list = new HashSet<Vector2>();
            if (_faceData[index + 4] != null)
            {
                Vector2* pIn = (Vector2*) _faceData[index + 4].Address;
                for (int i = 0; i < _pointCount; i++)
                {
                    list.Add(*pIn++);
                }
            }

            _uvs[index] = new Vector2[list.Count];
            list.CopyTo(_uvs[index]);

            return _uvs[index];
        }

        private readonly Vector2[][] _uvs = new Vector2[8][];

        /// <summary>
        /// Retrieves color values from raw facedata in a remapped array.
        /// </summary>
        /// <param name="index">The color set to retrieve. Values 0 - 1</param>
        /// <param name="force">If not forced, will return values created by a previous call (if there was one).</param>
        /// <returns></returns>
        public RGBAPixel[] GetColors(int index, bool force)
        {
            index = index.Clamp(0, 1);

            if (_colors[index] != null && _colors[index].Length != 0 && !force)
            {
                return _colors[index];
            }

            HashSet<RGBAPixel> list = new HashSet<RGBAPixel>();
            if (_faceData[index + 2] != null)
            {
                RGBAPixel* pIn = (RGBAPixel*) _faceData[index + 2].Address;
                for (int i = 0; i < _pointCount; i++)
                {
                    list.Add(*pIn++);
                }
            }

            _colors[index] = new RGBAPixel[list.Count];
            list.CopyTo(_colors[index]);

            return _colors[index];
        }

        private readonly RGBAPixel[][] _colors = new RGBAPixel[2][];

        #endregion

        #region Reading

        public PrimitiveManager()
        {
        }

        //public PrimitiveManager(
        //    BMDObjectsHeader* header,
        //    BMDObjectEntry* entry,
        //    UnsafeBuffer[] assets,
        //    IMatrixNode[] nodes)
        //{
        //    ElementDescriptor desc = new ElementDescriptor();
        //    desc.SetupBMD(header->GetAttrib(entry->_attribOffset));

        //    _pointCount = 0;

        //    bushort* nodeTable = header->MatrixTable;
        //    PacketLocation* grp = &header->Groups[entry->_firstGroupIndex];
        //    MatrixData* mtx = &header->MatrixData[entry->_firstMatrixDataIndex];

        //    for (int i = 0; i < entry->_groupCount; i++)
        //        _pointCount += FindPointCount((byte*)header->Primitives + grp[i]._offset, desc.Stride, grp[i]._size);

        //    _indices = new UnsafeBuffer(_pointCount * 2);

        //    byte*[] pAssetList = new byte*[12];
        //    byte*[] pFaceBuffers = new byte*[12];

        //    for (int i = 0; i < 12; i++)
        //        if (desc.HasData[i] && assets[i] != null)
        //        {
        //            pFaceBuffers[i] = (byte*)(_faceData[i] = new UnsafeBuffer((i < 2 ? 12 : i < 4 ? 4 : 8) * _pointCount)).Address;
        //            pAssetList[i] = (byte*)assets[i].Address;
        //        }

        //    int triCount = 0, lineCount = 0, pointCount = 0;
        //    fixed (byte** pOut = pFaceBuffers)
        //    fixed (byte** pAssets = pAssetList)
        //    {
        //        for (int i = 0; i < entry->_groupCount; i++)
        //        {
        //            for (ushort x = 0, value = nodeTable[mtx[i]._firstIndex]; x < mtx[i]._count; x++, value = nodeTable[mtx[i]._firstIndex + x])
        //                //if (value != 0xFFFF)
        //                    desc.SetNode(x, value);

        //            _primGroups.AddRange(ReadPrimitives(
        //                (byte*)header->Primitives + grp[i]._offset,
        //                ref desc, pOut, pAssets, nodes, ref triCount, ref lineCount, ref pointCount, grp[i]._size));
        //        }
        //    }

        //    CreateGLPrimitives(triCount, lineCount, pointCount);

        //    uint p3 = 0, p2 = 0, p1 = 0;
        //    for (int i = 0; i < entry->_groupCount; i++)
        //        ExtractIndices((byte*)header->Primitives + grp[i]._offset, desc.Stride, ref p3, ref p2, ref p1, grp[i]._size);

        //    _vertices = desc.Finish((Vector3*)pAssetList[0], nodes);

        //    ushort* pIndex = (ushort*)_indices.Address;
        //    for (int x = 0; x < _pointCount; x++)
        //        if (pIndex[x] >= 0 && pIndex[x] < _vertices.Count)
        //            _vertices[pIndex[x]]._faceDataIndices.Add(x);
        //}

        public PrimitiveManager(
            MDL0Object* polygon,
            AssetStorage assets,
            IMatrixNode[] nodes,
            IObject owner)
        {
            _owner = owner;

            byte*[] pAssetList = new byte*[12];
            byte*[] pFaceBuffers = new byte*[12];
            int id;

            //This relies on the header being accurate!
            _indices = new UnsafeBuffer(2 * (_pointCount = polygon->_numVertices));
            _faceCount = polygon->_numFaces;

            _arrayFlags = polygon->_arrayFlags;
            for (int i = 0; i < 8; i++)
            {
                HasTextureMatrix[i] = _arrayFlags.GetHasTexMatrix(i);
            }

            //Compile decode script by reading the polygon def list
            //This sets how to read the facepoints
            ElementDescriptor desc = new ElementDescriptor();
            desc.SetupMDL0(polygon);

            //Grab asset lists in sequential order.
            if ((id = polygon->_vertexId) >= 0 && desc.HasData[0] && assets.Assets[0] != null)
            {
                pFaceBuffers[0] = (byte*) (_faceData[0] = new UnsafeBuffer(12 * _pointCount)).Address;
                pAssetList[0] = (byte*) assets.Assets[0][id].Address;
            }

            if ((id = polygon->_normalId) >= 0 && desc.HasData[1] && assets.Assets[1] != null)
            {
                pFaceBuffers[1] = (byte*) (_faceData[1] = new UnsafeBuffer(12 * _pointCount)).Address;
                pAssetList[1] = (byte*) assets.Assets[1][id].Address;
            }

            for (int i = 0, x = 2; i < 2; i++, x++)
            {
                if ((id = ((bshort*) polygon->_colorIds)[i]) >= 0 && desc.HasData[x] && assets.Assets[2] != null)
                {
                    pFaceBuffers[x] = (byte*) (_faceData[x] = new UnsafeBuffer(4 * _pointCount)).Address;
                    pAssetList[x] = (byte*) assets.Assets[2][id].Address;
                }
            }

            for (int i = 0, x = 4; i < 8; i++, x++)
            {
                if ((id = ((bshort*) polygon->_uids)[i]) >= 0 && desc.HasData[x] && assets.Assets[3] != null)
                {
                    pFaceBuffers[x] = (byte*) (_faceData[x] = new UnsafeBuffer(8 * _pointCount)).Address;
                    pAssetList[x] = (byte*) assets.Assets[3][id].Address;
                }
            }

            int triCount = 0, lineCount = 0, pointCount = 0;
            byte* pData = (byte*) polygon->PrimitiveData;

            fixed (byte** pOut = pFaceBuffers)
            {
                fixed (byte** pAssets = pAssetList)
                {
                    _primGroups = ReadPrimitives(pData, ref desc, pOut, pAssets, nodes, ref triCount, ref lineCount,
                        ref pointCount);
                }
            }

            CreateGLPrimitives(triCount, lineCount, pointCount);

            uint p3 = 0, p2 = 0, p1 = 0;
            ExtractIndices(pData, desc.Stride, ref p3, ref p2, ref p1);

            //Compile merged vertex list
            _vertices = desc.Finish((Vector3*) pAssetList[0], nodes);

            ushort* pIndex = (ushort*) _indices.Address;
            for (int x = 0; x < _pointCount; x++, pIndex++)
            {
                if (*pIndex >= 0 && *pIndex < _vertices.Count)
                {
                    _vertices[*pIndex].FaceDataIndices.Add(x);
                }
            }

            foreach (Vertex3 v in _vertices)
            {
                v.Parent = _owner as IMatrixNodeUser;
            }
        }

        ~PrimitiveManager()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_graphicsBuffer != null)
            {
                _graphicsBuffer.Dispose();
                _graphicsBuffer = null;
            }

            if (_faceData != null)
            {
                for (int i = 0; i < _faceData.Length; i++)
                {
                    if (_faceData[i] != null)
                    {
                        _faceData[i].Dispose();
                        _faceData[i] = null;
                    }
                }

                _faceData = null;
            }

            if (_indices != null)
            {
                _indices.Dispose();
                _indices = null;
            }
        }

        private int FindPointCount(byte* pData, int stride, uint length = 0)
        {
            int count, pointCount = 0;
            byte* pEnd = pData + length;
            while (true)
            {
                switch ((GXListCommand) (*pData++))
                {
                    case GXListCommand.LoadIndexA:
                    case GXListCommand.LoadIndexB:
                    case GXListCommand.LoadIndexC:
                    case GXListCommand.LoadIndexD:
                        pData += 4;
                        continue;

                    case GXListCommand.DrawQuads:
                    case GXListCommand.DrawTriangles:
                    case GXListCommand.DrawTriangleFan:
                    case GXListCommand.DrawTriangleStrip:
                    case GXListCommand.DrawLines:
                    case GXListCommand.DrawLineStrip:
                    case GXListCommand.DrawPoints:
                        pointCount += count = *(bushort*) pData;
                        break;

                    default:
                        return pointCount;
                }

                pData += 2 + count * stride;

                if (length > 0 && pData > pEnd)
                {
                    return pointCount;
                }
            }
        }

        private List<PrimitiveGroup> ReadPrimitives(
            byte* pStart,
            ref ElementDescriptor desc,
            byte** pOut,
            byte** pAssets,
            IMatrixNode[] cache,
            ref int triCount,
            ref int lineCount,
            ref int pointCount,
            uint length = 0)
        {
            byte* pData = pStart;
            byte* pEnd = pStart + length;
            ushort* indices = (ushort*) _indices.Address;

            List<PrimitiveGroup> groups = new List<PrimitiveGroup>();
            PrimitiveGroup group = new PrimitiveGroup();
            bool newGroup = true;

            while (true)
            {
                GXListCommand cmd = (GXListCommand) (*pData++);
                ushort value = *(bushort*) pData;

                if (cmd <= GXListCommand.LoadIndexD && cmd >= GXListCommand.LoadIndexA)
                {
                    if (newGroup == false)
                    {
                        groups.Add(group);
                        group = new PrimitiveGroup {_offset = (uint) (pData - 1 - pStart)};
                        newGroup = true;
                    }

                    if (!group._nodes.Contains(value) && value != ushort.MaxValue)
                    {
                        group._nodes.Add(value);
                    }

                    if (cmd != GXListCommand.LoadIndexD) //How does the light command work? Never seen it used
                    {
                        if (value < cache.Length && value >= 0)
                        {
                            group._nodeOffsets.Add(
                                new NodeOffset((uint) (pData - pStart) - group._offset, cache[value]));
                        }

                        if (cmd == GXListCommand.LoadIndexA)
                        {
                            desc.SetNode(pData); //Set weight
                        }
                    }
                    else
                    {
                        Console.WriteLine("There are lights in here!");
                    }

                    pData += 4;
                    goto Next;
                }

                switch (cmd)
                {
                    case GXListCommand.DrawQuads:
                        triCount += value / 2 * 3;
                        break;

                    case GXListCommand.DrawTriangles:
                        triCount += value;
                        break;

                    case GXListCommand.DrawTriangleFan:
                    case GXListCommand.DrawTriangleStrip:
                        triCount += (value - 2) * 3;
                        break;

                    case GXListCommand.DrawLines:
                        lineCount += value;
                        break;

                    case GXListCommand.DrawLineStrip:
                        lineCount += (value - 1) * 2;
                        break;

                    case GXListCommand.DrawPoints:
                        pointCount += value;
                        break;

                    default:
                        goto Finish;
                }

                if (newGroup)
                {
                    newGroup = false;
                }

                group._headers.Add(new PrimitiveHeader {Type = (WiiBeginMode) cmd, Entries = value});

                pData += 2;

                //Extract facepoints here!
                desc.Run(ref pData, pAssets, pOut, value, group, ref indices, cache, UseIdentityTexMtx);

                Next:
                if (length > 0 && pData >= pEnd)
                {
                    break;
                }
            }

            Finish:
            groups.Add(group);
            return groups;
        }

        private void CreateGLPrimitives(int triCount, int lineCount, int pointCount)
        {
            _triangles = triCount > 0 ? new GLPrimitive(triCount, BeginMode.Triangles) : null;
            _lines = lineCount > 0 ? new GLPrimitive(lineCount, BeginMode.Lines) : null;
            _points = pointCount > 0 ? new GLPrimitive(pointCount, BeginMode.Points) : null;
        }

        private void ExtractIndices(byte* pData, int stride, ref uint p3, ref uint p2, ref uint p1, uint length = 0)
        {
            uint[] p1arr = null, p2arr = null, p3arr = null;
            ushort count;
            uint index = 0, temp;

            if (_points != null)
            {
                p1arr = _points._indices;
            }

            if (_lines != null)
            {
                p2arr = _lines._indices;
            }

            if (_triangles != null)
            {
                p3arr = _triangles._indices;
            }

            byte* pEnd = pData + length;

            //Extract indices in reverse order, this way we get CCW winding.
            while (true)
            {
                switch ((GXListCommand) (*pData++))
                {
                    case GXListCommand.LoadIndexA:
                    case GXListCommand.LoadIndexB:
                    case GXListCommand.LoadIndexC:
                    case GXListCommand.LoadIndexD:
                        pData += 4; //Skip
                        continue;

                    case GXListCommand.DrawQuads:
                        count = *(bushort*) pData;
                        for (int i = 0; i < count; i += 4)
                        {
                            p3arr[p3++] = index;
                            p3arr[p3++] = index + 2;
                            p3arr[p3++] = index + 1;
                            p3arr[p3++] = index;
                            p3arr[p3++] = index + 3;
                            p3arr[p3++] = index + 2;
                            index += 4;
                        }

                        break;
                    case GXListCommand.DrawTriangles:
                        count = *(bushort*) pData;
                        for (int i = 0; i < count; i += 3)
                        {
                            p3arr[p3++] = index + 2;
                            p3arr[p3++] = index + 1;
                            p3arr[p3++] = index;
                            index += 3;
                        }

                        break;
                    case GXListCommand.DrawTriangleFan:
                        count = *(bushort*) pData;
                        temp = index++;
                        for (int i = 2; i < count; i++)
                        {
                            p3arr[p3++] = temp;
                            p3arr[p3++] = index + 1;
                            p3arr[p3++] = index++;
                        }

                        index++;
                        break;
                    case GXListCommand.DrawTriangleStrip:
                        count = *(bushort*) pData;
                        index += 2;
                        for (int i = 2; i < count; i++)
                        {
                            p3arr[p3++] = index;
                            p3arr[p3++] = (uint) (index - 1 - (i & 1));
                            p3arr[p3++] = (uint) (index++ - 2 + (i & 1));
                        }

                        break;
                    case GXListCommand.DrawLines:
                        count = *(bushort*) pData;
                        for (int i = 0; i < count; i++)
                        {
                            p2arr[p2++] = index++;
                        }

                        break;
                    case GXListCommand.DrawLineStrip:
                        count = *(bushort*) pData;
                        for (int i = 1; i < count; i++)
                        {
                            p2arr[p2++] = index++;
                            p2arr[p2++] = index;
                        }

                        index++;
                        break;
                    case GXListCommand.DrawPoints:
                        count = *(bushort*) pData;
                        for (int i = 0; i < count; i++)
                        {
                            p1arr[p1++] = index++;
                        }

                        break;
                    default: return;
                }

                pData += 2 + count * stride;

                if (length > 0 && pData >= pEnd)
                {
                    return;
                }
            }
        }

        #endregion

        #region Writing

        public int GetDisplayListSize()
        {
            _primitiveSize = 0;

            foreach (PrimitiveGroup g in _primGroups)
            {
                if (_remakePrimitives)
                {
                    if (g._tristrips.Count != 0)
                    {
                        foreach (PointTriangleStrip strip in g._tristrips)
                        {
                            _primitiveSize += 3 + strip._points.Count * _fpStride;
                        }
                    }

                    if (g._triangles.Count != 0)
                    {
                        _primitiveSize += 3 + g._triangles.Count * 3 * _fpStride;
                    }

                    if (g._linestrips.Count != 0)
                    {
                        foreach (PointLineStrip strip in g._linestrips)
                        {
                            _primitiveSize += 3 + strip._points.Count * _fpStride;
                        }
                    }

                    if (g._lines.Count != 0)
                    {
                        _primitiveSize += 3 + g._lines.Count * 2 * _fpStride;
                    }
                }
                else
                {
                    g.RegroupNodes();
                    for (int i = 0; i < g._headers.Count; i++)
                    {
                        _primitiveSize += 3 + g._facePoints[i].Count * _fpStride;
                    }
                }

                if (g._nodes.Count == 1 && g._nodes[0] == 0xFFFF)
                {
                    _isWeighted = false;
                }

                if (_isWeighted)
                {
                    _primitiveSize += 5 * g._nodes.Count * (HasTexMtx ? 3 : 2); //Add total matrices size
                }
            }

            return _primitiveSize;
        }

        /// <summary>
        /// Currently should only should be called when importing a model with the collada importer
        /// or when internal asset data has changed and needs to be distributed to external arrays
        /// </summary>
        public void GroupPrimitives(
            bool useStrips,
            uint cacheSize,
            uint minStripLen,
            bool pushCacheHits,
            bool forceCCW,
            out int newPointCount,
            out int newFaceCount)
        {
            TriangleConverter converter = new TriangleConverter(useStrips, cacheSize, minStripLen, pushCacheHits);

            _primGroups = new List<PrimitiveGroup>();
            newPointCount = 0;
            newFaceCount = 0;

            //Merge vertices and assets into facepoints
            Facepoint[] facepoints = MergeInternalFaceData();
            if (_triangles != null)
            {
                Facepoint[] points = new Facepoint[_triangles._indices.Length];
                uint[] indices = _triangles._indices;

                //Indices are written in reverse for each triangle, 
                //so they need to be set to a triangle in reverse if not CCW
                for (int t = 0; t < _triangles._indices.Length; t++)
                {
                    uint index = indices[t];
                    int pointIndex = forceCCW ? t : t - t % 3 + (2 - t % 3);
                    if (pointIndex < points.Length)
                    {
                        if (index >= facepoints.Length)
                        {
                            points[pointIndex] = new Facepoint();
                        }
                        else
                        {
                            points[pointIndex] = facepoints[index];
                        }
                    }
                }

                _primGroups.AddRange(converter.GroupPrimitives(points, out newPointCount, out newFaceCount));
            }

            if (_lines != null)
            {
                //TODO: combine into linestrips (not as urgent as tristrips was)

                Facepoint[] points = new Facepoint[_lines._indices.Length];
                uint[] indices = _lines._indices;
                for (int t = 0; t < indices.Length; t++)
                {
                    points[t] = facepoints[indices[t]];
                }

                List<PrimitiveGroup> groups = new List<PrimitiveGroup>();

                for (int i = 0; i < points.Length; i += 2)
                {
                    PointLine line = new PointLine(points[i], points[i + 1]);

                    bool added = false;
                    foreach (PrimitiveGroup g in groups)
                    {
                        if (added = g.TryAdd(line))
                        {
                            break;
                        }
                    }

                    if (!added)
                    {
                        PrimitiveGroup g = new PrimitiveGroup();
                        g.TryAdd(line);
                        groups.Add(g);
                    }
                }

                _primGroups.AddRange(groups);
            }

            if (_points != null)
            {
                Facepoint[] points = new Facepoint[_points._indices.Length];
                uint[] indices = _points._indices;
                for (int t = 0; t < indices.Length; t++)
                {
                    points[t] = facepoints[indices[t]];
                }

                List<PrimitiveGroup> groups = new List<PrimitiveGroup>();

                for (int i = 0; i < points.Length; i++)
                {
                    FPoint p = new FPoint(points[i]);

                    bool added = false;
                    foreach (PrimitiveGroup g in groups)
                    {
                        if (added = g.TryAdd(p))
                        {
                            break;
                        }
                    }

                    if (!added)
                    {
                        PrimitiveGroup g = new PrimitiveGroup();
                        g.TryAdd(p);
                        groups.Add(g);
                    }
                }

                _primGroups.AddRange(groups);
            }
        }

        internal void WritePrimitives(MDL0Object* header, IMatrixNode[] cache)
        {
            VoidPtr start = header->PrimitiveData;
            VoidPtr address = header->PrimitiveData;

            foreach (PrimitiveGroup g in _primGroups)
            {
                g._nodes.Sort();
                g._offset = (uint) address - (uint) start;
                g._nodeOffsets.Clear();

                //Write matrix headers for linking matrix influences to points
                if (_isWeighted)
                {
                    ushort node;

                    //Texture Matrices
                    if (HasTexMtx)
                    {
                        for (int i = 0; i < g._nodes.Count; i++)
                        {
                            node = g._nodes[i];

                            *(byte*) address++ = 0x30;

                            if (node < cache.Length)
                            {
                                g._nodeOffsets.Add(new NodeOffset((uint) (address - start) - g._offset, cache[node]));
                            }

                            *(bushort*) address = node;
                            address += 2;
                            *(byte*) address++ = 0xB0;
                            *(byte*) address++ = (byte) (0x78 + 12 * i);
                        }
                    }

                    //Position Matrices
                    for (int i = 0; i < g._nodes.Count; i++)
                    {
                        node = g._nodes[i];

                        *(byte*) address++ = 0x20;

                        if (node < cache.Length)
                        {
                            g._nodeOffsets.Add(new NodeOffset((uint) (address - start) - g._offset, cache[node]));
                        }

                        *(bushort*) address = g._nodes[i];
                        address += 2;
                        *(byte*) address++ = 0xB0;
                        *(byte*) address++ = (byte) (12 * i);
                    }

                    //Normal Matrices
                    for (int i = 0; i < g._nodes.Count; i++)
                    {
                        node = g._nodes[i];

                        *(byte*) address++ = 0x28;

                        if (node < cache.Length)
                        {
                            g._nodeOffsets.Add(new NodeOffset((uint) (address - start) - g._offset, cache[node]));
                        }

                        *(bushort*) address = g._nodes[i];
                        address += 2;
                        *(byte*) address++ = 0x84;
                        *(byte*) address++ = (byte) (9 * i);
                    }
                }

                if (_remakePrimitives)
                {
                    if (g._tristrips.Count != 0)
                    {
                        foreach (PointTriangleStrip strip in g._tristrips)
                        {
                            *(PrimitiveHeader*) address = strip.Header;
                            address += 3;
                            foreach (Facepoint f in strip._points)
                            {
                                WriteFacepoint(f, g, ref address);
                            }
                        }
                    }

                    if (g._triangles.Count != 0)
                    {
                        *(PrimitiveHeader*) address = g.TriangleHeader;
                        address += 3;
                        foreach (PointTriangle tri in g._triangles)
                        {
                            WriteFacepoint(tri._x, g, ref address);
                            WriteFacepoint(tri._y, g, ref address);
                            WriteFacepoint(tri._z, g, ref address);
                        }
                    }

                    if (g._linestrips.Count != 0)
                    {
                        foreach (PointLineStrip strip in g._linestrips)
                        {
                            *(PrimitiveHeader*) address = strip.Header;
                            address += 3;
                            foreach (Facepoint f in strip._points)
                            {
                                WriteFacepoint(f, g, ref address);
                            }
                        }
                    }

                    if (g._lines.Count != 0)
                    {
                        *(PrimitiveHeader*) address = g.LineHeader;
                        address += 3;
                        foreach (PointLine line in g._lines)
                        {
                            WriteFacepoint(line._x, g, ref address);
                            WriteFacepoint(line._y, g, ref address);
                        }
                    }
                }
                else //Write the original primitives read from the model
                {
                    for (int i = 0; i < g._headers.Count; i++)
                    {
                        *(PrimitiveHeader*) address = g._headers[i];
                        address += 3;
                        foreach (Facepoint point in g._facePoints[i])
                        {
                            WriteFacepoint(point, g, ref address);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set vertex descriptor list before calling this
        /// </summary>
        internal void WriteFacepoint(Facepoint f, PrimitiveGroup g, ref VoidPtr address)
        {
            foreach (FacepointAttribute d in _descList)
            {
                switch (d._attr)
                {
                    case GXAttribute.PosNrmMtxId:
                        if (d._type == XFDataFormat.Direct)
                        {
                            *(byte*) address++ = (byte) (3 * g._nodes.IndexOf(f.NodeID));
                        }

                        break;
                    case GXAttribute.Tex0MtxId:
                    case GXAttribute.Tex1MtxId:
                    case GXAttribute.Tex2MtxId:
                    case GXAttribute.Tex3MtxId:
                    case GXAttribute.Tex4MtxId:
                    case GXAttribute.Tex5MtxId:
                    case GXAttribute.Tex6MtxId:
                    case GXAttribute.Tex7MtxId:
                        if (d._type == XFDataFormat.Direct)
                        {
                            int texId = d._attr - GXAttribute.Tex0MtxId;
                            byte value = (byte) (3 * g._nodes.IndexOf(f.NodeID));
                            if (!UseIdentityTexMtx[texId])
                            {
                                value += 30;
                            }

                            *(byte*) address++ = value;
                        }

                        break;
                    case GXAttribute.Position:
                        switch (d._type)
                        {
                            //case XFDataFormat.Direct:
                            //    byte* addr = (byte*)address;
                            //    node.Model._linker._vertices[node._elementIndices[0]].Write(ref addr, f._vertexIndex);
                            //    address = addr;
                            //    break;
                            case XFDataFormat.Index8:
                                *(byte*) address++ = (byte) f._vertexIndex;
                                break;
                            case XFDataFormat.Index16:
                                *(bushort*) address = (ushort) f._vertexIndex;
                                address += 2;
                                break;
                        }

                        break;
                    case GXAttribute.Normal:
                        switch (d._type)
                        {
                            //case XFDataFormat.Direct:
                            //    byte* addr = (byte*)address;
                            //    node.Model._linker._normals[node._elementIndices[1]].Write(ref addr, f._normalIndex);
                            //    address = addr;
                            //    break;
                            case XFDataFormat.Index8:
                                *(byte*) address++ = (byte) f._normalIndex;
                                break;
                            case XFDataFormat.Index16:
                                *(bushort*) address = (ushort) f._normalIndex;
                                address += 2;
                                break;
                        }

                        break;
                    case GXAttribute.Color0:
                    case GXAttribute.Color1:
                        switch (d._type)
                        {
                            //case XFDataFormat.Direct:
                            //    int index = (int)d._attr - 11;
                            //    byte* addr = (byte*)address;
                            //    node.Model._linker._colors[node._elementIndices[index + 2]].Write(ref addr, f._colorIndices[index]);
                            //    address = addr;
                            //    break;
                            case XFDataFormat.Index8:
                                *(byte*) address++ = (byte) f._colorIndices[(int) d._attr - 11];
                                break;
                            case XFDataFormat.Index16:
                                *(bushort*) address = (ushort) f._colorIndices[(int) d._attr - 11];
                                address += 2;
                                break;
                        }

                        break;
                    case GXAttribute.Tex0:
                    case GXAttribute.Tex1:
                    case GXAttribute.Tex2:
                    case GXAttribute.Tex3:
                    case GXAttribute.Tex4:
                    case GXAttribute.Tex5:
                    case GXAttribute.Tex6:
                    case GXAttribute.Tex7:
                        switch (d._type)
                        {
                            //case XFDataFormat.Direct:
                            //    int index = (int)d._attr - 13;
                            //    byte* addr = (byte*)address;
                            //    node.Model._linker._uvs[node._elementIndices[index + 4]].Write(ref addr, f._UVIndices[index]);
                            //    address = addr;
                            //    break;
                            case XFDataFormat.Index8:
                                *(byte*) address++ = (byte) f._UVIndices[(int) d._attr - 13];
                                break;
                            case XFDataFormat.Index16:
                                *(bushort*) address = (ushort) f._UVIndices[(int) d._attr - 13];
                                address += 2;
                                break;
                        }

                        break;
                }
            }
        }

        #endregion

        #region Flags

        public void SetVertexDescList(
            short[] indices,
            List<VertexCodec> vertexCodecs,
            List<ColorCodec> colorCodecs,
            params bool[] forceDirect)
        {
            //Everything is set in the order the facepoint is written!

            //Create new command list
            _descList = new List<FacepointAttribute>();
            _fpStride = 0;
            XFDataFormat fmt;
            _arrayFlags = new XFArrayFlags {_data = 0};

            if (_isWeighted)
            {
                _descList.Add(new FacepointAttribute(GXAttribute.PosNrmMtxId, XFDataFormat.Direct));
                _fpStride++;
                _arrayFlags.HasPosMatrix = true;

                //There are no texture matrices without a position/normal matrix also
                for (int i = 0; i < 8; i++)
                {
                    if (HasTextureMatrix[i])
                    {
                        _descList.Add(new FacepointAttribute((GXAttribute) (i + 1), XFDataFormat.Direct));
                        _fpStride++;
                        _arrayFlags.SetHasTexMatrix(i, true);
                    }
                }
            }

            if (indices[0] > -1 && _faceData[0] != null) //Positions
            {
                _arrayFlags.HasPositions = true;
                fmt = forceDirect != null && forceDirect.Length > 0 && forceDirect[0]
                    ? XFDataFormat.Direct
                    : (XFDataFormat) (GetVertices(false).Length > byte.MaxValue ? 3 : 2);
                _descList.Add(new FacepointAttribute(GXAttribute.Position, fmt));

                //if (fmt == XFDataFormat.Direct)
                //{
                //    _fpStride += linker._vertices[indices[0]]._dstStride;
                //}
                //else
                _fpStride += (int) fmt - 1;
            }

            if (indices[1] > -1 && _faceData[1] != null) //Normals
            {
                _arrayFlags.HasNormals = true;
                fmt = forceDirect != null && forceDirect.Length > 1 && forceDirect[1]
                    ? XFDataFormat.Direct
                    : (XFDataFormat) (GetNormals(false).Length > byte.MaxValue ? 3 : 2);
                _descList.Add(new FacepointAttribute(GXAttribute.Normal, fmt));

                //if (fmt == XFDataFormat.Direct)
                //{
                //    _fpStride += linker._normals[indices[1]]._dstStride;
                //}
                //else
                _fpStride += (int) fmt - 1;
            }

            for (int i = 2; i < 4; i++)
            {
                if (indices[i] > -1 && _faceData[i] != null) //Colors
                {
                    _arrayFlags.SetHasColor(i - 2, true);

                    fmt = forceDirect != null && forceDirect.Length > i && forceDirect[i]
                        ? XFDataFormat.Direct
                        : (XFDataFormat) (GetColors(i - 2, false).Length > byte.MaxValue ? 3 : 2);

                    _descList.Add(new FacepointAttribute((GXAttribute) (i + 9), fmt));

                    //if (fmt == XFDataFormat.Direct)
                    //{
                    //    _fpStride +=  linker._colors[indices[i]]._dstStride;
                    //}
                    //else
                    _fpStride += (int) fmt - 1;
                }
            }

            for (int i = 4; i < 12; i++)
            {
                if (indices[i] > -1 && _faceData[i] != null) //UVs
                {
                    _arrayFlags.SetHasUVs(i - 4, true);
                    fmt = forceDirect != null && forceDirect.Length > i && forceDirect[i]
                        ? XFDataFormat.Direct
                        : (XFDataFormat) (GetUVs(i - 4, false).Length > byte.MaxValue ? 3 : 2);
                    _descList.Add(new FacepointAttribute((GXAttribute) (i + 9), fmt));

                    //if (fmt == XFDataFormat.Direct)
                    //{
                    //    _fpStride += linker._uvs[indices[i]]._dstStride;
                    //}
                    //else
                    _fpStride += (int) fmt - 1;
                }
            }
        }

        /// <summary>
        /// This sets up how to read the facepoints that are going to be written.
        /// </summary>
        public void WriteVertexDescriptor(
            out CPVertexFormat vertexFormat,
            out XFVertexSpecs vertexSpecs)
        {
            XFNormalFormat nrmFmt = XFNormalFormat.None;
            int numColors = 0;
            int numUVs = 0;

            uint posNrmMtxId = 0;
            uint texMtxIdMask = 0;
            uint pos = 0;
            uint nrm = 0;
            uint col0 = 0;
            uint col1 = 0;
            uint tex0 = 0;
            uint tex1 = 0;
            uint tex2 = 0;
            uint tex3 = 0;
            uint tex4 = 0;
            uint tex5 = 0;
            uint tex6 = 0;
            uint tex7 = 0;

            foreach (FacepointAttribute attrPtr in _descList)
            {
                switch (attrPtr._attr)
                {
                    case GXAttribute.PosNrmMtxId:
                        posNrmMtxId = (uint) attrPtr._type;
                        break;

                    case GXAttribute.Tex0MtxId:
                        texMtxIdMask = (uint) (texMtxIdMask & ~1) | ((uint) attrPtr._type << 0);
                        break;
                    case GXAttribute.Tex1MtxId:
                        texMtxIdMask = (uint) (texMtxIdMask & ~2) | ((uint) attrPtr._type << 1);
                        break;
                    case GXAttribute.Tex2MtxId:
                        texMtxIdMask = (uint) (texMtxIdMask & ~4) | ((uint) attrPtr._type << 2);
                        break;
                    case GXAttribute.Tex3MtxId:
                        texMtxIdMask = (uint) (texMtxIdMask & ~8) | ((uint) attrPtr._type << 3);
                        break;
                    case GXAttribute.Tex4MtxId:
                        texMtxIdMask = (uint) (texMtxIdMask & ~16) | ((uint) attrPtr._type << 4);
                        break;
                    case GXAttribute.Tex5MtxId:
                        texMtxIdMask = (uint) (texMtxIdMask & ~32) | ((uint) attrPtr._type << 5);
                        break;
                    case GXAttribute.Tex6MtxId:
                        texMtxIdMask = (uint) (texMtxIdMask & ~64) | ((uint) attrPtr._type << 6);
                        break;
                    case GXAttribute.Tex7MtxId:
                        texMtxIdMask = (uint) (texMtxIdMask & ~128) | ((uint) attrPtr._type << 7);
                        break;

                    case GXAttribute.Position:
                        pos = (uint) attrPtr._type;
                        break;

                    case GXAttribute.Normal:
                        if (attrPtr._type != XFDataFormat.None)
                        {
                            nrm = (uint) attrPtr._type;
                            nrmFmt = XFNormalFormat.XYZ;
                        }

                        break;

                    case GXAttribute.NBT:
                        if (attrPtr._type != XFDataFormat.None)
                        {
                            nrm = (uint) attrPtr._type;
                            nrmFmt = XFNormalFormat.NBT;
                        }

                        break;

                    case GXAttribute.Color0:
                        col0 = (uint) attrPtr._type;
                        numColors += col0 != 0 ? 1 : 0;
                        break;
                    case GXAttribute.Color1:
                        col1 = (uint) attrPtr._type;
                        numColors += col1 != 0 ? 1 : 0;
                        break;

                    case GXAttribute.Tex0:
                        tex0 = (uint) attrPtr._type;
                        numUVs += tex0 != 0 ? 1 : 0;
                        break;
                    case GXAttribute.Tex1:
                        tex1 = (uint) attrPtr._type;
                        numUVs += tex1 != 0 ? 1 : 0;
                        break;
                    case GXAttribute.Tex2:
                        tex2 = (uint) attrPtr._type;
                        numUVs += tex2 != 0 ? 1 : 0;
                        break;
                    case GXAttribute.Tex3:
                        tex3 = (uint) attrPtr._type;
                        numUVs += tex3 != 0 ? 1 : 0;
                        break;
                    case GXAttribute.Tex4:
                        tex4 = (uint) attrPtr._type;
                        numUVs += tex4 != 0 ? 1 : 0;
                        break;
                    case GXAttribute.Tex5:
                        tex5 = (uint) attrPtr._type;
                        numUVs += tex5 != 0 ? 1 : 0;
                        break;
                    case GXAttribute.Tex6:
                        tex6 = (uint) attrPtr._type;
                        numUVs += tex6 != 0 ? 1 : 0;
                        break;
                    case GXAttribute.Tex7:
                        tex7 = (uint) attrPtr._type;
                        numUVs += tex7 != 0 ? 1 : 0;
                        break;
                }
            }

            vertexSpecs = new XFVertexSpecs(numColors, numUVs, nrmFmt);
            vertexFormat = new CPVertexFormat(
                ShiftVtxLo(posNrmMtxId, texMtxIdMask, pos, nrm, col0, col1),
                ShiftVtxHi(tex0, tex1, tex2, tex3, tex4, tex5, tex6, tex7));
        }

        public void SetFormatList(MDL0ObjectNode polygon, ModelLinker linker)
        {
            _fmtList = new List<VertexAttributeFormat>();
            VertexCodec vert = null;
            ColorCodec col = null;

            for (int i = 0; i < 12; i++)
            {
                if (polygon._manager._faceData[i] != null)
                {
                    switch (i)
                    {
                        case 0: //Positions
                            if (linker._vertices != null && linker._vertices.Count != 0 &&
                                polygon._elementIndices[0] != -1)
                            {
                                if ((vert = linker._vertices[polygon._elementIndices[0]]) != null)
                                {
                                    _fmtList.Add(new VertexAttributeFormat(
                                        GXAttribute.Position,
                                        (GXCompType) vert._type,
                                        (GXCompCnt) (vert._hasZ ? 1 : 0),
                                        (byte) vert._scale));
                                }
                            }

                            break;
                        case 1: //Normals
                            vert = null;
                            if (linker._normals != null && linker._normals.Count != 0 &&
                                polygon._elementIndices[1] != -1)
                            {
                                if ((vert = linker._normals[polygon._elementIndices[1]]) != null)
                                {
                                    _fmtList.Add(new VertexAttributeFormat(
                                        GXAttribute.Normal,
                                        (GXCompType) vert._type,
                                        GXCompCnt.NrmXYZ,
                                        (byte) vert._scale));
                                }
                            }

                            break;
                        case 2: //Color 1
                        case 3: //Color 2
                            col = null;
                            if (linker._colors != null && linker._colors.Count != 0 &&
                                polygon._elementIndices[i] != -1 &&
                                (col = linker._colors[polygon._elementIndices[i]]) != null)
                            {
                                _fmtList.Add(new VertexAttributeFormat(
                                    (GXAttribute) ((int) GXAttribute.Color0 + (i - 2)),
                                    (GXCompType) col._outType,
                                    (GXCompCnt) (col._hasAlpha ? 1 : 0),
                                    0));
                            }

                            break;
                        case 4:  //Tex 1
                        case 5:  //Tex 2
                        case 6:  //Tex 3
                        case 7:  //Tex 4
                        case 8:  //Tex 5
                        case 9:  //Tex 6
                        case 10: //Tex 7
                        case 11: //Tex 8
                            vert = null;
                            if (linker._uvs != null && linker._uvs.Count != 0 && polygon._elementIndices[i] != -1)
                            {
                                if ((vert = linker._uvs[polygon._elementIndices[i]]) != null)
                                {
                                    _fmtList.Add(new VertexAttributeFormat(
                                        (GXAttribute) ((int) GXAttribute.Tex0 + (i - 4)),
                                        (GXCompType) vert._type,
                                        GXCompCnt.TexST,
                                        (byte) vert._scale));
                                }
                            }

                            break;
                    }
                }
            }
        }

        public void WriteVertexFormat(MDL0Object* polygon)
        {
            //These are default values.

            uint posCnt = (int) GXCompCnt.PosXYZ;
            uint posType = (int) GXCompType.Float;
            uint posFrac = 0;

            uint nrmCnt = (int) GXCompCnt.NrmXYZ;
            uint nrmType = (int) GXCompType.Float;
            uint nrmId3 = 0;

            uint c0Cnt = (int) GXCompCnt.ClrRGBA;
            uint c0Type = (int) GXCompType.RGBA8;
            uint c1Cnt = (int) GXCompCnt.ClrRGBA;
            uint c1Type = (int) GXCompType.RGBA8;

            uint tx0Cnt = (int) GXCompCnt.TexST;
            uint tx0Type = (int) GXCompType.Float;
            uint tx0Frac = 0;
            uint tx1Cnt = (int) GXCompCnt.TexST;
            uint tx1Type = (int) GXCompType.Float;
            uint tx1Frac = 0;
            uint tx2Cnt = (int) GXCompCnt.TexST;
            uint tx2Type = (int) GXCompType.Float;
            uint tx2Frac = 0;
            uint tx3Cnt = (int) GXCompCnt.TexST;
            uint tx3Type = (int) GXCompType.Float;
            uint tx3Frac = 0;
            uint tx4Cnt = (int) GXCompCnt.TexST;
            uint tx4Type = (int) GXCompType.Float;
            uint tx4Frac = 0;
            uint tx5Cnt = (int) GXCompCnt.TexST;
            uint tx5Type = (int) GXCompType.Float;
            uint tx5Frac = 0;
            uint tx6Cnt = (int) GXCompCnt.TexST;
            uint tx6Type = (int) GXCompType.Float;
            uint tx6Frac = 0;
            uint tx7Cnt = (int) GXCompCnt.TexST;
            uint tx7Type = (int) GXCompType.Float;
            uint tx7Frac = 0;

            if (_fmtList != null)
            {
                foreach (VertexAttributeFormat list in _fmtList)
                {
                    switch (list._attr)
                    {
                        case GXAttribute.Position:
                            posCnt = (uint) list._cnt;
                            posType = (uint) list._type;
                            posFrac = list._frac;
                            break;
                        case GXAttribute.Normal:
                        case GXAttribute.NBT:
                            nrmType = (uint) list._type;
                            if (list._cnt == GXCompCnt.NrmNBT3)
                            {
                                nrmCnt = (uint) GXCompCnt.NrmNBT;
                                nrmId3 = 1;
                            }
                            else
                            {
                                nrmCnt = (uint) list._cnt;
                                nrmId3 = 0;
                            }

                            break;
                        case GXAttribute.Color0:
                            c0Cnt = (uint) list._cnt;
                            c0Type = (uint) list._type;
                            break;
                        case GXAttribute.Color1:
                            c1Cnt = (uint) list._cnt;
                            c1Type = (uint) list._type;
                            break;
                        case GXAttribute.Tex0:
                            tx0Cnt = (uint) list._cnt;
                            tx0Type = (uint) list._type;
                            tx0Frac = list._frac;
                            break;
                        case GXAttribute.Tex1:
                            tx1Cnt = (uint) list._cnt;
                            tx1Type = (uint) list._type;
                            tx1Frac = list._frac;
                            break;
                        case GXAttribute.Tex2:
                            tx2Cnt = (uint) list._cnt;
                            tx2Type = (uint) list._type;
                            tx2Frac = list._frac;
                            break;
                        case GXAttribute.Tex3:
                            tx3Cnt = (uint) list._cnt;
                            tx3Type = (uint) list._type;
                            tx3Frac = list._frac;
                            break;
                        case GXAttribute.Tex4:
                            tx4Cnt = (uint) list._cnt;
                            tx4Type = (uint) list._type;
                            tx4Frac = list._frac;
                            break;
                        case GXAttribute.Tex5:
                            tx5Cnt = (uint) list._cnt;
                            tx5Type = (uint) list._type;
                            tx5Frac = list._frac;
                            break;
                        case GXAttribute.Tex6:
                            tx6Cnt = (uint) list._cnt;
                            tx6Type = (uint) list._type;
                            tx6Frac = list._frac;
                            break;
                        case GXAttribute.Tex7:
                            tx7Cnt = (uint) list._cnt;
                            tx7Type = (uint) list._type;
                            tx7Frac = list._frac;
                            break;
                    }
                }
            }

            MDL0PolygonDefs* Defs = polygon->DefList;
            Defs->UVATA = ShiftUVATA(posCnt, posType, posFrac, nrmCnt, nrmType, c0Cnt, c0Type, c1Cnt, c1Type, tx0Cnt,
                tx0Type, tx0Frac, nrmId3);
            Defs->UVATB = ShiftUVATB(tx1Cnt, tx1Type, tx1Frac, tx2Cnt, tx2Type, tx2Frac, tx3Cnt, tx3Type, tx3Frac,
                tx4Cnt, tx4Type);
            Defs->UVATC = ShiftUVATC(tx4Frac, tx5Cnt, tx5Type, tx5Frac, tx6Cnt, tx6Type, tx6Frac, tx7Cnt, tx7Type,
                tx7Frac);
        }

        #endregion

        #region Shifts

        //Vertex Format Lo Shift
        public uint ShiftVtxLo(uint pmidx, uint t76543210midx, uint pos, uint nrm, uint col0, uint col1)
        {
            return (pmidx << 0) |
                   (t76543210midx << 1) |
                   (pos << 9) |
                   (nrm << 11) |
                   (col0 << 13) |
                   (col1 << 15);
        }

        //Vertex Format Hi Shift
        public uint ShiftVtxHi(uint tex0, uint tex1, uint tex2, uint tex3, uint tex4, uint tex5, uint tex6, uint tex7)
        {
            return (tex0 << 0) |
                   (tex1 << 2) |
                   (tex2 << 4) |
                   (tex3 << 6) |
                   (tex4 << 8) |
                   (tex5 << 10) |
                   (tex6 << 12) |
                   (tex7 << 14);
        }

        //XF Specs Shift
        public uint ShiftXFSpecs(uint host_colors, uint host_normal, uint host_textures)
        {
            return (host_colors << 0) |
                   (host_normal << 2) |
                   (host_textures << 4);
        }

        //UVAT Group A Shift
        public uint ShiftUVATA(uint posCnt, uint posFmt, uint posShft, uint nrmCnt, uint nrmFmt, uint Col0Cnt,
                               uint Col0Fmt, uint Col1Cnt, uint Col1Fmt, uint tex0Cnt, uint tex0Fmt, uint tex0Shft,
                               uint normalIndex3)
        {
            return (posCnt << 0) |
                   (posFmt << 1) |
                   (posShft << 4) |
                   (nrmCnt << 9) |
                   (nrmFmt << 10) |
                   (Col0Cnt << 13) |
                   (Col0Fmt << 14) |
                   (Col1Cnt << 17) |
                   (Col1Fmt << 18) |
                   (tex0Cnt << 21) |
                   (tex0Fmt << 22) |
                   (tex0Shft << 25) |
                   ((uint) 1 << 30) | //Should always be 1
                   (normalIndex3 << 31);
        }

        //UVAT Group B Shift
        public uint ShiftUVATB(uint tex1Cnt, uint tex1Fmt, uint tex1Shft, uint tex2Cnt, uint tex2Fmt, uint tex2Shft,
                               uint tex3Cnt, uint tex3Fmt, uint tex3Shft, uint tex4Cnt, uint tex4Fmt)
        {
            return (tex1Cnt << 0) |
                   (tex1Fmt << 1) |
                   (tex1Shft << 4) |
                   (tex2Cnt << 9) |
                   (tex2Fmt << 10) |
                   (tex2Shft << 13) |
                   (tex3Cnt << 18) |
                   (tex3Fmt << 19) |
                   (tex3Shft << 22) |
                   (tex4Cnt << 27) |
                   (tex4Fmt << 28) |
                   ((uint) 1 << 31); //Should always be 1
        }

        //UVAT Group C Shift
        public uint ShiftUVATC(uint tex4Shft, uint tex5Cnt, uint tex5Fmt, uint tex5Shft, uint tex6Cnt, uint tex6Fmt,
                               uint tex6Shft, uint tex7Cnt, uint tex7Fmt, uint tex7Shft)
        {
            return (tex4Shft << 0) |
                   (tex5Cnt << 5) |
                   (tex5Fmt << 6) |
                   (tex5Shft << 9) |
                   (tex6Cnt << 14) |
                   (tex6Fmt << 15) |
                   (tex6Shft << 18) |
                   (tex7Cnt << 23) |
                   (tex7Fmt << 24) |
                   (tex7Shft << 27);
        }

        #endregion

        #region Rendering

        private void CalcStride()
        {
            _renderStride = 0;
            for (int i = 0; i < 2; i++)
            {
                if (_faceData[i] != null)
                {
                    _renderStride += 12;
                }
            }

            for (int i = 2; i < 4; i++)
            {
                if (_faceData[i] != null)
                {
                    _renderStride += 4;
                }
            }

            for (int i = 4; i < 12; i++)
            {
                if (_faceData[i] != null)
                {
                    _renderStride += 8;
                }
            }
        }

        /// <summary>
        /// Merges vertex data into facepoint indices using linked asset nodes.
        /// </summary>
        internal Facepoint[] MergeExternalFaceData(MDL0ObjectNode poly)
        {
            Facepoint[] _facepoints = new Facepoint[_pointCount];

            ushort* pIndex = (ushort*) _indices.Address;
            for (int x = 0; x < 12; x++)
            {
                if (poly._elementIndices[x] < 0 && x != 0)
                {
                    continue;
                }

                switch (x)
                {
                    case 0:
                        Vector3* pIn0 = (Vector3*) _faceData[x].Address;
                        for (int i = 0; i < _pointCount; i++)
                        {
                            Facepoint f = _facepoints[i] = new Facepoint {_index = i};
                            if (_vertices.Count != 0)
                            {
                                ushort id = *pIndex++;
                                if (id < _vertices.Count && id >= 0)
                                {
                                    f._vertex = _vertices[id];
                                }

                                f._vertexIndex = f._vertex.Facepoints[0]._vertexIndex;
                            }
                        }

                        break;
                    case 1:
                        Vector3* pIn1 = (Vector3*) _faceData[x].Address;
                        for (int i = 0; i < _pointCount; i++)
                        {
                            _facepoints[i]._normalIndex = Array.IndexOf(poly._normalNode.Normals, *pIn1++);
                        }

                        break;
                    case 2:
                    case 3:
                        RGBAPixel* pIn2 = (RGBAPixel*) _faceData[x].Address;
                        for (int i = 0; i < _pointCount; i++)
                        {
                            _facepoints[i]._colorIndices[x - 2] = Array.IndexOf(poly._colorSet[x - 2].Colors, *pIn2++);
                        }

                        break;
                    default:
                        Vector2* pIn3 = (Vector2*) _faceData[x].Address;
                        for (int i = 0; i < _pointCount; i++)
                        {
                            _facepoints[i]._UVIndices[x - 4] = Array.IndexOf(poly._uvSet[x - 4].Points, *pIn3++);
                        }

                        break;
                }
            }

            return _facepoints;
        }

        /// <summary>
        /// Merges vertex data into facepoint indices using internal buffers.
        /// </summary>
        internal Facepoint[] MergeInternalFaceData()
        {
            Facepoint[] _facepoints = new Facepoint[_pointCount];

            bool isModelImport = Collada.Collada.CurrentModel != null;
            if (isModelImport)
            {
                ushort* pIndex = (ushort*) _indices.Address;
                for (int i = 0; i < _pointCount; i++)
                {
                    Facepoint f = _facepoints[i] = new Facepoint {_index = i};
                    f._vertexIndex = *pIndex++;
                    if (f._vertexIndex < _vertices.Count && f._vertexIndex >= 0)
                    {
                        f._vertex = _vertices[f._vertexIndex];
                    }
                }
            }
            else if (_vertices != null)
            {
                foreach (Vertex3 v in _vertices)
                {
                    for (int i = 0; i < v.FaceDataIndices.Count; i++)
                    {
                        _facepoints[v.FaceDataIndices[i]] = v.Facepoints[i];
                    }
                }
            }

            for (int x = 1; x < 12; x++)
            {
                if (_faceData[x] == null || !isModelImport && !_changed[x])
                {
                    continue;
                }

                switch (x)
                {
                    case 1:
                        Vector3* pIn1 = (Vector3*) _faceData[x].Address;
                        for (int i = 0; i < _pointCount; i++)
                        {
                            _facepoints[i]._normalIndex = Array.IndexOf(GetNormals(false), *pIn1++);
                        }

                        break;
                    case 2:
                    case 3:
                        RGBAPixel* pIn2 = (RGBAPixel*) _faceData[x].Address;
                        if (isModelImport)
                        {
                            if (Collada.Collada._importOptions._useOneNode)
                            {
                                for (int i = 0; i < _pointCount; i++)
                                {
                                    _facepoints[i]._colorIndices[x - 2] =
                                        Array.IndexOf(Collada.Collada._importOptions._singleColorNodeEntries, *pIn2++);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < _pointCount; i++)
                                {
                                    _facepoints[i]._colorIndices[x - 2] = Array
                                        .IndexOf(
                                            ((MDL0ObjectNode) ((MDL0Node) Collada.Collada
                                                    .CurrentModel)
                                                ._objList[_newClrObj[x - 2]])
                                            ._manager.GetColors(x - 2, false),
                                            *pIn2++).ClampMin(0);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < _pointCount; i++)
                            {
                                _facepoints[i]._colorIndices[x - 2] =
                                    Array.IndexOf(GetColors(x - 2, false), *pIn2++).ClampMin(0);
                            }
                        }

                        break;
                    default:
                        Vector2* pIn3 = (Vector2*) _faceData[x].Address;
                        for (int i = 0; i < _pointCount; i++)
                        {
                            _facepoints[i]._UVIndices[x - 4] = Array.IndexOf(GetUVs(x - 4, false), *pIn3++);
                        }

                        break;
                }
            }

            return _facepoints;
        }

        public void Unweight(bool updateAssets)
        {
            if (_vertices != null)
            {
                foreach (Vertex3 v in _vertices)
                {
                    v.Unweight(updateAssets);
                }
            }
        }

        public void Weight()
        {
            //Weight vertices
            if (_vertices != null)
            {
                foreach (Vertex3 v in _vertices)
                {
                    v.Weight();
                }
            }

            //Update graphics buffer with weighted data on next render
            _dirty[0] = true;
            _dirty[1] = true;
        }

        internal void UpdateStream(int index)
        {
            _dirty[index] = false;

            if (_faceData[index] == null || _vertices == null || _vertices.Count == 0)
            {
                return;
            }

            //Set starting address
            byte* pOut = (byte*) _graphicsBuffer.Address;
            for (int i = 0; i < index; i++)
            {
                if (_faceData[i] != null)
                {
                    if (i < 2)
                    {
                        pOut += 12;
                    }
                    else if (i < 4)
                    {
                        pOut += 4;
                    }
                    else
                    {
                        pOut += 8;
                    }
                }
            }

            int v;
            if (index == 0) //Vertices
            {
                ushort* pIndex = (ushort*) _indices.Address;
                for (int i = 0; i < _pointCount; i++, pOut += _renderStride)
                {
                    if ((v = *pIndex++) < _vertices.Count && v >= 0)
                    {
                        *(Vector3*) pOut = _vertices[v].WeightedPosition;
                    }
                }
            }
            else if (index == 1) //Normals
            {
                ushort* pIndex = (ushort*) _indices.Address;
                Vector3* pIn = (Vector3*) _faceData[index].Address;
                for (int i = 0; i < _pointCount; i++, pOut += _renderStride)
                {
                    if ((v = *pIndex++) < _vertices.Count && v >= 0)
                    {
                        *(Vector3*) pOut = *pIn++ * _vertices[v].GetMatrix().GetRotationMatrix();
                    }
                    else
                    {
                        *(Vector3*) pOut = *pIn++;
                    }
                }
            }
            else if (index < 4) //Colors
            {
                RGBAPixel* pIn = (RGBAPixel*) _faceData[index].Address;
                for (int i = 0; i < _pointCount; i++, pOut += _renderStride)
                {
                    *(RGBAPixel*) pOut = *pIn++;
                }
            }
            else //UVs
            {
                Vector2* pIn = (Vector2*) _faceData[index].Address;
                for (int i = 0; i < _pointCount; i++, pOut += _renderStride)
                {
                    *(Vector2*) pOut = *pIn++;
                }
            }
        }

        public void PrepareStream()
        {
            CalcStride();
            int bufferSize = _renderStride * _pointCount;

            //Dispose of buffer if size doesn't match
            if (_graphicsBuffer != null && _graphicsBuffer.Length != bufferSize)
            {
                _graphicsBuffer.Dispose();
                _graphicsBuffer = null;
            }

            //Create data buffer
            if (_graphicsBuffer == null)
            {
                _graphicsBuffer = new UnsafeBuffer(bufferSize);
                for (int i = 0; i < 12; i++)
                {
                    _dirty[i] = true;
                }
            }

            //Update streams before binding
            for (int i = 0; i < 12; i++)
            {
                if (_dirty[i])
                {
                    UpdateStream(i);
                }
            }
        }

        public void BindStream()
        {
            byte* pData = (byte*) _graphicsBuffer.Address;
            for (int i = 0; i < 12; i++)
            {
                if (_faceData[i] != null)
                {
                    switch (i)
                    {
                        case 0:
                            GL.EnableClientState(ArrayCap.VertexArray);
                            GL.VertexPointer(3, VertexPointerType.Float, _renderStride, (IntPtr) pData);
                            pData += 12;
                            break;
                        case 1:
                            GL.EnableClientState(ArrayCap.NormalArray);
                            GL.NormalPointer(NormalPointerType.Float, _renderStride, (IntPtr) pData);
                            pData += 12;
                            break;
                        case 2:
                            GL.EnableClientState(ArrayCap.ColorArray);
                            GL.ColorPointer(4, ColorPointerType.UnsignedByte, _renderStride, (IntPtr) pData);
                            pData += 4;
                            break;
                        case 3:
                            GL.EnableClientState(ArrayCap.SecondaryColorArray);
                            GL.SecondaryColorPointer(4, ColorPointerType.UnsignedByte, _renderStride, (IntPtr) pData);
                            pData += 4;
                            break;
                        default:
                            pData += 8;
                            break;
                    }
                }
            }
        }

        internal void DetachStreams()
        {
            for (int i = 0; i < 8; i++)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + i);
                GL.ClientActiveTexture(TextureUnit.Texture0 + i);
                GL.DisableClientState(ArrayCap.TextureCoordArray);
                GL.Disable(EnableCap.Texture2D);
            }

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.ClientActiveTexture(TextureUnit.Texture0);
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);
            GL.DisableClientState(ArrayCap.ColorArray);
        }

        public void DisableTextures()
        {
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.Disable(EnableCap.Texture2D);
        }

        public void ApplyTexture(TexSourceRow source)
        {
            int texId = source >= TexSourceRow.TexCoord0 ? source - TexSourceRow.TexCoord0 : -1 - (int) source;
            texId = texId < 0 ? 0 : texId;

            if (texId >= 0 && _faceData[texId + 4] != null)
            {
                byte* pData = (byte*) _graphicsBuffer.Address;
                for (int i = 0; i < texId + 4; i++)
                {
                    if (_faceData[i] != null)
                    {
                        if (i < 2)
                        {
                            pData += 12;
                        }
                        else if (i < 4)
                        {
                            pData += 4;
                        }
                        else
                        {
                            pData += 8;
                        }
                    }
                }

                GL.EnableClientState(ArrayCap.TextureCoordArray);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, _renderStride, (IntPtr) pData);
            }
        }

        public void RenderMesh()
        {
            _triangles?.Render();

            _lines?.Render();

            _points?.Render();
        }

        public static Color DefaultVertColor = Color.FromArgb(0, 128, 0);
        internal Color _vertColor = Color.Transparent;

        public static Color DefaultNormColor = Color.FromArgb(0, 0, 128);
        internal Color _normColor = Color.Transparent;

        public static float NormalLength = 0.5f;

        public const float _nodeRadius = 0.05f;
        private const float _nodeAdj = 0.01f;

        public bool _render = true;
        public bool _renderNormals = true;

        internal void RenderVertices(IMatrixNode singleBind, IBoneNode weightTarget, bool depthPass,
                                     GLCamera camera)
        {
            if (!_render)
            {
                return;
            }

            foreach (Vertex3 v in _vertices)
            {
                Color w = v.HighlightColor != Color.Transparent ? v.HighlightColor :
                    singleBind != null && singleBind == weightTarget ? Color.Red : v.GetWeightColor(weightTarget);
                if (w != Color.Transparent)
                {
                    GL.Color4(w);
                }
                else
                {
                    GL.Color4(DefaultVertColor);
                }

                float d = camera.GetPoint().DistanceTo(v.WeightedPosition);
                GL.PointSize(
                    d <= 0 ? 1 : (3000.0f / d).Clamp(1.0f, depthPass ? 8.0f : 5.0f) * (depthPass ? 1.5f : 1.2f));

                GL.Begin(BeginMode.Points);
                GL.Vertex3(v.WeightedPosition._x, v.WeightedPosition._y, v.WeightedPosition._z);
                GL.End();
            }
        }

        internal void RenderNormals()
        {
            if (!_render || _faceData[1] == null)
            {
                return;
            }

            ushort* indices = (ushort*) _indices.Address;
            Vector3* normals = (Vector3*) _faceData[1].Address;

            if (_normColor != Color.Transparent)
            {
                GL.Color4(_normColor);
            }
            else
            {
                GL.Color4(DefaultNormColor);
            }

            for (int i = 0; i < _pointCount; i++)
            {
                GL.PushMatrix();

                GL.Color4(Color.Blue);

                Vertex3 n = _vertices[indices[i]];
                Vector3 w = normals[i] * n.GetMatrix().GetRotationMatrix();

                Matrix m = Matrix.TransformMatrix(new Vector3(NormalLength), new Vector3(), n.WeightedPosition);
                GL.MultMatrix((float*) &m);

                GL.Begin(BeginMode.Lines);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(w._x, w._y, w._z);
                GL.End();

                GL.PopMatrix();
            }
        }

        #endregion

        //TODO: try to preserve vertex, normal and color array indices
        //otherwise SHP0 morph sets won't work anymore

        public void PositionsChanged(MDL0ObjectNode obj, bool forceNewNode = false)
        {
            if (obj == null || obj.Deleting || _vertices == null)
            {
                return;
            }

            SetAssetChanged(0);
            obj._forceRebuild = true;
            obj.SignalPropertyChange();

            MDL0VertexNode node;
            if (obj._vertexNode != null)
            {
                if (obj._vertexNode._objects.Count == 1 && !forceNewNode)
                {
                    node = obj._vertexNode;
                }
                else
                {
                    node = new MDL0VertexNode();
                    MDL0Node m = obj.Model;
                    if (m.VertexGroup == null)
                    {
                        MDL0GroupNode g = new MDL0GroupNode(MDLResourceType.Vertices);
                        m.LinkGroup(g);
                        g.Parent = m;
                    }

                    m.VertexGroup.AddChild(node);
                    node.Name = node.FindName("Regenerated");
                    if (obj._vertexNode._objects.Contains(obj))
                    {
                        obj._vertexNode._objects.Remove(obj);
                    }

                    obj._vertexNode = node;
                    obj._vertexNode._objects.Add(obj);
                    obj._elementIndices[0] = (short) obj._vertexNode.Index;
                }
            }
            else
            {
                node = new MDL0VertexNode();
                MDL0Node m = obj.Model;
                if (m.VertexGroup == null)
                {
                    MDL0GroupNode g = new MDL0GroupNode(MDLResourceType.Vertices);
                    m.LinkGroup(g);
                    g.Parent = m;
                }

                m.VertexGroup.AddChild(node);
                node.Name = node.FindName("Regenerated");
                obj._vertexNode = node;
                obj._vertexNode._objects.Add(obj);
                obj._elementIndices[0] = (short) obj._vertexNode.Index;
            }

            node.Vertices = GetVertices(true);
            int x = 0;
            foreach (Vertex3 v in _vertices)
            {
                foreach (Facepoint f in v.Facepoints)
                {
                    f._vertexIndex = x;
                    f._vertex = v;
                }

                x++;
            }
        }

        public void NormalsChanged(MDL0ObjectNode obj, bool forceNewNode = false)
        {
            if (obj == null || obj.Deleting || _faceData[1] == null)
            {
                return;
            }

            SetAssetChanged(1);
            obj._forceRebuild = true;
            obj.SignalPropertyChange();

            MDL0NormalNode node;
            if (obj._normalNode != null)
            {
                if (obj._normalNode._objects.Count == 1 && !forceNewNode)
                {
                    node = obj._normalNode;
                }
                else
                {
                    node = new MDL0NormalNode();
                    MDL0Node m = obj.Model;
                    if (m.NormalGroup == null)
                    {
                        MDL0GroupNode g = new MDL0GroupNode(MDLResourceType.Normals);
                        m.LinkGroup(g);
                        g.Parent = m;
                    }

                    m.NormalGroup.AddChild(node);
                    node.Name = node.FindName("Regenerated");
                    if (obj._normalNode._objects.Contains(obj))
                    {
                        obj._normalNode._objects.Remove(obj);
                    }

                    obj._normalNode = node;
                    obj._normalNode._objects.Add(obj);
                    obj._elementIndices[1] = (short) obj._normalNode.Index;
                }
            }
            else
            {
                node = new MDL0NormalNode();
                MDL0Node m = obj.Model;
                if (m.NormalGroup == null)
                {
                    MDL0GroupNode g = new MDL0GroupNode(MDLResourceType.Normals);
                    m.LinkGroup(g);
                    g.Parent = m;
                }

                m.NormalGroup.AddChild(node);
                node.Name = node.FindName("Regenerated");
                obj._normalNode = node;
                obj._normalNode._objects.Add(obj);
                obj._elementIndices[1] = (short) obj._normalNode.Index;
            }

            node.Normals = GetNormals(true);
            Vector3* pData = (Vector3*) _faceData[1].Address;
            foreach (Vertex3 v in _vertices)
            {
                for (int i = 0; i < v.FaceDataIndices.Count; i++)
                {
                    v.Facepoints[i]._normalIndex = Array.IndexOf(node.Normals, pData[v.FaceDataIndices[i]]);
                }
            }
        }

        public void ColorsChanged(MDL0ObjectNode obj, int id, bool forceNewNode = false)
        {
            id = id.Clamp(0, 1);

            if (obj == null || _faceData[id + 2] == null)
            {
                return;
            }

            SetAssetChanged(id + 2);
            obj._forceRebuild = true;
            obj.SignalPropertyChange();

            MDL0ColorNode node;
            if (obj._colorSet[id] != null)
            {
                if (obj._colorSet[id]._objects.Count == 1 && !forceNewNode)
                {
                    node = obj._colorSet[id];
                }
                else
                {
                    node = new MDL0ColorNode();
                    obj.Model.ColorGroup.AddChild(node);
                    node.Name = node.FindName("Regenerated");
                    if (obj._colorSet[id]._objects.Contains(obj))
                    {
                        obj._colorSet[id]._objects.Remove(obj);
                    }

                    obj._colorSet[id] = node;
                    obj._colorSet[id]._objects.Add(obj);
                    obj._elementIndices[id + 2] = (short) obj._colorSet[id].Index;
                }
            }
            else
            {
                node = new MDL0ColorNode();
                MDL0Node m = obj.Model;
                if (m.ColorGroup == null)
                {
                    MDL0GroupNode g = new MDL0GroupNode(MDLResourceType.Colors);
                    m.LinkGroup(g);
                    g.Parent = m;
                }

                m.ColorGroup.AddChild(node);
                node.Name = node.FindName("Regenerated");
                obj._colorSet[id] = node;
                obj._colorSet[id]._objects.Add(obj);
                obj._elementIndices[id + 2] = (short) obj._colorSet[id].Index;
            }

            node.Colors = GetColors(id, true);
            RGBAPixel* pData = (RGBAPixel*) _faceData[id + 2].Address;
            foreach (Vertex3 v in _vertices)
            {
                for (int i = 0; i < v.FaceDataIndices.Count; i++)
                {
                    v.Facepoints[i]._colorIndices[id] = Array.IndexOf(node.Colors, pData[v.FaceDataIndices[i]]);
                }
            }
        }

        public void UVsChanged(MDL0ObjectNode obj, int id, bool forceNewNode = false)
        {
            id = id.Clamp(0, 7);

            if (obj == null || _faceData[id + 4] == null)
            {
                return;
            }

            SetAssetChanged(id + 4);
            obj._forceRebuild = true;
            obj.SignalPropertyChange();

            MDL0UVNode node;
            if (obj._uvSet[id] != null)
            {
                if (obj._uvSet[id]._objects.Count == 1 && !forceNewNode)
                {
                    node = obj._uvSet[id];
                }
                else
                {
                    node = new MDL0UVNode();
                    obj.Model.UVGroup.AddChild(node);
                    node.Name = node.FindName("Regenerated");
                    if (obj._uvSet[id]._objects.Contains(obj))
                    {
                        obj._uvSet[id]._objects.Remove(obj);
                    }

                    obj._uvSet[id] = node;
                    obj._uvSet[id]._objects.Add(obj);
                    obj._elementIndices[id + 4] = (short) obj._uvSet[id].Index;
                }
            }
            else
            {
                node = new MDL0UVNode();
                MDL0Node m = obj.Model;
                if (m.UVGroup == null)
                {
                    MDL0GroupNode g = new MDL0GroupNode(MDLResourceType.UVs);
                    m.LinkGroup(g);
                    g.Parent = m;
                }

                m.UVGroup.AddChild(node);
                node.Name = node.FindName("Regenerated");
                obj._uvSet[id] = node;
                obj._uvSet[id]._objects.Add(obj);
                obj._elementIndices[id + 4] = (short) obj._uvSet[id].Index;
            }

            node.Points = GetUVs(id, true);
            Vector2* pData = (Vector2*) _faceData[id + 4].Address;
            foreach (Vertex3 v in _vertices)
            {
                for (int i = 0; i < v.FaceDataIndices.Count; i++)
                {
                    v.Facepoints[i]._UVIndices[id] = Array.IndexOf(node.Points, pData[v.FaceDataIndices[i]]);
                }
            }
        }
    }

    public class GLPrimitive
    {
        public BeginMode _type;
        public uint[] _indices;

        public GLPrimitive(int elements, BeginMode type)
        {
            _type = type;
            _indices = new uint[elements];
        }

        public void Render()
        {
            GL.DrawElements(_type, _indices.Length, DrawElementsType.UnsignedInt, _indices);
        }
    }
}