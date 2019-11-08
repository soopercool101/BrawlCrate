using BrawlLib.SSBB.Types.Audio;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSEQLabelNode : ResourceNode
    {
        internal LABLEntry* Header => (LABLEntry*) WorkingUncompressed.Address;

        private uint _id;

        [Category("RSEQ Label")] //Matches with RSAR Sound Part2 pack index
        public uint Id
        {
            get => _id;
            set
            {
                _id = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                if (Header->_stringLength > 0)
                {
                    _name = Header->Name;
                }
                else
                {
                    _name = $"Label[{Index}]";
                }
            }

            _id = Header->_id;

            return false;
        }
    }
}