using System.Runtime.InteropServices;

namespace System
{
    public class UnsafeBuffer : IDisposable
    {
        public UnsafeBuffer(int size)
        {
            Address = Marshal.AllocHGlobal(size);
            Length = size;
        }

        public VoidPtr Address { get; private set; }

        public int Length { get; set; }

        public VoidPtr this[int count, int stride] => Address[count, stride];

        public void Dispose()
        {
            try
            {
                if (Address)
                {
                    Marshal.FreeHGlobal(Address);
                    Address = null;
                    GC.SuppressFinalize(this);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        ~UnsafeBuffer()
        {
            Dispose();
        }
    }
}