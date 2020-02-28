using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GBLKNode : BLOCEntryNode
    {
        protected override Type SubEntryType => typeof(GBLKEntryNode);
        public override ResourceType ResourceFileType => ResourceType.GBLK;
        protected override string baseName => "Breakable Blocks";

        internal static ResourceNode TryParse(DataSource source)
        {
            return source.Tag == "GBLK" ? new GBLKNode() : null;
        }
    }

    public unsafe class GBLKEntryNode : ResourceNode
    {
        internal GBLKEntry* Header => (GBLKEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        [Category("Block Info")]
        [DisplayName("Hurtbox Size")]
        public bfloat Hurtbox => *(bfloat*) (WorkingUncompressed.Address + 0x18);

        [Category("Block Info")]
        [DisplayName("Base Model Index")]
        public int BMID => *(byte*) (WorkingUncompressed.Address + 0x29);

        [Category("Block Info")]
        [DisplayName("Position Model Index")]
        public int PMID => *(byte*) (WorkingUncompressed.Address + 0x2C);


        [Category("Block Info")]
        [DisplayName("Collision Index")]
        public int CID
        {
            get
            {
                int CID = *(byte*) (WorkingUncompressed.Address + 0x2D);
                if (CID == 0xFF)
                {
                    return -1;
                }

                return CID;
            }
        }

        [Category("Flags")] public byte Flag1 => *(byte*) (Header + 0x24);
        [Category("Flags")] public byte Flag2 => *(byte*) (Header + 0x25);
        [Category("Flags")] public byte Flag3 => *(byte*) (Header + 0x26);
        [Category("Flags")] public byte Flag4 => *(byte*) (Header + 0x27);

        [Category("Misc")] public int unk0 => *(byte*) (WorkingUncompressed.Address + 0x28);
        [Category("Misc")] public bfloat unk1 => *(bfloat*) (WorkingUncompressed.Address + 0x04);
        [Category("Misc")] public bfloat unk2 => *(bfloat*) (WorkingUncompressed.Address + 0x10);
        [Category("Misc")] public byte unk3 => *(byte*) (WorkingUncompressed.Address + 0x23);

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
            {
                _name = $"Block Group [{Index}]";
            }

            return false;
        }
    }
}