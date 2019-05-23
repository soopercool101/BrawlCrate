using System;
using System.Collections.Generic;
using Ikarus.MovesetFile;

namespace Ikarus.MovesetBuilder
{
    public unsafe class FDefSubActionStringTable
    {
        List<VoidPtr> _addrs = new List<VoidPtr>();
        List<string> _order = new List<string>();

        public void Add(string s)
        {
            if (!String.IsNullOrEmpty(s) && !_order.Contains(s))
            {
                _addrs.Add(0);
                _order.Add(s);
            }
        }

        public int TotalSize
        {
            get
            {
                int len = 0;
                foreach (string s in _order)
                    len += (s.Length + 1).Align(4);
                return len;
            }
        }

        public void Clear() { _addrs.Clear(); _order.Clear(); }

        public VoidPtr this[string s] { get { return _addrs[_order.IndexOf(s)]; } }

        public void WriteTable(VoidPtr address)
        {
            sSubActionString* entry = (sSubActionString*)address;
            for (int i = 0; i < _order.Count; i++)
            {
                string s = _order[i];
                _addrs[i] = entry;
                entry->Value = s;
                entry = entry->Next;
            }
        }
    }
}