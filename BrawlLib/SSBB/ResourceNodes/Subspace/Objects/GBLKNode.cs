using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GBLKNode : ResourceNode
    {
        internal GBLK* Header { get { return (GBLK*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.GBLK; } }

        [Category("GBLK")]
        [DisplayName("Entries")]
        public int count { get { return Header->_count; } }
        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source;
                if (i == Header->_count - 1)
                { source = new DataSource((*Header)[i], WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]); }
                else { source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]); }
                new GBLKEntryNode().Initialize(this, source);
            }
        }
        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Breakable Blocks";
            return Header->_count > 0;
        }

        internal static ResourceNode TryParse(DataSource source) { 
            return ((GBLK*)source.Address)->_tag == GBLK.Tag ? new GBLKNode() : null; 
        }
    }

    public unsafe class GBLKEntryNode : ResourceNode
    {
        internal GBLKEntry* Header { get { return (GBLKEntry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        [Category("Block Info")]
        [DisplayName("Hurtbox Size")]
        public bfloat Hurtbox { get { return *(bfloat*)(WorkingUncompressed.Address + 0x18); } }

        [Category("Block Info")]
        [DisplayName("Base Model Index")]
        public int BMID { get { return *(byte*)(WorkingUncompressed.Address + 0x29); } }

        [Category("Block Info")]
        [DisplayName("Position Model Index")]
        public int PMID { get { return *(byte*)(WorkingUncompressed.Address + 0x2C); } }


        [Category("Block Info")]
        [DisplayName("Collision Index")]
        public int CID
        {
            get
            {
                int CID = *(byte*)(WorkingUncompressed.Address + 0x2D);
                if (CID == 0xFF)
                    return -1;
                else
                    return CID;
            }
        }

        [Category("Flags")]
        public byte Flag1
        {get{return *(byte*)(Header + 0x24);}}
        [Category("Flags")]
        public byte Flag2
        {get{return *(byte*)(Header + 0x25);}}
        [Category("Flags")]
        public byte Flag3
        {get{return *(byte*)(Header + 0x26);}}
        [Category("Flags")]
        public byte Flag4
        {get{return *(byte*)(Header + 0x27);}}

        [Category("Misc")]
        public int unk0 { get { return *(byte*)(WorkingUncompressed.Address + 0x28); } }
        [Category("Misc")]
        public bfloat unk1 { get { return *(bfloat*)(WorkingUncompressed.Address + 0x04); } }
        [Category("Misc")]
        public bfloat unk2 { get { return *(bfloat*)(WorkingUncompressed.Address + 0x10); } }
        [Category("Misc")]
        public byte unk3 { get { return *(byte*)(WorkingUncompressed.Address + 0x23); } }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Block Group[" + Index + ']';
            return false;
        }
    }
}