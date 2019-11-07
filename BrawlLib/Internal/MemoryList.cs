using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BrawlLib.Internal
{
    public unsafe class MemoryEnumerator<T> : IEnumerator<T> where T : struct
    {
        private readonly int _stride;
        private readonly byte* _base;
        private byte* _current;
        private readonly byte* _ceil;

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

        object System.Collections.IEnumerator.Current => Current;

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
        private int _count;
        private readonly int _cap;
        private readonly int _stride;
        private readonly bool _readOnly;
        private readonly byte* _base;

        public MemoryList(void* address, int count) : this(address, count, count, true)
        {
        }

        public MemoryList(void* address, int count, bool readOnly) : this(address, count, count, readOnly)
        {
        }

        public MemoryList(void* address, int count, int capacity, bool readOnly)
        {
            _count = count;
            _cap = capacity;
            _readOnly = readOnly;
            _stride = Marshal.SizeOf(typeof(T));
            _base = (byte*) address;
        }

        public int IndexOf(T item)
        {
            byte* p = _base;
            for (int i = 0; i < _count; i++, p += _stride)
            {
                if (item.Equals(Marshal.PtrToStructure((IntPtr) p, typeof(T))))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            if (_readOnly || _count >= _cap)
            {
                throw new AccessViolationException();
            }

            if (index >= _count || index < 0)
            {
                throw new IndexOutOfRangeException();
            }

            byte* p = _base + index * _stride;
            Memory.Move(p + _stride, p, (uint) ((_count - index) * _stride));
            _count++;
        }

        public void RemoveAt(int index)
        {
            if (_readOnly)
            {
                throw new AccessViolationException();
            }

            if (index >= _count || index < 0)
            {
                throw new IndexOutOfRangeException();
            }

            byte* p = _base + index * _stride;
            Memory.Move(p, p + _stride, (uint) ((_count - 1 - index) * _stride));
            _count--;
        }

        public T this[int index]
        {
            get
            {
                if (index >= _count || index < 0)
                {
                    throw new IndexOutOfRangeException();
                }

                return (T) Marshal.PtrToStructure((IntPtr) (_base + index * _stride), typeof(T));
            }
            set
            {
                if (_readOnly)
                {
                    throw new AccessViolationException();
                }

                if (index >= _count || index < 0)
                {
                    throw new IndexOutOfRangeException();
                }

                Marshal.StructureToPtr(value, (IntPtr) (_base + index * _stride), false);
            }
        }

        public void Add(T item)
        {
            if (_readOnly || _count >= _cap)
            {
                throw new AccessViolationException();
            }

            Marshal.StructureToPtr(item, (IntPtr) (_base + _count * _stride), false);
            _count++;
        }

        public void Clear()
        {
            _count = 0;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex + _count > array.Length)
            {
                throw new ArgumentException();
            }

            int index = arrayIndex;
            foreach (T t in this)
            {
                array[index++] = t;
            }
        }

        public int Count => _count;
        public int Capacity => _cap;
        public bool IsReadOnly => _readOnly;

        public bool Remove(T item)
        {
            if (_readOnly)
            {
                throw new AccessViolationException();
            }

            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new MemoryEnumerator<T>(_base, _count);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}