using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Hazards;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GHPENode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GHPEEntryNode);
        protected override string baseName => "Hitpoint Affecting Areas";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GHPE" ? new GHPENode() : null;
        }
    }

    public unsafe class GHPEEntryNode : ResourceNode
    {
        protected internal GHPEEntry Data;

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

        [DisplayName("Unknown0x18 (float)")]
        [Category("Unknown")]
        public float Unknown0x18
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

        [DisplayName("Unknown0x28 (byte)")]
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

        [DisplayName("Unknown0x29 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x29
        {
            get => Data._unknown0x29;
            set
            {
                Data._unknown0x29 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x2A (byte)")]
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

        [DisplayName("Unknown0x2B (byte)")]
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

        [DisplayName("Unknown0x2C (short)")]
        [Category("Unknown")]
        public short Unknown0x2C
        {
            get => Data._unknown0x2C;
            set
            {
                Data._unknown0x2C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x2E (short)")]
        [Category("Unknown")]
        public short Unknown0x2E
        {
            get => Data._unknown0x2E;
            set
            {
                Data._unknown0x2E = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GHPEEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GHPEEntry*) WorkingUncompressed.Address;

            if (_name == null)
            {
                _name = $"Area [{Index}]";
            }

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GHPEEntry* hdr = (GHPEEntry*)address;
            *hdr = Data;
        }
    }
}