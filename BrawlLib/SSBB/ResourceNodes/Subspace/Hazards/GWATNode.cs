using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Hazards;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GWATNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GWATEntryNode);
        public override ResourceType ResourceFileType => ResourceType.GWAT;
        protected override string baseName => "Swimmable Water";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GWAT" ? new GWATNode() : null;
        }
    }

    public unsafe class GWATEntryNode : ResourceNode
    {
        internal GWATEntry Data;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

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

        [Category("Swimmable Water")]
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

        [Category("Swimmable Water")]
        public float Depth
        {
            get => Data._depth;
            set
            {
                Data._depth = value;
                SignalPropertyChange();
            }
        }

        [Category("Swimmable Water")]
        public float Width
        {
            get => Data._width;
            set
            {
                Data._width = value;
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

        [Category("Swimmable Water")]
        public bool CanDrown
        {
            get => Data._canDrown;
            set
            {
                Data._canDrown = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x2D
        {
            get => Data._unknown0x2D;
            set
            {
                Data._unknown0x2D = value;
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

        [Category("Swimmable Water")]
        public float CurrentSpeed
        {
            get => Data._currentSpeed;
            set
            {
                Data._currentSpeed = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x34
        {
            get => Data._unknown0x34;
            set
            {
                Data._unknown0x34 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            Data = *(GWATEntry*)WorkingUncompressed.Address;
            if (_name == null)
            {
                _name = "Water [" + Index + "]";
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return GWATEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GWATEntry* hdr = (GWATEntry*) address;
            *hdr = Data;
        }
    }
}