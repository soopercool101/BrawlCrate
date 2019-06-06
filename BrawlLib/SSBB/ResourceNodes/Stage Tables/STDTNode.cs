using BrawlLib.SSBBTypes;
using System;

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
            entries = new UnsafeBuffer(numEntries * 4);
            if (address == null)
            {
                byte* pOut = (byte*) entries.Address;
                for (int i = 0; i < numEntries * 4; i++)
                {
                    *pOut++ = 0;
                }
            }
            else
            {
                byte* pIn = (byte*) address;
                byte* pOut = (byte*) entries.Address;
                for (int i = 0; i < numEntries * 4; i++)
                {
                    *pOut++ = *pIn++;
                }
            }
        }

        public override bool OnInitialize()
        {
            version = Header->_version;
            unk1 = Header->_unk1;
            unk2 = Header->_unk2;
            EntryOffset = Header->_entryOffset;
            entries = new UnsafeBuffer(WorkingUncompressed.Length - EntryOffset);
            Memory.Move(entries.Address, Header->Entries, (uint) entries.Length);
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
            Memory.Move(header->Entries, entries.Address, (uint) entries.Length);
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((STDT*) source.Address)->_tag == STDT.Tag ? new STDTNode() : null;
        }
    }
}