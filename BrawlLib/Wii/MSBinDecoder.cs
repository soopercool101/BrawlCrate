using BrawlLib.Internal;
using System;
using System.Text;

namespace BrawlLib.Wii
{
    public static unsafe class MSBinDecoder
    {
        internal static string DecodeString(byte* sPtr, int len)
        {
            if (len == 0)
            {
                return "";
            }

            byte* buffer = stackalloc byte[1024];

            byte* dPtr = buffer;
            byte* ceil = sPtr + len;

            byte b;
            int bits;

            while (sPtr < ceil)
            {
                switch (b = *sPtr++)
                {
                    case 0x0A:
                        *dPtr++ = 0x0D; // \r
                        *dPtr++ = 0x0A; // \n
                        break;

                    case 0x11:
                        *dPtr++ = 0x5B; // [
                        WriteString(ref dPtr, "border=");
                        WriteHex(ref dPtr, ref sPtr, 4);
                        *dPtr++ = 0x5D; // ]
                        break;

                    case 0x12:
                        *dPtr++ = 0x3C; // <
                        bits = (*sPtr++).CountBits();
                        for (int i = 0; i < bits; i++)
                        {
                            if (i != 0)
                            {
                                *dPtr++ = 0x2C; // ,
                            }

                            switch (*sPtr++)
                            {
                                case 0x0C: //color
                                    WriteString(ref dPtr, "color=");
                                    WriteHex(ref dPtr, ref sPtr, 4);
                                    break;

                                case 0x0D: //flags
                                    WriteString(ref dPtr, "size=");
                                    WriteHex(ref dPtr, ref sPtr, 2);
                                    break;
                            }
                        }

                        *dPtr++ = 0x3E; // >
                        break;

                    case 0x13:
                        WriteString(ref dPtr, "</end>");
                        break;

                    default:
                        *dPtr++ = b;
                        break;
                }
            }

            int input_count = (int) dPtr - (int) buffer;
            int output_count = Encoding.UTF8.GetDecoder().GetCharCount(buffer, input_count, true);
            char[] buffer2 = new char[output_count];
            fixed (char* ptr = buffer2)
            {
                Encoding.UTF8.GetDecoder().GetChars(buffer, input_count,
                    ptr, output_count, true);
            }

            return new string(buffer2);
        }

        public static int EncodeString(string s, byte* dPtr)
        {
            if (s.Length == 0)
            {
                return 0;
            }

            int bLen;
            char c;

            int colorIndex, sizeIndex;
            byte* buffer = stackalloc byte[1024];
            byte* first = dPtr;

            byte[] utf8;
            fixed (char* p = s)
            {
                utf8 = new byte[Encoding.UTF8.GetEncoder().GetByteCount(p, s.Length, true)];
                bool completed;
                fixed (byte* b = utf8)
                {
                    Encoding.UTF8.GetEncoder().Convert(p, s.Length, b, utf8.Length, true,
                        out int charsUsed, out int bytesUsed, out completed);
                }

                if (!completed)
                {
                    throw new Exception("Could not encode the MSBin file.");
                }
            }

            // The loop below is ignorant of UTF8, but it looks for certain ASCII characters and writes each character as 1 byte, so hopefully this kludge will make it work
            char[] utf8_to_char_array = new char[utf8.Length];
            for (int i = 0; i < utf8.Length; i++)
            {
                utf8_to_char_array[i] = (char) utf8[i];
            }

            fixed (char* p = utf8_to_char_array)
            {
                char* sPtr = p, ceil = sPtr + utf8_to_char_array.Length;

                while (sPtr < ceil)
                {
                    c = *sPtr++;
                    if (c == '<')
                    {
                        char* last = sPtr;
                        byte* tPtr = buffer;
                        while (last < ceil && (c = *last++) != '>')
                        {
                            if (c != ' ')
                            {
                                *tPtr++ = (byte) c;
                            }
                        }

                        if (c != '>')
                        {
                            *dPtr++ = (byte) '<';
                        }
                        else
                        {
                            sPtr = last;
                            bLen = (int) tPtr - (int) buffer;
                            ToLower(buffer, bLen);

                            byte* control = dPtr;
                            byte* flag = dPtr + 1;
                            byte* data = dPtr + 2;

                            if ((sizeIndex = IndexOf(buffer, bLen, "size=")) >= 0)
                            {
                                *control = 0x12;
                                *flag |= 1;
                                *data++ = 0x0D;

                                tPtr = buffer + sizeIndex + 5;
                                ReadHex(ref data, ref tPtr, 2);

                                dPtr = data;
                            }

                            if ((colorIndex = IndexOf(buffer, bLen, "color=")) >= 0)
                            {
                                *control = 0x12;
                                *flag |= 2;
                                *data++ = 0x0C;

                                tPtr = buffer + colorIndex + 6;
                                ReadHex(ref data, ref tPtr, 4);

                                dPtr = data;
                            }

                            if (StrContains(buffer, bLen, "end"))
                            {
                                *dPtr++ = 0x13;
                            }
                        }
                    }
                    else if (c == '[')
                    {
                        char* last = sPtr;
                        byte* tPtr = buffer;
                        while (last < ceil && (c = *last++) != ']')
                        {
                            if (c != ' ')
                            {
                                *tPtr++ = (byte) c;
                            }
                        }

                        if (c != ']')
                        {
                            *dPtr++ = (byte) ']';
                        }
                        else
                        {
                            sPtr = last;
                            bLen = (int) tPtr - (int) buffer;
                            ToLower(buffer, bLen);

                            if ((colorIndex = IndexOf(buffer, bLen, "border=")) >= 0)
                            {
                                tPtr = buffer + colorIndex + 7;

                                *dPtr++ = 0x11;
                                ReadHex(ref dPtr, ref tPtr, 4);
                            }
                        }
                    }
                    else if (c == '\\')
                    {
                        if (sPtr < ceil)
                        {
                            *dPtr++ = (byte) *sPtr++;
                        }
                    }
                    else if (c == '\r')
                    {
                    } //do nothing
                    else
                    {
                        *dPtr++ = (byte) c;
                    }
                }
            }

            return (int) dPtr - (int) first;
        }

        public static int GetStringSize(string s)
        {
            if (s.Length == 0)
            {
                return 0;
            }

            int len = 0, bLen;
            int strlen = s.Length;
            char c;

            bool hasColor, hasSize;
            byte* buffer = stackalloc byte[1024];

            byte[] utf8;
            fixed (char* p = s)
            {
                utf8 = new byte[Encoding.UTF8.GetEncoder().GetByteCount(p, s.Length, true)];
                bool completed;
                fixed (byte* b = utf8)
                {
                    Encoding.UTF8.GetEncoder().Convert(p, s.Length, b, utf8.Length, true,
                        out int charsUsed, out int bytesUsed, out completed);
                }

                if (!completed)
                {
                    throw new Exception("Could not encode the MSBin file.");
                }
            }

            // The loop below is ignorant of UTF8, but it looks for certain ASCII characters and writes each character as 1 byte, so hopefully this kludge will make it work
            char[] utf8_to_char_array = new char[utf8.Length];
            for (int i = 0; i < utf8.Length; i++)
            {
                utf8_to_char_array[i] = (char) utf8[i];
            }

            fixed (char* p = utf8_to_char_array)
            {
                char* sPtr = p, ceil = sPtr + utf8_to_char_array.Length;

                while (sPtr < ceil)
                {
                    c = *sPtr++;
                    if (c == '<')
                    {
                        char* last = sPtr;
                        byte* tPtr = buffer;
                        while (last < ceil && (c = *last++) != '>')
                        {
                            if (c != ' ')
                            {
                                *tPtr++ = (byte) c;
                            }
                        }

                        if (c != '>')
                        {
                            len++;
                        }
                        else
                        {
                            sPtr = last;
                            bLen = (int) tPtr - (int) buffer;
                            ToLower(buffer, bLen);

                            if (hasColor = StrContains(buffer, bLen, "color="))
                            {
                                len += 5;
                            }

                            if (hasSize = StrContains(buffer, bLen, "size="))
                            {
                                len += 3;
                            }

                            if (hasSize || hasColor)
                            {
                                len += 2;
                            }

                            if (StrContains(buffer, bLen, "end"))
                            {
                                len++;
                            }
                        }
                    }
                    else if (c == '[')
                    {
                        char* last = sPtr;
                        byte* tPtr = buffer;
                        while (last < ceil && (c = *last++) != ']')
                        {
                            if (c != ' ')
                            {
                                *tPtr++ = (byte) c;
                            }
                        }

                        if (c != ']')
                        {
                            len++;
                        }
                        else
                        {
                            sPtr = last;
                            bLen = (int) tPtr - (int) buffer;
                            ToLower(buffer, bLen);

                            if (StrContains(buffer, bLen, "border="))
                            {
                                len += 5;
                            }
                        }
                    }
                    else if (c == '\\')
                    {
                        if (sPtr < ceil)
                        {
                            sPtr++;
                            len++;
                        }
                    }
                    else if (c == '\r')
                    {
                    } //do nothing
                    else
                    {
                        len++;
                    }
                }
            }

            return len;
        }

        private static void WriteString(ref byte* dPtr, string value)
        {
            int len = value.Length;

            fixed (char* sPtr = value)
            {
                for (int i = 0; i < len;)
                {
                    *dPtr++ = (byte) sPtr[i++];
                }
            }
        }

        private static void WriteHex(ref byte* dPtr, ref byte* sPtr, int len)
        {
            for (int i = 0; i++ < len;)
            {
                byte b = *sPtr++;
                *dPtr++ = b >= 0xA0 ? (byte) ((b >> 4) + 0x37) : (byte) ((b >> 4) + 0x30);

                b &= 0x0F;
                *dPtr++ = b >= 0x0A ? (byte) (b + 0x37) : (byte) (b + 0x30);
            }
        }

        private static void ReadHex(ref byte* dPtr, ref byte* sPtr, int len)
        {
            byte* buffer = stackalloc byte[2];
            for (int i = 0; i++ < len;)
            {
                for (int x = 0; x < 2; x++)
                {
                    byte b = *sPtr++;
                    if (b >= 0x30 && b <= 0x39)
                    {
                        b -= 0x30;
                    }
                    else if (b >= 0x41 && b <= 0x5A)
                    {
                        b -= 0x37;
                    }
                    else if (b >= 0x61 && b <= 0x7A)
                    {
                        b -= 0x57;
                    }
                    else
                    {
                        b = 0;
                    }

                    buffer[x] = b;
                }

                *dPtr++ = (byte) ((buffer[0] << 4) | buffer[1]);
            }
        }

        private static void ToLower(byte* ptr, int len)
        {
            byte b;
            for (byte* ceil = ptr + len; ptr < ceil; ptr++)
            {
                b = *ptr;
                if (b > 0x40 && b < 0x5B)
                {
                    *ptr = (byte) (b + 0x20);
                }
            }
        }

        private static int IndexOf(byte* ptr, int len, string s)
        {
            int slen = s.Length;
            int x, y;

            for (int start = 0, end = slen - 1; end < len; start++, end++)
            {
                for (x = start, y = 0; y < slen && ptr[x++] == s[y++];)
                {
                    ;
                }

                if (y == slen)
                {
                    return start;
                }
            }

            return -1;
        }

        private static bool StrContains(byte* ptr, int len, string s)
        {
            int slen = s.Length;
            int x, y;

            for (int start = 0, end = slen - 1; end < len; start++, end++)
            {
                for (x = start, y = 0; y < slen && ptr[x++] == s[y++];)
                {
                    ;
                }

                if (y == slen)
                {
                    return true;
                }
            }

            return false;
        }
    }
}