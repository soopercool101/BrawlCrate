using System;
using System.Runtime.InteropServices;

namespace BrawlLib.Internal
{
    public class UnsafeBuffer : IDisposable
    {
        private VoidPtr _data;
        public VoidPtr Address => _data;

        private int _length;

        public int Length
        {
            get => _length;
            set => _length = value;
        }

        public UnsafeBuffer(int size)
        {
            _data = Marshal.AllocHGlobal(size);
            _length = size;
        }

        ~UnsafeBuffer()
        {
            Dispose();
        }

        public VoidPtr this[int count, int stride] => _data[count, stride];

        public void Dispose()
        {
            try
            {
                if (_data)
                {
                    Marshal.FreeHGlobal(_data);
                    _data = null;
                    GC.SuppressFinalize(this);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}