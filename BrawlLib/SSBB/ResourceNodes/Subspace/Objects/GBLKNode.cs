using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GBLKNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GBLKEntryNode);
        public override ResourceType ResourceFileType => ResourceType.GBLK;
        protected override string baseName => "Breakable Blocks";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GBLK" ? new GBLKNode() : null;
        }
    }

    public unsafe class GBLKEntryNode : ResourceNode
    {
        internal GBLKEntry Data;
        public override ResourceType ResourceFileType => ResourceType.Unknown;
        
        [Category("Unknown")]
        public float Unknown0x00
        {
            get => Data._unknown0x00;
            set
            {
                Data._unknown0x00 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x04
        {
            get => Data._unknown0x04;
            set
            {
                Data._unknown0x04 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x08
        {
            get => Data._unknown0x08;
            set
            {
                Data._unknown0x08 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x0C
        {
            get => Data._unknown0x0C;
            set
            {
                Data._unknown0x0C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x10
        {
            get => Data._unknown0x10;
            set
            {
                Data._unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x14
        {
            get => Data._unknown0x14;
            set
            {
                Data._unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        [Category("GBLK")]
        public float HurtboxSize
        {
            get => Data._hurtboxSize;
            set
            {
                Data._hurtboxSize = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x1C
        {
            get => Data._unknown0x1C;
            set
            {
                Data._unknown0x1C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x1D
        {
            get => Data._unknown0x1D;
            set
            {
                Data._unknown0x1D = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x1E
        {
            get => Data._unknown0x1E;
            set
            {
                Data._unknown0x1E = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x1F
        {
            get => Data._unknown0x1F;
            set
            {
                Data._unknown0x1F = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x20
        {
            get => Data._unknown0x20;
            set
            {
                Data._unknown0x20 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x24
        {
            get => Data._unknown0x24;
            set
            {
                Data._unknown0x24 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x25
        {
            get => Data._unknown0x25;
            set
            {
                Data._unknown0x25 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x26
        {
            get => Data._unknown0x26;
            set
            {
                Data._unknown0x26 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x27
        {
            get => Data._unknown0x27;
            set
            {
                Data._unknown0x27 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x28
        {
            get => Data._unknown0x28;
            set
            {
                Data._unknown0x28 = value;
                SignalPropertyChange();
            }
        }

        [Category("GBLK")]
        public byte BaseModelIndex
        {
            get => Data._baseModelIndex;
            set
            {
                Data._baseModelIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x2A
        {
            get => Data._unknown0x2A;
            set
            {
                Data._unknown0x2A = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x2B
        {
            get => Data._unknown0x2B;
            set
            {
                Data._unknown0x2B = value;
                SignalPropertyChange();
            }
        }

        [Category("GBLK")]
        public byte PositionModelIndex
        {
            get => Data._positionModelIndex;
            set
            {
                Data._positionModelIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("GBLK")]
        public byte CollisionModelIndex
        {
            get => Data._collisionModelIndex;
            set
            {
                Data._collisionModelIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x2E
        {
            get => Data._unknown0x2E;
            set
            {
                Data._unknown0x2E = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x2F
        {
            get => Data._unknown0x2F;
            set
            {
                Data._unknown0x2F = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x30
        {
            get => Data._unknown0x30;
            set
            {
                Data._unknown0x30 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x34
        {
            get => Data._unknown0x34;
            set
            {
                Data._unknown0x34 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public short Unknown0x38
        {
            get => Data._unknown0x38;
            set
            {
                Data._unknown0x38 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public short Unknown0x3A
        {
            get => Data._unknown0x3A;
            set
            {
                Data._unknown0x3A = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0x3C
        {
            get => Data._unknown0x3C;
            set
            {
                Data._unknown0x3C = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            Data = *(GBLKEntry*)WorkingUncompressed.Address;
            if (_name == null)
            {
                _name = $"Block Group [{Index}]";
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return GBLKEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GBLKEntry* hdr = (GBLKEntry*)address;
            *hdr = Data;
        }
    }
}