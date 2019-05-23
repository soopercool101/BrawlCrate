using System;
using System.Collections.Generic;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using Ikarus.ModelViewer;

namespace Ikarus.MovesetFile
{
    public unsafe class ArticleNode : MovesetEntryNode
    {
        [Browsable(false)]
        public MDL0BoneNode CharBoneNode
        {
            get
            {
                MovesetNode node = (MovesetNode)_root;
                if (node.Model == null)
                    return null;
                if (_charBone > node.Model._linker.BoneCache.Length || _charBone < 0)
                    return null;
                return (MDL0BoneNode)node.Model._linker.BoneCache[_charBone];
            }
            set { _charBone = value.BoneIndex; }
        }

        [Browsable(false)]
        public MDL0BoneNode ArticleBoneNode
        {
            get
            {
                if (_info == null || _info._model == null)
                    return null;
                if (_articleBone >= _info._model.BoneCache.Length || _articleBone < 0)
                    return null;
                return (MDL0BoneNode)_info._model.BoneCache[_articleBone];
            }
            set { _articleBone = value.BoneIndex; }
        }

        [Category("Article"), Browsable(false)]
        public int ID { get { return Index; } }
        [Category("Article")]
        public int ARCGroupID { get { return _id; } set { _id = value; SignalPropertyChange(); } }
        [Category("Article"), Browsable(true), TypeConverter(typeof(DropDownListBonesArticleMDef))]
        public string ArticleBone { get { return ArticleBoneNode == null ? _articleBone.ToString() : ArticleBoneNode.Name; } set { if (Model == null) { _articleBone = Convert.ToInt32(value); } else { ArticleBoneNode = String.IsNullOrEmpty(value) ? ArticleBoneNode : _info._model.FindBone(value); } SignalPropertyChange(); } }
        [Category("Article"), Browsable(true), TypeConverter(typeof(DropDownListBonesMDef))]
        public string CharacterBone { get { return CharBoneNode == null ? _charBone.ToString() : CharBoneNode.Name; } set { if (Model == null) { _charBone = Convert.ToInt32(value); } else { CharBoneNode = String.IsNullOrEmpty(value) ? CharBoneNode : Model.FindBone(value); } SignalPropertyChange(); } }
        [Category("Article"), Browsable(true)]
        public int ActionFlagsStart { get { return _actionFlagsOffset; } }
        [Category("Article"), Browsable(true)]
        public int SubactionFlagsStart { get { return _subactionFlagsOffset; } }
        [Category("Article"), Browsable(true)]
        public int ActionsStart { get { return _actionArrayOffset; } }
        [Category("Article"), Browsable(true)]
        public int SubactionMainStart { get { return _subactionsMainArrayOffset; } }
        [Category("Article"), Browsable(true)]
        public int SubactionGFXStart { get { return _subactionGFXArrayOffset; } }
        [Category("Article"), Browsable(true)]
        public int SubactionSFXStart { get { return _subactionSFXArrayOffset; } }
        [Category("Article"), Browsable(true)]
        public int ModelVisibility { get { return _modelVisOffset; } }
        [Category("Article"), Browsable(true)]
        public int CollisionData { get { return _collisionDataOffset; } }
        [Category("Article"), Browsable(true)]
        public int AddAreaDataSetList { get { return _off2; } }
        [Category("Article"), Browsable(true)]
        public int DataOffset3 { get { return _off3; } }

        //public string ArticleStringID { get { return "ArticleType" + (Static ? "2_" : "1_") + (Name == "Entry Article" ? "Entry" : (Parent.Name == "Static Articles" ? "Static" + Index : _offsetID.ToString())); } }

        public int 
            _id,
            _articleBone,
            _charBone,
            _actionFlagsOffset,
            _subactionFlagsOffset, 
            _actionArrayOffset,
            _subactionsMainArrayOffset,
            _subactionGFXArrayOffset,
            _subactionSFXArrayOffset,
            _modelVisOffset,
            _collisionDataOffset,
            _off2,
            _off3;

        //[Browsable(false)]
        //public bool pikmin { get { return ArticleStringID == "ArticleType1_10" && RootNode.Name == "FitPikmin"; } }
        //[Browsable(false)]
        //public bool dedede { get { return ArticleStringID == "ArticleType1_14" && RootNode.Name == "FitDedede"; } }

        internal List<int>[] _scriptOffsets;

        protected override void OnParse(VoidPtr address)
        {
            sArticle* hdr = (sArticle*)address;
            _id = hdr->_id;
            _articleBone = hdr->_arcGroup;
            _charBone = hdr->_boneID;
            _actionFlagsOffset = hdr->_actionFlagsStart;
            _subactionFlagsOffset = hdr->_subactionFlagsStart;
            _actionArrayOffset = hdr->_actionsStart;
            _subactionsMainArrayOffset = hdr->_subactionMainStart;
            _subactionGFXArrayOffset = hdr->_subactionGFXStart;
            _subactionSFXArrayOffset = hdr->_subactionSFXStart;
            _modelVisOffset = hdr->_modelVisibility;
            _collisionDataOffset = hdr->_collisionData;
            _off2 = hdr->_unknownD2;
            _off3 = hdr->_unknownD3;

            _extraOffsets = new List<int>();
            _extraEntries = new List<MovesetEntryNode>();
            bint* extraAddr = (bint*)(address + 52);
            for (int i = 0; i < (_initSize - 52) / 4; i++)
                _extraOffsets.Add(extraAddr[i]);

            Static = _initSize > 52 && _extraOffsets[0] < 1480 && _extraOffsets[0] >= 0;

            _collisionData = Parse<CollisionData>(_collisionDataOffset);
            _mdlVis = Parse<ModelVisibility>(_modelVisOffset);

            _scriptOffsets = new List<int>[4];
            for (int i = 0; i < 4; i++)
                _scriptOffsets[i] = new List<int>();

            ParseScripts((bint*)hdr);
        }

        private void ParseScripts(bint* hdr)
        {
            Script s = null;
            int size, count;
            bint* actionOffset;
            _subActions = new BindingList<SubActionEntry>();
            _actions = new BindingList<ActionEntry>();

            if (hdr[3] > 0)
            {
                ActionEntry ag;
                sActionFlags* aflags = (sActionFlags*)Address(hdr[3]);
                if (hdr[5] > 0)
                {
                    actionOffset = (bint*)Address(hdr[5]);
                    size = _root.GetSize(hdr[5]);
                    for (int z = 0; z < size / 4; z++)
                    {
                        sActionFlags flag = aflags[z];
                        _actions.Add(ag = new ActionEntry(flag, z, z));
                        if (actionOffset[z] > 0)
                            ag._entry = Parse<Script>(actionOffset[z], this);
                        else
                            ag._entry = new Script(this);
                    }
                }
            }
            if (hdr[4] > 0)
            {
                SubActionEntry sg;
                sSubActionFlags* sflags = (sSubActionFlags*)Address(hdr[4]);

                if (hdr[6] > 1480)
                {
                    size = _root.GetSize(hdr[6]);
                    count = size / 4;
                    for (int z = 0; z < count; z++)
                    {
                        sSubActionFlags flag = sflags[z];
                        _subActions.Add(sg = new SubActionEntry(flag, flag._stringOffset > 0 ? new String((sbyte*)Address(flag._stringOffset)) : "<null>", z));
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        int x = hdr[6 + i];
                        if (x <= 0)
                            continue;

                        actionOffset = (bint*)Address(x);
                        for (int z = 0; z < count; z++)
                        {
                            if (actionOffset[z] > 0)
                                s = Parse<Script>(actionOffset[z], this);
                            else
                                s = new Script(this);
                            _subActions[z].SetWithType(i, s);
                        }
                    }
                }
            }
        }

        public bool Static = false;
        //public bool extraOffset = false;

        //public MoveDefActionFlagsNode _actionFlags;
        //public MoveDefActionListNode _actions;
        //public MoveDefFlagsNode _subActionFlags;
        public BindingList<SubActionEntry> _subActions;
        public BindingList<ActionEntry> _actions;
        public ModelVisibility _mdlVis;
        public CollisionData _collisionData;
        //public Data2ListNode _data2;
        public RawParamList _data3;

        [Category("Article"), Browsable(true)]
        public List<int> ExtraOffsets { get { return _extraOffsets; } }
        public List<int> _extraOffsets;
        [Category("Article"), Browsable(false)]
        public List<MovesetEntryNode> ExtraEntries { get { return _extraEntries; } }
        public List<MovesetEntryNode> _extraEntries;

        /*
        public override void Parse(VoidPtr address)
        {
            int off = 0;
            int actionCount = 0;
            int subactions = Static ? _extraOffsets[0] : (_offset - SubactionFlagsStart) / 8;
            if (ActionFlagsStart > 0)
                actionCount = _root.GetSize(ActionFlagsStart) / 16;
            if (SubactionFlagsStart > 0)
                subactions = _root.GetSize(SubactionFlagsStart) / 8;

            if (actionCount > 0)
            {
                (_actionFlags = new MoveDefActionFlagsNode("ActionFlags", actionCount)).Initialize(this, BaseAddress + ActionFlagsStart, actionCount * 16);
                if (ActionsStart > 0 || (dedede && _extraOffsets[0] > 0))
                {
                    _actions = new MoveDefActionListNode() { _name = "Actions" };
                    _actions.Parent = this;
                    for (int i = 0; i < actionCount; i++)
                    {
                        if (pikmin)
                        {
                            _actions.AddChild(new ActionGroup() { _name = "Action" + i }, false);

                            off = *((bint*)(BaseAddress + ActionsStart) + i);
                            if (off > 0)
                                new ActionScript("Entry", false, _actions.Children[i]).Initialize(_actions.Children[i], BaseAddress + off, 0);
                            else
                                _actions.Children[i].Children.Add(new ActionScript("Entry", true, _actions.Children[i]));
                            off = *((bint*)(BaseAddress + _extraOffsets[0]) + i);
                            if (off > 0)
                                new ActionScript("Exit", false, _actions.Children[i]).Initialize(_actions.Children[i], BaseAddress + off, 0);
                            else
                                _actions.Children[i].Children.Add(new ActionScript("Exit", true, _actions.Children[i]));
                        }
                        else
                        {
                            off = *((bint*)(BaseAddress + ActionsStart) + i);
                            if (off > 0)
                                new ActionScript("Action" + i, false, _actions).Initialize(_actions, BaseAddress + off, 0);
                            else
                                _actions.Children.Add(new ActionScript("Action" + i, true, _actions));
                        }
                    }
                }
            }

            if (SubactionFlagsStart > 0)
            {
                _subActionFlags = new MoveDefFlagsNode() { _parent = this };
                _subActionFlags.Initialize(this, BaseAddress + SubactionFlagsStart, subactions * 8);
                
                if (subactions == 0)
                    subactions = _subActionFlags._names.Count;

                int populateCount = 1;
                bool child = false;
                int bias = 0;

                if (dedede)
                {
                    _subActions = new MoveDefGroupNode() { Name = "SubActions" };
                    _subActions.AddChild(new MoveDefActionListNode() { _name = "Waddle Dee" }, false);
                    _subActions.AddChild(new MoveDefActionListNode() { _name = "Waddle Doo" }, false);
                    _subActions.AddChild(new MoveDefActionListNode() { _name = "Gyro" }, false);
                    populateCount = 3;
                    child = true;
                }
                else if (pikmin)
                {
                    _subActions = new MoveDefGroupNode() { Name = "SubActions" };
                    _subActions.AddChild(new MoveDefActionListNode() { _name = "Red" }, false);
                    _subActions.AddChild(new MoveDefActionListNode() { _name = "Yellow" }, false);
                    _subActions.AddChild(new MoveDefActionListNode() { _name = "Blue" }, false);
                    _subActions.AddChild(new MoveDefActionListNode() { _name = "Purple" }, false);
                    _subActions.AddChild(new MoveDefActionListNode() { _name = "White" }, false);
                    populateCount = 5;
                    child = true;
                    bias = 1;
                }
                else if (ArticleStringID == "ArticleType1_61" && RootNode.Name == "FitKirby")
                {
                    _subActions = new MoveDefGroupNode() { Name = "SubActions" };
                    _subActions.AddChild(new MoveDefActionListNode() { _name = "100 Ton Stone" }, false);
                    _subActions.AddChild(new MoveDefActionListNode() { _name = "Thwomp Stone" }, false);
                    _subActions.AddChild(new MoveDefActionListNode() { _name = "Spike Ball" }, false);
                    _subActions.AddChild(new MoveDefActionListNode() { _name = "Stone Kirby" }, false);
                    _subActions.AddChild(new MoveDefActionListNode() { _name = "Happy Stone" }, false);
                    populateCount = 5;
                    child = true;
                }
                else
                    _subActions = new MoveDefActionListNode() { _name = "SubActions" };

                _subActions.Parent = this;

                for (int x = 0; x < populateCount; x++)
                {
                    ResourceNode Base = _subActions;
                    int main = SubactionMainStart,
                        gfx = SubactionGFXStart,
                        sfx = SubactionSFXStart;
                    if (child)
                    {
                        Base = _subActions.Children[x];
                        main = _extraOffsets[x + bias];
                        gfx = _extraOffsets[x + populateCount + bias];
                        sfx = _extraOffsets[x + populateCount * 2 + bias];
                    }

                    for (int i = 0; i < subactions && i < _subActionFlags._names.Count; i++)
                        Base.AddChild(new SubActionGroup() { _name = _subActionFlags._names[i], _flags = _subActionFlags._flags[i]._Flags, _inTransTime = _subActionFlags._flags[i]._InTranslationTime }, false);

                    if (main > 0)
                        for (int i = 0; i < subactions && i < _subActionFlags._names.Count; i++)
                        {
                            off = *((bint*)(BaseAddress + main) + i);
                            if (off > 0)
                                new ActionScript("Main", false, Base.Children[i]).Initialize(Base.Children[i], BaseAddress + off, 0);
                            else
                                Base.Children[i].Children.Add(new ActionScript("Main", true, Base.Children[i]));
                        }
                    else
                        for (int i = 0; i < subactions && i < _subActionFlags._names.Count; i++)
                            Base.Children[i].Children.Add(new ActionScript("Main", true, Base.Children[i]));

                    if (gfx > 0)
                        for (int i = 0; i < subactions && i < _subActionFlags._names.Count; i++)
                        {
                            off = *((bint*)(BaseAddress + gfx) + i);
                            if (off > 0)
                                new ActionScript("GFX", false, Base.Children[i]).Initialize(Base.Children[i], BaseAddress + off, 0);
                            else
                                Base.Children[i].Children.Add(new ActionScript("GFX", true, Base.Children[i]));
                        }
                    else
                        for (int i = 0; i < subactions && i < _subActionFlags._names.Count; i++)
                            Base.Children[i].Children.Add(new ActionScript("GFX", true, Base.Children[i]));

                    if (sfx > 0)
                        for (int i = 0; i < subactions && i < _subActionFlags._names.Count; i++)
                        {
                            off = *((bint*)(BaseAddress + sfx) + i);
                            if (off > 0)
                                new ActionScript("SFX", false, Base.Children[i]).Initialize(Base.Children[i], BaseAddress + off, 0);
                            else
                                Base.Children[i].Children.Add(new ActionScript("SFX", true, Base.Children[i]));
                        }
                    else
                        for (int i = 0; i < subactions && i < _subActionFlags._names.Count; i++)
                            Base.Children[i].Children.Add(new ActionScript("SFX", true, Base.Children[i]));
                }
            }

            if (ModelVisibility != 0)
                (_mdlVis = new ModelVisibility()).Initialize(this, BaseAddress + ModelVisibility, 16);

            if (CollisionData != 0)
                (_data1 = new CollisionData() { _name = "Collision Data" }).Initialize(this, BaseAddress + CollisionData, 8);

            if (DataOffset2 != 0)
                (_data2 = new Data2ListNode() { _name = "Data2" }).Initialize(this, BaseAddress + DataOffset2, 8);

            if (DataOffset3 != 0)
                (_data3 = new RawParamList(3) { _name = "Data3" }).Initialize(this, BaseAddress + DataOffset3, 0);

            //Extra offsets.
            //Dedede:
            //Waddle Dee, Waddle Doo, and Gyro subactions main, gfx, sfx for first 9 offsets.
            //Pikmin:
            //Actions 2 is 1st offset.
            //Red, Yellow, Blue, Purple, and White subactions main, gfx, sfx for next 15 offsets.

            int index = 0;
            foreach (int i in _extraOffsets)
            {
                MovesetEntry entry = null;
                if (index < 9 && dedede) { }
                else if (index < 16 && pikmin) { }
                else if (ArticleStringID == "ArticleType1_61" && RootNode.Name == "FitKirby" && index < 15) { }
                else if (index == 0 && ArticleStringID == "ArticleType1_6" && RootNode.Name == "FitGameWatch")
                {
                    GameWatchArticle6 p = new GameWatchArticle6();
                    p.Initialize(this, BaseAddress + i, 0);
                    entry = p;
                }
                else if (index == 1 && (
                    (ArticleStringID == "ArticleType1_8" && (RootNode.Name == "FitLucas" || RootNode.Name == "FitNess")) || 
                    (ArticleStringID == "ArticleType1_11" && RootNode.Name == "FitGameWatch") || 
                    (ArticleStringID == "ArticleType1_4" && RootNode.Name == "FitWario") ||
                    (ArticleStringID == "ArticleType1_5" && RootNode.Name == "FitWarioMan")))
                {
                    MoveDefParamListNode p = new MoveDefParamListNode() { _name = "ParamList" + index };
                    p.Initialize(this, BaseAddress + i, 0);
                    entry = p;
                }
                else if (index > 0 && ArticleStringID == "ArticleType1_46" && RootNode.Name == "FitKirby")
                {
                    MovesetEntry p = null;
                    switch (index)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                            p = new MoveDefKirbyArticleP1Node() { _offsetID = index };
                            break;
                        case 6:
                        case 8:
                            //List of bytes - 1 or 0.
                            //7 & 9 are the index of the last 1 + 1
                            break;
                        case 9: break;
                    }
                    if (p != null)
                        p.Initialize(this, BaseAddress + i, 0);
                    entry = p;
                }
                else if (index > 0 && ((ArticleStringID == "ArticleType1_11" && RootNode.Name == "FitFox") || (ArticleStringID == "ArticleType1_9" && RootNode.Name == "FitFalco") || (ArticleStringID == "ArticleType1_11" && RootNode.Name == "FitWolf")))
                {
                    MovesetEntry p = null;
                    switch (index)
                    {
                        case 1:
                            p = new MoveDefItemAnchorListNode() { _name = "Bone Floats" };
                            break;
                        case 2:
                            p = new HitDataListOffsetNode() { _name = "HitDataList" + index };
                            break;
                        case 3:
                            p = new Fox11Falco9Wolf11Article3Node();
                            break;
                        case 4:
                            p = new ActionOffsetNode() { _name = "Data" + index };
                            break;
                        case 5:
                            p = new SecondaryActionOffsetNode() { _name = "Data" + index };
                            break;
                        case 6:
                            p = new Fox11Falco9Wolf11PopoArticle63Node() { _offsetID = index };
                            break;
                    }
                    if (p != null)
                        p.Initialize(this, BaseAddress + i, 0);
                    entry = p;
                }
                else if ((index == 23 || index == 24) && ArticleStringID == "ArticleType1_10" && RootNode.Name == "FitPikmin")
                {
                    MovesetEntry p = null;
                    switch (index)
                    {
                        case 23:
                            p = new ActionOffsetNode() { _name = "Data" + index };
                            break;
                        case 24:
                            p = new SecondaryActionOffsetNode() { _name = "Data" + index };
                            break;
                    }
                    if (p != null)
                        p.Initialize(this, BaseAddress + i, 0);
                    entry = p;
                }
                else if (index == 3 && ArticleStringID == "ArticleType1_14" && RootNode.Name == "FitPopo")
                {
                    Fox11Falco9Wolf11PopoArticle63Node p = new Fox11Falco9Wolf11PopoArticle63Node() { _offsetID = index };
                    p.Initialize(this, BaseAddress + i, 0);
                    entry = p;
                }
                else if (index > 4 && ArticleStringID == "ArticleType1_7" && RootNode.Name == "FitSonic")
                {
                    MovesetEntry p = null;
                    switch (index)
                    {
                        case 5:
                            p = new ActionOffsetNode() { _name = "Data" + index };
                            break;
                        case 6:
                            p = new SecondaryActionOffsetNode() { _name = "Data" + index };
                            break;
                    }
                    if (p != null)
                        p.Initialize(this, BaseAddress + i, 0);
                    entry = p;
                }
                else if (dedede && index == 11)
                {
                    DededeHitDataList p = new DededeHitDataList();
                    p.Initialize(this, BaseAddress + i, 0);
                    entry = p;
                }
                else if ((index == 3 && ArticleStringID == "ArticleType1_4" && RootNode.Name == "FitGanon") || (index == 1 && ArticleStringID == "ArticleType1_7" && RootNode.Name == "FitSonic"))
                {
                    MoveDefHitDataListNode p = new MoveDefHitDataListNode() { _name = "HitData" };
                    p.Initialize(this, BaseAddress + i, 0);
                    entry = p;
                }
                else
                {
                    if (i > 1480 && i < _root.dataSize)
                    {
                        ReferenceEntry e = _root.TryGetExternal(i);
                        if (e != null && e.Name.Contains("hitData"))
                        {
                            MoveDefHitDataListNode p = new MoveDefHitDataListNode() { _name = e.Name };
                            p.Initialize(this, new DataSource(BaseAddress + i, 0));
                            entry = p;
                        }
                        else
                            if (index < _extraOffsets.Count - 1 && _extraOffsets[index + 1] < 1480 && _extraOffsets[index + 1] > 1)
                            {
                                int count = _extraOffsets[index + 1];
                                int size = _root.GetSize(i);
                                if (size > 0 && count > 0)
                                {
                                    size /= count;
                                    RawData d = new RawData("ExtraParams" + index);
                                    entry = d;
                                    d.Initialize(this, BaseAddress + i, 0);
                                    for (int x = 0; x < count; x++)
                                        new RawParamList(x) { _name = "Part" + x }.Initialize(d, BaseAddress + i + x * size, size);
                                }
                            }
                            else
                            {
                                if (e != null && e.Name.Contains("hitData"))
                                {
                                    MoveDefHitDataListNode p = new MoveDefHitDataListNode() { _name = e.Name };
                                    entry = p;
                                    p.Initialize(this, new DataSource(BaseAddress + i, 0));
                                }
                                else
                                    (entry = new RawParamList(index) { _name = "ExtraParams" + index }).Initialize(this, BaseAddress + i, 0);
                            }
                    }
                }
                _extraEntries.Add(entry);
                index++;
            }
        }
        
        public FDefSubActionStringTable subActionStrings;
        public VoidPtr actionAddr;
        protected override int OnGetSize()
        {
            _buildHeader = true;
            _lookupCount = 0;
            subActionStrings = new FDefSubActionStringTable();
            _entryLength = 52 + _extraOffsets.Count * 4;

            int size = 0;

            if (_actionFlags != null)
            {
                _lookupCount++; //action flags offset
                size += 16 * _actionFlags.Children.Count;
            }

            if (_actions != null)
            {
                if (pikmin)
                {
                    //false for now
                    bool actions1Null = false, actions2Null = false;
                    foreach (ActionGroup grp in _actions.Children)
                        foreach (ActionScript a in grp.Children)
                            if (a.Children.Count > 0)
                                if (a.Index == 0)
                                    actions1Null = false;
                                else if (a.Index == 1)
                                    actions2Null = false;
                    _lookupCount += 2; //actions offsets
                    if (!actions1Null || !actions2Null)
                    {
                        foreach (ActionGroup grp in _actions.Children)
                            foreach (ActionScript a in grp.Children)
                                if (a.Children.Count > 0)
                                    _lookupCount++; //action offset
                        size += _actions.Children.Count * 8;
                    }
                }
                else
                {
                    bool actionsNull = true;
                    foreach (ActionScript a in _actions.Children)
                        if (a.Children.Count > 0) actionsNull = false;

                    if (!actionsNull)
                    {
                        _lookupCount++; //actions offsets
                        foreach (ActionScript a in _actions.Children)
                            if (a.Children.Count > 0)
                                _lookupCount++; //action offset
                        size += _actions.Children.Count * 4;
                    }
                }
            }

            if (_subActions != null)
            {
                if (_subActions.Children.Count > 0)
                    _lookupCount++; //subaction flags offset

                bool mainNull = true, gfxNull = true, sfxNull = true;
                MovesetEntry e = _subActions;
                int populateCount = 1;
                bool children = false;
                if (_subActions.Children[0] is MoveDefActionListNode)
                {
                    populateCount = _subActions.Children.Count;
                    children = true;
                }
                for (int i = 0; i < populateCount; i++)
                {
                    if (children)
                        e = _subActions.Children[i] as MovesetEntry;

                    foreach (SubActionGroup g in e.Children)
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
                        foreach (ActionScript a in g.Children)
                        {
                            //if ((Static && a.Children.Count > 0) || (!Static && write))
                            if (a.Children.Count > 0 || a._actionRefs.Count > 0 || a._build)
                            {
                                switch (a.Index)
                                {
                                    case 0: mainNull = false; break;
                                    case 1: gfxNull = false; break;
                                    case 2: sfxNull = false; break;
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
                        e = _subActions.Children[i] as MovesetEntry;

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

            if (_mdlVis != null)
            {
                _lookupCount++; //model vis offset
                size += _mdlVis.CalculateSize(true);
                _lookupCount += _mdlVis._lookupCount;
            }

            if (_data1 != null)
            {
                _lookupCount++; //data 1 offset
                if (!_data1.External)
                {
                    size += _data1.CalculateSize(true);
                    _lookupCount += _data1._lookupCount;
                }
            }

            if (_data2 != null)
            {
                _lookupCount++; //data 2 offset
                if (!_data2.External)
                {
                    size += _data2.CalculateSize(true);
                    _lookupCount += _data2._lookupCount;
                }
            }

            if (_data3 != null)
            {
                _lookupCount++; //data 3 offset
                if (!_data3.External)
                    size += _data3.CalculateSize(true);
            }

            foreach (MovesetEntry e in _extraEntries)
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
        protected override void OnWrite(VoidPtr address)
        {
            aFlags = sFlags = aStart = sMStart = sGStart = sSStart = visStart = off1 = off2 = off3 = 0;

            int a2Start = 0;

            List<int> mainStarts = null, gfxStarts = null, sfxStarts = null;

            VoidPtr addr = address;

            if (_subActions != null)
            {
                subActionStrings.WriteTable(addr);
                addr += subActionStrings.TotalSize;
            }

            if (_actionFlags != null)
            {
                FDefActionFlags* actionFlagsAddr = (FDefActionFlags*)addr;
                aFlags = (int)actionFlagsAddr - (int)RebuildBase;

                foreach (ActionFlags a in _actionFlags.Children)
                    a.Rebuild(actionFlagsAddr++, 16, true);

                addr = (VoidPtr)actionFlagsAddr;
            }

            if (_actions != null)
            {
                if (pikmin)
                {
                    //false for now
                    bool actions1Null = false, actions2Null = false;
                    foreach (ActionGroup grp in _actions.Children)
                        foreach (ActionScript a in grp.Children)
                            if (a.Children.Count > 0)
                                if (a.Index == 0)
                                    actions1Null = false;
                                else if (a.Index == 1)
                                    actions2Null = false;
                    if (!actions1Null || !actions2Null)
                    {
                        bint* action1Offsets = (bint*)addr;
                        aStart = (int)action1Offsets - (int)RebuildBase;
                        bint* action2Offsets = (bint*)(addr + _actions.Children.Count * 4);
                        a2Start = (int)action2Offsets - (int)RebuildBase;

                        foreach (ActionGroup grp in _actions.Children)
                            foreach (ActionScript a in grp.Children)
                            {
                                if (a.Index == 0)
                                {
                                    if (a.Children.Count > 0)
                                    {
                                        *action1Offsets = (int)a._rebuildAddr - (int)a.RebuildBase;
                                        _lookupOffsets.Add(action1Offsets);
                                    }
                                    action1Offsets++;
                                }
                                else if (a.Index == 1)
                                {
                                    if (a.Children.Count > 0)
                                    {
                                        *action2Offsets = (int)a._rebuildAddr - (int)a.RebuildBase;
                                        _lookupOffsets.Add(action2Offsets);
                                    }
                                    action2Offsets++;
                                }
                            }
                        addr = (VoidPtr)action2Offsets;
                        
                    }
                }
                else
                {
                    bool actionsNull = true;
                    foreach (ActionScript a in _actions.Children)
                        if (a.Children.Count > 0) actionsNull = false;

                    if (!actionsNull)
                    {
                        bint* actionOffsets = (bint*)addr;
                        aStart = (int)actionOffsets - (int)RebuildBase;

                        foreach (ActionScript a in _actions.Children)
                        {
                            if (a.Children.Count > 0)
                            {
                                *actionOffsets = (int)a._rebuildAddr - (int)a.RebuildBase;
                                _lookupOffsets.Add(actionOffsets);
                            }
                            actionOffsets++;
                        }
                        addr = (VoidPtr)actionOffsets;
                    }
                }
            }
            if (_mdlVis != null)
            {
                _mdlVis.Rebuild(addr, _mdlVis._calcSize, true);
                visStart = (int)_mdlVis.RebuildOffset;
                _lookupOffsets.AddRange(_mdlVis._lookupOffsets);
                addr += _mdlVis._calcSize;
            }

            if (_data1 != null)
            {
                if (!_data1.External)
                {
                    _data1.Rebuild(addr, _data1._calcSize, true);
                    _lookupOffsets.AddRange(_data1._lookupOffsets);
                    addr += _data1._calcSize;
                }
                off1 = (int)_data1._rebuildAddr - (int)RebuildBase;
            }

            if (_data2 != null)
            {
                if (!_data2.External)
                {
                    _data2.Rebuild(addr, _data2._calcSize, true);
                    _lookupOffsets.AddRange(_data2._lookupOffsets);
                    addr += _data2._calcSize;
                }
                off2 = (int)_data2._rebuildAddr - (int)RebuildBase;
            }

            if (_data3 != null)
            {
                if (_data3.External)
                    off3 = (int)_data3._rebuildAddr - (int)RebuildBase;
                else
                {
                    off3 = (int)addr - (int)RebuildBase;
                    _data3.Rebuild(addr, _data3._calcSize, true);
                    addr += _data3._calcSize;
                }
            }

            if (_subActions != null && _subActions.Children.Count > 0)
            {
                bint* lastOffsets = null, mainOffsets, GFXOffsets, SFXOffsets;
                FDefSubActionFlag* subActionFlagsAddr = null;
                bool mainNull = true, gfxNull = true, sfxNull = true;
                MovesetEntry e = _subActions;
                int populateCount = 1;
                bool children = false;
                if (_subActions.Children[0] is MoveDefActionListNode)
                {
                    populateCount = _subActions.Children.Count;
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
                        e = _subActions.Children[i] as MovesetEntry;

                    foreach (SubActionGroup g in e.Children)
                    {
                        foreach (ActionScript a in g.Children)
                            if (a.Children.Count > 0 || a._actionRefs.Count > 0 || a._build)
                                switch (a.Index)
                                {
                                    case 0: mainNull = false; break;
                                    case 1: gfxNull = false; break;
                                    case 2: sfxNull = false; break;
                                }
                    }

                    if (i == 0)
                    {
                        subActionFlagsAddr = (FDefSubActionFlag*)addr;
                        sFlags = (int)subActionFlagsAddr - (int)RebuildBase;
                        lastOffsets = (bint*)((VoidPtr)subActionFlagsAddr + e.Children.Count * 8);
                    }

                    mainOffsets = lastOffsets;

                    if (!(mainNull && Static))
                    {
                        if (!children)
                            sMStart = (int)mainOffsets - (int)RebuildBase;
                        else
                            mainStarts.Add((int)mainOffsets - (int)RebuildBase);
                        GFXOffsets = (bint*)((VoidPtr)mainOffsets + e.Children.Count * 4);
                    }
                    else GFXOffsets = mainOffsets;
                    if (!(gfxNull && Static))
                    {
                        if (!children)
                            sGStart = (int)GFXOffsets - (int)RebuildBase;
                        else
                            gfxStarts.Add((int)GFXOffsets - (int)RebuildBase);
                        SFXOffsets = (bint*)((VoidPtr)GFXOffsets + e.Children.Count * 4);
                    }
                    else SFXOffsets = GFXOffsets;

                    if (!(sfxNull && Static))
                    {
                        if (!children)
                            sSStart = (int)SFXOffsets - (int)RebuildBase;
                        else
                            sfxStarts.Add((int)SFXOffsets - (int)RebuildBase);
                        addr = ((VoidPtr)SFXOffsets + e.Children.Count * 4);
                    }
                    else addr = (VoidPtr)SFXOffsets;

                    lastOffsets = (bint*)addr;

                    int x = 0; //bool write = true;
                    foreach (SubActionGroup grp in e.Children)
                    {
                        if (i == 0)
                        {
                            *subActionFlagsAddr = new FDefSubActionFlag() { _Flags = grp._flags, _InTranslationTime = grp._inTransTime, _stringOffset = (int)subActionStrings[grp.Name] - (int)RebuildBase };

                            if (subActionFlagsAddr->_stringOffset > 0)
                                _lookupOffsets.Add(subActionFlagsAddr->_stringOffset.Address);

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
                        if (grp.Children[0].Children.Count > 0 || (grp.Children[0] as ActionScript)._actionRefs.Count > 0 || (grp.Children[0] as ActionScript)._build)
                        {
                            mainOffsets[x] = (int)(grp.Children[0] as ActionScript)._rebuildAddr - (int)RebuildBase;
                            _lookupOffsets.Add(&mainOffsets[x]);
                        }

                        //if ((Static && grp.Children[1].Children.Count > 0) || (!Static && write))
                        if (grp.Children[1].Children.Count > 0 || (grp.Children[1] as ActionScript)._actionRefs.Count > 0 || (grp.Children[1] as ActionScript)._build)
                        {
                            GFXOffsets[x] = (int)(grp.Children[1] as ActionScript)._rebuildAddr - (int)RebuildBase;
                            _lookupOffsets.Add(&GFXOffsets[x]);
                        }

                        //if ((Static && grp.Children[2].Children.Count > 0) || (!Static && write))
                        if (grp.Children[2].Children.Count > 0 || (grp.Children[2] as ActionScript)._actionRefs.Count > 0 || (grp.Children[2] as ActionScript)._build)
                        {
                            SFXOffsets[x] = (int)(grp.Children[2] as ActionScript)._rebuildAddr - (int)RebuildBase;
                            _lookupOffsets.Add(&SFXOffsets[x]);
                        }

                        x++;
                    }
                }
                addr = lastOffsets;
            }

            foreach (MovesetEntry e in _extraEntries)
            {
                if (e != null)
                {
                    if (!e.External)
                    {
                        e.Rebuild(addr, e._calcSize, true);
                        _lookupOffsets.AddRange(e._lookupOffsets);
                        if (e._lookupOffsets.Count != e._lookupCount)
                            Console.WriteLine(e._lookupCount - e._lookupOffsets.Count);
                        addr += e._calcSize;
                    }
                }
            }

            if (_buildHeader)
            {
                _rebuildAddr = addr;

                Article* article = (Article*)addr;

                article->_id = id;
                article->_boneID = charBone;
                article->_arcGroup = articleBone;

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

                bint* ext = (bint*)(addr + 52);
                int index = 0;
                if (_extraEntries.Count > 0)
                foreach (int i in _extraOffsets)
                {
                    MovesetEntry e = _extraEntries[index];
                    if (e != null)
                    {
                        ext[index] = (int)e._rebuildAddr - (int)RebuildBase;
                        _lookupOffsets.Add(&ext[index]);
                    }
                    else if (index == 0 && Static)
                        ext[index] = (_subActions == null ? 0 : _subActions.Children.Count);
                    else
                        ext[index] = i;
                    index++;
                }

                index = 0;

                if (pikmin)
                {
                    ext[0] = a2Start;
                    _lookupOffsets.Add(&ext[0]);
                }

                int bias = (pikmin ? 1 : 0);
                if (mainStarts != null)
                    foreach (int i in mainStarts)
                    {
                        ext[index + bias] = i;
                        _lookupOffsets.Add(&ext[index + bias]);
                        index++;
                    }
                if (gfxStarts != null)
                    foreach (int i in gfxStarts)
                    {
                        ext[index + bias] = i;
                        _lookupOffsets.Add(&ext[index + bias]);
                        index++;
                    }
                if (sfxStarts != null)
                    foreach (int i in sfxStarts)
                    {
                        ext[index + bias] = i;
                        _lookupOffsets.Add(&ext[index + bias]);
                        index++;
                    }

                if ((int)(addr + 52 + _extraOffsets.Count * 4) - (int)address != _calcSize)
                    Console.WriteLine(_calcSize - ((int)(addr + 52 + _extraOffsets.Count * 4) - (int)address));

                //Add all header offsets
                bint* off = (bint*)((VoidPtr)article + 12);
                for (int i = 0; i < 10; i++)
                    if (off[i] > 1480 && off[i] < _root.dataSize)
                        _lookupOffsets.Add(&off[i]);
                
                if (_lookupOffsets.Count != _lookupCount)
                    Console.WriteLine(_lookupCount - _lookupOffsets.Count);
            }
            else
            {
                if ((int)addr - (int)address != _childLength)
                    Console.WriteLine((int)addr - (int)address);
            }
        }
        */
        public ArticleInfo _info;

        public override string ToString()
        {
            return String.Format("[{0}] {1}", Index, _info == null || _info._model == null ? "Article" : _info._model.Name);
        }
    }
}
