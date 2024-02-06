using BrawlLib.Internal;
using BrawlLib.Wii.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BrawlLib.Modeling.Collada
{
    public unsafe partial class Collada
    {
        private static PrimitiveManager DecodePrimitivesWeighted(
            Matrix bindMatrix,
            GeometryEntry geo,
            SkinEntry skin,
            SceneEntry scene,
            InfluenceManager infManager,
            Type boneType)
        {
            PrimitiveManager manager = DecodePrimitives(geo);

            IBoneNode[] boneList;
            IBoneNode bone = null;
            int boneCount;

            string[] jointStringArray = null;
            string jointString = null;

            byte* pCmd = stackalloc byte[4];
            int cmdCount = skin._weightInputs.Count;
            float weight = 0;
            float* pWeights = null;
            Vector3* pVert = null, pNorms = null;
            ushort* pVInd = (ushort*) manager._indices.Address;
            List<Vertex3> vertList = new List<Vertex3>(skin._weightCount);
            Matrix* pMatrix = null;

            UnsafeBuffer remap = new UnsafeBuffer(skin._weightCount * 2);
            ushort* pRemap = (ushort*) remap.Address;

            if (manager._faceData[1] != null)
            {
                pNorms = (Vector3*) manager._faceData[1].Address;
            }

            manager._vertices = vertList;

            //Find vertex source
            foreach (SourceEntry s in geo._sources)
            {
                if (s._id == geo._verticesInput._source)
                {
                    pVert = (Vector3*) ((UnsafeBuffer) s._arrayData).Address;
                    break;
                }
            }

            //Find joint source
            foreach (InputEntry inp in skin._jointInputs)
            {
                if (inp._semantic == SemanticType.JOINT)
                {
                    foreach (SourceEntry src in skin._sources)
                    {
                        if (src._id == inp._source)
                        {
                            jointStringArray = src._arrayData as string[];
                            jointString = src._arrayDataString;
                            break;
                        }
                    }
                }
                else if (inp._semantic == SemanticType.INV_BIND_MATRIX)
                {
                    foreach (SourceEntry src in skin._sources)
                    {
                        if (src._id == inp._source)
                        {
                            pMatrix = (Matrix*) ((UnsafeBuffer) src._arrayData).Address;
                            break;
                        }
                    }
                }
            }

            Error = "There was a problem creating the list of bones for geometry entry " + geo._name;

            //Populate bone list
            boneCount = jointStringArray.Length;
            boneList = new IBoneNode[boneCount];
            for (int i = 0; i < boneCount; i++)
            {
                NodeEntry entry = scene.FindNode(jointStringArray[i]);
                if (entry?._node != null)
                {
                    boneList[i] = entry._node as IBoneNode;
                }
                else
                {
                    //Search in reverse!
                    foreach (NodeEntry node in scene._nodes)
                    {
                        if ((entry = RecursiveTestNode(jointString, node)) != null)
                        {
                            if (entry._node != null)
                            {
                                boneList[i] = entry._node as IBoneNode;
                            }

                            break;
                        }
                    }

                    //Couldn't find the bone
                    if (boneList[i] == null)
                    {
                        boneList[i] = Activator.CreateInstance(boneType) as IBoneNode;
                    }
                }
            }

            //Build command list
            foreach (InputEntry inp in skin._weightInputs)
            {
                switch (inp._semantic)
                {
                    case SemanticType.JOINT:
                        pCmd[inp._offset] = 1;
                        break;

                    case SemanticType.WEIGHT:
                        pCmd[inp._offset] = 2;

                        //Get weight source
                        foreach (SourceEntry src in skin._sources)
                        {
                            if (src._id == inp._source)
                            {
                                pWeights = (float*) ((UnsafeBuffer) src._arrayData).Address;
                                break;
                            }
                        }

                        break;

                    default:
                        pCmd[inp._offset] = 0;
                        break;
                }
            }

            Error = "There was a problem creating vertex influences for geometry entry " + geo._name;

            //Build vertex list and remap table
            for (int i = 0; i < skin._weightCount; i++)
            {
                //Create influence
                int iCount = skin._weights[i].Length / cmdCount;
                Influence inf = new Influence();
                fixed (int* p = skin._weights[i])
                {
                    int* iPtr = p;
                    for (int x = 0; x < iCount; x++)
                    {
                        for (int z = 0; z < cmdCount; z++, iPtr++)
                        {
                            if (pCmd[z] == 1)
                            {
                                bone = boneList[*iPtr];
                            }
                            else if (pCmd[z] == 2)
                            {
                                weight = pWeights[*iPtr];
                            }
                        }

                        inf.AddWeight(new BoneWeight(bone, weight));
                    }
                }

                inf.CalcMatrix();

                Error = "There was a problem creating a vertex from the geometry entry " + geo._name +
                        ".\nMake sure that all the vertices are weighted properly.";

                Vector3 worldPos = bindMatrix * skin._bindMatrix * pVert[i];
                Vertex3 v;
                if (inf.Weights.Count > 1)
                {
                    //Match with manager
                    inf = infManager.FindOrCreate(inf);
                    v = new Vertex3(worldPos, inf); //World position
                }
                else
                {
                    bone = inf.Weights[0].Bone;
                    v = new Vertex3(bone.InverseBindMatrix * worldPos, bone); //Local position
                }

                ushort index = 0;
                while (index < vertList.Count)
                {
                    if (v.Equals(vertList[index]))
                    {
                        break;
                    }

                    index++;
                }

                if (index == vertList.Count)
                {
                    vertList.Add(v);
                }

                pRemap[i] = index;
            }

            Error = "There was a problem fixing normal rotations for geometry entry " + geo._name;

            //Remap vertex indices and fix normals
            for (int i = 0; i < manager._pointCount; i++, pVInd++)
            {
                *pVInd = pRemap[*pVInd];

                if (pNorms != null)
                {
                    Vertex3 v = null;
                    if (*pVInd < vertList.Count)
                    {
                        v = vertList[*pVInd];
                    }

                    if (v?.MatrixNode != null)
                    {
                        if (v.MatrixNode.Weights.Count > 1)
                        {
                            pNorms[i] =
                                (bindMatrix *
                                 skin._bindMatrix).GetRotationMatrix() *
                                pNorms[i];
                        }
                        else
                        {
                            pNorms[i] =
                                (v.MatrixNode.Weights[0].Bone.InverseBindMatrix *
                                 bindMatrix *
                                 skin._bindMatrix).GetRotationMatrix() *
                                pNorms[i];
                        }
                    }
                }
            }

            remap.Dispose();
            return manager;
        }

        private static NodeEntry RecursiveTestNode(string jointStrings, NodeEntry node)
        {
            if (jointStrings.IndexOf(node._name) >= 0)
            {
                return node;
            }

            if (jointStrings.IndexOf(node._sid) >= 0)
            {
                return node;
            }

            if (jointStrings.IndexOf(node._id) >= 0)
            {
                return node;
            }

            NodeEntry e;
            foreach (NodeEntry n in node._children)
            {
                if ((e = RecursiveTestNode(jointStrings, n)) != null)
                {
                    return e;
                }
            }

            return null;
        }

        private static PrimitiveManager DecodePrimitivesUnweighted(Matrix bindMatrix, GeometryEntry geo)
        {
            PrimitiveManager manager = DecodePrimitives(geo);

            Vector3* pVert = null, pNorms = null;
            ushort* pVInd = (ushort*) manager._indices.Address;
            int vCount = 0;
            List<Vertex3> vertList = new List<Vertex3>(manager._pointCount);

            manager._vertices = vertList;

            if (manager._faceData[1] != null)
            {
                pNorms = (Vector3*) manager._faceData[1].Address;
            }

            //Find vertex source
            foreach (SourceEntry s in geo._sources)
            {
                if (s._id == geo._verticesInput._source)
                {
                    UnsafeBuffer b = s._arrayData as UnsafeBuffer;
                    pVert = (Vector3*) b.Address;
                    vCount = b.Length / 12;
                    break;
                }
            }

            UnsafeBuffer remap = new UnsafeBuffer(vCount * 2);
            ushort* pRemap = (ushort*) remap.Address;

            //Create remap table
            for (int i = 0; i < vCount; i++)
            {
                //Create vertex and look for match
                Vertex3 v = new Vertex3(bindMatrix * pVert[i]);

                int index = 0;
                while (index < vertList.Count)
                {
                    if (v.Equals(vertList[index]))
                    {
                        break;
                    }

                    index++;
                }

                if (index == vertList.Count)
                {
                    vertList.Add(v);
                }

                pRemap[i] = (ushort) index;
            }

            //Remap vertex indices and fix normals
            for (int i = 0; i < manager._pointCount; i++, pVInd++)
            {
                *pVInd = pRemap[*pVInd];

                if (pNorms != null)
                {
                    pNorms[i] = bindMatrix.GetRotationMatrix() * pNorms[i];
                }
            }

            remap.Dispose();

            return manager;
        }

        private static PrimitiveManager DecodePrimitives(GeometryEntry geo)
        {
            uint[] pTriarr = null, pLinarr = null;
            uint pTri = 0, pLin = 0;
            long* pInDataList = stackalloc long[12];
            long* pOutDataList = stackalloc long[12];
            int* pData = stackalloc int[16];
            int faces = 0, lines = 0, points = 0;
            uint fIndex = 0, lIndex = 0, temp;

            PrimitiveDecodeCommand* pCmd = (PrimitiveDecodeCommand*) pData;
            byte** pInData = (byte**) pInDataList;
            byte** pOutData = (byte**) pOutDataList;

            PrimitiveManager manager = new PrimitiveManager();

            //Assign vertex source
            foreach (SourceEntry s in geo._sources)
            {
                if (s._id == geo._verticesInput._source)
                {
                    pInData[0] = (byte*) ((UnsafeBuffer) s._arrayData).Address;
                    break;
                }
            }

            foreach (PrimitiveEntry prim in geo._primitives)
            {
                //Get face/line count
                if (prim._type == ColladaBeginMode.lines || prim._type == ColladaBeginMode.linestrips)
                {
                    lines += prim._faceCount;
                }
                else
                {
                    faces += prim._faceCount;
                }

                //Get point total
                points += prim._pointCount;

                //Signal storage buffers and set type offsets
                foreach (InputEntry inp in prim._inputs)
                {
                    int offset = -1;

                    switch (inp._semantic)
                    {
                        case SemanticType.VERTEX:
                            offset = 0;
                            break;
                        case SemanticType.NORMAL:
                            offset = 1;
                            break;
                        case SemanticType.COLOR:
                            if (inp._set < 2)
                            {
                                offset = 2 + inp._set;
                            }

                            break;
                        case SemanticType.TEXCOORD:
                            if (inp._set < 8)
                            {
                                offset = 4 + inp._set;
                            }

                            break;
                    }

                    if (offset != -1)
                    {
                        manager._dirty[offset] = true;
                    }

                    inp._outputOffset = offset;
                }
            }

            manager._pointCount = points;

            //Create primitives
            if (faces > 0)
            {
                manager._triangles = new GLPrimitive(faces * 3, OpenTK.Graphics.OpenGL.BeginMode.Triangles);
                pTriarr = manager._triangles._indices;
            }

            if (lines > 0)
            {
                manager._lines = new GLPrimitive(lines * 2, OpenTK.Graphics.OpenGL.BeginMode.Lines);
                pLinarr = manager._lines._indices;
            }

            manager._indices = new UnsafeBuffer(points * 2);
            //Create face buffers and assign output pointers
            for (int i = 0; i < 12; i++)
            {
                if (manager._dirty[i])
                {
                    int stride;
                    if (i == 0)
                    {
                        stride = 2;
                    }
                    else if (i == 1)
                    {
                        stride = 12;
                    }
                    else if (i < 4)
                    {
                        stride = 4;
                    }
                    else
                    {
                        stride = 8;
                    }

                    manager._faceData[i] = new UnsafeBuffer(points * stride);
                    if (i == 0)
                    {
                        pOutData[i] = (byte*) manager._indices.Address;
                    }
                    else
                    {
                        pOutData[i] = (byte*) manager._faceData[i].Address;
                    }
                }
            }

            //Decode primitives
            foreach (PrimitiveEntry prim in geo._primitives)
            {
                int count = prim._inputs.Count;
                //Map inputs to command sequence
                for (int i = 0; i < count; ++i)
                {
                    InputEntry inp = prim._inputs[i];

                    if (inp._outputOffset == -1)
                    {
                        pCmd[i].Cmd = 0;
                    }
                    else
                    {
                        pCmd[i].Cmd = (byte) inp._semantic;
                        pCmd[i].Index = (byte) inp._outputOffset;
                        pCmd[i].Offset = (byte)inp._offset;

                        //Assign input buffer
                        foreach (SourceEntry src in geo._sources)
                        {
                            if (src._id == inp._source)
                            {
                                pInData[inp._outputOffset] = (byte*) ((UnsafeBuffer) src._arrayData).Address;
                                break;
                            }
                        }
                    }
                }

                //Decode face data using command list
                foreach (PrimitiveFace f in prim._faces)
                {
                    fixed (ushort* p = f._pointIndices)
                    {
                        RunPrimitiveCmd(pInData, pOutData, pCmd, count, p, f._pointCount);
                    }
                }

                //Process point indices
                switch (prim._type)
                {
                    case ColladaBeginMode.triangles:
                        count = prim._faceCount * 3;
                        while (count-- > 0)
                        {
                            pTriarr[pTri++] = fIndex++;
                        }

                        break;
                    case ColladaBeginMode.trifans:
                    case ColladaBeginMode.polygons:
                    case ColladaBeginMode.polylist:
                        foreach (PrimitiveFace f in prim._faces)
                        {
                            count = f._pointCount - 2;
                            temp = fIndex;
                            fIndex += 2;
                            while (count-- > 0)
                            {
                                pTriarr[pTri++] = temp;
                                pTriarr[pTri++] = fIndex - 1;
                                pTriarr[pTri++] = fIndex++;
                            }
                        }

                        break;
                    case ColladaBeginMode.tristrips:
                        foreach (PrimitiveFace f in prim._faces)
                        {
                            count = f._pointCount;
                            fIndex += 2;
                            for (int i = 2; i < count; i++)
                            {
                                if ((i & 1) == 0)
                                {
                                    pTriarr[pTri++] = fIndex - 2;
                                    pTriarr[pTri++] = fIndex - 1;
                                    pTriarr[pTri++] = fIndex++;
                                }
                                else
                                {
                                    pTriarr[pTri++] = fIndex - 2;
                                    pTriarr[pTri++] = fIndex;
                                    pTriarr[pTri++] = fIndex++ - 1;
                                }
                            }
                        }

                        break;

                    case ColladaBeginMode.linestrips:
                        foreach (PrimitiveFace f in prim._faces)
                        {
                            count = f._pointCount - 1;
                            lIndex++;
                            while (count-- > 0)
                            {
                                pLinarr[pLin++] = lIndex - 1;
                                pLinarr[pLin++] = lIndex++;
                            }
                        }

                        break;

                    case ColladaBeginMode.lines:
                        foreach (PrimitiveFace f in prim._faces)
                        {
                            count = f._pointCount;
                            while (count-- > 0)
                            {
                                pLinarr[pLin++] = lIndex++;
                            }
                        }

                        break;
                }
            }

            return manager;
        }

        private static void RunPrimitiveCmd(byte** pIn, byte** pOut, PrimitiveDecodeCommand* pCmd, int cmdCount,
                                            ushort* pIndex, int count)
        {
            int stride = 0;
            int buffer;
            while (count-- > 0)
            {
                for (int i = 0; i < cmdCount; i++)
                {
                    buffer = pCmd[i].Index;
                    stride = Math.Max(stride, pCmd[i].Offset);
                    switch ((SemanticType) pCmd[i].Cmd)
                    {
                        case SemanticType.None:
                            break;

                        case SemanticType.VERTEX:
                            //Can't do remap table because weights haven't been assigned yet!
                            *(ushort*) pOut[buffer] = *(pIndex + pCmd[i].Offset);
                            pOut[buffer] += 2;
                            break;

                        case SemanticType.NORMAL:
                            *(Vector3*) pOut[buffer] = ((Vector3*) pIn[buffer])[*(pIndex + pCmd[i].Offset)];
                            pOut[buffer] += 12;
                            break;

                        case SemanticType.COLOR:
                            float* p = (float*) (pIn[buffer] + *(pIndex + pCmd[i].Offset) * 16);
                            byte* p2 = pOut[buffer];
                            for (int x = 0; x < 4; x++)
                            {
                                *p2++ = (byte) (*p++ * 255.0f + 0.5f);
                            }

                            pOut[buffer] = p2;
                            break;

                        case SemanticType.TEXCOORD:
                            //Flip y axis so coordinates are bottom-up
                            Vector2 v = ((Vector2*) pIn[buffer])[*(pIndex + pCmd[i].Offset)];
                            v._y = 1.0f - v._y;
                            *(Vector2*) pOut[buffer] = v;
                            pOut[buffer] += 8;
                            break;
                    }
                }
                pIndex += stride + 1;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct PrimitiveDecodeCommand
        {
            public byte Cmd;
            public byte Index;
            public byte Offset;
            public byte Padding;
        }
    }
}