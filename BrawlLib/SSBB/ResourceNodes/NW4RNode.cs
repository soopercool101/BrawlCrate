using BrawlLib.SSBB.Types;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class NW4RNode : ResourceNode
    {
        internal NW4RCommonHeader* CommonHeader => (NW4RCommonHeader*) WorkingUncompressed.Address;

        [Category("NW4R Node")] public byte VersionMajor => _major;
        [Category("NW4R Node")] public byte VersionMinor => _minor;
        internal byte _minor, _major;

        internal string _tag;
        internal int _length;
        internal Endian _endian;

        public override bool OnInitialize()
        {
            _major = CommonHeader->VersionMajor;
            _minor = CommonHeader->VersionMinor;
            _tag = CommonHeader->_tag;
            _length = CommonHeader->_length;
            _endian = CommonHeader->Endian;

            return false;
        }
    }

    public unsafe class NW4RArcEntryNode : ARCEntryNode
    {
        internal NW4RCommonHeader* CommonHeader => (NW4RCommonHeader*) WorkingUncompressed.Address;

        [Category("NW4R Node")] public byte VersionMajor => _major;
        [Category("NW4R Node")] public byte VersionMinor => _minor;
        internal byte _minor, _major;

        internal string _tag;
        internal int _length;
        internal Endian _endian;

        public override bool OnInitialize()
        {
            _major = CommonHeader->VersionMajor;
            _minor = CommonHeader->VersionMinor;
            _tag = CommonHeader->_tag;
            _length = CommonHeader->_length;
            _endian = CommonHeader->Endian;

            return false;
        }
    }

    public class NW4REntryNode : ResourceNode
    {
        internal NW4RNode NW4RRootNode
        {
            get
            {
                ResourceNode n = this;
                while ((n = n.Parent) != null && !(n is NW4RNode))
                {
                    ;
                }

                return n as NW4RNode;
            }
        }
    }
}