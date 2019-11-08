using BrawlLib.Internal;
using System.Collections.Generic;

namespace BrawlLib.SSBB
{
    /// <summary>
    /// When rebuilding, add the addresses of all offset values to this collection
    /// </summary>
    public class LookupManager : IEnumerable<VoidPtr>
    {
        private readonly List<VoidPtr> _values = new List<VoidPtr>();
        public int Count => _values.Count;

        public VoidPtr this[int index]
        {
            get
            {
                if (index >= 0 && index < _values.Count)
                {
                    return _values[index];
                }

                return null;
            }
            set
            {
                if (index >= 0 && index < _values.Count)
                {
                    _values[index] = value;
                }
            }
        }

        public void Add(params VoidPtr[] valueAddrs)
        {
            foreach (VoidPtr value in valueAddrs)
            {
                if (!_values.Contains(value))
                {
                    _values.Add(value);
                }
            }
        }

        public void Sort()
        {
            _values.Sort();
        }

        public unsafe int Write(VoidPtr address)
        {
            Sort();

            bint* values = (bint*) address;
            foreach (int offset in this)
            {
                *values++ = offset;
            }

            return (int) values - (int) address;
        }

        public void Write(ref VoidPtr address)
        {
            address += Write(address);
        }

        public IEnumerator<VoidPtr> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}