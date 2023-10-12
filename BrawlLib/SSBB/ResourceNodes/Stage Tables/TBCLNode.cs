using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Stage_Tables;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBCLNode : TBNode
    {
        public override ResourceType ResourceFileType => ResourceType.TBCL;
        public override Type SubEntryType => typeof(TBCLEntryNode);

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBCL" ? new TBCLNode() : null;
        }
    }

    public unsafe class TBCLEntryNode : ResourceNode
    {
        public TBCLEntry Data = new TBCLEntry
        {
            _unknown0x04 = -1,
            _unknown0x08 = -1,
            _unknown0x0C = -1,
            _unknown0x10 = -1,
            _unknown0x14 = -1,
            _unknown0x18 = -1,
            _unknown0x1C = -1,
            _unknown0x20 = -1,
            _unknown0x24 = -1,
            _unknown0x28 = -1,
            _unknown0x2C = -1,
            _unknown0x30 = -1,
            _unknown0x34 = -1,
            _unknown0x38 = -1
        };

        [Category("TBCL Entry")]
        public byte Count
        {
            get => Data._count;
            set
            {
                Data._count = value;
                SignalPropertyChange();
            }
        }
        
        [Category("TBCL Entry")]
        public float Unknown0x04
        {
            get => Data._unknown0x04;
            set
            {
                Data._unknown0x04 = value;
                SignalPropertyChange();
            }
        }

        [Category("TBCL Entry")]
        public float Unknown0x08
        {
            get => Data._unknown0x08;
            set
            {
                Data._unknown0x08 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("TBCL Entry")]
        public float Unknown0x0C
        {
            get => Data._unknown0x0C;
            set
            {
                Data._unknown0x0C = value;
                SignalPropertyChange();
            }
        }
        
        [Category("TBCL Entry")]
        public float Unknown0x10
        {
            get => Data._unknown0x10;
            set
            {
                Data._unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        [Category("TBCL Entry")]
        public float Unknown0x14
        {
            get => Data._unknown0x14;
            set
            {
                Data._unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        [Category("TBCL Entry")]
        public float Unknown0x18
        {
            get => Data._unknown0x18;
            set
            {
                Data._unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        [Category("TBCL Entry")]
        public float Unknown0x1C
        {
            get => Data._unknown0x1C;
            set
            {
                Data._unknown0x1C = value;
                SignalPropertyChange();
            }
        }

        [Category("TBCL Entry")]
        public float Unknown0x20
        {
            get => Data._unknown0x20;
            set
            {
                Data._unknown0x20 = value;
                SignalPropertyChange();
            }
        }

        [Category("TBCL Entry")]
        public float Unknown0x24
        {
            get => Data._unknown0x24;
            set
            {
                Data._unknown0x24 = value;
                SignalPropertyChange();
            }
        }

        [Category("TBCL Entry")]
        public float Unknown0x28
        {
            get => Data._unknown0x28;
            set
            {
                Data._unknown0x28 = value;
                SignalPropertyChange();
            }
        }

        [Category("TBCL Entry")]
        public float Unknown0x2C
        {
            get => Data._unknown0x2C;
            set
            {
                Data._unknown0x2C = value;
                SignalPropertyChange();
            }
        }

        [Category("TBCL Entry")]
        public float Unknown0x30
        {
            get => Data._unknown0x30;
            set
            {
                Data._unknown0x30 = value;
                SignalPropertyChange();
            }
        }

        [Category("TBCL Entry")]
        public float Unknown0x34
        {
            get => Data._unknown0x34;
            set
            {
                Data._unknown0x34 = value;
                SignalPropertyChange();
            }
        }

        [Category("TBCL Entry")]
        public float Unknown0x38
        {
            get => Data._unknown0x38;
            set
            {
                Data._unknown0x38 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x01
        {
            get => Data._unknown0x01;
            set
            {
                Data._unknown0x01 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x02
        {
            get => Data._unknown0x02;
            set
            {
                Data._unknown0x02 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x03
        {
            get => Data._unknown0x03;
            set
            {
                Data._unknown0x03 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return TBCLEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(TBCLEntry*) WorkingUncompressed.Address;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(TBCLEntry*) address = Data;
        }
    }
}