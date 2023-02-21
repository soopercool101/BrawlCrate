using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes.Subspace.Triggers
{
    public class GEPTNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GEPTEntryNode);
        protected override string baseName => "Plural Triggers";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GEPT" ? new GEPTNode() : null;
        }
    }

    public unsafe class GEPTEntryNode : ResourceNode
    {
        internal GEPTEntry Data;
        public override bool supportsCompression => false;

        public TriggerDataClass _activateTrigger;

        [Category("GEPT")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass ActivateTrigger
        {
            get => _activateTrigger;
            set
            {
                _activateTrigger = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _referenceTrigger1;

        [Category("GEPT")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass ReferenceTrigger1
        {
            get => _referenceTrigger1;
            set
            {
                _referenceTrigger1 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _referenceTrigger2;

        [Category("GEPT")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass ReferenceTrigger2
        {
            get => _referenceTrigger2;
            set
            {
                _referenceTrigger2 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _referenceTrigger3;

        [Category("GEPT")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass ReferenceTrigger3
        {
            get => _referenceTrigger3;
            set
            {
                _referenceTrigger3 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _referenceTrigger4;

        [Category("GEPT")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass ReferenceTrigger4
        {
            get => _referenceTrigger4;
            set
            {
                _referenceTrigger4 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _referenceTrigger5;

        [Category("GEPT")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass ReferenceTrigger5
        {
            get => _referenceTrigger5;
            set
            {
                _referenceTrigger5 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0x18
        {
            get => Data._unknown0x18;
            set
            {
                Data._unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0x19
        {
            get => Data._unknown0x19;
            set
            {
                Data._unknown0x19 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x1A
        {
            get => Data._unknown0x1A;
            set
            {
                Data._unknown0x1A = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x1B
        {
            get => Data._unknown0x1B;
            set
            {
                Data._unknown0x1B = value;
                SignalPropertyChange();
            }
        }

        public GEPTEntryNode()
        {
            _activateTrigger = new TriggerDataClass(this);
            _referenceTrigger1 = new TriggerDataClass(this);
            _referenceTrigger2 = new TriggerDataClass(this);
            _referenceTrigger3 = new TriggerDataClass(this);
            _referenceTrigger4 = new TriggerDataClass(this);
            _referenceTrigger5 = new TriggerDataClass(this);
        }

        public override int OnCalculateSize(bool force)
        {
            return GEPTEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GEPTEntry*)WorkingUncompressed.Address;

            _activateTrigger = new TriggerDataClass(this, Data._activateTrigger);
            _referenceTrigger1 = new TriggerDataClass(this, Data._referenceTrigger1);
            _referenceTrigger2 = new TriggerDataClass(this, Data._referenceTrigger2);
            _referenceTrigger3 = new TriggerDataClass(this, Data._referenceTrigger3);
            _referenceTrigger4 = new TriggerDataClass(this, Data._referenceTrigger4);
            _referenceTrigger5 = new TriggerDataClass(this, Data._referenceTrigger5);

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GEPTEntry* hdr = (GEPTEntry*)address;
            hdr->_activateTrigger = _activateTrigger;
            hdr->_referenceTrigger1 = _referenceTrigger1;
            hdr->_referenceTrigger2 = _referenceTrigger2;
            hdr->_referenceTrigger3 = _referenceTrigger3;
            hdr->_referenceTrigger4 = _referenceTrigger4;
            hdr->_referenceTrigger5 = _referenceTrigger5;
            hdr->_unknown0x18 = Unknown0x18;
            hdr->_unknown0x19 = Unknown0x19;
            hdr->_unknown0x1A = Unknown0x1A;
            hdr->_unknown0x1B = Unknown0x1B;
        }
    }
}
