using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefDataNode : MoveDefEntryNode
    {
        internal MovesetHeader* Header => (MovesetHeader*) WorkingUncompressed.Address;

        public List<SpecialOffset> specialOffsets = new List<SpecialOffset>();
        internal uint DataLen;

        private MovesetHeader hdr;

        [Category("Data Offsets")] public int SubactionFlagsStart => hdr.SubactionFlagsStart;

        [Category("Data Offsets")] public int ModelVisibilityStart => hdr.ModelVisibilityStart;

        [Category("Data Offsets")] public int AttributeStart => hdr.AttributeStart;

        [Category("Data Offsets")] public int SSEAttributeStart => hdr.SSEAttributeStart;

        [Category("Data Offsets")] public int MiscSectionOffset => hdr.MiscSectionOffset;

        [Category("Data Offsets")] public int CommonActionFlagsStart => hdr.CommonActionFlagsStart;

        [Category("Data Offsets")] public int ActionFlagsStart => hdr.ActionFlagsStart;

        [Category("Data Offsets")] public int Unknown7 => hdr.Unknown7;

        [Category("Data Offsets")] public int ActionInterrupts => hdr.ActionInterrupts;

        [Category("Data Offsets")] public int EntryActionsStart => hdr.EntryActionsStart;

        [Category("Data Offsets")] public int ExitActionsStart => hdr.ExitActionsStart;

        [Category("Data Offsets")] public int ActionPreStart => hdr.ActionPreStart;

        [Category("Data Offsets")] public int SubactionMainStart => hdr.SubactionMainStart;

        [Category("Data Offsets")] public int SubactionGFXStart => hdr.SubactionGFXStart;

        [Category("Data Offsets")] public int SubactionSFXStart => hdr.SubactionSFXStart;

        [Category("Data Offsets")] public int SubactionOtherStart => hdr.SubactionOtherStart;

        [Category("Data Offsets")] public int BoneFloats1 => hdr.BoneFloats1;

        [Category("Data Offsets")] public int BoneFloats2 => hdr.BoneFloats2;

        [Category("Data Offsets")] public int BoneRef1 => hdr.BoneRef1;

        [Category("Data Offsets")] public int BoneRef2 => hdr.BoneRef2;

        [Category("Data Offsets")] public int EntryActionOverrides => hdr.EntryActionOverrides;

        [Category("Data Offsets")] public int ExitActionOverrides => hdr.ExitActionOverrides;

        [Category("Data Offsets")] public int Unknown22 => hdr.Unknown22;

        [Category("Data Offsets")] public int BoneFloats3 => hdr.BoneFloats3;

        [Category("Data Offsets")] public int Unknown24 => hdr.Unknown24;

        [Category("Data Offsets")] public int StaticArticles => hdr.StaticArticlesStart;

        [Category("Data Offsets")] public int EntryArticleStart => hdr.EntryArticleStart;

        private int unk27, unk28, flags2;
        private uint flags1;

        //The following aren't offsets 
        [Category("Data Flags")]
        public int Unknown27
        {
            get => unk27;
            set
            {
                unk27 = value;
                SignalPropertyChange();
            }
        }

        [Category("Data Flags")]
        public int Unknown28
        {
            get => unk28;
            set
            {
                unk28 = value;
                SignalPropertyChange();
            }
        }

        [Category("Data Flags")]
        [TypeConverter(typeof(Bin32StringConverter))]
        public Bin32 Flags1bin
        {
            get => new Bin32(flags1);
            set
            {
                flags1 = value._data;
                SignalPropertyChange();
            }
        }

        [Category("Data Flags")]
        public uint Flags1uint
        {
            get => flags1;
            set
            {
                flags1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Data Flags")]
        [TypeConverter(typeof(Bin32StringConverter))]
        public Bin32 Flags2bin
        {
            get => new Bin32((uint) flags2);
            set
            {
                flags2 = (int) value._data;
                SignalPropertyChange();
            }
        }

        [Category("Data Flags")]
        public int Flags2int
        {
            get => flags2;
            set
            {
                flags2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Special Offsets Node")] public SpecialOffset[] Offsets => specialOffsets.ToArray();

        public MoveDefDataNode(uint dataLen, string name)
        {
            DataLen = dataLen;
            _name = name;
        }

        public MoveDefGroupNode _articleGroup;

        public MoveDefFlagsNode _animFlags;
        public MoveDefAttributeNode attributes, sseAttributes;
        public MoveDefMiscNode misc;
        public MoveDefActionOverrideNode override1;
        public MoveDefActionOverrideNode override2;
        public MoveDefActionFlagsNode commonActionFlags, actionFlags;
        public MoveDefUnk7Node unk7;
        public MoveDefActionPreNode actionPre;
        public MoveDefUnk22Node unk22;
        public MoveDefUnk17Node boneFloats3;
        public MoveDefModelVisibilityNode mdlVisibility;
        public MoveDefUnk17Node boneFloats1, boneFloats2;
        public MoveDefArticleNode entryArticle;
        public MoveDefStaticArticleGroupNode staticArticles;
        public MoveDefActionInterruptsNode actionInterrupts;
        public MoveDefUnk24Node unk24;
        public MoveDefBoneIndicesNode boneRef1;
        public MoveDefBoneRef2Node boneRef2;

        //Character Specific Nodes
        //Popo
        public MoveDefActionListNode nanaSubActions;

        public MoveDefSoundDatasNode nanaSoundData;

        //ZSS
        public SZerosuitExtraParams8Node zssParams8;

        public int zssFirstOffset = -1;

        //Wario
        public Wario8 warioParams8;
        public Wario6 warioParams6;
        public int warioSwing4StringOffset = -1;

        public List<MoveDefEntryNode> _extraEntries;
        public List<int> _extraOffsets;
        public List<int> ExtraOffsets => _extraOffsets;

        public SortedList<int, MoveDefEntryNode> _articles;

        public override bool OnInitialize()
        {
            unk27 = Header->Unknown27;
            unk28 = Header->Unknown28;
            flags1 = Header->Flags1;
            flags2 = Header->Flags2;
            hdr = *Header;
            _extraEntries = new List<MoveDefEntryNode>();
            _extraOffsets = new List<int>();
            _articles = new SortedList<int, MoveDefEntryNode>();
            //base.OnInitialize();
            bint* current = (bint*) Header;
            for (int i = 0; i < 27; i++)
            {
                specialOffsets.Add(new SpecialOffset {Index = i, Offset = *current++ + (i == 2 ? 1 : 0)});
            }

            CalculateDataLen();

            return true;
        }

        public override void OnPopulate()
        {
            #region Populate

            if (RootNode is ARCNode && (RootNode as ARCNode).IsFighter)
            {
                int commonActionFlagsCount = 0;
                int actionFlagsCount = 0;
                int totalActionCount = 0;
                List<int> ActionOffsets;

                MoveDefActionListNode subActions =
                        new MoveDefActionListNode {_name = "SubAction Scripts", _parent = this},
                    actions = new MoveDefActionListNode {_name = "Action Scripts", _parent = this};

                bint* actionOffset;

                //Parse offsets first
                for (int i = 9; i < 11; i++)
                {
                    actionOffset = (bint*) (BaseAddress + specialOffsets[i].Offset);
                    ActionOffsets = new List<int>();
                    for (int x = 0; x < specialOffsets[i].Size / 4; x++)
                    {
                        ActionOffsets.Add(actionOffset[x]);
                    }

                    actions.ActionOffsets.Add(ActionOffsets);
                }

                for (int i = 12; i < 16; i++)
                {
                    actionOffset = (bint*) (BaseAddress + specialOffsets[i].Offset);
                    ActionOffsets = new List<int>();
                    for (int x = 0; x < specialOffsets[i].Size / 4; x++)
                    {
                        ActionOffsets.Add(actionOffset[x]);
                    }

                    subActions.ActionOffsets.Add(ActionOffsets);
                }

                if (specialOffsets[4].Size != 0)
                {
                    (misc = new MoveDefMiscNode("Misc Section") {offsetID = 4}).Initialize(this,
                        new DataSource(BaseAddress + specialOffsets[4].Offset, 0));
                }

                int amt = 0;

                //The only way to compute the amount of extra offsets is broken by PSA.
                //Using the exact amount will work for now until REL editing is available.
                switch (RootNode.Name)
                {
                    case "FitZakoBall":
                    case "FitZakoBoy":
                    case "FitZakoGirl":
                    case "FitZakoChild":
                        amt = 1;
                        break;
                    case "FitPurin":
                        amt = 3;
                        break;
                    case "FitKoopa":
                    case "FitMetaknight":
                        amt = 5;
                        break;
                    case "FitGanon":
                    case "FitGKoopa":
                    case "FitMarth":
                        amt = 6;
                        break;
                    case "FitPokeFushigisou":
                        amt = 7;
                        break;
                    case "FitCaptain":
                    case "FitIke":
                    case "FitLuigi":
                    case "FitPokeLizardon":
                    case "FitPokeTrainer":
                    case "FitPokeZenigame":
                    case "FitSonic":
                        amt = 8;
                        break;
                    case "FitDonkey":
                    case "FitSheik":
                    case "FitWarioMan":
                        amt = 9;
                        break;
                    case "FitMario":
                    case "FitWario":
                    case "FitZelda":
                        amt = 10;
                        break;
                    case "FitFalco":
                    case "FitLucario":
                    case "FitPikachu":
                        amt = 11;
                        break;
                    case "FitSZerosuit":
                        amt = 12;
                        break;
                    case "FitDiddy":
                    case "FitFox":
                    case "FitLucas":
                    case "FitPikmin":
                    case "FitPit":
                    case "FitWolf":
                    case "FitYoshi":
                        amt = 13;
                        break;
                    case "FitNess":
                    case "FitPeach":
                    case "FitRobot":
                        amt = 14;
                        break;
                    case "FitDedede":
                    case "FitGameWatch":
                        amt = 16;
                        break;
                    case "FitPopo":
                        amt = 18;
                        break;
                    case "FitLink":
                    case "FitSnake":
                    case "FitToonLink":
                        amt = 20;
                        break;
                    case "FitSamus":
                        amt = 22;
                        break;
                    case "FitKirby":
                        amt = 68;
                        break;
                    default: //Only works on movesets untouched by PSA
                        amt = (Size - 124) / 4;
                        break;
                }

                bint* sPtr = (bint*) (BaseAddress + _offset + 124);
                for (int i = 0; i < amt; i++)
                {
                    _extraOffsets.Add(*sPtr++);
                }

                (attributes = new MoveDefAttributeNode("Attributes") {offsetID = 2}).Initialize(this,
                    new DataSource(BaseAddress + 0, 0x2E4));
                (sseAttributes = new MoveDefAttributeNode("SSE Attributes") {offsetID = 3}).Initialize(this,
                    new DataSource(BaseAddress + 0x2E4, 0x2E4));
                if (specialOffsets[5].Size != 0)
                {
                    (commonActionFlags = new MoveDefActionFlagsNode("Common Action Flags",
                            commonActionFlagsCount = (Unknown7 - CommonActionFlagsStart) / 16)
                        {offsetID = 5}).Initialize(this,
                        new DataSource(BaseAddress + specialOffsets[5].Offset, commonActionFlagsCount * 16));
                }

                if (specialOffsets[6].Size != 0)
                {
                    (actionFlags = new MoveDefActionFlagsNode("Action Flags",
                            actionFlagsCount = (EntryActionsStart - ActionFlagsStart) / 16)
                        {offsetID = 6}).Initialize(this,
                        new DataSource(BaseAddress + specialOffsets[6].Offset, actionFlagsCount * 16));
                }

                totalActionCount = commonActionFlagsCount + actionFlagsCount;
                if (specialOffsets[7].Size != 0)
                {
                    (unk7 = new MoveDefUnk7Node(totalActionCount) {offsetID = 7}).Initialize(this,
                        BaseAddress + specialOffsets[7].Offset, totalActionCount * 8);
                }

                if (specialOffsets[9].Size != 0 || specialOffsets[10].Size != 0)
                {
                    int count;
                    if (specialOffsets[9].Size == 0)
                    {
                        count = specialOffsets[10].Size / 4;
                    }
                    else
                    {
                        count = specialOffsets[9].Size / 4;
                    }

                    if (Root.GetSize(specialOffsets[10].Offset) != Root.GetSize(specialOffsets[9].Offset))
                    {
                        Console.WriteLine(Root.GetSize(specialOffsets[10].Offset) + " " +
                                          Root.GetSize(specialOffsets[9].Offset));
                    }

                    //Initialize using first offset so the node is sorted correctly
                    actions.Initialize(this, BaseAddress + specialOffsets[9].Offset, 0);

                    //Set up groups
                    for (int i = 0; i < count; i++)
                    {
                        actions.AddChild(new MoveDefActionGroupNode {_name = "Action" + (i + 274), offsetID = i},
                            false);
                    }

                    //Add children
                    for (int i = 0; i < 2; i++)
                    {
                        if (specialOffsets[i + 9].Size != 0)
                        {
                            PopulateActionGroup(actions, actions.ActionOffsets[i], false, i);
                        }
                    }

                    //Add to children (because the parent was set before initialization)
                    Children.Add(actions);

                    //actions.Children.Sort(MoveDefEntryNode.ActionCompare);

                    Root._actions = actions;
                }

                if (specialOffsets[11].Size != 0)
                {
                    (actionPre = new MoveDefActionPreNode(totalActionCount)).Initialize(this,
                        new DataSource(BaseAddress + specialOffsets[11].Offset, totalActionCount * 4));
                }

                if (specialOffsets[0].Size != 0)
                {
                    (_animFlags = new MoveDefFlagsNode {offsetID = 0, _parent = this}).Initialize(this,
                        BaseAddress + specialOffsets[0].Offset, specialOffsets[0].Size);
                }

                if (specialOffsets[12].Size != 0 || specialOffsets[13].Size != 0 || specialOffsets[14].Size != 0 ||
                    specialOffsets[15].Size != 0)
                {
                    string name;
                    int count = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (specialOffsets[i + 12].Size != 0)
                        {
                            count = specialOffsets[i + 12].Size / 4;
                            break;
                        }
                    }

                    //Initialize using first offset so the node is sorted correctly
                    subActions.Initialize(this, BaseAddress + specialOffsets[12].Offset, 0);

                    //Set up groups
                    for (int i = 0; i < count; i++)
                    {
                        if (_animFlags._names.Count > i && _animFlags._flags[i]._stringOffset > 0)
                        {
                            name = _animFlags._names[i];
                        }
                        else
                        {
                            name = "<null>";
                        }

                        subActions.AddChild(
                            new MoveDefSubActionGroupNode
                            {
                                _name = name, _flags = _animFlags._flags[i]._Flags,
                                _inTransTime = _animFlags._flags[i]._InTranslationTime
                            }, false);
                    }

                    //Add children
                    for (int i = 0; i < 4; i++)
                    {
                        if (specialOffsets[i + 12].Size != 0)
                        {
                            PopulateActionGroup(subActions, subActions.ActionOffsets[i], true, i);
                        }
                    }

                    //Add to children (because the parent was set before initialization)
                    Children.Add(subActions);

                    Root._subActions = subActions;
                }

                if (specialOffsets[1].Size != 0)
                {
                    (mdlVisibility = new MoveDefModelVisibilityNode {offsetID = 1}).Initialize(this,
                        BaseAddress + specialOffsets[1].Offset, 0);
                }

                if (specialOffsets[19].Size != 0)
                {
                    (boneRef2 = new MoveDefBoneRef2Node {offsetID = 19}).Initialize(this,
                        new DataSource(BaseAddress + specialOffsets[19].Offset, 0));
                }

                if (specialOffsets[24].Size != 0)
                {
                    (unk24 = new MoveDefUnk24Node {offsetID = 24}).Initialize(this,
                        BaseAddress + specialOffsets[24].Offset, 8);
                }

                if (specialOffsets[22].Size != 0)
                {
                    (unk22 = new MoveDefUnk22Node {offsetID = 22}).Initialize(this,
                        new DataSource(BaseAddress + specialOffsets[22].Offset, 0));
                }

                if (specialOffsets[25].Size != 0)
                {
                    (staticArticles = new MoveDefStaticArticleGroupNode {_name = "Static Articles", offsetID = 25})
                        .Initialize(this, new DataSource(BaseAddress + specialOffsets[25].Offset, 8));
                }

                if (specialOffsets[26].Size != 0)
                {
                    (entryArticle = new MoveDefArticleNode {Static = true, _name = "Entry Article", offsetID = 26})
                        .Initialize(this, new DataSource(BaseAddress + specialOffsets[26].Offset, 0));
                }

                if (specialOffsets[8].Size != 0)
                {
                    (actionInterrupts = new MoveDefActionInterruptsNode {offsetID = 8}).Initialize(this,
                        BaseAddress + specialOffsets[8].Offset, 8);
                }

                if (specialOffsets[16].Size != 0)
                {
                    (boneFloats1 = new MoveDefUnk17Node {_name = "Bone Floats 1", offsetID = 16}).Initialize(this,
                        new DataSource(BaseAddress + specialOffsets[16].Offset, 0));
                }

                if (specialOffsets[17].Size != 0)
                {
                    (boneFloats2 = new MoveDefUnk17Node {_name = "Bone Floats 2", offsetID = 17}).Initialize(this,
                        new DataSource(BaseAddress + specialOffsets[17].Offset, 0));
                }

                if (specialOffsets[23].Size != 0)
                {
                    (boneFloats3 = new MoveDefUnk17Node {_name = "Bone Floats 3", offsetID = 23}).Initialize(this,
                        new DataSource(BaseAddress + specialOffsets[23].Offset, 0));
                }

                if (specialOffsets[18].Size != 0)
                {
                    (boneRef1 = new MoveDefBoneIndicesNode("Bone References",
                            (misc.BoneRefOffset - specialOffsets[18].Offset) / 4)
                        {offsetID = 18}).Initialize(this, new DataSource(BaseAddress + specialOffsets[18].Offset, 0));
                }

                if (specialOffsets[20].Size != 0)
                {
                    (override1 = new MoveDefActionOverrideNode {offsetID = 20}).Initialize(this,
                        new DataSource(BaseAddress + specialOffsets[20].Offset, 0));
                }

                if (specialOffsets[21].Size != 0)
                {
                    (override2 = new MoveDefActionOverrideNode {offsetID = 21}).Initialize(this,
                        new DataSource(BaseAddress + specialOffsets[21].Offset, 0));
                }

                //These offsets follow no patterns
                int y = 0;
                MoveDefExternalNode ext = null;
                foreach (int DataOffset in _extraOffsets)
                {
                    if (y == 2 && RootNode.Name == "FitPokeTrainer")
                    {
                        MoveDefSoundDatasNode p = new MoveDefSoundDatasNode
                            {isExtra = true, separate = true, _name = "Sound Data 2"};
                        p.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
                        _extraEntries.Add(p);
                    }
                    else if (y == 49 && RootNode.Name == "FitKirby")
                    {
                        MoveDefKirbyParamList49Node p = new MoveDefKirbyParamList49Node {isExtra = true, offsetID = y};
                        p.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
                        _extraEntries.Add(p);
                    }
                    else if (y == 50 && RootNode.Name == "FitKirby")
                    {
                        //6 offsets
                        //that point to:
                        //offset
                        //count
                        //align to 0x10
                        //that points to list of:
                        //offset
                        //align list to 0x10
                        //that points to:
                        //offset
                        //count
                        //offset (sometimes 0)
                        //count (sometimes 0)
                        //that points to list of:
                        //offset
                        //count
                        //align list to 0x10
                        //that points to:
                        //int value
                    }
                    else if ((y == 51 || y == 52) && RootNode.Name == "FitKirby")
                    {
                        MoveDefKirbyParamList5152Node p = new MoveDefKirbyParamList5152Node
                            {isExtra = true, offsetID = y};
                        p.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
                        _extraEntries.Add(p);
                    }
                    else if (y == 7 && RootNode.Name == "FitPit" || y == 13 && RootNode.Name == "FitRobot")
                    {
                        Pit7Robot13Node p = new Pit7Robot13Node {isExtra = true, offsetID = y};
                        p.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
                        _extraEntries.Add(p);
                    }
                    else if (y == 8 && RootNode.Name == "FitLucario")
                    {
                        HitDataListOffsetNode p = new HitDataListOffsetNode
                            {isExtra = true, _name = "HitDataList" + y, offsetID = y};
                        p.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
                        _extraEntries.Add(p);
                    }
                    else if (y > 9 && RootNode.Name == "FitYoshi")
                    {
                        Yoshi9 p = new Yoshi9 {isExtra = true, offsetID = y};
                        p.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
                        _extraEntries.Add(p);
                    }
                    else if (y == 15 && RootNode.Name == "FitDedede")
                    {
                        Data2ListNode p = new Data2ListNode {isExtra = true, offsetID = y};
                        p.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
                        _extraEntries.Add(p);
                    }
                    else if (
                        y == 56 && RootNode.Name == "FitKirby" ||
                        y == 7 && RootNode.Name == "FitLink" ||
                        y == 8 && RootNode.Name == "FitPeach" ||
                        y == 4 && RootNode.Name == "FitPit" ||
                        y == 7 && RootNode.Name == "FitToonLink")
                    {
                        MoveDefHitDataListNode p = new MoveDefHitDataListNode
                            {isExtra = true, _name = "HitDataList" + y, offsetID = y};
                        p.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
                        _extraEntries.Add(p);
                    }
                    else if (y == 6 && RootNode.Name.StartsWith("FitWario"))
                    {
                        warioParams6 = new Wario6 {isExtra = true, offsetID = y};
                        warioParams6.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
                        _extraEntries.Add(warioParams6);
                    }
                    else if (y == 8 && RootNode.Name.StartsWith("FitWario"))
                    {
                        warioParams8 = new Wario8 {isExtra = true, offsetID = y};
                        warioParams8.Initialize(this, new DataSource(BaseAddress + DataOffset, 0));
                        _extraEntries.Add(warioParams8);
                    }
                    else if (y == 8 && RootNode.Name == "FitSZerosuit")
                    {
                        (zssParams8 = new SZerosuitExtraParams8Node {isExtra = true, offsetID = y}).Initialize(this,
                            BaseAddress + DataOffset, 32);
                        _extraEntries.Add(zssParams8);
                    }
                    else if (y < 4 && RootNode.Name == "FitPopo")
                    {
                        _extraEntries.Add(null);

                        if (y == 0)
                        {
                            nanaSubActions = new MoveDefActionListNode
                                {_name = "Nana SubAction Scripts", isExtra = true};
                        }

                        actionOffset = (bint*) (BaseAddress + DataOffset);
                        ActionOffsets = new List<int>();
                        for (int x = 0; x < Root.GetSize(DataOffset) / 4; x++)
                        {
                            ActionOffsets.Add(actionOffset[x]);
                        }

                        nanaSubActions.ActionOffsets.Add(ActionOffsets);

                        if (y == 3)
                        {
                            string name;
                            int count = 0;
                            for (int i = 0; i < 4; i++)
                            {
                                if ((count = Root.GetSize(DataOffset) / 4) > 0)
                                {
                                    break;
                                }
                            }

                            //Initialize using first offset so the node is sorted correctly
                            nanaSubActions.Initialize(this, BaseAddress + _extraOffsets[0], 0);

                            //Set up groups
                            for (int i = 0; i < count; i++)
                            {
                                if (_animFlags._names.Count > i && _animFlags._flags[i]._stringOffset > 0)
                                {
                                    name = _animFlags._names[i];
                                }
                                else
                                {
                                    name = "<null>";
                                }

                                nanaSubActions.AddChild(
                                    new MoveDefSubActionGroupNode
                                    {
                                        _name = name, _flags = _animFlags._flags[i]._Flags,
                                        _inTransTime = _animFlags._flags[i]._InTranslationTime
                                    }, false);
                            }

                            //Add children
                            for (int i = 0; i < 4; i++)
                            {
                                PopulateActionGroup(nanaSubActions, nanaSubActions.ActionOffsets[i], true, i);
                            }
                        }
                    }
                    else if (y == 10 && RootNode.Name == "FitPopo")
                    {
                        (nanaSoundData = new MoveDefSoundDatasNode {_name = "Nana Sound Data", isExtra = true})
                            .Initialize(this, (VoidPtr) Header + 124 + y * 4, 8);
                        _extraEntries.Add(null);
                    }
                    else
                    {
                        if (DataOffset > Root.dataSize) //probably flags or float
                        {
                            continue;
                        }

                        ext = null;
                        if (DataOffset > 1480) //I don't think a count would be greater than this
                        {
                            MoveDefEntryNode entry = null;
                            if ((ext = Root.IsExternal(DataOffset)) != null)
                            {
                                if (ext.Name.StartsWith("param"))
                                {
                                    int o = 0;
                                    if (y < _extraOffsets.Count - 1 && (o = _extraOffsets[y + 1]) < 1480 && o > 1)
                                    {
                                        MoveDefRawDataNode d = new MoveDefRawDataNode("ExtraParams" + y)
                                            {offsetID = y, isExtra = true};
                                        d.Initialize(this, BaseAddress + DataOffset, 0);
                                        for (int i = 0; i < o; i++)
                                        {
                                            new MoveDefSectionParamNode {_name = "Part" + i, _extOverride = i == 0}
                                                .Initialize(d, BaseAddress + DataOffset + d.Size / o * i, d.Size / o);
                                        }

                                        entry = d;
                                    }
                                    else
                                    {
                                        MoveDefSectionParamNode p = new MoveDefSectionParamNode
                                            {_name = "ExtraParams" + y, isExtra = true, offsetID = y};
                                        p.Initialize(this, BaseAddress + DataOffset, 0);
                                        entry = p;
                                    }
                                }
                                else
                                {
                                    Article* test = (Article*) (BaseAddress + DataOffset);
                                    if (Root.GetSize(DataOffset) < 52 ||
                                        test->_actionsStart > Root.dataSize || test->_actionsStart % 4 != 0 ||
                                        test->_subactionFlagsStart > Root.dataSize ||
                                        test->_subactionFlagsStart % 4 != 0 ||
                                        test->_subactionGFXStart > Root.dataSize || test->_subactionGFXStart % 4 != 0 ||
                                        test->_subactionSFXStart > Root.dataSize || test->_subactionSFXStart % 4 != 0 ||
                                        test->_modelVisibility > Root.dataSize || test->_modelVisibility % 4 != 0 ||
                                        test->_arcGroup > byte.MaxValue || test->_boneID > short.MaxValue ||
                                        test->_id > 1480)
                                    {
                                        int o = 0;
                                        if (y < _extraOffsets.Count - 1 && (o = _extraOffsets[y + 1]) < 1480 && o > 1)
                                        {
                                            MoveDefRawDataNode d = new MoveDefRawDataNode("ExtraParams" + y)
                                                {offsetID = y, isExtra = true};
                                            d.Initialize(this, BaseAddress + DataOffset, 0);
                                            for (int i = 0; i < o; i++)
                                            {
                                                new MoveDefSectionParamNode {_name = "Part" + i, _extOverride = i == 0}
                                                    .Initialize(d,
                                                        BaseAddress + DataOffset + d.Size / o * i, d.Size / o);
                                            }

                                            entry = d;
                                        }
                                        else
                                        {
                                            MoveDefSectionParamNode p = new MoveDefSectionParamNode
                                                {_name = "ExtraParams" + y, isExtra = true, offsetID = y};
                                            p.Initialize(this, BaseAddress + DataOffset, 0);
                                            entry = p;
                                        }
                                    }
                                    else
                                    {
                                        if (_articleGroup == null)
                                        {
                                            _articleGroup = new MoveDefGroupNode {_name = "Articles"};
                                            _articleGroup.Initialize(this, BaseAddress + DataOffset, 0);
                                        }

                                        (entry = new MoveDefArticleNode
                                                {offsetID = y, Static = true, isExtra = true, extraOffset = true})
                                            .Initialize(_articleGroup, BaseAddress + DataOffset, 0);
                                        _articles.Add(entry._offset, entry);
                                    }
                                }
                            }
                            else
                            {
                                Article* test = (Article*) (BaseAddress + DataOffset);
                                if (Root.GetSize(DataOffset) < 52 ||
                                    test->_actionsStart > Root.dataSize || test->_actionsStart % 4 != 0 ||
                                    test->_subactionFlagsStart > Root.dataSize || test->_subactionFlagsStart % 4 != 0 ||
                                    test->_subactionGFXStart > Root.dataSize || test->_subactionGFXStart % 4 != 0 ||
                                    test->_subactionSFXStart > Root.dataSize || test->_subactionSFXStart % 4 != 0 ||
                                    test->_modelVisibility > Root.dataSize || test->_modelVisibility % 4 != 0 ||
                                    test->_arcGroup > byte.MaxValue || test->_boneID > short.MaxValue ||
                                    test->_id > 1480)
                                {
                                    int o = 0;
                                    if (y < _extraOffsets.Count - 1 && (o = _extraOffsets[y + 1]) < 1480 && o > 1)
                                    {
                                        MoveDefRawDataNode d = new MoveDefRawDataNode("ExtraParams" + y)
                                            {offsetID = y, isExtra = true};
                                        d.Initialize(this, BaseAddress + DataOffset, 0);
                                        for (int i = 0; i < o; i++)
                                        {
                                            new MoveDefSectionParamNode {_name = "Part" + i, _extOverride = i == 0}
                                                .Initialize(d, BaseAddress + DataOffset + d.Size / o * i, d.Size / o);
                                        }

                                        entry = d;
                                    }
                                    else
                                    {
                                        MoveDefSectionParamNode p = new MoveDefSectionParamNode
                                            {_name = "ExtraParams" + y, isExtra = true, offsetID = y};
                                        p.Initialize(this, BaseAddress + DataOffset, 0);
                                        entry = p;
                                    }
                                }
                                else
                                {
                                    if (_articleGroup == null)
                                    {
                                        _articleGroup = new MoveDefGroupNode {_name = "Articles"};
                                        _articleGroup.Initialize(this, BaseAddress + DataOffset, 0);
                                    }

                                    (entry = new MoveDefArticleNode
                                            {offsetID = y, isExtra = true, Static = true, extraOffset = true})
                                        .Initialize(_articleGroup, BaseAddress + DataOffset, 0);
                                    _articles.Add(entry._offset, entry);
                                }
                            }

                            _extraEntries.Add(entry);
                        }
                    }

                    y++;
                }

                misc.Populate();

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

                //if (specialOffsets[9].Size != 0)
                //    new MoveDefActionsNode("Actions 1").Initialize(this, new DataSource(BaseAddress + specialOffsets[9].Offset, specialOffsets[9].Size));

                //if (specialOffsets[10].Size != 0)
                //    new MoveDefActionsNode("Actions 2").Initialize(this, new DataSource(BaseAddress + specialOffsets[10].Offset, specialOffsets[10].Size));

                ////if (specialOffsets[12].Size != 0)
                //(Root.Main = new MoveDefActionsNode("Main SubActions") { /*_parent = this*/ }).Initialize(this, new DataSource(BaseAddress + specialOffsets[12].Offset, specialOffsets[12].Size));

                ////if (specialOffsets[13].Size != 0)
                //(Root.GFX = new MoveDefActionsNode("GFX SubActions") { /*_parent = this*/ }).Initialize(this, new DataSource(BaseAddress + specialOffsets[13].Offset, specialOffsets[13].Size));

                ////if (specialOffsets[14].Size != 0)
                //(Root.SFX = new MoveDefActionsNode("SFX SubActions") { /*_parent = this*/ }).Initialize(this, new DataSource(BaseAddress + specialOffsets[14].Offset, specialOffsets[14].Size));

                ////if (specialOffsets[15].Size != 0)
                //(Root.Other = new MoveDefActionsNode("Other SubActions") { /*_parent = this*/ }).Initialize(this, new DataSource(BaseAddress + specialOffsets[15].Offset, specialOffsets[15].Size));

                //MoveDefGroupNode articles = new MoveDefGroupNode() { _name = "Articles", _parent = this };
            }

            #endregion

            SortChildren();
        }

        private void CalculateDataLen()
        {
            List<SpecialOffset> sorted = specialOffsets.OrderBy(x => x.Offset).ToList();
            for (int i = 0; i < sorted.Count; i++)
            {
                if (i < sorted.Count - 1)
                {
                    if (sorted[i + 1].Index == 2)
                    {
                        sorted[i].Size = (int) ((sorted[i + 1].Offset -= 1) - sorted[i].Offset);
                    }
                    else
                    {
                        sorted[i].Size = (int) (sorted[i + 1].Offset - sorted[i].Offset);
                    }
                }
                else
                {
                    sorted[i].Size = (int) (DataLen - sorted[i].Offset);
                }

                //Console.WriteLine(sorted[i].ToString());
            }
        }

        public void PopulateActionGroup(ResourceNode g, List<int> ActionOffsets, bool subactions, int index)
        {
            string name = "";
            if (subactions)
            {
                if (index == 0)
                {
                    name = "Main";
                }
                else if (index == 1)
                {
                    name = "GFX";
                }
                else if (index == 2)
                {
                    name = "SFX";
                }
                else if (index == 3)
                {
                    name = "Other";
                }
                else
                {
                    return;
                }
            }
            else if (index == 0)
            {
                name = "Entry";
            }
            else if (index == 1)
            {
                name = "Exit";
            }

            int i = 0;
            foreach (int offset in ActionOffsets)
            {
                if (offset > 0)
                {
                    new MoveDefActionNode(name, false, g.Children[i]).Initialize(g.Children[i],
                        new DataSource(BaseAddress + offset, 0));
                }
                else
                {
                    g.Children[i].Children.Add(new MoveDefActionNode(name, true, g.Children[i]));
                }

                i++;

                if (subactions && i == _animFlags._names.Count || i == g.Children.Count)
                {
                    break;
                }
            }
        }

        public FDefSubActionStringTable subActionTable;
        public VoidPtr dataHeaderAddr;

        public int
            part1Len = 0,
            part2Len = 0,
            part3Len = 0,
            part4Len = 0,
            part5Len = 0,
            part6Len = 0,
            part7Len = 0,
            part8Len = 0;

        public override int OnCalculateSize(bool force)
        {
            zssFirstOffset = warioSwing4StringOffset = -1;
            _entryLength = 124 + _extraOffsets.Count * 4;
            _childLength = MovesetConverter.CalcDataSize(this);
            return _entryLength + _childLength;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            MovesetConverter.BuildData(this, (MovesetHeader*) dataHeaderAddr, address, length, force);
        }
    }
}