using System.Runtime.InteropServices;

namespace BrawlLib.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PString
    {
        private byte* _address;

        public PString(byte* address)
        {
            _address = address;
        }

        public static implicit operator int(PString p)
        {
            return (int) p._address;
        }

        //public static implicit operator PString(int p) { return new PString((sbyte*)p); }
        public static implicit operator uint(PString p)
        {
            return (uint) p._address;
        }

        //public static implicit operator PString(uint p) { return new PString((sbyte*)p); }
        public static implicit operator sbyte*(PString p)
        {
            return (sbyte*) p._address;
        }

        public static implicit operator PString(sbyte* p)
        {
            return *(PString*) &p;
        }

        public static implicit operator byte*(PString p)
        {
            return p._address;
        }

        public static implicit operator PString(byte* p)
        {
            return *(PString*) &p;
        }

        public static implicit operator VoidPtr(PString p)
        {
            return *(VoidPtr*) &p;
        }

        public static implicit operator PString(VoidPtr p)
        {
            return *(PString*) &p;
        }

        public static explicit operator string(PString p)
        {
            return new string((sbyte*) p._address);
        }

        public static PString operator +(PString p, int amount)
        {
            p._address += amount;
            return p;
        }

        public static bool operator ==(PString p, string s)
        {
            return p.Equals(s, false);
        }

        public static bool operator !=(PString p, string s)
        {
            return !p.Equals(s, false);
        }

        public static bool operator ==(PString p, PString s)
        {
            return p.Equals(s, false);
        }

        public static bool operator !=(PString p, PString s)
        {
            return !p.Equals(s, false);
        }

        public override bool Equals(object obj)
        {
            if (obj is PString)
            {
                return Equals((PString) obj, false);
            }

            if (obj is string)
            {
                return Equals((string) obj, false);
            }

            return false;
        }

        public static bool Equals(PString s1, PString s2, bool ignoreCase)
        {
            return s1.Equals(s2, ignoreCase);
        }

        public static bool Equals(PString s1, string s2, bool ignoreCase)
        {
            return s1.Equals(s2, ignoreCase);
        }

        public bool Equals(PString s, bool ignoreCase)
        {
            byte* pStr1 = _address;
            byte* pStr2 = s._address;
            byte b1, b2;

            do
            {
                b1 = *pStr1++;
                b2 = *pStr2++;
                if (b1 != b2)
                {
                    if (ignoreCase)
                    {
                        if (b1 >= 0x41 && b1 <= 0x5A)
                        {
                            if (b1 + 0x20 == b2)
                            {
                                continue;
                            }
                        }
                        else if (b1 >= 0x61 && b1 <= 0x7A)
                        {
                            if (b1 - 0x20 == b2)
                            {
                                continue;
                            }
                        }
                    }

                    return false;
                }
            } while (b1 != 0);

            return true;
        }

        public bool Equals(string s, bool ignoreCase)
        {
            byte* pStr1 = _address;
            char* pStr2;
            byte b1, b2;

            fixed (char* pChar = s)
            {
                pStr2 = pChar;
                do
                {
                    b1 = *pStr1++;
                    b2 = (byte) *pStr2++;
                    if (b1 != b2)
                    {
                        if (ignoreCase)
                        {
                            if (b1 >= 0x41 && b1 <= 0x5A)
                            {
                                if (b1 + 0x20 == b2)
                                {
                                    continue;
                                }
                            }
                            else if (b1 >= 0x61 && b1 <= 0x7A)
                            {
                                if (b1 - 0x20 == b2)
                                {
                                    continue;
                                }
                            }
                        }

                        return false;
                    }
                } while (b1 != 0);
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return new string(this);
        }

        public byte this[int index]
        {
            get => _address[index];
            set => _address[index] = value;
        }

        public int Length
        {
            get
            {
                int len = 0;
                byte* p = _address;
                while (*p++ != 0)
                {
                    len++;
                }

                return len;
            }
        }

        public void Write(string s)
        {
            Write(s, 0);
        }

        public void Write(string s, int offset)
        {
            Write(s, offset, s.Length);
        }

        public void Write(string s, int offset, int len)
        {
            fixed (char* p = s)
            {
                Write(p, offset, len);
            }
        }

        public void Write(char* p, int offset, int len)
        {
            byte* s = _address;
            p += offset;
            for (int i = 0; i < len; i++)
            {
                *s++ = (byte) *p++;
            }
        }

        public void Write(byte* p, int offset, int len)
        {
            byte* s = _address;
            p += offset;
            for (int i = 0; i < len; i++)
            {
                *s++ = *p++;
            }
        }

        //internal static unsafe bool Equals(sbyte* pStr, string str)
        //{
        //    int c1, c2;
        //    fixed (char* p = str)
        //    {
        //        char* pStr2 = p;
        //        do
        //        {
        //            c1 = *pStr++;
        //            c2 = *pStr2++;
        //            if (c1 != c2)
        //                return false;
        //        } while ((c1 != 0) && (c2 != 0));
        //        return true;
        //    }
        //}
    }
}