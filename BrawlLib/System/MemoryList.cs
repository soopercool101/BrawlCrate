using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System
{
    public unsafe class MemoryEnumerator<T> : IEnumerator<T> where T : struct
    {
        int _stride;
        byte* _base;
        byte* _current;
        byte* _ceil;

        internal MemoryEnumerator(void* address, int count)
        {
            _stride = Marshal.SizeOf(typeof(T));
            _current = _base = (byte*)address;
            _ceil = _base + (_stride * count);
        }

        public T Current { get { return (T)Marshal.PtrToStructure((IntPtr)_current, typeof(T)); } }
        public void Dispose() { }
        object global::System.Collections.IEnumerator.Current { get { return Current; } }
        public bool MoveNext() { return (_current += _stride) < _ceil; }
        public void Reset() { _current = _base; }
    }

    public unsafe class MemoryList<T> : IList<T>, IEnumerable<T> where T : struct
    {
        int _count;
        int _cap;
        int _stride;
        bool _readOnly;
        byte* _base;

        public MemoryList(void* address, int count) : this(address, count, count, true) { }
        public MemoryList(void* address, int count, bool readOnly) : this(address, count, count, readOnly) { }
        public MemoryList(void* address, int count, int capacity, bool readOnly)
        {
            _count = count;
            _cap = capacity;
            _readOnly = readOnly;
            _stride = Marshal.SizeOf(typeof(T));
            _base = (byte*)address;
        }

        public int IndexOf(T item)
        {
            byte* p = _base;
            for (int i = 0; i < _count; i++, p += _stride)
                if (item.Equals(Marshal.PtrToStructure((IntPtr)p, typeof(T))))
                    return i;
            return -1;
        }

        public void Insert(int index, T item)
        {
            if (_readOnly || (_count >= _cap))
                throw new AccessViolationException();
            if ((index >= _count) || (index < 0))
                throw new IndexOutOfRangeException();

            byte* p = _base + (index * _stride);
            Memory.Move(p + _stride, p, (uint)((_count - index) * _stride));
            _count++;
        }

        public void RemoveAt(int index)
        {
            if (_readOnly)
                throw new AccessViolationException();
            if ((index >= _count) || (index < 0))
                throw new IndexOutOfRangeException();

            byte* p = _base + (index * _stride);
            Memory.Move(p, p + _stride, (uint)((_count - 1 - index) * _stride));
            _count--;
        }

        public T this[int index]
        {
            get
            {
                if ((index >= _count) || (index < 0))
                    throw new IndexOutOfRangeException();
                return (T)Marshal.PtrToStructure((IntPtr)(_base + index * _stride), typeof(T));
            }
            set
            {
                if (_readOnly)
                    throw new AccessViolationException();
                if ((index >= _count) || (index < 0))
                    throw new IndexOutOfRangeException();
                Marshal.StructureToPtr(value, (IntPtr)(_base + index * _stride), false);
            }
        }

        public void Add(T item)
        {
            if (_readOnly || (_count >= _cap))
                throw new AccessViolationException();

            Marshal.StructureToPtr(item, (IntPtr)(_base + _count * _stride), false);
            _count++;
        }
        public void Clear() { _count = 0; }
        public bool Contains(T item) { return IndexOf(item) >= 0; }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if ((arrayIndex + _count) > array.Length)
                throw new ArgumentException();

            int index = arrayIndex;
            foreach (T t in this)
                array[index++] = t;
        }

        public int Count { get { return _count; } }
        public int Capacity { get { return _cap; } }
        public bool IsReadOnly { get { return _readOnly; } }

        public bool Remove(T item)
        {
            if (_readOnly)
                throw new AccessViolationException();

            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator() { return new MemoryEnumerator<T>(_base, _count); }
        global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }

}
