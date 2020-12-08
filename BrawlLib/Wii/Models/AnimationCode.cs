using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Models
{
    //I = indexed/interpolated, L = linear
    public enum AnimDataFormat : byte
    {
        None = 0,
        I4 = 1,
        I6 = 2,
        I12 = 3,
        L1 = 4,
        L2 = 5,
        L4 = 6
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AnimationCode
    {
        public static AnimationCode Default = new AnimationCode {_data = 0x3FE07F};

        public Bin32 _data;

        //0000 0000 0000 0000 0000 0000 0000 0001       Always set.

        //0000 0000 0000 0000 0000 0000 0000 0010       Identity (Scale = 1, Rot = 0, Trans = 0)
        //0000 0000 0000 0000 0000 0000 0000 0100       Rot/Trans Zero (Rot = 0, Trans = 0) - Must be set if Identity is set
        //0000 0000 0000 0000 0000 0000 0000 1000       Scale One (Scale = 1) - Must be set if Identity is set

        //0000 0000 0000 0000 0000 0000 0001 0000       Scale isotropic
        //0000 0000 0000 0000 0000 0000 0010 0000       Rotation isotropic
        //0000 0000 0000 0000 0000 0000 0100 0000       Translation isotropic

        //0000 0000 0000 0000 0000 0000 1000 0000       Use Model Scale
        //0000 0000 0000 0000 0000 0001 0000 0000       Use Model Rotation
        //0000 0000 0000 0000 0000 0010 0000 0000       Use Model Translation

        //0000 0000 0000 0000 0000 0100 0000 0000       Scale Compensate Apply (Maya Calculations)
        //0000 0000 0000 0000 0000 1000 0000 0000       Scale Compensate Parent (Maya Calculations)
        //0000 0000 0000 0000 0001 0000 0000 0000       Classic Scale Off (SoftImage Calculations)

        //0000 0000 0000 0000 1110 0000 0000 0000       Scale fixed
        //0000 0000 0000 0111 0000 0000 0000 0000       Rotation fixed
        //0000 0000 0011 1000 0000 0000 0000 0000       Translation fixed

        //0000 0000 0100 0000 0000 0000 0000 0000       Scale exists (Equivalent to Use Model Scale & Scale One set to false)
        //0000 0000 1000 0000 0000 0000 0000 0000       Rotation exists (Equivalent to Use Model Rotation & Rotation Zero set to false)
        //0000 0001 0000 0000 0000 0000 0000 0000       Translation exists (Equivalent to Use Model Translation & Translation Zero set to false)

        //0000 0110 0000 0000 0000 0000 0000 0000       Scale format
        //0011 1000 0000 0000 0000 0000 0000 0000       Rotation format
        //1100 0000 0000 0000 0000 0000 0000 0000       Translation format

        public bool AlwaysSet
        {
            get => _data[0];
            set => _data[0] = value;
        }

        public bool Identity
        {
            get => _data[1];
            set => _data[1] = value;
        }

        public bool IgnoreRotAndTrans
        {
            get => _data[2];
            set => _data[2] = value;
        }

        public bool IgnoreScale
        {
            get => _data[3];
            set => _data[3] = value;
        }

        public bool IsScaleIsotropic
        {
            get => _data[4];
            set => _data[4] = value;
        }

        public bool IsRotationIsotropic
        {
            get => _data[5];
            set => _data[5] = value;
        }

        public bool IsTranslationIsotropic
        {
            get => _data[6];
            set => _data[6] = value;
        }

        public bool UseModelScale
        {
            get => _data[7];
            set => _data[7] = value;
        }

        public bool UseModelRot
        {
            get => _data[8];
            set => _data[8] = value;
        }

        public bool UseModelTrans
        {
            get => _data[9];
            set => _data[9] = value;
        }

        public bool ScaleCompApply
        {
            get => _data[10];
            set => _data[10] = value;
        }

        public bool ScaleCompParent
        {
            get => _data[11];
            set => _data[11] = value;
        }

        public bool ClassicScaleOff
        {
            get => _data[12];
            set => _data[12] = value;
        }

        public bool IsScaleXFixed
        {
            get => _data[13];
            set => _data[13] = value;
        }

        public bool IsScaleYFixed
        {
            get => _data[14];
            set => _data[14] = value;
        }

        public bool IsScaleZFixed
        {
            get => _data[15];
            set => _data[15] = value;
        }

        public bool IsRotationXFixed
        {
            get => _data[16];
            set => _data[16] = value;
        }

        public bool IsRotationYFixed
        {
            get => _data[17];
            set => _data[17] = value;
        }

        public bool IsRotationZFixed
        {
            get => _data[18];
            set => _data[18] = value;
        }

        public bool IsTranslationXFixed
        {
            get => _data[19];
            set => _data[19] = value;
        }

        public bool IsTranslationYFixed
        {
            get => _data[20];
            set => _data[20] = value;
        }

        public bool IsTranslationZFixed
        {
            get => _data[21];
            set => _data[21] = value;
        }

        public bool HasScale
        {
            get => _data[22];
            set => _data[22] = value;
        }

        public bool HasRotation
        {
            get => _data[23];
            set => _data[23] = value;
        }

        public bool HasTranslation
        {
            get => _data[24];
            set => _data[24] = value;
        }

        public AnimDataFormat ScaleDataFormat
        {
            get => (AnimDataFormat) _data[25, 2];
            set => _data[25, 2] = (uint) value;
        }

        public AnimDataFormat RotationDataFormat
        {
            get => (AnimDataFormat) _data[27, 3];
            set => _data[27, 3] = (uint) value;
        }

        public AnimDataFormat TranslationDataFormat
        {
            get => (AnimDataFormat) _data[30, 2];
            set => _data[30, 2] = (uint) value;
        }

        //public bool HasScale { get { return (_data & 0x400000) != 0; } set { _data = (_data & 0xFFBFFFFF) | (value ? (uint)0x400000 : 0); } }
        //public bool IsScaleIsotropic { get { return (_data & 0x10) != 0; } set { _data = (_data & 0xFFFFFFEF) | (value ? (uint)0x10 : 0); } }
        ////public bool IsIsotropicFixed { get { return (_data & 0xE000) != 0; } }
        //public bool IsScaleXFixed { get { return (_data & 0x2000) != 0; } set { _data = (_data & 0xFFFFDFFF) | ((value) ? (uint)0x2000 : 0); } }
        //public bool IsScaleYFixed { get { return (_data & 0x4000) != 0; } set { _data = (_data & 0xFFFFBFFF) | ((value) ? (uint)0x4000 : 0); } }
        //public bool IsScaleZFixed { get { return (_data & 0x8000) != 0; } set { _data = (_data & 0xFFFF7FFF) | ((value) ? (uint)0x8000 : 0); } }
        //public AnimDataFormat ScaleDataFormat { get { return (AnimDataFormat)((_data >> 25) & 3); } set { _data = (_data & 0xF9FFFFFF) | ((uint)value << 25); } }
        //public bool IgnoreScale { get { return (_data & 0x8) != 0; } set { _data = (_data & 0xFFFFFFF7) | (value ? (uint)8 : 0); } }

        //public bool HasRotation { get { return (_data & 0x800000) != 0; } set { _data = (_data & 0xFF7FFFFF) | (value ? (uint)0x800000 : 0); } }
        //public bool IsRotationIsotropic { get { return (_data & 0x20) != 0; } set { _data = (_data & 0xFFFFFFDF) | (value ? (uint)0x20 : 0); } }
        //public bool IsRotationXFixed { get { return (_data & 0x10000) != 0; } set { _data = (_data & 0xFFFEFFFF) | ((value) ? (uint)0x10000 : 0); } }
        //public bool IsRotationYFixed { get { return (_data & 0x20000) != 0; } set { _data = (_data & 0xFFFDFFFF) | ((value) ? (uint)0x20000 : 0); } }
        //public bool IsRotationZFixed { get { return (_data & 0x40000) != 0; } set { _data = (_data & 0xFFFBFFFF) | ((value) ? (uint)0x40000 : 0); } }
        //public AnimDataFormat RotationDataFormat { get { return (AnimDataFormat)((_data >> 27) & 7); } set { _data = (_data & 0xC7FFFFFF) | ((uint)value << 27); } }

        //public bool HasTranslation { get { return (_data & 0x1000000) != 0; } set { _data = (_data & 0xFEFFFFFF) | (value ? (uint)0x1000000 : 0); } }
        //public bool IsTranslationIsotropic { get { return (_data & 0x40) != 0; } set { _data = (_data & 0xFFFFFFBF) | (value ? (uint)0x40 : 0); } }
        //public bool IsTranslationXFixed { get { return (_data & 0x080000) != 0; } set { _data = (_data & 0xFFF7FFFF) | ((value) ? (uint)0x080000 : 0); } }
        //public bool IsTranslationYFixed { get { return (_data & 0x100000) != 0; } set { _data = (_data & 0xFFEFFFFF) | ((value) ? (uint)0x100000 : 0); } }
        //public bool IsTranslationZFixed { get { return (_data & 0x200000) != 0; } set { _data = (_data & 0xFFDFFFFF) | ((value) ? (uint)0x200000 : 0); } }
        //public AnimDataFormat TranslationDataFormat { get { return (AnimDataFormat)(_data >> 30); } set { _data = (_data & 0x3FFFFFFF) | ((uint)value << 30); } }

        //public int ExtraData { get { return (int)(_data & 0x6); } set { _data = (_data & 0xFFFFFFF9) | (uint)(value << 1); } }

        //public bool AlwaysOn { get { return (_data & 1) != 0; } set { _data = (_data & 0xFFFFFFFE) | ((value) ? (uint)1 : 0); } }

        public static implicit operator AnimationCode(uint data)
        {
            return new AnimationCode {_data = data};
        }

        public static implicit operator uint(AnimationCode code)
        {
            return code._data;
        }

        public bool GetIsFixed(int i)
        {
            return _data[13 + i];
        }

        public void SetIsFixed(int i, bool p)
        {
            _data[13 + i] = p;
        }

        public bool GetIsIsotropic(int i)
        {
            return _data[4 + i];
        }

        public void SetIsIsotropic(int i, bool p)
        {
            _data[4 + i] = p;
        }

        public bool GetExists(int i)
        {
            return _data[22 + i];
        }

        public void SetExists(int i, bool p)
        {
            _data[22 + i] = p;
        }

        public AnimDataFormat GetFormat(int index)
        {
            switch (index)
            {
                case 0: return ScaleDataFormat;
                case 1: return RotationDataFormat;
                case 2: return TranslationDataFormat;
            }

            return AnimDataFormat.None;
        }

        public void SetFormat(int index, AnimDataFormat format)
        {
            switch (index)
            {
                case 0:
                    ScaleDataFormat = format;
                    break;
                case 1:
                    RotationDataFormat = format;
                    break;
                case 2:
                    TranslationDataFormat = format;
                    break;
            }
        }

        public override string ToString()
        {
            //sbyte* buffer = stackalloc sbyte[39];

            //uint data = _data;
            //for (int i = 38; i >= 0; i--)
            //{
            //    if (((i + 1) % 5) == 0)
            //        buffer[i] = 0x20;
            //    else
            //    {
            //        buffer[i] = (sbyte)(((data & 1) == 0) ? 0x30 : 0x31);
            //        data >>= 1;
            //    }
            //}

            //return new string(buffer, 0, 39);

            return _data.ToString();

            //return String.Format("S:{0}; R:{1}; T:{2}; UMS:{3} UMR:{4} UMT:{5}", ScaleDataFormat.ToString(), RotationDataFormat.ToString(), TranslationDataFormat.ToString(), UseModelScale, UseModelRot, UseModelTrans);
        }
    }
}