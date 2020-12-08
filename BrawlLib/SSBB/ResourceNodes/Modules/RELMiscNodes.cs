using BrawlLib.Internal;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class RELGroupNode : RELEntryNode
    {
        public override ResourceType ResourceFileType => ResourceType.NoEditFolder;
    }

    public class RELEntryNode : ResourceNode
    {
        public override ResourceType ResourceFileType => ResourceType.Unknown;
        internal VoidPtr Data => WorkingUncompressed.Address;

        [Browsable(false)] public uint ModuleID => ((ModuleNode) Root).ID;

        [Browsable(false)] public uint RootOffset => Root != null && Data != 0 ? (uint) Data - (uint) BaseAddress : 0;
        public string FileOffset => "0x" + RootOffset.ToString("X");

        [Browsable(false)] public VoidPtr BaseAddress => Root?.WorkingUncompressed.Address ?? null;

        [Browsable(false)]
        public ResourceNode Root
        {
            get
            {
                ResourceNode n = _parent;
                while (!(n is ModuleNode) && n != null)
                {
                    n = n._parent;
                }

                return n;
            }
        }

        [Browsable(false)]
        public ModuleSectionNode Location
        {
            get
            {
                if (Root is RELNode)
                {
                    foreach (ModuleSectionNode s in (Root as RELNode)._sections)
                    {
                        if (s.RootOffset <= RootOffset && s.RootOffset + s._dataSize > RootOffset)
                        {
                            return s;
                        }
                    }
                }

                return null;
            }
        }
    }
}