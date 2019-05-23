using System;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class DataCommonSection : TableEntryNode
    {
        internal static TableEntryNode TestType(string name)
        {
            return name == "dataCommon" ? new DataCommonSection() : null;
        }

        DataCommonHeader hdr;
        
        [Category("Data Offsets")]
        public int GlobalICBasics { get { return hdr.GlobalICs; } }
        [Category("Data Offsets")]
        public int GlobalICBasicsSSE { get { return hdr.SSEGlobalICs; } }
        [Category("Data Offsets")]
        public int ICBasics { get { return hdr.ICs; } }
        [Category("Data Offsets")]
        public int ICBasicsSSE { get { return hdr.SSEICs; } }
        [Category("Data Offsets")]
        public int EntryActions { get { return hdr.EntryActions; } }
        [Category("Data Offsets")]
        public int ExitActions { get { return hdr.ExitActions; } }
        [Category("Data Offsets")]
        public int FlashOverlaysList { get { return hdr.FlashOverlayArray; } }
        [Category("Data Offsets")]
        public int Unk7 { get { return hdr.Unknown7; } }
        [Category("Data Offsets")]
        public int Unk8 { get { return hdr.Unknown8; } }
        [Category("Data Offsets")]
        public int Unk9 { get { return hdr.Unknown9; } }
        [Category("Data Offsets")]
        public int Unk10 { get { return hdr.Unknown10; } }
        [Category("Data Offsets")]
        public int Unk11 { get { return hdr.Unknown11; } }
        [Category("Data Offsets")]
        public int Unk12 { get { return hdr.Unknown12; } }
        [Category("Data Offsets")]
        public int Unk13 { get { return hdr.Unknown13; } }
        [Category("Data Offsets")]
        public int Unk14 { get { return hdr.Unknown14; } }
        [Category("Data Offsets")]
        public int Unk15 { get { return hdr.Unknown15; } }
        [Category("Data Offsets")]
        public int Unk16 { get { return hdr.Unknown16; } }
        [Category("Data Offsets")]
        public int Unk17 { get { return hdr.Unknown17; } }
        [Category("Data Offsets")]
        public int Unk18 { get { return hdr.Unknown18; } }
        [Category("Data Offsets")]
        public int FlashOverlayOffset { get { return hdr.FlashOverlays; } }
        [Category("Data Offsets")]
        public int ScreenTintOffset { get { return hdr.ScreenTints; } }
        [Category("Data Offsets")]
        public int LegBoneNames { get { return hdr.LegBones; } }
        [Category("Data Offsets")]
        public int Unk22 { get { return hdr.Unknown22; } }
        [Category("Data Offsets")]
        public int Unk23 { get { return hdr.Unknown23; } }
        [Category("Data Offsets")]
        public int Unk24 { get { return hdr.Unknown24; } }
        [Category("Data Offsets")]
        public int Unk25 { get { return hdr.Unknown25; } }
        
        [Browsable(false)]
        public BindingList<CommonAction> ScreenTints { get { return _screenTints; } }
        [Browsable(false)]
        public BindingList<CommonAction> FlashOverlays { get { return _flashOverlays; } }
        
        private BindingList<CommonAction> _flashOverlays, _screenTints;
        
        public EntryList<CommonUnk7Entry> _unk7;
        public EntryList<Unknown11EntryNode> _unk11;

        public CmnLegBonesNode _legBones;
        public CmnPatternPowerMulNode _ppMul;

        //All parameter lists
        public RawParamList
            _globalICs,
            _globalsseICs,
            _ICs,
            _sseICs,
            _unk8,
            _itemSwingData,
            _unk10,
            _unk12,
            _unk13,
            _unk14,
            _unk15,
            _unk16,
            _unk18,
            _unk23,
            _unk24;
        
        protected override void OnParse(VoidPtr address)
        {
            hdr = *(DataCommonHeader*)address;
            bint* v = (bint*)address;
            int offset = 0;

            //Calculate the sizes of each section using their offsets,
            //in order of appearance
            int[] sizes = SakuraiArchiveNode.CalculateSizes(_root._dataSize, v, 26, false);
            
            //Parse all script-related data first
            ParseScripts(v, sizes);

            //These ICs need to be sorted into int and float arrays
            //Right now they're just a mess of values
            //The indices in the IC variable storage class
            _globalICs = Parse<RawParamList>(v[0], 188);
            _globalsseICs = Parse<RawParamList>(v[1], 188);
            _ICs = Parse<RawParamList>(v[2], 2204);
            _sseICs = Parse<RawParamList>(v[3], 2204);
            //Entry action script offsets
            //Exit action script offsets
            //Flash overlay script offsets/flags
            _unk7 = Parse<EntryList<CommonUnk7Entry>>(v[7], 12);
            _unk8 = Parse<RawParamList>(v[8], 0x1A4);
            _itemSwingData = Parse<RawParamList>(v[9], 0x64);
            _unk10 = Parse<RawParamList>(v[10], 0x10);
            if ((offset = v[11]) > 0)
            {
                sListOffset* list = (sListOffset*)Address(offset);
                _unk11 = Parse<EntryList<Unknown11EntryNode>>(list->_startOffset, 12, (int)list->_listCount);
            }
            _unk12 = Parse<RawParamList>(v[12], 0x80);
            _unk13 = Parse<RawParamList>(v[13], 0x80);
            _unk14 = Parse<RawParamList>(v[14], 0x40);
            _unk15 = Parse<RawParamList>(v[15], 0x24);
            _unk16 = Parse<RawParamList>(v[16], 0x48);
            _ppMul = Parse<CmnPatternPowerMulNode>(v[17]);
            _unk18 = Parse<RawParamList>(v[18], 0x10);

            //Screen tint script offsets
            _legBones = Parse<CmnLegBonesNode>(v[21]);

            if ((offset = v[23]) > 0)
                _unk23 = Parse<RawParamList>(*(bint*)Address(offset), 0xA8);
            
            _unk24 = Parse<RawParamList>(v[24], 4);

            //Notes:

            //Unk12 and Unk13 are copies of the same parameters
            //with some different values

            //Unk23 is one value (0) in Global IC-Basics - offset to data at start of section child data
            //Params24 is one value (32) in IC-Basics
        }

        private void ParseScripts(bint* hdr, int[] sizes)
        {
            Script s = null;
            int size, count, offset;
            bint* actionOffset;

            MovesetNode node = _root as MovesetNode;

            //Collect offsets first
            for (int i = 0; i < 2; i++)
            {
                if ((offset = hdr[i + 4]) < 0)
                    continue;

                actionOffset = (bint*)Address(offset);
                for (int x = 0; x < sizes[i + 4] / 4; x++)
                    node._scriptOffsets[0][i].Add(*actionOffset++);
            }

            List<uint>[] flags = new List<uint>[] { new List<uint>(), new List<uint>() };
            for (int i = 0; i < 2; i++)
            {
                size = _root.GetSize(offset = hdr[i + 19]);
                if (offset > 0)
                {
                    actionOffset = (bint*)Address(offset);
                    for (int x = 0; x < size / 16; x++)
                    {
                        node._scriptOffsets[i + 3][0].Add(*actionOffset++);
                        flags[i].Add((uint)(int)*actionOffset++);
                    }
                }
            }

            //Now parse scripts

            ActionEntry ag;
            List<List<int>> actionOffsets = node._scriptOffsets[0];
            count = Math.Max(actionOffsets[0].Count, actionOffsets[1].Count);
            node._actions = new BindingList<ActionEntry>();
            for (int i = 0; i < count; i++)
            {
                node.Actions.Add(ag = new ActionEntry(new sActionFlags(), i, i));
                for (int x = 0; x < 2; x++)
                {
                    if (i < actionOffsets[x].Count && actionOffsets[x][i] > 0)
                        s = Parse<Script>(actionOffsets[x][i]);
                    else
                        s = new Script();
                    ag.SetWithType(x, s);
                }
            }

            _flashOverlays = new BindingList<CommonAction>();
            _screenTints = new BindingList<CommonAction>();
            BindingList<CommonAction>[] lists = new BindingList<CommonAction>[]
            {
                _flashOverlays,
                _screenTints
            };

            CommonAction ca;
            for (int x = 0; x < 2; x++)
            {
                List<int> offsets = node._scriptOffsets[x + 3][0];
                count = offsets.Count;
                for (int i = 0; i < count; i++)
                {
                    uint flag = flags[x][i];
                    ca = i < offsets.Count && offsets[i] > 0 ? 
                        Parse<CommonAction>(offsets[i], flag) :
                        new CommonAction(flag);
                    ca._index = i;
                    lists[x].Add(ca);
                }
            }
        }

        /*
        public VoidPtr dataHeaderAddr;
        public override void Parse(VoidPtr address)
        {
            #region Populate
            //if (ARCNode.SpecialName.Contains(RootNode.Name))
            //{
                MoveDefGroupNode g;
                List<int> ActionOffsets;

                MoveDefActionListNode actions = new MoveDefActionListNode() { _name = "Action Scripts", _parent = this };

                bint* actionOffset;

                //Parse offsets first
                for (int i = 4; i < 6; i++)
                {
                    actionOffset = (bint*)(BaseAddress + specialOffsets[i].Offset);
                    ActionOffsets = new List<int>();
                    for (int x = 0; x < specialOffsets[i].Size / 4; x++)
                        ActionOffsets.Add(actionOffset[x]);
                    actions.ActionOffsets.Add(ActionOffsets);
                }

                int r = 0;
                foreach (SpecialOffset s in specialOffsets)
                {
                    if (r != 4 && r != 5 && r != 6 && r != 7 && r != 11 && r != 17 && r != 19 && r != 20 && r != 21 && r != 22 && r != 23)
                    {
                        string name = "Params" + r;
                        if (r < 4) name = (r == 0 || r == 2 ? "" : "SSE ") + (r < 2 ? "Global " : "") + "IC-Basics";
                        
                        //Value at 0x64 in Global IC-Basics is not an IC; it is the offset to Unknown32's data.
                        //Value at 0x72C in IC-Basics is not an IC; it is the value of Params24.

                        new RawParamList(r) { _name = name }.Initialize(this, BaseAddress + s.Offset, 0);
                    }
                    r++;
                }

                //Copy list of offset 19?
                //if (specialOffsets[6].Size != 0)
                //    new MoveDefActionsNode("Flash Overlay Actions") { offsetID = 6 }.Initialize(this, BaseAddress + specialOffsets[6].Offset, 0);

                if (specialOffsets[7].Size != 0)
                    new MoveDefCommonUnk7ListNode() { _name = "Unknown7", _offsetID = 7 }.Initialize(this, BaseAddress + specialOffsets[7].Offset, 0);

                if (specialOffsets[11].Size != 0)
                    new MoveDefUnk11Node() { _name = "Unknown11", _offsetID = 11 }.Initialize(this, BaseAddress + specialOffsets[11].Offset, 0);

                if (specialOffsets[19].Size != 0)
                    (_flashOverlay = new MoveDefActionsSkipNode("Flash Overlay Actions") { _offsetID = 19 }).Initialize(this, BaseAddress + specialOffsets[19].Offset, 0);

                if (specialOffsets[20].Size != 0)
                    (_screenTint = new MoveDefActionsSkipNode("Screen Tint Actions") { _offsetID = 20 }).Initialize(this, BaseAddress + specialOffsets[20].Offset, 0);

                if (specialOffsets[21].Size != 0)
                    new MoveDefCommonUnk21Node() { _offsetID = 21 }.Initialize(this, BaseAddress + specialOffsets[21].Offset, 0);

                if (specialOffsets[22].Size != 0)
                    new MoveDefParamListNode() { _name = "Unknown22", _offsetID = 22 }.Initialize(this, BaseAddress + specialOffsets[22].Offset, 0);

                if (specialOffsets[23].Size != 0)
                    new MoveDefParamsOffsetNode() { _name = "Unknown23", _offsetID = 23 }.Initialize(this, BaseAddress + specialOffsets[23].Offset, 0);

                if (specialOffsets[17].Size != 0)
                    new MoveDefPatternPowerMulNode() { _name = "Unknown17", _offsetID = 17 }.Initialize(this, BaseAddress + specialOffsets[17].Offset, 0);

                if (specialOffsets[4].Size != 0 || specialOffsets[5].Size != 0)
                {
                    int count;
                    if (specialOffsets[4].Size == 0)
                        count = specialOffsets[5].Size / 4;
                    else
                        count = specialOffsets[4].Size / 4;

                    //Initialize using first offset so the node is sorted correctly
                    actions.Initialize(this, BaseAddress + specialOffsets[4].Offset, 0);
                    
                    //Set up groups
                    for (int i = 0; i < count; i++)
                        actions.AddChild(new ActionGroup() { _name = "Action" + i }, false);

                    //Add children
                    for (int i = 0; i < 2; i++)
                        if (specialOffsets[i + 4].Size != 0)
                            PopulateActionGroup(actions, actions.ActionOffsets[i], false, i);

                    //Add to children (because the parent was set before initialization)
                    Children.Add(actions);

                    _root._actions = actions;
                }
            //}
            #endregion
        }
         */
    }

    public unsafe class CommonAction : Script
    {
        public byte Unk1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [TypeConverter(typeof(Bin8StringConverter))]
        public Bin8 Flags { get { return new Bin8(_unk2); } set { _unk2 = (byte)value._data; SignalPropertyChange(); } }
        public byte Unk3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }
        public byte Unk4 { get { return _unk4; } set { _unk4 = value; SignalPropertyChange(); } }
        
        public byte _unk1, _unk2, _unk3, _unk4;

        public CommonAction(uint flags)
        {
            _unk1 = (byte)((flags >> 24) & 0xFF);
            _unk2 = (byte)((flags >> 16) & 0xFF);
            _unk3 = (byte)((flags >> 8) & 0xFF);
            _unk4 = (byte)((flags >> 0) & 0xFF);
        }
    }

    public unsafe class CommonUnk7Entry : MovesetEntryNode
    {
        public List<CommonUnk7EntryListEntry> _children;

        public int _dataOffset, _count;
        public short unk3, unk4;

        [Category("Unknown 7 Entry")]
        public int DataOffset { get { return _dataOffset; } }
        [Category("Unknown 7 Entry")]
        public int Count { get { return _count; } }
        [Category("Unknown 7 Entry")]
        public short Unknown1 { get { return unk3; } set { unk3 = value; SignalPropertyChange(); } }
        [Category("Unknown 7 Entry")]
        public short Unknown2 { get { return unk4; } set { unk4 = value; SignalPropertyChange(); } }

        protected override void OnParse(VoidPtr address)
        {
            _children = new List<CommonUnk7EntryListEntry>();

            sCommonUnk7Entry* hdr = (sCommonUnk7Entry*)address;
            _count = hdr->_list._startOffset;
            _dataOffset = hdr->_list._listCount;
            unk3 = hdr->_unk3;
            unk4 = hdr->_unk4;
            for (int i = 0; i < Count; i++)
                _children.Add(Parse<CommonUnk7EntryListEntry>(DataOffset + i * 8));
        }

        protected override int OnGetSize()
        {
            return 12;
        }

        protected override void OnWrite(VoidPtr address)
        {
            RebuildAddress = address;
            sCommonUnk7Entry* data = (sCommonUnk7Entry*)address;
            data->_list._startOffset = _dataOffset;
            data->_list._listCount = _children.Count;
            data->_unk3 = unk3;
            data->_unk4 = unk4;
        }
    }

    public unsafe class CommonUnk7EntryListEntry : MovesetEntryNode
    {
        public float unk1, unk2;

        [Category("Unknown 7 Entry")]
        public float Unknown1 { get { return unk1; } set { unk1 = value; SignalPropertyChange(); } }
        [Category("Unknown 7 Entry")]
        public float Unknown2 { get { return unk2; } set { unk2 = value; SignalPropertyChange(); } }

        protected override void OnParse(VoidPtr address)
        {
            sCommonUnk7EntryListEntry* hdr = (sCommonUnk7EntryListEntry*)address;
            unk1 = hdr->_unk1;
            unk2 = hdr->_unk2;
        }
        protected override int OnGetSize() { return 8; }
        protected override void OnWrite(VoidPtr address)
        {
            RebuildAddress = address;
            sCommonUnk7EntryListEntry* data = (sCommonUnk7EntryListEntry*)address;
            data->_unk1 = unk1;
            data->_unk2 = unk2;
        }
    }
}