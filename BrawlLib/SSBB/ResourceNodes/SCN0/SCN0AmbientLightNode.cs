using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SCN0AmbientLightNode : SCN0EntryNode, IColorSource
    {
        internal SCN0AmbientLight* Data => (SCN0AmbientLight*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.SCN0Ambient;

        private byte _fixedFlags, _usageFlags = 3;

        [Category("Ambient Light")]
        public bool ColorEnabled
        {
            get => (_usageFlags & 1) != 0;
            set
            {
                if (value)
                {
                    _usageFlags |= 1;
                }
                else
                {
                    _usageFlags &= 2;
                }

                SignalPropertyChange();
            }
        }

        [Category("Ambient Light")]
        public bool AlphaEnabled
        {
            get => (_usageFlags & 2) != 0;
            set
            {
                if (value)
                {
                    _usageFlags |= 2;
                }
                else
                {
                    _usageFlags &= 1;
                }

                SignalPropertyChange();
            }
        }

        public bool _constant = true;

        [Category("Ambient Light")]
        public bool Constant
        {
            get => _constant;
            set
            {
                if (_constant != value)
                {
                    _constant = value;
                    if (_constant)
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

        internal List<RGBAPixel> _colors = new List<RGBAPixel>();

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

        internal RGBAPixel _solidColor;

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
            _constant = true;
            _solidColor = color;
            SignalPropertyChange();
        }

        public void MakeList()
        {
            _constant = false;
            int entries = Scene.FrameCount;
            _numEntries = _colors.Count;
            NumEntries = entries;
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

        [Browsable(false)] public int TypeCount => 1;

        [Browsable(false)]
        public int ColorCount(int id)
        {
            return _numEntries == 0 ? 1 : _numEntries;
        }

        public ARGBPixel GetColor(int index, int id)
        {
            return _numEntries == 0 ? (ARGBPixel) _solidColor : (ARGBPixel) _colors[index];
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
            return Constant;
        }

        public void SetColorConstant(int id, bool constant)
        {
            Constant = constant;
        }

        #endregion

        public Vector4 GetAmbientColorFrame(float index)
        {
            if (Constant)
            {
                return _solidColor;
            }

            int colorIndex = (int) Math.Truncate(index);
            Vector4 color = _colors[colorIndex.Clamp(0, _colors.Count - 1)];
            if (colorIndex + 1 < _colors.Count)
            {
                float frac = index - colorIndex;
                Vector4 interp = _colors[colorIndex + 1];
                color += (interp - color) * frac;
            }

            return color;
        }

        [Browsable(false)] public int FrameCount => Scene.FrameCount;

        internal void SetSize(int numFrames)
        {
            _numEntries = _colors.Count;
            NumEntries = numFrames;
            if (_constant)
            {
                _numEntries = 0;
            }
        }

        public override bool OnInitialize()
        {
            //Read common header
            base.OnInitialize();

            //Set defaults
            _colors = new List<RGBAPixel>();

            //Read header values
            _fixedFlags = Data->_fixedFlags;
            _usageFlags = Data->_flags;

            if (_name == "<null>")
            {
                return false;
            }

            //Read ambient light color
            ReadColors(
                _fixedFlags,
                1 << 7,
                ref _solidColor,
                ref _colors,
                FrameCount,
                Data->_lighting.Address,
                ref _constant,
                ref _numEntries);

            return false;
        }

        private SCN0AmbientLightNode _match;

        public override int OnCalculateSize(bool force)
        {
            for (int i = 0; i < 3; i++)
            {
                _dataLengths[i] = 0;
            }

            _match = null;

            if (_name == "<null>")
            {
                return SCN0AmbientLight.Size;
            }

            _match = FindColorMatch(_constant, FrameCount, 0) as SCN0AmbientLightNode;
            if (_match == null && !_constant)
            {
                _dataLengths[1] += 4 * (FrameCount + 1);
            }

            if (_constant)
            {
                _fixedFlags |= 0x80;
            }
            else
            {
                _fixedFlags &= 0x7F;
            }

            return SCN0AmbientLight.Size;
        }

        private VoidPtr _matchAddr;

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);

            _matchAddr = null;

            SCN0AmbientLight* header = (SCN0AmbientLight*) address;

            header->_pad1 = 0;
            header->_pad2 = 0;

            if (_name == "<null>")
            {
                return;
            }

            int flags = _fixedFlags;
            _dataAddrs[1] += WriteColors(
                ref flags,
                0x80,
                _solidColor,
                _colors,
                _constant,
                FrameCount,
                header->_lighting.Address,
                ref _matchAddr,
                _match == null ? null : _match._matchAddr,
                (RGBAPixel*) _dataAddrs[1]);

            header->_fixedFlags = (byte) flags;
            header->_flags = _usageFlags;
        }
    }
}