using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GSAVNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GSAVEntryNode);
        protected override string baseName => "Save Points";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GSAV" ? new GSAVNode() : null;
        }
    }

    public unsafe class GSAVEntryNode : ResourceNode
    {
        internal GSAVEntry* Header => (GSAVEntry*)WorkingUncompressed.Address;
        internal GSAVEntry Data;
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
                }
                return _targets;
            }
        }

        public override bool supportsCompression => false;

        [Category("Unknown")]
        public uint Unknown0x00
        {
            get => Data._unknown0x00;
            set
            {
                Data._unknown0x00 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x04
        {
            get => Data._unknown0x04;
            set
            {
                Data._unknown0x04 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x08
        {
            get => Data._unknown0x08;
            set
            {
                Data._unknown0x08 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x0C
        {
            get => Data._unknown0x0C;
            set
            {
                Data._unknown0x0C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x10
        {
            get => Data._unknown0x10;
            set
            {
                Data._unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x14
        {
            get => Data._unknown0x14;
            set
            {
                Data._unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x18
        {
            get => Data._unknown0x18;
            set
            {
                Data._unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x1C
        {
            get => Data._unknown0x1C;
            set
            {
                Data._unknown0x1C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x20
        {
            get => Data._unknown0x20;
            set
            {
                Data._unknown0x20 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x24
        {
            get => Data._unknown0x24;
            set
            {
                Data._unknown0x24 = value;
                SignalPropertyChange();
            }
        }

        [Category("GSAV")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 Position
        {
            get => new Vector2(Data._posX, Data._posY);
            set
            {
                Data._posX = value.X;
                Data._posY = value.Y;
                SignalPropertyChange();
            }
        }

        [Category("GSAV")]
        public byte ModelDataIndex
        {
            get => Data._modelDataIndex;
            set
            {
                Data._modelDataIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x31
        {
            get => Data._unknown0x31;
            set
            {
                Data._unknown0x31 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x32
        {
            get => Data._unknown0x32;
            set
            {
                Data._unknown0x32 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x33
        {
            get => Data._unknown0x33;
            set
            {
                Data._unknown0x33 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GSAVEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *Header;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GSAVEntry* hdr = (GSAVEntry*)address;
            *hdr = Data;
        }
    }
}
