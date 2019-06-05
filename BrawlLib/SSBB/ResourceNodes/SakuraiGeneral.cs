using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace BrawlLib.SSBBTypes
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

        [Browsable(false)] public List<int> DataOffsets { get; set; } = new List<int>();

        [Browsable(false)] public List<SakuraiEntryNode> References { get; set; } = new List<SakuraiEntryNode>();

        private static void AddParsers(Assembly e)
        {
            if (e == null) return;

            Delegate del;
            foreach (var t in e.GetTypes())
                if (t.IsSubclassOf(typeof(TableEntryNode)))
                    if ((del = Delegate.CreateDelegate(typeof(SakuraiSectionParser), t, "TestType", false, false)) !=
                        null)
                        _parsers.Add(del as SakuraiSectionParser);
        }

        public static TableEntryNode GetRaw(string name)
        {
            TableEntryNode n = null;
            foreach (var d in _parsers)
                if ((n = d(name)) != null)
                    break;

            return n;
        }

        protected override void OnParse(VoidPtr address)
        {
            DataOffsets = new List<int> {_offset};

            int offset = *(bint*) address;

            while (offset > 0)
            {
                DataOffsets.Add(offset);

                offset = *(bint*) (BaseAddress + offset);

                //Infinite loops are NO GOOD
                if (DataOffsets.Contains(offset)) break;
            }
        }
    }

    public unsafe class RawDataNode : TableEntryNode
    {
        internal byte[] _data;

        protected override void OnParse(VoidPtr address)
        {
            DataOffsets.Add(_offset);

            if (_initSize > 0)
            {
                _data = new byte[_initSize];
                var b = (byte*) address;
                for (var i = 0; i < _data.Length; i++) _data[i] = b[i];
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
            var header = (byte*) address;
            if (_data != null)
                for (var i = 0; i < _data.Length; i++)
                    header[i] = _data[i];
        }
    }

    public unsafe class IndexValue : SakuraiEntryNode
    {
        private const string _validDec = "0123456789";
        private const string _validHex = "ABCDEFabcdef";

        public static bool _hexadecimal;
        public int _value;

        public bool HexDisplay
        {
            get => _hexadecimal;
            set => _hexadecimal = value;
        }

        [Category("Index Entry")]
        public string Value
        {
            get
            {
                if (_hexadecimal)
                {
                    var neg = false;
                    var val = _value;
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
                    var neg = false;
                    if (value.StartsWith("-"))
                    {
                        value = value.Substring(1);
                        neg = true;
                    }

                    if (value.StartsWith("0x")) value = value.Substring(2);

                    value = value.RemoveInvalidCharacters(_validDec + _validHex);
                    _value = int.Parse(value, NumberStyles.HexNumber);

                    if (neg) _value = -_value;
                }
                else
                {
                    value = value.RemoveInvalidCharacters(_validDec);
                    _value = int.Parse(value);
                }

                SignalPropertyChange();
            }
        }

        public static explicit operator int(IndexValue val)
        {
            return val._value;
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
        [Category("Offset Entry")] public int DataOffset { get; private set; }

        protected override void OnParse(VoidPtr address)
        {
            DataOffset = *(bint*) address;
        }
    }

    /// <summary>
    ///     Generic list class for handling structs in a memory array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntryList<T> : SakuraiEntryNode, IEnumerable<T>, IListSource where T : SakuraiEntryNode
    {
        private readonly int _stride, _count = -1;

        public EntryList(int stride, int count)
        {
            _stride = stride;
            _count = count;
        }

        public EntryList(int stride)
        {
            _stride = stride;
        }

        public BindingList<T> Entries { get; private set; }

        protected override void OnParse(VoidPtr address)
        {
            Entries = new BindingList<T>();
            if (_count > 0 || _initSize > 0)
                for (var i = 0; i < (_count > 0 ? _count : _initSize / _stride); i++)
                {
                    var e = Parse<T>(address[i, _stride]);
                    e._index = i;
                    Entries.Add(e);
                }
        }

        protected override int OnGetSize()
        {
            return Entries.Count * _stride;
        }

        protected override void OnWrite(VoidPtr address)
        {
            RebuildAddress = address;
            for (var i = 0; i < Entries.Count; i++) Entries[i].Write(address[i, _stride]);
        }

        protected override int OnGetLookupCount()
        {
            var count = 0;
            foreach (var e in Entries) count += e.GetLookupCount();

            return count;
        }

        #region Child Enumeration

        public IEnumerator<T> GetEnumerator()
        {
            return Entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsListCollection => false;

        public IList GetList()
        {
            return Entries;
        }

        public int Count => Entries.Count;

        public T this[int i]
        {
            get
            {
                if (i >= 0 && i < Count) return Entries[i];

                return null;
            }
            set
            {
                if (i >= 0 && i < Count)
                {
                    SignalPropertyChange();
                    Entries[i] = value;
                }
            }
        }

        private void Insert(int i, T e)
        {
            if (i >= 0)
            {
                if (i < Count)
                    Entries.Insert(i, e);
                else
                    Entries.Add(e);

                SignalRebuildChange();
            }
        }

        private void Add(int i, T e)
        {
            Entries.Add(e);
            SignalRebuildChange();
        }

        internal void RemoveAt(int i)
        {
            if (i >= 0 && i < Count)
            {
                Entries.RemoveAt(i);
                SignalRebuildChange();
            }
        }

        private void Clear()
        {
            if (Count != 0)
            {
                Entries.Clear();
                SignalRebuildChange();
            }
        }

        #endregion
    }

    /// <summary>
    ///     Deprecated; avoid use
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