using System;
using System.Globalization;
using System.IO;

namespace BrawlLib.Internal.IO
{
    public unsafe class XmlReader : IDisposable
    {
        private const int _nameMax = 128;
        private const int _valueMax = 384;

        internal byte* _base, _ptr, _ceil;
        private readonly int _length;

        private int _position;
        //private readonly int _depth;

        //private byte[] _buffer = new byte[512];
        internal bool _inTag;

        internal bool _inString = false;
        //private string _nameBuffer = new string(' ', _nameMax);
        //private string _valueBuffer = new string(' ', _valueMax);

        private UnsafeBuffer _stringBuffer; // = new UnsafeBuffer(_nameMax + _valueMax);
        private readonly byte* _namePtr, _valPtr;

        public PString Name => _namePtr;
        public PString Value => _valPtr;

        public XmlReader(void* pSource, int length)
        {
            _position = 0;
            _length = length;
            _base = _ptr = (byte*) pSource;
            _ceil = _ptr + length;

            _stringBuffer = new UnsafeBuffer(_nameMax + _valueMax + 2);
            _namePtr = (byte*) _stringBuffer.Address;
            _valPtr = _namePtr + _nameMax + 1;

            //Find start of Xml file
            if (BeginElement() && Name.Equals("?xml"))
            {
                while (_ptr < _ceil && *_ptr++ != '>')
                {
                    ;
                }

                _inTag = false;
            }
            else
            {
                throw new IOException("File is not a valid XML file.");
            }
        }

        ~XmlReader()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_stringBuffer != null)
            {
                _stringBuffer.Dispose();
                _stringBuffer = null;
            }
        }

        private bool ReadString()
        {
            int len = 0;
            bool inStr = false;
            byte* pOut = _valPtr;

            while (len < _valueMax && _ptr < _ceil)
            {
                if (*_ptr <= 0x20)
                {
                    if (inStr)
                    {
                        break;
                    }

                    inStr = true;
                    continue;
                }

                if (*_ptr == '<' || *_ptr == '>' || *_ptr == '/')
                {
                    if (!inStr)
                    {
                        break;
                    }
                }
                else
                {
                    if (!inStr)
                    {
                        inStr = true;
                    }
                }

                pOut[len++] = *_ptr++;
            }

            pOut[len] = 0;

            return len > 0;
        }

        //Reads characters into name pointer. Mainly for element/attribute names
        private bool ReadString(byte* pOut, int length)
        {
            int len = 0;
            bool inStr = false;
            //byte* pOut = _namePtr;
            byte b;

            SkipWhitespace();
            while (len < length && _ptr < _ceil)
            {
                if (inStr)
                {
                    b = *_ptr++;
                    if (b == '"')
                    {
                        break;
                    }

                    //if (b < 0x20)
                    //    continue;
                }
                else
                {
                    b = *_ptr;

                    if (b <= 0x20 || b == '<' || b == '>' || b == '/' || b == '=')
                    {
                        break;
                    }

                    if (b == '"')
                    {
                        if (len == 0)
                        {
                            _ptr++;
                            inStr = true;
                            continue;
                        }

                        break;
                    }

                    _ptr++;
                }

                pOut[len++] = b;
            }

            pOut[len] = 0;

            return len > 0;
        }

        private void SkipWhitespace()
        {
            while (_ptr < _ceil && *_ptr <= 0x20)
            {
                _ptr++;
            }
        }

        //Read next non-whitespace byte. Returns 0 on EOF
        private int ReadByte()
        {
            byte b;
            if (_position < _length)
            {
                b = _base[_position++];
                if (b >= 0x20)
                {
                    return b;
                }
            }

            return -1;

            //byte b;
            //while (_position < _length)
            //{
            //    b = _base[_position];
            //    if ((b == 0x3C) || (b == 0x3E))
            //        return false;

            //    *p = b;
            //    _position++;
            //    return true;
            //}
            //return false;
        }

        //Stops on tag end when inside tag.
        //Ignores comments
        //Exits current tag before searching
        public bool BeginElement()
        {
            bool comment = false;
            bool literal = false;
            byte b;

            Top:
            SkipWhitespace();
            while (_ptr < _ceil)
            {
                if (!_inTag)
                {
                    if (*_ptr++ == '<')
                    {
                        _inTag = true;
                        if (ReadString(_namePtr, _nameMax)) //Will fail on delimiter
                        {
                            if (_namePtr[0] == '!' && _namePtr[1] == '-' && _namePtr[2] == '-')
                            {
                                comment = true;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    //Skip string literals when inside tags
                    if (literal)
                    {
                        if (*_ptr++ == '"')
                        {
                            literal = false;
                        }

                        continue;
                    }

                    //Skip comments
                    if (comment)
                    {
                        if (*_ptr++ == '>' && _ptr[-2] == '-' && _ptr[-3] == '-')
                        {
                            comment = false;
                            _inTag = false;
                            goto Top;
                        }

                        continue;
                    }

                    if (*_ptr == '/')
                    {
                        return false;
                    }

                    b = *_ptr++;
                    if (b == '"')
                    {
                        literal = true;
                    }
                    else if (b == '>')
                    {
                        _inTag = false;
                    }
                }

                //if ((*_ptr == '/') && _inTag)
                //    return false;

                //b = *_ptr++;

                //if (b == '"')
                //{
                //    if (_inTag)
                //        literal = true;
                //}
                //else if (b == '<')
                //{
                //    _inTag = true;
                //    if (ReadString(_namePtr, _nameMax)) //Will fail on delimiter
                //    {
                //        if ((_namePtr[0] == '!') && (_namePtr[1] == '-') && (_namePtr[2] == '-'))
                //            comment = true;
                //        else
                //            return true;
                //    }
                //}
                //else if (b == '>')
                //{
                //    _inTag = false;
                //    goto Top;
                //}
            }

            return false;
        }

        //Continues until tag end has been found, then finds end bracket.
        public void EndElement()
        {
            //Guarantees that we are in the end tag, sitting on the delimiter. If not, something is wrong!
            while (BeginElement())
            {
                EndElement();
            }

            if (!_inTag || _ptr >= _ceil || *_ptr != '/')
            {
                return;
            }

            while (_ptr < _ceil && *_ptr++ != '>')
            {
                ;
            }

            _inTag = false;
        }

        public bool ReadAttribute()
        {
            if (!_inTag)
            {
                return false;
            }

            SkipWhitespace();
            if (ReadString(_namePtr, _nameMax))
            {
                SkipWhitespace();
                if (_ptr < _ceil && *_ptr == '=')
                {
                    _ptr++;
                    if (ReadString(_valPtr, _valueMax))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool LeaveTag()
        {
            if (!_inTag)
            {
                return true;
            }

            while (_ptr < _ceil)
            {
                if (*_ptr == '/')
                {
                    return false;
                }

                if (*_ptr++ == '>')
                {
                    _inTag = false;
                    return true;
                }
            }

            return false;
        }

        public bool ReadValue(float* pOut)
        {
            if (!LeaveTag())
            {
                return false;
            }

            if (ReadString(_valPtr, _valueMax))
            {
                if (float.TryParse((string) Value, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat,
                    out float f))
                {
                    *pOut = f;
                    return true;
                }
            }

            return false;
        }

        public bool ReadValue(float* pOut, float scale)
        {
            if (!LeaveTag())
            {
                return false;
            }

            if (ReadString(_valPtr, _valueMax))
            {
                if (float.TryParse((string) Value, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat,
                    out float f))
                {
                    *pOut = f * scale;
                    return true;
                }
            }

            return false;
        }

        public bool ReadValue(int* pOut)
        {
            if (!LeaveTag())
            {
                return false;
            }

            if (ReadString(_valPtr, _valueMax))
            {
                if (int.TryParse((string) Value, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat,
                    out int f))
                {
                    *pOut = f;
                    return true;
                }
            }

            return false;
        }

        public bool ReadStringSingle()
        {
            if (!LeaveTag())
            {
                return false;
            }

            if (ReadString(_valPtr, _valueMax))
            {
                return true;
            }

            return false;
        }

        public string ReadElementString()
        {
            if (!LeaveTag())
            {
                return null;
            }

            int len = 0;
            while (len < _valueMax && _ptr < _ceil && *_ptr != '<')
            {
                _valPtr[len++] = *_ptr++;
            }

            _valPtr[len] = 0;
            return new string((sbyte*) _valPtr);
        }
    }
}