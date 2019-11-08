using BrawlLib.SSBB.Types.Audio;
using BrawlLib.Wii.Audio;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARFolderNode : ResourceNode
    {
        internal RSARHeader* Header => (RSARHeader*) WorkingSource.Address;
        public override ResourceType ResourceFileType => ResourceType.RSARFolder;

        public override Type[] AllowedChildTypes => new Type[]
        {
            typeof(RSARFolderNode),
            typeof(RSARSoundNode),
            typeof(RSARBankNode),
            typeof(RSARGroupNode),
            typeof(RSARPlayerInfoNode)
        };

        public int _listIndex;

        [Browsable(false)]
        public RSARNode RSARNode
        {
            get
            {
                ResourceNode n = this;
                while ((n = n.Parent) != null && !(n is RSARNode))
                {
                    ;
                }

                return n as RSARNode;
            }
        }

        internal virtual void GetStrings(sbyte* path, int pathLen, RSAREntryList list, ref List<string> unusedFolders)
        {
            int len = _name.Length;
            int i = 0;
            if (len == 0)
            {
                return;
            }

            len += pathLen + (pathLen != 0 ? 1 : 0);

            sbyte* chars = stackalloc sbyte[len];

            if (pathLen > 0)
            {
                while (i < pathLen)
                {
                    chars[i++] = *path++;
                }

                chars[i++] = (sbyte) '_';
            }

            fixed (char* s = _name)
            {
                for (int x = 0; i < len;)
                {
                    chars[i++] = (sbyte) s[x++];
                }
            }

            if (Children.Count == 0)
            {
                unusedFolders.Add(len != 0 ? new string(chars, 0, len).Replace("_", "/") : "");
            }
            else
            {
                foreach (ResourceNode n in Children)
                {
                    if (n is RSARFolderNode)
                    {
                        ((RSARFolderNode) n).GetStrings(chars, len, list, ref unusedFolders);
                    }
                    else
                    {
                        (n as RSAREntryNode)?.GetStrings(chars, len, list);
                    }
                }
            }
        }
    }
}