using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GELENode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GELEEntryNode);
        protected override string baseName => "Elevators";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GELE" ? new GELENode() : null;
        }
    }

    public unsafe class GELEEntryNode : ResourceNode
    {
        protected internal GELEEntry Data;

        [DisplayName("Unknown0x00 (uint)")]
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

        [DisplayName("Unknown0x04 (uint)")]
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

        [DisplayName("Unknown0x08 (uint)")]
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

        [DisplayName("Unknown0x0C (uint)")]
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

        [DisplayName("Unknown0x10 (uint)")]
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

        [DisplayName("Unknown0x14 (uint)")]
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

        [DisplayName("Unknown0x18 (uint)")]
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

        [DisplayName("Unknown0x1C (float)")]
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

        [DisplayName("Unknown0x20 (float)")]
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

        [DisplayName("Unknown0x24 (float)")]
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

        [DisplayName("Unknown0x28 (float)")]
        [Category("Unknown")]
        public float Unknown0x28
        {
            get => Data._unknown0x28;
            set
            {
                Data._unknown0x28 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x2C (float)")]
        [Category("Unknown")]
        public float Unknown0x2C
        {
            get => Data._unknown0x2C;
            set
            {
                Data._unknown0x2C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x30 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x30
        {
            get => Data._unknown0x30;
            set
            {
                Data._unknown0x30 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x31 (byte)")]
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

        [DisplayName("Unknown0x32 (byte)")]
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

        [DisplayName("Unknown0x33 (byte)")]
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

        [DisplayName("Unknown0x34 (uint)")]
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

        [DisplayName("Unknown0x38 (int)")]
        [Category("Unknown")]
        public int Unknown0x38
        {
            get => Data._unknown0x38;
            set
            {
                Data._unknown0x38 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x3C (int)")]
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

        [DisplayName("Unknown0x40 (int)")]
        [Category("Unknown")]
        public int Unknown0x40
        {
            get => Data._unknown0x40;
            set
            {
                Data._unknown0x40 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GELEEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GELEEntry*) WorkingUncompressed.Address;

            if (_name == null)
            {
                _name = $"Elevator [{Index}]";
            }

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GELEEntry* hdr = (GELEEntry*)address;
            *hdr = Data;
        }
    }
}