using System.Collections.Generic;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class FileScanNode : ResourceNode
    {
        internal byte* Data => (byte*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public List<ResourceNode> _list;

        public override void OnPopulate()
        {
            foreach (ResourceNode r in _list)
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