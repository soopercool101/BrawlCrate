using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MovesetConverter
    {
        //Wow this format is messy.

        //Huge thanks to: 

        //PhantomWings, for the PSA source code and its text files.
        //Dantarion, for the OpenSA3/Tabuu source code.
        //Kryal, for Brawlbox and starting to make structs for this format.
        //Bero, for starting to parse the format for Brawlbox.
        //Toomai, for the hitbox/hurtbox flags and rendering code.
        //The Project M team, for their moveset text files to use in the internal event dictionary.

        //Data order:

        //Header
        //Attributes
        //SSE Attributes
        //Common Action Flags
        //Unknown 7
        //Subaction Other Data - includes articles (at end)
        //Subaction GFX Data - includes articles (at end)
        //Misc Bone Section
        //Subaction SFX Data - includes articles (at end)
        //Sound Lists Final Children Entries
        //Subaction Main Data - includes articles (at end)
        //Misc Section 1
        //Hurtboxes
        //Misc Unk Section 9 Final Children Entries
        //Ledgegrabs
        //Crawl
        //Tether
        //Multijump
        //Glide
        //Actions Data Part1/Part2 alternating
        //Subroutines
        //Article Actions
        //Action Flags
        //Action Entry Offsets
        //Action Exit Offsets
        //Action Pre
        //Subaction Flags
        //Subaction Offsets Main/GFX/SFX/Other
        //Model Visibility
        //Misc Item Bones
        //Hand Bones
        //Unknown Section 9
        //Unk24
        //Unknown Section 12
        //Unk22
        //Extra Params
        //Static Articles
        //Entry Article
        //Attributed Articles
        //Misc Section 2
        //Action Interrupts
        //Bone Floats 1
        //Bone Floats 2
        //Bone Floats 3
        //Bone References Main
        //Bone References Misc
        //Misc Sound Lists
        //Misc Section Header
        //Sections Data (includes data header)
        //Lookup Offsets
        //Sections Offsets
        //References Offsets
        //Sections/References String Table

        public static int CalcDataSize2()
        {
            int size = 0;
            foreach (MoveDefEntryNode e in MoveDefNode.nodeDictionary.Values)
            {
                if (e.External && !(e._extNode is MoveDefReferenceEntryNode))
                {
                    continue;
                }

                size += e.CalculateSize(true);
            }

            return size;
        }

        public static void BuildData2(MoveDefDataNode node, MovesetHeader* header, VoidPtr address, int length,
                                      bool force)
        {
            VoidPtr addr = address;
            foreach (MoveDefEntryNode e in MoveDefNode.nodeDictionary.Values)
            {
                if (e.External && !(e._extNode is MoveDefReferenceEntryNode))
                {
                    continue;
                }

                e.Rebuild(addr, e._calcSize, true);
            }
        }

        public static int CalcDataSize(MoveDefDataNode node)
        {
            int
                lookupCount = 0,
                part1Len = 0,
                part2Len = 0,
                part3Len = 0,
                part4Len = 0,
                part5Len = 0,
                part6Len = 0,
                part7Len = 0;

            MoveDefNode RootNode = node.Parent.Parent as MoveDefNode;

            FDefSubActionStringTable subActionTable = new FDefSubActionStringTable();

            #region Part 1

            part1Len += GetSize(node.attributes, ref lookupCount);
            part1Len += GetSize(node.sseAttributes, ref lookupCount);
            part1Len += GetSize(node.commonActionFlags, ref lookupCount);
            part1Len += GetSize(node.unk7, ref lookupCount);

            foreach (MoveDefSubActionGroupNode g in RootNode._subActions.Children)
            {
                if (g.Name != "<null>")
                {
                    //bool forceWrite = ForceSubActionWrite(g);
                    foreach (MoveDefActionNode a in g.Children)
                    {
                        if (a.Children.Count > 0 || a._actionRefs.Count > 0 || a._build)
                        {
                            part1Len += GetSize(a, ref lookupCount);
                            lookupCount++;
                        }
                    }
                }
            }

            part1Len += CalcSizeArticleActions(node, ref lookupCount, true, 0);

            part1Len += CalcSizeArticleActions(node, ref lookupCount, true, 1);

            part1Len += CalcSizeArticleActions(node, ref lookupCount, true, 2);

            part1Len += GetSize(node.misc.unkBoneSection, ref lookupCount);

            if (node.misc.soundData != null)
            {
                foreach (MoveDefSoundDataNode r in node.misc.soundData.Children)
                {
                    part1Len += r.Children.Count * 4;
                }
            }

            foreach (MoveDefActionGroupNode a in RootNode._actions.Children)
            {
                if (a.Children[0].Children.Count > 0 || (a.Children[0] as MoveDefActionNode)._actionRefs.Count > 0 ||
                    (a.Children[0] as MoveDefActionNode)._build) //Entry
                {
                    part1Len += GetSize(a.Children[0] as MoveDefActionNode, ref lookupCount);
                    lookupCount++;
                }

                if (a.Children[1].Children.Count > 0 || (a.Children[1] as MoveDefActionNode)._actionRefs.Count > 0 ||
                    (a.Children[1] as MoveDefActionNode)._build) //Exit
                {
                    part1Len += GetSize(a.Children[1] as MoveDefActionNode, ref lookupCount);
                    lookupCount++;
                }
            }

            #endregion

            #region Part 2

            part2Len += GetSize(node.misc.unkSection1, ref lookupCount);

            part2Len += GetSize(node.misc.hurtBoxes, ref lookupCount);

            //if (node.misc.collisionData != null)
            //    foreach (MoveDefOffsetNode offset in node.misc.collisionData.Children)
            //        if (offset.Children.Count > 0 && !(offset.Children[0] as MoveDefEntryNode).External)
            //            part2Len += offset.Children[0].Children.Count * 4;

            part2Len += GetSize(node.misc.ledgeGrabs, ref lookupCount);

            part2Len += GetSize(node.misc.tether, ref lookupCount);

            part2Len += GetSize(node.misc.crawl, ref lookupCount);

            part2Len += GetSize(node.misc.multiJump, ref lookupCount);

            part2Len += GetSize(node.misc.glide, ref lookupCount);

            for (int i = 0; i < RootNode._subRoutineList.Count; i++)
            {
                if ((RootNode._subRoutineList[i] as MoveDefActionNode)._actionRefs.Count > 0)
                {
                    part2Len += GetSize(RootNode._subRoutineList[i] as MoveDefActionNode, ref lookupCount);
                }
            }

            if (node.unk22 != null)
            {
                if (node.unk22.Children.Count > 0)
                {
                    part2Len += GetSize(node.unk22.Children[0] as MoveDefActionNode, ref lookupCount);
                }
            }

            if (node.override1 != null)
            {
                foreach (MoveDefActionOverrideEntryNode e in node.override1.Children)
                {
                    part2Len += GetSize(e.Children[0] as MoveDefActionNode, ref lookupCount);
                }
            }

            if (node.override2 != null)
            {
                foreach (MoveDefActionOverrideEntryNode e in node.override2.Children)
                {
                    part2Len += GetSize(e.Children[0] as MoveDefActionNode, ref lookupCount);
                }
            }

            part2Len += CalcSizeArticleActions(node, ref lookupCount, false, 0);

            part2Len += GetSize(node.actionFlags, ref lookupCount);

            #endregion

            #region Part 3

            //Actions part 1 and 2 offsets
            lookupCount += 2; //offset to the lists
            part3Len += RootNode._actions.Children.Count * 8;

            part3Len += GetSize(node.actionPre, ref lookupCount);

            #endregion

            #region Part 4

            //Subaction flags
            lookupCount++; //offset to the list
            foreach (MoveDefSubActionGroupNode g in RootNode._subActions.Children)
            {
                if (g.Name != "<null>")
                {
                    lookupCount++;
                    subActionTable.Add(g.Name);
                }

                part4Len += 8;
            }

            //Subaction string table
            part4Len += subActionTable.TotalSize;

            #endregion

            #region Part 5

            //Subaction offsets already written
            lookupCount += 4; //offset to the lists
            part5Len += RootNode._subActions.Children.Count * 16;

            part5Len += GetSize(node.mdlVisibility, ref lookupCount);

            part5Len += GetSize(node.misc.unkSection3, ref lookupCount);

            part5Len += GetSize(node.boneRef2, ref lookupCount);

            if (node.nanaSubActions != null)
            {
                foreach (MoveDefSubActionGroupNode g in node.nanaSubActions.Children)
                {
                    if (g.Name != "<null>")
                    {
                        foreach (MoveDefActionNode a in g.Children)
                        {
                            if (a.Children.Count > 0 || a._actionRefs.Count > 0 || a._build)
                            {
                                part5Len += GetSize(a, ref lookupCount);
                                lookupCount++;
                            }
                        }
                    }
                }

                part5Len += node.nanaSubActions.Children.Count * 16;
            }

            part5Len += GetSize(node.misc.collisionData, ref lookupCount);

            part5Len += GetSize(node.unk24, ref lookupCount);

            part5Len += GetSize(node.misc.unk12, ref lookupCount);

            part5Len += GetSize(node.unk22, ref lookupCount);

            #endregion

            #region Part 6

            part6Len += GetSize(node.staticArticles, ref lookupCount);

            part6Len += GetSize(node.entryArticle, ref lookupCount);

            foreach (MoveDefEntryNode e in node._articles.Values)
            {
                part6Len += GetSize(e, ref lookupCount);
            }

            part6Len += GetSize(node.misc.unkSection2, ref lookupCount);

            if (node.nanaSoundData != null)
            {
                foreach (MoveDefSoundDataNode r in node.nanaSoundData.Children)
                {
                    lookupCount += r.Children.Count > 0 ? 1 : 0;
                    part6Len += 8 + r.Children.Count * 4;
                }
            }

            part6Len += GetSize(node.actionInterrupts, ref lookupCount);

            part6Len += GetSize(node.boneFloats1, ref lookupCount);

            part6Len += GetSize(node.boneFloats2, ref lookupCount);

            part6Len += GetSize(node.boneFloats3, ref lookupCount);

            part6Len += GetSize(node.boneRef1, ref lookupCount);

            part6Len += GetSize(node.misc.boneRefs, ref lookupCount);

            part6Len += GetSize(node.misc.unkSection5, ref lookupCount);

            part6Len += GetSize(node.misc.soundData, ref lookupCount);

            #endregion

            #region Part 7

            //Misc Section
            part7Len += 0x4C;
            lookupCount++; //data offset

            #endregion

            foreach (int i in node._extraOffsets)
            {
                if (i > 1480 && i < RootNode.dataSize)
                {
                    lookupCount++;
                }
            }

            node.subActionTable = subActionTable;
            node._lookupCount = lookupCount;

            return node._childLength =
                (node.part1Len = part1Len) +
                (node.part2Len = part2Len) +
                (node.part3Len = part3Len) +
                (node.part4Len = part4Len) +
                (node.part5Len = part5Len) +
                (node.part6Len = part6Len) +
                (node.part7Len = part7Len);
        }

        public static int GetSize(MoveDefEntryNode node, ref int lookupCount)
        {
            if (node != null)
            {
                int size = 0;
                if (!(node.External && !(node._extNode is MoveDefReferenceEntryNode)))
                {
                    node._lookupOffsets = new List<int>();

                    if ((node.Parent is MoveDefDataNode || node.Parent is MoveDefMiscNode) && !node.isExtra)
                    {
                        lookupCount++;
                    }

                    size = node.CalculateSize(true);
                    lookupCount += node._lookupCount;
                }

                MoveDefEntryNode next = node;
                Top:
                //Check for random params around the file
                if (next.Parent is MoveDefDataNode)
                {
                    if (next.Parent.Children.Count > next.Index + 1)
                    {
                        if (
                            (next = next.Parent.Children[next.Index + 1] as MoveDefEntryNode) is
                            MoveDefCharSpecificNode || next is MoveDefRawDataNode && next.Children.Count > 0 &&
                            next.Children[0] is MoveDefSectionParamNode)
                        {
                            if (!(next is MoveDefRawDataNode))
                            {
                                size += next.CalculateSize(true);
                                lookupCount += next._lookupCount;
                            }
                            else
                            {
                                foreach (MoveDefSectionParamNode p in next.Children)
                                {
                                    size += p.CalculateSize(true);
                                }
                            }

                            goto Top;
                        }
                    }
                }

                return size;
            }

            return 0;
        }

        internal static void BuildData(MoveDefDataNode node, MovesetHeader* header, VoidPtr address, int length,
                                       bool force)
        {
            MoveDefNode RootNode = node.Parent.Parent as MoveDefNode;

            VoidPtr dataAddress = address;
            VoidPtr baseAddress = node._rebuildBase;

            node._entryOffset = header;

            bint* extraOffsets = (bint*) ((VoidPtr) header + 124);

            bint* action1Offsets = (bint*) (dataAddress + node.part1Len + node.part2Len);
            bint* action2Offsets = action1Offsets + RootNode._actions.Children.Count;

            bint* mainOffsets = (bint*) (dataAddress + (node.part1Len + node.part2Len + node.part3Len + node.part4Len));
            bint* GFXOffsets = (bint*) ((VoidPtr) mainOffsets + RootNode._subActions.Children.Count * 4);
            bint* SFXOffsets = (bint*) ((VoidPtr) GFXOffsets + RootNode._subActions.Children.Count * 4);
            bint* otherOffsets = (bint*) ((VoidPtr) SFXOffsets + RootNode._subActions.Children.Count * 4);

            FDefMiscSection* miscOffsetsAddr = (FDefMiscSection*) (dataAddress + (node._childLength - 0x4C));

            if (node != null)
            {
                node._entryOffset = header;
                if (node.misc != null)
                {
                    node.misc._entryOffset = miscOffsetsAddr;
                    header->MiscSectionOffset = (int) miscOffsetsAddr - (int) baseAddress;
                }
            }

            header->SubactionMainStart = (int) mainOffsets - (int) baseAddress;
            header->SubactionGFXStart = (int) GFXOffsets - (int) baseAddress;
            header->SubactionSFXStart = (int) SFXOffsets - (int) baseAddress;
            header->SubactionOtherStart = (int) otherOffsets - (int) baseAddress;

            header->EntryActionsStart = (int) action1Offsets - (int) baseAddress;
            header->ExitActionsStart = (int) action2Offsets - (int) baseAddress;

            #region Part 1

            if ((int) dataAddress - (int) baseAddress != 0)
            {
                Console.WriteLine("p1");
            }

            header->AttributeStart = Rebuild(RootNode, node.attributes, ref dataAddress, baseAddress);
            MoveDefNode._lookupOffsets.Add((int) header->AttributeStart.Address - (int) baseAddress);

            header->SSEAttributeStart = Rebuild(RootNode, node.sseAttributes, ref dataAddress, baseAddress);

            header->CommonActionFlagsStart = Rebuild(RootNode, node.commonActionFlags, ref dataAddress, baseAddress);

            header->Unknown7 = Rebuild(RootNode, node.unk7, ref dataAddress, baseAddress);

            foreach (MoveDefSubActionGroupNode grp in RootNode._subActions.Children)
            {
                if (grp.Name != "<null>")
                {
                    if (grp.Children[3].Children.Count > 0 ||
                        ((MoveDefActionNode) grp.Children[3])._actionRefs.Count > 0 ||
                        ((MoveDefActionNode) grp.Children[3])._build)
                    {
                        otherOffsets[grp.Index] = Rebuild(RootNode, grp.Children[3] as MoveDefActionNode,
                            ref dataAddress, baseAddress);
                        MoveDefNode._lookupOffsets.Add((int) &otherOffsets[grp.Index] - (int) baseAddress);
                    }
                    else
                    {
                        otherOffsets[grp.Index] = 0;
                    }
                }
            }

            foreach (MoveDefSubActionGroupNode grp in RootNode._subActions.Children)
            {
                if (grp.Name != "<null>")
                {
                    if (grp.Children[1].Children.Count > 0 ||
                        ((MoveDefActionNode) grp.Children[1])._actionRefs.Count > 0 ||
                        ((MoveDefActionNode) grp.Children[1])._build)
                    {
                        GFXOffsets[grp.Index] = Rebuild(RootNode, grp.Children[1] as MoveDefActionNode, ref dataAddress,
                            baseAddress);
                        MoveDefNode._lookupOffsets.Add((int) &GFXOffsets[grp.Index] - (int) baseAddress);
                    }
                    else
                    {
                        GFXOffsets[grp.Index] = 0;
                    }
                }
            }

            RebuildArticleActions(RootNode, node, ref dataAddress, baseAddress, true, 1);

            if ((miscOffsetsAddr->UnkBoneSectionOffset =
                Rebuild(RootNode, node.misc.unkBoneSection, ref dataAddress, baseAddress)) > 0)
            {
                miscOffsetsAddr->UnkBoneSectionCount = node.misc.unkBoneSection.Children.Count;
            }

            foreach (MoveDefSubActionGroupNode grp in RootNode._subActions.Children)
            {
                if (grp.Name != "<null>")
                {
                    if (grp.Children[2].Children.Count > 0 ||
                        ((MoveDefActionNode) grp.Children[2])._actionRefs.Count > 0 ||
                        ((MoveDefActionNode) grp.Children[2])._build)
                    {
                        SFXOffsets[grp.Index] = Rebuild(RootNode, grp.Children[2] as MoveDefActionNode, ref dataAddress,
                            baseAddress);
                        MoveDefNode._lookupOffsets.Add((int) &SFXOffsets[grp.Index] - (int) baseAddress);
                    }
                    else
                    {
                        SFXOffsets[grp.Index] = 0;
                    }
                }
            }

            RebuildArticleActions(RootNode, node, ref dataAddress, baseAddress, true, 2);

            if (node.misc.soundData != null)
            {
                foreach (MoveDefSoundDataNode r in node.misc.soundData.Children)
                {
                    foreach (MoveDefIndexNode b in r.Children)
                    {
                        b._entryOffset = dataAddress;
                        *(bint*) dataAddress = b.ItemIndex;
                        dataAddress += 4;
                    }
                }
            }

            foreach (MoveDefSubActionGroupNode grp in RootNode._subActions.Children)
            {
                if (grp.Name != "<null>")
                {
                    if (grp.Children[0].Children.Count > 0 ||
                        ((MoveDefActionNode) grp.Children[0])._actionRefs.Count > 0 ||
                        ((MoveDefActionNode) grp.Children[0])._build)
                    {
                        mainOffsets[grp.Index] = Rebuild(RootNode, grp.Children[0] as MoveDefActionNode,
                            ref dataAddress, baseAddress);
                        MoveDefNode._lookupOffsets.Add((int) &mainOffsets[grp.Index] - (int) baseAddress);
                    }
                    else
                    {
                        mainOffsets[grp.Index] = 0;
                    }
                }
            }

            RebuildArticleActions(RootNode, node, ref dataAddress, baseAddress, true, 0);

            foreach (MoveDefActionGroupNode grp in RootNode._actions.Children)
            {
                if (grp.Children[0].Children.Count > 0 ||
                    (grp.Children[0] as MoveDefActionNode)._actionRefs.Count > 0 ||
                    (grp.Children[0] as MoveDefActionNode)._build) //Entry
                {
                    action1Offsets[grp.Index] = Rebuild(RootNode, grp.Children[0] as MoveDefActionNode, ref dataAddress,
                        baseAddress);
                    MoveDefNode._lookupOffsets.Add((int) &action1Offsets[grp.Index] - (int) baseAddress);
                }

                if (grp.Children[1].Children.Count > 0 ||
                    (grp.Children[1] as MoveDefActionNode)._actionRefs.Count > 0 ||
                    (grp.Children[1] as MoveDefActionNode)._build) //Exit
                {
                    action2Offsets[grp.Index] = Rebuild(RootNode, grp.Children[1] as MoveDefActionNode, ref dataAddress,
                        baseAddress);
                    MoveDefNode._lookupOffsets.Add((int) &action2Offsets[grp.Index] - (int) baseAddress);
                }
            }

            #endregion

            #region Part 2

            if ((int) dataAddress - (int) baseAddress != node.part1Len)
            {
                Console.WriteLine("p2");
            }

            miscOffsetsAddr->UnknownSection1Offset =
                Rebuild(RootNode, node.misc.unkSection1, ref dataAddress, baseAddress);

            if ((miscOffsetsAddr->HurtBoxOffset =
                Rebuild(RootNode, node.misc.hurtBoxes, ref dataAddress, baseAddress)) > 0)
            {
                miscOffsetsAddr->HurtBoxCount = node.misc.hurtBoxes.Children.Count;
            }

            //if (node.misc.collisionData != null)
            //    foreach (MoveDefOffsetNode offset in node.misc.collisionData.Children)
            //        if (offset.Children.Count > 0 && !(offset.Children[0] as MoveDefEntryNode).External)
            //            foreach (MoveDefBoneIndexNode b in offset.Children[0].Children)
            //            {
            //                b._entryOffset = dataAddress;
            //                *(bint*)dataAddress = b.boneIndex;
            //                dataAddress += 4;
            //            }

            if ((miscOffsetsAddr->LedgegrabOffset =
                Rebuild(RootNode, node.misc.ledgeGrabs, ref dataAddress, baseAddress)) > 0)
            {
                miscOffsetsAddr->LedgegrabCount = node.misc.ledgeGrabs.Children.Count;
            }

            miscOffsetsAddr->TetherOffset = Rebuild(RootNode, node.misc.tether, ref dataAddress, baseAddress);

            miscOffsetsAddr->CrawlOffset = Rebuild(RootNode, node.misc.crawl, ref dataAddress, baseAddress);

            miscOffsetsAddr->MultiJumpOffset = Rebuild(RootNode, node.misc.multiJump, ref dataAddress, baseAddress);

            miscOffsetsAddr->GlideOffset = Rebuild(RootNode, node.misc.glide, ref dataAddress, baseAddress);

            for (int i = 0; i < RootNode._subRoutineList.Count; i++)
            {
                if ((RootNode._subRoutineList[i] as MoveDefActionNode)._actionRefs.Count > 0)
                {
                    Rebuild(RootNode, RootNode._subRoutineList[i] as MoveDefActionNode, ref dataAddress, baseAddress);
                }
            }

            if (node.unk22 != null)
            {
                if (node.unk22.Children.Count > 0)
                {
                    Rebuild(RootNode, node.unk22.Children[0] as MoveDefActionNode, ref dataAddress, baseAddress);
                }
            }

            if (node.override1 != null)
            {
                foreach (MoveDefActionOverrideEntryNode e in node.override1.Children)
                {
                    Rebuild(RootNode, e.Children[0] as MoveDefActionNode, ref dataAddress, baseAddress);
                }
            }

            if (node.override2 != null)
            {
                foreach (MoveDefActionOverrideEntryNode e in node.override2.Children)
                {
                    Rebuild(RootNode, e.Children[0] as MoveDefActionNode, ref dataAddress, baseAddress);
                }
            }

            RebuildArticleActions(RootNode, node, ref dataAddress, baseAddress, false, 0);

            header->ActionFlagsStart = Rebuild(RootNode, node.actionFlags, ref dataAddress, baseAddress);

            #endregion

            #region Part 3

            if ((int) dataAddress - (int) baseAddress != node.part1Len + node.part2Len)
            {
                Console.WriteLine("p3");
            }

            //Actions part 1 and 2 already written
            dataAddress += RootNode._actions.Children.Count * 8;

            header->ActionPreStart = Rebuild(RootNode, node.actionPre, ref dataAddress, baseAddress);

            #endregion

            #region Part 4

            if ((int) dataAddress - (int) baseAddress != node.part1Len + node.part2Len + node.part3Len)
            {
                Console.WriteLine("p4");
            }

            node.subActionTable.WriteTable(dataAddress);

            dataAddress += node.subActionTable.TotalSize;

            header->SubactionFlagsStart = (int) dataAddress - (int) baseAddress;

            int index = 0;
            FDefSubActionFlag* flags = (FDefSubActionFlag*) dataAddress;
            foreach (MoveDefSubActionGroupNode g in RootNode._subActions.Children)
            {
                *flags = new FDefSubActionFlag
                {
                    _InTranslationTime = g._inTransTime, _Flags = g._flags,
                    _stringOffset = g.Name == "<null>" ? 0 : (int) node.subActionTable[g.Name] - (int) baseAddress
                };

                if (flags->_stringOffset > 0)
                {
                    if (index == 412)
                    {
                        node.zssFirstOffset = (int) flags->_stringOffset.Address - (int) baseAddress;
                    }

                    if (index == 317)
                    {
                        node.warioSwing4StringOffset = (int) flags->_stringOffset.Address - (int) baseAddress;
                    }

                    MoveDefNode._lookupOffsets.Add((int) flags->_stringOffset.Address - (int) baseAddress);
                }

                flags++;
                index++;
            }

            dataAddress = flags;

            #endregion

            #region Part 5

            if ((int) dataAddress - (int) baseAddress != node.part1Len + node.part2Len + node.part3Len + node.part4Len)
            {
                Console.WriteLine("p5");
            }

            //Subaction offsets already written
            dataAddress += RootNode._subActions.Children.Count * 16;

            header->ModelVisibilityStart = Rebuild(RootNode, node.mdlVisibility, ref dataAddress, baseAddress);

            miscOffsetsAddr->UnknownSection3Offset =
                Rebuild(RootNode, node.misc.unkSection3, ref dataAddress, baseAddress);

            header->BoneRef2 = Rebuild(RootNode, node.boneRef2, ref dataAddress, baseAddress);

            if (node.nanaSubActions != null)
            {
                int dataOff = 0;
                foreach (MoveDefSubActionGroupNode grp in node.nanaSubActions.Children)
                {
                    if (grp.Name != "<null>")
                    {
                        foreach (MoveDefActionNode a in grp.Children)
                        {
                            if (a.Children.Count > 0 || a._actionRefs.Count > 0)
                            {
                                dataOff += a._calcSize;
                            }
                        }
                    }
                }

                bint* main2Offsets = (bint*) (dataAddress + dataOff);
                bint* GFX2Offsets = (bint*) ((VoidPtr) main2Offsets + node.nanaSubActions.Children.Count * 4);
                bint* SFX2Offsets = (bint*) ((VoidPtr) GFX2Offsets + node.nanaSubActions.Children.Count * 4);
                bint* other2Offsets = (bint*) ((VoidPtr) SFX2Offsets + node.nanaSubActions.Children.Count * 4);

                extraOffsets[0] = (int) main2Offsets - (int) baseAddress;
                extraOffsets[1] = (int) GFX2Offsets - (int) baseAddress;
                extraOffsets[2] = (int) SFX2Offsets - (int) baseAddress;
                extraOffsets[3] = (int) other2Offsets - (int) baseAddress;

                foreach (MoveDefSubActionGroupNode grp in node.nanaSubActions.Children)
                {
                    if (grp.Name != "<null>" && (grp.Children[3].Children.Count > 0 ||
                                                 ((MoveDefActionNode) grp.Children[3])._actionRefs.Count > 0 ||
                                                 ((MoveDefActionNode) grp.Children[3])._build))
                    {
                        other2Offsets[grp.Index] = Rebuild(RootNode, grp.Children[3] as MoveDefActionNode,
                            ref dataAddress, baseAddress);
                        MoveDefNode._lookupOffsets.Add((int) &other2Offsets[grp.Index] - (int) baseAddress);
                    }
                    else
                    {
                        other2Offsets[grp.Index] = 0;
                    }
                }

                foreach (MoveDefSubActionGroupNode grp in node.nanaSubActions.Children)
                {
                    if (grp.Name != "<null>" && (grp.Children[1].Children.Count > 0 ||
                                                 ((MoveDefActionNode) grp.Children[1])._actionRefs.Count > 0 ||
                                                 ((MoveDefActionNode) grp.Children[1])._build))
                    {
                        GFX2Offsets[grp.Index] = Rebuild(RootNode, grp.Children[1] as MoveDefActionNode,
                            ref dataAddress, baseAddress);
                        MoveDefNode._lookupOffsets.Add((int) &GFX2Offsets[grp.Index] - (int) baseAddress);
                    }
                    else
                    {
                        GFX2Offsets[grp.Index] = 0;
                    }
                }

                foreach (MoveDefSubActionGroupNode grp in node.nanaSubActions.Children)
                {
                    if (grp.Name != "<null>" && (grp.Children[2].Children.Count > 0 ||
                                                 ((MoveDefActionNode) grp.Children[2])._actionRefs.Count > 0 ||
                                                 ((MoveDefActionNode) grp.Children[2])._build))
                    {
                        SFX2Offsets[grp.Index] = Rebuild(RootNode, grp.Children[2] as MoveDefActionNode,
                            ref dataAddress, baseAddress);
                        MoveDefNode._lookupOffsets.Add((int) &SFX2Offsets[grp.Index] - (int) baseAddress);
                    }
                    else
                    {
                        SFX2Offsets[grp.Index] = 0;
                    }
                }

                foreach (MoveDefSubActionGroupNode grp in node.nanaSubActions.Children)
                {
                    if (grp.Name != "<null>" && (grp.Children[0].Children.Count > 0 ||
                                                 ((MoveDefActionNode) grp.Children[0])._actionRefs.Count > 0 ||
                                                 ((MoveDefActionNode) grp.Children[0])._build))
                    {
                        main2Offsets[grp.Index] = Rebuild(RootNode, grp.Children[0] as MoveDefActionNode,
                            ref dataAddress, baseAddress);
                        MoveDefNode._lookupOffsets.Add((int) &main2Offsets[grp.Index] - (int) baseAddress);
                    }
                    else
                    {
                        main2Offsets[grp.Index] = 0;
                    }
                }

                dataAddress = (VoidPtr) other2Offsets + node.nanaSubActions.Children.Count * 4;
            }

            miscOffsetsAddr->CollisionDataOffset =
                Rebuild(RootNode, node.misc.collisionData, ref dataAddress, baseAddress);

            header->Unknown24 = Rebuild(RootNode, node.unk24, ref dataAddress, baseAddress);

            miscOffsetsAddr->UnknownSection12Offset = Rebuild(RootNode, node.misc.unk12, ref dataAddress, baseAddress);

            header->Unknown22 = Rebuild(RootNode, node.unk22, ref dataAddress, baseAddress);

            #endregion

            #region Part 6

            if ((int) dataAddress - (int) baseAddress !=
                node.part1Len + node.part2Len + node.part3Len + node.part4Len + node.part5Len)
            {
                Console.WriteLine("p6");
            }

            header->StaticArticlesStart = Rebuild(RootNode, node.staticArticles, ref dataAddress, baseAddress);

            header->EntryArticleStart = Rebuild(RootNode, node.entryArticle, ref dataAddress, baseAddress);

            foreach (MoveDefArticleNode e in node._articles.Values)
            {
                Rebuild(RootNode, e, ref dataAddress, baseAddress);
            }

            if ((miscOffsetsAddr->UnknownSection2Offset =
                Rebuild(RootNode, node.misc.unkSection2, ref dataAddress, baseAddress)) > 0)
            {
                miscOffsetsAddr->UnknownSection2Count = node.misc.unkSection2.Children.Count;
            }

            if (node.nanaSoundData != null)
            {
                foreach (MoveDefSoundDataNode r in node.nanaSoundData.Children)
                {
                    foreach (MoveDefIndexNode b in r.Children)
                    {
                        b._entryOffset = dataAddress;
                        *(bint*) dataAddress = b.ItemIndex;
                        dataAddress += 4;
                    }
                }

                FDefListOffset* sndLists = (FDefListOffset*) dataAddress;
                FDefListOffset* data = (FDefListOffset*) &extraOffsets[10];

                node.nanaSoundData._entryOffset = data;

                if (node.nanaSoundData.Children.Count > 0)
                {
                    data->_startOffset = (int) sndLists - (int) baseAddress;
                    MoveDefNode._lookupOffsets.Add((int) data->_startOffset.Address - (int) baseAddress);
                }

                data->_listCount = node.nanaSoundData.Children.Count;

                foreach (MoveDefSoundDataNode r in node.nanaSoundData.Children)
                {
                    if (r.Children.Count > 0)
                    {
                        sndLists->_startOffset =
                            (int) (r.Children[0] as MoveDefEntryNode)._entryOffset - (int) baseAddress;
                        MoveDefNode._lookupOffsets.Add((int) sndLists->_startOffset.Address - (int) baseAddress);
                    }

                    (sndLists++)->_listCount = r.Children.Count;
                }

                dataAddress = sndLists;
            }

            header->ActionInterrupts = Rebuild(RootNode, node.actionInterrupts, ref dataAddress, baseAddress);

            header->BoneFloats1 = Rebuild(RootNode, node.boneFloats1, ref dataAddress, baseAddress);

            header->BoneFloats2 = Rebuild(RootNode, node.boneFloats2, ref dataAddress, baseAddress);

            header->BoneFloats3 = Rebuild(RootNode, node.boneFloats3, ref dataAddress, baseAddress);

            header->BoneRef1 = Rebuild(RootNode, node.boneRef1, ref dataAddress, baseAddress);

            miscOffsetsAddr->BoneRef2Offset = Rebuild(RootNode, node.misc.boneRefs, ref dataAddress, baseAddress);

            miscOffsetsAddr->UnknownSection5Offset =
                Rebuild(RootNode, node.misc.unkSection5, ref dataAddress, baseAddress);

            miscOffsetsAddr->SoundDataOffset = Rebuild(RootNode, node.misc.soundData, ref dataAddress, baseAddress);

            #endregion

            #region Part 7

            if ((int) dataAddress - (int) baseAddress != node.part1Len + node.part2Len + node.part3Len + node.part4Len +
                node.part5Len + node.part6Len)
            {
                Console.WriteLine("p7");
            }

            //Misc section, already written
            dataAddress += 0x4C;

            #endregion

            //Params
            int l = 0, ind = 0;
            foreach (int i in node._extraOffsets)
            {
                if (i > 1480 && i < RootNode.dataSize)
                {
                    MoveDefEntryNode e = node._extraEntries[l];

                    if (extraOffsets[ind] == 0)
                    {
                        if (e == null)
                        {
                            extraOffsets[ind] = i;
                        }
                        else
                        {
                            extraOffsets[ind] = (int) e._entryOffset - (int) baseAddress;
                        }
                    }

                    MoveDefNode._lookupOffsets.Add((int) &extraOffsets[ind] - (int) baseAddress);

                    l++;
                }
                else
                {
                    extraOffsets[ind] = i;
                }

                ind++;
            }

            header->EntryActionOverrides = node.override1 != null && node.override1.External
                ? (int) node.override1._entryOffset - (int) baseAddress
                : 0;
            header->ExitActionOverrides = node.override2 != null && node.override2.External
                ? (int) node.override2._entryOffset - (int) baseAddress
                : 0;

            header->Unknown27 = node.Unknown27;
            header->Unknown28 = node.Unknown28;
            header->Flags1 = node.Flags1uint;
            header->Flags2 = node.Flags2int;

            bint* offsets = (bint*) header;
            for (int i = 0; i < 27; i++)
            {
                if (offsets[i] > 0)
                {
                    MoveDefNode._lookupOffsets.Add((int) &offsets[i] - (int) baseAddress);
                }
            }

            offsets = (bint*) miscOffsetsAddr;
            for (int i = 0; i < 19; i++)
            {
                if (offsets[i] > 0 && !(i % 2 == 0 && i > 0 && i < 9))
                {
                    MoveDefNode._lookupOffsets.Add((int) &offsets[i] - (int) baseAddress);
                }
            }

            //Go back and add offsets to nodes that need them
            foreach (MoveDefEntryNode entry in RootNode._postProcessNodes)
            {
                entry.PostProcess();
            }
        }

        public static int Rebuild(MoveDefNode root, MoveDefEntryNode node, ref VoidPtr dataAddress, VoidPtr baseAddress)
        {
            if (node != null)
            {
                if (!(node.External && !(node._extNode is MoveDefReferenceEntryNode)))
                {
                    node.Rebuild(dataAddress, node._calcSize, true);
                    dataAddress += node._calcSize;

                    if (node._lookupOffsets.Count != node._lookupCount && !(node is MoveDefActionNode))
                    {
                        Console.WriteLine(node.TreePath + (node._lookupCount - node._lookupOffsets.Count));
                    }

                    MoveDefNode._lookupOffsets.AddRange(node._lookupOffsets.ToArray());
                }

                MoveDefEntryNode next = node;
                Top:
                //Check for random params around the file
                if (next.Parent is MoveDefDataNode)
                {
                    if (next.Parent.Children.Count > next.Index + 1)
                    {
                        if (
                            (next = next.Parent.Children[next.Index + 1] as MoveDefEntryNode) is
                            MoveDefCharSpecificNode || next is MoveDefRawDataNode && next.Children.Count > 0 &&
                            next.Children[0] is MoveDefSectionParamNode)
                        {
                            if (!(next is MoveDefRawDataNode))
                            {
                                next.Rebuild(dataAddress, next._calcSize, true);
                                MoveDefNode._lookupOffsets.AddRange(next._lookupOffsets.ToArray());
                                dataAddress += next._calcSize;

                                if (next._lookupCount != next._lookupOffsets.Count)
                                {
                                    Console.WriteLine();
                                }
                            }
                            else
                            {
                                next._entryOffset = dataAddress;
                                foreach (MoveDefSectionParamNode p in next.Children)
                                {
                                    p.Rebuild(dataAddress, p.AttributeBuffer.Length, true);
                                    dataAddress += p.AttributeBuffer.Length;
                                }
                            }

                            goto Top;
                        }
                    }
                }

                return node._rebuildOffset;
            }
            else
            {
                return 0;
            }
        }

        public static int CalcSizeArticleActions(MoveDefDataNode node, ref int lookupCount, bool subactions, int index)
        {
            int size = 0;
            if (node.staticArticles != null && node.staticArticles.Children.Count > 0)
            {
                foreach (MoveDefArticleNode d in node.staticArticles.Children)
                {
                    if (!subactions)
                    {
                        if (d.actions != null)
                        {
                            foreach (MoveDefActionNode a in d.actions.Children)
                            {
                                if (a.Children.Count > 0)
                                {
                                    size += GetSize(a, ref lookupCount);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (d.subActions != null)
                        {
                            foreach (MoveDefSubActionGroupNode grp in d.subActions.Children)
                            {
                                if (grp.Children[index].Children.Count > 0 ||
                                    (grp.Children[index] as MoveDefActionNode)._actionRefs.Count > 0 ||
                                    (grp.Children[index] as MoveDefActionNode)._build)
                                {
                                    size += GetSize(grp.Children[index] as MoveDefActionNode, ref lookupCount);
                                }
                            }
                        }
                    }
                }
            }

            if (node.entryArticle != null)
            {
                if (!subactions)
                {
                    if (node.entryArticle.actions != null)
                    {
                        foreach (MoveDefActionNode a in node.entryArticle.actions.Children)
                        {
                            if (a.Children.Count > 0)
                            {
                                size += GetSize(a, ref lookupCount);
                            }
                        }
                    }
                }
                else
                {
                    if (node.entryArticle.subActions != null)
                    {
                        foreach (MoveDefSubActionGroupNode grp in node.entryArticle.subActions.Children)
                        {
                            if (grp.Children[index].Children.Count > 0 ||
                                (grp.Children[index] as MoveDefActionNode)._actionRefs.Count > 0 ||
                                (grp.Children[index] as MoveDefActionNode)._build)
                            {
                                size += GetSize(grp.Children[index] as MoveDefActionNode, ref lookupCount);
                            }
                        }
                    }
                }
            }

            foreach (MoveDefArticleNode d in node._articles.Values)
            {
                if (!subactions)
                {
                    if (d.actions != null)
                    {
                        if (d.pikmin)
                        {
                            foreach (MoveDefActionGroupNode grp in d.actions.Children)
                            {
                                foreach (MoveDefActionNode a in grp.Children)
                                {
                                    if (a.Children.Count > 0)
                                    {
                                        size += GetSize(a, ref lookupCount);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (MoveDefActionNode a in d.actions.Children)
                            {
                                if (a.Children.Count > 0)
                                {
                                    size += GetSize(a, ref lookupCount);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (d.subActions != null)
                    {
                        MoveDefEntryNode e = d.subActions;
                        int populateCount = 1;
                        bool children = false;
                        if (d.subActions.Children[0] is MoveDefActionListNode)
                        {
                            populateCount = d.subActions.Children.Count;
                            children = true;
                        }

                        for (int i = 0; i < populateCount; i++)
                        {
                            if (children)
                            {
                                e = d.subActions.Children[i] as MoveDefEntryNode;
                            }

                            foreach (MoveDefSubActionGroupNode grp in e.Children)
                            {
                                if (grp.Children[index].Children.Count > 0 ||
                                    (grp.Children[index] as MoveDefActionNode)._actionRefs.Count > 0 ||
                                    (grp.Children[index] as MoveDefActionNode)._build)
                                {
                                    size += GetSize(grp.Children[index] as MoveDefActionNode, ref lookupCount);
                                }
                            }
                        }
                    }
                }
            }

            return size;
        }

        public static void RebuildArticleActions(MoveDefNode RootNode, MoveDefDataNode node, ref VoidPtr dataAddress,
                                                 VoidPtr baseAddress, bool subactions, int index)
        {
            if (node.staticArticles != null && node.staticArticles.Children.Count > 0)
            {
                foreach (MoveDefArticleNode d in node.staticArticles.Children)
                {
                    if (!subactions)
                    {
                        if (d.actions != null)
                        {
                            foreach (MoveDefActionNode a in d.actions.Children)
                            {
                                if (a.Children.Count > 0)
                                {
                                    Rebuild(RootNode, a, ref dataAddress, baseAddress);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (d.subActions != null)
                        {
                            foreach (MoveDefSubActionGroupNode grp in d.subActions.Children)
                            {
                                if (grp.Children[index].Children.Count > 0 ||
                                    (grp.Children[index] as MoveDefActionNode)._actionRefs.Count > 0 ||
                                    (grp.Children[index] as MoveDefActionNode)._build)
                                {
                                    Rebuild(RootNode, grp.Children[index] as MoveDefActionNode, ref dataAddress,
                                        baseAddress);
                                }
                            }
                        }
                    }
                }
            }

            if (node.entryArticle != null)
            {
                if (!subactions)
                {
                    if (node.entryArticle.actions != null)
                    {
                        foreach (MoveDefActionNode a in node.entryArticle.actions.Children)
                        {
                            if (a.Children.Count > 0)
                            {
                                Rebuild(RootNode, a, ref dataAddress, baseAddress);
                            }
                        }
                    }
                }
                else
                {
                    if (node.entryArticle.subActions != null)
                    {
                        foreach (MoveDefSubActionGroupNode grp in node.entryArticle.subActions.Children)
                        {
                            if (grp.Children[index].Children.Count > 0 ||
                                (grp.Children[index] as MoveDefActionNode)._actionRefs.Count > 0 ||
                                (grp.Children[index] as MoveDefActionNode)._build)
                            {
                                Rebuild(RootNode, grp.Children[index] as MoveDefActionNode, ref dataAddress,
                                    baseAddress);
                            }
                        }
                    }
                }
            }

            foreach (MoveDefArticleNode d in node._articles.Values)
            {
                if (!subactions)
                {
                    if (d.actions != null)
                    {
                        if (d.pikmin)
                        {
                            foreach (MoveDefActionGroupNode grp in d.actions.Children)
                            {
                                foreach (MoveDefActionNode a in grp.Children)
                                {
                                    if (a.Children.Count > 0)
                                    {
                                        Rebuild(RootNode, a, ref dataAddress, baseAddress);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (MoveDefActionNode a in d.actions.Children)
                            {
                                if (a.Children.Count > 0)
                                {
                                    Rebuild(RootNode, a, ref dataAddress, baseAddress);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (d.subActions != null)
                    {
                        MoveDefEntryNode e = d.subActions;
                        int populateCount = 1;
                        bool children = false;
                        if (d.subActions.Children[0] is MoveDefActionListNode)
                        {
                            populateCount = d.subActions.Children.Count;
                            children = true;
                        }

                        for (int i = 0; i < populateCount; i++)
                        {
                            if (children)
                            {
                                e = d.subActions.Children[i] as MoveDefEntryNode;
                            }

                            foreach (MoveDefSubActionGroupNode grp in e.Children)
                            {
                                if (grp.Children[index].Children.Count > 0 ||
                                    (grp.Children[index] as MoveDefActionNode)._actionRefs.Count > 0 ||
                                    (grp.Children[index] as MoveDefActionNode)._build)
                                {
                                    Rebuild(RootNode, grp.Children[index] as MoveDefActionNode, ref dataAddress,
                                        baseAddress);
                                }
                            }
                        }
                    }
                }
            }
        }

        //dataCommon:

        //Unknown7 entries
        //Params8
        //Params10
        //Params16
        //Params18
        //Global IC-Basics
        //Unknown23
        //IC-Basics
        //Params24
        //Params12
        //Params13
        //Params14
        //Params15        
        //SSE Global IC-Basics
        //SSE IC-Basics
        //Flash Overlay Actions
        //patternPowerMul parameters
        //Flash Overlay Action Offsets
        //Screen Tint Actions
        //Screen Tint Action Offsets
        //Unknown22 entries
        //Entry/Exit actions alternating
        //Subroutines
        //Unknown7 Data entries
        //Unknown11
        //Leg bones
        //Unknown22 header
        //patternPowerMul header
        //patternPowerMul events
        //Sections data
        //dataCommon header

        public static int CalcDataCommonSize(MoveDefDataCommonNode node)
        {
            return 0;
        }

        internal static void BuildDataCommon(MoveDefDataCommonNode node, int length, bool force)
        {
        }
    }
}