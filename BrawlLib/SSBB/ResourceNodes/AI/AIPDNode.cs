using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class AIPDNode : ARCEntryNode
    {
        public override ResourceType ResourceFileType => ResourceType.AIPD;
        internal AIPD* Header => (AIPD*) WorkingUncompressed.Address;

        public override bool OnInitialize()
        {
            if (_name == null || _name == "")
            {
                _name = "AIPD_" + Parent.Name.Replace("ai_", "");
            }

            return true;
        }

        public override void OnPopulate()
        {
            new AIPDDefBlockNode().Initialize(this, new DataSource(Header->DefBlock1, 0x60));
            new AIPDDefBlockNode().Initialize(this, new DataSource(Header->DefBlock2, 0x60));
            new AIPDSubBlockNode().Initialize(this, new DataSource(Header->SubBlock1, 0x30));
            new AIPDSubBlockNode().Initialize(this, new DataSource(Header->SubBlock2, 0x30));
            new AIPDUnkBlockNode().Initialize(this, new DataSource(Header->UnkBlock, 0x40));
            new AIPDType1OffsetsNode().Initialize(this, new DataSource(Header->Type1Offsets, 0x70));
            new AIPDType2OffsetsNode().Initialize(this,
                new DataSource(Header->Type2Offsets, 0x0)); //size cannot be recognized
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            AIPD* header = (AIPD*) address;
            header->_tag = AIPD.Tag;
            header->_unk1 = Header->_unk1;
            header->_unk2 = Header->_unk2;
            header->DataOffset = Header->DataOffset;
            int offset = 0x10;
            int type2Offsetsize = 0;
            int type1EntrySize = 0;
            foreach (ResourceNode n in Children)
            {
                if (n is AIPDType2OffsetsNode)
                {
                    type2Offsetsize += ((AIPDType2OffsetsNode) n).CalculateSize(true);
                    break;
                }
            }

            foreach (ResourceNode n in Children)
            {
                int size = n.CalculateSize(true);
                if (n is AIPDType1OffsetsNode)
                {
                    ((AIPDType1OffsetsNode) n).Type2OffsetSize = type2Offsetsize;
                    type1EntrySize = ((AIPDType1OffsetsNode) n).CalculateChildrenSize();
                    type1EntrySize =
                        type1EntrySize + 0x10 -
                        (type1EntrySize + type2Offsetsize) % 0x10; //size of type1EntrySize must be multiple of ten
                }

                if (n is AIPDType2OffsetsNode)
                {
                    ((AIPDType2OffsetsNode) n).Type1EntrySize = type1EntrySize;
                }

                n.Rebuild(address + offset, size, true);
                offset += size;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = 0x10; //AIPD header
            int type2OffsetSize = 0;
            int type1EntrySize = 0;
            foreach (ResourceNode n in Children)
            {
                if (n is AIPDType2OffsetsNode)
                {
                    type2OffsetSize = n.CalculateSize(true);
                }
            }

            foreach (ResourceNode n in Children)
            {
                size += n.CalculateSize(true);
                if (n is AIPDType1OffsetsNode)
                {
                    type1EntrySize = ((AIPDType1OffsetsNode) n).CalculateChildrenSize();
                    size += type1EntrySize + 0x10 -
                            (type1EntrySize + type2OffsetSize) % 0x10; //size of type1EntrySize must be multiple of ten
                }
                else if (n is AIPDType2OffsetsNode)
                {
                    size += ((AIPDType2OffsetsNode) n).CalculateChildrenSize();
                }
            }

            return size;
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((AIPD*) source.Address)->_tag == AIPD.Tag ? new AIPDNode() : null;
        }
    }

    public unsafe class AIPDDefBlockNode : ResourceNode
    {
        internal AIPDDefBlock* Header => (AIPDDefBlock*) WorkingUncompressed.Address;

        #region Fields

        internal float float1, float2, float3, float4, float5, float6, float7, float8, float9, float10;

        internal short short1,
            short2,
            short3,
            short4,
            short5,
            short6,
            short7,
            short8,
            short9,
            short10,
            short11,
            short12;

        internal int int1, int2, int3, int4, int5, int6, int7;
        internal byte byte1, byte2, byte3, byte4;

        #endregion

        #region Properties

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Float1
        {
            get => float1.ToString();
            set
            {
                float1 = Convert.ToSingle(value);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Float2
        {
            get => float2.ToString();
            set
            {
                float2 = Convert.ToSingle(value);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short1
        {
            get => short1.ToString("X");
            set
            {
                short1 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short2
        {
            get => short2.ToString("X");
            set
            {
                short2 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short3
        {
            get => short3.ToString("X");
            set
            {
                short3 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short4
        {
            get => short4.ToString("X");
            set
            {
                short4 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Float3
        {
            get => float3.ToString();
            set
            {
                float3 = Convert.ToSingle(value);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Float4
        {
            get => float4.ToString();
            set
            {
                float4 = Convert.ToSingle(value);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short5
        {
            get => short5.ToString("X");
            set
            {
                short5 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short6
        {
            get => short6.ToString("X");
            set
            {
                short6 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short7
        {
            get => short7.ToString("X");
            set
            {
                short7 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short8
        {
            get => short8.ToString("X");
            set
            {
                short8 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Float5
        {
            get => float5.ToString();
            set
            {
                float5 = Convert.ToSingle(value);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short9
        {
            get => short9.ToString("X");
            set
            {
                short9 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short10
        {
            get => short10.ToString("X");
            set
            {
                short10 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Float6
        {
            get => float6.ToString();
            set
            {
                float6 = Convert.ToSingle(value);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short11
        {
            get => short11.ToString("X");
            set
            {
                short11 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short12
        {
            get => short12.ToString("X");
            set
            {
                short12 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Float7
        {
            get => float7.ToString();
            set
            {
                float7 = Convert.ToSingle(value);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Float8
        {
            get => float8.ToString();
            set
            {
                float8 = Convert.ToSingle(value);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Float9
        {
            get => float9.ToString();
            set
            {
                float9 = Convert.ToSingle(value);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Float10
        {
            get => float10.ToString();
            set
            {
                float10 = Convert.ToSingle(value);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Int1
        {
            get => int1.ToString("X");
            set
            {
                int1 = Convert.ToInt32(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Int2
        {
            get => int2.ToString("X");
            set
            {
                int2 = Convert.ToInt32(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Int3
        {
            get => int3.ToString("X");
            set
            {
                int3 = Convert.ToInt32(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Int4
        {
            get => int4.ToString("X");
            set
            {
                int4 = Convert.ToInt32(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Int5
        {
            get => int5.ToString("X");
            set
            {
                int5 = Convert.ToInt32(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Int6
        {
            get => int6.ToString("X");
            set
            {
                int6 = Convert.ToInt32(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Int7
        {
            get => int7.ToString("X");
            set
            {
                int7 = Convert.ToInt32(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Byte1
        {
            get => byte1.ToString("X");
            set
            {
                byte1 = Convert.ToByte(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Byte2
        {
            get => byte2.ToString("X");
            set
            {
                byte2 = Convert.ToByte(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Byte3
        {
            get => byte3.ToString("X");
            set
            {
                byte3 = Convert.ToByte(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Byte4
        {
            get => byte4.ToString("X");
            set
            {
                byte4 = Convert.ToByte(value, 16);
                SignalPropertyChange();
            }
        }

        #endregion

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "DefBlock";
            }

            #region Fields Initializing

            float1 = Header->_float1;
            float2 = Header->_float2;
            float3 = Header->_float3;
            float4 = Header->_float4;
            float5 = Header->_float5;
            float6 = Header->_float6;
            float7 = Header->_float7;
            float8 = Header->_float8;
            float9 = Header->_float9;
            float10 = Header->_float10;
            short1 = Header->_short1;
            short2 = Header->_short2;
            short3 = Header->_short3;
            short4 = Header->_short4;
            short5 = Header->_short5;
            short6 = Header->_short6;
            short7 = Header->_short7;
            short8 = Header->_short8;
            short9 = Header->_short9;
            short10 = Header->_short10;
            short11 = Header->_short11;
            short12 = Header->_short12;
            int1 = Header->_int1;
            int2 = Header->_int2;
            int3 = Header->_int3;
            int4 = Header->_int4;
            int5 = Header->_int5;
            int6 = Header->_int6;
            int7 = Header->_int7;
            byte1 = Header->_byte1;
            byte2 = Header->_byte2;
            byte3 = Header->_byte3;
            byte4 = Header->_byte4;

            #endregion

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            AIPDDefBlock* header = (AIPDDefBlock*) address;

            #region Rebuilding

            header->_float1 = float1;
            header->_float2 = float2;
            header->_float3 = float3;
            header->_float4 = float4;
            header->_float5 = float5;
            header->_float6 = float6;
            header->_float7 = float7;
            header->_float8 = float8;
            header->_float9 = float9;
            header->_float10 = float10;
            header->_short1 = short1;
            header->_short2 = short2;
            header->_short3 = short3;
            header->_short4 = short4;
            header->_short5 = short5;
            header->_short6 = short6;
            header->_short7 = short7;
            header->_short8 = short8;
            header->_short9 = short9;
            header->_short10 = short10;
            header->_short11 = short11;
            header->_short12 = short12;
            header->_int1 = int1;
            header->_int2 = int2;
            header->_int3 = int3;
            header->_int4 = int4;
            header->_int5 = int5;
            header->_int6 = int6;
            header->_int7 = int7;
            header->_byte1 = byte1;
            header->_byte2 = byte2;
            header->_byte3 = byte3;
            header->_byte4 = byte4;

            #endregion
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x60;
        }
    }

    public unsafe class AIPDSubBlockNode : ResourceNode
    {
        internal AIPDSubBlock* Header => (AIPDSubBlock*) WorkingUncompressed.Address;

        #region Fields

        internal short short1,
            short2,
            short3,
            short4,
            short5,
            short6,
            short7,
            short8,
            short9,
            short10,
            short11,
            short12;

        internal float float1, float2;
        internal byte byte1, byte2, byte3, byte4;
        internal int int1, int2, int3;

        #endregion

        #region Properties

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short1
        {
            get => short1.ToString("X");
            set
            {
                short1 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short2
        {
            get => short2.ToString("X");
            set
            {
                short2 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short3
        {
            get => short3.ToString("X");
            set
            {
                short3 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short4
        {
            get => short4.ToString("X");
            set
            {
                short4 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Float1
        {
            get => float1.ToString();
            set => float1 = Convert.ToSingle(value);
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Float2
        {
            get => float2.ToString();
            set => float2 = Convert.ToSingle(value);
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short5
        {
            get => short5.ToString("X");
            set
            {
                short5 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short6
        {
            get => short6.ToString("X");
            set
            {
                short6 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short7
        {
            get => short7.ToString("X");
            set
            {
                short7 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short8
        {
            get => short8.ToString("X");
            set
            {
                short8 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short9
        {
            get => short9.ToString("X");
            set
            {
                short9 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short10
        {
            get => short10.ToString("X");
            set
            {
                short10 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Byte1
        {
            get => byte1.ToString("X");
            set
            {
                byte1 = Convert.ToByte(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Byte2
        {
            get => byte2.ToString("X");
            set
            {
                byte2 = Convert.ToByte(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Byte3
        {
            get => byte3.ToString("X");
            set
            {
                byte3 = Convert.ToByte(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Byte4
        {
            get => byte4.ToString("X");
            set
            {
                byte4 = Convert.ToByte(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short11
        {
            get => short11.ToString("X");
            set
            {
                short11 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Short12
        {
            get => short12.ToString("X");
            set
            {
                short12 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Int1
        {
            get => int1.ToString("X");
            set
            {
                int1 = Convert.ToInt32(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Int2
        {
            get => int2.ToString("X");
            set
            {
                int2 = Convert.ToInt32(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("AIPD Attribute")]
        [Description("No Description yet.")]
        public string Int3
        {
            get => int3.ToString("X");
            set
            {
                int3 = Convert.ToInt32(value, 16);
                SignalPropertyChange();
            }
        }

        #endregion

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "SubBlock";
            }

            #region File Initializing

            short1 = Header->_short1;
            short2 = Header->_short2;
            short3 = Header->_short3;
            short4 = Header->_short4;
            short5 = Header->_short5;
            short6 = Header->_short6;
            short7 = Header->_short7;
            short8 = Header->_short8;
            short9 = Header->_short9;
            short10 = Header->_short10;
            short11 = Header->_short11;
            short12 = Header->_short12;
            float1 = Header->_float1;
            float2 = Header->_float2;
            byte1 = Header->_byte1;
            byte2 = Header->_byte2;
            byte3 = Header->_byte3;
            byte4 = Header->_byte4;
            int1 = Header->_int1;
            int2 = Header->_int2;
            int3 = Header->_int3;

            #endregion

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            AIPDSubBlock* header = (AIPDSubBlock*) address;

            #region Rebuilding

            header->_short1 = short1;
            header->_short2 = short2;
            header->_short3 = short3;
            header->_short4 = short4;
            header->_short5 = short5;
            header->_short6 = short6;
            header->_short7 = short7;
            header->_short8 = short8;
            header->_short9 = short9;
            header->_short10 = short10;
            header->_short11 = short11;
            header->_short12 = short12;
            header->_float1 = float1;
            header->_float2 = float2;
            header->_byte1 = byte1;
            header->_byte2 = byte2;
            header->_byte3 = byte3;
            header->_byte4 = byte4;
            header->_int1 = int1;
            header->_int2 = int2;
            header->_int3 = int3;

            #endregion
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x30;
        }
    }

    public unsafe class AIPDUnkBlockNode : ResourceNode
    {
        internal AIPDUnkBlock* Header => (AIPDUnkBlock*) WorkingUncompressed.Address;
        internal byte[] padding = new byte[AIPDUnkBlock.numEntries];

        [Category("Padding")] public byte[] Padding => padding;

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "UnkBlock";
            }

            padding = Header->Padding;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            for (int i = 0; i < padding.Length; i++)
            {
                ((byte*) address)[i] = padding[i];
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x40;
        }
    }

    public unsafe class AIPDType1OffsetsNode : ResourceNode
    {
        internal AIPDType1Offsets* Header => (AIPDType1Offsets*) WorkingUncompressed.Address;
        internal List<VoidPtr> Entries = new List<VoidPtr>();

        [Browsable(false)] public int Type2OffsetSize { get; set; }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Type1";
            }

            Entries = Header->Entries;
            return true;
        }

        public override void OnPopulate()
        {
            foreach (VoidPtr ptr in Entries)
            {
                new AIPDType1Node().Initialize(this, new DataSource(ptr, 0x0));
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            bint* Offsets = (bint*) address;
            int offset = Type2OffsetSize + 0x70 + 0x170;
            VoidPtr entry = address + offset - 0x170;
            int childSize = 0;
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].Name != "NULL")
                {
                    Offsets[i] = offset; //set offset
                }

                childSize = Children[i].CalculateSize(true); //calculate entry size
                Children[i].Rebuild(entry, childSize, true); //rebuild
                entry += childSize;
                offset += childSize; //add childsize to entry address, offset 
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x70;
        }

        public int CalculateChildrenSize()
        {
            int size = 0;
            foreach (AIPDType1Node n in Children)
            {
                size += n.CalculateSize(true);
            }

            return size;
        }
    }

    public unsafe class AIPDType1Node : ResourceNode
    {
        internal AIPDType1* Header => (AIPDType1*) WorkingUncompressed.Address;
        internal List<VoidPtr> Entries = new List<VoidPtr>();

        public override bool OnInitialize()
        {
            if ((VoidPtr) Header != 0x0)
            {
                if (_name == null)
                {
                    _name = "Entry" + Parent.Children.IndexOf(this);
                }

                Entries = Header->Entries;
                return true;
            }

            if (_name == null)
            {
                _name = "NULL";
            }

            return false;
        }

        public override void OnPopulate()
        {
            foreach (VoidPtr ptr in Entries)
            {
                new AIPDType1EntryNode().Initialize(this, new DataSource(ptr, 0x3));
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            foreach (AIPDType1EntryNode n in Children)
            {
                n.Rebuild(address, 0x3, true);
                address += 0x3;
            }

            if (_name != "NULL")
            {
                *(byte*) address = 0x0;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = 0;
            foreach (AIPDType1EntryNode n in Children)
            {
                size += n.CalculateSize(true);
            }

            if (_name != "NULL")
            {
                return size + 0x1; //0x1 is size of 00 of each entry's last
            }

            return 0x0;
        }
    }

    public unsafe class AIPDType1EntryNode : ResourceNode
    {
        internal AIPDType1Entry* Header => (AIPDType1Entry*) WorkingUncompressed.Address;
        internal byte command, control1, control2;

        [Category("Attributes")]
        [Description("No description yet.")]
        public string Command
        {
            get => command.ToString("X");
            set
            {
                command = Convert.ToByte(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("Attributes")]
        [Description("No description yet.")]
        public string Control1
        {
            get => control1.ToString("X");
            set
            {
                control1 = Convert.ToByte(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("Attributes")]
        [Description("No description yet.")]
        public string Control2
        {
            get => control2.ToString("X");
            set
            {
                control2 = Convert.ToByte(value, 16);
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "0x" + Header->_command.ToString("X");
            }

            command = Header->_command;
            control1 = Header->_control1;
            control2 = Header->_control2;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            AIPDType1Entry* header = (AIPDType1Entry*) address;
            header->_command = command; //test code
            header->_control1 = control1;
            header->_control2 = control2;
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x3;
        }
    }

    public unsafe class AIPDType2OffsetsNode : ResourceNode
    {
        internal AIPDType2Offsets* Header => (AIPDType2Offsets*) WorkingUncompressed.Address;
        public List<VoidPtr> Entries = new List<VoidPtr>();
        [Browsable(false)] public int Type1EntrySize { get; set; }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Type2";
            }

            Entries = Header->Entries;
            return true;
        }

        public override void OnPopulate()
        {
            foreach (VoidPtr ptr in Entries)
            {
                new AIPDType2Node().Initialize(this, new DataSource(ptr, 0x0));
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            bint* Offsets = (bint*) address;
            int offset = Type1EntrySize + Children.Count * 0x4 + 0x1E0;
            VoidPtr entry = address + offset - 0x1E0;
            int childSize = 0;
            for (int i = 0; i < Entries.Count; i++)
            {
                Offsets[i] = offset;
                childSize = Children[i].CalculateSize(true); //calculate entry size
                Children[i].Rebuild(entry, childSize, true); //rebuild
                entry += childSize;
                offset += childSize; //add childsize to entry address, offset
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = 0x0;
            size += Children.Count * 0x4; //Offsets size
            return size;
        }

        public int CalculateChildrenSize()
        {
            int size = 0x0;
            foreach (AIPDType2Node n in Children)
            {
                size += n.CalculateSize(true);
            }

            return size;
        }
    }

    public unsafe class AIPDType2Node : ResourceNode
    {
        internal AIPDType2* Header => (AIPDType2*) WorkingUncompressed.Address;
        internal short id;
        internal byte flag, numEntries;
        internal List<VoidPtr> Entries = new List<VoidPtr>();

        [Category("Type2")]
        [Description("Entry ID")]
        public string ID
        {
            get => "0x" + ((int) id).ToString("X");
            set
            {
                id = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("Type2")]
        [Description("Entry flag")]
        public string Flag
        {
            get => flag.ToString("X");
            set
            {
                flag = Convert.ToByte(value, 16);
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            id = Header->_id;
            flag = Header->_flag;
            numEntries = Header->_numEntries;
            Entries = Header->Entries;
            if (_name == null)
            {
                _name = "0x" + ((short) Header->_id).ToString("X");
            }

            return true;
        }

        public override void OnPopulate()
        {
            foreach (VoidPtr ptr in Entries)
            {
                new AIPDType2EntryNode().Initialize(this, new DataSource(ptr, 0x0));
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            AIPDType2* header = (AIPDType2*) address;
            header->_flag = flag;
            header->_id = id;
            header->_numEntries = (byte) Entries.Count;
            AIPDType2Entry* entry = (AIPDType2Entry*) (address + 0x4);
            foreach (AIPDType2EntryNode n in Children)
            {
                n.Rebuild(entry, 0x6, true);
                entry++;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = 0x4; //id and flag, numEntries
            size += Entries.Count * 0x6;
            return size;
        }
    }

    public unsafe class AIPDType2EntryNode : ResourceNode
    {
        internal AIPDType2Entry* Header => (AIPDType2Entry*) WorkingUncompressed.Address;
        internal byte unk1, unk2, unk3, unk4;
        internal short unk5;

        [Category("Type2 Entry")]
        [Description("No description yet")]
        public string Unknown1
        {
            get => unk1.ToString("X");
            set
            {
                unk1 = Convert.ToByte(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("Type2 Entry")]
        [Description("No description yet")]
        public string Unknown2
        {
            get => unk2.ToString("X");
            set
            {
                unk2 = Convert.ToByte(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("Type2 Entry")]
        [Description("No description yet")]
        public string Unknown3
        {
            get => unk3.ToString("X");
            set
            {
                unk3 = Convert.ToByte(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("Type2 Entry")]
        [Description("No description yet")]
        public string Unknown4
        {
            get => unk4.ToString("X");
            set
            {
                unk4 = Convert.ToByte(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("Type2 Entry")]
        [Description("No description yet")]
        public string Unknown5
        {
            get => unk5.ToString("X");
            set
            {
                unk5 = Convert.ToInt16(value, 16);
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Type2Entry " + Parent.Children.IndexOf(this);
            }

            unk1 = Header->_unk1;
            unk2 = Header->_unk2;
            unk3 = Header->_unk3;
            unk4 = Header->_unk4;
            unk5 = Header->_unk5;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            AIPDType2Entry* header = (AIPDType2Entry*) address;
            header->_unk1 = unk1;
            header->_unk2 = unk2;
            header->_unk3 = unk3;
            header->_unk4 = unk4;
            header->_unk5 = unk5;
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x6;
        }
    }
}