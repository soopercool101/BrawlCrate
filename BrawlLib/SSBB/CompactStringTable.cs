using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BrawlLib
{
    public unsafe class CompactStringTable
    {
        public SortedList<string, VoidPtr> _table = new SortedList<string, VoidPtr>(StringComparer.Ordinal);

        public int TotalSize
        {
            get
            {
                var len = 0;
                foreach (var s in _table.Keys) len += s.Length + 1;

                return len;
            }
        }

        public VoidPtr this[string s] => _table[s];

        public void Add(params string[] r)
        {
            foreach (var s in r)
                if (!string.IsNullOrEmpty(s) && !_table.ContainsKey(s))
                    _table.Add(s, 0);
        }

        public void Clear()
        {
            _table.Clear();
        }

        public void WriteTable(VoidPtr address)
        {
            var entry = (CompactStringEntry*) address;
            for (var i = 0; i < _table.Count; i++)
            {
                var s = _table.Keys[i];
                _table[s] = entry;
                entry->Value = s;
                entry = entry->Next;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CompactStringEntry
    {
        public sbyte _data;

        private void* Address
        {
            get
            {
                fixed (void* p = &this)
                {
                    return p;
                }
            }
        }

        public sbyte* Data => (sbyte*) Address;

        public int Length => Value.Length;

        public string Value
        {
            get => new string(Data);
            set
            {
                if (value == null) value = "";

                var ptr = Data;
                value.Write(ptr);
            }
        }

        public CompactStringEntry* Next => (CompactStringEntry*) ((byte*) Address + (Length + 1));

        public CompactStringEntry* End
        {
            get
            {
                var p = (CompactStringEntry*) Address;
                while (p->Length != 0) p = p->Next;
                return p;
            }
        }
    }
}