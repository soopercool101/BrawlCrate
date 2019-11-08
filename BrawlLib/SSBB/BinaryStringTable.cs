using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.Collections.Generic;

namespace BrawlLib.SSBB
{
    public unsafe class BinaryStringTable
    {
        private readonly List<BinaryStringEntry> _entries = new List<BinaryStringEntry>();
        private readonly BinaryStringEntry _root = new BinaryStringEntry("", 0);

        public List<BinaryStringEntry> Entries => _entries;
        public BinaryStringEntry RootEntry => _root;

        public BinaryStringTable()
        {
            _entries.Add(_root);
        }

        public void Add(string s)
        {
            BinaryStringEntry entry = new BinaryStringEntry(s, _entries.Count);
            _entries.Add(entry);
            Traverse(entry);
        }

        private void Traverse(BinaryStringEntry entry)
        {
            BinaryStringEntry current = _root._left, prev = _root;
            bool isRight = false;

            while (entry._id <= current._id)
            {
                if (entry._id == current._id)
                {
                    entry.GenerateId(current);
                }

                isRight = current.IsRight(entry);

                prev = current;
                current = isRight ? current._right : current._left;

                if (prev._id <= current._id)
                {
                    break;
                }
            }

            if (isRight)
            {
                prev.InsertRight(entry);
            }
            else
            {
                prev.InsertLeft(entry);
            }
        }

        public int GetTotalSize()
        {
            return _entries.Count * 0x10 + 8;
        }

        public void Write(VoidPtr address)
        {
            ResourceGroup* group = (ResourceGroup*) address;
            group->_numEntries = _entries.Count - 1;
            group->_totalSize = _entries.Count * 0x10 + 8;

            ResourceEntry* pEntry = &group->_first;
            foreach (BinaryStringEntry e in _entries)
            {
                *pEntry++ = e.GetEntry();
            }
        }
    }

    public class BinaryStringEntry
    {
        public string _name;
        public int _id, _index;
        public BinaryStringEntry _left, _right;

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
            {
                entry._right = _left;
            }
            else
            {
                entry._left = _left;
            }

            _left = entry;
        }

        public void InsertRight(BinaryStringEntry entry)
        {
            if (entry.IsRight(_right))
            {
                entry._right = _right;
            }
            else
            {
                entry._left = _right;
            }

            _right = entry;
        }

        public int GenerateId(BinaryStringEntry comparison)
        {
            for (int i = _name.Length; i-- > 0;)
            {
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
            {
                if ((b1 & b) != (b2 & b))
                {
                    return i;
                }
            }

            return 0;
        }
    }
}