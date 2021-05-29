using BrawlLib.Imaging;
using BrawlLib.Internal;
using System;

namespace BrawlLib.SSBB
{
    public unsafe class ParameterValueManager
    {
        public UnsafeBuffer _values;

        public ParameterValueManager(VoidPtr address)
        {
            _values = new UnsafeBuffer(256);
            if (address == null)
            {
                byte* pOut = (byte*) _values.Address;
                for (int i = 0; i < 256; i++)
                {
                    *pOut++ = 0;
                }
            }
            else
            {
                byte* pIn = (byte*) address;
                byte* pOut = (byte*) _values.Address;
                for (int i = 0; i < 256; i++)
                {
                    *pOut++ = *pIn++;
                }
            }
        }

        ~ParameterValueManager()
        {
            _values.Dispose();
        }

        public float GetFloat(int index)
        {
            return ((bfloat*) _values.Address)[index];
        }

        public void SetFloat(int index, float value)
        {
            ((bfloat*) _values.Address)[index] = value;
        }

        public int GetInt(int index)
        {
            return ((bint*) _values.Address)[index];
        }

        public void SetInt(int index, int value)
        {
            ((bint*) _values.Address)[index] = value;
        }

        public short GetShort(int index, int index2)
        {
            return ((bshort*)_values.Address)[index * 2 + index2];
        }

        public void SetShort(int index, int index2, short value)
        {
            ((bshort*)_values.Address)[index * 2 + index2] = value;
        }

        public RGBAPixel GetRGBA(int index)
        {
            return ((RGBAPixel*) _values.Address)[index];
        }

        public void SetRGBA(int index, RGBAPixel value)
        {
            ((RGBAPixel*) _values.Address)[index] = value;
        }

        public byte GetByte(int index, int index2)
        {
            return ((byte*) _values.Address)[index * 4 + index2];
        }

        public void SetByte(int index, int index2, byte value)
        {
            ((byte*) _values.Address)[index * 4 + index2] = value;
        }

        public string GetHex(int index)
        {
            return "0x" + GetInt(index).ToString("X8");
        }

        public void SetHex(int index, string value)
        {
            SetInt(index, Convert.ToInt32(value, value.StartsWith("0x") ? 16 : 10));
        }
    }
}