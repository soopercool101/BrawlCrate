using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System
{
    public unsafe class MemoryEnumerator<T> : IEnumerator<T> where T : struct
    {
        private readonly byte* _base;
        private readonly byte* _ceil;
        private readonly int _stride;
        private byte* _current;

        internal MemoryEnumerator(void* address, int count)
        {
            _stride = Marshal.SizeOf(typeof(T));
            _current = _base = (byte*) address;
            _ceil = _base + _stride * count;
        }

        public T Current => (T) Marshal.PtrToStructure((IntPtr) _current, typeof(T));

        public void Dispose()
        {
        }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            return (_current += _stride) < _ceil;
        }

        public void Reset()
        {
            _current = _base;
        }
    }

    public unsafe class MemoryList<T> : IList<T>, IEnumerable<T> where T : struct
    {
        private readonly byte* _base;
        private readonly int _stride;

        public MemoryList(void* address, int count) : this(address, count, count, true)
        {
        }

        public MemoryList(void* address, int count, bool readOnly) : this(address, count, count, readOnly)
        {
        }

        public MemoryList(void* address, int count, int capacity, bool readOnly)
        {
            Count = count;
            Capacity = capacity;
            IsReadOnly = readOnly;
            _stride = Marshal.SizeOf(typeof(T));
            _base = (byte*) address;
        }

        public int Capacity { get; }

        public int IndexOf(T item)
        {
            var p = _base;
            for (var i = 0; i < Count; i++, p += _stride)
                if (item.Equals(Marshal.PtrToStructure((IntPtr) p, typeof(T))))
                    return i;

            return -1;
        }

        public void Insert(int index, T item)
        {
            if (IsReadOnly || Count >= Capacity) throw new AccessViolationException();

            if (index >= Count || index < 0) throw new IndexOutOfRangeException();

            var p = _base + index * _stride;
            Memory.Move(p + _stride, p, (uint) ((Count - index) * _stride));
            Count++;
        }

        public void RemoveAt(int index)
        {
            if (IsReadOnly) throw new AccessViolationException();

            if (index >= Count || index < 0) throw new IndexOutOfRangeException();

            var p = _base + index * _stride;
            Memory.Move(p, p + _stride, (uint) ((Count - 1 - index) * _stride));
            Count--;
        }

        public T this[int index]
        {
            get
            {
                if (index >= Count || index < 0) throw new IndexOutOfRangeException();

                return (T) Marshal.PtrToStructure((IntPtr) (_base + index * _stride), typeof(T));
            }
            set
            {
                if (IsReadOnly) throw new AccessViolationException();

                if (index >= Count || index < 0) throw new IndexOutOfRangeException();

                Marshal.StructureToPtr(value, (IntPtr) (_base + index * _stride), false);
            }
        }

        public void Add(T item)
        {
            if (IsReadOnly || Count >= Capacity) throw new AccessViolationException();

            Marshal.StructureToPtr(item, (IntPtr) (_base + Count * _stride), false);
            Count++;
        }

        public void Clear()
        {
            Count = 0;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex + Count > array.Length) throw new ArgumentException();

            var index = arrayIndex;
            foreach (var t in this) array[index++] = t;
        }

        public int Count { get; private set; }

        public bool IsReadOnly { get; }

        public bool Remove(T item)
        {
            if (IsReadOnly) throw new AccessViolationException();

            var index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new MemoryEnumerator<T>(_base, Count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}