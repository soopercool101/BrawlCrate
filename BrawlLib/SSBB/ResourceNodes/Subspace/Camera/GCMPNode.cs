using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Camera;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GCMPNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GCMPEntryNode);
        protected override string baseName => "Camera Path Changers";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GCMP" ? new GCMPNode() : null;
        }
    }

    public unsafe class GCMPEntryNode : ResourceNode
    {
        internal GCMPEntry Data;

        [Category("GCMP")]
        public byte PathDataIndex
        {
            get => Data._pathDataIndex;
            set
            {
                Data._pathDataIndex = value;
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

        private TriggerDataClass _activationTrigger;
        [Category("GCMP")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass ActivationTrigger
        {
            get => _activationTrigger ?? (_activationTrigger = new TriggerDataClass(this));
            set
            {
                _activationTrigger = value;
                SignalPropertyChange();
            }
        }


        public override bool OnInitialize()
        {
            Data = *(GCMPEntry*)WorkingUncompressed.Address;
            if (_name == null)
            {
                _name = "Entry [" + Index + "]";
            }

            _activationTrigger = new TriggerDataClass(this, Data._activationTrigger);

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return GCMPEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            Data._activationTrigger = ActivationTrigger;
            GCMPEntry* hdr = (GCMPEntry*)address;
            *hdr = Data;
        }
    }
}