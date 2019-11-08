using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Stage_Tables;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class TBCLNode : StageTableNode
    {
        public override ResourceType ResourceFileType => ResourceType.TBCL;
        internal TBCL* Header => (TBCL*) WorkingUncompressed.Address;
        internal override string DocumentationSubDirectory => "TBCL";

        [Category("Stage Data Table")]
        [DisplayName("Version")]
        public override int Unknown0
        {
            get => unk0;
            set
            {
                unk0 = value;
                SignalPropertyChange();
            }
        }

        public TBCLNode()
        {
            unk0 = 1;
            _entryOffset = 0x14;
        }

        public TBCLNode(int numEntries)
        {
            unk0 = 1;
            unk1 = 0;
            unk2 = 0;
            _entryOffset = 0x14;
            while (NumEntries < numEntries)
            {
                EntryList.Add(0);
            }
        }

        public override bool OnInitialize()
        {
            unk0 = Header->_version;
            unk1 = Header->_unk1;
            unk2 = Header->_unk2;
            _entryOffset = Header->_entryOffset;
            for (int i = 0; i < WorkingUncompressed.Length - EntryOffset; i += 4)
            {
                EntryList.Add(((byte*) Header->Entries)[i + 3]);
                EntryList.Add(((byte*) Header->Entries)[i + 2]);
                EntryList.Add(((byte*) Header->Entries)[i + 1]);
                EntryList.Add(((byte*) Header->Entries)[i]);
            }

            return false;
        }

        protected override string GetName()
        {
            return base.GetName("TBCL");
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TBCL* header = (TBCL*) address;
            header->_tag = TBCL.Tag;
            header->_unk1 = unk1;
            header->_unk2 = unk2;
            header->_version = unk0;
            header->_entryOffset = EntryOffset;
            for (int i = 0; i * 4 < EntryList.Count; i++)
            {
                header->Entries[i] = GetFloat(i);
            }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((TBCL*) source.Address)->_tag == TBCL.Tag ? new TBCLNode(0) : null;
        }
    }
}