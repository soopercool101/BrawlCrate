using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSEQLabelNode : ResourceNode
    {
        internal LABLEntry* Header { get { return (LABLEntry*)WorkingUncompressed.Address; } }

        uint _id;

        [Category("RSEQ Label")] //Matches with RSAR Sound Part2 pack index
        public uint Id { get { return _id; } set { _id = value; SignalPropertyChange(); } } 
        
        public override bool OnInitialize()
        {
            if (_name == null)
                if (Header->_stringLength > 0)
                    _name = Header->Name;
                else
                    _name = string.Format("Label[{0}]", Index);

            _id = Header->_id;

            return false;
        }
    }
}
