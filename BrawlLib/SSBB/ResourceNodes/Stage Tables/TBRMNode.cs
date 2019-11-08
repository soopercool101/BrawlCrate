using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Stage_Tables;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class TBRMNode : StageTableNode
    {
        public override ResourceType ResourceFileType => ResourceType.TBRM;
        internal TBRM* Header => (TBRM*) WorkingUncompressed.Address;
        internal override string DocumentationSubDirectory => "TBRM";
        internal override int EntryOffset => 0x10;

        public TBRMNode()
        {
            unk0 = 1;
            unk1 = 0;
            unk2 = 0;
        }

        public TBRMNode(int numEntries)
        {
            unk0 = 1;
            unk1 = 0;
            unk2 = 0;
            while (NumEntries < numEntries)
            {
                EntryList.Add(0);
            }
        }

        public override bool OnInitialize()
        {
            unk0 = Header->_unk0;
            unk1 = Header->_unk1;
            unk2 = Header->_unk2;
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
            return base.GetName("TBRM");
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TBRM* header = (TBRM*) address;
            header->_tag = TBRM.Tag;
            header->_unk0 = unk0;
            header->_unk1 = unk1;
            header->_unk2 = unk2;
            for (int i = 0; i * 4 < EntryList.Count; i++)
            {
                header->Entries[i] = GetFloat(i);
            }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((TBRM*) source.Address)->_tag == TBRM.Tag ? new TBRMNode(0) : null;
        }
    }
}