using BrawlLib.Internal;
using BrawlLib.Modeling;
using BrawlLib.Modeling.Collada;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using MR = BrawlLib.Wii.Models.MDLResourceType;

namespace BrawlLib.Wii.Models
{
    public enum MDLResourceType : int
    {
        Definitions,
        Bones,
        Vertices,
        Normals,
        Colors,
        UVs,
        Materials,
        Shaders,
        Objects,
        Textures,
        Palettes,
        FurVectors,
        FurLayerCoords
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe class ModelLinker // : IDisposable
    {
        #region Linker lists

        internal const int BankLen = 13;

        internal static bool SpecialRebuildData(int typeIndex)
        {
            if (typeIndex == 0 || typeIndex == 9 || typeIndex == 10)
            {
                return true;
            }

            return false;
        }

        internal static readonly Type[] TypeBank = new Type[]
        {
            typeof(MDL0DefNode), //0, special handling
            typeof(MDL0BoneNode),
            typeof(MDL0VertexNode),
            typeof(MDL0NormalNode),
            typeof(MDL0ColorNode),
            typeof(MDL0UVNode),
            typeof(MDL0MaterialNode),
            typeof(MDL0ShaderNode),
            typeof(MDL0ObjectNode),
            typeof(MDL0TextureNode), //9, special handling
            typeof(MDL0TextureNode), //10, special handling
            typeof(MDL0FurVecNode),
            typeof(MDL0FurPosNode)
        };

        internal static readonly MR[] OrderBank = new MR[]
        {
            MR.Textures,
            MR.Palettes,
            MR.Definitions,
            MR.Bones,
            MR.FurVectors,
            MR.FurLayerCoords,
            MR.Materials,
            MR.Shaders,
            MR.Objects,
            MR.Vertices,
            MR.Normals,
            MR.Colors,
            MR.UVs
        };

        public static readonly List<MR>[] IndexBank = new List<MR>[]
        {
            null, //0
            null, //1
            null, //2
            null, //3
            null, //4
            null, //5
            null, //6
            null, //7
            new List<MR>(new MR[]
            {
                MR.Definitions, MR.Bones, MR.Vertices, MR.Normals, MR.Colors, MR.UVs, MR.Materials, MR.Shaders,
                MR.Objects, MR.Textures, MR.Palettes
            }),
            new List<MR>(new MR[]
            {
                MR.Definitions, MR.Bones, MR.Vertices, MR.Normals, MR.Colors, MR.UVs, MR.Materials, MR.Shaders,
                MR.Objects, MR.Textures, MR.Palettes
            }),
            new List<MR>(new MR[]
            {
                MR.Definitions, MR.Bones, MR.Vertices, MR.Normals, MR.Colors, MR.UVs, MR.FurVectors, MR.FurLayerCoords,
                MR.Materials, MR.Shaders, MR.Objects, MR.Textures, MR.Palettes
            }),
            new List<MR>(new MR[]
            {
                MR.Definitions, MR.Bones, MR.Vertices, MR.Normals, MR.Colors, MR.UVs, MR.FurVectors, MR.FurLayerCoords,
                MR.Materials, MR.Shaders, MR.Objects, MR.Textures, MR.Palettes
            })
        };

        #endregion

        public MDL0Header* Header;

        public int Version = 9;

        //Build relocation offsets in this order:
        public ResourceGroup* Defs;      //1
        public ResourceGroup* Bones;     //2
        public ResourceGroup* Vertices;  //6
        public ResourceGroup* Normals;   //7
        public ResourceGroup* Colors;    //8
        public ResourceGroup* UVs;       //9
        public ResourceGroup* Materials; //3
        public ResourceGroup* Shaders;   //4
        public ResourceGroup* Objects;   //5
        public ResourceGroup* Textures;  //10
        public ResourceGroup* Palettes;  //11
        public ResourceGroup* FurVectors;
        public ResourceGroup* FurLayerCoords;

        public MDL0GroupNode[] Groups = new MDL0GroupNode[BankLen];

        public MDL0Node Model;
        public int _headerLen, _tableLen, _groupLen, _texLen, _boneLen, _dataLen, _defLen, _assetLen;
        public List<MDL0TextureNode> _texList, _pltList;
        public int _nodeCount;
        public MDL0BoneNode[] BoneCache;
        public IMatrixNode[] NodeCache;

        public List<VertexCodec> _vertices;
        public List<VertexCodec> _normals;
        public List<ColorCodec> _colors;
        public List<VertexCodec> _uvs;

        public bool[] _forceDirectAssets = new bool[12];

        private ModelLinker()
        {
        }

        public ModelLinker(MDL0Header* pModel)
        {
            Header = pModel;
            Version = pModel->_header._version;
            NodeCache = pModel->Properties == null
                ? new IMatrixNode[0]
                : new IMatrixNode[pModel->Properties->_numNodes];
            BoneCache = new MDL0BoneNode[0];

            bint* offsets = (bint*) ((byte*) pModel + 0x10);
            if (Version >= 8 && Version <= 11)
            {
                List<MR> iList = IndexBank[Version];
                int groupCount = iList.Count;
                int offset;

                //Extract resource addresses
                fixed (ResourceGroup** gList = &Defs)
                {
                    for (int i = 0; i < groupCount; i++)
                    {
                        if ((offset = offsets[i]) > 0)
                        {
                            gList[(int) iList[i]] = (ResourceGroup*) ((byte*) pModel + offset);
                        }
                    }
                }
            }
        }

        public void RegenerateBoneCache(bool remake = false)
        {
            if (Model?._boneGroup != null)
            {
                if (remake)
                {
                    BoneCache = Model._boneGroup.FindChildrenByType(null, ResourceType.MDL0Bone)
                        .Select(x => x as MDL0BoneNode).ToArray();
                }
                else
                {
                    BoneCache = Model._boneGroup.FindChildrenByType(null, ResourceType.MDL0Bone)
                        .Select(x => x as MDL0BoneNode).OrderBy(x => x.BoneIndex).ToArray();
                }
            }
            else
            {
                BoneCache = new MDL0BoneNode[0];
            }

            int i = 0;
            foreach (MDL0BoneNode b in BoneCache)
            {
                b._entryIndex = i++;
            }
        }

        public static ModelLinker Prepare(MDL0Node model)
        {
            ModelLinker linker = new ModelLinker
            {
                Model = model,
                Version = model._version
            };

            //Get flattened bone list and assign it to bone cache
            linker.RegenerateBoneCache();

            MR resType;
            int index;
            List<MR> iList = IndexBank[model._version];

            foreach (MDL0GroupNode group in model.Children)
            {
                //If version contains resource type, add it to group list
                resType = (MR) Enum.Parse(typeof(MR), group.Name);
                if ((index = iList.IndexOf(resType)) >= 0)
                {
                    linker.Groups[(int) resType] = group;
                }
            }

            return linker;
        }

        public void Write(Collada form, ref byte* pGroup, ref byte* pData, bool force)
        {
            MDL0GroupNode group;
            ResourceGroup* pGrp;
            ResourceEntry* pEntry;
            int len;

            //Write data in the order it appears
            foreach (MR resType in OrderBank)
            {
                if ((@group = Groups[(int) resType]) == null || SpecialRebuildData((int) resType))
                {
                    continue;
                }

                if (resType == MR.Bones)
                {
                    form?.Say("Writing Bones");

                    MDL0Bone* pBone = (MDL0Bone*) pData;
                    foreach (MDL0BoneNode e in BoneCache)
                    {
                        len = e._calcSize;
                        e.Rebuild(pData, len, true);
                        pData += len;
                    }

                    //Loop through after all bones are written
                    //and set header offsets to related bones
                    foreach (MDL0BoneNode e in BoneCache)
                    {
                        e.CalculateOffsets();
                    }
                }
                else if (resType == MR.Shaders)
                {
                    MDL0GroupNode mats = Groups[(int) MR.Materials];
                    MDL0Material* mHeader;

                    form?.Say("Writing Shaders");

                    //Write data without headers
                    foreach (ResourceNode e in group.Children)
                    {
                        if (((MDL0ShaderNode) e)._materials.Count > 0)
                        {
                            len = e._calcSize;
                            e.Rebuild(pData, len, force);
                            pData += len;
                        }
                    }

                    //Write one header for each material, using same order.
                    if (mats != null)
                    {
                        foreach (MDL0MaterialNode mat in mats.Children)
                        {
                            mHeader = mat.Header;
                            if (mat._shader != null)
                            {
                                len = (int) mat._shader.Header;
                                mHeader->_shaderOffset = len - (int) mHeader;
                            }
                            else
                            {
                                mHeader->_shaderOffset = 0;
                            }
                        }
                    }
                }
                else if (resType == MR.Objects || resType == MR.Materials)
                {
                    foreach (ResourceNode r in group.Children)
                    {
                        form?.Say("Writing " + resType + " - " + r.Name);

                        len = r._calcSize;
                        r.Rebuild(pData, len, true); //Forced to fix object node ids and align materials
                        pData += len;
                    }
                }
                else
                {
                    bool rebuild = true;

                    if (Model._isImport)
                    {
                        if (group._name == "Vertices" ||
                            group._name == "Normals" ||
                            group._name == "UVs" ||
                            group._name == "Colors")
                        {
                            rebuild = false; //The data has already been written!
                        }
                    }

                    if (rebuild)
                    {
                        foreach (ResourceNode e in group.Children)
                        {
                            //Console.WriteLine("Rebuilding the " + group.Name);

                            form?.Say("Writing the " + resType + " - " + e.Name);

                            len = e._calcSize;
                            e.Rebuild(pData, len, true); //Forced just in case we need to convert to float.
                            pData += len;
                        }
                    }
                }
            }

            //Write relocation offsets in the order of the header
            fixed (ResourceGroup** pOut = &Defs)
            {
                foreach (MR resType in IndexBank[Version])
                {
                    if ((@group = Groups[(int) resType]) == null || SpecialRebuildData((int) resType))
                    {
                        continue;
                    }

                    pOut[(int) resType] = pGrp = (ResourceGroup*) pGroup;
                    pEntry = &pGrp->_first + 1;
                    if (resType == MR.Bones)
                    {
                        *pGrp = new ResourceGroup(BoneCache.Length);
                        foreach (ResourceNode e in BoneCache)
                        {
                            (pEntry++)->_dataOffset = (int) ((byte*) e.WorkingUncompressed.Address - pGroup);
                        }
                    }
                    else if (resType == MR.Shaders)
                    {
                        MDL0GroupNode mats = Groups[(int) MR.Materials];

                        if (mats != null)
                        {
                            //Create a material group with the amount of entries
                            *pGrp = new ResourceGroup(mats.Children.Count);

                            foreach (MDL0MaterialNode mat in mats.Children)
                            {
                                (pEntry++)->_dataOffset = (int) mat._shader.Header - (int) pGrp;
                            }
                        }
                    }
                    else
                    {
                        *pGrp = new ResourceGroup(group.Children.Count);
                        foreach (ResourceNode e in group.Children)
                        {
                            (pEntry++)->_dataOffset = (int) ((byte*) e.WorkingUncompressed.Address - pGroup);
                        }
                    }

                    pGroup += pGrp->_totalSize;
                }
            }
        }

        //Write stored offsets to MDL header
        public void Finish()
        {
            List<MR> iList = IndexBank[Version];
            MR resType;
            bint* pOffset = (bint*) Header + 4;
            int count = iList.Count, offset;

            fixed (ResourceGroup** pGroup = &Defs)
            {
                for (int i = 0; i < count; i++)
                {
                    resType = iList[i];
                    if ((offset = (int) pGroup[(int) resType]) > 0)
                    {
                        offset -= (int) Header;
                    }

                    pOffset[i] = offset;
                }
            }
        }
    }
}