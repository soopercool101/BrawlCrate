using System;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Wii.Graphics;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0ShaderNode : MDL0EntryNode
    {
        internal MDL0Shader* Header { get { return (MDL0Shader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.MDL0Shader; } }

        [Browsable(false)]
        public byte Stages { get { return (byte)Children.Count; } }
        public MDL0MaterialNode[] Materials { get { return _materials.ToArray(); } }

        public KSelSwapBlock _swapBlock = KSelSwapBlock.Default;
        public List<MDL0MaterialNode> _materials = new List<MDL0MaterialNode>();
        public sbyte
            _ref0 = -1,
            _ref1 = -1,
            _ref2 = -1,
            _ref3 = -1,
            _ref4 = -1,
            _ref5 = -1,
            _ref6 = -1,
            _ref7 = -1;

        public string[] _fragShaderSource = null;

        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap0Red { get { return (ColorChannel)_swapBlock._Value01.XRB; } set { _swapBlock._Value01.XRB = value; SignalPropertyChange(); } }
        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap0Green { get { return (ColorChannel)_swapBlock._Value01.XGA; } set { _swapBlock._Value01.XGA = value; SignalPropertyChange(); } }

        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap0Blue { get { return (ColorChannel)_swapBlock._Value03.XRB; } set { _swapBlock._Value03.XRB = value; SignalPropertyChange(); } }
        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap0Alpha { get { return (ColorChannel)_swapBlock._Value03.XGA; } set { _swapBlock._Value03.XGA = value; SignalPropertyChange(); } }

        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap1Red { get { return (ColorChannel)_swapBlock._Value05.XRB; } set { _swapBlock._Value05.XRB = value; SignalPropertyChange(); } }
        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap1Green { get { return (ColorChannel)_swapBlock._Value05.XGA; } set { _swapBlock._Value05.XGA = value; SignalPropertyChange(); } }

        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap1Blue { get { return (ColorChannel)_swapBlock._Value07.XRB; } set { _swapBlock._Value07.XRB = value; SignalPropertyChange(); } }
        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap1Alpha { get { return (ColorChannel)_swapBlock._Value07.XGA; } set { _swapBlock._Value07.XGA = value; SignalPropertyChange(); } }

        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap2Red { get { return (ColorChannel)_swapBlock._Value09.XRB; } set { _swapBlock._Value09.XRB = value; SignalPropertyChange();  } }
        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap2Green { get { return (ColorChannel)_swapBlock._Value09.XGA; } set { _swapBlock._Value09.XGA = value; SignalPropertyChange(); } }

        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap2Blue { get { return (ColorChannel)_swapBlock._Value11.XRB; } set { _swapBlock._Value11.XRB = value; SignalPropertyChange(); } }
        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap2Alpha { get { return (ColorChannel)_swapBlock._Value11.XGA; } set { _swapBlock._Value11.XGA = value; SignalPropertyChange(); } }

        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap3Red { get { return (ColorChannel)_swapBlock._Value13.XRB; } set { _swapBlock._Value13.XRB = value; SignalPropertyChange(); } }
        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap3Green { get { return (ColorChannel)_swapBlock._Value13.XGA; } set { _swapBlock._Value13.XGA = value; SignalPropertyChange(); } }

        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap3Blue { get { return (ColorChannel)_swapBlock._Value15.XRB; } set { _swapBlock._Value15.XRB = value; SignalPropertyChange(); } }
        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap3Alpha { get { return (ColorChannel)_swapBlock._Value15.XGA; } set { _swapBlock._Value15.XGA = value; SignalPropertyChange(); } }

        [Category("TEV Indirect Texture Sources"), DisplayName("Indirect Texture Map 0 ID")]
        public TexMapID IndTex0MapID { get { return _swapBlock._Value16.TexMap0; } set { _swapBlock._Value16.TexMap0 = value; SignalPropertyChange(); } }
        [Category("TEV Indirect Texture Sources"), DisplayName("Indirect Texture Coord 0 ID")]
        public TexCoordID IndTex0Coord { get { return _swapBlock._Value16.TexCoord0; } set { _swapBlock._Value16.TexCoord0 = value; SignalPropertyChange(); } }
        [Category("TEV Indirect Texture Sources"), DisplayName("Indirect Texture Map 1 ID")]
        public TexMapID IndTex1MapID { get { return _swapBlock._Value16.TexMap1; } set { _swapBlock._Value16.TexMap1 = value; SignalPropertyChange(); } }
        [Category("TEV Indirect Texture Sources"), DisplayName("Indirect Texture Coord 1 ID")]
        public TexCoordID IndTex1Coord { get { return _swapBlock._Value16.TexCoord1; } set { _swapBlock._Value16.TexCoord1 = value; SignalPropertyChange(); } }
        [Category("TEV Indirect Texture Sources"), DisplayName("Indirect Texture Map 2 ID")]
        public TexMapID IndTex2MapID { get { return _swapBlock._Value16.TexMap2; } set { _swapBlock._Value16.TexMap2 = value; SignalPropertyChange(); } }
        [Category("TEV Indirect Texture Sources"), DisplayName("Indirect Texture Coord 2 ID")]
        public TexCoordID IndTex2Coord { get { return _swapBlock._Value16.TexCoord2; } set { _swapBlock._Value16.TexCoord2 = value; SignalPropertyChange(); } }
        [Category("TEV Indirect Texture Sources"), DisplayName("Indirect Texture Map 3 ID")]
        public TexMapID IndTex3MapID { get { return _swapBlock._Value16.TexMap3; } set { _swapBlock._Value16.TexMap3 = value; SignalPropertyChange(); } }
        [Category("TEV Indirect Texture Sources"), DisplayName("Indirect Texture Coord 3 ID")]
        public TexCoordID IndTex3Coord { get { return _swapBlock._Value16.TexCoord3; } set { _swapBlock._Value16.TexCoord3 = value; SignalPropertyChange(); } }

        public override void RemoveChild(ResourceNode child)
        {
            base.RemoveChild(child);

            foreach (MDL0MaterialNode m in Materials)
            {
                m._updating = true;
                m.ActiveShaderStages = Stages;
                m._updating = false;
            }
        }

        public override void AddChild(ResourceNode child, bool change)
        {
            base.AddChild(child, change);

            foreach (MDL0MaterialNode m in Materials)
            {
                m._updating = true;
                m.ActiveShaderStages = Stages;
                m._updating = false;
            }
        }

        [Category("Shader Data"), Browsable(true), Description("Enables the material's first texture reference for use.")]
        public bool TextureRef0 { get { return _ref0 != -1; } set { _ref0 = (sbyte)(value ? 0 : -1); SignalPropertyChange(); } }
        [Category("Shader Data"), Browsable(true), Description("Enables the material's second texture reference for use.")]
        public bool TextureRef1 { get { return _ref1 != -1; } set { _ref1 = (sbyte)(value ? 1 : -1); SignalPropertyChange(); } }
        [Category("Shader Data"), Browsable(true), Description("Enables the material's third texture reference for use.")]
        public bool TextureRef2 { get { return _ref2 != -1; } set { _ref2 = (sbyte)(value ? 2 : -1); SignalPropertyChange(); } }
        [Category("Shader Data"), Browsable(true), Description("Enables the material's fourth texture reference for use.")]
        public bool TextureRef3 { get { return _ref3 != -1; } set { _ref3 = (sbyte)(value ? 3 : -1); SignalPropertyChange(); } }
        [Category("Shader Data"), Browsable(true), Description("Enables the material's fifth texture reference for use.")]
        public bool TextureRef4 { get { return _ref4 != -1; } set { _ref4 = (sbyte)(value ? 4 : -1); SignalPropertyChange(); } }
        [Category("Shader Data"), Browsable(true), Description("Enables the material's sixth texture reference for use.")]
        public bool TextureRef5 { get { return _ref5 != -1; } set { _ref5 = (sbyte)(value ? 5 : -1); SignalPropertyChange(); } }
        [Category("Shader Data"), Browsable(true), Description("Enables the material's seventh texture reference for use.")]
        public bool TextureRef6 { get { return _ref6 != -1; } set { _ref6 = (sbyte)(value ? 6 : -1); SignalPropertyChange(); } }
        [Category("Shader Data"), Browsable(true), Description("Enables the material's eighth texture reference for use.")]
        public bool TextureRef7 { get { return _ref7 != -1; } set { _ref7 = (sbyte)(value ? 7 : -1); SignalPropertyChange(); } }

        public override void SignalPropertyChange()
        {
            _fragShaderSource = null;
            base.SignalPropertyChange();
        }

        public bool _enabled = true;
        public bool _autoMetal = false;
        public int _texCount = -1;
        public bool _rendered = false;

        public override string Name
        {
            get
            {
                return String.Format("Shader{0}", Index);
            }
            set
            {
                base.Name = value;
            }
        }

        public void Default() { Default(true); }
        public void Default(bool change)
        {
            _ref0 =
            _ref1 =
            _ref2 =
            _ref3 =
            _ref4 =
            _ref5 =
            _ref6 =
            _ref7 = -1;

            MDL0TEVStageNode stage = new MDL0TEVStageNode();
            AddChild(stage, change);
        }

        public void DefaultAsMetal(int texcount)
        {
            _autoMetal = true;

            _ref0 =
            _ref1 =
            _ref2 =
            _ref3 =
            _ref4 =
            _ref5 =
            _ref6 =
            _ref7 = -1;

            switch ((_texCount = texcount) - 1)
            {
                case 0: _ref0 = 0; break;
                case 1: _ref1 = 1; break;
                case 2: _ref2 = 2; break;
                case 3: _ref3 = 3; break;
                case 4: _ref4 = 4; break;
                case 5: _ref5 = 5; break;
                case 6: _ref6 = 6; break;
                case 7: _ref7 = 7; break;
            }

            Children.Clear();

            int i = 0;
            MDL0TEVStageNode s;
            while (i++ < 4)
            {
                AddChild(s = new MDL0TEVStageNode());
                s.DefaultAsMetal(texcount - 1);
            }
        }

        internal override void GetStrings(StringTable table)
        {
            //We DO NOT want to add the name to the string table!
        }

        public override bool OnInitialize()
        {
            _ref0 = Header->_ref0;
            _ref1 = Header->_ref1;
            _ref2 = Header->_ref2;
            _ref3 = Header->_ref3;
            _ref4 = Header->_ref4;
            _ref5 = Header->_ref5;
            _ref6 = Header->_ref6;
            _ref7 = Header->_ref7;
            
            //Attach to materials
            byte* pHeader = (byte*)Header;
            if ((Model != null) && (Model._matList != null))
                foreach (MDL0MaterialNode mat in Model._matList)
                {
                    MDL0Material* mHeader = mat.Header;
                    if (((byte*)mHeader + mHeader->_shaderOffset) == pHeader)
                    {
                        mat._shader = this;
                        _materials.Add(mat);
                    }
                }

            _swapBlock = *Header->SwapBlock;

            return Header->_stages > 0;
        }

        public override void OnPopulate()
        {
            StageGroup* grp = Header->First;
            for (int r = 0; r < 8; r++, grp = grp->Next)
                if (grp->_mask.Reg == 0x61)
                {
                    MDL0TEVStageNode s0 = new MDL0TEVStageNode();

                    KSel ksel = new KSel(grp->_ksel.Data.Value);
                    RAS1_TRef tref = new RAS1_TRef(grp->_tref.Data.Value);

                    s0._colorEnv = grp->_evenColorEnv.Data;
                    s0._alphaEnv = grp->_evenAlphaEnv.Data;
                    s0._cmd = grp->_evenCmd.Data;

                    s0._kcSel = ksel.KCSel0;
                    s0._kaSel = ksel.KASel0;

                    s0._texMapID = tref.TexMapID0;
                    s0._texCoord = tref.TexCoord0;
                    s0._colorChan = tref.ColorChannel0;
                    s0._texEnabled = tref.TexEnabled0;

                    s0._parent = this;
                    _children.Add(s0);

                    if (grp->_oddColorEnv.Reg == 0x61 && grp->_oddAlphaEnv.Reg == 0x61 && grp->_oddCmd.Reg == 0x61)
                    {
                        MDL0TEVStageNode s1 = new MDL0TEVStageNode();

                        s1._colorEnv = grp->_oddColorEnv.Data;
                        s1._alphaEnv = grp->_oddAlphaEnv.Data;
                        s1._cmd = grp->_oddCmd.Data;

                        s1._kcSel = ksel.KCSel1;
                        s1._kaSel = ksel.KASel1;

                        s1._texMapID = tref.TexMapID1;
                        s1._texCoord = tref.TexCoord1;
                        s1._colorChan = tref.ColorChannel1;
                        s1._texEnabled = tref.TexEnabled1;

                        s1._parent = this;
                        _children.Add(s1);
                    }
                }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            MDL0Shader* header = (MDL0Shader*)address;

            header->_dataLength = length;
            header->_index = Index;

            header->_stages = Stages;

            header->_ref0 = _ref0;
            header->_ref1 = _ref1;
            header->_ref2 = _ref2;
            header->_ref3 = _ref3;
            header->_ref4 = _ref4;
            header->_ref5 = _ref5;
            header->_ref6 = _ref6;
            header->_ref7 = _ref7;

            header->_pad0 = 0;
            header->_pad1 = 0;
            header->_pad2 = 0;
            header->_pad3 = 0;
            header->_pad4 = 0;

            *header->SwapBlock = _swapBlock;

            StageGroup* grp = (StageGroup*)(address + 0x80);
            for (int i = 0; i < Children.Count; i++)
            {
                MDL0TEVStageNode c = (MDL0TEVStageNode)Children[i]; //Current Stage

                if ((i & 1) == 0) //Even Stage
                {
                    *grp = StageGroup.Default;

                    grp->SetGroup(i >> 1);
                    grp->SetStage(i);

                    grp->_evenColorEnv.Data = c._colorEnv;
                    grp->_evenAlphaEnv.Data = c._alphaEnv;
                    grp->_evenCmd.Data = c._cmd;

                    if (i == Children.Count - 1) //Last stage is even, odd stage isn't used
                    {
                        grp->_ksel.Data = new KSel(0, 0, c._kcSel, c._kaSel, 0, 0);
                        grp->_tref.Data = new RAS1_TRef(c._texMapID, c._texCoord, c._texEnabled, c._colorChan, TexMapID.TexMap7, TexCoordID.TexCoord7, false, ColorSelChan.Zero);
                    }
                }
                else //Odd Stage
                {
                    MDL0TEVStageNode p = (MDL0TEVStageNode)Children[i - 1]; //Previous Stage

                    grp->SetStage(i);

                    grp->_oddColorEnv.Data = c._colorEnv;
                    grp->_oddAlphaEnv.Data = c._alphaEnv;
                    grp->_oddCmd.Data = c._cmd;

                    grp->_ksel.Data = new KSel(0, 0, p._kcSel, p._kaSel, c._kcSel, c._kaSel);
                    grp->_tref.Data = new RAS1_TRef(p._texMapID, p._texCoord, p._texEnabled, p._colorChan, c._texMapID, c._texCoord, c._texEnabled, c._colorChan);

                    grp = grp->Next;
                }
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return 512;
        }

        internal override void Bind()
        {
        }

        internal override void Unbind()
        {
        }
    }
}
