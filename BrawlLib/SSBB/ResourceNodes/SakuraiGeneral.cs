using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace BrawlLib.SSBB.ResourceNodes
{
    public delegate TableEntryNode SakuraiSectionParser(string name);

    public unsafe class TableEntryNode : SakuraiEntryNode
    {
        public static List<SakuraiSectionParser> _parsers = new List<SakuraiSectionParser>();

        static TableEntryNode()
        {
            AddParsers(Assembly.GetExecutingAssembly());
            AddParsers(Assembly.GetEntryAssembly());
        }

        private static void AddParsers(Assembly e)
        {
            if (e == null)
            {
                return;
            }

            Delegate del;
            foreach (Type t in e.GetTypes())
            {
                if (t.IsSubclassOf(typeof(TableEntryNode)))
                {
                    if ((del = Delegate.CreateDelegate(typeof(SakuraiSectionParser), t, "TestType", false, false)) !=
                        null)
                    {
                        _parsers.Add(del as SakuraiSectionParser);
                    }
                }
            }
        }

        public static TableEntryNode GetRaw(string name)
        {
            TableEntryNode n = null;
            foreach (SakuraiSectionParser d in _parsers)
            {
                if ((n = d(name)) != null)
                {
                    break;
                }
            }

            return n;
        }

        [Browsable(false)]
        public List<int> DataOffsets
        {
            get => _dataOffsets;
            set => _dataOffsets = value;
        }

        private List<int> _dataOffsets = new List<int>();

        [Browsable(false)]
        public List<SakuraiEntryNode> References
        {
            get => _references;
            set => _references = value;
        }

        private List<SakuraiEntryNode> _references = new List<SakuraiEntryNode>();

        protected override void OnParse(VoidPtr address)
        {
            _dataOffsets = new List<int> {_offset};

            int offset = *(bint*) address;

            while (offset > 0)
            {
                _dataOffsets.Add(offset);

                offset = *(bint*) (BaseAddress + offset);

                //Infinite loops are NO GOOD
                if (_dataOffsets.Contains(offset))
                {
                    break;
                }
            }
        }
    }

    public unsafe class RawDataNode2 : TableEntryNode
    {
        internal byte[] _data;

        protected override void OnParse(VoidPtr address)
        {
            DataOffsets.Add(_offset);

            if (_initSize > 0)
            {
                _data = new byte[_initSize];
                byte* b = (byte*) address;
                for (int i = 0; i < _data.Length; i++)
                {
                    _data[i] = b[i];
                }
            }
            else
            {
                _data = new byte[0];
            }
        }

        protected override int OnGetSize()
        {
            return _entryLength = _data.Length;
        }

        protected override void OnWrite(VoidPtr address)
        {
            RebuildAddress = address;
            byte* header = (byte*) address;
            if (_data != null)
            {
                for (int i = 0; i < _data.Length; i++)
                {
                    header[i] = _data[i];
                }
            }
        }
    }

    public unsafe class IndexValue : SakuraiEntryNode
    {
        public int _value;

        public static explicit operator int(IndexValue val)
        {
            return val._value;
        }

        public static bool _hexadecimal;

        public bool HexDisplay
        {
            get => _hexadecimal;
            set => _hexadecimal = value;
        }

        private const string _validDec = "0123456789";
        private const string _validHex = "ABCDEFabcdef";

        [Category("Index Entry")]
        public string Value
        {
            get
            {
                if (_hexadecimal)
                {
                    bool neg = false;
                    int val = _value;
                    if (val < 0)
                    {
                        neg = true;
                        val = -val;
                    }

                    return (neg ? "-" : "") + val.ToString("X");
                }

                return _value.ToString();
            }
            set
            {
                if (_hexadecimal)
                {
                    bool neg = false;
                    if (value.StartsWith("-"))
                    {
                        value = value.Substring(1);
                        neg = true;
                    }

                    if (value.StartsWith("0x"))
                    {
                        value = value.Substring(2);
                    }

                    value = value.RemoveInvalidCharacters(_validDec + _validHex);
                    _value = int.Parse(value, System.Globalization.NumberStyles.HexNumber);

                    if (neg)
                    {
                        _value = -_value;
                    }
                }
                else
                {
                    value = value.RemoveInvalidCharacters(_validDec);
                    _value = int.Parse(value);
                }

                SignalPropertyChange();
            }
        }

        protected override void OnParse(VoidPtr address)
        {
            _value = *(bint*) address;
        }

        protected override int OnGetSize()
        {
            return 4;
        }

        protected override void OnWrite(VoidPtr address)
        {
            *(bint*) (RebuildAddress = address) = _value;
        }
    }

    public unsafe class OffsetValue : SakuraiEntryNode
    {
        [Category("Offset Entry")] public int DataOffset => _dataOffset;
        private int _dataOffset;

        protected override void OnParse(VoidPtr address)
        {
            _dataOffset = *(bint*) address;
        }
    }

    /// <summary>
    /// Generic list class for handling structs in a memory array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntryList<T> : SakuraiEntryNode, IEnumerable<T>, IListSource where T : SakuraiEntryNode
    {
        #region Child Enumeration

        public IEnumerator<T> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsListCollection => false;

        public IList GetList()
        {
            return _entries;
        }

        public int Count => _entries.Count;

        public T this[int i]
        {
            get
            {
                if (i >= 0 && i < Count)
                {
                    return _entries[i];
                }

                return null;
            }
            set
            {
                if (i >= 0 && i < Count)
                {
                    SignalPropertyChange();
                    _entries[i] = value;
                }
            }
        }

        private void Insert(int i, T e)
        {
            if (i >= 0)
            {
                if (i < Count)
                {
                    _entries.Insert(i, e);
                }
                else
                {
                    _entries.Add(e);
                }

                SignalRebuildChange();
            }
        }

        private void Add(int i, T e)
        {
            _entries.Add(e);
            SignalRebuildChange();
        }

        internal void RemoveAt(int i)
        {
            if (i >= 0 && i < Count)
            {
                _entries.RemoveAt(i);
                SignalRebuildChange();
            }
        }

        private void Clear()
        {
            if (Count != 0)
            {
                _entries.Clear();
                SignalRebuildChange();
            }
        }

        #endregion

        private readonly int _stride, _count = -1;
        private BindingList<T> _entries;

        public BindingList<T> Entries => _entries;

        public EntryList(int stride, int count)
        {
            _stride = stride;
            _count = count;
        }

        public EntryList(int stride)
        {
            _stride = stride;
        }

        protected override void OnParse(VoidPtr address)
        {
            _entries = new BindingList<T>();
            if (_count > 0 || _initSize > 0)
            {
                for (int i = 0; i < (_count > 0 ? _count : _initSize / _stride); i++)
                {
                    T e = Parse<T>(address[i, _stride]);
                    e._index = i;
                    _entries.Add(e);
                }
            }
        }

        protected override int OnGetSize()
        {
            return _entries.Count * _stride;
        }

        protected override void OnWrite(VoidPtr address)
        {
            RebuildAddress = address;
            for (int i = 0; i < _entries.Count; i++)
            {
                _entries[i].Write(address[i, _stride]);
            }
        }

        protected override int OnGetLookupCount()
        {
            int count = 0;
            foreach (T e in _entries)
            {
                count += e.GetLookupCount();
            }

            return count;
        }
    }

    /// <summary>
    /// Deprecated; avoid use
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //public unsafe class EntryListOffset<T> : ListOffset where T : SakuraiEntryNode
    //{
    //    private int _stride;
    //    public List<T> _entries;

    //    public EntryListOffset(int stride) { _stride = stride; }
    //    protected override void OnParse(VoidPtr address)
    //    {
    //        base.OnParse(address);
    //        _entries = new List<T>();
    //        if (Count > 0)
    //            for (int i = 0; i < Count; i++)
    //                _entries.Add(Parse<T>(DataOffset + i * _stride));
    //    }

    //    protected override int OnGetLookupCount()
    //    {
    //        return _entries.Count > 0 ? 1 : 0;
    //    }

    //    protected override int OnGetSize()
    //    {
    //        return 8 + _entries.Count * _stride;
    //    }

    //    protected override void OnWrite(VoidPtr address)
    //    {
    //        for (int i = 0; i < _entries.Count; i++)
    //            _entries[i].Write(address[i, _stride]);

    //        sListOffset* o = (sListOffset*)(RebuildAddress = address + _entries.Count * _stride);
    //        if (_entries.Count > 0)
    //        {
    //            o->_startOffset = Offset(address);
    //            o->_listCount = _entries.Count;
    //            Lookup(&o->_startOffset);
    //        }
    //        else
    //        {
    //            o->_startOffset = 0;
    //            o->_listCount = 0;
    //        }
    //    }
    //}
    public abstract unsafe class ListOffset : SakuraiEntryNode
    {
        private sListOffset hdr;

        [Category("List Offset")] public int StartOffset => hdr._startOffset;
        [Category("List Offset")] public int Count => hdr._listCount;

        protected override void OnParse(VoidPtr address)
        {
            hdr = *(sListOffset*) address;
        }
    }
}