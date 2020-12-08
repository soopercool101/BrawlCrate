using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.Modeling;
using BrawlLib.Modeling.Collada;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BrawlLib.Wii.Models
{
    public static unsafe class ModelEncoder
    {
        //This assumes influence list has already been cleaned
        public static void AssignNodeIndices(ModelLinker linker)
        {
            MDL0Node model = linker.Model;
            int index = 0;

            int count = model._influences.Count + linker.BoneCache.Length;

            linker._nodeCount = count;
            linker.Model._numNodes = count;
            linker.NodeCache = new IMatrixNode[count];

            //Add referenced primaries
            foreach (MDL0BoneNode bone in linker.BoneCache)
            {
                if (bone.Users.Count > 0 || bone._singleBindObjects.Count > 0)
                {
                    linker.NodeCache[bone._nodeIndex = index++] = bone;
                }
                else
                {
                    bone._nodeIndex = -1;
                }

                bone._weightCount = 0;
            }

            //Add weight groups
            foreach (Influence i in model._influences._influences)
            {
                linker.NodeCache[i._index = index++] = i;
                foreach (BoneWeight b in i.Weights)
                {
                    if (b.Bone != null)
                    {
                        b.Bone.WeightCount++;
                    }
                }
            }

            //Add remaining bones
            foreach (MDL0BoneNode bone in linker.BoneCache)
            {
                if (bone._nodeIndex == -1)
                {
                    linker.NodeCache[bone._nodeIndex = index++] = bone;
                }
            }
        }

        public static int CalcSize(ModelLinker linker)
        {
            return CalcSize(null, linker);
        }

        public static int CalcSize(Collada form, ModelLinker linker)
        {
            MDL0Node model = linker.Model;
            model._needsNrmMtxArray = model._needsTexMtxArray = false;
            model._numFacepoints = model._numTriangles = 0;

            int headerLen,
                groupLen = 0,
                tableLen = 0,
                texLen = 0,
                boneLen = 0,
                dataLen = 0,
                defLen = 0,
                assetLen = 0,
                treeLen = 0,
                mixLen = 0,
                opaLen = 0,
                xluLen = 0;

            int aInd, aLen;

            //Get header length
            switch (linker.Version)
            {
                case 0x08:
                case 0x09:
                    headerLen = 0x80;
                    break;
                case 0x0A:
                    headerLen = 0x88;
                    break;
                case 0x0B:
                    headerLen = 0x8C;
                    break;
                default:
                    headerLen = 0x80;
                    //Unsupported version. Change to 9 as default.
                    linker.Version = 9;
                    break;
            }

            //Assign node indices
            AssignNodeIndices(linker);

            //Get table length
            tableLen = (linker._nodeCount + 1) << 2;

            //Get group/data length
            List<MDLResourceType> iList = ModelLinker.IndexBank[linker.Version];
            foreach (MDLResourceType resType in iList)
            {
                IEnumerable entryList = null;
                int entries = 0;

                switch (resType)
                {
                    case MDLResourceType.Definitions:

                        //NodeTree
                        treeLen = linker.BoneCache.Length * 5;

                        //NodeMix
                        foreach (Influence i in model._influences._influences)
                        {
                            mixLen += 4;
                            foreach (BoneWeight w in i.Weights)
                            {
                                MDL0BoneNode bone = w.Bone as MDL0BoneNode;
                                if (bone != null && w.Weight != 0 && bone._nodeIndex < linker.NodeCache.Length &&
                                    bone._nodeIndex >= 0 && linker.NodeCache[bone._nodeIndex] is MDL0BoneNode)
                                {
                                    mixLen += 6;
                                }
                            }
                        }

                        foreach (MDL0BoneNode b in linker.BoneCache)
                        {
                            if (b._weightCount > 0)
                            {
                                mixLen += 5;
                            }
                        }

                        //DrawOpa and DrawXlu
                        //Get assigned materials and categorize
                        if (model._objList != null)
                        {
                            for (int i = 0; i < model._objList.Count; i++)
                            {
                                //Entries are ordered by material, not by polygon.
                                //Using the material's attached polygon list is untrustable if the definitions were corrupt on parse.
                                MDL0ObjectNode poly = model._objList[i] as MDL0ObjectNode;

                                model._numTriangles += poly._numFaces;
                                model._numFacepoints += poly._numFacepoints;

                                foreach (DrawCall c in poly._drawCalls)
                                {
                                    if (c.DrawPass == DrawCall.DrawPassType.Opaque)
                                    {
                                        opaLen += 8;
                                    }
                                    else
                                    {
                                        xluLen += 8;
                                    }
                                }
                            }
                        }

                        //Add terminate byte and set model def flags
                        if (model._hasTree = treeLen > 0)
                        {
                            treeLen++;
                            entries++;
                        }

                        if (model._hasMix = mixLen > 0)
                        {
                            mixLen++;
                            entries++;
                        }

                        if (model._hasOpa = opaLen > 0)
                        {
                            opaLen++;
                            entries++;
                        }

                        if (model._hasXlu = xluLen > 0)
                        {
                            xluLen++;
                            entries++;
                        }

                        //Align data
                        defLen += (treeLen + mixLen + opaLen + xluLen).Align(4);

                        break;

                    case MDLResourceType.Vertices:
                        if (model._vertList != null)
                        {
                            entryList = model._vertList;
                            break;
                        }
                        else
                        {
                            aInd = 0; //Set the ID
                            aLen = 1; //Offset count
                        }

                        EvalAssets:

                        List<ResourceNode> polyList = model._objList;
                        if (polyList == null)
                        {
                            break;
                        }

                        string str = "";

                        bool direct = linker._forceDirectAssets[aInd];

                        //Create asset lists
                        IList aList;
                        switch (aInd) //Switch by the set ID
                        {
                            case 0:
                                aList = linker._vertices = new List<VertexCodec>(polyList.Count);
                                str = "Vertices ";
                                break;
                            case 1:
                                aList = linker._normals = new List<VertexCodec>(polyList.Count);
                                str = "Normals ";
                                break;
                            case 2:
                                aList = linker._colors = new List<ColorCodec>(polyList.Count);
                                str = "Colors ";
                                break;
                            default:
                                aList = linker._uvs = new List<VertexCodec>(polyList.Count);
                                str = "UVs ";
                                break;
                        }

                        aLen += aInd;
                        for (int i = 0; i < polyList.Count; i++)
                        {
                            MDL0ObjectNode obj = polyList[i] as MDL0ObjectNode;
                            for (int x = aInd; x < aLen; x++)
                            {
                                if (obj._manager._faceData[x] != null)
                                {
                                    //Remap color nodes
                                    if (x == 2 || x == 3)
                                    {
                                        if (Collada._importOptions._rmpClrs)
                                        {
                                            obj._elementIndices[x] = -1;
                                            foreach (MDL0ObjectNode thatObj in polyList.OrderBy(c =>
                                                -((MDL0ObjectNode) c)._manager.GetColors(x - 2, false).Length))
                                            {
                                                //Only compare up to the current object
                                                if (thatObj == obj)
                                                {
                                                    break;
                                                }

                                                RGBAPixel[] thatArr = thatObj._manager.GetColors(x - 2, false);
                                                RGBAPixel[] thisArr = obj._manager.GetColors(x - 2, false);
                                                bool equals = true;
                                                if (thisArr.Length == thatArr.Length)
                                                {
                                                    for (int n = 0; n < thisArr.Length; n++)
                                                    {
                                                        if (thisArr[n] != thatArr[n])
                                                        {
                                                            equals = false;
                                                            break;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (RGBAPixel px in thisArr)
                                                    {
                                                        if (Array.IndexOf(thatArr, px) < 0)
                                                        {
                                                            equals = false;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (equals)
                                                {
                                                    //Found a match
                                                    obj._elementIndices[x] = thatObj._elementIndices[x];
                                                    obj._manager._newClrObj[x - 2] = thatObj.Index;
                                                    break;
                                                }
                                            }

                                            if (obj._elementIndices[x] != -1)
                                            {
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            obj._manager._newClrObj[x - 2] = i;
                                        }
                                    }

                                    obj._elementIndices[x] = (short) aList.Count;

                                    form?.Say("Encoding " + str + (x - aInd) + " for Object " + i + ": " + obj.Name);

                                    VertexCodec vert;
                                    switch (aInd)
                                    {
                                        case 0:
                                            vert = new VertexCodec(obj._manager.GetVertices(false), false,
                                                Collada._importOptions._fltVerts);
                                            aList.Add(vert);
                                            if (!direct)
                                            {
                                                assetLen += vert._dataLen.Align(0x20) + 0x40;
                                            }

                                            break;
                                        case 1:
                                            vert = new VertexCodec(obj._manager.GetNormals(false), false,
                                                Collada._importOptions._fltNrms);
                                            aList.Add(vert);
                                            if (!direct)
                                            {
                                                assetLen += vert._dataLen.Align(0x20) + 0x20;
                                            }

                                            break;
                                        case 2:
                                            ColorCodec col = new ColorCodec(obj._manager.GetColors(x - 2, false));
                                            aList.Add(col);
                                            if (!direct)
                                            {
                                                assetLen += col._dataLen.Align(0x20) + 0x20;
                                            }

                                            break;
                                        default:
                                            vert = new VertexCodec(obj._manager.GetUVs(x - 4, false),
                                                Collada._importOptions._fltUVs);
                                            aList.Add(vert);
                                            if (!direct)
                                            {
                                                assetLen += vert._dataLen.Align(0x20) + 0x40;
                                            }

                                            break;
                                    }
                                }
                                else
                                {
                                    obj._elementIndices[x] = -1;
                                }
                            }
                        }

                        if (!direct)
                        {
                            entries = aList.Count;
                        }

                        break;
                    case MDLResourceType.Normals:
                        if (model._normList != null)
                        {
                            entryList = model._normList;
                        }
                        else
                        {
                            aInd = 1; //Set the ID
                            aLen = 1; //Offset count
                            goto EvalAssets;
                        }

                        break;
                    case MDLResourceType.Colors:
                        if (model._colorList != null)
                        {
                            entryList = model._colorList;
                        }
                        else
                        {
                            if (Collada._importOptions._useOneNode)
                            {
                                HashSet<RGBAPixel> pixels = new HashSet<RGBAPixel>();
                                if (model._objList != null)
                                {
                                    foreach (MDL0ObjectNode obj in model._objList)
                                    {
                                        for (int i = 0; i < 2; i++)
                                        {
                                            RGBAPixel[] arr = obj._manager.GetColors(i, false);
                                            if (arr.Length > 0)
                                            {
                                                obj._elementIndices[i + 2] = 0;
                                                foreach (RGBAPixel p in arr)
                                                {
                                                    pixels.Add(p);
                                                }
                                            }
                                            else
                                            {
                                                obj._elementIndices[i + 2] = -1;
                                            }
                                        }
                                    }
                                }

                                List<RGBAPixel> le = pixels.ToList();
                                le.Sort();

                                if (le.Count == 0)
                                {
                                    break;
                                }

                                Collada._importOptions._singleColorNodeEntries = le.ToArray();

                                ColorCodec col = new ColorCodec(Collada._importOptions._singleColorNodeEntries);
                                linker._colors = new List<ColorCodec> {col};
                                assetLen += col._dataLen.Align(0x20) + 0x20;
                                entries = 1;
                            }
                            else
                            {
                                aInd = 2; //Set the ID
                                aLen = 2; //Offset count
                                goto EvalAssets;
                            }
                        }

                        break;
                    case MDLResourceType.UVs:
                        if (model._uvList != null)
                        {
                            entryList = model._uvList;
                        }
                        else
                        {
                            aInd = 4; //Set the ID
                            aLen = 8; //Offset count
                            goto EvalAssets;
                        }

                        break;

                    case MDLResourceType.Bones:
                        int index = 0;
                        foreach (MDL0BoneNode b in linker.BoneCache)
                        {
                            form?.Say("Calculating the size of the Bones - " + b.Name);

                            b._entryIndex = index++;
                            boneLen += b.CalculateSize(true);
                        }

                        entries = linker.BoneCache.Length;
                        break;

                    case MDLResourceType.Materials:
                        if (model._matList != null)
                        {
                            entries = model._matList.Count;
                        }

                        break;

                    case MDLResourceType.Objects:
                        if (model._objList != null)
                        {
                            entryList = model._objList;
                            foreach (MDL0ObjectNode n in model._objList)
                            {
                                if (n.NormalNode != null || n._manager._faceData[1] != null)
                                {
                                    model._needsNrmMtxArray = true;
                                }

                                if (n.HasTexMtx)
                                {
                                    model._needsTexMtxArray = true;
                                }
                            }
                        }

                        break;

                    case MDLResourceType.Shaders:
                        if (model._matList != null && (entryList = model.GetUsedShaders()) != null)
                        {
                            entries = model._matList.Count;
                        }

                        break;

                    case MDLResourceType.Textures:
                        if (model._texList != null)
                        {
                            List<MDL0TextureNode> texNodes = new List<MDL0TextureNode>();
                            foreach (MDL0TextureNode tex in model._texList)
                            {
                                texNodes.Add(tex);
                                texLen += tex._references.Count * 8 + 4;
                            }

                            entries = (linker._texList = texNodes).Count;
                        }

                        break;

                    case MDLResourceType.Palettes:
                        if (model._pltList != null)
                        {
                            List<MDL0TextureNode> pltNodes = new List<MDL0TextureNode>();
                            foreach (MDL0TextureNode plt in model._pltList)
                            {
                                pltNodes.Add(plt);
                                texLen += plt._references.Count * 8 + 4;
                            }

                            entries = (linker._pltList = pltNodes).Count;
                        }

                        break;
                }

                if (entryList != null)
                {
                    int index = 0;
                    foreach (MDL0EntryNode e in entryList)
                    {
                        if (form != null)
                        {
                            if (resType == MDLResourceType.Objects)
                            {
                                form.Say("Encoding the " + resType + " - " + e.Name);
                            }
                            else
                            {
                                form.Say("Calculating the size of the " + resType + " - " + e.Name);
                            }
                        }

                        e._entryIndex = index++;
                        dataLen += e.CalculateSize(true);
                    }

                    if (entries == 0)
                    {
                        entries = index;
                    }
                }

                if (entries > 0)
                {
                    groupLen += entries * 0x10 + 0x18;
                }
            }

            //Align the materials perfectly using the data length
            int temp = 0;
            if (model._matList != null && iList.IndexOf(MDLResourceType.Materials) != -1)
            {
                int index = 0;
                MDL0MaterialNode prev = null;
                foreach (MDL0MaterialNode e in model._matList)
                {
                    form?.Say("Calculating the size of the Materials - " + e.Name);

                    if (index != 0)
                    {
                        e._mdlOffset = (prev = (MDL0MaterialNode) model._matList[index - 1])._mdlOffset +
                                       prev._calcSize;
                    }
                    else if ((temp =
                        (e._mdlOffset = headerLen + tableLen + groupLen + texLen + defLen + boneLen).Align(
                            0x10)) != e._mdlOffset)
                    {
                        e._dataAlign = temp - e._mdlOffset;
                    }

                    e._entryIndex = index++;
                    dataLen += e.CalculateSize(true);
                }
            }

            if (model._isImport && model._objList != null)
            {
                foreach (MDL0ObjectNode obj1 in model._objList)
                {
                    if (obj1?._drawCalls == null || obj1._drawCalls.Count == 0)
                    {
                        continue;
                    }

                    MDL0MaterialNode p = obj1._drawCalls[0].MaterialNode;
                    if (p == null)
                    {
                        continue;
                    }

                    //Set materials to use register color if option set
                    if (!Collada._importOptions._useReg &&
                        linker._colors != null &&
                        linker._colors.Count > 0)
                    {
                        p.C1AlphaMaterialSource = GXColorSrc.Vertex;
                        p.C1ColorMaterialSource = GXColorSrc.Vertex;
                    }
                    else
                    {
                        p.C1MaterialColor = Collada._importOptions._dfltClr;
                        p.C1ColorMaterialSource = GXColorSrc.Register;
                        p.C1AlphaMaterialSource = GXColorSrc.Register;
                    }
                }
            }

            return
                (linker._headerLen = headerLen) +
                (linker._tableLen = tableLen) +
                (linker._groupLen = groupLen) +
                (linker._texLen = texLen) +
                (linker._defLen = defLen) +
                (linker._boneLen = boneLen) +
                (linker._assetLen = assetLen) +
                (linker._dataLen = dataLen) +
                (linker.Version > 9 ? model._userEntries.GetSize() : 0);
        }

        internal static void Build(ModelLinker linker, MDL0Header* header, int length, bool force)
        {
            Build(null, linker, header, length, force);
        }

        internal static void Build(Collada form, ModelLinker linker, MDL0Header* header, int length, bool force)
        {
            byte* groupAddr = (byte*) header + linker._headerLen + linker._tableLen;
            byte* dataAddr = groupAddr + linker._groupLen + linker._texLen; //Definitions start here
            byte* assetAddr = dataAddr + linker._defLen + linker._boneLen + linker._dataLen;

            linker.Header = header;

            form?.Say("Writing header...");

            //Create new model header
            *header = new MDL0Header(length, linker.Version);

            form?.Say("Writing node table...");

            //Write node table, assign node ids
            WriteNodeTable(linker);

            form?.Say("Writing definitions...");

            //Write def table
            WriteDefs(linker, ref groupAddr, ref dataAddr);

            //Set format list for each polygon's UVAT groups
            SetFormatLists(linker);

            //Write assets first, but only if the model is an import
            if (linker.Model._isImport)
            {
                WriteAssets(form, linker, ref assetAddr);
            }

            //Write groups
            linker.Write(form, ref groupAddr, ref dataAddr, force);

            //Write user entries
            if (linker.Model._userEntries.Count > 0 && linker.Version > 9)
            {
                header->UserDataOffset = (int) dataAddr - (int) header;
                linker.Model._userEntries.Write(header->UserData);
            }
            else
            {
                header->UserDataOffset = 0;
            }

            //Write textures
            WriteTextures(linker, ref groupAddr);

            //Set box min and box max
            if (linker.Model._isImport)
            {
                SetBox(linker);
            }

            //Store group offsets
            linker.Finish();

            //Set new properties
            *header->Properties = new MDL0Props(linker.Version, linker.Model._numFacepoints, linker.Model._numTriangles,
                linker.Model._numNodes, linker.Model._scalingRule, linker.Model._texMtxMode,
                linker.Model._needsNrmMtxArray, linker.Model._needsTexMtxArray, linker.Model._enableExtents,
                linker.Model._envMtxMode, linker.Model._extents.Min, linker.Model._extents.Max);
        }

        private static void WriteNodeTable(ModelLinker linker)
        {
            bint* ptr = (bint*) ((byte*) linker.Header + linker._headerLen);
            int len = linker._nodeCount;
            int i = 0;

            //Set length
            *ptr++ = len;

            //Write indices
            while (i < len)
            {
                IMatrixNode n = linker.NodeCache[i++];
                if (n.IsPrimaryNode)
                {
                    *ptr++ = ((MDL0BoneNode) n)._entryIndex;
                }
                else
                {
                    *ptr++ = -1;
                }
            }
        }

        private static void WriteDefs(ModelLinker linker, ref byte* pGroup, ref byte* pData)
        {
            MDL0Node mdl = linker.Model;

            //This should never happen!
            if (!mdl._hasMix && !mdl._hasOpa && !mdl._hasTree && !mdl._hasXlu)
            {
                return;
            }

            ResourceNode[] polyList = null;
            if (mdl._objList != null)
            {
                polyList = new ResourceNode[mdl._objList.Count];
                Array.Copy(mdl._objList.ToArray(), polyList, mdl._objList.Count);
            }

            DrawCall drawCall;
            int entryCount = 0;
            byte* floor = pData;
            int dataLen;

            ResourceGroup* group = linker.Defs = (ResourceGroup*) pGroup;
            ResourceEntry* entry = &group->_first + 1;

            //NodeTree
            if (mdl._hasTree)
            {
                //Write group entry
                entry[entryCount++]._dataOffset = (int) (pData - pGroup);

                int bCount = linker.BoneCache.Length;
                for (int i = 0; i < bCount; i++)
                {
                    MDL0BoneNode bone = linker.BoneCache[i];

                    *pData = 2; //Entry tag
                    *(bushort*) (pData + 1) = (ushort) bone._entryIndex;
                    *(bushort*) (pData + 3) =
                        (ushort) (bone._parent is MDL0BoneNode ? ((MDL0BoneNode) bone._parent)._nodeIndex : 0);
                    pData += 5; //Advance
                }

                *pData++ = 1; //Terminate
            }

            //NodeMix
            //Only weight references go here.
            //First list bones used by weight groups, in bone order
            //Then list weight groups that use bones. Ordered by entry count.
            if (mdl._hasMix)
            {
                //Write group entry
                entry[entryCount++]._dataOffset = (int) (pData - pGroup);

                //Add bones first (using flat bone list)
                foreach (MDL0BoneNode b in linker.BoneCache)
                {
                    if (b._weightCount > 0)
                    {
                        *pData = 5; //Tag
                        *(bushort*) (pData + 1) = (ushort) b._nodeIndex;
                        *(bushort*) (pData + 3) = (ushort) b._entryIndex;
                        pData += 5; //Advance
                    }
                }

                //Add weight groups (using sorted influence list)
                foreach (Influence i in mdl._influences._influences)
                {
                    *pData++ = 3; //Tag
                    *(bushort*) pData = (ushort) i._index;
                    pData += 2;

                    byte* countAddr = pData++;
                    byte count = 0;
                    foreach (BoneWeight w in i.Weights)
                    {
                        MDL0BoneNode bone = w.Bone as MDL0BoneNode;
                        if (bone == null || w.Weight == 0 || bone._nodeIndex >= linker.NodeCache.Length ||
                            bone._nodeIndex < 0)
                        {
                            continue;
                        }

                        *(bushort*) pData = (ushort) bone._nodeIndex;
                        *(bfloat*) (pData + 2) = w.Weight;
                        pData += 6; //Advance

                        if (linker.NodeCache[bone._nodeIndex] is MDL0BoneNode)
                        {
                            count++;
                        }
                    }

                    *countAddr = count;
                }

                *pData++ = 1; //Terminate
            }

            //DrawOpa
            if (mdl._hasOpa && polyList != null)
            {
                DrawCall[] objects = polyList.SelectMany(x => ((MDL0ObjectNode) x)._drawCalls)
                    .Where(x => x.DrawPass == DrawCall.DrawPassType.Opaque).ToArray();

                Array.Sort(objects, DrawCall.DrawCompare);

                //Write group entry
                entry[entryCount++]._dataOffset = (int) (pData - pGroup);

                for (int i = 0; i < objects.Length; i++)
                {
                    drawCall = objects[i];

                    *pData = 4; //Tag
                    *(bushort*) (pData + 1) = (ushort) drawCall.MaterialNode._entryIndex;
                    *(bushort*) (pData + 3) = (ushort) drawCall._parentObject._entryIndex;
                    *(bushort*) (pData + 5) =
                        (ushort) (drawCall.VisibilityBoneNode != null ? drawCall.VisibilityBoneNode.BoneIndex : 0);
                    pData[7] = drawCall.DrawPriority;
                    pData += 8; //Advance
                }

                *pData++ = 1; //Terminate
            }

            //DrawXlu
            if (mdl._hasXlu && polyList != null)
            {
                DrawCall[] objects = polyList.SelectMany(x => ((MDL0ObjectNode) x)._drawCalls)
                    .Where(x => x.DrawPass == DrawCall.DrawPassType.Transparent).ToArray();

                Array.Sort(objects, DrawCall.DrawCompare);

                //Write group entry
                entry[entryCount++]._dataOffset = (int) (pData - pGroup);

                for (int i = 0; i < objects.Length; i++)
                {
                    drawCall = objects[i];

                    *pData = 4; //Tag
                    *(bushort*) (pData + 1) = (ushort) drawCall.MaterialNode._entryIndex;
                    *(bushort*) (pData + 3) = (ushort) drawCall._parentObject._entryIndex;
                    *(bushort*) (pData + 5) =
                        (ushort) (drawCall.VisibilityBoneNode != null ? drawCall.VisibilityBoneNode.BoneIndex : 0);
                    pData[7] = drawCall.DrawPriority;
                    pData += 8; //Advance
                }

                *pData++ = 1; //Terminate
            }

            //Align data
            dataLen = (int) (pData - floor);
            while ((dataLen++ & 3) != 0)
            {
                *pData++ = 0;
            }

            //Set header
            *group = new ResourceGroup(entryCount);

            //Advance group poiner
            pGroup += group->_totalSize;
        }

        //Write assets will only be used for model imports.
        private static void WriteAssets(Collada form, ModelLinker linker, ref byte* pData)
        {
            int index;
            MDL0Node model = linker.Model;

            if (linker._vertices != null && linker._vertices.Count != 0)
            {
                model.LinkGroup(new MDL0GroupNode(MDLResourceType.Vertices));
                model._vertGroup._parent = model;

                index = 0;
                foreach (VertexCodec c in linker._vertices)
                {
                    MDL0VertexNode node = new MDL0VertexNode
                    {
                        _name = model.Name + "_" + model._objList[index]._name
                    };

                    MDL0ObjectNode n = (MDL0ObjectNode) model._objList[index];
                    if (n._drawCalls.Count > 0 && n._drawCalls[0].MaterialNode != null)
                    {
                        node._name += "_" + ((MDL0ObjectNode) model._objList[index])._drawCalls[0].MaterialNode._name;
                    }

                    form?.Say("Writing Vertices - " + node.Name);

                    MDL0VertexData* header = (MDL0VertexData*) pData;
                    header->_dataLen = c._dataLen.Align(0x20) + 0x40;
                    header->_dataOffset = 0x40;
                    header->_index = index++;
                    header->_isXYZ = c._hasZ ? 1 : 0;
                    header->_type = (int) c._type;
                    header->_divisor = (byte) c._scale;
                    header->_entryStride = (byte) c._dstStride;
                    header->_numVertices = (ushort) c._dstCount;
                    header->_eMin = c._min;
                    header->_eMax = c._max;
                    header->_pad1 = header->_pad2 = 0;

                    c.Write(pData + 0x40);

                    node._replSrc = node._replUncompSrc = new DataSource(header, header->_dataLen);
                    model._vertGroup.AddChild(node, false);

                    pData += header->_dataLen;
                }
            }

            if (linker._normals != null && linker._normals.Count != 0)
            {
                model.LinkGroup(new MDL0GroupNode(MDLResourceType.Normals));
                model._normGroup._parent = model;

                index = 0;
                foreach (VertexCodec c in linker._normals)
                {
                    MDL0NormalNode node = new MDL0NormalNode
                    {
                        _name = model.Name + "_" + model._objList[index]._name
                    };
                    MDL0ObjectNode n = (MDL0ObjectNode) model._objList[index];
                    if (n._drawCalls.Count > 0 && n._drawCalls[0].MaterialNode != null)
                    {
                        node._name += "_" + ((MDL0ObjectNode) model._objList[index])._drawCalls[0].MaterialNode._name;
                    }

                    form?.Say("Writing Normals - " + node.Name);

                    MDL0NormalData* header = (MDL0NormalData*) pData;
                    header->_dataLen = c._dataLen.Align(0x20) + 0x20;
                    header->_dataOffset = 0x20;
                    header->_index = index++;
                    header->_isNBT = 0;
                    header->_type = (int) c._type;
                    header->_divisor = (byte) c._scale;
                    header->_entryStride = (byte) c._dstStride;
                    header->_numVertices = (ushort) c._dstCount;

                    c.Write(pData + 0x20);

                    node._replSrc = node._replUncompSrc = new DataSource(header, header->_dataLen);
                    model._normGroup.AddChild(node, false);

                    pData += header->_dataLen;
                }
            }

            if (linker._colors != null && linker._colors.Count != 0)
            {
                model.LinkGroup(new MDL0GroupNode(MDLResourceType.Colors));
                model._colorGroup._parent = model;

                index = 0;
                foreach (ColorCodec c in linker._colors)
                {
                    MDL0ColorNode node = new MDL0ColorNode
                    {
                        _name = model.Name + "_" + model._objList[index]._name
                    };
                    MDL0ObjectNode n = (MDL0ObjectNode) model._objList[index];
                    if (n._drawCalls.Count > 0 && n._drawCalls[0].MaterialNode != null)
                    {
                        node._name += "_" + ((MDL0ObjectNode) model._objList[index])._drawCalls[0].MaterialNode._name;
                    }

                    form?.Say("Writing Colors - " + node.Name);

                    MDL0ColorData* header = (MDL0ColorData*) pData;
                    header->_dataLen = c._dataLen.Align(0x20) + 0x20;
                    header->_dataOffset = 0x20;
                    header->_index = index++;
                    header->_isRGBA = c._hasAlpha ? 1 : 0;
                    header->_format = (int) c._outType;
                    header->_entryStride = (byte) c._dstStride;
                    header->_pad = 0;
                    header->_numEntries = (ushort) c._dstCount;

                    c.Write(pData + 0x20);

                    node._replSrc = node._replUncompSrc = new DataSource(header, header->_dataLen);
                    model._colorGroup.AddChild(node, false);

                    pData += header->_dataLen;
                }
            }

            if (linker._uvs != null && linker._uvs.Count != 0)
            {
                model.LinkGroup(new MDL0GroupNode(MDLResourceType.UVs));
                model._uvGroup._parent = model;

                index = 0;
                foreach (VertexCodec c in linker._uvs)
                {
                    MDL0UVNode node = new MDL0UVNode {_name = "#" + index};

                    form?.Say("Writing UVs - " + node.Name);

                    MDL0UVData* header = (MDL0UVData*) pData;
                    header->_dataLen = c._dataLen.Align(0x20) + 0x40;
                    header->_dataOffset = 0x40;
                    header->_index = index++;
                    header->_format = (int) c._type;
                    header->_divisor = (byte) c._scale;
                    header->_isST = 1;
                    header->_entryStride = (byte) c._dstStride;
                    header->_numEntries = (ushort) c._dstCount;
                    header->_min = (Vector2) c._min;
                    header->_max = (Vector2) c._max;
                    header->_pad1 = header->_pad2 = header->_pad3 = header->_pad4 = 0;

                    c.Write(pData + 0x40);

                    node._replSrc = node._replUncompSrc = new DataSource(header, header->_dataLen);
                    model._uvGroup.AddChild(node, false);

                    pData += header->_dataLen;
                }
            }

            //Clean groups
            if (model._vertList != null && model._vertList.Count > 0)
            {
                model._children.Add(model._vertGroup);
                linker.Groups[(int) (MDLResourceType) Enum.Parse(typeof(MDLResourceType), model._vertGroup.Name)] =
                    model._vertGroup;
            }
            else
            {
                model.UnlinkGroup(model._vertGroup);
            }

            if (model._normList != null && model._normList.Count > 0)
            {
                model._children.Add(model._normGroup);
                linker.Groups[(int) (MDLResourceType) Enum.Parse(typeof(MDLResourceType), model._normGroup.Name)] =
                    model._normGroup;
            }
            else
            {
                model.UnlinkGroup(model._normGroup);
            }

            if (model._uvList != null && model._uvList.Count > 0)
            {
                model._children.Add(model._uvGroup);
                linker.Groups[(int) (MDLResourceType) Enum.Parse(typeof(MDLResourceType), model._uvGroup.Name)] =
                    model._uvGroup;
            }
            else
            {
                model.UnlinkGroup(model._uvGroup);
            }

            if (model._colorList != null && model._colorList.Count > 0)
            {
                model._children.Add(model._colorGroup);
                linker.Groups[(int) (MDLResourceType) Enum.Parse(typeof(MDLResourceType), model._colorGroup.Name)] =
                    model._colorGroup;
            }
            else
            {
                model.UnlinkGroup(model._colorGroup);
            }

            //Link sets
            if (model._objList != null)
            {
                foreach (MDL0ObjectNode poly in model._objList)
                {
                    if (poly._elementIndices[0] != -1 && model._vertList != null &&
                        model._vertList.Count > poly._elementIndices[0])
                    {
                        poly._vertexNode = (MDL0VertexNode) model._vertGroup._children[poly._elementIndices[0]];
                    }

                    if (poly._elementIndices[1] != -1 && model._normList != null &&
                        model._normList.Count > poly._elementIndices[1])
                    {
                        poly._normalNode = (MDL0NormalNode) model._normGroup._children[poly._elementIndices[1]];
                    }

                    for (int i = 2; i < 4; i++)
                    {
                        if (poly._elementIndices[i] != -1 && model._colorList != null &&
                            model._colorList.Count > poly._elementIndices[i])
                        {
                            poly._colorSet[i - 2] =
                                (MDL0ColorNode) model._colorGroup._children[poly._elementIndices[i]];
                        }
                    }

                    for (int i = 4; i < 12; i++)
                    {
                        if (poly._elementIndices[i] != -1 && model._uvList != null &&
                            model._uvList.Count > poly._elementIndices[i])
                        {
                            poly._uvSet[i - 4] = (MDL0UVNode) model._uvGroup._children[poly._elementIndices[i]];
                        }
                    }
                }
            }
        }

        //Materials must already be written. Do this last!
        private static void WriteTextures(ModelLinker linker, ref byte* pGroup)
        {
            //metal00 is apparently built last

            ResourceGroup* pTexGroup = null;
            ResourceEntry* pTexEntry = null;
            if (linker._texList != null && linker._texList.Count > 0)
            {
                linker.Textures = pTexGroup = (ResourceGroup*) pGroup;
                *pTexGroup = new ResourceGroup(linker._texList.Count);
                pTexEntry = &pTexGroup->_first + 1;
                pGroup += pTexGroup->_totalSize;
            }

            ResourceGroup* pPltGroup = null;
            ResourceEntry* pPltEntry = null;
            if (linker._pltList != null && linker._pltList.Count > 0)
            {
                linker.Palettes = pPltGroup = (ResourceGroup*) pGroup;
                *pPltGroup = new ResourceGroup(linker._pltList.Count);
                pPltEntry = &pPltGroup->_first + 1;
                pGroup += pPltGroup->_totalSize;
            }

            bint* pData = (bint*) pGroup;
            int offset;

            //Textures
            List<MDL0TextureNode> list = linker._texList;
            if (list != null)
            {
                list.Sort(); //Alphabetical order
                if (pTexGroup != null)
                {
                    foreach (MDL0TextureNode t in list)
                    {
                        offset = (int) pData;
                        (pTexEntry++)->_dataOffset = offset - (int) pTexGroup;
                        *pData++ = t._references.Count;
                        foreach (MDL0MaterialRefNode mat in t._references)
                        {
                            *pData++ = (int) mat.Material.WorkingUncompressed.Address - offset;
                            *pData++ = (int) mat.WorkingUncompressed.Address - offset;
                        }
                    }
                }
            }

            //Palettes
            list = linker._pltList;
            if (list != null)
            {
                list.Sort(); //Alphabetical order
                if (pPltGroup != null)
                {
                    foreach (MDL0TextureNode t in list)
                    {
                        offset = (int) pData;
                        (pPltEntry++)->_dataOffset = offset - (int) pPltGroup;
                        *pData++ = t._references.Count;
                        foreach (MDL0MaterialRefNode mat in t._references)
                        {
                            *pData++ = (int) mat.Material.WorkingUncompressed.Address - offset;
                            *pData++ = (int) mat.WorkingUncompressed.Address - offset;
                        }
                    }
                }
            }
        }

        private static void SetBox(ModelLinker linker)
        {
            linker.Model.CalculateBoundingBoxes();
            if (linker.Model._objList != null)
            {
                linker.Model._numFacepoints = 0;
                linker.Model._numTriangles = 0;
                foreach (MDL0ObjectNode n in linker.Model._objList)
                {
                    linker.Model._numFacepoints += n._numFacepoints;
                    linker.Model._numTriangles += n._numFaces;
                }
            }
        }

        private static void SetFormatLists(ModelLinker linker)
        {
            if (linker.Model._objList != null)
            {
                for (int i = 0; i < linker.Model._objList.Count; i++)
                {
                    MDL0ObjectNode poly = (MDL0ObjectNode) linker.Model._objList[i];
                    poly._manager.SetFormatList(poly, linker);
                }
            }
        }
    }
}