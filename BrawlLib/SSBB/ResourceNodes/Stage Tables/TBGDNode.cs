using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Stage_Tables;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBGDNode : TBNode
    {
        public override ResourceType ResourceFileType => ResourceType.TBGD;
        public override Type SubEntryType => typeof(TBGDEntryNode);

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBGD" ? new TBGDNode() : null;
        }
    }

    public unsafe class TBGDEntryNode : ResourceNode
    {
        public TBGDEntry Data;

        [Category("TBGD")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte VillagerID
        {
            get => Data._unk0x00;
            set
            {
                Data._unk0x00 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x01
        {
            get => Data._unk0x01;
            set
            {
                Data._unk0x01 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x02
        {
            get => Data._unk0x02;
            set
            {
                Data._unk0x02 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x03
        {
            get => Data._unk0x03;
            set
            {
                Data._unk0x03 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x04
        {
            get => Data._unk0x04;
            set
            {
                Data._unk0x04 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0x08
        {
            get => Data._unk0x08;
            set
            {
                Data._unk0x08 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0x09
        {
            get => Data._unk0x09;
            set
            {
                Data._unk0x09 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0x0A
        {
            get => Data._unk0x0A;
            set
            {
                Data._unk0x0A = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0x0B
        {
            get => Data._unk0x0B;
            set
            {
                Data._unk0x0B = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0x0C
        {
            get => Data._unk0x0C;
            set
            {
                Data._unk0x0C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0x0D
        {
            get => Data._unk0x0D;
            set
            {
                Data._unk0x0D = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0x0E
        {
            get => Data._unk0x0E;
            set
            {
                Data._unk0x0E = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0x0F
        {
            get => Data._unk0x0F;
            set
            {
                Data._unk0x0F = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0x10
        {
            get => Data._unk0x10;
            set
            {
                Data._unk0x10 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0x11
        {
            get => Data._unk0x11;
            set
            {
                Data._unk0x11 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0x12
        {
            get => Data._unk0x12;
            set
            {
                Data._unk0x12 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0x13
        {
            get => Data._unk0x13;
            set
            {
                Data._unk0x13 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return TBGDEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(TBGDEntry*)WorkingUncompressed.Address;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(TBGDEntry*)address = Data;
        }
    }
}