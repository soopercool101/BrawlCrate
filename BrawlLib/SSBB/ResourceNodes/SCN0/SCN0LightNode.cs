using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Animations;
using BrawlLib.Wii.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SCN0LightNode : SCN0EntryNode, IBoolArraySource, IColorSource, IKeyframeSource
    {
        internal SCN0Light* Data => (SCN0Light*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.SCN0Light;

        #region Variables

        //Header variables
        internal int _nonSpecLightId, _distFunc, _spotFunc;
        internal FixedFlags _fixedFlags = (FixedFlags) 0xFFF8;
        internal Bin16 _typeUsageFlags = 0x35;

        //Visibility array
        internal byte[] _data = new byte[0];
        internal int _entryCount;

        //Color arrays
        public int[] _numEntries = new int[] {0, 0};
        public RGBAPixel[] _solidColors = new RGBAPixel[2];

        private List<RGBAPixel>
            _lightColor = new List<RGBAPixel>(),
            _specColor = new List<RGBAPixel>();

        public bool[] _constants = new bool[] {true, true};

        #endregion

        #region User Editable Variables

        [Category("User Data")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public UserDataCollection UserEntries
        {
            get => _userEntries;
            set
            {
                _userEntries = value;
                SignalPropertyChange();
            }
        }

        internal UserDataCollection _userEntries = new UserDataCollection();

        [Category("SCN0 Entry")]
        public int NonSpecularLightID
        {
            get
            {
                if (!SpecularEnabled)
                {
                    return 0;
                }

                int i = 0;
                foreach (SCN0LightNode n in Parent.Children)
                {
                    if (n.Index == Index)
                    {
                        return Parent.Children.Count + i;
                    }

                    if (n.SpecularEnabled && n.Index != Index)
                    {
                        i++;
                    }
                }

                return 0;
            }
        }

        [Category("Light")]
        public LightType LightType
        {
            get => (LightType) _typeUsageFlags[0, 2];
            set
            {
                _typeUsageFlags[0, 2] = (ushort) value;
                SignalPropertyChange();
            }
        }

        [Category("Light")]
        public bool ColorEnabled
        {
            get => UsageFlags.HasFlag(UsageFlags.ColorEnabled);
            set
            {
                if (value)
                {
                    UsageFlags |= UsageFlags.ColorEnabled;
                }
                else
                {
                    UsageFlags &= ~UsageFlags.ColorEnabled;
                }

                SignalPropertyChange();
            }
        }

        [Category("Light")]
        public bool AlphaEnabled
        {
            get => UsageFlags.HasFlag(UsageFlags.AlphaEnabled);
            set
            {
                if (value)
                {
                    UsageFlags |= UsageFlags.AlphaEnabled;
                }
                else
                {
                    UsageFlags &= ~UsageFlags.AlphaEnabled;
                }

                SignalPropertyChange();
            }
        }

        [Category("Light")]
        public bool SpecularEnabled
        {
            get => UsageFlags.HasFlag(UsageFlags.SpecularEnabled);
            set
            {
                if (value)
                {
                    UsageFlags |= UsageFlags.SpecularEnabled;
                }
                else
                {
                    UsageFlags &= ~UsageFlags.SpecularEnabled;
                }

                SignalPropertyChange();
            }
        }

        [Category("Source Light")]
        public DistAttnFn DistanceFunction
        {
            get => (DistAttnFn) _distFunc;
            set
            {
                _distFunc = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("Spotlight")]
        public SpotFn SpotFunction
        {
            get => (SpotFn) _spotFunc;
            set
            {
                _spotFunc = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("Light Colors")]
        public bool ConstantColor
        {
            get => _constants[0];
            set
            {
                if (_constants[0] != value)
                {
                    _constants[0] = value;
                    if (_constants[0])
                    {
                        MakeSolid(new ARGBPixel(), 0);
                    }
                    else
                    {
                        MakeList(0);
                    }

                    UpdateCurrentControl();
                }
            }
        }

        [Category("Light Colors")]
        public bool ConstantSpecular
        {
            get => _constants[1];
            set
            {
                if (_constants[1] != value)
                {
                    _constants[1] = value;
                    if (_constants[1])
                    {
                        MakeSolid(new ARGBPixel(), 1);
                    }
                    else
                    {
                        MakeList(1);
                    }

                    UpdateCurrentControl();
                }
            }
        }

        [Category("Light Enable")]
        public bool Constant
        {
            get => ConstantVisibility;
            set
            {
                if (value != ConstantVisibility)
                {
                    if (value)
                    {
                        MakeConstant(true);
                    }
                    else
                    {
                        MakeAnimated();
                    }

                    UpdateCurrentControl();
                }
            }
        }

        [Category("Light Enable")]
        public bool Enabled
        {
            get => VisibilityEnabled;
            set
            {
                VisibilityEnabled = value;
                SignalPropertyChange();
            }
        }

        #endregion

        #region Flags

        [Browsable(false)]
        public UsageFlags UsageFlags
        {
            get => (UsageFlags) _typeUsageFlags[2, 4];
            set
            {
                _typeUsageFlags[2, 4] = (ushort) value;
                SignalPropertyChange();
            }
        }

        [Browsable(false)]
        public bool ConstantVisibility
        {
            get => _fixedFlags.HasFlag(FixedFlags.EnabledConstant);
            set
            {
                if (value)
                {
                    _fixedFlags |= FixedFlags.EnabledConstant;
                }
                else
                {
                    _fixedFlags &= ~FixedFlags.EnabledConstant;
                }
            }
        }

        [Browsable(false)]
        public bool VisibilityEnabled
        {
            get => UsageFlags.HasFlag(UsageFlags.Enabled);
            set
            {
                if (value)
                {
                    UsageFlags |= UsageFlags.Enabled;
                }
                else
                {
                    UsageFlags &= ~UsageFlags.Enabled;
                }
            }
        }

        private static readonly FixedFlags[] _ordered = new FixedFlags[]
        {
            FixedFlags.StartXConstant,
            FixedFlags.StartYConstant,
            FixedFlags.StartZConstant,
            FixedFlags.EndXConstant,
            FixedFlags.EndYConstant,
            FixedFlags.EndZConstant,
            FixedFlags.RefDistanceConstant,
            FixedFlags.RefBrightnessConstant,
            FixedFlags.CutoffConstant,
            FixedFlags.ShininessConstant
        };

        [Flags]
        public enum FixedFlags : ushort
        {
            StartXConstant = 0x8,
            StartYConstant = 0x10,
            StartZConstant = 0x20,
            ColorConstant = 0x40,
            EnabledConstant = 0x80, //Refer to Enabled in UsageFlags if constant
            EndXConstant = 0x100,
            EndYConstant = 0x200,
            EndZConstant = 0x400,
            CutoffConstant = 0x800,
            RefDistanceConstant = 0x1000,
            RefBrightnessConstant = 0x2000,
            SpecColorConstant = 0x4000,
            ShininessConstant = 0x8000
        }

        #endregion

        #region Light Color

        public List<RGBAPixel> GetColors(int i)
        {
            switch (i)
            {
                case 0: return _lightColor;
                case 1: return _specColor;
            }

            return null;
        }

        public void SetColors(int i, List<RGBAPixel> value)
        {
            switch (i)
            {
                case 0:
                    _lightColor = value;
                    break;
                case 1:
                    _specColor = value;
                    break;
            }
        }

        public void MakeSolid(ARGBPixel color, int id)
        {
            _numEntries[id] = 0;
            _constants[id] = true;
            _solidColors[id] = color;
            SignalPropertyChange();
        }

        public void MakeList(int id)
        {
            _constants[id] = false;
            int entries = Scene.FrameCount;
            _numEntries[id] = GetColors(id).Count;
            SetNumEntries(id, entries);
        }

        [Browsable(false)]
        internal void SetNumEntries(int id, int value)
        {
            //if (_numEntries[id] == 0)
            //    return;

            if (value > _numEntries[id])
            {
                ARGBPixel p = _numEntries[id] > 0
                    ? (ARGBPixel) GetColors(id)[_numEntries[id] - 1]
                    : new ARGBPixel(255, 0, 0, 0);
                for (int i = value - _numEntries[id]; i-- > 0;)
                {
                    GetColors(id).Add(p);
                }
            }
            else if (value < GetColors(id).Count)
            {
                GetColors(id).RemoveRange(value, GetColors(id).Count - value);
            }

            _numEntries[id] = value;
        }

        #endregion

        [Browsable(false)] public int FrameCount => Keyframes.FrameLimit;

        internal void SetSize(int numFrames, bool looped)
        {
            _numEntries[0] = GetColors(0).Count;
            _numEntries[1] = GetColors(1).Count;

            SetNumEntries(0, numFrames);
            SetNumEntries(1, numFrames);

            if (_constants[0])
            {
                _numEntries[0] = 0;
            }

            if (_constants[1])
            {
                _numEntries[1] = 0;
            }

            Keyframes.FrameLimit = numFrames + (looped ? 1 : 0);

            _entryCount = numFrames;
            int numBytes = Math.Min(_entryCount.Align(32) / 8, _data.Length);
            Array.Resize(ref _data, numBytes);
        }

        public override bool OnInitialize()
        {
            //Read common header
            base.OnInitialize();

            //Initialize defaults
            _numEntries = new int[] {0, 0};
            _solidColors = new RGBAPixel[2];
            _constants = new bool[] {true, true};

            //Read header values
            _nonSpecLightId = Data->_nonSpecLightId;
            _fixedFlags = (FixedFlags) (ushort) Data->_fixedFlags;
            _typeUsageFlags = (ushort) Data->_usageFlags;
            _distFunc = Data->_distFunc;
            _spotFunc = Data->_spotFunc;

            //Read user data
            (_userEntries = new UserDataCollection()).Read(Data->UserData, WorkingUncompressed);

            //Don't bother reading data if the entry is null
            if (Name == "<null>")
            {
                return false;
            }

            //Read light visibility array
            if (!_fixedFlags.HasFlag(FixedFlags.EnabledConstant) && !_replaced)
            {
                _entryCount = Scene.FrameCount;
                int numBytes = _entryCount.Align(32) / 8;

                _data = new byte[numBytes];
                Marshal.Copy((IntPtr) Data->VisBitEntries, _data, 0, numBytes);
            }
            else
            {
                _entryCount = 0;
                _data = new byte[0];
            }

            //Read light color
            ReadColors(
                (uint) _fixedFlags,
                (uint) FixedFlags.ColorConstant,
                ref _solidColors[0],
                ref _lightColor,
                Scene.FrameCount,
                Data->_lightColor.Address,
                ref _constants[0],
                ref _numEntries[0]);

            if (!SpecularEnabled)
            {
                _constants[1] = (_fixedFlags & FixedFlags.SpecColorConstant) != 0;
                return false;
            }

            //Read light specular color
            ReadColors(
                (uint) _fixedFlags,
                (uint) FixedFlags.SpecColorConstant,
                ref _solidColors[1],
                ref _specColor,
                Scene.FrameCount,
                Data->_specularColor.Address,
                ref _constants[1],
                ref _numEntries[1]);

            return false;
        }

        private readonly SCN0LightNode[] _matches = {null, null};

        public override int OnCalculateSize(bool force)
        {
            _matches[0] = null;
            _matches[1] = null;

            for (int i = 0; i < 3; i++)
            {
                _dataLengths[i] = 0;
            }

            if (_name == "<null>")
            {
                return SCN0Light.Size;
            }

            for (int i = 0; i < Keyframes.ArrayCount; i++)
            {
                CalcKeyLen(Keyframes[i]);
            }

            for (int i = 0; i < 2; i++)
            {
                if (i == 1 && !SpecularEnabled)
                {
                    break;
                }

                _matches[i] = FindColorMatch(_constants[i], Scene.FrameCount, i) as SCN0LightNode;
                if (_matches[i] == null && !_constants[i])
                {
                    _dataLengths[1] += 4 * (FrameCount + 1);
                }
            }

            if (!ConstantVisibility)
            {
                _dataLengths[2] += _entryCount.Align(32) / 8;
            }

            //If this light uses specular lighting, 
            //increment SCN0 specular light count
            if (SpecularEnabled && Scene != null)
            {
                Scene._specLights++;
            }

            return SCN0Light.Size;
        }

        private VoidPtr _lightAddress, _specularAddress;

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            //Build common header
            base.OnRebuild(address, length, force);

            //Reset addresses
            _lightAddress = null;
            _specularAddress = null;

            //Don't write anything if this node is null
            if (_name == "<null>")
            {
                return;
            }

            //Write header information
            SCN0Light* header = (SCN0Light*) address;
            header->_nonSpecLightId = NonSpecularLightID;
            header->_userDataOffset = 0;
            header->_distFunc = _distFunc;
            header->_spotFunc = _spotFunc;

            int newFlags = 0;

            //Encode keyframe data
            for (int i = 0, index = 0; i < 14; i++)
            {
                if (!(i == 3 || i == 7 || i == 10 || i == 12))
                {
                    _dataAddrs[0] += EncodeKeyframes(
                        Keyframes[index],
                        _dataAddrs[0],
                        header->_startPoint._x.Address + i * 4,
                        ref newFlags,
                        (int) _ordered[index++]);
                }
            }

            _dataAddrs[1] += WriteColors(
                ref newFlags,
                (int) FixedFlags.ColorConstant,
                _solidColors[0],
                _lightColor,
                _constants[0],
                FrameCount,
                header->_lightColor.Address,
                ref _lightAddress,
                _matches[0] == null ? null : _matches[0]._lightAddress,
                (RGBAPixel*) _dataAddrs[1]);

            //Only bother writing if specular is enabled
            if (SpecularEnabled)
            {
                _dataAddrs[1] += WriteColors(
                    ref newFlags,
                    (int) FixedFlags.SpecColorConstant,
                    _solidColors[1],
                    _specColor,
                    _constants[1],
                    FrameCount,
                    header->_specularColor.Address,
                    ref _specularAddress,
                    _matches[1] == null ? null : _matches[1]._specularAddress,
                    (RGBAPixel*) _dataAddrs[1]);
            }
            else
            {
                //The value is set to 0
                header->_specularColor = new RGBAPixel();

                //The flag, while unused, seems to be set to the same state as the color constant flag
                if (_constants[0])
                {
                    newFlags |= (int) FixedFlags.SpecColorConstant;
                }
                else
                {
                    newFlags &= (int) ~FixedFlags.SpecColorConstant;
                }
            }

            if (!ConstantVisibility && _entryCount != 0)
            {
                header->_visOffset = (int) _dataAddrs[2] - (int) header->_visOffset.Address;
                Marshal.Copy(_data, 0, _dataAddrs[2], _data.Length);
                _dataAddrs[2] = _dataAddrs[2] + EntryCount.Align(32) / 8;
            }
            else
            {
                newFlags |= (int) FixedFlags.EnabledConstant;
            }

            //Set newly calculated flags
            header->_fixedFlags = (ushort) (_fixedFlags = (FixedFlags) newFlags);
            header->_usageFlags = _typeUsageFlags._data;
        }

        protected internal override void PostProcess(VoidPtr scn0Address, VoidPtr dataAddress, StringTable stringTable)
        {
            base.PostProcess(scn0Address, dataAddress, stringTable);
        }

        #region IColorSource Members

        public bool HasPrimary(int id)
        {
            return false;
        }

        public ARGBPixel GetPrimaryColor(int id)
        {
            return new ARGBPixel();
        }

        public void SetPrimaryColor(int id, ARGBPixel color)
        {
        }

        [Browsable(false)]
        public string PrimaryColorName(int id)
        {
            return null;
        }

        [Browsable(false)] public int TypeCount => _numEntries.Length;

        [Browsable(false)]
        public int ColorCount(int id)
        {
            return _numEntries[id] == 0 ? 1 : _numEntries[id];
        }

        public ARGBPixel GetColor(int index, int id)
        {
            return _numEntries[id] == 0 ? (ARGBPixel) _solidColors[id] : (ARGBPixel) GetColors(id)[index];
        }

        public void SetColor(int index, int id, ARGBPixel color)
        {
            if (_numEntries[id] == 0)
            {
                _solidColors[id] = color;
            }
            else
            {
                GetColors(id)[index] = color;
            }

            SignalPropertyChange();
        }

        public bool GetColorConstant(int id)
        {
            switch (id)
            {
                case 0: return ConstantColor;
                case 1: return ConstantSpecular;
            }

            return false;
        }

        public void SetColorConstant(int id, bool constant)
        {
            switch (id)
            {
                case 0:
                    ConstantColor = constant;
                    break;
                case 1:
                    ConstantSpecular = constant;
                    break;
            }
        }

        #endregion

        #region IBoolArraySource Members

        [Browsable(false)]
        public int EntryCount
        {
            get => _entryCount;
            set
            {
                if (_entryCount == 0)
                {
                    return;
                }

                _entryCount = value;
                int len = value.Align(32) / 8;

                if (_data.Length < len)
                {
                    byte[] newArr = new byte[len];
                    Array.Copy(_data, newArr, _data.Length);
                    _data = newArr;
                }

                SignalPropertyChange();
            }
        }

        public bool GetEntry(int index)
        {
            if (_data.Length == 0)
            {
                return Enabled;
            }

            int i = index >> 3;
            int bit = 1 << (7 - (index & 0x7));
            return (_data[i] & bit) != 0;
        }

        public void SetEntry(int index, bool value)
        {
            int i = index >> 3;
            int bit = 1 << (7 - (index & 0x7));
            int mask = ~bit;
            _data[i] = (byte) ((_data[i] & mask) | (value ? bit : 0));
            SignalPropertyChange();
        }

        public void MakeConstant(bool value)
        {
            ConstantVisibility = true;
            VisibilityEnabled = value;
            _entryCount = 0;

            SignalPropertyChange();
        }

        public void MakeAnimated()
        {
            bool enabled = VisibilityEnabled;

            ConstantVisibility = false;
            VisibilityEnabled = false;

            _entryCount = -1;
            EntryCount = FrameCount + 1;

            if (enabled)
            {
                for (int i = 0; i < _entryCount; i++)
                {
                    SetEntry(i, true);
                }
            }

            SignalPropertyChange();
        }

        #endregion

        #region Keyframe Management

        public static bool _generateTangents = true;

        public GLSLLightFrame GetGLSLAnimFrame(float index)
        {
            bool enabled = GetEnabled(index);

            //No point calculating anything if the light isn't used
            if (!enabled)
            {
                return new GLSLLightFrame();
            }

            Vector4 color = new Vector4(0.0f),
                specColor = new Vector4(0.0f);
            Vector3
                pos = GetStart(index),
                aim = GetEnd(index),
                diffAttnK = LightType != LightType.Directional
                    ? GetLightDistCoefs(index)
                    : new Vector3(1.0f, 0.0f, 0.0f),
                diffAttnA = LightType == LightType.Spotlight ? GetLightSpotCoefs(index) : new Vector3(1.0f, 0.0f, 0.0f),
                specAttnK = SpecularEnabled ? GetSpecDistCoefs(index) : new Vector3(1.0f, 0.0f, 0.0f);

            //No point calculating colors if neither color nor alpha is used
            if (ColorEnabled || AlphaEnabled)
            {
                if (ConstantColor)
                {
                    color = _solidColors[0];
                }
                else
                {
                    //Interpolate the color, in case the index isn't an integer

                    int colorIndex = (int) Math.Truncate(index);
                    color = _lightColor[colorIndex.Clamp(0, _lightColor.Count - 1)];
                    if (colorIndex + 1 < _lightColor.Count)
                    {
                        float frac = index - colorIndex;
                        Vector4 interp = _lightColor[colorIndex + 1];
                        color += (interp - color) * frac;
                    }
                }

                if (SpecularEnabled)
                {
                    if (ConstantSpecular)
                    {
                        specColor = _solidColors[1];
                    }
                    else
                    {
                        //Interpolate the color, in case the index isn't an integer

                        int specIndex = (int) Math.Truncate(index);
                        specColor = _specColor[specIndex.Clamp(0, _specColor.Count - 1)];
                        if (specIndex + 1 < _specColor.Count)
                        {
                            float frac = index - specIndex;
                            Vector4 interp = _specColor[specIndex + 1];
                            specColor += (interp - specColor) * frac;
                        }
                    }
                }

                if (!ColorEnabled)
                {
                    color._x = 0.0f;
                    color._y = 0.0f;
                    color._z = 0.0f;
                    specColor._x = 0.0f;
                    specColor._y = 0.0f;
                    specColor._z = 0.0f;
                }

                if (!AlphaEnabled)
                {
                    color._w = 0.0f;
                    specColor._w = 0.0f;
                }
            }

            return new GLSLLightFrame(
                enabled,
                LightType,
                pos,
                aim,
                color,
                diffAttnK,
                diffAttnA,
                SpecularEnabled,
                specColor,
                specAttnK);
        }

        public bool GetEnabled(float frame)
        {
            return !VisibilityEnabled ? false :
                ConstantVisibility ? UsageFlags.HasFlag(UsageFlags.Enabled) :
                GetEntry((int) frame);
        }

        public LightAnimationFrame GetAnimFrame(float index)
        {
            LightAnimationFrame frame;
            float* dPtr = (float*) &frame;
            for (int x = 0; x < Keyframes.ArrayCount; x++)
            {
                KeyframeArray a = Keyframes[x];
                *dPtr++ = a.GetFrameValue(index);
                frame.SetBools(x, a.GetKeyframe((int) index) != null);
                frame.Index = index;
            }

            frame.Enabled = GetEnabled(index);

            if (ConstantColor)
            {
                frame.Color = _solidColors[0];
            }
            else if (_lightColor.Count > 0)
            {
                int colorIndex = ((int) Math.Truncate(index)).Clamp(0, _lightColor.Count - 1);
                Vector4 color = _lightColor[colorIndex];
                if (colorIndex + 1 < _lightColor.Count)
                {
                    float frac = index - colorIndex;
                    Vector4 interp = _lightColor[colorIndex + 1];
                    color += (interp - color) * frac;
                }

                frame.Color = color;
            }

            if (SpecularEnabled)
            {
                if (ConstantSpecular)
                {
                    frame.SpecularColor = _solidColors[1];
                }
                else if (_specColor.Count > 0)
                {
                    int specIndex = ((int) Math.Truncate(index)).Clamp(0, _specColor.Count - 1);
                    Vector4 specColor = _specColor[specIndex];
                    if (specIndex + 1 < _specColor.Count)
                    {
                        float frac = index - specIndex;
                        Vector4 interp = _specColor[specIndex + 1];
                        specColor += (interp - specColor) * frac;
                    }

                    frame.SpecularColor = specColor;
                }
            }

            return frame;
        }

        public float GetFrameValue(LightKeyframeMode keyFrameMode, float index)
        {
            return Keyframes[(int) keyFrameMode].GetFrameValue(index);
        }

        public KeyframeEntry GetKeyframe(LightKeyframeMode keyFrameMode, int index)
        {
            return Keyframes[(int) keyFrameMode].GetKeyframe(index);
        }

        public void RemoveKeyframe(LightKeyframeMode keyFrameMode, int index)
        {
            KeyframeEntry k = Keyframes[(int) keyFrameMode].Remove(index);
            if (k != null && _generateTangents)
            {
                k._prev.GenerateTangent();
                k._next.GenerateTangent();
                SignalPropertyChange();
            }
        }

        public void SetKeyframe(LightKeyframeMode keyFrameMode, int index, float value)
        {
            KeyframeArray keys = Keyframes[(int) keyFrameMode];
            bool exists = keys.GetKeyframe(index) != null;
            KeyframeEntry k = keys.SetFrameValue(index, value);

            if (!exists && !_generateTangents)
            {
                k.GenerateTangent();
            }

            if (_generateTangents)
            {
                k.GenerateTangent();
                k._prev.GenerateTangent();
                k._next.GenerateTangent();
            }

            SignalPropertyChange();
        }

        public Vector3 GetStart(float frame)
        {
            return new Vector3(
                StartX.GetFrameValue(frame),
                StartY.GetFrameValue(frame),
                StartZ.GetFrameValue(frame));
        }

        public Vector3 GetEnd(float frame)
        {
            return new Vector3(
                EndX.GetFrameValue(frame),
                EndY.GetFrameValue(frame),
                EndZ.GetFrameValue(frame));
        }

        public Vector3 GetLightSpotCoefs(float frame)
        {
            return GetLightSpotCoefs(SpotCut.GetFrameValue(frame), SpotFunction);
        }

        public static Vector3 GetLightSpotCoefs(float cutoff, SpotFn spotFunc)
        {
            //a2x^2 + a1x + a0
            float a0, a1, a2, d, cr = (float) Math.Cos(cutoff * Maths._deg2radf);

            if (cutoff <= 0.0f || cutoff > 90.0f)
            {
                spotFunc = SpotFn.Off;
            }

            switch (spotFunc)
            {
                case SpotFn.Flat:
                    a0 = -1000.0f * cr;
                    a1 = 1000.0f;
                    a2 = 0.0f;
                    break;
                case SpotFn.Cos:
                    a0 = -cr / (1.0f - cr);
                    a1 = 1.0f / (1.0f - cr);
                    a2 = 0.0f;
                    break;
                case SpotFn.Cos2:
                    a0 = 0.0f;
                    a1 = -cr / (1.0f - cr);
                    a2 = 1.0f / (1.0f - cr);
                    break;
                case SpotFn.Sharp:
                    d = (1.0f - cr) * (1.0f - cr);
                    a0 = cr * (cr - 2.0f) / d;
                    a1 = 2.0f / d;
                    a2 = -1.0f / d;
                    break;
                case SpotFn.Ring:
                    d = (1.0f - cr) * (1.0f - cr);
                    a0 = -4.0f * cr / d;
                    a1 = 4.0f * (1.0f + cr) / d;
                    a2 = -4.0f / d;
                    break;
                case SpotFn.Ring2:
                    d = (1.0f - cr) * (1.0f - cr);
                    a0 = 1.0f - 2.0f * cr * cr / d;
                    a1 = 4.0f * cr / d;
                    a2 = -2.0f / d;
                    break;
                case SpotFn.Off:
                default:
                    a0 = 1.0f;
                    a1 = 0.0f;
                    a2 = 0.0f;
                    break;
            }

            return new Vector3(a0, a1, a2);
        }

        public Vector3 GetLightDistCoefs(float frame)
        {
            return GetLightDistCoefs(RefDist.GetFrameValue(frame), RefBright.GetFrameValue(frame), DistanceFunction);
        }

        public Vector3 GetSpecDistCoefs(float frame)
        {
            return GetSpecShineDistCoefs(SpecShininess.GetFrameValue(frame));
        }

        public static Vector3 GetSpecShineDistCoefs(float shininess)
        {
            return new Vector3(shininess / 2.0f, 0.0f, 1.0f - shininess / 2.0f);
        }

        public static Vector3 GetLightDistCoefs(float distance, float brightness, DistAttnFn distFunc)
        {
            //constant attn, linear attn, quadratic attn
            //k2x^2 + k1x + k0
            float k0, k1, k2;

            if (distance < 0.0F || brightness <= 0.0F || brightness >= 1.0F)
            {
                distFunc = DistAttnFn.Off;
            }

            switch (distFunc)
            {
                case DistAttnFn.Gentle:
                    k0 = 1.0f;
                    k1 = (1.0f - brightness) / (brightness * distance);
                    k2 = 0.0f;
                    break;
                case DistAttnFn.Medium:
                    k0 = 1.0f;
                    k1 = 0.5f * (1.0f - brightness) / (brightness * distance);
                    k2 = 0.5f * (1.0f - brightness) / (brightness * distance * distance);
                    break;
                case DistAttnFn.Steep:
                    k0 = 1.0f;
                    k1 = 0.0f;
                    k2 = (1.0f - brightness) / (brightness * distance * distance);
                    break;
                case DistAttnFn.Off:
                default:
                    k0 = 1.0f;
                    k1 = 0.0f;
                    k2 = 0.0f;
                    break;
            }

            return new Vector3(k0, k1, k2);
        }

        [Browsable(false)] public KeyframeArray StartX => Keyframes[0];
        [Browsable(false)] public KeyframeArray StartY => Keyframes[1];
        [Browsable(false)] public KeyframeArray StartZ => Keyframes[2];
        [Browsable(false)] public KeyframeArray EndX => Keyframes[3];
        [Browsable(false)] public KeyframeArray EndY => Keyframes[4];
        [Browsable(false)] public KeyframeArray EndZ => Keyframes[5];
        [Browsable(false)] public KeyframeArray RefDist => Keyframes[6];
        [Browsable(false)] public KeyframeArray RefBright => Keyframes[7];
        [Browsable(false)] public KeyframeArray SpotCut => Keyframes[8];
        [Browsable(false)] public KeyframeArray SpecShininess => Keyframes[9];

        private KeyframeCollection _keyframes;

        [Browsable(false)]
        public KeyframeCollection Keyframes
        {
            get
            {
                if (_keyframes == null)
                {
                    _keyframes =
                        new KeyframeCollection(10, Scene == null ? 1 : Scene.FrameCount + (Scene.Loop ? 1 : 0));
                    if (Data != null && _name != "<null>")
                    {
                        for (int i = 0, index = 0; i < 14; i++)
                        {
                            if (!(i == 3 || i == 7 || i == 10 || i == 12))
                            {
                                DecodeKeyframes(
                                    Keyframes[index],
                                    Data->_startPoint._x.Address + i * 4,
                                    (int) _fixedFlags,
                                    (int) _ordered[index++]);
                            }
                        }
                    }
                }

                return _keyframes;
            }
        }

        [Browsable(false)] public KeyframeArray[] KeyArrays => Keyframes._keyArrays;

        #endregion
    }

    public enum LightKeyframeMode
    {
        StartX,
        StartY,
        StartZ,
        EndX,
        EndY,
        EndZ,
        SpotCut,
        SpotBright,
        RefDist,
        RefBright
    }
}