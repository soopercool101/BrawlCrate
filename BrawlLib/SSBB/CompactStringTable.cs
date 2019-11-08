using BrawlLib.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB
{
    public unsafe class CompactStringTable
    {
        public SortedList<string, VoidPtr> _table = new SortedList<string, VoidPtr>(StringComparer.Ordinal);

        public void Add(params string[] r)
        {
            foreach (string s in r)
            {
                if (!string.IsNullOrEmpty(s) && !_table.ContainsKey(s))
                {
                    _table.Add(s, 0);
                }
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
            CompactStringEntry* entry = (CompactStringEntry*) address;
            for (int i = 0; i < _table.Count; i++)
            {
                string s = _table.Keys[i];
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
                if (value == null)
                {
                    value = "";
                }

                sbyte* ptr = Data;
                value.Write(ptr);
            }
        }

        public CompactStringEntry* Next => (CompactStringEntry*) ((byte*) Address + (Length + 1));

        public CompactStringEntry* End
        {
            get
            {
                CompactStringEntry* p = (CompactStringEntry*) Address;
                while (p->Length != 0)
                {
                    p = p->Next;
                }

                return p;
            }
        }
    }
}