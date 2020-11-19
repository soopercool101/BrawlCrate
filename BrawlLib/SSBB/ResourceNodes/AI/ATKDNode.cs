using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ATKDNode : ARCEntryNode
    {
        internal ATKD* Header => (ATKD*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.ATKD;

        [Category("ATKD Property")] public int Entries => Header->_numEntries;

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
            {
                _name = "ATKD " + Parent.Name.Replace("ai_", ""); //Naming this node
            }

            return Header->_numEntries > 0;
        }

        public override void OnPopulate()
        {
            ATKDEntry* entry = Header->entries;
            for (int i = 0; i < Header->_numEntries; i++)
            {
                new ATKDEntryNode().Initialize(this, new DataSource(entry, 0x18));
                entry++;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int header = 0x10;
            int entries = 0;
            for (int i = 0; i < Children.Count; i++)
            {
                entries += (int) ATKDEntry.Size;
            }

            return header + entries;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ATKD* atkd = (ATKD*) address;
            atkd->_numEntries = Header->_numEntries;
            atkd->_tag = Header->_tag;
            atkd->_unk1 = Header->_unk1;
            atkd->_unk2 = Header->_unk2;
            ATKDEntry* entries = (ATKDEntry*) ((VoidPtr) atkd + 0x10);
            foreach (ATKDEntryNode node in Children)
            {
                node.Rebuild(entries, 0x24, true);
                entries++;
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((ATKD*) source.Address)->_tag == ATKD.Tag ? new ATKDNode() : null;
        }
    }

    public unsafe class ATKDEntryNode : ResourceNode
    {
        internal ATKDEntry* Header => (ATKDEntry*) WorkingUncompressed.Address;

        private short SubActID, unk1, unk2, unk3;
        private float xMinRange, xMaxRange, yMinRange, yMaxRange;

        [Category("ATKD Entry")]
        [Description("ID of Sub Action. This is same to PSA's")]
        public SubActionID SubActionName
        {
            get => (SubActionID) SubActID;
            set
            {
                SubActID = (short) value;
                SignalPropertyChange();
            }
        }

        [Category("ATKD Entry")]
        [Description("Always 0")]
        public string Unknown1
        {
            get => "0x" + unk1.ToString("X");
            set
            {
                unk1 = (short) Convert.ToInt32(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("ATKD Entry")]
        [Description("Beginning frame of Danger Box")]
        public string StartFrame
        {
            get => "0x" + unk2.ToString("X");
            set
            {
                unk2 = (short) Convert.ToInt32(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("ATKD Entry")]
        [Description("Ending Frame of Danger Box")]
        public string EndFrame
        {
            get => "0x" + unk3.ToString("X");
            set
            {
                unk3 = (short) Convert.ToInt32(value, 16);
                SignalPropertyChange();
            }
        }

        [Category("ATKD Entry")]
        [Description("Minimum offensive collision range in direction of X")]
        public float XMinRange
        {
            get => xMinRange;
            set
            {
                xMinRange = value;
                SignalPropertyChange();
            }
        }

        [Category("ATKD Entry")]
        [Description("Maximum offensive collision range in direction of X")]
        public float XMaxRange
        {
            get => xMaxRange;
            set
            {
                xMaxRange = value;
                SignalPropertyChange();
            }
        }

        [Category("ATKD Entry")]
        [Description("Minimum offensive collision range in direction of Y")]
        public float YMinRange
        {
            get => yMinRange;
            set
            {
                yMinRange = value;
                SignalPropertyChange();
            }
        }

        [Category("ATKD Entry")]
        [Description("Maximum offensive collision range in direction of Y")]
        public float YMaxRange
        {
            get => yMaxRange;
            set
            {
                yMaxRange = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = ((SubActionID) (short) Header->_SubActID).ToString();
            }

            SubActID = Header->_SubActID;
            unk1 = Header->_unk1;
            unk2 = Header->_StartFrame;
            unk3 = Header->_EndFrame;
            xMaxRange = Header->_xMaxRange;
            xMinRange = Header->_xMinRange;
            yMaxRange = Header->_yMaxRange;
            yMinRange = Header->_yMinRange;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ATKDEntry* atkdEntry = (ATKDEntry*) address;
            atkdEntry->_SubActID = SubActID;
            atkdEntry->_unk1 = unk1;
            atkdEntry->_StartFrame = unk2;
            atkdEntry->_EndFrame = unk3;
            atkdEntry->_xMaxRange = xMaxRange;
            atkdEntry->_xMinRange = xMinRange;
            atkdEntry->_yMaxRange = yMaxRange;
            atkdEntry->_yMinRange = yMinRange;
        }
    }
}