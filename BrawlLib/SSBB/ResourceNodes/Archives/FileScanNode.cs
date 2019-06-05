using System.Collections.Generic;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class FileScanNode : ResourceNode
    {
        public List<ResourceNode> _list;
        internal byte* Data => (byte*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public override void OnPopulate()
        {
            foreach (var r in _list)
            {
                r._parent = this;
                _children.Add(r);
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _name = Path.GetFileNameWithoutExtension(_origPath);
            return true;
        }
    }
}