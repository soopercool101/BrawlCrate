using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class FDefSubActionStringTable
    {
        private SortedList<string, VoidPtr> _table = new SortedList<string, VoidPtr>();

        private List<string> _order = new List<string>();

        public void Add(string s)
        {
            if (!string.IsNullOrEmpty(s) && !_table.ContainsKey(s))
            {
                _table.Add(s, 0);
                _order.Add(s);
            }
        }

        public int TotalSize
        {
            get
            {
                int len = 0;
                foreach (string s in _table.Keys)
                {
                    len += (s.Length + 1).Align(4);
                }

                return len;
            }
        }

        public void Clear()
        {
            _table.Clear();
            _order.Clear();
        }

        public VoidPtr this[string s] => _table[s];

        public void WriteTable(VoidPtr address)
        {
            FDefSubActionString* entry = (FDefSubActionString*) address;
            for (int i = 0; i < _table.Count; i++)
            {
                string s = _order[i];
                _table[s] = entry;
                entry->Value = s;
                entry = entry->Next;
            }
        }
    }

    public unsafe class CompactStringTable
    {
        public SortedList<string, VoidPtr> _table = new SortedList<string, VoidPtr>(StringComparer.Ordinal);

        public void Add(string s)
        {
            if (!string.IsNullOrEmpty(s) && !_table.ContainsKey(s))
            {
                _table.Add(s, 0);
            }
        }

        public int TotalSize
        {
            get
            {
                int len = 0;
                foreach (string s in _table.Keys)
                {
                    len += s.Length + 1;
                }

                return len;
            }
        }

        public void Clear()
        {
            _table.Clear();
        }

        public VoidPtr this[string s] => _table[s];

        public void WriteTable(VoidPtr address)
        {
            FDefReferenceString* entry = (FDefReferenceString*) address;
            for (int i = 0; i < _table.Count; i++)
            {
                string s = _table.Keys[i];
                _table[s] = entry;
                entry->Value = s;
                entry = entry->Next;
            }
        }
    }
}