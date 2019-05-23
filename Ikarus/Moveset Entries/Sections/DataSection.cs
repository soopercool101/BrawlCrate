using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using BrawlLib.SSBBTypes;
using Ikarus.MovesetBuilder;

namespace Ikarus.MovesetFile
{
    public unsafe class DataSection : TableEntryNode
    {
        internal static TableEntryNode TestType(string name)
        {
            return name == "data" ? new DataSection() : null;
        }
        
        DataHeader _hdr;
        int _unk27, _unk28, _flags2;
        uint _flags1;

        [Category("Data Offsets")]
        public int SubactionFlags { get { return _hdr.SubactionFlagsStart; } }
        [Category("Data Offsets")]
        public int ModelVisibility { get { return _hdr.ModelVisibilityStart; } }
        [Category("Data Offsets")]
        public int Attributes { get { return _hdr.AttributeStart; } }
        [Category("Data Offsets")]
        public int SSEAttributes { get { return _hdr.SSEAttributeStart; } }
        [Category("Data Offsets")]
        public int MiscSection { get { return _hdr.MiscSectionOffset; } }
        [Category("Data Offsets")]
        public int CommonActionFlags { get { return _hdr.CommonActionFlagsStart; } }
        [Category("Data Offsets")]
        public int ActionFlags { get { return _hdr.ActionFlagsStart; } }
        [Category("Data Offsets")]
        public int Unknown7 { get { return _hdr.Unknown7; } }
        [Category("Data Offsets")]
        public int ActionInterrupts { get { return _hdr.ActionInterrupts; } }
        [Category("Data Offsets")]
        public int EntryActions { get { return _hdr.EntryActionsStart; } }
        [Category("Data Offsets")]
        public int ExitActions { get { return _hdr.ExitActionsStart; } }
        [Category("Data Offsets")]
        public int ActionPre { get { return _hdr.ActionPreStart; } }
        [Category("Data Offsets")]
        public int SubactionMain { get { return _hdr.SubactionMainStart; } }
        [Category("Data Offsets")]
        public int SubactionGFX { get { return _hdr.SubactionGFXStart; } }
        [Category("Data Offsets")]
        public int SubactionSFX { get { return _hdr.SubactionSFXStart; } }
        [Category("Data Offsets")]
        public int SubactionOther { get { return _hdr.SubactionOtherStart; } }
        [Category("Data Offsets")]
        public int AnchoredItemPositions { get { return _hdr.AnchoredItemPositions; } }
        [Category("Data Offsets")]
        public int GooeyBombPositions { get { return _hdr.GooeyBombPositions; } }
        [Category("Data Offsets")]
        public int BoneRef1 { get { return _hdr.BoneRef1; } }
        [Category("Data Offsets")]
        public int BoneRef2 { get { return _hdr.BoneRef2; } }
        [Category("Data Offsets")]
        public int EntryActionOverrides { get { return _hdr.EntryActionOverrides; } }
        [Category("Data Offsets")]
        public int ExitActionOverrides { get { return _hdr.ExitActionOverrides; } }
        [Category("Data Offsets")]
        public int Unknown22 { get { return _hdr.Unknown22; } }
        [Category("Data Offsets")]
        public int SamusArmCannonPositions { get { return _hdr.SamusArmCannonPositions; } }
        [Category("Data Offsets")]
        public int Unknown24 { get { return _hdr.Unknown24; } }
        [Category("Data Offsets")]
        public int StaticArticles { get { return _hdr.StaticArticlesStart; } }
        [Category("Data Offsets")]
        public int EntryArticle { get { return _hdr.EntryArticleStart; } }
        
        [Category("Data Flags")]
        public int Unknown27 { get { return _unk27; } set { _unk27 = value; SignalPropertyChange(); } }
        [Category("Data Flags")]
        public int Unknown28 { get { return _unk28; } set { _unk28 = value; SignalPropertyChange(); } }
        [Category("Data Flags"), TypeConverter(typeof(Bin32StringConverter))]
        public Bin32 Flags1bin { get { return new Bin32(_flags1); } set { _flags1 = value._data; SignalPropertyChange(); } }
        [Category("Data Flags")]
        public uint Flags1uint { get { return _flags1; } set { _flags1 = value; SignalPropertyChange(); } }
        [Category("Data Flags"), TypeConverter(typeof(Bin32StringConverter))]
        public Bin32 Flags2bin { get { return new Bin32((uint)_flags2); } set { _flags2 = (int)value._data; SignalPropertyChange(); } }
        [Category("Data Flags")]
        public int Flags2int { get { return _flags2; } set { _flags2 = value; SignalPropertyChange(); } }
        
        //Individual entries
        public AttributeList _attributes, _sseAttributes;
        public MiscSectionNode _misc;
        public ModelVisibility _modelVis;
        public ArticleNode _entryArticle;
        public ActionInterrupts _actionInterrupts;
        public Unknown22 _unknown22;
        public Unknown24 _unknown24;
        public BoneReferences2 _boneRef2;

        //EntryLists
        public EntryList<ActionPre> _actionPre;
        public EntryList<BoneIndexValue> _boneRef1;
        public EntryList<Unknown7Entry> _unknown7;
        public EntryList<ActionFlags> _commonActionFlags;
        public EntryList<ItemAnchor> _anchoredItems, _gooeyBomb, _samusCannon;

        //Other lists
        public ActionOverrideList _exitOverrides;
        public ActionOverrideList _entryOverrides;

        public BindingList<SubActionEntry> _subActions;
        public List<ArticleNode> _staticArticles;

        //Extra data offset lists
        public List<ArticleNode> _articles;
        public List<RawParamList> _paramLists;
        public List<CollisionData> _hitData;
        public List<sAddAreaDataSet> _addAreaDataSets;

        //public MoveDefStaticArticleGroupNode _staticArticles;
        ////Character Specific Nodes
        ////Popo
        //public MoveDefActionListNode nanaSubActions;
        //public MoveDefSoundDatasNode nanaSoundData;
        ////ZSS
        //public SZerosuitExtraParams8Node zssParams8;
        //public int zssFirstOffset = -1;
        ////Wario
        //public Wario8 warioParams8;
        //public Wario6 warioParams6;
        //public int warioSwing4StringOffset = -1;

        public BindingList<SubActionEntry> SubActions { get { return _subActions; } }

        protected override void OnParse(VoidPtr address)
        {
            //Initialize lists
            _paramLists = new List<RawParamList>();
            _articles = new List<ArticleNode>();
            _subActions = new BindingList<SubActionEntry>();

            //Get header values
            _hdr = *(DataHeader*)address; 
            _unk27 = _hdr.Unknown27;
            _unk28 = _hdr.Unknown28;
            _flags1 = _hdr.Flags1;
            _flags2 = _hdr.Flags2;

            bint* v = (bint*)address;

            //Calculate the size of each section using the offsets in order of data appearance
            int[] sizes = SakuraiArchiveNode.CalculateSizes(_root._dataSize, v, 27, true);
            
            //Parse any data related to scripts
            ParseScripts(v, sizes);

            //Now parse all other data entries for this character.
            //If an offset is 0 (except for the attributes), the entry will be set to null.

            //subaction flags (offset 0) are parsed in ParseScripts
            _modelVis = Parse<ModelVisibility>(v[1]);
            _attributes = Parse<AttributeList>(v[2], "Attributes");
            _sseAttributes = Parse<AttributeList>(v[3]);
            _misc = Parse<MiscSectionNode>(v[4]);
            _commonActionFlags = Parse<EntryList<ActionFlags>>(v[5], 0x10);
            //action flags (offset 6) are parsed in ParseScripts
            _unknown7 = Parse<EntryList<Unknown7Entry>>(v[7], 8);
            _actionInterrupts = Parse<ActionInterrupts>(v[8]);
            //entry actions (offset 9) are parsed in ParseScripts
            //exit actions (offset 10) are parsed in ParseScripts
            _actionPre = Parse<EntryList<ActionPre>>(v[11], 4);
            //main subactions (offset 12) are parsed in ParseScripts
            //gfx subactions (offset 13) are parsed in ParseScripts
            //sfx subactions (offset 14) are parsed in ParseScripts
            //other subactions (offset 15) are parsed in ParseScripts
            _anchoredItems = Parse<EntryList<ItemAnchor>>(v[16], sItemAnchor.Size);
            _gooeyBomb = Parse<EntryList<ItemAnchor>>(v[17], sItemAnchor.Size);
            _boneRef1 = Parse<EntryList<BoneIndexValue>>(v[18], 4, sizes[18] / 4);
            _boneRef2 = Parse<BoneReferences2>(v[19]);
            _entryOverrides = Parse<ActionOverrideList>(v[20]);
            _exitOverrides = Parse<ActionOverrideList>(v[21]);
            _unknown22 = Parse<Unknown22>(v[22]);
            _samusCannon = Parse<EntryList<ItemAnchor>>(v[23], sItemAnchor.Size + 4);
            _unknown24 = Parse<Unknown24>(v[24]);

            //Parse extra offsets specific to this character
            //It appears that the module controls this data
            ExtraDataOffsets.ParseCharacter(((MovesetNode)_root).Character, this, address + DataHeader.Size);

            //Sort and set article indices (they are parsed as extra offsets)
            int u = 0;
            _articles = _articles.OrderBy(x => x._offset).ToList();
            foreach (ArticleNode e in _articles)
                e._index = u++;
        }

        private void ParseScripts(bint* hdr, int[] sizes)
        {
            Script s = null;
            int size, count;
            bint* actionOffset;
            List<List<int>> list;

            MovesetNode node = _root as MovesetNode;

            sSubActionFlags* sflags = (sSubActionFlags*)Address(hdr[0]);
            sActionFlags* aflags = (sActionFlags*)Address(hdr[6]);

            //Collect offsets first
            size = sizes[9];
            for (int i = 9; i < 11; i++)
            {
                if (hdr[i] < 0) continue;
                actionOffset = (bint*)Address(hdr[i]);
                for (int x = 0; x < size / 4; x++)
                    node._scriptOffsets[0][i - 9].Add(actionOffset[x]);
            }
            size = sizes[12];
            for (int i = 12; i < 16; i++)
            {
                if (hdr[i] < 0) continue;
                actionOffset = (bint*)Address(hdr[i]);
                for (int x = 0; x < size / 4; x++)
                    node._scriptOffsets[1][i - 12].Add(actionOffset[x]);
            }

            //Now parse scripts using collected offsets
            //The offsets are stored in the root in order to find scripts later

            //Actions first
            ActionEntry ag;
            list = node._scriptOffsets[0];
            count = list[0].Count;
            node._actions = new BindingList<ActionEntry>();
            for (int i = 0; i < count; i++)
            {
                sActionFlags flag = aflags[i];
                node.Actions.Add(ag = new ActionEntry(flag, i, i + 274));
                for (int x = 0; x < 2; x++)
                {
                    if (i < list[x].Count && list[x][i] > 0)
                        s = Parse<Script>(list[x][i]);
                    else
                        s = new Script();
                    ag.SetWithType(x, s);
                }
            }

            //Now subactions
            SubActionEntry sg;
            list = node._scriptOffsets[1];
            count = list[0].Count;
            _subActions = new BindingList<SubActionEntry>();
            for (int i = 0; i < count; i++)
            {
                sSubActionFlags flag = sflags[i];
                _subActions.Add(sg = new SubActionEntry(flag, flag._stringOffset > 0 ? new String((sbyte*)Address(flag._stringOffset)) : "<null>", i));
                for (int x = 0; x < 4; x++)
                {
                    if (i < list[x].Count && list[x][i] > 0)
                        s = Parse<Script>(list[x][i]);
                    else
                        s = new Script();
                    sg.SetWithType(x, s);
                }
            }
        }

        //public override void Parse(VoidPtr address)
        //{

            //List<int> temp;
            //bint* actionOffset;

            ////Parse offsets first
            //for (int i = 9; i < 11; i++)
            //{
            //    actionOffset = (bint*)(BaseAddress + specialOffsets[i].Offset);
            //    temp = new List<int>();
            //    for (int x = 0; x < specialOffsets[i].Size / 4; x++)
            //        temp.Add(actionOffset[x]);
            //    actions.ActionOffsets.Add(temp);
            //}
            //for (int i = 12; i < 16; i++)
            //{
            //    actionOffset = (bint*)(BaseAddress + specialOffsets[i].Offset);
            //    temp = new List<int>();
            //    for (int x = 0; x < specialOffsets[i].Size / 4; x++)
            //        temp.Add(actionOffset[x]);
            //    subActions.ActionOffsets.Add(temp);
            //}

            //if (specialOffsets[5].Size != 0)
            //    (_commonActionFlags = new MoveDefActionFlagsNode("Common Action Flags", commonActionFlagsCount = ((Unknown7 - CommonActionFlagsStart) / 16)) { _offsetID = 5 }).Initialize(this, new DataSource(BaseAddress + specialOffsets[5].Offset, commonActionFlagsCount * 16));
            //if (specialOffsets[6].Size != 0)
            //    (_actionFlags = new MoveDefActionFlagsNode("Action Flags", actionFlagsCount = ((EntryActionsStart - ActionFlagsStart) / 16)) { _offsetID = 6 }).Initialize(this, new DataSource(BaseAddress + specialOffsets[6].Offset, actionFlagsCount * 16));
            //if (specialOffsets[9].Size != 0 || specialOffsets[10].Size != 0)
            //{
            //    int count;
            //    if (specialOffsets[9].Size == 0)
            //        count = specialOffsets[10].Size / 4;
            //    else
            //        count = specialOffsets[9].Size / 4;

            //    if (_root.GetSize(specialOffsets[10].Offset) != _root.GetSize(specialOffsets[9].Offset))
            //        Console.WriteLine(_root.GetSize(specialOffsets[10].Offset) + " " + _root.GetSize(specialOffsets[9].Offset));

            //    //Initialize using first offset so the node is sorted correctly
            //    actions.Initialize(this, BaseAddress + specialOffsets[9].Offset, 0);

            //    //Set up groups
            //    for (int i = 0; i < count; i++)
            //        actions.AddChild(new ActionGroup() { _name = "Action" + (i + 274), _offsetID = i }, false);

            //    //Add children
            //    for (int i = 0; i < 2; i++)
            //        if (specialOffsets[i + 9].Size != 0)
            //            PopulateActionGroup(actions, actions.ActionOffsets[i], false, i);

            //    //Add to children (because the parent was set before initialization)
            //    Children.Add(actions);

            //    //actions.Children.Sort(MoveDefEntryNode.ActionCompare);

            //    _root._actions = actions;
            //}
            //if (specialOffsets[0].Size != 0)
            //    (_animFlags = new MoveDefFlagsNode() { _offsetID = 0, _parent = this }).Initialize(this, BaseAddress + specialOffsets[0].Offset, specialOffsets[0].Size);
            //if (specialOffsets[12].Size != 0 || specialOffsets[13].Size != 0 || specialOffsets[14].Size != 0 || specialOffsets[15].Size != 0)
            //{
            //    string name;
            //    int count = 0;
            //    for (int i = 0; i < 4; i++)
            //        if (specialOffsets[i + 12].Size != 0)
            //        {
            //            count = specialOffsets[i + 12].Size / 4;
            //            break;
            //        }

            //    //Initialize using first offset so the node is sorted correctly
            //    subActions.Initialize(this, BaseAddress + specialOffsets[12].Offset, 0);

            //    //Set up groups
            //    for (int i = 0; i < count; i++)
            //    {
            //        if (_animFlags._names.Count > i && _animFlags._flags[i]._stringOffset > 0)
            //            name = _animFlags._names[i];
            //        else
            //            name = "<null>";
            //        subActions.AddChild(new SubActionGroup() { _name = name, _flags = _animFlags._flags[i]._Flags, _inTransTime = _animFlags._flags[i]._InTranslationTime }, false);
            //    }

            //    //Add children
            //    for (int i = 0; i < 4; i++)
            //        if (specialOffsets[i + 12].Size != 0)
            //            PopulateActionGroup(subActions, subActions.ActionOffsets[i], true, i);

            //    //Add to children (because the parent was set before initialization)
            //    Children.Add(subActions);

            //    _root._subActions = subActions;
            //}
            //if (specialOffsets[25].Size != 0)
            //    (_staticArticles = new MoveDefStaticArticleGroupNode() { _name = "Static Articles", _offsetID = 25 }).Initialize(this, new DataSource(BaseAddress + specialOffsets[25].Offset, 8));
            //if (specialOffsets[26].Size != 0)
            //    (_entryArticle = new ArticleEntry() { Static = true, _name = "Entry Article", _offsetID = 26 }).Initialize(this, new DataSource(BaseAddress + specialOffsets[26].Offset, 0));

            #region old
            ////These offsets follow no patterns
            //int y = 0;
            //MoveDefExternalNode ext = null;
            //foreach (int DataOffset in _extraOffsets)
            //{
            //    if (y == 2 && character == CharFolder.poketrainer)
            //    {
            //        MoveDefSoundDatasNode p = new MoveDefSoundDatasNode() { isExtra = true, seperate = true, _name = "Sound Data 2" };
            //        p.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
            //        _extraEntries.Add(p);
            //    }
            //    else if (y == 49 && character == CharFolder.kirby)
            //    {
            //        MoveDefKirbyParamList49Node p = new MoveDefKirbyParamList49Node() { isExtra = true, offsetID = y };
            //        p.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
            //        _extraEntries.Add(p);
            //    }
            //    else if (y == 50 && character == CharFolder.kirby)
            //    {
            //        //6 offsets
            //        //that point to:
            //        //offset
            //        //count
            //        //align to 0x10
            //        //that points to list of:
            //        //offset
            //        //align list to 0x10
            //        //that points to:
            //        //offset
            //        //count
            //        //offset (sometimes 0)
            //        //count (sometimes 0)
            //        //that points to list of:
            //        //offset
            //        //count
            //        //align list to 0x10
            //        //that points to:
            //        //int value
            //    }
            //    else if ((y == 51 || y == 52) && character == CharFolder.kirby)
            //    {
            //        MoveDefKirbyParamList5152Node p = new MoveDefKirbyParamList5152Node() { isExtra = true, offsetID = y };
            //        p.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
            //        _extraEntries.Add(p);
            //    }
            //    else if ((y == 7 && character == CharFolder.pit) || (y == 13 && character == CharFolder.robot))
            //    {
            //        Pit7Robot13Node p = new Pit7Robot13Node() { isExtra = true, offsetID = y };
            //        p.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
            //        _extraEntries.Add(p);
            //    }
            //    else if (y == 8 && character == CharFolder.lucario)
            //    {
            //        HitDataListOffsetNode p = new HitDataListOffsetNode() { isExtra = true, _name = "HitDataList" + y, offsetID = y };
            //        p.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
            //        _extraEntries.Add(p);
            //    }
            //    else if (y > 9 && character == CharFolder.yoshi)
            //    {
            //        Yoshi9 p = new Yoshi9() { isExtra = true, offsetID = y };
            //        p.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
            //        _extraEntries.Add(p);
            //    }
            //    else if (y == 15 && character == CharFolder.dedede)
            //    {
            //        Data2ListNode p = new Data2ListNode() { isExtra = true, offsetID = y };
            //        p.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
            //        _extraEntries.Add(p);
            //    }
            //    else if (
            //        (y == 56 && character == CharFolder.kirby) ||
            //        (y == 7 && character == CharFolder.link) ||
            //        (y == 8 && character == CharFolder.peach) ||
            //        (y == 4 && character == CharFolder.pit) ||
            //        (y == 7 && character == CharFolder.toonlink))
            //    {
            //        MoveDefHitDataListNode p = new MoveDefHitDataListNode() { isExtra = true, _name = "HitDataList" + y, offsetID = y };
            //        p.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
            //        _extraEntries.Add(p);
            //    }
            //    else if (y == 6 && character == CharFolder.wario)
            //    {
            //        warioParams6 = new Wario6() { isExtra = true, offsetID = y };
            //        warioParams6.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
            //        _extraEntries.Add(warioParams6);
            //    }
            //    else if (y == 8 && character == CharFolder.wario)
            //    {
            //        warioParams8 = new Wario8() { isExtra = true, offsetID = y };
            //        warioParams8.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
            //        _extraEntries.Add(warioParams8);
            //    }
            //    else if (y == 8 && character == CharFolder.szerosuit)
            //    {
            //        (zssParams8 = new SZerosuitExtraParams8Node() { isExtra = true, offsetID = y }).Initialize(this, BaseAddress + DataOffset, 32);
            //        _extraEntries.Add(zssParams8);
            //    }
            //    else if (y < 4 && character == CharFolder.popo)
            //    {
            //        _extraEntries.Add(null);

            //        if (y == 0)
            //            nanaSubActions = new MoveDefActionListNode() { _name = "Nana SubAction Scripts", isExtra = true };

            //        actionOffset = (bint*)(BaseAddress + DataOffset);
            //        ActionOffsets = new List<int>();
            //        for (int x = 0; x < Root.GetSize(DataOffset) / 4; x++)
            //            ActionOffsets.Add(actionOffset[x]);
            //        nanaSubActions.ActionOffsets.Add(ActionOffsets);

            //        if (y == 3)
            //        {
            //            string name;
            //            int count = 0;
            //            for (int i = 0; i < 4; i++)
            //                if ((count = Root.GetSize(DataOffset) / 4) > 0)
            //                    break;

            //            //Initialize using first offset so the node is sorted correctly
            //            nanaSubActions.Initialize(this, BaseAddress + _extraOffsets[0], 0);

            //            //Set up groups
            //            for (int i = 0; i < count; i++)
            //            {
            //                if (_animFlags._names.Count > i && _animFlags._flags[i]._stringOffset > 0)
            //                    name = _animFlags._names[i];
            //                else
            //                    name = "<null>";
            //                nanaSubActions.AddChild(new MoveDefSubActionGroupNode() { _name = name, _flags = _animFlags._flags[i]._Flags, _inTransTime = _animFlags._flags[i]._InTranslationTime }, false);
            //            }

            //            //Add children
            //            for (int i = 0; i < 4; i++)
            //                PopulateActionGroup(nanaSubActions, nanaSubActions.ActionOffsets[i], true, i);
            //        }
            //    }
            //    else if (y == 10 && character == CharFolder.popo)
            //    {
            //        (nanaSoundData = new MoveDefSoundDatasNode() { _name = "Nana Sound Data", isExtra = true }).Initialize(this, (VoidPtr)Header + 124 + y * 4, 8);
            //        _extraEntries.Add(null);
            //    }
            //    else
            //    {
            //        if (DataOffset > Root.dataSize) //probably flags or float
            //            continue;

            //        ext = null;
            //        if (DataOffset > 1480) //I don't think a count would be greater than this
            //        {
            //            MoveDefEntryNode entry = null;
            //            if ((ext = Root.IsExternal(DataOffset)) != null)
            //            {
            //                if (ext.Name.StartsWith("param"))
            //                {
            //                    int o = 0;
            //                    if (y < _extraOffsets.Count - 1 && (o = _extraOffsets[y + 1]) < 1480 && o > 1)
            //                    {
            //                        MoveDefRawDataNode d = new MoveDefRawDataNode("ExtraParams" + y) { offsetID = y, isExtra = true };
            //                        d.Initialize(this, BaseAddress + DataOffset, 0);
            //                        for (int i = 0; i < o; i++)
            //                            new MoveDefSectionParamNode() { _name = "Part" + i, _extOverride = i == 0 }.Initialize(d, BaseAddress + DataOffset + ((d.Size / o) * i), (d.Size / o));
            //                        entry = d;
            //                    }
            //                    else
            //                    {
            //                        MoveDefSectionParamNode p = new MoveDefSectionParamNode() { _name = "ExtraParams" + y, isExtra = true, offsetID = y };
            //                        p.Initialize(this, BaseAddress + DataOffset, 0);
            //                        entry = p;
            //                    }
            //                }
            //                else
            //                {
            //                    Article* test = (Article*)(BaseAddress + DataOffset);
            //                    if (Root.GetSize(DataOffset) < 52 ||
            //                        test->_actionsStart > Root.dataSize || test->_actionsStart % 4 != 0 ||
            //                        test->_subactionFlagsStart > Root.dataSize || test->_subactionFlagsStart % 4 != 0 ||
            //                        test->_subactionGFXStart > Root.dataSize || test->_subactionGFXStart % 4 != 0 ||
            //                        test->_subactionSFXStart > Root.dataSize || test->_subactionSFXStart % 4 != 0 ||
            //                        test->_modelVisibility > Root.dataSize || test->_modelVisibility % 4 != 0 ||
            //                        test->_arcGroup > byte.MaxValue || test->_boneID > short.MaxValue || test->_id > 1480)
            //                    {
            //                        int o = 0;
            //                        if (y < _extraOffsets.Count - 1 && (o = _extraOffsets[y + 1]) < 1480 && o > 1)
            //                        {
            //                            MoveDefRawDataNode d = new MoveDefRawDataNode("ExtraParams" + y) { offsetID = y, isExtra = true };
            //                            d.Initialize(this, BaseAddress + DataOffset, 0);
            //                            for (int i = 0; i < o; i++)
            //                                new MoveDefSectionParamNode() { _name = "Part" + i, _extOverride = i == 0 }.Initialize(d, BaseAddress + DataOffset + ((d.Size / o) * i), (d.Size / o));
            //                            entry = d;
            //                        }
            //                        else
            //                        {
            //                            MoveDefSectionParamNode p = new MoveDefSectionParamNode() { _name = "ExtraParams" + y, isExtra = true, offsetID = y };
            //                            p.Initialize(this, BaseAddress + DataOffset, 0);
            //                            entry = p;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (_articleGroup == null)
            //                        {
            //                            _articleGroup = new MoveDefGroupNode() { _name = "Articles" };
            //                            _articleGroup.Initialize(this, BaseAddress + DataOffset, 0);
            //                        }

            //                        (entry = new MoveDefArticleNode() { offsetID = y, Static = true, isExtra = true, extraOffset = true }).Initialize(_articleGroup, BaseAddress + DataOffset, 0);
            //                        _articles.Add(entry._offset, entry);
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                Article* test = (Article*)(BaseAddress + DataOffset);
            //                if (Root.GetSize(DataOffset) < 52 ||
            //                    test->_actionsStart > Root.dataSize || test->_actionsStart % 4 != 0 ||
            //                    test->_subactionFlagsStart > Root.dataSize || test->_subactionFlagsStart % 4 != 0 ||
            //                    test->_subactionGFXStart > Root.dataSize || test->_subactionGFXStart % 4 != 0 ||
            //                    test->_subactionSFXStart > Root.dataSize || test->_subactionSFXStart % 4 != 0 ||
            //                    test->_modelVisibility > Root.dataSize || test->_modelVisibility % 4 != 0 ||
            //                    test->_arcGroup > byte.MaxValue || test->_boneID > short.MaxValue || test->_id > 1480)
            //                {
            //                    int o = 0;
            //                    if (y < _extraOffsets.Count - 1 && (o = _extraOffsets[y + 1]) < 1480 && o > 1)
            //                    {
            //                        MoveDefRawDataNode d = new MoveDefRawDataNode("ExtraParams" + y) { offsetID = y, isExtra = true };
            //                        d.Initialize(this, BaseAddress + DataOffset, 0);
            //                        for (int i = 0; i < o; i++)
            //                            new MoveDefSectionParamNode() { _name = "Part" + i, _extOverride = i == 0 }.Initialize(d, BaseAddress + DataOffset + ((d.Size / o) * i), (d.Size / o));
            //                        entry = d;
            //                    }
            //                    else
            //                    {
            //                        MoveDefSectionParamNode p = new MoveDefSectionParamNode() { _name = "ExtraParams" + y, isExtra = true, offsetID = y };
            //                        p.Initialize(this, BaseAddress + DataOffset, 0);
            //                        entry = p;
            //                    }
            //                }
            //                else
            //                {
            //                    if (_articleGroup == null)
            //                    {
            //                        _articleGroup = new MoveDefGroupNode() { _name = "Articles" };
            //                        _articleGroup.Initialize(this, BaseAddress + DataOffset, 0);
            //                    }

            //                    (entry = new MoveDefArticleNode() { offsetID = y, isExtra = true, Static = true, extraOffset = true }).Initialize(_articleGroup, BaseAddress + DataOffset, 0);
            //                    _articles.Add(entry._offset, entry);
            //                }
            //            }
            //            _extraEntries.Add(entry);
            //        }
            //        else { } //Probably a count
            //    }
            //    y++;
            //}

            //if (_extraEntries.Count > 0)
            //{
            //    if (!Directory.Exists(Application.StartupPath + "/MovesetData/CharSpecific"))
            //        Directory.CreateDirectory(Application.StartupPath + "/MovesetData/CharSpecific");
            //    string events = Application.StartupPath + "/MovesetData/CharSpecific/" + Root.Parent.Name + ".txt";
            //    using (StreamWriter file = new StreamWriter(events))
            //    {
            //        foreach (MoveDefEntryNode e in _extraEntries)
            //        {
            //            if (e is MoveDefSectionParamNode)
            //            {
            //                MoveDefSectionParamNode p = e as MoveDefSectionParamNode;
            //                file.WriteLine(p.Name);
            //                file.WriteLine(p.Name); //Replaced name
            //                foreach (AttributeInfo i in p._info)
            //                {
            //                    file.WriteLine(i._name);
            //                    file.WriteLine(i._description);
            //                    file.WriteLine(i._type == false ? 0 : 1);
            //                    file.WriteLine();
            //                }
            //                file.WriteLine();
            //            }
            //        }
            //    }
            //}
#endregion
        //}

        protected override int OnGetLookupCount() { return _builder._lookupCount; }

        DataBuilder _builder;
        protected override int OnGetSize()
        {
            _builder = new DataBuilder(this);
            _entryLength = DataHeader.Size + _builder._extraDataOffsets.Size;
            _childLength = _builder.CalcSize();
            return _entryLength + _childLength;
        }
        protected override void OnWrite(VoidPtr address)
        {
            _builder.Build(address);
            _builder = null;
            Lookup(_builder._lookupAddresses.ToList());
        }
    }
}