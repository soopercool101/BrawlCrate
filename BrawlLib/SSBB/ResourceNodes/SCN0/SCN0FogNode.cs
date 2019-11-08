using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Animations;
using BrawlLib.Wii.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SCN0FogNode : SCN0EntryNode, IColorSource, IKeyframeSource
    {
        internal SCN0Fog* Data => (SCN0Fog*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.SCN0Fog;

        private FogType _type = FogType.PerspectiveLinear;
        public SCN0FogFlags _flags = (SCN0FogFlags) 0xE0;
        public bool _constantColor = true;
        public KeyframeCollection _keyframes;
        internal List<RGBAPixel> _colors = new List<RGBAPixel>();
        internal RGBAPixel _solidColor;

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

        [Browsable(false)] public int TypeCount => 1;

        [Browsable(false)]
        public int ColorCount(int id)
        {
            return _numEntries == 0 ? 1 : _numEntries;
        }

        public ARGBPixel GetColor(int index, int id)
        {
            return _numEntries != 0 && index < _colors.Count ? (ARGBPixel) _colors[index] : (ARGBPixel) _solidColor;
        }

        public void SetColor(int index, int id, ARGBPixel color)
        {
            if (_numEntries == 0)
            {
                _solidColor = color;
            }
            else
            {
                _colors[index] = color;
            }

            SignalPropertyChange();
        }

        public bool GetColorConstant(int id)
        {
            return ConstantColor;
        }

        public void SetColorConstant(int id, bool constant)
        {
            ConstantColor = constant;
        }

        #endregion

        [Category("Fog")]
        public bool ConstantColor
        {
            get => _constantColor;
            set
            {
                if (_constantColor != value)
                {
                    _constantColor = value;
                    if (_constantColor)
                    {
                        MakeSolid(new RGBAPixel());
                    }
                    else
                    {
                        MakeList();
                    }

                    UpdateCurrentControl();
                }
            }
        }

        [Category("Fog")]
        public FogType Type
        {
            get => _type;
            set
            {
                _type = value;
                SignalPropertyChange();
            }
        }

        [Browsable(false)] public int FrameCount => Keyframes.FrameLimit;

        [Browsable(false)]
        public KeyframeCollection Keyframes
        {
            get
            {
                if (_keyframes == null)
                {
                    _keyframes = new KeyframeCollection(2, Scene == null ? 1 : Scene.FrameCount + (Scene.Loop ? 1 : 0));
                    if (Data != null && _name != "<null>")
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            DecodeKeyframes(
                                KeyArrays[i],
                                Data->_start.Address + i * 4,
                                (int) _flags,
                                (int) SCN0FogFlags.FixedStart + i * 0x20);
                        }
                    }
                }

                return _keyframes;
            }
        }

        [Browsable(false)] public KeyframeArray[] KeyArrays => Keyframes._keyArrays;

        [Browsable(false)]
        public List<RGBAPixel> Colors
        {
            get => _colors;
            set
            {
                _colors = value;
                SignalPropertyChange();
            }
        }

        [Browsable(false)]
        public RGBAPixel SolidColor
        {
            get => _solidColor;
            set
            {
                _solidColor = value;
                SignalPropertyChange();
            }
        }

        internal int _numEntries;

        [Browsable(false)]
        internal int NumEntries
        {
            get => _numEntries;
            set
            {
                //if (_numEntries == 0)
                //    return;

                if (value > _numEntries)
                {
                    RGBAPixel p = _numEntries > 0 ? _colors[_numEntries - 1] : new RGBAPixel(0, 0, 0, 255);
                    for (int i = value - _numEntries; i-- > 0;)
                    {
                        _colors.Add(p);
                    }
                }
                else if (value < _colors.Count)
                {
                    _colors.RemoveRange(value, _colors.Count - value);
                }

                _numEntries = value;
            }
        }

        public void MakeSolid(RGBAPixel color)
        {
            _numEntries = 0;
            _constantColor = true;
            _solidColor = color;
            SignalPropertyChange();
        }

        public void MakeList()
        {
            _constantColor = false;
            int entries = Scene.FrameCount;
            _numEntries = _colors.Count;
            NumEntries = entries;
        }

        public override bool OnInitialize()
        {
            //Read common header
            base.OnInitialize();

            //Set defaults
            _colors = new List<RGBAPixel>();

            //Read header values
            _flags = (SCN0FogFlags) Data->_flags;
            _type = (FogType) (int) Data->_type;

            //Don't bother reading data if the entry is null
            if (_name == "<null>")
            {
                return false;
            }

            //Read fog color
            ReadColors(
                (uint) _flags,
                (uint) SCN0FogFlags.FixedColor,
                ref _solidColor,
                ref _colors,
                FrameCount,
                Data->_color.Address,
                ref _constantColor,
                ref _numEntries);

            return false;
        }

        private SCN0FogNode _match;

        public override int OnCalculateSize(bool force)
        {
            //If a previous fog node has the same exact color array as this one,
            //both offsets will point to only the first color array.
            _match = null;

            //Reset data lengths
            for (int i = 0; i < 3; i++)
            {
                _dataLengths[i] = 0;
            }

            //Null nodes are only empty headers. No data.
            if (_name == "<null>")
            {
                return SCN0Fog.Size;
            }

            //Add keyframe array sizes
            for (int i = 0; i < 2; i++)
            {
                CalcKeyLen(KeyArrays[i]);
            }

            _match = FindColorMatch(_constantColor, FrameCount, 0) as SCN0FogNode;
            if (_match == null && !_constantColor)
            {
                _dataLengths[1] += 4 * (FrameCount + 1);
            }

            return SCN0Fog.Size;
        }

        private VoidPtr _matchAddr;

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);

            _matchAddr = null;

            if (_name == "<null>")
            {
                return;
            }

            SCN0Fog* header = (SCN0Fog*) address;

            int flags = 0;

            for (int i = 0; i < 2; i++)
            {
                _dataAddrs[0] += EncodeKeyframes(
                    KeyArrays[i],
                    _dataAddrs[0],
                    header->_start.Address + i * 4,
                    ref flags,
                    (int) SCN0FogFlags.FixedStart + i * 0x20);
            }

            _dataAddrs[1] += WriteColors(
                ref flags,
                (int) SCN0FogFlags.FixedColor,
                _solidColor,
                _colors,
                _constantColor,
                FrameCount,
                header->_color.Address,
                ref _matchAddr,
                _match == null ? null : _match._matchAddr,
                (RGBAPixel*) _dataAddrs[1]);

            _flags = (SCN0FogFlags) flags;
            header->_flags = (byte) flags;
            header->_type = (int) _type;
        }

        protected internal override void PostProcess(VoidPtr scn0Address, VoidPtr dataAddress, StringTable stringTable)
        {
            base.PostProcess(scn0Address, dataAddress, stringTable);
        }

        public static bool _generateTangents = true;

        public FogAnimationFrame GetAnimFrame(float index)
        {
            FogAnimationFrame frame;
            float* dPtr = (float*) &frame;
            for (int x = 0; x < 2; x++)
            {
                KeyframeArray a = KeyArrays[x];
                *dPtr++ = a.GetFrameValue(index);
                frame.SetBools(x, a.GetKeyframe((int) index) != null);
                frame.Index = index;
            }

            //Ignore alpha value; not used
            if (ConstantColor)
            {
                frame.Color = _solidColor;
            }
            else
            {
                int colorIndex = (int) Math.Truncate(index);
                Vector3 color = _colors[colorIndex];
                if (colorIndex + 1 < _colors.Count)
                {
                    float frac = index - colorIndex;
                    Vector3 interp = _colors[colorIndex + 1];
                    color += (interp - color) * frac;
                }

                frame.Color = color;
            }

            frame.Type = Type;
            return frame;
        }

        public KeyframeEntry GetKeyframe(int keyFrameMode, int index)
        {
            return KeyArrays[keyFrameMode].GetKeyframe(index);
        }

        public void RemoveKeyframe(int keyFrameMode, int index)
        {
            KeyframeEntry k = KeyArrays[keyFrameMode].Remove(index);
            if (k != null && _generateTangents)
            {
                k._prev.GenerateTangent();
                k._next.GenerateTangent();
                SignalPropertyChange();
            }
        }

        public void SetKeyframe(int keyFrameMode, int index, float value)
        {
            KeyframeArray keys = KeyArrays[keyFrameMode];
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

        internal void SetSize(int numFrames, bool looped)
        {
            _numEntries = _colors.Count;
            NumEntries = numFrames;
            if (_constantColor)
            {
                _numEntries = 0;
            }

            Keyframes.FrameLimit = numFrames + (looped ? 1 : 0);
        }
    }
}