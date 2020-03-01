using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefArticleNode : MoveDefEntryNode
    {
        internal Article* Header => (Article*) WorkingUncompressed.Address;

        [Browsable(false)]
        public MDL0BoneNode BoneNode
        {
            get
            {
                if (Model == null)
                {
                    return null;
                }

                if (bone > Model._linker.BoneCache.Length || bone < 0)
                {
                    return null;
                }

                return (MDL0BoneNode) Model._linker.BoneCache[bone];
            }
            set => bone = value.BoneIndex;
        }

        [Category("Article")] public int ID => Index;

        [Category("Article")]
        public int ArticleGroupID
        {
            get => id;
            set
            {
                id = value;
                SignalPropertyChange();
            }
        }

        [Category("Article")]
        public int ARCEntryGroup
        {
            get => group;
            set
            {
                group = value;
                SignalPropertyChange();
            }
        }

        [Category("Article")]
        [Browsable(true)]
        [TypeConverter(typeof(DropDownListBonesMDef))]
        public string Bone
        {
            get => BoneNode == null ? bone.ToString() : BoneNode.Name;
            set
            {
                if (Model == null)
                {
                    bone = Convert.ToInt32(value);
                }
                else
                {
                    BoneNode = string.IsNullOrEmpty(value) ? BoneNode : Model.FindBone(value);
                }

                SignalPropertyChange();
            }
        }

        [Category("Article")] public int ActionFlagsStart => aFlags;

        [Category("Article")] public int SubactionFlagsStart => sFlags;

        [Category("Article")] public int ActionsStart => aStart;

        [Category("Article")] public int SubactionMainStart => sMStart;

        [Category("Article")] public int SubactionGFXStart => sGStart;

        [Category("Article")] public int SubactionSFXStart => sSStart;

        [Category("Article")] public int ModelVisibility => visStart;

        [Category("Article")] public int CollisionData => off1;

        [Category("Article")] public int DataOffset2 => off2;

        [Category("Article")] public int DataOffset3 => off3;

        public string ArticleStringID => "ArticleType" + (Static ? "2_" : "1_") + (Name == "Entry Article" ? "Entry" :
            Parent.Name == "Static Articles" ? "Static" + Index : offsetID.ToString());

        public int id, group, bone, aFlags, sFlags, aStart, sMStart, sGStart, sSStart, visStart, off1, off2, off3;

        [Browsable(false)] public bool pikmin => ArticleStringID == "ArticleType1_10" && RootNode.Name == "FitPikmin";

        [Browsable(false)] public bool dedede => ArticleStringID == "ArticleType1_14" && RootNode.Name == "FitDedede";

        public bool Static;
        public bool extraOffset = false;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            id = Header->_id;
            group = Header->_arcGroup;
            bone = Header->_boneID;
            aFlags = Header->_actionFlagsStart;
            sFlags = Header->_subactionFlagsStart;
            aStart = Header->_actionsStart;
            sMStart = Header->_subactionMainStart;
            sGStart = Header->_subactionGFXStart;
            sSStart = Header->_subactionSFXStart;
            visStart = Header->_modelVisibility;
            off1 = Header->_collisionData;
            off2 = Header->_unknownD2;
            off3 = Header->_unknownD3;

            bool extra = false;
            _extraOffsets = new List<int>();
            _extraEntries = new List<MoveDefEntryNode>();
            bint* extraAddr = (bint*) ((VoidPtr) Header + 52);
            for (int i = 0; i < (Size - 52) / 4; i++)
            {
                _extraOffsets.Add(extraAddr[i]);
                if (extraAddr[i] > 0)
                {
                    extra = true;
                }
            }

            Static = Size > 52 && _extraOffsets[0] < 1480 && _extraOffsets[0] >= 0;

            if (_name == null)
            {
                _name = "Article" + ID;
            }

            return SubactionFlagsStart > 0 || extra || ActionsStart > 0 || ActionFlagsStart > 0 || CollisionData > 0 ||
                   DataOffset2 > 0 || DataOffset3 > 0;
        }

        public MoveDefActionFlagsNode actionFlags;
        public MoveDefActionListNode actions;
        public MoveDefFlagsNode subActionFlags;
        public MoveDefEntryNode subActions;
        public MoveDefModelVisibilityNode mdlVis;
        public CollisionDataNode data1;
        public Data2ListNode data2;
        public MoveDefSectionParamNode data3;

        [Category("Article")] public List<int> ExtraOffsets => _extraOffsets;

        public List<int> _extraOffsets;

        [Category("Article")]
        [Browsable(false)]
        public List<MoveDefEntryNode> ExtraEntries => _extraEntries;

        public List<MoveDefEntryNode> _extraEntries;

        public override void OnPopulate()
        {
            int off = 0;
            int actionCount = 0;
            int subactions = Static ? _extraOffsets[0] : (_offset - SubactionFlagsStart) / 8;
            if (ActionFlagsStart > 0)
            {
                actionCount = Root.GetSize(ActionFlagsStart) / 16;
            }

            if (SubactionFlagsStart > 0)
            {
                subactions = Root.GetSize(SubactionFlagsStart) / 8;
            }

            if (actionCount > 0)
            {
                (actionFlags = new MoveDefActionFlagsNode("ActionFlags", actionCount)).Initialize(this,
                    BaseAddress + ActionFlagsStart, actionCount * 16);
                if (ActionsStart > 0 || dedede && _extraOffsets[0] > 0)
                {
                    actions = new MoveDefActionListNode {_name = "Actions"};
                    actions.Parent = this;
                    for (int i = 0; i < actionCount; i++)
                    {
                        if (pikmin)
                        {
                            actions.AddChild(new MoveDefActionGroupNode {_name = "Action" + i}, false);

                            off = *((bint*) (BaseAddress + ActionsStart) + i);
                            if (off > 0)
                            {
                                new MoveDefActionNode("Entry", false, actions.Children[i]).Initialize(
                                    actions.Children[i], BaseAddress + off, 0);
                            }
                            else
                            {
                                actions.Children[i].Children
                                    .Add(new MoveDefActionNode("Entry", true, actions.Children[i]));
                            }

                            off = *((bint*) (BaseAddress + _extraOffsets[0]) + i);
                            if (off > 0)
                            {
                                new MoveDefActionNode("Exit", false, actions.Children[i]).Initialize(
                                    actions.Children[i], BaseAddress + off, 0);
                            }
                            else
                            {
                                actions.Children[i].Children
                                    .Add(new MoveDefActionNode("Exit", true, actions.Children[i]));
                            }
                        }
                        else
                        {
                            off = *((bint*) (BaseAddress + ActionsStart) + i);
                            if (off > 0)
                            {
                                new MoveDefActionNode("Action" + i, false, actions).Initialize(actions,
                                    BaseAddress + off, 0);
                            }
                            else
                            {
                                actions.Children.Add(new MoveDefActionNode("Action" + i, true, actions));
                            }
                        }
                    }
                }
            }

            if (SubactionFlagsStart > 0)
            {
                subActionFlags = new MoveDefFlagsNode {_parent = this};
                subActionFlags.Initialize(this, BaseAddress + SubactionFlagsStart, subactions * 8);

                if (subactions == 0)
                {
                    subactions = subActionFlags._names.Count;
                }

                int populateCount = 1;
                bool child = false;
                int bias = 0;

                if (dedede)
                {
                    subActions = new MoveDefGroupNode {Name = "SubActions"};
                    subActions.AddChild(new MoveDefActionListNode {_name = "Waddle Dee"}, false);
                    subActions.AddChild(new MoveDefActionListNode {_name = "Waddle Doo"}, false);
                    subActions.AddChild(new MoveDefActionListNode {_name = "Gyro"}, false);
                    populateCount = 3;
                    child = true;
                }
                else if (pikmin)
                {
                    subActions = new MoveDefGroupNode {Name = "SubActions"};
                    subActions.AddChild(new MoveDefActionListNode {_name = "Red"}, false);
                    subActions.AddChild(new MoveDefActionListNode {_name = "Yellow"}, false);
                    subActions.AddChild(new MoveDefActionListNode {_name = "Blue"}, false);
                    subActions.AddChild(new MoveDefActionListNode {_name = "Purple"}, false);
                    subActions.AddChild(new MoveDefActionListNode {_name = "White"}, false);
                    populateCount = 5;
                    child = true;
                    bias = 1;
                }
                else if (ArticleStringID == "ArticleType1_61" && RootNode.Name == "FitKirby")
                {
                    subActions = new MoveDefGroupNode {Name = "SubActions"};
                    subActions.AddChild(new MoveDefActionListNode {_name = "100 Ton Stone"}, false);
                    subActions.AddChild(new MoveDefActionListNode {_name = "Thwomp Stone"}, false);
                    subActions.AddChild(new MoveDefActionListNode {_name = "Spike Ball"}, false);
                    subActions.AddChild(new MoveDefActionListNode {_name = "Stone Kirby"}, false);
                    subActions.AddChild(new MoveDefActionListNode {_name = "Happy Stone"}, false);
                    populateCount = 5;
                    child = true;
                }
                else
                {
                    subActions = new MoveDefActionListNode {_name = "SubActions"};
                }

                subActions.Parent = this;

                for (int x = 0; x < populateCount; x++)
                {
                    ResourceNode Base = subActions;
                    int main = SubactionMainStart,
                        gfx = SubactionGFXStart,
                        sfx = SubactionSFXStart;
                    if (child)
                    {
                        Base = subActions.Children[x];
                        main = _extraOffsets[x + bias];
                        gfx = _extraOffsets[x + populateCount + bias];
                        sfx = _extraOffsets[x + populateCount * 2 + bias];
                    }

                    for (int i = 0; i < subactions && i < subActionFlags._names.Count; i++)
                    {
                        Base.AddChild(
                            new MoveDefSubActionGroupNode
                            {
                                _name = subActionFlags._names[i], _flags = subActionFlags._flags[i]._Flags,
                                _inTransTime = subActionFlags._flags[i]._InTranslationTime
                            }, false);
                    }

                    if (main > 0)
                    {
                        for (int i = 0; i < subactions && i < subActionFlags._names.Count; i++)
                        {
                            off = *((bint*) (BaseAddress + main) + i);
                            if (off > 0)
                            {
                                new MoveDefActionNode("Main", false, Base.Children[i]).Initialize(Base.Children[i],
                                    BaseAddress + off, 0);
                            }
                            else
                            {
                                Base.Children[i].Children.Add(new MoveDefActionNode("Main", true, Base.Children[i]));
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < subactions && i < subActionFlags._names.Count; i++)
                        {
                            Base.Children[i].Children.Add(new MoveDefActionNode("Main", true, Base.Children[i]));
                        }
                    }

                    if (gfx > 0)
                    {
                        for (int i = 0; i < subactions && i < subActionFlags._names.Count; i++)
                        {
                            off = *((bint*) (BaseAddress + gfx) + i);
                            if (off > 0)
                            {
                                new MoveDefActionNode("GFX", false, Base.Children[i]).Initialize(Base.Children[i],
                                    BaseAddress + off, 0);
                            }
                            else
                            {
                                Base.Children[i].Children.Add(new MoveDefActionNode("GFX", true, Base.Children[i]));
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < subactions && i < subActionFlags._names.Count; i++)
                        {
                            Base.Children[i].Children.Add(new MoveDefActionNode("GFX", true, Base.Children[i]));
                        }
                    }

                    if (sfx > 0)
                    {
                        for (int i = 0; i < subactions && i < subActionFlags._names.Count; i++)
                        {
                            off = *((bint*) (BaseAddress + sfx) + i);
                            if (off > 0)
                            {
                                new MoveDefActionNode("SFX", false, Base.Children[i]).Initialize(Base.Children[i],
                                    BaseAddress + off, 0);
                            }
                            else
                            {
                                Base.Children[i].Children.Add(new MoveDefActionNode("SFX", true, Base.Children[i]));
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < subactions && i < subActionFlags._names.Count; i++)
                        {
                            Base.Children[i].Children.Add(new MoveDefActionNode("SFX", true, Base.Children[i]));
                        }
                    }
                }
            }

            if (ModelVisibility != 0)
            {
                (mdlVis = new MoveDefModelVisibilityNode()).Initialize(this, BaseAddress + ModelVisibility, 16);
            }

            if (CollisionData != 0)
            {
                (data1 = new CollisionDataNode {_name = "Collision Data"}).Initialize(this,
                    BaseAddress + CollisionData, 8);
            }

            if (DataOffset2 != 0)
            {
                (data2 = new Data2ListNode {_name = "Data2"}).Initialize(this, BaseAddress + DataOffset2, 8);
            }

            if (DataOffset3 != 0)
            {
                (data3 = new MoveDefSectionParamNode {_name = "Data3"}).Initialize(this, BaseAddress + DataOffset3,
                    0);
            }

            //Extra offsets.
            //Dedede:
            //Waddle Dee, Waddle Doo, and Gyro subactions main, gfx, sfx for first 9 offsets.
            //Pikmin:
            //Actions 2 is 1st offset.
            //Red, Yellow, Blue, Purple, and White subactions main, gfx, sfx for next 15 offsets.

            int index = 0;
            foreach (int i in _extraOffsets)
            {
                MoveDefEntryNode entry = null;
                if (index < 9 && dedede)
                {
                }
                else if (index < 16 && pikmin)
                {
                }
                else if (ArticleStringID == "ArticleType1_61" && RootNode.Name == "FitKirby" && index < 15)
                {
                }
                else if (index == 0 && ArticleStringID == "ArticleType1_6" && RootNode.Name == "FitGameWatch")
                {
                    GameWatchArticle6 p = new GameWatchArticle6();
                    p.Initialize(this, BaseAddress + i, 0);
                    entry = p;
                }
                else if (index == 1 && (
                    ArticleStringID == "ArticleType1_8" &&
                    (RootNode.Name == "FitLucas" || RootNode.Name == "FitNess") ||
                    ArticleStringID == "ArticleType1_11" && RootNode.Name == "FitGameWatch" ||
                    ArticleStringID == "ArticleType1_4" && RootNode.Name == "FitWario" ||
                    ArticleStringID == "ArticleType1_5" && RootNode.Name == "FitWarioMan"))
                {
                    MoveDefParamListNode p = new MoveDefParamListNode {_name = "ParamList" + index};
                    p.Initialize(this, BaseAddress + i, 0);
                    entry = p;
                }
                else if (index > 0 && ArticleStringID == "ArticleType1_46" && RootNode.Name == "FitKirby")
                {
                    MoveDefEntryNode p = null;
                    switch (index)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                            p = new MoveDefKirbyArticleP1Node {offsetID = index};
                            break;
                        case 6:
                        case 8:
                            //List of bytes - 1 or 0.
                            //7 & 9 are the index of the last 1 + 1
                            break;
                        case 9: break;
                    }

                    p?.Initialize(this, BaseAddress + i, 0);

                    entry = p;
                }
                else if (index > 0 && (ArticleStringID == "ArticleType1_11" && RootNode.Name == "FitFox" ||
                                       ArticleStringID == "ArticleType1_9" && RootNode.Name == "FitFalco" ||
                                       ArticleStringID == "ArticleType1_11" && RootNode.Name == "FitWolf"))
                {
                    MoveDefEntryNode p = null;
                    switch (index)
                    {
                        case 1:
                            p = new MoveDefUnk17Node {_name = "Bone Floats"};
                            break;
                        case 2:
                            p = new HitDataListOffsetNode {_name = "HitDataList" + index};
                            break;
                        case 3:
                            p = new Fox11Falco9Wolf11Article3Node();
                            break;
                        case 4:
                            p = new ActionOffsetNode {_name = "Data" + index};
                            break;
                        case 5:
                            p = new SecondaryActionOffsetNode {_name = "Data" + index};
                            break;
                        case 6:
                            p = new Fox11Falco9Wolf11PopoArticle63Node {offsetID = index};
                            break;
                    }

                    p?.Initialize(this, BaseAddress + i, 0);

                    entry = p;
                }
                else if ((index == 23 || index == 24) && ArticleStringID == "ArticleType1_10" &&
                         RootNode.Name == "FitPikmin")
                {
                    MoveDefEntryNode p = null;
                    switch (index)
                    {
                        case 23:
                            p = new ActionOffsetNode {_name = "Data" + index};
                            break;
                        case 24:
                            p = new SecondaryActionOffsetNode {_name = "Data" + index};
                            break;
                    }

                    p?.Initialize(this, BaseAddress + i, 0);

                    entry = p;
                }
                else if (index == 3 && ArticleStringID == "ArticleType1_14" && RootNode.Name == "FitPopo")
                {
                    Fox11Falco9Wolf11PopoArticle63Node p = new Fox11Falco9Wolf11PopoArticle63Node {offsetID = index};
                    p.Initialize(this, BaseAddress + i, 0);
                    entry = p;
                }
                else if (index > 4 && ArticleStringID == "ArticleType1_7" && RootNode.Name == "FitSonic")
                {
                    MoveDefEntryNode p = null;
                    switch (index)
                    {
                        case 5:
                            p = new ActionOffsetNode {_name = "Data" + index};
                            break;
                        case 6:
                            p = new SecondaryActionOffsetNode {_name = "Data" + index};
                            break;
                    }

                    p?.Initialize(this, BaseAddress + i, 0);

                    entry = p;
                }
                else if (dedede && index == 11)
                {
                    DededeHitDataList p = new DededeHitDataList();
                    p.Initialize(this, BaseAddress + i, 0);
                    entry = p;
                }
                else if (index == 3 && ArticleStringID == "ArticleType1_4" && RootNode.Name == "FitGanon" ||
                         index == 1 && ArticleStringID == "ArticleType1_7" && RootNode.Name == "FitSonic")
                {
                    MoveDefHitDataListNode p = new MoveDefHitDataListNode {_name = "HitData"};
                    p.Initialize(this, BaseAddress + i, 0);
                    entry = p;
                }
                else
                {
                    if (i > 1480 && i < Root.dataSize)
                    {
                        MoveDefExternalNode e = Root.IsExternal(i);
                        if (e != null && e.Name.Contains("hitData"))
                        {
                            MoveDefHitDataListNode p = new MoveDefHitDataListNode {_name = e.Name};
                            p.Initialize(this, new DataSource(BaseAddress + i, 0));
                            entry = p;
                        }
                        else if (index < _extraOffsets.Count - 1 && _extraOffsets[index + 1] < 1480 &&
                                 _extraOffsets[index + 1] > 1)
                        {
                            int count = _extraOffsets[index + 1];
                            int size = Root.GetSize(i);
                            if (size > 0 && count > 0)
                            {
                                size /= count;
                                MoveDefRawDataNode d = new MoveDefRawDataNode("ExtraParams" + index);
                                entry = d;
                                d.Initialize(this, BaseAddress + i, 0);
                                for (int x = 0; x < count; x++)
                                {
                                    new MoveDefSectionParamNode {_name = "Part" + x}.Initialize(d,
                                        BaseAddress + i + x * size, size);
                                }
                            }
                        }
                        else
                        {
                            if (e != null && e.Name.Contains("hitData"))
                            {
                                MoveDefHitDataListNode p = new MoveDefHitDataListNode {_name = e.Name};
                                entry = p;
                                p.Initialize(this, new DataSource(BaseAddress + i, 0));
                            }
                            else
                            {
                                (entry = new MoveDefSectionParamNode {_name = "ExtraParams" + index}).Initialize(this,
                                    BaseAddress + i, 0);
                            }
                        }
                    }
                }

                _extraEntries.Add(entry);
                index++;
            }
        }

        public FDefSubActionStringTable subActionStrings;
        public VoidPtr actionAddr;

        public override int OnCalculateSize(bool force)
        {
            _buildHeader = true;
            _lookupCount = 0;
            subActionStrings = new FDefSubActionStringTable();
            _entryLength = 52 + _extraOffsets.Count * 4;

            int size = 0;

            if (actionFlags != null)
            {
                _lookupCount++; //action flags offset
                size += 16 * actionFlags.Children.Count;
            }

            if (actions != null)
            {
                if (pikmin)
                {
                    //false for now
                    bool actions1Null = false, actions2Null = false;
                    foreach (MoveDefActionGroupNode grp in actions.Children)
                    {
                        foreach (MoveDefActionNode a in grp.Children)
                        {
                            if (a.Children.Count > 0)
                            {
                                if (a.Index == 0)
                                {
                                    actions1Null = false;
                                }
                                else if (a.Index == 1)
                                {
                                    actions2Null = false;
                                }
                            }
                        }
                    }

                    _lookupCount += 2; //actions offsets
                    if (!actions1Null || !actions2Null)
                    {
                        foreach (MoveDefActionGroupNode grp in actions.Children)
                        {
                            foreach (MoveDefActionNode a in grp.Children)
                            {
                                if (a.Children.Count > 0)
                                {
                                    _lookupCount++; //action offset
                                }
                            }
                        }

                        size += actions.Children.Count * 8;
                    }
                }
                else
                {
                    bool actionsNull = true;
                    foreach (MoveDefActionNode a in actions.Children)
                    {
                        if (a.Children.Count > 0)
                        {
                            actionsNull = false;
                        }
                    }

                    if (!actionsNull)
                    {
                        _lookupCount++; //actions offsets
                        foreach (MoveDefActionNode a in actions.Children)
                        {
                            if (a.Children.Count > 0)
                            {
                                _lookupCount++; //action offset
                            }
                        }

                        size += actions.Children.Count * 4;
                    }
                }
            }

            if (subActions != null)
            {
                if (subActions.Children.Count > 0)
                {
                    _lookupCount++; //subaction flags offset
                }

                bool mainNull = true, gfxNull = true, sfxNull = true;
                MoveDefEntryNode e = subActions;
                int populateCount = 1;
                bool children = false;
                if (subActions.Children[0] is MoveDefActionListNode)
                {
                    populateCount = subActions.Children.Count;
                    children = true;
                }

                for (int i = 0; i < populateCount; i++)
                {
                    if (children)
                    {
                        e = subActions.Children[i] as MoveDefEntryNode;
                    }

                    foreach (MoveDefSubActionGroupNode g in e.Children)
                    {
                        if (i == 0)
                        {
                            subActionStrings.Add(g.Name);
                            _lookupCount++; //subaction name offset
                            size += 8;
                        }

                        //bool write = true;
                        //if (!Static)
                        //{
                        //    write = false;
                        //    foreach (MoveDefActionNode a in g.Children)
                        //        if (a.Children.Count > 0 || a._actionRefs.Count > 0)
                        //            write = true;
                        //}
                        foreach (MoveDefActionNode a in g.Children)
                        {
                            //if ((Static && a.Children.Count > 0) || (!Static && write))
                            if (a.Children.Count > 0 || a._actionRefs.Count > 0 || a._build)
                            {
                                switch (a.Index)
                                {
                                    case 0:
                                        mainNull = false;
                                        break;
                                    case 1:
                                        gfxNull = false;
                                        break;
                                    case 2:
                                        sfxNull = false;
                                        break;
                                }

                                _lookupCount++; //action offset
                            }
                        }
                    }
                }

                size += subActionStrings.TotalSize;
                for (int i = 0; i < populateCount; i++)
                {
                    if (children)
                    {
                        e = subActions.Children[i] as MoveDefEntryNode;
                    }

                    if (!(mainNull && Static))
                    {
                        _lookupCount++; //main actions offset
                        size += e.Children.Count * 4;
                    }

                    if (!(gfxNull && Static))
                    {
                        _lookupCount++; //gfx actions offset
                        size += e.Children.Count * 4;
                    }

                    if (!(sfxNull && Static))
                    {
                        _lookupCount++; //sfx actions offset
                        size += e.Children.Count * 4;
                    }
                }
            }

            if (mdlVis != null)
            {
                _lookupCount++; //model vis offset
                size += mdlVis.CalculateSize(true);
                _lookupCount += mdlVis._lookupCount;
            }

            if (data1 != null)
            {
                _lookupCount++; //data 1 offset
                if (!data1.External)
                {
                    size += data1.CalculateSize(true);
                    _lookupCount += data1._lookupCount;
                }
            }

            if (data2 != null)
            {
                _lookupCount++; //data 2 offset
                if (!data2.External)
                {
                    size += data2.CalculateSize(true);
                    _lookupCount += data2._lookupCount;
                }
            }

            if (data3 != null)
            {
                _lookupCount++; //data 3 offset
                if (!data3.External)
                {
                    size += data3.CalculateSize(true);
                }
            }

            foreach (MoveDefEntryNode e in _extraEntries)
            {
                if (e != null)
                {
                    if (!e.External)
                    {
                        size += e.CalculateSize(true);
                        _lookupCount += e._lookupCount;
                    }

                    _lookupCount++;
                }
            }

            _childLength = size;

            return _childLength + _entryLength;
        }

        public bool _buildHeader = true;

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            aFlags = sFlags = aStart = sMStart = sGStart = sSStart = visStart = off1 = off2 = off3 = 0;

            int a2Start = 0;

            List<int> mainStarts = null, gfxStarts = null, sfxStarts = null;

            VoidPtr addr = address;

            if (subActions != null)
            {
                subActionStrings.WriteTable(addr);
                addr += subActionStrings.TotalSize;
            }

            if (actionFlags != null)
            {
                ActionFlags* actionFlagsAddr = (ActionFlags*) addr;
                aFlags = (int) actionFlagsAddr - (int) _rebuildBase;

                foreach (MoveDefActionFlagsEntryNode a in actionFlags.Children)
                {
                    a.Rebuild(actionFlagsAddr++, 16, true);
                }

                addr = (VoidPtr) actionFlagsAddr;
            }

            if (actions != null)
            {
                if (pikmin)
                {
                    //false for now
                    bool actions1Null = false, actions2Null = false;
                    foreach (MoveDefActionGroupNode grp in actions.Children)
                    {
                        foreach (MoveDefActionNode a in grp.Children)
                        {
                            if (a.Children.Count > 0)
                            {
                                if (a.Index == 0)
                                {
                                    actions1Null = false;
                                }
                                else if (a.Index == 1)
                                {
                                    actions2Null = false;
                                }
                            }
                        }
                    }

                    if (!actions1Null || !actions2Null)
                    {
                        bint* action1Offsets = (bint*) addr;
                        aStart = (int) action1Offsets - (int) _rebuildBase;
                        bint* action2Offsets = (bint*) (addr + actions.Children.Count * 4);
                        a2Start = (int) action2Offsets - (int) _rebuildBase;

                        foreach (MoveDefActionGroupNode grp in actions.Children)
                        {
                            foreach (MoveDefActionNode a in grp.Children)
                            {
                                if (a.Index == 0)
                                {
                                    if (a.Children.Count > 0)
                                    {
                                        *action1Offsets = (int) a._entryOffset - (int) a._rebuildBase;
                                        _lookupOffsets.Add((int) action1Offsets - (int) _rebuildBase);
                                    }

                                    action1Offsets++;
                                }
                                else if (a.Index == 1)
                                {
                                    if (a.Children.Count > 0)
                                    {
                                        *action2Offsets = (int) a._entryOffset - (int) a._rebuildBase;
                                        _lookupOffsets.Add((int) action2Offsets - (int) _rebuildBase);
                                    }

                                    action2Offsets++;
                                }
                            }
                        }

                        addr = (VoidPtr) action2Offsets;
                    }
                }
                else
                {
                    bool actionsNull = true;
                    foreach (MoveDefActionNode a in actions.Children)
                    {
                        if (a.Children.Count > 0)
                        {
                            actionsNull = false;
                        }
                    }

                    if (!actionsNull)
                    {
                        bint* actionOffsets = (bint*) addr;
                        aStart = (int) actionOffsets - (int) _rebuildBase;

                        foreach (MoveDefActionNode a in actions.Children)
                        {
                            if (a.Children.Count > 0)
                            {
                                *actionOffsets = (int) a._entryOffset - (int) a._rebuildBase;
                                _lookupOffsets.Add((int) actionOffsets - (int) _rebuildBase);
                            }

                            actionOffsets++;
                        }

                        addr = (VoidPtr) actionOffsets;
                    }
                }
            }

            if (mdlVis != null)
            {
                mdlVis.Rebuild(addr, mdlVis._calcSize, true);
                visStart = (int) mdlVis._rebuildOffset;
                _lookupOffsets.AddRange(mdlVis._lookupOffsets);
                addr += mdlVis._calcSize;
            }

            if (data1 != null)
            {
                if (!data1.External)
                {
                    data1.Rebuild(addr, data1._calcSize, true);
                    _lookupOffsets.AddRange(data1._lookupOffsets);
                    addr += data1._calcSize;
                }

                off1 = (int) data1._entryOffset - (int) _rebuildBase;
            }

            if (data2 != null)
            {
                if (!data2.External)
                {
                    data2.Rebuild(addr, data2._calcSize, true);
                    _lookupOffsets.AddRange(data2._lookupOffsets);
                    addr += data2._calcSize;
                }

                off2 = (int) data2._entryOffset - (int) _rebuildBase;
            }

            if (data3 != null)
            {
                if (data3.External)
                {
                    off3 = (int) data3._entryOffset - (int) _rebuildBase;
                }
                else
                {
                    off3 = (int) addr - (int) _rebuildBase;
                    data3.Rebuild(addr, data3._calcSize, true);
                    addr += data3._calcSize;
                }
            }

            if (subActions != null && subActions.Children.Count > 0)
            {
                bint* lastOffsets = null, mainOffsets, GFXOffsets, SFXOffsets;
                FDefSubActionFlag* subActionFlagsAddr = null;
                bool mainNull = true, gfxNull = true, sfxNull = true;
                MoveDefEntryNode e = subActions;
                int populateCount = 1;
                bool children = false;
                if (subActions.Children[0] is MoveDefActionListNode)
                {
                    populateCount = subActions.Children.Count;
                    children = true;
                    mainStarts = new List<int>();
                    gfxStarts = new List<int>();
                    sfxStarts = new List<int>();
                    sMStart = 0;
                    sGStart = 0;
                    sSStart = 0;
                }

                for (int i = 0; i < populateCount; i++)
                {
                    if (children)
                    {
                        e = subActions.Children[i] as MoveDefEntryNode;
                    }

                    foreach (MoveDefSubActionGroupNode g in e.Children)
                    {
                        foreach (MoveDefActionNode a in g.Children)
                        {
                            if (a.Children.Count > 0 || a._actionRefs.Count > 0 || a._build)
                            {
                                switch (a.Index)
                                {
                                    case 0:
                                        mainNull = false;
                                        break;
                                    case 1:
                                        gfxNull = false;
                                        break;
                                    case 2:
                                        sfxNull = false;
                                        break;
                                }
                            }
                        }
                    }

                    if (i == 0)
                    {
                        subActionFlagsAddr = (FDefSubActionFlag*) addr;
                        sFlags = (int) subActionFlagsAddr - (int) _rebuildBase;
                        lastOffsets = (bint*) ((VoidPtr) subActionFlagsAddr + e.Children.Count * 8);
                    }

                    mainOffsets = lastOffsets;

                    if (!(mainNull && Static))
                    {
                        if (!children)
                        {
                            sMStart = (int) mainOffsets - (int) _rebuildBase;
                        }
                        else
                        {
                            mainStarts.Add((int) mainOffsets - (int) _rebuildBase);
                        }

                        GFXOffsets = (bint*) ((VoidPtr) mainOffsets + e.Children.Count * 4);
                    }
                    else
                    {
                        GFXOffsets = mainOffsets;
                    }

                    if (!(gfxNull && Static))
                    {
                        if (!children)
                        {
                            sGStart = (int) GFXOffsets - (int) _rebuildBase;
                        }
                        else
                        {
                            gfxStarts.Add((int) GFXOffsets - (int) _rebuildBase);
                        }

                        SFXOffsets = (bint*) ((VoidPtr) GFXOffsets + e.Children.Count * 4);
                    }
                    else
                    {
                        SFXOffsets = GFXOffsets;
                    }

                    if (!(sfxNull && Static))
                    {
                        if (!children)
                        {
                            sSStart = (int) SFXOffsets - (int) _rebuildBase;
                        }
                        else
                        {
                            sfxStarts.Add((int) SFXOffsets - (int) _rebuildBase);
                        }

                        addr = (VoidPtr) SFXOffsets + e.Children.Count * 4;
                    }
                    else
                    {
                        addr = (VoidPtr) SFXOffsets;
                    }

                    lastOffsets = (bint*) addr;

                    int x = 0; //bool write = true;
                    foreach (MoveDefSubActionGroupNode grp in e.Children)
                    {
                        if (i == 0)
                        {
                            *subActionFlagsAddr = new FDefSubActionFlag
                            {
                                _Flags = grp._flags, _InTranslationTime = grp._inTransTime,
                                _stringOffset = (int) subActionStrings[grp.Name] - (int) _rebuildBase
                            };

                            if (subActionFlagsAddr->_stringOffset > 0)
                            {
                                _lookupOffsets.Add((int) subActionFlagsAddr->_stringOffset.Address -
                                                   (int) _rebuildBase);
                            }

                            subActionFlagsAddr++;
                        }

                        //if (!Static)
                        //{
                        //    write = false;
                        //    foreach (MoveDefActionNode a in grp.Children)
                        //        if (a.Children.Count > 0 || a._actionRefs.Count > 0)
                        //            write = true;
                        //}
                        //if ((Static && grp.Children[0].Children.Count > 0) || (!Static && write))
                        if (grp.Children[0].Children.Count > 0 ||
                            (grp.Children[0] as MoveDefActionNode)._actionRefs.Count > 0 ||
                            (grp.Children[0] as MoveDefActionNode)._build)
                        {
                            mainOffsets[x] = (int) (grp.Children[0] as MoveDefActionNode)._entryOffset -
                                             (int) _rebuildBase;
                            _lookupOffsets.Add((int) &mainOffsets[x] - (int) _rebuildBase);
                        }

                        //if ((Static && grp.Children[1].Children.Count > 0) || (!Static && write))
                        if (grp.Children[1].Children.Count > 0 ||
                            (grp.Children[1] as MoveDefActionNode)._actionRefs.Count > 0 ||
                            (grp.Children[1] as MoveDefActionNode)._build)
                        {
                            GFXOffsets[x] = (int) (grp.Children[1] as MoveDefActionNode)._entryOffset -
                                            (int) _rebuildBase;
                            _lookupOffsets.Add((int) &GFXOffsets[x] - (int) _rebuildBase);
                        }

                        //if ((Static && grp.Children[2].Children.Count > 0) || (!Static && write))
                        if (grp.Children[2].Children.Count > 0 ||
                            (grp.Children[2] as MoveDefActionNode)._actionRefs.Count > 0 ||
                            (grp.Children[2] as MoveDefActionNode)._build)
                        {
                            SFXOffsets[x] = (int) (grp.Children[2] as MoveDefActionNode)._entryOffset -
                                            (int) _rebuildBase;
                            _lookupOffsets.Add((int) &SFXOffsets[x] - (int) _rebuildBase);
                        }

                        x++;
                    }
                }

                addr = lastOffsets;
            }

            foreach (MoveDefEntryNode e in _extraEntries)
            {
                if (e != null)
                {
                    if (!e.External)
                    {
                        e.Rebuild(addr, e._calcSize, true);
                        _lookupOffsets.AddRange(e._lookupOffsets);
                        if (e._lookupOffsets.Count != e._lookupCount)
                        {
                            Console.WriteLine(e._lookupCount - e._lookupOffsets.Count);
                        }

                        addr += e._calcSize;
                    }
                }
            }

            if (_buildHeader)
            {
                _entryOffset = addr;

                Article* article = (Article*) addr;

                article->_id = id;
                article->_boneID = bone;
                article->_arcGroup = group;

                article->_actionsStart = aStart;
                article->_actionFlagsStart = aFlags;
                article->_subactionFlagsStart = sFlags;
                article->_subactionMainStart = sMStart;
                article->_subactionGFXStart = sGStart;
                article->_subactionSFXStart = sSStart;
                article->_modelVisibility = visStart;
                article->_collisionData = off1;
                article->_unknownD2 = off2;
                article->_unknownD3 = off3;

                bint* ext = (bint*) (addr + 52);
                int index = 0;
                if (_extraEntries.Count > 0)
                {
                    foreach (int i in _extraOffsets)
                    {
                        MoveDefEntryNode e = _extraEntries[index];
                        if (e != null)
                        {
                            ext[index] = (int) e._entryOffset - (int) _rebuildBase;
                            _lookupOffsets.Add((int) &ext[index] - (int) _rebuildBase);
                        }
                        else if (index == 0 && Static)
                        {
                            ext[index] = subActions == null ? 0 : subActions.Children.Count;
                        }
                        else
                        {
                            ext[index] = i;
                        }

                        index++;
                    }
                }

                index = 0;

                if (pikmin)
                {
                    ext[0] = a2Start;
                    _lookupOffsets.Add((int) &ext[0] - (int) _rebuildBase);
                }

                int bias = pikmin ? 1 : 0;
                if (mainStarts != null)
                {
                    foreach (int i in mainStarts)
                    {
                        ext[index + bias] = i;
                        _lookupOffsets.Add((int) &ext[index + bias] - (int) _rebuildBase);
                        index++;
                    }
                }

                if (gfxStarts != null)
                {
                    foreach (int i in gfxStarts)
                    {
                        ext[index + bias] = i;
                        _lookupOffsets.Add((int) &ext[index + bias] - (int) _rebuildBase);
                        index++;
                    }
                }

                if (sfxStarts != null)
                {
                    foreach (int i in sfxStarts)
                    {
                        ext[index + bias] = i;
                        _lookupOffsets.Add((int) &ext[index + bias] - (int) _rebuildBase);
                        index++;
                    }
                }

                if ((int) (addr + 52 + _extraOffsets.Count * 4) - (int) address != _calcSize)
                {
                    Console.WriteLine(_calcSize - ((int) (addr + 52 + _extraOffsets.Count * 4) - (int) address));
                }

                //Add all header offsets
                bint* off = (bint*) ((VoidPtr) article + 12);
                for (int i = 0; i < 10; i++)
                {
                    if (off[i] > 1480 && off[i] < Root.dataSize)
                    {
                        _lookupOffsets.Add((int) &off[i] - (int) _rebuildBase);
                    }
                }

                if (_lookupOffsets.Count != _lookupCount)
                {
                    Console.WriteLine(_lookupCount - _lookupOffsets.Count);
                }
            }
            else
            {
                if ((int) addr - (int) address != _childLength)
                {
                    Console.WriteLine((int) addr - (int) address);
                }
            }
        }

        /*
        public int currentSubAction = 0;
        public MDL0Node Model = null;

        public void Attach(TKContext ctx)
        {
            if (Model != null)
            {
                Model.Attach(ctx);
            }
        }

        public void Detach()
        {
            if (Model != null)
            {
                Model.Detach();
            }
        }

        public void Refesh()
        {
            if (Model != null)
            {
                Model.Refesh();
            }
        }

        public void GetBox(out Vector3 min, out Vector3 max)
        {
            min = new Vector3();
            max = new Vector3();
        }

        public void Render(TKContext ctx, ModelPanel mainWindow)
        {
            if (Model != null)
            {
                Model.Render(ctx, mainWindow);
                if (Model._linker.BoneCache.Length > 0 && BoneNode != null)
                {
                    MDL0BoneNode b = Model._linker.BoneCache[0] as MDL0BoneNode;
                    b._frameMatrix = BoneNode._frameMatrix * b._frameState._transform;
                    b._inverseFrameMatrix = b._frameState._iTransform * BoneNode._inverseFrameMatrix;
                    foreach (MDL0BoneNode bone in b.Children)
                    {
                        bone.RecalcFrameState();
                    }
                }
            }
        }
        */
    }

    public unsafe class CollDataType0 : MoveDefEntryNode
    {
        internal collData0* Header => (collData0*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public int type, offset, count;
        public float unk1, unk2, unk3;

        [Category("Collision Data Type 0")] public int Type => type;

        [Category("Collision Data Type 0")] public int Offset => offset;

        [Category("Collision Data Type 0")] public int Count => count;

        [Category("Collision Data Type 0")]
        public float Unknown1
        {
            get => unk1;
            set
            {
                unk1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Collision Data Type 0")]
        public float Unknown2
        {
            get => unk2;
            set
            {
                unk2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Collision Data Type 0")]
        public float Unknown3
        {
            get => unk3;
            set
            {
                unk3 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            type = Header->type;
            offset = Header->_list._startOffset;
            count = Header->_list._listCount;
            unk1 = Header->unk1;
            unk2 = Header->unk2;
            unk3 = Header->unk3;
            return Offset > 0 && Count > 0;
        }

        public override void OnPopulate()
        {
            VoidPtr addr = BaseAddress + Offset;
            for (int i = 0; i < count; i++)
            {
                new MoveDefBoneIndexNode().Initialize(this, addr + i * 4, 4);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            _lookupCount = Children.Count > 0 ? 1 : 0;
            return 24 + Children.Count * 4;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            bint* addr = (bint*) address;
            foreach (MoveDefBoneIndexNode b in Children)
            {
                b._entryOffset = addr;
                *addr++ = b.boneIndex;
            }

            _entryOffset = addr;
            collData0* data = (collData0*) addr;
            data->type = 0;
            data->unk1 = unk1;
            data->unk2 = unk2;
            data->unk3 = unk3;
            if (Children.Count > 0)
            {
                data->_list._startOffset = (int) address - (int) _rebuildBase;
                _lookupOffsets.Add((int) data->_list._startOffset.Address - (int) _rebuildBase);
            }

            data->_list._listCount = Children.Count;
        }
    }

    public unsafe class CollisionDataNode : MoveDefEntryNode
    {
        internal FDefListOffset* Header => (FDefListOffset*) WorkingUncompressed.Address;

        //public override ResourceType ResourceType { get { return ResourceType.Unknown; } }
        private FDefListOffset hdr;
        [Category("List Offset")] public int DataOffset => hdr._startOffset;

        [Category("List Offset")] public int Count => hdr._listCount;

        public override void OnPopulate()
        {
            if (DataOffset > 0)
            {
                MoveDefOffsetNode o;
                bint* addr = (bint*) (BaseAddress + DataOffset);
                for (int i = 0; i < Count; i++)
                {
                    (o = new MoveDefOffsetNode {_name = "Entry"}).Initialize(this, addr++, 4);
                    if (o.DataOffset > 0)
                    {
                        int value = *(int*) (BaseAddress + o.DataOffset);
                        switch (value)
                        {
                            case 0:
                                new CollDataType0 {_name = "Data"}.Initialize(o, BaseAddress + o.DataOffset, 0);
                                break;
                            case 1:
                                new CollDataType1 {_name = "Data"}.Initialize(o, BaseAddress + o.DataOffset, 0);
                                break;
                            case 2:
                                new CollDataType2 {_name = "Data"}.Initialize(o, BaseAddress + o.DataOffset, 0);
                                break;
                            default:
                                Console.WriteLine(value);
                                new MoveDefSectionParamNode {_name = "Data"}.Initialize(o, BaseAddress + o.DataOffset,
                                    0);
                                break;
                        }
                    }
                }
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            hdr = *Header;
            return hdr._startOffset > 0;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            bint* offsets = (bint*) address;
            VoidPtr dataAddr = address;
            if (Children.Count > 0)
            {
                foreach (MoveDefOffsetNode o in Children)
                {
                    if (o.Children.Count > 0 && !(o.Children[0] as MoveDefEntryNode).External)
                    {
                        o.Children[0].Rebuild(dataAddr, o.Children[0]._calcSize, true);
                        _lookupOffsets.AddRange((o.Children[0] as MoveDefEntryNode)._lookupOffsets);
                        dataAddr += o.Children[0]._calcSize;
                    }
                }

                offsets = (bint*) dataAddr;
                foreach (MoveDefOffsetNode o in Children)
                {
                    if (o.Children.Count > 0)
                    {
                        *offsets = (int) (o.Children[0] as MoveDefEntryNode)._entryOffset - (int) _rebuildBase;
                        _lookupOffsets.Add((int) offsets - (int) _rebuildBase); //offset to child
                    }

                    offsets++;
                }
            }

            _entryOffset = offsets;
            FDefListOffset* header = (FDefListOffset*) offsets;

            header->_listCount = Children.Count;
            if (Children.Count > 0)
            {
                header->_startOffset = (int) dataAddr - (int) _rebuildBase;
                _lookupOffsets.Add((int) header->_startOffset.Address - (int) _rebuildBase);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            _entryLength = 8;
            _childLength = 0;
            if (Children.Count > 0)
            {
                _lookupCount++; //offset to children
                foreach (MoveDefOffsetNode o in Children)
                {
                    _childLength += 4;
                    if (o.Children.Count > 0)
                    {
                        _lookupCount++; //offset to child
                        if (!(o.Children[0] as MoveDefEntryNode).External)
                        {
                            _childLength += o.Children[0].CalculateSize(true);
                            _lookupCount += (o.Children[0] as MoveDefEntryNode)._lookupCount;
                        }
                    }
                }
            }

            return _childLength + _entryLength;
        }
    }

    public unsafe class CollDataType1 : MoveDefEntryNode
    {
        internal collData1* Header => (collData1*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public int type;
        public float unk1, unk2, unk3;

        [Category("Collision Data Type 1")] public int Type => type;

        [Category("Collision Data Type 1")]
        public float Unknown1
        {
            get => unk1;
            set
            {
                unk1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Collision Data Type 1")]
        public float Unknown2
        {
            get => unk2;
            set
            {
                unk2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Collision Data Type 1")]
        public float Unknown3
        {
            get => unk3;
            set
            {
                unk3 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            type = Header->type;
            unk1 = Header->unk1;
            unk2 = Header->unk2;
            unk3 = Header->unk3;
            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return 16;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            collData1* data = (collData1*) address;
            data->type = 1;
            data->unk1 = unk1;
            data->unk2 = unk2;
            data->unk3 = unk3;
        }
    }

    public unsafe class CollDataType2 : MoveDefEntryNode
    {
        internal collData2* Header => (collData2*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public int type, flags;
        public float unk1, unk2, unk3, unk4;

        [Category("Collision Data Type 2")] public int Type => type;

        [Category("Collision Data Type 2")]
        public int Flags
        {
            get => flags;
            set
            {
                flags = value;
                SignalPropertyChange();
            }
        }

        [Category("Collision Data Type 2")]
        public float Unknown1
        {
            get => unk1;
            set
            {
                unk1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Collision Data Type 2")]
        public float Unknown2
        {
            get => unk2;
            set
            {
                unk2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Collision Data Type 2")]
        public float Unknown3
        {
            get => unk3;
            set
            {
                unk3 = value;
                SignalPropertyChange();
            }
        }

        [Category("Collision Data Type 2")]
        public float Unknown4
        {
            get => unk4;
            set
            {
                unk4 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            type = Header->type;
            flags = Header->flags;
            unk1 = Header->unk1;
            unk2 = Header->unk2;
            unk3 = Header->unk3;

            if ((flags & 2) == 2)
            {
                unk4 = Header->unk4;
            }

            if (Size != 24 && Size != 20)
            {
                Console.WriteLine(Size);
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return (flags & 2) == 2 ? 24 : 20;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            collData2* data = (collData2*) address;
            data->type = 2;
            data->flags = flags;
            data->unk1 = unk1;
            data->unk2 = unk2;
            data->unk3 = unk3;
            if ((flags & 2) == 2)
            {
                data->unk4 = unk4;
            }
        }
    }
}