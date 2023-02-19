using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes.Subspace.Triggers
{
    public class GEPTNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GEPTEntryNode);

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GEPT" ? new GEPTNode() : null;
        }
    }

    public unsafe class GEPTEntryNode : ResourceNode
    {
        internal GEPTEntry* Header => (GEPTEntry*)WorkingUncompressed.Address;
        public override bool supportsCompression => false;

        public TriggerDataClass _trigger1;

        [Category("GEPT")]
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

        public TriggerDataClass _trigger2;

        [Category("GEPT")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger2
        {
            get => _trigger2;
            set
            {
                _trigger2 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _trigger3;

        [Category("GEPT")]
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

        public TriggerDataClass _trigger4;

        [Category("GEPT")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger4
        {
            get => _trigger4;
            set
            {
                _trigger4 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _trigger5;

        [Category("GEPT")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger5
        {
            get => _trigger5;
            set
            {
                _trigger5 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _trigger6;

        [Category("GEPT")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger6
        {
            get => _trigger6;
            set
            {
                _trigger6 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _trigger7;

        [Category("GEPT")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger7
        {
            get => _trigger7;
            set
            {
                _trigger7 = value;
                SignalPropertyChange();
            }
        }

        public GEPTEntryNode()
        {
            _trigger1 = new TriggerDataClass(this);
            _trigger2 = new TriggerDataClass(this);
            _trigger3 = new TriggerDataClass(this);
            _trigger4 = new TriggerDataClass(this);
            _trigger5 = new TriggerDataClass(this);
            _trigger6 = new TriggerDataClass(this);
            _trigger7 = new TriggerDataClass(this);
        }

        public override int OnCalculateSize(bool force)
        {
            return GEPTEntry.Size;
        }

        public override bool OnInitialize()
        {
            _trigger1 = new TriggerDataClass(this, Header->_trigger1);
            _trigger2 = new TriggerDataClass(this, Header->_trigger2);
            _trigger3 = new TriggerDataClass(this, Header->_trigger3);
            _trigger4 = new TriggerDataClass(this, Header->_trigger4);
            _trigger5 = new TriggerDataClass(this, Header->_trigger5);
            _trigger6 = new TriggerDataClass(this, Header->_trigger6);
            _trigger7 = new TriggerDataClass(this, Header->_trigger7);

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GEPTEntry* hdr = (GEPTEntry*)address;

            hdr->_trigger1 = _trigger1;
            hdr->_trigger2 = _trigger2;
            hdr->_trigger3 = _trigger3;
            hdr->_trigger4 = _trigger4;
            hdr->_trigger5 = _trigger5;
            hdr->_trigger6 = _trigger6;
            hdr->_trigger7 = _trigger7;
        }
    }
}
