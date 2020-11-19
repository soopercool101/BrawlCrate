using BrawlLib.Imaging;
using BrawlLib.Internal;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public interface IAttributeList
    {
        /// <summary>
        /// The number of (four-byte) entries - i.e. the length in bytes divided by four.
        /// </summary>
        int NumEntries { get; }

        void SetFloat(int index, float value);
        float GetFloat(int index);
        void SetDegrees(int index, float value);
        float GetDegrees(int index);
        void SetInt(int index, int value);
        int GetInt(int index);
        void SetRGBAPixel(int index, string value);
        void SetRGBAPixel(int index, RGBAPixel value);
        RGBAPixel GetRGBAPixel(int index);
        void SetHex(int index, string value);
        string GetHex(int index);
        void SetBytes(int index, params byte[] values);
        string GetBytes(int index);
        void SetShorts(int index, short value1, short value2);
        string GetShorts(int index);

        void SignalPropertyChange();
    }

    public interface MultipleInterpretationIAttributeList : IAttributeList
    {
        IEnumerable<AttributeInterpretation> GetPossibleInterpretations();
    }
}