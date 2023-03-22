using System;
using System.Runtime.InteropServices;
using System.Text;

namespace BrawlLib.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct VoidPtr
    {
        //address
        public void* address;

        public byte Byte => *(byte*) address;
        public sbyte SByte => *(sbyte*) address;
        public char Char => *(char*)address;
        public ushort UShort => *(bushort*) address;
        public short Short => *(bshort*) address;
        public uint UInt => *(buint*) address;
        public int Int => *(bint*) address;
        public float Single => *(bfloat*) address;

        public static int operator -(VoidPtr p1, VoidPtr p2)
        {
            return checked((int) ((byte*) p1.address - (byte*) p2.address));
        }

        public static VoidPtr operator +(VoidPtr p1, uint addr)
        {
            return new VoidPtr {address = (byte*) p1.address + addr};
        }

        public static VoidPtr operator -(VoidPtr p1, uint addr)
        {
            return new VoidPtr {address = (byte*) p1.address - addr};
        }

        public static VoidPtr operator +(VoidPtr p1, int addr)
        {
            return new VoidPtr {address = (byte*) p1.address + addr};
        }

        public static VoidPtr operator -(VoidPtr p1, int addr)
        {
            return new VoidPtr {address = (byte*) p1.address - addr};
        }

        public static VoidPtr operator +(VoidPtr p1, ulong addr)
        {
            return new VoidPtr {address = (byte*) p1.address + addr};
        }

        public static VoidPtr operator -(VoidPtr p1, ulong addr)
        {
            return new VoidPtr {address = (byte*) p1.address - addr};
        }

        public static VoidPtr operator +(VoidPtr p1, long addr)
        {
            return new VoidPtr {address = (byte*) p1.address + addr};
        }

        public static VoidPtr operator -(VoidPtr p1, long addr)
        {
            return new VoidPtr {address = (byte*) p1.address - addr};
        }

        public static bool operator >(VoidPtr p1, VoidPtr p2)
        {
            return p1.address > p2.address;
        }

        public static bool operator <(VoidPtr p1, VoidPtr p2)
        {
            return p1.address < p2.address;
        }

        public static bool operator >=(VoidPtr p1, VoidPtr p2)
        {
            return p1.address >= p2.address;
        }

        public static bool operator <=(VoidPtr p1, VoidPtr p2)
        {
            return p1.address <= p2.address;
        }

        public static bool operator ==(VoidPtr p1, VoidPtr p2)
        {
            return p1.address == p2.address;
        }

        public static bool operator !=(VoidPtr p1, VoidPtr p2)
        {
            return p1.address != p2.address;
        }

        public VoidPtr this[int count, int stride] => this + count * stride;

        //type casts
        public static implicit operator bool(VoidPtr ptr)
        {
            return ptr.address != null;
        }

        public static implicit operator void*(VoidPtr ptr)
        {
            return ptr.address;
        }

        public static implicit operator VoidPtr(void* ptr)
        {
            return new VoidPtr {address = ptr};
        }

        public static implicit operator uint(VoidPtr ptr)
        {
            return checked((uint) ptr.address);
        }

        public static implicit operator VoidPtr(uint ptr)
        {
            return new VoidPtr {address = (void*) ptr};
        }

        public static implicit operator int(VoidPtr ptr)
        {
            return checked((int) ptr.address);
        }

        public static implicit operator VoidPtr(int ptr)
        {
            return new VoidPtr {address = (void*) ptr};
        }

        public static implicit operator ulong(VoidPtr ptr)
        {
            return (ulong) ptr.address;
        }

        public static implicit operator VoidPtr(ulong ptr)
        {
            return new VoidPtr {address = (void*) ptr};
        }

        public static implicit operator long(VoidPtr ptr)
        {
            return (long) ptr.address;
        }

        public static implicit operator VoidPtr(long ptr)
        {
            return new VoidPtr {address = (void*) ptr};
        }

        public static implicit operator VoidPtr(IntPtr ptr)
        {
            return new VoidPtr {address = (void*) ptr};
        }

        public static implicit operator IntPtr(VoidPtr ptr)
        {
            return (IntPtr) ptr.address;
        }

        public override int GetHashCode()
        {
            return (int) address;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static void Swap(float* p1, float* p2)
        {
            float f = *p1;
            *p1 = *p2;
            *p2 = f;
        }

        public static void Swap(int* p1, int* p2)
        {
            int f = *p1;
            *p1 = *p2;
            *p2 = f;
        }

        public static void Swap(short* p1, short* p2)
        {
            short f = *p1;
            *p1 = *p2;
            *p2 = f;
        }

        public static void Swap(ushort* p1, ushort* p2)
        {
            ushort f = *p1;
            *p1 = *p2;
            *p2 = f;
        }

        public static void Swap(byte* p1, byte* p2)
        {
            byte f = *p1;
            *p1 = *p2;
            *p2 = f;
        }

        public string GetUTF8String()
        {
            return GetUTF8String(0);
        }

        public string GetUTF8String(uint offset)
        {
            byte[] bytes = new byte[new string((sbyte*)(this + offset)).Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (this + offset).Byte;
                offset++;
            }

            return Encoding.UTF8.GetString(bytes);
        }

        public string GetUTF8String(uint offset, uint size)
        {
            byte[] bytes = new byte[size];
            for (int i = 0; i < size; i++)
            {
                bytes[i] = (this + offset).Byte;
                offset++;
            }

            return Encoding.UTF8.GetString(bytes);
        }

        public uint WriteUTF8String(string s)
        {
            return WriteUTF8String(s, true, 0);
        }

        public uint WriteUTF8String(string s, bool nullTerminated)
        {
            return WriteUTF8String(s, nullTerminated, 0);
        }

        public uint WriteUTF8String(string s, uint offset)
        {
            return WriteUTF8String(s, true, offset);
        }

        public uint WriteUTF8String(string s, bool nullTerminated, uint offset)
        {
            return WriteUTF8String(s, nullTerminated, offset, (uint)(s.UTF8Length() + (nullTerminated ? 1 : 0)));
        }

        public uint WriteUTF8String(string s, uint offset, uint size)
        {
            return WriteUTF8String(s, false, offset, size);
        }

        public uint WriteUTF8String(string s, bool nullTerminated, uint offset, uint size)
        {
            byte* ptr = (byte*)(this + offset);
            byte[] name = Encoding.UTF8.GetBytes(s);
            for (int j = 0; j < name.Length && j < size; j++)
            {
                ptr[j] = name[j];
            }

            if (name.Length < size && nullTerminated)
            {
                ptr[name.Length] = 0;
            }
            else if (nullTerminated)
            {
                ptr[size - 1] = 0;
            }

            for (int j = name.Length; j < size; j++)
            {
                ptr[j] = 0;
            }

            return size;
        }
    }
}