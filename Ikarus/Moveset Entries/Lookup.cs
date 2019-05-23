using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BrawlLib.SSBBTypes;
using System.Runtime.InteropServices;
using System.Runtime.ExceptionServices;
using Ikarus;

namespace Ikarus.MovesetFile
{
    public unsafe class MoveDefLookupNode : MovesetEntry
    {
        internal bint* First { get { return (bint*)WorkingUncompressed.Address; } }
        int Count = 0;

        public MoveDefLookupNode(int count) { Count = count; }

        public override void Parse(VoidPtr address)
        {
            MoveDefLookupOffsetNode o;
            bint* addr = First;
            VoidPtr current = BaseAddress + *addr++;
            VoidPtr next = BaseAddress + *addr++;
            int size = 0;
            for (int i = 1; i < Count; i++)
            {
                size = (int)next - (int)current;
                (o = new MoveDefLookupOffsetNode()).Initialize(this, current, size);
                if (_root._lookupSizes.ContainsKey(o.DataOffset))
                    if (_root._lookupSizes[o.DataOffset].DataSize < o.DataSize)
                        _root._lookupSizes[o.DataOffset] = o;
                    else { }
                else
                    _root._lookupSizes.Add(o.DataOffset, o);
                current = next;
                next = BaseAddress + *addr++;
            }
            size = ((int)_offset - (int)(current - BaseAddress));
            (o = new MoveDefLookupOffsetNode()).Initialize(this, current, size);

            if (!_root._lookupSizes.ContainsKey(o.DataOffset))
                _root._lookupSizes.Add(o.DataOffset, o);

            //Sorting by data offset will allow us to get the exact size of every entry!
            Children.Sort(MoveDefLookupOffsetNode.LookupCompare);
        }
    }

    public unsafe class MoveDefLookupOffsetNode : MovesetEntry
    {
        internal FDefLookupOffset* Header { get { return (FDefLookupOffset*)WorkingUncompressed.Address; } }

        [Category("MoveDef Lookup Node")]
        public int DataOffset { get { return Header->_offset; } }
        [Category("MoveDef Lookup Node")]
        public int DataSize { get { return Index == Parent.Children.Count - 1 ? _root._sections.dataOffset - DataOffset : ((MoveDefLookupOffsetNode)Parent.Children[Index + 1]).DataOffset - DataOffset; } }

        public static int LookupCompare(ResourceNode n1, ResourceNode n2)
        {
            if (((MoveDefLookupOffsetNode)n1).DataOffset < ((MoveDefLookupOffsetNode)n2).DataOffset)
                return -1;
            if (((MoveDefLookupOffsetNode)n1).DataOffset > ((MoveDefLookupOffsetNode)n2).DataOffset)
                return 1;

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
            if ((val = *(bint*)Header) == -1)
            {
                ReferenceEntry ext = _root.TryGetExternal(_offset);
                if (ext != null)
                    _name = ext.Name;
                return false;
            }
            return false;
        }
    }
}
