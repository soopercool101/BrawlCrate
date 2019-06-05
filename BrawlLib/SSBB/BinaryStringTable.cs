using System;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;

namespace BrawlLib
{
    public unsafe class BinaryStringTable
    {
        public BinaryStringTable()
        {
            Entries.Add(RootEntry);
        }

        public List<BinaryStringEntry> Entries { get; } = new List<BinaryStringEntry>();

        public BinaryStringEntry RootEntry { get; } = new BinaryStringEntry("", 0);

        public void Add(string s)
        {
            var entry = new BinaryStringEntry(s, Entries.Count);
            Entries.Add(entry);
            Traverse(entry);
        }

        private void Traverse(BinaryStringEntry entry)
        {
            BinaryStringEntry current = RootEntry._left, prev = RootEntry;
            var isRight = false;

            while (entry._id <= current._id)
            {
                if (entry._id == current._id) entry.GenerateId(current);

                isRight = current.IsRight(entry);

                prev = current;
                current = isRight ? current._right : current._left;

                if (prev._id <= current._id) break;
            }

            if (isRight)
                prev.InsertRight(entry);
            else
                prev.InsertLeft(entry);
        }

        public int GetTotalSize()
        {
            return Entries.Count * 0x10 + 8;
        }

        public void Write(VoidPtr address)
        {
            var group = (ResourceGroup*) address;
            group->_numEntries = Entries.Count - 1;
            group->_totalSize = Entries.Count * 0x10 + 8;

            var pEntry = &group->_first;
            foreach (var e in Entries) *pEntry++ = e.GetEntry();
        }
    }

    public class BinaryStringEntry
    {
        public int _id, _index;
        public BinaryStringEntry _left, _right;
        public string _name;

        public BinaryStringEntry(string name, int index)
        {
            _name = name;
            _index = index;
            _left = _right = this;
            _id = name == "" ? -1 : ((name.Length - 1) << 3) | CompareBits(name[name.Length - 1], 0);
        }

        public void InsertLeft(BinaryStringEntry entry)
        {
            if (entry.IsRight(_left))
                entry._right = _left;
            else
                entry._left = _left;

            _left = entry;
        }

        public void InsertRight(BinaryStringEntry entry)
        {
            if (entry.IsRight(_right))
                entry._right = _right;
            else
                entry._left = _right;

            _right = entry;
        }

        public int GenerateId(BinaryStringEntry comparison)
        {
            for (var i = _name.Length; i-- > 0;)
                if (_name[i] != comparison._name[i])
                {
                    _id = (i << 3) | CompareBits(_name[i], comparison._name[i]);
                    if (IsRight(comparison))
                    {
                        _left = this;
                        _right = comparison;
                    }
                    else
                    {
                        _left = comparison;
                        _right = this;
                    }

                    return _id;
                }

            return 0;
        }

        public bool IsRight(BinaryStringEntry entry)
        {
            return _name.Length != entry._name.Length ? false : ((entry._name[_id >> 3] >> (_id & 7)) & 1) != 0;
        }

        public ResourceEntry GetEntry()
        {
            return new ResourceEntry(_id, _left._index, _right._index);
        }

        private static int CompareBits(int b1, int b2)
        {
            for (int i = 8, b = 0x80; i-- != 0; b >>= 1)
                if ((b1 & b) != (b2 & b))
                    return i;

            return 0;
        }
    }
}