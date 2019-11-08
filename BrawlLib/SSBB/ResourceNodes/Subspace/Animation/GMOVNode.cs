using BrawlLib.SSBB.Types.Subspace.Animation;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GMOVNode : ResourceNode
    {
        internal GMOV* Header => (GMOV*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.GMOV;

        [Category("GMOV")]
        [DisplayName("Entries")]
        public int count => Header->_count;

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source;
                if (i == Header->_count - 1)
                {
                    source = new DataSource((*Header)[i],
                        WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]);
                }
                else
                {
                    source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]);
                }

                new GMOVEntryNode().Initialize(this, source);
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
            {
                _name = "Movable Grounds";
            }

            return Header->_count > 0;
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((GMOV*) source.Address)->_tag == GMOV.Tag ? new GMOVNode() : null;
        }
    }

    public unsafe class GMOVEntryNode : ResourceNode
    {
        internal GMOVEntry* Header => (GMOVEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        [Category("Movable Ground")]
        [DisplayName("Model Index")]
        public int MID => *(byte*) (WorkingUncompressed.Address + 0x44);

        [Category("Movable Ground")]
        [DisplayName("Collision Index")]
        public int CID
        {
            get
            {
                int CID = *(byte*) (WorkingUncompressed.Address + 0x45);
                if (CID == 0xFF)
                {
                    return -1;
                }
                else
                {
                    return CID;
                }
            }
        }

        [Category("Movable Ground")]
        [DisplayName("Path Index")]
        public int PID => *(byte*) (WorkingUncompressed.Address + 0x06);

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
            {
                _name = "Object[" + Index + ']';
            }

            return false;
        }
    }
}