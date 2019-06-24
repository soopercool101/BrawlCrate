using BrawlLib.SSBBTypes;
using System;
using System.Linq;
using System.Windows.Forms;
using Be.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class STDTNode : StageTableNode
    {
        public override ResourceType ResourceFileType => ResourceType.STDT;
        internal STDT* Header => (STDT*) WorkingUncompressed.Address;
        internal override string DocumentationSubDirectory => "STDT";

        public STDTNode()
        {
        }

        public STDTNode(VoidPtr address, int numEntries)
        {
            version = 1;
            unk1 = 0;
            unk2 = 0;
            while (EntryList.Count < numEntries)
            {
                EntryList.Add(0);
            }
        }

        public override bool OnInitialize()
        {
            version = Header->_version;
            unk1 = Header->_unk1;
            unk2 = Header->_unk2;
            _entryOffset = Header->_entryOffset;
            for (int i = 0; i < WorkingUncompressed.Length - EntryOffset; i += 4)
            {
                EntryList.Add(((byte*)Header->Entries)[i + 3]);
                EntryList.Add(((byte*)Header->Entries)[i + 2]);
                EntryList.Add(((byte*)Header->Entries)[i + 1]);
                EntryList.Add(((byte*)Header->Entries)[i]);
            }
            return false;
        }

        protected override string GetName()
        {
            return base.GetName("Stage Trap Data Table");
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            STDT* header = (STDT*) address;
            header->_tag = STDT.Tag;
            header->_unk1 = unk1;
            header->_unk2 = unk2;
            header->_version = version;
            header->_entryOffset = EntryOffset;
            for (int i = 0; i*4 < EntryList.Count; i++)
            {
                header->Entries[i] = GetFloat(i);
            }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((STDT*) source.Address)->_tag == STDT.Tag ? new STDTNode() : null;
        }
    }
}