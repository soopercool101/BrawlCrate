using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class STPMNode : ARCEntryNode
    {
        internal Parameter* Header => (Parameter*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.STPM;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            return Header->_count > 0;
        }

        protected override string GetName()
        {
            return base.GetName("Stage Parameters");
        }

        private const int _entrySize = 260;

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                new STPMEntryNode().Initialize(this, new DataSource((*Header)[i], _entrySize));
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x10 + Children.Count * 4 + Children.Count * _entrySize;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            Parameter* header = (Parameter*) address;
            *header = new Parameter(Parameter.TagSTPM, Children.Count);
            uint offset = (uint) (0x10 + Children.Count * 4);
            for (int i = 0; i < Children.Count; i++)
            {
                ResourceNode r = Children[i];
                *(buint*) (address + 0x10 + i * 4) = offset;
                r.Rebuild(address + offset, _entrySize, true);
                offset += _entrySize;
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((Parameter*) source.Address)->_tag == Parameter.TagSTPM ? new STPMNode() : null;
        }

        public void ReplaceCamera(STPMNode external)
        {
            for (int i = 0; i < Children.Count && i < external.Children.Count; i++)
            {
                if (!(external.Children[i] is STPMEntryNode ext) || !(Children[i] is STPMEntryNode cur))
                {
                    continue;
                }

                // In-game camera
                cur.CameraFOV = ext.CameraFOV;

                cur.MinimumZ = ext.MinimumZ;
                cur.MaximumZ = ext.MaximumZ;

                cur.MinimumTilt = ext.MinimumTilt;
                cur.MaximumTilt = ext.MaximumTilt;

                cur.HorizontalRotationFactor = ext.HorizontalRotationFactor;
                cur.VerticalRotationFactor = ext.VerticalRotationFactor;

                cur.CharacterBubbleBufferMultiplier = ext.CharacterBubbleBufferMultiplier;

                cur.CameraSpeed = ext.CameraSpeed;

                cur.StarKOCamTilt = ext.StarKOCamTilt;
                cur.FinalSmashCamTilt = ext.FinalSmashCamTilt;

                cur.CameraRight = ext.CameraRight;
                cur.CameraLeft = ext.CameraLeft;

                // Pause camera
                cur.PauseCamX = ext.PauseCamX;
                cur.PauseCamY = ext.PauseCamY;
                cur.PauseCamZ = ext.PauseCamZ;
                cur.PauseCamAngle = ext.PauseCamAngle;

                cur.PauseCamZoomIn = ext.PauseCamZoomIn;
                cur.PauseCamZoomOut = ext.PauseCamZoomOut;
                cur.PauseCamZoomDefault = ext.PauseCamZoomDefault;

                cur.PauseCamRotYMin = ext.PauseCamRotYMin;
                cur.PauseCamRotYMax = ext.PauseCamRotYMax;
                cur.PauseCamRotXMin = ext.PauseCamRotXMin;
                cur.PauseCamRotXMax = ext.PauseCamRotXMax;

                // Fixed camera
                cur.FixedCamX = ext.FixedCamX;
                cur.FixedCamY = ext.FixedCamY;
                cur.FixedCamZ = ext.FixedCamZ;
                cur.FixedCamFOV = ext.FixedCamFOV;
                cur.FixedHorizontalAngle = ext.FixedHorizontalAngle;
                cur.FixedVerticalAngle = ext.FixedVerticalAngle;
            }
        }

        public void ReplaceGameplayVariables(STPMNode external)
        {
            for (int i = 0; i < Children.Count && i < external.Children.Count; i++)
            {
                if (!(external.Children[i] is STPMEntryNode ext) || !(Children[i] is STPMEntryNode cur))
                {
                    continue;
                }

                cur.MinimumZ = ext.MinimumZ;
                cur.MaximumZ = ext.MaximumZ;
                cur.MinimumTilt = ext.MinimumTilt;
                cur.MaximumTilt = ext.MaximumTilt;
                cur.HorizontalRotationFactor = ext.HorizontalRotationFactor;
                cur.VerticalRotationFactor = ext.VerticalRotationFactor;
                cur.CharacterBubbleBufferMultiplier = ext.CharacterBubbleBufferMultiplier;
                cur.Value18 = ext.Value18;
                cur.CameraSpeed = ext.CameraSpeed;
                cur.StarKOCamTilt = ext.StarKOCamTilt;
                cur.FinalSmashCamTilt = ext.FinalSmashCamTilt;
                cur.CameraRight = ext.CameraRight;
                cur.CameraLeft = ext.CameraLeft;
                cur.PauseCamX = ext.PauseCamX;
                cur.PauseCamY = ext.PauseCamY;
                cur.PauseCamZ = ext.PauseCamZ;
                cur.PauseCamAngle = ext.PauseCamAngle;
                cur.PauseCamZoomIn = ext.PauseCamZoomIn;
                cur.PauseCamZoomDefault = ext.PauseCamZoomDefault;
                cur.PauseCamZoomOut = ext.PauseCamZoomOut;
                cur.PauseCamRotYMin = ext.PauseCamRotYMin;
                cur.PauseCamRotYMax = ext.PauseCamRotYMax;
                cur.PauseCamRotXMin = ext.PauseCamRotXMin;
                cur.PauseCamRotXMax = ext.PauseCamRotXMax;
                cur.FixedCamX = ext.FixedCamX;
                cur.FixedCamY = ext.FixedCamY;
                cur.FixedCamZ = ext.FixedCamZ;
                cur.FixedCamFOV = ext.FixedCamFOV;
                cur.FixedHorizontalAngle = ext.FixedHorizontalAngle;
                cur.FixedVerticalAngle = ext.FixedVerticalAngle;
                cur.Value41 = ext.Value41;
                cur.OlimarFinalCamAngle = ext.OlimarFinalCamAngle;
                cur.IceClimbersFinalPosX = ext.IceClimbersFinalPosX;
                cur.IceClimbersFinalPosY = ext.IceClimbersFinalPosY;
                cur.IceClimbersFinalPosZ = ext.IceClimbersFinalPosZ;
                cur.IceClimbersFinalScaleX = ext.IceClimbersFinalScaleX;
                cur.IceClimbersFinalScaleY = ext.IceClimbersFinalScaleY;
                cur.PitFinalPalutenaScale = ext.PitFinalPalutenaScale;
                cur.Value49 = ext.Value49;
            }
        }

        public void ReplaceCosmeticVariables(STPMNode external)
        {
            for (int i = 0; i < Children.Count && i < external.Children.Count; i++)
            {
                if (!(external.Children[i] is STPMEntryNode ext) || !(Children[i] is STPMEntryNode cur))
                {
                    continue;
                }

                cur.Id1 = ext.Id1;
                cur.Id2 = ext.Id2;
                cur.Value1 = ext.Value1;
                cur.Value2 = ext.Value2;
                cur.Value3 = ext.Value3;
                cur.Value4 = ext.Value4;
                cur.MusicVolume = ext.MusicVolume;
                cur.SoundVolume = ext.SoundVolume;
                cur.Value5c = ext.Value5c;
                cur.Value5d = ext.Value5d;
                cur.ShadowPitch = ext.ShadowPitch;
                cur.ShadowYaw = ext.ShadowYaw;
                cur.Value8 = ext.Value8;
                cur.Value9 = ext.Value9;
                cur.StageWindEnabled = ext.StageWindEnabled;
                cur.CharacterWindEnabled = ext.CharacterWindEnabled;
                cur.Value50c = ext.Value50c;
                cur.Value50d = ext.Value50d;
                cur.WindStrength = ext.WindStrength;
                cur.HorizontalWindRotation = ext.HorizontalWindRotation;
                cur.VerticalWindRotation = ext.VerticalWindRotation;
                cur.Value54 = ext.Value54;
                cur.Value55 = ext.Value55;
                cur.Value56 = ext.Value56;
                cur.EchoDepth = ext.EchoDepth;
                cur.EchoDepthRemoval = ext.EchoDepthRemoval;
                cur.EchoRepeats1 = ext.EchoRepeats1;
                cur.EchoRepeats2 = ext.EchoRepeats2;
                cur.Value61 = ext.Value61;
                cur.Value62 = ext.Value62;
                cur.Value63 = ext.Value63;
                cur.Value64 = ext.Value64;
            }
        }
    }

    public unsafe class STPMEntryNode : ResourceNode
    {
        internal StageParameterEntry* Header => (StageParameterEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        private byte echo;
        private byte id2;
        private ushort id;

        private ParameterValueManager _values = new ParameterValueManager(null);

        [Category("STPM Data")]
        public byte Echo
        {
            get => echo;
            set
            {
                echo = value;
                SignalPropertyChange();
            }
        }

        [Category("STPM Data")]
        public ushort Id1
        {
            get => id;
            set
            {
                id = value;
                SignalPropertyChange();
            }
        }

        [Category("STPM Data")]
        public byte Id2
        {
            get => id2;
            set
            {
                id2 = value;
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float Value1
        {
            get => _values.GetFloat(0);
            set
            {
                _values.SetFloat(0, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float Value2
        {
            get => _values.GetFloat(1);
            set
            {
                _values.SetFloat(1, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float Value3
        {
            get => _values.GetFloat(2);
            set
            {
                _values.SetFloat(2, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float Value4
        {
            get => _values.GetFloat(3);
            set
            {
                _values.SetFloat(3, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public byte MusicVolume
        {
            get => _values.GetByte(4, 0);
            set
            {
                _values.SetByte(4, 0, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public byte SoundVolume
        {
            get => _values.GetByte(4, 1);
            set
            {
                _values.SetByte(4, 1, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public byte Value5c
        {
            get => _values.GetByte(4, 2);
            set
            {
                _values.SetByte(4, 2, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public byte Value5d
        {
            get => _values.GetByte(4, 3);
            set
            {
                _values.SetByte(4, 3, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float ShadowPitch
        {
            get => _values.GetFloat(5);
            set
            {
                _values.SetFloat(5, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float ShadowYaw
        {
            get => _values.GetFloat(6);
            set
            {
                _values.SetFloat(6, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float Value8
        {
            get => _values.GetFloat(7);
            set
            {
                _values.SetFloat(7, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float Value9
        {
            get => _values.GetFloat(8);
            set
            {
                _values.SetFloat(8, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float CameraFOV
        {
            get => _values.GetFloat(9);
            set
            {
                _values.SetFloat(9, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float MinimumZ
        {
            get => _values.GetFloat(10);
            set
            {
                _values.SetFloat(10, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float MaximumZ
        {
            get => _values.GetFloat(11);
            set
            {
                _values.SetFloat(11, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float MinimumTilt
        {
            get => _values.GetFloat(12);
            set
            {
                _values.SetFloat(12, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float MaximumTilt
        {
            get => _values.GetFloat(13);
            set
            {
                _values.SetFloat(13, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float HorizontalRotationFactor
        {
            get => _values.GetFloat(14);
            set
            {
                _values.SetFloat(14, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float VerticalRotationFactor
        {
            get => _values.GetFloat(15);
            set
            {
                _values.SetFloat(15, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float CharacterBubbleBufferMultiplier
        {
            get => _values.GetFloat(16);
            set
            {
                _values.SetFloat(16, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float Value18
        {
            get => _values.GetFloat(17);
            set
            {
                _values.SetFloat(17, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float CameraSpeed
        {
            get => _values.GetFloat(18);
            set
            {
                _values.SetFloat(18, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float StarKOCamTilt
        {
            get => _values.GetFloat(19);
            set
            {
                _values.SetFloat(19, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float FinalSmashCamTilt
        {
            get => _values.GetFloat(20);
            set
            {
                _values.SetFloat(20, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float CameraRight
        {
            get => _values.GetFloat(21);
            set
            {
                _values.SetFloat(21, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float CameraLeft
        {
            get => _values.GetFloat(22);
            set
            {
                _values.SetFloat(22, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float PauseCamX
        {
            get => _values.GetFloat(23);
            set
            {
                _values.SetFloat(23, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float PauseCamY
        {
            get => _values.GetFloat(24);
            set
            {
                _values.SetFloat(24, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float PauseCamZ
        {
            get => _values.GetFloat(25);
            set
            {
                _values.SetFloat(25, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float PauseCamAngle
        {
            get => _values.GetFloat(26);
            set
            {
                _values.SetFloat(26, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float PauseCamZoomIn
        {
            get => _values.GetFloat(27);
            set
            {
                _values.SetFloat(27, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float PauseCamZoomDefault
        {
            get => _values.GetFloat(28);
            set
            {
                _values.SetFloat(28, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float PauseCamZoomOut
        {
            get => _values.GetFloat(29);
            set
            {
                _values.SetFloat(29, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float PauseCamRotYMin
        {
            get => _values.GetFloat(30);
            set
            {
                _values.SetFloat(30, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float PauseCamRotYMax
        {
            get => _values.GetFloat(31);
            set
            {
                _values.SetFloat(31, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float PauseCamRotXMin
        {
            get => _values.GetFloat(32);
            set
            {
                _values.SetFloat(32, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float PauseCamRotXMax
        {
            get => _values.GetFloat(33);
            set
            {
                _values.SetFloat(33, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float FixedCamX
        {
            get => _values.GetFloat(34);
            set
            {
                _values.SetFloat(34, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float FixedCamY
        {
            get => _values.GetFloat(35);
            set
            {
                _values.SetFloat(35, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float FixedCamZ
        {
            get => _values.GetFloat(36);
            set
            {
                _values.SetFloat(36, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float FixedCamFOV
        {
            get => _values.GetFloat(37);
            set
            {
                _values.SetFloat(37, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float FixedHorizontalAngle
        {
            get => _values.GetFloat(38);
            set
            {
                _values.SetFloat(38, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float FixedVerticalAngle
        {
            get => _values.GetFloat(39);
            set
            {
                _values.SetFloat(39, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float Value41
        {
            get => _values.GetFloat(40);
            set
            {
                _values.SetFloat(40, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float OlimarFinalCamAngle
        {
            get => _values.GetFloat(41);
            set
            {
                _values.SetFloat(41, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float IceClimbersFinalPosX
        {
            get => _values.GetFloat(42);
            set
            {
                _values.SetFloat(42, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float IceClimbersFinalPosY
        {
            get => _values.GetFloat(43);
            set
            {
                _values.SetFloat(43, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float IceClimbersFinalPosZ
        {
            get => _values.GetFloat(44);
            set
            {
                _values.SetFloat(44, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float IceClimbersFinalScaleX
        {
            get => _values.GetFloat(45);
            set
            {
                _values.SetFloat(45, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float IceClimbersFinalScaleY
        {
            get => _values.GetFloat(46);
            set
            {
                _values.SetFloat(46, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float PitFinalPalutenaScale
        {
            get => _values.GetFloat(47);
            set
            {
                _values.SetFloat(47, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float Value49
        {
            get => _values.GetFloat(48);
            set
            {
                _values.SetFloat(48, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public bool StageWindEnabled
        {
            get => _values.GetByte(49, 0) != 0;
            set
            {
                _values.SetByte(49, 0, (byte) (value ? 1 : 0));
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public bool CharacterWindEnabled
        {
            get => _values.GetByte(49, 1) != 0;
            set
            {
                _values.SetByte(49, 1, (byte) (value ? 1 : 0));
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public byte Value50c
        {
            get => _values.GetByte(49, 2);
            set
            {
                _values.SetByte(49, 2, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public byte Value50d
        {
            get => _values.GetByte(49, 3);
            set
            {
                _values.SetByte(49, 3, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float WindStrength
        {
            get => _values.GetFloat(50);
            set
            {
                _values.SetFloat(50, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float HorizontalWindRotation
        {
            get => _values.GetFloat(51);
            set
            {
                _values.SetFloat(51, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float VerticalWindRotation
        {
            get => _values.GetFloat(52);
            set
            {
                _values.SetFloat(52, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public float Value54
        {
            get => _values.GetFloat(53);
            set
            {
                _values.SetFloat(53, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public RGBAPixel Value55
        {
            get => _values.GetRGBA(54);
            set
            {
                _values.SetRGBA(54, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public int Value56
        {
            get => _values.GetInt(55);
            set
            {
                _values.SetInt(55, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public int EchoDepth
        {
            get => _values.GetInt(56);
            set
            {
                _values.SetInt(56, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public int EchoDepthRemoval
        {
            get => _values.GetInt(57);
            set
            {
                _values.SetInt(57, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public int EchoRepeats1
        {
            get => _values.GetInt(58);
            set
            {
                _values.SetInt(58, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public int EchoRepeats2
        {
            get => _values.GetInt(59);
            set
            {
                _values.SetInt(59, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public int Value61
        {
            get => _values.GetInt(60);
            set
            {
                _values.SetInt(60, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public int Value62
        {
            get => _values.GetInt(61);
            set
            {
                _values.SetInt(61, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public int Value63
        {
            get => _values.GetInt(62);
            set
            {
                _values.SetInt(62, value);
                SignalPropertyChange();
            }
        }

        [Category("STPM Values")]
        public int Value64
        {
            get => _values.GetInt(63);
            set
            {
                _values.SetInt(63, value);
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            id = Header->_id;
            echo = Header->_echo;
            id2 = Header->_id2;

            if (_name == null)
            {
                _name = "STPMEntry " + id;
            }

            _values = new ParameterValueManager((VoidPtr) Header + 4);

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            StageParameterEntry* header = (StageParameterEntry*) address;
            *header = new StageParameterEntry(id, echo, id2);
            byte* pOut = (byte*) header + 4;
            byte* pIn = (byte*) _values._values.Address;
            for (int i = 0; i < 64 * 4; i++)
            {
                *pOut++ = *pIn++;
            }
        }
    }
}
