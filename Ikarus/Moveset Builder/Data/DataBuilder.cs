using System;
using System.Collections.Generic;
using Ikarus.MovesetFile;

namespace Ikarus.MovesetBuilder
{
    //TODO: program something that will compare all read entries of each moveset,
    //compare their offsets, determine where each one needs to be written,
    //and write them out in order, 
    //not including repetitive child entries that already appear before their parent

    public unsafe partial class DataBuilder : BuilderBase
    {
        DataSection _data;
        MiscSectionNode _misc;
        FDefSubActionStringTable _subActionTable;
        public OffsetHolder _extraDataOffsets;

        public DataBuilder(DataSection data)
        {
            _moveset = (_data = data)._root as MovesetNode;
            _misc = _data._misc;
            _subActionTable = new FDefSubActionStringTable();
            _extraDataOffsets = ExtraDataOffsets.GetOffsets(_moveset.Character);
            _getPartSize = new Action[]
            {
                GetSizePart1,
                GetSizePart2,
                GetSizePart3,
                GetSizePart4,
                GetSizePart5,
            };
            _buildPart = new Action[]
            {
                BuildPart1,
                BuildPart2,
                BuildPart3,
                BuildPart4,
                BuildPart5,
            };
        }

        DataHeader* dataHeader;
        bint*[] actionArrays = new bint*[2]; //Entry, Exit
        bint*[] subActionArrays = new bint*[4]; //Main, GFX, SFX, Other
        sDataMisc* miscHeader;

        public override void Build(VoidPtr address)
        {
            dataHeader = (DataHeader*)(address + _data._childLength);

            //Action arrays are at the start of part 3
            actionArrays[0] = (bint*)(address + _lengths[0] + _lengths[1]);
            actionArrays[1] = actionArrays[0] + _moveset._actions.Count;

            //Subaction arrays are at the start of part 4
            subActionArrays[0] = (bint*)(address + (_lengths[0] + _lengths[1] + _lengths[2]));
            subActionArrays[1] = (bint*)(subActionArrays[0] + _data._subActions.Count);
            subActionArrays[2] = (bint*)(subActionArrays[1] + _data._subActions.Count);
            subActionArrays[3] = (bint*)(subActionArrays[2] + _data._subActions.Count);

            //Misc is right before the data header
            miscHeader = (sDataMisc*)(address + (_data._childLength - sDataMisc.Size));

            dataHeader->MiscSectionOffset = Offset(miscHeader);
            dataHeader->SubactionMainStart = Offset(subActionArrays[0]);
            dataHeader->SubactionGFXStart = Offset(subActionArrays[1]);
            dataHeader->SubactionSFXStart = Offset(subActionArrays[2]);
            dataHeader->SubactionOtherStart = Offset(subActionArrays[3]);
            dataHeader->EntryActionsStart = Offset(actionArrays[0]);
            dataHeader->ExitActionsStart = Offset(actionArrays[1]);

            base.Build(address);

            dataHeader->Unknown27 = _data.Unknown27;
            dataHeader->Unknown28 = _data.Unknown28;
            dataHeader->Flags1 = _data.Flags1uint;
            dataHeader->Flags2 = _data.Flags2int;

            List<VoidPtr> lookup = _extraDataOffsets.Write(_data, (VoidPtr)dataHeader + DataHeader.Size);

#if DEBUG
            foreach (VoidPtr addr in lookup)
                if ((int)addr < (int)_baseAddress)
                    throw new Exception("Offset value set in lookup, not the address of the offset value.");
#endif

            _lookupAddresses.AddRange(lookup);
        }

        enum ArticleType
        {
            Entry,
            Static,
            Extra,
        }

        private void Write(ArticleNode d, ArticleType type, bool subactions, int index)
        {
            switch (type)
            {
                case ArticleType.Static:
                case ArticleType.Entry:
                    if (!subactions)
                    {
                        //if (d._actions != null)
                        //    foreach (MoveDefActionNode a in d._actions)
                        //        if (a.Children.Count > 0)
                        //            size += GetSize(a, ref lookupCount);
                    }
                    else
                    {
                        //if (d._subActions != null)
                        //    foreach (MoveDefSubActionGroupNode grp in d.subActions.Children)
                        //        if (grp.Children[index].Children.Count > 0 || (grp.Children[index] as MoveDefActionNode)._actionRefs.Count > 0 || (grp.Children[index] as MoveDefActionNode)._build)
                        //            size += GetSize((grp.Children[index] as MoveDefActionNode), ref lookupCount);
                    }
                    break;
            }
        }

        private int CalcSize(ArticleNode d, ArticleType type, bool subactions, int index)
        {
            int size = 0;

            switch (type)
            {
                case ArticleType.Static:
                case ArticleType.Entry:
                    if (!subactions)
                    {
                        //if (d._actions != null)
                        //    foreach (MoveDefActionNode a in d._actions)
                        //        if (a.Children.Count > 0)
                        //            size += GetSize(a, ref lookupCount);
                    }
                    else
                    {
                        //if (d._subActions != null)
                        //    foreach (MoveDefSubActionGroupNode grp in d.subActions.Children)
                        //        if (grp.Children[index].Children.Count > 0 || (grp.Children[index] as MoveDefActionNode)._actionRefs.Count > 0 || (grp.Children[index] as MoveDefActionNode)._build)
                        //            size += GetSize((grp.Children[index] as MoveDefActionNode), ref lookupCount);
                    }
                    break;
            }

            return size;
        }

        public void WriteArticleActions(bool subactions, int index)
        {
            if (_data._staticArticles != null)
                foreach (ArticleNode d in _data._staticArticles)
                    Write(d, ArticleType.Static, subactions, index);

            if (_data._entryArticle != null)
                Write(_data._entryArticle, ArticleType.Entry, subactions, index);

            foreach (ArticleNode d in _data._articles)
                if (!subactions)
                {
                    if (d._actions != null)
                    {

                    }
                }
                else
                {
                    if (d._subActions != null)
                    {

                    }
                }
        }

        public int CalcSizeArticleActions(bool subactions, int index)
        {
            int size = 0;
            if (_data._staticArticles != null)
                foreach (ArticleNode d in _data._staticArticles)
                    size += CalcSize(d, ArticleType.Static, subactions, index);

            if (_data._entryArticle != null)
                size += CalcSize(_data._entryArticle, ArticleType.Entry, subactions, index);

            foreach (ArticleNode d in _data._articles)
                if (!subactions)
                {
                    if (d._actions != null)
                    {
                        //if (d._pikmin)
                        //{
                        //    foreach (MoveDefActionGroupNode grp in d.actions.Children)
                        //        foreach (MoveDefActionNode a in grp.Children)
                        //            if (a.Children.Count > 0)
                        //                size += GetSize(a, ref lookupCount);
                        //}
                        //else
                            //foreach (ActionEntry a in d._actions)
                            //    if (a.Children.Count > 0)
                            //        size += GetSize(a, ref lookupCount);
                    }
                }
                else
                {
                    if (d._subActions != null)
                    {
                        //var e = d._subActions;
                        //int populateCount = 1;
                        //bool children = false;
                        //if (e.Children[0] is MoveDefActionListNode)
                        //{
                        //    populateCount = d.subActions.Children.Count;
                        //    children = true;
                        //}
                        //for (int i = 0; i < populateCount; i++)
                        //{
                        //    if (children)
                        //        e = d.subActions.Children[i] as MoveDefEntryNode;

                        //    foreach (MoveDefSubActionGroupNode grp in e.Children)
                        //        if (grp.Children[index].Children.Count > 0 || (grp.Children[index] as MoveDefActionNode)._actionRefs.Count > 0 || (grp.Children[index] as MoveDefActionNode)._build)
                        //            size += GetSize((grp.Children[index] as MoveDefActionNode), ref lookupCount);
                        //}
                    }
                }

            return size;
        }
    }
}