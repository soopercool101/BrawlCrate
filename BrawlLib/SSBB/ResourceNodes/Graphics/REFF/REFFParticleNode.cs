using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class REFFParticleNode : ResourceNode
    {
        internal ParticleParameterHeader* Params => (ParticleParameterHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        private ParticleParameterHeader hdr;
        private ParticleParameterDesc desc;

        //[Category("Particle Parameters")]
        //public uint HeaderSize { get { return hdr.headersize; } }

        [Category("Particle Parameters")]
        [TypeConverter(typeof(RGBAStringConverter))]
        public RGBAPixel Color1Primary
        {
            get => desc.mColor11;
            set
            {
                desc.mColor11 = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        [TypeConverter(typeof(RGBAStringConverter))]
        public RGBAPixel Color1Secondary
        {
            get => desc.mColor12;
            set
            {
                desc.mColor12 = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        [TypeConverter(typeof(RGBAStringConverter))]
        public RGBAPixel Color2Primary
        {
            get => desc.mColor21;
            set
            {
                desc.mColor21 = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        [TypeConverter(typeof(RGBAStringConverter))]
        public RGBAPixel Color2Secondary
        {
            get => desc.mColor22;
            set
            {
                desc.mColor22 = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 Size
        {
            get => desc.size;
            set
            {
                desc.size = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 Scale
        {
            get => desc.scale;
            set
            {
                desc.scale = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Rotate
        {
            get => desc.rotate;
            set
            {
                desc.rotate = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 TextureScale1
        {
            get => desc.textureScale1;
            set
            {
                desc.textureScale1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 TextureScale2
        {
            get => desc.textureScale2;
            set
            {
                desc.textureScale2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 TextureScale3
        {
            get => desc.textureScale3;
            set
            {
                desc.textureScale3 = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 TextureRotate
        {
            get => desc.textureRotate;
            set
            {
                desc.textureRotate = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 TextureTranslate1
        {
            get => desc.textureTranslate1;
            set
            {
                desc.textureTranslate1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 TextureTranslate2
        {
            get => desc.textureTranslate2;
            set
            {
                desc.textureTranslate2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 TextureTranslate3
        {
            get => desc.textureTranslate3;
            set
            {
                desc.textureTranslate3 = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        public ushort TextureWrap
        {
            get => desc.textureWrap;
            set
            {
                desc.textureWrap = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        public byte TextureReverse
        {
            get => desc.textureReverse;
            set
            {
                desc.textureReverse = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        public byte AlphaCompareRef0
        {
            get => desc.mACmpRef0;
            set
            {
                desc.mACmpRef0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        public byte AlphaCompareRef1
        {
            get => desc.mACmpRef1;
            set
            {
                desc.mACmpRef1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        public byte RotateOffsetRandom1
        {
            get => desc.rotateOffsetRandomX;
            set
            {
                desc.rotateOffsetRandomX = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        public byte RotateOffsetRandom2
        {
            get => desc.rotateOffsetRandomY;
            set
            {
                desc.rotateOffsetRandomY = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        public byte RotateOffsetRandom3
        {
            get => desc.rotateOffsetRandomZ;
            set
            {
                desc.rotateOffsetRandomZ = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 RotateOffset
        {
            get => desc.rotateOffset;
            set
            {
                desc.rotateOffset = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        public string Texture1Name
        {
            get => _textureNames[0];
            set
            {
                _textureNames[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        public string Texture2Name
        {
            get => _textureNames[1];
            set
            {
                _textureNames[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Particle Parameters")]
        public string Texture3Name
        {
            get => _textureNames[2];
            set
            {
                _textureNames[2] = value;
                SignalPropertyChange();
            }
        }

        public string[] _textureNames = new string[3] {"", "", ""};

        public override bool OnInitialize()
        {
            _name = "Particle";
            hdr = *Params;
            desc = hdr.paramDesc;

            VoidPtr addr = Params->paramDesc.textureNames.Address;
            for (int i = 0; i < 3; i++)
            {
                if (*(bushort*) addr > 1)
                {
                    _textureNames[i] = new string((sbyte*) (addr + 2));
                }
                else
                {
                    _textureNames[i] = null;
                }

                addr += 2 + *(bushort*) addr;
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            int size = 0x8C;
            foreach (string s in _textureNames)
            {
                size += 3;
                if (s != null && s.Length > 0)
                {
                    size += s.Length;
                }
            }

            return size.Align(4);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ParticleParameterHeader* p = (ParticleParameterHeader*) address;
            p->headersize = (uint) length - 4;
            p->paramDesc = desc;
            sbyte* ptr = (sbyte*) p->paramDesc.textureNames.Address;
            foreach (string s in _textureNames)
            {
                if (s != null && s.Length > 0)
                {
                    *(bushort*) ptr = (ushort) (s.Length + 1);
                    ptr += 2;
                    s.Write(ref ptr);
                }
                else
                {
                    *(bushort*) ptr = 1;
                    ptr += 3;
                }
            }
        }
    }
}