using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GMOVNode : ResourceNode
    {
        internal GMOV* Header { get { return (GMOV*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.GMOV; } }

        [Category("GMOV")]
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
                new GMOVEntryNode().Initialize(this, source);
            }
        }
        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Movable Grounds";
            return Header->_count > 0;
        }

        internal static ResourceNode TryParse(DataSource source) { return ((GMOV*)source.Address)->_tag == GMOV.Tag ? new GMOVNode() : null; }
    }

    public unsafe class GMOVEntryNode : ResourceNode
    {
        internal GMOVEntry* Header { get { return (GMOVEntry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }
        [Category("Movable Ground")]
        [DisplayName("Model Index")]
        public int MID { get { return *(byte*)(WorkingUncompressed.Address + 0x44); } }
        
        [Category("Movable Ground")]
        [DisplayName("Collision Index")]
        public int CID 
        { 
            get 
            {
            int CID = *(byte*)(WorkingUncompressed.Address + 0x45);
            if (CID == 0xFF) 
                return -1;
            else
                return CID;
            } 
        }
        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Object[" + Index + ']';
            return false;
        }
    }
}