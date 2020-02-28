using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.ComponentModel;
using System.Runtime.ExceptionServices;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefLookupNode : MoveDefEntryNode
    {
        internal bint* First => (bint*) WorkingUncompressed.Address;
        private int Count;

        public MoveDefLookupNode(int count)
        {
            Count = count;
        }

        public override bool OnInitialize()
        {
            _name = "Lookup Entries";
            Populate();
            return Count > 0;
        }

        public override void OnPopulate()
        {
            MoveDefLookupOffsetNode o;
            bint* addr = First;
            VoidPtr current = BaseAddress + *addr++;
            VoidPtr next = BaseAddress + *addr++;
            int size = 0;
            for (int i = 1; i < Count; i++)
            {
                size = (int) next - (int) current;
                (o = new MoveDefLookupOffsetNode()).Initialize(this, current, size);
                if (Root._lookupSizes.ContainsKey(o.DataOffset))
                {
                    if (Root._lookupSizes[o.DataOffset].DataSize < o.DataSize)
                    {
                        Root._lookupSizes[o.DataOffset] = o;
                    }
                }
                else
                {
                    Root._lookupSizes.Add(o.DataOffset, o);
                }

                current = next;
                next = BaseAddress + *addr++;
            }

            size = (int) _offset - (int) (current - BaseAddress);
            (o = new MoveDefLookupOffsetNode()).Initialize(this, current, size);

            if (!Root._lookupSizes.ContainsKey(o.DataOffset))
            {
                Root._lookupSizes.Add(o.DataOffset, o);
            }

            //Sorting by data offset will allow us to get the exact size of every entry!
            Children.Sort(MoveDefLookupOffsetNode.LookupCompare);
        }
    }

    public unsafe class MoveDefLookupOffsetNode : MoveDefEntryNode
    {
        internal FDefLookupOffset* Header => (FDefLookupOffset*) WorkingUncompressed.Address;

        [Category("MoveDef Lookup Node")] public int DataOffset => Header->_offset;

        [Category("MoveDef Lookup Node")]
        public int DataSize => Index == Parent.Children.Count - 1
            ? Root.sections.dataOffset - DataOffset
            : ((MoveDefLookupOffsetNode) Parent.Children[Index + 1]).DataOffset - DataOffset;

        public static int LookupCompare(ResourceNode n1, ResourceNode n2)
        {
            if (((MoveDefLookupOffsetNode) n1).DataOffset < ((MoveDefLookupOffsetNode) n2).DataOffset)
            {
                return -1;
            }

            if (((MoveDefLookupOffsetNode) n1).DataOffset > ((MoveDefLookupOffsetNode) n2).DataOffset)
            {
                return 1;
            }

            return 0;
        }

        public bool remove = false;

        [HandleProcessCorruptedStateExceptions]
        public override bool OnInitialize()
        {
            //ResourceNode n;
            //if ((n = Root.FindNode(DataOffset)) != null)
            //    _name = n.Name;
            //else
            _name = "Lookup" + Index;
            int val;
            if ((val = *(bint*) Header) == -1)
            {
                MoveDefExternalNode ext = Root.IsExternal(_offset);
                if (ext != null)
                {
                    _name = ext.Name;
                }

                return false;
            }

            return false;
        }
    }
}