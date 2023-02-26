using BrawlLib.Internal;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Subspace.Animation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GMOTNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GMOTEntryNode);
        public override ResourceType ResourceFileType => ResourceType.GMOT;
        protected override string baseName => "Animated Objects";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GMOT" ? new GMOTNode() : null;
        }
    }

    public unsafe class GMOTEntryNode : ResourceNode, IRenderedLink
    {
        internal GMOTEntry Data;
        internal GMOTEntry* Header => (GMOTEntry*) WorkingUncompressed.Address;
        public override bool supportsCompression => false;
        public override ResourceType ResourceFileType => ResourceType.Unknown;
        
        public List<ResourceNode> RenderTargets
        {
            get
            {
                List<ResourceNode> _targets = new List<ResourceNode>();
                if (Parent?.Parent?.Parent is ARCNode a)
                {
                    if (ModelDataIndex != byte.MaxValue)
                    {
                        ResourceNode model = a.Children.FirstOrDefault(c => c is ARCEntryNode ae && ae.FileType == ARCFileType.ModelData && ae.FileIndex == ModelDataIndex);
                        if (model != null)
                            _targets.Add(model);
                    }
                    if (CollisionDataIndex != byte.MaxValue)
                    {
                        ResourceNode coll = a.Children.FirstOrDefault(c => c is CollisionNode ae && ae.FileType == ARCFileType.MiscData && ae.FileIndex == CollisionDataIndex);
                        if (coll != null)
                            _targets.Add(coll);
                    }
                }
                return _targets;
            }
        }

        [Category("GMOT")]
        public byte ModelDataIndex
        {
            get => Data._modelDataIndex;
            set
            {
                Data._modelDataIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("GMOT")]
        [TypeConverter(typeof(NullableByteConverter))]
        public byte CollisionDataIndex
        {
            get => Data._collisionDataIndex;
            set
            {
                Data._collisionDataIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("GMOT")]
        [DisplayName("Sound Info Index")]
        [TypeConverter(typeof(HexIntConverter))]
        public int SoundInfoIndex
        {
            get => Data._soundInfoIndex;
            set
            {
                Data._soundInfoIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x000  
        {
            get => Data._unknown0x000;
            set
            {
                Data._unknown0x000 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x004
        {
            get => Data._unknown0x004;
            set
            {
                Data._unknown0x004 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x008
        {
            get => Data._unknown0x008;
            set
            {
                Data._unknown0x008 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x00C
        {
            get => Data._unknown0x00C;
            set
            {
                Data._unknown0x00C = value; 
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x010
        {
            get => Data._unknown0x010;
            set
            {
                Data._unknown0x010 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x014
        {
            get => Data._unknown0x014;
            set
            {
                Data._unknown0x014 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x018
        {
            get => Data._unknown0x018;
            set
            {
                Data._unknown0x018 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x01C
        {
            get => Data._unknown0x01C;
            set
            {
                Data._unknown0x01C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x020
        {
            get => Data._unknown0x020;
            set
            {
                Data._unknown0x020 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x024
        {
            get => Data._unknown0x024;
            set
            {
                Data._unknown0x024 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x028
        {
            get => Data._unknown0x028;
            set
            {
                Data._unknown0x028 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x02C
        {
            get => Data._unknown0x02C;
            set
            {
                Data._unknown0x02C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x030
        {
            get => Data._unknown0x030;
            set
            {
                Data._unknown0x030 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x034
        {
            get => Data._unknown0x034;
            set
            {
                Data._unknown0x034 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x038
        {
            get => Data._unknown0x038;
            set
            {
                Data._unknown0x038 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x03E
        {
            get => Data._unknown0x03E;
            set
            {
                Data._unknown0x03E = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x03F
        {
            get => Data._unknown0x03F;
            set
            {
                Data._unknown0x03F = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x040
        {
            get => Data._unknown0x040;
            set
            {
                Data._unknown0x040 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x044
        {
            get => Data._unknown0x044;
            set
            {
                Data._unknown0x044 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x048
        {
            get => Data._unknown0x048;
            set
            {
                Data._unknown0x048 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x04C
        {
            get => Data._unknown0x04C;
            set
            {
                Data._unknown0x04C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x050
        {
            get => Data._unknown0x050;
            set
            {
                Data._unknown0x050 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x054
        {
            get => Data._unknown0x054;
            set
            {
                Data._unknown0x054 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x058
        {
            get => Data._unknown0x058;
            set
            {
                Data._unknown0x058 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x05C
        {
            get => Data._unknown0x05C;
            set
            {
                Data._unknown0x05C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x060
        {
            get => Data._unknown0x060;
            set
            {
                Data._unknown0x060 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x064
        {
            get => Data._unknown0x064;
            set
            {
                Data._unknown0x064 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x068
        {
            get => Data._unknown0x068;
            set
            {
                Data._unknown0x068 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x06C
        {
            get => Data._unknown0x06C;
            set
            {
                Data._unknown0x06C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x070
        {
            get => Data._unknown0x070;
            set
            {
                Data._unknown0x070 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x074
        {
            get => Data._unknown0x074;
            set
            {
                Data._unknown0x074 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x078
        {
            get => Data._unknown0x078;
            set
            {
                Data._unknown0x078 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x07C
        {
            get => Data._unknown0x07C;
            set
            {
                Data._unknown0x07C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x080
        {
            get => Data._unknown0x080;
            set
            {
                Data._unknown0x080 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x084
        {
            get => Data._unknown0x084;
            set
            {
                Data._unknown0x084 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x088
        {
            get => Data._unknown0x088;
            set
            {
                Data._unknown0x088 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x08C
        {
            get => Data._unknown0x08C;
            set
            {
                Data._unknown0x08C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x090
        {
            get => Data._unknown0x090;
            set
            {
                Data._unknown0x090 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x094
        {
            get => Data._unknown0x094;
            set
            {
                Data._unknown0x094 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x098
        {
            get => Data._unknown0x098;
            set
            {
                Data._unknown0x098 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x09C
        {
            get => Data._unknown0x09C;
            set
            {
                Data._unknown0x09C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0A0
        {
            get => Data._unknown0x0A0;
            set
            {
                Data._unknown0x0A0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0A4
        {
            get => Data._unknown0x0A4;
            set
            {
                Data._unknown0x0A4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0A8
        {
            get => Data._unknown0x0A8;
            set
            {
                Data._unknown0x0A8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0AC
        {
            get => Data._unknown0x0AC;
            set
            {
                Data._unknown0x0AC = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0B0
        {
            get => Data._unknown0x0B0;
            set
            {
                Data._unknown0x0B0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0B4
        {
            get => Data._unknown0x0B4;
            set
            {
                Data._unknown0x0B4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0B8
        {
            get => Data._unknown0x0B8;
            set
            {
                Data._unknown0x0B8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0BC
        {
            get => Data._unknown0x0BC;
            set
            {
                Data._unknown0x0BC = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0C0
        {
            get => Data._unknown0x0C0;
            set
            {
                Data._unknown0x0C0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0C4
        {
            get => Data._unknown0x0C4;
            set
            {
                Data._unknown0x0C4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0C8
        {
            get => Data._unknown0x0C8;
            set
            {
                Data._unknown0x0C8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0CC
        {
            get => Data._unknown0x0CC;
            set
            {
                Data._unknown0x0CC = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0D0
        {
            get => Data._unknown0x0D0;
            set
            {
                Data._unknown0x0D0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0D4
        {
            get => Data._unknown0x0D4;
            set
            {
                Data._unknown0x0D4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0D8
        {
            get => Data._unknown0x0D8;
            set
            {
                Data._unknown0x0D8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0DC
        {
            get => Data._unknown0x0DC;
            set
            {
                Data._unknown0x0DC = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0E0
        {
            get => Data._unknown0x0E0;
            set
            {
                Data._unknown0x0E0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0E4
        {
            get => Data._unknown0x0E4;
            set
            {
                Data._unknown0x0E4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0E8
        {
            get => Data._unknown0x0E8;
            set
            {
                Data._unknown0x0E8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0EC
        {
            get => Data._unknown0x0EC;
            set
            {
                Data._unknown0x0EC = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0F0
        {
            get => Data._unknown0x0F0;
            set
            {
                Data._unknown0x0F0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0F4
        {
            get => Data._unknown0x0F4;
            set
            {
                Data._unknown0x0F4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0F8
        {
            get => Data._unknown0x0F8;
            set
            {
                Data._unknown0x0F8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x0FC
        {
            get => Data._unknown0x0FC;
            set
            {
                Data._unknown0x0FC = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x100
        {
            get => Data._unknown0x100;
            set
            {
                Data._unknown0x100 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x104
        {
            get => Data._unknown0x104;
            set
            {
                Data._unknown0x104 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x108
        {
            get => Data._unknown0x108;
            set
            {
                Data._unknown0x108 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x10C
        {
            get => Data._unknown0x10C;
            set
            {
                Data._unknown0x10C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x110
        {
            get => Data._unknown0x110;
            set
            {
                Data._unknown0x110 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x114
        {
            get => Data._unknown0x114;
            set
            {
                Data._unknown0x114 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x11C
        {
            get => Data._unknown0x11C;
            set
            {
                Data._unknown0x11C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x120
        {
            get => Data._unknown0x120;
            set
            {
                Data._unknown0x120 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x124
        {
            get => Data._unknown0x124;
            set
            {
                Data._unknown0x124 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x128
        {
            get => Data._unknown0x128;
            set
            {
                Data._unknown0x128 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x12C
        {
            get => Data._unknown0x12C;
            set
            {
                Data._unknown0x12C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x130
        {
            get => Data._unknown0x130;
            set
            {
                Data._unknown0x130 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x134
        {
            get => Data._unknown0x134;
            set
            {
                Data._unknown0x134 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x138
        {
            get => Data._unknown0x138;
            set
            {
                Data._unknown0x138 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x13C
        {
            get => Data._unknown0x13C;
            set
            {
                Data._unknown0x13C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x140
        {
            get => Data._unknown0x140;
            set
            {
                Data._unknown0x140 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x144
        {
            get => Data._unknown0x144;
            set
            {
                Data._unknown0x144 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x148
        {
            get => Data._unknown0x148;
            set
            {
                Data._unknown0x148 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x14C
        {
            get => Data._unknown0x14C;
            set
            {
                Data._unknown0x14C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x150
        {
            get => Data._unknown0x150;
            set
            {
                Data._unknown0x150 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x154
        {
            get => Data._unknown0x154;
            set
            {
                Data._unknown0x154 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x158
        {
            get => Data._unknown0x158;
            set
            {
                Data._unknown0x158 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x15C
        {
            get => Data._unknown0x15C;
            set
            {
                Data._unknown0x15C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x160
        {
            get => Data._unknown0x160;
            set
            {
                Data._unknown0x160 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x164
        {
            get => Data._unknown0x164;
            set
            {
                Data._unknown0x164 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x168
        {
            get => Data._unknown0x168;
            set
            {
                Data._unknown0x168 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x16C
        {
            get => Data._unknown0x16C;
            set
            {
                Data._unknown0x16C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x170
        {
            get => Data._unknown0x170;
            set
            {
                Data._unknown0x170 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x174
        {
            get => Data._unknown0x174;
            set
            {
                Data._unknown0x174 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x178
        {
            get => Data._unknown0x178;
            set
            {
                Data._unknown0x178 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x17C
        {
            get => Data._unknown0x17C;
            set
            {
                Data._unknown0x17C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x180
        {
            get => Data._unknown0x180;
            set
            {
                Data._unknown0x180 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x184
        {
            get => Data._unknown0x184;
            set
            {
                Data._unknown0x184 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x188
        {
            get => Data._unknown0x188;
            set
            {
                Data._unknown0x188 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x18C
        {
            get => Data._unknown0x18C;
            set
            {
                Data._unknown0x18C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x190
        {
            get => Data._unknown0x190;
            set
            {
                Data._unknown0x190 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x194
        {
            get => Data._unknown0x194;
            set
            {
                Data._unknown0x194 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x198
        {
            get => Data._unknown0x198;
            set
            {
                Data._unknown0x198 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x19C
        {
            get => Data._unknown0x19C;
            set
            {
                Data._unknown0x19C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x1A0
        {
            get => Data._unknown0x1A0;
            set
            {
                Data._unknown0x1A0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x1A4
        {
            get => Data._unknown0x1A4;
            set
            {
                Data._unknown0x1A4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x1A8
        {
            get => Data._unknown0x1A8;
            set
            {
                Data._unknown0x1A8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x1AC
        {
            get => Data._unknown0x1AC;
            set
            {
                Data._unknown0x1AC = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x1B0
        {
            get => Data._unknown0x1B0;
            set
            {
                Data._unknown0x1B0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x1B4
        {
            get => Data._unknown0x1B4;
            set
            {
                Data._unknown0x1B4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x1B8
        {
            get => Data._unknown0x1B8;
            set
            {
                Data._unknown0x1B8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x1BC
        {
            get => Data._unknown0x1BC;
            set
            {
                Data._unknown0x1BC = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x1C0
        {
            get => Data._unknown0x1C0;
            set
            {
                Data._unknown0x1C0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x1C4
        {
            get => Data._unknown0x1C4;
            set
            {
                Data._unknown0x1C4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x1C8
        {
            get => Data._unknown0x1C8;
            set
            {
                Data._unknown0x1C8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x1CC
        {
            get => Data._unknown0x1CC;
            set
            {
                Data._unknown0x1CC = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x1D0
        {
            get => Data._unknown0x1D0;
            set
            {
                Data._unknown0x1D0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x1D4
        {
            get => Data._unknown0x1D4;
            set
            {
                Data._unknown0x1D4 = value;
                SignalPropertyChange();
            }
        }

        private TriggerDataClass _trigger1;
        [Category("GMOT")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger1
        {
            get => _trigger1;
            set
            {
                _trigger1 = value;
                SignalPropertyChange();
            }
        }

        private TriggerDataClass _trigger2;
        [Category("GMOT")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger2
        {
            get => _trigger2;
            set {
                _trigger2 = value;
                SignalPropertyChange();
            }
        }

        private TriggerDataClass _trigger3;
        [Category("GMOT")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger3
        {
            get => _trigger3;
            set
            {
                _trigger3 = value;
                SignalPropertyChange();
            }
        }

        public GMOTEntryNode()
        {
            Data = new GMOTEntry();
            _trigger1 = new TriggerDataClass(this);
            _trigger2 = new TriggerDataClass(this);
            _trigger3 = new TriggerDataClass(this);
        }
        
        public override int OnCalculateSize(bool force)
        {
            return GMOTEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *Header;
            _trigger1 = new TriggerDataClass(this, Data._trigger1);
            _trigger2 = new TriggerDataClass(this, Data._trigger2);
            _trigger3 = new TriggerDataClass(this, Data._trigger3);
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GMOTEntry* hdr = (GMOTEntry*)address;
            Data._trigger1 = _trigger1;
            Data._trigger2 = _trigger2;
            Data._trigger3 = _trigger3;
            *hdr = Data;
        }
    }
}