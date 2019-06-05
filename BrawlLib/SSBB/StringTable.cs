using System;
using System.Collections;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;

namespace BrawlLib
{
    public unsafe class StringTable : IEnumerable<string>, IEnumerable
    {
        private readonly SortedList<string, VoidPtr> _table = new SortedList<string, VoidPtr>(StringComparer.Ordinal);

        public VoidPtr this[string s]
        {
            get
            {
                if (!string.IsNullOrEmpty(s) && _table.ContainsKey(s)) return _table[s];

                return _table.Values[0];
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _table.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _table.Keys.GetEnumerator();
        }

        public void Add(string s)
        {
            if (!string.IsNullOrEmpty(s) && !_table.ContainsKey(s)) _table.Add(s, 0);
        }

        public int GetTotalSize()
        {
            var len = 0;
            foreach (var s in _table.Keys) len += (s.Length + 5).Align(4);

            return len;
        }

        public void Clear()
        {
            _table.Clear();
        }

        public void WriteTable(VoidPtr address)
        {
            var entry = (BRESString*) address;
            for (var i = 0; i < _table.Count; i++)
            {
                var s = _table.Keys[i];
                _table[s] = entry;
                entry->Value = s;
                entry = entry->Next;
            }
        }
    }
}